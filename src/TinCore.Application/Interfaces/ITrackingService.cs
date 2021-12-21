using System;
using System.Collections.Generic;
using TinCore.Application.EventSourcedNormalizers;
using TinCore.Application.ViewModels;
using TinCore.Domain.Models;

namespace TinCore.Application.Interfaces
{
    public interface ITrackingService : IDisposable
    {
        dynamic GetListOfEvents(string version, long id);
        IEnumerable<dynamic> GetEventTickets(string version, long event_id);
        dynamic GetLocationEvents(string version, long id, long location_id);
        dynamic GetEvents(string version, long id);
        dynamic GetListOfCoupons(string version, long id);
        dynamic GetEntityCoupons(string version, long id);
    }
}
