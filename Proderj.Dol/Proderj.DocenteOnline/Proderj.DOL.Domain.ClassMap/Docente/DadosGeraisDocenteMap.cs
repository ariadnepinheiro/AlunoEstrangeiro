using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Proderj.DOL.Domain.ClassMap
{
    public class DadosGeraisDocenteMap : ClassMap<DadosGeraisDocente>
    {
        public DadosGeraisDocenteMap()
        {
            Table("VW_DADOSCADASTRAIS_DOCENTE");

            LazyLoad();

            Id(x => x.Matricula)
                .Column("MATRICULA")
                .Length(100)
                .Not.Nullable();

            Map(x => x.Num_func)
                .Column("NUM_FUNC");

            Map(x => x.IDFuncional)
                .Column("IDFUNCIONAL");

            Map(x => x.IDVinculo)
                .Column("IDVINCULO");

            Map(x => x.Nome)
                .Column("NOME_DOCENTE")
                .Length(100);

            Map(x => x.NomeSocial)
                .Column("PRE_NOME_SOCIAL")
                .Length(100);

            Map(x => x.DataNasc)
                .Column("DATA_NASC");
            
            Map(x => x.CorRaca)
                .Column("ETNIA")
                .Length(50);

            Map(x => x.Sexo)
                .Column("SEXO")
                .Length(1);

            Map(x => x.NecessidadeEspecial)
                .Column("NECESSIDADE_ESPECIAL")
                .Length(40);
            
            Map(x => x.EstadoCivil)
                .Column("EST_CIVIL")
                .Length(30);

            Map(x => x.PaisNasc)
                .Column("PAIS_NASC")
                .Length(50)
                .Not.Nullable();

            Map(x => x.Nacionalidade)
               .Column("NACIONALIDADE")
               .Length(15);

            Map(x => x.Naturalidade)
                .Column("NATURALIDADE")
                .Length(50)
                .Not.Nullable();

            Map(x => x.UFNascimento)
                .Column("UF_NASC")
                .Length(2)
                .Not.Nullable();
            
            Map(x => x.Endereco)
                .Column("ENDERECO")
                .Length(50)
                .Not.Nullable();

            Map(x => x.Numero)
                .Column("NUMERO")
                .Length(15)
                .Not.Nullable();

            Map(x => x.Complemento)
                .Column("COMPLEMENTO")
                .Length(50);

            Map(x => x.Bairro)
                .Column("BAIRRO")
                .Length(50);

            Map(x => x.EndMunicipio)
                .Column("END_MUNICIPIO")
                .Not.Nullable()
                .Length(50);

            Map(x => x.UFEndereco)
                .Column("UF_END")
                .Length(2)
                .Not.Nullable();

            Map(x => x.Cep)
                .Column("CEP")
                .Length(9)
                .Not.Nullable();

            Map(x => x.Pais)
                .Column("PAIS")
                .Length(50)
                .Not.Nullable();

            Map(x => x.ZonaResidencial)
                .Column("ZONA_RESIDENCIAL")
                .Length(20);

            Map(x => x.Telefone)
                .Column("TELEFONE")
                .Length(30);

            Map(x => x.Celular)
                .Column("CELULAR")
                .Length(30);

            Map(x => x.Email)
                .Column("E_MAIL")
                .Length(100);

            Map(x => x.RGTipo)
                .Column("RG_TIPO")
                .Length(15);

            Map(x => x.RGNumero)
                .Column("RG_NUM")
                .Length(20);

            Map(x => x.RGUF)
                .Column("RG_UF")
                .Length(2);

            Map(x => x.RGEmissor)
                .Column("RG_EMISSOR")
                .Length(15);

            Map(x => x.RGDtExp)
                .Column("RG_DTEXP");

            Map(x => x.CPF)
                .Column("CPF")
                .Length(14);

            Map(x => x.EmailInterno)
                .Column("E_MAIL_INTERNO")
                .Length(100);

            Map(x => x.EmailGoogle)
                .Column("E_MAIL_GOOGLE")
                .Length(100);
        }
    }
}
