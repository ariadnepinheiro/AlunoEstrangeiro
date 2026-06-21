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
    [NavUrl("~/PrestacaoContas/AnalisareVerificarCreditoeDebitoCOOREF.aspx"), ControlText("Analisar Crédito e Débito Geral"), Title("Analisar Crédito e Débito Geral")]
    public partial class AnalisareVerificarCreditoeDebitoCOOREF : TPage
    {
        private RN.PeriodoLetivo rnPeriodoLetivo = new RN.PeriodoLetivo();
        RN.PrestacaoContas.PlanoTrabalho rnPlanoTrabalho = new RN.PrestacaoContas.PlanoTrabalho();
        RN.PrestacaoContas.Operacao rnOperacao = new RN.PrestacaoContas.Operacao();
        RN.PrestacaoContas.OperacaoExigencia rnOperacaoExigencia = new RN.PrestacaoContas.OperacaoExigencia();
        RN.PrestacaoContas.OperacaoExigenciaArquivo rnOperacaoExigenciaArquivo = new RN.PrestacaoContas.OperacaoExigenciaArquivo();        
        RN.PrestacaoContas.OperacaoDocumentos rnOperacaoDocumentos = new RN.PrestacaoContas.OperacaoDocumentos();

        public void Insert(object TIPOEXIGENCIAOPERACAOID, object JUSTIFICATIVA, object NOTAEXPLICATIVA, object APROVADO) { }
        public void Insert(object TIPOEXIGENCIAOPERACAOID, object NOTAEXPLICATIVA, object APROVADO) { }
        public void Insert(object TIPOEXIGENCIAOPERACAOID, object NOTAEXPLICATIVA) { }
        public void Insert() { }
        public void Update(object OPERACAOID, object TIPOEXIGENCIAOPERACAOID, object NOTAEXPLICATIVA, object JUSTIFICATIVA, object APROVADO, object OPERACAOEXIGENCIAID) { }
        public void Update(object TIPOEXIGENCIAOPERACAOID, object NOTAEXPLICATIVA, object OPERACAOEXIGENCIAID) { }
        public void Delete(object OPERACAOEXIGENCIAID) { }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdExigencias);
            ControlaAcesso(btnAprovar, AcaoControle.editar);
            ControlaAcesso(btnReprovar, AcaoControle.editar);
            ControlaAcesso(btnDevolver, AcaoControle.editar);
            ControlaAcesso(grdExigencias, AcaoControle.editar, "btnAprovarExigencia");
            ControlaAcesso(grdExigencias, AcaoControle.editar, "btnRejeitarExigencia");
            pucConfirmar.ShowOnPageLoad = false;
            AcessoGrid();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
       
            TituloGrid(grdExigencias, string.Empty);
            TituloGrid(grdDocumento, string.Empty);
            
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                RN.PrestacaoContas.PlanoTrabalho rnPlanoTrabalho = new Techne.Lyceum.RN.PrestacaoContas.PlanoTrabalho();

                var operacaoId = Request.QueryString["OperacaoID"];
               // Carrega Dados
                var operacao = new RN.PrestacaoContas.Entidades.Operacao();
                operacao = rnOperacao.ObtemPor(Convert.ToInt32(operacaoId));
                hdnOcorrenciaId.Value = operacaoId;

               
                RN.Entidades.LyUnidadeEnsino rowUE = RN.UnidadeEnsino.Carregar(operacao.Censo.ToString());
                lblUnidadeEnsino.Text = rowUE.NomeComp;
                lblProjeto.Text = rnPlanoTrabalho.PesquisaPorId(operacao.PlanoTrabalhoId.ToString());
                lblOperacao.Text = operacao.TipoOperacao.ToString() == "C" ? "Crédito" : "Débito";
                lblDataEnvio.Text = operacao.DataCadastro.ToString();
                txtValor.Text = operacao.Valor.ToString();
                txtJustificativa.Text = operacao.Justificativa.ToString();
                lblStatus.Text = ObterDescricaoStatus(operacao.Status.ToString());
                hidStatus.Value = operacao.Status.ToString();
                if(operacao.Status.ToString() == "3")
                    lblcodigooperacao.Text = operacao.CodOperacao.ToString();
                else
                    lblcodigooperacao.Text = operacao.OperacaoId.ToString();

                lblCNPJ.Text = rowUE.Cgc;                

              switch (operacao.Status.ToString())
              {
                  case "1":
                      btnAprovar.Visible = true;
                      btnReprovar.Visible = true;
                      btnDevolver.Visible = true;
                      txtValor.Enabled = true;
                      txtJustificativa.Enabled = true;
                      break;
                  case "2":

                      btnAprovar.Visible = true;
                      btnReprovar.Visible = true;
                      btnDevolver.Visible = false;
                      txtValor.Enabled = true;
                      txtJustificativa.Enabled = true;
                      break; 
                case "3":
                    btnAprovar.Visible = false;
                    btnReprovar.Visible = false;
                    btnDevolver.Visible = false;
                    txtValor.Enabled = false;
                    txtJustificativa.Enabled = false;
                    break;
                case "4":

                    btnAprovar.Visible = false;
                    btnReprovar.Visible = false;
                    btnDevolver.Visible = false;
                    txtValor.Enabled = false;
                    txtJustificativa.Enabled = false;
                    break;
                 }


            }
            catch {
                return;
            }

        }

        protected void grdExigencias_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            try
            {
                var justificada = !Convert.ToString(grdExigencias.GetRowValues(e.VisibleIndex, "JUSTIFICATIVA")).IsNullOrEmptyOrWhiteSpace();
                var temArquivo = !Convert.ToString(grdExigencias.GetRowValues(e.VisibleIndex, "TIPOARQUIVO")).IsNullOrEmptyOrWhiteSpace();

                int aprovado = 3;

                if (grdExigencias.GetRowValues(e.VisibleIndex, "APROVADO").Equals("Não analisado"))
                    aprovado = 2;
                if (grdExigencias.GetRowValues(e.VisibleIndex, "APROVADO").Equals("Aprovado"))
                    aprovado = 1;
                if (grdExigencias.GetRowValues(e.VisibleIndex, "APROVADO").Equals("Reprovado"))
                    aprovado = 0;

                //  var eventoFinalizado = new string[] { "Aprovado", "Reprovado" }.Contains(lblStatus.Text);

                if (e.ButtonID == "btnAprovarExigencia")
                {
                    e.Visible = ((aprovado == 2) && (justificada || temArquivo)) ? DevExpress.Web.ASPxClasses.DefaultBoolean.True : DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                    //   if (eventoFinalizado)
                    //      e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;

                    if (!Permission.AllowUpdate)
                        e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;

                }

                if (e.ButtonID == "btnRejeitarExigencia")
                {
                    e.Visible = ((aprovado == 2) && (justificada && temArquivo)) ? DevExpress.Web.ASPxClasses.DefaultBoolean.True : DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                    //   if (eventoFinalizado)
                    //       e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;

                    if (!Permission.AllowUpdate)
                        e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                }
            }
            catch (Exception ex) { 
            }
        }

        protected void grdExigencias_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            try
            {
                var exigenciaEventoId = Convert.ToInt32(grdExigencias.GetRowValues(e.VisibleIndex, "OPERACAOEXIGENCIAID"));
                var usuarioId = User.Identity.Name;

                if (e.ButtonID == "btnAprovarExigencia")
                {
                    var validacao = rnOperacaoExigencia.ValidaAprovacao(exigenciaEventoId, usuarioId);

                    if (validacao.Valido)
                        rnOperacaoExigencia.Aprova(exigenciaEventoId, usuarioId);
                    else
                        throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));

                    grdExigencias.DataBind();
                }

                if (e.ButtonID == "btnRejeitarExigencia")
                {
                    var validacao = rnOperacaoExigencia.ValidaRejeicao(exigenciaEventoId, usuarioId);

                    if (validacao.Valido)
                        rnOperacaoExigencia.Rejeita(exigenciaEventoId, usuarioId);
                    else
                        throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));

                    grdExigencias.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void AcessoGrid()
        {
            try
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
                                img.Visible = false;
                            }

                        }
                        else
                        {
                            img.Visible = false;
                        }


                    }
                }
            }
            catch {
                return;
            }
        }
        public object ListaDocumento(object id)
        {
            try
            {
                RN.PrestacaoContas.OperacaoDocumentos rnArquivoOcorrencia = new Techne.Lyceum.RN.PrestacaoContas.OperacaoDocumentos();

                if (id != null)
                {
                    if (!string.IsNullOrEmpty(id.ToString()))
                    {
                        return rnArquivoOcorrencia.ListaPor(Convert.ToInt32(id));
                    }
                }
            }
            catch
            {
                return null;
            }
            return null;
        }
        protected void grdExigencias_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            try
            {
                    if (this.grdExigencias.IsNewRowEditing)
                    {
                        if (e.Column.FieldName == "OPERACAOID")
                        {
                            e.Editor.ReadOnly = true;
                        }
                        if (e.Column.FieldName == "TIPOEXIGENCIAOPERACAOID")
                        {
                            e.Editor.ReadOnly = false;
                        }
                        if (e.Column.FieldName == "JUSTIFICATIVA")
                        {
                            e.Editor.ReadOnly = true;
                            e.Editor.Enabled = false;
                        }
                        if (e.Column.FieldName == "NOTAEXPLICATIVA")
                        {
                            e.Editor.ReadOnly = false;
                        }
                        if (e.Column.FieldName == "APROVADO")
                        {
                            e.Editor.ReadOnly = true;
                            e.Editor.Enabled = false;
                        }
                    }
                    else if (this.grdExigencias.IsEditing)
                    {
                        if (e.Column.FieldName == "OPERACAOID")
                        {
                            e.Editor.Enabled = true;
                        }
                        if (e.Column.FieldName == "TIPOEXIGENCIAOPERACAOID")
                        {
                            e.Editor.ReadOnly = false;
                        }
                        if (e.Column.FieldName == "JUSTIFICATIVA")
                        {
                            e.Editor.ReadOnly = true;
                            e.Editor.Enabled = false;
                        }
                        if (e.Column.FieldName == "NOTAEXPLICATIVA")
                        {
                            e.Editor.ReadOnly = false;
                        }
                        if (e.Column.FieldName == "APROVADO")
                        {
                            e.Editor.ReadOnly = true;
                            e.Editor.Enabled = false;
                        }
                    }
            }
            catch
            {
                return;
            }
        }
        protected void grdExigencias_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["APROVADO"] = null;
            grdExigencias.Settings.ShowFilterRow = true;
        }
        protected void grdExigencias_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdExigencias.Settings.ShowFilterRow = false;
        }
        public object ListaExigencias(object id)
        {
            try
            {
                RN.PrestacaoContas.OperacaoDocumentos rnArquivoOcorrencia = new Techne.Lyceum.RN.PrestacaoContas.OperacaoDocumentos();
 
                return rnArquivoOcorrencia.ListaExigenciasPorId(Convert.ToInt32(id));
            }
            catch {
                return null;
            }
        }

        public object ListaTipoReprovacao()
        {
            try
            {
                RN.PrestacaoContas.MotivoRepOperacao rnMotivoRepOperacao = new Techne.Lyceum.RN.PrestacaoContas.MotivoRepOperacao();

                return rnMotivoRepOperacao.ListaAtivo();
            }
            catch
            {
                return null;
            }
        }
        public object ListaTipoExigencias()
        {
            try
            {
                RN.PrestacaoContas.TipoExigenciaOperacao rnArquivoOcorrencia = new Techne.Lyceum.RN.PrestacaoContas.TipoExigenciaOperacao();
        
                return rnArquivoOcorrencia.ListaAtivo();
            }
            catch
            {
                return null;
            }
        }
        protected void grdExigencias_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
          try{
              ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.OperacaoExigencia operacaoExigencia = new Techne.Lyceum.RN.PrestacaoContas.Entidades.OperacaoExigencia();

            RN.PrestacaoContas.Entidades.TipoExigenciaOperacao tipoExigenciaOperacao = new Techne.Lyceum.RN.PrestacaoContas.Entidades.TipoExigenciaOperacao();

            RN.PrestacaoContas.OperacaoExigencia rnOperacaoExigencia = new Techne.Lyceum.RN.PrestacaoContas.OperacaoExigencia();
            operacaoExigencia.TipoExigenciaOperacaoId = Convert.ToInt32(e.NewValues["TIPOEXIGENCIAOPERACAOID"]) ;
            operacaoExigencia.Justificativa = e.NewValues["JUSTIFICATIVA"] != null ? e.NewValues["JUSTIFICATIVA"].ToString().Trim().ToUpper() : null;
            operacaoExigencia.NotaExplicativa = e.NewValues["NOTAEXPLICATIVA"] != null ? e.NewValues["NOTAEXPLICATIVA"].ToString().Trim().ToUpper() : null;
            operacaoExigencia.OperacaoId = Convert.ToInt32(hdnOcorrenciaId.Value);
            //operacaoExigencia.Aprovado = ;
            operacaoExigencia.UsuarioId = User.Identity.Name;

            validacao = rnOperacaoExigencia.Valida(operacaoExigencia,1);

            if (validacao.Valido)
            {
                rnOperacaoExigencia.Insere(operacaoExigencia);
            }
           
            odsCreditoeDebito.Select();
            odsCreditoeDebito.DataBind();
            grdExigencias.DataBind();
          }
          catch
          {
              return;
          }
        }
         
        protected void grdExigencias_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();

                RN.PrestacaoContas.OperacaoExigencia rnOperacaoExigencia = new Techne.Lyceum.RN.PrestacaoContas.OperacaoExigencia();
                RN.PrestacaoContas.Entidades.OperacaoExigencia operacaoExigencia = new Techne.Lyceum.RN.PrestacaoContas.Entidades.OperacaoExigencia();

                operacaoExigencia.OperacaoExigenciaId = Convert.ToInt32(e.Keys["OPERACAOEXIGENCIAID"]);
                operacaoExigencia.TipoExigenciaOperacaoId = Convert.ToInt32(e.NewValues["TIPOEXIGENCIAOPERACAOID"]);
               // operacaoExigencia.Justificativa = e.NewValues["JUSTIFICATIVA"] != null ? e.NewValues["JUSTIFICATIVA"].ToString().Trim().ToUpper() : null;
                operacaoExigencia.NotaExplicativa = e.NewValues["NOTAEXPLICATIVA"] != null ? e.NewValues["NOTAEXPLICATIVA"].ToString().Trim().ToUpper() : null;
                operacaoExigencia.OperacaoId = Convert.ToInt32(hdnOcorrenciaId.Value);

                operacaoExigencia.UsuarioId = User.Identity.Name;

                validacao = rnOperacaoExigencia.Valida(operacaoExigencia,1);

                if (validacao.Valido)
                {
                    rnOperacaoExigencia.AtualizaExigencia(operacaoExigencia);
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }

                grdExigencias.DataBind();
            }
            catch
            {
                return;
            }
        }

        protected void grdExigencias_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
          try{  
              ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.MotivoExigenciaCredDeb rnMotivoExigenciaCredDeb = new Techne.Lyceum.RN.PrestacaoContas.MotivoExigenciaCredDeb();
            int tipoexigenciaoperacaoid = 0;

            tipoexigenciaoperacaoid = Convert.ToInt32(e.Keys["OPERACAOEXIGENCIAID"]);

            validacao = rnMotivoExigenciaCredDeb.ValidaRemocao(tipoexigenciaoperacaoid);

            if (validacao.Valido)
            {
                rnMotivoExigenciaCredDeb.RemoveExigencia(tipoexigenciaoperacaoid);
                grdExigencias.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
          }
          catch
          {
              return;
          }
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
        protected void repCarrossel_ItemDataBound(Object Sender, RepeaterItemEventArgs e)
        {
            try{
                if (new ListItemType[] { ListItemType.Item, ListItemType.AlternatingItem }.Contains(e.Item.ItemType))
            {
                var tipoArquivo = ((System.Data.DataRowView)e.Item.DataItem)["TipoArquivo"].ToString();
                ((PlaceHolder)e.Item.FindControl("plaTipoPDF")).Visible = (tipoArquivo == "application/pdf");
                ((PlaceHolder)e.Item.FindControl("plaTipoImagem")).Visible = (tipoArquivo.StartsWith("image/"));
                ((PlaceHolder)e.Item.FindControl("plaSemArquivo")).Visible = (tipoArquivo != "application/pdf" && !tipoArquivo.StartsWith("image/"));
            }
            }
            catch
            {
                return;
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
                IOperacaoExigenciaArquivo rnArquivo = null;
                IOperacaoDocumentos rnArquivoDocumento = null;

                switch (((WebControl)sender).ID)
                {
                    case "lnkVisualizar":
                        tabela = "OperacaoDocumentos";
                        rnArquivoDocumento = rnOperacaoDocumentos;
                        break;

                    case "btnVisualizar":
                        tabela = "OperacaoExigenciaArquivo";
                        rnArquivo = rnOperacaoExigenciaArquivo;
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
        protected void ddlOperacao_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                  Response.Redirect("AnalisarCreditoeDebito.aspx");
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
                var ano = DateTime.Now.Year;
                var codoperacao = "";
                  var operacaoId = 0;
                  int.TryParse(hdnOcorrenciaId.Value ?? "", out operacaoId);
                  if (operacaoId == 0)
                      throw new Exception("STATUS: É necessário selecionar uma OPERAÇÃO antes");

                  var possuiExigenciasNaoAnalisadas = rnOperacao.PossuiExigenciasNaoAnalisadas(operacaoId);
                  if (possuiExigenciasNaoAnalisadas)
                      throw new Exception("Não é possível aprovar esta operação porque há exigências não analisadas");

                  var possuiExigenciasReprovadas = rnOperacao.PossuiExigenciasReprovadas(operacaoId);
                  if (possuiExigenciasReprovadas)
                      throw new Exception("Não é possível aprovar esta operação porque existe exigências reprovadas");

              //    var verificaenviosei = rnOperacao.VerificaEnvioSEI(operacaoId);
              //    if (verificaenviosei)
              //        throw new Exception("Não é possível aprovar esta operação porque o Formulário SEI já foi gerado"); 

                  var status = (int?)null;
                  try { status = Convert.ToInt32(hidStatus.Value); }
                  catch { status = null; }

                if (status == 0)
                    throw new Exception("A Operação não pode ser analisada");

                  switch (status)
                  {
                     // case 0:
                         // rnOperacao.AtualizaStatus(operacaoId, 3);
                      //    lblMensagem.Text = "A Operação não pode ser aprovada pois ainda não foi encaminhada";
                        //  btnAprovar.Visible = false;
                        //  btnReprovar.Visible = false;
                        //  btnDevolver.Visible = false;
                        //  txtValor.Enabled = false;
                        //  txtJustificativa.Enabled = false;
                        //  lblStatus.Text = ObterDescricaoStatus("3");
                       //  hidStatus.Value = "3";
                      //    break;
                      case 1:
                          rnOperacao.AtualizaStatus(operacaoId, 3);
                          lblMensagem.Text = "Operação aprovada com sucesso";
                          btnAprovar.Visible = false;
                          btnReprovar.Visible = false;
                          btnDevolver.Visible = false;
                          txtValor.Enabled = false;
                          txtJustificativa.Enabled = false;
                          lblStatus.Text = ObterDescricaoStatus("3");
                          hidStatus.Value = "3";
                          
                          codoperacao = ano.ToString() + rnOperacao.RetornaTipoOperacao(operacaoId) + operacaoId.ToString().PadLeft(6, '0');
                          lblcodigooperacao.Text = codoperacao;
                          break;
                      case 2:
                          rnOperacao.AtualizaStatus(operacaoId, 3);
                          lblMensagem.Text = "Operação aprovada com sucesso";
                          btnAprovar.Visible = false;
                          btnReprovar.Visible = false;
                          btnDevolver.Visible = false;
                          txtValor.Enabled = false;
                          txtJustificativa.Enabled = false;
                          lblStatus.Text = ObterDescricaoStatus("3");
                          hidStatus.Value = "3";
                         
                          codoperacao = ano.ToString() + rnOperacao.RetornaTipoOperacao(operacaoId) + operacaoId.ToString().PadLeft(6, '0');
                          lblcodigooperacao.Text = codoperacao;

                          break;
                      case 3:
                          rnOperacao.AtualizaStatus(operacaoId, 3);
                          lblMensagem.Text = "Operação aprovada com sucesso";
                          btnAprovar.Visible = false;
                          btnReprovar.Visible = false;
                          btnDevolver.Visible = false;
                          txtValor.Enabled = false;
                          txtJustificativa.Enabled = false;
                          lblStatus.Text = ObterDescricaoStatus("3");
                          hidStatus.Value = "3";
                          codoperacao = ano.ToString() + rnOperacao.RetornaTipoOperacao(operacaoId) + operacaoId.ToString().PadLeft(6, '0');
                          lblcodigooperacao.Text = codoperacao;


                          break;

                      default:
                          lblMensagem.Text = "Status desconhecido. Nenhuma ação foi realizada.";
                          break;
                  }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btConfirmaReprovacao_Click(object sender, EventArgs e)
        {
            try
            {
                var extratoBancarioId = 0;
                int.TryParse(hdnOcorrenciaId.Value ?? "", out extratoBancarioId);
                if (extratoBancarioId == 0)
                    throw new Exception("STATUS: É necessário selecionar uma OPERAÇÃO antes");

                rnOperacao.AtualizaStatusReprovado(extratoBancarioId, 4, Convert.ToString(cmbMotivo.SelectedValue));

                lblMensagem.Text = "Operação reprovada com sucesso";
                btnAprovar.Visible = false;
                btnReprovar.Visible = false;
                btnDevolver.Visible = false;
                txtValor.Enabled = false;
                txtJustificativa.Enabled = false;
                hidStatus.Value = "4";
                lblStatus.Text = ObterDescricaoStatus(hidStatus.Value);
         
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
                int.TryParse(hdnOcorrenciaId.Value ?? "", out extratoBancarioId);
                if (extratoBancarioId == 0)
                    throw new Exception("STATUS: É necessário selecionar uma OPERAÇÃO antes");

                var status = (int?)null;
                try { status = Convert.ToInt32(hidStatus.Value); }
                catch { status = null; }

                if (status == 0)
                    throw new Exception("A Operação não pode ser analisada");

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopup();", true);
                return;
             /*   switch (status)
                {
                    case 0:
                        rnOperacao.AtualizaStatus(extratoBancarioId, 4);
                        lblMensagem.Text = "Operação reprovada com sucesso";
                        btnAprovar.Visible = false;
                        btnReprovar.Visible = false;
                        btnDevolver.Visible = false;
                        txtValor.Enabled = false;
                        txtJustificativa.Enabled = false;
                        hidStatus.Value = "4";
                        lblStatus.Text = ObterDescricaoStatus(hidStatus.Value);
                        break;

                    case 1:
                        rnOperacao.AtualizaStatus(extratoBancarioId, 4);
                        lblMensagem.Text = "Operação reprovada com sucesso";
                        btnAprovar.Visible = false;
                        btnReprovar.Visible = false;
                        btnDevolver.Visible = false;
                        txtValor.Enabled = false;
                        txtJustificativa.Enabled = false;
                        hidStatus.Value = "4";
                        lblStatus.Text = ObterDescricaoStatus(hidStatus.Value);
                        break;
                    case 2:
                        rnOperacao.AtualizaStatus(extratoBancarioId, 4);
                        lblMensagem.Text = "Operação reprovada com sucesso";
                        btnAprovar.Visible = false;
                        btnReprovar.Visible = false;
                        btnDevolver.Visible = false;
                        txtValor.Enabled = false;
                        txtJustificativa.Enabled = false;
                        hidStatus.Value = "4";
                        lblStatus.Text = ObterDescricaoStatus(hidStatus.Value);
                        break;
                    case 3:
                        rnOperacao .AtualizaStatus(extratoBancarioId, 4);
                        lblMensagem.Text = "Operação reprovado com sucesso";
                        btnAprovar.Visible = false;
                        btnReprovar.Visible = false;
                        btnDevolver.Visible = false;
                        txtValor.Enabled = false;
                        txtJustificativa.Enabled = false;
                        hidStatus.Value = "4";
                        lblStatus.Text = ObterDescricaoStatus(hidStatus.Value);
                        break;

                    default:
                        lblMensagem.Text = "Status desconhecido. Nenhuma ação foi realizada.";
                        break;
                }*/

                 // ModoTela = ModoTelaEnum.TelaExtratoReprovado;
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
                int.TryParse(hdnOcorrenciaId.Value ?? "", out extratoBancarioId);
                if (extratoBancarioId == 0)
                    throw new Exception("STATUS: É necessário selecionar um OPERAÇÃO antes");

                var status = (int?)null;
                try { status = Convert.ToInt32(hidStatus.Value); }
                catch { status = null; }

                switch (status)
                {
                    case 0:
                        throw new Exception("A operação ainda não foi analisada");
                        break;

                    case 1:
                        if (grdExigencias.VisibleRowCount == 0)
                            throw new Exception("Devolver a operação requer pelo menos 1 exigência cadastrada");

                        rnOperacao.AtualizaStatus(extratoBancarioId, 2);
                        hidStatus.Value = "2";
                        lblStatus.Text = ObterDescricaoStatus(hidStatus.Value);
                        lblMensagem.Text = "Operação Devolvida para cumprimento de exigência";
                        btnDevolver.Visible = false;
                        break;
                    case 2:
                        throw new Exception("A operação já consta como devolvida");
                        break;
                    case 3:
                        if (grdExigencias.VisibleRowCount == 0)
                            throw new Exception("Devolver a operação requer pelo menos 1 exigência cadastrada");

                        rnOperacao.AtualizaStatus(extratoBancarioId, 2);
                        hidStatus.Value = "2";
                        lblMensagem.Text = "Operação Devolvida para cumprimento de exigência";
                        lblStatus.Text = ObterDescricaoStatus(hidStatus.Value);
                        btnDevolver.Visible = false;
                        break;

                    default:
                        lblMensagem.Text = "Status desconhecido. Nenhuma ação foi realizada.";
                        break;
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void grdDocumento_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {

           if (e.ButtonID == "btnVizualizar")
            {

            }

        }

   
        protected void grdExigenciaExtrato_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();

                var ee = new Techne.Lyceum.RN.PrestacaoContas.Entidades.OperacaoExigencia();

                    ee.OperacaoExigenciaId  = Convert.ToInt32(grdExigencias.GetRowValues(grdExigencias.EditingRowVisibleIndex, "OPERACAOEXTRATOID"));
                    ee.TipoExigenciaOperacaoId = Convert.ToInt32(e.NewValues["TIPOEXIGENCIAOPERACAOID"]);
                    ee.NotaExplicativa = Convert.ToString(e.NewValues["NOTAEXPLICATIVA"]);
                    if (e.NewValues["APROVADO"] != null)
                        if (e.NewValues["APROVADO"].ToString() == "False")
                            ee.Aprovado = 0;
                        else
                            ee.Aprovado = 1;
                    else
                        ee.Aprovado = -1;
                    ee.UsuarioId = User.Identity.Name;
                    ee.DataAlteracao = DateTime.Now;

                    validacao = rnOperacaoExigencia.Valida(ee,1);

                    if (validacao.Valido)
                    {
                        rnOperacaoExigencia.Atualiza(ee);
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

                var ee = new Techne.Lyceum.RN.PrestacaoContas.Entidades.OperacaoExigencia();

                    ee.OperacaoExigenciaId = Convert.ToInt32(grdExigencias.GetRowValues(grdExigencias.EditingRowVisibleIndex, "OPERACAOEXTRATOID"));
                    ee.TipoExigenciaOperacaoId = Convert.ToInt32(e.NewValues["TIPOEXIGENCIAOPERACAOID"]);
                    ee.NotaExplicativa = Convert.ToString(e.NewValues["NOTAEXPLICATIVA"]);
                    if (e.NewValues["APROVADO"] != null)
                        if (e.NewValues["APROVADO"].ToString() == "False")
                            ee.Aprovado = 0;
                        else
                            ee.Aprovado = 1;
                    else
                        ee.Aprovado = -1;
                    ee.UsuarioId = User.Identity.Name;
                    ee.DataAlteracao = DateTime.Now;

                    validacao = rnOperacaoExigencia.Valida(ee,1);

                    if (validacao.Valido)
                    {
                        rnOperacaoExigencia.Atualiza(ee);
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

                   validacao = rnOperacaoExigencia.ValidaRemocao(exigenciaExtratoId);

                   if (validacao.Valido)
                   {
                       rnOperacaoExigencia.Remove(exigenciaExtratoId);
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
                            plaCreditoeDebito.Visible = false;

                            break;

                        case ModoTelaEnum.TelaSomenteGrid:
                            plaCreditoeDebito.Visible = false;

                            break;

                        case ModoTelaEnum.TelaExtratoNaoPreenchido:

                            plaCreditoeDebito.Visible = true;

                            btnDevolver.Visible = false;
                            btnReprovar.Visible = false;
                            btnAprovar.Visible = false;
                            plaStatus.Visible = false;
                            pcExtratoBancario.TabPages.FindByName("tabExigenciasExtrato").Enabled = false;

                            break;

                        case ModoTelaEnum.TelaExtratoEnviadoParaAnalise:
                        case ModoTelaEnum.TelaExtratoRevisadoPelaAAE:
                            btnDevolver.Visible = true;
                            btnReprovar.Visible = true;
                            btnAprovar.Visible = true;
                            plaStatus.Visible = true;
                            pcExtratoBancario.TabPages.FindByName("tabExigenciasExtrato").Enabled = true;

                            break;

                        case ModoTelaEnum.TelaExtratoLancamentoPelaAAE:
                        case ModoTelaEnum.TelaExtratoDevolvidoParaAAE:
                            plaCreditoeDebito.Visible = true;

                            btnDevolver.Visible = false;
                            btnReprovar.Visible = false;
                            btnAprovar.Visible = false;
                            plaStatus.Visible = false;
                            pcExtratoBancario.TabPages.FindByName("tabExigenciasExtrato").Enabled = false;

                            break;
                        case ModoTelaEnum.TelaExtratoAprovado:
                        case ModoTelaEnum.TelaExtratoReprovado:
                            plaCreditoeDebito.Visible = true;

                            btnDevolver.Visible = false;
                            btnReprovar.Visible = false;
                            btnAprovar.Visible = false;

                            plaStatus.Visible = true;
                            pcExtratoBancario.TabPages.FindByName("tabExigenciasExtrato").Enabled = true;

                            break;
                    }
                }
          }

        public string ObterDescricaoStatus(string status)
        {
            switch (status)
            {
                case "0":
                    return "Lançamento pela AAE";
                case "1":
                    return "Enviado para análise";
                case "2":
                    return "Devolvido para revisão";
                case "3":
                    return "Aprovado";
                case "4":
                    return "Reprovado - " + rnOperacao.RetornaStatusReprovado(Convert.ToInt32(hdnOcorrenciaId.Value));
                default:
                    return "(Status desconhecido)";
            }
        }
    }
}
