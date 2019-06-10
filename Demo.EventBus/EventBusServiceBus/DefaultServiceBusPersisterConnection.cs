using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using System;

namespace Demo.EventBus
{
    public class DefaultServiceBusPersisterConnection : IServiceBusPersisterConnection
    {
        private readonly ILogger<DefaultServiceBusPersisterConnection> _logger;
        private readonly ServiceBusConnectionStringBuilder _serviceBusConnectionStringBuilder;
        //private ITopicClient _topicClient;
        private IQueueClient _queueClient;

        bool _disposed;

        public DefaultServiceBusPersisterConnection(ServiceBusConnectionStringBuilder serviceBusConnectionStringBuilder,
            ILogger<DefaultServiceBusPersisterConnection> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _serviceBusConnectionStringBuilder = serviceBusConnectionStringBuilder ??
                throw new ArgumentNullException(nameof(serviceBusConnectionStringBuilder));
            //_topicClient = new TopicClient(_serviceBusConnectionStringBuilder, RetryPolicy.Default);
            _queueClient = new QueueClient(_serviceBusConnectionStringBuilder, ReceiveMode.PeekLock);
        }

        public ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder => _serviceBusConnectionStringBuilder;

        //public ITopicClient CreateModel()
        //{
        //    if (_topicClient.IsClosedOrClosing)
        //    {
        //        _topicClient = new TopicClient(_serviceBusConnectionStringBuilder, RetryPolicy.Default);
        //    }

        //    return _topicClient;
        //}

        public IQueueClient CreateModel()
        {
            if (_queueClient.IsClosedOrClosing)
            {
                _queueClient = new QueueClient(_serviceBusConnectionStringBuilder, ReceiveMode.PeekLock);
            }

            return _queueClient;
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;
        }
    }
}
