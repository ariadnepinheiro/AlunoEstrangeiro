using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;


namespace Techne.Lyceum.RN.Certificacao.Entidades
{
    [AtributoTabela("CertificacaoEscolar.TipoDocumentoCertifica", Nome = "CertificacaoEscolar.TipoDocumentoCertifica")]
    public class TipoDocumentoCertifica : IEntity
    {
        [AtributoCampo(Nome = "DocumentoID")]
        public int DocumentoID { get; set; }
        
        [AtributoCampo(Nome = "Tipo_Documento")]
        public string Tipo_Documento { get; set; }

        [AtributoCampo(Nome = "Descricacao")]
        public string Descricacao { get; set; }

        [AtributoCampo(Nome = "Corpo")]
        public string Corpo { get; set; }

        [AtributoCampo(Nome = "TIPOVALIDADOR")]
        public string TipoValidador { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }

    }
}
