using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Protocolo.Entidades
{
    [AtributoTabela("Protocolo.Analise", Nome = "Protocolo.Analise")]
    public class Analise
    {
        [AtributoCampo(Nome = "ANALISEID")]
        public int AnaliseId { get; set; }

        [AtributoCampo(Nome = "PROTOCOLOPRESTACAOID")]
        public int ProtocoloPrestacaoId { get; set; }

        [AtributoCampo(Nome = "SITUACAOPROTOCOLOID")]
        public int SituacaoProtocoloId { get; set; }

        [AtributoCampo(Nome = "DATASITUACAO")]
        public DateTime DataSituacao { get; set; }

        public string Descricao { get; set; }

        [AtributoCampo(Nome = "USUARIOANALISADOR")]
        public string UsuarioAnalisador { get; set; }

        [AtributoCampo(Nome = "USUARIOREVISOR")]
        public string UsuarioRevisor { get; set; }

        [AtributoCampo(Nome = "USUARIOSISTEMA")]
        public string UsuarioSistema { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
