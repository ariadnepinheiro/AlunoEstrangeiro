using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Entity;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.CartaoEstudante.Enum;

namespace Techne.Lyceum.RN.CartaoEstudante.Query
{
    class SolicitacaoQuery : QueryBase<SolicitacaoQuery>
    {
        SolicitacaoQuery()
        {
            NomeTabela = "Lyceum.CartaoEstudante.Solicitacao";
        }

        public void Insere(Entity.Solicitacao solicitacao)
        {
            string INSERT_SOLICITACAO = @"INSERT INTO " + NomeTabela +
                " (TIPOSOLICITACAOID, MUNICIPIOID, ALUNO, OBSERVACAO, USUARIO, OPERADORAID, SITUACAO) " +
                "VALUES (@TIPOSOLICITACAOID, @MUNICIPIOID, @ALUNO, @OBSERVACAO, @USUARIO, @OPERADORAID, @SITUACAO)";

            AplicarModificacoes(INSERT_SOLICITACAO, solicitacao.TipoSolicitacaoId, solicitacao.MunicipioId, solicitacao.Aluno,
                solicitacao.Observacao, solicitacao.Usuario, solicitacao.OperadoraId, solicitacao.Situacao);
        }

        public bool PossuiCondicoesDeGeracaoEssenciais(Entity.Solicitacao solicitacao)
        {
            TipoSolicitacaoEnum tipoSolicitacao;
            bool possuiCondicoes = true;
            string consulta = "";

            if (System.Enum.IsDefined(typeof(TipoSolicitacaoEnum), solicitacao.TipoSolicitacaoId))
            {
                tipoSolicitacao = (TipoSolicitacaoEnum)solicitacao.TipoSolicitacaoId;

                if (tipoSolicitacao != TipoSolicitacaoEnum.Codigo49)
                {
                    if (tipoSolicitacao == TipoSolicitacaoEnum.Codigo1005)
                    {
                        consulta = @"SELECT COUNT(*)
                                FROM LY_ALUNO A (NOLOCK)
                                JOIN LY_PESSOA P (NOLOCK) ON P.PESSOA = A.PESSOA
                                JOIN LY_UNIDADE_ENSINO UE (NOLOCK) ON UE.UNIDADE_ENS = A.UNIDADE_ENSINO
                                JOIN LY_FL_PESSOA F (NOLOCK) ON A.PESSOA = F.PESSOA
                                --2- e cuja unidade de ensino fique em um município atendido pela operadora,
                                JOIN CartaoEstudante.MUNICIPIOBILHETAGEM MB (NOLOCK) ON MB.MUNICIPIOID = UE.MUNICIPIO
                                WHERE
                                --1- matrículas de alunos enturmados (ativos)
                                A.SIT_ALUNO = 'Ativo'
                                AND EXISTS (SELECT TOP 1 1 FROM LY_MATRICULA M (NOLOCK) WHERE M.ALUNO = A.ALUNO AND M.ANO = YEAR(GETDATE()) AND M.SIT_MATRICULA = 'Matriculado')
                                --3- e que utilizem transporte público com direito a gratuidade de qualquer modal,
                                AND F.FL_FIELD_04 = 'S'
                                --4- e com e-mail preenchido
                                AND Len(LTrim(isnull(P.E_MAIL,'')))>0
                                --6- Não insere caso o municipio da unidade de ensino seja igual aos da lista
                                AND UE.MUNICIPIO NOT IN ( '00006845','00006862','00006863','00006914','00006951','00006972',
                                '00007004','00007015','00007017','00007023','00007030','00007035',
                                '00007040','00007041','00007110','00007113','00007121')
                                AND A.ALUNO = @MATRICULA";
                    }
                    if (tipoSolicitacao == TipoSolicitacaoEnum.Codigo1006
                        || tipoSolicitacao == TipoSolicitacaoEnum.Codigo1007
                        || tipoSolicitacao == TipoSolicitacaoEnum.Codigo99)
                    {
                        consulta = @"SELECT COUNT(*)
                                FROM LY_ALUNO A (NOLOCK)
                                JOIN LY_PESSOA P (NOLOCK) ON A.PESSOA = P.PESSOA
                                --1- matrículas de alunos enturmados (ativos)
                                JOIN LY_UNIDADE_ENSINO UE (NOLOCK) ON UE.UNIDADE_ENS = A.UNIDADE_ENSINO
                                JOIN CartaoEstudante.MUNICIPIOBILHETAGEM MB (NOLOCK) ON MB.MUNICIPIOID = UE.MUNICIPIO
                                JOIN LY_FL_PESSOA F on A.PESSOA = F.PESSOA
                                WHERE
                                --1- matrículas de alunos enturmados (ativos)
                                A.SIT_ALUNO = 'Ativo'
                                --3- e que utilizem transporte público com direito a gratuidade de qualquer modal,
                                AND F.FL_FIELD_04 = 'S'
                                --4- e com e-mail preenchido
                                AND Len(LTrim(isnull(P.E_MAIL,'')))>0
                                AND exists (SELECT Top 1 1 FROM LY_MATRICULA M (NOLOCK) WHERE  M.ALUNO = A.ALUNO AND M.ANO = YEAR(GETDATE()) AND M.SIT_MATRICULA = 'Matriculado' ) 
                                AND A.ALUNO = @MATRICULA";
                    }

                    possuiCondicoes = Possui(consulta, solicitacao.Aluno);
                }
            }
            else
            { 
                possuiCondicoes = false; 
            }

            return possuiCondicoes;
        }

