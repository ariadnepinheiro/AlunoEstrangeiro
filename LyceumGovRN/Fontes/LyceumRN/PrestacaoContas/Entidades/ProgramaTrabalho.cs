using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.PRODUTOSERVICOGRUPO", Nome = "PrestacaoContas.PRODUTOSERVICOGRUPO")]
    public class ProgramaTrabalho : IEntity
    {
        [AtributoCampo(Nome = "PROGRAMATRABALHOID")]
        public int ProgramaTrabalhoId { get; set; }

        [AtributoCampo(Nome = "WSPROGRAMASEFAZID")]
        public int WsProgramaSefazId { get; set; }

        public bool Ativo { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}