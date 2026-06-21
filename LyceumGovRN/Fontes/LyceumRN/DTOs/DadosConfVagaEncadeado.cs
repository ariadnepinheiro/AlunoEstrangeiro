using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosConfVagaEncadeado
    {
        public int IdConfVaga { get; set; }

        public int IdAgendaConfTurnoVaga { get; set; }

        public string Curriculo { get; set; }

       public string Curso { get; set; }

        public string Turno { get; set; }

        public string Sala { get; set; }

        public string Censo { get; set; }

        public string Turma { get; set; }

        public int VagasNovas { get; set; }

        public int VagasContinuidade { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime DtAlteracao { get; set; }

        public int SalaCapacidade { get; set; }

        public int? Serie { get; set; }

        public bool Editavel { get; set; }

        public int Periodo { get; set; }

        public int Ano { get; set; }

        //TURMA FILHA - Turma que estava anteriormente na sala, será o proximo item da lista encadeada
        public string TurmaReferenciada { get; set; }        

        //Para controlar se o item da lista já passou pela validação
        public bool Validado { get; set; }

        //Para controlar se o item da lista já passou pelo salvar
        public bool Processado { get; set; }
    }
}
