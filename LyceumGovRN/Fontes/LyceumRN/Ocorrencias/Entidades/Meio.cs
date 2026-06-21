using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Ocorrencias.Entidades
{
    [AtributoTabela("Ocorrencias.MEIO", Nome = "Ocorrencias.MEIO")]
    public class Meio : IEntity
    {
        [AtributoCampo(Nome = "MEIOID")]
        public int MeioId { get; set; }

        public string Descricao { get; set; }

        public int Ordem { get; set; }

        public bool Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}