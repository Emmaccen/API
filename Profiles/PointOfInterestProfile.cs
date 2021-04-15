using AutoMapper;

namespace CityInfo.API.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        public PointOfInterestProfile()
        {
            CreateMap<Entities.PointOfInterest, Models.PointsOfInterestsDto>();
            CreateMap<Entities.PointOfInterest, Models.PointsOfInterestForUpdateDto>();
            CreateMap<Models.PointsOfInterestForCreation, Entities.PointOfInterest>();
        }
    }
}
