using System;
using Enferno.Public.Web.N2.Items.Editors.Attributes;
using Enferno.Public.Web.N2.Items.Editors.Data;
using N2;
using N2.Integrity;

namespace Enferno.Public.Web.N2.Items.Parts
{
    [PartDefinition("Product spot", IconClass = "n2-icon-th", SortOrder = 3)]
    [AllowedZones(AllowedZones.AllNamed)]
    [Serializable]
    public class ProductSpotItem : BasePart
    {
        [EditableProduct(Title = "Product")]
        public ProductEditData Data
        {
            get { return (ProductEditData) GetDetail("Data") ?? new ProductEditData(); }
            set { SetDetail("Data", value); }
        }
    }
}