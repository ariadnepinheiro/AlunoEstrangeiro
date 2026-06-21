using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using Techne.Data;

namespace Techne.Controls
{
    internal delegate void BulkProcessingEventHandler(object sender, BulkProcessingEventArgs args);

    [
        DefaultEvent("ItemCommand")
    ]
    internal class TDataGrid : TDataGridBase, 
                               IContainerManager, IContainerManagerInternal, IDepender
    {
        // (em ordem alfabética)
        private const bool AllowSorting_Def = true;

        private const bool AutoDataBind_Def = true;

        private const string HideDeletedImageUrl_Def = "~/images/HideDel.gif";

        private const bool HideRestrictedColumns_Def = true;

        private const string HistoryUrl_Def = "~/History.aspx";

        private const int MaxRecords_Def = 0;

        private const bool RestrictByNullControls_Def = false;

        private const string ShowDeletedImageUrl_Def = "~/images/ShowDel.gif";

        private const bool ShowDeletedRecords_Def = false;

        private const string SqlWhere_Def = "";

        private const string TotalsCssClass_Def = "GridTotal";

        private readonly BusinessMethodCollection associatedMethods;

        private object[] beforeEditState;

        /// <summary>
        ///   Conexão aberta e fechada no GetDataRows().
        ///   É global para que o OnItemDataBound() possa aproveitá-la (GetDataRows() chama DataBind()).
        /// </summary>
        private TConnection bindingConnection;

        private bool changeFilterDenied;

        private ChangedEventHandler changedHandler;

        private bool dataEntry;

        private int deletedRowCount;

        private string[] dependees;

        private string hideDeletedImageUrl;

        private string historyUrl;

        private DataGridColumn[] insertInfoColumns;

        private bool insertInfoVisible;

        private object[] lastWhere;

        private bool settingState;

        private string showDeletedImageUrl;

        private string sqlWhere;

        private Number[] totals;

        private string totalsCssClass;

        private string transacao;

        /// <summary>
        ///   Indica se será feita consulta no banco de dados para preencher a grid no OnPreRender().
        /// </summary>
        private bool willQuery;

        public TDataGrid()
        {
            this.associatedMethods = new BusinessMethodCollection(this);

            // Inicialização das propriedades do DataGrid (em ordem alfabética)
            this.AllowPaging = false;
            this.AllowSorting = AllowSorting_Def;

            // Inicialização das propriedades do TDataGrid (em ordem alfabética)
            this.AutoDataBind = AutoDataBind_Def;
            this.DataEntry = false;

            

            this.SetDeletedRowCount(0);

            

            this.HideDeletedImageUrl = HideDeletedImageUrl_Def;
            this.HideRestrictedColumns = HideRestrictedColumns_Def;
            this.HistoryUrl = HistoryUrl_Def;

            #region InsertInfoVisible = false;

            this.SetInsertInfoVisible(false);

            #endregion

            this.MaxRecords = MaxRecords_Def;
            this.RestrictByNullControls = RestrictByNullControls_Def;

            #region RowCount = 0;

            this.SetRowCount(0);

            #endregion

            this.ShowDeletedImageUrl = ShowDeletedImageUrl_Def;
            this.ShowDeletedRecords = ShowDeletedRecords_Def;
            this.SQLWhere = SqlWhere_Def;
            this.TotalsCssClass = TotalsCssClass_Def;
            this.Transacao = string.Empty;
            this.WhereValues = new DbObject[0];

            this.thisManager.IsRoot = true;
            this.thisManager.StoreColumns = new string[0];
        }

        [
            Category("Techne"), 
        ]
        public event BulkProcessingEventHandler BulkProcessing;

        [
            Category("Techne"), 
        ]
        public event PostContainerOperationEventHandler PostContainerOperation;

        [
            Category("Techne"), 
        ]
        public event PostPutDataRowEventHandler PostRowDelete;

        [
            Category("Techne"), 
        ]
        public event PostPutDataRowEventHandler PostRowInsert;

        [
            Category("Techne"), 
        ]
        public event PostPutDataRowEventHandler PostRowUpdate;

        [
            Category("Techne"), 
        ]
        public event PrePutDataRowDelegate PreRowDelete;

        [
            Category("Techne"), 
        ]
        public event PrePutDataRowDelegate PreRowInsert;

        [
            Category("Techne"), 
        ]
        public event PrePutDataRowDelegate PreRowUpdate;

        /// <summary>
        ///   Permite especificar um DataSet, DataTable ou DataView como fonte de dados (DataSource) para a grid.
        ///   Caso um DataSet seja especificado, DataMember deve indicar o nome de um DataTable dele.
        /// </summary>
        [
            Category("Techne"), 
        ]
        public event EventHandler RefreshDataSource;

        [
            Category("Techne"), 
        ]
        public event SetSqlClausesEventHandler SetSqlClauses;

        [
            DefaultValue(AllowSorting_Def), 
        ]
        public override bool AllowSorting
        {
            get
            {
                return base.AllowSorting;
            }

            set
            {
                base.AllowSorting = value;
            }
        }

        [
            Category("Techne"), 
            Description(
                "Relação de métodos passíveis de serem executados pelo usuário através " +
                "de botões gerados no EditButtons associado a esta grid. " +
                "O método será executado uma vez para cada registro marcado."
                ), 
            DesignerSerializationVisibility(DesignerSerializationVisibility.Content), 
            PersistenceMode(PersistenceMode.InnerProperty), 
        ]
        public BusinessMethodCollection AssociatedMethods
        {
            get
            {
                return this.associatedMethods;
            }
        }

        // AutoDataBind é internal porque TSearchBase ainda utiliza esta propriedade.
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool AutoDataBind { get; set; }

        [
            Category("Techne"), 
            DefaultValue(false)
        ]
        public bool DataEntry
        {
            get
            {
                return this.thisManager.DataEntry;
            }

            set
            {
                this.dataEntry = value;
            }
        }

        [
            Category("Images"), 
            DefaultValue(HideDeletedImageUrl_Def), 
            Editor(typeof (ImageUrlEditor), typeof (UITypeEditor)), 
        ]
        public string HideDeletedImageUrl
        {
            get
            {
                return this.hideDeletedImageUrl;
            }

            set
            {
                this.hideDeletedImageUrl = value == null ? string.Empty : value.Trim();
            }
        }

        [Category("Techne"), DefaultValue(HideRestrictedColumns_Def), Description("Esconde as colunas que estejam sendo restringidas por um valor determinado."),]
        public bool HideRestrictedColumns { get; set; }

        [
            Category("Techne"), 
            DefaultValue(HistoryUrl_Def), 
            Description("Endereço da página que mostra o histórico de alteração de registros."), 
        ]
        public string HistoryUrl
        {
            get
            {
                return this.historyUrl;
            }

            set
            {
                this.historyUrl = value == null ? string.Empty : value;
            }
        }

        /// <summary>
        ///   Indica se as colunas contendo informações de insert em tabelas historificadas estão visíveis.
        /// </summary>
        [
            Browsable(false), 
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), 
        ]
        public bool InsertInfoVisible
        {
            get
            {
                return this.insertInfoVisible;
            }
        }

