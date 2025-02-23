using LibroSync.Domain.Entities;
using System.Linq.Expressions;
using System.Threading;

namespace LibroSync.Data.IRepositories;

public interface IRepository
{
    Task InsertAsync(Book book, CancellationToken cancellationToken = default);
    Task InsertManyAsync(IEnumerable<Book> books, CancellationToken cancellationToken = default);
    Task<Book?> RetrieveByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Book?> RetrieveByTitleAsync(string title, CancellationToken cancellationToken = default);
    IQueryable<Book> RetrieveAll(Expression<Func<Book, bool>>? expression = null, bool asNoTracking = true, string[]? includes = null);
    Task UpdateAsync(Book book, CancellationToken cancellationToken = default);
    Task DeleteAsync(Book book, CancellationToken cancellationToken = default);
    Task DeleteManyAsync(Expression<Func<Book, bool>> predicate, CancellationToken cancellationToken = default);
}
