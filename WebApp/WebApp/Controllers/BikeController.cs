using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.Model;
using WebApp.Service;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikeController : ControllerBase
    {
        // POST: api/Bike
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetFilteredModel([FromQuery] int startYear, [FromQuery] int endYear)
        {
            var service = new VehicleService();
            var vehicles  = await service.GetVehiclesAsync(startYear, endYear);
            return Ok(vehicles);
        }
    }
}
