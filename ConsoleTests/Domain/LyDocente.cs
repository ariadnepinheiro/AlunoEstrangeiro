namespace ConsoleTests.Domain
{
    using System;
    using Seeduc.Infra.Entities;

    public class LyDocente : IEntity
    {
        public string Agencia { get; set; }

        public decimal? AnoIngresso { get; set; }

        public string Bairro { get; set; }

        public int Banco { get; set; }

        public string Candidato { get; set; }

        public string Categoria { get; set; }

        public string Celular { get; set; }

        public string Cep { get; set; }

        public string Concurso { get; set; }

        public string ContaBanco { get; set; }

        public string ContratoTrabalho { get; set; }

        public string Cpf { get; set; }

        public DateTime? CprofDataexp { get; set; }

        public string CprofNum { get; set; }

        public string CprofSerie { get; set; }

        public string CprofUf { get; set; }

        public DateTime? DtAdmissao { get; set; }

        public DateTime? DtDemissao { get; set; }

        public DateTime? DtNasc { get; set; }

        public string EMail { get; set; }

        public string EndCompl { get; set; }

        public string EndNum { get; set; }

        public string Endereco { get; set; }

        public string EstCivil { get; set; }

        public string FecharTurmaInternet { get; set; }

        public string Fone { get; set; }

        public string Matricula { get; set; }

        public string Municipio { get; set; }

        public string NomeCompl { get; set; }

        public decimal NumFunc { get; set; }

        public decimal Pessoa { get; set; }

        public string Pispasep { get; set; }

        public DateTime? RgDtexp { get; set; }

        public string RgEmissor { get; set; }

        public string RgNum { get; set; }

        public string RgTipo { get; set; }

        public string RgUf { get; set; }

        public string SenhaAlterada { get; set; }

        public string Sexo { get; set; }

        public string TipoIngresso { get; set; }

        public string Winusuario { get; set; }
    }
}