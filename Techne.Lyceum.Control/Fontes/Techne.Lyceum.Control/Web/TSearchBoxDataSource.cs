using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using Techne.Controls;
using Techne.Library.Sql.Structure;

namespace Techne.Web
{
    [PersistChildren(false), ParseChildren(true), DefaultProperty("TypeName"), ToolboxItem(false), DefaultEvent("Selecting")]
    public class TSearchBoxDataSource : DataSourceControl
    {
        private TSearchBoxDataView _view;

        private IRecordContainer recordContainer;

        public string Connection { get; set; }

        /// <summary>
        ///   Obtém o record container no qual o controle está inserido.
        ///   Devolve null caso não esteja em nenhum record container.
        /// </summary>
        [Browsable(false)]
        public IRecordContainer RecordContainer
        {
            get
            {
                // TODO Deve-se criar uma booleana indicando que o GetRecordContainer() já foi chamado, pois se ele devolver null, o próximo acesso a RecordContainer(get) o chamará novamente.
                if (this.recordContainer == null)
                {
                    this.recordContainer = Techne.Controls.TControl.GetRecordContainer(this);
                }

                return this.recordContainer;
            }
        }

        public string SqlOrder
        {
            get
            {
                return (string)this.ViewState["SqlOrder"];
            }

            set
            {
                this.ViewState["SqlOrder"] = value == null ? string.Empty : value;
            }
        }

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

        protected internal SqlSelect SqlSelectExtended
        {
            get
            {
                var sqlSelect = this.SqlSelect.Clone();

                // if (!sqlSelect.Columns.Contains(KeyCol)) sqlSelect.Columns.Insert(0, SqlSelectColumn.Parse(KeyCol));
                // if (!sqlSelect.Columns.Contains(ArgumentCol)) sqlSelect.Columns.Insert(1, SqlSelectColumn.Parse(ArgumentCol));

                // foreach (string alternateKey in BaseAlternateKeys)
                // if (!sqlSelect.Columns.Contains(alternateKey))
                // sqlSelect.Columns.Add(alternateKey);
                return sqlSelect;
            }
        }

        protected override DataSourceView GetView(string viewName)
        {
            if (this._view == null)
            {
                this._view = new TSearchBoxDataView(this, "DefaultView", System.Web.HttpContext.Current);
            }

            return this._view;
        }

        protected override ICollection GetViewNames()
        {
            return new[] { "DefaultView" };
        }
    }
}