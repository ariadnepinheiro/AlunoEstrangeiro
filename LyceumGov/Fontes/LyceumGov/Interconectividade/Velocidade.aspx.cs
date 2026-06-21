using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.Net.Interconectividade
{
    [
        NavUrl("~/Interconectividade/Velocidade.aspx"),
        ControlText("Velocidade"),
        Title("Velocidade")
    ]
    public partial class Velocidade : TPage
    {

        public object Lista()
        {
            RN.FiscalizacaoLink.Velocidade rnVelocidade = new Techne.Lyceum.RN.FiscalizacaoLink.Velocidade();
            return rnVelocidade.Lista();
        }

        public object ListaUnidade()
        {
            RN.FiscalizacaoLink.UnidadeVelocidade rnUnidadeVelocidade = new Techne.Lyceum.RN.FiscalizacaoLink.UnidadeVelocidade();
            return rnUnidadeVelocidade.ListaAtivo();
        }

        public void Insert(object UNIDADEVELOCIDADEID, object VALOR, object ATIVO) { }
        public void Update(object UNIDADEVELOCIDADEID, object VALOR, object ATIVO, object VELOCIDADEID) { }
        public void Delete(object VELOCIDADEID) { }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdVelocidade, "Velocidade");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdVelocidade);
        }

        protected void grdVelocidade_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdVelocidade.Settings.ShowFilterRow = false;
        }

        protected void grdVelocidade_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            e.NewValues["ATIVO"] = true;
            grdVelocidade.Settings.ShowFilterRow = false;
        }

        protected void grdVelocidade_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.FiscalizacaoLink.Entidades.Velocidade velocidade = new Techne.Lyceum.RN.FiscalizacaoLink.Entidades.Velocidade();
            RN.FiscalizacaoLink.Velocidade rnVelocidade = new RN.FiscalizacaoLink.Velocidade();

            velocidade.UnidadeVelocidadeId = e.NewValues["UNIDADEVELOCIDADEID"] != null ? Convert.ToInt32(e.NewValues["UNIDADEVELOCIDADEID"]) : -1;
            velocidade.Valor = e.NewValues["VALOR"] != null ? Convert.ToInt32(e.NewValues["VALOR"]) : 0;
            velocidade.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            velocidade.UsuarioId = User.Identity.Name;

            validacao = rnVelocidade.Valida(velocidade, true);

            if (validacao.Valido)
            {
                rnVelocidade.Insere(velocidade);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdVelocidade.DataBind();

        }

        protected void grdVelocidade_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.FiscalizacaoLink.Entidades.Velocidade velocidade = new Techne.Lyceum.RN.FiscalizacaoLink.Entidades.Velocidade();
            RN.FiscalizacaoLink.Velocidade rnVelocidade = new RN.FiscalizacaoLink.Velocidade();

            velocidade.UnidadeVelocidadeId = e.NewValues["UNIDADEVELOCIDADEID"] != null ? Convert.ToInt32(e.NewValues["UNIDADEVELOCIDADEID"]) : -1;
            velocidade.Valor = e.NewValues["VALOR"] != null ? Convert.ToInt32(e.NewValues["VALOR"]) : 0;
            velocidade.Ativo = (e.NewValues["ATIVO"] == null || Convert.ToBoolean(e.NewValues["ATIVO"]) == false) ? false : true;
            velocidade.VelocidadeId = Convert.ToInt32(e.Keys["VELOCIDADEID"]);
            velocidade.UsuarioId = User.Identity.Name;

            validacao = rnVelocidade.Valida(velocidade, true);

            if (validacao.Valido)
            {
                rnVelocidade.Atualiza(velocidade);
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }

            grdVelocidade.DataBind();
        }

        protected void grdVelocidade_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            ValidacaoDados validacao = new ValidacaoDados();
            RN.FiscalizacaoLink.Velocidade rnVelocidade = new RN.FiscalizacaoLink.Velocidade();
            int velocidadeId = 0;

            velocidadeId = Convert.ToInt32(e.Keys["VELOCIDADEID"]);

            validacao = rnVelocidade.ValidaRemocao(velocidadeId);

            if (validacao.Valido)
            {
                rnVelocidade.Remove(velocidadeId);
                grdVelocidade.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
            }
        }
    }
}
