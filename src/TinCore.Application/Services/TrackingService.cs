using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Xml.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Extensions.Configuration;
using TinCore.Application.Interfaces;
using TinCore.Application.ViewModels;
using TinCore.Domain.Commands;
using TinCore.Domain.Core.Bus;
using TinCore.Domain.Interfaces;
using TinCore.Domain.Models;

namespace TinCore.Application.Services
{
    public class TrackingService : ITrackingService
    {
        private readonly IMapper _mapper;
        private readonly ITrackingRepository _trackingRepository;
        private readonly IEntityObjectsRepository _entityObjectRepository;
        private readonly IConfiguration _configuration;
        private readonly IMediatorHandler _bus;

        public TrackingService(IMapper mapper,
                                  ITrackingRepository trackingRepository,
                                  IEntityObjectsRepository entityObjectRepository,
                                  IConfiguration configuration,
                                  IMediatorHandler bus)
        {
            _mapper = mapper;
            _trackingRepository = trackingRepository;
            _entityObjectRepository = entityObjectRepository;
            _configuration = configuration;
            _bus = bus;
        }

        public dynamic GetListOfEvents(string version, long id)
        {
            var section = _configuration.GetSection("proman_codes");
            long tracking_type = section.GetValue<long>("TRACKING~EVENT");
            short tracking_subtype = section.GetValue<short>("EO_TYPE~INSTANCE");
            short tracking_rowtype = section.GetValue<short>("TRACKING_ROW_TYPE~ACTUAL");
            short eo_type = section.GetValue<short>("EO_TYPE~PERSON");
            short eo_subtype = section.GetValue<short>("EO_TYPE~INSTANCE");

            dynamic listOfEvents = new ExpandoObject();
            listOfEvents.upcoming_events = new ExpandoObject();
            listOfEvents.upcoming_events.eo_name = "UPCOMING EVENTS";
            listOfEvents.upcoming_events.eo_desc = "";
            listOfEvents.upcoming_events.eo_type = "ul";

            listOfEvents.upcoming_events.li = _trackingRepository.GetAll()
                .Where((x) => x.tracking_type == tracking_type && x.tracking_subtype == tracking_subtype)
                .Where(x => x.tracking_rowtype == tracking_rowtype && x.hisn3 == id)
                .Where(x => x.eo_type == eo_type && x.eo_subtype == eo_subtype)
                .Where(x => x.end_datetime > DateTime.Now)
                .Select((o) => new
                {
                    id = o.isn,
                    start_datetime = o.start_datetime,
                    end_datetime = o.end_datetime,
                    descr_title = ProcessXML(o.long_comment, "Short_Description"),
                    descr_subtitle = ProcessXML(o.long_comment, "Long_Description"),
                    ticket_url = ProcessXML(o.long_comment, "ticket_url")
                })
                .OrderBy(x => x.id)
                .ToList();

            return listOfEvents;
        }

        public IEnumerable<dynamic> GetEventTickets(string version, long event_id)
        {
            var section = _configuration.GetSection("proman_codes");
            long tracking_type = section.GetValue<long>("TRACKING~TICKET");
            short tracking_subtype = section.GetValue<short>("EO_TYPE~INSTANCE");
            short tracking_rowtype = section.GetValue<short>("TRACKING_ROW_TYPE~ACTUAL");
            short eo_type = section.GetValue<short>("EO_TYPE~PERSON");
            short eo_subtype = section.GetValue<short>("EO_TYPE~INSTANCE");

            return _trackingRepository.GetAll().Where((x) => x.tracking_type == tracking_type && x.tracking_subtype == tracking_subtype)
                .Where(x => x.tracking_rowtype == tracking_rowtype)
                .Where(x => x.hisn1 == event_id)
                .Select((o) => new
                {
                    id = o.isn,
                    entity_id = o.eo_isn,
                    start_datetime = o.start_datetime,
                    end_datetime = o.end_datetime,
                    descr_title = ProcessXML(o.long_comment, "desc1"),
                    descr_subtitle = ProcessXML(o.long_comment, "desc2"),
                    image_url = ProcessXML(o.long_comment, "image_url"),
                    ticket_url = ProcessXML(o.long_comment, "ticket_url"),
                    price = o.actual,
                    status = o.status,
                    long_comment = o.long_comment,
                    desc1 = ProcessXML(o.long_comment, "desc1"),
                    desc2 = ProcessXML(o.long_comment, "desc2"),
                    desc3 = ProcessXML(o.long_comment, "desc3")
                })
                .OrderBy(x => x.id)
                .ToList();

        }

