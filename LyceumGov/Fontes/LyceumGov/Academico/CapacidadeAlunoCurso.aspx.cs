using System;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Academico
{
    using Techne.Web;
    using Techne.Lyceum.RN.Util;
    using Techne.Lyceum.RN.DTOs;
    [NavUrl("~/Academico/CapacidadeAlunoCurso.aspx"),
 ControlText("CapacidadeAlunoCurso"),
 Title("Capacidade de Alunos em Turma por Curso"),]
    public partial class CapacidadeAlunoCurso : TPage
    {
        public object Listar(object ano, object periodo)
        {
            if ((ano != null && ano.ToString() != "Selecione")
                && (periodo != null && periodo.ToString() != "Selecione"))
            {
                return RN.CapacidaDeAlunoTurma.Listar(Convert.ToDecimal(ano), Convert.ToDecimal(periodo));
            }

            return null;
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdCapacidade, "Capacidade de Alunos em Turma por Curso");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;
            this.ValidarCampos();

            if (!IsPostBack)
            {
                ddlAno.DataSource = RN.PeriodoLetivo.ListarAnos();
                ddlAno.Items.Insert(0, "Selecione");
                ddlAno.DataBind();
            }
        }
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(ddlAno.SelectedValue) && ddlAno.SelectedValue != "Selecione")
            //{
            //    tsCurso.QueryParameters["ano"].DefaultValue = ddlAno.SelectedValue;
            //}

            //if (!string.IsNullOrEmpty(ddlPeriodo.SelectedValue) && ddlPeriodo.SelectedValue != "Selecione")
            //{
            //    tsCurso.QueryParameters["periodo"].DefaultValue = ddlPeriodo.SelectedValue;
            //}

            //if (!string.IsNullOrEmpty(ddlModalidade.SelectedValue) && ddlModalidade.SelectedValue  != "Selecione")
            //{
            //    tsCurso.QueryParameters["modalidade"].DefaultValue = ddlModalidade.SelectedValue;
            //}

            //if (!string.IsNullOrEmpty(ddlNivel.SelectedValue) && ddlNivel.SelectedValue  != "Selecione")
            //{
            //    tsCurso.QueryParameters["tipo"].DefaultValue = ddlNivel.SelectedValue;
            //}
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlPeriodo.Items.Clear();
            ddlReplicar.Items.Clear();
            ddlModalidade.Items.Clear();
            ddlNivel.Items.Clear();
            //tsCurso.ResetValue();
            tseCurso.ResetValue();
            grdCapacidade.Visible = false;
            pnlGerais.Visible = false;
            txtMaxima.Text = string.Empty;
            txtMinima.Text = string.Empty;
            btnSalvar.Visible = true;
            btnReplicar.Visible = false;

            if (ddlAno.SelectedValue != "Selecione" && !string.IsNullOrEmpty(ddlAno.SelectedValue))
            {
                this.ddlPeriodo.DataSource = RN.PeriodoLetivo.ListarPeriodo(this.ddlAno.SelectedValue);
                this.ddlPeriodo.Items.Insert(0, "Selecione");
                this.ddlPeriodo.DataBind();
            }
        }
        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlReplicar.Items.Clear();
            ddlModalidade.Items.Clear();
            ddlNivel.Items.Clear();
            tseCurso.ResetValue();
            grdCapacidade.Visible = false;
            pnlGerais.Visible = false;
            txtMaxima.Text = string.Empty;
            txtMinima.Text = string.Empty;
            btnSalvar.Visible = true;
            btnReplicar.Visible = false;

            if (ddlPeriodo.SelectedValue != "Selecione")
            {
                grdCapacidade.Visible = true;
                pnlGerais.Visible = true;

                this.ddlReplicar.DataSource = RN.CapacidaDeAlunoTurma.ListarAnoPeriodoReplicacao(int.Parse(ddlAno.SelectedValue), int.Parse(ddlPeriodo.SelectedValue));
                this.ddlReplicar.Items.Insert(0, "Nenhum");
                this.ddlReplicar.DataBind();

                ddlModalidade.Items.Clear();
                ddlModalidade.DataSource = RN.Curso.ListarModalidadeSerie();
                ddlModalidade.Items.Insert(0, "Selecione");
                ddlModalidade.DataBind();

                ddlNivel.DataSource = RN.Curso.ListarTipoCurso();
                ddlNivel.Items.Insert(0, "Selecione");
                ddlNivel.DataBind();
            }
        }

        protected void ddlModalidade_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlNivel.ClearSelection();
            tseCurso.ResetValue();
            txtMaxima.Text = string.Empty;
            txtMinima.Text = string.Empty;

            if (!string.IsNullOrEmpty(ddlModalidade.SelectedValue))
            {
                tseCurso.SqlWhere = "c.modalidade = '" + ddlModalidade.SelectedValue + "'";
            }
        }

        protected void ddlNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtMaxima.Text = string.Empty;
            txtMinima.Text = string.Empty;

            if (!string.IsNullOrEmpty(ddlNivel.SelectedValue))
            {
                if (!string.IsNullOrEmpty(ddlNivel.SelectedValue))
                {
                    tseCurso.SqlWhere = "c.modalidade = '" + ddlModalidade.SelectedValue + "' AND c.tipo = '" + ddlNivel.SelectedValue + "'";
                }
                else
                {
                    tseCurso.SqlWhere = "c.tipo = '" + ddlNivel.SelectedValue + "'";
                }
            }
        }

        protected void ddlReplicar_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlModalidade.ClearSelection();
            ddlNivel.ClearSelection();
            tseCurso.ResetValue();
            txtMaxima.Text = string.Empty;
            txtMinima.Text = string.Empty;

            if (ddlReplicar.SelectedValue != "Nenhum")
            {
                ddlModalidade.Enabled = false;
                ddlNivel.Enabled = false;
                tseCurso.Enabled = false;
                txtMinima.Enabled = false;
                txtMaxima.Enabled = false;
                btnSalvar.Visible = false;
                btnReplicar.Visible = true;
            }
            else
            {
                ddlModalidade.Enabled = true;
                ddlNivel.Enabled = true;
                tseCurso.Enabled = true;
                txtMinima.Enabled = true;
                txtMaxima.Enabled = true;
                btnSalvar.Visible = true;
                btnReplicar.Visible = false;
            }
        }

        protected void tsCurso_Changed(object sender, EventArgs args)
        {
            if (this.Page.IsCallback)
            {
                return;
            }

            this.txtMinima.Text = string.Empty;
            this.txtMaxima.Text = string.Empty;
          
            if (!this.tseCurso.DBValue.IsNull)
            {
                this.pnGeral.Visible = true;
            }
        }
        private void ValidarCampos()
        {
            this.txtMaxima.Attributes.Add("onkeyPress", "return onlyNumbers(event, this);");
            this.txtMaxima.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
            this.txtMaxima.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");

            this.txtMinima.Attributes.Add("onkeyPress", "return onlyNumbers(event, this);");
            this.txtMinima.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
            this.txtMinima.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");
        }

        protected void grdCapacidade_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdCapacidade);
        }

        protected void grdCapacidade_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdCapacidade.Settings.ShowFilterRow = false;
        }

        public void Update(object CAPACIDADEALUNOTURMAID, object ANOPERIODO, object CURSOID, object DESCRICAOCURSO, object DESCRICAOTIPO, object CAPACIDADEMINIMA, object CAPACIDADEMAXIMA, object DATAALTERACAO)
        {
        } 

        public void Delete(object CAPACIDADEALUNOTURMAID)
        {
        }

        protected void odsCapacidade_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var id = e.InputParameters["CAPACIDADEALUNOTURMAID"].ToString();

            RN.CapacidaDeAlunoTurma.Remover(int.Parse(id));
        }

        protected void odsCapacidade_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var validacao = new ValidacaoDados();

            string[] anoperiodo = e.InputParameters["ANOPERIODO"].ToString().Split('/');

            var capac = new RN.Entidades.CapacidaDeAlunoTurma
            {
                CapacidaDeAlunoTurmaId = Convert.ToInt32(e.InputParameters["CAPACIDADEALUNOTURMAID"].ToString()),
                CapacidadeMinima = (e.InputParameters["CAPACIDADEMINIMA"] != null) ? Convert.ToInt32(e.InputParameters["CAPACIDADEMINIMA"]) : -1,
                CapacidadeMaxima = (e.InputParameters["CAPACIDADEMAXIMA"] != null) ? Convert.ToInt32(e.InputParameters["CAPACIDADEMAXIMA"]) : -1,
                CursoId = e.InputParameters["CURSOID"].ToString(),
                Ano = Convert.ToInt32(anoperiodo[0]),
                Periodo = Convert.ToInt32(anoperiodo[1]),
                Matricula = this.User.Identity.Name
            };

            validacao = RN.CapacidaDeAlunoTurma.Validar(capac);

            if (!validacao.Valido)
            {
                throw new Exception(validacao.Mensagem);
            }
            
            RN.CapacidaDeAlunoTurma.Alterar(capac);
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlReplicar.SelectedValue == "Nenhum")
                {
                    long resultado;

                    if (!long.TryParse(this.txtMinima.Text, out resultado))
                    {
                        this.lblMensagem.Text = "O campo Cap. Mínima é obrigatório e deve ser composto por números.";
                        this.txtMinima.Text = string.Empty;
                        this.txtMinima.Focus();
                        return;
                    }

                    if (Convert.ToInt32(this.txtMaxima.Text) >35 && Convert.ToString(this.tseCurso.DBValue) == "9999.01" )
                    {
                        this.lblMensagem.Text = "O campo Cap. Máxima é supera o valor esperado.";
                        this.txtMaxima.Text = string.Empty;
                        this.txtMaxima.Focus();
                        return;
                    }
                    if (Convert.ToInt32(this.txtMaxima.Text) > 35 && Convert.ToString(this.tseCurso.DBValue) == "9999.03")
                    {
                        this.lblMensagem.Text = "O campo Cap. Máxima é supera o valor esperado.";
                        this.txtMaxima.Text = string.Empty;
                        this.txtMaxima.Focus();
                        return;
                    }
                    if (Convert.ToInt32(this.txtMaxima.Text) > 100 && Convert.ToString(this.tseCurso.DBValue) == "9999.02")
                    {
                        this.lblMensagem.Text = "O campo Cap. Máxima é supera o valor esperado.";
                        this.txtMaxima.Text = string.Empty;
                        this.txtMaxima.Focus();
                        return;
                    }
                    if (!long.TryParse(this.txtMaxima.Text, out resultado))
                    {
                        this.lblMensagem.Text = "O campo Cap. Máxima é obrigatório e deve ser composto por números.";
                        this.txtMaxima.Text = string.Empty;
                        this.txtMaxima.Focus();
                        return;
                    }
                    

                    var capac = new RN.Entidades.CapacidaDeAlunoTurma
                                    {
                                        Ano = Convert.ToInt32(this.ddlAno.SelectedValue),
                                        Periodo = Convert.ToInt32(this.ddlPeriodo.SelectedValue),
                                        CursoId = Convert.ToString(this.tseCurso.DBValue),
                                        CapacidadeMaxima = Convert.ToInt32(this.txtMaxima.Text),
                                        CapacidadeMinima = Convert.ToInt32(this.txtMinima.Text),
                                        Matricula = this.User.Identity.Name
                                    };

                    var validacao = RN.CapacidaDeAlunoTurma.Validar(capac);

                    if (validacao.Valido)
                    {
                        RN.CapacidaDeAlunoTurma.Inserir(capac);

                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup",
                                                                     "alert('Registros gravados com sucesso.');", true);

                        this.ddlModalidade.ClearSelection();
                        this.ddlNivel.ClearSelection();
                        ddlReplicar.ClearSelection();
                        tseCurso.ResetValue();
                        //tsCurso.GridFilterParameters.Clear();
                        txtMinima.Text = string.Empty;
                        txtMaxima.Text = string.Empty;

                        this.odsCapacidade.Select();
                        this.odsCapacidade.DataBind();
                        this.grdCapacidade.DataBind();
                    }
                    else
                    {
                        this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    }
                }
                else
                {
                    var anoperiodoReplicar = ddlReplicar.SelectedValue.Split('/');

                    var capac = new DadosReplicacaoCapacidadeTurma
                    {
                        Ano = Convert.ToInt32(this.ddlAno.SelectedValue),
                        Periodo = Convert.ToInt32(this.ddlPeriodo.SelectedValue),
                        AnoReplicacao = Convert.ToDecimal(anoperiodoReplicar[0]),
                        PeriodoReplicacao = Convert.ToDecimal(anoperiodoReplicar[1]),
                        Matricula = this.User.Identity.Name
                    };

                    RN.CapacidaDeAlunoTurma.Replicar(capac);

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup",
                                                                    "alert('Registros gravados com sucesso.');", true);

                    ddlReplicar.ClearSelection();

                    this.odsCapacidade.Select();
                    this.odsCapacidade.DataBind();
                    this.grdCapacidade.DataBind();

                    ddlModalidade.Enabled = true;
                    ddlNivel.Enabled = true;
                    tseCurso.Enabled = true;
                    txtMinima.Enabled = true;
                    txtMaxima.Enabled = true;
                    btnSalvar.Visible = true;
                    btnReplicar.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
