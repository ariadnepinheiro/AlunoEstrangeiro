using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.PrestacaoContas.Entidades
{
    [AtributoTabela("PrestacaoContas.PEQUENADESPESASERVIDOR", Nome = "PrestacaoContas.PEQUENADESPESASERVIDOR")]
    public class PequenaDespesaServidor : IEntity
    {
        [AtributoCampo(Nome = "PEQUENADESPESASERVIDORID")]
        public int PequenaDespesaServidorId { get; set; }

        [AtributoCampo(Nome = "PEQUENADESPESAID")]
        public int PequenaDespesaId { get; set; }

        [AtributoCampo(Nome = "PESSOA")]
        public int Pessoa { get; set; }

        [AtributoCampo(Nome = "IDFUNCIONAL")]
        public int IdFuncional { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
