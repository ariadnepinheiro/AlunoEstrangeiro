using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.FiscalizacaoLink.Entidades
{
    [AtributoTabela("FiscalizacaoLink.CIRCUITOSETOR", Nome = "FiscalizacaoLink.CIRCUITOSETOR")]
    public class CircuitoSetor : IEntity
    {
        [AtributoCampo(Nome = "CIRCUITOSETORID")]
        public int CircuitoSetorId {get; set; }

        [AtributoCampo(Nome = "CONTRATOSETORID")]
        public int ContratoSetorId {get; set; }

        [AtributoCampo(Nome = "VELOCIDADEID")]
        public int VelocidadeId {get; set; }

        [AtributoCampo(Nome = "TECNOLOGIAID")]
        public int TecnologiaId {get; set; }

        public string Designacao {get; set; } 

        [AtributoCampo(Nome = "CUSTOMENSAL")]
        public decimal? CustoMensal { get; set; } 

        [AtributoCampo(Nome = "QUANTIDADEMESES")]
        public int? QuantidadeMeses { get; set; } 

        [AtributoCampo(Nome = "INICIO")]
        public DateTime? Inicio { get; set; } 

        [AtributoCampo(Nome = "FIM")]
        public DateTime? Fim { get; set; } 

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}