using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.PLANOTRABALHO", Nome = "PrestacaoContas.PLANILHAORCAMENTARIA")]
    public class PlanoTrabalho : IEntity
    {
        [AtributoCampo(Nome = "PLANOTRABALHOID")]
        public int PlanoTrabalhoId { get; set; }

        [AtributoCampo(Nome = "FINALIDADEID")]
        public int FinalidadeId { get; set; }

        [AtributoCampo(Nome = "SUPERINTENDENCIAID")]
        public int SuperintendenciaId { get; set; }

        [AtributoCampo(Nome = "TIPODESPESAID")]
        public int TipoDespesaId { get; set; }

        [AtributoCampo(Nome = "TIPOCONTRATACAOID")]
        public int TipoContratacaoId { get; set; }

        [AtributoCampo(Nome = "PROGRAMATRABALHOID")]
        public int ProgramaTrabalhoId { get; set; }

        public string Descricao { get; set; }

        public string Identificador { get; set; }

        [AtributoCampo(Nome = "IDENTIFICADORSEQ")]
        public int IdentificadorSeq { get; set; } 

        public string Periodicidade { get; set; }

        [AtributoCampo(Nome = "PEQUENADESPESA")]
        public bool PequenaDespesa { get; set; }       

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
