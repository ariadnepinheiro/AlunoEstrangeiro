
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Techne.Data;
using Techne.Lyceum.RN.Util;
using Techne.Web;
using Seeduc.Infra.Data;
using System;
using System.Web;

namespace Techne.Lyceum.RN.Query
{
    public class QueryDocenteQHI : LyceumQuery
    {
        public QueryDocenteQHI()
        {

            this.GridColumns.Add(new TSearchColumn() { FieldName = "matricula", Caption = "Matrícula", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "nome_compl", Caption = "Nome", Width = Unit.Percentage(90) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "num_func", Caption = "Nº Docente", Visible = false, Width = Unit.Percentage(0) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "idvinculo_matricula", Caption = "ID/Vínculo", Width = Unit.Percentage(10) });

            this.Messages.KeyNotFound = "Matrícula ou ID/Vínculo sem habilitação cadastrada.";
            this.TextField = "idvinculo_matricula";
            this.DescriptionField = "nome_compl";
            this.GridFilterParameters.Add("nome_compl", "Nome", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("matricula", "Matrícula", TSearchDataType.String, 20);
            this.GridFilterParameters.Add("cpf", "CPF", TSearchDataType.String, 19);
            this.GridFilterParameters.Add("idvinculo_matricula", "ID/Vínculo", TSearchDataType.String, 15);

            this.MaxRows = 100;
            this.MaxLength = 100;
            this.GridWidth = Unit.Pixel(800);
        }


        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            DataTable dt = null;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            DataView dv = new DataView();
            var contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DISTINCT TOP ( @maxRows ) V.num_func,
		                        V.matricula,
		                        V.nome_compl,
		                        GHDI.disciplina,
		                        rg_num,
		                        cpf,
		                        idfuncional,
		                        vinculo,
		                        IDVINCULO,
		                        IDVINCULO AS idvinculo_matricula ,
                                VOLUNTARIO
                        INTO #PROFESSORES
                        FROM  VW_DOCENTE_ATIVO V
	                        INNER JOIN LY_UNIDADE_ENSINO UE ON V.SETOR = UE.SETOR
	                        INNER JOIN LY_GRUPO_HABILITACAO_DOC GHDO ON GHDO.NUM_FUNC = V.NUM_FUNC
                            INNER JOIN LY_GRUPO_HABILITACAO_DISC GHDI ON GHDI.AGRUPAMENTO = GHDO.AGRUPAMENTO
	                        LEFT JOIN LY_CANDIDATO_DOC_CONTRATO CDC (NOLOCK)
                                              ON CDC.CANDIDATO = V.CANDIDATO
                                                 AND CDC.CONCURSO = V.CONCURSO
                        WHERE NOT EXISTS (SELECT TOP 1 1
                                                       FROM   LY_LICENCA_DOCENTE (NOLOCK)
                                                       WHERE  LY_LICENCA_DOCENTE.NUM_FUNC = V.NUM_FUNC
                                                              AND DTFIM IS NULL
                                                              AND MOTIVO <> '43'
                                                              AND DTFIM >= CONVERT(DATE, GETDATE()))

                        ";

                if (pars.ContainsKey("disciplina") && pars["disciplina"] != null && pars["disciplina"].ToString().Trim().Length > 0)
                {
                    contextQuery.Command += @"AND (disciplina = '' or disciplina = @disciplina)
                            ";

                    string[] disciplinas = pars["disciplina"].ToString().Split('|');
                    if (disciplinas.Length > 1)
                    {
                        contextQuery.Parameters.Add("@disciplina", disciplinas[1]);
                    }
                    else
                    {
                        contextQuery.Parameters.Add("@disciplina", disciplinas[0]);
                    }
                }

                if (key != null)
                {
                    contextQuery.Command += @"AND IDVINCULO = @IDVINCULO 
                            ";
                }
                else
                {
                    if (pars.ContainsKey("idvinculo_matricula") && pars["idvinculo_matricula"] != null && pars["idvinculo_matricula"].ToString().Trim().Length > 0)
                    {
                        contextQuery.Command += @"AND IDVINCULO  like @IDVINCULO 
                                                    ";
                    }

                    if (pars.ContainsKey("nome_compl") && pars["nome_compl"] != null && pars["nome_compl"].ToString().Trim().Length > 0)
                    {
                        contextQuery.Command += @"AND v.nome_compl  like @NOME_COMPL 
                                                    ";
                    }

                    if (pars.ContainsKey("rg_num") && pars["rg_num"] != null && pars["rg_num"].ToString().Trim().Length > 0)
                    {
                        contextQuery.Command += @"AND RG_NUM  LIKE @RG_NUM 
                                                    ";
                        contextQuery.Parameters.Add("@rg_num", this.LikeExpression(pars["rg_num"].ToString()));
                    }

                    if (pars.ContainsKey("cpf") && pars["cpf"] != null && pars["cpf"].ToString().Trim().Length > 0)
                    {
                        contextQuery.Command += @"AND cpf LIKE @cpf 
                                                    ";
                        contextQuery.Parameters.Add("@cpf", this.LikeExpression(pars["cpf"].ToString().RetirarMascaraCPF()));
                    }                    
                    if ((pars.ContainsKey("dtInicio") && pars["dtInicio"] != null && pars["dtInicio"].ToString().Trim().Length > 0) &&
                        (pars.ContainsKey("dtFim") && pars["dtFim"] != null && pars["dtFim"].ToString().Trim().Length > 0))
                    {
                        contextQuery.Command += @"AND ( ( DT_INICIO_CONTRATO IS NULL AND DT_FIM_CONTRATO IS NULL )
                                    OR ( DT_INICIO_CONTRATO <= @DATAFIMCONTRATO AND DT_FIM_CONTRATO >= @DATAINICIOCONTRATO )
                                    OR ( ( DT_INICIO_CONTRATO <= @DATAFIMCONTRATO ) AND ( DT_INICIO_CONTRATO <= GETDATE() ) AND ( DT_FIM_CONTRATO IS NULL ) ) ) 
                                                    ";
                        contextQuery.Parameters.Add("@DATAINICIOCONTRATO", pars["dtInicio"]);
                        contextQuery.Parameters.Add("@DATAFIMCONTRATO", pars["dtFim"]);
                    }
                    if (pars.ContainsKey("voluntario") && pars["voluntario"] != null && pars["voluntario"].ToString().Trim().Length > 0)
                    {
                        contextQuery.Command += @"AND voluntario = @voluntario 
                                                    ";
                        contextQuery.Parameters.Add("@voluntario", pars["voluntario"].ToString());
                    }
                    

                    if (pars.ContainsKey("matricula") && pars["matricula"] != null && pars["matricula"].ToString().Trim().Length > 0)
                    {
                        contextQuery.Command += @" and v.matricula like @matricula
                                                    ";
                    }
                }

