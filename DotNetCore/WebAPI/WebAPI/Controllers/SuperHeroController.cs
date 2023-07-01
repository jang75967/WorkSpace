using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SuperHeroController : ControllerBase
    {
        private static List<SuperHero> heroes = new List<SuperHero>
        {
            new SuperHero
            {
                ID = 1,
                Name = "Spider Man",
                FirstName = "Peter",
                LastName = "Paker",
                Place = "New York City",
                DateAdded = new DateTime(2010, 08, 10),
                DateModified = null
            },
            new SuperHero
            {
                ID = 2,
                Name = "Iron Man",
                FirstName = "Tony",
                LastName = "Stark",
                Place = "Malibu",
                DateAdded = new DateTime(2070, 05, 29),
                DateModified = null
            },
        };

        private readonly IMapper _mapper;

        public SuperHeroController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<List<SuperHero>> GetHeroes()
        {
            //return heroes;
            //return Ok(heroes);


            // Use Mapper
            return Ok(heroes.Select(hero => _mapper.Map<SuperHeroDto>(hero)));
        }

        [HttpPost]
        public ActionResult<List<SuperHero>> AddHero(SuperHeroDto newHero)
        {
            // Use Mapper
            var hero = _mapper.Map<SuperHero>(newHero);
            heroes.Add(hero);

            return Ok(heroes.Select(hero => _mapper.Map<SuperHeroDto>(hero)));
        }
    }
}
