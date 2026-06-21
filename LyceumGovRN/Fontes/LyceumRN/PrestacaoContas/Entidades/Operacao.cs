using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.OPERACAO", Nome = "PrestacaoContas.OPERACAO")]
    public class Operacao : IEntity
    {
        [AtributoCampo(Nome = "OPERACAOID")]
        public int OperacaoId { get; set; }

        [AtributoCampo(Nome = "PERIODOREFERENCIAID")]
        public int PeriodoReferenciaId { get; set; }

        [AtributoCampo(Nome = "PLANOTRABALHOID")]
        public int PlanoTrabalhoId { get; set; }

        [AtributoCampo(Nome = "CENSO")]
        public string Censo { get; set; }

        [AtributoCampo(Nome = "JUSTIFICATIVA")]
        public string Justificativa { get; set; }

        [AtributoCampo(Nome = "TIPOOPERACAO")]
        public string TipoOperacao { get; set; }

        [AtributoCampo(Nome = "VALOR")]
        public decimal Valor { get; set; }

        [AtributoCampo(Nome = "STATUS")]
        public int Status { get; set; }

        [AtributoCampo(Nome = "DATAANALISE")]
        public DateTime DataAnalise { get; set; }

        [AtributoCampo(Nome = "MOTIVOREPROVACAOOPERACAOID")]
        public int MotivoReprovacaoOperacaoId { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }

        [AtributoCampo(Nome = "CODOPERACAO")]
        public string CodOperacao { get; set; }

        
    }
}
