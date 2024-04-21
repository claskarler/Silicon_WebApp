using Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Infrastructure.Services;

public class CourseService(HttpClient httpClient, IConfiguration configuration)
{
    private readonly HttpClient _httpClient = httpClient;

    private readonly IConfiguration _configuration = configuration;

    public async Task<IEnumerable<Course>> GetCoursesAsync()
    {
        var response = await _httpClient.GetAsync(_configuration["ApiUris:Courses"]);
        if (response.IsSuccessStatusCode)
        {
            var result = JsonConvert.DeserializeObject<CourseResult>(await response.Content.ReadAsStringAsync());
            if(result != null && result.Succeeded) 
            {
                return result.Courses ??= null!;
            }
        }
        return null!;
    }

}
