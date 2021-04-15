using CityInfo.API.Contexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _cityInfoContext;

        public CityInfoRepository(CityInfoContext cityInfoContext)
        {
            this._cityInfoContext = cityInfoContext ?? throw new ArgumentNullException(nameof(cityInfoContext));
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

        public bool SaveChanges()
        {
            return _cityInfoContext.SaveChanges() >= 0;
        }
    }
}
