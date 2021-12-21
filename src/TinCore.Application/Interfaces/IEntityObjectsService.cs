using System;
using System.Collections.Generic;
using TinCore.Application.EventSourcedNormalizers;
using TinCore.Application.ViewModels;
using TinCore.Common.CommonModels;
using TinCore.Domain.Core.Models;
using TinCore.Domain.Models;

namespace TinCore.Application.Interfaces
{
    public interface IEntityObjectService : IDisposable
    {                                             
        dynamic GetGallaryImages(string version, long Id, long entity_id);
        dynamic GetGallaryVideos(string version, long id, long entity_id);
        dynamic GetEntityFolders(string version, long id);
        dynamic GetEntityFolderFiles(string version, long Id, long entity_id);
        dynamic GetFeaturedList(string version, long Id, long entity_id);
        dynamic GetPageMenu(string version, long Id);
        dynamic GetCityPrograms(string version, long Id);
        dynamic GetAttributesList(string version);
        dynamic GetPageBlocks(string version, long id, string type, string name);
        dynamic GetLocationEvents(string version, long hisn1);
        dynamic GetCoupons(string version, long Id);
        dynamic GetEntitiesList(string version, long id, long location_id, string url, int hisn1, string name);
        dynamic GetEntity(string version, long id, string tag, string search);
        dynamic GetEntity2(string version, long id, string tag);
        dynamic GetEntityGallaries(string version, long id);        
        dynamic GetEntityReviewCategories(string version, long id);
        dynamic GetProductList(string version, int ownerId);
        dynamic GetReviewCategories(string version, long id);
        dynamic GetOverview(string version, long id);
        dynamic GetLocation(string version, string lat, string lng, string result);
        dynamic GetEvents(string version, long location_id);
        dynamic GetEventsPHQ(string version, string offset, string lat, string lng, string within);
        dynamic GetVideoTemplatesList(string version);
        dynamic GetRecentUsedVideoTemplatesList(string version);
        dynamic GetMyVideosList(string version);
        dynamic GetVideoTemplateMomentsList(string version);
        void SaveEntity(EntityViewModel viewModel);        
    }
}
