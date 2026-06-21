using System;
using Techne.Lyceum.RN;

namespace Techne.Lyceum.RN.Agenda.Entidades
{
    public class Evento
    {
        public int AgendaId { get; set; }

        public int EventoId { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        public int TipoEventoId { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }

        public string UsuarioId { get; set; }

    }

   
}