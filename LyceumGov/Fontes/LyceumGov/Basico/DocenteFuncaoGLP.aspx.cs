using System;
using System.Web;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using System.Linq;

namespace Techne.Lyceum.Net.Basico
{
    [
     NavUrl("~/Basico/DocenteFuncaoGLP.aspx"),
      ControlText("DocenteFuncaoGLP"),
      Title("Solicitação de GLP"),
    ]

    public partial class DocenteFuncaoGLP : TPage
    {
        public object Listar(object unidadeAdm)
        {
            RN.DocenteGLP rnDocenteGLP = new DocenteGLP();

            var unidade = unidadeAdm != null ? unidadeAdm.ToString() : null;

            if (!string.IsNullOrEmpty(unidade))
            {
                return rnDocenteGLP.ListaSolicitacaoPor(unidade);
            }
            return null;
        }

        public object ListarTurmaCarencia(object unidadeEnsino, object agrupamentoDisciplina)
        {
            RN.AulaDocente rnAulaDocente = new AulaDocente();

            var unidade = unidadeEnsino != null ? unidadeEnsino.ToString() : null;
            var disciplina = agrupamentoDisciplina != null ? agrupamentoDisciplina.ToString() : null;

            if (!string.IsNullOrEmpty(unidade) && !string.IsNullOrEmpty(disciplina))
            {
                return rnAulaDocente.ListaTurmaCarenciaPor(unidade.ToString(), disciplina.ToString());
            }
            return null;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdDocenteFuncaoGLP, "Solicitações de GLP");
            TituloGrid(grdTurmaCarencia, "Turmas com Carência");
            TituloGrid(grdTurmasPedido, string.Empty);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    pnlGridTurmas.Visible = false;
                    ListItem itemVazio = new ListItem("Selecione", string.Empty);
                    ddlDisciplina.Items.Add(itemVazio);

                    if (!RN.Usuarios.UsuarioPrivilegiado(HttpContext.Current.User.Identity.Name))
                    {
                        tseUnidadeAdministrativa.SqlWhere = " uuf.USUARIO = '" + HttpContext.Current.User.Identity.Name + "'";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdDocenteFuncaoGLP);
            ControlaAcesso(btnSalvar, AcaoControle.novo);

        }

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

