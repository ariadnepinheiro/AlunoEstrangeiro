using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Entity;

namespace Techne.Lyceum.RN.CartaoEstudante.Query
{
    class ProcessoQuery : QueryBase<ProcessoQuery>
    {
        private const string TABELA_PROCESSO = "Lyceum.CartaoEstudante.Processo";
        
        ProcessoQuery()
        {
            NomeTabela = TABELA_PROCESSO;
        }
       
        public Processo ObtemPor(string nomeProcesso)
        {
            string SELECT_CONTROLE_PROCESSAMENTO = "SELECT * FROM " + NomeTabela + " WHERE NOMEPROCESSO = @NOMEPROCESSO";
            return ObtemPor<Processo>(SELECT_CONTROLE_PROCESSAMENTO, nomeProcesso);
        }

        public List<Processo> ListaPor(string nomeProcesso)
        {
            string SELECT_CONTROLE_PROCESSAMENTO = "SELECT * FROM " + NomeTabela + " WHERE NOMEPROCESSO = @NOMEPROCESSO";
            return ListaPor<Processo>(SELECT_CONTROLE_PROCESSAMENTO, nomeProcesso);
        }
    }
}
