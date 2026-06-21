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

namespace Techne.Lyceum.Net.Interconectividade
{
    [NavUrl("~/Interconectividade/Interrupcao.aspx"), ControlText("Avaliação"), Title("Avaliação")]
    public partial class Avaliacao : TPage
    {
        public object Lista(object avaliacao)
        {
            RN.FiscalizacaoLink.Interrupcao rnInterrupcao = new Techne.Lyceum.RN.FiscalizacaoLink.Interrupcao();

            if (!string.IsNullOrEmpty(avaliacao.ToString()))
            {
                return rnInterrupcao.ListaPor(Convert.ToInt32(avaliacao));
            }
            return null;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                
                if (!IsPostBack)
                {
                    lblMensagemAvaliacao.Text = string.Empty;
                    LimparDadosInterrupcao();
                    pnlGridInterrupcao.Visible = false;
                    pnlInterrupcao.Visible = false;
                    pnlResposta.Visible = false;
                    btnEnviarFaturamento.Visible = false;
                    btnSalvar.Visible = false;
                    Session["idInterrupcao"] = null;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdInterrupcao, "Interrupções");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnSalvar, AcaoControle.novo);
            ControlaAcesso(btnEnviarFaturamento, AcaoControle.novo);
            ControlaAcesso(grdInterrupcao);
        }

        protected void CarregaAno()
        {
            RN.FiscalizacaoLink.ContratoSetor rnContratoSetor = new Techne.Lyceum.RN.FiscalizacaoLink.ContratoSetor();
            if (!this.tseUnidadeAdministrativa.DBValue.IsNull && this.tseUnidadeAdministrativa.IsValidDBValue)
            {
                ddlAno.DataSource = rnContratoSetor.ListaAnosHabilitadosPor(tseUnidadeAdministrativa.DBValue.ToString());
                ddlAno.DataBind();
                ddlAno.Items.Insert(0, new ListItem("Selecione", string.Empty));
            }
        }

        protected void LimparDadosInterrupcao()
        {           
            txtChamado.Text = string.Empty;
            dtInterrupcao.Text = string.Empty;
            dtReestabelecimento.Text = string.Empty;
            txtHoraInterrupcao.Text = string.Empty;
            txtHoraReestabelecimento.Text = string.Empty;
            rblResposta.ClearSelection();
            rblTipoProblema.ClearSelection();
            hdnAvaliacaoId.Value = string.Empty;
            hdnContratoSetorId.Value = string.Empty;
            Session["idInterrupcao"] = null;
            ddlMotivoInterrupcao.ClearSelection();
            txtComplemento.Text = string.Empty;
        }

        protected void rblResposta_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlInterrupcao.Visible = false;            
            txtChamado.Text = string.Empty;
            dtInterrupcao.Text = string.Empty;
            dtReestabelecimento.Text = string.Empty;
            txtHoraInterrupcao.Text = string.Empty;
            txtHoraReestabelecimento.Text = string.Empty;
            rblTipoProblema.ClearSelection();
            txtComplemento.Text = string.Empty;
            ddlMotivoInterrupcao.ClearSelection();

