using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.Net.Basico;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxGridView;
using Techne.Web;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxEditors;

namespace Techne.Lyceum.Net.Cadastros
{
    [NavUrl("~/Cadastros/MaeCadastro.aspx")]
    [ControlText("Mae Cadastro")]
    [Title("MAE Cadastro")]

    public partial class MaeCadastro : TPage
    {
        public object Lista(object cpf)
        {
            RN.Cadastros.MaeInscricaoAluno rnMaeInscricaoAluno = new Techne.Lyceum.RN.Cadastros.MaeInscricaoAluno();

            if (cpf != null)
            {
                return rnMaeInscricaoAluno.ListaAlunosPor(cpf.ToString());
            }
            return null;

        }       

        public object ListaLotacao(object cpf)
        {
            RN.Cadastros.MaeLotacao rnMaeLotacao = new Techne.Lyceum.RN.Cadastros.MaeLotacao();

            if (cpf != null)
            {
                return rnMaeLotacao.ListaLotacaoPor(cpf.ToString());
            }

            return null;

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.lblMensagem.Text = string.Empty;

                if (!this.IsPostBack)
                {
                    if (Request.QueryString.Keys.Count > 0)
                    {
                        divPrincipal.Visible = false;
                        if (Request.QueryString["ChaveConfirmacao"] != null)
                        {
                            byte[] decodedBytes = Convert.FromBase64String(Request.QueryString["ChaveConfirmacao"]);
                            string decodedText = System.Text.Encoding.UTF8.GetString(decodedBytes);

                            LimparCampos();

                            string cpf = Convert.ToString(decodedText);

                            tseCPF.DBValue = cpf;
                            tseCPF_Changed(null, null);
                            tseCPF.Mode = Techne.Controls.ControlMode.View;
                           
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimparCampos()
        {
            txtNumeroInscricao.Text = string.Empty;
            txtNomeCompl.Text = string.Empty;
            txtDtNascimento.Text = string.Empty;
            txtSexo.Text = string.Empty;
            txtDesempregado.Text = string.Empty;
            txtSeguro.Text = string.Empty;
            txtCHLivre.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtTelefone.Text = string.Empty;
            txtTelAlternativo.Text = string.Empty;
            txtCPF.Text = string.Empty;
            txtRgNumero.Text = string.Empty;
            txtRgEmissor.Text = string.Empty;
            txtRgUf.Text = string.Empty;
            txtCEP.Text = string.Empty;
            txtLogradouro.Text = string.Empty;
            txtNumero.Text = string.Empty;
            txtComplemento.Text = string.Empty;
            txtUFEndereco.Text = string.Empty;
            txtMunicipioEndereco.Text = string.Empty;
            txtBairro.Text = string.Empty;
            txtEscolaridade.Text = string.Empty;
            txtExprienciaTrabalho.Text = string.Empty;
            txtTurnoTrabalho.Text = string.Empty;
            grdAlunoResponsabilidade.DataBind();
            grdLotacao.DataBind();

        }

        protected void tseCPF_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                RN.Cadastros.MaeInscricao rnMaeInscricao = new Techne.Lyceum.RN.Cadastros.MaeInscricao();
                RN.Cadastros.DTOs.MaeDadosInscricao dados = new Techne.Lyceum.RN.Cadastros.DTOs.MaeDadosInscricao();

                LimparCampos();
                if (this.Page.IsCallback)
                {
                    return;
                }

                this.divPrincipal.Visible = false;

                if (!this.tseCPF.DBValue.IsNull)
                {
                    if (this.tseCPF.IsValidDBValue)
                    {
                        divPrincipal.Visible = true;
                        dados = rnMaeInscricao.ObtemDadosInscricaoPor(tseCPF.DBValue.ToString());

                        if (!dados.CPF.IsNullOrEmptyOrWhiteSpace())
                        {
                            PreencheDados(dados);
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "CPF não encontrado.";
                    }
                }
                else
                {
                    lblMensagem.Text = "CPF não encontrado.";
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void PreencheDados(RN.Cadastros.DTOs.MaeDadosInscricao dados)
        {
            txtNumeroInscricao.Text = dados.MaeInscricaoId != 0 ? dados.MaeInscricaoId.ToString() : string.Empty;
            txtNomeCompl.Text = !dados.Nome.IsNullOrEmptyOrWhiteSpace() ? dados.Nome.Trim() : string.Empty;
            txtDtNascimento.Text = dados.DataNascimento != null ? dados.DataNascimento.ToString("dd/MM/yyyy") : string.Empty;
            txtSexo.Text = !dados.Sexo.IsNullOrEmptyOrWhiteSpace() ? (dados.Sexo == "F" ? "Feminino" : "Masculino") : string.Empty;
            txtDesempregado.Text = dados.Desempregado ? "Sim" : "Não";
            txtSeguro.Text = dados.SeguroDesemprego ? "Sim" : "Não";
            txtCHLivre.Text = dados.CargaHorariaLivre ? "Sim" : "Não";
            txtEmail.Text = !dados.Email.IsNullOrEmptyOrWhiteSpace() ? dados.Email.Trim() : string.Empty;
            txtTelefone.Text = !dados.Celular.IsNullOrEmptyOrWhiteSpace() ? dados.Celular : string.Empty;
            txtTelAlternativo.Text = !dados.FixoCelular.IsNullOrEmptyOrWhiteSpace() ? dados.FixoCelular : string.Empty;
            txtCPF.Text = !dados.CPF.IsNullOrEmptyOrWhiteSpace() ? dados.CPF.AplicarMascaraCPF() : string.Empty;
            txtRgNumero.Text = !dados.NumeroRG.IsNullOrEmptyOrWhiteSpace() ? dados.NumeroRG : string.Empty;
            txtRgEmissor.Text = !dados.OrgaoRG.IsNullOrEmptyOrWhiteSpace() ? dados.OrgaoRG : string.Empty;
            txtRgUf.Text = !dados.UfRG.IsNullOrEmptyOrWhiteSpace() ? dados.UfRG : string.Empty;
            txtCEP.Text = !dados.CEP.IsNullOrEmptyOrWhiteSpace() ? dados.CEP : string.Empty;
            txtLogradouro.Text = !dados.Endereco.IsNullOrEmptyOrWhiteSpace() ? dados.Endereco : string.Empty;
            txtNumero.Text = !dados.Numero.IsNullOrEmptyOrWhiteSpace() ? dados.Numero : string.Empty;
            txtComplemento.Text = !dados.Complemento.IsNullOrEmptyOrWhiteSpace() ? dados.Complemento : string.Empty;
            txtUFEndereco.Text = !dados.UF.IsNullOrEmptyOrWhiteSpace() ? dados.UF : string.Empty;
            txtMunicipioEndereco.Text = !dados.MunicipioDescricao.IsNullOrEmptyOrWhiteSpace() ? dados.MunicipioDescricao : string.Empty;
            txtBairro.Text = !dados.BairroDescricao.IsNullOrEmptyOrWhiteSpace() ? dados.BairroDescricao : string.Empty;
            txtEscolaridade.Text = !dados.EscolaridadeDescricao.IsNullOrEmptyOrWhiteSpace() ? dados.EscolaridadeDescricao : string.Empty;
            txtExprienciaTrabalho.Text = dados.ExperienciaTrabalho ? "Sim" : "Não";

            if (dados.Manha.Value && dados.Tarde.Value)
            {
                txtTurnoTrabalho.Text = "Manhã ou Tarde";
            }
            else if (dados.Manha.Value && !dados.Tarde.Value)
            {
                txtTurnoTrabalho.Text = "Manhã";
            }
            else if (!dados.Manha.Value && dados.Tarde.Value)
            {
                txtTurnoTrabalho.Text = "Tarde";
            }

            if (dados.TomouVacina)
            {
                if (dados.DoseUnica)
                {
                    txtTomouVacina.Text = "Imunizada. Dose única em " + dados.DataVacina1.ToString("dd/MM/yyyy");
                }
                else
                {
                    if (dados.DataVacina2 != null)
                    {
                        txtTomouVacina.Text = "Imunizada. 1ª dose em " + dados.DataVacina1.ToString("dd/MM/yyyy") + " e 2ª dose em " + dados.DataVacina2.Value.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        txtTomouVacina.Text = "Imunizada. 1ª dose em " + dados.DataVacina1.ToString("dd/MM/yyyy");
                    }
                }
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdAlunoResponsabilidade, "Aluno(s) sob minha responsabilidade");
            TituloGrid(grdLotacao, "Lotação");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(grdAlunoResponsabilidade);
            ControlaAcesso(grdLotacao);
        }

        protected void grdAlunoResponsabilidade_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdAlunoResponsabilidade.Settings.ShowFilterRow = false;
        }

        protected void grdAlunoResponsabilidade_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {

            grdAlunoResponsabilidade.Settings.ShowFilterRow = false;
        }

        protected void grdLotacao_AfterPerformCallback(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdLotacao);

        }
        protected void grdLotacao_CancelRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            tseCPF.Enabled = true;
        }
        protected void grdLotacao_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {

            grdLotacao.Settings.ShowFilterRow = false;
        }

        protected void grdLotacao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdLotacao.Settings.ShowFilterRow = false;
        }

        protected void grdLotacao_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e)
        {
            var pode = Convert.ToString(grdLotacao.GetRowValues(e.VisibleIndex, "PODEALTERAR"));

            if (!string.IsNullOrEmpty(pode))
            {
                if (pode != "1")
                {
                    if (e.ButtonType == ColumnCommandButtonType.Edit)
                    {
                        e.Visible = false;
                    }
                }
            }
        }

