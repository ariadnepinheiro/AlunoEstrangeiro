using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.CartaoEstudante.Entity
{
    class ControleProcessamento: IEntity
    {
        [AtributoCampo(Nome = "CONTROLEPROCESSAMENTOID")]
        public int ControleProcessamentoId { get; set; }

        [AtributoCampo(Nome = "NOMEPROCESSO")]
        public string NomeProcesso { get; set; }

        [AtributoCampo(Nome = "DATAAGENDA")]
        public DateTime DataAgenda { get; set; }

        [AtributoCampo(Nome = "FREQUENCIA")]
        public string Frequencia { get; set; }

        [AtributoCampo(Nome = "DATAINCLUSAO")]
        public DateTime DataInclusao { get; set; }

        [AtributoCampo(Nome = "USUARIO")]
        public string Usuario { get; set; }

        [AtributoCampo(Nome = "DATAINICIOMOVIMENTO")]
        public DateTime? DataInicioMovimento { get; set; }

        [AtributoCampo(Nome = "DATAFIMMOVIMENTO")]
        public DateTime? DataFimMovimento { get; set; }
    }
}
