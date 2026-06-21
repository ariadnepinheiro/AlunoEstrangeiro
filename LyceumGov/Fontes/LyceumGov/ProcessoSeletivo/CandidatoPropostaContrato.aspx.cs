using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Controls;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using System.Collections.Generic;
using Techne.Library.Sql.Structure;
using System.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.ContratoTemporario;
using Techne.Lyceum.RN.ContratoTemporario.Entidades;

namespace Techne.Lyceum.Net.ProcessoSeletivo
{
    [
     NavUrl("~/ProcessoSeletivo/CandidatoPropostaContrato.aspx"),
      ControlText("CandidatoPropostaContrato"),
      Title("Proposta de Contrato Temporário"),
    ]

    public partial class CandidatoPropostaContrato : TPage
    {
        #region Código Padrão Techne

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

        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        #endregion

        #region Propriedades e Enum
        public enum TipoOperacao
        {
            Novo,
            Excluir,
            Alterar,
            Consultar,
            Inicial,
            Confirmar,
            Sucesso
        }

        private TipoOperacao _tipoOperacao
        {
            get { return (TipoOperacao)ViewState["_tipoOperacao"]; }
            set { ViewState["_tipoOperacao"] = value; }
        }
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string onKeyPressSomenteNumeros = "return somenteNumeros(event);";
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    pcCandidatoPropostaContrato.ActiveTabIndex = 0; //abrir sempre na primeira aba                    
                    _tipoOperacao = TipoOperacao.Inicial;
                    ControlarTipoOperacao();
                }

                tseCoordenadoria.Mode = ControlMode.View;
                tseMunicipio.Mode = ControlMode.View;
                tseNaturalidade.Mode = ControlMode.View;
                dtAdmissao.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                dtDataExped.MaxDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                tblRegional.Visible = false;

                IDictionary<string, string> pares = new Dictionary<string, string>();
                pares.Add("candidato", tseInscricao.DBValue.ToString());
                pares.Add("concurso", tseConcurso.DBValue.ToString());
                btnImprimir.Attributes.Add(
                    "onclick",
                    @"
                    javascript:window.open('../Relatorio/Relatorios.aspx?report=rspropcontrtempinterno&grp=processoseletivo&" +
                        TPage.CodificaQueryString(pares) +
                        "','', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes'); return false;"
                );

                txtMatricula.Attributes.Add("onkeypress", onKeyPressSomenteNumeros);
                txtIdentidadeFuncional.Attributes.Add("onkeypress", onKeyPressSomenteNumeros);
                txtIdentidadeFuncional.Attributes.Add("onblur", "validarIdentidadeFuncional(this)");
                txtVinculo.Attributes.Add("onkeypress", onKeyPressSomenteNumeros);
                txtVinculo.Attributes.Add("onblur", "validarIdentidadeFuncional(this)");
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

        protected void btnCancelar_Click(object sender, ImageClickEventArgs e)
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

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                LyLotacao lotacao = new LyLotacao();
                LyCandidatoDocente candidatoDocente = new LyCandidatoDocente();
                LyGrupoHabilitacaoDoc habilitacao = new LyGrupoHabilitacaoDoc();
                LyDocente docente = new LyDocente();
                LyPessoa pessoa = new LyPessoa();
                RN.CategoriaDocente rnCategoriaDocente = new Techne.Lyceum.RN.CategoriaDocente();
                RN.RecursosHumanos.Entidades.PessoaDadosBancarios pessoaDadosBancarios = new Techne.Lyceum.RN.RecursosHumanos.Entidades.PessoaDadosBancarios();
                RN.RecursosHumanos.Entidades.Acumulacao acumulacao = new Techne.Lyceum.RN.RecursosHumanos.Entidades.Acumulacao();
                RN.CandidatoDocente rnCandidatoDocente = new Techne.Lyceum.RN.CandidatoDocente();
                ValidacaoDados validacao = new ValidacaoDados();
                string mensagem;
                string script;

                if (ddlSituacao.SelectedItem.Text == RN.ProcessoSeletivo.Status.Faltoso.ToString() || ddlSituacao.SelectedItem.Text == RN.ProcessoSeletivo.Status.Inabilitado.ToString() || ddlSituacao.SelectedItem.Text == RN.ProcessoSeletivo.Status.Desistente.ToString())
                {
                    rnCandidatoDocente.AtualizaSituacaoCandidato(tseInscricao.DBValue.ToString(), tseConcurso.DBValue.ToString(), Convert.ToInt32(ddlSituacao.SelectedValue));
                    mensagem = "Situação atualizada com sucesso.";
                }
                else
                {
                    candidatoDocente.Concurso = (!this.tseConcurso.DBValue.IsNull && this.tseConcurso.IsValidDBValue) ? tseConcurso.DBValue.ToString() : null;
                    candidatoDocente.Candidato = (!this.tseInscricao.DBValue.IsNull && this.tseInscricao.IsValidDBValue) ? tseInscricao.DBValue.ToString() : null;
                    candidatoDocente.Status = !ddlSituacao.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(ddlSituacao.SelectedValue) : -1;
                    candidatoDocente.Dt_proposta = !dtAdmissao.Text.IsNullOrEmptyOrWhiteSpace() ? dtAdmissao.Date : (DateTime?)null;
                    candidatoDocente.Carga_Horaria = !ddlCargaHoraria.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlCargaHoraria.SelectedValue) : -1;
                    candidatoDocente.IdFuncional = !txtIdentidadeFuncional.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtIdentidadeFuncional.Text) : (int?)null;
                    candidatoDocente.Categoria = !txtCargo.Text.IsNullOrEmptyOrWhiteSpace() ? txtCargo.Text : null;
                    candidatoDocente.Cprof_uf = !ddDlCprof_Uf.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddDlCprof_Uf.SelectedValue : null;
                    candidatoDocente.Cprof_num = !txtCProfNum.Text.IsNullOrEmptyOrWhiteSpace() ? txtCProfNum.Text.Trim() : null;
                    candidatoDocente.Cprof_serie = !txtCProfSerie.Text.IsNullOrEmptyOrWhiteSpace() ? txtCProfSerie.Text.Trim() : null;
                    candidatoDocente.Cprof_dtexp = !string.IsNullOrEmpty(dteCProfDtExp.Text) ? dteCProfDtExp.Date : (DateTime?)null;
                    candidatoDocente.Pis_pasep = !txtPISPASEP.Text.IsNullOrEmptyOrWhiteSpace() ? txtPISPASEP.Text.Trim() : null;

