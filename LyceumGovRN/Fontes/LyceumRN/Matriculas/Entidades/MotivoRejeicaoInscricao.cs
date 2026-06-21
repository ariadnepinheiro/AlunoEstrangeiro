using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Matriculas.Entidades
{
    [AtributoTabela("Matricula.MOTIVOREJEICAOINSCRICAO", Nome = "Matricula.MOTIVOREJEICAOINSCRICAO")]
    public class MotivoRejeicaoInscricao : IEntity
    {
        [AtributoCampo(Nome = "MOTIVOREJEICAOINSCRICAOID")]
        public int MotivoRejeicaoInscricaoId { get; set; }

        public string Descricao { get; set; }

        public int Tipo { get; set; }

        public bool Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
