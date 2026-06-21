using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Techne.Controls.Design;
using Techne.Data;

namespace Techne.Controls
{
    [
        Designer(typeof (RecordManagerDesigner)), 
        ParseChildren(false), 
        PersistChildren(true)
    ]
    internal class RecordManager : WebControl, 
                                   IContainerManager, IContainerManagerInternal, 
                                   IRecordContainer, IRecordContainerInternal, 
                                   IDepender
    {
        // (em ordem alfabética)
        private const string AddNewDoneMsg_Def = "Cadastramento realizado";

        private const bool AutoDataBind_Def = true;

        private const ControlMessageType ControlMessageType_Def = ControlMessageType.Icon;

        private const string DeleteDoneMsg_Def = "Registro removido";

        private const string DeletedItemCssClass_Def = "DeletedItem";

        private const string EditDoneMsg_Def = "Alterações realizadas";

        private const string EmptyManagerMsg_Def = "[Nenhum registro]";

        private const bool EnableClientScript_Def = true;

        private const string HistoryImageUrl_Def = "~/images/Historified.gif";

        private const string HistoryToolTip_Def = "As alterações são historificadas";

        private const string HistoryUrl_Def = "~/History.aspx";

        private const string ModifiedImageUrl_Def = "~/images/History.gif";

        private const string ModifiedToolTip_Def = "O registro foi alterado";

        private const bool ShowDeletedRecords_Def = false;

        private const bool ShowHistoryIcon_Def = true;

        private const bool ShowMessageBox_Def = true;

        private const bool ShowTitleWhenEmpty_Def = false;

        private const string TitleCssClass_Def = "ManagerTitle";

        private const string TitleRowCssClass_Def = "ManagerTitleRow";

        private readonly BusinessMethodCollection associatedMethods;

        /// <summary>
        ///   Add's neste ArrayList devem ser realizados somente através do método AddMessage().
        /// </summary>
        private readonly ArrayList generalMessageList = new ArrayList();

        private readonly ArrayList managedControls = new ArrayList();

        private bool bindingDone;

        private bool changeFilterDenied;

        private ChangedEventHandler changedHandler;

        private string dataSourceClass = string.Empty;

        private bool debugMode;

        private object ds;

        private object dsAuto;

        private Image imgChanges;

        private ImageButton imgHistory;

        /// <summary>
        ///   [0]: where (string), [1]: whereValues (DbObject[]).
        /// </summary>
        private object[] lastWhere;

        private bool loaded;

        private RecordManagerMode modeBeforeCommit;

        private HtmlTableRow rowMessage;

        private HtmlTableRow rowTitle;

        /// <summary>
        ///   Exigido para implementação de IState. Não deve ser acessado diretamente.
        /// </summary>
        private bool settingState;

        private HtmlTable tabHeader;

        private TDataTable table;

        private string transacao;

        public RecordManager() : base(HtmlTextWriterTag.Div)
        {
            this.associatedMethods = new BusinessMethodCollection(this);

            this.AddNewDoneMsg = AddNewDoneMsg_Def;
            this.AutoDataBind = AutoDataBind_Def;
            this.ControlMessageType = ControlMessageType_Def;
            this.DataEntry = false;
            this.DataMember = string.Empty;
            this.DebugMode = false;
            this.DeletedItemCssClass = DeletedItemCssClass_Def;
            this.DeleteDoneMsg = DeleteDoneMsg_Def;
            this.EditDoneMsg = EditDoneMsg_Def;
            this.EmptyManagerMsg = EmptyManagerMsg_Def;
            this.EnableClientScript = EnableClientScript_Def;
            this.GeneralErrorLabel = string.Empty;
            this.HistoryImageUrl = HistoryImageUrl_Def;
            this.HistoryToolTip = HistoryToolTip_Def;
            this.HistoryUrl = HistoryUrl_Def;

            

            this.SetInsertInfoVisible(false);

            

            this.ModifiedImageUrl = ModifiedImageUrl_Def;
            this.ModifiedToolTip = ModifiedToolTip_Def;
            this.ShowDeletedRecords = ShowDeletedRecords_Def;
            this.ShowHistoryIcon = ShowHistoryIcon_Def;
            this.ShowMessageBox = ShowMessageBox_Def;
            this.ShowTitleWhenEmpty = ShowTitleWhenEmpty_Def;
            this.SqlWhere = string.Empty;
            this.SqlWhereValues = new DbObject[0];
            this.Title = string.Empty;
            this.TitleCssClass = TitleCssClass_Def;
            this.TitleRowCssClass = TitleRowCssClass_Def;
            this.Transacao = string.Empty;

            this.thisManager.IsRoot = true;
            this.thisManager.StoreColumns = new string[0];
            this.thisContainer.SetChanged(false);
            this.thisContainer.SetDeleted(false);
            this.thisContainer.SetHistInfo(string.Empty, DateTime.MinValue);
            this.thisContainer.SetMode(RecordManagerMode.View);
            this.thisContainer.SetPrimaryKeyValues(null);
            this.thisContainer.SetStoredValues(new DbObject[0]);

            this.bindingDone = false;
        }

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

        [
            Category("Techne - Messages"), 
            DefaultValue(AddNewDoneMsg_Def), 
            Description("Mensagem a ser exibida quando o manager cadastrar (insert) com sucesso.")
        ]
        public string AddNewDoneMsg
        {
            get
            {
                return (string)this.ViewState["AddNewDoneMsg"];
            }

            set
            {
                this.ViewState["AddNewDoneMsg"] = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne"), 
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

        [
            Category("Techne"), 
            DefaultValue(AutoDataBind_Def)
        ]
        public bool AutoDataBind
        {
            get
            {
                return (bool)this.ViewState["AutoDataBind"];
            }

            set
            {
                this.ViewState["AutoDataBind"] = value;
            }
        }

        [Browsable(false), Category("Techne - Validação"), DefaultValue(ControlMessageType_Def), Description("Tipo de mensagem de erro dos controles"),]
        public ControlMessageType ControlMessageType { get; set; }

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
                this.ViewState["DataEntry"] = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(""), 
            TypeConverter(typeof (DataMemberConverter)), 
            Bindable(true)
        ]
        public string DataMember
        {
            get
            {
                return (string)this.ViewState["DataMember"];
            }

            set
            {
                this.ViewState["DataMember"] = value == null ? string.Empty : value.Trim();
            }
        }

        [
            Category("Techne"), 
            DefaultValue(null)
        ]
        public object DataSource
        {
            get
            {
                if (this.ds == null && this.dataSourceClass.Trim().Length > 0 && !this.DesignMode)
                {
                    if (this.dsAuto == null)
                    {
                        try
                        {
                            this.dsAuto = TUtil.CreateInstance(this.dataSourceClass);
                        }
                        catch
                        {
                        }
                    }

                    this.ds = this.dsAuto;
                }

                return this.ds;
            }

            set
            {
                if (value == null)
                {
                    this.ds = null;
                    return;
                }

                if (!(value is TDataSet || value is TDataTable))
                {
                    throw new ArgumentException("Os únicos tipos válidos para DataSource são Techne.Library.Data.TDataSet e Techne.Library.Data.TDataTable.");
                }

                this.ds = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(""), 
            Editor(typeof (TDataSourceClassEditor), typeof (System.Drawing.Design.UITypeEditor)), 
            Bindable(true)
        ]
        public string DataSourceClass
        {
            get
            {
                return this.dataSourceClass;
            }

            set
            {
                this.dataSourceClass = value == null ? string.Empty : value;
            }
        }

        [
            DefaultValue(false)
        ]
        public bool DebugMode
        {
            get
            {
                return this.debugMode;
            }

            set
            {
                this.debugMode = value;
            }
        }

        [
            Category("Techne - Messages"), 
            DefaultValue(DeleteDoneMsg_Def), 
            Description("Mensagem a ser exibida quando o manager remover (delete) com sucesso.")
        ]
        public string DeleteDoneMsg
        {
            get
            {
                return (string)this.ViewState["DeleteDoneMsg"];
            }

            set
            {
                this.ViewState["DeleteDoneMsg"] = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne - Styles"), 
            DefaultValue(DeletedItemCssClass_Def), 
            Description("Formatação utilizada para a apresentação de registros removidos em tabelas historificadas."), 
        ]
        public string DeletedItemCssClass
        {
            get
            {
                return (string)this.ViewState["DeletedItemCssClass"];
            }

            set
            {
                this.ViewState["DeletedItemCssClass"] = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne - Messages"), 
            DefaultValue(EditDoneMsg_Def), 
            Description("Mensagem a ser exibida quando o manager atualizar (update) com sucesso.")
        ]
        public string EditDoneMsg
        {
            get
            {
                return (string)this.ViewState["EditDoneMsg"];
            }

            set
            {
                this.ViewState["EditDoneMsg"] = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne - Messages"), 
            DefaultValue(EmptyManagerMsg_Def), 
            Description("Mensagem a ser exibida quando o manager não apresenta nenhum registro")
        ]
        public string EmptyManagerMsg
        {
            get
            {
                return (string)this.ViewState["EmptyManagerMsg"];
            }

            set
            {
                this.ViewState["EmptyManagerMsg"] = value == null ? string.Empty : value;
            }
        }

        [Browsable(false), Category("Techne - Validação"), DefaultValue(EnableClientScript_Def), Description("Tipo de mensagem de erro dos controles"),]
        public bool EnableClientScript { get; set; }

        [
            Category("Techne"), 
            DefaultValue(""), 
            Description("Controle (System.Web.UI.WebControls.Label) que receberá as mensagens de erro genéricas."), 
            TypeConverter(typeof (LabelConverter))
        ]
        public string GeneralErrorLabel
        {
            get
            {
                return (string)this.ViewState["GeneralErrorLabel"];
            }

            set
            {
                this.ViewState["GeneralErrorLabel"] = value == null ? string.Empty : value;
            }
        }

        [
            Category("Image"), 
            DefaultValue(HistoryImageUrl_Def), 
            Editor("System.Web.UI.Design.ImageUrlEditor", "System.Drawing.Design.UITypeEditor")
        ]
        public string HistoryImageUrl
        {
            get
            {
                return (string)this.ViewState["HistoryImageUrl"];
            }

            set
            {
                this.ViewState["HistoryImageUrl"] = value == null ? HistoryImageUrl_Def : value.Trim();
            }
        }

        [
            Category("Techne - Messages"), 
            DefaultValue(HistoryToolTip_Def), 
        ]
        public string HistoryToolTip
        {
            get
            {
                return (string)this.ViewState["HistoryToolTip"];
            }

            set
            {
                this.ViewState["HistoryToolTip"] = value == null ? string.Empty : value.Trim();
            }
        }

        [
            Category("Techne"), 
            DefaultValue(HistoryUrl_Def), 
            Description("Endereço da página que mostra o histórico de alteração de registros."), 
        ]
        public string HistoryUrl
        {
            get
            {
                return (string)this.ViewState["HistoryUrl"];
            }

            set
            {
                this.ViewState["HistoryUrl"] = value == null ? string.Empty : value;
            }
        }

        /// <summary>
        ///   Indica se as informações de insert em tabelas historificadas estão visíveis.
        /// </summary>
        [
            Browsable(false), 
        ]
        public bool InsertInfoVisible
        {
            get
            {
                return (bool)this.ViewState["InsertInfoVisible"];
            }
        }

        [
            Browsable(false), 
            Description(
                "Obtém o modo corrente do manager. " +
                "Para alterá-lo, utilize os métodos EnterEditMode(), EnterAddNewMode(), Delete(), CommitRecord() e Rollback()."
                )
        ]
        public RecordManagerMode Mode
        {
            get
            {
                return this.thisContainer.Mode;
            }
        }

        [
            Category("Image"), 
            DefaultValue(ModifiedImageUrl_Def), 
            Editor("System.Web.UI.Design.ImageUrlEditor", "System.Drawing.Design.UITypeEditor")
        ]
        public string ModifiedImageUrl
        {
            get
            {
                return (string)this.ViewState["ModifiedImageUrl"];
            }

            set
            {
                this.ViewState["ModifiedImageUrl"] = value == null ? ModifiedImageUrl_Def : value.Trim();
            }
        }

        [
            Category("Techne - Messages"), 
            DefaultValue(ModifiedToolTip_Def), 
        ]
        public string ModifiedToolTip
        {
            get
            {
                return (string)this.ViewState["ModifiedToolTip"];
            }

            set
            {
                this.ViewState["ModifiedToolTip"] = value == null ? string.Empty : value.Trim();
            }
        }

        // TODO Esta propriedade deve ser cached
        [Browsable(false)]
        public IContainerManager ParentManager
        {
            get
            {
                return TControl.GetManager(this);
            }
        }

        /// <summary>
        ///   Devolve os valores das colunas da primary key da tabela ao qual o controle está associado.
        ///   Caso nenhum registro esteja sendo apresentado (query não retornou nenhum registro, ou registro
        ///   foi removido), será devolvido um array DbObject[] sem nenhum elemento (.Length == 0).
        /// </summary>
        [Browsable(false)]
        public DbObject[] PrimaryKeyValues
        {
            get
            {
                return this.thisContainer.PrimaryKeyValues;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(ShowDeletedRecords_Def), 
            Description("Indica se o controle exibirá registros removidos de tabelas historificadas."), 
        ]
        public bool ShowDeletedRecords
        {
            get
            {
                return (bool)this.ViewState["ShowDeletedRecords"];
            }

            set
            {
                this.ViewState["ShowDeletedRecords"] = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(ShowHistoryIcon_Def), 
            Description("Indica se o ícone indicador de tabela historificada deve ser mostrado se for o caso."), 
        ]
        public bool ShowHistoryIcon
        {
            get
            {
                return (bool)this.ViewState["ShowHistoryIcon"];
            }

            set
            {
                this.ViewState["ShowHistoryIcon"] = value;
            }
        }

        [Browsable(true), Category("Techne - Validação"), DefaultValue(ShowMessageBox_Def), Description("Mostra message box se houver erro na validação de campos feita pelos scripts do browser."),]
        public bool ShowMessageBox { get; set; }

        [
            Category("Techne"), 
            DefaultValue(ShowTitleWhenEmpty_Def), 
            Description("Indica se a linha de título deve ser mostrada quando não há registro corrente no controle."), 
        ]
        public bool ShowTitleWhenEmpty
        {
            get
            {
                return (bool)this.ViewState["ShowTitleWhenEmpty"];
            }

            set
            {
                this.ViewState["ShowTitleWhenEmpty"] = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(""), 
            Description("Permite especificar o registro a ser editado sem usar variáveis de contexto.")
        ]
        public string SqlWhere
        {
            get
            {
                return (string)this.ViewState["SqlWhere"];
            }

            set
            {
                this.ViewState["SqlWhere"] = value == null ? string.Empty : value.Trim();

                // Força o recálculo de Dependees
                this.ViewState["Dependees"] = null;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DbObject[] SqlWhereValues
        {
            get
            {
                return DbObject.ToDbObjectArray((object[])this.ViewState["SqlWhereValues"]);
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                this.ViewState["SqlWhereValues"] = DbObject.ToObjectArray(value);
            }
        }

        [
            Browsable(false), 
            Description(
                "Seta a tabela que será utilizada pelo manager. Normalmente esta propriedade é utilizada somente " +
                "quando o manager é criado em run-time. Para setar a tabela em design-time, recomenda-se utilizar " +
                "o RecordManager, setando as propriedades DataSource e DataMember."
                ), 
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public TDataTable Table
        {
            get
            {
                return this.thisManager.Table;
            }

            set
            {
                this.table = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(""), 
            Description("Título do RecordManager renderizado sobre ele. Informe \"?\" (sem as aspas) para utilizar o nome cadastrado no Cronos."), 
        ]
        public string Title
        {
            get
            {
                return (string)this.ViewState["Title"];
            }

            set
            {
                this.ViewState["Title"] = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne - Styles"), 
            DefaultValue(TitleCssClass_Def), 
            Description("Stylesheet a ser empregado sobre o title do RecordManager")
        ]
        public string TitleCssClass
        {
            get
            {
                return (string)this.ViewState["TitleCssClass"];
            }

            set
            {
                this.ViewState["TitleCssClass"] = value == null ? string.Empty : value;
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
                return (string)this.ViewState["TitleRowCssClass"];
            }

            set
            {
                this.ViewState["TitleRowCssClass"] = value == null ? string.Empty : value;
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

        bool IRecordContainer.Changed
        {
            get
            {
                return (bool)this.ViewState["Changed"];
            }
        }

        ChangedEventHandler IDepender.ChangedHandler
        {
            get
            {
                if (this.changedHandler == null)
                {
                    this.changedHandler = new ChangedEventHandler(this.controlChanged);
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
                    return (bool)this.ViewState["DataEntry"];
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
                return new IRecordContainer[] { this };
            }
        }

        bool IRecordContainer.Deleted
        {
            get
            {
                return (bool)this.ViewState["Deleted"];
            }
        }

        private string[] Dependees
        {
            get
            {
                if (this.ViewState["Dependees"] == null)
                {
                    try
                    {
                        this.ViewState["Dependees"] = DependerLib.GetDependees(this);
                    }
                    catch (Exception exc)
                    {
                        throw new InvalidOperationException("Existe algum erro na propriedade " + this.UniqueID + ".SqlWhere.", exc);
                    }
                }

                return (string[])this.ViewState["Dependees"];
            }
        }

        string[] IDepender.Dependees
        {
            get
            {
                return this.Dependees;
            }
        }

        bool IRecordContainerInternal.HasErrors
        {
            get
            {
                return this.generalMessageList.Count > 0;
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

        private bool IsEmpty
        {
            get
            {
                return this.thisManager.IsEmpty;
            }
        }

        bool IContainerManager.IsEmpty
        {
            get
            {
                return ContainerManagerLib.IsEmpty(this);
            }
        }

        bool IContainerManager.IsRoot
        {
            get
            {
                return (bool)this.ViewState["IsRoot"];
            }

            set
            {
                this.ViewState["IsRoot"] = value;
            }
        }

        IContainerManager IRecordContainer.Manager
        {
            get
            {
                return this;
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

        bool IContainerManagerInternal.ProcessUnchangedContainers
        {
            get
            {
                return true;
            }
        }

        int IContainerManager.RowCount
        {
            get
            {
                return this.PrimaryKeyValues == null ? 0 : 1;
            }
        }

        string[] IContainerManager.StoreColumns
        {
            get
            {
                return (string[])this.ViewState["StoreColumns"];
            }

            set
            {
                this.ViewState["StoreColumns"] = value == null ? new string[0] : value;
            }
        }

        DbObject[] IRecordContainer.StoredValues
        {
            get
            {
                return DbObject.ToDbObjectArray((object[])this.ViewState["StoredValues"]);
            }
        }

        TDataTable IContainerManager.Table
        {
            get
            {
                if (!this.bindingDone)
                {
                    this.DataBind();
                }

                return this.table;
            }
        }

        private IRecordContainerInternal thisContainer
        {
            get
            {
                return this;
            }
        }

        private IContainerManager thisManager
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

        public override void DataBind()
        {
            // O DataBind() original (Control.DataBind()) chama, além do OnDataBinding(),
            // o DataBind() de cada um dos controles internos.
            this.OnDataBinding(EventArgs.Empty);

            if (this.DataSource != null)
            {
                if (this.DataSource is TDataSet && this.DataMember.Length > 0)
                {
                    this.table = (TDataTable)((TDataSet)this.DataSource).Tables[this.DataMember];
                }
                else if (this.DataSource is TDataTable)
                {
                    this.table = (TDataTable)this.DataSource;
                }
                else
                {
                    this.table = null;
                }

                if (this.table == null)
                {
                    throw new InvalidOperationException("A tabela definida pelas propriedades " + this.ID + ".DataSource e DataMember não foi encontrada.");
                }
            }
            else if (this.table == null && TControl.GetChildManagers(this).Length == 0)
            {
                throw new InvalidOperationException("A propriedade " + this.ID + ".DataSource e/ou DataMember e/ou Table não foi informada.");
            }

            this.bindingDone = true;
        }

        /// <summary>
        ///   Remove do manager o registro corrente sem removê-lo do banco.
        /// </summary>
        public void Clear()
        {
            if (this.Mode != RecordManagerMode.View)
            {
                throw new InvalidOperationException("O método " + this.UniqueID + ".Clear() não é permitido no modo " + this.Mode);
            }

            this.thisContainer.SetMode(RecordManagerMode.Edit);
            RecordContainerLib.SetDataRow(this, null);
            this.thisContainer.SetMode(RecordManagerMode.View);

            this.lastWhere = null;
        }

        public bool Commit()
        {
            if (!this.bindingDone)
            {
                this.DataBind();
            }

            return ContainerManagerLib.Commit(this);
        }

        public bool Delete()
        {
            if (!this.bindingDone)
            {
                this.DataBind();
            }

            return ContainerManagerLib.Delete(this);
        }

        /// <summary>
        ///   Coloca o manager em modo de inserção.
        ///   Deve ser utilizado a partir do evento RecordManager.Load, mas não antes disso.
        /// </summary>
        public void EnterAddNewMode()
        {
            if (!this.bindingDone)
            {
                this.DataBind();
            }

            ContainerManagerLib.EnterAddNewMode(this);
        }

        public void EnterEditMode()
        {
            if (!this.bindingDone)
            {
                this.DataBind();
            }

            ContainerManagerLib.EnterEditMode(this);
        }

        public RetVal ExecAssociatedMethod(int methodIndex)
        {
            var associatedMethod = this.AssociatedMethods[methodIndex];

            RetVal result;
            try
            {
                result = associatedMethod.Call(this);
            }
            catch (Exception exc)
            {
                throw new InvalidOperationException("Existe um problema com o método '" + associatedMethod.ExecuteMethod + "' em " + this.ID + ".AssociatedMethods: " + exc.Message);
            }

            if (result.Ok)
            {
                ContainerManagerLib.Refresh(associatedMethod, this.Page);
            }

            // Não chama AddMessage() se deu sucesso sem mensagem.
            if (!result.Ok || result.Message.Length > 0)
            {
                this.thisContainer.AddMessage(result.Message, !result.Ok);
            }

            return result;
        }

        /// <summary>
        ///   Repopula o RecordManager baseado na dependência corrente (controles e contexto).
        /// </summary>
        public void Requery()
        {
            if (this.Mode != RecordManagerMode.View)
            {
                throw new InvalidOperationException("O método " + this.UniqueID + ".Requery() não é permitido no modo " + this.Mode);
            }

            if (!this.bindingDone)
            {
                this.DataBind();
            }

            this.thisContainer.SetMode(RecordManagerMode.Edit);
            RecordContainerLib.SetDataRow(this, this.GetDataRow(null));
            this.thisContainer.SetMode(RecordManagerMode.View);
        }

        public void Rollback()
        {
            if (!this.bindingDone)
            {
                this.DataBind();
            }

            ContainerManagerLib.Rollback(this);
        }

        public void SetDataRow(TDataRow row)
        {
            if (this.AutoDataBind)
            {
                throw new InvalidOperationException("O método SetDataRow() é inválido quando AutoDataBind=true.");
            }

            if (this.Mode != RecordManagerMode.View)
            {
                throw new InvalidOperationException("O método SetDataRow() só é permitido em View.");
            }

            if (row.Deleted && !this.ShowDeletedRecords)
            {
                throw new InvalidOperationException("O registro foi removido e a propriedade " + this.UniqueID + ".ShowDeletedRecords é false.");
            }

            this.thisContainer.SetMode(RecordManagerMode.Edit);
            RecordContainerLib.SetDataRow(this, row);
            this.thisContainer.SetMode(RecordManagerMode.View);
        }

        public void ToggleInsertInfo()
        {
            this.SetInsertInfoVisible(!this.InsertInfoVisible);
        }

        public bool Undelete()
        {
            return ContainerManagerLib.Undelete(this);
        }

        protected virtual void RenderManager(HtmlTextWriter writer)
        {
            var designMode = TControl.InDesignMode(this);
            var empty = this.IsEmpty && this.Mode == RecordManagerMode.View;
            var historyIcons = this.table != null && this.table.HistoryEnabled && this.ShowHistoryIcon;
            var renderTitleRow = designMode ? this.Title.Length > 0
                                     : ((this.Title.Length > 0 || historyIcons) &&
                                        (!empty || this.ShowTitleWhenEmpty));
            var hasMessages = this.GeneralErrorLabel.Length == 0 && this.ParentManager == null && this.generalMessageList.Count > 0;
            var insertInfo = this.InsertInfoVisible && this.Mode == RecordManagerMode.View;

            this.rowTitle.Visible = renderTitleRow;
            this.rowMessage.Visible = !designMode && (insertInfo || empty || hasMessages);

            this.imgHistory.Visible = historyIcons;

            

            this.imgChanges.Visible = false;
            if (historyIcons && this.rowTitle.Visible && this.Mode == RecordManagerMode.View && this.thisContainer.PrimaryKeyValues != null)
            {
                var dataRow = RecordContainerLib.GetRecordFromDb(this, null, this.table, true);
                if (dataRow != null && dataRow.Updated)
                {
                    HistoryLib.RegisterHistoryScript(this.Page);

                    this.imgChanges.Attributes.Add("onclick", "openHistory('" + TUtil.TranslateRelativeUrl(this.thisManager.HistoryUrl) + "?" + HistoryLib.GetHistoryQueryString(dataRow) + "');");
                    this.imgChanges.Attributes.Add("onmouseover", "style.cursor = 'hand';");
                    this.imgChanges.Attributes.Add("onmouseout", "style.cursor = 'auto';");

                    this.imgChanges.Visible = true;
                }
            }

            

            if (this.rowMessage.Visible)
            {
                var cell = new HtmlTableCell();
                this.rowMessage.Cells.Add(cell);

                if (empty)
                {
                    #region Mensagem de manager vazio

                    var lbl = new Label();
                    lbl.Text = this.EmptyManagerMsg;
                    cell.Controls.Add(lbl);

                    #endregion
                }

                if (insertInfo)
                {
                    #region Informações de cadastro

                    var lbl = new Label();
                    lbl.CssClass = "HistoryInfo";
                    {
// Monta a mensagem com informações de cadastro conforme disponibilidade
                        var user = this.thisContainer.HistInsertUser;
                        var stamp = this.thisContainer.HistInsertStamp;
                        if (user.Length == 0)
                        {
                            if (stamp == DateTime.MinValue)
                            {
                                lbl.Text = "Não existem informações de cadastro deste registro";
                            }
                            else
                            {
                                lbl.Text = string.Format("Registro cadastrado em {0:dd/MM/yyyy\" às \"HH:mm:ss}", stamp);
                            }
                        }
                        else if (stamp == DateTime.MinValue)
                        {
                            lbl.Text = string.Format("Registro cadastrado por {0}", user);
                        }
                        else
                        {
                            lbl.Text = string.Format("Registro cadastrado por {0} em {1:dd/MM/yyyy\" às \"HH:mm:ss}", user, stamp);
                        }
                    }

                    cell.Controls.Add(lbl);

                    #endregion
                }

                if (hasMessages)
                {
                    #region Mensagens de generalMessageList

                    foreach (Label lbl in this.generalMessageList)
                    {
                        cell.Controls.Add(new LiteralControl("<BR/>"));
                        cell.Controls.Add(lbl);
                    }
                }

                #endregion
            }

            if (empty)
            {
                // Remove todos os controles do manager
                var controls = new Control[this.Controls.Count];
                this.Controls.CopyTo(controls, 0);
                foreach (var control in controls)
                {
                    if (control != this.tabHeader)
                    {
                        this.Controls.Remove(control);
                    }
                }
            }

            // Se rowTitle ou rowMessage for visível, adiciona tabHeader
            // A adição é feita neste momento para que SaveViewState() não salve
            // o estado de tabHeader. Isso causaria problema no próximo postback
            // porque LoadViewState() não encontraria o controle correspondente.
            if (this.rowTitle.Visible || this.rowMessage.Visible)
            {
                this.Controls.AddAt(0, this.tabHeader);
            }

            base.Render(writer);
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            // Precisa chamar DataBind() para que Table esteja disponível em CreateTitleRow()
            if (!this.bindingDone)
            {
                this.DataBind();
            }
            {
// Header do manager (título e insert info)
                this.rowTitle = this.CreateTitleRow();
                this.rowMessage = new HtmlTableRow();

                // tabHeader será adicionado a Controls somente no momento da renderização
                // (RenderManager()) para que seu estado não seja salvo em SaveViewState().
                this.tabHeader = new HtmlTable();
                this.tabHeader.Rows.Add(this.rowTitle);
                this.tabHeader.Rows.Add(this.rowMessage);
                this.tabHeader.Width = "100%";
            }
        }

        protected override void LoadViewState(object savedState)
        {
            this.settingState = true;
            try
            {
                var state = (object[])savedState;
                var offset = 0;

                base.LoadViewState(state[offset++]);
                this.thisManager.IsRoot = (bool)state[offset++];
                this.lastWhere = (object[])state[offset++];
                RecordContainerLib.SetState(this, (IList)state[offset++]);

                // Refaz a query para preencher o manager.
                // Isso é necessário caso algum controle bound (dentro do manager) alterar
                // sua visibilidade (com o viewstate habilitado esse problema não existe).
                if (this.Mode != RecordManagerMode.New)
                {
                    this.DataBind(); // para obter DataSource

                    var row = this.GetDataRow(this.lastWhere);

                    if (row != null)
                    {
                        var oldMode = this.Mode;

                        if (oldMode != RecordManagerMode.Edit)
                        {
                            this.thisContainer.SetMode(RecordManagerMode.Edit);
                        }

                        RecordContainerLib.SetDataRow(this, row);
                        if (oldMode != RecordManagerMode.Edit)
                        {
                            this.thisContainer.SetMode(oldMode);
                        }

                        this.thisManager.Table.Rows.Remove(row);
                    }
                }
            }
            finally
            {
                this.settingState = false;
            }
        }

        protected override void OnInit(EventArgs args)
        {
            if (!TControl.InDesignMode(this))
            {
                this.thisContainer.SetMode(RecordManagerMode.View);
            }

            base.OnInit(args);
        }

        protected override void OnLoad(EventArgs e)
        {
            // Inicializa a propriedade base.Table
            if (!this.bindingDone)
            {
                this.DataBind();
            }

            foreach (var control in TControl.GetChildTControls(this))
            {
                if (control is ITControlEditable && control.ColumnName.Length > 0)
                {
                    ((ITControlEditable)control).Changed += this.childChanged;
                }
            }

            if (this.Table != null)
            {
                // Grava propriedades de validação nos controles
                RecordContainerLib.SetClientValidationAttributes(this, this.Table, this.ShowMessageBox, this.ControlMessageType, this.EnableClientScript);

                // Setar os DataType's de todos os TControls neste manager
                // Deve ser feito a cada postback porque o manager pode estar vazio (os TControl's
                // com EnableViewState = false (valor default) não estão renderizados para guardarem
                // seus DataType's).
                RecordContainerLib.SetControlsDataTypes(TControl.GetChildTControls(this), this.Table);
            }

            // Detecta alterações nos controles dos quais este controle depende
            DependerLib.RegisterDepender(this);

            if (!this.Page.IsPostBack && !this.loaded)
            {
                

                // A verificação de PrimaryKeyValues == null é realizada porque o método Requery(),
                // EnterEditMode() ou EnterAddNewMode() pode já ter sido chamado, situação na qual
                // o modo inicial deverá ser ignorado.
                if (this.thisManager.IsRoot && this.PrimaryKeyValues == null)
                {
                    var initialMode = ContainerManagerLib.GetInitialMode(this);
                    var permission = TControl.GetPermission((IContainerManager)this);

                    if (initialMode == RecordManagerMode.New)
                    {
                        if (permission == null || !permission.ReadOnly)
                        {
                            this.EnterAddNewMode();
                        }
                        else if (this.Table != null)
                        {
                            this.LoadInitialRow();
                            this.AddMessage(TPermission.DenialMessage, true);
                        }
                    }
                    else if (initialMode == RecordManagerMode.Edit)
                    {
                        if (permission == null || !permission.ReadOnly)
                        {
                            this.EnterEditMode();
                        }
                        else if (this.Table != null)
                        {
                            this.LoadInitialRow();
                            this.AddMessage(TPermission.DenialMessage, true);
                        }
                    }
                    else if (this.Table != null && this.AutoDataBind)
                    {
                        this.LoadInitialRow();
                    }

                    RecordContainerLib.SetControlsMode(this);
                }
            }

            // Seta loaded para true, se é que ainda não tenha sito setado
            this.loaded = true;

            // Chama o evento Load do manager depois de setar o estado inicial.
            // Caso o estado inicial seja View e um registro tenha sido trazido do banco, será possível
            // utilizar estes valores no tratamento do Load.
            base.OnLoad(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            this.FillGeneralMessageLabel();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.changeFilterDenied)
            {
                writer.WriteLine("<P><B>Alteração no filtro não é permitida no modo corrente.</B></P>");
            }

            if (!this.DebugMode)
            {
                this.RenderManager(writer);
                return;
            }

            writer.WriteLine("<TABLE BORDER=1 CELLSPACING=0 CELLPADDING=3><TR><TD>");
            writer.WriteLine("<P><FONT FACE=verdana SIZE=1 COLOR=gray>");
            TControl.WriteDebugInfo(writer, this);
            writer.WriteLine("<B>lastWhere:</B> " + (this.lastWhere == null ? "null" : this.lastWhere[0] + ", { " + StrLib.EnumerableToStr((DbObject[])this.lastWhere[1], ", ") + " }") + "<BR/>");
            ContainerManagerLib.WriteDebugInfo(writer, this.thisManager);
            RecordContainerLib.WriteDebugInfo(writer, this.thisContainer);
            DependerLib.WriteDebugInfo(writer, this);

            writer.WriteLine("</FONT></P>");

            this.RenderManager(writer);

            writer.WriteLine("</TD></TR></TABLE>");
        }

        protected override object SaveViewState()
        {
            try
            {
                return new[]
                       {
                           base.SaveViewState(), 
                           this.thisManager.IsRoot, 
                           this.lastWhere, 
                           RecordContainerLib.GetState(this), 
                       };
            }
            finally
            {
            }
        }

        protected void OnPostContainerOperation(PostContainerOperationEventArgs args)
        {
            if (this.PostContainerOperation != null)
            {
                this.PostContainerOperation(this, args);
            }

            this.RaiseBubbleEvent(this, args);
        }

        /// <summary>
        ///   Adiciona um erro genérico à lista de erros genéricos.
        ///   Caso GeneralErrorLabel não tenha sido informado, adiciona na lista do parent manager.
        /// </summary>
        private void AddMessage(string message, bool isError)
        {
            if (this.GeneralErrorLabel.Length == 0 && this.ParentManager != null && this.ParentManager is IRecordContainerInternal)
            {
                ((IRecordContainerInternal)this.ParentManager).AddMessage(message, isError);
            }
            else
            {
                var lbl = new Label();
                lbl.EnableViewState = false;
                lbl.CssClass = isError ? "MsgError" : "MsgWarning";
                lbl.Text = message;
                this.generalMessageList.Add(lbl);
                lbl = null;
            }
        }

        void IRecordContainerInternal.AddMessage(string message, bool isError)
        {
            this.AddMessage(message, isError);
        }

        void IContainerManagerInternal.AddMessage(string message, bool isError)
        {
            this.AddMessage(message, isError);
        }

        TConnection IContainerManager.CreateConnection()
        {
            return ContainerManagerLib.CreateConnection(this);
        }

        private HtmlTableRow CreateTitleRow()
        {
            var tableRow = new HtmlTableRow();

            var cell = new HtmlTableCell();
            cell.Attributes.Add("class", this.TitleRowCssClass);
            cell.Attributes.Add("nowrap", "nowrap");
            tableRow.Cells.Add(cell);

            // Ícone de tabela historificada
            this.imgHistory = new ImageButton();
            this.imgHistory.ID = this.UniqueID + "_imgHistory";
            this.imgHistory.ImageUrl = TUtil.TranslateRelativeUrl(this.HistoryImageUrl);
            this.imgHistory.ToolTip = this.HistoryToolTip;
            this.imgHistory.Click += this.imgHistory_Click;
            cell.Controls.Add(this.imgHistory);

            // Ícone de registro alterado
            this.imgChanges = new Image();
            this.imgChanges.ImageUrl = TUtil.TranslateRelativeUrl(this.ModifiedImageUrl);
            this.imgChanges.ToolTip = this.ModifiedToolTip;
            this.imgChanges.Visible = false;
            cell.Controls.Add(this.imgChanges);

            // Renderiza um espaço entre o ícone de histórico e o título
            cell.Controls.Add(new LiteralControl(" "));

            // Substitui Title se foi informado "?"
            string title;
            {
                title = this.Title;
                if (this.Table != null && title == "?")
                {
                    title = this.Table.Name;
                    if (title.Length == 0)
                    {
                        title = this.Table.TableName;
                    }

                    title = StrLib.ToProper(title);
                }
            }

            // Título
            var lbl = new Label();
            lbl.Text = title;
            lbl.CssClass = this.TitleCssClass;
            cell.Controls.Add(lbl);

            return tableRow;
        }

        TConnectionWritable IContainerManagerInternal.CreateWritableConnection()
        {
            return ContainerManagerLib.CreateWritableConnection(this);
        }

        /// <summary>
        ///   Preenche o controle informado na propriedade GeneralErrorLabel com as mensagens geradas
        ///   por este controle (via AddGeneralMessage). Se a propriedade não foi informada, trata
        ///   as mensagens em RenderManager().
        /// </summary>
        private void FillGeneralMessageLabel()
        {
            if (this.GeneralErrorLabel.Length > 0)
            {
                var control = this.NamingContainer.FindControl(this.GeneralErrorLabel);
                if (control == null)
                {
                    throw new InvalidOperationException("O controle informado na propriedade GeneralErrorLabel (" + this.GeneralErrorLabel + ") não foi encontrado na página.");
                }

                if (!(control is Label))
                {
                    throw new InvalidOperationException("O controle informado na propriedade GeneralErrorLabel deve ser do tipo " + typeof (Label).FullName + ".");
                }

                var swriter = new StringWriter();
                var hwriter = new HtmlTextWriter(swriter);
                foreach (Label lbl in this.generalMessageList)
                {
                    lbl.RenderControl(hwriter);
                    hwriter.WriteLine("<BR/>");
                }

                ((Label)control).Text = swriter.ToString();
            }

            // Se GeneralErrorLabel.Length == 0, o caso será tratado em RenderManager().
        }

        string IContainerManager.GetCaption(string fieldName)
        {
            var controls = this.thisContainer.GetControl(fieldName);
            if (controls.Length == 0)
            {
                return ((TDataColumn)this.Table.Columns[fieldName]).GetCaption(Thread.CurrentThread.CurrentCulture.Name);
            }
            else
            {
                return controls[0].GetCaption(Thread.CurrentThread.CurrentCulture.Name);
            }
        }

        ITControl[] IRecordContainerInternal.GetControl(string fieldName)
        {
            var list = new ArrayList();

            foreach (ITControl control in this.managedControls)
            {
                if (string.Compare(control.ColumnName, fieldName, true) == 0)
                {
                    list.Add(control);
                }
            }

            return (ITControl[])list.ToArray(typeof (ITControl));
        }

        /// <summary>
        ///   Realiza consulta no banco de dados, de acordo com a propriedade SqlWhere.
        /// </summary>
        private TDataRow GetDataRow(object[] stateWhere)
        {
            TDataRow row = null;

            if (!this.bindingDone)
            {
                this.DataBind();
            }

            if (this.table == null)
            {
                return null;
            }

            var cn = this.table.CreateConnection();
            cn.Open();

            try
            {
                string where;
                DbObject[] whereValues;
                if (stateWhere == null)
                {
                    

                    where = this.SqlWhere;
                    whereValues = this.SqlWhereValues;

                    try
                    {
                        TControl.GetSqlWhere(this.Table, null, this.NamingContainer, cn.Rdbms, true, ref where, ref whereValues);
                    }
                    catch (System.Exception exc)
                    {
                        throw new System.Exception("Erro na propriedade " + this.UniqueID + ".SqlWhere.", exc);
                    }

                    
                }
                else
                {
                    where = (string)stateWhere[0];
                    whereValues = (DbObject[])stateWhere[1];
                }

                this.lastWhere = new object[] { where, whereValues };

                // Realiza o Get() somente se houver restrições (de contexto ou SqlWhere)
                if (where.Length > 0)
                {
                    int count;

                    

                    try
                    {
                        count = this.table.Get(
                            cn, 
                            RecordContainerLib.GetColumnNames(this, this.Table), 
                            where, whereValues, string.Empty, 0, 0, 
                            true, this.table.HistoryEnabled && this.ShowDeletedRecords
                            );
                    }
                    catch (System.Exception exc)
                    {
                        throw new InvalidOperationException("Possível erro na propriedade " + this.UniqueID + ".SqlWhere: " + where, exc);
                    }

                    // Obtém ponteiro para o registro obtido
                    // Se o manager não estiver sendo restrito, deve devolver null
                    if (count == 1)
                    {
                        row = this.Table.Rows[this.Table.Rows.Count - 1]; // O registro obtido é o último da tabela.
                    }

                    // Se obteve mais de um registro, dá exception.
                    if (count > 1)
                    {
                        throw new TooManyRowsException(this, where == null ? "<vazio>" : where, whereValues);
                    }
                }
                else
                {
                }
            }
            finally
            {
                cn.Close();
            }

            return row;
        }

        DataTable IContainerManager.GetDesignTimeDataTable()
        {
            return TControl.GetDesignTimeDataTable(this, this.DataMember);
        }

        string[] IContainerManager.GetFields()
        {
            if (!this.bindingDone)
            {
                this.DataBind();
            }

            return RecordContainerLib.GetColumnNames(this, this.Table);
        }

        string IContainerManager.GetReturnUrl()
        {
            return this.Page.Request["ReturnUrl"];
        }

        string IContainerManager.GetTitle()
        {
            return ContainerManagerLib.GetTitle(this);
        }

        private TDataRow LoadInitialRow()
        {
            var row = this.GetDataRow(null);

            this.thisContainer.SetMode(RecordManagerMode.Edit);
            RecordContainerLib.SetDataRow(this, row);
            this.thisContainer.SetMode(RecordManagerMode.View);

            this.loaded = true;

            return row;
        }

        void IRecordContainerInternal.OnPostContainerOperation(PostContainerOperationEventArgs args)
        {
            this.OnPostContainerOperation(args);
        }

        void IRecordContainerInternal.OnPostPutDataRow(PostPutDataRowArgs args)
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

        void IRecordContainerInternal.OnPrePutDataRow(PrePutDataRowArgs args)
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

        void IContainerManagerInternal.PosCommit(bool commited, bool isRoot)
        {
            if (commited)
            {
                if (isRoot)
                {
                    var row = RecordContainerLib.GetRecordFromDb(this, null, this.Table, false);

                    if (row != null)
                    {
                        // row == null quando RecordManager não está associado a DataTable
                        // Buscar o registro do banco de dados para que as alterações realizadas no banco
                        // por entry-points (ou mesmo a geração de valores de colunas identity) sejam mostradas.
                        var changeToEdit = false;
                        if (this.thisContainer.Mode == RecordManagerMode.View)
                        {
                            this.thisContainer.SetMode(RecordManagerMode.Edit);
                            changeToEdit = true;
                        }

                        RecordContainerLib.SetDataRow(this, row);
                        if (changeToEdit)
                        {
                            this.thisContainer.SetMode(RecordManagerMode.View);
                        }

                        if (this.modeBeforeCommit == RecordManagerMode.New)
                        {
                            if (this.lastWhere == null)
                            {
                                this.lastWhere = new object[2];
                            }

// row aqui será null caso SkipRow tenha sido setado em
                            // PreRowInsert e SetPrimaryKey() não tenha sido chamado.
                            string where;
                            DbObject[] whereValues;
                            DataLib2.CreateWhere(row, out where, out whereValues);
                            this.lastWhere[0] = where;
                            this.lastWhere[1] = whereValues;
                        }
                    }

                    // Mensagem de insert ou update bem sucedido
                    if (this.modeBeforeCommit == RecordManagerMode.New)
                    {
                        this.thisContainer.AddMessage(this.AddNewDoneMsg + (this.DebugMode ? " (" + this.ID + ")" : string.Empty), false);
                    }
                    else if (this.modeBeforeCommit == RecordManagerMode.Edit)
                    {
                        this.thisContainer.AddMessage(this.EditDoneMsg + (this.DebugMode ? " (" + this.ID + ")" : string.Empty), false);
                    }
                }

                if (this.table != null)
                {
                    this.table.Rows.Clear();
                }
            }
        }

        void IContainerManagerInternal.PosDelete(bool deleted)
        {
            if (deleted)
            {
                var returnUrl = this.thisManager.GetReturnUrl();

                // Se existir ReturnUrl na query string, redireciona para a página
                if (returnUrl != null && returnUrl != string.Empty)
                {
                    // Não precisa aplicar HttpUtility.UrlDecode() em returnUrl!!
                    this.Page.Response.Redirect(returnUrl);
                }
                else
                {
                    if (this.table.HistoryEnabled && this.ShowDeletedRecords)
                    {
                        // Buscar o registro do banco de dados para que as informações de history sejam atualizadas.
                        this.thisContainer.SetMode(RecordManagerMode.Edit);
                        RecordContainerLib.SetDataRow(this, RecordContainerLib.GetRecordFromDb(this, null, this.Table, false));
                        this.thisContainer.SetMode(RecordManagerMode.View);
                    }

                    // Mensagem de delete bem sucedido
                    this.thisContainer.AddMessage(this.DeleteDoneMsg + (this.DebugMode ? " (" + this.ID + ")" : string.Empty), false);
                }
            }
        }

        void IContainerManagerInternal.PosEnterAddNewMode()
        {
        }

        void IContainerManagerInternal.PosEnterEditMode()
        {
        }

        void IContainerManagerInternal.PosRollback(bool isRoot)
        {
            if (isRoot &&
// Só usa ReturnUrl se estiver cancelando em modo AddNew e for DataEntry
                this.modeBeforeCommit == RecordManagerMode.New && this.DataEntry)
            {
                var returnUrl = this.thisManager.GetReturnUrl();
                if (returnUrl != null && returnUrl != string.Empty)
                {
                    // Não precisa aplicar HttpUtility.UrlDecode() em returnUrl!!
                    this.Page.Response.Redirect(returnUrl);
                }
            }
        }

        void IContainerManagerInternal.PosUndelete(bool undeleted)
        {
            if (undeleted)
            {
                if (this.table.HistoryEnabled && this.ShowDeletedRecords)
                {
                    // Buscar o registro do banco de dados para que as informações de history sejam atualizadas.
                    this.thisContainer.SetMode(RecordManagerMode.Edit);
                    RecordContainerLib.SetDataRow(this, RecordContainerLib.GetRecordFromDb(this, null, this.Table, false));
                    this.thisContainer.SetMode(RecordManagerMode.View);
                }

                // Mensagem de undelete bem sucedido
                this.thisContainer.AddMessage("Registro reinserido." + (this.DebugMode ? " (" + this.ID + ")" : string.Empty), false);
            }
        }

        void IContainerManagerInternal.PreCommit()
        {
            this.modeBeforeCommit = this.Mode;
        }

        void IContainerManagerInternal.PreDelete()
        {
            if (this.PrimaryKeyValues == null && TControl.GetChildManagers(this).Length == 0)
            {
                throw new InvalidOperationException("Não existe registro a ser removido.");
            }
        }

        void IContainerManagerInternal.PreEnterAddNewMode()
        {
        }

        void IContainerManagerInternal.PreEnterEditMode()
        {
            if (!this.loaded)
            {
                this.LoadInitialRow();
            }

            if (this.PrimaryKeyValues == null && TControl.GetChildManagers(this).Length == 0)
            {
                throw new NoRowsException(this);
            }
        }

        void IContainerManagerInternal.PreRollback()
        {
        }

        void IContainerManagerInternal.PreUndelete()
        {
        }

        void IContainerManager.Refresh()
        {
            if (!this.bindingDone)
            {
                this.DataBind();
            }

            DataRow row = this.GetDataRow(this.lastWhere);

            this.thisContainer.SetMode(RecordManagerMode.Edit);
            RecordContainerLib.SetDataRow(this, row);
            this.thisContainer.SetMode(RecordManagerMode.View);
        }

        void IRecordContainerInternal.RegisterControl(ITControl control)
        {
            if (control.ColumnName.Length > 0)
            {
                this.managedControls.Add(control);
            }
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

        private void SetInsertInfoVisible(bool visible)
        {
            if (!this.settingState)
            {
                if (visible && !this.thisManager.Table.HistoryEnabled)
                {
                    throw new InvalidOperationException("Operação permitida somente em tabelas historificadas.");
                }
            }

            this.ViewState["InsertInfoVisible"] = visible;
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

        private void childChanged(object sender, ChangedEventArgs args)
        {
            this.thisContainer.SetChanged(true);
        }

        private void controlChanged(object sender, ChangedEventArgs args)
        {
            if (this.Mode != RecordManagerMode.View)
            {
                // Ativa flag para ser tratado no Render()
                this.changeFilterDenied = true;
                return;
            }

            if (this.AutoDataBind)
            {
                this.Requery();
            }
            else
            {
                this.Clear();
            }
        }

        private void imgHistory_Click(object sender, ImageClickEventArgs e)
        {
            this.ToggleInsertInfo();
        }

        internal class Exception : System.Exception
        {
            internal Exception()
                : this(string.Empty, null)
            {
            }

            internal Exception(string message)
                : this(message, null)
            {
            }

            internal Exception(RecordManager manager, System.Exception innerException)
                : this(
                    "Ocorreu um erro no RecordManager" +
                    (manager == null ? string.Empty : " " + manager.ID), 
                    innerException
                    )
            {
            }

            internal Exception(string message, System.Exception innerException)
                : base(message, innerException)
            {
            }
        }

        internal class InvalidActionException : InvalidOperationException
        {
            internal InvalidActionException(string action, RecordManagerMode mode)
                : base(
                    "A operação " +
                    action + " " +
                    "é inválida no modo " +
                    mode
                    )
            {
            }
        }

        internal class InvalidOperationException : Exception
        {
            internal InvalidOperationException()
                : this(string.Empty, null)
            {
            }

            internal InvalidOperationException(string message)
                : this(message, null)
            {
            }

            internal InvalidOperationException(string message, System.Exception innerException)
                : base(message, innerException)
            {
            }
        }

        internal class NoRowsException : InvalidOperationException
        {
            internal NoRowsException()
                : this(null, string.Empty, new DbObject[0])
            {
            }

            internal NoRowsException(RecordManager manager)
                : this(manager, string.Empty, new DbObject[0])
            {
            }

            internal NoRowsException(RecordManager manager, string where)
                : this(manager, where, new DbObject[0])
            {
            }

            internal NoRowsException(RecordManager manager, string where, DbObject[] whereValues)
                : base("A consulta do RecordManager " +
                       (manager == null ? string.Empty : manager.ID + " ") +
                       "não trouxe nenhum registro" +
                       (where == null || where == string.Empty
                            ? string.Empty
                            : ". Where: " + where + " [" + StrLib.EnumerableToStr(whereValues, ", ") + "]"))
            {
            }
        }

        internal class TooManyRowsException : InvalidOperationException
        {
            internal TooManyRowsException()
                : this(null, string.Empty, new DbObject[0])
            {
            }

            internal TooManyRowsException(RecordManager manager, string where)
                : this(manager, where, new DbObject[0])
            {
            }

            internal TooManyRowsException(RecordManager manager, string where, DbObject[] whereValues)
                : base("A consulta do RecordManager " +
                       (manager == null ? string.Empty : manager.ID + " ") +
                       "trouxe mais de um registro" +
                       (where == null || where == string.Empty
                            ? string.Empty
                            : ". Where: " + where + " [" + StrLib.EnumerableToStr(whereValues, ", ") + "]"))
            {
            }
        }
    }
}