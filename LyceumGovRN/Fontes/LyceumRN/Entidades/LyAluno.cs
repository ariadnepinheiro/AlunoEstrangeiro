namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

    public class LyAluno : IEntity
    {
        public string Aluno { get; set; }
        public Decimal? Concurso { get; set; }
        public Decimal? Candidato { get; set; }
        public string Curso { get; set; }
        public string Turno { get; set; }
        public string Curriculo { get; set; }
        public Decimal Serie { get; set; }     
        public string TipoIngresso { get; set; }
        public Decimal AnoIngresso { get; set; }
        public Decimal SemIngresso { get; set; }
        public string SitAluno { get; set; }     
        public string EMailInterno { get; set; }      
        public string UnidadeFisica { get; set; }       
        public Decimal? Pessoa { get; set; }
        public string OutraFaculdade { get; set; }       
        public DateTime StampAtualizacao { get; set; }
        public string UnidadeEnsino { get; set; }       
        public string Numinscricao { get; set; }
        public string TipoEnsinoProfissionalizante { get; set; }
        public string RedeEnsinoOrigem { get; set; }
        public int? TempoAfastamentoRede  { get; set; }
        public DateTime DtCadastro { get; set; }
        public string Usuario { get; set; }
        public DateTime DataAlteracao { get; set; }

       //Necessidade Tecnologia Assistida
        public string Ano_Educ { get; set; }
        public string Periodo_Educ { get; set; }
        public string DtOferta_Educ { get; set; }
        public string Nec_Tec_Assistida { get; set; }
        public string Aceite_Educ { get; set; }
        public string Censo_Educ { get; set; }
        public string Observacao_Educ { get; set; }

        [AtributoCampo(Nome = "DATAATUALIZACAOEMAILINTERNO")]
        public DateTime? DataAtualizacaoEmailInterno { get; set; }

        //Campos auxiliares para tela de aluno
        public string EMailAnterior { get; set; }
        public string EMailConfirmacao { get; set; }
        public bool DeclaroNecessidadeEspecial { get; set; }      
        public bool DeclaroAusenciaMae { get; set; }       
        public bool DeclaroAusenciaPai { get; set; }
        public bool DeclaroCertidaoCivil { get; set; }
        public string MunicipioEscola { get; set; }
        public bool NenhumRecursoAplicacaoProva { get; set; }

        public bool Suspenso { get; set; }
        public DateTime? DataEmSuspensao { get; set; }

      }
}
