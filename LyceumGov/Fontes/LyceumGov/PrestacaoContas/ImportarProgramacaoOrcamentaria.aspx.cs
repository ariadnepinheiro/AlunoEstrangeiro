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
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [
    NavUrl("~/PrestacaoContas/ImportarProgramacaoOrcamentaria.aspx"),
     ControlText("ImportarProgramacaoOrcamentaria"),
     Title("Importar Programação Orçamentária"),
   ]

    public partial class ImportarProgramacaoOrcamentaria : TPage
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
                    CarregaAno();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        private void CarregaAno()
        {
            RN.PeriodoLetivo rnPeriodoLetivo = new RN.PeriodoLetivo();
            ddlAno.Items.Clear();
            ddlAno.DataSource = rnPeriodoLetivo.ListaAnos(false);
            ddlAno.DataBind();
            ddlAno.Items.Insert(0, new ListItem("Selecione", string.Empty));
        }


        public static DataTable CarregarCSV(StreamReader sr, Encoding encoding)
        {
            string[] headers = sr.ReadLine().Split(';');
            DataTable dt = new DataTable();

            foreach (string header in headers)
                dt.Columns.Add(header.Trim());

            while (!sr.EndOfStream)
            {
                string[] rows = Regex.Split(sr.ReadLine(), ";(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    dr[i] = rows[i];
                }
                dt.Rows.Add(dr);
            }

            return dt;
        }
        public static DataTable CarregarCSV(FileInfo file, Encoding encoding)
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

            //validar as colunas da planilha [PLANILHAORCAMENTARIA]
            if (!dt.Columns.Contains("OBJETO"))  //[PLANILHAORCAMENTARIA].[DESCRICAO]
                erros.Add("Coluna \"OBJETO\" não existe na planilha especificada");

            if (!dt.Columns.Contains("ND")) //[PLANILHAORCAMENTARIA].NATUREZADESPESAID  SELECT *  FROM [LYCEUM].[PrestacaoContas].[NATUREZADESPESA]
                erros.Add("Coluna \"ND\" não existe na planilha especificada");

            if (!dt.Columns.Contains("COD.DESP."))//[PLANILHAORCAMENTARIA].PLANOTRABALHOID    SELECT * FROM [LYCEUM].[PrestacaoContas].[PLANOTRABALHO]
                erros.Add("Coluna \"CODDESP\" não existe na planilha especificada");

            if (!dt.Columns.Contains("COD ÁREA")) //[PLANILHAORCAMENTARIA].[REGIAOFINANCEIRAID]
                erros.Add("Coluna \"COD ÁREA\" não existe na planilha especificada");

            if (!dt.Columns.Contains("PROCESSO")) //[PLANILHAORCAMENTARIA].[PROCESSO]
                erros.Add("Coluna \"PROCESSO\" não existe na planilha especificada");

            //validar as colunas da planilha [ITEMPLANILHAORCAMENTARIA]
            if (!dt.Columns.Contains("FR"))//[ITEMPLANILHAORCAMENTARIA].[FONTERECURSOID]
                erros.Add("Coluna \"FR\" não existe na planilha especificada");

            if (!dt.Columns.Contains("JANEIRO"))//[ITEMPLANILHAORCAMENTARIA].[REFERENCIA] e [valor]
                erros.Add("Coluna \"JANEIRO\" não existe na planilha especificada");
            if (!dt.Columns.Contains("FEVEREIRO"))
                erros.Add("Coluna \"FEVEREIRO\" não existe na planilha especificada");
            if (!dt.Columns.Contains("Março"))
                erros.Add("Coluna \"Março\" não existe na planilha especificada");
            if (!dt.Columns.Contains("ABRIL"))
                erros.Add("Coluna \"ABRIL\" não existe na planilha especificada");
            if (!dt.Columns.Contains("MAIO"))
                erros.Add("Coluna \"MAIO\" não existe na planilha especificada");
            if (!dt.Columns.Contains("JUNHO"))
                erros.Add("Coluna \"JUNHO\" não existe na planilha especificada");
            if (!dt.Columns.Contains("JULHO"))
                erros.Add("Coluna \"JULHO\" não existe na planilha especificada");
            if (!dt.Columns.Contains("AGOSTO"))
                erros.Add("Coluna \"AGOSTO\" não existe na planilha especificada");
            if (!dt.Columns.Contains("SETEMBRO"))
                erros.Add("Coluna \"SETEMBRO\" não existe na planilha especificada");
            if (!dt.Columns.Contains("OUTUBRO"))
                erros.Add("Coluna \"OUTUBRO\" não existe na planilha especificada");
            if (!dt.Columns.Contains("NOVEMBRO"))
                erros.Add("Coluna \"NOVEMBRO\" não existe na planilha especificada");
            if (!dt.Columns.Contains("DEZEMBRO"))
                erros.Add("Coluna \"DEZEMBRO\" não existe na planilha especificada");


            if (erros.Count == 0)
            {
                //validar os dados da planilha
                int contador = 0;
                var errosplan = new List<string>();

                foreach (DataRowView drv in dt.DefaultView)
                {
                    contador++;

                    var valorPROCESSO = (drv["PROCESSO"] ?? "").ToString();
                    var valorOBJETO = (drv["OBJETO"] ?? "").ToString();
                    var valorCODAREA = (drv["COD ÁREA"] ?? "").ToString();
                    var valorFR = (drv["FR"] ?? "").ToString();
                    var valorND = (drv["ND"] ?? "").ToString();
                    var valorCODDESP = (drv["COD.DESP."] ?? "").ToString();

                    var valorJANEIRO = (drv["JANEIRO"] ?? "").ToString();
                    var valorFEVEREIRO = (drv["FEVEREIRO"] ?? "").ToString();
                    var valorMARCO = (drv["Março"] ?? "").ToString();
                    var valorABRIL = (drv["ABRIL"] ?? "").ToString();
                    var valorMAIO = (drv["MAIO"] ?? "").ToString();
                    var valorJUNHO = (drv["JUNHO"] ?? "").ToString();
                    var valorJULHO = (drv["JULHO"] ?? "").ToString();
                    var valorAGOSTO = (drv["AGOSTO"] ?? "").ToString();
                    var valorSETEMBRO = (drv["SETEMBRO"] ?? "").ToString();
                    var valorOUTUBRO = (drv["OUTUBRO"] ?? "").ToString();
                    var valorNOVEMBRO = (drv["NOVEMBRO"] ?? "").ToString();
                    var valorDEZEMBRO = (drv["DEZEMBRO"] ?? "").ToString();

                }
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

                    if (!ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                    {
                        var linha = 0;
                        var linhaimportado = 0;

                        if (arquivo.Value != "")
                        {
                            if (fileName.Contains(".csv"))
                            {
                                //connection.Open(true);
                                var csvFile = new FileInfo(arquivoserver);

                                DateTime DataInicial = System.DateTime.Now;
                                DataView dvMatriz = CarregarCSV(csvFile, Encoding.UTF7).DefaultView;
                                using (DataView dv = CarregarCSV(csvFile, Encoding.UTF7).DefaultView)
                                {
                                    //validação da planilha
                                    var erros = EhPlanilhaValida(dv.Table);//colocar a validação dos itens conforme a tabela PRODUTOSERVICOVALORMAXIMO
                                    int contadorErros = 0;
                                    foreach (var erro in erros)
                                    {
                                        contadorErros++;
                                    }
                                    if (contadorErros == 0)
                                    {
                                        Techne.Lyceum.RN.PrestacaoContas.ImportarProgramacaoOrcamentaria rnImportarProgramacaoOrcamentaria = new Techne.Lyceum.RN.PrestacaoContas.ImportarProgramacaoOrcamentaria();
                                        rnImportarProgramacaoOrcamentaria.ImportaArquivo(dv, Convert.ToInt32(ddlAno.SelectedValue), User.Identity.Name, out linha, out errosprocessamento, out linhaimportado);

                                        var exibeerro = "";
                                        for (int i = 0; i < errosprocessamento.Count(); i++)
                                        {
                                            exibeerro = exibeerro + " " + errosprocessamento[i] + "<br/>";
                                        }

                                        lblMensagem.Text = "Importação Finalizada.<br/><br/>Total de Linhas no arquivo da Programação Orçamentária:" + linha + "<br/>Total de Linhas no Arquivo da Programação Orçamentária importados: " + linhaimportado + "<br/>Total de Erros: " + errosprocessamento.Count() + "<br/><br/> " + exibeerro;

                                    }
                                    else { lblMensagem.Text = "Arquivo informado esta fora do layout exigido para importação. <br/>" + erros.Aggregate((x, y) => x + Environment.NewLine + y).Replace(Environment.NewLine, "<br />"); }
                                }
                            }
                            else { lblMensagem.Text = "Arquivo tem que estar no formato CSV."; }
                        }
                        else { lblMensagem.Text = "Arquivo informado não é na extensão CSV."; }
                    }
                    else { lblMensagem.Text = "Ano não preenchido!!!"; }
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