        public dynamic GetLocationEvents(string version, long id, long location_id)
        {
            var section = _configuration.GetSection("proman_codes");
            long tracking_type = section.GetValue<long>("TRACKING~EVENT");
            short tracking_subtype = section.GetValue<short>("EO_TYPE~INSTANCE");
            short tracking_rowtype = section.GetValue<short>("TRACKING_ROW_TYPE~ACTUAL");
            short eo_type = section.GetValue<short>("EO_TYPE~PERSON");
            short eo_subtype = section.GetValue<short>("EO_TYPE~INSTANCE");

            var entityObject = _entityObjectRepository
                .GetAll()
                .Where(x => x.isn == id)
                .Select((o) => new
                {
                    long_comment = o.long_comment
                }).FirstOrDefault();

            var eventIds = new List<int>();
            var feventIds = new List<int>();
            if (!string.IsNullOrEmpty(entityObject.long_comment))
            {
                var events = ProcessXML(entityObject.long_comment, "events");
                if (!string.IsNullOrEmpty(events))
                {
                    eventIds = events.Split(",").Select(x => Convert.ToInt32(x)).ToList();
                }

                var fevents = ProcessXML(entityObject.long_comment, "fevent");
                if (!string.IsNullOrEmpty(fevents))
                {
                    feventIds = events.Split(",").Select(x => Convert.ToInt32(x)).ToList();
                }
            }

            dynamic listOfEvents = new ExpandoObject();
            listOfEvents.upcoming_events = new ExpandoObject();
            listOfEvents.upcoming_events.eo_name = "UPCOMING EVENTS";
            listOfEvents.upcoming_events.eo_desc = "";
            listOfEvents.upcoming_events.eo_type = "ul";

            listOfEvents.upcoming_events.li = _trackingRepository.GetAll()
                .Where((x) => x.tracking_type == tracking_type && x.tracking_subtype == tracking_subtype)
                .Where(x => x.tracking_rowtype == tracking_rowtype)
                .Where(x => x.eo_type == eo_type && x.eo_subtype == eo_subtype)
                .Where(x => eventIds.Contains(x.isn))
                .Where(x => x.end_datetime > DateTime.Now)
                .Select((o) => new
                {
                    id = o.isn,
                    entity_id = o.eo_isn,
                    start_datetime = o.start_datetime,
                    end_datetime = o.end_datetime,
                    descr_title = ProcessXML(o.long_comment, "desc1"),
                    descr_subtitle = ProcessXML(o.long_comment, "desc2"),
                    image_url = ProcessXML(o.long_comment, "image_url"),
                    ticket_url = ProcessXML(o.long_comment, "ticket_url")
                })
                .OrderBy(x => x.id)
                .ToList();

            listOfEvents.upcoming_events.featured = _trackingRepository.GetAll()
                .Where((x) => x.tracking_type == tracking_type && x.tracking_subtype == tracking_subtype)
                .Where(x => x.tracking_rowtype == tracking_rowtype && feventIds.Contains(x.isn))
                .Where(x => x.eo_type == eo_type && x.eo_subtype == eo_subtype)
                .Where(x => x.end_datetime > DateTime.Now)
                .Select((o) => new
                {
                    id = o.isn,
                    entity_id = o.eo_isn,
                    start_datetime = o.start_datetime,
                    end_datetime = o.end_datetime,
                    descr_title = ProcessXML(o.long_comment, "desc1"),
                    descr_subtitle = ProcessXML(o.long_comment, "desc2"),
                    image_url = ProcessXML(o.long_comment, "image_url"),
                    ticket_url = ProcessXML(o.long_comment, "ticket_url")
                })
                .OrderBy(x => x.id)
                .ToList();

            return listOfEvents;
        }

