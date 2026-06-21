using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
    public class DadosCapacitacaoDocenteMap: ClassMap<DadosCapacitacaoDocente>
    {
        public DadosCapacitacaoDocenteMap()
        {
            Table("VW_CAPACITACAO_DOCENTE");

            LazyLoad();

            Id(x => x.Row)
                .Column("Row");

            Map(x => x.Matricula)
                .Column("MATRICULA")
                .Not.Nullable()
                .Length(100);

            Map(x => x.OferecidoSEEDUC)
                .Column("OFERECIDOSEEDUC")
                .Length(3);

            Map(x => x.TipoCurso)
                .Column("TIPO_CURSO")
                .Length(200);

            Map(x => x.AreaConhecimento)
                .Column("AREA_CONHECIMENTO")
                .Not.Nullable()
                .Length(200);

            Map(x => x.Capacitacao)
                .Column("CAPACITACAO")
                .Length(200);

            Map(x => x.NomeInstituicao)
                .Column("NOME_INSTITUICAO")
                .Length(500);

            Map(x => x.CargaHoraria)
                .Column("CARGA_HORARIA");

            Map(x => x.DataConclusao)
                .Column("DATA_CONCLUSAO");
        }
    }
}
