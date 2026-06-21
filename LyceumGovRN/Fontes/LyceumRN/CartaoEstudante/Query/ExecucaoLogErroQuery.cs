using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Entity;

namespace Techne.Lyceum.RN.CartaoEstudante.Query
{
    class ExecucaoLogErroQuery : QueryBase<ExecucaoLogErroQuery>
    {
        private static readonly string TABELA_EXECUCAO_LOG_ERRO = "LYCEUM.CartaoEstudante.EXECUCAOLOGERRO";

        private static readonly string INSERT_EXECUCAO_LOG_ERRO = "INSERT INTO " + TABELA_EXECUCAO_LOG_ERRO +
        " (EXECUCAOID, ALUNO, DESCRICAOERRO, DATAINCLUSAO) " + "VALUES " + " (@EXECUCAOID, @ALUNO, @DESCRICAOERRO, GETDATE())";
        
        ExecucaoLogErroQuery(){
            NomeTabela = TABELA_EXECUCAO_LOG_ERRO;
        }

        public void Insere(ExecucaoLogErro execucaoLogErro)
        {
            AplicarModificacoes(
                INSERT_EXECUCAO_LOG_ERRO,
                execucaoLogErro.ExecucaoId, execucaoLogErro.Aluno,
                execucaoLogErro.Descricao
            );
        }

    }
}