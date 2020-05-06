using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Core.API.RelyExtension
{
    public static class SwaggerRely
    {
        /// <summary>
        /// Api接口版本 自定义
        /// </summary>
        public enum ApiVersions
        {

            /// <summary>
            /// V1 版本
            /// </summary>
            V1 = 1,
            /// <summary>
            /// V2 版本
            /// </summary>
            V2 = 2,
        }
        public static void AddSwagger(this IServiceCollection servers)
        {
            //nameof用于获取变量、类型或成员的简单（非限定）字符串名称。可以在错误消息中使用类型或成员的非限定字符串名称，而无需对字符串进行硬编码，这样也方便重构。
            if (servers == null) throw new ArgumentNullException(nameof(servers));
            var basePath = AppContext.BaseDirectory;//获取项目地址
            servers.AddSwaggerGen(s =>
            {
                typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
                {
                    //SwaggerDoc 定义一个或多个由Swagger生成器创建的文档
                    //OpenApiInfo 开放API信息对象，它提供关于开放API的元数据。
                    s.SwaggerDoc(version, new OpenApiInfo
                    {
                        Version = version,
                        Title = "学习接口文档"
                    });
                    //OrderActionsBy 在将操作转换为操作之前，为它们提供排序的自定义策略
                    //RelativePath 为此获取或设置相对url路径模板(相对于应用程序根目录)
                    s.OrderActionsBy(o => o.RelativePath);
                });
                //Path.Combine 合并地址
                var xmlPath = Path.Combine(basePath, "Core.API.XML");//创建的项目xml地址和名字
                //IncludeXmlComments 为基于操作、参数和模式的操作注入对人友好的描述
                s.IncludeXmlComments(xmlPath, true);                
                s.OperationFilter<AddResponseHeadersFilter>();//开启加权锁
                s.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();                
                s.OperationFilter<SecurityRequirementsOperationFilter>();// 在header中添加token，传递到后台
            });
        }
    }
}
