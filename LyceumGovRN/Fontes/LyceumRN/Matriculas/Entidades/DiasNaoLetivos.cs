using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Matriculas.Entidades
{
    [AtributoTabela("Matricula.DIASNAOLETIVOS", Nome = "Matricula.DIASNAOLETIVOS")]
    public class DiasNaoLetivos : IEntity
    {
        [AtributoCampo(Nome = "DIASNAOLETIVOSID")]
        public int DiasNaoLetivosId { get; set; }

        [AtributoCampo(Nome = "DIA")]
        public DateTime Dia { get; set; }

        [AtributoCampo(Nome = "MUNICIPIOID")]
        public string MunicipioId { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
