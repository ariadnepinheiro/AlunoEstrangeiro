using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Entidades;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Certificacao.Entidades
{
    [AtributoTabela("CertificacaoEscolar.ALUNODOCUMENTO", Nome = "CertificacaoEscolar.ALUNODOCUMENTO")]
    public class AlunoCertificacao : IEntity
    {
        [AtributoCampo(Nome = "ALUNOCERTIFICACAOID")]
        public int AlunoCertificacaoId { get; set; }

        public string Nome { get; set; }

        [AtributoCampo(Nome = "NOMEMAE")]
        public string NomeMae { get; set; }

        [AtributoCampo(Nome = "NOMEPAI")]
        public string NomePai { get; set; }

        [AtributoCampo(Nome = "MAENAODECLARADA")]
        public bool chkNaoDeclarMae { get; set; }

        [AtributoCampo(Nome = "PAINAODECLARADO")]
        public bool chkNaoDeclarPai { get; set; }

        [AtributoCampo(Nome = "DATANASCIMENTO")]
        public DateTime DataNascimento { get; set; }

        [AtributoCampo(Nome = "MUNICIPIONASCIMENTO")]
        public string MunicipioNascimento { get; set; }

        public string Nacionalidade { get; set; }

        public string CPF { get; set; }

        [AtributoCampo(Nome = "RGNUMERO")]
        public string RgNumero { get; set; }

        [AtributoCampo(Nome = "RGEMISSOR")]
        public string RgEmissor { get; set; }

        [AtributoCampo(Nome = "RGUF")]
        public string RgUf { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioResponsavel { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
