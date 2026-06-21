using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Turmas.Entidades
{
    [AtributoTabela("Turma.OFICIOMPRJ", Nome = "Turma.OFICIOMPRJ")]
    public class OficioMPRJ : IEntity
    {
        [AtributoCampo(Nome = "OFICIOMPRJID")]
        public int OficioMPRJId { get; set; }

        [AtributoCampo(Nome = "NOTIFICACAOID")]
        public int NotificacaoId { get; set; }

        [AtributoCampo(Nome = "PROMOTORIA")]
        public string Promotoria { get; set; }

        [AtributoCampo(Nome = "CEP")]
        public string Cep { get; set; }

        [AtributoCampo(Nome = "MUNICIPIO")]
        public string Municipio { get; set; }

        [AtributoCampo(Nome = "ENDERECO")]
        public string Endereco { get; set; }

        [AtributoCampo(Nome = "NUMERO")]
        public string Numero { get; set; }

        [AtributoCampo(Nome = "bairro")]
        public string Bairro { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
