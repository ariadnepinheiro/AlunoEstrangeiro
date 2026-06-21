using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Entidades
{
    [AtributoTabela("dbo.LY_PERIODO_LETIVO", Nome = "dbo.LY_PERIODO_LETIVO")]
    public class LyPeriodoLetivo
    {
        [AtributoCampo(Nome = "ANO")]
        public decimal Ano { get; set; }

        [AtributoCampo(Nome = "PERIODO")]
        public decimal Periodo { get; set; }

        [AtributoCampo(Nome = "DESCRICAO")]
        public string Descricao { get; set; }

        [AtributoCampo(Nome = "PER_ANO")]
        public decimal? PerAno { get; set; }

        [AtributoCampo(Nome = "PER_PERIODO")]
        public decimal? PerPeriodo { get; set; }

        [AtributoCampo(Nome = "DT_INICIO")]
        public DateTime? DtInicio { get; set; }

        [AtributoCampo(Nome = "DT_FIM")]
        public DateTime? DtFim { get; set; }

        [AtributoCampo(Nome = "DT_INICIO_AULA")]
        public DateTime? DtInicioAula { get; set; }

        [AtributoCampo(Nome = "DT_FIM_AULA")]
        public DateTime? DtFimAula { get; set; }

        [AtributoCampo(Nome = "DATA_INICIO_DOCENTE")]
        public DateTime? DataInicioDocente { get; set; }

        [AtributoCampo(Nome = "DATA_FIM_DOCENTE")]
        public DateTime? DataFimDocente { get; set; }

        [AtributoCampo(Nome = "DATA_INICIO_INDICACAO_ELETIVA")]
        public DateTime? DataInicioIndicacaoEletiva { get; set; }

        [AtributoCampo(Nome = "DATA_FIM_INDICACAO_ELETIVA")]
        public DateTime? DataFimIndicacaoEletiva { get; set; }

        [AtributoCampo(Nome = "DATA_INICIO_DISTRIBUICAO_ELETIVA")]
        public DateTime? DataInicioDistribuicaoEletiva { get; set; }

        [AtributoCampo(Nome = "DATA_FIM_INDICACAO_ELETIVA")]
        public DateTime? DataFimDistribuicaoEletiva { get; set; }

        [AtributoCampo(Nome = "QTDE_SUBPERIODO")]
        public int QtdeSubperiodo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
