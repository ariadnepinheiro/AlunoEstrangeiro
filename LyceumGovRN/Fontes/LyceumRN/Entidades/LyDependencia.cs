using System;

namespace Techne.Lyceum.RN.Entidades
{
    using Seeduc.Infra.Entities;

    public class LyDependencia : IEntity
    {
        public string Faculdade { get; set; }

        public string Dependencia { get; set; }

        public string TipoDepend { get; set; }

        public string Ativa { get; set; }

        public decimal? NumAlunos { get; set; }

        public string Descricao { get; set; }

        public string Obs { get; set; }

        public string Edificacao { get; set; }

        public string Pavimento { get; set; }

        public double? Area { get; set; }

        public string CadSalaAula { get; set; }

        public string SalaAnexa { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime DtAlteracao { get; set; }
    }
}