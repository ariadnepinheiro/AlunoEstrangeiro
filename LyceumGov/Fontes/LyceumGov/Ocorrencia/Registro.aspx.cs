using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Techne.Lyceum.RN.Entidades;
using Techne.Web;
using Techne.Data;
using Techne.Lyceum.CR;
using System.Collections.Generic;
using Techne.Lyceum.RN;
using System.Linq;
using DevExpress.Web.ASPxTabControl;
using System.Text;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using System.Data;
using Seeduc.Infra.Data;
using System.Configuration;
using Techne.Lyceum.RN.Util;
using Techne.Controls;

namespace Techne.Lyceum.Net.Ocorrencia
{

    [NavUrl("~/Ocorrencia/Registro.aspx"),
     ControlText("Registro"),
     Title("Registro"),]

    public partial class Registro : TPage
    {
        #region Propriedades e Enum
        public enum TipoOperacao
        {
            Novo,
            Desativar,
            Alterar,
            Consultar,
            Inicial,
            Sucesso,
            Arquivado
        }

        private TipoOperacao _tipoOperacao
        {
            get { return (TipoOperacao)ViewState["_tipoOperacao"]; }
            set { ViewState["_tipoOperacao"] = value; }
        }
        #endregion

        public object ListaDocumento(object id)
        {
            RN.Ocorrencias.ArquivoOcorrencia rnArquivoOcorrencia = new Techne.Lyceum.RN.Ocorrencias.ArquivoOcorrencia();

            if (id != null)
            {
                if (!string.IsNullOrEmpty(id.ToString()))
                {
                    return rnArquivoOcorrencia.ListaPor(Convert.ToInt32(id));
                }
            }
            return null;
        }

        public object ListaEncaminhamento(object id)
        {
            RN.Ocorrencias.OcorrenciaEncaminhamento rnOcorrenciaEncaminhamento = new Techne.Lyceum.RN.Ocorrencias.OcorrenciaEncaminhamento();

            if (id != null)
            {
                if (!string.IsNullOrEmpty(id.ToString()))
                {
                    return rnOcorrenciaEncaminhamento.ListaEncaminhamentoPor(Convert.ToInt32(id));
                }
            }
            return null;
        }

        public object ListaTratamento(object id)
        {
            RN.Ocorrencias.OcorrenciaTratamento rnOcorrenciaTratamento = new Techne.Lyceum.RN.Ocorrencias.OcorrenciaTratamento();

            if (id != null)
            {
                if (!string.IsNullOrEmpty(id.ToString()))
                {
                    return rnOcorrenciaTratamento.ObtemListaPor(Convert.ToInt32(id));
                }
            }
            return null;
        }

        public object ListaInterrupcao(object id)
        {
            RN.Ocorrencias.OcorrenciaInterrupcao rnOcorrenciaInterrupcao = new Techne.Lyceum.RN.Ocorrencias.OcorrenciaInterrupcao();

            if (id != null)
            {
                if (!string.IsNullOrEmpty(id.ToString()))
                {
                    return rnOcorrenciaInterrupcao.ListaInterrupcaoPor(Convert.ToInt32(id));
                }
            }
            return null;
        }

        public object ListaVitima(object id)
        {
            RN.Ocorrencias.Vitima rnVitima = new Techne.Lyceum.RN.Ocorrencias.Vitima();

            if (id != null)
            {
                if (!string.IsNullOrEmpty(id.ToString()))
                {
                    return rnVitima.ListaPor(Convert.ToInt32(id));
                }
            }
            return null;
        }

        public object ListaAcusado(object id)
        {
            RN.Ocorrencias.Acusado rnAcusado = new Techne.Lyceum.RN.Ocorrencias.Acusado();

