using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.Data;
using Techne.Lyceum.RN.PrestacaoContas;
using Techne.Lyceum.RN.PrestacaoContas.DTO;
using Techne.Lyceum.RN.PrestacaoContas.DTOs;
using Techne.Lyceum.RN.Util;
using Techne.Web;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [NavUrl("~/PrestacaoContas/AnalisarEvento.aspx"), ControlText("Analisar Despesa"), Title("Analisar Despesa")]
    public partial class AnalisarEvento : TPage
    {
        private readonly Techne.Lyceum.RN.PrestacaoContas.Evento rnEvento = new Techne.Lyceum.RN.PrestacaoContas.Evento();
        private readonly Techne.Lyceum.RN.PrestacaoContas.Fornecedor rnFornecedor = new Techne.Lyceum.RN.PrestacaoContas.Fornecedor();
        private readonly Techne.Lyceum.RN.PrestacaoContas.ExigenciaEvento rnExigenciaEvento = new Techne.Lyceum.RN.PrestacaoContas.ExigenciaEvento();
        private readonly Techne.Lyceum.RN.PrestacaoContas.ExigenciaEventoArquivo rnExigenciaEventoArquivo = new Techne.Lyceum.RN.PrestacaoContas.ExigenciaEventoArquivo();
        private readonly Techne.Lyceum.RN.PrestacaoContas.TipoTransporte rnTipoTransporte = new Techne.Lyceum.RN.PrestacaoContas.TipoTransporte();
        private readonly Techne.Lyceum.RN.PrestacaoContas.ImportacaoXmlEvento rnImportacaoXmlEvento = new Techne.Lyceum.RN.PrestacaoContas.ImportacaoXmlEvento();
        private readonly Techne.Lyceum.RN.PeriodoLetivo rnPeriodoLetivo = new Techne.Lyceum.RN.PeriodoLetivo();
        private readonly Techne.Lyceum.RN.PrestacaoContas.MotivoExigenciaEvento rnMotivoExigenciaEvento = new Techne.Lyceum.RN.PrestacaoContas.MotivoExigenciaEvento();
        private readonly Techne.Lyceum.RN.PrestacaoContas.UnidadeMedida rnUnidadeMedida = new Techne.Lyceum.RN.PrestacaoContas.UnidadeMedida();
        private readonly Techne.Lyceum.RN.PrestacaoContas.ProdutoServico rnProdutoServico = new Techne.Lyceum.RN.PrestacaoContas.ProdutoServico();

        protected int PlanoTrabalhoId
        {
            get
            {
                var result = 0;
                int.TryParse(Convert.ToString(ViewState["PlanoTrabalhoId"]) ?? "0", out result);
                return result;
            }
            set
            {
                ViewState["PlanoTrabalhoId"] = value;
            }
        }        

        protected string Censo
        {
            get
            {
                return Convert.ToString(ViewState["Censo"]);
            }
            set
            {
                ViewState["Censo"] = value;
            }
        }

        protected int Tipo
        {
            get
            {
                return Convert.ToInt32(ViewState["Tipo"]);
            }
            set
            {
                ViewState["Tipo"] = value;
            }
        }

        protected int EventoId
        {
            get
            {
                var result = 0;
                var decodedBytes = Convert.FromBase64String(this.Request.QueryString["Chave"]);
                var decodedText = Encoding.UTF8.GetString(decodedBytes);
                
                var listaDados = decodedText.Split('&');

                foreach (var dados in listaDados)
                {
                    if (dados.IndexOf("eventoId") >= 0)
                    {
                        int.TryParse(dados.Substring(dados.LastIndexOf('=') + 1) ?? "0", out result);
                    }
                    if (dados.IndexOf("censo") >= 0)
                    {
                        hdnCenso.Value = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                }
               
                hdnEventoId.Value = result.ToString();
                return result;
            }
        }

        protected string UsuarioId
        {
            get
            {
                return User.Identity.Name;
            }
        }

        protected void Page_Init()
        {
            TituloGrid(grdExigenciaEvento, "Exigências da Despesa");
            TituloGrid(grdXML, "Dados do XML");
            TituloGrid(grdItensXml, "Itens da Nota Fiscal");
            TituloGrid(grdServidores_DCTS, "Servidores Incluídos");
        }

        protected void Page_Load()
        {
            try
            {
                if (IsPostBack)
                    return;

                if (EventoId == 0)
                {
                    lblMensagem.Text = "Não foi especificada a despesa na hora de abrir esta tela";
                    plaAnalisarEvento.Visible = false;
                    return;
                }

                ConsultaExistente();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnAprovar, AcaoControle.editar);
            ControlaAcesso(grdItensXml);
            ControlaAcesso(grdExigenciaEvento);            
            ControlaAcesso(grdServidores_DCTS);
            ControlaAcesso(grdXML);
            ControlaAcesso(grdExigenciaEvento, AcaoControle.editar, "btnAprovarExigencia");
            ControlaAcesso(grdExigenciaEvento, AcaoControle.editar, "btnRejeitarExigencia");
            ControlaAcesso(grdExigenciaEvento, AcaoControle.novo, "btnNovaExigenciaEvento");
            AcessoGrid();

        }

        protected void AcessoGrid()
        {
            if (grdExigenciaEvento != null)
            {
                ImageButton img = (ImageButton)grdExigenciaEvento.FindHeaderTemplateControl(grdExigenciaEvento.Columns[""], "btnNovaExigenciaEvento");


                if (img != null)
                {
                    img.Visible = Permission.AllowInsert;


                }
            }
        }
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            string planoTrabalhoId = string.Empty, censo = string.Empty, tipo = string.Empty, eventoId = string.Empty, periodoPrest = string.Empty, situacao = string.Empty;

             string tpdespesa=  string.Empty;
             string unidade = string.Empty;
             string projeto = string.Empty;

            if (this.Request.QueryString.Keys.Count > 0)
            {
                var decodedBytes = Convert.FromBase64String(this.Request.QueryString["Chave"]);
                var decodedText = Encoding.UTF8.GetString(decodedBytes);             

                var listaDados = decodedText.Split('&');

                foreach (var dados in listaDados)
                {
                    if (dados.IndexOf("planoTrabalhoId") >= 0)
                    {
                        planoTrabalhoId = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("censo") >= 0)
                    {
                        censo = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("tipo") >= 0)
                    {
                        tipo = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("eventoId") >= 0)
                    {
                        eventoId = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("periodo") >= 0)
                    {
                        periodoPrest = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("situacao") >= 0)
                    {
                        situacao = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("tpdespesa") >= 0)
                    {
                        tpdespesa = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("unidade") >= 0)
                    {
                        unidade = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("projeto") >= 0)
                    {
                        projeto = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                }

                string queryString = string.Format("planoTrabalhoId={0}&censo={1}&tipo={2}&eventoId={3}&periodo={4}&situacao={5}&tpdespesa={6}&unidade={7}&projeto={8}",
                   planoTrabalhoId,
                   censo,
                   tipo,
                   eventoId,
                   periodoPrest,
                   situacao,
                   tpdespesa,
                   unidade,
                   projeto
               );

                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                Response.Redirect("~/PrestacaoContas/ListaAnalisarEvento.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
            }
        }



        protected void btnAprovar_Click(object sender, EventArgs e)
        {
            try
            {
                var validacao = rnEvento.ValidaFinalizacao(EventoId,Convert.ToInt32(hdnFinalidadeId.Value), UsuarioId, false);

                if (validacao.Valido)
                {
                    rnEvento.Finaliza(EventoId, UsuarioId);
                    ConsultaExistente();
                }
                else
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void lnkVisualizarOrcamento_Command(object sender, CommandEventArgs e)
        {
            try
            {
                var tabela = "OrcamentoArquivo";
                RN.PrestacaoContas.OrcamentoArquivo rnArquivo = new OrcamentoArquivo();

                string embed = string.Empty;
                bimgArquivo.Visible = false;
                ltEmbed.Visible = false;
                DataTable dados = new DataTable();

                string[] chave = ((LinkButton)sender).CommandArgument.ToString().Split(new char[] { ',' });

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
                        ltEmbed.Text = string.Format(embed, ResolveUrl("~/PrestacaoContas/FileCS.ashx?Tabela=" + tabela + "&Id="), chave[0].ToString());
                        ltEmbed.Visible = true;
                        pucVisualizarArquivo.Width = Unit.Pixel(880);
                        pucVisualizarArquivo.Height = Unit.Pixel(580);
                    }
                    else
                    {
                        pucVisualizarArquivo.Width = Unit.Pixel(350);
                        pucVisualizarArquivo.Height = Unit.Pixel(350);
                        bimgArquivo.ContentBytes = rnArquivo.ObtemArquivoPor(Convert.ToInt32(chave[0]));
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

        protected void lnkVisualizarNotaFiscal_Command(object sender, CommandEventArgs e)
        {
            try
            {
                var tabela = "EventoNotaFiscalArquivo";
                RN.PrestacaoContas.EventoNotaFiscalArquivo rnArquivo = new EventoNotaFiscalArquivo();

                string embed = string.Empty;
                bimgArquivo.Visible = false;
                ltEmbed.Visible = false;
                DataTable dados = new DataTable();

                string[] chave = ((LinkButton)sender).CommandArgument.ToString().Split(new char[] { ',' });

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
                        ltEmbed.Text = string.Format(embed, ResolveUrl("~/PrestacaoContas/FileCS.ashx?Tabela=" + tabela + "&Id="), chave[0].ToString());
                        ltEmbed.Visible = true;
                        pucVisualizarArquivo.Width = Unit.Pixel(880);
                        pucVisualizarArquivo.Height = Unit.Pixel(580);
                    }
                    else
                    {
                        pucVisualizarArquivo.Width = Unit.Pixel(350);
                        pucVisualizarArquivo.Height = Unit.Pixel(350);
                        bimgArquivo.ContentBytes = rnArquivo.ObtemArquivoPor(Convert.ToInt32(chave[0]));
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

        protected void lnkVisualizarComprovantePgto_Command(object sender, CommandEventArgs e)
        {
            try
            {
                var tabela = "ComprovantePagamentoArquivo";
                RN.PrestacaoContas.ComprovantePagamentoArquivo rnArquivo = new ComprovantePagamentoArquivo();

                string embed = string.Empty;
                bimgArquivo.Visible = false;
                ltEmbed.Visible = false;
                DataTable dados = new DataTable();

                string[] chave = ((LinkButton)sender).CommandArgument.ToString().Split(new char[] { ',' });

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
                        ltEmbed.Text = string.Format(embed, ResolveUrl("~/PrestacaoContas/FileCS.ashx?Tabela=" + tabela + "&Id="), chave[0].ToString());
                        ltEmbed.Visible = true;
                        pucVisualizarArquivo.Width = Unit.Pixel(880);
                        pucVisualizarArquivo.Height = Unit.Pixel(580);
                    }
                    else
                    {
                        pucVisualizarArquivo.Width = Unit.Pixel(350);
                        pucVisualizarArquivo.Height = Unit.Pixel(350);
                        bimgArquivo.ContentBytes = rnArquivo.ObtemArquivoPor(Convert.ToInt32(chave[0]));
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

        protected void lnkVisualizarXML_Command(object sender, CommandEventArgs e)
        {
            try
            {
                pucVisualizarXML.ShowOnPageLoad = true;
                pucVisualizarXML.Width = Unit.Pixel(880);
                pucVisualizarXML.Height = Unit.Pixel(580);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void lnkVisualizarNotaFiscal_PD_Command(object sender, CommandEventArgs e)
        {
            try
            {
                var tabela = "EventoNotaFiscalArquivo";
                RN.PrestacaoContas.EventoNotaFiscalArquivo rnArquivo = new EventoNotaFiscalArquivo();

                string embed = string.Empty;
                bimgArquivo.Visible = false;
                ltEmbed.Visible = false;
                DataTable dados = new DataTable();

                string[] chave = ((LinkButton)sender).CommandArgument.ToString().Split(new char[] { ',' });

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
                        ltEmbed.Text = string.Format(embed, ResolveUrl("~/PrestacaoContas/FileCS.ashx?Tabela=" + tabela + "&Id="), chave[0].ToString());
                        ltEmbed.Visible = true;
                        pucVisualizarArquivo.Width = Unit.Pixel(880);
                        pucVisualizarArquivo.Height = Unit.Pixel(580);
                    }
                    else
                    {
                        pucVisualizarArquivo.Width = Unit.Pixel(350);
                        pucVisualizarArquivo.Height = Unit.Pixel(350);
                        bimgArquivo.ContentBytes = rnArquivo.ObtemArquivoPor(Convert.ToInt32(chave[0]));
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

        protected void lnkVisualizarEvidencia_Command(object sender, CommandEventArgs e)
        {
            try
            {
                var tabela = "EvidenciaArquivo";
                //RN.PrestacaoContas.OrcamentoArquivo rnArquivo = new OrcamentoArquivo();
                RN.PrestacaoContas.EvidenciaArquivo rnEvidenciaArquivo = new EvidenciaArquivo();

                string embed = string.Empty;
                bimgArquivo.Visible = false;
                ltEmbed.Visible = false;
                DataTable dados = new DataTable();

                string[] chave = ((LinkButton)sender).CommandArgument.ToString().Split(new char[] { ',' });

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
                        ltEmbed.Text = string.Format(embed, ResolveUrl("~/PrestacaoContas/FileCS.ashx?Tabela=" + tabela + "&Id="), chave[0].ToString());
                        ltEmbed.Visible = true;
                        pucVisualizarArquivo.Width = Unit.Pixel(880);
                        pucVisualizarArquivo.Height = Unit.Pixel(580);
                    }
                    else
                    {
                        pucVisualizarArquivo.Width = Unit.Pixel(350);
                        pucVisualizarArquivo.Height = Unit.Pixel(350);
                        bimgArquivo.ContentBytes = rnEvidenciaArquivo.ObtemArquivoPor(Convert.ToInt32(chave[0]));
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

        protected void lnkVisualizarNotaFiscal_DCC_Command(object sender, CommandEventArgs e)
        {
            try
            {
                var tabela = "EventoNotaFiscalArquivo";
                RN.PrestacaoContas.EventoNotaFiscalArquivo rnArquivo = new EventoNotaFiscalArquivo();

                string embed = string.Empty;
                bimgArquivo.Visible = false;
                ltEmbed.Visible = false;
                DataTable dados = new DataTable();

                string[] chave = ((LinkButton)sender).CommandArgument.ToString().Split(new char[] { ',' });

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
                        ltEmbed.Text = string.Format(embed, ResolveUrl("~/PrestacaoContas/FileCS.ashx?Tabela=" + tabela + "&Id="), chave[0].ToString());
                        ltEmbed.Visible = true;
                        pucVisualizarArquivo.Width = Unit.Pixel(880);
                        pucVisualizarArquivo.Height = Unit.Pixel(580);
                    }
                    else
                    {
                        pucVisualizarArquivo.Width = Unit.Pixel(350);
                        pucVisualizarArquivo.Height = Unit.Pixel(350);
                        bimgArquivo.ContentBytes = rnArquivo.ObtemArquivoPor(Convert.ToInt32(chave[0]));
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

        protected void lnkVisualizarComprovantePgto_DCC_Command(object sender, CommandEventArgs e)
        {
            try
            {
                var tabela = "ComprovantePagamentoArquivo";
                RN.PrestacaoContas.ComprovantePagamentoArquivo rnArquivo = new ComprovantePagamentoArquivo();

                string embed = string.Empty;
                bimgArquivo.Visible = false;
                ltEmbed.Visible = false;
                DataTable dados = new DataTable();

                string[] chave = ((LinkButton)sender).CommandArgument.ToString().Split(new char[] { ',' });

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
                        ltEmbed.Text = string.Format(embed, ResolveUrl("~/PrestacaoContas/FileCS.ashx?Tabela=" + tabela + "&Id="), chave[0].ToString());
                        ltEmbed.Visible = true;
                        pucVisualizarArquivo.Width = Unit.Pixel(880);
                        pucVisualizarArquivo.Height = Unit.Pixel(580);
                    }
                    else
                    {
                        pucVisualizarArquivo.Width = Unit.Pixel(350);
                        pucVisualizarArquivo.Height = Unit.Pixel(350);
                        bimgArquivo.ContentBytes = rnArquivo.ObtemArquivoPor(Convert.ToInt32(chave[0]));
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

        #region Grid de Exigências

        protected void btnVisualizar_Command(object sender, CommandEventArgs e)
        {
            try
            {
                var tabela = "ExigenciaEventoArquivo";
                RN.PrestacaoContas.ExigenciaEventoArquivo rnArquivo = new ExigenciaEventoArquivo();

                string embed = string.Empty;
                bimgArquivo.Visible = false;
                ltEmbed.Visible = false;
                DataTable dados = new DataTable();

                string[] chave = ((ImageButton)sender).CommandArgument.ToString().Split(new char[] { ',' });

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
                        ltEmbed.Text = string.Format(embed, ResolveUrl("~/PrestacaoContas/FileCS.ashx?Tabela=" + tabela + "&Id="), chave[0].ToString());
                        ltEmbed.Visible = true;
                        pucVisualizarArquivo.Width = Unit.Pixel(880);
                        pucVisualizarArquivo.Height = Unit.Pixel(580);
                    }
                    else
                    {
                        pucVisualizarArquivo.Width = Unit.Pixel(350);
                        pucVisualizarArquivo.Height = Unit.Pixel(350);
                        bimgArquivo.ContentBytes = rnArquivo.ObtemArquivoPor(Convert.ToInt32(chave[0]));
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

       
        protected void grdExigenciaEvento_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            var justificada = !Convert.ToString(grdExigenciaEvento.GetRowValues(e.VisibleIndex, "JUSTIFICATIVA")).IsNullOrEmptyOrWhiteSpace();
            var temArquivo = !Convert.ToString(grdExigenciaEvento.GetRowValues(e.VisibleIndex, "TIPOARQUIVO")).IsNullOrEmptyOrWhiteSpace();
            var aprovado = Convert.ToBoolean(grdExigenciaEvento.GetRowValues(e.VisibleIndex, "APROVADO"));
            var rejeitado = Convert.ToBoolean(grdExigenciaEvento.GetRowValues(e.VisibleIndex, "REJEITADO"));

            var eventoFinalizado = new string[] { "Aprovado", "Reprovado" }.Contains(lblStatusEvento.Text);

            if (e.ButtonType == ColumnCommandButtonType.Edit)
            {
                e.Visible = !aprovado;
                if (eventoFinalizado)
                    e.Visible = false;
            }

            if (e.ButtonType == ColumnCommandButtonType.Delete)
            {
                e.Visible = !aprovado && !rejeitado && (!justificada || !temArquivo);
                if (eventoFinalizado)
                    e.Visible = false;
            }
        }

        protected void grdExigenciaEvento_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            var justificada = !Convert.ToString(grdExigenciaEvento.GetRowValues(e.VisibleIndex, "JUSTIFICATIVA")).IsNullOrEmptyOrWhiteSpace();
            var temArquivo = !Convert.ToString(grdExigenciaEvento.GetRowValues(e.VisibleIndex, "TIPOARQUIVO")).IsNullOrEmptyOrWhiteSpace();
            var aprovado = Convert.ToBoolean(grdExigenciaEvento.GetRowValues(e.VisibleIndex, "APROVADO"));
            var rejeitado = Convert.ToBoolean(grdExigenciaEvento.GetRowValues(e.VisibleIndex, "REJEITADO"));

            var eventoFinalizado = new string[] { "Aprovado", "Reprovado" }.Contains(lblStatusEvento.Text);

            if (e.ButtonID == "btnAprovarExigencia")
            {
                e.Visible = (!aprovado && (justificada || temArquivo)) ? DevExpress.Web.ASPxClasses.DefaultBoolean.True : DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                if (eventoFinalizado)
                    e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;

                if (!Permission.AllowUpdate)
                    e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;

            }

            if (e.ButtonID == "btnRejeitarExigencia")
            {
                e.Visible = (!aprovado && (justificada && temArquivo) && !rejeitado) ? DevExpress.Web.ASPxClasses.DefaultBoolean.True : DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                if (eventoFinalizado)
                    e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;

                if (!Permission.AllowUpdate)
                    e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
            }
        }

        protected void grdExigenciaEvento_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            try
            {
                var exigenciaEventoId = Convert.ToInt32(grdExigenciaEvento.GetRowValues(e.VisibleIndex, "EXIGENCIAEVENTOID"));
                var usuarioId = User.Identity.Name;
                    
                if (e.ButtonID == "btnAprovarExigencia")
                {
                    var validacao = rnExigenciaEvento.ValidaAprovacao(exigenciaEventoId, usuarioId);
                    
                    if (validacao.Valido)
                        rnExigenciaEvento.Aprova(exigenciaEventoId, usuarioId);
                    else
                        throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));

                    grdExigenciaEvento.DataBind();
                }

                if (e.ButtonID == "btnRejeitarExigencia")
                {
                    var validacao = rnExigenciaEvento.ValidaRejeicao(exigenciaEventoId, usuarioId);

                    if (validacao.Valido)
                        rnExigenciaEvento.Rejeita(exigenciaEventoId, usuarioId);
                    else
                        throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));

                    grdExigenciaEvento.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdExigenciaEvento_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();

                var ee = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ExigenciaEvento();

                ee.EventoId = EventoId;
                ee.MotivoExigenciaEventoId = Convert.ToInt32(e.NewValues["MOTIVOEXIGENCIAEVENTOID"]);
                ee.NotaExplicativa = Convert.ToString(e.NewValues["NOTAEXPLICATIVA"]);
                ee.UsuarioId = UsuarioId;
                ee.DataCadastro = DateTime.Now;

                validacao = rnExigenciaEvento.Valida(ee, true);

                if (validacao.Valido)
                {
                    rnExigenciaEvento.Insere(ee);
                }
                else
                {
                    if (!validacao.Mensagem.Contains("Esta exigência já foi cadastrada para essa despesa / data."))
                    {
                        e.Cancel = true;
                    }
                    
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }

                grdExigenciaEvento.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdExigenciaEvento_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();

                var ee = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ExigenciaEvento();

                ee.ExigenciaEventoId = Convert.ToInt32(grdExigenciaEvento.GetRowValues(grdExigenciaEvento.EditingRowVisibleIndex, "EXIGENCIAEVENTOID"));
                ee.EventoId = EventoId;
                ee.MotivoExigenciaEventoId = Convert.ToInt32(e.NewValues["MOTIVOEXIGENCIAEVENTOID"]);
                ee.NotaExplicativa = Convert.ToString(e.NewValues["NOTAEXPLICATIVA"]);
                ee.UsuarioId = User.Identity.Name;
                ee.DataAlteracao = DateTime.Now;

                validacao = rnExigenciaEvento.Valida(ee, false);

                if (validacao.Valido)
                {
                    rnExigenciaEvento.Atualiza(ee);
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }

                grdExigenciaEvento.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdExigenciaEvento_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                e.Cancel = true;
                rnExigenciaEvento.Remove(Convert.ToInt32(e.Keys["EXIGENCIAEVENTOID"]));

                grdExigenciaEvento.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnNovaExigenciaEvento_Load(object sender, EventArgs e)
        {
            ((ImageButton)sender).Visible = !new string[] { "Aprovado", "Reprovado" }.Contains(lblStatusEvento.Text);
        }

        public object ListaAno()
        {
            return rnPeriodoLetivo.ListaAnos(false);
        }

        public DataTable ListaMotivoExigenciaEvento()
        {
            return rnMotivoExigenciaEvento.ListaAtivo();
        }

        public DataTable ListaExigenciaEvento(int eventoId)
        {
            return rnExigenciaEvento.ListaExigenciasPor(eventoId);
        }

        public void InsertExigenciaEvento(object MOTIVOEXIGENCIAEVENTOID, object NOTAEXPLICATIVA)
        {
        }

        public void UpdateExigenciaEvento(object EXIGENCIAEVENTOID, object MOTIVOEXIGENCIAEVENTOID, object NOTAEXPLICATIVA, object APROVADO)
        {
        }

        public void UpdateExigenciaEvento(object EXIGENCIAEVENTOID, object MOTIVOEXIGENCIAEVENTOID, object NOTAEXPLICATIVA)
        {
        }

        public void DeleteExigenciaEvento(object EXIGENCIAEVENTOID)
        { }

        #endregion

        #region Grid XML (do popup que é aberto com o link de visualização de XML)

        public DataTable ListaXML(int eventoId, string censo)
        {
            return rnImportacaoXmlEvento.ListaItensXmlPor(eventoId, censo);
        }

        #endregion

        #region Grid XML (aba Itens da NF)

        private ASPxTextBox txtNCM;
        private ASPxComboBox ddlUnidadeMedida;
        private ASPxComboBox ddlCodigoFgv;
        private ASPxTextBox txtValorFgv;
        private ASPxTextBox txtDiferenca;
        private ASPxTextBox txtPorcentagemDiferenca;

        protected void grdItensXml_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (grdItensXml.IsEditing)
            {
                switch (e.Column.FieldName)
                {
                    case "NCM":
                        txtNCM = e.Editor as ASPxTextBox;
                        break;

                    case "UNIDADEMEDIDAID":
                        ddlUnidadeMedida = e.Editor as ASPxComboBox;
                        break;

                    case "CODIGO_FGV":
                        ddlCodigoFgv = e.Editor as ASPxComboBox;

                        var ncm = txtNCM.Text;
                        var unidadeMedidaId = Convert.ToInt32(ddlUnidadeMedida.Value ?? 0);
                        var codigoFgv = Convert.ToString(ddlCodigoFgv.Value ?? "");

                        if (!ncm.IsNullOrEmptyOrWhiteSpace() && unidadeMedidaId > 0 && !codigoFgv.IsNullOrEmptyOrWhiteSpace())
                        {
                            ddlCodigoFgv.DataSource = ListaCodigoFgv(ncm, unidadeMedidaId);
                            ddlCodigoFgv.DataBind();
                            var itemFound = ddlCodigoFgv.Items.FindByValue(codigoFgv);
                            if (itemFound != null)
                                itemFound.Selected = true;
                        }

                        break;

                    case "VALORFGV":
                        txtValorFgv = e.Editor as ASPxTextBox;
                        break;

                    case "DIFERENCA":
                        txtDiferenca = e.Editor as ASPxTextBox;
                        break;

                    case "PORCENTAGEMDIFERENCA":
                        txtPorcentagemDiferenca = e.Editor as ASPxTextBox;
                        break;

                    case "FLAGNAOPERMITIDO":
                        break;
                }
            }
        }

        protected void grdItensXml_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            var aprovado = lblStatusEvento.Text == "Validado";
            var eventoFinalizado = new string[] { "Validado", "Reprovado" }.Contains(lblStatusEvento.Text);

            if (e.ButtonType == ColumnCommandButtonType.Edit)
            {
                e.Visible = !aprovado;
                if (eventoFinalizado)
                    e.Visible = false;
            }
        }

        protected void grdItensXml_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            try
            {
                var parameters = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(e.Parameters);
                var operation = Convert.ToString(parameters["Operation"]);

                switch (operation)
                {
                    case "ddlUnidadeMedida_SelectedIndexChanged":
                        ddlUnidadeMedida_SelectedIndexChanged(parameters);
                        break;

                    case "ddlCodigoFgv_SelectedIndexChanged":
                        ddlCodigoFgv_SelectedIndexChanged(parameters);
                        break;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdItensXml_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();

                var importacaoXmlEventoId = Convert.ToInt32(grdItensXml.GetRowValues(grdItensXml.EditingRowVisibleIndex, "IMPORTACAOXMLEVENTOID"));
                var ncm = Convert.ToString(e.NewValues["NCM"]);
                var unidadeMedidaId = Convert.ToInt32(e.NewValues["UNIDADEMEDIDAID"]);
                var codigoFgv = Convert.ToString(e.NewValues["CODIGO_FGV"]);

                validacao = rnImportacaoXmlEvento.ValidaAnalise(importacaoXmlEventoId, ncm, unidadeMedidaId, codigoFgv, UsuarioId);

                if (validacao.Valido)
                {
                    rnImportacaoXmlEvento.SalvaAnalise(importacaoXmlEventoId, ncm, unidadeMedidaId, codigoFgv, UsuarioId);
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }

                grdItensXml.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ddlUnidadeMedida_SelectedIndexChanged(Dictionary<string, string> parameters)
        {
            try
            {
                if (parameters["Operation"] != "ddlUnidadeMedida_SelectedIndexChanged")
                    return;

                var ncm = Convert.ToString(parameters["NCM"]);
                var unidadeMedidaId = Convert.ToInt32(parameters["UnidadeMedidaId"]);

                ddlCodigoFgv.SelectedItem = null;
                ddlCodigoFgv.Items.Clear();
                ddlCodigoFgv.DataSource = ListaCodigoFgv(ncm, unidadeMedidaId);
                ddlCodigoFgv.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ddlCodigoFgv_SelectedIndexChanged(Dictionary<string, string> parameters)
        {
            try
            {
                if (parameters["Operation"] != "ddlCodigoFgv_SelectedIndexChanged")
                    return;

                var ncm = Convert.ToString(parameters["NCM"]);
                var unidadeMedidaId = Convert.ToInt32(parameters["UnidadeMedidaId"]);
                var codigoFgvId = Convert.ToInt32(parameters["CodigoFgvId"]);

                ddlCodigoFgv.DataSource = ListaCodigoFgv(ncm, unidadeMedidaId);
                ddlCodigoFgv.DataBind();
                ddlCodigoFgv.SelectedItem.Value = codigoFgvId;

                var valorMaximo = rnProdutoServico.ObtemValorFgvMaximoPor(codigoFgvId.ToString(), unidadeMedidaId, EventoId);

                if (valorMaximo.HasValue)
                {
                    var valorPago = Convert.ToDecimal(grdItensXml.GetRowValues(grdItensXml.EditingRowVisibleIndex, "VALORUNITARIO"));

                    txtValorFgv.Text = valorMaximo.Value.ToString("N2");
                    txtDiferenca.Text = (valorMaximo.Value - valorPago).ToString("N2");
                    txtPorcentagemDiferenca.Text = (((valorMaximo.Value - valorPago) / valorMaximo.Value) * 100 * -1).ToString("N2").Replace(".", "");

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public DataTable ListaUnidadeMedida()
        {
            return rnUnidadeMedida.ListaAtivo();
        }

        public DataTable ListaCodigoFgv(string ncm, int unidadeMedidaId)
        {
            var lista = rnProdutoServico.ListaCodigoFgvPor(ncm, unidadeMedidaId);
            if (lista.Rows.Count > 1)
            {
                DataRow newRow = lista.NewRow();
                lista.Rows.InsertAt(newRow, 0);
            }
            return lista;
        }

        public DataTable ListaItensXml(int eventoId, string censo)
        {
            return rnImportacaoXmlEvento.ListaItensXmlPor(eventoId, censo);
        }

        public void UpdateItensXml(object ITEM, object NCM, object UNIDADEMEDIDAID, object CODIGO_FGV, object VALORFGV, object DIFERENCA, object PORCENTAGEMDIFERENCA, object IMPORTACAOXMLEVENTOID)
        {
        }

        #endregion

        #region Métodos auxiliares

        private void ConsultaExistente()
        {
            ImportacaoXmlEvento rnImportacaoXmlEvento = new ImportacaoXmlEvento();

            if (EventoId == 0)
                return;

            hdnFinalidadeId.Value = string.Empty;

            var evento = rnEvento.ObtemDadosEventoPor(EventoId);

            if (evento.StatusAnalise == "Aberto")
            {
                lblMensagem.Text = "Esta despesa não pode ser acessada porque ainda não foi enviado para análise";
                plaAnalisarEvento.Visible = false;
                return;
            }

            Tipo = evento.TipoDespesa;
            PlanoTrabalhoId = evento.PlanoTrabalhoId;
            Censo = evento.Censo;
            hdnFinalidadeId.Value = evento.FinalidadeId.ToString();

            switch (Tipo)
            {
                case (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaComum: //Tipo 0
                    ModoTela = ModoTelaEnum.ConsultaDespesaComumExistente;
                    ConsultaEventoExistente(evento);
                    break;

                case (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaDocumentosFiscais: //Tipo 1
                    ModoTela = ModoTelaEnum.ConsultaDespesaDocumentosFiscais;
                    ConsultaDespesaDocumentosFiscais(evento);
                    break;

                case (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaComComprovacao: //Tipo 2
                    ModoTela = ModoTelaEnum.ConsultaPequenaDespesaComComprovacaoExistente;
                    ConsultaPequenaDespesaComComprovacaoExistente(evento);
                    break;

                case (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaSemComprovacao: //Tipo 4
                    ModoTela = ModoTelaEnum.ConsultaPequenaDespesaSemComprovacaoExistente;
                    ConsultaPequenaDespesaSemComprovacaoExistente(evento);
                    break;

                case (int)RN.PrestacaoContas.Evento.TipoDespesa.PequenaDespesaComTransladoServidores: //Tipo 3
                    ModoTela = ModoTelaEnum.ConsultaPequenaDespesaComTransladoExistente;
                    ConsultaPequenaDespesaComTransladoExistente(evento);
                    break;
            }

            btnAprovar.Visible = new string[] { "Enviado para Análise" }.Contains(evento.StatusAnalise);

            if (evento.StatusAnalise == "Validado")
            {
                lblStatusEvento.Text = evento.StatusAnalise + " por " + evento.NomeUsuario + " - Matrícula/ID Funcional: " + evento.UsuarioId;
            }
            else
            {
                lblStatusEvento.Text = evento.StatusAnalise;
            }

            var tabItemNF = pcAnalisarEvento.TabPages.FindByName("tabItemNF");
            
            //Aba Itens NF aparece tipo NF-e de merenda 
            tabItemNF.ClientVisible = evento.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaComum && evento.FinalidadeId == 2;

            //Xml Aparece para tipo NF-e que tem xml
            lnkVisualizarXML.Visible = evento.TipoDespesa == (int)RN.PrestacaoContas.Evento.TipoDespesa.DespesaComum && rnImportacaoXmlEvento.ListaItensXmlPor(evento.EventoId, evento.Censo).Rows.Count > 0;

            lnkVisualizarXML.Text = "XML inserido";
            if (lnkVisualizarXML.Visible && rnEvento.PossuiXmlGeradoInternamente(evento.EventoId))
            {
                lnkVisualizarXML.Text = "Nota sem XML";
            }
        }

        private void ConsultaEventoExistente(DadosEvento evento)
        {
            try
            {
                lblPlanoTrabalho.Text = evento.PlanoTrabalhoDescricao;
                lblUnidadeEnsino.Text = evento.CensoNomeComp;
                lblTipoEvento.Text = evento.DescricaoTipoDespesa;
                lblNumeroEvento.Text = evento.NumeroEvento;
                lblFinalidade.Text = evento.FinalidadeDescricao;

                txtDescricao.Text = evento.Descricao;

                lnkVisualizarOrcamento1.Visible = evento.Orcamento1Id.HasValue;
                lnkVisualizarOrcamento1.CommandArgument = (evento.Orcamento1Id ?? 0).ToString() + "," + (!evento.Orcamento1TipoArquivo.IsNullOrEmptyOrWhiteSpace() ? evento.Orcamento1TipoArquivo : "");
                lnkVisualizarOrcamento2.Visible = evento.Orcamento2Id.HasValue;
                lnkVisualizarOrcamento2.CommandArgument = (evento.Orcamento2Id ?? 0).ToString() + "," + (!evento.Orcamento2TipoArquivo.IsNullOrEmptyOrWhiteSpace() ? evento.Orcamento2TipoArquivo : "");
                lnkVisualizarOrcamento3.Visible = evento.Orcamento3Id.HasValue;
                lnkVisualizarOrcamento3.CommandArgument = (evento.Orcamento3Id ?? 0).ToString() + "," + (!evento.Orcamento3TipoArquivo.IsNullOrEmptyOrWhiteSpace() ? evento.Orcamento3TipoArquivo : "");
                txtJustificativa.Text = evento.JustificativaOrcamento;

                var fornecedor = rnFornecedor.ObtemPor(evento.FornecedorId ?? 0);
                if (fornecedor != null)
                    lblFornecedor.Text = fornecedor.FornecedorId + " - " + fornecedor.RazaoSocial;
                else
                    lblFornecedor.Text = "";

                tbChaveAcesso.Visible = true;
                txtChaveAcesso.Text = evento.ChaveAcesso;
                txtNumeroNF.Text = evento.NumeroNotaFiscal;
                txtValorTotalNF.Text = evento.ValorNotaFiscal.HasValue ? evento.ValorNotaFiscal.Value.ToString("N2") : "";
                txtDataNF.Text = evento.DataNotaFiscal.HasValue ? evento.DataNotaFiscal.Value.ToString("dd/MM/yyyy") : "";
                txtValorPagoNF.Text = evento.ValorPagamento.ToString("N2");
                txtDataPagamentoNF.Text = evento.DataPagamento.HasValue ? evento.DataPagamento.Value.ToString("dd/MM/yyyy") : "";
                lnkVisualizarNotaFiscal.Visible = evento.NotaFiscalArquivoId.HasValue;
                lnkVisualizarNotaFiscal.CommandArgument = evento.NotaFiscalArquivoId.ToString() + "," + (!evento.NotaFiscalTipoArquivo.IsNullOrEmptyOrWhiteSpace() ? evento.NotaFiscalTipoArquivo : "");
                lnkVisualizarComprovantePgto.Visible = evento.ComprovantePagamentoArquivoId.HasValue;
                lnkVisualizarComprovantePgto.CommandArgument = evento.ComprovantePagamentoArquivoId.ToString() + "," + (!evento.ComprovantePagamentoTipoArquivo.IsNullOrEmptyOrWhiteSpace() ? evento.ComprovantePagamentoTipoArquivo : "");

                tbXML.Visible = true;

                txtObservacao.Text = evento.Observacoes;
                lnkVisualizarEvidencia.Visible = evento.EvidenciaArquivoId.HasValue;
                lnkVisualizarEvidencia.CommandArgument = evento.EvidenciaArquivoId.ToString() + "," + (!evento.EvidenciaTipoArquivo.IsNullOrEmptyOrWhiteSpace() ? evento.EvidenciaTipoArquivo : "");
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ConsultaDespesaDocumentosFiscais(DadosEvento evento)
        {
            try
            {
                lblPlanoTrabalho.Text = evento.PlanoTrabalhoDescricao;
                lblUnidadeEnsino.Text = evento.CensoNomeComp;
                lblTipoEvento.Text = evento.DescricaoTipoDespesa;
                lblNumeroEvento.Text = evento.NumeroEvento;
                lblFinalidade.Text = evento.FinalidadeDescricao;

                txtDescricao.Text = evento.Descricao;

                lnkVisualizarOrcamento1.Visible = evento.Orcamento1Id.HasValue;
                lnkVisualizarOrcamento1.CommandArgument = (evento.Orcamento1Id ?? 0).ToString() + "," + (!evento.Orcamento1TipoArquivo.IsNullOrEmptyOrWhiteSpace() ? evento.Orcamento1TipoArquivo : "");
                lnkVisualizarOrcamento2.Visible = evento.Orcamento2Id.HasValue;
                lnkVisualizarOrcamento2.CommandArgument = (evento.Orcamento2Id ?? 0).ToString() + "," + (!evento.Orcamento2TipoArquivo.IsNullOrEmptyOrWhiteSpace() ? evento.Orcamento2TipoArquivo : "");
                lnkVisualizarOrcamento3.Visible = evento.Orcamento3Id.HasValue;
                lnkVisualizarOrcamento3.CommandArgument = (evento.Orcamento3Id ?? 0).ToString() + "," + (!evento.Orcamento3TipoArquivo.IsNullOrEmptyOrWhiteSpace() ? evento.Orcamento3TipoArquivo : "");
                txtJustificativa.Text = evento.JustificativaOrcamento;

                var fornecedor = rnFornecedor.ObtemPor(evento.FornecedorId ?? 0);
                if (fornecedor != null)
                    lblFornecedor.Text = fornecedor.FornecedorId + " - " + fornecedor.RazaoSocial;
                else
                    lblFornecedor.Text = "";

                tbChaveAcesso.Visible = false;

                txtNumeroNF.Text = evento.NumeroNotaFiscal;
                txtValorTotalNF.Text = evento.ValorNotaFiscal.HasValue ? evento.ValorNotaFiscal.Value.ToString("N2") : "";
                txtDataNF.Text = evento.DataNotaFiscal.HasValue ? evento.DataNotaFiscal.Value.ToString("dd/MM/yyyy") : "";
                txtValorPagoNF.Text = evento.ValorPagamento.ToString("N2");
                txtDataPagamentoNF.Text = evento.DataPagamento.HasValue ? evento.DataPagamento.Value.ToString("dd/MM/yyyy") : "";
                lnkVisualizarNotaFiscal.Visible = evento.NotaFiscalArquivoId.HasValue;
                lnkVisualizarNotaFiscal.CommandArgument = evento.NotaFiscalArquivoId.ToString() + "," + (!evento.NotaFiscalTipoArquivo.IsNullOrEmptyOrWhiteSpace() ? evento.NotaFiscalTipoArquivo : "");
                lnkVisualizarComprovantePgto.Visible = evento.ComprovantePagamentoArquivoId.HasValue;
                lnkVisualizarComprovantePgto.CommandArgument = evento.ComprovantePagamentoArquivoId.ToString() + "," + (!evento.ComprovantePagamentoTipoArquivo.IsNullOrEmptyOrWhiteSpace() ? evento.ComprovantePagamentoTipoArquivo : "");

                tbXML.Visible = false;

                txtObservacao.Text = evento.Observacoes;
                lnkVisualizarEvidencia.Visible = evento.EvidenciaArquivoId.HasValue;
                lnkVisualizarEvidencia.CommandArgument = evento.EvidenciaArquivoId.ToString() + "," + (!evento.EvidenciaTipoArquivo.IsNullOrEmptyOrWhiteSpace() ? evento.EvidenciaTipoArquivo : "");
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ConsultaPequenaDespesaComComprovacaoExistente(DadosEvento evento)
        {
            try
            {
                lblPlanoTrabalho.Text = evento.PlanoTrabalhoDescricao;
                lblUnidadeEnsino.Text = evento.CensoNomeComp;
                lblTipoEvento.Text = evento.DescricaoTipoDespesa;
                lblNumeroEvento.Text = evento.NumeroEvento;
                lblFinalidade.Text = evento.FinalidadeDescricao;

                txtDescricao.Text = evento.Descricao;

                var fornecedor = rnFornecedor.ObtemPor(evento.FornecedorId ?? 0);
                if (fornecedor != null)
                {
                    lblFornecedor_DCC.Text = fornecedor.FornecedorId + " - " + fornecedor.RazaoSocial;
                }
                else
                {
                    lblFornecedor_DCC.Text = "";
                }

                txtDataNF_DCC.Text = evento.DataNotaFiscal.HasValue ? evento.DataNotaFiscal.Value.ToString("dd/MM/yyyy") : "";               
                txtValorPagoNF_DCC.Text = evento.ValorPagamento.ToString("N2");
                txtDataPagamento_DCC.Text = evento.DataPagamento.HasValue ? evento.DataPagamento.Value.ToString("dd/MM/yyyy") : "";
                ddlFormaPagamento_DCC.SelectedValue = evento.FormaPagamento;

                tbXML.Visible = false;
                tbChaveAcesso.Visible = false;

                txtNumeroNF_DCC.Text = evento.NumeroNotaFiscal;
                txtValorNF_DCC.Text = evento.ValorNotaFiscal.HasValue ? evento.ValorNotaFiscal.Value.ToString("N2") : "";
                txtValorPagoNF_DCC.Text = evento.ValorPagamento.ToString("N2");
                lnkVisualizarNotaFiscal_DCC.Visible = evento.NotaFiscalArquivoId.HasValue;
                lnkVisualizarNotaFiscal_DCC.CommandArgument = evento.NotaFiscalArquivoId.ToString() + "," + (!evento.NotaFiscalTipoArquivo.IsNullOrEmptyOrWhiteSpace() ? evento.NotaFiscalTipoArquivo : "");
                lnkVisualizarComprovantePgto_DCC.Visible = evento.ComprovantePagamentoArquivoId.HasValue;
                lnkVisualizarComprovantePgto_DCC.CommandArgument = evento.ComprovantePagamentoArquivoId.ToString() + "," + (!evento.ComprovantePagamentoTipoArquivo.IsNullOrEmptyOrWhiteSpace() ? evento.ComprovantePagamentoTipoArquivo : "");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void ConsultaPequenaDespesaSemComprovacaoExistente(DadosEvento evento)
        {
            try
            {
                lblPlanoTrabalho.Text = evento.PlanoTrabalhoDescricao;
                lblUnidadeEnsino.Text = evento.CensoNomeComp;
                lblTipoEvento.Text = evento.DescricaoTipoDespesa;
                lblNumeroEvento.Text = evento.NumeroEvento;
                lblFinalidade.Text = evento.FinalidadeDescricao;

                txtDescricao.Text = evento.Descricao;

                tbXML.Visible = false;
                tbChaveAcesso.Visible = false;

                var fornecedor = rnFornecedor.ObtemPor(evento.FornecedorId ?? 0);
                if (fornecedor != null)
                {
                    lblFornecedor_DSC.Text = fornecedor.FornecedorId + " - " + fornecedor.RazaoSocial;
                }
                else
                    lblFornecedor_DSC.Text = "";

                txtValorPago_DSC.Text = evento.ValorPagamento.ToString("N2");
                txtDataPagamento_DSC.Text = evento.DataPagamento.HasValue ? evento.DataPagamento.Value.ToString("dd/MM/yyyy") : "";
                ddlFormaPagamento_DSC.SelectedValue = evento.FormaPagamento;
                txtJustificativa_DSC.Text = evento.Justificativa;
            }
            catch
            {
                throw;
            }
        }

        private void ConsultaPequenaDespesaComTransladoExistente(DadosEvento evento)
        {
            try
            {
                lblPlanoTrabalho.Text = evento.PlanoTrabalhoDescricao;
                lblUnidadeEnsino.Text = evento.CensoNomeComp;
                lblTipoEvento.Text = evento.DescricaoTipoDespesa;
                lblNumeroEvento.Text = evento.NumeroEvento;
                lblFinalidade.Text = evento.FinalidadeDescricao;

                txtDescricao.Text = evento.Descricao;

                tbXML.Visible = false;
                tbChaveAcesso.Visible = false;

                ddlModalTransporte_DCTS.Items.Clear();
                ddlModalTransporte_DCTS.Items.Add(new ListItem { Text = "", Value = "" });
                ddlModalTransporte_DCTS.DataSource = ListaTipoTransporte();
                ddlModalTransporte_DCTS.DataBind();
                ddlModalTransporte_DCTS.SelectedValue = evento.TipoTransporteId.HasValue && evento.TipoTransporteId.Value > 0 ? evento.TipoTransporteId.Value.ToString() : "";
                txtOrigem_DCTS.Text = evento.Origem;
                txtDestino_DCTS.Text = evento.Destino;
                txtValorPago_DCTS.Text = evento.ValorPagamento.ToString("N2");
                txtDataPagamento_DCTS.Text = evento.DataPagamento.HasValue ? evento.DataPagamento.Value.ToString("dd/MM/yyyy") : "";
                txtJustificativa_DCTS.Text = evento.Justificativa;
                Servidores_DCTS = evento.Servidores;

                grdServidores_DCTS.DataSource = Servidores_DCTS;
                grdServidores_DCTS.DataBind();
            }
            catch
            {
                throw;
            }
        }

        public DataTable ListaTipoTransporte()
        {
            return rnTipoTransporte.ListaAtivo();
        }

        #endregion

        #region Propriedade auxiliar de modo de tela

        private enum ModoTelaEnum
        {
            FiltroVazio = -1,
            ConsultaVazio = -2,
            ConsultaDespesaComumExistente = 0, //Despesa com NF-e
            ConsultaDespesaDocumentosFiscais = 1, // Despesa com Demais Documentos Fiscais
            ConsultaPequenaDespesaComComprovacaoExistente = 2, //Pequena Despesa
            ConsultaPequenaDespesaSemComprovacaoExistente = 4, //Pequena Despesa sem comprovacao
            ConsultaPequenaDespesaComTransladoExistente = 3,
        }

        private ModoTelaEnum ModoTela
        {
            get
            {
                return (ModoTelaEnum)(ViewState["ModoTela"] ?? ModoTelaEnum.FiltroVazio);
            }
            set
            {
                var tabDespesaComum = pcAnalisarEvento.TabPages.FindByName("tabDespesaComum");
                var tabPequenaDespesaComComprovacao = pcAnalisarEvento.TabPages.FindByName("tabPequenaDespesaComComprovacao");
                var tabPequenaDespesaSemComprovacao = pcAnalisarEvento.TabPages.FindByName("tabPequenaDespesaSemComprovacao");
                var tabPequenaDespesaComTranslado = pcAnalisarEvento.TabPages.FindByName("tabPequenaDespesaComTranslado");
                var tabExigencias = pcAnalisarEvento.TabPages.FindByName("tabExigencias");

                switch (value)
                {
                    case ModoTelaEnum.FiltroVazio:

                        plaAnalisarEvento.Visible = false;

                        break;

                    case ModoTelaEnum.ConsultaVazio:

                        plaAnalisarEvento.Visible = false;

                        break;

                    case ModoTelaEnum.ConsultaDespesaComumExistente:

                        plaAnalisarEvento.Visible = true;

                        pnlDescricao.Visible = true;
                        txtDescricao.Enabled = false;

                        tabDespesaComum.ClientVisible = true;
                        tabPequenaDespesaComComprovacao.ClientVisible = false;
                        tabPequenaDespesaComTranslado.ClientVisible = false;
                        tabExigencias.ClientVisible = true;

                        pcAnalisarEvento.ActiveTabPage = tabDespesaComum;

                        break;

                    case ModoTelaEnum.ConsultaDespesaDocumentosFiscais:

                        plaAnalisarEvento.Visible = true;

                        pnlDescricao.Visible = true;
                        txtDescricao.Enabled = false;

                        tabDespesaComum.ClientVisible = true;
                        tabPequenaDespesaComComprovacao.ClientVisible = false;
                        tabPequenaDespesaComTranslado.ClientVisible = false;
                        tabExigencias.ClientVisible = true;

                        pcAnalisarEvento.ActiveTabPage = tabDespesaComum;

                        break;
                    case ModoTelaEnum.ConsultaPequenaDespesaComComprovacaoExistente:

                        plaAnalisarEvento.Visible = true;

                        pnlDescricao.Visible = true;
                        txtDescricao.Enabled = false;

                        tabDespesaComum.ClientVisible = false;
                        tabPequenaDespesaComComprovacao.ClientVisible = true;
                        tabPequenaDespesaSemComprovacao.ClientVisible = false;
                        tabPequenaDespesaComTranslado.ClientVisible = false;
                        tabExigencias.ClientVisible = true;

                        pcAnalisarEvento.ActiveTabPage = tabPequenaDespesaComComprovacao;

                        break;

                    case ModoTelaEnum.ConsultaPequenaDespesaSemComprovacaoExistente:

                        plaAnalisarEvento.Visible = true;

                        pnlDescricao.Visible = true;
                        txtDescricao.Enabled = false;

                        tabDespesaComum.ClientVisible = false;
                        tabPequenaDespesaComComprovacao.ClientVisible = false;
                        tabPequenaDespesaSemComprovacao.ClientVisible = true;
                        tabPequenaDespesaComTranslado.ClientVisible = false;
                        tabExigencias.ClientVisible = true;

                        pcAnalisarEvento.ActiveTabPage = tabPequenaDespesaComComprovacao;

                        break;
                    case ModoTelaEnum.ConsultaPequenaDespesaComTransladoExistente:

                        plaAnalisarEvento.Visible = true;

                        pnlDescricao.Visible = true;
                        txtDescricao.Enabled = false;

                        tabDespesaComum.ClientVisible = false;
                        tabPequenaDespesaComComprovacao.ClientVisible = false;
                        tabPequenaDespesaComTranslado.ClientVisible = true;
                        tabExigencias.ClientVisible = true;

                        pcAnalisarEvento.ActiveTabPage = tabPequenaDespesaComTranslado;

                        break;
                }

                ViewState["ModoTela"] = value;
            }
        }

        #endregion

        protected IList<Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosPequenaDespesaServidor> Servidores_DCTS
        {
            get
            {
                if (ViewState["Servidores_DCTS"] == null)
                    ViewState["Servidores_DCTS"] = new List<Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosPequenaDespesaServidor>();

                return (ViewState["Servidores_DCTS"] as List<Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosPequenaDespesaServidor>);
            }
            set
            {
                ViewState["Servidores_DCTS"] = value;
            }
        }

        public List<DadosEventoArquivo> ListaTodosOsArquivosDaDespesa(int eventoId)
        {
            return rnEvento.ListaTodosOsArquivosDaDespesa(eventoId);
        }
    }
}