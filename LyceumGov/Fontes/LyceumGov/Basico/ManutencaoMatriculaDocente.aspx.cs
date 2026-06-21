using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.CR;
using DevExpress.Web.ASPxGridView;
using Techne.Web;
using Techne.Lyceum.RN.Util;
using System.Collections.Generic;

namespace Techne.Lyceum.Net.Basico
{
    [NavUrl("~/Basico/ManutencaoMatriculaDocente.aspx"),
    ControlText("Manutenção de Id/Vínculo de Docente"),
    Title("Manutenção de Id/Vínculo de Docente")]
    public partial class ManutencaoMatriculaDocente : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                if (!IsPostBack)
                {
                    ImageButton[] controles = new ImageButton[] { };
                    ControlarVisibilidadeControle(controles);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdHistoricoAtualizacao, "Histórico");
            TituloGrid(grdResumoLotacoes, "Resumo de Lotações");
            TituloGrid(grdSituacao, "Situações");
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnSalvar, AcaoControle.editar);
            ControlaAcesso(grdHistoricoAtualizacao);
            ControlaAcesso(grdResumoLotacoes);
            ControlaAcesso(grdSituacao);
        }

        protected void tseDocente_Changed(object sender, EventArgs args)
        {
            try
            {
                pnAbas.Visible = false;
                LimparTela();
                ImageButton[] controles = new ImageButton[] { };
                pcManutencaoDocentes.ActiveTabIndex = 0;
                if (!string.IsNullOrEmpty(tseDocente.DBValue.ToString()))
                {
                    if (tseDocente.IsValidDBValue)
                    {
                        pnAbas.Visible = true;
                        CarregaDadosDocente(Convert.ToString(tseDocente["matricula"]));
                        controles = new ImageButton[] { btnEditar };
                    }
                    else
                    {
                        lblMensagem.Text = "Docente não cadastrado ou usuário sem permissão de visualização do Docente(Lotação).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Docente não cadastrado ou usuário sem permissão de visualização do Docente(Lotação).";
                }
                ControlarVisibilidadeControle(controles);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void LimparTela()
        {
            txtIdFuncionalAtual.Text = string.Empty;
            txtVinculoAtual.Text = string.Empty;
            txtMatriculaAtual.Text = string.Empty;
            txtIdFuncionalNovo.Text = string.Empty;
            txtVinculoNovo.Text = string.Empty;
            txtMatriculaNova.Text = string.Empty;
            txtNome.Text = string.Empty;
            txtCPF.Text = string.Empty;
            rblSexo.ClearSelection();
            dtDtNasc.Text = string.Empty;
            txtIdFuncionalNovo.Visible = false;
            txtVinculoNovo.Visible = false;
            txtMatriculaNova.Visible = false;
            lblIdFuncionalNovo.Visible = false;
            lblVinculoNovo.Visible = false;
            lblMatriculaNova.Visible = false;
            lblAviso.Visible = false;
            grdResumoLotacoes.DataSource = null;
            grdResumoLotacoes.DataBind();
        }

        protected void CarregaGridResumoLotacao()
        {
            Lotacao rnLotacao = new Lotacao();
            try
            {
                grdResumoLotacoes.DataSource = rnLotacao.ObtemListaLotacaoDocentePor(Convert.ToInt32(tseDocente["pessoa"]));
                grdResumoLotacoes.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void CarregaGridSituacao()
        {
            RN.LicencaDocente rnLicencaDocente = new Techne.Lyceum.RN.LicencaDocente();

            try
            {
                grdSituacao.DataSource = rnLicencaDocente.ObtemListaLicencaDocentePor(Convert.ToInt32(tseDocente["pessoa"]));
                grdSituacao.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void CarregaGridHistorico()
        {
            LogAtualizacaoMatricula rnLogAtualizacaoMatricula = new LogAtualizacaoMatricula();
            try
            {
                grdHistoricoAtualizacao.DataSource = rnLogAtualizacaoMatricula.ObtemListaPor(Convert.ToInt32(tseDocente["num_func"]));
                grdHistoricoAtualizacao.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public void grdResumoLotacoes_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            CarregaGridResumoLotacao();
        }

        public void grdSituacao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            CarregaGridSituacao();
        }

        public void grdHistoricoAtualizacao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            CarregaGridHistorico();
        }

        protected void CarregaDadosDocente(string matricula)
        {
            RN.Docentes rnDocente = new Techne.Lyceum.RN.Docentes();
            DadosTrocaMatriculaDocente dadosDocente = new DadosTrocaMatriculaDocente();

            try
            {
                dadosDocente = rnDocente.ObtemDadosTrocaMatriculaDocentePor(matricula);

                if ((dadosDocente != null) && (dadosDocente.Pessoa > 0))
                {
                    txtIdFuncionalAtual.Text = Convert.ToString(dadosDocente.IdFuncional);
                    txtVinculoAtual.Text = Convert.ToString(dadosDocente.Vinculo);

                    txtMatriculaAtual.Text = dadosDocente.Matricula;


                    txtNome.Text = dadosDocente.NomeCompl;
                    dtDtNasc.Date = dadosDocente.DtNasc;
                    txtCPF.Text = dadosDocente.Cpf;
                    rblSexo.SelectedValue = dadosDocente.Sexo;

                    CarregaGridResumoLotacao();
                    CarregaGridSituacao();
                    CarregaGridHistorico();

                    //Inicia dados novos com atuais
                    txtIdFuncionalNovo.Text = txtIdFuncionalAtual.Text;
                    txtVinculoNovo.Text = txtVinculoAtual.Text;
                    txtMatriculaNova.Text = txtMatriculaAtual.Text;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                lblIdFuncionalNovo.Visible = true;
                lblVinculoNovo.Visible = true;
                lblMatriculaNova.Visible = true;
                lblAviso.Visible = true;
                txtMatriculaNova.Visible = true;
                txtIdFuncionalNovo.Visible = true;
                txtVinculoNovo.Visible = true;
                txtIdFuncionalNovo.Text = txtIdFuncionalAtual.Text;
                txtVinculoNovo.Text = txtVinculoAtual.Text;
                txtMatriculaNova.Text = txtMatriculaAtual.Text;

                var controles = new[] { btnCancel, btnSalvar };
                ControlarVisibilidadeControle(controles);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        public void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ImageButton[] controles = new ImageButton[] { btnEditar };
                ControlarVisibilidadeControle(controles);
                txtIdFuncionalNovo.Visible = false;
                txtVinculoNovo.Visible = false;
                txtMatriculaNova.Visible = false;
                lblIdFuncionalNovo.Visible = false;
                lblVinculoNovo.Visible = false;
                lblMatriculaNova.Visible = false;
                lblAviso.Visible = false;
                txtIdFuncionalNovo.Text = string.Empty;
                txtVinculoNovo.Text = string.Empty;
                txtMatriculaNova.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (ImageButton botao in botoes)
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

        public void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                LogAtualizacaoMatricula rnLogAtualizacaoMatricula = new LogAtualizacaoMatricula();
                ValidacaoDados validacao = new ValidacaoDados();
                RN.Docentes rnDocentes = new Techne.Lyceum.RN.Docentes();

                RN.Entidades.LogAtualizacaoMatricula logAtualizacaoMatricula = new Techne.Lyceum.RN.Entidades.LogAtualizacaoMatricula
                {
                    IdFuncionalAnterior = txtIdFuncionalAtual.Text.IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(txtIdFuncionalAtual.Text),
                    IdFuncionalNovo = txtIdFuncionalNovo.Text.IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(txtIdFuncionalNovo.Text),
                    VinculoAnterior = txtVinculoAtual.Text.IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(txtVinculoAtual.Text),
                    VinculoNovo = txtVinculoNovo.Text.IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(txtVinculoNovo.Text),
                    MatriculaAnterior = txtMatriculaAtual.Text,
                    MatriculaNova = txtMatriculaNova.Text,
                    UsuarioId = User.Identity.Name,
                    DocenteId = Convert.ToInt32(tseDocente["num_func"].ToString())
                };

                int pessoa = Convert.ToInt32(tseDocente["pessoa"]);
                validacao = rnDocentes.ValidaTrocaMatricula(logAtualizacaoMatricula, pessoa);

                if (validacao.Valido)
                {
                    rnDocentes.TrocaMatricula(logAtualizacaoMatricula, pessoa);

                    string idVinculoMatricula;
                    if (txtIdFuncionalNovo.Text.IsNullOrEmptyOrWhiteSpace() || txtVinculoNovo.Text.IsNullOrEmptyOrWhiteSpace())
                    {
                        idVinculoMatricula = logAtualizacaoMatricula.MatriculaNova;
                    }
                    else
                    {
                        idVinculoMatricula = string.Format("{0}/{1}", logAtualizacaoMatricula.IdFuncionalNovo, logAtualizacaoMatricula.VinculoNovo);
                    }

                    LimparTela();
                    CarregaDadosDocente(logAtualizacaoMatricula.MatriculaNova);
                    tseDocente.DBValue = idVinculoMatricula;
                    ImageButton[] controles = new ImageButton[] { btnEditar };
                    ControlarVisibilidadeControle(controles);
                    lblMensagem.Text = "Registro gravado com sucesso.";
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
            catch (FormatException)
            {
                lblMensagem.Text = "Dados estão no formato incorreto. Digite apenas números";
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }
    }
}
