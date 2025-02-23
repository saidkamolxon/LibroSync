namespace LibroSync.Domain.Entities;

public class Book : Auditable
{
    public required string Title { get; set; }
    public int PublicationYear { get; set; }
    public required string AuthorName { get; set; }
    public long ViewsCount { get; set; }
}
