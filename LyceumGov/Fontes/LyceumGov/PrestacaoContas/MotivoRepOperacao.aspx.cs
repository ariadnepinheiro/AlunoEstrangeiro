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
    [ControlText("Motivo Reprovação Operação")]
    [Title("Motivo Reprovação Operação")]

    public partial class MotivoRepOperacao : TPage
    {
        public object Lista()
        {
            RN.PrestacaoContas.MotivoRepOperacao rnMotivoRepOperacao = new Techne.Lyceum.RN.PrestacaoContas.MotivoRepOperacao();

            return rnMotivoRepOperacao.Lista();
        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object MOTIVOREPROVACAOOPERACAOID) { }
        public void Delete(object MOTIVOREPROVACAOOPERACAOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTipoVeiculo, "Motivo Reprovação Operação");
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
            RN.PrestacaoContas.Entidades.MotivoReprovacaoOperacao MotivoRepOperacao = new Techne.Lyceum.RN.PrestacaoContas.Entidades.MotivoReprovacaoOperacao();
            RN.PrestacaoContas.MotivoRepOperacao rnMotivoRepOperacao = new Techne.Lyceum.RN.PrestacaoContas.MotivoRepOperacao();

            MotivoRepOperacao.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            MotivoRepOperacao.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            MotivoRepOperacao.UsuarioId = User.Identity.Name;

            validacao = rnMotivoRepOperacao.Valida(MotivoRepOperacao, true);

            if (validacao.Valido)
            {
                rnMotivoRepOperacao.Insere(MotivoRepOperacao);
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
            RN.PrestacaoContas.Entidades.MotivoReprovacaoOperacao MotivoRepOperacao = new Techne.Lyceum.RN.PrestacaoContas.Entidades.MotivoReprovacaoOperacao();
            RN.PrestacaoContas.MotivoRepOperacao rnMotivoRepOperacao = new Techne.Lyceum.RN.PrestacaoContas.MotivoRepOperacao();

            MotivoRepOperacao.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            MotivoRepOperacao.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            MotivoRepOperacao.UsuarioId = User.Identity.Name;
            MotivoRepOperacao.MotivoReprovacaoOperacaoId = Convert.ToInt32(e.Keys["MOTIVOREPROVACAOOPERACAOID"]);


            validacao = rnMotivoRepOperacao.Valida(MotivoRepOperacao, true);

            if (validacao.Valido)
            {
                rnMotivoRepOperacao.Atualiza(MotivoRepOperacao);
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
            RN.PrestacaoContas.MotivoRepOperacao rnMotivoRepOperacao = new Techne.Lyceum.RN.PrestacaoContas.MotivoRepOperacao();
            int MOTIVOREPROVACAOOPERACAOid = 0;

            MOTIVOREPROVACAOOPERACAOid = Convert.ToInt32(e.Keys["MOTIVOREPROVACAOOPERACAOID"]);

            validacao = rnMotivoRepOperacao.ValidaRemocao(MOTIVOREPROVACAOOPERACAOid);

            if (validacao.Valido)
            {
                rnMotivoRepOperacao.Remove(MOTIVOREPROVACAOOPERACAOid);
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
