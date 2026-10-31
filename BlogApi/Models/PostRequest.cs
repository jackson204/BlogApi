using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models;

public record PostRequest(
    [Required, MaxLength(200)] string Title,
    [Required] string Content);
