using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace DapperCrudTutorial.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class DisneyHeroController : ControllerBase
    {
     
        private readonly HttpClient _httpClient;
        
        public DisneyHeroController()
        {
            _httpClient = new HttpClient();
            
        }

        // GET /<heroId>
        [HttpGet("{heroId}")]
        public async Task<IActionResult> GetDisneyHero(int heroId)
        {
            try
            {
                
                
                var response = await _httpClient.GetAsync("https://api.disneyapi.dev/character/" + heroId);

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
