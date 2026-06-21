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
    [NavUrl("~/Matricula/MotivoEncaminhamentoEspecial.aspx")]
    [ControlText("Motivo Encaminhamento Especial")]
    [Title("Motivo Encaminhamento Especial")]
    
    public partial class MotivoEncaminhamentoEspecial : TPage
    {
        public object Lista()
        {
            RN.Matriculas.MotivoEncaminhamentoEspecial rnMotivoEncaminhamentoEspecial = new Techne.Lyceum.RN.Matriculas.MotivoEncaminhamentoEspecial();

            return rnMotivoEncaminhamentoEspecial.Lista();
        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object MOTIVOENCAMINHAMENTOESPECIALID) { }
        public void Delete(object MOTIVOENCAMINHAMENTOESPECIALID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdMotivoEncaminhamentoEspecial, "Motivo Encaminhamento Especial");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdMotivoEncaminhamentoEspecial);
        }

        protected void grdMotivoEncaminhamentoEspecial_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdMotivoEncaminhamentoEspecial.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoEncaminhamentoEspecial_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdMotivoEncaminhamentoEspecial.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoEncaminhamentoEspecial_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Matriculas.Entidades.MotivoEncaminhamentoEspecial MotivoEncaminhamentoEspecial = new Techne.Lyceum.RN.Matriculas.Entidades.MotivoEncaminhamentoEspecial();
            RN.Matriculas.MotivoEncaminhamentoEspecial rnMotivoEncaminhamentoEspecial = new Techne.Lyceum.RN.Matriculas.MotivoEncaminhamentoEspecial();

            MotivoEncaminhamentoEspecial.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            MotivoEncaminhamentoEspecial.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            MotivoEncaminhamentoEspecial.UsuarioId = User.Identity.Name;

            validacao = rnMotivoEncaminhamentoEspecial.Valida(MotivoEncaminhamentoEspecial, true);

            if (validacao.Valido)
            {
                rnMotivoEncaminhamentoEspecial.Insere(MotivoEncaminhamentoEspecial);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoEncaminhamentoEspecial.DataBind();

        }

        protected void grdMotivoEncaminhamentoEspecial_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Matriculas.Entidades.MotivoEncaminhamentoEspecial MotivoEncaminhamentoEspecial = new Techne.Lyceum.RN.Matriculas.Entidades.MotivoEncaminhamentoEspecial();
            RN.Matriculas.MotivoEncaminhamentoEspecial rnMotivoEncaminhamentoEspecial = new Techne.Lyceum.RN.Matriculas.MotivoEncaminhamentoEspecial();

            MotivoEncaminhamentoEspecial.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            MotivoEncaminhamentoEspecial.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            MotivoEncaminhamentoEspecial.UsuarioId = User.Identity.Name;
            MotivoEncaminhamentoEspecial.MotivoEncaminhamentoEspecialId = Convert.ToInt32(e.Keys["MOTIVOENCAMINHAMENTOESPECIALID"]);


            validacao = rnMotivoEncaminhamentoEspecial.Valida(MotivoEncaminhamentoEspecial, true);

            if (validacao.Valido)
            {
                rnMotivoEncaminhamentoEspecial.Atualiza(MotivoEncaminhamentoEspecial);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoEncaminhamentoEspecial.DataBind();
        }

        protected void grdMotivoEncaminhamentoEspecial_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Matriculas.MotivoEncaminhamentoEspecial rnMotivoEncaminhamentoEspecial = new Techne.Lyceum.RN.Matriculas.MotivoEncaminhamentoEspecial();
            int motivoEncaminhamentoEspecialId = 0;

            motivoEncaminhamentoEspecialId = Convert.ToInt32(e.Keys["MOTIVOENCAMINHAMENTOESPECIALID"]);

            validacao = rnMotivoEncaminhamentoEspecial.ValidaRemocao(motivoEncaminhamentoEspecialId);

            if (validacao.Valido)
            {
                rnMotivoEncaminhamentoEspecial.Remove(motivoEncaminhamentoEspecialId);
                grdMotivoEncaminhamentoEspecial.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
