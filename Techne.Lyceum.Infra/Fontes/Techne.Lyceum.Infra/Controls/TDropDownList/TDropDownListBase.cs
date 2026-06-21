using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Data;
using techne.library.sql.structure;
using Techne.Library.Sql.Structure;

namespace Techne.Controls
{
    [
        Designer("Techne.Controls.Design.TDropDownListDesigner"), 
        ParseChildren(true, "Items")
    ]
    internal abstract class TDropDownListBase : TControlEditable, IPostBackDataHandler, IDepender
    {
        public const string NotFoundText_Def = "<Lista Vazia>";

        public const string NullText_Def = "<Não Informado>";

        public const string NullValue = "__NULL__";

        public const string SelectAllText_Def = "<Todos>";

        public const string SelectAllValue = "__ALL__";

        internal const bool RestrictByNullControls_Def = false;

        internal const bool SelectAllAllowed_Def = false;

        private const bool ForcePostBack_Def = false;

        private string baseDataTextField;

        private string baseDataValueField;

        private string baseNotFoundText;

        private string baseNullText;

        private string baseSelectAllText;

        private string baseSqlOrder;

        private SqlSelect baseSqlSelect;

        private string baseSqlWhere;

        private ChangedEventHandler changedHandler;

        private string connection;

        /// <summary>
        ///   Inicializado por InitDefaultValue().
        ///   Utilizado somente quando o controle tem lista fixa.
        /// </summary>
        private DbObject defaultValue;

        private string[] dependees;

        private DropDownList drp;

        private ListItem[] fixedItems = new ListItem[0];

        private object loaded;

        private bool selectAllAllowed;

        private int selectedIndex;

        public TDropDownListBase()
        {
            

            // (em ordem alfabética)
            this.Connection = string.Empty;
            this.ForcePostBack = ForcePostBack_Def;
            this.HasValue = false;
            this.NullAllowed = false;
            this.RestrictByNullControls = RestrictByNullControls_Def;
            this.SelectAllAllowed = SelectAllAllowed_Def;
            this.SelectedIndex = -1;

            

            this.BaseDataTextField = string.Empty;
            this.BaseDataValueField = string.Empty;
            this.BaseNotFoundText = NotFoundText_Def;
            this.BaseNullText = NullText_Def;
            this.BaseSelectAllText = SelectAllText_Def;
            this.BaseSqlOrder = string.Empty;
            this.BaseSqlSelect = new SqlSelect();
            this.BaseSqlWhere = string.Empty;
            this.BaseSqlWhereValues = new DbObject[0];

            this.loaded = false;
            this.Initialized = false;
        }

        // (em ordem alfabética)
        [
            Category("Techne"), 
            Description("Disparado após preenchimento da lista do controle."), 
        ]
        public event EventHandler DataLoaded;

        [
            Category("Techne"), 
            Description("Permite o preenchimento da lista item a item (a propriedade SqlSelect deve ser vazia)."), 
        ]
        public event FixedItemsInitEventHandler FixedItemsInit;

        [
            Category("Techne"), 
            DefaultValue(""), 
            Description("ConnectionString, ou chave da connectionString na classe Techne.Data.ConnectionList, a ser usada para executar o comando SQLQuery. ATENÇÃO: esta propriedade é case-sensitive.")
        ]
        public virtual string Connection
        {
            get
            {
                return this.connection;
            }

            set
            {
                this.connection = value == null ? string.Empty : value.Trim();
            }
        }

        [DefaultValue(false), Category("Techne"), Description("Indica se o controle permite o valor DBNull. " + "Esta propriedade será setada automaticamente quando o controle pertencer a um manager (de acordo com o NOT NULL da base table)."),]
        public virtual bool NullAllowed { get; set; }

        [
            DefaultValue(SelectAllAllowed_Def), 
            Category("Techne"), 
            Description("Determina se o controle permite selecionar todos os itens da lista (não impor restrições)"), 
        ]
        public virtual bool SelectAllAllowed
        {
            get
            {
                // Se estiver dentro de um record manager, devolve false.
                if (this.Manager != null)
                {
                    return false;
                }

                return this.selectAllAllowed;
            }

            set
            {
                // Se estiver dentro de um record manager, causará exception.
                if (this.Manager != null)
                {
                    throw new InvalidOperationException("Esta propriedade não pode ser setada se o controle estiver dentro de um manager");
                }

                this.selectAllAllowed = value;
            }
        }

