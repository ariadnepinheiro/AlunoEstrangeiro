using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN
{
    public class UnidadeEnsinoCursos : RNBase
    {
        public bool PossuiCursoPor(DataContext ctx, string curso)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(1) 
                                FROM LY_UNIDADE_ENSINO_CURSOS
                                WHERE CURSO = @CURSO ";

            contextQuery.Parameters.Add("@CURSO", curso);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public static DataTable Listar(string censo)
        {
            var contextQuery = new ContextQuery(
                @"  SELECT  DISTINCT uc.UNIDADE_ENS AS unidade_ens,
                            uc.ATO AS ato,
                            uc.DT_IMPLANTACAO AS dt_implantacao,
                            uc.DT_DO AS dt_do,
                            c.NOME AS nome,
                            c.CURSO AS codigo,
                            mc.DESCRICAO AS modalidade,
                            tp.DESCRICAO AS nivel,
                            OBSERVACOES,
                            PROCESSO
                    FROM    LY_UNIDADE_ENSINO_CURSOS uc
                            JOIN LY_CURSO c ON uc.CURSO = c.CURSO                            
                            LEFT JOIN LY_MODALIDADE_CURSO mc ON c.MODALIDADE = mc.MODALIDADE
                            LEFT JOIN LY_TIPO_CURSO tp ON tp.TIPO = c.TIPO
                    WHERE   uc.UNIDADE_ENS =  @CENSO
                    ORDER BY c.NOME
                                     ");

            contextQuery.Parameters.Add("@CENSO", censo);

            return Consultar(contextQuery);
        }

        public static DataTable Listar(string censo,string curso)
        {
            var contextQuery = new ContextQuery(
                @"  SELECT  DISTINCT uc.UNIDADE_ENS AS unidade_ens,
                            case when UC.ATO IS NULL 
                                      OR UC.ATO = '' tHEN UC.PROCESSO ELSE UC.ATO END AS ato,
                            uc.DT_IMPLANTACAO AS dt_implantacao,
                            uc.DT_DO AS dt_do,
                            c.NOME AS nome,
                            c.CURSO AS codigo,
                            mc.DESCRICAO AS modalidade,
                            tp.DESCRICAO AS nivel,
                            OBSERVACOES,
                            PROCESSO
                    FROM    LY_UNIDADE_ENSINO_CURSOS uc
                            JOIN LY_CURSO c ON uc.CURSO = c.CURSO                            
                            LEFT JOIN LY_MODALIDADE_CURSO mc ON c.MODALIDADE = mc.MODALIDADE
                            LEFT JOIN LY_TIPO_CURSO tp ON tp.TIPO = c.TIPO
                    WHERE   uc.UNIDADE_ENS =  @CENSO AND
                            C.CURSO = @CURSO
                    ORDER BY c.NOME
                                     ");

            contextQuery.Parameters.Add("@CENSO", censo);
            contextQuery.Parameters.Add("@CURSO", curso);

            return Consultar(contextQuery);
        }

        private static decimal GerarOrdem(string censo)
        {
            var contextQuery = new ContextQuery(
                @" SELECT ISNULL(MAX(ORDEM), 0) + 1
                     FROM   LY_UNIDADE_ENSINO_CURSOS
                     WHERE  UNIDADE_ENS = @CENSO ");

            contextQuery.Parameters.Add("@CENSO", censo);

            return ExecutarFuncao(contextQuery);
        }

        public static void Inserir(LyUnidadeEnsinoCursos unidadeEnsinoCursos)
        {
            unidadeEnsinoCursos.Ordem = GerarOrdem(unidadeEnsinoCursos.UnidadeEns);

            var turnos = RN.Turno.Consultar();

            foreach (SimpleRow tur in turnos.Rows)
            {
                var contextQuery = new ContextQuery(
                    @" INSERT  INTO LY_UNIDADE_ENSINO_CURSOS ( UNIDADE_ENS, ORDEM, CURSO, ATO, DT_IMPLANTACAO, DT_DO,
                                                        OBSERVACOES,PROCESSO, TURNO, MATRICULA )
                VALUES  ( @UNIDADE_ENS, @ORDEM, @CURSO, @ATO,
                          @DT_IMPLANTACAO, @DT_DO, @OBSERVACOES,@PROCESSO ,@TURNO, @MATRICULA ) ");

                contextQuery.Parameters.Add("@UNIDADE_ENS", unidadeEnsinoCursos.UnidadeEns);
                contextQuery.Parameters.Add("@ORDEM", unidadeEnsinoCursos.Ordem);
                contextQuery.Parameters.Add("@CURSO", unidadeEnsinoCursos.Curso);
                contextQuery.Parameters.Add("@ATO", unidadeEnsinoCursos.Ato);
                contextQuery.Parameters.Add("@DT_IMPLANTACAO", unidadeEnsinoCursos.DtImplantacao);
                contextQuery.Parameters.Add("@DT_DO", unidadeEnsinoCursos.DtDo);
                contextQuery.Parameters.Add("@OBSERVACOES", unidadeEnsinoCursos.Observacoes);
                contextQuery.Parameters.Add("@PROCESSO", unidadeEnsinoCursos.Processo);
                contextQuery.Parameters.Add("@MATRICULA", unidadeEnsinoCursos.Matricula);
                contextQuery.Parameters.Add("@TURNO",tur["TURNO"].ToString());

                ExecutarAlteracao(contextQuery);
            }
        }

        public static void Alterar(LyUnidadeEnsinoCursos unidadeEnsinoCursos)
        {
            var contextQuery = new ContextQuery(
            @" UPDATE  LY_UNIDADE_ENSINO_CURSOS
                SET     ATO = @ATO,
                        DT_IMPLANTACAO = @DT_IMPLANTACAO, 
                        DT_DO = @DT_DO,
                        OBSERVACOES = @OBSERVACOES, 
                        PROCESSO = @PROCESSO, 
                        MATRICULA = @MATRICULA,
                        DT_ALTERACAO = GETDATE()
                WHERE   UNIDADE_ENS = @UNIDADE_ENS
                        AND CURSO = @CURSO
                         ");

            contextQuery.Parameters.Add("@UNIDADE_ENS", unidadeEnsinoCursos.UnidadeEns);
            contextQuery.Parameters.Add("@CURSO", unidadeEnsinoCursos.Curso);
            contextQuery.Parameters.Add("@ATO", unidadeEnsinoCursos.Ato);
            contextQuery.Parameters.Add("@DT_IMPLANTACAO", unidadeEnsinoCursos.DtImplantacao);
            contextQuery.Parameters.Add("@DT_DO", unidadeEnsinoCursos.DtDo);
            contextQuery.Parameters.Add("@OBSERVACOES", unidadeEnsinoCursos.Observacoes);
            contextQuery.Parameters.Add("@PROCESSO", unidadeEnsinoCursos.Processo);
            contextQuery.Parameters.Add("@MATRICULA", unidadeEnsinoCursos.Matricula);

            ExecutarAlteracao(contextQuery);
        }

        public static ValidacaoDados Validar(LyUnidadeEnsinoCursos unidadeEnsinoCursos)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (unidadeEnsinoCursos == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(unidadeEnsinoCursos.Curso))
            {
                mensagens.Add("O campo CURSO é obrigatório!");
            }

            if (unidadeEnsinoCursos.DtImplantacao == null || unidadeEnsinoCursos.DtImplantacao <= DateTime.MinValue)
            {
                mensagens.Add("O campo DATA DE IMPLANTAÇÃO é obrigatório!");
            }
            else
            {
                if (Convert.ToDateTime(unidadeEnsinoCursos.DtImplantacao).Date > DateTime.Now.Date)
                {
                    mensagens.Add("DATA DE IMPLANTAÇÃO não pode ser maior que data atual.");
                }                
            }

            if (unidadeEnsinoCursos.DtDo == null || unidadeEnsinoCursos.DtDo <= DateTime.MinValue)
            {
                mensagens.Add("O campo DATA DO é obrigatório!");
            }
            else
            {
                if (Convert.ToDateTime(unidadeEnsinoCursos.DtDo).Date > DateTime.Now.Date)
                {
                    mensagens.Add("DATA DO não pode ser maior que data atual.");
                }
            }

            if (string.IsNullOrEmpty(unidadeEnsinoCursos.Ato) && string.IsNullOrEmpty(unidadeEnsinoCursos.Processo))
            {
                mensagens.Add("Favor preencher o NÚMERO DO ATO ou PROCESSO DE AUTORIZAÇÃO.");
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public static void Remover(string unidadeEnsino, string curso)
        {
            if (string.IsNullOrEmpty(unidadeEnsino) || string.IsNullOrEmpty(curso))
            {
                return;
            }

            var contextQuery = new ContextQuery(
                @"DELETE  LY_UNIDADE_ENSINO_CURSOS
                     WHERE   UNIDADE_ENS = @UNIDADE_ENS
                        AND CURSO = @CURSO ");

            contextQuery.Parameters.Add("@UNIDADE_ENS", unidadeEnsino);
            contextQuery.Parameters.Add("@CURSO", curso);

            ExecutarAlteracao(contextQuery);
        }
    }
}
