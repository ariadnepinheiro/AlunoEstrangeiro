using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
    public class LY_PESSOAMap : ClassMap<LY_PESSOA>
    {
        public LY_PESSOAMap()
        {
			Table("LY_PESSOA");
			
            LazyLoad();

            Id(x => x.PESSOA).GeneratedBy.Assigned().Column("PESSOA");
            //Map(x => x.NOME_COMPL).Column("NOME_COMPL").Not.Nullable().Length(100);
            //Map(x => x.DT_NASC).Column("DT_NASC");
            //Map(x => x.MUNICIPIO_NASC).Column("MUNICIPIO_NASC").Length(20);
            //Map(x => x.PAIS_NASC).Column("PAIS_NASC").Length(50);
            //Map(x => x.NACIONALIDADE).Column("NACIONALIDADE").Length(15);
            //Map(x => x.NOME_PAI).Column("NOME_PAI").Length(100);
            //Map(x => x.NOME_MAE).Column("NOME_MAE").Length(100);
            //Map(x => x.SEXO).Column("SEXO").Length(1);
            //Map(x => x.EST_CIVIL).Column("EST_CIVIL").Length(30);
            Map(x => x.ENDERECO).Column("ENDERECO").Not.Nullable().Length(50);
            Map(x => x.END_NUM).Column("END_NUM").Not.Nullable().Length(15);
            Map(x => x.END_COMPL).Column("END_COMPL").Length(50);
            Map(x => x.BAIRRO).Column("BAIRRO").Length(50);
            Map(x => x.END_MUNICIPIO).Column("END_MUNICIPIO").Not.Nullable().Length(20);
            Map(x => x.END_PAIS).Column("END_PAIS").Length(50);
            Map(x => x.CEP).Column("CEP").Not.Nullable().Length(9);
            Map(x => x.FONE).Column("FONE").Length(30);
            //Map(x => x.RG_NUM).Column("RG_NUM").Length(20);
            //Map(x => x.RG_TIPO).Column("RG_TIPO").Length(15);
            //Map(x => x.RG_EMISSOR).Column("RG_EMISSOR").Length(15);
            //Map(x => x.RG_UF).Column("RG_UF").Length(2);
            //Map(x => x.RG_DTEXP).Column("RG_DTEXP");
            //Map(x => x.CPF).Column("CPF").Length(14);
            //Map(x => x.ALIST_NUM).Column("ALIST_NUM").Length(17);
            //Map(x => x.ALIST_SERIE).Column("ALIST_SERIE").Length(15);
            //Map(x => x.ALIST_RM).Column("ALIST_RM").Length(15);
            //Map(x => x.ALIST_CSM).Column("ALIST_CSM").Length(15);
            //Map(x => x.ALIST_DTEXP).Column("ALIST_DTEXP");
            //Map(x => x.CR_NUM).Column("CR_NUM").Length(17);
            //Map(x => x.CR_CAT).Column("CR_CAT").Length(15);
            //Map(x => x.CR_SERIE).Column("CR_SERIE").Length(15);
            //Map(x => x.CR_RM).Column("CR_RM").Length(15);
            //Map(x => x.CR_CSM).Column("CR_CSM").Length(15);
            //Map(x => x.CR_DTEXP).Column("CR_DTEXP");
            //Map(x => x.TELEITOR_NUM).Column("TELEITOR_NUM").Length(15);
            //Map(x => x.TELEITOR_ZONA).Column("TELEITOR_ZONA").Length(15);
            //Map(x => x.TELEITOR_SECAO).Column("TELEITOR_SECAO").Length(15);
            //Map(x => x.TELEITOR_DTEXP).Column("TELEITOR_DTEXP");
            //Map(x => x.CPROF_NUM).Column("CPROF_NUM").Length(15);
            //Map(x => x.CPROF_SERIE).Column("CPROF_SERIE").Length(15);
            //Map(x => x.CPROF_UF).Column("CPROF_UF").Length(2);
            //Map(x => x.CPROF_DTEXP).Column("CPROF_DTEXP");
            Map(x => x.E_MAIL).Column("E_MAIL").Length(100);
            //Map(x => x.RESP_NOME_COMPL).Column("RESP_NOME_COMPL").Length(100);
            //Map(x => x.RESP_FONE).Column("RESP_FONE").Length(30);
            //Map(x => x.RESP_CPF).Column("RESP_CPF").Length(14);
            Map(x => x.CELULAR).Column("CELULAR").Length(30);
            Map(x => x.E_MAIL_INTERNO).Column("E_MAIL_INTERNO").Length(100);
            //Map(x => x.TELEITOR_MUN).Column("TELEITOR_MUN").Length(20);
            //Map(x => x.CERT_NASC_NUM).Column("CERT_NASC_NUM").Length(15);
            //Map(x => x.CERT_NASC_FOLHA).Column("CERT_NASC_FOLHA").Length(15);
            //Map(x => x.CERT_NASC_LIVRO).Column("CERT_NASC_LIVRO").Length(15);
            //Map(x => x.CERT_NASC_EMISSAO).Column("CERT_NASC_EMISSAO");
            //Map(x => x.CERT_NASC_CARTORIO_UF).Column("CERT_NASC_CARTORIO_UF").Length(2);
            //Map(x => x.CERT_NASC_CARTORIO_EXPED).Column("CERT_NASC_CARTORIO_EXPED").Length(1000);
            //Map(x => x.PASSAPORTE).Column("PASSAPORTE").Length(50);
            //Map(x => x.ID_CENSO).Column("ID_CENSO").Length(20);
            //Map(x => x.TIPO_SANGUINEO).Column("TIPO_SANGUINEO").Length(40);
            //Map(x => x.ETNIA).Column("ETNIA").Length(40);
            //Map(x => x.CREDO).Column("CREDO").Length(40);
            //Map(x => x.QT_FILHOS).Column("QT_FILHOS").Precision(3);
            //Map(x => x.PRE_NOME_SOCIAL).Column("PRE_NOME_SOCIAL").Length(100);
            //Map(x => x.STAMP_ATUALIZACAO).Column("STAMP_ATUALIZACAO");
            //Map(x => x.CERT_NUMERO_MATRICULA).Column("CERT_NUMERO_MATRICULA").Length(50);
            //Map(x => x.ID_CARTORIO).Column("ID_CARTORIO").Precision(10);
            //Map(x => x.MAE_FALECIDA).Column("MAE_FALECIDA").Length(1);
            //Map(x => x.PAI_FALECIDO).Column("PAI_FALECIDO").Length(1);
            //Map(x => x.MAE_CPF).Column("MAE_CPF").Length(14);
            //Map(x => x.PAI_CPF).Column("PAI_CPF").Length(14);
            //Map(x => x.MAE_TELEFONE).Column("MAE_TELEFONE").Length(30);
            //Map(x => x.RESPONSAVEL).Column("RESPONSAVEL").Length(50);
            //Map(x => x.PAI_TELEFONE).Column("PAI_TELEFONE").Length(30);
            //Map(x => x.IDFUNCIONAL).Column("IDFUNCIONAL").Precision(10);
            //Map(x => x.PISPASEP).Column("PISPASEP").Length(11);
            Map(x => x.USUARIOID).Column("USUARIOID").Length(15);
            //Map(x => x.DATACADASTRO).Column("DATACADASTRO");
            Map(x => x.DATAALTERACAO).Column("DATAALTERACAO");
            //Map(x => x.NECESSIDADEESPECIALID).Column("NECESSIDADEESPECIALID").Precision(10);
            //Map(x => x.LATITUDE).Column("LATITUDE").Length(50);
            //Map(x => x.LONGITUDE).Column("LONGITUDE").Length(50);
            Map(x => x.AREA_QUILOMBOS).Column("AREA_QUILOMBOS").Length(1);
            Map(x => x.TERRA_INDIGENA).Column("TERRA_INDIGENA").Length(1);
            Map(x => x.AREA_ASSENTAMENTO).Column("AREA_ASSENTAMENTO").Length(1);

            HasMany(x => x.LY_DOCENTE)
                .KeyColumn("PESSOA")
                .Inverse()
                .Cascade.None();

            HasOne(x => x.LY_FL_PESSOA)
                .ForeignKey("PESSOA")
                .Cascade.None();
        }
    }
}
