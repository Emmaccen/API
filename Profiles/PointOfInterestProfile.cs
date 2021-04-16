using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;

namespace CityInfo.API.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        public PointOfInterestProfile()
        {
            CreateMap<Entities.PointOfInterest, Models.PointsOfInterestsDto>();

            CreateMap<Models.PointsOfInterestForUpdateDto, Entities.PointOfInterest>()
                .ReverseMap();

            CreateMap<Models.PointsOfInterestForCreation, Entities.PointOfInterest>();

        }
    }
}
