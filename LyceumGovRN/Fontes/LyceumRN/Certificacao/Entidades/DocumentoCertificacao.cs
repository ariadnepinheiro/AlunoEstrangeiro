using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Certificacao.Entidades
{
    [AtributoTabela("CertificacaoEscolar.DocumentoCertificacao", Nome = "CertificacaoEscolar.DocumentoCertificacao")]
    public class DocumentoCertificacao : IEntity
    {
        [AtributoCampo(Nome = "TipoConclusaoId")]
        public int TipoConclusaoId { get; set; }

        [AtributoCampo(Nome = "Aluno")]
        public string Aluno { get; set; }

        [AtributoCampo(Nome = "Pessoa")]
        public decimal? Pessoa { get; set; }

        [AtributoCampo(Nome = "DocumentoCertId")]
        public int DocumentoCertId { get; set; }
        
       [AtributoCampo(Nome = "DocumentoId")]
        public int DocumentoId { get; set; }

        [AtributoCampo(Nome = "Numero")]
        public string Numero { get; set; }

        [AtributoCampo(Nome = "Folhas")]
        public string Folhas { get; set; }

        [AtributoCampo(Nome = "Livro")]
        public string Livro { get; set; }

        [AtributoCampo(Nome = "Observacao")]
        public string Observacao { get; set; }

        [AtributoCampo(Nome = "Eixo")]
        public string Eixo { get; set; }

        [AtributoCampo(Nome = "CENSO")]
        public string Censo { get; set; } //Será alimentado apenas ao gerar o pdf

        [AtributoCampo(Nome = "SEQUENCIAL")]
        public int Sequencial { get; set; } //Será alimentado apenas ao gerar o pdf

        [AtributoCampo(Nome = "CODIGOVALIDOR")]
        public string CodigoValidador { get; set; } //Será alimentado apenas ao gerar o pdf

        [AtributoCampo(Nome = "Autorizado")]
        public bool Autorizado { get; set;}

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
