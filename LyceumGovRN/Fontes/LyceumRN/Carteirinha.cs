using Seeduc.Infra.Data;
using System;
using System.Data;
namespace Techne.Lyceum.RN
{
    public class Carteirinha : RNBase
    {
        /// <summary>
        /// Verifica se existe carteirinha com o código de barras informado
        /// </summary>
        /// <param name="codigo_de_barras">código de barras</param>
        /// <returns>true = existe carteirinha, false = não existe carteirinha</returns>
        public static bool ExisteCarteirinha(string codigo_de_barras)
        {
            var sql = @"SELECT  1
                        FROM    LY_CARTEIRINHA
                        WHERE   COD_BARRAS_CARTEIRINHA = ?";
            var retorno = ExecutarFuncao(sql, codigo_de_barras);

            return retorno == 1;
        }

        public DataTable RetornaCarteirinhaAtivaPor(DataContext contexto, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            contextQuery.Command = @" SELECT C.PESSOA AS PESSOA, 
                                           C.VIA_CARTEIRINHA AS VIA_CARTEIRINHA, 
                                           A.ALUNO 
                                    FROM   LY_CARTEIRINHA C 
                                           JOIN LY_ALUNO A 
                                             ON C.PESSOA = A.PESSOA 
                                    WHERE  A.ALUNO = @ALUNO 
                                           AND C.SIT_CARTEIRINHA = 'Ativa' 
                                    ORDER  BY VIA_CARTEIRINHA DESC ";

            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno); 

            dt = contexto.GetDataTable(contextQuery);

            return dt;
        }

        /// <summary>
        /// Verifica se o aluno já possui alguma carteirinha.
        /// </summary>
        /// <param name="aluno">código do aluno</param>
        /// <returns>true = possui carteirinha, false = não possui carteirinha</returns>
        public static bool PossuiCarteirinha(string aluno)
        {	
            RN.Carteirinha rnCarteirinha = new Carteirinha();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return rnCarteirinha.PossuiCarteirinhaPor(contexto, aluno);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
        }

        public bool PossuiCarteirinhaPor(DataContext contexto, string aluno)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM    dbo.LY_CARTEIRINHA
                                    WHERE   ALUNO = @ALUNO ";

            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        /// <summary>
        /// Verifica se o aluno possui está com a carteirinha atual bloqueada.
        /// </summary>
        /// <param name="aluno">código do aluno</param>
        /// <returns>true = possui carteirinha bloqueada, false = não possui carteirinha bloqueada</returns>
        public static bool PossuiCarteirinhaBloqueada(string aluno)
        {
            var sql = @"SELECT  COUNT(*)
                        FROM    ( SELECT    ALUNO,
                                            MAX(VIA_CARTEIRINHA) AS VIA_CARTEIRINHA
                                  FROM      LY_CARTEIRINHA
                                  WHERE     aluno = ?
                                  GROUP BY  ALUNO ) R,
                                dbo.LY_CARTEIRINHA C
                        WHERE   r.ALUNO = c.ALUNO
                                AND R.VIA_CARTEIRINHA = c.VIA_CARTEIRINHA
                                AND c.SIT_CARTEIRINHA = 'Bloqueada'";
            var retorno = ExecutarFuncao(sql, aluno);

            return retorno > 0;
        }

        public void AtivaCarteirinha(DataContext contexto, string aluno, DateTime dataReabertura)
        {	
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_CARTEIRINHA 
                        SET    SIT_CARTEIRINHA = 'Ativa', 
                               MOTIVO = 'Regularmente matriculado', 
                               DATA_ALT_SITUACAO = @DATA_ALT_SITUACAO 
                        WHERE  ALUNO = @ALUNO 
                               AND VIA_CARTEIRINHA = (SELECT TOP 1 VIA_CARTEIRINHA 
                                                      FROM   LY_CARTEIRINHA C2 
                                                      WHERE  C2.ALUNO = @ALUNO 
                                                      ORDER  BY C2.VIA_CARTEIRINHA DESC)  ";

            contextQuery.Parameters.Add("@DATA_ALT_SITUACAO", TechneDbType.T_DATA, dataReabertura);
            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);

