using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.RecursosHumanos.DTO
{
    public class DadosCadastraisAluno
    {
        public Decimal Pessoa { get; set; }

        public string Nome_compl { get; set; }

        public string PreNomeSocial { get; set; }

        public DateTime? Dt_nasc { get; set; }

        public string Sexo { get; set; }

        public Decimal? QtFilhos { get; set; }

        public string Etnia { get; set; }

        public string Est_civil { get; set; }

        public string Pais_nasc { get; set; }

        public string Pais_nasc_nome { get; set; }

        public string Nacionalidade { get; set; }

        public string Municipio_nasc { get; set; }

        public string Municipio_nasc_nome { get; set; }

        public string UF_nasc { get; set; } 

        public string NomeMae { get; set; }

        public bool DeclaroAusenciaMae { get; set; }

        public string MaeFalecida { get; set; }

        public string MaeCpf { get; set; }

        public string MaeTelefone { get; set; }

        public string NomePai { get; set; }

        public bool DeclaroAusenciaPai { get; set; }

        public string PaiFalecido { get; set; }

        public string PaiCpf { get; set; }

        public string PaiTelefone { get; set; }

        public string Responsavel { get; set; }

        public string RespNomeCompl { get; set; }

        public string RespCpf { get; set; }

        public string RespFone { get; set; }

        public string Cep { get; set; }

        public string End_municipio { get; set; }

        public string End_NomeMunicipio { get; set; }

        public string End_UF { get; set; }

        public string Endereco { get; set; }

        public string End_num { get; set; }

        public string End_compl { get; set; }

        public string Bairro { get; set; }

        public string ZonaResidencial { get; set; } //FL_FIELD_01

        public string AreaQuilombos { get; set; }

        public string AreaTradicional { get; set; }

        public string TerraIndigena { get; set; }

        public string AreaAssentamento { get; set; }

        public string Fone { get; set; }

        public string Celular { get; set; }

        public string E_mail { get; set; }

        public string Cpf { get; set; }

        public string Rg_tipo { get; set; }

        public string Rg_num { get; set; }

        public string ComplementoIdentidade { get; set; } //FL_FIELD_07

        public string Rg_uf { get; set; }

        public string Rg_emissor { get; set; }

        public DateTime? Rg_dtexp { get; set; }

        public string TipoCertidao { get; set; } //FL_FIELD_02

        public bool DeclaroCertidaoCivil { get; set; }

        public string CertidaoCivil { get; set; } //FL_FIELD_09

        public string UfCartorio { get; set; }

        public string UfCartorioNome { get; set; }
        
        public string MunicipioCartorio { get; set; }

        public string MunicipioCartorioNome { get; set; } 

        public int? IdCartorio { get; set; }

        public string NomeCartorio { get; set; }

        public string CertNascNum { get; set; }

        public string CertNascFolha { get; set; }

        public string CertNascLivro { get; set; }

        public DateTime? CertNascEmissao { get; set; }

        public string CertNascCartorioUf { get; set; }

        public string CertNumeroMatricula { get; set; }

        public string UsuarioId { get; set; }
    }
}