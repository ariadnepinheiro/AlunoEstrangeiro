using System;
using System.Data;
using System.Web;
using DevExpress.Web.ASPxGridView;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using DevExpress.Web.Data;
using DevExpress.Web.ASPxEditors;
using System.Linq;
using System.Collections.Generic;

namespace Techne.Lyceum.Net.AvaliacaoExterna
{
    [NavUrl("~/AvaliacaoExterna/EtapaTurma.aspx")]
    [ControlText("Turmas")]
    [Title("Turmas")]
    public partial class EtapaTurma : TPage
    {
        public readonly RN.AvaliacaoExterna.ReaberturaTurma rnReaberturaTurma;
        public readonly RN.AvaliacaoExterna.Avaliacao rnAvaliacao;
        public readonly RN.AvaliacaoExterna.Prova rnProva;
        public readonly RN.AvaliacaoExterna.Etapa rnEtapa;

        public EtapaTurma()
        {
            rnReaberturaTurma = new Techne.Lyceum.RN.AvaliacaoExterna.ReaberturaTurma();
            rnAvaliacao = new Techne.Lyceum.RN.AvaliacaoExterna.Avaliacao();
            rnProva = new Techne.Lyceum.RN.AvaliacaoExterna.Prova();
            rnEtapa = new Techne.Lyceum.RN.AvaliacaoExterna.Etapa();
        }

        public int Ano
        {
            get
            {
                return rnAvaliacao.ObtemAnoPorAvaliacaoId(AvaliacaoId);
            }
        }

        public int AvaliacaoId
        {
            get
            {
                int avaliacaoId;
                string key = this.QueryStringDecodificada["avaliacaoId"]; 
                int.TryParse(key ?? "0", out avaliacaoId);
                return avaliacaoId;
            }
        }

        public int EtapaId
        {
            get
            {
                int etapaId;
                string key = this.QueryStringDecodificada["etapaId"];
                int.TryParse(key ?? "0", out etapaId);
                return etapaId;
            }
        }

        public int ProvaId
        {
            get
            {
                int provaId;
                string key = this.QueryStringDecodificada["provaId"];
                int.TryParse(key ?? "0", out provaId);
                return provaId;
            }
        } 

        public int Periodo
        {
            get
            {
                int periodo;
                string key = this.QueryStringDecodificada["periodo"];
                int.TryParse(key ?? "0", out periodo);
                return periodo;
            }
        }

        public int Unidade_Ens
        {
            get
            {
                int unidade_ens;
                string key = this.QueryStringDecodificada["unidade_ens"];
                int.TryParse(key ?? "0", out unidade_ens);
                return unidade_ens;
            }
        }

