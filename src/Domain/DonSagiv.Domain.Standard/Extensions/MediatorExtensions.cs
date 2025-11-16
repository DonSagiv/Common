using Autofac;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using System.Linq;

namespace DonSagiv.Domain.Standard.Extensions;

public static class MediatorExtensions
{
    public static ContainerBuilder AddMediatR(this ContainerBuilder builder,
        params string[] assemblyDirectories)
    {
        var assemblies = AssemblyExtensions
            .GetAssemblies(x => x.Contains("DonSagiv"), assemblyDirectories.AddExecutingAssemblyDirectory())
            .ToArray();

        var configuration = MediatRConfigurationBuilder.Create(assemblies)
            .WithAllOpenGenericHandlerTypesRegistered()
            .Build();

        builder.RegisterMediatR(configuration);

        return builder;
    }
}
