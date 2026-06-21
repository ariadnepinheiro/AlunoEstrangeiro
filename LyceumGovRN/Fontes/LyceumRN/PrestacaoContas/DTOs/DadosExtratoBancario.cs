using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.PrestacaoContas.DTOs
{
    public class DadosExtratoBancario
    {
        public int ExtratoBancarioId { get; set; }
        public int PeriodoReferenciaExtratoBancarioId { get; set; }
        public string Censo { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
        public string Observacao { get; set; }
        public int? Status { get; set; }
        public int ExtratoBancarioArquivoId { get; set; }
        public string ChaveArquivo { get; set; }
        public byte[] Arquivo { get; set; }
        public string TipoArquivo { get; set; }
        public string NomeArquivo { get; set; }
        public string UsuarioId { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAlteracao { get; set; }
    }
}
