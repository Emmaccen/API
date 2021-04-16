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

        PointOfInterest GetSinglePointOfInterest(int cityId, int poiId);

        bool CityExists(int cityId);

        bool PoiExists(int cityId, int poiId);

        bool SaveChanges();

        void AddPoiForSingleCity(int cityId, PointOfInterest poi);

        void DeleteSinglePoi(int cityId, int poiId, bool sendMailService);

        void UpdatePoi(int cityId, PointOfInterest poiFromStore);
    }
}
