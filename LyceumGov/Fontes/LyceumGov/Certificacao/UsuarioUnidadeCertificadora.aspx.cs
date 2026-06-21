using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Controls;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxTabControl;
using System.Data;
using Techne.Lyceum.RN.DTOs;
using DevExpress.Web.ASPxGridView;


namespace Techne.Lyceum.Net.Certificacao
{
    [NavUrl("~/Certificacao/UsuarioUnidadeCertificadora.aspx")]
    [ControlText("UsuarioUnidadeCertificadora")]
    [Title("Usuários Unidade Certificadora")]

    public partial class UsuarioUnidadeCertificadora : TPage
    {
        public object Lista()
        {
            RN.Certificacao.UsuarioUnidadeCertificadora rnUsuarioUnidadeCertificadora = new Techne.Lyceum.RN.Certificacao.UsuarioUnidadeCertificadora();

            return rnUsuarioUnidadeCertificadora.Lista();
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {              
                lblMensagem.Text = String.Empty;

                if (!IsPostBack)
                {
                    pnUsuarioUnidade.Visible = false;
                    btnCancelar.Visible = false;

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdUsuarioUnidade, string.Empty);           
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdUsuarioUnidade);
            ControlaAcesso(btnNovo, AcaoControle.novo);
        }

        protected void btnNovo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                tseUnidade.ResetValue();
                tseUsuario.ResetValue();
                btnCancelar.Visible = true;
                pnUsuarioUnidade.Visible = true;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                pnUsuarioUnidade.Visible = false;
                tseUnidade.ResetValue();
                tseUsuario.ResetValue();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidade_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {               
               

                if (!tseUnidade.DBValue.IsNull)
                {
                    if (tseUnidade.IsValidDBValue)
                    {
                        
                    }
                }
                else
                {
                    lblMensagem.Text = "Unidade não cadastrada.";
                  
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        protected void tseUsuario_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {


                if (!tseUsuario.DBValue.IsNull)
                {
                    if (tseUsuario.IsValidDBValue)
                    {

                    }
                }
                else
                {
                    lblMensagem.Text = "Usuário não cadastrada.";

                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }

        }

        protected void btnIncluir_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Certificacao.UsuarioUnidadeCertificadora rnUsuarioUnidadeCertificadora = new Techne.Lyceum.RN.Certificacao.UsuarioUnidadeCertificadora();
                RN.Certificacao.Entidades.UsuarioUnidadeCertificadora usuario = new Techne.Lyceum.RN.Certificacao.Entidades.UsuarioUnidadeCertificadora();

                usuario.Usuario = (!tseUsuario.DBValue.IsNull && tseUsuario.IsValidDBValue) ? tseUsuario.DBValue.ToString() : null;
                usuario.UnidadeCertificadoraId = (!tseUnidade.DBValue.IsNull && tseUnidade.IsValidDBValue) ? Convert.ToInt32(tseUnidade.DBValue) : -1;
                usuario.UsuarioId = User.Identity.Name;

                validacao = rnUsuarioUnidadeCertificadora.Valida(usuario);

                if (validacao.Valido)
                {
                    rnUsuarioUnidadeCertificadora.Insere(usuario);
                    tseUsuario.ResetValue();
                    tseUnidade.ResetValue();
                    pnUsuarioUnidade.Visible = false;
                    lblMensagem.Text = "Usuário associado a unidade certificadora";
                    grdUsuarioUnidade.DataBind();
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public void Delete(object USUARIOUNIDADECERTIFICADORAID)
        { }

        protected void grdUsuarioUnidade_OnAfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdUsuarioUnidade);
        }

        protected void grdUsuarioUnidade_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Certificacao.UsuarioUnidadeCertificadora rnUsuarioUnidadeCertificadora = new Techne.Lyceum.RN.Certificacao.UsuarioUnidadeCertificadora();
            int usuarioId = 0;

            usuarioId = Convert.ToInt32(e.Keys["USUARIOUNIDADECERTIFICADORAID"]);

            validacao = rnUsuarioUnidadeCertificadora.ValidaRemocao(usuarioId);

            if (validacao.Valido)
            {
                rnUsuarioUnidadeCertificadora.Remove(usuarioId);
                grdUsuarioUnidade.DataBind();
            }
            else
            {
                e.Cancel = true;
                lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
            }
        }
    }
}
