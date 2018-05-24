using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Data;
using IOService.DiscService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
                options.UseMySql(Configuration.GetConnectionString("MySqlConnection")));

            services.AddUnitOfWork<IOTCDbcontext>();
            //services.AddScoped(typeof(ISellerService), typeof(SellerService));

            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

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
            //containerBuilder.RegisterType<SellerService>().As<ISellerService>();
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

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi");
            });

            app.UseMvc();
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
