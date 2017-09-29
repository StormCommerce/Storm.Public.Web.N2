using System;
using N2;
using N2.Definitions;
using N2.Details;
using N2.Integrity;

namespace Enferno.Public.Web.N2.Items.Pages
{
    [PageDefinition("Redirect",
            Description = "Redirects to another page or an external address.",
            SortOrder = 40,
            IconClass = "n2-icon-external-link")]
    [RestrictParents(typeof(IStructuralPage))]
    [Serializable]
    public class RedirectItem : ContentItem, IStructuralPage, IRedirect
    {
        public override string Url
        {
            get { return global::N2.Web.Url.ToAbsolute(RedirectUrl); }
        }

        [EditableUrl("Redirect to", 40, Required = true)]
        public virtual string RedirectUrl
        {
            get { return (string)base.GetDetail("RedirectUrl"); }
            set { base.SetDetail("RedirectUrl", value); }
        }

        [EditableCheckBox("301 (permanent) redirect", 100)]
        public virtual bool Redirect301
        {
            get { return (bool)(GetDetail("Redirect301") ?? true); }
            set { SetDetail("Redirect301", value, true); }
        }

        [EditableCheckBox("Visible", 40)]
        public override bool Visible
        {
            get { return base.Visible; }
            set { base.Visible = value; }
        }

        #region IRedirect Members

        public ContentItem RedirectTo
        {
            get { return Context.Current.UrlParser.Parse(RedirectUrl); }
        }

        #endregion
    }
}
