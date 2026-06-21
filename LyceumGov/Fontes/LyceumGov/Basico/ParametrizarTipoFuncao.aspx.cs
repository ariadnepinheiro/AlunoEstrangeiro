using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Techne.Lyceum.RN.Util;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/ParametrizarTipoFuncao.aspx")]
    [ControlText("ParametrizarTipoFuncao")]
    [Title("Parametrizar Tipo Função")]
    public partial class ParametrizarTipoFuncao : TPage
    {
        public object Listar()
        {
            RN.TipoFuncao rnTipoFuncao = new Techne.Lyceum.RN.TipoFuncao();

            return rnTipoFuncao.ListaFuncaoTipoFuncao();
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdTipoFuncao, "Tipo Função");
        }
       
        protected void grdTipoFuncao_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            if (e.NewValues["FUNCAOID"] == null)
            {
                e.RowError = "Favor informar a Função.";
                return;
            }

            if (e.NewValues["TIPOFUNCAO"] == null)
            {
                e.RowError = "Necessário informar o Tipo de Função.";
                return;
            }
        }
        protected void grdTipoFuncao_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            RN.Entidades.Funcao funcao = new Techne.Lyceum.RN.Entidades.Funcao();
            RN.Funcao rnFuncao = new Techne.Lyceum.RN.Funcao();
            ValidacaoDados validacao = new ValidacaoDados();

            funcao.FuncaoId = Convert.ToInt32(e.NewValues["FUNCAOID"]);
            funcao.TipoFuncaoId = Convert.ToInt32(e.NewValues["TIPOFUNCAO"]);
            funcao.Descricao = hdnDescricaoFuncao.Value;

            validacao = rnFuncao.Valida(funcao);

            if (validacao.Valido)
            {
                rnFuncao.Insere(funcao);
                grdTipoFuncao.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem);
            }
        
        }
        protected void grdTipoFuncao_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            RN.Entidades.Funcao funcao = new Techne.Lyceum.RN.Entidades.Funcao();
            RN.Funcao rnFuncao = new Techne.Lyceum.RN.Funcao();
            ValidacaoDados validacao = new ValidacaoDados();

            funcao.FuncaoId = Convert.ToInt32(e.Values["FUNCAOID"]);
            funcao.TipoFuncaoId = Convert.ToInt32(e.Values["TIPOFUNCAOID"]);

            validacao = rnFuncao.ValidaRemocaoTipoFuncao(funcao);

            if (validacao.Valido)
            {
                rnFuncao.RemoveTipoFuncao(funcao);
                grdTipoFuncao.DataBind();
            }
            else
            {
                e.Cancel = true;
                throw new Exception(validacao.Mensagem);
            }
        }
        protected void grdTipoFuncao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTipoFuncao.Settings.ShowFilterRow = false;
        }
        public void Insert(object FUNCAOID,object TIPOFUNCAO)
        {
        }

        public void Delete(object FUNCAOID) { }

    }
}
