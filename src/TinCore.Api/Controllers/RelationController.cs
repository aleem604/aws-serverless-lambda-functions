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
        [HttpGet("relation")]
        public IActionResult GetInfo()
        {
            return Response("Tin Project");
        }

        // api/v1/reviews/comments?id=58432
        [HttpGet("reviews/comments")]
        public IActionResult GetReviews(long id)
        {
            return Response(_relationService.GetReviews(id));
        }

    }
}
