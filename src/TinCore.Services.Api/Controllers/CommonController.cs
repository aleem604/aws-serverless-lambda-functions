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
    public class CommonController : ApiController
    {
        private readonly ICommonService _commonAppService;

        public CommonController(
            ICommonService commonAppService,
            INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediator) : base(notifications, mediator)
        {
            _commonAppService = commonAppService;
        }
        [Route("values")]
        public IActionResult Get()
        {
            return Response("Tin Project");
        }

        //api/v1/company/company_id
        [Route("company/company_id")]
        public IActionResult GetCompanyId()
        {
            return Response(_commonAppService.GetCompanyId());
        }
        
    }
}
