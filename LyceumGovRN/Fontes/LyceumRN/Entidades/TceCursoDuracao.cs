using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceCursoDuracao : IEntity
    {
        public int IdCursoDuracao { get; set; }

        public int Ano { get; set; }

        public string Turno { get; set; }

        public string Curso { get; set; }

        public int Duracao { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }
    }
}
