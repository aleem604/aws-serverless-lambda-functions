using System.Security.Claims;
using System.Threading.Tasks;
using TinCore.Domain.Core.Bus;
using TinCore.Domain.Core.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TinCore.Services.Api.Controllers
{
    [Authorize]
    public class AccountController : ApiController
    {
        private readonly ILogger _logger;

        public AccountController(
            INotificationHandler<DomainNotification> notifications,
            ILoggerFactory loggerFactory,
            IMediatorHandler mediator) : base(notifications, mediator)
        {
            _logger = loggerFactory.CreateLogger<AccountController>();
        }
             
    }
}
