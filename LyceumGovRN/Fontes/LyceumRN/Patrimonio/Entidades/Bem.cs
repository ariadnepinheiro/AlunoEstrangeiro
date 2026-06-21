using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Entities;
using Seeduc.Infra.MapeamentoAtributos;

namespace Techne.Lyceum.RN.Patrimonio.Entidades
{
    [AtributoTabela("Patrimonio.BEM", Nome = "Patrimonio.BEM")]
    public class Bem : IEntity
    {
        [AtributoCampo(Nome = "BEMID")]
        public int BemId { get; set; }

        [AtributoCampo(Nome = "OPERACAOID")]
        public int OperacaoId { get; set; }

        [AtributoCampo(Nome = "CLASSIFICACAOID")]
        public int ClassificacaoId { get; set; }

        public int Numero { get; set; }

        public string Descricao { get; set; } 

        [AtributoCampo(Nome = "DATAAQUISICAO")]
        public DateTime DataAquisicao { get; set; }

        [AtributoCampo(Nome = "DOCUMENTOHABIL")]
        public string DocumentoHabil { get; set; }

        [AtributoCampo(Nome = "VALORMERCADO")]
        public decimal? ValorMercado { get; set; }

        [AtributoCampo(Nome = "PERIODOVIDAUTILIZADO")]
        public int? PeriodoVidaUtilizado { get; set; }

        public string Historico { get; set; }

        public bool Baixa { get; set; }

        [AtributoCampo(Nome = "MOTIVOBAIXAID")]
        public int? MotivoBaixaId { get; set; }

        [AtributoCampo(Nome = "DATABAIXA")]
        public DateTime? DataBaixa { get; set; }

        [AtributoCampo(Nome = "PROCESSOBAIXA")]
        public string ProcessoBaixa { get; set; }

        [AtributoCampo(Nome = "JUSTIFICATIVABAIXA")]
        public string JustificativaBaixa { get; set; }

        [AtributoCampo(Nome = "BOLETIMOCORRENCIA")]
        public string BoletimOcorrencia { get; set; }

        [AtributoCampo(Nome = "INSTITUICAODESTINO")]
        public string InstituicaoDestino { get; set; }

        [AtributoCampo(Nome = "CNPJINSTITUICAODESTINO")]
        public string CnpjInstituicaoDestino { get; set; }

        [AtributoCampo(Nome = "USUARIOID")]
        public string UsuarioId { get; set; }

        [AtributoCampo(Nome = "DATACADASTRO")]
        public DateTime DataCadastro { get; set; }

        [AtributoCampo(Nome = "DATAALTERACAO")]
        public DateTime DataAlteracao { get; set; }
    }
}
