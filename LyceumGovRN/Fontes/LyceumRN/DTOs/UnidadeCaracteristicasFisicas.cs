using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class UnidadeCaracteristicasFisicas
    {
        public UnidadeCaracteristicasFisicas()
        {
            FormasAcessibilidade = new List<int>();
        }

        //UNIDADE_FIS (LY_UNIDADE_FISICA)
        public string UnidadeFisica { get; set; }

        //NOME_COMP e NOME_ABREV (LY_UNIDADE_FISICA)
        public string NomeUnidadeFisica { get; set; }

        //FORMA_OCUPACAO (LY_UNIDADE_FISICA)
        public string FormaOcupacaoLocalizacao { get; set; }

        //LOCAL_FUNCIONAMENTO (LY_UNIDADE_FISICA)
        public string LocalFuncionamento { get; set; }

        //AREA_ASSENTAMENTO (LY_UNIDADE_FISICA)
        public string AreaAssentamento { get; set; }

        //TERRA_INDIGENA (LY_UNIDADE_FISICA)
        public string TerraIndigena { get; set; }

        //AREA_QUILOMBOS (LY_UNIDADE_FISICA)
        public string AreaQuilombo { get; set; }

        //UNIDADE_SUSTENTAVEL (LY_UNIDADE_FISICA)
        public string UnidadeSustentavel { get; set; }

        //TIPO (LY_UNIDADE_FISICA)
        public string FormaOcupacaoTipo { get; set; } 

        //TITULAR_PREDIO (LY_UNIDADE_FISICA)
        public string OcupacaoFormal { get; set; }

        //IMOVEL_COMPARTILHADO_REDE (LY_UNIDADE_FISICA)
        public string ImovelCompartilhadoRede { get; set; }

        //CEP (LY_UNIDADE_ENSINO e LY_UNIDADE_FISICA)
        public string Cep { get; set; }

        //MUNICIPIO (LY_UNIDADE_ENSINO e LY_UNIDADE_FISICA)
        public string Municipio { get; set; }

        public string MunicipioDescricao { get; set; }

        public string UF { get; set; }

        //LATITUDE (LY_UNIDADE_FISICA)
        public string Latitude { get; set; }

        //LONGITUDE (LY_UNIDADE_FISICA)
        public string Longitude { get; set; }

        //ENDERECO (LY_UNIDADE_ENSINO e LY_UNIDADE_FISICA)
        public string Endereco { get; set; }

        //END_NUM (LY_UNIDADE_ENSINO e LY_UNIDADE_FISICA)
        public string EnderecoNumero { get; set; }

        //END_COMPL (LY_UNIDADE_ENSINO e LY_UNIDADE_FISICA)
        public string EnderecoComplemento { get; set; }

        //BAIRRO (LY_UNIDADE_ENSINO e LY_UNIDADE_FISICA)
        public string EnderecoBairro { get; set; }

        //DESCRIÇÃO BAIRRO (HADES.bdo.BAIRRO)
        public string EnderecoDescricaoBairro { get; set; }

        //DISTRITO (LY_UNIDADE_FISICA)
        public string Distrito { get; set; }

        //ACESSO_NECESSIDADE_ESPECIAL (LY_UNIDADE_FISICA)
        public string AcessoNecessidadeEspecial { get; set; }

        //ACESSO_DIFICIL (LY_UNIDADE_FISICA)
        public string AcessoDificil { get; set; }

        //AREA_TOTAL_TERRENO (LY_UNIDADE_FISICA)
        public decimal? AreaTotalTerreno { get; set; }

        //AREA_TOTAL_CONSTRUIDA (LY_UNIDADE_FISICA)
        public decimal? AreaTotalConstruida { get; set; }

        //AREA_TERRENO (LY_UNIDADE_FISICA)
        public decimal? AreaTerreno { get; set; }

        //campos que erão da aba Informações Gerais

        //DEPENDENCIA_ADM (LY_UNIDADE_ENSINO)
        public string DependenciaAdministrativa { get; set; }

        //E_MAIL (LY_UNIDADE_ENSINO e LY_UNIDADE_FISICA)
        public string Email { get; set; }

        //CGC (LY_UNIDADE_ENSINO e LY_UNIDADE_FISICA)
        public string Cnpj { get; set; }

        //EXTRACLASSE (LY_UNIDADE_ENSINO)
        public string Extraclasse { get; set; }

        //FONE (LY_UNIDADE_ENSINO e LY_UNIDADE_FISICA)
        public string Telefone1 { get; set; }

        //TEL2 (LY_UNIDADE_ENSINO)
        public string Telefone2 { get; set; }

        //FAX (LY_UNIDADE_ENSINO e LY_UNIDADE_FISICA)
        public string Fax { get; set; }

        //MATRICULA (LY_UNIDADE_ENSINO e LY_UNIDADE_FISICA)
        public string UsuarioResponsavel { get; set; }

        //IDs DA TABELA FORMASACESSIBILIDADE (UNIDADEFISICA_FORMASACESSIBILIDADE)
        public List<int> FormasAcessibilidade { get; set; }
        
        //SALACLIMATIZADA (LY_UNIDADE_FISICA)
        public int? SalaClimatizada { get; set; }

        //SALAACESSIBILIDADE (LY_UNIDADE_FISICA)
        public int? SalaAcessibilidade { get; set; }

        //SALACANTINHOLEITURA (LY_UNIDADE_FISICA)
        public int? SalaCantinhoLeitura { get; set; }

        //REGIONAL (LY_UNIDADE_ENSINO )
        public string Regional { get; set; }
    }
}
