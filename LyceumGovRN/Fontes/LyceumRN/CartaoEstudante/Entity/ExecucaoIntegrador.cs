using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.CartaoEstudante.Entity
{
    public class ExecucaoIntegrador
    {
        public int ExecucaoIntegradorId { get; set; }

        public string Tipo { get; set; }

        public int? QuantidadeRegistros { get; set; }

        public int? Cadastrados { get; set; }

        public int? Alterados { get; set; }

        public DateTime DataExecucao { get; set; }

        public string ParametrosConsulta { get; set; }

        public bool? Sucesso { get; set; }

        public string Erro { get; set; }

        public string UsuarioId { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }
    }
}
