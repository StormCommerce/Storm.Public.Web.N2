using System.Linq;
using Enferno.Public.Web.N2.Items.Pages;
using N2;
using N2.Definitions;
using N2.Web;

namespace Enferno.Public.Web.N2.Utilities
{
    public static class PageHelper
    {
        public static ProductPageItem FindProductPage()
        {
            var startPage = FindStartPage();
            var productPage = startPage.Children.OfType<ProductPageItem>().FirstOrDefault();
            return productPage;
        }

        public static ManufacturerPageItem FindManufacturerPage()
        {
            var startPage = FindStartPage();
            var page = startPage.Children.OfType<ManufacturerPageItem>().FirstOrDefault();
            return page;
        }

        public static ContentItem FindStartPage()
        {
            var currentPage = Context.CurrentPage ?? Find.StartPage;           
            if (currentPage == Find.RootItem)
            {
                currentPage = !currentPage.Children.Any() ? null : currentPage.Children.First();
            }

            if (currentPage is ISitesSource)
            {
                return !currentPage.Children.Any() ? null : currentPage.Children.First();
            }

            return Find.StartPage;
        }

        public static ContentItem FindApplicationRoot()
        {
            var startPage = FindStartPage();
            if (startPage == null) return null;
            return (startPage.Parent is ISitesSource) ? startPage.Parent : null;
        }
    }
}
