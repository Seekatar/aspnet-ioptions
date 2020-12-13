using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace aspnet_webapi
{
    // singleton class that injects Options via DI
    internal class TestServiceInject : ITestServiceInject
    {
        private readonly ILogger<TestServiceInject> _logger;
        private readonly OneTimeOptions _options;
        private readonly MonitorOptions _monitor;

        public TestServiceInject(ILogger<TestServiceInject> logger, IOptions<OneTimeOptions> options, IOptions<MonitorOptions> monitor)
        {
            _logger = logger;
            _options = options.Value;
            _monitor = monitor.Value;
            _logger.LogInformation($"In TestServiceInject constructor!!! {options}");
            _logger.LogInformation($"In TestServiceInject constructor!!! {_monitor}");
        }

        public void DoIt()
        {
            _logger.LogInformation($"In TestServiceInject {nameof(DoIt)} {_options}");
            _logger.LogInformation($"In TestServiceInject {nameof(DoIt)} {_monitor}");
        }
    }
}