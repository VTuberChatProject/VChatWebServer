using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Net.WebSockets;
using System;
using System.IO;
using System.Threading;
using VChatWebServer.Services;
using VChatWebServer.Endpoints;

namespace VChatWebServer.Tests.Integration
{
    public static class TestClientFactory
    {
        public static HttpClient CreateClient()
        {
            var builder = new WebHostBuilder()
                .UseWebRoot("assets")
                .ConfigureServices(services =>
                {
                    services.AddSingleton<WebSocketManagerService>();
                    services.AddEndpointsApiExplorer();
                    services.AddRouting();
                    services.AddSwaggerGen();
                })
                .Configure(app =>
                {
                    app.UseWebSockets();
                    app.UseStaticFiles();
                    app.UseSwagger();
                    app.UseSwaggerUI();
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapHomeEndpoints();
                        endpoints.MapStatusEndpoints();
                        endpoints.MapConfigEndpoints();
                        endpoints.MapWebSocketEndpoints();
                    });
                });

            var server = new TestServer(builder);
            return server.CreateClient();
        }
    }
}
