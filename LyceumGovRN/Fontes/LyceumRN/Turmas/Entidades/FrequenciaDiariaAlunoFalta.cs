using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Turmas.Entidades
{
    [AtributoTabela("Turma.FREQUENCIADIARIA_ALUNOFALTA", Nome = "Turma.FREQUENCIADIARIA_ALUNOFALTA")]
    public class FrequenciaDiariaAlunoFalta: IEntity
    {
        [AtributoCampo(Nome = "FREQUENCIADIARIA_ALUNOID")]
        public int FrequenciaDiariaAlunoFaltaId { get; set; }

        [AtributoCampo(Nome = "FREQUENCIADIARIAID")]
        public int FrequenciaDiariaId { get; set; }

        public string Aluno { get; set; }

        public bool Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
