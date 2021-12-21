using System;
using TinCore.Application.Interfaces;
using TinCore.Application.ViewModels;
using TinCore.Domain.Core.Bus;
using TinCore.Domain.Core.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace TinCore.Api.Controllers
{
    public class LocationController : ApiController
    {
        private readonly ILocationService _locationService;
        private readonly ILocationProgramService _locationProgramService;
        private readonly IConfiguration _configuration;

        public LocationController(
            ILocationService locationService,
            //ILocationProgramService locationProgramService,
            INotificationHandler<DomainNotification> notifications,
            IConfiguration configuration,
            IMediatorHandler mediator) : base(notifications, mediator)
        {
            _locationService = locationService;
            //_locationProgramService = locationProgramService;
            _configuration = configuration;
        }


        //api/v1/hierarchy/countries
        [HttpGet("hierarchy/countries")]
        public IActionResult GetCountries(string version)
        {
            return Response(_locationService.GetCountries(version));
        }

        //api/v1/hierarchy/regions?id=1442
        [HttpGet, Route("hierarchy/regions")]
        public IActionResult GetRegions(string version, int id)
        {
            return Response(_locationService.GetRegions(version, id));
        }

        //api/v1/hierarchy/cities
        [HttpGet, Route("hierarchy/cities")]
        public IActionResult GetCities(string version, int id)
        {
            return Response(_locationService.GetCities(version, id));
        }

        //api/v1/hierarchy/neighborhoods
        [HttpGet, Route("hierarchy/neighborhoods")]
        public IActionResult GetNeighborhoods(string version, int id)
        {
            return Response(_locationService.GetNeighbourhoods(version, id));
        }

        //api/v1/hierarchy/place?name=United Kingdom~England~London
        [HttpGet, Route("hierarchy/place")]
        public IActionResult GetPlace(string version, string name)
        {
            return Response(_locationService.GetPlace(version, name));
        }

        //api/v1/hierarchy/location?location_id=4545
        [HttpGet, Route("hierarchy/location")]
        public IActionResult GetLocation(string version, long location_id)
        {
            return Response(_locationService.GetLocation(version, location_id));
        }

        //api/v1/hierarchy/location_featured?id=63771
        [HttpGet("hierarchy/location_featured")]
        public IActionResult GetFeaturedLocation(string version, long id)
        {
            return Response(_locationService.GetFeaturedLocation(version, id));
        }

        //v1/hierarchy/location_featured_entities?id=1723&status=1 
        [HttpGet("hierarchy/location_featured_entities")]
        public IActionResult GetLocationFeaturedEntities(string version, int id, short status)
        {
            return Response(_locationService.GetLocationFeaturedEntities(version, id, status));
        }

        //api/v1/hierarchy/location_entities   
        [HttpGet("hierarchy/location_entities")]
        public IActionResult GetLocationEntities(string version, int id, short status, string entity_list_type = "featured_entities")
        {
            return Response(_locationService.GetLocationEnities(version, id, status, entity_list_type));
        }

        //api/v1/hierarchy/location_programs?id=4545
        [HttpGet("hierarchy/location_programs")]
        public IActionResult GetLocationPrograms(string version, long id)
        {
            return Response(_locationProgramService.GetLocationPrograms(version, id));
        }

        //api/v1/hierarchy/categories?location_id=63771&list_type=featured_categories
        [HttpGet("hierarchy/categories")]
        public IActionResult GetCategories(string version, long id, long location_id, int hisn5, string list_type, string category_string)
        {
            if (location_id > 0)
                return Response(_locationService.GetLocationCategories(version, id, location_id, hisn5, list_type, category_string));
            else
                return Response(_locationService.GetCategories(version, list_type));
        }

    }
}