        protected void grdLotacao_HtmlRowCreated(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewTableRowEventArgs e)
        {
            if (!this.grdLotacao.Visible
             || this.grdLotacao.VisibleRowCount == 0
             )
            {
                return;
            }
            ASPxDateEdit dataInicio = (ASPxDateEdit)grdLotacao.FindEditFormTemplateControl("DATAINICIO");
            TSearchBox tseUnidade = (TSearchBox)grdLotacao.FindEditFormTemplateControl("tseUnidade");

            if (grdLotacao.IsEditing && !grdLotacao.IsNewRowEditing)
            {      
                if (dataInicio != null)
                {
                    dataInicio.ClientEnabled = false;
                }

                if (tseUnidade != null)
                {
                    tseUnidade.Mode = Techne.Controls.ControlMode.View;
                }
            }
            else
            {
                if (dataInicio != null)
                {
                    dataInicio.ClientEnabled = true;
                }

                if (tseUnidade != null)
                {
                    tseUnidade.Mode = Techne.Controls.ControlMode.Edit;
                }
            }
        }

        protected void grdLotacao_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            TSearchBox tseUnidade = (TSearchBox)grdLotacao.FindEditFormTemplateControl("tseUnidade");

            if (grdLotacao.IsNewRowEditing)
            {
                tseCPF.Enabled = false;

                if (tseUnidade != null)
                {
                    tseUnidade.Mode = Techne.Controls.ControlMode.Edit;
                }

                if ((e.Column.FieldName) == "DATAINICIO")
                {
                    e.Editor.ClientEnabled = true;
                }

            }
            else if (grdLotacao.IsEditing)
            {
                tseCPF.Enabled = false;

                if (tseUnidade != null)
                {
                    tseUnidade.Mode = Techne.Controls.ControlMode.View;
                }               
              
                if ((e.Column.FieldName) == "DATAINICIO")
                {
                    e.Editor.ClientEnabled = false;
                }
            }
        }

