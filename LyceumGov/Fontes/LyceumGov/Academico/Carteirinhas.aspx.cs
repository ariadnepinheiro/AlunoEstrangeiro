using System;
using System.Data;
using System.Web;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Web;


namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/Carteirinhas.aspx"),
      ControlText("Carteirinhas"),
      Title("Cartão do Aluno"),]
    public partial class Carteirinhas : TPage
    {
        //#region Propriedades

        //private DateTime DataAtual
        //{
        //    get { return (DateTime)ViewState["_dtAtual"]; }
        //    set { ViewState["_dtAtual"] = value; }
        //}

        //private String SituacaoAtual
        //{
        //    get { return (String)ViewState["_sitAtual"]; }
        //    set { ViewState["_sitAtual"] = value; }
        //}


        //#endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                grdCarteirinha.Visible = false;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
           // CarregaGrid();
        }

        //protected void Page_PreRenderComplete(object sender, EventArgs e)
        //{
        //    ControlaAcesso(grdCarteirinha);
        //}

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdCarteirinha, "Cartão do Aluno");
        }
        
        //public static string GetUrl()
        //{
        //    #region Código gerado Techne
        //    return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
        //    #endregion
        //}

        //#region Web Form Designer generated code
        //override protected void OnInit(EventArgs e)
        //{
        //    InitializeComponent();
        //    base.OnInit(e);
        //}

        //private void InitializeComponent()
        //{
        //}
        //#endregion

        //protected void grdCarteirinha_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        //{
        //    ControlaAcesso(grdCarteirinha);
        //}

        //protected void grdCarteirinha_CustomUnboundColumnData(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewColumnDataEventArgs e)
        //{
        //    if (e.Column.FieldName == "CompositeKey")
        //    {
        //        string pessoa = e.GetListSourceFieldValue("pessoa").ToString();
        //        string viaCarteirinha = e.GetListSourceFieldValue("via_carteirinha").ToString();
        //        e.Value = pessoa + "|" + tseAluno.DBValue.ToString() + "|" + viaCarteirinha;
        //    }
        //}

        //protected void grdCarteirinha_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        //{
        //    string[] chaves = e.Keys["CompositeKey"].ToString().Split('|');
        //    e.Keys.Clear();
        //    e.Keys.Add("pessoa", chaves[0]);
        //    e.Keys.Add("aluno", chaves[1]);
        //    e.Keys.Add("via_carteirinha", chaves[2]);

        //    e.NewValues["data_alt_situacao"] = DateTime.Now;
        //    e.NewValues["aluno"] = tseAluno.DBValue.ToString();
        //}

        //protected void grdCarteirinha_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        //{
        //    e.Keys.Clear();
        //    e.Keys.Add("pessoa", e.Values["pessoa"]);
        //    e.Keys.Add("aluno", tseAluno.DBValue.ToString());
        //    e.Keys.Add("via_carteirinha", e.Values["via_carteirinha"]);
        //    e.Values["data_alt_situacao"] = DateTime.Now;
        //    e.Values["aluno"] = tseAluno.DBValue.ToString();
        //}

        //protected void grdCarteirinha_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        //{
        //    if (grdCarteirinha.IsNewRowEditing)
        //    {
        //        if ((e.Column.FieldName) == "data_alt_situacao")
        //        {
        //            e.Editor.Value = DateTime.Now;
        //        }
        //        else if ((e.Column.FieldName) == "via_carteirinha")
        //        {
        //            e.Editor.Enabled = true;
        //        }
        //        else if ((e.Column.FieldName) == "usuario")
        //        {
        //            e.Editor.Value = HttpContext.Current.User.Identity.Name;
        //        }
        //    }
        //    else if (grdCarteirinha.IsEditing)
        //    {
        //        if ((e.Column.FieldName) == "data_alt_situacao")
        //        {
        //            e.Editor.Value = DateTime.Now;
        //        }
        //        else if ((e.Column.FieldName) == "usuario")
        //        {
        //            e.Editor.Value = HttpContext.Current.User.Identity.Name;
        //        }
        //        else if ((e.Column.FieldName) == "via_carteirinha")
        //        {
        //            e.Editor.Enabled = false;
        //        }
        //    }
        //}

        //protected void grdCarteirinha_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        //{
        //    e.NewValues["pessoa"] = (decimal)((DataView)tdsPessoaAluno.Select())[0]["pessoa"];
        //    e.NewValues["data_alt_situacao"] = DateTime.Now;
        //    e.NewValues["aluno"] = tseAluno.DBValue.ToString();
        //}

        protected void grdCarteirinha_PageIndexChanged(object sender, EventArgs e)
        {
            CarregaGrid();
        }

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            if (!tseAluno.DBValue.IsNull)
            {
                if (tseAluno.IsValidDBValue)
                {
                    grdCarteirinha.Visible = true;
                    lblMensagem.Text = string.Empty;
                    CarregaGrid();
                }
                else
                {
                    grdCarteirinha.Visible = false;
                    lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                }
            }
            else
            {
                grdCarteirinha.Visible = false;
            }

        }

        //protected void grdCarteirinha_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        //{
        //    int contador_ativas = 0;
        //    int qdtItemsCarteirinha = ((DataView)tdsCarteirinha.Select()).Count;
        //    DataView viewItemsCarteirinha = (DataView)tdsCarteirinha.Select();

        //    //Se o novo valor é ativa, conta se existe outra ativa na grid
        //    if (e.NewValues["sit_carteirinha"].ToString() == "Ativa")
        //    {
        //        for (int i = 0; i < qdtItemsCarteirinha; i++)
        //        {
        //                //Se houver algum item como "Ativa":
        //            if ((viewItemsCarteirinha)[i]["sit_carteirinha"].ToString() == "Ativa")
        //                    contador_ativas++;
        //        }

        //        if (grdCarteirinha.IsNewRowEditing)
        //        {
        //            if (contador_ativas > 0)
        //                e.RowError = "Só é permitida uma carteirinha ativa por aluno. Desative a existente para ativar outra.";
        //        }
        //        else if (grdCarteirinha.IsEditing)
        //        {
        //            if (e.OldValues["sit_carteirinha"].ToString() != "Ativa" && contador_ativas != 0)
        //                e.RowError = "Só é permitida uma carteirinha ativa por aluno. Desative a existente para ativar outra.";
        //        }

        //    }
            
        //    //data não podem ser menores que 1900 e maiores que data atual
        //    DateTime hoje = DateTime.Now;
        //    DateTime milnov = new DateTime(1900, 1, 1);
        //    DateTime datasolicit = Convert.ToDateTime(e.NewValues["dt_solicitacao"]);

        //    if (datasolicit > hoje)
        //        e.RowError = "Data de solicitação não pode ser maior que data atual.";

        //    if (datasolicit < milnov)
        //        e.RowError = "Data de solicitação não pode ser menor que 1900.";

        //    //verifica se já existe o código de barras informado
        //    if (!string.IsNullOrEmpty(Convert.ToString(e.NewValues["cod_barras_carteirinha"])))
        //    {
        //        if (RN.Carteirinha.ExisteCarteirinha(e.NewValues["cod_barras_carteirinha"].ToString()))
        //        {
        //            e.RowError = "Esse código de barras já está cadastrado.";
        //        }
        //    }
        //}

        //protected void grdCarteirinha_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        //{
        //    grdCarteirinha.Settings.ShowFilterRow = false;
        //}

        //protected void grdCarteirinha_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        //{
        //    grdCarteirinha.Settings.ShowFilterRow = false;
        //    e.NewValues["dt_solicitacao"] = DateTime.Now;
        //}

        private void CarregaGrid()
        {
            try
            {
                RN.CartaoEstudante.Service.CarteirinhaService rnCarteirinhaService = RN.CartaoEstudante.Service.CarteirinhaService.Instancia;


                 string filtro = tseAluno.Text;


                 grdCarteirinha.DataSource = rnCarteirinhaService.ListaCarteirinhas(filtro);
                grdCarteirinha.DataBind();

                if (grdCarteirinha.VisibleRowCount > 0)
                {
                    grdCarteirinha.Visible = true;
                }
                else
                {
                    lblMensagem.Text = "Não existem cartões para o aluno.";
                    grdCarteirinha.Visible = false;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}