        [
            Description("Valor do controle tipado conforme a propriedade DataType. Para null deve-se utilizar DBNull ou string.Empty. O valor null causará ArgumentNullException."), 
            Browsable(false)
        ]
        public override DbObject DBValue
        {
            get
            {
                if (!this.Loaded)
                {
                    return base.DBValue;
                }

                if (this.SelectedItem.Value == NullValue)
                {
                    return DBNull.Value;
                }
                else if (this.SelectedItem.Value == SelectAllValue)
                {
                    return null;
                }
                else
                {
                    // O campo Value dos ListItem's estão no formato da cultura do controle
                    return StrLib.TypeStr(this.SelectedItem.Value, this.DataType, this.Culture);
                }
            }

            set
            {
                base.DBValue = value;

                if (!this.Loaded)
                {
                    this.SelectedIndex = -1;
                }
                else
                {
                    var selectItem = this.FindListItem(value);

                    // Se o valor não for encontrado na lista: NADA FAZ!
                    if (selectItem == null)
                    {
                        return;
                    }

                    this.SelectedIndex = this.Items.IndexOf(selectItem);

                    foreach (ListItem listItem in this.Items)
                    {
                        listItem.Selected = listItem == selectItem;
                    }
                }
            }
        }

        public override DbType DataType
        {
            get
            {
                return base.DataType;
            }

            set
            {
                base.DataType = value;
                this.InitDefaultValue();
            }
        }

        [
            Browsable(false)
        ]
        public ListItem[] FixedItems
        {
            get
            {
                if (this.fixedItems == null)
                {
                    throw new InvalidOperationException();
                }

                return this.fixedItems;
            }
        }

        [Category("Techne"), DefaultValue(ForcePostBack_Def), Description("Força o postback mesmo quando não existem controles que dependam deste."),]
        public bool ForcePostBack { get; set; }

        [
            PersistenceMode(PersistenceMode.InnerDefaultProperty)
        ]
        public ListItemCollection Items
        {
            get
            {
                return this.drp.Items;
            }
        }

        /// <summary>
        ///   Indica se o método DataLoad() já foi chamado.
        /// </summary>
        [Browsable(false)]
        public bool Loaded
        {
            get
            {
                // Esta verificação é necessária pois o DBValue(get) chama esta propriedade no TControl.ctor(),
                // o que impede de inicializar esta propriedade antes.
                if (this.loaded == null)
                {
                    return false;
                }

                return (bool)this.loaded;
            }
        }

        [Category("Techne"), DefaultValue(RestrictByNullControls_Def), Description("Indica se os controles com DBValue = DbNull, referenciados no BaseSqlWhere restringirão este controle")]
        public bool RestrictByNullControls { get; set; }

        [
            Browsable(false)
        ]
        public int SelectedIndex
        {
            get
            {
                return this.selectedIndex;
            }

            set
            {
                this.selectedIndex = value < 0 ? -1 : value;
            }
        }

        protected virtual string BaseSqlOrder
        {
            get
            {
                return this.baseSqlOrder;
            }

            set
            {
                this.baseSqlOrder = value == null ? string.Empty : value.Trim();
            }
        }

        protected virtual SqlSelect BaseSqlSelect
        {
            get
            {
                return this.baseSqlSelect;
            }

            set
            {
                this.baseSqlSelect = value == null ? new SqlSelect() : value;
            }
        }

        protected virtual string BaseSqlWhere
        {
            get
            {
                return this.baseSqlWhere;
            }

            set
            {
                this.baseSqlWhere = value == null ? string.Empty : value.Trim();

                // Força o recálculo de Dependees
                this.dependees = null;
            }
        }

        protected virtual DbObject[] BaseSqlWhereValues { get; set; }

        protected override string EntryValue
        {
            get
            {
                if (this.SelectedItem.Value == NullValue)
                {
                    return string.Empty;
                }
                else
                {
                    return this.SelectedItem.Value;
                }
            }
        }

        protected string BaseDataTextField
        {
            get
            {
                return this.baseDataTextField;
            }

            set
            {
                this.baseDataTextField = value == null ? string.Empty : value;
            }
        }

        protected string BaseDataValueField
        {
            get
            {
                return this.baseDataValueField;
            }

            set
            {
                this.baseDataValueField = value == null ? string.Empty : value;
            }
        }

        protected string BaseNotFoundText
        {
            get
            {
                return this.baseNotFoundText;
            }

            set
            {
                this.baseNotFoundText = value == null ? string.Empty : value;
            }
        }

        protected string BaseNullText
        {
            get
            {
                return this.baseNullText;
            }

            set
            {
                this.baseNullText = value == null ? string.Empty : value;
            }
        }

        protected string BaseSelectAllText
        {
            get
            {
                return this.baseSelectAllText;
            }

            set
            {
                this.baseSelectAllText = value == null ? string.Empty : value;
            }
        }

