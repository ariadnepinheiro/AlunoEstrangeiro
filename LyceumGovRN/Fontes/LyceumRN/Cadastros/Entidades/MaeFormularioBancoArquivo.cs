using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Cadastros.Entidades
{
    [AtributoTabela("Cadastros.MAE_FORMULARIOBANCOARQUIVO", Nome = "Cadastros.MAE_FORMULARIOBANCOARQUIVO")]
    public class MaeFormularioBancoArquivo : IEntity
    {
        [AtributoCampo(Nome = "MAE_FORMULARIOBANCOARQUIVOID")]
        public int MaeFormularioBancoArquivoId { get; set; }

        [AtributoCampo(Nome = "MAE_INSCRICAOID")]
        public int MaeInscricaoId { get; set; }

        [AtributoCampo(Nome = "CHAVEARQUIVO")]
        public string ChaveArquivo { get; set; }

        [AtributoCampo(Nome = "ARQUIVO")]
        public byte[] Arquivo { get; set; }

        [AtributoCampo(Nome = "TIPOARQUIVO")]
        public string TipoArquivo { get; set; }

        [AtributoCampo(Nome = "NOMEARQUIVO")]
        public string NomeArquivo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
