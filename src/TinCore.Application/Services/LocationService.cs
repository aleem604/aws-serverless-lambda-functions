using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using TinCore.Application.Interfaces;
using TinCore.Common;
using TinCore.Common.Enums;
using TinCore.Domain.Core.Bus;
using TinCore.Domain.Interfaces;
using TinCore.Domain.Models;

namespace TinCore.Application.Services
{
    public class LocationService : ILocationService
    {
        private readonly IMapper _mapper;
        private readonly IVLocationRepo _vLocationRepo;
        private readonly ICountryRepo _countryRepo;
        private readonly IRegionRepo _regionRepo;
        private readonly ICityRepo _cityRepo;
        private readonly INeighbourhoodRepo _neighbourhoodRepo;
        private readonly IVClassificationRepo _vClassificationRepo;
        private readonly IClassificationRepo _classificationRepo;
        private readonly IProfileAttributeRepo _profileAttrRepo;
        private readonly IEntityRepo _entityRepo;
        private readonly ILocationCategoryRelationRepo _locationCategoryRepo;
        private readonly ILocationEntityRelationRepo _locationEntityRepo;
        private readonly IRelationEntityRelationRepo _relationEntityRepo;
        private readonly IConfiguration _configuration;
        private readonly IMediatorHandler Bus;

        public LocationService(IMapper mapper,
                                  IVLocationRepo vLocationRepo,
                                  ICountryRepo countryRepo,
                                  IRegionRepo regionRepo,
                                  ICityRepo cityRepo,
                                  INeighbourhoodRepo neighbourhoodRepo,
                                  IVClassificationRepo vClassificationRepo,
                                  IClassificationRepo classificationRepo,
                                  IProfileAttributeRepo profileAttrRepo,
                                  IEntityRepo entityRepository,
                                  ILocationEntityRelationRepo locationEntityRepo,
                                  ILocationCategoryRelationRepo locationCategoryRepo,
                                  IRelationEntityRelationRepo relationEntityRepo,
                                  IConfiguration configuration,
                                  IMediatorHandler bus)
        {
            _mapper = mapper;
            _vLocationRepo = vLocationRepo;
            _countryRepo = countryRepo;
            _regionRepo = regionRepo;
            _cityRepo = cityRepo;
            _neighbourhoodRepo = neighbourhoodRepo;
            _vClassificationRepo = vClassificationRepo;
            _classificationRepo = classificationRepo;
            _profileAttrRepo = profileAttrRepo;
            _entityRepo = entityRepository;
            _locationEntityRepo = locationEntityRepo;
            _locationCategoryRepo = locationCategoryRepo;
            _relationEntityRepo = relationEntityRepo;
            _configuration = configuration;
            Bus = bus;
        }
        public dynamic GetCountries(string version)
        {
            return _vLocationRepo.GetAll()
                .Where(x => x.LocationType == eLocationType.Country && x.Status == eRecordStatus.Active)
                    .Select((o) => new
                    {
                        id = o.Id,
                        name = o.Name
                    })
                .OrderBy(o => o.name)
                .ToList();
        }

        public dynamic GetRegions(string version, long id)
        {
            return _regionRepo.GetAll()
                .Where(x => x.CountryId == id
                && x.LocationType == eLocationType.Region
                && x.Status == eRecordStatus.Active)
                    .Select((o) => new
                    {
                        id = o.Id,
                        name = o.Name
                    })
                .OrderBy(o => o.name)
                .ToList();
        }

        public dynamic GetCities(string version, long id)
        {
            return _cityRepo.GetAll()
                 .Where(x => x.RegionId == id
                && x.LocationType == eLocationType.City
                && x.Status == eRecordStatus.Active)
                    .Select((o) => new
                    {
                        id = o.Id,
                        name = o.Name
                    })
                .OrderBy(o => o.name)
                .ToList(); ;
        }

        public dynamic GetNeighbourhoods(string version, long id)
        {

            return _neighbourhoodRepo.GetAll()
                 .Where(x => x.CityId == id
                && x.LocationType == eLocationType.Neighbourhood
                && x.Status == eRecordStatus.Active)
                    .Select((o) => new
                    {
                        id = o.Id,
                        name = o.Name
                    })
                .OrderBy(o => o.name)
                .ToList(); ;
        }

