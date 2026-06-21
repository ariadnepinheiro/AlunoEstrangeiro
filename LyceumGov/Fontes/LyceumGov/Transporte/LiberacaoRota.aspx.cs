using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Lyceum.RN.Util;
using Techne.Web;

namespace Techne.Lyceum.Net.Transporte
{
    [NavUrl("~/Transporte/LiberacaoRota.aspx")]
    [ControlText("Liberação Rota de Transporte")]
    [Title("Liberação Rota de Transporte")]
    public partial class LiberacaoRota : TPage
    {
        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            TituloGrid(grdRota, "Rotas Ativas");
        }

        protected void tseRegional_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }

                pnlRotas.Visible = false;

                var sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (sessao != null)
                {
                    if (!this.tseRegional.DBValue.IsNull)
                    {
                        if (this.tseRegional.IsValidDBValue)
                        {
                            sessao.Regional = Convert.ToString(tseRegional.DBValue);
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;

                            tseMunicipio.ResetValue();
                            tseUnidadeResponsavel.ResetValue();
                        }
                        else
                        {
                            sessao.Regional = string.Empty;
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;
                        }
                    }
                    else
                    {
                        sessao.Regional = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                        tseMunicipio.ResetValue();
                        tseUnidadeResponsavel.ResetValue();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
            }
        }

        protected void tseMunicipio_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }
                pnlRotas.Visible = false;
                var sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (sessao != null)
                {
                    if (!this.tseMunicipio.DBValue.IsNull)
                    {
                        if (this.tseMunicipio.IsValidDBValue)
                        {
                            sessao.Municipio = Convert.ToString(tseMunicipio.DBValue);
                            sessao.Escola = string.Empty;
                        }
                        else
                        {
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;
                            tseUnidadeResponsavel.ResetValue();
                        }
                    }
                    else
                    {
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                        tseUnidadeResponsavel.ResetValue();
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeResponsavel_Changed(object sender, Techne.Controls.ChangedEventArgs args)
        {
            try
            {
                if (this.Page.IsCallback)
                {
                    return;
                }
                pnlRotas.Visible = false;
                var sessao = RN.SessaoUsuario.GetSessaoUsuario();

                if (!this.tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (this.tseUnidadeResponsavel.IsValidDBValue)
                    {
                        pnlRotas.Visible = true;
                        grdRota.DataBind();
                        if (!this.tseUnidadeResponsavel["unidade_ens"].IsNull)
                        {
                            this.tseRegional.Value = this.tseUnidadeResponsavel["id_regional"];
                            this.tseMunicipio.Value = this.tseUnidadeResponsavel["municipio"];
                        }

                        if (sessao != null)
                        {
                            sessao.Escola = Convert.ToString(this.tseUnidadeResponsavel.DBValue);
                            sessao.Regional = Convert.ToString(this.tseRegional.DBValue);
                            sessao.Municipio = Convert.ToString(this.tseMunicipio.DBValue);
                        }

                    }
                    else
                    {
                        lblMensagem.Text = "Unidade de ensino não encontrada.";
                        if (sessao != null)
                        {
                            sessao.Escola = string.Empty;
                            sessao.Municipio = string.Empty;
                            sessao.Regional = string.Empty;
                        }
                    }
                }
                else
                {
                    if (sessao != null)
                    {
                        sessao.Escola = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Regional = string.Empty;
                    }
                    lblMensagem.Text = "Favor consultar uma unidade de ensino.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagem.Visible = true;
            }
        }

        public object ListaRota(object unidade_ens)
        {
            RN.Transporte.Rota rnRota = new Techne.Lyceum.RN.Transporte.Rota();

            if (!string.IsNullOrEmpty(unidade_ens.ToString()))
            {
                return rnRota.ListaAtivaPor(unidade_ens.ToString());
            }

            return null;
        }

        protected void grdRota_CustomButtonInitialize(object sender, ASPxGridViewCustomButtonEventArgs e)
        {
            if (e.VisibleIndex == -1) return;

            if (e.CellType == GridViewTableCommandCellType.Filter)
            {
                e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                return;
            }

            string situacao = Convert.ToString(grdRota.GetRowValues(e.VisibleIndex, "SITUACAO"));
            string dataAluno = Convert.ToString(grdRota.GetRowValues(e.VisibleIndex, "DATALIMITEEDICAOALUNO"));

            if (e.ButtonID == "btnAprovar")
            {
                //Verifica se ainda não foi aprovado
                if (situacao != "AGUARDANDO APROVAÇÃO")
                {
                    e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                }
            }
            if (e.ButtonID == "btnLiberarAluno")
            {
                //Verifica se já foi aprovado e se ja passou o prazo
                if (situacao != "APROVADO" || dataAluno.IsNullOrEmptyOrWhiteSpace() || Convert.ToDateTime(dataAluno).Date >= DateTime.Now.Date)
                {
                    e.Visible = DevExpress.Web.ASPxClasses.DefaultBoolean.False;
                }
            }
        }

        protected void grdRota_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdRota);
            ControlaAcesso(grdRota, AcaoControle.editar, "btnAprovar");
            ControlaAcesso(grdRota, AcaoControle.editar, "btnLiberarAluno");
        }

        protected void LimpaCamposRota()
        {
            lblCodigo.Text = string.Empty;
            lblEscola.Text = string.Empty;
            lblRegionalRota.Text = string.Empty;
            lblMunicipioRota.Text = string.Empty;
            lblRegiaoFinanceira.Text = string.Empty;
            lblCnpj.Text = string.Empty;
            lblTurno.Text = string.Empty;
            lblTipoCalculoPagamento.Text = string.Empty;
            lblTipoContratacaoIda.Text = string.Empty;
            lblValorRotaIda.Text = string.Empty;
            lblQuantidadeKmIda.Text = string.Empty;
            lblQuantidadeAlunoIda.Text = string.Empty;
            lblPrestadorIda.Text = string.Empty;
            lblCondutorIda.Text = string.Empty;
            lblVeiculoIda.Text = string.Empty;
            lblTipoContratacaoVolta.Text = string.Empty;
            lblValorRotaVolta.Text = string.Empty;
            lblQuantidadeKmVolta.Text = string.Empty;
            lblQuantidadeAlunoVolta.Text = string.Empty;
            lblPrestadorVolta.Text = string.Empty;
            lblCondutorVolta.Text = string.Empty;
            lblVeiculoVolta.Text = string.Empty;
        }

        protected void grdRota_CustomButtonCallback(object sender, ASPxGridViewCustomButtonCallbackEventArgs e)
        {
            RN.DTOs.DadosRota dadosRota = new Techne.Lyceum.RN.DTOs.DadosRota();
            RN.Transporte.Rota rnRota = new Techne.Lyceum.RN.Transporte.Rota();
            RN.Transporte.RotaAluno rnRotaAluno = new Techne.Lyceum.RN.Transporte.RotaAluno();
            RN.Turno rnTurno = new Techne.Lyceum.RN.Turno();
            RN.Transporte.Veiculo rnVeiculo = new Techne.Lyceum.RN.Transporte.Veiculo();
            RN.Transporte.Prestador rnPrestador = new Techne.Lyceum.RN.Transporte.Prestador();
            RN.Transporte.Condutor rnCondutor = new Techne.Lyceum.RN.Transporte.Condutor();
            RN.Transporte.Entidades.Prestador prestadorIda = new Techne.Lyceum.RN.Transporte.Entidades.Prestador();
            RN.Transporte.Entidades.Prestador prestadorVolta = new Techne.Lyceum.RN.Transporte.Entidades.Prestador();
            RN.Transporte.Entidades.Veiculo veiculoIda = new Techne.Lyceum.RN.Transporte.Entidades.Veiculo();
            RN.Transporte.Entidades.Veiculo veiculoVolta = new Techne.Lyceum.RN.Transporte.Entidades.Veiculo();
            RN.Transporte.Entidades.Condutor condutorIda = new Techne.Lyceum.RN.Transporte.Entidades.Condutor();
            RN.Transporte.Entidades.Condutor condutorVolta = new Techne.Lyceum.RN.Transporte.Entidades.Condutor();
            ValidacaoDados validacao = new ValidacaoDados();

            //Busca dados selecionado
            int rotaId = Convert.ToInt32(grdRota.GetRowValues(e.VisibleIndex, "ROTAID"));

            try
            {
                if (e.ButtonID == "btnDados")
                {
                    LimpaCamposRota();

                    //Busca dados da rota
                    dadosRota = rnRota.ObtemDadosRotaPor(rotaId);
                    lblCodigo.Text = dadosRota.Codigo;
                    lblEscola.Text = RN.UnidadeEnsino.RetornaNomeUnidadeEnsino(dadosRota.Censo);
                    lblRegionalRota.Text = dadosRota.RegionalDescricao;
                    lblMunicipioRota.Text = dadosRota.MunicipioDescricao;
                    lblRegiaoFinanceira.Text = dadosRota.RegiaoFinanceiraDescricao;
                    lblCnpj.Text = dadosRota.Cnpj;
                    lblTurno.Text = rnTurno.RetornaDescricaoTurno(dadosRota.Turno);
                    lblTipoCalculoPagamento.Text = dadosRota.TipoCalculoPagamento;
                    lblTipoContratacaoIda.Text = dadosRota.TipoContratacaoDescricaoIda;
                    lblValorRotaIda.Text = Convert.ToString(dadosRota.ValorRotaIda);
                    lblQuantidadeKmIda.Text = Convert.ToString(dadosRota.QuantidadeKmIda);
                    lblTipoContratacaoVolta.Text = dadosRota.TipoContratacaoDescricaoVolta.ToString();
                    lblValorRotaVolta.Text = Convert.ToString(dadosRota.ValorRotaVolta);
                    lblQuantidadeKmVolta.Text = Convert.ToString(dadosRota.QuantidadeKmVolta);

                    //Busca quantidades de alunos
                    lblQuantidadeAlunoIda.Text = Convert.ToString(rnRotaAluno.ObtemAlunosAtivosPor(dadosRota.RotaTrajetoIdIda, DateTime.Now));
                    lblQuantidadeAlunoVolta.Text = Convert.ToString(rnRotaAluno.ObtemAlunosAtivosPor(dadosRota.RotaTrajetoIdVolta, DateTime.Now));

                    //Busca dados do prestador
                    prestadorIda = rnPrestador.ObtemPor(dadosRota.PrestadorIdIda);
                    prestadorVolta = rnPrestador.ObtemPor(dadosRota.PrestadorIdVolta);
                    lblPrestadorIda.Text = string.Format("{0} - {1}", prestadorIda.Cnpj.IsNullOrEmptyOrWhiteSpace() ? prestadorIda.Cpf : prestadorIda.Cnpj, prestadorIda.Nome);
                    lblPrestadorVolta.Text = string.Format("{0} - {1}", prestadorVolta.Cnpj.IsNullOrEmptyOrWhiteSpace() ? prestadorVolta.Cpf : prestadorVolta.Cnpj, prestadorVolta.Nome);

                    //Busca dados do condutor
                    condutorIda = rnCondutor.ObtemPor(dadosRota.CondutorIdIda);
                    condutorVolta = rnCondutor.ObtemPor(dadosRota.CondutorIdVolta);
                    lblCondutorIda.Text = string.Format("{0} - {1}", condutorIda.Cpf, condutorIda.Nome);
                    lblCondutorVolta.Text = string.Format("{0} - {1}", condutorVolta.Cpf, condutorVolta.Nome);

                    //Busca dados do veiculo
                    veiculoIda = rnVeiculo.ObtemPor(dadosRota.VeiculoIdIda);
                    veiculoVolta = rnVeiculo.ObtemPor(dadosRota.VeiculoIdVolta);
                    if (!veiculoIda.Placa.IsNullOrEmptyOrWhiteSpace() && !veiculoIda.Nome.IsNullOrEmptyOrWhiteSpace())
                    {
                        lblVeiculoIda.Text = string.Format("{0} - {1}", veiculoIda.Placa, veiculoIda.Nome);

                        if (veiculoIda.QuantidadeAssentos != 0)
                        {
                            lblVeiculoIda.Text = lblVeiculoIda.Text + " com " + Convert.ToString(veiculoIda.QuantidadeAssentos) + " assentos";
                        }
                    }
                    else
                    {
                        lblVeiculoIda.Text = "-";
                    }


                    if (!veiculoVolta.Placa.IsNullOrEmptyOrWhiteSpace() && !veiculoVolta.Nome.IsNullOrEmptyOrWhiteSpace())
                    {
                        lblVeiculoVolta.Text = string.Format("{0} - {1}", veiculoVolta.Placa, veiculoVolta.Nome);

                        if (veiculoVolta.QuantidadeAssentos != 0)
                        {
                            lblVeiculoVolta.Text = lblVeiculoVolta.Text + " com " + Convert.ToString(veiculoVolta.QuantidadeAssentos) + " assentos";
                        }
                    }
                    else
                    {
                        lblVeiculoVolta.Text = "-";
                    }


                    pucRota.ShowOnPageLoad = true;
                }
                else if (e.ButtonID == "btnAprovar")
                {
                    validacao = rnRota.ValidaAprovacao(rotaId, User.Identity.Name);

                    if (validacao.Valido)
                    {
                        rnRota.Aprova(rotaId, User.Identity.Name);
                        odsRota.Select();
                        odsRota.DataBind();
                        grdRota.DataBind();
                        lblMensagem.Text = "Rota Aprovada com sucesso.";
                    }
                    else
                    {
                        throw new Exception(validacao.Mensagem);
                    }
                }
                else if (e.ButtonID == "btnLiberarAluno")
                {
                    validacao = rnRota.ValidaReaberturaEdicao(rotaId, User.Identity.Name);

                    if (validacao.Valido)
                    {
                        rnRota.ReabreEdicaoAluno(rotaId, User.Identity.Name);
                        odsRota.Select();
                        odsRota.DataBind();
                        grdRota.DataBind();
                        lblMensagem.Text = "Associação de alunos liberada com sucesso.";
                    }
                    else
                    {
                        throw new Exception(validacao.Mensagem.Replace(Environment.NewLine, "<br />"));
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                pucRota.ShowOnPageLoad = false;
            }
        }
    }
}