            contexto.ApplyModifications(contextQuery);
        }

        public void BloqueiaCarteirinha(DataContext contexto, string motivo, decimal pessoa, string aluno, string viaCarteirinha, DateTime dataBloqueio)
        {
             ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_CARTEIRINHA 
                                SET    SIT_CARTEIRINHA = 'Bloqueada', 
                                       MOTIVO = @MOTIVO, 
                                       DATA_ALT_SITUACAO = @DATA_ALT_SITUACAO 
                                WHERE  PESSOA = @PESSOA 
                                       AND ALUNO = @ALUNO 
                                       AND VIA_CARTEIRINHA = @VIA_CARTEIRINHA  ";

            contextQuery.Parameters.Add("@MOTIVO", motivo);
            contextQuery.Parameters.Add("@DATA_ALT_SITUACAO", TechneDbType.T_DATA, dataBloqueio);
            contextQuery.Parameters.Add("@ALUNO", TechneDbType.T_CODIGO, aluno);
            contextQuery.Parameters.Add("@PESSOA", pessoa);
            contextQuery.Parameters.Add("@VIA_CARTEIRINHA", viaCarteirinha);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaSuspensao(DataContext contexto, int historicoSuspensaoId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_CARTEIRINHA 
                                        SET    SIT_CARTEIRINHA = 'Bloqueada', 
                                                MOTIVO = 'CANCEL_PROMOV', 
                                                DATA_ALT_SITUACAO = GETDATE() 
                                        FROM LY_CARTEIRINHA C
                                        INNER JOIN Turma.HISTORICOSUSPENSAO h ON h.ALUNO = C.ALUNO
                                        WHERE  SIT_CARTEIRINHA = 'Ativa'
	                                        and HISTORICOSUSPENSAOID = @HISTORICOSUSPENSAOID ";

            contextQuery.Parameters.Add("@HISTORICOSUSPENSAOID", SqlDbType.Int, historicoSuspensaoId);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaPessoa(DataContext contexto, decimal pessoaCorreta, decimal pessoaErrada, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO LY_CARTEIRINHA 
                                                (PESSOA, 
                                                 VIA_CARTEIRINHA, 
                                                 COD_BARRAS_CARTEIRINHA, 
                                                 ALUNO, 
                                                 SIT_CARTEIRINHA, 
                                                 MATRICULA_CATRACA, 
                                                 DT_SOLICITACAO, 
                                                 USUARIO, 
                                                 MOTIVO, 
                                                 DATA_ALT_SITUACAO, 
                                                 DT_IMPRESSAO, 
                                                 STAMP_ATUALIZACAO) 
                                    SELECT @PESSOACORRETA, 
                                           VIA_CARTEIRINHA, 
                                           COD_BARRAS_CARTEIRINHA, 
                                           ALUNO, 
                                           SIT_CARTEIRINHA, 
                                           MATRICULA_CATRACA, 
                                           DT_SOLICITACAO, 
                                           @USUARIO, 
                                           MOTIVO, 
                                           DATA_ALT_SITUACAO, 
                                           DT_IMPRESSAO, 
                                           @DATAALTERACAO 
                                    FROM   LY_CARTEIRINHA 
                                    WHERE  PESSOA = @PESSOAERRADA  ";

            contextQuery.Parameters.Add("@PESSOACORRETA", pessoaCorreta);
            contextQuery.Parameters.Add("@USUARIO", usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);
            contextQuery.Parameters.Add("@PESSOAERRADA", pessoaErrada);

            contexto.ApplyModifications(contextQuery);
        }

        public void Remove(DataContext contexto, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE LY_CARTEIRINHA                                                 
                                    WHERE  PESSOA = @PESSOA  ";

            contextQuery.Parameters.Add("@PESSOA", pessoa);

            contexto.ApplyModifications(contextQuery);
        }
    }
}