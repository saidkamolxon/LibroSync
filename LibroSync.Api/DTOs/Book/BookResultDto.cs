namespace LibroSync.Api.DTOs.Book;

public record BookResultDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int PublicationYear { get; set; }
    public string AuthorName { get; set; }
    public long ViewsCount { get; set; }

    public int YearsSincePublished
    {
        get => DateTime.UtcNow.Year - PublicationYear;
    }

    public double PopularityScore
    {
        get => ViewsCount * 0.5 + (10 / Math.Log(YearsSincePublished + 2));
    }
}
