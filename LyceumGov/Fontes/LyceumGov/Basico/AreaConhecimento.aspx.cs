using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
    using Techne.Lyceum.RN.Entidades;
    [
        NavUrl("~/Basico/AreaConhecimento.aspx"),
        ControlText("AreasConhecimento"),
        Title("Áreas de Conhecimento"),
    ]
    public partial class AreaConhecimento : TPage
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                TituloGrid(grdAreaConhecimento, "Áreas de Conhecimento");
                lblMensagem.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdAreaConhecimento);
        }

        protected void grdAreaConhecimento_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdAreaConhecimento.Settings.ShowFilterRow = false;
        }

        protected void grdAreaConhecimento_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdAreaConhecimento.Settings.ShowFilterRow = false;
        }

        protected void grdAreaConhecimento_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (grdAreaConhecimento.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "pais")
                {
                    e.Editor.Enabled = true;
                }
            }
            else if (grdAreaConhecimento.IsEditing)
            {
                if ((e.Column.FieldName) == "pais")
                {
                    e.Editor.Enabled = false;
                }
            }
        }

        protected void grdAreaConhecimento_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdAreaConhecimento);
        }

        protected void odsAreaConhecimento_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var validacao = new ValidacaoDados();
            string id = e.InputParameters["AREACONHECIMENTOID"].ToString();

            validacao = RN.AreaConhecimento.ValidarExclusao(Convert.ToInt32(id));

            if (!validacao.Valido)
            {
                throw new Exception(validacao.Mensagem.ToString());
            }
            else
            {
                if (RN.AreaConhecimento.Remover(int.Parse(id)) > 0)
                {
                    odsAreaConhecimento.Select();
                    odsAreaConhecimento.DataBind();
                    grdAreaConhecimento.DataBind();
                }
            }
        }

        protected void odsAreaConhecimento_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var validacao = new ValidacaoDados();

            var AC = new AreasConhecimento
            {
                AreaConhecimentoId = Convert.ToInt32(e.InputParameters["AREACONHECIMENTOID"]),
                Descricao = e.InputParameters["DESCRICAO"].ToString()
            };

            validacao = RN.AreaConhecimento.Validar(AC);

            if (!validacao.Valido)
            {
                throw new Exception(validacao.Mensagem.ToString());
            }
            else
            {
                if (RN.AreaConhecimento.Alterar(AC) > 0)
                {
                    odsAreaConhecimento.Select();
                    odsAreaConhecimento.DataBind();
                    grdAreaConhecimento.DataBind();
                }
            }
        }

        protected void odsAreaConhecimento_Insert(object sender, ObjectDataSourceMethodEventArgs e)
        {
            AreasConhecimento AC = new AreasConhecimento();
            var validacao = new ValidacaoDados();
            AC.Descricao = e.InputParameters["DESCRICAO"].ToString();
            validacao = RN.AreaConhecimento.Validar(AC);

            if (validacao.Valido)
            {
                if (RN.AreaConhecimento.Inserir(AC) > 0)
                {
                    odsAreaConhecimento.Select();
                    odsAreaConhecimento.DataBind();
                    grdAreaConhecimento.DataBind();

                }

            }
            else
            {
            }
        }

        public static void InsertArea(string DESCRICAO)
        {

        }

        public static void DeleteArea(int AREACONHECIMENTOID)
        {

        }

        public static void AlterArea(int AREACONHECIMENTOID, string DESCRICAO)
        {

        }

        public object Listar()
        {
            return RN.AreaConhecimento.Listar();
        }
    }
}
