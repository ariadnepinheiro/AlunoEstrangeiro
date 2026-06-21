using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.NecessidadeEspecial
{
    public class TipoRecursoNecessidadeEspecial : RNBase
    {
        public enum TipoRecurso
        {
            [StringValue("Cuidador")]
            Cuidador = 1,
            [StringValue("Ledor")]
            Ledor = 2,
            [StringValue("Intérprete de Libras")]
            Interprete = 3,
            [StringValue("Sala de Recursos")]
            SalaRecurso = 4,
            [StringValue("Professor Articulador Pedagógico Educação Especial")]
            PAPEE = 5
        }

        public DataTable ListaTipoRecursoNecessidadeEspecialAtivo()
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT * 
                                    FROM   NecessidadeEspecial.TIPORECURSONECESSIDADEESPECIAL (NOLOCK)
                                    WHERE  ATIVO = 1  ";

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Empty;
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
                ctx.Dispose();
            }

            return dt;
        }

        public TipoRecurso RetornaTipoRecursoPor(int tipoRecursoId)
        {
            switch (tipoRecursoId)
            {
                case (int)TipoRecurso.Cuidador:
                    return TipoRecurso.Cuidador;
                case (int)TipoRecurso.Ledor:
                    return TipoRecurso.Ledor;
                case (int)TipoRecurso.Interprete:
                    return TipoRecurso.Interprete;
                case (int)TipoRecurso.SalaRecurso:
                    return TipoRecurso.SalaRecurso;
                case (int)TipoRecurso.PAPEE:
                    return TipoRecurso.PAPEE;
                default:
                    return new TipoRecurso();
            }
        }
    }
}