        protected void grdDocenteFuncaoGLP_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdDocenteFuncaoGLP);
        }

        protected void tseUnidadeAdministrativa_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                tseDocente.ResetValue();
                tseUnidade_Ensino.ResetValue();
                ddlCategoriaCurso.ClearSelection();
                ddlDisciplina.Items.Clear();
                txtQtdGLP.Text = "0";
                grdDocenteFuncaoGLP.Visible = false;
                pnPrincipal.Visible = false;
                pnlGridTurmas.Visible = false;
                btnSalvar.Visible = false;

                if (tseUnidadeAdministrativa.IsValidDBValue && !tseUnidadeAdministrativa.DBValue.IsNull)
                {
                    pnPrincipal.Visible = true;
                    grdDocenteFuncaoGLP.Visible = true;
                }
                else if (!tseUnidadeAdministrativa.DBValue.IsNull)
                {
                    lblMensagem.Text = "Unidade Administrativa não cadastrada.";
                }
                else
                {
                    lblMensagem.Text = "Favor consultar uma unidade administrativa.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseDocente_Changed(object sender, EventArgs e)
        {
            RN.Docentes rnDocentes = new Techne.Lyceum.RN.Docentes();
            try
            {
                lblMensagem.Text = string.Empty;
                btnSalvar.Visible = false;
                pnlGridTurmas.Visible = false;
                ddlDisciplina.Items.Clear();
                ListItem itemVazio = new ListItem("Selecione", string.Empty);
                ddlDisciplina.Items.Add(itemVazio);

                if (tseDocente.IsValidDBValue & !tseDocente.DBValue.IsNull)
                {
                    if (!tseDocente["idfuncional"].ToString().IsNullOrEmptyOrWhiteSpace() && !tseDocente["idvinculo"].ToString().Contains("/"))
                    {
                        lblMensagem.Text = "Formato incorreto. Para pesquisar um docente é necessário informar Id/Vinculo.";
                        return;
                    }

                    if (rnDocentes.MatriculaContrato(tseDocente["num_func"].ToString()))
                    {
                        lblMensagem.Text = "Não é permitido GLP para Contrato Temporário.";
                        return;
                    }
                    CarregarDadosDrop(ddlDisciplina.ID);

                }
                else
                {
                    lblMensagem.Text = "Id/vínculo inválido ou formato incorreto.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private QueryTable CarregarDadosDrop(string idDrop)
        {
            QueryTable dadosDrop = null;
            try
            {
                switch (idDrop.ToUpper())
                {
                    case "DDLDISCIPLINA":
                        {
                            string numFunc = tseDocente["num_func"].ToString();
                            dadosDrop = RN.DocenteGLP.ConsultarAgrupamentosHabilitados(numFunc);
                            CarregarDropDownList(ddlDisciplina, dadosDrop, null);
                            break;
                        }
                    default:
                        break;
                }
            }
            catch
            {
                throw;
            }

            return dadosDrop;
        }

        private void CarregarDropDownList(DropDownList drop, QueryTable data, string defaultValue)
        {
            drop.Items.Clear();
            drop.DataSource = data;
            drop.DataBind();

            if (drop.Items.Count == 0)
            {
                ListItem itemVazio = new ListItem("<Lista Vazia>", "");
                drop.Items.Add(itemVazio);
            }
            else
            {
                ListItem item = new ListItem("<Selecione>", "");
                drop.Items.Add(item);
                drop.SelectedValue = "";
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            int total = 0;

            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.DocenteGLP rnDocenteGLP = new DocenteGLP();
                RN.Entidades.LyDocenteFuncaoGlp docenteFuncaoGLP = new Techne.Lyceum.RN.Entidades.LyDocenteFuncaoGlp();
                System.Collections.Generic.List<RN.RecursosHumanos.Entidades.DocenteFuncaoGlpTurma> listaDocenteFuncaoGlpTurma = new System.Collections.Generic.List<RN.RecursosHumanos.Entidades.DocenteFuncaoGlpTurma>();
                RN.RecursosHumanos.Entidades.DocenteFuncaoGlpTurma docenteFuncaoGLPTurma = new Techne.Lyceum.RN.RecursosHumanos.Entidades.DocenteFuncaoGlpTurma();

                docenteFuncaoGLP.Matricula = tseDocente["matricula"].ToString();
                docenteFuncaoGLP.Agrupamento = !ddlDisciplina.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlDisciplina.SelectedValue : null;
                docenteFuncaoGLP.Ano = DateTime.Today.Year;
                docenteFuncaoGLP.Mes = DateTime.Today.Month;
                docenteFuncaoGLP.Status = "Aguardando";
                docenteFuncaoGLP.Data = DateTime.Today;
                docenteFuncaoGLP.DataSolicitacao = DateTime.Today;
                docenteFuncaoGLP.UnidadeEns = tseUnidade_Ensino.DBValue.ToString();
                docenteFuncaoGLP.Prazo = 0;
                docenteFuncaoGLP.UsuarioSolicitacaoId = User.Identity.Name;

                //descobrir qual a função pelo curso indicado e número de matrícula
                if (ddlCategoriaCurso.SelectedValue == "Ensino Fundamental Anos Iniciais")
                {
                    if (docenteFuncaoGLP.Matricula.StartsWith("50"))
                        docenteFuncaoGLP.FuncaoGlp = "109"; //doc II 40
                    else
                        docenteFuncaoGLP.FuncaoGlp = "108"; //doc II
                }
                else if (ddlCategoriaCurso.SelectedValue == "Ensino Fundamental Anos Finais / Ensino Médio")
                {
                    if (docenteFuncaoGLP.Matricula.StartsWith("50"))
                        docenteFuncaoGLP.FuncaoGlp = "107";//doc I 40
                    else
                        docenteFuncaoGLP.FuncaoGlp = "106";//doc I
                }


                listaDocenteFuncaoGlpTurma = this.grdTurmaCarencia
               .GetSelectedFieldValues("CompositeKey")
               .Select(x => new Techne.Lyceum.RN.RecursosHumanos.Entidades.DocenteFuncaoGlpTurma()
               {
                   // e.Value = ano + "|" + semestre + "|" + turma + "|" + disciplina + "|" + NumFuncCarencia + "|" + contagemcarencias;                  

                   Ano = Convert.ToDecimal(x.ToString().Split('|')[0]),
                   Periodo = Convert.ToDecimal(x.ToString().Split('|')[1]),
                   Turma = x.ToString().Split('|')[2],
                   Disciplina = x.ToString().Split('|')[3],
                   NumFuncCarencia = Convert.ToDecimal(x.ToString().Split('|')[4]),
                   CargaHoraria = Convert.ToInt32(x.ToString().Split('|')[5]),
               })
               .ToList();


                total = listaDocenteFuncaoGlpTurma.Sum(x => x.CargaHoraria);

                docenteFuncaoGLP.GlpSolicitada = total;

                validacao = rnDocenteGLP.ValidaSolicitacao(docenteFuncaoGLP, listaDocenteFuncaoGlpTurma, Convert.ToDecimal(tseDocente["num_func"].ToString()), Convert.ToDecimal(tseDocente["pessoa"].ToString()));

                if (validacao.Valido)
                {
                    rnDocenteGLP.Solicita(docenteFuncaoGLP, listaDocenteFuncaoGlpTurma);
                    lblMensagem.Text = "Pedido incluído com sucesso. Docente é candidato a realizar GLP. Isso não significa que será alocado em GLP.";
                    LimparCampos();
                    odsDocenteFuncaoGLP.Select();
                    odsDocenteFuncaoGLP.DataBind();
                    grdDocenteFuncaoGLP.DataBind();
                    odsTurmaCarencia.Select();
                    odsTurmaCarencia.DataBind();
                    grdTurmaCarencia.DataBind();
                    pnlGridTurmas.Visible = false;
                }
                else
                {
                    this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    txtQtdGLP.Text = total.ToString();
                }
            }
            catch (Exception ex)
            {
                txtQtdGLP.Text = total.ToString();
                lblMensagem.Text = ex.Message;
            }
        }


        protected void LimparCampos()
        {
            tseDocente.ResetValue();
            tseUnidade_Ensino.ResetValue();
            txtQtdGLP.Text = string.Empty;
            ddlCategoriaCurso.SelectedValue = "";
            ddlDisciplina.Items.Clear();
            ListItem itemVazio = new ListItem("Selecione", string.Empty);
            ddlDisciplina.Items.Add(itemVazio);

        }


        protected void grdDocenteFuncaoGLP_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            if (Session["Mensagem"] == null)
                e.Properties["cpMessage"] = String.Empty;
            else
            {
                e.Properties["cpMessage"] = Session["Mensagem"].ToString();
                Session["Mensagem"] = null;
            }
        }

        public void Delete(object id_docente_funcao_glp) { }

        protected void odsDocenteFuncaoGLP_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            RN.DocenteGLP rnDocenteGLP = new DocenteGLP();
            ValidacaoDados validacao = new ValidacaoDados();

            int id_solicitacao = Convert.ToInt32(e.InputParameters["id_docente_funcao_glp"]);

            validacao = rnDocenteGLP.ValidaRemocaoSolicitacao(id_solicitacao);

            if (validacao.Valido)
            {
                rnDocenteGLP.RemoveSolicitacao(id_solicitacao);
                odsDocenteFuncaoGLP.Select();
                odsDocenteFuncaoGLP.DataBind();
                grdDocenteFuncaoGLP.DataBind();
            }
            else
            {
                //this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");

                throw new Exception(validacao.Mensagem);

            }

            e.Cancel = true;
            this.grdDocenteFuncaoGLP.CancelEdit();
        }

        protected void ddlDisciplina_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pnlGridTurmas.Visible = false;
                txtQtdGLP.Text = "0";
                grdTurmaCarencia.Selection.UnselectAll();
                grdTurmaCarencia.DataBind();
                btnSalvar.Visible = false;

                if (!ddlDisciplina.SelectedValue.IsNullOrEmptyOrWhiteSpace() && (!this.tseDocente.DBValue.IsNull && tseDocente.IsValidDBValue))
                {
                    pnlGridTurmas.Visible = true;
                    btnSalvar.Visible = true;
                }
            }
            catch (Exception EX)
            {
                lblMensagem.Text = EX.Message;
            }

        }

        protected void grdDocenteFuncaoGLP_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "segmento")
            {
                string valor = e.Value.ToString();
                if (!string.IsNullOrEmpty(valor))
                {
                    if (valor.Contains("DOC II"))
                    {
                        e.DisplayText = "Ensino Fundamental Anos Iniciais";
                    }
                    else if (valor.Contains("DOC I"))
                    {
                        e.DisplayText = "Ensino Fundamental Anos Finais / Ensino Médio";
                    }
                }
            }
        }

        protected void grdTurmaCarencia_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdTurmaCarencia);
        }

        protected void grdTurmaCarencia_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CompositeKey")
            {
                string ano = Convert.ToString(e.GetListSourceFieldValue("ANO"));
                string semestre = Convert.ToString(e.GetListSourceFieldValue("SEMESTRE"));
                string turma = Convert.ToString(e.GetListSourceFieldValue("TURMA"));
                string disciplina = Convert.ToString(e.GetListSourceFieldValue("DISCIPLINA"));
                string NumFuncCarencia = Convert.ToString(e.GetListSourceFieldValue("NUM_FUNC"));
                string contagemcarencias = Convert.ToString(e.GetListSourceFieldValue("CONTAGEMCARENCIAS"));

                e.Value = ano + "|" + semestre + "|" + turma + "|" + disciplina + "|" + NumFuncCarencia + "|" + contagemcarencias;
            }
        }

        protected void btnDetalhes_Command(object sender, CommandEventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(e.CommandArgument);
                string codigoSolicitacao = grdDocenteFuncaoGLP.GetRowValuesByKeyValue(id, "id_docente_funcao_glp").ToString();

                txtRow.Value = codigoSolicitacao;

                pucTurmaPedido.ShowOnPageLoad = true;

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }
    }
}
