using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using DevExpress.Web.ASPxTabControl;
using Techne.Lyceum.RN.Util;
using Techne.Controls;
using System.Data;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxEditors;
using Seeduc.Infra.Data;
using Seeduc.Infra.Helpers;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.PrestacaoContas.DTOs;
using System.Data.SqlTypes;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [
        NavUrl("~/PrestacaoContas/InformacoesAdicionaisAAE.aspx"),
        ControlText("InformacoesAdicionaisAAE"),
        Title("Informacoes Adicionais AAE"),
    ]
    public partial class InformacoesAdicionaisAAE : TPage
    {



        public enum TipoOperacao
        {
            Novo,
            Alterar,
            Consultar,
            Inicial,
            Sucesso,
            Excluir,
            Desativar
        }

        private TipoOperacao _tipoOperacao
        {
            get
            {
                if (ViewState["_tipoOperacao"] != null)
                {
                    if (ViewState["_tipoOperacao"] is TipoOperacao)
                    {
                        return (TipoOperacao)ViewState["_tipoOperacao"];
                    }
                }

                return TipoOperacao.Inicial;
            }

            set
            {
                ViewState["_tipoOperacao"] = value;
            }
        }

        public object ListaObrigacoesFiscais(object censo)
        {
            RN.PrestacaoContas.ObrigacaoFiscalAae rnObrigacaoFiscalAae = new Techne.Lyceum.RN.PrestacaoContas.ObrigacaoFiscalAae();

            if (censo != null)
            {
                if (!string.IsNullOrEmpty(censo.ToString()))
                {
                    return rnObrigacaoFiscalAae.ListaDeclaracaoAaePor(censo.ToString());
                }
            }
            return null;
        }

        public object ListaMandatosAae(object censo)
        {
            RN.PrestacaoContas.MandatoAae rnMandatoAae = new Techne.Lyceum.RN.PrestacaoContas.MandatoAae();

            if (censo != null)
            {
                if (!string.IsNullOrEmpty(censo.ToString()))
                {
                    return rnMandatoAae.ListaPor(censo.ToString());
                }
            }
            return null;
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
            ddlAno.DataSource = rnPeriodoLetivo.ListaAnosAcima2015(false);
            ddlAno.DataBind();
            ddlAno.Items.Insert(0, new ListItem("Selecione", string.Empty));
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
                        byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                        string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

                        tseUnidadeResponsavel.DBValue = decodedText;
                        tseUnidadeResponsavel_Changed(null, null);
                    }
                    else
                    {
                        _tipoOperacao = TipoOperacao.Inicial;
                        ControlarTipoOperacao();
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
            TituloGrid(grdObrigacoesFiscais, "");
            TituloGrid(grdMandatosAae, "");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdObrigacoesFiscais);
            ControlaAcesso(grdMandatosAae);
            ControlaAcesso(grdMandatosAae, AcaoControle.editar, "btnExcluir");
            ControlaAcesso(btnEditar, Techne.Lyceum.Net.TPage.AcaoControle.editar);
            ControlaAcesso(btnSalvar, Techne.Lyceum.Net.TPage.AcaoControle.editar);
        }

        private void ControlarTipoOperacao()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { };
                        ControlarVisibilidadeControle(controles, null);
                        ControlarTSearchs();
                        LimpaDadosInfoGerais();

                        tseUnidadeResponsavel.ResetValue();
                        pcFornecedor.Visible = false;
                        pcFornecedor.ActiveTabIndex = 0;

                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { };

                        controles = new[] { btnEditar };

                        ControlarVisibilidadeControle(controles, null);
                        ControlarTSearchs();

                        HabilitaDesabilitaCamposAbaInformacoesGerais(false);
                        grdMandatosAae.DataBind();

                        pcFornecedor.ActiveTabIndex = 1;
                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        RN.PrestacaoContas.MandatoAae rnMandatoAEE = new Techne.Lyceum.RN.PrestacaoContas.MandatoAae();
                        RN.PrestacaoContas.Entidades.MandatoAae fornecedor = new Techne.Lyceum.RN.PrestacaoContas.Entidades.MandatoAae();

                        ImageButton[] controles = new ImageButton[] { btnEditar };
                        ControlarVisibilidadeControle(controles, null);
                        ControlarTSearchs();
                        pcFornecedor.Visible = true;
                        pcFornecedor.ActiveTabIndex = 0;

                        LimpaDadosInfoGerais();
                        LimpaDadosTesoureiro();
                        LimpaDadosPresidente();

                        HabilitaDesabilitaCamposAbaInformacoesGerais(false);


                        var dadosInformacaoAdcionalAEE = rnMandatoAEE.ObtemDadosUnidadeAaePor(tseUnidadeResponsavel.DBValue.ToString());
                        Session["DadosRetorno"] = dadosInformacaoAdcionalAEE;

                        if (!String.IsNullOrEmpty(dadosInformacaoAdcionalAEE.Censo))
                        {
                            CarregarMes();
                            CarregaAno();
                            hdnMandatoAae.Value = Convert.ToString(dadosInformacaoAdcionalAEE.MandatoAaeId);
                            grdMandatosAae.DataBind();

                            txtEndCompl.Text = !dadosInformacaoAdcionalAEE.Complemento.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.Complemento : string.Empty;
                            txtEndNum.Text = !dadosInformacaoAdcionalAEE.Numero.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.Numero : string.Empty;
                            txtBairro.Text = !dadosInformacaoAdcionalAEE.Bairro.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.Bairro.ToUpper() : string.Empty;
                            txtBanco.Text = !dadosInformacaoAdcionalAEE.Banco.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.Banco.ToUpper() : string.Empty;
                            txtAgencia.Text = !dadosInformacaoAdcionalAEE.Agencia.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.Agencia.ToUpper() : string.Empty;
                            txtContaCorrente.Text = !dadosInformacaoAdcionalAEE.ContaCorrente.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.ContaCorrente.ToUpper() : string.Empty;
                            txtMotivoImpedimento.Text = !dadosInformacaoAdcionalAEE.MotivoImpedimento.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.MotivoImpedimento.ToUpper() : string.Empty;

                            txtNomePresAAE.Text = !dadosInformacaoAdcionalAEE.PresidenteNome.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.PresidenteNome.ToUpper() : string.Empty;
                            txtRgPresAAE.Text = !dadosInformacaoAdcionalAEE.PresidenteRg.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.PresidenteRg.ToUpper() : string.Empty;
                            txtCpfPresAAE.Text = !dadosInformacaoAdcionalAEE.PresidenteCpf.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.PresidenteCpf.ToUpper() : string.Empty;

                            txtEnderecoPresAAE.Text = !dadosInformacaoAdcionalAEE.PresidenteEndereco.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.PresidenteEndereco.ToUpper() : string.Empty;
                            txtNumeroPresAAE.Text = !dadosInformacaoAdcionalAEE.PresidenteNumero.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.PresidenteNumero.ToUpper() : string.Empty;
                            txtComplementoPresAAE.Text = !dadosInformacaoAdcionalAEE.PresidenteComplemento.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.PresidenteComplemento.ToUpper() : string.Empty;

                            txtBairroPresAAE.Text = !dadosInformacaoAdcionalAEE.PresidenteBairro.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.PresidenteBairro.ToUpper() : string.Empty;
                            txtMunicipioPresidenteAAE.Text = !dadosInformacaoAdcionalAEE.PresidenteMunicipio.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.PresidenteMunicipio.ToUpper() : string.Empty;
                            txtEmailPresAAE.Text = !dadosInformacaoAdcionalAEE.PresidenteEmail.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.PresidenteEmail.ToUpper() : string.Empty;
                            txtTelefonePresAAE.Text = !dadosInformacaoAdcionalAEE.PresidenteTelefone.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.PresidenteTelefone.ToUpper() : string.Empty;

                            txtIdFuncionalPresAAE.Text = !dadosInformacaoAdcionalAEE.PresidenteIdFuncional.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.PresidenteIdFuncional.ToUpper() : string.Empty;
                            txtMatriculaPresAAE.Text = !dadosInformacaoAdcionalAEE.PresidenteMatricula.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.PresidenteMatricula.ToUpper() : string.Empty;

                            txtRgTesoureiroAAE.Text = !dadosInformacaoAdcionalAEE.TesoureiroRg.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.TesoureiroRg.ToUpper() : string.Empty;
                            txtCpfTesoureiroAAE.Text = !dadosInformacaoAdcionalAEE.TesoureiroCpf.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.TesoureiroCpf.ToUpper() : string.Empty;

                            txtEnderecoTesoureiroAAE.Text = !dadosInformacaoAdcionalAEE.TesoureiroEndereco.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.TesoureiroEndereco.ToUpper() : string.Empty;
                            txtNumeroTesoureiroAAE.Text = !dadosInformacaoAdcionalAEE.TesoureiroNumero.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.TesoureiroNumero.ToUpper() : string.Empty;
                            txtComplementoTesoureiroAAE.Text = !dadosInformacaoAdcionalAEE.TesoureiroComplemento.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.TesoureiroComplemento.ToUpper() : string.Empty;

                            txtBairroTesoureiroAAE.Text = !dadosInformacaoAdcionalAEE.TesoureiroBairro.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.TesoureiroBairro.ToUpper() : string.Empty;
                            txtMunicipioTesoureiroAAE.Text = !dadosInformacaoAdcionalAEE.TesoureiroMunicipio.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.TesoureiroMunicipio.ToUpper() : string.Empty;

                            txtEmailTesoureiroAAE.Text = !dadosInformacaoAdcionalAEE.TesoureiroEmail.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.TesoureiroEmail.ToUpper() : string.Empty;
                            txtTelefoneTesoureiroAAE.Text = !dadosInformacaoAdcionalAEE.TesoureiroTelefone.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.TesoureiroTelefone.ToUpper() : string.Empty;
                            txtIdFuncionalTesoureiroAAE.Text = !dadosInformacaoAdcionalAEE.TesoureiroIdFuncional.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.TesoureiroIdFuncional.ToUpper() : string.Empty;
                            txtFinalDoMandato.Text = !String.IsNullOrEmpty(dadosInformacaoAdcionalAEE.FimMandato.ToString()) ? dadosInformacaoAdcionalAEE.FimMandato.ToShortDateString() : "";

                            txtRegional.Text = !dadosInformacaoAdcionalAEE.Regional.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.Regional.ToUpper() : string.Empty;
                            txtEndereco.Text = !dadosInformacaoAdcionalAEE.Endereco.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.Endereco.ToUpper() : string.Empty;
                            txtEndNum.Text = !dadosInformacaoAdcionalAEE.Numero.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.Numero.ToUpper() : string.Empty;
                            txtEndCompl.Text = !dadosInformacaoAdcionalAEE.Complemento.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.Complemento.ToUpper() : string.Empty;
                            txtBairro.Text = !dadosInformacaoAdcionalAEE.Bairro.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.Bairro.ToUpper() : string.Empty;
                            txtMunicipio.Text = !dadosInformacaoAdcionalAEE.Municipio.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.Municipio.ToUpper() : string.Empty;
                            txtTelefone.Text = !dadosInformacaoAdcionalAEE.Telefone.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.Telefone.ToUpper() : string.Empty;

                            txtDiretor.Text = !dadosInformacaoAdcionalAEE.Diretor.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.Diretor.ToUpper() : string.Empty;
                            txtCenso.Text = !dadosInformacaoAdcionalAEE.Censo.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.Censo.ToUpper() : string.Empty;
                            txtEmail.Text = !dadosInformacaoAdcionalAEE.Email.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.Email.ToUpper() : string.Empty;
                            txtNumeroAlunos.Text = !dadosInformacaoAdcionalAEE.NumeroAluno.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.NumeroAluno.ToUpper() : string.Empty;
                            if (dadosInformacaoAdcionalAEE.MandatoAaeId == 0)
                            {
                                if (dadosInformacaoAdcionalAEE.InicioMandato != DateTime.MinValue)
                                {
                                    txtInicioMandato.Text = !dadosInformacaoAdcionalAEE.InicioMandato.ToShortDateString().IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.InicioMandato.ToShortDateString().ToUpper() : string.Empty;
                                }
                                else
                                {
                                    txtInicioMandato.Text = string.Empty;
                                }

                                if (dadosInformacaoAdcionalAEE.FimMandato != DateTime.MinValue)
                                {
                                    txtFinalDoMandato.Text = !String.IsNullOrEmpty(dadosInformacaoAdcionalAEE.FimMandato.ToString()) ? dadosInformacaoAdcionalAEE.FimMandato.ToShortDateString() : "";
                                }
                                else
                                {
                                    txtFinalDoMandato.Text = string.Empty;
                                }

                            }
                            else if (dadosInformacaoAdcionalAEE.MandatoAaeId != 0)
                            {
                                txtInicioMandato.Text = !dadosInformacaoAdcionalAEE.InicioMandato.ToShortDateString().IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.InicioMandato.ToShortDateString().ToUpper() : string.Empty;
                                txtFinalDoMandato.Text = !String.IsNullOrEmpty(dadosInformacaoAdcionalAEE.FimMandato.ToString()) ? dadosInformacaoAdcionalAEE.FimMandato.ToShortDateString() : "";
                                lnkVisualizarRelatorio.Text = !dadosInformacaoAdcionalAEE.NomeArquivo.IsNullOrEmptyOrWhiteSpace() ? dadosInformacaoAdcionalAEE.NomeArquivo : string.Empty;
                                txtMandato.Text = dadosInformacaoAdcionalAEE.Mandato > 0 ? dadosInformacaoAdcionalAEE.Mandato.ToString() : string.Empty;

                            }

                            if (dadosInformacaoAdcionalAEE.TesoureiroPessoa != null)
                            {
                                chkPossuiIdFunc.Checked = true;
                                chkPossuiIdFunc_Clicked(null, null);
                                tseTesoureiro.DBValue = Convert.ToString(dadosInformacaoAdcionalAEE.TesoureiroPessoa);
                            }
                            else if (dadosInformacaoAdcionalAEE.TesoureiroId != null)
                            {
                                chkNaoPossuiIdFunc.Checked = true;
                                chkNaoPossuiIdFunc_Clicked(null, null);
                                tseTesoureiro.DBValue = Convert.ToString(dadosInformacaoAdcionalAEE.TesoureiroId);
                            }

                            txtRgTesoureiroAAE.Text = !String.IsNullOrEmpty(dadosInformacaoAdcionalAEE.TesoureiroRg) ? dadosInformacaoAdcionalAEE.TesoureiroRg : "";
                            txtCpfTesoureiroAAE.Text = !String.IsNullOrEmpty(dadosInformacaoAdcionalAEE.TesoureiroCpf) ? dadosInformacaoAdcionalAEE.TesoureiroCpf : "";
                            txtEnderecoTesoureiroAAE.Text = !String.IsNullOrEmpty(dadosInformacaoAdcionalAEE.TesoureiroEndereco) ? dadosInformacaoAdcionalAEE.TesoureiroEndereco : "";
                            txtNumeroTesoureiroAAE.Text = !String.IsNullOrEmpty(dadosInformacaoAdcionalAEE.TesoureiroNumero) ? dadosInformacaoAdcionalAEE.TesoureiroNumero : "";
                            txtComplementoTesoureiroAAE.Text = !String.IsNullOrEmpty(dadosInformacaoAdcionalAEE.TesoureiroComplemento) ? dadosInformacaoAdcionalAEE.TesoureiroComplemento : "";
                            txtBairroTesoureiroAAE.Text = !String.IsNullOrEmpty(dadosInformacaoAdcionalAEE.TesoureiroBairro) ? dadosInformacaoAdcionalAEE.TesoureiroBairro : "";
                            txtMunicipioTesoureiroAAE.Text = !String.IsNullOrEmpty(dadosInformacaoAdcionalAEE.TesoureiroMunicipio) ? dadosInformacaoAdcionalAEE.TesoureiroMunicipio : "";
                            txtEmailTesoureiroAAE.Text = !String.IsNullOrEmpty(dadosInformacaoAdcionalAEE.TesoureiroEmail) ? dadosInformacaoAdcionalAEE.TesoureiroEmail : "";
                            txtTelefoneTesoureiroAAE.Text = !String.IsNullOrEmpty(dadosInformacaoAdcionalAEE.TesoureiroTelefone) ? dadosInformacaoAdcionalAEE.TesoureiroTelefone : "";
                            txtIdFuncionalTesoureiroAAE.Text = !String.IsNullOrEmpty(dadosInformacaoAdcionalAEE.TesoureiroIdFuncional) ? dadosInformacaoAdcionalAEE.TesoureiroIdFuncional : "";


                            Int64 result;
                            string fone = dadosInformacaoAdcionalAEE.Telefone.RetirarMascaraTelefone();
                            if (Int64.TryParse(fone, out result))
                            {
                                txtTelefone.Text = string.Format("{0:(00)0000-0000}", result);
                            }
                            else
                            {
                                txtTelefone.Text = dadosInformacaoAdcionalAEE.Telefone;
                            }
                        }

                        break;

                    }
                case TipoOperacao.Alterar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };

                        ControlarVisibilidadeControle(controles, null);
                        ControlarTSearchs();
                        HabilitaDesabilitaCamposAbaInformacoesGerais(true);

                        break;

                    }

            }
        }

        protected void txtInicioMandato_ValueChanged(object sender, EventArgs e)
        {
            DateTime temp;
            if (!txtInicioMandato.Text.IsNullOrEmptyOrWhiteSpace())
            {
                var dataValida = DateTime.TryParse(txtInicioMandato.Text, out temp);
                if (dataValida)
                {
                    if (txtMandato.Text.Length > 4)
                    {
                        lblMensagem.Text = "O campo Quantidade de meses do Mandato conter no máximo 4 digitos.";
                        txtMandato.Text = string.Empty;
                    }
                    else if (!String.IsNullOrEmpty(txtInicioMandato.Text) && !String.IsNullOrEmpty(txtMandato.Text))
                    {
                        txtFinalDoMandato.Text = Convert.ToDateTime(txtInicioMandato.Text).AddMonths(Convert.ToInt32(txtMandato.Text.Replace("m", ""))).Date.AddDays(-1).ToShortDateString();
                    }
                }
                else
                {
                    lblMensagem.Text = "Data do início da vigência do mandato da AAE não é valida";
                }
            }
        }

        protected void txtMandato_TextChanged(object sender, EventArgs e)
        {
            DateTime temp;
            if (!txtInicioMandato.Text.IsNullOrEmptyOrWhiteSpace())
            {
                var dataValida = DateTime.TryParse(txtInicioMandato.Text, out temp);
                if (dataValida)
                {
                    if (txtMandato.Text.Length > 4)
                    {
                        lblMensagem.Text = "O campo Quantidade de meses do Mandato conter no máximo 4 digitos.";
                        txtMandato.Text = string.Empty;
                    }
                    else if (!String.IsNullOrEmpty(txtInicioMandato.Text) && !String.IsNullOrEmpty(txtMandato.Text))
                    {
                        txtFinalDoMandato.Text = Convert.ToDateTime(txtInicioMandato.Text).AddMonths(Convert.ToInt32(txtMandato.Text.Replace("m", ""))).Date.AddDays(-1).ToShortDateString();
                    }
                }
                else
                {
                    lblMensagem.Text = "Data do início da vigência do mandato da AAE não é valida";
                }
            }
        }

        protected void chkPossuiIdFunc_Clicked(object sender, EventArgs e)
        {
            try
            {
                chkNaoPossuiIdFunc.Checked = false;
                tseTesoureiro.ResetValue();
                txtRgTesoureiroAAE.Text = string.Empty;
                txtCpfTesoureiroAAE.Text = string.Empty;
                txtEnderecoTesoureiroAAE.Text = string.Empty;
                txtNumeroTesoureiroAAE.Text = string.Empty;
                txtComplementoTesoureiroAAE.Text = string.Empty;
                txtBairroTesoureiroAAE.Text = string.Empty;
                txtMunicipioTesoureiroAAE.Text = string.Empty;
                txtEmailTesoureiroAAE.Text = string.Empty;
                txtTelefoneTesoureiroAAE.Text = string.Empty;
                txtIdFuncionalTesoureiroAAE.Text = string.Empty;

                var dados = (DadosUnidadeAae)Session["DadosRetorno"];
                string table = string.Empty;
                string sqlWhere = string.Empty;
                Techne.Library.Sql.Structure.SqlSelectColumns coluna = new Techne.Library.Sql.Structure.SqlSelectColumns();
                Techne.Library.Sql.Structure.SqlSelect sqlSelect;

                table = " PrestacaoContas.VW_FUNCIONARIOS_TESOUREIRO ";
                sqlWhere = null;
                sqlWhere = " DATA_DESATIVACAO IS NULL and FUNCAO <> 14 and UNIDADE_ENS = " + Convert.ToString(dados.Censo);

                coluna.Add("pessoa");
                coluna.Add("nome_compl");
                coluna.Add("CPF");
                coluna.Add("IDFUNCIONAL");

                sqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);

                tseTesoureiro.SqlSelect = sqlSelect;
                tseTesoureiro.SqlWhere = sqlWhere;

                tseTesoureiro.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        protected void chkNaoPossuiIdFunc_Clicked(object sender, EventArgs e)
        {
            try
            {
                chkPossuiIdFunc.Checked = false;
                tseTesoureiro.ResetValue();
                txtRgTesoureiroAAE.Text = string.Empty;
                txtCpfTesoureiroAAE.Text = string.Empty;
                txtEnderecoTesoureiroAAE.Text = string.Empty;
                txtNumeroTesoureiroAAE.Text = string.Empty;
                txtComplementoTesoureiroAAE.Text = string.Empty;
                txtBairroTesoureiroAAE.Text = string.Empty;
                txtMunicipioTesoureiroAAE.Text = string.Empty;
                txtEmailTesoureiroAAE.Text = string.Empty;
                txtTelefoneTesoureiroAAE.Text = string.Empty;
                txtIdFuncionalTesoureiroAAE.Text = string.Empty;

                var dados = (DadosUnidadeAae)Session["DadosRetorno"];
                string table = string.Empty;
                string sqlWhere = string.Empty;
                Techne.Library.Sql.Structure.SqlSelectColumns coluna = new Techne.Library.Sql.Structure.SqlSelectColumns();
                Techne.Library.Sql.Structure.SqlSelect sqlSelect;

                table = " PrestacaoContas.VW_TESOUREIRO ";
                sqlWhere = null;

                coluna.Add("pessoa");
                coluna.Add("nome_compl");
                coluna.Add("CPF");
                coluna.Add("IDFUNCIONAL");

                sqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);

                tseTesoureiro.SqlSelect = sqlSelect;
                tseTesoureiro.SqlWhere = sqlWhere;

                tseTesoureiro.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        private void HabilitaDesabilitaCamposAbaInformacoesGerais(bool habilita)
        {

            chkPossuiIdFunc.Enabled = habilita;
            chkNaoPossuiIdFunc.Enabled = habilita;
            txtMandato.Enabled = habilita;
            FileUpload2.Enabled = habilita;
            txtInicioMandato.Enabled = habilita;
        }

        private void ControlarVisibilidadeControle(ImageButton[] imgBotoes, Button[] botoes)
        {
            RetiraVisibilidadeBotao();

            if (imgBotoes != null)
            {
                foreach (ImageButton botao in imgBotoes)
                {
                    botao.Visible = true;
                }
            }

            if (botoes != null)
            {
                foreach (Button botao in botoes)
                {
                    botao.Visible = true;
                }
            }

            ControlaAcesso(btnSalvar, Techne.Lyceum.Net.TPage.AcaoControle.editar);
            ControlaAcesso(btnEditar, Techne.Lyceum.Net.TPage.AcaoControle.editar);
        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnEditar.Visible = false;
            btnSalvar.Visible = false;
        }

        private void LimpaDadosInfoGerais()
        {

            txtEndCompl.Text = string.Empty;
            txtEndNum.Text = string.Empty;
            txtBairro.Text = string.Empty;
            txtTelefone.Text = string.Empty;
            hdnObrigacaoFiscalAaeId.Value = string.Empty;
            hdnDeclaracaoAaeId.Value = string.Empty;
            txtRegional.Text = string.Empty;
            txtEndereco.Text = string.Empty;
            txtMunicipio.Text = string.Empty;
            txtTelefone.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtDiretor.Text = string.Empty;
            txtCenso.Text = string.Empty;
            txtNumeroAlunos.Text = string.Empty;
        }

        private void LimpaDadosTesoureiro()
        {
            txtRgTesoureiroAAE.Text = string.Empty;
            txtCpfTesoureiroAAE.Text = string.Empty;
            txtEnderecoTesoureiroAAE.Text = string.Empty;

            txtNumeroTesoureiroAAE.Text = string.Empty;
            txtComplementoTesoureiroAAE.Text = string.Empty;
            txtBairroTesoureiroAAE.Text = string.Empty;
            txtMunicipioTesoureiroAAE.Text = string.Empty;

            txtEmailTesoureiroAAE.Text = string.Empty;
            txtTelefoneTesoureiroAAE.Text = string.Empty;
            txtIdFuncionalTesoureiroAAE.Text = string.Empty;
            txtFinalDoMandato.Text = string.Empty;
            txtMandato.Text = string.Empty;
            tseTesoureiro.ResetValue();
            lnkVisualizarRelatorio.Text = string.Empty;
            hdnMandatoId.Value = string.Empty;
        }

        private void LimpaDadosPresidente()
        {
            txtNomePresAAE.Text = string.Empty;

            txtRgPresAAE.Text = string.Empty;
            txtCpfPresAAE.Text = string.Empty;
            txtEnderecoPresAAE.Text = string.Empty;
            txtNumeroPresAAE.Text = string.Empty;
            txtComplementoPresAAE.Text = string.Empty;
            txtBairroPresAAE.Text = string.Empty;
            txtMunicipioPresidenteAAE.Text = string.Empty;
            txtEmailPresAAE.Text = string.Empty;
            txtTelefonePresAAE.Text = string.Empty;
            txtIdFuncionalPresAAE.Text = string.Empty;
            txtMatriculaPresAAE.Text = string.Empty;
            txtInicioMandato.Text = string.Empty;


        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Inicial;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnEditar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Alterar;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            RN.PrestacaoContas.MandatoAae rnMandatoAee = new Techne.Lyceum.RN.PrestacaoContas.MandatoAae();
            RN.PrestacaoContas.Entidades.MandatoAae mandatoAee = new Techne.Lyceum.RN.PrestacaoContas.Entidades.MandatoAae();
            RN.PrestacaoContas.Entidades.ArquivoAae arquivoAee = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ArquivoAae();

            var dadosInformacaoAdcionalAEE = new RN.PrestacaoContas.DTOs.DadosUnidadeAae();
            ValidacaoDados validacao = new ValidacaoDados();
            string mensagem = string.Empty;
            bool atualizaArquivo = true;

            try
            {
                if (!FileUpload2.PostedFile.FileName.IsNullOrEmptyOrWhiteSpace())
                {
                    byte[] imageBytes = new byte[FileUpload2.PostedFile.InputStream.Length + 1];
                    FileUpload2.PostedFile.InputStream.Read(imageBytes, 0, imageBytes.Length);
                    arquivoAee.Arquivo = imageBytes;

                    arquivoAee.NomeArquivo = FileUpload2.PostedFile.FileName;
                    arquivoAee.TipoArquivo = FileUpload2.PostedFile.ContentType;
                    arquivoAee.ChaveArquivo = Guid.NewGuid().ToString();
                    arquivoAee.UsuarioId = User.Identity.Name;
                }
                else
                {
                    if (lnkVisualizarRelatorio.Text != "Visualizar Ata" && lnkVisualizarRelatorio.Text != string.Empty)
                    {
                        atualizaArquivo = false;
                    }
                }

                mandatoAee.UsuarioId = User.Identity.Name;
                mandatoAee.DataInicioMandato = !String.IsNullOrEmpty(txtInicioMandato.Text) ? txtInicioMandato.Date : DateTime.MinValue;
                mandatoAee.Mandato = !String.IsNullOrEmpty(txtMandato.Text) ? Convert.ToInt32(txtMandato.Text) : -1;
                mandatoAee.DataFimMandato = !String.IsNullOrEmpty(txtFinalDoMandato.Text) ? Convert.ToDateTime(txtFinalDoMandato.Text).Date : DateTime.MinValue;
                mandatoAee.MandatoAaeId = !String.IsNullOrEmpty(hdnMandatoAae.Value) ? Convert.ToInt32(hdnMandatoAae.Value) : -1;
                mandatoAee.PessoaTesoureiro = (chkPossuiIdFunc.Checked && tseTesoureiro.IsValidDBValue && !tseTesoureiro.DBValue.IsNull) ? Convert.ToInt32(tseTesoureiro.DBValue) : (decimal?)null;
                mandatoAee.TesoureiroId = (chkNaoPossuiIdFunc.Checked && tseTesoureiro.IsValidDBValue && !tseTesoureiro.DBValue.IsNull) ? Convert.ToInt32(tseTesoureiro.DBValue) : (int?)null;


                var dados = (DadosUnidadeAae)Session["DadosRetorno"];
                if (dados != null)
                {
                    mandatoAee.Censo = dados.Censo;
                    arquivoAee.MandatoAaeId = dados.MandatoAaeId;
                }

                if (mandatoAee.Censo == null && !string.IsNullOrEmpty(tseUnidadeResponsavel.DBValue.ToString()))
                {
                    mandatoAee.Censo = tseUnidadeResponsavel.DBValue.ToString();
                }

                validacao = rnMandatoAee.Valida(mandatoAee, arquivoAee, atualizaArquivo);

                if (validacao.Valido)
                {
                    if (mandatoAee.MandatoAaeId == 0)
                    {
                        rnMandatoAee.Insere(mandatoAee, arquivoAee);
                        mensagem = "Mandato Aae inserido com sucesso.";
                        hdnMandatoAae.Value = mandatoAee.MandatoAaeId.ToString();
                    }
                    else
                    {
                        rnMandatoAee.Atualiza(mandatoAee, arquivoAee, atualizaArquivo);
                        mensagem = "Mandato Aae atualizado com sucesso.";
                    }

                    if (atualizaArquivo)
                    {
                        lnkVisualizarRelatorio.Text = FileUpload2.PostedFile.FileName;
                    }

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('" + mensagem + ".');", true);
                    lblMensagem.Text = mensagem;

                    _tipoOperacao = TipoOperacao.Consultar;
                    ControlarTipoOperacao();

                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void lnkVisualizarRelatorio_Click(object sender, EventArgs e)
        {
            try
            {
                RN.PrestacaoContas.ArquivoAae rnArquivoAae = new Techne.Lyceum.RN.PrestacaoContas.ArquivoAae();
                string embed = string.Empty;
                bimgArquivo.Visible = false;
                ltEmbed.Visible = false;
                DataTable dados = new DataTable();

                var dadosRetorno = (DadosUnidadeAae)Session["DadosRetorno"];

                string tipoArquivo = dadosRetorno.TipoArquivo;
                string arquivoAaeId = Convert.ToString(dadosRetorno.AtaMandatoArquivoId);

                if (arquivoAaeId.ToString().IsNullOrEmptyOrWhiteSpace())
                {
                    lblMensagem.Text = "Ainda não existe uma ata cadastrada.";
                }
                else
                {
                    if (!tipoArquivo.ToString().IsNullOrEmptyOrWhiteSpace())
                    {
                        pucVisualizarArquivo.ShowOnPageLoad = true;

                        if (tipoArquivo.ToString() == "application/pdf")
                        {
                            embed = " <object data=\"{0}{1}\"";
                            embed += "type=\"application/pdf\" width=\"100%\" height=\"100%\">";
                            embed += "<iframe   src=\"{0}{1}\"  width=\"100%\"   height=\"100%\"";
                            embed += "style=\"border: none;\">    <p>Your browser does not support PDFs.";
                            embed += "<a href=\"{0}{1}\">Download the PDF</a>.</p>";
                            embed += "</iframe></object>";

                            ltEmbed.Text = string.Format(embed, ResolveUrl("~/PrestacaoContas/FileCS.ashx?Tabela=ArquivoAae&Id="), arquivoAaeId);

                            ltEmbed.Visible = true;
                            pucVisualizarArquivo.Width = Unit.Pixel(880);
                            pucVisualizarArquivo.Height = Unit.Pixel(580);
                        }
                        else
                        {
                            pucVisualizarArquivo.Width = Unit.Pixel(350);
                            pucVisualizarArquivo.Height = Unit.Pixel(350);

                            //Obtem Arquivos
                            bimgArquivo.ContentBytes = rnArquivoAae.ObtemArquivoPor(Convert.ToInt32(arquivoAaeId));
                            bimgArquivo.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void pcFornecedor_TabClick(object source, TabControlCancelEventArgs e)
        {
            lblMensagem.Text = string.Empty;
        }

        protected void tseUnidadeResponsavel_Changed(object sender, EventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                if (!tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (tseUnidadeResponsavel.IsValidDBValue)
                    {
                        this._tipoOperacao = TipoOperacao.Consultar;

                        lblMensagem.Text = string.Empty;
                    }
                    else
                    {
                        this._tipoOperacao = TipoOperacao.Inicial;

                        lblMensagem.Text = "Unidade não ativa ou não cadastrada (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Unidade não ativa ou não cadastrada (favor verificar).";
                    _tipoOperacao = TipoOperacao.Inicial;
                }


                ControlarTipoOperacao();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ControlarTSearchs()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        tseUnidadeResponsavel.Enabled = true;
                        tseUnidadeResponsavel.Mode = ControlMode.Edit;
                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        tseUnidadeResponsavel.Enabled = true;
                        tseUnidadeResponsavel.Mode = ControlMode.Edit;
                        //tseMunicipio.Mode = ControlMode.View;
                        tseTesoureiro.Mode = ControlMode.View;
                        break;
                    }

                case TipoOperacao.Alterar:
                    {
                        tseUnidadeResponsavel.Enabled = false;
                        tseUnidadeResponsavel.Mode = ControlMode.View;
                        tseTesoureiro.Mode = ControlMode.Edit;

                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        tseTesoureiro.Mode = ControlMode.View;

                        break;
                    }
            }
        }

        protected void tseUnidadeResponsavel_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseTesoureiro_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseTesoureiro_Changed(object sender, EventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                if (!tseTesoureiro.DBValue.IsNull)
                {
                    if (tseTesoureiro.IsValidDBValue)
                    {
                        //this._tipoOperacao = TipoOperacao.Consultar;
                        //lblMensagem.Text = string.Empty;
                        RN.PrestacaoContas.MandatoAae rnMandatoAae = new Techne.Lyceum.RN.PrestacaoContas.MandatoAae();
                        RN.PrestacaoContas.DTOs.DadosTesoureiro dtoTesoureiro = new DadosTesoureiro();

                        if (chkNaoPossuiIdFunc.Checked)
                        {
                            dtoTesoureiro = rnMandatoAae.ObtemDadosTesoureiroPor(null, Convert.ToInt32(tseTesoureiro.DBValue));
                        }
                        else if (chkPossuiIdFunc.Checked)
                        {
                            dtoTesoureiro = rnMandatoAae.ObtemDadosTesoureiroPor(Convert.ToDecimal(tseTesoureiro.DBValue), null);

                        }
                        txtRgTesoureiroAAE.Text = dtoTesoureiro.Rg;
                        txtCpfTesoureiroAAE.Text = dtoTesoureiro.Cpf;
                        txtEnderecoTesoureiroAAE.Text = dtoTesoureiro.Endereco;
                        txtNumeroTesoureiroAAE.Text = dtoTesoureiro.Numero;
                        txtComplementoTesoureiroAAE.Text = dtoTesoureiro.Complemento;
                        txtBairroTesoureiroAAE.Text = dtoTesoureiro.Bairro;
                        txtMunicipioTesoureiroAAE.Text = dtoTesoureiro.Municipio;
                        txtEmailTesoureiroAAE.Text = dtoTesoureiro.Email;
                        txtTelefoneTesoureiroAAE.Text = dtoTesoureiro.Telefone;
                        txtIdFuncionalTesoureiroAAE.Text = dtoTesoureiro.IdFuncional;
                    }
                    else
                    {
                        this._tipoOperacao = TipoOperacao.Inicial;

                        lblMensagem.Text = "Grupo Produto/Serviço não ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Grupo Produto/Serviço não ativo ou não cadastrado (favor verificar).";
                    _tipoOperacao = TipoOperacao.Inicial;
                }


                ControlarTipoOperacao();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdObrigacoesFiscais_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (!this.grdObrigacoesFiscais.Visible || this.grdObrigacoesFiscais.VisibleRowCount == 0)
            {
                return;
            }

            var btnVisualizar = DevExpressHelper.GetControl<ImageButton>(this.grdObrigacoesFiscais, e.VisibleIndex, "btnVisualizarObrigacoes", "btnVisualizarObrigacoes");
            var btnDetalhes = DevExpressHelper.GetControl<ImageButton>(this.grdObrigacoesFiscais, e.VisibleIndex, "btnDetalhes", "btnDetalhes");

            string enviado = (string)grdObrigacoesFiscais.GetRowValues(e.VisibleIndex, "ENVIADO");
            btnVisualizar.Visible = false;
            btnDetalhes.Visible = false;

            if (enviado == "Sim")
            {
                btnVisualizar.Visible = true;
            }

            //Verifica se tem permissão
            if (Permission.AllowUpdate)
            {
                btnDetalhes.Visible = true;
            }
        }

        protected void grdMandatosAae_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (!this.grdMandatosAae.Visible || this.grdMandatosAae.VisibleRowCount == 0)
            {
                return;
            }

            var btnVisualizar = DevExpressHelper.GetControl<ImageButton>(this.grdMandatosAae, e.VisibleIndex, "btnVisualizarMandato", "btnVisualizarMandato");

            string enviado = Convert.ToString(grdMandatosAae.GetRowValues(e.VisibleIndex, "NOMEARQUIVO"));
            btnVisualizar.Visible = false;

            if (!enviado.IsNullOrEmptyOrWhiteSpace())
            {
                btnVisualizar.Visible = true;
            }
        }

        public void Delete(object MANDATOAAEID) { }


        protected void grdMandatosAae_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            try
            {
                if (e.ButtonID == "btnExcluir")
                {
                    hdnMandatoId.Value = Convert.ToString(grdMandatosAae.GetRowValues(e.VisibleIndex, "MANDATOAAEID"));
                    
                    popup.ShowOnPageLoad = true;                   
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;

            }
        
        }

        protected void btnExcluir_Click(object sender, EventArgs e)
        {

            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.PrestacaoContas.MandatoAae rnMandatoAae = new RN.PrestacaoContas.MandatoAae();
                int mandatoAaeId = Convert.ToInt32(hdnMandatoId.Value);
      
                validacao = rnMandatoAae.ValidaRemocao(mandatoAaeId);

                if (validacao.Valido)
                {
                    rnMandatoAae.Remove(mandatoAaeId);
                    grdMandatosAae.DataBind();

                    LimpaDadosTesoureiro();
                    _tipoOperacao = TipoOperacao.Consultar;
                    ControlarTipoOperacao();
                    pcFornecedor.ActiveTabIndex = 1;
                    lblMensagem.Text = "Mandato excluído com sucesso.";
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }               
                
              
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        
        protected void btnImportar_Click(object sender, EventArgs e)
        {
            try
            {
                RN.PrestacaoContas.ObrigacaoFiscalAae rnObrigacaoFiscalAae = new Techne.Lyceum.RN.PrestacaoContas.ObrigacaoFiscalAae();
                RN.PrestacaoContas.Entidades.ObrigacaoFiscalAae obrigacaoFiscalAae = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ObrigacaoFiscalAae();
                RN.PrestacaoContas.Entidades.DeclaracaoFiscalArquivo declaracaoFiscalArquivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.DeclaracaoFiscalArquivo();
                ValidacaoDados validacao = new ValidacaoDados();
                string mensagem = string.Empty;
                Statuslbl.Text = string.Empty;

                byte[] imageBytes = new byte[FileUpload1.PostedFile.InputStream.Length + 1];
                FileUpload1.PostedFile.InputStream.Read(imageBytes, 0, imageBytes.Length);

                obrigacaoFiscalAae.UsuarioId = User.Identity.Name;
                obrigacaoFiscalAae.DeclaracaoAaeId = !hdnDeclaracaoAaeId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnDeclaracaoAaeId.Value) : -1;
                obrigacaoFiscalAae.ObrigacaoFiscalAaeId = !hdnObrigacaoFiscalAaeId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnObrigacaoFiscalAaeId.Value) : -1;
                obrigacaoFiscalAae.AnoBase = !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlAno.SelectedValue) : -1;
                obrigacaoFiscalAae.Mes = !ddlMes.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlMes.SelectedValue) : -1;
                obrigacaoFiscalAae.Censo = !this.tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue ? tseUnidadeResponsavel.DBValue.ToString() : null;

                declaracaoFiscalArquivo.Arquivo = imageBytes;
                declaracaoFiscalArquivo.ObrigacaoFiscalAaeId = !hdnObrigacaoFiscalAaeId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnObrigacaoFiscalAaeId.Value) : -1;
                declaracaoFiscalArquivo.NomeArquivo = FileUpload1.PostedFile.FileName;
                declaracaoFiscalArquivo.TipoArquivo = FileUpload1.PostedFile.ContentType;

                bool cadastro = declaracaoFiscalArquivo.ObrigacaoFiscalAaeId == -1;

                validacao = rnObrigacaoFiscalAae.Valida(obrigacaoFiscalAae, declaracaoFiscalArquivo, cadastro);

                if (validacao.Valido)
                {
                    if (cadastro)
                    {
                        rnObrigacaoFiscalAae.Insere(obrigacaoFiscalAae, declaracaoFiscalArquivo);
                        mensagem = "Arquivo importado com sucesso.";
                    }
                    else
                    {
                        rnObrigacaoFiscalAae.Atualiza(obrigacaoFiscalAae, declaracaoFiscalArquivo);
                        mensagem = "Arquivo atualizado com sucesso.";
                    }

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('" + mensagem + ".');", true);
                    Statuslbl.Text = mensagem;
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    Statuslbl.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }

                grdObrigacoesFiscais.DataBind();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnDetalhes_Command(object sender, CommandEventArgs e)
        {
            try
            {
                hdnDeclaracaoAaeId.Value = string.Empty;
                hdnObrigacaoFiscalAaeId.Value = string.Empty;
                Statuslbl.Text = string.Empty;

                string[] chave = e.CommandArgument.ToString().Split(new char[] { ',' });

                hdnDeclaracaoAaeId.Value = chave[1].ToString();
                hdnObrigacaoFiscalAaeId.Value = chave[0].ToString();

                //Verifica se Já possui cadastro
                if (!hdnObrigacaoFiscalAaeId.Value.IsNullOrEmptyOrWhiteSpace())
                {
                    //Busca dados salvos
                    int declaracaoAaeId = Convert.ToInt32(hdnDeclaracaoAaeId.Value);
                    string anoBase = grdObrigacoesFiscais.GetRowValuesByKeyValue(declaracaoAaeId, "ANOBASE").ToString();
                    string mes = grdObrigacoesFiscais.GetRowValuesByKeyValue(declaracaoAaeId, "MES").ToString();

                    ddlAno.SelectedValue = anoBase;
                    ddlMes.SelectedValue = Convert.ToString(Utils.ObtemCodigoMesPor(mes));
                }

                pucConfirmarArquivo.ShowOnPageLoad = true;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnVisualizarMandato_Command(object sender, CommandEventArgs e)
        {
            try
            {
                string embed = string.Empty;
                bimgArquivo.Visible = false;
                ltEmbed.Visible = false;
                DataTable dados = new DataTable();
                RN.PrestacaoContas.DeclaracaoFiscalArquivo rnDeclaracaoFiscalArquivo = new Techne.Lyceum.RN.PrestacaoContas.DeclaracaoFiscalArquivo();
                RN.PrestacaoContas.ArquivoAae rnArquivoAae = new Techne.Lyceum.RN.PrestacaoContas.ArquivoAae();

                string[] chave = e.CommandArgument.ToString().Split(new char[] { ',' });

                if (!chave[1].ToString().IsNullOrEmptyOrWhiteSpace())
                {
                    pucVisualizarArquivo.ShowOnPageLoad = true;

                    if (chave[1].ToString() == "application/pdf")
                    {
                        embed = " <object data=\"{0}{1}\"";
                        embed += "type=\"application/pdf\" width=\"100%\" height=\"100%\">";
                        embed += "<iframe   src=\"{0}{1}\"  width=\"100%\"   height=\"100%\"";
                        embed += "style=\"border: none;\">    <p>Your browser does not support PDFs.";
                        embed += "<a href=\"{0}{1}\">Download the PDF</a>.</p>";
                        embed += "</iframe></object>";

                        ltEmbed.Text = string.Format(embed, ResolveUrl("~/PrestacaoContas/FileCS.ashx?Tabela=ArquivoAae&Id="), chave[0].ToString());

                        ltEmbed.Visible = true;
                        pucVisualizarArquivo.Width = Unit.Pixel(880);
                        pucVisualizarArquivo.Height = Unit.Pixel(580);
                    }
                    else
                    {
                        pucVisualizarArquivo.Width = Unit.Pixel(350);
                        pucVisualizarArquivo.Height = Unit.Pixel(350);
                        bimgArquivo.ContentBytes = rnArquivoAae.ObtemArquivoPor(Convert.ToInt32(chave[0]));
                        bimgArquivo.Visible = true;
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Não existe documento para visualização');", true);
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnVisualizar_Command(object sender, CommandEventArgs e)
        {
            try
            {
                string embed = string.Empty;
                bimgArquivo.Visible = false;
                ltEmbed.Visible = false;
                DataTable dados = new DataTable();
                RN.PrestacaoContas.DeclaracaoFiscalArquivo rnDeclaracaoFiscalArquivo = new Techne.Lyceum.RN.PrestacaoContas.DeclaracaoFiscalArquivo();
                RN.PrestacaoContas.ArquivoAae rnArquivoAae = new Techne.Lyceum.RN.PrestacaoContas.ArquivoAae();

                string[] chave = e.CommandArgument.ToString().Split(new char[] { ',' });

                if (!chave[1].ToString().IsNullOrEmptyOrWhiteSpace())
                {
                    pucVisualizarArquivo.ShowOnPageLoad = true;

                    if (chave[1].ToString() == "application/pdf")
                    {
                        embed = " <object data=\"{0}{1}\"";
                        embed += "type=\"application/pdf\" width=\"100%\" height=\"100%\">";
                        embed += "<iframe   src=\"{0}{1}\"  width=\"100%\"   height=\"100%\"";
                        embed += "style=\"border: none;\">    <p>Your browser does not support PDFs.";
                        embed += "<a href=\"{0}{1}\">Download the PDF</a>.</p>";
                        embed += "</iframe></object>";

                        ltEmbed.Text = string.Format(embed, ResolveUrl("~/PrestacaoContas/FileCS.ashx?Tabela=DeclaracaoFiscalArquivo&Id="), chave[0].ToString());

                        ltEmbed.Visible = true;
                        pucVisualizarArquivo.Width = Unit.Pixel(880);
                        pucVisualizarArquivo.Height = Unit.Pixel(580);
                    }
                    else
                    {
                        pucVisualizarArquivo.Width = Unit.Pixel(350);
                        pucVisualizarArquivo.Height = Unit.Pixel(350);
                        bimgArquivo.ContentBytes = rnDeclaracaoFiscalArquivo.ObtemArquivoPor(Convert.ToInt32(chave[0]));
                        bimgArquivo.Visible = true;
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Não existe documento para visualização');", true);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
