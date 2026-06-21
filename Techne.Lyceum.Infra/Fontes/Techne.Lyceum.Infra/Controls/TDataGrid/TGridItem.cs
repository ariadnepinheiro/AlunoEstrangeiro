using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Data;
using Techne.Library;

namespace Techne.Controls
{
    internal delegate void ProcessGridItemEventHandler(object sender, ProcessGridItemEventArgs args);

    internal delegate void TGridItemChangedEventHandler(object sender, TGridItemChangedEventArgs args);

    internal class TGridItem : DataGridItem, IRecordContainer, IRecordContainerInternal
    {
        private readonly ArrayList generalMessages = new ArrayList();

        private readonly ArrayList warningMessages = new ArrayList();

        private ControlMessageType pvControlMessageType = ControlMessageType.None;

        public TGridItem(int itemIndex, int dataSourceIndex, ListItemType itemType) : base(itemIndex, dataSourceIndex, itemType)
        {
            this.MarkControlEnabled = true;

            

            this.SetRowValues(new DbObject[0]);

            

            this.thisContainer.SetChanged(false);
            this.thisContainer.SetDeleted(false);
            this.thisContainer.SetHistInfo(string.Empty, DateTime.MinValue);
            this.thisContainer.SetMode(RecordManagerMode.View);
            this.thisContainer.SetPrimaryKeyValues(null);
        }

        public event PostContainerOperationEventHandler PostContainerOperation;

        public event PostPutDataRowEventHandler PostPutDataRow;

        public event PrePutDataRowDelegate PrePutDataRow;

        public ControlMessageType ControlMessageType
        {
            get
            {
                return this.pvControlMessageType;
            }

            set
            {
                this.pvControlMessageType = value;
            }
        }

        /// <summary>
        ///   True se o ListItem for Item, AlternatingItem, SelectedItem ou EditItem.
        /// </summary>
        public bool IsDataItem
        {
            get
            {
                return ItemTypeIsData(this.ItemType);
            }
        }

        public bool MarkControlEnabled
        {
            get
            {
                return (bool)this.ViewState["MarkControlEnabled"];
            }

            set
            {
                this.ViewState["MarkControlEnabled"] = value;
            }
        }

        public bool Marked
        {
            get
            {
                // Verifica se algum checkbox entre os MarkColumn's está checado
                foreach (TableCell cell in this.Cells)
                {
                    if (cell is MarkCell && ((MarkCell)cell).Marked)
                    {
                        return true;
                    }
                }

                return false;
            }

            set
            {
                foreach (TableCell cell in this.Cells)
                {
                    if (cell is MarkCell)
                    {
                        ((MarkCell)cell).Marked = value;
                    }
                }
            }
        }

        public DbObject[] PrimaryKeyValues
        {
            get
            {
                return this.thisContainer.PrimaryKeyValues;
            }
        }

        /// <summary>
        ///   Valores persistidos para esta linha da grid.
        /// </summary>
        public DbObject[] RowValues
        {
            get
            {
                return DbObject.ToDbObjectArray((object[])this.ViewState["RowValues"]);
            }
        }

        bool IRecordContainer.Changed
        {
            get
            {
                return (bool)this.ViewState["Changed"];
            }
        }

        private bool DebugMode
        {
            get
            {
                var grid = this.Grid;
                return grid == null ? false : grid.DebugMode;
            }
        }

        bool IRecordContainer.Deleted
        {
            get
            {
                return (bool)this.ViewState["Deleted"];
            }
        }

        private TDataGridBase Grid
        {
            get
            {
                return (TDataGridBase)TControl.FindContainer(this, typeof (TDataGridBase));
            }
        }

        bool IRecordContainerInternal.HasErrors
        {
            get
            {
                if (this.generalMessages.Count > 0)
                {
                    return true;
                }
                else
                {
                    var dataRowView = this.DataItem as DataRowView;
                    var row = dataRowView == null ? null : dataRowView.Row;
                    return row != null && row.HasErrors;
                }
            }
        }

        DateTime IRecordContainer.HistInsertStamp
        {
            get
            {
                return (DateTime)this.ViewState["HistInsertStamp"];
            }
        }

        string IRecordContainer.HistInsertUser
        {
            get
            {
                return (string)this.ViewState["HistInsertUser"];
            }
        }

