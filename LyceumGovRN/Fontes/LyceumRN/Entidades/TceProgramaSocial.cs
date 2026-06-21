using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceProgramaSocial : IEntity
    {
        public int IdProgramaSocial { get; set; }

        public string Aluno { get; set; }

        public string Programa { get; set; }

        public bool Elegivel { get; set; }

        public bool Beneficiario { get; set; }

        public DateTime InicioVigencia { get; set; }

        public DateTime FimVigencia { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }
    }
}
