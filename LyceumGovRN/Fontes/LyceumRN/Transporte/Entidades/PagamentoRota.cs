using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Transporte.Entidades
{
    [AtributoTabela("Transporte.PAGAMENTOROTA", Nome = "Transporte.PAGAMENTOROTA")]
    public class PagamentoRota : IEntity
    {
        [AtributoCampo(Nome = "PAGAMENTOROTAID")]
        public int PagamentoRotaId { get; set; }

        [AtributoCampo(Nome = "PAGAMENTOID")]
        public int PagamentoId { get; set; }

        [AtributoCampo(Nome = "ROTAID")]
        public int RotaId { get; set; }

        [AtributoCampo(Nome = "SITUACAOPAGAMENTOID")]
        public int SituacaoPagamentoId { get; set; }

        [AtributoCampo(Nome = "QUANTIDADEDIASIDA")]
        public int QuantidadeDiasIda { get; set; }

        [AtributoCampo(Nome = "QUANTIDADEDIASVOLTA")]
        public int QuantidadeDiasVolta { get; set; }

        [AtributoCampo(Nome = "QUANTIDADEALUNOIDA")]
        public int QuantidadeAlunoIda { get; set; }

        [AtributoCampo(Nome = "QUANTIDADEALUNOVOLTA")]
        public int QuantidadeAlunoVolta { get; set; }

        [AtributoCampo(Nome = "QUANTIDADEKMIDA")]
        public decimal QuantidadeKmIda { get; set; }

        [AtributoCampo(Nome = "QUANTIDADEKMVOLTA")]
        public decimal QuantidadeKmVolta { get; set; }

        [AtributoCampo(Nome = "VALORROTAIDA")]
        public decimal ValorRotaIda { get; set; }

        [AtributoCampo(Nome = "VALORROTAVOLTA")]
        public decimal ValorRotaVolta { get; set; }

        [AtributoCampo(Nome = "DESCONTO")]
        public decimal Desconto { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }

        [AtributoCampo(Nome = "VALORTOTAL")]
        public decimal ValorTotal { get; set; }
    }
}
