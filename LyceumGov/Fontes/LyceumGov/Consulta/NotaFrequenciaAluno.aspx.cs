using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Techne.Web;
using System.Data;

namespace Techne.Lyceum.Net.Consulta
{
    [NavUrl("~/Consulta/NotaFrequenciaAluno.aspx"),
    ControlText("Notas e Frequência do Aluno"),
    Title("Notas e Frequência do Aluno")]
    public partial class NotaFrequenciaAluno : TPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMensagem.Text = string.Empty;             

            if (!IsPostBack)
            {
                lblAnoBusca.Text = DateTime.Now.Year.ToString();
            }
        }

        protected void tseAluno_Changed(object sender, EventArgs args)
        {
            try
            {
                pnDadosAluno.Visible = false;
                pnEntumacao.Visible = false;
                LimpaTela();

                if (!tseAluno.DBValue.IsNull)
                {
                    if (!tseAluno.IsValidDBValue)
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

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (ValidaBusca())
            {
                pnDadosAluno.Visible = true;
                this.CarregaDadosAluno();
            }
            else
            {
                pnDadosAluno.Visible = false;
                pnEntumacao.Visible = false;
                LimpaTela();
            }
        }

        public bool ValidaBusca()
        {
            bool retorno = true;
            List<string> mensagens = new List<string>();

            if (string.IsNullOrEmpty(Convert.ToString(tseAluno.DBValue)))
            {
                mensagens.Add("Favor preencher o campo Aluno.");
                retorno = false;
            }

            if (string.IsNullOrEmpty(lblAnoBusca.Text))
            {
                mensagens.Add("Favor preencher o campo Ano.");
                retorno = false;
            }

            if (string.IsNullOrEmpty(cmbPeriodoBusca.SelectedValue))
            {
                mensagens.Add("Favor preencher o campo Periodo.");
                retorno = false;
            }

            if (!retorno)
            {
                this.lblMensagem.Text = mensagens.Aggregate((x, y) => x + "<br />" + y);
            }

            return retorno;
        }

        private void CarregaDadosAluno()
        {
            RN.Boletim rnBoletim = new RN.Boletim();
            DataTable dtDadosPrincipal = new DataTable();
            int ano;
            int periodo;
            string turma;
            string aluno;
            pnEntumacao.Visible = false;
            lblAviso.Text = string.Empty;

            try
            {
                //Busca filtros da tela
                aluno = Convert.ToString(tseAluno.DBValue);
                ano = Convert.ToInt32(lblAnoBusca.Text);
                periodo = Convert.ToInt32(cmbPeriodoBusca.SelectedValue);

                //Busca matricula ativa do aluno
                dtDadosPrincipal = rnBoletim.ObtemDadosAlunoTurmaPrincipalAtivaPor(aluno, ano, periodo);

                //Preenche Dados Aluno
                if (dtDadosPrincipal.Rows.Count > 0)
                {                    
                    //Dados do aluno
                    lblRegional.Text = dtDadosPrincipal.Rows[0]["REGIONAL"].ToString();
                    lblMunicipio.Text = dtDadosPrincipal.Rows[0]["MUNICIPIO"].ToString();
                    lblCenso.Text = dtDadosPrincipal.Rows[0]["CENSO"].ToString();
                    lblEscola.Text = dtDadosPrincipal.Rows[0]["UNIDADE"].ToString();
                    lblMatricula.Text = dtDadosPrincipal.Rows[0]["ALUNO"].ToString();
                    lblNome.Text = dtDadosPrincipal.Rows[0]["NOMEALUNO"].ToString();
                    lblMae.Text = dtDadosPrincipal.Rows[0]["NOMEMAE"].ToString();
                    lblStatus.Text = dtDadosPrincipal.Rows[0]["SIT_ALUNO"].ToString();

                    if (!string.IsNullOrEmpty(Convert.ToString(dtDadosPrincipal.Rows[0]["DT_NASC"])))
                    {
                        lblDataNascimento.Text = Convert.ToDateTime(dtDadosPrincipal.Rows[0]["DT_NASC"]).ToString("dd/MM/yyyy");
                    }

                    //Verifica se está enturmado
                    if (!string.IsNullOrEmpty(Convert.ToString(dtDadosPrincipal.Rows[0]["TURMA"])))
                    {
                        pnEntumacao.Visible = true;
                        ano = Convert.ToInt32(dtDadosPrincipal.Rows[0]["ANO"]);
                        periodo = Convert.ToInt32(dtDadosPrincipal.Rows[0]["SEMESTRE"]);
                        turma = dtDadosPrincipal.Rows[0]["TURMA"].ToString();

                        lblAno.Text = ano.ToString();
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
                        lblTurmaPrincipal.Text = turma;

                        this.MontarBoletim(aluno, ano, periodo, turma);
                    }
                    else
                    {                        
                        this.lblAviso.Text = "Este aluno não possui enturmação ativa.";
                        return;
                    }
                }
                else
                {
                    this.LimpaTela();
                    this.lblMensagem.Text = "Aluno não encontrado.";
                    return;
                }
            }
            catch (Exception ex)
            {
                this.lblMensagem.Text = ex.Message;
            }
        }

        private void MontarBoletim(string aluno, int ano, int periodo, string turma)
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
                DataTable dtDadosPrincipal = new DataTable();
                DataTable geral = new DataTable();
                DataTable porTurma = new DataTable();
                DataTable dadosSituacaoFinal = new DataTable();
                string nomeGrid = string.Empty;

                geral = rnBoletim.ListaBoletimConsolidadoBimestralPor(ano, periodo, aluno);
                frequenciaGlobalTurma = rnBoletim.ObtemFrequenciaGlobalAtualPor(ano, periodo, aluno, turma);

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
                        html += "<table style=\"width: 100%;\">  <thead>" +
                               "<tr><td class=\"centralizado-micro-titulo\">Disciplina</td>" +
                               "<td class=\"centralizado-micro-titulo\">Nota<br />1º Bim</td>" +
                               "<td class=\"centralizado-micro-titulo\">Faltas<br />1º Bim</td>" +
                               "<td class=\"centralizado-micro-titulo\">Frequência<br />1º Bim(%)</td>" +
                               "<td class=\"centralizado-micro-titulo\">Nota<br />2º Bim</td>" +
                               "<td class=\"centralizado-micro-titulo\">Faltas<br />2º Bim</td>" +
                               "<td class=\"centralizado-micro-titulo\">Frequência<br />2º Bim(%)</td>";

                        //Verifica se o boletim é de turmas anuais
                        if (periodo == 0)
                        {
                            quantidadeBimestres = 4;

                            html += "<td class=\"centralizado-micro-titulo\">Nota<br />3º Bim</td>" +
                               "<td class=\"centralizado-micro-titulo\">Faltas<br />3º Bim</td>" +
                               "<td class=\"centralizado-micro-titulo\">Frequência<br />3º Bim(%)</td>" +
                               "<td class=\"centralizado-micro-titulo\">Nota<br />4º Bim</td>" +
                               "<td class=\"centralizado-micro-titulo\">Faltas<br />4º Bim</td>" +
                               "<td class=\"centralizado-micro-titulo\">Frequência<br />4º Bim(%)</td>";
                        }
                        else
                        {
                            quantidadeBimestres = 2;
                        }

                        html += "<td class=\"centralizado-micro-titulo\">Notas<br />Acum.</td>" +
                               "<td class=\"centralizado-micro-titulo\">Frequência<br />Acum.(%)</td></tr></thead>";

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
                                html += "\">" + frequencia + "</td><td class=\"centralizado-micro\"";

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

                                html += ">" + nota + "</td><td class=\"centralizado-micro\">" + Convert.ToString(falta) +
                                "</td><td class=\"centralizado-micro\" style=\" border-right: 2px solid Gray;";

                                frequencia = string.Empty;
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
                                if (boletimVal["NOTA1"] == DBNull.Value && boletimVal["NOTA2"] == DBNull.Value && boletimVal["NOTA3"] == DBNull.Value && boletimVal["NOTA4"] == DBNull.Value)
                                {
                                    nota = string.Empty;
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
                }
            }
            catch (Exception ex)
            {
                this.lblMensagem.Text = ex.Message;
            }
        }

        private void LimpaTela()
        {
            lblRegional.Text = string.Empty;
            lblMunicipio.Text = string.Empty;
            lblCenso.Text = string.Empty;
            lblEscola.Text = string.Empty;
            lblMatricula.Text = string.Empty;
            lblNome.Text = string.Empty;
            lblMae.Text = string.Empty;
            lblDataNascimento.Text = string.Empty;
            lblStatus.Text = string.Empty;
            divGrdBoletim.InnerHtml = string.Empty;
            lblAno.Text = string.Empty;
            lblPeriodo.Text = string.Empty;
            lblEscolaridade.Text = string.Empty;
            lblAnoEscolaridade.Text = string.Empty;
            lblTurno.Text = string.Empty;
            lblTurmaPrincipal.Text = string.Empty;
        }
    }
}
