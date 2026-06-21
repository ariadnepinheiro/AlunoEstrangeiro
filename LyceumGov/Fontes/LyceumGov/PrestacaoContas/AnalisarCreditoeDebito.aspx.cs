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
using System.Data.SqlTypes;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [NavUrl("~/PrestacaoContas/AnalisarCreditoeDebito.aspx"), ControlText("Analisar Crédito e Débito"), Title("Analisar Crédito e Débito")]
    public partial class AnalisarCreditoeDebito : TPage
    {
        private RN.PeriodoLetivo rnPeriodoLetivo = new RN.PeriodoLetivo();
        private RN.PrestacaoContas.ExtratoBancario rnExtratoBancario = new RN.PrestacaoContas.ExtratoBancario();
        private RN.PrestacaoContas.ExtratoBancarioArquivo rnExtratoBancarioArquivo = new RN.PrestacaoContas.ExtratoBancarioArquivo();
        private RN.PrestacaoContas.ExigenciaExtrato rnExigenciaExtrato = new RN.PrestacaoContas.ExigenciaExtrato();
        private RN.PrestacaoContas.ExigenciaExtratoArquivo rnExigenciaExtratoArquivo = new RN.PrestacaoContas.ExigenciaExtratoArquivo();
        private RN.PrestacaoContas.TipoExigenciaExtrato rnTipoExigenciaExtrato = new RN.PrestacaoContas.TipoExigenciaExtrato();

        RN.PrestacaoContas.PlanoTrabalho rnPlanoTrabalho = new RN.PrestacaoContas.PlanoTrabalho();
        RN.PrestacaoContas.Operacao rnOperacao = new RN.PrestacaoContas.Operacao();
        RN.PrestacaoContas.OperacaoDocumentos rnOperacaoDocumentos = new RN.PrestacaoContas.OperacaoDocumentos();

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdExigencias);
            ControlaAcesso(btnAprovar, AcaoControle.editar);
            ControlaAcesso(btnReprovar, AcaoControle.editar);
            ControlaAcesso(btnDevolver, AcaoControle.editar);
            AcessoGrid();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
         //   TituloGrid(grdExtratoBancario, "Extratos Bancários");
            TituloGrid(grdExigencias, string.Empty);
            TituloGrid(grdRegistro, "");
        }

        protected void AcessoGrid()
        {
            if (grdExigencias != null)
            {
                HtmlImage img = (HtmlImage)grdExigencias.FindHeaderTemplateControl(grdExigencias.Columns[""], "btnNovoGrid");               
                
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
        protected void tsePlanoTrabalho_Changed(object sender, EventArgs args)
        {
            try
            {
                if (!tsePlanoTrabalho.DBValue.IsNull)
                {
                    if (tsePlanoTrabalho.IsValidDBValue)
                    {
                        DataContext contexto = null;
                        contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                        string[] dadosidentificador = rnPlanoTrabalho.ObtemIdentificadorPor(contexto, Convert.ToInt32(tsePlanoTrabalho.DBValue));
                       // lblIdentificador.Text = dadosidentificador[0];
                       // lblDescricao.Text = dadosidentificador[1];
                    }
                    else
                    {
                        //this._tipoOperacao = TipoOperacao.Inicial;
                        lblMensagem.Text = "Plano de Trabalho não ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Plano de Trabalho não ativo ou não cadastrado (favor verificar).";
                    //_tipoOperacao = TipoOperacao.Inicial;
                }


                //ControlarTipoOperacao();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void tseUnidadeEnsino_Changed(object sender, EventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                if (!tseUnidadeEnsino.DBValue.IsNull)
                {
                    if (tseUnidadeEnsino.IsValidDBValue)
                    {
                        //     this._tipoOperacao = TipoOperacao.Consultar;
                   //     ControlarTipoOperacao();
                        lblMensagem.Text = string.Empty;
                    }
                    else
                    {
                  //      this._tipoOperacao = TipoOperacao.Inicial;
                        lblMensagem.Text = "Unidade de Ensino não ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Unidade de Ensino não ativo ou não cadastrado (favor verificar).";
                 //   _tipoOperacao = TipoOperacao.Inicial;
                }


                //ControlarTipoOperacao();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void tseOperacao_Changed(object sender, EventArgs e)
        {
            try
            {
                if (!tseOperacao.IsValidDBValue)
                {

                    return;
                }


                //  var operacao = new RN.PrestacaoContas.Entidades.Operacao();

                var mensagens = new List<string>();

                if (tsePeriodoPrestacaoContas.Value == null || !tsePeriodoPrestacaoContas.IsValidDBValue)
                    mensagens.Add("PERÍODO DE PRESTAÇÃO DE CONTAS: Seleção obrigatória");

                if (tseUnidadeEnsino.Value == null || !tseUnidadeEnsino.IsValidDBValue)
                    mensagens.Add("UNIDADE DE ENSINO: Seleção obrigatória");

                if (tsePlanoTrabalho.Value == null || !tsePlanoTrabalho.IsValidDBValue)
                    mensagens.Add("PROJETO/PROGRAMA: Seleção obrigatória");

                if (mensagens.Any())
                {
                    lblMensagem.Text = mensagens.Distinct().Aggregate((x, y) => x + "<br />" + y);
                    return;
                }

                var operacao = rnOperacao.ObtemPor(Convert.ToInt32(tseOperacao.Value));
                //operacao = rnOperacao.ListaOperacaoPor(tsePeriodoPrestacaoContas.Value.ToString(), tsePlanoTrabalho.Value.ToString(), tseUnidadeEnsino.Value.ToString());
                //   hdnOcorrenciaId.Value = operacao.OperacaoId.ToString();
               // txtValor.Text = operacao.Valor.ToString();
              //  txtJustificativa.Text = operacao.Justificativa.ToString();

                //    pcCreditoDebito.Visible = true;
                //    pnlDocumento.Visible = true;

               // _tipoOperacao = TipoOperacao.Consultar;
               // ControlarTipoOperacao();


            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                ModoTela = ModoTelaEnum.TelaInicial;

                var mensagens = new List<string>();
                if (tsePeriodoPrestacaoContas.Value ==null)
                   mensagens.Add("Período da Prestação de Contas: Preenchimento obrigatório");
                else
                    if (tsePeriodoPrestacaoContas.Value.ToString() == "" )
                      mensagens.Add("Período da Prestação de Contas: Preenchimento obrigatório");

                if (mensagens.Any())
                {                    
                    lblMensagem.Text = mensagens.Aggregate((i, j) => i + "<br />" + j);
                    return;
                }

                CarregaGrid();


             //   odsResultado.SelectParameters["PeriodoReferencia"].DefaultValue = tsePeriodoPrestacaoContas.Value.ToString();
             //   odsCreditoeDebito.SelectParameters["ano"].DefaultValue = ddlAno.SelectedValue;
             //   odsCreditoeDebito.SelectParameters["censo"].DefaultValue = tseUnidadeEnsino.Value.ToString();

                pnlRegistro.Visible = true;
                grdRegistro.DataBind();
                grdRegistro.FocusedRowIndex = -1;

                ModoTela = ModoTelaEnum.TelaSomenteGrid;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private int GetSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = grdRegistro.PageIndex * grdRegistro.SettingsPager.PageSize;
            for (int i = 0; i < grdRegistro.VisibleRowCount; i++)
            {
                if (grdRegistro.Selection.IsRowSelected(startIndexOnPage + i))
                    return startIndexOnPage + i;
            }

            return -1;
        }

        protected void grdRegistro_PageIndexChanged(object sender, EventArgs e)
        {
            CarregaGrid();
        }
        private void CarregaGrid()
        {
            try
            {
            

                RN.PrestacaoContas.OperacaoDocumentos rnArquivoOcorrencia = new Techne.Lyceum.RN.PrestacaoContas.OperacaoDocumentos();


                grdRegistro.DataSource = rnArquivoOcorrencia.ListaExigenciasGridAnalisePor(
                    tsePeriodoPrestacaoContas.IsValidDBValue && !tsePeriodoPrestacaoContas.DBValue.IsNull ? Convert.ToInt32(tsePeriodoPrestacaoContas.DBValue) : -1,
                    rbFiltroOperacao.SelectedValue,
                     tseUnidadeEnsino.DBValue.ToString() ,
                     tsePlanoTrabalho.DBValue.ToString(),
                     tseOperacao.DBValue.ToString() );

                grdRegistro.DataBind();

                if (grdRegistro.VisibleRowCount > 0)
                {
                    grdRegistro.Visible = true;
                }
                else
                {
                    lblMensagem.Text = "Não existem ocorrências para o filtro selecionado.";
                    grdRegistro.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void grdRegistro_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            if (e.CallbackName.Equals("SELECTION"))
            {
                
                var operacaoId = Convert.ToInt32(grdRegistro.GetRowValues(GetSelectedRowOnTheCurrentPage(), "OPERACAOID")); ;

              //  var retstatus = rnOperacao.RetornaStatus(operacaoId);                
              
              ASPxWebControl.RedirectOnCallback("AnalisareVerificarCreditoeDebito.aspx?OperacaoID=" + operacaoId);
               
             
            }

        }

        private string MontarQueryString(Techne.Lyceum.RN.DTOs.DadosOcorrencia ocorrencia)
        {
            string queryString = string.Empty;

            if (ocorrencia != null)
            {
                queryString += "tela=" + "consulta";
                queryString += "&tipoOperacao=" + "CONSULTAR";
                queryString += "&codigo=" + ocorrencia.OcorrenciaId;
              //  queryString += "&ano=" + ddlAno.SelectedValue;
//                queryString += "&dtocor=" + dtDataOcorrencia.Text;
                //queryString += "&regional=" + tseRegional.DBValue.ToString();
                //queryString += "&municipio=" + tseMunicipio.DBValue.ToString();
                //queryString += "&escola=" + tseUnidadeResponsavel.DBValue.ToString();
                //queryString += "&classe=" + tseClasse.DBValue.ToString();
                //queryString += "&subcl=" + tseSubClasse.DBValue.ToString();
                //queryString += "&tratamento=" + tseTratamento.DBValue.ToString();
                //queryString += "&situacao=" + ddlSituacao.SelectedValue;
            }
            return queryString;
        }

        protected void grdDocumento_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdRegistro.Settings.ShowFilterRow = false;

        }


        public object UpdateResultadoBusca(object PeriodoReferencia)
        {
            RN.PrestacaoContas.OperacaoDocumentos rnArquivoOcorrencia = new Techne.Lyceum.RN.PrestacaoContas.OperacaoDocumentos();

            if (PeriodoReferencia != null)
            {
                if (!string.IsNullOrEmpty(PeriodoReferencia.ToString()))
                {
                    return rnArquivoOcorrencia.ListaExigenciasPor(Convert.ToInt32(PeriodoReferencia), "9", "99999", "99999", "9");
                }
            }
            return null;
        }

        protected void grdDocumento_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            /*    if (this.grdDocumento.Visible || this.grdDocumento.VisibleRowCount == 0)
                {
                    DropDownList cmbMotivo = grdDocumento.FindRowCellTemplateControl(e.VisibleIndex, grdDocumento.Columns["MOTIVO"] as GridViewDataColumn, "cmbMotivo") as DropDownList;

                    if (cmbMotivo != null)
                    {
                        HiddenField hfMotivo = grdDocumento.FindRowCellTemplateControl(e.VisibleIndex, grdDocumento.Columns["MOTIVO"] as GridViewDataColumn, "hfMotivo") as HiddenField;
                        TextBox txtMotivo = grdDocumento.FindRowCellTemplateControl(e.VisibleIndex, grdDocumento.Columns["MOTIVO"] as GridViewDataColumn, "txtMotivo") as TextBox;
                        txtMotivo.Visible = false;
                        int MotivoIndeferidoId = !string.IsNullOrEmpty(grdDocumento.GetRowValues(e.VisibleIndex, "MOTIVOINDEFERIDOID").ToString()) ? Convert.ToInt32(grdDocumento.GetRowValues(e.VisibleIndex, "MOTIVOINDEFERIDOID")) : 0;
                        string dataVerificacao = Convert.ToString(grdDocumento.GetRowValues(e.VisibleIndex, "DATAVERIFICACAO"));
                        string dataEntrega = Convert.ToString(grdDocumento.GetRowValues(e.VisibleIndex, "DATAENTREGA"));
                        RadioButtonList rblistSituacao = grdDocumento.FindRowCellTemplateControl(e.VisibleIndex, grdDocumento.Columns["Situacao"] as GridViewDataColumn, "rblistSituacao") as RadioButtonList;
                        ListItem radioButtonEmitido = null; ListItem radioButtonEntregue = null; ListItem radioButtonIndeferido = null; ListItem radioButtonSolicitado = null;
                        string motivoDesc = Convert.ToString(grdDocumento.GetRowValues(e.VisibleIndex, "MOTIVOINDEFERIDODESCRICAO"));


                        foreach (var item in rblistSituacao.Items)
                        {
                            var temp = item as ListItem;
                            if (temp.Text == "Emitido")
                            {
                                radioButtonEmitido = temp;
                            }
                            else if (temp.Text == "Entregue")
                            {
                                radioButtonEntregue = temp;
                            }
                            else if (temp.Text == "Indeferido")
                            {
                                radioButtonIndeferido = temp;
                            }
                            else if (temp.Text == "Solicitado")
                            {
                                radioButtonSolicitado = temp;
                            }
                        }

                        ASPxDateEdit dtVerificacao = grdDocumento.FindRowCellTemplateControl(e.VisibleIndex, grdDocumento.Columns["DATAVERIFICACAO"] as GridViewDataColumn, "dtVerificacao") as ASPxDateEdit;
                        ASPxDateEdit dtEntrega = grdDocumento.FindRowCellTemplateControl(e.VisibleIndex, grdDocumento.Columns["DTENTREGA"] as GridViewDataColumn, "dtEntrega") as ASPxDateEdit;

                        if (!dataVerificacao.IsNullOrEmptyOrWhiteSpace())
                        {
                            dtVerificacao.Date = Convert.ToDateTime(dataVerificacao);
                        }
                        if (!dataEntrega.IsNullOrEmptyOrWhiteSpace())
                        {
                            dtEntrega.Date = Convert.ToDateTime(dataEntrega);
                        }

                        string motivoSelecionado = Convert.ToString(MotivoIndeferidoId);

                        if (!string.IsNullOrEmpty(motivoDesc))
                        {
                            if (cmbMotivo.Items.FindByValue(motivoSelecionado) != null)
                            {
                                cmbMotivo.SelectedValue = motivoSelecionado;
                            }
                        }

                        if (radioButtonIndeferido.Selected)
                        {
                            rblistSituacao.Enabled = false;
                            dtVerificacao.Enabled = false;
                            dtEntrega.Enabled = false;
                            txtMotivo.Visible = true;
                            cmbMotivo.Visible = false;

                        }

                        if (radioButtonEmitido.Selected)
                        {
                            dtVerificacao.Enabled = false;
                            dtEntrega.Enabled = true;
                            rblistSituacao.Enabled = true;
                            rblistSituacao.Items[0].Enabled = false;
                            rblistSituacao.Items[1].Enabled = true; //Entregue
                            rblistSituacao.Items[2].Enabled = false;
                            rblistSituacao.Items[3].Enabled = false;
                            txtMotivo.Visible = true;
                            txtMotivo.Enabled = false;
                            cmbMotivo.Visible = false;
                        }

                        if (radioButtonEntregue.Selected)
                        {
                            dtVerificacao.Enabled = false;
                            dtEntrega.Enabled = false;
                            rblistSituacao.Enabled = false;
                            txtMotivo.Visible = true;
                            txtMotivo.Enabled = false;
                            cmbMotivo.Visible = false;
                        }
                        if (radioButtonSolicitado.Selected)
                        {
                            rblistSituacao.Enabled = true;
                        }

                        for (int i = 0; i <= rblistSituacao.Items.Count - 1; i++)
                        {
                            rblistSituacao.Items[i].Attributes.Add("cmbMotivo", cmbMotivo.ClientID);
                        }

                        var observacao = string.IsNullOrEmpty(hfMotivo.Value) ? "Selecione" : hfMotivo.Value;

                        //cmbMotivo.SelectedValue = observacao;



                    }


                }*/


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

                //lnkVisualizar.CommandArgument = String.Empty;
                //txtObservacao.Text = String.Empty;

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
               // lnkVisualizar.CommandArgument = String.Empty;
                //txtObservacao.Text = String.Empty;
                grdExigencias.DataBind();

                ModoTela = ModoTelaEnum.TelaExtratoNaoPreenchido;

                return;
            }

            var row = extratoBancario.Rows[0];

            var status = row["STATUS"] != DBNull.Value ? (int?)Convert.ToInt32(row["STATUS"]) : null;
            hidStatus.Value = status.HasValue ? status.Value.ToString() : String.Empty;
            lblStatus.Text = ObterDescricaoStatus(hidStatus.Value);

            hidExtratoBancarioId.Value = extratoBancarioId.ToString();
            //lnkVisualizar.CommandArgument = Convert.ToString(extratoBancarioId) + "," + Convert.ToString(row["TIPOARQUIVO"]);
            //txtObservacao.Text = Convert.ToString(row["OBSERVACAO"]);
            grdExigencias.DataBind();

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
                //bimgArquivo.Visible = false;
                //ltEmbed.Visible = false;
                DataTable dados = new DataTable();

                string[] chave = e.CommandArgument.ToString().Split(new char[] { ',' });

                if (!chave[1].ToString().IsNullOrEmptyOrWhiteSpace())
                {
                 //   pucVisualizarArquivo.ShowOnPageLoad = true;

                    if (chave[1].ToString() == "application/pdf")
                    {
                        embed = " <object data=\"{0}{1}\"";
                        embed += "type=\"application/pdf\" width=\"100%\" height=\"100%\">";
                        embed += "<iframe   src=\"{0}{1}\"  width=\"100%\"   height=\"100%\"";
                        embed += "style=\"border: none;\">    <p>Your browser does not support PDFs.";
                        embed += "<a href=\"{0}{1}\">Download the PDF</a>.</p>";
                        embed += "</iframe></object>";
                       // ltEmbed.Text = string.Format(embed, ResolveUrl("~/PrestacaoContas/FileCS.ashx?Tabela=" + tabela + "&Id="), chave[0].ToString());
                       // ltEmbed.Visible = true;
                        //pucVisualizarArquivo.Width = Unit.Pixel(880);
                       // pucVisualizarArquivo.Height = Unit.Pixel(580);
                    }
                    else
                    {
                       // pucVisualizarArquivo.Width = Unit.Pixel(350);
                      //  pucVisualizarArquivo.Height = Unit.Pixel(350);
                      //  bimgArquivo.ContentBytes = rnArquivo.ObtemArquivoPor(Convert.ToInt32(chave[0]));
                      //  bimgArquivo.Visible = true;
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
        protected void ddlOperacao_SelectedIndexChanged(object sender, EventArgs e)
        {

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

              //  var verificaenviosei = rnExtratoBancario.VerificaEnvioSEI(extratoBancarioId);
               // if (verificaenviosei)
               //     throw new Exception("Não é possível aprovar este extrato porque o Formulário SEI já foi gerado");               

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

                //grdExtratoBancario.FocusedRowChanged -= grdExtratoBancario_FocusedRowChanged;
                //grdExtratoBancario.DataBind();
                //grdExtratoBancario.FocusedRowChanged += grdExtratoBancario_FocusedRowChanged;
                //grdExtratoBancario_FocusedRowChanged(grdExtratoBancario, e);

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
               
              //  var verificaenviosei = rnExtratoBancario.VerificaEnvioSEI(extratoBancarioId);
              //  if (verificaenviosei)
              //      throw new Exception("Não é possível reprovar este extrato porque o Formulário SEI já foi gerado");

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

                //grdExtratoBancario.FocusedRowChanged -= grdExtratoBancario_FocusedRowChanged;
                //grdExtratoBancario.DataBind();
                //grdExtratoBancario.FocusedRowChanged += grdExtratoBancario_FocusedRowChanged;
                //grdExtratoBancario_FocusedRowChanged(grdExtratoBancario, e);

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
             
             //   var verificaenviosei = rnExtratoBancario.VerificaEnvioSEI(extratoBancarioId);
              //  if (verificaenviosei)
              //      throw new Exception("Não é possível aprovar este extrato porque o Formulário SEI já foi gerado");

                var status = (int?)null;
                try { status = Convert.ToInt32(hidStatus.Value); }
                catch { status = null; }

                switch (status)
                {
                    case 1:
                    case 3:
                        if (grdExigencias.VisibleRowCount == 0)
                            throw new Exception("Devolver o extrato bancário requer pelo menos 1 exigência cadastrada");

                        rnExtratoBancario.AtualizaStatus(extratoBancarioId, 2);
                        lblMensagem.Text = "Extrato devolvido para AAE com sucesso";
                        break;

                    default:
                        lblMensagem.Text = "Status desconhecido. Nenhuma ação foi realizada.";
                        break;
                }

                //grdExtratoBancario.FocusedRowChanged -= grdExtratoBancario_FocusedRowChanged;
                //grdExtratoBancario.DataBind();
                //grdExtratoBancario.FocusedRowChanged += grdExtratoBancario_FocusedRowChanged;
                //grdExtratoBancario_FocusedRowChanged(grdExtratoBancario, e);

                ModoTela = ModoTelaEnum.TelaExtratoDevolvidoParaAAE;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void grdDocumento_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {

        /*    string censo = grdAplicacoesFinanceiras.GetRowValues(e.VisibleIndex, "CENSO").ToString();
            string valor = grdAplicacoesFinanceiras.GetRowValues(e.VisibleIndex, "VALOR").ToString();
            string justificativa = grdAplicacoesFinanceiras.GetRowValues(e.VisibleIndex, "JUSTIFICATIVA").ToString();
            string extratoBancarioId = grdAplicacoesFinanceiras.GetRowValues(e.VisibleIndex, "EXTRATOBANCARIOID").ToString();
            string aplicacaoFinanceiraId = grdAplicacoesFinanceiras.GetRowValues(e.VisibleIndex, "APLICACAOFINANCEIRAID").ToString();
            string aplicacaoFinanceiraComprovanteArquivoId = grdAplicacoesFinanceiras.GetRowValues(e.VisibleIndex, "APLICACAOFINANCEIRACOMPROVANTEARQUIVOID") != null ? grdAplicacoesFinanceiras.GetRowValues(e.VisibleIndex, "APLICACAOFINANCEIRACOMPROVANTEARQUIVOID").ToString() : null;

            LimpaControles();
            btnCancel.Visible = true;
            pnlDados.Visible = true;

            tseUnidadeResponsavelDados.DBValue = censo;
            tseExtratoBancarioDados.DBValue = Convert.ToInt32(extratoBancarioId);
            txtValorAplicacao.Text = string.Format("{0:N2}", valor);
            txtJustificativa.Text = justificativa;

            if (e.ButtonID == "btnEditarCustom")
            {
                hdnAplicacaoFinanceiraId.Value = aplicacaoFinanceiraId;
                hdnAplicacaoFinanceiraComprovanteArquivoId.Value = aplicacaoFinanceiraComprovanteArquivoId;

            }*/

            if (e.ButtonID == "btnVizualizar")
            {

            }

        }

        protected void grdDocumento_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;

            if (e.CellType == GridViewTableCommandCellType.Filter)
            {
                e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                return;
            }

            if (e.ButtonID == "btnEditarCustom")
            {
                if (Permission.AllowUpdate)
                {
                    e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.True;
                }
            }
        }
        protected void grdExigencias_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            var justificada = !Convert.ToString(grdExigencias.GetRowValues(e.VisibleIndex, "JUSTIFICATIVA")).IsNullOrEmptyOrWhiteSpace();
            var temArquivo = (grdExigencias.GetRowValues(e.VisibleIndex, "ARQUIVO") as byte[] ?? new byte[] { }).Any();
            var aprovado = Convert.ToBoolean(grdExigencias.GetRowValues(e.VisibleIndex, "APROVADO") != DBNull.Value ? grdExigencias.GetRowValues(e.VisibleIndex, "APROVADO") : 0);

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

        protected void grdExigencias_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            try
            {
                var exigenciaExtratoId = Convert.ToInt32(grdExigencias.GetRowValues(e.VisibleIndex, "EXIGENCIAEXTRATOID"));
                var extratoBancarioId = Convert.ToInt32(grdExigencias.GetRowValues(e.VisibleIndex, "EXTRATOBANCARIOID"));

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

        protected void grdExigencias_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            try
            {
                var exigenciaExtratoId = Convert.ToInt32(grdExigencias.GetRowValues(e.VisibleIndex, "EXIGENCIAEXTRATOID"));

                switch (e.ButtonID)
                {
                    case "btnAprovarExigencia":
                        rnExigenciaExtrato.AtualizaAprovacao(exigenciaExtratoId, true);
                        lblMensagem.Text = "Aprovado com sucesso";
                        grdExigencias.DataBind();
                        break;

                    case "btnReprovarExigencia":
                        rnExigenciaExtrato.AtualizaAprovacao(exigenciaExtratoId, false);
                        lblMensagem.Text = "Reprovado com sucesso";
                        grdExigencias.DataBind();
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
        /*    protected void grdDocumento_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
            {
                try
                {
                    var exigenciaExtratoId = Convert.ToInt32(grdExigencias.GetRowValues(e.VisibleIndex, "EXIGENCIAEXTRATOID"));

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
            }*/
        protected void grdExigencias_RowInserting(object sender, ASPxDataInsertingEventArgs e)
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

                grdExigencias.DataBind();
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

                ee.ExigenciaExtratoId = Convert.ToInt32(grdExigencias.GetRowValues(grdExigencias.EditingRowVisibleIndex, "EXIGENCIAEXTRATOID"));
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

                grdExigencias.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void grdDocumento_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();

                var ee = new Techne.Lyceum.RN.PrestacaoContas.Entidades.ExigenciaExtrato();

                ee.ExigenciaExtratoId = Convert.ToInt32(grdExigencias.GetRowValues(grdExigencias.EditingRowVisibleIndex, "EXIGENCIAEXTRATOID"));
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

                grdExigencias.DataBind();
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
                    grdExigencias.DataBind();
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
                        //pnlDocumento.Visible = false;
                        //plaGrid.Visible = false;
                        plaCreditoeDebito.Visible = false;
                        grdRegistro.Visible = false;
                        break;

                    case ModoTelaEnum.TelaSomenteGrid:

                       // plaGrid.Visible = true;
                        plaCreditoeDebito.Visible = false;
                        grdRegistro.Visible = true;
                        break;

                    case ModoTelaEnum.TelaExtratoNaoPreenchido:

                      //  plaGrid.Visible = true;
                        plaCreditoeDebito.Visible = true;

                        btnDevolver.Visible = false;
                        btnReprovar.Visible = false;
                        btnAprovar.Visible = false;

                      //  plaVazio.Visible = true;
                       // plaExistente.Visible = false;
                        plaStatus.Visible = false;
                       // txtObservacao.Enabled = false;
                       // filExtratoBancario.Visible = false;
                       // lnkVisualizar.Visible = false;
                        pcExtratoBancario.TabPages.FindByName("tabExigenciasExtrato").Enabled = false;

                        break;

                    case ModoTelaEnum.TelaExtratoEnviadoParaAnalise:
                    case ModoTelaEnum.TelaExtratoRevisadoPelaAAE:

                       // plaGrid.Visible = true;
                        //plaCreditoeDebito.Visible = true;

                        btnDevolver.Visible = true;
                        btnReprovar.Visible = true;
                        btnAprovar.Visible = true;

                        //plaVazio.Visible = false;
                       // plaExistente.Visible = true;
                        plaStatus.Visible = true;
                       // txtObservacao.Enabled = false;
                       // filExtratoBancario.Visible = false;
                       // lnkVisualizar.Visible = true;
                        pcExtratoBancario.TabPages.FindByName("tabExigenciasExtrato").Enabled = true;

                        break;

                    case ModoTelaEnum.TelaExtratoLancamentoPelaAAE:
                    case ModoTelaEnum.TelaExtratoDevolvidoParaAAE:
                    case ModoTelaEnum.TelaExtratoAprovado:
                    case ModoTelaEnum.TelaExtratoReprovado:

                       // plaGrid.Visible = true;
                        plaCreditoeDebito.Visible = true;

                        btnDevolver.Visible = false;
                        btnReprovar.Visible = false;
                        btnAprovar.Visible = false;

                        //plaVazio.Visible = false;
                       // plaExistente.Visible = true;
                        plaStatus.Visible = true;
                       // txtObservacao.Enabled = false;
                       // filExtratoBancario.Visible = false;
                        //lnkVisualizar.Visible = true;
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
