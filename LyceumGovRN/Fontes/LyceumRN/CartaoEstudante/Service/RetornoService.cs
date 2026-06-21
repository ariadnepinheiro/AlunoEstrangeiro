using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.Util;
using Techne.Lyceum.RN.CartaoEstudante.Query;
using Techne.Lyceum.RN.CartaoEstudante.DTO;
using Techne.Lyceum.RN.CartaoEstudante.Entity;

namespace Techne.Lyceum.RN.CartaoEstudante.Service
{
    public class RetornoService : SingletonBase<RetornoService>
    {
        private static readonly RetornoQuery retornoQuery = RetornoQuery.Instancia;
        private static readonly RetornoCriticaQuery retornoCriticaQuery = RetornoCriticaQuery.Instancia;

        RetornoService() { }

        public RetornoDTO ObtemDetalhesUltimoRetornoPor(int remessaId)
        {
            RetornoDTO detalheRetornoDTO;
            
            Retorno retorno = retornoQuery.ObtemUltimoRetornoPor(remessaId);

            detalheRetornoDTO = PreencheDetalheRetornoDTO(retorno);

            return detalheRetornoDTO;
        }

        private RetornoDTO PreencheDetalheRetornoDTO(Retorno retorno)
        {
            RetornoDTO dto = null;
            if (retorno != null)
            {
                dto = new RetornoDTO();
                dto.IdBeneficiario = retorno.IdBeneficiario;
                dto.RetornoId = retorno.RetornoId;
                dto.RemessaId = retorno.RemessaId;
                dto.OperadoraId = retorno.OperadoraId;
                dto.DataInclusao = retorno.DataInclusao;
                dto.DataProcessamento = retorno.DataProcessamento;
                dto.SituacaoProcessamento = retorno.SituacaoProcessamento;

                dto.Criticas = retornoCriticaQuery.ObtemPor(retorno.RetornoId).ToList<RetornoCriticaDTO>();
            }         
            return dto;
        }
    }
}
