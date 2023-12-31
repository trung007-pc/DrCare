using System.Reflection;
using Blazored.LocalStorage;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using MudBlazor.Services;
using Radzen;
using Todo.AdminBlazor.Data;
using Todo.AdminBlazor.Middlewares;
using Todo.AdminBlazor.Network;
using Todo.AdminBlazor.Security;
using Todo.AdminBlazor.ServiceInstallers;
using Todo.AdminBlazor.Services;
using Todo.Core.DependencyRegistrationTypes;
using Todo.Localziration;
using DialogService = Radzen.DialogService;
using Variant = Radzen.Variant;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = MudBlazor.Variant.Filled;
});
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<RoleService>();
builder.Services.AddSingleton<Localizer>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddScoped<AuthenticationStateProvider , ApiAuthenticationStateProvider>();
builder.Services.AddScoped<ClientSetter>();
builder.Services.AddScoped<TenantService>();
builder.Services.InstallServices(builder.Configuration);








builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseMiddleware<LocalizationMiddleware>();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();