using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.Foundation.Framework;
using NHibernate;
using NHibernate.Transform;
using Proderj.DOL.Domain;

namespace Proderj.DOL.Repository
{
    public class TurmaMaterialEstudoRepository : NHRepositoryBase<TurmaMaterialEstudo>, ITurmaMaterialEstudoRepository
    {
        public IList<TurmaMaterialEstudo> ObtemPor(string identificador)
        {

            ISQLQuery qry = SessaoAuditada.CreateSQLQuery(@"
                SELECT DESCRICAO as [Descricao], MATERIALESTUDOID as [MaterialEstudoId] from lancamentonotas.materialestudo 
                WHERE ATIVO = 1                             
            ");

            

            qry.SetResultTransformer(Transformers.AliasToBean<TurmaMaterialEstudo>());

            return qry.List<TurmaMaterialEstudo>();

        }

        public IList<TurmaMaterialEstudo> ObtemTurmaMatEstPor(string turma, int ano, int semestre, string disciplina, int subperiodo)
        {

            ISQLQuery qry = SessaoAuditada.CreateSQLQuery(@"
                SELECT TURMA_MATERIALESTUDOID as [Turma_MaterialEstudoId] , MATERIALESTUDOID
				as [MaterialEstudoId], SUBPERIODO as [SubPeriodo] from lancamentonotas.TURMA_MATERIALESTUDO 
                WHERE TURMA = :TURMA and DISCIPLINA = :DISCIPLINA and SEMESTRE = :SEMESTRE and ANO = :ANO and SUBPERIODO = :SUBPERIODO                            
            ");

            

            qry.SetParameter<string>("TURMA", turma);
            qry.SetParameter<int>("ANO", ano);
            qry.SetParameter<int>("SEMESTRE", semestre);
            qry.SetParameter<string>("DISCIPLINA", disciplina);
            qry.SetParameter<int>("SUBPERIODO", subperiodo);

            qry.SetResultTransformer(Transformers.AliasToBean<TurmaMaterialEstudo>());

            return qry.List<TurmaMaterialEstudo>();

        }

        

        public int Insere(TurmaMaterialEstudo turmaMaterialEstudo)
        {

            try
            {
                var query = SessaoAuditada.CreateSQLQuery(
                                    @" INSERT  INTO lancamentonotas.turma_materialestudo       
                                    (
                                        
                                        ANO,
                                        SEMESTRE,
                                        TURMA,
                                        DISCIPLINA,
                                        SUBPERIODO,
                                        MATERIALESTUDOID,                                        
                                        USUARIOID
                                        
                                    )
									VALUES  
									(
										
										:ANO,
										:SEMESTRE,
										:TURMA,
										:DISCIPLINA,
										:SUBPERIODO,
										:MATERIALESTUDOID,                                        
										:USUARIOID
									)");

                //query.SetInt64("TURMA_MATERIALESTUDOID", turmaMaterialEstudo.tur);
                query.SetInt32("ANO", turmaMaterialEstudo.Ano);
                query.SetInt32("SEMESTRE", turmaMaterialEstudo.Semestre);
                query.SetString("TURMA", !String.IsNullOrWhiteSpace( turmaMaterialEstudo.Turma) ? turmaMaterialEstudo.Turma : "" );
                query.SetString("DISCIPLINA", !String.IsNullOrWhiteSpace(turmaMaterialEstudo.Disciplina) ? turmaMaterialEstudo.Disciplina : "");
                //query.SetInt32("SUBPERIODO", turmaMaterialEstudo.SubPeriodo);
                query.SetDecimal("SUBPERIODO", turmaMaterialEstudo.SubPeriodo);
                //query.SetInt32("MATERIALESTUDOID", turmaMaterialEstudo.MaterialEstudoId);
                query.SetInt32("MATERIALESTUDOID", turmaMaterialEstudo.MaterialEstudoId);
                //query.SetInt32("TURMA_MATERIALESTUDO", turmaMaterialEstudo.Turma_MaterialEstudo);
                query.SetString("USUARIOID", turmaMaterialEstudo.UsuarioId);
                //query.SetString("DATACADASTRO", turmaMaterialEstudo.DataCadastro);
                //query.SetString("DATAALTERACAO", turmaMaterialEstudo.DataAlteracao);


                return query.ExecuteUpdate();
            }
            catch (Exception ex)
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }
        }

        public int Remove(int identificador, string turma, int ano, int semestre, string disciplina, int subperiodo)
        {
            StringBuilder stb = new StringBuilder();

            try
            {
                                
                stb.Append("delete from Proderj.DOL.Domain.TurmaMaterialEstudo ");
                stb.Append("where MATERIALESTUDOID = :MATERIALESTUDOID and turma = :TURMA and ano = :ANO and semestre = :SEMESTRE and disciplina = :DISCIPLINA and subperiodo = :SUBPERIODO ");

                var query = SessaoAuditada.CreateQuery(stb.ToString());
                query.SetParameter("MATERIALESTUDOID", identificador);

                query.SetParameter("TURMA", turma);
                query.SetParameter("ANO", ano);
                query.SetParameter("SEMESTRE", semestre);
                query.SetParameter("DISCIPLINA", disciplina);
                query.SetParameter("SUBPERIODO", subperiodo);

                return query.ExecuteUpdate();

            }
            catch (Exception ex)
            {
                throw new Exception("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.");
            }
        }

    }
}
