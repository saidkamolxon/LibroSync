using FluentValidation;
using LibroSync.Api.DTOs.Book;
using LibroSync.Data.IRepositories;

namespace LibroSync.Api.Validators.Book;

public class BookCreationDtoValidator : AbstractValidator<BookCreationDto>
{
    public BookCreationDtoValidator(IServiceProvider serviceProvider)
    {
        RuleFor(b => b.Title)
            .NotEmpty().WithMessage("Book title is required.")
            .Length(3, 100).WithMessage("Book title must be between 3 and 100 characters.")
            .MustAsync(async (title, cancellationToken) =>
            {
                var repository = serviceProvider.GetRequiredService<IRepository>();
                return await repository.RetrieveByTitleAsync(title, cancellationToken) is null;
            })
            .WithMessage("A book with this title already exists.");

        RuleFor(b => b.AuthorName)
            .NotEmpty().WithMessage("Author's name is required.")
            .Length(6, 100).WithMessage("Author's name must be between 6 and 100 characters.");

        RuleFor(b => b.PublicationYear)
            .InclusiveBetween(1000, DateTime.UtcNow.Year)
            .WithMessage($"Publication year must be between 1000 and {DateTime.UtcNow.Year}.");
    }
}
