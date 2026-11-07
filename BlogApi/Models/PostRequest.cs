using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models;

public record PostRequest(
    [Range(1, int.MaxValue)] int AuthorId,
    [Required, MaxLength(200)] string Title,
    [Required] string Content);
