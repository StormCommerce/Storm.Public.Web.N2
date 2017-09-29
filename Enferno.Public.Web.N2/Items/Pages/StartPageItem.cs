using System;
using System.Globalization;
using N2;
using N2.Definitions;
using N2.Details;
using N2.Engine.Globalization;
using N2.Integrity;

namespace Enferno.Public.Web.N2.Items.Pages
{
    /// <summary>
    /// This is the start Page on a site
    /// </summary>
    [PageDefinition(Name = "Startpage (Enferno)", Description = "Startpage for a new language")]
    [RestrictParents(typeof(ApplicationRootPage<StartPageItem>))]
    [Serializable]
    public class StartPageItem : BasePage, IStartPage, IStructuralPage, ILanguage
    {
        #region ILanguage Members

        public virtual string LanguageCode { get; set; }

        [EditableText(Title = "Language name", Required = true, SortOrder = 5)]
        public virtual string LanguageName { get; set; }

        public string LanguageTitle => string.IsNullOrEmpty(LanguageCode) ? "" : new CultureInfo(LanguageCode).DisplayName;

        #endregion
    }
}
