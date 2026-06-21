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

namespace Techne.Lyceum.Net.Matricula
{
    [NavUrl("~/Matricula/ConfirmacaoCandidato.aspx")]
    [ControlText("ConfirmacaoCandidato")]
    [Title("Confirmação Candidato")]

    public partial class ConfirmacaoCandidato : TPage
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
                return opcao.ListaConvocadosPor(unidade, Convert.ToInt32(anoFiltro), Convert.ToInt32(semestre), Convert.ToInt32(faseMatricula));

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
                        if (!this.tseUnidadeResponsavel["unidade_ens"].IsNull)
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
            int opcaoInscricaoId = Convert.ToInt32(grdControle.GetRowValues(e.VisibleIndex, "OPCAOINSCRICAOID"));

            string queryString = string.Empty;

            if (e.ButtonID == "btnConfirmar")
            {
                int preCadastroAlunoId = Convert.ToInt32(grdControle.GetRowValues(e.VisibleIndex, "PRECADASTROALUNOID"));
                int controleVagaId = Convert.ToInt32(grdControle.GetRowValues(e.VisibleIndex, "CONTROLEVAGAID"));
                DateTime dataNascimento = Convert.ToDateTime(grdControle.GetRowValues(e.VisibleIndex, "DATANASCIMENTO"));
                decimal pessoa = grdControle.GetRowValues(e.VisibleIndex, "PESSOAID") != DBNull.Value ? Convert.ToDecimal(grdControle.GetRowValues(e.VisibleIndex, "PESSOAID")) : 0;
                string nomeMae = grdControle.GetRowValues(e.VisibleIndex, "NOMEMAE").ToString();
                string cpf = grdControle.GetRowValues(e.VisibleIndex, "CPF") != null ? grdControle.GetRowValues(e.VisibleIndex, "CPF").ToString() : null;
                string censo = grdControle.GetRowValues(e.VisibleIndex, "CENSO").ToString();
                queryString = MontarQueryStringConfirmacao(opcaoInscricaoId, preCadastroAlunoId, controleVagaId, dataNascimento, nomeMae, pessoa, cpf, censo);

                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                Response.Redirect("ConfirmaCandidato.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
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
