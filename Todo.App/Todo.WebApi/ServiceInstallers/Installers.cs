using System.Configuration;
using System.Reflection;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Todo.App.Middlewares;
using Todo.Core.DependencyRegistrationTypes;
using Todo.Core.Securitys;
using Todo.Core.Validations;
using Todo.Domain.Roles;
using Todo.Domain.Users;
using Todo.Localziration;
using Todo.MongoDb;
using Todo.MongoDb.PostgreSQL;
using Todo.MongoDb.Repositorys;

namespace Todo.App.ServiceInstallers;

public static class Installers
{
    private static  IServiceCollection _service { get; set; }
    private static IConfiguration _configuration { get; set; }
    
    public static void  InstallServices(this  IServiceCollection service, IConfiguration configuration)
    {
        _service = service;
        _configuration = configuration;
        RegisterService();
        Postgresql();
        // InstallMongoDb();
        InstallFluentValidation();
        InstallLocalization();
        InstallJwt();
    }

    private static void InstallMongoDb()
    {
        
        // _service.AddTransient<TodoContext>(x =>
        // {
        //     var connection = MongoDbConnection.FromUrl(new MongoUrl("mongodb://localhost:27017/MyTodo"));
        //     return new TodoContext(connection);
        // });
        
        _service.Configure<DatabaseSettings>(
            _configuration.GetSection(nameof(DatabaseSettings)));
        _service.AddSingleton<DatabaseSettings>(sp =>
            sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);
        
        // _service.AddScoped<MongoClient>(x => new MongoClient("mongodb://localhost:27017"));
    }

    private static void Postgresql()
    {
        _service.AddDbContext<TodoContext>(options => {
            options.UseNpgsql(_configuration.GetConnectionString("MyApp"),b=>b.MigrationsAssembly("Todo.WebApi"));
        });
        _service.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<TodoContext>()
            .AddDefaultTokenProviders();
        
        _service.Configure<IdentityOptions> (options => {
            // set up Password
            options.Password.RequireDigit = false; // Không bắt phải có số
            options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
            options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
            options.Password.RequireUppercase = false; // Không bắt buộc chữ in
            options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
            options.Password.RequiredUniqueChars = 0; // Số ký tự riêng biệt
    
            // Cấu hình Lockout - khóa user
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (5); // Khóa 5 phút
            options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lầ thì khóa
            options.Lockout.AllowedForNewUsers = true;

            // Cấu hình về User.
            
            options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = false;  // Email là duy nhất
    
            // configure login
            options.SignIn.RequireConfirmedEmail = false;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
            options.SignIn.RequireConfirmedPhoneNumber = false;   
            // Xác thực số điện thoại

        });
    }
    private static void InstallFluentValidation()
    {
        _service.AddFluentValidationAutoValidation();
        _service.AddFluentValidationClientsideAdapters();
        _service.AddValidatorsFromAssemblyContaining<IValidatorService>();
    }

    private static void InstallLocalization()
    {
        _service.AddSingleton<Localizer>();
    }

    private static void InstallJwt()
    {
        _service.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };
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
                .Where(t => typeof(IDependencyService).IsAssignableFrom(t) && t.IsClass);
        
            foreach (var type in types)
            {
                
                if (typeof(ITransientDependency).IsAssignableFrom(type))
                {
                    _service.AddTransient(type);

                }else if (typeof(IScopeDependency).IsAssignableFrom(type))
                {
                    _service.AddScoped(type);

                }
                else
                {
                    _service.AddSingleton(type);
                }
            }
        }
        
        

        _service.AddScoped<MultiTenantServiceMiddleware>();
        _service.AddScoped<TenantContext>();
        _service.AddScoped<IAuthorizationHandler, AppAuthorizationHandler>();

    }
}