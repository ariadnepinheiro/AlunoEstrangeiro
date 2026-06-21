using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Matriculas;
using Techne.Web;

namespace Techne.Lyceum.Net.Transporte
{
    [NavUrl("~/Transporte/TipoVeiculo.aspx")]
    [ControlText("Tipo Veículo")]
    [Title("Tipo Veículo")]

    public partial class TipoVeiculo : TPage
    {
        public object Lista()
        {
            RN.Transporte.TipoVeiculo rnTipoVeiculo = new Techne.Lyceum.RN.Transporte.TipoVeiculo();

            return rnTipoVeiculo.Lista();
        }

        public void Insert(object DESCRICAO, object ATIVO) { }
        public void Update(object DESCRICAO, object ATIVO, object TIPOVEICULOID) { }
        public void Delete(object TIPOVEICULOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdTipoVeiculo, "Tipo de Veículo");
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
            grdTipoVeiculo.Settings.ShowFilterRow = false;
        }

        protected void grdTipoVeiculo_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Entidades.TipoVeiculo tipoVeiculo = new Techne.Lyceum.RN.Transporte.Entidades.TipoVeiculo();
            RN.Transporte.TipoVeiculo rnTipoVeiculo = new Techne.Lyceum.RN.Transporte.TipoVeiculo();

            tipoVeiculo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipoVeiculo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;            
            tipoVeiculo.UsuarioId = User.Identity.Name;

            validacao = rnTipoVeiculo.Valida(tipoVeiculo, true);

            if (validacao.Valido)
            {
                rnTipoVeiculo.Insere(tipoVeiculo);
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
            RN.Transporte.Entidades.TipoVeiculo tipoVeiculo = new Techne.Lyceum.RN.Transporte.Entidades.TipoVeiculo();
            RN.Transporte.TipoVeiculo rnTipoVeiculo = new Techne.Lyceum.RN.Transporte.TipoVeiculo();

            tipoVeiculo.Descricao = e.NewValues["DESCRICAO"] != null ? e.NewValues["DESCRICAO"].ToString().Trim().ToUpper() : null;
            tipoVeiculo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;            
            tipoVeiculo.UsuarioId = User.Identity.Name;
            tipoVeiculo.TipoVeiculoId = Convert.ToInt32(e.Keys["TIPOVEICULOID"]);


            validacao = rnTipoVeiculo.Valida(tipoVeiculo, true);

            if (validacao.Valido)
            {
                rnTipoVeiculo.Atualiza(tipoVeiculo);
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
            RN.Transporte.TipoVeiculo rnTipoVeiculo = new Techne.Lyceum.RN.Transporte.TipoVeiculo();
            int tipoVeiculoId = 0;

            tipoVeiculoId = Convert.ToInt32(e.Keys["TIPOVEICULOID"]);

            validacao = rnTipoVeiculo.ValidaRemocao(tipoVeiculoId);

            if (validacao.Valido)
            {
                rnTipoVeiculo.Remove(tipoVeiculoId);
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
