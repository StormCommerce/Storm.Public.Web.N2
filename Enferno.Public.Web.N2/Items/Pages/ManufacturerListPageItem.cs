using System;
using Enferno.StormApiClient.Products;
using N2;
using N2.Definitions;
using N2.Details;

namespace Enferno.Public.Web.N2.Items.Pages
{
    [PageDefinition(Name = "Manufacturer List Page (Enferno)", Description = "This manufacturer list page lists manufacturers.")]
    [Serializable]
    public class ManufacturerListPageItem : BasePage, IStructuralPage
    {
        [EditableText]
        public virtual string Header { get; set; }
        [EditableFreeTextArea]
        public virtual string Text { get; set; }

        public ManufacturerItemList ManufacturerList { get; set; }
    }
}
