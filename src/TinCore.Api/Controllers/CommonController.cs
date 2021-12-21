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
    public class CommonController : ApiController
    {
        private readonly ICommonService _commonAppService;
        private readonly IConfiguration _configuration;

        public CommonController(
            ICommonService commonAppService,
            INotificationHandler<DomainNotification> notifications,
            IConfiguration configuration,
            IMediatorHandler mediator) : base(notifications, mediator)
        {
            _commonAppService = commonAppService;
            _configuration = configuration;
        }
        [HttpGet, Route("values")]
        public IActionResult Get()
        {
            return Response("Tin Project");
        }

        //api/v1/company/company_id
        [HttpGet, Route("company/company_id")]
        public IActionResult GetCompanyId(string version)
        {
            return Response(_configuration.GetSection($"settings:v{version}:defaults").GetValue<long>("company"));
        }
        //api/v1/setup/languages
        [HttpGet, Route("setup/languages")]
        public IActionResult GetLanguages(string version)
        {
            return Response(_configuration.GetSection($"settings:v{version}:languages")
                .GetChildren().Select(item => new KeyValuePair<string, string>(item.Key, item.Value))
                .ToDictionary(x => x.Key, x => x.Value)
                );
        }
        
    }
}
