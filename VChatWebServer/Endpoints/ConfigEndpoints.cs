using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.IO;
using System.Text;

namespace VChatWebServer.Endpoints
{
    public static class ConfigEndpoints
    {
        public static IEndpointRouteBuilder MapConfigEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/echo-live/config.js", async (IWebHostEnvironment env) =>
            {
                var filePath = Path.Combine(env.WebRootPath, "echo-live/config.js");
                if (!File.Exists(filePath))
                {
                    return Results.NotFound("File not found");
                }
                var jsContent = await File.ReadAllTextAsync(filePath);
                jsContent += @"
 
 function InjectConfig() {
     const protocol = window.location.protocol === 'https:' ? 'wss:' : 'ws:';
     const hostname = window.location.hostname;
     const port = window.location.port;
     const wsAddress = `${protocol}//${hostname}:${port}/ws`;
     config.echolive.broadcast.enable = true;
     config.echolive.broadcast.websocket_enable = true;
     config.echolive.broadcast.websocket_url = wsAddress;
     config.editor.websocket.enable = true;
     config.editor.websocket.url = wsAddress;
     config.editor.websocket.auto_url = false;
 }
 InjectConfig();";
                return Results.Text(jsContent, "application/javascript", Encoding.UTF8);
            });
            return routes;
        }
    }
}
