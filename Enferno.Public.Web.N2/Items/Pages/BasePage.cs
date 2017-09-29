
using System;
using N2;
using N2.Definitions;
using N2.Details;
using N2.Integrity;
using N2.Web.UI;

namespace Enferno.Public.Web.N2.Items.Pages
{
    /// <summary>
    /// Base implementation for pages on a site.
    /// </summary>
    [WithEditableTitle]
    [WithEditableName(ContainerName = "Metadata")]
    [WithEditableVisibility(ContainerName = "Metadata")]
    [SidebarContainer("Metadata", 100, HeadingText = "Metadata")]
    [TabContainer("Content", "Content", 1, TabText = "Content")]
    [TabContainer("SEO", "SEO", 2, TabText = "SEO")]
    [RestrictParents(typeof(IPage))]
    [Serializable]
    public class BasePage : ContentItem, IPage
    {
        [EditableText(Title = "Page title", SortOrder = 20, ContainerName = "SEO", HelpTitle = "Displayed in the browser title area and on external search results")]
        public virtual string HeadTitle { get; set; }

        [EditableMetaTag(Title = "Description", SortOrder = 60, ContainerName = "SEO", HelpTitle = "Description used by search engine to describe this page")]
        public virtual string Description { get; set; }

        [EditableMetaTag(Title = "Keywords", SortOrder = 40, ContainerName = "SEO", HelpTitle = "Keywords used to search engine to categorize this page.")]
        public virtual string Keywords { get; set; }

        [EditableCheckBox(Title = "No Index", SortOrder = 80, ContainerName = "SEO", HelpTitle = "Check to tag as NOINDEX and remove from sitemap.")]
        public virtual bool NoIndex { get; set; }

    }
}
