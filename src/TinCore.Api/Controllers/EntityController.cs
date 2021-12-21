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
    public class EntityController : ApiController
    {
        private readonly IEntityService _entityService;
        private readonly INotificationHandler<DomainNotification> _notifications;
        private readonly ILogger<EntityController> _logger;

        public EntityController(
            IEntityService entityService,
            INotificationHandler<DomainNotification> notifications,
            ILogger<EntityController> logger,
            IMediatorHandler mediator) : base(notifications, mediator)
        {
            _entityService = entityService;
            _notifications = notifications;
            _logger = logger;
        }

        //api/v1/hierarchy/list_of_entities
        [HttpGet("hierarchy/list_of_entities")]
        public IActionResult GetEntities(string version, long id, long location_id, string url, long parent_id, string name)
        {
            return Response(_entityService.GetEntitiesList(version, id, location_id, url, parent_id, name));
        }

        //api/v1/hierarchy/set_entity
        [HttpPost("hierarchy/set_entity")]
        public IActionResult PostEntity([FromBody]EntityViewModel viewModel)
        {
            _entityService.SaveEntity(viewModel);
            return Response();
        }

        //api/v1/hierarchy/entity_category
        [HttpPost("hierarchy/entity_category")]
        public IActionResult PostEntityCategory([FromBody]EntityViewModel viewModel)
        {
            _entityService.SaveEntityCategory(viewModel);
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
