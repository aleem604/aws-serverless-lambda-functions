using System;
using System.Collections.Generic;
using TinCore.Application.EventSourcedNormalizers;
using TinCore.Application.ViewModels;
using TinCore.Common.CommonModels;
using TinCore.Domain.Core.Models;
using TinCore.Domain.Models;

namespace TinCore.Application.Interfaces
{
    public interface IEntityService : IDisposable
    {
        dynamic GetEntitiesList(string version, long id, long location_id, string url, long parent_id, string name);
        dynamic GetEntityById(string version, long entityId);
        void SaveEntity(EntityViewModel viewModel);
        void SaveEntityCategory(EntityViewModel viewModel);
    }
}
