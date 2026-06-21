using System;
using System.Collections.Generic;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.PrestacaoContas.DTOs
{
    public class DadosEvento
    {
        //Campos gerais

        public int PlanoTrabalhoId { get; set; }

        public string Descricao { get; set; }

        public string Censo { get; set; }

        public int EventoId { get; set; }

        public string NumeroEvento { get; set; }

        public int TipoDespesa { get; set; }

        public string DescricaoTipoDespesa { get; set; }

        public bool? Aprovado { get; set; }

        public DateTime? DataAprovacao { get; set; }

        public string PlanoTrabalhoDescricao { get; set; }

        public string CensoNomeComp { get; set; }

        public int FinalidadeId { get; set; }

        public string FinalidadeDescricao { get; set; }

        public bool TodasExigenciasAprovadas { get; set; }

        //Campos das abas por Tipo

        public int? FornecedorId { get; set; } //usado para Evento e Pequenas Despesas

        public string NumeroNotaFiscal { get; set; } //usado para Evento e Pequenas Despesas

        public decimal? ValorNotaFiscal { get; set; } //usado para Evento e Pequenas Despesas       

        public DateTime? DataNotaFiscal { get; set; } //usado para Evento

        public decimal ValorPagamento { get; set; }   //Usado para todos os tipos

        public DateTime? DataPagamento { get; set; } //usado para Evento e Pequenas Despesas  

        public int? NotaFiscalArquivoId { get; set; } //usado para Evento e Pequenas Despesas  

        public byte[] NotaFiscalArquivo { get; set; } //usado para Evento e Pequenas Despesas

        public string NotaFiscalNomeArquivo { get; set; } //usado para Evento e Pequenas Despesas

        public string NotaFiscalTipoArquivo { get; set; } //usado para Evento e Pequenas Despesas  

        public int? Orcamento1Id { get; set; } //usado para Evento

        public byte[] Orcamento1Arquivo { get; set; } //usado para Evento

        public string Orcamento1NomeArquivo { get; set; } //usado para Evento

        public string Orcamento1TipoArquivo { get; set; } //usado para Evento

        public int? Orcamento2Id { get; set; } //usado para Evento

        public byte[] Orcamento2Arquivo { get; set; } //usado para Evento

        public string Orcamento2NomeArquivo { get; set; } //usado para Evento

        public string Orcamento2TipoArquivo { get; set; } //usado para Evento

        public int? Orcamento3Id { get; set; } //usado para Evento

        public byte[] Orcamento3Arquivo { get; set; } //usado para Evento

        public string Orcamento3NomeArquivo { get; set; } //usado para Evento

        public string Orcamento3TipoArquivo { get; set; } //usado para Evento


        public byte[] EvidenciaArquivo { get; set; } //usado para Evento

        public string EvidenciaNomeArquivo { get; set; } //usado para Evento

        public string EvidenciaTipoArquivo { get; set; } //usado para Evento


        public string JustificativaOrcamento { get; set; } //usado para Evento

        public string ChaveAcesso { get; set; }//usado para Evento

        public int? ComprovantePagamentoArquivoId { get; set; }//usado para Evento

        public byte[] ComprovantePagamentoArquivo { get; set; } //usado para Evento

        public string ComprovantePagamentoNomeArquivo { get; set; } //usado para Evento

        public string ComprovantePagamentoTipoArquivo { get; set; }//usado para Evento

        public bool PossuiXmlImportado { get; set; }//usado para Evento

        public string Observacoes { get; set; }//usado para Evento

        public string Evidencias { get; set; }//usado para Evento



        public int? EvidenciaArquivoId { get; set; } //usado para Evento

        //public byte[] EvidenciaArquivo { get; set; } //usado para Evento

        //public string EvidenciaNomeArquivo { get; set; } //usado para Evento

        //public string EvidenciaTipoArquivo { get; set; } //usado para Evento



        public List<DTOs.DadosPequenaDespesaServidor> Servidores { get; set; }



        public string FormaPagamento { get; set; }//usado para Pequenas Despesas

        public int? TipoTransporteId { get; set; } //usado para Transporte

        public string Origem { get; set; }//usado para Transporte

        public string Destino { get; set; }//usado para Transporte

        public string Justificativa { get; set; }//usado para Transporte

        public string UsuarioId { get; set; } // Todos

        public string NomeUsuario { get; set; } // usado para Prestação de Contas

        public string StatusAnalise
        {
            get
            {
                if (!NumeroEvento.IsNullOrEmptyOrWhiteSpace() && !Aprovado.HasValue)
                    return "Enviado para Análise";
                else if (!NumeroEvento.IsNullOrEmptyOrWhiteSpace() && !Aprovado.Value)
                    return "Reprovado";
                else if (!NumeroEvento.IsNullOrEmptyOrWhiteSpace() && Aprovado.Value)
                    return "Validado";
                else
                    return "Aberto";
            }
        }

        public int PequenaDespesaId { get; set; }
    }
}
