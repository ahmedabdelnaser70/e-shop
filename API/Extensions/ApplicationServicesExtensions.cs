using API.Errors;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using static System.Net.Mime.MediaTypeNames;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {

            services.AddDbContext<StoreContext>(options => options.UseSqlite(config.GetConnectionString("StoreDB")));

            //this line to set something like DB in Redis for Basket
            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var option = ConfigurationOptions.Parse(config.GetConnectionString("Redis"));
                return ConnectionMultiplexer.Connect(option);
            });

            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IProductRepo, ProductRepo>();
            services.AddScoped<ITokenService, TokenService>();




            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            //Add Swager Autherization 
            #region Swager Autherization

            services.AddSwaggerGen(c =>
            {
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Auth Bearer Scheme",
                    Name = "Authorisation",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securitySchema);

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        securitySchema, new[] {"Bearer"}
                    }
                };

                c.AddSecurityRequirement(securityRequirement);

            });

            #endregion

            // Error Configurations
            #region Error Configurations
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ActionContext =>
                {
                    var errors = ActionContext.ModelState
                    .Where(e => e.Value.Errors.Count() > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };

            });

            #endregion



            //Add Cors
            #region Add Cors
            var cors = "CorsPolicy";
            services.AddCors(options =>
            {
                options.AddPolicy(cors, builder =>
                {
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                });
            });
            #endregion

            return services;
        }
    }
}
