using System.ComponentModel;
using Techne.Library.Sql.Structure;

namespace Techne.Controls
{
    internal class DropDownListColumn : TGridColumn
    {
        public DropDownListColumn()
        {
            this.Connection = string.Empty;
            this.DataTextField = string.Empty;
            this.DataValueField = string.Empty;
            this.NotFoundText = TDropDownListBase.NotFoundText_Def;
            this.NullAllowed = false;
            this.NullText = TDropDownListBase.NullText_Def;
            this.SelectedIndex = -1;
            this.SqlOrder = string.Empty;
            this.SqlSelect = new SqlSelect();
            this.SqlWhere = string.Empty;
        }

        [DefaultValue(false), Category("Techne"), Description("Indica se o controle permite o valor DBNull")]
        public virtual bool NullAllowed
        {
            get
            {
                return (bool)this.ViewState["NullAllowed"];
            }

            set
            {
                this.ViewState["NullAllowed"] = value;
            }
        }

        [Category("Techne"), DefaultValue(""), Description("ConnectionString, ou chave da connectionString na classe Techne.Data.ConnectionList, a ser usada para executar o comando SQLQuery. ATEN\x00c7\x00c3O: esta propriedade \x00e9 case-sensitive.")]
        public string Connection
        {
            get
            {
                return (string)this.ViewState["Connection"];
            }

            set
            {
                this.ViewState["Connection"] = value == null ? string.Empty : value.Trim();
            }
        }

        [Category("Techne"), DefaultValue(""), Description("Indica a coluna da propriedade SqlSelect que ser\x00e1 mostrada na lista do controle. Caso n\x00e3o seja informada e houver mais de uma coluna, a segunda coluna ser\x00e1 utilizada.")]
        public string DataTextField
        {
            get
            {
                return (string)this.ViewState["DataTextField"];
            }

            set
            {
                this.ViewState["DataTextField"] = value == null ? string.Empty : value;
            }
        }

        [Category("Techne"), DefaultValue(""), Description("Indica a coluna da propriedade SqlSelect que ser\x00e1 devolvida pela propriedade DBValue. Caso n\x00e3o seja informada, a primeira coluna ser\x00e1 utilizada.")]
        public string DataValueField
        {
            get
            {
                return (string)this.ViewState["DataValueField"];
            }

            set
            {
                this.ViewState["DataValueField"] = value == null ? string.Empty : value;
            }
        }

        [DefaultValue("<Lista Vazia>"), Category("Techne"), Description("Texto a ser usado quando o n\x00famero de \x00edtens do DropDownList \x00e9 zero")]
        public string NotFoundText
        {
            get
            {
                return (string)this.ViewState["NotFoundText"];
            }

            set
            {
                this.ViewState["NotFoundText"] = value == null ? string.Empty : value;
            }
        }

        [Category("Techne"), DefaultValue("<N\x00e3o Informado>"), Description("Texto a ser usado como op\x00e7\x00e3o para indicar o valor DBNull")]
        public string NullText
        {
            get
            {
                return (string)this.ViewState["NullText"];
            }

            set
            {
                this.ViewState["NullText"] = value == null ? string.Empty : value;
            }
        }

        [
            Browsable(false), 
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), 
        ]
        public int SelectedIndex
        {
            get
            {
                return (int)this.ViewState["SelectedIndex"];
            }

            set
            {
                this.ViewState["SelectedIndex"] = value < 0 ? -1 : value;
            }
        }

        [Category("Techne"), DefaultValue(""), Description("Cl\x00e1usula ORDER BY para ordenar a lista de itens do controle.")]
        public string SqlOrder
        {
            get
            {
                return (string)this.ViewState["SqlOrder"];
            }

            set
            {
                this.ViewState["SqlOrder"] = value == null ? string.Empty : value.Trim();
            }
        }

        [Category("Techne"), DefaultValue((string)null), Description("Select para popular a lista")]
        public SqlSelect SqlSelect
        {
            get
            {
                return (SqlSelect)this.ViewState["SqlSelect"];
            }

            set
            {
                this.ViewState["SqlSelect"] = value == null ? new SqlSelect() : value;
            }
        }

        [Category("Techne"), DefaultValue(""), Description("Cl\x00e1usula Where do select que popula a lista")]
        public string SqlWhere
        {
            get
            {
                return (string)this.ViewState["SqlWhere"];
            }

            set
            {
                this.ViewState["SqlWhere"] = value == null ? string.Empty : value;
            }
        }

        protected override bool ExpandInEditMode
        {
            get
            {
                return true;
            }
        }

        protected override void CopyProperties(ITControl target)
        {
            var drp = target as TDropDownList;
            if (drp == null)
            {
                return;
            }

            base.CopyProperties(target);

            drp.Connection = this.Connection;
            drp.DataTextField = this.DataTextField;
            drp.DataValueField = this.DataValueField;
            drp.NotFoundText = this.NotFoundText;
            drp.NullAllowed = this.NullAllowed;
            drp.NullText = this.NullText;
            drp.SelectedIndex = this.SelectedIndex;
            drp.SQLOrder = this.SqlOrder;
            drp.SqlSelect = this.SqlSelect;
            drp.SQLWhere = this.SqlWhere;
        }

        protected override ITControl GetTControl(string id)
        {
            var drp = new TDropDownList();
            drp.ID = id;
            this.CopyProperties(drp);
            return drp;
        }
    }
}