using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DapperCrudTutorial.Services;

namespace DapperCrudTutorial.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class DisneyHeroController : ControllerBase
    {
     
        private readonly HttpClient _httpClient;

        public DisneyHeroController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("DisneyAPI");

        }

        // GET /<heroId>
        [HttpGet("{heroId}")]
        public async Task<IActionResult> GetDisneyHero(int heroId)
        {
            try
            {
                
                
                var response = await _httpClient.GetAsync("character/" + heroId);

                // Check if the response is successful.
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    return Ok(json);
                }
                else
                {
                    // Handle the error here if the request was not successful.
                    return StatusCode((int)response.StatusCode, "API request failed.");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the request.
                return BadRequest($"Error: {ex.Message}");
            }
        }

    }
    
}
