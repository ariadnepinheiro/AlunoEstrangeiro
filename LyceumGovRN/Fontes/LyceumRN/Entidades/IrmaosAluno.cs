using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;


namespace Techne.Lyceum.RN.Entidades
{
    public class IrmaosAluno: IEntity
    {
        public string Tipo { get; set; }
        public string Aluno { get; set; }
        public string Nome { get; set; }
        public string NomeMae { get; set; }
        public string NomePai { get; set; }
        public DateTime DtNascimento { get; set; }
        public string CertNascimento { get; set; }
        public string Termo { get; set; }
        public string Escola { get; set; }
    }
}
