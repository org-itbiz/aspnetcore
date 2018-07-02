using System;
using System.IO;
using ApiInsights;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Data;
using IOService.DiscService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using NLog.Config;
using Swashbuckle.AspNetCore.Swagger;
using web_api.Controllers;

namespace web_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IOTCDbcontext>(options =>
                options.UseMySql(Configuration.GetConnectionString("read_conn")));

            services.AddUnitOfWork<IOTCDbcontext>();

            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            //services.AddApiInsights();

            services.AddMvc()
                .AddJsonOptions(options => {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Web Api"
                });
                //var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                //var xmlPath = Path.Combine(basePath, "web-api.xml");
                //c.IncludeXmlComments(xmlPath);
            });

            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<DefaultModule>();
            //
            containerBuilder.RegisterType<ValuesController>().PropertiesAutowired();
            //containerBuilder.RegisterTypes(feature.Controllers.Select(ti => ti.AsType()).ToArray()).PropertiesAutowired();
            containerBuilder.Populate(services);

            var container = containerBuilder.Build();
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            using (StreamReader apacheModRewriteStreamReader = File.OpenText("ApacheModRewrite.txt"))
            using (StreamReader iisUrlRewriteStreamReader = File.OpenText("IISUrlRewrite.xml"))
            {
                var options = new RewriteOptions()
                    .AddRedirect("redirect-rule/(.*)", "redirected/$1")
                    .AddRewrite(@"^rewrite-rule/(\d+)/(\d+)", "rewritten?var1=$1&var2=$2",
                        skipRemainingRules: true)
                    //.AddApacheModRewrite(apacheModRewriteStreamReader)
                    //.AddIISUrlRewrite(iisUrlRewriteStreamReader)
                    //.Add(MethodRules.RedirectXMLRequests)
                    .Add(new RedirectImageRequests(".png", "/png-images"))
                    .Add(new RedirectImageRequests(".jpg", "/jpg-images"));

                app.UseRewriter(options);
            }

            //app.Run(context => context.Response.WriteAsync(
            //    $"Rewritten or Redirected Url: " +
            //    $"{context.Request.Path + context.Request.QueryString}"));

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "web api");
            });

            app.UseMvc();
            app.UseGlobalHttpContext();

            //LogManager.Configuration = new XmlLoggingConfiguration(Path.Combine(env.ContentRootPath, "NLog.config"));
            //LogManager.Configuration.Variables["root"] = env.ContentRootPath;
            //LogManager.Configuration.Variables["connectionString"] = Configuration.GetConnectionString("logger_conn");

        }

        private class DefaultModule : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterType<SellerService>().As<ISellerService>();

            }
        }
    }
}
