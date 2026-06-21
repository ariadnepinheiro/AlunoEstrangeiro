using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using System.Data;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.TurnosVagas
{
    public class ConfirmacaoOferta
    {
        public List<DTOs.DadosConfirmacaoVagasOferta> ObtemDadosConfirmacaoVagasOferta(int ano, string censo)
        {
            DataContext contexto = null;
            List<DTOs.DadosConfirmacaoVagasOferta> lista = new List<Techne.Lyceum.RN.DTOs.DadosConfirmacaoVagasOferta>();
            RN.Matricula rnMatricula = new Matricula();
            RN.Turma rnTurma = new Turma();

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                //Referencia para Matriculados: serão usados de acordo com cursos liberados para alunos responder pesquisa                 
                //Ensino Medio
                int anoReferenciaEnsinoMedio = ano - 1;
                int periodoReferenciaEnsinoMedio = 0;
                int serieReferenciaEnsinoMedio = 1;
                string cursoReferenciaEnsinoMedio = "0002.61";

                //Turnos: serão usados turnos de cursos em terminadade a proxima serie dos alunos da pesquisa
                int serieEM = 2; //Serie destino da oferta
                string cursoEM = "0002.31";
                int periodoEM = 0;
                List<string> censoDiesp = new List<string>();
                censoDiesp.Add("33111626");
                censoDiesp.Add("33111707");
                censoDiesp.Add("33124620");
                censoDiesp.Add("33131422");
                censoDiesp.Add("33136670");
                censoDiesp.Add("33170223");
                censoDiesp.Add("33170240");

                if (censoDiesp.Contains(censo))
                {
                    cursoEM = cursoReferenciaEnsinoMedio;
                    serieEM = serieReferenciaEnsinoMedio;
                }

                //Busca Turnos
                List<string> turnosEnsinoMedio = rnTurma.ListaTurnosPor(contexto, censo, anoReferenciaEnsinoMedio, periodoEM, serieEM, cursoEM, serieReferenciaEnsinoMedio, cursoReferenciaEnsinoMedio);

                //Verifica se a escola possui ensino medio
                if (turnosEnsinoMedio.Count > 0)
                {
                    //Monta Ensino Médio
                    DTOs.DadosConfirmacaoVagasOferta dadosEnsinoMedio = new Techne.Lyceum.RN.DTOs.DadosConfirmacaoVagasOferta();
                    dadosEnsinoMedio.Ano = ano;
                    dadosEnsinoMedio.Periodo = 0;
                    dadosEnsinoMedio.ModalidadeCurso = "Ensino Médio";
                    dadosEnsinoMedio.CursoReferencia = "0002.61";
                    dadosEnsinoMedio.SerieReferencia = 1;

                    //Verifica se foi finalizado
                    var finalizacao = ObtemDadosFinalizacaoOfertaPor(contexto, censo, ano, periodoEM);
                    if (finalizacao.Finalizado)
                    {                        
                        dadosEnsinoMedio.Finalizado = finalizacao.Finalizado;
                        dadosEnsinoMedio.UsuarioId = finalizacao.UsuarioId;
                        dadosEnsinoMedio.UsuarioNome = finalizacao.UsuarioNome;
                        dadosEnsinoMedio.Data = finalizacao.Data;

                        //Apos finalização busca numeros gravados
                        dadosEnsinoMedio.MatriculadosManha = finalizacao.MatriculadosManha;
                        dadosEnsinoMedio.MatriculadosTarde = finalizacao.MatriculadosTarde;
                        dadosEnsinoMedio.MatriculadosNoite = finalizacao.MatriculadosNoite;
                        dadosEnsinoMedio.MatriculadosIntegral = finalizacao.MatriculadosIntegral;
                        dadosEnsinoMedio.QuantidadeMatriculados = dadosEnsinoMedio.MatriculadosManha + dadosEnsinoMedio.MatriculadosTarde + dadosEnsinoMedio.MatriculadosNoite + dadosEnsinoMedio.MatriculadosIntegral;
                    }
                    else
                    {
                        //Enquanto não estiver finalizado busca numeros reais de matriculado

                        //BUSCA QUANTIDADE DE MATRICULADOS NOS CURSOS QUE PODEM RESPONDER PESQUISA DO ENSINO MEDIO   
                        dadosEnsinoMedio.MatriculadosManha = rnMatricula.ObtemQuantidadeMatriculadosPor(contexto, censo, anoReferenciaEnsinoMedio, periodoReferenciaEnsinoMedio, serieReferenciaEnsinoMedio, cursoReferenciaEnsinoMedio, "M");
                        dadosEnsinoMedio.MatriculadosTarde = rnMatricula.ObtemQuantidadeMatriculadosPor(contexto, censo, anoReferenciaEnsinoMedio, periodoReferenciaEnsinoMedio, serieReferenciaEnsinoMedio, cursoReferenciaEnsinoMedio, "T");
                        dadosEnsinoMedio.MatriculadosNoite = rnMatricula.ObtemQuantidadeMatriculadosPor(contexto, censo, anoReferenciaEnsinoMedio, periodoReferenciaEnsinoMedio, serieReferenciaEnsinoMedio, cursoReferenciaEnsinoMedio, "N");
                        dadosEnsinoMedio.MatriculadosIntegral = rnMatricula.ObtemQuantidadeMatriculadosPor(contexto, censo, anoReferenciaEnsinoMedio, periodoReferenciaEnsinoMedio, serieReferenciaEnsinoMedio, cursoReferenciaEnsinoMedio, "I");
                        dadosEnsinoMedio.QuantidadeMatriculados = dadosEnsinoMedio.MatriculadosManha + dadosEnsinoMedio.MatriculadosTarde + dadosEnsinoMedio.MatriculadosNoite + dadosEnsinoMedio.MatriculadosIntegral;
                    }

                    //Busca cursos
                    dadosEnsinoMedio.Ofertas = this.ObtemDadosConfirmacaoVagasOfertaPor(contexto, censo, ano, dadosEnsinoMedio.Periodo, turnosEnsinoMedio, true);

                    lista.Add(dadosEnsinoMedio);
                }

                //Referencia usados de acordo com cursos liberados para alunos responder pesquisa  
                //EJA 
                int anoReferenciaEJA = ano - 1;
                int periodoReferenciaEJA = 2;
                int serieReferenciaEJA = 2;
                string cursoReferenciaEJA = "0002.83";

                //Turnos: serão usados turnos de cursos em terminadade a proxima serie dos alunos da pesquisa
                int serieEJA = 3; //Serie destino da oferta
                string cursoEJA = "0002.44";
                int periodoEJA = 2;

                //Busca Turnos
                List<string> turnosEnsinoEJA = rnTurma.ListaTurnosPor(contexto, censo, anoReferenciaEJA, periodoEJA, serieEJA, cursoEJA, serieReferenciaEJA, cursoReferenciaEJA);

                //Verifica se a escola possui EJA
                if (turnosEnsinoEJA.Count > 0)
                {
                    //Monta EJA
                    DTOs.DadosConfirmacaoVagasOferta dadosEJA = new Techne.Lyceum.RN.DTOs.DadosConfirmacaoVagasOferta();
                    dadosEJA.Ano = ano;
                    dadosEJA.Periodo = 1;
                    dadosEJA.ModalidadeCurso = "Educação de Jovens e Adultos";
                    dadosEJA.CursoReferencia = "0002.83";
                    dadosEJA.SerieReferencia = 2;

                    //Verifica se foi finalizado
                    var finalizacao = ObtemDadosFinalizacaoOfertaPor(contexto, censo, ano, dadosEJA.Periodo);
                    if (finalizacao.Finalizado)
                    { 
                        dadosEJA.Finalizado = finalizacao.Finalizado;
                        dadosEJA.UsuarioId = finalizacao.UsuarioId;
                        dadosEJA.UsuarioNome = finalizacao.UsuarioNome;
                        dadosEJA.Data = finalizacao.Data;

                        //Apos finalização busca numeros gravados
                        dadosEJA.MatriculadosManha = finalizacao.MatriculadosManha;
                        dadosEJA.MatriculadosTarde = finalizacao.MatriculadosTarde;
                        dadosEJA.MatriculadosNoite = finalizacao.MatriculadosNoite;
                        dadosEJA.MatriculadosIntegral = finalizacao.MatriculadosIntegral;
                        dadosEJA.QuantidadeMatriculados = dadosEJA.MatriculadosManha + dadosEJA.MatriculadosTarde + dadosEJA.MatriculadosNoite + dadosEJA.MatriculadosIntegral;
                    }
                    else
                    {
                        //Enquanto não estiver finalizado busca numeros reais de matriculado

                        //BUSCA QUANTIDADE DE MATRICULADOS NOS CURSOS QUE PODEM RESPONDER PESQUISA DO EJA  
                        dadosEJA.MatriculadosManha = rnMatricula.ObtemQuantidadeMatriculadosPor(contexto, censo, anoReferenciaEJA, periodoReferenciaEJA, serieReferenciaEJA, cursoReferenciaEJA, "M");
                        dadosEJA.MatriculadosTarde = rnMatricula.ObtemQuantidadeMatriculadosPor(contexto, censo, anoReferenciaEJA, periodoReferenciaEJA, serieReferenciaEJA, cursoReferenciaEJA, "T");
                        dadosEJA.MatriculadosNoite = rnMatricula.ObtemQuantidadeMatriculadosPor(contexto, censo, anoReferenciaEJA, periodoReferenciaEJA, serieReferenciaEJA, cursoReferenciaEJA, "N");
                        dadosEJA.MatriculadosIntegral = rnMatricula.ObtemQuantidadeMatriculadosPor(contexto, censo, anoReferenciaEJA, periodoReferenciaEJA, serieReferenciaEJA, cursoReferenciaEJA, "I");
                        dadosEJA.QuantidadeMatriculados = dadosEJA.MatriculadosManha + dadosEJA.MatriculadosTarde + dadosEJA.MatriculadosNoite + dadosEJA.MatriculadosIntegral;
                    }
                    

                    //Busca cursos
                    dadosEJA.Ofertas = this.ObtemDadosConfirmacaoVagasOfertaPor(contexto, censo, ano, dadosEJA.Periodo, turnosEnsinoEJA, false);

                    lista.Add(dadosEJA);
                }
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                {
                    contexto.Dispose();
                }
            }
            return lista;
        }

        private List<DadosConfirmacaoVagasOfertaCurso> ObtemDadosConfirmacaoVagasOfertaPor(DataContext contexto, string censo, int ano, int periodo, List<string> turnosPossiveis, bool ensinoMedio)
        {
            List<DadosConfirmacaoVagasOfertaCurso> cursos = new List<DadosConfirmacaoVagasOfertaCurso>();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            StringBuilder sql = new StringBuilder();

            try
            {
                sql.Append(@" SELECT DISTINCT TE.CENSO,
	                                            C.CURSO,
	                                            C.NOME, 
	                                            C.MODALIDADE,
	                                            T.TRILHAAPRENDIZAGEMID,
	                                            CASE 
		                                            WHEN C.MODALIDADE = 'RE1' THEN 2
		                                            WHEN C.MODALIDADE = 'ED2' THEN 3
		                                            WHEN C.MODALIDADE = 'ED7' THEN 3
	                                            END SERIE,
	                                            VAGASMANHA,
	                                            VAGASTARDE,
	                                            VAGASNOITE,
	                                            VAGASINTEGRAL,
	                                            (SELECT COUNT(DISTINCT ALUNO) 
		                                            FROM [PEDAGOGICO].[TRILHAAPRENDIZAGEM_ALUNO] TA
		                                            WHERE TA.CURSOTRILHA = TE.CURSO
			                                              AND TA.CENSO = TE.CENSO
			                                              AND TA.ANOOFERTA = @ANO) AS OPTANTES,
	                                            COV.USUARIOID,
	                                            COV.CONFIRMACAOOFERTAID
                                            FROM [Pedagogico].[TRILHAAPRENDIZAGEM_ESCOLA] TE
												INNER JOIN LY_CURSO C ON TE.CURSO = C.CURSO
	                                            INNER JOIN [PEDAGOGICO].[TRILHAAPRENDIZAGEM] T ON C.TRILHAAPRENDIZAGEMID = T.TRILHAAPRENDIZAGEMID
	                                            INNER JOIN [PEDAGOGICO].[ITINERARIOFORMATIVO] I ON T.ITINERARIOFORMATIVOID = I.ITINERARIOFORMATIVOID
	                                            LEFT JOIN ( SELECT DISTINCT CO.CONFIRMACAOOFERTAID, 
														            ANO, 
														            PERIODO, 
														            CENSO, 
														            CURSOREFERENCIA, 
														            SERIEREFERENCIA, 
														            MATRICULADOSMANHA, 
														            MATRICULADOSTARDE, 
														            MATRICULADOSNOITE, 
														            MATRICULADOSINTEGRAL, 
														            FINALIZADO, 
														            CO.USUARIOID,
														            CONFIRMACAOOFERTAVAGASID,
														            CV.CURSO, 
														            SERIE, 
														            QUANTIDADEOPTANTES, 
														            VAGASMANHA, 
														            VAGASTARDE, 
														            VAGASNOITE, 
														            VAGASINTEGRAL
														FROM TURNOSVAGAS.CONFIRMACAOOFERTA CO
														    INNER JOIN TURNOSVAGAS.CONFIRMACAOOFERTAVAGAS CV 
																    ON CO.CONFIRMACAOOFERTAID = CV.CONFIRMACAOOFERTAID			
															    ) COV ON COV.ANO = @ANO
													                 AND COV.PERIODO = @PERIODO
													                 AND COV.CENSO = @CENSO	
																     AND COV.CURSO = TE.CURSO		                              
                                            WHERE TE.CENSO = @CENSO
												  AND TE.ANO = @ANO ");


                if (ensinoMedio)
                {
                    sql.Append("AND C.MODALIDADE = 'RE1'");
                }
                else
                {
                    sql.Append("AND C.MODALIDADE IN ('ED2', 'ED7') ");
                }

                contextQuery.Command = sql.ToString();

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    DadosConfirmacaoVagasOfertaCurso oferta = new DadosConfirmacaoVagasOfertaCurso();
                    oferta.ConfirmacaoOfertaId = reader["CONFIRMACAOOFERTAID"] != DBNull.Value ? Convert.ToInt32(reader["CONFIRMACAOOFERTAID"]) : 0;
                    oferta.Curso = Convert.ToString(reader["CURSO"]);
                    oferta.Serie = Convert.ToInt32(reader["SERIE"]);
                    oferta.NomeCurso = Convert.ToString(reader["NOME"]);

                    if (turnosPossiveis.Contains("M"))
                    {
                        oferta.VagasManha = reader["VAGASMANHA"] != DBNull.Value ? Convert.ToInt32(reader["VAGASMANHA"]) : 0;
                        oferta.HabilitaManha = true;
                    }
                    else
                    {
                        oferta.VagasManha = 0;
                        oferta.HabilitaManha = false;
                    }

                    if (turnosPossiveis.Contains("T"))
                    {
                        oferta.VagasTarde = reader["VAGASTARDE"] != DBNull.Value ? Convert.ToInt32(reader["VAGASTARDE"]) : 0;
                        oferta.HabilitaTarde = true;
                    }
                    else
                    {
                        oferta.VagasTarde = 0;
                        oferta.HabilitaTarde = false;
                    }

                    if (turnosPossiveis.Contains("N"))
                    {
                        oferta.VagasNoite = reader["VAGASNOITE"] != DBNull.Value ? Convert.ToInt32(reader["VAGASNOITE"]) : 0;
                        oferta.HabilitaNoite = true;
                    }
                    else
                    {
                        oferta.VagasNoite = 0;
                        oferta.HabilitaNoite = false;
                    }

                    if (turnosPossiveis.Contains("I"))
                    {
                        oferta.VagasIntegral = reader["VAGASINTEGRAL"] != DBNull.Value ? Convert.ToInt32(reader["VAGASINTEGRAL"]) : 0;
                        oferta.HabilitaIntegral = true;
                    }
                    else
                    {
                        oferta.VagasIntegral = 0;
                        oferta.HabilitaIntegral = false;
                    }

                    oferta.QuantidadeOptantes = Convert.ToInt32(reader["OPTANTES"]);

                    cursos.Add(oferta);
                }

                return cursos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        private DadosFinalizacaoOferta ObtemDadosFinalizacaoOfertaPor(DataContext contexto, string censo, int ano, int periodo)
        {
            DadosFinalizacaoOferta dadosFinalizacaoOferta = new DadosFinalizacaoOferta();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT TOP 1 C.FINALIZADO, 
											C.USUARIOID, 
											U.NOME, 
											C.DATAALTERACAO,
											C.MATRICULADOSMANHA,
											C.MATRICULADOSTARDE,
											C.MATRICULADOSNOITE,
											C.MATRICULADOSINTEGRAL
                                        FROM   [TurnosVagas].[CONFIRMACAOOFERTA] C (NOLOCK)  
											INNER JOIN HADES..HD_USUARIO U (NOLOCK) ON C.USUARIOID = U.USUARIO
                                        WHERE  ANO = @ANO
											AND PERIODO = @PERIODO
											AND CENSO = @CENSO
											AND FINALIZADO = 1
                                        ORDER BY DATAALTERACAO DESC ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, periodo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosFinalizacaoOferta.Finalizado = Convert.ToBoolean(reader["FINALIZADO"]);
                    if (dadosFinalizacaoOferta.Finalizado)
                    {
                        dadosFinalizacaoOferta.UsuarioId = Convert.ToString(reader["USUARIOID"]);
                        dadosFinalizacaoOferta.UsuarioNome = Convert.ToString(reader["NOME"]);
                        dadosFinalizacaoOferta.Data = Convert.ToDateTime(reader["DATAALTERACAO"]);
                        dadosFinalizacaoOferta.MatriculadosManha = Convert.ToInt32(reader["MATRICULADOSMANHA"]);
                        dadosFinalizacaoOferta.MatriculadosTarde = Convert.ToInt32(reader["MATRICULADOSTARDE"]);
                        dadosFinalizacaoOferta.MatriculadosNoite = Convert.ToInt32(reader["MATRICULADOSNOITE"]);
                        dadosFinalizacaoOferta.MatriculadosIntegral = Convert.ToInt32(reader["MATRICULADOSINTEGRAL"]);
                    }
                }

                return dadosFinalizacaoOferta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }               
            }
        }

        public ValidacaoDados Valida(List<DadosConfirmacaoVagasOferta> dadosConfirmacaoOferta, bool finalizar)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosConfirmacaoOferta == null)
            {
                return validacaoDados;
            }

            if (dadosConfirmacaoOferta.Count == 0)
            {
                mensagens.Add("Nenhum item enviado.");
            }
            else
            {
                foreach (DadosConfirmacaoVagasOferta modalidade in dadosConfirmacaoOferta)
                {
                    if (modalidade.Ano <= 0)
                    {
                        mensagens.Add("Campo ANO é obrigatório.");
                    }

                    if (modalidade.Periodo < 0)
                    {
                        mensagens.Add("Campo PERÍODO é obrigatório.");
                    }

                    if (modalidade.ModalidadeCurso.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo MODALIDADE é obrigatório.");
                    }

                    if (modalidade.CursoReferencia.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo CURSO REFERÊNCIA é obrigatório.");
                    }

                    if (modalidade.SerieReferencia <= 0)
                    {
                        mensagens.Add("Campo SÉRIE REFERÊNCIA é obrigatório.");
                    }

                    if (modalidade.MatriculadosManha < 0)
                    {
                        mensagens.Add("Campo QUANTIDADE DE MATRICULADOS MANHÃ é obrigatório.");
                    }

                    if (modalidade.MatriculadosTarde < 0)
                    {
                        mensagens.Add("Campo QUANTIDADE DE MATRICULADOS TARDE é obrigatório.");
                    }

                    if (modalidade.MatriculadosNoite < 0)
                    {
                        mensagens.Add("Campo QUANTIDADE DE MATRICULADOS NOITE é obrigatório.");
                    }

                    if (modalidade.MatriculadosIntegral < 0)
                    {
                        mensagens.Add("Campo QUANTIDADE DE MATRICULADOS INTEGRAL é obrigatório.");
                    }

                    if (modalidade.Ofertas.Count() == 0)
                    {
                        mensagens.Add(string.Format("Nenhum curso enviado para o {0}.", modalidade.ModalidadeCurso));
                    }
                    else
                    {
                        foreach (DadosConfirmacaoVagasOfertaCurso curso in modalidade.Ofertas)
                        {
                            if (curso.Curso.IsNullOrEmptyOrWhiteSpace())
                            {
                                mensagens.Add(string.Format("Campo CURSO é obrigatório para o {0}.", modalidade.ModalidadeCurso));
                            }
                            else
                            {
                                if (curso.Serie <= 0)
                                {
                                    mensagens.Add(string.Format("Campo SÉRIE é obrigatório para o curso {0} - {1}.", curso.Curso, curso.NomeCurso));
                                }

                                if (curso.HabilitaManha)
                                {
                                    if (curso.VagasManha < 0)
                                    {
                                        mensagens.Add(string.Format("Campo VAGAS MANHÃ é obrigatório para o curso {0} - {1}.", curso.Curso, curso.NomeCurso));
                                    }
                                }
                                else if (curso.VagasManha > 0)
                                {
                                    mensagens.Add(string.Format("Campo VAGAS MANHÃ não pode ser informado para o curso {0} - {1}.", curso.Curso, curso.NomeCurso));
                                }

                                if (curso.HabilitaTarde)
                                {
                                    if (curso.VagasTarde < 0)
                                    {
                                        mensagens.Add(string.Format("Campo VAGAS TARDE é obrigatório para o curso {0} - {1}.", curso.Curso, curso.NomeCurso));
                                    }
                                }
                                else if (curso.VagasTarde > 0)
                                {
                                    mensagens.Add(string.Format("Campo VAGAS TARDE não pode ser informado para o curso {0} - {1}.", curso.Curso, curso.NomeCurso));
                                }

                                if (curso.HabilitaNoite)
                                {
                                    if (curso.VagasNoite < 0)
                                    {
                                        mensagens.Add(string.Format("Campo VAGAS NOITE é obrigatório para o curso {0} - {1}.", curso.Curso, curso.NomeCurso));
                                    }
                                }
                                else if (curso.VagasNoite > 0)
                                {
                                    mensagens.Add(string.Format("Campo VAGAS NOITE não pode ser informado para o curso {0} - {1}.", curso.Curso, curso.NomeCurso));
                                }

                                if (curso.HabilitaIntegral)
                                {
                                    if (curso.VagasIntegral < 0)
                                    {
                                        mensagens.Add(string.Format("Campo VAGAS INTEGRAL é obrigatório para o curso {0} - {1}.", curso.Curso, curso.NomeCurso));
                                    }
                                }
                                else if (curso.VagasIntegral > 0)
                                {
                                    mensagens.Add(string.Format("Campo VAGAS INTEGRAL não pode ser informado para o curso {0} - {1}.", curso.Curso, curso.NomeCurso));
                                }

                                if (curso.QuantidadeOptantes < 0)
                                {
                                    mensagens.Add(string.Format("Campo QUANTIDADE DE OPTANTES é obrigatório para o curso {0} - {1}.", curso.Curso, curso.NomeCurso));
                                }
                            }
                        }

                        //Verifica se esta finalizando
                        if (finalizar)
                        {
                            //Busca totais de matriculados e vagas ofereridas
                            int totalMatriculados = modalidade.MatriculadosManha + modalidade.MatriculadosTarde + modalidade.MatriculadosNoite + modalidade.MatriculadosIntegral;
                            int totalVagas = modalidade.Ofertas.Sum(x => x.VagasManha) + modalidade.Ofertas.Sum(x => x.VagasTarde) + modalidade.Ofertas.Sum(x => x.VagasNoite) + modalidade.Ofertas.Sum(x => x.VagasIntegral);

                            //Caso seja finalizado ver se vai ter q chegar em 100% dos alunos matriculados na serie anterior
                            if (totalVagas != totalMatriculados)
                            {
                                mensagens.Add(string.Format("Para finalizar a modalidade {0}, deve atingir 100% dos alunos matriculados na série anterior.", modalidade.ModalidadeCurso));
                            }
                        }
                    }

                    if (modalidade.UsuarioId.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo USUÁRIO é obrigatório.");
                    }

                    modalidade.Finalizado = finalizar;
                }
            }

            //Verifica periodo
            DateTime dtInicio = Convert.ToDateTime(System.Configuration.ConfigurationSettings.AppSettings["DataInicioConfirmacaoOfertaEscola"]);
            DateTime dtFim = Convert.ToDateTime(System.Configuration.ConfigurationSettings.AppSettings["DataFimConfirmacaoOfertaEscola"]);
            if (DateTime.Now < dtInicio || DateTime.Now > dtFim)
            {
                mensagens.Add(string.Format("O período para Confirmação de Vagas Continuidade após Escolha dos Itinerários Formativos pela escola é de {0} até {1}.", dtInicio.ToString("dd/MM/yyyy HH:mm"), dtFim.ToString("dd/MM/yyyy HH:mm")));
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    int ano = dadosConfirmacaoOferta.Select(x => x.Ano).First();
                    string censo = dadosConfirmacaoOferta.Select(x => x.Censo).First();

                    //Verifica se já esta finalizado
                    if (this.EhFinalizadoPor(contexto, censo, ano))
                    {
                        mensagens.Add("Esta operação não pode ser realizada pois a escola já finalizou a confirmação.");
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
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void Salva(List<DadosConfirmacaoVagasOferta> dadosConfirmacaoOferta)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ConfirmacaoOfertaVagas rnConfirmacaoOfertaVagas = new ConfirmacaoOfertaVagas();

            try
            {
                foreach (DadosConfirmacaoVagasOferta modalidade in dadosConfirmacaoOferta)
                {
                    //Monta Entidade
                    Entidades.ConfirmacaoOferta confirmacaoOferta = new Entidades.ConfirmacaoOferta();
                    confirmacaoOferta.Ano = modalidade.Ano;
                    confirmacaoOferta.Periodo = modalidade.Periodo;
                    confirmacaoOferta.Censo = modalidade.Censo;
                    confirmacaoOferta.CursoReferencia = modalidade.CursoReferencia;
                    confirmacaoOferta.SerieReferencia = modalidade.SerieReferencia;
                    confirmacaoOferta.MatriculadosManha = modalidade.MatriculadosManha;
                    confirmacaoOferta.MatriculadosTarde = modalidade.MatriculadosTarde;
                    confirmacaoOferta.MatriculadosNoite = modalidade.MatriculadosNoite;
                    confirmacaoOferta.MatriculadosIntegral = modalidade.MatriculadosIntegral;
                    confirmacaoOferta.Finalizado = modalidade.Finalizado;
                    confirmacaoOferta.UsuarioId = modalidade.UsuarioId;

                    //Busca id caso já exista cadastro
                    confirmacaoOferta.ConfirmacaoOfertaId = this.ObtemConfirmacaoOfertaIdPor(contexto, confirmacaoOferta);

                    //Verifica se já existe cadastro
                    if (confirmacaoOferta.ConfirmacaoOfertaId > 0)
                    {
                        //Altera
                        this.Atualiza(contexto, confirmacaoOferta);

                        //Remove os cursos anteriores
                        rnConfirmacaoOfertaVagas.RemoveTodos(contexto, confirmacaoOferta.ConfirmacaoOfertaId);
                    }
                    else
                    {
                        //Insere
                        this.Insere(contexto, confirmacaoOferta);
                    }
                    foreach (DadosConfirmacaoVagasOfertaCurso curso in modalidade.Ofertas)
                    {
                        //Monta Entidade
                        Entidades.ConfirmacaoOfertaVagas confirmacaoOfertaVagas = new Entidades.ConfirmacaoOfertaVagas();
                        confirmacaoOfertaVagas.ConfirmacaoOfertaId = confirmacaoOferta.ConfirmacaoOfertaId;
                        confirmacaoOfertaVagas.Curso = curso.Curso;
                        confirmacaoOfertaVagas.Serie = curso.Serie;
                        confirmacaoOfertaVagas.VagasManha = curso.VagasManha;
                        confirmacaoOfertaVagas.VagasTarde = curso.VagasTarde;
                        confirmacaoOfertaVagas.VagasNoite = curso.VagasNoite;
                        confirmacaoOfertaVagas.VagasIntegral = curso.VagasIntegral;
                        confirmacaoOfertaVagas.QuantidadeOptantes = curso.QuantidadeOptantes;

                        //Insere
                        rnConfirmacaoOfertaVagas.Insere(contexto, confirmacaoOfertaVagas);
                    }
                }
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

        private int ObtemConfirmacaoOfertaIdPor(DataContext contexto, Entidades.ConfirmacaoOferta confirmacaoOferta)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;

            try
            {
                contextQuery.Command = @"  SELECT CONFIRMACAOOFERTAID
                                    FROM [TurnosVagas].[CONFIRMACAOOFERTA] (NOLOCK)
                                    WHERE ANO = @ANO
	                                    AND PERIODO = @PERIODO
	                                    AND CENSO = @CENSO
	                                    AND CURSOREFERENCIA = @CURSOREFERENCIA 
	                                    AND SERIEREFERENCIA = @SERIEREFERENCIA ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, confirmacaoOferta.Ano);
                contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, confirmacaoOferta.Periodo);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, confirmacaoOferta.Censo);
                contextQuery.Parameters.Add("@CURSOREFERENCIA", SqlDbType.VarChar, confirmacaoOferta.CursoReferencia);
                contextQuery.Parameters.Add("@SERIEREFERENCIA", SqlDbType.Int, confirmacaoOferta.SerieReferencia);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["CONFIRMACAOOFERTAID"]);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return retorno;
        }

        public bool EhFinalizadoPor(DataContext contexto, string censo, int ano)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                        FROM   [TurnosVagas].[CONFIRMACAOOFERTA] (NOLOCK) 
                                        WHERE  ANO = @ANO
											AND CENSO = @CENSO
											AND FINALIZADO = 1 ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private void Insere(DataContext contexto, Entidades.ConfirmacaoOferta confirmacaoOferta)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO TurnosVagas.CONFIRMACAOOFERTA
                                           (ANO
                                           ,PERIODO
                                           ,CENSO
                                           ,CURSOREFERENCIA
                                           ,SERIEREFERENCIA
                                           ,MATRICULADOSMANHA
                                           ,MATRICULADOSTARDE
                                           ,MATRICULADOSNOITE
                                           ,MATRICULADOSINTEGRAL
                                           ,FINALIZADO
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
                                     VALUES
                                           (@ANO, 
                                           @PERIODO, 
                                           @CENSO, 
                                           @CURSOREFERENCIA, 
                                           @SERIEREFERENCIA, 
                                           @MATRICULADOSMANHA, 
                                           @MATRICULADOSTARDE, 
                                           @MATRICULADOSNOITE, 
                                           @MATRICULADOSINTEGRAL, 
                                           @FINALIZADO, 
                                           @USUARIOID, 
                                           @DATACADASTRO, 
                                           @DATAALTERACAO)

                                SELECT IDENT_CURRENT('TurnosVagas.CONFIRMACAOOFERTA') ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, confirmacaoOferta.Ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, confirmacaoOferta.Periodo);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, confirmacaoOferta.Censo);
            contextQuery.Parameters.Add("@CURSOREFERENCIA", SqlDbType.VarChar, confirmacaoOferta.CursoReferencia);
            contextQuery.Parameters.Add("@SERIEREFERENCIA", SqlDbType.Int, confirmacaoOferta.SerieReferencia);
            contextQuery.Parameters.Add("@MATRICULADOSMANHA", SqlDbType.Int, confirmacaoOferta.MatriculadosManha);
            contextQuery.Parameters.Add("@MATRICULADOSTARDE", SqlDbType.Int, confirmacaoOferta.MatriculadosTarde);
            contextQuery.Parameters.Add("@MATRICULADOSNOITE", SqlDbType.Int, confirmacaoOferta.MatriculadosNoite);
            contextQuery.Parameters.Add("@MATRICULADOSINTEGRAL", SqlDbType.Int, confirmacaoOferta.MatriculadosIntegral);
            contextQuery.Parameters.Add("@FINALIZADO", SqlDbType.Bit, confirmacaoOferta.Finalizado);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, confirmacaoOferta.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            confirmacaoOferta.ConfirmacaoOfertaId = Convert.ToInt32(contexto.GetReturnValue(contextQuery));
        }

        private void Atualiza(DataContext contexto, Entidades.ConfirmacaoOferta confirmacaoOferta)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE TurnosVagas.CONFIRMACAOOFERTA
                                       SET ANO = @ANO, 
                                          PERIODO = @PERIODO, 
                                          CENSO = @CENSO, 
                                          CURSOREFERENCIA = @CURSOREFERENCIA, 
                                          SERIEREFERENCIA = @SERIEREFERENCIA, 
                                          MATRICULADOSMANHA = @MATRICULADOSMANHA, 
                                          MATRICULADOSTARDE = @MATRICULADOSTARDE, 
                                          MATRICULADOSNOITE = @MATRICULADOSNOITE, 
                                          MATRICULADOSINTEGRAL = @MATRICULADOSINTEGRAL, 
                                          FINALIZADO = @FINALIZADO, 
                                          USUARIOID = @USUARIOID, 
                                          DATAALTERACAO = @DATAALTERACAO 
                                     WHERE CONFIRMACAOOFERTAID = @CONFIRMACAOOFERTAID ";

            contextQuery.Parameters.Add("@CONFIRMACAOOFERTAID", SqlDbType.Int, confirmacaoOferta.ConfirmacaoOfertaId);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, confirmacaoOferta.Ano);
            contextQuery.Parameters.Add("@PERIODO", SqlDbType.Int, confirmacaoOferta.Periodo);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, confirmacaoOferta.Censo);
            contextQuery.Parameters.Add("@CURSOREFERENCIA", SqlDbType.VarChar, confirmacaoOferta.CursoReferencia);
            contextQuery.Parameters.Add("@SERIEREFERENCIA", SqlDbType.Int, confirmacaoOferta.SerieReferencia);
            contextQuery.Parameters.Add("@MATRICULADOSMANHA", SqlDbType.Int, confirmacaoOferta.MatriculadosManha);
            contextQuery.Parameters.Add("@MATRICULADOSTARDE", SqlDbType.Int, confirmacaoOferta.MatriculadosTarde);
            contextQuery.Parameters.Add("@MATRICULADOSNOITE", SqlDbType.Int, confirmacaoOferta.MatriculadosNoite);
            contextQuery.Parameters.Add("@MATRICULADOSINTEGRAL", SqlDbType.Int, confirmacaoOferta.MatriculadosIntegral);
            contextQuery.Parameters.Add("@FINALIZADO", SqlDbType.Bit, confirmacaoOferta.Finalizado);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, confirmacaoOferta.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }
    }
}