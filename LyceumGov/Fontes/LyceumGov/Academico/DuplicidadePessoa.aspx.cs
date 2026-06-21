using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Entidades;
using System.Data;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using DevExpress.Web.ASPxGridView;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/DuplicidadePessoa.aspx"),
    ControlText("Duplicidade de Aluno"),
    Title("Duplicidade de Aluno")]
    public partial class DuplicidadePessoa : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;
            lblMensagemSituacao.Text = string.Empty;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdDuplicidade, "Alunos");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            btnSalvar.Visible = true;
            ControlaAcesso(btnSalvar, AcaoControle.novo);
        }

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            try
            {
                pnDuplicidade.Visible = false;
                pnlGrid.Visible = false;
                pnlInformacao.Visible = false;
                pnlDadosAluno.Visible = false;
                ddlConfirmacaoMatricula.Value = null;
                txtNomeCompl.Text = string.Empty;
                txtNomeMae.Text = string.Empty;
                dtDataNasc.Text = string.Empty;
                hdnMatriculas.Value = string.Empty;
                rblSituacao.ClearSelection();

                if (!tseAluno.DBValue.IsNull)
                {
                    if (tseAluno.IsValidDBValue)
                    {
                        pnDuplicidade.Visible = true;
                        txtNomeCompl.Text = tseAluno["nome"].ToString();
                        txtNomeMae.Text = tseAluno["mae"].ToString();
                        dtDataNasc.Text = tseAluno["dt_nascimento"].ToString();

                        if (txtNomeMae.Text == chkNaoDeclarMae.Text.ToUpper())
                        {
                            chkNaoDeclarMae.Checked = true;
                            txtNomeMae.Text = chkNaoDeclarMae.Text.ToUpper();
                            txtNomeMae.ReadOnly = true;
                        }
                        else
                        {
                            chkNaoDeclarMae.Checked = false;
                            txtNomeMae.ReadOnly = false;
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Favor preencher o campo Aluno.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                hdnMatriculas.Value = string.Empty;
                pnlGrid.Visible = false;
                pnlInformacao.Visible = false;
                pnlDadosAluno.Visible = false;
                rblSituacao.Enabled = true;

                if (string.IsNullOrEmpty(txtNomeCompl.Text) || string.IsNullOrEmpty(txtNomeMae.Text) || string.IsNullOrEmpty(dtDataNasc.Text))
                {
                    lblMensagem.Text = "Para efetuar a busca é necessário preencher todos campos.";
                    return;
                }
                else
                {
                    CarregaGrid(tseAluno.DBValue.ToString(), txtNomeCompl.Text.Trim(), txtNomeMae.Text.Trim(), Convert.ToDateTime(dtDataNasc.Text));
                    pnlGrid.Visible = true;
                    pnlInformacao.Visible = true;

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void chkNaoDeclarMae_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtNomeMae.ReadOnly = false;
                txtNomeMae.Text = string.Empty;

                if (chkNaoDeclarMae.Checked)
                {
                    txtNomeMae.Text = chkNaoDeclarMae.Text.ToUpper();
                    txtNomeMae.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void grdDuplicidade_CustomUnboundColumnData(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "CELULARFORMATADO")
            {
                var celular = e.GetListSourceFieldValue("CELULAR");
                string celularFormatado = string.Empty;
                if (celular != null)
                {
                    long resultado;

                    if (long.TryParse(celular.ToString().Trim().RetirarMascaraTelefone(), out resultado))
                    {
                        if (celular.ToString().Trim().RetirarMascaraTelefone().Length == 10)
                        {
                            celularFormatado = string.Format("{0:(00)0000-0000}", resultado);
                        }
                        if (celular.ToString().Trim().RetirarMascaraTelefone().Length == 11)
                        {
                            celularFormatado = string.Format("{0:(00)00000-0000}", resultado);
                        }
                    }
                }
                e.Value = celularFormatado;
            }
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            RN.Pessoa rnPessoa = new Techne.Lyceum.RN.Pessoa();
            ValidacaoDados validacao = new ValidacaoDados();
            decimal pessoaCorreta = 0;
            List<decimal> pessoasParaRemover = new List<decimal>();
            List<string> matriculasParaCancelar = new List<string>();
            List<decimal> pessoasParaRemoverFinal = new List<decimal>();
            List<int> selecionados = new List<int>();
            List<int> naoSelecionados = new List<int>();
            RN.DTOs.DadosDuplicidadeAluno dados = new Techne.Lyceum.RN.DTOs.DadosDuplicidadeAluno();
            RN.Entidades.TceConfirmacaoMatricula confirmacao = new TceConfirmacaoMatricula();
            RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new Techne.Lyceum.RN.ConfirmacaoMatricula();
            string matriculaCorreta = string.Empty;
            try
            {
                for (int index = 0; index < grdDuplicidade.VisibleRowCount; index++)
                {
                    GridViewDataColumn col = (GridViewDataColumn)grdDuplicidade.Columns["Utilizar"];
                    Control control = grdDuplicidade.FindRowCellTemplateControl(index, col, "ckUtilizar");

                    if ((control is CheckBox))
                    {
                        CheckBox cb = ((CheckBox)control);
                        if (cb != null)
                        {
                            if (cb.Checked)
                            {
                                selecionados.Add(index);
                                pessoaCorreta = Convert.ToDecimal(grdDuplicidade.GetRowValues(index, "PESSOA"));
                                matriculaCorreta = Convert.ToString(grdDuplicidade.GetRowValues(index, "ALUNO"));
                            }
                            else
                            {
                                if (!pessoasParaRemover.Contains(Convert.ToDecimal(grdDuplicidade.GetRowValues(index, "PESSOA"))))
                                {
                                    pessoasParaRemover.Add(Convert.ToDecimal(grdDuplicidade.GetRowValues(index, "PESSOA")));
                                }
                                matriculasParaCancelar.Add(Convert.ToString(grdDuplicidade.GetRowValues(index, "ALUNO")));
                            }
                        }
                    }
                }

                if (selecionados.Count == 0)
                {
                    lblMensagem.Text = "Selecione no grid a matrícula que deve ser utilizada.";
                    return;
                }
                else if (selecionados.Count > 1)
                {
                    lblMensagem.Text = "Apenas uma matrícula deve estar selecionada no grid para ser utilizada.";
                    return;
                }

                if (selecionados.Count == 1)
                {
                    pessoaCorreta = Convert.ToDecimal(grdDuplicidade.GetRowValues(selecionados.FirstOrDefault(), "PESSOA"));
                    matriculaCorreta = Convert.ToString(grdDuplicidade.GetRowValues(selecionados.FirstOrDefault(), "ALUNO"));
                }

                if (pessoasParaRemover.Contains(pessoaCorreta))
                {
                    pessoasParaRemover.Remove(pessoaCorreta);
                }

                dados.PessoaCorreta = pessoaCorreta > 0 ? pessoaCorreta : -1;
                dados.MatriculaCorreta = !matriculaCorreta.IsNullOrEmptyOrWhiteSpace() ? matriculaCorreta : null;
                dados.UsuarioId = User.Identity.Name;
                dados.PessoasParaRemover = pessoasParaRemover.Count > 0 ? pessoasParaRemover : null;
                dados.MatriculasParaCancelar = matriculasParaCancelar.Count > 0 ? matriculasParaCancelar : null;
                dados.Nome = !txtNomeCompl.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeCompl.Text.Trim() : null;
                dados.NomeMae = !txtNomeMae.Text.IsNullOrEmptyOrWhiteSpace() ? txtNomeMae.Text.Trim() : null;
                dados.DataNascimento = !dtDataNasc.Text.IsNullOrEmptyOrWhiteSpace() ? Convert.ToDateTime(dtDataNasc.Text) : DateTime.MinValue;
                dados.SituacaoMatricula = !rblSituacao.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? rblSituacao.SelectedValue : null;

                if (dados.SituacaoMatricula == "Ativo")
                {
                    if (ddlConfirmacaoMatricula.Value != null)
                    {                   
                        confirmacao = rnConfirmacaoMatricula.ObtemConfirmacaoPor(Convert.ToInt32(ddlConfirmacaoMatricula.Value.ToString().Split('-')[0].Trim()));

                        dados.Ano = Convert.ToInt32(confirmacao.Ano);
                        dados.Periodo = Convert.ToInt32(confirmacao.Periodo);
                        dados.Censo = !confirmacao.Censo.IsNullOrEmptyOrWhiteSpace() ? confirmacao.Censo : null;
                        dados.Curso = !confirmacao.Curso.IsNullOrEmptyOrWhiteSpace() ? confirmacao.Curso : null;
                        dados.Turno = !confirmacao.Turno.IsNullOrEmptyOrWhiteSpace() ? confirmacao.Turno : null;
                        dados.Serie = confirmacao.Serie > 0 ? Convert.ToInt32(confirmacao.Serie) : -1;
                        dados.TipoVaga = !confirmacao.TipoVagaOcupada.IsNullOrEmptyOrWhiteSpace() ? confirmacao.TipoVagaOcupada : "VU";
                        dados.LinguaEstrangeiraFacultativa = confirmacao.LinguaEstrangeiraFacultativa;
                        dados.EnsinoReligioso = confirmacao.EnsinoReligioso;
                    }
                }

                validacao = rnPessoa.ValidaRemocaoDuplicidade(dados);

                if (validacao.Valido)
                {
                    rnPessoa.RemoveDuplicidade(dados);
                    this.lblMensagem.Text = "Operação realizada com sucesso.";
                    tseAluno.ResetValue();
                    tseAluno.DBValue = matriculaCorreta;
                    tseAluno.DataBind();

                    pnlGrid.Visible = false;
                    pnlInformacao.Visible = false;
                    pnlDadosAluno.Visible = false;
                    ddlConfirmacaoMatricula.Value = null;
                    txtNomeCompl.Text = dados.Nome;
                    txtNomeMae.Text = dados.NomeMae;
                    dtDataNasc.Text = dados.DataNascimento.ToShortDateString();
                    rblSituacao.ClearSelection();
                    hdnMatriculas.Value = string.Empty;
                    pnDuplicidade.Visible = true;

                }
                else
                {
                    this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void rblSituacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RN.PeriodoLetivo rnPeriodoLetivo = new Techne.Lyceum.RN.PeriodoLetivo();

                pnlDadosAluno.Visible = false;
                if (rblSituacao.SelectedValue == "Ativo")
                {
                    pnlDadosAluno.Visible = true;
                    CarregaConfirmacao();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaConfirmacao()
        {
            RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new Techne.Lyceum.RN.ConfirmacaoMatricula();
            DataTable dtConfirmacoes = new DataTable();
            ddlConfirmacaoMatricula.Items.Clear();

            if (!hdnMatriculas.Value.IsNullOrEmptyOrWhiteSpace())
            {
                dtConfirmacoes = rnConfirmacaoMatricula.ListaConfirmacaoMatriculaPor(hdnMatriculas.Value);
                ddlConfirmacaoMatricula.DataSource = dtConfirmacoes;
                ddlConfirmacaoMatricula.DataBind();
            }

        }


        private void CarregaGrid(string aluno, string nomeAluno, string nomeMae, DateTime dataNascimento)
        {
            RN.Aluno rnAluno = new Techne.Lyceum.RN.Aluno();
            RN.Pessoa rnPessoa = new Techne.Lyceum.RN.Pessoa();
            RN.DTOs.DadosAlunoPessoa dadosAluno = new RN.DTOs.DadosAlunoPessoa();
            DataTable dtAluno = new DataTable();
            List<string> matriculas = new List<string>();
            hdnMatriculas.Value = string.Empty;

            dadosAluno = rnAluno.ObtemDadosAlunoPessoaPor(Convert.ToString(aluno));
            dtAluno = rnPessoa.ObtemListaPor(nomeAluno.ToString(), nomeMae.ToString(), Convert.ToDateTime(dataNascimento.ToString()), dadosAluno.Nome, dadosAluno.NomeMae, Convert.ToDateTime(dadosAluno.DataNascimento));

            for (int index = 0; index < dtAluno.Rows.Count; index++)
            {
                var situacao = dtAluno.Rows[index]["SIT_ALUNO"].ToString();
                var matricula = dtAluno.Rows[index]["ALUNO"].ToString();

                if (situacao == "Ativo")
                {
                    matriculas.Add(matricula);
                }
            }

            grdDuplicidade.DataSource = dtAluno;
            grdDuplicidade.DataBind();
            if (matriculas.Count > 0)
            {
                hdnMatriculas.Value = matriculas.Aggregate((x, y) => x + "', '" + y);
            }
            else
            {
                rblSituacao.SelectedValue = "Cancelado";
                rblSituacao.Enabled = false;
                lblMensagemSituacao.Text = "Nenhuma das matrículas do aluno possui Confirmação de Matrícula Confirmada.";
            }
        }
    }
}
