using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Matriculas;
using Techne.Web;

namespace Techne.Lyceum.Net.Matricula
{
    [NavUrl("~/Matricula/MotivoRetorno.aspx")]
    [ControlText("Motivo Retorno ")]
    [Title("Motivo Retorno")]


    public partial class MotivoRetorno : TPage
    {
        public object Lista()
        {
            RN.Matriculas.MotivoRetorno rnMotivoRetorno = new Techne.Lyceum.RN.Matriculas.MotivoRetorno();

            return rnMotivoRetorno.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object MOTIVORETORNOID) { }
        public void Delete(object MOTIVORETORNOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdMotivoRetorno, "Motivo Retorno");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdMotivoRetorno);
        }

        protected void grdMotivoRetorno_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdMotivoRetorno.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoRetorno_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdMotivoRetorno.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoRetorno_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Matriculas.Entidades.MotivoRetorno MotivoRetornoInscricao = new Techne.Lyceum.RN.Matriculas.Entidades.MotivoRetorno();
            RN.Matriculas.MotivoRetorno rnMotivoRetorno = new Techne.Lyceum.RN.Matriculas.MotivoRetorno();

            MotivoRetornoInscricao.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            MotivoRetornoInscricao.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            MotivoRetornoInscricao.UsuarioId = User.Identity.Name;

            validacao = rnMotivoRetorno.Valida(MotivoRetornoInscricao, true);

            if (validacao.Valido)
            {
                rnMotivoRetorno.Insere(MotivoRetornoInscricao);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoRetorno.DataBind();

        }

        protected void grdMotivoRetorno_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Matriculas.Entidades.MotivoRetorno MotivoRetornoInscricao = new Techne.Lyceum.RN.Matriculas.Entidades.MotivoRetorno();
            RN.Matriculas.MotivoRetorno rnMotivoRetorno = new Techne.Lyceum.RN.Matriculas.MotivoRetorno();

            MotivoRetornoInscricao.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            MotivoRetornoInscricao.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            MotivoRetornoInscricao.UsuarioId = User.Identity.Name;
            MotivoRetornoInscricao.MotivoRetornoId = Convert.ToInt32(e.Keys["MOTIVORETORNOID"]);


            validacao = rnMotivoRetorno.Valida(MotivoRetornoInscricao, true);

            if (validacao.Valido)
            {
                rnMotivoRetorno.Atualiza(MotivoRetornoInscricao);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoRetorno.DataBind();
        }

        protected void grdMotivoRetorno_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Matriculas.MotivoRetorno rnMotivoRetorno = new Techne.Lyceum.RN.Matriculas.MotivoRetorno();
            int MotivoRetornoInscricaoId = 0;

            MotivoRetornoInscricaoId = Convert.ToInt32(e.Keys["MOTIVORETORNOID"]);

            validacao = rnMotivoRetorno.ValidaRemocao(MotivoRetornoInscricaoId);

            if (validacao.Valido)
            {
                rnMotivoRetorno.Remove(MotivoRetornoInscricaoId);
                grdMotivoRetorno.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
