using System;
using TinCore.Application.Interfaces;
using TinCore.Application.ViewModels;
using TinCore.Domain.Core.Bus;
using TinCore.Domain.Core.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace TinCore.Services.Api.Controllers
{
    public class RelationController : ApiController
    {
        private readonly IRelationService _relationService;

        public RelationController(
            IRelationService relationService,
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator) : base(notifications, mediator)
        {
            _relationService = relationService;
        }
        [Route("relation")]
        public IActionResult Get()
        {
            return Response("Tin Project");
        }

        [Route("reviews/comments")]
        public IActionResult GetReviews(int id)
        {
            var reviews = _relationService.GetReviews(id).ToList();

            return Response(reviews);
        }

    }
}
