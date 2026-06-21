using System;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Entidades
{
    public class LyUnidadeEnsino : IEntity
    {
        public string UnidadeEns { get; set; }

        public string NomeComp { get; set; }

        public string NomeAbrev { get; set; }

        public string Endereco { get; set; }

        public string EndNum { get; set; }

        public string EndCompl { get; set; }

        public string Bairro { get; set; }

        public string Municipio { get; set; }

        public string Cep { get; set; }

        public string CaixaPostal { get; set; }

        public string Fone { get; set; }

        public string Fax { get; set; }

        public string Cgc { get; set; }

        public string EMail { get; set; }

        public string Turmapref { get; set; }

        public string Ccm { get; set; }

        public string Mnemonico { get; set; }

        public string OutraFaculdade { get; set; }

        public decimal? Banco { get; set; }

        public string Agencia { get; set; }

        public string ContaBanco { get; set; }

        public string Titular { get; set; }

        public string WebSite { get; set; }

        public string InepFaculdade { get; set; }

        public string InscrEstadual { get; set; }

        public string Marca { get; set; }

        public string Grupo { get; set; }

        public DateTime? StampAtualizacao { get; set; }

        public string Nucleo { get; set; }

        public string Setor { get; set; }

        public string Tipo { get; set; }

        public string DependenciaAdm { get; set; }

        public string Classificacao { get; set; }

        public string Extraclasse { get; set; }

        public string EscolaAberta { get; set; }

        public string SitFuncionamento { get; set; }

        public int? IdRegional { get; set; }

        public string Tel2 { get; set; }

        public string Matricula { get; set; }

        public DateTime? DtCadastro { get; set; }
    }
}
