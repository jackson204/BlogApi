namespace BlogApi.Models;

public record AuthorResponse(
    int Id,
    string Name,
    string? Bio,
    int PostCount);
