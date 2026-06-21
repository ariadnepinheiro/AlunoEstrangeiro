using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosExterno
    {
        public int TipoExternoId { get; set; }

        public string TipoExterno { get; set; }

        public int UsuarioExternoId { get; set; }

        public bool Bloqueado { get; set; }

        public decimal PessoaId { get; set; }

        public string Nome { get; set; }

        public string Cpf { get; set; }

        public DateTime? DataNascimento { get; set; }

        public string Sexo { get; set; }

        public string EstadoCivl { get; set; }

        public string PaisEndereco { get; set; }

        public string Endereco { get; set; }

        public string Numero { get; set; }

        public string Complemento { get; set; }

        public string Bairro { get; set; }

        public string Cep { get; set; }

        public string Municipio { get; set; }

        public string Email { get; set; }

        public string EmailAlternativo { get; set; }

        public string Telefone { get; set; }

        public string Celular { get; set; }

        public bool Ativo { get; set; }

        public string UsuarioResponsavel { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }
    }
}
