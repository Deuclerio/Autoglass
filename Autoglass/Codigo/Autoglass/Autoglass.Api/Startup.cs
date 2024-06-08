using Autoglass.Api.Filters;
using Autoglass.Api.Monitoring;
using Autoglass.Application.AutoMapper;
using Autoglass.Application.Service.Interfaces;
using Autoglass.Application.Services;
using Autoglass.Domain;
using Autoglass.Domain.Core.Interfaces.Repositories;
using Autoglass.Domain.Core.Interfaces.Services;
using Autoglass.Domain.Services.Services;
using Autoglass.Infra.Data.Context;
using Autoglass.Infra.Data.Repositories;
using Autoglass.Infra.Data.UoW;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Prometheus;
using Prometheus.SystemMetrics;
using Serilog;
using Serilog.Debugging;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Autoglass.Api
{
    public class Startup
    {
        private readonly string CorsPolicy = "All";
        public IConfiguration Configuration { get; }
        public AppSettings AppSettings { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(configuration)
                            .CreateLogger();

            SelfLog.Enable(Console.Out);
        }

        // Este método é chamado pelo tempo de execução. Use este método para configurar o pipeline de solicitação HTTP.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<Filters.ExceptionHandlerMiddleware>();
            app.UseMiddleware<SecurityHeadersMiddleware>();
            app.UseRouting();
            app.UseHttpMetrics();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(CorsPolicy);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapMetrics();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    ResponseWriter = StartupHealthCheck.WriteResponse
                });
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }

        // Este método é chamado pelo tempo de execução. Use este método para adicionar serviços ao contêiner.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSystemMetrics();

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddHealthChecks()
                .AddCheck("Db Check", new SqlConnectionHealthCheck(connectionString))
                .ForwardToPrometheus();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API",
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            var mvc = services.AddMvc(setupAction =>
            {
                setupAction.Filters.Add(new ProducesAttribute("application/json", "application/xml"));

                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            });

            mvc.AddJsonOptions(
                options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    options.JsonSerializerOptions.WriteIndented = false;
                    options.JsonSerializerOptions.AllowTrailingCommas = false;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            AppSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();
            services.AddSingleton(AppSettings);

            var siteURL = AppSettings.SiteUrl;

            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicy,
                builder =>
                {
                    builder.WithOrigins(siteURL)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            // Registro correto do DbContext
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(connectionString));

            ConfigureContainerServices(services);
        }

        private void ConfigureContainerServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddScoped<IDbConnection>(p => new SqlConnection(connectionString));

            services.AddHttpClient();

            // Remova a linha abaixo, pois já estamos registrando DatabaseContext como DbContext
            // services.AddScoped<DbContext, DatabaseContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<IMemoryCache>(p => new MemoryCache(new MemoryCacheOptions() { SizeLimit = 5000 }));

            services.AddScoped<IProdutoAppService, ProdutoAppService>();
            services.AddScoped<IProdutoService, ProdutoService>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();

            var config = AutoMapperConfig.RegisterMappings();
            services.AddSingleton(config);
            services.AddTransient(p => config.CreateMapper(services.BuildServiceProvider().GetService));
        }
    }
}
