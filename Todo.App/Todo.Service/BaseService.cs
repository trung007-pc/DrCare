using AutoMapper;

namespace Todo.Service;

public class BaseService
{
    protected IMapper ObjectMapper { get;}
        
    public BaseService()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoMapperProfile());
        });
        ObjectMapper = config.CreateMapper();
       
    }
}

