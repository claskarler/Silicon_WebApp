namespace Infrastructure.Models;

public class Course
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Image { get; set; }
    public string Author { get; set; } = null!;
    public bool IsBestseller { get; set; }
    public bool IsDigital { get; set; }
    public int Hours { get; set; }
    public string? LikesInProcent { get; set; }
    public string? NumberOfLikes { get; set; }
    public string OriginalPrice { get; set; } = null!;
    public string? DiscountPrice { get; set; }
    public string? Category { get; set; } = null!;

}
