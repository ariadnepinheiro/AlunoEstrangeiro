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
using DevExpress.Web.ASPxClasses;

namespace Techne.Lyceum.Net.Transporte
{
    [NavUrl("~/Transporte/ListarPagamento.aspx")]
    [ControlText("Listar Pagamentos")]
    [Title("Listar Pagamentos")]

    public partial class ListarPagamento : TPage
    {
        public enum TipoOperacao
        {
            Novo,
            Cancelar,
            Consultar,
            Inicial
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

        private void ControlarTipoOperacao()
        {
            RN.Transporte.Entidades.Prestador prestador = new Techne.Lyceum.RN.Transporte.Entidades.Prestador();
            RN.Transporte.Prestador rnPrestador = new Techne.Lyceum.RN.Transporte.Prestador();

            switch (this._tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { };
                        ControlarVisibilidadeControle(controles, null);
                        tseRegional.ResetValue();
                        tseMunicipio.ResetValue();
                        tseUnidadeResponsavel.ResetValue();
                        pnlDadosNovo.Visible = false;
                        dtDataInicio.Text = string.Empty;
                        dtDataFim.Text = string.Empty;
                        pnlGrid.Visible = false;
                        break;
                    }

                case TipoOperacao.Novo:
                    {
                        ImageButton[] controles = new ImageButton[] { btnCancel };
                        Button[] controlesButton = new[] { btnProsseguir };

                        ControlarVisibilidadeControle(controles, controlesButton);

                        dtDataInicio.Text = string.Empty;
                        dtDataFim.Text = string.Empty;
                        pnlDadosNovo.Visible = true;
                        break;
                    }
                case TipoOperacao.Consultar:
                    {
                        var controles = new[] { btnNovo };
                        ControlarVisibilidadeControle(controles, null);
                        pnlDadosNovo.Visible = false;
                        dtDataInicio.Text = string.Empty;
                        dtDataFim.Text = string.Empty;
                        pnlGrid.Visible = true;

                        break;
                    }
                case TipoOperacao.Cancelar:
                    {
                        ImageButton[] controles = new ImageButton[] { btnNovo };
                        ControlarVisibilidadeControle(controles, null);
                        tseRegional.ResetValue();
                        tseMunicipio.ResetValue();
                        tseUnidadeResponsavel.ResetValue();
                        pnlDadosNovo.Visible = false;
                        dtDataInicio.Text = string.Empty;
                        dtDataFim.Text = string.Empty;
                        pnlGrid.Visible = false;

                        break;
                    }

            }

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

            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(btnProsseguir, AcaoControle.novo);

        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnNovo.Visible = false;
            btnProsseguir.Visible = false;

        }

        public object Listar(object unidade)
        {
            RN.Transporte.Pagamento rnPagamento = new Techne.Lyceum.RN.Transporte.Pagamento();

