using System;
using N2;
using N2.Definitions;
using N2.Web.UI;

namespace Enferno.Public.Web.N2.Items.Parts
{
    [SidebarContainer("Metadata", 100, HeadingText = "Metadata")]
    [Serializable]
    public abstract class BasePart : ContentItem, IPart
    {
    }
}