        IContainerManager IRecordContainer.Manager
        {
            get
            {
                return this.Grid as IContainerManager;
            }
        }

        RecordManagerMode IRecordContainer.Mode
        {
            get
            {
                return (RecordManagerMode)this.ViewState["Mode"];
            }
        }

        DbObject[] IRecordContainer.PrimaryKeyValues
        {
            get
            {
                return DbObject.ToDbObjectArray((object[])this.ViewState["PrimaryKeyValues"]);
            }
        }

        DbObject[] IRecordContainer.StoredValues
        {
            get
            {
                return DbObject.ToDbObjectArray((object[])this.ViewState["StoredValues"]);
            }
        }

        private IRecordContainerInternal thisContainer
        {
            get
            {
                return this;
            }
        }

        public DbObject this[string columnName]
        {
            get
            {
                return RecordContainerLib.IndexerGet(this, columnName);
            }

            set
            {
                RecordContainerLib.IndexerSet(this, columnName, value);
            }
        }

        /// <summary>
        ///   True se o parâmetro informado for Item, AlternatingItem, SelectedItem ou EditItem.
        /// </summary>
        public static bool ItemTypeIsData(ListItemType itemType)
        {
            if (((itemType != ListItemType.Item) && (itemType != ListItemType.AlternatingItem)) && (itemType != ListItemType.EditItem))
            {
                return itemType == ListItemType.SelectedItem;
            }

            return true;
        }

        /// <summary>
        ///   Apresenta mensagens de erro associadas ou não a campos.
        ///   A operação que estiver sendo processada NÃO será desfeita.
        /// </summary>
        public void AddErrorMessage(ErrorList errors)
        {
            RecordContainerLib.DistributeErrorMessages(this, errors);
        }

        /// <summary>
        ///   Apresenta mensagem de erro.
        ///   A operação que estiver sendo processada NÃO será desfeita.
        /// </summary>
        /// <param name = "message">
        ///   Mensagem a ser apresentada.
        /// </param>
        public void AddErrorMessage(string message)
        {
            this.AddErrorMessage(string.Empty, message);
        }

        /// <summary>
        ///   Apresenta mensagem de erro.
        ///   A operação que estiver sendo processada NÃO será desfeita.
        /// </summary>
        /// <param name = "field">
        ///   Campo que contém o erro. A mensagem será apresentada próxima ao campo.
        ///   Se a mensagem não estiver associada a nenhum campo deve-se informar string.Empty.
        /// </param>
        /// <param name = "message">
        ///   Mensagem a ser apresentada.
        /// </param>
        public void AddErrorMessage(string field, string message)
        {
            if (message == null || field == null)
            {
                throw new ArgumentNullException();
            }

// TODO Melhorar esta implementação
            this.AddErrorMessage(new ErrorList(message + "|" + field));
        }

        /// <summary>
        ///   Apresenta mensagem de warning.
        /// </summary>
        public void AddWarningMessage(string message)
        {
            this.thisContainer.AddMessage(message, false);
        }

        /// <summary>
        ///   Obtém a célula correspondente a uma coluna, se a coluna for derivada de TGridColumn.
        /// </summary>
        public TTableCell GetCell(string columnName)
        {
            if (!this.IsDataItem)
            {
                throw new InvalidOperationException("Operação não permitida para itens do tipo " + this.ItemType + ".");
            }

            var lower = columnName.ToLower();

            foreach (TTableCell cell in this.Cells)
            {
                if (cell.ColumnName.ToLower() == lower)
                {
                    return cell;
                }
            }

            throw new ArgumentException("A coluna informada não foi encontrada na linha da grid.");
        }

        protected internal void SetRowValues(DbObject[] key)
        {
            if (key == null)
            {
                throw new ArgumentNullException();
            }

            this.ViewState["RowValues"] = DbObject.ToObjectArray(key);
        }

        protected override void LoadViewState(object savedState)
        {
            if (savedState == null)
            {
                return;
            }

            var state = (object[])savedState;
            var offset = 0;

            base.LoadViewState(state[offset++]);

            var containerState = (IList)state[offset++];
            if (containerState != null)
            {
                RecordContainerLib.SetState(this, containerState);
            }
        }

