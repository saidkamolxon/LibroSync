namespace LibroSync.Api.DTOs.Book;

public record BookCreationDto(
    string Title,
    int PublicationYear,
    string AuthorName
);