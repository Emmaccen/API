using CityInfo.API.Contexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _cityInfoContext;
        private readonly IMailService _mailService;

        public CityInfoRepository(CityInfoContext cityInfoContext, IMailService mailService)
        {
            this._cityInfoContext = cityInfoContext ?? throw new ArgumentNullException(nameof(cityInfoContext));
            this._mailService = mailService;
        }

        public void AddPoiForSingleCity(int cityId, PointOfInterest poi)
        {
            GetSingleCity(cityId, false).PointsOfInterests.Add(poi);
            SaveChanges();
        }

        public bool CityExists(int cityId)
        {
            return _cityInfoContext.City.Any(c => c.Id == cityId);
        }

        public void DeleteSinglePoi(int cityId, int poiId, bool sendMailService)
        {
            var poiToDelete = _cityInfoContext.PointOfInterests
                .Where(p => p.City.Id == cityId && p.Id == poiId)
                .FirstOrDefault();

            _cityInfoContext.City.FirstOrDefault(c => c.Id == cityId)
                .PointsOfInterests.Remove(poiToDelete);

            SaveChanges();

            if(sendMailService)
            _mailService.ActivateMailService($"New point of interest deleted",
           $"Point of interest with Name: {poiToDelete.Name} and ID: {poiToDelete.Id} has been deleted");
        }

        public IEnumerable<City> GetAllCities()
        {
            return _cityInfoContext.City.OrderBy(c => c.Name).ToList();
        }

        public IEnumerable<PointOfInterest> GetAllPointsOfInterests()
        {
            return _cityInfoContext.City.SelectMany(c => c.PointsOfInterests).ToList();
        }

        public IEnumerable<PointOfInterest> GetPointOfInterestsForSingleCity(int cityId)
        {
            return _cityInfoContext.PointOfInterests.Where(p => p.City.Id == cityId).ToList();
        }

        public City GetSingleCity(int cityId, bool includePoi)

        {
            return includePoi ? _cityInfoContext.City
                .Where(c => c.Id == cityId)
                .Include(p => p.PointsOfInterests)
                .FirstOrDefault()
                :
                _cityInfoContext.City.FirstOrDefault(c => c.Id == cityId);
        }

        public PointOfInterest GetSinglePointOfInterest(int cityId, int poiId)
        {
            return _cityInfoContext.PointOfInterests
                .Where(p => p.City.Id == cityId && p.Id == poiId)
                .FirstOrDefault();
        }

        public bool PoiExists(int cityId, int poiId)
        {
            return _cityInfoContext.PointOfInterests
                .Where(p => p.City.Id == cityId && p.Id == poiId)
                .Any();
        }

        public bool SaveChanges()
        {
            return _cityInfoContext.SaveChanges() >= 0;
        }

        public void UpdatePoi(int cityId, PointOfInterest poiFromStore)
        {
            /*
             this method will be empty because we're tryna avoid problems when we inject
            a repository that doesn't automatically track its Entities.
             */
        }
    }
}
