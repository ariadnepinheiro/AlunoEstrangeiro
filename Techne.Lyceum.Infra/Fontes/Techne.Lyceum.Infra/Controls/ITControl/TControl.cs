using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using techne;
using Techne.Data;
using techne.library.sql.structure;

namespace Techne.Controls
{
    public enum ControlMessageType
    {
        NotSet, 
        None, 
        Icon, 
        Label
    }

    public abstract class TControl : WebControl, ITControl, IDependee
    {
        public const string Caption_Def = "?";

        public const string CssClass_Def = "FormField";

        public const bool DBNullRestrictManager_Def = false;

        public const string MessageImageUrl_Def = "~/images/AlertaMens.gif";

        public const string ReadOnlyCssClass_Def = "ReadOnlyField";

        public const string TControlScriptUrl = "~/scripts/TControl.js";

        internal const ControlMessageType MessageControlType_Def = ControlMessageType.NotSet;

        private const string MessageCssClass_Def = "MsgError";

        /// <summary>
        ///   Lista de controles que dependem deste controle
        ///   (controles que referenciam este via SqlWhere).
        /// </summary>
        private readonly ArrayList dependers = new ArrayList();

        private string caption;

        private string columnName;

        private DbType dataType;

        private ArrayList dependentManagers = new ArrayList();

        private string format;

        /// <summary>
        ///   Utilizado para otimizar chamadas à propriedade Manager.
        /// </summary>
        private IContainerManager manager;

        private string messageCssClass;

        private string messageImageUrl;

        private string messageLabel;

        private string msg;

        private string readOnlyCssClass;

        /// <summary>
        ///   Utilizado para otimizar chamadas à propriedade RecordContainer.
        /// </summary>
        private IRecordContainer recordContainer;

        /// <summary>
        ///   Exigido para implementação de IState. Não deve ser acessado diretamente.
        /// </summary>
        private bool settingState;

        public TControl()
        {
            this.PreTControlCtor();

            this.Caption = Caption_Def;
            this.ColumnName = string.Empty;
            this.ControlMessageType = MessageControlType_Def;
            this.CssClass = CssClass_Def;
            this.DataType = DbType.VarChar;
            this.DBNullRestrictManager = DBNullRestrictManager_Def;
            this.DBValue = DBNull.Value; // Deve ser depois de Format e de Mode
            this.DebugMode = false;
            this.MessageCssClass = MessageCssClass_Def;
            this.MessageImageUrl = MessageImageUrl_Def;
            this.MessageLabel = string.Empty;
            this.Msg = string.Empty;
            this.ReadOnlyCssClass = ReadOnlyCssClass_Def;
        }

        [
            Category("Techne"), 
            DefaultValue(Caption_Def), 
            Description("Texto renderizado à esquerda do controle. Informe \"?\" (sem as aspas) para utilizar o nome cadastrado no Cronos."), 
        ]
        public virtual string Caption
        {
            get
            {
                return this.caption;
            }

            set
            {
                this.caption = value == null || value.Trim().Length == 0 ? string.Empty : value;
            }
        }

        [Category("Appearance"), DefaultValue(MessageControlType_Def), Description("Determina o modo como a mensagem de erro é mostrada: ToolTip ou Mensagem")]
        public virtual ControlMessageType ControlMessageType { get; set; }

        [Description("Valor do controle tipado conforme a propriedade DataType."), Bindable(true), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),]
        public virtual DbObject DBValue { get; set; }

        [
            Category("Techne"), 
            DefaultValue(DbType.VarChar), 
            Description("Tipo que a propriedade DBValue devolve."), 
        ]
        public virtual DbType DataType
        {
            get
            {
                return this.dataType;
            }

            set
            {
                if (value == DbType.Null)
                {
                    throw new InvalidOperationException("O valor " + value + " não é permitido.");
                }

                this.dataType = value;
            }
        }

        /// <summary>
        ///   Indica o formato a ser utilizado pelo texto do controle.
        ///   IMPORTANTE: Não force a ordem dia-mês no quando o tipo for DateTime.
        ///   Utilize 'd' ao invés de 'dd/MM/yyyy' para dia sem hora, por exemplo.
        /// </summary>
        [
            Category("Techne"), 
            DefaultValue(""), 
            Description("Indica o formato a ser utilizado pelo texto do controle. " +
                        "IMPORTANTE: Não force a ordem dia-mês no quando o tipo for DateTime. " +
                        "Utilize 'd' ao invés de 'dd/MM/yyyy' para dia sem hora, por exemplo."), 
        ]
        public virtual string Format
        {
            get
            {
                return this.format;
            }

            set
            {
                this.format = value == null ? string.Empty : value;
            }
        }

        [
            DefaultValue(CssClass_Def), 
        ]
        public override string CssClass
        {
            get
            {
                if (InDesignMode(this))
                {
                    return base.CssClass;
                }
                else if (this.Manager != null && this.RecordContainer.Deleted)
                {
                    return this.Manager.DeletedItemCssClass;
                }
                else
                {
                    return this.ReadOnlyCssClass;
                }
            }

            set
            {
                base.CssClass = value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(""), 
            TypeConverter(typeof (ColumnNameConverter))
        ]
        public string ColumnName
        {
            get
            {
                return this.columnName;
            }

            set
            {
                this.columnName = value == null ? string.Empty : value.Trim();
            }
        }

        [Category("Techne"), DefaultValue(DBNullRestrictManager_Def), Description("Indica se o controle restringirá a query do RecordManager quando seu valor (DBValue) for DBNull")]
        public bool DBNullRestrictManager { get; set; }

        [DefaultValue(false)]
        public bool DebugMode { get; set; }

        /// <summary>
        ///   Obtém o manager no qual o controle está inserido.
        ///   Devolve null caso não esteja em nenhum manager.
        /// </summary>
        [Browsable(false)]
        public IContainerManager Manager
        {
            get
            {
                // TODO Deve-se criar uma booleana indicando que o GetManager() já foi chamado, pois se ele devolver null, o próximo acesso a Manager(get) o chamará novamente.
                if (this.manager == null)
                {
                    this.manager = GetManager(this);
                }

                return this.manager;
            }
        }

        [
            Category("Appearance"), 
            DefaultValue(MessageCssClass_Def), 
        ]
        public string MessageCssClass
        {
            get
            {
                return this.messageCssClass;
            }

            set
            {
                this.messageCssClass = value == null ? string.Empty : value.Trim();
            }
        }

        [
            Category("Appearance"), 
            DefaultValue(MessageImageUrl_Def), 
            Editor(typeof (System.Web.UI.Design.ImageUrlEditor), typeof (System.Drawing.Design.UITypeEditor)), 
        ]
        public string MessageImageUrl
        {
            get
            {
                return this.messageImageUrl;
            }

            set
            {
                this.messageImageUrl = value == null ? string.Empty : value;
            }
        }

        [
            Category("Techne"), 
            DefaultValue(""), 
            Description("Controle (System.Web.UI.WebControls.Label) que receberá mensagens. " +
                        "Caso não seja informado, a mensagem será mostrada ao lado do controle."), 
            TypeConverter(typeof (LabelConverter))
        ]
        public string MessageLabel
        {
            get
            {
                return this.messageLabel;
            }

            set
            {
                this.messageLabel = value == null ? string.Empty : value;
            }
        }

        [
            Browsable(false), 
            Description("Mensagem a ser apresentada ao usuário informando erros sobre o conteúdo do controle"), 
            DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), 
        ]
        public string Msg
        {
            get
            {
                return this.msg;
            }

            set
            {
                this.msg = value == null ? string.Empty : value;
            }
        }

