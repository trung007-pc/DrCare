using System.Linq.Expressions;

namespace Todo.MongoDb.RepGenerationPatten;

public interface IGenericRepository<T,TKey> where T : class where TKey:struct
{
    Task<List<T>> GetListAsync(Expression<Func<T, bool>> expression,bool istracked = false);
    List<T> GetList(Expression<Func<T, bool>> expression,bool istracked = false);

    Task<List<T>> ToListAsync(bool istracked = false);
    List<T> ToList(bool istracked = false);

}