namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;
    using Seeduc.Infra.MapeamentoAtributos;

    public class LyDocente : IEntity
    {       
        public decimal? Ano_ingresso { get; set; }

        public DateTime? Dt_admissao { get; set; }

        public DateTime? Dt_demissao { get; set; }

        public string Candidato { get; set; }

        public string Categoria { get; set; }

        public string Concurso { get; set; }

        public string Matricula { get; set; }

        public decimal Num_func { get; set; }

        public decimal Pessoa { get; set; }

        public string Senha_alterada { get; set; }

        public string Voluntario { get; set; }

        [AtributoCampo(Nome = "REGIMECONTRATACAOID")]
        public int? RegimeContratacaoId { get; set; }

        public string Senha_dol { get; set; }

        //Carga Horaria
        public string Regime_trabalho { get; set; }

        public int Acumulacao { get; set; }

        public int? Vinculo { get; set; }

        public string Usuario { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }


    }
}