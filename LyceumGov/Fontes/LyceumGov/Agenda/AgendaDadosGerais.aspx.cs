using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util.ImportacaoArquivo;
using Techne.Web;
using System.Web;
using System.Threading;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Agenda
{
    [NavUrl("~/Agenda/AgendaDadosGerais.aspx"), ControlText("AgendaDadosGerais"), Title("Agenda de eventos")]
    public partial class AgendaDadosGerais : TPage
    {
        //--------------------------------------------------

        #region PROPRIEDADES DE PÁGINA

        DataTable dataTableAgenda;

        public DataTable DataTableAgenda
        {
            set
            {
                dataTableAgenda = value ?? new DataTable();
            }

            get { return dataTableAgenda; }
        }

        DataTable dt = new DataTable();

        public DataTable Dt
        {
            get { return dt; }
            set { dt = value; }
        }

        public AbasAgendaPaginaDadosGeraisEnum TabPageAtiva
        {
            get
            {
                object o = ViewState["TabPageAtiva"];
                return (o == null) ? 0 : (AbasAgendaPaginaDadosGeraisEnum)o;
            }

            set
            {
                ViewState["TabPageAtiva"] = value;
            }
        }

        public String IdAgenda
        {
            get
            {
                object o = ViewState["IdAgenda"];
                return (o == null) ? String.Empty : (string)o;
            }

            set
            {
                ViewState["IdAgenda"] = value;
            }
        }

        public String MensagemErro { get; set; }
        public String MensagemSucesso { get; set; }
        protected string mensagemImportacao { get; set; }
        protected string arquivoUpload { get; set; }

        #endregion

        //--------------------------------------------------

        #region EVENTOS

        #region EVENTOS GERAIS

        protected void Page_Load(object sender, EventArgs e)
        {
            IdAgenda = Session["SSAGENDAID"] != null ? Session["SSAGENDAID"].ToString() : string.Empty;

            if (IdAgenda != string.Empty)
            {
                if (!Page.IsPostBack)
                {
                    TabPageAtiva = AbasAgendaPaginaDadosGeraisEnum.AbaAgenda;
                }
            }
            else
            {
                Response.Redirect("~/Default.aspx");
            }

            RealizaImportacaoArquivoDeResultadoProcessoSeletivo();
            CarregaAbas(TabPageAtiva);
        }

        protected void pcAgenda_TabClick(object source, DevExpress.Web.ASPxTabControl.TabControlCancelEventArgs e)
        {
            TabPageAtiva = (AbasAgendaPaginaDadosGeraisEnum)Convert.ToInt16(e.Tab.Index);
            CarregaAbas(TabPageAtiva);
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/Agenda/ConsultaAgenda.aspx?retorno=R");
        }

        #endregion

        #region EVENTOS ABA PROCESSO SELETIVO

        protected void btnVisualizaEditalProcessoSeletivo_Click(object sender, ImageClickEventArgs e)
        {
            ExibeEdital(new RN.Agenda.ProcessoSeletivo().ListaEditalPorAgenda(Convert.ToInt32(IdAgenda)));
        }

        #endregion

        #region EVENTOS ABA GESTÃO DO PROCESSO SELETIVO

        protected void btnExportacaoCandidatosInscritos_Click(object sender, EventArgs e)
        {
            ExportacaoCandidatosInscritos();
        }

        protected void btnGeracaoPreMatricula_Click(object sender, EventArgs e)
        {
            GeraPreMatricula(Convert.ToInt32(IdAgenda), User.Identity.Name);
        }

        protected void ArquivoLog_Command(object sender, CommandEventArgs e)
        {
            int idHistoricoImportacao = Convert.ToInt32(e.CommandArgument);
            RN.HistoricoImportacao rnHistoricoImportacao = new Techne.Lyceum.RN.HistoricoImportacao();
            RN.Entidades.HistoricoImportacao historicoImportacao = rnHistoricoImportacao.Consultar(idHistoricoImportacao);

            if (historicoImportacao.HistoricoImportacaoId != 0)
            {
                byte[] arquivoHistoricoImportacao = historicoImportacao.Arquivo;
                new RN.Util.Download().RealizaDownload("arquivolog_" + historicoImportacao.DataImportacao.ToString("ddMMyyyy_HHmmss") + ".zip", arquivoHistoricoImportacao, this);
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "JsScript", "<script type='text/javascript'>window.setTimeout(\"alert('Arquivo/Log de Histórico de importação não encontrado.');\", 1000);</script>");
            }
        }

        protected void grdHistoricoImportacaoProcessoSeletivo_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName != "STATUSPROCESSAMENTO") return;

            switch ((RN.HistoricoImportacao.StatusProcessamento)Convert.ToInt32(e.CellValue))
            {
                case Techne.Lyceum.RN.HistoricoImportacao.StatusProcessamento.EmExecucao:
                    e.Cell.BackColor = System.Drawing.Color.FromArgb(255, 251, 166);
                    break;
                case Techne.Lyceum.RN.HistoricoImportacao.StatusProcessamento.Concluido:
                    e.Cell.BackColor = System.Drawing.Color.FromArgb(202, 243, 188);
                    break;
                case Techne.Lyceum.RN.HistoricoImportacao.StatusProcessamento.Falha:
                    e.Cell.BackColor = System.Drawing.Color.FromArgb(251, 170, 172);
                    break;
                default:
                    break;
            }
        }

        protected void grdHistoricoImportacaoProcessoSeletivo_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != DevExpress.Web.ASPxGridView.GridViewRowType.Data) return;

            Control button = (Control)grdHistoricoImportacaoProcessoSeletivo.FindRowCellTemplateControl(e.VisibleIndex, grdHistoricoImportacaoProcessoSeletivo.Columns["ArquivoLog"] as GridViewDataColumn, "ArquivoLog");
            var scriptManager = ScriptManager.GetCurrent(this.Page);
            if (scriptManager != null)
            {
                scriptManager.RegisterPostBackControl(button);
            }
        }
        #endregion

        #endregion

        //--------------------------------------------------

        #region MÉTODOS

        #region MÉTODOS GERAIS

        private void CarregaAbas(AbasAgendaPaginaDadosGeraisEnum abasAgendaPaginaDadosGeraisEnum)
        {
            int agendaId = Convert.ToInt32(IdAgenda);

            switch (abasAgendaPaginaDadosGeraisEnum)
            {
                case AbasAgendaPaginaDadosGeraisEnum.AbaAgenda:
                    Dt = new RN.Agenda.Agenda().ConsultaDadosGeraisAgenda(agendaId);
                    CarregaAbaDadosGerais(Dt);
                    break;

                case AbasAgendaPaginaDadosGeraisEnum.AbaCursoPorUnidade:
                    Dt = new RN.Agenda.Agenda_Curso__Agenda_UnidadeEnsino().ListaCursoPorUnidadePorAgendaAgrupadoPorTurno(agendaId);
                    grdCursoPorUnidade = PopulaGridView(Dt, grdCursoPorUnidade, "Cursos por Unidade");
                    break;

                case AbasAgendaPaginaDadosGeraisEnum.AbaEventos:
                    Dt = new RN.Agenda.Evento().ListaEventosPorAgendaComDescricaoTipoEvento(agendaId);
                    grdEventos = PopulaGridView(Dt, grdEventos, "Eventos");
                    break;

                case AbasAgendaPaginaDadosGeraisEnum.AbaGestaoProcessoSeletivo:
                    Dt = new RN.ProcessoSeletivoAluno.ProcessoSeletivo_HistoricoImportacao().ListarPorAgendaId(agendaId);
                    grdHistoricoImportacaoProcessoSeletivo = PopulaGridView(Dt, grdHistoricoImportacaoProcessoSeletivo, "Histórico de Importação de Classificados do Processo Seletivo");

                    Dt = new RN.ProcessoSeletivoAluno.ProcessoSeletivo_HistoricoImportacao().ListaHistoricoGeracaoPreMatricula(agendaId);
                    grdHistoricoGeracaoPreMatricula = PopulaGridView(Dt, grdHistoricoGeracaoPreMatricula, "Histórico de Geração de Pré-Matrícula");

                    HabilitaCamposAbaGestaoProcessoSeletivo();

                    //------------------------------------------------------------------------------------------
                    //Tratamento para realizar o download quando tiver um UpdatePanel do AJAX
                    //------------------------------------------------------------------------------------------
                    //Realizado por: Lucas Collina - Data: 09/12/2013
                    //Adiciona o evento FullPostBack ao botão de Exportação de Candidatos Inscritos
                    ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(this.btnExportacaoCandidatosInscritos);
                    ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(this.btnGeracaoPreMatricula);

                    break;

                case AbasAgendaPaginaDadosGeraisEnum.AbaProcessoSeletivo:
                    Dt = new RN.Agenda.ProcessoSeletivo().ConsultaPorAgendaId(agendaId);
                    CarregaAbaProcSeletivo(Dt);

                    Dt = new RN.Agenda.UnidadeEnsinoProcessoSeletivo().ListaPorAgendaId(agendaId);
                    grdUnidadeEnsinoProcessoSeletivo = PopulaGridView(Dt, grdUnidadeEnsinoProcessoSeletivo, "Parametrização Unidade de Ensino Participante");

                    break;
            }
        }

        /// <summary>
        /// Carrega controles baseados em listas
        /// </summary>
        /// <param name="idControl">Id do Controle</param>
        /// <param name="textField">Nome do TEXTFIELD</param>
        /// <param name="valueField">Nome do VALUEFIELD</param>
        private void CarregarControle(string idControl, string textField, string valueField)
        {
            switch (idControl.ToUpper())
            {
                case "DDLTIPOEVENTO":
                    {
                        ddlTipoEvento.DataSource = RN.Agenda.TipoEvento.ListaTipoEventoPaiAtivo(1);
                        ddlTipoEvento.DataBind();
                        ddlTipoEvento.DataTextField = textField;
                        ddlTipoEvento.DataValueField = valueField;
                        ListItem item = new ListItem("<selecione>", "");
                        ddlAno.Items.Insert(0, item);
                        break;
                    }
                case "DDLANO":
                    {
                        ddlAno.DataSource = RN.PeriodoLetivo.ConsultarAno();
                        ddlAno.DataBind();
                        ddlAno.DataTextField = "ANO";
                        ddlAno.DataValueField = "ANO";
                        ListItem item = new ListItem("<selecione>", "");
                        ddlAno.Items.Insert(0, item);
                        break;
                    }
                case "CBLPERIODO":
                    {
                        cblPeriodo.DataSource = RN.PeriodoLetivo.ConsultarPeriodo(Session["ssDDLANO"].ToString());
                        cblPeriodo.DataBind();
                        cblPeriodo.DataTextField = textField;
                        cblPeriodo.DataValueField = valueField;
                        break;
                    }
                default:
                    break;
            }
        }

        /// <summary>
        /// Formata a checkbox de período
        /// </summary>
        /// <param name="ddlValue"></param>
        /// <param name="ChkBoxValue"></param>
        private void FormataCblperiodo(string ddlValue, string ChkBoxValue)
        {
            string[] valor = ChkBoxValue.Split(',');
            for (int i = 0; i < valor.Count(); i++)
            {
                if (Convert.ToInt32(valor[i]) == i)
                {
                    cblPeriodo.Items[Convert.ToInt32(valor[i])].Selected = true;
                }
            }
        }

        /// <summary>
        /// Popula a GridView
        /// </summary>
        /// <param name="DadosAgenda"></param>
        private ASPxGridView PopulaGridView(DataTable DadosAgenda, ASPxGridView grdv, string tituloGdv)
        {
            grdv.Visible = true;
            grdv.DataSource = DadosAgenda;
            grdv.DataBind();
            grdv.SettingsText.Title = tituloGdv;
            return grdv;
        }

        /// <summary>
        /// Exibe uma janela de popup com conteúdo html
        /// </summary>
        /// <param name="div"></param>
        /// <param name="cabecalhoPopup"></param>
        private void ExibePopup(string conteudoDiv, string cabecalhoPopup)
        {
            popupAgenda.HeaderText = cabecalhoPopup;
            divPopup.Visible = true;
            divConteudoPopup.InnerHtml = conteudoDiv;
            popupAgenda.ShowOnPageLoad = true;
        }

        #endregion

        #region MÉTODOS ABA DADOS GERAIS

        /// <summary>
        /// Carrega os controles da aba Dados Gerais
        /// </summary>
        /// <param name="dtAbaDados"></param>
        private void CarregaAbaDadosGerais(DataTable dtAbaDados)
        {
            for (int i = 0; i < dtAbaDados.Rows.Count; i++)
            {
                txtDescricaoDadosGerais.Text = dtAbaDados.Rows[i]["DESCRICAO"].ToString();

                txtDataInicioDadosGerais.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dtAbaDados.Rows[i]["DATAINICIO"]));
                txtDataFimDadosGerais.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dtAbaDados.Rows[i]["DATAFIM"]));

                CarregarControle(ddlTipoEvento.ID, "NOME", "TIPOEVENTOID");
                CarregarControle(ddlAno.ID, "ANO", "ANO");
                CarregarControle(cblPeriodo.ID, "PERIODO", "ID_REDUZIDA");

                ddlTipoEvento.SelectedValue = dtAbaDados.Rows[i]["TIPOEVENTOID"].ToString();
                FormataCblperiodo(Session["ssDDLANO"].ToString(), Session["ssCHKVALUE"].ToString());
                ddlAno.SelectedValue = Session["ssDDLANO"].ToString();

                rblCursoParticipante.SelectedValue = dtAbaDados.Rows[i]["PARTICIPACURSOID"].ToString();
                rblUnidadeParticipante.SelectedValue = dtAbaDados.Rows[i]["PARTICIPAUNIDADEID"].ToString();
                chkCursoPorUnidade.Checked = Convert.ToBoolean(dtAbaDados.Rows[i]["CURSOPORUNIDADE"]);

                txtObservacao.Text = dtAbaDados.Rows[i]["OBSERVACAO"].ToString();
            }
        }

        #endregion

        #region MÉTODOS ABA PROCESSO SELETIVO

        /// <summary>
        /// Carrega os controles da aba Processo Seletivo
        /// </summary>
        /// <param name="Dt"></param>
        private void CarregaAbaProcSeletivo(DataTable Dt)
        {
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                txtNumeroEdital.Text = Dt.Rows[i]["NUMEROEDITAL"].ToString();
                txtTextoEdital.Text = Dt.Rows[i]["NOMEARQUIVOEDITAL"].ToString();
                dtNascimentoInicial.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Dt.Rows[i]["DATANASCIMENTOINICIAL"].ToString()));
                dtNascimentoFinal.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Dt.Rows[i]["DATANASCIMENTOFINAL"].ToString()));
            }
        }

        /// <summary>
        /// Converte o array de bytes em string, monta o html do edital e chama o popup para exibição
        /// </summary>
        /// <param name="Dt"></param>
        private void ExibeEdital(DataTable Dt)
        {
            string conteudoDiv = string.Empty;
            if (Dt.Rows.Count > 0)
            {
                conteudoDiv += "<div style='margin: 100px;'>";
                conteudoDiv += "<table style='width: 100%;margin=100px;'>";
                // Converte o byte[] para String
                byte[] dBytes = Dt.Rows[0]["EDITAL"] as byte[]; // seu array de bytes.
                string str; // String que irá receber a conversão
                Encoding enc = Encoding.GetEncoding("iso-8859-1");
                str = enc.GetString(dBytes);
                conteudoDiv += "<tr> <td>";
                conteudoDiv += str;
                conteudoDiv += "</td> </tr>";
                conteudoDiv += "</table>";
                conteudoDiv += "</div>";
            }

            ExibePopup(conteudoDiv, "Edital");
        }

        #endregion

        #region MÉTODOS ABA GESTÃO DO PROCESSO SELETIVO

        private void HabilitaCamposAbaGestaoProcessoSeletivo()
        {
            RN.ProcessoSeletivoAluno.ProcessoSeletivo_HistoricoImportacao rnAgenda_HistoricoImportacao = new RN.ProcessoSeletivoAluno.ProcessoSeletivo_HistoricoImportacao();
            btnGeracaoPreMatricula.Enabled = rnAgenda_HistoricoImportacao.VerificaImportacaoConcluidaPorAgendaETipoImportacao(Convert.ToInt32(IdAgenda), (int)RN.HistoricoImportacao.TipoImportacaoEnum.ClassificadosProcessoSeletivoAlunoCeperj);
        }

        /// <summary>
        /// Gera Pre Matricula de Alunos
        /// </summary>
        /// <param name="idAgenda"></param>
        /// <param name="userId"></param>
        public void GeraPreMatricula(int idAgenda, string userId)
        {
            string conteudoDiv = string.Empty;
            string chamada = string.Empty;
            string mensagemErroProcedure = string.Empty;

            btnGeracaoPreMatricula.Enabled = false;

            if (grdHistoricoImportacaoProcessoSeletivo.VisibleRowCount > 0)
            {
                dt = new RN.Agenda.ProcessoSeletivo().GeraAlunoPreMatricula(idAgenda, userId, ref mensagemErroProcedure);

                if (mensagemErroProcedure != string.Empty)
                {
                    RN.Util.Utils.ExibeMensagem(mensagemErroProcedure, this);
                }
                else
                {
                    if (dt != null && dt.Rows.Count == 0)
                    {
                        chamada = new RN.Agenda.ProcessoSeletivo().RetornaNumeroChamada(idAgenda).Rows[0]["NUMEROCHAMADA"].ToString();
                        //MENSAGEM DE SUCESSO NA GERAÇÃO DE PRE-MATRÍCULA 
                        MensagemSucesso = "Pré-matrícula dos Candidatos Classificados da " + chamada + "ª chamada gerada com sucesso!";
                        RN.Util.Utils.ExibeMensagem(MensagemSucesso, this);
                    }
                    else
                    {
                        //MENSAGEM DE ERRO NA GERAÇÃO DE PRE-MATRÍCULA 
                        MensagemErro = "Foram encontrados erros durante o processo de Geração de Pré-Matrícula.";
                        RN.Util.Utils.ExibeMensagem(MensagemErro, this);

                        conteudoDiv = "<div>";
                        foreach (DataRow item in dt.Rows)
                        {
                            conteudoDiv += "</br><div style='padding-left: 10px;'>" + item["ERRO"].ToString() + "</div>";
                        }
                        conteudoDiv += "</div>";

                        ExibePopup(conteudoDiv, "Erros encontrados na Geração de Pré-Matrícula");
                    }
                }
            }

            CarregaAbas(AbasAgendaPaginaDadosGeraisEnum.AbaGestaoProcessoSeletivo);
        }

        /// <summary>
        /// Exporta todos candidatos inscritos em um concurso para um arquivo CSV
        /// </summary>
        private void ExportacaoCandidatosInscritos()
        {
            ZipFileStream zipFileStream = new ZipFileStream();
            bool existeCandidatoInscrito = false;

            //FA1
            existeCandidatoInscrito = new RN.ProcessoSeletivoAluno.Inscricao().VerificaSeExisteCandidatoInscrito(Convert.ToInt32(IdAgenda));

            if (existeCandidatoInscrito)
            {
                //========Exporta Inscritos para csv e adiciona ao arquivo zip
                Dt = new RN.Agenda.ProcessoSeletivo().ExportaDadosInscritos(Convert.ToInt32(IdAgenda));
                zipFileStream.AdicionaFileStreamZip("candidato.csv", new RN.Util.ExportaCsv().ExportaDataTableCSV(Dt, ";"));

                //========Exporta Inscritos para csv e adiciona ao arquivo zip
                Dt = new RN.Agenda.ProcessoSeletivo().ExportaRecursosAplicacaoProvaInscritos(Convert.ToInt32(IdAgenda));
                zipFileStream.AdicionaFileStreamZip("recursoAplicacaoProva.csv", new RN.Util.ExportaCsv().ExportaDataTableCSV(Dt, ";"));

                //========Exporta Inscritos para csv
                Dt = new RN.Agenda.ProcessoSeletivo().ExportaUnidadeEnsinoCursoTurnoInscritos(Convert.ToInt32(IdAgenda));
                zipFileStream.AdicionaFileStreamZip("undidadeEnsino_Curso_Turno.csv", new RN.Util.ExportaCsv().ExportaDataTableCSV(Dt, ";"));

                //========Adiciona arquivos ao zip , nomeia o zip e exibe download
                byte[] array = zipFileStream.retornaBytesZipados();
                new RN.Util.Download().RealizaDownload("candidatosinscritos_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".zip", array, this);
            }
            else
            {   //MENSAGEM DE ERRO M1
                MensagemErro = "Não há candidatos inscritos a serem exportados";
                RN.Util.Utils.ExibeMensagem(MensagemErro, this);
            }
        }

        #region Evento de Importação de Arquivo de Resultado

        private void RealizaImportacaoArquivoDeResultadoProcessoSeletivo()
        {
            if (importaArquivo.Value == "1")
            {
                if (!string.IsNullOrEmpty(FileUpload.Value))
                {
                    string mensagemImportacao = string.Empty;
                    IniciaUpload(ref mensagemImportacao);
                    popupProcessamento.ShowOnPageLoad = false;
                    ClientScript.RegisterClientScriptBlock(GetType(), "JsScript", "<script type='text/javascript'>window.setTimeout(\"alert('" + mensagemImportacao + "');\", 1000);</script>");
                }

                importaArquivo.Value = "";
            }
        }

        /// <summary>
        /// Realiza o Upload do Arquivo de Importação de Resultado
        /// </summary>
        protected void IniciaUpload(ref string mensagemImportacao)
        {
            RN.Importacao.ImportaResultadoProcessoSeletivoAluno importaResultadoProcessoSeletivoAluno = new Techne.Lyceum.RN.Importacao.ImportaResultadoProcessoSeletivoAluno(Convert.ToInt32(IdAgenda), FileUpload, Techne.Lyceum.RN.HistoricoImportacao.TipoImportacaoEnum.ClassificadosProcessoSeletivoAlunoCeperj, User.Identity.Name);
            mensagemImportacao = importaResultadoProcessoSeletivoAluno.ImportarArquivo();
        }

        #endregion
        
        #region Métodos para Envio de E-mail ---- AINDA NÃO UTILIZADOS

        /// <summary>
        /// Monta a lista e envia emails aos candidatos aprovados
        /// </summary>
        /// <param name="idagenda"></param>
        /// <param name="delimitadores"></param>
        /// <param name="NomeCampoEmail"></param>
        /// <returns></returns>
        public bool MontaEmail(int idagenda, char delimitadores, string NomeCampoEmail)
        {
            DataTable DtListaEmails = new DataTable();
            DataTable DtDadosEmails = new DataTable();
            DataTable DtCamposSusb = new DataTable();
            String idEnvioMensagem = string.Empty;
            String Remetente = string.Empty;
            String Assunto = string.Empty;
            String Mensagem = string.Empty;
            bool ret;
            DtDadosEmails = new RN.Agenda.Agenda_EnvioMensagem().SelecionaDadosEmail(idagenda);
            if (DtDadosEmails.Rows.Count > 0)
            {
                int contadorErro = 0;
                foreach (DataRow item in DtDadosEmails.Rows)
                {
                    List<string> Retorno = new List<string>();
                    idEnvioMensagem = item["ENVIOMENSAGEMID"].ToString();
                    DtListaEmails = new RN.ProcessoSeletivoAluno.Inscricao_EnvioMensagem().ListaDestinatarioEmail(Convert.ToInt32(idEnvioMensagem));
                    foreach (DataRow drListaEmail in DtListaEmails.Rows)
                    {
                        String ListaCampos = string.Empty;
                        Mensagem = item["MENSAGEM"].ToString();
                        Remetente = item["REMETENTE"].ToString();
                        Assunto = item["ASSUNTO"].ToString();
                        Retorno = Mensagem.Split(delimitadores).ToList();
                        int idMsg = Convert.ToInt32(drListaEmail["INSCRICAO_ENVIOMENSAGEM_ID"]);
                        for (int i = 0; i < Retorno.Count; i++)
                        {
                            if (!(i % 2 == 0))
                                ListaCampos += " '" + delimitadores + "" + Retorno[i] + "" + delimitadores + "',";
                        }
                        ListaCampos = ListaCampos.Remove(ListaCampos.Length - 1);
                        DtCamposSusb = new RN.CampoEnvioMensagem().ListaCampoSubstituido(ListaCampos);
                        foreach (DataRow drCampos in DtCamposSusb.Rows)
                        {
                            Mensagem = Mensagem.Replace(drCampos["IDENTIFICADORCAMPO"].ToString(), drListaEmail[drCampos["NOMECAMPO"].ToString()].ToString());
                        }
                        RN.DTOs.DadosEmail dadosMail = new Techne.Lyceum.RN.DTOs.DadosEmail();
                        dadosMail.Assunto = Assunto;
                        dadosMail.Destinatario = drListaEmail[NomeCampoEmail].ToString();
                        dadosMail.Remetente = Remetente;
                        dadosMail.Texto = Mensagem;
                        if (RN.Util.Email.EnviarMail(dadosMail) == true)
                        {
                            new RN.ProcessoSeletivoAluno.Inscricao_EnvioMensagem().AtualizaSituacaoEnvio(idMsg);
                        }
                        else
                        {
                            contadorErro++;
                            new RN.ProcessoSeletivoAluno.Inscricao_EnvioMensagem().MensagemErroEnvio(idMsg, "Erro durante o processo de envio de emails aos candidatos, não enviando " + contadorErro + " de " + DtListaEmails.Rows.Count + " mensagens");
                        }
                    }
                    if (contadorErro > 0)
                    {
                        MensagemErro = "Erro durante o processo de envio de emails aos candidatos, não enviando " + contadorErro + " de " + DtListaEmails.Rows.Count + " mensagens";
                        RN.Util.Utils.ExibeMensagem(MensagemErro, this);
                        return ret = false;
                    }
                }
                ret = true;
            }
            else
            {
                ret = false;
            }

            return ret;
        }

        /// <summary>
        /// Envia emails para alunos com pre-matricula gerada
        /// </summary>
        public void EnviaEmail()
        {
            if (grdHistoricoImportacaoProcessoSeletivo.VisibleRowCount > 0)
            {
                if (new RN.Agenda.ProcessoSeletivo().GeraPreMatriculaCandidatos(Convert.ToInt32(IdAgenda)).Rows.Count != 0)
                {
                    MensagemSucesso = "Geração de pre-matricula efetuada com sucesso! O sistema irá inicializar o processo de envio de emails a todos os candidatos classificados";
                    RN.Util.Utils.ExibeMensagem(MensagemSucesso, this);

                    Dt = new RN.Agenda.Agenda_EnvioMensagem().VerificaMensagemEmail(Convert.ToInt32(IdAgenda));
                    if (Dt.Rows.Count != 0)
                    {
                        int ret = new RN.ProcessoSeletivoAluno.Inscricao_EnvioMensagem().PreparaEmail(Convert.ToInt32(IdAgenda), User.Identity.Name);
                        if (ret > 0)
                        {
                            if (MontaEmail(Convert.ToInt32(IdAgenda), '#', "EMAIL") == true)
                            {
                                MensagemSucesso = "Processo de envio de e-mail concluído";//MENSAGEM M8
                                RN.Util.Utils.ExibeMensagem(MensagemSucesso, this);
                            }
                        }
                        else
                        {
                            MensagemErro = "Erro na preparação dos emails para envio, favor contactar o suporte";
                            RN.Util.Utils.ExibeMensagem(MensagemErro, this);
                            return;
                        }
                    }
                    else
                    {
                        MensagemErro = "Mensagem de e-mail padrão para candidatos classificados não encontrada";//MENSAGEM M5
                        RN.Util.Utils.ExibeMensagem(MensagemErro, this);
                        return;
                    }
                }
                else
                {
                    //MENSAGEM DE ERRO NA GERAÇÃO DE PRE-MATRÍCULA DO EMAIL
                    MensagemErro = "Erro durante a geração de Pré-Matricula! Favor efetuar novamente a operação ou contate o suporte";
                    RN.Util.Utils.ExibeMensagem(MensagemErro, this);
                    return;
                }
            }
        }

        /// <summary>
        /// Prepara e envia emails para alunos com pre-matricula gerada
        /// </summary>
        /// <param name="IdAgenda"></param>
        /// <param name="userId"></param>
        private void EnviaEmailAlunos(int IdAgenda, string userId)
        {
            Dt = new RN.Agenda.Agenda().VerificaMensagemEmail(IdAgenda);
            if (Dt.Rows.Count != 0)
            {
                int ret = new RN.Agenda.Agenda().PreparaEmail(IdAgenda, userId);
                if (ret > 0)
                {
                    if (MontaEmail(Convert.ToInt32(IdAgenda), '#', "EMAIL") == true)
                    {
                        MensagemSucesso = "Processo de envio de e-mail concluído";//MENSAGEM M8
                        RN.Util.Utils.ExibeMensagem(MensagemSucesso, this);
                    }
                }
                else
                {
                    MensagemErro = "Erro na preparação dos emails para envio, favor contactar o suporte";
                    RN.Util.Utils.ExibeMensagem(MensagemErro, this);

                }
            }
            else
            {
                MensagemErro = "Mensagem de e-mail padrão para candidatos classificados não encontrada";//MENSAGEM M5
                RN.Util.Utils.ExibeMensagem(MensagemErro, this);

            }
        }

        #endregion

        #endregion

        #endregion

        //--------------------------------------------------

    }
}
