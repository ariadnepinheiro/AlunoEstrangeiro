using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Basico
{
    [
         NavUrl("~/Basico/MotivoDiaSemAula.aspx"),
         ControlText("Motivo Dia Sem Aula"),
         Title("Motivo Dia Sem Aula")
     ]
    public partial class MotivoDiaSemAula : TPage
    {
         public object Lista()
        {
            RN.Turmas.MotivoDiaSemAula rnMotivoDiaSemAula = new Techne.Lyceum.RN.Turmas.MotivoDiaSemAula();

            return rnMotivoDiaSemAula.Lista();

        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object MOTIVODIASEMAULAID) { }
        public void Delete(object MOTIVODIASEMAULAID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdMotivoDiaSemAula, "Motivo Dia Sem Aula");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdMotivoDiaSemAula);
        }

        protected void grdMotivoDiaSemAula_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdMotivoDiaSemAula);
        }

        protected void grdMotivoDiaSemAula_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdMotivoDiaSemAula.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoDiaSemAula_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdMotivoDiaSemAula.Settings.ShowFilterRow = false;
        }

        protected void grdMotivoDiaSemAula_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Turmas.Entidades.MotivoDiaSemAula motivoDiaSemAula = new Techne.Lyceum.RN.Turmas.Entidades.MotivoDiaSemAula();
            RN.Turmas.MotivoDiaSemAula rnMotivoDiaSemAula = new Techne.Lyceum.RN.Turmas.MotivoDiaSemAula();

            motivoDiaSemAula.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivoDiaSemAula.Ativo = e.NewValues["ATIVO"] == null ? false : true;
            motivoDiaSemAula.UsuarioId = User.Identity.Name;

            validacao = rnMotivoDiaSemAula.Valida(motivoDiaSemAula, true);

            if (validacao.Valido)
            {
                rnMotivoDiaSemAula.Insere(motivoDiaSemAula);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoDiaSemAula.DataBind();

        }

        protected void grdMotivoDiaSemAula_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Turmas.Entidades.MotivoDiaSemAula motivoDiaSemAula = new Techne.Lyceum.RN.Turmas.Entidades.MotivoDiaSemAula();
            RN.Turmas.MotivoDiaSemAula rnMotivoDiaSemAula = new Techne.Lyceum.RN.Turmas.MotivoDiaSemAula();

            motivoDiaSemAula.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            motivoDiaSemAula.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            motivoDiaSemAula.MotivoDiaSemAulaId = Convert.ToInt32(e.Keys["MOTIVODIASEMAULAID"]);
            motivoDiaSemAula.UsuarioId = User.Identity.Name;

            validacao = rnMotivoDiaSemAula.Valida(motivoDiaSemAula, false);

            if (validacao.Valido)
            {
                rnMotivoDiaSemAula.Atualiza(motivoDiaSemAula);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdMotivoDiaSemAula.DataBind();
        }

        protected void grdMotivoDiaSemAula_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Turmas.MotivoDiaSemAula rnMotivoDiaSemAula = new Techne.Lyceum.RN.Turmas.MotivoDiaSemAula();
            int motivoDiaSemAulaId = 0;

            motivoDiaSemAulaId = Convert.ToInt32(e.Keys["MOTIVODIASEMAULAID"]);

            validacao = rnMotivoDiaSemAula.ValidaRemocao(motivoDiaSemAulaId);

            if (validacao.Valido)
            {
                rnMotivoDiaSemAula.Remove(motivoDiaSemAulaId);
                grdMotivoDiaSemAula.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
