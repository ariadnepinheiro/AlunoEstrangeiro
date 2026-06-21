using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.FiscalizacaoLink.Entidades
{
    [AtributoTabela("FiscalizacaoLink.CONTRATOOPERADORA", Nome = "FiscalizacaoLink.CONTRATOOPERADORA")]
    public class ContratoOperadora : IEntity
    {
        [AtributoCampo(Nome = "CONTRATOOPERADORAID")]
        public int ContratoOperadoraId { get; set; }

        [AtributoCampo(Nome = "CONTRATOID")]
        public int ContratoId { get; set; }

        [AtributoCampo(Nome = "OPERADORAID")]
        public int OperadoraId { get; set; }

        [AtributoCampo(Nome = "LOTE")]
        public string Lote { get; set; }       

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}

