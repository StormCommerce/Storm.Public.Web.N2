using System;

namespace Enferno.Public.Web.N2.Items.Editors.Data
{
    [Serializable]
    public class ProductEditData
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string ImageKey { get; set; }
    }
}