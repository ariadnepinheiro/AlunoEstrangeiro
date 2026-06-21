using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Ocorrencias.Entidades
{
    [AtributoTabela("Ocorrencias.OCORRENCIAINTERRUPCAO", Nome = "Ocorrencias.OCORRENCIAINTERRUPCAO")]
    public class OcorrenciaInterrupcao : IEntity
    {
        [AtributoCampo(Nome = "OCORRENCIAINTERRUPCAOID")]
        public int OcorrenciaInterrupcaoId { get; set; }

        [AtributoCampo(Nome = "OCORRENCIAID")]
        public int OcorrenciaId { get; set; }

        [AtributoCampo(Nome = "DATAINTERRUPCAO")]
        public DateTime DataInterrupcao { get; set; }

        public bool Manha { get; set; }

        public bool Tarde { get; set; }

        public bool Noite { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
