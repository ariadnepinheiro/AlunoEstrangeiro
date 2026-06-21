using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosVoluntario
    {
        public int RecursoNecessidadeEspecialId { get; set; }

        public bool Bloqueado { get; set; }

        public decimal PessoaId { get; set; }

        public string Nome { get; set; }

        public string NomeSocial { get; set; }

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

        public string Telefone { get; set; }

        public string Celular { get; set; }

        //Pessoa.Id_censo
        public string IdInep { get; set; }

        //Pessoa.Cor_raca
        public string CorRaca { get; set; }

        //Pessoa.Pais_nasc
        public string PaisNascimento { get; set; }

        //Pessoa.Municipio_nasc
        public string MunicipioNascimento { get; set; }

        //Nacionalidade
        public string Nacionalidade { get; set; }

        //Necessidade_especial  
        public string NecessidadeEspecial { get; set; }

        //Rg_tipo
        public string RgTipo { get; set; }

        //Rg_num
        public string RgNumero { get; set; }

        //Rg_dtexp
        public DateTime? RgDataExpedicao { get; set; }

        //Rg_uf
        public string RgUf { get; set; }

        //Rg_emissor
        public string RgEmissor { get; set; }

        public string UsuarioId { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }
    }
}
