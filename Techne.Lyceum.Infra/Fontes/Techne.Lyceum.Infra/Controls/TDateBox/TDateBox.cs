using System;
using System.ComponentModel;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Techne.Controls
{
    [Designer("Techne.Controls.Design.TDateBoxDesigner, Techne")]
    [ToolboxData("<{0}:TDateBox runat=server></{0}:TDateBox>")]
    [ControlValueProperty("DBValue")]
    internal class TDateBox : TTextBox
    {
        private const string ButtonClass_Def = "TDateBoxButton";

        private const int DateColumns_Def = 12;

        private const string DayClass_Def = "TDateBoxDay";

        private const string DayHeaderClass_Def = "TDateBoxDayHeader";

        private const string Format_Def = "d";

        private const string ImageButtonUrl_Def = "~/images/TDateBoxButton.gif";

        private const string OtherMonthDayClass_Def = "TDateBoxOtherMonthDay";

        private const string ScriptURL = "~/scripts/TDateBox.js";

        private const string TitleClass_Def = "TDateBoxTitle";

        public TDateBox()
        {
            this.ChildControlsCreated = false;

            this.ButtonClass = ButtonClass_Def;
            this.Columns = DateColumns_Def;
            this.DataType = DbType.Date;
            this.DayClass = DayClass_Def;
            this.DayHeaderClass = DayHeaderClass_Def;
            this.ImageButtonUrl = ImageButtonUrl_Def;
            this.OtherMonthDayClass = OtherMonthDayClass_Def;
            this.TitleClass = TitleClass_Def;
        }

        public static string[] AllowedFormats
        {
            get
            {
                return new[] { "d", "M/yyyy", "G" };
            }
        }

        [
            DefaultValue(DateColumns_Def), 
        ]
        public override int Columns
        {
            get
            {
                return base.Columns;
            }

            set
            {
                base.Columns = value;
            }
        }

        public override DbObject DBValue
        {
            get
            {
                var currentFormat = this.Format;

                if (currentFormat == "G" && this.Mode == ControlMode.Edit)
                {
                    this.Format = "d";
                }

                var value = base.DBValue;
                this.Format = currentFormat;

                return value;
            }

            set
            {
                var currentFormat = this.Format;

                if (currentFormat == "G" && this.Mode == ControlMode.Edit)
                {
                    this.Format = "d";
                }

                base.DBValue = value;
                this.Format = currentFormat;
            }
        }

        [
            Browsable(false), 
            DefaultValue(DbType.Date), 
        ]
        public override DbType DataType
        {
            get
            {
                return base.DataType;
            }

            set
            {
                base.DataType = value;
            }
        }

        [
            DefaultValue(Format_Def), 
            TypeConverter(typeof (DateTimeFormatCharConverter))
        ]
        public override string Format
        {
            get
            {
                return base.Format;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                if (value == string.Empty)
                {
                    value = Format_Def;
                }

                // Verifica se o formato é um dos suportados
                if (Array.IndexOf(AllowedFormats, value) < 0)
                {
                    throw new ArgumentException(
                        "O caracteres de formatação permitidos para TDateBox.Format são " + StrLib.EnumerableToStr(AllowedFormats, ", ", "'") + ". ");
                }

                base.Format = value;
            }
        }

        [
            Browsable(false), 
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public override string FormatDateEdit
        {
            get
            {
                return this.Format;
            }

            set
            {
            }
        }

        [
            Category("Techne - Appearance"), 
            DefaultValue(ButtonClass_Def), 
        ]
        public string ButtonClass
        {
            get
            {
                return (string)this.ViewState["ButtonClass"];
            }

            set
            {
                this.ViewState["ButtonClass"] = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne - Appearance"), 
            DefaultValue(DayClass_Def), 
        ]
        public string DayClass
        {
            get
            {
                return (string)this.ViewState["DayClass"];
            }

            set
            {
                this.ViewState["DayClass"] = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne - Appearance"), 
            DefaultValue(DayHeaderClass_Def), 
        ]
        public string DayHeaderClass
        {
            get
            {
                return (string)this.ViewState["DayHeaderClass"];
            }

            set
            {
                this.ViewState["DayHeaderClass"] = value == null ? string.Empty : value;
            }
        }

        [
            Bindable(true), 
            Category("Appearance"), 
            DefaultValue(ImageButtonUrl_Def)
        ]
        public string ImageButtonUrl
        {
            get
            {
                return (string)this.ViewState["ImageButtonUrl"];
            }

            set
            {
                this.ViewState["ImageButtonUrl"] = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne - Appearance"), 
            DefaultValue(OtherMonthDayClass_Def), 
        ]
        public string OtherMonthDayClass
        {
            get
            {
                return (string)this.ViewState["OtherMonthDayClass"];
            }

            set
            {
                this.ViewState["OtherMonthDayClass"] = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne - Appearance"), 
            DefaultValue(TitleClass_Def), 
        ]
        public string TitleClass
        {
            get
            {
                return (string)this.ViewState["TitleClass"];
            }

            set
            {
                this.ViewState["TitleClass"] = value == null ? string.Empty : value;
            }
        }

        public override string GetValueError()
        {
            string ret = null, format = this.Format;
            if (format == "G" && this.Mode == ControlMode.Edit)
            {
                this.Format = "d";
            }

            ret = base.GetValueError();
            this.Format = format;
            return ret;
        }

        internal static void RenderDateBoxButton(TDateBox dateBox, HtmlTextWriter writer)
        {
            string format;
            if (dateBox.Format == "G" || dateBox.Format.Length == 0)
            {
                format = StrLib.GetDateFormat('d', Thread.CurrentThread.CurrentCulture.DateTimeFormat);
            }
            else if (dateBox.Format.Length == 1)
            {
                format = StrLib.GetDateFormat(dateBox.Format[0], Thread.CurrentThread.CurrentCulture.DateTimeFormat);
            }
            else
            {
                format = dateBox.Format;
            }

            var img = new Image();
            img.ImageUrl = TUtil.TranslateRelativeUrl(dateBox.ImageButtonUrl, dateBox);
            img.Style.Clear();

            writer.AddAttribute("id", dateBox.ClientID + "_btn");
            writer.AddAttribute("type", "button");
            writer.AddAttribute("class", "TDateBoxButton");
            writer.AddStyleAttribute("border-width", "1px");
            if (!dateBox.Height.IsEmpty)
            {
                writer.AddStyleAttribute("height", dateBox.Height.ToString());
            }

            writer.AddStyleAttribute("padding", "0px");
            writer.AddStyleAttribute("margin", "0px");
            writer.RenderBeginTag("button");
            img.RenderControl(writer);
            writer.RenderEndTag();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (this.Page != null && !this.Page.ClientScript.IsClientScriptBlockRegistered(typeof (TDateBox), "TDateBox"))
            {
                this.Page.ClientScript.RegisterClientScriptBlock(typeof (TDateBox), 
                                                                 "TDateBox", 
                                                                 "\r\n<script language=\"javascript\" src=\"" + TUtil.TranslateRelativeUrl(ScriptURL, this) + "\"></script>\r\n"
                    );
            }
        }

        protected override void PreTControlCtor()
        {
            base.PreTControlCtor();
            this.Format = Format_Def; // Deve ser antes de DBValue
        }

        protected override void RenderControlEditMode(HtmlTextWriter writer)
        {
            string style = null;

            if (this.Page != null && !this.Page.ClientScript.IsStartupScriptRegistered(typeof (TDateBox), this.UniqueID + "ClientEvents"))
            {
                this.Page.ClientScript.RegisterStartupScript(typeof (TDateBox), 
                                                             this.UniqueID + "ClientEvents", 
                                                             "\r\n<script language=\"javascript\">\r\n" +
                                                             "TDateBox_AttachEvents('" + this.ClientID + "');\r\n" +
                                                             "</script>\r\n"
                    );
            }

            if (this.Attributes["style"] != null)
            {
                style = this.Attributes["style"];
                this.Attributes.Remove("style");
                writer.AddAttribute("style", style);
            }

            writer.AddStyleAttribute("white-space", "nowrap");
            writer.RenderBeginTag("span");
            if (this.DayClass.Length > 0)
            {
                this.Attributes.Add("dayclass", this.DayClass);
            }

            if (this.DayHeaderClass.Length > 0)
            {
                this.Attributes.Add("dayheaderclass", this.DayHeaderClass);
            }

            if (this.TitleClass.Length > 0)
            {
                this.Attributes.Add("titleclass", this.TitleClass);
            }

            if (this.OtherMonthDayClass.Length > 0)
            {
                this.Attributes.Add("othermonthdayclass", this.OtherMonthDayClass);
            }

            base.RenderControlEditMode(writer);

            if (this.Enabled)
            {
                RenderDateBoxButton(this, writer);
            }

            if (style != null)
            {
                this.Attributes.Add("style", style);
            }

            writer.RenderEndTag();
        }

        protected override void RenderExtra(HtmlTextWriter writer)
        {
            if (this.Enabled && this.Mode == ControlMode.Edit)
            {
                // Renderiza o calendário.
                writer.AddStyleAttribute("position", "absolute");
                writer.AddStyleAttribute("visibility", "hidden");
                writer.AddStyleAttribute("z-index", "10000");
                writer.AddStyleAttribute("border-style", "solid");
                writer.AddStyleAttribute("border-width", "1px");
                writer.AddStyleAttribute("border-color", "black");
                writer.AddStyleAttribute("background-color", "white");
                writer.AddStyleAttribute("display", "inline");
                writer.AddStyleAttribute("font-size", "xx-small");
                writer.AddStyleAttribute("font-family", "Arial");
                writer.AddAttribute("id", this.ClientID + "_cld");
                writer.AddAttribute("name", "tdbcal");
                writer.RenderBeginTag("span");
                writer.RenderEndTag();
            }
        }
    }

    internal class DateTimeFormatCharConverter : TypeConverter
    {
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(TDateBox.AllowedFormats);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}