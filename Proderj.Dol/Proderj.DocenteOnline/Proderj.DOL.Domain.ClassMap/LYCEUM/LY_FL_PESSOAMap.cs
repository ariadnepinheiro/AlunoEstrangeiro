using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
    public class LY_FL_PESSOAMap : ClassMap<LY_FL_PESSOA>
    {
        public LY_FL_PESSOAMap()
        {
			Table("LY_FL_PESSOA");
			
            LazyLoad();

            Id(x => x.PESSOA).GeneratedBy.Assigned().Column("PESSOA");
            //Map(x => x.PESSOA).Column("PESSOA").Not.Nullable().Precision(10);
            Map(x => x.FL_FIELD_01).Column("FL_FIELD_01").Length(2000);
            //Map(x => x.FL_FIELD_02).Column("FL_FIELD_02").Length(2000);
            //Map(x => x.FL_FIELD_03).Column("FL_FIELD_03").Length(2000);
            //Map(x => x.FL_FIELD_04).Column("FL_FIELD_04").Length(2000);
            //Map(x => x.FL_FIELD_05).Column("FL_FIELD_05").Length(2000);
            //Map(x => x.FL_FIELD_06).Column("FL_FIELD_06").Length(2000);
            //Map(x => x.FL_FIELD_07).Column("FL_FIELD_07").Length(2000);
            //Map(x => x.FL_FIELD_08).Column("FL_FIELD_08").Length(2000);
            //Map(x => x.FL_FIELD_09).Column("FL_FIELD_09").Length(2000);
            //Map(x => x.FL_FIELD_10).Column("FL_FIELD_10").Length(2000);
            //Map(x => x.FL_FIELD_11).Column("FL_FIELD_11").Length(2000);
            //Map(x => x.FL_FIELD_12).Column("FL_FIELD_12").Length(2000);
            //Map(x => x.FL_FIELD_13).Column("FL_FIELD_13").Length(2000);
            //Map(x => x.FL_FIELD_14).Column("FL_FIELD_14").Length(2000);
            //Map(x => x.FL_FIELD_15).Column("FL_FIELD_15").Length(2000);
            //Map(x => x.FL_FIELD_16).Column("FL_FIELD_16").Length(2000);
            //Map(x => x.FL_FIELD_17).Column("FL_FIELD_17").Length(2000);
            //Map(x => x.FL_FIELD_18).Column("FL_FIELD_18").Length(2000);
            //Map(x => x.FL_FIELD_19).Column("FL_FIELD_19").Length(2000);
            //Map(x => x.FL_FIELD_20).Column("FL_FIELD_20").Length(2000);
            //Map(x => x.FL_FIELD_21).Column("FL_FIELD_21").Length(2000);
            //Map(x => x.FL_FIELD_22).Column("FL_FIELD_22").Length(2000);
            //Map(x => x.FL_FIELD_23).Column("FL_FIELD_23").Length(2000);
            //Map(x => x.FL_FIELD_24).Column("FL_FIELD_24").Length(2000);
            //Map(x => x.FL_FIELD_25).Column("FL_FIELD_25").Length(2000);
            //Map(x => x.FL_FIELD_26).Column("FL_FIELD_26").Length(2000);
            //Map(x => x.FL_FIELD_27).Column("FL_FIELD_27").Length(2000);
            //Map(x => x.FL_FIELD_28).Column("FL_FIELD_28").Length(2000);
            //Map(x => x.FL_FIELD_29).Column("FL_FIELD_29").Length(2000);
            //Map(x => x.FL_FIELD_30).Column("FL_FIELD_30").Length(2000);
            //Map(x => x.FL_FIELD_31).Column("FL_FIELD_31").Length(2000);
            //Map(x => x.FL_FIELD_32).Column("FL_FIELD_32").Length(2000);
            //Map(x => x.FL_FIELD_33).Column("FL_FIELD_33").Length(2000);
            //Map(x => x.FL_FIELD_34).Column("FL_FIELD_34").Length(2000);
            //Map(x => x.FL_FIELD_35).Column("FL_FIELD_35").Length(2000);
            //Map(x => x.FL_FIELD_36).Column("FL_FIELD_36").Length(2000);
            //Map(x => x.FL_FIELD_37).Column("FL_FIELD_37").Length(2000);
            //Map(x => x.FL_FIELD_38).Column("FL_FIELD_38").Length(2000);
            //Map(x => x.FL_FIELD_39).Column("FL_FIELD_39").Length(2000);
            //Map(x => x.FL_FIELD_40).Column("FL_FIELD_40").Length(2000);

            //References(x => x.LY_PESSOA, "PESSOA")
            //    .Cascade.None();
        }
    }
}
