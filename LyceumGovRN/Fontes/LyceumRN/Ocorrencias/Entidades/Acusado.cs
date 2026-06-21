using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.MapeamentoAtributos;
using Seeduc.Infra.Entities;

namespace Techne.Lyceum.RN.Ocorrencias.Entidades
{
    [AtributoTabela("Ocorrencias.ACUSADO", Nome = "Ocorrencias.ACUSADO")]
    public class Acusado : IEntity
    {
        [AtributoCampo(Nome = "ACUSADOID")]
        public int AcusadoId { get; set; }

        [AtributoCampo(Nome = "OCORRENCIAID")]
        public int OcorrenciaId { get; set; }

        public int Tipo { get; set; } //1-Aluno / 2-Servidor / 3-Unidade / 4-Outros

        public bool Desconhecido { get; set; }

        [AtributoCampo(Nome = "PESSOAID")]
        public decimal? PessoaId { get; set; }

        public int? Vinculo { get; set; }

        public string Nome { get; set; }

        [AtributoCampo(Nome = "RGNUMERO")]
        public string RgNumero { get; set; }

        [AtributoCampo(Nome = "RGTIPO")]
        public string RgTipo { get; set; }

        [AtributoCampo(Nome = "RGEMISSOR")]
        public string RgEmissor { get; set; }

        [AtributoCampo(Nome = "RGUF")]
        public string RgUF { get; set; }

        [AtributoCampo(Nome = "RGDATAEXP")]
        public DateTime? RgDataExp { get; set; }

        public string CPF { get; set; }

        [AtributoCampo(Nome = "DATANASCIMENTO")]
        public DateTime? DataNascimento { get; set; }

        public string Cargo { get; set; }

        public string Funcao { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
