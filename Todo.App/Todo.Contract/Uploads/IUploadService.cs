using Microsoft.AspNetCore.Http;

namespace Todo.Contract.Uploads;

public interface IUploadService
{
    Task<UrlDto> UploadAvatar(IFormFile file);
    Task<List<UrlDto>> UploadMessageImages(List<IFormFile> files);
}