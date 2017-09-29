using System;
using Enferno.Public.InversionOfControl;
using Enferno.Public.Web.Repositories;
using Enferno.StormApiClient.Products;
using Microsoft.Practices.Unity;
using N2;
using N2.Definitions;
using N2.Details;
using N2.Web;

namespace Enferno.Public.Web.N2.Items.Pages
{
    [PageDefinition(Name = "Manufacturerpage (Enferno)", Description = "This ManufacturerPage handles virtual manufacturers pages by extracting the id or name from the url.")]
    [Serializable]
    public class ManufacturerPageItem : BasePage, IContentPage, IStructuralPage
    {
        /// <summary>
        /// Main content of this content item.
        /// </summary>
        [EditableFreeTextArea(Title = "Text", SortOrder = 50)]
        [DisplayableTokens]
        public virtual string Text { get; set; }

        public override string TemplateUrl
        {
            get
            {
                return "~//ManufacturerPage/Index";
            }
        }

        private string argument;

        private Manufacturer currentManufacturer;
        public Manufacturer CurrentManufacturer {
            get { return currentManufacturer ?? GetCurrentManufacturer(); }
            set { currentManufacturer = value; }
        }

        public override PathData FindPath(string remainingUrl)
        {
            var basePath = base.FindPath(remainingUrl);
            argument = basePath.Argument;
            var pathData = basePath.CurrentItem != null ? basePath : base.FindPath(null);
            pathData.IsCacheable = false;
            return pathData;
        }      

        private Manufacturer GetCurrentManufacturer()
        {
            int? id = null;
            int i;
            if (int.TryParse(argument, out i)) id = i;
            var name = argument;

            var repo = IoC.Container.Resolve<IRepository>();
            currentManufacturer = id.HasValue ? repo.Products.GetManufacturer(id.Value) : repo.Products.GetManufacturer(name);
            return currentManufacturer;
        }
    }
}
