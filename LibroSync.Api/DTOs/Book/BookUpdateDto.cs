namespace LibroSync.Api.DTOs.Book;

public record BookUpdateDto(
    int Id,
    string Title,
    int PublicationYear,
    string AuthorName
);
