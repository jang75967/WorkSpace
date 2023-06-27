using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Common;

public class TestFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    public GrpcChannel Channel => CreateChannel();
    public TestFactory() { }

    protected GrpcChannel CreateChannel()
    {
        return GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
        {
            HttpHandler = Server.CreateHandler(),
            MaxReceiveMessageSize = 1024 * 1024 * 1566,
            MaxSendMessageSize = 1024 * 1024 * 1566,
        });
    }
}