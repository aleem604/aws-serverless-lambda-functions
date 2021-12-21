using System;
using TinCore.Common.Enums;
using TinCore.Domain.Core.Commands;

namespace TinCore.Domain.Commands
{
    public abstract class LocationCommand : Command
    {
        public long Id { get; set; }
        public long ParentId { get; set; }
        public eLocationType LocationType { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string Code { get; set; }
        public string Desc1 { get; set; }
        public string Desc2 { get; set; }
        public string Desc3 { get; set; }
        public string Image_url { get; set; }
        public string FeaturedIds { get; set; }
        public string FeaturedEntities { get; set; }
        public string Admin { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Neighbourhood { get; set; }
        public string Region { get; set; }
        public string Site_Owner { get; set; }
        public string DonotmissEntities { get; set; }
    }
}