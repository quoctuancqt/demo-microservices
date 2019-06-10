using Demo.EventBus.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Demo.EventBus.HostServices
{
    public abstract class BaseConsumeHostService : BackgroundService
    {
        protected readonly IEventBus _eventBus;
        protected readonly ILogger<BaseConsumeHostService> _logger;
        public BaseConsumeHostService(IEventBus eventBus, ILogger<BaseConsumeHostService> logger)
        {
            _eventBus = eventBus;
            _logger = logger;
        }
    }
}
