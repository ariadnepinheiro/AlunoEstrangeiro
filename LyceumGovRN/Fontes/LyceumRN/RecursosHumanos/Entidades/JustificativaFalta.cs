using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.RecursosHumanos.Entidades
{
    [AtributoTabela("RecursosHumanos.JUSTIFICATIVAFALTA", Nome = "RecursosHumanos.JUSTIFICATIVAFALTA")]
    public class JustificativaFalta : IEntity
    {
        [AtributoCampo(Nome = "JUSTIFICATIVAFALTAID")]
        public int JustificativaFaltaId { get; set; }

        public string Descricao { get; set; }

        [AtributoCampo(Nome = "LEIAMPARO")]
        public string LeiAmparo { get; set; }

        [AtributoCampo(Nome = "CASOESPECIFICO")]
        public bool CasoEspecifico { get; set; }

        public bool Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }

    }
}
