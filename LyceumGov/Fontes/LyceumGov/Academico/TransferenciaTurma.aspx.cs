using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxGridView;
using Techne.Data;
using Techne.Lyceum.Net.Modulos;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Entidades;
using Techne.Web;
using DevExpress.Web.ASPxEditors;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Linq;
using Techne.Lyceum.RN.DTOs.Agenda;
using Techne.Controls;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/TransferenciaTurma.aspx"),
      ControlText("TransferenciaTurma"),
      Title("Transferência de Turma"),]

    public partial class TransferenciaTurma : TPage
    {
        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }
        private void InitializeComponent()
        {

        }
        #endregion

        #region Propriedades e Enumeradores

        public enum TipoOperacao
        {
            Novo,
            Excluir,
            Alterar,
            Consultar,
            Inicial,
            Sucesso,
            Retorno,
            SucessoPermuta
        }

        private TipoOperacao _tipoOperacao
        {
            get { return (TipoOperacao)ViewState["_tipoOperacao"]; }
            set { ViewState["_tipoOperacao"] = value; }
        }

        #endregion

        #region Eventos

        protected void Page_Init(object sender, EventArgs e)
        {
            LyceumMaster mp = (LyceumMaster)Master;
            mp.habilitaLoading = true;
            TituloGrid(this.grdProgressao, string.Empty);
            TituloGrid(this.grdOptativaReforco, string.Empty);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblMensagem.Text = string.Empty;

                //Desabilita campos, pois ira valer os dados da confirmação 
                DesabilitarCamposMatriculaPrincipal();

                if (!IsPostBack)
                {
                    _tipoOperacao = TipoOperacao.Inicial;
                    this.ControlarTipoOperacao();
                }
                CarregaTSearchs();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                lblMensagemTurma.Text = string.Empty;
                ppcTransfTurma.ShowOnPageLoad = false;
                RN.DTOs.DadosTransferencia dadosTransferencia = this.ObterDadosTela();
                RN.DTOs.DadosTransferencia dadosTransferenciaPermuta = new DadosTransferencia();
                RN.TransferenciaTurma rnTransferenciaTurma = new Techne.Lyceum.RN.TransferenciaTurma();

                if (dadosTransferencia.GradeIdAtual.IsNullOrEmptyOrWhiteSpace())
                {
                    lblMensagem.Text = "Grade Id não encontrado. Verifique!";
                    return;
                }

                if (rblConfirmacao.SelectedValue == "Sim")
                {
                    dadosTransferenciaPermuta = ObterDadosPermuta();
                }

                string aviso;
                rnTransferenciaTurma.TransfereTurmaPrincipal(dadosTransferencia, (rblConfirmacao.SelectedValue == "Sim" ? true : false), dadosTransferenciaPermuta, out aviso);
                tseAluno.ResetValue();
                tseAluno.DataBind();
                tseAluno.DBValue = dadosTransferencia.Aluno;

                _tipoOperacao = TipoOperacao.Sucesso;
                this.ControlarTipoOperacao();

                if (!aviso.IsNullOrEmptyOrWhiteSpace())
                {
                    lblMensagem.Text += "<br/>" + aviso;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                lblMensagemTurma.Text = ex.Message;
            }
        }

        protected void btnTransferirEducEspecial_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();

                lblMensagemTurma.Text = string.Empty;

                var lyMatriculaOrigem = new LyMatricula
                {
                    Ano = decimal.Parse(lblAnoEducEspecialOrigem.Text),
                    Semestre = decimal.Parse(lblPeriodoEducEspecialOrigem.Text),
                    Turma = lblTurmaEducEspecialOrigem.Text,
                    Matricula = User.Identity.Name,
                    Aluno = Convert.ToString(tseAluno.DBValue)
                };

                var lyMatriculaDestino = new LyMatricula
                {
                    Ano = decimal.Parse(lblAnoEducEspecialOrigem.Text),
                    Semestre = decimal.Parse(lblPeriodoEducEspecialOrigem.Text),
                    Turma = ddlTurmaEducEspecialDestino.SelectedValue,
                    Matricula = User.Identity.Name,
                    Aluno = Convert.ToString(tseAluno.DBValue)
                };

                var atendimentos = new List<Atendimento>();

                foreach (ListItem li in lbHorariosDestino.Items)
                {
                    var a = new Atendimento
                                {
                                    Disciplina = li.Value,
                                    Horario = li.Text
                                };

                    atendimentos.Add(a);
                }

                validacao = RN.TransferenciaTurma.ValidarTransEducacaoEspecial(lyMatriculaOrigem, lyMatriculaDestino, atendimentos);

                if (validacao.Valido)
                {
                    RN.TransferenciaTurma.TransferirTurmaEducacaoEspecial(lyMatriculaOrigem, lyMatriculaDestino, atendimentos);
                    _tipoOperacao = TipoOperacao.Sucesso;
                    this.ControlarTipoOperacao();

                    var mensagem = "Turma de Educação Especial transferida com sucesso.";
                    var script = @"alert('" + mensagem + @"');";
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem;
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnTransferirMaisEducacao_Click(object sender, EventArgs e)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                RN.TransferenciaTurma rnTransferenciaTurma = new Techne.Lyceum.RN.TransferenciaTurma();
                lblMensagemTurma.Text = string.Empty;

                var lyMatriculaOrigem = new LyMatricula
                {
                    Ano = decimal.Parse(lblAnoMaisEducacao.Text),
                    Semestre = decimal.Parse(lblPeriodoMaisEducacao.Text),
                    Turma = lblTurmaMaisEducacaoOrigem.Text,
                    Matricula = User.Identity.Name,
                    Aluno = Convert.ToString(tseAluno.DBValue)
                };

                var lyMatriculaDestino = new LyMatricula
                {
                    Ano = decimal.Parse(lblAnoMaisEducacao.Text),
                    Semestre = decimal.Parse(lblPeriodoMaisEducacao.Text),
                    Turma = ddlTurmaMaisEducacaoDestino.SelectedValue.Split('|')[1],
                    Matricula = User.Identity.Name,
                    Aluno = Convert.ToString(tseAluno.DBValue)
                };

                validacao = rnTransferenciaTurma.ValidarTransMaisEducacao(lyMatriculaOrigem, lyMatriculaDestino);

                if (validacao.Valido)
                {
                    RN.TransferenciaTurma.TransferirTurmaMaisEducacao(lyMatriculaOrigem, lyMatriculaDestino);
                    _tipoOperacao = TipoOperacao.Sucesso;
                    this.ControlarTipoOperacao();

                    var mensagem = "Turma de Mais Educação transferida com sucesso.";
                    var script = @"alert('" + mensagem + @"');";
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem;
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private bool ValidarAdicao()
        {
            if (string.IsNullOrEmpty(ddlHorarioEducEspecialDestino.SelectedValue))
            {
                lblMensagem.Text = "Selecione um horário para ser adicionado.";
                var mensagem = lblMensagem.Text;
                var script = @"alert('" + mensagem + @"');";
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);

                return false;
            }

            if (lbHorariosDestino.Items.Count > 3)
            {
                lblMensagem.Text = "Podem ser adicionados no máximo 3 horários.";
                var mensagem = lblMensagem.Text;
                var script = @"alert('" + mensagem + @"');";
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);

                return false;
            }

            foreach (ListItem li in lbHorariosDestino.Items)
            {
                if (ddlHorarioEducEspecialDestino.SelectedValue == li.Value)
                {
                    lblMensagem.Text = "Este horário já foi adicionado anteriormente.";
                    var mensagem = lblMensagem.Text;
                    var script = @"alert('" + mensagem + @"');";
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);

                    return false;
                }
            }
            return true;
        }

        protected void btnAdicionar_Click(object sender, EventArgs e)
        {
            if (this.ValidarAdicao())
            {
                lbHorariosDestino.Items.Add(new ListItem(ddlHorarioEducEspecialDestino.SelectedItem.Text, ddlHorarioEducEspecialDestino.SelectedValue));
                ddlHorarioEducEspecialDestino.ClearSelection();
            }
        }

        protected void btnRemover_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lbHorariosDestino.SelectedValue))
            {
                lblMensagem.Text = "Selecione um horário para ser removido.";
                var mensagem = lblMensagem.Text;
                var script = @"alert('" + mensagem + @"');";
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);

                return;
            }

            lbHorariosDestino.Items.Remove(lbHorariosDestino.SelectedItem);
        }

        protected void btnSalvarConcomitante_Click(object sender, EventArgs e)
        {
            try
            {
                RN.TransferenciaTurma rnTransferenciaTurma = new Techne.Lyceum.RN.TransferenciaTurma();
                ValidacaoDados validacao = new ValidacaoDados();
                lblMensagemTurma.Text = string.Empty;

                var lyMatricula = new LyMatricula()
                {
                    Ano = decimal.Parse(txtAnoLetivo.Text),
                    Semestre = decimal.Parse(lblPeriodoConcomitanteOrigem.Text),
                    Turma = ddlTurmaConcomitante.SelectedValue.Split('|')[1],
                    Matricula = User.Identity.Name,
                    Aluno = Convert.ToString(tseAluno.DBValue)
                };

                LyMatricula lyMatriculaRegular = new LyMatricula()
                {
                    Ano = decimal.Parse(txtAnoLetivo.Text),
                    Semestre = decimal.Parse(txtPeriodoLetivo.Text),
                    Turma = txtTurmaAtual.Text,
                    Matricula = User.Identity.Name,
                    Aluno = tseAluno.DBValue.ToString()
                };

                validacao = rnTransferenciaTurma.ValidarTransConcomitante(lyMatricula, lyMatriculaRegular, ddlTurnoConcomitante.SelectedValue);

                if (validacao.Valido)
                {
                    RN.TransferenciaTurma.TransferirTurmaConcomitante(lyMatricula);
                    _tipoOperacao = TipoOperacao.Sucesso;
                    this.ControlarTipoOperacao();

                    var mensagem = "Turma de Ensino Profissional Concomitante transferida com sucesso.";
                    var script = @"alert('" + mensagem + @"');";
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);

                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem;
                    var mensagem = lblMensagem.Text;
                    var script = @"alert('" + mensagem + @"');";
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            lblMensagemTurma.Text = string.Empty;
            ppcTransfTurma.ShowOnPageLoad = false;
        }

        private void TransfereTurmaPrincipal(bool permuta)
        {
            try
            {
                ValidacaoDados validacao = new ValidacaoDados();
                lblMensagemTurma.Text = string.Empty;
                lblMensagemTurmaPermuta.Text = string.Empty;
                RN.TransferenciaTurma rnTransferenciaTurma = new Techne.Lyceum.RN.TransferenciaTurma();
                RN.DTOs.DadosTransferencia dadosTransferencia = ObterDadosTela();
                RN.DTOs.DadosTransferencia dadosTransferenciaPermuta = new DadosTransferencia();
                RN.ControleVaga rnControleVaga = new ControleVaga();
                List<string> listaAvisosParaConfirmacao = null;
                List<string> listaAvisosParaConfirmacaoPermuta = null;

                //Verifica se existe bloqueio de transferencia para os dados de destino do aluno
                if (this.VerificaBloqueio(false, permuta))
                {
                    lblMensagem.Text = "A transferência não pode ser realizada: " + lblMensagemBloqueio.Text;
                    var mensagem = lblMensagem.Text;
                    var script = @"alert('" + mensagem + @"');";
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                    lblMensagemBloqueio.Text = string.Empty;

                    return;
                }

                if (permuta)
                {
                    dadosTransferenciaPermuta = ObterDadosPermuta();
                }

                validacao = rnTransferenciaTurma.ValidaTransferenciaTurmaPrincipal(dadosTransferencia, out listaAvisosParaConfirmacao, permuta, dadosTransferenciaPermuta, out listaAvisosParaConfirmacaoPermuta);

                if (validacao.Valido)
                {
                    //Verificar se existe avisos para usuario confirmar antes da transferencia
                    if (listaAvisosParaConfirmacao.Count > 0 || listaAvisosParaConfirmacaoPermuta.Count > 0)
                    {
                        //Caso exista avisos abrir popup para confirmação antes de transferir aluno
                        ppcTransfTurma.ShowOnPageLoad = true;

                        if (listaAvisosParaConfirmacao.Count > 0)
                        {
                            lblMensagemTurma.Text = "Deseja realmente efetivar a transferência de turma do(a) Aluno(a): " + tseAluno["nome"].ToString() + "  com esses problemas apresentados ?<br />";
                            lblMensagemTurma.Text += listaAvisosParaConfirmacao.Aggregate((x, y) => x + "<br />" + y);
                        }

                        if (permuta && listaAvisosParaConfirmacaoPermuta.Count > 0)
                        {
                            lblMensagemTurmaPermuta.Text = "Deseja realmente efetivar a transferência de turma do(a) Aluno(a): " + tseAlunoPermuta["nome"].ToString() + "  com esses problemas apresentados ?<br />";
                            lblMensagemTurmaPermuta.Text += listaAvisosParaConfirmacaoPermuta.Aggregate((x, y) => x + "<br />" + y);
                        }
                    }
                    else
                    {
                        string aviso;

                        //Caso não exista avisos transferir aluno
                        rnTransferenciaTurma.TransfereTurmaPrincipal(dadosTransferencia, permuta, dadosTransferenciaPermuta, out aviso);
                        tseAluno.ResetValue();
                        tseAluno.DataBind();
                        tseAluno.DBValue = dadosTransferencia.Aluno;
                        _tipoOperacao = TipoOperacao.Sucesso;
                        this.ControlarTipoOperacao();

                        if (!aviso.IsNullOrEmptyOrWhiteSpace())
                        {
                            lblMensagem.Text += "<br/>" + aviso;
                        }
                    }
                }
                else
                {
                    lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    lblMensagemTurma.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMensagemTurma.Text = ex.Message;
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnTransferir_Click(object sender, EventArgs e)
        {
            try
            {
                RN.Matricula rnMatricula = new RN.Matricula();
                string mensagem = string.Empty;

                //Verifica se o aluno possui enturmação em turma de Optativa de Ensino Religioso
                bool possuiEnsinoReligioso = rnMatricula.PossuiMatriculaOptativaEnsinoReligioso(Convert.ToString(tseAluno.DBValue));

                //Verifica se o aluno possui enturmação em turma de Optativa de Lingua Estrangeira
                bool possuiLinguaEstrangeira = rnMatricula.PossuiMatriculaOptativaLinguaEstrangeira(Convert.ToString(tseAluno.DBValue));

                if (possuiEnsinoReligioso)
                {
                    //Verifica se o curriculo permite
                    if (!chkEnsReligioso.Enabled)
                    {
                        mensagem = "O curriculo da turma de destino não permite optativas Ensino Religioso.<br />";
                    }
                    //Verifica se a opção foi marcada
                    else if (!chkEnsReligioso.Checked)
                    {
                        mensagem = "O aluno possui uma enturmação em optativa Ensino Religioso e não existe marcação na opção.<br />";
                    }
                }

                if (possuiLinguaEstrangeira)
                {
                    //Verifica se o curriculo permite
                    if (!chkLinguaEstrangeira.Enabled)
                    {
                        mensagem += "O curriculo da turma de destino não permite optativas Lingua Estrangeira Facultativa.<br />";
                    }
                    //Verifica se a opção foi marcada
                    else if (!chkLinguaEstrangeira.Checked)
                    {
                        mensagem += "O aluno possui uma enturmação em optativa Lingua Estrangeira Facultativa e não existe marcação na opção.<br />";
                    }
                }

                //Verifica se o aluno possui enturmação em eletivas
                if (rnMatricula.PossuiMatriculaEletivaAtiva(Convert.ToString(tseAluno.DBValue)))
                {
                    mensagem += "O aluno possui uma enturmação em turmas eletivas. Após a transferência deve ser alocado nas novas eletivas<br />";
                }

                if (!string.IsNullOrEmpty(mensagem))
                {
                    mensagem += @"Caso confirme a transferência a enturmação nas turmas optativas e/ou eletivas serão excluídas.<br /><br />
                              Deseja continuar?";

                    this.pucConfirmar.ShowOnPageLoad = true;
                    lblConfirmar.Text = mensagem;
                    return;
                }

                if (rblConfirmacao.SelectedValue.IsNullOrEmptyOrWhiteSpace() && pnlConfirmacao.Visible)
                {
                    lblMensagem.Text = "Para efetuar a transferência é necessário responder se \"Existe algum aluno para colocar na vaga que está sendo liberada? ";
                    return;
                }


                TransfereTurmaPrincipal(rblConfirmacao.SelectedValue == "Sim" ? true : false);

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btConfirma_Click(object sender, EventArgs e)
        {
            try
            {

                TransfereTurmaPrincipal(false);
                grdOptativaReforco.DataBind();
                this.pucConfirmar.ShowOnPageLoad = false;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("TransferenciaTurma.aspx");
        }

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            try
            {
                lblMensagem.Text = string.Empty;
                tseCurso.ResetValue();
                lblMensagemBloqueio.Text = string.Empty;
                lblTipoCurso.Visible = false;
                ddlTipoCurso.Visible = false;
                ddlTipoCurso.ClearSelection();
                if (!tseAluno.DBValue.IsNull)
                {
                    if (tseAluno.IsValidDBValue)
                    {
                        _tipoOperacao = TipoOperacao.Consultar;
                        this.ControlarTipoOperacao();

                        //Verifica se existe bloqueio de transferencia para os dados de origem do aluno
                        if (VerificaBloqueio(true, false))
                        {
                            _tipoOperacao = TipoOperacao.Inicial;
                            this.ControlarTipoOperacao();
                            lblMensagem.Text = string.Empty;
                            return;
                        }

                    }
                    else
                    {
                        lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                        var mensagem = lblMensagem.Text;
                        var script = @"alert('" + mensagem + @"');";
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);

                        _tipoOperacao = TipoOperacao.Inicial;
                        this.ControlarTipoOperacao();
                    }
                }
                else
                {
                    lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                    var mensagem = lblMensagem.Text;
                    var script = @"alert('" + mensagem + @"');";
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);

                    _tipoOperacao = TipoOperacao.Inicial;
                    this.ControlarTipoOperacao();
                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected bool VerificaBloqueio(bool porOrigem, bool permuta)
        {
            RN.Aluno.DadosAluno aluno = new RN.Aluno.DadosAluno();
            RN.Agenda.Agenda rnAgenda = new Techne.Lyceum.RN.Agenda.Agenda();
            int idEventoBloqueioTransferenciaTurma = Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.BloqueioTranferenciaTurma);
            DadosParticipacao bloqueio = new DadosParticipacao();
            RN.Perfil rnPerfil = new Perfil();
            try
            {
                if (porOrigem)
                {
                    //Busca dados atuais do aluno                                
                    aluno.UnidadeResponsavel = !permuta ? tseAluno["unidade_ensino"].ToString() : tseAlunoPermuta["unidade_ensino"].ToString();
                    aluno.Curso = !permuta ? txtCurso.Text : tseAlunoPermuta["curso"].ToString();
                    aluno.Serie = !permuta ? txtSerie.Text : tseAlunoPermuta["serie"].ToString();
                    aluno.Turno = !permuta ? txtTurno.Text : tseAlunoPermuta["turno"].ToString();

                }
                else
                {
                    //Busca dados de destino da transferencia
                    aluno.UnidadeResponsavel = txtUniEnsino.Text;
                    aluno.Curso = Convert.ToString(tseCurso.DBValue);
                    aluno.Turno = ddlTurno.SelectedValue;
                    aluno.Serie = ddlSerie.SelectedValue;
                }

                if (!rnPerfil.PossuiPerfilMatriculaTransferenciaPeriodoBloqueioPor(User.Identity.Name))
                {
                    if (aluno.UnidadeResponsavel.IsNullOrEmptyOrWhiteSpace() || aluno.Curso.IsNullOrEmptyOrWhiteSpace() || aluno.Turno.IsNullOrEmptyOrWhiteSpace() || aluno.Serie.IsNullOrEmptyOrWhiteSpace())
                    {
                        lblMensagemBloqueio.Text = "Para efetuar a transfêrencia é necessario ter os dados de origem/destino preenchidos.";
                        return true;
                    }
                    else
                    {

                        bloqueio = rnAgenda.VerificaEventoInversoPor(idEventoBloqueioTransferenciaTurma, aluno.UnidadeResponsavel, aluno.Curso, aluno.Turno, Convert.ToInt32(aluno.Serie));

                        if (bloqueio.ParticipaTotal)
                        {
                            lblMensagemBloqueio.Text = string.Format("Transferências de alunos entre turmas estão bloqueadas no período de {0} á {1} de acordo com a Agenda da SEEDUC.",
                                       bloqueio.DataInicio.ToString("dd/MM/yyyy"),
                                       bloqueio.DataFim.ToString("dd/MM/yyyy"));
                            return true;
                        }

                        if ((bloqueio.ParticipaCurso && bloqueio.ParticipaUnidade) && porOrigem)
                        {
                            lblMensagemBloqueio.Text = string.Format("Transferências de alunos entre turmas, com os dados da turma atual deste aluno, estão bloqueadas no período de {0} á {1} de acordo com a Agenda da SEEDUC.",
                                       bloqueio.DataInicio.ToString("dd/MM/yyyy"),
                                       bloqueio.DataFim.ToString("dd/MM/yyyy"));
                            return true;
                        }

                        if ((bloqueio.ParticipaCurso && bloqueio.ParticipaUnidade) && !porOrigem)
                        {
                            lblMensagemBloqueio.Text = string.Format("Transferências de alunos entre turmas, com os dados da turma destino, estão bloqueadas no período de {0} á {1} de acordo com a Agenda da SEEDUC.",
                                       bloqueio.DataInicio.ToString("dd/MM/yyyy"),
                                       bloqueio.DataFim.ToString("dd/MM/yyyy"));
                            return true;
                        }
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                return true;
            }
        }

        protected void tseCurso_Changed(object sender, EventArgs args)
        {
            try
            {
                string tipo_curso = string.Empty;
                this.CarregarDadosDrop(ddlTurno.ID);
                this.CarregarDadosDrop(ddlSerie.ID);
                pnlConfirmacao.Visible = false;
                ddlTurmaDestino.Items.Clear();
                var itemVazio = new ListItem("<Lista Vazia>", string.Empty);
                ddlTurmaDestino.Items.Add(itemVazio);

                hdnCurriculo.Value = string.Empty;
                hdnLinguaEstrangeiraFacultativa.Value = string.Empty;
                hdnEnsinoReligioso.Value = string.Empty;
                chkEnsReligioso.Enabled = false;
                chkEnsReligioso.Checked = false;
                chkLinguaEstrangeira.Enabled = false;
                chkLinguaEstrangeira.Checked = false;

                lblTipoCurso.Visible = false;
                ddlTipoCurso.Visible = false;
                ddlTipoCurso.ClearSelection();

                if (!tseCurso.DBValue.IsNull)
                {
                    tipo_curso = tseCurso["TIPO_CURSO"].ToString();
                    if (tipo_curso == "Concomitante/Subsequente")
                    {
                        lblTipoCurso.Visible = true;
                        ddlTipoCurso.Visible = true;
                    }
                }

                ddlTurno.Enabled = true;


                //CarregaDadosConfirmacao(true);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseCursoConcomitante_Changed(object sender, EventArgs args)
        {
            this.CarregarDadosDrop(ddlTurnoConcomitante.ID);
            this.CarregarDadosDrop(ddlSerieConcomitante.ID);
            this.CarregarDadosDrop(ddlTurmaConcomitante.ID, true);
        }

        protected void ddlSerie_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlConfirmacao.Visible = false;
            if (!string.IsNullOrEmpty(ddlSerie.SelectedValue))
            {
                this.CarregarDadosDrop(ddlTurmaDestino.ID, true);
            }
        }

        protected void ddlTurmaDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LyCurriculo curriculo = new LyCurriculo();
                RN.Curriculo rnCurriculo = new Techne.Lyceum.RN.Curriculo();
                TceConfirmacaoMatricula confirmacaoMatricula = new TceConfirmacaoMatricula();
                RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
                RN.ControleVaga rnControleVaga = new ControleVaga();
                RN.Curso rnCurso = new Curso();
                RN.Usuarios rnUsuarios = new Usuarios();
                RN.Perfil rnPerfil = new Perfil();

                pnlConfirmacao.Visible = false;
                pnlPermuta.Visible = false;

                hdnCurriculo.Value = string.Empty;
                hdnLinguaEstrangeiraFacultativa.Value = string.Empty;
                hdnEnsinoReligioso.Value = string.Empty;
                chkEnsReligioso.Enabled = false;
                chkEnsReligioso.Checked = false;
                chkLinguaEstrangeira.Enabled = false;
                chkLinguaEstrangeira.Checked = false;

                if (!string.IsNullOrEmpty(ddlTurmaDestino.SelectedValue))
                {
                    string aluno = tseAluno.Value.ToString();

                    //Busca Curriculo
                    curriculo = rnCurriculo.ObtemCurriculoPor(Convert.ToInt32(txtAnoLetivo.Text), Convert.ToInt32(ddlPeriodoDestino.SelectedValue), ddlTurno.SelectedValue, Convert.ToString(tseCurso.DBValue), txtUniEnsino.Text, ddlTurmaDestino.SelectedValue.Split('|')[1]);
                    hdnCurriculo.Value = curriculo.Curriculo;

                    //Busca opções da confirmaçao de matricula
                    confirmacaoMatricula = rnConfirmacaoMatricula.ObtemConfirmacaoAtivaPor(aluno, Convert.ToDecimal(txtAnoLetivo.Text), Convert.ToDecimal(txtPeriodoLetivo.Text), Convert.ToInt32(txtSerie.Text), Convert.ToString(txtTurno.Text), txtCurso.Text, Convert.ToString(txtUniEnsino.Text));
                    hdnEnsinoReligioso.Value = Convert.ToString(confirmacaoMatricula.EnsinoReligioso);
                    hdnLinguaEstrangeiraFacultativa.Value = Convert.ToString(confirmacaoMatricula.LinguaEstrangeiraFacultativa);

                    //Verifica se o curriculo permite ensino religioso
                    if (curriculo.EnsinoReligioso == "S")
                    {
                        chkEnsReligioso.Enabled = true;
                        chkEnsReligioso.Checked = Convert.ToBoolean(hdnEnsinoReligioso.Value);
                    }

                    //Verifica se o curriculo permite lingua estrangeira
                    if (curriculo.LinguaEstrangeira == "S")
                    {
                        chkLinguaEstrangeira.Enabled = true;
                        chkLinguaEstrangeira.Checked = Convert.ToBoolean(hdnLinguaEstrangeiraFacultativa.Value);
                    }

                    if (!txtAnoLetivo.Text.IsNullOrEmptyOrWhiteSpace() && !txtPeriodoLetivo.Text.IsNullOrEmptyOrWhiteSpace() && !txtCurso.Text.IsNullOrEmptyOrWhiteSpace() && !txtSerie.Text.IsNullOrEmptyOrWhiteSpace() && !txtTurno.Text.IsNullOrEmptyOrWhiteSpace())
                    {
                        if (rnControleVaga.PartipaMatriculaFacilPor(txtUniEnsino.Text, Convert.ToInt32(txtAnoLetivo.Text), Convert.ToInt32(txtPeriodoLetivo.Text), txtCurso.Text, Convert.ToInt32(txtSerie.Text), txtTurno.Text))
                        {
                            pnlConfirmacao.Visible = true;
                            LimparTelaPermuta();
                        }
                    }
                    else
                    {
                        lblMensagem.Text = "É necessário ter preenchido Ano/Período/Curso/Série/Turno.";
                        return;
                    }

                    btnTransferir.Enabled = true;

                    //Verifica se o curso atual é itineario
                    if (!rnCurso.EhItinerarioFormativoTrihaPor(tseCurso.DBValue.ToString()))
                    {
                        if (txtPeriodoLetivo.Text != ddlPeriodoDestino.SelectedValue || txtCurso.Text != tseCurso.DBValue.ToString() || txtTurno.Text != ddlTurno.SelectedValue || txtSerie.Text != ddlSerie.SelectedValue)
                        {
                            //Verifica se usuario possui padrao de acesso para alterar campos diferentes da turma
                            if (!rnUsuarios.EhPrivilegiado(User.Identity.Name) && !rnPerfil.PossuiPerfilTransferenciaTurmaTotalPor(User.Identity.Name))
                            {
                                if (rnControleVaga.PartipaMatriculaFacilPor(txtUniEnsino.Text, Convert.ToInt32(txtAnoLetivo.Text), Convert.ToInt32(ddlPeriodoDestino.SelectedValue), tseCurso.DBValue.ToString(), Convert.ToInt32(ddlSerie.SelectedValue), ddlTurno.SelectedValue))
                                {
                                    lblMensagem.Text = "Não será possível realizar a transferência, pois o curso/série está participando do Matrícula Fácil.";
                                    btnTransferir.Enabled = false;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlSerieConcomitante_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CarregarDadosDrop(ddlTurmaConcomitante.ID, true);
        }

        protected void ddlSerieMaisEducacaoDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlTurmaMaisEducacaoDestino.Items.Clear();
                Turma rnTurma = new Turma();

                if (!string.IsNullOrEmpty(ddlSerieMaisEducacaoDestino.SelectedValue))
                {
                    //Curso do Mais Educacao: 9999.92 
                    decimal serie = string.IsNullOrEmpty(ddlSerieMaisEducacaoDestino.SelectedValue) ? -1 : Convert.ToDecimal(ddlSerieMaisEducacaoDestino.SelectedValue);
                    var qt = rnTurma.ListaTurmasGradeComVagasPor(lblAnoMaisEducacao.Text, lblPeriodoMaisEducacao.Text, lblCensoMaisEducacao.Text, "9999.92", ddlTurnoMaisEducacaoDestino.SelectedValue, serie);
                    if (qt.Rows.Count > 0)
                    {
                        ddlTurmaMaisEducacaoDestino.DataSource = qt;
                        ddlTurmaMaisEducacaoDestino.DataBind();
                        ddlTurmaMaisEducacaoDestino.Items.Insert(0, new ListItem("<Selecione>", string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlSerieEducEspecialDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Turma rnTurma = new Turma();
                ddlTurmaEducEspecialDestino.Items.Clear();
                ddlHorarioEducEspecialDestino.Items.Clear();

                if (!string.IsNullOrEmpty(ddlSerieEducEspecialDestino.SelectedValue))
                {
                    decimal serie = string.IsNullOrEmpty(ddlSerieEducEspecialDestino.SelectedValue) ? -1 : Convert.ToDecimal(ddlSerieEducEspecialDestino.SelectedValue);
                    //curso do Educacao Especial: 9999.91 
                    var qt = rnTurma.ListaTurmasGradeComVagasPor(lblAnoEducEspecialOrigem.Text, lblPeriodoEducEspecialOrigem.Text, lblCensoEducEspecialOrigem.Text, "9999.91", ddlTurnoEducEspecialDestino.SelectedValue, serie);
                    if (qt.Rows.Count > 0)
                    {
                        ddlTurmaEducEspecialDestino.DataSource = qt;
                        ddlTurmaEducEspecialDestino.DataBind();
                        ddlTurmaEducEspecialDestino.Items.Insert(0, new ListItem("<Selecione>", string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlTurmaEducEspecialDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlHorarioEducEspecialDestino.Items.Clear();

                if (!string.IsNullOrEmpty(ddlTurmaEducEspecialDestino.SelectedValue))
                {
                    var qt = RN.Turma.ListarAtendimentosEducacaoEspecial(ddlTurmaEducEspecialDestino.SelectedValue, Convert.ToInt32(lblAnoEducEspecialOrigem.Text), Convert.ToInt32(lblPeriodoEducEspecialOrigem.Text));

                    if (qt.Rows.Count > 0)
                    {
                        ddlHorarioEducEspecialDestino.DataSource = qt;
                        ddlHorarioEducEspecialDestino.DataBind();
                        ddlHorarioEducEspecialDestino.Items.Insert(0, new ListItem("<Selecione>", string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlPeriodoDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                tseCurso.ResetValue();
                ddlTurno.Items.Clear();
                ddlSerie.Items.Clear();
                ddlTurmaDestino.Items.Clear();

                hdnCurriculo.Value = string.Empty;
                hdnLinguaEstrangeiraFacultativa.Value = string.Empty;
                hdnEnsinoReligioso.Value = string.Empty;
                pnlConfirmacao.Visible = false;
                chkEnsReligioso.Enabled = false;
                chkEnsReligioso.Checked = false;
                chkLinguaEstrangeira.Enabled = false;
                chkLinguaEstrangeira.Checked = false;
                CarregaCurso();
                //CarregaDadosConfirmacao(false);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        #endregion

        #region Metodos
        public static string GetUrl()
        {
            #region Código gerado Techne
            return Techne.Web.Navigation.GetNavigation(System.Reflection.MethodInfo.GetCurrentMethod()).GetUrl(new object[] { });
            #endregion
        }

        private void DesabilitarCamposMatriculaPrincipal()
        {
            //RN.Curso rnCurso = new Curso();
            //ddlTurno.Enabled = false;
            //ddlSerie.Enabled = false;

            ////Verifica se o curso atual é itineario
            //if (rnCurso.EhItinerarioFormativoTrihaPor(txtCurso.Text))
            //{
            //    tseCurso.Mode = ControlMode.Edit;
            //}
            //else
            //{
            //    tseCurso.Mode = ControlMode.View;
            //}            
        }

        private void CarregaDadosConfirmacao(bool trocaCurso)
        {

            RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            TceConfirmacaoMatricula confirmacaoMatricula = new TceConfirmacaoMatricula();
            RN.Turno rnTurno = new Turno();
            string aluno = tseAluno.DBValue.ToString();
            string censo = txtUniEnsino.Text;
            int ano = Convert.ToInt32(txtAnoLetivo.Text);
            int periodo = Convert.ToInt32(ddlPeriodoDestino.SelectedValue);

            //Busca confirmação de matricula confirmada para o ano / periodo selecionado
            confirmacaoMatricula = rnConfirmacaoMatricula.ObtemConfirmacaoAtivaPossiveisPeriodoPor(aluno, ano, periodo);

            //verifica se aluno possui confirmação de matricula confirmada para a escola
            if (confirmacaoMatricula.Censo == censo)
            {
                //Verifica se está trocando de curso
                if (!trocaCurso)
                {
                    tseCurso.DBValue = confirmacaoMatricula.Curso;
                    tseCurso_Changed(null, null);
                }

                ddlTurno.SelectedValue = confirmacaoMatricula.Turno;

                ddlSerie.Items.Clear();
                ddlSerie.Items.Add(confirmacaoMatricula.Serie.ToString());
                ddlSerie.SelectedValue = confirmacaoMatricula.Serie.ToString();
                ddlSerie_SelectedIndexChanged(null, null);
            }
            else
            {
                lblMensagem.Text = "Não existe confirmação de matrícula para este aluno nesta escola / ano / periodo.";
            }
        }

        private void ControlarTipoOperacao()
        {
            RN.Aluno rnAluno = new Aluno();

            switch (_tipoOperacao)
            {
                case TipoOperacao.Inicial:
                    {
                        btnTransferir.Visible = false;
                        btnVoltar.Visible = false;
                        pnDados.Visible = false;
                        pnGrid.Visible = false;
                        pnlTurmaConcomitante.Visible = false;
                        pnlTurmaConcomitanteOrigem.Visible = false;
                        pnlTurmaMaisEducacaoOrigem.Visible = false;
                        pnlTurmaMaisEducacaoDestino.Visible = false;
                        pnlTurmaEducacaoEspecialOrigem.Visible = false;
                        pnlTurmaEducacaoEspecialDestino.Visible = false;
                        pnlProgressao.Visible = false;
                        pnBotoesTransferencia.Visible = false;
                        btnSalvarConcomitante.Visible = false;
                        btnTransferirMaisEducacao.Visible = false;
                        btnTransferirEducEspecial.Visible = false;
                        pnlTurmaOptativaReforco.Visible = false;
                        tseAluno.ResetValue();

                        break;
                    }

                case TipoOperacao.Consultar:
                    {
                        DataTable dadosAluno = new DataTable();
                        lblMensagem.Text = string.Empty;

                        tseAluno.Enabled = true;
                        btnVoltar.Visible = true;
                        pnDados.Visible = true;

                        string aluno = Convert.ToString(tseAluno.DBValue);

                        dadosAluno = rnAluno.ObtemDadosAlunoPor(aluno);

                        if (dadosAluno.Rows.Count > 0)
                        {
                            this.PreencherDadosTela(dadosAluno);
                            pnDados.Visible = true;
                            pnGrid.Visible = true;
                            pnlProgressao.Visible = true;
                            if (!string.IsNullOrEmpty(txtUniEnsino.Text) && RN.Usuarios.VerificaAcesso(txtUniEnsino.Text, User.Identity.Name))
                            {
                                pnBotoesTransferencia.Visible = true;
                                btnTransferirMaisEducacao.Visible = true;
                                pnlProgressao.Visible = true;
                                pnlTurmaOptativaReforco.Visible = true;
                                btnTransferir.Visible = true;
                            }

                            if (!string.IsNullOrEmpty(lblUnidadeEnsinoOrigem.Text) && RN.Usuarios.VerificaAcesso(lblUnidadeEnsinoOrigem.Text, User.Identity.Name))
                            {
                                btnSalvarConcomitante.Visible = true;
                            }

                            if (!string.IsNullOrEmpty(lblCensoEducEspecialOrigem.Text) && RN.Usuarios.VerificaAcesso(lblCensoEducEspecialOrigem.Text, User.Identity.Name))
                            {
                                btnTransferirEducEspecial.Visible = true;
                            }
                        }
                        else
                        {
                            this.LimparTela();
                            pnDados.Visible = false;
                            pnGrid.Visible = false;
                            pnlTurmaConcomitante.Visible = false;
                            pnlTurmaConcomitanteOrigem.Visible = false;
                            pnlTurmaMaisEducacaoOrigem.Visible = false;
                            pnlTurmaMaisEducacaoDestino.Visible = false;
                            pnlTurmaEducacaoEspecialOrigem.Visible = false;
                            pnlTurmaEducacaoEspecialDestino.Visible = false;
                            pnlProgressao.Visible = false;
                            pnlTurmaOptativaReforco.Visible = false;
                            pnBotoesTransferencia.Visible = false;
                            lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                            var mensagem = lblMensagem.Text;
                            var script = @"alert('" + mensagem + @"');";
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);

                        }
                        break;


                    }
                case TipoOperacao.Sucesso:
                    {
                        DataTable dadosAluno = new DataTable();
                        tseAluno.Enabled = true;
                        btnVoltar.Visible = true;
                        pnDados.Visible = true;

                        string aluno = Convert.ToString(tseAluno.DBValue);

                        dadosAluno = rnAluno.ObtemDadosAlunoPor(aluno);

                        if (dadosAluno.Rows.Count > 0)
                        {
                            this.PreencherDadosTela(dadosAluno);
                            pnDados.Visible = true;
                            pnGrid.Visible = true;
                            tseCurso.ResetValue();

                            var mensagem = "Transferência(s) realizada com sucesso.";
                            var script = @"alert('" + mensagem + @"');";
                            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);

                            lblMensagem.Text = "Transferência(s) realizada com sucesso.";

                            //Limpar dados de transferencia
                            tseCurso.ResetValue();
                            ddlTurno.Items.Clear();
                            ddlSerie.Items.Clear();
                            ddlTurmaDestino.Items.Clear();
                            ddlMotivo.ClearSelection();

                            hdnCurriculo.Value = string.Empty;
                            hdnLinguaEstrangeiraFacultativa.Value = string.Empty;
                            hdnEnsinoReligioso.Value = string.Empty;
                            chkEnsReligioso.Enabled = false;
                            chkEnsReligioso.Checked = false;
                            chkLinguaEstrangeira.Enabled = false;
                            chkLinguaEstrangeira.Checked = false;

                            if (!string.IsNullOrEmpty(txtUniEnsino.Text) && RN.Usuarios.VerificaAcesso(txtUniEnsino.Text, User.Identity.Name))
                            {
                                pnBotoesTransferencia.Visible = true;
                                btnTransferirMaisEducacao.Visible = true;
                                pnlProgressao.Visible = true;
                                btnTransferir.Visible = true;
                            }
                            if (pnlTurmaConcomitante.Visible)
                            {
                                tseCursoConcomitante.ResetValue();
                                CarregaCursoConcomitante();
                                ddlTurmaConcomitante.Items.Clear();
                            }
                            if (!string.IsNullOrEmpty(lblUnidadeEnsinoOrigem.Text) && RN.Usuarios.VerificaAcesso(lblUnidadeEnsinoOrigem.Text, User.Identity.Name))
                            {
                                btnSalvarConcomitante.Visible = true;
                            }

                            if (!string.IsNullOrEmpty(lblCensoEducEspecialOrigem.Text) && RN.Usuarios.VerificaAcesso(lblCensoEducEspecialOrigem.Text, User.Identity.Name))
                            {
                                btnTransferirEducEspecial.Visible = true;
                            }


                            LimparTelaPermuta();
                            pnlConfirmacao.Visible = false;
                        }
                        else
                        {
                            this.LimparTela();
                            pnDados.Visible = false;
                            pnGrid.Visible = false;
                            pnlTurmaConcomitante.Visible = false;
                            pnlTurmaConcomitanteOrigem.Visible = false;
                            pnlTurmaMaisEducacaoOrigem.Visible = false;
                            pnlTurmaMaisEducacaoDestino.Visible = false;
                            pnlTurmaEducacaoEspecialOrigem.Visible = false;
                            pnlTurmaEducacaoEspecialDestino.Visible = false;
                            pnBotoesTransferencia.Visible = false;
                            btnSalvarConcomitante.Visible = false;
                            btnTransferirEducEspecial.Visible = false;
                            btnTransferirMaisEducacao.Visible = false;
                            pnlProgressao.Visible = false;
                            pnlTurmaOptativaReforco.Visible = false;
                            lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                        }
                        break;
                    }

            }
        }

        private void PreencherDadosTela(DataTable dadosAluno)
        {
            try
            {
                RN.Curso rnCurso = new Curso();
                RN.TransferenciaTurma rnTransferenciaTurma = new Techne.Lyceum.RN.TransferenciaTurma();
                DataTable dadosTurmaAtual = new DataTable();
                string aluno = tseAluno.Value.ToString();

                txtCurriculo.Text = Convert.ToString(dadosAluno.Rows[0]["curriculo"]);
                txtCurso.Text = Convert.ToString(dadosAluno.Rows[0]["curso"]);

                txtNomeCurso.Text = Convert.ToString(dadosAluno.Rows[0]["nome_curso"]);
                txtNomeSerie.Text = Convert.ToString(dadosAluno.Rows[0]["nome_serie"]);
                txtNomeTurno.Text = Convert.ToString(dadosAluno.Rows[0]["nome_turno"]);

                txtSerie.Text = Convert.ToString(dadosAluno.Rows[0]["serie"]);
                txtSituacao.Text = Convert.ToString(dadosAluno.Rows[0]["sit_aluno"]);
                dadosTurmaAtual = rnTransferenciaTurma.ObtemDadosTurmaAtualPor(aluno);

                if (dadosTurmaAtual.Rows.Count > 0)
                {
                    txtTurmaAtual.Text = Convert.ToString(dadosTurmaAtual.Rows[0]["TURMA"]);
                    txtAnoLetivo.Text = Convert.ToString(dadosTurmaAtual.Rows[0]["ANO"]);
                    txtPeriodoLetivo.Text = Convert.ToString(dadosTurmaAtual.Rows[0]["SEMESTRE"]);

                    bool existeTurmaConcomitante = RN.TransferenciaTurma.ExisteTurmaConcomitante(aluno, txtAnoLetivo.Text, txtPeriodoLetivo.Text);

                    pnlTurmaConcomitante.Visible = existeTurmaConcomitante;
                    pnlTurmaConcomitanteOrigem.Visible = existeTurmaConcomitante;

                    this.CarregaDadosConcomitante();
                    this.CarregarDadosMaisEducacao();
                    this.CarregarDadosEducacaoEspecial();
                }

                if (!string.IsNullOrEmpty(txtAnoLetivo.Text))
                {
                    var qt = RN.PeriodoLetivo.ListarPeriodo(txtAnoLetivo.Text);
                    if (qt.Rows.Count > 0)
                    {
                        ddlPeriodoDestino.DataSource = qt;
                        ddlPeriodoDestino.DataBind();
                        ddlPeriodoDestino.SelectedValue = txtPeriodoLetivo.Text;
                    }
                }

                txtTurno.Text = Convert.ToString(dadosAluno.Rows[0]["turno"]);
                txtUniEnsino.Text = Convert.ToString(dadosAluno.Rows[0]["uni_ensino"]);
                txtNomeUniEnsino.Text = Convert.ToString(dadosAluno.Rows[0]["nome_uni_ensino"]);
                txtCobran.Text = Convert.ToString(dadosAluno.Rows[0]["COBRAN_DISC"]);

                CarregaCurso();
                CarregaCursoConcomitante();
                this.CarregarDadosDrop(ddlMotivo.ID);

                //Remover esta condição caso não possa mais transferir 
                if (txtCobran.Text.ToUpper() == "S")
                {
                    //habilita campos para mudança de série
                    this.HabilitaSelecaoSerie(true);
                    this.CarregarDadosDrop(ddlTurno.ID);
                    this.CarregarDadosDrop(ddlSerie.ID);
                    this.CarregarDadosDrop(ddlTurmaDestino.ID, true);
                    this.CarregarDadosDrop(ddlTurnoConcomitante.ID);
                    this.CarregarDadosDrop(ddlSerieConcomitante.ID);
                    this.CarregarDadosDrop(ddlTurmaConcomitante.ID, true);
                }
                else
                {
                    this.HabilitaSelecaoSerie(false);
                }

                CarregaDadosConfirmacao(false);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregarDadosMaisEducacao()
        {
            try
            {
                RN.Turno rnTurno = new Turno();
                DataTable dtTurno = new DataTable();
                RN.DTOs.DadosMaisEducacao maisEducacao = new DadosMaisEducacao();

                maisEducacao = RN.Matricula.CarregarDadosMaisEducacao(Convert.ToString(tseAluno.DBValue),
                                                                              int.Parse(txtAnoLetivo.Text),
                                                                              int.Parse(txtPeriodoLetivo.Text), User.Identity.Name);
                pnlTurmaMaisEducacaoOrigem.Visible = maisEducacao.Enturmado;
                pnlTurmaMaisEducacaoDestino.Visible = maisEducacao.Enturmado;

                if (maisEducacao.Enturmado)
                {
                    lblCensoMaisEducacao.Text = maisEducacao.Censo;
                    lblEscolaMaisEducacao.Text = maisEducacao.NomeUnidadeEnsino;
                    lblAnoMaisEducacao.Text = Convert.ToString(maisEducacao.Ano);
                    lblPeriodoMaisEducacao.Text = Convert.ToString(maisEducacao.Periodo);
                    lblCursoMaisEducacaoOrigem.Text = maisEducacao.Curso;
                    lblNomeCursoMaisEducacaoOrigem.Text = maisEducacao.NomeCurso;
                    lblTurnoMaisEducacaoOrigem.Text = maisEducacao.NomeTurno;
                    lblSerieMaisEducacaoOrigem.Text = Convert.ToString(maisEducacao.Serie);
                    lblTurmaMaisEducacaoOrigem.Text = maisEducacao.Turma;

                    //Curso do Mais Educacao: 9999.92
                    dtTurno = rnTurno.ListaTurnoPor(lblCensoMaisEducacao.Text, "9999.92");
                    if (dtTurno.Rows.Count > 0)
                    {
                        ddlTurnoMaisEducacaoDestino.DataSource = dtTurno;
                        ddlTurnoMaisEducacaoDestino.DataBind();
                        ddlTurnoMaisEducacaoDestino.Items.Insert(0, new ListItem("<Selecione>", string.Empty));
                    }

                    ddlSerieMaisEducacaoDestino.Items.Clear();
                    ddlTurmaMaisEducacaoDestino.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregarDadosEducacaoEspecial()
        {
            try
            {
                RN.Turno rnTurno = new Turno();
                DataTable dtTurno = new DataTable();
                RN.DTOs.DadosEducacaoEspecial educacaoEspecial = new DadosEducacaoEspecial();

                educacaoEspecial = RN.Matricula.CarregarDadosEducacaoEspecial(Convert.ToString(tseAluno.DBValue),
                                                                              int.Parse(txtAnoLetivo.Text),
                                                                              int.Parse(txtPeriodoLetivo.Text), User.Identity.Name);
                pnlTurmaEducacaoEspecialOrigem.Visible = educacaoEspecial.Enturmado;
                pnlTurmaEducacaoEspecialDestino.Visible = educacaoEspecial.Enturmado;

                if (educacaoEspecial.Enturmado)
                {
                    lblCensoEducEspecialOrigem.Text = educacaoEspecial.Censo;
                    lblEscolaEducEspecialOrigem.Text = educacaoEspecial.NomeUnidadeEnsino;
                    lblAnoEducEspecialOrigem.Text = Convert.ToString(educacaoEspecial.Ano);
                    lblPeriodoEducEspecialOrigem.Text = Convert.ToString(educacaoEspecial.Periodo);
                    lblCursoEducEspecialOrigem.Text = educacaoEspecial.Curso;
                    lblNomeCursoEspecialOrigem.Text = educacaoEspecial.NomeCurso;
                    lblTurnoEducEspecialOrigem.Text = educacaoEspecial.NomeTurno;
                    lblSerieEducEspecialOrigem.Text = Convert.ToString(educacaoEspecial.Serie);
                    lblTurmaEducEspecialOrigem.Text = educacaoEspecial.Turma;

                    lblHorariosOrigem.Items.Clear();
                    foreach (var item in educacaoEspecial.Atendimentos)
                    {
                        lblHorariosOrigem.Items.Add(new ListItem(item.Horario, item.Disciplina));
                    }

                    //Curso do Educacao Especial: 9999.91
                    dtTurno = rnTurno.ListaTurnoEducEspecialPor(lblCensoEducEspecialOrigem.Text, "9999.91");
                    if (dtTurno.Rows.Count > 0)
                    {
                        ddlTurnoEducEspecialDestino.DataSource = dtTurno;
                        ddlTurnoEducEspecialDestino.DataBind();
                        ddlTurnoEducEspecialDestino.Items.Insert(0, new ListItem("<Selecione>", string.Empty));
                    }

                    ddlSerieEducEspecialDestino.Items.Clear();
                    ddlTurmaEducEspecialDestino.Items.Clear();
                    ddlHorarioEducEspecialDestino.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void HabilitaSelecaoSerie(bool habilita)
        {
            lblDDLTurno.Visible = habilita;
            lblDDLSerie.Visible = habilita;
            lblCursotse.Visible = habilita;
            ddlTurno.Visible = habilita;
            ddlSerie.Visible = habilita;
            tseCurso.Visible = habilita;
            tseCursoConcomitante.Visible = habilita;
            lblPeriodoDestino.Visible = habilita;
            ddlPeriodoDestino.Visible = habilita;
            lblDDLSerieConcomitante.Visible = habilita;
            ddlSerieConcomitante.Visible = habilita;
            lblDDLTurnoConcomitante.Visible = habilita;
            ddlTurnoConcomitante.Visible = habilita;
        }

        protected void LimparTela()
        {
            txtAnoLetivo.Text = string.Empty;
            txtCurriculo.Text = string.Empty;
            txtCurso.Text = string.Empty;
            txtNomeCurso.Text = string.Empty;
            txtNomeSerie.Text = string.Empty;
            txtNomeTurno.Text = string.Empty;
            txtPeriodoLetivo.Text = string.Empty;
            txtSerie.Text = string.Empty;
            txtSituacao.Text = string.Empty;
            txtTurmaAtual.Text = string.Empty;
            txtTurno.Text = string.Empty;
            txtUniEnsino.Text = string.Empty;
            txtNomeUniEnsino.Text = string.Empty;
            hdnAnoLetivo.Value = string.Empty;
            hdnPeriodoLetivo.Value = string.Empty;

        }

        private RN.DTOs.DadosTransferencia ObterDadosTela()
        {
            RN.DTOs.DadosTransferencia dadosTransferencia = null;
            RN.GradeTurma rnGradeTurma = new GradeTurma();
            RN.GradeSerie rnGradeSerie = new GradeSerie();

            if (tseAluno.IsValidDBValue)
            {
                dadosTransferencia = new RN.DTOs.DadosTransferencia();
                dadosTransferencia.Aluno = Convert.ToString(tseAluno.Value);
                dadosTransferencia.Ano = txtAnoLetivo.Text;

                dadosTransferencia.UsuarioResponsavel = User.Identity.Name;
                dadosTransferencia.NecessidadeEspecial = tseAluno["necessidade_especial"].ToString();
                dadosTransferencia.DataNascimento = (!tseAluno.DBValue.IsNull && tseAluno.IsValidDBValue) ? Convert.ToDateTime(tseAluno["dt_nascimento"]) : DateTime.MinValue;
                dadosTransferencia.TurnoAtual = txtTurno.Text;
                dadosTransferencia.CursoAtual = txtCurso.Text;
                dadosTransferencia.SerieAtual = txtSerie.Text;

                dadosTransferencia.GradeIdDestino = !ddlTurmaDestino.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? ddlTurmaDestino.SelectedValue.Split('|')[0] : null;
                dadosTransferencia.MotivoTransferencia = ddlMotivo.SelectedValue;

                dadosTransferencia.SemestreAtual = txtPeriodoLetivo.Text;
                dadosTransferencia.TurmaAtual = txtTurmaAtual.Text;
                dadosTransferencia.TurmaDestino = !ddlTurmaDestino.SelectedValue.IsNullOrEmptyOrWhiteSpace() ? RN.TransferenciaTurma.ConsultarGrade(ddlTurmaDestino.SelectedValue.Split('|')[0]) : null;

                dadosTransferencia.SemestreDestino = ddlPeriodoDestino.SelectedValue;
                dadosTransferencia.GradeIdAtual = rnGradeSerie.ObtemGradeIdPor(Convert.ToDecimal(dadosTransferencia.Ano), Convert.ToDecimal(dadosTransferencia.SemestreAtual), dadosTransferencia.CursoAtual, Convert.ToDecimal(dadosTransferencia.SerieAtual), dadosTransferencia.TurmaAtual);


                if (txtCobran.Text == "S")
                {
                    dadosTransferencia.TurnoDestino = ddlTurno.SelectedValue;
                    dadosTransferencia.SerieDestino = ddlSerie.SelectedValue;
                    dadosTransferencia.CursoDestino = (!tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue) ? Convert.ToString(tseCurso.DBValue) : null;
                    dadosTransferencia.TipoCursoDestino = (!tseCurso.DBValue.IsNull && tseCurso.IsValidDBValue) ? tseCurso["tipo_curso"].ToString() : null;
                    dadosTransferencia.CurriculoDestino = hdnCurriculo.Value;
                    dadosTransferencia.EnsinoReligioso = Convert.ToString(chkEnsReligioso.Checked);
                    dadosTransferencia.LinguaEstrangeira = Convert.ToString(chkLinguaEstrangeira.Checked);
                }

                if (!string.IsNullOrEmpty(ddlTipoCurso.SelectedValue))
                {
                    dadosTransferencia.TipoEnsProfissionalizanteDestino = ddlTipoCurso.SelectedValue;
                }
                else
                {
                    dadosTransferencia.TipoEnsProfissionalizanteDestino = string.Empty;
                }

                dadosTransferencia.UnidadeEnsino = txtUniEnsino.Text;
                dadosTransferencia.UnidadeFisica = txtUniEnsino.Text;
                dadosTransferencia.SituacaoAluno = txtSituacao.Text;
                dadosTransferencia.PossuiTurmaConcomitante = pnlTurmaConcomitante.Visible;
                if (!string.IsNullOrEmpty(lblTurnoConcomitanteOrigem.Text))
                {
                    dadosTransferencia.TurnoAtualTurmaConcomitante = lblTurnoConcomitanteOrigem.Text.Substring(0, 1);
                }
                else
                {
                    dadosTransferencia.TurnoAtualTurmaConcomitante = string.Empty;
                }

                dadosTransferencia.ListaDisciplinasTurmaDestino = new List<string>();

                if (!txtAnoLetivo.Text.IsNullOrEmptyOrWhiteSpace() && !ddlPeriodoDestino.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlTurmaDestino.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    dadosTransferencia.ListaDisciplinasTurmaDestino = rnGradeTurma.ListaDisciplinaMatriculaRegular(Convert.ToDecimal(txtAnoLetivo.Text), Convert.ToDecimal(ddlPeriodoDestino.SelectedValue), ddlTurmaDestino.SelectedValue.Split('|')[1], ddlTurmaDestino.SelectedValue.Split('|')[0]);
                }
                else
                {
                    dadosTransferencia.ListaDisciplinasTurmaDestino = null;
                }
            }

            return dadosTransferencia;
        }

        #region COMBOS
        private void CarregarDropDownList(DropDownList drop, QueryTable data, string defaultValue)
        {
            drop.DataSource = data;
            drop.DataBind();

            if (drop.Items.Count == 0)
            {
                var itemVazio = new ListItem("<Lista Vazia>", string.Empty);
                drop.Items.Add(itemVazio);
            }
            else
            {
                try
                {
                    if (!string.IsNullOrEmpty(defaultValue))
                    {
                        drop.SelectedValue = defaultValue;
                    }
                    else
                    {
                        var itemNulo = new ListItem("<Nenhum>", string.Empty);
                        drop.Items.Add(itemNulo);
                        drop.SelectedValue = string.Empty;
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    drop.ClearSelection();
                }
            }
        }

        private QueryTable CarregarDadosDrop(string idDrop, params bool[] diferenciado)
        {
            QueryTable dadosDrop = null;

            try
            {
                switch (idDrop.ToUpper())
                {
                    case "DDLMOTIVO":
                        {
                            RN.TransferenciaTurma rnTransferenciaTurma = new Techne.Lyceum.RN.TransferenciaTurma();
                            ddlMotivo.DataSource = rnTransferenciaTurma.ListaMotivoTransferencia();
                            ddlMotivo.DataBind();
                            ddlMotivo.Items.Insert(0, new ListItem("<Nenhum>", string.Empty));

                            break;
                        }
                    case "DDLTURMADESTINO":
                        {
                            Turma rnTurma = new Turma();

                            if (diferenciado.Length > 0 && diferenciado[0])
                            {
                                decimal serie = string.IsNullOrEmpty(ddlSerie.SelectedValue) ? -1 : Convert.ToDecimal(ddlSerie.SelectedValue);

                                if (!txtAnoLetivo.Text.IsNullOrEmptyOrWhiteSpace() && !ddlPeriodoDestino.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !txtUniEnsino.Text.IsNullOrEmptyOrWhiteSpace() && tseCurso.Value != null && !ddlTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace() && serie != 0)
                                {
                                    ddlTurmaDestino.DataSource = rnTurma.ListaTurmasGradeComVagasPor(txtAnoLetivo.Text, ddlPeriodoDestino.SelectedValue, txtUniEnsino.Text, Convert.ToString(tseCurso.DBValue), ddlTurno.SelectedValue, serie);
                                    //ddlTurmaDestino.DataSource = rnTurma.PrimeiraTurmasGradeComVagasDiferentePor(txtAnoLetivo.Text, ddlPeriodoDestino.SelectedValue, txtUniEnsino.Text, Convert.ToString(tseCurso.DBValue), ddlTurno.SelectedValue, serie, txtTurmaAtual.Text);
                                    ddlTurmaDestino.DataBind();
                                    ddlTurmaDestino.Items.Insert(0, new ListItem("<Nenhum>", string.Empty));
                                }
                            }
                            else
                            {
                                ddlTurmaDestino.DataSource = rnTurma.ListaTurmasGradeComVagasPor(txtAnoLetivo.Text, ddlPeriodoDestino.SelectedValue, txtUniEnsino.Text);
                                //ddlTurmaDestino.DataSource = rnTurma.PrimeiraTurmasGradeComVagasDiferentePor(txtAnoLetivo.Text, ddlPeriodoDestino.SelectedValue, txtUniEnsino.Text, txtTurmaAtual.Text);
                                ddlTurmaDestino.DataBind();
                                ddlTurmaDestino.Items.Insert(0, new ListItem("<Nenhum>", string.Empty));
                            }

                            break;
                        }
                    case "DDLTURNO":
                        {
                            RN.Turno rnTurno = new Turno();
                            if (!tseCurso.DBValue.IsNull && !txtUniEnsino.Text.IsNullOrEmptyOrWhiteSpace())
                            {
                                ddlTurno.DataSource = rnTurno.ListaTurnoPor(txtUniEnsino.Text, Convert.ToString(tseCurso.DBValue));
                                ddlTurno.DataBind();
                            }
                            ddlTurno.Items.Insert(0, new ListItem("<Nenhum>", string.Empty));
                            break;
                        }

                    case "DDLSERIE":
                        {
                            RN.Serie rnSerie = new Serie();
                            DataTable series = new DataTable();
                            series.Columns.Add("SERIE", typeof(string));

                            //Caso esteja indo do fundamental para o aprendendo a aprender vai para serie 1
                            if ((txtCurso.Text == "0001.21" || txtCurso.Text == "2024.19" || txtCurso.Text == "0001.26" || txtCurso.Text == "0001.22")
                                && (txtSerie.Text == "6" || txtSerie.Text == "7") && Convert.ToString(tseCurso.DBValue) == "2024.20")
                            {
                                series.Rows.Add(1);
                            }
                            else
                            {
                                //Permite transferencia para a propria série
                                series.Rows.Add(Convert.ToInt32(txtSerie.Text));
                            }

                            //if (!tseCurso.DBValue.IsNull && !ddlTurno.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                            //{
                            //    ddlSerie.DataSource = rnSerie.ListaSeriePor(Convert.ToString(tseCurso.DBValue), ddlTurno.SelectedValue);
                            //    ddlSerie.DataBind();
                            //}

                            ddlSerie.DataSource = series;
                            ddlSerie.DataBind();
                            ddlSerie.Items.Insert(0, new ListItem("<Nenhum>", string.Empty));

                            break;
                        }

                    case "DDLTURMACONCOMITANTE":
                        {
                            Turma rnTurma = new Turma();

                            if (diferenciado.Length > 0 && diferenciado[0])
                            {
                                decimal serie = string.IsNullOrEmpty(ddlSerieConcomitante.SelectedValue) ? -1 : Convert.ToDecimal(ddlSerieConcomitante.SelectedValue);

                                ddlTurmaConcomitante.DataSource = rnTurma.ListaTurmasGradeComVagasPor(txtAnoLetivo.Text, lblPeriodoConcomitanteOrigem.Text, lblUnidadeEnsinoOrigem.Text, Convert.ToString(tseCursoConcomitante.DBValue), ddlTurnoConcomitante.SelectedValue, serie);
                                ddlTurmaConcomitante.DataBind();
                                ddlTurmaConcomitante.Items.Insert(0, new ListItem("<Nenhum>", string.Empty));

                            }
                            else
                            {
                                ddlTurmaConcomitante.DataSource = rnTurma.ListaTurmasGradeComVagasPor(txtAnoLetivo.Text, lblPeriodoConcomitanteOrigem.Text, lblUnidadeEnsinoOrigem.Text);
                                ddlTurmaConcomitante.DataBind();
                                ddlTurmaConcomitante.Items.Insert(0, new ListItem("<Nenhum>", string.Empty));
                            }

                            break;
                        }
                    case "DDLTURNOCONCOMITANTE":
                        {

                            RN.Turno rnTurno = new Turno();

                            ddlTurnoConcomitante.DataSource = rnTurno.ListaTurnoPor(lblUnidadeEnsinoOrigem.Text, Convert.ToString(tseCursoConcomitante.DBValue));
                            ddlTurnoConcomitante.DataBind();
                            ddlTurnoConcomitante.Items.Insert(0, new ListItem("<Nenhum>", string.Empty));
                            break;
                        }

                    case "DDLSERIECONCOMITANTE":
                        {

                            RN.Serie rnSerie = new Serie();

                            ddlSerieConcomitante.DataSource = rnSerie.ListaSeriePor(Convert.ToString(tseCursoConcomitante.DBValue), ddlTurnoConcomitante.SelectedValue);
                            ddlSerieConcomitante.DataBind();
                            ddlSerieConcomitante.Items.Insert(0, new ListItem("<Nenhum>", string.Empty));

                            break;
                        }
                    case "DDLMOTIVOPERMUTA":
                        {
                            RN.TransferenciaTurma rnTransferenciaTurma = new Techne.Lyceum.RN.TransferenciaTurma();
                            ddlMotivoPermuta.DataSource = rnTransferenciaTurma.ListaMotivoTransferencia();
                            ddlMotivoPermuta.DataBind();
                            ddlMotivoPermuta.Items.Insert(0, new ListItem("Selecione", string.Empty));

                            break;
                        }
                }
            }
            catch
            {
                throw;
            }

            return dadosDrop;
        }
        #endregion

        protected void ddlTurno_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CarregarDadosDrop(ddlSerie.ID);
            ddlTurmaDestino.Items.Clear();
            var itemVazio = new ListItem("<Lista Vazia>", string.Empty);
            ddlTurmaDestino.Items.Add(itemVazio);
            pnlConfirmacao.Visible = false;
            hdnCurriculo.Value = string.Empty;
            hdnLinguaEstrangeiraFacultativa.Value = string.Empty;
            hdnEnsinoReligioso.Value = string.Empty;
            chkEnsReligioso.Enabled = false;
            chkEnsReligioso.Checked = false;
            chkLinguaEstrangeira.Enabled = false;
            chkLinguaEstrangeira.Checked = false;
            ddlSerie.Enabled = true;
        }

        protected void ddlTurnoConcomitante_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CarregarDadosDrop(ddlSerieConcomitante.ID);
            this.CarregarDadosDrop(ddlTurmaConcomitante.ID, true);
        }

        protected void ddlTurnoMaisEducacaoDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Serie rnSerie = new Serie();
                DataTable dtSerie = new DataTable();


                ddlSerieMaisEducacaoDestino.Items.Clear();
                ddlTurmaMaisEducacaoDestino.Items.Clear();

                if (!string.IsNullOrEmpty(ddlTurnoMaisEducacaoDestino.SelectedValue))
                {
                    //Curso do Mais Educacao: 9999.92
                    dtSerie = rnSerie.ListaSeriePor("9999.92", ddlTurnoMaisEducacaoDestino.SelectedValue);

                    if (dtSerie.Rows.Count > 0)
                    {
                        ddlSerieMaisEducacaoDestino.DataSource = dtSerie;
                        ddlSerieMaisEducacaoDestino.DataBind();
                        ddlSerieMaisEducacaoDestino.Items.Insert(0, new ListItem("<Selecione>", string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void ddlTurnoEducEspecialDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Serie rnSerie = new Serie();
                DataTable dtSerie = new DataTable();

                ddlSerieEducEspecialDestino.Items.Clear();
                ddlTurmaEducEspecialDestino.Items.Clear();
                ddlHorarioEducEspecialDestino.Items.Clear();

                if (!string.IsNullOrEmpty(ddlTurnoEducEspecialDestino.SelectedValue))
                {
                    //curso do Educacao Especial: 9999.91
                    dtSerie = rnSerie.ListaSeriePor("9999.91", ddlTurnoEducEspecialDestino.SelectedValue);

                    if (dtSerie.Rows.Count > 0)
                    {
                        ddlSerieEducEspecialDestino.DataSource = dtSerie;
                        ddlSerieEducEspecialDestino.DataBind();
                        ddlSerieEducEspecialDestino.Items.Insert(0, new ListItem("<Selecione>", string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        #endregion

        public object ListarProgressao(object aluno, object ano)
        {
            var alu = aluno.ToString();

            if (!string.IsNullOrEmpty(alu))
            {
                return RN.Matricula.ListarProgressaoParcial(alu);
            }

            return null;
        }

        protected void grdProgressao_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdProgressao.Settings.ShowFilterRow = false;
        }
        protected void grdProgressao_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdProgressao.Settings.ShowFilterRow = false;
        }

        public void UPDATE(object ANO, object SEMESTRE, object DSC_TURNO, object TURMA, object DSC_DISCIPLINA, object SERIE_REFERENCIA, object DSC_DISCIPLINA_REFERENCIA, object NOVATURMA, object ALUNO, object DISCIPLINA)
        {
        }

        protected void odsProgressao_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            var disciplina_ref = e.InputParameters["DSC_DISCIPLINA_REFERENCIA"].ToString().Split('|');

            var matricula = new LyMatricula
            {
                Aluno = e.InputParameters["ALUNO"].ToString(),
                Disciplina = e.InputParameters["DISCIPLINA"].ToString(),
                Turma = e.InputParameters["TURMA"].ToString(),
                Ano = Convert.ToDecimal(e.InputParameters["ANO"]),
                Semestre = Convert.ToDecimal(e.InputParameters["SEMESTRE"]),
                SerieReferencia = Convert.ToDecimal(e.InputParameters["SERIE_REFERENCIA"]),
                DisciplinaReferencia = disciplina_ref[0].TrimEnd(),
                Matricula = this.User.Identity.Name
            };

            var turmaDestino = e.InputParameters["NOVATURMA"].ToString();

            var validacao = RN.TransferenciaTurma.ValidarTransferirProgressaoParcial(matricula, turmaDestino);

            if (validacao.Valido)
            {
                RN.TransferenciaTurma.TransferirProgressaoParcial(matricula, turmaDestino);
            }
            else
            {
                throw new Exception(validacao.Mensagem);
            }
        }

        protected void grdProgressao_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdProgressao);
        }

        protected void grdProgressao_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdProgressao.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "ALUNO")
                {
                    e.Editor.Enabled = true;
                }

                if ((e.Column.FieldName) == "DISCIPLINA_REFERENCIA")
                {
                    e.Editor.Enabled = true;
                    e.Editor.ReadOnly = true;
                }
            }
            else if (grdProgressao.IsEditing)
            {
                if ((e.Column.FieldName) == "ALUNO")
                {
                    e.Editor.Enabled = false;
                }

                if ((e.Column.FieldName) == "DISCIPLINA_REFERENCIA")
                {
                    e.Editor.Enabled = false;
                }
            }

            if (this.grdProgressao.IsEditing
               && e.Column.FieldName == "NOVATURMA"
               && e.KeyValue != DBNull.Value
               && e.KeyValue != null)
            {
                var valAno = this.grdProgressao.GetRowValuesByKeyValue(e.KeyValue, "ANO");
                var valSemestre = this.grdProgressao.GetRowValuesByKeyValue(e.KeyValue, "SEMESTRE");

                if (valAno == DBNull.Value)
                {
                    return;
                }

                var combo = e.Editor as ASPxComboBox;

                if (combo == null)
                {
                    return;
                }

                this.ConsultarTurmaDestino(combo, txtUniEnsino.Text, valAno.ToString(), valSemestre.ToString());
            }
        }

        private void ConsultarTurmaDestino(ASPxComboBox cmbTurmaDestino, string unidade, string ano, string semestre)
        {
            if (unidade == null && ano == null && semestre == null)
            {
                return;
            }

            cmbTurmaDestino.Items.Clear();
            cmbTurmaDestino.TextField = "TURMA";
            cmbTurmaDestino.ValueField = "TURMA";
            cmbTurmaDestino.DataSource = Turma.ListarPorTurmaUE(Convert.ToString(unidade), Convert.ToInt32(ano), Convert.ToInt32(semestre));
            cmbTurmaDestino.DataBind();
        }

        public object ListaMatriculaOptativaReforcoPor(object aluno)
        {
            string alunoMatriculado = aluno.ToString();

            RN.Matricula matricula = new RN.Matricula();
            DataTable dt = new DataTable();

            if (!string.IsNullOrEmpty(alunoMatriculado))
                dt = matricula.ListaMatriculaAtivaOptativaReforcoPor(alunoMatriculado);

            return dt;
        }

        protected void grdOptativaReforco_StartRowEditing(object sender, DevExpress.Web.Data.ASPxStartRowEditingEventArgs e)
        {
            grdOptativaReforco.Settings.ShowFilterRow = false;
        }

        protected void grdOptativaReforco_InitNewRow(object sender, DevExpress.Web.Data.ASPxDataInitNewRowEventArgs e)
        {
            grdOptativaReforco.Settings.ShowFilterRow = false;
        }

        public void AtualizaTurmaDaMatriculaOptativaReforco(object ALUNO, object ANO, object SEMESTRE, object TURMA, object NOME, object DT_MATRICULA, object NOVATURMA)
        {
        }

        protected void odsOptativaReforco_Updating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            RN.TransferenciaTurma transferenciaTurma = new RN.TransferenciaTurma();

            var matricula = new LyMatricula
            {
                Aluno = e.InputParameters["ALUNO"].ToString(),
                Ano = Convert.ToDecimal(e.InputParameters["ANO"]),
                Semestre = Convert.ToDecimal(e.InputParameters["SEMESTRE"]),
                Turma = e.InputParameters["TURMA"].ToString(),
                Matricula = this.User.Identity.Name
            };

            var turmaDestino = Convert.ToString(e.InputParameters["NOVATURMA"]);

            var validacao = transferenciaTurma.ValidaTransferenciaTurmaOptativaReforco(matricula, turmaDestino);

            if (validacao.Valido)
                transferenciaTurma.TransfereTurmaOptativaReforco(matricula, turmaDestino);
            else
                throw new Exception(validacao.Mensagem);
        }

        protected void grdOptativaReforco_AfterPerformCallback(object sender, ASPxGridViewAfterPerformCallbackEventArgs e)
        {
            ControlaAcesso(grdOptativaReforco);
        }

        protected void grdOptativaReforco_CellEditorInitialize(object sender, DevExpress.Web.ASPxGridView.ASPxGridViewEditorEventArgs e)
        {
            if (grdOptativaReforco.IsNewRowEditing)
            {
                if ((e.Column.FieldName) == "ALUNO")
                {
                    e.Editor.Enabled = true;
                }
            }
            else if (grdOptativaReforco.IsEditing)
            {
                if ((e.Column.FieldName) == "ALUNO")
                {
                    e.Editor.Enabled = false;
                }
            }

            if (this.grdOptativaReforco.IsEditing
               && e.Column.FieldName == "NOVATURMA"
               && e.KeyValue != DBNull.Value
               && e.KeyValue != null)
            {
                var valAno = this.grdOptativaReforco.GetRowValuesByKeyValue(e.KeyValue, "ANO");
                var valSemestre = this.grdOptativaReforco.GetRowValuesByKeyValue(e.KeyValue, "SEMESTRE");
                var valTurma = this.grdOptativaReforco.GetRowValuesByKeyValue(e.KeyValue, "TURMA");
                var valAluno = this.grdOptativaReforco.GetRowValuesByKeyValue(e.KeyValue, "ALUNO");

                if (valAno == DBNull.Value)
                {
                    return;
                }

                var combo = e.Editor as ASPxComboBox;

                if (combo == null)
                {
                    return;
                }

                this.ConsultaTurmaOptativaReforcoDestino(combo, txtUniEnsino.Text, int.Parse(valAno.ToString()), int.Parse(valSemestre.ToString()), valTurma.ToString(), valAluno.ToString());
            }
        }

        private void ConsultaTurmaOptativaReforcoDestino(ASPxComboBox cmbTurmaOptativaReforcoDestino, string unidade, int ano, int semestre, string turma, string aluno)
        {
            RN.Turma turmaRn = new RN.Turma();

            if (unidade != null && turma != null)
            {
                cmbTurmaOptativaReforcoDestino.Items.Clear();
                cmbTurmaOptativaReforcoDestino.TextField = "TURMA";
                cmbTurmaOptativaReforcoDestino.ValueField = "TURMA";
                cmbTurmaOptativaReforcoDestino.DataSource = turmaRn.ListaTurmaParaTransferenciaAbertaComVagaOptativaReforcoPor(ano, semestre, unidade, turma, aluno);
                cmbTurmaOptativaReforcoDestino.DataBind();
            }
        }

        private void CarregaDadosConcomitante()
        {
            RN.DTOs.DadosEnsProfConcomitante concomitante = RN.Matricula.CarregarEnsProfConcomitante(Convert.ToString(tseAluno.DBValue), int.Parse(txtAnoLetivo.Text),
                                                     int.Parse(txtPeriodoLetivo.Text), User.Identity.Name);

            lblUnidadeEnsinoOrigem.Text = concomitante.Censo;
            lblNomeUnidadeEnsinoOrigem.Text = concomitante.NomeUnidadeEnsino;
            lblAnoConcomitanteOrigem.Text = Convert.ToString(concomitante.Ano);
            lblPeriodoConcomitanteOrigem.Text = Convert.ToString(concomitante.Periodo);
            lblCursoConcomitanteOrigem.Text = concomitante.Curso;
            lblTurnoConcomitanteOrigem.Text = concomitante.NomeTurno;
            lblAnoEscolaridadeConcomitanteOrigem.Text = Convert.ToString(concomitante.Serie);
            lblTurmaConcomitanteOrigem.Text = concomitante.Turma;
        }

        private void CarregaCurso()
        {
            RN.Curso rnCurso = new Curso();
            string table = string.Empty;
            Techne.Library.Sql.Structure.SqlSelectColumns coluna = new Techne.Library.Sql.Structure.SqlSelectColumns();
            Techne.Library.Sql.Structure.SqlSelect sqlSelect;
            coluna.Add("uec.curso");
            coluna.Add("nome");
            coluna.Add("tipo_curso");
            sqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);
            tseCurso.SqlSelect = sqlSelect;

            if (!string.IsNullOrEmpty(ddlPeriodoDestino.SelectedValue))
            {
                table = @" LY_CURSO c 
                           INNER JOIN LY_UNIDADE_ENSINO_CURSOS uec on (c.CURSO = uec.CURSO)
                           INNER JOIN TCE_CONTROLE_VAGA V ON uec.unidade_ens = V.CENSO AND V.CURSO = uec.CURSO ";

                sqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);
                tseCurso.SqlSelect = sqlSelect;

                //Verifica se o curso atual é itineario
                if (rnCurso.EhItinerarioFormativoTrihaPor(txtCurso.Text))
                {
                    //Busa dados do curso atual
                    RN.DTOs.DadosCurso dadosCurso = rnCurso.ObtemDadosCursoPor(txtCurso.Text);

                    //Lista cursos de itinerarios da mesma modalidade / tipo (menos trilha integrado que não pode ser escolhida por alunos) e cursos que não participam da matricula facil
                    tseCurso.SqlWhere = string.Format(@" uec.unidade_ens = '{0}'
                            AND V.ANO = {1}
                            AND V.PERIODO = {2}
                            AND ( (c.ITINERARIOFORMATIVO IS NOT NULL 
	                             AND c.ITINERARIOFORMATIVO = 'S' 
	                             AND c.TRILHAAPRENDIZAGEMID IS NOT NULL 
	                             AND c.MODALIDADE = '{3}' 
	                             AND c.TIPO = {4}
	                             and c.TRILHAAPRENDIZAGEMID <> 31 )
                            OR V.PARTICIPAMATRICULAFACIL = 0 )", txtUniEnsino.Text, txtAnoLetivo.Text, ddlPeriodoDestino.SelectedValue, dadosCurso.Modalidade, dadosCurso.Tipo);
                }
                else
                {
                    //Verifica se a matricula é do 6° e 7º ano do:ENSINO FUNDAMENTAL ANOS FINAIS (0001.21) / ENSINO FUNDAMENTAL 
                    //INTEGRAL COM ÊNFASE EM INOVAÇÃO (2024.190 / ENSINO FUNDAMENTAL COM ÊNFASE EM TECNOLOGIA, SUSTENTABILIDADE, ARTE E ESPORTE (0001.26) / 
                    //EDUCAÇÃO INTEGRAL ENSINO FUNDAMENTAL (0001.22)
                    if ((txtCurso.Text == "0001.21" || txtCurso.Text == "2024.19" || txtCurso.Text == "0001.26" || txtCurso.Text == "0001.22")
                        && (txtSerie.Text == "6" || txtSerie.Text == "7"))
                    {
                        //Adiciona Aprendendo a Aprender (2024.20) e curso da confirmacao e cursos que não participam da matricula facil  
                        tseCurso.SqlWhere = string.Format(@" uec.unidade_ens = '{0}'
                            AND V.ANO = {1}
                            AND V.PERIODO = {2}
                            AND (c.CURSO = '{3}' or c.CURSO = '2024.20' OR V.PARTICIPAMATRICULAFACIL = 0)", txtUniEnsino.Text, txtAnoLetivo.Text, ddlPeriodoDestino.SelectedValue, txtCurso.Text);
                    }
                    else
                    {
                        //Usa curso da confirmacao e cursos que não participam da matricula facil
                        tseCurso.SqlWhere = string.Format(@" uec.unidade_ens = '{0}'
                            AND V.ANO = {1}
                            AND V.PERIODO = {2}
                            AND (c.CURSO = '{3}' OR V.PARTICIPAMATRICULAFACIL = 0)", txtUniEnsino.Text, txtAnoLetivo.Text, ddlPeriodoDestino.SelectedValue, txtCurso.Text);
                    }
                }

                tseCurso.DataBind();
            }
        }
        private void CarregaTSearchs()
        {
            RN.Perfil rnPerfil = new Perfil();
            RN.Usuarios rnUsuarios = new Usuarios();
            RN.ControleVaga rnControleVaga = new ControleVaga();

            if (Page.IsCallback)
            {
                return;
            }

            CarregaCursoConcomitante();
            CarregaCurso();
        }

        private void CarregaCursoConcomitante()
        {
            string table = string.Empty;
            Techne.Library.Sql.Structure.SqlSelectColumns coluna = new Techne.Library.Sql.Structure.SqlSelectColumns();
            Techne.Library.Sql.Structure.SqlSelect sqlSelect;

            if (pnlTurmaConcomitante.Visible)
            {
                //Verificar se tem parametros para query
                if (!string.IsNullOrEmpty(lblUnidadeEnsinoOrigem.Text))
                {
                    table = " VW_CURSOCONCOMITANTE ";

                    sqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);

                    tseCursoConcomitante.SqlSelect = sqlSelect;
                    tseCursoConcomitante.SqlWhere = string.Format(" unidade_ens = '{0}' ", lblUnidadeEnsinoOrigem.Text);
                    tseCursoConcomitante.DataBind();
                }
            }

            coluna.Add("curso");
            coluna.Add("nome");

            tseCursoConcomitante.SqlSelect = new Techne.Library.Sql.Structure.SqlSelect(table, coluna, true);

        }

        protected void rblConfirmacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pnlPermuta.Visible = false;
                tseAlunoPermuta.ResetValue();
                chkEnsReligiosoPermuta.Checked = false;
                chkLinEstrangeiraPermuta.Checked = false;
                ddlMotivoPermuta.ClearSelection();

                if (!rblConfirmacao.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                {
                    if (rblConfirmacao.SelectedValue == "Sim")
                    {
                        pnlPermuta.Visible = true;
                    }
                }
            }

            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        protected void tseAlunoPermuta_Changed(object sender, EventArgs args)
        {
            try
            {
                DataTable podeERLE = null;
                RN.Curriculo rnCurriculo = new Techne.Lyceum.RN.Curriculo();
                lblMensagemBloqueio.Text = string.Empty;

                if (!tseAlunoPermuta.DBValue.IsNull)
                {
                    if (tseAlunoPermuta.IsValidDBValue)
                    {
                        //Verifica se existe bloqueio de transferencia para os dados de origem do aluno
                        if (VerificaBloqueio(true, true) || !lblMensagemBloqueio.Text.IsNullOrEmptyOrWhiteSpace())
                        {
                            _tipoOperacao = TipoOperacao.Inicial;
                            this.ControlarTipoOperacao();
                            lblMensagem.Text = string.Empty;
                            return;
                        }

                        this.CarregarDadosDrop(ddlMotivoPermuta.ID);

                        podeERLE = rnCurriculo.ObtemPodeEnsinoReligiosoLinguaEstrangPor(txtCurriculo.Text, txtCurso.Text, txtTurno.Text, Convert.ToInt32(txtAnoLetivo.Text), Convert.ToInt32(txtPeriodoLetivo.Text));

                        if (podeERLE.Rows.Count > 0)
                        {
                            chkEnsReligiosoPermuta.Enabled = Convert.ToBoolean(podeERLE.Rows[0]["PODE_ENSINO_RELIGIOSO"]);
                            chkLinEstrangeiraPermuta.Enabled = Convert.ToBoolean(podeERLE.Rows[0]["PODE_LINGUA_ESTRANGEIRA"]);
                        }

                    }
                    else
                    {
                        lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                        var mensagem = lblMensagem.Text;
                        var script = @"alert('" + mensagem + @"');";
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);


                    }
                }
                else
                {
                    lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                    var mensagem = lblMensagem.Text;
                    var script = @"alert('" + mensagem + @"');";
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);


                }

            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private RN.DTOs.DadosTransferencia ObterDadosPermuta()
        {
            RN.DTOs.DadosTransferencia dadosTransferencia = null;
            RN.Aluno rnAluno = new Techne.Lyceum.RN.Aluno();
            RN.TransferenciaTurma rnTransferenciaTurma = new Techne.Lyceum.RN.TransferenciaTurma();
            RN.GradeSerie rnGradeSerie = new GradeSerie();
            RN.GradeTurma rnGradeTurma = new GradeTurma();
            DataTable dtPermuta = new DataTable();
            DataTable dtTurmaAtualPermuta = new DataTable();


            if (tseAlunoPermuta.IsValidDBValue)
            {
                dadosTransferencia = new RN.DTOs.DadosTransferencia();

                dtPermuta = rnAluno.ObtemDadosAlunoPor(tseAlunoPermuta.DBValue.ToString());

                dtTurmaAtualPermuta = rnTransferenciaTurma.ObtemDadosTurmaAtualPor(tseAlunoPermuta.DBValue.ToString());

                if (dtPermuta.Rows.Count > 0)
                {
                    dadosTransferencia.Aluno = Convert.ToString(tseAlunoPermuta.Value);
                    dadosTransferencia.Ano = txtAnoLetivo.Text;

                    dadosTransferencia.UsuarioResponsavel = User.Identity.Name;
                    dadosTransferencia.NecessidadeEspecial = tseAlunoPermuta["necessidade_especial"].ToString();
                    dadosTransferencia.DataNascimento = (!tseAlunoPermuta.DBValue.IsNull && tseAlunoPermuta.IsValidDBValue) ? Convert.ToDateTime(tseAlunoPermuta["dt_nascimento"]) : DateTime.MinValue;
                    dadosTransferencia.TurnoAtual = tseAlunoPermuta["turno"].ToString();
                    dadosTransferencia.CursoAtual = tseAlunoPermuta["curso"].ToString();
                    dadosTransferencia.SerieAtual = tseAlunoPermuta["serie"].ToString();
                    dadosTransferencia.SemestreAtual = txtPeriodoLetivo.Text;
                    dadosTransferencia.TurmaAtual = dtTurmaAtualPermuta.Rows[0]["TURMA"].ToString();
                    dadosTransferencia.SemestreDestino = txtPeriodoLetivo.Text;

                    dadosTransferencia.TurmaDestino = txtTurmaAtual.Text;
                    dadosTransferencia.MotivoTransferencia = ddlMotivoPermuta.SelectedValue;

                    dadosTransferencia.GradeIdAtual = Convert.ToString(RN.TransferenciaTurma.ConsultarGradeID(dadosTransferencia.Aluno, dadosTransferencia.TurmaAtual));

                    if (txtCobran.Text == "S")
                    {
                        dadosTransferencia.TurnoDestino = txtTurno.Text;
                        dadosTransferencia.SerieDestino = txtSerie.Text;
                        dadosTransferencia.CursoDestino = txtCurso.Text;
                        dadosTransferencia.TipoCursoDestino = RN.Curso.ConsultarTipoProfCurso(txtCurso.Text);
                        dadosTransferencia.CurriculoDestino = txtCurriculo.Text;
                        dadosTransferencia.EnsinoReligioso = Convert.ToString(chkEnsReligiosoPermuta.Checked);
                        dadosTransferencia.LinguaEstrangeira = Convert.ToString(chkLinEstrangeiraPermuta.Checked);
                    }

                    if (!string.IsNullOrEmpty(dadosTransferencia.TipoCursoDestino))
                    {
                        dadosTransferencia.TipoEnsProfissionalizanteDestino = dadosTransferencia.TipoCursoDestino;
                    }
                    else
                    {
                        dadosTransferencia.TipoEnsProfissionalizanteDestino = string.Empty;
                    }

                    dadosTransferencia.UnidadeEnsino = txtUniEnsino.Text;
                    dadosTransferencia.UnidadeFisica = txtUniEnsino.Text;
                    dadosTransferencia.SituacaoAluno = txtSituacao.Text;

                    dadosTransferencia.GradeIdDestino = rnGradeSerie.ObtemGradeIdPor(Convert.ToDecimal(dadosTransferencia.Ano), Convert.ToDecimal(dadosTransferencia.SemestreDestino), dadosTransferencia.CursoDestino, Convert.ToDecimal(dadosTransferencia.SerieDestino), dadosTransferencia.TurmaDestino);

                    dadosTransferencia.ListaDisciplinasTurmaDestino = new List<string>();

                    if (!txtAnoLetivo.Text.IsNullOrEmptyOrWhiteSpace() && !ddlPeriodoDestino.SelectedValue.IsNullOrEmptyOrWhiteSpace() && !ddlTurmaDestino.SelectedValue.IsNullOrEmptyOrWhiteSpace())
                    {
                        dadosTransferencia.ListaDisciplinasTurmaDestino = rnGradeTurma.ListaDisciplinaMatriculaRegular(Convert.ToDecimal(dadosTransferencia.Ano), Convert.ToDecimal(dadosTransferencia.SemestreDestino), dadosTransferencia.TurmaDestino, dadosTransferencia.GradeIdDestino);
                    }
                    else
                    {
                        dadosTransferencia.ListaDisciplinasTurmaDestino = null;
                    }
                }
            }

            return dadosTransferencia;
        }
        protected void LimparTelaPermuta()
        {
            rblConfirmacao.ClearSelection();
            tseAlunoPermuta.ResetValue();
            chkEnsReligiosoPermuta.Checked = false;
            chkLinEstrangeiraPermuta.Checked = false;
            ddlMotivoPermuta.ClearSelection();

        }

    }
}
