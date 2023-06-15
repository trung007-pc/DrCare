
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Todo.App;
using Todo.App.Middlewares;
using Todo.App.ServiceInstallers;
using Todo.MongoDb.PostgreSQL;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.InstallServices(builder.Configuration);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseMiddleware<LocalizationMiddleware>();
app.UseMiddleware<GlobalErrorHandlingMiddleware>();
// app.UseStaticFiles(new StaticFileOptions
// {
//     FileProvider = new PhysicalFileProvider(
//         Path.Combine(builder.Environment.ContentRootPath, "wwwroot")),
//     RequestPath = "/a1s3s4e5tss"
// });
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();