            if (rblResposta.SelectedValue == "1")
            {
                pnlInterrupcao.Visible = true;
            }
        }

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                try
                {
                    ValidacaoDados validacao = new ValidacaoDados();
                    RN.FiscalizacaoLink.Avaliacao rnAvaliacao = new Techne.Lyceum.RN.FiscalizacaoLink.Avaliacao();
                    DateTime dataInterrupcao = new DateTime();
                    DateTime dataReestabelecimento = new DateTime();

                    if (rblResposta.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                    {
                        lblMensagem.Text = "Para salvar é necessário escolher uma resposta.";
                        return;
                    }

                    var avaliacao = new RN.FiscalizacaoLink.Entidades.Avaliacao
                    {
                        SetorId = (this.tseUnidadeAdministrativa.IsValidDBValue && !this.tseUnidadeAdministrativa.DBValue.IsNull) ? tseUnidadeAdministrativa.DBValue.ToString() : null,
                        CircuitoSetorId = (this.tseCircuito.IsValidDBValue && !this.tseCircuito.DBValue.IsNull) ? Convert.ToInt32(tseCircuito["circuitosetorid"]) : -1,
                        Ano = !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlAno.SelectedValue) : -1,
                        Mes = !ddlMes.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlMes.SelectedValue) : -1,
                        Interrupcao = !rblResposta.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblResposta.SelectedValue == "1" ? true : false) : (bool?)null,
                        EnvioFaturamento = false,
                        DataAlteracao = DateTime.Now,
                        UsuarioId = User.Identity.Name,
                    };

                    if (!dtInterrupcao.Text.IsNullOrEmptyOrWhiteSpace() && !txtHoraInterrupcao.Text.Split(':')[0].IsNullOrEmptyOrWhiteSpace() && !txtHoraInterrupcao.Text.Split(':')[0].IsNullOrEmptyOrWhiteSpace())
                    {
                        dataInterrupcao = new DateTime(dtInterrupcao.Date.Year, dtInterrupcao.Date.Month, dtInterrupcao.Date.Day, Convert.ToInt32(txtHoraInterrupcao.Text.Split(':')[0]), Convert.ToInt32(txtHoraInterrupcao.Text.Split(':')[1]), 0);
                    }
                    if (!dtReestabelecimento.Text.IsNullOrEmptyOrWhiteSpace() && !txtHoraReestabelecimento.Text.Split(':')[0].IsNullOrEmptyOrWhiteSpace() && !txtHoraReestabelecimento.Text.Split(':')[0].IsNullOrEmptyOrWhiteSpace())
                    {
                        dataReestabelecimento = new DateTime(dtReestabelecimento.Date.Year, dtReestabelecimento.Date.Month, dtReestabelecimento.Date.Day, Convert.ToInt32(txtHoraReestabelecimento.Text.Split(':')[0]), Convert.ToInt32(txtHoraReestabelecimento.Text.Split(':')[1]), 0);
                    }
                    var interrupcao = new RN.FiscalizacaoLink.Entidades.Interrupcao
                    {                        
                        AvaliacaoId = !hdnAvaliacaoId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnAvaliacaoId.Value) : 0,
                        Chamado = !txtChamado.Text.IsNullOrEmptyOrWhiteSpace() ? txtChamado.Text.Trim() : null,
                        MotivoInterrupcaoId = !ddlMotivoInterrupcao.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlMotivoInterrupcao.SelectedValue) : -1,
                        MotivoComplemento = !txtComplemento.Text.IsNullOrEmptyOrWhiteSpace() ? txtComplemento.Text.Trim() : null,                       
                        DataInterrupcao = dataInterrupcao != DateTime.MinValue ? dataInterrupcao : DateTime.MinValue,
                        DataReestabelecimento = dataReestabelecimento != DateTime.MinValue ? dataReestabelecimento : DateTime.MinValue,
                        TipoProblema = !rblTipoProblema.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblTipoProblema.SelectedValue : string.Empty,
                        UsuarioId = User.Identity.Name,

                    };

                    if (hdnAvaliacaoId.Value.IsNullOrEmptyOrWhiteSpace())
                    {
                        validacao = rnAvaliacao.ValidaInsercao(avaliacao, interrupcao);
                    }
                    else
                    {
                        avaliacao.AvaliacaoId = Convert.ToInt32(hdnAvaliacaoId.Value);
                        validacao = rnAvaliacao.ValidaAtualizacao(avaliacao, interrupcao);
                    }

                    if (validacao.Valido)
                    {
                        if (hdnAvaliacaoId.Value.IsNullOrEmptyOrWhiteSpace())
                        {
                            rnAvaliacao.Insere(avaliacao, interrupcao);
                            hdnAvaliacaoId.Value = avaliacao.AvaliacaoId.ToString();
                        }
                        else
                        {
                            rnAvaliacao.Atualiza(avaliacao, interrupcao);
                        }

                        hdnRespostaAvaliacao.Value = avaliacao.Interrupcao != null ? ((bool)avaliacao.Interrupcao ? "1" : "0") : string.Empty;
                        lblMensagemAvaliacao.Text = string.Format("Foi declarada em {0} às {1} por {2} - {3}, que {4} interrupção do serviço.", avaliacao.DataAlteracao.ToShortDateString(), avaliacao.DataAlteracao.ToShortTimeString(), avaliacao.UsuarioId, RN.Usuarios.BuscaNome(avaliacao.UsuarioId), (bool)avaliacao.Interrupcao ? "HOUVE" : "NÃO HOUVE");
                    
                        grdInterrupcao.DataBind();
                        pnlGridInterrupcao.Visible = true;                       
                        txtChamado.Text = string.Empty;
                        dtInterrupcao.Text = string.Empty;
                        dtReestabelecimento.Text = string.Empty;
                        txtHoraInterrupcao.Text = string.Empty;
                        txtHoraReestabelecimento.Text = string.Empty;
                        ddlMotivoInterrupcao.ClearSelection();
                        txtComplemento.Text = string.Empty;
                        rblResposta.ClearSelection();
                        rblTipoProblema.ClearSelection();
                        pnlInterrupcao.Visible = false;

                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup",
                          "alert('Avaliação atualizada com sucesso.');", true);

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
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnEnviarFaturamento_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                try
                {
                    ValidacaoDados validacao = new ValidacaoDados();
                    RN.FiscalizacaoLink.Avaliacao rnAvaliacao = new Techne.Lyceum.RN.FiscalizacaoLink.Avaliacao();
                    DateTime dataInterrupcao = new DateTime();
                    DateTime dataReestabelecimento = new DateTime();
                    bool? respostaAvaliacao = null;
                    lblMensagem.Text = string.Empty;

                    if (rblResposta.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                    {
                        if (!hdnRespostaAvaliacao.Value.IsNullOrEmptyOrWhiteSpace())
                        {
                            respostaAvaliacao = (hdnRespostaAvaliacao.Value == "1" ? true : false);
                        }                        
                    }

                    var avaliacao = new RN.FiscalizacaoLink.Entidades.Avaliacao
                    {
                        SetorId = (this.tseUnidadeAdministrativa.IsValidDBValue && !this.tseUnidadeAdministrativa.DBValue.IsNull) ? tseUnidadeAdministrativa.DBValue.ToString() : null,
                        CircuitoSetorId = (this.tseCircuito.IsValidDBValue && !this.tseCircuito.DBValue.IsNull) ? Convert.ToInt32(tseCircuito["circuitosetorid"]) : -1,
                        Ano = !ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlAno.SelectedValue) : -1,
                        Mes = !ddlMes.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlMes.SelectedValue) : -1,
                        Interrupcao = !rblResposta.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? (rblResposta.SelectedValue == "1" ? true : false) : respostaAvaliacao,
                        EnvioFaturamento = true,
                        DataEnvioFaturamento = DateTime.Now,
                        UsuarioId = User.Identity.Name,
                    };

                    if (!dtInterrupcao.Text.IsNullOrEmptyOrWhiteSpace() && !txtHoraInterrupcao.Text.Split(':')[0].IsNullOrEmptyOrWhiteSpace() && !txtHoraInterrupcao.Text.Split(':')[0].IsNullOrEmptyOrWhiteSpace())
                    {
                        dataInterrupcao = new DateTime(dtInterrupcao.Date.Year, dtInterrupcao.Date.Month, dtInterrupcao.Date.Day, Convert.ToInt32(txtHoraInterrupcao.Text.Split(':')[0]), Convert.ToInt32(txtHoraInterrupcao.Text.Split(':')[1]), 0);
                    }
                    if (!dtReestabelecimento.Text.IsNullOrEmptyOrWhiteSpace() && !txtHoraReestabelecimento.Text.Split(':')[0].IsNullOrEmptyOrWhiteSpace() && !txtHoraReestabelecimento.Text.Split(':')[0].IsNullOrEmptyOrWhiteSpace())
                    {
                        dataReestabelecimento = new DateTime(dtReestabelecimento.Date.Year, dtReestabelecimento.Date.Month, dtReestabelecimento.Date.Day, Convert.ToInt32(txtHoraReestabelecimento.Text.Split(':')[0]), Convert.ToInt32(txtHoraReestabelecimento.Text.Split(':')[1]), 0);
                    }
                    var interrupcao = new RN.FiscalizacaoLink.Entidades.Interrupcao
                    {
                        AvaliacaoId = !hdnAvaliacaoId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnAvaliacaoId.Value) : 0,
                        Chamado = !txtChamado.Text.IsNullOrEmptyOrWhiteSpace() ? txtChamado.Text.Trim() : null,
                        DataInterrupcao = dataInterrupcao != DateTime.MinValue ? dataInterrupcao : DateTime.MinValue,
                        DataReestabelecimento = dataReestabelecimento != DateTime.MinValue ? dataReestabelecimento : DateTime.MinValue,
                        TipoProblema = !rblTipoProblema.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblTipoProblema.SelectedValue : string.Empty,
                        UsuarioId = User.Identity.Name,

                    };

                    if (hdnAvaliacaoId.Value.IsNullOrEmptyOrWhiteSpace())
                    {
                        validacao = rnAvaliacao.ValidaInsercao(avaliacao, interrupcao);
                    }
                    else
                    {
                        avaliacao.AvaliacaoId = Convert.ToInt32(hdnAvaliacaoId.Value);
                        validacao = rnAvaliacao.ValidaAtualizacao(avaliacao, interrupcao);
                    }
                    if (validacao.Valido)
                    {
                        if (hdnAvaliacaoId.Value.IsNullOrEmptyOrWhiteSpace())
                        {
                            rnAvaliacao.Insere(avaliacao, interrupcao);
                        }
                        else
                        {
                            rnAvaliacao.Atualiza(avaliacao, interrupcao);
                        }

                        btnSalvar.Visible = false;
                        btnEnviarFaturamento.Visible = false;
                        pnlResposta.Visible = false;
                        pnlInterrupcao.Visible = false;
                        grdInterrupcao.DataBind();
                        txtChamado.Text = string.Empty;
                        dtInterrupcao.Text = string.Empty;
                        dtReestabelecimento.Text = string.Empty;
                        txtHoraInterrupcao.Text = string.Empty;
                        txtHoraReestabelecimento.Text = string.Empty;
                        rblResposta.ClearSelection();
                        rblTipoProblema.ClearSelection();

                        lblMensagemAvaliacao.Text = string.Format("Foi enviada para faturamento  em {0} às {1} por {2} - {3}, {4} interrupção(ões) do serviço.", Convert.ToDateTime(avaliacao.DataEnvioFaturamento).ToShortDateString(), Convert.ToDateTime(avaliacao.DataEnvioFaturamento).ToShortTimeString(), avaliacao.UsuarioId, RN.Usuarios.BuscaNome(avaliacao.UsuarioId), (bool)avaliacao.Interrupcao ? "COM" : "SEM");


                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup",
                            "alert('Avaliação enviada com sucesso.');", true);

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
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeAdministrativa_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                pnlGridInterrupcao.Visible = false;
                pnlInterrupcao.Visible = false;
                pnlResposta.Visible = false;
                tseContrato.ResetValue();
                tseCircuito.ResetValue();
                ddlAno.Items.Clear();
                ddlMes.Items.Clear();
                LimparDadosInterrupcao();
                lblMensagemAvaliacao.Text = string.Empty;
                btnSalvar.Visible = false;
                btnEnviarFaturamento.Visible = false;

                if (!this.tseUnidadeAdministrativa.DBValue.IsNull)
                {
                    if (!this.tseUnidadeAdministrativa.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Unidade Administrativa não cadastrada.";
                    }
                    else
                    {
                        CarregaAno();
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar uma unidade administrativa.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RN.FiscalizacaoLink.ContratoSetor rnContratoSetor = new Techne.Lyceum.RN.FiscalizacaoLink.ContratoSetor();
                tseContrato.ResetValue();
                tseCircuito.ResetValue();
                LimparDadosInterrupcao();
                ddlMes.Items.Clear();
                tseContrato.ResetValue();
                tseCircuito.ResetValue();
                pnlGridInterrupcao.Visible = false;
                pnlInterrupcao.Visible = false;
                pnlResposta.Visible = false;
                lblMensagemAvaliacao.Text = string.Empty;
                btnSalvar.Visible = false;
                btnEnviarFaturamento.Visible = false;

                if (!ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    ddlMes.DataSource = rnContratoSetor.ListaMesesHabilitadosPor(tseUnidadeAdministrativa.DBValue.ToString(), Convert.ToInt32(ddlAno.SelectedValue));
                    ddlMes.DataBind();
                    ddlMes.Items.Insert(0, new ListItem("Selecione", string.Empty));
 
                    string ano = ddlAno.SelectedValue;

                    //Filtra contratos e circuitos do ano / mes
                    tseContrato.ResetValue();
                    tseContrato.SqlWhere = " SETORID = #tseUnidadeAdministrativa# and year(CS.DATAIMPLANTACAO) <= " + ano + " and (CS.DATATERMINO is null or (CS.DATATERMINO is not null and year(CS.DATATERMINO) >= " + ano + ")) ";
                    tseCircuito.ResetValue();
                    tseCircuito.SqlWhere = "  numero = #tseContrato# AND SETORID = #tseUnidadeAdministrativa# and year(INICIO) <= " + ano + " and (FIM is null or (FIM is not null and year(FIM) >= " + ano + "))  ";
                }

                tseContrato.DataBind();
                tseCircuito.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlMes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tseContrato.ResetValue();
                tseCircuito.ResetValue();
                LimparDadosInterrupcao();
                pnlGridInterrupcao.Visible = false;
                pnlInterrupcao.Visible = false;
                pnlResposta.Visible = false;
                lblMensagemAvaliacao.Text = string.Empty;
                btnSalvar.Visible = false;
                btnEnviarFaturamento.Visible = false;

                if (!ddlMes.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    string ano = ddlAno.SelectedValue;
                    string mes = ddlMes.SelectedValue;
                    
                    //Filtra contratos e circuitos do ano / mes
                    tseContrato.ResetValue();
                    tseContrato.SqlWhere = " SETORID= #tseUnidadeAdministrativa# and year(CS.DATAIMPLANTACAO) <= " + ano + " and (CS.DATATERMINO is null or (CS.DATATERMINO is not null and year(CS.DATATERMINO) >= " + ano + ")) and (year(CS.DATAIMPLANTACAO) < " + ano + " or (year(CS.DATAIMPLANTACAO) = " + ano + " and month(CS.DATAIMPLANTACAO) <=  " + mes + ")) AND (CS.DATATERMINO is null OR year(CS.DATATERMINO) > " + ano + " OR (year(CS.DATATERMINO) = " + ano + " AND month(CS.DATATERMINO) >=  " + mes + ")) ";
                    tseCircuito.ResetValue();
                    tseCircuito.SqlWhere = "  numero = #tseContrato# AND SETORID= #tseUnidadeAdministrativa# and year(INICIO) <=  " + ano + " and (FIM is null or (FIM is not null and year(FIM) >=  " + ano + ")) and (year(INICIO) <  " + ano + " or (year(INICIO) =  " + ano + " and month(INICIO) <= " + mes + ")) AND (FIM is null OR year(FIM) >  " + ano + " OR (year(FIM) =  " + ano + " AND month(FIM) >= " + mes + "))  ";
                }

                tseContrato.DataBind();
                tseCircuito.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseContrato_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                tseCircuito.ResetValue();
                LimparDadosInterrupcao();
                pnlGridInterrupcao.Visible = false;
                pnlInterrupcao.Visible = false;
                pnlResposta.Visible = false;
                lblMensagemAvaliacao.Text = string.Empty;
                btnSalvar.Visible = false;
                btnEnviarFaturamento.Visible = false;

                if (!this.tseContrato.DBValue.IsNull)
                {
                    if (!this.tseContrato.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Contrato não cadastrado.";
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar um contrato.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseCircuito_Changed(object sender, ChangedEventArgs args)
        {
            RN.FiscalizacaoLink.Avaliacao rnAvaliacao = new Techne.Lyceum.RN.FiscalizacaoLink.Avaliacao();
            RN.FiscalizacaoLink.Entidades.Avaliacao avaliacao = new Techne.Lyceum.RN.FiscalizacaoLink.Entidades.Avaliacao();

            try
            {
                LimparDadosInterrupcao();
                pnlGridInterrupcao.Visible = false;
                pnlInterrupcao.Visible = false;
                pnlResposta.Visible = false;
                lblMensagemAvaliacao.Text = string.Empty;
                btnSalvar.Visible = false;
                btnEnviarFaturamento.Visible = false;

                if (!this.tseCircuito.DBValue.IsNull)
                {
                    if (!this.tseCircuito.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Circuito não cadastrado.";
                    }
                    else
                    {
                        if (!ddlMes.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                        {
                            pnlResposta.Visible = true;
                            btnSalvar.Visible = true;
                            btnEnviarFaturamento.Visible = false; //Não tem mais necessidade dessa funcionalidade
                            CarregaMotivoInterrupcao();

                            avaliacao = rnAvaliacao.ObtemPor(Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlMes.SelectedValue), tseUnidadeAdministrativa.DBValue.ToString(), Convert.ToInt32(tseCircuito["circuitosetorid"]));

                            if (avaliacao.AvaliacaoId > 0)
                            {
                                pnlGridInterrupcao.Visible = true;
                                hdnRespostaAvaliacao.Value = avaliacao.Interrupcao != null ? ((bool)avaliacao.Interrupcao ? "1" : "0") : string.Empty;
                                hdnAvaliacaoId.Value = avaliacao.AvaliacaoId.ToString();
                                grdInterrupcao.DataBind();
                                if (!avaliacao.EnvioFaturamento)
                                {
                                    lblMensagemAvaliacao.Text = string.Format("Foi declarada em {0} às {1} por {2} - {3}, que {4} interrupção do serviço.", avaliacao.DataAlteracao.ToShortDateString(), avaliacao.DataAlteracao.ToShortTimeString(), avaliacao.UsuarioId, RN.Usuarios.BuscaNome(avaliacao.UsuarioId), (bool)avaliacao.Interrupcao ? "HOUVE" : "NÃO HOUVE");
                                }
                                else
                                {
                                    btnSalvar.Visible = false;
                                    btnEnviarFaturamento.Visible = false;
                                    pnlResposta.Visible = false;
                                    lblMensagemAvaliacao.Text = string.Format("Foi enviada para faturamento  em {0} às {1} por {2} - {3}, {4} interrupção(ões) do serviço.", Convert.ToDateTime(avaliacao.DataEnvioFaturamento).ToShortDateString(), Convert.ToDateTime(avaliacao.DataEnvioFaturamento).ToShortTimeString(), avaliacao.UsuarioId, RN.Usuarios.BuscaNome(avaliacao.UsuarioId), (bool)avaliacao.Interrupcao ? "COM" : "SEM");
                                }
                            }
                        }
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar um circuito.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdInterrupcao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdInterrupcao);
        }

        protected void grdInterrupcao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdInterrupcao.Settings.ShowFilterRow = false;
        }

        protected void grdInterrupcao_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdInterrupcao.Settings.ShowFilterRow = false;
        }

        protected void grdInterrupcao_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdInterrupcao.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "INTERRUPCAOID")
                {
                    e.Editor.Enabled = true;
                }
                if ((e.Column.FieldName) == "CHAMADO")
                {
                    e.Editor.ReadOnly = false;
                }
                if ((e.Column.FieldName) == "DATAINTERRUPCAO")
                {
                    e.Editor.ReadOnly = false;
                }
                if ((e.Column.FieldName) == "DATAREESTABELECIMENTO")
                {
                    e.Editor.ReadOnly = false;
                }
                if ((e.Column.FieldName) == "TIPOPROBLEMA")
                {
                    e.Editor.ReadOnly = false;
                }
            }
            else if (grdInterrupcao.IsEditing)
            {
                if ((e.Column.FieldName) == "INTERRUPCAOID")
                {
                    e.Editor.Enabled = false;
                }
                if ((e.Column.FieldName) == "CHAMADO")
                {
                    e.Editor.ReadOnly = true;
                    e.Editor.Enabled = false;
                }
                if ((e.Column.FieldName) == "DATAINTERRUPCAO")
                {
                    e.Editor.ReadOnly = false;
                }
                if ((e.Column.FieldName) == "DATAREESTABELECIMENTO")
                {
                    e.Editor.ReadOnly = false;
                }
                if ((e.Column.FieldName) == "TIPOPROBLEMA")
                {
                    e.Editor.ReadOnly = false;
                }
            }
        }

        public void Update(object CHAMADO, object DATAINTERRUPCAO, object DATAREESTABELECIMENTO, object TIPOPROBLEMA, object INTERRUPCAOID)
        {
        }

        public void Delete(object INTERRUPCAOID)
        { }

        protected void grdInterrupcao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {

            ValidacaoDados validacao = new ValidacaoDados();
            RN.FiscalizacaoLink.Interrupcao rnInterrupcao = new Techne.Lyceum.RN.FiscalizacaoLink.Interrupcao();
            DateTime dataInterrupcao = new DateTime();
            DateTime dataReestabelecimento = new DateTime();

            var circuitoSetorId = ((ASPxGridView)sender).GetRowValuesByKeyValue(e.Keys[0], "CIRCUITOSETORID");
            var avaliacaoId = ((ASPxGridView)sender).GetRowValuesByKeyValue(e.Keys[0], "AVALIACAOID");
            var chamado = ((ASPxGridView)sender).GetRowValuesByKeyValue(e.Keys[0], "CHAMADO");

            if (e.NewValues["DATAINTERRUPCAO"] != null && e.NewValues["HORAINTERRUPCAO"] != null)
            {
                dataInterrupcao = new DateTime(Convert.ToDateTime(e.NewValues["DATAINTERRUPCAO"]).Year, Convert.ToDateTime(e.NewValues["DATAINTERRUPCAO"]).Month, Convert.ToDateTime(e.NewValues["DATAINTERRUPCAO"]).Day, Convert.ToInt32(e.NewValues["HORAINTERRUPCAO"].ToString().Split(':')[0]), Convert.ToInt32(e.NewValues["HORAINTERRUPCAO"].ToString().Split(':')[1]), 0);
            }
            if (e.NewValues["DATAREESTABELECIMENTO"] != null && e.NewValues["HORAREESTABELECIMENTO"] != null)
            {
                dataReestabelecimento = new DateTime(Convert.ToDateTime(e.NewValues["DATAREESTABELECIMENTO"]).Year, Convert.ToDateTime(e.NewValues["DATAREESTABELECIMENTO"]).Month, Convert.ToDateTime(e.NewValues["DATAREESTABELECIMENTO"]).Day, Convert.ToInt32(e.NewValues["HORAREESTABELECIMENTO"].ToString().Split(':')[0]), Convert.ToInt32(e.NewValues["HORAREESTABELECIMENTO"].ToString().Split(':')[1]), 0);
            }

            RN.FiscalizacaoLink.Entidades.Interrupcao Interrupcao = new Techne.Lyceum.RN.FiscalizacaoLink.Entidades.Interrupcao
            {
                InterrupcaoId = Convert.ToInt32(e.Keys["INTERRUPCAOID"]),
                AvaliacaoId = Convert.ToInt32(avaliacaoId),
                Chamado = chamado.ToString(),
                MotivoInterrupcaoId = e.NewValues["MOTIVOINTERRUPCAOID"] != null ? Convert.ToInt32(e.NewValues["MOTIVOINTERRUPCAOID"]) : -1,
                MotivoComplemento = e.NewValues["MOTIVOCOMPLEMENTO"] != null ? e.NewValues["MOTIVOCOMPLEMENTO"].ToString() : null,
                TipoProblema = e.NewValues["TIPOPROBLEMA"] != null ? e.NewValues["TIPOPROBLEMA"].ToString() : null,
                DataInterrupcao = dataInterrupcao != DateTime.MinValue ? dataInterrupcao : DateTime.MinValue,
                DataReestabelecimento = dataReestabelecimento != DateTime.MinValue ? dataReestabelecimento : DateTime.MinValue,
                UsuarioId = this.User.Identity.Name
            };


            validacao = rnInterrupcao.Valida(Interrupcao, Convert.ToInt32(ddlAno.SelectedValue), Convert.ToInt32(ddlMes.SelectedValue), false, Convert.ToInt32(circuitoSetorId)
);

            if (validacao.Valido)
            {
                rnInterrupcao.Atualiza(Interrupcao);
                e.Cancel = true;
                this.grdInterrupcao.CancelEdit();
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void grdInterrupcao_Init(object sender, EventArgs e)
        {
            ASPxGridView gridView = sender as ASPxGridView;
            gridView.JSProperties["cpShowDeleteConfirmBox"] = false;
        }

        protected void grdInterrupcao_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.FiscalizacaoLink.Interrupcao rnInterrupcao = new Techne.Lyceum.RN.FiscalizacaoLink.Interrupcao();
            var avaliacaoId = ((ASPxGridView)sender).GetRowValuesByKeyValue(e.Keys[0], "AVALIACAOID");
            Session["idInterrupcao"] = null;

            if (rnInterrupcao.ObtemQuantidadeInterrupcoesPor(Convert.ToInt32(avaliacaoId)) == 1)
            {
                grdInterrupcao.JSProperties["cpShowDeleteConfirmBox"] = true;
                hdnAvaliacaoId.Value = avaliacaoId.ToString();
                Session["idInterrupcao"] = e.Keys["INTERRUPCAOID"].ToString();
            }
            else
            {
                validacao = rnInterrupcao.ValidaRemocao(Convert.ToInt32(e.Keys["INTERRUPCAOID"]), Convert.ToInt32(avaliacaoId), User.Identity.Name);

                if (validacao.Valido)
                {
                    rnInterrupcao.Remove(Convert.ToInt32(e.Keys["INTERRUPCAOID"]), Convert.ToInt32(avaliacaoId), User.Identity.Name);
                    e.Cancel = true;
                    this.grdInterrupcao.CancelEdit();
                }
                else
                {
                    throw new Exception(validacao.Mensagem);
                }        
            }
        }

        protected void btnConfirmarExclusao_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.FiscalizacaoLink.Interrupcao rnInterrupcao = new Techne.Lyceum.RN.FiscalizacaoLink.Interrupcao();
                this.pucConfirm.ShowOnPageLoad = false;

                if (Session["idInterrupcao"] != null)
                {
                    validacao = rnInterrupcao.ValidaRemocao(Convert.ToInt32(Session["idInterrupcao"]), Convert.ToInt32(hdnAvaliacaoId.Value), User.Identity.Name);

                    if (validacao.Valido)
                    {
                        rnInterrupcao.Remove(Convert.ToInt32(Session["idInterrupcao"]), Convert.ToInt32(hdnAvaliacaoId.Value), User.Identity.Name);
                        grdInterrupcao.DataBind();
                        ddlMes_SelectedIndexChanged(null, null);
                    }
                    else
                    {
                        this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    }
                    grdInterrupcao.CancelEdit();
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.pucConfirm.ShowOnPageLoad = false;
            grdInterrupcao.CancelEdit();
        }

        protected void grdInterrupcao_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            var enviado = (bool)this.grdInterrupcao.GetRowValues(e.VisibleIndex, "ENVIOFATURAMENTO");

            if (enviado)
            {
                e.Visible = false;
            }
        }


        private void CarregaMotivoInterrupcao()
        {
            RN.FiscalizacaoLink.MotivoInterrupcao rnMotivoInterrupcao = new Techne.Lyceum.RN.FiscalizacaoLink.MotivoInterrupcao();
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlMotivoInterrupcao.Items.Clear();
            ddlMotivoInterrupcao.DataSource = rnMotivoInterrupcao.ListaMotivoInterrupcaoAtiva();
            ddlMotivoInterrupcao.DataBind();
            ddlMotivoInterrupcao.Items.Insert(0, item);
        }
    }
}
