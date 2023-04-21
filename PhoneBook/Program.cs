using Microsoft.OpenApi.Models;
using PhoneBook.Model;
using PhoneBook.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace PhoneBook
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "PhoneBook API",
                    Description = "A simple API for managing a phone book."
                });
            });

            // Add Logger
            var logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(builder.Configuration)
                            .Enrich.FromLogContext()
                            .CreateLogger();

            builder.Logging.AddSerilog(logger);

            // Add SQLite database
            builder.Services.AddDbContext<PhoneBookContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("PhoneBookDatabase"));
            });

            builder.Services.AddScoped<IPhoneBookService, DatabasePhoneBookService>();
            //builder.Services.AddSingleton<IPhoneBookService, DictionaryPhoneBookService>();

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