using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Techne.Data;
using Techne.Lyceum.RN.Util;
using Techne.Web;

namespace Techne.Lyceum.RN.Query
{
    public class QueryFuncionario : LyceumQuery
    {
        public QueryFuncionario() : base()
        {
            //Colunas da grid da TSearch
            this.GridColumns.Add(new TSearchColumn() { FieldName = "idvinculo_matricula", Caption = "IdVinculo / Matricula", Width = Unit.Percentage(10), Visible = false });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "idvinculo", Caption = "Id/Vinculo", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "matricula", Caption = "Matrícula ou ID/Vínculo", Width = Unit.Percentage(20) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "pessoa", Caption = "Código", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "nome", Caption = "Nome", Width = Unit.Percentage(50) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "cpf", Caption = "CPF", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "rg_num", Caption = "Documento", Width = Unit.Percentage(10) });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "fone", Caption = "fone", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "celular", Caption = "cel", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "e_mail_interno", Caption = "email_interno", Width = Unit.Percentage(0), Visible = false });
            this.GridColumns.Add(new TSearchColumn() { FieldName = "vinculo", Caption = "vinculo", Width = Unit.Percentage(0), Visible = false });

            this.Messages.KeyNotFound = "Funcionário inválido"; //Mensagem para chave não encontrada
            this.TextField = "idvinculo_matricula"; //chave
            this.DescriptionField = "nome"; //descrição

            //Filtros disponíveis na TSearch
            this.GridFilterParameters.Add("idvinculo", "Id/Vinculo", TSearchDataType.String, 19);
            this.GridFilterParameters.Add("idfuncional", "ID Funcional", TSearchDataType.String, 19);
            this.GridFilterParameters.Add("matricula", "Matrícula", TSearchDataType.String, 19);
            this.GridFilterParameters.Add("pessoa", "Código", TSearchDataType.String, 19);
            this.GridFilterParameters.Add("nome", "Nome", TSearchDataType.String, 100);            
            this.GridFilterParameters.Add("cpf", "CPF", TSearchDataType.String, 19);
            this.GridFilterParameters.Add("rg_num", "Documento", TSearchDataType.String, 25);
            
            this.MaxRows = 100; //máximo de linhas
            this.GridWidth = Unit.Pixel(800); //largura da grid
            this.TextFieldType = TSearchDataType.String; //tipo da chave
            this.MaxLength = 100; //tamanho máximo da chave
        }

        public override DataView GetData(IDictionary<string, object> pars, object key, int maxRows)
        {
            ArrayList parValues = new ArrayList();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT distinct top " + maxRows.ToString() + @" p.matricula,
                                p.pessoa,
                                p.nome_compl AS nome,
                                p.rg_num,
                                p.cpf,
                                p.fone,
                                p.celular,
                                p.e_mail_interno,
                                p.nome_mae   AS mae,
                                p.nome_pai   AS pai,
                                p.idfuncional,
						        p.vinculo,
						        p.idvinculo,
						        p.idvinculo_matricula
                        FROM   vw_funcionarios p
                        WHERE  1 = 1  ");

            if (key != null)
            {
                sql.Append(" and p.idvinculo_matricula = ? ");
                parValues.Add(key.ToString());
            }
            else
            {
                if (pars.ContainsKey("idvinculo_matricula") && pars["idvinculo_matricula"] != null && pars["idvinculo_matricula"].ToString().Trim().Length > 0)
                {
                    sql.Append(" and p.idvinculo_matricula like ? ");
                    parValues.Add("%" + LikeExpression(pars["matricula"].ToString()));
                }
            }

            if (pars.ContainsKey("matricula") && pars["matricula"] != null && pars["matricula"].ToString().Trim().Length > 0)
            {
                sql.Append(" and p.matricula like ? ");
                parValues.Add("%" + LikeExpression(pars["matricula"].ToString()));
            }

            if (pars.ContainsKey("pessoa") && pars["pessoa"] != null && pars["pessoa"].ToString().Trim().Length > 0)
            {
                sql.Append(" and convert(varchar,p.pessoa) like ? ");
                parValues.Add(LikeExpression(pars["pessoa"].ToString()));
            }

            if (pars.ContainsKey("nome") && pars["nome"] != null && pars["nome"].ToString().Trim().Length > 0)
            {
                sql.Append(" and p.nome_compl like ? ");
                parValues.Add("%" + LikeExpression(pars["nome"].ToString()));
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

            if (pars.ContainsKey("idfuncional") && pars["idfuncional"] != null && pars["idfuncional"].ToString().Trim().Length > 0)
            {
                sql.Append(" and convert(varchar,p.idfuncional) like ? ");
                parValues.Add(LikeExpression(pars["idfuncional"].ToString()));
            }

            if (pars.ContainsKey("idvinculo") && pars["idvinculo"] != null && pars["idvinculo"].ToString().Trim().Length > 0)
            {
                sql.Append(" and p.idvinculo like ? ");
                parValues.Add(LikeExpression(pars["idvinculo"].ToString()));
            }

            QueryTable qt = new QueryTable(sql.ToString());
            qt.Query(this.CreateConnection(), parValues.ToArray());
            DataView dv = qt.DefaultView;
            dv.Sort = "nome asc";
            return dv;
        }

    }
}




