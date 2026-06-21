using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Controls;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using System.Collections.Generic;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.Net.ProcessoSeletivo
{
    [
     NavUrl("~/ProcessoSeletivo/AlteracaoSituacaoCandidatos.aspx"),
      ControlText("AlteracaoSituacaoCandidatos"),
      Title("Alteração da Situação de Candidatos"),
    ]

    public partial class AlteracaoSituacaoCandidatos : TPage
    {
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

        #region Propriedades e Enum

        public enum TipoOperacao
        {
            Alterar,
            Consultar,
            Inicial,
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
                lblMensagem.Text = string.Empty;
                bool bolEhContratoTempo = RN.PadroesDeAcessos.ConsultarPadacesContratoTempoPorUsuario(User.Identity.Name);

                if (!bolEhContratoTempo)
                {
                    int intRegional = RN.Usuarios.RetornarRegionalUsuario(User.Identity.Name);
                    tseInscricao.SqlWhere = tseInscricao.SqlWhere + " AND CD.REGIONALID = " + intRegional;
                }

                if (!IsPostBack)
                {
                    _tipoOperacao = TipoOperacao.Inicial;
                    ControlarTipoOperacao();
                }
                DesabilitarTSearchs();
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
                pcAlteracaoSituacaoCandidatos.Visible = false;
                RetirarVisibilidadeBotoes();
                if (!tseConcurso.IsValidDBValue || tseConcurso.DBValue.IsNull)
                {
                    lblMensagem.Text = "Favor consultar processo seletivo e número de inscrição.";
                    return;
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
                pcAlteracaoSituacaoCandidatos.Visible = false;
                RetirarVisibilidadeBotoes();
                if (tseInscricao.IsValidDBValue && !tseInscricao.DBValue.IsNull &&
                    (tseConcurso.IsValidDBValue && !tseConcurso.DBValue.IsNull))
                {
                    RN.ProcessoSeletivo.Status statusCandidato = RN.ProcessoSeletivo.ConsultarStatusCandidato(tseConcurso.DBValue.ToString(), tseInscricao.DBValue.ToString());
                    if (statusCandidato.Equals(RN.ProcessoSeletivo.Status.Inscrito) ||
                        statusCandidato.Equals(RN.ProcessoSeletivo.Status.Convocado) ||
                        statusCandidato.Equals(RN.ProcessoSeletivo.Status.Desistente) ||
                        statusCandidato.Equals(RN.ProcessoSeletivo.Status.Faltoso) ||
                        statusCandidato.Equals(RN.ProcessoSeletivo.Status.Aguardando) ||
                        statusCandidato.Equals(RN.ProcessoSeletivo.Status.Inabilitado))
                    {
                        _tipoOperacao = TipoOperacao.Consultar;
                        ControlarTipoOperacao();
                        lblMensagem.Text = string.Empty;
                    }
                    else
                        lblMensagem.Text = "Acesso restrito apenas a candidatos que estão aguardando, convocados, desistentes, faltosos e inabliitados.";
                }
                else if (!tseInscricao.DBValue.IsNull)
                {
                    lblMensagem.Text = "Favor consultar processo seletivo e número de inscrição.";
                }
                else
                {
                    lblMensagem.Text = "Favor consultar processo seletivo e número de inscrição.";
                }
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

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            RetirarVisibilidadeBotoes();

            try
            {
                if (_tipoOperacao.Equals(TipoOperacao.Alterar))
                {
                    if (ddlSituacao.SelectedItem.Text == RN.ProcessoSeletivo.Status.Aguardando.ToString())
                    {
                        RN.ProcessoSeletivo.AlterarSituacaoEDadosConvocacao(ViewState["concurso"].ToString(), ViewState["candidato"].ToString());
                    }
                    else
                    {
                        RN.ProcessoSeletivo.AlterarSituacaoCandidato(ViewState["concurso"].ToString(), ViewState["candidato"].ToString(), ddlSituacao.SelectedValue);
                    }

                    lblMensagem.Text = "Alteração realizada com sucesso.";
                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();
                }
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
                lblMensagem.Text = string.Empty;
                _tipoOperacao = TipoOperacao.Inicial;
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ControlarTipoOperacao()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        pcAlteracaoSituacaoCandidatos.Visible = false;
                        tseConcurso.Mode = ControlMode.Edit;
                        tseInscricao.Mode = ControlMode.Edit;
                        tseConcurso.ResetValue();
                        tseInscricao.ResetValue();
                        RetirarVisibilidadeBotoes();
                        divTitulo.Visible = false;
                        LimparTela();
                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        LimparTela();
                        ViewState["concurso"] = tseConcurso.DBValue.ToString();
                        ViewState["candidato"] = tseInscricao.DBValue.ToString();
                        LyCandidatoDocente dadosCandidato = RN.CandidatoDocente.ListarDadosCandidatoDocente(tseConcurso.DBValue.ToString(), tseInscricao.DBValue.ToString());
                        if (dadosCandidato != null)
                        {
                            divTitulo.Visible = true;
                            ImageButton[] controles = new ImageButton[] { btnEditar };
                            ControlarVisibilidadeControle(controles);
                            pcAlteracaoSituacaoCandidatos.Visible = true;
                            pcAlteracaoSituacaoCandidatos.ActiveTabIndex = 0;
                            PreencherDadosTela(dadosCandidato);
                            tseCoordenadoria.Mode = ControlMode.View;
                            tseDisciplina.Mode = ControlMode.View;
                            tseMunicipio.Mode = ControlMode.View;
                            tseNaturalidade.Mode = ControlMode.View;
                            tseMunicipioProc.Mode = ControlMode.View;
                            ddlSituacao.Enabled = false;
                        }
                        else
                        {
                            pcAlteracaoSituacaoCandidatos.Visible = false;
                            lblMensagem.Text = "Não existem dados do candidato selecionado.";
                            _tipoOperacao = TipoOperacao.Inicial;
                            ControlarTipoOperacao();
                        }
                        break;
                    }
                case TipoOperacao.Alterar:
                    {
                        lblMensagem.Text = string.Empty;
                        ImageButton[] controles = new ImageButton[] { btnCancelar, btnSalvar };
                        ControlarVisibilidadeControle(controles);
                        pcAlteracaoSituacaoCandidatos.Visible = true;
                        tseConcurso.Mode = ControlMode.View;
                        tseInscricao.Mode = ControlMode.View;
                        DesabilitarTSearchs();
                        ddlSituacao.Enabled = true;
                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { btnEditar };
                        ControlarVisibilidadeControle(controles);
                        pcAlteracaoSituacaoCandidatos.Visible = true;
                        tseConcurso.Mode = ControlMode.Edit;
                        tseInscricao.Mode = ControlMode.Edit;
                        DesabilitarTSearchs();
                        ddlSituacao.Enabled = false;
                        break;
                    }
            }
        }

        private void PreencherDadosTela(LyCandidatoDocente dadosCandidato)
        {
            CarregarDadosDrop(ddlEst_Civil.ID);
            CarregarDadosDrop(ddlNacionalidade.ID);
            CarregarDadosDrop(ddlPaisNasc.ID);
            CarregarDadosDrop(ddlPaises.ID);
            CarregarDadosDropSituacao(Convert.ToInt32(dadosCandidato.Status));
            CarregaNecessidadeEspecial();


            txtNome_Comp.Text = Convert.ToString(dadosCandidato.Nome);
            if (dadosCandidato.Dt_nasc.HasValue)
                dtDataNasc.Date = dadosCandidato.Dt_nasc.Value;
            rblSexo.Text = dadosCandidato.Sexo;
            txtNomeMae.Text = dadosCandidato.Nome_mae;
            txtNomePai.Text = dadosCandidato.Nome_pai;
            PreencherDadoCombo(ddlNecessidadeEspecial, dadosCandidato.NecessidadeEspecialId.ToString());
            PreencherDadoCombo(ddlEst_Civil, dadosCandidato.Estado_civil);
            txtRGUF.Text = Convert.ToString(dadosCandidato.Rg_uf);
            PreencherDadoCombo(cmbRGEmissor, Convert.ToString(dadosCandidato.Rg_emissor));
            PreencherDadoCombo(ddlRGTipo, Convert.ToString(dadosCandidato.Rg_tipo));
            if (dadosCandidato.Rg_num == null)
                txtRGNum.Text = string.Empty;
            else
                txtRGNum.Text = dadosCandidato.Rg_num;
            if (dadosCandidato.Rg_dtexp.HasValue)
                dtDataExped.Date = dadosCandidato.Rg_dtexp.Value;
            Int64 result;
            if (Int64.TryParse(dadosCandidato.Cpf, out result))
            {
                if (result != 0)
                    txtCPF.Text = string.Format(@"{0:000\.000\.000-00}", result);
                else
                    txtCPF.Text = "";
            }
            else
                txtCPF.Text = dadosCandidato.Cpf;

            PreencherDadoCombo(ddlPaisNasc, dadosCandidato.Pais_nasc);
            PreencherDadoCombo(ddlNacionalidade, dadosCandidato.Nacionalidade);
            if (!string.IsNullOrEmpty(dadosCandidato.Pais_nasc))
            {
                string descricaoPais = RN.Endereco.ObterPais(dadosCandidato.Pais_nasc);
                //verifica se valor não é Brasil
                if (descricaoPais.ToUpper() != "BRASIL")
                {
                    tseNaturalidade.Visible = false;
                    //obtém o municipio estrangeiro
                    SimpleRow sr = RN.Endereco.ObterMunicipioEstrangeiro(dadosCandidato.Municipio_nasc);
                    //verifica se a função retornou algum valor para a simplerow
                    if (sr != null)
                    {
                        //preenche os dados obtidos de municipio estrangeiro
                        if (!sr["nome"].IsNull)
                            txtNaturalidadeNasc.Text = Convert.ToString(sr["nome"]);

                        if (!sr["nome_estado"].IsNull)
                            txtNaturalidadeUF.Value = Convert.ToString(sr["nome_estado"]);
                    }
                }
                else //se for Brasil
                {
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
            }

            PreencherDadoCombo(ddlPaises, dadosCandidato.Pais_nasc);
            txtEndereco.Text = dadosCandidato.Endereco;
            txtEndNum.Text = dadosCandidato.End_num;
            txtEndCompl.Text = dadosCandidato.End_compl;
            txtCep.Text = dadosCandidato.Cep;
            txtBairro.Text = dadosCandidato.Bairro;
            if (!string.IsNullOrEmpty(dadosCandidato.End_pais))
            {
                string descricaoPais = RN.Endereco.ObterPais(dadosCandidato.End_pais);
                if (descricaoPais.ToUpper() != "BRASIL")
                {
                    tseMunicipio.Visible = false;
                    SimpleRow sr = RN.Endereco.ObterMunicipioEstrangeiro(dadosCandidato.End_municipio);
                    if (sr != null)
                    {
                        if (!sr["nome"].IsNull)
                            txtMunicipio.Text = Convert.ToString(sr["nome"]);
                        if (!sr["uf_sigla"].IsNull)
                            txtEstado.Value = Convert.ToString(sr["uf_sigla"]);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(dadosCandidato.End_municipio))
                    {
                        tseMunicipio.Visible = true;
                        txtMunicipio.Visible = false;
                        tseMunicipio.DBValue = dadosCandidato.End_municipio;
                        tseMunicipio.Mode = ControlMode.View;
                        txtEstado.Value = RN.Endereco.ObterUFMunicipio(dadosCandidato.End_municipio);
                    }
                    else
                    {
                        tseMunicipio.ResetValue();
                        txtEstado.Value = string.Empty;
                    }
                }
            }

            txtEmail.Text = dadosCandidato.E_mail;
            int resul;
            if (int.TryParse(dadosCandidato.Fone, out resul))
                txtFone.Text = string.Format("{0:(00)0000-0000}", resul);
            else
                txtFone.Text = dadosCandidato.Fone.AplicarMascaraTelefoneComDDD();
            if (int.TryParse(dadosCandidato.Celular, out resul))
                txtCelular.Text = string.Format("{0:(00)0000-0000}", resul);
            else
                txtCelular.Text = dadosCandidato.Celular.AplicarMascaraTelefoneComDDD();

            txtCProfNum.Text = dadosCandidato.Cprof_num;
            txtCProfSerie.Text = dadosCandidato.Cprof_serie;
            dteCProfDtExp.Value = dadosCandidato.Cprof_dtexp;
            txtCProfUF.Text = dadosCandidato.Cprof_uf;
            txtPISPASEP.Text = dadosCandidato.Pis_pasep;

            cmbCargo.Items.Clear();
            PreencherFuncao(dadosCandidato.Concurso);
            if (!dadosCandidato.Agrupamento_ingresso.IsNullOrEmptyOrWhiteSpace())
            {
                tseDisciplina.Value = dadosCandidato.Agrupamento_ingresso;
                tseDisciplina.Mode = ControlMode.View;
            }
            tseCoordenadoria.Value = dadosCandidato.Nucleo;
            tseCoordenadoria.Mode = ControlMode.View;
            tseMunicipioProc.Value = dadosCandidato.Municipio_proc;
            tseMunicipioProc.Mode = ControlMode.View;
            ddlSituacao.SelectedValue = ((RN.ProcessoSeletivo.Status)Enum.Parse(typeof(RN.ProcessoSeletivo.Status), dadosCandidato.Status.ToString())).ToDecimal().ToString();

        }

        private void PreencherFuncao(string concurso)
        {
            RN.Funcao rnFuncao = new RN.Funcao();

            System.Data.DataTable dt = rnFuncao.RetornaFuncao(concurso);

            foreach (var item in dt.Rows)
            {
                cmbCargo.Items.Add(dt.Rows[0]["descricao"].ToString(), dt.Rows[0]["codigo"].ToString());
            }

            cmbCargo.SelectedIndex = 0;
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
                    case "ddlPaises":
                        {
                            dadosDrop = RN.Basico.ConsultarPais();
                            CarregarDropDownList(ddlPaises, dadosDrop, "");
                            break;
                        }
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

        private void CarregarDadosDropSituacao(int statusCandidato)
        {
            ddlSituacao.Items.Clear();
            ddlSituacao.DataSource = RN.CandidatoDocente.ListarStatusCandidatoDocentePor(statusCandidato);
            ddlSituacao.DataBind();
        }

        private void CarregarDropDownList(DropDownList drop, object data, string defaultValue)
        {
            drop.SelectedIndex = -1;
            drop.Items.Clear();
            drop.SelectedValue = null;
            drop.DataSource = data;
            drop.DataBind();
            ListItem itemVazio = new ListItem("Selecione", string.Empty);
            drop.Items.Add(itemVazio);
        }

        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetirarVisibilidadeBotoes();
            foreach (ImageButton botao in botoes)
            {
                botao.Visible = true;
            }
        }

        private void RetirarVisibilidadeBotoes()
        {
            btnEditar.Visible = false;
            btnSalvar.Visible = false;
            btnCancelar.Visible = false;
        }

        protected void DesabilitarTSearchs()
        {
            tseCoordenadoria.Mode = ControlMode.View;
            tseDisciplina.Mode = ControlMode.View;
            tseMunicipio.Mode = ControlMode.View;
            tseNaturalidade.Mode = ControlMode.View;
            tseMunicipioProc.Mode = ControlMode.View;
        }
        private void LimparTela()
        {           
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
            txtCProfUF.Text = string.Empty;

            ddlNecessidadeEspecial.ClearSelection();
            ddlPaises.ClearSelection();
            ddlEst_Civil.ClearSelection();
            
            ddlNacionalidade.ClearSelection();
            ddlPaisNasc.ClearSelection();
            ddlRGTipo.ClearSelection();
            ddlSituacao.ClearSelection();
            tseDisciplina.ResetValue();
            tseMunicipio.ResetValue();
            tseMunicipioProc.ResetValue();
            tseNaturalidade.ResetValue();

        }
    }
}
