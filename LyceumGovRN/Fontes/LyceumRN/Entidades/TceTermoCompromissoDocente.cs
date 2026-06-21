using System;

namespace Techne.Lyceum.RN.Entidades
{
    public class TceTermoCompromissoDocente
    {
        public int IdTermoDocente { get; set; }

        public int Ano { get; set; }

        public Date DtInicio { get; set; }

        public Date DtFim { get; set; }

        public string Arquivo { get; set; }

        public DateTime DtCadastro { get; set; }

        public DateTime DtAlteracao { get; set; }

        public string Matricula { get; set; }
    }
}
