using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace VChatWebServer.Services
{
    /// <summary>
    /// 管理 WebSocket 连接并提供广播能力的服务。
    /// </summary>
    public class WebSocketManagerService
    {
        private readonly ConcurrentDictionary<Guid, WebSocket> _sockets = new();
        /// <summary>
        /// 注册新的 WebSocket 连接并返回其唯一标识。
        /// </summary>
        /// <param name="socket">待注册的 WebSocket 实例。</param>
        /// <returns>连接的 <see cref="Guid"/> 标识。</returns>
        public Guid AddSocket(WebSocket socket)
        {
            var id = Guid.NewGuid();
            _sockets.TryAdd(id, socket);
            return id;
        }
        /// <summary>
        /// 移除指定标识对应的 WebSocket 连接。
        /// </summary>
        /// <param name="id">连接的唯一标识。</param>
        public void RemoveSocket(Guid id)
        {
            _sockets.TryRemove(id, out _);
        }
        /// <summary>
        /// 向所有处于打开状态的连接广播文本消息。
        /// </summary>
        /// <param name="message">要广播的文本消息。</param>
        /// <returns>表示广播过程的异步任务。</returns>
        public async Task BroadcastAsync(string message)
        {
            var buffer = System.Text.Encoding.UTF8.GetBytes(message);
            var segment = new ArraySegment<byte>(buffer);

            foreach (var socket in _sockets.Values)
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}
