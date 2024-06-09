using API.Extensions;
using API.Middleware;
using Infrastructure.Data;
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
            builder.Services.AddControllers();
            builder.Services.AddApplicationServices(builder.Configuration);



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStatusCodePagesWithReExecute("errors/{0}");
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // to serve static files(images)
            app.UseStaticFiles();

            app.UseAuthorization();

            app.MapControllers();

            //To create db migration every time when i build the porgram
            #region create db migration
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
            #endregion


            app.Run();
        }
    }
}