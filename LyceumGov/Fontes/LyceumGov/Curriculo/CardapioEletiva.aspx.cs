using System;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.Data;
using Techne.Controls;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.Entidades;
using Techne.Web;
using System.Web.UI.WebControls;
using Techne.Lyceum.RN.Util;
using System.Web.UI;
using System.Collections.Generic;

namespace Techne.Lyceum.Net.Curriculo
{
    [NavUrl("~/Curriculo/CardapioEletiva.aspx"), ControlText("CardapioEletiva"), Title("Cardápio de Eletiva")]

    public partial class CardapioEletiva : TPage
    {
        public enum TipoOperacao
        {
            ConsultaSerie,
            Inicial,
            Salvar,
            Finalizar,
            Validar
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
                RN.Perfil rnPerfil = new Perfil();
                RN.PeriodoLetivo rnPeriodoLetivo = new PeriodoLetivo();
                this.lblMensagem.Text = string.Empty;               

                if (!this.IsPostBack)
                {
                    lblMensagemFinalizacao.Text = string.Empty;
                    lblMensagemValidacao.Text = string.Empty;
                    LimparCampos();
                    LimparEletivas();
                    ImageButton[] controles = new ImageButton[] { };
                    ControlarVisibilidadeControle(controles);

                    if (!rnPeriodoLetivo.EhPeriodoIndicacaoEletiva())
                    {
                        this.lblMensagem.Text = "O período para lançamento do Cardápio de Eletiva ainda não iniciou. Aguarde!";
                        pnGeral.Visible = false;
                        divEdit.Visible = false;
                        return;
                    }
                    else
                    {
                        pnGeral.Visible = true;
                        divEdit.Visible = true;
                    }

                    hdnValida.Value = rnPerfil.PossuiPerfilValidaCardapioPor(User.Identity.Name) ? "S" : string.Empty;
                    hdnFinaliza.Value = rnPerfil.PossuiPerfilFinalizaCardapioPor(User.Identity.Name) ? "S" : string.Empty;                  
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            if (tseEletivaManha_1.DBValue.IsNull && !hdnEletivaManha1.Value.IsNullOrEmptyOrWhiteSpace())
            {
                tseEletivaManha_1.DBValue = hdnEletivaManha1.Value;
            }

            if (tseEletivaManha_2.DBValue.IsNull && !hdnEletivaManha2.Value.IsNullOrEmptyOrWhiteSpace())
            {
                tseEletivaManha_2.DBValue = hdnEletivaManha2.Value;
            }

            if (tseEletivaTarde_1.DBValue.IsNull && !hdnEletivaTarde1.Value.IsNullOrEmptyOrWhiteSpace())
            {
                tseEletivaTarde_1.DBValue = hdnEletivaTarde1.Value;
            }

            if (tseEletivaTarde_2.DBValue.IsNull && !hdnEletivaTarde2.Value.IsNullOrEmptyOrWhiteSpace())
            {
                tseEletivaTarde_2.DBValue = hdnEletivaTarde2.Value;
            }

            if (tseEletivaNoite_1.DBValue.IsNull && !hdnEletivaNoite1.Value.IsNullOrEmptyOrWhiteSpace())
            {
                tseEletivaNoite_1.DBValue = hdnEletivaNoite1.Value;
            }

            if (tseEletivaNoite_2.DBValue.IsNull && !hdnEletivaNoite2.Value.IsNullOrEmptyOrWhiteSpace())
            {
                tseEletivaNoite_2.DBValue = hdnEletivaNoite2.Value;
            }

            if (tseEletivaIntegral_1.DBValue.IsNull && !hdnEletivaIntegral1.Value.IsNullOrEmptyOrWhiteSpace())
            {
                tseEletivaIntegral_1.DBValue = hdnEletivaIntegral1.Value;
            }

            if (tseEletivaIntegral_2.DBValue.IsNull && !hdnEletivaIntegral2.Value.IsNullOrEmptyOrWhiteSpace())
            {
                tseEletivaIntegral_2.DBValue = hdnEletivaIntegral2.Value;
            }

            if (tseEletivaAmpliado_1.DBValue.IsNull && !hdnEletivaAmpliado1.Value.IsNullOrEmptyOrWhiteSpace())
            {
                tseEletivaAmpliado_1.DBValue = hdnEletivaAmpliado1.Value;
            }

            if (tseEletivaAmpliado_2.DBValue.IsNull && !hdnEletivaAmpliado2.Value.IsNullOrEmptyOrWhiteSpace())
            {
                tseEletivaAmpliado_2.DBValue = hdnEletivaAmpliado2.Value;
            } 
        }

