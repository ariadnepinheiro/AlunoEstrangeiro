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


namespace Techne.Lyceum.Net.Basico
{
    using Techne.Lyceum.RN.Entidades;
    using DevExpress.Web.ASPxEditors;
    using DevExpress.Web.Data;
    using DevExpress.Web.ASPxClasses;
    [
        NavUrl("~/Basico/CadastroSaladeAula.aspx"),
         ControlText("CadastroSaladeAula"),
         Title("Cadastro Salas de Aula"),
        ]
    public partial class CadastroSaladeAula : TPage
    {
        public object Listar(object unidade_ens)
        {
            var ue = unidade_ens.ToString();
            return Dependencia.ListarSalaDeAula(ue);
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdSaladeAula, "Sala de Aula");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                if (!IsPostBack)
                {
                    pnlGrid.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }
        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdSaladeAula);

        }
        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }
        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;
                pnlGrid.Visible = false;

                //CarregarGrid();
                RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
                if (sessao != null)
                {
                    if (!tseRegional.DBValue.IsNull)
                    {
                        if (tseRegional.IsValidDBValue)
                        {
                            sessao.Regional = Convert.ToString(tseRegional.DBValue);
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
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;

                pnlGrid.Visible = false;
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
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void tseUnidadeResponsavel_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;
                pnlGrid.Visible = false;
                RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (!tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (tseUnidadeResponsavel.IsValidDBValue)
                    {
                        pnlGrid.Visible = true;
                        if (!tseUnidadeResponsavel["unidade_ens"].IsNull)
                        {
                            tseRegional.Value = tseUnidadeResponsavel["id_regional"];
                            tseMunicipio.Value = tseUnidadeResponsavel["municipio"];
                        }

                        if (sessao != null)
                        {
                            sessao.Escola = Convert.ToString(tseUnidadeResponsavel.DBValue);
                            sessao.Regional = Convert.ToString(tseRegional.DBValue);
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
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        protected void grdSaladeAula_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                var dependencia = Convert.ToString(e.GetListSourceFieldValue("DEPENDENCIA"));
                var faculdade = Convert.ToString(e.GetListSourceFieldValue("FACULDADE"));

                e.Value = faculdade + ";" + dependencia;
            }
        }

        protected void grdSaladeAula_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs asPxGridViewAfterPerformCallbackEventArgs)
        {
            this.ControlaAcesso(this.grdSaladeAula);
        }

        protected void grdSaladeAula_RowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {

            var dep = Dependencia.Bind(e.Keys, e.Values);
            dep.Matricula = User.Identity.Name;
            var validacao = Dependencia.ValidarDesabilitarSalaDeAula(dep);
            string mensagem = string.Empty;

            if (!validacao.Valido)
            {
                throw new Exception(validacao.Mensagem.ToString());
            }
            else
            {
                Dependencia.DesabilitarSalaDeAula(dep);

                var dt = Dependencia.VerificaAlunoMatriculadoPorSala(dep.Faculdade, dep.Dependencia);

                if (dt.Rows.Count > 0)
                {
                    mensagem = string.Format("Sala desativada com sucesso. Importante ressaltar que a sala desativada estava vinculada à {0} turma(s).", dt.Rows.Count);
                    lblMensagem.Text = mensagem;
                    e.Cancel = true;
                    return;
                }
            }
        }

        protected void grdSaladeAula_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {

            var dep = Dependencia.Bind(null, e.NewValues);
            dep.Matricula = User.Identity.Name;
            dep.Faculdade = tseUnidadeResponsavel.DBValue.ToString();

            Dependencia.InserirSalaDeAula(dep);

            e.Cancel = true;
            this.grdSaladeAula.CancelEdit();
        }

        protected void grdSaladeAula_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            var dep = Dependencia.Bind(e.Keys, e.NewValues);
            dep.Matricula = User.Identity.Name;
            Dependencia.AlterarSalaDeAula(dep);

            e.Cancel = true;
            this.grdSaladeAula.CancelEdit();
        }

        protected void grdSaladeAula_RowValidating(object sender, ASPxDataValidationEventArgs e)
        {
            if (tseUnidadeResponsavel.DBValue.IsNull)
            {
                e.RowError = "O campo Unidade de Ensino é obrigatório.";
                return;
            }

            var dep = Dependencia.Bind(e.Keys, e.NewValues);
            dep.Faculdade = tseUnidadeResponsavel.DBValue.ToString();

            var validacao = Dependencia.ValidarSalaDeAula(dep);

            if (!validacao.Valido)
            {
                e.RowError = validacao.Mensagem;
            }
        }
        public void Delete(object CompositeKey)
        {
        }

        protected void btnSim_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Entidades.LyDependencia dep = new LyDependencia();

                string[] depend = hdnDependencia.Value.Split(';');

                dep.Dependencia = depend[1];
                dep.Faculdade = depend[0];
                dep.Matricula = User.Identity.Name;

                validacao = Dependencia.ValidarDesabilitarSalaDeAula(dep);

                if (validacao.Valido)
                {
                    Dependencia.DesabilitarSalaDeAula(dep);
                    grdSaladeAula.DataBind();
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    grdSaladeAula.CancelEdit();
                }

                this.pucConfirmarDesativacao.ShowOnPageLoad = false;
            }

            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnNao_Click(object sender, EventArgs e)
        {
            this.pucConfirmarDesativacao.ShowOnPageLoad = false;
            grdSaladeAula.CancelEdit();
        }

        protected void grdSaladeAula_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {                   

            if (e.ButtonID == "btnExcluir")
            {
                hdnDependencia.Value = Convert.ToString(grdSaladeAula.GetRowValues(e.VisibleIndex, "CompositeKey"));

                string[] dep = hdnDependencia.Value.Split(';');

                var dt = Dependencia.VerificaAlunoMatriculadoPorSala( dep[0],dep[1]);

                if (dt.Rows.Count > 0)
                {
                    lblTexto.Text = "Esta sala de aula possui " + dt.Rows.Count.ToString() + " turma(s) vinculada(s). Confirma a desativação da sala?";
                }
                else
                {
                    lblTexto.Text = "Confirma a desativação da sala?";
                }

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "abrirPopup();", true);
            }
        }

    }
}
