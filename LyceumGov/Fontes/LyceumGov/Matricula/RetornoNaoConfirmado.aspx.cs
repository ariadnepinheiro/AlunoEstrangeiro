using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.Net.Basico;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxGridView;
using Techne.Web;
using DevExpress.Web.ASPxClasses;
using Seeduc.Infra.Helpers;

namespace Techne.Lyceum.Net.Matricula
{
    [NavUrl("~/Matricula/RetornoNaoConfirmado.aspx")]
    [ControlText("RetornoConfirmacaoCandidato")]
    [Title("Retorno Confirmação Candidato")]

    public partial class RetornoNaoConfirmado : TPage
    {
        public object Listar(object unidade_ens, object ano, object periodo, object fase)
        {
            RN.Matriculas.OpcaoInscricao opcao = new Techne.Lyceum.RN.Matriculas.OpcaoInscricao();

            var anoFiltro = ano != null ? ano.ToString() : null;
            var unidade = unidade_ens != null ? unidade_ens.ToString() : null;
            var semestre = periodo != null ? periodo.ToString() : null;
            var faseMatricula = fase != null ? fase.ToString() : null;

            if (!anoFiltro.IsNullOrEmptyOrWhiteSpace() && !unidade.IsNullOrEmptyOrWhiteSpace() && !semestre.IsNullOrEmptyOrWhiteSpace() && !faseMatricula.IsNullOrEmptyOrWhiteSpace())
            {
                return opcao.ListaNaoConfirmadosPor(unidade, Convert.ToInt32(anoFiltro), Convert.ToInt32(semestre), Convert.ToInt32(faseMatricula));

            }
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.lblMensagem.Text = string.Empty;
                RN.DTOs.DadosConfirmacaoCandidato dadosCandidato = new Techne.Lyceum.RN.DTOs.DadosConfirmacaoCandidato();
                RN.DTOs.DadosControleVaga dadosControleVaga = new Techne.Lyceum.RN.DTOs.DadosControleVaga();

                if (!this.IsPostBack)
                {
                    if (Request.QueryString.Keys.Count > 0)
                    {

                        if (Request.QueryString["ChaveConfirmacao"] != null)
                        {
                            byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["ChaveConfirmacao"]);
                            string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

                            LimparCampos();

                            RN.ControleVaga rnControleVaga = new Techne.Lyceum.RN.ControleVaga();

                            dadosControleVaga = rnControleVaga.ObtemDadosControleVagaPor(Convert.ToInt32(decodedText));

                            PreencherTela(dadosControleVaga);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdControle, string.Empty);
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnBuscar, AcaoControle.novo);
            ControlaAcesso(grdControle);
        }

        private void CarregaAno()
        {
            cmbAno.Items.Clear();
            ListItem item = new ListItem("Selecione", string.Empty);
            cmbAno.DataSource = RN.PeriodoLetivo.ListarAnos();
            cmbAno.DataBind();
            cmbAno.Items.Insert(0, item);
        }


        protected void cmbAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.cmbPeriodo.Items.Clear();
                this.ddlFase.ClearSelection();
                this.pnGrid.Visible = false;

                if (!string.IsNullOrEmpty(cmbAno.SelectedValue))
                {
                    ListItem item = new ListItem("Selecione", string.Empty);
                    this.cmbPeriodo.DataSource = RN.PeriodoLetivo.ListarPeriodo(this.cmbAno.SelectedValue);
                    this.cmbPeriodo.DataBind();
                    this.cmbPeriodo.Items.Insert(0, item);
                }
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
                if (this.Page.IsCallback)
                {
                    return;
                }

                var sessao = SessaoUsuario.GetSessaoUsuario();
                tseUnidadeResponsavel.ResetValue();
                this.cmbAno.Items.Clear();
                this.cmbPeriodo.Items.Clear();
                this.ddlFase.ClearSelection();
                this.pnGrid.Visible = false;
                this.pnGrid.Visible = false;

                if (sessao != null)
                {
                    if (!this.tseMunicipio.DBValue.IsNull)
                    {
                        if (this.tseMunicipio.IsValidDBValue)
                        {
                            sessao.Municipio = Convert.ToString(this.tseMunicipio.DBValue);

                            sessao.Escola = string.Empty;
                            this.tseUnidadeResponsavel.ResetValue();
                        }
                        else
                        {
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;
                        }
                    }
                    else
                    {
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                    }
                }
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
                var sessao = SessaoUsuario.GetSessaoUsuario();
                this.cmbAno.Items.Clear();
                this.cmbPeriodo.Items.Clear();
                this.ddlFase.ClearSelection();

                this.pnGrid.Visible = false;


