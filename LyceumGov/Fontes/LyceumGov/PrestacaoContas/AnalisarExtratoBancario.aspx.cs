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
using DevExpress.Web.ASPxClasses;
using System.Web.UI.HtmlControls;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [NavUrl("~/PrestacaoContas/AnalisarExtratoBancario.aspx"), ControlText("Analisar Extrato Bancário"), Title("Analisar Extrato Bancário")]
    public partial class AnalisarExtratoBancario : TPage
    {
        private RN.PeriodoLetivo rnPeriodoLetivo = new RN.PeriodoLetivo();
        private RN.PrestacaoContas.ExtratoBancario rnExtratoBancario = new RN.PrestacaoContas.ExtratoBancario();
        private RN.PrestacaoContas.ExtratoBancarioArquivo rnExtratoBancarioArquivo = new RN.PrestacaoContas.ExtratoBancarioArquivo();
        private RN.PrestacaoContas.ExigenciaExtrato rnExigenciaExtrato = new RN.PrestacaoContas.ExigenciaExtrato();
        private RN.PrestacaoContas.ExigenciaExtratoArquivo rnExigenciaExtratoArquivo = new RN.PrestacaoContas.ExigenciaExtratoArquivo();
        private RN.PrestacaoContas.TipoExigenciaExtrato rnTipoExigenciaExtrato = new RN.PrestacaoContas.TipoExigenciaExtrato();

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdExigenciaExtrato);
            ControlaAcesso(btnAprovar, AcaoControle.editar);
            ControlaAcesso(btnReprovar, AcaoControle.editar);
            ControlaAcesso(btnDevolver, AcaoControle.editar);
            AcessoGrid();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdExtratoBancario, "Extratos Bancários");
            TituloGrid(grdExigenciaExtrato, string.Empty);
        }

        protected void AcessoGrid()
        {
            if (grdExigenciaExtrato != null)
            {
                HtmlImage img = (HtmlImage)grdExigenciaExtrato.FindHeaderTemplateControl(grdExigenciaExtrato.Columns[""], "btnNovoGrid");               
                
                if (img != null)
                {
                    if (Permission.AllowInsert)
                    {
                        img.Visible = true;

                        if (hidStatus.Value != "1" && hidStatus.Value != "3" && !hidStatus.Value.IsNullOrEmptyOrWhiteSpace())
                        {
                            img.Visible = false ;
                        }
                       
                    }
                    else
                    {
                        img.Visible = false;
                    }


                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                ModoTela = ModoTelaEnum.TelaInicial;

                var mensagens = new List<string>();

                if ((tseUnidadeEnsino.Value as string).IsNullOrEmptyOrWhiteSpace())
                    mensagens.Add("UNIDADE DE ENSINO: Preenchimento obrigatório");

                if (mensagens.Any())
                {
                    odsExtratoBancario.SelectParameters["mes"].DefaultValue = string.Empty;
                    odsExtratoBancario.SelectParameters["ano"].DefaultValue = string.Empty;
                    odsExtratoBancario.SelectParameters["censo"].DefaultValue = string.Empty;

                    lblMensagem.Text = mensagens.Aggregate((i, j) => i + "<br />" + j);
                    return;
                }

                odsExtratoBancario.SelectParameters["mes"].DefaultValue = ddlMes.SelectedValue;
                odsExtratoBancario.SelectParameters["ano"].DefaultValue = ddlAno.SelectedValue;
                odsExtratoBancario.SelectParameters["censo"].DefaultValue = tseUnidadeEnsino.Value.ToString();

                grdExtratoBancario.DataBind();
                grdExtratoBancario.FocusedRowIndex = -1;

                ModoTela = ModoTelaEnum.TelaSomenteGrid;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdExtratoBancario_FocusedRowChanged(object sender, EventArgs e)
        {
            if (!IsPostBack)
                return;

            var self = (ASPxGridView)sender;

            ModoTela = ModoTelaEnum.TelaSomenteGrid;

            var extratoBancarioId = Convert.ToInt32(self.GetRowValues(self.FocusedRowIndex, "EXTRATOBANCARIOID"));
            var censo = Convert.ToString(self.GetRowValues(self.FocusedRowIndex, "CENSO"));
            var mes = Convert.ToInt32(self.GetRowValues(self.FocusedRowIndex, "MES"));
            var ano = Convert.ToInt32(self.GetRowValues(self.FocusedRowIndex, "ANO"));

            if (extratoBancarioId == -1)
            {
                hidExtratoBancarioId.Value = String.Empty;
                hidMes.Value = String.Empty;
                hidAno.Value = String.Empty;
                hidUnidadeEnsino.Value = String.Empty;

                lnkVisualizar.CommandArgument = String.Empty;
                txtObservacao.Text = String.Empty;

                return;
            }

            hidMes.Value = mes.ToString();
            hidAno.Value = ano.ToString();
            hidUnidadeEnsino.Value = censo;

            var extratoBancario = rnExtratoBancario.ObtemExtratoBancario(extratoBancarioId);

            if (extratoBancario.Rows.Count == 0)
            {
                hidStatus.Value = String.Empty;
                lblStatus.Text = String.Empty;

                hidExtratoBancarioId.Value = String.Empty;
                lnkVisualizar.CommandArgument = String.Empty;
                txtObservacao.Text = String.Empty;
                grdExigenciaExtrato.DataBind();

                ModoTela = ModoTelaEnum.TelaExtratoNaoPreenchido;

                return;
            }

            var row = extratoBancario.Rows[0];

            var status = row["STATUS"] != DBNull.Value ? (int?)Convert.ToInt32(row["STATUS"]) : null;
            hidStatus.Value = status.HasValue ? status.Value.ToString() : String.Empty;
            lblStatus.Text = ObterDescricaoStatus(hidStatus.Value);

            hidExtratoBancarioId.Value = extratoBancarioId.ToString();
            lnkVisualizar.CommandArgument = Convert.ToString(extratoBancarioId) + "," + Convert.ToString(row["TIPOARQUIVO"]);
            txtObservacao.Text = Convert.ToString(row["OBSERVACAO"]);
            grdExigenciaExtrato.DataBind();

            switch (status)
            {
                case null: 
                    ModoTela = ModoTelaEnum.TelaExtratoLancamentoPelaAAE;
                    break;

                case 1: 
                    ModoTela = ModoTelaEnum.TelaExtratoEnviadoParaAnalise;
                    break;

                case 2: 
                    ModoTela = ModoTelaEnum.TelaExtratoDevolvidoParaAAE;
                    break;

                case 3: 
                    ModoTela = ModoTelaEnum.TelaExtratoRevisadoPelaAAE;
                    break;
                
                case 4: 
                    ModoTela = ModoTelaEnum.TelaExtratoAprovado;
                    break;
                    
                case 5: 
                    ModoTela = ModoTelaEnum.TelaExtratoReprovado;
                    break;
            }
        }

        protected void pcExtratoBancario_TabClick(object sender, TabControlCancelEventArgs e) 
        {
            lblMensagem.Text = string.Empty;
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
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnAprovar_Click(object sender, EventArgs e)
        {
            try
            {
                var extratoBancarioId = 0;
                int.TryParse(hidExtratoBancarioId.Value ?? "", out extratoBancarioId);
                if (extratoBancarioId == 0)
                    throw new Exception("STATUS: É necessário selecionar um EXTRATO BANCÁRIO antes");

                var possuiExigenciasNaoAnalisadas = rnExtratoBancario.PossuiExigenciasNaoAnalisadas(extratoBancarioId);
                if (possuiExigenciasNaoAnalisadas)
                    throw new Exception("Não é possível aprovar este extrato bancário porque há exigências não analisadas");

                var possuiExigenciasReprovadas = rnExtratoBancario.PossuiExigenciasReprovadas(extratoBancarioId);
                if (possuiExigenciasReprovadas)
                    throw new Exception("Não é possível aprovar este extrato bancário porque há exigências reprovadas");

                var status = (int?)null;
                try { status = Convert.ToInt32(hidStatus.Value); }
                catch { status = null; }

                switch (status)
                {
                    case 1:
                    case 3:
                        rnExtratoBancario.AtualizaStatus(extratoBancarioId, 4);
                        lblMensagem.Text = "Extrato aprovado com sucesso";
                        break;

                    default:
                        lblMensagem.Text = "Status desconhecido. Nenhuma ação foi realizada.";
                        break;
                }

                grdExtratoBancario.FocusedRowChanged -= grdExtratoBancario_FocusedRowChanged;
                grdExtratoBancario.DataBind();
                grdExtratoBancario.FocusedRowChanged += grdExtratoBancario_FocusedRowChanged;
                grdExtratoBancario_FocusedRowChanged(grdExtratoBancario, e);

                ModoTela = ModoTelaEnum.TelaExtratoAprovado;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnReprovar_Click(object sender, EventArgs e)
        {
            try
            {
                var extratoBancarioId = 0;
                int.TryParse(hidExtratoBancarioId.Value ?? "", out extratoBancarioId);
                if (extratoBancarioId == 0)
                    throw new Exception("STATUS: É necessário selecionar um EXTRATO BANCÁRIO antes");

                var possuiExigenciasNaoAnalisadas = rnExtratoBancario.PossuiExigenciasNaoAnalisadas(extratoBancarioId);
                if (possuiExigenciasNaoAnalisadas)
                    throw new Exception("Não é possível reprovar este extrato bancário porque há exigências não analisadas");

                var status = (int?)null;
                try { status = Convert.ToInt32(hidStatus.Value); }
                catch { status = null; }

                switch (status)
                {
                    case 1:
                    case 3:
                        rnExtratoBancario.AtualizaStatus(extratoBancarioId, 5);
                        lblMensagem.Text = "Extrato reprovado com sucesso";
                        break;

                    default:
                        lblMensagem.Text = "Status desconhecido. Nenhuma ação foi realizada.";
                        break;
                }

                grdExtratoBancario.FocusedRowChanged -= grdExtratoBancario_FocusedRowChanged;
                grdExtratoBancario.DataBind();
                grdExtratoBancario.FocusedRowChanged += grdExtratoBancario_FocusedRowChanged;
                grdExtratoBancario_FocusedRowChanged(grdExtratoBancario, e);

                ModoTela = ModoTelaEnum.TelaExtratoReprovado;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnDevolver_Click(object sender, EventArgs e)
        {
            try
            {
                var extratoBancarioId = 0;
                int.TryParse(hidExtratoBancarioId.Value ?? "", out extratoBancarioId);
                if (extratoBancarioId == 0)
                    throw new Exception("STATUS: É necessário selecionar um EXTRATO BANCÁRIO antes");

                var status = (int?)null;
                try { status = Convert.ToInt32(hidStatus.Value); }
                catch { status = null; }

                switch (status)
                {
                    case 1:
                    case 3:
                        if (grdExigenciaExtrato.VisibleRowCount == 0)
                            throw new Exception("Devolver o extrato bancário requer pelo menos 1 exigência cadastrada");

                        rnExtratoBancario.AtualizaStatus(extratoBancarioId, 2);
                        lblMensagem.Text = "Extrato devolvido para AAE com sucesso";
                        break;

                    default:
                        lblMensagem.Text = "Status desconhecido. Nenhuma ação foi realizada.";
                        break;
                }

                grdExtratoBancario.FocusedRowChanged -= grdExtratoBancario_FocusedRowChanged;
                grdExtratoBancario.DataBind();
                grdExtratoBancario.FocusedRowChanged += grdExtratoBancario_FocusedRowChanged;
                grdExtratoBancario_FocusedRowChanged(grdExtratoBancario, e);

                ModoTela = ModoTelaEnum.TelaExtratoDevolvidoParaAAE;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdExigenciaExtrato_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            var justificada = !Convert.ToString(grdExigenciaExtrato.GetRowValues(e.VisibleIndex, "JUSTIFICATIVA")).IsNullOrEmptyOrWhiteSpace();
            var temArquivo = (grdExigenciaExtrato.GetRowValues(e.VisibleIndex, "ARQUIVO") as byte[] ?? new byte[] { }).Any();
            var aprovado = Convert.ToBoolean(grdExigenciaExtrato.GetRowValues(e.VisibleIndex, "APROVADO") != DBNull.Value ? grdExigenciaExtrato.GetRowValues(e.VisibleIndex, "APROVADO") : 0);

            if (e.ButtonType == ColumnCommandButtonType.Edit)
            {
                e.Visible = !aprovado;

                if (!Permission.AllowUpdate)
                    e.Visible = false;

            }

            if (e.ButtonType == ColumnCommandButtonType.Delete)
            {
                if (!aprovado)
                {
                    if (!justificada || !temArquivo)
                        e.Visible = true;
                    else
                        e.Visible = false;
                }
                else
                    e.Visible = false;


                if (!Permission.AllowUpdate)
                    e.Visible = false;
            }

            var extratoBancarioId = 0;
            int.TryParse(hidExtratoBancarioId.Value, out extratoBancarioId);
            if (extratoBancarioId == 0)
                return;

            var status = rnExtratoBancario.ObtemStatus(extratoBancarioId);
            if (new int?[] { null, 2, 4, 5 }.Contains(status))
            {
                e.Visible = false;
                return;
            }
        }

        protected void grdExigenciaExtrato_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            try
            {
                var exigenciaExtratoId = Convert.ToInt32(grdExigenciaExtrato.GetRowValues(e.VisibleIndex, "EXIGENCIAEXTRATOID"));
                var extratoBancarioId = Convert.ToInt32(grdExigenciaExtrato.GetRowValues(e.VisibleIndex, "EXTRATOBANCARIOID"));

                var status = rnExtratoBancario.ObtemStatus(extratoBancarioId);

                var analisavel = new int?[] { 3 }.Contains(status);
                if (!analisavel)
                {
                    e.Visible = DefaultBoolean.False;
                    return;
                }

                var aprovado = rnExigenciaExtrato.EstaAprovado(exigenciaExtratoId);

                switch (e.ButtonID)
                {
                    case "btnAprovarExigencia":
                        e.Visible = (aprovado ?? false) ? DefaultBoolean.False : DefaultBoolean.True;

                        if (!Permission.AllowUpdate)
                            e.Visible = DefaultBoolean.False;
                        break;

                    case "btnReprovarExigencia":
                        e.Visible = aprovado.HasValue ? DefaultBoolean.False : DefaultBoolean.True;
                        if (!Permission.AllowUpdate)
                            e.Visible = DefaultBoolean.False;
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdExigenciaExtrato_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            try
            {
                var exigenciaExtratoId = Convert.ToInt32(grdExigenciaExtrato.GetRowValues(e.VisibleIndex, "EXIGENCIAEXTRATOID"));

                switch (e.ButtonID)
                {
                    case "btnAprovarExigencia":
                        rnExigenciaExtrato.AtualizaAprovacao(exigenciaExtratoId, true);
                        lblMensagem.Text = "Aprovado com sucesso";
                        grdExigenciaExtrato.DataBind();
                        break;

                    case "btnReprovarExigencia":
                        rnExigenciaExtrato.AtualizaAprovacao(exigenciaExtratoId, false);
                        lblMensagem.Text = "Reprovado com sucesso";
                        grdExigenciaExtrato.DataBind();
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdExigenciaExtrato_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();

                var ee = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ExigenciaExtrato();

                ee.ExtratoBancarioId = Convert.ToInt32(hidExtratoBancarioId.Value);
                ee.TipoExigenciaExtratoId = Convert.ToInt32(e.NewValues["TIPOEXIGENCIAEXTRATOID"]);
                ee.NotaExplicativa = Convert.ToString(e.NewValues["NOTAEXPLICATIVA"]);
                ee.UsuarioId = User.Identity.Name;
                ee.DataCadastro = DateTime.Now;

                validacao = rnExigenciaExtrato.Valida(ee);

                if (validacao.Valido)
                {
                    rnExigenciaExtrato.Insere(ee);
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

        protected void grdExigenciaExtrato_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();

                var ee = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ExigenciaExtrato();

                ee.ExigenciaExtratoId = Convert.ToInt32(grdExigenciaExtrato.GetRowValues(grdExigenciaExtrato.EditingRowVisibleIndex, "EXIGENCIAEXTRATOID"));
                ee.TipoExigenciaExtratoId = Convert.ToInt32(e.NewValues["TIPOEXIGENCIAEXTRATOID"]);
                ee.NotaExplicativa = Convert.ToString(e.NewValues["NOTAEXPLICATIVA"]);
                //ee.Aprovado = ((CheckBox)grdExigenciaExtrato.FindEditRowCellTemplateControl(grdExigenciaExtrato.Columns["APROVADO"] as GridViewDataColumn, "chkAprovado")).Checked;
                ee.UsuarioId = User.Identity.Name;
                ee.DataAlteracao = DateTime.Now;

                validacao = rnExigenciaExtrato.Valida(ee);

                if (validacao.Valido)
                {
                    rnExigenciaExtrato.Atualiza(ee);
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

        protected void grdExigenciaExtrato_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                int exigenciaExtratoId = 0;

                exigenciaExtratoId = Convert.ToInt32(e.Keys["EXIGENCIAEXTRATOID"]);

                validacao = rnExigenciaExtrato.ValidaRemocao(exigenciaExtratoId);

                if (validacao.Valido)
                {
                    rnExigenciaExtrato.Remove(exigenciaExtratoId);
                    grdExigenciaExtrato.DataBind();
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public object ListaExtratoBancario(int? mes, int? ano, string censo)
        {
            return rnExtratoBancario.ListaExtratoBancario(mes, ano, censo);
        }

        public object ListaAno()
        {
            return rnPeriodoLetivo.ListaAnos(false);
        }

        public DataTable ListaTipoExigenciaExtrato()
        {
            return rnTipoExigenciaExtrato.ListaAtivo();
        }

        public DataTable ListaExigenciaExtrato(int extratoBancarioId)
        {
            return rnExigenciaExtrato.ListaPor(extratoBancarioId);
        }

        public void InsertExigenciaExtrato(object TIPOEXIGENCIAEXTRATOID, object NOTAEXPLICATIVA)
        {
        }

        public void UpdateExigenciaExtrato(object EXIGENCIAEXTRATOID, object TIPOEXIGENCIAEXTRATOID, object NOTAEXPLICATIVA, object APROVADO) 
        { 
        }

        public void UpdateExigenciaExtrato(object EXIGENCIAEXTRATOID, object TIPOEXIGENCIAEXTRATOID, object NOTAEXPLICATIVA)
        {
        }

        public void DeleteExigenciaExtrato(object EXIGENCIAEXTRATOID) 
        { }

        public enum ModoTelaEnum
        {
            TelaInicial,
            TelaSomenteGrid,
            TelaExtratoNaoPreenchido,
            TelaExtratoLancamentoPelaAAE,
            TelaExtratoEnviadoParaAnalise,
            TelaExtratoDevolvidoParaAAE,
            TelaExtratoRevisadoPelaAAE,
            TelaExtratoAprovado,
            TelaExtratoReprovado,
        }
        
        public ModoTelaEnum ModoTela
        {
            get
            {
                if (hidExtratoBancarioId.Value.IsNullOrEmptyOrWhiteSpace())
                    return ModoTelaEnum.TelaExtratoNaoPreenchido;
                else
                    return ModoTelaEnum.TelaExtratoEnviadoParaAnalise;
            }
            set
            {
                switch (value)
                {
                    case ModoTelaEnum.TelaInicial:

                        plaGrid.Visible = false;
                        plaExtratoBancario.Visible = false;

                        break;

                    case ModoTelaEnum.TelaSomenteGrid:

                        plaGrid.Visible = true;
                        plaExtratoBancario.Visible = false;

                        break;

                    case ModoTelaEnum.TelaExtratoNaoPreenchido:

                        plaGrid.Visible = true;
                        plaExtratoBancario.Visible = true;

                        btnDevolver.Visible = false;
                        btnReprovar.Visible = false;
                        btnAprovar.Visible = false;

                        plaVazio.Visible = true;
                        plaExistente.Visible = false;
                        plaStatus.Visible = false;
                        txtObservacao.Enabled = false;
                        filExtratoBancario.Visible = false;
                        lnkVisualizar.Visible = false;
                        pcExtratoBancario.TabPages.FindByName("tabExigenciasExtrato").Enabled = false;

                        break;

                    case ModoTelaEnum.TelaExtratoEnviadoParaAnalise:
                    case ModoTelaEnum.TelaExtratoRevisadoPelaAAE:

                        plaGrid.Visible = true;
                        plaExtratoBancario.Visible = true;

                        btnDevolver.Visible = true;
                        btnReprovar.Visible = true;
                        btnAprovar.Visible = true;

                        plaVazio.Visible = false;
                        plaExistente.Visible = true;
                        plaStatus.Visible = true;
                        txtObservacao.Enabled = false;
                        filExtratoBancario.Visible = false;
                        lnkVisualizar.Visible = true;
                        pcExtratoBancario.TabPages.FindByName("tabExigenciasExtrato").Enabled = true;

                        break;

                    case ModoTelaEnum.TelaExtratoLancamentoPelaAAE:
                    case ModoTelaEnum.TelaExtratoDevolvidoParaAAE:
                    case ModoTelaEnum.TelaExtratoAprovado:
                    case ModoTelaEnum.TelaExtratoReprovado:

                        plaGrid.Visible = true;
                        plaExtratoBancario.Visible = true;

                        btnDevolver.Visible = false;
                        btnReprovar.Visible = false;
                        btnAprovar.Visible = false;

                        plaVazio.Visible = false;
                        plaExistente.Visible = true;
                        plaStatus.Visible = true;
                        txtObservacao.Enabled = false;
                        filExtratoBancario.Visible = false;
                        lnkVisualizar.Visible = true;
                        pcExtratoBancario.TabPages.FindByName("tabExigenciasExtrato").Enabled = true;

                        break;
                }
            }
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
