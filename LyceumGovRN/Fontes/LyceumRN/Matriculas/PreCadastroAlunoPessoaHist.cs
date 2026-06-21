using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;

namespace Techne.Lyceum.RN.Matriculas
{
    public class PreCadastroAlunoPessoaHist
    {
        public void Insere(DataContext contexto, decimal pessoa, string usuario)
        {	
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"   INSERT INTO Matricula.PRECADASTROALUNOPESSOAHIST 
                                                    (PRECADASTROALUNOID, 
                                                     PESSOAID, 
                                                     USUARIOID, 
                                                     DATACADASTRO, 
                                                     DATAALTERACAO) 
                                        SELECT PC.PRECADASTROALUNOID, 
                                               PC.PESSOAID, 
                                               @USUARIOID, 
                                               PC.DATACADASTRO, 
                                               @DATAALTERACAO 
                                        FROM   MATRICULA.PRECADASTROALUNO PC (NOLOCK) 
                                               INNER JOIN MATRICULA.INSCRICAOALUNO I (NOLOCK) 
                                                       ON PC.PRECADASTROALUNOID = I.PRECADASTROALUNOID 
                                        WHERE  PC.PESSOAID = @PESSOAID  ";

            contextQuery.Parameters.Add("@PESSOAID", SqlDbType.Decimal, pessoa);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuario);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }
        
        public void Insere(DataContext contexto, string nome, DateTime nascimento, string mae, string usuario)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"   INSERT INTO Matricula.PRECADASTROALUNOPESSOAHIST 
                                                    (PRECADASTROALUNOID, 
                                                     PESSOAID, 
                                                     USUARIOID, 
                                                     DATACADASTRO, 
                                                     DATAALTERACAO) 
                                        SELECT PC.PRECADASTROALUNOID, 
                                               PC.PESSOAID, 
                                               @USUARIOID, 
                                               PC.DATACADASTRO, 
                                               @DATAALTERACAO 
                                        FROM   MATRICULA.PRECADASTROALUNO PC (NOLOCK) 
                                               INNER JOIN MATRICULA.INSCRICAOALUNO I (NOLOCK) 
                                                       ON PC.PRECADASTROALUNOID = I.PRECADASTROALUNOID 
                                        WHERE PC.NOME = @NOME
											  AND PC.DATANASCIMENTO = @DATANASCIMENTO
											  AND ISNULL(PC.NOMEMAE, '') = @NOMEMAE
											  AND PC.PESSOAID IS NOT NULL  ";
           
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuario);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, nome);
            contextQuery.Parameters.Add("@NOMEMAE", SqlDbType.VarChar, mae);
            contextQuery.Parameters.Add("@DATANASCIMENTO", SqlDbType.DateTime, nascimento);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