        public void Insert(object DATAFIM, object MAE_MOTIVODESLIGAMENTOID, object CENSO, object BANCO, object AGENCIA, object CONTACORRENTE, object DATAINICIO, object DESCRICAOOUTROS) { }
                
        public void Insert(object DATAFIM, object CENSO) { }

        public void Update(object DATAFIM, object MAE_MOTIVODESLIGAMENTOID, object DATAINICIO, object DESCRICAOOUTROS, object MAE_LOTACAOID) { }

        public void Update(object DATAFIM, object MAE_MOTIVODESLIGAMENTOID, object CENSO, object BANCO, object AGENCIA, object CONTACORRENTE, object DATAINICIO, object DESCRICAOOUTROS, object MAE_LOTACAOID) { }
        
        protected void grdLotacao_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Cadastros.MaeLotacao rnMaeLotacao = new Techne.Lyceum.RN.Cadastros.MaeLotacao();
                RN.Cadastros.DTOs.MaeDadosBancarios dados = new Techne.Lyceum.RN.Cadastros.DTOs.MaeDadosBancarios();

                TSearchBox tseBanco = (TSearchBox)grdLotacao.FindEditFormTemplateControl("tseBanco");
                TSearchBox tseAgencia = (TSearchBox)grdLotacao.FindEditFormTemplateControl("tsAgencia");
                TextBox txtConta = (TextBox)grdLotacao.FindEditFormTemplateControl("txtConta");
                ASPxTextBox txtOutroMotivo = (ASPxTextBox)grdLotacao.FindEditFormTemplateControl("txtOutroMotivo");
                
