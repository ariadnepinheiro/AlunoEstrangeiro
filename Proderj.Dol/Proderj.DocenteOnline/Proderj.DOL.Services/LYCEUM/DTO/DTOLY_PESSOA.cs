using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Repository;

namespace Proderj.DOL.Service
{
    public class DTOLY_PESSOA
    {
        public DTOLY_PESSOA()
        {
            LY_DOCENTE = new HashSet<DTOLY_DOCENTE>();
        }

        public int? PESSOA { get; set; }
        //public string NOME_COMPL { get; set; }
        //public DateTime? DT_NASC { get; set; }
        //public string MUNICIPIO_NASC { get; set; }
        //public string PAIS_NASC { get; set; }
        //public string NACIONALIDADE { get; set; }
        //public string NOME_PAI { get; set; }
        //public string NOME_MAE { get; set; }
        //public char SEXO { get; set; }
        //public string EST_CIVIL { get; set; }
        public string ENDERECO { get; set; }
        public string END_NUM { get; set; }
        public string END_COMPL { get; set; }
        public string BAIRRO { get; set; }
        public string END_MUNICIPIO { get; set; }
        public string END_PAIS { get; set; }
        public string CEP { get; set; }
        public string FONE { get; set; }
        //public string RG_NUM { get; set; }
        //public string RG_TIPO { get; set; }
        //public string RG_EMISSOR { get; set; }
        //public string RG_UF { get; set; }
        //public DateTime? RG_DTEXP { get; set; }
        //public string CPF { get; set; }
        //public string ALIST_NUM { get; set; }
        //public string ALIST_SERIE { get; set; }
        //public string ALIST_RM { get; set; }
        //public string ALIST_CSM { get; set; }
        //public DateTime? ALIST_DTEXP { get; set; }
        //public string CR_NUM { get; set; }
        //public string CR_CAT { get; set; }
        //public string CR_SERIE { get; set; }
        //public string CR_RM { get; set; }
        //public string CR_CSM { get; set; }
        //public DateTime? CR_DTEXP { get; set; }
        //public string TELEITOR_NUM { get; set; }
        //public string TELEITOR_ZONA { get; set; }
        //public string TELEITOR_SECAO { get; set; }
        //public DateTime? TELEITOR_DTEXP { get; set; }
        //public string CPROF_NUM { get; set; }
        //public string CPROF_SERIE { get; set; }
        //public string CPROF_UF { get; set; }
        //public DateTime? CPROF_DTEXP { get; set; }
        public string E_MAIL { get; set; }
        //public string RESP_NOME_COMPL { get; set; }
        //public string RESP_FONE { get; set; }
        //public string RESP_CPF { get; set; }
        public string CELULAR { get; set; }
        public string E_MAIL_INTERNO { get; set; }
        public string E_MAIL_EDUCA { get; set; }
        //public string TELEITOR_MUN { get; set; }
        //public string CERT_NASC_NUM { get; set; }
        //public string CERT_NASC_FOLHA { get; set; }
        //public string CERT_NASC_LIVRO { get; set; }
        //public DateTime? CERT_NASC_EMISSAO { get; set; }
        //public string CERT_NASC_CARTORIO_UF { get; set; }
        //public string CERT_NASC_CARTORIO_EXPED { get; set; }
        //public string PASSAPORTE { get; set; }
        //public string ID_CENSO { get; set; }
        //public string TIPO_SANGUINEO { get; set; }
        //public string ETNIA { get; set; }
        //public string CREDO { get; set; }
        //public int? QT_FILHOS { get; set; }
        //public string PRE_NOME_SOCIAL { get; set; }
        //public DateTime? STAMP_ATUALIZACAO { get; set; }
        //public string CERT_NUMERO_MATRICULA { get; set; }
        //public int? ID_CARTORIO { get; set; }
        //public char? MAE_FALECIDA { get; set; }
        //public char? PAI_FALECIDO { get; set; }
        //public string MAE_CPF { get; set; }
        //public string PAI_CPF { get; set; }
        //public string MAE_TELEFONE { get; set; }
        //public string RESPONSAVEL { get; set; }
        //public string PAI_TELEFONE { get; set; }
        //public int IDFUNCIONAL { get; set; }
        //public string PISPASEP { get; set; }
        public string USUARIOID { get; set; }
        //public DateTime? DATACADASTRO { get; set; }
        public DateTime? DATAALTERACAO { get; set; }
        //public int? NECESSIDADEESPECIALID { get; set; }
        //public int? ID_INSCRICAO { get; set; }
        //public string LATITUDE { get; set; }
        //public string LONGITUDE { get; set; }
        public char? AREA_QUILOMBOS { get; set; }
        public char? TERRA_INDIGENA { get; set; }
        public char? AREA_ASSENTAMENTO { get; set; }

