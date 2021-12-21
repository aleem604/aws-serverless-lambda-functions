using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TinCore.Application.Interfaces;
using TinCore.Application.ViewModels;
using TinCore.Common;
using TinCore.Common.CommonModels;
using TinCore.Common.Enums;
using TinCore.Domain.Commands;
using TinCore.Domain.Core.Bus;
using TinCore.Domain.Interfaces;
using TinCore.Domain.Models;

namespace TinCore.Application.Services
{
    public class EntityService : IEntityService
    {
        private readonly IMapper _mapper;
        private readonly IProfileAttributeRepo _profileAttributeRepo;
        private readonly ITinContactRepo _contactRepo;
        private readonly IConfiguration _configuration;
        private readonly IEntityRepo _entityRepository;
        private readonly IEntitySubscriptionTrackingRepo _entitySubscriptiontrackingRepo;
        private readonly IVLocationRepo _vLocationRepo;
        private readonly IRelationEntityRelationRepo _relationEntityRepo;
        private readonly ILocationCategoryRelationRepo _locationCategoryRepo;
        private readonly IVClassificationRepo _vClassificationRepo;
        private readonly IVRelationRepo _vRelationRepo;
        private readonly IAttributeRepo _attributeRepo;
        private readonly IMediatorHandler _bus;

        public EntityService(IMapper mapper,
                                  IProfileAttributeRepo profileAttributeRepo,
                                  ITinContactRepo contactRepo,
                                  IEntityRepo entityRepository,
                                  IEntitySubscriptionTrackingRepo entitySubscriptionTrackingRepo,
                                  IVLocationRepo vLocationRepo,
                                  IRelationEntityRelationRepo relationEntityRepo,
                                  ILocationCategoryRelationRepo locationCategoryRepo,
                                  IVClassificationRepo vClassificationRepo,
                                  IVRelationRepo vRelationRepo,
                                  IAttributeRepo attributeRepo,
                                  IConfiguration configuration,
                                  IMediatorHandler bus)
        {
            _mapper = mapper;
            _profileAttributeRepo = profileAttributeRepo;
            _contactRepo = contactRepo;
            _entityRepository = entityRepository;
            _entitySubscriptiontrackingRepo = entitySubscriptionTrackingRepo;
            _vLocationRepo = vLocationRepo;
            _relationEntityRepo = relationEntityRepo;
            _locationCategoryRepo = locationCategoryRepo;
            _vClassificationRepo = vClassificationRepo;
            _vRelationRepo = vRelationRepo;
            _attributeRepo = attributeRepo;
            _configuration = configuration;
            _bus = bus;
        }

        public dynamic GetEntitiesList(string version, long id, long location_id, string url, long hisn1, string name)
        {
            var section = _configuration.GetSection("proman_codes");
            var defaultsSection = _configuration.GetSection($"settings:v{version}:defaults");
            var tinIconArray = _configuration.GetSection($"settings:v{version}:defaults:entity_attribute_icons_array")
                                    .GetChildren().Select(item => new KeyValuePair<string, string>(item.Key, item.Value))
                                    .ToDictionary(x => x.Key, x => x.Value);

            if (id == 0 && location_id == 0 && !string.IsNullOrEmpty(url))
            {
                location_id = IdentifyLocations(url, ref hisn1, section);
                // for id 
                IdentifyCatgory(ref id, ref url, section);
            }

            //var query0 = from eo in _entityRepository.GetAll()
            //             join p in _profileAttributeRepo.GetAll() on eo.Id equals p.EntityId where p.ProfileSubtype == eProfileSubType.Profile1
            //             join p2 in _profileAttributeRepo.GetAll() on eo.Id equals p2.EntityId where p2.ProfileSubtype == eProfileSubType.Profile2
            //             join t in _entitySubscriptiontrackingRepo.GetAll() on eo.Id equals t.EntityId;


            return new ExpandoObject();
        }

        public dynamic GetEntityById(string version, long entityId)
        {
            return _entityRepository.GetEntityByIdWithInclude(entityId);
        }

        public void SaveEntity(EntityViewModel viewModel)
        {
            if (viewModel.Id > 0)
                _bus.SendCommand(_mapper.Map<UpdateEntityCommand>(viewModel));
            else
                _bus.SendCommand(_mapper.Map<NewEntityCommand>(viewModel));
        }

