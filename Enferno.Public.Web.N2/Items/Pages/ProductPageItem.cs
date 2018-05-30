
using System;
using Enferno.Public.InversionOfControl;
using Enferno.Public.Web.Repositories;
using Enferno.StormApiClient.Products;
using Unity;
using N2;
using N2.Definitions;
using N2.Details;
using N2.Web;

namespace Enferno.Public.Web.N2.Items.Pages
{
    [PageDefinition(Name = "Productpage (Enferno)", Description = "This ProductPage handles virtual product pages by extracting the id or name from the url.")]
    [Serializable]
    public class ProductPageItem : BasePage, IContentPage, IStructuralPage
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
                return "~//ProductPage/Index";
            }
        }

        private string argument;

        private Product currentProduct;
        public Product CurrentProduct {
            get { return currentProduct ?? GetCurrentProduct(); }
            set { currentProduct = value; }
        }

        public override PathData FindPath(string remainingUrl)
        {
            var basePath = base.FindPath(remainingUrl);
            argument = basePath.Argument;
            var pathData = basePath.CurrentItem != null ? basePath : base.FindPath(null);
            pathData.IsCacheable = false;
            return pathData;
        }      

        private Product GetCurrentProduct()
        {
            int? id = null;
            int i;
            if (int.TryParse(argument, out i)) id = i;
            var name = argument.TrimEnd('/'); // Remove trailing slash

            currentProduct = id.HasValue ? GetProduct(id.Value) : GetProductByUniqueName(name);
            return currentProduct;
        }

        /// <summary>
        /// Gets the Product by id. All parameters to the API are defaulted to whats in StormContext. If overridden, dont call the base class implementation.
        /// </summary>
        /// <param name="id">The id of the Product.</param>
        /// <returns></returns>
        public virtual Product GetProduct(int id)
        {
            var repo = IoC.Container.Resolve<IRepository>();
            return repo.Products.GetProduct(id);
        }

        /// <summary>
        /// Gets the Product by the unique url friendly name. All parameters to the API are defaulted to whats in StormContext. If overridden, dont call the base class implementation.
        /// </summary>
        /// <param name="name">The unique url name of the product. Note that names are culture dependent.</param>
        /// <returns></returns>
        public virtual Product GetProductByUniqueName(string name)
        {
            var repo = IoC.Container.Resolve<IRepository>();
            return repo.Products.GetProduct(name);
        }
    }
}