        [Category("Techne"), DefaultValue(MaxRecords_Def), Description("Número máximo de linhas lidas no DataSet quando autodatabind=true. Se for igual a zero, não há limite.")]
        public int MaxRecords { get; set; }

        [Category("Techne"), DefaultValue(RestrictByNullControls_Def), Description("Indica que os controles referenciados na propriedade SqlWhere devem restringir esta grid quando seus valores forem DBNull. " + "Se false, os controles cujo DBValue for DBNull serão ignorados e descartados da propriedade SqlWhere."),]
        public bool RestrictByNullControls { get; set; }

        [
            Browsable(false), 
            Category("Techne"), 
            Description("Indica o número de registros obtidos do banco de dados."), 
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), 
        ]
        public int RowCount
        {
            get
            {
                return (int)this.ViewState["RowCount"];
            }
        }

        [
            Category("Techne"), 
            Description("Where da cláusula SQL. Para fazer restrições usando controles do tipo ITControl, use o seu ID delimitado por sustenidos (ex: sis = #drpSistema#)"), 
            DefaultValue(SqlWhere_Def)
        ]
        public string SQLWhere
        {
            get
            {
                return this.sqlWhere;
            }

            set
            {
                var newValue = value == null ? string.Empty : value.Trim();
                this.sqlWhere = newValue;

                // Força o recálculo de Dependees
                this.dependees = null;
            }
        }

        [
            Category("Images"), 
            DefaultValue(ShowDeletedImageUrl_Def), 
            Editor(typeof (ImageUrlEditor), typeof (UITypeEditor)), 
        ]
        public string ShowDeletedImageUrl
        {
            get
            {
                return this.showDeletedImageUrl;
            }

            set
            {
                this.showDeletedImageUrl = value == null ? string.Empty : value.Trim();
            }
        }

        [Category("Techne"), DefaultValue(ShowDeletedRecords_Def), Description("Indica se a consulta realizada pela grid no banco de dados deve trazer os registros removidos em tabelas historificadas."),]
        public bool ShowDeletedRecords { get; set; }

        [
            Category("Techne - Styles"), 
            DefaultValue(TotalsCssClass_Def), 
            Description("Estilo utilizado pelos valores apresentados quando a propriedade ShowTotals=True."), 
        ]
        public string TotalsCssClass
        {
            get
            {
                return this.totalsCssClass;
            }

            set
            {
                this.totalsCssClass = value != null ? value : string.Empty;
            }
        }

        [
            Category("Techne"), 
            Description(
                "Nome da transação para fins de auditoria. " +
                "Todas as operações realizadas sob este manager serão auditadas sob a descrição informada nesta propriedade. " +
                "Caso não seja informada, será utilizada descrição default (contendo descrição do DataTable)."
                ), 
            DefaultValue(""), 
        ]
        public string Transacao
        {
            get
            {
                return this.transacao;
            }

            set
            {
                this.transacao = value == null ? string.Empty : value.Trim();
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DbObject[] WhereValues { get; set; }

        protected override bool WillCustomizeItems
        {
            get
            {
                return this.beforeEditState == null;
            }
        }

        protected override bool WillShowTitleRow
        {
            get
            {
                return base.WillShowTitleRow || this.DeletedRowCount > 0;
            }
        }

        ChangedEventHandler IDepender.ChangedHandler
        {
            get
            {
                if (this.changedHandler == null)
                {
                    this.changedHandler = new ChangedEventHandler(this.ControlChanged);
                }

                return this.changedHandler;
            }
        }

        bool IContainerManager.DataEntry
        {
            get
            {
                if (this.thisManager.IsRoot)
                {
                    return this.dataEntry;
                }
                else
                {
                    var root = TControl.GetRootManager(this);
                    return root.DataEntry;
                }
            }
        }

        IRecordContainer[] IContainerManagerInternal.DeleteContainers
        {
            get
            {
                return this.MarkedItems;
            }
        }

        private int DeletedRowCount
        {
            get
            {
                return this.deletedRowCount;
            }
        }

        private string[] Dependees
        {
            get
            {
                if (this.dependees == null)
                {
                    try
                    {
                        this.dependees = DependerLib.GetDependees(this);
                    }
                    catch (Exception exc)
                    {
                        throw new InvalidOperationException("Existe algum erro na propriedade " + this.UniqueID + ".SQLWhere (" + this.SQLWhere + ").", exc);
                    }
                }

                return this.dependees;
            }
        }

        string[] IDepender.Dependees
        {
            get
            {
                return this.Dependees;
            }
        }

        bool IContainerManager.IsEmpty
        {
            get
            {
                return ContainerManagerLib.IsEmpty(this);
            }
        }

        bool IContainerManager.IsRoot { get; set; }

        bool IContainerManagerInternal.ProcessUnchangedContainers
        {
            get
            {
                return false;
            }
        }

        string IDepender.SqlWhere
        {
            get
            {
                return this.SQLWhere;
            }
        }

        string[] IContainerManager.StoreColumns
        {
            get
            {
                return this.PersistColumns;
            }

            set
            {
                this.PersistColumns = value;
            }
        }

        TDataTable IContainerManager.Table
        {
            get
            {
                return this.Table;
            }
        }

        private IContainerManager thisManager
        {
            get
            {
                return this;
            }
        }

        public override void Clear()
        {
            base.Clear();
            this.lastWhere = null;
        }

        public void AddNew()
        {
            this.OnItemCommand(null);
        }

        /// <summary>
        ///   Indica se todos os containers (TGridItem's) deste controle estão no modo informado.
        /// </summary>
        public bool AllContainersInMode(RecordManagerMode mode)
        {
            return ContainerManagerLib.AllContainersInMode(this, mode);
        }

        public bool Commit()
        {
            return ContainerManagerLib.Commit(this);
        }

        public bool Delete()
        {
            if (!this.AutoDataBind)
            {
                if (this.DataSource == null)
                {
                    throw new InvalidOperationException("DataBind() deve ser chamado previamente quando AutoDataBind=false.");
                }
                else if (!(this.DataSource is TDataSet || this.DataSource is TDataTable))
                {
                    throw new InvalidOperationException("Delete() só é possível quando DataSource informa um tipo derivado de TDataSet.");
                }
            }

            return ContainerManagerLib.Delete(this);
        }

        public void EnterAddNewMode()
        {
            if (!this.AutoDataBind)
            {
                if (this.DataSource == null)
                {
                    throw new InvalidOperationException("DataBind() deve ser chamado previamente quando AutoDataBind=false.");
                }
                else if (!(this.DataSource is TDataSet || this.DataSource is TDataTable))
                {
                    throw new InvalidOperationException("EnterAddNewMode() só é possível quando DataSource informa um tipo derivado de TDataSet.");
                }
            }

            ContainerManagerLib.EnterAddNewMode(this);
        }

        public void EnterEditMode()
        {
            if (!this.AutoDataBind)
            {
                if (this.DataSource == null)
                {
                    throw new InvalidOperationException("DataBind() deve ser chamado previamente quando AutoDataBind=false.");
                }
                else if (!(this.DataSource is TDataSet || this.DataSource is TDataTable))
                {
                    throw new InvalidOperationException("EnterEditMode() só é possível quando DataSource informa um tipo derivado de TDataSet.");
                }
            }

            ContainerManagerLib.EnterEditMode(this);
        }

        public RetVal ExecAssociatedMethod(int methodIndex)
        {
            if (this.MarkedItems.Length == 0)
            {
                return "Selecione pelo menos um registro.";
            }

            var associatedMethod = this.AssociatedMethods[methodIndex];

            RetVal result;
            try
            {
                result = associatedMethod.Call(this.MarkedItems);
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException("Houve um problema na execução do método '" + associatedMethod.ExecuteMethod + "' em " + this.ID + ".AssociatedMethods.", exc);
            }

            if (result.Ok)
            {
                // Faz o refresh de todos os containers que dependam diretamente das tabelas alteradas.
                // Isso não funcionará em containers baseados em views que fazem referência a essas tabelas!!
                ContainerManagerLib.Refresh(associatedMethod, this.Page);
            }

            return result;
        }

        /// <summary>
        ///   Realiza busca no banco através do DataSet informado na propriedade DataSource, utilizando
        ///   a propriedade SQLWhere e disparando o evento SetSqlClauses. IMPORTANTE: o método DataBind()
        ///   é executado automaticamente por este método, não sendo necessária a sua chamada após o Get().
        /// </summary>
        public int Get()
        {
            if (!ContainerManagerLib.AllContainersInMode(this, RecordManagerMode.View))
            {
                throw new InvalidOperationException("Existe pelo menos um container fora do modo View. O método Get() exige que todos estejam em modo View.");
            }

            return this.GetDataRows(null);
        }

        /// <summary>
        ///   Disparará evento ProcessMarkedItem para cada um dos registros marcados.
        ///   Útil quando deseja-se realizar operações sobre os registros (que não sejam insert, update ou delete).
        /// </summary>
        /// <param name = "commandName">
        ///   String que será passada ao evento disparado que identificará unicamente a chamada.
        /// </param>
        /// <param name = "openConnection">
        ///   Indica se uma conexão será aberta para ser passada ao evento disparado.
        ///   A mesma conexão será passada a cada evento disparado, possibilitando a realização de operações transacionais.
        /// </param>
        /// <param name = "getIfSuccess">Chama o método Get() se for devolver true.</param>
        public bool Process(string commandName, bool openConnection, bool getIfSuccess)
        {
            bool processed;
            if (openConnection)
            {
                var tab = this.Table;
                if (tab == null)
                {
                    throw new InvalidOperationException();
                }

                var cn = tab.CreateWritableConnection(TControl.GetPermission((Control)this));
                cn.Open(true);
                try
                {
                    processed = this.Process(commandName, cn);
                    if (!processed)
                    {
                        cn.Rollback();
                    }
                }
                catch
                {
                    cn.Rollback();
                    throw;
                }
                finally
                {
                    if (cn.State == ConnectionState.Open)
                    {
                        cn.Close();
                    }
                }
            }
            else
            {
                processed = this.Process(commandName, null);
            }

            if (processed && getIfSuccess)
            {
                this.Get();
            }

            return processed;
        }

        /// <summary>
        ///   Dispara o evento BulkProcessing que contém um array dos DataRow's selecionados.
        /// </summary>
        public bool ProcessBulk(string commandName)
        {
            var ds = this.DataSource as TDataSet;
            if (ds == null)
            {
                throw new InvalidOperationException();
            }

            var table = this.MyTable;

            this.GetDataRows(this.lastWhere, false);

            DataRow[] rows;
            {
                var markedItems = this.MarkedItems;
                var len = markedItems.Length;
                rows = new DataRow[len];
                for (var i = 0; i < len; i++)
                {
                    rows[i] = table.Rows.Find(DbObject.ToObjectArray(markedItems[i].PrimaryKeyValues));
                }
            }

            this.OnBulkProcessing(new BulkProcessingEventArgs(table, rows));

            this.DataBind();
            this.willQuery = false;

            var error = this.MyTable.HasErrors;
            this.MyTable.Rows.Clear();
            return !error;
        }

        public void Rollback()
        {
            ContainerManagerLib.Rollback(this);
        }

        public void ToggleInsertInfo()
        {
            this.SetInsertInfoVisible(!this.InsertInfoVisible);

            if (this.InsertInfoVisible && this.insertInfoColumns == null)
            {
                this.InsertInfoColumnsAdd();
            }
            else if (!this.InsertInfoVisible && this.insertInfoColumns != null)
            {
                this.InsertInfoColumnsRemove();
            }

            this.willQuery = true;
        }

        public void TogglePaging()
        {
            this.AllowPaging = !this.AllowPaging;
            this.CurrentPageIndex = 0;

            this.willQuery = true;
        }

        public bool Undelete()
        {
            if (!this.AutoDataBind)
            {
                if (this.DataSource == null)
                {
                    throw new InvalidOperationException("DataBind() deve ser chamado previamente quando AutoDataBind=false.");
                }
                else if (!(this.DataSource is TDataSet || this.DataSource is TDataTable))
                {
                    throw new InvalidOperationException("Undelete() só é possível quando DataSource informa um tipo derivado de TDataSet.");
                }
            }

            return ContainerManagerLib.Undelete(this);
        }

        internal void LoadViewStateInternal(object savedState)
        {
            this.settingState = true;
            try
            {
                var state = (ArrayList)savedState;
                var offset = 0;

                

                this.SetInsertInfoVisible((bool)state[offset++]);
                if (this.InsertInfoVisible)
                {
                    this.InsertInfoColumnsAdd();
                }

                

                var modeNew = (bool)state[offset++];
                this.thisManager.IsRoot = (bool)state[offset++];
                this.lastWhere = (object[])state[offset++];
                this.beforeEditState = (object[])state[offset++];
                this.ShowDeletedRecords = (bool)state[offset++];

                base.LoadViewState(state[offset++]);

                // Após recuperar o estado da grid (as propriedades), refaz a query para recuperar os itens
                // e depois recupera os estado de cada um dos controles internos que for necessário.
                if (modeNew)
                {
                    this.thisManager.EnterAddNewMode();
                }
            }
            finally
            {
                this.settingState = false;
            }
        }

        internal object SaveViewStateInternal()
        {
            var list = new ArrayList();

            list.Add(this.InsertInfoVisible);
            list.Add(ContainerManagerLib.AllContainersInMode(this, RecordManagerMode.New));
            list.Add(this.thisManager.IsRoot);
            list.Add(this.lastWhere);
            list.Add(this.beforeEditState);
            list.Add(this.ShowDeletedRecords);

            list.Add(base.SaveViewState());

            return list;
        }

        protected virtual void OnBulkProcessing(BulkProcessingEventArgs args)
        {
            if (this.BulkProcessing != null)
            {
                this.BulkProcessing(this, args);
            }
        }

        protected virtual void OnPreRowDelete(PrePutDataRowArgs args)
        {
            if (this.PreRowDelete != null)
            {
                this.PreRowDelete(this, args);
            }
        }

        protected virtual void OnRefreshDataSource(EventArgs args)
        {
            if (this.RefreshDataSource != null)
            {
                this.RefreshDataSource(this, args);
            }
        }

        protected override void AddTitleLeftIcon(Control target)
        {
            base.AddTitleLeftIcon(target);

            if (this.ShowHistoryIcon)
            {
                var butHistory = new ImageButton();
                butHistory.ID = "butHistory";
                butHistory.ImageUrl = TUtil.TranslateRelativeUrl(this.HistoryImageUrl);
                butHistory.ToolTip = this.HistoryToolTip;
                butHistory.Click += this.HistoryImage_Click;
                target.Controls.Add(butHistory);

                var butShowDel = new ImageButton();
                butShowDel.ID = "butShowDel";
                butShowDel.ImageUrl = TUtil.TranslateRelativeUrl(this.ShowDeletedRecords ? this.HideDeletedImageUrl : this.ShowDeletedImageUrl);
                butShowDel.ToolTip = this.ShowDeletedRecords ? "Esconde registros removidos" : "Mostra registros removidos";
                butShowDel.Click += this.butHistory_Click;
                target.Controls.Add(butShowDel);
            }
        }

        protected override void LoadViewState(object savedState)
        {
            try
            {
                this.LoadViewStateInternal(savedState);
            }
            catch (Exception exc)
            {
                throw new ApplicationException(exc.Message, exc);
            }
            finally
            {
            }
        }

        protected override bool OnBubbleEvent(object sender, EventArgs args)
        {
            if (args is PostContainerOperationEventArgs)
            {
                this.OnPostContainerOperation((PostContainerOperationEventArgs)args);
                return false;
            }
            else if (args is PrePutDataRowArgs)
            {
                this.OnPrePutDataRow((PrePutDataRowArgs)args);
                return false;
            }
            else if (args is PostPutDataRowArgs)
            {
                this.OnPostPutDataRow((PostPutDataRowArgs)args);
                return false;
            }
            else
            {
                return base.OnBubbleEvent(sender, args);
            }
        }

        protected override void OnItemCreated(DataGridItemEventArgs e)
        {
            base.OnItemCreated(e);

            var item = e.Item;
            if (item.ItemType == ListItemType.Footer && this.totals != null)
            {
                for (int i = this.DebugMode ? 1 : 0, j = 0; i < item.Cells.Count; i++, j++)
                {
                    if (this.totals[j] != null)
                    {
                        var gridColumn = (TGridColumn)this.Columns[j];
                        var lbl = new Label();
                        lbl.Text = this.totals[j].ToString(gridColumn.Format);
                        lbl.CssClass = this.TotalsCssClass;
                        lbl.ToolTip = gridColumn.TotalToolTip;
                        item.Cells[i].Controls.Add(lbl);
                    }
                }
            }
        }

        protected override void OnItemDataBound(DataGridItemEventArgs e)
        {
            var container = e.Item as IRecordContainer;

            base.OnItemDataBound(e);

            if (!TControl.InDesignMode(this))
            {
                

                string historyUrl;
                {
                    var rowView = e.Item.DataItem as DataRowView;
                    var row = rowView == null ? null : rowView.Row as TDataRow;

                    var historyEnabled = row == null || container == null || this.bindingConnection == null
                                             ? false
                                             : container.Mode == RecordManagerMode.View &&
                                               this.thisManager.Table.HistoryEnabled &&
                                               row.Updated;

                    historyUrl = historyEnabled ? TUtil.TranslateRelativeUrl(this.HistoryUrl) + "?" + HistoryLib.GetHistoryQueryString(row)
                                     : string.Empty;
                }

                MarkColumn.SetHistoryUrl(this, e.Item, historyUrl);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // A verificação de AllContainersInMode(View) é realizada porque o método EnterEditMode()
            // ou EnterAddNewMode() pode já ter sido chamado, situação na qual o modo inicial deverá
            // ser ignorado, e willQuery deverá ser false.
            if (!this.Page.IsPostBack && ContainerManagerLib.AllContainersInMode(this, RecordManagerMode.View))
            {
                if (this.thisManager.IsRoot && !(this.Parent is TSearchBase))
                {
                    var initialMode = ContainerManagerLib.GetInitialMode(this);
                    var permission = TControl.GetPermission((IContainerManager)this);

                    if (initialMode == RecordManagerMode.New)
                    {
                        if (permission == null || !permission.ReadOnly)
                        {
                            this.thisManager.EnterAddNewMode();
                        }
                        else
                        {
                            this.willQuery = true;
                            this.lastWhere = null;
                            this.AddMessage(TPermission.DenialMessage, true);
                        }
                    }
                    else if (initialMode == RecordManagerMode.Edit)
                    {
                        if (permission == null || !permission.ReadOnly)
                        {
                            this.thisManager.EnterEditMode();
                        }
                        else
                        {
                            this.willQuery = true;
                            this.lastWhere = null;
                            this.AddMessage(TPermission.DenialMessage, true);
                        }
                    }
                    else if (this.AutoDataBind)
                    {
                        this.willQuery = true;
                        this.lastWhere = null;
                    }
                }
            }

            // Detecta alterações nos controles dos quais este controle depende
            DependerLib.RegisterDepender(this);
        }

        protected override void OnPageIndexChanged(DataGridPageChangedEventArgs e)
        {
            base.OnPageIndexChanged(e);

            // Tem que ser feito DEPOIS de setar CurrentPageIndex
            if (!this.AllowCustomPaging)
            {
                if (this.DataSource == null || this.DataSource is TDataSet || this.DataSource is TDataTable)
                {
                    this.willQuery = true;
                }
                else
                {
                    this.DataBind();
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            HistoryLib.RegisterHistoryScript(this.Page);

            // Teoricamente o evento PreRender deve limitar-se a alterar layout.
            // Assim, não permitiremos que os TControl's que estejam restringindo o SqlWhere da grid
            // tenham seus valores alterados neste evento, refletindo na query.
            // Se fizermos o Get() antes de disparar o PreRender, o RowCount estará disponível no PreRender.
            if (this.willQuery)
            {
                this.GetDataRows(this.lastWhere);
            }

            base.OnPreRender(e);

            // Decide qual ícone de histórico mostrar: o que é só imagem ou o que mostra insert info.
            {
// Precisa ser feito depois de chamar base.OnPreRender(), pois ele seta imgHistory.Visible.
                var imgHistory = this.GetTitleRowControl("imgHistory");
                var butHistory = this.GetTitleRowControl("butHistory");
                var butShowDel = this.GetTitleRowControl("butShowDel");

                var viewMode = ContainerManagerLib.AllContainersInMode(this, RecordManagerMode.View);

                if (butShowDel != null)
                {
                    butShowDel.Visible = viewMode &&
                                         imgHistory != null && imgHistory.Visible &&
                                         this.DeletedRowCount > 0;
                }

                if (imgHistory == null)
                {
                    // Se imgHistory não existe, então butHistory não deverá estar visível
                    if (butHistory != null)
                    {
                        butHistory.Visible = false;
                    }
                }
                else if (butHistory != null)
                {
                    // Se imgHistory está visível, coloca butHistory no seu lugar
                    butHistory.Visible = imgHistory.Visible && this.BoundRowCount > 0 && viewMode;
                    if (butHistory.Visible)
                    {
                        imgHistory.Visible = false;
                    }
                }
            }
        }

        protected override void OnSortCommand(DataGridSortCommandEventArgs e)
        {
            base.OnSortCommand(e);

            if (this.DataSource == null || this.DataSource is TDataSet || this.DataSource is TDataTable)
            {
                this.willQuery = true;
            }
            else
            {
                this.DataBind();
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.changeFilterDenied)
            {
                writer.WriteLine("<P><B>Alteração no filtro não é permitida no modo corrente.</B></P>");
            }

            base.Render(writer);
        }

        protected override void RenderDebugInfo(HtmlTextWriter writer)
        {
            base.RenderDebugInfo(writer);
            ContainerManagerLib.WriteDebugInfo(writer, this.thisManager);
            DependerLib.WriteDebugInfo(writer, this);
        }

        protected override object SaveViewState()
        {
            return this.SaveViewStateInternal();
        }

        protected void OnPostContainerOperation(PostContainerOperationEventArgs args)
        {
            if (this.PostContainerOperation != null)
            {
                this.PostContainerOperation(this, args);
            }

            this.RaiseBubbleEvent(this, args);
        }

        protected void OnPostPutDataRow(PostPutDataRowArgs args)
        {
            if (args.Operation == PutOperationEnum.Insert)
            {
                if (this.PostRowInsert != null)
                {
                    this.PostRowInsert(this, args);
                }
            }
            else if (args.Operation == PutOperationEnum.Update)
            {
                if (this.PostRowUpdate != null)
                {
                    this.PostRowUpdate(this, args);
                }
            }
            else if (args.Operation == PutOperationEnum.Delete)
            {
                if (this.PostRowDelete != null)
                {
                    this.PostRowDelete(this, args);
                }
            }
        }

        protected void OnPrePutDataRow(PrePutDataRowArgs args)
        {
            if (args.Operation == PutOperationEnum.Insert)
            {
                if (this.PreRowInsert != null)
                {
                    this.PreRowInsert(this, args);
                }
            }
            else if (args.Operation == PutOperationEnum.Update)
            {
                if (this.PreRowUpdate != null)
                {
                    this.PreRowUpdate(this, args);
                }
            }
            else if (args.Operation == PutOperationEnum.Delete)
            {
                if (this.PreRowDelete != null)
                {
                    this.PreRowDelete(this, args);
                }
            }
        }

        protected void OnSetSqlClauses(SetSqlClausesEventArgs args)
        {
            if (this.SetSqlClauses != null)
            {
                this.SetSqlClauses(this, args);
            }
        }

        void IContainerManagerInternal.AddMessage(string message, bool isError)
        {
            this.AddMessage(message, isError);
        }

        private void ControlChanged(object sender, ChangedEventArgs args)
        {
            if (!ContainerManagerLib.AllContainersInMode(this, RecordManagerMode.View))
            {
                // Ativa flag para ser tratado no Render()
                this.changeFilterDenied = true;
                return;
            }

            if (this.AutoDataBind)
            {
                // O novo conjunto de dados pode não completar o número de páginas necessárias
                // para manter a página corrente. Por isso, volta para a primeira página.
                this.CurrentPageIndex = 0;

                this.willQuery = true;
                this.lastWhere = null;
            }
            else
            {
                this.Clear();
            }
        }

        TConnection IContainerManager.CreateConnection()
        {
            return ContainerManagerLib.CreateConnection(this);
        }

        TConnectionWritable IContainerManagerInternal.CreateWritableConnection()
        {
            return ContainerManagerLib.CreateWritableConnection(this);
        }

        private void DoHideRestrictedColumns(string[] columnsInWhere, string[] fixedColumns)
        {
            var table = this.MyTable as TDataTable;

            if (!this.HideRestrictedColumns || table == null || this.DebugMode)
            {
                return;
            }

            foreach (var columnInWhere in columnsInWhere)
            {
                var dataColumn = table.FindColumn(columnInWhere);
                if (dataColumn != null)
                {
                    foreach (DataGridColumn gridColumn in this.Columns)
                    {
                        string bindField;
                        if (gridColumn is TemplateColumn)
                        {
                            bindField = string.Empty;
                        }
                        else
                        {
                            bindField = GetDataField(gridColumn);
                        }

                        if (string.Compare(bindField, dataColumn.ColumnName, true) == 0)
                        {
                            // Achou um DataGridColumn associado à coluna do Where
                            gridColumn.Visible = Array.IndexOf(fixedColumns, columnInWhere) < 0;
                        }
                    }
                }
            }
        }

        string IContainerManager.GetCaption(string fieldName)
        {
            return GetCaption(fieldName);
        }

        /// <summary>
        ///   Realiza select no banco de dados de acordo com a propriedade SQLWhere.
        ///   Não remove registros já existentes no DataTable.
        ///   Devolve a quantidade registros obtidos.
        /// </summary>
        private int GetDataRows(object[] stateWhere)
        {
            return this.GetDataRows(stateWhere, true);
        }

        private int GetDataRows(object[] stateWhere, bool clearRows)
        {
            try
            {
                var oldIndex = this.CurrentPageIndex;
                this.CurrentPageIndex = 0;
                this.DataBind();
                this.CurrentPageIndex = oldIndex;

                if (this.MyTable is TDataTable)
                {
                    var table = (TDataTable)this.MyTable;

                    // Se bindingConnection não for null, alguém a está utilizando ou é inconsistência do componente.
                    if (this.bindingConnection != null)
                    {
                        throw new InvalidOperationException();
                    }

                    // Não pode existir nenhum registro no DataTable
                    if (table.Rows.Count > 0)
                    {
                        throw new InvalidOperationException("Não devem existir registros no dataset na chamada a " + this.UniqueID + ".GetDataRows().");
                    }

                    // A conexão está sendo aberta aqui para que seja possível passar o Rdbms para o método GetWhere() abaixo.
                    this.bindingConnection = table.CreateConnection();

                    // * Este Get sempre trará todas as colunas da tabela.
                    // Daria para descobrir quais colunas trazer se fossem só BoundColumn e HyperlinkColumn's.
                    // O problema é com TemplateColumn. Não sei quais colunas ele utiliza...
                    string where;
                    string order;
                    DbObject[] whereValues;
                    string[] columnsInWhere, fixedColumns;

                    if (stateWhere == null)
                    {
                        where = this.SQLWhere;
                        order = string.Empty;
                        whereValues = this.WhereValues;

                        try
                        {
                            TControl.GetSqlWhere(table, TControl.GetRecordContainer(this), 
                                                 this.NamingContainer, this.bindingConnection.Rdbms, this.RestrictByNullControls, 
                                                 ref where, ref whereValues, out columnsInWhere, out fixedColumns);
                        }
                        catch (Exception exc)
                        {
                            throw new Exception("Possivelmente a propriedade " + this.ID + ".SQLWhere é inválida: " + this.SQLWhere, exc);
                        }
                        {
// Dispara OnSetSqlClauses()
                            var args = new SetSqlClausesEventArgs(where, whereValues, order);
                            this.OnSetSqlClauses(args);
                            where = args.Where;
                            whereValues = (DbObject[])ArrayList.Adapter(args.WhereValues).ToArray(typeof (DbObject));
                            order = args.Order;
                        }
                    }
                    else
                    {
                        where = (string)stateWhere[0];
                        order = (string)stateWhere[1];
                        whereValues = (DbObject[])stateWhere[2];
                        columnsInWhere = (string[])stateWhere[3];
                        fixedColumns = (string[])stateWhere[4];
                    }

                    this.lastWhere = new object[] { where, order, whereValues, columnsInWhere, fixedColumns };

                    // Esconde as colunas sendo restringidas
                    this.DoHideRestrictedColumns(columnsInWhere, fixedColumns);

                    var count = 0;
                    var countDel = 0;
                    this.bindingConnection.Open();
                    try
                    {
                        try
                        {
                            count = table.Get(
                                this.bindingConnection, null, where, whereValues, order, 
                                0, this.MaxRecords, // startRecord, maxRecords
                                true, table.HistoryEnabled // createPrimaryKey, getDeletedRecords
                                );
                        }
                        catch (Exception exc)
                        {
                            throw new Exception("Erro em " + this.UniqueID + " (" + table.TableName + ".Get(" + this.DataMember + ")). Where: " + (where == null || where == string.Empty ? "<vazio>" : where), exc);
                        }

                        this.totals = this.QueryTotals(table, this.ShowDeletedRecords, where, whereValues);

                        // Conta o número de registros deletados (TDataRow.Deleted)
                        var listRemove = new ArrayList();
                        foreach (TDataRow row in table.Rows)
                        {
                            if (row.Deleted)
                            {
                                listRemove.Add(row);
                            }
                        }

                        countDel = listRemove.Count;

                        if (table.HistoryEnabled && !this.ShowDeletedRecords)
                        {
                            // Remove os registros deletados do DataTable antes de fazer o DataBind()
                            foreach (TDataRow row in listRemove)
                            {
                                table.Rows.Remove(row);
                            }
                        }

                        // Se não conseguir manter a página corrente, volta para a primeira.
                        // Faz mais sentido do que ir para a última possível.
                        var maxPageIndex = (table.Rows.Count - 1) / this.PageSize;
                        if (this.CurrentPageIndex > maxPageIndex)
                        {
                            this.CurrentPageIndex = 0;
                        }

                        // O OnItemDataBound() vai precisar da conexão, por isso o DataBind()
                        // é chamado aqui, antes que ela seja fechada.
                        this.DataBind();
                    }
                    finally
                    {
                        if (this.bindingConnection.State == ConnectionState.Open)
                        {
                            this.bindingConnection.Close();
                        }

                        this.bindingConnection = null;
                    }

                    if (clearRows)
                    {
                        table.Rows.Clear();
                    }

                    this.SetRowCount(count);
                    this.SetDeletedRowCount(countDel);
                    this.willQuery = false;
                    return count;
                }
                else if (this.MyTable is QueryTable)
                {
                    var table = (QueryTable)this.MyTable;
                    var count = table.Select().Length;

                    this.totals = this.QueryTotals(table);

                    var maxPageIndex = (count - 1) / this.PageSize;
                    if (this.CurrentPageIndex > maxPageIndex)
                    {
                        this.CurrentPageIndex = 0;
                    }

                    this.DataBind();

                    this.SetRowCount(count);
                    return count;
                }

                    // Caso DataSource não seja derivado de TDataSet: dispara RefreshDataSource.
                else
                {
                    this.OnRefreshDataSource(EventArgs.Empty);

                    int count;
                    if (this.DataSource is DataView)
                    {
                        count = ((DataView)this.DataSource).Count;
                    }
                    else if (this.DataSource is DataTable)
                    {
                        count = ((DataTable)this.DataSource).Rows.Count;
                    }
                    else if (this.DataSource is DataSet)
                    {
                        var tab = ((DataSet)this.DataSource).Tables[this.DataMember];
                        if (tab == null)
                        {
                            throw new InvalidOperationException("A propriedade DataMember deve especificar um DataTable do DataSet informado na propriedade DataSource.");
                        }

                        count = tab.Rows.Count;
                    }
                    else
                    {
                        throw new InvalidOperationException("DataSource deve conter um DataSet, DataTable ou DataView.");
                    }

                    // Se não conseguir manter a página corrente, volta para a primeira.
                    // Faz mais sentido do que ir para a última possível.
                    var maxPageIndex = (count - 1) / this.PageSize;
                    if (this.CurrentPageIndex > maxPageIndex)
                    {
                        this.CurrentPageIndex = 0;
                    }

                    this.DataBind();

                    return count;
                }
            }
            finally
            {
            }
        }

        DataTable IContainerManager.GetDesignTimeDataTable()
        {
            return TControl.GetDesignTimeDataTable(this, this.DataMember);
        }

        string[] IContainerManager.GetFields()
        {
            var list = new ArrayList();

            var tab = this.Table;

            foreach (DataGridColumn column in this.Columns)
            {
                string columnName;

                if (column is TemplateColumn)
                {
                    var cell = new TableCell();
                    column.InitializeCell(cell, 0, ListItemType.Item);
                    foreach (Control child in cell.Controls)
                    {
                        if (child is ITControl)
                        {
                            columnName = ((ITControl)child).ColumnName.ToLower();
                            if (tab.ContainsColumn(columnName) && !list.Contains(columnName))
                            {
                                list.Add(columnName);
                            }
                        }
                    }
                }
                else
                {
                    columnName = GetDataField(column);
                    if (tab.ContainsColumn(columnName) && !list.Contains(columnName))
                    {
                        list.Add(columnName);
                    }
                }
            }

            return (string[])list.ToArray(typeof (string));
        }

        private object[] GetGridState()
        {
            var state = new object[7];

            state[0] = this.CurrentPageIndex;
            state[1] = this.AllowSorting;
            state[2] = this.SortExpression;
            state[3] = this.IsSortedAscending;
            state[4] = this.ShowFooter;
            state[5] = this.AllowPaging;
            state[6] = this.Width.ToString();

            return state;
        }

        string IContainerManager.GetReturnUrl()
        {
            return this.Page.Request["ReturnUrl"];
        }

        string IContainerManager.GetTitle()
        {
            return ContainerManagerLib.GetTitle(this);
        }

        private void HidePagers()
        {
            if (this.Controls.Count > 0 && this.Controls[0] is Table)
            {
                foreach (TableRow tableRow in ((Table)this.Controls[0]).Rows)
                {
                    if (tableRow is DataGridItem && ((DataGridItem)tableRow).ItemType == ListItemType.Pager)
                    {
                        tableRow.Visible = false;
                    }
                }
            }
        }

        private void HistoryImage_Click(object sender, ImageClickEventArgs args)
        {
            this.ToggleInsertInfo();
        }

        private void InsertInfoColumnsAdd()
        {
            var colUser = new BoundColumn();
            colUser.DataField = "Usuario Cad";
            colUser.HeaderText = "Cadastro (Usuário)";
            colUser.HeaderStyle.CssClass = "HistoryLabel";
            colUser.ItemStyle.CssClass = "HistoryInfo";
            this.Columns.Add(colUser);

            var colStamp = new BoundColumn();
            colStamp.DataField = "Stamp Cad";
            colStamp.HeaderText = "Cadastro (Data)";
            colStamp.DataFormatString = "{0:G}";
            colStamp.HeaderStyle.CssClass = "HistoryLabel";
            colStamp.ItemStyle.CssClass = "HistoryInfo";
            this.Columns.Add(colStamp);

            this.insertInfoColumns = new DataGridColumn[] { colUser, colStamp };
        }

        private void InsertInfoColumnsRemove()
        {
            foreach (var column in this.insertInfoColumns)
            {
                this.Columns.Remove(column);
            }

            this.insertInfoColumns = null;
        }

        void IContainerManagerInternal.PosCommit(bool commited, bool isRoot)
        {
            if (commited)
            {
                if (!this.DataEntry)
                {
                    if (this.beforeEditState != null)
                    {
                        this.SetGridState(this.beforeEditState);
                        this.beforeEditState = null;
                    }

                    // Precisa refazer a query tanto em AddNew como em Edit para que as alterações realizadas
                    // no banco por entry-points (ou mesmo a geração de valores de colunas identity) sejam mostradas.
                    this.willQuery = true;
                }
            }

            // Deve limpar o DataTable para que o GetDataRows() não reclame de registros existentes
            this.Table.Rows.Clear();
        }

        void IContainerManagerInternal.PosDelete(bool deleted)
        {
            // Recupera o estado anterior (se deletando em modo Edit)
            if (this.beforeEditState != null)
            {
                this.SetGridState(this.beforeEditState);
                this.beforeEditState = null;
            }

            if (deleted)
            {
                this.willQuery = true;
            }

            // Deve limpar o DataTable para que o GetDataRows() não reclame de registros existentes
            this.Table.Rows.Clear();
        }

        void IContainerManagerInternal.PosEnterAddNewMode()
        {
        }

        void IContainerManagerInternal.PosEnterEditMode()
        {
        }

        void IContainerManagerInternal.PosRollback(bool isRoot)
        {
            if (this.beforeEditState != null)
            {
                this.SetGridState(this.beforeEditState);
                this.beforeEditState = null;
            }

            this.willQuery = true; // Na verdade, só precisa setar qdo estiver fazendo rollback de AddNew (Edit não precisa)
        }

        void IContainerManagerInternal.PosUndelete(bool undeleted)
        {
            if (undeleted)
            {
                this.willQuery = true;
            }
        }

        void IContainerManagerInternal.PreCommit()
        {
            if (this.Table == null)
            {
                throw new InvalidOperationException("Propriedade " + this.ID + ".DataSource não informada.");
            }
        }

        void IContainerManagerInternal.PreDelete()
        {
        }

        void IContainerManagerInternal.PreEnterAddNewMode()
        {
            if (this.InsertInfoVisible)
            {
                this.ToggleInsertInfo();

                // O ToggleInsertInfo() chamado acima seta willQuery para true.
                // Isso não é necessário, pois estamos entrando em AddNew.
                this.willQuery = false;
            }

            // Se BeforeViewState já contém valor, se este método estiver sendo chamado de SetState
            // (devido a postback). Neste caso, o estado atual NÃO deve ser salvo, pois a grid já estava
            // em modo AddNew antes do postback.
            if (this.beforeEditState == null)
            {
                // O estado deve ser salvo antes de DataBind() para que CurrentPageIndex
                // possa ser setado para 0 (senão, se chamar AddNew fora da primeira página dá runtime error).

                
                this.beforeEditState = this.GetGridState();

                this.CurrentPageIndex = 0;
                this.AllowSorting = false;
                this.SortExpression = string.Empty;
                this.IsSortedAscending = false;
                this.ShowFooter = false;
                this.AllowPaging = false;
                this.Width = new Unit(100, UnitType.Percentage);

                
            }

            // Para obter MyTable
            this.DataBind();

            var tab = this.MyTable;

            

            var rows = new DataRow[this.PageSize];
            for (var i = 0; i < this.PageSize; i++)
            {
                rows[i] = tab.NewRow();
            }

            bool removeFromDs;
            if (tab.DataSet == null)
            {
                var ds = new DataSet();
                ds.Tables.Add(tab);
                removeFromDs = true;
            }
            else
            {
                removeFromDs = false;
            }

            // Desabilita constraints para poder inserir registro com campos null.
            tab.DataSet.EnforceConstraints = false;

            foreach (var row in rows)
            {
                tab.Rows.Add(row);
            }

            if (removeFromDs)
            {
                tab.DataSet.Tables.Remove(tab);
            }

            // Para adicionar as novas linhas à grid
            this.DataBind();

            foreach (var row in rows)
            {
                tab.Rows.Remove(row);
            }

            

            #region DoHideRestrictedColumns()

            var table = tab as TDataTable;
            if (table != null)
            {
                var where = this.SQLWhere;
                var whereValues = this.WhereValues;

                // Esta conexão é só para obter cn.Rdbms
                var cn = table.CreateConnection();

                // Precisa disparar OnSetSqlClauses()?

                // Obtém columnsInWhere e fixedColumns
                string[] columnsInWhere, fixedColumns;
                try
                {
                    TControl.GetSqlWhere(table, TControl.GetRecordContainer(this), 
                                         this.NamingContainer, cn.Rdbms, this.RestrictByNullControls, 
                                         ref where, ref whereValues, out columnsInWhere, out fixedColumns);
                }
                catch (Exception exc)
                {
                    throw new Exception("Possivelmente a propriedade " + this.ID + ".SQLWhere é inválida: " + this.SQLWhere, exc);
                }

                this.DoHideRestrictedColumns(columnsInWhere, fixedColumns);
            }

            #endregion
        }

        void IContainerManagerInternal.PreEnterEditMode()
        {
            // Entrada em edit mode não exige novo get e nem binding, pois os valores já estão nos TControl's.
            // O get só é necessário quando o insert info está sendo mostrado (no modo View) para remontar
            // a grid sem as informações de insert, ou quando a grid está entrando diretamente em modo Edit.
            if (this.InsertInfoVisible)
            {
                this.ToggleInsertInfo();
                this.GetDataRows(this.lastWhere);
            }

            // Desabilita todos os ícones de histórico em modo Edit
            var gridTable = this.Controls.Count == 0 ? null : this.Controls[0] as Table;
            if (gridTable != null)
            {
                foreach (TableRow tableRow in gridTable.Rows)
                {
                    if (tableRow is TGridItem)
                    {
                        MarkColumn.SetHistoryUrl(this, (TGridItem)tableRow, string.Empty);
                    }
                }
            }

            // A página pode estar entrando diretamente em Edit
            // O Get(), neste caso, tem que ser chamado antes de setar beforeEditState para que
            // WillCustomizeItems seja true (se for false a linha do Title não é mostrada).
            if (this.BoundRowCount < 0)
            {
                if (this.Get() == 0)
                {
                    return;
                }
            }

            if (this.beforeEditState == null)
            {
                

                this.beforeEditState = this.GetGridState();

                this.AllowSorting = false;
                this.SortExpression = string.Empty;
                this.IsSortedAscending = false;
                this.ShowFooter = false;
                this.Width = new Unit(100, UnitType.Percentage);

                
            }

            this.HidePagers();
        }

        void IContainerManagerInternal.PreRollback()
        {
        }

        void IContainerManagerInternal.PreUndelete()
        {
        }

        private Number[] QueryTotals(TDataTable table, bool includeDeleted, string where, DbObject[] whereValues)
        {
            var sumList = new ArrayList();
            var columnList = new ArrayList();
            for (var i = 0; i < this.Columns.Count; i++)
            {
                var column = this.Columns[i] as TGridColumn;
                if (column == null || !column.ShowTotal)
                {
                    continue;
                }

                columnList.Add(column);
                sumList.Add("SUM(" + column.ColumnName.ToLower() + ") AS " + column.ColumnName.ToLower());
            }

            if (sumList.Count == 0)
            {
                return null;
            }

            string strWhere;
            {
                

                if (where == null || where.Trim().Length == 0)
                {
                    strWhere = string.Empty;
                }
                else
                {
                    strWhere = where.Trim();
                }

                if (table.HistoryEnabled && !includeDeleted)
                {
                    if (strWhere.Length > 0)
                    {
                        strWhere = "(" + strWhere + ") AND ";
                    }

                    if (this.bindingConnection.Rdbms == Rdbms.SQLServer)
                    {
                        strWhere += "([Hist Status] IS NULL OR [Hist Status] <> 'R')";
                    }
                    else if (this.bindingConnection.Rdbms == Rdbms.Oracle)
                    {
                        strWhere += "(\"Hist Status\" IS NULL OR \"Hist Status\" <> 'R')";
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }

                if (strWhere.Length > 0)
                {
                    strWhere = " WHERE " + strWhere;
                }

                
            }

            var row = SimpleRow.QueryFirstRow(this.bindingConnection, 
                                              "SELECT " + StrLib.EnumerableToStr(sumList, ", ") + " " +
                                              "FROM " + table.MainTableName.ToLower() +
                                              strWhere, 
                                              whereValues
                );

            var totals = new Number[this.Columns.Count];
            for (var i = 0; i < this.Columns.Count; i++)
            {
                var index = columnList.IndexOf(this.Columns[i]);
                if (index >= 0)
                {
                    totals[i] = (Number)row[((TGridColumn)columnList[index]).ColumnName];
                }
                else
                {
                    totals[i] = null;
                }
            }

            return totals;
        }

        private Number[] QueryTotals(QueryTable table)
        {
            var totals = new Number[this.Columns.Count];
            var exists = false;

            for (var i = 0; i < this.Columns.Count; i++)
            {
                var column = this.Columns[i] as TGridColumn;
                if (column != null && column.ShowTotal)
                {
                    totals[i] = Convert.ToDecimal(table.Compute("SUM(" + column.ColumnName + ")", string.Empty));
                    exists = true;
                }
                else
                {
                    totals[i] = null;
                }
            }

            return exists ? totals : null;
        }

        void IContainerManager.Refresh()
        {
            this.willQuery = true;
        }

        /// <summary>
        ///   Dada uma lista de DataRow's, remove-os da tabela.
        /// </summary>
        private void RemoveRows(ICollection dataRows)
        {
            var tableRows = ((DataSet)this.DataSource).Tables[this.DataMember].Rows;

            foreach (DataRow row in dataRows)
            {
                // Este loop interno é necessário para que o Remove() não seja
                // chamado para um registro que não está no DataTable.
                foreach (DataRow tableRow in tableRows)
                {
                    if (tableRow == row)
                    {
                        tableRows.Remove(row);
                        break;
                    }
                }
            }
        }

        private void SetDeletedRowCount(int count)
        {
            this.deletedRowCount = count;
        }

        private void SetGridState(object[] state)
        {
            if (state == null)
            {
                throw new ArgumentNullException();
            }

            this.CurrentPageIndex = (int)state[0];
            this.AllowSorting = (bool)state[1];
            this.SortExpression = (string)state[2];
            this.IsSortedAscending = (bool)state[3];
            this.ShowFooter = (bool)state[4];
            this.AllowPaging = (bool)state[5];
            this.Width = Unit.Parse((string)state[6]);
        }

        private void SetInsertInfoVisible(bool visible)
        {
            if (!this.settingState)
            {
                if (visible)
                {
                    if (!this.thisManager.Table.HistoryEnabled)
                    {
                        throw new InvalidOperationException("Operação permitida somente em tabelas historificadas.");
                    }
                }
            }

            this.insertInfoVisible = visible;
        }

        private void SetRowCount(int rowCount)
        {
            this.ViewState["RowCount"] = rowCount;
        }

        private void butHistory_Click(object sender, ImageClickEventArgs e)
        {
            this.ShowDeletedRecords = !this.ShowDeletedRecords;
            this.willQuery = true;
        }
    }

    internal class BulkProcessingEventArgs : EventArgs
    {
        private readonly DataRow[] rows;

        private readonly DataTable table;

        public BulkProcessingEventArgs(DataTable table, DataRow[] rows)
        {
            this.table = table;
            this.rows = rows;
        }

        public DataRow[] Rows
        {
            get
            {
                return this.rows;
            }
        }

        public DataTable Table
        {
            get
            {
                return this.table;
            }
        }
    }
}