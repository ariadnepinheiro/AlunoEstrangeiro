using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Matriculas;
using Techne.Web;

namespace Techne.Lyceum.Net.PrestacaoContas
{
    [NavUrl("~/PrestacaoContas/PrestacaoContas.aspx")]
    [ControlText("Tipo Exigência Créditos e Débitos")]
    [Title("Tipo Exigência Créditos e Débitoss")]

    public partial class MotivoExigenciaCredDeb : TPage
    {
        public object Lista()
        {
            RN.PrestacaoContas.MotivoExigenciaCredDeb rnMotivoExigenciaCredDeb = new Techne.Lyceum.RN.PrestacaoContas.MotivoExigenciaCredDeb();

            return rnMotivoExigenciaCredDeb.Lista();
        }

        public void Insert(object DESCRICAO, object ATIVO, object NECESSITAANEXO) { }
        public void Update(object DESCRICAO, object ATIVO, object NECESSITAANEXO, object TIPOEXIGENCIAOPERACAOID) { }
        public void Delete(object TIPOEXIGENCIAOPERACAOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTipoVeiculo, "Tipo Exigências Créditos e Débitos");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTipoVeiculo);
        }

        protected void grdTipoVeiculo_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTipoVeiculo.Settings.ShowFilterRow = false;
        }

        protected void grdTipoVeiculo_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            e.NewValues["NECESSITAANEXO"] = false;
            grdTipoVeiculo.Settings.ShowFilterRow = false;
        }

        protected void grdTipoVeiculo_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.MotivoExigenciaCredDeb motivoExigenciaCredDeb = new Techne.Lyceum.RN.PrestacaoContas.Entidades.MotivoExigenciaCredDeb();
            RN.PrestacaoContas.MotivoExigenciaCredDeb rnMotivoExigenciaCredDeb = new Techne.Lyceum.RN.PrestacaoContas.MotivoExigenciaCredDeb();

            motivoExigenciaCredDeb.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivoExigenciaCredDeb.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivoExigenciaCredDeb.NecessitaAnexo = (e.NewValues["NECESSITAANEXO"] == null || Convert.ToBoolean(e.NewValues["NECESSITAANEXO"]) == false) ? false : true;
            motivoExigenciaCredDeb.UsuarioId = User.Identity.Name;

            validacao = rnMotivoExigenciaCredDeb.Valida(motivoExigenciaCredDeb, true);

            if (validacao.Valido)
            {
                rnMotivoExigenciaCredDeb.Insere(motivoExigenciaCredDeb);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoVeiculo.DataBind();

        }

        protected void grdTipoVeiculo_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.Entidades.MotivoExigenciaCredDeb motivoExigenciaCredDeb = new Techne.Lyceum.RN.PrestacaoContas.Entidades.MotivoExigenciaCredDeb();
            RN.PrestacaoContas.MotivoExigenciaCredDeb rnMotivoExigenciaCredDeb = new Techne.Lyceum.RN.PrestacaoContas.MotivoExigenciaCredDeb();

            motivoExigenciaCredDeb.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivoExigenciaCredDeb.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivoExigenciaCredDeb.NecessitaAnexo = (e.NewValues["NECESSITAANEXO"] == null || Convert.ToBoolean(e.NewValues["NECESSITAANEXO"]) == false) ? false : true;
            motivoExigenciaCredDeb.UsuarioId = User.Identity.Name;
            motivoExigenciaCredDeb.TipoExigenciaOperacaoId = Convert.ToInt32(e.Keys["TIPOEXIGENCIAOPERACAOID"]);


            validacao = rnMotivoExigenciaCredDeb.Valida(motivoExigenciaCredDeb, false);

            if (validacao.Valido)
            {
                rnMotivoExigenciaCredDeb.Atualiza(motivoExigenciaCredDeb);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdTipoVeiculo.DataBind();
        }

        protected void grdTipoVeiculo_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.PrestacaoContas.MotivoExigenciaCredDeb rnMotivoExigenciaCredDeb = new Techne.Lyceum.RN.PrestacaoContas.MotivoExigenciaCredDeb();
            int tipoexigenciaoperacaoid = 0;

            tipoexigenciaoperacaoid = Convert.ToInt32(e.Keys["TIPOEXIGENCIAOPERACAOID"]);

            validacao = rnMotivoExigenciaCredDeb.ValidaRemocao(tipoexigenciaoperacaoid);

            if (validacao.Valido)
            {
                rnMotivoExigenciaCredDeb.Remove(tipoexigenciaoperacaoid);
                grdTipoVeiculo.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
