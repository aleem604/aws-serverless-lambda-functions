using System.Collections.Generic;
using TinCore.Common.CommonModels;
using TinCore.Domain.Models;

namespace TinCore.Domain.Interfaces
{
    public interface IRelationEntityRelationRepo : IRepositoryT<RelationEntityRelation>
    {
        dynamic GetCategoriesForLocationFeaturedEntities(List<long> entityIds);
        dynamic GetLocationCategories(long id, short eo_type, short eo_subtype, short status, string list_type, List<long> location_eo_Ids);
    }
}
