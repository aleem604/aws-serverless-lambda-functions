using System.Linq;
using TinCore.Domain.Interfaces;
using TinCore.Domain.Models;
using TinCore.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using TinCore.Domain.Core.Models;
using System.Collections.Generic;
using TinCore.Common.CommonModels;
using Microsoft.Extensions.Configuration;

namespace TinCore.Infra.Data.Repository
{
    public class SubscriptionRepo : Repository<Subscription>, ISubscriptionRepo
    {
        private readonly IConfiguration _configuration;
        public SubscriptionRepo(TinCoreContext context, IConfiguration configuration)
            : base(context)
        {
            _configuration = configuration;
        }
        
    }
}