        public dynamic GetPlace(string version, string name)
        {
            var locations = new List<string>();
            long prvId = 0;
            if (name.IndexOf("~") > -1)
            {
                locations = name.Split("~").ToList();
            }
            var resultList = new List<dynamic>();

            for (short i = 0; i < locations.Count; i++)
            {
                var location = locations[i];
                var result = _vLocationRepo.GetAll()
                .Where(x => x.Name == location && x.Status == eRecordStatus.Active && (x.ParentId == prvId || prvId == 0))
                    .Select((o) => new
                    {
                        id = o.Id,
                        name = o.FullName
                    })
                .OrderBy(o => o.name)
                .ToList();
                prvId = result.FirstOrDefault().id;
                resultList.Add(result.FirstOrDefault());
            }
            return resultList;
        }

        public dynamic GetLocation(string version, long location_id)
        {
            var location = new List<dynamic>();
            var record = _vLocationRepo.GetAll()
                .Where(x => x.Id == location_id)
                    .Select((o) => new
                    {
                        id = o.Id,
                        name = o.FullName,
                        status = o.Status,
                        
                    })
                .OrderBy(o => o.name)
                .ToList();

            foreach (var attr in record)
            {
                var tempDict = new Dictionary<string, dynamic>();

                tempDict.Add("id", attr.id);
                tempDict.Add("name", attr.name);
                tempDict.Add("status", attr.status);

                var comment = string.Empty;
                if (!string.IsNullOrEmpty(comment))
                {
                    var lat = Utils.ProcessXML(comment, "lat");
                    if (!string.IsNullOrEmpty(lat)) { tempDict.Add("lat", lat); }

                    var lng = Utils.ProcessXML(comment, "lng");
                    if (!string.IsNullOrEmpty(lng)) { tempDict.Add("lng", lng); }

                    var language = Utils.ProcessXML(comment, "language");
                    if (!string.IsNullOrEmpty(language)) { tempDict.Add("language", language); }

                    var currency_id = Utils.ProcessXML(comment, "currency_id");
                    if (!string.IsNullOrEmpty(currency_id)) { tempDict.Add("currency_id", currency_id); }

                    var currency_code = Utils.ProcessXML(comment, "currency_code");
                    if (!string.IsNullOrEmpty(currency_code)) { tempDict.Add("currency_code", currency_code); }

                    var currency_sign = Utils.ProcessXML(comment, "currency_sign");
                    if (!string.IsNullOrEmpty(currency_sign)) { tempDict.Add("currency_sign", currency_sign); }

                }
                location.Add(tempDict);
            }

            return location;
        }

        public dynamic GetFeaturedLocation(string version, long location_id)
        {
            var featuredLocations = new List<dynamic>();
            var locations = _vLocationRepo.GetAll().Where(x => x.Id == location_id && x.Status == eRecordStatus.Active)
                    .Select((o) => new
                    {
                        id = o.Id,
                        name = o.Name,
                        status = o.Status, 
                        desc1 = o.Desc1,
                        desc2 = o.Desc2,
                        image_url = o.ImageUrl,
                        fetured = o.Featured,
                    }).ToList();
            
            foreach (var location in locations)
            {
                var dict = new Dictionary<string, dynamic>
                {
                    { "id", location.id },
                    { "name", location.name },
                    { "desc1", location.desc1 },
                    { "desc2", location.desc2 },
                    { "image_url", location.image_url }
                };
                var featured = location.fetured;

                var isns = featured.Split(",").Select((s) => { return Convert.ToInt64(s); }).ToList();
                var flocations = _vLocationRepo.GetAll().Where(x => isns.Contains(x.Id))
                    .Select((o) => new {
                        id = o.Id,
                        desc1 = o.Desc1,
                        desc2 = o.Desc2,
                        image_url = o.ImageUrl
                    }).ToList();

                dict.Add("featured", flocations);
                featuredLocations.Add(dict);
            }

            return featuredLocations;
        }

