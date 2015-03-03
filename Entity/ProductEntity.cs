using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SyncDataTool.Entity
{
    [Serializable]
    public class ProductEntity : ICloneable
    {
        public ProductContentEntity product { get; set; }

        /// <summary>
        /// 深度克隆
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {

            BinaryFormatter Formatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));

            MemoryStream stream = new MemoryStream();

            Formatter.Serialize(stream, this);

            stream.Position = 0;

            object clonedObj = Formatter.Deserialize(stream);

            stream.Close();

            return clonedObj;

        }
    }

    [Serializable]
    public class ProductContentEntity
    {
        public object body_html { get; set; }
        public object created_at { get; set; }
        public object handle { get; set; }
        public object id { get; set; }
        public object product_type { get; set; }
        public object published_at { get; set; }
        public object published_scope { get; set; }
        public object template_suffix { get; set; }
        public object title { get; set; }
        public object updated_at { get; set; }
        public object vendor { get; set; }
        public object tags { get; set; }
        public List<variantEntity> variants { get; set; }
        public List<optionEntity> options { get; set; }
        public List<imageEntity> images { get; set; }
        public imageEntity image { get; set; }
    }

    [Serializable]
    public class variantEntity
    {
        public object barcode { get; set; }
        public object compare_at_price { get; set; }
        public object created_at { get; set; }
        public object fulfillment_service { get; set; }
        public object grams { get; set; }
        public object id { get; set; }
        public object inventory_management { get; set; }
        public object inventory_policy { get; set; }
        public object option1 { get; set; }
        public object option2 { get; set; }
        public object option3 { get; set; }
        public object position { get; set; }
        public object price { get; set; }
        public object product_id { get; set; }
        public object requires_shipping { get; set; }
        public object sku { get; set; }
        public object taxable { get; set; }
        public object title { get; set; }
        public object updated_at { get; set; }
        public object inventory_quantity { get; set; }
        public object old_inventory_quantity { get; set; }
    }

    [Serializable]
    public class optionEntity
    {
        public object id { get; set; }
        public object name { get; set; }
        public object position { get; set; }
        public object product_id { get; set; }
    }

    [Serializable]
    public class imageEntity
    {
        public object created_at { get; set; }
        public object id { get; set; }
        public object position { get; set; }
        public object product_id { get; set; }
        public object updated_at { get; set; }
        public object src { get; set; }
    }
}
