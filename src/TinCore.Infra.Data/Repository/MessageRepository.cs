﻿using System.Linq;
using TinCore.Domain.Interfaces;
using TinCore.Domain.Models;
using TinCore.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using TinCore.Domain.Core.Models;
using System.Collections.Generic;

namespace TinCore.Infra.Data.Repository
{
    public class MessageRepository : Repository<Message>, IMessageRepo
    {
        public MessageRepository(TinCoreContext context)
            : base(context)
        { }

    }
}
