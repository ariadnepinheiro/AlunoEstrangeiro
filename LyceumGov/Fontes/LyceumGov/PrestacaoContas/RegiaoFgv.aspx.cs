using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxTabControl;
using Techne.Controls;


namespace Techne.Lyceum.Net.PrestacaoContas
{
    [
         NavUrl("~/PrestacaoContas/RegiaoFgv.aspx"),
         ControlText("RegiaoFgv"),
         Title("Região FGV")
     ]
    public partial class RegiaoFgv : TPage
    {
        public object Lista()
        {
            RN.PrestacaoContas.RegiaoFgv rnRegiaoFgv = new Techne.Lyceum.RN.PrestacaoContas.RegiaoFgv();

            return rnRegiaoFgv.Lista();

        }

        public object ListaMunicipio(object regiao)
        {
            RN.PrestacaoContas.RegiaoFgv rnRegiaoFgv = new Techne.Lyceum.RN.PrestacaoContas.RegiaoFgv();

            if (regiao.ToString() != string.Empty)
            {
            return rnRegiaoFgv.ListaMunicipioPor(Convert.ToInt32(regiao));
            }
            return null;

        }

        public void Insert(object DESCRICAO, object DATAINICIO, object DATAFIM) { }
        public void Update(object DESCRICAO, object DATAINICIO, object DATAFIM, object REGIAOFGVID) { }
        public void Delete(object REGIAOFGVID) { }
        public void DeleteMunicipio(object REGIAOFGV__MUNICIPIOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdRegiaoFgv, "Região FGV");
            TituloGrid(grdRegiaoMunicipio, "Município da Região FGV");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdRegiaoFgv);
            ControlaAcesso(grdRegiaoMunicipio);            
            ControlaAcesso(btnSalvar, AcaoControle.novo);
        }

        protected void grdRegiaoFgv_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdRegiaoFgv);
        }

        protected void grdRegiaoFgv_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdRegiaoFgv.Settings.ShowFilterRow = false;
        }

        protected void grdRegiaoFgv_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {

            grdRegiaoFgv.Settings.ShowFilterRow = false;
        }

        protected void grdRegiaoFgv_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.RegiaoFgv regiaoFgv = new Techne.Lyceum.RN.PrestacaoContas.Entidades.RegiaoFgv();
            RN.PrestacaoContas.RegiaoFgv rnRegiaoFgv = new RN.PrestacaoContas.RegiaoFgv();

            regiaoFgv.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            regiaoFgv.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            regiaoFgv.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            regiaoFgv.UsuarioId = User.Identity.Name;

            validacao = rnRegiaoFgv.Valida(regiaoFgv, true);

            if (validacao.Valido)
            {
                rnRegiaoFgv.Insere(regiaoFgv);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdRegiaoFgv.DataBind();

        }

        protected void grdRegiaoFgv_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.RegiaoFgv regiaoFgv = new Techne.Lyceum.RN.PrestacaoContas.Entidades.RegiaoFgv();
            RN.PrestacaoContas.RegiaoFgv rnRegiaoFgv = new RN.PrestacaoContas.RegiaoFgv();

            regiaoFgv.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            regiaoFgv.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
            regiaoFgv.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
            regiaoFgv.RegiaoFgvId = Convert.ToInt32(e.Keys["REGIAOFGVID"]);
            regiaoFgv.UsuarioId = User.Identity.Name;

            validacao = rnRegiaoFgv.Valida(regiaoFgv, true);

            if (validacao.Valido)
            {
                rnRegiaoFgv.Atualiza(regiaoFgv);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdRegiaoFgv.DataBind();
        }

        protected void grdRegiaoFgv_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.RegiaoFgv rnRegiaoFgv = new RN.PrestacaoContas.RegiaoFgv();
            int regiaoFgvId = 0;

            regiaoFgvId = Convert.ToInt32(e.Keys["REGIAOFGVID"]);

            validacao = rnRegiaoFgv.ValidaRemocao(regiaoFgvId);

            if (validacao.Valido)
            {
                rnRegiaoFgv.Remove(regiaoFgvId);
                grdRegiaoFgv.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

        protected void grdRegiaoMunicipio_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdRegiaoMunicipio);
        }

        protected void grdRegiaoMunicipio_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdRegiaoMunicipio.Settings.ShowFilterRow = false;
        }

        protected void grdRegiaoMunicipio_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdRegiaoMunicipio.Settings.ShowFilterRow = false;
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.RegiaoFgv rnRegiaoFgv = new Techne.Lyceum.RN.PrestacaoContas.RegiaoFgv();
            try
            {
                var regiao = (tseRegiaoFGV.IsValidDBValue && !tseRegiaoFGV.DBValue.IsNull) ? Convert.ToInt32(tseRegiaoFGV.DBValue) : -1;
                var municipio = (tseMunicipio.IsValidDBValue && !tseMunicipio.DBValue.IsNull) ? tseMunicipio.DBValue.ToString() : null;


                validacao = rnRegiaoFgv.ValidaMunicipio(regiao, municipio, User.Identity.Name);

                if (validacao.Valido)
                {
                    rnRegiaoFgv.InsereMunicipio(regiao, municipio, User.Identity.Name);

                    tseMunicipio.ResetValue();

                    lblMensagem.Text = "Município da região incluído com sucesso.";

                    grdRegiaoMunicipio.DataBind();

                }
                else
                {
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnCancelarAtualizacao_Click(object sender, EventArgs e)
        {
            try
            {
                tseMunicipio.ResetValue();
                tseRegiaoFGV.ResetValue();
                grdRegiaoMunicipio.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdRegiaoMunicipio_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.RegiaoFgv rnRegiaoFgv = new RN.PrestacaoContas.RegiaoFgv();
            int regiaoFgvId = 0;

            regiaoFgvId = Convert.ToInt32(e.Keys["REGIAOFGV__MUNICIPIOID"]);

            validacao = rnRegiaoFgv.ValidaRemocao(regiaoFgvId);

            if (validacao.Valido)
            {
                rnRegiaoFgv.RemoveMunicipio(regiaoFgvId);
                grdRegiaoMunicipio.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

        protected void pcRegiao_TabClick(object source, TabControlCancelEventArgs e)
        {
            this.lblMensagem.Text = string.Empty;

            if (e.Tab.Name == "Dados Gerais")
            {


            }
            else if (e.Tab.Name == "Municípios")
            {
                tseMunicipio.ResetValue();
                tseRegiaoFGV.ResetValue();
            }
        }

        protected void tseRegiaoFGV_Changed(object sender, ChangedEventArgs args)
        {
            try
            {

                if (!this.tseRegiaoFGV.DBValue.IsNull)
                {
                    if (!this.tseRegiaoFGV.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Região não cadastrada.";
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar uma região.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseMunicipio_Changed(object sender, ChangedEventArgs args)
        {
            try
            {

                if (!this.tseMunicipio.DBValue.IsNull)
                {
                    if (!this.tseMunicipio.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Município não cadastrado.";
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Favor consultar um Município.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
