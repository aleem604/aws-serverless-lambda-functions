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
    public class EntityObjectService : IEntityObjectService
    {
        private readonly IMapper _mapper;
        private readonly IProfileRepository _profileRepository;
        private readonly IRelationRepository _relationRepository;
        private readonly ITinContactRepository _contactRepository;
        private readonly IConfiguration _configuration;
        private readonly IEntityObjectsRepository _entityObjectsRepository;
        private readonly ITrackingRepository _trackingRepository;
        private readonly IMediatorHandler _bus;

        public EntityObjectService(IMapper mapper,
                                  IProfileRepository profileRepository,
                                  IRelationRepository relationRepository,
                                  ITinContactRepository contactRepository,
                                  IEntityObjectsRepository entityObjectsRepository,
                                  ITrackingRepository trackingRepository,
                                  IConfiguration configuration,
                                  IMediatorHandler bus)
        {
            _mapper = mapper;
            _profileRepository = profileRepository;
            _relationRepository = relationRepository;
            _contactRepository = contactRepository;
            _entityObjectsRepository = entityObjectsRepository;
            _trackingRepository = trackingRepository;
            _configuration = configuration;
            _bus = bus;
        }
        public dynamic GetEntitiesList(string version, long id, long location_id, string url, int hisn1, string name)
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
                id = IdentifyCatgory(ref url, section);
            }

            var query0 = from r1 in _relationRepository.GetAll()
                         join r2 in _relationRepository.GetAll() on r1.isn equals r2.eo1_isn
                         where r2.status == section.GetValue<short>("STATUS~ACTIVE")
                         join eo in _entityObjectsRepository.GetAll() on r2.eo2_isn equals eo.isn
                         where eo.status == section.GetValue<short>("STATUS~ACTIVE")
                         join t in _trackingRepository.GetAll() on eo.isn equals t.eo_isn
                         where t.eo_type == eo.eo_type && t.eo_subtype == eo.eo_subtype
                         && t.tracking_type == section.GetValue<short>("EO_TYPE~SUBSCRIPTION")
                         && t.start_datetime < DateTime.Now && t.end_datetime > DateTime.Now
                         && t.status == section.GetValue<short>("STATUS~ACTIVE")
                         join e2 in _entityObjectsRepository.GetAll() on t.hisn1 equals e2.isn
                         where (new List<int>() { 2, 3, 4 }).Contains(e2.hisn5)
                         join e3 in _entityObjectsRepository.GetAll() on r1.eo2_isn equals e3.isn
                         select new { r1, r2, eo, t, e2, e3 };
            if (id > 0)
            {
                query0 = query0.Where(x => x.e3.hisn1 == id);
            }
            if (id == 0)
            {
                query0 = query0.Where(x => x.r1.relation_type == section.GetValue<short>("RELATION~LOCATION_EO"));
            }
            else
            {
                query0 = query0.Where(x => x.r1.relation_type == section.GetValue<short>("RELATION~LOCATION_CATEGORY"));
            }
            query0 = query0.Where(x => x.r1.relation_subtype == section.GetValue<short>("EO_TYPE~INSTANCE") && x.r1.eo1_isn == location_id);

            if (!string.IsNullOrEmpty(name))
                query0 = query0.Where(x => x.r2.eo2_name.Contains(name));

            var query1 = from r1 in _relationRepository.GetAll()
                         join r2 in _relationRepository.GetAll() on r1.isn equals r2.eo1_isn
                         where r2.status == section.GetValue<short>("STATUS~ACTIVE")
                         join eo in _entityObjectsRepository.GetAll() on r2.eo2_isn equals eo.isn
                         join t in _trackingRepository.GetAll() on eo.isn equals t.EntityId
                         where t.EOType ==(eEOTypes) eo.eo_type
                         && t.EOSubtype == (eEOSubTypes) eo.eo_subtype && t.TrackingType == eTrackingType.EO_TYPE_SUBSCRIPTION
                         && t.StartDate < DateTime.Now && t.EndDate > DateTime.Now
                         && t.Status == eRecordStatus.Active
                         join e2 in _entityObjectsRepository.GetAll() on t.EntitySubscriptionId equals e2.isn
                         where (new List<int>() { 2, 3, 4 }).Contains(e2.hisn5)
                         && r1.relation_type == (id == 0 ? section.GetValue<short>("RELATION~LOCATION_EO") : section.GetValue<short>("RELATION~LOCATION_CATEGORY"))
                         && r1.relation_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                         && r1.eo1_isn == location_id
                         select new { r1, r2, eo, t, e2 };
            if (id > 0)
                query1 = query1.Where(x => x.r1.eo2_isn == id);
            if (!string.IsNullOrEmpty(name))
                query1 = query1.Where(x => x.r2.eo1_name.Contains(name));

            var ids1 = query0.Select(x => x.r2.eo2_isn).Distinct().ToList();
            var ids2 = query1.Select(x => x.r2.eo2_isn).Distinct().ToList();
            var isns = ids1.Union(ids2).ToList();

            var entityQuery = (from eo in _entityObjectsRepository.GetAll()
                               join p in _profileRepository.GetAll() on eo.isn equals p.eo_isn
                               where p.profile_type == section.GetValue<short>("PROFILE~ATTRIBUTES") && p.profile_subtype == section.GetValue<short>("PROFILE~1")
                               join p2 in _profileRepository.GetAll() on eo.isn equals p2.eo_isn
                               where p2.profile_type == section.GetValue<short>("PROFILE~ATTRIBUTES") && p2.profile_subtype == section.GetValue<short>("PROFILE~2")
                               join t in _trackingRepository.GetAll() on eo.isn equals t.eo_isn
                               where t.eo_type == eo.eo_type && t.eo_subtype == eo.eo_subtype && t.tracking_type == section.GetValue<short>("EO_TYPE~SUBSCRIPTION")
                               && t.start_datetime < DateTime.Now && t.end_datetime > DateTime.Now && t.status == section.GetValue<short>("STATUS~ACTIVE")
                               join e2 in _entityObjectsRepository.GetAll() on t.hisn1 equals e2.isn
                               where (new List<int>() { 2, 3, 4 }).Contains(e2.hisn5)
                               join c1 in _contactRepository.GetAll() on eo.isn equals c1.eo_isn
                               where eo.eo_type == c1.eo_type && eo.eo_subtype == c1.eo_subtype && c1.contact_type == section.GetValue<short>("CONTACT~BUSINESS")
                               && isns.Contains(eo.isn)
                               orderby eo.isn
                               select new
                               {
                                   id = eo.isn,
                                   name = eo.long_name,
                                   desc1 = Utils.ProcessXML(p.profile9, "desc1"),
                                   desc2 = Utils.ProcessXML(p.profile9, "desc2"),
                                   price = p.profile7,
                                   image_url = p.profile6,
                                   rating = p.profile1,
                                   card_type = GetCardType(e2.hisn5),
                                   lat = p2.profile6,
                                   lng = p2.profile7,
                                   number_of_likes = "",
                                   number_of_views = "",
                                   full_address = GetFullAddress(c1.contact1, c1.city, c1.state, c1.country, c1.postal_code)
                               }).ToList();
            if (entityQuery.Count == 0 && id == 0)
            {
                var query_e2_isn = from r1 in _relationRepository.GetAll()
                                   join eo in _entityObjectsRepository.GetAll() on r1.eo2_isn equals eo.isn
                                   where eo.status == section.GetValue<short>("STATUS~ACTIVE")
                                   && r1.relation_type == section.GetValue<short>("RELATION~LOCATION_EO")
                                    && r1.relation_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                                    && r1.eo1_isn == id
                                   select new { r1, eo };
                if (!string.IsNullOrEmpty(name))
                    query_e2_isn = query_e2_isn.Where(x => x.r1.eo2_name.Contains(name));

                var e2_isns = query_e2_isn.Select(o => o.r1.eo2_isn).Distinct().ToList();

                var categories = (from eo in _entityObjectsRepository.GetAll()
                                  join p in _profileRepository.GetAll() on eo.isn equals p.eo_isn
                                  where p.profile_type == section.GetValue<short>("PROFILE~ATTRIBUTES")
                                  && p.profile_subtype == section.GetValue<short>("PROFILE~1")
                                  join p2 in _profileRepository.GetAll() on eo.isn equals p2.eo_isn
                                  where p2.profile_type == section.GetValue<short>("PROFILE~ATTRIBUTES")
                                  && p2.profile_subtype == section.GetValue<short>("PROFILE~2")
                                  join t in _trackingRepository.GetAll() on eo.isn equals t.eo_isn
                                  where t.eo_type == eo.eo_type && t.eo_subtype == eo.eo_subtype
                                  && t.tracking_type == section.GetValue<short>("EO_TYPE~SUBSCRIPTION")
                                  join e2 in _entityObjectsRepository.GetAll() on t.hisn1 equals e2.isn
                                  join c1 in _contactRepository.GetAll() on eo.isn equals c1.eo_isn
                                  where eo.eo_type == c1.eo_type && eo.eo_subtype == c1.eo_subtype
                                  && c1.contact_type == section.GetValue<short>("CONTACT~BUSINESS")
                                  && e2_isns.Contains(eo.isn)
                                  select new
                                  {
                                      id = eo.isn,
                                      name = eo.long_name,
                                      desc1 = Utils.ProcessXML(p.profile9, "desc1"),
                                      desc2 = Utils.ProcessXML(p.profile9, "desc2"),
                                      price = p.profile7,
                                      image_url = p.profile6,
                                      rating = string.Format("{0:0.00}", p.profile1),
                                      card_type = GetCardType(e2.hisn5),
                                      lat = p2.profile6,
                                      lng = p2.profile7,
                                      number_of_likes = "",
                                      number_of_views = "",
                                      full_address = GetFullAddress(c1.contact1, c1.city, c1.state, c1.country, c1.postal_code)
                                  }).ToList();
                if (categories.Count > 0 && id == 0)
                {
                    var locations = GetLocationsFromUrl(url);
                    var result = new Dictionary<string, dynamic>();
                    for (int k = 0; k < categories.Count; k++)
                    {
                        if (k == 0 || (k > 0 && categories[k - 1].id != categories[k].id))
                        {
                            for (int i = 0; i < locations.Count; i++)
                            {
                                if (locations[i] == "category")
                                {
                                    result.TryAdd("categories", locations);
                                }
                                else
                                {
                                    result.TryAdd(locations[i].ToString(), locations);
                                }
                            }
                        }
                        else
                        {
                            //              if (isset($res[$k]["category"]))
                            //              {
                            //$r[$j - 1]["categories"][$n] =$res[$k]["category"];
                            //$n++;
                            //              }
                        }
                    }
                }
            }
            var resultQuery = new List<dynamic>();
            if (entityQuery.Count > 0)
            {
                foreach (var eq in entityQuery)
                {
                    var tempDic = new Dictionary<string, dynamic>();
                    foreach (var k in eq.GetType().GetProperties())
                    {
                        tempDic.TryAdd(k.Name, k.GetValue(eq));
                    }
                    var attrs = (from r in _relationRepository.GetAll()
                                 join eo in _entityObjectsRepository.GetAll() on r.eo2_isn equals eo.isn
                                 where r.eo2_type == eo.eo_type && r.eo2_subtype == eo.eo_subtype
                                 && r.eo1_isn == eq.id && r.eo1_type == section.GetValue<short>("EO_TYPE~PERSON")
                                 && r.eo1_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                                 && r.relation_type == section.GetValue<short>("RELATION~EO_ATTRIBUTE")
                                 && r.relation_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                                 select new
                                 {
                                     name = eo.name1,
                                     image_url = eo.name3
                                 }).ToList();

                    var attrList = new List<dynamic>();
                    foreach (var attr in attrs)
                    {
                        var d = new Dictionary<string, string>();
                        d.TryAdd("name", attr.name);

                        foreach (var icon in tinIconArray)
                        {
                            d.TryAdd($"image_url{icon.Key}", $"{defaultsSection.GetValue<string>("amazon_tininfo_url_prefix")}{icon.Value}{attr.image_url}");
                        }
                        attrList.Add(d);
                    }
                    tempDic.Add("attributes", attrList);
                    resultQuery.Add(tempDic);
                }
            }

            return resultQuery;
        }


        public dynamic GetEntity(string version, long id, string tag, string search)
        {
            var section = _configuration.GetSection("proman_codes");
            var defaultsSection = _configuration.GetSection($"settings:v{version}:defaults");

            short eo_type = section.GetValue<short>("EO_TYPE~CLASSIFICATION");
            short or_eo_type = section.GetValue<short>("EO_TYPE~CLASSIFICATIONTYPE");
            short eo_subtype = section.GetValue<short>("EO_TYPE~INSTANCE");
            short status = section.GetValue<short>("STATUS~ACTIVE");
            var entityResults = new List<Dictionary<string, dynamic>>();

            var query = from eo in _entityObjectsRepository.GetAll()
                        join c in _contactRepository.GetAll() on eo.isn equals c.eo_isn
                        where eo.eo_type == c.eo_type && eo.eo_subtype == c.eo_subtype
                        join p in _profileRepository.GetAll() on eo.isn equals p.eo_isn
                        where eo.eo_type == section.GetValue<short>("EO_TYPE~PERSON")
                        && eo.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                        && c.contact_type == section.GetValue<short>("CONTACT~BUSINESS")
                        && c.contact_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                        && p.profile_type == section.GetValue<short>("PROFILE~ATTRIBUTES")
                        && p.profile_subtype == section.GetValue<short>("PROFILE~1")
                        && eo.status == section.GetValue<short>("STATUS~ACTIVE")
                        select new { eo, c, p };

            if (id == 0 && !string.IsNullOrEmpty(tag))
                query = query.Where(x => x.eo.name1.Contains(tag));
            else if (id == 0 && !string.IsNullOrEmpty(search))
                query = query.Where(x => x.eo.long_name.Contains(search));
            else
                query = query.Where(x => x.eo.isn == id);

            var entities = query.Select((o) => new
            {
                id = o.eo.isn,
                tag = o.eo.name5,
                name = o.eo.long_name,
                address = o.c.contact1,
                address2 = o.c.contact2,
                o.c.city,
                o.c.state,
                o.c.country,
                o.c.phone,
                email = o.c.contact7,
                website = o.c.contact8,
                image_url = o.p.profile6,
                rating = o.p.profile1,
                desc1 = Utils.ProcessXML(o.p.profile9, "desc1"),
                desc2 = Utils.ProcessXML(o.p.profile9, "desc2"),
                social_media = Utils.ProcessXML(o.p.profile9, "social_media").DeSerialize(),
                price = o.p.profile7,
                number_of_like = "",
                number_of_views = ""
            });

            var isns = entities.Select(x => x.id).ToList();

            var locations = (from p in _profileRepository.GetAll()
                             where isns.Contains(p.eo_isn)
                             && p.profile_type == section.GetValue<short>("PROFILE~ATTRIBUTES")
                             && p.profile_subtype == section.GetValue<short>("PROFILE~2")
                             select new
                             {
                                 id = p.eo_isn,
                                 lat = p.profile6,
                                 lng = p.profile7

                             }).ToList();

            var locationIds = (from r in _relationRepository.GetAll()
                               where r.relation_type == section.GetValue<short>("RELATION~LOCATION_EO")
                              && r.relation_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                              && isns.Contains(r.eo2_isn)
                              && status == section.GetValue<short>("STATUS~ACTIVE")
                               orderby r.isn
                               select new
                               {
                                   rid = r.eo2_isn,
                                   id = r.eo1_isn
                               }).ToList();
            var relationTexts = (from r in _relationRepository.GetAll()
                                 join r2 in _relationRepository.GetAll() on r.eo1_isn equals r2.isn
                                 join eo in _entityObjectsRepository.GetAll() on r2.eo2_isn equals eo.isn
                                 join e2 in _entityObjectsRepository.GetAll() on eo.hisn1 equals e2.isn
                                 join e3 in _entityObjectsRepository.GetAll() on e2.hisn1 equals e3.isn
                                 where isns.Contains(r.eo2_isn) && r.status == 1
                                 select new
                                 {
                                     id = r.eo2_isn,
                                     c1 = e3.name1,
                                     c2 = e2.name1,
                                     c3 = eo.name1
                                 }).ToList();

            var attributes = (from r1 in _relationRepository.GetAll()
                              join eo in _entityObjectsRepository.GetAll() on r1.eo2_isn equals eo.isn
                              where r1.eo2_type == eo.eo_subtype
                              && isns.Contains(r1.eo1_isn)
                              && r1.eo1_type == section.GetValue<short>("EO_TYPE~PERSON")
                              && r1.eo1_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                              && r1.relation_type == section.GetValue<short>("RELATION~EO_ATTRIBUTE")
                              && r1.relation_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                              && r1.status == section.GetValue<short>("STATUS~ACTIVE")
                              select new
                              {
                                  rid = r1.eo1_isn,
                                  id = r1.isn,
                                  name = eo.name1,
                                  image_url = eo.name3
                              }).ToList();

            var categories = (from r in _relationRepository.GetAll()
                              join r2 in _relationRepository.GetAll() on r.eo1_isn equals r2.isn
                              join eo in _entityObjectsRepository.GetAll() on r2.eo2_isn equals eo.isn
                              where r.relation_type == section.GetValue<short>("RELATION~RELATION_RELATION")
                              && r.relation_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                              && r.eo2_type == section.GetValue<short>("EO_TYPE~PERSON")
                              && r.eo2_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                              && r.eo1_type == section.GetValue<short>("RELATION~LOCATION_CATEGORY")
                              && r.eo1_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                              && isns.Contains(r.eo2_isn)
                              select new
                              {
                                  rid = r.eo2_isn,
                                  id = r.isn,
                                  location_id = r2.eo1_isn,
                                  category_id = r2.eo2_isn,
                                  section_id = eo.hisn1,
                                  r.status,
                                  eo.hierarchy_string
                              }).ToList();

            foreach (var entity in entities)
            {
                var e = new Dictionary<string, dynamic>();
                foreach (var prop in entity.GetType().GetProperties())
                    e.TryAdd(prop.Name, prop.GetValue(entity));

                // location ids addition
                foreach (var location in locations.Where(x => x.id == entity.id).ToList())
                    foreach (var prop in location.GetType().GetProperties())
                        e.TryAdd(prop.Name, prop.GetValue(location));

                e.TryAdd("location_ids", locationIds.Where(x => x.rid == entity.id).ToList().Select(x => new { id = x.id }));

                // meta attributes
                foreach (var r2 in relationTexts.Where(x => x.id == entity.id))
                {
                    e.TryAdd("head_title", $"{entity.name}, listed in the {r2.c1} section {r2.c2}, {r2.c3} of city {entity.city}");
                    e.TryAdd("head_meta_description", $"company {entity.name} listing in {r2.c1}, {r2.c2}, {r2.c3} in {entity.city} city business directory");
                    e.TryAdd("head_meta_keywords", $"{entity.name}, {r2.c1} {r2.c2} {r2.c3} {entity.city} listing");
                }

                // attributes section
                var entityAttrs = new List<Dictionary<string, dynamic>>();
                var tinIconArray = _configuration.GetSection($"settings:v{version}:defaults:entity_attribute_icons_array")
                                    .GetChildren().Select(item => new KeyValuePair<string, string>(item.Key, item.Value))
                                    .ToDictionary(x => x.Key, x => x.Value);
                foreach (var attr in attributes.Where(x => x.rid == entity.id))
                {
                    var d = new Dictionary<string, dynamic>();
                    d.TryAdd("id", attr.id);
                    d.TryAdd("name", attr.name);
                    foreach (var icon in tinIconArray)
                    {
                        d.TryAdd($"image_url{icon.Key}", $"{defaultsSection.GetValue<string>("amazon_tininfo_url_prefix")}{icon.Value}{attr.image_url}");
                    }
                    entityAttrs.Add(d);
                }
                e.TryAdd("attributes", entityAttrs);

                // Categories section
                var categoriesList = new List<Dictionary<string, dynamic>>();
                foreach (var cat in categories.Where(x => x.rid == entity.id).ToList())
                {
                    var catDict = new Dictionary<string, dynamic>();

                    foreach (var attr in cat.GetType().GetProperties())
                    {
                        if ((new List<string> { "rid", "hierarchy_string", "section_id" }).IndexOf(attr.Name) > -1) continue;
                        if (attr.Name == "category_id")
                        {
                            catDict.TryAdd("category_id", cat.hierarchy_string?.Split("-")[1]);
                            catDict.TryAdd("section_id", cat.hierarchy_string?.Split("-")[0]);
                        }
                        else
                            catDict.TryAdd(attr.Name, attr.GetValue(cat));
                    }
                    categoriesList.Add(catDict);
                }
                e.TryAdd("categories", categoriesList);

                entityResults.Add(e);
            }

            return entityResults;
        }

        public dynamic GetEntity2(string version, long id, string tag)
        {
            var section = _configuration.GetSection("proman_codes");
            var defaultsSection = _configuration.GetSection($"settings:v{version}:defaults");
            var resultEntities = new List<Dictionary<string, dynamic>>();

            var query = from eo in _entityObjectsRepository.GetAll()
                        join c in _contactRepository.GetAll() on eo.isn equals c.eo_isn
                        where eo.eo_type == c.eo_type && eo.eo_subtype == c.eo_subtype
                        join p in _profileRepository.GetAll() on eo.isn equals p.eo_isn
                        where eo.eo_type == section.GetValue<short>("EO_TYPE~PERSON")
                        && eo.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                        && c.contact_type == section.GetValue<short>("CONTACT~PRIMARY")
                        && c.contact_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                        && p.profile_type == section.GetValue<short>("PROFILE~ATTRIBUTES")
                        && p.profile_subtype == section.GetValue<short>("PROFILE~1")
                        && eo.status == section.GetValue<short>("STATUS~ACTIVE")
                        select new { eo, c, p };

            if (id == 0 && !string.IsNullOrEmpty(tag))
                query = query.Where(x => x.eo.name5.Contains(tag));
            else
                query = query.Where(x => x.eo.isn == id);

            var entities = query
                            .Select((o) => new
                            {
                                id = o.eo.isn,
                                name = o.eo.long_name,
                                address = o.c.contact1,
                                o.c.city,
                                o.c.state,
                                o.c.country,
                                o.c.phone,
                                email = o.c.contact7,
                                website = o.c.contact8,
                                image_url = string.IsNullOrEmpty(o.p.profile6) ? "" : $"{defaultsSection.GetValue<string>("img_url_prefix")}{o.p.profile6}",
                                rating = o.p.profile1,
                                desc1 = Utils.ProcessXML(o.p.profile9, "desc1"),
                                desc2 = Utils.ProcessXML(o.p.profile9, "desc2"),
                                price = o.p.profile7,
                                lat = "",
                                lng = "",
                                number_of_likes = "",
                                number_of_views = ""
                            }).ToList();

            if (id == 0 && !string.IsNullOrEmpty(tag) && entities.FirstOrDefault().id > 0)
                id = entities.FirstOrDefault().id;

            var attributes = (from r in _relationRepository.GetAll()
                              join eo in _entityObjectsRepository.GetAll() on r.eo2_isn equals eo.isn
                              where r.eo2_type == eo.eo_type && r.eo2_subtype == eo.eo_subtype
                              && r.eo1_isn == id && r.eo1_type == section.GetValue<short>("EO_TYPE~PERSON")
                              && r.eo1_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                              && r.relation_type == section.GetValue<short>("RELATION~EO_ATTRIBUTE")
                              && r.relation_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                              select new
                              {
                                  name = eo.name1,
                                  image_url = $"{defaultsSection.GetValue<string>("site_url_prefix")}{defaultsSection.GetValue<string>("entity_attribute_icons")}{eo.name3}"
                              }).ToList();
            foreach (var entity in entities)
            {
                var dict = new Dictionary<string, dynamic>();
                foreach (var e in entity.GetType().GetProperties())
                {
                    dict.TryAdd(e.Name, e.GetValue(entity));
                }
                dict.Add("attributes", attributes);

                resultEntities.Add(dict);
            }

            return resultEntities;
        }

        public dynamic GetEntityGallaries(string version, long id)
        {
            var section = _configuration.GetSection("proman_codes");
            short eo_type = section.GetValue<short>("EO_TYPE~GALLERY");
            short eo_subtype = section.GetValue<short>("EO_TYPE~INSTANCE");
            short status = section.GetValue<short>("STATUS~ACTIVE");

            return _entityObjectsRepository.GetAll()
                .Where(x => x.hisn1 == id)
                .Where(x => x.eo_type == eo_type)
                .Where(x => x.eo_subtype == eo_subtype)
                .Where(x => x.status == 1)
                .Select((o) => new
                {
                    id = o.isn,
                    name = o.name1
                })
                .OrderBy(x => x.id)
                .ToList();
        }

        public dynamic GetGallaryImages(string version, long id, long entity_id)
        {
            var section = _configuration.GetSection("proman_codes");

            var gallaryIds = _entityObjectsRepository.GetAll()
                            .Where(x => x.hisn1 == entity_id)
                            .Where(x => x.eo_type == section.GetValue<short>("EO_TYPE~GALLERY"))
                            .Where(x => x.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE"))
                            .Where(x => x.status == section.GetValue<short>("STATUS~ACTIVE"))
                            .Select((o) => new
                            {
                                o.isn
                            })
                            .ToList()
                            .Select((o) =>
                            {
                                return o.isn;
                            }).ToList() ?? new List<int> { Convert.ToInt32(id) };

            gallaryIds = gallaryIds.Count() > 0 ? gallaryIds : new List<int> { Convert.ToInt32(id) };

            return _entityObjectsRepository.GetAll()
                .Where(x => gallaryIds.Contains(x.hisn1))
                .Where(x => x.eo_type == section.GetValue<short>("EO_TYPE~IMAGE"))
                .Where(x => x.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE"))
                .Where(x => x.status == section.GetValue<short>("STATUS~ACTIVE"))
                .Select((o) => new
                {
                    id = o.isn,
                    image_title = o.long_name,
                    image_desc = o.description,
                    image_url = o.long_name
                })
                .OrderBy(x => x.id)
                .ToList();
        }

        public dynamic GetGallaryVideos(string version, long id, long entity_id)
        {
            var section = _configuration.GetSection("proman_codes");

            var videoIds = _entityObjectsRepository.GetAll()
                            .Where(x => x.hisn1 == entity_id)
                            .Where(x => x.eo_type == section.GetValue<short>("EO_TYPE~GALLERY"))
                            .Where(x => x.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE"))
                            .Where(x => x.status == section.GetValue<short>("STATUS~ACTIVE"))
                            .Select((o) => new
                            {
                                o.isn
                            })
                            .ToList()
                            .Select((o) =>
                            {
                                return o.isn;
                            }).ToList() ?? new List<int> { Convert.ToInt32(id) };

            videoIds = videoIds.Count() > 0 ? videoIds : new List<int> { Convert.ToInt32(id) };

            return _entityObjectsRepository.GetAll()
                .Where(x => videoIds.Contains(x.hisn1))
                .Where(x => x.eo_type == section.GetValue<short>("EO_TYPE~VIDEO"))
                .Where(x => x.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE"))
                .Where(x => x.status == section.GetValue<short>("STATUS~ACTIVE"))
                .Select((o) => new
                {
                    id = o.isn,
                    video_title = o.long_name,
                    video_desc = o.description,
                    video_url = Utils.ProcessXML(o.long_name, "video_url")
                })
                .OrderBy(x => x.id)
                .ToList();
        }

        public IEnumerable<EntityObject> GetLocationEvents(string version, long hisn1)
        {
            return _entityObjectsRepository.GetLocationEvents(hisn1);
        }

        public dynamic GetEntityFolders(string version, long id)
        {
            var section = _configuration.GetSection("proman_codes");
            var defaultsSection = _configuration.GetSection($"settings:v{version}:defaults");

            return _entityObjectsRepository.GetAll()
                .Where(x => x.hisn3 == id)
                .Where(x => x.eo_type == section.GetValue<short>("EO_TYPE~FOLDER"))
                .Where(x => x.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE"))
                .Where(x => x.status == section.GetValue<short>("STATUS~ACTIVE"))
                .Select((o) => new
                {
                    id = o.isn,
                    name = o.name1
                })
                .OrderBy(x => x.id)
                .ToList();
        }

        public dynamic GetEntityFolderFiles(string version, long id, long entity_id)
        {
            var section = _configuration.GetSection("proman_codes");

            var folderIds = _entityObjectsRepository.GetAll()
                            .Where(x => x.hisn3 == entity_id)
                            .Where(x => x.eo_type == section.GetValue<short>("EO_TYPE~FOLDER"))
                            .Where(x => x.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE"))
                            .Where(x => x.status == section.GetValue<short>("STATUS~ACTIVE"))
                            .Select((o) => new
                            {
                                o.isn
                            })
                            .ToList()
                            .Select((o) =>
                            {
                                return o.isn;
                            }).ToList() ?? new List<int> { Convert.ToInt32(id) };

            folderIds = folderIds.Count() > 0 ? folderIds : new List<int> { Convert.ToInt32(id) };
            var defaultsSection = _configuration.GetSection($"settings:v{version}:defaults");

            return _entityObjectsRepository.GetAll()
                .Where(x => folderIds.Contains(x.hisn1))
                .Where(x => x.eo_type == section.GetValue<short>("EO_TYPE~FILE"))
                .Where(x => x.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE"))
                .Where(x => x.status == section.GetValue<short>("STATUS~ACTIVE"))
                .Select((o) => new
                {
                    id = o.isn,
                    file_title = o.long_name,
                    file_desc = o.description,
                    file_url = $"{ defaultsSection.GetValue<string>("file_url_prefix")}{o.name1}"
                })
                .OrderBy(x => x.id)
                .ToList();
        }

        public dynamic GetFeaturedList(string version, long id, long entity_id)
        {
            var defaultsSection = _configuration.GetSection($"settings:v{version}:defaults");
            var section = _configuration.GetSection("proman_codes");

            dynamic featuredList = new ExpandoObject();
            featuredList.eo_name = "featured_list";
            featuredList.eo_desc = "";
            featuredList.eo_type = "ul";
            featuredList.img = "";
            featuredList.li = new ExpandoObject();

            var featuredQuery = (from e1 in _entityObjectsRepository.GetAll()
                                 join e2 in _entityObjectsRepository.GetAll() on e1.hisn1 equals e2.isn
                                 join e3 in _entityObjectsRepository.GetAll() on e2.hisn1 equals e3.isn
                                 where e1.eo_type == section.GetValue<short>("EO_TYPE~LOCATION") &&
                                 e1.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE") &&
                                 e1.isn == id
                                 select new
                                 {
                                     city = e1.long_name,
                                     region = e2.name2,
                                     country = e3.name3
                                 })
                                 .FirstOrDefault();

            var location_name = featuredQuery.city?.ToLower();
            var location_link = $"{featuredQuery.city?.ToLower()}-{featuredQuery.region?.ToLower()}-{featuredQuery.country?.ToLower()}";

            var query = from e1 in _entityObjectsRepository.GetAll()
                        join e2 in _entityObjectsRepository.GetAll() on e1.isn equals e2.hisn1
                        where e1.name1.Equals("featured_slider", StringComparison.InvariantCultureIgnoreCase)
                        select new { item_name = e2.long_name, item_link = e2.name1, e2.long_comment };
            featuredList.li = query.Take(4)
                             .Select((o) => new
                             {
                                 o.item_name,
                                 item_link = $"{defaultsSection.GetValue<string>("site_url_prefix")}/location/{location_link}/featured/{o.item_link}",
                                 item_image = $"{defaultsSection.GetValue<string>("site_url_prefix")}{Utils.ProcessXML(o.long_comment, "img")}"
                             })
                             .ToList();
            return featuredList;
        }

        public IEnumerable<EntityObject> GetCoupons(string version, long hisn1)
        {
            return _entityObjectsRepository.GetCoupons(hisn1);
        }

        public IEnumerable<EntityObject> GetEntities(string version, long id, long location_id)
        {
            return _entityObjectsRepository.GetEntities(id, location_id);
        }

        public dynamic GetEntityReviewCategories(string version, long id)
        {
            var section = _configuration.GetSection("proman_codes");

            short relation_type = section.GetValue<short>("RELATION~LOCATION_CATEGORY");
            short relation_subtype = section.GetValue<short>("EO_TYPE~INSTANCE");
            short eo_type = section.GetValue<short>("EO_TYPE~CLASSIFICATION");
            short or_eo_type = section.GetValue<short>("EO_TYPE~CLASSIFICATIONTYPE");
            short eo_subtype = section.GetValue<short>("EO_TYPE~INSTANCE");
            short status = section.GetValue<short>("STATUS~ACTIVE");

            //var r1 = _relationRepository;
            //var eo = _entityObjectsRepository;
            //var e2 = _entityObjectsRepository;

            var entityObjectISN = (from r1 in _relationRepository.GetAll()
                                   join r2 in _relationRepository.GetAll() on r1.isn equals r2.eo1_isn
                                   join e1 in _entityObjectsRepository.GetAll() on r2.eo2_isn equals e1.isn
                                   join e2 in _entityObjectsRepository.GetAll() on r1.eo2_isn equals e2.isn
                                   where r1.relation_type == relation_type && r1.relation_subtype == relation_subtype &&
                                   e1.isn == id && r2.status == 1
                                   select new
                                   {
                                       isn = e2.hisn1
                                       //r1.eo1_isn,
                                       //r1.eo2_isn,
                                       //id = r2.eo2_isn,
                                       //name = r2.eo2_name,
                                       //location = r1.eo1_name,
                                       //category = r1.eo2_name,
                                   }).FirstOrDefault();

            var entityObjectISN2 = _entityObjectsRepository.GetAll()
                                    .Where(x => x.hisn1 == entityObjectISN.isn)
                                    .Where(x => x.eo_type == section.GetValue<short>("EO_TYPE~REVIEW_CATEGORY_UL"))
                                    .Where(x => x.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE"))
                                    .Select((o) => new { o.isn })
                                    .FirstOrDefault();

            return _entityObjectsRepository.GetAll()
                    .Where(x => x.hisn1 == entityObjectISN2.isn)
                    .Where(x => x.eo_type == section.GetValue<short>("EO_TYPE~REVIEW_CATEGORY_LI"))
                    .Where(x => x.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE"))
                    .Select((o) => new
                    {
                        o.isn,
                        o.name1
                    })
                    .ToList();
        }

        public dynamic GetPageMenu(string version, long id)
        {
            var defaultsSection = _configuration.GetSection($"settings:v{version}:defaults");
            var section = _configuration.GetSection("proman_codes");

            dynamic featuredList = new ExpandoObject();
            featuredList.menu_structure = "SITE MAP";
            featuredList.eo_desc = "";
            featuredList.eo_type = "ul";
            featuredList.li = new ExpandoObject();

            var featuredQuery = (from e1 in _entityObjectsRepository.GetAll()
                                 join e2 in _entityObjectsRepository.GetAll() on e1.hisn1 equals e2.isn
                                 join e3 in _entityObjectsRepository.GetAll() on e2.hisn1 equals e3.isn
                                 where e1.eo_type == section.GetValue<short>("EO_TYPE~LOCATION")
                                 && e1.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                                 && e1.isn == id
                                 select new
                                 {
                                     city = e1.long_name,
                                     region = e2.name2,
                                     country = e3.name3
                                 })
                                 .FirstOrDefault();

            var location_name = featuredQuery.city?.ToLower();
            var location_link = $"{featuredQuery.city?.ToLower()}-{featuredQuery.region?.ToLower()}-{featuredQuery.country?.ToLower()}";

            var pageMenuData = (from e1 in _entityObjectsRepository.GetAll()
                                join e2 in _entityObjectsRepository.GetAll() on e1.isn equals e2.hisn1
                                where e1.name1 == "site_menu"
                                 && e2.hisn3 == id
                                orderby e2.priority
                                select new PageMenu
                                {
                                    isn = e2.isn,
                                    eo_name = e2.name1,
                                    eo_desc = "",
                                    eo_type = "ulr",
                                    text = e2.long_name,
                                    link = $"/location/{e2.name1}"
                                }).ToList();

            // TODO: implementation needs review
            var pageMenuData1 = new List<PageMenu>(); ;

            foreach (var key in pageMenuData)
            {
                var keyClone = new PageMenu
                {
                    isn = key.isn,
                    eo_name = key.eo_name,
                    eo_desc = key.eo_desc,
                    eo_type = key.eo_type,
                    text = key.text,
                    link = key.link

                };
                var lnpos = key.text.IndexOf("{$location_name}");
                if (lnpos > -1)
                {
                    keyClone.text = key.text.Replace("{$location_name}", location_name);
                }
                keyClone.eo_name = $"{key.eo_name} {location_name?.ToLower()}";
                keyClone.link = $"{defaultsSection.GetValue<string>("site_url_prefix")}{key.link}";

                var snsData = _entityObjectsRepository.GetAll()
                        .Where(x => x.hisn1 == key.isn)
                        .Where(x => x.eo_type == section.GetValue<short>("EO_TYPE~LIST_ITEM"))
                        .Where(x => x.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE"))
                       .Select((o) =>
                        new
                        {
                            text = o.long_name,
                            link = o.name1,
                        })
                        .ToList();
                foreach (var key1 in snsData)
                {
                    var shortMenu = new ShortMenu();
                    var lnpos1 = key1.text.IndexOf("{$location_name}");
                    if (lnpos1 > -1)
                    {
                        shortMenu.text = key1.text.Replace("{$location_name}", location_name);
                    }
                    shortMenu.link = $"{defaultsSection.GetValue<string>("site_url_prefix")}{key1.link}";
                    keyClone.li.Add(shortMenu);
                }

                pageMenuData1.Add(keyClone);

            }

            featuredList.li = pageMenuData1;

            return featuredList;
        }

        public dynamic GetCityPrograms(string version, long id)
        {
            var defaultsSection = _configuration.GetSection($"settings:v{version}:defaults");
            var section = _configuration.GetSection("proman_codes");

            dynamic cityPrograms = new ExpandoObject();
            cityPrograms.eo_name = "CITY PROGRAMS";
            cityPrograms.eo_desc = "";
            cityPrograms.eo_type = "ul";
            cityPrograms.li = new ExpandoObject();

            var query = from e1 in _entityObjectsRepository.GetAll()
                        join e2 in _entityObjectsRepository.GetAll() on e1.hisn1 equals e2.isn
                        join e3 in _entityObjectsRepository.GetAll() on e2.hisn1 equals e3.isn
                        where e1.eo_type == section.GetValue<short>("EO_TYPE~LOCATION")
                        && e1.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                        && e1.isn == id
                        select new { city = e1.long_name, region = e2.name2, country = e3.name3 };
            var result = query.Take(4).ToList();
            var location_name = result.FirstOrDefault().city;
            var location_link = $"{result.FirstOrDefault().city.ToLower()}-{result.FirstOrDefault().region.ToLower()}-{result.FirstOrDefault().country.ToLower()}";

            var programISN = (from e in _entityObjectsRepository.GetAll()
                              where e.eo_type == section.GetValue<short>("EO_TYPE~LIST")
                               && e.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                               && e.name1 == "middle_box" && e.hisn3 == id
                              select e.isn).FirstOrDefault();
            if (programISN == 0)
                id = 0;     // if there is no city program for this city then take generic city program (middle_box)

            var cityProgramsData = (from e1 in _entityObjectsRepository.GetAll()
                                    join e2 in _entityObjectsRepository.GetAll() on e1.isn equals e2.hisn1
                                    where e2.eo_type == section.GetValue<short>("EO_TYPE~LIST_ITEM") && e2.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                                    join e3 in _entityObjectsRepository.GetAll() on e2.isn equals e3.hisn1
                                    where e1.name1 == "middle_box"
                                    && e3.eo_type == section.GetValue<short>("EO_TYPE~LIST") && e3.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                                    && e2.hisn3 == id
                                    orderby e2.isn descending
                                    select new CityPrograms
                                    {
                                        isn = e2.isn,
                                        eo_name = e2.long_name,
                                        eo_desc = "",
                                        eo_type = "ul",
                                        link = $"{defaultsSection.GetValue<string>("site_url_prefix")}/location/{e2.name1}",
                                        item_image = $"{defaultsSection.GetValue<string>("site_url_prefix")}/{Utils.ProcessXML(e3.long_comment, "img")}"

                                    }).Take(4).ToList();
            // TODO:             
            var cityProgramsDataISN = cityProgramsData.Select(x => x.isn).Distinct().ToList();
            var innerData = (from e1 in _entityObjectsRepository.GetAll()
                             join e2 in _entityObjectsRepository.GetAll() on e1.isn equals e2.hisn1
                             where cityProgramsDataISN.Contains(e1.hisn1)
                             && e2.hisn3 == id
                             orderby e2.isn descending
                             select new
                             {
                                 e1.hisn1,
                                 text = e2.long_name,
                                 link = e2.name1
                             }
                             ).ToList();

            foreach (var key1 in cityProgramsData)
            {

                var isnObject = innerData.Where(x => x.hisn1 == key1.isn).ToList();
                foreach (var key2 in isnObject)
                {
                    key1.li.Add(new ShortMenu
                    {
                        text = key2.text,
                        link = $"{defaultsSection.GetValue<string>("site_url_prefix")}/location/{ location_link}/{ key2.link}"
                    });
                }

            }

            cityPrograms.li = cityProgramsData.Select((o) => new { o.eo_name, o.eo_desc, o.eo_type, o.link, o.item_image, o.li });
            return cityPrograms;
        }

        public dynamic GetAttributesList(string version)
        {
            var section = _configuration.GetSection("proman_codes");
            return _entityObjectsRepository.GetAll()
                .Where(x => x.eo_type == section.GetValue<short>("EO_TYPE~ATTRIBUTE"))
                .Where(x => x.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE"))
                .Select((o) => new
                {
                    id = o.isn,
                    name = o.name1
                }).ToList();

        }

        public dynamic GetPageBlocks(string version, long id, string type, string block_name)
        {
            var section = _configuration.GetSection("proman_codes");
            var result = new List<KeyValuePair<string, dynamic>>();
            var dataResult = (from e in _entityObjectsRepository.GetAll()
                              where e.eo_type == section.GetValue<short>("EO_TYPE~PAGE_BLOCK")
                              && e.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                              && e.name1 == block_name && e.hisn1 == id && e.status == 1
                              select new { id = e.isn, name = e.name1, parent_id = e.hisn1, e.long_comment })
                             .ToList();
            if (dataResult.Count() > 0)
            {
                if (block_name.Equals("location_news", StringComparison.CurrentCultureIgnoreCase))
                {
                    for (int i = 0; i < dataResult.Count; i++)
                    {
                        //if (dataResult[i]["desc1"] != "")
                        //{
                        //$news_info = explode('~', $r[$i]['desc1']);
                        //$r[$i]['type'] = $news_info[0];
                        //$r[$i]['date'] = (count($news_info) > 1 ? $news_info[1] : null );
                        //$r[$i]['author'] = (count($news_info) > 1 ? $news_info[2] : null );                            
                        //}
                    }
                }

                foreach (var key in dataResult)
                {
                    result.Add(new KeyValuePair<string, dynamic>("id", key.id));
                    result.Add(new KeyValuePair<string, dynamic>("name", key.name));
                    result.Add(new KeyValuePair<string, dynamic>("parent_id", key.parent_id));
                    var comments = XDocument.Parse(key.long_comment).Root.Attributes().ToDictionary(sp => sp.Name, sp => sp.Value);
                    foreach (var c in comments)
                    {
                        result.Add(new KeyValuePair<string, dynamic>(c.Key.LocalName, c.Value));
                    }
                }
            }

            return result.ToDictionary(p => p.Key, p => p.Value);
        }

        public dynamic GetProductList(string version, int ownerId)
        {
            var productsList = new List<dynamic>();
            var defaultsSection = _configuration.GetSection($"settings:v{version}:defaults");
            var section = _configuration.GetSection("proman_codes");
            EntityObject entityObjec = new EntityObject();

            short relation_type = section.GetValue<short>("RELATION~LOCATION_CATEGORY");
            short relation_subtype = section.GetValue<short>("EO_TYPE~INSTANCE");
            short eo_type = section.GetValue<short>("EO_TYPE~CLASSIFICATION");
            short or_eo_type = section.GetValue<short>("EO_TYPE~CLASSIFICATIONTYPE");
            short eo_subtype = section.GetValue<short>("EO_TYPE~INSTANCE");
            short status = section.GetValue<short>("STATUS~ACTIVE");

            if (ownerId > 0)
            {
                entityObjec = _entityObjectsRepository.GetById(ownerId);
            }
            var products = from eo in _entityObjectsRepository.GetAll()
                           where eo.eo_type == section.GetValue<short>("EO_TYPE~PRODUCT")
                           && eo.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                           && eo.status == defaultsSection.GetValue<short>("status")
                           select eo;

            if (entityObjec.isn > 0)
                products = products.Where(x => x.hisn3 == ownerId);

            var results = products.Select((o) => new
            {
                id = o.isn,
                product_id = o.name1,
                name = o.name2,
                serial_number = o.name3,
                lc_data = o.long_comment
            }).ToList();

            var entityObjectIds = results.Select(x => Convert.ToInt64(x.id)).ToList();

            var prices = (from t in _trackingRepository.GetAll()
                          where entityObjectIds.Contains(t.EntityId)
                          && t.EOType == Common.Enums.eEOTypes.PRODUCT
                          && t.EOSubtype ==Common.Enums.eEOSubTypes.INSTANCE
                          && t.Status == Common.Enums.eRecordStatus.Active
                          select new
                          {
                              t.EntityId,
                              price = t.StatusChangedBy,
                              //price2 = t.act2,
                              //price3 = t.act3

                          }).ToList();

            results.ForEach((p) =>
            {
                var newProduct = new Dictionary<string, dynamic>
                {
                    { "id", p.id },
                    { "product_id", p.product_id },
                    { "name", p.name },
                    { "serial_number", p.serial_number }
                };

                if (!string.IsNullOrEmpty(p.lc_data))
                {
                    var comments = XDocument.Parse(p.lc_data).Root.Attributes().ToDictionary(sp => sp.Name, sp => sp.Value);
                    foreach (var c in comments)
                    {
                        newProduct.Add(c.Key.LocalName, c.Value);
                    }
                }
                var priceObject = prices.Where(x => x.EntityId == p.id).Select(x => new { x.price}).ToList();

                newProduct.Add("price", priceObject);
                productsList.Add(newProduct);
            });

            return productsList;
        }

        public dynamic GetReviewCategories(string version, long id)
        {
            var section = _configuration.GetSection("proman_codes");

            var query = from e1 in _entityObjectsRepository.GetAll()
                        join e2 in _entityObjectsRepository.GetAll() on e1.hisn1 equals e2.isn
                        where e2.eo_type == section.GetValue<short>("EO_TYPE~REVIEW_CATEGORY_UL")
                        && e2.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                        && e2.hisn1 == id
                        && e1.eo_type == section.GetValue<short>("EO_TYPE~REVIEW_CATEGORY_LI")
                        && e1.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                        select new
                        {
                            id = e1.isn,
                            name = e1.name1
                        };

            return query.ToList();
        }

        public dynamic GetOverview(string version, long id)
        {
            var section = _configuration.GetSection("proman_codes");

            var query = from e1 in _entityObjectsRepository.GetAll()
                        join e2 in _entityObjectsRepository.GetAll() on e1.hisn1 equals e2.isn
                        where e2.eo_type == section.GetValue<short>("EO_TYPE~REVIEW_CATEGORY_UL")
                        && e2.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                         && e2.hisn1 == id
                        && e1.eo_type == section.GetValue<short>("EO_TYPE~REVIEW_CATEGORY_LI")
                        && e1.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                        select new
                        {
                            id = e1.isn,
                            name = e1.name1
                        };

            return query.ToList();
        }

        public dynamic GetLocation(string version, string lat, string lng, string result)
        {
            var locations = new Dictionary<string, dynamic>();
            var section = _configuration.GetSection("proman_codes");
            WebClient client = new WebClient();
            var url = $"https://maps.googleapis.com/maps/api/geocode/json?key=AIzaSyBZlsM_AHkUK5Q8nPmF7q4j-B7slLLi1FQ&latlng={lat},{lng}&sensor=true";
            var resp = client.DownloadString(url);

            string city = "unknown";
            dynamic res = JsonConvert.DeserializeObject<dynamic>(resp);
            if (result != "full" && res.results[0]["formatted_address"] != "")
            {
                locations.Add("address", res.results[0]["formatted_address"]);

                for (var i = 0; i < res.results[0]["address_components"].Count; i++)
                {
                    if (res.results[0].address_components[i]["types"][0] == "locality")
                        city = res.results[0].address_components[i].long_name;
                }
                var details = (from e1 in _entityObjectsRepository.GetAll()
                               where e1.eo_type == section.GetValue<short>("EO_TYPE~LOCATION") && e1.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                               join e2 in _entityObjectsRepository.GetAll() on e1.isn equals e2.hisn1
                               where e2.eo_type == section.GetValue<short>("EO_TYPE~LOCATION") && e2.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                               join e3 in _entityObjectsRepository.GetAll() on e2.isn equals e3.hisn1
                               where e3.eo_type == section.GetValue<short>("EO_TYPE~LOCATION") && e3.eo_subtype == section.GetValue<short>("EO_TYPE~INSTANCE")
                               && e3.long_name == city
                               select new
                               {
                                   country_id = e1.isn,
                                   country_name = e1.long_name,
                                   region_id = e2.isn,
                                   region_name = e2.long_name,
                                   city_name = e3.long_name,
                                   city

                               }).ToList();
                locations.Add("details", details);
            }

            return locations;
        }

        public dynamic GetEvents(string version, long location_id)
        {
            WebClient client = new WebClient();
            var url = "http://api.eventful.com/json/events/search?app_key=sJXpnB7ftrgc5tTg&location=Toronto&date=Future";
            var resp = client.DownloadString(url);
            return JsonConvert.DeserializeObject<dynamic>(resp);
        }

        public dynamic GetEventsPHQ(string version, string offset, string lat, string lng, string within)
        {
            WebClient client = new WebClient();
            client.Headers.Add("Authorization", "Bearer 38rKzFLE7XKaOFhLpAsoi879lNC0n4 ");
            var url = $"https://api.predicthq.com/v1/events/?offset={offset}&within={within}@{lat},{lng}";
            var resp = client.DownloadString(url);
            return JsonConvert.DeserializeObject<dynamic>(resp);
        }

        public dynamic GetVideoTemplatesList(string version)
        {
            return new Dictionary<string, dynamic>
            {
                { "id", 0 },
                { "video_preview_url", "" },
                { "video_template_url", "" },
                { "name", "" },
                { "moments_count", "" },
                { "video_length_in_seconds", 0 }
            };

        }

        public dynamic GetRecentUsedVideoTemplatesList(string version)
        {
            return new Dictionary<string, dynamic>
            {
                { "id", 0 },
                { "video_preview_url", "" },
                { "video_template_url", "" },
                { "name", "" },
                { "moments_count", "" },
                { "video_length_in_seconds", 0 }
            };

        }

        public dynamic GetMyVideosList(string version)
        {
            return new Dictionary<string, dynamic>
            {
                { "video_id", 0 },
                { "video_preview_url", "" },
                { "video_template_url", "" },
                { "name", "" },
                { "moments_count", "" },
                { "video_length_in_seconds", 0 }
            };

        }

        public dynamic GetVideoTemplateMomentsList(string version)
        {
            return new Dictionary<string, dynamic>
            {
                { "time_in_seconds_from_start_video_of_moment", 0 },
                { "category_id_of_moment", "" },
                { "moment_id", "" },
                { "moment_description", "" }
            };

        }

        public void SaveEntity(EntityViewModel viewModel)
        {
            if (viewModel.Id > 0)
                _bus.SendCommand(_mapper.Map<UpdateEntityCommand>(viewModel));
            else
                _bus.SendCommand(_mapper.Map<NewEntityCommand>(viewModel));
        }

        
        private long IdentifyCatgory(ref string url, IConfigurationSection section)
        {
            long id = 0;
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
            return (from eo in _entityObjectsRepository.GetAll()
                    where eo.name1 == category_name && eo.eo_type == section.GetValue<short>("EO_TYPE~CLASSIFICATION")
                    && eo.eo_subtype == section.GetValue<short>("EO_TYPE~CLASSIFICATIONTYPE")
                    && eo.status == section.GetValue<short>("STATUS~ACTIVE")
                    orderby eo.isn descending
                    select eo.isn).FirstOrDefault();

        }

        private long IdentifyLocations(string url, ref int hisn1, IConfigurationSection section)
        {
            List<string> locations = GetLocationsFromUrl(url);

            var hisn5Arr = new List<int>() {
                        section.GetValue<int>("ISN~LOCATIONTYPE~COUNTRY"),
                        section.GetValue<int>("ISN~LOCATIONTYPE~REGION"),
                        section.GetValue<int>("ISN~LOCATIONTYPE~CITY"),
                        section.GetValue<int>("ISN~LOCATIONTYPE~NEIGHBORHOOD")
                };

            var o = new List<dynamic>();
            for (var i = 0; i < locations.Count; i++)
            {
                var name = locations.ElementAt(i);
                if (string.IsNullOrEmpty(name) && name == "c")
                    continue;

                var query = from eo in _entityObjectsRepository.GetAll()
                            where eo.name1 == name.Trim() && eo.hisn5 == hisn5Arr[i]
                            select eo;
                if (hisn1 > 0)
                {
                    var tempHisn1 = hisn1;
                    query = query.Where(x => x.hisn1 == tempHisn1);
                }

                var identify_locations = query.Where(x => x.eo_type == section.GetValue<int>("EO_TYPE~LOCATION"))
                          .Where(x => x.eo_subtype == section.GetValue<int>("EO_TYPE~INSTANCE"))
                          .Where(x => x.status == section.GetValue<int>("STATUS~ACTIVE"))
                          .Select((e) => new
                          {
                              id = e.isn,
                              name = e.long_name
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

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}