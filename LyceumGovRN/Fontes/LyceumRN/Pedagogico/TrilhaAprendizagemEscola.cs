using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.Pedagogico
{
    public class TrilhaAprendizagemEscola
    {
        public bool PossuiTrilhaAprendizagemPor(DataContext ctx, int trilhaAprendizagemId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [Pedagogico].[TRILHAAPRENDIZAGEM_ESCOLA]
                                WHERE TRILHAAPRENDIZAGEMID = @TRILHAAPRENDIZAGEMID ";

            contextQuery.Parameters.Add("@TRILHAAPRENDIZAGEMID", SqlDbType.Int, trilhaAprendizagemId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PodeParticiparPor(string censo, int ano)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.PodeParticiparPor(contexto, censo, ano);
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

        private bool PodeParticiparPor(DataContext ctx, string censo, int ano)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;
            int anoReferencia = ano - 1;


            contextQuery.Command = @" SELECT COUNT(DISTINCT T.TURMA) AS TURMAS
                            FROM LY_TURMA T
	                            INNER JOIN LY_CURSO C ON T.CURSO = C.CURSO
                                inner JOIN LY_TURNO TU ON TU.TURNO = T.TURNO
                            WHERE ANO = @ANO	                           
								AND T.FACULDADE = @CENSO                     
                                AND SIT_TURMA = 'Aberta'
								AND (
									(MODALIDADE = 'RE1' AND SEMESTRE = 0 AND SERIE = 1 AND T.CURSO = '0002.61')
									OR (MODALIDADE IN ('ED2','ED7') AND SEMESTRE = 2 AND SERIE = 2 AND T.CURSO = '0002.83')
								) ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, anoReferencia);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public DataTable ListaPor(string censo, int ano)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT TE.TRILHAAPRENDIZAGEM_ESCOLAID,										   
                                           I.ITINERARIOFORMATIVOID,
                                           I.DESCRICAO AS ITINERARIO,
										   C.TRILHAAPRENDIZAGEMID,
	                                       T.DESCRICAO AS TRILHA,
                                           C.CURSO,
                                           TE.ANO,
                                           TU.DESCRICAO AS TURNO,
                                           TE.TURNO AS CODTURNO,
                                           TE.USUARIOID,
                                           TE.DATACADASTRO
                                    FROM   [Pedagogico].[TRILHAAPRENDIZAGEM_ESCOLA] (NOLOCK) TE	
                                    INNER JOIN LY_CURSO C ON C.CURSO = TE.CURSO
                                    INNER JOIN LY_TURNO TU ON TU.TURNO = TE.TURNO									
								    INNER JOIN  [Pedagogico].[TRILHAAPRENDIZAGEM] (NOLOCK) T ON T.TRILHAAPRENDIZAGEMID = C.TRILHAAPRENDIZAGEMID
                                    INNER JOIN  [Pedagogico].[ITINERARIOFORMATIVO] (NOLOCK) I ON I.ITINERARIOFORMATIVOID = T.ITINERARIOFORMATIVOID
									WHERE TE.CENSO = @CENSO
                                          AND TE.ANO = @ANO  ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Empty;
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
                ctx.Dispose();
            }

            return dt;
        }

        public ValidacaoDados Valida(Entidades.TrilhaAprendizagemEscola trilhaAprendizagemEscola, string modalidade, int categoriaItinerarioFormativoId)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.Curso rnCurso = new Curso();
            RN.Turma rnTurma = new Turma();
            TrilhaAprendizagem rnTrilhaAprendizagem = new TrilhaAprendizagem();
            RN.UnidadeFisica rnUnidadeFisica = new UnidadeFisica();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (trilhaAprendizagemEscola == null)
            {
                return validacaoDados;
            }

            if (modalidade.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo MODALIDADE é obrigatório.");
            }
            else if (modalidade != "ED2" && modalidade != "ED7" && modalidade != "RE1")
            {
                mensagens.Add("Apenas podem ser informadas MODALIDADES EJA ou Ensino Médio.");
            }

            if (categoriaItinerarioFormativoId < 0)
            {
                mensagens.Add("Campo CATEGORIA é obrigatório.");
            }

            if (trilhaAprendizagemEscola.Ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (trilhaAprendizagemEscola.Censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CENSO é obrigatório.");
            }

            if (trilhaAprendizagemEscola.Curso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TRILHA é obrigatório.");
            }

            if (trilhaAprendizagemEscola.Turno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo TURNO é obrigatório.");
            }

            if (trilhaAprendizagemEscola.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIOID é obrigatório.");
            }

            //Verifica periodo de oferta
            DateTime dtInicio = Convert.ToDateTime(System.Configuration.ConfigurationSettings.AppSettings["DataInicioOfertaEscola"]);
            DateTime dtFim = Convert.ToDateTime(System.Configuration.ConfigurationSettings.AppSettings["DataFimOfertaEscola"]);
            if (DateTime.Now < dtInicio || DateTime.Now > dtFim)
            {
                mensagens.Add(string.Format("O periodo para escolha de trilhas pela escola é de {0} até {1}.", dtInicio.ToString("dd/MM/yyyy HH:mm"), dtFim.ToString("dd/MM/yyyy HH:mm")));
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se escola pode participar, apenas escolas com EJA- Módulo III e Ensino Médio Regular 2ª série
                    if (this.PodeParticiparPor(contexto, trilhaAprendizagemEscola.Censo, trilhaAprendizagemEscola.Ano))
                    {
                        //Busca dados da trilha
                        string modalidadeTrilha = rnCurso.RetonaModalidadePor(contexto, trilhaAprendizagemEscola.Curso);
                        int quantidadeTurmas = rnTurma.RetornaQuantidadeTurmaOfertaPor(contexto, trilhaAprendizagemEscola.Ano, trilhaAprendizagemEscola.Censo, modalidade, trilhaAprendizagemEscola.Turno);
                        int quantidadeOfertasAprofundamento = this.RetornaQuantidadeOfertaPor(contexto, trilhaAprendizagemEscola.Censo, modalidade, trilhaAprendizagemEscola.Turno, "APROFUNDAMENTO", trilhaAprendizagemEscola.Ano);
                        int quantidadeOfertasProfissionalizante = this.RetornaQuantidadeOfertaPor(contexto, trilhaAprendizagemEscola.Censo, modalidade, trilhaAprendizagemEscola.Turno, "PROFISSIONALIZANTE", trilhaAprendizagemEscola.Ano);
                        int quantidadeOfertas = quantidadeOfertasAprofundamento; //+ quantidadeOfertasProfissionalizante;
                        string tipoTrilha = rnTrilhaAprendizagem.ObtemTipoTrilhaPor(contexto, trilhaAprendizagemEscola.Curso);

                        // Verifica se já existe
                        if (this.PossuiOutroCadastradoPor(contexto, trilhaAprendizagemEscola.Turno, trilhaAprendizagemEscola.Curso, trilhaAprendizagemEscola.Censo, trilhaAprendizagemEscola.TrilhaAprendizagemEscolaId, trilhaAprendizagemEscola.Ano))
                        {
                            mensagens.Add("Este ITINERÁRIO / TRILHA DE APRENDIZAGEM / TURNO já foi associado para esta escola.");
                        }
                        else
                        {
                            //Validaçoes para Itinerários EJA 
                            if (modalidade == "ED2" || modalidade == "ED7") //ED2	EDUCAÇÃO DE JOVENS E ADULTOS
                            {
                                if (modalidadeTrilha == "RE1")
                                {
                                    mensagens.Add("Fica vedado para esta modalidade de ensino as categorias referentes ao Ensino Médio Regular.");
                                }

                                //Verifica se é escola unica no municipio
                                if (trilhaAprendizagemEscola.Censo == "33004447" || trilhaAprendizagemEscola.Censo == "33005664"
                                    || trilhaAprendizagemEscola.Censo == "33016070" || trilhaAprendizagemEscola.Censo == "33016550"
                                    || trilhaAprendizagemEscola.Censo == "33017000" || trilhaAprendizagemEscola.Censo == "33023751"
                                    || trilhaAprendizagemEscola.Censo == "33027820" || trilhaAprendizagemEscola.Censo == "33030456"
                                    || trilhaAprendizagemEscola.Censo == "33031185" || trilhaAprendizagemEscola.Censo == "33032130"
                                    || trilhaAprendizagemEscola.Censo == "33042047" || trilhaAprendizagemEscola.Censo == "33093555")
                                {
                                    //Escolas únicas no Município com: 
                                    //4 turmas ou mais, no turno, poderão ofertar até 4 Trilhas de aprofundamento de qualquer categoria de Itinerários. 
                                    if (quantidadeTurmas >= 4)
                                    {
                                        //Verifica se já tem alguma trilha
                                        if (quantidadeOfertas >= 5)
                                        {
                                            mensagens.Add("Para Escolas únicas no Município com 4 turmas ou mais, no turno, apenas podem ser cadastradas cinco trilhas, e esta escola já cadastrou 5 trilhas para esta MODALIDADE / TURNO");
                                        }
                                    }
                                }
                                else
                                {
                                    //Escolas com 4 Turmas ou mais no turno:
                                    //Deverão ofertar até 4 Trilhas de aprofundamento de qualquer categoria de Itinerários ou 3 Trilhas de aprofundamento  
                                    //ou 2 Trilhas de aprofundamento 
                                    if (quantidadeTurmas >= 4)
                                    {
                                        //Verifica se já tem alguma trilha
                                        if (quantidadeOfertas >= 4)
                                        {
                                            mensagens.Add("Para Escolas com 4 Turmas ou mais no turno, podem ser cadastradas de 2 a 4 trilhas, e esta escola já cadastrou 4 trilhas para esta MODALIDADE / TURNO.");
                                        }

                                    }
                                }


                                //Escolas com Turmas únicas no turno:
                                //Deverão ofertar, somente, Trilha de aprofundamento de qualquer categoria de Itinerários de áreas ou Trilha Profissionalizante;
                                if (quantidadeTurmas == 1)
                                {
                                    //Verifica se já tem alguma trilha
                                    if (quantidadeOfertas >= 1)
                                    {
                                        mensagens.Add("Para Escolas com Turmas únicas apenas pode ser cadastrada uma trilha, e esta escola já cadastrou trilha para esta MODALIDADE / TURNO.");
                                    }
                                }

                                //Escolas com 2 Turmas no turno:
                                //Deverão ofertar 2 Trilhas de aprofundamento de qualquer categoria de Itinerários ou 1 Trilha de Aprofundamento e 1 Trilha Profissionalizante;
                                if (quantidadeTurmas == 2)
                                {
                                    //Verifica se já tem alguma trilha
                                    if (quantidadeOfertas >= 2)
                                    {
                                        mensagens.Add("Para Escolas com 2 Turmas no turno, apenas podem ser cadastradas duas trilhas, e esta escola já cadastrou 2 trilhas para esta MODALIDADE / TURNO.");
                                    }

                                }

                                //Escolas com 3 Turmas no turno:
                                //Deverão ofertar até 3 Trilhas de aprofundamento de qualquer categoria de Itinerários ou 2 Trilhas de aprofundamento e 1 Trilha Profissionalizante;
                                if (quantidadeTurmas == 3)
                                {
                                    //Verifica se já tem alguma trilha
                                    if (quantidadeOfertas >= 3)
                                    {
                                        mensagens.Add("Para Escolas com 3 Turmas no turno, podem ser cadastradas 2 a 3 trilhas, e esta escola já cadastrou 3 trilhas para esta MODALIDADE / TURNO.");
                                    }

                                }


                            }
                            //Validaçoes para Itinerários EMR
                            else if (modalidade == "RE1") //RE1	REGULAR
                            {
                                //Apenas podem ofertar Trilhas de aprofundamento
                                if (tipoTrilha != "APROFUNDAMENTO")
                                {
                                    mensagens.Add("Esta modalidade apenas pode ofertar Trilhas de aprofundamento.");
                                }

                                //Verifica se é escola rural //Itinerários Educação do Campo
                                if (rnUnidadeFisica.EhEscolaRuralPor(contexto, trilhaAprendizagemEscola.Censo))
                                {
                                    if (modalidadeTrilha == "ED2" || modalidadeTrilha == "ED7")
                                    {
                                        mensagens.Add("Fica vedado para esta modalidade de ensino as categorias ofertadas para a EJA.");
                                    }

                                    if (categoriaItinerarioFormativoId == 5) //5 - SOCIOEDUCAÇÃO
                                    {
                                        mensagens.Add("Fica vedado para esta modalidade de ensino as categorias ofertadas para Socioeducação.");
                                    }

                                    //Escolas com Turmas únicas no turno:
                                    //Deverão ofertar, somente, Trilha de aprofundamento pertinente a esta categoria
                                    if (quantidadeTurmas == 1)
                                    {
                                        if (categoriaItinerarioFormativoId != 6 && categoriaItinerarioFormativoId != 3) //6 - EDUCAÇÃO DO CAMPO
                                        {
                                            mensagens.Add("Para Escolas com Turmas únicas no turno, apenas pode ofertar Trilha de aprofundamento pertinente categoria Educação do Campo ou Integração de áreas.");
                                        }
                                        else
                                        {
                                            //Verifica se já tem alguma trilha
                                            if (quantidadeOfertas >= 1)
                                            {
                                                mensagens.Add("Para Escolas com Turmas únicas no turno, apenas pode ser cadastrada uma trilha, e esta escola já cadastrou trilha para esta MODALIDADE / TURNO");
                                            }
                                        }
                                    }
                                }
                                else if (trilhaAprendizagemEscola.Censo == "33131422" || trilhaAprendizagemEscola.Censo == "33170223"
                                    || trilhaAprendizagemEscola.Censo == "33111626" || trilhaAprendizagemEscola.Censo == "33111707"
                                    || trilhaAprendizagemEscola.Censo == "33124620" || trilhaAprendizagemEscola.Censo == "33170240"
                                    || trilhaAprendizagemEscola.Censo == "33136670") //Itinerários Socioeducação - Apenas escolas enviadas pela area
                                {
                                    if (modalidadeTrilha == "ED2" || modalidadeTrilha == "ED7")
                                    {
                                        mensagens.Add("Fica vedado para esta modalidade de ensino as categorias ofertadas para a EJA.");
                                    }

                                    if (categoriaItinerarioFormativoId == 6) //6 - EDUCAÇÃO DO CAMPO
                                    {
                                        mensagens.Add("Fica vedado para esta modalidade de ensino as categorias ofertadas para Educação do Campo.");
                                    }

                                    //Escolas com Turmas únicas no turno:
                                    //Deverão ofertar, somente, Trilha de aprofundamento pertinente a sua categoria
                                    if (quantidadeTurmas == 1)
                                    {
                                        if (categoriaItinerarioFormativoId != 5 && categoriaItinerarioFormativoId != 3) //5 - SOCIOEDUCAÇÃO
                                        {
                                            mensagens.Add("Para Escolas com Turmas únicas no turno, apenas pode ofertar Trilha de aprofundamento pertinente categoria Socioeducação ou Integração de áreas.");
                                        }
                                        else
                                        {
                                            //Verifica se já tem alguma trilha
                                            if (quantidadeOfertas >= 1)
                                            {
                                                mensagens.Add("Para Escolas com Turmas únicas no turno, apenas pode ser cadastrada uma trilha, e esta escola já cadastrou trilha para esta MODALIDADE / TURNO");
                                            }
                                        }
                                    }
                                }
                                else //Os demais
                                {
                                    if (modalidadeTrilha == "ED2" || modalidadeTrilha == "ED7")
                                    {
                                        mensagens.Add("Fica vedado para esta modalidade de ensino as categorias ofertadas para a EJA.");
                                    }

                                    if (categoriaItinerarioFormativoId == 5) //5 - SOCIOEDUCAÇÃO
                                    {
                                        mensagens.Add("Fica vedado para esta modalidade de ensino as categorias ofertadas para Socioeducação.");
                                    }

                                    if (categoriaItinerarioFormativoId == 6) //6 - EDUCAÇÃO DO CAMPO
                                    {
                                        mensagens.Add("Fica vedado para esta modalidade de ensino as categorias ofertadas para Educação do Campo.");
                                    }

                                    //Escolas com Turmas únicas no turno:
                                    //Deverão ofertar, somente, Trilha de aprofundamento do Itinerário Integração de áreas;
                                    if (quantidadeTurmas == 1)
                                    {
                                        if (categoriaItinerarioFormativoId != 3) //3 - INTEGRADO
                                        {
                                            mensagens.Add("Para Escolas com Turmas únicas no turno, apenas pode ofertar Trilha de aprofundamento do Itinerário Integração de áreas.");
                                        }
                                        else
                                        {
                                            //Verifica se já tem alguma trilha
                                            if (quantidadeOfertas >= 1)
                                            {
                                                mensagens.Add("Para Escolas com Turmas únicas no turno, apenas pode ser cadastrada uma trilha, e esta escola já cadastrou trilha para esta MODALIDADE / TURNO");
                                            }
                                        }
                                    }
                                }

                                if (mensagens.Count == 0)
                                {
                                    //Escolas com 2 Turmas no turno:
                                    //Deverão ofertar 2 Trilhas de aprofundamento de qualquer categoria de Itinerários;
                                    if (quantidadeTurmas == 2)
                                    {
                                        //Verifica se já tem alguma trilha
                                        if (quantidadeOfertas >= 2)
                                        {
                                            mensagens.Add("Para Escolas com 2 Turmas no turno, apenas podem ser cadastradas duas trilhas, e esta escola já cadastrou 2 trilhas para esta MODALIDADE / TURNO");
                                        }
                                    }

                                    //Escolas com 3 Turmas no turno:

                                    //Deverão ofertar de 2 a 3 Trilhas de aprofundamento de qualquer categoria de Itinerários;
                                    if (quantidadeTurmas == 3)
                                    {
                                        //Verifica se já tem alguma trilha
                                        if (quantidadeOfertas >= 3)
                                        {
                                            mensagens.Add("Para Escolas com 3 Turmas no turno, podem ser cadastradas 2 a 3 trilhas, e esta escola " + quantidadeOfertas + "  trilha(s) para esta MODALIDADE / TURNO");
                                        }
                                    }

                                    //Verifica se é escola unica no municipio
                                    if (trilhaAprendizagemEscola.Censo == "33004447" || trilhaAprendizagemEscola.Censo == "33005664"
                                        || trilhaAprendizagemEscola.Censo == "33016070" || trilhaAprendizagemEscola.Censo == "33016550"
                                        || trilhaAprendizagemEscola.Censo == "33017000" || trilhaAprendizagemEscola.Censo == "33023751"
                                        || trilhaAprendizagemEscola.Censo == "33027820" || trilhaAprendizagemEscola.Censo == "33030456"
                                        || trilhaAprendizagemEscola.Censo == "33031185" || trilhaAprendizagemEscola.Censo == "33032130"
                                        || trilhaAprendizagemEscola.Censo == "33042047" || trilhaAprendizagemEscola.Censo == "33093555")
                                    {
                                        //Escolas únicas no Município com: 
                                        //4 turmas ou mais, no turno, poderão ofertar até 4 Trilhas de aprofundamento de qualquer categoria de Itinerários. 
                                        if (quantidadeTurmas >= 4)
                                        {
                                            //Verifica se já tem alguma trilha
                                            if (quantidadeOfertas >= 4)
                                            {
                                                mensagens.Add("Para Escolas únicas no Município com 4 turmas ou mais, no turno, apenas podem ser cadastradas quatro trilhas, e esta escola já cadastrou 4 trilhas para esta MODALIDADE / TURNO");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //Escolas com 4 a 9 Turmas no turno:
                                        //Deverão ofertar de 2 a 4 Trilhas de aprofundamento de qualquer categoria de Itinerários;
                                        if (quantidadeTurmas >= 4 && quantidadeTurmas <= 9)
                                        {
                                            //Verifica se já tem alguma trilha
                                            if (quantidadeOfertas >= 4)
                                            {
                                                mensagens.Add("Para Escolas com 4 ou mais Turmas no turno, apenas podem ser cadastradas de 2 a 4 trilhas, e esta escola " + quantidadeOfertas + "  trilha(s) para esta MODALIDADE / TURNO");
                                            }
                                        }

                                        //Escolas com 10 Turmas ou mais no turno:
                                        //Poderão ofertar de 2 a 5  Trilhas de aprofundamento de qualquer categoria de Itinerários;
                                        //if (quantidadeTurmas >= 10)
                                        //{
                                        //    //Verifica se já tem alguma trilha
                                        //    if (quantidadeOfertas >= 5)
                                        //    {
                                        //        mensagens.Add("Para Escolas com 10 Turmas ou mais no turno, podem ser cadastradas de 2 a 5 trilhas, e esta escola " + quantidadeOfertas + "  trilha(s) para esta MODALIDADE / TURNO");
                                        //    }
                                        //}
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //Prezado(a) Diretor(a),
                        //Sua Unidade Escolar não participa deste processo, por não ocorrer a oferta as Modalidades: Educação de Jovens e 
                        //Adultos (EJA- Módulo III) e Ensino Médio Regular em horário parcial (2ª série).
                        mensagens.Add("Prezado(a) Diretor(a),</br>Sua Unidade Escolar não participa deste processo, por não ocorrer a oferta das Modalidades: Educação de Jovens e Adultos (EJA- Módulo III) e Ensino Médio Regular em horário parcial (2ª série).");
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

        public int RetornaQuantidadeOfertaPor(DataContext contexto, string censo, string modalidade, string turno, string tipo, int ano)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            int retorno = 0;

            try
            {

                contextQuery.Command = @" SELECT COUNT(1) AS OFERTA
                                        FROM [Pedagogico].[TRILHAAPRENDIZAGEM_ESCOLA] t
	                                        INNER JOIN LY_CURSO C ON T.CURSO = C.CURSO
											INNER JOIN Pedagogico.TRILHAAPRENDIZAGEM TR ON C.TRILHAAPRENDIZAGEMID = TR.TRILHAAPRENDIZAGEMID
                                        WHERE MODALIDADE = @MODALIDADE
	                                        AND T.CENSO = @CENSO
	                                        AND T.TURNO = @TURNO
											AND TR.TIPO = @TIPO
                                            AND T.ANO = @ANO ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@MODALIDADE", SqlDbType.VarChar, modalidade);
                contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, turno);
                contextQuery.Parameters.Add("@TIPO", SqlDbType.VarChar, tipo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["OFERTA"]);
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

        private bool PossuiOutroCadastradoPor(DataContext ctx, string turno, string curso, string censo, int trilhaAprendizagemEscolaId, int ano)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM [Pedagogico].[TRILHAAPRENDIZAGEM_ESCOLA] (NOLOCK)
                                WHERE CENSO = @CENSO
                                    AND CURSO = @CURSO
                                    AND TURNO = @TURNO
                                    AND ANO = @ANO
                                    AND TRILHAAPRENDIZAGEM_ESCOLAID <> @TRILHAAPRENDIZAGEM_ESCOLAID ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
            contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
            contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, turno);
            contextQuery.Parameters.Add("@TRILHAAPRENDIZAGEM_ESCOLAID", SqlDbType.Int, trilhaAprendizagemEscolaId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }
        
        public static bool EhCursoOfertaPor(int ano, string censo, string curso, int serie)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*)
                                FROM [Pedagogico].[TRILHAAPRENDIZAGEM_ESCOLA] te (NOLOCK)
									inner join TurnosVagas.CONFIRMACAOOFERTA c (nolock) on te.ANO = c.ANO 
									inner join TurnosVagas.CONFIRMACAOOFERTAVAGAS v (nolock) on c.CONFIRMACAOOFERTAID = v.CONFIRMACAOOFERTAID and te.CURSO = v.CURSO
                                WHERE TE.CENSO = @CENSO
                                    AND TE.CURSO = @CURSO
									AND TE.ANO = @ANO
									AND V.SERIE = @SERIE ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, censo);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, curso);
                contextQuery.Parameters.Add("@SERIE", SqlDbType.Int, serie);

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
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

        public void Insere(Entidades.TrilhaAprendizagemEscola trilhaAprendizagemEscola)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" INSERT INTO [Pedagogico].[TRILHAAPRENDIZAGEM_ESCOLA]
                                                        (
                                                         ANO,
                                                         CURSO,
                                                         TURNO,
                                                         CENSO, 
                                                         USUARIOID, 
                                                         DATACADASTRO, 
                                                         DATAALTERACAO) 
                                            VALUES      (
                                                         @ANO,
                                                         @CURSO,
                                                         @TURNO,
                                                         @CENSO, 
                                                         @USUARIOID, 
                                                         @DATACADASTRO, 
                                                         @DATAALTERACAO) ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, trilhaAprendizagemEscola.Ano);
                contextQuery.Parameters.Add("@CURSO", SqlDbType.VarChar, trilhaAprendizagemEscola.Curso);
                contextQuery.Parameters.Add("@TURNO", SqlDbType.VarChar, trilhaAprendizagemEscola.Turno);
                contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, trilhaAprendizagemEscola.Censo);
                contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, trilhaAprendizagemEscola.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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
                ctx.Dispose();
            }
        }

        public ValidacaoDados ValidaRemocao(int trilhaAprendizagemEscolaId, string censo, string curso, int ano)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            TrilhaAprendizagemAluno rnTrilhaAprendizagemAluno = new TrilhaAprendizagemAluno();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (trilhaAprendizagemEscolaId <= 0)
            {
                mensagens.Add("Campo ID é obrigatório.");
            }

            if (ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CENSO é obrigatório.");
            }

            if (curso.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CURSO é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se ja foi utilizado
                    if (rnTrilhaAprendizagemAluno.PossuiTrilhaAprendizagemEscolaPor(contexto, censo, curso, ano))
                    {
                        mensagens.Add("Registro não pode ser excluído pois já foi utilizado por um aluno.");
                    }
                }
                catch
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw;
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

        public void Remove(int trilhaAprendizagemId)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE [Pedagogico].[TRILHAAPRENDIZAGEM_ESCOLA]
                            WHERE  TRILHAAPRENDIZAGEM_ESCOLAID = @TRILHAAPRENDIZAGEM_ESCOLAID ";

                contextQuery.Parameters.Add("@TRILHAAPRENDIZAGEM_ESCOLAID", SqlDbType.Int, trilhaAprendizagemId);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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
                ctx.Dispose();
            }
        }


    }
}
