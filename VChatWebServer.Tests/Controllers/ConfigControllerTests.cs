using System.Net;
using System.Threading.Tasks;
using VChatWebServer.Tests.Integration;
using Xunit;

namespace VChatWebServer.Tests.Controllers
{
    public class ConfigControllerTests
    {
        // 目的：验证当静态资源目录中缺失 echo-live/config.js 文件时，控制器返回 404。
        [Fact]
        public async Task Get_WhenFileMissing_ReturnsNotFound()
        {
            // 准备：默认测试服务器使用空的 assets 目录
            var client = TestClientFactory.CreateClient();
            var resp = await client.GetAsync("/echo-live/config.js");
            // 断言：应返回 NotFound
            Assert.Equal(HttpStatusCode.NotFound, resp.StatusCode);
        }
    }
}
