using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Controls;
using Techne.Controls.Design;
using Techne.Data;

namespace Techne.Web
{
    public class HelpData
    {
        public static string EditButtonsFile = "EditButtons.aspx";

        public static string ImagesPath = "~/Images";

        public static string ManualHelpPath = "~/Help";

        public static string ScriptFilepath = "~/Scripts/Help.js";

        public bool ShowDefaultHelp = true;

        private readonly OperacaoDescCollection operacao;

        private readonly TPage page;

        private readonly PreReqCollection prereq;

        private readonly SeeAlsoCollection seealso;

        private readonly TextCollection summary;

        internal HelpData(TPage page)
        {
            this.page = page;

            this.operacao = new OperacaoDescCollection(this, "OPER");
            this.prereq = new PreReqCollection(this, "PREQ");
            this.seealso = new SeeAlsoCollection(this, "ALSO");
            this.summary = new TextCollection(this, "SUMM");
        }

        public OperacaoDescCollection Oper
        {
            get
            {
                return this.operacao;
            }
        }

        public PreReqCollection PreReq
        {
            get
            {
                return this.prereq;
            }
        }

        public SeeAlsoCollection SeeAlso
        {
            get
            {
                return this.seealso;
            }
        }

        public TextCollection Summary
        {
            get
            {
                return this.summary;
            }
        }

        internal string NamespacePrefix
        {
            get
            {
                return "Techne." + this.page.ApplicationName + ".Web.";
            }
        }

        internal Page Page
        {
            get
            {
                return this.page;
            }
        }

