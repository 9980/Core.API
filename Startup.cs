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

        // This method gets called by the runtime. Use this method to add services to the container.�˷���������ʱ���á�ʹ�ô˷�����������ӵ������С�
        //IServiceCollection ָ���������������ϵ���Լ��
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
                    //SwaggerEndpoint ���Swagger JSON�˵㡣��������ȫ�޶��Ļ��������UIҳ���
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
