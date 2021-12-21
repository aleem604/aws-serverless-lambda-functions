using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Configuration;
using TinCore.Application.Interfaces;
using TinCore.Application.ViewModels;
using TinCore.Common;
using TinCore.Common.Enums;
using TinCore.Domain.Commands;
using TinCore.Domain.Core.Bus;
using TinCore.Domain.Core.Notifications;
using TinCore.Domain.Interfaces;
using TinCore.Domain.Models;

namespace TinCore.Application.Services
{
    public class RelationService : IRelationService
    {
        private readonly IMapper _mapper;
        private readonly IRelationEntityRelationRepo _relationEntityRepo;
        private readonly IEntityAttributeRelationRepo _entityAttributeRepo;
        private readonly ILocationEntityRelationRepo _locationEntityRepo;
        private readonly ILocationCategoryRelationRepo _locationCategoryRepo;
        private readonly IVLocationRepo _locationRepo;
        private readonly IClassificationRepo _classificationRepo;
        private readonly IClassificationTypeRepo _classificationTypeRepo;
        private readonly IConfiguration _configuration;
        private readonly IMediatorHandler _bus;

        public RelationService(IMapper mapper,
            IRelationEntityRelationRepo relationEntityRepo,
            IEntityAttributeRelationRepo entityAttributeRepo,
            ILocationEntityRelationRepo locationEntityRepo,
            ILocationCategoryRelationRepo locationCategoryRepo,
            IClassificationRepo classificationRepo,
            IClassificationTypeRepo classificationTypeRepo,
            IVLocationRepo locationRepo,
            IConfiguration configuration,
            IMediatorHandler bus)
        {
            _mapper = mapper;
            _relationEntityRepo = relationEntityRepo;
            _entityAttributeRepo = entityAttributeRepo;
            _locationEntityRepo = locationEntityRepo;
            _locationCategoryRepo = locationCategoryRepo;
            _locationRepo = locationRepo;
            _classificationRepo = classificationRepo;
            _classificationTypeRepo = classificationTypeRepo;
            _configuration = configuration;
            _bus = bus;
        }

        public dynamic GetReviews(long id)
        {
            var result = new Dictionary<string, dynamic>();
            //var section = _configuration.GetSection("proman_codes");

            //var getRow = _eoRepository.GetAll().Where(x => x.isn == id).Select((o) => new { entity_id = o.isn, entity_name = o.long_name }).FirstOrDefault();
            //result.Add("entity_id", getRow.entity_id);
            //result.Add("entity_name", getRow.entity_name);

            //var categories = (from r1 in _relationRepository.GetAll()
            //                  join r2 in _relationRepository.GetAll() on r1.eo1_isn equals r2.isn
            //                  join eo in _eoRepository.GetAll() on r2.eo2_isn equals eo.isn
            //                  where r1.eo2_isn == id
            //                  select new
            //                  {
            //                      entity_category_id = r2.eo2_isn,
            //                      entity_category_name = eo.name1
            //                  }).ToList();
            //result.Add("categories", categories);

            //var comments = (from eo in _eoRepository.GetAll()
            //                join p in _profileRepository.GetAll() on eo.isn equals p.profile1
            //                where p.profile_type == section.GetValue<short>("PROFILE~REVIEW")
            //                && p.profile_subtype == section.GetValue<short>("PROFILE~1")
            //                && p.eo_isn == (int)id
            //                && p.eo_type == section.GetValue<short>("EO_TYPE~PERSON")
            //                && p.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
            //                orderby p.isn
            //                select new
            //                {
            //                    comment_id = p.isn,
            //                    timestamp = p.profile8,
            //                    comments_by_id = p.profile1,
            //                    user_name = $"{eo.name3} {eo.name2}",
            //                    rating = p.profile2,
            //                    desc1 = p.profile6,
            //                    comment_text = Utils.ProcessXML(p.profile9, "desc2"),
            //                    verified = false,
            //                    photo_urls = Utils.GetListFromXML(p.profile9, "photo_urls")
            //                }).ToList();

            //result.Add("comments", comments);
            return result;
        }
        public void SentRelationEntityRelation(TinEntity entity, long locationId, long categoryId)
        {
             var relationObject = new LocationCategoryRelation
            {
                Entity2Id = entity.Id,
                Entity2Type = eEoTypes.ENTITY,
                Entity2Subtype = eEoSubTypes.INSTANCE,
                Entity2Name = entity.Name
            };

            long? location_id = 0;
            if (locationId == 0)
            {
                location_id = (from l in _locationRepo.GetAll()
                               where l.Name.Equals(entity.Contact.City, StringComparison.CurrentCultureIgnoreCase)
                               select new { l.Id }).FirstOrDefault()?.Id;


            }
            locationId = location_id ?? 2830;

            long? cat_id = 0;
            if (categoryId == 0)
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
            categoryId = cat_id.HasValue ? cat_id.Value : 6144;

            long loc_cat_rel_id = 0;
            var relationEntity = (from r in _locationCategoryRepo.GetAll()
                                  where r.Entity1Id == locationId && r.Entity1Type == eEoTypes.LOCATION && r.Entity1Subtype == eEoSubTypes.INSTANCE
                                  && r.Entity2Id == categoryId && r.Entity2Type == eEoTypes.CLASSIFICATION && r.Entity2Subtype == eEoSubTypes.INSTANCE
                                  && r.Status == eRecordStatus.Active
                                  select new { r.Id, r.Entity1Name, r.Entity2Name }
                                ).FirstOrDefault();
            if (relationEntity is null)
            {
                relationEntity = (from r in _locationCategoryRepo.GetAll()
                                  where r.Entity1Id == locationId && r.Entity1Type == eEoTypes.LOCATION && r.Entity1Subtype == eEoSubTypes.INSTANCE
                                  && r.Entity2Id == categoryId && r.Entity2Type == eEoTypes.CLASSIFICATIONTYPE && r.Entity2Subtype == eEoSubTypes.INSTANCE
                                  && r.Status == eRecordStatus.Active
                                  select new { r.Id, r.Entity1Name, r.Entity2Name }
                                  ).FirstOrDefault();
            }

            if (relationEntity != null)
            {
                loc_cat_rel_id = relationEntity.Id;
                relationObject.Entity1Name = $"{relationEntity.Entity1Name}~{relationEntity.Entity2Name}";
            }

            var relationRecord = (from r in _locationCategoryRepo.GetAll()
                                  where r.Entity1Id == loc_cat_rel_id
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

                _locationCategoryRepo.Update(relationRecord);
                
            }
            else if (loc_cat_rel_id > 0)
            {
                relationObject.Entity1Id = loc_cat_rel_id;
                relationObject.Entity1Type = eEoTypes.LOCATION_CATEGORY;
                relationObject.Entity1Subtype = eEoSubTypes.INSTANCE;
                relationObject.RelationType = eRelationTypes.RELATION_RELATION;
                relationObject.RelationSubtype = eEoSubTypes.INSTANCE;
                relationObject.Status = eRecordStatus.Active;
                _locationCategoryRepo.Add(relationObject);
                
            }
            if(!(_locationCategoryRepo.SaveChanges()>0))
                _bus.RaiseEvent(new DomainNotification(GetType().Name, "An error occurred while processing your request. Please try again later."));
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
