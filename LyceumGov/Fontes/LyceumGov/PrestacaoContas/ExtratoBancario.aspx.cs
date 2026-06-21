using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxTabControl;
using DevExpress.Web.Data;
using Techne.Lyceum.RN.PrestacaoContas;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [NavUrl("~/PrestacaoContas/ExtratoBancario.aspx"), ControlText("Extrato Bancário"), Title("Extrato Bancário")]
    public partial class ExtratoBancario : TPage
    {
        private RN.PeriodoLetivo rnPeriodoLetivo = new RN.PeriodoLetivo();
        private RN.PrestacaoContas.ExtratoBancario rnExtratoBancario = new RN.PrestacaoContas.ExtratoBancario();
        private RN.PrestacaoContas.ExtratoBancarioArquivo rnExtratoBancarioArquivo = new RN.PrestacaoContas.ExtratoBancarioArquivo();
        private RN.PrestacaoContas.ExigenciaExtrato rnExigenciaExtrato = new RN.PrestacaoContas.ExigenciaExtrato();
        private RN.PrestacaoContas.ExigenciaExtratoArquivo rnExigenciaExtratoArquivo = new RN.PrestacaoContas.ExigenciaExtratoArquivo();

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnNovo, AcaoControle.novo);
            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnEnviarParaAnalise, AcaoControle.editar);
            ControlaAcesso(btnImportar, AcaoControle.editar);
            ControlaAcesso(grdExigenciaExtrato, AcaoControle.editar, "btnDetalhes");
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdExigenciaExtrato, string.Empty);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                var mensagens = new List<string>();
                pcExtratoBancario.Visible = false;

                if (ddlMes.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                    mensagens.Add("MÊS: Preenchimento obrigatório");

                if (ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                    mensagens.Add("ANO: Preenchimento obrigatório");

                if ((tseUnidadeEnsino.Value as string).IsNullOrEmptyOrWhiteSpace())
                    mensagens.Add("UNIDADE DE ENSINO: Preenchimento obrigatório");

                if (mensagens.Any())
                {
                    plaExtratoBancario.Visible = false;

                    hidMes.Value = String.Empty;
                    hidAno.Value = String.Empty;
                    hidUnidadeEnsino.Value = String.Empty;

                    hidStatus.Value = String.Empty;
                    hidExtratoBancarioId.Value = String.Empty;
                    lnkVisualizar.CommandArgument = String.Empty;
                    txtObservacao.Text = String.Empty;

                    lblMensagem.Text = mensagens.Aggregate((i, j) => i + "<br />" + j);
                    return;
                }

                AtualizarTela();

                lblMensagem.Text = String.Empty;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void pcExtratoBancario_TabClick(object sender, TabControlCancelEventArgs e) 
        {
            lblMensagem.Text = string.Empty;
        }

        protected void btnNovo_Click(object sender, EventArgs e)
        {
            ModoTela = ModoTelaEnum.InserirNovo;

            lblMensagem.Text = String.Empty;
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            ModoTela = ModoTelaEnum.EditarExistente;

            lblMensagem.Text = String.Empty;
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                var eb = new RN.PrestacaoContas.DTOs.DadosExtratoBancario();
                eb.ExtratoBancarioId = !hidExtratoBancarioId.Value.IsNullOrEmptyOrWhiteSpace() ? int.Parse(hidExtratoBancarioId.Value) : 0;
                eb.Ano = int.Parse(hidAno.Value);
                eb.Mes = int.Parse(hidMes.Value);
                eb.Censo = hidUnidadeEnsino.Value;
                eb.Arquivo = filExtratoBancario.FileBytes;
                eb.TipoArquivo = filExtratoBancario.PostedFile.ContentType;
                eb.NomeArquivo = filExtratoBancario.FileName;
                eb.Observacao = txtObservacao.Text;
                eb.UsuarioId = User.Identity.Name;

                var validacao = rnExtratoBancario.Valida(eb);

                if (validacao.Valido)
                {
                    if (hidExtratoBancarioId.Value.IsNullOrEmptyOrWhiteSpace())
                        rnExtratoBancario.Insere(eb);

                    else
                        rnExtratoBancario.Atualiza(eb);

                    btnBuscar_Click(sender, e);
                    ModoTela = ModoTelaEnum.ConsultaExistente;

                    lblMensagem.Text = String.Empty;
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            btnBuscar_Click(sender, e);

            lblMensagem.Text = String.Empty;
        }

        protected void btnEnviarParaAnalise_Click(object sender, EventArgs e)
        {
            try
            {
                var extratoBancarioId = 0;
                int.TryParse(hidExtratoBancarioId.Value ?? "", out extratoBancarioId);
                if (extratoBancarioId == 0)
                {
                    lblMensagem.Text = "STATUS: É necessário selecionar um EXTRATO BANCÁRIO antes";
                    return;
                }

                var status = (int?)null;
                try { status = Convert.ToInt32(hidStatus.Value); }
                catch { status = null; }

                switch (status)
                {
                    case null:
                        rnExtratoBancario.AtualizaStatus(extratoBancarioId, 1);
                        lblMensagem.Text = "Extrato enviado para análise com sucesso";
                        break;

                    case 2:
                        var validacao = rnExtratoBancario.ValidaAtualizacaoStatus(extratoBancarioId, status);
                        if (validacao.Valido)
                        {
                            rnExtratoBancario.AtualizaStatus(extratoBancarioId, 3);
                            lblMensagem.Text = "Extrato revisado pela AAE com sucesso";
                            break;
                        }
                        else
                        {
                            lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                            return;
                        }

                    default:
                        lblMensagem.Text = "Status desconhecido. Nenhuma ação foi realizada.";
                        break;
                }

                btnBuscar_Click(sender, e);
                ModoTela = ModoTelaEnum.ConsultaExistente;
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
                hdnExigenciaExtratoId.Value = string.Empty;
                txtJustificativa.Text = string.Empty;

                string[] chave = e.CommandArgument.ToString().Split(new char[] { ',' });

                hdnExigenciaExtratoId.Value = chave[0].ToString();

                pucConfirmarArquivo.ShowOnPageLoad = true;

                lblMensagem.Text = String.Empty;
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
                RN.PrestacaoContas.Entidades.ExigenciaExtrato exigenciaExtrato = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ExigenciaExtrato();
                RN.PrestacaoContas.Entidades.ExigenciaExtratoArquivo exigenciaExtratoArquivo = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ExigenciaExtratoArquivo();
                ValidacaoDados validacao = new ValidacaoDados();
                ValidacaoDados validacaoArquivo = new ValidacaoDados();

                byte[] imageBytes = new byte[FileUpload1.PostedFile.InputStream.Length + 1];
                FileUpload1.PostedFile.InputStream.Read(imageBytes, 0, imageBytes.Length);

                exigenciaExtrato.ExigenciaExtratoId = !hdnExigenciaExtratoId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnExigenciaExtratoId.Value) : -1;
                exigenciaExtrato.Justificativa = txtJustificativa.Text;
                exigenciaExtrato.UsuarioId = User.Identity.Name;
                exigenciaExtrato.DataAlteracao = DateTime.Now;

                exigenciaExtratoArquivo.ExigenciaExtratoId = !hdnExigenciaExtratoId.Value.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(hdnExigenciaExtratoId.Value) : -1;
                exigenciaExtratoArquivo.ChaveArquivo = Guid.NewGuid().ToString();
                exigenciaExtratoArquivo.Arquivo = imageBytes;
                exigenciaExtratoArquivo.NomeArquivo = FileUpload1.PostedFile.FileName;
                exigenciaExtratoArquivo.TipoArquivo = FileUpload1.PostedFile.ContentType;
                exigenciaExtratoArquivo.UsuarioId = User.Identity.Name;
                exigenciaExtratoArquivo.DataCadastro = DateTime.Now;
                exigenciaExtratoArquivo.DataAlteracao = DateTime.Now;

                validacao = rnExigenciaExtrato.ValidaJustificativa(exigenciaExtrato);
                if (!validacao.Valido)
                    lblMensagem.Text += (lblMensagem.Text.Length > 0 ? "<br />" : "") + validacao.Mensagem.Replace(Environment.NewLine, "<br />");

                validacaoArquivo = rnExigenciaExtratoArquivo.Valida(exigenciaExtratoArquivo);
                if (!validacaoArquivo.Valido)
                    lblMensagem.Text += (lblMensagem.Text.Length > 0 ? "<br />" : "") + validacaoArquivo.Mensagem.Replace(Environment.NewLine, "<br />");

                if (!validacao.Valido || !validacaoArquivo.Valido)
                    return;

                rnExigenciaExtrato.AtualizaJustificativa(exigenciaExtrato);
                rnExigenciaExtratoArquivo.Atualiza(exigenciaExtratoArquivo);
                lblMensagem.Text = "Arquivo atualizado com sucesso.";

                grdExigenciaExtrato.DataBind();
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
                var tabela = "";
                IExtratoBancarioArquivo rnArquivo = null;

                switch (((WebControl)sender).ID)
                {
                    case "lnkVisualizar":
                        tabela = "ExtratoBancarioArquivo";
                        rnArquivo = rnExtratoBancarioArquivo;
                        break;

                    case "btnVisualizar":
                        tabela = "ExigenciaExtratoArquivo";
                        rnArquivo = rnExigenciaExtratoArquivo;
                        break;

                    default:
                        throw new Exception("Deu erro na rotina de visualização de fotos.");
                }

                string embed = string.Empty;
                bimgArquivo.Visible = false;
                ltEmbed.Visible = false;
                DataTable dados = new DataTable();

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

                lblMensagem.Text = String.Empty;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdExigenciaExtrato_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();

                var ee = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ExigenciaExtrato();

                ee.ExigenciaExtratoId = Convert.ToInt32(grdExigenciaExtrato.GetRowValues(grdExigenciaExtrato.EditingRowVisibleIndex, "EXIGENCIAEXTRATOID"));
                ee.Justificativa = Convert.ToString(e.NewValues["JUSTIFICATIVA"] ?? "");
                ee.UsuarioId = User.Identity.Name;

                validacao = rnExigenciaExtrato.ValidaJustificativa(ee);

                if (validacao.Valido)
                {
                    rnExigenciaExtrato.AtualizaJustificativa(ee);
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }

                grdExigenciaExtrato.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdExigenciaExtrato_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            try
            {
                var btnDetalhes = (ImageButton)e.Row.Cells[3].Controls[0].Controls[1];
                ControlaAcesso(btnDetalhes, AcaoControle.editar);
            }
            catch
            {
            }
        }

        public object ListaAno()
        {
            return rnPeriodoLetivo.ListaAnos(false);
        }

        public DataTable ListaExigenciaExtrato(int extratoBancarioId)
        {
            if (new int?[] { null, 1 }.Contains(rnExtratoBancario.ObtemStatus(extratoBancarioId)))
                return null;

            return rnExigenciaExtrato.ListaPor(extratoBancarioId);
        }

        public void UpdateExigenciaExtrato(object JUSTIFICATIVA, object EXIGENCIAEXTRATOID) 
        { 
        }

        public enum ModoTelaEnum
        {
            ConsultaVazio = 0,
            ConsultaExistente = 1,
            InserirNovo = 2,
            EditarExistente = 3,
        }
        
        public ModoTelaEnum ModoTela
        {
            get
            {
                if (btnNovo.Visible == false && btnEditar.Visible == false && btnSalvar.Visible == true && btnCancel.Visible == true)
                    if (hidExtratoBancarioId.Value.IsNullOrEmptyOrWhiteSpace())
                        return ModoTelaEnum.InserirNovo;
                    else
                        return ModoTelaEnum.EditarExistente;
                else
                    if (hidExtratoBancarioId.Value.IsNullOrEmptyOrWhiteSpace())
                        return ModoTelaEnum.ConsultaVazio;
                    else
                        return ModoTelaEnum.ConsultaExistente;

            }
            set
            {
                switch (value)
                {
                    case ModoTelaEnum.ConsultaVazio:

                        btnNovo.Visible = true;
                        btnEditar.Visible = false;
                        btnSalvar.Visible = false;
                        btnCancel.Visible = false;
                        btnEnviarParaAnalise.Visible = false;

                        plaVazio.Visible = true;
                        plaExistente.Visible = false;
                        plaStatus.Visible = false;
                        txtObservacao.Enabled = false;
                        filExtratoBancario.Visible = false;
                        lnkVisualizar.Visible = false;
                        pcExtratoBancario.TabPages.FindByName("tabExigenciasExtrato").Enabled = false;

                        break;

                    case ModoTelaEnum.ConsultaExistente:

                        btnNovo.Visible = false;
                        btnEditar.Visible = new string[] { String.Empty }.Contains(hidStatus.Value);
                        btnSalvar.Visible = false;
                        btnCancel.Visible = false;
                        btnEnviarParaAnalise.Visible = new string[] { String.Empty, "2" }.Contains(hidStatus.Value);

                        plaVazio.Visible = false;
                        plaExistente.Visible = true;
                        plaStatus.Visible = true;
                        txtObservacao.Enabled = false;
                        filExtratoBancario.Visible = false;
                        lnkVisualizar.Visible = true;
                        pcExtratoBancario.TabPages.FindByName("tabExigenciasExtrato").Enabled = true;

                        break;

                    case ModoTelaEnum.InserirNovo:

                        btnNovo.Visible = false;
                        btnEditar.Visible = false;
                        btnSalvar.Visible = true;
                        btnCancel.Visible = true;
                        btnEnviarParaAnalise.Visible = false;

                        plaVazio.Visible = false;
                        plaExistente.Visible = true;
                        plaStatus.Visible = false;
                        txtObservacao.Enabled = true;
                        filExtratoBancario.Visible = true;
                        lnkVisualizar.Visible = false;
                        pcExtratoBancario.TabPages.FindByName("tabExigenciasExtrato").Enabled = false;

                        break;

                    case ModoTelaEnum.EditarExistente:

                        btnNovo.Visible = false;
                        btnEditar.Visible = false;
                        btnSalvar.Visible = true;
                        btnCancel.Visible = true;
                        btnEnviarParaAnalise.Visible = false;

                        plaVazio.Visible = false;
                        plaExistente.Visible = true;
                        plaStatus.Visible = true;
                        txtObservacao.Enabled = true;
                        filExtratoBancario.Visible = new string[] { String.Empty }.Contains(hidStatus.Value); 
                        lnkVisualizar.Visible = false;
                        pcExtratoBancario.TabPages.FindByName("tabExigenciasExtrato").Enabled = false;

                        break;
                }
            }
        }

        public void AtualizarTela()
        {
            hidMes.Value = ddlMes.SelectedValue;
            hidAno.Value = ddlAno.SelectedValue;
            hidUnidadeEnsino.Value = tseUnidadeEnsino.Value.ToString();

            var extratoBancario = rnExtratoBancario.ObtemExtratoBancario(int.Parse(hidMes.Value), int.Parse(hidAno.Value), hidUnidadeEnsino.Value);

            if (extratoBancario.Rows.Count > 0)
            {
                var row = extratoBancario.Rows[0];
                var status = row["STATUS"] != DBNull.Value ? (int?)Convert.ToInt32(row["STATUS"]) : null;

                hidStatus.Value = status.HasValue ? status.Value.ToString() : String.Empty;
                lblStatus.Text = ObterDescricaoStatus(hidStatus.Value);

                hidExtratoBancarioId.Value = Convert.ToString(row["EXTRATOBANCARIOID"]);
                lnkVisualizar.CommandArgument = Convert.ToString(row["EXTRATOBANCARIOID"]) + "," + Convert.ToString(row["TIPOARQUIVO"]);
                txtObservacao.Text = Convert.ToString(row["OBSERVACAO"]);

                grdExigenciaExtrato.DataBind();

                ModoTela = ModoTelaEnum.ConsultaExistente;
            }
            else
            {
                hidStatus.Value = String.Empty;
                lblStatus.Text = String.Empty;

                hidExtratoBancarioId.Value = String.Empty;
                lnkVisualizar.CommandArgument = String.Empty;
                txtObservacao.Text = String.Empty;

                ModoTela = ModoTelaEnum.ConsultaVazio;
            }

            plaExtratoBancario.Visible = true;
            pcExtratoBancario.Visible = true;
        }

        public string ObterDescricaoStatus(string status)
        {
            switch (status)
            {
                case "":
                    return "Lançamento pela AAE";
                case "1":
                    return "Enviado para análise";
                case "2":
                    return "Devolvido para revisão";
                case "3":
                    return "Revisado pela AAE";
                case "4":
                    return "Aprovado";
                case "5":
                    return "Reprovado";
                default:
                    return "(Status desconhecido)";
            }
        }
    }
}
