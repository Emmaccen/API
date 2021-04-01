using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [ApiController]
    //[Route("Api/[controller]")]
    [Route("Api/Cities")]
    public class CitiesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetCities()
        {
            return Ok(CitiesDataStore.Current.Cities);
        }
        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            var result = CitiesDataStore.Current.Cities.FirstOrDefault(uniqueId => uniqueId.Id == id);

            if (result == null)
                return NotFound();
            else
                return Ok(result);
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
