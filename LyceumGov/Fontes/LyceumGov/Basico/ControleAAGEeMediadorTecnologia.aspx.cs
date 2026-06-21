using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Techne.Web;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.Data;
using Techne.Lyceum.RN.AAGE.Entidades;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN;
using Techne.Lyceum.Net.Util;
using Techne.Lyceum.RN.DTOs;
using DevExpress.Web.ASPxEditors;
using System.Collections.Generic;

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/ControleAAGEeMediadorTecnologia.aspx")]
    [ControlText("ControleAAGEeMediadorTecnologia")]
    [Title("Controle AAGE e Mediador Tecnologia")]
    public partial class ControleAAGEeMediadorTecnologia : TPage
    {
        public ControleAAGEeMediadorTecnologia()
            : base(typeof(ControleAAGEeMediadorTecnologia))
        {

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
            lblMensagem.Text = string.Empty;

            if (!this.Page.IsPostBack)
            {
                this.BuscaPerfis();
            }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void BuscaPerfis()
        {
            List<string> perfis = new List<string>();
            RN.Perfil rnPerfil = new Perfil();

            //Caso seja privilegiado acrescenta informaçao
            if (RN.Usuarios.UsuarioPrivilegiado(HttpContext.Current.User.Identity.Name))
            {
                 hdnPerfil.Value = "PRIVILEGIADO";
                 return;
            }
            
            //Verifica perfis do usuario
            perfis = rnPerfil.ListaDescricoesPerfisPor(User.Identity.Name);
            if (perfis.Count > 0)
            {
                hdnPerfil.Value = perfis.Aggregate((a, b) => a + ", " + b);
            }
        }

        public object Listar(object docente)
        {
            RN.AAGE.DocenteMediadorUnidadeEnsino rnDocenteMediadorUnidadeEnsino = new Techne.Lyceum.RN.AAGE.DocenteMediadorUnidadeEnsino();
            if (docente != null)
            {
                return rnDocenteMediadorUnidadeEnsino.ListaVinculoDocenteMediadorUnidadeEnsinoPor(Convert.ToDecimal(docente.ToString()));
            }

            return null;
        }
        public object ListarArticulador(object docente)
        {
            RN.AAGE.DocenteArticuladorRegional rnDocenteArticuladorRegional = new Techne.Lyceum.RN.AAGE.DocenteArticuladorRegional();
            if (docente != null)
            {
                return rnDocenteArticuladorRegional.ListaVinculoDocenteArticuladorRegionalPor(Convert.ToDecimal(docente.ToString()));
            }

            return null;
        }
        public object ListarAAGE(object docente)
        {
            RN.AAGE.DocenteAAGEUnidadeEnsino rnDocenteAAGEUnidadeEnsino = new Techne.Lyceum.RN.AAGE.DocenteAAGEUnidadeEnsino();
            if (docente != null)
            {
                return rnDocenteAAGEUnidadeEnsino.ListaVinculoDocenteAAGEUnidadeEnsinoPor(Convert.ToDecimal(docente.ToString()));
            }

            return null;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdMediador, "Mediador de Tecnologia");
            TituloGrid(this.grdArticulador, "Mediador Articulador");
            TituloGrid(this.grdAAGE, "AAGE");
            TituloGrid(this.grdLotacao, "");
        }
        protected void tseDocentes_Changed(object sender, EventArgs args)
        {
            try
            {
                RN.Docentes rnDocente = new Techne.Lyceum.RN.Docentes();
                DadosPessoaisAAGEMediador dadosDocente = new DadosPessoaisAAGEMediador();
                DataTable dtLotacao = new DataTable();
                Lotacao rnLotacao = new Lotacao();
                LimparCampos();
                if (!string.IsNullOrEmpty(tseDocentes.DBValue.ToString()))
                {
                    if (tseDocentes.IsValidDBValue)
                    {
                        apcDocente.Visible = true;                     
                        dadosDocente = rnDocente.ObtemDadosPessoaisAAGEMediadorPor(Convert.ToDecimal(tseDocentes["num_func"]));

                        if (dadosDocente.DocenteId != 0)
                        {
                            lblDocente.Text = dadosDocente.DocenteId.ToString();
                            lblNome.Text = dadosDocente.NomeCompleto;
                            lblCPF.Text = dadosDocente.Cpf;
                            lblDataNascimento.Text = dadosDocente.DataNascimento.ToShortDateString();
                            lblSexo.Text = dadosDocente.Sexo;
                            lblEstadoCivil.Text = dadosDocente.EstadoCivl;
                            lblEndereco.Text = dadosDocente.Endereco;
                            lblNumero.Text = dadosDocente.Numero;
                            lblComplemento.Text = dadosDocente.Complemento;
                            lblBairro.Text = dadosDocente.Bairro;
                            lblCEP.Text = dadosDocente.Cep;
                            lblMunicipio.Text = dadosDocente.Municipio;
                            lblTelefone.Text = dadosDocente.Telefone;

                            dtLotacao = rnLotacao.ObtemListaLotacaoDocenteAtivaPor(Convert.ToInt32(tseDocentes["pessoa"]));

                            if (dtLotacao.Rows.Count > 0)
                            {
                                grdLotacao.DataSource = dtLotacao;
                                grdLotacao.DataBind();
                            }
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "Docente não cadastrado ou usuário sem permissão de visualização do Docente(Lotação)..";
                        apcDocente.Visible = false;
                    }
                }
                else
                {
                    lblMensagem.Text = "Docente não cadastrado ou usuário sem permissão de visualização do Docente(Lotação)..";
                    apcDocente.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimparCampos()
        {
            lblDocente.Text = string.Empty;
            lblNome.Text = string.Empty;
            lblCPF.Text = string.Empty;
            lblDataNascimento.Text = string.Empty;
            lblSexo.Text = string.Empty;
            lblEstadoCivil.Text = string.Empty;
            lblEndereco.Text = string.Empty;
            lblNumero.Text = string.Empty;
            lblComplemento.Text = string.Empty;
            lblBairro.Text = string.Empty;
            lblCEP.Text = string.Empty;
            lblMunicipio.Text = string.Empty;
            lblTelefone.Text = string.Empty;
            grdLotacao.DataSource = null;
            grdLotacao.DataBind();

        }

        protected void imgMediador_Load(object sender, EventArgs e)
        {
            ASPxImage img = sender as ASPxImage;

            if (!hdnPerfil.Value.Contains("SUPED") && !hdnPerfil.Value.Contains("PRIVILEGIADO"))
            {
                img.Visible = false;
            }
        }

        protected void imgAAGE_Load(object sender, EventArgs e)
        {
            ASPxImage img = sender as ASPxImage;

            if (!hdnPerfil.Value.Contains("SUPGE") && !hdnPerfil.Value.Contains("PRIVILEGIADO"))
            {
                img.Visible = false;
            }
        }

        protected void grdMediador_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (!hdnPerfil.Value.Contains("SUPED") && !hdnPerfil.Value.Contains("PRIVILEGIADO"))
            {
                e.Visible = false;
            }
        }
        protected void grdMediador_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdMediador.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "MATRICULA")
                {
                    e.Editor.ReadOnly = false;
                }
                if ((e.Column.FieldName) == "TIPOFUNCAO")
                {
                    e.Editor.Value = TipoFuncao.EnumTipoFuncao.MediadorDeTecnologia.GetStringValue();
                }
            }

            else if (grdMediador.IsEditing)
            {
                if ((e.Column.FieldName) == "TIPOFUNCAO")
                {
                    e.Editor.ReadOnly = true;
                }
            }
        }

        protected void grdMediador_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {

            if (grdMediador.IsEditing && !grdMediador.IsNewRowEditing)
            {
                TSearchBox tseUnidadeEnsino = (TSearchBox)grdMediador.FindEditFormTemplateControl("tseFuncao");

                if (tseUnidadeEnsino != null)
                {
                    tseUnidadeEnsino.Mode = Techne.Controls.ControlMode.View;
                }
            }
        }

        protected void grdMediador_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            DocenteMediadorUnidadeEnsino mediador = new DocenteMediadorUnidadeEnsino();
            RN.AAGE.DocenteMediadorUnidadeEnsino rnDocenteMediadorUnidadeEnsino = new Techne.Lyceum.RN.AAGE.DocenteMediadorUnidadeEnsino();
            ValidacaoDados validacao = new ValidacaoDados();

            mediador.DocenteId = Convert.ToDecimal(tseDocentes["num_func"].ToString());
            mediador.DataInicioVinculo = Convert.ToDateTime(e.NewValues["DATAINICIO_VINCULO"]);
            mediador.DataFimVinculo = Convert.ToDateTime(e.NewValues["DATAFIM_VINCULO"]);
            mediador.UsuarioId = User.Identity.Name;
            mediador.UnidadeEnsinoId = Convert.ToString(e.NewValues["UNIDADEENSINOID"]);

            validacao = rnDocenteMediadorUnidadeEnsino.Valida(mediador);

            if (validacao.Valido)
            {
                rnDocenteMediadorUnidadeEnsino.Insere(mediador);
                grdMediador.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void grdMediador_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            DocenteMediadorUnidadeEnsino mediador = new DocenteMediadorUnidadeEnsino();
            RN.AAGE.DocenteMediadorUnidadeEnsino rnDocenteMediadorUnidadeEnsino = new Techne.Lyceum.RN.AAGE.DocenteMediadorUnidadeEnsino();
            ValidacaoDados validacao = new ValidacaoDados();

            mediador.DocenteId = Convert.ToDecimal(tseDocentes["num_func"].ToString());
            mediador.DataInicioVinculo = Convert.ToDateTime(e.NewValues["DATAINICIO_VINCULO"]);
            mediador.DataFimVinculo = Convert.ToDateTime(e.NewValues["DATAFIM_VINCULO"]);
            mediador.UsuarioId = User.Identity.Name;
            mediador.UnidadeEnsinoId = Convert.ToString(e.NewValues["UNIDADEENSINOID"]);
            mediador.DocenteMediadorUnidadeEnsinoId = int.Parse(e.Keys["DOCENTEMEDIADOR_UNIDADEENSINO_ID"].ToString());

            validacao = rnDocenteMediadorUnidadeEnsino.Valida(mediador);

            if (validacao.Valido)
            {
                rnDocenteMediadorUnidadeEnsino.Atualiza(mediador);
                grdMediador.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void grdMediador_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            int chave = -1;
            RN.AAGE.DocenteMediadorUnidadeEnsino rnDocenteMediadorUnidadeEnsino = new Techne.Lyceum.RN.AAGE.DocenteMediadorUnidadeEnsino();

            chave = int.Parse(e.Keys["DOCENTEMEDIADOR_UNIDADEENSINO_ID"].ToString());

            rnDocenteMediadorUnidadeEnsino.Remove(chave);

            grdMediador.DataBind();

        }

        protected void grdMediador_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (e.NewValues["UNIDADEENSINOID"] == null)
            {
                e.RowError = "Favor informar a Unidade de Ensino.";
                return;
            }

            if (e.NewValues["DATAINICIO_VINCULO"] == null)
            {
                e.RowError = "Necessário informar data de início.";
                return;
            }
            if (e.NewValues["DATAFIM_VINCULO"] == null)
            {
                e.RowError = "Necessário informar data de fim.";
                return;
            }

            DateTime hoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            object datainicio = e.NewValues["DATAINICIO_VINCULO"];
            DateTime dataini = Convert.ToDateTime(datainicio);

            //Verifica se a data incio da situacao é maior que a data atual
            if (dataini != null && dataini > hoje)
            {
                e.RowError = "Data Início não pode ser maior que hoje.";
                return;
            }
        }

        protected void grdMediador_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdMediador.Settings.ShowFilterRow = false;
        }

        public void Update(object TIPOFUNCAO, object DATAINICIO_VINCULO, object DATAFIM_VINCULO, object UNIDADEENSINOID, object DOCENTEMEDIADOR_UNIDADEENSINO_ID)
        {
        }

        public void Insert(object TIPOFUNCAO, object DATAINICIO_VINCULO, object DATAFIM_VINCULO, object UNIDADEENSINOID)
        {
        }

        public void Delete(object DOCENTEMEDIADOR_UNIDADEENSINO_ID) { }


        protected void grdArticulador_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdArticulador.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "MATRICULA")
                {
                    e.Editor.ReadOnly = false;
                }
                if ((e.Column.FieldName) == "TIPOFUNCAO")
                {
                    e.Editor.Value = TipoFuncao.EnumTipoFuncao.MediadorArticulador.GetStringValue();
                }
            }

            else if (grdArticulador.IsEditing)
            {
                if ((e.Column.FieldName) == "TIPOFUNCAO")
                {
                    e.Editor.ReadOnly = true;
                }
            }
        }
        protected void grdArticulador_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (!hdnPerfil.Value.Contains("SUPED") && !hdnPerfil.Value.Contains("PRIVILEGIADO"))
            {
                e.Visible = false;
            }
        }
        protected void grdArticulador_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {

            if (grdArticulador.IsEditing && !grdArticulador.IsNewRowEditing)
            {
                TSearchBox tseUnidadeEnsino = (TSearchBox)grdArticulador.FindEditFormTemplateControl("tseFuncao");

                if (tseUnidadeEnsino != null)
                {
                    tseUnidadeEnsino.Mode = Techne.Controls.ControlMode.View;
                }
            }
        }

        protected void grdArticulador_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            DocenteArticuladorRegional articulador = new DocenteArticuladorRegional();
            RN.AAGE.DocenteArticuladorRegional rnDocenteArticuladorRegional = new Techne.Lyceum.RN.AAGE.DocenteArticuladorRegional();
            ValidacaoDados validacao = new ValidacaoDados();

            articulador.DocenteId = Convert.ToDecimal(tseDocentes["num_func"].ToString());
            articulador.DataInicioVinculo = Convert.ToDateTime(e.NewValues["DATAINICIO_VINCULO"]);
            articulador.DataFimVinculo = Convert.ToDateTime(e.NewValues["DATAFIM_VINCULO"]);
            articulador.UsuarioId = User.Identity.Name;
            articulador.RegionalId = Convert.ToInt32(e.NewValues["REGIONALID"]);

            validacao = rnDocenteArticuladorRegional.Valida(articulador);

            if (validacao.Valido)
            {
                rnDocenteArticuladorRegional.Insere(articulador);
                grdArticulador.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void grdArticulador_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            DocenteArticuladorRegional articulador = new DocenteArticuladorRegional();
            RN.AAGE.DocenteArticuladorRegional rnDocenteArticuladorRegional = new Techne.Lyceum.RN.AAGE.DocenteArticuladorRegional();
            ValidacaoDados validacao = new ValidacaoDados();

            articulador.DocenteId = Convert.ToDecimal(tseDocentes["num_func"].ToString());
            articulador.DataInicioVinculo = Convert.ToDateTime(e.NewValues["DATAINICIO_VINCULO"]);
            articulador.DataFimVinculo = Convert.ToDateTime(e.NewValues["DATAFIM_VINCULO"]);
            articulador.UsuarioId = User.Identity.Name;
            articulador.RegionalId = Convert.ToInt32(e.NewValues["REGIONALID"]);
            articulador.DocenteArticuladorRegionalId = int.Parse(e.Keys["DOCENTEARTICULADOR_REGIONAL_ID"].ToString());

            validacao = rnDocenteArticuladorRegional.Valida(articulador);

            if (validacao.Valido)
            {
                rnDocenteArticuladorRegional.Atualiza(articulador);
                grdArticulador.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void grdArticulador_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            int chave = -1;
            RN.AAGE.DocenteArticuladorRegional rnDocenteArticuladorRegional = new Techne.Lyceum.RN.AAGE.DocenteArticuladorRegional();

            chave = int.Parse(e.Keys["DOCENTEARTICULADOR_REGIONAL_ID"].ToString());

            rnDocenteArticuladorRegional.Remove(chave);

            grdArticulador.DataBind();

        }

        protected void grdArticulador_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (e.NewValues["REGIONALID"] == null)
            {
                e.RowError = "Favor informar a Regional.";
                return;
            }

            if (e.NewValues["DATAINICIO_VINCULO"] == null)
            {
                e.RowError = "Necessário informar data de início.";
                return;
            }
            if (e.NewValues["DATAFIM_VINCULO"] == null)
            {
                e.RowError = "Necessário informar data de fim.";
                return;
            }

            DateTime hoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            object datainicio = e.NewValues["DATAINICIO_VINCULO"];
            DateTime dataini = Convert.ToDateTime(datainicio);

            //Verifica se a data incio da situacao é maior que a data atual
            if (dataini != null && dataini > hoje)
            {
                e.RowError = "Data Início não pode ser maior que hoje.";
                return;
            }
        }

        protected void grdArticulador_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdArticulador.Settings.ShowFilterRow = false;
        }

        public void UpdateArticulador(object TIPOFUNCAO, object DATAINICIO_VINCULO, object DATAFIM_VINCULO, object REGIONALID, object DOCENTEARTICULADOR_REGIONAL_ID)
        {
        }

        public void InsertArticulador(object TIPOFUNCAO, object DATAINICIO_VINCULO, object DATAFIM_VINCULO, object REGIONALID)
        {
        }

        public void DeleteArticulador(object DOCENTEARTICULADOR_REGIONAL_ID) { }

        protected void grdAAGE_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdAAGE.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "MATRICULA")
                {
                    e.Editor.ReadOnly = false;
                }
                if ((e.Column.FieldName) == "TIPOFUNCAO")
                {
                    e.Editor.Value = TipoFuncao.EnumTipoFuncao.AAGE.GetStringValue();
                }
            }

            else if (grdAAGE.IsEditing)
            {
                if ((e.Column.FieldName) == "TIPOFUNCAO")
                {
                    e.Editor.ReadOnly = true;
                }
            }
        }

        protected void grdAAGE_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {

            if (grdAAGE.IsEditing && !grdAAGE.IsNewRowEditing)
            {
                TSearchBox tseUnidadeEnsino = (TSearchBox)grdAAGE.FindEditFormTemplateControl("tseFuncao");

                if (tseUnidadeEnsino != null)
                {
                    tseUnidadeEnsino.Mode = Techne.Controls.ControlMode.View;
                }
            }
        }
        protected void grdAAGE_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            if (!hdnPerfil.Value.Contains("SUPGE") && !hdnPerfil.Value.Contains("PRIVILEGIADO"))
            {
                e.Visible = false;
            }

        }
        protected void grdAAGE_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            DocenteAAGEUnidadeEnsino aage = new DocenteAAGEUnidadeEnsino();
            RN.AAGE.DocenteAAGEUnidadeEnsino rnDocenteAAGEUnidadeEnsino = new Techne.Lyceum.RN.AAGE.DocenteAAGEUnidadeEnsino();
            ValidacaoDados validacao = new ValidacaoDados();

            aage.DocenteId = Convert.ToDecimal(tseDocentes["num_func"].ToString());
            aage.DataInicioVinculo = Convert.ToDateTime(e.NewValues["DATAINICIO_VINCULO"]);
            aage.DataFimVinculo = Convert.ToDateTime(e.NewValues["DATAFIM_VINCULO"]);
            aage.UsuarioId = User.Identity.Name;
            aage.UnidadeEnsinoId = Convert.ToString(e.NewValues["UNIDADEENSINOID"]);

            validacao = rnDocenteAAGEUnidadeEnsino.Valida(aage);

            if (validacao.Valido)
            {
                rnDocenteAAGEUnidadeEnsino.Insere(aage);
                grdAAGE.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void grdAAGE_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            DocenteAAGEUnidadeEnsino aage = new DocenteAAGEUnidadeEnsino();
            RN.AAGE.DocenteAAGEUnidadeEnsino rnDocenteAAGEUnidadeEnsino = new Techne.Lyceum.RN.AAGE.DocenteAAGEUnidadeEnsino();
            ValidacaoDados validacao = new ValidacaoDados();

            aage.DocenteId = Convert.ToDecimal(tseDocentes["num_func"].ToString());
            aage.DataInicioVinculo = Convert.ToDateTime(e.NewValues["DATAINICIO_VINCULO"]);
            aage.DataFimVinculo = Convert.ToDateTime(e.NewValues["DATAFIM_VINCULO"]);
            aage.UsuarioId = User.Identity.Name;
            aage.UnidadeEnsinoId = Convert.ToString(e.NewValues["UNIDADEENSINOID"]);
            aage.DocenteAAGEUnidadeEnsinoId = int.Parse(e.Keys["DOCENTEAAGE_UNIDADEENSINO_ID"].ToString());

            validacao = rnDocenteAAGEUnidadeEnsino.Valida(aage);

            if (validacao.Valido)
            {
                rnDocenteAAGEUnidadeEnsino.Atualiza(aage);
                grdAAGE.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void grdAAGE_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            int chave = -1;
            RN.AAGE.DocenteAAGEUnidadeEnsino rnDocenteAAGEUnidadeEnsino = new Techne.Lyceum.RN.AAGE.DocenteAAGEUnidadeEnsino();

            chave = int.Parse(e.Keys["DOCENTEAAGE_UNIDADEENSINO_ID"].ToString());

            rnDocenteAAGEUnidadeEnsino.Remove(chave);

            grdAAGE.DataBind();

        }

        protected void grdAAGE_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (e.NewValues["UNIDADEENSINOID"] == null)
            {
                e.RowError = "Favor informar a Unidade de Ensino.";
                return;
            }

            if (e.NewValues["DATAINICIO_VINCULO"] == null)
            {
                e.RowError = "Necessário informar data de início.";
                return;
            }
            if (e.NewValues["DATAFIM_VINCULO"] == null)
            {
                e.RowError = "Necessário informar data de fim.";
                return;
            }

            DateTime hoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            object datainicio = e.NewValues["DATAINICIO_VINCULO"];
            DateTime dataini = Convert.ToDateTime(datainicio);

            //Verifica se a data incio da situacao é maior que a data atual
            if (dataini != null && dataini > hoje)
            {
                e.RowError = "Data Início não pode ser maior que hoje.";
                return;
            }
        }

        protected void grdAAGE_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdAAGE.Settings.ShowFilterRow = false;
        }

        public void UpdateAAGE(object TIPOFUNCAO, object DATAINICIO_VINCULO, object DATAFIM_VINCULO, object UNIDADEENSINOID, object DOCENTEAAGE_UNIDADEENSINO_ID)
        {
        }

        public void InsertAAGE(object TIPOFUNCAO, object DATAINICIO_VINCULO, object DATAFIM_VINCULO, object UNIDADEENSINOID)
        {
        }

        public void DeleteAAGE(object DOCENTEAAGE_UNIDADEENSINO_ID) { }
    }
}
