using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosFichaAluno
    {
        #region Dados Pessoais
        public int Pessoa { get; set; }

        public LyFotoPessoa Foto { get; set; }

        public string AlunoMatricula { get; set; }

        public string NomeAluno { get; set; }

        public DateTime DataNascimento { get; set; }

        public string Sexo { get; set; }

        public int QuantidadeFilhos { get; set; }

        public string TipoSanguineo { get; set; }

        public string Etnia { get; set; }

        public string EstadoCivil { get; set; }

        public string PaisNascimento { get; set; }

        public string Nacionalidade { get; set; }

        public string UfNascimento { get; set; }

        public string Naturalidade { get; set; }

        public string Credo { get; set; }

        public string DescendenciaFamiliar { get; set; }

        public string NecessidadeEspecial { get; set; }

        public DateTime? DataSituacao { get; set; }
        #endregion

        #region Filiacao
        public string NomeMae { get; set; }

        public string FalecidaMae { get; set; }

        public string CPFMae { get; set; }

        public string NomePai { get; set; }

        public string FalecidoPai { get; set; }

        public string CPFPai { get; set; }

        public string ResponsavelLegal { get; set; }

        public string NomeOutros { get; set; }

        public string CpfOutros { get; set; }

        public string TelMae { get; set; }

        public string TelPai { get; set; }

        public string TelResponsavel { get; set; }
        #endregion

        #region Endereco
        public string Endereco { get; set; }

        public string NumeroEndereco { get; set; }

        public string ComplementoEndereco { get; set; }

        public string BairroEndereco { get; set; }

        public string MunicipioEndereco { get; set; }

        public string EstadoEndereco { get; set; }

        public string CepEndereco { get; set; }

        public string LocalizacaoEndereco { get; set; }
        #endregion

        #region Contato
        public string Telefone { get; set; }

        public string Celular { get; set; }

        public string Email { get; set; }
        #endregion

        #region Documento
        public string Cpf { get; set; }

        public string TipoDocumento { get; set; }

        public string NumeroDocumento { get; set; }

        public string ComplementoIdentidade { get; set; }

        public string EstadoDocumento { get; set; }

        public string OrgaoEmissorDocumento { get; set; }

        public DateTime? DataExpedicaoDocumento { get; set; }
        #endregion

        #region Outras Informacoes
        public string Inep { get; set; }

        public string Nis { get; set; }
        #endregion

        #region Certidao Civil
        public string TipoCertidao { get; set; }

        public string CertidaoCivil { get; set; }

        public string UfCartorio { get; set; }

        public string MunicipioCartorio { get; set; }

        public string NomeCartorio { get; set; }

        public string Livro { get; set; }

        public string Folha { get; set; }

        public string NumeroTermo { get; set; }

        public DateTime? DataEmissaoCertidao { get; set; }

        public string MatriculaCertidao { get; set; }
        #endregion
    }
}
