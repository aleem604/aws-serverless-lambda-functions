using System;
using TinCore.Application.Interfaces;
using TinCore.Application.ViewModels;
using TinCore.Domain.Core.Bus;
using TinCore.Domain.Core.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace TinCore.Api.Controllers
{
    public class TrackingController : ApiController
    {
        private readonly ITrackingService _trackingService;

        public TrackingController(
            ITrackingService trackingService,
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator) : base(notifications, mediator)
        {
            _trackingService = trackingService;
        }
        

        //api/v1/hierarchy/list_of_events 
        [HttpGet("hierarchy/list_of_events")]
        public IActionResult GetListOfEvents(string version, long id)
        {            

            return Response(_trackingService.GetListOfEvents(version, id));
        }

        //api/v1/hierarchy/events?event_id=9843
        //api/v1/hierarchy/events?id=1478&&location_id= 
        //api/v1/hierarchy/events?id=7429
        [HttpGet("hierarchy/events")]
        public IActionResult GetEvents(string version, long id, long event_id, long location_id)
        {
            if(event_id>0)
                return Response(_trackingService.GetEventTickets(version, event_id));
            if(location_id>0)
                return Response(_trackingService.GetLocationEvents(version, id, location_id));
            else
            return Response(_trackingService.GetEvents(version, id));
        }

        //api/v1/hierarchy/entity_events?id=58421
        [HttpGet("hierarchy/entity_events")]
        public IActionResult GetEntityEvents(string version, long id)
        {
            return Response(_trackingService.GetEvents(version, id));
        }

        //api/v1/hierarchy/location_events?id=58421
        [HttpGet("hierarchy/location_events")]
        public IActionResult GetLocationEvents(string version, long id, long location_id)
        {
            return Response(_trackingService.GetLocationEvents(version, id, location_id));
        }

        //api/v1/hierarchy/list_of_coupons?location_id=2868
        [HttpGet("hierarchy/list_of_coupons")]
        public IActionResult GetListOfCoupons(string version, long location_id)
        {
            return Response(_trackingService.GetListOfCoupons(version, location_id));
        }

        //api/v1/hierarchy/entity_coupons?id=58421
        [HttpGet("hierarchy/entity_coupons")]
        public IActionResult GetEntityCoupons(string version, long id)
        {
            return Response(_trackingService.GetEntityCoupons(version, id));
        }


    }
}
