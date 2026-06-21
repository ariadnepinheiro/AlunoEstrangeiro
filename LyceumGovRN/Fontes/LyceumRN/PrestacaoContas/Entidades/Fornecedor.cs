using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.FORNECEDOR", Nome = "PrestacaoContas.FORNECEDOR")]
    public class Fornecedor : IEntity
    {
        [AtributoCampo(Nome = "FORNECEDORID")]
        public int FornecedorId { get; set; }

        [AtributoCampo(Nome = "TIPO")]
        public string Tipo { get; set; }  

        [AtributoCampo(Nome = "CNPJ")]
        public string Cnpj { get; set; }       

        [AtributoCampo(Nome = "INSCRICAOESTADUAL")]
        public string InscricaoEstadual { get; set; }

        [AtributoCampo(Nome = "INSCRICAOMUNICIPAL")]
        public string InscricaoMunicipal { get; set; }

        [AtributoCampo(Nome = "ENDERECO")]
        public string Endereco { get; set; }

        [AtributoCampo(Nome = "NUMERO")]
        public string Numero { get; set; }

        [AtributoCampo(Nome = "COMPLEMENTO")]
        public string Complemento { get; set; }

        [AtributoCampo(Nome = "BAIRRO")]
        public string Bairro { get; set; } 

        [AtributoCampo(Nome = "MUNICIPIOID")]
        public string MunicipioId { get; set; }

        [AtributoCampo(Nome = "CEP")]
        public string Cep { get; set; }

        [AtributoCampo(Nome = "EMAIL")]
        public string Email { get; set; }

        [AtributoCampo(Nome = "TELEFONE")]
        public string Telefone { get; set; }

        [AtributoCampo(Nome = "ENVIADO")]
        public bool Enviado { get; set; }

        [AtributoCampo(Nome = "FINALIZADO")]
        public bool? Finalizado { get; set; }

        [AtributoCampo(Nome = "GRANDEPORTE")]
        public bool GrandePorte { get; set; }

        [AtributoCampo(Nome = "EVENTUAL")]
        public bool Eventual { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
