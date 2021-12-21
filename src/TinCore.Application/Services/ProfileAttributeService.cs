using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Configuration;
using TinCore.Application.Interfaces;
using TinCore.Application.ViewModels;
using TinCore.Common.CommonModels;
using TinCore.Domain.Commands;
using TinCore.Domain.Core.Bus;
using TinCore.Domain.Core.Models;
using TinCore.Domain.Interfaces;
using TinCore.Domain.Models;

namespace TinCore.Application.Services
{
    public class ProfileAttributeService : IProfileAttributeService
    {
        private readonly IMapper _mapper;
        private readonly IProfileAttributeRepository _profileAttrRepository;
        IConfiguration _configuration;
        private readonly IMediatorHandler _bus;

        public ProfileAttributeService(IMapper mapper,
                                  IProfileAttributeRepository profileAttrRepository,
                                  IConfiguration configuration,
                                  IMediatorHandler bus)
        {
            _mapper = mapper;
            _profileAttrRepository = profileAttrRepository;
            _configuration = configuration;
            _bus = bus;
        }

       
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


    }
}
