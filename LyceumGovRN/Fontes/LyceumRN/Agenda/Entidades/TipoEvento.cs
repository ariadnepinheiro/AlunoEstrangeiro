using System;

namespace Techne.Lyceum.RN.Agenda.Entidades
{
    public class TipoEvento
    {

        public int TipoEventoId { get; set; }

        public int Sistemico { get; set; }

        public int Ativo { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }

        public string UsuarioId { get; set; }

        public string Nome { get; set; }

    }
}