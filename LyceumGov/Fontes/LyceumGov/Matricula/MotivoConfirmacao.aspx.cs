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
    [NavUrl("~/Matricula/MotivoConfirmacao.aspx")]
    [ControlText("Motivo Confirmação")]
    [Title("Motivo Confirmação")]


    public partial class MotivoConfirmacao : TPage
    {
        public object Lista()
        {
            RN.Matriculas.MotivoRejeicaoInscricao rnMotivoRejeicaoInscricao = new Techne.Lyceum.RN.Matriculas.MotivoRejeicaoInscricao();

            return rnMotivoRejeicaoInscricao.ListaMotivoParaConfirmacao();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object MOTIVOREJEICAOINSCRICAOID) { }
        public void Delete(object MOTIVOREJEICAOINSCRICAOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdMotivoConfirmacao, "Motivo Confirmacao");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdMotivoConfirmacao);
        }

        protected void grdMotivoConfirmacao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdMotivoConfirmacao.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoConfirmacao_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdMotivoConfirmacao.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoConfirmacao_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Matriculas.Entidades.MotivoRejeicaoInscricao motivoRejeicaoInscricao = new Techne.Lyceum.RN.Matriculas.Entidades.MotivoRejeicaoInscricao();
            RN.Matriculas.MotivoRejeicaoInscricao rnMotivoRejeicaoInscricao = new Techne.Lyceum.RN.Matriculas.MotivoRejeicaoInscricao();

            motivoRejeicaoInscricao.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivoRejeicaoInscricao.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivoRejeicaoInscricao.Tipo = (int)MotivoRejeicaoInscricao.Tipo.Confirmacao;
            motivoRejeicaoInscricao.UsuarioId = User.Identity.Name;

            validacao = rnMotivoRejeicaoInscricao.Valida(motivoRejeicaoInscricao, true);

            if (validacao.Valido)
            {
                rnMotivoRejeicaoInscricao.Insere(motivoRejeicaoInscricao);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoConfirmacao.DataBind();

        }

        protected void grdMotivoConfirmacao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Matriculas.Entidades.MotivoRejeicaoInscricao motivoRejeicaoInscricao = new Techne.Lyceum.RN.Matriculas.Entidades.MotivoRejeicaoInscricao();
            RN.Matriculas.MotivoRejeicaoInscricao rnMotivoRejeicaoInscricao = new Techne.Lyceum.RN.Matriculas.MotivoRejeicaoInscricao();

            motivoRejeicaoInscricao.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivoRejeicaoInscricao.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivoRejeicaoInscricao.Tipo = (int)MotivoRejeicaoInscricao.Tipo.Confirmacao;
            motivoRejeicaoInscricao.UsuarioId = User.Identity.Name;
            motivoRejeicaoInscricao.MotivoRejeicaoInscricaoId = Convert.ToInt32(e.Keys["MOTIVOREJEICAOINSCRICAOID"]);


            validacao = rnMotivoRejeicaoInscricao.Valida(motivoRejeicaoInscricao, true);

            if (validacao.Valido)
            {
                rnMotivoRejeicaoInscricao.Atualiza(motivoRejeicaoInscricao);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoConfirmacao.DataBind();
        }

        protected void grdMotivoConfirmacao_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Matriculas.MotivoRejeicaoInscricao rnMotivoRejeicaoInscricao = new Techne.Lyceum.RN.Matriculas.MotivoRejeicaoInscricao();
            int motivoRejeicaoInscricaoId = 0;

            motivoRejeicaoInscricaoId = Convert.ToInt32(e.Keys["MOTIVOREJEICAOINSCRICAOID"]);

            validacao = rnMotivoRejeicaoInscricao.ValidaRemocao(motivoRejeicaoInscricaoId, (int)MotivoRejeicaoInscricao.Tipo.Confirmacao);

            if (validacao.Valido)
            {
                rnMotivoRejeicaoInscricao.Remove(motivoRejeicaoInscricaoId);
                grdMotivoConfirmacao.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
