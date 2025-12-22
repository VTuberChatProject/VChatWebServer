using System;
using System.Reflection;
using System.Threading.Tasks;
using VChatWebServer.Services;
using Xunit;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;

namespace VChatWebServer.Tests.Services
{
    public class WebSocketManagerServiceTests
    {
        // 目的：验证连接管理的增删行为是否正确影响内部连接集合计数。
        [Fact]
        public void AddAndRemoveSocket_Changes_InternalCount()
        {
            // 准备：创建服务与一个 WebSocket 实例，添加到集合
            var svc = new WebSocketManagerService();
            var ws = new System.Net.WebSockets.ClientWebSocket();
            var id = svc.AddSocket(ws);
            var field = typeof(WebSocketManagerService).GetField("_sockets", BindingFlags.NonPublic | BindingFlags.Instance);
            var dict = (System.Collections.IDictionary)field!.GetValue(svc)!;
            // 断言：添加后计数为 1
            Assert.Equal(1, dict.Count);
            // 行为：移除该连接
            svc.RemoveSocket(id);
            // 断言：移除后计数为 0
            Assert.Equal(0, dict.Count);
        }

        // 目的：验证在没有任何连接的情况下进行广播不会抛出异常。
        [Fact]
        public async Task BroadcastAsync_Does_Not_Throw_When_No_Sockets()
        {
            // 准备：空连接集合
            var svc = new WebSocketManagerService();
            // 行为与断言：不应抛出异常
            await svc.BroadcastAsync("hello");
        }

        private class FakeWebSocket : WebSocket
        {
            public List<string> Sent { get; } = new();
            public override WebSocketCloseStatus? CloseStatus => null;
            public override string? CloseStatusDescription => null;
            public override WebSocketState State => WebSocketState.Open;
            public override string? SubProtocol => null;
            public override void Abort() { }
            public override void Dispose() { }
            public override Task CloseAsync(WebSocketCloseStatus closeStatus, string? statusDescription, CancellationToken cancellationToken) => Task.CompletedTask;
            public override Task CloseOutputAsync(WebSocketCloseStatus closeStatus, string? statusDescription, CancellationToken cancellationToken) => Task.CompletedTask;
            public override Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken) => Task.FromResult(new WebSocketReceiveResult(0, WebSocketMessageType.Text, true));
            public override Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
            {
                var s = System.Text.Encoding.UTF8.GetString(buffer.Array!, buffer.Offset, buffer.Count);
                Sent.Add(s);
                return Task.CompletedTask;
            }
        }

        [Fact]
        public async Task BroadcastAsync_Excludes_Sender()
        {
            var svc = new WebSocketManagerService();
            var ws1 = new FakeWebSocket();
            var ws2 = new FakeWebSocket();
            var id1 = svc.AddSocket(ws1);
            var id2 = svc.AddSocket(ws2);
            await svc.BroadcastAsync("hello", id1);
            Assert.Empty(ws1.Sent);
            Assert.Single(ws2.Sent);
            Assert.Equal("hello", ws2.Sent[0]);
        }
    }
}
