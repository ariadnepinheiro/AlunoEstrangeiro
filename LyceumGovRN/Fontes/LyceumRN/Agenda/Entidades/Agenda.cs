using System;

namespace Techne.Lyceum.RN.Agenda.Entidades
{
    [Serializable]
    public class Agenda
    {
        public int AgendaId { get; set; }

        public string Descricao { get; set; }

        public string Observacao { get; set; }

        public int ParticipaUnidadeId { get; set; }

        public int ParticipaCursoId { get; set; }
        
        public bool CursoPorUnidade { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }

        public string UsuarioId { get; set; }

    }
}
