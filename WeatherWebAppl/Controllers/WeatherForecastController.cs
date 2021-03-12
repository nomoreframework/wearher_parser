using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherWebAppl.Models;

namespace WeatherWebAppl.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        readonly WetherContext db;
        public WeatherForecastController(WetherContext context)
        {
            db = context;
        }
     [HttpGet]
     public async Task<ActionResult> GetWeather()
        {
            var weather = await db.GetWeather();
            return Ok(weather);
        }
    }
}
