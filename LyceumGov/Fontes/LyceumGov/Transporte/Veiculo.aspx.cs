using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using Techne.Web;

namespace Techne.Lyceum.Net.Transporte
{
    [NavUrl("~/Transporte/Veiculo.aspx")]
    [ControlText("Veículo")]
    [Title("Veículo")]

    public partial class Veiculo : TPage
    {
        public object ListarTipoVeiculo()
        {
            RN.Transporte.TipoVeiculo rnTipoVeiculo = new Techne.Lyceum.RN.Transporte.TipoVeiculo();

            return rnTipoVeiculo.ListaAtivo();
        }

        public object ListarVeiculo()
        {
            RN.Transporte.Veiculo rnVeiculo = new Techne.Lyceum.RN.Transporte.Veiculo();

            return rnVeiculo.Lista();
        }


        public void Update(object TIPOVEICULOID, object NOME, object PLACA, object ANOLICENCIAMENTO, object ANOMODELO, object QUANTIDADEASSENTOS, object OBSERVACAO, object ATIVO, object VEICULOID) { }
        public void Insert(object TIPOVEICULOID, object NOME, object PLACA, object ANOLICENCIAMENTO, object ANOMODELO, object QUANTIDADEASSENTOS, object OBSERVACAO, object ATIVO) { }
        public void Delete(object VEICULOID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdVeiculo, "Veículo");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdVeiculo);
        }

        protected void grdVeiculo_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdVeiculo.Settings.ShowFilterRow = false;
        }

        protected void grdVeiculo_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            // e.NewValues["ATIVO"] = true;
            grdVeiculo.Settings.ShowFilterRow = false;

        }

        protected void grdVeiculo_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdVeiculo.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "ATIVO")
                {
                    e.Editor.Value = true;
                }
            }
        }

        protected void grdVeiculo_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Entidades.Veiculo veiculo = new Techne.Lyceum.RN.Transporte.Entidades.Veiculo();
            RN.Transporte.Veiculo rnVeiculo = new Techne.Lyceum.RN.Transporte.Veiculo();

            veiculo.TipoVeiculoId = e.NewValues["TIPOVEICULOID"] != null ? Convert.ToInt32(e.NewValues["TIPOVEICULOID"]) : -1;
            veiculo.Placa = e.NewValues["PLACA"] != null ? e.NewValues["PLACA"].ToString().Trim().ToUpper() : null;
            veiculo.AnoLicenciamento = e.NewValues["ANOLICENCIAMENTO"] != null ? Convert.ToInt32(e.NewValues["ANOLICENCIAMENTO"]) : -1;
            veiculo.Nome = e.NewValues["NOME"] != null ? e.NewValues["NOME"].ToString().Trim().ToUpper() : null;
            veiculo.AnoModelo = e.NewValues["ANOMODELO"] != null ? Convert.ToInt32(e.NewValues["ANOMODELO"]) : -1;
            veiculo.QuantidadeAssentos = e.NewValues["QUANTIDADEASSENTOS"] != null ? Convert.ToInt32(e.NewValues["QUANTIDADEASSENTOS"]) : -1;
            veiculo.Observacao = e.NewValues["OBSERVACAO"] != null ? e.NewValues["OBSERVACAO"].ToString().Trim() : null;
            veiculo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            veiculo.UsuarioId = User.Identity.Name;

            validacao = rnVeiculo.Valida(veiculo, true);

            if (validacao.Valido)
            {
                rnVeiculo.Insere(veiculo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdVeiculo.DataBind();

        }

        protected void grdVeiculo_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Entidades.Veiculo veiculo = new Techne.Lyceum.RN.Transporte.Entidades.Veiculo();
            RN.Transporte.Veiculo rnVeiculo = new Techne.Lyceum.RN.Transporte.Veiculo();

            veiculo.TipoVeiculoId = e.NewValues["TIPOVEICULOID"] != null ? Convert.ToInt32(e.NewValues["TIPOVEICULOID"]) : -1;
            veiculo.Placa = e.NewValues["PLACA"] != null ? e.NewValues["PLACA"].ToString().Trim().ToUpper() : null;
            veiculo.AnoLicenciamento = e.NewValues["ANOLICENCIAMENTO"] != null ? Convert.ToInt32(e.NewValues["ANOLICENCIAMENTO"]) : -1;
            veiculo.Nome = e.NewValues["NOME"] != null ? e.NewValues["NOME"].ToString().Trim().ToUpper() : null;
            veiculo.AnoModelo = e.NewValues["ANOMODELO"] != null ? Convert.ToInt32(e.NewValues["ANOMODELO"]) : -1;
            veiculo.QuantidadeAssentos = e.NewValues["QUANTIDADEASSENTOS"] != null ? Convert.ToInt32(e.NewValues["QUANTIDADEASSENTOS"]) : -1;
            veiculo.Observacao = e.NewValues["OBSERVACAO"] != null ? e.NewValues["OBSERVACAO"].ToString().Trim() : null;
            veiculo.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            veiculo.UsuarioId = User.Identity.Name;
            veiculo.VeiculoId = Convert.ToInt32(e.Keys["VEICULOID"]);

            validacao = rnVeiculo.Valida(veiculo, true);

            if (validacao.Valido)
            {
                rnVeiculo.Atualiza(veiculo);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdVeiculo.DataBind();
        }

        protected void grdVeiculo_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.Transporte.Veiculo rnVeiculo = new Techne.Lyceum.RN.Transporte.Veiculo();
            int veiculoId = 0;

            veiculoId = Convert.ToInt32(e.Keys["VEICULOID"]);

            validacao = rnVeiculo.ValidaRemocao(veiculoId);

            if (validacao.Valido)
            {
                rnVeiculo.Remove(veiculoId);
                grdVeiculo.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
