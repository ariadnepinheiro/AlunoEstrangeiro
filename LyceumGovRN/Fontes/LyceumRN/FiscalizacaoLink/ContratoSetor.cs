using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.FiscalizacaoLink
{
    public class ContratoSetor
    {
        public DataTable ListaAnosHabilitadosPor(string setor)
        {
            DataTable listaAnos = new DataTable();
            DateTime inicio;
            DateTime fim;
            string[] dataInicioFim = new string[2];

            try
            {
                listaAnos.Columns.Add("ANO");
                listaAnos.Columns.Add("DESCRICAO");

                //Busca data limites
                dataInicioFim = this.ObtemDataLimitesPor(setor);

                if (!dataInicioFim[0].IsNullOrEmptyOrWhiteSpace() && !dataInicioFim[1].IsNullOrEmptyOrWhiteSpace())
                {
                    inicio = Convert.ToDateTime(dataInicioFim[0]);
                    fim = Convert.ToDateTime(dataInicioFim[1]);

                    for (int i = inicio.Year; i <= fim.Year; i++)
                    {
                        listaAnos.Rows.Add(i, i);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listaAnos;
        }

        public DataTable ListaMesesHabilitadosPor(string setor, int ano)
        {
            DataTable listaMeses = new DataTable();
            DateTime inicio;
            DateTime fim;
            string[] dataInicioFim = new string[2];
            int mesInicio;
            int mesFim;

            try
            {
                listaMeses.Columns.Add("MES");
                listaMeses.Columns.Add("DESCRICAO");

                //Busca data limites
                dataInicioFim = this.ObtemDataLimitesPor(setor);

                if (!dataInicioFim[0].IsNullOrEmptyOrWhiteSpace() && !dataInicioFim[1].IsNullOrEmptyOrWhiteSpace())
                {
                    inicio = Convert.ToDateTime(dataInicioFim[0]);
                    fim = Convert.ToDateTime(dataInicioFim[1]);

                    //Caso o ano da pesquisa seja maior q o ano inicio
                    if (ano > inicio.Year)
                    {
                        //começa lista no primeiro mes
                        mesInicio = 1;
                    }
                    else
                    {
                        //Caso o ano da pesquisa seja igual ao ano inicio
                        if (ano == inicio.Year)
                        {
                            //começa lista no proprio mes
                            mesInicio = inicio.Month;
                        }
                        else
                        {
                            //Ano da esquisa não pode ser menor que o inicio
                            return null;
                        }
                    }

                    //Caso o ano da pesquisa seja menor q o ano fim
                    if (ano < fim.Year)
                    {
                        //termina lista no ultimo mes
                        mesFim = 12;
                    }
                    else
                    {
                        //Caso o ano da pesquisa seja igual ao ano fim
                        if (ano == inicio.Year)
                        {
                            //termina lista no proprio mes
                            mesFim = inicio.Month;
                        }
                        else
                        {
                            //Ano da esquisa não pode ser maior que o inicio
                            return null;
                        }
                    }

                    for (int i = mesInicio; i <= mesFim; i++)
                    {
                        listaMeses.Rows.Add(i, RN.Util.Utils.ObtemDescricaoMesPor(i));
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listaMeses;
        }

        private string[] ObtemDataLimitesPor(string setor)
        {
            string[] datas = new string[2];
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT MIN(CS.DATAIMPLANTACAO) AS DATAIMPLANTACAO, 
                                               MAX(ISNULL(CS.DATATERMINO, C.DATATERMINO)) AS DATATERMINO 
                                        FROM   FISCALIZACAOLINK.CONTRATOSETOR CS (NOLOCK) 
                                               INNER JOIN FISCALIZACAOLINK.CONTRATO C (NOLOCK) 
                                                       ON CS.CONTRATOID = C.CONTRATOID 
                                        WHERE  SETORID = @SETORID  ";

                contextQuery.Parameters.Add("@SETORID", SqlDbType.VarChar, setor);
                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    datas[0] = Convert.ToString(reader["DATAIMPLANTACAO"]);
                    datas[1] = Convert.ToString(reader["DATATERMINO"]);
                }

                return datas;
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }

        public RN.FiscalizacaoLink.Entidades.ContratoSetor ObtemPor(int contratoId, string setor)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            RN.FiscalizacaoLink.Entidades.ContratoSetor entidade = new RN.FiscalizacaoLink.Entidades.ContratoSetor();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT * 
                                            FROM   FISCALIZACAOLINK.CONTRATOSETOR (NOLOCK)
                                            WHERE  SETORID = @SETORID 
                                                   AND CONTRATOID = @CONTRATOID  ";

                contextQuery.Parameters.Add("@SETORID", SqlDbType.VarChar, setor);
                contextQuery.Parameters.Add("@CONTRATOID", SqlDbType.Int, contratoId);

                entidade = contexto.TryToBindEntity<FiscalizacaoLink.Entidades.ContratoSetor>(contextQuery);

                return entidade;
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

        public void Insere(DataContext contexto, RN.FiscalizacaoLink.Entidades.ContratoSetor contratoSetor)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO FiscalizacaoLink.CONTRATOSETOR
                                               (SETORID
                                               ,CONTRATOID
                                               ,DATACONTRATACAO
                                               ,DATAIMPLANTACAO
                                               ,DATATERMINO
                                               ,USUARIOID
                                               ,DATACADASTRO
                                               ,DATAALTERACAO )
                                         VALUES
                                              (@SETORID
                                               ,@CONTRATOID
                                               ,@DATACONTRATACAO
                                               ,@DATAIMPLANTACAO
                                               ,@DATATERMINO
                                               ,@USUARIOID
                                               ,@DATACADASTRO
                                               ,@DATAALTERACAO ) ";

            contextQuery.Parameters.Add("@SETORID", SqlDbType.VarChar, contratoSetor.SetorId);
            contextQuery.Parameters.Add("@CONTRATOID", SqlDbType.Int, contratoSetor.ContratoId);
            contextQuery.Parameters.Add("@DATACONTRATACAO", SqlDbType.DateTime, Convert.ToDateTime(contratoSetor.DataContratacao).Date);
            contextQuery.Parameters.Add("@DATAIMPLANTACAO", SqlDbType.DateTime, Convert.ToDateTime(contratoSetor.DataImplantacao).Date);           
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, contratoSetor.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            if (contratoSetor.DataTermino == null || contratoSetor.DataTermino <= DateTime.MinValue)
            {
                contextQuery.Parameters.Add("@DATATERMINO", SqlDbType.DateTime, null);
            }
            else
            {
                contextQuery.Parameters.Add("@DATATERMINO", SqlDbType.DateTime, Convert.ToDateTime(contratoSetor.DataTermino).Date);
            }

            contexto.ApplyModifications(contextQuery);
        }

        public void Atualiza(DataContext contexto, RN.FiscalizacaoLink.Entidades.ContratoSetor contratoSetor)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE FiscalizacaoLink.CONTRATOSETOR
                                       SET DATAIMPLANTACAO = @DATAIMPLANTACAO,
                                          DATATERMINO = @DATATERMINO, 
                                          USUARIOID = @USUARIOID,
                                          DATAALTERACAO = @DATAALTERACAO, 
                                          DATACONTRATACAO = @DATACONTRATACAO
                                      WHERE CONTRATOSETORID = @CONTRATOSETORID ";

            contextQuery.Parameters.Add("@CONTRATOSETORID", SqlDbType.Int, contratoSetor.ContratoSetorId);
            contextQuery.Parameters.Add("@DATACONTRATACAO", SqlDbType.DateTime, Convert.ToDateTime(contratoSetor.DataContratacao).Date);
            contextQuery.Parameters.Add("@DATAIMPLANTACAO", SqlDbType.DateTime, Convert.ToDateTime(contratoSetor.DataImplantacao).Date);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, contratoSetor.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            if (contratoSetor.DataTermino == null || contratoSetor.DataTermino <= DateTime.MinValue)
            {
                contextQuery.Parameters.Add("@DATATERMINO", SqlDbType.DateTime, null);
            }
            else
            {
                contextQuery.Parameters.Add("@DATATERMINO", SqlDbType.DateTime, Convert.ToDateTime(contratoSetor.DataTermino).Date);
            }

            contexto.ApplyModifications(contextQuery);
        }
    }
}