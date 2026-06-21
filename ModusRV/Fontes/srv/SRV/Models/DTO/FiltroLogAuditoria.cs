using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SRV.Models.Domain;
using System.Collections;
using System.ComponentModel;

namespace SRV.Models.DTO
{
    public enum ObjetoAuditoria
    {
        [DescriptionAttribute("ANO REFERÊNCIA")]
        AnoReferencia,

        [DescriptionAttribute("AVALIAÇÃO EXTERNA")]
        AvaliacaoExterna,

        [DescriptionAttribute("AVALIAÇÃO EXTERNA UNIDADE ADMINISTRATIVA")]
        AvaliacaoExternaUnidadeAdmin,

        [DescriptionAttribute("CRITÉRIO UNIDADE ADMINISTRATIVA")]
        CriterioUnidadeAdministrativa,

        [DescriptionAttribute("FUNÇÃO")]
        Funcao,

        [DescriptionAttribute("FUNÇÃO SERVIDOR")]
        FuncaoServidor,

        [DescriptionAttribute("GRUPO FUNÇÃO")]
        GrupoFuncao,

        [DescriptionAttribute("INDICADOR")]
        Indicador,

        [DescriptionAttribute("INDICADOR UNIDADE ADMINISTRATIVA")]
        IndicadorUnidadeAdministrativa,

        [DescriptionAttribute("META IGE UNIDADE ADMINISTRATIVA")]
        MetaIGEUnidadeAdministrativa,

        [DescriptionAttribute("META UNIDADE ADMINISTRATIVA")]
        MetaUnidadeAdministrativa,

        [DescriptionAttribute("MODALIDADE")]
        Modalidade,

        [DescriptionAttribute("NOTA")]
        Nota,

        [DescriptionAttribute("NÍVEL DE ENSINO")]
        NivelEnsino,

        [DescriptionAttribute("NÍVEL DE ENSINO UNIDADE ADMINISTRATIVA")]
        NivelEnsinoUnidadeAdministrativa,

        [DescriptionAttribute("OCORRÊNCIA")]
        Ocorrencia,

        [DescriptionAttribute("OCORRÊNCIA SERVIDOR")]
        OcorrenciaServidor,

        [DescriptionAttribute("PARÂMETRO CURVA PREMIAÇÃO")]
        ParametroCurva,

        [DescriptionAttribute("PARÂMETRO NOTA")]
        ParametroNota,

        [DescriptionAttribute("PARÂMETRO PESO")]
        ParametroPeso,

        [DescriptionAttribute("SERVIDOR")]
        Servidor,

        [DescriptionAttribute("TIPO CRITÉRIO ELEGIBILIDADE")]
        TipoCriterioElegibilidade,

        [DescriptionAttribute("TIPO DE OCORRÊNCIA")]
        TipoOcorrencia,

        [DescriptionAttribute("TIPO UNIDADE ADMINISTRATIVA")]
        TipoUnidadeAdministrativa,

        [DescriptionAttribute("UNIDADE ADMINISTRATIVA")]
        UnidadeAdministrativa
    }

    public class FiltroLogAuditoria
    {
        [Display(Name = "Operação")]
        public OperacaoAuditoria? TipoOperacao { get; set; }
        public IEnumerable OperacoesAuditoria { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data/Hora Início")]
        public DateTime? DataIni { get; set; }

        [DataType(DataType.Time)]
        public DateTime HoraIni { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data/Hora Fim")]
        public DateTime? DataFin { get; set; }

        [DataType(DataType.Time)]
        public DateTime HoraFin { get; set; }

        [Display(Name = "Objeto")]
        public ObjetoAuditoria? ObjetoAuditoria { get; set; }
        public IEnumerable ObjetosAuditoria { get; set; }

        [Display(Name = "Usuário")]
        public int? IdUsuario { get; set; }
        public string LoginUsuario { get; set; }
        public string NomeUsuario { get; set; }

        public Paging<LogAuditoria> Logs { get; set; }
    }
}