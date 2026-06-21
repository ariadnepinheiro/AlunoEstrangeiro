using System;
using System.Web;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using Techne.Lyceum.RN.Entidades;
using Techne.Web;
using Techne.Lyceum.RN;

namespace Techne.Lyceum.Net.Basico
{
    [
    NavUrl("~/Basico/AgendaConfTurnosVagas.aspx"),
    ControlText("AgendaConfTurnosVagas"),
    Title("Agenda de Confirmação de Turnos e Vagas"),
    ]
    public partial class AgendaConfTurnosVagas : TPage
    {
        public object Listar()
        {
            return RN.CtvAgendaConfTurnoVaga.Listar();
        }
        
        public object ListarCurso()
        {
            if (Session["perfil"].ToString() == "privilegiado")
            {
                return RN.Curso.ConsultarDetalhesCurso();
            }

            if (Session["perfil"].ToString() == "1")
            {
                return RN.Curso.ConsultarSUPED();
            }

            if (Session["perfil"].ToString() == "2")
            {
                return RN.Curso.ConsultarSUPLAN();
            }

            return null;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdAgenda, "Agenda de Confirmação de Turnos e Vagas");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;

            if (!IsPostBack)
            {
                var dtPerfil = Perfil.ListarPerfil(User.Identity.Name);

                if (RN.Usuarios.UsuarioPrivilegiado(HttpContext.Current.User.Identity.Name))
                {
                    dtPerfil.Rows.Add(string.Empty, string.Empty, "privilegiado", 0);
                }

                if (dtPerfil.Select("id_perfil =" + 0).Length > 0)
                {
                    Session["perfil"] = "privilegiado";
                }
                else if (dtPerfil.Select("id_perfil =" + 2).Length > 0) 
                {
                    //SUPLAN
                    Session["perfil"] = "2";
                }
                else if (dtPerfil.Select("id_perfil =" + 1).Length > 0) 
                {
                    //SUPED
                    Session["perfil"] = "1";
                }
                else
                {
                    Session["perfil"] = string.Empty;
                }
            }
        }

