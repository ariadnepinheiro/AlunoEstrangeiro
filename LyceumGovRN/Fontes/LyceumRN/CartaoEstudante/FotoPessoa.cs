using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.CartaoEstudante
{
    public class FotoPessoa
    {
        public void AtualizaProcessamento(DataContext contexto, Entity.WsStatusFoto wsStatusFoto, bool processado, bool aprovado)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"   UPDATE F
                                        SET IDBENEFICIARIO = @IDBENEFICIARIO,
	                                        PROCESSADO = @PROCESSADO,
	                                        APROVADO = @APROVADO,
	                                        USUARIOID = @USUARIOID,
	                                        DATAALTERACAO = @DATAALTERACAO
                                        FROM CartaoEstudante.FOTOPESSOA f
	                                        INNER JOIN LY_ALUNO A ON F.PESSOA = A.PESSOA
                                        WHERE ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@IDBENEFICIARIO", SqlDbType.Int, wsStatusFoto.IdBeneficiario);
            contextQuery.Parameters.Add("@PROCESSADO", SqlDbType.Bit, processado);
            contextQuery.Parameters.Add("@APROVADO", SqlDbType.Bit, aprovado);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, wsStatusFoto.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, wsStatusFoto.Matricula);

            contexto.ApplyModifications(contextQuery);
        }


        public void AtualizaPessoaCorreta(DataContext contexto, decimal pessoaCorreta, decimal pessoaErrada, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"   UPDATE F
                                        SET PESSOA = @PESSOACORRETA,
	                                        USUARIOID = @USUARIOID,
	                                        DATAALTERACAO = @DATAALTERACAO
                                        FROM CartaoEstudante.FOTOPESSOA f	                                 
                                        WHERE PESSOA = @PESSOAERRADA ";

            contextQuery.Parameters.Add("@PESSOAERRADA", SqlDbType.Int, pessoaErrada);
            contextQuery.Parameters.Add("@PESSOACORRETA", SqlDbType.Int, pessoaCorreta);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }


        public bool PossuiFotoPessoaCorretaPor(DataContext contexto, decimal pessoaCorreta)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*)
                                        FROM  CartaoEstudante.FOTOPESSOA
                                        WHERE PESSOA = @PESSOACORRETA ";

            contextQuery.Parameters.Add("@PESSOACORRETA", pessoaCorreta);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public void RemoveFotoPessoa(DataContext contexto, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE CartaoEstudante.FOTOPESSOA
                            WHERE PESSOA = @PESSOA
                                   ";

            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, pessoa);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
