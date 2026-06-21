using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Transporte.Entidades
{
    [AtributoTabela("Transporte.ROTATRAJETO", Nome = "Transporte.ROTATRAJETO")]
    public class RotaTrajeto : IEntity
    {
        [AtributoCampo(Nome = "ROTATRAJETOID")]
        public int RotaTrajetoId { get; set; }

        [AtributoCampo(Nome = "ROTAID")]
        public int RotaId { get; set; }

        [AtributoCampo(Nome = "PRESTADORID")]
        public int? PrestadorId { get; set; }

        [AtributoCampo(Nome = "CONDUTORID")]
        public int? CondutorId { get; set; }

        [AtributoCampo(Nome = "VEICULOID")]
        public int? VeiculoId { get; set; }

        [AtributoCampo(Nome = "TIPOCONTRATACAOID")]
        public int TipoContratacaoId { get; set; }

        [AtributoCampo(Nome = "VALORROTA")]
        public decimal ValorRota { get; set; }

        public bool Ida { get; set; }

        [AtributoCampo(Nome = "QUANTIDADEKM")]
        public decimal? QuantidadeKm { get; set; }

        [AtributoCampo(Nome = "TEMPO")]
        public int? Tempo { get; set; }

        public bool Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
