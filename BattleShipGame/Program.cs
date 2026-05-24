using System.Text;
using BattleShipGame.Application.Dtos;
using BattleShipGame.Application.Interfaces;
using BattleShipGame.Application.Mapping;
using BattleShipGame.Application.Services;
using BattleShipGame.Domain.Entities;
using BattleShipGame.Infrastructure.Auth;
using BattleShipGame.Infrastructure.Persistence;
using BattleShipGame.Infrastructure.Repositiories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BattleShipGame
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

            builder
                .Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<UserDbContext>();
            builder.Services.AddAutoMapper(
                cfg =>
                {
                    cfg.CreateMap<User, UserDto>();
                },
                typeof(UserProfile)
            );
            builder.Services.AddDbContext<UserDbContext>(options =>
                options.UseMySql(
                    builder.Configuration.GetConnectionString("default"),
                    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("default"))
                )
            );
            builder
                .Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "BattleShipGameApi",
                        ValidAudience = "BattleShipGameApi-client",
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                builder.Configuration.GetSection("Token:Key").Value
                            )
                        ),
                    };
                });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
