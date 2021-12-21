using System;
using System.Threading;
using System.Threading.Tasks;
using TinCore.Domain.Commands;
using TinCore.Domain.Core.Bus;
using TinCore.Domain.Core.Notifications;
using TinCore.Domain.Events;
using TinCore.Domain.Interfaces;
using TinCore.Domain.Models;
using MediatR;
using AutoMapper;
using TinCore.Common.Enums;
using System.Linq;

namespace TinCore.Domain.CommandHandlers
{
    public class RelationCommandHandler : CommandHandler,
        IRequestHandler<RelationEntityRelationCommand, bool>,
        IRequestHandler<EntityAttributeRelationCommand, bool>,
        IRequestHandler<LocationEntityRelationCommand, bool>,
        IRequestHandler<LocationCategoryRelationCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly IEntityRepo _entityRepo;
        private readonly IVLocationRepo _locationRepo;
        private readonly IClassificationRepo _classificationRepo;
        private readonly IClassificationTypeRepo _classificationTypeRepo;
        private readonly IEntitySubscriptionTrackingRepo _trackingRepo;
        private readonly ILocationCategoryRelationRepo _locationCategoryRelationRepo;
        private readonly ILocationEntityRelationRepo _locationEntityRelationRepo;
        private readonly IMediatorHandler _bus;

        public RelationCommandHandler(IMapper mapper,
            IEntityRepo entityRepository,
            IVLocationRepo locationRepository,
            IClassificationRepo classificationRepo,
            IClassificationTypeRepo classificationTypeRepo,
            IEntitySubscriptionTrackingRepo trackingRepo,
            ILocationCategoryRelationRepo locatoinCategoryRelationRepo,
            ILocationEntityRelationRepo locatoinEntityRelationRepo,
            IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications)
            : base(uow, bus, notifications)
        {
            _mapper = mapper;
            _entityRepo = entityRepository;
            _locationRepo = locationRepository;
            _classificationRepo = classificationRepo;
            _classificationTypeRepo = classificationTypeRepo;
            _trackingRepo = trackingRepo;
            _locationCategoryRelationRepo = locatoinCategoryRelationRepo;
            _locationEntityRelationRepo = locatoinEntityRelationRepo;
            _bus = bus;
        }

