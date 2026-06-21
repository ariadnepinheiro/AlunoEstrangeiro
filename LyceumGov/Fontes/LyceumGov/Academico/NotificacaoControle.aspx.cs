using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Seeduc.Infra.Helpers;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.DTOs;
using Techne.Web;
using Techne.Lyceum.RN.Entidades;
using DevExpress.Web.ASPxGridView.Rendering;
using Techne.Data;
using Techne.Lyceum.RN.Agenda.Entidades;
using Techne.Lyceum.RN.Util;
using System.Drawing;
using System.Collections;
using DevExpress.Web.ASPxEditors;
using System.Text.RegularExpressions;
using Seeduc.Infra.Data;
using Techne.Controls;
using System.IO;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/NotificacaoControle.aspx")]
    [ControlText("Notificações a Órgãos de Controle")]
    [Title("Notificações a Órgãos de Controle")]

    public partial class NotificacaoControle : TPage
    {
        public enum TipoOperacao
        {
            Novo,
            Excluir,
            Alterar,
            Consultar,
            Inicial,
            Sucesso
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

                        ObterDadosQueryString(decodedText);


                        _tipoOperacao = TipoOperacao.Consultar;

                    }
                    else
                        Response.Redirect("ListarNotificacaoControle.aspx");

                    ControlarTipoOperacao();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private string ObterDadosQueryString(string queryString)
        {
            string[] listaDados = queryString.Split('&');

            hdnNotificacaoId.Value = string.Empty;

            foreach (string dados in listaDados)
            {
                if (dados.IndexOf("aluno=") >= 0)
                    txtAluno.Text = dados.Substring(dados.LastIndexOf('=') + 1);
                if (dados.IndexOf("Id=") >= 0)
                    hdnNotificacaoId.Value = dados.Substring(dados.LastIndexOf('=') + 1);
            }

            return hdnNotificacaoId.Value;
        }


        protected void Page_Init(object sender, EventArgs e)
        {

        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            try
            {
                //ControlaAcesso();

                if (_tipoOperacao == TipoOperacao.Alterar)
                {
                    tseUnidade.Mode = ControlMode.View;
                    tseUnidade.Enabled = false;
                }


            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ControlarTipoOperacao()
        {
            switch (this._tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {

                        ImageButton[] controles = new ImageButton[] { };
                        ControlarVisibilidadeControle(controles);
                        LimparTela();
                        tseUnidade.Enabled = false;
                        tseRegional.Enabled = false;
                        RetiraVisibilidadeBotaoInterno();
                        break;
                    }

                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel, btnEditar };
                        ControlarVisibilidadeControle(controles);


                        tseUnidade.Enabled = false;
                        tseRegional.Enabled = false;
                        RetiraVisibilidadeBotaoInterno();
                        DesabilitaCampos();

                        break;
                    }

                case TipoOperacao.Novo:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel };
                        ControlarVisibilidadeControle(controles);
                        RetiraVisibilidadeBotaoInterno();
                        tseUnidade.ResetValue();
                        tseUnidade.Mode = ControlMode.View;
                        tseUnidade.Enabled = false;

                        pcNotificaControle.TabPages[2].Enabled = false;//FICAI
                        pcNotificaControle.TabPages[3].Enabled = false;//FAMI
                        pcNotificaControle.TabPages[4].Enabled = false;//CONSELHO
                        pcNotificaControle.TabPages[5].Enabled = false;//MP

                        if (Convert.ToInt32(hdnIdade.Value) < 18)
                        {
                            pcNotificaControle.TabPages[2].Enabled = true;
                            pcNotificaControle.TabPages[3].Enabled = false;

                            if (!hdnNotificacaoId.Value.IsNullOrEmptyOrWhiteSpace())
                            {
                                pcNotificaControle.TabPages[4].Enabled = true;
                                pcNotificaControle.TabPages[5].Enabled = true;
                            }
                        }
                        else
                        {
                            pcNotificaControle.TabPages[2].Enabled = false;
                            pcNotificaControle.TabPages[3].Enabled = true;
                            pcNotificaControle.TabPages[4].Enabled = false;
                            pcNotificaControle.TabPages[5].Enabled = false;
                        }
                        break;
                    }

                case TipoOperacao.Alterar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel };
                        ControlarVisibilidadeControle(controles);
                        RetiraVisibilidadeBotaoInterno();

                        HabilitaCampos();
                        if (Convert.ToInt32(hdnIdade.Value) < 18)
                        {
                            btnSalvarFICAI.Visible = true;

                            if (!hdnNotificacaoId.Value.IsNullOrEmptyOrWhiteSpace())
                            {
                                btnSalvarConselho.Visible = true;
                                btnSalvarMP.Visible = true;
                            }
                        }
                        else
                        {
                            btnSalvarFAMI.Visible = true;
                        }

                        break;
                    }

                case TipoOperacao.Consultar:
                    {

                        pcNotificaControle.TabPages[3].Enabled = false;
                        pcNotificaControle.TabPages[4].Enabled = false;

                        RN.Aluno rnAluno = new Aluno();
                        RN.DTOs.DadosAlunoNotificacao dados = new Techne.Lyceum.RN.DTOs.DadosAlunoNotificacao();
                        RN.Turmas.Notificacao rnNotificacao = new Techne.Lyceum.RN.Turmas.Notificacao();
                        RN.Turmas.OficioConselho rnOficioConselho = new Techne.Lyceum.RN.Turmas.OficioConselho();
                        RN.Turmas.OficioMPRJ rnOficioMPRJ = new Techne.Lyceum.RN.Turmas.OficioMPRJ();
                        RN.Turmas.Entidades.OficioConselho conselho = new Techne.Lyceum.RN.Turmas.Entidades.OficioConselho();
                        RN.Turmas.Entidades.OficioMPRJ mp = new Techne.Lyceum.RN.Turmas.Entidades.OficioMPRJ();
                        RN.Turmas.Entidades.Notificacao notificacao = new Techne.Lyceum.RN.Turmas.Entidades.Notificacao();
                        DateTime? dataInicio;

                        ImageButton[] controles = new ImageButton[] { btnCancel, btnEditar };
                        ControlarVisibilidadeControle(controles);
                        RetiraVisibilidadeBotaoInterno();

                        DesabilitaCampos();
                        LimparTela();

                        CarregaNivel();
                        CarregaModalidade();
                        CarregaTurno();
                        CarregaFormaContato();
                        CarregaMedidaMP();
                        CarregaMedidaConselho();
                        CarregaTipoEncaminhamento();
                        CarregaSituacaoFamiliar();

                        divPrincipalConselho.Visible = false;
                        divPrincipalMP.Visible = false;

                        tseUnidade.Enabled = false;
                        tseRegional.Enabled = false;
                        tseUnidade.Mode = ControlMode.View;
                        tseRegional.Mode = ControlMode.View;

                        pcNotificaControle.TabPages[2].Enabled = false;
                        pcNotificaControle.TabPages[3].Enabled = false;
                        pcNotificaControle.TabPages[4].Enabled = false;
                        pcNotificaControle.TabPages[5].Enabled = false;
                        pnlImprimirConselho.Visible = false;
                        pnlImprimirMP.Visible = false;



                        dados = rnAluno.ObtemDadosAlunoNotificacaoPor(txtAluno.Text.ToString());

                        if (!dados.Aluno.IsNullOrEmptyOrWhiteSpace())
                        {
                            CarregaDados(dados);

                            if (!hdnNotificacaoId.Value.IsNullOrEmptyOrWhiteSpace())
                            {
                                notificacao = rnNotificacao.ObtemPor(Convert.ToInt32(hdnNotificacaoId.Value));

                                if (!notificacao.NumeroFicai.IsNullOrEmptyOrWhiteSpace() || !notificacao.NumeroFami.IsNullOrEmptyOrWhiteSpace())
                                {
                                    hdnDataCadastro.Value = notificacao.DataCadastro.ToShortDateString();

                                    if (Convert.ToInt32(hdnIdade.Value) < 18)
                                    {
                                        CarregaDadosFICAI(notificacao);

                                        conselho = rnOficioConselho.ObtemPor(Convert.ToInt32(hdnNotificacaoId.Value));

                                        if (conselho.OficioConselhoId > 0)
                                        {
                                            CarregaDadosConselho(conselho);

                                            if (!dtEncaminhaTutelar.Text.IsNullOrEmptyOrWhiteSpace() && !txtProtocoloConTutelar.Text.IsNullOrEmptyOrWhiteSpace())
                                            {
                                                PreencheCamposOficioConselho();
                                                divPrincipalConselho.Visible = true;
                                                pnlImprimirConselho.Visible = true;
                                            }
                                            else
                                            {
                                                lblMensagem.Text = " Para exibir o ofício é necessário que no FICAI os dados referente MEDIDAS ADOTADAS PELO CONSELHO TUTELAR estejam preenchidos.";

                                            }
                                        }

                                        mp = rnOficioMPRJ.ObtemPor(Convert.ToInt32(hdnNotificacaoId.Value));

                                        if (mp.OficioMPRJId > 0)
                                        {
                                            CarregaDadosMP(mp);

                                            if (!dtEncaminhaTutelar.Text.IsNullOrEmptyOrWhiteSpace() && !ddlMedidasTutelar.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                                            {
                                                divPrincipalMP.Visible = true;
                                                PreencheCamposOficioMP();
                                                pnlImprimirMP.Visible = true;
                                            }
                                            else
                                            {
                                                lblMensagem.Text = " Para exibir o ofício é necessário que no FICAI os dados referente ATUAÇÃO MINISTÉRIO PÚBLICO estejam preenchidos.";

                                            }

                                        }

                                    }
                                    else
                                    {
                                        CarregaDadosFAMI(notificacao);
                                    }
                                }
                            }

                            if (Convert.ToInt32(hdnIdade.Value) < 18)
                            {
                                pcNotificaControle.TabPages[2].Enabled = true;
                                pcNotificaControle.TabPages[3].Enabled = false;

                                txtQtdFaltas.Text = rnNotificacao.ObtemQuantidadeFaltasPor(dados.Aluno, out dataInicio).ToString();

                                if (dataInicio != null)
                                {
                                    txtInicioFaltas.Text = dataInicio.Value.ToShortDateString();
                                }

                                if (!hdnNotificacaoId.Value.IsNullOrEmptyOrWhiteSpace())
                                {
                                    pcNotificaControle.TabPages[4].Enabled = true;
                                    pcNotificaControle.TabPages[5].Enabled = true;
                                }
                            }
                            else
                            {
                                pcNotificaControle.TabPages[2].Enabled = false;
                                pcNotificaControle.TabPages[3].Enabled = true;
                                pcNotificaControle.TabPages[4].Enabled = false;
                                pcNotificaControle.TabPages[5].Enabled = false;

                                txtNumFaltasFAMI.Text = rnNotificacao.ObtemQuantidadeFaltasPor(dados.Aluno, out dataInicio).ToString();

                                if (dataInicio != null)
                                {
                                    txtInicioFaltasFAMI.Text = dataInicio.Value.ToShortDateString();
                                }
                            }
                        }


                        break;
                    }
            }
        }

        private void CarregaModalidade()
        {
            ListItem item = new ListItem("Selecione", string.Empty);
            Object objModalidade = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.Modalidade, RN.Curso.QueryListaModalidadeCurso);

            ddlModalidade.Items.Clear();
            ddlModalidade.DataSource = objModalidade;
            ddlModalidade.DataBind();
            ddlModalidade.Items.Insert(0, item);

        }

        private void CarregaNivel()
        {
            ListItem item = new ListItem("Selecione", string.Empty);
            Object objNivel = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.Nivel, RN.Curso.QueryListaTipoCurso);

            ddlNivel.Items.Clear();
            ddlNivel.DataSource = objNivel;
            ddlNivel.DataBind();
            ddlNivel.Items.Insert(0, item);
        }

        private void CarregaDados(RN.DTOs.DadosAlunoNotificacao dados)
        {
            int idade = 0;
            string dataLimite = "31/01/" + (DateTime.Now.Year + 1);

            txtAluno.Text = !dados.Aluno.IsNullOrEmptyOrWhiteSpace() ? dados.Aluno : string.Empty;
            lblAlunoTSearch.Text = !dados.Nome.IsNullOrEmptyOrWhiteSpace() ? dados.Nome : string.Empty;
            txtNomeCompl.Text = !dados.Nome.IsNullOrEmptyOrWhiteSpace() ? dados.Nome : string.Empty;

            if (dados.DataNascimento.Day > 0)
            {
                dtDataNasc.Date = dados.DataNascimento;
                idade = Utils.CalcularIdadePorData(dtDataNasc.Date, Convert.ToDateTime(dataLimite));
                hdnIdade.Value = idade.ToString();
                lblIdade.Text = idade.ToString() + " Anos";
            }

            txtNomeSocial.Text = !dados.NomeSocial.IsNullOrEmptyOrWhiteSpace() ? dados.NomeSocial : string.Empty;

            txtEndereco.Text = !dados.Endereco.IsNullOrEmptyOrWhiteSpace() ? dados.Endereco : string.Empty;
            txtEndNum.Text = !dados.Numero.IsNullOrEmptyOrWhiteSpace() ? dados.Numero : string.Empty;
            txtEndCompl.Text = !dados.Complemento.IsNullOrEmptyOrWhiteSpace() ? dados.Complemento : string.Empty;
            txtCep.Text = !dados.Cep.IsNullOrEmptyOrWhiteSpace() ? dados.Cep : string.Empty;
            txtBairro.Text = !dados.Bairro.IsNullOrEmptyOrWhiteSpace() ? dados.Bairro : string.Empty;

            // verifica se existe valor para municipio
            if (!string.IsNullOrEmpty(dados.Municipio))
            {
                // preenche os dados nos controles da tela
                hdnCodMunicipio.Value = dados.CodMunicipio;
                txtMunicipio.Text = dados.Municipio;

                // obtém a UF de acordo com o codigo do municipío
                txtEstado.Value = Endereco.ObterUFMunicipio(dados.CodMunicipio);
            }
            else
            {
                hdnCodMunicipio.Value = string.Empty;
                txtMunicipio.Text = string.Empty;
                txtEstado.Value = string.Empty;
            }

            txtNomeMae.Text = dados.NomeMae;
            txtNomePai.Text = dados.NomePai;


            if (!string.IsNullOrEmpty(dados.ResponsavelLegal))
            {
                string[] tipo_resp = dados.ResponsavelLegal.Split(';');
                foreach (String str in tipo_resp)
                {
                    if (!string.IsNullOrEmpty(str) && (rblResponsavel.Items.FindByValue(str) != null))
                    {
                        rblResponsavel.Items.FindByValue(str).Selected = true;
                    }
                }
            }
            long result;

            if (!string.IsNullOrEmpty(dados.ResponsavelNome))
            {
                txtTelefoneResp.Visible = true;
                txtCPFResponsavel.Visible = true;
                txtNomeResponsavel.Visible = true;
                lblTelefoneResponsavel.Visible = true;
                lblCPFResponsavel.Visible = true;
                lblNomeResponsavel.Visible = true;

                txtNomeResponsavel.Text = dados.ResponsavelNome;



                if (Int64.TryParse(dados.ResponsavelCpf, out result))
                {
                    if (result != 0)
                        txtCPFResponsavel.Text = string.Format(@"{0:000\.000\.000-00}", result);
                    else
                        txtCPFResponsavel.Text = "";
                }
                else
                    txtCPFResponsavel.Text = dados.ResponsavelCpf;

                if (Int64.TryParse(dados.ResponsavelTelefone, out result))
                    txtTelefoneResp.Text = string.Format("{0:(00)0000-0000}", result);
                else
                    txtTelefoneResp.Text = dados.ResponsavelTelefone;

            }

            if (!dados.MaeCpf.IsNullOrEmptyOrWhiteSpace())
            {

                if (Int64.TryParse(dados.MaeCpf, out result))
                {
                    if (result != 0)
                        txtCPFMae.Text = string.Format(@"{0:000\.000\.000-00}", result);
                    else
                        txtCPFMae.Text = "";
                }
                else
                    txtCPFMae.Text = dados.MaeCpf;
            }

            if (!dados.PaiCpf.IsNullOrEmptyOrWhiteSpace())
            {


                if (Int64.TryParse(dados.PaiCpf, out result))
                {
                    if (result != 0)
                        txtCPFPai.Text = string.Format(@"{0:000\.000\.000-00}", result);
                    else
                        txtCPFPai.Text = "";
                }
                else
                    txtCPFPai.Text = dados.PaiCpf;
            }

            if (Int64.TryParse(dados.MaeTelefone, out result))
                txtTelefoneMae.Text = string.Format("{0:(00)0000-0000}", result);
            else
                txtTelefoneMae.Text = dados.MaeTelefone;

            if (Int64.TryParse(dados.PaiTelefone, out result))
                txtTelefonePai.Text = string.Format("{0:(00)0000-0000}", result);
            else
                txtTelefonePai.Text = dados.PaiTelefone;

            chkFalecidaMae.Checked = dados.MaeFalecida;
            chkFalecidoPai.Checked = dados.PaiFalecido;

            chkNaoDeclarMae.Checked = dados.NomeMae == chkNaoDeclarMae.Text.ToUpper();


            if (chkFalecidaMae.Checked || chkNaoDeclarMae.Checked)
                DesabilitaResponsavelLegal("H", "Mãe");

            if (chkNaoDeclarMae.Checked)
            {
                txtCPFMae.Text = string.Empty;
                txtTelefoneMae.Text = string.Empty;
                chkFalecidaMae.Checked = false;
                txtNomeMae.ReadOnly = true;
                txtCPFMae.Enabled = false;
                txtTelefoneMae.Enabled = false;
                chkFalecidaMae.Enabled = false;
            }
            chkNaoDeclarPai.Checked = dados.NomePai == chkNaoDeclarPai.Text.ToUpper();


            if (chkFalecidoPai.Checked || chkNaoDeclarPai.Checked)
                DesabilitaResponsavelLegal("H", "Pai");

            if (chkNaoDeclarPai.Checked)
            {
                txtCPFPai.Text = string.Empty;
                txtTelefonePai.Text = string.Empty;
                txtNomePai.ReadOnly = true;
                txtCPFPai.Enabled = false;
                txtTelefonePai.Enabled = false;
                chkFalecidoPai.Checked = false;
            }

            tseRegional.DBValue = dados.Regional;

            if (!string.IsNullOrEmpty(dados.Censo))
            {
                tseUnidade.DBValue = dados.Censo;
            }
            else
            {
                tseUnidade.ResetValue();
            }

            ddlTurno.SelectedValue = dados.Turno;

            ddlNivel.SelectedValue = dados.Nível;

            ddlModalidade.SelectedValue = dados.Modalidade;

            CarregaCurso();

            ddlCurso.SelectedValue = dados.Curso;

            hdnCurriculo.Value = dados.Curriculo;

            CarregaSerie();

            ddlSerie.SelectedValue = dados.Serie;

            txtSituacao.Text = dados.Situacao;

        }

        protected void DesabilitaResponsavelLegal(string operacao, string filiacao)
        {
            foreach (ListItem item in rblResponsavel.Items)
            {
                if (item.Text == filiacao)
                {
                    if (operacao == "H")
                    {
                        item.Selected = false;
                        item.Enabled = false;
                        return;
                    }
                    if (operacao == "D")
                    {
                        item.Selected = false;
                        item.Enabled = true;
                        return;
                    }
                }
            }
        }

        private void CarregaTurno()
        {
            RN.Turno rnTurno = new Turno();

            ListItem item = new ListItem("Selecione", string.Empty);
            ddlTurno.Items.Clear();

            ddlTurno.DataSource = rnTurno.ListaTurnosPor();

            ddlTurno.DataBind();
            ddlTurno.Items.Insert(0, item);
        }

        private void CarregaTipoEncaminhamento()
        {
            RN.Turmas.TipoEncaminhamento rnTipoEncaminhamento = new Techne.Lyceum.RN.Turmas.TipoEncaminhamento();

            ListItem item = new ListItem("Selecione", string.Empty);
            ddlEncaminhaUE.Items.Clear();

            ddlEncaminhaUE.DataSource = rnTipoEncaminhamento.ListaAtivo();

            ddlEncaminhaUE.DataBind();
            ddlEncaminhaUE.Items.Insert(0, item);
        }

        private void CarregaSituacaoFamiliar()
        {
            RN.Turmas.SituacaoFamiliar rnSituacaoFamiliar = new Techne.Lyceum.RN.Turmas.SituacaoFamiliar();

            ListItem item = new ListItem("Selecione", string.Empty);
            ddlSituacaoFamiliarFICAI.Items.Clear();

            ddlSituacaoFamiliarFICAI.DataSource = rnSituacaoFamiliar.ListaAtivo();
            ddlSituacaoFamiliarFICAI.DataBind();
            ddlSituacaoFamiliarFICAI.Items.Insert(0, item);
        }

        private void CarregaMedidaMP()
        {
            RN.Turmas.MedidaMPRJ rnMedidaMPRJ = new Techne.Lyceum.RN.Turmas.MedidaMPRJ();

            ListItem item = new ListItem("Selecione", string.Empty);
            ddlMedidasMinisterio.Items.Clear();

            ddlMedidasMinisterio.DataSource = rnMedidaMPRJ.ListaAtivo();
            ddlMedidasMinisterio.DataBind();
            ddlMedidasMinisterio.Items.Insert(0, item);
        }

        private void CarregaMedidaConselho()
        {
            RN.Turmas.MedidaConselhoTutelar rnTipoEncaminhamento = new Techne.Lyceum.RN.Turmas.MedidaConselhoTutelar();

            ListItem item = new ListItem("Selecione", string.Empty);
            ddlMedidasTutelar.Items.Clear();

            ddlMedidasTutelar.DataSource = rnTipoEncaminhamento.ListaAtivo();
            ddlMedidasTutelar.DataBind();
            ddlMedidasTutelar.Items.Insert(0, item);
        }

        private void CarregaFormaContato()
        {
            RN.Turmas.FormaContato rnFormaContato = new Techne.Lyceum.RN.Turmas.FormaContato();
            DataTable dt = new DataTable();

            ListItem item = new ListItem("Selecione", string.Empty);

            ddlFormaContato1.Items.Clear();
            ddlFormaContato2.Items.Clear();
            ddlFormaContato3.Items.Clear();

            dt = rnFormaContato.ListaAtivo();

            ddlFormaContato1.DataSource = dt;
            ddlFormaContato2.DataSource = dt;
            ddlFormaContato3.DataSource = dt;

            ddlFormaContato1.DataBind();
            ddlFormaContato2.DataBind();
            ddlFormaContato3.DataBind();


            ddlFormaContato1.Items.Insert(0, item);
            ddlFormaContato2.Items.Insert(0, item);
            ddlFormaContato3.Items.Insert(0, item);
        }

        private void CarregaFormaContato(DropDownList controle)
        {
            RN.Turmas.FormaContato rnFormaContato = new Techne.Lyceum.RN.Turmas.FormaContato();
            DataTable dt = new DataTable();

            ListItem item = new ListItem("Selecione", string.Empty);

            controle.Items.Clear();

            dt = rnFormaContato.ListaAtivo();

            controle.DataSource = dt;
            controle.DataBind();
            controle.Items.Insert(0, item);

        }

        private void CarregaCurso()
        {
            RN.Curso rnCurso = new Curso();

            ListItem item = new ListItem("Selecione", string.Empty);
            ddlCurso.Items.Clear();

            ddlCurso.DataSource = RN.Curso.ListarCursoPorUE(tseUnidade.DBValue.ToString(), ddlModalidade.SelectedValue, ddlNivel.SelectedValue);

            ddlCurso.DataBind();
            ddlCurso.Items.Insert(0, item);
        }

        private void CarregaSerie()
        {
            RN.Serie rnSerie = new Serie();

            ListItem item = new ListItem("Selecione", string.Empty);
            ddlSerie.Items.Clear();

            ddlSerie.DataSource = rnSerie.ObtemSeriesNotificacaoPor(ddlCurso.SelectedValue, ddlTurno.SelectedValue.ToString(), hdnCurriculo.Value);

            ddlSerie.DataBind();
            ddlSerie.Items.Insert(0, item);
        }

        private void LimparTela()
        {
            hdnDataCadastro.Value = string.Empty;
            txtSituacao.Text = string.Empty;
            txtNomeSocial.Text = string.Empty;
            tseRegional.ResetValue();
            tseUnidade.ResetValue();
            ddlTurno.ClearSelection();
            hdnCurriculo.Value = string.Empty;
            ddlCurso.Items.Clear();
            ddlSerie.Items.Clear();
            ddlNivel.Items.Clear();
            ddlModalidade.Items.Clear();

            dtDataNasc.Text = string.Empty;
            txtEndereco.Text = string.Empty;
            txtEndNum.Text = string.Empty;
            txtEndCompl.Text = string.Empty;
            txtCep.Text = string.Empty;
            txtBairro.Text = string.Empty;
            txtMunicipio.Text = string.Empty;

            txtNomePai.Text = string.Empty;
            txtNomeMae.Text = string.Empty;
            chkNaoDeclarMae.Checked = false;
            chkNaoDeclarPai.Checked = false;
            chkFalecidaMae.Checked = false;
            chkFalecidoPai.Checked = false;

            txtCPFPai.Text = string.Empty;
            txtCPFMae.Text = string.Empty;
            txtTelefoneMae.Text = string.Empty;
            txtTelefonePai.Text = string.Empty;
            rblResponsavel.ClearSelection();
            txtNomeResponsavel.Text = string.Empty;
            txtCPFResponsavel.Text = string.Empty;
            txtTelefoneResp.Text = string.Empty;
            txtNomeResponsavel.Visible = false;
            lblNomeResponsavel.Visible = false;
            txtCPFResponsavel.Visible = false;
            lblCPFResponsavel.Visible = false;
            txtTelefoneResp.Visible = false;
            lblTelefoneResponsavel.Visible = false;

            //FICAI
            txtQtdFaltas.Text = string.Empty;
            txtInicioFaltas.Text = string.Empty;
            txtNumFICAI.Text = string.Empty;
            dtComunicacao.Text = string.Empty;
            txtObsEstudante.Text = string.Empty;
            ddlFormaContato1.ClearSelection();
            ddlFormaContato2.ClearSelection();
            ddlFormaContato3.ClearSelection();
            dtData1.Text = string.Empty;
            dtData2.Text = string.Empty;
            dtData3.Text = string.Empty;
            txtAlegacaofaltasFICAI.Text = string.Empty;
            ddlSituacaoFamiliarFICAI.ClearSelection();
            ddlEncaminhaUE.ClearSelection();
            txtEquipamento.Text = string.Empty;
            dtRetorno.Text = string.Empty;
            dtEncaminha.Text = string.Empty;
            txtProtocoloConTutelar.Text = string.Empty;
            ddlMedidasTutelar.ClearSelection();
            dtEncaminhaTutelar.Text = string.Empty;
            txtConselheiro.Text = string.Empty;
            ddlMedidasMinisterio.ClearSelection();
            dtEncaminhaMinisterio.Text = string.Empty;
            txtPromotor.Text = string.Empty;

            //FAMI
            txtNumFAMI.Text = string.Empty;
            dtComunicacaoFAMI.Text = string.Empty;
            txtNumFaltasFAMI.Text = string.Empty;
            txtInicioFaltasFAMI.Text = string.Empty;
            txtAlegacaoFaltasFAMI.Text = string.Empty;
            txtEncaminhamentosFAMI.Text = string.Empty;

            //CONSELHO
            txtAoConselho.Text = string.Empty;
            txtCEPOficioConselho.Text = string.Empty;
            txtMunicipioOficioConselho.Text = string.Empty;
            txtEndNumOficioConselho.Text = string.Empty;
            txtBairroOficioConselho.Text = string.Empty;
            txtEstadoOficioConselho.Value = string.Empty;
            hdnMunicipioOficioConselho.Value = string.Empty;
            hdnOficioConselhoId.Value = string.Empty;

            //MP
            txtPromotoria.Text = string.Empty;
            txtEnderecoOficioMPRJ.Text = string.Empty;
            txtEndNumOficioMPRJ.Text = string.Empty;
            txtBairroOficioMPRJ.Text = string.Empty;
            txtCEPOficioMPRJ.Text = string.Empty;
            txtMunicipioOficioMPRJ.Text = string.Empty;
            txtEstadoOficioMPRJ.Value = string.Empty;
            hdnMunicipioOficioMPRJ.Value = string.Empty;
            hdnOficioMPRJId.Value = string.Empty;

        }

        private void DesabilitaCampos()
        {
            //FICAI 
            dtComunicacao.Enabled = false;
            txtObsEstudante.Enabled = false;
            ddlFormaContato1.Enabled = false;
            ddlFormaContato2.Enabled = false;
            ddlFormaContato3.Enabled = false;
            dtData1.Enabled = false;
            dtData2.Enabled = false;
            dtData3.Enabled = false;
            txtAlegacaofaltasFICAI.Enabled = false;
            ddlSituacaoFamiliarFICAI.Enabled = false;
            ddlEncaminhaUE.Enabled = false;
            txtEquipamento.Enabled = false;
            dtRetorno.Enabled = false;
            dtEncaminha.Enabled = false;
            txtProtocoloConTutelar.Enabled = false;
            ddlMedidasTutelar.Enabled = false;
            dtEncaminhaTutelar.Enabled = false;
            txtConselheiro.Enabled = false;
            ddlMedidasMinisterio.Enabled = false;
            dtEncaminhaMinisterio.Enabled = false;
            txtPromotor.Enabled = false;

            //FAMI

            dtComunicacaoFAMI.Enabled = false;
            txtAlegacaoFaltasFAMI.Enabled = false;
            txtEncaminhamentosFAMI.Enabled = false;

            //CONSELHO
            txtAoConselho.Enabled = false;
            txtCEPOficioConselho.Enabled = false;
            txtMunicipioOficioConselho.Enabled = false;
            txtEndNumOficioConselho.Enabled = false;
            txtBairroOficioConselho.Enabled = false;
            txtEnderecoOficioConselho.Enabled = false;

            //MP
            txtPromotoria.Enabled = false;
            txtEnderecoOficioMPRJ.Enabled = false;
            txtEndNumOficioMPRJ.Enabled = false;
            txtBairroOficioMPRJ.Enabled = false;
            txtCEPOficioMPRJ.Enabled = false;
            txtMunicipioOficioMPRJ.Enabled = false;

        }

        private void HabilitaCampos()
        {
            //FICAI 
            dtComunicacao.Enabled = true;
            txtObsEstudante.Enabled = true;
            ddlFormaContato1.Enabled = true;
            ddlFormaContato2.Enabled = true;
            ddlFormaContato3.Enabled = true;
            dtData1.Enabled = true;
            dtData2.Enabled = true;
            dtData3.Enabled = true;
            txtAlegacaofaltasFICAI.Enabled = true;
            ddlSituacaoFamiliarFICAI.Enabled = true;
            ddlEncaminhaUE.Enabled = true;
            txtEquipamento.Enabled = true;
            dtRetorno.Enabled = true;
            dtEncaminha.Enabled = true;
            txtProtocoloConTutelar.Enabled = true;
            ddlMedidasTutelar.Enabled = true;
            dtEncaminhaTutelar.Enabled = true;
            txtConselheiro.Enabled = true;
            ddlMedidasMinisterio.Enabled = true;
            dtEncaminhaMinisterio.Enabled = true;
            txtPromotor.Enabled = true;

            //FAMI

            dtComunicacaoFAMI.Enabled = true;
            txtAlegacaoFaltasFAMI.Enabled = true;
            txtEncaminhamentosFAMI.Enabled = true;

            //CONSELHO
            txtAoConselho.Enabled = true;
            txtCEPOficioConselho.Enabled = true;
            txtMunicipioOficioConselho.Enabled = true;
            txtEndNumOficioConselho.Enabled = true;
            txtBairroOficioConselho.Enabled = true;
            txtEnderecoOficioConselho.Enabled = true;

            //MP
            txtPromotoria.Enabled = true;
            txtEnderecoOficioMPRJ.Enabled = true;
            txtEndNumOficioMPRJ.Enabled = true;
            txtBairroOficioMPRJ.Enabled = true;
            txtCEPOficioMPRJ.Enabled = true;
            txtMunicipioOficioMPRJ.Enabled = true;

        }

        private void CarregaDadosFAMI(RN.Turmas.Entidades.Notificacao notificacao)
        {
            txtNumFAMI.Text = notificacao.NumeroFami;
            dtComunicacaoFAMI.Text = notificacao.DataComunicacao.ToShortDateString();
            txtNumFaltasFAMI.Text = Convert.ToString(notificacao.QuantidadeFaltas);
            txtInicioFaltasFAMI.Text = notificacao.DataInicioFaltas.ToShortDateString();
            txtAlegacaoFaltasFAMI.Text = notificacao.Alegacao;
            txtEncaminhamentosFAMI.Text = notificacao.EncaminhamentosRealizado;
        }

        private void CarregaDadosFICAI(RN.Turmas.Entidades.Notificacao notificacao)
        {
            lblNumeroFICAIConselho.Text = Convert.ToString(notificacao.NumeroFicai);
            lblNumeroFICAIMP.Text = Convert.ToString(notificacao.NumeroFicai);
            txtQtdFaltas.Text = Convert.ToString(notificacao.QuantidadeFaltas);
            txtInicioFaltas.Text = notificacao.DataInicioFaltas.ToShortDateString();
            txtNumFICAI.Text = Convert.ToString(notificacao.NumeroFicai);
            dtComunicacao.Date = notificacao.DataComunicacao;
            txtObsEstudante.Text = notificacao.Observacao;
            if (notificacao.FormaContatoId1.HasValue)
            {
                CarregaFormaContato(ddlFormaContato1);
                ddlFormaContato1.SelectedValue = notificacao.FormaContatoId1.Value.ToString();
                dtData1.Date = notificacao.DataContato1.Value;
            }

            if (notificacao.FormaContatoId2.HasValue)
            {
                CarregaFormaContato(ddlFormaContato2);
                ddlFormaContato2.SelectedValue = notificacao.FormaContatoId2.Value.ToString();
                dtData2.Date = notificacao.DataContato2.Value;
            }


            if (notificacao.FormaContatoId3.HasValue)
            {
                CarregaFormaContato(ddlFormaContato3);
                ddlFormaContato3.SelectedValue = notificacao.FormaContatoId3.Value.ToString();
                dtData3.Date = notificacao.DataContato3.Value;
            }


            txtAlegacaofaltasFICAI.Text = notificacao.Alegacao;
            ddlSituacaoFamiliarFICAI.SelectedValue = Convert.ToString(notificacao.SituacaoFamiliarId);
            ddlEncaminhaUE.SelectedValue = Convert.ToString(notificacao.TipoEncaminhamentoId);
            txtEquipamento.Text = notificacao.EquipamentoUsado;

            if (notificacao.DataRetorno.HasValue)
                dtRetorno.Date = notificacao.DataRetorno.Value;

            if (notificacao.DataEncaminhamentoEscola.HasValue)
                dtEncaminha.Date = notificacao.DataEncaminhamentoEscola.Value;

            txtProtocoloConTutelar.Text = notificacao.ProtocoloConselho;
            ddlMedidasTutelar.SelectedValue = Convert.ToString(notificacao.MedidasConselhoTutelarId);
            if (notificacao.DataEncaminhamentoConselho.HasValue)
                dtEncaminhaTutelar.Date = notificacao.DataEncaminhamentoConselho.Value;

            txtConselheiro.Text = notificacao.NomeConselheiro;
            ddlMedidasMinisterio.SelectedValue = Convert.ToString(notificacao.MedidaMPRJId);

            if (notificacao.DataEncaminhamentoMprj.HasValue)
                dtEncaminhaMinisterio.Date = notificacao.DataEncaminhamentoMprj.Value;

            txtPromotor.Text = notificacao.Promotor;
        }

        private void CarregaDadosConselho(RN.Turmas.Entidades.OficioConselho conselho)
        {
            hdnOficioConselhoId.Value = conselho.OficioConselhoId.ToString();
            txtAoConselho.Text = conselho.Conselho;
            txtCEPOficioConselho.Text = conselho.Cep;
            tseCEPOficioConselho.DBValue = conselho.Cep;
            txtEnderecoOficioConselho.Text = conselho.Endereco;
            txtMunicipioOficioConselho.Text = Endereco.ObterDescricaoMunicipio(conselho.Municipio);
            txtEndNumOficioConselho.Text = conselho.Numero;
            txtBairroOficioConselho.Text = conselho.Bairro;
            hdnMunicipioOficioConselho.Value = conselho.Municipio;
            txtEstadoOficioConselho.Value = Endereco.ObterUFMunicipio(conselho.Municipio);
        }

        private void CarregaDadosMP(RN.Turmas.Entidades.OficioMPRJ mp)
        {
            hdnOficioMPRJId.Value = mp.OficioMPRJId.ToString();
            txtPromotoria.Text = mp.Promotoria;
            txtEnderecoOficioMPRJ.Text = mp.Endereco;
            txtEndNumOficioMPRJ.Text = mp.Numero;
            txtBairroOficioMPRJ.Text = mp.Bairro;
            txtCEPOficioMPRJ.Text = mp.Cep;
            tseCEPOficioMPRJ.DBValue = mp.Cep;
            txtMunicipioOficioMPRJ.Text = Endereco.ObterDescricaoMunicipio(mp.Municipio);
            hdnMunicipioOficioMPRJ.Value = mp.Municipio;
            txtEstadoOficioMPRJ.Value = Endereco.ObterUFMunicipio(mp.Municipio);
        }

        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (var botao in botoes)
            {
                botao.Visible = true;
            }

            ControlaAcesso(btnEditar, AcaoControle.editar);

        }

        private void RetiraVisibilidadeBotao()
        {
            btnEditar.Visible = false;
            btnCancel.Visible = false;
        }

        private void RetiraVisibilidadeBotaoInterno()
        {
            btnSalvarConselho.Visible = false;
            btnSalvarFAMI.Visible = false;
            btnSalvarFICAI.Visible = false;
            btnSalvarMP.Visible = false;
        }

        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Novo;
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

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                btnEditar.Visible = true;
                RetiraVisibilidadeBotaoInterno();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnCancelarVoltar_Click(object sender, ImageClickEventArgs e)
        {

            this.Server.Transfer("ListarNotificacaoControle.aspx?Chave=" + string.Empty);
        }

        protected void btnSalvarFAMI_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Turmas.Notificacao rnNotificacao = new Techne.Lyceum.RN.Turmas.Notificacao();
                RN.Turmas.Entidades.Notificacao notificacao = new Techne.Lyceum.RN.Turmas.Entidades.Notificacao();

                notificacao.Aluno = !txtAluno.Text.IsNullOrEmptyOrWhiteSpace() ? txtAluno.Text.ToString() : null;
                notificacao.Ano = !dtComunicacaoFAMI.Text.IsNullOrEmptyOrWhiteSpace() ? dtComunicacaoFAMI.Date.Year : -1;
                notificacao.NotificacaoId = !hdnNotificacaoId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnNotificacaoId.Value) : 0;
                notificacao.DataComunicacao = !dtComunicacaoFAMI.Text.IsNullOrEmptyOrWhiteSpace() ? dtComunicacaoFAMI.Date : DateTime.MinValue;
                notificacao.QuantidadeFaltas = !txtNumFaltasFAMI.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtNumFaltasFAMI.Text) : -1;
                notificacao.DataInicioFaltas = !txtInicioFaltasFAMI.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(txtInicioFaltasFAMI.Text) : DateTime.MinValue;
                notificacao.Alegacao = !txtAlegacaoFaltasFAMI.Text.IsNullOrEmptyOrWhiteSpace() ? txtAlegacaoFaltasFAMI.Text : null;
                notificacao.EncaminhamentosRealizado = !txtEncaminhamentosFAMI.Text.IsNullOrEmptyOrWhiteSpace() ? txtEncaminhamentosFAMI.Text : null;
                notificacao.UsuarioId = User.Identity.Name;

                validacao = rnNotificacao.Valida(notificacao, Convert.ToInt32(hdnIdade.Value), notificacao.NotificacaoId == 0 ? true : false);

                if (validacao.Valido)
                {
                    if (notificacao.NotificacaoId == 0)
                    {
                        rnNotificacao.Insere(notificacao);
                        lblMensagem.Text = "FAMI cadastrada com sucesso.";
                        txtNumFaltasFAMI.Text = notificacao.NumeroFami.ToString();

                    }
                    else
                    {
                        rnNotificacao.Atualiza(notificacao);
                        lblMensagem.Text = "FAMI atualizado com sucesso.";
                    }

                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();

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

        protected void btnSalvarFICAI_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Turmas.Notificacao rnNotificacao = new Techne.Lyceum.RN.Turmas.Notificacao();
                RN.Turmas.Entidades.Notificacao notificacao = new Techne.Lyceum.RN.Turmas.Entidades.Notificacao();

                notificacao.Aluno = !txtAluno.Text.IsNullOrEmptyOrWhiteSpace() ? txtAluno.Text.ToString() : null;
                notificacao.Ano = !dtComunicacao.Text.IsNullOrEmptyOrWhiteSpace() ? dtComunicacao.Date.Year : -1;
                notificacao.NotificacaoId = !hdnNotificacaoId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnNotificacaoId.Value) : 0;
                notificacao.QuantidadeFaltas = !txtQtdFaltas.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(txtQtdFaltas.Text) : -1;
                notificacao.DataInicioFaltas = !txtInicioFaltas.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(txtInicioFaltas.Text) : DateTime.MinValue;
                notificacao.DataComunicacao = !dtComunicacao.Text.IsNullOrEmptyOrWhiteSpace() ? dtComunicacao.Date : DateTime.MinValue;
                notificacao.Observacao = !txtObsEstudante.Text.IsNullOrEmptyOrWhiteSpace() ? txtObsEstudante.Text : null;
                notificacao.FormaContatoId1 = !ddlFormaContato1.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlFormaContato1.SelectedValue) : -1;
                notificacao.FormaContatoId2 = !ddlFormaContato2.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlFormaContato2.SelectedValue) : -1;
                notificacao.FormaContatoId3 = !ddlFormaContato3.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlFormaContato3.SelectedValue) : -1;
                notificacao.DataContato1 = !dtData1.Text.IsNullOrEmptyOrWhiteSpace() ? dtData1.Date : (DateTime?)null;
                notificacao.DataContato2 = !dtData2.Text.IsNullOrEmptyOrWhiteSpace() ? dtData2.Date : (DateTime?)null;
                notificacao.DataContato3 = !dtData3.Text.IsNullOrEmptyOrWhiteSpace() ? dtData3.Date : (DateTime?)null;
                notificacao.SituacaoFamiliarId = !ddlSituacaoFamiliarFICAI.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlSituacaoFamiliarFICAI.SelectedValue) : -1;
                notificacao.Alegacao = !txtAlegacaofaltasFICAI.Text.IsNullOrEmptyOrWhiteSpace() ? txtAlegacaofaltasFICAI.Text : null;
                notificacao.TipoEncaminhamentoId = !ddlEncaminhaUE.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlEncaminhaUE.SelectedValue) : -1;
                notificacao.EquipamentoUsado = !txtEquipamento.Text.IsNullOrEmptyOrWhiteSpace() ? txtEquipamento.Text : null;
                notificacao.DataRetorno = !dtRetorno.Text.IsNullOrEmptyOrWhiteSpace() ? dtRetorno.Date : (DateTime?)null;
                notificacao.DataEncaminhamentoEscola = !dtEncaminha.Text.IsNullOrEmptyOrWhiteSpace() ? dtEncaminha.Date : (DateTime?)null;
                notificacao.ProtocoloConselho = !txtProtocoloConTutelar.Text.IsNullOrEmptyOrWhiteSpace() ? txtProtocoloConTutelar.Text : null;
                notificacao.MedidasConselhoTutelarId = !ddlMedidasTutelar.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlMedidasTutelar.SelectedValue) : -1;
                notificacao.DataEncaminhamentoConselho = !dtEncaminhaTutelar.Text.IsNullOrEmptyOrWhiteSpace() ? dtEncaminhaTutelar.Date : (DateTime?)null;
                notificacao.NomeConselheiro = !txtConselheiro.Text.IsNullOrEmptyOrWhiteSpace() ? txtConselheiro.Text : null;
                notificacao.MedidaMPRJId = !ddlMedidasMinisterio.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlMedidasMinisterio.SelectedValue) : -1;
                notificacao.DataEncaminhamentoMprj = !dtEncaminhaMinisterio.Text.IsNullOrEmptyOrWhiteSpace() ? dtEncaminhaMinisterio.Date : (DateTime?)null;
                notificacao.Promotor = !txtPromotor.Text.IsNullOrEmptyOrWhiteSpace() ? txtPromotor.Text : null;
                notificacao.UsuarioId = User.Identity.Name;

                validacao = rnNotificacao.Valida(notificacao, Convert.ToInt32(hdnIdade.Value), notificacao.NotificacaoId == 0 ? true : false);

                if (validacao.Valido)
                {
                    if (notificacao.NotificacaoId == 0)
                    {
                        rnNotificacao.Insere(notificacao);
                        lblMensagem.Text = "FICAI cadastrada com sucesso.";
                        txtNumFICAI.Text = notificacao.NumeroFicai.ToString();
                        hdnNotificacaoId.Value = notificacao.NotificacaoId.ToString();

                    }
                    else
                    {
                        rnNotificacao.Atualiza(notificacao);
                        lblMensagem.Text = "FICAI atualizado com sucesso.";
                    }

                    _tipoOperacao = TipoOperacao.Sucesso;
                    ControlarTipoOperacao();

                    if (!hdnNotificacaoId.Value.IsNullOrEmptyOrWhiteSpace())
                    {
                        pcNotificaControle.TabPages[4].Enabled = true;
                        pcNotificaControle.TabPages[5].Enabled = true;
                    }

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

        protected void btnSalvarConselho_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Turmas.OficioConselho rnOficioConselho = new Techne.Lyceum.RN.Turmas.OficioConselho();
                RN.Turmas.Entidades.OficioConselho conselho = new Techne.Lyceum.RN.Turmas.Entidades.OficioConselho();
                RN.Turmas.Notificacao rnNotificacao = new Techne.Lyceum.RN.Turmas.Notificacao();

                conselho.NotificacaoId = !hdnNotificacaoId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnNotificacaoId.Value) : 0;
                conselho.Conselho = !txtAoConselho.Text.IsNullOrEmptyOrWhiteSpace() ? txtAoConselho.Text : null;
                conselho.Cep = !txtCEPOficioConselho.Text.IsNullOrEmptyOrWhiteSpace() ? txtCEPOficioConselho.Text : null;
                conselho.Municipio = !hdnMunicipioOficioConselho.Value.IsNullOrEmptyOrWhiteSpace() ? hdnMunicipioOficioConselho.Value : null;
                conselho.Endereco = !txtEnderecoOficioConselho.Text.IsNullOrEmptyOrWhiteSpace() ? txtEnderecoOficioConselho.Text : null;
                conselho.Numero = !txtEndNumOficioConselho.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndNumOficioConselho.Text : null;
                conselho.Bairro = !txtBairroOficioConselho.Text.IsNullOrEmptyOrWhiteSpace() ? txtBairroOficioConselho.Text : null;
                conselho.UsuarioId = User.Identity.Name;
                conselho.OficioConselhoId = !hdnOficioConselhoId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnOficioConselhoId.Value) : 0;


                validacao = rnOficioConselho.Valida(conselho, txtAluno.Text.ToString(), conselho.OficioConselhoId == 0 ? true : false);

                if (validacao.Valido)
                {
                    if (conselho.OficioConselhoId == 0)
                    {
                        rnOficioConselho.Insere(conselho);
                        lblMensagem.Text = "Ofício Conselho Tutelar cadastrado com sucesso.";

                        hdnOficioConselhoId.Value = conselho.OficioConselhoId.ToString();
                    }
                    else
                    {
                        rnOficioConselho.Atualiza(conselho);
                        lblMensagem.Text = "Ofício Conselho Tutelar atualizado com sucesso.";
                    }

                    if (conselho.OficioConselhoId > 0)
                    {                     
                        if (!dtEncaminhaTutelar.Text.IsNullOrEmptyOrWhiteSpace() && !txtProtocoloConTutelar.Text.IsNullOrEmptyOrWhiteSpace())
                        {
                            PreencheCamposOficioConselho();
                            divPrincipalConselho.Visible = true;
                            pnlImprimirConselho.Visible = true;
                        }
                        else
                        {
                            lblMensagem.Text += " Para exibir o ofício é necessário que no FICAI os dados referente MEDIDAS ADOTADAS PELO CONSELHO TUTELAR estejam preenchidos.";
                        }


                    }

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

        protected void btnSalvarMP_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Turmas.OficioMPRJ rnOficioMPRJ = new Techne.Lyceum.RN.Turmas.OficioMPRJ();
                RN.Turmas.Entidades.OficioMPRJ conselho = new Techne.Lyceum.RN.Turmas.Entidades.OficioMPRJ();
                RN.Turmas.Notificacao rnNotificacao = new Techne.Lyceum.RN.Turmas.Notificacao();

                conselho.NotificacaoId = !hdnNotificacaoId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnNotificacaoId.Value) : 0;
                conselho.Promotoria = !txtPromotoria.Text.IsNullOrEmptyOrWhiteSpace() ? txtPromotoria.Text : null;
                conselho.Cep = !txtCEPOficioMPRJ.Text.IsNullOrEmptyOrWhiteSpace() ? txtCEPOficioMPRJ.Text : null;
                conselho.Municipio = !hdnMunicipioOficioMPRJ.Value.IsNullOrEmptyOrWhiteSpace() ? hdnMunicipioOficioMPRJ.Value : null;
                conselho.Endereco = !txtEnderecoOficioMPRJ.Text.IsNullOrEmptyOrWhiteSpace() ? txtEnderecoOficioMPRJ.Text : null;
                conselho.Numero = !txtEndNumOficioMPRJ.Text.IsNullOrEmptyOrWhiteSpace() ? txtEndNumOficioMPRJ.Text : null;
                conselho.Bairro = !txtBairroOficioMPRJ.Text.IsNullOrEmptyOrWhiteSpace() ? txtBairroOficioMPRJ.Text : null;
                conselho.UsuarioId = User.Identity.Name;
                conselho.OficioMPRJId = !hdnOficioMPRJId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnOficioMPRJId.Value) : 0;

                validacao = rnOficioMPRJ.Valida(conselho, txtAluno.Text.ToString(), conselho.OficioMPRJId == 0 ? true : false);

                if (validacao.Valido)
                {
                    if (conselho.OficioMPRJId == 0)
                    {
                        rnOficioMPRJ.Insere(conselho);
                        lblMensagem.Text = "Ofício MPRJ cadastrado com sucesso.";

                        hdnOficioMPRJId.Value = conselho.OficioMPRJId.ToString();
                    }
                    else
                    {

                        rnOficioMPRJ.Atualiza(conselho);
                        lblMensagem.Text = "Ofício MPRJ atualizado com sucesso.";
                    }

                    if (conselho.OficioMPRJId > 0)
                    {
                        if (!ddlMedidasMinisterio.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                        {
                            PreencheCamposOficioMP();
                            divPrincipalMP.Visible = true;
                            pnlImprimirMP.Visible = true;
                        }
                        else
                        {
                            lblMensagem.Text += " Para exibir o ofício é necessário que no FICAI os dados referente ATUAÇÃO MINISTÉRIO PÚBLICO estejam preenchidos.";
                        }
                    }
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

        private void PreencheCamposOficioConselho()
        {
            RN.Usuarios rnUsuarios = new Usuarios();
            DataTable dt = new DataTable();

            lblNumOficioConselho.Text = txtNumFAMI.Text;

            lblNomeConselho.Text = txtAoConselho.Text;

            lblEnderecoConselho.Text = txtEnderecoOficioConselho.Text + "," + txtEndNumOficioConselho.Text + " - " + txtBairroOficioConselho.Text + " - " + txtMunicipioOficioConselho.Text + " -" + txtEstadoOficioConselho.Value;

            lblNomeAlunoConselhoTitulo.Text = txtNomeCompl.Text;

            lblNomeEscolaConselho.Text = tseUnidade["nome_comp"].ToString();

            lblNomeAlunoConselho.Text = txtNomeCompl.Text;

            lblMatriculaAlunoConselho.Text = txtAluno.Text.ToString();

            lblSerieAlunoConselho.Text = ddlSerie.SelectedItem.Text;

            lblQtdeDiasConselho.Text = txtQtdFaltas.Text;

            var contatos = new List<string>();

            if (!ddlFormaContato1.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                contatos.Add(ddlFormaContato1.SelectedItem.Text);

            if (!ddlFormaContato2.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                contatos.Add(ddlFormaContato2.SelectedItem.Text);

            if (!ddlFormaContato3.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                contatos.Add(ddlFormaContato3.SelectedItem.Text);

            lblTipoContatoConselho.Text = string.Join(", ", contatos.Distinct().ToArray());

            var datasContatos = new List<string>();

            if (!dtData1.Text.IsNullOrEmptyOrWhiteSpace())
                datasContatos.Add(dtData1.Date.ToShortDateString());

            if (!dtData2.Text.IsNullOrEmptyOrWhiteSpace())
                datasContatos.Add(dtData2.Date.ToShortDateString());

            if (!dtData3.Text.IsNullOrEmptyOrWhiteSpace())
                datasContatos.Add(dtData3.Date.ToShortDateString());

            lblDatasContatoFICAIConselho.Text = string.Join(", ", datasContatos.Distinct().ToArray());

            lblDataRegistroFICAI.Text = dtComunicacao.Date.ToShortDateString();

            lblProtocoloFICAI.Text = txtProtocoloConTutelar.Text;

            lblDataRegistroConexaoConselho.Text = hdnDataCadastro.Value;

            dt = rnUsuarios.ObtemDadosUsuario(User.Identity.Name);

            if (dt.Rows.Count > 0)
            {
                lblIDUsuarioConselho.Text = dt.Rows[0]["IDVINCULO"].ToString();
                lblNomeUsuarioConselho.Text = dt.Rows[0]["NOME_COMPL"].ToString();
            }
        }

        private void PreencheCamposOficioMP()
        {
            RN.Usuarios rnUsuarios = new Usuarios();
            DataTable dt = new DataTable();

            lblNomePromotoriaMP.Text = txtPromotoria.Text;

            lblEnderecoMP.Text = txtEnderecoOficioMPRJ.Text + "," + txtEndNumOficioMPRJ.Text + " - " + txtBairroOficioMPRJ.Text + " - " + txtMunicipioOficioMPRJ.Text + " -" + txtEstadoOficioMPRJ.Value;

            lblNomeAlunoMPTitulo.Text = txtNomeCompl.Text;

            lblNomeEscolaMP.Text = tseUnidade["nome_comp"].ToString();

            lblNomeAlunoMP.Text = txtNomeCompl.Text;

            lblMatriculaAlunoMP.Text = txtAluno.Text.ToString();

            lblSerieAlunoMP.Text = ddlSerie.SelectedItem.Text;

            lblQtdeDiasMP.Text = txtQtdFaltas.Text;

            var contatos = new List<string>();

            if (!ddlFormaContato1.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                contatos.Add(ddlFormaContato1.SelectedItem.Text);

            if (!ddlFormaContato2.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                contatos.Add(ddlFormaContato2.SelectedItem.Text);

            if (!ddlFormaContato3.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                contatos.Add(ddlFormaContato3.SelectedItem.Text);

            lblTipoContatoMP.Text = string.Join(", ", contatos.Distinct().ToArray());

            var datasContatos = new List<string>();

            if (!dtData1.Text.IsNullOrEmptyOrWhiteSpace())
                datasContatos.Add(dtData1.Date.ToShortDateString());

            if (!dtData2.Text.IsNullOrEmptyOrWhiteSpace())
                datasContatos.Add(dtData2.Date.ToShortDateString());

            if (!dtData3.Text.IsNullOrEmptyOrWhiteSpace())
                datasContatos.Add(dtData3.Date.ToShortDateString());

            lblDatasContatoFICAIMP.Text = string.Join(", ", datasContatos.Distinct().ToArray());

            lblNomeConselhoTutelar.Text = txtAoConselho.Text;

            lblDataEncaminhamentoConselho.Text = dtEncaminhaTutelar.Date.ToShortDateString();

            lblDataRegistroFICAI.Text = dtComunicacao.Date.ToShortDateString();

            lblDataRegistroConexaoMP.Text = hdnDataCadastro.Value;

            dt = rnUsuarios.ObtemDadosUsuario(User.Identity.Name);

            if (dt.Rows.Count > 0)
            {
                lblIdUsuarioMP.Text = dt.Rows[0]["IDVINCULO"].ToString();
                lblNomeUsuarioMP.Text = dt.Rows[0]["NOME_COMPL"].ToString();
            }
        }

        protected void btnExportarPDFConselho_Click(object sender, ImageClickEventArgs e)
        {
            RN.Util.ExportaPdf exportaPdf = new ExportaPdf();

            try
            {
                //Verifica se dados para exportar já estão montados na tela
                if (divPrincipalConselho.Visible)
                {
                    Image1.Src = HttpContext.Current.Server.MapPath("~/Images/logo_govrj.jpg");
                    Image1.Align = "center";

                    //Cria arquivo com div
                    StringBuilder html = new StringBuilder();
                    StringWriter stringWriter = new StringWriter(html);
                    HtmlTextWriter writer = new HtmlTextWriter(stringWriter);
                    divPrincipalConselho.RenderControl(writer);

                    //Cria css
                    string cssText = File.ReadAllText(HttpContext.Current.Server.MapPath("../LyceumNet.css"));
                    cssText = cssText + File.ReadAllText(HttpContext.Current.Server.MapPath("../Scripts/themes/RelatorioNotificacao.css"));

                    exportaPdf.ExportaHtmlCssPor(html.ToString(), "OfConselhoTutelar_" + txtNomeCompl.Text.ToString() + "_" + String.Format("{0:dd/MM/yyyy}", DateTime.Now), cssText);
                }
                else
                {
                    lblMensagem.Text = "Não existem dados à serem exportados.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnExportarPDFMP_Click(object sender, ImageClickEventArgs e)
        {
            RN.Util.ExportaPdf exportaPdf = new ExportaPdf();

            try
            {
                //Verifica se dados para exportar já estão montados na tela
                if (divPrincipalMP.Visible)
                {
                    Image1.Src = HttpContext.Current.Server.MapPath("~/Images/logo_govrj.jpg");
                    Image1.Align = "center";

                    //Cria arquivo com div
                    StringBuilder html = new StringBuilder();
                    StringWriter stringWriter = new StringWriter(html);
                    HtmlTextWriter writer = new HtmlTextWriter(stringWriter);
                    divPrincipalMP.RenderControl(writer);

                    //Cria css
                    string cssText = File.ReadAllText(HttpContext.Current.Server.MapPath("../LyceumNet.css"));
                    //cssText = cssText + File.ReadAllText(HttpContext.Current.Server.MapPath("../Scripts/themes/RelatorioPatrimonio.css"));

                    exportaPdf.ExportaHtmlCssPor(html.ToString(), "OfMP_" + txtNomeCompl.Text.ToString() + "_" + String.Format("{0:dd/MM/yyyy}", DateTime.Now), cssText);
                }
                else
                {
                    lblMensagem.Text = "Não existem dados à serem exportados.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
