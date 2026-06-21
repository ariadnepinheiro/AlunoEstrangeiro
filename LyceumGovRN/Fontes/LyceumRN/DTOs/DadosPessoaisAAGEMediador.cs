using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosPessoaisAAGEMediador
    {
        public decimal DocenteId { get; set; }

        public string Matricula { get; set; }

        public string NomeCompleto { get; set; }

        public string Cpf { get; set; }

        public DateTime DataNascimento { get; set; }

        public string Sexo { get; set; }

        public string EstadoCivl { get; set; }

        public string Endereco { get; set; }

        public string Numero { get; set; }

        public string Complemento { get; set; }

        public string Bairro { get; set; }

        public string Cep { get; set; }

        public string Municipio { get; set; }

        public string Telefone { get; set; }
    }
}
