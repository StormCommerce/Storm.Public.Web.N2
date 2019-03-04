
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Xml;
using Enferno.Public.InversionOfControl;
using Enferno.Public.Web.N2.Items.Pages;
using Enferno.Public.Web.Repositories;
using N2;

namespace Enferno.Public.Web.N2.Services
{
    /// <summary>
    /// Provides a sitemap format file (XML) for the site, accessed via
    /// http://www.yoursite.com/sitemap.xml
    /// See sitemap.org for details on what's outputted.
    /// </summary>
    /// <remarks>
    /// You can ping google to update your sitemap (along with the webmaster tools) using
    /// http://www.google.com/ping?sitemap=url
    /// </remarks>
    public class SiteMapHandler : IHttpHandler
    {
        private readonly IRepository repository = IoC.Resolve<IRepository>();
        private readonly ISiteRules siteRules = IoC.Resolve<ISiteRules>();

        private string domain;
        private string cultureCode;

        public bool IsReusable { get { return false; } }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ClearHeaders();
            context.Response.ClearContent();

            context.Response.ContentType = "text/xml";

            context.Response.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            context.Response.Write(GenerateSiteMap(context));
        }

        protected string GenerateSiteMap(HttpContext context)
        {
            var root = Find.RootItem;

            var cacheKey = "sitemaphandler:" + context.Request.Url.Host;

            // As this is a heavy operation, use the cache
            if (context.Cache[cacheKey] != null) return context.Cache[cacheKey].ToString();

            var list = new List<NavigatableUrl>();
            var sitePage = root.Children.FirstOrDefault(c => (c is IApplicationRoot) && ((IApplicationRoot)c).HostName == context.Request.Url.Host);
            if (sitePage == null) return "";

            var application = sitePage as IApplicationRoot;
            if (application == null) return "";

            domain = "http://" + application.HostName;
            list.Add(new NavigatableUrl(domain, ""));
            foreach (var homepage in sitePage.Children.Where(c => c is StartPageItem))
            {
                var languageRoot = homepage as StartPageItem;
                if (languageRoot == null) continue;

                cultureCode = languageRoot.LanguageCode;

                list.Add(new NavigatableUrl(domain, homepage.Url));
                RecurseTree(list, homepage);
            }
            
            var builder = new StringBuilder();
            GenerateXml(builder, list);
            var xml = builder.ToString();

            // Add to the cache for 12 hours.           
            context.Cache.Add(cacheKey, xml, null, DateTime.Today.AddMinutes(720), Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
            return xml;
        }

        private static void GenerateXml(StringBuilder builder, IEnumerable<NavigatableUrl> list)
        {
            var settings = new XmlWriterSettings {OmitXmlDeclaration = true, Indent = true, Encoding = Encoding.UTF8};
            using (var stringWriter = new StringWriter(builder))
            using (var writer = XmlWriter.Create(stringWriter, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

                foreach (var item in list.Where(p => !p.Url.StartsWith("http")))
                {
                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", item.Domain + item.Url);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
                stringWriter.Flush();
            }
        }

        /// <summary>
        /// Builds up the entire site tree recursively, adding items to the list
        /// </summary>
        /// <param name="list">This should be an empty list</param>
        /// <param name="parent">This should be called using the root item</param>
        private void RecurseTree(List<NavigatableUrl> list, ContentItem parent)
        {
            foreach (var item in parent.GetChildren().Where(IsPagePublished))
            {
                if (item is ManufacturerPageItem)
                {
                    if (!CanIndex(item)) continue;
                    var ml = repository.Products.ListManufacturers(cultureCode: cultureCode);
                    list.AddRange(ml.Items.Select(mfr => new NavigatableUrl(domain, siteRules.FormatManufacturerPageUrl(item.Url, mfr))));
                }
                else if (item is ProductPageItem)
                {
                    if (!CanIndex(item)) continue;
                    var categories = repository.Products.ListCategoryItems(cultureCode);
                    var seed = string.Join(",", categories.Select(c => c.CategoryId.ToString(CultureInfo.InvariantCulture)));
                    var products = repository.Products.ListProducts(false, categorySeed: seed);
                    list.AddRange(products.Items.Select(product => new NavigatableUrl(domain, siteRules.FormatProductPageUrl(item.Url, product))));
                }
                else
                {
                    if (CanIndex(item)) list.Add(new NavigatableUrl(domain, item.Url));
                    RecurseTree(list, item);
                }
            }
        }

        private static bool IsPagePublished(ContentItem item)
        {
            return item.IsPage && item.IsPublished() && item.State == ContentState.Published;
        }

        private static bool CanIndex(ContentItem item)
        {
            if (item is BasePage) return !(item as BasePage).NoIndex;
            return item.Visible;
        }
    }

    internal class NavigatableUrl
    {
        public string Domain { get; set; }
        public string Url { get; set; }
        public string Modified { get; set; }

        public NavigatableUrl(string domain, string url)
        {
            Domain = domain;
            Url = url;
        } 
    }
}