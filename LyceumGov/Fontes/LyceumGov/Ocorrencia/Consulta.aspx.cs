using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Techne.Lyceum.RN.Entidades;
using Techne.Web;
using Techne.Data;
using Techne.Lyceum.CR;
using System.Collections.Generic;
using Techne.Lyceum.RN;
using System.Linq;
using DevExpress.Web.ASPxTabControl;
using System.Text;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using System.Data;
using Seeduc.Infra.Data;
using System.Configuration;
using Techne.Lyceum.RN.Util;
using Techne.Controls;
using DevExpress.Web.ASPxClasses;


namespace Techne.Lyceum.Net.Ocorrencia
{
    [NavUrl("~/Ocorrencia/Consulta.aspx"),
    ControlText("Consulta"),
     Title("Consulta"),]

    public partial class Consulta : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                RN.Perfil rnPerfil = new Perfil();
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    CarregaAno();

                    if (!rnPerfil.PossuiPerfilAdministradorRVEPor(User.Identity.Name))
                    {
                        lblMensagem.Text = "Você não possui permissão para visualizar esta tela.";
                        grdRegistro.Visible = false;
                        btnBuscar.Visible = false;
                    }

                    if (Request.QueryString.Keys.Count > 0)
                    {
                        LimparDados();

                        byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["Chave"]);
                        string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

                        string[] listaDados = decodedText.Split('&');

                        foreach (string lista in listaDados)
                        {                          
                            if (lista.IndexOf("ano") >= 0)
                                ddlAno.SelectedValue = lista.Substring(lista.LastIndexOf('=') + 1);

                            if (lista.IndexOf("escola") >= 0)
                                tseUnidadeResponsavel.DBValue = lista.Substring(lista.LastIndexOf('=') + 1);                                                       

                            if (lista.IndexOf("regional") >= 0)
                                tseRegional.DBValue = lista.Substring(lista.LastIndexOf('=') + 1);

                            if (lista.IndexOf("municipio") >= 0)
                                tseMunicipio.DBValue = lista.Substring(lista.LastIndexOf('=') + 1);

                            if (lista.IndexOf("dtocor") >= 0)
                                dtDataOcorrencia.Text = lista.Substring(lista.LastIndexOf('=') + 1);

                            if (lista.IndexOf("situacao") >= 0)
                            {
                                var situacao = lista.Substring(lista.LastIndexOf('=') + 1);

                                if (situacao == "Em aberto")
                                {
                                    ddlSituacao.SelectedValue = "EmAberto";
                                }
                                else if (situacao == "Encaminhado" || situacao == "Arquivado")
                                {
                                    ddlSituacao.SelectedValue = situacao;
                                }
                                else 
                                {
                                    ddlSituacao.SelectedValue = string.Empty;
                                }
                            }

                            if (lista.IndexOf("classe") >= 0)
                                tseClasse.DBValue = lista.Substring(lista.LastIndexOf('=') + 1);

                            if (lista.IndexOf("subcl") >= 0)
                                tseSubClasse.DBValue = lista.Substring(lista.LastIndexOf('=') + 1);

                            if (lista.IndexOf("tratamento") >= 0)
                                tseTratamento.DBValue = lista.Substring(lista.LastIndexOf('=') + 1);

                            if (lista.IndexOf("tela") >= 0)
                                Session["tela"] = lista.Substring(lista.LastIndexOf('=') + 1);
                                                                                   
                        }