        public dynamic GetEvents(string version, long id)
        {
            var section = _configuration.GetSection("proman_codes");
            long tracking_type = section.GetValue<long>("TRACKING~EVENT");
            short tracking_subtype = section.GetValue<short>("EO_TYPE~INSTANCE");
            short tracking_rowtype = section.GetValue<short>("TRACKING_ROW_TYPE~ACTUAL");
            short eo_type = section.GetValue<short>("EO_TYPE~PERSON");
            short eo_subtype = section.GetValue<short>("EO_TYPE~INSTANCE");

            dynamic listOfEvents = new ExpandoObject();
            listOfEvents.upcoming_events = new ExpandoObject();
            listOfEvents.upcoming_events.eo_name = "UPCOMING EVENTS";
            listOfEvents.upcoming_events.eo_desc = "";
            listOfEvents.upcoming_events.eo_type = "ul";

            listOfEvents.upcoming_events.li = _trackingRepository.GetAll()
                .Where((x) => x.tracking_type == tracking_type && x.tracking_subtype == tracking_subtype)
                .Where(x => x.tracking_rowtype == tracking_rowtype)
                .Where(x => x.eo_type == eo_type && x.eo_subtype == eo_subtype)
                .Where(x => x.eo_isn == id)
                .Where(x => x.end_datetime > DateTime.Now)
                .Select((o) => new
                {
                    id = o.isn,
                    start_datetime = o.start_datetime,
                    end_datetime = o.end_datetime,
                    descr_title = ProcessXML(o.long_comment, "Short_Description"),
                    descr_subtitle = ProcessXML(o.long_comment, "Long_Description"),
                    ticket_url = ProcessXML(o.long_comment, "ticket_url")
                })
                .OrderBy(x => x.id)
                .ToList();

            return listOfEvents;
        }

        public dynamic GetListOfCoupons(string version, long location_id)
        {
            var section = _configuration.GetSection("proman_codes");
            long tracking_type = section.GetValue<long>("TRACKING~COUPON");
            short tracking_subtype = section.GetValue<short>("EO_TYPE~INSTANCE");
            short tracking_rowtype = section.GetValue<short>("TRACKING_ROW_TYPE~ACTUAL");
            short eo_type = section.GetValue<short>("EO_TYPE~PERSON");
            short eo_subtype = section.GetValue<short>("EO_TYPE~INSTANCE");

            dynamic listOfEvents = new ExpandoObject();
            listOfEvents.coupons = new ExpandoObject();
            listOfEvents.coupons.eo_name = "COUPONS";
            listOfEvents.coupons.eo_desc = "";
            listOfEvents.coupons.eo_type = "ul";

            listOfEvents.coupons.li = _trackingRepository.GetAll()
                .Where(x => x.hisn3 == location_id)
                .Where((x) => x.tracking_type == tracking_type && x.tracking_subtype == tracking_subtype)
                .Where(x => x.tracking_rowtype == tracking_rowtype)
                .Where(x => x.eo_type == eo_type && x.eo_subtype == eo_subtype)
                .Where(x => x.end_datetime > DateTime.Now)
                .Select((o) => new
                {
                    id = o.isn,
                    belongs_to = o.eo_isn,
                    start_datetime = o.start_datetime,
                    end_datetime = o.end_datetime,
                    descr_title = ProcessXML(o.long_comment, "title"),
                    descr_subtitle = ProcessXML(o.long_comment, "subtitle"),
                    description = ProcessXML(o.long_comment, "description"),
                    image_url = ProcessXML(o.long_comment, "image1")
                })
                .OrderBy(x => x.id)
                .ToList();

            return listOfEvents;
        }

        public dynamic GetEntityCoupons(string version, long id)
        {
            var section = _configuration.GetSection("proman_codes");
            long tracking_type = section.GetValue<long>("TRACKING~COUPON");
            short tracking_subtype = section.GetValue<short>("EO_TYPE~INSTANCE");
            short tracking_rowtype = section.GetValue<short>("TRACKING_ROW_TYPE~ACTUAL");
            short eo_type = section.GetValue<short>("EO_TYPE~PERSON");
            short eo_subtype = section.GetValue<short>("EO_TYPE~INSTANCE");

            dynamic listOfEvents = new ExpandoObject();
            listOfEvents.coupons = new ExpandoObject();
            listOfEvents.coupons.eo_name = "COUPONS";
            listOfEvents.coupons.eo_desc = "";
            listOfEvents.coupons.eo_type = "ul";

            listOfEvents.coupons.li = _trackingRepository.GetAll()
                .Where(x => x.eo_isn == id)
                .Where((x) => x.tracking_type == tracking_type && x.tracking_subtype == tracking_subtype)
                .Where(x => x.tracking_rowtype == tracking_rowtype)
                .Where(x => x.eo_type == eo_type && x.eo_subtype == eo_subtype)
                .Where(x => x.end_datetime > DateTime.Now)
                .Select((o) => new
                {
                    id = o.isn,
                    belongs_to = o.eo_isn,
                    start_datetime = o.start_datetime,
                    end_datetime = o.end_datetime,
                    descr_title = ProcessXML(o.long_comment, "title"),
                    descr_subtitle = ProcessXML(o.long_comment, "subtitle"),
                    description = ProcessXML(o.long_comment, "description"),
                    image_url = ProcessXML(o.long_comment, "image1")
                })
                .OrderBy(x => x.id)
                .ToList();

            return listOfEvents;
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

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
