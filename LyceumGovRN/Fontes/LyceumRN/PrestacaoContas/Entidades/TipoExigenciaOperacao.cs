using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.TIPOEXIGENCIAOPERACAO", Nome = "PrestacaoContas.TIPOEXIGENCIAOPERACAO")]
    public class TipoExigenciaOperacao : IEntity
    {
        [AtributoCampo(Nome = "TIPOEXIGENCIAOPERACAOID")]
        public int TipoExigenciaOperacaoId { get; set; }

        [AtributoCampo(Nome = "DESCRICAO")]
        public string Descricao { get; set; }

        [AtributoCampo(Nome = "ATIVO")]
        public int Ativo { get; set; }

        [AtributoCampo(Nome = "NECESSITAANEXO")]
        public int NecessitaAnexo { get; set; }
        
        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
