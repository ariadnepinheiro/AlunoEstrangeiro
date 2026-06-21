using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.DTOs;
using DevExpress.Web.ASPxGridView;
using DevExpress.Utils;
using Techne.Lyceum.RN.Util;
using System.Data;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/LiberacaoConfirmacaoMatricula.aspx"), ControlText("LiberacaoConfirmacaoMatricula"), Title("Liberação do Registro de Confirmação")]

    public partial class LiberacaoConfirmacaoMatricula : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                int tipoEventoAgenda;
                DataTable dt = new DataTable();
                RN.Perfil rnPerfil = new Perfil();

                if (!IsPostBack)
                {
                    if (!rnPerfil.PossuiPerfilLiberacaoRegistroConfirmacaoMatriculaPor(User.Identity.Name))
                    {
                        tipoEventoAgenda = Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.LiberacaoConfirmacaoMatricula);

                        dt = RN.Agenda.Agenda.ListaAgendaPorTipoEventoEDataEvento(tipoEventoAgenda, DateTime.Now);

                        if (dt.Rows.Count == 0)
                        {
                            lblMensagem.Text = "Não existe Liberação do Registro de Confirmação vigente na agenda de eventos";
                            pnBusca.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void Limpar()
        { 
            grdConfirmacao.DataSource = null;
            grdConfirmacao.Visible = false;
            Session["liberacao"] = null;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdConfirmacao, "Histórico de Confirmação/Renovação de Matrícula");
            TituloGrid(grdMatriculaTurmas, "Matrícula por Turma");
        }
      
        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                Limpar();
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
                if (!tseAluno.DBValue.IsNull)
                {
                    if (tseAluno.IsValidDBValue)
                    {
                        CarregaGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdConfirmacao_PageIndexChanged(object sender, EventArgs e)
        {
            CarregaGrid();
        }

        private void CarregaGrid()
        {
            try
            {
                RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
                if ((!tseAluno.DBValue.IsNull) && (tseAluno.IsValidDBValue))
                {
                    grdConfirmacao.DataSource = rnConfirmacaoMatricula.ListaConfirmacoesParaLiberacaoPor(tseAluno.DBValue.ToString());
                    grdConfirmacao.DataBind();

                    if (grdConfirmacao.VisibleRowCount > 0)
                    {
                        grdConfirmacao.Visible = true;
                    }
                    else
                    {
                        lblMensagem.Text = "Aluno sem confirmação de matricula para liberação.";
                        grdConfirmacao.Visible = false;
                    }
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

        protected void grdConfirmacao_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {

            if (e.VisibleIndex == -1) return;

            var status = (string)grdConfirmacao.GetRowValues(e.VisibleIndex, "STATUS");
            if (!string.IsNullOrEmpty(status)
                && status == "Pendente")
            {
                e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
            }
        }

        protected void grdConfirmacao_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            RN.DTOs.DadosLiberacaoConfirmacao liberacao = new RN.DTOs.DadosLiberacaoConfirmacao();
            RN.Matricula rnMatricula = new Techne.Lyceum.RN.Matricula();
            Session["liberacao"] = null; //Limpa sessão

            if (e.ButtonID == "btnLiberar" && (!tseAluno.DBValue.IsNull) && (tseAluno.IsValidDBValue))
            {
                try
                {
                    liberacao.Aluno = Convert.ToString(tseAluno.DBValue);
                    liberacao.IdConfirmacaoMatricula = Convert.ToInt32(grdConfirmacao.GetRowValues(e.VisibleIndex, "ID_CONFIRMACAO_MATRICULA"));
                    liberacao.Ano = Convert.ToInt32(grdConfirmacao.GetRowValues(e.VisibleIndex, "ANO"));
                    liberacao.Periodo = Convert.ToInt32(grdConfirmacao.GetRowValues(e.VisibleIndex, "PERIODO"));
                    liberacao.SituacaoAtual = Convert.ToString(grdConfirmacao.GetRowValues(e.VisibleIndex, "STATUS"));
                    liberacao.MatriculaResponsavel = User.Identity.Name;

                    //Carrega dados da liberação em sessao
                    Session["liberacao"] = liberacao;

                    //Carrega popup de confirmacao
                    pcMatriculaTurmas.ShowOnPageLoad = true;
                    grdMatriculaTurmas.DataSource = rnMatricula.ListaTurmasParaLiberacaoConfirmacaoPor(liberacao.Aluno, liberacao.Ano, liberacao.Periodo);
                    grdMatriculaTurmas.DataBind();

                    if (grdMatriculaTurmas.VisibleRowCount == 0)
                    {
                        btnConfirmarLiberacao_Click(null, null);
                    }
                    else
                    {
                        pcMatriculaTurmas.ShowOnPageLoad = true;
                    }
                }
                catch (Exception ex)
                {
                    lblMensagem.Text = ex.Message;
                }
            }
        }

        protected void btnConfirmarLiberacao_Click(object sender, EventArgs e)
        {
            RN.DTOs.DadosLiberacaoConfirmacao liberacao = new RN.DTOs.DadosLiberacaoConfirmacao();
            RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            ValidacaoDados validacao = new ValidacaoDados();
            pcMatriculaTurmas.ShowOnPageLoad = false;

            if (Session["liberacao"] != null)
            {
                liberacao = (DadosLiberacaoConfirmacao)Session["liberacao"];
                validacao = rnConfirmacaoMatricula.ValidaLiberacaoPor(liberacao);

                if (validacao.Valido)
                {
                    rnConfirmacaoMatricula.LiberaPor(liberacao);
                    CarregaGrid();
                    lblMensagem.Text = "Confirmação de matricula liberada com sucesso.";
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />"); ;
                }
            }
            else
            {
                lblMensagem.Text = "Confirmação de matricula não encontrada.";
            }
        }

        protected void btnCancelaLiberacao_Click(object sender, EventArgs e)
        {
            pcMatriculaTurmas.ShowOnPageLoad = false;
        }
    }
}
