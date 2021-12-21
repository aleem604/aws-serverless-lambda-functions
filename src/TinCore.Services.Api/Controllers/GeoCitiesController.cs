using System;
using TinCore.Application.Interfaces;
using TinCore.Application.ViewModels;
using TinCore.Domain.Core.Bus;
using TinCore.Domain.Core.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TinCore.Services.Api.Controllers
{
    public class GeoCitiesController : ApiController
    {
        private readonly IGeoCityAppService _geoCitiesService;

        public GeoCitiesController(
            IGeoCityAppService geoCitiesService,
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator) : base(notifications, mediator)
        {
            _geoCitiesService = geoCitiesService;
        }

        ///api/v1/geocities
        [HttpGet,Route("geocities")]
        [AllowAnonymous]
        public IActionResult Get()
        {
            return Response(_geoCitiesService.GetAll());
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("geocities/{id}")]
        public IActionResult Get(string id)
        {
            var customerViewModel = _geoCitiesService.GetByStringId(id);

            return Response(customerViewModel);
        }     

        [HttpPost]
        [Route("geocities")]
        public IActionResult Post([FromBody]GeoCityViewModel geoCitiesViewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(geoCitiesViewModel);
            }

            _geoCitiesService.Register(geoCitiesViewModel);

            return Response(geoCitiesViewModel);
        }
        
        [HttpPut]
        public IActionResult Put([FromBody]GeoCityViewModel geoCitiesViewModel)
        {
            if (!ModelState.IsValid)
            {
                NotifyModelStateErrors();
                return Response(geoCitiesViewModel);
            }

            _geoCitiesService.Update(geoCitiesViewModel);

            return Response(geoCitiesViewModel);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _geoCitiesService.Remove(id);
            
            return Response();
        }

    }
}
