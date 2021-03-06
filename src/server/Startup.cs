using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Krokodil.Profiles;
using Krokodil.Services;
using Krokodil.Services.PlayerPicker;
using Krokodil.Services.TimerManager;
using Krokodil.Services.WordPicker;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Krokodil
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(builder =>
                builder.UseSqlite("Data Source=db.db;")
            );

            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder.AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                    builder.WithOrigins("http://localhost:3000");
                });
            });

            services.AddTransient<IPlayerPicker, PlayerPicker>();
            services.AddTransient<IGameManager, GameManager>();
            services.AddSingleton<IWordPicker, WordPicker>();
            services.AddSingleton<ITimerManager, TimerManager>();
            services.AddMemoryCache();
            services.AddSignalR();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("EnableCORS");

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<MessageHub>("/messages");
            });
        }
    }
}
