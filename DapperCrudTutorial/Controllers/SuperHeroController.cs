using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Net;



namespace DapperCrudTutorial.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SuperHeroController : ControllerBase
    {
        public IConfiguration _config { get; }

        public SuperHeroController(IConfiguration config)
        {
            _config = config;
        }

        // GET /superhero 
        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetAllSuperHeros()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var heroes = await connection.QueryAsync<SuperHero>("select * from superheroes");

            return Ok(heroes);
        }


        // GET /superhero/{heroId}
        [HttpGet("{heroId}")]
        public async Task<ActionResult<SuperHero>> GetHero(int heroId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var hero = await connection.QueryAsync<SuperHero>("select * from superheroes where id = @Id", new { Id = heroId });
            if (hero.Count() == 0)
            {
                return Ok("This Id does not exits");
            }

            return Ok(hero);
        }

        // POST add a new superhero in the database
        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> CreateHero(SuperHero hero)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("insert into superheroes (name, firstname, lastname, place) values (@Name, @FirstName, @LastName, @Place)", hero);
            var heroes = await connection.QueryAsync<SuperHero>("select * from superheroes");
            return Ok(heroes);
        }

        //PUT update a superhero column
        [HttpPut]
        public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero hero)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("update superheroes set name = @Name, firstname = @FirstName, lastname = @LastName, place = @Place where id = @Id", hero);
            var heroes = await connection.QueryAsync<SuperHero>("select * from superheroes");
            return Ok(heroes);
        }

        // DELETE delete a superhero by id
        [HttpDelete("{heroId}")]
        public async Task<ActionResult<List<SuperHero>>> DeleteHero(int heroId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var result = await connection.ExecuteAsync("delete from superheroes where id = @Id", new { Id = heroId });
            if(result == 0)
            {
                return Ok("This Id does not exist.");

            }
            var heroes = await connection.QueryAsync<SuperHero>("select * from superheroes");

            return Ok(heroes);
        }


    }
}


