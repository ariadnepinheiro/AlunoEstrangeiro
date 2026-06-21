using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Techne.Controls.Design;
using Techne.Data;

namespace Techne.Controls
{
    [
        Designer(typeof (TDataGridBaseDesigner))
    ]
    internal class TDataGridBase : DataGrid
    {
        private const bool AutoGenerateColumns_Def = false;

        private const ControlMessageType ControlMessageType_Def = ControlMessageType.Icon;

        private const string DeletedItemCssClass_Def = "DeletedItem";

        private const bool EnableClientScript_Def = false;

        private const GridLines GridLines_Def = GridLines.None;

        private const string HistoryImageUrl_Def = "~/images/Historified.gif";

        private const string HistoryToolTip_Def = "As alterações são historificadas";

        private const string InvisibleMsg_Def = "[Nenhum registro]";

        private const bool InvisibleWhenEmpty_Def = true;

        private const string LinkDetailCommandName = "_LinkDetail_";

        private const string LinkDetailText_Def = "";

        private const string NoSortImageUrl_Def = ""; // "~/images/TDataGridNoSort.gif";

        private const int PageSize_Def = 12;

        private const string PagerCurrentPageFormat_Def = "[ {0} ]";

        private const string PagerOtherPageFormat_Def = "{0}";

        private const bool ShowHistoryIcon_Def = true;

        private const bool ShowScrollBar_Def = false;

        private const bool ShowTitleWhenEmpty_Def = false;

        private const string SortAscImageUrl_Def = "~/images/TDataGridSortAsc.gif";

        private const string SortDescImageUrl_Def = "~/images/TDataGridSortDesc.gif";

        private const string TitleCssClass_Def = "ManagerTitle";

        private const string TitleRowCssClass_Def = "ManagerTitleRow";

        private const string Title_Def = "";

        /// <summary>
        ///   Add's neste ArrayList devem ser realizados somente através do método AddMessage().
        /// </summary>
        private readonly ArrayList generalMessageList = new ArrayList();

        /// <summary>
        ///   NÃO ACESSAR DIRETAMENTE.
        ///   Utilizado para saber quantas vezes OnItemDataBound() é chamado para registros de dados
        ///   (não conta header, footer ou pager) para uma chamada de DataBind().
        /// </summary>
        private int boundCount = -1;

        private string deletedItemCssClass;

        private bool headerCreated;

        private string historyImageUrl;

        private string historyToolTip;

        private string invisibleMsg;

        private string linkDetailText;

        private TGridItem[] markedItems = new TGridItem[0];

        private string noSortImageUrl;

        private int pagerCount;

        private string pagerCurrentPageCssClass;

        private string pagerCurrentPageFormat;

        private string pagerOtherPageCssClass;

        private string pagerOtherPageFormat;

        private string[] persistColumns;

        private string[] persistedColumnNames;

        private TableRow rowHeader;

        private TableRow rowMessage;

        private TableRow rowTitle;

        private string sortAscImageUrl;

        private string sortDescImageUrl;

        private string title;

        private string titleCssClass;

        private string titleRowCssClass;

        public TDataGridBase()
        {
            // Inicialização das propriedades do DataGrid (em ordem alfabética)
            this.PagerStyle.HorizontalAlign = HorizontalAlign.Right;
            this.PagerStyle.Mode = PagerMode.NumericPages;

            // Inicialização das propriedades do TDataGridBase (em ordem alfabética)
            this.AutoGenerateColumns = AutoGenerateColumns_Def;
            this.BoundRowCount = -1;
            this.ControlMessageType = ControlMessageType_Def;
            this.DebugMode = false;
            this.DeletedItemCssClass = DeletedItemCssClass_Def;
            this.EnableClientScript = EnableClientScript_Def;
            this.GridLines = GridLines_Def;
            this.HistoryImageUrl = HistoryImageUrl_Def;
            this.HistoryToolTip = HistoryToolTip_Def;
            this.InvisibleMsg = InvisibleMsg_Def;
            this.InvisibleWhenEmpty = InvisibleWhenEmpty_Def;
            this.IsSortedAscending = true;
            this.LinkDetailText = LinkDetailText_Def;
            this.NoSortImageUrl = NoSortImageUrl_Def;
            this.PagerCurrentPageCssClass = string.Empty;
            this.PagerCurrentPageFormat = PagerCurrentPageFormat_Def;
            this.PagerOtherPageCssClass = string.Empty;
            this.PagerOtherPageFormat = PagerOtherPageFormat_Def;
            this.PageSize = PageSize_Def;
            this.PersistColumns = new string[0];
            this.PersistedColumnNames = null;
            this.ShowHistoryIcon = ShowHistoryIcon_Def;

            

            this.SetShowingGrid(false);

            

            this.ShowScrollBar = ShowScrollBar_Def;
            this.ShowTitleWhenEmpty = ShowTitleWhenEmpty_Def;
            this.SortAscImageUrl = SortAscImageUrl_Def;
            this.SortDescImageUrl = SortDescImageUrl_Def;
            this.SortExpression = string.Empty;
            this.Title = Title_Def;
            this.TitleCssClass = TitleCssClass_Def;
            this.TitleRowCssClass = TitleRowCssClass_Def;
        }

        public event TGridItemChangedEventHandler Changed;

        public event EventHandler LinkDetailClick;

        public event ProcessGridItemEventHandler ProcessingItem;

        [
            Category("Messages"), 
            DefaultValue(HistoryToolTip_Def), 
        ]
        public virtual string HistoryToolTip
        {
            get
            {
                return this.historyToolTip;
            }

            set
            {
                this.historyToolTip = value == null ? string.Empty : value.Trim();
            }
        }

        [
            DefaultValue(AutoGenerateColumns_Def)
        ]
        public override bool AutoGenerateColumns
        {
            get
            {
                return base.AutoGenerateColumns;
            }

            set
            {
                base.AutoGenerateColumns = value;
            }
        }

        [
            DefaultValue(GridLines_Def), 
        ]
        public override GridLines GridLines
        {
            get
            {
                return base.GridLines;
            }

            set
            {
                base.GridLines = value;
            }
        }

        [DefaultValue(PageSize_Def)]
        public override int PageSize
        {
            get
            {
                return base.PageSize;
            }

            set
            {
                base.PageSize = value;
            }
        }

        [Category("Techne - Validação"), DefaultValue(ControlMessageType_Def),]
        public ControlMessageType ControlMessageType { get; set; }

