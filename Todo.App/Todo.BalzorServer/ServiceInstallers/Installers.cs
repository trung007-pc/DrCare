using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Todo.Core.Consts.Permissions;
using Todo.Core.DependencyRegistrationTypes;
using Todo.Core.Securitys;

namespace Todo.BalzorServer.ServiceInstallers;

public static class Installers
{
    private static  IServiceCollection _service { get; set; }
    private static IConfiguration _configuration { get; set; }
    
    public static void  InstallServices(this  IServiceCollection service, IConfiguration configuration)
    {
        _service = service;
        _configuration = configuration;
        RegisterService();
        AddAuthorization();
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

    

            // The rest omitted for brevity.
        });
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

    }
}