        private void ControlarVisibilidadeControle(ImageButton[] botoes)
        {
            RetiraVisibilidadeBotao();

            foreach (var botao in botoes)
            {
                botao.Visible = true;
            }

            ControlaAcesso(btnSalvar, AcaoControle.editar);

        }

        private void RetiraVisibilidadeBotao()
        {

            btnFinalizar.Visible = false;
            btnValidar.Visible = false;
            btnSalvar.Visible = false;
            btnLimpar.Visible = false;

        }

        protected void tseMunicipio_Changed(object sender, ChangedEventArgs args)
        {
            try
            {


                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;
                
                tseAno.ResetValue();
                tsePeriodo.ResetValue();
                tseCurso.ResetValue();
                tseSerie.ResetValue();

                this.lblMensagem.Text = string.Empty;
                pnlTurnos.Visible = false;
                LimparEletivas();
                ImageButton[] controles = new ImageButton[] { };
                ControlarVisibilidadeControle(controles);

                if (this.Page.IsCallback)
                {
                    return;
                }

                var sessao = SessaoUsuario.GetSessaoUsuario();

                if (sessao != null)
                {
                    if (!this.tseMunicipio.DBValue.IsNull)
                    {
                        if (this.tseMunicipio.IsValidDBValue)
                        {
                            sessao.Municipio = Convert.ToString(this.tseMunicipio.DBValue);

                            sessao.Escola = string.Empty;
                            this.tseUnidadeResponsavel.ResetValue();
                        }
                        else
                        {
                            sessao.Municipio = string.Empty;
                            sessao.Escola = string.Empty;
                        }
                    }
                    else
                    {
                        sessao.Municipio = string.Empty;
                        sessao.Escola = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseUnidadeResponsavel_Changed(object sender, ChangedEventArgs args)
        {
            try
            {

                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;

                this.lblMensagem.Text = string.Empty;
                lblMensagemFinalizacao.Text = string.Empty;
                lblMensagemValidacao.Text = string.Empty;

                ImageButton[] controles = new ImageButton[] { };
                ControlarVisibilidadeControle(controles);

                pnlTurnos.Visible = false;
                LimparEletivas();
                tseAno.ResetValue();
                tsePeriodo.ResetValue();
                tseCurso.ResetValue();
                tseSerie.ResetValue();

                var sessao = SessaoUsuario.GetSessaoUsuario();

                if (!this.tseUnidadeResponsavel.DBValue.IsNull)
                {
                    if (this.tseUnidadeResponsavel.IsValidDBValue)
                    {
                        if (!this.tseUnidadeResponsavel["unidade_ens"].IsNull)
                        {
                            sessao.Escola = Convert.ToString(this.tseUnidadeResponsavel.DBValue);
                            this.tseMunicipio.Value = this.tseUnidadeResponsavel["municipio"];
                        }

                        this.lblMensagem.Text = string.Empty;

                    }
                    else
                    {
                        if (sessao != null)
                        {
                            sessao.Escola = string.Empty;
                            sessao.Municipio = string.Empty;
                            sessao.Coordenadoria = string.Empty;
                        }


                        this.lblMensagem.Text = "Unidade de Ensino não cadastrada.";

                    }
                }
                else
                {
                    if (sessao != null)
                    {
                        sessao.Escola = string.Empty;
                        sessao.Municipio = string.Empty;
                        sessao.Coordenadoria = string.Empty;
                    }


                    this.lblMensagem.Text = "Favor consultar uma unidade de ensino.";
                    
                    this.LimparCampos();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseAno_Changed(object sender, ChangedEventArgs args)
        {
            try
            {

                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;
                this.lblMensagem.Text = string.Empty;
                ImageButton[] controles = new ImageButton[] { };
                ControlarVisibilidadeControle(controles);

                pnlTurnos.Visible = false;
                LimparEletivas();
                tsePeriodo.ResetValue();
                tseCurso.ResetValue();
                tseSerie.ResetValue();
                this.lblMensagem.Text = string.Empty;

                var sessao = SessaoUsuario.GetSessaoUsuario();

                if (!this.tseAno.DBValue.IsNull)
                {
                    if (!this.tseAno.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Ano não cadastrado.";

                    }
                }
                else
                {


                    this.lblMensagem.Text = "Favor consultar um ano.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void tsePeriodo_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;

                this.lblMensagem.Text = string.Empty;
                ImageButton[] controles = new ImageButton[] { };
                ControlarVisibilidadeControle(controles);
                pnlTurnos.Visible = false;
                LimparEletivas();
                tseCurso.ResetValue();
                tseSerie.ResetValue();
                this.lblMensagem.Text = string.Empty;

                if (!this.tsePeriodo.DBValue.IsNull)
                {
                    if (!this.tsePeriodo.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Período não cadastrado.";

                    }
                }
                else
                {

                    this.lblMensagem.Text = "Favor consultar um Período.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void tseCurso_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;

                this.lblMensagem.Text = string.Empty;
                ImageButton[] controles = new ImageButton[] { };
                ControlarVisibilidadeControle(controles);
                tseSerie.ResetValue();

                if (!this.tseCurso.DBValue.IsNull)
                {
                    if (!this.tseCurso.IsValidDBValue)
                    {
                        this.lblMensagem.Text = "Curso não cadastrado.";

                    }
                }
                else
                {

                    this.lblMensagem.Text = "Favor consultar um Curso.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void tseSerie_Changed(object sender, ChangedEventArgs args)
        {
            try
            {
                //ignora callbacks causados controles
                if (Page.IsCallback)
                    return;

                this.lblMensagem.Text = string.Empty;
                ImageButton[] controles = new ImageButton[] { };
                ControlarVisibilidadeControle(controles);

                pnlManha.Enabled = false;
                pnlTarde.Enabled = false;
                pnlNoite.Enabled = false;
                pnlIntegral.Enabled = false;
                pnlAmpliado.Enabled = false;
                pnlTurnos.Visible = false;
                LimparEletivas();
                RN.Turmas.CardapioEletiva rnCardapioEletiva = new Techne.Lyceum.RN.Turmas.CardapioEletiva();
                RN.Turmas.DTOs.DadosCardapio dados = new Techne.Lyceum.RN.Turmas.DTOs.DadosCardapio();
                RN.Turma rnTurmas = new Techne.Lyceum.RN.Turma();
                List<string> turnos = new List<string>();
                lblMensagem.Text = string.Empty;
                lblMensagemFinalizacao.Text = string.Empty;
                lblMensagemValidacao.Text = string.Empty;

                if (!this.tseSerie.DBValue.IsNull)
                {
                    if (this.tseSerie.IsValidDBValue)
                    {
                        if ((!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue) &&
                              (!tseAno.DBValue.IsNull && tseAno.IsValidDBValue) &&
                              (!tsePeriodo.DBValue.IsNull && tsePeriodo.IsValidDBValue) &&
                              (!tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue) &&
                              (!tseSerie.DBValue.IsNull && tseSerie.IsValidDBValue))
                        {
                            ImageButton[] controlesSerie = new ImageButton[] { btnSalvar };
                            ControlarVisibilidadeControle(controlesSerie);                                                       

                            btnValidar.Visible = hdnValida.Value == "S";
                            btnFinalizar.Visible = hdnFinaliza.Value == "S";

                            turnos = rnTurmas.ListaTurnoPor(tseUnidadeResponsavel.DBValue.ToString(), Convert.ToInt32(tseAno.DBValue), Convert.ToInt32(tsePeriodo.DBValue), tseCurso.DBValue.ToString(), Convert.ToInt32(tseSerie.DBValue));

                            if (turnos.Count > 0)
                            {
                                pnlManha.Enabled = turnos.Contains("M");
                                pnlTarde.Enabled = turnos.Contains("T");
                                pnlNoite.Enabled = turnos.Contains("N");
                                pnlIntegral.Enabled = turnos.Contains("I");
                                pnlAmpliado.Enabled = turnos.Contains("A");

                                pnlTurnos.Visible = true;
                            }
                            else
                            {
                                lblMensagem.Text = "Esta Unidade de Ensino não possui turno habilitado para este Ano/Período/Curso/Série";
                                return;
                            }


                            dados = rnCardapioEletiva.ObtemPor(tseUnidadeResponsavel.DBValue.ToString(), Convert.ToInt32(tseAno.DBValue.ToString()), Convert.ToInt32(tsePeriodo.DBValue.ToString()), tseCurso.DBValue.ToString(), Convert.ToInt32(tseSerie.DBValue.ToString()));
                            if (dados.CardapioEletivaId > 0)
                            {
                                hdnEletivaManha1.Value = !dados.DisciplinaManha1.IsNullOrEmptyOrWhiteSpace() ? dados.DisciplinaManha1 : string.Empty;
                                hdnEletivaManha2.Value = !dados.DisciplinaManha2.IsNullOrEmptyOrWhiteSpace() ? dados.DisciplinaManha2 : string.Empty;
                                hdnEletivaTarde1.Value = !dados.DisciplinaTarde1.IsNullOrEmptyOrWhiteSpace() ? dados.DisciplinaTarde1 : string.Empty;
                                hdnEletivaTarde2.Value = !dados.DisciplinaTarde2.IsNullOrEmptyOrWhiteSpace() ? dados.DisciplinaTarde2 : string.Empty;
                                hdnEletivaNoite1.Value = !dados.DisciplinaNoite1.IsNullOrEmptyOrWhiteSpace() ? dados.DisciplinaNoite1 : string.Empty;
                                hdnEletivaNoite2.Value = !dados.DisciplinaNoite2.IsNullOrEmptyOrWhiteSpace() ? dados.DisciplinaNoite2 : string.Empty;
                                hdnEletivaIntegral1.Value = !dados.DisciplinaIntegral1.IsNullOrEmptyOrWhiteSpace() ? dados.DisciplinaIntegral1 : string.Empty;
                                hdnEletivaIntegral2.Value = !dados.DisciplinaIntegral2.IsNullOrEmptyOrWhiteSpace() ? dados.DisciplinaIntegral2 : string.Empty;
                                hdnEletivaAmpliado1.Value = !dados.DisciplinaAmpliado1.IsNullOrEmptyOrWhiteSpace() ? dados.DisciplinaAmpliado1 : string.Empty;
                                hdnEletivaAmpliado2.Value = !dados.DisciplinaAmpliado2.IsNullOrEmptyOrWhiteSpace() ? dados.DisciplinaAmpliado2 : string.Empty;


                                if (!dados.Finalizado && hdnFinaliza.Value == "S")
                                {
                                    btnFinalizar.Visible = true;
                                   
                                }
                                else if (dados.Finalizado)
                                {
                                    lblMensagemFinalizacao.Text = "O Cardápio das eletivas foi  finalizado por " + dados.UsuarioFinalizacao + " - " + dados.NomeUsuarioFinalizacao + " em " + dados.DataFinalizacao;
                                    btnSalvar.Visible = false;
                                    btnFinalizar.Visible = false;
                                }

                                if (!dados.Validado && hdnValida.Value == "S")
                                {
                                    btnValidar.Visible = true;
                                }
                                else if (dados.Validado)
                                {
                                    lblMensagemValidacao.Text = "O Cardápio das eletivas foi validado por " + dados.UsuarioValidacao + " - " + dados.NomeUsuarioValidacao + " em " + dados.DataValidacao;
                                    btnSalvar.Visible = false;
                                    btnValidar.Visible = false;
                                }

                                if (dados.Validado && hdnFinaliza.Value == "S")
                                {
                                    btnLimpar.Visible = true;
                                }
                            }                         

                        }

                        this._tipoOperacao = TipoOperacao.ConsultaSerie;
                        ControlarTSearchs();

                    }
                    else
                    {
                        this.lblMensagem.Text = "Série não cadastrada.";
                    }
                }
                else
                {

                    this.lblMensagem.Text = "Favor consultar uma Série.";
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void LimparCampos()
        {
            this.lblMensagem.Text = string.Empty;
            lblMensagemFinalizacao.Text = string.Empty;
            lblMensagemValidacao.Text = string.Empty;

            this.tseMunicipio.ResetValue();
            this.tseUnidadeResponsavel.ResetValue();

            hdnFinaliza.Value = string.Empty;
            hdnValida.Value = string.Empty;

        }

        private void LimparEletivas()
        {
            this.tseEletivaManha_1.ResetValue();
            this.tseEletivaManha_2.ResetValue();
            this.tseEletivaTarde_1.ResetValue();
            this.tseEletivaTarde_2.ResetValue();
            this.tseEletivaNoite_1.ResetValue();
            this.tseEletivaNoite_2.ResetValue();
            this.tseEletivaIntegral_1.ResetValue();
            this.tseEletivaIntegral_2.ResetValue();
            this.tseEletivaAmpliado_1.ResetValue();
            this.tseEletivaAmpliado_2.ResetValue();
            hdnEletivaManha1.Value = string.Empty;
            hdnEletivaManha2.Value = string.Empty;
            hdnEletivaTarde1.Value = string.Empty;
            hdnEletivaTarde2.Value = string.Empty;
            hdnEletivaNoite1.Value = string.Empty;
            hdnEletivaNoite2.Value = string.Empty;
            hdnEletivaIntegral1.Value = string.Empty;
            hdnEletivaIntegral2.Value = string.Empty;
            hdnEletivaAmpliado1.Value = string.Empty;
            hdnEletivaAmpliado2.Value = string.Empty;
        }

        protected void btnSalvar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.lblMensagem.Text = string.Empty;

                ValidacaoDados validacao = new ValidacaoDados();
                RN.Turmas.CardapioEletiva rnCardapioEletiva = new Techne.Lyceum.RN.Turmas.CardapioEletiva();
                RN.Turmas.DTOs.DadosCardapio dados = new Techne.Lyceum.RN.Turmas.DTOs.DadosCardapio();

                if ((!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue) &&
                   (!tseAno.DBValue.IsNull && tseAno.IsValidDBValue) &&
                   (!tsePeriodo.DBValue.IsNull && tsePeriodo.IsValidDBValue) &&
                   (!tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue) &&
                   (!tseSerie.DBValue.IsNull && tseSerie.IsValidDBValue))
                {
                    dados.DisciplinaManha1 = (!tseEletivaManha_1.DBValue.IsNull && tseEletivaManha_1.IsValidDBValue) ? tseEletivaManha_1.DBValue.ToString() : null;
                    dados.DisciplinaManha2 = (!tseEletivaManha_2.DBValue.IsNull && tseEletivaManha_2.IsValidDBValue) ? tseEletivaManha_2.DBValue.ToString() : null;
                    dados.DisciplinaTarde1 = (!tseEletivaTarde_1.DBValue.IsNull && tseEletivaTarde_1.IsValidDBValue) ? tseEletivaTarde_1.DBValue.ToString() : null;
                    dados.DisciplinaTarde2 = (!tseEletivaTarde_2.DBValue.IsNull && tseEletivaTarde_2.IsValidDBValue) ? tseEletivaTarde_2.DBValue.ToString() : null;
                    dados.DisciplinaNoite1 = (!tseEletivaNoite_1.DBValue.IsNull && tseEletivaNoite_1.IsValidDBValue) ? tseEletivaNoite_1.DBValue.ToString() : null;
                    dados.DisciplinaNoite2 = (!tseEletivaNoite_2.DBValue.IsNull && tseEletivaNoite_2.IsValidDBValue) ? tseEletivaNoite_2.DBValue.ToString() : null;
                    dados.DisciplinaIntegral1 = (!tseEletivaIntegral_1.DBValue.IsNull && tseEletivaIntegral_1.IsValidDBValue) ? tseEletivaIntegral_1.DBValue.ToString() : null;
                    dados.DisciplinaIntegral2 = (!tseEletivaIntegral_2.DBValue.IsNull && tseEletivaIntegral_2.IsValidDBValue) ? tseEletivaIntegral_2.DBValue.ToString() : null;
                    dados.DisciplinaAmpliado1 = (!tseEletivaAmpliado_1.DBValue.IsNull && tseEletivaAmpliado_1.IsValidDBValue) ? tseEletivaAmpliado_1.DBValue.ToString() : null;
                    dados.DisciplinaAmpliado2 = (!tseEletivaAmpliado_2.DBValue.IsNull && tseEletivaAmpliado_2.IsValidDBValue) ? tseEletivaAmpliado_2.DBValue.ToString() : null;

                    dados.Ano = (!tseAno.DBValue.IsNull && tseAno.IsValidDBValue) ? Convert.ToInt32(tseAno.DBValue) : -1;
                    dados.Periodo = (!tsePeriodo.DBValue.IsNull && tsePeriodo.IsValidDBValue) ? Convert.ToInt32(tsePeriodo.DBValue) : -1;
                    dados.Serie = (!tseSerie.DBValue.IsNull && tseSerie.IsValidDBValue) ? Convert.ToInt32(tseSerie.DBValue) : -1;
                    dados.Curso = (!tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue) ? tseCurso.DBValue.ToString() : null;
                    dados.Censo = (!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue) ? tseUnidadeResponsavel.DBValue.ToString() : null;
                    dados.UsuarioId = User.Identity.Name;

                    validacao = rnCardapioEletiva.Valida(dados);

                    if (validacao.Valido)
                    {
                        rnCardapioEletiva.Salva(dados);

                        lblMensagem.Text = "Cardápio de Eletiva salvo com sucesso";
                    }
                    else
                    {
                        lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    }
                }
                else
                {
                    lblMensagem.Text = "Para salvar é necessario preencher a Unidade de ensino/Ano/Período/Curso/Série";

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnValidar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.lblMensagem.Text = string.Empty;

                ValidacaoDados validacao = new ValidacaoDados();
                RN.Turmas.CardapioEletiva rnCardapioEletiva = new Techne.Lyceum.RN.Turmas.CardapioEletiva();
                RN.Turmas.DTOs.DadosCardapio dados = new Techne.Lyceum.RN.Turmas.DTOs.DadosCardapio();

                if ((!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue) &&
                   (!tseAno.DBValue.IsNull && tseAno.IsValidDBValue) &&
                   (!tsePeriodo.DBValue.IsNull && tsePeriodo.IsValidDBValue) &&
                   (!tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue) &&
                   (!tseSerie.DBValue.IsNull && tseSerie.IsValidDBValue))
                {
                    validacao = rnCardapioEletiva.ValidaValidacao(tseUnidadeResponsavel.DBValue.ToString(), Convert.ToInt32(tseAno.DBValue), Convert.ToInt32(tsePeriodo.DBValue), tseCurso.DBValue.ToString(), Convert.ToInt32(tseSerie.DBValue), User.Identity.Name);

                    if (validacao.Valido)
                    {
                        rnCardapioEletiva.SalvaValidacao(tseUnidadeResponsavel.DBValue.ToString(), Convert.ToInt32(tseAno.DBValue), Convert.ToInt32(tsePeriodo.DBValue), tseCurso.DBValue.ToString(), Convert.ToInt32(tseSerie.DBValue), User.Identity.Name);
                        lblMensagem.Text = "Cardápio de Eletiva validado.";
                        btnSalvar.Visible = false;
                        btnValidar.Visible = false;                       

                        if (hdnFinaliza.Value == "S")
                        {
                            btnLimpar.Visible = true;
                            btnFinalizar.Visible = true;
                        }
                    }
                    else
                    {
                        lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    }
                }
                else
                {
                    lblMensagem.Text = "Para realizar a validar é necessario preencher a Unidade de ensino/Ano/Período/Curso/Série";

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnFinalizar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.lblMensagem.Text = string.Empty;

                ValidacaoDados validacao = new ValidacaoDados();
                RN.Turmas.CardapioEletiva rnCardapioEletiva = new Techne.Lyceum.RN.Turmas.CardapioEletiva();
                RN.Turmas.DTOs.DadosCardapio dados = new Techne.Lyceum.RN.Turmas.DTOs.DadosCardapio();

                if ((!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue) &&
                    (!tseAno.DBValue.IsNull && tseAno.IsValidDBValue) &&
                    (!tsePeriodo.DBValue.IsNull && tsePeriodo.IsValidDBValue) &&
                    (!tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue) &&
                    (!tseSerie.DBValue.IsNull && tseSerie.IsValidDBValue))
                {
                    validacao = rnCardapioEletiva.ValidaFinalizacao(tseUnidadeResponsavel.DBValue.ToString(), Convert.ToInt32(tseAno.DBValue), Convert.ToInt32(tsePeriodo.DBValue), tseCurso.DBValue.ToString(), Convert.ToInt32(tseSerie.DBValue), User.Identity.Name);

                    if (validacao.Valido)
                    {
                        rnCardapioEletiva.SalvaFinalizacao(tseUnidadeResponsavel.DBValue.ToString(), Convert.ToInt32(tseAno.DBValue), Convert.ToInt32(tsePeriodo.DBValue), tseCurso.DBValue.ToString(), Convert.ToInt32(tseSerie.DBValue), User.Identity.Name);

                        lblMensagem.Text = "Cardápio de Eletiva finalizado.";
                    }
                    else
                    {
                        lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    }
                }
                else
                {
                    lblMensagem.Text = "Para realizar a finalização é necessario preencher a Unidade de ensino/Ano/Período/Curso/Série";

                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }


        protected void btnLimpar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                this.lblMensagem.Text = string.Empty;

                ValidacaoDados validacao = new ValidacaoDados();
                RN.Turmas.CardapioEletiva rnCardapioEletiva = new Techne.Lyceum.RN.Turmas.CardapioEletiva();
                RN.Turmas.DTOs.DadosCardapio dados = new Techne.Lyceum.RN.Turmas.DTOs.DadosCardapio();


                if ((!tseUnidadeResponsavel.DBValue.IsNull && tseUnidadeResponsavel.IsValidDBValue) &&
                     (!tseAno.DBValue.IsNull && tseAno.IsValidDBValue) &&
                     (!tsePeriodo.DBValue.IsNull && tsePeriodo.IsValidDBValue) &&
                     (!tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue) &&
                     (!tseSerie.DBValue.IsNull && tseSerie.IsValidDBValue))
                {
                    validacao = rnCardapioEletiva.ValidaLimpar(tseUnidadeResponsavel.DBValue.ToString(), Convert.ToInt32(tseAno.DBValue), Convert.ToInt32(tsePeriodo.DBValue), tseCurso.DBValue.ToString(), Convert.ToInt32(tseSerie.DBValue), User.Identity.Name);

                    if (validacao.Valido)
                    {
                        rnCardapioEletiva.SalvaLimpar(tseUnidadeResponsavel.DBValue.ToString(), Convert.ToInt32(tseAno.DBValue), Convert.ToInt32(tsePeriodo.DBValue), tseCurso.DBValue.ToString(), Convert.ToInt32(tseSerie.DBValue), User.Identity.Name);

                        lblMensagemFinalizacao.Text = string.Empty;
                        lblMensagemValidacao.Text = string.Empty;
                        lblMensagem.Text = "Validação do Cardápio de Eletiva foi retirada com sucesso.";
                        btnLimpar.Visible = false;       
                       

                        ImageButton[] controlesSerie = new ImageButton[] { btnSalvar };
                        ControlarVisibilidadeControle(controlesSerie);

                        btnValidar.Visible = hdnValida.Value == "S";
                        btnFinalizar.Visible = hdnFinaliza.Value == "S";
                    }
                    else
                    {
                        lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    }
                }
                else
                {
                    lblMensagem.Text = "Para realizar a limpeza da validação é necessario preencher a Unidade de ensino/Ano/Período/Curso/Série";

                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseEletivaManha_1_Load(object sender, EventArgs e)
        {
            ControlarTSearchs();
        }

        private void ControlarTSearchs()
        {
            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        tseEletivaManha_1.Mode = ControlMode.View;
                        tseEletivaManha_2.Mode = ControlMode.View;
                        tseEletivaTarde_1.Mode = ControlMode.View;
                        tseEletivaTarde_2.Mode = ControlMode.View;
                        tseEletivaNoite_1.Mode = ControlMode.View;
                        tseEletivaNoite_2.Mode = ControlMode.View;
                        tseEletivaIntegral_1.Mode = ControlMode.View;
                        tseEletivaIntegral_2.Mode = ControlMode.View;
                        tseEletivaAmpliado_1.Mode = ControlMode.View;
                        tseEletivaAmpliado_2.Mode = ControlMode.View;
                        break;
                    }
                case TipoOperacao.ConsultaSerie:
                    {
                        if (!pnlManha.Enabled)
                        {
                            tseEletivaManha_1.Mode = ControlMode.View;
                            tseEletivaManha_2.Mode = ControlMode.View;
                        }
                        else
                        {
                            tseEletivaManha_1.Mode = ControlMode.Edit;
                            tseEletivaManha_2.Mode = ControlMode.Edit;
                        }

                        if (!pnlTarde.Enabled)
                        {
                            tseEletivaTarde_1.Mode = ControlMode.View;
                            tseEletivaTarde_2.Mode = ControlMode.View;
                        }
                        else
                        {
                            tseEletivaTarde_1.Mode = ControlMode.Edit;
                            tseEletivaTarde_2.Mode = ControlMode.Edit;
                        }

                        if (!pnlNoite.Enabled)
                        {
                            tseEletivaNoite_1.Mode = ControlMode.View;
                            tseEletivaNoite_2.Mode = ControlMode.View;
                        }
                        else
                        {
                            tseEletivaNoite_1.Mode = ControlMode.Edit;
                            tseEletivaNoite_2.Mode = ControlMode.Edit;
                        }

                        if (!pnlIntegral.Enabled)
                        {
                            tseEletivaIntegral_1.Mode = ControlMode.View;
                            tseEletivaIntegral_2.Mode = ControlMode.View;
                        }
                        else
                        {
                            tseEletivaIntegral_1.Mode = ControlMode.Edit;
                            tseEletivaIntegral_2.Mode = ControlMode.Edit;
                        }

                        if (!pnlAmpliado.Enabled)
                        {
                            tseEletivaAmpliado_1.Mode = ControlMode.View;
                            tseEletivaAmpliado_2.Mode = ControlMode.View;
                        }
                        else
                        {
                            tseEletivaAmpliado_1.Mode = ControlMode.Edit;
                            tseEletivaAmpliado_2.Mode = ControlMode.Edit;
                        }

                        if ((!btnSalvar.Visible && !btnValidar.Visible && !btnFinalizar.Visible) || (!btnSalvar.Visible))
                        {
                            tseEletivaManha_1.Mode = ControlMode.View;
                            tseEletivaManha_2.Mode = ControlMode.View;
                            tseEletivaTarde_1.Mode = ControlMode.View;
                            tseEletivaTarde_2.Mode = ControlMode.View;
                            tseEletivaNoite_1.Mode = ControlMode.View;
                            tseEletivaNoite_2.Mode = ControlMode.View;
                            tseEletivaIntegral_1.Mode = ControlMode.View;
                            tseEletivaIntegral_2.Mode = ControlMode.View;
                            tseEletivaAmpliado_1.Mode = ControlMode.View;
                            tseEletivaAmpliado_2.Mode = ControlMode.View;
                        }

                        break;
                    }
            }
        }


    }
}