        private string CssFilepath
        {
            get
            {
                if (this.page != null)
                {
                    return this.page.CssFilepath;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public static Type GetClassType(object obj)
        {
            if (obj != null)
            {
                var baseType = obj.GetType();
                while ((baseType != null) && ((baseType.Namespace == "ASP") || (baseType.Namespace == "_ASP")))
                {
                    baseType = baseType.BaseType;
                }

                if (baseType != null)
                {
                    return baseType;
                }
            }

            return null;
        }

        /// <summary>
        ///   Obtém pelo id um controle contido numa grid.
        ///   Pela característica da grid, a busca é feita somente nos TemplateColumn's
        ///   (outros tipos de colunas não permitem especificar o id dos controles internos).
        /// </summary>
        public Control GetGridControl(DataGrid grid, string controlName)
        {
            // Este método não é static para que nunca seja chamado
            // fora do contexto da construção do help.
            if (grid == null)
            {
                throw new ArgumentNullException();
            }

            Control control = null;

            foreach (DataGridColumn column in grid.Columns)
            {
                if (column is TemplateColumn)
                {
                    var cell = new TableCell();
                    column.InitializeCell(cell, 0, ListItemType.Item);

// TableCell.FindControl() não funciona porque cell não foi adicionada à grid.
                    control = this.FindControl(cell, controlName);
                    if (control != null)
                    {
                        break;
                    }
                }
            }

            return control;
        }

        /// <summary>
        ///   Dado uma classe derivada de TPage, devolve um HelpData correspondente à essa página
        ///   instanciando-a e chamando o HelpInit().
        /// </summary>
        internal static HelpData InitHelpData(Type type)
        {
            var ctor = type.GetConstructor(Type.EmptyTypes);
            var page = ctor.Invoke(null) as TPage;
            if (page == null)
            {
                throw new InvalidOperationException();
            }

            var helpInit = type.GetMethod("HelpInit", new[] { typeof (HelpData) });
            if (helpInit == null)
            {
                throw new InvalidOperationException();
            }

            var helpData = new HelpData(page);

            // Executa HelpInit()
            helpInit.Invoke(page, new object[] { helpData });

            return helpData;
        }

        internal void Render()
        {
            var response = HttpContext.Current.Response;
            var pageTitle = TPage.GetPageTitle(GetClassType(this.page), Thread.CurrentThread.CurrentCulture.Name);
            var managers = TControl.GetChildManagers(this.page);
            var linkLists = (THyperLinkList[])TechLib.FindControls(typeof (THyperLinkList), this.page);
            var links = (THyperLink[])TechLib.FindControls(typeof (THyperLink), this.page, true, new[] { typeof (IContainerManager) });
            var linkmtds = (TLinkMethod[])TechLib.FindControls(typeof (TLinkMethod), this.page, true, new[] { typeof (IContainerManager) });
            var edbuttons = (EditButtons[])TechLib.FindControls(typeof (EditButtons), this.page, true, new[] { typeof (IContainerManager) });

            response.Write("<HTML>");
            response.Write("<HEAD>");
            response.Write("<TITLE>Ajuda: " + pageTitle + "</TITLE>");
            response.Write("<LINK HREF=\"" + TUtil.TranslateRelativeUrl(this.CssFilepath) + "\" REL=\"StyleSheet\">");
            response.Write("</HEAD>");

            response.Write("<BODY BGPROPERTIES=\"fixed\">");
            response.Write("<SCRIPT LANGUAGE=\"javascript\" SRC=\"" + TUtil.TranslateRelativeUrl(ScriptFilepath) + "\"></script>");

            response.Write("<P>\r\n");
            response.Write("<P CLASS=\"ONLINEOBJ\">" + pageTitle + "</P>\r\n");

            if (this.summary.Count > 0)
            {
                response.Write(this.summary + "\r\n");
            }

            if (this.prereq.Count > 0)
            {
                response.Write("<P>\r\n");
                response.Write("<P CLASS=\"BLOCO\">Pré-requisitos:</P>\r\n");
                response.Write(this.prereq + "\r\n");
                response.Write("</P>\r\n");
            }

            if (this.operacao.Count > 0)
            {
                response.Write("<P>\r\n");
                response.Write("<P CLASS=\"BLOCO\">Operação:</P>\r\n");
                response.Write(this.operacao + "\r\n");
                response.Write("</P>\r\n");
            }

            if (this.seealso.Count > 0)
            {
                response.Write("<P>\r\n");
                response.Write("<P CLASS=\"BLOCO\">Veja também:</P>\r\n");
                response.Write(this.seealso + "\r\n");
                response.Write("</P>\r\n");
            }

            if (this.ShowDefaultHelp && (((managers.Length > 0) || (linkLists.Length > 0)) || ((linkmtds.Length > 0) || (links.Length > 0))))
            {
                response.Write("<P>\r\n");
                response.Write("<P CLASS=\"BLOCO\">Informações Complementares:</P>\r\n");
                this.RenderManagersHelp(managers);
                this.RenderLinkListsHelp(linkLists);
                this.RenderLinkMethodsHelp(linkmtds);
                if (links.Length > 0)
                {
                    this.RenderLinksHelp(links);
                }

                if (edbuttons.Length > 0)
                {
                    this.RenderEditButtonsHelp();
                }

                response.Write("</P>\r\n");
            }

            response.Write("</P>\r\n");

            response.Write("</BODY>");
            response.Write("</HTML>");
        }

        /// <summary>
        ///   Faz busca recursiva de um controle pelo seu id.
        ///   Não busca dentro de INamingContainer's, a não ser que o controle
        ///   especificado já seja um INamingContainer.
        /// </summary>
        private Control FindControl(Control scope, string id)
        {
            if (scope.ID == id)
            {
                return scope;
            }

            foreach (Control child in scope.Controls)
            {
                if (!(child is INamingContainer))
                {
                    var c = this.FindControl(child, id);
                    if (c != null)
                    {
                        return c;
                    }
                }
            }

            return null;
        }

        private void RenderEditButtonsHelp()
        {
            var response = HttpContext.Current.Response;
            const string TitleCssClass = "ManagerTitle";

            response.Write("<P><SPAN CLASS=\"" + TitleCssClass + "\">EditButtons</SPAN></P>");

            var filepath = Path.Combine(ManualHelpPath, EditButtonsFile);

            response.Write("<P><A HREF=\"" + TUtil.TranslateRelativeUrl(filepath) + "\" TITLE=\"Navega para o help do EditButtons\">");
            response.Write("<IMG SRC=\"" + TUtil.TranslateRelativeUrl(EditButtons.DeleteButtonImageUrl_Def) + "\" BORDER=\"0\" />");
            response.Write("<IMG SRC=\"" + TUtil.TranslateRelativeUrl(EditButtons.NewButtonImageUrl_Def) + "\" BORDER=\"0\" />");
            response.Write("<IMG SRC=\"" + TUtil.TranslateRelativeUrl(EditButtons.EditButtonImageUrl_Def) + "\" BORDER=\"0\" />");
            response.Write("</A></P>");
        }

        private void RenderLinkListsHelp(THyperLinkList[] linkLists)
        {
            var response = HttpContext.Current.Response;

            foreach (var linkList in linkLists)
            {
                response.Write("<P>" + HtmlString.ConvertTHyperLinkList(linkList) + "</P>");
            }
        }

        private void RenderLinkMethodsHelp(TLinkMethod[] linkmtds)
        {
            var response = HttpContext.Current.Response;

            foreach (var linkmtd in linkmtds)
            {
                try
                {
                    response.Write("<P>" + HtmlString.ConvertTLinkMethod(linkmtd, true, linkmtd.Manager) + "</P>");
                }
                catch (HelpParagraphCollection.WarningException exc)
                {
                    response.Write("<P><SPAN CLASS=\"parameter-error\" TITLE=\"" + exc.Message + "\">" + linkmtd.ID + "</SPAN></P>");
                }
            }
        }

        private void RenderLinksHelp(THyperLink[] links)
        {
            var list = new StringCollection();
            foreach (var hyp in links)
            {
                list.Add(HtmlString.ConvertTHyperLink(hyp, true, hyp.Manager) + "<BR>");
            }

            HttpContext.Current.Response.Write(HtmlString.CreateLinksHelpTable(list, "Outros links"));
        }

        private void RenderManagersHelp(IContainerManager[] managers)
        {
            var response = HttpContext.Current.Response;

            foreach (var mgr in managers)
            {
                response.Write("\r\n<A NAME=\"Help_" + ((Control)mgr).UniqueID + "\" />\r\n");
                if (mgr is RecordManager)
                {
                    response.Write("<P>" + HtmlString.ConvertRecordManager((RecordManager)mgr) + "</P>");
                }
                else if (mgr is TDataGrid)
                {
                    response.Write("<P>" + HtmlString.ConvertGrid((TDataGrid)mgr) + "</P>");
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }
    }

    internal class ImageUrl
    {
        public ImageUrl(string url)
        {
            this.Url = url;
        }

        public string Url { get; set; }
    }

    internal class HtmlString
    {
        public static string IconGotoPage = "~/Images/Esfera.gif";

        public static string ConvertGrid(TDataGrid grid)
        {
            IContainerManager manager = grid;

            var table = manager.Table;
            if (table == null)
            {
                grid.DataBind();
                table = manager.Table;
            }

            var fields = new StringCollection();
            var list = new StringCollection();

            foreach (DataGridColumn column in grid.Columns)
            {
                var field = TDataGridBase.GetDataField(column);
                var columnCaption = grid.GetCaption(column);
                string columnDescription;
                string colInfo;

                if (column is TemplateColumn)
                {
                    

                    var cell = new TableCell();
                    column.InitializeCell(cell, 0, ListItemType.Item);
                    colInfo = string.Empty;

                    // Monta lista contendo descrição de cada um dos controles dentro do template column.
                    var columnDescs = new StringCollection();
                    foreach (var control in TControl.GetChildTControls(cell))
                    {
                        string caption, description, controlColInfo;
                        ConvertTControl(control, grid, out caption, out description, out controlColInfo);

                        // Se o caption do controle for igual ao caption da coluna, omite.
                        if (caption == columnCaption)
                        {
                            caption = string.Empty;
                        }

                        // Se o controle referenciar a mesma coluna da tabela que a coluna
                        // da grid referencia, aproveita as informações da coluna da tabela.
                        if (string.Compare(control.ColumnName, field, true) == 0)
                        {
                            colInfo = controlColInfo;
                        }

                        // Coloca tooltip no caption do controle com informações da coluna associada.
                        if (caption.Length > 0 && controlColInfo.Length > 0)
                        {
                            caption = "<SPAN TITLE=\"" + controlColInfo + "\">" + caption + "</SPAN>";
                        }

                        // Utiliza o mesmo estilo do controle no texto da descrição.
                        var descriptionStyle = ((TControl)control).CssClass;
                        if (description.Length > 0 && descriptionStyle.Length > 0)
                        {
                            description = "<SPAN CLASS=\"" + descriptionStyle + "\">" + description + "</SPAN>";
                        }

                        columnDescs.Add(
                            (caption.Length > 0 ? caption : string.Empty) +
                            (caption.Length > 0 && description.Length > 0 ? ": " : string.Empty) +
                            description
                            );
                    }

                    columnDescription = StrLib.EnumerableToStr(
                        columnDescs, "<BR>", 
// Prefixa help de cada controle por hífen somente se existir mais de um controle.
                        columnDescs.Count > 1 ? "-&nbsp;" : string.Empty, 
                        string.Empty
                        );

                    
                }
                else if (column is Techne.Controls.HyperLinkColumn)
                {
                    columnDescription = ConvertTHyperLink(column, false, grid);
                    colInfo = table != null && field.Length > 0 ? GetColInfo((TDataColumn)table.Columns[field]) : string.Empty;
                }
                    
// Outras colunas bound
                else if (table != null && field.Length > 0)
                {
                    var dataCol = (TDataColumn)table.Columns[field];
                    columnDescription = dataCol.GetDescricao(Thread.CurrentThread.CurrentCulture.Name);
                    colInfo = GetColInfo(dataCol);
                }
                else
                {
                    columnDescription = string.Empty;
                    colInfo = string.Empty;
                }

                // Coloca tooltip no caption da coluna da grid com as informações da coluna da tabela.
                if (columnCaption.Length > 0 && colInfo.Length > 0)
                {
                    columnCaption = "<SPAN TITLE=\"" + colInfo + "\">" + columnCaption + "</SPAN>";
                }

                fields.Add("<TD>" + columnCaption + "</TD>");
                list.Add(columnDescription);
            }

            return CreateManagerHelpTable(grid, 0, fields.Count, 
// Header
                                          "<TR CLASS=\"" + grid.HeaderStyle.CssClass + "\">\r\n" +
                                          StrLib.EnumerableToStr(fields, "\r\n") + "\r\n" +
                                          "</TR>\r\n" +
// Data (descrição)
                                          "<TR>\r\n" +
                                          StrLib.EnumerableToStr(list, string.Empty, "<TD VALIGN=\"top\">", "</TD>\r\n") +
                                          "</TR>\r\n"
                );
        }

        public static string ConvertPageType(Type pageType)
        {
            // Link para a página de help.
            var pageHelpLink = GetControlHelpText(pageType, null);

            // Link para a página da aplicação.
            var pageLink = CreateHtmlLink(pageType, "<IMG SRC=\"" + TUtil.TranslateRelativeUrl(IconGotoPage) + "\" BORDER=0 />", false);

            return pageHelpLink + "&nbsp;" + pageLink;
        }

        public static string ConvertRecordManager(RecordManager manager)
        {
            var rows = new StringCollection();
            var table = manager.Table;
            if (table == null)
            {
                manager.DataBind();
                table = manager.Table;
            }

            foreach (var control in TControl.GetChildTControls(manager))
            {
                string caption, description, colInfo;
                ConvertTControl(control, manager, out caption, out description, out colInfo);

                // Adiciona tooltip ao caption do controle com informações da coluna da tabela.
                if (caption.Length > 0 && colInfo.Length > 0)
                {
                    caption = "<SPAN TITLE=\"" + colInfo + "\">" + caption + "</SPAN>";
                }

                // Utiliza o mesmo estilo do controle no texto da descrição.
                var descriptionStyle = ((TControl)control).CssClass;
                if (description.Length > 0 && descriptionStyle.Length > 0)
                {
                    description = "<SPAN CLASS=\"" + descriptionStyle + "\">" + description + "</SPAN>";
                }

                rows.Add(
                    "<TD WIDTH=\"30%\">" + caption + "</TD>\r\n" +
                    "<TD WIDTH=\"70%\">" + description + "</TD>\r\n"
                    );
            }

            return CreateManagerHelpTable(manager, 1, 2, StrLib.EnumerableToStr(rows, string.Empty, "<TR>", "</TR>\r\n"));
        }

        /// <param name = "showControlText">
        ///   Mostra o controle com o texto "navega para a página..." ou somente o texto.
        /// </param>
        public static string ConvertTHyperLink(THyperLink link, bool showControlText, IContainerManager manager)
        {
            return ConvertTHyperLink((object)link, showControlText, manager);
        }

        public static string ConvertTHyperLinkList(THyperLinkList links)
        {
            var title = links.Title;
            if (title.Length == 0)
            {
                title = "Links";
            }

            var rows = new StringCollection();
            foreach (LinkListItem link in links.Links)
            {
                rows.Add(ConvertLinkListItem(link) + "<BR>\r\n");
            }

            return CreateLinksHelpTable(rows, title);
        }

        /// <summary>
        ///   Devolve html contendo a representação visual do controle,
        ///   mais a descrição do método chamado com os seus parâmetros.
        /// </summary>
        public static string ConvertTLinkMethod(TLinkMethod link, bool showControlText, IContainerManager manager)
        {
            MethodInfo method;
            try
            {
                method = BusinessMethod.FindBusinessMethod(link.ExecuteMethod);
            }
            catch (Exception exc)
            {
                throw new HelpParagraphCollection.WarningException("Existe um erro na propriedade ExecuteMethod: " + exc.Message);
            }

            string description;
            try
            {
                description = MethodDescriptionAttribute.GetDescription(method, TPage.IDIOMADEFAULT);
            }
            catch (Exception exc)
            {
                description = "<SPAN CLASS=\"parameter-error\" TITLE=\"" + exc.Message + "\">" + link.ID + "</SPAN>";
            }

            string parametros;
            {
                var parameters = method.GetParameters();
                if (parameters.Length > 0)
                {
                    var list = new StringCollection();
                    foreach (var parameter in parameters)
                    {
                        list.Add(parameter.Name + " (" + parameter.ParameterType.Name + ")");
                    }

                    parametros = "Recebe como parâmetros: " + StrLib.EnumerableToStr(list, ", ") + ".";
                }
                else
                {
                    parametros = "Não recebe parâmetros.";
                }
            }

            return (showControlText ? "<SPAN CLASS=\"title-ref\">" + GetControlHelpText(link, manager) + "</SPAN>: " : string.Empty) +
                   (description != null ? description : "chama o método " + BusinessMethod.GetMethodSignature(method, false) + ".") + " " +
                   parametros;
        }

        /// <summary>
        ///   Representação de um objeto (normalmente algum controle ou type, se for página web) dentro do help.
        /// </summary>
        /// <param name = "control">
        ///   Tipos tratados: TLinkBase, LinkListItem, IContainerManager, ITControl, Type.
        /// </param>
        public static string GetControlHelpText(object control, IContainerManager manager)
        {
            if (control == null)
            {
                throw new ArgumentNullException();
            }

            if (control is TLinkBase)
            {
                var lnk = (TLinkBase)control;
                var swriter = new StringWriter();
                var writer = new HtmlTextWriter(swriter);
                var text = lnk.ColumnName.Length > 0 && manager != null
                               ? TControl.GetCaption(lnk, manager, Thread.CurrentThread.CurrentCulture.Name)
                               : (lnk.Text == "?" ? lnk.GetAutoText() : lnk.Text);
                TLinkBaseDesigner.RenderHollowControl(lnk, writer, true, text, lnk.GetImageUrl());
                return swriter.ToString();
            }
            else if (control is LinkListItem)
            {
                var listItem = (LinkListItem)control;
                var text = listItem.GetText(false);

// Deve-se tratar '?' porque foi passado false como parâmetro de link.GetText().
                if (text != "?")
                {
                    return HttpUtility.HtmlEncode(text);
                }
                else
                {
                    // link.GetText() só devolve '?' quando link.NavigationMethod não é informado.
                    throw new HelpParagraphCollection.WarningException("A propriedade NavigationMethod de um dos itens do controle " + listItem.ParentControl.UniqueID + " não foi informada.");
                }
            }

                // IContainerManager.Title
            else if (control is IContainerManager)
            {
                return "<A HREF=\"#Help_" + ((Control)control).UniqueID + "\" " +
                       "TITLE=\"Mostra a descrição dos campos\">" +
                       ((IContainerManager)control).GetTitle() +
                       "</A>";
            }

                // ITControl.Caption
            else if (control is ITControl)
            {
                return TControl.GetCaption((ITControl)control, manager, Thread.CurrentThread.CurrentCulture.Name);
            }

                // THyperLinkList.Title
            else if (control is THyperLinkList)
            {
                var title = ((THyperLinkList)control).Title;
                if (title.Length == 0)
                {
                    throw new HelpParagraphCollection.WarningException("A propriedade " + ((THyperLinkList)control).UniqueID + ".Title não foi informada");
                }

                return title;
            }

                // Link para help com texto TPage.Title
            else if (control is Type)
            {
                var pageType = (Type)control;
                var text = TPage.GetPageTitle(pageType, Thread.CurrentThread.CurrentCulture.Name);
                return CreateHtmlLink(pageType, text, true);
            }
            else
            {
                throw new NotSupportedException("A classe " + control.GetType().FullName + " não é suportada por HtmlString.GetControlHelpText().");
            }
        }

        internal static string CreateLinksHelpTable(IList list, string title)
        {
            const string TitleCssClass = "ManagerTitle";
            const string TitleRowCssClass = "ManagerTitleRow";

            return
                "<TABLE WIDTH=\"95%\" ALIGN=\"CENTER\" CELLPADDING=\"1\" CELLSPACING=\"1\" BORDER=\"0\" STYLE=\"BORDER-COLLAPSE: collapse\">\r\n" +
// Título
                "<TR STYLE=\"height: 0px;\"><TD COLSPAN=\"2\"><TABLE CLASS=\"" + TitleRowCssClass + "\" BORDER=\"0\" STYLE=\"height: 0%; width: 100%;\"><TR>" +
                "<TD STYLE=\"width: 100%;\" NOWRAP=\"nowrap\"><SPAN CLASS=\"" + TitleCssClass + "\">" +
                title +
                "</SPAN></TD>\r\n" +
                "</TR></TABLE></TD></TR>\r\n" +
                "<TR><TD>" +
                StrLib.EnumerableToStr(list, string.Empty) +
                "</TD></TR>\r\n" +
                "</TABLE>\r\n";
        }

        private static string ConvertLinkListItem(LinkListItem link)
        {
            return ConvertTHyperLink(link, true, null);
        }

        /// <summary>
        ///   Dado um controle, determina sua representação visual no help, descrição e informações da coluna
        ///   no banco de dados, se aplicável.
        /// </summary>
        /// <param name = "manager">
        ///   Só é necessário ser informado quando o controle informado pertencer a um TemplateColumn,
        ///   caso contrário, null poderá ser informado, situação na qual control.Manager será utilizado.
        /// </param>
        /// <param name = "caption">Representação visual do controle no help.</param>
        /// <param name = "description">Descrição da coluna cadastrada no Cronos, caso o controle esteja associado a uma.</param>
        /// <param name = "colInfo">Informações da coluna, caso o controle esteja associado a uma.</param>
        private static void ConvertTControl(ITControl control, IContainerManager manager, 
                                            out string caption, out string description, out string colInfo)
        {
            if (manager == null)
            {
                manager = control.Manager;
            }

            // caption
            try
            {
                caption = GetControlHelpText(control, manager);
            }
            catch (HelpParagraphCollection.WarningException exc)
            {
                caption = "<SPAN CLASS=\"parameter-error\" TITLE=\"" + exc.Message + "\">" + ((Control)control).ID + "</SPAN>";
            }

            if (manager.Table != null && control.ColumnName.Length > 0)
            {
                var column = (TDataColumn)manager.Table.Columns[control.ColumnName];
                description = column.GetDescricao(Thread.CurrentThread.CurrentCulture.Name);
                colInfo = GetColInfo(column);
            }
            else
            {
                description = string.Empty;
                colInfo = string.Empty;
            }

            if (control is THyperLink)
            {
                if (description.Length > 0)
                {
                    description += "<br>";
                }

                description += ConvertTHyperLink((THyperLink)control, false, manager);
            }
            else if (control is TLinkMethod)
            {
                try
                {
                    if (description.Length > 0)
                    {
                        description += "<br>";
                    }

                    description += ConvertTLinkMethod((TLinkMethod)control, false, manager);
                }
                catch (HelpParagraphCollection.WarningException exc)
                {
                    description += "<SPAN CLASS=\"parameter-error\" TITLE=\"" + exc.Message + "\">" + ((Control)control).ID + "</SPAN>";
                }
            }
        }

        /// <param name = "control">
        ///   Pode ser THyperLink ou LinkListItem (um item de THyperLinkList).
        /// </param>
        /// <param name = "showControlText">
        ///   Mostra o controle com o texto "navega para a página..." ou somente o texto.
        /// </param>
        private static string ConvertTHyperLink(object control, bool showControlText, IContainerManager manager)
        {
            string navigationMethod;
            if (control is THyperLink)
            {
                navigationMethod = ((THyperLink)control).NavigationMethod;
            }
            else if (control is LinkListItem)
            {
                navigationMethod = ((LinkListItem)control).NavigationMethod;
            }
            else if (control is Techne.Controls.HyperLinkColumn)
            {
                navigationMethod = ((Techne.Controls.HyperLinkColumn)control).NavigationMethod;
            }
            else
            {
                throw new ArgumentException("O parâmetro control dever ser THyperLink, LinkListItem ou HyperLinkColumn.");
            }

            if (navigationMethod.Length > 0)
            {
                string pageLink;
                {
                    var type = Navigation.GetType(navigationMethod);
                    var pageTitle = TPage.GetPageTitle(type, Thread.CurrentThread.CurrentCulture.Name);

                    pageLink = CreateHtmlLink(type, pageTitle, true);
                }

                return (showControlText ? "<SPAN CLASS=\"title-ref\">" + GetControlHelpText(control, manager) + "</SPAN>: " : string.Empty) +
                       "navega para a página " + pageLink;
            }
            else
            {
                string id;
                if (control is THyperLink)
                {
                    id = ((THyperLink)control).UniqueID;
                }
                else if (control is LinkListItem)
                {
                    id = ((LinkListItem)control).ParentControl.UniqueID + "[" + ((LinkListItem)control).GetText(false) + "]";
                }
                else
                {
                    throw new NotSupportedException();
                }

                return "<SPAN CLASS=\"parameter-error\" TITLE=\"Propriedade NavigationMethod não informada.\">" + id + "</SPAN>";
            }
        }

        /// <summary>
        ///   Cria um link para a página ou para o help da página, conforme parâmetro.
        /// </summary>
        private static string CreateHtmlLink(Type pageType, string linkText, bool help)
        {
            var url = TPage.GetPageUrl(pageType);
            var title = TitleAttribute.GetPageTitle(pageType, Thread.CurrentThread.CurrentCulture.Name);

            string href, tooltip;
            if (help)
            {
                href = TUtil.TranslateRelativeUrl(url) + "?help";
                tooltip = "Mostra o help da página" + (title != null && title != linkText ? " " + title : string.Empty);
            }
            else
            {
                href = "javascript:navega_apl('" + TUtil.TranslateRelativeUrl(url) + "');";
                tooltip = "Navega para a página" + (title != null && title != linkText ? " " + title : string.Empty);
            }

            return "<A HREF=\"" + href + "\" TITLE=\"" + tooltip + "\">" + linkText + "</A>";
        }

        private static string CreateManagerHelpTable(IContainerManager manager, int border, int titleColSpan, string body)
        {
            var table = manager.Table;

            return
                "<TABLE WIDTH=\"95%\" ALIGN=\"CENTER\" CELLPADDING=\"1\" CELLSPACING=\"1\" BORDER=\"" + border + "\" STYLE=\"BORDER-COLLAPSE: collapse\">\r\n" +
// Título
                "<TR STYLE=\"height: 0px;\"><TD COLSPAN=\"" + titleColSpan + "\"><TABLE CLASS=\"" + manager.TitleRowCssClass + "\" BORDER=\"0\" STYLE=\"height: 0%; width: 100%;\"><TR>" +
                "<TD STYLE=\"width: 100%;\" NOWRAP=\"nowrap\"><SPAN CLASS=\"" + manager.TitleCssClass + "\">" +
                manager.GetTitle() +
                "</SPAN></TD>\r\n" +
                "<TD ALIGN=\"right\" NOWRAP=\"nowrap\"><FONT COLOR=\"silver\">" +
                (table == null ? string.Empty : table.TableName + (table.HistoryEnabled ? "&nbsp;(historificada)" : string.Empty)) +
                "</FONT></TD>" +
                "</TR></TABLE></TD></TR>\r\n" +
                body +
                "</TABLE>\r\n";
        }

        private static string GetColInfo(TDataColumn column)
        {
            var col = column.RefCol;

            // A primeira versão de TDataColumn.RefCol é virtual e devolve string.Empty.
            // Trata essa especificidade utilizando-se FullCol, que dá o nome da coluna utilizando
            // alias definidos na query do data table. Não é o ideal, mas é provisório até
            // TDataColumn.RefCol ser transformado de virtual para abstract.
            if (col.Length == 0)
            {
                col = column.FullCol;
            }

            return col + ":\r\n" +
                   column.SqlServerType +
                   (column.PrimaryKey ? ", PK" : string.Empty) +
                   (column.NotNull ? ", NOT NULL" : string.Empty);
        }
    }

    public abstract class HelpParagraphCollection
    {
        private const char ParameterChar = '?';

        private readonly HelpData parent;

        private string expandableControlPrefix;

        protected HelpParagraphCollection(HelpData parent, string expandableControlPrefix)
        {
            this.parent = parent;
            this.expandableControlPrefix = expandableControlPrefix;
        }

        public abstract int Count { get; }

        protected HelpData Parent
        {
            get
            {
                return this.parent;
            }
        }

        protected abstract string ToString(string leftDelimiter, string rightDelimiter);

        protected string ReplaceParameters(string text, object[] parameters)
        {
            if (this.Parent.Page == null)
            {
                throw new InvalidOperationException("Help.Page não foi setado.");
            }

            var b = new StringBuilder();
            var parIndex = 0;

            var i = 0;
            while (i < text.Length)
            {
                var p = text.IndexOf(ParameterChar, i);
                if (p < 0)
                {
                    // Não tem mais nenhum token.
                    b.Append(HttpUtility.HtmlEncode(text.Substring(i)));
                    break;
                }

                b.Append(HttpUtility.HtmlEncode(text.Substring(i, p - i)));

                if (p == text.Length - 1)
                {
                    throw new InvalidOperationException("O caractere '" + ParameterChar + "' não pode aparecer no final do texto.");
                }

                var parameterType = text[p + 1];
                try
                {
                    switch (text[p + 1])
                    {
                        case 'T':
                        case 't':
                            try
                            {
                                if (parIndex >= parameters.Length)
                                {
                                    throw new WarningException("Falta parâmetro");
                                }

                                var parameter = parameters[parIndex];
                                if (parameter == null)
                                {
                                    throw new WarningException("Foi informado null como parâmetro");
                                }

                                string controlText;
                                try
                                {
                                    // Apesar de GetControlHelpText() conseguir tratar LinkListItem, a idéia é que
                                    // isso não seja permitido ao desenvolvedor de helps. Isso evita acessar um
                                    // item de THyperLinkList pelo seu índice numérico.
                                    if (parameter is LinkListItem)
                                    {
                                        throw new NotSupportedException();
                                    }

                                    if (parameter is ImageButton)
                                    {
                                        controlText = "<IMG SRC=\"" + TUtil.TranslateRelativeUrl(((ImageButton)parameter).ImageUrl) + "\"/>";
                                    }
                                    else if (parameter is Image)
                                    {
                                        controlText = "<IMG SRC=\"" + TUtil.TranslateRelativeUrl(((Image)parameter).ImageUrl) + "\"/>";
                                    }
                                    else
                                    {
                                        var manager = parameter is ITControl ? ((ITControl)parameter).Manager : null;
                                        controlText = HtmlString.GetControlHelpText(parameter, manager);
                                    }
                                }
                                catch (NotSupportedException)
                                {
                                    throw new WarningException("O tipo " + parameter.GetType().FullName + " não é permitido neste contexto.");
                                }

                                if (controlText.Length == 0)
                                {
                                    throw new WarningException("O texto é vazio");
                                }

                                if (parameter is Type || parameter is IContainerManager || parameter is ImageButton || parameter is Image)
                                {
                                    // Se for classe de página, ou manager não evidencia o título, pois a evidência já é feita pelo link.
                                    b.Append(controlText);
                                }
                                else
                                {
                                    b.Append("<SPAN CLASS=\"title-ref\">" + controlText + "</SPAN>");
                                }
                            }
                            finally
                            {
                                parIndex++;
                            }

                            break;

                        case '?':
                            b.Append('?');
                            break;
                        case 'I':
                        case 'i':
                            try
                            {
                                var parameter = parameters[parIndex];
                                if (parameter is string)
                                {
                                    b.Append("<IMG SRC=\"" + TUtil.TranslateRelativeUrl(parameter as string) + "\"/>");
                                }
                            }
                            catch
                            {
                            }
                            finally
                            {
                                parIndex++;
                            }

                            break;
                        default:
                            throw new WarningException("Tipo inválido");
                    }
                }
                catch (WarningException warning)
                {
                    b.Append("<SPAN CLASS=\"parameter-error\" TITLE=\"" + warning.Message + "\">" + ParameterChar + parameterType + "</SPAN>");
                }

                // Pula a interrogação e o caractere após.
                i = p + 2;
            }

            return b.ToString();
        }

        internal class WarningException : ApplicationException
        {
            public WarningException(string message) : base(message)
            {
            }
        }
    }

    public class TextCollection : HelpParagraphCollection
    {
        protected StringCollection list = new StringCollection();

        internal TextCollection(HelpData parent, string expandableControlPrefix) : base(parent, expandableControlPrefix)
        {
        }

        public override int Count
        {
            get
            {
                return this.list.Count;
            }
        }

        public override string ToString()
        {
            return this.ToString("<P CLASS=\"DESCBLOCO\">", "</P>");
        }

        public void Add(string text, params object[] parameters)
        {
            if (text != null)
            {
                this.list.Add(this.ReplaceParameters(text, parameters));
            }
        }

        protected override string ToString(string leftDelimiter, string rightDelimiter)
        {
            var list = new ArrayList();

            foreach (var str in this.list)
            {
                list.Add(leftDelimiter + str + rightDelimiter);
            }

            return StrLib.EnumerableToStr(list, "\r\n");
        }
    }

    public class PreReqCollection : HelpParagraphCollection
    {
        protected ArrayList list = new ArrayList();

        internal PreReqCollection(HelpData parent, string expandableControlPrefix) : base(parent, expandableControlPrefix)
        {
        }

        public override int Count
        {
            get
            {
                return this.list.Count;
            }
        }

        public override string ToString()
        {
            return this.ToString("<P CLASS=\"DESCBLOCO\">", "</P>");
        }

        public void Add(Type pageType)
        {
            this.list.Add(HtmlString.ConvertPageType(pageType));
        }

        protected override string ToString(string leftDelimiter, string rightDelimiter)
        {
            var list = new StringCollection();

            foreach (string str in this.list)
            {
                list.Add(leftDelimiter + str + rightDelimiter);
            }

            return StrLib.EnumerableToStr(list, "\r\n");
        }
    }

    public class SeeAlsoCollection : TextCollection
    {
        internal SeeAlsoCollection(HelpData parent, string expandableControlPrefix) : base(parent, expandableControlPrefix)
        {
        }

        public void Add(params Type[] pageTypes)
        {
            var a = new ArrayList();

            foreach (var pageType in pageTypes)
            {
                a.Add(HtmlString.GetControlHelpText(pageType, null));
            }

            // Se foi informado mais de um Type, adiciona-os numa mesma linha separados por vírgulas.
            this.list.Add(StrLib.EnumerableToStr(a, ",&nbsp;"));
        }
    }

    public class OperacaoDescCollection : HelpParagraphCollection
    {
        private readonly ArrayList list = new ArrayList();

        private int itemCount;

        internal OperacaoDescCollection(HelpData parent, string expandableControlPrefix) : base(parent, expandableControlPrefix)
        {
        }

        public override int Count
        {
            get
            {
                return this.list.Count;
            }
        }

        public override string ToString()
        {
            return this.ToString("<P CLASS=\"DESCBLOCO\">", "</P>");
        }

        public void Add(string text, params object[] parameters)
        {
            if (text != null)
            {
                this.list.Add(this.ReplaceParameters(text, parameters));
            }
        }

        public void Add(IContainerManager manager)
        {
            if (manager != null)
            {
                this.list.Add(manager);
            }
        }

        public void ImageAdd(string imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                var img = new ImageUrl(imageUrl);
                this.list.Add(img);
            }
        }

        public void NewItem()
        {
            this.list.Add(null);
        }

        public void TitleAdd(string text)
        {
            this.list.Add(new Subtitle(text));
        }

        protected override string ToString(string leftDelimiter, string rightDelimiter)
        {
            var strlist = new StringCollection();
            var stritem = string.Empty;

            foreach (var item in this.list)
            {
                // O null é adicionado por NewItem().
                if (item == null)
                {
                    if (stritem.Length > 0)
                    {
                        strlist.Add(leftDelimiter + stritem + rightDelimiter);
                    }

                    try
                    {
                        var gifName = this.GetGifName(++this.itemCount);
                        stritem = "<IMG SRC=\"" + TUtil.TranslateRelativeUrl(HelpData.ImagesPath) + "/" + gifName + "\"/>&nbsp;";
                    }
                    catch (NotSupportedException)
                    {
                        stritem = "<SPAN CLASS=\"parameter-error\" TITLE=\"Não existe gif para o número\">" + this.itemCount + "</SPAN>";
                    }
                }
                else if (item is Subtitle)
                {
                    if (stritem.Length > 0)
                    {
                        strlist.Add(leftDelimiter + stritem + rightDelimiter);
                        stritem = string.Empty;
                    }

                    strlist.Add("<P CLASS=\"OPERSUBTITLE\">" + ((Subtitle)item).Text + "</P>");
                }
                else if (item is ImageUrl)
                {
                    stritem = "<IMG SRC=\"" + TUtil.TranslateRelativeUrl(((ImageUrl)item).Url) + "\"/>&nbsp;";
                }
                else
                {
                    if (item is string)
                    {
                        stritem += (string)item;
                    }
                    else if (item is RecordManager)
                    {
                        stritem += HtmlString.ConvertRecordManager((RecordManager)item);
                    }
                    else if (item is TDataGrid)
                    {
                        stritem += HtmlString.ConvertGrid((TDataGrid)item);
                    }
                    else if (item is THyperLinkList)
                    {
                        stritem += HtmlString.ConvertTHyperLinkList((THyperLinkList)item);
                    }
                    else if (item is THyperLink)
                    {
                        stritem += HtmlString.ConvertTHyperLink((THyperLink)item, true, ((THyperLink)item).Manager);
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }

                    strlist.Add(leftDelimiter + stritem + rightDelimiter);
                    stritem = string.Empty;
                }
            }

            return StrLib.EnumerableToStr(strlist, "\r\n");
        }

        private string GetGifName(int count)
        {
            switch (count)
            {
                case 1:
                    return "one.gif";
                case 2:
                    return "two.gif";
                case 3:
                    return "three.gif";
                case 4:
                    return "four.gif";
                default:
                    throw new NotSupportedException();
            }
        }

        private class Subtitle
        {
            private readonly string text;

            public Subtitle(string text)
            {
                if (text == null)
                {
                    throw new ArgumentNullException();
                }

                this.text = text;
            }

            public string Text
            {
                get
                {
                    return this.text;
                }
            }
        }
    }
}