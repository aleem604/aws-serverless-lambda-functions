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
    public class EntityCommandHandler : CommandHandler,
        IRequestHandler<NewEntityCommand, bool>,
        IRequestHandler<UpdateEntityCommand, bool>,
        IRequestHandler<RemoveEntityCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly IEntityRepo _entityRepo;
        private readonly IVLocationRepo _vLocationRepo;
        private readonly IClassificationRepo _classificationRepo;
        private readonly IClassificationTypeRepo _classificationTypeRepo;
        private readonly IRelationEntityRelationRepo _relationEntityRepo;
        private readonly ILocationEntityRelationRepo _locationEntityRepo;
        private readonly IEntitySubscriptionTrackingRepo _trackingRepo;
        private readonly IMediatorHandler _bus;

        public EntityCommandHandler(IMapper mapper,
            IEntityRepo entityRepository,
            IVLocationRepo vLocationRepository,
            IClassificationRepo classificationRepo,
            IClassificationTypeRepo classificationTypeRepo,
            IRelationEntityRelationRepo relationEntityRepo,
            ILocationEntityRelationRepo locationEntityRepo,
            IEntitySubscriptionTrackingRepo trackingRepo,
            IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications)
            : base(uow, bus, notifications)
        {
            _mapper = mapper;
            _entityRepo = entityRepository;
            _vLocationRepo = vLocationRepository;
            _classificationRepo = classificationRepo;
            _classificationTypeRepo = classificationTypeRepo;
            _relationEntityRepo = relationEntityRepo;
            _locationEntityRepo = locationEntityRepo;
            _trackingRepo = trackingRepo;
            _bus = bus;
        }

        public Task<bool> Handle(NewEntityCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }

            var entity = _mapper.Map<TinEntity>(message);
            entity.Status = eRecordStatus.Active;
            entity.CreateDate = DateTime.Now;
            entity.CreatedBy = 1;
            _entityRepo.Add(entity);
            if (Commit())
            {
                _bus.RaiseEvent(new BusinessRegisteredEvent(entity.Id, entity.Name));
                // set entity category
                SentEntityCategory(message, entity);
                // set entity subscription 
                SetEntitySubscription(entity.Id);

                if (message.Location_Id > 0)
                    SetEntityLocation(message.Location_Id, entity.Id, message.City_Featured, message.Region_Featured, message.City_Donotmiss, message.Region_Donotmiss);
            }
            else
            {
                _bus.RaiseEvent(new DomainNotification(message.MessageType, "An error occurred while processing your request. Please try again later."));
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(UpdateEntityCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
            {
                NotifyValidationErrors(message);
                return Task.FromResult(false);
            }
            var entity = _mapper.Map<TinEntity>(message);

            var dbEntity = _entityRepo.GetEntity(message.Id, IncludeOptions.All);
            if (dbEntity is null)
            {
                _bus.RaiseEvent(new DomainNotification(message.MessageType, "Invalid request for update record."));
                return Task.FromResult(false);
            }

            dbEntity.Name = entity.Name;
            dbEntity.FullName = entity.FullName;
            dbEntity.ShortName = entity.ShortName;
            dbEntity.Tag = entity.Tag;
            dbEntity.Description = entity.Description;
            dbEntity.Desc1 = entity.Desc1;
            dbEntity.Desc2 = entity.Desc2;
            dbEntity.Desc3 = entity.Desc3;
            dbEntity.VideoUrl = entity.VideoUrl;
            dbEntity.UpdateDate = DateTime.Now;
            dbEntity.UpdatedBy = 0;

            if (dbEntity.Contact is null)
            {
                dbEntity.Contact = new EntityContact
                {
                    EntityId = dbEntity.Id
                };
            }

            dbEntity.Contact.Address = entity.Contact.Address;
            dbEntity.Contact.Address2 = entity.Contact.Address2;
            dbEntity.Contact.City = entity.Contact.City;
            dbEntity.Contact.State = entity.Contact.State;
            dbEntity.Contact.Country = entity.Contact.Country;
            dbEntity.Contact.Phone = entity.Contact.Phone;
            dbEntity.Contact.Email = entity.Contact.Email;
            dbEntity.Contact.Website = entity.Contact.Website;
            dbEntity.Contact.UpdateDate = DateTime.Now;
            dbEntity.Contact.UpdatedBy = 0;

            if (dbEntity.ProfileAttribute is null)
            {
                dbEntity.ProfileAttribute = new ProfileAttribute
                {
                    EntityId = dbEntity.Id
                };
            }
            dbEntity.ProfileAttribute.Desc1 = entity.ProfileAttribute.Desc1;
            dbEntity.ProfileAttribute.Desc2 = entity.ProfileAttribute.Desc2;
            dbEntity.ProfileAttribute.ImageUrl = entity.ProfileAttribute.ImageUrl;
            dbEntity.ProfileAttribute.Longitude = entity.ProfileAttribute.Longitude;
            dbEntity.ProfileAttribute.Latitude = entity.ProfileAttribute.Latitude;
            dbEntity.ProfileAttribute.Price = entity.ProfileAttribute.Price;

            if (dbEntity.ProfileReview is null)
            {
                dbEntity.ProfileReview = new ProfileReview
                {
                    EntityId = dbEntity.Id
                };
            }
            dbEntity.ProfileReview.ReviewedBy = 0;
            dbEntity.ProfileReview.ReviewedBy = entity.ProfileReview.ReviewedBy;
            dbEntity.ProfileReview.Stars = entity.ProfileReview.Stars;
            dbEntity.ProfileReview.ReviewDate = DateTime.Now;
            dbEntity.ProfileReview.Desc1 = entity.ProfileReview.Desc1;
            dbEntity.ProfileReview.Desc2 = entity.ProfileReview.Desc2;
            dbEntity.ProfileReview.ImageUrl = entity.ProfileReview.ImageUrl;

            if (dbEntity.ProfileSection is null)
            {
                dbEntity.ProfileSection = new ProfileSection
                {
                    EntityId = dbEntity.Id
                };
            }
            dbEntity.ProfileSection.Name = dbEntity.ProfileSection.Name;
            dbEntity.ProfileSection.Description = dbEntity.ProfileSection.Description;
            dbEntity.ProfileSection.ImageUrl = dbEntity.ProfileSection.ImageUrl;

            _entityRepo.Update(dbEntity);

            if (Commit())
            {
                _bus.RaiseEvent(new BusinessUpdatedEvent(entity.Id, entity.Name));
            }

            return Task.FromResult(true);
        }

        public Task<bool> Handle(RemoveEntityCommand message, CancellationToken cancellationToken)
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

        #region private methods
        private void SentEntityCategory(NewEntityCommand message, TinEntity entity)
        {
            var relationObject = new RelationEntityRelation
            {
                Entity2Id = entity.Id,
                Entity2Type = eEoTypes.ENTITY,
                Entity2Subtype = eEoSubTypes.INSTANCE,
                Entity2Name = entity.Name
            };

            long? location_id = 0;
            if (message.Location_Id == 0)
            {
                location_id = (from l in _vLocationRepo.GetAll()
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
            message.Category_Id = cat_id ?? 6144;

            long loc_cat_rel_id = 0;
            var relationEntity = (from r in _relationEntityRepo.GetAll()
                                  where r.RelationType == eRelationTypes.LOCATION_CATEGORY && r.RelationSubtype == eEoSubTypes.INSTANCE
                                  && r.Entity1Id == message.Location_Id && r.Entity1Type == eEoTypes.LOCATION && r.Entity1Subtype == eEoSubTypes.INSTANCE
                                  && r.Entity2Id == message.Category_Id && r.Entity2Type == eEoTypes.CLASSIFICATION && r.Entity2Subtype == eEoSubTypes.INSTANCE
                                  && r.Status == eRecordStatus.Active
                                  select new { r.Id, r.Entity1Name, r.Entity2Name }
                                ).FirstOrDefault();
            if (relationEntity is null)
            {
                relationEntity = (from r in _relationEntityRepo.GetAll()
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

            var relationRecord = (from r in _relationEntityRepo.GetAll()
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

                _relationEntityRepo.Update(relationRecord);
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
                _relationEntityRepo.Add(relationObject);
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
            if (trackingRec is null)
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
            else // what to update
            {
                
            }

        }
        private void SetEntityLocation(long location_id, long entityId, string city_Featured, string region_Featured, string city_Donotmiss, string region_Donotmiss)
        {
            var entity = _entityRepo.GetById(entityId);
            if (entity is null) return;

            var ralationId = (from r in _relationEntityRepo.GetAll()
                              where r.Entity1Id == location_id && r.Entity1Type == eEoTypes.LOCATION && r.Entity1Subtype == eEoSubTypes.INSTANCE
                              && r.Entity2Id == entity.Id && r.Entity2Type == eEoTypes.ENTITY && r.Entity2Subtype == eEoSubTypes.INSTANCE
                              && r.RelationType == eRelationTypes.LOCATION_EO && r.RelationSubtype == eEoSubTypes.INSTANCE
                              select new { r.Id }).FirstOrDefault();
            if (ralationId is null)
            {
                

            }
            else
            {
                var relationObject = new LocationEntityRelation
                {
                    RelationType = eRelationTypes.LOCATION_EO,
                    RelationSubtype = eEoSubTypes.INSTANCE,
                    Entity2Id = entity.Id,
                    Entity2Type = eEoTypes.ENTITY,
                    Entity2Subtype = eEoSubTypes.INSTANCE,
                    Entity2Name = entity.Name,
                    Status = eRecordStatus.Active
                };

                var locationEntity = _vLocationRepo.GetById(location_id);
                if (!(locationEntity is null))
                {
                    relationObject.Entity1Id = locationEntity.Id;
                    relationObject.Entity1Type = eEoTypes.LOCATION;
                    relationObject.Entity1Subtype = eEoSubTypes.INSTANCE;
                    relationObject.Entity1Name = locationEntity.Name;
                }
                _locationEntityRepo.Add(relationObject);
                Commit();

            }
        }

        private void SetLocationEntities(long location_id, long entityId, string city_featured, string region_featured, string city_donotmiss, string region_donotmiss)
        {
            VLocation city = null;
            VLocation region = null;
            var locationDetail = _vLocationRepo.GetById(location_id);
            if (locationDetail is null) return;
            if (locationDetail.LocationType == eLocationType.Neighbourhood)
            {
                city = _vLocationRepo.GetById(locationDetail.ParentId.Value);
            }
            else
            {
                city = locationDetail;
            }
            region = _vLocationRepo.GetById(city.ParentId.Value);

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

            _vLocationRepo.Update(city);
            _vLocationRepo.Update(region);
            Commit();

        }
        #endregion private methods

        public void Dispose()
        {
            _entityRepo.Dispose();
            _vLocationRepo.Dispose();
            _classificationRepo.Dispose();
            _classificationTypeRepo.Dispose();
            _relationEntityRepo.Dispose();
            _trackingRepo.Dispose();

        }
    }
}