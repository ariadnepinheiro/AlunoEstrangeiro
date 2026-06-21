using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using Techne.Controls;
using System.Collections.Generic;
using System.Linq;

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/Voluntarios.aspx")]
    [ControlText("Voluntários/Estagiários")]
    [Title("Voluntários/Estagiários")]
    public partial class Voluntarios : TPage
    {
        #region Propriedades e Enumeradores

        public enum TipoOperacao
        {
            Novo,
            Alterar,
            ConsultarDocente,
            ConsultarPessoa,
            Inicial,
            Sucesso
        }

        private TipoOperacao _tipoOperacao
        {
            get { return (TipoOperacao)ViewState["_tipoOperacao"]; }
            set { ViewState["_tipoOperacao"] = value; }
        }

        #endregion

        public static string GetUrl()
        {
            return Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
        }

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdVinculos, "Vínculos");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    _tipoOperacao = TipoOperacao.Inicial;
                    ControlarTipoOperacao();
                }

                //tamanho maximo das datas
                DateTime dtAtual = DateTime.Now;

                dteRGDataExpPessoa.MaxDate = dtAtual.Date;
                dteDtNasc.MaxDate = dtAtual.Date.AddDays(-1);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            ControlarEnderecoPais();
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(btnSalvarVinculo, AcaoControle.novo);
        }

        #endregion

        #region Buttons Events

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                VinculoTce rnVinculo = new VinculoTce();
                RN.Docentes rnDocentes = new RN.Docentes();
                ValidacaoDados validacao = new ValidacaoDados();

                if ((_tipoOperacao == TipoOperacao.Novo) && txtCPFBusca.Text.IsNullOrEmptyOrWhiteSpace())
                {
                    lblMensagem.Text = "Para salvar um novo voluntário/estagiário é necessário informar o campo CPF.";
                    return;
                }

                var pessoa = new LyPessoa
                {
                    Pessoa = !string.IsNullOrEmpty(txtPessoa.Text) ? Convert.ToDecimal(txtPessoa.Text) : 0m,
                    Nome_compl = !txtNomeComplPessoa.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeComplPessoa.Text.Trim().ToUpper() : null,
                    Nome_social = !txtNomeSocial.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeSocial.Text.Trim().ToUpper() : null,
                    Endereco = !txtEnderecoPessoa.Text.IsNullOrEmptyOrWhiteSpace() ? txtEnderecoPessoa.Text : null,
                    End_num = !txtEndNumPessoa.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndNumPessoa.Text : null,
                    End_municipio = (!this.tseMunicipio.DBValue.IsNull && this.tseMunicipio.IsValidDBValue) ? tseMunicipio.DBValue.ToString() : null,
                    Cep = !txtCEP.Text.IsNullOrEmptyOrWhiteSpace() ? txtCEP.Text.RetirarCaracteres() : null,
                    Rg_emissor = !ddlRGEmissorPessoa.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlRGEmissorPessoa.SelectedValue : null,
                    Rg_tipo = !ddlRGTipoPessoa.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlRGTipoPessoa.SelectedValue : null,
                    Municipio_nasc = (!this.tseNaturalidade.DBValue.IsNull && this.tseNaturalidade.IsValidDBValue) ? tseNaturalidade.DBValue.ToString() : null,
                    Pais_nasc = !ddlPaisNasc.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlPaisNasc.SelectedValue : null,
                    Nacionalidade = !ddlNacionalidade.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlNacionalidade.SelectedValue : null,
                    Etnia = !ddlRaca.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlRaca.SelectedValue : null,
                    Rg_uf = !ddlRGUFPessoa.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlRGUFPessoa.SelectedValue : null,
                    NecessidadeEspecialId = !ddlNecessidadeEspecial.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlNecessidadeEspecial.SelectedValue) : (int?)null,
                    Est_civil = !ddlEst_Civil.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlEst_Civil.SelectedValue : null,
                    Sexo = !rblSexo.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToString(rblSexo.Text) : null,
                    End_compl = !txtEndCompl.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndCompl.Text : null,
                    Bairro = !txtBairro.Text.IsNullOrEmptyOrWhiteSpace() ? txtBairro.Text : null,
                    Fone = !txtFone.Text.IsNullOrEmptyOrWhiteSpace() ? txtFone.Text : null,
                    Celular = !txtCelular.Text.IsNullOrEmptyOrWhiteSpace() ? txtCelular.Text : null,
                    E_mail = !string.IsNullOrEmpty(txtEmail.Text.Trim()) ? txtEmail.Text.Trim() : null,
                    Rg_num = !txtRGNumPessoa.Text.IsNullOrEmptyOrWhiteSpace() ? txtRGNumPessoa.Text : null,
                    Cpf = !txtCPF.Text.IsNullOrEmptyOrWhiteSpace() ? txtCPF.Text.RetirarMascaraCPF().Trim() : null,
                    Dt_nasc = !string.IsNullOrEmpty(dteDtNasc.Text) ? dteDtNasc.Date : (DateTime?)null,
                    Rg_dtexp = !string.IsNullOrEmpty(dteRGDataExpPessoa.Text) ? dteRGDataExpPessoa.Date : (DateTime?)null,
                    Pispasep = !string.IsNullOrEmpty(txtPISPASEP.Text) ? txtPISPASEP.Text :null,
                    UsuarioId = User.Identity.Name
                };

                var docente = new LyDocente
                {
                    Pessoa = pessoa.Pessoa,
                    Voluntario = "S",
                    Categoria =  !rblFuncao.SelectedValue.IsNullOrEmptyOrWhiteSpace() ?  rblFuncao.SelectedValue : null,
                    Acumulacao = (int)RN.Docentes.PossuiAcumulacao.NaoInformado,
                    Vinculo = null,
                    Matricula = !tseVoluntarios.DBValue.IsNull ? tseVoluntarios.DBValue.ToString() : null
                };

                string zonaResidencial = rblLocalizacaoUF.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? null : rblLocalizacaoUF.SelectedValue;

                long resultado;
                if (long.TryParse(txtCelular.Text.Trim().RetirarMascaraTelefone(), out resultado))
                {
                    if (txtCelular.Text.Trim().RetirarMascaraTelefone().Length == 10)
                    {
                        pessoa.Celular = string.Format("{0:(00)0000-0000}", resultado);
                    }
                    if (txtCelular.Text.Trim().RetirarMascaraTelefone().Length == 11)
                    {
                        pessoa.Celular = string.Format("{0:(00)00000-0000}", resultado);
                    }
                    txtCelular.Text = pessoa.Celular;
                }
                else
                {
                    pessoa.Celular = null;
                }

                validacao = rnDocentes.ValidaVoluntario(pessoa, docente, zonaResidencial, txtVoluntario.Text.IsNullOrEmptyOrWhiteSpace());

                if (validacao.Valido)
                {
                    if (txtVoluntario.Text.IsNullOrEmptyOrWhiteSpace())
                    {
                        rnDocentes.InsereVoluntario(pessoa, docente, zonaResidencial);

                        txtVoluntario.Text = docente.Matricula;
                        txtPessoa.Text = pessoa.Pessoa.ToString();
                        tseVoluntarios.DBValue = docente.Matricula;
                        tseUnidadeEnsino.Enabled = true;
                        tseUnidadeEnsino.Mode = ControlMode.Edit;
                        dtVincInicio.Enabled = true;
                        dtVincInicio.Text = String.Empty;
                        dtVincFim.Enabled = true;
                        dtVincFim.Text = String.Empty;
                        grdVinculos.Enabled = true;
                        chkPrincipal.Enabled = true;
                        chkPrincipal.Checked = false;
                        apcVoluntario.TabPages.FindByName("Vinculos").Enabled = true;
                        apcVoluntario.ActiveTabIndex = 2;

                    }
                    else
                    {
                        rnDocentes.AtualizaVoluntario(docente, pessoa, zonaResidencial);
                    }
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }

                _tipoOperacao = TipoOperacao.Sucesso;
                ControlarTipoOperacao();
                ControlarTSearchs();
                lblMensagem.Text = "Operação realizada com sucesso.";

                this.odsVinculos.Select();
                this.odsVinculos.DataBind();
                this.grdVinculos.DataBind();

                string matriculaDocente = string.IsNullOrEmpty(tseVoluntarios["matricula"].ToString()) ? txtVoluntario.Text : tseVoluntarios["matricula"].ToString();
                if (!rnVinculo.VerificaVinculoPrincipal(matriculaDocente))
                {
                    chkPrincipal.Enabled = false;
                    chkPrincipal.Checked = true;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Preencha os dados de vínculo. É indispensável um vinculo principal Ativo.');", true);
                }
                else
                {
                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();
                    ControlarTSearchs();
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Novo;
            ControlarTipoOperacao();
            ControlarTSearchs();
            btnSalvar.Visible = false;
        }

        protected void btnEditar_Click(object sender, ImageClickEventArgs e)
        {
            _tipoOperacao = TipoOperacao.Alterar;
            ControlarTipoOperacao();
            ControlarTSearchs();
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            tseVoluntarios.ResetValue();
            _tipoOperacao = TipoOperacao.Inicial;
            ControlarTipoOperacao();
            ControlarTSearchs();
        }

        #endregion

        #region TseVoluntarios

        protected void tseVoluntarios_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        protected void tseVoluntarios_Changed(object sender, EventArgs args)
        {
            try
            {
                if (!string.IsNullOrEmpty(tseVoluntarios.DBValue.ToString()))
                {
                    if (tseVoluntarios.IsValidDBValue)
                    {
                        LimparTela();
                        _tipoOperacao = TipoOperacao.ConsultarDocente;
                        ControlarTipoOperacao();
                        ControlarTSearchs();
                    }
                    else
                    {
                        lblMensagem.Text = "Voluntário não cadastrado ou usuário sem permissão de visualização do Voluntário(Lotação)..";
                        _tipoOperacao = TipoOperacao.Inicial;
                        ControlarTipoOperacao();
                        ControlarTSearchs();
                    }
                }
                else
                {
                    lblMensagem.Text = "Voluntário não cadastrado ou usuário sem permissão de visualização do Voluntário(Lotação)..";
                    _tipoOperacao = TipoOperacao.Inicial;
                    ControlarTipoOperacao();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        #endregion

        #region TAB - DadosPessoais

        protected void ddlPaisNasc_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimparEnderecoNascimento();
        }

        protected void tseNaturalidade_Changed(object sender, EventArgs args)
        {
            if (tseNaturalidade.IsValidDBValue
                && !tseNaturalidade.DBValue.IsNull)
            {
                txtEstadoNaturalidade.Value = tseNaturalidade["uf_sigla"].ToString();
            }
        }

        protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimparEndereco();
        }

        protected void tseMunicipio_Changed(object sender, EventArgs args)
        {
            if (tseMunicipio.IsValidDBValue
                && !tseMunicipio.DBValue.IsNull)
            {
                txtEstado.Value = Convert.ToString(tseMunicipio["uf_sigla"]);
            }
        }

        #endregion

        #region TAB - Documentos

        protected void ddlRGTipoPessoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRGTipoPessoa.SelectedValue == "RG")
            {
                lblRGUFPessoa.Text = "Estado*: ";
                lblRGUFPessoa.Font.Bold = true;
                lblRGDataExpPessoa.Text = "Data de Expedição*: ";
                lblRGDataExpPessoa.Font.Bold = true;
            }
            else
            {
                lblRGUFPessoa.Text = "Estado: ";
                lblRGUFPessoa.Font.Bold = false;
                lblRGDataExpPessoa.Text = "Data de Expedição: ";
                lblRGDataExpPessoa.Font.Bold = false;
            }
        }

        #endregion

        #region Controle Visibilidade

        private void ControlarTipoOperacao()
        {
            RN.Docentes rnDocentes = new Techne.Lyceum.RN.Docentes();
            
            try
            {
                apcVoluntario.ActiveTabIndex = 0;

                switch (_tipoOperacao)
                {
                    case TipoOperacao.Inicial:
                        {
                            ImageButton[] controles = new ImageButton[] { btnNovo };
                            ControlarVisibilidadeControle(controles);
                            lbltxtPessoa.Visible = false;
                            txtPessoa.Visible = false;
                            divDados.Visible = false;
                            txtCPFBusca.Text = string.Empty;
                            pnlBuscaCPF.Visible = false;
                            lblmensagemBloqueio.Visible = false;
                            break;
                        }
                    case TipoOperacao.Sucesso:
                        {
                            ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar };
                            ControlarVisibilidadeControle(controles);
                            apcVoluntario.Visible = true;
                            DesabilitaCampos();
                            txtCPFBusca.Text = string.Empty;
                            pnlBuscaCPF.Visible = false;
                            lbltxtPessoa.Visible = false;
                            txtPessoa.Visible = false;
                            btnSalvarVinculo.Visible = false;
                            break;
                        }
                    case TipoOperacao.Novo:
                        {
                            LimparTela();
                            txtCPFBusca.Text = string.Empty;
                            txtCPFBusca.Enabled = true;
                            pnlBuscaCPF.Visible = true;
                            divDados.Visible = false;                           

                            tseVoluntarios.ResetValue();
                            tseVoluntarios.Enabled = false;

                            var controles = new[] { btnCancel, btnSalvar };
                            ControlarVisibilidadeControle(controles);

                            var aba = apcVoluntario.TabPages.FindByName("Vinculos");
                            if (aba != null)
                                aba.Enabled = false;

                            break;
                        }
                    case TipoOperacao.Alterar:
                        {
                            RN.Aluno rnAluno = new Aluno();
                            txtPessoa.Visible = false;
                            lbltxtPessoa.Visible = false;

                            //Verifica se a Pessoa é um aluno ativo
                            if (!rnAluno.EhAlunoAtivo(Convert.ToDecimal(txtPessoa.Text)))
                            {
                                HabilitaCampos();
                            }
                            else
                            {
                                lblmensagemBloqueio.Visible = true;
                                DesabilitaCampos();
                                txtPISPASEP.Enabled = true;
                                rblLocalizacaoUF.Enabled = true;
                                tseUnidadeEnsino.Enabled = true;
                                tseUnidadeEnsino.Mode = ControlMode.Edit;
                                dtVincInicio.Enabled = true;
                                dtVincInicio.Text = String.Empty;
                                dtVincFim.Enabled = true;
                                dtVincFim.Text = String.Empty;
                                chkPrincipal.Enabled = true;
                                grdVinculos.Enabled = true;
                            }

                            rblFuncao.Enabled = false;

                            var controles = new ImageButton[] { btnCancel, btnSalvar };
                            ControlarVisibilidadeControle(controles);
                            btnSalvarVinculo.Visible = true;

                            break;
                        }
                    case TipoOperacao.ConsultarDocente:
                        {
                            lbltxtPessoa.Visible = false;
                            txtPessoa.Visible = false;
                            txtCPFBusca.Text = string.Empty;

                           var matricula = tseVoluntarios["matricula"].ToString();
                           var pessoa = Convert.ToDecimal(tseVoluntarios["pessoa"]);

                           if (pessoa > 0)
                            {

                                if (!string.IsNullOrEmpty(tseVoluntarios["categoria"].ToString()))
                                {
                                    if (rblFuncao.Items.FindByValue(tseVoluntarios["categoria"].ToString()) != null)
                                    {
                                        rblFuncao.SelectedValue = tseVoluntarios["categoria"].ToString();
                                    }
                                }

                                txtPessoa.Text = Convert.ToString(pessoa);
                                txtVoluntario.Text = Convert.ToString(matricula);

                                var aba = apcVoluntario.TabPages.FindByName("Vinculos");
                                if (aba != null)
                                    aba.Enabled = true;

                                CarregaCombo();

                                ImageButton[] controles = new ImageButton[] { btnNovo, btnEditar };
                                ControlarVisibilidadeControle(controles);
                                btnSalvarVinculo.Visible = false;
                                apcVoluntario.Visible = true;
                                LyPessoa dadosPessoa = new LyPessoa();
                                dadosPessoa = RN.Pessoa.Carregar(Convert.ToInt32(pessoa));
                                PreencherDadosTelaPessoa(dadosPessoa);

                                DesabilitaCampos();
                                divDados.Visible = true;
                                pnlBuscaCPF.Visible = false;
                            }
                            else
                            {
                                LimparTela();

                                ImageButton[] controles = new ImageButton[] { btnNovo };
                                ControlarVisibilidadeControle(controles);
                                lblMensagem.Text = "Pessoa não cadastrada para este docente.";
                            }
                            break;
                        }
                }
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
                case TipoOperacao.Sucesso:
                    {
                        tseVoluntarios.Enabled = true;
                        break;
                    }

                case TipoOperacao.Novo:
                    {
                        tseVoluntarios.Enabled = false;
                        break;
                    }
                case TipoOperacao.Alterar:
                    {
                        tseVoluntarios.Enabled = false;
                        break;
                    }
                case TipoOperacao.ConsultarDocente:
                    {
                        tseVoluntarios.Enabled = true;
                        break;
                    }
                case TipoOperacao.ConsultarPessoa:
                    {
                        tseVoluntarios.Enabled = false;
                        break;
                    }
            }
        }

        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (ImageButton botao in botoes)
            {
                botao.Visible = true;
            }
            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(btnSalvarVinculo, AcaoControle.novo);
        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnEditar.Visible = false;
            btnNovo.Visible = false;
            btnSalvar.Visible = false;
        }

        protected void LimparTela()
        {
            txtVoluntario.Text = string.Empty;
            tseNaturalidade.ResetValue();
            txtEstadoNaturalidade.Value = string.Empty;
            txtPessoa.Text = string.Empty;
            txtIDINEP.Text = string.Empty;
            txtNomeComplPessoa.Text = string.Empty;
            txtNomeSocial.Text = string.Empty;
            dteDtNasc.Text = string.Empty;
            rblSexo.SelectedIndex = -1;
            rblLocalizacaoUF.SelectedIndex = -1;
            ddlRaca.Items.Clear();
            ddlNecessidadeEspecial.Items.Clear();
            ddlPaisNasc.Items.Clear();
            ddlNacionalidade.Items.Clear();
            txtEnderecoPessoa.Text = string.Empty;
            txtEndNumPessoa.Text = string.Empty;
            txtEndCompl.Text = string.Empty;
            txtMunicipio.Text = string.Empty;
            txtBairro.Text = string.Empty;
            txtCEP.Text = string.Empty;
            txtFone.Text = string.Empty;
            txtCelular.Text = string.Empty;
            txtEmail.Text = string.Empty;
            ddlRGTipoPessoa.Items.Clear();
            txtRGNumPessoa.Text = string.Empty;
            ddlRGUFPessoa.Items.Clear();
            dteRGDataExpPessoa.Text = string.Empty;
            ddlRGEmissorPessoa.Items.Clear();
            txtCPF.Text = string.Empty;
            txtPISPASEP.Text = string.Empty;
            this.LimparCamposVinculo();
            this.odsVinculos.Select();
            this.odsVinculos.DataBind();
            this.grdVinculos.DataBind();
            rblFuncao.ClearSelection();
        }

        protected void HabilitaCampos()
        {
            VinculoTce rnVinculo = new VinculoTce();
            string matriculaDocente;

            txtMunicipioNaturalidade.Enabled = true;
            txtEstadoNaturalidade.Attributes.Add("readonly", "readonly");
            txtMunicipio.Enabled = true;
            txtEstado.Attributes.Add("readonly", "readonly");
            txtNomeComplPessoa.Enabled = true;
            txtNomeSocial.Enabled = true;
            dteDtNasc.Enabled = true;
            rblSexo.Enabled = true;
            rblFuncao.Enabled = true;
            rblLocalizacaoUF.Enabled = true;
            ddlRaca.Enabled = true;
            ddlNecessidadeEspecial.Enabled = true;
            ddlEst_Civil.Enabled = true;
            ddlPaisNasc.Enabled = true;
            ddlNacionalidade.Enabled = true;
            txtFone.Enabled = true;
            txtCelular.Enabled = true;
            txtEmail.Enabled = true;
            ddlRGTipoPessoa.Enabled = true;
            txtRGNumPessoa.Enabled = true;
            ddlRGUFPessoa.Enabled = true;
            dteRGDataExpPessoa.Enabled = true;
            ddlRGEmissorPessoa.Enabled = true;
            txtCPF.Enabled = true;
            txtPISPASEP.Enabled = true;
            txtCEP.Enabled = true;
            tsCEP.ShowButton = true;
            txtEndCompl.Enabled = true;
            txtEndNumPessoa.Enabled = true;
            txtEnderecoPessoa.Enabled = true;
            txtBairro.Enabled = true;
            tseUnidadeEnsino.Enabled = true;
            tseUnidadeEnsino.Mode = ControlMode.Edit;
            dtVincInicio.Enabled = true;
            dtVincFim.Enabled = true;
            grdVinculos.Enabled = true;

            //Verifica se não existe outro vinculo principal cadastrado
            matriculaDocente = string.IsNullOrEmpty(tseVoluntarios["matricula"].ToString()) ? txtVoluntario.Text : tseVoluntarios["matricula"].ToString();
            if (string.IsNullOrEmpty(matriculaDocente) || !rnVinculo.VerificaVinculoPrincipal(matriculaDocente))
            {
                //Se nao existir, força o primeiro vinculo a ser principal
                chkPrincipal.Enabled = false;
                chkPrincipal.Checked = true;
            }
            else
            {
                chkPrincipal.Enabled = true;
            }
        }

        protected void DesabilitaCampos()
        {
            tseMunicipio.Mode = ControlMode.View;
            tseNaturalidade.Mode = ControlMode.View;
            txtMunicipioNaturalidade.Enabled = false;
            txtEstadoNaturalidade.Attributes.Add("readonly", "readonly");
            txtMunicipio.Enabled = false;
            txtEstado.Attributes.Add("readonly", "readonly");
            txtNomeComplPessoa.Enabled = false;
            txtNomeSocial.Enabled = false;
            dteDtNasc.Enabled = false;
            rblSexo.Enabled = false;
            rblFuncao.Enabled = false;
            rblLocalizacaoUF.Enabled = false;
            ddlRaca.Enabled = false;
            ddlNecessidadeEspecial.Enabled = false;
            ddlEst_Civil.Enabled = false;
            ddlPaisNasc.Enabled = false;
            ddlNacionalidade.Enabled = false;
            txtFone.Enabled = false;
            txtCelular.Enabled = false;
            txtEmail.Enabled = false;
            ddlRGTipoPessoa.Enabled = false;
            txtRGNumPessoa.Enabled = false;
            ddlRGUFPessoa.Enabled = false;
            dteRGDataExpPessoa.Enabled = false;
            ddlRGEmissorPessoa.Enabled = false;
            txtCPF.Enabled = false;
            txtPISPASEP.Enabled = false;
            txtCEP.Enabled = false;
            tsCEP.ShowButton = false;
            txtEndCompl.Enabled = false;
            txtEndNumPessoa.Enabled = false;
            txtEnderecoPessoa.Enabled = false;
            txtBairro.Enabled = false;
            tseUnidadeEnsino.Enabled = false;
            tseUnidadeEnsino.Mode = ControlMode.View;
            dtVincInicio.Enabled = false;
            dtVincFim.Enabled = false;
            chkPrincipal.Enabled = false;
            grdVinculos.Enabled = false;
        }

        protected void CarregaCombo()
        {
            CarregarDadosDrop(ddlPaisNasc.ID);
            CarregarDadosDrop(ddlNacionalidade.ID);
            CarregarDadosDrop(ddlEst_Civil.ID);
            CarregarDadosDrop(ddlRGEmissorPessoa.ID);
            CarregarDadosDrop(ddlRGUFPessoa.ID);
            CarregarDadosDrop(ddlRGTipoPessoa.ID);
            CarregarDadosDrop(ddlRaca.ID);
            CarregaNecessidadeEspecial();
        }

        private void CarregaNecessidadeEspecial()
        {
            RN.NecessidadeEspecial.NecessidadeEspecial rnNecessidadeEspecial = new RN.NecessidadeEspecial.NecessidadeEspecial();
            ListItem itemVazio = new ListItem("Selecione", string.Empty);

            ddlNecessidadeEspecial.Items.Clear();
            ddlNecessidadeEspecial.DataSource = rnNecessidadeEspecial.ListaNecessidadeEspecialAtiva();
            ddlNecessidadeEspecial.DataBind();
            ddlNecessidadeEspecial.Items.Insert(0, itemVazio);
        }
        private void ControlarEnderecoPais()
        {
            if (ddlPaisNasc.SelectedItem != null)
            {
                if (ddlPaisNasc.SelectedItem.Text.ToUpper() != "BRASIL")
                {
                    txtMunicipioNaturalidade.Visible = true;
                    tseNaturalidade.Visible = false;
                    if (_tipoOperacao == TipoOperacao.Novo || _tipoOperacao == TipoOperacao.Alterar || _tipoOperacao == TipoOperacao.ConsultarPessoa)
                        txtEstadoNaturalidade.Attributes.Remove("readonly");
                }
                else
                {
                    txtMunicipioNaturalidade.Visible = false;
                    tseNaturalidade.Visible = true;

                    txtEstadoNaturalidade.Attributes.Add("readonly", "readonly");
                }
            }
        }

        private QueryTable CarregarDadosDrop(string idDrop)
        {
            QueryTable dadosDrop = null;

            try
            {
                ListItem item = new ListItem("Selecione", string.Empty);

                switch (idDrop)
                {
                    case "ddlEst_Civil":
                        {
                            dadosDrop = RN.Basico.ConsultaItemTabelaValDescr("Estado civil");
                            CarregarDropDownList(ddlEst_Civil, dadosDrop, "");
                            ddlEst_Civil.Items.Insert(0, item);
                            break;
                        }
                    case "ddlNacionalidade":
                        {
                            dadosDrop = RN.Basico.ConsultarNacionalidade();
                            CarregarDropDownList(ddlNacionalidade, dadosDrop, "");
                            ddlNacionalidade.Items.Insert(0, item);
                            break;
                        }
                    case "ddlPaisNasc":
                        {
                            dadosDrop = RN.Basico.ConsultarPais();
                            CarregarDropDownList(ddlPaisNasc, dadosDrop, "");
                            ddlPaisNasc.Items.Insert(0, item);
                            break;
                        }
                    case "ddlRGTipoPessoa":
                        {
                            string param = "TIPO DOC";
                            dadosDrop = RN.Basico.ConsultaItemTabelaValDescr(param);
                            CarregarDropDownList(ddlRGTipoPessoa, dadosDrop, "");
                            ddlRGTipoPessoa.Items.Insert(0, item);
                            break;
                        }               

                    case "ddlRGEmissorPessoa":
                        {
                            string param = "ORGAO RG";
                            dadosDrop = RN.Basico.ConsultaItemTabelaValDescr(param);
                            CarregarDropDownList(ddlRGEmissorPessoa, dadosDrop, "");
                            ddlRGEmissorPessoa.Items.Insert(0, item);
                            break;
                        }

                    case "ddlRGUFPessoa":
                        {
                            dadosDrop = RN.Basico.ConsultarUF();
                            CarregarDropDownList(ddlRGUFPessoa, dadosDrop, "");
                            ddlRGUFPessoa.Items.Insert(0, item);
                            break;
                        }
                    case "ddlRaca":
                        {
                            dadosDrop = RN.Basico.ConsultaItemTabelaValDescr("Cor_Raca");
                            CarregarDropDownList(ddlRaca, dadosDrop, "");
                            ddlRaca.Items.Insert(0, item);
                            break;
                        }

                }
            }
            catch
            {
                throw;
            }
            return dadosDrop;
        }

        private void CarregarDropDownList(DropDownList drop, object data, string defaultValue)
        {

            drop.Items.Clear();
            drop.DataSource = data;
            drop.DataBind();

            if (_tipoOperacao.Equals(TipoOperacao.Novo))
            {
                // drop.SelectedValue = "";
                if (drop == ddlPaisNasc)
                {
                    ListItem listItem = drop.Items.FindByText("BRASIL");
                    if (listItem != null)
                    {
                        drop.ClearSelection();
                        listItem.Selected = true;
                    }
                }
                if (drop == ddlNacionalidade)
                {
                    ListItem listItem = drop.Items.FindByText("BRASILEIRA");
                    if (listItem != null)
                    {
                        drop.ClearSelection();
                        listItem.Selected = true;
                    }
                }
            }           
        }

        private void LimparEndereco()
        {
            txtMunicipio.Text = string.Empty;
            tseMunicipio.ResetValue();
            txtEstado.Value = string.Empty;
            txtEnderecoPessoa.Text = string.Empty;
            txtCEP.Text = string.Empty;
            txtEndCompl.Text = string.Empty;
            txtEndNumPessoa.Text = string.Empty;
            txtBairro.Text = string.Empty;
        }

        private void LimparEnderecoNascimento()
        {
            txtMunicipioNaturalidade.Text = string.Empty;
            tseNaturalidade.ResetValue();
            txtEstadoNaturalidade.Value = string.Empty;
        }

        protected void txtCPFBusca_TextChanged(object sender, EventArgs e)
        {
            try
            {
                RN.DTOs.DadosVoluntario dadosVoluntario = new Techne.Lyceum.RN.DTOs.DadosVoluntario();
                RN.Docentes rnDocentes = new Techne.Lyceum.RN.Docentes();
                List<string> mensagens = new List<string>();

                lblmensagemBloqueio.Visible = false;
                divDados.Visible = false;
                btnSalvar.Visible = false;

                if (!txtCPFBusca.Text.IsNullOrEmptyOrWhiteSpace() && txtCPFBusca.Text.RetirarMascaraCPF().Length == 11)
                {
                    txtCPFBusca.Text = txtCPFBusca.Text.AplicarMascaraCPF();
                    CarregaCombo();
                    dadosVoluntario = rnDocentes.ObtemDadosVoluntariolPor(txtCPFBusca.Text.RetirarMascaraCPF());

                    if (dadosVoluntario.PessoaId > 0)
                    {
                        if (_tipoOperacao == TipoOperacao.Novo)
                        {
                            if (rnDocentes.EhVoluntarioPor(dadosVoluntario.PessoaId))
                            {
                                mensagens.Add("Número de CPF já cadastrado como voluntário.");
                            }
                            else
                            {
                                if (rnDocentes.EhDocenteAtivoPor(dadosVoluntario.PessoaId))
                                {
                                    mensagens.Add("O Voluntário não pode ser um docente ativo (sem data de demissão).");
                                }

                                if (rnDocentes.PossuiLicencaDefinitivaPor(dadosVoluntario.PessoaId))
                                {
                                    mensagens.Add("O Voluntário não pode ser um docente com afastamento definitivo.");
                                }

                                if (rnDocentes.PossuiLotacaoAtivaPor(dadosVoluntario.PessoaId))
                                {
                                    mensagens.Add("O Voluntário não pode ter uma lotação ativa.");
                                }
                            }

                            if (mensagens.Count > 0)
                            {
                                lblMensagem.Text = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y).Replace(Environment.NewLine, "<br />");
                                return;
                            }
                        }
                        PreencheDadosVoluntario(dadosVoluntario);
                        if (dadosVoluntario.Bloqueado)
                        {
                            lblmensagemBloqueio.Visible = true;
                            DesabilitaCampos();
                            txtPISPASEP.Enabled = true;
                            rblLocalizacaoUF.Enabled = true;
                        }
                        else
                        {
                            HabilitaCampos();
                        }
                    }
                    divDados.Visible = true;
                    btnSalvar.Visible = true;
                    txtCPF.Text = txtCPFBusca.Text.AplicarMascaraCPF();
                    txtCPF.Enabled = false;

                }
                else
                {
                    lblMensagem.Text = "CPF é de preenchimento obrigatório.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        private void PreencheDadosVoluntario(RN.DTOs.DadosVoluntario dadosVoluntario)
        {
            txtPessoa.Text = Convert.ToString(dadosVoluntario.PessoaId);
            txtIDINEP.Text = Convert.ToString(dadosVoluntario.IdInep);
            txtNomeComplPessoa.Text = dadosVoluntario.Nome;
            txtNomeSocial.Text = dadosVoluntario.NomeSocial;
            dteDtNasc.Date = Convert.ToDateTime(dadosVoluntario.DataNascimento);
            rblSexo.Text = dadosVoluntario.Sexo;
            PreencherDadoCombo(ddlRaca, Convert.ToString(dadosVoluntario.CorRaca));
            txtEnderecoPessoa.Text = dadosVoluntario.Endereco;
            txtEndNumPessoa.Text = dadosVoluntario.Numero;
            txtEndCompl.Text = dadosVoluntario.Complemento;

            if (!string.IsNullOrEmpty(dadosVoluntario.PaisNascimento))
            {
                string descricaoPais = RN.Endereco.ObterPais(dadosVoluntario.PaisNascimento);

                //verifica se valor não é Brasil
                if (descricaoPais.ToUpper() != "BRASIL")
                {
                    //obtém o municipio estrangeiro
                    SimpleRow sr = RN.Endereco.ObterMunicipioEstrangeiro(dadosVoluntario.MunicipioNascimento);

                    //verifica se a função retornou algum valor para a simplerow
                    if (sr != null)
                    {
                        //preenche os dados obtidos de municipio estrangeiro
                        if (!sr["nome"].IsNull)
                            txtMunicipioNaturalidade.Text = Convert.ToString(sr["nome"]);

                        if (!sr["nome_estado"].IsNull)
                            txtEstadoNaturalidade.Value = Convert.ToString(sr["nome_estado"]);
                    }
                }
                else //se for Brasil
                {
                    //verifica se existe valor para municipio
                    if (!string.IsNullOrEmpty(dadosVoluntario.MunicipioNascimento))
                    {
                        //preenche os dados nos controles da tela
                        tseNaturalidade.DBValue = dadosVoluntario.MunicipioNascimento;
                        //obtém a UF de acordo com o codigo do municipío
                        txtEstadoNaturalidade.Value = RN.Endereco.ObterUFMunicipio(dadosVoluntario.MunicipioNascimento);
                    }
                    else
                    {
                        tseNaturalidade.ResetValue();
                        txtEstadoNaturalidade.Value = string.Empty;
                    }
                }
            }

            if (!string.IsNullOrEmpty(dadosVoluntario.Municipio))
            {
                tseMunicipio.DBValue = dadosVoluntario.Municipio;
                txtEstado.Value = RN.Endereco.ObterUFMunicipio(dadosVoluntario.Municipio);

                if (tseMunicipio.DBValue.IsNull)
                {
                    SimpleRow sr = RN.Endereco.ObterMunicipioEstrangeiro(dadosVoluntario.Municipio);

                    if (sr != null)
                    {
                        if (!sr["nome"].IsNull)
                            txtMunicipio.Text = Convert.ToString(sr["nome"]);

                        if (!sr["nome_estado"].IsNull)
                            txtEstado.Value = Convert.ToString(sr["nome_estado"]);
                    }
                }
                else
                {
                    txtMunicipio.Visible = false;
                }
            }
            else
            {
                tseMunicipio.ResetValue();
                txtEstado.Value = string.Empty;
            }

            txtBairro.Text = dadosVoluntario.Bairro;
            txtCEP.Text = dadosVoluntario.Cep;
            Int64 result;
            if (Int64.TryParse(dadosVoluntario.Telefone, out result))
                txtFone.Text = string.Format("{0:(00)0000-0000}", result);
            else
                txtFone.Text = dadosVoluntario.Telefone;

            long resultado;
            if (long.TryParse(dadosVoluntario.Celular, out resultado))
            {
                if (dadosVoluntario.Celular.Length == 10)
                {
                    txtCelular.Text = string.Format("{0:(00)0000-0000}", resultado);
                }
                else
                {
                    txtCelular.Text = string.Format("{0:(00)00000-0000}", resultado);
                }
            }

            txtEmail.Text = dadosVoluntario.Email;

            txtRGNumPessoa.Text = dadosVoluntario.RgNumero;

            if (!string.IsNullOrEmpty(Convert.ToString(dadosVoluntario.RgDataExpedicao)))
                dteRGDataExpPessoa.Date = Convert.ToDateTime(dadosVoluntario.RgDataExpedicao);
            else
                dteRGDataExpPessoa.Text = string.Empty;

            PreencherDadoCombo(ddlNacionalidade, Convert.ToString(dadosVoluntario.Nacionalidade));
            PreencherDadoCombo(ddlPaisNasc, Convert.ToString(dadosVoluntario.PaisNascimento));
            PreencherDadoCombo(ddlRGTipoPessoa, Convert.ToString(dadosVoluntario.RgTipo));
            PreencherDadoCombo(ddlNecessidadeEspecial, Convert.ToString(dadosVoluntario.NecessidadeEspecial));
            PreencherDadoCombo(ddlRGUFPessoa, Convert.ToString(dadosVoluntario.RgUf));
            PreencherDadoCombo(ddlRGEmissorPessoa, Convert.ToString(dadosVoluntario.RgEmissor));
            PreencherDadoCombo(ddlEst_Civil, Convert.ToString(dadosVoluntario.EstadoCivl));

            ControlarObrigatoriedadeDocumentos(ddlRGTipoPessoa.Text);

            if (Int64.TryParse(dadosVoluntario.Cpf, out result))
                txtCPF.Text = string.Format(@"{0:000\.000\.000-00}", result);
            else
            {
                txtCPF.Text = dadosVoluntario.Cpf;
                txtCPF.ReadOnly = true;
            }
        }
        #endregion

        #region Private Methods

        private void LimparCamposVinculo()
        {
            this.tseUnidadeEnsino.ResetValue();
            dtVincInicio.Text = String.Empty;
            dtVincFim.Text = String.Empty;
            chkPrincipal.Checked = false;

        }

        private void PreencherDadosTelaPessoa(LyPessoa dadosPessoa)
        {
            RN.FlPessoa rnFlPessoa = new FlPessoa();
            string zonaResidencial = rnFlPessoa.ObtemZonaResidencialPor(dadosPessoa.Pessoa);

            txtPessoa.Text = Convert.ToString(dadosPessoa.Pessoa);
            txtIDINEP.Text = Convert.ToString(dadosPessoa.Id_censo);
            txtNomeComplPessoa.Text = dadosPessoa.Nome_compl;
            txtNomeSocial.Text = dadosPessoa.Nome_social;
            dteDtNasc.Date = Convert.ToDateTime(dadosPessoa.Dt_nasc);

            txtPISPASEP.Text = dadosPessoa.Pispasep;
            if (!zonaResidencial.IsNullOrEmptyOrWhiteSpace())
            {
                if (rblLocalizacaoUF.Items.FindByValue(zonaResidencial) != null)
                {
                    rblLocalizacaoUF.Text = zonaResidencial;
                    rblLocalizacaoUF.SelectedValue = zonaResidencial;
                }
            }

            if (!dadosPessoa.Sexo.IsNullOrEmptyOrWhiteSpace())
            {
                if (rblSexo.Items.FindByValue(dadosPessoa.Sexo) != null)
                {
                    rblSexo.Text = dadosPessoa.Sexo;
                    rblSexo.SelectedValue = dadosPessoa.Sexo;
                }
            }

            PreencherDadoCombo(ddlRaca, Convert.ToString(dadosPessoa.Etnia));
            txtEnderecoPessoa.Text = dadosPessoa.Endereco;
            txtEndNumPessoa.Text = dadosPessoa.End_num;
            txtEndCompl.Text = dadosPessoa.End_compl;

            if (!string.IsNullOrEmpty(dadosPessoa.Pais_nasc))
            {
                string descricaoPais = RN.Endereco.ObterPais(dadosPessoa.Pais_nasc);

                //verifica se valor não é Brasil
                if (descricaoPais.ToUpper() != "BRASIL")
                {
                    //obtém o municipio estrangeiro
                    SimpleRow sr = RN.Endereco.ObterMunicipioEstrangeiro(dadosPessoa.Municipio_nasc);

                    //verifica se a função retornou algum valor para a simplerow
                    if (sr != null)
                    {
                        //preenche os dados obtidos de municipio estrangeiro
                        if (!sr["nome"].IsNull)
                            txtMunicipioNaturalidade.Text = Convert.ToString(sr["nome"]);

                        if (!sr["nome_estado"].IsNull)
                            txtEstadoNaturalidade.Value = Convert.ToString(sr["nome_estado"]);
                    }
                }
                else //se for Brasil
                {
                    //verifica se existe valor para municipio
                    if (!string.IsNullOrEmpty(dadosPessoa.Municipio_nasc))
                    {
                        //preenche os dados nos controles da tela
                        tseNaturalidade.DBValue = dadosPessoa.Municipio_nasc;
                        //obtém a UF de acordo com o codigo do municipío
                        txtEstadoNaturalidade.Value = RN.Endereco.ObterUFMunicipio(dadosPessoa.Municipio_nasc);
                    }
                    else
                    {
                        tseNaturalidade.ResetValue();
                        txtEstadoNaturalidade.Value = string.Empty;
                    }
                }
            }

            if (!string.IsNullOrEmpty(dadosPessoa.End_municipio))
            {

                //preenche os dados nos controles da tela
                tseMunicipio.DBValue = dadosPessoa.End_municipio;
                //obtém a UF de acordo com o codigo do municipío
                txtEstado.Value = RN.Endereco.ObterUFMunicipio(dadosPessoa.End_municipio);

                if (!tseMunicipio.DBValue.IsNull)
                {
                    txtMunicipio.Visible = false;
                }
                else
                {
                    SimpleRow sr = RN.Endereco.ObterMunicipioEstrangeiro(dadosPessoa.End_municipio);

                    if (sr != null)
                    {
                        if (!sr["nome"].IsNull)
                            txtMunicipio.Text = Convert.ToString(sr["nome"]);

                        if (!sr["nome_estado"].IsNull)
                            txtEstado.Value = Convert.ToString(sr["nome_estado"]);
                    }
                }
            }
            else
            {
                tseMunicipio.ResetValue();
                txtEstado.Value = string.Empty;
            }

            txtBairro.Text = dadosPessoa.Bairro;
            txtCEP.Text = dadosPessoa.Cep;
            Int64 result;
            if (Int64.TryParse(dadosPessoa.Fone, out result))
                txtFone.Text = string.Format("{0:(00)0000-0000}", result);
            else
                txtFone.Text = dadosPessoa.Fone;

            long resultado;
            if (long.TryParse(dadosPessoa.Celular, out resultado))
            {
                if (dadosPessoa.Celular.Length == 10)
                {
                    txtCelular.Text = string.Format("{0:(00)0000-0000}", resultado);
                }
                else
                {
                    txtCelular.Text = string.Format("{0:(00)00000-0000}", resultado);
                }
            }

            txtEmail.Text = dadosPessoa.E_mail;

            txtRGNumPessoa.Text = dadosPessoa.Rg_num;

            if (!string.IsNullOrEmpty(Convert.ToString(dadosPessoa.Rg_dtexp)))
                dteRGDataExpPessoa.Date = Convert.ToDateTime(dadosPessoa.Rg_dtexp);
            else
                dteRGDataExpPessoa.Text = string.Empty;

            PreencherDadoCombo(ddlPaisNasc, Convert.ToString(dadosPessoa.Pais_nasc));
            PreencherDadoCombo(ddlRGTipoPessoa, Convert.ToString(dadosPessoa.Rg_tipo));
            PreencherDadoCombo(ddlNecessidadeEspecial, Convert.ToString(dadosPessoa.NecessidadeEspecialId));
            PreencherDadoCombo(ddlRGUFPessoa, Convert.ToString(dadosPessoa.Rg_uf));
            PreencherDadoCombo(ddlRGEmissorPessoa, Convert.ToString(dadosPessoa.Rg_emissor));
            
            if (!string.IsNullOrEmpty(dadosPessoa.Est_civil))
            {
                if (ddlEst_Civil.Items.FindByValue(dadosPessoa.Est_civil) != null)
                {
                    ddlEst_Civil.SelectedValue = dadosPessoa.Est_civil;
                }
                else
                {
                    ddlEst_Civil.SelectedValue = string.Empty;
                }
            }

            if (!string.IsNullOrEmpty(dadosPessoa.Nacionalidade))
            {
                if (ddlNacionalidade.Items.FindByValue(dadosPessoa.Nacionalidade) != null)
                {
                    ddlNacionalidade.SelectedValue = dadosPessoa.Nacionalidade;
                }
            }     
            ControlarObrigatoriedadeDocumentos(ddlRGTipoPessoa.Text);

            if (Int64.TryParse(dadosPessoa.Cpf, out result))
                txtCPF.Text = string.Format(@"{0:000\.000\.000-00}", result);
            else
            {
                txtCPF.Text = dadosPessoa.Cpf;
                txtCPF.ReadOnly = true;
            }
        }      

        private void ControlarObrigatoriedadeDocumentos(string tipoDocumento)
        {
            if (ddlRGTipoPessoa.SelectedValue == "RG")
            {
                lblRGUFPessoa.Text = "Estado*: ";
                lblRGUFPessoa.Font.Bold = true;
                lblRGDataExpPessoa.Text = "Data de Expedição*: ";
                lblRGDataExpPessoa.Font.Bold = true;
            }
            else
            {
                lblRGUFPessoa.Text = "Estado: ";
                lblRGUFPessoa.Font.Bold = false;
                lblRGDataExpPessoa.Text = "Data de Expedição: ";
                lblRGDataExpPessoa.Font.Bold = false;
            }
        }

        #endregion

        #region odsVinculos

        public object Listar(object matricula)
        {
            if (matricula.ToString().IsNullOrEmptyOrWhiteSpace())
            {
                return null;
            }

            return RN.VinculoTce.Listar(matricula.ToString());
        }

        public void Update(object ID_VINCULO, object UNIDADE_ENS, object NOME_COMP, object DT_INICIO, object DT_FIM, object PRINCIPAL)
        {
        }

        protected void odsVinculos_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var vinculo = new TceVinculo
            {
                IdVinculo = Convert.ToInt32(e.InputParameters["ID_VINCULO"]),
                DtInicio = e.InputParameters["DT_INICIO"] == null ? DateTime.MinValue : Convert.ToDateTime(e.InputParameters["DT_INICIO"]),
                DtFim = e.InputParameters["DT_FIM"] == null ? DateTime.MinValue : Convert.ToDateTime(e.InputParameters["DT_FIM"]),
                UnidadeEnsino = Convert.ToString(e.InputParameters["UNIDADE_ENS"]),
                Principal = Convert.ToBoolean(e.InputParameters["PRINCIPAL"]),
                Matricula = string.IsNullOrEmpty(tseVoluntarios["matricula"].ToString()) ? txtVoluntario.Text : tseVoluntarios["matricula"].ToString()
            };

            var validacao = RN.VinculoTce.ValidarUpdate(vinculo);

            if (validacao.Valido)
            {
                if (vinculo.Principal)
                {
                    RN.VinculoTce.InserirOuAtualizarLotacao(vinculo, Convert.ToDecimal(txtPessoa.Text), User.Identity.Name);
                }

                RN.VinculoTce.Alterar(vinculo);

                tseVoluntarios.ResetValue();
                tseVoluntarios.DBValue = vinculo.Matricula;
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        #endregion

        #region grdVinculos

        protected void btnSalvarVinculo_Click(object sender, EventArgs e)
        {
            try
            {
                VinculoTce rnVinculo = new VinculoTce();

                var vinculo = new TceVinculo
                {
                    DtInicio = string.IsNullOrEmpty(dtVincInicio.Text) ? DateTime.MinValue : Convert.ToDateTime(dtVincInicio.Text),
                    DtFim = string.IsNullOrEmpty(dtVincFim.Text) ? DateTime.MinValue : Convert.ToDateTime(dtVincFim.Text),
                    UnidadeEnsino = ((!this.tseUnidadeEnsino.DBValue.IsNull) && (this.tseUnidadeEnsino.IsValidDBValue)) ? Convert.ToString(tseUnidadeEnsino.DBValue) : string.Empty,
                    Principal = chkPrincipal.Checked,
                    Matricula = txtVoluntario.Text
                };

                var validacao = RN.VinculoTce.ValidarInsercao(vinculo);

                if (validacao.Valido)
                {
                    RN.VinculoTce.Inserir(vinculo);

                    if (vinculo.Principal && rblFuncao.SelectedValue == "REG MAIS EDUCACAO")
                    {
                        RN.VinculoTce.InserirLotacao(vinculo, Convert.ToDecimal(txtPessoa.Text), User.Identity.Name);
                    }

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Vínculo Incluído com sucesso.');", true);

                    tseVoluntarios.ResetValue();
                    tseVoluntarios.DBValue = vinculo.Matricula;

                    this.LimparCamposVinculo();

                    //Verifica se não existe outro vinculo principal cadastrado
                    if (!rnVinculo.VerificaVinculoPrincipal(vinculo.Matricula))
                    {
                        //Se nao existir, força o primeiro vinculo a ser principal
                        chkPrincipal.Enabled = false;
                        chkPrincipal.Checked = true;
                    }
                    else
                    {
                        chkPrincipal.Enabled = true;
                    }

                    this.odsVinculos.Select();
                    this.odsVinculos.DataBind();
                    this.grdVinculos.DataBind();
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

        protected void grdVinculos_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdVinculos.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "PRINCIPAL")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "NOME_COMP")
                    e.Editor.ReadOnly = true;
            }
            else if (grdVinculos.IsEditing)
            {
                if ((e.Column.FieldName) == "PRINCIPAL")
                {
                    if (Convert.ToBoolean(e.Value))
                        e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "NOME_COMP")
                    e.Editor.ReadOnly = true;
            }
        }

        protected void grdVinculos_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
        }

        protected void grdVinculos_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {

        }

        protected void grdVinculos_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdVinculos);
        }

        #endregion
    }
}
