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
using TinEntity = TinCore.Domain.Models.TinEntity;
using TinCore.Common.Enums;
using System.Threading.Tasks;

namespace TinCore.Infra.Data.Repository
{
    public class EntityRepository : Repository<TinEntity>, IEntityRepo
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _configuration;

        public EntityRepository(TinCoreContext context, IHostingEnvironment env, IConfiguration configuration)
            : base(context)
        {
            _env = env;
            _configuration = configuration;
        }
        
        public dynamic GetEntityByIdWithInclude(long entityId)
        {
            return DbSet.Where(x => x.Id == entityId).Include(x => x.Contact)
                .Select((o) => new {
                    id = o.Id,
                    contact = new
                    {
                        contact_id = o.Contact.Id
                    }
                });
        }

        public TinEntity GetEntity(long entityId, IncludeOptions options)
        {
            var query = DbSet.Where(x => x.Id == entityId);
            if(options.IncludeEntityContact)            
                query = query.Include(x => x.Contact);
            if (options.IncludeEntityProfile)
            {
                query = query.Include(x => x.ProfileAttribute);
                query = query.Include(x => x.ProfileSection);
                query = query.Include(x => x.ProfileReview);
            }



            return query.FirstOrDefault();
        }



    }
}
