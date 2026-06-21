using System;
using System.Data;
using System.Reflection;
using Techne.Lyceum.RN;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.HtmlControls;
using Techne.Auditing;
using System.Web;

namespace Techne.Lyceum.Net.Academico
{
    [NavUrl("~/Academico/Boletim.aspx"), ControlText("Boletim"), Title("Boletim")]
    public partial class Boletim : TPage
    {
        private AlunoAutenticado AlunoAutenticado
        {
            get
            {
                //Verifica se existe session do aluno logado
                if (this.Session["AlunoAutenticado"] == null)
                {
                    //Caso não exista redirecionar para login
                    Response.Redirect(Seguranca.Identificacao.GetUrl());
                    return null;
                }

                return this.Session["AlunoAutenticado"] as AlunoAutenticado;
            }
        }

        public static string GetUrl()
        {
            return Navigation
                .GetNavigation(MethodBase.GetCurrentMethod())
                .GetUrl(new object[]
                        {
                        });
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            AuditaPagina(System.Web.HttpContext.Current.Request.Url.AbsolutePath);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.lblMensagem.Text = string.Empty;

            if (!this.IsPostBack)
            {
                try
                {
                    LimpaTela();
                    this.CarregaDadosSession();
                    this.CarregaDadosAluno();
                }
                catch (Exception ex)
                {
                    this.lblMensagem.Text = ex.Message;
                }
            }
        }

        protected void LimpaTela()
        {
            this.divTelefone.Visible = false;
            this.divPortaria.Visible = false;
            this.divInformativo.Visible = false;
            this.divImprimir.Visible = false;
            this.divPrincipal.Visible = false;
            this.divGrdBoletim.Visible = false;
            this.divSituacaoFinal.Visible = false;
            this.divmsg.Visible = true;
            this.txtFone.Text = string.Empty;
            this.lblNome.Text = string.Empty;
            this.lblUnidade.Text = string.Empty;
            this.lblAno.Text = string.Empty;
            this.lblPeriodo.Text = string.Empty;
            this.lblEscolaridade.Text = string.Empty;
            this.lblAnoEscolaridade.Text = string.Empty;
            this.lblTurno.Text = string.Empty;
            this.lblTurmaPrincipal.Text = string.Empty;
            this.lblSituacaoFinal.Text = string.Empty;
        }

        protected void btnAtualizar_Click(object sender, EventArgs e)
        {
            string telefone = string.Empty;
            RN.Boletim rnBoletim = new Techne.Lyceum.RN.Boletim();

            try
            {
                telefone = !txtFone.Text.IsNullOrEmptyOrWhiteSpace() ? this.txtFone.Text.RetirarMascaraTelefone() : null;

                if (string.IsNullOrEmpty(telefone))
                {
                    this.lblMensagem.Text = "O TELEFONE DO RESPONSÁVEL é de preenchimento obrigatório.";
                    return;
                }

                if (this.AlunoAutenticado == null || this.AlunoAutenticado.Pessoa <= 0)
                {
                    this.lblMensagem.Text = "Aluno autenticado não foi localizado, favor efetuar novamente o login";
                    return;
                }

                // Salva telefone do responsável
                rnBoletim.SalvaTelefoneResponsavel(this.AlunoAutenticado.Pessoa, telefone);
                AlunoAutenticado.TelefoneResponsavel = telefone;

                this.lblMensagem.Text = "Telefone do responsável atualizado com sucesso.";
            }
            catch (Exception ex)
            {
                this.lblMensagem.Text = ex.Message;
            }
        }

