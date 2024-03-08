
using System.Data;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySqlConnector;

namespace OSSApi
{
#pragma warning disable CS1591
    public class Program
    {
        public static void Main(string[] args)
        {
            Dapper.SqlMapper.AddTypeHandler(typeof(Models.PlayerSetting),new JsonTypeHandler());

            var builder = WebApplication.CreateBuilder(args);

            #region Configs

            var configs = builder.Configuration;

            var ucenterAddr = configs["UserCenter"];
            if (string.IsNullOrEmpty(ucenterAddr))
            {
                throw new Exception("UserCenter address is not configured");
            }

            var apiScope = configs["ApiScope"];
            if (string.IsNullOrEmpty(apiScope))
            {
                throw new Exception("ApiScope is not configured");
            }

            var dbConnectionString = configs.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(dbConnectionString))
            {
                throw new Exception("Connection string is not configured");
            }
            Global.ConnectionString = dbConnectionString;

            #endregion

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = ucenterAddr;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", apiScope);
                });
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Values API",
                    Description = "An ASP.NET Core Web API for managing Values items",
                    // TermsOfService = new Uri("https://example.com/terms"),
                    // Contact = new OpenApiContact
                    // {
                    //     Name = "Example Contact",
                    //     Url = new Uri("https://example.com/contact")
                    // },
                    // License = new OpenApiLicense
                    // {
                    //     Name = "Example License",
                    //     Url = new Uri("https://example.com/license")
                    // }
                });

                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
                
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            builder.Services.AddHttpClient<IUserInfoClient, UserInfoClient>(client =>
            {
                client.BaseAddress = new Uri(ucenterAddr);
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("SiteCorsPolicy", x =>
                {
                    x.WithOrigins(configs.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>())
                        .AllowCredentials().WithHeaders(configs.GetSection("AllowedHeaders").Get<string[]>() ?? Array.Empty<string>())
                        .WithMethods(configs.GetSection("AllowedMethods").Get<string[]>() ?? Array.Empty<string>());
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseCors("SiteCorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers()
                .RequireAuthorization("ApiScope");

            app.Run();
        }
    }
#pragma warning restore CS1591
}
