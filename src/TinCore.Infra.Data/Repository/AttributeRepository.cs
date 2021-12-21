using System.Linq;
using TinCore.Domain.Interfaces;
using TinCore.Domain.Models;
using TinCore.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using TinCore.Domain.Core.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using System;
using TinCore.Application.ViewModels;
using System.Xml;
using System.Xml.Linq;
using TinCore.Common.CommonModels;
using Microsoft.Extensions.Configuration;
using System.Dynamic;
using TinCore.Common.Enums;
using System.Threading.Tasks;

namespace TinCore.Infra.Data.Repository
{
    public class AttributeRepo : Repository<TinAttribute>, IAttributeRepo
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _configuration;

        public AttributeRepo(TinCoreContext context, IHostingEnvironment env, IConfiguration configuration)
            : base(context)
        {
            _env = env;
            _configuration = configuration;
        }
                    

    }
}
