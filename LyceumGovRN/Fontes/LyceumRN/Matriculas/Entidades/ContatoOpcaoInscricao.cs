using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Matriculas.Entidades
{
    [AtributoTabela("Matricula.CONTATOOPCAOINSCRICAO", Nome = "Matricula.CONTATOOPCAOINSCRICAO")]
    public class ContatoOpcaoInscricao : IEntity
    {
        [AtributoCampo(Nome = "CONTATOOPCAOINSCRICAOID")]
        public int ContatoOpcaoInscricaoId { get; set; }

        [AtributoCampo(Nome = "OPCAOINSCRICAOID")]
        public int OpcaoInscricaoId { get; set; }

        [AtributoCampo(Nome = "DATACONTATO")]
        public DateTime DataContato { get; set; }

        public bool? Contato { get; set; }

        public bool? Aceito { get; set; }

        [AtributoCampo(Nome = "MOTIVOREJEICAOINSCRICAOID")]
        public int? MotivoRejeicaoInscricaoId { get; set; }

        public string Observacao { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
