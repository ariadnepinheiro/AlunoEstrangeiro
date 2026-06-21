using System;
using System.Web.UI;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Web;
using Techne.Lyceum.CR;
using System.Web;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Curriculo
{
    [NavUrl("~/Curriculo/Servidores.aspx"),
      ControlText("Servidores"),
      Title("Lotação/Situação dos Servidores e Funcionários")]

    public partial class Servidores : TPage
    {
        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                txtUsuarioHidden.Text = HttpContext.Current.User.Identity.Name;

                //se o usuário for privilegiado não tem where, se ele for de um núcleo possuirá filtro
                if (!RN.Usuarios.UsuarioPrivilegiado(HttpContext.Current.User.Identity.Name))
                {
                    if (!IsPostBack)
                    {
                        if (RN.UsuarioUnidadeFis.PossuiUmaUnidadeSo(HttpContext.Current.User.Identity.Name))
                        {
                            tseUnidade_Ensino.DBValue = RN.UnidadesAssociadas.ConsultarUnidadeAssociada(HttpContext.Current.User.Identity.Name);
                            tseRegional.DBValue = tseUnidade_Ensino["id_regional"].ToString();
                            tseMunicipio.DBValue = tseUnidade_Ensino["municipio"].ToString();
                            grdServidores.Visible = true;
                            grdServidores.CancelEdit();
                            odsServidores.Select();
                            odsServidores.DataBind();
                            grdServidores.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdServidores, "Servidores");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdServidores);
        }

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PreencherDadosSession();
            }
        }

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            if (Page.IsCallback)
                return;

            try
            {
                grdServidores.CancelEdit();
                grdServidores.Visible = false;
                tseMunicipio.ResetValue();
                tseUnidade_Ensino.ResetValue();

                RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
                if (sessao != null)
                {
                    if (!tseRegional.DBValue.IsNull)
                    {
                        tseMunicipio.DataBind();
                        if (tseRegional.IsValidDBValue)
                        {
                            sessao.Regional = Convert.ToString(tseRegional.DBValue);
                            lblMensagem.Text = string.Empty;
                        }
                        else
                        {
                            lblMensagem.Text = "Regional não cadastrada.";
                            sessao.Regional = string.Empty;
                            sessao.Municipio = string.Empty;
                        }
                    }
                    else
                    {
                        sessao.Regional = string.Empty;
                        sessao.Municipio = string.Empty;
                    }
                }
                grdServidores.Visible = false;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            if (Page.IsCallback)
                return;
            try
            {
                grdServidores.CancelEdit();
                grdServidores.Visible = false;
                tseUnidade_Ensino.ResetValue();

                RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
                if (sessao != null)
                {
                    if (!tseMunicipio.DBValue.IsNull)
                    {
                        if (tseMunicipio.IsValidDBValue)
                        {
                            sessao.Municipio = Convert.ToString(tseMunicipio.DBValue);
                            lblMensagem.Text = string.Empty;
                        }
                        else
                        {
                            lblMensagem.Text = "Município não cadastrado.";
                            sessao.Municipio = string.Empty;
                        }
                    }
                    else
                    {
                        lblMensagem.Text = string.Empty;
                        sessao.Municipio = string.Empty;
                    }
                }
                grdServidores.Visible = false;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidade_Ensino_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            if (Page.IsCallback)
                return;
            try
            {
                grdServidores.CancelEdit();
                grdServidores.Visible = false;

                RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
                if (tseUnidade_Ensino.IsValidDBValue && !tseUnidade_Ensino.DBValue.IsNull)
                {
                    lblMensagem.Text = string.Empty;

                    if (sessao != null)
                    {
                        sessao.Escola = Convert.ToString(tseUnidade_Ensino.DBValue);
                    }

                    tseRegional.DBValue = tseUnidade_Ensino["id_regional"];
                    tseMunicipio.DBValue = tseUnidade_Ensino["municipio"];

                    grdServidores.Visible = true;
                    grdServidores.CancelEdit();
                    odsServidores.Select();
                    odsServidores.DataBind();
                    grdServidores.DataBind();
                }
                else if (!tseUnidade_Ensino.DBValue.IsNull)
                {
                    lblMensagem.Text = "Unidade de ensino não cadastrada.";
                    if (sessao != null)
                        sessao.Escola = string.Empty;
                }
                else
                {
                    if (sessao != null)
                        sessao.Escola = string.Empty;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        private void PreencherDadosSession()
        {
            try
            {
                RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
                if (sessao != null)
                {
                    if (!string.IsNullOrEmpty(sessao.Regional))
                    {
                        tseRegional.DBValue = sessao.Regional;
                        if (!tseRegional.IsValidDBValue)
                        {
                            tseRegional.Msg = string.Empty;
                            tseRegional.ResetValue();
                        }
                    }
                    if (!string.IsNullOrEmpty(sessao.Municipio))
                    {
                        tseMunicipio.DBValue = sessao.Municipio;
                        if (!tseMunicipio.IsValidDBValue)
                        {
                            tseMunicipio.Msg = string.Empty;
                            tseMunicipio.ResetValue();
                        }
                    }
                    if (tseUnidade_Ensino.DBValue.IsNull)
                    {

                        if (!string.IsNullOrEmpty(sessao.Escola))
                        {
                            tseUnidade_Ensino.DBValue = sessao.Escola;

                            if (!tseUnidade_Ensino.IsValidDBValue)
                            {
                                tseUnidade_Ensino.Msg = string.Empty;
                                tseUnidade_Ensino.ResetValue();
                            }
                            else
                            {
                                tseRegional.DBValue = tseUnidade_Ensino["id_regional"];
                                grdServidores.Visible = true;
                                grdServidores.CancelEdit();
                                odsServidores.Select();
                                odsServidores.DataBind();
                                grdServidores.DataBind();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        public object Listar(DbObject tseUnidade_Ensino)
        {
            RN.Lotacao rnLotacao = new Techne.Lyceum.RN.Lotacao();

            DbObject unidade = DBNull.Value;

            if (!tseUnidade_Ensino.IsNull)
            {
                unidade = tseUnidade_Ensino.ToString();
                return rnLotacao.ListaServidoresPor(unidade.ToString());
            }

            return null;
        }

        protected void odsServidores_Updating(object sender, System.Web.UI.WebControls.ObjectDataSourceMethodEventArgs e)
        {
            RN.DTOs.DadosLotacaoDocenteFuncionario dados = new Techne.Lyceum.RN.DTOs.DadosLotacaoDocenteFuncionario();
            RN.Lotacao rnLotacao = new Lotacao();
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Docentes rnDocentes = new Docentes();
            RN.VinculoLy rnVinculoLy = new VinculoLy();


            dados.Matricula = e.InputParameters["matricula"] != null ? Convert.ToString(e.InputParameters["matricula"]) : null;
            dados.Funcao = e.InputParameters["funcao"] != null ? Convert.ToString(e.InputParameters["funcao"]) : null;
            dados.Setor = grdServidores.GetRowValues(grdServidores.EditingRowVisibleIndex, "setor") != null ? Convert.ToString(grdServidores.GetRowValues(grdServidores.EditingRowVisibleIndex, "setor")) : null;
            dados.Readaptado = e.InputParameters["readaptado"] != null ? (e.InputParameters["readaptado"].ToString() == "N" ? false : true) : false;
            dados.UnidadeEnsino = tseUnidade_Ensino.DBValue.IsNull ? null : tseUnidade_Ensino.DBValue.ToString();
            dados.Regional = tseRegional.DBValue.IsNull ? null : tseRegional.DBValue.ToString();
            dados.DataInicioReadaptacao = e.InputParameters["dt_inicio_readaptacao"] != null ? Convert.ToDateTime(e.InputParameters["dt_inicio_readaptacao"]) : (DateTime?)null;
            dados.DataFimReadaptacao = e.InputParameters["dt_fim_readaptacao"] != null ? Convert.ToDateTime(e.InputParameters["dt_fim_readaptacao"]) : (DateTime?)null;
            dados.Situacao = e.InputParameters["motivo"] != null ? e.InputParameters["motivo"].ToString().Split('|')[0] : null;
            dados.DataInicioSituacao = e.InputParameters["dataini"] != null ? Convert.ToDateTime(e.InputParameters["dataini"]) : (DateTime?)null;
            dados.DataFimSituacao = e.InputParameters["datafim"] != null ? Convert.ToDateTime(e.InputParameters["datafim"]) : (DateTime?)null;
            dados.Reducaoch = e.InputParameters["reducaoch"] != null ? e.InputParameters["reducaoch"].ToString() : "N";
            dados.dtinich = e.InputParameters["dtinich"] != null ? Convert.ToDateTime(e.InputParameters["dtinich"]) : (DateTime?)null;
            dados.dtfimch = e.InputParameters["dtfimch"] != null ? Convert.ToDateTime(e.InputParameters["dtfimch"]) : (DateTime?)null;
            dados.Usuario = User.Identity.Name;
            dados.NumFunc = grdServidores.GetRowValues(grdServidores.EditingRowVisibleIndex, "num_func") != null ? Convert.ToDecimal(grdServidores.GetRowValues(grdServidores.EditingRowVisibleIndex, "num_func")) : -1;
            dados.LicencaPossuiDataFim = e.InputParameters["motivo"] != null ? (e.InputParameters["motivo"].ToString().Split('|')[1] == "N" ? false : true) : false;
            dados.Pessoa = grdServidores.GetRowValues(grdServidores.EditingRowVisibleIndex,"pessoa") != null? Convert.ToString(grdServidores.GetRowValues(grdServidores.EditingRowVisibleIndex,"pessoa")): null;

            if (rnDocentes.EhMatriculaDocentePor(dados.Matricula))
            {

                dados.Categoria = rnDocentes.ObtemCategoriaPor(dados.Matricula);
            }
            else
            {
                dados.Categoria = rnVinculoLy.ObtemCategoriaPor(dados.Matricula);
            }
            
            validacao = rnLotacao.ValidaAlteracaoLotacaoSituacaoServidor(dados);

            if (validacao.Valido)
            {
                rnLotacao.AlteraLotacaoSituacaoServidor(dados);
                grdServidores.DataBind();
                lblMensagem.Text = string.Empty;
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }

        }

        protected void grdServidores_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            RN.AulaDocente rnAulaDocente = new AulaDocente();
            RN.Funcao rnFuncao = new Funcao();

            int aulasAlocadas = 0;
            if (grdServidores.IsEditing)
            {
                decimal num_func = Convert.ToDecimal(grdServidores.GetRowValues(e.VisibleIndex, "num_func"));
                string matricula = Convert.ToString(grdServidores.GetRowValues(e.VisibleIndex, "matricula"));
                string motivoedata = Convert.ToString(grdServidores.GetRowValues(e.VisibleIndex, "motivo"));
                string motivo = motivoedata.Split('|')[0];

                if (e.Column.FieldName == "funcao")
                {
                    aulasAlocadas = rnAulaDocente.ObtemTotalAulasAlocadasPor(num_func, DateTime.Today);
                    if (aulasAlocadas > 0)
                    {
                        e.Editor.ReadOnly = true;
                        e.Editor.ClientEnabled = false;
                        return;
                    }

                    String funcao = Convert.ToString(grdServidores.GetRowValues(e.VisibleIndex, "funcao"));
                    if (rnFuncao.EhFuncaoDiretor(funcao) || rnFuncao.EhFuncaoSecretario(funcao))
                    {
                        e.Editor.ReadOnly = true;
                        e.Editor.ClientEnabled = false;
                        return;
                    }

                    string readaptado = Convert.ToString(grdServidores.GetRowValues(e.VisibleIndex, "readaptado"));
                    if (!String.IsNullOrEmpty(readaptado))
                    {
                        if (readaptado.Equals("S"))
                        {
                            e.Editor.ReadOnly = true;
                        }
                    }
                }
                else if (e.Column.FieldName == "motivo")
                {
                    if (!String.IsNullOrEmpty(motivo))//desabilita a edição da Situação (motivo)
                    {
                        e.Editor.Attributes.Add("temMotivo", "True");
                    }
                    else
                    {
                        e.Editor.Attributes.Add("temMotivo", "False");
                    }
                }
                else if (e.Column.FieldName == "readaptado")
                {
                    if (num_func == -1)
                    {
                        e.Editor.ReadOnly = true;
                        return;
                    }

                    string readaptado = Convert.ToString(grdServidores.GetRowValues(e.VisibleIndex, "readaptado"));

                    if (!String.IsNullOrEmpty(readaptado))
                    {
                        if (readaptado.Equals("S"))
                        {
                            e.Editor.Attributes.Add("Readaptado", "True");
                            e.Editor.ReadOnly = true;
                        }
                        else
                            e.Editor.Attributes.Add("Readaptado", "False");
                    }
                    else
                    {
                        e.Editor.Attributes.Add("Readaptado", "False");
                    }

                    //SERA UTILIZADO PRA BLOQUEAR INSERÇAO READAPTADO
                    if (RN.Coordenadoria.IsUsuarioNucleoOuUnidade(RN.Lotacao.ObterSetorUsuario(HttpContext.Current.User.Identity.Name)))
                        e.Editor.ReadOnly = true;
                }
            }
        }

        protected void grdServidores_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdServidores);

        }

        protected void grdServidores_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdServidores.Settings.ShowFilterRow = false;
        }

        protected void grdServidores_CustomColumnDisplayText(object sender, ASPxGridViewColumnDisplayTextEventArgs e)
        {
            RN.Funcao rnFuncao = new Funcao();

            if (e.Column.FieldName == "funcao" && e.Value != null)
            {
                if (e.Value.ToString() != "Regente")
                {
                    string descricao = rnFuncao.ObtemDescricaoPor(e.Value.ToString());
                    e.DisplayText = descricao;
                }
            }
        }

        protected void grdServidores_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {//recupera o valor da função que foi trocado por "regente"

            if (e.NewValues["aulas_alocadas"] != null)
            {
                int aulas = 0;

                if (int.TryParse(e.NewValues["aulas_alocadas"].ToString(), out aulas))
                {
                    if (aulas > 0)
                        e.NewValues["funcao"] = RN.Lotacao.ConsultarFuncao(e.NewValues["matricula"].ToString());
                }
            }

            if (e.NewValues["aulas_alocadas_glp"] != null)
            {
                int aulas_glp = 0;

                if (int.TryParse(e.NewValues["aulas_alocadas_glp"].ToString(), out aulas_glp))
                {
                    if (aulas_glp > 0)
                        e.NewValues["funcao"] = RN.Lotacao.ConsultarFuncao(e.NewValues["matricula"].ToString());
                }
            }
        }

        protected void grdServidores_CustomJSProperties(object sender, ASPxGridViewClientJSPropertiesEventArgs e)
        {
            if (Session["Mensagem"] == null)
                e.Properties["cpMessage"] = String.Empty;
            else
            {
                e.Properties["cpMessage"] = Session["Mensagem"].ToString();
                Session["Mensagem"] = null;
            }
        }

        protected void grdServidores_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            decimal num_func = Convert.ToDecimal(grdServidores.GetRowValues(e.VisibleIndex, "num_func"));
            decimal pessoa = Convert.ToDecimal(grdServidores.GetRowValues(e.VisibleIndex, "pessoa"));
            string matricula = Convert.ToString(grdServidores.GetRowValues(e.VisibleIndex, "matricula"));
            decimal ordemLicenca = grdServidores.GetRowValues(e.VisibleIndex, "ordemLicenca") != null ? Convert.ToDecimal(grdServidores.GetRowValues(e.VisibleIndex, "ordemLicenca")) : 0;
            DateTime dataInicio = grdServidores.GetRowValues(e.VisibleIndex, "dataini") != DBNull.Value ? Convert.ToDateTime(grdServidores.GetRowValues(e.VisibleIndex, "dataini")) : DateTime.MinValue;
            RN.Lotacao rnLotacao = new Lotacao();
            ValidacaoDados validacao = new ValidacaoDados();
            RN.LicencaPessoa rnLicencaPessoa = new LicencaPessoa();
            RN.LicencaDocente rnLicencaDocente = new LicencaDocente();
            RN.DTOs.DadosExclusaoFuncaoLotacao dadosExclusao = new Techne.Lyceum.RN.DTOs.DadosExclusaoFuncaoLotacao();

            grdServidores.CancelEdit();
            if (e.ButtonID == "btnExcluirFunc")
            {
                dadosExclusao.Pessoa = pessoa;
                dadosExclusao.NumFunc = num_func;
                dadosExclusao.Matricula = matricula;
                dadosExclusao.UsuarioResponsavel = User.Identity.Name;
                
                validacao = rnLotacao.ValidaRemocaoLotacaoFuncao(dadosExclusao);

                if (validacao.Valido)
                {
                    rnLotacao.RemoveLotacaoFuncao(dadosExclusao);

                    odsServidores.Select();
                    odsServidores.DataBind();
                    grdServidores.DataBind();
                    lblMensagem.Text = string.Empty;
                }
                else
                {
                    throw new Exception(validacao.Mensagem);
                }                  
            }
            else if (e.ButtonID == "btnExcluirSit")
            {
                if (num_func > 0)
                {
                    validacao = rnLicencaDocente.ValidaRemocaoLicencaAtiva(num_func, dataInicio, User.Identity.Name);
                }
                else
                {
                    validacao = rnLicencaPessoa.ValidaRemocaoLicencaAtiva(pessoa, dataInicio, User.Identity.Name);
                }              

                if (validacao.Valido)
                {
                    if (num_func > 0)
                    {
                        rnLicencaDocente.RemoveLicenca(num_func, dataInicio);
                    }
                    else
                    {
                        rnLicencaPessoa.Remove(pessoa, ordemLicenca, dataInicio);
                    }              
                    odsServidores.Select();
                    odsServidores.DataBind();
                    grdServidores.DataBind();
                    lblMensagem.Text = string.Empty;
                }
                else
                {
                    throw new Exception(validacao.Mensagem);
                }
            }
        }

        protected void tseSituacaoFuncionamento_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                    return;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        public void Update(
            object matricula,
            object idvinculo,
            object nome,
            object cargo,
            object disciplina,
            object funcao,
            object readaptado,
            object dt_inicio_readaptacao,
            object dt_fim_readaptacao,
            object motivo,
            object dataini,
            object datafim,
            object reducaoch,
            object dtinich,
            object dtfimch,
            object aulas_alocadas,
            object aulas_alocadas_glp)
        {
        }

    }
}
