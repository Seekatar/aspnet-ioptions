using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace aspnet_webapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IOptions<OneTimeOptions> _oneTime;
        private readonly IOptionsSnapshot<SnapshotOptions> _snapshot;
        private readonly IOptionsMonitor<MonitorOptions> _monitor;
        private readonly OneTimeOptions _oneTimeValue;
        private readonly SnapshotOptions _snapshotValue;
        private readonly MonitorOptions _monitorValue;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IOptions<OneTimeOptions> oneTime,
            IOptionsSnapshot<SnapshotOptions> snapshot,
            IOptionsMonitor<MonitorOptions> monitor,
            ITestService test,
            ITestServiceInject testInject)
        {
            _logger = logger;
            _oneTime = oneTime;
            _snapshot = snapshot;
            _monitor = monitor;
            _oneTimeValue = oneTime.Value;
            _snapshotValue = snapshot.Value;
            _monitorValue = monitor.CurrentValue;
            test.DoIt();
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation(_oneTimeValue.ToString());
            _logger.LogInformation(_snapshotValue.ToString());
            _logger.LogInformation(_monitorValue.ToString());

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

        }
    }
}
