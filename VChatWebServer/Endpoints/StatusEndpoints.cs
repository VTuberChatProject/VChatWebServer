using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;

namespace VChatWebServer.Endpoints
{
    public static class StatusEndpoints
    {
        public static IEndpointRouteBuilder MapStatusEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/api/status", () => Results.Json(new { status = "ok", server = "VChatWebServer", time = DateTime.UtcNow }));
            return routes;
        }
    }
}
