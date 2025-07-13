using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SurveyBasket.Api.Authentication;
using SurveyBasket.Api.Persistence;
using System.Text;
namespace SurveyBasket.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddDependecies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddAuthConfig();

        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
          throw new InvalidOperationException("Connection string 'DefaultConnection' not Found ");

        services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));

        services
            .AddMapsterConf()
            .AddFluentValidationConf();


        services.AddOpenApi();

        services.AddScoped<IPollService, PollService>();
        services.AddScoped<IAuthService, AuthService>();
        


        return services;
    }

    private static IServiceCollection AddMapsterConf(this IServiceCollection services)
    {
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton<IMapper>(new Mapper(mappingConfig));

        return services;
    }
    private static IServiceCollection AddFluentValidationConf(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }

    private static IServiceCollection AddAuthConfig(this IServiceCollection services)
    {
        services.AddSingleton<IJwtProvider, JwtProvider>();
        services.AddIdentity<ApplicationUser, IdentityRole>().
            AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o=>
        {
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "SurveyBasket",
                ValidAudience = "SurveyBasket",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("zE:gT~+3_BHUy,E"))
            };
        }
        );

        return services;
    }
}
