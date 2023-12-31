
using Api.Middlewares;
using Api.Validators;
using Cinema.Application.Interfaces.Repositories;
using Cinema.Application.Interfaces.Services;
using Cinema.Application.Services;
using Data;
using Data.Configurations;
using DnsClient;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			try
			{
				var builder = WebApplication.CreateBuilder(args);

				builder.Host.UseSerilog((ctx, config) => config.ReadFrom.Configuration(ctx.Configuration));

				var logger = new LoggerConfiguration()
							.ReadFrom
							.Configuration(builder.Configuration)
							.CreateBootstrapLogger();

				// Add services to the container.

				builder.Services.AddControllers();
				builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

				// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
				builder.Services.AddEndpointsApiExplorer();
				builder.Services.AddSwaggerGen();

				builder.Services.AddTransient<ITheaterService, TheaterService>();
				builder.Services.AddTransient<IMovieService, MovieService>();
				builder.Services.RegisterDataServices(builder.Configuration);

				builder.Services.AddCors(opt =>
				{
					opt.AddPolicy("web-app", builder =>
					{
						builder
							.WithOrigins("http://localhost:4200")
							.AllowAnyHeader()
							.AllowAnyMethod();
					});
				});

				var app = builder.Build();


				// Configure the HTTP request pipeline.
				if (app.Environment.IsDevelopment())
				{
					app.UseSwagger();
					app.UseSwaggerUI((options) =>
					{
						options.SwaggerEndpoint("v1/swagger.json", "V1");
					});
				}

				app.UseMiddleware<ErrorLoggerMiddleware>();

				app.UseRouting();
				app.UseHttpsRedirection();
				app.UseCors("web-app");
				app.UseAuthorization();

				app.UseEndpoints((config) =>
				{
					config.MapControllers();
				});

				app.Run();
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "Application terminated unexpectedly");
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}
	}
}