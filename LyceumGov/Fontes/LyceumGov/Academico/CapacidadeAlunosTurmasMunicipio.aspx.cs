using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.DTOs;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/CapacidadeAlunosTurmasMunicipio.aspx"),
    ControlText("CapacidadeAlunosTurmasMunicipio"),
    Title("Capacidade de Alunos em Turma por Município"),]
    public partial class CapacidadeAlunosTurmasMunicipio : TPage
    {
        public object RetornarCapacidadeAlunoMunicipioPor(object ano, object periodo)
        {
            if ((ano != null && ano.ToString() != "Selecione")
                && (periodo != null && periodo.ToString() != "Selecione"))
            {
                return RN.CapacidadeAlunoTurmaMunicipio.RetornarCapacidadeAlunoMunicipioPor(Convert.ToDecimal(ano), Convert.ToDecimal(periodo));
            }

            return null;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdCapacidade, "Capacidade de Alunos em Turma por Município");
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

                CarregaDdlUf();
            }
        }

        private void CarregaDdlUf()
        {
            ddlUf.DataSource = RN.Municipio.ListarUF();
            ddlUf.SelectedValue = "RJ";
            ddlUf.DataBind();

            tseMunicipio.ResetValue();
            if (ddlUf.SelectedValue != "Selecione")
            {
                tseMunicipio.SqlWhere = " UF_SIGLA = '" + ddlUf.SelectedValue + "'";
            }
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlPeriodo.Items.Clear();
            ddlReplicar.Items.Clear();
            
            grdCapacidade.Visible = false;
            pnlGerais.Visible = false;
            txtCapacidade.Text = string.Empty;
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
            grdCapacidade.Visible = false;
            pnlGerais.Visible = false;
            txtCapacidade.Text = string.Empty;
            btnSalvar.Visible = true;
            btnReplicar.Visible = false;

            if (ddlPeriodo.SelectedValue != "Selecione")
            {
                grdCapacidade.Visible = true;
                pnlGerais.Visible = true;

                this.ddlReplicar.DataSource = RN.CapacidadeAlunoTurmaMunicipio.ListarAnoPeriodoReplicacao(int.Parse(ddlAno.SelectedValue), int.Parse(ddlPeriodo.SelectedValue));
                this.ddlReplicar.Items.Insert(0, "Nenhum");
                this.ddlReplicar.DataBind();
            }
        }

        protected void ddlUf_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCapacidade.Text = string.Empty;
            tseMunicipio.ResetValue();
            if (ddlUf.SelectedValue != "Selecione")
            {
                tseMunicipio.SqlWhere = " UF_SIGLA = '" + ddlUf.SelectedValue + "'";
            }
        }

        protected void tseMunicipio_Changed(object sender, EventArgs args)
        {
            if (tseMunicipio.IsValidDBValue
                && !tseMunicipio.DBValue.IsNull)
            {
                ddlUf.SelectedValue = tseMunicipio["uf_sigla"].ToString();
            }
        }

        protected void ddlReplicar_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlUf.ClearSelection();
            txtCapacidade.Text = string.Empty;

            if (ddlReplicar.SelectedValue != "Nenhum")
            {
                ddlUf.Enabled = false;
                ddlTipo.Enabled = false;
                txtCapacidade.Enabled = false;
                btnSalvar.Visible = false;
                tseMunicipio.Enabled = false;
                btnReplicar.Visible = true;
            }
            else
            {
                ddlUf.Enabled = true;
                ddlTipo.Enabled = true;
                txtCapacidade.Enabled = true;
                tseMunicipio.Enabled = true;
                btnSalvar.Visible = true;
                btnReplicar.Visible = false;
            }
        }

        private void ValidarCampos()
        {
            this.txtCapacidade.Attributes.Add("onkeyPress", "return onlyNumbers(event, this);");
            this.txtCapacidade.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
            this.txtCapacidade.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");
        }

        protected void grdCapacidade_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdCapacidade);
        }

        protected void grdCapacidade_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdCapacidade.Settings.ShowFilterRow = false;
        }

        protected void odsCapacidade_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var id = e.InputParameters["CAPACIDADEID"].ToString();

            RN.CapacidadeAlunoTurmaMunicipio.Remove(int.Parse(id));
        }

        protected void odsCapacidade_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var validacao = new ValidacaoDados();

            string[] anoperiodo = e.InputParameters["ANOPERIODO"].ToString().Split('/');

            int tipo = int.MinValue;

         
            if ((e.InputParameters["TIPO"]).Equals("MÍNIMA"))
            {
                tipo = 0;
            }
            else
            {
                tipo = 1;
            }

            var capac = new RN.Entidades.CapacidadeAlunoTurmaMunicipio
            {
                CapacidadeAlunoTurmaMunicipioId = (e.InputParameters["CAPACIDADEID"] != null) ? Convert.ToInt32(e.InputParameters["CAPACIDADEID"]) : -1,
                Capacidade = (e.InputParameters["CAPACIDADE"] != null) ? Convert.ToInt32(e.InputParameters["CAPACIDADE"]) : -1,
                Ano = Convert.ToInt32(anoperiodo[0]),
                Periodo = Convert.ToInt32(anoperiodo[1]),
                Matricula = this.User.Identity.Name,
                MunicipioId = (e.InputParameters["CODMUNICIPIO"] != null) ? Convert.ToString(e.InputParameters["CODMUNICIPIO"]) : string.Empty,
                Tipo = tipo,
            };

            validacao = RN.CapacidadeAlunoTurmaMunicipio.Validar(capac);

            if (!validacao.Valido)
            {
                throw new Exception(validacao.Mensagem);
            }

            RN.CapacidadeAlunoTurmaMunicipio.Atualiza(capac);
        }

        public void Update(object CAPACIDADEID, object ANOPERIODO, object UFSIGLA, object CODMUNICIPIO, object NOME, object TIPO, object CAPACIDADE, object DATAALTERACAO)
        {
            
        }

        public void Delete(object CAPACIDADEID, object CODMUNICIPIO)
        {
            //this.LimparCampos();
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlReplicar.SelectedValue == "Nenhum")
                {
                    long resultado;

                    if (!long.TryParse(this.txtCapacidade.Text, out resultado))
                    {
                        this.lblMensagem.Text = "O campo Capacidade deve ser composto por números.";
                        this.txtCapacidade.Text = string.Empty;
                        this.txtCapacidade.Focus();
                        return;
                    }

                    var capac = new RN.Entidades.CapacidadeAlunoTurmaMunicipio
                    {
                        Ano = Convert.ToInt32(this.ddlAno.SelectedValue),
                        Periodo = Convert.ToInt32(this.ddlPeriodo.SelectedValue),
                        Capacidade = Convert.ToInt32(this.txtCapacidade.Text),
                        Matricula = this.User.Identity.Name,
                        MunicipioId = Convert.ToString(tseMunicipio.Value),
                        Tipo = Convert.ToInt32(ddlTipo.SelectedItem.Value)
                    };

                    var validacao = RN.CapacidadeAlunoTurmaMunicipio.Validar(capac);

                    if (validacao.Valido)
                    {
                        RN.CapacidadeAlunoTurmaMunicipio.Grava(capac);

                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup",
                                                                     "alert('Registros gravados com sucesso.');", true);

                        this.ddlUf.ClearSelection();
                        ddlReplicar.ClearSelection();
                        txtCapacidade.Text = string.Empty;

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

                    RN.CapacidadeAlunoTurmaMunicipio.Replicar(capac);

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup",
                                                                    "alert('Registros gravados com sucesso.');", true);

                    ddlReplicar.ClearSelection();

                    this.odsCapacidade.Select();
                    this.odsCapacidade.DataBind();
                    this.grdCapacidade.DataBind();

                    ddlUf.Enabled = true;
                    txtCapacidade.Enabled = true;
                    btnSalvar.Visible = true;
                    btnReplicar.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
            LimparCampos();
        }

        private void LimparCampos()
        {
            ddlReplicar.ClearSelection();
            CarregaDdlUf();
            tseMunicipio.ResetValue();
            ddlTipo.ClearSelection();
            txtCapacidade.Text = string.Empty;

            this.odsCapacidade.Select();
            this.odsCapacidade.DataBind();
            this.grdCapacidade.DataBind();
        }
    }
}
