using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.CartaoEstudante.Entity
{
    class LogControleProcessamento: IEntity
    {
        [AtributoCampo(Nome = "LOGCONTROLEPROCESSAMENTOID")]
        public int LogControleProcessamentoId { get; set; }

        [AtributoCampo(Nome = "CONTROLEPROCESSAMENTOID")]
        public int ControleProcessamentoId { get; set; }

        [AtributoCampo(Nome = "DATAPROCESSAMENTO")]
        public DateTime DataProcessamento { get; set; }

        [AtributoCampo(Nome = "REGISTROINICIAL")]
        public int? RegistroInicial { get; set; }

        [AtributoCampo(Nome = "REGISTROFINAL")]
        public int? RegistroFinal { get; set; }

        [AtributoCampo(Nome = "QUANTIDADEREGISTROSRETORNADOS")]
        public int? QuantidadeRegistrosRetornados { get; set; }

        [AtributoCampo(Nome="ULTIMOREGISTROPROCESSADO")]
        public int? UltimoRegistroProcessado { get; set; }

        [AtributoCampo(Nome = "SITUACAOPROCESSAMENTO")]
        public string SituacaoProcessamento { get; set; }
    }
}
