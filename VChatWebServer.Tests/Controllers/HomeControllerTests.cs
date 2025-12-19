using System.Net;
using System.Threading.Tasks;
using VChatWebServer.Tests.Integration;
using Xunit;

namespace VChatWebServer.Tests.Controllers
{
    public class HomeControllerTests
    {
        // 目的：验证根路径返回运行中的提示文本。
        [Fact]
        public async Task Root_ReturnsRunningMessage()
        {
            // 行为：请求根路径
            var client = TestClientFactory.CreateClient();
            var resp = await client.GetAsync("/");
            // 断言：返回 200 且包含指定文案
            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
            var text = await resp.Content.ReadAsStringAsync();
            Assert.Contains("VChat Web Server is Running", text);
        }
    }
}
