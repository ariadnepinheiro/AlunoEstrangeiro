using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Matriculas.Entidades
{
    [AtributoTabela("Matricula.OPCAOINSCRICAO", Nome = "Matricula.OPCAOINSCRICAO")]
    public class OpcaoInscricao : IEntity
    {
        [AtributoCampo(Nome = "OPCAOINSCRICAOID")]
        public int OpcaoInscricaoId { get; set; }

        [AtributoCampo(Nome = "INSCRICAOALUNOID")]
        public int InscricaoAlunoId { get; set; }

        [AtributoCampo(Nome = "CONTROLEVAGAID")]
        public int ControleVagaId { get; set; }

        [AtributoCampo(Nome = "TIPOCANDIDATOID")]
        public int? TipoCandidatoId { get; set; }

        [AtributoCampo(Nome = "TIPOFILAID")]
        public int? TipoFilaId { get; set; }

        [AtributoCampo(Nome = "DATACONVOCACAO")]
        public DateTime? DataConvocacao { get; set; }

        [AtributoCampo(Nome = "PRAZORESPOSTA")]
        public DateTime? PrazoResposta { get; set; }

        [AtributoCampo(Nome = "FASE")]
        public int Fase { get; set; }

        [AtributoCampo(Nome = "VAGACONCORRENTE")]
        public bool VagaConcorrente { get; set; }

        [AtributoCampo(Nome = "MOTIVORETORNOID")]
        public int MotivoRetornoId { get; set; }

        [AtributoCampo(Nome = "DATARETORNO")]
        public DateTime DataRetorno { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
