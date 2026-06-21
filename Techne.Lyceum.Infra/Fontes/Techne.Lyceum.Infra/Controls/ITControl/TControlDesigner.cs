using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace Techne.Controls.Design
{
    public abstract class TControlDesigner : ControlDesigner
    {
        public override string GetDesignTimeHtml()
        {
            try
            {
                var swriter = new StringWriter();
                var writer = new HtmlTextWriter(swriter);

                if (this.Component != null)
                {
                    if (!(this.Component is ITControl))
                    {
                        throw new ApplicationException(
                            "O controle " + this.Component.GetType().FullName + " não pode ser utilizado por TControlDesigner."
                            );
                    }

                    var control = (ITControl)this.Component;
                    if (control is TControlEditable)
                    {
                        ((TControlEditable)control).ConfigureObject();
                    }

                    // Chama RenderSpecific() antes de renderizar o caption porque este
                    // só deve ser renderizado se RenderSpecific() renderizar algo.
                    string spec;
                    {
                        var spswriter = new StringWriter();
                        var spwriter = new HtmlTextWriter(spswriter);
                        this.RenderSpecific(spwriter);
                        spec = spswriter.ToString();
                    }

                    if (spec.Length > 0)
                    {
                        // TODO Quando o controle estiver dentro de uma grid, não deve renderizar o caption.
                        var caption = control.Caption == "?"
                                          ? (control.ColumnName.Length > 0 ? "<" + StrLib.ToProper(control.ColumnName) + ">" : string.Empty)
                                          : control.Caption;
                        if (caption.Length > 0)
                        {
                            var lbl = new Label();
                            lbl.Text = HttpUtility.HtmlEncode(caption) + "&nbsp;";
                            lbl.RenderControl(writer);
                        }

                        writer.Write(spec);
                    }
                }

                var str = swriter.ToString();
                return str.Length > 0 ? str : this.GetEmptyDesignTimeHtml();
            }
            catch (Exception exc)
            {
                return this.GetErrorDesignTimeHtml(exc);
            }
        }

        protected abstract void RenderControlEditMode(HtmlTextWriter writer);

        protected virtual void RenderControlViewMode(HtmlTextWriter writer)
        {
            var source = (TControl)this.Component;

            var lbl = new Label();
            source.CopyProperties(lbl);
            lbl.Text = "[" + source.ID + "]";
            lbl.RenderControl(writer);
        }

        protected virtual void RenderSpecific(HtmlTextWriter writer)
        {
            if ((base.Component is ITControlEditable) && !((ITControlEditable)base.Component).ReadOnly)
            {
                this.RenderControlEditMode(writer);
            }
            else
            {
                this.RenderControlViewMode(writer);
            }
        }

        protected override string GetErrorDesignTimeHtml(Exception e)
        {
            if (e != null)
            {
                var targetSite = e.TargetSite;
            }

            return "[" + this.ID + ((e != null) ? (": " + e.Message) : string.Empty) + "]";
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);

            // Remove algumas propriedades se o controle não pertence a um record manager.
            if (this.Component is ITControl)
            {
                var control = (ITControl)this.Component;
                if (control.Manager == null)
                {
                    if (control is ITControlEditable)
                    {
                        properties.Remove("KeepValueAfterSave");
                    }
                }
                else
                {
                    if (control.ColumnName.Length > 0)
                    {
                        properties.Remove("DataType");
                    }
                }
            }
        }
    }
}