using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Data;
using System.Data;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN.LancamentoNotas
{
    public class TurmaMaterialEstudo
    {
        public bool PossuiMaterialEstudoPor(DataContext contexto, int materialEstudoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    from [LancamentoNotas].[TURMA_MATERIALESTUDO] (NOLOCK)
                                    WHERE MATERIALESTUDOID = @MATERIALESTUDOID ";

            contextQuery.Parameters.Add("@MATERIALESTUDOID", SqlDbType.Int, materialEstudoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        private List<int> ListaTurmaMaterialEstudoPor(DataContext ctx, int ano, int semestre, string turma, string disciplina, int subPeriodo)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            List<int> retorno = new List<int>();
            try
            {
                contextQuery.Command = @" SELECT MATERIALESTUDOID 
                                        FROM   LANCAMENTONOTAS.TURMA_MATERIALESTUDO (NOLOCK) 
                                        WHERE  ANO = @ANO AND
                                               SEMESTRE = @SEMESTRE AND
                                               TURMA = @TURMA AND
                                               DISCIPLINA = @DISCIPLINA AND
                                               SUBPERIODO = @SUBPERIODO  ";

                contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
                contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, semestre);
                contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
                contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, disciplina);
                contextQuery.Parameters.Add("@SUBPERIODO", SqlDbType.Int, subPeriodo);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno.Add(Convert.ToInt32(reader["MATERIALESTUDOID"]));
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return retorno;
        }

        public List<int> ListaTurmaMaterialEstudoPor(int ano, int periodo, string turma, string disciplina, int subPeriodo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            List<int> lista = new List<int>();
            try
            {
                lista = this.ListaTurmaMaterialEstudoPor(contexto, ano, periodo, turma, disciplina, subPeriodo);

                return lista;
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

        public void InsereLista(DataContext contexto, List<int> materialEstudoId, int ano, int periodo, int bimestre, string turma, string disciplina, string usuarioId)
        {
            //Deleta os anteriores
            this.RemoveLista(contexto, ano, periodo, bimestre, turma, disciplina);

            foreach (var id in materialEstudoId)
            {
                //Monta entidade
                Entidades.TurmaMaterialEstudo turmaMaterialEstudo = new Techne.Lyceum.RN.LancamentoNotas.Entidades.TurmaMaterialEstudo();
                turmaMaterialEstudo.Ano = ano;
                turmaMaterialEstudo.Semestre = periodo;
                turmaMaterialEstudo.Turma = turma;
                turmaMaterialEstudo.Disciplina = disciplina;
                turmaMaterialEstudo.MaterialEstudoId = id;
                turmaMaterialEstudo.Subperiodo = bimestre;
                turmaMaterialEstudo.UsuarioId = usuarioId;

                //Insere
                this.Insere(contexto, turmaMaterialEstudo);
            }
        }

        private void Insere(DataContext contexto, Entidades.TurmaMaterialEstudo turmaMaterialEstudo)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO LancamentoNotas.TURMA_MATERIALESTUDO
                               (ANO
                               ,SEMESTRE
                               ,TURMA
                               ,DISCIPLINA
                               ,SUBPERIODO
                               ,MATERIALESTUDOID
                               ,USUARIOID
                               ,DATACADASTRO
                               ,DATAALTERACAO)
                         VALUES
                               (@ANO, 
                               @SEMESTRE,
                               @TURMA, 
                               @DISCIPLINA, 
                               @SUBPERIODO, 
                               @MATERIALESTUDOID, 
                               @USUARIOID, 
                               @DATACADASTRO, 
                               @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, turmaMaterialEstudo.Ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, turmaMaterialEstudo.Semestre);
            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turmaMaterialEstudo.Turma);
            contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, turmaMaterialEstudo.Disciplina);
            contextQuery.Parameters.Add("@SUBPERIODO", SqlDbType.Int, turmaMaterialEstudo.Subperiodo);
            contextQuery.Parameters.Add("@MATERIALESTUDOID", SqlDbType.Int, turmaMaterialEstudo.MaterialEstudoId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, turmaMaterialEstudo.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        private void RemoveLista(DataContext contexto, int ano, int periodo, int bimestre, string turma, string disciplina)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" DELETE LANCAMENTONOTAS.TURMA_MATERIALESTUDO
                                WHERE ANO = @ANO
	                                AND SEMESTRE = @SEMESTRE
	                                AND TURMA = @TURMA
	                                AND DISCIPLINA = @DISCIPLINA
	                                AND SUBPERIODO = @SUBPERIODO ";

            contextQuery.Parameters.Add("@ANO", SqlDbType.Int, ano);
            contextQuery.Parameters.Add("@SEMESTRE", SqlDbType.Int, periodo);
            contextQuery.Parameters.Add("@TURMA", SqlDbType.VarChar, turma);
            contextQuery.Parameters.Add("@DISCIPLINA", SqlDbType.VarChar, disciplina);
            contextQuery.Parameters.Add("@SUBPERIODO", SqlDbType.Int, bimestre);

            contexto.ApplyModifications(contextQuery);
        }
    }
}