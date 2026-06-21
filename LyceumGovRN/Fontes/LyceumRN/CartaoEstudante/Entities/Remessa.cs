using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.CartaoEstudante.Entities
{
    public class Remessa
    {
        public string Serie { get; set; }

        public long IdSolicitacao { get; set; }

        public DateTime dataHoraFoto { get; set; }

        public string email { get; set; }

        public string login { get; set; }

        public string AssinaturaDigital { get; set; }

        public string Turma { get; set; }

        public string Turno { get; set; }

        public string alunoCpf { get; set; }

        public DateTime alunoDataNascimento { get; set; }

        public byte[] alunoFoto { get; set; }

        public string alunoNome { get; set; }

        public DateTime alunoRGDataExpedicao { get; set; }

        public string alunoRGOrgaoExpedicao { get; set; }

        public string alunoRg { get; set; }

        public string alunoRgUfExpedicao { get; set; }

        public string codigoCenso { get; set; }

        public string enderecoBairro { get; set; }

        public string enderecoCep { get; set; }

        public string enderecoComplemento { get; set; }

        public string enderecoMunicipioestado { get; set; }

        public string enderecoNomeLogradouro { get; set; }

        public string enderecoNumero { get; set; }

        public string enderecoTipoLogradouro { get; set; }

        public string gratuidade { get; set; }

        public string maeNome { get; set; }

        public string matricula{ get; set; }

        public string modalBARCAS { get; set; }

        public string modalMETRO { get; set; }

        public string modalONIBUS { get; set; }

        public string modalTREM { get; set; }

        public string paiNome { get; set; }

        public int tipoMovimentacao { get; set; }
    }
}
