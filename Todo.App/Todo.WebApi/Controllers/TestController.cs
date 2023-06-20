using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Domain.Tests;
using Todo.MongoDb.Repositorys;

namespace Todo.App.Controllers;

[ApiController]
[Route("api/test/")]
public class TestController
{
    public UnitOfWork _unitOfWork;
    public TestController(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    [HttpPost]
    public async Task Create()
    {
        var tests = await _unitOfWork.TestRepository.Entity.Where(x => x.names.Contains("A")).ToListAsync();
        
        Console.Write(tests);
    }
}