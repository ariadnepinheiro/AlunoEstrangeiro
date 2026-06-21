using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Entity;

namespace Techne.Lyceum.RN.CartaoEstudante.Query
{
    class CartaoQuery: QueryBase<CartaoQuery>
    {
        private const string TABELA_CARTAO = "Lyceum.CartaoEstudante.Cartao";
        private const string TABELA_CARTAO_ALUNO = "Lyceum.CartaoEstudante.Cartao__Aluno";
        private const string TABELA_CARTAO_ALUNO_SITUACAO = "Lyceum.CartaoEstudante.Cartao__AlunoSituacao";
        private const string TABELA_CARTAO_SITUACAO = "Lyceum.CartaoEstudante.CartaoSituacao";

        public static readonly string INSERT_CARTAO = "INSERT INTO " + TABELA_CARTAO_ALUNO +
            " (OPERADORAID, NUMEROCHIP, NUMEROCARTAO, NUMEROLOGICO) " +
            "VALUES" +
            " (@OPERADORAID, @NUMEROCHIP, @NUMEROCARTAO, @NUMEROLOGICO)";
        
        public static readonly string INSERT_CARTAO_ALUNO = "INSERT INTO " + TABELA_CARTAO_ALUNO +
            " (ALUNO, CARTAOID) " +
            "VALUES" +
            " (@ALUNO, @CARTAOID)";

        public static readonly string INSERT_CARTAO_ALUNO_SITUACAO = "INSERT INTO " + TABELA_CARTAO_ALUNO_SITUACAO +
            " (CARTAO__ALUNOID, SITUACAO, DATASITUACAO) " +
            "VALUES" +
            " (@CARTAO__ALUNOID, @SITUACAO, @DATASITUACAO)";

        public static readonly string INSERT_CARTAO_SITUACAO = "INSERT INTO " + TABELA_CARTAO_SITUACAO +
            " (TIPOCANCELAMENTOID, RETORNOCARTAOID, TIPOSITUACAOCARTAOID, CARTAOID, SITUACAOCARTAO, DATASITUACAO) " +
            "VALUES" +
            " (@TIPOCANCELAMENTOID, @RETORNOCARTAOID, @TIPOSITUACAOCARTAOID, @CARTAOID, @SITUACAOCARTAO, @DATASITUACAO)";

        CartaoQuery() {
            NomeTabela = TABELA_CARTAO;
        }
    }
}
