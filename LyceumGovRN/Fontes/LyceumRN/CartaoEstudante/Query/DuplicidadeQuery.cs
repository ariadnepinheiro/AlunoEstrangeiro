using System;
using Techne.Lyceum.RN.CartaoEstudante.Entity;
using Seeduc.Infra.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

namespace Techne.Lyceum.RN.CartaoEstudante.Query
{
    class DuplicidadeQuery : QueryBase<DuplicidadeQuery>
    {
        private const string TABELA_DUPLICIDADE = "Lyceum.CartaoEstudante.Duplicidade";

        DuplicidadeQuery() { }

        public void InsereDuplicidade(Duplicidade duplicidade)
        {
            string INSERT_DUPLICIDADE = "INSERT INTO " + TABELA_DUPLICIDADE +
                    " (OPERADORAID, ALUNO, IDBENEFICIARIO, FLAGMATRICULAPRINCIPAL, DATAFLAGMATRICULAPRINCIPAL, DATAATUALIZACAO) " +
                    "VALUES " +
                    " (@OPERADORAID, @ALUNO, @IDBENEFICIARIO, @FLAGMATRICULAPRINCIPAL, @DATAFLAGMATRICULAPRINCIPAL, @DATAATUALIZACAO)";

            AplicarModificacoes(
                INSERT_DUPLICIDADE,
                duplicidade.OperadoraId, duplicidade.Aluno, duplicidade.IdBeneficiario,
                duplicidade.FlagMatriculaPrincipal, duplicidade.DataFlagMatriculaPrincipal, duplicidade.DataAtualizacao
            );
        }

        internal string ObtemIdBeneficiario(string aluno) {
            var sql = new StringBuilder();

            sql.Append(@" SELECT DISTINCT IDBENEFICIARIO FROM [CartaoEstudante].[DUPLICIDADE]
                        WHERE ALUNO = " + aluno );


            return base.ObtemValorSimples<string>(sql.ToString());
        }

        internal System.Data.DataTable ObtemDuplicidades(string IdBeneficiario, string unidadeEnsino, string regional)
        {
            var sql = new StringBuilder();

            sql.Append(@"SELECT UE.ID_REGIONAL, UE.UNIDADE_ENS, O.NOMEOPERADORA, D.ALUNO, PE.NOME_COMPL, UPPER(A.SIT_ALUNO) SIT_ALUNO, D.IDBENEFICIARIO, 
                        CASE D.FLAGMATRICULAPRINCIPAL WHEN 'S' THEN 'SIM' WHEN 'N' THEN 'NÃO' END AS FLAGMATRICULAPRINCIPAL
                        , CAST(D.DATAATUALIZACAO AS DATE) AS DATAATUALIZACAO, CAST(MAX(D.DATAINCLUSAO) AS DATE) AS DATAINCLUSAO
                        FROM [CartaoEstudante].[DUPLICIDADE] D 
                        JOIN LY_ALUNO A ON D.ALUNO = A.ALUNO
                        INNER JOIN LY_PESSOA PE (NOLOCK)  ON PE.PESSOA = A.PESSOA
                        JOIN [CartaoEstudante].[OPERADORA] O ON D.OPERADORAID = O.OPERADORAID
                        LEFT JOIN VW_UNIDADE_ENSINO_SITUACAO UE     ON UE.UNIDADE_ENS = A.UNIDADE_ENSINO
                        ");

            if (!string.IsNullOrEmpty(IdBeneficiario))
            {
                sql.Append(String.Format(@" WHERE D.IDBENEFICIARIO IN (
                                                                        SELECT IDBENEFICIARIO 
                                                                        FROM CARTAOESTUDANTE.DUPLICIDADE D2
                                                                        JOIN LY_ALUNO A ON A.ALUNO = D2.ALUNO
                                                                        LEFT JOIN VW_UNIDADE_ENSINO_SITUACAO UE ON UE.UNIDADE_ENS = A.UNIDADE_ENSINO
                                                                        WHERE D2.IDBENEFICIARIO = '{0}')"
                , IdBeneficiario));
            }
            else if (!string.IsNullOrEmpty(unidadeEnsino))
            {
                sql.Append(String.Format(@" WHERE D.IDBENEFICIARIO IN (
                                                                        SELECT IDBENEFICIARIO 
                                                                        FROM CARTAOESTUDANTE.DUPLICIDADE D2
                                                                        JOIN LY_ALUNO A ON A.ALUNO = D2.ALUNO
                                                                        LEFT JOIN VW_UNIDADE_ENSINO_SITUACAO UE ON UE.UNIDADE_ENS = A.UNIDADE_ENSINO
                                                                        WHERE A.UNIDADE_ENSINO = {0})"
                , unidadeEnsino));
            }
            else if (!string.IsNullOrEmpty(regional))
            {
                sql.Append(String.Format(@" WHERE D.IDBENEFICIARIO IN (
                                                                        SELECT IDBENEFICIARIO 
                                                                        FROM CARTAOESTUDANTE.DUPLICIDADE D2
                                                                        JOIN LY_ALUNO A ON A.ALUNO = D2.ALUNO
                                                                        LEFT JOIN VW_UNIDADE_ENSINO_SITUACAO UE ON UE.UNIDADE_ENS = A.UNIDADE_ENSINO
                                                                        WHERE UE.ID_REGIONAL = {0})"
                , regional));
            }

            sql.Append(@" GROUP BY UE.ID_REGIONAL,UE.UNIDADE_ENS, O.NOMEOPERADORA, D.ALUNO, PE.NOME_COMPL, A.SIT_ALUNO, D.IDBENEFICIARIO, D.FLAGMATRICULAPRINCIPAL, D.DATAATUALIZACAO
                          ORDER BY IDBENEFICIARIO
                        ");


            return base.ObterDataTable(sql.ToString());
        }
    }
}
