using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using N2.Details;

namespace Enferno.Public.Web.N2.Items.Editors.Attributes
{
    public class EditableProductIdAttribute : AbstractEditableAttribute
    {
        protected override Control AddEditor(Control container)
        {
            var textbox = new TextBox {ID = "ProductId", ClientIDMode = ClientIDMode.AutoID};
            container.Controls.Add(textbox);
            var controlId = textbox.ClientID;
            const string searchType = "product";

            //var script = String.Format("setControlId('#{0}');", controlId);
            var script = String.Format(@"jQuery(function ($) {{
            var controller = new Search({{ ids: {{ id: '#{0}' }}, types: {{ searchType: '{1}' }} }});
            controller.init();
            }});", controlId, searchType);
            
            
            ScriptManager.RegisterClientScriptInclude(textbox, typeof(Page), "product", "/Scripts/stormsearch.js");

            ScriptManager.RegisterClientScriptBlock(textbox, typeof(TextBox), "setControlId", script, true);

            return textbox;
        }

        public override void UpdateEditor(global::N2.ContentItem item, Control editor)
        {
            // here we update the editor control to reflect what was saved
            var selectedItem = (int)item[Name];
            var textBox = editor as TextBox;
            if (textBox != null) textBox.Text = selectedItem.ToString(CultureInfo.InvariantCulture);
        }

        public override bool UpdateItem(global::N2.ContentItem item, Control editor)
        {
            // here we update the item from dropdown selection
            var textBox = editor as TextBox;
            if (textBox == null) return true;
            var selectedId = int.Parse(textBox.Text);

            var previouslySelected = (int)item[Name];
            if (previouslySelected == selectedId)
                // no change
                return false;

            item[Name] = selectedId;
            //Engine.Persister.Get(item.ID);
            return true;
        }
    }
}