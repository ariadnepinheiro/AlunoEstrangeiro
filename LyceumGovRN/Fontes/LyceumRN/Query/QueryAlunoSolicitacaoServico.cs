namespace Techne.Lyceum.RN.Query
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web.UI.WebControls;
    using Data;
    using Techne.Web;

    public class QueryAlunoSolicitacaoServico : LyceumQuery
    {
        public QueryAlunoSolicitacaoServico()
        {
            this.GridColumns.Add(new TSearchColumn { FieldName = "aluno", Caption = "Matrícula", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "nome", Caption = "Nome", Width = Unit.Percentage(20) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "rg_num", Caption = "Documento", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "cpf", Caption = "CPF", Width = Unit.Percentage(8) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "mae", Caption = "Nome da Mãe", Width = Unit.Percentage(18) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "pai", Caption = "Nome do Pai", Width = Unit.Percentage(18) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "dt_nascimento", Caption = "Nascimento", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn { FieldName = "pessoa", Caption = "pessoa", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn { FieldName = "numinscricao", Caption = "Inscrição Matrícula Fácil", Width = Unit.Percentage(18) });

            this.Messages.KeyNotFound = "Matrícula inválida";
            this.TextField = "aluno";
            this.MaxLength = 20;
            this.DescriptionField = "nome";

            this.GridFilterParameters.Add("nome", "Nome", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("aluno", "Matrícula", TSearchDataType.String, 20);
            this.GridFilterParameters.Add("mae", "Mãe", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("pai", "Pai", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("numinscricao", "Inscrição Matrícula Fácil", TSearchDataType.String, 100);

            this.GridFilterParameters.Add(new TSearchParameter { Caption = "ativo", ParameterName = "ativo", ShowInFilterPanel = false, ParameterType = TSearchDataType.String });
            this.MaxRows = 50;
            this.GridWidth = Unit.Pixel(860);
        }

        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            var parValues = new ArrayList();
            var sql = new StringBuilder();

            sql.Append(string.Format(
                            @"SELECT DISTINCT aluno, numinscricao, pai, mae, pessoa, nome, rg_num, cpf, dt_nascimento FROM (
                              SELECT DISTINCT TOP {0}
                                     a.aluno,
                                     a.numinscricao,
                                     p.nome_pai AS pai,
                                     p.nome_mae AS mae,
                                     p.pessoa,
                                     p.nome_compl AS nome,
                                     p.rg_num,
                                     p.cpf,
                                     CONVERT(VARCHAR(10), p.DT_NASC, 103) AS dt_nascimento
                                FROM USUARIO u WITH (NOLOCK),
                                     LY_ALUNO a WITH (NOLOCK)
                               INNER JOIN LY_PESSOA p WITH (NOLOCK)
                                  ON p.PESSOA = a.PESSOA
                               INNER JOIN LY_UNIDADE_ENSINO UE
                                  ON UE.UNIDADE_ENS = A.UNIDADE_ENSINO
                                 AND UE.ID_REGIONAL <> 5
                               WHERE u.USUARIO = '{1}'
                                 AND (
                                       EXISTS ( 
                                                SELECT TOP 1 UNIDADE_FIS
                                                  FROM LY_USUARIO_UNIDADE_FIS usuuni WITH (NOLOCK)
                                                 WHERE usuuni.UNIDADE_FIS = a.UNIDADE_FISICA
                                                   AND usuuni.USUARIO = u.USUARIO
                                                   AND u.PRIVIL <> 'S' 
                                              )
                                           OR (U.PRIVIL = 'S')
                                     )
                                 AND a.ALUNO NOT IN (
                                                      SELECT ALUNO 
                                                        FROM TCE_ALUNO_CONCOMITANTE ac 
                                                       WHERE ac.CENSO = a.UNIDADE_ENSINO
                                                 )
                                 AND a.ALUNO NOT IN (
                                                      SELECT ALUNO 
                                                        FROM TCE_ALUNO_EDUC_ESPECIAL esp 
                                                       WHERE esp.CENSO = a.UNIDADE_ENSINO
                                                    ) 
                                 AND EXISTS (
                                              SELECT 1
                                                FROM CartaoEstudante.MUNICIPIOBILHETAGEM
                                               WHERE MUNICIPIOID = UE.MUNICIPIO
                                            )   ",
                             maxRows,
                             RNBase.MudarAspas(System.Threading.Thread.CurrentPrincipal.Identity.Name)));

            if (key != null)
            {
                sql.Append("   AND a.aluno = ? ");
                parValues.Add(key.ToString());
            }
            else
            {
                if (this.HasValue(pars, "aluno"))
                {
                    sql.Append("   AND a.aluno LIKE ? ");
                    parValues.Add(this.LikeExpression(pars["aluno"].ToString()));
                }
            }

            if (this.HasValue(pars, "nome"))
            {
                sql.Append("   AND P.nome_compl LIKE ? ");
                parValues.Add(this.LikeExpression(pars["nome"].ToString()));
            }

            if (this.HasValue(pars, "mae"))
            {
                sql.Append("   AND p.nome_mae LIKE ? ");
                parValues.Add(this.LikeExpression(pars["mae"].ToString()));
            }

            if (this.HasValue(pars, "pai"))
            {
                sql.Append("   AND p.nome_pai LIKE ? ");
                parValues.Add(this.LikeExpression(pars["pai"].ToString()));
            }

            if (this.HasValue(pars, "ativo"))
            {
                sql.Append("   AND a.SIT_ALUNO = ? ");
                parValues.Add(pars["ativo"].ToString());
            }

            if (this.HasValue(pars, "numinscricao"))
            {
                sql.Append("   AND a.Numinscricao = ? ");
                parValues.Add(pars["numinscricao"].ToString());
            }

            sql.Append(string.Format(
                            @" UNION ALL
                              SELECT DISTINCT TOP {0}
                                     a.aluno,
                                     a.numinscricao,
                                     p.nome_pai AS pai,
                                     p.nome_mae AS mae,
                                     p.pessoa,
                                     p.nome_compl AS nome,
                                     p.rg_num,
                                     p.cpf,
                                     CONVERT(VARCHAR(10), p.DT_NASC, 103) AS dt_nascimento
                                FROM USUARIO u WITH (NOLOCK),
                                     TCE_ALUNO_CONCOMITANTE ac WITH (NOLOCK), 
                                     LY_ALUNO a WITH (NOLOCK)
                               INNER JOIN LY_PESSOA p WITH (NOLOCK)
                                  ON p.PESSOA = a.PESSOA
                               INNER JOIN LY_UNIDADE_ENSINO UE
                                  ON UE.UNIDADE_ENS = A.UNIDADE_ENSINO
                                 AND UE.ID_REGIONAL <> 5
                               WHERE u.USUARIO = '{1}'
                                 AND (
                                       EXISTS ( 
                                                SELECT TOP 1
                                                       usuuni.UNIDADE_FIS
                                                  FROM LY_USUARIO_UNIDADE_FIS usuuni WITH (NOLOCK) 
                                                 INNER JOIN LY_UNIDADES_ASSOCIADAS usuuna WITH (NOLOCK) 
                                                    ON usuuni.UNIDADE_FIS = usuuna.UNIDADE_FIS 
                                                 WHERE usuuna.UNIDADE_ENS = ac.CENSO
                                                   AND usuuni.USUARIO = u.USUARIO
                                                   AND u.PRIVIL <> 'S' 
                                              )
                                           OR (U.PRIVIL = 'S')
                                     )
                                 AND ac.STATUS = 'Liberado' 
                                 AND ac.DT_CADASTRO = (
                                                        SELECT MAX(acc.DT_CADASTRO) 
                                                          FROM TCE_ALUNO_CONCOMITANTE acc 
                                                         WHERE acc.ALUNO = ac.ALUNO
                                                      ) 
                                 AND a.ALUNO IN (
                                                  SELECT ALUNO 
                                                    FROM TCE_ALUNO_CONCOMITANTE
                                                ) 
                                 AND EXISTS (
                                              SELECT 1
                                                FROM CartaoEstudante.MUNICIPIOBILHETAGEM
                                               WHERE MUNICIPIOID = UE.MUNICIPIO
                                            ) ",
                             maxRows,
                             RNBase.MudarAspas(System.Threading.Thread.CurrentPrincipal.Identity.Name)));

            if (key != null)
            {
                sql.Append("   AND a.aluno = ? ");
                parValues.Add(key.ToString());
            }
            else
            {
                if (this.HasValue(pars, "aluno"))
                {
                    sql.Append("   AND a.aluno LIKE ? ");
                    parValues.Add(this.LikeExpression(pars["aluno"].ToString()));
                }
            }

            if (this.HasValue(pars, "nome"))
            {
                sql.Append("   AND P.nome_compl LIKE ? ");
                parValues.Add(this.LikeExpression(pars["nome"].ToString()));
            }

            if (this.HasValue(pars, "mae"))
            {
                sql.Append("   AND p.nome_mae LIKE ? ");
                parValues.Add(this.LikeExpression(pars["mae"].ToString()));
            }

            if (this.HasValue(pars, "pai"))
            {
                sql.Append("   AND p.nome_pai LIKE ? ");
                parValues.Add(this.LikeExpression(pars["pai"].ToString()));
            }

            if (this.HasValue(pars, "ativo"))
            {
                sql.Append("   AND a.SIT_ALUNO = ? ");
                parValues.Add(pars["ativo"].ToString());
            }

            if (this.HasValue(pars, "numinscricao"))
            {
                sql.Append("   AND a.Numinscricao = ? ");
                parValues.Add(pars["numinscricao"].ToString());
            }

            sql.Append(string.Format(
                            @" UNION ALL
                              SELECT DISTINCT TOP {0}
                                     a.aluno,
                                     a.numinscricao,
                                     p.nome_pai AS pai,
                                     p.nome_mae AS mae,
                                     p.pessoa,
                                     p.nome_compl AS nome,
                                     p.rg_num,
                                     p.cpf,
                                     CONVERT(VARCHAR(10), p.DT_NASC, 103) AS dt_nascimento
                                FROM USUARIO u WITH (NOLOCK),
                                     TCE_ALUNO_EDUC_ESPECIAL esp WITH (NOLOCK), 
                                     LY_ALUNO a WITH (NOLOCK)
                               INNER JOIN LY_PESSOA p WITH (NOLOCK)
                                  ON p.PESSOA = a.PESSOA
                               INNER JOIN LY_UNIDADE_ENSINO UE
                                  ON UE.UNIDADE_ENS = A.UNIDADE_ENSINO
                                 AND UE.ID_REGIONAL <> 5
                               WHERE u.USUARIO = '{1}'
                                 AND (
                                       EXISTS ( 
                                                SELECT TOP 1
                                                       usuuni.UNIDADE_FIS
                                                  FROM LY_USUARIO_UNIDADE_FIS usuuni WITH (NOLOCK) 
                                                 INNER JOIN LY_UNIDADES_ASSOCIADAS usuuna WITH (NOLOCK) 
                                                    ON usuuni.UNIDADE_FIS = usuuna.UNIDADE_FIS 
                                                 WHERE usuuna.UNIDADE_ENS = esp.CENSO
                                                   AND usuuni.USUARIO = u.USUARIO
                                                   AND u.PRIVIL <> 'S' 
                                              )
                                           OR (U.PRIVIL = 'S')
                                     )
                                 AND esp.ACEITE = 1 
                                 AND esp.DT_CADASTRO = (
                                                         SELECT MAX(espp.DT_CADASTRO) 
                                                           FROM TCE_ALUNO_EDUC_ESPECIAL espp 
                                                          WHERE espp.ALUNO = esp.ALUNO) 
                                 AND a.ALUNO IN (
                                                  SELECT ALUNO 
                                                    FROM TCE_ALUNO_EDUC_ESPECIAL
                                                ) 
                                 AND EXISTS (
                                              SELECT 1
                                                FROM CartaoEstudante.MUNICIPIOBILHETAGEM
                                               WHERE MUNICIPIOID = UE.MUNICIPIO
                                            ) ",
                             maxRows,
                             RNBase.MudarAspas(System.Threading.Thread.CurrentPrincipal.Identity.Name)));

            if (key != null)
            {
                sql.Append("   AND a.aluno = ? ");
                parValues.Add(key.ToString());
            }
            else
            {
                if (this.HasValue(pars, "aluno"))
                {
                    sql.Append("   AND a.aluno LIKE ? ");
                    parValues.Add(this.LikeExpression(pars["aluno"].ToString()));
                }
            }

            if (this.HasValue(pars, "nome"))
            {
                sql.Append("   AND P.nome_compl LIKE ? ");
                parValues.Add(this.LikeExpression(pars["nome"].ToString()));
            }

            if (this.HasValue(pars, "mae"))
            {
                sql.Append("   AND p.nome_mae LIKE ? ");
                parValues.Add(this.LikeExpression(pars["mae"].ToString()));
            }

            if (this.HasValue(pars, "pai"))
            {
                sql.Append("   AND p.nome_pai LIKE ? ");
                parValues.Add(this.LikeExpression(pars["pai"].ToString()));
            }

            if (this.HasValue(pars, "ativo"))
            {
                sql.Append("   AND a.SIT_ALUNO = ? ");
                parValues.Add(pars["ativo"].ToString());
            }

            if (this.HasValue(pars, "numinscricao"))
            {
                sql.Append("   AND a.Numinscricao = ? ");
                parValues.Add(pars["numinscricao"].ToString());
            }

            sql.Append(@") tabelaAlunos");

            var qt = new QueryTable(sql.ToString());

            qt.Query(this.CreateConnection(), parValues.ToArray());

            var dv = qt.DefaultView;

            dv.Sort = "nome ASC";

            return dv;
        }
    }
}