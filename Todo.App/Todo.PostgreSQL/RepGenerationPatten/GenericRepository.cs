using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Todo.Domain.BaseEntities;
using Todo.MongoDb.PostgreSQL;

namespace Todo.MongoDb.RepGenerationPatten;

public class GenericRepository<T, Key> : IGenericRepository<T, Key> where T : class where Key : struct
{
    protected readonly TodoContext _context;
    public DbSet<T> Entity { get; set; }
    public GenericRepository(TodoContext context) 
    {
         
        _context = context;
         Entity = context.Set<T>();
    }


    public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> expression, bool istracked = false)
    {
        if (istracked)
        {
            return await _context.Set<T>().Where(expression).AsNoTracking().ToListAsync();
        }
        return await _context.Set<T>().Where(expression).AsNoTracking().ToListAsync();
    }

    public List<T> GetList(Expression<Func<T, bool>> expression, bool istracked = false)
    {
        if (istracked)
        {
            return  _context.Set<T>().Where(expression).AsNoTracking().ToList();
        }
        return  _context.Set<T>().Where(expression).AsNoTracking().ToList();
    }

    public async Task<List<T>> ToListAsync(bool istracked = false)
    {
        
        if (istracked)
        {
            return  await Entity.ToListAsync();
        }
    
        return await Entity.AsNoTracking().ToListAsync();
    }
    
    public List<T> ToList(bool istracked = false)
    {
        if (istracked)
        {
            return   Entity.ToList();
        }
    
        return  Entity.AsNoTracking().ToList();
    }


}