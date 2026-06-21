using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.MobileControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Entidades;
using Techne.Controls;


namespace Techne.Lyceum.Net.Curriculo
{
    [NavUrl("~/Curriculo/ExclusaoAlocacaoDocente.aspx"),
    ControlText("ExclusaoAlocacaoDocente"),
    Title("Exclusão da Alocação dos Docentes"),]

    public partial class ExclusaoAlocacaoDocente : TPage
    {
        #region Código padrão
        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
        }
        #endregion
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    pnlDados.Visible = !tseDocente.DBValue.IsNull && tseDocente.IsValidDBValue;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }          
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            try
            {
                pnlAcao.Visible = grdAulas.VisibleRowCount > 0;

                if (!IsPostBack)
                {
                    string anoAtual = DateTime.Today.Year.ToString();
                    if (ddlAno.Items.FindByText(anoAtual) != null)
                        ddlAno.Items.FindByText(anoAtual).Selected = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdAulas, "Exclusão de Alocações do Docente");
        }

        protected void tseDocente_Changed(object sender, EventArgs e)
        {
            try
            {
                LimparTela();
                lblMensagem.Text = string.Empty;
                pnlDados.Visible = false;

                if (!tseDocente.DBValue.IsNull && tseDocente.IsValidDBValue)
                {
                    String matricula = Convert.ToString(tseDocente["num_func"]);
                    hdnNumFunc.Value = Convert.ToString(tseDocente["num_func"]);

                    if (!hdnNumFunc.Value.IsNullOrEmptyOrWhiteSpace())
                    {

                        QueryTable qtDocente = RN.Turma.ConsultarDadosDocente(matricula);
                        if (qtDocente == null || qtDocente.Rows.Count == 0)
                        {
                            lblMensagem.Text = "Informações do docente não encontrado ou não ativo.";
                           
                        }
                        else
                        {

                            SimpleRow rowDocente = qtDocente.Rows[0];
                            txtNome.Text = Convert.ToString(rowDocente["nome"]);
                            txtMatricula.Text = Convert.ToString(rowDocente["idvinculo"]);

                            Int64 cpf;
                            Int64.TryParse(Convert.ToString(rowDocente["cpf"]), out cpf);
                            txtCPF.Text = string.Format(@"{0:000\.000\.000-00}", cpf);

                            txtCargo.Text = Convert.ToString(rowDocente["cargo"]);
                            txtFuncao.Text = Convert.ToString(rowDocente["funcao"]);
                            txtDisciplinaIngresso.Text = "<TODO>";
                            txtSituacao.Text = "<TODO>";
                            txtCHIngresso.Text = "<TODO>";
                            txtCHTurma.Text = "<TODO>";

                            trSegundaMatricula.Visible = !rowDocente["matricula2"].IsNull;
                            txtSegundaMatricula.Text = Convert.ToString(rowDocente["matricula2"]);

                            trCH.Visible = false;
                            trDiscipIngressoSit.Visible = false;

                            pnlDados.Visible = !tseDocente.DBValue.IsNull && tseDocente.IsValidDBValue;
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "Docente não encontrado.";
                       
                    }
                }
            }
            catch(Exception ex)
            {
                lblMensagem.Text = ex.Message;
                

            }
        }

        protected void LimparTela()
        {
            hdnNumFunc.Value = string.Empty;
            txtNome.Text = String.Empty;
            txtMatricula.Text = String.Empty;
            txtCPF.Text = String.Empty;
            txtCargo.Text = String.Empty;
            txtFuncao.Text = String.Empty;
            txtDisciplinaIngresso.Text = String.Empty;
            txtSituacao.Text = String.Empty;
            txtCHIngresso.Text = String.Empty;
            txtCHTurma.Text = String.Empty;
            txtSegundaMatricula.Text = String.Empty;
            tseFuncaoLotacao.ResetValue();
        }

        protected void grdAulas_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (e.DataColumn.DataItemTemplate != null)
            {
                if (Convert.ToString(e.CellValue) == "0")
                {
                    e.Cell.Enabled = false;
                    e.Cell.Attributes.Add("style", "visibility:hidden");
                }
            }
        }

        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            String matriculaCarencia = rbtnCarencia.SelectedItem.Value.ToString();

            List<int> indexes = new List<int>();

            for (int index = 0; index <= grdAulas.VisibleRowCount; index++)
            {
                GridViewDataColumn col = (GridViewDataColumn)grdAulas.Columns["pode_excluir"];
                Control control = grdAulas.FindRowCellTemplateControl(index, col, "cbExcluir");

                if (!(control is CheckBox))
                    continue;

                CheckBox cb = ((CheckBox)control);
                if (cb != null && cb.Checked)
                    indexes.Add(index);
            }

            List<Ly_aula_docente.Row> dtADRemocao = new List<Ly_aula_docente.Row>();
            List<Ly_aula_docente_tipo.Row> dtADTRemocao = new List<Ly_aula_docente_tipo.Row>();

            foreach (int index in indexes) // Percorre todas as linhas selecionadas para exclusão
            {
                DataRowView drv = grdAulas.GetRow(index) as DataRowView;
                String matricula = Convert.ToString(drv.Row["matricula"]);
                Decimal dia_semana = Convert.ToDecimal(drv.Row["dia_Semana"]);
                Decimal aula = Convert.ToDecimal(drv.Row["aula"]);
                String disciplina = Convert.ToString(drv.Row["disciplina"]);
                String turma = Convert.ToString(drv.Row["turma"]);
                String turno = Convert.ToString(drv.Row["turno"]);
                String faculdade = Convert.ToString(drv.Row["ue"]);
                Decimal ano = Convert.ToDecimal(drv.Row["ano"]);
                Decimal semestre = Convert.ToDecimal(drv.Row["semestre"]);
                Decimal num_func = Convert.ToDecimal(drv.Row["num_func"]);
                DateTime data_inicio = Convert.ToDateTime(drv.Row["data_inicio"]);
                Boolean GLP = Convert.ToString(drv.Row["tipo"]) == "GLP";

                Ly_aula_docente.Row adRemocao = new Ly_aula_docente().NewRow();
                adRemocao.Num_func = num_func;
                adRemocao.Turno = turno;
                adRemocao.Faculdade = faculdade;
                adRemocao.Dia_semana = dia_semana;
                adRemocao.Aula = aula;
                adRemocao.Disciplina = disciplina;
                adRemocao.Turma = turma;
                adRemocao.Ano = ano;
                adRemocao.Semestre = semestre;
                adRemocao.Data_inicio = data_inicio;
                dtADRemocao.Add(adRemocao);

                if (GLP)
                {
                    Ly_aula_docente_tipo.Row adtRemocao = new Ly_aula_docente_tipo().NewRow();
                    adtRemocao.Num_func = num_func;
                    adtRemocao.Turno = turno;
                    adtRemocao.Faculdade = faculdade;
                    adtRemocao.Dia_semana = dia_semana;
                    adtRemocao.Aula = aula;
                    adtRemocao.Disciplina = disciplina;
                    adtRemocao.Turma = turma;
                    adtRemocao.Ano = ano;
                    adtRemocao.Semestre = semestre;
                    adtRemocao.Data_inicio = data_inicio;
                    dtADTRemocao.Add(adtRemocao);
                }
            }

            RetValue ret = RN.Turma.DesalocarAulasDocenteAlocarCarencia(dtADRemocao.ToArray(), dtADTRemocao.ToArray(), matriculaCarencia);
            if (ret != null)
            {
                lblMensagem.Text = ret.Errors.Count > 0 ? ret.Errors.ToString() : ret.Message;
                lblMensagem.Visible = true;
            }
            grdAulas.DataBind();
        }

        protected void btnAlterarFuncao_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new Techne.Lyceum.RN.Util.ValidacaoDados();
                RN.Docentes rnDocentes = new Techne.Lyceum.RN.Docentes();
                RN.Lotacao rnLotacao = new Techne.Lyceum.RN.Lotacao();
                RN.Entidades.LyLotacao lotacao = new LyLotacao();
                RN.Entidades.LyLotacao proximaLotacao = new LyLotacao();

                 if (!tseDocente.DBValue.IsNull && tseDocente.IsValidDBValue)
                 {

                     if (!tseFuncaoLotacao.DBValue.IsNull && tseFuncaoLotacao.IsValidDBValue)
                     {

                         decimal pessoa = Convert.ToDecimal(tseDocente["pessoa"]);

                         validacao = rnLotacao.ValidaAlteracaoFuncaoDocente(pessoa, tseDocente["MATRICULA"].ToString(), User.Identity.Name, tseFuncaoLotacao.DBValue.ToString(), out lotacao, out proximaLotacao);

                         if (validacao.Valido)
                         {
                             rnLotacao.AlteraFuncaoDocente(lotacao, proximaLotacao);

                             lblMensagem.Text = "Troca de função realizada com sucesso.";
                             lblMensagem.Visible = true;
                             txtFuncao.Text = tseFuncaoLotacao["descricao"].ToString();
                             tseFuncaoLotacao.ResetValue();
                         }
                         else
                         {
                             lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                         }
                     }
                     else
                     {
                         lblMensagem.Text = "Para efetuar a troca é necessário escolher uma Função.";
                     }
                     lblMensagem.Visible = true;
                 }               
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
            }
        }

        protected void tseFuncaoLotacao_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }
                if (!this.tseFuncaoLotacao.DBValue.IsNull)
                {
                    if (!this.tseFuncaoLotacao.IsValidDBValue)                   
                    {
                        lblMensagem.Text = "Função não cadastrada.";
                    }
                }
                else
                {
                    lblMensagem.Text = "Favor informar uma função.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }    
}
