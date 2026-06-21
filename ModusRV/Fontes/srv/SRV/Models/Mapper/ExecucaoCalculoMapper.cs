using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SRV.Models.Domain;

namespace SRV.Models.Mapper
{
    public class ExecucaoCalculoMapper : BaseMapper<ExecucaoCalculo>
    {
        protected override string QueryFindObject()
        {
            throw new NotImplementedException();
        }

        protected override string QueryListObjects()
        {
            throw new NotImplementedException();
        }

        private string QueryFindUltimaExecucao()
        {
            return @"SELECT id_execucao_calculo, dt_execucao, status_execucao,
                            u.id_usuario, u.des_nome_usuario, c.MENSAGEM
                       FROM rv_execucao_calculo c, rv_usuario u
                      WHERE u.id_usuario = c.id_usuario 
                        AND c.id_execucao_calculo = (SELECT MAX(id_execucao_calculo) FROM rv_execucao_calculo)";
        }

        private string QueryInsert()
        {
            return @"INSERT INTO rv_execucao_calculo
                                (dt_execucao,
                                 status_execucao, 
                                 id_usuario)
                          VALUES(GETDATE(),
                                 @idStatusEmExecucao,
                                 @idUsuario)";
        }

        public override ExecucaoCalculo LoadObject(System.Data.SqlClient.SqlDataReader reader)
        {
            ExecucaoCalculo execucaoCalculo = new ExecucaoCalculo();

            execucaoCalculo.IdExecucaoCalculo = Convert.ToInt32(reader["id_execucao_calculo"]);
            execucaoCalculo.DtExecucao = (DateTime)reader["dt_execucao"];
            execucaoCalculo.StatusExecucao = (StatusExecucao)Enum.ToObject(typeof(StatusExecucao), Convert.ToInt32(reader["status_execucao"]));
            execucaoCalculo.MENSAGEM = reader["MENSAGEM"] != null ? reader["MENSAGEM"].ToString() : String.Empty;

            Usuario usuario = new Usuario();
            usuario.Id = Convert.ToInt32(reader["id_usuario"]);
            usuario.Nome = (string)reader["des_nome_usuario"];

            execucaoCalculo.Usuario = usuario;

            return execucaoCalculo;
        }

        public ExecucaoCalculo FindUltimaExecucao()
        {
            return FindObject(QueryFindUltimaExecucao(), null);
        }

        public ExecucaoCalculo Insert(ExecucaoCalculo execucaoCalculo)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("idStatusEmExecucao", (int)StatusExecucao.EmExecucao);
            param.Add("idUsuario", execucaoCalculo.Usuario.Id);

            int? idExecucao = InsertObjectWithIdentity(QueryInsert(), param);

            execucaoCalculo.IdExecucaoCalculo = idExecucao.Value;

            return execucaoCalculo;
        }

    }
}