            if (!string.IsNullOrEmpty(unidade.ToString()))
            {
                return rnPagamento.ListaPor(Convert.ToString(unidade));
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
                    PreencherDadosSession();

                    if (Request.QueryString.Keys.Count > 0)
                    {
                        _tipoOperacao = TipoOperacao.Consultar;
                    }
                    else
                    {
                        _tipoOperacao = TipoOperacao.Inicial;
                        
                    }
                    ControlarTipoOperacao();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void PreencherDadosSession()
        {
            RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();

            if (sessao != null)
            {
                if (!string.IsNullOrEmpty(sessao.Regional))
                {
                    tseRegional.DBValue = sessao.Regional;
                    if (!tseRegional.IsValidDBValue)
                    {
                        tseRegional.Msg = string.Empty;
                        tseRegional.ResetValue();
                    }
                }
                if (!string.IsNullOrEmpty(sessao.Municipio))
                {
                    tseMunicipio.DBValue = sessao.Municipio;
                    if (!tseMunicipio.IsValidDBValue)
                    {
                        tseMunicipio.Msg = string.Empty;
                        tseMunicipio.ResetValue();
                    }
                }
                if (!string.IsNullOrEmpty(sessao.Escola))
                {
                    tseUnidadeResponsavel.DBValue = sessao.Escola;                 

                    if (!tseUnidadeResponsavel.IsValidDBValue)
                    {
                        tseUnidadeResponsavel.Msg = string.Empty;
                        tseUnidadeResponsavel.ResetValue();
                    }
                }
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdPagamento, "Pagamentos Efetuados");

        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(btnProsseguir, AcaoControle.novo);
        }


        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }
                var sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (sessao != null)
                {
                    if (!this.tseRegional.DBValue.IsNull)
                    {
                        if (this.tseRegional.IsValidDBValue)
                        {
                            sessao.Regional = Convert.ToString(tseRegional.DBValue);
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;

                            tseMunicipio.ResetValue();
                            tseUnidadeResponsavel.ResetValue();
                        }
                        else
                        {
                            sessao.Regional = string.Empty;
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;
                        }
                    }
                    else
                    {
                        sessao.Regional = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                        tseMunicipio.ResetValue();
                        tseUnidadeResponsavel.ResetValue();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
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

                var sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (sessao != null)
                {
                    if (!this.tseMunicipio.DBValue.IsNull)
                    {
                        if (this.tseMunicipio.IsValidDBValue)
                        {
                            sessao.Municipio = Convert.ToString(tseMunicipio.DBValue);
                            sessao.Escola = string.Empty;
                        }
                        else
                        {
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;
                            tseUnidadeResponsavel.ResetValue();
                        }
                    }
                    else
                    {
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                        tseUnidadeResponsavel.ResetValue();
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
                if (this.Page.IsCallback)
                {
                    return;
                }

                var sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (!this.tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (this.tseUnidadeResponsavel.IsValidDBValue)
                    {
                        if (!this.tseUnidadeResponsavel["unidade_ens"].IsNull)
                        {
                            this.tseRegional.Value = this.tseUnidadeResponsavel["id_regional"];
                            this.tseMunicipio.Value = this.tseUnidadeResponsavel["municipio"];
                        }

                        if (sessao != null)
                        {
                            sessao.Escola = Convert.ToString(this.tseUnidadeResponsavel.DBValue);
                            sessao.Regional = Convert.ToString(this.tseRegional.DBValue);
                            sessao.Municipio = Convert.ToString(this.tseMunicipio.DBValue);
                        }

                        _tipoOperacao = TipoOperacao.Consultar;


                    }
                    else
                    {
                        lblMensagem.Text = "Unidade de ensino não encontrada.";
                        if (sessao != null)
                        {
                            sessao.Escola = string.Empty;
                            sessao.Municipio = string.Empty;
                            sessao.Regional = string.Empty;
                        }
                        _tipoOperacao = TipoOperacao.Inicial;
                    }
                }
                else
                {
                    if (sessao != null)
                    {
                        sessao.Escola = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Regional = string.Empty;
                    }
                    lblMensagem.Text = "Favor consultar uma unidade de ensino.";
                    _tipoOperacao = TipoOperacao.Inicial;
                }

                ControlarTipoOperacao();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
            }
        }

        private string MontarQueryString(string tipoOperacao, string idPagamento, string dataInicio, string dataFim, string valorTotal)
        {
            string queryString = string.Empty;

            queryString += "tipoOperacao=" + tipoOperacao;
            queryString += "&codmuc=" + tseMunicipio.DBValue.ToString();
            queryString += "&nomemunicipio=" + tseMunicipio["nome"].ToString();
            queryString += "&censo=" + tseUnidadeResponsavel.DBValue.ToString();
            queryString += "&escola=" + tseUnidadeResponsavel["nome_comp"].ToString();
            queryString += "&Inicio=" + dataInicio;
            queryString += "&Fim=" + dataFim;
            queryString += "&idPagamento=" + idPagamento;
            queryString += "&valorTotal=" + valorTotal;

            return queryString;
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

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Cancelar;
                ControlarTipoOperacao();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnProsseguir_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(dtDataInicio.Text) && !string.IsNullOrEmpty(dtDataFim.Text))
                {
                    string queryString = MontarQueryString(Enum.GetName(typeof(TipoOperacaoEnum), TipoOperacaoEnum.NOVO), string.Empty,dtDataInicio.Text,dtDataFim.Text,string.Empty);
                    byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                    Response.Redirect("Pagamento.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
                }
                else
                {
                    lblMensagem.Text = "Para efetuar o lançamento do pagamento é necessário preencher o período.<br> Data de Início e Fim são campos obrigatórios.";
                }


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
                string tipoOperacao = string.Empty;

                string pagamentoId = e.CommandArgument.ToString();
                string dataInicio = Convert.ToDateTime(grdPagamento.GetRowValuesByKeyValue(pagamentoId, "DATAINICIO")).ToString("dd/MM/yyyy");
                string dataFim = Convert.ToDateTime(grdPagamento.GetRowValuesByKeyValue(pagamentoId, "DATAFIM")).ToString("dd/MM/yyyy");
                string valorTotal = string.Format("{0:N2}", grdPagamento.GetRowValuesByKeyValue(pagamentoId, "VALORTOTAL"));

                tipoOperacao = Enum.GetName(typeof(TipoOperacaoEnum), TipoOperacaoEnum.CONSULTAR);

                string queryString = MontarQueryString(tipoOperacao, pagamentoId,dataInicio,dataFim, valorTotal);
                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                Response.Redirect("Pagamento.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

    }
}
