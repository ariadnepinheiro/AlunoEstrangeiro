using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Pedagogico
{
    public class TrilhaAprendizagemEscolaFinalizada
    {
        public bool PossuiFinalizacaoPor(string censo, int ano, out string mensagemFinalizacao)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;
            mensagemFinalizacao = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT F.DATAFINALIZACAO, 
                                       F.USUARIOID, 
                                       P.NOME_COMPL 
                                FROM   Pedagogico.TrilhaAprendizagemEscolaFinalizada F (NOLOCK) 
                                       LEFT JOIN HADES.DBO.HD_USUARIO U (NOLOCK) 
                                              ON F.USUARIOID = U.USUARIO 
                                       LEFT JOIN LY_PESSOA P (NOLOCK) 
                                              ON U.PESSOA = P.PESSOA 
                                WHERE  CENSO = @CENSO 
                                       AND ANO = @ANO 
                                        ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.Int, censo);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                
                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    string usuario = Convert.ToString(reader["USUARIOID"]);
                    string nome = Convert.ToString(reader["NOME_COMPL"]);
                    DateTime dataFinalizacao = Convert.ToDateTime(reader["DATAFINALIZACAO"]);

                    mensagemFinalizacao = string.Format("Oferta Escolar finalizada em {0} pelo usuário {1} - {2}", dataFinalizacao.ToString("dd/MM/yyyy"), usuario, nome);
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }

        public void Finaliza(Entidades.TrilhaAprendizagemEscolaFinalizada trilhaAprendizagemEscolaFinalizada)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
                      
            try
            {
                //Insere Finalização
                this.Insere(contexto, trilhaAprendizagemEscolaFinalizada);
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

        private void Insere(DataContext contexto, Entidades.TrilhaAprendizagemEscolaFinalizada trilhaAprendizagemEscolaFinalizada)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Pedagogico.TRILHAAPRENDIZAGEMESCOLAFINALIZADA 
                                                    (CENSO,                                                      
                                                     ANO, 
                                                     DATAFINALIZACAO, 
                                                     USUARIOID) 
                                        VALUES      (@CENSO,                                                     
                                                     @ANO, 
                                                     @DATAFINALIZACAO, 
                                                     @USUARIOID) ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, trilhaAprendizagemEscolaFinalizada.Censo);      
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, trilhaAprendizagemEscolaFinalizada.Ano);
            contextQuery.Parameters.Add("@DATAFINALIZACAO", SqlDbType.DateTime, trilhaAprendizagemEscolaFinalizada.DataFinalizacao);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, trilhaAprendizagemEscolaFinalizada.UsuarioId);

            contexto.ApplyModifications(contextQuery);
        }

        public ValidacaoDados ValidaFinalizacao(RN.Pedagogico.Entidades.TrilhaAprendizagemEscolaFinalizada trilhaAprendizagemEscolaFinalizada)
        {

            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.Turma rnTurma = new Turma();
            List<string> modalidades = new List<string>();
            RN.Pedagogico.TrilhaAprendizagemEscola rnTrilhaAprendizagemEscola = new TrilhaAprendizagemEscola();

            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (trilhaAprendizagemEscolaFinalizada == null)
            {
                return validacaoDados;
            }           

            if (trilhaAprendizagemEscolaFinalizada.Ano <= 0)
            {
                mensagens.Add("Campo ANO é obrigatório.");
            }

            if (trilhaAprendizagemEscolaFinalizada.Censo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CENSO é obrigatório.");
            }

            if (trilhaAprendizagemEscolaFinalizada.UsuarioId.IsNullOrEmptyOrWhiteSpace())
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

                    //modalidades participantes
                    modalidades.Add("RE1");
                    modalidades.Add("ED2");

                    //Verifica se escola pode participar, apenas escolas com EJA- Módulo III e Ensino Médio Regular 2ª série
                    if (rnTrilhaAprendizagemEscola.PodeParticiparPor(trilhaAprendizagemEscolaFinalizada.Censo, trilhaAprendizagemEscolaFinalizada.Ano))
                    {
                        foreach (string modalidade in modalidades)
                        {
                            var turnos = rnTurma.ListaTurnosOfertaPor(contexto, trilhaAprendizagemEscolaFinalizada.Ano, trilhaAprendizagemEscolaFinalizada.Censo, modalidade);

                            foreach (string turno in turnos)
                            {
                                int quantidadeTurmas = rnTurma.RetornaQuantidadeTurmaOfertaPor(contexto, trilhaAprendizagemEscolaFinalizada.Ano, trilhaAprendizagemEscolaFinalizada.Censo, modalidade, turno);
                                int quantidadeOfertas = rnTrilhaAprendizagemEscola.RetornaQuantidadeOfertaPor(contexto, trilhaAprendizagemEscolaFinalizada.Censo, modalidade, turno, "APROFUNDAMENTO", trilhaAprendizagemEscolaFinalizada.Ano);

                                if (quantidadeTurmas == 1 && quantidadeOfertas != 1)
                                {
                                    mensagens.Add(string.Format("Para finalizar a escola com 1 turma, apenas pode ser cadastrada 1 trilha, e esta escola cadastrou {0} trilha(s) para a modalidade {1} no turno {2}.", quantidadeOfertas.ToString(), modalidade == "RE1" ? "Regular" : "EJA",turno ));
                                }

                                if (quantidadeTurmas == 2 && quantidadeOfertas != 2)
                                {
                                    mensagens.Add(string.Format("Para finalizar a escola com 2 turmas, apenas podem ser cadastradas 2 trilhas, e esta escola cadastrou {0} trilha(s) para a modalidade {1} no turno {2}.", quantidadeOfertas.ToString(), modalidade == "RE1" ? "Regular" : "EJA", turno));
                                }

                                if (quantidadeTurmas == 3 && (quantidadeOfertas < 2 || quantidadeOfertas > 3))
                                {
                                    mensagens.Add(string.Format("Para finalizar a escola com 3 turmas, podem ser cadastradas de 2 até 3 Trilhas, e esta escola cadastrou {0} trilha(s) para a modalidade {1} no turno {2}.", quantidadeOfertas.ToString(), modalidade == "RE1" ? "Regular" : "EJA", turno));
                                }

                                if ((quantidadeTurmas >= 4 ) && (quantidadeOfertas < 2 || quantidadeOfertas > 4))
                                {
                                    mensagens.Add(string.Format("Para finalizar a escola com 4 turmas ou mais, podem ser cadastradas de 2 até 4 Trilhas, e esta escola cadastrou {0} trilha(s) para a modalidade {1} no turno {2}.", quantidadeOfertas.ToString(), modalidade == "RE1" ? "Regular" : "EJA", turno));
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

    }
}