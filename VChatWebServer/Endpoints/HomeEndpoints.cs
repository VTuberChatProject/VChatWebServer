using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Text;

namespace VChatWebServer.Endpoints
{
    public static class HomeEndpoints
    {
        public static IEndpointRouteBuilder MapHomeEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/", () => Results.Text("VChat Web Server is Running.....", "text/plain", Encoding.UTF8));
            return routes;
        }
    }
}
