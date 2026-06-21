using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain
{
    public class TurmaMaterialEstudoMap : ClassMap<TurmaMaterialEstudo>
    {
        public TurmaMaterialEstudoMap()
        {
            Table("lancamentonotas.turma_materialestudo");
            			
            LazyLoad();

            //Chave primária 
            Id(x => x.Turma_MaterialEstudoId)
                .Column("TURMA_MATERIALESTUDOID")
                    .Not.Nullable();

            Map(x => x.MaterialEstudoId)
                .Column("MATERIALESTUDOID")
                    .Not.Nullable();
			
            Map(x => x.Descricao)
                .Column("DESCRICAO")
                    .Length(100)
                    .Not.Nullable();

            Map(x => x.Ano)
               .Column("ANO")
                   .Not.Nullable();

            Map(x => x.Semestre)
               .Column("SEMESTRE")                   
                   .Not.Nullable();

            Map(x => x.Turma)
               .Column("TURMA")
                   .Length(100)
                   .Not.Nullable();

            Map(x => x.Disciplina)
                .Column("DISCIPLINA")
                    .Length(100)
                    .Not.Nullable();

            Map(x => x.SubPeriodo)
                .Column("SUBPERIODO")
                    .Not.Nullable();

            Map(x => x.Ativo)
                .Column("ATIVO")
                    .Not.Nullable();

            Map(x => x.UsuarioId)
                .Column("USUARIOID");
			
            Map(x => x.DataCadastro)
                .Column("DATACADASTRO");
			
            Map(x => x.DataAlteracao)
                .Column("DATAALTERACAO");
			
            
        }
    }
}
