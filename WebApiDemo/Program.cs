using System.Text;
using Serilog.Events;
using Serilog;
using WebApiDemo.Controllers;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Microsoft.Extensions.Options;



namespace WebApiDemo
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            builder.Services.AddControllers();
            //var jwtOptions = new JwtOptions();

            var jwtOptions = Options.Create(new JwtOptions
            {
                SecretKey = "SecretKeySecretKeySecretKeySecretKeySecretKeySecretKeySecretKeySecretKey",
                ExpiresHours = 3
            });
            
            
            
            var services = new Services.ChatService(jwtOptions);
            
            builder.Services.AddSingleton<Services.IChatService>(services);
          
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSignalR();

            //builder.Services.AddSingleton<UserController>();

            var appName = typeof(Program).Assembly.GetName().Name;

            var loggerConfiguration = new LoggerConfiguration();

            Log.Logger = loggerConfiguration.MinimumLevel.Debug()
                                            .MinimumLevel.Override("/Microsoft", LogEventLevel.Information)
                                            .Enrich.FromLogContext()
                                            .WriteTo.Console()
                                            .CreateLogger();

             
            builder.Host.UseSerilog();

            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
                //app.UseSwaggerUI();
            }
            
            app.UseCors(builder =>
            {
                
                builder.WithOrigins("http://localhost:5173") 
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
            
             /*services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost",
                    builder => builder.WithOrigins("http://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });*/
            app.UseCors("AllowLocalhost");


            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.MapHub<ChatHub>("/Hub");

            /*app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/hub");
            });
            */
            app.Run();
        }
    }
}
