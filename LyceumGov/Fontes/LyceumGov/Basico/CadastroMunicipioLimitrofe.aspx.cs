namespace Techne.Lyceum.Net.Basico
{
    using System;
    using System.Web.UI.WebControls;
    using Techne.Controls;
    using Techne.Lyceum.RN;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Web;

    [NavUrl("~/Basico/CadastroMunicipioLimitrofe.aspx"), ControlText("Municípios Limítrofes"), Title("Municípios Limítrofes")]
    public partial class CadastroMunicipioLimitrofe : TPage
    {
        public void Delete(object ID_MUNICIPIO_LIMITROFE)
        {
        }

        public object Listar(object ID_MUNICIPIO)
        {
            if (ID_MUNICIPIO == null
                || string.IsNullOrEmpty(ID_MUNICIPIO.ToString()))
            {
                return null;
            }

            return MunicipioLimitrofe.Listar(ID_MUNICIPIO.ToString());
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdMunicipioLimitrofe, "Municípios Limítrofes");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.lblMensagem.Text = string.Empty;

            if (!this.IsPostBack)
            {
                this.ddlUFOrigem.DataSource = Municipio.ListarUF();
                this.ddlUFOrigem.Items.Insert(0, "Selecione");
                this.ddlUFOrigem.DataBind();
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            this.ControlaAcesso(this.grdMunicipioLimitrofe);
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            var uf = string.Empty;

            if (!string.IsNullOrEmpty(this.ddlUFLimitrofe.SelectedValue)
                && this.ddlUFLimitrofe.SelectedValue != "Selecione")
            {
                uf = this.ddlUFLimitrofe.SelectedValue;
            }

            var municipioLimitrofe = new TceMunicipioLimitrofe
                                     {
                                         CodigoMunicipio = this.tseMunicipio.DBValue.ToString(),
                                         CodigoMunicipioLimitrofe = this.tseMunicipioLimitrofe.DBValue.ToString(),
                                         Uf = uf,
                                         Matricula = this.User.Identity.Name
                                     };
            var validacao = MunicipioLimitrofe.Validar(municipioLimitrofe);

            if (validacao.Valido)
            {
                MunicipioLimitrofe.Inserir(municipioLimitrofe);

                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Município Limítrofe incluído com sucesso.');", true);

                this.LimparCampos();

                this.odsMunicipioLimitrofe.Select();
                this.odsMunicipioLimitrofe.DataBind();
                this.grdMunicipioLimitrofe.DataBind();
            }
            else
            {
                this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
            }
        }

        protected void ddlUFLimitrofe_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.tseMunicipioLimitrofe.ResetValue();

            if ((this.ddlUFLimitrofe.SelectedValue != "Selecione") && (!string.IsNullOrEmpty(this.tseMunicipio.DBValue.ToString())))
            {
                this.tseMunicipioLimitrofe.SqlWhere = " UF = '" + this.ddlUFLimitrofe.SelectedValue + "' AND ID_MUNICIPIO <> '" + this.tseMunicipio.DBValue + "'";
            }
        }

        protected void ddlUFOrigem_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pnlLimitrofe.Visible = false;
            this.tseMunicipio.ResetValue();
            this.ddlUFLimitrofe.Items.Clear();
            this.tseMunicipioLimitrofe.ResetValue();

            if (this.ddlUFOrigem.SelectedValue != "Selecione")
            {
                this.tseMunicipio.SqlWhere = " UF = '" + this.ddlUFOrigem.SelectedValue + "'";
            }
        }

        protected void odsMunicipioLimitrofe_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var id = e.InputParameters["ID_MUNICIPIO_LIMITROFE"].ToString();

            MunicipioLimitrofe.Remover(int.Parse(id));
        }

        protected void tseMunicipioLimitrofe_Changed(object sender, ChangedEventArgs args)
        {
            if (this.Page.IsCallback)
            {
                return;
            }

            if (!this.tseMunicipioLimitrofe.DBValue.IsNull)
            {
                if (this.tseMunicipioLimitrofe.IsValidDBValue)
                {
                    if (this.ddlUFLimitrofe.SelectedValue == "Selecione")
                    {
                        this.ddlUFLimitrofe.SelectedValue = this.tseMunicipioLimitrofe["uf"].ToString();
                    }
                }
            }
            else
            {
                if (this.ddlUFLimitrofe.SelectedValue != "Selecione")
                {
                    this.tseMunicipioLimitrofe.SqlWhere = " UF = '" + this.ddlUFLimitrofe.SelectedValue + "'";
                }
            }
        }

        protected void tseMunicipio_Changed(object sender, ChangedEventArgs args)
        {
            if (this.Page.IsCallback)
            {
                return;
            }

            this.pnlLimitrofe.Visible = false;
            this.tseMunicipioLimitrofe.ResetValue();
            this.ddlUFLimitrofe.Items.Clear();

            if (!this.tseMunicipio.DBValue.IsNull)
            {
                if (this.tseMunicipio.IsValidDBValue)
                {
                    this.ddlUFLimitrofe.DataSource = Municipio.ListarUF();
                    this.ddlUFLimitrofe.Items.Insert(0, "Selecione");
                    this.ddlUFLimitrofe.DataBind();

                    if (this.ddlUFOrigem.SelectedValue == "Selecione")
                    {
                        this.ddlUFOrigem.SelectedValue = this.tseMunicipio["uf"].ToString();
                    }

                    this.tseMunicipioLimitrofe.SqlWhere = " ID_MUNICIPIO <> '" + this.tseMunicipio.DBValue + "'";

                    this.pnlLimitrofe.Visible = true;
                }
            }
            else
            {
                if (this.ddlUFOrigem.SelectedValue != "Selecione")
                {
                    this.tseMunicipio.SqlWhere = " UF = '" + this.ddlUFOrigem.SelectedValue + "'";
                }
            }

            this.odsMunicipioLimitrofe.Select();
            this.odsMunicipioLimitrofe.DataBind();
            this.grdMunicipioLimitrofe.DataBind();
        }

        private void LimparCampos()
        {
            this.lblMensagem.Text = string.Empty;
            this.tseMunicipioLimitrofe.ResetValue();
            this.tseMunicipioLimitrofe.SqlWhere = " CODIGO <> '" + this.tseMunicipio.DBValue + "'";
            this.ddlUFLimitrofe.ClearSelection();
        }
    }
}