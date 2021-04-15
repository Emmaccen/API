using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API.Controllers
{
    [ApiController]
    //[Route("Api/[controller]")]
    [Route("Api/Cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ICityInfoRepository infoRepository;
        private readonly IMapper mapper;

        public CitiesController(ICityInfoRepository infoRepository, IMapper mapper)
        {
            this.infoRepository = infoRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("/")]
        public IActionResult StartUp()
        {
            return Ok(new { value1 = 1, value2 = 2 });
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            var cities = infoRepository.GetAllCities();
            // using mapper
            return Ok(mapper.Map<IEnumerable<CitiesWithoutPoi>>(cities));
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            var cities = infoRepository.GetSingleCity(id, true);
            return Ok(mapper.Map<CityDto>(cities));
        }

        [HttpPost(Name = "CreatedCity")]
        public IActionResult CreateCity([FromBody] CityDto newCity)
        {
            var cities = CitiesDataStore.Current.Cities;
            var maxCityId = CitiesDataStore.Current.Cities.Max(c => c.Id);
            var maxPoiId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterests)
                .Max(id => id.Id);

            foreach (var city in newCity.PointsOfInterests)
            {
                city.Id = ++maxPoiId;
            }

            var finalResult = new CityDto()
            {
                Id = ++maxCityId,
                Name = newCity.Name,
                Description = newCity.Description,
                PointsOfInterests = newCity.PointsOfInterests
            };

            if (newCity == null)
                return BadRequest();
            else
            {
                cities.Add(finalResult);
                return CreatedAtRoute("createdCity",
                new
                {
                    finalResult.Id
                },
                finalResult
                );
            }
        }
    }
}
