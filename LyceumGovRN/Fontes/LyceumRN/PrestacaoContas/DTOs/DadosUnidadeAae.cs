using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.PrestacaoContas.DTOs
{
    public class DadosUnidadeAae
    {
        public int IdRegional { get; set; }

        public string Regional { get; set; }

        public string Endereco { get; set; }

        public string Numero { get; set; }

        public string Complemento { get; set; }

        public string Bairro { get; set; }

        public string Municipio { get; set; }

        public string Telefone { get; set; }

        public string Email { get; set; }

        public string Diretor { get; set; }

        public string Censo { get; set; }

        public string NumeroAluno { get; set; }

        public string PresidenteNome { get; set; }

        public string PresidenteRg { get; set; }

        public string PresidenteCpf { get; set; }

        public string PresidenteEndereco { get; set; }

        public string PresidenteNumero { get; set; }

        public string PresidenteComplemento { get; set; }

        public string PresidenteBairro { get; set; }

        public string PresidenteMunicipio { get; set; }

        public string PresidenteEmail { get; set; }

        public string PresidenteTelefone { get; set; }

        public string PresidenteIdFuncional { get; set; }

        public string PresidenteMatricula { get; set; }

        public bool TesoureiroPossuiIdFuncional { get; set; }

        public decimal? TesoureiroPessoa { get; set; }

        public int? TesoureiroId { get; set; }

        public string TesoureiroNome { get; set; }

        public string TesoureiroRg { get; set; }

        public string TesoureiroCpf { get; set; }

        public string TesoureiroEndereco { get; set; }

        public string TesoureiroNumero { get; set; }

        public string TesoureiroComplemento { get; set; }

        public string TesoureiroBairro { get; set; }

        public string TesoureiroMunicipio { get; set; }

        public string TesoureiroEmail { get; set; }

        public string TesoureiroTelefone { get; set; }

        public string TesoureiroIdFuncional { get; set; }

        public int Mandato { get; set; }

        public int MandatoAaeId { get; set; }

        public DateTime InicioMandato { get; set; }

        public DateTime FimMandato { get; set; }

        public int? AtaMandatoArquivoId { get; set; }

        public String TipoArquivo { get; set; }

        public String NomeArquivo { get; set; }

        public string Banco { get; set; }

        public string Agencia { get; set; }

        public string ContaCorrente { get; set; }

        public bool Impedida { get; set; }

        public string MotivoImpedimento { get; set; }
    }
}
