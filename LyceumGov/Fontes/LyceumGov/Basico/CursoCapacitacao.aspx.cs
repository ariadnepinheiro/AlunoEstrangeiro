using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Techne.Web;
using DevExpress.Web.ASPxGridView;
using Techne.Lyceum.RN.Util;
using System.Collections.Generic;

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/CursoCapacitacao.aspx")]
    [ControlText("CursoCapacitacao")]
    [Title("Cursos de Capacitação")]

    public partial class CursoCapacitacao : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                TituloGrid(grdCursoCapacitacao, "Cursos de Capacitação");
                lblMensagem.Text = string.Empty;
                ValidarCampos();

                if (!IsPostBack)
                {
                    ddlAreaConhecimento.DataSource = RN.AreaConhecimento.Listar();
                    ddlAreaConhecimento.Items.Insert(0, "Selecione um Área de Conhecimento");
                    ddlAreaConhecimento.DataBind();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdCursoCapacitacao);
        }

        private void ValidarCampos()
        {
            //txtCargaHoraria.Attributes.Add("onkeyPress", "return onlyNumbers(event, this);");
            txtCargaHoraria.Attributes.Add("onkeyPress", "return isNumberKey(event);");
            txtCargaHoraria.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
            txtCargaHoraria.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");
        }

        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        private void AtualizaGridCursoCapacitacao()
        {
            odsCursoCapacitacao.Select();
            odsCursoCapacitacao.DataBind();
            grdCursoCapacitacao.DataBind();
        }

        #region Eventos

        protected void btnSalvarCursoCapacitacao_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidarCursoCapacitacao())
                {
                    RN.Entidades.CursoCapacitacao entidadeCursoCapacitacao = new RN.Entidades.CursoCapacitacao();
                    var validacao = new ValidacaoDados();
                    bool alterar = false;

                    if (!string.IsNullOrEmpty(txtCursoCapacitacaoID.Text))
                    {
                        alterar = true;
                        entidadeCursoCapacitacao.CursoCapacitacaoId = Convert.ToInt32(txtCursoCapacitacaoID.Text);
                    }

                    entidadeCursoCapacitacao.AreaConhecimentoId = Convert.ToInt32(ddlAreaConhecimento.SelectedValue);
                    entidadeCursoCapacitacao.CargaHoraria = Convert.ToInt32(txtCargaHoraria.Text.Trim());
                    entidadeCursoCapacitacao.DataConclusao = dteDataConclusao.Date;
                    entidadeCursoCapacitacao.DataInicio = dteDataInicio.Date;
                    entidadeCursoCapacitacao.NomeCurso = txtNomeCursoCapacitacao.Text.Trim();
                    entidadeCursoCapacitacao.NomeInstituicao = txtNomeInstituicao.Text.Trim();
                    entidadeCursoCapacitacao.OferecidoSeeduc = Convert.ToBoolean(Convert.ToInt32(rbtListOferecidoSEEDUC.SelectedValue));

                    validacao = RN.CursoCapacitacao.Validar(entidadeCursoCapacitacao);

                    if (validacao.Valido)
                    {
                        if (alterar)
                        {
                            if (RN.CursoCapacitacao.AlterarCurso(entidadeCursoCapacitacao) > 0)
                            {
                                InsereCheckListTipoCurso(alterar);
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Curso de Capacitação alterado com sucesso.');", true);
                            }
                        }
                        else
                        {
                            if (RN.CursoCapacitacao.InserirCurso(entidadeCursoCapacitacao) > 0)
                            {
                                txtCursoCapacitacaoID.Text = RN.CursoCapacitacao.ObtemIdentityCursoCapacitacao().ToString();
                                InsereCheckListTipoCurso(alterar);
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Curso de Capacitação incluído com sucesso.');", true);
                            }
                        }

                        AtualizaGridCursoCapacitacao();
                        LimpaCampos();
                    }
                    else
                    {
                        lblMensagem.Text = validacao.Mensagem;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void InsereCheckListTipoCurso(bool alterar)
        {
            RN.Entidades.ClasseGenerica entidadeTipoCursoCapacitacao = new RN.Entidades.ClasseGenerica();
            IList<int> filhos = new List<int>();

            entidadeTipoCursoCapacitacao.Pai = Convert.ToInt32(txtCursoCapacitacaoID.Text);

            for (int itemLista = 0; itemLista < chkListTipoCurso.Items.Count; itemLista++)
            {
                if (chkListTipoCurso.Items[itemLista].Selected)
                {
                    filhos.Add(Convert.ToInt32(chkListTipoCurso.Items[itemLista].Value));
                }
            }

            entidadeTipoCursoCapacitacao.Filhos = filhos;
            RN.CursoCapacitacao.InserirTiposCurso(entidadeTipoCursoCapacitacao);
        }

        private void LimpaCampos()
        {
            txtCursoCapacitacaoID.Text = string.Empty;
            rbtListOferecidoSEEDUC.SelectedIndex = -1;
            ddlAreaConhecimento.SelectedIndex = 0;
            chkListTipoCurso.SelectedIndex = -1;
            txtNomeCursoCapacitacao.Text = string.Empty;
            txtNomeInstituicao.Text = string.Empty;
            txtCargaHoraria.Text = "";
            dteDataInicio.Date = DateTime.MinValue;
            dteDataConclusao.Date = DateTime.MinValue;
            btnSalvarCursoCapacitacao.Text = "Incluir Capacitação";
        }

        private bool ValidarCursoCapacitacao()
        {
            lblMensagem.Text = "";

            if (string.IsNullOrEmpty(chkListTipoCurso.SelectedValue))
            {
                lblMensagem.Text = "Favor informar o(s) Tipo(s) de Curso.";
                chkListTipoCurso.Focus();
                return false;
            }

            if (ddlAreaConhecimento.SelectedIndex <= 0)
            {
                lblMensagem.Text = "Favor informar a Área de Conhecimento.";
                ddlAreaConhecimento.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtNomeCursoCapacitacao.Text.Trim()))
            {
                lblMensagem.Text = "Favor digitar o Curso de Capacitação.";
                txtNomeCursoCapacitacao.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtNomeInstituicao.Text.Trim()))
            {
                lblMensagem.Text = "Favor digitar o Nome da Instituição.";
                txtNomeInstituicao.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtCargaHoraria.Text.Trim()))
            {
                lblMensagem.Text = "Favor digitar a Carga Horária.";
                txtCargaHoraria.Focus();
                return false;
            }

            if (dteDataInicio.Date == DateTime.MinValue)
            {
                lblMensagem.Text = "Favor informar a Data de Início.";
                dteDataInicio.Focus();
                return false;
            }

            if (dteDataConclusao.Date == DateTime.MinValue)
            {
                lblMensagem.Text = "Favor informar a Data de Conclusão.";
                dteDataConclusao.Focus();
                return false;
            }

            if (dteDataConclusao.Date <= dteDataInicio.Date)
            {
                lblMensagem.Text = "Data de Conclusão deve ser maior do que a Data de Início.";
                dteDataConclusao.Focus();
                return false;
            }

            rbtListOferecidoSEEDUC.SelectedValue = "1";

            return true;
        }

        #endregion

        #region Eventos grdCursoCapacitacao

        protected void grdCursoCapacitacao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdCursoCapacitacao);
        }

        protected void grdCursoCapacitacao_CustomButtonCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            if (e.ButtonID == "Editar")
            {
                string idCursoCapacitacao = grdCursoCapacitacao.GetRowValues(e.VisibleIndex, "CURSOCAPACITACAOID").ToString();
                string idAreaConhecimento = grdCursoCapacitacao.GetRowValues(e.VisibleIndex, "AREACONHECIMENTOID").ToString();
                string nomeCurso = grdCursoCapacitacao.GetRowValues(e.VisibleIndex, "NOMECURSO").ToString();
                string nomeInstituicao = grdCursoCapacitacao.GetRowValues(e.VisibleIndex, "NOMEINSTITUICAO").ToString();
                string cargaHoraria = grdCursoCapacitacao.GetRowValues(e.VisibleIndex, "CARGAHORARIA").ToString();
                string dataInicio = grdCursoCapacitacao.GetRowValues(e.VisibleIndex, "DATAINICIO").ToString();
                string dataConclusao = grdCursoCapacitacao.GetRowValues(e.VisibleIndex, "DATACONCLUSAO").ToString();
                string oferecidoSeeduc = grdCursoCapacitacao.GetRowValues(e.VisibleIndex, "OFERECIDOSEEDUC").ToString();

                txtCursoCapacitacaoID.Text = idCursoCapacitacao.ToString();

                DataTable dtTiposCurso = RN.CursoCapacitacao.ListarTiposCurso(idCursoCapacitacao);

                foreach (DataRow dr in dtTiposCurso.Rows)
                {
                    ListItem chkListAtual = chkListTipoCurso.Items.FindByValue(dr["TIPOCURSOCAPACITACAOID"].ToString());
                    if (chkListAtual != null)
                    {
                        chkListAtual.Selected = true;
                    }
                }

                ddlAreaConhecimento.SelectedValue = idAreaConhecimento;
                txtNomeCursoCapacitacao.Text = nomeCurso;
                txtNomeInstituicao.Text = nomeInstituicao;
                txtCargaHoraria.Text = cargaHoraria;
                dteDataInicio.Date = Convert.ToDateTime(dataInicio);
                dteDataConclusao.Date = Convert.ToDateTime(dataConclusao);
                rbtListOferecidoSEEDUC.Items.FindByValue((Convert.ToInt32(Convert.ToBoolean(oferecidoSeeduc)).ToString())).Selected = true;

                btnSalvarCursoCapacitacao.Text = "Salvar Curso de Capacitação";
            }
        }

        #endregion

        #region Eventos odsCursoCapacitacao

        public object Listar()
        {
            return RN.CursoCapacitacao.Listar();
        }

        public static void DeleteCursoCapacitacao(int CURSOCAPACITACAOID)
        {

        }

        protected void odsCursoCapacitacao_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var validacao = new ValidacaoDados();
            string id = e.InputParameters["CURSOCAPACITACAOID"].ToString();

            validacao = RN.CursoCapacitacao.ValidarExclusao(Convert.ToInt32(id));

            if (!validacao.Valido)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "sas", "<script> alert('" + validacao.Mensagem.ToString() + "');</script>");
            }
            else
            {
                if (RN.CursoCapacitacao.RemoverCurso(int.Parse(id)) > 0)
                {
                    ClientScript.RegisterClientScriptBlock(GetType(), "sas", "<script> alert('Curso de Capacitação excluído com sucesso!');</script>");
                    AtualizaGridCursoCapacitacao();
                }
            }
        }

        #endregion
    }
}
