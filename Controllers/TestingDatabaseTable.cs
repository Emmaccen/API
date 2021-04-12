using CityInfo.API.Contexts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestingDatabaseTable : ControllerBase
    {
        private readonly CityInfoContext cityInfoContext;

        public TestingDatabaseTable(CityInfoContext cityInfoContext)
        {
            this.cityInfoContext = cityInfoContext ?? throw new ArgumentNullException(nameof(cityInfoContext));
        }

        [HttpGet]
        public IActionResult Testing()
        {
            return Ok();
        }
    }
}
