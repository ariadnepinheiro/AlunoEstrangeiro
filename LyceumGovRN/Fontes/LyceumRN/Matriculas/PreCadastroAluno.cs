using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.DTOs;

namespace Techne.Lyceum.RN.Matriculas
{
    public class PreCadastroAluno
    {
        public void AtualizaPessoa(DataContext contexto, int preCadastroAlunoId, decimal pessoa, string usuarioResponsavel)
        {	
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" --Limpa pessoa caso esteja em precadastro errado
                                        UPDATE MATRICULA.PRECADASTROALUNO
	                                        SET PESSOAID = null,
                                                USUARIOID = @USUARIOID,
                                                DATAALTERACAO = @DATAALTERACAO
                                        WHERE PRECADASTROALUNOID <> @PRECADASTROALUNOID
	                                        AND PESSOAID = @PESSOA

                                        --Atualiza para pessoa correta
                                        UPDATE MATRICULA.PRECADASTROALUNO
                                                SET PESSOAID = @PESSOA,
                                                    USUARIOID = @USUARIOID,
                                                    DATAALTERACAO = @DATAALTERACAO
                                        WHERE PRECADASTROALUNOID = @PRECADASTROALUNOID ";

            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, pessoa);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@PRECADASTROALUNOID", SqlDbType.Int, preCadastroAlunoId);

            contexto.ApplyModifications(contextQuery);
        }

        public void RemovePreCadastroSemInscricao(DataContext contexto, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"   DELETE PC 
                                        FROM   MATRICULA.PRECADASTROALUNO PC
                                               LEFT JOIN MATRICULA.INSCRICAOALUNO I (NOLOCK) 
                                                      ON PC.PRECADASTROALUNOID = I.PRECADASTROALUNOID 
                                        WHERE  I.INSCRICAOALUNOID IS NULL 
                                               AND PC.PESSOAID = @PESSOAID ";

            contextQuery.Parameters.Add("@PESSOAID", SqlDbType.Decimal, pessoa);

            contexto.ApplyModifications(contextQuery);
        }

        public void RemovePreCadastroSemInscricao(DataContext contexto, string nome, string nomeMae, DateTime dataNascimento)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"   DELETE PC 
                                        FROM   MATRICULA.PRECADASTROALUNO PC
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

