using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.Cadastros.DTOs
{
    public class MaeDadosBancarios
    {
        public int MaeInscricaoId { get; set; }

        public int MaeLotacaoId { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        public string Banco { get; set; }

        public string Agencia { get; set; }

        public string ContaCorrente { get; set; }

        public string UsuarioId { get; set; }

        public string Censo { get; set; }

        public int? MaeMotivoDesligamentoId { get; set; }

        public string DescricaoOutros { get; set; }
    }
}
