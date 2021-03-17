using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MG.Services.Catalog.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private static IEnumerable<WeatherForecast> _dummyWeatherData;

        private static IEnumerable<WeatherForecast> DummyWeatherData
        {
            get
            {
                if (_dummyWeatherData != null) return _dummyWeatherData;

                var rng = new Random();
                _dummyWeatherData = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();

                return _dummyWeatherData;
            }
            set
            {
                _dummyWeatherData = value;
            }
        }

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return DummyWeatherData;
        }

        [HttpGet("{id}", Name = "GetById")]
        public IActionResult GetById(Guid id)
        {
            var item = DummyWeatherData.FirstOrDefault(x => x.Id == id);
            if (item == null) return NotFound();

            return Ok(item);
        }

        [Authorize("catalog.fullaccess")]
        [HttpPost]
        public IActionResult Post([FromBody] WeatherForecast weather)
        {
            weather.Id = Guid.NewGuid();
            DummyWeatherData = DummyWeatherData.Append(weather);
            return CreatedAtAction("GetById", new { id = weather.Id }, weather);
        }
    }
}
