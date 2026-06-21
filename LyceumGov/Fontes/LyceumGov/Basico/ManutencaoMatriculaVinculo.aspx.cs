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
    [NavUrl("~/Basico/ManutencaoMatriculaVinculo.aspx"),
    ControlText("Manutenção de Id/Vínculo de Vinculo"),
    Title("Manutenção de Id/Vínculo de Vinculo")]
    public partial class ManutencaoMatriculaVinculo : TPage
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
        }

        void Page_PreRenderComplete(object sender, EventArgs e)
        {
            ControlaAcesso(btnEditar, AcaoControle.editar);
            ControlaAcesso(btnSalvar, AcaoControle.editar);
            ControlaAcesso(grdHistoricoAtualizacao);
            ControlaAcesso(grdResumoLotacoes);
        }

        protected void tseVinculo_Changed(object sender, EventArgs args)
        {
            try
            {
                pnAbas.Visible = false;
                LimparTela();
                ImageButton[] controles = new ImageButton[] { };
                pcManutencaoVinculo.ActiveTabIndex = 0;
                if (!string.IsNullOrEmpty(tseVinculo.DBValue.ToString()))
                {
                    if (tseVinculo.IsValidDBValue)
                    {
                        pnAbas.Visible = true;
                        CarregaDadosVinculo(Convert.ToString(tseVinculo["matricula"]));
                        controles = new ImageButton[] { btnEditar };
                    }
                    else
                    {
                        lblMensagem.Text = "Servidor ou funcionário não cadastrado ou usuário sem permissão de visualização do Servidor ou funcionário(Lotação).";
                    }
                }
                else
                {
                    lblMensagem.Text = "Servidor ou funcionário não cadastrado ou usuário sem permissão de visualização do Servidor ou funcionário(Lotação).";
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
                grdResumoLotacoes.DataSource = rnLotacao.ObtemListaLotacaoVinculoPor(Convert.ToInt32(tseVinculo["pessoa"]));
                grdResumoLotacoes.DataBind();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void CarregaGridHistorico()
        {
            LogAtualizacaoMatriculaServidor rnLogAtualizacaoMatriculaServidor = new LogAtualizacaoMatriculaServidor();
            try
            {
                grdHistoricoAtualizacao.DataSource = rnLogAtualizacaoMatriculaServidor.ObtemListaPor(Convert.ToInt32(tseVinculo["pessoa"]));
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

        public void grdHistoricoAtualizacao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            CarregaGridHistorico();
        }

        protected void CarregaDadosVinculo(string matricula)
        {
            RN.VinculoLy rnVinculo = new Techne.Lyceum.RN.VinculoLy();
            DadosTrocaMatriculaVinculo dadosVinculo = new DadosTrocaMatriculaVinculo();

            try
            {
                dadosVinculo = rnVinculo.ObtemDadosTrocaMatriculaVinculoPor(matricula);

                if ((dadosVinculo != null) && (dadosVinculo.Pessoa > 0))
                {
                    txtIdFuncionalAtual.Text = Convert.ToString(dadosVinculo.IdFuncional);
                    txtVinculoAtual.Text = Convert.ToString(dadosVinculo.Vinculo);

                    if(!dadosVinculo.Matricula.Contains("/"))
                    {
                        txtMatriculaAtual.Text = Convert.ToString(dadosVinculo.Matricula);
                    }

                    txtNome.Text = dadosVinculo.NomeCompl;
                    dtDtNasc.Date = dadosVinculo.DtNasc;
                    txtCPF.Text = dadosVinculo.Cpf;
                    rblSexo.SelectedValue = dadosVinculo.Sexo;

                    CarregaGridResumoLotacao();
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
                LogAtualizacaoMatriculaServidor rnLogAtualizacaoMatriculaServidor = new LogAtualizacaoMatriculaServidor();
                ValidacaoDados validacao = new ValidacaoDados();
                RN.VinculoLy rnVinculo = new Techne.Lyceum.RN.VinculoLy();

                RN.Entidades.LogAtualizacaoMatriculaServidor logAtualizacaoMatriculaServidor = new Techne.Lyceum.RN.Entidades.LogAtualizacaoMatriculaServidor
                {
                    Pessoa = Convert.ToInt32(tseVinculo["pessoa"].ToString()),
                    Ordem = Convert.ToInt32(tseVinculo["ordem"]),
                    MatriculaAnterior = txtMatriculaAtual.Text,
                    MatriculaNova = txtMatriculaNova.Text,
                    UsuarioId = User.Identity.Name,
                    IdFuncionalAnterior = txtIdFuncionalAtual.Text.IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(txtIdFuncionalAtual.Text),
                    IdFuncionalNovo = txtIdFuncionalNovo.Text.IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(txtIdFuncionalNovo.Text),
                    VinculoAnterior = txtVinculoAtual.Text.IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(txtVinculoAtual.Text),
                    VinculoNovo = txtVinculoNovo.Text.IsNullOrEmptyOrWhiteSpace() ? (int?)null : Convert.ToInt32(txtVinculoNovo.Text)
                };

                int pessoa = Convert.ToInt32(tseVinculo["pessoa"]);
                validacao = rnVinculo.ValidaTrocaMatricula(logAtualizacaoMatriculaServidor, pessoa);

                if (validacao.Valido)
                {
                    rnVinculo.TrocaMatricula(logAtualizacaoMatriculaServidor, pessoa);

                    string idVinculoMatricula;
                    if (txtIdFuncionalNovo.Text.IsNullOrEmptyOrWhiteSpace() || txtVinculoNovo.Text.IsNullOrEmptyOrWhiteSpace())
                    {
                        idVinculoMatricula = logAtualizacaoMatriculaServidor.MatriculaNova;
                    }
                    else
                    {
                        idVinculoMatricula = string.Format("{0}/{1}", logAtualizacaoMatriculaServidor.IdFuncionalNovo, logAtualizacaoMatriculaServidor.VinculoNovo);
                    }

                    LimparTela();
                    CarregaDadosVinculo(logAtualizacaoMatriculaServidor.MatriculaNova);
                    tseVinculo.DBValue = idVinculoMatricula;
                    ImageButton[] controles = new ImageButton[] { btnEditar };
                    ControlarVisibilidadeControle(controles);
                    lblMensagem.Text = "Vinculo Novo Alterado Com Sucesso.";
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
    }
}
