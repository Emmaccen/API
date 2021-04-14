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

        public IEnumerable<City> GetAllCities()
        {
            return _cityInfoContext.City.OrderBy(c => c.Name).ToList();
        }

        public IEnumerable<PointOfInterest> GetPointOfInterestsForSingleCity(int cityId)
        {
            return _cityInfoContext.PointOfInterests.Where(p => p.City.Id == cityId).ToList();
        }

        public City GetSingleCity(int cityId, bool includePoi)
        {
            return includePoi ? _cityInfoContext.City
                .Include(p => p.PointsOfInterests)
                .Where(c => c.Id == cityId).FirstOrDefault()
                :
                _cityInfoContext.City.FirstOrDefault(c => c.Id == cityId);
        }

        public IEnumerable<PointOfInterest> GetSinglePointOfInterest(int cityId, int poiId)
        {
            return _cityInfoContext.PointOfInterests
                .Where(p => p.City.Id == cityId && p.Id == poiId)
                .ToList();
        }
    }
}
