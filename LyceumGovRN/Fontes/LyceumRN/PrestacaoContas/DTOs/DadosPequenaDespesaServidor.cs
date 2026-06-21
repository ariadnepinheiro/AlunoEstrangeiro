using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.PrestacaoContas.DTOs
{
    [Serializable]
    public class DadosPequenaDespesaServidor
    {
        public int PequenaDespesaServidorId { get; set; }
        public int PequenaDespesaId { get; set; }
        public int Pessoa { get; set; }
        public string NomeCompl { get; set; }
        public int IdFuncional { get; set; }
        public string Matricula { get; set; }
        public string UsuarioId { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAlteracao { get; set; }
    }
}
