using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.Transporte
{
    public class PagamentoRota
    {
        public bool PossuiSituacaoPagamentoPor(DataContext contexto, int situacaoPagamentoId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Transporte.PAGAMENTOROTA (NOLOCK)
                                    WHERE SITUACAOPAGAMENTOID = @SITUACAOPAGAMENTOID ";

            contextQuery.Parameters.Add("@SITUACAOPAGAMENTOID", SqlDbType.Int, situacaoPagamentoId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiRotaPor(DataContext contexto, int rotaId)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                    FROM Transporte.PAGAMENTOROTA (NOLOCK)
                                    WHERE ROTAID = @ROTAID ";

            contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, rotaId);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public decimal CalculaValor(int tipoContratacao, decimal valorRota, decimal quantidadeKm, int diasLetivos)
        {
            //Deverá ser calculado levando em consideração a informação do campo [contratação] e [valor rota]
            decimal valorCalculado = 0;

            if (tipoContratacao == 2) //2 - KM
            {
                //Para KM = [QT KM ROTA] * [DIAS LETIVOS DECLARADOS] * [VALOR ROTA]
                valorCalculado = quantidadeKm * diasLetivos * valorRota;
            }
            else if (tipoContratacao == 1) //1 - ALUGUEL
            {
                //Para ALUGUEL = [VALOR ROTA] * [DIAS LETIVOS DECLARADOS]
                valorCalculado = valorRota * diasLetivos;
            }
           
            return valorCalculado;
        }

        public void Insere(DataContext contexto, Entidades.PagamentoRota pagamentoRota)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO Transporte.PAGAMENTOROTA
                                           (ROTAID
                                           ,PAGAMENTOID
                                           ,SITUACAOPAGAMENTOID
                                           ,QUANTIDADEDIASIDA
                                           ,QUANTIDADEDIASVOLTA
                                           ,QUANTIDADEALUNOIDA
                                           ,QUANTIDADEALUNOVOLTA
                                           ,QUANTIDADEKMIDA
                                           ,QUANTIDADEKMVOLTA
                                           ,VALORROTAIDA
                                           ,VALORROTAVOLTA
                                           ,DESCONTO
                                           ,USUARIOID
                                           ,DATACADASTRO
                                           ,DATAALTERACAO)
                                     VALUES
                                           (@ROTAID,
                                           @PAGAMENTOID,
                                           @SITUACAOPAGAMENTOID,
                                           @QUANTIDADEDIASIDA,
                                           @QUANTIDADEDIASVOLTA,
                                           @QUANTIDADEALUNOIDA,
                                           @QUANTIDADEALUNOVOLTA,
                                           @QUANTIDADEKMIDA,
                                           @QUANTIDADEKMVOLTA,
                                           @VALORROTAIDA,
                                           @VALORROTAVOLTA,
                                           @DESCONTO,
                                           @USUARIOID,
                                           @DATACADASTRO,
                                           @DATAALTERACAO) ";

            contextQuery.Parameters.Add("@ROTAID", SqlDbType.Int, pagamentoRota.RotaId);
            contextQuery.Parameters.Add("@PAGAMENTOID", SqlDbType.Int, pagamentoRota.PagamentoId);
            contextQuery.Parameters.Add("@SITUACAOPAGAMENTOID", SqlDbType.Int, pagamentoRota.SituacaoPagamentoId);
            contextQuery.Parameters.Add("@QUANTIDADEDIASIDA", SqlDbType.Int, pagamentoRota.QuantidadeDiasIda);
            contextQuery.Parameters.Add("@QUANTIDADEDIASVOLTA", SqlDbType.Int, pagamentoRota.QuantidadeDiasVolta);
            contextQuery.Parameters.Add("@QUANTIDADEALUNOIDA", SqlDbType.Int, pagamentoRota.QuantidadeAlunoIda);
            contextQuery.Parameters.Add("@QUANTIDADEALUNOVOLTA", SqlDbType.Int, pagamentoRota.QuantidadeAlunoVolta);
            contextQuery.Parameters.Add("@QUANTIDADEKMIDA", SqlDbType.Decimal, pagamentoRota.QuantidadeKmIda);
            contextQuery.Parameters.Add("@QUANTIDADEKMVOLTA", SqlDbType.Decimal, pagamentoRota.QuantidadeKmVolta);
            contextQuery.Parameters.Add("@VALORROTAIDA", SqlDbType.Decimal, pagamentoRota.ValorRotaIda);
            contextQuery.Parameters.Add("@VALORROTAVOLTA", SqlDbType.Decimal, pagamentoRota.ValorRotaVolta);
            contextQuery.Parameters.Add("@DESCONTO", SqlDbType.Decimal, pagamentoRota.Desconto);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, pagamentoRota.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
