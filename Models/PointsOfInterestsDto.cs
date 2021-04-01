using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Models
{
    public class PointsOfInterestsDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "You must should provide a name bruv!")]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
