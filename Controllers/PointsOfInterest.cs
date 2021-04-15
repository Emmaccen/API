using AutoMapper;
using CityInfo.API.Contexts;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("Api/cities/{cityId}/[controller]")]
    public class PointsOfInterest : ControllerBase
    {
        private readonly ILogger<PointsOfInterest> logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository infoRepository;
        private readonly IMapper mapper;
        private readonly CityInfoContext cityInfoContext;

        public PointsOfInterest(ILogger<PointsOfInterest> logger, IMailService mailService
            , ICityInfoRepository infoRepository, IMapper mapper, CityInfoContext cityInfoContext)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this._mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            this.infoRepository = infoRepository;
            this.mapper = mapper;
            this.cityInfoContext = cityInfoContext;
        }

        [HttpGet]
        public IActionResult GetPointsOfInterests(int cityId)
        {
            try
            {
                var poi = infoRepository.GetPointOfInterestsForSingleCity(cityId);
                if (poi == null)
                {
                    logger.LogInformation($"The city with id {cityId} cannot be found");
                    return NotFound();
                }
                else
                    return Ok(mapper.Map<List<PointsOfInterestsDto>>(poi));
            }
            catch (Exception ex)
            {
                logger.LogCritical("An error occured when getting points of interest", ex);
                return StatusCode(500, "Please try again later, your request will succeed when server is done being angry");
            }
        }

        [HttpGet("{id}", Name = "CreatedPointOfInterest")]
        public IActionResult GetSinglePointOfInterest(int cityId, int id)
        {
            var poi = infoRepository.GetSinglePointOfInterest(cityId, id);
            if (poi != null)
                return Ok(mapper.Map<PointsOfInterestsDto>(poi));
            else
                return NotFound();
        }

        [HttpPost]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointsOfInterestForCreation poi)
        {
            if (poi.Name == poi.Description)
                ModelState.AddModelError("Description", "Name and Description cannot be the same");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (infoRepository.CityExists(cityId))
            {
                var finalPoi = mapper.Map<Entities.PointOfInterest>(poi);
                infoRepository.AddPoiForSingleCity(cityId, finalPoi);
                var response = mapper.Map<PointsOfInterestsDto>(finalPoi);
                return CreatedAtRoute("CreatedPointOfInterest",
                new{ cityId, response.Id}, response);
            }
            else
                return NotFound();
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

        /*[HttpGet]
        public IActionResult GetAllPointsOfInterests() => Ok(infoRepository.GetAllPointsOfInterests());*/


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
            _mailService.ActivateMailService($"New point of interest deleted",
                $"Point of interest with Name: {poiFromStore.Name} and ID: {poiFromStore.Id} has been deleted");

            return NoContent();
        }
    }
}
