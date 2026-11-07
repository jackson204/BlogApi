using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models;
public record AuthorRequest(
    [Required, MaxLength(50)] string Name,
    [MaxLength(500)] string? Bio);