        [
            Category("Appearance"), 
            DefaultValue(ReadOnlyCssClass_Def), 
            Description("CssClass para quando o controle estiver no modo ReadOnly")
        ]
        public string ReadOnlyCssClass
        {
            get
            {
                return this.readOnlyCssClass;
            }

            set
            {
                this.readOnlyCssClass = value == null ? string.Empty : value;
            }
        }

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
                    this.recordContainer = GetRecordContainer(this);
                }

                return this.recordContainer;
            }
        }

        protected Control[] Dependers
        {
            get
            {
                return (Control[])this.dependers.ToArray(typeof (Control));
            }
        }

        /// <summary>
        ///   Flag indicador de operações internas. É verificado para permitir certas operações em modos
        ///   que não seriam permitidos para desenvolvedores. Ex: setar DBValue em modo View.
        /// </summary>
        protected bool SettingState
        {
            get
            {
                return this.settingState;
            }
        }

        Control[] IDependee.Dependers
        {
            get
            {
                return this.Dependers;
            }
        }

        /// <summary>
        ///   Utilizado em TControls que contém a propriedade Connection.
        ///   Devolve conexão baseado nesta propriedade, se informada. Caso contrário, tentará obter a conexão
        ///   do manager ao qual o controle pertença, ou da propriedade Connection da TPage.
        /// </summary>
        /// <param name = "control">Controle que possui a propriedade Connection.</param>
        /// <param name = "connectionPropertyValue">A propriedade Connection do TControl.</param>
        public static TConnection CreateConnection(Control control, string connectionPropertyValue)
        {
            return new TConnection(GetConnectionString(control, connectionPropertyValue));
        }

        public static TConnectionWritable CreateWritableConnection(Control control, string connectionPropertyValue)
        {
            var permission = TControl.GetPermission(control);
            return new TConnectionWritable(permission, GetConnectionString(control, connectionPropertyValue));
        }

        /// <summary>
        ///   Encontra o container no qual o controle informado está inserido.
        /// </summary>
        /// <param name = "containerType">O tipo do container a ser encontrado.</param>
        public static Control FindContainer(Control control, Type containerType)
        {
            if (control == null)
            {
                throw new ArgumentNullException();
            }

            var parent = control.Parent;

            while (parent != null && !containerType.IsInstanceOfType(parent))
            {
                parent = parent.Parent;
            }

            return parent;
        }

        /// <summary>
        ///   Faz um FindControl no container informado. Se não encontrar, aumenta o escopo para o
        ///   NamingContainer do container e faz um FindControl novamente. Aumenta o escopo até
        ///   encontrar. Se não encontrar, devolve null.
        /// </summary>
        public static ITControl FindTControl(string controlName, INamingContainer container)
        {
            while (container != null)
            {
                var control = ((Control)container).FindControl(controlName);

                if (control is ITControl)
                {
                    return (ITControl)control;
                }
                else if (control == null)
                {
                    container = ((Control)container).NamingContainer as INamingContainer;
                }
                else
                {
                    throw new ArgumentException("O controle " + controlName + " não é derivado de ITControl.");
                }
            }

            return null;
        }

        /// <summary>
        ///   Obtém os managers filhos do controle informado.
        ///   Caso um filho não seja manager, devolve os filhos deste filho que são, recursivamente.
        /// </summary>
        public static IContainerManager[] GetChildManagers(Control control)
        {
            return (IContainerManager[])TechLib.FindControls(
                typeof (IContainerManager), 
                control, 
                false, 
                new[] { typeof (ITControl) }
                                            );
        }

        /// <summary>
        ///   Devolve todos os IRecordContainer's sob o controle informado.
        /// </summary>
        /// <param name = "selfInclude">Quando true, devolve o próprio controle se ele implementa IRecordContainer.
        ///   Se false, devolve os controles que implementam IRecordContainer e que são filhos do controle informado.</param>
        public static IRecordContainer[] GetChildRecordContainers(Control control, bool selfInclude)
        {
            return (IRecordContainer[])TechLib.FindControls(
                typeof (IRecordContainer), 
                control, 
                selfInclude, 
                new[] { typeof (IRecordContainer), typeof (ITControl) }
                                           );
        }

        /// <summary>
        ///   Busca os controles do tipo TControl dentro do container informado.
        ///   A busca é recursiva, exceto quando o controle encontrado for do tipo TControl ou implementar
        ///   IRecordContainer ou IContainerManager, ou seja, dentro desses controles a busca não é realizada.
        /// </summary>
        public static ITControl[] GetChildTControls(Control container)
        {
            var result = new ArrayList();

            foreach (Control child in container.Controls)
            {
                if (child is ITControl)
                {
                    result.Add(child);
                }
                else if (!(child is IRecordContainer || child is IContainerManager))
                {
                    result.AddRange(GetChildTControls(child));
                }
            }

            return (ITControl[])result.ToArray(typeof (ITControl));
        }

        /// <summary>
        ///   Obtém os controles associados à coluna informada.
        /// </summary>
        /// <param name = "column">Nome da coluna cujos controles deseja-se buscar.</param>
        /// <param name = "editableOnly">Indica que deve devolver somente os controles com ReadOnly=false.</param>
        public static ITControl[] GetChildTControls(Control control, string column, bool editableOnly)
        {
            var result = new ArrayList();

            foreach (var child in GetChildTControls(control))
            {
                if (string.Compare(child.ColumnName, column, true) == 0)
                {
                    if (!editableOnly || (child is ITControlEditable && !((ITControlEditable)child).ReadOnly))
                    {
                        result.Add(child);
                    }
                }
            }

            return (ITControl[])result.ToArray(typeof (ITControl));
        }

        /// <summary>
        ///   Obtém o controle associado à coluna informada.
        ///   Prioriza os controles com propriedade ReadOnly=false.
        ///   Se o controle correspondente não for encontrado, dá exception.
        ///   Se for encontrado mais de um controle, devolve o valor do primeiro deles (evite esta situação
        ///   acessando diretamente a propriedade DBValue do controle).
        /// </summary>
        /// <param name = "container">Controle que será o escopo da busca. Normalmente é IRecordContainer.</param>
        public static ITControl GetColumnControl(Control container, string columnName)
        {
            var controls = GetChildTControls(container, columnName, true);
            if (controls.Length == 0)
            {
                controls = GetChildTControls(container, columnName, false);
            }

            if (controls.Length == 0)
            {
                throw new ArgumentException("O controle relacionado à coluna " + columnName + " no container " + container.ID + " não foi encontrado.");
            }

            return controls[0];
        }

        /// <summary>
        ///   Devolve uma connection string baseado no parâmetro connectionPropertyValue (identificador
        ///   declarado em web.config). Se for uma string vazia, tentará obter a connection string do manager
        ///   ao qual o controle informado pertença, ou da propriedade Connection de TPage.
        /// </summary>
        public static string GetConnectionString(Control control, string connectionPropertyValue)
        {
            var manager = GetManager(control);

            var strConn = string.Empty;
            if (connectionPropertyValue.Length == 0)
            {
                if (manager != null && manager.Table != null && manager.Table.ConnectionString.Length > 0)
                {
                    return manager.Table.ConnectionString;
                }
                else if (control.Page is TPage)
                {
                    strConn = ((TPage)control.Page).Connection;
                }

                if (strConn.Length == 0)
                {
                    throw new InvalidOperationException("A propriedade " + control.UniqueID + ".Connection não foi informada.");
                }
            }
            else
            {
                strConn = connectionPropertyValue;
            }

            var connStr = ConnectionList.GetConnectionString(strConn);
            if (connStr == null)
            {
                connStr = strConn;
            }

            return connStr;
        }

        /// <summary>
        ///   Função que monta uma chamada à função CONTAINS, para busca indexada por palavras. Se
        ///   a expressão com a lista de palavras tiver sintaxe inválida, dispara Exception.
        /// </summary>
        /// <param name = "column">Coluna da tabela onde será feita a busca. Se for null, busca em todas as colunas com texto indexado.</param>
        /// <param name = "expr">Expressão com a lista de palavras a serem procuradas.</param>
        /// <param name = "rdbms">Banco de dados. A sintaxe da função varia de banco pra banco</param>
        /// <returns>Retorna a expressão da chamada à função CONTAINS</returns>
        public static string GetContainsExpression(string column, string expr, Rdbms rdbms)
        {
            string[] elementos;
            int i;
            string pal, pal_ant = null;
            var contexpr = string.Empty;
            var cont_par_abertos = 0;

            if (expr == null || expr.Trim() == string.Empty)
            {
                return null;
            }

            elementos = BreakExpression(expr.Replace("'", string.Empty));
            for (i = 0; i < elementos.Length; i++)
            {
                pal = elementos[i].ToLower();
                if (pal == "não" || pal == "not")
                {
                    pal = "nao";
                }
                else if (pal == "or")
                {
                    pal = "ou";
                }
                else if (pal == "and")
                {
                    pal = "e";
                }
                else if (pal != "e" && pal != "ou" && pal != "nao" && pal != "(" && pal != ")")
                {
                    pal = "pal";
                }

                switch (pal)
                {
                    case "(":
                        if (pal_ant == ")")
                        {
                            throw new Exception("Não é permitido ) antes de (");
                        }
                        else if (pal_ant == "pal")
                        {
                            throw new Exception("Não é permitido usar palavra antes de (");
                        }

                        cont_par_abertos++;
                        contexpr += "(";
                        break;
                    case ")":
                        if (pal_ant == null)
                        {
                            throw new Exception("A expressão não pode começar com )");
                        }
                        else if (pal_ant == "ou")
                        {
                            throw new Exception("Não é permitido usar OU antes de )");
                        }
                        else if (pal_ant == "e")
                        {
                            throw new Exception("Não é permitido usar E antes de )");
                        }
                        else if (pal_ant == "nao")
                        {
                            throw new Exception("Não é permitido usar NÃO antes de )");
                        }
                        else if (pal_ant == "(")
                        {
                            throw new Exception("Não é permitido usar parênteses vazios ()");
                        }

                        cont_par_abertos--;
                        contexpr += ")";
                        break;
                    case "e":
                        if (pal_ant == null)
                        {
                            throw new Exception("A expressão não pode começar com E");
                        }
                        else if (pal_ant == "ou")
                        {
                            throw new Exception("Não é permitido usar OU E");
                        }
                        else if (pal_ant == "e")
                        {
                            throw new Exception("Não é permitido usar E E");
                        }
                        else if (pal_ant == "nao")
                        {
                            throw new Exception("Não é permitido usar NÃO E");
                        }
                        else if (pal_ant == "(")
                        {
                            throw new Exception("Não é permitido usar E após (");
                        }

                        contexpr += " AND ";
                        break;
                    case "ou":
                        if (pal_ant == null)
                        {
                            throw new Exception("A expressão não pode começar com OU");
                        }
                        else if (pal_ant == "ou")
                        {
                            throw new Exception("Não é permitido usar OU OU");
                        }
                        else if (pal_ant == "e")
                        {
                            throw new Exception("Não é permitido usar E OU");
                        }
                        else if (pal_ant == "nao")
                        {
                            throw new Exception("Não é permitido usar NÃO OU");
                        }
                        else if (pal_ant == "(")
                        {
                            throw new Exception("Não é permitido usar OU após (");
                        }

                        contexpr += " OR ";
                        break;
                    case "nao":
                        if (pal_ant == null)
                        {
                            throw new Exception("A expressão não pode começar com NÃO");
                        }
                        else if (pal_ant != "e")
                        {
                            throw new Exception("A palavra NÃO só é permitida depois de E");
                        }

                        if (rdbms == Rdbms.Oracle)
                        {
                            // Tira o AND do final e põe ~, que significa NOT e AND NOT no Oracle
                            if (contexpr.Length > 4 && contexpr.Substring(contexpr.Length - 5, 5) == " AND ")
                            {
                                contexpr = contexpr.Substring(0, contexpr.Length - 5);
                            }

                            contexpr += " ~ ";
                        }
                        else
                        {
                            contexpr += " NOT ";
                        }

                        break;
                    default:
                        if (pal_ant == "pal" || pal_ant == ")")
                        {
                            contexpr += " AND ";
                        }

                        if (elementos[i].Replace("\"", string.Empty).Length == 1)
                        {
                            throw new Exception("Não é permitido palavra com uma só letra");
                        }
                        else if (elementos[i].Replace("\"", string.Empty).Length > 0 && elementos[i].Substring(0, 1) == "*")
                        {
                            throw new Exception("Não é permitido palavra começando com *");
                        }
                        else if (elementos[i].Replace("\"", string.Empty).Length > 0 && elementos[i].Substring(elementos[i].Length - 1, 1) == "*")
                        {
                            if (rdbms == Rdbms.SQLServer || rdbms == Rdbms.Unknown)
                            {
                                elementos[i] = "\"" + elementos[i].Replace("\"", string.Empty).TrimEnd('*') + "*\"";
                            }
                            else if (rdbms == Rdbms.Oracle)
                            {
                                elementos[i] = "\"" + elementos[i].Replace("\"", string.Empty).TrimEnd('*') + "%\"";
                            }
                        }
                        else
                        {
                            elementos[i] = "\"" + elementos[i].Replace("\"", string.Empty) + "\"";
                        }

                        contexpr += elementos[i];
                        break;
                }

                pal_ant = pal;
            }

            if (pal_ant == "ou")
            {
                throw new Exception("A expressão não pode terminar com OU");
            }
            else if (pal_ant == "e")
            {
                throw new Exception("A expressão não pode terminar com E");
            }
            else if (pal_ant == "(")
            {
                throw new Exception("A expressão não pode terminar com (");
            }

            if (cont_par_abertos > 0)
            {
                throw new Exception("Falta fechar " + cont_par_abertos + " parênteses");
            }
            else if (cont_par_abertos < 0)
            {
                throw new Exception("Foram fechados " + (-cont_par_abertos) + " parênteses a mais");
            }

            contexpr = contexpr.Replace("'", "''");
            if (rdbms == Rdbms.SQLServer || rdbms == Rdbms.Unknown)
            {
                contexpr = "Contains(" + (column == null || column.Trim() == string.Empty ? "*" : column) + ",'" + contexpr + "')";
            }
            else if (rdbms == Rdbms.Oracle)
            {
                contexpr = "(Contains(" + (column == null || column.Trim() == string.Empty ? "*" : column) + ",'" + contexpr + "')>0)";
            }

            return contexpr;
        }

        public static DataTable GetDesignTimeDataTable(Control control, string dataMember)
        {
            DataTable result = null;

            // Obtém o valor da propriedade DataSource (em design-time).
            var propColl = TypeDescriptor.GetProperties(control);
            var property = propColl["DataSource"];
            var dataSourceName = (string)property.GetValue(control);

            var componentSite = control.Site;
            if (componentSite == null)
            {
                throw new Exception();
            }

            var container = (IContainer)componentSite.GetService(typeof (IContainer));
            if (container == null)
            {
                throw new Exception();
            }

            if (dataSourceName != string.Empty)
            {
                object oDataSource = container.Components[dataSourceName];

                if (oDataSource is DataSet)
                {
                    var dataSource = (DataSet)oDataSource;

                    if (dataMember != string.Empty)
                    {
                        if (!dataSource.Tables.Contains(dataMember))
                        {
                            result = null;
                        }
                        else
                        {
                            result = dataSource.Tables[dataMember];
                        }
                    }
                }
                else if (oDataSource is DataTable)
                {
                    result = (DataTable)oDataSource;
                }
                else
                {
                    result = null;
                }
            }
            else
            {
                var props = TypeDescriptor.GetProperties(control);
                var classProperty = props["DataSourceClass"];
                var dataSourceClass = string.Empty + property.GetValue(control);
                if (dataSourceClass.Length == 0 && control is RecordManager)
                {
                    dataSourceClass = ((RecordManager)control).DataSourceClass;
                }

                var table = TUtil.CreateInstance(dataSourceClass, control.Site);
                if (table is TDataTable)
                {
                    result = (TDataTable)table;
                }
            }

            return result;
        }

        /// <summary>
        ///   Obtém o manager no qual o controle está inserido.
        ///   Devolve null caso não esteja em nenhum.
        ///   Se o próprio controle informado for um manager, não leva isso em consideração, ou seja,
        ///   devolverá o manager no qual o manager informado estiver inserido.
        /// </summary>
        public static IContainerManager GetManager(Control control)
        {
            return (IContainerManager)FindContainer(control, typeof (IContainerManager));
        }

        /// <summary>
        ///   Obtém o container no qual o controle está inserido.
        ///   Devolve null caso não esteja em nenhum.
        /// </summary>
        public static IRecordContainer GetRecordContainer(Control control)
        {
            return (IRecordContainer)FindContainer(control, typeof (IRecordContainer));
        }

        public static IContainerManager GetRootManager(Control control)
        {
            var root = GetManager(control);

            while (root != null && !root.IsRoot)
            {
                root = GetManager((Control)root);
            }

            return root;
        }

        /// <summary>
        ///   Monta uma cláusula Where válida, substituindo os tokens indicadores de ITControl's (#controle#)
        ///   pela propriedade DBValue respectiva, tratando os casos em que DBValue == null e DBValue is DBNull.
        ///   Devolve uma lista de colunas que estão sendo restritas por um valor fixo ([coluna] = valor ou [coluna] IS NULL).
        /// </summary>
        /// <param name = "scope">
        ///   Se a cláusula where continver controles delimitados por '#', deve-se indicar o escopo (Control)
        ///   onde a busca pelo nome do controle deverá ser realizada. Normalmente indica-se o NamingContainer
        ///   no qual o controle (que chama o GetSqlWhere) está inserido. Se estivermos no nível da página,
        ///   NamingContainer é null, situação na qual a própria Page deve ser informada.
        ///   Informe null se não existirem referências a controles.
        /// </param>
        /// <param name = "rdbms">
        ///   Tipo do banco no qual a cláusula where será utilizada. Normalmente utiliza-se a propriedade Rdbms
        ///   da TConnection através da qual a consulta será realizada.
        /// </param>
        /// <param name = "RestrictByNullControls">
        ///   Quando false, remove todas as expressões do tipo [coluna] = [valor] quando valor for NULL.
        /// </param>
        public static string[] GetSqlWhere(Control scope, Rdbms rdbms, 
                                           bool RestrictByNullControls, 
                                           ref string sqlWhere, ref DbObject[] whereValues)
        {
            return GetSqlWhere(null, null, scope, rdbms, RestrictByNullControls, ref sqlWhere, ref whereValues);
        }

        /// <summary>
        ///   Dado uma cláusula where contendo parâmetros OleDb identificados por '?', controles delimitados por '#',
        ///   colunas delimitadas por '$', modifica-a de forma a transformar todos as referências a controles e
        ///   colunas em parâmetros OleDb.
        /// </summary>
        /// <param name = "table">
        ///   Quando informado, substitui todas as expressões do tipo [coluna] = [valor] por [tabela].[coluna] = [valor].
        /// </param>
        /// <param name = "container">
        ///   Se a cláusula where contiver colunas delimitadas por '$', deve-se indicar o container no qual os
        ///   valores deverão ser encontrados. Informe null se não existirem referências a colunas.
        /// </param>
        /// <param name = "scope">
        ///   Se a cláusula where continver controles delimitados por '#', deve-se indicar o escopo (Control)
        ///   onde a busca pelo nome do controle deverá ser realizada. Normalmente indica-se o NamingContainer
        ///   no qual o controle (que chama o GetSqlWhere) está inserido. Se estivermos no nível da página,
        ///   NamingContainer é null, situação na qual a própria Page deve ser informada.
        ///   Informe null se não existirem referências a controles.
        /// </param>
        /// <param name = "rdbms">
        ///   Tipo do banco no qual a cláusula where será utilizada. Normalmente utiliza-se a propriedade Rdbms
        ///   da TConnection através da qual a consulta será realizada.
        /// </param>
        /// <param name = "RestrictByNullControls">
        ///   Quando false, remove todas as expressões do tipo [coluna] = [valor] quando valor for NULL.
        /// </param>
        public static string[] GetSqlWhere(TDataTable table, 
                                           IRecordContainer container, 
                                           Control scope, 
                                           Rdbms rdbms, 
                                           bool RestrictByNullControls, 
                                           ref string sqlWhere, 
                                           ref DbObject[] whereValues)
        {
            string[] columnsInWhere, fixedColumns;
            GetSqlWhere(
                table, container, scope, rdbms, RestrictByNullControls, 
                ref sqlWhere, ref whereValues, 
                out columnsInWhere, out fixedColumns
                );
            return fixedColumns;
        }

        /// <summary>
        ///   Dado uma cláusula where contendo parâmetros OleDb identificados por '?', controles delimitados por '#',
        ///   colunas delimitadas por '$', modifica-a de forma a transformar todos as referências a controles e
        ///   colunas em parâmetros OleDb.
        /// </summary>
        /// <param name = "table">
        ///   Quando informado, substitui todas as expressões do tipo [coluna] = [valor] por [tabela].[coluna] = [valor].
        /// </param>
        /// <param name = "container">
        ///   Se a cláusula where contiver colunas delimitadas por '$', deve-se indicar o container no qual os
        ///   valores deverão ser encontrados. Informe null se não existirem referências a colunas.
        /// </param>
        /// <param name = "scope">
        ///   Se a cláusula where continver controles delimitados por '#', deve-se indicar o escopo (Control)
        ///   onde a busca pelo nome do controle deverá ser realizada. Normalmente indica-se o NamingContainer
        ///   no qual o controle (que chama o GetSqlWhere) está inserido. Se estivermos no nível da página,
        ///   NamingContainer é null, situação na qual a própria Page deve ser informada.
        ///   Informe null se não existirem referências a controles.
        /// </param>
        /// <param name = "rdbms">
        ///   Tipo do banco no qual a cláusula where será utilizada. Normalmente utiliza-se a propriedade Rdbms
        ///   da TConnection através da qual a consulta será realizada.
        /// </param>
        /// <param name = "RestrictByNullControls">
        ///   Quando false, remove todas as expressões do tipo [coluna] = [valor] quando valor for NULL.
        /// </param>
        /// <param name = "columnsInWhere">
        ///   Lista de colunas que aparecem no parâmetro sqlWhere. As colunas estão no formato 'tabela.coluna'.
        /// </param>
        /// <param name = "fixedColumns">
        ///   Lista de colunas que estão sendo restritas por um valor fixo ([coluna] = valor ou [coluna] IS NULL).
        ///   As colunas estão no formato 'tabela.coluna'.
        ///   Esta lista está contida na lista devolvida pelo parâmetro columnsInWhere.
        /// </param>
        public static void GetSqlWhere(TDataTable table, 
                                       IRecordContainer container, 
                                       Control scope, 
                                       Rdbms rdbms, 
                                       bool RestrictByNullControls, 
                                       ref string sqlWhere, 
                                       ref DbObject[] whereValues, 
                                       out string[] columnsInWhere, 
                                       out string[] fixedColumns)
        {
            if (sqlWhere.Trim() == string.Empty)
            {
                whereValues = new DbObject[0];
                columnsInWhere = new string[0];
                fixedColumns = new string[0];
                return;
            }

            var where = sqlWhere;
            var e = RootPointer.parse(where, Language.CSharp);

            // Transforma DbObject[] em object[], entretanto os elementos ainda são DbObject.
            // Aqui não deve ser utilizado DbObject.ToObjectArray().
            e.setParametersValues(whereValues);

            if (table != null)
            {
                ReplaceColumnIdentifiers(e, table);
            }

            var array = TechLib.EnumerableItemMethod(e.selectByType(JLib.typeToClass(typeof (ColumnIdentifier))), "ToString");
            columnsInWhere = array.Length == 0 ? new string[0] : (string[])array;

            // Trata #controle#
            ReplaceControlIdentifiers(e, table, scope, rdbms, RestrictByNullControls);

            // Trata $campo$
            if (container != null)
            {
                ReplaceColumnValueIdentifiers(e, container);
            }

            // Trata @parametro@
            ReplacePageParameterIdentifiers(e, scope.Page);

            // Esta funcionalidade foi implementada aqui no TControl ao invés do DataSet porque o parse
            // já está feito aqui. Se fosse no DataSet, além de ter que fazer o parse novamente, ele acabaria
            // limitando a sintaxe do where (consultas feitas diretamente no DataSet, não através de controles).
            if (table != null)
            {
                SetColumnsWithoutTable(e, table);
            }

            if (e.getRoot() == null)
            {
                sqlWhere = string.Empty;
                whereValues = new DbObject[0];
            }
            else
            {
                sqlWhere = e.ToString();

                // e.getParameterValues() devolve object[], mas seus elementos são DbObject.
                // DbObject.ToDbObjectArray() não funcionaria aqui.
                var parameterValues = e.getParametersValues();
                whereValues = new DbObject[parameterValues.Length];
                for (var i = 0; i < parameterValues.Length; i++)
                {
                    whereValues[i] = (DbObject)parameterValues[i];
                }
            }

            // Obtém a lista das colunas restritas por um valor
            var list = new ArrayList();
            foreach (ColumnIdentifier columnIdentifier in e.selectByType(JLib.typeToClass(typeof (ColumnIdentifier))))
            {
                if (columnIdentifier.getParent() is CompareOperation)
                {
                    var compareOperation = (CompareOperation)columnIdentifier.getParent();
                    var expression = compareOperation.getRightExpression();
                    if (compareOperation.getOperation() == ComparsionOperator.Equal && expression is ParameterIdentifier)
                    {
                        list.Add(
                            (columnIdentifier.getTable().Length == 0 ? string.Empty : columnIdentifier.getTable() + ".") +
                            columnIdentifier.getName()
                            );
                    }
                }
            }

            fixedColumns = (string[])list.ToArray(typeof (string));
        }

        /// <summary>
        ///   Se fornecida uma string no formato "#controle#", devolve o controle correspondente.
        ///   Se a string estiver no formato "$campo$", devolve o controle associado ao campo. Neste caso,
        ///   se houver mais de controle associado ao campo, dá exception.
        /// </summary>
        public static ITControl GetTControl(string controlOrFieldName, INamingContainer namingContainer)
        {
            if (controlOrFieldName == null)
            {
                throw new ArgumentNullException();
            }

            if (controlOrFieldName.Length > 2 &&
                (controlOrFieldName[0] == '$' && controlOrFieldName[controlOrFieldName.Length - 1] == '$' ||
                 controlOrFieldName[0] == '#' && controlOrFieldName[controlOrFieldName.Length - 1] == '#'))
            {
                var field = controlOrFieldName[0] == '$';

                controlOrFieldName = controlOrFieldName.Substring(1, controlOrFieldName.Length - 2);

                if (field)
                {
                    var childControls = TControl.GetChildTControls((Control)namingContainer, controlOrFieldName, false);
                    if (childControls.Length == 0)
                    {
                        throw new ArgumentException("O campo '" + controlOrFieldName + "' não possui controle associado no container.");
                    }

                    if (childControls.Length > 1)
                    {
                        throw new ArgumentException("O campo '" + controlOrFieldName + "' possui mais de um controle associado no container.");
                    }

                    return childControls[0];
                }
                else
                {
                    var control = FindTControl(controlOrFieldName, namingContainer);
                    if (control == null)
                    {
                        throw new InvalidOperationException("O controle '" + controlOrFieldName + "' não foi encontrado no container.");
                    }

                    return control;
                }
            }
            else
            {
                throw new ArgumentException("O parâmetro informado deve estar no formato #controle# ou $campo$.");
            }
        }

        /// <summary>
        ///   Obtém um array de valores correspondente às propriedades DBValues de uma série de TControl's
        ///   contidos em um array. Caso o controle seja null, a posição correspondente do array conterá
        ///   DBNull (o array devolvido sempre conterá o mesmo número de posições do array informado).
        /// </summary>
        public static DbObject[] GetValues(TControl[] controls)
        {
            var values = new DbObject[controls.Length];

            for (var i = 0; i < controls.Length; i++)
            {
                if (controls[i] == null)
                {
                    values[i] = DBNull.Value;
                }
                else
                {
                    values[i] = controls[i].DBValue;
                }
            }

            return values;
        }

        public static bool InDesignMode(Control control)
        {
            return control.Page != null && control.Page.Site != null && control.Page.Site.DesignMode;
        }

        public static void RegisterTControlScript(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException();
            }

            var page = control.Page;
            if (page == null || page.ClientScript.IsClientScriptBlockRegistered(typeof (TControl), "TControl"))
            {
                return;
            }

            page.ClientScript.RegisterClientScriptBlock(typeof (TControl), "TControl", 
                                                        "<script language=\"javascript\">\n" +
                                                        "  var MessageCssClass_Def = \"" + MessageCssClass_Def + "\";\n" +
                                                        "  var MessageImageUrl_Def = \"" + TUtil.TranslateRelativeUrl(MessageImageUrl_Def) + "\";\n" +
                                                        "</script>\n" +
                                                        "<script language=\"javascript\" src=\"" + TUtil.TranslateRelativeUrl(TControlScriptUrl, control) + "\">" +
                                                        "</script>\n"
                );
        }

        public static void ReplaceControlIdentifiers(RootPointer root, 
                                                     TDataTable table, Control scope, Rdbms rdbms, 
                                                     bool RestrictByNullControls)
        {
            foreach (ControlIdentifier identifier in root.selectByType(JLib.typeToClass(typeof (ControlIdentifier))))
            {
                Control control = null;
                var controlScope = scope;
                while (controlScope != null && control == null)
                {
                    control = controlScope.FindControl(identifier.getName());
                    if (control == null)
                    {
                        controlScope = controlScope.NamingContainer;
                    }
                }

                if (control == null)
                {
                    throw new ControlIdentifier.ControlNotFoundException(identifier, "Controle " + identifier.getName() + " não encontrado");
                }

                var tcontrol = control as TControl;
                if (tcontrol == null)
                {
                    // se o controle não é um TControl
                    // busca a propriedade de valor do controle indicada pelo atributo ControlValueProperty
                    ControlValuePropertyAttribute ctrlValueAttr = null;
                    var attr = control.GetType().GetCustomAttributes(typeof (ControlValuePropertyAttribute), true);
                    if (attr != null && attr.Length > 0)
                    {
                        ctrlValueAttr = attr[0] as ControlValuePropertyAttribute;
                    }

                    if (ctrlValueAttr == null)
                    {
                        throw new NotSupportedException("O controle " + control.ID + " não é do tipo ITControl, nem possui o atributo ControlValueProperty");
                    }

                    // pega o valor da propriedade
                    object propValue = null;
                    var propInfo = control.GetType().GetProperty(ctrlValueAttr.Name);
                    if (propInfo != null && propInfo.GetGetMethod() != null)
                    {
                        propValue = propInfo.GetGetMethod().Invoke(control, new object[] { });
                    }

                    DbObject controlValue = DBNull.Value;
                    try
                    {
                        if (propValue != null)
                        {
                            if (!DbObject.CanConvertToDbObject(propValue))
                            {
                                throw new NotSupportedException("A propriedade " + propInfo.Name + " do controle " + control.ID + " não é de um tipo aceito por RecordManagers");
                            }

                            controlValue = DbObject.ToDbObject(propValue);
                        }

                        if (controlValue == null)
                        {
                            controlValue = DBNull.Value;
                        }
                    }
                    catch
                    {
                        throw new NotSupportedException("A propriedade " + propInfo.Name + " do controle " + control.ID + " não pôde ser lida.");
                    }

                    if (controlValue.IsNull)
                    {
                        if (RestrictByNullControls)
                        {
                            identifier.replace(new NullIdentifier());
                        }
                        else
                        {
                            identifier.remove();
                        }
                    }
                    else
                    {
                        identifier.replace(new ParameterIdentifier(controlValue));
                    }
                }
                else if (tcontrol.RecordContainer != null &&
                         tcontrol.RecordContainer.PrimaryKeyValues == null &&
                         tcontrol.RecordContainer.Mode == RecordManagerMode.View)
                {
                    // Controle é um TControl contido dentro de um container vazio em modo View
                    // Teoricamente um controle nessa situação tem seu DBValue igual a DBNull, mas vamos garantir isso aqui.
                    identifier.replace(new NullIdentifier());
                }

                    // Tratamento dos controle que são indexados (utilizam cláusula contains do DB)
                else if (control is TTextBox && ((TTextBox)control).IndexedColumn)
                {
                    if (!(identifier.getParent() is CompareOperation))
                    {
                        throw new InvalidOperationException("Só operações de comparação são permitidas com controles do tipo TTextBox com propriedade IndexedColumn true");
                    }

                    var opCompare = (CompareOperation)identifier.getParent();
                    var leftExpression = opCompare.getLeftExpression().ToString();

                    if (!(opCompare.getLeftExpression() is Identifier))
                    {
                        throw new InvalidOperationException(
                            "Ao realizar comparações com controles do tipo TTextBox com propriedade IndexedColumn true, " +
                            "somente colunas são permitidas. A expressão \"" + leftExpression + "\" é inválida."
                            );
                    }

                    var srb = (TTextBox)control;
                    srb.Msg = string.Empty;

                    if (table != null)
                    {
                        leftExpression = ((TDataColumn)table.Columns[leftExpression]).FullCol;
                    }
                    {
                        var expContains = "(0 = 1)";
                        if (srb.DBValue.Type != DbType.Null)
                        {
                            try
                            {
                                expContains = GetContainsExpression(leftExpression, StrLib.ToStr(srb.DBValue), rdbms);
                            }
                            catch (Exception exc)
                            {
                                srb.Msg = exc.Message;
                            }
                        }

                        // TODO O correto seria parsear expContains com ExpressionParser
                        opCompare.replace(new UnknownElement(expContains));
                    }
                }
                else
                {
                    if (tcontrol is TDropDownListBase && tcontrol.Page != null && !tcontrol.Page.IsPostBack)
                    {
                        var ddl = (TDropDownListBase)tcontrol;
                        if ((ddl.DBValue == null || ddl.DBValue == DBNull.Value) && ddl.Items.Count == 0)
                        {
                            ddl.DataLoad();
                        }
                    }

                    var controlValue = tcontrol.DBValue;

                    if (controlValue == null)
                    {
                        // Situação possível para TDropDownList (.DBValue == null)
                        identifier.remove();
                    }
                    else if (controlValue.IsNull)
                    {
                        if (RestrictByNullControls)
                        {
                            identifier.replace(new NullIdentifier());
                        }
                        else
                        {
                            identifier.remove();
                        }
                    }
                    else
                    {
                        identifier.replace(new ParameterIdentifier(controlValue));
                    }
                }
            }
        }

        public static void ReplacePageParameterIdentifiers(RootPointer root, Page page)
        {
            var parameters = root.selectByType(JLib.typeToClass(typeof (PageParameterIdentifier)));
            if (parameters.Length == 0)
            {
                return;
            }

            if (!(page is TPage))
            {
                throw new ApplicationException("A página não deriva de TPage.");
            }

            foreach (PageParameterIdentifier parameter in parameters)
            {
                var parameterValue = ((TPage)page).Parameters[parameter.getPageParameter()];
                if (parameterValue == null)
                {
                    // Se o parâmetro não for informado, não restringe.
                    parameter.remove();
                }
                else
                {
                    parameter.getParent().replace(
                        parameter, 
                        new ParameterIdentifier(parameterValue)
                        );
                }
            }
        }

        public static void WriteDebugInfo(HtmlTextWriter writer, Control control)
        {
            writer.Write("<FONT COLOR=lightblue><b>[" + control.UniqueID + " (" + control.GetType().Name + ")]</b></FONT><br/>");
        }

        /// <summary>
        ///   Copia propriedades de um TControl para um WebControl.
        ///   Utilizado para renderização. Nesta classe (TControl) devem ser copiadas somente as
        ///   propriedades comuns aos WebControl's.
        /// </summary>
        public virtual void CopyProperties(WebControl target)
        {
            // CopyFrom() copia BackColor, BorderColor, BorderStyle, BorderWidth,
            // CssClass, Font, ForeColor, Height e Width. Entretanto, este método
            // não está chamando o override do get da propriedade.
            // Este problema foi detectado na cópia de CssClass.
            // Portanto, dentre todas as propriedades citadas acima, as que são
            // overriden devem ser recopiadas logo abaixo.
            target.ControlStyle.CopyFrom(this.ControlStyle);

            // Veja observação de CopyFrom().
            target.CssClass = this.CssClass;

            target.AccessKey = this.AccessKey;
            target.Enabled = this.Enabled;
            target.EnableViewState = this.EnableViewState;
            target.TabIndex = this.TabIndex;
            target.ToolTip = this.ToolTip;
            target.Visible = this.Visible;

            target.Attributes.Clear();
            foreach (string key in this.Attributes.Keys)
            {
                target.Attributes.Add(key, this.Attributes[key]);
            }
        }

        public virtual void InitFromParameters(string parameter)
        {
            var page = this.Page as TPage;
            if (page == null || page.IsPostBack)
            {
                return;
            }

            if (page.Parameters.Contains(parameter))
            {
                var parameterValue = page.Parameters[parameter];
                if (parameterValue == null)
                {
                    throw new InvalidOperationException(
                        "O valor do parâmetro '" + parameter + "' (null) é inválido no controle " + this.ID + ". " +
                        "Ele é permitido somente em TDropDownList's."
                        );
                }

                this.DBValue = parameterValue;
            }
        }

        public virtual void ResetValue()
        {
            this.DBValue = DBNull.Value;
        }

        public string GetCaption(string idioma)
        {
            return GetCaption(this, this.Manager, idioma);
        }

        /// <summary>
        ///   Dado um controle, obtém seu caption.
        ///   Se a propriedade Caption for '?', busca o caption cadastrado no Cronos para a coluna correspondente.
        ///   Como o manager é passado como parâmetro, a consulta funciona mesmo se o controle não pertencer
        ///   fisicamente ao manager (por exemplo: na montagem do help dinâmico, ao buscar o caption para um
        ///   controle dentro de um TemplateColumn).
        /// </summary>
        internal static string GetCaption(ITControl control, IContainerManager manager, string idioma)
        {
            if (control.Caption != "?")
            {
                return control.Caption;
            }
            else if (control.ColumnName.Length > 0 && manager != null && manager.Table != null)
            {
                var column = (TDataColumn)manager.Table.Columns[control.ColumnName];
                return column.GetCaption(idioma);
            }
            else
            {
                return string.Empty;
            }
        }

        internal static TPermission GetPermission(Control control)
        {
            if (control.Page is TPage)
            {
                return ((TPage)control.Page).Permission;
            }
            else
            {
                return null;
            }
        }

        internal static TPermission GetPermission(IRecordContainer container)
        {
            return GetPermission(container.Manager);
        }

        internal static TPermission GetPermission(IContainerManager manager)
        {
            return GetPermission((Control)manager);
        }

        internal virtual void LoadViewStateInternal(object savedState)
        {
            this.settingState = true;
            try
            {
                var state = (Pair)savedState;

                this.DataType = (DbType)state.First;

                // Somente recupera DBValue do estado se:
                if (this.RecordContainer == null || // Não estiver em container
                    this.RecordContainer.Mode == RecordManagerMode.New || // Está em modo New (PrimaryKeyValue.Length == 0)
                    this.RecordContainer.PrimaryKeyValues != null)
                {
// Tem registro no container
                    // O DBNull é guardado como "true" (exige menos bytes para persistência).
                    this.DBValue = state.Second is bool ? DBNull.Value : DbObject.ToDbObject(state.Second);
                }
            }
            finally
            {
                this.settingState = false;
            }
        }

        internal virtual object SaveViewStateInternal()
        {
            var value = this.DBValue;
            var pvalue = value == null ? null : value.ToObject();

            // O DBNull é guardado como "true" (exige menos bytes para persistência).
            return new Pair(
                this.DataType, 
                pvalue is DBNull ? true : pvalue
                );
        }

        protected virtual string GetMessageCssClass()
        {
            return this.MessageCssClass;
        }

        /// <summary>
        ///   Permite que sejam realizadas inicializações nas classes derivadas
        ///   antes do construtor do TControl.
        /// </summary>
        protected virtual void PreTControlCtor()
        {
            this.Format = string.Empty; // Deve ser antes de DBValue
        }

        protected virtual void RenderCaption(HtmlTextWriter writer)
        {
            var caption = this.GetCaption(Thread.CurrentThread.CurrentCulture.Name);
            if (caption.Length > 0)
            {
                var lbl = new Label();
                lbl.Text = caption + "&nbsp;";
                lbl.RenderControl(writer);
            }
        }

        protected virtual void RenderControlViewMode(HtmlTextWriter writer)
        {
            var lbl = new Label();
            this.CopyProperties(lbl);
            lbl.Text = this.ToString(this.DBValue);
            lbl.CssClass = this.CssClass;
            lbl.RenderControl(writer);
        }

        protected virtual void RenderDebugInfo(HtmlTextWriter writer)
        {
            WriteDebugInfo(writer, this);

            writer.Write("<B>DBValue: </B>");
            if (this.DBValue == null)
            {
                writer.Write("&lt;SelectAll&gt;");
            }
            else if (this.DBValue.Type == DbType.Null)
            {
                writer.Write("&lt;Null&gt;");
            }
            else
            {
                writer.Write(this.ToString(this.DBValue));
            }

            writer.Write("<BR/>");

            writer.Write("<B>DataType: </B>" + this.DataType + "<BR/>");

            DependerLib.WriteDebugInfo(writer, this);
        }

        protected virtual void RenderExtra(HtmlTextWriter writer)
        {
        }

        protected virtual void RenderMessage(HtmlTextWriter writer)
        {
            if (this.ControlMessageType == ControlMessageType.None || this.Msg.Length == 0)
            {
                return;
            }

            if (this.MessageLabel != string.Empty)
            {
                try
                {
                    var lblu = (Label)this.Page.FindControl(this.MessageLabel);
                    if (lblu != null)
                    {
                        lblu.Text = this.Msg;
                        return;
                    }
                }
                catch
                {
                }
            }

            if (this.ControlMessageType == ControlMessageType.Label)
            {
                var lbl = new HtmlGenericControl("SPAN");
                lbl.Attributes.Add("class", this.GetMessageCssClass());
                lbl.ID = this.UniqueID + "_msg"; // Usei UniqueID para o caso de INamingContainers
                lbl.Controls.Add(new LiteralControl("&nbsp;" + HttpUtility.HtmlEncode(this.Msg)));
                lbl.RenderControl(writer);
            }
            else
            {
                var img = new HtmlImage();
                img.Src = TUtil.TranslateRelativeUrl(this.MessageImageUrl, this);
                img.Attributes.Add("title", this.Msg);
                img.ID = this.UniqueID + "_msg"; // Usei UniqueID para o caso de INamingContainers
                img.RenderControl(writer);
            }
        }

        protected virtual void RenderSpecific(HtmlTextWriter writer)
        {
            this.RenderControlViewMode(writer);
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

        protected override void OnInit(EventArgs e)
        {
            if (!InDesignMode(this))
            {
                if (this.RecordContainer != null && this.ColumnName.Length > 0)
                {
                    ((IRecordContainerInternal)this.RecordContainer).RegisterControl(this);
                }
            }

            base.OnInit(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            var dbValue = this.DBValue;
            this.Attributes["DBValue"] = dbValue == null ? null : dbValue.ToString(CultureInfo.InvariantCulture);

            // Atributo datatype
            var dataType = this.DataType;
            if (dataType == DbType.Number)
            {
                this.Attributes["DataType"] = "double";
            }
            else if (dataType == DbType.Date)
            {
                this.Attributes["DataType"] = "date";
            }

// else if(dataType == typeof(short) || dataType == typeof(int) || dataType == typeof(long))

// Attributes["DataType"] = "integer";
        }

        protected override void Render(HtmlTextWriter w)
        {
            var swriter = new StringWriter();
            var writer = new HtmlTextWriter(swriter);

            try
            {
                if (!InDesignMode(this) && this.DebugMode)
                {
                    writer.Write("<TABLE BORDER=1 CELLSPACING=0 CELLPADDING=3><TR><TD>");
                    writer.Write("<FONT FACE=verdana SIZE=1 COLOR=gray>");
                    this.RenderDebugInfo(writer);
                    writer.Write("</FONT>");
                }

                bool gridlayout;

                
                {
                    gridlayout = false;

                    var itensstyle = new StringCollection();
                    itensstyle.AddRange(new[] { "top", "left", "z-index", "position" });

                    var keys = new string[this.Style.Count];
                    this.Style.Keys.CopyTo(keys, 0);
                    foreach (var k in keys)
                    {
                        if (itensstyle.Contains(k.ToLower()))
                        {
                            writer.AddStyleAttribute(k.ToLower(), this.Style[k]);
                            this.Style.Remove(k);
                            gridlayout = true;
                        }
                    }

                    if (gridlayout)
                    {
                        writer.AddStyleAttribute("white-space", "nowrap");
                        writer.RenderBeginTag("span");
                    }
                }

                

                if (this.Msg.Length == 0)
                {
                    if (this.ControlMessageType == ControlMessageType.NotSet)
                    {
                        this.ControlMessageType = ControlMessageType.Icon;
                    }

                    this.Attributes.Add("ControlMessageType", this.ControlMessageType.ToString());
                    if (this.MessageCssClass != MessageCssClass_Def)
                    {
                        this.Attributes.Add("MessageCssClass", this.MessageCssClass);
                    }

                    if (this.MessageImageUrl != MessageImageUrl_Def)
                    {
                        this.Attributes.Add("MessageImageUrl", this.MessageImageUrl);
                    }
                }

                // Não renderiza caption se o controle estiver num TDataGrid.
                if (!(this.Manager is TDataGrid))
                {
                    this.RenderCaption(writer);
                }

                this.RenderSpecific(writer);
                if (!InDesignMode(this))
                {
                    this.RenderMessage(writer);
                }

                if (gridlayout)
                {
                    writer.RenderEndTag();
                }

                this.RenderExtra(writer);

                if (!InDesignMode(this) && this.DebugMode)
                {
                    writer.Write("</TD></TR></TABLE>");
                }
            }
            catch (Exception exc)
            {
                if (InDesignMode(this))
                {
                    var lbl = new Label();
                    lbl.Text = "[" + this.ID + "] " + exc.Message;
                    lbl.BackColor = Color.LightGray;
                    lbl.BorderStyle = BorderStyle.Outset;
                    lbl.BorderWidth = 1;
                    lbl.ForeColor = Color.Black;
                    lbl.RenderControl(writer);
                }
                else
                {
                    throw new Exception("Ocorreu um erro em " + this.UniqueID + ".Render().", exc);
                }
            }

            w.Write(swriter.ToString());
        }

        protected override object SaveViewState()
        {
            try
            {
                return this.SaveViewStateInternal();
            }
            finally
            {
            }
        }

        /// <summary>
        ///   Converte um valor para string utilizando o formato e a cultura do controle.
        /// </summary>
        protected string ToString(DbObject value)
        {
            return StrLib.ToStr(value, this.Format, Thread.CurrentThread.CurrentCulture.Name);
        }

        /// <summary>
        ///   Tipa uma string de acordo com o DataType, formato e cultura do controle.
        /// </summary>
        protected DbObject TypeString(string value)
        {
            return StrLib.TypeStr(value, this.DataType, Thread.CurrentThread.CurrentCulture.Name);
        }

        private static string[] BreakExpression(string expr)
        {
            string texto;
            string[] retvals;
            ArrayList list;
            int pos, esp, apr, fpr, aa, fa;

            list = new ArrayList();
            texto = expr.Trim();
            while (texto.Length > 0)
            {
                esp = texto.IndexOf(" ");
                apr = texto.IndexOf("(");
                fpr = texto.IndexOf(")");
                aa = texto.IndexOf("\"");
                fa = aa > -1 && aa < texto.Length - 1 ? texto.IndexOf("\"", aa + 1) : -1;
                if (aa == 0)
                {
                    if (fa == -1)
                    {
                        throw new Exception("Falta fechar aspas");

// texto+="\"";
                        // fa=texto.Length-1;
                    }

                    list.Add(texto.Substring(aa, fa - aa + 1));
                    pos = fa;
                }
                else if (apr == 0)
                {
                    list.Add(texto.Substring(apr, 1));
                    pos = 0;
                }
                else if (fpr == 0)
                {
                    list.Add(texto.Substring(fpr, 1));
                    pos = 0;
                }
                else if (esp > 0 || fpr > 0 || apr > 0 || aa > 0)
                {
                    pos = texto.Length;
                    pos = pos > esp && esp > -1 ? esp : pos;
                    pos = pos > fpr && fpr > -1 ? fpr : pos;
                    pos = pos > apr && apr > -1 ? apr : pos;
                    pos = pos > aa && aa > -1 ? aa : pos;
                    list.Add(texto.Substring(0, pos));
                    pos = pos - 1;
                }
                else
                {
                    list.Add(texto.Substring(0).Trim());
                    pos = texto.Length - 1;
                }

                if (pos == texto.Length - 1)
                {
                    texto = string.Empty;
                }
                else
                {
                    texto = texto.Substring(pos + 1).Trim();
                }
            }

            retvals = new string[list.Count];
            list.CopyTo(retvals);
            return retvals;
        }

        private static void ReplaceColumnIdentifiers(RootPointer root, TDataTable table)
        {
            foreach (Identifier column in root.selectByType(JLib.typeToClass(typeof (Identifier))))
            {
                if (!(column is ColumnIdentifier))
                {
                    var achou = false;

                    // Prioritariamente procura na tabela principal
                    foreach (DataColumn col in table.Columns)
                    {
                        var dataColumn = col as TDataColumn;
                        if (dataColumn != null && !dataColumn.IsAux)
                        {
                            if (string.Compare(dataColumn.OriginalCol, column.getName(), true) == 0)
                            {
                                var fullCol = dataColumn.FullCol.Split('.');
                                if (fullCol.Length != 2)
                                {
                                    throw new InvalidOperationException();
                                }

                                column.getParent().replace(
                                    column, 
                                    new ColumnIdentifier(fullCol[0], fullCol[1])
                                    );
                                achou = true;
                                break;
                            }
                        }
                    }

                    // Procura nas tabelas auxiliares se não encontrou na tabela principal
                    if (!achou)
                    {
                        var auxTables = new ArrayList();

                        foreach (DataColumn col in table.Columns)
                        {
                            var dataColumn = col as TDataColumn;
                            if (dataColumn != null && dataColumn.IsAux)
                            {
                                if (string.Compare(dataColumn.OriginalCol, column.getName(), true) == 0)
                                {
                                    auxTables.Add(dataColumn);
                                }
                            }
                        }

                        if (auxTables.Count == 0)
                        {
                            throw new InvalidOperationException("A coluna " + column.getName() + " não existe na tabela " + table.TableName + ".");
                        }
                        else if (auxTables.Count > 1)
                        {
                            throw new InvalidOperationException(
                                "A coluna " + column.getName() + " foi encontrada em mais de uma coluna do DataTable " + table.TableName + ". " +
                                "Utilize o formato 'tabela.coluna'. " +
                                "(encontrados: " + TechLib.EnumerableItemProperty(auxTables, "FullCol") + ")"
                                );
                        }

                        var fullCol = ((TDataColumn)auxTables[0]).FullCol.Split('.');
                        if (fullCol.Length != 2)
                        {
                            throw new InvalidOperationException();
                        }

                        column.getParent().replace(
                            column, 
                            new ColumnIdentifier(fullCol[0], fullCol[1])
                            );
                    }
                }
            }
        }

        private static void ReplaceColumnValueIdentifiers(RootPointer root, IRecordContainer container)
        {
            foreach (ColumnValueIdentifier identifier in root.selectByType(JLib.typeToClass(typeof (ColumnValueIdentifier))))
            {
                identifier.replace(new ParameterIdentifier(container[identifier.getColumnName()]));
            }
        }

        private static void SetColumnsWithoutTable(RootPointer root, TDataTable table)
        {
            foreach (ColumnIdentifier column in root.selectByType(JLib.typeToClass(typeof (ColumnIdentifier))))
            {
                if (column.getTable().Length == 0)
                {
                    column.setTable(table.MainTableName.ToLower());
                }
            }
        }

        void IDependee.AddDepender(Control child)
        {
            this.dependers.Add(child);
        }
    }
}