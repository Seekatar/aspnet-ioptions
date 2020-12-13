using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace aspnet_webapi
{
    // singleton class that gets Options from constructor in Startup
    internal class TestService : ITestService
    {
        private readonly ILogger _logger;
        private readonly OneTimeOptions _options;
        private readonly MonitorOptions _monitor;

        public TestService(ILogger logger, OneTimeOptions options, MonitorOptions monitor)
        {
            _logger = logger;
            _options = options;
            _monitor = monitor;
            _logger.LogInformation($"In TestService constructor!!! {options}");
            _logger.LogInformation($"In TestService constructor!!! {_monitor}");

        }

        public void DoIt()
        {
            _logger.LogInformation($"In TestService {nameof(DoIt)} {_options}");
            _logger.LogInformation($"In TestService {nameof(DoIt)} {_monitor}");
        }
    }
}