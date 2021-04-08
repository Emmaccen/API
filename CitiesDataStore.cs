using CityInfo.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public static CitiesDataStore Current { get; } = new CitiesDataStore();
        public List<CityDto> Cities { get; set; }
        public PointsOfInterestsDto PointsOfInterests { get; set; }

        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1, Name = "Lagos", Description = "City of the wise", PointsOfInterests = new List<PointsOfInterestsDto>()
                    {
                        new PointsOfInterestsDto
                        {
                            Name = "some City", Description = "City of the gods", Id = 1
                        },
                        new PointsOfInterestsDto
                        {
                            Name = "some City", Description = "City of the gods", Id = 2
                        },
                        new PointsOfInterestsDto
                        {
                            Name = "some City", Description = "City of the gods", Id = 3
                        }
                    }
                },
                new CityDto()
                {
                    Id = 2, Name = "Chad", Description = "Unknown worlds", PointsOfInterests = new List<PointsOfInterestsDto>()
                    {
                        new PointsOfInterestsDto
                        {
                            Name = "some City", Description = "City of the gods", Id = 4
                        },
                        new PointsOfInterestsDto
                        {
                            Name = "some City", Description = "City of the gods", Id = 5
                        },
                        new PointsOfInterestsDto
                        {
                            Name = "some City", Description = "City of the gods", Id = 6
                        }
                    }
                },
                new CityDto()
                {
                    Id = 3, Name = "Kwakalalaka", Description = "Why don't you ask google", PointsOfInterests = new List<PointsOfInterestsDto>()
                    {
                        new PointsOfInterestsDto
                        {
                            Name = "some City", Description = "City of the gods", Id = 7
                        },
                        new PointsOfInterestsDto
                        {
                            Name = "some City", Description = "City of the gods", Id = 8
                        },
                        new PointsOfInterestsDto
                        {
                            Name = "some City", Description = "City of the gods", Id = 9
                        }
                    }
                }
            };
        }
    }
}
