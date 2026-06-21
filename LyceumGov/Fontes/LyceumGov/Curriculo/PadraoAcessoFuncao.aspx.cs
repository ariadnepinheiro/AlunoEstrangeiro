using System;
using System.Web.UI.WebControls;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN;
using Techne.Web;

namespace Techne.Lyceum.Net.Curriculo
{
    [NavUrl("~/Curriculo/LotacaoDocente.aspx"),
    ControlText("Lotação Docente"),
    Title("Padrão de Acesso das Funções"),]
    public partial class PadraoAcessoFuncao : TPage
    {
        #region Código Padrão Techne
        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }
        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
        }
        #endregion
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdPadacesFuncao, "Função para Padrão de Acesso");
        }

        protected void grdPadacesFuncao_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdPadacesFuncao);
        }

        protected void grdPadacesFuncao_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            e.NewValues["padaces"] = tsePadrao.DBValue.ToString();
        }

        protected void tsePadrao_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            if (tsePadrao.IsValidDBValue && !tsePadrao.DBValue.IsNull)
            {
                grdPadacesFuncao.Visible = true;
            }
            else
            {
                grdPadacesFuncao.CancelEdit();
                grdPadacesFuncao.Visible = false;
            }
        }

        protected void grdPadacesFuncao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            e.NewValues["padaces"] = tsePadrao.DBValue.ToString();
        }

        protected void odsPadacesFuncao_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            RetValue retorno = null;

            Ly_padaces_funcao dtPadacesFuncao = new Ly_padaces_funcao();
            Ly_padaces_funcao.Row rowPadacesFuncao = dtPadacesFuncao.NewRow();

            rowPadacesFuncao.Funcao = e.InputParameters["funcao"].ToString();
            rowPadacesFuncao.Padaces = e.InputParameters["padaces"].ToString();

            retorno = RN.PadraoAcessoFuncao.Alterar(rowPadacesFuncao);

            if (retorno != null)
            {
                if (!retorno.Ok)
                {
                    throw new Exception(retorno.Errors.ToString());
                }
            }
        }

        protected void odsPadacesFuncao_Inserting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            RetValue retorno = null;

            Ly_padaces_funcao dtPadacesFuncao = new Ly_padaces_funcao();
            Ly_padaces_funcao.Row rowPadacesFuncao = dtPadacesFuncao.NewRow();

            rowPadacesFuncao.Funcao = e.InputParameters["funcao"].ToString();
            rowPadacesFuncao.Padaces = e.InputParameters["padaces"].ToString();

            retorno = RN.PadraoAcessoFuncao.Inserir(rowPadacesFuncao);

            if (retorno != null)
            {
                if (!retorno.Ok)
                {
                    throw new Exception(retorno.Errors.ToString());
                }
            }
        }

        protected void odsPadacesFuncao_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            RetValue retorno = null;

            Ly_padaces_funcao dtPadacesFuncao = new Ly_padaces_funcao();
            Ly_padaces_funcao.Row rowPadacesFuncao = dtPadacesFuncao.NewRow();

            rowPadacesFuncao.Funcao = e.InputParameters["funcao"].ToString();
            rowPadacesFuncao.Padaces = e.InputParameters["padaces"].ToString();

            retorno = RN.PadraoAcessoFuncao.Excluir(rowPadacesFuncao);

            if (retorno != null)
            {
                if (!retorno.Ok)
                {
                    throw new Exception(retorno.Errors.ToString());
                }
            }
        }
    }
}
