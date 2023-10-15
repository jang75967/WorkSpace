using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using AutoMapper;
using Client.Business.Extensions;
using Client.Business.Infrastructure;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Client.Business.IntegratedTest.Factories;

public class ServiceProviderCustomization : ICustomization
{
    public GrpcChannel Channel { get; set; } = default!;
    public ServiceProvider ServiceProvider { get; set; } = default!;
    public ServiceProviderCustomization()
    {
        ServiceProvider = new ServiceCollection()
            .AddAutoMapper()
            .BuildServiceProvider();
    }
    public void Customize(IFixture fixture)
    {
        fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
        fixture.Customizations.Add(new TypeRelay(typeof(Application.Mappers.IMapper), typeof(AutoMapperDI)));
        fixture.Customizations.Add(new TypeRelay(typeof(IMapper), typeof(MapperConfiguration)));


        var mapper = ServiceProvider.GetRequiredService<Application.Mappers.IMapper>();
        var config = ServiceProvider.GetRequiredService<IMapper>();

        fixture.Inject(mapper);
        fixture.Inject(config);

        fixture.Register(() => Channel);
    }
}
