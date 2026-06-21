namespace Techne.Lyceum.Net.Academico
{
    using System;
    using System.Web;
    using DevExpress.Web.ASPxTabControl;
    using Techne.Controls;
    using Techne.Lyceum.RN;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Web;
    using Techne.Lyceum.RN.Util;
    using System.Text;
    using System.Data;
    using Techne.Lyceum.RN.DTOs;
    using Techne.Lyceum.RN.DTOs.Agenda;

    [NavUrl("~/Academico/SolicitacaoTransferenciaAluno.aspx"), ControlText("Solicitacao Transferencia Aluno"), Title("Solicitação Transferência Aluno")]
    public partial class SolicitacaoTransferenciaAluno : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.lblMensagem.Text = string.Empty;
            DataTable dt = new DataTable();
            if (!this.IsPostBack)
            {
                this.cmbAno.Items.Clear();
                this.cmbAno.DataSource = PeriodoLetivo.ListarAnos();
                this.cmbAno.Items.Insert(0, "Selecione");
                this.cmbAno.DataBind();

                var alunoChave = this.ObtemAluno();

                if (alunoChave != null)
                {
                    tseAluno.DBValue = alunoChave.Aluno;
                    tseAluno.DataBind();

                    DataTable dtAno = new DataTable();

                    dt = Aluno.ListaAlunoNovoTransf(tseAluno.DBValue.ToString());

                    if (dt.Rows.Count > 0)
                    {

                        tseMunicipio.DBValue = !string.IsNullOrEmpty(dt.Rows[0]["municipio"].ToString()) ? dt.Rows[0]["municipio"].ToString() : null;
                        tseMunicipio.DataBind();

                        tseUnidadeResponsavel.DBValue = dt.Rows[0]["unidade_ensino"].ToString();
                        tseUnidadeResponsavel.DataBind();

                    }

                    if (hdnCompartilhada.Value == "S")
                    {
                        cmbAno.Enabled = false;
                        cmbPeriodo.Enabled = false;
                        
                        cmbSerie.Enabled = false;
                        tseCurso.Enabled = false;
                        tseUnidadeEnsinoDestino.Enabled = false;
                      
                    }

                    //if (!string.IsNullOrEmpty(dt.Rows[0]["municipio"].ToString()))
                    //{
                    //    tseMunicipio.DBValue = dt.Rows[0]["municipio"].ToString();
                    //    tseMunicipio.DataBind();
                    //}

                    this.lblMensagem.Text = string.Empty;

                    if (Transferencia.ExistePendenciaTranf(this.tseAluno.DBValue.ToString()))
                    {
                        this.lblMensagem.Text = "Já existe uma solicitação de transferência pendente para este aluno.";
                        return;
                    }

                  

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["sit_aluno"].ToString() == "Ativo")
                        {
                            dtAno = Aluno.RetornaAnoPeriodoMatricula(this.tseAluno.DBValue.ToString());

                            if (dtAno.Rows.Count > 0)
                            {
                                this.cmbAno.Enabled = false;
                                this.cmbAno.SelectedValue = dtAno.Rows[0]["ano"].ToString();

                                if (!string.IsNullOrEmpty(cmbAno.SelectedValue))
                                {
                                    this.cmbPeriodo.Items.Clear();
                                    this.cmbPeriodo.DataSource = PeriodoLetivo.ListarPeriodo(this.cmbAno.SelectedValue);
                                    this.cmbPeriodo.Items.Insert(0, "Selecione");
                                    this.cmbPeriodo.DataBind();

                                    this.cmbPeriodo.SelectedValue = dtAno.Rows[0]["semestre"].ToString();
                                    this.hdnPeriodoOrigem.Value = dtAno.Rows[0]["semestre"].ToString();
                                }
                            }
                        }
                    }
                }
            }
        }

        private Aluno.DadosAluno ObtemAluno()
        {
            try
            {
                var objetoAluno = new Aluno.DadosAluno();
                hdnCompartilhada.Value = string.Empty;
                hdnSerieCompartilhada.Value = string.Empty;
                var decodedBytes = Convert.FromBase64String(this.Request.QueryString["Chave"]);
                var decodedText = Encoding.UTF8.GetString(decodedBytes);

                var listaDados = decodedText.Split('&');

                foreach (var dados in listaDados)
                {
                    if (dados.IndexOf("aluno") >= 0)
                    {
                        objetoAluno.Aluno = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("unidadeResponsavel") >= 0)
                    {
                        objetoAluno.UnidadeResponsavel = dados.Substring(dados.LastIndexOf('=') + 1);
                        tseUnidadeEnsinoDestino.DBValue = objetoAluno.UnidadeResponsavel;
                    }
                    else if (dados.IndexOf("comp") >= 0)
                    {
                        hdnCompartilhada.Value = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("ano") >= 0)
                    {
                        cmbAno.SelectedValue = dados.Substring(dados.LastIndexOf('=') + 1);
                    }

                    else if (dados.IndexOf("periodo") >= 0)
                    {
                        cmbAno_SelectedIndexChanged(null, null);
                        cmbPeriodo.SelectedValue = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("curso") >= 0)
                    {
                        cmbPeriodo_SelectedIndexChanged(null, null);
                        tseCurso.DBValue = dados.Substring(dados.LastIndexOf('=') + 1);
                        tseCurso_Changed(null, null);
                    }
                    else if (dados.IndexOf("serie") >= 0)
                    {
                        hdnSerieCompartilhada.Value = dados.Substring(dados.LastIndexOf('=') + 1);
                    }
                    else if (dados.IndexOf("destino") >= 0)
                    {
                        objetoAluno.UnidadeResponsavel = dados.Substring(dados.LastIndexOf('=') + 1);
                        tseUnidadeEnsinoDestino.DBValue = objetoAluno.UnidadeResponsavel;
                    }

                }
                return objetoAluno;
            }
            catch
            {
                return null;
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            ControlarVisibilidadeControle();
        }

        private void ControlarVisibilidadeControle()
        {
            ControlaAcesso(btnSalvar, AcaoControle.novo);
        }

        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            var transferencia = new TceTransferencia();
            var transferenciaOrigem = new TceTransferenciaOrigem();
            var transferenciaDestino = new TceTransferenciaDestino();
            RestricaoIdadeSerie rnRestricaoIdadeSerie = new RestricaoIdadeSerie();
            TceRestricaoIdadeSerie restricao = new TceRestricaoIdadeSerie();
            int ano;
            int periodo;
            int serie;
            int idade = 0;
            string mensagemAlerta = string.Empty;

            if (this.chkAtesto.Checked == false)
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Não é possivel realizar a solicitação sem a entrega da declaração de transferência.\\nPor favor, entre em contato com a escola de origem do aluno.');", true);
                return;
            }

            if (this.tseCurso.DBValue.IsNull
                || !this.tseCurso.IsValidDBValue
                || !(int.TryParse(this.cmbSerie.SelectedValue, out serie)))
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Os campos curso/série é de preenchimento obrigatório.');", true);
                return;
            }
            if (string.IsNullOrEmpty(tseAluno["dt_nascimento"].ToString()))
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Aluno sem Data de Nascimento cadastrada. Favor verifique!');", true);
                return;
            }

            idade = Utils.CalcularIdade(Convert.ToDateTime(tseAluno["dt_nascimento"]));
            restricao = rnRestricaoIdadeSerie.CarregaRestricaoPor(this.tseCurso.DBValue.ToString(), Convert.ToInt32(this.cmbSerie.SelectedValue));

            if (string.IsNullOrEmpty(tseAluno["necessidade_especial"].ToString()) || tseAluno["necessidade_especial"].ToString() == "Não possui.")
            {
                //Para Alunos sem necessidades Especiais Verificar restrição de idade minima e maxima
                if (idade < restricao.IdadeMinima || idade > restricao.IdadeMaxima)
                {
                    string mensagem = string.Format("Para o curso selecionado é permitido cadastrar alunos entre {0} e {1} anos. Favor verificar a DATA DE NASCIMENTO!",
                        restricao.IdadeMinima,
                        restricao.IdadeMaxima);
                    lblMensagem.Text = mensagem;
                    var script = @"alert('" + mensagem + @"');";
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                    return;
                }
            }
            else
            {
                //Para Alunos com necessidades Especiais Verificar restrição de idade minima
                if (idade < restricao.IdadeMinima)
                {
                    restricao = rnRestricaoIdadeSerie.CarregaRestricaoPor(this.tseCurso.DBValue.ToString(), Convert.ToInt32(this.cmbSerie.SelectedValue));
                    string mensagem = string.Format("Para o curso selecionado não é permitido cadastrar alunos com necessidade especial com menos de {0} anos. Favor verificar a DATA DE NASCIMENTO!",
                        restricao.IdadeMinima);
                    lblMensagem.Text = mensagem;
                    var script = @"alert('" + mensagem + @"');";
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                    return;
                }
            }

            transferencia.MatriculaSolicitante = this.User.Identity.Name;
            transferencia.Status = Transferencia.Pendente;

            if (!string.IsNullOrEmpty(this.cmbTurno.SelectedValue)
                && this.cmbTurno.SelectedValue != "Selecione")
            {
                transferenciaDestino.Turno = this.cmbTurno.SelectedValue;
            }

            if (!string.IsNullOrEmpty(this.cmbTurma.SelectedValue)
                && this.cmbTurma.SelectedValue != "Selecione")
            {
                transferenciaDestino.Turma = this.cmbTurma.SelectedValue;
            }


            if (!string.IsNullOrEmpty(this.cmbMotivoTransf.SelectedValue)
                && this.cmbMotivoTransf.SelectedValue != "Selecione")
            {
                transferencia.Motivo = this.cmbMotivoTransf.SelectedValue;
            }

            if (!string.IsNullOrEmpty(this.cmbTipoCurso.SelectedValue)
                && this.cmbTipoCurso.SelectedValue != "Selecione")
            {
                transferenciaDestino.TipoCurso = this.cmbTipoCurso.SelectedValue;
            }

            if (int.TryParse(this.cmbAno.SelectedValue, out ano))
            {
                transferenciaDestino.Ano = ano;
            }

            if (int.TryParse(this.cmbPeriodo.SelectedValue, out periodo))
            {
                transferenciaDestino.Periodo = periodo;
            }

            if (int.TryParse(this.cmbSerie.SelectedValue, out serie))
            {
                transferenciaDestino.Serie = serie;
            }

            if (!this.tseAluno.DBValue.IsNull
                && this.tseAluno.IsValidDBValue)
            {
                transferencia.Aluno = this.tseAluno.DBValue.ToString();
            }

            if (!this.tseCurso.DBValue.IsNull
                && this.tseCurso.IsValidDBValue)
            {
                transferenciaDestino.Curso = this.tseCurso.DBValue.ToString();
            }

            if (!this.tseUnidadeResponsavel.DBValue.IsNull
                && this.tseUnidadeResponsavel.IsValidDBValue)
            {
                transferenciaOrigem.Censo = this.tseUnidadeResponsavel.DBValue.ToString();
            }

            if (!this.tseUnidadeEnsinoDestino.DBValue.IsNull
                && this.tseUnidadeEnsinoDestino.IsValidDBValue)
            {
                transferenciaDestino.Censo = this.tseUnidadeEnsinoDestino.DBValue.ToString();

                if (!this.tseUnidadeEnsinoDestino["UNIDADE_FIS"].IsNull)
                {
                    transferenciaDestino.UnidadeFisica = this.tseUnidadeEnsinoDestino["UNIDADE_FIS"].ToString();
                }
            }

            transferenciaDestino.EnsinoReligioso = chkEnsReligioso.Checked;
            transferenciaDestino.LinguaEstrangeiraFacultativa = chkLinguaEstrangeira.Checked;

            bool compartilhada = false;

            if (hdnCompartilhada.Value == "S")
                compartilhada = true;

            var validacao = Transferencia.ValidarSolicitacao(transferencia, transferenciaDestino, transferenciaOrigem, compartilhada);

            if (validacao.Valido)
            {
                //Verifica se existe bloqueio de transferencia para os dados de destino do aluno, caso nao seja transferencia de compartilhada
                if (!compartilhada && this.VerificaBloqueio())
                {
                    lblMensagem.Text = "A transferencia não pode ser realizada: " + lblMensagemBloqueio.Text;
                    var mensagem = lblMensagem.Text;
                    var script = @"alert('" + mensagem + @"');";
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", script, true);
                    lblMensagemBloqueio.Text = string.Empty;

                    return;
                }

                Transferencia.Inserir(transferencia, transferenciaDestino, transferenciaOrigem, this.tseAluno["sit_aluno"].ToString());

                mensagemAlerta = "Sua solicitação foi enviada com sucesso.\\nA unidade de origem terá o prazo de 5 dias úteis para analisar o pedido de transferência.\\nPara acompanhar o status da solicitação, acesse a aba\\n\"Acompanhamento das Solicitações\" disponível nessa mesma tela.";

                if (this.tseAluno["sit_aluno"].ToString() != "Ativo")
                {
                    mensagemAlerta += @"\\nA MATRÍCULA REABERTA SOMENTE TERÁ DIREITO À GRATUIDADE APÓS A ATUALIZAÇÃO CADASTRAL DOS CAMPOS: 
                                    ENDEREÇO/E-MAIL/GRATUIDADE/ MODAL. CASO O ALUNO JÁ TENHA UTILIZADO TRANSPORTE E POSSUA O CARTÃO, O MESMO SERÁ 
                                    REATIVADO, CASO NÃO POSSUA, ORIENTE QUE ACESSE O SITE DA RIOCARD PARA SOLICITAR O CARTÃO.";
                }

                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", string.Format("alert('{0}');", mensagemAlerta), true);

                this.LimparCampos();
                this.tseAluno.ResetValue();
            }
            else
            {
                if (!string.IsNullOrEmpty(validacao.Mensagem))
                {
                    this.lblMensagem.Text = validacao.Mensagem.Replace(Environment.NewLine, "<br />");
                }
            }
        }

        protected void cmbAno_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmbPeriodo.Items.Clear();
            this.cmbSerie.Items.Clear();
            this.cmbTurno.Items.Clear();
            this.tseCurso.ResetValue();

            if (this.cmbAno.SelectedValue != "Selecione" && !string.IsNullOrEmpty(cmbAno.SelectedValue))
            {
                this.cmbPeriodo.DataSource = PeriodoLetivo.ListarPeriodo(this.cmbAno.SelectedValue);
                this.cmbPeriodo.Items.Insert(0, "Selecione");
                this.cmbPeriodo.DataBind();
            }
        }

        protected void cmbPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmbSerie.Items.Clear();
            this.cmbTurno.Items.Clear();
            this.cmbTurma.Items.Clear();
            this.cmbMotivoTransf.Items.Clear();
            this.tseCurso.ResetValue();
            chkEnsReligioso.Enabled = false;
            chkEnsReligioso.Checked = false;
            chkLinguaEstrangeira.Enabled = false;
            chkLinguaEstrangeira.Checked = false;

            if (string.IsNullOrEmpty(this.tseUnidadeEnsinoDestino.DBValue.ToString()))
            {
                this.lblMensagem.Text = "O campo Unidade de Ensino de Destino deverá ser preenchido.";
                return;
            }

            if (this.cmbPeriodo.SelectedValue != "Selecione")
            {
                if (!string.IsNullOrEmpty(this.hdnPeriodoOrigem.Value) && this.hdnPeriodoOrigem.Value != "0")
                {
                    if (this.hdnPeriodoOrigem.Value == "1" && this.cmbPeriodo.SelectedValue == "2")
                    {
                        this.lblMensagem.Text = "Não é permitido transferir aluno do periodo letivo 1 para o 2";

                        this.cmbPeriodo.ClearSelection();

                        return;
                    }

                    if (this.hdnPeriodoOrigem.Value == "2" && this.cmbPeriodo.SelectedValue == "1")
                    {
                        this.lblMensagem.Text = "Não é permitido transferir aluno do periodo letivo 2 para o 1";

                        this.cmbPeriodo.ClearSelection();

                        return;
                    }
                }

                this.tseCurso.SqlWhere = " t.faculdade = '" + Convert.ToString(this.tseUnidadeEnsinoDestino.DBValue) + "' and c.curso not in ('9999.91','9999.92') and t.ano = " + int.Parse(this.cmbAno.SelectedValue) + " and t.semestre = " + int.Parse(this.cmbPeriodo.SelectedValue);
            }
        }

        protected void cmbSerie_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmbTurma.Items.Clear();
            this.cmbMotivoTransf.Items.Clear();
            chkEnsReligioso.Enabled = false;
            chkEnsReligioso.Checked = false;
            chkLinguaEstrangeira.Enabled = false;
            chkLinguaEstrangeira.Checked = false;

            if (this.cmbSerie.SelectedValue != "Selecione")
            {
                this.cmbTurma.DataSource = Turma.ConsultarPrimeiraTurmaDisponivel(
                    int.Parse(this.cmbAno.SelectedValue),
                    int.Parse(this.cmbPeriodo.SelectedValue),
                    Convert.ToString(this.tseUnidadeEnsinoDestino.DBValue),
                    this.cmbTurno.SelectedValue,
                    this.tseCurso.DBValue.ToString(),
                    int.Parse(this.cmbSerie.SelectedValue)
                    );
                this.cmbTurma.Items.Insert(0, "Selecione");
                this.cmbTurma.DataBind();
            }
        }

        protected void cmbTurno_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmbSerie.Items.Clear();
            this.cmbTurma.Items.Clear();
            this.cmbMotivoTransf.Items.Clear();
            chkEnsReligioso.Enabled = false;
            chkEnsReligioso.Checked = false;
            chkLinguaEstrangeira.Enabled = false;
            chkLinguaEstrangeira.Checked = false;

            if (this.cmbTurno.SelectedValue != "Selecione")
            {
                this.cmbSerie.DataSource = Serie.ListarSeriePorTurmaUE(Convert.ToString(this.tseUnidadeEnsinoDestino.DBValue), this.tseCurso.DBValue.ToString(), int.Parse(this.cmbAno.SelectedValue), int.Parse(this.cmbPeriodo.SelectedValue), this.cmbTurno.SelectedValue);
                this.cmbSerie.Items.Insert(0, "Selecione");
                this.cmbSerie.DataBind();

                if (hdnCompartilhada.Value == "S")
                {

                    if (cmbSerie.Items.FindByValue(hdnSerieCompartilhada.Value) != null)
                    {
                        cmbSerie.SelectedValue = hdnSerieCompartilhada.Value;
                        cmbSerie_SelectedIndexChanged(null, null);
                    }
                    else
                    {
                        lblMensagem.Text = "A série escolhida na Inscriçao de Compartilhadas não possui turma vigente para este ano/período. Verifique.";
                    }
                }
            }
        }

        protected void cmbTurma_SelectedIndexChanged(object sender, EventArgs e)
        {
            LyCurriculo curriculo = new LyCurriculo();
            TceConfirmacaoMatricula confirmacaoMatricula = new TceConfirmacaoMatricula();
            RN.Curriculo rnCurriculo = new Techne.Lyceum.RN.Curriculo();
            RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();

            this.cmbMotivoTransf.Items.Clear();
            chkEnsReligioso.Enabled = false;
            chkEnsReligioso.Checked = false;
            chkLinguaEstrangeira.Enabled = false;
            chkLinguaEstrangeira.Checked = false;

            if (this.cmbTurma.SelectedValue != "Selecione")
            {
                //Busca Curriculo
                curriculo = rnCurriculo.ObtemCurriculoPor(int.Parse(this.cmbAno.SelectedValue), int.Parse(this.cmbPeriodo.SelectedValue), this.cmbTurno.SelectedValue, this.tseCurso.DBValue.ToString(), Convert.ToString(this.tseUnidadeEnsinoDestino.DBValue), this.cmbTurma.SelectedValue);

                //Busca opções da confirmaçao de matricula atual do aluno
                confirmacaoMatricula = rnConfirmacaoMatricula.ObtemConfirmacaoAtivaPor(tseAluno.DBValue.ToString(), decimal.Parse(this.cmbAno.SelectedValue), decimal.Parse(this.cmbPeriodo.SelectedValue), int.Parse(this.cmbSerie.SelectedValue), this.cmbTurno.SelectedValue, this.tseCurso.DBValue.ToString(), Convert.ToString(this.tseUnidadeResponsavel.DBValue));

                //Verifica se o curriculo permite ensino religioso
                if (curriculo.EnsinoReligioso == "S")
                {
                    chkEnsReligioso.Enabled = true;
                    chkEnsReligioso.Checked = confirmacaoMatricula.EnsinoReligioso;
                }

                //Verifica se o curriculo permite lingua estrangeira
                if (curriculo.LinguaEstrangeira == "S")
                {
                    chkLinguaEstrangeira.Enabled = true;
                    chkLinguaEstrangeira.Checked = confirmacaoMatricula.LinguaEstrangeiraFacultativa;
                }

                this.cmbMotivoTransf.Items.Clear();
                this.cmbMotivoTransf.DataSource = TabelaGeral.ConsultaItemTabelaValDescr("MotivoTransferência");
                this.cmbMotivoTransf.DataBind();
                this.cmbMotivoTransf.Items.Insert(0, "Selecione");
            }
        }

        protected void pcTransferencia_TabClick(object source, TabControlCancelEventArgs e)
        {
            this.Server.Transfer("AcompanhamentoTransferencia.aspx");
        }

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            if (this.Page.IsCallback)
            {
                return;
            }

            this.LimparCampos();

            if (!this.tseAluno.DBValue.IsNull)
            {
                if (this.tseAluno.IsValidDBValue)
                {
                    this.tseMunicipio.Value = this.tseAluno["municipio"];
                    this.tseUnidadeResponsavel.Value = this.tseAluno["unidade_ensino"];
                    this.lblMensagem.Text = string.Empty;

                    if (Transferencia.ExistePendenciaTranf(this.tseAluno.DBValue.ToString()))
                    {
                        this.lblMensagem.Text = "Já existe uma solicitação de transferência pendente para este aluno.";
                        return;
                    }
                    if (string.IsNullOrEmpty(tseAluno["dt_nascimento"].ToString()))
                    {
                        this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "popup", "alert('Aluno sem Data de Nascimento cadastrada. Favor verifique!');", true);
                        return;
                    }

                    this.cmbAno.Items.Clear();
                    this.cmbAno.DataSource = PeriodoLetivo.ListarAnos();
                    this.cmbAno.Items.Insert(0, "Selecione");
                    this.cmbAno.DataBind();

                    if (this.tseAluno["sit_aluno"].ToString() == "Ativo")
                    {
                        var dt = Aluno.RetornaAnoPeriodoMatricula(this.tseAluno.DBValue.ToString());

                        if (dt.Rows.Count > 0)
                        {
                            this.cmbAno.Enabled = false;
                            this.cmbAno.SelectedValue = dt.Rows[0]["ano"].ToString();

                            if (!string.IsNullOrEmpty(cmbAno.SelectedValue))
                            {
                                this.cmbPeriodo.Items.Clear();
                                this.cmbPeriodo.DataSource = PeriodoLetivo.ListarPeriodo(this.cmbAno.SelectedValue);
                                this.cmbPeriodo.Items.Insert(0, "Selecione");
                                this.cmbPeriodo.DataBind();

                                this.cmbPeriodo.SelectedValue = dt.Rows[0]["semestre"].ToString();
                                this.hdnPeriodoOrigem.Value = dt.Rows[0]["semestre"].ToString();
                            }
                        }
                    }
                    else
                    {
                        this.cmbAno.Enabled = true;
                        this.cmbPeriodo.Enabled = true;
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
                }
            }
            else
            {
                this.lblMensagem.Text = "Aluno não ativo ou não cadastrado (favor verificar).";
            }
        }

        protected bool VerificaBloqueio()
        {
            RN.Aluno rnAluno = new RN.Aluno();
            RN.Aluno.DadosAluno aluno = new RN.Aluno.DadosAluno();
            RN.Agenda.Agenda rnAgenda = new Techne.Lyceum.RN.Agenda.Agenda();
            int idEventoBloqueioTransferenciaUnidade = Convert.ToInt32(RN.Agenda.TipoEvento.TipoEventoAgenda.BloqueioTransferenciaUnidade);
            DadosParticipacao bloqueio = new DadosParticipacao();
            RN.ControleVaga rnControleVaga = new ControleVaga();
            RN.Perfil rnPerfil = new Perfil();

            try
            {
                //Busca dados de destino da transferencia
                aluno.UnidadeResponsavel = Convert.ToString(tseUnidadeEnsinoDestino.DBValue);
                aluno.Curso = Convert.ToString(tseCurso.DBValue);
                aluno.Turno = cmbTurno.SelectedValue;
                aluno.Serie = cmbSerie.SelectedValue;
                int ano = Convert.ToInt32(this.cmbAno.SelectedValue);
                int periodo = Convert.ToInt32(this.cmbPeriodo.SelectedValue);

                //Verifica se Escola / Curso / Série / Turno participa da fase 3
                if (rnControleVaga.PartipaMatriculaFacilPor(aluno.UnidadeResponsavel, ano, periodo, aluno.Curso, Convert.ToInt32(aluno.Serie), aluno.Turno))
                {
                    this.tseUnidadeResponsavel.ResetValue();
                    this.tseMunicipio.ResetValue();
                    this.tseAluno.ResetValue();
                    this.LimparCampos();
                    lblMensagemBloqueio.Text = "Transferências não permitida. Esta Escola / Curso / Série / Turno está participando do Matrícula Fácil.";
                    return true;
                }

                bloqueio = rnAgenda.VerificaEventoInversoPor(idEventoBloqueioTransferenciaUnidade, aluno.UnidadeResponsavel, aluno.Curso, aluno.Turno, Convert.ToInt32(aluno.Serie));

                if (!rnPerfil.PossuiPerfilMatriculaTransferenciaPeriodoBloqueioPor(User.Identity.Name))
                {

                    if (bloqueio.ParticipaTotal)
                    {
                        this.tseUnidadeResponsavel.ResetValue();
                        this.tseMunicipio.ResetValue();
                        this.tseAluno.ResetValue();
                        this.LimparCampos();
                        lblMensagemBloqueio.Text = string.Format("Transferências de alunos entre unidades estão bloqueadas no período de {0} á {1} de acordo com a Agenda da SEEDUC.",
                                   bloqueio.DataInicio.ToString("dd/MM/yyyy"),
                                   bloqueio.DataFim.ToString("dd/MM/yyyy"));
                        return true;
                    }

                    if ((bloqueio.ParticipaCurso && bloqueio.ParticipaUnidade))
                    {
                        this.tseUnidadeResponsavel.ResetValue();
                        this.tseMunicipio.ResetValue();
                        this.tseAluno.ResetValue();
                        this.LimparCampos();
                        lblMensagemBloqueio.Text = string.Format("Transferências entre unidades com os dados de destino, estão bloqueadas no período de {0} á {1} de acordo com a Agenda da SEEDUC.",
                                   bloqueio.DataInicio.ToString("dd/MM/yyyy"),
                                   bloqueio.DataFim.ToString("dd/MM/yyyy"));
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
                return false;
            }
        }

        protected void tseCurso_Changed(object sender, ChangedEventArgs args)
        {

            if (cmbPeriodo.SelectedValue == "Selecione" || cmbAno.SelectedValue == "Selecione")
            {
                lblMensagem.Text = "Favor selecionar o Ano/Período Letivo.";
                return;
            }

            this.lblTipoCurso.Visible = false;
            this.cmbTipoCurso.Visible = false;
            this.cmbTipoCurso.ClearSelection();
            this.cmbSerie.Items.Clear();
            this.cmbTurno.Items.Clear();
            this.cmbTurma.Items.Clear();
            this.cmbMotivoTransf.Items.Clear();
            chkEnsReligioso.Enabled = false;
            chkEnsReligioso.Checked = false;
            chkLinguaEstrangeira.Enabled = false;
            chkLinguaEstrangeira.Checked = false;

            if (!this.tseCurso.DBValue.IsNull && !this.tseUnidadeEnsinoDestino.DBValue.IsNull)
            {
                this.cmbTurno.DataSource = Turno.ListarTurnosPorTurmaUE(Convert.ToString(this.tseUnidadeEnsinoDestino.DBValue), this.tseCurso.DBValue.ToString(), int.Parse(this.cmbAno.SelectedValue), int.Parse(this.cmbPeriodo.SelectedValue));
                this.cmbTurno.Items.Insert(0, "Selecione");
                this.cmbTurno.DataBind();

                var tipo_curso = Curso.ConsultarTipoProfCurso(this.tseCurso.Value.ToString());

                if (tipo_curso == "Concomitante/Subsequente")
                {
                    this.lblTipoCurso.Visible = true;
                    this.cmbTipoCurso.Visible = true;
                }
                else
                {
                    this.lblTipoCurso.Visible = false;
                    this.cmbTipoCurso.Visible = false;
                }
            }
        }

        protected void tseMunicipio_Changed(object sender, ChangedEventArgs args)
        {
            if (this.Page.IsCallback)
            {
                return;
            }

            this.LimparCampos();

            this.tseUnidadeResponsavel.ResetValue();
            this.tseAluno.ResetValue();

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

        protected void tseUnidadeEnsinoDestino_Changed(object sender, ChangedEventArgs args)
        {
            this.cmbSerie.Items.Clear();
            this.cmbTurno.Items.Clear();
            this.cmbTurma.Items.Clear();
            this.cmbMotivoTransf.Items.Clear();
            this.tseCurso.ResetValue();
            chkEnsReligioso.Enabled = false;
            chkEnsReligioso.Checked = false;
            chkLinguaEstrangeira.Enabled = false;
            chkLinguaEstrangeira.Checked = false;

            if (!this.tseUnidadeResponsavel.DBValue.IsNull)
            {
                if (this.tseUnidadeResponsavel.IsValidDBValue)
                {
                    if (!string.IsNullOrEmpty(this.cmbPeriodo.SelectedValue) && this.cmbPeriodo.SelectedValue != "Selecione")
                    {
                        this.tseCurso.SqlWhere = " t.faculdade = '" + Convert.ToString(this.tseUnidadeEnsinoDestino.DBValue) + "' and c.curso not in ('9999.91','9999.92') and t.ano = " + int.Parse(this.cmbAno.SelectedValue) + " and t.semestre = " + int.Parse(this.cmbPeriodo.SelectedValue);
                    }
                }
                else
                {
                    this.lblMensagem.Text = "Unidade de Ensino de Destino não cadastrada.";
                }
            }
            else
            {
                this.lblMensagem.Text = "Favor consultar uma unidade de ensino de destino.";

                this.LimparCampos();
            }
        }

        protected void tseUnidadeResponsavel_Changed(object sender, ChangedEventArgs args)
        {
            this.LimparCampos();

            this.tseAluno.ResetValue();

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

        private void LimparCampos()
        {
            this.lblMensagemBloqueio.Text = string.Empty;
            this.lblMensagem.Text = string.Empty;
            this.cmbPeriodo.Items.Clear();
            this.cmbTurma.Items.Clear();
            this.cmbSerie.Items.Clear();
            this.cmbTurno.Items.Clear();
            this.cmbAno.Items.Clear();
            this.cmbMotivoTransf.Items.Clear();
            this.tseUnidadeEnsinoDestino.ResetValue();
            this.hdnPeriodoOrigem.Value = string.Empty;
            this.tseCurso.ResetValue();
            this.lblTipoCurso.Visible = false;
            this.cmbTipoCurso.ClearSelection();
            this.cmbTipoCurso.Visible = false;
            this.chkAtesto.Checked = false;
            chkEnsReligioso.Enabled = false;
            chkEnsReligioso.Checked = false;
            chkLinguaEstrangeira.Enabled = false;
            chkLinguaEstrangeira.Checked = false;

        }
    }
}