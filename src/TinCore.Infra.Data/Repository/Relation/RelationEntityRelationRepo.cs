using System.Linq;
using TinCore.Domain.Interfaces;
using TinCore.Domain.Models;
using TinCore.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using TinCore.Domain.Core.Models;
using System.Collections.Generic;
using TinCore.Common.CommonModels;
using Microsoft.Extensions.Configuration;
using System;
using TinCore.Common.Enums;

namespace TinCore.Infra.Data.Repository
{
    public class RelationEntityRelationRepo : Repository<RelationEntityRelation>, IRelationEntityRelationRepo
    {
        private readonly IConfiguration _configuration;
        private readonly IVClassificationRepo _vClassificationRepository;

        public RelationEntityRelationRepo(TinCoreContext context, IConfiguration configuration, IVClassificationRepo vClassificationRepository)
            : base(context)
        {
            _configuration = configuration;
            _vClassificationRepository = vClassificationRepository;
        }

        public dynamic GetCategoriesForLocationFeaturedEntities(List<long> entityIds)
        {
            return (from a in DbSet.AsNoTracking()
                    join b in DbSet.AsNoTracking() on a.Entity1Id equals b.Id
                    where entityIds.Contains(a.Entity2Id)
                    select new { id = a.Entity1Id, cat_id = b.Entity2Id, cat_name = b.Entity2Name })
                .Select((o) => new
                {
                    EntityObjectId = o.id,
                    CategoryId = o.cat_id,
                    CategoryName = o.cat_name
                }).ToList();
        }

        public dynamic GetLocationCategories(long id, short eo_type, short eo_subtype, short status, string list_type, List<long> location_eo_Ids)
        {
            var list_type_requested = 1;
            var section = _configuration.GetSection("proman_codes");
            short relation_type = section.GetValue<short>("RELATION~LOCATION_CATEGORY");
            short relation_subtype = section.GetValue<short>("EO_TYPE~INSTANCE");

            if (string.IsNullOrEmpty(list_type))
            {
                list_type = "featured_categories";
                list_type_requested = 0;
            }

            var eo2Isnquery = _vClassificationRepository.GetAll().Where((eo) => eo.ParentId == id);            
            eo2Isnquery = eo2Isnquery.Where((y) => y.EoSubtype == eEoSubTypes.INSTANCE);
            eo2Isnquery = eo2Isnquery.Where((y) => y.Status == eRecordStatus.Active);

            var eo2Ids = eo2Isnquery.OrderBy(x => x.Id).Select((o) => Convert.ToInt64(o.Id)).ToList();

            var query = DbSet.AsNoTracking().Where(x => x.RelationType == Common.Enums.eRelationTypes.LOCATION_CATEGORY
                        && x.RelationSubtype == Common.Enums.eEoSubTypes.INSTANCE
                        && x.Entity1Id == id
                        && x.Status == Common.Enums.eRecordStatus.Active);

            if (location_eo_Ids.Count > 0 && list_type_requested == 1)
                query = query.Where(x => location_eo_Ids.Contains(x.Id));

            return query.Where(x => eo2Ids.Contains(x.Entity2Id)).OrderBy(x => x.Entity2Id).ToList();

        }
        
    }
}
