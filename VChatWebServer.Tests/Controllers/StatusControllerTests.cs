using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using VChatWebServer.Tests.Integration;
using Xunit;

namespace VChatWebServer.Tests.Controllers
{
    public class StatusControllerTests
    {
        // 目的：验证状态接口返回 200 且包含 status=ok 的 JSON。
        [Fact]
        public async Task Get_ReturnsOkWithStatus()
        {
            // 行为：请求 /api/status
            var client = TestClientFactory.CreateClient();
            var resp = await client.GetAsync("/api/status");
            // 断言：返回 200 并解析 JSON 校验字段
            Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
            var json = await resp.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            Assert.Equal("ok", root.GetProperty("status").GetString());
        }
    }
}
