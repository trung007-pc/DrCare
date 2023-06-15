using Microsoft.AspNetCore.Mvc;
using Todo.Contract.Uploads;
using Todo.Service.Uploads;

namespace Todo.App.Controllers;

[ApiController]
[Route("api/upload/")]
public class UploadController : IUploadService
{
    private UploadService _uploadService;
    public UploadController(UploadService uploadService)
    {
        _uploadService = uploadService;
    }
    
    [HttpPost]
    [Route("save-avatar")]
    public Task<UrlDto> UploadAvatar(IFormFile file)
    {
        return _uploadService.UploadAvatar(file);
    }

    [HttpPost]
    [Route("save-avataqqqqqr")]
    public Task<List<UrlDto>> UploadMessageImages(List<IFormFile> files)
    {
        throw new NotImplementedException();
    }
}