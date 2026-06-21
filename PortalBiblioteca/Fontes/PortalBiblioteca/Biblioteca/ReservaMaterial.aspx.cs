using System;
using System.Web;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.RN;
using Techne.Web;

namespace Techne.Lyceum.Net.Biblioteca
{
    [NavUrl("~/Biblioteca/ReservaMaterial.aspx"),
    ControlText("ReservaMaterial"),
    Title("Reserva"),]

    public partial class ReservaMaterial : TPage
    {
        #region Código gerado Techne
        public static string GetUrl()
        {
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
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
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(QueryStringDecodificada["id"]))
                {
                    hddTitulo.Value = QueryStringDecodificada["id"];
                    Consultar(hddTitulo.Value);
                }
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdReserva, "Exemplares");
        }

        private void Consultar(string titulo)
        {
            QueryTable qt = RN.Biblioteca.ConsultarTitulo2(titulo);

            if (qt != null && qt.Rows.Count > 0)
            {
                PreencherDadosTela(qt);
                pnReservas.Visible = true;
            }

            grdReserva.Visible = true;
            odsReserva.Select();
            odsReserva.DataBind();
            grdReserva.DataBind();
        }

        private void PreencherDadosTela(QueryTable qt)
        {
            if (qt.Rows[0]["imagem"] != null && qt.Rows[0]["imagem"] != DBNull.Value)
            {
                bnImageLivro.ContentBytes = (byte[])qt.Rows[0]["imagem"];
            }
            lblTitulo.Text = qt.Rows[0]["titulo"].ToString();
            lblAutor.Text = qt.Rows[0]["autores"].ToString();
            lblEditora.Text = qt.Rows[0]["editora"].ToString();
        }

        protected void grdReserva_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            lblMensagem.Text = string.Empty;

            string usuario = HttpContext.Current.User.Identity.Name;
            if (string.IsNullOrEmpty(usuario))
            {
                lblMensagem.Text = "É necessário entrar com usuário e senha no sistema para fazer uma reserva.";
                return;
            }

           
            Techne.Lyceum.CR.Ly_bib_reserva.Row row = new Techne.Lyceum.CR.Ly_bib_reserva().NewRow();
            row.Id_bib_usuario = RN.Biblioteca.ConsultarIDUsuario(usuario); //obter usuário em ly_bib_usuario através da pessoa logada no ssitema
            row.Id_bib_material = Convert.ToDecimal(grdReserva.GetRowValues(e.VisibleIndex, "codigo"));
            row.Id_bib_titulo = Convert.ToDecimal(hddTitulo.Value);
            row.Dt_reserva = DateTime.Today;
            row.Dt_validade = DateTime.Today.AddDays(30);
            decimal id_bib_bilioteca = Convert.ToDecimal(grdReserva.GetRowValues(e.VisibleIndex, "id_bib_biblioteca"));

            //não reservar se o usuário logado não for usuário da biblioteca onde se encontra o material
            if (!RN.Biblioteca.VerificaExisteUsuarioBib(usuario,id_bib_bilioteca))
            {
                lblMensagem.Text = "Usuário não cadastrado na biblioteca onde tentou realizar reserva.";
                return;
            }


            RetValue retorno = null;

            if (e.ButtonID == "btnReservar")
            {
                try
                {
                    retorno = RN.Biblioteca.FazerReserva(row);
                    if (retorno != null)
                    {
                        if (!retorno.Ok)
                        {
                            throw new Exception(retorno.Errors.ToString());
                        }
                        else
                        {
                            lblMensagem.Text = retorno.Message;
                            odsReserva.Select();
                            odsReserva.DataBind();
                            grdReserva.DataBind();
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblMensagem.Text = ex.Message;
                }
            }
        }

        public object Listar(string titulo)
        {
            QueryTable qt = null;

            if (!string.IsNullOrEmpty(titulo))
            {
                qt = RN.Biblioteca.ConsultarMateriais(titulo);
            }
            return qt;
        }

    }
}
