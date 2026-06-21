namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;

    public class TceCompartilhadaOferta : IEntity
    {
        public string Curso { get; set; }

        public DateTime? DtAlteracao { get; set; }

        public DateTime DtCadastro { get; set; }

        public int IdCompartilhada { get; set; }

        public int IdCompartilhadaOferta { get; set; }

        public string Matricula { get; set; }

        public string Turno { get; set; }
    }
}