        protected override bool OnBubbleEvent(object sender, EventArgs args)
        {
            if (args is ChangedEventArgs)
            {
                this.RaiseBubbleEvent(this, new TGridItemChangedEventArgs(this, (ChangedEventArgs)args));
                return true;
            }
            else
            {
                return base.OnBubbleEvent(sender, args);
            }
        }

        protected override void OnLoad(EventArgs args)
        {
            base.OnLoad(args);
            foreach (var control in TControl.GetChildTControls(this))
            {
                if (control is ITControlEditable)
                {
                    ((ITControlEditable)control).Changed += this.control_Changed;
                }
            }
        }

        protected override void OnPreRender(EventArgs args)
        {
            base.OnPreRender(args);

            // Habilita o checkbox dos MarkColumn's se a propriedade MarkColumn.EnableCheckBox
            // tiver sido setada em design-time
            foreach (TableCell cell in this.Cells)
            {
                if (cell is MarkCell)
                {
                    ((MarkCell)cell).EnableMark = this.MarkControlEnabled;
                }
            }

            // Coloca as generalMessages no ícone correspondente dos MarkColumn's
            if (this.generalMessages.Count > 0)
            {
                var msg = StrLib.EnumerableToStr(this.generalMessages, "\r\n");

                if (!MarkColumn.SetMessage(this, msg, true))
                {
                    

                    var cellMsg = new MarkCell();
                    MarkColumn.InitializeCell(cellMsg, this.Cells.Count, this.ItemType, 
                                              MarkColumn.HistoryImageUrl_Def, MarkColumn.ErrorImageUrl_Def, MarkColumn.WarningImageUrl_Def, 
                                              false, false, true);
                    this.Cells.Add(cellMsg);

                    // Teoricamente não acontecerá, mas se não conseguiu setar a mensagem mesmo assim, causa exception.
                    if (!MarkColumn.SetMessage(this, msg, true))
                    {
                        throw new Exception(msg + " (MarkColumn.SetMessage() failure)");
                    }

                    
                }
            }

            // Coloca as warningMessages no ícone correspondente dos MarkColumn's
            if (this.warningMessages.Count > 0)
            {
                var msg = StrLib.EnumerableToStr(this.warningMessages, "\r\n");

                if (!MarkColumn.SetMessage(this, msg, false))
                {
                    

                    var cellMsg = new MarkCell();
                    MarkColumn.InitializeCell(cellMsg, this.Cells.Count, this.ItemType, 
                                              MarkColumn.HistoryImageUrl_Def, MarkColumn.ErrorImageUrl_Def, MarkColumn.WarningImageUrl_Def, 
                                              false, false, true);
                    this.Cells.Add(cellMsg);

                    // Teoricamente não acontecerá, mas se não conseguiu setar a mensagem mesmo assim, causa exception.
                    if (!MarkColumn.SetMessage(this, msg, false))
                    {
                        throw new Exception(msg + " (MarkColumn.SetMessage() failure)");
                    }

                    
                }
            }

            // Aplica o estilo deleted para os valores mostrados pela grid que não sejam através de TControl's
            if (this.thisContainer.Deleted)
            {
                foreach (TableCell cell in this.Cells)
                {
                    cell.ControlStyle.CssClass = this.Grid.DeletedItemCssClass;
                }
            }

            

            var cellDebug = this.FindControl("_dbg") as TableCell;
            if (cellDebug != null)
            {
                var swriter = new StringWriter();
                var writer = new HtmlTextWriter(swriter);

                writer.Write("<TABLE BORDER=1 CELLSPACING=0 CELLPADDING=3><TR><TD>");
                writer.Write("<FONT FACE=verdana SIZE=1 COLOR=gray>");
                TControl.WriteDebugInfo(writer, this);
                RecordContainerLib.WriteDebugInfo(writer, this.thisContainer);
                writer.Write("<B>Marked: </B>" + this.Marked + "<BR/>");
                writer.Write("</FONT>");
                writer.Write("</TD></TR></TABLE>");

                cellDebug.Text = swriter.ToString();

                writer.Close();
                swriter.Close();
            }

            
        }