        protected ListItem SelectedItem
        {
            get
            {
                var index = this.SelectedIndex;
                if (index < 0)
                {
                    throw new Exception("A configuração do controle " + this.UniqueID + " não permite a seleção de um item. Reveja as propriedades do controle, especialmente SelectAllAllowed e NullAllowed.");
                }

                if (index >= this.Items.Count)
                {
                    throw new Exception(string.Format("Erro interno (SelectedIndex={0}, Items.Count={1})", index, this.Items.Count));
                }

                return this.Items[index];
            }
        }

        ChangedEventHandler IDepender.ChangedHandler
        {
            get
            {
                if (this.changedHandler == null)
                {
                    this.changedHandler = new ChangedEventHandler(this.DataLoad);
                }

                return this.changedHandler;
            }
        }

        private string Culture
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentCulture.Name;
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
                        throw new InvalidOperationException("Existe algum erro na propriedade " + this.UniqueID + ".BaseSqlWhere: " + this.BaseSqlWhere + ".", exc);
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

        /// <summary>
        ///   Indica se a propriedade SQLQuery possui mais de uma coluna.
        ///   Só tem valor definido quando Loaded=true (após a chamada do método DataLoadInternal()).
        /// </summary>
        private bool HasValue { get; set; }

        private bool Initialized { get; set; }

        string IDepender.SqlWhere
        {
            get
            {
                return this.BaseSqlWhere;
            }
        }

        public static ListItem[] ConvertIntArrayToListItems(TDropDownListBase drp, int[] indexes)
        {
            var listItems = new ArrayList();
            for (var i = 0; i < indexes.Length; i++)
            {
                if (indexes[i] >= 0)
                {
                    listItems.Add(drp.Items[indexes[i]]);
                }
            }

            return (ListItem[])listItems.ToArray(typeof (ListItem));
        }

        public static int[] ConvertListItemsToIntArray(TDropDownListBase drp, ListItem[] listItems)
        {
            var indexes = new int[listItems.Length];
            for (var i = 0; i < listItems.Length; i++)
            {
                indexes[i] = drp.Items.IndexOf(listItems[i]);
            }

            return indexes;
        }

        public virtual void DataLoad()
        {
            this.DataLoadInternal(false);
        }

        public override void CopyProperties(WebControl target)
        {
            base.CopyProperties(target);

            if (target is DropDownList)
            {
                var drp = (DropDownList)target;

                drp.Page = this.Page;
                drp.AutoPostBack = (this.ForcePostBack || this.Dependers.Length > 0) && !TControl.InDesignMode(this);
                drp.DataTextField = this.BaseDataTextField;
                drp.DataValueField = this.BaseDataValueField;
                drp.SelectedIndex = this.SelectedIndex;

// // Estas propriedades são específicas do DropDownList, mas foram implementadas pelo TDropDownListBase

// drp.DataTextFormatString = DataTextFormatString;
            }
        }

