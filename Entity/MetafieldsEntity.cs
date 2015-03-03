using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyncDataTool.Entity
{
    public class MetafieldsEntity
    {
        public List<MetafieldEntity> metafields { get; set; }
    }

    public class MetafieldUpdateResultEntity
    {
        public MetafieldEntity metafield { get; set; }
    }

    public class MetafieldEntity
    {
        public object created_at { get; set; }
        public object description { get; set; }
        public object id { get; set; }
        public object key { get; set; }
        public object @namespace { get; set; }
        public object owner_id { get; set; }
        public object updated_at { get; set; }
        public object value { get; set; }
        public object value_type { get; set; }
        public object owner_resource { get; set; }
    }
}