                dados.MaeInscricaoId = Convert.ToInt32(((ASPxGridView)sender).GetRowValuesByKeyValue(e.Keys[0], "MAE_INSCRICAOID"));
                dados.MaeLotacaoId = Convert.ToInt32(e.Keys["MAE_LOTACAOID"]); 
                dados.Banco = (tseBanco.IsValidDBValue && !tseBanco.DBValue.IsNull) ? tseBanco.DBValue.ToString() : null;
                dados.Agencia = (tseAgencia.IsValidDBValue && !tseAgencia.DBValue.IsNull) ? tseAgencia.DBValue.ToString() : null;
                dados.ContaCorrente = !txtConta.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToString(txtConta.Text) : null;
                dados.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
                dados.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : DateTime.MinValue;
                dados.UsuarioId= User.Identity.Name;
                dados.MaeMotivoDesligamentoId = e.NewValues["MAE_MOTIVODESLIGAMENTOID"] != null ? Convert.ToInt32(e.NewValues["MAE_MOTIVODESLIGAMENTOID"]) : -1;
                dados.DescricaoOutros = !txtOutroMotivo.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToString(txtOutroMotivo.Text) : null;

                validacao = rnMaeLotacao.ValidaDadosBancarios(dados, false);


                if (validacao.Valido)
                {
                    rnMaeLotacao.AtualizaDadosBancarios(dados);
                    grdLotacao.DataBind();
                    tseCPF.Enabled = true;
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdLotacao_RowInserting(object sender, DevExpress.Web.Data.ASPxDataInsertingEventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Cadastros.MaeLotacao rnMaeLotacao = new Techne.Lyceum.RN.Cadastros.MaeLotacao();
                RN.Cadastros.DTOs.MaeDadosBancarios dados = new Techne.Lyceum.RN.Cadastros.DTOs.MaeDadosBancarios();

                TSearchBox tseBanco = (TSearchBox)grdLotacao.FindEditFormTemplateControl("tseBanco");
                TSearchBox tseAgencia = (TSearchBox)grdLotacao.FindEditFormTemplateControl("tsAgencia");
                TSearchBox tseUnidade = (TSearchBox)grdLotacao.FindEditFormTemplateControl("tseUnidade");
                TextBox txtConta = (TextBox)grdLotacao.FindEditFormTemplateControl("txtConta");
                ASPxTextBox txtOutroMotivo = (ASPxTextBox)grdLotacao.FindEditFormTemplateControl("txtOutroMotivo");
                
                dados.MaeInscricaoId = Convert.ToInt32(txtNumeroInscricao.Text);
                dados.Censo = (tseUnidade.IsValidDBValue && !tseUnidade.DBValue.IsNull) ? tseUnidade.DBValue.ToString() : null;
                dados.Banco = (tseBanco.IsValidDBValue && !tseBanco.DBValue.IsNull) ? tseBanco.DBValue.ToString() : null;
                dados.Agencia = (tseAgencia.IsValidDBValue && !tseAgencia.DBValue.IsNull) ? tseAgencia.DBValue.ToString() : null;
                dados.ContaCorrente = !txtConta.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToString(txtConta.Text) : null;
                dados.DataInicio = e.NewValues["DATAINICIO"] != null ? Convert.ToDateTime(e.NewValues["DATAINICIO"]) : DateTime.MinValue;
                dados.DataFim = e.NewValues["DATAFIM"] != null ? Convert.ToDateTime(e.NewValues["DATAFIM"]) : (DateTime?)null;
                dados.MaeMotivoDesligamentoId = e.NewValues["MAE_MOTIVODESLIGAMENTOID"] != null ? Convert.ToInt32(e.NewValues["MAE_MOTIVODESLIGAMENTOID"]) : -1;
                dados.DescricaoOutros = !txtOutroMotivo.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToString(txtOutroMotivo.Text) : null;
                dados.UsuarioId = User.Identity.Name;

                validacao = rnMaeLotacao.ValidaDadosBancarios(dados,true);

                if (validacao.Valido)
                {
                    rnMaeLotacao.InsereDadosBancarios(dados);
                    grdLotacao.DataBind();
                    tseCPF.Enabled = true;
                }
                else
                {
                    e.Cancel = true;
                    throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
