using System.Net;
using Microsoft.AspNetCore.Http;
using Todo.Core.Consts.ErrorCodes;

namespace Todo.Core.Helpers;

public class FileHelper
{
    public static async Task<FileModel> UploadFile(IFormFile file,string basePath,List<string> ExtensionsConstraint,string baseUri = "")
    {
        string directoryPath = "";
        var filePath = "";
        var fileDto = new FileModel();
        if (file.Length > 0)
        {
            var ext = Path.GetExtension(file.FileName);

                  
            if (!ExtensionsConstraint.Any(x => x.Contains(ext)))
            {
                throw new Exception(UploadErrorCode.InvalidExtension);
            }
            
            var directory = $"{DateTime.Now:dd-MM-yyyy}";
            directoryPath = Path.GetFullPath(Path.Combine(basePath,directory));
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
                    
            var StorageFileName = $"{Guid.NewGuid()}{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid()}" + file.FileName;
            filePath = Path.Combine(directoryPath,StorageFileName);
            using (var fileStream = new FileStream(filePath , FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            fileDto.Extension = ext;
            fileDto.Path = filePath;
            fileDto.Name =  file.FileName;
            if (baseUri != "")
            {
                fileDto.Url = Path.Combine(baseUri,directory,StorageFileName);;
            }
            return fileDto;
        }
        else
        {
            return fileDto;
        }
           
    }
    
    public class FileModel
    {
        public string Path { get; set; }
        public string Name { get; set;}
        public string Extension { get; set; }
        
        public string Url { get; set; }
        
    }
}