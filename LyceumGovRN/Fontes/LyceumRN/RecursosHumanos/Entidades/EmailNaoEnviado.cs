using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.RecursosHumanos.Entidades
{
    [AtributoTabela("RecursosHumanos. EMAILNAOENVIADO", Nome = "RecursosHumanos.EMAILNAOENVIADO")]
    public class EmailNaoEnviado : IEntity
    {
        [AtributoCampo(Nome = "EMAILNAOENVIADOID")]
        public int EmailNaoEnviadoId { get; set; }

        [AtributoCampo(Nome = "PROJETO")]
        public string Projeto { get; set; }

        [AtributoCampo(Nome = "REMETENTE")]
        public string Remetente { get; set; }

        [AtributoCampo(Nome = "DESTINATARIO")]
        public string Destinatario { get; set; }

        [AtributoCampo(Nome = "ASSUNTO")]
        public string Assunto { get; set; }

        [AtributoCampo(Nome = "TEXTO")]
        public string Texto { get; set; }

        [AtributoCampo(Nome = "ENVIADO")]
        public bool Enviado { get; set; }

        [AtributoCampo(Nome = "DATAENVIO")]
        public DateTime DataEnvio { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
