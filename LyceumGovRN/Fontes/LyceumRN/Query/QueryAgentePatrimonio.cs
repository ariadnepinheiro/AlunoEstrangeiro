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

namespace Techne.Lyceum.RN.Query
{
    public class QueryAgentePatrimonio : LyceumQuery
    {
        public QueryAgentePatrimonio()
            : base()
        {
            //Colunas da grid da TSearch
            this.GridColumns.Add(new TSearchColumn() { FieldName = "matricula", Caption = "Matrícula", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "pessoa", Caption = "Código", Width = Unit.Percentage(12) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "nome", Caption = "Nome", Width = Unit.Percentage(28) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "rg_num", Caption = "Documento", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "cpf", Caption = "CPF", Width = Unit.Percentage(10) });
              this.GridColumns.Add(new TSearchColumn() { FieldName = "funcaodesc", Caption = "Função", Width = Unit.Percentage(0), Visible = false });

            this.Messages.KeyNotFound = "Matrícula inválida"; //Mensagem para chave não encontrada
            this.TextField = "matricula"; //chave
            this.DescriptionField = "nome"; //descrição

            //Filtros disponíveis na TSearch
            this.GridFilterParameters.Add("matricula", "Matrícula", TSearchDataType.String, 19);
            this.GridFilterParameters.Add("pessoa", "Código", TSearchDataType.String, 19);
            this.GridFilterParameters.Add("nome", "Nome", TSearchDataType.String, 100);
            this.GridFilterParameters.Add("rg_num", "Documento", TSearchDataType.String, 25);
            this.GridFilterParameters.Add("cpf", "CPF", TSearchDataType.String, 19);

            this.MaxRows = 100; //máximo de linhas
            this.GridWidth = Unit.Pixel(800); //largura da grid
            this.TextFieldType = TSearchDataType.String; //tipo da chave
            this.MaxLength = 10; //tamanho máximo da chave
        }


        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            DataTable dt = null;
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            DataView dv = new DataView();
            var contextQuery = new ContextQuery();
            var contextQueryPar = new ContextQuery();
            RN.Usuarios rnUsuarios = new Usuarios();
            RN.Perfil rnPerfil = new Perfil();

            try
            {
                if (key != null)
                {
                    contextQueryPar.Command += " AND SR.matricula = @MATRICULA ";
                    contextQuery.Parameters.Add("@MATRICULA ", key.ToString());
                }
                else if (pars.ContainsKey("matricula") && pars["matricula"] != null && pars["matricula"].ToString().Trim().Length > 0)
                {
                    contextQueryPar.Command += " AND convert(varchar, SR.matricula ) like @matricula ";
                    contextQuery.Parameters.Add("@matricula ", this.LikeExpression(pars["matricula"].ToString()));
                }

                if (pars.ContainsKey("pessoa") && pars["pessoa"] != null && pars["pessoa"].ToString().Trim().Length > 0)
                {
                    contextQueryPar.Command += " AND convert(varchar, p.pessoa) like @pessoa ";
                    contextQuery.Parameters.Add("@pessoa ", this.LikeExpression(pars["pessoa"].ToString()));
                }

                if (pars.ContainsKey("nome") && pars["nome"] != null && pars["nome"].ToString().Trim().Length > 0)
                {
                    contextQueryPar.Command += " AND convert(varchar,  p.nome_compl) like @nome ";
                    contextQuery.Parameters.Add("@nome ", this.LikeExpression(pars["nome"].ToString()));
                }
                if (pars.ContainsKey("rg_num") && pars["rg_num"] != null && pars["rg_num"].ToString().Trim().Length > 0)
                {
                    contextQueryPar.Command += " AND convert(varchar,  p.rg_num) like @rg_num ";
                    contextQuery.Parameters.Add("@rg_num ", this.LikeExpression(pars["rg_num"].ToString().RetirarMascaraRG()));
                }
                if (pars.ContainsKey("cpf") && pars["cpf"] != null && pars["cpf"].ToString().Trim().Length > 0)
                {
                    contextQueryPar.Command += " AND convert(varchar,  p.cpf) like @cpf ";
                    contextQuery.Parameters.Add("@cpf ", this.LikeExpression(pars["cpf"].ToString().RetirarMascaraCPF()));
                }

                contextQuery.Command += @" SELECT DISTINCT TOP ( @maxRows ) SR.MATRICULA, 
                                                            P.PESSOA, 
                                                            P.NOME_COMPL AS NOME, 
                                                            P.RG_NUM, 
                                                            P.CPF, 
                                                            P.FONE, 
                                                            P.CELULAR, 
                                                            P.E_MAIL_INTERNO, 
                                                            P.NOME_MAE AS MAE, 
                                                            P.NOME_PAI AS PAI ,
                                                            F.DESCRICAO AS FUNCAODESC
                                            FROM   LY_PESSOA P
                                                   JOIN (SELECT PESSOA,
                                                                MATRICULA,
                                                                DATA_DESATIVACAO
                                                         FROM   LY_VINCULO VV WITH ( NOLOCK )
                                                         UNION
                                                         SELECT PESSOA,
                                                                MATRICULA,
                                                                DT_DEMISSAO
                                                         FROM   LY_DOCENTE WITH ( NOLOCK )
                                                         WHERE  ISNULL(VOLUNTARIO, 'N') = 'N'
                                                                AND MATRICULA NOT IN ( '00002121', '00000000', '11111111',
                                                                                       '22222222',
                                                                                       '44444444', '55555551', '55555555',
                                                                                       '66666666',
                                                                                       '77777777', '88888888', '99999999',
                                                                                       '88888880',
                                                                                       '88888881' )
                                                                AND REGIMECONTRATACAOID IN ( 1, 2 )) SR --LISTA TODAS AS MATRICULAS DE SERVIDORES CONCURSADO
                                                     ON P.PESSOA = SR.PESSOA
                                                   LEFT JOIN LY_LOTACAO L (NOLOCK)
                                                          ON SR.MATRICULA = L.MATRICULA
                                                             AND ( L.DATA_DESATIVACAO IS NULL
                                                                    OR CONVERT(DATE, L.DATA_DESATIVACAO) >
                                                                       CONVERT(DATE, Getdate())
                                                                 )
                                                   LEFT JOIN LY_FUNCAO F
                                                          ON F.FUNCAO = L.FUNCAO
                                                   WHERE SR.MATRICULA IS NOT NULL 
                                            ";

                if (contextQueryPar.Command != null)
                {
                    contextQuery.Command += contextQueryPar.Command;
                }

                contextQuery.Parameters.Add("@maxRows", maxRows);

                dt = ctx.GetDataTable(contextQuery);

                dv = dt.DefaultView;

                dv.Sort = "nome ASC";
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