        public bool PossuiCondicoesDeGeracaoDesconsideraveis(Entity.Solicitacao solicitacao)
        {
            bool possuiCondicoes = true;
            string consulta = "";
            TipoSolicitacaoEnum tipoSolicitacao;

            if (System.Enum.IsDefined(typeof(TipoSolicitacaoEnum), solicitacao.TipoSolicitacaoId))
            {
                tipoSolicitacao = (TipoSolicitacaoEnum)solicitacao.TipoSolicitacaoId;

                    if (tipoSolicitacao == TipoSolicitacaoEnum.Codigo1005)
                    {
                        consulta = @"SELECT COUNT(*)
                                    FROM LY_ALUNO A (NOLOCK)
                                    JOIN LY_PESSOA P (NOLOCK) ON P.PESSOA = A.PESSOA
                                    JOIN LY_UNIDADE_ENSINO UE (NOLOCK) ON UE.UNIDADE_ENS = A.UNIDADE_ENSINO
                                    JOIN LY_FL_PESSOA F (NOLOCK) ON A.PESSOA = F.PESSOA
                                    JOIN CartaoEstudante.MUNICIPIOBILHETAGEM MB (NOLOCK) ON MB.MUNICIPIOID = UE.MUNICIPIO
                                    WHERE
                                    --5- e que ainda não tenham sido enviados para a operadora, com códigos 1005 e 1001,
                                    NOT EXISTS (SELECT TOP 1 1 FROM DBO.MATRICULAS_ATIVAS_RIOCARD_22012015 T9 (NOLOCK) WHERE T9.MATRICULA = A.ALUNO)
                                    AND NOT EXISTS (SELECT TOP 1 1
                                    FROM CartaoEstudante.SOLICITACAO S3 (NOLOCK)
                                    JOIN CartaoEstudante.TIPOSOLICITACAO TS (NOLOCK) ON S3.TIPOSOLICITACAOID = TS.TIPOSOLICITACAOID
                                    AND TS.CODSOLICITACAO = '1005'
                                    WHERE S3.ALUNO = A.ALUNO
                                    )
                                    --7- No caso de aluno concomitante, este deverá estar com status “Liberado”;
                                    AND A.ALUNO NOT IN (
                                        SELECT TOP 1 AC.ALUNO
                                        FROM TCE_ALUNO_CONCOMITANTE AC (NOLOCK)
                                        WHERE AC.ANO = YEAR(GETDATE())
                                        AND AC.ALUNO = A.ALUNO
                                        AND AC.STATUS <> 'Liberado'
                                        ORDER BY AC.ALUNO, AC.DT_CADASTRO DESC
                                    )
                                    --8- No caso de aluno de educação especial, tem que estar marcado com aceite = “1”;
                                    AND A.ALUNO NOT IN (
                                        SELECT TOP 1 ESP.ALUNO
                                        FROM TCE_ALUNO_EDUC_ESPECIAL ESP (NOLOCK)
                                        WHERE ESP.ANO = YEAR(GETDATE())
                                        AND ESP.ALUNO = A.ALUNO
                                        AND ESP.ACEITE <> 1
                                        ORDER BY ESP.ALUNO, ESP.DT_CADASTRO DESC
                                    )
                                    --9- A unidade de ensino do aluno tem que ser diferente de ESPECIAL DE UNIDADES SOCIOEDUCATIVAS E PRISIONAIS;
                                    AND UE.ID_REGIONAL <> 5
                                    AND A.ALUNO = @MATRICULA";
                    }
                    if (tipoSolicitacao == TipoSolicitacaoEnum.Codigo1006
                        || tipoSolicitacao == TipoSolicitacaoEnum.Codigo1007
                        || tipoSolicitacao == TipoSolicitacaoEnum.Codigo99)
                    {
                        consulta = @"SELECT COUNT(*)
                                    FROM LY_ALUNO A (NOLOCK)
                                    JOIN LY_PESSOA P (NOLOCK) ON A.PESSOA = P.PESSOA
                                    -- A geração do tipo de movimentação 1006 (alteração de cadastro) deverá ocorrer para as matrículas de alunos enturmados (ativos)
                                    JOIN LY_UNIDADE_ENSINO UE (NOLOCK) ON UE.UNIDADE_ENS = A.UNIDADE_ENSINO
                                    JOIN CartaoEstudante.MUNICIPIOBILHETAGEM MB (NOLOCK) ON MB.MUNICIPIOID = UE.MUNICIPIO
                                    JOIN LY_FL_PESSOA F on A.PESSOA = F.PESSOA
                                    WHERE
                                    -- que já tenham sido enviados para a operadora, com códigos 1005 ou 1001,
                                    (
                                    EXISTS (SELECT TOP 1 1 FROM DBO.MATRICULAS_ATIVAS_RIOCARD_22012015 T9 (NOLOCK) WHERE T9.MATRICULA = A.ALUNO)
                                    OR
                                    EXISTS (SELECT 1 FROM CartaoEstudante.SOLICITACAO S3 (NOLOCK) JOIN CartaoEstudante.TIPOSOLICITACAO TS ON S3.TIPOSOLICITACAOID = TS.TIPOSOLICITACAOID AND TS.CODSOLICITACAO = '1005' WHERE S3.ALUNO = A.ALUNO)
                                    )
                                    AND A.ALUNO = @MATRICULA";
                    }
                    if (tipoSolicitacao == TipoSolicitacaoEnum.Codigo49)
                    {
                        consulta = @"SELECT COUNT(*)
                                    FROM LY_ALUNO A (NOLOCK)
                                    JOIN LY_PESSOA P (NOLOCK) ON A.PESSOA = P.PESSOA
                                    -- A geração do tipo de movimentação 1006 (alteração de cadastro) deverá ocorrer para as matrículas de alunos enturmados (ativos)
                                    JOIN LY_UNIDADE_ENSINO UE (NOLOCK) ON UE.UNIDADE_ENS = A.UNIDADE_ENSINO
                                    -- JOIN MUNICIPIO MU (NOLOCK) ON MU.CODIGO = UE.MUNICIPIO
                                    JOIN CartaoEstudante.MUNICIPIOBILHETAGEM MB (NOLOCK) ON MB.MUNICIPIOID = UE.MUNICIPIO
                                    WHERE
                                    -- que já tenham sido enviados para a operadora, com códigos 1005 ou 1001,
                                    (
                                    EXISTS (SELECT TOP 1 1 FROM DBO.MATRICULAS_ATIVAS_RIOCARD_22012015 T9 (NOLOCK) WHERE T9.MATRICULA = A.ALUNO)
                                    OR
                                    EXISTS (SELECT 1 FROM CartaoEstudante.SOLICITACAO S3 (NOLOCK) JOIN CartaoEstudante.TIPOSOLICITACAO TS ON S3.TIPOSOLICITACAOID = TS.TIPOSOLICITACAOID AND TS.CODSOLICITACAO = '1005' WHERE S3.ALUNO = A.ALUNO)
                                    )
                                    AND A.ALUNO = @MATRICULA";
                    }

                    possuiCondicoes = Possui(consulta, solicitacao.Aluno);                
            }
            else
            {
                possuiCondicoes = false;
            }

            return possuiCondicoes;
        }

