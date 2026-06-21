using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN
{
    public class EventoGeral
    {
        public bool ExisteBloqueioCadastroAlunoAtivoPor(string curso)
        {
            Seeduc.Infra.Data.DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            bool retorno = false;

            try
            {
                retorno = this.ExisteBloqueioCadastroAlunoAtivoPor(contexto, curso);
                return retorno;
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

        public bool ExisteBloqueioCadastroAlunoAtivoPor(Seeduc.Infra.Data.DataContext contexto, string curso)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT count(*) 
                                    FROM LY_EVENTO_GERAL (nolock)
                                    WHERE CURSO = @CURSO
                                          AND TIPO_FILTRO = 'Bloqueio_Cadastro_Aluno'
                                          AND CONVERT(DATE,GETDATE()) BETWEEN DT_INICIO AND DT_FIM ";

            contextQuery.Parameters.Add("@CURSO", TechneDbType.T_CODIGO, curso);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }
    }
}
