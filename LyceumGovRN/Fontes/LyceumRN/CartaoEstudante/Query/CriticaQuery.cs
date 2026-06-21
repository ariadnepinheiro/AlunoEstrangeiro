using System;
using System.Collections.Generic;
using Techne.Lyceum.RN.CartaoEstudante.Entity;
using System.Data.SqlClient;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.CartaoEstudante.Query
{
    class CriticaQuery : QueryBase<CriticaQuery>
    {
        private const string TABELA_TIPO_RETORNO_CRITICA = "LYCEUM.CartaoEstudante.TIPORETORNOCRITICA";
        public string Erro_Nao_Identificado_Na_Aplicacao { get { return "99"; } }

        CriticaQuery() { }

        #region MÉTODOS
        public List<TipoRetornoCritica> ListaCriticasPadrao()
        {
            List<TipoRetornoCritica> criticasPadrao = new List<TipoRetornoCritica>();

            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = "5", Descricao = "Data de nascimento nula ou inválida" });
            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = "7", Descricao = "Tipo de movimentação inválido" });
            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = "8", Descricao = "Município nulo ou inválido" });
            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = "9", Descricao = "Censo nulo, não cadastrado ou inativo" });
            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = "12", Descricao = "Nome nulo" });
            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = "14", Descricao = "Nome da mãe nulo" });
            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = "16", Descricao = "Endereço, ou parte dele, nulo ou inválido" });
            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = "27", Descricao = "ID da solicitação menor que o último processado" });
            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = "28", Descricao = "Município da escola bloqueado" });
            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = "29", Descricao = "Município da escola de outra bilhetagem" });
            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = "30", Descricao = "Matrícula nula ou inválida" });
            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = "31", Descricao = "Matrícula não localizada" });
            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = "32", Descricao = "Turno nulo ou inválido" });
            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = "33", Descricao = "Turma nula ou inválida" });
            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = "34", Descricao = "Outro campo obrigatório nulo (se obrigatório) ou inválido" });
            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = "35", Descricao = "E-mail nulo ou inválido" });
            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = "36", Descricao = "Login nulo ou inválido" });
            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = "37", Descricao = "Cartão já estava bloqueado" });
            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = "38", Descricao = "Cartão já estava descancelado" });
            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = "39", Descricao = "Cadastro do aluno bloqueado" });
            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = "40", Descricao = "Marcação de necessidade de gratuidade não definida ou negada" });
            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = "41", Descricao = "Marcação de necessidade de modal não definida ou negada" });
            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = "42", Descricao = "Data/hora da foto nula ou inválida" });
            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = "43", Descricao = "Matrícula já incluída anteriormente" });
            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = "44", Descricao = "Possui benefício mais amplo ou impeditivo" });
            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = "45", Descricao = "Assinatura inválida" });
            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = "46", Descricao = "E-mail duplicado em cadastro de outro aluno" });

            // No momento está sendo usando "99" por não haver uma definição de qual será.          
            criticasPadrao.Add(new TipoRetornoCritica() { CodigoCritica = Erro_Nao_Identificado_Na_Aplicacao, Descricao = "Erro não identificado na aplicação." });

            return criticasPadrao;
        }

        public void InsereCriticasPadrao()
        {
            List<TipoRetornoCritica> criticas = new List<TipoRetornoCritica>();

            try
            {
                criticas = ListaCriticasPadrao();

                foreach (TipoRetornoCritica critica in criticas)
                {
                    if (!PossuiCritica(critica))
                        InsereCritica(critica);
                }                   
            }
            catch (SqlException sqlEx)
            {
                throw sqlEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsereCritica(TipoRetornoCritica tipoRetornoCritica)
        {
            string INSERT_TIPO_RETORNO_CRITICA = @"
                INSERT INTO " + TABELA_TIPO_RETORNO_CRITICA + " (CODIGOCRITICA, DESCRICAO) VALUES (@CODIGOCRITICA, @DESCRICAO)";

            AplicarModificacoes(
                INSERT_TIPO_RETORNO_CRITICA, 
                tipoRetornoCritica.CodigoCritica, tipoRetornoCritica.Descricao
            );
        }

        public bool PossuiCritica(TipoRetornoCritica tipoRetornoCritica)
        {
            string SELECT_POSSUI_CRITICA = 
                "SELECT 1 FROM " + TABELA_TIPO_RETORNO_CRITICA + " " + 
                "WHERE " +
	            "    CODIGOCRITICA = @CODIGOCRITICA AND" +
                "    DESCRICAO = @DESCRICAOCRITICA";    

            return Possui(
                SELECT_POSSUI_CRITICA, 
                tipoRetornoCritica.CodigoCritica, tipoRetornoCritica.Descricao
            );
        }

        public TipoRetornoCritica ObtemCriticaPadrao()
        {
            return ObtemCriticaPor(Erro_Nao_Identificado_Na_Aplicacao);
        }

        public TipoRetornoCritica ObtemCriticaPor(string codigo)
        {
            TipoRetornoCritica tipoRetornoCritica;

            string SELECT_CRITICA =
                "SELECT * FROM " + TABELA_TIPO_RETORNO_CRITICA + " " +
                "WHERE " +
                "    CODIGOCRITICA = @CODIGOCRITICA";

            tipoRetornoCritica = ObtemPor<TipoRetornoCritica>(
                SELECT_CRITICA, 
                codigo
            );

            return tipoRetornoCritica;    
        }

        #endregion
    }
}
