using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Entidades;
using Techne.Controls;
using System.Data;
using DevExpress.Web.ASPxGridView;
using Seeduc.Infra.Helpers;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/DisciplinasOptativasConfirmacao.aspx"), ControlText("Disciplinas Optativas do Registro de Confirmação"), Title("Disciplinas Optativas do Registro de Confirmação")]

    public partial class DisciplinasOptativasConfirmacao : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    CarregaAno();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdConfirmacao, "Confirmação de Matrícula");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdConfirmacao);
        }

        public void Update(object ANO, object PERIODO, object UNIDADE_ENSINO, object MOD_SEG_CURSO, object SERIE, object TURNO, object DT_SUGERIDA_FORMATADA, object ENSINO_RELIGIOSO, object LINGUA_ESTRANGEIRA_FACULTATIVA, object DT_ALTERACAO_FORMATADA, object STATUS, object ID_CONFIRMACAO_MATRICULA) { }

        public object Lista(object aluno, object ano)
        {
            RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();

            if (!Convert.ToString(aluno).IsNullOrEmptyOrWhiteSpace() && !Convert.ToString(ano).IsNullOrEmptyOrWhiteSpace())
            {
                return rnConfirmacaoMatricula.ListaPossiveisConfirmacaoMatriculaPor(Convert.ToString(aluno), Convert.ToInt32(ano));
            }

            return null;
        }

        protected void grdConfirmacao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdConfirmacao.Settings.ShowFilterRow = false;
        }

        protected void grdConfirmacao_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdConfirmacao.IsEditing)
            {
                if ((e.Column.FieldName) == "ID_CONFIRMACAO_MATRICULA")
                {
                    e.Editor.Enabled = false;
                }
                if ((e.Column.FieldName) == "ALUNO")
                {
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "ANO")
                {
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "PERIODO")
                {
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "UNIDADE_ENSINO")
                {
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "MOD_SEG_CURSO")
                {
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "SERIE")
                {
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "TURNO")
                {
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "DT_SUGERIDA")
                {
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "DT_ALTERACAO")
                {
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "DT_SUGERIDA")
                {
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "ENSINO_RELIGIOSO")
                {
                    var pode = this.grdConfirmacao.GetRowValuesByKeyValue(e.KeyValue, "PODE_ENSINO_RELIGIOSO");

                    if (Convert.ToString(pode) == "S")
                    {
                        e.Editor.ReadOnly = false;
                    }
                    else
                    {
                        e.Editor.ReadOnly = true;
                    }
                }

                if ((e.Column.FieldName) == "LINGUA_ESTRANGEIRA_FACULTATIVA")
                {
                    var pode = this.grdConfirmacao.GetRowValuesByKeyValue(e.KeyValue, "PODE_LINGUA_ESTRANGEIRA");

                    if (Convert.ToString(pode) == "S")
                    {
                        e.Editor.ReadOnly = false;
                    }
                    else
                    {
                        e.Editor.ReadOnly = true;
                    }
                }
            }
        }

        protected void grdConfirmacao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();

                int idConfirmacaoMatricula = Convert.ToInt32(e.Keys["ID_CONFIRMACAO_MATRICULA"]);
                bool ensinoReligioso = (e.NewValues["ENSINO_RELIGIOSO"] == null || Convert.ToBoolean(e.NewValues["ENSINO_RELIGIOSO"]) == false) ? false : true; ;
                bool linguaEstrangeiraFacultativa = (e.NewValues["LINGUA_ESTRANGEIRA_FACULTATIVA"] == null || Convert.ToBoolean(e.NewValues["LINGUA_ESTRANGEIRA_FACULTATIVA"]) == false) ? false : true; ;
                string usuario = User.Identity.Name;

                validacao = rnConfirmacaoMatricula.ValidaDisciplinasOptativas(idConfirmacaoMatricula, ensinoReligioso, linguaEstrangeiraFacultativa, usuario);

                if (validacao.Valido)
                {
                    rnConfirmacaoMatricula.AtualizaDisciplinasOptativas(idConfirmacaoMatricula, ensinoReligioso, linguaEstrangeiraFacultativa, usuario);
                    grdConfirmacao.DataBind();
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

        private void CarregaAno()
        {
            ListItem item = new ListItem("Selecione", string.Empty);

            ddlAno.Items.Clear();
            ddlAno.DataSource = RN.Util.Cache.CarregaResultadoQueryPor(RN.Util.Cache.ProximosAnosLetivos, RN.PeriodoLetivo.QueryListaProximosAnos);
            ddlAno.DataBind();
            ddlAno.Items.Insert(0, item);
        }

        private void LimparTela()
        {
            ddlAno.ClearSelection();
            grdConfirmacao.DataSource = null;
            grdConfirmacao.DataBind();
        }

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            try
            {
                RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();

                if (Page.IsCallback)
                {
                    return;
                }

                LimparTela();

                if (!tseAluno.DBValue.IsNull)
                {
                    if (tseAluno.IsValidDBValue)
                    {
                        lblMensagem.Text = string.Empty;
                    }
                    else
                    {
                        lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected bool VerificarCheck(object valor)
        {
            if (valor is DBNull)
            {
                return false;
            }

            if (valor is string)
            {
                return (string)valor == "1";
            }

            if (valor is bool)
            {
                return (bool)valor;
            }
            return false;
        }
    }
}
