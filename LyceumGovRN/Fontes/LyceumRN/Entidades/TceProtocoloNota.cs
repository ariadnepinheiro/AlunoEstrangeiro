namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;

    public class TceProtocoloNota : IEntity
    {
        public int Ano { get; set; }

        public string Codigo
        {
            get
            {
                if (this.IdProtocoloNota == 0
                    || string.IsNullOrEmpty(this.Tipo))
                {
                    return string.Empty;
                }

                return string.Format(
                    "{0}{1}{2}{3}{4}/{5}{6}",
                    this.Ano,
                    this.Periodo,
                    this.Turma,
                    this.Disciplina,
                    this.Matricula,
                    this.Tipo,
                    this.IdProtocoloNota.ToString().PadLeft(9, '0'));
            }
        }

        public string Disciplina { get; set; }

        public DateTime DtCadastro { get; set; }

        public int IdProtocoloNota { get; set; }

        public string Matricula { get; set; }

        public string NomeDisciplina { get; set; }

        public int Periodo { get; set; }

        public int Subperiodo { get; set; }

        public string Tipo { get; set; }

        public string Turma { get; set; }
    }
}