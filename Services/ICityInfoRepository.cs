using CityInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {

        IEnumerable<City> GetAllCities();

        City GetSingleCity(int cityId, bool includePoi);

        IEnumerable<PointOfInterest> GetPointOfInterestsForSingleCity(int cityId);

        IEnumerable<PointOfInterest> GetAllPointsOfInterests();

        IEnumerable<PointOfInterest> GetSinglePointOfInterest(int cityId, int poiId);

    }
}