        public dynamic GetLocationFeaturedEntities(string version, int id, short status)
        {
            var defaultsSection = _configuration.GetSection($"settings:v{version}:defaults");
            var location_status = status > 0 ? status : defaultsSection.GetValue<short>("status");
           
            var locationEntity = _vLocationRepo.GetById(id);
            if (locationEntity == null)
                return new ExpandoObject();

            var localFeaturedEntities = new List<dynamic>();

            var entityIds = new List<long>();
            var featuredEntitiesIds = (from e in _vLocationRepo.GetAll()
                                where e.Id == id
                                && e.Status == (eRecordStatus)location_status
                                orderby e.FullName
                                select e.FeaturedEntities).FirstOrDefault();

            if (!string.IsNullOrEmpty(featuredEntitiesIds))
                entityIds = featuredEntitiesIds.Split(",").Select((s) => { return Convert.ToInt64(s); }).ToList();

            var featuredEntities = _entityRepo.GetAll()
                                .Where(x => entityIds.Contains(x.Id))
                                .Select((o) => new
                                {
                                    id = o.Id,
                                    name = o.FullName
                                })
                                .OrderByDescending(o => o.id).ToList();
            var ratings = _profileAttrRepo.GetAll()
                            .Where(x => entityIds.Contains(x.EntityId)                           
                            && x.ProfileSubtype == eProfileSubType.Profile1)
                            .Select((o) => new
                            {
                                id = o.EntityId,
                                rating = o.Rating   
                            });


            foreach (var v in entityIds)
            {
                var dict = new Dictionary<string, dynamic>();
                var eoObject = featuredEntities.Where(x => x.id == v).FirstOrDefault();
                if (eoObject is null)
                    continue;
                var rating = ratings.Where(x => x.id == v).FirstOrDefault()?.rating.ToString("0.00");
                var categories = (from r1 in _relationEntityRepo.GetAll()
                                  join r2 in _locationCategoryRepo.GetAll() on r1.Entity1Id equals r2.Id
                                  where r1.Entity2Id == v
                                  orderby r2.Entity2Id
                                  select new
                                  {
                                      category_id = r2.Entity2Id,
                                      category_name = r2.Entity2Name
                                  }).FirstOrDefault();

                dict.Add("id", eoObject.id);
                dict.Add("name", eoObject.name);
                dict.Add("rating", rating);
                dict.Add("categories", categories);

                localFeaturedEntities.Add(dict);
            }


            return localFeaturedEntities;
        }

        public dynamic GetLocationEnities(string version, int id, short status, string entity_list_type)
        {
            var defaultsSection = _configuration.GetSection($"settings:v{version}:defaults");
            var location_status = status > 0 ? status : defaultsSection.GetValue<short>("status");            
            var locationEntity = _vLocationRepo.GetById(id);
            if (locationEntity == null)
                return new ExpandoObject();

            var localFeaturedEntities = new List<dynamic>();

            var entityIds = new List<long>();
            var featuredEntitiesStr = (from e in _vLocationRepo.GetAll()
                                where e.Id == id
                                && e.Status == (eRecordStatus)location_status
                                orderby e.FullName
                                select e.FeaturedEntities).FirstOrDefault();

            if (!string.IsNullOrEmpty(featuredEntitiesStr))
                entityIds = featuredEntitiesStr.Split(",").Select((s) => { return Convert.ToInt64(s); }).ToList();

            var featuredEntities = _entityRepo.GetAll()
                                .Where(x => entityIds.Contains(x.Id))
                                .Select((o) => new
                                {
                                    id = o.Id,
                                    name = o.FullName
                                })
                                .OrderByDescending(o => o.id).ToList();
            var ratings = _profileAttrRepo.GetAll()
                            .Where(x => entityIds.Contains(x.EntityId))
                            .Select((o) => new
                            {
                                id = o.EntityId,
                                rating = o.Rating
                            });


            foreach (var v in entityIds)
            {
                var dict = new Dictionary<string, dynamic>();
                var eoObjec = _entityRepo.GetById(v);
                var rating = ratings.Where(x => x.id == v).FirstOrDefault()?.rating.ToString("0.00");
                var categories = (from r1 in _relationEntityRepo.GetAll()
                                  join r2 in _locationCategoryRepo.GetAll() on r1.Entity1Id equals r2.Id
                                  where r1.Entity2Id == v
                                  orderby r2.Entity2Id ascending
                                  select new
                                  {
                                      category_id = r2.Entity2Id,
                                      category_name = r2.Entity2Name
                                  }).FirstOrDefault();

                dict.Add("id", eoObjec.Id);
                dict.Add("name", eoObjec.FullName);
                dict.Add("rating", rating);
                dict.Add("categories", categories); 
                if(!(eoObjec.Desc1 is null))
                dict.Add("desc1", eoObjec.Desc1);

                localFeaturedEntities.Add(dict);
            }

            return localFeaturedEntities;
        }

