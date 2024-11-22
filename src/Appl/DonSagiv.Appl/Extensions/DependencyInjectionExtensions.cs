using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Extensions.DependencyInjection;
using Autofac.Features.AttributeFilters;
using DonSagiv.Domain.DependencyInjection;
using DonSagiv.Domain.Extensions;
using Microsoft.Extensions.Hosting;

namespace DonSagiv.Appl.Extensions;

public static class DependencyInjectionExtensions
{
    #region Initialization
    public static IHostBuilder BuildContainers(this IHostBuilder hostBuilder,
        Action<ContainerBuilder> builderAction,
        params string[] externalPaths)
    {
        hostBuilder.UseServiceProviderFactory(new AutofacServiceProviderFactory(builder =>
        {
            builderAction.Invoke(builder);
            builder.ImportExternalAssemblies(externalPaths);
        }));

        return hostBuilder;
    }

    private static void ImportExternalAssemblies(this ContainerBuilder builder,
        params string[] externalPaths)
    {

    }
    #endregion

    #region Composition
    public static void AddSingleton<TType>(this ContainerBuilder builder)
        where TType : notnull
    {
        builder.RegisterType<TType>()
            .SingleInstance()
            .WithAttributeFiltering();
    }

    public static void AddSingleton<TBase, TDerived>(this ContainerBuilder builder,
            object? contractKey = null)
        where TBase : notnull
        where TDerived : notnull
    {
        if (!typeof(TDerived).IsAssignableFrom(typeof(TBase)))
        {
            throw new ArgumentException($"Type {typeof(TDerived).Name} is not assignable from Type {typeof(TBase).Name}");
        }

        builder.RegisterType<TDerived>()
            .ApplyContract(typeof(TBase), contractKey, CreationPolicy.Singleton);
    }

    public static void AddScoped<TType>(this ContainerBuilder builder)
        where TType : notnull
    {
        builder.RegisterType<TType>()
            .InstancePerLifetimeScope()
            .WithAttributeFiltering();
    }

    public static void AddScoped<TBase, TDerived>(this ContainerBuilder builder,
            object? contractKey = null)
        where TBase : notnull
        where TDerived : notnull
    {
        builder.RegisterType<TDerived>()
            .ApplyContract(typeof(TBase), contractKey, CreationPolicy.Scoped);
    }

    public static void AddTransient<TType>(this ContainerBuilder builder)
        where TType : notnull
    {
        builder.RegisterType<TType>()
            .WithAttributeFiltering();
    }

    public static void AddTransient<TBase, TDerived>(this ContainerBuilder builder,
            object? contractKey = null)
        where TBase : notnull
        where TDerived : notnull
    {
        builder.RegisterType<TDerived>()
            .ApplyContract(typeof(TBase), contractKey, CreationPolicy.Singleton);
    }

    public static void AddTransientGeneric(this ContainerBuilder builder,
        Type baseType,
        Type derivedType,
        object? contractKey = null)
    {
        if(!baseType.IsGenericType || !derivedType.IsGenericType)
        {
            throw new ArgumentException("Both base and derived types must be generic.");
        }

        builder.RegisterGeneric(derivedType)
            .ApplyContract(baseType, contractKey);
    }

    public static void AddAttributedComponentsFromAssembly(this ContainerBuilder builder, 
        Assembly assembly)
    {
        var imports = assembly.GetTypes()
            .SelectWhere(static (Type type, out (Type type, ExportAttribute exportData) typeAttribute) =>
            {
                typeAttribute = (type, type.GetCustomAttribute<ExportAttribute>()!);
                return typeAttribute.exportData is not null;
            });

        foreach(var (type, exportData) in imports)
        {
            BuildFromAttribute(builder, type, exportData);
        }
    }

    private static void BuildFromAttribute(ContainerBuilder builder, 
        Type import, 
        ExportAttribute exportData)
    {
        if(exportData is ExportGenericAttribute)
        {
            if (!import.IsGenericType)
            {
                throw new ArgumentException("Cannot use non-generic type for generic export.");
            }

            builder.RegisterGeneric(import)
                .ApplyContract(exportData.ContractType, exportData.ContractKey, exportData.CreationPolicy);
        }
        else
        {
            if (!import.IsGenericTypeDefinition && exportData.ContractType.IsGenericTypeDefinition)
            {
                throw new ArgumentException("Cannot create export from generic types. Use ExportGeneric instead");
            }

            builder.RegisterType(import)
                .ApplyContract(exportData.ContractType, exportData.ContractKey, exportData.CreationPolicy);
        }
    }

    private static IRegistrationBuilder<TDerived, TReflectionActivatorData, TRegistrationStyle> ApplyContract<TDerived, TReflectionActivatorData, TRegistrationStyle>(this IRegistrationBuilder<TDerived, TReflectionActivatorData, TRegistrationStyle> regBuilder,
            Type contractType,
            object? contractKey = null,
            CreationPolicy creationPolicy = CreationPolicy.Transient)
        where TReflectionActivatorData : ReflectionActivatorData
    {
        if(contractKey is null)
        {
            regBuilder.As(contractType);
        }
        else
        {
            regBuilder.Keyed(contractKey, contractType);
        }

        switch (creationPolicy)
        {
            case CreationPolicy.Scoped:
                regBuilder.InstancePerLifetimeScope();
                break;

            case CreationPolicy.Singleton:
                regBuilder.SingleInstance();
                break;
        }

        return regBuilder.WithAttributeFiltering();
    }
    #endregion
}
