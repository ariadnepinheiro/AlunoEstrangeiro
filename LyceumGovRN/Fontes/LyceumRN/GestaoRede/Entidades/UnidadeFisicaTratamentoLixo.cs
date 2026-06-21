using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.GestaoRede.Entidades
{
    [AtributoTabela("GestaoRede.UNIDADEFISICA_TRATAMENTOLIXO", Nome = "GestaoRede.UNIDADEFISICA_TRATAMENTOLIXO")]
    public class UnidadeFisicaTratamentoLixo : IEntity
    {
        [AtributoCampo(Nome = "UNIDADEFISICA_TRATAMENTOLIXOID")]
        public int UnidadeFisicaTratamentoLixoId { get; set; }

        [AtributoCampo(Nome = "TRATAMENTOLIXOID")]
        public int TratamentoLixoId { get; set; }

        [AtributoCampo(Nome = "UNIDADEFISICAID")]
        public string UnidadeFisicaId { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
