using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proderj.DOL.Service
{
    public class DTOLY_DOCENTE
    {
        public DTOLY_DOCENTE()
        {
            
        }

        public virtual int? NUM_FUNC { get; set; }
        public virtual string SENHA_DOL { get; set; }
        public virtual string CATEGORIA { get; set; }
        public virtual DateTime? DT_ADMISSAO { get; set; }
        public virtual int? PESSOA { get; set; }
        public virtual string REGIME_TRABALHO { get; set; }
        public virtual DateTime? DT_DEMISSAO { get; set; }
        public virtual DateTime? STAMP_ATUALIZACAO { get; set; }
        public virtual string MATRICULA { get; set; }
        public virtual char? SENHA_ALTERADA { get; set; }
        public virtual int? ANO_INGRESSO { get; set; }
        public virtual string CONCURSO { get; set; }
        public virtual string CANDIDATO { get; set; }
        public virtual char? VOLUNTARIO { get; set; }
        public virtual int? REGIMECONTRATACAOID { get; set; }
        public virtual int ACUMULACAO { get; set; }
        public virtual int? VINCULO { get; set; }
        public virtual string USUARIO { get; set; }
        public virtual DateTime? DATACADASTRO { get; set; }
        public virtual DateTime? DATAALTERACAO { get; set; }

        public class DTOLY_PESSOA
        {
            public virtual int? PESSOA { get; set; }
            public virtual string NOME_COMPL { get; set; }
            public virtual string PRE_NOME_SOCIAL { get; set; }
            public virtual DateTime? DT_NASC { get; set; }
            public virtual string MUNICIPIO_NASC { get; set; }
            public virtual string PAIS_NASC { get; set; }
            public virtual string NACIONALIDADE { get; set; }
            public virtual string NOME_PAI { get; set; }
            public virtual string NOME_MAE { get; set; }
            public virtual char SEXO { get; set; }
            public virtual string EST_CIVIL { get; set; }
            public virtual string ENDERECO { get; set; }
            public virtual string END_NUM { get; set; }
            public virtual string END_COMPL { get; set; }
            public virtual string BAIRRO { get; set; }
            public virtual string END_MUNICIPIO { get; set; }
            public virtual string END_PAIS { get; set; }
            public virtual string CEP { get; set; }
            public virtual string FONE { get; set; }
            public virtual string RG_NUM { get; set; }
            public virtual string RG_TIPO { get; set; }
            public virtual string RG_EMISSOR { get; set; }
            public virtual string RG_UF { get; set; }
            public virtual DateTime? RG_DTEXP { get; set; }
            public virtual string CPF { get; set; }
            public virtual string ALIST_NUM { get; set; }
            public virtual string ALIST_SERIE { get; set; }
            public virtual string ALIST_RM { get; set; }
            public virtual string ALIST_CSM { get; set; }
            public virtual DateTime? ALIST_DTEXP { get; set; }
            public virtual string CR_NUM { get; set; }
            public virtual string CR_CAT { get; set; }
            public virtual string CR_SERIE { get; set; }
            public virtual string CR_RM { get; set; }
            public virtual string CR_CSM { get; set; }
            public virtual DateTime? CR_DTEXP { get; set; }
            public virtual string TELEITOR_NUM { get; set; }
            public virtual string TELEITOR_ZONA { get; set; }
            public virtual string TELEITOR_SECAO { get; set; }
            public virtual DateTime? TELEITOR_DTEXP { get; set; }
            public virtual string CPROF_NUM { get; set; }
            public virtual string CPROF_SERIE { get; set; }
            public virtual string CPROF_UF { get; set; }
            public virtual DateTime? CPROF_DTEXP { get; set; }
            public virtual string E_MAIL { get; set; }
            public virtual string RESP_NOME_COMPL { get; set; }
            public virtual string RESP_FONE { get; set; }
            public virtual string RESP_CPF { get; set; }
            public virtual string CELULAR { get; set; }
            public virtual string E_MAIL_INTERNO { get; set; }
            public virtual string TELEITOR_MUN { get; set; }
            public virtual string CERT_NASC_NUM { get; set; }
            public virtual string CERT_NASC_FOLHA { get; set; }
            public virtual string CERT_NASC_LIVRO { get; set; }
            public virtual DateTime? CERT_NASC_EMISSAO { get; set; }
            public virtual string CERT_NASC_CARTORIO_UF { get; set; }
            public virtual string CERT_NASC_CARTORIO_EXPED { get; set; }
            public virtual string PASSAPORTE { get; set; }
            public virtual string ID_CENSO { get; set; }
            public virtual string TIPO_SANGUINEO { get; set; }
            public virtual string ETNIA { get; set; }
            public virtual string CREDO { get; set; }
            public virtual int? QT_FILHOS { get; set; }
            public virtual DateTime? STAMP_ATUALIZACAO { get; set; }
            public virtual string CERT_NUMERO_MATRICULA { get; set; }
            public virtual int? ID_CARTORIO { get; set; }
            public virtual char? MAE_FALECIDA { get; set; }
            public virtual char? PAI_FALECIDO { get; set; }
            public virtual string MAE_CPF { get; set; }
            public virtual string PAI_CPF { get; set; }
            public virtual string MAE_TELEFONE { get; set; }
            public virtual string RESPONSAVEL { get; set; }
            public virtual string PAI_TELEFONE { get; set; }
            public virtual int IDFUNCIONAL { get; set; }
            public virtual string PISPASEP { get; set; }
            public virtual string USUARIOID { get; set; }
            public virtual DateTime? DATACADASTRO { get; set; }
            public virtual DateTime? DATAALTERACAO { get; set; }
            public virtual int? NECESSIDADEESPECIALID { get; set; }
            public virtual int? ID_INSCRICAO { get; set; }
            public virtual string LATITUDE { get; set; }
            public virtual string LONGITUDE { get; set; }
            public virtual char? AREA_QUILOMBOS { get; set; }
            public virtual char? TERRA_INDIGENA { get; set; }
            public virtual char? AREA_ASSENTAMENTO { get; set; }
        }

        public virtual DTOLY_PESSOA Pessoa { get; set; }
    }
}