        [DefaultValue(false)]
        public bool DebugMode { get; set; }

        [
            Category("Techne - Styles"), 
            DefaultValue(DeletedItemCssClass_Def), 
            Description("Formatação utilizada para a apresentação de registros removidos em tabelas historificadas."), 
        ]
        public string DeletedItemCssClass
        {
            get
            {
                return this.deletedItemCssClass;
            }

            set
            {
                this.deletedItemCssClass = value == null ? string.Empty : value;
            }
        }

        [Browsable(false), Category("Techne - Validação"), DefaultValue(EnableClientScript_Def), Description("Tipo de mensagem de erro dos controles"),]
        public bool EnableClientScript { get; set; }

        [
            Category("Images"), 
            DefaultValue(HistoryImageUrl_Def), 
            Editor("System.Web.UI.Design.ImageUrlEditor", "System.Drawing.Design.UITypeEditor")
        ]
        public string HistoryImageUrl
        {
            get
            {
                return this.historyImageUrl;
            }

            set
            {
                this.historyImageUrl = value == null ? HistoryImageUrl_Def : value.Trim();
            }
        }

        [
            Category("Messages"), 
            DefaultValue(InvisibleMsg_Def), 
            Description("Mensagem a ser mostrada quando o TDataGrid fica invisível")
        ]
        public string InvisibleMsg
        {
            get
            {
                return this.invisibleMsg;
            }

            set
            {
                this.invisibleMsg = value == null ? string.Empty : value;
            }
        }

        [DefaultValue(InvisibleWhenEmpty_Def), Category("Techne"), Description("Faz com que o TDataGrid se torne invísível quando não existem registros")]
        public bool InvisibleWhenEmpty { get; set; }

        [
            Category("Techne"), 
            DefaultValue(true)
        ]
        public bool IsSortedAscending
        {
            get
            {
                return (bool)this.ViewState["IsSortedAscending"];
            }

            set
            {
                this.ViewState["IsSortedAscending"] = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(LinkDetailText_Def), 
            Description("Texto do link apresentado na linha de título da grid. " +
                        "O link somente é apresentado se esta propriedade tiver sido informada."), 
        ]
        public string LinkDetailText
        {
            get
            {
                return this.linkDetailText;
            }

            set
            {
                this.linkDetailText = value == null ? LinkDetailText_Def : value;
            }
        }

        /// <summary>
        ///   Devolve um array de TGridItem's que estão marcados (através da coluna MarkColumn).
        /// </summary>
        [
            Browsable(false), 
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), 
        ]
        public TGridItem[] MarkedItems
        {
            get
            {
                return this.markedItems;
            }
        }

        [
            DefaultValue(NoSortImageUrl_Def), 
            Editor(typeof (System.Web.UI.Design.ImageUrlEditor), typeof (System.Drawing.Design.UITypeEditor)), 
            Category("Images")
        ]
        public string NoSortImageUrl
        {
            get
            {
                return this.noSortImageUrl;
            }

            set
            {
                this.noSortImageUrl = value == null ? NoSortImageUrl_Def : value;
            }
        }

        [
            Category("Techne - Styles"), 
            DefaultValue("")
        ]
        public string PagerCurrentPageCssClass
        {
            get
            {
                return this.pagerCurrentPageCssClass;
            }

            set
            {
                this.pagerCurrentPageCssClass = value == null ? string.Empty : value;
            }
        }

