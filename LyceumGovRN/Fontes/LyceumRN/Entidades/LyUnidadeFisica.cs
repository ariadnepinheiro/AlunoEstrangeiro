using System;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Entidades
{
    public class LyUnidadeFisica : IEntity
    {
        public string UnidadeFis { get; set; }

        public string NomeComp { get; set; }

        public string Endereco { get; set; }

        public string EndNum { get; set; }

        public string EndCompl { get; set; }

        public string Bairro { get; set; }

        public string Municipio { get; set; }

        public string Cep { get; set; }

        public string Tipo { get; set; }

        public string CondicaoFunc { get; set; }

        public string SituacaoLegal { get; set; }

        public string FormaOcupacao { get; set; }

        public DateTime? StampAtualizacao { get; set; }

        public string TitularPredio { get; set; }

        public Decimal? AreaTerreno { get; set; }

        public string AcessoNecessidadeEspecial { get; set; }

        public string AcessoDificil { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string ImovelCompartilhado { get; set; }

        public Decimal? AreaTotalTerreno { get; set; }

        public Decimal? AreaTotalConstruida { get; set; }

        public string LocalFuncionamento { get; set; }

        public string Distrito { get; set; }

        public string Matricula { get; set; }

        public DateTime DtCadastro { get; set; }

        public string MunicipioNome { get; set; }
        
        public string AreaAssentamento { get; set; }

        public string Sustentavel { get; set; }

        public string Quilombo { get; set; }

        public string TerraIndigena { get; set; }

        public string ProgramaBrasilAlfabetizado { get; set; }

        public string FimDeSemanaComunidade { get; set; }
    }
}
