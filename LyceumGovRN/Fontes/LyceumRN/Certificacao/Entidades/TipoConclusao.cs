using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Certificacao.Entidades
{
    [AtributoTabela("CertificacaoEscolar.TipoConclusao", Nome = "CertificacaoEscolar.TipoConclusao")]
    public class TipoConclusao : IEntity
    {
        [AtributoCampo(Nome = "TipoConclusaoID")]
        public int TipoConclusaoID { get; set; }

        [AtributoCampo(Nome = "Descricacao")]
        public string Descricacao { get; set; }

        [AtributoCampo(Nome = "Ativo")]
        public bool Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
