using AlbaranApi.Contracts;
using AlbaranApi.Publisher;
using AlbaranApi.Repository;
using AlbaranApi.Services;
using Autofac;
using Inventario.MessageBus;
using Microsoft.Extensions.Configuration;

namespace AlbaranApi.Autofac
{
    public class AutoFacModule :
        Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterBus(builder);
            RegisterServices(builder);
        }

        private static void RegisterBus(ContainerBuilder builder)
        {
            builder.RegisterMessageBus(context =>
                {
                    var configuration = context.Resolve<IConfiguration>();

                    var connectionOptions = new ConnectionsOptions
                    {
                        Host = configuration.GetSection("MessageBus:Host").Value,
                        Username = configuration.GetSection("MessageBus:Username").Value,
                        Password = configuration.GetSection("MessageBus:Password").Value
                    };
                    return connectionOptions;
                }
            );
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<PublishDomainEventResultConsumer>()
                .As<IDomainEventResultPublisher>()
                .SingleInstance();
            builder.RegisterType<QrServices>()
                .As<IQrService>()
                .SingleInstance();
            builder.RegisterType<Handler.Handler>()
                .As<IHandler>()
                .SingleInstance();
            builder.RegisterType<EntradaRepository>()
                .As<IEntradaRepository>()
                .SingleInstance();


            builder
                .Register(context =>
                {
                    var configuration = context.Resolve<IConfiguration>();
                    var connectionString = configuration.GetSection("MongoDb").Value;
                    var mongoDatabaseFactory = new MongoDatabaseFactory(connectionString);

                    return mongoDatabaseFactory;
                })
                .As<IMongoDatabaseFactory>()
                .SingleInstance();
        }
    }
}