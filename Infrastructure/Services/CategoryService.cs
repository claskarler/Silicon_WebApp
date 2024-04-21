using Microsoft.Extensions.Configuration;
using Infrastructure.Models;
using Newtonsoft.Json;

namespace Infrastructure.Services;

public class CategoryService(HttpClient httpClient, IConfiguration configuration)
{
    private readonly HttpClient _httpClient = httpClient;

    private readonly IConfiguration _configuration = configuration;

    public async Task <IEnumerable<Category>> GetCategoriesAsync()
    {
        var response = await _httpClient.GetAsync(_configuration["ApiUris:Categories"]);
        if (response.IsSuccessStatusCode) 
        {
            var categories = JsonConvert.DeserializeObject<IEnumerable<Category>>(await response.Content.ReadAsStringAsync());
            return categories ??= null!;
        }
        return null!;
    }

}