        private void PreencheTelefoneResponsavel()
        {
            try
            {

                long resultadoFixoCelular;
                var fixoCelular = this.AlunoAutenticado.TelefoneResponsavel.RetirarMascaraTelefone();

                if (long.TryParse(fixoCelular, out resultadoFixoCelular))
                {
                    if (fixoCelular.Length == 10)
                    {
                        txtFone.Text = string.Format("{0:(00)0000-0000}", resultadoFixoCelular);
                    }
                    else if (fixoCelular.Length == 11)
                    {
                        txtFone.Text = string.Format("{0:(00)00000-0000}", resultadoFixoCelular);
                    }
                    else
                    {
                        txtFone.Text = resultadoFixoCelular.ToString();

                    }
                }

            }
            catch (Exception ex)
            {
                this.lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaDadosSession()
        {
            this.hdnAno.Value = string.Empty;
            this.hdnSemestre.Value = string.Empty;
            this.hdnQuantidadeBimestres.Value = string.Empty;
            this.hdnMatriculaAtiva.Value = string.Empty;
            this.hdnMatriculaHistorico.Value = string.Empty;

            try
            {
                //Verifica se existe as sessions do login
                if (this.Session["AlunoAutenticado"] == null || this.Session["AnoLetivo"] == null || this.Session["PeriodoLetivo"] == null
                    || this.Session["MatriculaAtiva"] == null || this.Session["MatriculaHistorico"] == null)
                {
                    //Caso não exista redirecionar para login
                    Response.Redirect(Seguranca.Identificacao.GetUrl());
                }

                hdnAno.Value = Convert.ToString(this.Session["AnoLetivo"]);
                hdnSemestre.Value = Convert.ToString(this.Session["PeriodoLetivo"]);
                hdnMatriculaAtiva.Value = Convert.ToString(this.Session["MatriculaAtiva"]);
                hdnMatriculaHistorico.Value = Convert.ToString(this.Session["MatriculaHistorico"]);
            }
            catch (Exception ex)
            {
                lblMensagem.Text = ex.Message;
            }
        }

        private void CarregaDadosAluno()
        {
            this.divPortaria.Visible = true;
            this.divTelefone.Visible = true;
            this.divPrincipal.Visible = true;
            this.divInformativo.Visible = true;
            this.divmsg.Visible = true;
            RN.Boletim rnBoletim = new RN.Boletim();
            DataTable dtDadosPrincipal = new DataTable();
            this.divImprimir.Visible = true;
            string aluno = string.Empty;
            string nomeAluno = string.Empty;
            int ano = Convert.ToInt32(hdnAno.Value);
            int periodo = Convert.ToInt32(hdnSemestre.Value);
            DataTable dadosSituacaoFinal = new DataTable();

            try
            {
                aluno = this.AlunoAutenticado.Matricula;
                nomeAluno = this.AlunoAutenticado.Nome.ToUpper();

                //Verifica se existe notas pendendtes em anos anteriores
                if (rnBoletim.ExisteProvaSemNotaPor(aluno) || rnBoletim.ExisteFrequenciaSemFaltaPor(aluno))
                {
                    this.lblMensagem.Text = "Nota e ou Frequencia pendente em anos anteriores, entre em contato com a direção de sua escola.";
                }

                if (Convert.ToBoolean(hdnMatriculaAtiva.Value))
                {
                    //caso seja matricula atual
                    dtDadosPrincipal = rnBoletim.ObtemDadosTurmaPrincipalPor(aluno, ano, periodo);
                }
                else if (Convert.ToBoolean(hdnMatriculaHistorico.Value))
                {
                    //caso seja matricula de historico
                    this.divImprimir.Visible = false;
                    dtDadosPrincipal = rnBoletim.ObtemDadosTurmaPrincipalHistoricoPor(aluno, ano, periodo);
                }
                else
                {
                    this.LimpaTela();
                    this.lblMensagem.Text = "Por favor, procure a secretaria de sua unidade para obter informações referentes à sua matrícula.";
                    return;
                }

                //Preenche Dados Aluno
                if (dtDadosPrincipal.Rows.Count > 0)
                {
                    lblNome.Text = aluno + " - " + nomeAluno;
                    lblUnidade.Text = dtDadosPrincipal.Rows[0]["UNIDADE"].ToString();
                    lblAno.Text = dtDadosPrincipal.Rows[0]["ANO"].ToString();
                    if (dtDadosPrincipal.Rows[0]["SEMESTRE"].ToString() == "0")
                    {
                        lblPeriodo.Text = "Anual";
                    }
                    else
                    {
                        lblPeriodo.Text = dtDadosPrincipal.Rows[0]["SEMESTRE"].ToString() + "o. Semestre";
                    }
                    lblEscolaridade.Text = dtDadosPrincipal.Rows[0]["CURSO"].ToString();
                    lblAnoEscolaridade.Text = dtDadosPrincipal.Rows[0]["SERIE"].ToString();
                    lblTurno.Text = dtDadosPrincipal.Rows[0]["TURNO"].ToString();
                    lblTurmaPrincipal.Text = dtDadosPrincipal.Rows[0]["TURMA"].ToString();
                }

                this.PreencheTelefoneResponsavel();
                var dtInicio = System.Configuration.ConfigurationSettings.AppSettings["InicioLancamento"];
                var dtFim = System.Configuration.ConfigurationSettings.AppSettings["FimLancamento"];               
                var semestreBloqueado = System.Configuration.ConfigurationSettings.AppSettings["SemestreBloqueado"];

                if (lblAno.Text != "2020" && lblAno.Text != "2021")
                {
                    dadosSituacaoFinal = rnBoletim.ObtemSituacaoFinalPor(ano, periodo, aluno, lblTurmaPrincipal.Text);

                    if (dadosSituacaoFinal.Rows.Count > 0)
                    {
                        lblSituacaoFinal.Text = dadosSituacaoFinal.Rows[0]["SITUACAO_FINAL"].ToString();
                    }

                    if (!lblSituacaoFinal.Text.IsNullOrEmptyOrWhiteSpace() || (!semestreBloqueado.IsNullOrEmptyOrWhiteSpace() &&  semestreBloqueado != periodo.ToString()) || (DateTime.Now.Date < Convert.ToDateTime(dtInicio.ToString()) || DateTime.Now.Date > Convert.ToDateTime(dtFim.ToString())) )
                    {
                        this.MontarBoletim();
                    }
                    else
                    {
                        lblMensagem.Text = "Querido(a) aluno(a), excepcionalmente, seu boletim ficará disponível somente a partir de " + Convert.ToDateTime(dtFim).AddDays(1).ToString("dd/MM/yyyy") + ".";

                        //lblMensagem.Text = "Querido aluno, suas notas estão sendo lançadas no Sistema e estará disponibilizado para consulta a partir do dia " + Convert.ToDateTime(dtFimAnual).AddDays(1).ToString("dd/MM/yyyy") + ".";

                        divPortaria.Visible = false;
                        divImprimir.Visible = false;
                        divMensagemSituacao2020.Visible = false;
                        divInformativo.Visible = false;
                    }

                }
                else
                {
                    if (DateTime.Now.Date < Convert.ToDateTime(dtInicio.ToString()) || DateTime.Now.Date > Convert.ToDateTime(dtFim.ToString()))
                    {
                        dadosSituacaoFinal = rnBoletim.ObtemSituacaoFinalPor(ano, periodo, aluno, lblTurmaPrincipal.Text);

                        if (dadosSituacaoFinal.Rows.Count > 0)
                        {
                            lblSituacaoFinal.Text = dadosSituacaoFinal.Rows[0]["SITUACAO_FINAL"].ToString();
                            divSituacaoFinal.Visible = true;
                        }

                        divPortaria.Visible = true;
                        divImprimir.Visible = true;
                        divMensagemSituacao2020.Visible = true;
                        divInformativo.Visible = true;

                        DateTime dataUltimaAtualizacao;
                        lblUltimaAtualizacao.Text = string.Empty;
                        lblTipoMaricula.Text = string.Empty;

                        dataUltimaAtualizacao = rnBoletim.ObtemDataUltimaAtualizacaoBancoPor();
                        lblUltimaAtualizacao.Text = dataUltimaAtualizacao != DateTime.MinValue ? string.Format("Última atualização da base realizada em: {0}.", dataUltimaAtualizacao.ToString("dd/MM/yyyy - HH:mm")) : "Em tempo Real";
                        lblTipoMaricula.Text = string.Format(rnBoletim.ObtemSituacaoMatricula(aluno, ano, periodo));
                    }
                    else
                    {
                        //lblMensagem.Text = "O BOLETIM DO ALUNO ESTARÁ DISPONÍVEL NO DIA 22/12.";
                        divPortaria.Visible = false;
                        divImprimir.Visible = false;
                        divMensagemSituacao2020.Visible = false;
                        divInformativo.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                this.lblMensagem.Text = ex.Message;
            }
        }

        private void MontarBoletim()
        {
            //possibilita o distinct do datatable em uma unica turma
            this.divGrdBoletim.Visible = true;
            string html = string.Empty;
            int media = 5;
            string nota = string.Empty;
            int? falta = null;
            decimal frequenciaGlobalTurma = 0;
            string frequencia = string.Empty;
            int quantidadeBimestres = 0;

            try
            {
                RN.Boletim rnBoletim = new Techne.Lyceum.RN.Boletim();
                DataTable geral = new DataTable();
                DataTable porTurma = new DataTable();
                DataTable dadosSituacaoFinal = new DataTable();
                string nomeGrid = string.Empty;
                DateTime dataUltimaAtualizacao;
                lblUltimaAtualizacao.Text = string.Empty;
                lblTipoMaricula.Text = string.Empty;
                //Busca informaçoes do aluno
                string aluno = this.AlunoAutenticado.Matricula;
                int ano = Convert.ToInt32(hdnAno.Value);
                int periodo = Convert.ToInt32(hdnSemestre.Value);
                string turma = lblTurmaPrincipal.Text;

                //Busca boletim do aluno
                if (Convert.ToBoolean(hdnMatriculaAtiva.Value))
                {
                    geral = rnBoletim.ListaBoletimConsolidadoBimestralPor(ano, periodo, aluno);
                    frequenciaGlobalTurma = rnBoletim.ObtemFrequenciaGlobalAtualPor(ano, periodo, aluno, turma);

                }
                else if (Convert.ToBoolean(hdnMatriculaHistorico.Value))
                {
                    geral = rnBoletim.ListaBoletimConsolidadoBimestralHistoricoPor(ano, periodo, aluno);

                    //Busca Situação final
                    this.divSituacaoFinal.Visible = true;
                    dadosSituacaoFinal = rnBoletim.ObtemSituacaoFinalPor(ano, periodo, aluno, turma);

                    if (dadosSituacaoFinal.Rows.Count > 0)
                    {
                        lblSituacaoFinal.Text = dadosSituacaoFinal.Rows[0]["SITUACAO_FINAL"].ToString();
                        frequenciaGlobalTurma = Convert.ToDecimal(dadosSituacaoFinal.Rows[0]["FREQUENCIA_GLOBAL"].ToString());
                    }
                }

                //Busca data da ultima atualização do banco

                dataUltimaAtualizacao = rnBoletim.ObtemDataUltimaAtualizacaoBancoPor();
                lblUltimaAtualizacao.Text = dataUltimaAtualizacao != DateTime.MinValue ? string.Format("Última atualização da base realizada em: {0}.", dataUltimaAtualizacao.ToString("dd/MM/yyyy - HH:mm")) : "Em tempo Real";

                lblTipoMaricula.Text = string.Format(rnBoletim.ObtemSituacaoMatricula(aluno, ano, periodo));

                //lblTipoMaricula.Text = string.Format("Matrícula Ativa às xx:xx do dia {0}", dataUltimaAtualizacao.ToString("dd/MM/yyyy - HH:mm"));

                //Monta div principal com css
                html = "<div class=\"tabela-padrao-principal\" ><div class=\"tabela-padrao\" >";

                if (geral.Rows.Count > 0)
                {
                    //inicia a montagem da table para cada turma
                    foreach (DataRow item in geral.DefaultView.ToTable(true, "turma").Rows)
                    {

                        html += "<div class=\"tabela-cabecalho\" >" +
                          "<img class=\"aba-direita\" />" +
                          "<div class=\"centro\">" +
                          "<img src=\"../Images/tabela_cabecalho_grid.png\" /></div>" +
                          "<div class=\"aba-esquerda\">" +
                          "</div> " +
                          "<span> TURMA : " + item["TURMA"].ToString() + " </span> " +
                          "</div>";

                        //Adiciona Cabeçalho                 
                        if (ano >= 2025 && periodo == 0)
                        {
                            html += @"<table> <thead>" +
                              "<tr><td class=\"centralizado-micro-titulo\">Disciplina</td>" +
                              "<td class=\"centralizado-micro-titulo\">Nota<br />1º Tri</td>" +
                              "<td class=\"centralizado-micro-titulo\">Faltas<br />1º Tri</td>" +
                              "<td class=\"centralizado-micro-titulo\">Frequência<br />1º Tri(%)</td>" +
                              "<td class=\"centralizado-micro-titulo\">Nota<br />2º Tri</td>" +
                              "<td class=\"centralizado-micro-titulo\">Faltas<br />2º Tri</td>" +
                              "<td class=\"centralizado-micro-titulo\">Frequência<br />2º Tri(%)</td>";
                        }
                        else
                        {
                            html += @"<table> <thead>" +
                                "<tr><td class=\"centralizado-micro-titulo\">Disciplina</td>" +
                                "<td class=\"centralizado-micro-titulo\">Nota<br />1º Bim</td>" +
                                "<td class=\"centralizado-micro-titulo\">Faltas<br />1º Bim</td>" +
                                "<td class=\"centralizado-micro-titulo\">Frequência<br />1º Bim(%)</td>" +
                                "<td class=\"centralizado-micro-titulo\">Nota<br />2º Bim</td>" +
                                "<td class=\"centralizado-micro-titulo\">Faltas<br />2º Bim</td>" +
                                "<td class=\"centralizado-micro-titulo\">Frequência<br />2º Bim(%)</td>";
                        }

                        //Verifica se o boletim é de turmas anuais
                        if (periodo == 0)
                        {
                            if (ano < 2025)
                            {
                                hdnQuantidadeBimestres.Value = "4";

                                html += "<td class=\"centralizado-micro-titulo\">Nota<br />3º Bim</td>" +
                                   "<td class=\"centralizado-micro-titulo\">Faltas<br />3º Bim</td>" +
                                   "<td class=\"centralizado-micro-titulo\">Frequência<br />3º Bim(%)</td>" +
                                   "<td class=\"centralizado-micro-titulo\">Nota<br />4º Bim</td>" +
                                   "<td class=\"centralizado-micro-titulo\">Faltas<br />4º Bim</td>" +
                                   "<td class=\"centralizado-micro-titulo\">Frequência<br />4º Bim(%)</td>";
                            }
                            else
                            {
                                hdnQuantidadeBimestres.Value = "3";

                                html += "<td class=\"centralizado-micro-titulo\">Nota<br />3º Tri</td>" +
                                   "<td class=\"centralizado-micro-titulo\">Faltas<br />3º Tri</td>" +
                                   "<td class=\"centralizado-micro-titulo\">Frequência<br />3º Tri(%)</td>";
                            }

                        }

                        else
                        {
                            hdnQuantidadeBimestres.Value = "2";
                        }

                        html += "<td class=\"centralizado-micro-titulo\">Notas<br />Acum.</td>" +
                               "<td class=\"centralizado-micro-titulo\">Frequência<br />Acum.(%)</td></tr></thead>";
                        quantidadeBimestres = Convert.ToInt32(hdnQuantidadeBimestres.Value);
                        //Monta linhas para cada disciplina da turma
                        foreach (DataRow boletimVal in geral.Select("TURMA ='" + item["TURMA"].ToString() + "'"))
                        {
                            html += "<tbody><tr class=\"classeZebrado\"><td class=\"centralizado-micro-disciplina\">" + boletimVal["NOMEDISCIPLINA"].ToString() + " </td><td class=\"centralizado-micro\"";

                            nota = string.Empty;
                            if (boletimVal["NOTA1"] != DBNull.Value)
                            {
                                nota = boletimVal["NOTA1"].ToString().Replace('.', ',');
                                if (Convert.ToDecimal(nota) < media)
                                {
                                    html += " style=\" color: red\"";
                                }
                            }

                            if (boletimVal["FALTAS1"] != DBNull.Value)
                            {
                                falta = Convert.ToInt32(boletimVal["FALTAS1"]);
                            }
                            else
                            {
                                falta = null;
                            }

                            html += ">" + nota + "</td><td class=\"centralizado-micro\">" + Convert.ToString(falta) +
                            "</td><td class=\"centralizado-micro\" style=\" border-right: 2px solid Gray; ";

                            frequencia = string.Empty;
                            if (boletimVal["PERCENTUALFREQUENCIA1"] != DBNull.Value)
                            {
                                frequencia = boletimVal["PERCENTUALFREQUENCIA1"].ToString().Replace('.', ',');
                                if (Convert.ToDecimal(frequencia) < 75)
                                {
                                    html += " color: red ";
                                }
                            }
                            html += "\">" + frequencia + "</td><td class=\"centralizado-micro\"";

                            nota = string.Empty;
                            if (boletimVal["NOTA2"] != DBNull.Value)
                            {
                                nota = boletimVal["NOTA2"].ToString().Replace('.', ',');
                                if (Convert.ToDecimal(nota) < media)
                                {
                                    html += " style=\" color: red\"";
                                }
                            }

                            if (boletimVal["FALTAS2"] != DBNull.Value)
                            {
                                falta = Convert.ToInt32(boletimVal["FALTAS2"]);
                            }
                            else
                            {
                                falta = null;
                            }

                            //if (ano < 2025)

                            html += ">" + nota + "</td><td class=\"centralizado-micro\">" + Convert.ToString(falta) +
                            "</td><td class=\"centralizado-micro\" style=\" border-right: 2px solid Gray;  ";

                            frequencia = string.Empty;
                            if (boletimVal["PERCENTUALFREQUENCIA2"] != DBNull.Value)
                            {
                                frequencia = boletimVal["PERCENTUALFREQUENCIA2"].ToString().Replace('.', ',');
                                if (Convert.ToDecimal(frequencia) < 75)
                                {
                                    html += " color: red ";
                                }
                            }
                            html += "\">" + frequencia;

                            //Verifica se o boletim é de turmas anuais
                            if (periodo == 0)
                            {
                                html += "</td><td class=\"centralizado-micro\"";
                                nota = string.Empty;
                                if (boletimVal["NOTA3"] != DBNull.Value)
                                {
                                    nota = boletimVal["NOTA3"].ToString().Replace('.', ',');
                                    if (Convert.ToDecimal(nota) < media)
                                    {
                                        html += " style=\" color: red\"";
                                    }
                                }

                                if (boletimVal["FALTAS3"] != DBNull.Value)
                                {
                                    falta = Convert.ToInt32(boletimVal["FALTAS3"]);
                                }
                                else
                                {
                                    falta = null;
                                }

                                html += ">" + nota + "</td><td class=\"centralizado-micro\">" + Convert.ToString(falta) +
                                "</td><td class=\"centralizado-micro\" style=\" border-right: 2px solid Gray; ";

                                frequencia = string.Empty;
                                if (boletimVal["PERCENTUALFREQUENCIA3"] != DBNull.Value)
                                {
                                    frequencia = boletimVal["PERCENTUALFREQUENCIA3"].ToString().Replace('.', ',');
                                    if (Convert.ToDecimal(frequencia) < 75)
                                    {
                                        html += " color: red ";
                                    }
                                }

                                html += "\">" + frequencia;

                                if (ano < 2025)
                                {
                                    html += "</td><td class=\"centralizado-micro\"";

                                    nota = string.Empty;
                                    if (boletimVal["NOTA4"] != DBNull.Value)
                                    {
                                        nota = boletimVal["NOTA4"].ToString().Replace('.', ',');
                                        if (Convert.ToDecimal(nota) < media)
                                        {
                                            html += " style=\" color: red\"";
                                        }
                                    }

                                    if (boletimVal["FALTAS4"] != DBNull.Value)
                                    {
                                        falta = Convert.ToInt32(boletimVal["FALTAS4"]);
                                    }
                                    else
                                    {
                                        falta = null;
                                    }
                                }

                                if (ano < 2025)
                                {
                                    html += ">" + nota + "</td><td class=\"centralizado-micro\">" + Convert.ToString(falta) +
                                        "</td><td class=\"centralizado-micro\" style=\" border-right: 2px solid Gray;";
                                }
                                frequencia = string.Empty;
                                if (ano < 2025)
                                {

                                    if (boletimVal["PERCENTUALFREQUENCIA4"] != DBNull.Value)
                                    {
                                        frequencia = boletimVal["PERCENTUALFREQUENCIA4"].ToString().Replace('.', ',');
                                        if (Convert.ToDecimal(frequencia) < 75)
                                        {
                                            html += " color: red ";
                                        }
                                    }
                                    html += "\">" + frequencia;
                                }
                            }

                            html += "</td><td class=\"centralizado-micro-totais\"";

                            nota = string.Empty;
                            if (boletimVal["NOTASACUMULADAS"] != DBNull.Value)
                            {
                                nota = boletimVal["NOTASACUMULADAS"].ToString().Replace('.', ',');
                                if (Convert.ToDecimal(nota) < (media * quantidadeBimestres))
                                {
                                    html += " style=\" color: red\"";
                                }
                            }
                            if (periodo == 0)
                            {
                                if (ano < 2025)
                                {
                                    if (boletimVal["NOTA1"] == DBNull.Value && boletimVal["NOTA2"] == DBNull.Value && boletimVal["NOTA3"] == DBNull.Value && boletimVal["NOTA4"] == DBNull.Value)
                                    {
                                        nota = string.Empty;
                                    }
                                }
                                else
                                {
                                    if (boletimVal["NOTA1"] == DBNull.Value && boletimVal["NOTA2"] == DBNull.Value && boletimVal["NOTA3"] == DBNull.Value)
                                    {
                                        nota = string.Empty;
                                    }
                                }

                            }
                            else
                            {
                                if (boletimVal["NOTA1"] == DBNull.Value && boletimVal["NOTA2"] == DBNull.Value)
                                {
                                    nota = string.Empty;
                                }
                            }
                            html += " >" + nota +
                           "</td><td class=\"centralizado-micro-totais\"";


                            frequencia = string.Empty;
                            if (boletimVal["PERCENTUALFREQUENCIAACUMULADA"] != DBNull.Value)
                            {
                                frequencia = boletimVal["PERCENTUALFREQUENCIAACUMULADA"].ToString().Replace('.', ',');
                                if (Convert.ToDecimal(frequencia) < 75)
                                {
                                    html += " style=\" color: red\"";
                                }

                            }
                            html += ">" + frequencia + "</td></tr>";
                        }

                        //Monta linha com Frequencia Global somente da turma principal
                        if (turma == item["TURMA"].ToString())
                        {
                            html += "<tr><td class=\"tdFrequenciaGlobal\" colspan=\"" + Convert.ToString((quantidadeBimestres * 3) + 2) + "\">Frequência Global do Aluno na Turma:</td><td class=\"centralizado-micro-totais\" ";

                            if (frequenciaGlobalTurma < 75)
                            {
                                html += " style=\" color: red\"";
                            }

                            html += " >" + frequenciaGlobalTurma.ToString("0.00") + "</td></tr>";
                        }
                        html += "</tbody>";

                        //fecha a table
                        html += " </table> <br/>  <br/>";
                    }


                    //fecha a div
                    html += " </div></div>";

                    divGrdBoletim.InnerHtml = html;
                }
                else
                {
                    lblMensagem.Text = "Não existe boletim disponível para este aluno.";
                    divImprimir.Visible = false;
                }
            }
            catch (Exception ex)
            {
                this.lblMensagem.Text = ex.Message;
            }
        }
    }
}

