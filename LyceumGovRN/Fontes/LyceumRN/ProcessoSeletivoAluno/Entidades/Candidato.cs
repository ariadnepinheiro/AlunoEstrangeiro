using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.ProcessoSeletivoAluno.Entidades
{
    public class Candidato
    {
        public string Candidatoid { get; set; }
        public string NomeCompleto { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string MunicipioNascimento { get; set; }
        public string PaisNascimento { get; set; }
        public string Nacionalidade { get; set; }
        public string NomePai { get; set; }
        public string PaiCPF { get; set; }
        public string PaiFalecido { get; set; }
        public string PaiTelefone { get; set; }
        public string NomeMãe { get; set; }
        public string MaeCPF { get; set; }
        public string MaeFalecida { get; set; }
        public string RedeEnsino { get; set; }
        public string MaeTelefone { get; set; }
        public string Sexo { get; set; }
        public string EstadoCivil { get; set; }
        public string Etnia { get; set; }
        public string Credo { get; set; }
        public string NecessidadeEspecial { get; set; }
        public string Endereco { get; set; }
        public string EnderecoNumero { get; set; }
        public string EnderecoCompleto { get; set; }
        public string EnderecoBairro { get; set; }
        public string EnderecoMunicipio { get; set; }
        public string EnderecoPais { get; set; }
        public string EnderecoCep { get; set; }
        public string LocalizacaoZonaResidencia { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string RGTipo { get; set; }
        public string RGNumero { get; set; }
        public string RGEmissor { get; set; }
        public string RGUF { get; set; }
        public DateTime? RGDataExpedida { get; set; }
        public string RGComplemento { get; set; }
        public string CPF { get; set; }
        public string TipoCertidao { get; set; }
        public string CertidaoNumero { get; set; }
        public string CertidaoFolha { get; set; }
        public string CertidaoLivro { get; set; }
        public DateTime? CertidaoDataEmissao { get; set; }
        public string CertidaoCartorioUF { get; set; }
        public string CertidaoCartorioExpedicao { get; set; }
        public string CertidaoNumeroMatricula { get; set; }
        public int CartorioID { get; set; }
        public string ModeloCertidaoCivil { get; set; }
        public string Responsavel { get; set; }
        public string ResponsavelNome { get; set; }
        public string ResponsavelTelefone { get; set; }
        public string ResponsavelCPF { get; set; }
        public string Situacao { get; set; }
        public string TipoBolsaParticular { get; set; }
        public string AlunoID { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAlteracao { get; set; }
        public string IP { get; set; }
    }
}
