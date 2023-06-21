using System.Reflection;
using Blazored.LocalStorage;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Todo.AdminBlazor.Helper;
using Todo.AdminBlazor.Middlewares;
using Todo.AdminBlazor.Network;
using Todo.Core.Consts.Permissions;
using Todo.Core.DependencyRegistrationTypes;
using Todo.Core.Securitys;
using Todo.Core.Validations;
using Todo.Localziration;

namespace Todo.AdminBlazor.ServiceInstallers;

public static class Installers
{
    private static  IServiceCollection _service { get; set; }
    private static IConfiguration _configuration { get; set; }

    public static void  InstallServices(this  IServiceCollection service, IConfiguration configuration)
    {
        _service = service;
        _configuration = configuration;
        InstallFluentValidation();
        RegisterService();
        AddAuthorization();
        AddHttpClientConfig();
    }



     private static async void AddHttpClientConfig()
     {
         _service.AddHttpClient("DefaultClient", client =>
         {
             client.BaseAddress = new Uri("https://localhost:7195/api/");
         });



     }
    
    private static void AddAuthorization()
    {
        _service.AddScoped<IAuthorizationHandler, AppAuthorizationHandler>();

        _service.AddAuthorization(options =>
        {
            options.AddPolicy(AccessClaims.Roles.Default, builder =>
            {
                builder.AddRequirements(new ClaimRequirement(AccessClaims.Roles.Default));
            });
            options.AddPolicy(AccessClaims.Tenants.Default, builder =>
                {
                    builder.AddRequirements(new ClaimRequirement(AccessClaims.Tenants.Default));
                }
            );
            options.AddPolicy(AccessClaims.Users.Default, builder =>
            {
                builder.AddRequirements(new ClaimRequirement(AccessClaims.Users.Default));
            });
            // The rest omitted for brevity.
        });
    }

    private static void InstallFluentValidation()
    {
        _service.AddFluentValidationAutoValidation();
        _service.AddFluentValidationClientsideAdapters();
        _service.AddValidatorsFromAssemblyContaining<IValidatorService>();
    }
    private static void RegisterService()
    {
        var executingAssembly = Assembly.GetExecutingAssembly();
        var referencedAssemblies = executingAssembly.GetReferencedAssemblies();

        foreach (var assemblyName in referencedAssemblies)
        {
            var assembly = Assembly.Load(assemblyName);
            var types = assembly.GetTypes()
                .Where(t => typeof(ITransientDependency).IsAssignableFrom(t) && t.IsClass);
        
            foreach (var type in types)
            {
                _service.AddTransient(type);
            }
        }
        
        _service.AddScoped<IAuthorizationHandler, AppAuthorizationHandler>();
        _service.AddHttpContextAccessor();
        _service.AddScoped<JwtTokenHeaderHandler>();
        _service.AddScoped<CookieHelper>();
        _service.AddSingleton<Localizer>();
        _service.AddScoped<LocalizationMiddleware>();

    }
}