using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using System;
using Techne.Lyceum.RN.Util;
using System.Collections.Generic;
using Techne.Data;
using System.Linq;

namespace Techne.Lyceum.RN.NecessidadeEspecial
{
    public class RecursoAplicacaoProva
    {
        public static DataTable Listar()
        {
            try
            {
                using (var contexto = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @" SELECT R.[RECURSOAPLICACAOPROVAID] RECURSOAPLICACAOPROVAID,
                                            R.[NOME] NOME,
	                                        R.[EXCLUSIVO] EXCLUSIVO,
                                            R.ATIVO
                                       FROM [NecessidadeEspecial].[RECURSOAPLICACAOPROVA] R "
                    };

                    return contexto.GetDataTable(contextQuery);
                }
            }
            catch (Exception exception)
            {
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine, Convert.ToString(exception.Message));
                throw new Exception(mensagem);
            }
        }
    }
}
