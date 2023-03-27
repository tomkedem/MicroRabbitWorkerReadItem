using MediatR;

using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.IoC;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Domain.EventHandlers;      
using MicroRabbit.Transfer.Domain.Events;
    
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace MicroRabbit.Transfer.Api
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
            services.AddDbContext<ItemDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("TransferDbConnection"));
            }
            );

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Transfer Microservice", Version = "v1" });
                //option.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{Assembly.GetEntryAssembly().GetName().Name}.xml");

            });

            services.AddMediatR(typeof(Startup));

            RegisterServices(services);
        }

        private void RegisterServices(IServiceCollection services)
        {
            DependencyContainer.RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Transfer Microservice V1");
            });
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

            });

            ConfigureEventBus(app);
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<TransferCreatedEvent, TransferEventHandler>();
        }
    }
}
