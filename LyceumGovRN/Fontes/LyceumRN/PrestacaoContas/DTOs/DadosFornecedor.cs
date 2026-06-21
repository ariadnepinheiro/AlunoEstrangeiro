using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.PrestacaoContas.DTOs
{
    public class DadosFornecedor
    {
        public int? FornecedorId { get; set; }

        public string Situacao { get; set; }

        public int? FornecedorRazaoSocialId { get; set; }

        public string Tipo { get; set; }

        public bool GrandePorte { get; set; }

        public bool Eventual { get; set; }

        public string Cnpj { get; set; }

        public string RazaoSocial { get; set; }

        public DateTime? DataInicio { get; set; }

        public DateTime? DataFim { get; set; }

        public string InscricaoEstadual { get; set; }

        public string InscricaoMunicipal { get; set; }

        public string Endereco { get; set; }

        public string Numero { get; set; }

        public string Complemento { get; set; }

        public string Bairro { get; set; }

        public string MunicipioId { get; set; }

        public string MunicipioDescricao { get; set; }

        public string Uf { get; set; }

        public string Cep { get; set; }

        public string Email { get; set; }

        public string Telefone { get; set; }

        public bool Enviado { get; set; }

        public bool? Finalizado { get; set; }

        public string UsuarioId { get; set; }

        public DateTime? DataCadastro { get; set; }

        public DateTime? DataAlteracao { get; set; }
    }
}