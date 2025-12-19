using System.Net;
using System.Threading.Tasks;
using VChatWebServer.Tests.Integration;
using Xunit;

namespace VChatWebServer.Tests.Controllers
{
    public class WebSocketControllerTests
    {
        // 目的：验证当请求不是 WebSocket 握手时，控制器返回 400 BadRequest。
        [Fact]
        public async Task Get_NonWebSocketRequest_Returns400()
        {
            // 准备：使用集成测试客户端直接发起普通 HTTP 请求
            var client = TestClientFactory.CreateClient();
            var resp = await client.GetAsync("/ws");
            // 断言：状态码应为 400
            Assert.Equal(HttpStatusCode.BadRequest, resp.StatusCode);
        }
    }
}
