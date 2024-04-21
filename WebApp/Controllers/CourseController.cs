using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Authorize]
public class CourseController(CategoryService categoryService, CourseService courseService) : Controller
{
    private readonly CategoryService _categoryService = categoryService;
    private readonly CourseService _courseService = courseService;

    [Route("/courses")]
    public async Task<IActionResult> Courses()
    {
        var viewModel = new CourseIndexViewModel
        {
            Categories = await _categoryService.GetCategoriesAsync(),
            Courses = await _courseService.GetCoursesAsync(),

        };

        return View(viewModel);
    }
}