        private void grdEtapaTurmaDataBind()
        {          
            int? periodo = null;
            if (ddlPeriodo != null && ddlPeriodo.SelectedIndex >= 0 && !ddlPeriodo.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                periodo = Convert.ToInt32((ddlPeriodo.SelectedValue));

            int? unidadeEnsinoId = null;
            if (tseUnidadeResponsavel != null && tseUnidadeResponsavel.IsValidDBValue && !tseUnidadeResponsavel.DBValue.IsNull)
                unidadeEnsinoId = Convert.ToInt32(tseUnidadeResponsavel.DBValue);

            int? provaId = null;
            if (ddlProva != null && ddlProva.SelectedIndex >= 0 && !ddlProva.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                provaId = Convert.ToInt32((ddlProva.SelectedValue));

            int? etapaId = null;
            if (ddlEtapa != null && ddlEtapa.SelectedIndex >= 0 && !ddlEtapa.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                etapaId = Convert.ToInt32((ddlEtapa.SelectedValue));

            odsEtapaTurma.SelectParameters["periodo"].DefaultValue = periodo.HasValue ? periodo.ToString() : null;
            odsEtapaTurma.SelectParameters["unidadeEnsino"].DefaultValue = unidadeEnsinoId.HasValue ? Convert.ToString(unidadeEnsinoId) : null;
            odsEtapaTurma.SelectParameters["provaId"].DefaultValue = periodo.HasValue ? provaId.ToString() : null;
            odsEtapaTurma.SelectParameters["etapaId"].DefaultValue = periodo.HasValue ? etapaId.ToString() : null;

            grdEtapaTurma.DataBind();
        }

        public object ListaProva(object avaliacaoId, object censo)
        {
            if (Convert.ToString(avaliacaoId).IsNullOrEmptyOrWhiteSpace() || Convert.ToString(censo).IsNullOrEmptyOrWhiteSpace())
                return null;

            DataTable lista = rnProva.ListaAtivoPor(Convert.ToInt32(avaliacaoId), censo.ToString());
            DataRow newRow = lista.NewRow();
            lista.Rows.InsertAt(newRow, 0);
            return lista;
        }

        public object ListaEtapa(object provaId, object censo)
        {
            if (provaId == null || Convert.ToString(censo).IsNullOrEmptyOrWhiteSpace())
                return null;

            DataTable lista = rnEtapa.ListaAtivoPor(Convert.ToInt32(provaId), censo.ToString());
            DataRow newRow = lista.NewRow();
            lista.Rows.InsertAt(newRow, 0);
            return lista;
        }

        public DataTable Lista(int? periodo, string unidadeEnsino, int? provaId, int? etapaId)
        {
            if (!periodo.HasValue || unidadeEnsino.IsNullOrEmptyOrWhiteSpace() || !provaId.HasValue || !etapaId.HasValue)
                return null;

            return rnReaberturaTurma.Consultar(Convert.ToInt32(periodo), unidadeEnsino, Convert.ToInt32(provaId), Convert.ToInt32(etapaId));
        }

        public void Update(object TURMA) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdEtapaTurma, "Transcrição das Respostas");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
           ControlaAcessoGrid();           
        }

        protected void ControlaAcessoGrid()
        {
            if (grdEtapaTurma != null)
            {
                if (!Permission.AllowDelete && !Permission.AllowInsert && !Permission.AllowUpdate)
                {
                    grdEtapaTurma.Columns[""].Visible = false;
                }
            }
            ControlaAcesso(grdEtapaTurma, AcaoControle.editar, "btnVisualizarTranscricao");
            ControlaAcesso(grdEtapaTurma, AcaoControle.novo, "btnTranscricaoResposta");
            ControlaAcesso(grdEtapaTurma, AcaoControle.editar, "btnSolicitacaoReabertura");
            ControlaAcesso(grdEtapaTurma);
        }

        protected void tseAvaliacao_PreRender(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            if (AvaliacaoId > 0)
            {
                tseAvaliacao.Value = AvaliacaoId.ToString();
            }
        }

        protected void tseAvaliacao_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            ddlProva.ClearSelection();
            ddlProva.DataBind();
            ddlEtapa.ClearSelection();
            ddlEtapa.DataBind();
            grdEtapaTurmaDataBind();
        }

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            tseMunicipio.ResetValue();
            tseUnidadeResponsavel.ResetValue();
        }

        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            tseUnidadeResponsavel.ResetValue();
        }

        protected void tseUnidadeResponsavel_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            if (tseUnidadeResponsavel.IsValidDBValue && !tseUnidadeResponsavel["unidade_ens"].IsNull)
            {
                tseRegional.Value = tseUnidadeResponsavel["id_regional"];
                tseMunicipio.Value = tseUnidadeResponsavel["municipio"];
            }

            grdEtapaTurmaDataBind();
        }

        protected void tseUnidadeResponsavel_PreRender(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            if (Unidade_Ens > 0)
            {
                tseUnidadeResponsavel.Value = Unidade_Ens.ToString();
                tseUnidadeResponsavel_Changed(sender, null);
            }
        }

        protected void ddlPeriodo_PreRender(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            if (Periodo >= 0)
            {
                ddlPeriodo.SelectedValue = Periodo.ToString();
            }
        }

        protected void ddlEtapa_PreRender(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            if (EtapaId >= 0)
            {
                ddlEtapa.SelectedValue = EtapaId.ToString();
                grdEtapaTurmaDataBind();
            }
        }

        protected void ddlProva_PreRender(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            if (ProvaId >= 0)
            {
                ddlProva.SelectedValue = ProvaId.ToString();
            }
        }

        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlProva.ClearSelection();
            ddlProva.DataBind();
            ddlEtapa.ClearSelection();
            ddlEtapa.DataBind();
            grdEtapaTurmaDataBind();
        }

        protected void ddlProva_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlEtapa.ClearSelection();
            ddlEtapa.DataBind();
            grdEtapaTurmaDataBind();
        }

        protected void ddlEtapa_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdEtapaTurmaDataBind();
        }

        protected void grdEtapaTurma_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex == -1)
                return;
            
            DataRowView dataRowView = grdEtapaTurma.GetRow(e.VisibleIndex) as DataRowView;
            if (dataRowView == null)
                return;

            if (grdEtapaTurma.IsEditing)
            {
                if (!e.IsEditingRow && (e.ButtonID == "btnTranscricaoResposta" || e.ButtonID == "btnSolicitacaoReabertura" || e.ButtonID == "btnVisualizarTranscricao"))
                {
                    e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                }
                else if (e.IsEditingRow && (e.ButtonID == "btnTranscricaoResposta" || e.ButtonID == "btnSolicitacaoReabertura" || e.ButtonID == "btnVisualizarTranscricao"))
                {
                    e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                }
                else
                {
                    e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.Default;
                }
            }
            else
            {
/*
TABELA DE DECISÃO PARA VISIBILIDADE DOS BOTÕES
                                                                                        Reabertura Transcricao
FIMTRANSCRICAO >= Dia de hoje && INICIOTRANSCRICAO <= Dia de hoje                       false      true       
FIMTRANSCRICAO >= Dia de hoje && INICIOTRANSCRICAO > Dia de hoje                        false      false
FIMTRANSCRICAO < Dia de hoje && REABERTURATURMAID == NULL                               true       false
FIMTRANSCRICAO < Dia de hoje && REABERTURATURMAID != NULL && DATAFECHAMENTO  == NULL    false      false
FIMTRANSCRICAO < Dia de hoje && REABERTURATURMAID != NULL && FECHAMENTO < Dia de hoje   false      false		
FIMTRANSCRICAO < Dia de hoje && REABERTURATURMAID != NULL && FECHAMENTO >= Dia de hoje  false      true
*/

                if (e.CellType == GridViewTableCommandCellType.Data && Convert.ToString(dataRowView.Row["STATUSTRANSCRICAO"]) == "FINALIZADA")
                {
                    if (e.ButtonID == "btnSolicitacaoReabertura" && new string[] { "", "APROVADO", "REPROVADO" }.Contains(Convert.ToString(dataRowView.Row["STATUS"])))
                    {
                        e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.Default;
                    }
                    else
                    {
                        e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                    }

                    if (e.ButtonID == "btnTranscricaoResposta")
                        e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                    if (e.ButtonID == "btnVisualizarTranscricao")
                        e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.Default;
                }
                else if (e.CellType == GridViewTableCommandCellType.Data && Convert.ToString(dataRowView.Row["STATUSTRANSCRICAO"]) != "FINALIZADA")
                {
                    if (e.CellType == GridViewTableCommandCellType.Data && Convert.ToDateTime(dataRowView.Row["FIMTRANSCRICAO"]).Date >= DateTime.Now.Date && Convert.ToDateTime(dataRowView.Row["INICIOTRANSCRICAO"]).Date <= DateTime.Now.Date)
                    {
                        if (e.ButtonID == "btnSolicitacaoReabertura")
                            e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                        if (e.ButtonID == "btnTranscricaoResposta")
                            e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.Default;
                        if (e.ButtonID == "btnVisualizarTranscricao")
                            e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                    }

                    if (e.CellType == GridViewTableCommandCellType.Data && Convert.ToDateTime(dataRowView.Row["FIMTRANSCRICAO"]).Date >= DateTime.Now.Date && Convert.ToDateTime(dataRowView.Row["INICIOTRANSCRICAO"]).Date > DateTime.Now.Date)
                    {
                        if (e.ButtonID == "btnSolicitacaoReabertura")
                            e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                        if (e.ButtonID == "btnTranscricaoResposta")
                            e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                        if (e.ButtonID == "btnVisualizarTranscricao")
                            e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                    }

                    if (e.CellType == GridViewTableCommandCellType.Data && Convert.ToDateTime(dataRowView.Row["FIMTRANSCRICAO"]).Date < DateTime.Now.Date && dataRowView.Row["REABERTURATURMAID"] == System.DBNull.Value)
                    {
                        if (e.ButtonID == "btnSolicitacaoReabertura")
                            e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.Default;
                        if (e.ButtonID == "btnTranscricaoResposta")
                            e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                        if (e.ButtonID == "btnVisualizarTranscricao")
                            e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                    }

                    if (e.CellType == GridViewTableCommandCellType.Data && Convert.ToDateTime(dataRowView.Row["FIMTRANSCRICAO"]).Date < DateTime.Now.Date && dataRowView.Row["REABERTURATURMAID"] != System.DBNull.Value && dataRowView.Row["DATAFECHAMENTO"] == System.DBNull.Value)
                    {
                        if (e.ButtonID == "btnSolicitacaoReabertura")
                            e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                        if (e.ButtonID == "btnTranscricaoResposta")
                            e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                        if (e.ButtonID == "btnVisualizarTranscricao")
                            e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                    }

                    if (e.CellType == GridViewTableCommandCellType.Data && Convert.ToDateTime(dataRowView.Row["FIMTRANSCRICAO"]).Date < DateTime.Now.Date && dataRowView.Row["REABERTURATURMAID"] != System.DBNull.Value && dataRowView.Row["DATAFECHAMENTO"] != System.DBNull.Value && Convert.ToDateTime(dataRowView.Row["DATAFECHAMENTO"]).Date < DateTime.Now.Date)
                    {
                        if (e.ButtonID == "btnSolicitacaoReabertura")
                            e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                        if (e.ButtonID == "btnTranscricaoResposta")
                            e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                        if (e.ButtonID == "btnVisualizarTranscricao")
                            e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                    }

                    if (e.CellType == GridViewTableCommandCellType.Data && Convert.ToDateTime(dataRowView.Row["FIMTRANSCRICAO"]).Date < DateTime.Now.Date && dataRowView.Row["REABERTURATURMAID"] != System.DBNull.Value && dataRowView.Row["DATAFECHAMENTO"] != System.DBNull.Value && Convert.ToDateTime(dataRowView.Row["DATAFECHAMENTO"]).Date >= DateTime.Now.Date)
                    {
                        if (e.ButtonID == "btnSolicitacaoReabertura")
                            e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                        if (e.ButtonID == "btnTranscricaoResposta")
                            e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.Default;
                        if (e.ButtonID == "btnVisualizarTranscricao")
                            e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                    }
                }
            }
        }

        protected void grdEtapaTurma_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdEtapaTurma.Settings.ShowFilterRow = false;
        }

        protected void grdEtapaTurma_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdEtapaTurma.Settings.ShowFilterRow = false;
        }

        protected void grdEtapaTurma_CommandButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex == -1)
                return;
            
            DataRow dataRow = grdEtapaTurma.GetDataRow(e.VisibleIndex);

            if (e.ButtonID == "btnSolicitacaoReabertura" && dataRow["REABERTURATURMAID"] != null)
            {
                e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
            }
        }

        protected void grdEtapaTurma_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            Dictionary<string, string> qryPars = new Dictionary<string, string>();
            var etapaId = grdEtapaTurma.GetRowValues(e.VisibleIndex, "ETAPAID");
            var transcricaoId = grdEtapaTurma.GetRowValues(e.VisibleIndex, "TRANSCRICAOTURMAID");
            var unidade_ens = tseUnidadeResponsavel.Value;
            var turma = grdEtapaTurma.GetRowValues(e.VisibleIndex, "TURMA");
            var ano = grdEtapaTurma.GetRowValues(e.VisibleIndex, "ANO");
            var semestre = ddlPeriodo.SelectedValue;
            var provaId = ddlProva.SelectedValue; 
            var prova = ddlProva.SelectedItem.Text;
            var avaliacao = Convert.ToString(tseAvaliacao["DESCRICAO"]);
            var avaliacaoId = tseAvaliacao.Value;
           
            qryPars = new Dictionary<string, string>();
            qryPars.Add("etapaId", etapaId.ToString());
            qryPars.Add("unidade_ens", unidade_ens.ToString());
            qryPars.Add("turma", turma.ToString());
            qryPars.Add("transcricaoId", transcricaoId.ToString());
            qryPars.Add("ano", ano.ToString());
            qryPars.Add("semestre", semestre.ToString());
            qryPars.Add("provaId", provaId.ToString());
            qryPars.Add("prova", prova.ToString());
            qryPars.Add("avaliacao", avaliacao.ToString());
            qryPars.Add("avaliacaoId", avaliacaoId.ToString());            

            string queryString = TPage.CodificaQueryString(qryPars);

            switch (e.ButtonID)
            {
                case "btnSolicitacaoReabertura":
                    for (int i = 3; i <= 14; i++)
                        grdEtapaTurma.Columns[i].Visible = false;
                    grdEtapaTurma.Columns[15].Visible = true;
                    grdEtapaTurma.StartEdit(e.VisibleIndex);
                    break;

                case "btnTranscricaoResposta":
                case "btnVisualizarTranscricao":
                    Response.RedirectLocation = string.Format("Transcricao.aspx?{0}", queryString);
                    break;

                default:
                    break;
            }
        }

        protected void grdEtapaTurma_CancelRowEditing(object sender, EventArgs e)
        {
            for (int i = 14; i >= 3; i--)
                grdEtapaTurma.Columns[i].Visible = true;
            grdEtapaTurma.Columns[15].Visible = false;
        }

        protected void grdEtapaTurma_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.AvaliacaoExterna.Entidades.ReaberturaTurma reaberturaTurma = new RN.AvaliacaoExterna.Entidades.ReaberturaTurma();
            GridViewDataColumn gridViewDataColumn = grdEtapaTurma.Columns["JUSTIFICATIVA"] as GridViewDataColumn;
            ASPxTextBox txtJustificativa = grdEtapaTurma.FindEditRowCellTemplateControl(gridViewDataColumn, "txtJustificativa") as ASPxTextBox;
            DataRow dataRow = grdEtapaTurma.GetDataRow(grdEtapaTurma.EditingRowVisibleIndex);

            int etapaId = Convert.ToInt32(dataRow["ETAPAID"]);
            string turmaId = dataRow["TURMA"].ToString();
            string solicitante = User.Identity.Name;
            string justificativa = txtJustificativa.Text;
            int ano = Convert.ToString(tseAvaliacao["ANO"]).IsNullOrEmptyOrWhiteSpace() ? 0 : Convert.ToInt32(tseAvaliacao["ANO"]);
            int semestre = Convert.ToInt32(ddlPeriodo.SelectedValue);

            //Monta Entidade
            reaberturaTurma.EtapaId = etapaId;
            reaberturaTurma.Turma = turmaId;
            reaberturaTurma.Ano = ano; 
            reaberturaTurma.Semestre = semestre;
            reaberturaTurma.SolicitanteId = solicitante;
            reaberturaTurma.Justificativa = justificativa;
            reaberturaTurma.UsuarioID = solicitante;

            validacao = rnReaberturaTurma.ValidaSolicitacao(reaberturaTurma);

            if (validacao.Valido)
            {
                rnReaberturaTurma.Solicita(reaberturaTurma);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdEtapaTurma_CancelRowEditing(sender, e);
            grdEtapaTurma.DataBind();
        }

        protected void grdEtapaTurma_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.FieldName == "STATUSTRANSCRICAO")
            {
                String statusTranscricao = Convert.ToString(e.CellValue).Trim().ToUpper();

                switch (statusTranscricao)
                {
                    case "SEM TRANSCRIÇÃO":
                        e.Cell.ForeColor = System.Drawing.Color.Red;
                        break;

                    case "INICIADA":
                        e.Cell.ForeColor = System.Drawing.Color.Blue;
                        break;

                    case "FINALIZADA":
                        e.Cell.ForeColor = System.Drawing.Color.Green;
                        break;
                }
            }
        }
    }
}