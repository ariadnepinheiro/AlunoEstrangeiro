using System;

namespace Techne.Lyceum.RN.PrestacaoContas.DTO
{
    public class DadosEventoArquivo
    {
        public string Tabela { get; set; }

        public int ArquivoId { get; set; }

        public int EventoId { get; set; }

        public string ChaveArquivo { get; set; }

        public byte[] Arquivo { get; set; }

        public string TipoArquivo { get; set; }

        public string NomeArquivo { get; set; }

        public string Descricao { get; set; }

        public string Justificativa { get; set; }

        public string UsuarioId { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }
    }
}
