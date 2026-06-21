using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.GestaoRede.Entidades
{
    [AtributoTabela("GestaoRede.UNIDADEFISICA_REDEINTERNET", Nome = "GestaoRede.UNIDADEFISICA_REDEINTERNET")]
    public class UnidadeFisicaRedeInternet : IEntity
    {
        [AtributoCampo(Nome = "UNIDADEFISICA_REDEINTERNETID")]
        public int UnidadeFisicaRedeInternetId { get; set; }

        [AtributoCampo(Nome = "UNIDADEFISICAID")]
        public string UnidadeFisicaId { get; set; }

        [AtributoCampo(Nome = "BANDALARGA")]
        public string BandaLarga { get; set; }

        [AtributoCampo(Nome = "DISPOSITIVOESCOLA")]
        public string DispositivoEscola { get; set; }

        [AtributoCampo(Nome = "DISPOSITIVOPESSOAL")]
        public string DispositivoPessoal { get; set; }

        [AtributoCampo(Nome = "REDECABO")]
        public string RedeCabo { get; set; }

        [AtributoCampo(Nome = "REDEWIRELESS")]
        public string RedeWireless { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
