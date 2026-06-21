using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Transporte.Entidades
{
    [AtributoTabela("Transporte.ROTA", Nome = "Transporte.ROTA")]
    public class Rota : IEntity
    {
        [AtributoCampo(Nome = "ROTAID")]
        public int RotaId { get; set; }

        [AtributoCampo(Nome = "TIPOCONTRATACAOID")]
        public int TipoCalculoPagamentoId { get; set; }

        public string Censo { get; set; }

        public string Turno { get; set; }

        public int Ordem { get; set; }

        public bool Aprovado { get; set; }

        [AtributoCampo(Nome = "USUARIOAPROVACAOID")]
        public string UsuarioAprovacaoId { get; set; }

        [AtributoCampo(Nome = "DATALIMITEEDICAO")]
        public DateTime? DataLimiteEdicao { get; set; }

         [AtributoCampo(Nome = "DATALIMITEEDICAOALUNO")]
        public DateTime? DataLimiteEdicaoAluno { get; set; }

        public bool Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
