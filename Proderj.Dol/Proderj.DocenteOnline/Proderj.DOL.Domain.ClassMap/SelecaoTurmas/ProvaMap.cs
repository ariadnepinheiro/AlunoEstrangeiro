//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using FluentNHibernate.Mapping;

//namespace Proderj.DOL.Domain.ClassMap
//{
//    public class ProvaMap : ClassMap<Prova>
//    {
//        public ProvaMap()
//        {
//            Table("VW_LY_PROVA");
                        
//            LazyLoad();

//            Id(x => x.MediaProva)
//                .Column("PROVA")
//                    .Not.Nullable()
//                        .Length(10);
                       
//            Map(x => x.Disciplina)
//                .Column("DISCIPLINA")
//                    .Not.Nullable()
//                        .Length(20);
            
//            Map(x => x.Turma)
//                .Column("TURMA")
//                    .Not.Nullable()
//                        .Length(20);
            
//            Map(x => x.Ano)
//                .Column("ANO")
//                    .Not.Nullable();
            
//            Map(x => x.Semestre)
//                .Column("SEMESTRE")
//                    .Not.Nullable();
            
//            //Map(x => x.Prova)
//            //    .Column("PROVA")
//            //        .Not.Nullable()
//            //            .Length(10);
            
//            Map(x => x.Ordem)
//                .Column("ORDEM")
//                    .Not.Nullable();
            
//            Map(x => x.Subperiodo)
//                .Column("SUBPERIODO");
            
//            Map(x => x.Nome)
//                .Column("NOME")
//                    .Not.Nullable()
//                        .Length(500);
            
//            Map(x => x.NotaMaxima)
//                .Column("NOTA_MAX")
//                    .Length(15);
            
//            Map(x => x.Complemento)
//                .Column("COMPLEMENTO")
//                    .Length(50);
//        }
//    }
//}
