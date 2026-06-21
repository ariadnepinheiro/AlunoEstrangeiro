using System;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceAlunoEducEspecial : IEntity
    {
        public int IdAlunoEducEspecial { get; set; }

        public string Aluno { get; set; }

        public int Ano { get; set; }

        public int Periodo { get; set; }

        public DateTime DtOferta { get; set; }

        public bool NecTecAssistida { get; set; }

        public bool Aceite { get; set; }

        public string Censo { get; set; }

        public string Observacao { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime? DtAlteracao { get; set; }

        public string Matricula { get; set; }

        public bool Cuidador { get; set; }

        [AtributoCampo(Nome = "INTERPRETELIBRAS")]
        public bool InterpreteLibras { get; set; }

        public bool Ledor { get; set; }

        [AtributoCampo(Nome = "TRANSPORTEADAPTADO")]
        public bool TransporteAdaptado { get; set; }        
    }
}
