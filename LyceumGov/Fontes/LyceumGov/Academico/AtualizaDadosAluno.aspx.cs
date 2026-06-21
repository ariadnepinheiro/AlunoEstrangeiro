using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Seeduc.Infra.Extensions;
using Seeduc.Infra.Helpers;
using Seeduc.Infra.Validation;
using Techne.Controls;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Servicos;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.CartaoEstudante.Service;
using Techne.Web;
using Image = System.Drawing.Image;
using Seeduc.Infra.Data;
using DevExpress.Web.ASPxEditors;
using System.Text.RegularExpressions;
using DevExpress.Web.ASPxClasses;
using System.Linq;
using Techne.Lyceum.RN.DTOs.Agenda;
using System.Net;
using System.Web.Services;
using System.Web.Script.Services;
using System.Configuration;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/AtualizaDadosAluno.aspx"),
    ControlText("Atualização CPF Alunos"),
    Title("Atualização CPF Alunos")]
    public partial class AtualizaDadosAluno : TPage
    {
        public enum TipoOperacao
        {
            Inicial,
            Alterar,
            Sucesso,
            Consultar
        }

        private TipoOperacao _tipoOperacao
        {
            get
            {
                if (ViewState["_tipoOperacao"] != null)
                {
                    if (ViewState["_tipoOperacao"] is TipoOperacao)
                    {
                        return (TipoOperacao)ViewState["_tipoOperacao"];
                    }
                }

                return TipoOperacao.Inicial;
            }

            set
            {
                ViewState["_tipoOperacao"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                if (!IsPostBack)
                {
                    this._tipoOperacao = TipoOperacao.Inicial;
                    this.ControlarTipoOperacao();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnEditar, AcaoControle.editar);
        }

        protected void DesabilitaCampos()
        {
            txtCPF.Enabled = false;
        }

        protected void HabilitaCampos()
        {
            txtCPF.Enabled = true;
        }

        private void LimparTela()
        {
            lblNomeAluno.Text = string.Empty;
            lblNomeMae.Text = string.Empty;
            lblDataNascimento.Text = string.Empty;
            txtCPF.Text = string.Empty;
        }

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            try
            {
                if (Page.IsCallback)
                {
                    return;
                }

                if (!tseAluno.DBValue.IsNull)
                {
                    if (tseAluno.IsValidDBValue)
                    {
                        this._tipoOperacao = TipoOperacao.Consultar;

                        lblMensagem.Text = string.Empty;


                    }
                    else
                    {
                        this._tipoOperacao = TipoOperacao.Inicial;

                        lblMensagem.Text = "Aluno ativo ou não cadastrado (favor verificar).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Aluno ativo ou não cadastrado (favor verificar).";
                    _tipoOperacao = TipoOperacao.Inicial;
                }

                ControlarTipoOperacao();

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _tipoOperacao = TipoOperacao.Inicial;

                ControlarTipoOperacao();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnEditar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if ((!tseAluno.DBValue.IsNull) && (tseAluno.IsValidDBValue))
                {
                    _tipoOperacao = TipoOperacao.Alterar;
                    ControlarTipoOperacao();
                }
                else
                {
                    this._tipoOperacao = TipoOperacao.Inicial;
                    lblMensagem.Text = "Aluno ativo ou não cadastrado (favor verificar).";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                RN.Aluno rnAluno = new Aluno();
                ValidacaoDados validacao = new ValidacaoDados();

                decimal pessoa = (!tseAluno.DBValue.IsNull && tseAluno.IsValidDBValue) ? Convert.ToDecimal(tseAluno["pessoa"]) : 0;
                string aluno = (!tseAluno.DBValue.IsNull && tseAluno.IsValidDBValue) ? Convert.ToString(tseAluno.DBValue) : string.Empty;
                string cpf = !txtCPF.Text.IsNullOrEmptyOrWhiteSpace() ? txtCPF.Text.Trim() : null;
                string usuarioResponsavel = User.Identity.Name;

                validacao = rnAluno.ValidaAtualizacaoCpf(aluno, cpf, usuarioResponsavel, pessoa);

                if (validacao.Valido)
                {
                    rnAluno.AtualizaCpf(cpf, usuarioResponsavel, pessoa);
                    lblMensagem.Text = "Dados de Aluno atualizado com sucesso.";

                    this._tipoOperacao = TipoOperacao.Sucesso;
                    this.ControlarTipoOperacao();
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (var botao in botoes)
            {
                botao.Visible = true;
            }

            ControlaAcesso(btnEditar, AcaoControle.editar);

        }

        private void RetiraVisibilidadeBotao()
        {
            btnCancel.Visible = false;
            btnEditar.Visible = false;
            btnSalvar.Visible = false;
        }

        private void ControlarTipoOperacao()
        {
            switch (this._tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        ImageButton[] controles = new ImageButton[] { };
                        ControlarVisibilidadeControle(controles);

                        tseAluno.ResetValue();
                        LimparTela();
                        pnlDados.Visible = false;
                        break;
                    }
                case TipoOperacao.Alterar:
                    {
                        if ((!tseAluno.DBValue.IsNull) && (tseAluno.IsValidDBValue))
                        {
                            ImageButton[] controles = new ImageButton[] { btnCancel, btnSalvar };
                            ControlarVisibilidadeControle(controles);
                            lblMensagem.Text = String.Empty;
                            HabilitaCampos();
                            break;
                        }
                        else
                        {
                            this._tipoOperacao = TipoOperacao.Inicial;
                            lblMensagem.Text = "Aluno ativo ou não cadastrado (favor verificar).";
                            break;
                        }
                    }
                case TipoOperacao.Consultar:
                    {
                        if ((!tseAluno.DBValue.IsNull) && (tseAluno.IsValidDBValue))
                        {
                            ImageButton[] controles = new ImageButton[] { btnEditar };
                            ControlarVisibilidadeControle(controles);
                            lblMensagem.Text = String.Empty;
                            this.CarregaDadosAluno(Convert.ToString(tseAluno.DBValue));
                            tseAluno.Enabled = true;
                            DesabilitaCampos();
                        }
                        else
                        {
                            this._tipoOperacao = TipoOperacao.Inicial;
                            lblMensagem.Text = "Aluno ativo ou não cadastrado (favor verificar).";
                            break;
                        }
                        break;
                    }
                case TipoOperacao.Sucesso:
                    {
                        ImageButton[] controles = new ImageButton[] { btnEditar };
                        ControlarVisibilidadeControle(controles);
                        DesabilitaCampos();
                        break;
                    }
            }
        }

        private void CarregaDadosAluno(string aluno)
        {
            RN.Aluno rnAluno = new Techne.Lyceum.RN.Aluno();
            RN.DTOs.DadosAlunoPessoa dadosAluno = new RN.DTOs.DadosAlunoPessoa();
            LimparTela();

            dadosAluno = rnAluno.ObtemDadosAlunoPessoaPor(Convert.ToString(aluno));

            if (dadosAluno.Aluno.IsNullOrEmptyOrWhiteSpace())
            {
                lblMensagem.Text = "Aluno não encontrado.";
                ImageButton[] controles = new ImageButton[] { };
                ControlarVisibilidadeControle(controles);
            }
            else
            {
                lblNomeAluno.Text = dadosAluno.Nome;
                lblNomeMae.Text = dadosAluno.NomeMae;
                lblDataNascimento.Text = dadosAluno.DataNascimento != null ? Convert.ToDateTime(dadosAluno.DataNascimento).ToString("dd/MM/yyyy") : string.Empty;
                lblSituacao.Text = dadosAluno.SitAluno;
                txtCPF.Text = dadosAluno.Cpf;
                pnlDados.Visible = true;
            }
        }
    }
}
