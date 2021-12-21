using System;
using TinCore.Application.Interfaces;
using TinCore.Application.ViewModels;
using TinCore.Domain.Core.Bus;
using TinCore.Domain.Core.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace TinCore.Api.Controllers
{
    public class EntityObjectsController : ApiController
    {
        private readonly IEntityObjectService _eoService;
        private readonly IEntityService _entityService;
        private readonly INotificationHandler<DomainNotification> _notifications;
        private readonly ILogger<EntityObjectsController> _logger;

        public EntityObjectsController(
            IEntityObjectService eoService,
            IEntityService entityService,
            INotificationHandler<DomainNotification> notifications,
            ILogger<EntityObjectsController> logger,
            IMediatorHandler mediator) : base(notifications, mediator)
        {
            _eoService = eoService;
            _entityService = entityService;
            _notifications = notifications;
            _logger = logger;
        }

        //api/v1/hierarchy/list_of_entities
        //[HttpGet("hierarchy/list_of_entities")]
        //public IActionResult GetEntities(string version, long id, long location_id, string url, int hisn1, string name)
        //{
        //    return Response(_eoService.GetEntitiesList(version, id, location_id, url, hisn1, name));
        //}

        //api/v1/hierarchy/entity?id=65069 
        [HttpGet("hierarchy/entity")]
        public IActionResult GetEntity(string version, long id, string tag, string search)
        {
            return Response(_eoService.GetEntity(version, id, tag, search));
        }

        //api/v1/hierarchy/entity2?id= &tag=
        [HttpGet("hierarchy/entity2")]
        public IActionResult GetEntity2(string version, long id, string tag)
        {
            return Response(_eoService.GetEntity2(version, id, tag));
        }

        //api/v1/hierarchy/entity_review_categories?id=58421
        [HttpGet("hierarchy/entity_review_categories")]
        public IActionResult GetEntityReviewCategories(string version, int id)
        {
            return Response(_eoService.GetEntityReviewCategories(version, id));
        }

        //api/v1/hierarchy/entity_galleries?id=22142
        [HttpGet("hierarchy/entity_galleries")]
        public IActionResult GetEntityGallaries(string version, long id)
        {
            return Response(_eoService.GetEntityGallaries(version, id));
        }

        //api/v1/hierarchy/gallery_images?id=64044 && entity_id =
        [HttpGet("hierarchy/gallery_images")]
        public IActionResult GetGallaryImages(string version, long id, long entity_id)
        {
            return Response(_eoService.GetGallaryImages(version, id, entity_id));
        }

        //api/v1/hierarchy/gallery_videos?id=64321&&entity_id =
        [HttpGet("hierarchy/gallery_videos")]
        public IActionResult GetGallaryVideos(string version, long id, long entity_id)
        {
            return Response(_eoService.GetGallaryVideos(version, id, entity_id));
        }

        //api/v1/hierarchy/entity_folder?id=58420
        [HttpGet("hierarchy/entity_folder")]
        public IActionResult GetEntityFolders(string version, long id, long entity_id)
        {
            return Response(_eoService.GetEntityFolders(version, id));
        }

        //api/v1/hierarchy/folder_files?id=64324&entity_id= 
        [HttpGet("hierarchy/folder_files")]
        public IActionResult GetFolderFiles(string version, long id, long entity_id)
        {
            return Response(_eoService.GetEntityFolderFiles(version, id, entity_id));
        }

        //api/v1/hierarchy/featured_list?id=1723
        [HttpGet("hierarchy/featured_list")]
        public IActionResult GetFeaturedList(string version, long id, long entity_id)
        {
            return Response(_eoService.GetFeaturedList(version, id, entity_id));
        }

        //hierarchy/page_menu
        [HttpGet("hierarchy/page_menu")]
        public IActionResult GetPageMenu(string version, long id)
        {
            return Response(_eoService.GetPageMenu(version, id));
        }

        //api/v1/hierarchy/city_programs?id=58420
        [HttpGet("hierarchy/city_programs")]
        public IActionResult GetCityPrograms(string version, long id)
        {
            return Response(_eoService.GetCityPrograms(version, id));
        }

        //api/v1/hierarchy/list_of_attributes
        [HttpGet("hierarchy/list_of_attributes")]
        public IActionResult GetAttributesList(string version)
        {
            return Response(_eoService.GetAttributesList(version));
        }

        //api/v1/hierarchy/page_block?id=63771&type=location&name=location_slider
        [HttpGet("hierarchy/page_block")]
        public IActionResult GetPageBlocks(string version, long id, string type, string name)
        {
            return Response(_eoService.GetPageBlocks(version, id, type, name));
        }

        //api/v1/product/product_list?owner=12345
        [HttpGet("product/product_list")]
        public IActionResult GetProductList(string version, int ownerId)
        {
            return Response(_eoService.GetProductList(version, ownerId));
        }

        //api/v1/reviews/review_categories?id=6140
        [HttpGet("reviews/review_categories")]
        public IActionResult GetReviewCategories(string version, long id)
        {
            return Response(_eoService.GetReviewCategories(version, id));
        }

        //api/v1/reviews/overview?id=6140
        [HttpGet("reviews/overview")]
        public IActionResult GetOverview(string version, long id)
        {
            return Response(_eoService.GetOverview(version, id));
        }
        ///api/v1/geo/get_location
        [HttpGet("geo/get_location")]
        public IActionResult GetLocation(string version, string lat = "48.584295", string lng = "7.736080", string result = "short")
        {
            return Response(_eoService.GetLocation(version, lat, lng, result));
        }

        ///api/v1/events/get_events
        [HttpGet("events/get_events")]
        [AllowAnonymous]
        public IActionResult GetEvents(string version, long location_id)
        {
            return Response(_eoService.GetEvents(version, location_id));
        }

        ///api/v1/events/get_events_phq?offset=0&lat=52.3555177&lng=-1.1743197000000691
        [HttpGet("events/get_events_phq")]
        public IActionResult GetEventsPHQ(string version, string offset = "0", string lat = "48.584295", string lng = "7.736080", string within = "10km")
        {
            return Response(_eoService.GetEventsPHQ(version, offset, lat, lng, within));
        }

        ///api/v1/moments/get_list_of_video_templates 
        [HttpGet("moments/get_list_of_video_templates")]
        [AllowAnonymous]
        public IActionResult GetVideoTemplatesList(string version)
        {
            return Response(_eoService.GetVideoTemplatesList(version));
        }

        ///api/v1/moments/get_list_of_recent_used_video_templates 
        [HttpGet("moments/get_list_of_recent_used_video_templates")]
        public IActionResult GetRecentUsedVideoTemplatesList(string version)
        {
            return Response(_eoService.GetRecentUsedVideoTemplatesList(version));
        }

        ///api/v1/moments/get_list_of_my_videos
        [HttpGet("moments/get_list_of_my_videos")]
        public IActionResult GetMyVideosList(string version)
        {
            return Response(_eoService.GetMyVideosList(version));
        }

        ///api/v1/moments/get_list_of_video_template_moments
        [HttpGet("moments/get_list_of_video_template_moments")]
        public IActionResult GetVideoTemplateMomentsList(string version)
        {
            return Response(_eoService.GetVideoTemplateMomentsList(version));
        }

        //api/v1/hierarchy/set_entity
        [HttpPost("hierarchy/set_entity")]
        public IActionResult PostEntity([FromBody]EntityViewModel viewModel)
        {
            _eoService.SaveEntity(viewModel);
            return Response();
        }

        //api/v1/hierarchy/get_entity
        [HttpGet("hierarchy/get_entity")]
        public IActionResult GetEntity(string version, long id)
        {
            var result = _entityService.GetEntityById(version, id);
            return Response(result);
        }


    }
}
