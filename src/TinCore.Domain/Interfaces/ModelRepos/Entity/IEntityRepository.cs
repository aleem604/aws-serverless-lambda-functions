using System.Collections.Generic;
using System.Threading.Tasks;
using TinCore.Common.CommonModels;
using TinCore.Common.Enums;
using TinCore.Domain.Core.Models;
using TinCore.Domain.Models;
using TinEntity = TinCore.Domain.Models.TinEntity;

namespace TinCore.Domain.Interfaces
{
    public interface IEntityRepo : IRepositoryT<TinEntity>
    {
        dynamic GetEntityByIdWithInclude(long entityId);
        TinEntity GetEntity(long id, IncludeOptions all);
    }
}