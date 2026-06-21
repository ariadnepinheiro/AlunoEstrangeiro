using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxEditors;
using Techne.Controls;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Entidades;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/DiretoriaRegional.aspx"), ControlText("DiretoriaRegional"), Title("Diretoria Regional")]
    public partial class DiretoriaRegional : TPage
    {
        public object Listar(object regional)
        {
            var reg = regional.ToString();

            if (string.IsNullOrEmpty(reg))
            {
                return Regional.Listar();
            }

            return Regional.Listar(int.Parse(reg));
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(this.grdRegional, "Diretoria Regional");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            this.lblMensagem.Text = string.Empty;
            this.ValidarCampos();
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnSalvar, AcaoControle.novo);
            this.ControlaAcesso(this.grdRegional);
        }

        private void ValidarCampos()
        {
            this.txtEnd_Num.Attributes.Add("onkeyPress", "return onlyNumbers(event, this);");
            this.txtEnd_Num.Attributes.Add("onkeyDown", "return BloquearCtrl(event, this);");
            this.txtEnd_Num.Attributes.Add("onmousedown", "return desabilitaBotaoDireito(event, this);");
        }

        private void LimparCampos()
        {
            hdnIdRegional.Value = "-1";
            this.lblMensagem.Text = string.Empty;
            this.txtNome.Text = string.Empty;          
            tsCEP.ResetValue();
            this.txtEndereco.Text = string.Empty;
            this.txtEnd_Num.Text = string.Empty;
            this.txtEnd_Compl.Text = string.Empty;
            this.txtBairro.Text = string.Empty;
            txtCEP.Text = string.Empty;
            txtMunicipio.Text = string.Empty;
            tseRegional.ResetValue();           
            txtEstado.Value = string.Empty;
        }

        protected void tseRegional_Changed(object sender, ChangedEventArgs args)
        {
            if (this.Page.IsCallback)
            {
                return;
            }

            if (!tseRegional.DBValue.IsNull)
            {
                if (tseRegional.IsValidDBValue)
                {
                    var reg = Regional.Carregar(Convert.ToInt32(tseRegional.DBValue));
                    hdnIdRegional.Value = reg.IdRegional.ToString();
                    txtNome.Text = reg.Regional;                   
                    txtMunicipio.Text = reg.NomeMunicipio;
                    hdnCodMunicipio.Value = reg.Municipio;
                    txtEstado.Value = reg.Uf;
                    txtCEP.Text = reg.Cep;
                    txtEndereco.Text = reg.Logradouro;
                    txtEnd_Compl.Text = reg.Complemento;
                    txtEnd_Num.Text = reg.Numero;
                    txtBairro.Text = reg.Bairro;

                    lblMensagem.Text = string.Empty;
                }
                else
                {
                    LimparCampos();
                    lblMensagem.Text = "Regional não cadastrada (favor verificar).";
                }
            }
            else
            {
                LimparCampos();
                lblMensagem.Text = "Regional não cadastrada (favor verificar).";
            }

        }

        protected void grdRegional_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdRegional.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "ID_REGIONAL")
                    e.Editor.Enabled = true;               
                if ((e.Column.FieldName) == "NOME")
                    e.Editor.Enabled = true;

            }
            else if (grdRegional.IsEditing)
            {
                if ((e.Column.FieldName) == "ID_REGIONAL")
                    e.Editor.ReadOnly = true;             
                if ((e.Column.FieldName) == "NOME")
                    e.Editor.ReadOnly = true;

                if ((e.Column.FieldName) == "MUNICIPIO")
                {
                    ASPxComboBox cmbMunicipio = (e.Editor as ASPxComboBox);

                    cmbMunicipio.Items.Clear();
                    cmbMunicipio.DataSource = RN.Municipio.ListarMunicipio("RJ");
                    cmbMunicipio.TextField = "NOME";
                    cmbMunicipio.ValueField = "CODIGO";
                    cmbMunicipio.DataBind();

                    var item = cmbMunicipio.Items.FindByText(e.Value.ToString());

                    if (item != null)
                    {
                        item.Selected = true;
                    }
                }
            }
        }
        protected void grdRegional_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdRegional.Settings.ShowFilterRow = false;

        }
        protected void grdRegional_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdRegional.Settings.ShowFilterRow = false;
        }

        protected void odsRegional_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (e.InputParameters["CEP"] == null)
            {
                throw new Exception("O campo Cep é de preenchimento obrigatório.");
            }

            if (e.InputParameters["MUNICIPIO"] == null)
            {
                throw new Exception("O campo Município é de preenchimento obrigatório.");
            }

            if (e.InputParameters["LOGRADOURO"] == null)
            {
                throw new Exception("O campo Logradouro é de preenchimento obrigatório.");
            }
            if (e.InputParameters["NUMERO"] == null)
            {
                throw new Exception("O campo Número é de preenchimento obrigatório.");
            }

            var regional = new TceRegional
            {
                IdRegional = int.Parse(e.InputParameters["ID_REGIONAL"].ToString()),
                Regional = e.InputParameters["REGIONAL"].ToString().Trim().ToUpper(),
                Cep = e.InputParameters["CEP"].ToString(),
                Municipio = e.InputParameters["MUNICIPIO"].ToString(),
                Logradouro = e.InputParameters["LOGRADOURO"].ToString().Trim().ToUpper(),
                Numero = e.InputParameters["NUMERO"].ToString(),
                Complemento = e.InputParameters["COMPLEMENTO"].ToString().Trim().ToUpper(),
                Bairro = e.InputParameters["BAIRRO"].ToString().Trim().ToUpper(),
                Matricula = this.User.Identity.Name
            };

            Regional.Alterar(regional);
        }

        protected void odsRegional_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var id = e.InputParameters["ID_REGIONAL"].ToString();
            var validacao = Regional.ValidarRemover(Convert.ToInt32(id));

            if (!validacao.Valido)
            {
                throw new Exception(validacao.Mensagem);
            }

            Regional.Remover(int.Parse(id));
        }

        public void Update(object REGIONAL, object CEP, object MUNICIPIO, object NOME, object LOGRADOURO, object NUMERO, object COMPLEMENTO, object BAIRRO, object ID_REGIONAL)
        {
        }
        public void Delete(object ID_REGIONAL)
        {
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            var mensagem = string.Empty;

            var regional = new TceRegional
            {
                IdRegional = (string.IsNullOrEmpty(hdnIdRegional.Value) || hdnIdRegional.Value == "-1") ? -1 : Convert.ToInt32(hdnIdRegional.Value),
                Regional = txtNome.Text.Trim().ToUpper(),
                Cep = txtCEP.Text.Trim(),               
                Municipio = hdnCodMunicipio.Value.Trim(),
                Logradouro = txtEndereco.Text.Trim().ToUpper(),
                Numero = txtEnd_Num.Text.Trim(),
                Complemento = txtEnd_Compl.Text.Trim().ToUpper(),
                Bairro = txtBairro.Text.Trim().ToUpper(),
                Matricula = this.User.Identity.Name
            };

            var validacao = Regional.Validar(regional);

            if (validacao.Valido)
            {
                if (string.IsNullOrEmpty(hdnIdRegional.Value) || hdnIdRegional.Value == "-1")
                {
                    Regional.Inserir(regional);
                    mensagem = "Diretoria Regional incluída com sucesso.";

                }
                else
                {
                    Regional.Alterar(regional);
                    mensagem = "Diretoria Regional atualizada com sucesso.";
                }

                var script = @"alert('" + mensagem + @"');";

                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);

                this.LimparCampos();

                this.odsRegional.Select();
                this.odsRegional.DataBind();
                this.grdRegional.DataBind();
            }
            else
            {
                this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
            }
        }
    }
}
