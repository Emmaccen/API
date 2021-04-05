using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfo.API.Models;
using Microsoft.AspNetCore.JsonPatch;
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

            if (poi.Name == poi.Description)
            {
                ModelState.AddModelError("Description", "Name and Description cannot be the same");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //you can use the below, if you're not doing the checks yourself
            /* if (city == null)
                 return NotFound();*/

            var maxPoiId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterests)
                .Max(id => id.Id);

            var finalResult = new PointsOfInterestsDto()
            {
                Name = poi.Name,
                Id = ++maxPoiId,
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

        [HttpPut("{poiId}")]
        public IActionResult ReplacePointOfInterest(int poiId, int cityId, [FromBody] PointsOfInterestForUpdateDto poiUpdate)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
                return NotFound();

            var poiFromStore = city.PointsOfInterests.FirstOrDefault(poi => poi.Id == poiId);

            if (poiFromStore == null)
                return NotFound();

            poiFromStore.Name = poiUpdate.Name;
            poiFromStore.Description = poiUpdate.Description;

            return NoContent();
        }

        [HttpPatch("{poiId}")]
        public IActionResult UpdatePointOfInterest(int poiId, int cityId, 
            [FromBody] JsonPatchDocument<PointsOfInterestForUpdateDto> poiUpdate)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
                return NotFound();

            var poiFromStore = city.PointsOfInterests.FirstOrDefault(poi => poi.Id == poiId);

            if (poiFromStore == null)
                return NotFound();

            var poiToPatch = new PointsOfInterestForUpdateDto
            {
                Name = poiFromStore.Name,
                Description = poiFromStore.Description,
            };

            poiUpdate.ApplyTo(poiToPatch, ModelState);

            if (!ModelState.IsValid)
                return BadRequest();

            /* Now in a case where someone send a request like
             * [{
                "op" : "remove",
                "path": "/name",
                },]
            This kind of request will succeed because the input is a "JsonPatchDocument"
            * which is a valid request, but... then our Name field is requered and should
            * not be removeable/nullable so, after checking ModelState, which would be valid
            * cuz our JsonPatchDocument is a valid request, we also wanna chech the poiToPatch
            * variable to make sure everything checks out fine
            */

            if (!TryValidateModel(poiToPatch))
                return BadRequest();

            poiFromStore.Name = poiToPatch.Name;
            poiFromStore.Description = poiToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{poiId}")]
        public IActionResult DeletePointOfInterest(int cityId, int poiId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (city == null)
                return NotFound();

            var poiFromStore = city.PointsOfInterests.FirstOrDefault(poi => poi.Id == poiId);

            if (poiFromStore == null)
                return NotFound();

            city.PointsOfInterests.Remove(poiFromStore);

            return NoContent();
        }
    }
}
