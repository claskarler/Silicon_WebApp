using Infrastructure.Models;


namespace WebApp.ViewModels;

public class CourseIndexViewModel
{
    public IEnumerable<Category>? Categories { get; set; }
    public IEnumerable<Course>? Courses { get; set; }

}
