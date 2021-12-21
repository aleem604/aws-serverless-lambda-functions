using System.Linq;
using TinCore.Domain.Interfaces;
using TinCore.Domain.Models;
using TinCore.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using TinCore.Domain.Core.Models;
using System.Collections.Generic;

namespace TinCore.Infra.Data.Repository
{
    public class LogonRepository : Repository<Logon>, ILogonRepo
    {
        public LogonRepository(TinCoreContext context)
            : base(context)
        { }

        //public IEnumerable<EntityObject> GetGallaryImages(int Id)
        //{
        //    return DbSet.AsNoTracking().Where(c => c.isn == Id).ToList();
        //}

    }
}
