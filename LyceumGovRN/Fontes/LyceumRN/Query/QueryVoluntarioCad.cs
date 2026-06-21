using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Techne.Data;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using System.Web;


namespace Techne.Lyceum.RN.Query
{
    public class QueryVoluntarioCad : LyceumQuery
    {
        public QueryVoluntarioCad()
        {
			this.GridColumns.Add(new TSearchColumn() { FieldName = "matricula", Caption = "Matrícula", Width = Unit.Percentage(10) });
			this.GridColumns.Add(new TSearchColumn() { FieldName = "nome", Caption = "Nome", Width = Unit.Percentage(24) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "rg_num", Caption = "Documento", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "cpf", Caption = "CPF", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "mae", Caption = "Nome da Mãe", Width = Unit.Percentage(18) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "pai", Caption = "Nome do Pai", Width = Unit.Percentage(18) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "num_func", Caption = "Nº Docente", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "pessoa", Caption = "Pessoa", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "categoria", Caption = "Funcao", Width = Unit.Percentage(0), Visible = false });

            this.Messages.KeyNotFound = "Matrícula inválida ou usuário sem permissão de visualização do Docente(Lotação).";
            this.TextField = "matricula";
            this.DescriptionField = "nome";
            this.GridFilterParameters.Add("nome", "Nome", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("matricula", "Matrícula", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("rg_num", "Documento", TSearchDataType.String, 25);
            this.GridFilterParameters.Add("cpf", "CPF", TSearchDataType.String, 19);
            this.GridFilterParameters.Add("num_func", "Nº Docente", TSearchDataType.String, 15);
            this.MaxRows = 100;
            this.MaxLength = 100;
            this.GridWidth = Unit.Pixel(800);
        }

        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            ArrayList parValues = new ArrayList();
            StringBuilder sql = new StringBuilder();
            sql.Append(string.Format

                    (@"SELECT distinct top {0} d.matricula, d.num_func, p.nome_compl as nome,  p.rg_num, p.cpf, p.pessoa, 
                                    p.nome_mae as mae, p.nome_pai as pai, d.categoria 
                    FROM   USUARIO u WITH (NOLOCK),
                    ly_pessoa p (nolock) inner join ly_docente d 
                    on p.pessoa = d.pessoa 
                    inner join TCE_MATRICULA_VOLUNTARIO mv (NOLOCK) 
                                on d.matricula = mv.matricula
                    inner JOIN ly_lotacao lo ( NOLOCK ) ON d.matricula = lo.matricula
                    where 1=1 
                    and voluntario='S'                    
                                and  u.USUARIO = '{1}'
                               ",
                             maxRows,
                             RNBase.MudarAspas(System.Threading.Thread.CurrentPrincipal.Identity.Name)));

            if (!RN.PadroesDeAcessos.VerificaPrivilegio(System.Threading.Thread.CurrentPrincipal.Identity.Name))
            { 
                sql.Append(@" AND (EXISTS ( SELECT TOP 1
                                                      UNIDADE_FIS
                                            FROM      LY_USUARIO_UNIDADE_FIS usuuni WITH (NOLOCK)
                                            WHERE     usuuni.UNIDADE_FIS = lo.UNIDADE_ENS
                                                      AND usuuni.USUARIO = u.USUARIO
                                                      AND u.PRIVIL <> 'S' )
	                                                  OR (U.PRIVIL = 'S')) ") ;
            }



            if (key != null)
            {
                sql.Append(" and d.matricula= ? ");
                parValues.Add(key);
            }
            else if (pars.ContainsKey("matricula") && pars["matricula"] != null && pars["matricula"].ToString().Trim().Length > 0)
            {
                sql.Append(" and convert(varchar,d.matricula) like ? ");
                parValues.Add(LikeExpression(pars["matricula"].ToString()));
            }

            if (pars.ContainsKey("nome") && pars["nome"] != null && pars["nome"].ToString().Trim().Length > 0)
            {
                sql.Append(" and p.nome_compl like ? ");
                parValues.Add(LikeExpression(pars["nome"].ToString()));
            }

            if (pars.ContainsKey("rg_num") && pars["rg_num"] != null && pars["rg_num"].ToString().Trim().Length > 0)
            {
                sql.Append(" and p.rg_num like ? ");
                string rg = LikeExpression(pars["rg_num"].ToString().RetirarMascaraRG());
                parValues.Add(rg);
            }

            if (pars.ContainsKey("cpf") && pars["cpf"] != null && pars["cpf"].ToString().Trim().Length > 0)
            {
                sql.Append(" and p.cpf like ? ");
                string cpf = LikeExpression(pars["cpf"].ToString().RetirarMascaraCPF());
                parValues.Add(cpf);
            }
            if (pars.ContainsKey("num_func") && pars["num_func"] != null && pars["num_func"].ToString().Trim().Length > 0)
            {
                sql.Append(" and convert(varchar,d.num_func) like ? ");
                parValues.Add(LikeExpression(pars["num_func"].ToString()));
            }
            
            //Inicio: Union inserido para tambem listar docentes sem lotação
            sql.Append(@" UNION
                        SELECT  D.MATRICULA ,
                                D.NUM_FUNC ,
                                P.NOME_COMPL AS NOME ,
                                P.RG_NUM ,
                                P.CPF ,
                                P.PESSOA ,
                                P.NOME_MAE AS MAE ,
                                P.NOME_PAI AS PAI,
                                D.CATEGORIA
                        FROM    DBO.LY_DOCENTE D
                                INNER JOIN DBO.LY_PESSOA P ON D.PESSOA = P.PESSOA
                                INNER JOIN TCE_MATRICULA_VOLUNTARIO MV ( NOLOCK ) ON D.MATRICULA = MV.MATRICULA
                        WHERE   VOLUNTARIO = 'S'
                                AND NOT EXISTS ( SELECT TOP 1
                                                        1
                                                 FROM   LY_LOTACAO LO ( NOLOCK )
                                                 WHERE  D.MATRICULA = LO.MATRICULA 
														AND (DATA_DESATIVACAO IS NULL
																OR CONVERT(DATE, DATA_DESATIVACAO) > CONVERT(DATE, GETDATE()))
														AND CONVERT(DATE,DATA_NOMEACAO) <= CONVERT(DATE,GETDATE())) ");

            if (key != null)
            {
                sql.Append(" and d.matricula= ? ");
                parValues.Add(key);
            }
            else if (pars.ContainsKey("matricula") && pars["matricula"] != null && pars["matricula"].ToString().Trim().Length > 0)
            {
                sql.Append(" and convert(varchar,d.matricula) like ? ");
                parValues.Add(LikeExpression(pars["matricula"].ToString()));
            }

            if (pars.ContainsKey("nome") && pars["nome"] != null && pars["nome"].ToString().Trim().Length > 0)
            {
                sql.Append(" and p.nome_compl like ? ");
                parValues.Add(LikeExpression(pars["nome"].ToString()));
            }

            if (pars.ContainsKey("rg_num") && pars["rg_num"] != null && pars["rg_num"].ToString().Trim().Length > 0)
            {
                sql.Append(" and p.rg_num like ? ");
                string rg = LikeExpression(pars["rg_num"].ToString().RetirarMascaraRG());
                parValues.Add(rg);
            }

            if (pars.ContainsKey("cpf") && pars["cpf"] != null && pars["cpf"].ToString().Trim().Length > 0)
            {
                sql.Append(" and p.cpf like ? ");
                string cpf = LikeExpression(pars["cpf"].ToString().RetirarMascaraCPF());
                parValues.Add(cpf);
            }
            if (pars.ContainsKey("num_func") && pars["num_func"] != null && pars["num_func"].ToString().Trim().Length > 0)
            {
                sql.Append(" and convert(varchar,d.num_func) like ? ");
                parValues.Add(LikeExpression(pars["num_func"].ToString()));
            }

            //Fim: Union inserido para tambem listar docentes sem lotação

            QueryTable qt = new QueryTable(sql.ToString());
            qt.Query(this.CreateConnection(), parValues.ToArray());
            DataView dv = qt.DefaultView;
            dv.Sort = "nome asc";
            return dv;

        }

    }
}
