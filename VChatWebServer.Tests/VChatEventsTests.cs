using System;
using System.Reflection;
using VChatWebServer;
using VChatWebServer.Services;
using Xunit;

namespace VChatWebServer.Tests
{
    public class VChatEventsTests
    {
        // 目的：验证接收客户端消息事件 RecvClientMsg 是否按预期触发，
        // 并且能够携带正确的连接标识 Guid 与消息文本字符串供外部订阅者使用。
        [Fact]
        public void RecvClientMsg_Fires_On_InternalNotify()
        {
            // 准备：订阅事件并在回调中捕获传入的参数
            Guid? cid = null;
            string? msg = null;
            Action<Guid, string> handler2 = (id, m) => { cid = id; msg = m; };
            VChatWebServer.RecvClientMsg += handler2;

            // 行为：通过反射调用内部触发方法，模拟服务器接收到客户端消息的场景
            // 注意：反射方法名区分大小写，这里使用实际定义的 "NotifyRecvClientMsg"
            var method = typeof(VChatWebServer).GetMethod("NotifyRecvClientMsg", BindingFlags.NonPublic | BindingFlags.Static);
            var id = Guid.NewGuid();
            method!.Invoke(null, new object[] { id, "client-msg" });

            // 断言：外部订阅者应获得与触发时一致的参数值
            Assert.Equal(id, cid);
            Assert.Equal("client-msg", msg);

            // 清理：取消订阅，避免对其他测试造成污染
            VChatWebServer.RecvClientMsg -= handler2;
        }
    }
}
