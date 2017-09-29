
using System;
using System.Collections.Generic;
using System.Linq;
using N2;
using N2.Definitions;
using N2.Details;
using N2.Installation;
using N2.Integrity;
using N2.Security;
using N2.Web;
using N2.Web.UI;

namespace Enferno.Public.Web.N2.Items.Pages
{
    /// <summary>
    /// This interface is used to mark an ApplicationRoot with a certificate file to the API.
    /// </summary>
    public interface IApplicationRoot
    {
        /// <summary>
        /// The un-rooted path, within the application, to the certificate file. Ex:"App_Data/ApiCertificate.pfx"
        /// </summary>
        string CertificatePath { get; set; }

        string HostName { get; set; }
    }

    /// <summary>
    /// Redirects to the child start page that matches the user agent's language.
    /// </summary>
    [PageDefinition(IconClass = "n2-icon-globe n2-gold", InstallerVisibility = global::N2.Installation.InstallerHint.PreferredStartPage)]
    [RestrictParents(typeof(IRootPage))]
    [RestrictChildren(typeof(StartPageItem))]
    [RecursiveContainer("LanguagesContainer", 1000, RequiredPermission = Permission.Administer)]
    [Serializable]
    public class ApplicationRootPage : ApplicationRootPage<StartPageItem>
    {
        [EditableText(Title = "Market name", Required = true, SortOrder = 10)]
        public virtual string MarketName { get; set; }

        [EditableMediaUpload(Title = "Market flag", SortOrder = 20)]
        public virtual string MarketFlag { get; set; }
    }

    /// <summary>
    /// Redirects to the child start page that matches the user agent's language.
    /// </summary>
    [PageDefinition(IconClass = "n2-icon-globe n2-gold", InstallerVisibility = InstallerHint.PreferredStartPage)]
    [RestrictParents(typeof(IRootPage))]
    [RestrictChildren(typeof(StartPageItem))]
    [TabContainer("Site", "Host", 1000, ContainerName = "LanguagesContainer")]
    [RecursiveContainer("LanguagesContainer", 1000, RequiredPermission = Permission.Administer)]
    [Serializable]
    public class ApplicationRootPage<T> : BasePage, ISitesSource, IRedirect, IApplicationRoot where T : StartPageItem
    {
        #region IApplicationRoot Menbers

        [EditableText(Title = "Certificate path", ContainerName = "Site", HelpTitle = "A relative path within the application to the API-certificate. Ex: App_Data/ApiCertificate.pfx")]
        public virtual string CertificatePath { get; set; }

        #endregion

        #region ISitesSource Members

        [EditableText(Title = "Site collection host name (DNS)", ContainerName = "Site", HelpTitle = "Sets a shared host name for all languages on a site. The web server must be configured to accept this host name for this to work.")]        
        public virtual string HostName { get; set; }

        public IEnumerable<Site> GetSites()
        {
            if (!string.IsNullOrEmpty(HostName)) yield return new Site(Find.EnumerateParents(this, null, true).Last().ID, ID, HostName) { Wildcards = true };
        }

        #endregion

        #region IRedirect Members

        public string RedirectUrl
        {
            get { return Children.OfType<T>().Select(sp => sp.Url).FirstOrDefault() ?? this.Url; }
        }

        public ContentItem RedirectTo
        {
            get { return Children.OfType<T>().FirstOrDefault(); }
        }

        #endregion
    }
}