                    pessoa.Pessoa = !string.IsNullOrEmpty(txtPessoa.Text) ? Convert.ToDecimal(txtPessoa.Text) : 0m;
                    pessoa.IdFuncional = !txtIdentidadeFuncional.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtIdentidadeFuncional.Text) : (int?)null;
                    pessoa.Nome_compl = !txtNome_Comp.Text.IsNullOrEmptyOrWhiteSpace() ? txtNome_Comp.Text : null;
                    pessoa.Endereco = !txtEndereco.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndereco.Text.Trim() : null;
                    pessoa.End_compl = !txtEndCompl.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndCompl.Text.Trim() : null;
                    pessoa.End_num = !txtEndNum.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndNum.Text.Trim() : null;
                    pessoa.End_municipio = (!this.tseMunicipio.DBValue.IsNull && this.tseMunicipio.IsValidDBValue) ? tseMunicipio.DBValue.ToString() : null;
                    pessoa.Bairro = !txtBairro.Text.IsNullOrEmptyOrWhiteSpace() ? txtBairro.Text : null;
                    pessoa.Cep = !txtCep.Text.IsNullOrEmptyOrWhiteSpace() ? txtCep.Text.RetirarCaracteres() : null;
                    pessoa.Rg_emissor = !cmbRGEmissor.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? cmbRGEmissor.SelectedValue : null;
                    pessoa.Rg_tipo = !ddlRGTipo.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlRGTipo.SelectedValue : null;
                    pessoa.Municipio_nasc = (!this.tseNaturalidade.DBValue.IsNull && this.tseNaturalidade.IsValidDBValue) ? tseNaturalidade.DBValue.ToString() : null;
                    pessoa.Pais_nasc = !ddlPaisNasc.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlPaisNasc.SelectedValue : null;
                    pessoa.Nacionalidade = !ddlNacionalidade.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlNacionalidade.SelectedValue : null;
                    pessoa.Etnia = !ddlCorRaca.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlCorRaca.SelectedValue : null;
                    pessoa.Rg_uf = !txtRGUF.Text.IsNullOrEmptyOrWhiteSpace() ? txtRGUF.Text : null;
                    pessoa.End_pais = !ddlPaises.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlPaises.SelectedValue : null;
                    pessoa.NecessidadeEspecialId = !ddlNecessidadeEspecial.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlNecessidadeEspecial.SelectedValue) : (int?)null;
                    pessoa.Est_civil = !ddlEst_Civil.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlEst_Civil.SelectedValue : null;
                    pessoa.Cprof_uf = !ddDlCprof_Uf.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddDlCprof_Uf.SelectedValue : null;
                    pessoa.Sexo = !rblSexo.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToString(rblSexo.Text) : null;;
                    pessoa.Fone = !txtFone.Text.IsNullOrEmptyOrWhiteSpace() ? txtFone.Text.Trim() : null;
                    pessoa.Celular = !txtCelular.Text.IsNullOrEmptyOrWhiteSpace() ? txtCelular.Text.Trim() : null;
                    pessoa.E_mail = !txtEmail.Text.IsNullOrEmptyOrWhiteSpace() ? txtEmail.Text.Trim() : null;
                    pessoa.Rg_num = !txtRGNum.Text.IsNullOrEmptyOrWhiteSpace() ? txtRGNum.Text.Trim() : null;
                    pessoa.NomeMae = !txtNomeMae.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeMae.Text.TrimEnd() : null;
                    pessoa.NomePai = !txtNomePai.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomePai.Text.TrimEnd() : null;
                    pessoa.Cpf = !txtCPF.Text.IsNullOrEmptyOrWhiteSpace() ? txtCPF.Text.RetirarMascaraCPF().Trim() : null;
                    pessoa.Cprof_num = !txtCProfNum.Text.IsNullOrEmptyOrWhiteSpace() ? txtCProfNum.Text.Trim() : null;
                    pessoa.Cprof_serie = !txtCProfSerie.Text.IsNullOrEmptyOrWhiteSpace() ? txtCProfSerie.Text.Trim() : null;
                    pessoa.Dt_nasc = !string.IsNullOrEmpty(dtDataNasc.Text) ? dtDataNasc.Date : (DateTime?)null;
                    pessoa.Rg_dtexp = !string.IsNullOrEmpty(dtDataExped.Text) ? dtDataExped.Date : (DateTime?)null;
                    pessoa.Cprof_dtexp = !string.IsNullOrEmpty(dteCProfDtExp.Text) ? dteCProfDtExp.Date : (DateTime?)null;
                    pessoa.Id_censo = null;
                    pessoa.Pispasep = !txtPISPASEP.Text.IsNullOrEmptyOrWhiteSpace() ? txtPISPASEP.Text.Trim() : null;
                    pessoa.UsuarioId = User.Identity.Name;

                    long resultado;

                    if (long.TryParse(txtCelular.Text.Trim().RetirarMascaraTelefone(), out resultado))
                    {
                        if (txtCelular.Text.Trim().RetirarMascaraTelefone().Length == 10)
                        {
                            pessoa.Celular = string.Format("{0:(00)0000-0000}", resultado);
                            candidatoDocente.Celular = pessoa.Celular;
                        }
                        if (txtCelular.Text.Trim().RetirarMascaraTelefone().Length == 11)
                        {
                            pessoa.Celular = string.Format("{0:(00)00000-0000}", resultado);
                            candidatoDocente.Celular = pessoa.Celular;
                        }
                        txtCelular.Text = pessoa.Celular;
                    }
                    else
                    {
                        pessoa.Celular = null;
                        candidatoDocente.Celular = null;
                    }

                    docente.Pessoa = !string.IsNullOrEmpty(txtPessoa.Text) ? Convert.ToDecimal(txtPessoa.Text) : 0m;
                    docente.Num_func = !string.IsNullOrEmpty(txtNumFunc.Text) ? Convert.ToDecimal(txtNumFunc.Text) : 0m;
                    docente.Matricula = !txtMatricula.Text.IsNullOrEmptyOrWhiteSpace() ? txtMatricula.Text : null;
                    docente.Vinculo = !txtVinculo.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtVinculo.Text) : -1;
                    docente.Dt_admissao = !dtAdmissao.Text.IsNullOrEmptyOrWhiteSpace() ? dtAdmissao.Date : (DateTime?)null;
                    docente.Categoria = !txtCargo.Text.IsNullOrEmptyOrWhiteSpace() ? txtCargo.Text : null;
                    docente.RegimeContratacaoId = (int)RN.RecursosHumanos.RegimeContratacao.Regime.ContratoTemporario;
                    docente.Candidato = (!this.tseInscricao.DBValue.IsNull && this.tseInscricao.IsValidDBValue) ? tseInscricao.DBValue.ToString() : null;
                    docente.Concurso = (!this.tseConcurso.DBValue.IsNull && this.tseConcurso.IsValidDBValue) ? tseConcurso.DBValue.ToString() : null;
                    docente.Regime_trabalho = !ddlCargaHoraria.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlCargaHoraria.SelectedValue : null;
                    docente.Senha_alterada = "S";
                    docente.Senha_dol = !txtCPF.Text.IsNullOrEmptyOrWhiteSpace() ? txtCPF.Text.RetirarMascaraCPF().Trim() : null;
                    docente.Acumulacao = rblAcumulacao.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (int)RN.Docentes.PossuiAcumulacao.NaoInformado : Convert.ToInt32(rblAcumulacao.SelectedValue);
                    docente.Usuario = User.Identity.Name;

                    if (rblAcumulacao.SelectedValue == "1")
                    {
                        acumulacao.MatriculaOrgao = txtMatriculaAcumulacao.Text;
                        acumulacao.Orgao = txtOrgaoAcumulacao.Text;
                        acumulacao.NumeroProcesso = txtNumProcessoAcumulacao.Text;
                        acumulacao.UsuarioId = User.Identity.Name;
                    }

                    lotacao.Pessoa = !string.IsNullOrEmpty(txtPessoa.Text) ? Convert.ToDecimal(txtPessoa.Text) : 0m;
                    lotacao.Matricula = !txtMatricula.Text.IsNullOrEmptyOrWhiteSpace() ? txtMatricula.Text.Trim() : null;
                    lotacao.Ordem = 1;
                    lotacao.Funcao = !txtCargo.Text.IsNullOrEmptyOrWhiteSpace() ? rnCategoriaDocente.RetornaFuncaoPor(txtCargo.Text) : null;
                    if (!dtAdmissao.Text.IsNullOrEmptyOrWhiteSpace())
                    {
                        lotacao.DataNomeacao = Convert.ToDateTime(dtAdmissao.Text);
                    }
                    lotacao.UnidadeEns = (!this.tseUnidade_Ensino.DBValue.IsNull && this.tseUnidade_Ensino.IsValidDBValue) ? tseUnidade_Ensino.DBValue.ToString() : null;
                    lotacao.UnidadeFis = (!this.tseUnidade_Ensino.DBValue.IsNull && this.tseUnidade_Ensino.IsValidDBValue) ? tseUnidade_Ensino.DBValue.ToString() : null;
                    lotacao.Nucleo = (!this.tseCoordenadoria.DBValue.IsNull && this.tseCoordenadoria.IsValidDBValue) ? tseCoordenadoria.DBValue.ToString() : null;
                    lotacao.Setor = (!this.tseUnidade_Ensino.DBValue.IsNull && this.tseUnidade_Ensino.IsValidDBValue) ? tseUnidade_Ensino["SETOR"].ToString() : null;
                    lotacao.Categoria = !txtCargo.Text.IsNullOrEmptyOrWhiteSpace() ? txtCargo.Text : null;
                    lotacao.Usuario = User.Identity.Name;
                    lotacao.Readaptado = "N";

                    habilitacao.Agrupamento = (!this.tseDisciplina.DBValue.IsNull && this.tseDisciplina.IsValidDBValue) ? tseDisciplina.DBValue.ToString() : null;
                    habilitacao.AgrupamentoIngresso = "S";
                    habilitacao.Provisorio = "N";
                    habilitacao.Campo01 = "S";
                    habilitacao.Campo02 = "N";

                    pessoaDadosBancarios.Agencia = (!this.tsAgencia.DBValue.IsNull && this.tsAgencia.IsValidDBValue) ? tsAgencia.DBValue.ToString() : null;
                    pessoaDadosBancarios.Banco = (!this.tseBanco.DBValue.IsNull && this.tseBanco.IsValidDBValue) ? Convert.ToDecimal(tseBanco.DBValue.ToString()) : -1;
                    pessoaDadosBancarios.ContaBanco = !txtConta.Text.IsNullOrEmptyOrWhiteSpace() ? txtConta.Text.Trim() : null;
                    pessoaDadosBancarios.PessoaId = !string.IsNullOrEmpty(txtPessoa.Text) ? Convert.ToDecimal(txtPessoa.Text) : 0m;
                    pessoaDadosBancarios.UsuarioId = User.Identity.Name;

                    validacao = rnCandidatoDocente.ValidaProposta(candidatoDocente, pessoa, docente, acumulacao, lotacao, pessoaDadosBancarios,habilitacao);

                    if (validacao.Valido)
                    {
                        rnCandidatoDocente.GeraProposta(candidatoDocente, pessoa, docente, acumulacao, lotacao, pessoaDadosBancarios,habilitacao);
                        mensagem = "Matrícula e lotação incluídas com sucesso.";
                    }
                    else
                    {
                        lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        return;
                    }
                }

                script = @"alert('" + mensagem + @"');";

                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);

                _tipoOperacao = TipoOperacao.Sucesso;
                ControlarTipoOperacao();
                btnSalvar.Visible = false;
                btnCancelar.Visible = false;

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tsAgencia_Changed(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }
                txtAgencia.Text = string.Empty;

                if (!this.tsAgencia.DBValue.IsNull)
                {
                    if (this.tsAgencia.IsValidDBValue)
                    {
                        txtAgencia.Text = tsAgencia.DBValue.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseBanco_Changed(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }
                txtBanco.Text = string.Empty;
                if (!this.tseBanco.DBValue.IsNull)
                {
                    if (this.tseBanco.IsValidDBValue)
                    {
                        txtBanco.Text = tseBanco.DBValue.ToString();
                        tsAgencia.Mode = ControlMode.Edit;
                        tsAgencia.SqlWhere = "banco= '" + txtBanco.Text + "'";
                        tsAgencia.DataBind();
                    }
                }
                else
                {
                    tseBanco.ResetValue();
                    tsAgencia.Mode = ControlMode.View;
                    tsAgencia.ResetValue();
                    tsAgencia.SqlWhere = string.Empty;
                    tsAgencia.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseConcurso_Changed(object sender, EventArgs e)
        {
            try
            {
                pcCandidatoPropostaContrato.Visible = false;
                btnImprimir.Visible = false;
                if (!tseConcurso.IsValidDBValue || tseConcurso.DBValue.IsNull)
                {
                    lblMensagem.Text = "Favor consultar processo seletivo e número de inscrição.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseInscricao_Changed(object sender, EventArgs e)
        {
            try
            {
                if (tseInscricao.IsValidDBValue && !tseInscricao.DBValue.IsNull &&
                    (tseConcurso.IsValidDBValue && !tseConcurso.DBValue.IsNull))
                {
                    pcCandidatoPropostaContrato.Visible = false;

                    RN.CandidatoDocente queryCandidatoDocente = new Techne.Lyceum.RN.CandidatoDocente();
                    bool contratado = queryCandidatoDocente.EhContratadoCandidatoDocente(
                        tseConcurso.DBValue.ToString(), tseInscricao.DBValue.ToString());

                    if (contratado)
                    {
                        _tipoOperacao = TipoOperacao.Consultar;
                        ControlarTipoOperacao();
                    }
                    else
                    {
                        _tipoOperacao = TipoOperacao.Novo;
                        ControlarTipoOperacao();
                    }
                }
                else
                {
                    btnImprimir.Visible = false;
                    lblMensagem.Text = "Favor consultar processo seletivo e número de inscrição.";
                    pcCandidatoPropostaContrato.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void rblAcumulacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtMatriculaAcumulacao.Text = string.Empty;
                txtOrgaoAcumulacao.Text = string.Empty;
                txtNumProcessoAcumulacao.Text = string.Empty;

                if (rblAcumulacao.SelectedValue == "1")
                {
                    txtMatriculaAcumulacao.ReadOnly = false;
                    txtOrgaoAcumulacao.ReadOnly = false;
                    txtNumProcessoAcumulacao.ReadOnly = false;
                }
                else
                {
                    txtMatriculaAcumulacao.ReadOnly = true;
                    txtOrgaoAcumulacao.ReadOnly = true;
                    txtNumProcessoAcumulacao.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlCargaHoraria_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RN.ProcessoSeletivo retornaCategoria = new RN.ProcessoSeletivo();
                txtCargo.Text = retornaCategoria.ObtemCategoriaPor(tseConcurso.DBValue.ToString(), (!ddlCargaHoraria.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDecimal(ddlCargaHoraria.SelectedValue) : 0));
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ControlarTipoOperacao()
        {
            LyCandidatoDocente candidatoDocente = new LyCandidatoDocente();
            RN.CandidatoDocente rnCandidatoDocente = new Techne.Lyceum.RN.CandidatoDocente();
            RN.ProcessoSeletivo rnProcessoSeletivo = new Techne.Lyceum.RN.ProcessoSeletivo();
            DataTable dtFolhaPagamento = new DataTable();

            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        pcCandidatoPropostaContrato.ActiveTabIndex = 0;
                        btnImprimir.Visible = false;
                        tseConcurso.ResetValue();
                        tseInscricao.ResetValue();
                        LimparTela();
                        pcCandidatoPropostaContrato.Visible = false;
                        tseConcurso.Mode = ControlMode.Edit;
                        tseInscricao.Mode = ControlMode.Edit;
                        tseCoordenadoria.Mode = ControlMode.View;
                        tseDisciplina.Mode = ControlMode.View;
                        tseMunicipio.Mode = ControlMode.View;
                        tseNaturalidade.Mode = ControlMode.View;
                        DesabilitaCamposAcumulacao();
                        RetiraVisibilidadeBotao();
                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        btnImprimir.Visible = true;
                        pcCandidatoPropostaContrato.Visible = true;
                        txtCProfNum.ReadOnly = true;
                        txtCProfSerie.ReadOnly = true;
                        dtDataExped.ReadOnly = true;
                        ddDlCprof_Uf.Enabled = false;
                        DesabilitaCamposFolhaPagamento();
                        tseConcurso.Mode = ControlMode.Edit;
                        tseInscricao.Mode = ControlMode.Edit;
                        tseCoordenadoria.Mode = ControlMode.View;
                        tseDisciplina.Mode = ControlMode.View;
                        tseMunicipio.Mode = ControlMode.View;
                        tseMunicipioProc.Mode = ControlMode.View;
                        tseNaturalidade.Mode = ControlMode.View;
                        tseUnidade_Ensino.Mode = ControlMode.View;
                        ddlSituacao.Enabled = false;
                        txtMatricula.ReadOnly = true;
                        txtIdentidadeFuncional.ReadOnly = true;
                        txtVinculo.ReadOnly = true;
                        txtMatriculaAcumulacao.Enabled = false;
                        txtOrgaoAcumulacao.Enabled = false;
                        txtNumProcessoAcumulacao.Enabled = false;
                        rblAcumulacao.Enabled = false;
                        break;
                    }
                case TipoOperacao.Novo:
                    {
                        pcCandidatoPropostaContrato.ActiveTabIndex = 0;
                        btnImprimir.Visible = false;
                        LimparTela();
                       
                        DesabilitaCamposAcumulacao();

                        candidatoDocente = rnCandidatoDocente.ObtemCandidoDocentePor(tseConcurso.DBValue.ToString(), tseInscricao.DBValue.ToString());

                        if (!candidatoDocente.Candidato.IsNullOrEmptyOrWhiteSpace())
                        {
                            pcCandidatoPropostaContrato.Visible = true;
                            PreencherDadosTela(candidatoDocente);

                            dtFolhaPagamento = rnProcessoSeletivo.ObtemDadosFolhaPagamentoPor(tseConcurso.DBValue.ToString(), tseInscricao.DBValue.ToString());
                            if (dtFolhaPagamento.Rows.Count > 0)
                            {
                                PreencherDadosFolhaPagamento(dtFolhaPagamento);
                                btnImprimir.Visible = true;
                            }
                            ImageButton[] controles = new ImageButton[] { btnEditar };
                            ControlarVisibilidadeControle(controles);
                            tseCoordenadoria.Mode = ControlMode.View;
                            tseDisciplina.Mode = ControlMode.View;
                            tseMunicipio.Mode = ControlMode.View;
                            tseMunicipioProc.Mode = ControlMode.View;
                            tseNaturalidade.Mode = ControlMode.View;
                            tseUnidade_Ensino.Mode = ControlMode.View;
                            ddlSituacao.Enabled = false;
                            ddlCargaHoraria.Enabled = false;
                            tsAgencia.Enabled = false;
                            tseBanco.Enabled = false;
                            txtConta.Enabled = false;
                            dtAdmissao.Enabled = false;
                            tseDisciplina.Enabled = true;
                        }
                        else
                        {
                            pcCandidatoPropostaContrato.Visible = false;
                            lblMensagem.Text = "Não existem dados.";
                            _tipoOperacao = TipoOperacao.Inicial;
                            ControlarTipoOperacao();
                        }
                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        pcCandidatoPropostaContrato.ActiveTabIndex = 0;
                        btnImprimir.Visible = true;
                        LimparTela();

                         candidatoDocente = rnCandidatoDocente.ObtemCandidoDocentePor(tseConcurso.DBValue.ToString(), tseInscricao.DBValue.ToString());

                        if (!candidatoDocente.Candidato.IsNullOrEmptyOrWhiteSpace())
                        {
                            pcCandidatoPropostaContrato.Visible = true;
                            PreencherDadosTela(candidatoDocente);
                            dtFolhaPagamento = rnProcessoSeletivo.ObtemDadosFolhaPagamentoPor(tseConcurso.DBValue.ToString(), tseInscricao.DBValue.ToString());

                            if (dtFolhaPagamento.Rows.Count > 0)
                            {
                                //Apenas para candidatos que já sejam docentes
                                PreencherDadosFolhaPagamento(dtFolhaPagamento);
                                PreencherDadosAcumulacao(tseConcurso.DBValue.ToString(), tseInscricao.DBValue.ToString());
                                btnEditar.Visible = false;
                            }

                            tseCoordenadoria.Mode = ControlMode.View;
                            tseMunicipio.Mode = ControlMode.View;
                            tseNaturalidade.Mode = ControlMode.View;
                            RetiraVisibilidadeBotao();
                            DesabilitaCamposFolhaPagamento();
                            btnImprimir.Visible = true;
                            ddlSituacao.Enabled = false;
                            ddlCorRaca.Enabled = false;
                            ddDlCprof_Uf.Enabled = false;
                            txtCProfSerie.ReadOnly = true;
                            txtCProfNum.ReadOnly = true;
                            dteCProfDtExp.ReadOnly = true;
                            CarregarDadosDropSituacao(candidatoDocente.Status.ToString());
                        }
                        else
                        {
                            pcCandidatoPropostaContrato.Visible = false;
                            lblMensagem.Text = "Não existem dados.";
                            _tipoOperacao = TipoOperacao.Inicial;
                            ControlarTipoOperacao();
                        }
                        break;
                    }
                case TipoOperacao.Alterar:
                    { 
                        ImageButton[] controles = new ImageButton[] { btnCancelar, btnSalvar };
                        ControlarVisibilidadeControle(controles);
                        tseConcurso.Mode = ControlMode.View;
                        tseInscricao.Mode = ControlMode.View;
                        tseMunicipioProc.Mode = ControlMode.View;
                        btnImprimir.Visible = false;
                        CarregarDadosDropSituacao(ddlSituacao.SelectedValue);
                        dtAdmissao.Date = DateTime.Now;
                        HabilitaCamposAlteracao();

                        candidatoDocente = rnCandidatoDocente.ObtemCandidoDocentePor(tseConcurso.DBValue.ToString(), tseInscricao.DBValue.ToString());
                        PreencherDadosTela(candidatoDocente);

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
        }

        private void RetiraVisibilidadeBotao()
        {
            btnEditar.Visible = false;
            btnSalvar.Visible = false;
            btnCancelar.Visible = false;
        }

        protected void HabilitaCamposAlteracao()
        {
            ddDlCprof_Uf.Enabled = true;
            txtCProfSerie.ReadOnly = false;
            txtCProfNum.ReadOnly = false;
            dteCProfDtExp.ReadOnly = false;
            dteCProfDtExp.Enabled = true;
            tseUnidade_Ensino.Mode = ControlMode.Edit;
            tseBanco.Enabled = true;
            tsAgencia.Mode = ControlMode.Edit;
            txtConta.ReadOnly = false;
            dtAdmissao.Enabled = true;
            txtMatriculaAcumulacao.Enabled = true;
            txtOrgaoAcumulacao.Enabled = true;
            txtNumProcessoAcumulacao.Enabled = true;
            rblAcumulacao.Enabled = true;
            rblAcumulacao_SelectedIndexChanged(null, null);
            ddlCargaHoraria.Enabled = true;
            tsAgencia.Enabled = true;
            tseBanco.Enabled = true;
            txtConta.Enabled = true;
            txtMatricula.ReadOnly = true;
            txtIdentidadeFuncional.ReadOnly = false;
            txtVinculo.ReadOnly = false;
            ddlSituacao.Enabled = true;
            pnlCarteiraProfissional.Enabled = true;
        }

        protected void DesabilitaCamposFolhaPagamento()
        {
            ddlCargaHoraria.Enabled = false;
            tseUnidade_Ensino.Mode = ControlMode.View;
            tseBanco.Enabled = false;
            tsAgencia.Mode = ControlMode.View;
            txtConta.ReadOnly = true;
            dtAdmissao.Enabled = false;
        }

        protected void DesabilitaCamposAcumulacao()
        {
            txtMatriculaAcumulacao.Text = string.Empty;
            txtMatriculaAcumulacao.ReadOnly = true;
            txtMatriculaAcumulacao.Enabled = false;
            txtOrgaoAcumulacao.Text = string.Empty;
            txtOrgaoAcumulacao.ReadOnly = true;
            txtOrgaoAcumulacao.Enabled = false;
            txtNumProcessoAcumulacao.Text = string.Empty;
            txtNumProcessoAcumulacao.ReadOnly = true;
            txtNumProcessoAcumulacao.Enabled = false;
            rblAcumulacao.ClearSelection();
            rblAcumulacao.Enabled = false;
        }

        private void ObterDadosPessoa(Ly_pessoa dtPessoa, Ly_candidato_docente.Row dadosCandidato)
        {
            Techne.Lyceum.CR.Ly_pessoa.Row dadosPessoa = dtPessoa.NewRow();

            //não podem ser nulos!
            dadosPessoa.Pessoa = Convert.ToDecimal(txtPessoa.Text); //numérico
            dadosPessoa.IdFuncional = Convert.ToInt32(txtIdentidadeFuncional.Text);
            dadosPessoa.Pispasep = txtPISPASEP.Text;
            dadosPessoa.Nome_compl = txtNome_Comp.Text;
            dadosPessoa.Nome_mae = txtNomeMae.Text;
            dadosPessoa.Nome_pai = txtNomePai.Text;
            dadosPessoa.Endereco = txtEndereco.Text;
            dadosPessoa.End_num = txtEndNum.Text;
            dadosPessoa.End_municipio = txtMunicipio.Text;
            dadosPessoa.Cep = txtCep.Text;
            dadosPessoa.Rg_tipo = dadosCandidato.Rg_tipo;
            dadosPessoa.Municipio_nasc = tseNaturalidade.DBValue.ToString();
            dadosPessoa.Pais_nasc = ddlPaisNasc.SelectedValue;
            dadosPessoa.Nacionalidade = ddlNacionalidade.SelectedValue;
            dadosPessoa.Rg_num = txtRGNum.Text.Trim();
            dadosPessoa.Rg_uf = txtRGUF.Text.Trim();
            dadosPessoa.Rg_emissor = cmbRGEmissor.Text.Trim();
            dadosPessoa.Cpf = txtCPF.Text.Trim().RetirarMascaraCPF();

            //combos com valor padrão vazio            
            dadosPessoa.End_pais = ddlPaises.SelectedValue;
            dadosPessoa.Necessidadeespecialid = !ddlNecessidadeEspecial.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlNecessidadeEspecial.SelectedValue) : (int?)null;
            dadosPessoa.Est_civil = ddlEst_Civil.SelectedValue;
            dadosPessoa.Cprof_uf = ddDlCprof_Uf.SelectedValue.ToString();

            //confere se não estão vazios:
            if (!string.IsNullOrEmpty(dtDataNasc.Text))
                dadosPessoa.Dt_nasc = dtDataNasc.Date;
            if (!string.IsNullOrEmpty(rblSexo.Text))
                dadosPessoa.Sexo = Convert.ToString(rblSexo.Text);
            if (!string.IsNullOrEmpty(txtEndCompl.Text))
                dadosPessoa.End_compl = txtEndCompl.Text;
            if (!string.IsNullOrEmpty(txtBairro.Text))
                dadosPessoa.Bairro = txtBairro.Text;
            if (!string.IsNullOrEmpty(txtFone.Text))
                dadosPessoa.Fone = txtFone.Text.RetirarMascaraTelefone();
            if (!string.IsNullOrEmpty(txtCelular.Text))
                dadosPessoa.Celular = txtCelular.Text.RetirarMascaraTelefone();
            if (!string.IsNullOrEmpty(txtEmail.Text))
                dadosPessoa.E_mail_interno = dadosPessoa.E_mail = txtEmail.Text;

            if (!string.IsNullOrEmpty(Convert.ToString(dadosCandidato.Rg_dtexp)))
                dadosPessoa.Rg_dtexp = dadosCandidato.Rg_dtexp;
            else
                dadosPessoa.Rg_dtexp = null;

            if (!string.IsNullOrEmpty(txtCProfNum.Text))
                dadosPessoa.Cprof_num = txtCProfNum.Text;
            if (!string.IsNullOrEmpty(txtCProfSerie.Text))
                dadosPessoa.Cprof_serie = txtCProfSerie.Text;
            if (!string.IsNullOrEmpty(dteCProfDtExp.Text))
                dadosPessoa.Cprof_dtexp = dteCProfDtExp.Date;

            dadosPessoa.Alist_num = null;
            dadosPessoa.Alist_rm = null;
            dadosPessoa.Alist_serie = null;
            dadosPessoa.Alist_csm = null;
            dadosPessoa.Cr_num = null;
            dadosPessoa.Cr_rm = null;
            dadosPessoa.Cr_serie = null;
            dadosPessoa.Cr_csm = null;
            dadosPessoa.Cr_cat = null;
            dadosPessoa.Alist_dtexp = null;
            dadosPessoa.Cr_dtexp = null;
            dadosPessoa.Teleitor_num = null;
            dadosPessoa.Teleitor_zona = null;
            dadosPessoa.Teleitor_secao = null;
            dadosPessoa.Teleitor_dtexp = null;


            if (!string.IsNullOrEmpty(ddlCorRaca.SelectedValue))
            {
                //Buscar ItemTabelaGeral
                RN.Etnia rnEtnia = new Techne.Lyceum.RN.Etnia();
                dadosPessoa.Etnia = rnEtnia.ObtemTabelaItemIPor(ddlCorRaca.SelectedValue);
            }
            else
            {
                dadosPessoa.Etnia = null;
            }
            dadosPessoa.Usuarioid = User.Identity.Name;
            dtPessoa.Rows.Add(dadosPessoa);
        }

        private void ObterDadosLotacao(Ly_lotacao dtLotacao, Ly_candidato_docente dadosCandidato)
        {
            RN.CategoriaDocente rnCategoriaDocente = new Techne.Lyceum.RN.CategoriaDocente();
            string concurso = tseConcurso.DBValue.ToString();
            string candidato = tseInscricao.DBValue.ToString();

            Techne.Lyceum.CR.Ly_lotacao.Row dadosLotacao = dtLotacao.NewRow();

            //obrigatórios
            dadosLotacao.Ordem = 1;
            dadosLotacao.Data_nomeacao = dtAdmissao.Date;
            dadosLotacao.Matricula = txtMatricula.Text;
            dadosLotacao.Pessoa = Convert.ToDecimal(txtPessoa.Text);

            string funcao = rnCategoriaDocente.RetornaFuncaoPor(dadosCandidato.Rows[0].Categoria);
            if (!string.IsNullOrEmpty(funcao))
                dadosLotacao.Funcao = funcao;

            dadosLotacao.Unidade_ens = tseUnidade_Ensino.DBValue.ToString();
            dadosLotacao.Nucleo = tseCoordenadoria.Value.ToString();
            dadosLotacao.Setor = tseUnidade_Ensino["setor"].ToString();
            dadosLotacao.Turno = null;
            dadosLotacao.Data_desativacao = null;
            dadosLotacao.Ato_oficial = null;
            dadosLotacao.Resp_documentacao = null;
            dadosLotacao.Unidade_fis = null;
            dadosLotacao.Data_nomeacao_do = null;
            dadosLotacao.Data_desativacao_do = null;
            dadosLotacao.Tipo_desativacao = null;

            if (!tseMunicipioProc.DBValue.IsNull)
                dadosLotacao.Municipio = tseMunicipioProc.Value.ToString();

            dtLotacao.Rows.Add(dadosLotacao);
        }

        private void ObterDadosCandidatoDocente(Ly_candidato_docente dtCandidatoDoc, Ly_candidato_docente.Row dadosCandidato)
        {
            string concurso = tseConcurso.DBValue.ToString();
            string candidato = tseInscricao.DBValue.ToString();
            Techne.Lyceum.CR.Ly_candidato_docente.Row dadosCandidatoDocente = dtCandidatoDoc.NewRow();
            dadosCandidatoDocente.Concurso = concurso;
            dadosCandidatoDocente.Candidato = candidato;
            dadosCandidatoDocente.Status = Convert.ToDecimal(ddlSituacao.SelectedValue);
            dadosCandidatoDocente.Dt_proposta = dtAdmissao.Date;
            if (ddlCargaHoraria.SelectedValue != null && ddlCargaHoraria.SelectedValue != "")
                dadosCandidatoDocente.Carga_horaria = Convert.ToDecimal(ddlCargaHoraria.SelectedValue);
            dadosCandidatoDocente.IdFuncional = (string.IsNullOrEmpty(txtIdentidadeFuncional.Text))
                ? 0
                : Convert.ToInt32(txtIdentidadeFuncional.Text);

            dadosCandidatoDocente.Cprof_num = txtCProfNum.Text;
            dadosCandidatoDocente.Cprof_serie = txtCProfSerie.Text;
            dadosCandidatoDocente.Cprof_uf = ddDlCprof_Uf.SelectedValue.ToString();
            dadosCandidatoDocente.Cprof_dtexp = Convert.ToDateTime(dteCProfDtExp.Value);
            dadosCandidatoDocente.Categoria = txtCargo.Text;
            dadosCandidatoDocente.Pis_pasep = txtPISPASEP.Text;

            dtCandidatoDoc.Rows.Add(dadosCandidatoDocente);
        }

        private void PreencherDadosTela(LyCandidatoDocente dadosCandidato)
        {
            RN.ContratoTemporario.Cota rnCotaConsulta = new RN.ContratoTemporario.Cota();
            RN.CandidatoDocente rnCandidatoDocente = new Techne.Lyceum.RN.CandidatoDocente();

            DataTable dtCandidato = new DataTable();
            CarregarDrops();
            CarregarDadosDropSituacao(dadosCandidato.Status.ToString());

            dtCandidato = rnCandidatoDocente.ObtemDadosDocenteCandidatoPor(dadosCandidato.Concurso, dadosCandidato.Candidato);

            if (dtCandidato.Rows.Count > 0)
            {
                txtMatricula.Text = dtCandidato.Rows[0]["MATRICULA"].ToString();
                txtVinculo.Text = dtCandidato.Rows[0]["VINCULO"].ToString();
                txtIdentidadeFuncional.Text = dtCandidato.Rows[0]["IDFUNCIONAL"].ToString();
            }

            PreencherDadoCombo(ddlNecessidadeEspecial, dadosCandidato.NecessidadeEspecialId.ToString());
            PreencherDadoCombo(ddlEst_Civil, dadosCandidato.Estado_civil);
            PreencherDadoCombo(cmbRGEmissor, Convert.ToString(dadosCandidato.Rg_emissor));
            PreencherDadoCombo(ddlRGTipo, Convert.ToString(dadosCandidato.Rg_tipo));

            txtNome_Comp.Text = Convert.ToString(dadosCandidato.Nome);
            txtNomeMae.Text = dadosCandidato.Nome_mae;
            txtNomePai.Text = dadosCandidato.Nome_pai;
            txtRGUF.Text = Convert.ToString(dadosCandidato.Rg_uf);
            rblSexo.Text = dadosCandidato.Sexo;

            if (dadosCandidato.Dt_nasc.HasValue)
                dtDataNasc.Date = dadosCandidato.Dt_nasc.Value;

            if (dadosCandidato.Rg_num == null)
                txtRGNum.Text = string.Empty;
            else
                txtRGNum.Text = dadosCandidato.Rg_num;

            if (dadosCandidato.Rg_dtexp.HasValue)
                dtDataExped.Date = dadosCandidato.Rg_dtexp.Value;

            txtCPF.Text = !dadosCandidato.Cpf.IsNullOrEmptyOrWhiteSpace() ? dadosCandidato.Cpf.AplicarMascaraCPF() : string.Empty;

            //DADOS ENDEREÇO NASCIMENTO
            PreencherDadoCombo(ddlPaisNasc, dadosCandidato.Pais_nasc);
            PreencherDadoCombo(ddlNacionalidade, dadosCandidato.Nacionalidade);
            PreencherDadoCombo(ddlPaises, dadosCandidato.End_pais);

            if (!string.IsNullOrEmpty(dadosCandidato.Pais_nasc))
            {
                string descricaoPais = RN.Endereco.ObterPais(dadosCandidato.Pais_nasc);

                //verifica se existe valor para municipio
                if (!string.IsNullOrEmpty(dadosCandidato.Municipio_nasc))
                {
                    tseNaturalidade.Visible = true;
                    txtNaturalidadeNasc.Visible = false;
                    //preenche os dados nos controles da tela
                    tseNaturalidade.DBValue = dadosCandidato.Municipio_nasc;
                    tseNaturalidade.Mode = ControlMode.View;
                    //obtém a UF de acordo com o codigo do municipío
                    txtNaturalidadeUF.Value = RN.Endereco.ObterUFMunicipio(dadosCandidato.Municipio_nasc);
                }
                else
                {
                    tseNaturalidade.ResetValue();
                    txtNaturalidadeUF.Value = string.Empty;
                }
            }

            //DADOS ENDEREÇO
            txtEndereco.Text = dadosCandidato.Endereco;
            txtEndNum.Text = dadosCandidato.End_num;
            txtEndCompl.Text = dadosCandidato.End_compl;
            txtCep.Text = dadosCandidato.Cep;
            txtBairro.Text = dadosCandidato.Bairro;

            if (!string.IsNullOrEmpty(dadosCandidato.End_pais))
            {
                string descricaoPais = RN.Endereco.ObterPais(dadosCandidato.End_pais);

                //verifica se existe valor para municipio
                if (!string.IsNullOrEmpty(dadosCandidato.End_municipio))
                {
                    tseMunicipio.Visible = true;
                    txtMunicipio.Visible = false;
                    //preenche os dados nos controles da tela
                    tseMunicipio.DBValue = dadosCandidato.End_municipio;
                    tseMunicipio.Mode = ControlMode.View;
                    //obtém a UF de acordo com o codigo do municipío
                    txtEstado.Value = RN.Endereco.ObterUFMunicipio(dadosCandidato.End_municipio);
                }
                else
                {
                    tseMunicipio.ResetValue();
                    txtEstado.Value = string.Empty;
                }
            }

            txtEmail.Text = dadosCandidato.E_mail;
            int resul;
            if (int.TryParse(dadosCandidato.Fone, out resul))
                txtFone.Text = string.Format("{0:(00)0000-0000}", resul);
            else
                txtFone.Text = dadosCandidato.Fone.AplicarMascaraTelefoneComDDD();

            long resultado;
            if (long.TryParse(dadosCandidato.Celular, out resultado))
            {
                if (dadosCandidato.Celular.Length == 10)
                {
                    txtCelular.Text = string.Format("{0:(00)0000-0000}", resultado);
                }
                else
                {
                    txtCelular.Text = string.Format("{0:(00)00000-0000}", resultado);
                }
            }

            txtCProfNum.Text = dadosCandidato.Cprof_num;
            txtCProfSerie.Text = dadosCandidato.Cprof_serie;
            dteCProfDtExp.Value = dadosCandidato.Cprof_dtexp;
            PreencherDadoCombo(ddDlCprof_Uf, Convert.ToString(dadosCandidato.Cprof_uf));

            txtPISPASEP.Text = dadosCandidato.Pis_pasep;

            // Folha de pagamento
            cmbCargo.Items.Clear();
            PreencherFuncao(dadosCandidato.Concurso);
            tseDisciplina.SqlWhere = "CD.CONCURSO = '" + dadosCandidato.Concurso.ToString() + "' AND CD.CANDIDATO = '" + dadosCandidato.Candidato.ToString() + "'";

            if (dadosCandidato.Agrupamento_ingresso != null)
            {
                tseDisciplina.DBValue = dadosCandidato.Agrupamento_ingresso.ToString();
            }

            tseCoordenadoria.Value = dadosCandidato.Nucleo;
            tseCoordenadoria.Mode = ControlMode.View;
            tseMunicipioProc.SqlWhere = "CONCURSO = '" + dadosCandidato.Concurso.ToString() + "' AND CANDIDATO = '" + dadosCandidato.Candidato.ToString() + "'";
            tseMunicipioProc.DBValue = dadosCandidato.Municipio_proc != null ? Convert.ToString(dadosCandidato.Municipio_proc) : string.Empty;

            ListItem li = ddlCargaHoraria.Items.FindByValue(dadosCandidato.Carga_Horaria.ToString());
            if (li != null)
            {
                ddlCargaHoraria.SelectedValue = dadosCandidato.Carga_Horaria.ToString();
            }
            else
            {
                lblMensagem.Text = "Carga Horária com problema. Verifique!";
            }
            tseDisciplina.Value = dadosCandidato.Agrupamento_ingresso;
            tseDisciplina.Mode = ControlMode.View;
            tseCoordenadoria.Value = dadosCandidato.Nucleo;
            tseCoordenadoria.Mode = ControlMode.View;

            if (ddlCargaHoraria.SelectedIndex == 0)
                txtCargo.Text = string.Empty;
            else
                txtCargo.Text = dadosCandidato.Categoria;

            AtualizarUnidadeEnsinoPor(dadosCandidato.RegionalId);

            int cotaIdConvocaocaoAux;
            txtCota.Text = int.TryParse(dadosCandidato.CotaIdConvocacao.ToString(), out cotaIdConvocaocaoAux)
                ? rnCotaConsulta.ObterDescricaoPor(cotaIdConvocaocaoAux)
                : string.Empty;

            PreencherDadoCombo(ddlCorRaca, dadosCandidato.EtniaId.ToString());
        }

        private void AtualizarUnidadeEnsinoPor(int? regionalId)
        {
            tseUnidade_Ensino.ResetValue();

            if (regionalId.HasValue && regionalId > 0)
            {
                RN.Regional rnRegional = new Techne.Lyceum.RN.Regional();
                DataTable regionalAux = rnRegional.RetornarRegionalPor((int)regionalId);

                txtPolo.Text = regionalAux.Rows[0]["POLO"].ToString();
                txtDescricaoRegional.Text = regionalAux.Rows[0]["REGIONAL"].ToString();
                tblRegional.Visible = true;

                tseCoordenadoria.Visible = false;
                lblCoordenadoria.Text = "Regional:";

                tseUnidade_Ensino.SqlWhere =
                    "id_regional = " + regionalId.ToString() + " and municipio = '" + tseMunicipioProc["codigo"].ToString() + "'";
            }
            else
            {
                lblCoordenadoria.Text = "Coordenadoria:";
                tseUnidade_Ensino.SqlWhere = "nucleo = '" + tseInscricao["nucleo"].ToString() + "'";

                if (tseMunicipioProc.IsValidDBValue)
                    tseUnidade_Ensino.SqlWhere = tseUnidade_Ensino.SqlWhere + " and municipio = '" + tseMunicipioProc["codigo"].ToString() + "'";
            }

            tseUnidade_Ensino.DataBind();
        }

        private void PreencherDadosFolhaPagamento(DataTable dadosFolhaPagamento)
        {
            if (dadosFolhaPagamento.Rows[0]["UNIDADE_ENS"] != DBNull.Value )
            {
                int? regionalId = (!string.IsNullOrEmpty(txtPolo.Text)) ? (int?)Convert.ToInt32(txtPolo.Text) : null;

                AtualizarUnidadeEnsinoPor(regionalId);
                tseUnidade_Ensino.DBValue = Convert.ToString(dadosFolhaPagamento.Rows[0]["UNIDADE_ENS"]);
            }
            if (dadosFolhaPagamento.Rows[0]["BANCO"] != DBNull.Value)
            {
                tseBanco.DBValue = Convert.ToDecimal(dadosFolhaPagamento.Rows[0]["BANCO"]);
                tsAgencia.SqlWhere = "AGENCIAS.banco =" + tseBanco.DBValue.ToString();
                tsAgencia.DataBind();
            }
            if (dadosFolhaPagamento.Rows[0]["AGENCIA"] != DBNull.Value)
                tsAgencia.DBValue = Convert.ToString(dadosFolhaPagamento.Rows[0]["AGENCIA"]);

            if (dadosFolhaPagamento.Rows[0]["CONTA"] != DBNull.Value)
                txtConta.Text = Convert.ToString(dadosFolhaPagamento.Rows[0]["CONTA"]);

            if (dadosFolhaPagamento.Rows[0]["DT_ADMISSAO"] != DBNull.Value)
                dtAdmissao.Date = Convert.ToDateTime(dadosFolhaPagamento.Rows[0]["DT_ADMISSAO"].ToString());
        }

        private void PreencherDadosAcumulacao(string concurso, string candidato)
        {
            RN.RecursosHumanos.Acumulacao rnAcumulacao = new Techne.Lyceum.RN.RecursosHumanos.Acumulacao();
            RN.RecursosHumanos.Entidades.Acumulacao acumulacao = new Techne.Lyceum.RN.RecursosHumanos.Entidades.Acumulacao();

            try
            {
                acumulacao = rnAcumulacao.ObtemPor(concurso, candidato);
                if (acumulacao.AcumulacaoId > 0)
                {
                    rblAcumulacao.SelectedValue = "1";
                    rblAcumulacao_SelectedIndexChanged(null, null);
                    txtMatriculaAcumulacao.Text = acumulacao.MatriculaOrgao;
                    txtOrgaoAcumulacao.Text = acumulacao.Orgao;
                    txtNumProcessoAcumulacao.Text = acumulacao.NumeroProcesso;
                }
                else
                {
                    rblAcumulacao.SelectedValue = "0";
                    txtMatriculaAcumulacao.Text = string.Empty;
                    txtOrgaoAcumulacao.Text = string.Empty;
                    txtNumProcessoAcumulacao.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        /// <summary>
        /// Preenche o campo Função da ficha de inscrição usando a notação definida pela SEEDUC.
        /// DEFINIÇÂO: Nomenclatura utilizada pelo usuário do sistema para Função de docente
        /// DOC I = Professor para atuar nos anos finais do ensino fundamental e/ou ensino médio
        /// DOC II = Professor para atuar nos anos iniciais do ensino fundamental
        /// </summary>
        /// <param name="funcaoCandidato">Função</param>
        private void PreencherFuncao(string strConcurso)
        {
            DataTable dt = new DataTable();
            RN.Funcao rnFuncao = new Techne.Lyceum.RN.Funcao();

            dt = rnFuncao.RetornaFuncao(strConcurso);

            if (dt.Rows.Count > 0)
                cmbCargo.Items.Add(dt.Rows[0].ItemArray[1].ToString(), dt.Rows[0].ItemArray[0]);

            cmbCargo.SelectedIndex = 0;
        }

        private void CarregarDrops()
        {
            CarregarDadosDrop(ddlPaises.ID);
            CarregarDadosDrop(ddlEst_Civil.ID);
            CarregarDadosDrop(ddlNacionalidade.ID);
            CarregarDadosDrop(ddlPaisNasc.ID);
            CarregarDadosDrop(ddlCargaHoraria.ID);
            CarregarDadosDrop(ddlCorRaca.ID);
            CarregarDadosDrop(ddDlCprof_Uf.ID);
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

        private object CarregarDadosDrop(string idDrop)
        {
            QueryTable dadosDrop = null;

            try
            {
                switch (idDrop)
                {
                    case "ddlEst_Civil":
                        {
                            dadosDrop = RN.Basico.ConsultaItemTabelaValDescr("Estado civil");
                            CarregarDropDownList(ddlEst_Civil, dadosDrop, "");
                            break;
                        }                 
                    case "ddlPaisNasc":
                        {
                            dadosDrop = RN.Basico.ConsultarPais();
                            CarregarDropDownList(ddlPaisNasc, dadosDrop, "");
                            break;
                        }
                    case "ddlNacionalidade":
                        {
                            dadosDrop = RN.Basico.ConsultarNacionalidade();
                            CarregarDropDownList(ddlNacionalidade, dadosDrop, "");
                            break;
                        }
                    case "ddlCargaHoraria":
                        {
                            dadosDrop = RN.ProcessoSeletivo.ConsultarCargaHoraria(tseConcurso.DBValue.ToString());
                            if (dadosDrop.Rows.Count > 0)
                            {
                                CarregarCargaHoraria(ddlCargaHoraria, Convert.ToInt32(dadosDrop.Rows[0]["CH_SEMANAL_EFETIVA"]));
                            }
                            break;
                        }
                    case "ddlPaises":
                        {
                            dadosDrop = RN.Basico.ConsultarPais();
                            CarregarDropDownList(ddlPaises, dadosDrop, "");
                            break;
                        }
                    case "ddlCorRaca":
                        {
                            RN.Etnia rnEtnia = new RN.Etnia();
                            dadosDrop = rnEtnia.ConsultarEtnia();
                            CarregarDropDownList(ddlCorRaca, dadosDrop, "");
                            break;
                        }
                    case "ddDlCprof_Uf":
                        dadosDrop = RN.Basico.ConsultarUF();
                        PreencheDropdownList(ddDlCprof_Uf, dadosDrop, "");
                        break;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                dadosDrop = null;
            }
            return dadosDrop;
        }

        private void CarregarDropDownList(DropDownList drop, object data, string defaultValue)
        {
            drop.SelectedIndex = -1;
            drop.Items.Clear();
            drop.SelectedValue = null;
            drop.DataSource = data;
            drop.DataBind();
            drop.Items.Insert(0, new ListItem("Selecione", string.Empty));

            if (_tipoOperacao.Equals(TipoOperacao.Novo))
            {
                drop.SelectedValue = "";
                if (drop == ddlPaises || drop == ddlPaisNasc)
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

        private void CarregarDadosDropSituacao(string statusCandidato)
        {
            try
            {
                RN.ProcessoSeletivo rnProcessoSeletivo = new Techne.Lyceum.RN.ProcessoSeletivo();

                if (_tipoOperacao == TipoOperacao.Novo || _tipoOperacao == TipoOperacao.Consultar)
                {
                    ddlSituacao.DataSource = RN.ProcessoSeletivo.RetornaSituacaoPropostaContrato();
                }
                else if (_tipoOperacao == TipoOperacao.Alterar)
                {
                    ddlSituacao.DataSource = rnProcessoSeletivo.CarregaSituacaoPropostaContrato();
                }
                ddlSituacao.DataValueField = "statusid";
                ddlSituacao.DataTextField = "descricao";
                ddlSituacao.DataBind();

                if (ddlSituacao.Items.FindByValue(statusCandidato.ToString()) != null)
                    ddlSituacao.SelectedValue = statusCandidato.ToString();
                else
                {
                    ddlSituacao.SelectedIndex = 0;
                }
            }
            catch
            {
                throw;
            }
        }

        private void CarregarCargaHoraria(DropDownList drop, int valor)
        {
            drop.Items.Clear();

            for (int i = 1; i <= valor; i++)
            {
                drop.Items.Add(i.ToString());
            }

            drop.DataBind();
            drop.Items.Insert(0, new ListItem("Selecione", string.Empty));
            drop.SelectedIndex = 0;
        }

        private void PreencheDropdownList(DropDownList lista, object dados, string valorPadrao)
        {
            lista.SelectedIndex = -1;
            lista.Items.Clear();
            lista.SelectedValue = null;
            lista.DataSource = dados;
            lista.DataBind();
            ListItem itemVazio = new ListItem("Selecione", string.Empty);
            lista.Items.Insert(0, itemVazio);

            if (_tipoOperacao.Equals(TipoOperacao.Novo))
            {
                if (lista == ddlNacionalidade)
                {
                    ListItem listItem = lista.Items.FindByText("BRASILEIRA");
                    if (listItem != null)
                    {
                        lista.ClearSelection();
                        listItem.Selected = true;
                    }
                }
            }

            if (valorPadrao != "")
            {
                ListItem itemPadrao = lista.Items.FindByText(valorPadrao);
                if (itemPadrao != null)
                {
                    lista.ClearSelection();
                    itemPadrao.Selected = true;
                }
                else
                {
                    lista.Items.Add(new ListItem(valorPadrao, string.Empty));
                }
            }
        }
        private void LimparTela()
        {
            txtMatricula.Text = string.Empty;
            txtIdentidadeFuncional.Text = string.Empty;
            txtVinculo.Text = string.Empty;
            txtNome_Comp.Text = string.Empty;
            dtDataNasc.Text = string.Empty;
            rblSexo.ClearSelection();
            txtNomeMae.Text = string.Empty;
            txtNomePai.Text = string.Empty;
            txtEstado.Value = string.Empty;
            txtCep.Text = string.Empty;
            txtEndereco.Text = string.Empty;
            txtEndNum.Text = string.Empty;
            txtEndCompl.Text = string.Empty;
            txtBairro.Text = string.Empty;
            txtFone.Text = string.Empty;
            txtCelular.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtCPF.Text = string.Empty;
            txtRGNum.Text = string.Empty;
            txtRGUF.Text = string.Empty;
            dtDataExped.Text = string.Empty;
            txtPISPASEP.Text = string.Empty;
            txtCProfNum.Text = string.Empty;
            txtCProfSerie.Text = string.Empty;
            dteCProfDtExp.Text = string.Empty;
            txtMatriculaAcumulacao.Text = string.Empty;
            txtNumProcessoAcumulacao.Text = string.Empty;
            txtOrgaoAcumulacao.Text = string.Empty;
            txtCota.Text = string.Empty;
            txtCargo.Text = string.Empty;
            txtConta.Text = string.Empty;
            dtAdmissao.Text = string.Empty;

            ddlCorRaca.ClearSelection();
            ddlNecessidadeEspecial.ClearSelection();
            ddlPaises.ClearSelection();
            ddlEst_Civil.ClearSelection();
            ddDlCprof_Uf.ClearSelection();
            ddlCargaHoraria.ClearSelection();
            ddlNacionalidade.ClearSelection();
            ddlPaisNasc.ClearSelection();
            ddlRGTipo.ClearSelection();
            ddlSituacao.ClearSelection();
            txtDescricaoRegional.Text = string.Empty;
            txtPolo.Text = string.Empty;
            tseBanco.ResetValue();
            tseDisciplina.ResetValue();
            tseMunicipio.ResetValue();
            tseMunicipioProc.ResetValue();
            tseNaturalidade.ResetValue();
            tseUnidade_Ensino.ResetValue();
        }
    }
}