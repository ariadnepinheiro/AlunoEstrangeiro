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
        NavUrl("~/Basico/TipoCursoCapacitacao.aspx"),
        ControlText("TipoCursoCapacitacao"),
        Title("Tipo de Curso de Capacitação"),
    ]
    public partial class TipoCursoCapacitacao : TPage
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
                TituloGrid(grdTipoCursoCapacitacao, "Tipo de Curso de Capacitação");
                lblMensagem.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdTipoCursoCapacitacao);
        }

        protected void grdTipoCursoCapacitacao_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdTipoCursoCapacitacao.Settings.ShowFilterRow = false;
        }

        protected void grdTipoCursoCapacitacao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdTipoCursoCapacitacao.Settings.ShowFilterRow = false;
        }

        protected void grdTipoCursoCapacitacao_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e)
        {
            if (grdTipoCursoCapacitacao.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "pais")
                {
                    e.Editor.Enabled = true;
                }
            }
            else if (grdTipoCursoCapacitacao.IsEditing)
            {
                if ((e.Column.FieldName) == "pais")
                {
                    e.Editor.Enabled = false;
                }
            }
        }

        protected void grdTipoCursoCapacitacao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdTipoCursoCapacitacao);
        }

        protected void odsTipoCursoCapacitacao_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var validacao = new ValidacaoDados();
            string id = e.InputParameters["TIPOCURSOCAPACITACAOID"].ToString();

            validacao = RN.TipoCursoCapacitacao.ValidarExclusao(Convert.ToInt32(id));

            if (!validacao.Valido)
            {
                throw new Exception(validacao.Mensagem.ToString());
            }
            else
            {
                if (RN.TipoCursoCapacitacao.Remover(int.Parse(id)) > 0)
                {
                    odsTipoCursoCapacitacao.Select();
                    odsTipoCursoCapacitacao.DataBind();
                    grdTipoCursoCapacitacao.DataBind();
                }
            }
        }

        protected void odsTipoCursoCapacitacao_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            RN.TipoCursoCapacitacao rnTipoCursoCapacitacao = new Techne.Lyceum.RN.TipoCursoCapacitacao();
            var validacao = new ValidacaoDados();

            var TCC = new TiposCursosCapacitacao()
            {
                TipoCursoCapacitacaoId = Convert.ToInt32(e.InputParameters["TIPOCURSOCAPACITACAOID"]),
                Descricao = e.InputParameters["DESCRICAO"].ToString()
            };

            validacao = RN.TipoCursoCapacitacao.Validar(TCC);

            if (!validacao.Valido)
            {
                throw new Exception(validacao.Mensagem.ToString());
            }
            else
            {
                    rnTipoCursoCapacitacao.Atualiza(TCC);
                    odsTipoCursoCapacitacao.Select();
                    odsTipoCursoCapacitacao.DataBind();
                    grdTipoCursoCapacitacao.DataBind();
                
            }
        }

        protected void odsTipoCursoCapacitacao_Insert(object sender, ObjectDataSourceMethodEventArgs e)
        {
            TiposCursosCapacitacao TCC = new TiposCursosCapacitacao();
            var validacao = new ValidacaoDados();
            TCC.Descricao = e.InputParameters["DESCRICAO"].ToString();
            validacao = RN.TipoCursoCapacitacao.Validar(TCC);

            if (validacao.Valido)
            {
                if (RN.TipoCursoCapacitacao.Inserir(TCC) > 0)
                {
                    odsTipoCursoCapacitacao.Select();
                    odsTipoCursoCapacitacao.DataBind();
                    grdTipoCursoCapacitacao.DataBind();

                }
            }           
        }

        public static void InsertTipo(string DESCRICAO)
        {
            
        }

        public static void DeleteTipo(int TIPOCURSOCAPACITACAOID)
        {

        }

        public static void AlterTipo(int TIPOCURSOCAPACITACAOID, string DESCRICAO)
        {

        }
        
        public object Listar()
        {
            return RN.TipoCursoCapacitacao.Listar();
        }
        }
}
