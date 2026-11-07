using BlogApi.Data;
using BlogApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly BlogDbContext _db;

    public AuthorsController(BlogDbContext db)
    {
        _db = db;
    }

    // GET api/authors
    [HttpGet]
    public async Task<ActionResult<List<AuthorResponse>>> GetAll()
    {
        var authors = await _db.Authors
            .OrderBy(a => a.Id)
            .Select(a => new AuthorResponse(a.Id, a.Name, a.Bio, a.Posts.Count))
            .ToListAsync();

        return authors;
    }

    // GET api/authors/5
    [HttpGet("{id}")]
    public async Task<ActionResult<AuthorResponse>> GetById(int id)
    {
        var author = await _db.Authors
            .Where(a => a.Id == id)
            .Select(a => new AuthorResponse(a.Id, a.Name, a.Bio, a.Posts.Count))
            .FirstOrDefaultAsync();

        if (author is null)
        {
            return NotFound();
        }

        return author;
    }

    // POST api/authors
    [HttpPost]
    public async Task<ActionResult<AuthorResponse>> Create(AuthorRequest request)
    {
        var author = new Author
        {
            Name = request.Name,
            Bio = request.Bio
        };

        _db.Authors.Add(author);
        await _db.SaveChangesAsync();

        var response = new AuthorResponse(author.Id, author.Name, author.Bio, 0);
        return CreatedAtAction(nameof(GetById), new { id = author.Id }, response);
    }
}