using BlogApi.Data;
using BlogApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly BlogDbContext _db;

    public PostsController(BlogDbContext db)
    {
        _db = db;
    }

    // GET api/posts?keyword=docker
    [HttpGet]
    public async Task<ActionResult<List<Post>>> GetAll(string? keyword)
    {
        // 起點：還沒送 SQL。宣告成 IQueryable<Post>，下面才能把 Where 的結果放回同一個變數（程式碼後面有說明）
        IQueryable<Post> query = _db.Posts;

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(p => p.Title.Contains(keyword));
        }

        var posts = await query
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return posts;
    }

    // GET api/posts/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Post>> GetById(int id)
    {
        var post = await _db.Posts.FindAsync(id);
        if (post is null)
        {
            return NotFound();
        }

        return post;
    }

    // POST api/posts
    [HttpPost]
    public async Task<ActionResult<Post>> Create(PostRequest request)
    {
        // 作者不存在就回 400；拿到的 author 之後回應要放作者名時會用到
        var author = await _db.Authors.FindAsync(request.AuthorId);
        if (author is null)
        {
            ModelState.AddModelError(nameof(request.AuthorId), "作者不存在");
            return ValidationProblem();
        }

        var post = new Post
        {
            AuthorId = request.AuthorId,
            Title = request.Title,
            Content = request.Content,
            CreatedAt = DateTime.UtcNow
        };

        _db.Posts.Add(post);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = post.Id }, post);
    }

    // PUT api/posts/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PostRequest request)
    {
        var post = await _db.Posts.FindAsync(id);
        if (post is null)
        {
            return NotFound();
        }

        // Update 不需要作者的欄位，只問存不存在（SQL 是 EXISTS）
        bool authorExists = await _db.Authors.AnyAsync(a => a.Id == request.AuthorId);
        if (!authorExists)
        {
            ModelState.AddModelError(nameof(request.AuthorId), "作者不存在");
            return ValidationProblem();
        }

        post.AuthorId = request.AuthorId;
        post.Title = request.Title;
        post.Content = request.Content;
        post.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE api/posts/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var post = await _db.Posts.FindAsync(id);
        if (post is null)
        {
            return NotFound();
        }

        _db.Posts.Remove(post);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}