        public dynamic GetLocationCategories(string version, long id, long location_id, int hisn5, string list_type, string category_string)
        {
            var section = _configuration.GetSection("proman_codes");
            long category_isn = 0;
            short list_type_requested = 0;
            if (string.IsNullOrEmpty(list_type))
            {
                list_type = "featured_categories";
                list_type_requested = 0;
            }
            else
            {
                list_type_requested = 1;
            }

            var eoList = (from eo in _vLocationRepo.GetAll()
                          where eo.Id == location_id && eo.Status == eRecordStatus.Active
                          select new
                          {
                              //eolist = Utils.ProcessXML(eo.MetaData, list_type)
                              eolist = eo.Featured
                          }).FirstOrDefault()?.eolist;

            if (!string.IsNullOrEmpty(category_string))
            {
                category_isn = (from e in _vClassificationRepo.GetAll()
                                where e.Name == category_string && e.Status == eRecordStatus.Active
                                orderby e.Id descending
                                select e.Id).FirstOrDefault();
            }
            IQueryable<LocationCategoryRelation> cats;
            if (category_isn > 0)
            {
                cats = from r in _locationCategoryRepo.GetAll()
                       where r.RelationType == eRelationTypes.LOCATION_CATEGORY
                       && r.RelationSubtype == eEoSubTypes.INSTANCE
                       && r.Status == eRecordStatus.Active
                       && r.Entity1Id == location_id
                       && r.Entity2Id == category_isn
                       select r;
            }
            else
            {
                cats = from r in _locationCategoryRepo.GetAll()
                       where r.RelationType == eRelationTypes.LOCATION_CATEGORY
                       && r.RelationSubtype == eEoSubTypes.INSTANCE
                       && r.Entity1Id == location_id
                       select r;
                if (!string.IsNullOrEmpty(eoList) && list_type_requested == 1)
                {
                    var isnArray = eoList.Split(",").Select(x => Convert.ToInt64(x)).ToList();
                    cats = cats.Where(x => isnArray.Contains(x.Id));
                }
                cats = cats.Where(x => x.Status == eRecordStatus.Active);

                if (id > 0)
                {
                    var isns = _vClassificationRepo.GetAll().Where(x => x.Id == id )
                        .Where(x => x.EoSubtype == eEoSubTypes.INSTANCE && x.Status == eRecordStatus.Active)
                        .Select(x => Convert.ToInt64(x.Id)).ToList();

                    cats = cats.Where(x => isns.Contains(x.Entity2Id));
                }
                cats = cats.OrderBy(x => x.Entity2Id);
            }

            var results = cats.Select((r) => new
            {
                id = r.Id,
                category_id = r.Entity2Id,
                category_type = r.Entity2Type,
                name = r.Entity2Name
                //r.long_comment
            }).ToList();


            return results;
        }

        public dynamic GetCategories(string version, string list_type)
        {
            return _classificationRepo.GetAll()
                .Where(x => x.ParentId == 0  && x.EoSubtype == eEoSubTypes.INSTANCE && x.Status == eRecordStatus.Active
                        )
                    .Select(
                    (o) => new
                    {
                        id = o.Id,
                        type = o.EoType,
                        name = o.FullName,
                        image_url = string.Empty,
                        desc1 = string.Empty
                    }
                )
                .OrderBy(o => o.id)
                .ToList();          
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
