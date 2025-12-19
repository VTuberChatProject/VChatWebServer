using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace VChatWebServer.Interfaces
{
    /// <summary>
    /// 定义 WebSocket 会话处理的接口。
    /// </summary>
    public interface IWebSocketHandler
    {
        /// <summary>
        /// 处理指定 WebSocket 连接的会话生命周期。
        /// </summary>
        /// <param name="socket">客户端 WebSocket 连接。</param>
        /// <returns>表示处理过程的异步任务。</returns>
        Task HandleAsync(WebSocket socket);
    }
}