        internal System.Data.DataTable ListaSolicitacoesPor(string aluno)
        {
            var parameters = new List<string>();

            StringBuilder consulta = new StringBuilder(@"SELECT S.SOLICITACAOID
                                                              , S.ALUNO
                                                              , O.NOMEOPERADORA
                                                              , TS.DESCRICAO as TipoSolicitacao
                                                              , S.OBSERVACAO
                                                              , S.DATASOLICITACAO
                                                              , S.USUARIO
                                                              , RT.SITUACAOPROCESSAMENTO as SituacaoRetorno
                                                              , S.SITUACAO as SituacaoSolicitacao
                                                              , RT.DATAINCLUSAO as DataUltimoRetorno
                                                        FROM CartaoEstudante.SOLICITACAO S
                                                        JOIN CARTAOESTUDANTE.OPERADORA O ON O.OPERADORAID = S.OPERADORAID
                                                        JOIN CARTAOESTUDANTE.TIPOSOLICITACAO TS ON TS.TIPOSOLICITACAOID = S.TIPOSOLICITACAOID
                                                        LEFT JOIN CartaoEstudante.REMESSA R ON R.SOLICITACAOID = S.SOLICITACAOID
                                                        LEFT JOIN CartaoEstudante.VW_RETORNO_ULTIMA_REMESSA URT ON URT.REMESSAID = R.REMESSAID
                                                        LEFT JOIN CartaoEstudante.RETORNO RT ON RT.RETORNOID = URT.RETORNOID
                                                        WHERE S.ALUNO = @ALUNO 
                                                        ORDER BY S.SOLICITACAOID DESC");

            return base.ObterDataTable(consulta.ToString(), aluno);
        }
    }
}
