using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Turmas.Entidades
{
    [AtributoTabela("Turma.HISTORICOSUSPENSAO", Nome = "HISTORICOSUSPENSAO.NOTIFICACAO")]
    public class HistoricoSuspensao : IEntity
    {
        [AtributoCampo(Nome = "HISTORICOSUSPENSAOID")]
        public int HistoricoSuspensaoId { get; set; }

        public string Aluno { get; set; }

        [AtributoCampo(Nome = "DATA_EM_SUSPENSAO")]
        public DateTime DataEmSuspensao { get; set; }

        [AtributoCampo(Nome = "DIASFALTAS")]
        public int DiasFaltas { get; set; }

        [AtributoCampo(Nome = "INICIOFALTA")]
        public DateTime InicioFalta { get; set; }

        [AtributoCampo(Nome = "FIMFALTA")]
        public DateTime FimFalta { get; set; }

        [AtributoCampo(Nome = "DATA_RETORNO")]
        public DateTime? DataRetorno { get; set; }

        [AtributoCampo(Nome = "USUARIO_RETORNO")]
        public string UsuarioRetorno { get; set; }

        [AtributoCampo(Nome = "DATA_SUSPENSAO")]
        public DateTime? DataSuspensao { get; set; }

        [AtributoCampo(Nome = "DIASFALTASSUSPENSAO")]
        public int? DiasFaltasSuspensao { get; set; }

        [AtributoCampo(Nome = "INICIOFALTASUSPENSAO")]
        public DateTime? InicioFaltaSuspensao { get; set; }

        [AtributoCampo(Nome = "FIMFALTASUSPENSAO")]
        public DateTime? FimFaltaSuspensao { get; set; }

        [AtributoCampo(Nome = "DATA_CANCELADO")]
        public DateTime? DataCancelado { get; set; }

        [AtributoCampo(Nome = "ATIVOPARASUSPENDER")]
        public bool AtivoParaSuspender { get; set; }        

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}