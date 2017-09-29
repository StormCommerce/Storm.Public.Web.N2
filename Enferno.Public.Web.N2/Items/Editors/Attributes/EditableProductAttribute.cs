using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Enferno.Public.Web.N2.Items.Editors.Data;
using N2.Details;
using N2.Web.UI.WebControls;

namespace Enferno.Public.Web.N2.Items.Editors.Attributes
{
    public class EditableProductAttribute : AbstractEditableAttribute
    {
        protected override Control AddEditor(Control container)
        {
            var prd = new TextBox { ID = "Product", ClientIDMode = ClientIDMode.AutoID };
            var img = new UrlSelector { ID = "ImageKey", ClientIDMode = ClientIDMode.AutoID, AvailableModes = UrlSelectorMode.Files, SelectableExtensions = ".gif,.png,.jpg,.jpeg" };
            var name = new TextBox { ID = "Name", ClientIDMode = ClientIDMode.AutoID };
            //var desc = new FreeTextArea { ID = "DescText", ClientIDMode = ClientIDMode.AutoID };

            container.Controls.Add(new Literal { Text = "<br />"});
            container.Controls.Add(prd);
            container.Controls.Add(new Literal { Text = "</div><div class=\"editDetail\"><label for=\"" + name.ClientID + "\" class=\"editorLabel\" data-sortorder=\"0\">Name</label>" });
            container.Controls.Add(name);
            container.Controls.Add(new Literal { Text = "</div><div class=\"editDetail\"><label for=\"" + img.ClientID + "\" class=\"editorLabel\" data-sortorder=\"0\">Image</label>" });
            container.Controls.Add(img);
            container.Controls.Add(new Literal { Text = "</div><div class=\"editDetail\">" });
            // Display all images of product
            //container.Controls.Add(new Literal { Text = "</div><div class=\"editDetail\"><label for=\"" + desc.ClientID + "\" class=\"editorLabel\" data-sortorder=\"0\">Text</label>" });
            //container.Controls.Add(desc);
            container.Controls.Add(new Literal { Text = "</div>"});
            
            const string searchType = "product";

            var script = String.Format(@"jQuery(function ($) {{
            var controller = new Search({{ ids: {{ id: '#{0}', name: '#{1}', imageKey: '#{2}' }}, types: {{ searchType: '{3}' }} }});
            controller.init();
            }});", prd.ClientID, name.ClientID, img.ClientID, searchType);

            // TODO Add /Scripts/search.js as a script block instead when functions are debugged and stable.
            ScriptManager.RegisterClientScriptInclude(prd, typeof(Page), "product", "/Scripts/stormsearch.js");
            ScriptManager.RegisterClientScriptBlock(prd, typeof(TextBox), "setControlId", script, true);

            return container;
        }

        public override void UpdateEditor(global::N2.ContentItem item, Control editor)
        {
            var data = item[Name] as ProductEditData;
            var prd = editor.FindControl("Product") as TextBox;
            var img = editor.FindControl("ImageKey") as UrlSelector;
            var name = editor.FindControl("Name") as TextBox;
            prd.Text = data.ProductId.ToString(CultureInfo.InvariantCulture);
            img.Url = data.ImageKey;
            name.Text = data.Name;
        }

        public override bool UpdateItem(global::N2.ContentItem item, Control editor)
        {
            var isChanged = false;
            var prd = editor.FindControl("Product") as TextBox;
            var img = editor.FindControl("ImageKey") as UrlSelector;
            var name = editor.FindControl("Name") as TextBox;

            var data = item[Name] as ProductEditData;

            var selectedId = data.ProductId;
            if (int.TryParse(prd.Text, out selectedId) && data.ProductId != selectedId)
            {
                isChanged = true;
                data.ProductId = selectedId;
            }
            if (data.ImageKey != img.Url)
            {
                isChanged = true;
                data.ImageKey = img.Url;
            }
            if (data.Name != name.Text)
            {
                isChanged = true;
                data.Name = name.Text;
            }

            item[Name] = data;
            //Engine.Persister.Get(item.ID);
            return isChanged;
        }
    }
}