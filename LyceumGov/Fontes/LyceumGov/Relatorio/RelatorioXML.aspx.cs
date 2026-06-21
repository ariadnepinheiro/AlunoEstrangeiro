using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using DevExpress.Web.ASPxEditors;
using Techne.Data;
using Techne.Web;

namespace Techne.Lyceum.Net.Relatorio
{
    [NavUrl("~/Relatorio/RelatorioXML.aspx"),
      ControlText("Relatório"),
      Title("Relatório"),]
    public partial class RelatorioXML : TPage
    {
        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {

        }
        #endregion

        private static string PaginaAnterior = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = "";

            CarregaRelatorio();
        }

        void Page_Init(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(HttpContext.Current.Server.MapPath("~/XMLRelatorio/" + QueryStringDecodificada["xml"] + ".xml"));
            }
            catch (Exception)
            {
                lblMensagem.Text = "Arquivo de relatório não encontrado.";
                return;
            }

            XmlNodeList nodes = xmlDoc.GetElementsByTagName("parametros");

            if (nodes != null && nodes.Count > 0 && !dvParametros.HasControls())
            {
                var temp = nodes[0].ChildNodes.Cast<XmlElement>().Select(node =>
                    new { FieldType = node.GetAttribute("fieldType"), Name = node.GetAttribute("name"), Label = node.GetAttribute("label") });

                //var temp2 = from node in nodes[0].ChildNodes.Cast<XmlElement>()                                
                //            select new { FieldType = node.GetAttribute("fieldType"), Name = node.GetAttribute("name") };
                Table tb = new Table();

                foreach (var parametro in temp)
                {
                    TableRow tr = new TableRow();
                    TableCell td = new TableCell();
                    td.Style.Add(HtmlTextWriterStyle.TextAlign, "right");
                    Label lb = new Label();
                    lb.ID = "lbl" + parametro.Name;
                    lb.Text = parametro.Label;
                    td.Controls.Add(lb);
                    tr.Cells.Add(td);
                    TableCell tc = new TableCell();

                    switch (parametro.FieldType)
                    {
                        case "text":

                            TextBox txt = new TextBox();
                            txt.ID = parametro.Name;
                            txt.Width = Unit.Pixel(200);
                            tc.Controls.Add(txt);
                            break;
                        case "date":

                            ASPxDateEdit dt = new ASPxDateEdit();
                            dt.ID = parametro.Name;
                            dt.Width = Unit.Pixel(200);
                            tc.Controls.Add(dt);
                            break;
                    }
                    tr.Cells.Add(tc);
                    tb.Rows.Add(tr);
                }

                dvParametros.Controls.Add(tb);
                //dvParametros

                return;
                //for (int i = 0; i < nodes[0].ChildNodes.Count; i++)
                //{
                //    string fieldtype = ((XmlElement)nodes[0].ChildNodes[i]).GetAttribute("fieldType");
                //}
            }


        }

        private void CarregaRelatorio()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();

                try
                {
                    xmlDoc.Load(HttpContext.Current.Server.MapPath("~/XMLRelatorio/" + QueryStringDecodificada["xml"] + ".xml"));
                }
                catch (Exception)
                {
                    lblMensagem.Text = "Arquivo de relatório não encontrado.";
                }

                List<object> list = new List<object>();

                for (int i = 1; i < 11; i++)
                {
                    if (QueryStringDecodificada["p" + i] != null && QueryStringDecodificada["p" + i].ToString() != null)
                        list.Add(QueryStringDecodificada["p" + i]);
                    else
                        break;
                }

                XmlNodeList nodes = xmlDoc.GetElementsByTagName("parametros");

                if (dvParametros.HasControls())
                {
                    var temp = nodes[0].ChildNodes.Cast<XmlElement>().Select(node =>
                        new { FieldType = node.GetAttribute("fieldType"), Name = node.GetAttribute("name"), Label = node.GetAttribute("label") });

                    foreach (var parametro in temp)
                    {
                        switch (parametro.FieldType)
                        {
                            case "text":
                                TextBox txt = (TextBox)dvParametros.FindControl(parametro.Name);
                                if (txt != null && txt.Text == string.Empty)
                                {
                                    lblMensagem.Text = "Preencha todos os campos e clique em Exibir Relatório.";
                                    return;
                                }
                                else
                                {
                                    list.Add(txt.Text);
                                }
                                break;
                            case "date":
                                ASPxDateEdit dt = (ASPxDateEdit)dvParametros.FindControl(parametro.Name);
                                if (dt != null && dt.Date != null && dt.Text == string.Empty)
                                {
                                    lblMensagem.Text = "Preencha todos os campos e clique em Exibir Relatório.";
                                    return;
                                }
                                else
                                {
                                    list.Add(dt.Date);
                                }
                                break;
                        }
                    }
                }

                string query = xmlDoc.GetElementsByTagName("query")[0].InnerText;

                QueryTable qt = RN.Relatorio.ExecutaQuery(query, list);
                if (qt.Rows.Count == 0)
                {
                    lblMensagem.Text = "A pesquisa do relatório não retornou dados.";
                    return;
                }

                string textoParam = xmlDoc.GetElementsByTagName("texto")[0].InnerText;

                Dictionary<string, string> replacements = new Dictionary<string, string>();
                for (int i = 0; i < qt.Columns.Count; i++)
                {
                    replacements.Add(qt.Columns[i].ColumnName, qt.Rows[0][i].ToString());
                }

                Regex regex = new Regex(@"\#(?<key>\w+)\#");

                string newStr = regex.Replace(textoParam, match => replacements.ContainsKey(match.Groups[1].Value) ? replacements[match.Groups[1].Value] : match.Groups[0].Value);

                dvTexto.InnerHtml = newStr;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void bt_Click(object sender, EventArgs e)
        {
            CarregaRelatorio();
        }

        protected void bt_Voltar(object sender, EventArgs e)
        {
            //if (PaginaAnterior != string.Empty)
            Response.Redirect("~/ProcessoSeletivo/RelatoriosContratos.aspx");
        }

        protected void bt_Imprimir(object sender, EventArgs e)
        {
            //
        }
    }
}
