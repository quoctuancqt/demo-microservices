using Microsoft.ServiceFabric.Services.Communication.Client;

namespace Demo.SFCommunication
{
    public interface IHttpCommunicationClientFactory : ICommunicationClientFactory<HttpCommunicationClient>
    {
    }
}