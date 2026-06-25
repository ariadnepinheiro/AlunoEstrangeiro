using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Controls;
using Techne.Web;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Entidades;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [
    NavUrl("~/PrestacaoContas/ImportacaoTabelaFgv.aspx"),
     ControlText("ImportacaoTabelaFgv"),
     Title("Importar Tabela de Preços de Valores Máximos"),
   ]

    public partial class ImportacaoTabelaFgv : TPage
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    CarregarMes();
                    CarregaAno();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnImportar, AcaoControle.novo);
            
        }
        private void CarregarMes()
        {
            ddlMes.Items.Clear();
            ddlMes.DataSource = RN.Util.Utils.ListaMes();
            ddlMes.DataBind();
            ddlMes.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        private void CarregaAno()
        {
            RN.PeriodoLetivo rnPeriodoLetivo = new RN.PeriodoLetivo();
            ddlAno.Items.Clear();
            ddlAno.DataSource = rnPeriodoLetivo.ListaAnos(false);
            ddlAno.DataBind();
            ddlAno.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }

        protected void tseRegiaoFGV_Changed(object sender, EventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                if (!tseRegiaoFGV.DBValue.IsNull)
                {
                    if (!tseRegiaoFGV.IsValidDBValue)
                    {

                        lblMensagem.Text = "Áreas Geográficas não ativa ou não cadastrada (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Áreas Geográficas não ativa ou não cadastrada (favor verificar).";

                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public DataTable CarregarCSV(StreamReader sr, Encoding encoding)
        {
            try
            {
                var linha = 1;
                string[] headers = sr.ReadLine().Split(';');
                DataTable dt = new DataTable();
                int l = 0;

                if (headers.Count() != 6)
                {
                    lblMensagem.Text = "Arquivo fora do formato permitido. CODIGO|NOME|UNIDADEMEDIDA|VALOR|NCM|FLAGNCM";
                    return null;
                } 
                
                foreach (string header in headers)
                    dt.Columns.Add(header);

                dt.Columns.Add("ROW");

                while (!sr.EndOfStream)
                {
                    linha++;


                    var conteudo = sr.ReadLine().Replace("\t", "");
          
                    string[] rows = Regex.Split(conteudo, ";(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                    DataRow dr = dt.NewRow();
                    dr["ROW"] = linha;

                    if (rows.Count() < headers.Length)
                    {
                        lblMensagem.Text = "Problema identificado na linha " + linha + ". Favor verificar pelo csv.";
                        return null;
                    }

                    for (int i = 0; i < headers.Length && i < dr.Table.Columns.Count; i++)
                    {
                        l = i;
                        dr[i] = rows[i];

                        if (i == 4 /*NCM*/)
                        {
                            //Considerar somente o que for número na string. O que não for, descarta
                            var ncm = Convert.ToString(dr[i]);

                            string resultado = new string(ncm.Where(char.IsDigit).ToArray());

                            dr[i] = resultado;
                        }
                    }
                    dt.Rows.Add(dr);
                }

                return dt;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                return null;
            }
        }

        public DataTable CarregarCSV(FileInfo file, Encoding encoding)
        {
            StreamReader sr = null;

            try
            {
                sr = new StreamReader(file.FullName, encoding);
                return CarregarCSV(sr, encoding);
            }
            catch (IOException ioex)
            {
                throw ioex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sr != null)
                    sr.Dispose();
            }
        }

        public static IEnumerable<string> EhPlanilhaValida(DataTable dt)
        {
            var erros = new List<string>();

            //validar as colunas da planilha
            if (!dt.Columns.Contains("CODIGO"))
                erros.Add("Coluna \"CODIGO\" não existe na planilha especificada");

            if (!dt.Columns.Contains("NOME"))
                erros.Add("Coluna \"NOME\" não existe na planilha especificada");

            if (!dt.Columns.Contains("UNIDADEMEDIDA"))
                erros.Add("Coluna \"UNIDADEMEDIDA\" não existe na planilha especificada");

            if (!dt.Columns.Contains("VALOR"))
                erros.Add("Coluna \"VALOR\" não existe na planilha especificada");

            if (!dt.Columns.Contains("NCM"))
                erros.Add("Coluna \"NCM\" não existe na planilha especificada");

            if (!dt.Columns.Contains("FLAGNCM"))
                erros.Add("Coluna \"FLAGNCM\" não existe na planilha especificada");

            if (erros.Any())
                return erros;

            //validar os dados da planilha
            foreach (DataRowView drv in dt.DefaultView)
            {
                var valorCODFGV = (drv["CODIGO"] ?? "").ToString();
                var valorNOME = (drv["NOME"] ?? "").ToString();
                var valorUNIDADEMEDIDA = (drv["UNIDADEMEDIDA"] ?? "").ToString();
                var valor = (drv["VALOR"] ?? "").ToString();
                var valorNCM = (drv["NCM"] ?? "").ToString();
                var valorFLAGNCM = (drv["FLAGNCM"] ?? "").ToString();
                var contador = Convert.ToInt32(drv["ROW"]);

                if (valorCODFGV.Length > 10)
                    erros.Add("Linha:" + contador + " - CÓDIGO não pode ter mais do que 10 caracteres");

                if (valorNOME.Length > 250)
                    erros.Add("Linha:" + contador + " - NOME não pode ter mais do que 250 caracteres");

                if (valorUNIDADEMEDIDA.Length > 10)
                    erros.Add("Linha:" + contador + " - UNIDADEMEDIDA não pode ter mais do que 10 caracteres");

                if (valor.IsNullOrEmptyOrWhiteSpace())
                    erros.Add("Linha:" + contador + " - VALOR não pode estar em branco");

                if (!valor.IsNullOrEmptyOrWhiteSpace() && valor.Length > 15)
                    erros.Add("Linha:" + contador + " - VALOR não pode ter mais do que 15 caracteres");

                if (valorNCM.Length > 10)
                    erros.Add("Linha:" + contador + " - NCM não pode ter mais do que 10 caracteres");

                if (valorCODFGV.Length < 2)
                    erros.Add("{preMsg}CÓDIGO não pode ter menos do que 2 caracteres");


                if (valorNOME.Length < 2)
                    erros.Add("Linha:" + contador + " - NOME não pode ter menos do que 2 caracteres");

                if (valorUNIDADEMEDIDA.Length < 1)
                    erros.Add("Linha:" + contador + " - UNIDADEMEDIDA não pode ter menos do que 1 caracter");

                if (valorNCM.Length < 1)
                    erros.Add("Linha:" + contador + " - NVM não pode ter menos do que 1 caracter");
            }

            return erros;
        }

        protected void btnImportar_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = System.IO.Path.GetFileName(arquivo.PostedFile.FileName);
                if (fileName != "")
                {
                    arquivo.PostedFile.SaveAs("C:/Logs/Gestao/PrestacaoContas/" + fileName);
                    var arquivoserver = "C:/Logs/Gestao/PrestacaoContas/" + fileName;
                    var errosprocessamento = new List<string>();
                    if (!tseRegiaoFGV.DBValue.IsNull && tseRegiaoFGV.IsValidDBValue)
                    {
                        var regiaofgv = Convert.ToInt32(tseRegiaoFGV.DBValue);

                        if (!ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlMes.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                        {
                            var linha = 0;
                            var linhaimportado = 0;

                            if (arquivo.Value != "")
                            {
                                if (fileName.Contains(".csv"))
                                {
                                    var csvFile = new FileInfo(arquivoserver);

                                    DateTime DataInicial = System.DateTime.Now;

                                    DataTable dt = CarregarCSV(csvFile, Encoding.UTF7);

                                    if (dt != null)
                                    {

                                        using (DataView dv = dt.DefaultView)
                                        {
                                            //validação da planilha
                                            var erros = EhPlanilhaValida(dv.Table);//colocar a validação dos itens conforme a tabela PRODUTOSERVICOVALORMAXIMO
                                            if (!erros.Any())
                                            {
                                                Techne.Lyceum.RN.PrestacaoContas.ImportacaoFgv rnImportacaoFgv = new Techne.Lyceum.RN.PrestacaoContas.ImportacaoFgv();
                                                rnImportacaoFgv.ImportaArquivo(dv, Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlMes.SelectedValue), regiaofgv, User.Identity.Name, out linha, out errosprocessamento, out linhaimportado);

                                                var exibeerro = "";
                                                for (int i = 0; i < errosprocessamento.Count(); i++)
                                                {
                                                    exibeerro = exibeerro + " " + errosprocessamento[i] + "<br/>";
                                                }

                                                lblMensagem.Text = "Importação Finalizada!!!<br/><br/>Total de Produtos/Serviços na tabela de Preços de Valores Máximos: " + linha + "<br/>Total de Produtos/Serviços importados: " + linhaimportado + "<br/>Total de Erros: " + errosprocessamento.Count() + "<br/><br/> " + exibeerro;

                                            }
                                            else
                                            {
                                                lblMensagem.Text = erros.Aggregate((c, n) => c + "<br />" + n);
                                            }
                                        }
                                    }
                                }

                                else { lblMensagem.Text = "Arquivo tem que estar no formato CSV."; }
                            }
                            else { lblMensagem.Text = "Arquivo informado não é na extensão CSV."; }
                        }
                        else { lblMensagem.Text = "Mês ou ano não preenchidos."; }
                    }
                    else { lblMensagem.Text = "Região não Preenchida."; }
                }
                else { lblMensagem.Text = "Arquivo não Selecionado!!!"; }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

    }
}
