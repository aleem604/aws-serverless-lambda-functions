using TinCore.Domain.Core.Notifications;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using TinCore.Application.Interfaces;
using TinCore.Application.Services;
using TinCore.Domain.Events;
using TinCore.Domain.EventHandlers;
using TinCore.Domain.Commands;
using TinCore.Domain.CommandHandlers;
using TinCore.Domain.Interfaces;
using TinCore.Infra.Data.Repository;
using TinCore.Infra.Data.Context;
using TinCore.Domain.Core.Bus;
using TinCore.Infra.CrossCutting.Bus;
using TinCore.Infra.Data.UoW;
using TinCore.Domain.Core.Models;

namespace TinCore.Infra.CrossCutting.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // ASP.NET HttpContext dependency
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, InMemoryBus>();
            RegisterApplicationServices(services);

            RegisterCommandAndEvents(services);

            RegisterRepos(services);
            // Infra - Data
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<TinCoreContext>();

        }

        private static void RegisterRepos(IServiceCollection services)
        {
            // Infra - repos
            services.AddScoped<ISubscriptionTemplateRepo, SubscriptionTemplateRepo>();
            services.AddScoped<ISubscriptionRepo, SubscriptionRepo>();
            services.AddScoped<IEntitySubscriptionTrackingRepo, EntitySubscriptionTrackingRepo>();
            services.AddScoped<ITinContactRepo, ContactRepository>();
            services.AddScoped<IProfileAttributeRepo, ProfileAttributeRepository>();
            services.AddScoped<IEntityRepo, EntityRepository>();
            services.AddScoped<ILocationProgramRepo, LocationProgramRepository>();
            services.AddScoped<IAttributeRepo, AttributeRepo>();

            // classification repo
            services.AddScoped<IClassificationRepo, ClassificationRepository>();
            services.AddScoped<IClassificationTypeRepo, ClassificationTypeRepository>();
            services.AddScoped<IVClassificationRepo, VClassificationRepository>();

            // relation repos
            services.AddScoped<IRelationEntityRelationRepo, RelationEntityRelationRepo>();
            services.AddScoped<IEntityAttributeRelationRepo, EntityAttributeRelationRepo>();
            services.AddScoped<ILocationEntityRelationRepo, LocationEntityRelationRepo>();
            services.AddScoped<ILocationCategoryRelationRepo, LocationCategoryRelationRepo>();
            services.AddScoped<IVRelationRepo, VRelationRepo>();

            // location repos
            services.AddScoped<IVLocationRepo, VLocationRepo>();
            services.AddScoped<ICountryRepo, CountryRepo>();
            services.AddScoped<ICityRepo, CityRepo>();
            services.AddScoped<IRegionRepo, RegionRepo>();
            services.AddScoped<INeighbourhoodRepo, NeighbourhoodRepo>();

        }

        private static void RegisterApplicationServices(IServiceCollection services)
        {
            // Application
            services.AddScoped<ICommonService, CommonService>();
            //services.AddScoped<IEntityObjectService, EntityObjectService>();
            //services.AddScoped<IRelationService, RelationService>();
            //services.AddScoped<ITinProfileService, TinProfileService>();
            //services.AddScoped<ITrackingService, TrackingService>();
            services.AddScoped<ILocationService, LocationService>();
            //services.AddScoped<IProfileAttributeService, ProfileAttributeService>();
            services.AddScoped<IEntityService, EntityService>();
            //services.AddScoped<ILocationProgramService, LocationProgramService>();
        }

        private static void RegisterCommandAndEvents(IServiceCollection services)
        {
            // Domain - Events
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<INotificationHandler<LocationRegisteredEvent>, LocationEventHandler>();
            services.AddScoped<INotificationHandler<LocationUpdatedEvent>, LocationEventHandler>();
            services.AddScoped<INotificationHandler<LocationRemovedEvent>, LocationEventHandler>();

            // Location - Commands
            //services.AddScoped<IRequestHandler<NewLocationCommand, bool>, LocationCommandHandler>();
            //services.AddScoped<IRequestHandler<UpdateLocationCommand, bool>, LocationCommandHandler>();
            //services.AddScoped<IRequestHandler<RemoveLocationCommand, bool>, LocationCommandHandler>();

            // Entity - Commands
            services.AddScoped<IRequestHandler<NewEntityCommand, bool>, EntityCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateEntityCommand, bool>, EntityCommandHandler>();
            services.AddScoped<IRequestHandler<RemoveEntityCommand, bool>, EntityCommandHandler>();

            // Entity - Events
            services.AddScoped<INotificationHandler<BusinessRegisteredEvent>, EntityEventHandler>();
            services.AddScoped<INotificationHandler<BusinessUpdatedEvent>, EntityEventHandler>();
            services.AddScoped<INotificationHandler<BusinessRemovedEvent>, EntityEventHandler>();
        }
    }
}