using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Communication;
using Azure.Messaging;
using Azure.Messaging.EventGrid;
using Azure.Messaging.EventGrid.SystemEvents;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Logging;
using Azure.Storage.Blobs;
using Azure.Communication.Identity;

namespace AdaniCall
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddSession();
            services.AddMemoryCache();
            services.AddHttpContextAccessor();
            services.AddSingleton<IConfiguration>(Configuration);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() )
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            //app.UseMvc();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "landing",
                   template: "s/{id}",
                   defaults: new { controller = "Users", action = "Index" }
               );
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=User}/{action=Login}/{id?}");
            });

            if (env.IsDevelopment() || env.IsProduction())
            {
                
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
                DefaultContentType = "text/plain"
            });

            app.UseDefaultFiles();
        }
    }
}
