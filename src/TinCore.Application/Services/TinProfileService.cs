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
    public class TinProfileService : ITinProfileService
    {
        private readonly IMapper _mapper;
        private readonly IProfileRepository _profileRepository;
        IConfiguration _configuration;
        private readonly IMediatorHandler _bus;

        public TinProfileService(IMapper mapper,
                                  IProfileRepository profileRepository,
                                  IConfiguration configuration,
                                  IMediatorHandler bus)
        {
            _mapper = mapper;
            _profileRepository = profileRepository;
            _configuration = configuration;
            _bus = bus;
        }

        public IEnumerable<dynamic> GetEntityReviews(string version, long id)
        {
            var section = _configuration.GetSection("proman_codes");
            long profile_type = section.GetValue<long>("PROFILE~REVIEW");
            short profile_subtype = section.GetValue<short>("PROFILE~1");
            short eo_type = section.GetValue<short>("EO_TYPE~PERSON");
            short eo_subtype = section.GetValue<short>("EO_TYPE~INSTANCE");

            return _profileRepository.GetAll().Where(x => x.eo_isn == id && x.profile_type == profile_type && x.profile_subtype == profile_subtype)
                 .Where(x => x.eo_type == eo_type && x.eo_subtype == eo_subtype)
                 .OrderBy(x => x.isn)
                 .Select((o) => new
                 {
                     review_id = o.isn,
                     reviewed_by_id = o.profile1,
                     stars = o.profile2,
                     review_datetime = o.profile8,
                     desc1 = o.profile6,
                     desc2 = o.profile9,
                     image_url = o.profile7,
                 }).ToList();
        }

        private string ProcessXML(string xml, string attribute)
        {
            if (string.IsNullOrEmpty(xml))
                return xml;
            var cxml = XDocument.Parse(xml).Root;
            if (cxml.HasAttributes && cxml.Attribute(attribute) != null)
                return cxml.Attribute(attribute).Value;

            return string.Empty;
        }

        public IEnumerable<dynamic> GetEntitySections(string version, long id)
        {
            var defaultsSection = _configuration.GetSection($"settings:v{version}:defaults");
            var img_url_prefix = defaultsSection.GetValue<string>("img_url_prefix");

            var section = _configuration.GetSection("proman_codes");
            long profile_type = section.GetValue<long>("PROFILE~SECTION");
            short eo_type = section.GetValue<short>("EO_TYPE~PERSON");
            short eo_subtype = section.GetValue<short>("EO_TYPE~INSTANCE");

            return _profileRepository.GetAll()
                .Where(x => x.eo_isn == id)
                .Where(x => x.profile_type == profile_type)
                .Where(x => x.eo_type == eo_type)
                .Where(x => x.eo_subtype == eo_subtype)
                .OrderBy(x => x.isn)
                .Select((o) => new
                {
                    name = o.profile6,
                    image_url = $"{img_url_prefix}{o.profile7}",
                    description = o.profile9,
                })
                 .ToList();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


    }
}
