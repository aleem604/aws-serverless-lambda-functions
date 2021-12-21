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
using Microsoft.Extensions.Configuration;

namespace TinCore.Services.Api.Controllers
{
    public class EntityObjectsController : ApiController
    {
        private readonly IEntityObjectService _eoService;
        private readonly INotificationHandler<DomainNotification> _notifications;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EntityObjectsController> _logger;

        public EntityObjectsController(
            IEntityObjectService eoService,
            INotificationHandler<DomainNotification> notifications,
            IConfiguration configuration,
            ILogger<EntityObjectsController> logger,
            IMediatorHandler mediator) : base(notifications, mediator)
        {
            _eoService = eoService;
            _notifications = notifications;
            _configuration = configuration;
            _logger = logger;            
        }
       
        //api/v1/hierarchy/gallery_images
        [Route("hierarchy/gallery_images")]
        public IActionResult GetGallaryImages(string version ,int id)
        {            
            var gallaryImages = _eoService.GetGallaryImages(version, id).ToList().Select(x=>new  {id=x.isn, image_title = x.long_name, image_desc = x.description, image_url= x.long_name });

            return Response(gallaryImages);
        }
        //api/v1/hierarchy/location_events?id=1713
        [Route("hierarchy/location_events")]
        public IActionResult GetLocationEvents(string version, int id)
        {
            var gallaryImages = _eoService.GetLocationEvents(version, id).ToList();

            return Response(gallaryImages);
        }

        //api/v1/hierarchy/entity_folder?id=58420
        [Route("hierarchy/entity_folder")]
        public IActionResult GetEntityFolders(string version, int id)
        {
            var gallaryImages = _eoService.GetLocationEvents(version, id).ToList();

            return Response(gallaryImages);
        }

        //api/v1/hierarchy/list_of_coupons?id=58420
        [Route("hierarchy/list_of_coupons")]
        public IActionResult GetCoupons(string version, int location_id)
        {
            var gallaryImages = _eoService.GetCoupons(version, location_id).ToList();

            return Response(gallaryImages);
        }

        //api/v1/hierarchy/city_programs?id=58420
        [Route("hierarchy/city_programs")]
        public IActionResult GetCityPrograms(string version, int id)
        {
            var gallaryImages = _eoService.GetCoupons(version, id).ToList();

            return Response(gallaryImages);
        }

        //api/v1/hierarchy/countries
        [Route("hierarchy/countries")]
        public IActionResult GetCityPrograms(string version)
        {
            var gallaryImages = _eoService.GetCountries(version).ToList();

            return Response(gallaryImages);
        }

        //api/v1/hierarchy/list_of_entities
        [Route("hierarchy/list_of_entities")]
        public IActionResult GetEntities(string version, int id, int location_id)
        {
            var gallaryImages = _eoService.GetEntities(version, id, location_id).ToList();

            return Response(gallaryImages);
        }

        //api/v1/hierarchy/entity_galleries?id=22142
        [Route("hierarchy/entity_galleries")]
        public IActionResult GetEntityGallaries(string version, int id)
        {
            var gallaryImages = _eoService.GetEntityGallaries(version, id).ToList();

            return Response(gallaryImages);
        }

        //api/v1/hierarchy/gallery_videos?id=64321
        [Route("hierarchy/gallary_videos")]
        public IActionResult GetGallaryVideos(string version, int id)
        {
            var gallaryVideos = _eoService.GetGallaryVideos(version, id).ToList();

            return Response(gallaryVideos);
        }

        //api/v1/hierarchy/entity_review_categories?id=58421
        [Route("hierarchy/entity_review_categories")]
        public IActionResult GetEntityReviewCategories(string version, int id)
        {
            var categories = _eoService.GetEntityReviewCategories(version, id).ToList();

            return Response(categories);
        }

        //api/v1/hierarchy/regions
        [Route("hierarchy/regions")]
        public IActionResult GetRegions(string version, int id)
        {
            var regions = _eoService.GetRegions(version).ToList();

            return Response(regions);
        }

    }
}
