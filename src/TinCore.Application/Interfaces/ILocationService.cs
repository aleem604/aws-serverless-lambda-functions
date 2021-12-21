using System;
using System.Collections.Generic;
using TinCore.Application.EventSourcedNormalizers;
using TinCore.Application.ViewModels;
using TinCore.Domain.Models;

namespace TinCore.Application.Interfaces
{
    public interface ILocationService : IDisposable
    {
        dynamic GetCountries(string version);
        dynamic GetRegions(string version, long id);
        dynamic GetCities(string version, long id);
        dynamic GetNeighbourhoods(string version, long id);
        dynamic GetPlace(string version, string name);
        dynamic GetLocation(string version, long location_id);
        dynamic GetFeaturedLocation(string version, long location_id);
        dynamic GetLocationFeaturedEntities(string version, int id, short status);
        dynamic GetLocationEnities(string version, int id, short status, string entity_list_type);
        dynamic GetLocationCategories(string version, long id, long location_id, int hisn5, string list_type, string category_string);
        dynamic GetCategories(string version, string list_type);

    }
}
