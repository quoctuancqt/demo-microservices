namespace Demo.EventBus
{
    using Microsoft.Azure.ServiceBus;
    using System;

    public interface IServiceBusPersisterConnection : IDisposable
    {
        ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder { get; }

        //ITopicClient CreateModel();
        IQueueClient CreateModel();
    }
}