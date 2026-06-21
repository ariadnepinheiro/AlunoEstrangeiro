using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosAnaliseProtocolo
    {
        public int ProtocoloPrestacaoId { get; set; }

        public int Ano { get; set; }

        public int? Semestre { get; set; }

        public string Temporalidade { get; set; }

        public string UnidadeEnsinoId { get; set; }

        public string UnidadeEnsino { get; set; }

        public int RegionalId { get; set; }

        public string Regional { get; set; }

        public string Processo { get; set; }

        public int? NumeroFolhas { get; set; }

        public int TipoProtocoloId { get; set; }

        public string TipoProtocolo { get; set; }

        public int ProgramaProtocoloId { get; set; }

        public string ProgramaProtocolo { get; set; }

        public string Observacao { get; set; }

        public DateTime DataProcesso { get; set; }

        public string Cnpj { get; set; }
    }
}