                        btnBuscar_Click(null, null);
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
            try
            {
                TituloGrid(grdRegistro, "Ocorrências");
                ControlaAcesso(grdRegistro);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void PreencherFiltroConsulta()
        { 
            
        }

        private void CarregaAno()
        {
            ddlAno.Items.Clear();
            ListItem item = new ListItem("Selecione", string.Empty);
            ddlAno.DataSource = RN.PeriodoLetivo.ListarAnos();
            ddlAno.DataBind();
            ddlAno.Items.Insert(0, item);
        }
        protected void LimparDados()
        {
            ddlAno.ClearSelection();
            tseRegional.ResetValue();
            tseMunicipio.ResetValue();
            tseUnidadeResponsavel.ResetValue();
            tseSubClasse.ResetValue();
            tseClasse.ResetValue();
            tseTratamento.ResetValue();
            ddlSituacao.ClearSelection();
            dtDataOcorrencia.Text = string.Empty;
        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    CarregaGrid();
                }
                else
                {
                    lblMensagem.Text = "Para efetuar a busca é necessário selecionar o Ano.";
                    grdRegistro.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        private void CarregaGrid()
        {
            try
            {

                RN.Ocorrencias.Ocorrencia rnOcorrencia = new Techne.Lyceum.RN.Ocorrencias.Ocorrencia();

                grdRegistro.DataSource = rnOcorrencia.ListaOcorrenciaAtivaPor(!ddlAno.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? Convert.ToInt32(ddlAno.SelectedValue) : -1,
                    tseRegional.IsValidDBValue && !tseRegional.DBValue.IsNull ? Convert.ToInt32(tseRegional.DBValue) : -1,
                    tseMunicipio.IsValidDBValue && !tseMunicipio.DBValue.IsNull ? tseMunicipio.DBValue.ToString() : null,
                    tseUnidadeResponsavel.IsValidDBValue && !tseUnidadeResponsavel.DBValue.IsNull ? tseUnidadeResponsavel.DBValue.ToString() : null,
                    !dtDataOcorrencia.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(dtDataOcorrencia.Value) : (DateTime?)null,
                    tseClasse.IsValidDBValue && !tseClasse.DBValue.IsNull ? Convert.ToInt32(tseClasse.DBValue) : -1,
                    tseSubClasse.IsValidDBValue && !tseSubClasse.DBValue.IsNull ? Convert.ToInt32(tseSubClasse.DBValue) : -1,
                    tseTratamento.IsValidDBValue && !tseTratamento.DBValue.IsNull ? Convert.ToInt32(tseTratamento.DBValue) : -1,
                    !ddlSituacao.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlSituacao.SelectedValue : null);


                grdRegistro.DataBind();

                if (grdRegistro.VisibleRowCount > 0)
                {
                    grdRegistro.Visible = true;
                }
                else
                {
                    lblMensagem.Text = "Não existem ocorrências para o filtro selecionado.";
                    grdRegistro.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdRegistro_PageIndexChanged(object sender, EventArgs e)
        {
            CarregaGrid();
        }

        protected void grdRegistro_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            if (e.CallbackName.Equals("SELECTION"))
            {
                RN.DTOs.DadosOcorrencia ocorrencia = new Techne.Lyceum.RN.DTOs.DadosOcorrencia();

                ocorrencia.OcorrenciaId = Convert.ToInt32(grdRegistro.GetRowValues(GetSelectedRowOnTheCurrentPage(), "OCORRENCIAID")); ;

                string queryString = MontarQueryString(ocorrencia);
                byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(queryString);

                ASPxWebControl.RedirectOnCallback("Registro.aspx?Chave=" + Convert.ToBase64String(bytesToEncode));
            }

        }

        private int GetSelectedRowOnTheCurrentPage()
        {
            int startIndexOnPage = grdRegistro.PageIndex * grdRegistro.SettingsPager.PageSize;
            for (int i = 0; i < grdRegistro.VisibleRowCount; i++)
            {
                if (grdRegistro.Selection.IsRowSelected(startIndexOnPage + i))
                    return startIndexOnPage + i;
            }

            return -1;
        }


        private string MontarQueryString(Techne.Lyceum.RN.DTOs.DadosOcorrencia ocorrencia)
        {
            string queryString = string.Empty;

            if (ocorrencia != null)
            {
                queryString += "tela=" + "consulta";
                queryString += "&tipoOperacao=" + "CONSULTAR";
                queryString += "&codigo=" + ocorrencia.OcorrenciaId;
                queryString += "&ano=" + ddlAno.SelectedValue;
                queryString += "&dtocor=" + dtDataOcorrencia.Text;
                queryString += "&regional=" + tseRegional.DBValue.ToString();
                queryString += "&municipio=" + tseMunicipio.DBValue.ToString();
                queryString += "&escola=" + tseUnidadeResponsavel.DBValue.ToString();
                queryString += "&classe=" + tseClasse.DBValue.ToString();
                queryString += "&subcl=" + tseSubClasse.DBValue.ToString();
                queryString += "&tratamento=" + tseTratamento.DBValue.ToString();
                queryString += "&situacao=" + ddlSituacao.SelectedValue;
            }
            return queryString;
        }

        protected void tseClasse_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                lblMensagem.Text = string.Empty;

                if (!tseClasse.DBValue.IsNull)
                {
                    if (!tseClasse.IsValidDBValue)
                    {
                        lblMensagem.Text = "Classe não cadastrada (favor verificar).";
                    }

                }
                else
                {

                    lblMensagem.Text = "Classe não cadastrada (favor verificar).";
                }


            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
        protected void tseSubClasse_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                lblMensagem.Text = string.Empty;

                if (!tseSubClasse.DBValue.IsNull)
                {
                    if (!tseSubClasse.IsValidDBValue)
                    {

                        lblMensagem.Text = "SubClasse não cadastrada (favor verificar).";
                    }
                    else
                    {
                        if (tseClasse.DBValue.IsNull)
                        {
                            tseClasse.DBValue = tseSubClasse["classeid"];
                        }
                    }
                }
                else
                {

                    lblMensagem.Text = "SubClasse não cadastrada (favor verificar).";
                }


            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseTratamento_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                lblMensagem.Text = string.Empty;

                if (!tseTratamento.DBValue.IsNull)
                {
                    if (!tseTratamento.IsValidDBValue)
                    {

                        lblMensagem.Text = "Tratamento não cadastrada (favor verificar).";
                    }

                }
                else
                {

                    lblMensagem.Text = "Tratamento não cadastrada (favor verificar).";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;

                RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (sessao != null)
                {
                    if (!tseMunicipio.DBValue.IsNull && tseMunicipio.IsValidDBValue)
                    {
                        sessao.Municipio = tseMunicipio.DBValue.ToString();
                        tseUnidadeResponsavel.ResetValue();

                    }

                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;



                RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();
                if (sessao != null)
                {
                    if (!tseRegional.DBValue.IsNull && tseRegional.IsValidDBValue)
                    {
                        sessao.Regional = Convert.ToString(tseRegional.DBValue);
                        tseUnidadeResponsavel.ResetValue();
                    }
                }

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
                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;



                RN.SessaoUsuario sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue && !tseUnidadeResponsavel["unidade_ens"].IsNull)
                {


                    tseRegional.Value = tseUnidadeResponsavel["id_regional"];
                    tseMunicipio.Value = tseUnidadeResponsavel["municipio"];

                    if (sessao != null)
                    {
                        sessao.Escola = Convert.ToString(tseUnidadeResponsavel.DBValue);
                        sessao.Municipio = tseMunicipio.DBValue.ToString();
                        sessao.Regional = tseUnidadeResponsavel["id_regional"].ToString();
                    }
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
