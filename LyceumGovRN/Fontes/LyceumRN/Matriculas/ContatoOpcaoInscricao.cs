using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.RN.Matriculas
{
    public class ContatoOpcaoInscricao
    {
        public DataTable ListaPor(int opcaoInscricaoId)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable retorno = null;

            try
            {
                contextQuery.Command = @" SELECT   CO.CONTATOOPCAOINSCRICAOID,
	                                               CO.OPCAOINSCRICAOID,
	                                               CO.DATACONTATO, 
                                                   CASE 
                                                     WHEN CO.CONTATO = 0 THEN 'Não foi possível contato' 
                                                     WHEN CO.CONTATO = 1 
                                                          AND CO.ACEITO = 1 THEN 'Aceito' 
                                                     ELSE 'Não Aceito - ' + MO.DESCRICAO 
                                                   END    RESPOSTA, 
                                                   CO.OBSERVACAO, 
                                                   CO.CONTATO,
                                                   CO.ACEITO,
                                                   U.USUARIO + ' - ' + U.NOME AS NOMEUSUARIO 
                                            FROM   Matricula.CONTATOOPCAOINSCRICAO CO (NOLOCK)
                                                   LEFT JOIN Matricula.MOTIVOREJEICAOINSCRICAO MO (NOLOCK)
                                                          ON CO.MOTIVOREJEICAOINSCRICAOID = MO.MOTIVOREJEICAOINSCRICAOID 
                                                   LEFT JOIN HADES.DBO.HD_USUARIO U (NOLOCK)
                                                          ON CO.USUARIOID = U.USUARIO 
                                            WHERE  CO.OPCAOINSCRICAOID = @OPCAOINSCRICAOID  ";

                contextQuery.Parameters.Add("@OPCAOINSCRICAOID", SqlDbType.Int, opcaoInscricaoId);

                retorno = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
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
                contexto.Dispose();
            }

            return retorno;
        }         

        public void Remove(DataContext contexto, int opcaoInscricaoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE Matricula.CONTATOOPCAOINSCRICAO 
                                      WHERE OPCAOINSCRICAOID = @OPCAOINSCRICAOID ";

            contextQuery.Parameters.Add("@OPCAOINSCRICAOID", SqlDbType.Int, opcaoInscricaoId);

            contexto.ApplyModifications(contextQuery);
        }        

        public bool PossuiMotivoPor(DataContext contexto, int motivoRejeicaoInscricaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM MATRICULA.CONTATOOPCAOINSCRICAO (NOLOCK)
                                    WHERE MOTIVOREJEICAOINSCRICAOID = @MOTIVOREJEICAOINSCRICAOID ";

            contextQuery.Parameters.Add("@MOTIVOREJEICAOINSCRICAOID", SqlDbType.Int, motivoRejeicaoInscricaoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }
    }
}