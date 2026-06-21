namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;

    public class TceVersao : IEntity
    {
        public DateTime DataVersao { get; set; }

        public string Descricao { get; set; }

        public int IdVersao { get; set; }

        public bool GestaoOnline { get; set; }

        public bool DocenteOnline { get; set; }

        public bool AlunoOnline { get; set; }

        public string Motivo { get; set; }

        public string Usuario { get; set; }

        public string Versao { get; set; }
    }
}