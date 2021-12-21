using System;
using System.Collections.Generic;
using System.Text;
using TinCore.Common.Enums;

namespace TinCore.Application.ViewModels
{
    public class RelationViewModel : BaseViewModel
    {
        public long Entity1Id { get; set; }
        public long Entity2_id { get; set; }
        public short Relation_subtype { get; set; }
        public string Entity1Name { get; set; }
        public string Entity2_name { get; set; }
        public string Comment { get; set; }
    }
}
