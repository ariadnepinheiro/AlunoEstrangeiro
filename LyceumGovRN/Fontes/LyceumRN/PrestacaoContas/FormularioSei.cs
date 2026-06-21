using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.Util;
using System.Data.SqlTypes;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class FormularioSei
    {
        public ValidacaoDados Valida(string censo, int periodoReferenciaId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            PeriodoReferencia rnPeriodoReferencia = new PeriodoReferencia();
            ExtratoBancario rnExtratoBancario = new ExtratoBancario();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo Unidade de Ensino não foi preenchido.");
            }

            if (periodoReferenciaId <= 0)
            {
                mensagens.Add("O campo Periodo de Referência não foi preenchido.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Busca Ultimo dia do periodo de Referencia
                    DateTime dataConsulta = rnPeriodoReferencia.ObtemUltimoDiaPor(contexto, periodoReferenciaId);
                    if (dataConsulta > DateTime.Now)
                    {
                        mensagens.Add(string.Format("Os Formulários deste periodo de refrência estarão disponvieis a após o dia {0}.", dataConsulta.ToString("dd/MM/yyyy")));
                    }
                    else
                    {
                        //Busca dados do periodo referencia
                        var periodoReferencia = rnPeriodoReferencia.ObtemPor(contexto, periodoReferenciaId);

                        //Obtem a lista de extratos bancários dessa unidade
                        var extratosBancarios = rnExtratoBancario.ObtemExtratoBancario(periodoReferencia.MesInicial, periodoReferencia.MesFinal, periodoReferencia.Ano, censo);

                        if (!extratosBancarios.Select("STATUS = 4").Any())
                        {
                            mensagens.Add("Este Período Referência não possui extratos Aprovados!");
                        }
                        else 
                        {
                            if (extratosBancarios.Select("STATUS IS NULL OR STATUS <> 4").Any())
                            {
                                mensagens.Add("Não foi possível Gerar o Formulário SEI. Verifique as análises dos extratos desse Período Referência");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (contexto != null)
                    {
                        contexto.Dispose();
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public DTOs.DadosFormulario1 ObtemDadosFormulario1Por(string censo, int periodoReferenciaId)
        {
            DTOs.DadosFormulario1 dadosDadosFormulario1 = new DTOs.DadosFormulario1();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            RN.UnidadeEnsino rnUnidadeEnsino = new UnidadeEnsino();
            RN.DTOs.DadosDiretor dadosDiretorFim = new Techne.Lyceum.RN.DTOs.DadosDiretor();
            RN.DTOs.DadosDiretor dadosDiretorInicio = new Techne.Lyceum.RN.DTOs.DadosDiretor();
            RN.PrestacaoContas.PeriodoReferencia rnPeriodoReferencia = new PeriodoReferencia();
            DTOs.DadosModalidadesEnsino dadosModalidadeEnsino = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosModalidadesEnsino();
            DTOs.DadosTesoureiro dadosTesoureiroFim = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosTesoureiro();
            DTOs.DadosTesoureiro dadosTesoureiroInicio = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosTesoureiro();
            RN.Docentes rnDocente = new Docentes();
            MandatoAae rnMandatoAae = new MandatoAae();
            DateTime dataConsulta;
            DateTime dataInicio;

            try
            {
                //Monta data para consulta, deve ser utilizada o ultimo dia no ano/mes final do periodo referencia                
                dataConsulta = rnPeriodoReferencia.ObtemUltimoDiaPor(contexto, periodoReferenciaId);

                //Monta data inicio, deve ser utilizada o primeiro dia no ano/mes inicial do periodo referencia
                dataInicio = rnPeriodoReferencia.ObtemPrimeiroDiaPor(contexto, periodoReferenciaId);

                //Busca Dados da Escola
                dadosDadosFormulario1 = rnUnidadeEnsino.ObtemDadosUnidadePor(contexto, censo, dataConsulta);

                //Busca dados da modalidade de ensino
                dadosModalidadeEnsino = rnUnidadeEnsino.ObtemDadosModalidadesEnsino(contexto, censo, dataConsulta);

                dadosDadosFormulario1.FundamentalModalidade = dadosModalidadeEnsino.FundamentalModalidade ? "X" : string.Empty;
                if (dadosModalidadeEnsino.FundamentalModalidade)
                {
                    dadosDadosFormulario1.FundamentalNumeroAlunos = dadosModalidadeEnsino.FundamentalNumeroAlunos.ToString();
                    dadosDadosFormulario1.FundamentalNumeroTurnos = dadosModalidadeEnsino.FundamentalNumeroTurnos.ToString();
                    dadosDadosFormulario1.FundamentalHorarioIntegralSim = dadosModalidadeEnsino.FundamentalHorarioIntegral ? "X" : string.Empty;
                    dadosDadosFormulario1.FundamentalHorarioIntegralNao = dadosModalidadeEnsino.FundamentalHorarioIntegral ? string.Empty : "X";
                }
                else
                {
                    dadosDadosFormulario1.FundamentalNumeroAlunos = " - ";
                    dadosDadosFormulario1.FundamentalNumeroTurnos = " - ";
                    dadosDadosFormulario1.FundamentalHorarioIntegralNao = "X";
                }

                dadosDadosFormulario1.MedioModalidade = dadosModalidadeEnsino.MedioModalidade ? "X" : string.Empty;
                if (dadosModalidadeEnsino.MedioModalidade)
                {
                    dadosDadosFormulario1.MedioNumeroAlunos = dadosModalidadeEnsino.MedioNumeroAlunos.ToString();
                    dadosDadosFormulario1.MedioNumeroTurnos = dadosModalidadeEnsino.MedioNumeroTurnos.ToString();
                    dadosDadosFormulario1.MedioHorarioIntegralSim = dadosModalidadeEnsino.MedioHorarioIntegral ? "X" : string.Empty;
                    dadosDadosFormulario1.MedioHorarioIntegralNao = dadosModalidadeEnsino.MedioHorarioIntegral ? string.Empty : "X";
                }
                else
                {
                    dadosDadosFormulario1.MedioNumeroAlunos = " - ";
                    dadosDadosFormulario1.MedioNumeroTurnos = " - ";
                    dadosDadosFormulario1.MedioHorarioIntegralNao = "X";
                }

                dadosDadosFormulario1.EjaModalidade = dadosModalidadeEnsino.EjaModalidade ? "X" : string.Empty;
                if (dadosModalidadeEnsino.EjaModalidade)
                {
                    dadosDadosFormulario1.EjaNumeroAlunos = dadosModalidadeEnsino.EjaNumeroAlunos.ToString();
                    dadosDadosFormulario1.EjaNumeroTurnos = dadosModalidadeEnsino.EjaNumeroTurnos.ToString();
                    dadosDadosFormulario1.EjaHorarioIntegralSim = dadosModalidadeEnsino.EjaHorarioIntegral ? "X" : string.Empty;
                    dadosDadosFormulario1.EjaHorarioIntegralNao = dadosModalidadeEnsino.EjaHorarioIntegral ? string.Empty : "X";
                }
                else
                {
                    dadosDadosFormulario1.EjaNumeroAlunos = " - ";
                    dadosDadosFormulario1.EjaNumeroTurnos = " - ";
                    dadosDadosFormulario1.EjaHorarioIntegralNao = "X";
                }

                dadosDadosFormulario1.EducacaoEspecialModalidade = dadosModalidadeEnsino.EducacaoEspecialModalidade ? "X" : string.Empty;
                if (dadosModalidadeEnsino.EducacaoEspecialModalidade)
                {
                    dadosDadosFormulario1.EducacaoEspecialNumeroAlunos = dadosModalidadeEnsino.EducacaoEspecialNumeroAlunos.ToString();
                    dadosDadosFormulario1.EducacaoEspecialNumeroTurnos = dadosModalidadeEnsino.EducacaoEspecialNumeroTurnos.ToString();
                    dadosDadosFormulario1.EducacaoEspecialHorarioIntegralSim = dadosModalidadeEnsino.EducacaoEspecialHorarioIntegral ? "X" : string.Empty;
                    dadosDadosFormulario1.EducacaoEspecialHorarioIntegralNao = dadosModalidadeEnsino.EducacaoEspecialHorarioIntegral ? string.Empty : "X";
                }
                else
                {
                    dadosDadosFormulario1.EducacaoEspecialNumeroAlunos = " - ";
                    dadosDadosFormulario1.EducacaoEspecialNumeroTurnos = " - ";
                    dadosDadosFormulario1.EducacaoEspecialHorarioIntegralNao = "X";
                }

                //Busca dados do diretor Atual
                dadosDiretorFim = rnUnidadeEnsino.ObtemDiretorPor(contexto, censo, dataConsulta);

                //Caso exista diretor alimenta campos
                if (!dadosDiretorFim.Matricula.IsNullOrEmptyOrWhiteSpace())
                {
                    //Atualiza campos do diretor
                    dadosDadosFormulario1.DiretorFimNome = dadosDiretorFim.Nome;
                    dadosDadosFormulario1.DiretorFimCpf = dadosDiretorFim.Cpf;
                    dadosDadosFormulario1.DiretorFimMatricula = dadosDiretorFim.Matricula;
                    dadosDadosFormulario1.DiretorFimIdFuncional = dadosDiretorFim.IdFuncional;
                    dadosDadosFormulario1.DiretorFimDataDO = (dadosDiretorFim.DataNomeacaoDo == null || dadosDiretorFim.DataNomeacaoDo == DateTime.MinValue) ? " - " : Convert.ToDateTime(dadosDiretorFim.DataNomeacaoDo).ToString("dd/MM/yyyy");
                }
                else
                {
                    dadosDadosFormulario1.DiretorFimNome = " - ";
                    dadosDadosFormulario1.DiretorFimCpf = " - ";
                    dadosDadosFormulario1.DiretorFimMatricula = " - ";
                    dadosDadosFormulario1.DiretorFimIdFuncional = " - ";
                    dadosDadosFormulario1.DiretorFimDataDO = " - ";
                }

                //Busca dados do diretor do Inicio
                dadosDiretorInicio = rnUnidadeEnsino.ObtemDiretorPor(contexto, censo, dataInicio);

                //Caso exista diretor alimenta campos
                if (!dadosDiretorInicio.Matricula.IsNullOrEmptyOrWhiteSpace())
                {
                    //Atualiza campos do diretor
                    dadosDadosFormulario1.DiretorInicioNome = dadosDiretorInicio.Nome;
                    dadosDadosFormulario1.DiretorInicioCpf = dadosDiretorInicio.Cpf;
                    dadosDadosFormulario1.DiretorInicioMatricula = dadosDiretorInicio.Matricula;
                    dadosDadosFormulario1.DiretorInicioIdFuncional = dadosDiretorInicio.IdFuncional;
                    dadosDadosFormulario1.DiretorInicioDataDO = (dadosDiretorInicio.DataNomeacaoDo == null || dadosDiretorInicio.DataNomeacaoDo == DateTime.MinValue) ? " - " : Convert.ToDateTime(dadosDiretorInicio.DataNomeacaoDo).ToString("dd/MM/yyyy");
                }
                else
                {
                    dadosDadosFormulario1.DiretorInicioNome = " - ";
                    dadosDadosFormulario1.DiretorInicioCpf = " - ";
                    dadosDadosFormulario1.DiretorInicioMatricula = " - ";
                    dadosDadosFormulario1.DiretorInicioIdFuncional = " - ";
                    dadosDadosFormulario1.DiretorInicioDataDO = " - ";
                }

                //Busca dados do mandato ativo
                var mandatoAaeFim = rnMandatoAae.ObtemMandatoAtivoPor(contexto, censo, dataConsulta);

                //Verifica se já tem mandato cadstrado
                if (mandatoAaeFim.MandatoAaeId > 0)
                {
                    //Verifica se tesoureiro é um servidor
                    if (mandatoAaeFim.TesoureiroId != null && mandatoAaeFim.TesoureiroId > 0)
                    {
                        //Busca dados do tesoureito
                        dadosTesoureiroFim = rnMandatoAae.ObtemTesoureiroExternoPor(contexto, mandatoAaeFim.MandatoAaeId);
                    }
                    else
                    {
                        //Busca dados do servidor
                        dadosTesoureiroFim = rnMandatoAae.ObtemTesoureiroServidorPor(contexto, mandatoAaeFim.MandatoAaeId);
                    }

                    //Atualiza dados do tesoureiro
                    dadosDadosFormulario1.TesoureiroFimNome = dadosTesoureiroFim.Nome;
                    dadosDadosFormulario1.TesoureiroFimCpf = dadosTesoureiroFim.Cpf.IsNullOrEmptyOrWhiteSpace() ? " - " : dadosTesoureiroFim.Cpf;
                    dadosDadosFormulario1.TesoureiroFimIdFuncional = dadosTesoureiroFim.IdFuncional.IsNullOrEmptyOrWhiteSpace() ? " - " : dadosTesoureiroFim.IdFuncional;

                    if (!dadosTesoureiroFim.IdFuncional.IsNullOrEmptyOrWhiteSpace())
                    {
                        string matricula = rnDocente.ObtemMatriculaPor(contexto, Convert.ToInt32(dadosTesoureiroFim.IdFuncional), censo, dataConsulta);
                        dadosDadosFormulario1.TesoureiroFimMatricula = matricula.IsNullOrEmptyOrWhiteSpace() ? " - " : matricula;
                    }
                    else
                    {
                        dadosDadosFormulario1.TesoureiroFimMatricula = " - ";
                    }
                }
                else
                {
                    dadosDadosFormulario1.TesoureiroFimNome = " - ";
                    dadosDadosFormulario1.TesoureiroFimCpf = " - ";
                    dadosDadosFormulario1.TesoureiroFimIdFuncional = " - ";
                    dadosDadosFormulario1.TesoureiroFimMatricula = " - ";
                }

                //Busca dados do mandato inicio
                var mandatoAaeInicio = rnMandatoAae.ObtemMandatoAtivoPor(contexto, censo, dataInicio);

                //Verifica se já tem mandato cadstrado
                if (mandatoAaeInicio.MandatoAaeId > 0)
                {
                    //Verifica se tesoureiro é um servidor
                    if (mandatoAaeInicio.TesoureiroId != null && mandatoAaeInicio.TesoureiroId > 0)
                    {
                        //Busca dados do tesoureito
                        dadosTesoureiroInicio = rnMandatoAae.ObtemTesoureiroExternoPor(contexto, mandatoAaeInicio.MandatoAaeId);
                    }
                    else
                    {
                        //Busca dados do servidor
                        dadosTesoureiroInicio = rnMandatoAae.ObtemTesoureiroServidorPor(contexto, mandatoAaeInicio.MandatoAaeId);
                    }

                    //Atualiza dados do tesoureiro
                    dadosDadosFormulario1.TesoureiroInicioNome = dadosTesoureiroInicio.Nome;
                    dadosDadosFormulario1.TesoureiroInicioCpf = dadosTesoureiroInicio.Cpf.IsNullOrEmptyOrWhiteSpace() ? " - " : dadosTesoureiroInicio.Cpf;
                    dadosDadosFormulario1.TesoureiroInicioIdFuncional = dadosTesoureiroInicio.IdFuncional.IsNullOrEmptyOrWhiteSpace() ? " - " : dadosTesoureiroInicio.IdFuncional;

                    if (!dadosTesoureiroInicio.IdFuncional.IsNullOrEmptyOrWhiteSpace())
                    {
                        string matricula = rnDocente.ObtemMatriculaPor(contexto, Convert.ToInt32(dadosTesoureiroInicio.IdFuncional), censo, dataInicio);
                        dadosDadosFormulario1.TesoureiroInicioMatricula = matricula.IsNullOrEmptyOrWhiteSpace() ? " - " : matricula;
                    }
                    else
                    {
                        dadosDadosFormulario1.TesoureiroInicioMatricula = " - ";
                    }
                }
                else
                {
                    dadosDadosFormulario1.TesoureiroInicioNome = " - ";
                    dadosDadosFormulario1.TesoureiroInicioCpf = " - ";
                    dadosDadosFormulario1.TesoureiroInicioIdFuncional = " - ";
                    dadosDadosFormulario1.TesoureiroInicioMatricula = " - ";
                }

                return dadosDadosFormulario1;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }
        }

        public DTOs.DadosFormulario2 ObtemDadosFormulario2APor(string censo, int periodoReferenciaId)
        {
            return this.ObtemDadosFormulario2Por(censo, periodoReferenciaId, 2, "12.306.0467.2421");
        }

        public DTOs.DadosFormulario2 ObtemDadosFormulario2BPor(string censo, int periodoReferenciaId)
        {
            return this.ObtemDadosFormulario2Por(censo, periodoReferenciaId, 1, "12.368.0441.2028");
        }        

        public List<DTOs.DadosFormulario2> ObtemDadosFormulario2CPor(string censo, int periodoReferenciaId)
        {
            List<DTOs.DadosFormulario2> lista = new List<DTOs.DadosFormulario2>();
            DTOs.DadosFormulario2 dados = new DTOs.DadosFormulario2();
            List<DTOs.DadosReceitasProjetoRecebidas> listaDadosReceitas = new List<Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosReceitasProjetoRecebidas>();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            PeriodoReferencia rnPeriodoReferencia = new PeriodoReferencia();
            Entidades.PeriodoReferencia periodoReferencia = new Techne.Lyceum.RN.PrestacaoContas.Entidades.PeriodoReferencia();
            PainelFinanceiro rnPainelFinanceiro = new PainelFinanceiro();
            Evento rnEvento = new Evento();
            DateTime dataInicio;
            DateTime dataFim;

            try
            {
                //Dados do periodo referencia  
                periodoReferencia = rnPeriodoReferencia.ObtemPor(contexto, periodoReferenciaId);

                dataInicio = new DateTime(periodoReferencia.Ano, periodoReferencia.MesInicial, 1);
                dataFim = new DateTime(periodoReferencia.Ano, periodoReferencia.MesFinal, DateTime.DaysInMonth(periodoReferencia.Ano, periodoReferencia.MesFinal));

                //Monta Periodo Referencia
                string periodoRef = string.Format("{0} - {1} / {2}",
                    Util.Utils.ObtemDescricaoMesPor(periodoReferencia.MesInicial),
                    Util.Utils.ObtemDescricaoMesPor(periodoReferencia.MesFinal),
                    periodoReferencia.Ano.ToString());

                //Busca Receitas
                //listaDadosReceitas = rnPainelFinanceiro.ObtemReceitasProgramaPor(contexto, censo, dataInicio, dataFim);
                listaDadosReceitas = rnPainelFinanceiro.ObtemReceitasProjetoPor(contexto, censo, dataInicio, dataFim, periodoReferenciaId);

                foreach (var dadosReceitas in listaDadosReceitas)
                {
                    dados = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosFormulario2();

                    dados.PeriodoPrestacao = periodoRef;

                    //Monta dados Receitas              
                    dados.SaldoAnterior = dadosReceitas.SaldoAnterior.ToString("c");
                    dados.RepassesRecebidos = dadosReceitas.Repasses.ToString("c");
                    dados.TotalDespesas = dadosReceitas.Despesas.ToString("c");
                    dados.CreditosDebitos = dadosReceitas.CreditosDebitos.ToString("c");
                    dados.ProgramaTrabalhoId = dadosReceitas.ProgramaTrabalhoId;
                    dados.Programa = dadosReceitas.Programa;
                    dados.PlanoTrabalhoId = dadosReceitas.PlanoTrabalhoId;
                    dados.PlanoTrabalho = dadosReceitas.PlanoTrabalho;
                    dados.Pt = dadosReceitas.Pt;
                    dados.PtRes = dadosReceitas.PtRes;

                    // SALDOINICIAL = SALDOANTERIOR + REPASESRECEBIDOS + CREDITOS/DEBITOS
                    decimal saldoInicial = dadosReceitas.SaldoAnterior + dadosReceitas.Repasses + dadosReceitas.CreditosDebitos;
                    dados.SaldoInicial = saldoInicial.ToString("c");

                    //SALDOFINAL = SALDOINICIAL - TOTALDESPESAS
                    decimal saldoFinal = saldoInicial - dadosReceitas.Despesas;
                    dados.SaldoFinal = saldoFinal.ToString("c");

                    //Busca eventos
                    List<DTOs.DadosDespesa> despesas = rnEvento.ObtemDadosDespesaProjetoPor(contexto, censo, dataInicio, dataFim, dadosReceitas.PlanoTrabalhoId);
                    dados.Despesas = despesas;

                    lista.Add(dados);
                }

                return lista;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }
        }

        private DTOs.DadosFormulario2 ObtemDadosFormulario2Por(string censo, int periodoReferenciaId, int finalidadeId, string pt)
        {
            DTOs.DadosFormulario2 dados = new DTOs.DadosFormulario2();
            DTOs.DadosReceitasRecebidas dadosReceitas = new DTOs.DadosReceitasRecebidas();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            PeriodoReferencia rnPeriodoReferencia = new PeriodoReferencia();
            Entidades.PeriodoReferencia periodoReferencia = new Techne.Lyceum.RN.PrestacaoContas.Entidades.PeriodoReferencia();
            PainelFinanceiro rnPainelFinanceiro = new PainelFinanceiro();
            Evento rnEvento = new Evento();
            DateTime dataInicio;
            DateTime dataFim;

            try
            {
                //Dados do periodo referencia  
                periodoReferencia = rnPeriodoReferencia.ObtemPor(contexto, periodoReferenciaId);

                dataInicio = new DateTime(periodoReferencia.Ano, periodoReferencia.MesInicial, 1);
                dataFim = new DateTime(periodoReferencia.Ano, periodoReferencia.MesFinal, DateTime.DaysInMonth(periodoReferencia.Ano, periodoReferencia.MesFinal));

                //Monta Periodo Referencia
                dados.PeriodoPrestacao = string.Format("{0} - {1} / {2}",
                    Util.Utils.ObtemDescricaoMesPor(periodoReferencia.MesInicial),
                    Util.Utils.ObtemDescricaoMesPor(periodoReferencia.MesFinal),
                    periodoReferencia.Ano.ToString());

                //Busca Receitas
                dadosReceitas = rnPainelFinanceiro.ObtemReceitasPor(contexto, censo, dataInicio, dataFim, finalidadeId, pt, periodoReferenciaId);

                //Monta dados Receitas              
                dados.SaldoAnterior = dadosReceitas.SaldoAnterior.ToString("c");
                dados.RepassesRecebidos = dadosReceitas.Repasses.ToString("c");
                dados.TotalDespesas = dadosReceitas.Despesas.ToString("c");
                dados.CreditosDebitos = dadosReceitas.CreditosDebitos.ToString("c");


                // SALDOINICIAL = SALDOANTERIOR + REPASESRECEBIDOS + CREDITOS/DEBITOS
                decimal saldoInicial = dadosReceitas.SaldoAnterior + dadosReceitas.Repasses + dadosReceitas.CreditosDebitos;
                dados.SaldoInicial = saldoInicial.ToString("c");

                //SALDOFINAL = SALDOINICIAL - TOTALDESPESAS
                decimal saldoFinal = saldoInicial - dadosReceitas.Despesas;
                dados.SaldoFinal = saldoFinal.ToString("c");

                dados.SaldoFinalComRendimento = (saldoFinal + dadosReceitas.Rendimentos).ToString("c");

                //Busca eventos
                List<DTOs.DadosDespesa> despesas = rnEvento.ObtemDadosDespesaPor(contexto, censo, dataInicio, dataFim, finalidadeId, pt);

                dados.Despesas = despesas;

                //Para manutentação busca valor de pequenas despesas em separado e não lista elas no grid
                if (finalidadeId == 1)
                {
                    List<DTOs.DadosDespesa> despesasComuns = new List<Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosDespesa>();
                    decimal totalPequenaDespesa = 0;

                    //Soma eventos que sejam de pequenas Despesas
                    foreach (DTOs.DadosDespesa item in despesas)
                    {
                        if (item.Evento.Contains("PD")) //Verifica se o evento é de sigla pequena despesa
                        {
                            totalPequenaDespesa += item.ValorCalculo;
                        }                        
                    }

                    dados.TotalPequenasDespesas = totalPequenaDespesa.ToString("c");
                    dados.TotalDespesas = (dadosReceitas.Despesas - totalPequenaDespesa).ToString("c"); //Diminui o total das pequenas despesas

               
                }
               
                return dados;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }
        }

        public DTOs.DadosFormulario5 ObtemDadosFormulario5Por(string censo, int periodoReferenciaId)
        {
            DTOs.DadosFormulario5 dados = new DTOs.DadosFormulario5();
            DTOs.DadosReceitasRecebidas dadosManutencao = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosReceitasRecebidas();
            DTOs.DadosReceitasRecebidas dadosMerenda = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosReceitasRecebidas();
            DTOs.DadosReceitasRecebidas dadosOutrosProjetos = new Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosReceitasRecebidas();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            PeriodoReferencia rnPeriodoReferencia = new PeriodoReferencia();
            Entidades.PeriodoReferencia periodoReferencia = new Techne.Lyceum.RN.PrestacaoContas.Entidades.PeriodoReferencia();
            PainelFinanceiro rnPainelFinanceiro = new PainelFinanceiro();
            DateTime dataInicio;
            DateTime dataFim;

            try
            {
                //Dados do periodo referencia  
                periodoReferencia = rnPeriodoReferencia.ObtemPor(contexto, periodoReferenciaId);

                dataInicio = new DateTime(periodoReferencia.Ano, periodoReferencia.MesInicial, 1);
                dataFim = new DateTime(periodoReferencia.Ano, periodoReferencia.MesFinal, DateTime.DaysInMonth(periodoReferencia.Ano, periodoReferencia.MesFinal));

                //Monta Periodo Referencia
                dados.PeriodoPrestacao = string.Format("{0} - {1} / {2}",
                    Util.Utils.ObtemDescricaoMesPor(periodoReferencia.MesInicial),
                    Util.Utils.ObtemDescricaoMesPor(periodoReferencia.MesFinal),
                    periodoReferencia.Ano.ToString());

                //Busca Receitas de Manutenção
                dadosManutencao = rnPainelFinanceiro.ObtemReceitasPor(contexto, censo, dataInicio, dataFim, 1, periodoReferenciaId);

                //Monta dados de manutenção
                dados.ManutencaoSaldoAnterior = dadosManutencao.SaldoAnterior.ToString("c");
                dados.ManutencaoRepasses = dadosManutencao.Repasses.ToString("c");
                dados.ManutencaoRendimentos = dadosManutencao.Rendimentos.ToString("c"); //Os rendimentos vao para manutenção
                dados.ManutencaoDevolucoes = dadosManutencao.Devolucoes.ToString("c");
                dados.ManutencaoDespesas = dadosManutencao.Despesas.ToString("c");
                dados.ManutencaoCreditosDebitos = dadosManutencao.CreditosDebitos.ToString("c");

                decimal manutencaoSaldoFinal = (dadosManutencao.SaldoAnterior + dadosManutencao.Repasses + dadosManutencao.Rendimentos + dadosManutencao.Devolucoes + dadosManutencao.CreditosDebitos) - dadosManutencao.Despesas;
                dados.ManutencaoSaldoFinal = manutencaoSaldoFinal.ToString("c");

                //Busca Receitas de Merenda
                dadosMerenda = rnPainelFinanceiro.ObtemReceitasPor(contexto, censo, dataInicio, dataFim, 2, periodoReferenciaId);

                //Monta dados de Merenda
                dados.MerendaSaldoAnterior = dadosMerenda.SaldoAnterior.ToString("c");
                dados.MerendaRepasses = dadosMerenda.Repasses.ToString("c");
                dados.MerendaRendimentos = " - "; //Os rendimentos vao para manutenção
                dados.MerendaDevolucoes = dadosMerenda.Devolucoes.ToString("c");
                dados.MerendaDespesas = dadosMerenda.Despesas.ToString("c");
                dados.MerendaCreditosDebitos = dadosMerenda.CreditosDebitos.ToString("c");

                decimal merendaSaldoFinal = (dadosMerenda.SaldoAnterior + dadosMerenda.Repasses + dadosMerenda.Devolucoes + dadosMerenda.CreditosDebitos) - dadosMerenda.Despesas;
                dados.MerendaSaldoFinal = merendaSaldoFinal.ToString("c");

                //Busca Receitas de Outros Projetos
                dadosOutrosProjetos = rnPainelFinanceiro.ObtemReceitasPor(contexto, censo, dataInicio, dataFim, 3, periodoReferenciaId);

                //Monta dados de Outros Projetos
                dados.OutrosProjetosSaldoAnterior = dadosOutrosProjetos.SaldoAnterior.ToString("c");
                dados.OutrosProjetosRepasses = dadosOutrosProjetos.Repasses.ToString("c");
                dados.OutrosProjetosRendimentos = " - ";//Os rendimentos vao para manutenção
                dados.OutrosProjetosDevolucoes = dadosOutrosProjetos.Devolucoes.ToString("c");
                dados.OutrosProjetosDespesas = dadosOutrosProjetos.Despesas.ToString("c");
                dados.OutrosProjetosCreditosDebitos = dadosOutrosProjetos.CreditosDebitos.ToString("c");

                decimal outrosProjetosSaldoFinal = (dadosOutrosProjetos.SaldoAnterior + dadosOutrosProjetos.Repasses + dadosOutrosProjetos.Devolucoes + dadosOutrosProjetos.CreditosDebitos) - dadosOutrosProjetos.Despesas;
                dados.OutrosProjetosSaldoFinal = outrosProjetosSaldoFinal.ToString("c");

                //Calcula Totais
                dados.TotalSaldoAnterior = (dadosManutencao.SaldoAnterior + dadosMerenda.SaldoAnterior + dadosOutrosProjetos.SaldoAnterior).ToString("c");
                dados.TotalRepasses = (dadosManutencao.Repasses + dadosMerenda.Repasses + dadosOutrosProjetos.Repasses).ToString("c");
                dados.TotalRendimentos = dadosManutencao.Rendimentos.ToString("c"); //Os rendimentos vao para manutenção
                dados.TotalDevolucoes = (dadosManutencao.Devolucoes + dadosMerenda.Devolucoes + dadosOutrosProjetos.Devolucoes).ToString("c");
                dados.TotalDespesas = (dadosManutencao.Despesas + dadosMerenda.Despesas + dadosOutrosProjetos.Despesas).ToString("c");
                dados.TotalSaldoFinal = (manutencaoSaldoFinal + merendaSaldoFinal + outrosProjetosSaldoFinal).ToString("c");
                dados.TotalCreditosDebitos = (dadosManutencao.CreditosDebitos + dadosMerenda.CreditosDebitos + dadosOutrosProjetos.CreditosDebitos).ToString("c");

                return dados;

            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }
        }

        public List<DTOs.DadosFormularioFornecedor> ObtemDadosFormularioFornecedorPor(string censo, int periodoReferenciaId)
        {
            List<DTOs.DadosFormularioFornecedor> lista = new List<Techne.Lyceum.RN.PrestacaoContas.DTOs.DadosFormularioFornecedor>();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            PeriodoReferencia rnPeriodoReferencia = new PeriodoReferencia();
            Entidades.PeriodoReferencia periodoReferencia = new Techne.Lyceum.RN.PrestacaoContas.Entidades.PeriodoReferencia();
            Fornecedor rnFornecedor = new Fornecedor();
            DateTime dataInicio;
            DateTime dataFim;

            try
            {
                //Dados do periodo referencia  
                periodoReferencia = rnPeriodoReferencia.ObtemPor(contexto, periodoReferenciaId);

                dataInicio = new DateTime(periodoReferencia.Ano, periodoReferencia.MesInicial, 1);
                dataFim = new DateTime(periodoReferencia.Ano, periodoReferencia.MesFinal, DateTime.DaysInMonth(periodoReferencia.Ano, periodoReferencia.MesFinal));

                lista = rnFornecedor.ObtemDadosFormularioFornecedorPor(contexto, censo, dataInicio, dataFim);

                return lista;

            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }
        }

        public void Insere(string censo, int periodoReferenciaId, string usuarioId)
        {
            ImportacaoSei rnImportacaoSei = new ImportacaoSei();
            rnImportacaoSei.Insere(censo, periodoReferenciaId, usuarioId);
        }
    }
}