        public void SaveEntityCategory(EntityViewModel viewModel)
        {
            //if (viewModel.Id > 0)
            //    _bus.SendCommand(_mapper.Map<UpdateRelationCommand>(viewModel));
            //else
            //    _bus.SendCommand(_mapper.Map<NewRelationCommand>(viewModel));
        }

        #region private methods
        private long IdentifyCatgory(ref long id, ref string url, IConfigurationSection section)
        {
            string category_name = string.Empty;
            var locations = new List<string>();
            if (url.IndexOf("~") == -1 && url.IndexOf("/") == -1)
                id = 0;
            else
            {
                if (url.IndexOf("c/") == 0)
                    category_name = url.Substring(2);
                else
                {
                    if (url.IndexOf("/") == 0)
                        url = url.TrimStart('/');
                    if (url.IndexOf("/") == url.Length - 1)
                        url = url.TrimEnd('/');
                    if (url.IndexOf("~") == -1)
                        locations = url.Split("/").ToList();
                    else
                        locations = url.Split("~").ToList();
                    category_name = locations.LastOrDefault();
                }
            }
            return (from eo in _vClassificationRepo.GetAll()
                    where eo.Name == category_name && eo.EoType == eEoTypes.CLASSIFICATION
                    || eo.EoType == eEoTypes.CLASSIFICATIONTYPE
                    && eo.Status == eRecordStatus.Active
                    orderby eo.Id descending
                    select eo.Id).FirstOrDefault();

        }

        private long IdentifyLocations(string url, ref long hisn1, IConfigurationSection section)
        {
            List<string> locations = GetLocationsFromUrl(url);

            var locationTypes = new List<eLocationType>() { eLocationType.Country,
                eLocationType.Region,
                eLocationType.City,
                eLocationType.Neighbourhood
            };

            var o = new List<dynamic>();
            for (var i = 0; i < locations.Count; i++)
            {
                var name = locations.ElementAt(i);
                if (string.IsNullOrEmpty(name) && name == "c")
                    continue;

                var query = from eo in _vLocationRepo.GetAll()
                            where eo.Name == name.Trim() && eo.LocationType ==(eLocationType)locationTypes[i]
                            select eo;
                if (hisn1 > 0)
                {
                    var tempHisn1 = hisn1;
                    query = query.Where(x => x.ParentId == tempHisn1);
                }

                var identify_locations = query.Where(x => x.Status == eRecordStatus.Active)
                          .Select((e) => new
                          {
                              id = e.Id,
                              name = e.FullName
                          }).OrderBy(x => x.name)
                          .ToList();
                o.Add(identify_locations);
                if (identify_locations.FirstOrDefault().id > 0)
                    hisn1 = identify_locations.FirstOrDefault().id;
            }
            return o.LastOrDefault().id;
        }

        private static List<string> GetLocationsFromUrl(string url)
        {
            var locations = new List<string>();
            if (url.IndexOf("~") == -1 && url.IndexOf("/") == -1)
            {
                url = $"{url}/";
            }
            if (url.IndexOf("/") == 0)
            {
                url = url.TrimStart('/');
            }
            if (url.IndexOf("/") == url.Length - 1)
            {
                url = url.TrimEnd('/');
            }
            if (url.IndexOf("~") == -1)
            {
                locations = url.Split("/").ToList();
            }
            else
            {
                locations = url.Split("~").ToList();
            }

            return locations;
        }

        private string GetCardType(int hisn5)
        {
            switch (hisn5)
            {
                case 4:
                    return "l_card";
                case 3:
                    return "m_card";
                case 2:
                    return "s_card";
                default:
                    return "";
            }
        }

        private string GetFullAddress(string contact1, string city, string state, string country, string postal_code)
        {
            var fullAddress = new StringBuilder();
            if (!string.IsNullOrEmpty(contact1))
                fullAddress.Append(contact1).Append(" ");

            if (!string.IsNullOrEmpty(city))
                fullAddress.Append(city).Append(" ");

            if (!string.IsNullOrEmpty(state))
                fullAddress.Append(state).Append(" ");

            if (!string.IsNullOrEmpty(country))
                fullAddress.Append(country).Append(" ");

            if (!string.IsNullOrEmpty(postal_code))
                fullAddress.Append(postal_code).Append(" ");

            return fullAddress.Length > 0 ? fullAddress.ToString().Substring(0, fullAddress.Length - 1) : fullAddress.ToString();
        }
        #endregion privage methods

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        
    }
}