using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using System.Data;
using DevExpress.Web.ASPxGridView;
using Techne.Web;
using DevExpress.Web.ASPxTabControl;
using Techne.Controls;
using Seeduc.Infra.Helpers;
using System.Drawing;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Globalization;
using Techne.Lyceum.Net.Modulos;
using DevExpress.Web.ASPxGridView.Rendering;

namespace Techne.Lyceum.Net.Transporte
{
    [NavUrl("~/Transporte/Pagamento.aspx")]
    [ControlText("Pagamento")]
    [Title("Pagamento")]

    public partial class Pagamento : TPage
    {
        public object Listar(object unidade, object codMunicipio, object dataInicio, object dataFim, object diasLetivo, object idPagamento)
        {
            RN.Transporte.Pagamento rnPagamento = new Techne.Lyceum.RN.Transporte.Pagamento();

            var censo = unidade != null ? unidade.ToString() : null;
            var municipio = codMunicipio != null ? codMunicipio.ToString() : null;
            var inicio = dataInicio != null ? dataInicio.ToString() : null;
            var fim = dataFim != null ? dataFim.ToString() : null;
            var dias = diasLetivo != null ? diasLetivo.ToString() : null;
            var id = idPagamento != null ? idPagamento.ToString() : null;


            if (!string.IsNullOrEmpty(censo) && !string.IsNullOrEmpty(municipio) && !string.IsNullOrEmpty(inicio) && !string.IsNullOrEmpty(fim) && !string.IsNullOrEmpty(dias))
            {
                if (id.IsNullOrEmptyOrWhiteSpace())
                {
                    return rnPagamento.ListaPagamentoRotaPor(Convert.ToString(censo), Convert.ToString(municipio), Convert.ToDateTime(inicio), Convert.ToDateTime(fim), Convert.ToInt32(dias));
                }
                else
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        return rnPagamento.ListaPagamentoRotaPor(Convert.ToInt32(id));
                    }
                }
            }
            return null;
        }

        public object ListarSituacaoPagamento()
        {
            return CarregaSituacaoPagamento();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    if (Request.QueryString.Keys.Count > 0)
                    {
                        LimparDados();
                        CarregarDados();

                        if (!hdnIdPagamento.Value.IsNullOrEmptyOrWhiteSpace())
                        {
                            btnSalvar.Visible = false;
                            btnCancelar.Visible = false;
                        }
                        else
                        {
                            lblValorTotalUnidade.Text = string.Format("{0:N2}", 0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimparDados()
        {
            lblCodMunicipio.Text = string.Empty;
            lblMunicipio.Text = string.Empty;
            lblCenso.Text = string.Empty;
            lblNomeEscola.Text = string.Empty;
            lblDataInicio.Text = string.Empty;
            lblDataFim.Text = string.Empty;
            lblDiasLetivo.Text = string.Empty;
            hdnIdPagamento.Value = string.Empty;
            lblValorTotalUnidade.Text = string.Empty;
        }

        protected DataTable CarregaSituacaoPagamento()
        {
            try
            {
                RN.Transporte.SituacaoPagamento rnSituacaoPagamento = new Techne.Lyceum.RN.Transporte.SituacaoPagamento();
                DataTable dt = new DataTable();
                DataTable resultado = new DataTable();

                dt = rnSituacaoPagamento.ListaAtivo();

                DataRow dr = dt.NewRow();
                dr["SITUACAOPAGAMENTOID"] = 0;
                dr["DESCRICAO"] = "Selecione";
                dt.Rows.Add(dr);

                dt.DefaultView.Sort = "SITUACAOPAGAMENTOID ASC";

                dt = dt.DefaultView.ToTable();

                return dt;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                return null;
            }
        }

        private void CarregarDados()
        {
            try
            {
                RN.Matriculas.DiasNaoLetivos rnDiasNaoLetivos = new Techne.Lyceum.RN.Matriculas.DiasNaoLetivos();

                byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

                string[] listaDados = decodedText.Split('&');

                foreach (string dados in listaDados)
                {
                    if (dados.IndexOf("codmuc") >= 0)
                        lblCodMunicipio.Text = dados.Substring(dados.LastIndexOf('=') + 1);
                    else if (dados.IndexOf("nomemunicipio") >= 0)
                        lblMunicipio.Text = dados.Substring(dados.LastIndexOf('=') + 1);
                    else if (dados.IndexOf("censo") >= 0)
                        lblCenso.Text = dados.Substring(dados.LastIndexOf('=') + 1);
                    else if (dados.IndexOf("escola") >= 0)
                        lblNomeEscola.Text = dados.Substring(dados.LastIndexOf('=') + 1);
                    else if (dados.IndexOf("Inicio") >= 0)
                        lblDataInicio.Text = dados.Substring(dados.LastIndexOf('=') + 1);
                    else if (dados.IndexOf("Fim") >= 0)
                        lblDataFim.Text = dados.Substring(dados.LastIndexOf('=') + 1);
                    else if (dados.IndexOf("idPagamento") >= 0)
                        hdnIdPagamento.Value = dados.Substring(dados.LastIndexOf('=') + 1);
                    else if (dados.IndexOf("valorTotal") >= 0)
                        lblValorTotalUnidade.Text = dados.Substring(dados.LastIndexOf('=') + 1);

                }

                lblDiasLetivo.Text = rnDiasNaoLetivos.RetornaDiasUteisPor(lblCodMunicipio.Text, Convert.ToDateTime(lblDataInicio.Text), Convert.ToDateTime(lblDataFim.Text)).ToString();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnCancelarVoltar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.btnSalvar.Visible = this.btnCancelar.Visible = false;

                var queryString = this.MontarQueryString();
                var bytesToEncode = Encoding.UTF8.GetBytes(queryString);

                this.Server.Transfer("ListarPagamento.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        private string MontarQueryString()
        {
            string queryString = string.Empty;


            queryString += "censo=" + lblCenso.Text;

            return queryString;
        }

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Transporte.Pagamento rnPagamento = new Techne.Lyceum.RN.Transporte.Pagamento();
                RN.Transporte.Entidades.Pagamento pagamento = new Techne.Lyceum.RN.Transporte.Entidades.Pagamento();
                List<RN.Transporte.Entidades.PagamentoRota> listaPagamentosRota = new List<Techne.Lyceum.RN.Transporte.Entidades.PagamentoRota>();
                decimal valorTotal = 0;
                decimal valorTotalporRota = 0;

                pagamento.Censo = !lblCenso.Text.IsNullOrEmptyOrWhiteSpace() ? lblCenso.Text : null;
                pagamento.DataInicio = !lblDataInicio.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(lblDataInicio.Text) : DateTime.MinValue;
                pagamento.DataFim = !lblDataFim.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(lblDataFim.Text) : DateTime.MinValue;
                pagamento.UsuarioId = User.Identity.Name;
                pagamento.QuantidadeDias = !lblDiasLetivo.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(lblDiasLetivo.Text) : -1;


                for (var rowIndex = 0; rowIndex < this.grdPagamento.VisibleRowCount; rowIndex++)
                {
                    var pagamentoRota = new Techne.Lyceum.RN.Transporte.Entidades.PagamentoRota();
                    var rotaId = this.grdPagamento.GetRowValues(rowIndex, "ROTAID").ToString();
                    var txtDesconto = DevExpressHelper.GetControl<TextBox>(this.grdPagamento, rowIndex, "DESCONTO", "txtDesconto");
                    var ddlSituacaoPagamento = DevExpressHelper.GetControl<DropDownList>(this.grdPagamento, rowIndex, "MOTIVO", "ddlSituacaoPagamento");
                    var quantidadeAlunoIda = this.grdPagamento.GetRowValues(rowIndex, "QUANTIDADEALUNOIDA").ToString();
                    var quantidadeAlunoVolta = this.grdPagamento.GetRowValues(rowIndex, "QUANTIDADEALUNOVOLTA").ToString();
                    var quantidadeDiasIda = this.grdPagamento.GetRowValues(rowIndex, "QUANTIDADEDIASIDA").ToString();
                    var quantidadeDiasVolta = this.grdPagamento.GetRowValues(rowIndex, "QUANTIDADEDIASVOLTA").ToString();
                    var quantidadeKmIda = this.grdPagamento.GetRowValues(rowIndex, "QUANTIDADEKMIDA").ToString();
                    var quantidadeKmVolta = this.grdPagamento.GetRowValues(rowIndex, "QUANTIDADEKMVOLTA").ToString();
                    var valorRotaIda = this.grdPagamento.GetRowValues(rowIndex, "VALORROTAIDA").ToString();
                    var valorRotaVolta = this.grdPagamento.GetRowValues(rowIndex, "VALORROTAVOLTA").ToString();
                    var valorCalculadoIda = Convert.ToDecimal(this.grdPagamento.GetRowValues(rowIndex, "VALORCALCULADOIDA").ToString());
                    var valorCalculadoVolta = Convert.ToDecimal(this.grdPagamento.GetRowValues(rowIndex, "VALORCALCULADOVOLTA").ToString());
                    var txtValorFinal = DevExpressHelper.GetControl<TextBox>(this.grdPagamento, rowIndex, "ValorFinal", "txtValorFinal");


                    pagamentoRota.RotaId = !rotaId.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(rotaId) : -1;
                    pagamentoRota.Desconto = !txtDesconto.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(txtDesconto.Text) : 0;
                    pagamentoRota.QuantidadeAlunoIda = !quantidadeAlunoIda.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(quantidadeAlunoIda) : -1;
                    pagamentoRota.QuantidadeAlunoVolta = !quantidadeAlunoVolta.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(quantidadeAlunoVolta) : -1;
                    pagamentoRota.QuantidadeDiasIda = !quantidadeDiasIda.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(quantidadeDiasIda) : -1;
                    pagamentoRota.QuantidadeDiasVolta = !quantidadeDiasVolta.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(quantidadeDiasVolta) : -1;
                    pagamentoRota.QuantidadeKmIda = !quantidadeKmIda.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(quantidadeKmIda) : -1;
                    pagamentoRota.QuantidadeKmVolta = !quantidadeKmVolta.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(quantidadeKmVolta) : -1;
                    pagamentoRota.SituacaoPagamentoId = !ddlSituacaoPagamento.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlSituacaoPagamento.SelectedValue) : -1;
                    pagamentoRota.ValorRotaIda = !valorRotaIda.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(valorRotaIda) : -1;
                    pagamentoRota.ValorRotaVolta = !valorRotaVolta.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(valorRotaVolta) : -1;
                    pagamentoRota.UsuarioId = User.Identity.Name;
                    pagamentoRota.ValorTotal = ((valorCalculadoIda + valorCalculadoVolta) - pagamentoRota.Desconto);

                    valorTotalporRota = valorTotalporRota + pagamentoRota.ValorTotal;

                    listaPagamentosRota.Add(pagamentoRota);

                }

                pagamento.ValorTotal = valorTotalporRota;

                validacao = rnPagamento.ValidaGeracaoPagamento(pagamento, listaPagamentosRota);

                if (validacao.Valido)
                {
                    rnPagamento.GerarPagamento(pagamento, listaPagamentosRota);

                    lblMensagem.Text = "Pagamento gerado com sucesso";

                    hdnIdPagamento.Value = pagamento.PagamentoId.ToString();
                    odsPagamento.Select();
                    odsPagamento.DataBind();
                    grdPagamento.DataBind();
                    btnSalvar.Visible = false;
                    //lblValorTotalUnidade.Text = "R$ " + string.Format("{0:N2}", valorTotal);
                    //lblValorTotalUnidade.Visible = true;
                    //lblDescValorTotal.Visible = true;

                    var script = @"alert('" + lblMensagem.Text + @"');";
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                }
                else
                {
                    this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }


            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdPagamento, "Pagamento");

            var mp = (LyceumMaster)this.Master;

            if (mp != null)
            {
                mp.habilitaLoading = true;
            }

            this.Page.MaintainScrollPositionOnPostBack = false;

        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            try
            {
                ControlaAcesso(btnSalvar, AcaoControle.novo);
                if (!hdnIdPagamento.Value.IsNullOrEmptyOrWhiteSpace())
                {
                    btnSalvar.Visible = false;
                    btnCancelar.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            Page.Header.Controls.AddAt(0, new HtmlMeta { HttpEquiv = "X-UA-Compatible", Content = "IE=8" });
        }

        protected void txtDesconto_OnTextChanged(object sender, EventArgs e)
        {
            lblValorTotalUnidade.Text = Convert.ToString(ObtemTotal());

            //var indiceLinhaTabela = (((GridViewTableDataCell)(currentTextBox.Parent.Parent))).VisibleIndex;

            //var teste = Convert.ToString(this.grdPagamento.GetRowValues(indiceLinhaTabela, "ValorFinal"));
            //var teste2 = grdPagamento.GroupSummary["ValorFinal"];
            //.GetGroupSummaryValue(indiceLinhaTabela, "ValorFinal");

            //grdPagamento_CustomSummaryCalculate(sender, grdPagamento.GetGroupSummaryValue(indiceLinhaTabela, "ValorFinal"));
            //this.grdPagamento.GetRow(indiceLinhaTabela));
            //DevExpress.Data.CustomSummaryEventArgs e)

            //ASPxSummaryItemCollection teste3 = grdPagamento.GroupSummary;
            //ASPxSummaryItem teste4 = grdPagamento.TotalSummary["ValorFinal"];

            //this.odsPagamento.DataBind();

            //grdPagamento.TotalSummary.Remove(teste4);
            //grdPagamento.TotalSummary.Add(teste4);            
        }

        private decimal ObtemTotal()
        {
            decimal total = 0;

            for (var rowIndex = 0; rowIndex < this.grdPagamento.VisibleRowCount; rowIndex++)
            {
                var colDesconto = this.grdPagamento.Columns["DESCONTO"] as GridViewDataColumn;
                var txtDesconto = (TextBox)this.grdPagamento.FindRowCellTemplateControl(rowIndex, colDesconto, "txtDesconto");
                decimal ida = Convert.ToDecimal(this.grdPagamento.GetRowValues(rowIndex, "VALORCALCULADOIDA"));
                decimal volta = Convert.ToDecimal(this.grdPagamento.GetRowValues(rowIndex, "VALORCALCULADOVOLTA"));

                decimal desconto = Convert.ToString(txtDesconto.Text).IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToDecimal(txtDesconto.Text);

                total = total + ((ida + volta) - desconto);
            }

            return total;
        }

        protected void grdPagamento_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (!this.grdPagamento.Visible
             || this.grdPagamento.VisibleRowCount == 0)
            {
                return;
            }
            var colDesconto = this.grdPagamento.Columns["DESCONTO"] as GridViewDataColumn;
            var txtDesconto = (TextBox)this.grdPagamento.FindRowCellTemplateControl(e.VisibleIndex, colDesconto, "txtDesconto");
            var ddlSituacaoPagamento = DevExpressHelper.GetControl<DropDownList>(this.grdPagamento, e.VisibleIndex, "MOTIVO", "ddlSituacaoPagamento");
            var hdnSituacaoPagamento = DevExpressHelper.GetControl<HiddenField>(this.grdPagamento, e.VisibleIndex, "MOTIVO", "hdnSituacaoPagamento");
            var ida = Convert.ToDecimal(this.grdPagamento.GetRowValues(e.VisibleIndex, "VALORCALCULADOIDA"));
            var volta = Convert.ToDecimal(this.grdPagamento.GetRowValues(e.VisibleIndex, "VALORCALCULADOVOLTA"));
            var colNotaFinal = this.grdPagamento.Columns["ValorFinal"] as GridViewDataColumn;
            var txtValorFinal = (TextBox)this.grdPagamento.FindRowCellTemplateControl(e.VisibleIndex, colNotaFinal, "txtValorFinal");

            if (txtDesconto == null)
                return;

            if (!hdnIdPagamento.Value.IsNullOrEmptyOrWhiteSpace())
            {
                txtDesconto.ReadOnly = true;
                txtDesconto.Enabled = false;
                txtDesconto.BackColor = Color.Gainsboro;

                txtValorFinal.ReadOnly = true;
                txtValorFinal.BackColor = Color.Gainsboro;
                txtValorFinal.TabIndex = -1;

                ddlSituacaoPagamento.Enabled = false;
                ddlSituacaoPagamento.ClearSelection();

                var situacaoPagamento = string.IsNullOrEmpty(hdnSituacaoPagamento.Value) ? "Selecione" : hdnSituacaoPagamento.Value;

                ddlSituacaoPagamento.SelectedValue = situacaoPagamento;
            }
            else
            {
                txtDesconto.Attributes.Add("validar", "true");
                txtDesconto.Attributes.Add("input-ida", ida.ToString());
                txtDesconto.Attributes.Add("input-volta", volta.ToString());
                txtValorFinal.Attributes.Add("navegar", "false");
            }

            decimal desconto = Convert.ToString(txtDesconto.Text).IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToDecimal(txtDesconto.Text);
            txtValorFinal.Text = "R$ " + string.Format("{0:N2}", ((ida + volta) - desconto));

            //Verifica se é novo
            if (hdnIdPagamento.Value.IsNullOrEmptyOrWhiteSpace() && !lblValorTotalUnidade.Text.IsNullOrEmptyOrWhiteSpace())
            {
                //Calcula total da escola
                decimal totalAnterior = Convert.ToDecimal(lblValorTotalUnidade.Text);
                lblValorTotalUnidade.Text = Convert.ToString(totalAnterior + ((ida + volta) - desconto));
            }
        }

        protected void grdPagamento_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            RN.DTOs.DadosRota dadosRota = new Techne.Lyceum.RN.DTOs.DadosRota();
            RN.Transporte.Rota rnRota = new Techne.Lyceum.RN.Transporte.Rota();
            RN.Transporte.RotaAluno rnRotaAluno = new Techne.Lyceum.RN.Transporte.RotaAluno();
            RN.Turno rnTurno = new Techne.Lyceum.RN.Turno();
            RN.Transporte.Veiculo rnVeiculo = new Techne.Lyceum.RN.Transporte.Veiculo();
            RN.Transporte.Prestador rnPrestador = new Techne.Lyceum.RN.Transporte.Prestador();
            RN.Transporte.Condutor rnCondutor = new Techne.Lyceum.RN.Transporte.Condutor();
            RN.Transporte.Entidades.Prestador prestadorIda = new Techne.Lyceum.RN.Transporte.Entidades.Prestador();
            RN.Transporte.Entidades.Prestador prestadorVolta = new Techne.Lyceum.RN.Transporte.Entidades.Prestador();
            RN.Transporte.Entidades.Veiculo veiculoIda = new Techne.Lyceum.RN.Transporte.Entidades.Veiculo();
            RN.Transporte.Entidades.Veiculo veiculoVolta = new Techne.Lyceum.RN.Transporte.Entidades.Veiculo();
            RN.Transporte.Entidades.Condutor condutorIda = new Techne.Lyceum.RN.Transporte.Entidades.Condutor();
            RN.Transporte.Entidades.Condutor condutorVolta = new Techne.Lyceum.RN.Transporte.Entidades.Condutor();
            ValidacaoDados validacao = new ValidacaoDados();

            //Busca dados selecionado
            int rotaId = Convert.ToInt32(grdPagamento.GetRowValues(e.VisibleIndex, "ROTAID"));
            int diasIda = Convert.ToInt32(grdPagamento.GetRowValues(e.VisibleIndex, "QUANTIDADEDIASIDA"));
            int diasVolta = Convert.ToInt32(grdPagamento.GetRowValues(e.VisibleIndex, "QUANTIDADEDIASVOLTA"));
            int pagamentoId = hdnIdPagamento.Value.IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToInt32(hdnIdPagamento.Value);

            try
            {
                if (e.ButtonID == "btnDados")
                {
                    LimpaCamposRota();

                    //Verifica se é consulta
                    if (pagamentoId > 0)
                    {
                        //Busca dados da rota
                        dadosRota = rnRota.ObtemDadosRotaPagamentoPor(pagamentoId);

                        lblQuantidadeAlunoIda.Text = Convert.ToString(dadosRota.QuantidadeAlunosIda);
                        lblQuantidadeAlunoVolta.Text = Convert.ToString(dadosRota.QuantidadeAlunosVolta);
                    }
                    else
                    {
                        //Busca dados da rota
                        dadosRota = rnRota.ObtemDadosRotaPor(rotaId);

                        //Busca quantidades de alunos
                        lblQuantidadeAlunoIda.Text = Convert.ToString(rnRotaAluno.ObtemAlunosAtivosPor(dadosRota.RotaTrajetoIdIda, DateTime.Now));
                        lblQuantidadeAlunoVolta.Text = Convert.ToString(rnRotaAluno.ObtemAlunosAtivosPor(dadosRota.RotaTrajetoIdVolta, DateTime.Now));
                    }

                    lblCodigo.Text = dadosRota.Codigo;
                    lblEscola.Text = RN.UnidadeEnsino.RetornaNomeUnidadeEnsino(dadosRota.Censo);
                    lblRegionalRota.Text = dadosRota.RegionalDescricao;
                    lblMunicipioRota.Text = dadosRota.MunicipioDescricao;
                    lblRegiaoFinanceira.Text = dadosRota.RegiaoFinanceiraDescricao;
                    lblCnpj.Text = dadosRota.Cnpj.AplicarMascaraCNPJ();
                    lblTurno.Text = rnTurno.RetornaDescricaoTurno(dadosRota.Turno);
                    lblTipoCalculoPagamento.Text = dadosRota.TipoCalculoPagamento;
                    lblTipoContratacaoIda.Text = dadosRota.TipoContratacaoDescricaoIda;
                    lblValorRotaIda.Text = Convert.ToString(dadosRota.ValorRotaIda);
                    lblQuantidadeKmIda.Text = Convert.ToString(dadosRota.QuantidadeKmIda);
                    lblTipoContratacaoVolta.Text = dadosRota.TipoContratacaoDescricaoVolta.ToString();
                    lblValorRotaVolta.Text = Convert.ToString(dadosRota.ValorRotaVolta);
                    lblQuantidadeKmVolta.Text = Convert.ToString(dadosRota.QuantidadeKmVolta);
                    lblQtdeDiasIda.Text = diasIda.ToString();
                    lblQtdeDiasVolta.Text = diasVolta.ToString();

                    //Busca dados do prestador
                    prestadorIda = rnPrestador.ObtemPor(dadosRota.PrestadorIdIda);
                    prestadorVolta = rnPrestador.ObtemPor(dadosRota.PrestadorIdVolta);
                    lblPrestadorIda.Text = string.Format("{0} - {1}", prestadorIda.Cnpj.IsNullOrEmptyOrWhiteSpace() ? prestadorIda.Cpf : prestadorIda.Cnpj, prestadorIda.Nome);
                    lblPrestadorVolta.Text = string.Format("{0} - {1}", prestadorVolta.Cnpj.IsNullOrEmptyOrWhiteSpace() ? prestadorVolta.Cpf : prestadorVolta.Cnpj, prestadorVolta.Nome);

                    //Busca dados do condutor
                    condutorIda = rnCondutor.ObtemPor(dadosRota.CondutorIdIda);
                    condutorVolta = rnCondutor.ObtemPor(dadosRota.CondutorIdVolta);
                    lblCondutorIda.Text = string.Format("{0} - {1}", condutorIda.Cpf, condutorIda.Nome);
                    lblCondutorVolta.Text = string.Format("{0} - {1}", condutorVolta.Cpf, condutorVolta.Nome);

                    //Busca dados do veiculo
                    veiculoIda = rnVeiculo.ObtemPor(dadosRota.VeiculoIdIda);
                    veiculoVolta = rnVeiculo.ObtemPor(dadosRota.VeiculoIdVolta);
                    if (!veiculoIda.Placa.IsNullOrEmptyOrWhiteSpace() && !veiculoIda.Nome.IsNullOrEmptyOrWhiteSpace())
                    {
                        lblVeiculoIda.Text = string.Format("{0} - {1}", veiculoIda.Placa, veiculoIda.Nome);

                        if (veiculoIda.QuantidadeAssentos != 0)
                        {
                            lblVeiculoIda.Text = lblVeiculoIda.Text + " com " + Convert.ToString(veiculoIda.QuantidadeAssentos) + " assentos";
                        }
                    }
                    else
                    {
                        lblVeiculoIda.Text = "-";
                    }


                    if (!veiculoVolta.Placa.IsNullOrEmptyOrWhiteSpace() && !veiculoVolta.Nome.IsNullOrEmptyOrWhiteSpace())
                    {
                        lblVeiculoVolta.Text = string.Format("{0} - {1}", veiculoVolta.Placa, veiculoVolta.Nome);

                        if (veiculoVolta.QuantidadeAssentos != 0)
                        {
                            lblVeiculoVolta.Text = lblVeiculoVolta.Text + " com " + Convert.ToString(veiculoVolta.QuantidadeAssentos) + " assentos";
                        }
                    }
                    else
                    {
                        lblVeiculoVolta.Text = "-";
                    }

                    pucRota.ShowOnPageLoad = true;
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                pucRota.ShowOnPageLoad = false;
            }
        }

        protected void LimpaCamposRota()
        {
            lblCodigo.Text = string.Empty;
            lblEscola.Text = string.Empty;
            lblRegionalRota.Text = string.Empty;
            lblMunicipioRota.Text = string.Empty;
            lblRegiaoFinanceira.Text = string.Empty;
            lblCnpj.Text = string.Empty;
            lblTurno.Text = string.Empty;
            lblTipoCalculoPagamento.Text = string.Empty;
            lblTipoContratacaoIda.Text = string.Empty;
            lblValorRotaIda.Text = string.Empty;
            lblQuantidadeKmIda.Text = string.Empty;
            lblQuantidadeAlunoIda.Text = string.Empty;
            lblPrestadorIda.Text = string.Empty;
            lblCondutorIda.Text = string.Empty;
            lblVeiculoIda.Text = string.Empty;
            lblTipoContratacaoVolta.Text = string.Empty;
            lblValorRotaVolta.Text = string.Empty;
            lblQuantidadeKmVolta.Text = string.Empty;
            lblQuantidadeAlunoVolta.Text = string.Empty;
            lblPrestadorVolta.Text = string.Empty;
            lblCondutorVolta.Text = string.Empty;
            lblVeiculoVolta.Text = string.Empty;
            lblQtdeDiasIda.Text = string.Empty;
            lblQtdeDiasVolta.Text = string.Empty;
        }

        protected void grdPagamento_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "ValorFinal")
            {
                var ida = e.GetListSourceFieldValue("VALORCALCULADOIDA");
                var volta = e.GetListSourceFieldValue("VALORCALCULADOVOLTA");
                var desconto = e.GetListSourceFieldValue("DESCONTO") != DBNull.Value ? e.GetListSourceFieldValue("DESCONTO") : 0;

                // var valorFormatado = string.Format("{0:N2}", valor);

                e.Value = ((Convert.ToDecimal(ida) + Convert.ToDecimal(volta)) - Convert.ToDecimal(desconto));
            }
        }

        //protected void grdPagamento_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        //{            
        //    decimal total = (e.TotalValue == null ? 0 : Convert.ToDecimal(e.TotalValue));
        //    switch (e.SummaryProcess)
        //    {
        //        case DevExpress.Data.CustomSummaryProcess.Start: total = 0; break;
        //        case DevExpress.Data.CustomSummaryProcess.Calculate:
        //            total += Convert.ToDecimal(e.GetValue("ValorFinal"));
        //            e.TotalValue = total;
        //            e.TotalValueReady = true;
        //            break;
        //        case DevExpress.Data.CustomSummaryProcess.Finalize:
        //            e.TotalValue = total;
        //            e.TotalValueReady = true;
        //            break;
        //    }
        //}
    }
}
