using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Matriculas.Entidades
{
    [AtributoTabela("Matricula.TIPOFILA", Nome = "Matricula.TIPOFILA")]
    public class TipoFila : IEntity
    {
        [AtributoCampo(Nome = "TIPOFILAID")]
        public int TipoFilaId { get; set; }

        public string Descricao { get; set; }

        public int Prioridade { get; set; }

        public int Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
