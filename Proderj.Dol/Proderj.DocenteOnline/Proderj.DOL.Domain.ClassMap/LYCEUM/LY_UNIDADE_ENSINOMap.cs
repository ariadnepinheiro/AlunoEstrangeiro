using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap 
{
    public class LY_UNIDADE_ENSINOMap : ClassMap<LY_UNIDADE_ENSINO>
    {
        public LY_UNIDADE_ENSINOMap()
        {
			Table("LY_UNIDADE_ENSINO");
			
            LazyLoad();

            Id(x => x.UNIDADE_ENS).GeneratedBy.Assigned().Column("UNIDADE_ENS");
            Map(x => x.NOME_COMP).Column("NOME_COMP").Not.Nullable().Length(100);
            Map(x => x.NOME_ABREV).Column("NOME_ABREV").Length(100);
            Map(x => x.ENDERECO).Column("ENDERECO").Not.Nullable().Length(50);
            Map(x => x.END_NUM).Column("END_NUM").Not.Nullable().Length(15);
            Map(x => x.END_COMPL).Column("END_COMPL").Length(50);
            Map(x => x.BAIRRO).Column("BAIRRO").Length(50);
            Map(x => x.MUNICIPIO).Column("MUNICIPIO").Not.Nullable().Length(20);
            Map(x => x.CEP).Column("CEP").Not.Nullable().Length(9);
            Map(x => x.CAIXA_POSTAL).Column("CAIXA_POSTAL").Length(15);
            Map(x => x.FONE).Column("FONE").Length(30);
            Map(x => x.FAX).Column("FAX").Length(30);
            Map(x => x.CGC).Column("CGC").Length(19);
            Map(x => x.E_MAIL).Column("E_MAIL").Length(100);
            Map(x => x.TURMAPREF).Column("TURMAPREF").Length(1);
            Map(x => x.CCM).Column("CCM").Length(20);
            Map(x => x.MNEMONICO).Column("MNEMONICO").Length(2);
            Map(x => x.OUTRA_FACULDADE).Column("OUTRA_FACULDADE").Length(20);
            Map(x => x.BANCO).Column("BANCO").Precision(3);
            Map(x => x.AGENCIA).Column("AGENCIA").Length(15);
            Map(x => x.CONTA_BANCO).Column("CONTA_BANCO").Length(15);
            Map(x => x.TITULAR).Column("TITULAR").Length(50);
            Map(x => x.WEB_SITE).Column("WEB_SITE").Length(100);
            Map(x => x.INEP_FACULDADE).Column("INEP_FACULDADE").Length(20);
            Map(x => x.INSCR_ESTADUAL).Column("INSCR_ESTADUAL").Length(50);
            Map(x => x.MARCA).Column("MARCA").Length(20);
            Map(x => x.GRUPO).Column("GRUPO").Length(20);
            Map(x => x.STAMP_ATUALIZACAO).Column("STAMP_ATUALIZACAO");
            Map(x => x.NUCLEO).Column("NUCLEO").Length(20);
            Map(x => x.SETOR).Column("SETOR").Length(15);
            Map(x => x.TIPO).Column("TIPO").Length(40);
            Map(x => x.DEPENDENCIA_ADM).Column("DEPENDENCIA_ADM").Length(100);
            Map(x => x.CLASSIFICACAO).Column("CLASSIFICACAO").Length(40);
            Map(x => x.EXTRACLASSE).Column("EXTRACLASSE").Length(1);
            Map(x => x.SIT_FUNCIONAMENTO).Column("SIT_FUNCIONAMENTO").Length(100);
            Map(x => x.ID_REGIONAL).Column("ID_REGIONAL").Precision(10);
            Map(x => x.TEL2).Column("TEL2").Length(30);
            Map(x => x.MATRICULA).Column("MATRICULA").Length(8);
            Map(x => x.DT_CADASTRO).Column("DT_CADASTRO");
            Map(x => x.ESCOLA_ABERTA).Column("ESCOLA_ABERTA").Length(1);
            Map(x => x.POSSUIPAGINAWEB).Column("POSSUIPAGINAWEB").Length(1);
            Map(x => x.PAGINAWEB).Column("PAGINAWEB").Length(500);
            Map(x => x.POSSUIPROJETOPEDAGOGICO).Column("POSSUIPROJETOPEDAGOGICO").Length(1);
            Map(x => x.CUMPRIUPROJETOPEDAGOGICO).Column("CUMPRIUPROJETOPEDAGOGICO").Length(1);

            //References(x => x.LY_INSTITUICAO).Column("OUTRA_FACULDADE");
            //References(x => x.LY_MARCA).Column("MARCA");
            //References(x => x.LY_GRUPO_UNID_ENS).Column("GRUPO");
            //References(x => x.LY_NUCLEO).Column("NUCLEO");
        }
    }
}
