using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.RecursosHumanos.Entidades
{
    [AtributoTabela("RecursosHumanos.HISTORICOALTERACAOALUNO", Nome = "RecursosHumanos.HISTORICOALTERACAOALUNO")]
    public class HistoricoAlteracaoAluno: IEntity
    {
        [AtributoCampo(Nome = "HISTORICOALTERACAOALUNOID")]
        public int HistoricoAlteracaoAlunoId { get; set; }

        [AtributoCampo(Nome = "PESSOA")]
        public int Pessoa { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
