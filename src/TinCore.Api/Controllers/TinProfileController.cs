using System;
using TinCore.Application.Interfaces;
using TinCore.Application.ViewModels;
using TinCore.Domain.Core.Bus;
using TinCore.Domain.Core.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Http;
using TinCore.Domain.Models;

namespace TinCore.Api.Controllers
{
    public class TinProfileController : ApiController
    {
        private readonly IRelationService _relationService;
        private readonly ITinProfileService _profileService;

        public TinProfileController(
            IRelationService relationService,
            ITinProfileService profileService,
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator) : base(notifications, mediator)
        {
            _relationService = relationService;
            _profileService = profileService;
        }


        //api/v1/hierarchy/entity_reviews?id=51441  
        [HttpGet("hierarchy/entity_reviews")]
        public IActionResult GetEntityReviews(string version, long id)
        {
            return Response(_profileService.GetEntityReviews(version, id));
        }

        //api/v1/hierarchy/set_entity_review 
        [HttpPost("hierarchy/ set_entity_review")]
        public IActionResult PostEntityReviews([FromBody]EntityViewModel model)
        {
            return Response(model);
        }

        //api/v1/hierarchy/entity_sections?id=58420
        [HttpGet("hierarchy/entity_sections")]
        public IActionResult GetEntityReviewCategories(string version, int id)
        {
            return Response(_profileService.GetEntitySections(version, id));
        }

    }
}
