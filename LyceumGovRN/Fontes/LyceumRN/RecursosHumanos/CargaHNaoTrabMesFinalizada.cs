using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.RecursosHumanos
{
    public class CargaHNaoTrabMesFinalizada
    {
        public bool PossuiFinalizacaoPor(string censo, int ano, int mes, out string mensagemFinalizacao)
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
                                FROM   RECURSOSHUMANOS.CARGAHNAOTRABMESFINALIZADA F (NOLOCK) 
                                       LEFT JOIN HADES.DBO.HD_USUARIO U (NOLOCK) 
                                              ON F.USUARIOID = U.USUARIO 
                                       LEFT JOIN LY_PESSOA P (NOLOCK) 
                                              ON U.PESSOA = P.PESSOA 
                                WHERE  CENSO = @CENSO 
                                       AND ANO = @ANO 
                                       AND MES = @MES  ";

                contextQuery.Parameters.Add("@CENSO", SqlDbType.Int, censo);
                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@MES", SqlDbType.Int, mes);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    string usuario = Convert.ToString(reader["USUARIOID"]);
                    string nome = Convert.ToString(reader["NOME_COMPL"]);
                    DateTime dataFinalizacao = Convert.ToDateTime(reader["DATAFINALIZACAO"]);

                    mensagemFinalizacao = string.Format("Frequência finalizada em {0} pelo usuário {1} - {2}", dataFinalizacao.ToString("dd/MM/yyyy"), usuario, nome);
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

        public void Finaliza(Entidades.CargaHNaoTrabMesFinalizada cargaHNaoTrabMesFinalizada, List<Entidades.CargaHNaoTrabMes> listaCargaHNaoTrabMes)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            CargaHNaoTrabMes rnCargaHNaoTrabMes = new CargaHNaoTrabMes();
            bool alterado = false;
            try
            {
                //Salva todas as linhas de frequencias de docentes
                foreach (Entidades.CargaHNaoTrabMes cargaHNaoTrabMes in listaCargaHNaoTrabMes)
                {
                    //Verifica se já existe cadastro para a frequencia do docentes
                    if (cargaHNaoTrabMes.IdCargaHNaoTrabMes > 0)
                    {
                        //Verifica se houve mudança
                        alterado = rnCargaHNaoTrabMes.PossuiAtualizacaoPor(contexto, cargaHNaoTrabMes.IdCargaHNaoTrabMes, cargaHNaoTrabMes.ChNaoTrabalhadaMes);

                        if (alterado)
                        {
                            //Atualiza cadastro
                            rnCargaHNaoTrabMes.Atualiza(contexto, cargaHNaoTrabMes.IdCargaHNaoTrabMes, cargaHNaoTrabMes.ChNaoTrabalhadaMes, cargaHNaoTrabMes.UsuarioId);
                        }
                    }
                    else
                    {
                        //Insere cadastro
                        rnCargaHNaoTrabMes.Insere(contexto, cargaHNaoTrabMes.NumFunc, cargaHNaoTrabMes.UnidadeEns, cargaHNaoTrabMes.Ano, cargaHNaoTrabMes.Mes, cargaHNaoTrabMes.ChMensal, cargaHNaoTrabMes.ChSemanal, cargaHNaoTrabMes.ChNaoTrabalhadaMes, cargaHNaoTrabMes.UsuarioId);
                    }
                }

                //Insere Finalização
                this.Insere(contexto, cargaHNaoTrabMesFinalizada);
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

        private void Insere(DataContext contexto, Entidades.CargaHNaoTrabMesFinalizada cargaHNaoTrabMesFinalizada)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO RecursosHumanos.CARGAHNAOTRABMESFINALIZADA 
                                                    (CENSO, 
                                                     MES, 
                                                     ANO, 
                                                     DATAFINALIZACAO, 
                                                     USUARIOID) 
                                        VALUES      (@CENSO, 
                                                     @MES, 
                                                     @ANO, 
                                                     @DATAFINALIZACAO, 
                                                     @USUARIOID) ";

            contextQuery.Parameters.Add("@CENSO", SqlDbType.VarChar, cargaHNaoTrabMesFinalizada.Censo);
            contextQuery.Parameters.Add("@MES", SqlDbType.Int, cargaHNaoTrabMesFinalizada.Mes);
            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, cargaHNaoTrabMesFinalizada.Ano);
            contextQuery.Parameters.Add("@DATAFINALIZACAO", SqlDbType.DateTime, cargaHNaoTrabMesFinalizada.DataFinalizacao);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, cargaHNaoTrabMesFinalizada.UsuarioId);

            contexto.ApplyModifications(contextQuery);
        }
    }
}