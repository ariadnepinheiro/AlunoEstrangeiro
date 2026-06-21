using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Patrimonio.Entidades
{
    [AtributoTabela("Patrimonio.MOEDA", Nome = "Patrimonio.MOEDA")]
    public class Moeda : IEntity
    {
        [AtributoCampo(Nome = "MOEDAID")]
        public int MoedaId { get; set; }

        public string Descricao { get; set; }

        public string Sigla { get; set; }

        public int Fator { get; set; }

        [AtributoCampo(Nome = "DATAINICIO")]
        public DateTime DataInicio { get; set; }

        [AtributoCampo(Nome = "DATAFIM")]
        public DateTime? DataFim { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
