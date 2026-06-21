using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Entidades;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Certificacao.Entidades
{
    [AtributoTabela("CertificacaoEscolar.ALUNODOCUMENTO", Nome = "CertificacaoEscolar.ALUNODOCUMENTO")]
    public class AlunoDocumento : IEntity
    {
        [AtributoCampo(Nome = "ALUNODOCUMENTOID")]
        public int AlunoDocumentoId { get; set; }

        [AtributoCampo(Nome = "ALUNOCERTIFICACAOID")]
        public int AlunoCertificacaoId { get; set; }

        [AtributoCampo(Nome = "UNIDADEENSINO")]
        public string UnidadeEnsino { get; set; }

        [AtributoCampo(Nome = "DOCUMENTOID")]
        public int DocumentoId { get; set; }

        [AtributoCampo(Nome = "TIPOCONCLUSAOID")]
        public int TipoConclusaoId { get; set; }

        public string Modalidade { get; set; }

        [AtributoCampo(Nome = "NOMECURSO")]
        public string NomeCurso { get; set; }

        [AtributoCampo(Nome = "ATOAUTORIZA")]
        public string AtoAutoriza { get; set; }

        [AtributoCampo(Nome = "DATAAUTORIZA")]
        public DateTime DataAutoriza { get; set; }

        [AtributoCampo(Nome = "TOTALHORASAULA")]
        public string TotalHorasAula { get; set; }

        [AtributoCampo(Nome = "TOTALHORASRELOGIO")]
        public string TotalHorasRelogio { get; set; }

        [AtributoCampo(Nome = "DATACONCLUSAO")]
        public DateTime DataConclusao { get; set; }

        [AtributoCampo(Nome = "NUMEROLIVRO")]
        public string NumeroLivro { get; set; }

        [AtributoCampo(Nome = "FOLHALIVRO")]
        public string FolhaLivro { get; set; }

        public string Livro { get; set; }

        public string Observacao { get; set; }

        public int Sequencial { get; set; }

        [AtributoCampo(Nome = "CODIGOVALIDADOR")]
        public string CodigoValidador { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}