                contextQuery.Command += @" INSERT INTO #PROFESSORES
	                                    (num_func,
		                                    matricula,
		                                    nome_compl,
		                                    disciplina,
		                                    rg_num,
		                                    cpf,
		                                    idfuncional,
		                                    vinculo,
		                                    IDVINCULO,
		                                    idvinculo_matricula ,
                                            VOLUNTARIO)
                                    SELECT NUM_FUNC,
                                                   MATRICULA,
                                                   PE.NOME_COMPL,
                                                   '',
                                                   '',
                                                   '',
                                                   PE.IDFUNCIONAL,
                                                   D.VINCULO,
                                                   MATRICULA,
                                                   MATRICULA,
                                                   'N'
                                            FROM   LY_DOCENTE D (NOLOCK)
                                                   INNER JOIN LY_PESSOA PE
                                                           ON PE.PESSOA = D.PESSOA 
                                    ";

                if (RN.PadraoAcessoTurmas.ConsultarPermissaoAlocacaoDocSemAula(System.Web.HttpContext.Current.User.Identity.Name.ToString()) || RN.Usuarios.UsuarioPrivilegiado(HttpContext.Current.User.Identity.Name))
                {
                    contextQuery.Command += @" WHERE  matricula IN ( '00000000', '11111111', '22222222', '44444444',
                                                                  '55555551', '55555555', '66666666', '77777777',
                                                                  '88888888', '99999999', '88888880', '88888881' )
                                    ";
                }
                else
                {
                    contextQuery.Command += @" WHERE  matricula IN ( '00000000', '11111111', '22222222', '44444444', '55555551', '88888888', '99999999' )
                                    ";
                }

                if (key != null)
                {
                    contextQuery.Command += "AND MATRICULA = @IDVINCULO ";
                }
                else
                {
                    if (pars.ContainsKey("idvinculo_matricula") && pars["idvinculo_matricula"] != null && pars["idvinculo_matricula"].ToString().Trim().Length > 0)
                    {
                        contextQuery.Command += @"AND MATRICULA like @IDVINCULO 
                                                    ";
                    }

                    if (pars.ContainsKey("matricula") && pars["matricula"] != null && pars["matricula"].ToString().Trim().Length > 0)
                    {
                        contextQuery.Command += @" and matricula like @matricula
                                                    ";
                    }

                    if (pars.ContainsKey("nome_compl") && pars["nome_compl"] != null && pars["nome_compl"].ToString().Trim().Length > 0)
                    {
                        contextQuery.Command += @"AND PE.NOME_COMPL  like @NOME_COMPL 
                                                    ";
                    }
                }

                //Adicionar parametros que podem ser repetidos
                if (key != null)
                {
                    contextQuery.Parameters.Add("@IDVINCULO", key.ToString());
                }
                else
                {
                    if (pars.ContainsKey("idvinculo_matricula") && pars["idvinculo_matricula"] != null && pars["idvinculo_matricula"].ToString().Trim().Length > 0)
                    {
                        contextQuery.Parameters.Add("@IDVINCULO", this.LikeExpression(pars["idvinculo_matricula"].ToString()));
                    }

                    if (pars.ContainsKey("matricula") && pars["matricula"] != null && pars["matricula"].ToString().Trim().Length > 0)
                    {
                        contextQuery.Parameters.Add("@matricula", this.LikeExpression(pars["matricula"].ToString()));
                    }

                    if (pars.ContainsKey("nome_compl") && pars["nome_compl"] != null && pars["nome_compl"].ToString().Trim().Length > 0)
                    {
                        contextQuery.Parameters.Add("@NOME_COMPL", this.LikeExpression(pars["nome_compl"].ToString()));
                    }
                }

                contextQuery.Command += @" SELECT *
                                        FROM #PROFESSORES F
                                        ORDER BY NOME_COMPL ASC ";
               

                contextQuery.Parameters.Add("@maxRows", maxRows);

                dt = ctx.GetDataTable(contextQuery);

                dv = dt.DefaultView;
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }

            return dt.DefaultView;
        }
    }
}






