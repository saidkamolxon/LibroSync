using LibroSync.Data.Contexts;
using LibroSync.Data.IRepositories;
using LibroSync.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibroSync.Data.Repositories;

public class Repository(AppDbContext context) : IRepository
{
    private readonly AppDbContext context = context;

    public IQueryable<Book> RetrieveAll(Expression<Func<Book, bool>>? expression = null, bool asNoTracking = true, string[]? includes = null)
    {
        IQueryable<Book> query = context.Books;

        if (expression is not null)
            query = query.Where(expression);

        if (asNoTracking)
            query = query.AsNoTracking();

        if (includes is not null)
            query = includes.Aggregate(query, (current, include) => current.Include(include));

        return query.Where(e => !e.IsDeleted);
    }

    public async Task<Book?> RetrieveByIdAsync(int id, CancellationToken cancellationToken = default)
        => await this.context.Books.FindAsync(id, cancellationToken);

    public async Task<Book?> RetrieveByTitleAsync(string title, CancellationToken cancellationToken = default)
        => await this.context.Books.FirstOrDefaultAsync(e => e.Title == title, cancellationToken);
    
    public async Task InsertAsync(Book book, CancellationToken cancellationToken = default)
    { 
        await this.context.AddAsync(book, cancellationToken);
        await this.context.SaveChangesAsync(cancellationToken);
    }

    public async Task InsertManyAsync(IEnumerable<Book> books, CancellationToken cancellationToken = default)
    {
        await this.context.AddRangeAsync(books, cancellationToken);
        await this.context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Book book, CancellationToken cancellationToken = default)
    { 
        book.IsDeleted = true;
        await this.context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task DeleteManyAsync(Expression<Func<Book, bool>> predicate, CancellationToken cancellationToken = default)
    {
        await this.context.Books
            .Where(predicate)
            .ExecuteUpdateAsync(setter => 
                setter.SetProperty(b => b.IsDeleted, true), cancellationToken);
    }

    public async Task UpdateAsync(Book book, CancellationToken cancellationToken = default)
    {
        book.UpdatedAt = DateTime.UtcNow;
        this.context.Entry(book).State = EntityState.Modified;
        await this.context.SaveChangesAsync(cancellationToken);
    }
}
