using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using WordCounter.Options;
using WordCounter.Services;
using WordCounter.Services.Base;

namespace WordCounter
{
    public class Startup
    {
        #region Constructors

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        #endregion

        #region Properties

        public IConfiguration Configuration { get; }

        #endregion

        #region Public Members

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseSwagger(swaggerOptions => { });
            app.UseSwaggerUI(c => { c.DocumentTitle = "Word Counter - Swagger UI"; });
            var corsOptions = app.ApplicationServices.GetService<IOptions<CorsOptions>>();
            corsOptions.Value.AddDefaultPolicy(policy =>
            {
                policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("http://localhost:3000"); // for UI
            });
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(swaggerGenOptions => { swaggerGenOptions.EnableAnnotations(); });
            services.AddOptions();
            services.Configure<WordCountOptions>(this.Configuration.GetSection("WordCount"));

            services.AddTransient<IWordCountService, WordCountService>();
            services.AddTransient<IHashService, Sha512HashService>();
            services.AddSingleton<IMemoryCache, MemoryCache>();
            services.AddCors();
        }

        #endregion
    }
}