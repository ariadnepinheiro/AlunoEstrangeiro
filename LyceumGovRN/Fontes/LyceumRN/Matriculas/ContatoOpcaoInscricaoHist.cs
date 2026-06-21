using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.Matriculas
{    
    public class ContatoOpcaoInscricaoHist
    {
        public void Insere(DataContext contexto, int opcaoInscricaoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Matricula.CONTATOOPCAOINSCRICAOHIST 
                                                (OPCAOINSCRICAOHISTID, 
                                                 DATACONTATO, 
                                                 CONTATO, 
                                                 ACEITO, 
                                                 MOTIVOREJEICAOINSCRICAOID, 
                                                 OBSERVACAO, 
                                                 USUARIOID, 
                                                 DATACADASTRO, 
                                                 DATAALTERACAO) 
                                  select OH.OPCAOINSCRICAOHISTID, 
                                                 DATACONTATO, 
                                                 CONTATO, 
                                                 ACEITO, 
                                                 co.MOTIVOREJEICAOINSCRICAOID, 
                                                 OBSERVACAO, 
                                                 CO.USUARIOID, 
                                                 CO.DATACADASTRO, 
                                                 CO.DATAALTERACAO 
								  from Matricula.CONTATOOPCAOINSCRICAO co (NOLOCK)
										INNER JOIN MATRICULA.OPCAOINSCRICAOHIST OH (NOLOCK) ON CO.OPCAOINSCRICAOID = OH.OPCAOINSCRICAOID
								  where CO.OPCAOINSCRICAOID = @OPCAOINSCRICAOID ";

            contextQuery.Parameters.Add("@OPCAOINSCRICAOID", SqlDbType.Int, opcaoInscricaoId);

            contexto.ApplyModifications(contextQuery);
        }

        public bool PossuiMotivoPor(DataContext contexto, int motivoRejeicaoInscricaoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM MATRICULA.CONTATOOPCAOINSCRICAOHIST (NOLOCK)
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
