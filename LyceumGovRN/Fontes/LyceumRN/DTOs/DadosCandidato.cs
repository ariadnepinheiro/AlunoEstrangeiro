using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class DadosCandidato
    {
        public DadosCandidato()
        {
            DadosIrmao = new DadosIrmao();
        }

        public int InscricaoAlunoId { get; set; }

        public int Ano { get; set; } 

       // public int Periodo { get; set; } 

        public int? NumeroInscricao { get; set; } 

        public int PreCadastroAlunoId { get; set; }

        public decimal? Pessoa { get; set; }

        public string Aluno { get; set; }

        public string Nome { get; set; }

        public string RedeOrigem { get; set; }

        public string Email { get; set; }

        public string Celular { get; set; }

        public string FixoCelular { get; set; }

        public string ConfirmacaoEmail { get; set; }
        
        public string ConfirmacaoSenha { get; set; }

        public DateTime? DataNascimento { get; set; }

        public string Responsavel { get; set; }

        public string NomeMae { get; set; }

        public string MaeCpf { get; set; }

        public string NomePai { get; set; }

        public string PaiCpf { get; set; }

        public string ResponsavelNome { get; set; }

        public string ResponsavelFone { get; set; }

        public string ResponsavelCpf { get; set; }

        public string ResponsavelNumeroRG { get; set; } //Não tem equivalente na pessoa

        public string ResposanvelEmissorRG { get; set; } //Não tem equivalente na pessoa

        public string ResposanvelUFRG { get; set; } //Não tem equivalente na pessoa
        
        public string IrmaoMatricula { get; set; } 

        public int? IrmaoNumeroInscricao { get; set; } 

        public int? IrmaoIdInscricao { get; set; } 

        public DadosIrmao DadosIrmao { get; set; } 

        public string Cpf { get; set; }

        public string NumeroRG { get; set; } //RG_NUM e Rg_tipo == "RG"

        public string OrgaoRG { get; set; } //RG_EMISSOR

        public string UFRG { get; set; } //RG_UF

        public string Sexo { get; set; }

        public string EstadoCivil { get; set; }

        public string Nacionalidade { get; set; }

        public string MunicipioNascimento { get; set; }

        public string DescricaoMunicipioNascimento { get; set; }

        public string UfNascimento { get; set; }

        public bool ConfirmacaoEndereco { get; set; }

        public string Cep { get; set; }

        public string Endereco { get; set; }

        public string NumeroEndereco { get; set; }

        public string ComplementoEndereco { get; set; }

        public string MunicipioEndereco { get; set; }

        public string DescricaoMunicipioEndereco { get; set; }

        public string UfEndereco { get; set; }

        public string Bairro { get; set; }

        public string ModeloCertidao { get; set; }

        public string TipoCertidao { get; set; }

        public string MatriculaCertidao { get; set; }

        public string TermoCertidao { get; set; }

        public string FolhaCertidao { get; set; }

        public string LivroCertidao { get; set; }

        public bool DeclaroAusenciaMae { get; set; }

        public bool DeclaroAusenciaPai { get; set; }

        public int? NecessidadeEspecialId { get; set; }

        public string DescricaoNecessidadeEspecial { get; set; }
    }
}
