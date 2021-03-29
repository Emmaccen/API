using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("Api/cities/{cityId}/[controller]")]
    public class PointsOfInterest : ControllerBase
    {
        public IActionResult GetPointsOfInterests(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
                return NotFound();
            return Ok(city.PointsOfInterests);
        }

        [HttpGet("{id}", Name = "CreatedPointOfInterest")]
        public IActionResult GetSinglePointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
                return NotFound();

            var singlePointOfInterest = city.PointsOfInterests.FirstOrDefault(interest => interest.Id == id);
            if (singlePointOfInterest == null)
                return NotFound();

            return Ok(singlePointOfInterest);
        }

        [HttpPost]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointsOfInterestForCreation poi)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
                return NotFound();

            var maxCityId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterests)
                .Max(id => id.Id);

            var finalResult = new PointsOfInterestsDto()
            { 
                Name = poi.Name, 
                Id = ++maxCityId, 
                Description = poi.Description 
            };

            city.PointsOfInterests.Add(finalResult);
            return CreatedAtRoute("CreatedPointOfInterest",
                new
                {
                   cityId,
                   finalResult.Id
                },
                finalResult
                );

        }
    }
}
