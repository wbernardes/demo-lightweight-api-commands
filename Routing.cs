
using System;
using System.Net;
using DemoLightweightApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DemoLightweightApi
{
    public static class Routing
    {
        public static void ConfigureRouting(this IApplicationBuilder app, string routeTemplate)
        {
            var routeBuilder = new RouteBuilder(app);
            routeBuilder.MapRoute(routeTemplate, new RequestDelegate(async context =>
            {
                try
                {
                    var action = context.GetRouteData().Values["action"].ToString();
                    var command = context.ReadAsCommand(action);
                    var mediator = (IMediator)app.ApplicationServices.GetService(typeof(IMediator));
                    var response = mediator.Send(command);
                    await context.Response.WriteJson(response);
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    await context.Response.WriteAsync(ex.Message);
                }
            }));

            var routes = routeBuilder.Build();
            app.UseRouter(routes);
        }
    }
}