            if (id != null)
            {
                if (!string.IsNullOrEmpty(id.ToString()))
                {
                    return rnAcusado.ListaPor(Convert.ToInt32(id));
                }
            }
            return null;
        }


        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdDocumento, "");
            TituloGrid(grdTratamento, "");
            TituloGrid(grdEncaminhamento, "");
            TituloGrid(grdInterrupcao, "");
            TituloGrid(grdAcusado, "");
            TituloGrid(grdVitima, "");
            TituloGrid(grdEncaminhamento, "");
            TituloGrid(grdInterrupcao, "Lista de interrupção");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdDocumento);
            ControlaAcesso(grdDocumento, AcaoControle.editar, "btnExcluirDocumento");
            ControlaAcesso(grdEncaminhamento);
            ControlaAcesso(grdEncaminhamento, AcaoControle.editar, "btnExcluirEncaminhamento");
            ControlaAcesso(grdTratamento, AcaoControle.editar, "btnExcluirTratamento");
            ControlaAcesso(grdInterrupcao, AcaoControle.editar, "btnExcluirInterrupcao");
            ControlaAcesso(grdAcusado, AcaoControle.editar, "btnExcluirAcusado");
            ControlaAcesso(grdVitima, AcaoControle.editar, "btnExcluirVitima");

            txtEncaminhamento.Visible = hdnPerfil.Value.IsNullOrEmptyOrWhiteSpace() || _tipoOperacao == TipoOperacao.Arquivado ? false : true;
            grdEncaminhamento.Columns[0].Visible = hdnPerfil.Value.IsNullOrEmptyOrWhiteSpace() || _tipoOperacao == TipoOperacao.Arquivado ? false : true;
            btnAdicionarEncaminhamento.Visible = hdnPerfil.Value.IsNullOrEmptyOrWhiteSpace() || _tipoOperacao == TipoOperacao.Arquivado ? false : true;
            FileUpload2.Visible = hdnPerfil.Value.IsNullOrEmptyOrWhiteSpace() || _tipoOperacao == TipoOperacao.Arquivado ? false : true;

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                RN.Ocorrencias.Ocorrencia rnOcorrencia = new Techne.Lyceum.RN.Ocorrencias.Ocorrencia();
                RN.Ocorrencias.OcorrenciaEncaminhamento rnOcorrenciaEncaminhamento = new Techne.Lyceum.RN.Ocorrencias.OcorrenciaEncaminhamento();
                RN.Perfil rnPerfil = new Perfil();
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    lblMensagemFinalizada.Text = string.Empty;
                    int id = 0;
                    Session["grid"] = null;
                    Session["tela"] = null;
                    lblMensagem.Text = string.Empty;
                    hdnCenso.Value = string.Empty;
                    hdnPerfil.Value = string.Empty;

                    _tipoOperacao = TipoOperacao.Inicial;

                    if (Request.QueryString.Keys.Count > 0)
                    {
                        string censo = string.Empty;
                        string unidade = string.Empty;
                        int ano = 0;

                        LimparDados();
                        LimparDadosFiltro();

                        byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                        string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

                        string[] listaDados = decodedText.Split('&');

                        foreach (string lista in listaDados)
                        {
                            if (lista.IndexOf("codigo") >= 0)
                                id = Convert.ToInt32(lista.Substring(lista.LastIndexOf('=') + 1));

                            if (lista.IndexOf("ano") >= 0)
                                ano = Convert.ToInt32(lista.Substring(lista.LastIndexOf('=') + 1));

                            if (lista.IndexOf("censo") >= 0)
                                censo = lista.Substring(lista.LastIndexOf('=') + 1);

                            if (lista.IndexOf("unidade") >= 0)
                                unidade = lista.Substring(lista.LastIndexOf('=') + 1);

                            if (lista.IndexOf("regional") >= 0)
                                lblRegional.Text = lista.Substring(lista.LastIndexOf('=') + 1);

                            if (lista.IndexOf("municipio") >= 0)
                                lblMunicipio.Text = lista.Substring(lista.LastIndexOf('=') + 1);

                            if (lista.IndexOf("bairro") >= 0)
                                lblBairro.Text = lista.Substring(lista.LastIndexOf('=') + 1);

                            if (lista.IndexOf("tela") >= 0)
                                Session["tela"] = lista.Substring(lista.LastIndexOf('=') + 1);

                            if (lista.IndexOf("tipoOperacao") >= 0)
                            {
                                string tipoOperacao = lista.Substring(lista.LastIndexOf('=') + 1);

                                if (tipoOperacao == "NOVO")
                                {
                                    _tipoOperacao = TipoOperacao.Novo;
                                    lblTipoOperacao.Text = "Modo de inclusão";

                                }
                                else if (tipoOperacao == "ALTERAR")
                                {
                                    _tipoOperacao = TipoOperacao.Alterar;
                                    lblTipoOperacao.Text = "Modo de alteração";
                                }

                                else if (tipoOperacao == "DESATIVAR")
                                {
                                    _tipoOperacao = TipoOperacao.Desativar;
                                    lblTipoOperacao.Text = "Modo de exclusão";
                                }

                                else if (tipoOperacao == "CONSULTAR")
                                {
                                    _tipoOperacao = TipoOperacao.Consultar;
                                    lblTipoOperacao.Text = "Modo de consulta";
                                }
                            }

                           
                        }

                        if (Convert.ToString(Session["tela"]) == "consulta")
                        {
                            hdnQueryString.Value = decodedText;
                            
                        }

                        lblEscola.Text = censo + " - " + unidade;
                        hdnCenso.Value = censo.ToString();

                        if (_tipoOperacao == TipoOperacao.Novo)
                        {
                            lblAno.Text = ano.ToString();
                        }

                        if (id > 0)
                        {
                            PreencheDadosTela(id);
                        }
                    }
                    else
                    {
                        Response.Redirect("ListarRegistro.aspx");
                    }

                    if (Session["tela"] == "consulta")
                    {
                        _tipoOperacao = TipoOperacao.Consultar;
                    }
                    else
                    {
                        var adm = rnPerfil.PossuiPerfilAdministradorRVEPor(User.Identity.Name);

                        hdnPerfil.Value = adm || RN.Usuarios.UsuarioPrivilegiado(HttpContext.Current.User.Identity.Name) ? "ADM" : string.Empty;

                        if (rnOcorrenciaEncaminhamento.PossuiEncaminhamentoPor(id) && hdnPerfil.Value.IsNullOrEmptyOrWhiteSpace())
                        {
                            _tipoOperacao = TipoOperacao.Consultar;
                            lblMensagemFinalizada.Text = "Registro encaminhado, bloqueado para edição.";
                        }

                        if (rnOcorrencia.PossuiArquivamentoPor(id))
                        {
                            _tipoOperacao = TipoOperacao.Arquivado;
                            lblMensagemFinalizada.Text = "Registro arquivado.";
                        }
                    }
                    ControlarTipoOperacao();
                }

            }
            catch (Exception ex)
            {

                lblMensagem.Text = ex.Message;
            }
        }

        private QueryTable CarregarDadosDrop(string idDrop)
        {
            QueryTable dadosDrop = null;

            try
            {
                switch (idDrop)
                {

                    case "ddlRGTipoPessoaVitima":
                        {
                            string param = "TIPO DOC";
                            dadosDrop = RN.Basico.ConsultaItemTabelaValDescr(param);
                            CarregarDropDownList(ddlRGTipoPessoaVitima, dadosDrop, "");
                            break;
                        }
                    case "ddlRGEmissorPessoaVitima":
                        {
                            string param = "ORGAO RG";
                            dadosDrop = RN.Basico.ConsultaItemTabelaValDescr(param);
                            CarregarDropDownList(ddlRGEmissorPessoaVitima, dadosDrop, "");
                            break;
                        }

                    case "ddlRGUFPessoaVitima":
                        {
                            dadosDrop = RN.Basico.ConsultarUF();
                            CarregarDropDownList(ddlRGUFPessoaVitima, dadosDrop, "");
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
            ListItem itemVazio = new ListItem("Selecione", "");
            drop.Items.Insert(0, itemVazio);

            if (_tipoOperacao.Equals(TipoOperacao.Novo))
            {
                drop.SelectedValue = "";
            }
        }

        private void LimparDadosFiltro()
        {
            lblEscola.Text = string.Empty;
            lblRegional.Text = string.Empty;
            lblMunicipio.Text = string.Empty;
            lblBairro.Text = string.Empty;
            lblAno.Text = string.Empty;
        }

        private void LimparDados()
        {
            hdnArquivoId.Value = string.Empty;
            hdnOcorrenciaId.Value = string.Empty;
            rblTipoVitima.ClearSelection();
            rblTipoAcusado.ClearSelection();
            dtDataOcorrencia.Text = string.Empty;
            tseClasse.ResetValue();
            tseSubClasse.ResetValue();
            tseMeio.ResetValue();
            tseDelegacia.ResetValue();
            tseBatalhao.ResetValue();
            tseTratamento.ResetValue();
            txtRO.Text = string.Empty;
            txtDescricao.Text = string.Empty;
            txtObservacao.Text = string.Empty;
            tseAlunoVitima.ResetValue();
            tseServidorVitima.ResetValue();
            txtNomeVitima.Text = string.Empty;
            dtNascimentoVitima.Text = string.Empty;
            txtCPFVitima.Text = string.Empty;
            ddlRGTipoPessoaVitima.ClearSelection();
            ddlRGUFPessoaVitima.ClearSelection();
            ddlRGEmissorPessoaVitima.ClearSelection();
            txtRGNumPessoaVitima.Text = string.Empty;
            dteRGDataExpPessoaVitima.Text = string.Empty;
            tseAlunoAcusado.ResetValue();
            tseServidorAcusado.ResetValue();
            txtNomeAcusado.Text = string.Empty;
            dtNascimentoAcusado.Text = string.Empty;
            txtCPFAcusado.Text = string.Empty;
            ddlRGTipoPessoaAcusado.ClearSelection();
            ddlRGUFPessoaAcusado.ClearSelection();
            ddlRGEmissorPessoaAcusado.ClearSelection();
            txtRGNumPessoaAcusado.Text = string.Empty;
            dteRGDataExpPessoaAcusado.Text = string.Empty;
            rblUsoArma.ClearSelection();
            chkUsoArma.ClearSelection();
            txtEncaminhamento.Text = string.Empty;
            lblCargoFuncaoAcusado.Text = string.Empty;
            lblCargoFuncaoVitima.Text = string.Empty;
            dtInterrupcao.Text = string.Empty;
            chkTurno.ClearSelection();
            ddlMotivoCancelamento.ClearSelection();
            lblIdadeAcusado.Text = string.Empty;
            lblIdadeVitima.Text = string.Empty;
        }

        private void DesabilitaCampos()
        {

            rblTipoVitima.Enabled = false;
            rblTipoAcusado.Enabled = false;
            dtDataOcorrencia.Enabled = false;
            tseClasse.Mode = ControlMode.View;
            tseSubClasse.Mode = ControlMode.View;
            tseMeio.Mode = ControlMode.View;
            tseDelegacia.Mode = ControlMode.View;
            tseBatalhao.Mode = ControlMode.View;
            tseTratamento.Mode = ControlMode.View;
            txtRO.Enabled = false;
            txtDescricao.Enabled = false;
            txtObservacao.Enabled = false;
            tseAlunoVitima.Enabled = false;
            tseServidorVitima.Enabled = false;
            txtNomeVitima.Enabled = false;
            txtCPFVitima.Enabled = false;
            ddlRGTipoPessoaVitima.Enabled = false;
            ddlRGUFPessoaVitima.Enabled = false;
            ddlRGEmissorPessoaVitima.Enabled = false;
            txtRGNumPessoaVitima.Enabled = false;
            dteRGDataExpPessoaVitima.Enabled = false;
            tseAlunoAcusado.Enabled = false;
            tseServidorAcusado.Enabled = false;
            txtNomeAcusado.Enabled = false;
            txtCPFAcusado.Enabled = false;
            ddlRGTipoPessoaAcusado.Enabled = false;
            ddlRGUFPessoaAcusado.Enabled = false;
            ddlRGEmissorPessoaAcusado.Enabled = false;
            txtRGNumPessoaAcusado.Enabled = false;
            dteRGDataExpPessoaAcusado.Enabled = false;
            rblUsoArma.Enabled = false;
            chkUsoArma.Enabled = false;
            rblInterrupcao.Enabled = false;
            dtInterrupcao.Enabled = false;
            chkTurno.Enabled = false;

        }

        private void HabilitaCampos()
        {

            rblTipoVitima.Enabled = true;
            rblTipoAcusado.Enabled = true;
            dtDataOcorrencia.Enabled = true;
            tseClasse.Mode = ControlMode.Edit;
            tseSubClasse.Mode = ControlMode.Edit;
            tseMeio.Mode = ControlMode.Edit;
            tseDelegacia.Mode = ControlMode.Edit;
            tseBatalhao.Mode = ControlMode.Edit;
            tseTratamento.Mode = ControlMode.Edit;
            txtRO.Enabled = true;
            txtDescricao.Enabled = true;
            txtObservacao.Enabled = true;
            tseAlunoVitima.Enabled = true;
            tseServidorVitima.Enabled = true;
            txtNomeVitima.Enabled = true;
            txtCPFVitima.Enabled = true;
            ddlRGTipoPessoaVitima.Enabled = true;
            ddlRGUFPessoaVitima.Enabled = true;
            ddlRGEmissorPessoaVitima.Enabled = true;
            txtRGNumPessoaVitima.Enabled = true;
            dteRGDataExpPessoaVitima.Enabled = true;
            tseAlunoAcusado.Enabled = true;
            tseServidorAcusado.Enabled = true;
            txtNomeAcusado.Enabled = true;
            txtCPFAcusado.Enabled = true;
            ddlRGTipoPessoaAcusado.Enabled = true;
            ddlRGUFPessoaAcusado.Enabled = true;
            ddlRGEmissorPessoaAcusado.Enabled = true;
            txtRGNumPessoaAcusado.Enabled = true;
            dteRGDataExpPessoaAcusado.Enabled = true;
            rblUsoArma.Enabled = true;
            chkUsoArma.Enabled = true;
            rblInterrupcao.Enabled = true;
            dtInterrupcao.Enabled = true;
            chkTurno.Enabled = true;
        }

        protected void rblUsoArma_IndexChanged(object sender, EventArgs e)
        {
            chkUsoArma.Visible = false;

            if (rblUsoArma.SelectedValue == "1")
            {
                chkUsoArma.Visible = true;
            }
            else
            {
                chkUsoArma.ClearSelection();
            }

        }

        private void PreencheDadosTela(int id)
        {
            try
            {
                RN.Ocorrencias.Ocorrencia rnOcorrencia = new Techne.Lyceum.RN.Ocorrencias.Ocorrencia();
                RN.DTOs.DadosOcorrencia dados = new Techne.Lyceum.RN.DTOs.DadosOcorrencia();
                RN.Pessoa rnPessoa = new Pessoa();
                if (id > 0)
                {
                    dados = rnOcorrencia.ObtemDadosOcorrenciaPor(id);

                    if (dados.OcorrenciaId > 0)
                    {
                        pnlAcusadoAluno.Visible = false;
                        pnlAcusadoOutro.Visible = false;
                        pnlAcusadoServidor.Visible = false;
                        pnlVitimaAluno.Visible = false;
                        pnlVitimaOutro.Visible = false;
                        pnlVitimaServidor.Visible = false;

                        hdnOcorrenciaId.Value = id.ToString();
                        lblAno.Text = dados.DataOcorrencia.Year.ToString();
                        lblEscola.Text = dados.Escola;
                        lblMunicipio.Text = dados.Municipio;
                        lblRegional.Text = dados.Regional;
                        lblBairro.Text = dados.Bairro;
                        dtDataOcorrencia.Date = dados.DataOcorrencia;
                        tseClasse.DBValue = dados.ClasseId;
                        if (dados.SubClasseId > 0)
                        {
                            tseSubClasse.DBValue = dados.SubClasseId;
                        }

                        rblUsoArma.SelectedValue = dados.UsoArma.Value ? "1" : "0";

                        if (rblUsoArma.SelectedValue == "1")
                        {
                            foreach (int tipo in dados.TiposArma)
                            {
                                if (tipo > 0)
                                {
                                    chkUsoArma.Items.FindByValue(tipo.ToString()).Selected = true;
                                }
                            }
                        }
                        rblUsoArma_IndexChanged(null, null);

                        tseMeio.DBValue = dados.MeioId;
                        if (dados.BatalhaoId != null)
                        {
                            tseBatalhao.DBValue = dados.BatalhaoId;
                        }
                        if (dados.DelegaciaId != null)
                        {
                            tseDelegacia.DBValue = dados.DelegaciaId;
                        }
                        txtRO.Text = dados.RegistroOcorrencia;
                        txtDescricao.Text = dados.Relato;
                        txtObservacao.Text = dados.Observacao;

                        if (dados.Interrupcao.Value == true)
                        {
                            rblInterrupcao.SelectedValue = "1";
                            pnlSimInterrupcao.Visible = true;
                        }
                        else
                        {
                            rblInterrupcao.SelectedValue = "0";
                            pnlSimInterrupcao.Visible = false;
                        }

                    }
                }

            }
            catch (Exception ex)
            {

                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (Convert.ToString(Session["tela"]) == "consulta")
                {
                    byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(hdnQueryString.Value);
                    Response.Redirect("Consulta.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
                }
                else
                {
                    byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(string.Empty);
                    Response.Redirect("ListarRegistro.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void btnNovaConsulta_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (Convert.ToString(Session["tela"]) == "consulta")
                {
                    Response.Redirect("Consulta.aspx");
                }
                else
                {
                    byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(string.Empty);
                    Response.Redirect("ListarRegistro.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
                }
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
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Ocorrencias.Ocorrencia rnOcorrencia = new Techne.Lyceum.RN.Ocorrencias.Ocorrencia();
                RN.Ocorrencias.Entidades.Ocorrencia ocorrencia = new Techne.Lyceum.RN.Ocorrencias.Entidades.Ocorrencia();

                var validItems = new List<int>();

                ocorrencia.OcorrenciaId = !hdnOcorrenciaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnOcorrenciaId.Value) : 0;
                ocorrencia.DataOcorrencia = dtDataOcorrencia.Date;
                ocorrencia.ClasseId = !tseClasse.DBValue.IsNull && tseClasse.IsValidDBValue ? Convert.ToInt32(tseClasse.DBValue) : -1;
                ocorrencia.SubClasseId = !tseSubClasse.DBValue.IsNull && tseSubClasse.IsValidDBValue ? Convert.ToInt32(tseSubClasse.DBValue) : -1;
                ocorrencia.MeioId = !tseMeio.DBValue.IsNull && tseMeio.IsValidDBValue ? Convert.ToInt32(tseMeio.DBValue) : -1;
                ocorrencia.DelegaciaId = !tseDelegacia.DBValue.IsNull && tseDelegacia.IsValidDBValue ? Convert.ToInt32(tseDelegacia.DBValue) : -1;
                ocorrencia.BatalhaoId = !tseBatalhao.DBValue.IsNull && tseBatalhao.IsValidDBValue ? Convert.ToInt32(tseBatalhao.DBValue) : -1;
                ocorrencia.RegistroOcorrencia = !txtRO.Text.IsNullOrEmptyOrWhiteSpace() ? txtRO.Text : null;
                ocorrencia.Relato = !txtDescricao.Text.IsNullOrEmptyOrWhiteSpace() ? txtDescricao.Text : null;
                ocorrencia.Observacao = !txtObservacao.Text.IsNullOrEmptyOrWhiteSpace() ? txtObservacao.Text : null;
                ocorrencia.Censo = !hdnCenso.Value.IsNullOrEmptyOrWhiteSpace() ? hdnCenso.Value : null;
                ocorrencia.UsuarioId = User.Identity.Name;
                ocorrencia.Ativo = true;

                ocorrencia.UsoArma = !rblUsoArma.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblUsoArma.SelectedValue == "1" ? true : false) : (bool?)null;
                
                List<int> listaTipoArma = new List<int>();

                foreach (ListItem item in chkUsoArma.Items)
                {
                    if (item.Selected)
                    {
                        listaTipoArma.Add(Convert.ToInt32(item.Value));
                    }
                }

                //if (listaTipoArma.Count > 0)
                //{
                //    ocorrencia.TiposArma = listaTipoArma;
                //}

                //VITIMA

                //ocorrencia.VitimaTipo = !rblTipoVitima.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(rblTipoVitima.SelectedValue) : -1;
                //ocorrencia.AcusadoTipo = !rblTipoAcusado.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(rblTipoAcusado.SelectedValue) : -1;

                //ocorrencia.VitimaPessoa = ocorrencia.VitimaTipo == 1 && !tseAlunoVitima.DBValue.IsNull && tseAlunoVitima.IsValidDBValue ? Convert.ToDecimal(tseAlunoVitima["pessoa"]) : ocorrencia.VitimaTipo == 2 && !tseServidorVitima.DBValue.IsNull && tseServidorVitima.IsValidDBValue ? Convert.ToDecimal(tseServidorVitima["pessoa"]) : -1;
                //ocorrencia.VitimaVinculo = ocorrencia.VitimaTipo == 2 && !tseServidorVitima.DBValue.IsNull && tseServidorVitima.IsValidDBValue ? (Convert.ToString(tseServidorVitima["vinculo"]).IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(tseServidorVitima["vinculo"])) : (int?)null;
                //ocorrencia.VitimaCargo = ocorrencia.VitimaTipo == 2 && !tseServidorVitima.DBValue.IsNull && tseServidorVitima.IsValidDBValue ? (Convert.ToString(tseServidorVitima["cargo"]).IsNullOrEmptyOrWhiteSpace() ? null : tseServidorVitima["cargo"].ToString()) : null;
                //ocorrencia.VitimaFuncao = ocorrencia.VitimaTipo == 2 && !tseServidorVitima.DBValue.IsNull && tseServidorVitima.IsValidDBValue ? (Convert.ToString(tseServidorVitima["funcao"]).IsNullOrEmptyOrWhiteSpace() ? null : tseServidorVitima["funcao"].ToString()) : null;
                //ocorrencia.VitimaAluno = ocorrencia.VitimaTipo == 1 && !tseAlunoVitima.DBValue.IsNull && tseAlunoVitima.IsValidDBValue ? tseAlunoVitima.DBValue.ToString() : null;
                //ocorrencia.VitimaNome = ocorrencia.VitimaTipo == 4 && !txtNomeVitima.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeVitima.Text.Trim() : null;
                //ocorrencia.VitimaCPF = ocorrencia.VitimaTipo == 4 && !txtCPFVitima.Text.IsNullOrEmptyOrWhiteSpace() ? txtCPFVitima.Text.RetirarMascaraCPF() : null;
                //ocorrencia.VitimaRgNumero = ocorrencia.VitimaTipo == 4 && !txtRGNumPessoaVitima.Text.IsNullOrEmptyOrWhiteSpace() ? txtRGNumPessoaVitima.Text.Trim() : null;
                //ocorrencia.VitimaRgTipo = ocorrencia.VitimaTipo == 4 && !ddlRGTipoPessoaVitima.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlRGTipoPessoaVitima.SelectedValue : null;
                //ocorrencia.VitimaRgUF = ocorrencia.VitimaTipo == 4 && !ddlRGUFPessoaVitima.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlRGUFPessoaVitima.SelectedValue : null;
                //ocorrencia.VitimaRgEmissor = ocorrencia.VitimaTipo == 4 && !ddlRGEmissorPessoaVitima.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlRGEmissorPessoaVitima.SelectedValue : null;
                //ocorrencia.VitimaRgDataExp = ocorrencia.VitimaTipo == 4 && !dteRGDataExpPessoaVitima.Text.IsNullOrEmptyOrWhiteSpace() ? dteRGDataExpPessoaVitima.Date : (DateTime?)null;
                //ocorrencia.VitimaDesconhecido = chkDesconhecidoVitimaAluno.Checked || chkDesconhecidoVitimaServidor.Checked;
                //ocorrencia.VitimaDataNascimento = ocorrencia.VitimaTipo == 4 && !dtNascimentoVitima.Text.IsNullOrEmptyOrWhiteSpace() ? dtNascimentoVitima.Date : (DateTime?)null;


                //ACUSADO

                //ocorrencia.AcusadoPessoa = ocorrencia.AcusadoTipo == 1 && !tseAlunoAcusado.DBValue.IsNull && tseAlunoAcusado.IsValidDBValue ? Convert.ToDecimal(tseAlunoAcusado["pessoa"]) : ocorrencia.AcusadoTipo == 2 && !tseServidorAcusado.DBValue.IsNull && tseServidorAcusado.IsValidDBValue ? Convert.ToDecimal(tseServidorAcusado["pessoa"]) : -1; ;
                //ocorrencia.AcusadoVinculo = ocorrencia.AcusadoTipo == 2 && !tseServidorAcusado.DBValue.IsNull && tseServidorAcusado.IsValidDBValue ? (Convert.ToString(tseServidorAcusado["vinculo"]).IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(tseServidorAcusado["vinculo"])) : (int?)null;
                //ocorrencia.AcusadoCargo = ocorrencia.AcusadoTipo == 2 && !tseServidorAcusado.DBValue.IsNull && tseServidorAcusado.IsValidDBValue ? (Convert.ToString(tseServidorAcusado["cargo"]).IsNullOrEmptyOrWhiteSpace() ? null : tseServidorAcusado["cargo"].ToString()) : null;
                //ocorrencia.AcusadoFuncao = ocorrencia.AcusadoTipo == 2 && !tseServidorAcusado.DBValue.IsNull && tseServidorAcusado.IsValidDBValue ? (Convert.ToString(tseServidorAcusado["funcao"]).IsNullOrEmptyOrWhiteSpace() ? null : tseServidorAcusado["funcao"].ToString()) : null;
                //ocorrencia.AcusadoAluno = ocorrencia.AcusadoTipo == 1 && !tseAlunoAcusado.DBValue.IsNull && tseAlunoAcusado.IsValidDBValue ? tseAlunoAcusado.DBValue.ToString() : null;
                //ocorrencia.AcusadoNome = ocorrencia.AcusadoTipo == 4 && !txtNomeAcusado.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeAcusado.Text.Trim() : null;
                //ocorrencia.AcusadoCPF = ocorrencia.AcusadoTipo == 4 && !txtCPFAcusado.Text.IsNullOrEmptyOrWhiteSpace() ? txtCPFAcusado.Text.RetirarMascaraCPF() : null;
                //ocorrencia.AcusadoRgNumero = ocorrencia.AcusadoTipo == 4 && !txtRGNumPessoaAcusado.Text.IsNullOrEmptyOrWhiteSpace() ? txtRGNumPessoaAcusado.Text.Trim() : null;
                //ocorrencia.AcusadoRgTipo = ocorrencia.AcusadoTipo == 4 && !ddlRGTipoPessoaAcusado.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlRGTipoPessoaAcusado.SelectedValue : null;
                //ocorrencia.AcusadoRgUF = ocorrencia.AcusadoTipo == 4 && !ddlRGUFPessoaAcusado.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlRGUFPessoaAcusado.SelectedValue : null;
                //ocorrencia.AcusadoRgEmissor = ocorrencia.AcusadoTipo == 4 && !ddlRGEmissorPessoaAcusado.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlRGEmissorPessoaAcusado.SelectedValue : null;
                //ocorrencia.AcusadoRgDataExp = ocorrencia.AcusadoTipo == 4 && !dteRGDataExpPessoaAcusado.Text.IsNullOrEmptyOrWhiteSpace() ? dteRGDataExpPessoaAcusado.Date : (DateTime?)null;
                //ocorrencia.AcusadoDesconhecido = chkDesconhecidoAcusadoAluno.Checked || chkDesconhecidoAcusadoServidor.Checked;
                //ocorrencia.AcusadoDataNascimento = ocorrencia.AcusadoTipo == 4 && !dtNascimentoAcusado.Text.IsNullOrEmptyOrWhiteSpace() ? dtNascimentoAcusado.Date : (DateTime?)null;

                //for (var rowIndex = 0; rowIndex < this.grdSelecionado.VisibleRowCount; rowIndex++)
                //{
                //    var id = Convert.ToInt32(grdSelecionado.GetRowValues(rowIndex, "TRATAMENTOID"));

                //    if (id > 0)
                //    {
                //        validItems.Add(id);
                //    }
                //}

                //if (validItems.Count > 0)
                //{
                //    ocorrencia.TratamentosId = validItems;
                //}

                validacao = rnOcorrencia.Valida(ocorrencia, listaTipoArma, ocorrencia.OcorrenciaId == 0 ? true : false);

                if (validacao.Valido)
                {
                    if (ocorrencia.OcorrenciaId == 0)
                    {
                        rnOcorrencia.Insere(ocorrencia, listaTipoArma);

                        hdnOcorrenciaId.Value = ocorrencia.OcorrenciaId.ToString();

                        odsTratamento.Select();
                        odsTratamento.DataBind();
                        grdTratamento.DataBind();
                        grdTratamento.Visible = true;
                        pnlDocumento.Visible = true;

                        lblMensagem.Text = "Registro criado com sucesso.";

                    }
                    else
                    {
                        rnOcorrencia.Atualiza(ocorrencia, listaTipoArma);
                        lblMensagem.Text = "Registro atualizado com sucesso.";
                    }
                    _tipoOperacao = TipoOperacao.Sucesso;
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }

                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnArquivar_Click(object sender, ImageClickEventArgs e)
        {
            lblMensagem.Text = string.Empty;
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Ocorrencias.Ocorrencia rnOcorrencia = new Techne.Lyceum.RN.Ocorrencias.Ocorrencia();
                RN.DTOs.DadosOcorrencia ocorrencia = new Techne.Lyceum.RN.DTOs.DadosOcorrencia();

                validacao = rnOcorrencia.ValidaArquivamento(Convert.ToInt32(hdnOcorrenciaId.Value), User.Identity.Name);

                if (validacao.Valido)
                {
                    rnOcorrencia.Arquiva(Convert.ToInt32(hdnOcorrenciaId.Value), User.Identity.Name);
                    lblMensagem.Text = "Registro arquivado com sucesso.";
                    _tipoOperacao = TipoOperacao.Arquivado;
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }
                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        protected void btnExcluirOcorrencia_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                CarregaMotivoCancelamento();

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopup();", true);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnVoltarCancelamento_Click(object sender, EventArgs e)
        {
            try
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "fecharPopup();", true);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnConfirmarCancelamento_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Ocorrencias.Ocorrencia rnOcorrencia = new Techne.Lyceum.RN.Ocorrencias.Ocorrencia();

                int motivo = !ddlMotivoCancelamento.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlMotivoCancelamento.SelectedValue) : -1;

                if (motivo != -1)
                {

                    rnOcorrencia.Desativa(Convert.ToInt32(hdnOcorrenciaId.Value), User.Identity.Name, motivo);

                    lblMensagem.Text = "Registro excluído com sucesso.";

                    byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(string.Empty);
                    Response.Redirect("ListarRegistro.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
                }
                else
                {
                    lblMensagemCancelamento.Text = "Para excluir uma ocorrência é necessário selecionar um motivo.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagemCancelamento.Text = ex.Message;
            }

        }
        protected void tseClasse_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                lblMensagem.Text = string.Empty;

                if (!tseClasse.DBValue.IsNull)
                {
                    if (!tseClasse.IsValidDBValue)
                    {
                        lblMensagem.Text = "Classe não cadastrada (favor verificar).";
                    }

                }
                else
                {

                    lblMensagem.Text = "Classe não cadastrada (favor verificar).";
                }


            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void tseSubClasse_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                lblMensagem.Text = string.Empty;

                if (!tseSubClasse.DBValue.IsNull)
                {
                    if (!tseSubClasse.IsValidDBValue)
                    {
                        this._tipoOperacao = TipoOperacao.Inicial;
                        lblMensagem.Text = "SubClasse não cadastrada (favor verificar).";
                    }
                    else
                    {
                        if (tseClasse.DBValue.IsNull)
                        {
                            tseClasse.DBValue = tseSubClasse["classeid"];
                        }
                    }
                }
                else
                {

                    lblMensagem.Text = "SubClasse não cadastrada (favor verificar).";
                }


            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseMeio_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                lblMensagem.Text = string.Empty;

                if (!tseMeio.DBValue.IsNull)
                {
                    if (!tseMeio.IsValidDBValue)
                    {

                        lblMensagem.Text = "Meio não cadastrado (favor verificar).";
                    }

                }
                else
                {

                    lblMensagem.Text = "Meio não cadastrado (favor verificar).";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseDelegacia_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                lblMensagem.Text = string.Empty;

                if (!tseDelegacia.DBValue.IsNull)
                {
                    if (!tseDelegacia.IsValidDBValue)
                    {

                        lblMensagem.Text = "Delegacia não cadastrada (favor verificar).";
                    }
                    else
                    {
                        if (!tseBatalhao.IsValidDBValue || tseBatalhao.DBValue.IsNull)
                        {
                            lblMensagem.Text = "Favor preencher o campo Batalhão.";
                            tseDelegacia.ResetValue();
                        }
                    }

                }
                else
                {

                    lblMensagem.Text = "Delegacia não cadastrada (favor verificar).";
                }


            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void tseBatalhao_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                lblMensagem.Text = string.Empty;

                if (!tseBatalhao.DBValue.IsNull)
                {
                    if (!tseBatalhao.IsValidDBValue)
                    {

                        lblMensagem.Text = "Batalhão não cadastrada (favor verificar).";
                    }

                }
                else
                {

                    lblMensagem.Text = "Batalhão não cadastrada (favor verificar).";
                }


            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseTratamento_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                lblMensagem.Text = string.Empty;

                if (!tseTratamento.DBValue.IsNull)
                {
                    if (!tseTratamento.IsValidDBValue)
                    {

                        lblMensagem.Text = "Tratamento não cadastrada (favor verificar).";
                    }

                }
                else
                {

                    lblMensagem.Text = "Tratamento não cadastrada (favor verificar).";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ControlarVisibilidadeControle(ImageButton[] imgBotoes)
        {
            RetiraVisibilidadeBotao();

            if (imgBotoes != null)
            {
                foreach (ImageButton botao in imgBotoes)
                {
                    botao.Visible = true;
                }
            }
            if (btnArquivar.Visible)
            {
                if (hdnPerfil.Value.IsNullOrEmptyOrWhiteSpace())
                {
                    btnArquivar.Visible = false;
                }


            }
            ControlaAcesso(btnSalvar, AcaoControle.editar);
            ControlaAcesso(btnExcluirOcorrencia, AcaoControle.excluir);
            ControlaAcesso(btnArquivar, AcaoControle.excluir);
        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnSalvar.Visible = false;
            btnExcluirOcorrencia.Visible = false;
            btnArquivar.Visible = false;
            btnNovaConsulta.Visible = false;
        }


        private void ControlarTSearchs()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {

                        break;
                    }
                case TipoOperacao.Sucesso:
                    {

                        break;
                    }

                case TipoOperacao.Novo:
                    {

                        break;
                    }

                case TipoOperacao.Alterar:
                    {


                        break;
                    }
                case TipoOperacao.Consultar:
                    {


                        break;
                    }
            }
        }

        private void ControlarTipoOperacao()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel };
                        ControlarVisibilidadeControle(controles);
                        LimparDados();

                        ControlarTSearchs();
                        CarregarDadosDrop("ddlRGEmissorPessoaAcusado");
                        CarregarDadosDrop("ddlRGEmissorPessoaVitima");
                        CarregarDadosDrop("ddlRGTipoPessoaVitima");
                        CarregarDadosDrop("ddlRGTipoPessoaAcusado");
                        CarregarDadosDrop("ddlRGUFPessoaAcusado");
                        CarregarDadosDrop("ddlRGUFPessoaVitima");
                        pnlAcusadoServidor.Visible = false;
                        pnlAcusadoOutro.Visible = false;
                        pnlAcusadoAluno.Visible = false;
                        pnlVitimaAluno.Visible = false;
                        pnlVitimaOutro.Visible = false;
                        pnlVitimaServidor.Visible = false;
                        grdTratamento.Visible = false;
                        pnlEncaminhamento.Visible = false;

                        break;
                    }
                case TipoOperacao.Novo:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel };
                        ControlarVisibilidadeControle(controles);
                        LimparDados();

                        pnlProsseguir.Visible = false;
                        pnlAcusadoServidor.Visible = false;
                        pnlAcusadoOutro.Visible = false;
                        pnlAcusadoAluno.Visible = false;
                        pnlVitimaAluno.Visible = false;
                        pnlVitimaOutro.Visible = false;
                        pnlVitimaServidor.Visible = false;
                        CarregarDadosDrop("ddlRGEmissorPessoaAcusado");
                        CarregarDadosDrop("ddlRGEmissorPessoaVitima");
                        CarregarDadosDrop("ddlRGTipoPessoaVitima");
                        CarregarDadosDrop("ddlRGUFPessoaAcusado");
                        CarregarDadosDrop("ddlRGUFPessoaAcusado");
                        CarregarDadosDrop("ddlRGUFPessoaVitima");
                        ControlarTSearchs();
                        HabilitaCampos();
                        grdDocumento.Columns[0].Visible = true;
                        btnAnexar.Visible = true;


                        grdTratamento.Visible = false;
                        pnlEncaminhamento.Visible = false;

                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnExcluirOcorrencia };

                        ControlarVisibilidadeControle(controles);
                        ControlarTSearchs();


                        grdTratamento.Visible = true;
                        grdTratamento.Columns[0].Visible = true;
                        break;
                    }

                case TipoOperacao.Alterar:
                    {

                        ImageButton[] controles = new ImageButton[] { btnCancel, btnExcluirOcorrencia, btnArquivar };
                        ControlarVisibilidadeControle(controles);

                        btnProsseguir.Text = "Salvar informações gerais";
                        grdTratamento.Visible = true;
                        pnlDocumento.Visible = true;
                        pnlProsseguir.Visible = true;
                        grdDocumento.Columns[0].Visible = true;
                        grdTratamento.Columns[0].Visible = true;
                        grdInterrupcao.Columns[0].Visible = true;
                        grdVitima.Columns[0].Visible = true;
                        grdAcusado.Columns[0].Visible = true;
                        pnlEncaminhamento.Visible = true;

                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        ImageButton[] controles;

                        if (Convert.ToString(Session["tela"]) == "consulta")
                        {
                            controles = new ImageButton[] { btnCancel, btnNovaConsulta };
                        }
                        else
                        {
                            controles = new ImageButton[] { btnCancel };
                        }
                        ControlarVisibilidadeControle(controles);

                        grdDocumento.Columns[0].Visible = false;
                        grdTratamento.Columns[0].Visible = false;
                        grdInterrupcao.Columns[0].Visible = false;
                        grdVitima.Columns[0].Visible = false;
                        grdAcusado.Columns[0].Visible = false;
                        btnArquivar.Visible = false;
                        btnAnexar.Visible = false;
                        FileUpload1.Visible = false;
                        DesabilitaCampos();
                        ControlarTSearchs();
                        btnAdicionar.Visible = false;
                        grdTratamento.Visible = true;
                        pnlDocumento.Visible = true;
                        pnlEncaminhamento.Visible = true;
                        pnlInterrupcao.Visible = true;
                        pnlProsseguir.Visible = true;
                        btnProsseguir.Visible = false;
                        btnAdicionarAcusado.Visible = false;
                        btnAdicionarInterrupcao.Visible = false;
                        btnAdicionarVitima.Visible = false;


                        break;
                    }

                case TipoOperacao.Desativar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnExcluirOcorrencia };
                        ControlarVisibilidadeControle(controles);

                        ControlarTSearchs();
                        break;
                    }

                case TipoOperacao.Arquivado:
                    {
                        ImageButton[] controles;

                        if (Convert.ToString(Session["tela"]) == "consulta")
                        {
                            controles = new ImageButton[] { btnCancel, btnNovaConsulta };
                        }
                        else
                        {
                            controles = new ImageButton[] { btnCancel };
                        }

                        ControlarVisibilidadeControle(controles);
                        grdDocumento.Columns[0].Visible = false;
                        grdTratamento.Columns[0].Visible = false;
                        grdInterrupcao.Columns[0].Visible = false;
                        grdVitima.Columns[0].Visible = false;
                        grdAcusado.Columns[0].Visible = false;
                        grdEncaminhamento.Columns[0].Visible = false;
                        btnArquivar.Visible = false;
                        btnAnexar.Visible = false;
                        FileUpload1.Visible = false;
                        FileUpload2.Visible = false;
                        txtEncaminhamento.Visible = false;
                        DesabilitaCampos();
                        ControlarTSearchs();
                        btnAdicionar.Visible = false;
                        grdTratamento.Visible = true;
                        pnlDocumento.Visible = true;
                        pnlEncaminhamento.Visible = true;
                        pnlInterrupcao.Visible = true;
                        pnlProsseguir.Visible = true;
                        btnProsseguir.Visible = false;
                        btnAdicionarAcusado.Visible = false;
                        btnAdicionarInterrupcao.Visible = false;
                        btnAdicionarVitima.Visible = false;
                        btnAdicionarEncaminhamento.Visible = false;
                        pnlSimInterrupcao.Visible = false;
                        break;
                    }
            }
        }

        protected void pcRegistro_TabClick(object source, TabControlCancelEventArgs e)
        {
            lblMensagem.Text = string.Empty;
        }

        protected void rblTipoVitima_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlVitimaServidor.Visible = false;
            pnlVitimaAluno.Visible = false;
            pnlVitimaOutro.Visible = false;

            tseAlunoVitima.ResetValue();
            tseServidorVitima.ResetValue();
            txtNomeVitima.Text = string.Empty;
            txtCPFVitima.Text = string.Empty;
            ddlRGTipoPessoaVitima.ClearSelection();
            ddlRGUFPessoaVitima.ClearSelection();
            ddlRGEmissorPessoaVitima.ClearSelection();
            txtRGNumPessoaVitima.Text = string.Empty;
            dteRGDataExpPessoaVitima.Text = string.Empty;

            if (rblTipoVitima.SelectedValue == "1")
            {
                pnlVitimaAluno.Visible = true;
            }

            if (rblTipoVitima.SelectedValue == "2")
            {
                pnlVitimaServidor.Visible = true;
            }

            if (rblTipoVitima.SelectedValue == "4")
            {
                pnlVitimaOutro.Visible = true;

            }

        }

        protected void rblTipoAcusado_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlAcusadoServidor.Visible = false;
            pnlAcusadoAluno.Visible = false;
            pnlAcusadoOutro.Visible = false;
            tseAlunoAcusado.ResetValue();
            tseServidorAcusado.ResetValue();
            txtNomeAcusado.Text = string.Empty;
            txtCPFAcusado.Text = string.Empty;
            ddlRGTipoPessoaAcusado.ClearSelection();
            ddlRGUFPessoaAcusado.ClearSelection();
            ddlRGEmissorPessoaAcusado.ClearSelection();
            txtRGNumPessoaAcusado.Text = string.Empty;
            dteRGDataExpPessoaAcusado.Text = string.Empty;

            if (rblTipoAcusado.SelectedValue == "1")
            {
                pnlAcusadoAluno.Visible = true;
            }

            if (rblTipoAcusado.SelectedValue == "2")
            {
                pnlAcusadoServidor.Visible = true;
            }

            if (rblTipoAcusado.SelectedValue == "4")
            {
                pnlAcusadoOutro.Visible = true;
            }
        }

        protected void tseAlunoVitima_Changed(object sender, EventArgs args)
        {
            try
            {
                int idade = 0;
                lblIdadeVitima.Text = string.Empty;
                pnlVitimaOutro.Visible = false;
                pnlVitimaServidor.Visible = false;

                if (Page.IsCallback)
                {
                    return;
                }

                if (!tseAlunoVitima.DBValue.IsNull)
                {
                    if (tseAlunoVitima.IsValidDBValue)
                    {
                        lblMensagem.Text = string.Empty;
                        idade = Utils.CalcularIdade(Convert.ToDateTime(tseAlunoVitima["dt_nascimento"]));
                        lblIdadeVitima.Text = idade.ToString() + " anos";
                    }
                    else
                    {
                        lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseAlunoAcusado_Changed(object sender, EventArgs args)
        {
            try
            {
                int idade = 0;
                lblIdadeAcusado.Text = string.Empty;
                pnlAcusadoOutro.Visible = false;
                pnlAcusadoServidor.Visible = false;

                if (Page.IsCallback)
                {
                    return;
                }

                if (!tseAlunoAcusado.DBValue.IsNull)
                {
                    if (tseAlunoAcusado.IsValidDBValue)
                    {
                        lblMensagem.Text = string.Empty;

                        idade = Utils.CalcularIdade(Convert.ToDateTime(tseAlunoAcusado["dt_nascimento"]));
                        lblIdadeAcusado.Text = idade.ToString() + " anos";
                    }
                    else
                    {
                        lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseServidorVitima_Changed(object sender, EventArgs args)
        {
            try
            {
                pnlVitimaAluno.Visible = false;
                pnlVitimaOutro.Visible = false;
                lblCargoFuncaoVitima.Text = string.Empty;

                if (Page.IsCallback)
                {
                    return;
                }

                if (!tseServidorVitima.DBValue.IsNull)
                {
                    if (tseServidorVitima.IsValidDBValue)
                    {
                        lblCargoFuncaoVitima.Text = tseServidorVitima["Cargo"].ToString() + " / " + tseServidorVitima["nomefuncao"].ToString();
                        lblMensagem.Text = string.Empty;
                    }
                    else
                    {
                        lblMensagem.Text = "Servidor não ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Servidor não ativo ou não cadastrado (favor verificar).";
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseServidorAcusado_Changed(object sender, EventArgs args)
        {
            try
            {
                pnlAcusadoAluno.Visible = false;
                pnlAcusadoOutro.Visible = false;
                lblCargoFuncaoAcusado.Text = string.Empty;

                if (Page.IsCallback)
                {
                    return;
                }

                if (!tseServidorAcusado.DBValue.IsNull)
                {
                    if (tseServidorAcusado.IsValidDBValue)
                    {
                        lblCargoFuncaoAcusado.Text = tseServidorAcusado["Cargo"].ToString() + " / " + tseServidorAcusado["nomefuncao"].ToString();
                        lblMensagem.Text = string.Empty;

                    }
                    else
                    {
                        lblMensagem.Text = "Servidor não ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Servidor não ativo ou não cadastrado (favor verificar).";
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlRGTipoPessoaVitima_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRGTipoPessoaVitima.SelectedValue == "RG")
            {
                ddlRGUFPessoaVitima.Enabled = true;
                lblRGUFPessoaVitima.Text = "Estado*: ";
                lblRGUFPessoaVitima.Font.Bold = true;

                dteRGDataExpPessoaVitima.Enabled = true;
                lblRGDataExpPessoaVitima.Text = "Data de Expedição*: ";
                lblRGDataExpPessoaVitima.Font.Bold = true;
            }
            else
            {
                ddlRGUFPessoaVitima.Enabled = false;
                lblRGUFPessoaVitima.Text = "Estado: ";
                lblRGUFPessoaVitima.Font.Bold = false;

                dteRGDataExpPessoaVitima.Enabled = false;
                lblRGDataExpPessoaVitima.Text = "Data de Expedição: ";
                lblRGDataExpPessoaVitima.Font.Bold = false;
            }
        }


        protected void ddlRGTipoPessoaAcusado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlRGTipoPessoaAcusado.SelectedValue == "RG")
            {
                ddlRGUFPessoaAcusado.Enabled = true;
                lblRGUFPessoaAcusado.Text = "Estado*: ";
                lblRGUFPessoaAcusado.Font.Bold = true;

                dteRGDataExpPessoaAcusado.Enabled = true;
                lblRGDataExpPessoaAcusado.Text = "Data de Expedição: ";
                lblRGDataExpPessoaAcusado.Font.Bold = true;
            }
            else
            {
                ddlRGUFPessoaAcusado.Enabled = false;
                lblRGUFPessoaAcusado.Text = "Estado: ";
                lblRGUFPessoaAcusado.Font.Bold = false;

                dteRGDataExpPessoaAcusado.Enabled = false;
                lblRGDataExpPessoaAcusado.Text = "Data de Expedição: ";
                lblRGDataExpPessoaAcusado.Font.Bold = false;
            }
        }
        protected void chkDesconhecidoVitima_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkDesconhecidoVitima.Checked)
                {
                    txtNomeVitima.Text = "DESCONHECIDA";
                    txtNomeVitima.Enabled = false;
                    txtCPFVitima.Text = string.Empty;
                    txtCPFVitima.Enabled = false;
                    txtRGNumPessoaVitima.Text = string.Empty;
                    txtRGNumPessoaVitima.Enabled = false;
                    ddlRGEmissorPessoaVitima.Enabled = false;
                    ddlRGEmissorPessoaVitima.ClearSelection();
                    ddlRGTipoPessoaVitima.ClearSelection();
                    ddlRGTipoPessoaVitima.Enabled = false;
                    ddlRGUFPessoaVitima.Enabled = false;
                    ddlRGUFPessoaVitima.ClearSelection();

                }
                else
                {
                    txtNomeVitima.Text = string.Empty;
                    txtNomeVitima.Enabled = true;
                    txtCPFVitima.Text = string.Empty;
                    txtCPFVitima.Enabled = true;
                    txtRGNumPessoaVitima.Text = string.Empty;
                    txtRGNumPessoaVitima.Enabled = true;
                    ddlRGEmissorPessoaVitima.Enabled = true;
                    ddlRGEmissorPessoaVitima.ClearSelection();
                    ddlRGTipoPessoaVitima.ClearSelection();
                    ddlRGTipoPessoaVitima.Enabled = true;
                    ddlRGUFPessoaVitima.Enabled = true;
                    ddlRGUFPessoaVitima.ClearSelection();

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkDesconhecidoVitimaAluno_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                tseAlunoVitima.Enabled = true;
                lblIdadeVitima.Text = string.Empty;

                if (chkDesconhecidoVitimaAluno.Checked)
                {
                    tseAlunoVitima.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkDesconhecidoVitimaServidor_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                tseServidorVitima.Enabled = true;
                lblCargoFuncaoVitima.Text = string.Empty;

                if (chkDesconhecidoVitimaServidor.Checked)
                {
                    tseServidorVitima.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkDesconhecidoAcusadoAluno_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                tseAlunoAcusado.Enabled = true;
                lblIdadeAcusado.Text = string.Empty;

                if (chkDesconhecidoAcusadoAluno.Checked)
                {
                    tseAlunoAcusado.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkDesconhecidoAcusadoServidor_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                tseServidorAcusado.Enabled = true;
                lblCargoFuncaoAcusado.Text = string.Empty;

                if (chkDesconhecidoAcusadoServidor.Checked)
                {
                    tseServidorAcusado.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkDesconhecidoAcusado_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkDesconhecidoAcusado.Checked)
                {
                    txtNomeAcusado.Text = "DESCONHECIDO";
                    txtNomeAcusado.Enabled = false;
                    txtCPFAcusado.Text = string.Empty;
                    txtCPFAcusado.Enabled = false;
                    txtRGNumPessoaAcusado.Text = string.Empty;
                    txtRGNumPessoaAcusado.Enabled = false;
                    ddlRGEmissorPessoaAcusado.Enabled = false;
                    ddlRGEmissorPessoaAcusado.ClearSelection();
                    ddlRGTipoPessoaAcusado.ClearSelection();
                    ddlRGTipoPessoaAcusado.Enabled = false;
                    ddlRGUFPessoaAcusado.Enabled = false;
                    ddlRGUFPessoaAcusado.ClearSelection();

                }
                else
                {
                    txtNomeAcusado.Text = string.Empty;
                    txtNomeAcusado.Enabled = true;
                    txtCPFAcusado.Text = string.Empty;
                    txtCPFAcusado.Enabled = true;
                    txtRGNumPessoaAcusado.Text = string.Empty;
                    txtRGNumPessoaAcusado.Enabled = true;
                    ddlRGEmissorPessoaAcusado.Enabled = true;
                    ddlRGEmissorPessoaAcusado.ClearSelection();
                    ddlRGTipoPessoaAcusado.ClearSelection();
                    ddlRGTipoPessoaAcusado.Enabled = true;
                    ddlRGUFPessoaAcusado.Enabled = true;
                    ddlRGUFPessoaAcusado.ClearSelection();

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public void Delete(object ARQUIVOOCORRENCIAID) { }

        protected void grdDocumento_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdDocumento);
            ControlaAcesso(grdDocumento, AcaoControle.editar, "btnExcluirDocumento");
        }

        protected void grdDocumento_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            try
            {

                if (e.ButtonID == "btnExcluirDocumento")
                {
                    ValidacaoDados validacao = new ValidacaoDados();
                    RN.Ocorrencias.ArquivoOcorrencia rnArquivoOcorrencia = new Techne.Lyceum.RN.Ocorrencias.ArquivoOcorrencia();
                    int id = Convert.ToInt32(grdDocumento.GetRowValues(e.VisibleIndex, "ARQUIVOOCORRENCIAID"));

                    validacao = rnArquivoOcorrencia.ValidaRemocao(id, User.Identity.Name);

                    if (validacao.Valido)
                    {
                        rnArquivoOcorrencia.Remove(id, User.Identity.Name);
                        grdDocumento.DataBind();
                        repCarrossel.DataBind();

                        lblMensagem.Text = "Documento excluído com sucesso.";
                    }
                    else
                    {
                        lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;

            }

        }

        protected void repCarrossel_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            if (new ListItemType[] { ListItemType.Item, ListItemType.AlternatingItem }.Contains(e.Item.ItemType))
            {
                var tipoArquivo = ((System.Data.DataRowView)e.Item.DataItem)["TipoArquivo"].ToString();
                ((PlaceHolder)e.Item.FindControl("plaTipoPDF")).Visible = (tipoArquivo == "application/pdf");
                ((PlaceHolder)e.Item.FindControl("plaTipoImagem")).Visible = (tipoArquivo.StartsWith("image/"));
                ((PlaceHolder)e.Item.FindControl("plaSemArquivo")).Visible = (tipoArquivo != "application/pdf" && !tipoArquivo.StartsWith("image/"));
            }
        }

        protected void btnExcluirDoc_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Ocorrencias.ArquivoOcorrencia rnArquivoOcorrencia = new Techne.Lyceum.RN.Ocorrencias.ArquivoOcorrencia();
                int id = Convert.ToInt32(hdnArquivoId.Value);

                validacao = rnArquivoOcorrencia.ValidaRemocao(id, User.Identity.Name);

                if (validacao.Valido)
                {
                    rnArquivoOcorrencia.Remove(id, User.Identity.Name);
                    grdDocumento.DataBind();
                    repCarrossel.DataBind();

                    lblMensagem.Text = "Documento excluído com sucesso.";
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

        protected void btnAnexar_Click(object sender, EventArgs e)
        {
            try
            {
                RN.Ocorrencias.ArquivoOcorrencia rnArquivoOcorrencia = new Techne.Lyceum.RN.Ocorrencias.ArquivoOcorrencia();
                RN.Ocorrencias.Entidades.ArquivoOcorrencia arquivo = new Techne.Lyceum.RN.Ocorrencias.Entidades.ArquivoOcorrencia();

                ValidacaoDados validacao = new ValidacaoDados();
                string mensagem = string.Empty;


                byte[] imageBytes = new byte[FileUpload1.PostedFile.InputStream.Length + 1];
                FileUpload1.PostedFile.InputStream.Read(imageBytes, 0, imageBytes.Length);

                arquivo.Arquivo = imageBytes;
                arquivo.OcorrenciaId = !hdnOcorrenciaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnOcorrenciaId.Value) : -1;
                arquivo.NomeArquivo = FileUpload1.PostedFile.FileName;
                arquivo.TipoArquivo = FileUpload1.PostedFile.ContentType;
                arquivo.ChaveArquivo = Guid.NewGuid().ToString();
                arquivo.UsuarioId = User.Identity.Name;

                validacao = rnArquivoOcorrencia.Valida(arquivo);

                if (validacao.Valido)
                {
                    rnArquivoOcorrencia.Insere(arquivo);

                    lblMensagem.Text = "Documento incluído com sucesso.";

                    odsDocumento.Select();
                    odsDocumento.DataBind();
                    grdDocumento.DataBind();

                    repCarrossel.DataBind();
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

        //private void InicializaGrid()
        //{

        //    DataTable dt = new DataTable();

        //    DataRow dr = null;

        //    dt.Columns.Add(new DataColumn("TRATAMENTOID", typeof(string)));
        //    dt.Columns.Add(new DataColumn("DESCRICAO", typeof(string)));

        //    dr = dt.NewRow();

        //    Session["grid"] = dt;

        //    grdSelecionado.DataSource = dt;
        //    grdSelecionado.DataBind();

        //}


        protected void btnAdicionar_Click(object sender, EventArgs e)
        {
            try
            {

                if (!tseTratamento.DBValue.IsNull && tseTratamento.IsValidDBValue)
                {

                    RN.Ocorrencias.OcorrenciaTratamento rnOcorrenciaTratamento = new Techne.Lyceum.RN.Ocorrencias.OcorrenciaTratamento();
                    ValidacaoDados validacao = new ValidacaoDados();


                    validacao = rnOcorrenciaTratamento.Valida(Convert.ToInt32(hdnOcorrenciaId.Value), (!tseTratamento.DBValue.IsNull && tseTratamento.IsValidDBValue ? Convert.ToInt32(tseTratamento.DBValue) : -1), User.Identity.Name);

                    if (validacao.Valido)
                    {
                        rnOcorrenciaTratamento.Insere(Convert.ToInt32(hdnOcorrenciaId.Value), Convert.ToInt32(tseTratamento.DBValue), User.Identity.Name);

                        odsTratamento.Select();
                        odsTratamento.DataBind();
                        grdTratamento.DataBind();
                        tseTratamento.ResetValue();
                        grdTratamento.Visible = true;
                    }
                    else
                    {
                        lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdTratamento_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {

            ValidacaoDados validacao = new ValidacaoDados();
            RN.Ocorrencias.OcorrenciaTratamento rnOOcorrenciaTratamento = new Techne.Lyceum.RN.Ocorrencias.OcorrenciaTratamento();
            RN.DTOs.DadosOcorrencia ocorrencia = new Techne.Lyceum.RN.DTOs.DadosOcorrencia();
            try
            {

                int id = Convert.ToInt32(grdTratamento.GetRowValues(e.VisibleIndex, "OCORRENCIATRATAMENTOID"));

                if (e.ButtonID == "btnExcluirTratamento")
                {
                    rnOOcorrenciaTratamento.Remove(id);
                    grdTratamento.DataBind();

                    lblMensagem.Text = "Tratamento excluído com sucesso.";
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void grdAcusado_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {

            ValidacaoDados validacao = new ValidacaoDados();
            RN.Ocorrencias.Acusado rnOOcorrenciaAcusado = new Techne.Lyceum.RN.Ocorrencias.Acusado();

            try
            {

                int id = Convert.ToInt32(grdAcusado.GetRowValues(e.VisibleIndex, "ACUSADOID"));

                if (e.ButtonID == "btnExcluirAcusado")
                {
                    rnOOcorrenciaAcusado.Remove(id);
                    grdAcusado.DataBind();

                    lblMensagem.Text = "Acusado excluído com sucesso.";
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void grdVitima_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {

            ValidacaoDados validacao = new ValidacaoDados();
            RN.Ocorrencias.Vitima rnOOcorrenciaVitima = new Techne.Lyceum.RN.Ocorrencias.Vitima();

            try
            {

                int id = Convert.ToInt32(grdVitima.GetRowValues(e.VisibleIndex, "VITIMAID"));

                if (e.ButtonID == "btnExcluirVitima")
                {
                    rnOOcorrenciaVitima.Remove(id);
                    grdVitima.DataBind();

                    lblMensagem.Text = "Vitima excluído com sucesso.";
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void btnAdicionarEncaminhamento_Click(object sender, EventArgs e)
        {
            try
            {
                RN.Ocorrencias.OcorrenciaEncaminhamento rnOcorrenciaEncaminhamento = new Techne.Lyceum.RN.Ocorrencias.OcorrenciaEncaminhamento();
                RN.Ocorrencias.Entidades.OcorrenciaEncaminhamento ocorrencia = new Techne.Lyceum.RN.Ocorrencias.Entidades.OcorrenciaEncaminhamento();
                RN.Ocorrencias.Entidades.OcorrenciaEncaminhamentoAquivo arquivo = new Techne.Lyceum.RN.Ocorrencias.Entidades.OcorrenciaEncaminhamentoAquivo();

                ValidacaoDados validacao = new ValidacaoDados();

                byte[] imageBytes = new byte[FileUpload2.PostedFile.InputStream.Length];
                FileUpload2.PostedFile.InputStream.Read(imageBytes, 0, imageBytes.Length);

                ocorrencia.Encaminhamento = !txtEncaminhamento.Text.IsNullOrEmptyOrWhiteSpace() ? txtEncaminhamento.Text.Trim() : null;
                ocorrencia.OcorrenciaId = !hdnOcorrenciaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnOcorrenciaId.Value) : -1;
                ocorrencia.UsuarioId = User.Identity.Name;
                arquivo.Arquivo = FileUpload2.PostedFile.InputStream.Length == 0 ? null : imageBytes;
                arquivo.NomeArquivo = FileUpload2.PostedFile.FileName;
                arquivo.TipoArquivo = FileUpload2.PostedFile.ContentType == "application/octet-stream" ? string.Empty : FileUpload2.PostedFile.ContentType;
                arquivo.UsuarioId = User.Identity.Name;

                validacao = rnOcorrenciaEncaminhamento.Valida(ocorrencia, arquivo, hdnCenso.Value);

                if (validacao.Valido)
                {
                    rnOcorrenciaEncaminhamento.Insere(ocorrencia, arquivo);

                    lblMensagem.Text = "Encaminhamento incluído com sucesso.";

                    txtEncaminhamento.Text = string.Empty;
                    odsEncaminhamento.Select();
                    odsEncaminhamento.DataBind();
                    grdEncaminhamento.DataBind();
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

        protected void grdEncaminhamento_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            try
            {

                if (e.ButtonID == "btnExcluirEncaminhamento")
                {
                    ValidacaoDados validacao = new ValidacaoDados();
                    RN.Ocorrencias.OcorrenciaEncaminhamento rnOcorrenciaEncaminhamento = new Techne.Lyceum.RN.Ocorrencias.OcorrenciaEncaminhamento();
                    int id = Convert.ToInt32(grdEncaminhamento.GetRowValues(e.VisibleIndex, "OCORRENCIAENCAMINHAMENTOID"));

                    validacao = rnOcorrenciaEncaminhamento.ValidaRemocao(id, User.Identity.Name);

                    if (validacao.Valido)
                    {
                        rnOcorrenciaEncaminhamento.Remove(id, User.Identity.Name);
                        grdEncaminhamento.DataBind();


                        lblMensagem.Text = "Encaminhamento excluído com sucesso.";
                    }
                    else
                    {
                        lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    }
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
                RN.Ocorrencias.OcorrenciaEncaminhamentoArquivo rnOcorrenciaEncaminhamentoArquivo = new Techne.Lyceum.RN.Ocorrencias.OcorrenciaEncaminhamentoArquivo();

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
                        ltEmbed.Text = string.Format(embed, ResolveUrl("~/Util/FileCS.ashx?Tabela=ocorrenciaencaminhamentoarquivo&Id="), chave[0].ToString());
                        ltEmbed.Visible = true;
                        pucVisualizarArquivo.Width = Unit.Pixel(880);
                        pucVisualizarArquivo.Height = Unit.Pixel(580);
                    }
                    else
                    {
                        pucVisualizarArquivo.Width = Unit.Pixel(350);
                        pucVisualizarArquivo.Height = Unit.Pixel(350);
                        bimgArquivo.ContentBytes = rnOcorrenciaEncaminhamentoArquivo.ObtemArquivoPor(Convert.ToInt32(chave[0]));
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

        protected void dtDataOcorrencia_DateChanged(object sender, EventArgs e)
        {
            try
            {

                if (!dtDataOcorrencia.Text.IsNullOrEmptyOrWhiteSpace())
                {
                    int dias = DateTime.Now.Subtract(dtDataOcorrencia.Date).Days;

                    if (dias > 7 && hdnPerfil.Value != "ADM")
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Esta data ultrapassa o limite para registrar uma ocorrência. Favor entrar em contato com o setor responsável.');", true);

                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void rblInterrupcao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pnlSimInterrupcao.Visible = false;
                dtInterrupcao.Text = string.Empty;
                chkTurno.ClearSelection();

                if (rblInterrupcao.SelectedValue == "1")
                {
                    pnlSimInterrupcao.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdInterrupcao_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {

            ValidacaoDados validacao = new ValidacaoDados();
            RN.Ocorrencias.OcorrenciaInterrupcao rnOcorrenciagrdInterrupcao = new Techne.Lyceum.RN.Ocorrencias.OcorrenciaInterrupcao();
            RN.DTOs.DadosOcorrencia ocorrencia = new Techne.Lyceum.RN.DTOs.DadosOcorrencia();
            try
            {

                int id = Convert.ToInt32(grdInterrupcao.GetRowValues(e.VisibleIndex, "OCORRENCIAINTERRUPCAOID"));

                if (e.ButtonID == "btnExcluirInterrupcao")
                {
                    rnOcorrenciagrdInterrupcao.Remove(id);
                    grdInterrupcao.DataBind();

                    lblMensagem.Text = "Interrupção excluído com sucesso.";
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void btnProsseguir_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Ocorrencias.Ocorrencia rnOcorrencia = new Techne.Lyceum.RN.Ocorrencias.Ocorrencia();
                RN.Ocorrencias.Entidades.Ocorrencia ocorrencia = new Techne.Lyceum.RN.Ocorrencias.Entidades.Ocorrencia();

                var validItems = new List<int>();

                ocorrencia.OcorrenciaId = !hdnOcorrenciaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnOcorrenciaId.Value) : 0;
                ocorrencia.DataOcorrencia = dtDataOcorrencia.Date;
                ocorrencia.ClasseId = !tseClasse.DBValue.IsNull && tseClasse.IsValidDBValue ? Convert.ToInt32(tseClasse.DBValue) : -1;
                ocorrencia.SubClasseId = !tseSubClasse.DBValue.IsNull && tseSubClasse.IsValidDBValue ? Convert.ToInt32(tseSubClasse.DBValue) : -1;
                ocorrencia.MeioId = !tseMeio.DBValue.IsNull && tseMeio.IsValidDBValue ? Convert.ToInt32(tseMeio.DBValue) : -1;
                ocorrencia.DelegaciaId = !tseDelegacia.DBValue.IsNull && tseDelegacia.IsValidDBValue ? Convert.ToInt32(tseDelegacia.DBValue) : -1;
                ocorrencia.BatalhaoId = !tseBatalhao.DBValue.IsNull && tseBatalhao.IsValidDBValue ? Convert.ToInt32(tseBatalhao.DBValue) : -1;
                ocorrencia.RegistroOcorrencia = !txtRO.Text.IsNullOrEmptyOrWhiteSpace() ? txtRO.Text : null;
                ocorrencia.Relato = !txtDescricao.Text.IsNullOrEmptyOrWhiteSpace() ? txtDescricao.Text : null;
                ocorrencia.Observacao = !txtObservacao.Text.IsNullOrEmptyOrWhiteSpace() ? txtObservacao.Text : null;
                ocorrencia.Censo = !hdnCenso.Value.IsNullOrEmptyOrWhiteSpace() ? hdnCenso.Value : null;
                ocorrencia.UsuarioId = User.Identity.Name;
                ocorrencia.Ativo = true;
                ocorrencia.UsoArma = !rblUsoArma.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblUsoArma.SelectedValue == "1" ? true : false) : (bool?)null;

                List<int> listaTipoArma = new List<int>();

                foreach (ListItem item in chkUsoArma.Items)
                {
                    if (item.Selected)
                    {
                        listaTipoArma.Add(Convert.ToInt32(item.Value));
                    }
                }

                validacao = rnOcorrencia.Valida(ocorrencia, listaTipoArma, ocorrencia.OcorrenciaId == 0 ? true : false);

                if (validacao.Valido)
                {
                    if (ocorrencia.OcorrenciaId == 0)
                    {
                        rnOcorrencia.Insere(ocorrencia, listaTipoArma);

                        hdnOcorrenciaId.Value = ocorrencia.OcorrenciaId.ToString();
                        pnlProsseguir.Visible = true;

                    }
                    else
                    {
                        rnOcorrencia.Atualiza(ocorrencia, listaTipoArma);

                        lblMensagem.Text = "Informações Gerais salva com sucesso.";
                    }
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void btnAdicionarInterrupcao_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Ocorrencias.OcorrenciaInterrupcao rnOcorrenciaInterrupcao = new Techne.Lyceum.RN.Ocorrencias.OcorrenciaInterrupcao();
                RN.Ocorrencias.Entidades.OcorrenciaInterrupcao interrupcao = new Techne.Lyceum.RN.Ocorrencias.Entidades.OcorrenciaInterrupcao();

                interrupcao.OcorrenciaId = !hdnOcorrenciaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnOcorrenciaId.Value) : 0;
                interrupcao.DataInterrupcao = !dtInterrupcao.Text.IsNullOrEmptyOrWhiteSpace() ? dtInterrupcao.Date : DateTime.MinValue;
                interrupcao.UsuarioId = User.Identity.Name;

                foreach (ListItem item in chkTurno.Items)
                {
                    if (item.Selected && !string.IsNullOrEmpty(item.Value))
                    {
                        if (item.Value == "M")
                        {
                            interrupcao.Manha = true;
                        }
                        if (item.Value == "T")
                        {
                            interrupcao.Tarde = true;
                        }
                        if (item.Value == "N")
                        {
                            interrupcao.Noite = true;
                        }
                    }
                }

                validacao = rnOcorrenciaInterrupcao.Valida(interrupcao, dtDataOcorrencia.Date);

                if (validacao.Valido)
                {
                    if (0 == 0)
                    {
                        rnOcorrenciaInterrupcao.Insere(interrupcao);

                        dtInterrupcao.Text = string.Empty;
                        chkTurno.ClearSelection();
                        pnlSimInterrupcao.Visible = false;
                        rblInterrupcao.ClearSelection();

                        odsInterrupcao.Select();
                        odsInterrupcao.DataBind();
                        grdInterrupcao.DataBind();

                    }
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void btnAdicionarVitima_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Ocorrencias.Vitima rnVitima = new Techne.Lyceum.RN.Ocorrencias.Vitima();
                RN.Ocorrencias.Entidades.Vitima ocorrencia = new Techne.Lyceum.RN.Ocorrencias.Entidades.Vitima();

                ocorrencia.Tipo = !rblTipoVitima.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(rblTipoVitima.SelectedValue) : -1;
                ocorrencia.OcorrenciaId = !hdnOcorrenciaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnOcorrenciaId.Value) : 0;
                ocorrencia.PessoaId = ocorrencia.Tipo == 1 && !tseAlunoVitima.DBValue.IsNull && tseAlunoVitima.IsValidDBValue ? Convert.ToDecimal(tseAlunoVitima["pessoa"]) : ocorrencia.Tipo == 2 && !tseServidorVitima.DBValue.IsNull && tseServidorVitima.IsValidDBValue ? Convert.ToDecimal(tseServidorVitima["pessoa"]) : -1;
                ocorrencia.Vinculo = ocorrencia.Tipo == 2 && !tseServidorVitima.DBValue.IsNull && tseServidorVitima.IsValidDBValue ? (Convert.ToString(tseServidorVitima["vinculo"]).IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(tseServidorVitima["vinculo"])) : (int?)null;
                ocorrencia.Cargo = ocorrencia.Tipo == 2 && !tseServidorVitima.DBValue.IsNull && tseServidorVitima.IsValidDBValue ? (Convert.ToString(tseServidorVitima["cargo"]).IsNullOrEmptyOrWhiteSpace() ? null : tseServidorVitima["cargo"].ToString()) : null;
                ocorrencia.Funcao = ocorrencia.Tipo == 2 && !tseServidorVitima.DBValue.IsNull && tseServidorVitima.IsValidDBValue ? (Convert.ToString(tseServidorVitima["funcao"]).IsNullOrEmptyOrWhiteSpace() ? null : tseServidorVitima["funcao"].ToString()) : null;
                ocorrencia.Nome = ocorrencia.Tipo == 4 && !txtNomeVitima.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeVitima.Text.Trim() : null;
                ocorrencia.CPF = ocorrencia.Tipo == 4 && !txtCPFVitima.Text.IsNullOrEmptyOrWhiteSpace() ? txtCPFVitima.Text.RetirarMascaraCPF() : null;
                ocorrencia.RgNumero = ocorrencia.Tipo == 4 && !txtRGNumPessoaVitima.Text.IsNullOrEmptyOrWhiteSpace() ? txtRGNumPessoaVitima.Text.Trim() : null;
                ocorrencia.RgTipo = ocorrencia.Tipo == 4 && !ddlRGTipoPessoaVitima.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlRGTipoPessoaVitima.SelectedValue : null;
                ocorrencia.RgUF = ocorrencia.Tipo == 4 && !ddlRGUFPessoaVitima.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlRGUFPessoaVitima.SelectedValue : null;
                ocorrencia.RgEmissor = ocorrencia.Tipo == 4 && !ddlRGEmissorPessoaVitima.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlRGEmissorPessoaVitima.SelectedValue : null;
                ocorrencia.RgDataExp = ocorrencia.Tipo == 4 && !dteRGDataExpPessoaVitima.Text.IsNullOrEmptyOrWhiteSpace() ? dteRGDataExpPessoaVitima.Date : (DateTime?)null;
                ocorrencia.Desconhecido = chkDesconhecidoVitimaAluno.Checked || chkDesconhecidoVitimaServidor.Checked || chkDesconhecidoVitima.Checked;
                ocorrencia.DataNascimento = ocorrencia.Tipo == 4 && !dtNascimentoVitima.Text.IsNullOrEmptyOrWhiteSpace() ? dtNascimentoVitima.Date : (DateTime?)null;
                ocorrencia.UsuarioId = User.Identity.Name;

                validacao = rnVitima.Valida(ocorrencia);

                if (validacao.Valido)
                {
                    rnVitima.Insere(ocorrencia);
                    tseAlunoVitima.ResetValue();
                    tseServidorVitima.ResetValue();
                    rblTipoVitima.ClearSelection();
                    txtNomeVitima.Text = string.Empty;
                    txtCPFVitima.Text = string.Empty;
                    ddlRGTipoPessoaVitima.ClearSelection();
                    ddlRGUFPessoaVitima.ClearSelection();
                    ddlRGEmissorPessoaVitima.ClearSelection();
                    txtRGNumPessoaVitima.Text = string.Empty;
                    dteRGDataExpPessoaVitima.Text = string.Empty;
                    lblIdadeVitima.Text = string.Empty;
                    lblCargoFuncaoVitima.Text = string.Empty;

                    odsVitima.Select();
                    odsVitima.DataBind();
                    grdVitima.DataBind();

                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        protected void btnAdicionarAcusado_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Ocorrencias.Acusado rnAcusado = new Techne.Lyceum.RN.Ocorrencias.Acusado();
                RN.Ocorrencias.Entidades.Acusado ocorrencia = new Techne.Lyceum.RN.Ocorrencias.Entidades.Acusado();

                ocorrencia.Tipo = !rblTipoAcusado.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(rblTipoAcusado.SelectedValue) : -1;
                ocorrencia.OcorrenciaId = !hdnOcorrenciaId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnOcorrenciaId.Value) : 0;
                ocorrencia.PessoaId = ocorrencia.Tipo == 1 && !tseAlunoAcusado.DBValue.IsNull && tseAlunoAcusado.IsValidDBValue ? Convert.ToDecimal(tseAlunoAcusado["pessoa"]) : ocorrencia.Tipo == 2 && !tseServidorAcusado.DBValue.IsNull && tseServidorAcusado.IsValidDBValue ? Convert.ToDecimal(tseServidorAcusado["pessoa"]) : -1;
                ocorrencia.Vinculo = ocorrencia.Tipo == 2 && !tseServidorAcusado.DBValue.IsNull && tseServidorAcusado.IsValidDBValue ? (Convert.ToString(tseServidorAcusado["vinculo"]).IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(tseServidorAcusado["vinculo"])) : (int?)null;
                ocorrencia.Cargo = ocorrencia.Tipo == 2 && !tseServidorAcusado.DBValue.IsNull && tseServidorAcusado.IsValidDBValue ? (Convert.ToString(tseServidorAcusado["cargo"]).IsNullOrEmptyOrWhiteSpace() ? null : tseServidorAcusado["cargo"].ToString()) : null;
                ocorrencia.Funcao = ocorrencia.Tipo == 2 && !tseServidorAcusado.DBValue.IsNull && tseServidorAcusado.IsValidDBValue ? (Convert.ToString(tseServidorAcusado["funcao"]).IsNullOrEmptyOrWhiteSpace() ? null : tseServidorAcusado["funcao"].ToString()) : null;
                ocorrencia.Nome = ocorrencia.Tipo == 4 && !txtNomeAcusado.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeAcusado.Text.Trim() : null;
                ocorrencia.CPF = ocorrencia.Tipo == 4 && !txtCPFAcusado.Text.IsNullOrEmptyOrWhiteSpace() ? txtCPFAcusado.Text.RetirarMascaraCPF() : null;
                ocorrencia.RgNumero = ocorrencia.Tipo == 4 && !txtRGNumPessoaAcusado.Text.IsNullOrEmptyOrWhiteSpace() ? txtRGNumPessoaAcusado.Text.Trim() : null;
                ocorrencia.RgTipo = ocorrencia.Tipo == 4 && !ddlRGTipoPessoaAcusado.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlRGTipoPessoaAcusado.SelectedValue : null;
                ocorrencia.RgUF = ocorrencia.Tipo == 4 && !ddlRGUFPessoaAcusado.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlRGUFPessoaAcusado.SelectedValue : null;
                ocorrencia.RgEmissor = ocorrencia.Tipo == 4 && !ddlRGEmissorPessoaAcusado.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlRGEmissorPessoaAcusado.SelectedValue : null;
                ocorrencia.RgDataExp = ocorrencia.Tipo == 4 && !dteRGDataExpPessoaAcusado.Text.IsNullOrEmptyOrWhiteSpace() ? dteRGDataExpPessoaAcusado.Date : (DateTime?)null;
                ocorrencia.Desconhecido = chkDesconhecidoAcusadoAluno.Checked || chkDesconhecidoAcusadoServidor.Checked || chkDesconhecidoAcusado.Checked;
                ocorrencia.DataNascimento = ocorrencia.Tipo == 4 && !dtNascimentoAcusado.Text.IsNullOrEmptyOrWhiteSpace() ? dtNascimentoAcusado.Date : (DateTime?)null;
                ocorrencia.UsuarioId = User.Identity.Name;

                validacao = rnAcusado.Valida(ocorrencia, false);

                if (validacao.Valido)
                {
                    rnAcusado.Insere(ocorrencia);
                    tseAlunoAcusado.ResetValue();
                    tseServidorAcusado.ResetValue();
                    rblTipoAcusado.ClearSelection();
                    txtNomeAcusado.Text = string.Empty;
                    txtCPFAcusado.Text = string.Empty;
                    ddlRGTipoPessoaAcusado.ClearSelection();
                    ddlRGUFPessoaAcusado.ClearSelection();
                    ddlRGEmissorPessoaAcusado.ClearSelection();
                    txtRGNumPessoaAcusado.Text = string.Empty;
                    dteRGDataExpPessoaAcusado.Text = string.Empty;
                    lblCargoFuncaoAcusado.Text = string.Empty;
                    lblIdadeAcusado.Text = string.Empty;

                    odsAcusado.Select();
                    odsAcusado.DataBind();
                    grdAcusado.DataBind();

                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void CarregaMotivoCancelamento()
        {
            try
            {

                RN.Ocorrencias.MotivoCancelamento rnMotivoCancelamento = new Techne.Lyceum.RN.Ocorrencias.MotivoCancelamento();
                ListItem item = new ListItem("Selecione", string.Empty);

                ddlMotivoCancelamento.Items.Clear();
                ddlMotivoCancelamento.DataSource = rnMotivoCancelamento.ListaAtivoPor();
                ddlMotivoCancelamento.DataBind();
                ddlMotivoCancelamento.Items.Insert(0, item);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }
    }
}
