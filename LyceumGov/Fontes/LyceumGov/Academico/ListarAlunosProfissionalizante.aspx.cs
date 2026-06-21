using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxGridView;
using Seeduc.Infra.Helpers;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Entidades;
using Techne.Web;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/ListarAlunosProfissionalizante.aspx"), ControlText("Educação Profissional Concomitante"), Title("Educação Profissional Concomitante")]
    public partial class ListarAlunosProfissionalizante : TPage
    {
        public object Listar(object unidade_ens)
        {
            var ue = unidade_ens.ToString();

            if (!String.IsNullOrEmpty(ue))
            {
                return AlunoConcomitante.Listar( ue);
            }

            return null;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdProfissional, "Educação Profissional Concomitante");

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;
            if (!IsPostBack)
            {
            }

        }

        protected void tseUnidade_Ensino_Changed(object sender, EventArgs e)
        {
            if (this.tseUnidade_Ensino.IsValidDBValue && !this.tseUnidade_Ensino.DBValue.IsNull)
            {
                pnlAlunosProfissionalizante.Visible = true;
            }
            else if (!this.tseUnidade_Ensino.DBValue.IsNull)
            {
                this.lblMensagem.Text = "Unidade de Ensino não cadastrada.";
                pnlAlunosProfissionalizante.Visible = false;
            }
            else
            {
                this.lblMensagem.Text = "Favor consultar uma Unidade de Ensino.";
                pnlAlunosProfissionalizante.Visible = false;
            }
        }

        protected void grdProfissional_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (!this.grdProfissional.Visible
                || this.grdProfissional.VisibleRowCount == 0)
            {
                return;
            }
            var aluno = (string)grdProfissional.GetRowValues(e.VisibleIndex, "ALUNO");
            var status = (string)grdProfissional.GetRowValues(e.VisibleIndex, "STATUS");
            var enturmado = (string)grdProfissional.GetRowValues(e.VisibleIndex, "ENTURMADO");
            var colCancelado = (GridViewDataColumn)this.grdProfissional.Columns["CANCELADO"];


            if (string.IsNullOrEmpty(status))
            {
                return;
            }

            var rbLiberado = DevExpressHelper.GetControl<RadioButton>(this.grdProfissional, e.VisibleIndex, "ANDAMENTO", "rbLiberado");
            var rbCancelado = DevExpressHelper.GetControl<RadioButton>(this.grdProfissional, e.VisibleIndex, "ANDAMENTO", "rbCancelado");
            var chkCancelado = (CheckBox)this.grdProfissional.FindRowCellTemplateControl(e.VisibleIndex, colCancelado, "chkCancelado");
            var colStatus = (GridViewDataColumn)this.grdProfissional.Columns["status"];
            var txtStatus = (TextBox)this.grdProfissional.FindRowCellTemplateControl(e.VisibleIndex, colStatus, "txtStatus");


            if (rbLiberado == null
                && rbCancelado == null && chkCancelado.Checked == false)
            {
                return;
            }

            if (status == AlunoConcomitante.Cancelado)
            {
                //rbLiberado.Enabled = false;
                //rbCancelado.Enabled = false;
                //rbCancelado.Checked = true;
                chkCancelado.Checked = true;
                chkCancelado.Enabled = false;
            }
            if (enturmado == "true")
            {
                chkCancelado.Visible = false;
            }
            //if (status == AlunoConcomitante.Liberado)
            //{
            //    rbCancelado.Checked = false;
            //    rbLiberado.Checked = enturmado == "true";
            //}
            chkCancelado.InputAttributes.Add("status", status);

           
        }

        protected void grdProfissional_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            var status = (string)grdProfissional.GetRowValues(e.VisibleIndex, "STATUS");
            var enturmado = (string)grdProfissional.GetRowValues(e.VisibleIndex, "ENTURMADO");


            if (!string.IsNullOrEmpty(status)
                && status == AlunoConcomitante.Cancelado)
            {
                if (e.ButtonType == ColumnCommandButtonType.Edit)
                {
                    e.Visible = false;
                }
                if (e.ButtonType == ColumnCommandButtonType.Select)
                {
                    e.Visible = false;
                    
                }
            }
            if (enturmado == "true")
            {
                if (e.ButtonType == ColumnCommandButtonType.Select)
                {
                    e.Visible = false;

                }
            }

        }

        protected void grdProfissional_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            if ((e.CallbackName == "ADDNEWROW") || (e.CallbackName == "STARTEDIT" || e.CallbackName == "SELECTION"))
            {
                string aluno = string.Empty;
                string tipoOperacao = string.Empty;

                if (e.CallbackName == "STARTEDIT")
                {
                    tipoOperacao = "ALTERAR";
                    aluno = Convert.ToString(grdProfissional.GetRowValuesByKeyValue(e.Args[0], "ALUNO"));


                    for (var rowIndex = 0; rowIndex < this.grdProfissional.VisibleRowCount; rowIndex++)
                    {
                        var rbLiberado = DevExpressHelper.GetControl<RadioButton>(this.grdProfissional, rowIndex, "ANDAMENTO",
                                                            "rbLiberado");
                        var rbCancelado = DevExpressHelper.GetControl<RadioButton>(this.grdProfissional, rowIndex, "ANDAMENTO",
                                    "rbCancelado");


                        if (rbLiberado != null || rbCancelado != null)
                        {
                            var ver = rbLiberado.Checked;
                        }
                        

                    }

                }
                else if (e.CallbackName == "SELECTION")
                {
                    tipoOperacao = "CONSULTAR";

                }

                string queryString = MontarQueryString(tipoOperacao, aluno);

                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                //Response.Write("<script> window.open('Alunos.aspx?Chave=" + Convert.ToBase64String(bytesToEncode) + "');</script>");

                // ASPxWebControl.RedirectOnCallback("Alunos.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));

            }


        }


        protected void btnCancelarMatricula_Click(object sender, EventArgs e)
        {
            try
            {
                for (var rowIndex = 0; rowIndex < this.grdProfissional.VisibleRowCount; rowIndex++)
                {

                    var chkCancelado = DevExpressHelper.GetControl<CheckBox>(this.grdProfissional, rowIndex,
                                                                             "CANCELADO",
                                                                             "chkCancelado");

                    var aluno = (string) grdProfissional.GetRowValues(rowIndex, "ALUNO");
                    var censo = (string) grdProfissional.GetRowValues(rowIndex, "CENSO");
                    var ano = (int) grdProfissional.GetRowValues(rowIndex, "ANO");
                    var periodo = (int) grdProfissional.GetRowValues(rowIndex, "PERIODO");
                    var status = (string) grdProfissional.GetRowValues(rowIndex, "STATUS");

                    if (chkCancelado != null)
                    {
                        if (status != "Cancelado" && chkCancelado.Checked)
                        {
                            var conc = new TceAlunoConcomitante
                                           {
                                               Aluno = aluno,
                                               Censo = censo,
                                               Matricula = User.Identity.Name,
                                               Ano = ano,
                                               Periodo = periodo,
                                               Status = AlunoConcomitante.Cancelado
                                           };

                            var validacao = AlunoConcomitante.Validar(conc);

                            if (!validacao.Valido)
                            {
                                if (!string.IsNullOrEmpty(validacao.Mensagem))
                                {
                                    this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                                }
                                return;
                            }
                            else
                            {
                                AlunoConcomitante.Salvar(conc);

                            }
                        }
                    }
                }
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "popup", "alert('Cancelamento(s) efetuado(s) com sucesso.');", true);

                odsProfissional.Select();
                odsProfissional.DataBind();
                grdProfissional.DataBind();
            }
            catch (Exception ex)
            {
                

                lblMensagem.Text = ex.Message;
            }
        }

        private string MontarQueryString(string tipoOperacao, string aluno)
        {
            string queryString = string.Empty;

            if (!string.IsNullOrEmpty(aluno))
            {
                queryString += "aluno=" + aluno;
            }
            return queryString;
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


        private int GetSelectedRowOnTheCurrentPage()
        {
            var startIndexOnPage = this.grdProfissional.PageIndex * this.grdProfissional.SettingsPager.PageSize;
            var selectedRow = -1;

            for (var i = 0; i < this.grdProfissional.VisibleRowCount; i++)
            {
                if (this.grdProfissional.Selection.IsRowSelected(startIndexOnPage + i))
                {
                    selectedRow = startIndexOnPage + i;

                    break;
                }
            }

            this.grdProfissional.Selection.UnselectAll();
            return selectedRow;
        }
        protected void grdProfissional_SelectionChanged(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                return;
            }

            var aluno = Convert.ToString(grdProfissional.GetRowValues(GetSelectedRowOnTheCurrentPage(), "ALUNO"));

            if (string.IsNullOrEmpty(aluno))
            {
                return;
            }

            var tipoOperacao = "ALTERAR";
            string queryString = MontarQueryString(tipoOperacao, aluno);

            byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

            //ASPxWebControl.RedirectOnCallback("Alunos.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
            //Server.Transfer("Alunos.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));

            //Response.Redirect("Alunos.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
            //Response.Write("<script>window.open('Alunos.aspx','FrameName');</script>");




        }
    }
}