        public void MoveInscricoesDePreCadastroAntigoParaPreCadastroNovo(DataContext contexto, int inscricaoAlunoId, string nome, string nomeMae, DateTime dataNascimento, string usuarioId)
        {
            /*
            Esta função move as inscrições de um pré-cadastro para o outro.
            Para que isto aconteça, o novo pré-cadastro (aquele que vai receber as inscrições do antigo pré-cadastro)
            não pode receber inscrição de um ano que já exista no pré-cadastro novo.
            */
 	        ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"
            declare @NOVOPRECADASTROALUNOID int
            declare @ANO int

            --obter o novo pré-cadastro e o ano baseado na inscrição fornecida
            select @NOVOPRECADASTROALUNOID = PRECADASTROALUNOID, @ANO = ANO from Matricula.INSCRICAOALUNO where INSCRICAOALUNOID = @INSCRICAOALUNOID

            /*
            atualizar o pré-cadastro que atende ao conjunto de dados (NOME, NOMEMAE, DATANASCIMENTO),
            que não seja o novo pré-cadastro, e que não tenha inscrição de ano referente ao ano
            obtido na inscrição do novo pré-cadastro.
            a idéia é não deixar que o pré-cadastro tenha + de 1 inscrição para o mesmo ano.
            */
            update i set
            i.PRECADASTROALUNOID = @NOVOPRECADASTROALUNOID
            ,i.USUARIO = @USUARIOID
            from
            Matricula.INSCRICAOALUNO i, Matricula.PRECADASTROALUNO pc
            where
            i.PRECADASTROALUNOID = pc.PRECADASTROALUNOID
            and i.ANO <> @ANO
            and pc.PRECADASTROALUNOID <> @NOVOPRECADASTROALUNOID
            and pc.NOME = @NOME
            and pc.NOMEMAE = @NOMEMAE
            and pc.DATANASCIMENTO = @DATANASCIMENTO
            ";

            contextQuery.Parameters.Add("@INSCRICAOALUNOID", SqlDbType.Int, inscricaoAlunoId);
            contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, nome);
            contextQuery.Parameters.Add("@NOMEMAE", SqlDbType.VarChar, nomeMae);
            contextQuery.Parameters.Add("@DATANASCIMENTO", SqlDbType.DateTime, dataNascimento);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioId);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaPessoa(DataContext contexto, decimal pessoaAtual, decimal pessoaNova)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"   UPDATE MATRICULA.PRECADASTROALUNO
											SET PESSOAID = @PESSOANOVA
                                        WHERE PESSOAID = @PESSOAATUAL ";

            contextQuery.Parameters.Add("@PESSOANOVA", SqlDbType.Decimal, pessoaNova);
            contextQuery.Parameters.Add("@PESSOAATUAL", SqlDbType.Decimal, pessoaAtual);

            contexto.ApplyModifications(contextQuery);
        }

        public void LimpaOutraPessoa(DataContext contexto, int precadastroAlunoId, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @"   UPDATE MATRICULA.PRECADASTROALUNO
											SET PESSOAID = null
                                        WHERE PESSOAID = @PESSOA 
                                            AND PRECADASTROALUNOID <> @PRECADASTROALUNOID ";

            contextQuery.Parameters.Add("@PRECADASTROALUNOID", SqlDbType.Int, precadastroAlunoId);
            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, pessoa);

            contexto.ApplyModifications(contextQuery);
        }

        public void LimpaPessoaPreCadastroComInscricao(DataContext contexto, decimal pessoa, string usuarioResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" 	INSERT INTO [MATRICULA].[DUPLICIDADEPRECADASTRO] 
                                                (PRECADASTROALUNOID, 
                                                 INSCRICAOALUNOID, 
                                                 PESSOAID, 
                                                 CPF, 
                                                 USUARIOID, 
                                                 DATACADASTRO, 
                                                 DATAALTERACAO) 
                                    SELECT PC.PRECADASTROALUNOID, 
                                           INSCRICAOALUNOID, 
                                           PESSOAID, 
                                           CPF, 
                                           @USUARIO, 
                                           GETDATE(), 
                                           GETDATE() 
                                    FROM   MATRICULA.PRECADASTROALUNO PC 
                                           INNER JOIN MATRICULA.INSCRICAOALUNO I (NOLOCK) 
                                                   ON PC.PRECADASTROALUNOID = I.PRECADASTROALUNOID 
                                    WHERE  PC.PESSOAID = @PESSOAID 

                                    UPDATE PC
										SET PESSOAID = NULL,
                                        CPF = NULL
                                        FROM   MATRICULA.PRECADASTROALUNO PC
                                               INNER JOIN MATRICULA.INSCRICAOALUNO I (NOLOCK) 
                                                      ON PC.PRECADASTROALUNOID = I.PRECADASTROALUNOID 
                                        WHERE  PC.PESSOAID = @PESSOAID ";

            contextQuery.Parameters.Add("@PESSOAID", SqlDbType.Decimal, pessoa);
            contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuarioResponsavel);
            
            contexto.ApplyModifications(contextQuery);
        }

        public decimal ObtemPessoaPor(DataContext contexto, string nome, DateTime nascimento, string mae)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            decimal retorno = -1;

            try
            {
                contextQuery.Command = @" SELECT ISNULL(PESSOAID, 0) PESSOA
                                            FROM MATRICULA.PRECADASTROALUNO (NOLOCK)
                                            WHERE NOME = @NOME
		                                            AND DATANASCIMENTO = @DATANASCIMENTO
		                                            AND ISNULL(NOMEMAE, '') = @NOMEMAE ";

                contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, nome);
                contextQuery.Parameters.Add("@DATANASCIMENTO", SqlDbType.DateTime, nascimento);
                contextQuery.Parameters.Add("@NOMEMAE", SqlDbType.VarChar, mae);  

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToDecimal(reader["PESSOA"]);
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

        public RN.Entidades.LyPessoa ObtemPessoaPor(DataContext contexto, string matricula)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" SELECT P.*
                        FROM   LY_ALUNO A (NOLOCK) 
                               INNER JOIN LY_PESSOA P (NOLOCK) 
                                       ON A.PESSOA = P.PESSOA
                        WHERE A.ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", SqlDbType.VarChar, matricula);

            return contexto.TryToBindEntity<RN.Entidades.LyPessoa>(contextQuery);
        }

        public bool PossuiNecessidadeEspecialPor(DataContext ctx, int preCadastroAlunoId)
        {
            string necessidadeEspecial = string.Empty;
            bool possui = false;

            ContextQuery contextQuery = new ContextQuery
            {
                Command = @" SELECT COUNT(*)
                            FROM MATRICULA.PRECADASTROALUNO
                            WHERE PRECADASTROALUNOID = @PRECADASTROALUNOID
                            AND NECESSIDADEESPECIALID <> 30 "               
            };

            contextQuery.Parameters.Add("@PRECADASTROALUNOID", preCadastroAlunoId);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public void AtualizaPessoa(DataContext contexto, decimal pessoa, string nome, DateTime nascimento, string mae, string usuarioResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE MATRICULA.PRECADASTROALUNO
                                        SET PESSOAID = @PESSOA,
                                            USUARIOID = @USUARIOID,
                                            DATAALTERACAO = @DATAALTERACAO
                                        WHERE NOME = @NOME
											  AND DATANASCIMENTO = @DATANASCIMENTO
											  AND ISNULL(NOMEMAE, '') = @NOMEMAE ";

            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, pessoa);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, nome);
            contextQuery.Parameters.Add("@NOMEMAE", SqlDbType.VarChar, mae);
            contextQuery.Parameters.Add("@DATANASCIMENTO", SqlDbType.DateTime, nascimento);

            contexto.ApplyModifications(contextQuery);
        }

        public void Insere(DataContext contexto, decimal pessoa, string usuarioResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO MATRICULA.PRECADASTROALUNO 
                                                (PESSOAID, 
                                                 NOME, 
                                                 EMAIL, 
                                                 DATANASCIMENTO, 
                                                 RESPONSAVEL, 
                                                 NOMEMAE, 
                                                 MAECPF, 
                                                 NOMEPAI, 
                                                 PAICPF, 
                                                 RESPONSAVELNOME, 
                                                 RESPONSAVELFONE, 
                                                 RESPONSAVELCPF, 
                                                 CPF, 
                                                 SEXO, 
                                                 ESTADOCIVIL, 
                                                 NACIONALIDADE, 
                                                 MUNICIPIONASCIMENTO, 
                                                 CELULAR, 
                                                 FIXOCELULAR, 
                                                 CEP, 
                                                 ENDERECO, 
                                                 NUMEROENDERECO, 
                                                 COMPLEMENTOENDERECO, 
                                                 BAIRRO, 
                                                 MUNICIPIOENDERECO, 
                                                 MATRICULACERTIDAO, 
                                                 TERMOCERTIDAO, 
                                                 FOLHACERTIDAO, 
                                                 LIVROCERTIDAO, 
                                                 NECESSIDADEESPECIALID, 
                                                 MODELOCERTIDAO, 
                                                 TIPOCERTIDAO, 
                                                 USUARIOID, 
                                                 DATACADASTRO, 
                                                 DATAALTERACAO,
												 REDEORIGEM,
												 NUMERORG,
												 ORGAORG,
												 UFRG) 
                                    SELECT DISTINCT 
	                                       P.PESSOA, 
                                           NOME_COMPL, 
                                           P.E_MAIL, 
                                           DT_NASC, 
                                           RESPONSAVEL, 
                                           NOME_MAE, 
                                           MAE_CPF, 
                                           NOME_PAI, 
                                           PAI_CPF, 
                                           RESP_NOME_COMPL, 
                                           RESP_FONE, 
                                           RESP_CPF, 
                                           CASE 
												WHEN EXISTS (SELECT TOP 1 1 
													FROM MATRICULA.PRECADASTROALUNO PA 
													WHERE PA.CPF = P.CPF) THEN NULL
												ELSE CPF
										   END CPF, 
                                           SEXO, 
                                           EST_CIVIL, 
                                           NACIONALIDADE, 
                                           MUNICIPIO_NASC, 
                                           CELULAR, 
                                           FONE, 
                                           CEP, 
                                           ENDERECO, 
                                           END_NUM, 
                                           END_COMPL, 
                                           BAIRRO, 
                                           END_MUNICIPIO, 
                                           CERT_NUMERO_MATRICULA, 
                                           CERT_NASC_NUM, 
                                           CERT_NASC_FOLHA, 
                                           CERT_NASC_LIVRO, 
                                           NECESSIDADEESPECIALID, 
                                           FL_FIELD_09, 
                                           FL_FIELD_02, 
                                           @USUARIO, 
                                           @DATAATUAL, 
                                           @DATAATUAL,
										   A.REDE_ENSINO_ORIGEM,
										   CASE
												WHEN RG_TIPO = 'RG' THEN RG_NUM 
												ELSE NULL
										   END RG_NUM,
										   CASE
												WHEN RG_TIPO = 'RG' THEN RG_EMISSOR 
												ELSE NULL
										   END RG_EMISSOR,
										   CASE
												WHEN RG_TIPO = 'RG' THEN RG_UF 
												ELSE NULL
										   END RG_UF
                                    FROM   LY_PESSOA P (NOLOCK) 
                                           LEFT JOIN LY_FL_PESSOA FL (NOLOCK) 
                                                   ON P.PESSOA = FL.PESSOA 
										   LEFT JOIN Matricula.PESSOAALUNO PA (NOLOCK) 
												   ON P.PESSOA = PA.PESSOAID
										   LEFT JOIN LY_ALUNO A (NOLOCK)
												   ON PA.ALUNO = A.ALUNO
                                    WHERE p.PESSOA = @PESSOA ";

            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, pessoa);
            contextQuery.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@DATAATUAL", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaDadosIdentificacao(DataContext contexto, string nome, string nomeMae, DateTime dataNascimento, decimal pessoa, string usuarioResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE P 
                        SET  NOME = @NOME_COMPL, 
                             DATANASCIMENTO = @DT_NASC, 
                             NOMEMAE = @NOME_MAE,
                             USUARIOID = @USUARIOID, 
                             DATAALTERACAO = @DATAALTERACAO 
                    FROM MATRICULA.PRECADASTROALUNO P
					WHERE  PESSOAID = @PESSOA
						   AND NOT EXISTS (SELECT TOP 1 1 
									FROM MATRICULA.PRECADASTROALUNO PC
									WHERE PC.NOME = @NOME_COMPL
										  AND PC.DATANASCIMENTO = @DT_NASC
										  AND ISNULL(PC.NOMEMAE, '') = ISNULL(@NOME_MAE, '')  )  ";

            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, pessoa);
            contextQuery.Parameters.Add("@NOME_COMPL", SqlDbType.VarChar, nome);
            contextQuery.Parameters.Add("@NOME_MAE", SqlDbType.VarChar, nomeMae);
            contextQuery.Parameters.Add("@DT_NASC", SqlDbType.DateTime, dataNascimento);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public DadosEncaminhamentoEspecial ObtemDadosEncaminhamentoEspecialPor(DataContext contexto, string nome, string nomeMae, DateTime dataNascimento)
        {
            DadosEncaminhamentoEspecial dados = new DadosEncaminhamentoEspecial();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT   PC.PESSOAID,
	                                               PC.NOME, 
                                                   PC.DATANASCIMENTO, 
                                                   PC.NOMEMAE, 
                                                   PC.SEXO, 
                                                   PC.NOMEPAI, 
                                                   PC.CEP, 
                                                   PC.ENDERECO, 
                                                   PC.NUMEROENDERECO, 
                                                   PC.COMPLEMENTOENDERECO, 
                                                   PC.MUNICIPIOENDERECO, 
                                                   M.NOME AS DESCRICAOMUNICIPIO, 
                                                   M.UF, 
                                                   PC.BAIRRO, 
                                                   PC.CPF,
                                                   PC.NECESSIDADEESPECIALID,
	                                               PC.PRECADASTROALUNOID,
	                                               PA.ALUNO
                                            FROM  Matricula.PRECADASTROALUNO pc (NOLOCK)
													LEFT JOIN HADES.DBO.TCE_MUNICIPIO M (NOLOCK) 
                                                           ON PC.MUNICIPIOENDERECO = M.ID_MUNICIPIO 
	                                               LEFT JOIN Matricula.PESSOAALUNO pa (NOLOCK) 
			                                               ON pa.PESSOAID = PC.PESSOAID
                                            WHERE  PC.NOME = @NOME 
                                                   AND PC.DATANASCIMENTO = @DT_NASC 
                                                   AND PC.NOMEMAE = @NOMEMAE  ";

                contextQuery.Parameters.Add("@NOME", SqlDbType.VarChar, nome);
                contextQuery.Parameters.Add("@DT_NASC", SqlDbType.DateTime, dataNascimento);
                contextQuery.Parameters.Add("@NOMEMAE", SqlDbType.VarChar, nomeMae);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dados.Pessoa = reader["PESSOAID"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["PESSOAID"]);
                    dados.Nome = Convert.ToString(reader["NOME"]);
                    dados.DataNascimento = reader["DATANASCIMENTO"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DATANASCIMENTO"]);
                    dados.NomeMae = Convert.ToString(reader["NOMEMAE"]);
                    dados.Sexo = Convert.ToString(reader["SEXO"]);
                    dados.NomePai = Convert.ToString(reader["NOMEPAI"]);
                    dados.Cep = Convert.ToString(reader["CEP"]);
                    dados.Endereco = Convert.ToString(reader["ENDERECO"]);
                    dados.NumeroEndereco = Convert.ToString(reader["NUMEROENDERECO"]);
                    dados.ComplementoEndereco = Convert.ToString(reader["COMPLEMENTOENDERECO"]);
                    dados.MunicipioEndereco = Convert.ToString(reader["MUNICIPIOENDERECO"]);
                    dados.DescricaoMunicipioEndereco = Convert.ToString(reader["DESCRICAOMUNICIPIO"]);
                    dados.UfEndereco = Convert.ToString(reader["UF"]);
                    dados.Bairro = Convert.ToString(reader["BAIRRO"]);
                    dados.Cpf = Convert.ToString(reader["CPF"]);
                    dados.NecessidadeEspecialId = reader["NECESSIDADEESPECIALID"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["NECESSIDADEESPECIALID"]);
                    dados.PreCadastroAlunoId = reader["PRECADASTROALUNOID"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["PRECADASTROALUNOID"]);
                }

                dados.MaeNãoDeclarada = dados.NomeMae == "NÃO DECLARADA" ? true : false;
                dados.PaiNãoDeclarado = dados.NomePai == "NÃO DECLARADO" ? true : false;

                return dados;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }
    }
}
