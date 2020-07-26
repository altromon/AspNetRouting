using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetRouting
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });

                //Routing with parameters. Parameter can be optional by adding ending ?
                endpoints.MapGet("/hello/{name}", async ctx =>
                {
                    var name = ctx.GetRouteValue("name").ToString();
                    await ctx.Response.WriteAsync($"Hello {name}!");
                });

                //Routing with parameters at different levels
                endpoints.MapGet("/catalog/{family}/{subfamily}/{id}", async ctx =>
                {
                    var family = ctx.GetRouteValue("family").ToString();
                    var subfamily = ctx.GetRouteValue("subfamily").ToString();
                    var id = ctx.GetRouteValue("id").ToString();
                    await ctx.Response.WriteAsync(
                        $"You are looking for {id} in {family}>{subfamily}"
                    );
                });

                //Routing with parameters using default values
                endpoints.MapGet("/catalog/browse/{currentPage=1}", async ctx =>
                {
                    var page = ctx.GetRouteValue("currentPage").ToString();
                    await ctx.Response.WriteAsync($"Browsing products, page {page}");
                });

                //Routing catching all the remaining route from catalog but the query parameters
                //EXAMPLE: /catalog/it/seems/it/is/raining?when=now
                endpoints.MapGet("/catalog/{*data}", async ctx =>
                {
                    var data = ctx.GetRouteValue("data").ToString();
                    await ctx.Response.WriteAsync(data);
                });

                //Only process requests whose id is int
                endpoints.MapGet("/product/edit/{id:int}", async ctx =>
                {
                    var id = ctx.GetRouteValue("id").ToString();
                    await ctx.Response.WriteAsync($"Editing product {id}");
                });
            });
        }
    }
}
