using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Todo.Contract.Uploads;
using Todo.Core.Helpers;

namespace Todo.Service.Uploads;

public class UploadService : IUploadService
{
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;
    private string RootPath { get; set; }
    
    public UploadService(IConfiguration configuration,IHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
        RootPath = environment.ContentRootPath;
    }
    public async Task<UrlDto> UploadAvatar(IFormFile file)
    {
        string pathBase = Path.Combine(RootPath,_configuration["Media:Avatars"]);
        var res = await FileHelper.UploadFile(file, pathBase,new List<string>(){".jpg",".pnd"}, _configuration["Media:a1s3s4e5tss/avatars"]);
        return new UrlDto() {Url = res.Url};
    }

    public Task<List<UrlDto>> UploadMessageImages(List<IFormFile> files)
    {
        throw new NotImplementedException();
    }
}