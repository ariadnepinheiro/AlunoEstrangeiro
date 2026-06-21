using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using System.Data;

namespace Techne.Lyceum.RN.Matriculas
{
    public class PreCadastroAlunoFoto
    {
        public Entidades.PreCadastroAlunoFoto ObtemPor(DataContext contexto, int precadastroAlunoId)
        {
            Entidades.PreCadastroAlunoFoto foto = new Entidades.PreCadastroAlunoFoto();
            SqlDataReader dataReader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT PRECADASTROALUNOFOTOID, 
		                        PRECADASTROALUNOID, 
		                        CHAVEARQUIVO, 
		                        ARQUIVO, 
		                        TIPOARQUIVO, 
		                        NOMEARQUIVO, 
		                        DATACADASTRO, 
		                        DATAALTERACAO
                        FROM Matricula.PRECADASTROALUNOFOTO
                        WHERE PRECADASTROALUNOID = @PRECADASTROALUNOID ";
                
                contextQuery.Parameters.Add("@PRECADASTROALUNOID", SqlDbType.Int, precadastroAlunoId);

                dataReader = contexto.GetDataReader(contextQuery);

                while (dataReader.Read())
                {
                    foto.PreCadastroAlunoFotoId = Convert.ToInt32(dataReader["PRECADASTROALUNOFOTOID"]);
                    foto.PreCadastroAlunoId = Convert.ToInt32(dataReader["PRECADASTROALUNOID"]);
                    foto.ChaveArquivo = Convert.ToString(dataReader["CHAVEARQUIVO"]);
                    foto.Arquivo = (byte[])dataReader["ARQUIVO"];
                    foto.TipoArquivo = Convert.ToString(dataReader["TIPOARQUIVO"]);
                    foto.NomeArquivo = Convert.ToString(dataReader["NOMEARQUIVO"]);
                }

                return foto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dataReader != null)
                {
                    dataReader.Close();
                }
            }
        }

        public void RemovePreCadastroFotoSemInscricao(DataContext contexto, string nome, string nomeMae, DateTime dataNascimento)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"  DELETE PF 
                                        FROM   MATRICULA.PRECADASTROALUNO PC
											   INNER JOIN Matricula.PRECADASTROALUNOFOTO PF (NOLOCK)
													  ON PF.PRECADASTROALUNOID = PC.PRECADASTROALUNOID
                                               LEFT JOIN MATRICULA.INSCRICAOALUNO I (NOLOCK) 
                                                      ON PC.PRECADASTROALUNOID = I.PRECADASTROALUNOID 
                                        WHERE  I.INSCRICAOALUNOID IS NULL 
                                               AND PC.NOME = @NOME
                                               AND PC.NOMEMAE = @NOMEMAE
                                               AND PC.DATANASCIMENTO = @DATANASCIMENTO ";

            contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, nome);
            contextQuery.Parameters.Add("@NOMEMAE", SqlDbType.VarChar, nomeMae);
            contextQuery.Parameters.Add("@DATANASCIMENTO", SqlDbType.DateTime, dataNascimento);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
