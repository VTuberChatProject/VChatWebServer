using VChatWebServer.Interfaces;
using VChatWebServer;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using VChatDanmakuAPIBuilder.Models.Messages;
using System.Text.Json;

namespace VChatWebServer.Services
{
    /// <summary>
    /// 处理单个 WebSocket 会话的收发与清理。
    /// </summary>
    /// <remarks>
    /// 初始化处理器。
    /// </remarks>
    /// <param name="manager">连接管理服务实例。</param>
    class VChatWebsocketHandler(WebSocketManagerService manager) : IWebSocketHandler
    {

        private readonly WebSocketManagerService _manager = manager;

        /// <summary>
        /// 处理 WebSocket 接收循环，并在关闭时完成清理。
        /// </summary>
        /// <param name="socket">当前客户端的 WebSocket 连接。</param>
        /// <returns>表示处理过程的异步任务。</returns>
        public async Task HandleAsync(WebSocket socket)
        {
            var id = _manager.AddSocket(socket);
            var buffer = new byte[1024 * 4];
            try
            {
                while (socket.State == WebSocketState.Open)
                {
                    var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close)
                        break;

                    var receivedText = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    VChatWebServer.NotifyRecvClientMsg(id, receivedText);
                    Console.WriteLine($"收到消息: {receivedText}");
                    // 心跳维持
                    // 解析收到的消息
                    MessageBase? message = JsonSerializer.Deserialize<MessageBase>(receivedText);
                    if (message != null && message.Target == "@vchat_danmaku" && message.Action == "ping")
                    {
                        // 发送pong响应
                        var pongMessage = new PongMessage
                        {
                            Target = "@vchat_danmaku",
                            Action = "pong",
                            From = new VChatDanmakuAPIBuilder.Models.Common.FromInfo
                            {
                                Name = Guid.NewGuid().ToString(),
                                Type = "server",
                                Uuid = Guid.NewGuid().ToString(),
                                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                            },
                        };
                        var pongJson = JsonSerializer.Serialize(pongMessage);
                        var pongBytes = Encoding.UTF8.GetBytes(pongJson);
                        await socket.SendAsync(new ArraySegment<byte>(pongBytes), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                }
            }
            finally
            {
                _manager.RemoveSocket(id);
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "连接关闭", CancellationToken.None);
            }
        }
    }
}
