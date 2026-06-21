using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosAlunoNotificacao
    {
        public decimal Pessoa { get; set; }

        public string Aluno { get; set; }

        public string Nome { get; set; }

        public string NomeSocial { get; set; }

        public DateTime DataNascimento { get; set; }

        public int Idade { get; set; }

        public string NomeMae { get; set; }

        public bool MaeNaoDeclarada { get; set; }

        public bool MaeFalecida { get; set; }

        public string MaeCpf { get; set; }

        public string MaeTelefone { get; set; }

        public string NomePai { get; set; }

        public bool PaiNaoDeclarado { get; set; }

        public bool PaiFalecido { get; set; }

        public string PaiCpf { get; set; }

        public string PaiTelefone { get; set; }

        public string ResponsavelLegal { get; set; }

        public string ResponsavelNome { get; set; }

        public string ResponsavelCpf { get; set; }

        public string ResponsavelTelefone { get; set; }

        public string Cep { get; set; }

        public string Municipio { get; set; }

        public string CodMunicipio { get; set; }

        public string Estado { get; set; }

        public string Endereco { get; set; }

        public string Numero { get; set; }

        public string Complemento { get; set; }

        public string Bairro { get; set; }

        public string Regional { get; set; }

        public string Censo { get; set; }

        public string Nível { get; set; }

        public string Modalidade { get; set; }

        public string Curso { get; set; }

        public string Turno { get; set; }

        public string Serie { get; set; }

        public string Curriculo { get; set; }

        public string Situacao { get; set; }       
    }
}
