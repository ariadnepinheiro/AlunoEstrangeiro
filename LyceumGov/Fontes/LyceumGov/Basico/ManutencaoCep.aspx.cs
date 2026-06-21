using System;
using System.Data;
using System.Web.UI.WebControls;
using Techne.Controls;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Entidades;
using Techne.Web;
using System.Linq;

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/ManutencaoCep.aspx"), ControlText("ManutencaoCep"), Title("Manutenção CEP")]
    public partial class ManutencaoCep : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }

            string municipioSelecionado = Convert.ToString(tseMunicipio.Value);

            if (!string.IsNullOrEmpty(municipioSelecionado))
            {
                CarregarGrid(municipioSelecionado);
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdCep, "Lista de CEP");
            //CarregarGrid(null);
        }

        private void CarregarGrid(string idMunicipio)
        {
            CepRN rn = new CepRN();

            grdCep.DataSource = !string.IsNullOrEmpty(idMunicipio) ? rn.ListarLogradourosPorMunicipio(idMunicipio) : null;

            grdCep.DataBind();
        }

        private void CarregarMunicipios()
        {
            ddlMunicipio.DataSource = Municipio.ListarTodos();
            ddlMunicipio.DataBind();
            ddlMunicipio.Items.Insert(0, new ListItem("-- Selecione --", ""));
        }

        protected void ddlMunicipio_SelectedIndexChanged(object sender, EventArgs e)
        {
            CarregarBairros(ddlMunicipio.SelectedValue, ddlBairro);
        }

        private void CarregarBairros(string codigoMunicipio, DropDownList ddl)
        {
            if (!string.IsNullOrEmpty(codigoMunicipio))
            {
                Techne.Lyceum.RN.Bairro bairro = new Techne.Lyceum.RN.Bairro();

                ddl.DataSource = bairro.ListaPor(codigoMunicipio);
                ddl.DataBind();
            }
            else
            {
                ddl.Items.Clear();
            }

            ddl.Items.Insert(0, new ListItem("-- Selecione --", ""));
        }

        protected void tseMunicipio_Changed(object sender, EventArgs e)
        {
            string municipioSelecionado = Convert.ToString(tseMunicipio.Value);

            if (!string.IsNullOrEmpty(municipioSelecionado))
            {
                pnlInformacoes.Visible = false;
                btnSalvar.Visible = false;

                novo.Visible = true;
                cancel.Visible = false;

                CarregarGrid(municipioSelecionado);
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";
            lblMensagem.ForeColor = System.Drawing.Color.Red;

            if (!ValidarNovoCep())
                return;

            string novoCep = txtCep.Text.Trim();

            CepRN rn = new CepRN();

            if (rn.CepExiste(novoCep, ddlMunicipio.SelectedValue, ddlBairro.SelectedValue))
            {
                lblMensagem.Text = "Já existe um registro com esse CEP.";
                return;
            }

            rn.AdicionarCep(
                novoCep,
                ddlMunicipio.SelectedValue,
                txtLogradouro.Text.Trim(),
                ddlBairro.SelectedValue
            );

            LimparCampos();

            lblMensagem.ForeColor = System.Drawing.Color.Green;
            lblMensagem.Text = "Logradouro adicionado com sucesso!";
        }

        protected void grdCep_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e)
        {
            try
            {
                string idLogradouro = e.Keys["ID_LOGRADOURO"].ToString();

                CepRN rn = new CepRN();
                rn.ExcluirCep(idLogradouro);

                lblMensagem.ForeColor = System.Drawing.Color.Green;
                lblMensagem.Text = "Registo excluido com sucesso.";

                CarregarGrid(tseMunicipio.Value.ToString());

                e.Cancel = true;
            }
            catch (Exception ex)
            {
                lblMensagem.ForeColor = System.Drawing.Color.Red;
                lblMensagem.Text = "Erro ao excluir registro.";
            }
        }

        private bool ValidarNovoCep()
        {
            if (string.IsNullOrEmpty(txtCep.Text))
            {
                lblMensagem.Text = "Informe o CEP.";
                return false;
            }

            if (!txtCep.Text.All(char.IsDigit))
            {
                lblMensagem.Text = "O CEP deve conter apenas números.";
                return false;
            }

            if (txtCep.Text.Length != 8)
            {
                lblMensagem.Text = "CEP deve possuir 8 números.";
                return false;
            }

            if (string.IsNullOrEmpty(ddlMunicipio.SelectedValue))
            {
                lblMensagem.Text = "Informe o Municipio.";
                return false;
            }

            if (string.IsNullOrEmpty(txtLogradouro.Text))
            {
                lblMensagem.Text = "Informe o Logradouro.";
                return false;
            }

            if (string.IsNullOrEmpty(ddlBairro.SelectedValue))
            {
                lblMensagem.Text = "Informe o Bairro.";
                return false;
            }

            return true;
        }


        private void LimparCampos()
        {
            tseMunicipio.Value = null;

            txtCep.Text = "";
            txtLogradouro.Text = "";

            ddlMunicipio.Items.Clear();
            ddlBairro.Items.Clear();

            lblMensagem.Text = "";

            CarregarGrid(null);
        }

        protected void novo_Click(object sender, EventArgs e)
        {
            pnlInformacoes.Visible = true;
            btnSalvar.Visible = true;

            novo.Visible = false;
            cancel.Visible = true;

            LimparCampos();
            CarregarMunicipios();
        }

        protected void cancel_Click(object sender, EventArgs e)
        {
            pnlInformacoes.Visible = false;
            btnSalvar.Visible = false;

            novo.Visible = true;
            cancel.Visible = false;

            LimparCampos();
        }
    }
}