        public override string GetValueError()
        {
            var result = string.Empty;

            if (this.Loaded)
            {
                if (this.SelectedItem.Value != NullValue && this.SelectedItem.Value != SelectAllValue)
                {
                    try
                    {
                        StrLib.TypeStr(this.SelectedItem.Value, this.DataType, this.Culture);
                    }
                    catch (FormatException exc)
                    {
                        result = exc.Message;
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///   Se a lista não estiver vazia e o valor do parâmetro não estiver contido nela, nada faz.
        /// </summary>
        /// <param name = "parameter">Nome do parâmetro cujo valor será utilizado para setar o valor deste controle</param>
        public override void InitFromParameters(string parameter)
        {
            var page = this.Page as TPage;
            if (page == null || page.IsPostBack)
            {
                return;
            }

            if (page.Parameters.Contains(parameter))
            {
                this.DBValue = page.Parameters[parameter];
            }
        }

        /// <summary>
        ///   Se a lista estiver preenchida, seta DBValue para o primeiro item da lista.
        ///   Se SelectAllAllowed=true e, além de SelectAll (e do Null, se existente), houver somente um elemento
        ///   na lista, esse elemento será escolhido, não o SelectAll.
        ///   Se a lista não estiver preenchida, seta DBValue para DBNull.
        /// </summary>
        public override void ResetValue()
        {
            if (!this.SettingState && this.Manager != null && this.ColumnName.Length > 0 && this.Mode != ControlMode.Edit)
            {
                throw new InvalidOperationException(
                    "O método " + this.UniqueID + ".ResetValue() não é permitido no modo " + this.Mode + " " +
                    "devido ao controle pertencer ao manager " + ((Control)this.Manager).UniqueID
                    );
            }

            if (this.defaultValue != null)
            {
                // Tem lista fixa: defaultValue será encontrado na lista por DBValue.set
                this.DBValue = this.defaultValue;
            }
            else if (!this.Loaded)
            {
                base.ResetValue();
            }
            else
            {
                this.SelectedIndex = this.Items.Count > 0 ? 0 : -1;

                // Se SelectAllAllowed == true e tiver somente um elemento na lista, seleciona este elemento
                if (this.SelectAllAllowed)
                {
                    if (this.NullAllowed)
                    {
                        if (this.Items.Count == 3)
                        {
                            this.SelectedIndex = 2;
                        }
                    }
                    else if (this.Items.Count == 2)
                    {
                        this.SelectedIndex = 1;
                    }
                }
            }
        }

        /// <summary>
        ///   Limpa a lista mantendo DBValue.
        ///   Se DBValue não estiver válido, somente limpa a lista.
        /// </summary>
        public void ClearItems()
        {
            if (!this.SettingState && this.Manager != null && this.ColumnName.Length > 0 && this.Mode != ControlMode.Edit)
            {
                throw new InvalidOperationException(
                    "O método " + this.UniqueID + ".ClearItems() não é permitido no modo " + this.Mode + " " +
                    "devido ao controle pertencer ao manager " + ((Control)this.Manager).UniqueID
                    );
            }

            // Se hasOldValue=true, oldValue contém o valor de DBValue no início deste método.
            var hasOldValue = true;
            DbObject oldValue = null;
            try
            {
                oldValue = this.DBValue;
            }
            catch
            {
                // TODO Anotar aqui quando esta exception ocorre
                hasOldValue = false;
            }

            this.Items.Clear();
            this.loaded = false;

            if (hasOldValue)
            {
                // Nunca vai dar ItemNotFoundException porque a lista está vazia
                this.DBValue = oldValue;
            }
            else
            {
                this.ResetValue();
            }
        }

        /// <summary>
        ///   Adiciona à lista os itens especiais SelectAll e Null, conforme as propriedade SelectAllAllowed e NullAllowed.
        ///   1) Se SelectAllAllowed=true e NullAllowed=true, adiciona ambos os itens (primeiro SelectAll, depois Null)
        ///   se houver pelo menos um item na lista. Caso contrário, adiciona somente Null com BaseNotFoundText;
        ///   2) Se SelectAllAllowed=true e NullAllowed=false, adiciona SelectAll se houver pelo menos um item na lista.
        ///   Caso contrário, adiciona somente Null com BaseNotFoundText;
        ///   3) Se SelectAllAllowed=false e NullAllowed=true, adiciona Null e utiliza BaseNotFoundText somente se não
        ///   houver nenhum elemento na lista;
        ///   4) Se SelectAllAllowed=false e NullAllowed=false, adiciona Null com BaseNotFoundText se não houver nenhum
        ///   elemento na lista.
        /// </summary>
        public void CreateReservedItems()
        {
            var itemsCount = this.Items.Count - this.ReservedItemsCount();

            if (this.SelectAllAllowed)
            {
                if (this.NullAllowed)
                {
                    if (itemsCount > 0)
                    {
                        this.AddAllItem();
                        this.AddNullItem(false);
                    }
                    else
                    {
                        this.AddNullItem(true);
                    }
                }
                else if (itemsCount > 0)
                {
                    this.AddAllItem();
                }
                else
                {
                    this.AddNullItem(true);
                }
            }
            else if (this.NullAllowed)
            {
                this.AddNullItem(itemsCount == 0);
            }
            else if (itemsCount == 0)
            {
                this.AddNullItem(true);
            }
        }

        /// <summary>
        ///   Obtém o mesmo comando SELECT executado pelo controle.
        ///   Dispara o evento SetSqlClauses para isso.
        /// </summary>
        public void GetSqlSelect(out string sql, out DbObject[] parametersValues)
        {
            this.GetSQL(this.GetConnection().Rdbms, out sql, out parametersValues);
        }

        /// <summary>
        ///   Devolve true se a lista não estiver vazia e o valor informado pertence a ela.
        ///   Lista vazia: devolve false.
        /// </summary>
        public bool ValueInList(DbObject value)
        {
            return this.FindListItem(value) != null;
        }

        protected virtual void OnDataLoaded(EventArgs args)
        {
            if (this.DataLoaded != null)
            {
                this.DataLoaded(this, args);
            }
        }

        protected virtual void OnFixedItemsInit(FixedItemsInitEventArgs args)
        {
            if (this.FixedItemsInit != null)
            {
                this.FixedItemsInit(this, args);
            }
        }

        protected virtual void OnSetItemDescription(SetItemDescriptionEventArgs args)
        {
            // Existe somente para ser overriden.
        }

        protected virtual void OnSetSqlClauses(SetSqlClausesEventArgs args)
        {
            // Existe somente para ser overriden.
        }

        protected override void LoadViewState(object savedState)
        {
            var state = (object[])savedState;
            var offset = 0;

            base.LoadViewState(state[offset++]);

            this.Items.Clear(); // Ignora os elementos criados em design-time
            foreach (string[] item in (object[])state[offset++])
            {
                this.Items.Add(new ListItem(item[0], item[1]));
            }

            this.fixedItems = ConvertIntArrayToListItems(this, (int[])state[offset++]);

            this.dependees = (string[])state[offset++];
            this.Initialized = (bool)state[offset++];
            this.SelectedIndex = (int)state[offset++];
        }

        protected override void OnInit(EventArgs e)
        {
            var listFixedItems = new ListItemCollection();
            listFixedItems.AddRange(this.GetItemsAsArray(this.Items));
            this.OnFixedItemsInit(new FixedItemsInitEventArgs(listFixedItems));
            this.fixedItems = this.GetItemsAsArray(listFixedItems);

            this.drp.ID = this.UniqueID;

            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                // Se existe lista fixa e Selected foi setado para algum item, seleciona o valor inicial agora
                if (!this.Initialized && (this.RecordContainer == null || this.ColumnName.Length == 0))
                {
                    if (this.fixedItems.Length > 0)
                    {
                        if (this.defaultValue == null)
                        {
                            this.InitDefaultValue();
                        }

                        if (this.defaultValue != null)
                        {
                            var oldMode = this.Mode;
                            if (this.Mode != ControlMode.Edit)
                            {
                                this.Mode = ControlMode.Edit;
                            }

// A verificação !Loaded || ValueInList(defaultValue) aqui seria sempre true
                            this.DBValue = this.defaultValue;
                            if (this.Mode != oldMode)
                            {
                                this.Mode = oldMode;
                            }
                        }
                    }

                    this.Initialized = true;
                }

                // O OnLoad() deve ser chamado após o DataLoad() para que o tratamento do
                // evento Load do controle o DBValue já esteja disponível.
                base.OnLoad(e);

                // Detecta alterações nos controles dos quais este controle depende
                DependerLib.RegisterDepender(this);
            }
            finally
            {
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (!this.Loaded)
            {
                var container = (IRecordContainerInternal)this.RecordContainer;

                // Indica situação em que DataLoadInternal() não deve sobreescrever DBValue se ele não for
                // encontrado na lista. Nesta situação, DataLoadInternal() desabilitará o controle.
                var forceValue = container != null && container.Mode != RecordManagerMode.New && container.PrimaryKeyValues != null && this.ColumnName.Length > 0;

                // Se estiver em View, altera o container para Edit para o DataLoadInternal() não reclamar.
                // Depois volta para View novamente.
                var switchToEdit = container != null && container.Mode == RecordManagerMode.View;
                if (switchToEdit)
                {
                    container.SetMode(RecordManagerMode.Edit);
                }

                this.DataLoadInternal(forceValue);

                if (switchToEdit)
                {
                    container.SetMode(RecordManagerMode.View);
                }
            }

            // OnPreRender() é chamado depois de preencher o controle
            // para que o tratamento do evento possa acessar DBValue.
            base.OnPreRender(e);
        }

        protected override void PreTControlCtor()
        {
            base.PreTControlCtor();

// Deve ser antes da inicialização das propriedades BaseDataTextField e BaseDataValueField
            this.drp = new DropDownList();
        }

        protected override void RenderControlEditMode(HtmlTextWriter writer)
        {
            this.CopyProperties(this.drp);
            this.drp.RenderControl(writer);
        }

        protected override void RenderControlViewMode(HtmlTextWriter writer)
        {
            var lbl = new Label();
            this.CopyProperties(lbl);

            if (this.SelectedIndex >= 0)
            {
                lbl.Text = System.Web.HttpUtility.HtmlEncode(this.SelectedItem.Text);
            }
            else if (this.DBValue != null && this.DBValue.IsNull)
            {
                lbl.Text = System.Web.HttpUtility.HtmlEncode(this.BaseNullText);
            }
            else
            {
                lbl.Text = System.Web.HttpUtility.HtmlEncode(this.ToString(this.DBValue));
            }

            lbl.CssClass = this.CssClass;

            lbl.RenderControl(writer);
        }

        protected override void RenderDebugInfo(HtmlTextWriter writer)
        {
            base.RenderDebugInfo(writer);

            writer.Write("<B>Items.Count: </B>" + this.Items.Count + "<BR/>");
            writer.Write("<B>Loaded: </B>" + this.Loaded + "<BR/>");
            writer.Write("<B>Mode: </B>" + this.Mode + "<BR/>");
            writer.Write("<B>BaseSqlSelect: </B>" + this.BaseSqlSelect + "<BR/>");
            writer.Write("<B>BaseSqlWhere: </B>" + this.BaseSqlWhere + "<BR/>");

            DependerLib.WriteDebugInfo(writer, (IDepender)this);
        }

        protected override void RenderSpecific(HtmlTextWriter writer)
        {
            if (!this.ReadOnly && (this.Loaded && this.Mode == ControlMode.Edit || TControl.InDesignMode(this)) && !this.Blocked)
            {
                this.RenderControlEditMode(writer);
            }
            else
            {
                this.RenderControlViewMode(writer);
            }
        }

        protected override object SaveViewState()
        {
            return new[]
                   {
                       base.SaveViewState(), 
                       this.ConvertItems(), 
                       ConvertListItemsToIntArray(this, this.fixedItems), 
                       this.Dependees, 
                       this.Initialized, 
                       this.SelectedIndex
                   };
        }

        protected override void TrackViewState()
        {
            base.TrackViewState();
            ((IStateManager)this.Items).TrackViewState();
        }

        private void AddAllItem()
        {
            var selectAllItem = this.Items.FindByValue(SelectAllValue);
            if (selectAllItem == null)
            {
                this.Items.Insert(0, this.BaseSelectAllText);
                this.Items[0].Value = SelectAllValue;
            }
            else
            {
                selectAllItem.Text = this.BaseSelectAllText;
            }
        }

        private void AddNullItem(bool useNotFoundText)
        {
            var nullText = useNotFoundText ? this.BaseNotFoundText : this.BaseNullText;

            var nullItem = this.Items.FindByValue(NullValue);
            if (nullItem == null)
            {
                this.Items.Insert(0, nullText);
                this.Items[0].Value = NullValue;
            }
            else
            {
                nullItem.Text = nullText;
            }
        }

        private object[] ConvertItems()
        {
            var result = new object[this.Items.Count];
            for (var i = 0; i < this.Items.Count; i++)
            {
                result[i] = new[] { this.Items[i].Text, this.Items[i].Value };
            }

            return result;
        }

        /// <summary>
        ///   Este overload do DataLoad() é chamado quando algum controle do qual este depende é alterado
        ///   (está associado ao evento TControl.Changed).
        /// </summary>
        private void DataLoad(object sender, ChangedEventArgs args)
        {
            if (this.Mode != ControlMode.View)
            {
                var oldValue = this.DBValue;

                // Fora do OnPreRender() este é o único lugar onde o DataLoadInternal() é chamado.
                // Ele tem que ser chamado aqui para que tratamentos do evento Changed possam acessar
                // o novo valor do controle.
                this.DataLoadInternal(false);

                // Este método é chamdo quando o evento Changed dos controles dos quais este controle
                // depende é disparado. Este fato garante que a página está em "RaiseChangedEvents", ou
                // seja, este evento Changed não está sendo disparado "fora da hora".
                if (this.DBValue == null && oldValue != null || this.DBValue != null && !this.DBValue.Equals(oldValue))
                {
                    this.OnChanged(new ChangedEventArgs(this));
                }
            }
        }

        /// <summary>
        ///   Preenche a lista de itens baseado nas propriedades BaseSqlSelect e BaseSqlWhere.
        ///   Se o DBValue antes da chamada deste método não for encontrado na lista, utiliza o
        ///   parâmetro keepCurrentValue para definir o comportamento do método.
        ///   IMPORTANTE: Para se chamar este método, deve-se setar forceLoad=true.
        ///   Isso fará com que ele seja chamado automaticamente em OnPreRender().
        /// </summary>
        /// <param name = "keepCurrentValue">
        ///   Este parâmetro é ignorado caso o valor de DBValue antes da chamada do método existir na lista obtida.
        ///   Caso não exista: 1) se keepCurrentValue for true, a lista será descartada, DBValue será mantido e
        ///   ItemNotFoundException ocorrerá; 2) se keepCurrentValue for false, ResetValue() será chamado.
        /// </param>
        private void DataLoadInternal(bool keepCurrentValue)
        {
            var oldEnabled = this.Enabled;
            try
            {
                this.Enabled = true;

                if (!TControl.InDesignMode(this))
                {
                    if (!this.SettingState && this.Manager != null && this.ColumnName.Length > 0 && this.Mode != ControlMode.Edit)
                    {
                        throw new InvalidOperationException(
                            "O método " + this.UniqueID + ".DataLoad() não é permitido no modo " + this.Mode + " " +
                            "devido ao controle pertencer ao manager " + ((Control)this.Manager).UniqueID
                            );
                    }

                    // Se hasOldValue=true, oldValue contém o valor de DBValue no início deste método.
                    DbObject oldValue = null;
                    var hasOldValue = true;
                    try
                    {
                        oldValue = this.DBValue;
                    }
                    catch
                    {
                        hasOldValue = false;
                    }

                    this.ClearItems();

                    if (this.BaseSqlSelect.ToString() != string.Empty)
                    {
                        

                        string sql;
                        DbObject[] sqlValues;
                        var cn = this.GetConnection();

                        this.GetSQL(cn.Rdbms, out sql, out sqlValues);

                        cn.Open();
                        try
                        {
                            TDataReader dr;
                            try
                            {
                                dr = cn.CreateDataReader(sql, sqlValues);
                            }
                            catch (Exception e)
                            {
                                throw new Exception("Possível erro na sintaxe do SQL do controle " + this.ID + " (" + sql + "): " + e.Message);
                            }

                            this.HasValue = dr.FieldCount > 1;

                            // Determina quais são as colunas de descrição e valor
                            var valueCol = this.BaseDataValueField == string.Empty ? dr.GetName(0) : this.BaseDataValueField;
                            var textCol = this.BaseDataTextField == string.Empty ? (this.HasValue ? dr.GetName(1) : valueCol) : this.BaseDataTextField;

                            try
                            {
                                while (dr.Read())
                                {
                                    var objValue = dr[valueCol];
                                    if (!objValue.IsNull && objValue.Type != this.DataType)
                                    {
                                        throw new InvalidOperationException("A coluna '" + valueCol + "' informada na propriedade " + this.UniqueID + ".SqlSelect possui tipo (" + objValue.Type + ") diferente do informado na propriedade DataType.");
                                    }

                                    var itemText = StrLib.ToStr(dr[textCol], this.Format, this.Culture);
                                    var itemValue = StrLib.ToStr(objValue, string.Empty, this.Culture);

                                    // Permite alteração da descrição default fornecida pela coluna informada na propriedade DataTextField
                                    var args = new SetItemDescriptionEventArgs(dr, itemText, objValue);
                                    this.OnSetItemDescription(args);
                                    itemText = args.Description;

                                    this.Items.Add(new ListItem(itemText, itemValue));
                                }
                            }
                            catch (Exception e)
                            {
                                throw new Exception("Possível erro na definição da propriedade " + this.ID + ".DataTextField " +
                                                    "(" + textCol + ") ou DataValueField (" + valueCol + ")", e);
                            }
                            finally
                            {
                                if (!dr.IsClosed)
                                {
                                    dr.Close();
                                }
                            }
                        }
                        finally
                        {
                            cn.Close();
                        }

                        
                    }
                    else
                    {
                        

                        foreach (var listitem in this.FixedItems)
                        {
                            this.Items.Add(listitem);
                        }

                        
                    }

                    this.CreateReservedItems();

                    this.loaded = true;

                    if (!hasOldValue)
                    {
                        // Seta um valor válido para o controle (DBValue)
                        this.ResetValue();
                    }
                    else if (this.ValueInList(oldValue))
                    {
// Loaded aqui certamente é true
                        this.DBValue = oldValue;
                    }
                    else if (keepCurrentValue)
                    {
                        this.ClearItems();
                        string strOldValue;
                        {
                            if (oldValue == null)
                            {
                                strOldValue = SelectAllValue;
                            }
                            else if (oldValue.IsNull || oldValue.Type == DbType.VarChar && (string)oldValue == string.Empty)
                            {
                                strOldValue = NullValue;
                            }
                            else
                            {
                                strOldValue = StrLib.ToStr(oldValue, string.Empty, this.Culture);
                            }
                        }

                        this.Items.Add(new ListItem(StrLib.ToStr(oldValue, this.Format, this.Culture), strOldValue));
                        this.loaded = true;
                        this.DBValue = oldValue; // Temos certeza agora que oldValue está na lista
                    }
                    else
                    {
                        this.ResetValue();
                    }

                    this.OnDataLoaded(EventArgs.Empty);
                }
            }
            finally
            {
                if (this.Manager == null)
                {
                    this.Enabled = oldEnabled;
                }
            }
        }

        /// <summary>
        ///   Devolve o ListItem correspondente ao valor informado ou null
        ///   caso ele não seja encontrado na lista ou a lista esteja vazia.
        /// </summary>
        private ListItem FindListItem(DbObject value)
        {
            if (!this.Loaded)
            {
                return null;
            }

            string strValue;
            {
                // As propriedades NullAllowed e SelectAllAllowed não são verificadas aqui porque o
                // DataLoadInternal() pode ter forçado a criação de um desses items.
                if (value == null)
                {
                    strValue = SelectAllValue;
                }
                else if (value.IsNull || value.Type == DbType.VarChar && (string)value == string.Empty)
                {
                    strValue = NullValue;
                }
                else
                {
                    strValue = StrLib.ToStr(value, string.Empty, this.Culture);
                }
            }

            return this.Items.FindByValue(strValue);
        }

        /// <summary>
        ///   Se Connection foi informada, devolve uma conexão com base nela.
        ///   Caso contrário, se o controle estiver sob um manager, obtém a conexão do TDataSet associado a ele.
        ///   Caso contrário, devolve connection da TPage.
        ///   Caso contrário, dá exception.
        /// </summary>
        private TConnection GetConnection()
        {
            return CreateConnection(this, this.Connection);
        }

        private ListItem[] GetItemsAsArray(ListItemCollection items)
        {
            var listItems = new ListItem[items.Count];
            var i = 0;
            foreach (ListItem listItem in items)
            {
                listItem.Value = StrLib.ToStr(StrLib.TypeStr(listItem.Value, this.DataType, this.Culture), string.Empty, this.Culture);
                listItems[i++] = listItem;
            }

            return listItems;
        }

        private void GetSQL(Rdbms rdbms, out string sql, out DbObject[] sqlValues)
        {
            var sqlWhere = this.BaseSqlWhere;
            sql = this.BaseSqlSelect.ToString();
            sqlValues = this.BaseSqlWhereValues;
            var sqlOrder = this.BaseSqlOrder;

            try
            {
                GetSqlWhere(null, this.RecordContainer, this.NamingContainer, rdbms, this.RestrictByNullControls, ref sqlWhere, ref sqlValues);
            }
            catch (ControlIdentifier.ControlNotFoundException exc)
            {
                throw new InvalidOperationException("Existe controle inválido informado na propriedade " + this.ID + ".BaseSqlWhere", exc);
            }
            {
// Chama OnSetSqlClauses()
                var args = new SetSqlClausesEventArgs(sqlWhere, sqlValues, sqlOrder);
                this.OnSetSqlClauses(args);
                sqlWhere = args.Where;
                sqlValues = (DbObject[])ArrayList.Adapter(args.WhereValues).ToArray(typeof (DbObject));
                sqlOrder = args.Order;
            }

            if (sqlWhere != string.Empty)
            {
                sql += " WHERE " + sqlWhere;
            }

            if (sqlOrder != string.Empty)
            {
                sql += " ORDER BY " + sqlOrder;
            }
        }

        /// <summary>
        ///   Inicializa a variável defaultValue com o valor selecionado na lista.
        ///   Utilizado no load quando o controle possue uma lista fixa (setado em design-time).
        ///   Poderá ser chamado somente se DataType já estiver setado.
        /// </summary>
        private void InitDefaultValue()
        {
            this.defaultValue = null;

            foreach (var listItem in this.fixedItems)
            {
                if (listItem.Selected)
                {
                    this.defaultValue = StrLib.TypeStr(listItem.Value, this.DataType, this.Culture);
                    break;
                }
            }
        }

        bool IPostBackDataHandler.LoadPostData(string postDataKey, 
                                               NameValueCollection postCollection)
        {
            try
            {
                var valores = postCollection.GetValues(postDataKey);

                if (valores != null)
                {
                    if (string.Compare(valores[0], SelectAllValue) == 0)
                    {
                        if (this.DBValue != null)
                        {
                            this.DBValue = null;
                            return true;
                        }
                    }
                    else if (string.Compare(valores[0], NullValue) == 0)
                    {
                        if (this.DBValue != null && !this.DBValue.IsNull)
                        {
                            this.DBValue = DBNull.Value;
                            return true;
                        }
                    }
                    else
                    {
                        var newValue = this.TypeString(valores[0]);
                        if (!newValue.Equals(this.DBValue))
                        {
                            this.DBValue = newValue;
                            return true;
                        }
                    }
                }

                return false;
            }
            finally
            {
            }
        }

        void IPostBackDataHandler.RaisePostDataChangedEvent()
        {
            this.OnChanged(new ChangedEventArgs(this));
        }

        private int ReservedItemsCount()
        {
            var count = 0;
            if (this.Items.FindByValue(NullValue) != null)
            {
                count++;
            }

            if (this.Items.FindByValue(SelectAllValue) != null)
            {
                count++;
            }

            return count;
        }

        internal class NoItemException : InvalidOperationException
        {
            public NoItemException(TDropDownListBase control) : this("Não foi inserido nenhum item no controle" + (control == null ? string.Empty : " " + control.ID) + ".")
            {
            }

            public NoItemException(string message) : base(message)
            {
            }
        }
    }

    internal delegate void FixedItemsInitEventHandler(object sender, FixedItemsInitEventArgs args);

    internal class FixedItemsInitEventArgs : EventArgs
    {
        private readonly ListItemCollection items;

        internal FixedItemsInitEventArgs(ListItemCollection items)
        {
            this.items = items;
        }

        public ListItemCollection Items
        {
            get
            {
                return this.items;
            }
        }
    }
}