        [
            Category("Paging"), 
            DefaultValue(PagerCurrentPageFormat_Def), 
            Description("Formato do número da página não selecionada")
        ]
        public string PagerCurrentPageFormat
        {
            get
            {
                return this.pagerCurrentPageFormat;
            }

            set
            {
                this.pagerCurrentPageFormat = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne - Styles"), 
            DefaultValue("")
        ]
        public string PagerOtherPageCssClass
        {
            get
            {
                return this.pagerOtherPageCssClass;
            }

            set
            {
                this.pagerOtherPageCssClass = value == null ? string.Empty : value;
            }
        }

        [
            Category("Paging"), 
            DefaultValue(PagerOtherPageFormat_Def), 
            Description("Formato do número da página selecionada")
        ]
        public string PagerOtherPageFormat
        {
            get
            {
                return this.pagerOtherPageFormat;
            }

            set
            {
                this.pagerOtherPageFormat = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(null), 
            Description("Nome das colunas separadas por vírgulas. Não é necessário informar as chaves primárias pois elas são persistidas automaticamente."), 
            TypeConverter(typeof (StringArrayConverter))
        ]
        public string[] PersistColumns
        {
            get
            {
                return this.persistColumns;
            }

            set
            {
                this.persistColumns = value == null ? new string[0] : value;
            }
        }

        [Category("Techne"), DefaultValue(ShowHistoryIcon_Def), Description("Indica se o ícone indicador de tabela historificada deve ser mostrado se for o caso."),]
        public bool ShowHistoryIcon { get; set; }

        [DefaultValue(ShowScrollBar_Def), Category("Paging")]
        public bool ShowScrollBar { get; set; }

        [Category("Techne"), DefaultValue(ShowTitleWhenEmpty_Def), Description("Indica se a linha de título deve ser mostrada quando não existem registros no controle. Veja também: Title."),]
        public bool ShowTitleWhenEmpty { get; set; }

        [
            DefaultValue(SortAscImageUrl_Def), 
            Editor(typeof (System.Web.UI.Design.ImageUrlEditor), typeof (System.Drawing.Design.UITypeEditor)), 
            Category("Images")
        ]
        public string SortAscImageUrl
        {
            get
            {
                return this.sortAscImageUrl;
            }

            set
            {
                this.sortAscImageUrl = value == null ? SortAscImageUrl_Def : value;
            }
        }

        [
            DefaultValue(SortDescImageUrl_Def), 
            Editor(typeof (System.Web.UI.Design.ImageUrlEditor), typeof (System.Drawing.Design.UITypeEditor)), 
            Category("Images")
        ]
        public string SortDescImageUrl
        {
            get
            {
                return this.sortDescImageUrl;
            }

            set
            {
                this.sortDescImageUrl = value == null ? SortDescImageUrl_Def : value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue("")
        ]
        public string SortExpression
        {
            get
            {
                return (string)this.ViewState["SortExpression"];
            }

            set
            {
                this.ViewState["SortExpression"] = value == null ? string.Empty : value;
            }
        }

        [
            DefaultValue(Title_Def), 
            Category("Techne"), 
            Description("Título do TDataGrid. Informe \"?\" (sem as aspas) para utilizar o nome do DataTable cadastrado no Cronos. " +
                        "Veja também: ShowTitleWhenEmpty.")
        ]
        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                this.title = value == null ? string.Empty : value;
            }
        }

        [
            DefaultValue(TitleCssClass_Def), 
            Category("Techne - Styles"), 
            Description("Cascade Style Sheet a ser empregado sobre o title do datagrid")
        ]
        public string TitleCssClass
        {
            get
            {
                return this.titleCssClass;
            }

            set
            {
                this.titleCssClass = value == null ? string.Empty : value;
            }
        }

        [
            DefaultValue(TitleRowCssClass_Def), 
            Category("Techne - Styles"), 
            Description("Estilo a ser aplicado na linha do título."), 
        ]
        public string TitleRowCssClass
        {
            get
            {
                return this.titleRowCssClass;
            }

            set
            {
                this.titleRowCssClass = value == null ? string.Empty : value;
            }
        }

        protected virtual bool WillCustomizeItems
        {
            get
            {
                return true;
            }
        }

        protected virtual bool WillShowTitleRow
        {
            get
            {
                return this.LinkDetailText.Length > 0 ||
                       this.Title.Length > 0 && (this.BoundRowCount > 0 || this.ShowTitleWhenEmpty);
            }
        }

        /// <summary>
        ///   Indica o número de registros "bindados" na grid.
        ///   Isso significa que, se a grid for paginada, indica o número de
        ///   registros na página, não o número obtido pelo select.
        ///   Se o DataBind() não ocorreu, devolve -1.
        /// </summary>
        protected int BoundRowCount
        {
            get
            {
                return (int)this.ViewState["BoundRowCount"];
            }

            set
            {
                this.ViewState["BoundRowCount"] = value;
            }
        }

        /// <summary>
        ///   Só devolve valor correto após a chamada do base.DataBind().
        /// </summary>
        protected DataTable MyTable
        {
            get
            {
                this.InitDataSource();

                if (this.DataSource is DataSet && this.DataMember != string.Empty && ((DataSet)this.DataSource).Tables.Contains(this.DataMember))
                {
                    return ((DataSet)this.DataSource).Tables[this.DataMember];
                }
                else if (this.DataSource is DataTable)
                {
                    return (DataTable)this.DataSource;
                }
                else if (this.DataSource is DataView && ((DataView)this.DataSource).Table != null)
                {
                    return ((DataView)this.DataSource).Table;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        ///   Devolve as colunas que deverão ser persistidas no ViewState.
        ///   O método DataBind() já deve ter sido chamado para que as propriedades DataSource e DataMember estejam válidas.
        /// </summary>
        protected string[] PersistedColumnNames
        {
            get
            {
                object names = this.persistedColumnNames;

                if (names == null)
                {
                    if (this.MyTable == null)
                    {
                        throw new InvalidOperationException("DataBind() ainda não foi chamado.");
                    }

                    var result = new ArrayList();

                    // Neste nível da hierarquia de classes, persiste as colunas primary key.
                    var columns = TechLib.EnumerableItemProperty(this.MyTable.PrimaryKey, "ColumnName");

                    result.AddRange(columns);
                    result.AddRange(this.PersistColumns);

                    names = result.ToArray(typeof (string));
                    this.persistedColumnNames = (string[])names;
                }

                return (string[])names;
            }

            set
            {
                this.persistedColumnNames = value;
            }
        }

        /// <summary>
        ///   Utilizado quando AutoDataBind é false para indicar que o Get() deve ser chamado a cada postback.
        /// </summary>
        protected bool ShowingGrid
        {
            get
            {
                return (bool)this.ViewState["ShowingGrid"];
            }
        }

        protected TDataTable Table
        {
            get
            {
                this.InitDataSource();

                if (this.DataSource is TDataTable)
                {
                    return (TDataTable)this.DataSource;
                }
                else if (this.DataSource is TDataSet)
                {
                    return (TDataTable)((TDataSet)this.DataSource).Tables[this.DataMember];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        ///   Remove os registros de uma grid, voltando ao estado antes do DataBind().
        /// </summary>
        public virtual void Clear()
        {
            this.Controls.Clear();
            this.CurrentPageIndex = 0;
            this.CreateControlHierarchy(true);
            this.BoundRowCount = -1;
            this.SetShowingGrid(false);
        }

        public override void DataBind()
        {
            this.pagerCount = 0;

            // BoundRowCount precisa assumir valor >= 0 antes de chamar base.DataBind() porque
            // o acesso à propriedade PersistedColumnNames feito em OnItemDataBound() acessa MyTable,
            // que por sua vez verifica se BoundRowCount é >= 0, dando exception caso contrário.
            this.BoundRowCount = 0;

            this.boundCount = 0;
            base.DataBind();
            this.BoundRowCount = this.boundCount;

            // Cria ou altera o tipo do DataSource dentro do ViewState
            this.ViewState.Add("DataSource", this.DataSource.GetType().FullName);

            // showingGrid deve setado aqui (não somente em Get()) porque a grid pode
            // estar mostrando dados que não são provenientes de TDataSet. Nessas
            // circunstâncias, o Get() (ao invés do DataBind() somente) causaria exception.
            this.SetShowingGrid(true);

            if (!TControl.InDesignMode(this))
            {
                if (this.SortExpression != string.Empty)
                {
                    

                    var sortExp = this.SortExpression + (this.IsSortedAscending ? string.Empty : " DESC");
                    var dataBind = true;

                    if (this.DataSource is DataSet)
                    {
                        var ds = (DataSet)this.DataSource;
                        ds.DefaultViewManager.DataViewSettings[this.DataMember].Sort = sortExp;
                    }
                    else if (this.DataSource is DataTable)
                    {
                        var dt = (DataTable)this.DataSource;
                        dt.DefaultView.Sort = sortExp;
                    }
                    else if (this.DataSource is DataView)
                    {
                        var dv = (DataView)this.DataSource;
                        dv.Sort = sortExp;
                    }
                    else
                    {
                        dataBind = false;
                    }

                    // Tem que chamar o DataBind() de novo...
                    if (dataBind)
                    {
                        base.DataBind();
                    }

                    
                }
            }
        }

        /// <summary>
        ///   Disparará evento ProcessMarkedItem para cada um dos registros marcados.
        ///   Útil quando deseja-se realizar operações sobre os registros (que não sejam insert, update ou delete).
        /// </summary>
        /// <param name = "commandName">
        ///   String que será passada ao evento disparado que identificará unicamente a chamada.
        /// </param>
        /// <param name = "connection">
        ///   Connection que será passada ao evento disparado.
        ///   Importante quando deseja-se realizar operações transacionais.
        ///   Deve-se informar null quando não for operações em banco não forem necessárias ou
        ///   não se desejar realizar operações transacionais.
        /// </param>
        public bool Process(string commandName, TConnectionWritable connection)
        {
            var error = false;

            foreach (var gridItem in this.MarkedItems)
            {
                var args = new ProcessGridItemEventArgs(gridItem, commandName, connection);
                this.OnProcessingItem(args);
                if (((IRecordContainerInternal)args.Item).HasErrors)
                {
                    error = true;
                }
            }

            return !error;
        }

        /// <summary>
        ///   Determina qual é a coluna da tabela à qual a coluna da grid está associada.
        ///   Atenção ao tipo TemplateColumn, tratado diferentemente.
        /// </summary>
        internal static string GetDataField(DataGridColumn column)
        {
            if (column is BoundColumn)
            {
                return ((BoundColumn)column).DataField;
            }
            else if (column is CheckImageColumn)
            {
                return ((CheckImageColumn)column).DataField;
            }
            else if (column is TemplateColumn)
            {
                return column.SortExpression;
            }
            else if (column is TGridColumn)
            {
                return ((TGridColumn)column).ColumnName;
            }
            else if (column is HyperLinkColumn)
            {
                return ((HyperLinkColumn)column).ColumnName;
            }
            else
            {
                return string.Empty;
            }
        }

        protected internal string GetCaption(DataGridColumn gridColumn)
        {
            if (gridColumn.HeaderText != "?")
            {
                return gridColumn.HeaderText;
            }
            else
            {
                var fieldName = GetDataField(gridColumn);
                if (fieldName.Length > 0)
                {
                    // A verificação de InDesignMode() aqui é para não precisar chamar a propriedade Table
                    // em design-time, que estava causando "Object reference not set to an instance of an object".
                    if (!TControl.InDesignMode(this) && this.Table != null)
                    {
                        return ((TDataColumn)this.Table.Columns[fieldName]).GetCaption(Thread.CurrentThread.CurrentCulture.Name);
                    }
                    else
                    {
                        // Este caso deve ocorrer em design-time.
                        return fieldName;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        protected virtual void AddTitleLeftIcon(Control target)
        {
            if (this.ShowHistoryIcon)
            {
                var imgHistory = new Image();

                imgHistory.ID = "imgHistory";
                imgHistory.ImageUrl = TUtil.TranslateRelativeUrl(this.HistoryImageUrl);
                imgHistory.ToolTip = this.HistoryToolTip;

                target.Controls.Add(imgHistory);
            }
        }

        protected virtual void OnLinkDetailClick(EventArgs args)
        {
            if (this.LinkDetailClick == null)
            {
                return;
            }

            this.LinkDetailClick(this, args);
        }

        protected virtual void OnProcessingItem(ProcessGridItemEventArgs args)
        {
            if (this.ProcessingItem != null)
            {
                this.ProcessingItem(this, args);
            }
        }

        protected virtual void RenderDebugInfo(HtmlTextWriter writer)
        {
            TControl.WriteDebugInfo(writer, this);
            writer.WriteLine("<B>Items.Count: </B>" + this.Items.Count + "<BR/>");
            writer.WriteLine("<B>BoundRowCount: </B>" + this.BoundRowCount + "<BR/>");
            writer.WriteLine("<B>MarkedItems.Length: </B>" + this.MarkedItems.Length + "<BR/>");
            writer.WriteLine("<B>ShowingGrid: </B>" + this.ShowingGrid + "<BR/>");
            writer.WriteLine("<B>CurrentPageIndex: </B>" + this.CurrentPageIndex + "<BR/>");
            writer.WriteLine("<B>SortExpression: </B>" + this.SortExpression + (this.SortExpression.Length == 0 || this.IsSortedAscending ? string.Empty : " DESC") + "<BR/>");
        }

        // Este flag é utilizado para que a distinção entre o pager superior do pager inferior no OnItemCreated()
        // seja possível. Ele indica se o header da grid já foi criado. Assim, se o header não tiver sido criado
        // e OnItemCreated() estiver criando um pager, ele só pode ser o superior. Da mesma forma, se o header
        // já tiver sido criado, o pager será o inferior.
        protected override void CreateControlHierarchy(bool useDataSource)
        {
            this.headerCreated = false;
            base.CreateControlHierarchy(useDataSource);
        }

        protected override DataGridItem CreateItem(int itemIndex, int dataSourceIndex, ListItemType itemType)
        {
            DataGridItem gridItem;

            if (TGridItem.ItemTypeIsData(itemType))
            {
                gridItem = new TGridItem(itemIndex, dataSourceIndex, itemType);
                ((TGridItem)gridItem).ControlMessageType = this.ControlMessageType;
            }
            else
            {
                gridItem = new DataGridItem(itemIndex, dataSourceIndex, itemType);
                gridItem.ID = "_grid" + itemType +
                              (itemType == ListItemType.Pager ? "_" + this.pagerCount++ : string.Empty);
            }

            return gridItem;
        }

        protected override void InitializeItem(DataGridItem item, DataGridColumn[] columns)
        {
            var cells = item.Cells;
            var itemType = item.ItemType;

            TDataTable table;
            if (this.DataSource is TDataSet && ((DataSet)this.DataSource).Tables.Contains(this.DataMember))
            {
                table = (TDataTable)((DataSet)this.DataSource).Tables[this.DataMember];
            }
            else if (this.DataSource is TDataTable)
            {
                table = (TDataTable)this.DataSource;
            }
            else
            {
                table = null;
            }

            if (this.DebugMode)
            {
                var debugCell = new TableCell();
                debugCell.ID = "_dbg";
                debugCell.Text = "Debug";
                cells.Add(debugCell);
            }

            for (var n = 0; n < columns.Length; n++)
            {
                TableCell newCell;
                if (columns[n] is MarkColumn)
                {
                    newCell = new MarkCell();
                }
                else if (columns[n] is TGridColumn)
                {
                    newCell = new TTableCell();
                }
                else
                {
                    newCell = new TableCell();
                }

                columns[n].InitializeCell(newCell, n, itemType);

                if (TGridItem.ItemTypeIsData(itemType) && columns[n] is TGridColumn)
                {
                    ((TTableCell)newCell).ColumnName = ((TGridColumn)columns[n]).ColumnName;
                }
                else if (itemType == ListItemType.Header)
                {
                    

                    if (columns[n].HeaderText == "?")
                    {
                        var columnName = GetDataField(columns[n]);
                        var columnText = string.Empty;
                        if (columnName.Length > 0 && table != null)
                        {
                            var column = (TDataColumn)table.Columns[columnName];
                            if (column == null)
                            {
                                throw new InvalidOperationException("A coluna " + columnName + " não foi encontrada no datatable " + table.TableName + ".");
                            }

                            columnText = column.GetCaption(Thread.CurrentThread.CurrentCulture.Name);
                        }

                        // Em último caso, usa a própria coluna para header text.
                        if (columnText.Length == 0)
                        {
                            columnText = StrLib.ToProper(columnName);
                        }

                        var linkButtons = (LinkButton[])TechLib.FindControls(typeof (LinkButton), newCell);
                        if (linkButtons.Length == 1)
                        {
                            linkButtons[0].Text = columnText;
                        }
                        else
                        {
                            newCell.Text = columnText;
                        }
                    }

                    
                }

                cells.Add(newCell);
            }
        }

        protected override bool OnBubbleEvent(object sender, EventArgs args)
        {
            if (args is TGridItemChangedEventArgs)
            {
                if (this.Changed != null)
                {
                    this.Changed(this, (TGridItemChangedEventArgs)args);
                }

                return true;
            }
            else
            {
                return base.OnBubbleEvent(sender, args);
            }
        }

        protected override void OnInit(EventArgs args)
        {
            base.OnInit(args);

            if (this.CssClass.ToLower() == "datagrid")
            {
                this.AlternatingItemStyle.CssClass = "GridAlternatingItem";
                this.EditItemStyle.CssClass = "GridEditItem";
                this.FooterStyle.CssClass = "GridFooter";
                this.HeaderStyle.CssClass = "GridHeader";
                this.ItemStyle.CssClass = "GridItem";
                this.PagerStyle.CssClass = "GridPager";
                this.SelectedItemStyle.CssClass = "GridSelectedItem";
            }
            else if (this.CssClass.ToLower() == "datagridbold")
            {
                this.AlternatingItemStyle.CssClass = "GridAlternatingItemBold";
                this.EditItemStyle.CssClass = "GridEditItemBold";
                this.FooterStyle.CssClass = "GridFooterBold";
                this.HeaderStyle.CssClass = "GridHeaderBold";
                this.ItemStyle.CssClass = "GridItemBold";
                this.PagerStyle.CssClass = "GridPagerBold";
                this.SelectedItemStyle.CssClass = "GridSelectedItemBold";
            }
            else if (this.CssClass.ToLower() == "datagridlow")
            {
                this.AlternatingItemStyle.CssClass = "GridAlternatingItemLow";
                this.EditItemStyle.CssClass = "GridEditItemLow";
                this.FooterStyle.CssClass = "GridFooterLow";
                this.HeaderStyle.CssClass = "GridHeaderLow";
                this.ItemStyle.CssClass = "GridItemLow";
                this.PagerStyle.CssClass = "GridPagerLow";
                this.SelectedItemStyle.CssClass = "GridSelectedItemLow";
            }
        }

        protected override void OnItemCommand(DataGridCommandEventArgs args)
        {
            if (args != null && args.CommandName == LinkDetailCommandName)
            {
                this.OnLinkDetailClick(EventArgs.Empty);
            }
            else
            {
                base.OnItemCommand(args);
            }
        }

        protected override void OnItemCreated(DataGridItemEventArgs e)
        {
            base.OnItemCreated(e);

            var gridItem = e.Item;
            var itemCells = gridItem.Cells;
            var itemType = gridItem.ItemType;

            if (this.WillCustomizeItems)
            {
                if (itemType == ListItemType.Header)
                {
                    

                    this.rowHeader = gridItem;
                    this.rowHeader.ID = this.ClientID + "_head";

                    for (var i = 0; i < this.Columns.Count; i++)
                    {
                        var cell = itemCells[this.DebugMode ? i + 1 : i];

                        if (this.AllowSorting)
                        {
                            // Nomeia cada link para sort para que o argumento do evento de sort
                            // possua o valor da coluna correta.
                            foreach (Control ctl in cell.Controls)
                            {
                                var but = ctl as LinkButton;
                                if (but != null && but.CommandName == "Sort")
                                {
                                    but.ID = "sort" + i;
                                    but.CssClass = "GridSortLink";
                                    but.ToolTip = "Clique para alterar a ordenação";
                                }
                            }

                            if (!(this.Columns[i] is MarkColumn) &&
                                this.Columns[i].HeaderText.Length > 0 &&
                                this.Columns[i].SortExpression.Length > 0)
                            {
                                #region cell.Controls.Add(<setinha>)

                                var img = new Image();

                                if (this.SortExpression == this.Columns[i].SortExpression)
                                {
                                    img.ImageUrl = this.IsSortedAscending ? this.SortAscImageUrl : this.SortDescImageUrl;
                                }
                                else
                                {
                                    img.ImageUrl = this.NoSortImageUrl;
                                }

                                img.ID = "SortIcon_" + i;
                                if (img.ImageUrl != null && img.ImageUrl.Trim() != string.Empty)
                                {
                                    cell.Controls.AddAt(0, new LiteralControl("&nbsp;"));
                                    cell.Controls.AddAt(0, img);
                                }

                                #endregion
                            }
                        }

                        // Adiciona checkbox no header das colunas MarkColumn
                        if (this.Columns[i] is MarkColumn && ((MarkColumn)this.Columns[i]).ShowCheckBox)
                        {
                            if (!this.Page.ClientScript.IsClientScriptBlockRegistered(typeof (TDataGridBase), "markAll"))
                            {
                                #region Page.RegisterClientScriptBlock("markAll", ...);

                                this.Page.ClientScript.RegisterClientScriptBlock(typeof (TDataGridBase), "markAll", 
                                                                                 "<SCRIPT LANGUAGE='javascript'>\r\n" +
                                                                                 "function markAll(gridName, coluna, checked) {\r\n" +
                                                                                 "  var linha;\r\n" +
                                                                                 "  for(linha = 2; ; linha++) {\r\n" +
                                                                                 "    var nome = gridName + '__ctl' + linha.toString() + '_mark' + coluna.toString() + '_chk';\r\n" +
                                                                                 "    var ctl = document.getElementById(nome);\r\n" +
                                                                                 "    if(ctl == null) break;\r\n" +
                                                                                 "\r\n" +
                                                                                 "    ctl.checked = checked;\r\n" +
                                                                                 "  }\r\n" +
                                                                                 "}\r\n" +
                                                                                 "</SCRIPT>\r\n"
                                    );
                            }

                            #endregion

                            var chk = new CheckBox();
                            chk.ID = "markAll";
                            chk.ToolTip = "Marca/desmarca todos os registros";
                            cell.Controls.Add(chk);
                            chk.Attributes.Add("onClick", "markAll('" + this.ClientID + "', " + i + ", " + chk.ClientID + ".checked);");
                        }
                    }

                    

                    // veja comentário sobre o flag headerCreated na declaração dele
                    this.headerCreated = true;
                }

                if (itemType == ListItemType.Pager)
                {
                    // É necessário batizar os pagers porque a hierarquia de controles eventualmente é criada duas
                    // vezes. Se eles não tiverem id's fixos, a segunda criação terá id diferente da primeira, o
                    // que causa problema no tratamento de eventos (o evento não é reconhecido porque o controle
                    // espera o id do primeira criação, mas recebe o da segunda).
                    gridItem.ID = this.headerCreated ? "pagerBottom" : "pagerTop";

                    

                    if (this.AllowPaging)
                    {
                        var pager = (TableCell)e.Item.Controls[0];

                        // Enumerates all the items in the pager...
                        for (var i = 0; i < pager.Controls.Count; i += 2)
                        {
                            // It can be either a Label or a Link button
                            if (pager.Controls[i] is LinkButton)
                            {
                                var h = (LinkButton)pager.Controls[i];
                                h.Text = string.Format(this.PagerOtherPageFormat, h.Text);
                                h.CssClass = this.PagerOtherPageCssClass;
                            }
                            else if (pager.Controls[i] is Label)
                            {
                                var l = (Label)pager.Controls[i];
                                l.Text = string.Format(this.PagerCurrentPageFormat, l.Text);
                                l.CssClass = this.PagerCurrentPageCssClass;
                            }
                            else
                            {
                                throw new NotImplementedException("TDataGridBase.OnItemCreated: " + pager.Controls[i].GetType().FullName);
                            }
                        }
                    }

                    
                }
            }

            // CreateTitleRow() deve ser chamado fora de WillCustomizeItems
            // para que ele também seja renderizado fora do modo View.
            if (itemType == ListItemType.Header)
            {
                this.rowMessage = this.CreateMessageRow();
                if (this.rowMessage != null)
                {
                    ((Table)this.Controls[0]).Rows.AddAt(0, this.rowMessage);
                }

                // A linha de título deve ser criada aqui para que o link detail exista no momento
                // de tratar os eventos de post back (especificamente neste caso o click do link detail).
                this.rowTitle = this.CreateTitleRow();
                if (this.rowTitle != null)
                {
                    ((Table)this.Controls[0]).Rows.AddAt(0, this.rowTitle);
                }
            }
        }

        protected override void OnItemDataBound(DataGridItemEventArgs e)
        {
            var gridItem = e.Item as TGridItem;

            if (gridItem != null)
            {
                this.boundCount++;

                if (!TControl.InDesignMode(this))
                {
                    DataRow row = null;
                    if (gridItem.DataItem is DataRowView)
                    {
                        row = ((DataRowView)gridItem.DataItem).Row;
                    }

                    if (row != null)
                    {
                        // Grava propriedades de validação nos controles
                        RecordContainerLib.SetClientValidationAttributes(gridItem, row.Table, false, this.ControlMessageType, this.EnableClientScript);
                        RecordContainerLib.SetControlsDataTypes(TControl.GetChildTControls(gridItem), row.Table);
                        RecordContainerLib.SetDataRow(gridItem, row);
                    }

                    RecordContainerLib.SetControlsMode(gridItem);

                    // Adiciona à ViewState as colunas a serem persistidas
                    if (row != null)
                    {
                        var persistedColumnNames = this.PersistedColumnNames;
                        var rowValues = new DbObject[persistedColumnNames.Length];
                        for (var i = 0; i < persistedColumnNames.Length; i++)
                        {
                            if (row.Table.Columns.Contains(persistedColumnNames[i]))
                            {
                                rowValues[i] = DbObject.ToDbObject(row[persistedColumnNames[i]]);
                            }
                            else if (!TControl.InDesignMode(this))
                            {
                                throw new InvalidOperationException("A coluna " + persistedColumnNames[i] + " informada na propriedade " + this.ID + ".PersistColumns não existe");
                            }
                        }

                        gridItem.SetRowValues(rowValues);
                    }
                }
            }

            // O OnItemDataBound() é chamado depois de setar todas as controles (colunas) da linha
            // para que os valores estejam disponíveis no tratamento do evento pelo programador.
            base.OnItemDataBound(e);
        }

        protected override void OnLoad(EventArgs args)
        {
            if (this.Page.IsPostBack)
            {
                this.BuildMarkedItemsCollection();
            }

            base.OnLoad(args);
        }

        protected override void OnPageIndexChanged(DataGridPageChangedEventArgs args)
        {
            if (!this.AllowCustomPaging)
            {
                this.CurrentPageIndex = args.NewPageIndex;
            }

            // Tem que ser chamado DEPOIS de setar CurrentPageIndex
            base.OnPageIndexChanged(args);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            var imgHistory = this.GetTitleRowControl("imgHistory");
            if (imgHistory != null)
            {
                imgHistory.Visible = this.Table != null && this.Table.HistoryEnabled;
            }

            if (this.BoundRowCount == 0)
            {
                this.AddMessage(this.InvisibleMsg, false);
            }

            if (this.rowMessage != null && this.generalMessageList.Count > 0)
            {
                var cell = this.rowMessage.FindControl("cell") as TableCell;
                if (cell == null)
                {
                    throw new ApplicationException();
                }

                foreach (Label lbl in this.generalMessageList)
                {
                    if (cell.Controls.Count > 0)
                    {
                        cell.Controls.Add(new LiteralControl("<br>"));
                    }

                    cell.Controls.Add(lbl);
                }
            }
        }

        protected override void OnSortCommand(DataGridSortCommandEventArgs args)
        {
            var oldSortExpression = this.SortExpression;
            var oldIsSortedAscending = this.IsSortedAscending;

            // Sets the new sorting field
            this.SortExpression = args.SortExpression;

            // Sets the order (defaults to ascending). If you click on the
            // sorted column, the order reverts.
            this.IsSortedAscending = true;
            if (args.SortExpression == oldSortExpression)
            {
                this.IsSortedAscending = !oldIsSortedAscending;
            }

            base.OnSortCommand(args);
        }

        protected override void PrepareControlHierarchy()
        {
            // A linha de título será removida antes de chamar PreparaControlHierarchy() porque
            // este método do DataGrid original assume que o pager superior, qdo existente, está na linha 0.
            // Ela será recriada após a execução do prepare.
            Table table = null;
            if (this.Controls.Count > 0)
            {
                table = (Table)this.Controls[0];
            }

            if (table != null && this.rowTitle != null)
            {
                table.Rows.Remove(this.rowTitle);
            }

            if (table != null && this.rowMessage != null)
            {
                table.Rows.Remove(this.rowMessage);
            }

            base.PrepareControlHierarchy();

            // Recria a linha de título removida anteriormente.
            if (table != null && this.rowMessage != null)
            {
                table.Rows.AddAt(0, this.rowMessage);
            }

            if (table != null && this.rowTitle != null)
            {
                table.Rows.AddAt(0, this.rowTitle);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.BoundRowCount >= 0)
            {
                // Ajusta a largura das colunas MarkColumn's.
                // Isto é feito aqui (e não no OnPreRender()) porque as mensagens são setadas
                // somente no TGridItem.OnPreRender(), que dispara DEPOIS de TDataGridBase.OnPreRender().
                // Neste ponto garante-se que todas as mensagens já foram setadas nos TGridItem's.
                foreach (DataGridColumn gridColumn in this.Columns)
                {
                    if (gridColumn is MarkColumn)
                    {
                        this.AdjustMarkColumnWidth((MarkColumn)gridColumn);
                    }
                }
            }

            if (!TControl.InDesignMode(this) && this.DebugMode)
            {
                writer.WriteLine("<TABLE BORDER=1 CELLSPACING=0 CELLPADDING=3><TR><TD>");
                writer.WriteLine("<P><FONT FACE=verdana SIZE=1 COLOR=gray>");
                this.RenderDebugInfo(writer);
                writer.WriteLine("</FONT></P>");
            }

            if (this.ShowingGrid)
            {
                this.RenderGrid(writer);
            }

            if (!TControl.InDesignMode(this) && this.DebugMode)
            {
                writer.WriteLine("</TD></TR></TABLE>");
            }
        }

        protected void AddMessage(string message, bool isError)
        {
            var lbl = new Label();
            lbl.EnableViewState = false;
            lbl.CssClass = isError ? "MsgError" : "MsgWarning";
            lbl.Text = message;
            this.generalMessageList.Add(lbl);
        }

        protected string GetCaption(string fieldName)
        {
            foreach (DataGridColumn gridCol in this.Columns)
            {
                if (string.Compare(GetDataField(gridCol), fieldName, true) == 0)
                {
                    return GetCaption(gridCol);
                }
            }

            return ((TDataColumn)this.Table.Columns[fieldName]).GetCaption(Thread.CurrentThread.CurrentCulture.Name);
        }

        protected Image GetTitleRowControl(string controlName)
        {
            return this.rowTitle == null ? null : this.rowTitle.FindControl(controlName) as Image;
        }

        /// <summary>
        ///   Ajusta a largura de cada uma das células da coluna informada para a largura da mais larga delas.
        /// </summary>
        /// <param name = "markColumn">Se null, nada faz.</param>
        private void AdjustMarkColumnWidth(MarkColumn markColumn)
        {
            if (markColumn == null)
            {
                return;
            }

            // Quando AutoDataBind é false, Controls.Count é 0
            if (this.Controls.Count == 0)
            {
                return;
            }

            // Determina o índice da coluna informada
            var index = -1;
            for (var i = 0; i < this.Columns.Count; i++)
            {
                if (this.Columns[i] == markColumn)
                {
                    index = i;
                    break;
                }
            }

            if (index < 0)
            {
                throw new ArgumentException("A coluna informada não foi encontrada em " + this.UniqueID + ".Columns.");
            }

            // Determina, entre as células da coluna, qual é a mais larga
            var maxWidth = int.MinValue;
            foreach (DataGridItem gridItem in ((Table)this.Controls[0]).Rows)
            {
                if (gridItem is TGridItem)
                {
                    var markCell = gridItem.Cells[index] as MarkCell;
                    if (markCell != null)
                    {
                        var cellWidth = markCell.GetWidth();
                        if (cellWidth > maxWidth)
                        {
                            maxWidth = cellWidth;
                        }
                    }
                }
            }

            // Seta a largura para todas as células da coluna
            if (maxWidth > int.MinValue)
            {
                foreach (DataGridItem gridItem in ((Table)this.Controls[0]).Rows)
                {
                    if (gridItem is TGridItem && index < gridItem.Cells.Count)
                    {
                        gridItem.Cells[index].Width = Unit.Pixel(maxWidth);
                    }
                }
            }
        }

        private void BuildMarkedItemsCollection()
        {
            if (this.BoundRowCount < 0)
            {
                return;
            }

            var markedItems = new ArrayList();

            foreach (TGridItem item in this.Items)
            {
                if (item.Marked)
                {
                    markedItems.Add(item);
                }
            }

            this.markedItems = (TGridItem[])markedItems.ToArray(typeof (TGridItem));
        }

        private TableRow CreateMessageRow()
        {
            if (this.Controls.Count == 0)
            {
                return null;
            }

            var table = this.Controls[0] as Table;
            if (table == null)
            {
                return null;
            }

            TableRow row = new DataGridItem(-1, -1, ListItemType.Item);
            row.Height = Unit.Pixel(0);

            var cell = new TableCell();
            cell.ID = "cell";
            cell.EnableViewState = false;
            row.Cells.Add(cell);

            if (table.Parent != null)
            {
                cell.ColumnSpan = ((DataGrid)table.Parent).Columns.Count + (this.DebugMode ? 1 : 0);
            }

            return row;
        }

        /// <summary>
        ///   Cria uma linha acima do header contendo o título (propriedade Title)
        ///   e o link (propriedade LinkDetailText).
        /// </summary>
        private TableRow CreateTitleRow()
        {
            if (this.Controls.Count == 0)
            {
                return null;
            }

            var table = this.Controls[0] as Table;
            if (table == null)
            {
                return null;
            }

            TableRow row = new DataGridItem(-1, -1, ListItemType.Item);
            row.ID = "_RowTitle";
            row.Height = Unit.Pixel(0);

            var cell = new TableCell();
            cell.ID = "c";
            cell.EnableViewState = false;
            row.Cells.Add(cell);

            if (table.Parent != null)
            {
                cell.ColumnSpan = ((DataGrid)table.Parent).Columns.Count + (this.DebugMode ? 1 : 0);
            }

            cell.Controls.Add(this.CreateTitleTable());

            return row;
        }

        private Table CreateTitleTable()
        {
            var cellTitulo = new TableCell();
            cellTitulo.ID = "ct";
            cellTitulo.Wrap = false;
            cellTitulo.Width = Unit.Percentage(100);

            if (!TControl.InDesignMode(this))
            {
                this.AddTitleLeftIcon(cellTitulo);

                if (cellTitulo.Controls.Count > 0)
                {
                    // Renderiza um espaço entre o ícone de histórico e o título
                    cellTitulo.Controls.Add(new LiteralControl(" "));
                }
            }

            var title = this.Title;
            if (title == "?" && this.Table != null)
            {
                title = this.Table.Name;
                if (title.Length != 0)
                {
                    // Se o nome foi cadastrado todo em minúsculas, usa ToProper().
                    if (StrLib.IsLower(title))
                    {
                        title = StrLib.ToProper(title);
                    }
                }
                else
                {
                    title = StrLib.ToProper(this.Table.TableName);
                }
            }

            var lbl = new Label();
            lbl.Text = title;
            lbl.CssClass = this.TitleCssClass;
            cellTitulo.Controls.Add(lbl);

            TableCell cellLink = null;
            if (this.LinkDetailText.Length > 0)
            {
                cellLink = new TableCell();
                cellLink.ID = "cl";
                cellLink.HorizontalAlign = HorizontalAlign.Right;
                cellLink.Wrap = false;

                var lnk = new LinkButton();
                lnk.ID = "lnk";
                lnk.Text = this.LinkDetailText;
                lnk.CommandName = LinkDetailCommandName;

                cellLink.Controls.Add(lnk);
            }

            var row = new TableRow();
            row.ID = "r";
            row.Cells.Add(cellTitulo);
            if (cellLink != null)
            {
                row.Cells.Add(cellLink);
            }

            var table = new Table();
            table.ID = "t";
            table.Width = Unit.Percentage(100);
            table.Height = Unit.Percentage(0);
            table.CssClass = this.TitleRowCssClass;
            table.Rows.Add(row);

            return table;
        }

        /// <summary>
        ///   Obtém DataSource sem fazer DataBind().
        ///   É necessário que o DataBind() tenha sido chamado pelo menos uma vez, mesmo que tenha sido antes do postback atual.
        /// </summary>
        private void InitDataSource()
        {
            if (this.DataSource != null)
            {
                return;
            }

            var className = this.ViewState["DataSource"] as string;
            if (className == null)
            {
                return;
            }

// className == "System.Data.DataTable": ocorre em controles MxNAddRemove
            if (className == typeof (DataTable).FullName)
            {
                return;
            }

            var type = TechLib.FindType(className, MainAssemblyAttribute.MainAssembly);

// type == QueryTable: ocorre em grids preenchidas por QueryTables
            if (type == typeof (QueryTable))
            {
                return;
            }

            this.DataSource = type.GetConstructor(Type.EmptyTypes).Invoke(null);
        }

        // TODO (03/04/03) Voltar o método TDataGridBase.RenderGrid() para dentro do Render()
        private void RenderGrid(HtmlTextWriter writer)
        {
            bool mostraScrollBar;
            var designMode = TControl.InDesignMode(this);

            this.PagerStyle.Visible = this.PageCount > 1;

            if (this.rowHeader != null)
            {
                this.rowHeader.Visible = designMode || this.BoundRowCount > 0;
            }

            if (this.rowTitle != null)
            {
                this.rowTitle.Visible = this.WillShowTitleRow;
            }

            if (this.rowMessage != null)
            {
                this.rowMessage.Visible = !designMode && this.generalMessageList.Count > 0;
            }

            

            if (!this.ShowScrollBar)
            {
                mostraScrollBar = false;
            }
            else if (HttpContext.Current == null)
            {
                mostraScrollBar = true;
            }
            else if (this.Page.Request.Browser.Browser == "IE" && this.Page.Request.Browser.MajorVersion > 3 ||
                     this.Page.Request.Browser.Browser == "Netscape" && this.Page.Request.Browser.MajorVersion > 4)
            {
                mostraScrollBar = true;
            }
            else
            {
                mostraScrollBar = false;
            }

            

            if (mostraScrollBar)
            {
                var div = new HtmlGenericControl("div");

                #region Coloca a grid dentro do div

                var origWidth = this.Width;
                var origHeight = this.Height;

                this.Width = Unit.Percentage(100);
                this.Height = Unit.Empty;

                div.Style.Add("width", origWidth.ToString());
                div.Style.Add("height", origHeight.ToString());

                foreach (var posStyle in new[] { "POSITION", "TOP", "LEFT", "Z-INDEX" })
                {
                    if (this.Style[posStyle] != null)
                    {
                        div.Style.Add(posStyle, this.Style[posStyle]);
                        this.Style.Remove(posStyle);
                    }
                }

                if (!this.BackColor.IsEmpty)
                {
                    div.Style.Add("background-color", this.BackColor.ToString());
                }

                div.Style.Add("border-color", this.BorderColor.IsEmpty ? System.Drawing.Color.Black.ToString()
                                                  : this.BorderColor.ToString());

                div.Style.Add("border-style", "solid");
                div.Style.Add("border-width", "1px;");
                div.Style.Add("overflow", "auto");

                // Coloca a grid dentro do div
                var gridWriter = new HtmlTextWriter(new StringWriter());
                base.Render(gridWriter);
                div.InnerHtml = gridWriter.InnerWriter.ToString();

                this.Width = origWidth;
                this.Height = origHeight;

                #endregion

                div.RenderControl(writer);
            }
            else
            {
                base.Render(writer);
            }
        }

        private void SetShowingGrid(bool showing)
        {
            this.ViewState["ShowingGrid"] = showing;
        }
    }
}