        public class DTOLY_DOCENTE
        {
            public int? NUM_FUNC { get; set; }
            //public string SENHA_DOL { get; set; }
            //public string CATEGORIA { get; set; }
            //public DateTime? DT_ADMISSAO { get; set; }
            //public int? PESSOA { get; set; }
            //public string REGIME_TRABALHO { get; set; }
            //public DateTime? DT_DEMISSAO { get; set; }
            //public DateTime? STAMP_ATUALIZACAO { get; set; }
            public string MATRICULA { get; set; }
            //public char? SENHA_ALTERADA { get; set; }
            //public int? ANO_INGRESSO { get; set; }
            //public string CONCURSO { get; set; }
            //public string CANDIDATO { get; set; }
            //public char? VOLUNTARIO { get; set; }
            //public int? REGIMECONTRATACAOID { get; set; }
            //public int ACUMULACAO { get; set; }
            //public int? VINCULO { get; set; }
            //public string USUARIO { get; set; }
            //public DateTime? DATACADASTRO { get; set; }
            //public DateTime? DATAALTERACAO { get; set; }
        }

        public ICollection<DTOLY_DOCENTE> LY_DOCENTE { get; set; }

        public class DTOLY_FL_PESSOA
        {
            public string FL_FIELD_01 { get; set; }
            //public string FL_FIELD_02 { get; set; }
            //public string FL_FIELD_03 { get; set; }
            //public string FL_FIELD_04 { get; set; }
            //public string FL_FIELD_05 { get; set; }
            //public string FL_FIELD_06 { get; set; }
            //public string FL_FIELD_07 { get; set; }
            //public string FL_FIELD_08 { get; set; }
            //public string FL_FIELD_09 { get; set; }
            //public string FL_FIELD_10 { get; set; }
            //public string FL_FIELD_11 { get; set; }
            //public string FL_FIELD_12 { get; set; }
            //public string FL_FIELD_13 { get; set; }
            //public string FL_FIELD_14 { get; set; }
            //public string FL_FIELD_15 { get; set; }
            //public string FL_FIELD_16 { get; set; }
            //public string FL_FIELD_17 { get; set; }
            //public string FL_FIELD_18 { get; set; }
            //public string FL_FIELD_19 { get; set; }
            //public string FL_FIELD_20 { get; set; }
            //public string FL_FIELD_21 { get; set; }
            //public string FL_FIELD_22 { get; set; }
            //public string FL_FIELD_23 { get; set; }
            //public string FL_FIELD_24 { get; set; }
            //public string FL_FIELD_25 { get; set; }
            //public string FL_FIELD_26 { get; set; }
            //public string FL_FIELD_27 { get; set; }
            //public string FL_FIELD_28 { get; set; }
            //public string FL_FIELD_29 { get; set; }
            //public string FL_FIELD_30 { get; set; }
            //public string FL_FIELD_31 { get; set; }
            //public string FL_FIELD_32 { get; set; }
            //public string FL_FIELD_33 { get; set; }
            //public string FL_FIELD_34 { get; set; }
            //public string FL_FIELD_35 { get; set; }
            //public string FL_FIELD_36 { get; set; }
            //public string FL_FIELD_37 { get; set; }
            //public string FL_FIELD_38 { get; set; }
            //public string FL_FIELD_39 { get; set; }
            //public string FL_FIELD_40 { get; set; }
        }

        public DTOLY_FL_PESSOA LY_FL_PESSOA { get; set; }

        public DTOREL_CH_SERV_ANO REL_CH_SERV_ANO1 { get; set; }
        public DTOREL_CH_SERV_ANO REL_CH_SERV_ANO2 { get; set; }

        public string END_UF { get; set; }
    }
}
