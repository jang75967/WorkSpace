
using CleanMovie.Application;
using CleanMovie.Application.Interface;
using CleanMovie.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CleanMovie.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Configuration 등록
            ConfigurationManager configuration = builder.Configuration;
            //


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            // DB 서비스 추가
            builder.Services.AddDbContext<MovieDbContext>(option => option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), 
                                                          build => build.MigrationsAssembly("CleanMovie.API")));
            //

            // Dependency Injection 
            builder.Services.AddScoped<IMovieService, MovieService>();
            builder.Services.AddScoped<IMovieRepository, MovieRepository>();
            //


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}