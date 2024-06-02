using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;



namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            //AddCors


            // Add services to the container.
            builder.Services.AddDbContext<StoreContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("StoreDB"))
            );

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IProductRepo, ProductRepo>();
            builder.Services.AddControllers();



            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


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

            //To create db migration every time when i build the porgram
            using var scope = app.Services.CreateScope();
            var service = scope.ServiceProvider;
            var context = service.GetRequiredService<StoreContext>();
            var logger = service.GetRequiredService<ILogger<Program>>();
            try
            {
                await context.Database.MigrateAsync();
                //Seed data into the database when building the program if it is not already present in the database.
                await StoreContextSeed.SeedAsync(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, " An error occured during migration");
            }
            //----------------------------------------------------------

            app.Run();
        }
    }
}