using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Techne.Lyceum.RN.DTOs
{
    public class UnidadeInformacoesGerais
    {
        //UNIDADE_ENS (LY_UNIDADE_ENSINO, LY_UNIDADES_ASSOCIADAS) e UNIDADE_FIS (LY_UNIDADE_FISICA, LY_UNIDADES_ASSOCIADAS)
        public string Censo { get; set; }

        //SIT_FUNCIONAMENTO (LY_UNIDADE_ENSINO)
        public string SituacaoFuncionamento { get; set; }

        //NOME_COMP e NOME_ABREV (LY_UNIDADE_ENSINO e LY_UNIDADE_FISICA)
        public string NomeUnidade { get; set; }

        //ID_REGIONAL (LY_UNIDADE_ENSINO)
        public int RegionalId { get; set; }

        // (TCE_REGIONAL)
        public string NomeRegional { get; set; }

        //NUCLEO (LY_UNIDADE_ENSINO)
        public string Coordenadoria { get; set; }

        // (LY_NUCLEO)
        public string NomeCoordenadoria { get; set; }

        //SETOR (LY_UNIDADE_ENSINO)
        public string UnidadeAdministrativa { get; set; }

        //CLASSIFICACAO (LY_UNIDADE_ENSINO)
        public string Classificacao { get; set; }

        //IMOVEL_COMPARTILHADO (LY_UNIDADE_FISICA)
        public string ImovelCompartilhado { get; set; }

        //MATRICULA (LY_UNIDADE_ENSINO e LY_UNIDADE_FISICA)
        public string UsuarioResponsavel { get; set; }

        //Campos que serão da aba Características Físicas / Localização

        //LATITUDE (LY_UNIDADE_FISICA)
        public string Latitude { get; set; }

        //LONGITUDE (LY_UNIDADE_FISICA)
        public string Longitude { get; set; }

        //Campos que apenas serão visiveis no cadastro:

        //CEP (LY_UNIDADE_ENSINO e LY_UNIDADE_FISICA)
        public string Cep { get; set; }

        //MUNICIPIO (LY_UNIDADE_ENSINO e LY_UNIDADE_FISICA)
        public string Municipio { get; set; }

        public string MunicipioDescricao { get; set; }

        public string UF { get; set; }

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
    }
}