        public void grdAgenda_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data
                || !this.grdAgenda.Visible
                || this.grdAgenda.VisibleRowCount == 0)
            {
                return;
            }
            var perfilResp = Convert.ToString(this.grdAgenda.GetRowValues(e.VisibleIndex, "PERFIL_RESPONSAVEL"));

            if (Session["perfil"].ToString() == "1")
            {
                //SUPED
                if (perfilResp != "SUPED")
                {
                    e.Row.Enabled = false;
                }
            }
            else if (Session["perfil"].ToString() == "2") 
            {
                //SUPLAN
                if (perfilResp != "SUPLAN")
                {
                    e.Row.Enabled = false;
                }
            }
            else
            {
                e.Row.Enabled = false;
            }
        }
        protected void grdAgenda_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            var perfilResp = Convert.ToString(this.grdAgenda.GetRowValues(e.VisibleIndex, "PERFIL_RESPONSAVEL"));

            if (Session["perfil"].ToString() == "1") 
            {
                if (perfilResp != "SUPED" && !string.IsNullOrEmpty(perfilResp))
                {
                    //SUPED
                    e.Visible = false;
                }
            }
            else if (Session["perfil"].ToString() == "2") 
            {
                if (perfilResp != "SUPLAN" && !string.IsNullOrEmpty(perfilResp))
                {
                    //SUPLAN
                    e.Visible = false;
                }
            }
            else if (Session["perfil"].ToString() != "privilegiado") 
            {
                e.Visible = false;
            }
        }

        protected void grdAgenda_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdAgenda);
        }

        protected void grdAgenda_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdAgenda.Settings.ShowFilterRow = false;
        }

        protected void grdAgenda_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdAgenda.Settings.ShowFilterRow = false;
        }

        protected void grdAgenda_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdAgenda.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "ID_AGENDA_CONF_TURNO_VAGA")
                {
                    e.Editor.Enabled = true;
                }
            }
            else if (grdAgenda.IsEditing)
            {
                if ((e.Column.FieldName) == "ID_AGENDA_CONF_TURNO_VAGA")
                {
                    e.Editor.Enabled = false;
                }
                if ((e.Column.FieldName) == "anoperiodo")
                {
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "CURSO")
                {
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "NOME")
                {
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "SERIE")
                {
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "anoperiodoreferencia")
                {
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "SITUACAO")
                {
                    e.Editor.ReadOnly = true;
                }
            }

            if (e.Column.FieldName == "SERIE")
            {
                var combo = e.Editor as ASPxComboBox;

                if (combo == null)
                {
                    return;
                }

                combo.Callback += this.cmbCurso_OnCallback;
            }

            if (this.grdAgenda.IsEditing
                && e.Column.FieldName == "SERIE"
                && e.KeyValue != DBNull.Value
                && e.KeyValue != null)
            {
                var val = this.grdAgenda.GetRowValuesByKeyValue(e.KeyValue, "CURSO");

                if (val == DBNull.Value)
                {
                    return;
                }

                var curso = (string)val;
                var combo = e.Editor as ASPxComboBox;

                if (combo == null)
                {
                    return;
                }

                this.CarregarSeries(combo, curso);
            }

            if (e.Column.FieldName == "anoperiodoreferencia")
            {
                var combo = e.Editor as ASPxComboBox;

                if (combo == null)
                {
                    return;
                }

                combo.Callback += this.cmbAnoPeriodo_OnCallback;
            }

            if (this.grdAgenda.IsEditing
                && e.Column.FieldName == "anoperiodoreferencia"
                && e.KeyValue != DBNull.Value
                && e.KeyValue != null)
            {
                var val = this.grdAgenda.GetRowValuesByKeyValue(e.KeyValue, "anoperiodo");

                if (val == DBNull.Value)
                {
                    return;
                }

                var anoperiodo = (string)val;
                var combo = e.Editor as ASPxComboBox;

                if (combo == null)
                {
                    return;
                }

                this.CarregarAnoPeriodoReferencia(combo, anoperiodo);
            }
        }

        private void CarregarSeries(ASPxComboBox cmbSerie, string curso)
        {
            if (string.IsNullOrEmpty(curso))
            {
                return;
            }

            cmbSerie.Items.Clear();
            cmbSerie.TextField = "SERIE";
            cmbSerie.ValueField = "SERIE";
            cmbSerie.DataSource = Serie.ListarSeries(curso);
            cmbSerie.DataBind();
        }
        private void CarregarAnoPeriodoReferencia(ASPxComboBox cmbAnoPeriodoReferencia, string anoperiodo)
        {
            if (string.IsNullOrEmpty(anoperiodo))
            {
                return;
            }
            
            string[] Anoreferencia = anoperiodo.ToString().Split('-');
            cmbAnoPeriodoReferencia.Items.Clear();
            cmbAnoPeriodoReferencia.TextField = "anoperiodo";
            cmbAnoPeriodoReferencia.ValueField = "anoperiodo";
            cmbAnoPeriodoReferencia.DataSource = RN.PeriodoLetivo.ConsultarAnoPeriodo();
            cmbAnoPeriodoReferencia.DataBind();
        }
        private void cmbCurso_OnCallback(object source, CallbackEventArgsBase e)
        {
            this.CarregarSeries(source as ASPxComboBox, e.Parameter);
        }
        private void cmbAnoPeriodo_OnCallback(object source, CallbackEventArgsBase e)
        {
            this.CarregarAnoPeriodoReferencia(source as ASPxComboBox, e.Parameter);
        }

        public void Insert(object anoperiodo,object modalidade,object nivel, object CURSO,object NOME, object SERIE, object anoperiodoreferencia, object DT_INICIO_CONF_TURNO, object DT_FIM_CONF_TURNO, object DT_INICIO_CONF_VAGAS, object DT_FIM_CONF_VAGAS,object SITUACAO)
        {
        }
        
        public void Update(object anoperiodo, object modalidade, object nivel, object CURSO, object NOME, object SERIE, object anoperiodoreferencia, object DT_INICIO_CONF_TURNO, object DT_FIM_CONF_TURNO, object DT_INICIO_CONF_VAGAS, object DT_FIM_CONF_VAGAS,object SITUACAO, object ID_AGENDA_CONF_TURNO_VAGA)
        {
        }
        
        public void Delete(object ID_AGENDA_CONF_TURNO_VAGA)
        {
        }

        protected void odsAgenda_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var id = e.InputParameters["ID_AGENDA_CONF_TURNO_VAGA"].ToString();

            var validacao = RN.CtvAgendaConfTurnoVaga.ValidarRemover(int.Parse(id));

            if (validacao.Valido)
            {
                RN.CtvAgendaConfTurnoVaga.Remover(int.Parse(id));
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void odsAgenda_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            string[] anoperiodo = e.InputParameters["anoperiodo"].ToString().Split('-');
            string[] anoperiodoreferencia = e.InputParameters["anoperiodoreferencia"].ToString().Split('-');

            var agenda = new TceCtvAgendaConfTurnoVaga
            {
                IdAgendaConfTurnoVaga = int.Parse(e.InputParameters["ID_AGENDA_CONF_TURNO_VAGA"].ToString()),
                Ano = Convert.ToInt32(anoperiodo[0].Trim()),
                Periodo = Convert.ToInt32(anoperiodo[1].Trim()),
                Curso = e.InputParameters["CURSO"].ToString(),
                Serie = Convert.ToInt32(e.InputParameters["SERIE"].ToString()),
                AnoReferencia = Convert.ToInt32(anoperiodoreferencia[0].Trim()),
                PeriodoReferencia = Convert.ToInt32(anoperiodoreferencia[1].Trim()),
                DtInicioConfTurno = Convert.ToDateTime(e.InputParameters["DT_INICIO_CONF_TURNO"].ToString()),
                DtFimConfTurno = Convert.ToDateTime(e.InputParameters["DT_FIM_CONF_TURNO"].ToString()),
                DtInicioConfVagas = Convert.ToDateTime(e.InputParameters["DT_INICIO_CONF_VAGAS"].ToString()),
                DtFimConfVagas = Convert.ToDateTime(e.InputParameters["DT_FIM_CONF_VAGAS"].ToString()),
                Matricula = this.User.Identity.Name
            };

            var validacao = RN.CtvAgendaConfTurnoVaga.Validar(agenda);

            if (validacao.Valido)
            {
                RN.CtvAgendaConfTurnoVaga.Alterar(agenda);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }
        protected void odsAgenda_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {            
            string[] anoperiodo = e.InputParameters["anoperiodo"].ToString().Split('-');
            string[] anoperiodoreferencia = e.InputParameters["anoperiodoreferencia"].ToString().Split('-');
            int agendaId = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["AgendaIdTurnosVagas"]);

            TceCtvAgendaConfTurnoVaga agenda = new TceCtvAgendaConfTurnoVaga
            {
                Ano = Convert.ToInt32(anoperiodo[0].Trim()),
                Periodo = Convert.ToInt32(anoperiodo[1].Trim()),
                Curso = e.InputParameters["CURSO"].ToString(),
                Serie = Convert.ToInt32(e.InputParameters["SERIE"].ToString()),
                AnoReferencia = Convert.ToInt32(anoperiodoreferencia[0].Trim()),
                PeriodoReferencia = Convert.ToInt32(anoperiodoreferencia[1].Trim()),
                DtInicioConfTurno = Convert.ToDateTime(e.InputParameters["DT_INICIO_CONF_TURNO"].ToString()),
                DtFimConfTurno = Convert.ToDateTime(e.InputParameters["DT_FIM_CONF_TURNO"].ToString()),
                DtInicioConfVagas = Convert.ToDateTime(e.InputParameters["DT_INICIO_CONF_VAGAS"].ToString()),
                DtFimConfVagas = Convert.ToDateTime(e.InputParameters["DT_FIM_CONF_VAGAS"].ToString()),
                AgendaId = agendaId,
                Matricula = this.User.Identity.Name
            };

            var validacao = RN.CtvAgendaConfTurnoVaga.Validar(agenda);

            if (validacao.Valido)
            {
                RN.CtvAgendaConfTurnoVaga.Inserir(agenda);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }
    }
}
