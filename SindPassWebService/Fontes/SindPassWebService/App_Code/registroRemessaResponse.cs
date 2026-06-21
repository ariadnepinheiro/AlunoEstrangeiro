using System;
using System.Collections.Generic;

public class registroRemessaResponse
{
    public string nomeArquivo { get; set; }

    public int quantidadeRegistros { get; set; }

    public List<registro> registros { get; set; }

    public class registro
    {
        public string QHITurma { get; set; }

        public string QHITurno { get; set; }

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

        public string matriculaNova { get; set; }

        public string modalBARCAS { get; set; }

        public string modalMETRO { get; set; }

        public string modalONIBUS { get; set; }

        public string modalTREM { get; set; }

        public string paiNome { get; set; }

        public int tipoMovimentacao { get; set; }
    }
}