        public Task<bool> Handle(RelationEntityRelationCommand message, CancellationToken cancellationToken)
        {
            
            return Task.FromResult(true);
        }

        
        public Task<bool> Handle(EntityAttributeRelationCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            _entityRepo.Remove(message.Id);

            if (Commit())
            {
                _bus.RaiseEvent(new BusinessRemovedEvent(message.Id));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(LocationEntityRelationCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        public Task<bool> Handle(LocationCategoryRelationCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
        #region private methods
        // set locatoin category relation  eRelationTypes.LOCATION_CATEGORY
        private void SentLocationCategoryRelation(NewEntityCommand message, TinEntity entity)
        {
            var relationObject = new LocationCategoryRelation
            {
                Entity2Id = entity.Id,
                Entity2Name = entity.Name
            };

            long? location_id = 0;
            if (message.Location_Id == 0)
            {
                location_id = (from l in _locationRepo.GetAll()
                               where l.Name.Equals(entity.Contact.City, StringComparison.CurrentCultureIgnoreCase)
                               select new { l.Id }).FirstOrDefault()?.Id;


            }
            message.Location_Id = location_id ?? 2830;

            long? cat_id = 0;
            if (message.Category_Id == 0)
            {
                cat_id = (from x in _classificationRepo.GetAll()
                          where x.Name.Equals(entity.Contact.City, StringComparison.CurrentCultureIgnoreCase)
                          select new { x.Id }).FirstOrDefault()?.Id;
            }
            if (!cat_id.HasValue)
            {
                cat_id = (from x in _classificationTypeRepo.GetAll()
                          where x.Name.Equals(entity.Contact.City, StringComparison.CurrentCultureIgnoreCase)
                          select new { x.Id }).ToList().FirstOrDefault()?.Id;
            }
            message.Category_Id = cat_id.HasValue ? cat_id.Value : 6144;

            long loc_cat_rel_id = 0;
            var relationEntity = (from r in _locationCategoryRelationRepo.GetAll()
                                  where r.RelationType == eRelationTypes.LOCATION_CATEGORY && r.RelationSubtype == eEoSubTypes.INSTANCE
                                  && r.Entity1Id == message.Location_Id && r.Entity1Type == eEoTypes.LOCATION && r.Entity1Subtype == eEoSubTypes.INSTANCE
                                  && r.Entity2Id == message.Category_Id && r.Entity2Type == eEoTypes.CLASSIFICATION && r.Entity2Subtype == eEoSubTypes.INSTANCE
                                  && r.Status == eRecordStatus.Active
                                  select new { r.Id, r.Entity1Name, r.Entity2Name }
                                ).FirstOrDefault();
            if (relationEntity is null)
            {
                relationEntity = (from r in _locationCategoryRelationRepo.GetAll()
                                  where r.RelationType == eRelationTypes.LOCATION_CATEGORY && r.RelationSubtype == eEoSubTypes.INSTANCE
                                  && r.Entity1Id == message.Location_Id && r.Entity1Type == eEoTypes.LOCATION && r.Entity1Subtype == eEoSubTypes.INSTANCE
                                  && r.Entity2Id == message.Category_Id && r.Entity2Type == eEoTypes.CLASSIFICATIONTYPE && r.Entity2Subtype == eEoSubTypes.INSTANCE
                                  && r.Status == eRecordStatus.Active
                                  select new { r.Id, r.Entity1Name, r.Entity2Name }
                                  ).FirstOrDefault();
            }

            if (relationEntity != null)
            {
                loc_cat_rel_id = relationEntity.Id;
                relationObject.Entity1Name = $"{relationEntity.Entity1Name}~{relationEntity.Entity2Name}";
            }

            var relationRecord = (from r in _locationCategoryRelationRepo.GetAll()
                                  where r.Entity1Type == eEoTypes.LOCATION_CATEGORY && r.Entity1Subtype == eEoSubTypes.INSTANCE
                                  && r.Entity1Id == loc_cat_rel_id
                                  && r.Entity2Id == entity.Id && r.Entity2Type == eEoTypes.ENTITY && r.Entity2Subtype == eEoSubTypes.INSTANCE
                                  && r.RelationType == eRelationTypes.RELATION_RELATION && r.RelationSubtype == eEoSubTypes.INSTANCE
                                  && r.Status == eRecordStatus.Active
                                  select r
                                ).FirstOrDefault();
            if (!(relationRecord is null))
            {
                relationRecord.Entity1Name = relationObject.Entity1Name;
                relationRecord.UpdateDate = DateTime.Now;
                relationRecord.UpdatedBy = 0;

                _locationCategoryRelationRepo.Update(relationRecord);
                Commit();
            }
            else if (loc_cat_rel_id > 0)
            {
                relationObject.Entity1Id = loc_cat_rel_id;
                relationObject.Entity1Type = eEoTypes.LOCATION_CATEGORY;
                relationObject.Entity1Subtype = eEoSubTypes.INSTANCE;
                relationObject.RelationType = eRelationTypes.RELATION_RELATION;
                relationObject.RelationSubtype = eEoSubTypes.INSTANCE;
                relationObject.Status = eRecordStatus.Active;
                _locationCategoryRelationRepo.Add(relationObject);
                Commit();
            }
        }

        // set  entity_tracking
        private void SetEntitySubscription(long entityId, long subscriptionId = 63611)
        {
            var trackingEntity = new EntitySubscriptionTracking();
            var trackingRec = (from t in _trackingRepo.GetAll()
                               where t.EntityId == entityId && t.Status == eRecordStatus.Active
                               select t).FirstOrDefault();
            if (trackingRec != null)
            {

            }
            else
            {
                trackingEntity.EntityId = entityId;
                trackingEntity.SubscriptionId = subscriptionId;
                DateTime.TryParse("2018-01-01", out DateTime startDate);
                trackingEntity.StartDate = startDate;
                DateTime.TryParse("2020-12-31", out DateTime endDate);
                trackingEntity.EndDate = endDate;
                trackingEntity.Status = eRecordStatus.Active;
                _trackingRepo.Add(trackingEntity);
                Commit();
            }

        }
        private void SetLocationEntityRelation(long location_id, long entityId, string city_Featured, string region_Featured, string city_Donotmiss, string region_Donotmiss)
        {
            var entity = _entityRepo.GetById(entityId);
            if (entity is null) return;

            var ralationId = (from r in _locationEntityRelationRepo.GetAll()
                              where r.Entity1Id == location_id && r.Entity1Type == eEoTypes.LOCATION && r.Entity1Subtype == eEoSubTypes.INSTANCE
                              && r.Entity2Id == entity.Id && r.Entity2Type == eEoTypes.ENTITY && r.Entity2Subtype == eEoSubTypes.INSTANCE
                              && r.RelationType == eRelationTypes.LOCATION_EO && r.RelationSubtype == eEoSubTypes.INSTANCE
                              select new { r.Id }).FirstOrDefault()?.Id;
            if (ralationId.HasValue)
            {
                // TODO: what to do whe existing relation 
                // what type of update is needed

            }
            else
            {
                var relationObject = new LocationEntityRelation
                {
                    Entity2Id = entity.Id,
                    Entity2Name = entity.Name,
                    Status = eRecordStatus.Active
                };

                var locationEntity = _locationRepo.GetById(location_id);
                if (!(locationEntity is null))
                {
                    relationObject.Entity1Id = locationEntity.Id;
                    relationObject.Entity1Name = locationEntity.Name;
                }
                _locationEntityRelationRepo.Add(relationObject);
                Commit();

            }
        }

        private void SetLocationEntities(long location_id, long entityId, string city_featured, string region_featured, string city_donotmiss, string region_donotmiss)
        {
            VLocation city = null;
            VLocation region = null;
            var locationDetail = _locationRepo.GetById(location_id);
            if (locationDetail is null) return;
            if (locationDetail.LocationType == eLocationType.Neighbourhood)
            {
                city = _locationRepo.GetById(locationDetail.ParentId.Value);
            }
            else
            {
                city = locationDetail;
            }
            region = _locationRepo.GetById(city.ParentId.Value);

            if (!string.IsNullOrEmpty(city_featured))
            {
                city.FeaturedEntities = city_featured;
            }
            if (!string.IsNullOrEmpty(region_featured))
            {
                region.FeaturedEntities = region_featured;
            }

            if (!string.IsNullOrEmpty(city_donotmiss))
            {
                //city.DonotmissEntities = city_donotmiss;
            }
            if (!string.IsNullOrEmpty(region_donotmiss))
            {
                //region.DonotmissEntities = region_donotmiss;
            }

            _locationRepo.Update(city);
            _locationRepo.Update(region);
            Commit();

        }
        #endregion private methods

        public void Dispose()
        {
            _entityRepo.Dispose();
            _locationRepo.Dispose();
            _classificationRepo.Dispose();
            _classificationTypeRepo.Dispose();
            _locationCategoryRelationRepo.Dispose();
            _locationEntityRelationRepo.Dispose();
            _trackingRepo.Dispose();

        }

    }
}