        protected override object SaveViewState()
        {
            return new[]
                   {
                       base.SaveViewState(), 
                       RecordContainerLib.GetState(this)
                   };
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
            if (this.PostPutDataRow != null)
            {
                this.PostPutDataRow(this, args);
            }

            this.RaiseBubbleEvent(this, args);
        }

        protected void OnPrePutDataRow(PrePutDataRowArgs args)
        {
            if (this.PrePutDataRow != null)
            {
                this.PrePutDataRow(this, args);
            }

            this.RaiseBubbleEvent(this, args);
        }

        void IRecordContainerInternal.AddMessage(string message, bool isError)
        {
            if (isError)
            {
                this.generalMessages.Add(message);
            }
            else
            {
                this.warningMessages.Add(message);
            }
        }

        ITControl[] IRecordContainerInternal.GetControl(string fieldName)
        {
            throw new NotSupportedException();
        }

        void IRecordContainerInternal.OnPostContainerOperation(PostContainerOperationEventArgs args)
        {
            this.OnPostContainerOperation(args);
        }

        void IRecordContainerInternal.OnPostPutDataRow(PostPutDataRowArgs args)
        {
            this.OnPostPutDataRow(args);
        }

        void IRecordContainerInternal.OnPrePutDataRow(PrePutDataRowArgs args)
        {
            this.OnPrePutDataRow(args);
        }

        void IRecordContainerInternal.RegisterControl(ITControl control)
        {
        }

        void IRecordContainerInternal.SetChanged(bool changed)
        {
            this.ViewState["Changed"] = changed;
        }

        void IRecordContainerInternal.SetDeleted(bool deleted)
        {
            this.ViewState["Deleted"] = deleted;
        }

        void IRecordContainerInternal.SetHistInfo(string insertUser, DateTime insertStamp)
        {
            this.ViewState["HistInsertUser"] = insertUser == null ? string.Empty : insertUser;
            this.ViewState["HistInsertStamp"] = insertStamp;
        }

        void IRecordContainerInternal.SetMode(RecordManagerMode mode)
        {
            this.ViewState["Mode"] = mode;
            RecordContainerLib.SetMode(this);
        }

        void IRecordContainerInternal.SetMode(RecordManagerMode mode, ContainerManagerAction action)
        {
            var oldMode = this.thisContainer.Mode;
            this.ViewState["Mode"] = mode;
            RecordContainerLib.SetMode(this, oldMode, action);
        }

        void IRecordContainerInternal.SetPrimaryKeyValues(DbObject[] primaryKeyValues)
        {
            this.ViewState["PrimaryKeyValues"] = DbObject.ToObjectArray(primaryKeyValues);
        }

        void IRecordContainerInternal.SetStoredValues(DbObject[] values)
        {
            this.ViewState["StoredValues"] = DbObject.ToObjectArray(values);
        }

        private void control_Changed(object sender, ChangedEventArgs args)
        {
            this.thisContainer.SetChanged(true);
        }
    }

    internal class ProcessGridItemEventArgs
    {
        private readonly string commandName;

        private readonly TConnectionWritable connection;

        private readonly TGridItem gridItem;

        public ProcessGridItemEventArgs(TGridItem gridItem, string commandName, TConnectionWritable connection)
        {
            this.commandName = commandName;
            this.connection = connection;
            this.gridItem = gridItem;
        }

        /// <summary>
        ///   String identificador do processo passado como argumento no método Process().
        /// </summary>
        public string CommandName
        {
            get
            {
                return this.commandName;
            }
        }

        /// <summary>
        ///   Conexão passada como argumento do método Process() ou, se o parâmetro openConnection
        ///   do método Process() for true, a conexão aberta automaticamente.
        /// </summary>
        public TConnectionWritable Connection
        {
            get
            {
                return this.connection;
            }
        }

        public TGridItem Item
        {
            get
            {
                return this.gridItem;
            }
        }
    }

    internal class TGridItemChangedEventArgs : DataGridItemEventArgs
    {
        private readonly ITControlEditable changedControl;

        public TGridItemChangedEventArgs(DataGridItem gridItem, ChangedEventArgs originalArgs) : base(gridItem)
        {
            this.changedControl = originalArgs.ChangedControl;
        }

        public ITControlEditable ChangedControl
        {
            get
            {
                return this.changedControl;
            }
        }
    }
}