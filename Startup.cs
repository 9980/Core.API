using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Core.API.RelyExtension;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using static Core.API.RelyExtension.SwaggerRely;

namespace Core.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.此方法由运行时调用。使用此方法将服务添加到容器中。
        //IServiceCollection 指定服务描述符集合的契约。
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(us =>
            {
                typeof(ApiVersions).GetEnumNames().OrderByDescending(ob => ob).ToList().ForEach(version => {
                    //SwaggerEndpoint 添加Swagger JSON端点。可以是完全限定的还是相对于UI页面的
                    us.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{version}");
                });
                us.RoutePrefix = "";
                us.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("Core.API.index.html");
            });
            #endregion
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
