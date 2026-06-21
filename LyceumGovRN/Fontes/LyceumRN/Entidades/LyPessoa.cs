namespace Techne.Lyceum.RN.Entidades
{
    using System;
    using Seeduc.Infra.Entities;

    public class LyPessoa : IEntity
    {
        public string Alist_csm { get; set; }

        public DateTime? Alist_dtexp { get; set; }

        public string Alist_num { get; set; }

        public string Alist_rm { get; set; }

        public string Alist_serie { get; set; }

        public string Bairro { get; set; }

        public string Celular { get; set; }

        public string Cep { get; set; }

        public string Etnia { get; set; }

        public string Cpf { get; set; }

        public DateTime? Cprof_dtexp { get; set; }

        public string Cprof_num { get; set; }

        public string Cprof_serie { get; set; }

        public string Cprof_uf { get; set; }

        public string Cr_cat { get; set; }

        public string Cr_csm { get; set; }

        public DateTime? Cr_dtexp { get; set; }

        public string Cr_num { get; set; }

        public string Cr_rm { get; set; }

        public string Cr_serie { get; set; }

        public DateTime? Dt_nasc { get; set; }

        public string E_mail { get; set; }

        public string E_mail_interno { get; set; }

        public string E_mail_google { get; set; }

        public string End_compl { get; set; }

        public string End_municipio { get; set; }

        public string End_num { get; set; }

        public string End_pais { get; set; }

        public string Endereco { get; set; }

        public string Est_civil { get; set; }

        public string Fone { get; set; }

        public string Id_censo { get; set; }

        public string Municipio_nasc { get; set; }

        public string Nacionalidade { get; set; }

        public int? NecessidadeEspecialId { get; set; }

        public string Nome_compl { get; set; }

        public string Nome_social { get; set; }

        public string Pais_nasc { get; set; }

        public Decimal Pessoa { get; set; }

        public DateTime? Rg_dtexp { get; set; }

        public string Rg_emissor { get; set; }

        public string Rg_num { get; set; }

        public string Rg_tipo { get; set; }

        public string Rg_uf { get; set; }

        public string Sexo { get; set; }

        public DateTime Stamp_atualizacao { get; set; }

        public DateTime? Teleitor_dtexp { get; set; }

        public string Teleitor_num { get; set; }

        public string Teleitor_secao { get; set; }

        public string Teleitor_zona { get; set; }

        public string Teleitor_mun { get; set; }

        public string Tipo_Sanguineo { get; set; }

        public string Credo { get; set; }

        public Decimal? QtFilhos { get; set; }

        public string PreNomeSocial { get; set; }

        public string CertNascNum { get; set; }

        public string CertNascFolha { get; set; }

        public string CertNascLivro { get; set; }

        public string CertNascCartorioExped { get; set; }

        public int? IdCartorio { get; set; }

        public string CertNumeroMatricula { get; set; }

        public DateTime? CertNascEmissao { get; set; }

        public string CertNascCartorioUf { get; set; }

        public string CodigoUf { get; set; }

        public string CodigoMunicipio { get; set; }

        public string NomePai { get; set; }

        public string NomeMae { get; set; }

        public string MaeFalecida { get; set; }

        public string PaiFalecido { get; set; }

        public string MaeCpf { get; set; }

        public string PaiCpf { get; set; }

        public string MaeTelefone { get; set; }

        public string PaiTelefone { get; set; }

        public string Responsavel { get; set; }

        public string RespNomeCompl { get; set; }

        public string RespFone { get; set; }

        public string RespCpf { get; set; }

        public string NomeMunicipio { get; set; }
               
        public int? IdFuncional { get; set; }

        public string UsuarioId { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }

        public string Pispasep { get; set; }

        public string Passaporte { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string AreaQuilombos { get; set; }

        public string AreaTradicional { get; set; }

        public string TerraIndigena { get; set; }

        public string AreaAssentamento { get; set; }
    }
}