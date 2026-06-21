using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using System.Data;

namespace Techne.Lyceum.Net.Curriculo
{
    using Techne.Lyceum.RN.Entidades;
    using DevExpress.Web.ASPxEditors;
    using DevExpress.Web.ASPxClasses;
    using DevExpress.Web.ASPxTabControl;
    [
        NavUrl("~/Curriculo/ReposicaoAulas.aspx"),
        ControlText("ReposicaoAulas"),
        Title("Reposição de Aulas"),
    ]

    public partial class ReposicaoAulas : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;
            if (!IsPostBack)
            {
                ddlAno.Items.Clear();
                ddlAno.DataSource = RN.SubperiodoLetivo.ListarAnos();
                ddlAno.DataBind();

                ddlPeriodo.Items.Clear();
                ddlPeriodo.DataSource = RN.PeriodoLetivo.ListarPeriodo(ddlAno.SelectedValue);
                ddlPeriodo.Items.Insert(0, "Selecione");
                ddlPeriodo.DataBind();

            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdReposicao, "Reposição de Aulas");

        }
        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdReposicao);

        }

        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlPeriodo.Items.Clear();

            if (ddlAno.SelectedValue != "Selecione")
            {
                ddlPeriodo.DataSource = RN.PeriodoLetivo.ListarPeriodo(ddlAno.SelectedValue);
                ddlPeriodo.Items.Insert(0, "Selecione");
                ddlPeriodo.DataBind();
            }
        }


        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {

            PreencherTurma();
        }

        protected void tseCoordenadoria_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            //ignora callbacks causados controles
            if (Page.IsCallback)
                return;


            //CarregarGrid();
            RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
            if (sessao != null)
            {
                if (!tseCoordenadoria.DBValue.IsNull)
                {
                    if (tseCoordenadoria.IsValidDBValue)
                    {
                        sessao.Coordenadoria = Convert.ToString(tseCoordenadoria.DBValue);
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;

                        tseUnidadeResponsavel.ResetValue();
                    }
                    else
                    {
                        sessao.Coordenadoria = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                    }
                }
                else
                {
                    sessao.Coordenadoria = string.Empty;
                    sessao.Municipio = string.Empty;
                    sessao.Escola = string.Empty;
                }
            }
        }


        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            //ignora callbacks causados controles
            if (Page.IsCallback)
                return;
            RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
            if (sessao != null)
            {
                if (!tseMunicipio.DBValue.IsNull)
                {
                    if (tseMunicipio.IsValidDBValue)
                    {
                        sessao.Municipio = Convert.ToString(tseMunicipio.DBValue);

                        sessao.Escola = string.Empty;
                        tseUnidadeResponsavel.ResetValue();
                    }
                    else
                    {
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                    }
                }
                else
                {
                    sessao.Municipio = string.Empty;
                    sessao.Escola = string.Empty;
                }
            }
        }


        protected void tseUnidadeResponsavel_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            //ignora callbacks causados controles
            if (Page.IsCallback)
                return;

            RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
            ddlTurma.Items.Clear();
            if (!tseUnidadeResponsavel.DBValue.IsNull)
            {
                if (tseUnidadeResponsavel.IsValidDBValue)
                {
                    if (!tseUnidadeResponsavel["unidade_ens"].IsNull)
                    {
                        tseCoordenadoria.Value = tseUnidadeResponsavel["nucleo"];
                        tseMunicipio.Value = tseUnidadeResponsavel["municipio"];
                    }

                    PreencherTurma();
                    if (sessao != null)
                    {
                        sessao.Escola = Convert.ToString(tseUnidadeResponsavel.DBValue);
                        sessao.Coordenadoria = Convert.ToString(tseCoordenadoria.DBValue);
                        sessao.Municipio = Convert.ToString(tseMunicipio.DBValue);
                    }
                }
                else
                {
                    if (sessao != null)
                    {
                        sessao.Escola = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Coordenadoria = string.Empty;
                    }
                }
            }
            else
            {
                if (sessao != null)
                {
                    sessao.Escola = string.Empty;
                    sessao.Municipio = string.Empty;
                    sessao.Coordenadoria = string.Empty;

                }
            }

        }

        protected void ddlTurma_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (Validar())
            {
                //grdReposicao.DataSource = RN.AulaDocenteGreve.Listar(ddlTurma.SelectedValue, int.Parse(ddlAno.SelectedValue), int.Parse(ddlPeriodo.SelectedValue), tseUnidadeResponsavel.Value.ToString(), hdDataInicio.Value, hdDataFim.Value);
                //grdReposicao.DataBind();

            }
        }

        private bool Validar()
        {
            if (ddlAno.SelectedValue == "Selecione")
            {
                lblMensagem.Text = "Campo Ano é de preenchimento obrigatório";
                ddlAno.Focus();
                return false;

            }
            if (string.IsNullOrEmpty(ddlPeriodo.SelectedValue))
            {
                lblMensagem.Text = "Campo Período é de preenchimento obrigatório";
                ddlAno.Focus();
                return false;
            }
            if (ddlPeriodo.SelectedValue == "Selecione")
            {
                lblMensagem.Text = "Campo Período é de preenchimento obrigatório";
                ddlAno.Focus();
                return false;
            }
            if (tseCoordenadoria.DBValue.IsNull)
            {
                lblMensagem.Text = "Campo Coordenadoria é de preenchimento obrigatório";
                tseCoordenadoria.Focus();
                return false;
            }

            if (tseMunicipio.DBValue.IsNull)
            {
                lblMensagem.Text = "Campo Município é de preenchimento obrigatório";
                tseMunicipio.Focus();
                return false;
            }
            if (tseUnidadeResponsavel.DBValue.IsNull)
            {
                lblMensagem.Text = "Campo Unidade de Ensino é de preenchimento obrigatório";
                tseUnidadeResponsavel.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(ddlTurma.SelectedValue))
            {
                lblMensagem.Text = "Campo Turma é de preenchimento obrigatório";
                ddlTurma.Focus();
                return false;
            }
            if (ddlTurma.SelectedValue == "Selecione")
            {
                lblMensagem.Text = "Campo Turma é de preenchimento obrigatório";
                ddlTurma.Focus();
                return false;

            }
            return true;
        }

        private void PreencherTurma()
        {
            ddlTurma.Items.Clear();

            if (ddlAno.SelectedValue != "Selecione" && ddlPeriodo.SelectedValue != "Selecione" && !tseUnidadeResponsavel.DBValue.IsNull)
            {
                ddlTurma.DataSource = RN.GradeSerie.ConsultarTurmasGreve(ddlAno.SelectedValue, ddlPeriodo.SelectedValue, tseUnidadeResponsavel.Value.ToString(), hdDataInicio.Value, hdDataFim.Value);
                ddlTurma.Items.Insert(0, "Selecione");
                ddlTurma.DataBind();
            }
        }




        protected void grdReposicao_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (grdReposicao.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "MATRICULA")
                    e.Editor.ReadOnly = false;
                if ((e.Column.FieldName) == "IDVINCULO")
                    e.Editor.ReadOnly = false;
                if ((e.Column.FieldName) == "NOME_COMPL")
                    e.Editor.ReadOnly = false;
                if ((e.Column.FieldName) == "HORARIO")
                    e.Editor.ReadOnly = false;
                if ((e.Column.FieldName) == "DIA")
                    e.Editor.ReadOnly = false;
                if ((e.Column.FieldName) == "DISCIPLINA")
                    e.Editor.ReadOnly = false;
                if ((e.Column.FieldName) == "TIPO")
                    e.Editor.ReadOnly = false;
                if ((e.Column.FieldName) == "NOME_SUBSTITUTO")
                    e.Editor.ReadOnly = false;
                if ((e.Column.FieldName) == "TIPO_DISCIPLINA")
                    e.Editor.ReadOnly = false;
                if ((e.Column.FieldName) == "NOME_DISCIPLINA")
                    e.Editor.ReadOnly = false;
                if ((e.Column.FieldName) == "MATRICULA_SUBSTITUTO")
                    e.Editor.ReadOnly = false;

            }
            else if (grdReposicao.IsEditing)
            {
                if ((e.Column.FieldName) == "MATRICULA")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "IDVINCULO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "NOME_COMPL")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "HORARIO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "DIA")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "DISCIPLINA")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "TIPO_DISCIPLINA")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "NOME_DISCIPLINA")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "TIPO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "NOME_SUBSTITUTO")
                    e.Editor.ReadOnly = true;
                if ((e.Column.FieldName) == "MATRICULA_SUBSTITUTO")
                    e.Editor.ReadOnly = true;

            }
        }
        protected void grdReposicao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdReposicao);

        }
        protected void grdReposicao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdReposicao.Settings.ShowFilterRow = false;

        }
        protected void grdReposicao_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                var num_func = Convert.ToString(e.GetListSourceFieldValue("NUM_FUNC"));
                var turno = Convert.ToString(e.GetListSourceFieldValue("TURNO"));
                var faculdade = Convert.ToString(e.GetListSourceFieldValue("FACULDADE"));
                var dia_semana = Convert.ToString(e.GetListSourceFieldValue("DIA_SEMANA"));
                var aula = Convert.ToString(e.GetListSourceFieldValue("AULA"));
                var disciplina = Convert.ToString(e.GetListSourceFieldValue("DISCIPLINA"));
                var turma = Convert.ToString(e.GetListSourceFieldValue("TURMA"));
                var ano = Convert.ToString(e.GetListSourceFieldValue("ANO"));
                var semestre = Convert.ToString(e.GetListSourceFieldValue("SEMESTRE"));
                var data_inicio = Convert.ToString(e.GetListSourceFieldValue("DATA_INICIO"));
                var codigo = string.IsNullOrEmpty(e.GetListSourceFieldValue("CODIGO").ToString()) ? "0" : Convert.ToString(e.GetListSourceFieldValue("CODIGO"));
                var tipo_disciplina = Convert.ToString(e.GetListSourceFieldValue("TIPO_DISCIPLINA"));
                e.Value = num_func + ";" + turno + ";" + faculdade + ";" + dia_semana + ";" + aula + ";" + disciplina + ";" + turma + ";" + ano + ";" + semestre + ";" + data_inicio + ";" + codigo + ";" + tipo_disciplina;
            }
        }
        protected void grdReposicao_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();

           
            try
            {
                DateTime datainicio = Convert.ToDateTime(e.NewValues["DATA_INICIO_SUBSTITUICAO"]);
                DateTime datafim = Convert.ToDateTime(e.NewValues["DATA_FIM_SUBSTITUICAO"]);

                if (datainicio < Convert.ToDateTime(hdDataInicio.Value))
                {
                    e.RowError = "Data de início da reposição não pode ser menor que " + hdDataInicio.Value;
                    return;
                }


                if (datafim > Convert.ToDateTime(hdDataFim.Value))
                {
                    e.RowError = "Data de Fim da reposição não pode ser maior que " + hdDataFim.Value;
                    return;
                }

                if (datainicio > datafim)
                {
                    e.RowError = "Data de Início da reposição não pode ser maior que a Data de Fim.";
                    return;
                }

                var id = e.NewValues["IDVINCULO2"].ToString();
                               
                DataTable dt = RN.Docentes.VerificaDocenteAtivo(id);

                if (dt.Rows.Count > 0)
                {
                    var funcao = dt.Rows[0]["FUNCAO"].ToString();

                    if (funcao != "106" && funcao != "107" && funcao != "108" && funcao != "109" && funcao != "10001")
                    {
                        e.RowError = "Matrícula sem função de regente.";
                        return;
                    }

                    string[] chaves = e.Keys["CompositeKey"].ToString().Split(';');
                    decimal? valor;
                    connection.Open(true);
                    if (!RN.DocenteGLP.ExisteSaldoValor(connection, Convert.ToDecimal(chaves[7]), Convert.ToDecimal(DateTime.Now.Month), Convert.ToDecimal(1), funcao, chaves[11], out valor))
                    {
                        e.RowError = "Reposição não pode ser aceita pois não existe saldo disponível.";
                        return;
                    }
                    hdValorGLP.Value = valor.Value.ToString();
                }
                else
                {
                    e.RowError = "Matrícula inválida ou sem lotação ativa.";
                
                }
            }
            catch (Exception)
            {
                connection.Rollback();
                
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        protected void grdReposicao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                RN.Docentes rnDocentes = new Docentes();

                string[] chaves = e.Keys["CompositeKey"].ToString().Split(';');

                var id = rnDocentes.ObtemMatriculaPor(e.NewValues["IDVINCULO2"].ToString());

                if (!id.IsNullOrEmptyOrWhiteSpace())
                {

                    var TADG = new TceAulaDocenteGreve
                    {
                        NumFunc = int.Parse(chaves[0]),
                        Turno = chaves[1],
                        Faculdade = chaves[2],
                        DiaSemana = int.Parse(chaves[3]),
                        Aula = int.Parse(chaves[4]),
                        Disciplina = chaves[5],
                        Turma = chaves[6],
                        Ano = int.Parse(chaves[7]),
                        Semestre = int.Parse(chaves[8]),
                        DataInicio = Convert.ToDateTime(chaves[9]),
                        DataInicioSubstituicao = Convert.ToDateTime(e.NewValues["DATA_INICIO_SUBSTITUICAO"].ToString()),
                        DataFimSubstituicao = Convert.ToDateTime(e.NewValues["DATA_FIM_SUBSTITUICAO"].ToString()),
                        MatriculaSubstituto = id,
                        Matricula = User.Identity.Name,
                        Tipo = e.NewValues["TIPO"].ToString(),
                        Codigo = int.Parse(chaves[10]),
                        ValorGLP = hdValorGLP.Value
                    };

                    RN.AulaDocenteGreve.Salvar(TADG);
                }
                else
                {
                    throw new Exception("ID/Vínculo substituto não encontrado.");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object Listar(object turma, object ano, object periodo, DbObject unidade, object dtinicio, object dtfim)
        {
            if (turma != null)
            {
                if (turma.ToString() != "Selecione" && periodo.ToString() != "Selecione")
                    return RN.AulaDocenteGreve.Listar(turma.ToString(), int.Parse(ano.ToString()), int.Parse(periodo.ToString()), unidade.ToString(), dtinicio.ToString(), dtfim.ToString());
            }


            return null;
        }
        public void Delete(object CompositeKey)
        {
        }

        public void Update(object MATRICULA,object IDVINCULO, object NOME_COMPL, object HORARIO, object DIA, object NOME_DISCIPLINA, object TIPO, object NOME_SUBSTITUTO,object MATRICULA_SUBSTITUTO,object IDVINCULO2, object DATA_INICIO_SUBSTITUICAO, object DATA_FIM_SUBSTITUICAO, object CompositeKey)
        {
        }

        protected void grdReposicao_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                string[] chaves = e.Keys["CompositeKey"].ToString().Split(';');

                var codigo = int.Parse(chaves[10]);

                if (codigo != 0)
                    RN.AulaDocenteGreve.Remover(codigo, User.Identity.Name);
                else
                    throw new Exception("Não existe reposição para ser excluída.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }
}

