using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap.LancamentoNotas
{
    public class MaterialEstudoMap : ClassMap<MaterialEstudo>
    {
        public MaterialEstudoMap()
        {
            Table("lancamentonotas.materialestudo");
            			
            LazyLoad();

            //Chave primária 
            Id(x => x.MaterialEstudoId)
                .Column("MATERIALESTUDOID")
                    .Not.Nullable();
			
            Map(x => x.Descricao)
                .Column("DESCRICAO")
                    .Length(100)
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
