using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using Techne.Web;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/AgrupamentoCargos.aspx")]
    [ControlText("Agrupamento dos Cargos")]
    [Title("Agrupamento dos Cargos")]
    public partial class AgrupamentoCargos : TPage
    {
        public object Lista()
        {
            RN.RecursosHumanos.AgrupamentoCargos rnAgrupamentoCargos = new Techne.Lyceum.RN.RecursosHumanos.AgrupamentoCargos();
            return rnAgrupamentoCargos.Lista();
        }

        public void Insert(object DESCRICAO, object CARGAHORARIA, object ATIVO) { }
        public void Update(object DESCRICAO, object CARGAHORARIA, object ATIVO, object AGRUPAMENTOCARGOSID) { }
        public void Delete(object AGRUPAMENTOCARGOSID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdAgrupamentoCargo, "Agrupamento dos Cargos");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnRelatorio.Attributes.Add("onclick", @"javascript:window.open('../Relatorio/Relatorios.aspx?report=RelAgupCargos&grp=qhi','', 'directories=no, location=no, status=no, scrollbars=yes, menubar=yes, width=860, height=600, resizable=yes'); return false;");
        }
        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdAgrupamentoCargo);
        }

        protected void grdAgrupamentoCargo_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdAgrupamentoCargo.Settings.ShowFilterRow = false;
        }

        protected void grdAgrupamentoCargo_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdAgrupamentoCargo.Settings.ShowFilterRow = false;
        }

        protected void grdAgrupamentoCargo_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RecursosHumanos.Entidades.AgrupamentoCargos agrupamentoCargos = new Techne.Lyceum.RN.RecursosHumanos.Entidades.AgrupamentoCargos();
            RN.RecursosHumanos.AgrupamentoCargos rnAgrupamentoCargos = new Techne.Lyceum.RN.RecursosHumanos.AgrupamentoCargos();

            agrupamentoCargos.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            agrupamentoCargos.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            agrupamentoCargos.CargaHoraria = e.NewValues["CARGAHORARIA"] != null ? Convert.ToInt32(e.NewValues["CARGAHORARIA"]) : -1;
            agrupamentoCargos.UsuarioId = User.Identity.Name;

            validacao = rnAgrupamentoCargos.Valida(agrupamentoCargos, true);

            if (validacao.Valido)
            {
                rnAgrupamentoCargos.Insere(agrupamentoCargos);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdAgrupamentoCargo.DataBind();
        }

        protected void grdAgrupamentoCargo_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RecursosHumanos.Entidades.AgrupamentoCargos agrupamentoCargos = new Techne.Lyceum.RN.RecursosHumanos.Entidades.AgrupamentoCargos();
            RN.RecursosHumanos.AgrupamentoCargos rnAgrupamentoCargos = new Techne.Lyceum.RN.RecursosHumanos.AgrupamentoCargos();

            agrupamentoCargos.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            agrupamentoCargos.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            agrupamentoCargos.CargaHoraria = e.NewValues["CARGAHORARIA"] != null ? Convert.ToInt32(e.NewValues["CARGAHORARIA"]) : -1;
            agrupamentoCargos.UsuarioId = User.Identity.Name;
            agrupamentoCargos.AgrupamentoCargosId = Convert.ToInt32(e.Keys["AGRUPAMENTOCARGOSID"]);

            validacao = rnAgrupamentoCargos.Valida(agrupamentoCargos, true);

            if (validacao.Valido)
            {
                rnAgrupamentoCargos.Atualiza(agrupamentoCargos);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdAgrupamentoCargo.DataBind();
        }

        protected void grdAgrupamentoCargo_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.RecursosHumanos.AgrupamentoCargos rnAgrupamentoCargos = new Techne.Lyceum.RN.RecursosHumanos.AgrupamentoCargos();
            int agrupamentoCargosId = 0;

            agrupamentoCargosId = Convert.ToInt32(e.Keys["AGRUPAMENTOCARGOSID"]);

            validacao = rnAgrupamentoCargos.ValidaRemocao(agrupamentoCargosId);

            if (validacao.Valido)
            {
                rnAgrupamentoCargos.Remove(agrupamentoCargosId);
                grdAgrupamentoCargo.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }

       
    }
}
