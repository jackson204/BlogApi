namespace BlogApi.Models;

public class Author
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Bio { get; set; }

    // 一對多：一個作者有很多文章。這是「集合導覽屬性」，不會變成資料表的欄位
    public List<Post> Posts { get; set; } = new(); 
}
