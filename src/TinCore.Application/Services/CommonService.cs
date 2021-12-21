using System;
using System.Linq;
using AutoMapper;
using TinCore.Application.Interfaces;
using TinCore.Domain.Core.Bus;
using TinCore.Domain.Core.Models;
using TinCore.Domain.Interfaces;

namespace TinCore.Application.Services
{
    public class CommonService : ICommonService
    {
        private readonly IMapper _mapper;
        private readonly IMediatorHandler _bus;

        public CommonService(IMapper mapper,
                                  IMediatorHandler bus)
        {
            _mapper = mapper;
            _bus = bus;
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
