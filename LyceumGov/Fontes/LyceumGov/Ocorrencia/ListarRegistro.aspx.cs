using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Web;
using RN = Techne.Lyceum.RN;
using System.Linq;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Ocorrencia
{
    [
        NavUrl("~/Ocorrencia/ListarRegistro.aspx"),
        ControlText("ListarRegistro"),
        Title("Registros"),
    ]
    public partial class ListarRegistro : TPage
    {
        #region Propriedades

        private Techne.Lyceum.RN.Turma.DadosTurma ObjetoTurma
        {
            get { return (Techne.Lyceum.RN.Turma.DadosTurma)ViewState["ObjetoTurma"]; }
            set { ViewState["ObjetoTurma"] = value; }
        }       

        private decimal AnoVigente
        {
            get
            {
                if (ViewState["ano_vigente"] == null)
                    ViewState["ano_vigente"] = RN.PeriodoLetivo.ConsultarAnoLetivoAtual();
                return (decimal)ViewState["ano_vigente"];
            }
        }

        #endregion

        #region Eventos

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                TituloGrid(grdRegistro, "Ocorrências");
                ControlaAcesso(grdRegistro);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdRegistro);
            ControlaAcesso(grdRegistro, AcaoControle.excluir, "btnDesativar"); 

            foreach (ListItem item in ddlAno.Items)
            {
                if (item.Value == Convert.ToString(AnoVigente + 1))
                    item.Attributes.Add("style", "color:Red");
                else
                    item.Attributes.Add("style", "color:#0353AB");
            }
        }

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    CarregarDadosDrop(ddlAno.ID);
                    PreencherDadosSession();

                    //verifica se existe alguma querystring
                    if (Request.QueryString.Keys.Count > 0)
                        CarregarDados();
                    else
                        grdRegistro.Enabled = false;
                }

                DefinirFiltroUnidadeEnsino();

                CarregarExibirGrid();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void CarregarExibirGrid()
        {
            try
            {
                lblMensagem.Text = string.Empty;
                              
                CarregarGrid();

                grdRegistro.Enabled = true;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }


        }

        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;

                lblUAValor.Text = string.Empty;


                RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (sessao != null)
                {
                    if (!tseMunicipio.DBValue.IsNull && tseMunicipio.IsValidDBValue)
                    {
                        sessao.Municipio = tseMunicipio.DBValue.ToString();
                        tseUnidadeResponsavel.ResetValue();

                    }

                }
                CarregarExibirGrid();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;

                lblUAValor.Text = string.Empty;

                RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
                if (sessao != null)
                {
                    if (!tseRegional.DBValue.IsNull && tseRegional.IsValidDBValue)
                    {
                        sessao.Regional = Convert.ToString(tseRegional.DBValue);
                        tseUnidadeResponsavel.ResetValue();
                    }
                }
                CarregarExibirGrid();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeResponsavel_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;

                lblUAValor.Text = string.Empty;

                RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue && !tseUnidadeResponsavel["unidade_ens"].IsNull)
                {
                    lblUAValor.Text = Convert.ToString(tseUnidadeResponsavel["UA_ATUAL"]);

                    tseRegional.Value = tseUnidadeResponsavel["id_regional"];
                    tseMunicipio.Value = tseUnidadeResponsavel["municipio"];

                    if (sessao != null)
                    {
                        sessao.Escola = Convert.ToString(tseUnidadeResponsavel.DBValue);
                        sessao.Municipio = tseMunicipio.DBValue.ToString();
                        sessao.Regional = tseUnidadeResponsavel["id_regional"].ToString();
                    }
                }
                CarregarExibirGrid();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {

            RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
            if (sessao != null && !string.IsNullOrEmpty(ddlAno.SelectedValue))
            {
                sessao.Ano = ddlAno.SelectedValue;
            }
            CarregarExibirGrid();
        }



        /// <summary>
        /// Evento de Click do ImageButton de busca
        /// </summary>
        /// <param name="sender">objeto do sistema</param>
        /// <param name="e">argumento do sistema</param>
        protected void btnPesquisar_Click(object sender, ImageClickEventArgs e)
        {
            CarregarExibirGrid();
        }

        private bool PodeExecutarOperacaoPorNomeRetornoGrid(string nomeRetorno)
        {
            return nomeRetorno == Enum.GetName(typeof(NomeRetornoGridDevExpressEnum), NomeRetornoGridDevExpressEnum.ADDNEWROW)
                || nomeRetorno == Enum.GetName(typeof(NomeRetornoGridDevExpressEnum), NomeRetornoGridDevExpressEnum.STARTEDIT)
                || nomeRetorno == Enum.GetName(typeof(NomeRetornoGridDevExpressEnum), NomeRetornoGridDevExpressEnum.SELECTION)
                || nomeRetorno == Enum.GetName(typeof(NomeRetornoGridDevExpressEnum), NomeRetornoGridDevExpressEnum.REATIVAR);
        }

        private RN.DTOs.DadosOcorrencia PreencheTurmaComValoresObtidosDaGridGradeSerieParaAlteracao(ASPxGridViewAfterPerformCallbackEventArgs gridGradeSerie)
        {
            RN.DTOs.DadosOcorrencia ocorrencia = new Techne.Lyceum.RN.DTOs.DadosOcorrencia();          

            return ocorrencia;
        }


        protected void grdRegistro_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            string tipoOperacao = string.Empty;

            if (PodeExecutarOperacaoPorNomeRetornoGrid(e.CallbackName))
            {
                RN.DTOs.DadosOcorrencia ocorrencia = new Techne.Lyceum.RN.DTOs.DadosOcorrencia();                

                if (e.CallbackName.Equals(Enum.GetName(typeof(NomeRetornoGridDevExpressEnum), NomeRetornoGridDevExpressEnum.ADDNEWROW)))
                {
                    //tipoOperacao = "NOVO";
                    tipoOperacao = Enum.GetName(typeof(TipoOperacaoEnum), TipoOperacaoEnum.NOVO);

                    if (string.IsNullOrEmpty(ddlAno.SelectedValue))
                    {
                        throw new Exception("O Campo ANO é de preenchimento obrigatório.");
                    }

                    if (!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue && !tseUnidadeResponsavel["unidade_ens"].IsNull)
                    {
                        ocorrencia.Censo = tseUnidadeResponsavel.DBValue.ToString();

                   }
                    else
                    {
                        throw new Exception("O Campo UNIDADE DE ENSINO é de preenchimento obrigatório.");
                    }
                }
                else if (e.CallbackName.Equals(Enum.GetName(typeof(NomeRetornoGridDevExpressEnum), NomeRetornoGridDevExpressEnum.STARTEDIT)))
                {
                    tipoOperacao = Enum.GetName(typeof(TipoOperacaoEnum), TipoOperacaoEnum.ALTERAR);               

                    ocorrencia.OcorrenciaId = Convert.ToInt32(grdRegistro.GetRowValuesByKeyValue(e.Args[0], "OCORRENCIAID"));

                }
                else if (e.CallbackName.Equals(Enum.GetName(typeof(NomeRetornoGridDevExpressEnum), NomeRetornoGridDevExpressEnum.SELECTION)))
                {
                    //tipoOperacao = "CONSULTAR";
                    tipoOperacao = Enum.GetName(typeof(TipoOperacaoEnum), TipoOperacaoEnum.CONSULTAR);
                    ocorrencia.OcorrenciaId = Convert.ToInt32(grdRegistro.GetRowValues(GetSelectedRowOnTheCurrentPage(), "OCORRENCIAID"));
                   
                }
                ocorrencia.Censo = tseUnidadeResponsavel.DBValue.ToString();

                string queryString = MontarQueryString(tipoOperacao, ocorrencia);
                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                ASPxWebControl.RedirectOnCallback("Registro.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
            }

            ControlaAcesso(grdRegistro);
            ControlaAcesso(grdRegistro, AcaoControle.excluir, "btnDesativar");
        }


        protected void grdRegistro_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {

            RN.DTOs.DadosOcorrencia ocorrencia = new Techne.Lyceum.RN.DTOs.DadosOcorrencia();
                      
            string queryString = MontarQueryString("EXCLUIR", ocorrencia);

            String nometurma = e.Values["grade_token"] + "-" + lblUAValor.Text;
           
            byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);
            ASPxWebControl.RedirectOnCallback("Registro.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
        }

       

        protected void grdRegistro_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {

            ValidacaoDados validacao = new ValidacaoDados();
            RN.Ocorrencias.Ocorrencia rnOcorrencia = new Techne.Lyceum.RN.Ocorrencias.Ocorrencia();
            RN.DTOs.DadosOcorrencia ocorrencia = new Techne.Lyceum.RN.DTOs.DadosOcorrencia();
            try
            {
              
                int id = Convert.ToInt32(grdRegistro.GetRowValues(e.VisibleIndex, "OCORRENCIAID"));

                if (e.ButtonID == "btnDesativar")
                {
                    lblMensagem.Text = "Registro excluído com sucesso.";

                    CarregarGrid();
                   
                }
              
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Código Padrão Techne
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
        #endregion

        #region Métodos

        private void DefinirFiltroUnidadeEnsino()
        {
            string sqlWhere = string.Empty;
            string table = string.Empty;

            Techne.Library.Sql.Structure.SqlSelectColumns coluna = new Techne.Library.Sql.Structure.SqlSelectColumns();

            table = " VW_UNIDADE_ENSINO_SITUACAO ";

            coluna.Add("unidade_ens");
            coluna.Add("nome_comp");
            coluna.Add("setor");
            coluna.Add("cgc");
            coluna.Add("situacao");
            coluna.Add("nucleo");
            coluna.Add("municipio");
            coluna.Add("id_regional");
            coluna.Add("ua_atual");
            coluna.Add("ua_antiga");
            coluna.Add("bairro");

            if (!tseRegional.DBValue.IsNull)
            {
                if (tseRegional.IsValidDBValue)
                    sqlWhere = " id_regional = " + Convert.ToString(tseRegional.DBValue);
            }

            if (!tseMunicipio.DBValue.IsNull)
            {
                if (tseMunicipio.IsValidDBValue)
                {
                    if (string.IsNullOrEmpty(sqlWhere))
                        sqlWhere = "municipio = " + Convert.ToString(tseMunicipio.DBValue);
                    else
                        sqlWhere += " AND municipio = " + Convert.ToString(tseMunicipio.DBValue);
                }
            }

            if (coluna.Count > 0)
            {
                Techne.Library.Sql.Structure.SqlSelect sqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);

                tseUnidadeResponsavel.SqlSelect = sqlSelect;
                tseUnidadeResponsavel.SqlWhere = sqlWhere;

                tseUnidadeResponsavel.DataBind();
            }
        }

        private void PreencherDadosSession()
        {
            RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();


            if (sessao != null)
            {
                if (!string.IsNullOrEmpty(sessao.Ano))
                {
                    if (ddlAno.Items.FindByValue(sessao.Ano) != null)
                    {
                        ddlAno.SelectedValue = sessao.Ano;
                        
                    }

                }
                if (!string.IsNullOrEmpty(sessao.Regional))
                {
                    tseRegional.DBValue = sessao.Regional;
                    if (!tseRegional.IsValidDBValue)
                    {
                        tseRegional.Msg = string.Empty;
                        tseRegional.ResetValue();
                    }
                }
                if (!string.IsNullOrEmpty(sessao.Municipio))
                {
                    tseMunicipio.DBValue = sessao.Municipio;
                    if (!tseMunicipio.IsValidDBValue)
                    {
                        tseMunicipio.Msg = string.Empty;
                        tseMunicipio.ResetValue();
                    }
                }
                if (!string.IsNullOrEmpty(sessao.Escola))
                {
                    lblUAValor.Text = string.Empty;

                    tseUnidadeResponsavel.DBValue = sessao.Escola;

                    if (!tseUnidadeResponsavel.IsValidDBValue)
                    {
                        tseUnidadeResponsavel.Msg = string.Empty;
                        tseUnidadeResponsavel.ResetValue();
                    }
                    else
                    {
                        if (!tseUnidadeResponsavel["unidade_ens"].IsNull)
                        {
                            lblUAValor.Text = Convert.ToString(tseUnidadeResponsavel["UA_ATUAL"]);

                        }
                    }
                }
            }
        }

        private int GetSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = grdRegistro.PageIndex * grdRegistro.SettingsPager.PageSize;
            for (int i = 0; i < grdRegistro.VisibleRowCount; i++)
            {
                if (grdRegistro.Selection.IsRowSelected(startIndexOnPage + i))
                    return startIndexOnPage + i;
            }

            return -1;
        }




        private void ObterDadosQueryString(string queryString)
        {
            ObjetoTurma = new Techne.Lyceum.RN.Turma.DadosTurma();
            lblMensagem.Text = string.Empty;
            string[] listaDados = queryString.Split('&');

            foreach (string dados in listaDados)
            {
                if (dados.IndexOf("ano") >= 0)
                    ObjetoTurma.Ano = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("semestre") >= 0)
                    ObjetoTurma.Periodo = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("nucleo") >= 0)
                    ObjetoTurma.Nucleo = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("municipio") >= 0)
                    ObjetoTurma.Municipio = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("grade") >= 0)
                    ObjetoTurma.Grade = dados.Substring(dados.LastIndexOf('=') + 1);
                else if (dados.IndexOf("mensagem") >= 0)
                    lblMensagem.Text = dados.Substring(dados.LastIndexOf('=') + 1);
            }
        }

        private void CarregarDados()
        {
            try
            {
                byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

                ObterDadosQueryString(decodedText);

                CarregarDadosDrop(ddlAno.ID);

                grdRegistro.Enabled = true;

               
                ObjetoTurma = null;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private DataTable DataTableEstrutura()
        {
            DataTable dtEstrutura = new DataTable();

            dtEstrutura.Columns.Add("OCORRENCIAID", typeof(int));
            dtEstrutura.Columns.Add("CENSO", typeof(int));
            dtEstrutura.Columns.Add("DATAOCORRENCIA", typeof(int));
            dtEstrutura.Columns.Add("SUBCLASSE", typeof(string));
            dtEstrutura.Columns.Add("CLASSE", typeof(string));
            dtEstrutura.Columns.Add("TRATAMENTO", typeof(string));
            dtEstrutura.Columns.Add("BATALHAO", typeof(string));
            dtEstrutura.Columns.Add("DELEGACIA", typeof(string));
            dtEstrutura.Columns.Add("SITUACAO", typeof(string));

            return dtEstrutura;
        }

        

        private void CarregarGrid()
        {
            RN.Ocorrencias.Ocorrencia rnOcorrencia = new Techne.Lyceum.RN.Ocorrencias.Ocorrencia();

            DataTable dtRegistro = new DataTable();
            RN.Perfil rnPerfil = new Techne.Lyceum.RN.Perfil();
            try
            {
                if (!this.tseUnidadeResponsavel.DBValue.IsNull && this.tseUnidadeResponsavel.IsValidDBValue)
                {

                    if (rnPerfil.PossuiPerfilAdministradorRVEPor(User.Identity.Name) || RN.Usuarios.UsuarioPrivilegiado(User.Identity.Name))
                    {
                        dtRegistro = rnOcorrencia.ListaOcorrenciaAtivoPor(tseUnidadeResponsavel.DBValue.ToString(), Convert.ToInt32(ddlAno.SelectedValue), false);
                    }
                    else
                    {
                        dtRegistro = rnOcorrencia.ListaOcorrenciaAtivoPor(tseUnidadeResponsavel.DBValue.ToString(), Convert.ToInt32(ddlAno.SelectedValue), true);

                    }
                }

                if (dtRegistro != null)
                {
                    if (dtRegistro.Rows.Count > 0)
                        grdRegistro.DataSource = dtRegistro;
                    else
                        grdRegistro.DataSource = DataTableEstrutura();
                }
                else
                    grdRegistro.DataSource = DataTableEstrutura();

                grdRegistro.DataBind();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }



        private void CarregarDropDownList(DropDownList drop, QueryTable data, List<DropDownList> listaDrop, string defaultValue)
        {
            drop.DataSource = data;
            drop.DataBind();

            if (data != null)
            {
                if (data.Rows != null)
                {
                    if (data.Rows.Count > 0)
                    {
                        if (drop.Items.FindByValue(defaultValue) != null)
                            drop.SelectedValue = defaultValue;

                    }
                }
            }

            if (listaDrop != null)
            {
                foreach (DropDownList dropDependente in listaDrop)
                {
                    dropDependente.Items.Clear();
                    dropDependente.DataBind();
                }
            }
        }


        private QueryTable CarregarDadosDrop(string idDrop)
        {
            QueryTable dadosDrop = null;

            try
            {
                switch (idDrop.ToUpper())
                {
                    case "DDLANO":
                        {
                            dadosDrop = RN.PeriodoLetivo.ConsultarAno();

                            List<DropDownList> listaDrop = new List<DropDownList>();

                            string anoAtual = Convert.ToString(AnoVigente);

                            if ((ObjetoTurma == null) || (ObjetoTurma != null && string.IsNullOrEmpty(ObjetoTurma.Ano)))
                            {
                                CarregarDropDownList(ddlAno, dadosDrop, listaDrop, anoAtual);

                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(ObjetoTurma.Ano))
                                    CarregarDropDownList(ddlAno, dadosDrop, listaDrop, ObjetoTurma.Ano);
                                else
                                    CarregarDropDownList(ddlAno, dadosDrop, listaDrop, anoAtual);
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
            return dadosDrop;
        }

        private string MontarQueryString(string tipoOperacao, Techne.Lyceum.RN.DTOs.DadosOcorrencia ocorrencia)
        {
            string queryString = string.Empty;

            if (ocorrencia != null)
            {
                queryString += "tela=" + "registro";
                queryString += "tipoOperacao=" + tipoOperacao;
                queryString += "&codigo=" + ocorrencia.OcorrenciaId;
                queryString += "&censo=" + ocorrencia.Censo;
                queryString += "&bairro=" + tseUnidadeResponsavel["bairro"].ToString();
                queryString += "&municipio=" + tseMunicipio["nome"].ToString();
                queryString += "&regional=" + tseRegional["descricao"].ToString();
                queryString += "&unidade=" + tseUnidadeResponsavel["nome_comp"].ToString();
                queryString += "&ano=" + ddlAno.SelectedValue;
              
            }
            return queryString;
        }
        #endregion


        protected void grdRegistro_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            var situacao = (string)grdRegistro.GetRowValues(e.VisibleIndex, "SITUACAO");

            if (!string.IsNullOrEmpty(situacao)
                && situacao == "Arquivado")
            {
                if (e.ButtonType == ColumnCommandButtonType.Edit)
                {
                    e.Visible = false;
                }
            }
        }

        protected void grdRegistro_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "SITUACAO")
            {
                String situacao = Convert.ToString(e.CellValue);
                if (situacao == "Incompleto - Sem Alvo" || situacao == "Incompleto - Sem Autor")
                    e.Cell.ForeColor = System.Drawing.Color.Red;
            }
            
        }

       
    }
}
