using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.Text;
using VChatWebServer.Services;
using VChatWebServer.Endpoints;

namespace VChatWebServer
{
    /// <summary>
    /// 项目入口类，负责启动/停止 Web 服务器并进行消息广播。
    /// </summary>
    public class VChatWebServer
    {
        private static WebApplication? _app;
        private static WebSocketManagerService? _wsService;
        public static event Action<Guid, string>? RecvClientMsg;

        /// <summary>
        /// 启动 Web 服务器，配置静态文件与 WebSocket 端点。
        /// </summary>
        /// <param name="port">服务器监听的端口号。</param>
        public static void StartWebServer(int port)
        {
            string rootPath = Directory.GetCurrentDirectory();
            string folderPath = @"\assets";
            if (!Directory.Exists(rootPath + folderPath))
            {
                try
                {
                    // 创建文件夹
                    Directory.CreateDirectory(rootPath + folderPath);
                    Console.WriteLine("文件夹已成功创建: " + rootPath + folderPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("创建文件夹时发生错误: " + ex.Message);
                }
            }
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                WebRootPath = rootPath + folderPath
            });
            builder.WebHost.UseSetting(WebHostDefaults.PreventHostingStartupKey, "true");
            builder.Services.AddSingleton<WebSocketManagerService>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            _app = builder.Build();
            _app.UseWebSockets();
            //_app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(rootPath + folderPath),
            //    RequestPath = "/"
            //});
            _app.UseStaticFiles();
            _app.UseSwagger();
            _app.UseSwaggerUI();
            _wsService = _app.Services.GetRequiredService<WebSocketManagerService>();

            _app.MapHomeEndpoints();
            _app.MapStatusEndpoints();
            _app.MapConfigEndpoints();
            _app.MapWebSocketEndpoints();
            //_app.UseRequestLocalization(localizationOptions);
            //_app.MapGet("/hello", () => "Hello, HTTP!");
            _app.Run($"http://0.0.0.0:{port}");
        }

        /// <summary>
        /// 停止 Web 服务器并释放相关资源。
        /// </summary>
        public static void StopWebServer()
        {
            Console.WriteLine("Stopping server...");
            _app?.StopAsync().GetAwaiter().GetResult();
            _app?.DisposeAsync().GetAwaiter().GetResult();
            _app = null;
            Console.WriteLine("Server stopped.");
        }

        /// <summary>
        /// 通过 WebSocket 广播 JSON 文本消息。
        /// </summary>
        /// <param name="jsonstr">要广播的 JSON 字符串。</param>
        public static void SendJsonMsg(string jsonstr)
        {
            if (_wsService == null)
            {
                return;
            }

            // Fire-and-forget
            _ = _wsService.BroadcastAsync(jsonstr);
        }

        internal static void NotifyRecvClientMsg(Guid id, string msg)
        {
            RecvClientMsg?.Invoke(id, msg);
        }
    }
}
