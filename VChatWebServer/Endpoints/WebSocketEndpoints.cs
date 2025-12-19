using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Net.WebSockets;
using System.Threading.Tasks;
using VChatWebServer.Services;

namespace VChatWebServer.Endpoints
{
    public static class WebSocketEndpoints
    {
        public static IEndpointRouteBuilder MapWebSocketEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/ws", async (HttpContext context, WebSocketManagerService manager) =>
            {
                if (!context.WebSockets.IsWebSocketRequest)
                {
                    context.Response.StatusCode = 400;
                    return;
                }
                var socket = await context.WebSockets.AcceptWebSocketAsync();
                var wsHandler = new VChatWebsocketHandler(manager);
                await wsHandler.HandleAsync(socket);
            });
            return routes;
        }
    }
}
