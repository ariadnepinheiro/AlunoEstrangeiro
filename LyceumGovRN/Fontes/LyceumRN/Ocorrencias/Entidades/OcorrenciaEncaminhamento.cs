using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Ocorrencias.Entidades
{
    [AtributoTabela("Ocorrencias.OCORRENCIAENCAMINHAMENTO", Nome = "Ocorrencias.OCORRENCIAENCAMINHAMENTO")]
    public class OcorrenciaEncaminhamento : IEntity
    {
        [AtributoCampo(Nome = "ARQUIVOOCORRENCIAID")]
        public int OcorrenciaEncaminhamentoId { get; set; }

        [AtributoCampo(Nome = "OCORRENCIAID")]
        public int OcorrenciaId { get; set; }

        [AtributoCampo(Nome = "ENCAMINHAMENTO")]
        public string Encaminhamento { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