                if (!this.tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (this.tseUnidadeResponsavel.IsValidDBValue)
                    {
                        if (!this.tseUnidadeResponsavel["SETOR"].IsNull)
                        {
                            CarregaAno();
                            sessao.Escola = Convert.ToString(this.tseUnidadeResponsavel.DBValue);
                            this.tseMunicipio.Value = this.tseUnidadeResponsavel["municipio"];
                        }
                    }
                    else
                    {
                        if (sessao != null)
                        {
                            sessao.Escola = string.Empty;
                            sessao.Municipio = string.Empty;
                            sessao.Coordenadoria = string.Empty;
                        }


                        this.lblMensagem.Text = "Unidade de Ensino não cadastrada.";

                    }
                }
                else
                {
                    if (sessao != null)
                    {
                        sessao.Escola = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Coordenadoria = string.Empty;
                    }

                    this.lblMensagem.Text = "Favor consultar uma unidade de ensino.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimparCampos()
        {
            this.lblMensagem.Text = string.Empty;
            this.tseMunicipio.ResetValue();
            this.tseUnidadeResponsavel.ResetValue();
            this.cmbAno.SelectedIndex = 0;
            this.cmbPeriodo.Items.Clear();
            this.ddlFase.ClearSelection();

            this.pnGrid.Visible = false;

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                this.pnGrid.Visible = false;
                if ((tseUnidadeResponsavel.DBValue.IsNull && !tseUnidadeResponsavel.IsValidDBValue) || cmbAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() || cmbPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace() || ddlFase.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    lblMensagem.Text = "Para efetuar a busca da lista de candidatos é necessário preencher todos os campos de filtro.";
                    return;
                }
                else
                {
                    this.pnGrid.Visible = true;
                    odsControle.Select();
                    grdControle.DataBind();
                }


            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void hplVisualizarDados_Click(object sender, EventArgs e)
        {
            try
            {
                var script = string.Empty;
                var inscricao = (sender as LinkButton).CommandArgument.ToString();

                if (!inscricao.IsNullOrEmptyOrWhiteSpace())
                {
                    byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(inscricao);

                    script = @"window.open('CadastroCandidato.aspx?Chave=" + Convert.ToBase64String(bytesToEncode) + @"','', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=900, height=600, resizable=yes'); ";

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void grdControle_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {           
            if (e.ButtonID == "btnConfirmar")
            {
                RN.Matriculas.OpcaoInscricao opcao = new Techne.Lyceum.RN.Matriculas.OpcaoInscricao();
                RN.DTOs.DadosRetornoOpcaoNaoConfirmada dados = new Techne.Lyceum.RN.DTOs.DadosRetornoOpcaoNaoConfirmada();
                ValidacaoDados validacao = new ValidacaoDados();

                var ddlMotivo = DevExpressHelper.GetControl<DropDownList>(this.grdControle, e.VisibleIndex, "MOTIVORETORNOID", "ddlMotivo");

                dados.ControleVagaId = Convert.ToInt32(grdControle.GetRowValues(e.VisibleIndex, "CONTROLEVAGAID"));
                dados.DataRetorno = DateTime.Now;
                dados.Fase = !ddlFase.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlFase.SelectedValue) : -1;
                dados.InscricaoAlunoId = Convert.ToInt32(grdControle.GetRowValues(e.VisibleIndex, "INSCRICAOALUNOID"));
                dados.MotivoRetornoId = !ddlMotivo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlMotivo.SelectedValue): -1 ;
                dados.OpcaoInscricaoId = Convert.ToInt32(grdControle.GetRowValues(e.VisibleIndex, "OPCAOINSCRICAOID"));
                dados.UsuarioId = User.Identity.Name;
                               

                validacao = opcao.ValidaRetornoNaoConfirmado(dados);

                if (validacao.Valido)
                {
                    opcao.RetornaNaoConfirmado(dados);
                    grdControle.DataBind();
                    lblMensagem.Text = "Inscrição disponível novamente para confirmação.";
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
        }


        private string MontarQueryStringConfirmacao(int OpcaoInscricaoId, int PreCadastroAlunoId, int ControleVagaId, DateTime DataNascimento, string NomeMae, decimal Pessoa, string Cpf, string Censo)
        {
            string queryString = string.Empty;
            queryString += "OpcaoInscricaoId=" + OpcaoInscricaoId;
            queryString += "&PreCadastroAlunoId=" + PreCadastroAlunoId;
            queryString += "&ControleVagaId=" + ControleVagaId;
            queryString += "&DataNascimento=" + DataNascimento;
            queryString += "&Pessoa=" + Pessoa;
            queryString += "&Censo=" + Censo;
            queryString += "&Mae=" + NomeMae;
            queryString += "&CPF=" + Cpf;


            return queryString;
        }

        private void PreencherTela(RN.DTOs.DadosControleVaga dados)
        {
            tseUnidadeResponsavel.DBValue = dados.Censo;
            tseUnidadeResponsavel_Changed(null, null);
            cmbAno.SelectedValue = dados.Ano.ToString();
            cmbAno_SelectedIndexChanged(null, null);
            cmbPeriodo.SelectedValue = dados.Periodo.ToString();
            btnBuscar_Click(null, null);
        }


    }
}
