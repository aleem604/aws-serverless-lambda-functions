using System;
using System.Collections.Generic;
using TinCore.Application.EventSourcedNormalizers;
using TinCore.Application.ViewModels;
using TinCore.Domain.Models;

namespace TinCore.Application.Interfaces
{
    public interface ITinProfileService : IDisposable
    {
        IEnumerable<dynamic> GetEntityReviews(string version, long id);
        IEnumerable<dynamic> GetEntitySections(string version, long id);
    }
}
