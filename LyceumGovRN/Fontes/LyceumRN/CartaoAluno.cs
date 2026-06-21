using System;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.DTOs;

namespace Techne.Lyceum.RN
{
    public class CartaoAluno : RNBase
    {
        public static DadosCartaoAluno UltimaSituacao(string aluno)
        {
            var cartao = new DadosCartaoAluno();

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT TOP 1 S.SOLICITACAO, A.DATA, A.PASSO, A.STATUS
                        FROM    DBO.LY_SOLICITACAO_SERV S
                                LEFT JOIN DBO.LY_ITENS_SOLICIT_SERV I ON S.SOLICITACAO = I.SOLICITACAO
                                LEFT JOIN DBO.LY_ANDAMENTO A ON A.SOLICITACAO = S.SOLICITACAO
                        WHERE   ALUNO = @ALUNO
                        ORDER BY A.DATA DESC "
                };
                contextQuery.Parameters.Add("@ALUNO", aluno);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    if (reader.Read())
                    {
                        cartao.Passo = reader["PASSO"].ToString();
                        cartao.Status = reader["STATUS"].ToString();
                        if (reader["DATA"] != DBNull.Value)
                        {
                            cartao.Data = Convert.ToDateTime(reader["DATA"]);
                        }
                    }
                }
                return cartao;
            }
        }
    }
}
