using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.Data;
using Techne.Controls;
using Techne.Lyceum.RN;
using Seeduc.Infra.Data;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/AbsorcaoUnidadeEnsino.aspx"), ControlText("AbsorcaoUnidadeEnsino"), Title("Absorção de Unidade de Ensino")]
    public partial class AbsorcaoUnidadeEnsino : TPage
    {
        //estacia as propriedades da camada RN
        RN.AbsorcaoUnidadeEnsino Propriedades = new RN.AbsorcaoUnidadeEnsino();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string table = string.Empty;

                this.DropNivelAbsorcao.Items.Insert(0, "Selecione");
                this.DropNivelAbsorcao.Items.Insert(1, "Unidade Ensino");
                this.DropNivelAbsorcao.Items.Insert(2, "Curso");
                this.DropNivelAbsorcao.Items.Insert(3, "Turno");
                this.DropNivelAbsorcao.Items.Insert(4, "Série");
            }
            lblMensagem.Font.Size = 11;
            lblMensagem.Text = "";
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnSalvar, AcaoControle.novo);
            this.ControlaAcesso(this.grdAbosorcaoUnidadeEnsino);
        }

        protected void DropNivelAbsorcao_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToString(DropNivelAbsorcao.SelectedIndex) == "1")
            {
                DropCursoAbsorvido.Enabled = false;
                DropCursoAbsorvido.Items.Clear();
                DropTurno.Enabled = false;
                DropTurno.Items.Clear();
                DropSerie.Enabled = false;
                DropSerie.Items.Clear();

            }
            if (Convert.ToString(DropNivelAbsorcao.SelectedIndex) == "2")
            {
                DropTurno.Enabled = false;
                DropTurno.Items.Clear();
                DropSerie.Enabled = false;
                DropSerie.Items.Clear();

                DropCursoAbsorvido.Enabled = true;
                this.DropCursoAbsorvido.DataSource = RN.AbsorcaoUnidadeEnsino.ConsultarCursoAbsorvido(Convert.ToString(tseUnidade_Ensino_Origem.Value));
                this.DropCursoAbsorvido.DataBind();
                this.DropCursoAbsorvido.Items.Insert(0, "Selecione");
            }
            if (Convert.ToString(DropNivelAbsorcao.SelectedIndex) == "3")
            {
                DropTurno.Enabled = true;
                DropSerie.Enabled = false;
                DropCursoAbsorvido.Enabled = true;
                this.DropCursoAbsorvido.DataSource = RN.AbsorcaoUnidadeEnsino.ConsultarCursoAbsorvido(Convert.ToString(tseUnidade_Ensino_Origem.Value));
                this.DropCursoAbsorvido.DataBind();
                this.DropCursoAbsorvido.Items.Insert(0, "Selecione");
                CarregarTurnoAbsorvido();
            }
            if (Convert.ToString(DropNivelAbsorcao.SelectedIndex) == "4")
            {
                DropTurno.Enabled = true;
                DropSerie.Enabled = true;
                DropCursoAbsorvido.Enabled = true;
                this.DropCursoAbsorvido.DataSource = RN.AbsorcaoUnidadeEnsino.ConsultarCursoAbsorvido(Convert.ToString(tseUnidade_Ensino_Origem.Value));
                this.DropCursoAbsorvido.DataBind();
                this.DropCursoAbsorvido.Items.Insert(0, "Selecione");
                this.DropSerie.DataSource = RN.AbsorcaoUnidadeEnsino.ConsultarSerieAbsorvida(DropCursoAbsorvido.SelectedValue, DropTurno.SelectedValue, Convert.ToString(tseUnidade_Ensino_Origem.Value));
                this.DropSerie.DataBind();
                this.DropSerie.Items.Insert(0, "Selecione");
            }
        }

        protected void DropCursoAbsorvido_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToString(DropNivelAbsorcao.SelectedIndex) != "2")
            {
                this.DropTurno.DataSource = RN.AbsorcaoUnidadeEnsino.ConsultarTurnoAbsorvido(Convert.ToString(tseUnidade_Ensino_Origem.Value), DropCursoAbsorvido.SelectedValue);
                this.DropTurno.DataBind();
                this.DropTurno.Items.Insert(0, "Selecione");
                this.DropSerie.DataSource = RN.AbsorcaoUnidadeEnsino.ConsultarSerieAbsorvida(DropCursoAbsorvido.SelectedValue, DropTurno.SelectedValue, Convert.ToString(tseUnidade_Ensino_Origem.Value));
                this.DropSerie.DataBind();
                this.DropSerie.Items.Insert(0, "Selecione");
            }
        }

        protected void DropTurno_SelectedIndexChanged(object sender, EventArgs e)
        {            
            this.DropSerie.DataSource = RN.AbsorcaoUnidadeEnsino.ConsultarSerieAbsorvida(DropCursoAbsorvido.SelectedValue, DropTurno.SelectedValue, Convert.ToString(tseUnidade_Ensino_Origem.Value));
            this.DropSerie.DataBind();
            this.DropSerie.Items.Insert(0, "Selecione");            
        }

        protected void DropSerie_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                var context = DataContextBuilder.FromLyceum.UsingLock();
                if (DropNivelAbsorcao.SelectedIndex == 0)
                {
                    lblMensagem.Text = "O Campo Nível de Absorção é obrigatório.";
                }
                else if (string.IsNullOrEmpty(calendario.Text))
                {
                    lblMensagem.Text = "A data da Absorvição é obrigatória.";

                }
                else
                {
                    Propriedades.UnidadeEnsinodestinoid = Convert.ToString(tseUnidade_Ensino_Destino.Value);
                    Propriedades.UnidadeEnsinoorigemid = Convert.ToString(tseUnidade_Ensino_Origem.Value);
                    Propriedades.Cursoorigemid = DropCursoAbsorvido.SelectedValue;
                    Propriedades.Turnoorigemid = Convert.ToString(DropTurno.SelectedItem) == "Selecione" ? null : Convert.ToString(DropTurno.SelectedValue);
                    Propriedades.Serieorigemid = Convert.ToString(DropSerie.SelectedItem);
                    Propriedades.Dataabsorcao = Convert.ToDateTime(calendario.Text);
                    Propriedades.Datacadastro = DateTime.Now;
                    Propriedades.Matricula = User.Identity.Name;
                    Propriedades.Nivelabsorcaoid = Convert.ToString(DropNivelAbsorcao.SelectedIndex);

                    if (Convert.ToString(DropNivelAbsorcao.SelectedIndex) == "1")
                    {
                        if (tseUnidade_Ensino_Origem.Value != null)
                        {
                            if (RN.AbsorcaoUnidadeEnsino.ConsultaUnidadeAbsorvida(Convert.ToString(tseUnidade_Ensino_Origem.Value)) == false)
                            {
                                if (tseUnidade_Ensino_Destino.Value != null)
                                {
                                    RN.AbsorcaoUnidadeEnsino.InserirUnidadeEnsinoAbsorvida(Propriedades, context);
                                    lblMensagem.Text = "Registro Gravado com sucesso!";
                                    grdAbsorcao.Select();
                                    grdAbosorcaoUnidadeEnsino.DataBind();
                                    LimpaCampos();
                                }
                                else
                                {
                                    lblMensagem.Text = "O campo unidade de ensino destino é obrigatório!";
                                }
                            }
                            else
                            {
                                lblMensagem.Text = "A unidade de ensino já foi absorvida por completo.";
                            }
                        }
                        else
                        {
                            lblMensagem.Text = "O campo unidade de ensino é obrigatório.";
                        }
                    }
                    if (Convert.ToString(DropNivelAbsorcao.SelectedIndex) == "2")
                    {
                        if (tseUnidade_Ensino_Origem.Value != null & DropCursoAbsorvido.SelectedValue != "Selecione")
                        {
                            if (RN.AbsorcaoUnidadeEnsino.ConsultaUnidadeAbsorvida(Convert.ToString(tseUnidade_Ensino_Origem.Value)) == false)
                            {
                                if (tseUnidade_Ensino_Destino.Value != null)
                                {

                                    RN.AbsorcaoUnidadeEnsino.InserirUnidadeEnsinoAbsorvida(Propriedades, context);
                                    lblMensagem.Text = "Registro Gravado com sucesso!";
                                    grdAbsorcao.Select();
                                    grdAbosorcaoUnidadeEnsino.DataBind();
                                    LimpaCampos();
                                }
                                else
                                {
                                    lblMensagem.Text = "O campo unidade de ensino destino é obrigatório!";
                                }
                            }
                            else
                            {
                                lblMensagem.Text = "A unidade de ensino já foi absorvida por completo.";
                            }
                        }
                        else
                        {
                            if (tseUnidade_Ensino_Origem.Value == null & DropCursoAbsorvido.SelectedValue == "Selecione")
                            {
                                lblMensagem.Text = "Os campos Unidade de Ensino e Curso Absorvido são obrigatórios.";
                            }
                            else
                            {
                                if (tseUnidade_Ensino_Origem.Value == null)
                                {
                                    lblMensagem.Text = "O campo unidade de ensino é obrigatório.";
                                }
                                if (DropCursoAbsorvido.SelectedValue == "Selecione")
                                {
                                    lblMensagem.Text = "O campo Curso Absorvido é obrigatório.";
                                }
                            }
                        }
                    }
                    if (Convert.ToString(DropNivelAbsorcao.SelectedIndex) == "3")
                    {
                        if (tseUnidade_Ensino_Origem.Value != null & DropTurno.SelectedValue != "Selecione")
                        {
                            if (RN.AbsorcaoUnidadeEnsino.ConsultaUnidadeAbsorvida(Convert.ToString(tseUnidade_Ensino_Origem.Value)) == false)
                            {
                                if (tseUnidade_Ensino_Destino.Value != null)
                                {
                                    RN.AbsorcaoUnidadeEnsino.InserirUnidadeEnsinoAbsorvida(Propriedades, context);
                                    lblMensagem.Text = "Registro Gravado com sucesso!";
                                    grdAbsorcao.Select();
                                    LimpaCampos();
                                  
                                }
                                else
                                {
                                    lblMensagem.Text = "O campo unidade de ensino destino é obrigatório!";
                                }
                            }
                            else
                            {
                                lblMensagem.Text = "A unidade de ensino já foi absorvida por completo.";
                            }
                        }
                        else
                        {
                            if (tseUnidade_Ensino_Origem.Value == null & DropTurno.SelectedValue == "Selecione")
                            {
                                lblMensagem.Text = "Os campos Unidade de Ensino e Turno Absorvido são Obrigatório.";
                            }
                            if (tseUnidade_Ensino_Origem.Value == null)
                            {
                                lblMensagem.Text = "O campo Unidade de Ensino é obrigatório.";
                            }
                            if (DropTurno.SelectedValue == "Selecione")
                            {
                                lblMensagem.Text = "O campo Turno Absorvido é obrigatório.";
                            }
                        }
                    }
                    if (Convert.ToString(DropNivelAbsorcao.SelectedIndex) == "4")
                    {
                        if (tseUnidade_Ensino_Origem.Value != null & DropCursoAbsorvido.SelectedValue != "Selecione" && DropSerie.SelectedValue != "Selecione")
                        {
                            if (RN.AbsorcaoUnidadeEnsino.ConsultaUnidadeAbsorvida(Convert.ToString(tseUnidade_Ensino_Origem.Value)) == false)
                            {
                                if (tseUnidade_Ensino_Destino.Value != null)
                                {
                                    RN.AbsorcaoUnidadeEnsino.InserirUnidadeEnsinoAbsorvida(Propriedades, context);
                                    lblMensagem.Text = "Registro Gravado com sucesso!";
                                    grdAbsorcao.Select();
                                    grdAbosorcaoUnidadeEnsino.DataBind();
                                    LimpaCampos();
                                }
                                else
                                {
                                    lblMensagem.Text = "O campo unidade de ensino destino é obrigatório!";
                                }
                            }
                            else
                            {
                                lblMensagem.Text = "A unidade de ensino já foi absorvida por completo.";
                            }
                        }
                        else
                        {
                            if (tseUnidade_Ensino_Origem.Value == null & DropCursoAbsorvido.SelectedValue == "Selecione")
                            {
                                lblMensagem.Text = "Os campos Unidade de Ensino e Curso Absorvido são Obrigatório.";
                            }
                            if (tseUnidade_Ensino_Origem.Value == null)
                            {
                                lblMensagem.Text = "O campo Unidade de Ensino é obrigatório.";
                            }
                            if (DropCursoAbsorvido.SelectedValue == "Selecione")
                            {
                                lblMensagem.Text = "O campo Curso Absorvido é obrigatório.";
                            }
                            if (DropSerie.SelectedValue == "Selecione")
                            {
                                lblMensagem.Text = "O campo Série Absorvida é obrigatório.";
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                lblMensagem.Text = "falha de acesso ao banco de dados. Entre em contato com o administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.";
            }          
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdAbosorcaoUnidadeEnsino, "Absorção de Unidade de Enisno");
        }

        protected void tseUnidade_Ensino_Origem_Changed(object sender, EventArgs args)
        {
            DropNivelAbsorcao.SelectedIndex = 0;
            DropCursoAbsorvido.Enabled = false;
            DropCursoAbsorvido.Items.Clear();
            DropTurno.Enabled = false;
            DropTurno.Items.Clear();
            DropSerie.Enabled = false;
            DropSerie.Items.Clear();

            calendario.Text = string.Empty;

        }

        protected void tseUnidade_Ensino_Origem_TextChanged(object sender, EventArgs e)
        {
            if (tseUnidade_Ensino_Origem.DBValue == tseUnidade_Ensino_Destino.DBValue)
            {
                tseUnidade_Ensino_Origem.DBValue = null;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Unidade Ensino Origem", "alert('Não é possível selecionar unidades iguais')", true);
                return;
            }
        }

        protected void tseUnidade_Ensino_Destino_Changed(object sender, EventArgs args)
        {
            if (Page.IsCallback)
            {
                return;
            }

            if (!tseUnidade_Ensino_Destino.DBValue.IsNull)
            {
                PnDadosAbosorvido.Visible = true;
                grdAbosorcaoUnidadeEnsino.Visible = true;
                grdAbsorcao.DataBind();
            }
            else
            {
                PnDadosAbosorvido.Visible = false;
                grdAbosorcaoUnidadeEnsino.Visible = false;
                grdAbsorcao.DataBind();
            }
        }

        protected void tseUnidade_Ensino_Destino_Load(object sender, EventArgs e)
        {
            tseUnidade_Ensino_Destino.Enabled = true;
        }

        protected void grdAbsorcao_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var id = e.InputParameters["SERIEABSORVIDAID"].ToString();
            RN.AbsorcaoUnidadeEnsino.DeletaUnidadeAbsorvida(id);
            lblMensagem.Text = "Absorção deletada com sucesso.";
        }

        #region Métodos Privados

        private void CarregarTurnoAbsorvido()
        {
            this.DropTurno.DataSource = RN.AbsorcaoUnidadeEnsino.ConsultarTurnoAbsorvido(Convert.ToString(tseUnidade_Ensino_Origem.Value), DropCursoAbsorvido.SelectedValue != "Selecione" ? DropCursoAbsorvido.SelectedValue : string.Empty);
            this.DropTurno.DataBind();
            this.DropTurno.Items.Insert(0, "Selecione");
        }

        //private void CarregarUnidadeEnsinoOrigem()
        //{
        //    if (tseUnidade_Ensino_Destino.DBValue != null)
        //        tseUnidade_Ensino_Origem.SqlWhere = " unidade_ens <> #tseUnidade_Ensino_Destino# ";
        //}
        private void LimpaCampos()
        {
            grdAbosorcaoUnidadeEnsino.DataBind();
            tseUnidade_Ensino_Origem.ResetValue();
            DropNivelAbsorcao.SelectedIndex = 0;
            DropCursoAbsorvido.Enabled = false;
            DropCursoAbsorvido.Items.Clear();
            DropTurno.Enabled = false;
            DropTurno.Items.Clear();
            DropSerie.Enabled = false;
            DropSerie.Items.Clear();

            calendario.Text = string.Empty;
        }
        #endregion
    }
}
