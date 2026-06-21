using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Techne.Controls
{
    internal abstract class TGridColumn : DataGridColumn, INamingContainer
    {
        public const string HeaderText_Def = "?";

        private const bool ShowTotal_Def = false;

        private const string TotalToolTip_Def = "Valor total";

        private string totalToolTip;

        public TGridColumn(string columnName)
        {
            this.ColumnName = columnName;
            this.HeaderText = HeaderText_Def;
            this.SortExpression = columnName;

            this.DataType = DbType.VarChar;
            this.Format = string.Empty;
            this.ControlMessageType = TControl.MessageControlType_Def;
            this.ReadOnly = false;
            this.ReadOnlyCssClass = TControl.ReadOnlyCssClass_Def;
            this.ShowTotal = ShowTotal_Def;
            this.TotalToolTip = TotalToolTip_Def;
        }

        public TGridColumn() : this(string.Empty)
        {
        }

        [
            Category("Techne"), 
            DefaultValue(TControl.MessageControlType_Def), 
            Description("Determina o modo como a mensagem de erro é mostrada: ToolTip ou Mensagem")
        ]
        public virtual ControlMessageType ControlMessageType
        {
            get
            {
                return (ControlMessageType)this.ViewState["ControlMessageType"];
            }

            set
            {
                this.ViewState["ControlMessageType"] = value;
            }
        }

        [
            DefaultValue(HeaderText_Def), 
        ]
        public override string HeaderText
        {
            get
            {
                return base.HeaderText;
            }

            set
            {
                base.HeaderText = value == null ? string.Empty : value;
            }
        }

        [
            Browsable(false), 
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), 
        ]
        public override string SortExpression
        {
            get
            {
                return base.SortExpression;
            }

            set
            {
                base.SortExpression = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(""), 
            TypeConverter(typeof (ColumnNameConverter)), 
        ]
        public string ColumnName
        {
            get
            {
                return (string)this.ViewState["ColumnName"];
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                var oldName = this.ColumnName;

                this.ViewState["ColumnName"] = value == null ? string.Empty : value;

                if (this.SortExpression == oldName)
                {
                    this.SortExpression = this.ColumnName;
                }

                this.OnColumnChanged();
            }
        }

        [
            Category("Techne"), 
            DefaultValue(DbType.VarChar), 
        ]
        public DbType DataType
        {
            get
            {
                return (DbType)this.ViewState["DataType"];
            }

            set
            {
                if (value == DbType.Null)
                {
                    throw new InvalidOperationException("O valor " + value + " não é permitido.");
                }

                if (value == DbType.Raw)
                {
                    throw new NotImplementedException("O valor " + value + " não é suportado.");
                }

                this.ViewState["DataType"] = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue("")
        ]
        public string Format
        {
            get
            {
                return (string)this.ViewState["Format"];
            }

            set
            {
                this.ViewState["Format"] = value == null ? string.Empty : value;
                this.OnColumnChanged();
            }
        }

        [
            Category("Techne"), 
            DefaultValue(false)
        ]
        public bool ReadOnly
        {
            get
            {
                return (bool)this.ViewState["ReadOnly"];
            }

            set
            {
                this.ViewState["ReadOnly"] = value;
                this.OnColumnChanged();
            }
        }

        [
            Category("Techne"), 
            DefaultValue(TControl.ReadOnlyCssClass_Def), 
            Description("CssClass para quando o controle estiver no modo ReadOnly")
        ]
        public string ReadOnlyCssClass
        {
            get
            {
                return (string)this.ViewState["ReadOnlyCssClass"];
            }

            set
            {
                this.ViewState["ReadOnlyCssClass"] = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(ShowTotal_Def), 
            Description(
                "Mostra totalizador para colunas numéricas. Ignorado se o tipo da coluna não for Number. " +
                "É necessário que ShowFooter da grid seja True."
                ), 
        ]
        public bool ShowTotal
        {
            get
            {
                return (bool)this.ViewState["ShowTotal"];
            }

            set
            {
                this.ViewState["ShowTotal"] = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(TotalToolTip_Def), 
        ]
        public string TotalToolTip
        {
            get
            {
                return this.totalToolTip;
            }

            set
            {
                this.totalToolTip = value != null ? value : string.Empty;
            }
        }

        protected virtual bool ExpandInEditMode
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        ///   Encontra uma coluna numa grid através da propriedade ColumnName.
        ///   O escopo são as colunas da grid derivadas de TGridColumn.
        ///   Se houver mais de uma, devolve somente a primeira.
        /// </summary>
        public static TGridColumn Find(string columnName, DataGrid grid)
        {
            var i = IndexOf(columnName, grid);
            return i >= 0 ? (TGridColumn)grid.Columns[i] : null;
        }

        public override void InitializeCell(TableCell cell, int columnIndex, ListItemType itemType)
        {
            base.InitializeCell(cell, columnIndex, itemType);

            if (itemType == ListItemType.EditItem && !this.ReadOnly)
            {
                throw new NotSupportedException("Linhas do tipo EditItem não são suportadas.");
            }
            else if (TGridItem.ItemTypeIsData(itemType))
            {
                ((TTableCell)cell).SetTControl(this.GetTControl("_" + columnIndex + "_" + (this.ColumnName.Length > 0 ? this.ColumnName : string.Empty)));
            }
        }

        public override string ToString()
        {
            return this.ColumnName.Length == 0 ? base.ToString() : this.ColumnName;
        }

        /// <summary>
        ///   Determina o índice de uma coluna numa grid através da propriedade ColumnName.
        ///   O escopo são as colunas da grid derivadas de TGridColumn.
        ///   Se houver mais de uma, devolve somente o índice da primeira.
        ///   Se não encontrar, devolve -1.
        /// </summary>
        internal static int IndexOf(string columnName, DataGrid grid)
        {
            for (var i = 0; i < grid.Columns.Count; i++)
            {
                var column = grid.Columns[i] as TGridColumn;
                if (column != null && string.Compare(column.ColumnName, columnName, true) == 0)
                {
                    return i;
                }
            }

            return -1;
        }

        internal DataGrid GetOwner()
        {
            return this.Owner;
        }

        protected abstract ITControl GetTControl(string id);

        /// <summary>
        ///   Copia as propriedades deste TGridColumn para um TControl.
        /// </summary>
        protected virtual void CopyProperties(ITControl target)
        {
            if (this.ExpandInEditMode)
            {
                ((WebControl)target).Width = Unit.Percentage(100);
            }

            target.ColumnName = this.ColumnName;
            target.DataType = this.DataType;
            target.Format = this.Format;
            target.ControlMessageType = this.ControlMessageType;
            target.ReadOnlyCssClass = this.ReadOnlyCssClass;

            if (target is ITControlEditable)
            {
                ((ITControlEditable)target).ReadOnly = this.ReadOnly;
            }
        }
    }
}