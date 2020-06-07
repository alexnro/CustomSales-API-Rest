using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIRestCustomSales.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace APIRestCustomSales {
    public class Startup {

        private readonly string AllowSpecificOrigins = "_AllowSpecificOrigins";

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddCors(options => {
                options.AddPolicy(
                    name: AllowSpecificOrigins,
                    builder => {
                        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();        
                    });
            });

            services.Configure<DatabaseSettings>(
                Configuration.GetSection(nameof(DatabaseSettings)));

            services.AddSingleton<IDatabaseSettings>(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<DatabaseSettings>>().Value);

            services.AddSingleton<ProductsService>();

            services.AddSingleton<ClientsService>();

            services.AddControllers()
               .AddNewtonsoftJson(options => options.UseMemberCasing());

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(AllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
