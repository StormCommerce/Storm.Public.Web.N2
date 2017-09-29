
using System;
using System.Collections.Generic;
using Enferno.Public.Web.Models;
using Enferno.Public.Web.N2.Items.Editors.Attributes;
using N2;
using N2.Definitions;
using N2.Details;

namespace Enferno.Public.Web.N2.Items.Pages
{
    [PageDefinition(Name = "Product List Page (Enferno)", Description = "This ProductPage handles virtual product pages by extracting the id or name from the url.")]
    [Serializable]
    public class ProductListPageItem : BasePage, IStructuralPage
    {
        [EditableText(Title = "Header", SortOrder = 10)]
        public virtual string Header { get; set; }

        [EditableFreeTextArea(Title = "Text", SortOrder = 20)]
        public virtual string Text { get; set; }

        [EditableCategoryId(Title = "Category id", SortOrder = 30)]
        public virtual int CategoryId { get; set; }

        [EditableText(Title = "Category Ids", SortOrder = 32)]
        public virtual string CategoryIdSeed { get; set; }

        [EditableNumber(Title = "Parameter id", SortOrder = 40)]
        public virtual int? ParameterId { get; set; }

        [EditableNumber(Title = "Parameter value id", SortOrder = 50)]
        public virtual int? ParameterValueId { get; set; } 

        public ProductListModel ProductList { get; set; }

        public List<FilterModel> Filters { get; set; }
    }
}
