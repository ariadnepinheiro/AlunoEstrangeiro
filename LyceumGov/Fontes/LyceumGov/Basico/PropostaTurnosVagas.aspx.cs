using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxGridView;
using Techne.Lyceum.RN;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/PropostaTurnosVagas.aspx"),
     ControlText("Proposta de Turnos e Vagas"),
     Title("Proposta de Turnos e Vagas")]
    public partial class PropostaTurnosVagas : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;

            if (!IsPostBack)
            {
                this.CarregaAno();
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdProposta, "Proposta de Turnos e Vagas");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdProposta);
        }

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                pnlProposta.Visible = false;

                var sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (sessao != null)
                {
                    if (!this.tseRegional.DBValue.IsNull)
                    {
                        if (this.tseRegional.IsValidDBValue)
                        {
                            sessao.Regional = Convert.ToString(tseRegional.DBValue);
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;

                            tseMunicipio.ResetValue();
                            tseUnidadeResponsavel.ResetValue();
                        }
                        else
                        {
                            sessao.Regional = string.Empty;
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;
                        }
                    }
                    else
                    {
                        sessao.Regional = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                        tseMunicipio.ResetValue();
                        tseUnidadeResponsavel.ResetValue();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
            }
        }

        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }
                pnlProposta.Visible = false;

                var sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (sessao != null)
                {
                    if (!this.tseMunicipio.DBValue.IsNull)
                    {
                        if (this.tseMunicipio.IsValidDBValue)
                        {
                            sessao.Municipio = Convert.ToString(tseMunicipio.DBValue);
                            sessao.Escola = string.Empty;
                        }
                        else
                        {
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;
                            tseUnidadeResponsavel.ResetValue();
                        }
                    }
                    else
                    {
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                        tseUnidadeResponsavel.ResetValue();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CarregaProposta();
        }

        protected void CarregaProposta()
        {
            //Valida se está preeenchido ano e escola
            if (!this.tseUnidadeResponsavel.DBValue.IsNull && this.tseUnidadeResponsavel.IsValidDBValue
                && !this.ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace())
            {
                pnlProposta.Visible = true;
                grdProposta.DataBind();
            }
            else 
            {
                lblMensagem.Text = "Favor informar ano e unidade de ensino.";
            }
        }

        protected void CarregaAno()
        {
            try
            {
                ddlAno.DataSource = RN.PeriodoLetivo.ConsultarAno();
                ddlAno.DataBind();
                ddlAno.DataTextField = "ANO";
                ddlAno.DataValueField = "ANO";
                ListItem item = new ListItem("Selecione", "");
                ddlAno.Items.Insert(0, item);
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
                if (this.Page.IsCallback)
                {
                    return;
                }
                pnlProposta.Visible = false;

                var sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (!this.tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (this.tseUnidadeResponsavel.IsValidDBValue)
                    {
                        CarregaProposta();

                        if (!this.tseUnidadeResponsavel["unidade_ens"].IsNull)
                        {
                            this.tseRegional.Value = this.tseUnidadeResponsavel["id_regional"];
                            this.tseMunicipio.Value = this.tseUnidadeResponsavel["municipio"];
                        }

                        if (sessao != null)
                        {
                            sessao.Escola = Convert.ToString(this.tseUnidadeResponsavel.DBValue);
                            sessao.Regional = Convert.ToString(this.tseRegional.DBValue);
                            sessao.Municipio = Convert.ToString(this.tseMunicipio.DBValue);
                        }

                    }
                    else
                    {
                        lblMensagem.Text = "Unidade de ensino não encontrada.";
                        if (sessao != null)
                        {
                            sessao.Escola = string.Empty;
                            sessao.Municipio = string.Empty;
                            sessao.Regional = string.Empty;
                        }
                    }
                }
                else
                {
                    if (sessao != null)
                    {
                        sessao.Escola = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Regional = string.Empty;
                    }
                    lblMensagem.Text = "Favor consultar uma unidade de ensino.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public object Lista(object ano, object censo)
        {
            RN.CtvPropostaSeeduc rnCtvPropostaSeeduc = new Techne.Lyceum.RN.CtvPropostaSeeduc();
            if (censo != null && ano != null && Convert.ToInt32(ano) > 0)
            {
                return rnCtvPropostaSeeduc.ListaPor(Convert.ToString(censo), Convert.ToInt32(ano));
            }

            return null;
        }

        public object ListaCurso()
        {
            return RN.Curso.ConsultarDetalhesCurso();
        }

        public void Insert(object ANO, object PERIODO, object MODALIDADE, object NIVEL, object CURSO, object NOME, object SERIE, object VAGAS_CONTINUIDADE, object VAGAS_NOVAS, object TAXAREPROVACAO) { }

        public void Update(object ANO, object PERIODO, object MODALIDADE, object NIVEL, object CURSO, object NOME, object SERIE, object VAGAS_CONTINUIDADE, object VAGAS_NOVAS, object TAXAREPROVACAO, object ID_PROPOSTA_SEEDUC) { }

        protected void grdProposta_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdProposta);
        }

        protected void grdProposta_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdProposta.Settings.ShowFilterRow = false;
        }

        protected void grdProposta_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdProposta.Settings.ShowFilterRow = false;
        }

        protected void grdProposta_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdProposta.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "ID_PROPOSTA_SEEDUC")
                {
                    e.Editor.Enabled = true;
                }
            }
            else if (grdProposta.IsEditing)
            {
                if ((e.Column.FieldName) == "ID_PROPOSTA_SEEDUC")
                {
                    e.Editor.Enabled = false;
                }                
                if ((e.Column.FieldName) == "CURSO")
                {
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "NOME")
                {
                    e.Editor.ReadOnly = true;
                }
                if ((e.Column.FieldName) == "SERIE")
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
            }

            if (e.Column.FieldName == "SERIE")
            {
                var combo = e.Editor as ASPxComboBox;

                if (combo == null)
                {
                    return;
                }

                combo.Callback += this.cmbCurso_OnCallback;
            }

            if (this.grdProposta.IsEditing
                && e.Column.FieldName == "SERIE"
                && e.KeyValue != DBNull.Value
                && e.KeyValue != null)
            {
                var val = this.grdProposta.GetRowValuesByKeyValue(e.KeyValue, "CURSO");

                if (val == DBNull.Value)
                {
                    return;
                }

                var curso = (string)val;
                var combo = e.Editor as ASPxComboBox;

                if (combo == null)
                {
                    return;
                }

                this.CarregarSeries(combo, curso);
            }            
        }

        private void cmbCurso_OnCallback(object source, CallbackEventArgsBase e)
        {
            this.CarregarSeries(source as ASPxComboBox, e.Parameter);
        }

        private void CarregarSeries(ASPxComboBox cmbSerie, string curso)
        {
            if (string.IsNullOrEmpty(curso))
            {
                return;
            }

            cmbSerie.Items.Clear();
            cmbSerie.TextField = "SERIE";
            cmbSerie.ValueField = "SERIE";
            cmbSerie.DataSource = RN.Serie.ListarSeries(curso);
            cmbSerie.DataBind();
        }

        protected void grdProposta_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.DTOs.DadosPropostaTurnosVagas dadosProposta = new Techne.Lyceum.RN.DTOs.DadosPropostaTurnosVagas();
            RN.CtvPropostaSeeduc rnCtvPropostaSeeduc = new CtvPropostaSeeduc();

            dadosProposta.Ano = e.NewValues["ANO"] != null ? Convert.ToInt32(e.NewValues["ANO"]) : -1;
            dadosProposta.Periodo = e.NewValues["PERIODO"] != null ? Convert.ToInt32(e.NewValues["PERIODO"]) : -1;
            dadosProposta.Censo = !this.tseUnidadeResponsavel.DBValue.IsNull && this.tseUnidadeResponsavel.IsValidDBValue ? tseUnidadeResponsavel.DBValue.ToString() : null;
            dadosProposta.Curso = e.NewValues["CURSO"] != null ? e.NewValues["CURSO"].ToString() : null;
            dadosProposta.Serie = e.NewValues["SERIE"] != null ? Convert.ToInt32(e.NewValues["SERIE"]) : -1;
            dadosProposta.VagasContinuidade = e.NewValues["VAGAS_CONTINUIDADE"] != null ? Convert.ToInt32(e.NewValues["VAGAS_CONTINUIDADE"]) : -1;
            dadosProposta.VagasNovas = e.NewValues["VAGAS_NOVAS"] != null ? Convert.ToInt32(e.NewValues["VAGAS_NOVAS"]) : -1;
            dadosProposta.TaxaReprovacao = e.NewValues["TAXAREPROVACAO"] != null ? Convert.ToDecimal(e.NewValues["TAXAREPROVACAO"]) : -1;
            dadosProposta.Matricula = User.Identity.Name;

            validacao = rnCtvPropostaSeeduc.Valida(dadosProposta, true);

            if (validacao.Valido)
            {
                rnCtvPropostaSeeduc.Insere(dadosProposta);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdProposta.DataBind();
        }

        protected void grdProposta_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.CtvPropostaSeeduc rnCtvPropostaSeeduc = new CtvPropostaSeeduc();
            RN.DTOs.DadosPropostaTurnosVagas dadosProposta = new Techne.Lyceum.RN.DTOs.DadosPropostaTurnosVagas();

            dadosProposta.IdPropostaSeeduc = e.Keys["ID_PROPOSTA_SEEDUC"] != null ? Convert.ToInt32(e.Keys["ID_PROPOSTA_SEEDUC"]) : -1;
            dadosProposta.Ano = e.NewValues["ANO"] != null ? Convert.ToInt32(e.NewValues["ANO"]) : -1;
            dadosProposta.Periodo = e.NewValues["PERIODO"] != null ? Convert.ToInt32(e.NewValues["PERIODO"]) : -1;
            dadosProposta.Censo = !this.tseUnidadeResponsavel.DBValue.IsNull && this.tseUnidadeResponsavel.IsValidDBValue ? tseUnidadeResponsavel.DBValue.ToString() : null;
            dadosProposta.Curso = e.NewValues["CURSO"] != null ? e.NewValues["CURSO"].ToString().Trim().ToUpper() : null;
            dadosProposta.Serie = e.NewValues["SERIE"] != null ? Convert.ToInt32(e.NewValues["SERIE"]) : -1;
            dadosProposta.VagasContinuidade = e.NewValues["VAGAS_CONTINUIDADE"] != null ? Convert.ToInt32(e.NewValues["VAGAS_CONTINUIDADE"]) : -1;
            dadosProposta.VagasNovas = e.NewValues["VAGAS_NOVAS"] != null ? Convert.ToInt32(e.NewValues["VAGAS_NOVAS"]) : -1;
            dadosProposta.TaxaReprovacao = e.NewValues["TAXAREPROVACAO"] != null ? Convert.ToDecimal(e.NewValues["TAXAREPROVACAO"]) : -1;
            dadosProposta.Matricula = User.Identity.Name;

            validacao = rnCtvPropostaSeeduc.Valida(dadosProposta, false);

            if (validacao.Valido)
            {
                rnCtvPropostaSeeduc.Atualiza(dadosProposta);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdProposta.DataBind();
        }
    }
}
