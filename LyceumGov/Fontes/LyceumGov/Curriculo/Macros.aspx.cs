using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Entidades;
using Techne.Web;
using DevExpress.Web.ASPxTabControl;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Curriculo
{
    [
        NavUrl("~/Curriculo/Macros.aspx"),
         ControlText("Macrocampos"),
         Title("Macrocampos"),
    ]
    public partial class Macros : TPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdMacro, "Macrocampos");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;
        }

        #region Events Clicks

        protected void pcTermo_TabClick(object source, TabControlCancelEventArgs e)
        {
            this.lblMensagem.Text = string.Empty;
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                var TMC = new TceMacroCampos
                {
                    Matricula = this.User.Identity.Name,
                    Nome = txtNome.Text.Trim(),
                    Obrigatorio = chkObrigatoria.Checked
                };

                var validacao = RN.MacroCampos.Validar(TMC);

                if (validacao.Valido)
                {
                    RN.MacroCampos.Inserir(TMC);

                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Macro incluído com sucesso.');", true);

                    this.LimparCampos();

                    this.odsMacro.Select();
                    this.odsMacro.DataBind();
                    this.grdMacro.DataBind();
                }
                else
                {
                    if (!string.IsNullOrEmpty(validacao.Mensagem))
                    {
                        this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    }
                }
            }
            catch (Exception ex)
            {
                this.lblMensagem.Text = ex.Message;
            }
        }

        #endregion

        #region Events odsMacro

        public object Listar()
        {
            return RN.MacroCampos.Listar();
        }

        public void Delete(object ID_MACRO_CAMPOS)
        {
        }

        public void Update(object NOME, object OBRIGATORIO, object MATRICULA, object ID_MACRO_CAMPOS)
        {
        }

        protected void odsMacro_Deleting(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var id = e.InputParameters["ID_MACRO_CAMPOS"].ToString();

            var validacao = RN.MacroCampos.ValidarRemover(int.Parse(id));

            if (validacao.Valido)
            {
                RN.MacroCampos.Remover(int.Parse(id));
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void odsMacro_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var TMC = new TceMacroCampos
            {
                IdMacroCampos = Convert.ToInt32(e.InputParameters["ID_MACRO_CAMPOS"]),
                Matricula = this.User.Identity.Name,
                Nome = e.InputParameters["NOME"].ToString(),
                Obrigatorio = (e.InputParameters["OBRIGATORIO"].ToString().ToLower() == "true")
            };
            
            var validacao = RN.MacroCampos.Validar(TMC);

            if (validacao.Valido)
            {
                RN.MacroCampos.Alterar(TMC);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }

            //this.odsMacro.Select();
            //this.odsMacro.DataBind();
            //this.grdMacro.DataBind();
        }

        #endregion

        #region Private Methods

        private void LimparCampos()
        {
            this.lblMensagem.Text = string.Empty;
            txtNome.Text = string.Empty;
        }

        #endregion

        #region grdMacros

        protected void grdMacro_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdMacro.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "MATRICULA")
                    e.Editor.ReadOnly = true;                
            }
            else if (grdMacro.IsEditing)
            {
                if ((e.Column.FieldName) == "MATRICULA")
                    e.Editor.ReadOnly = true;               
            }
        }

        protected void grdMacro_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdMacro.Settings.ShowFilterRow = false;

            if (RN.MacroCampos.ValidaObrigatorioGrade(Convert.ToInt32(e.EditingKeyValue)))
            {
                e.Cancel = true;
                throw new Exception("Item como obrigatório e com ligação a alguma grade. Não pode ser Editado.");
            }
        }

        protected void grdMacro_RowValidating(object sender, DevExpress.Web.Data.ASPxDataValidationEventArgs e)
        {
            lblMensagem.Text = string.Empty;

            if (string.IsNullOrEmpty(Convert.ToString(e.NewValues["NOME"])))
            {
                e.RowError = "Favor informar o Nome";
            }
        }

        protected void grdMacro_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdMacro);
        }

        #endregion

    }
}

