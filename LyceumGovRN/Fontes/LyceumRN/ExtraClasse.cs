using System;
using System.IO;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;


namespace Techne.Lyceum.RN
{
    public class ExtraClasse : RNBase
    {
        public static int NumeroAlunos(string unidade, decimal ano)
        {
            string sql = @"select count(distinct s.aluno) from 
                    (select distinct a.aluno from ly_aluno a
                    inner join ly_matricula m  on a.ALUNO = m.ALUNO
                    where m.SIT_MATRICULA in ('Matriculado', 'Aprovado', 'Rep Freq', 'Rep Nota')
                    and a.UNIDADE_ENSINO = ? and m.ANO = ?
                    union
                    select distinct a.aluno from ly_aluno a
                    inner join ly_histmatricula m  on a.ALUNO = m.ALUNO
                    where m.SITUACAO_HIST in ('Matriculado', 'Aprovado', 'Rep Freq', 'Rep Nota')
                    and a.UNIDADE_ENSINO = ? and m.ANO = ?
                    ) s ";
            int num = ExecutarFuncao(sql, unidade, ano, unidade, ano - 1);
            return num;
        }

        public static int NumeroTurmas(string unidade, decimal ano)
        {
            string sql = @"select count(distinct turma) from LY_TURMA 
            where UNIDADE_RESPONSAVEL = ? and SIT_TURMA = 'Aberta' and ANO = ?";
            int num = ExecutarFuncao(sql, unidade, ano);
            return num;
        }

        public static int NumeroSegmentoModalidade(string unidade, decimal ano)
        {
            string sql = @"select count(distinct mo.MODALIDADE) from LY_MODALIDADE_CURSO mo
                inner join LY_CURSO cur on cur.MODALIDADE = mo.MODALIDADE
                inner join LY_TURMA tu on tu.CURSO = cur.CURSO
                where tu.SIT_TURMA = 'Aberta' 
                and tu.UNIDADE_RESPONSAVEL = ? and tu.ANO = ?";
            int num = ExecutarFuncao(sql, unidade, ano);
            return num;
        }

        public static int NumeroTurnos(string unidade, decimal ano)
        {
            string sql = @"select COUNT(distinct t.turno) from ly_turno t
                inner join LY_TURMA tu on tu.TURNO = t.TURNO
                where tu.SIT_TURMA = 'Aberta' 
                and tu.UNIDADE_RESPONSAVEL = ? and tu.ANO = ?";
            int num = ExecutarFuncao(sql, unidade, ano);
            return num;
        }

        public static int NumeroServidores(string unidade, DateTime? data)
        {
            string sql = @"
                select COUNT(1) from LY_LOTACAO 
                where UNIDADE_ENS = ? 
                and MATRICULA not like '33%' 
                and MATRICULA not like '55%' 
                AND (DATA_DESATIVACAO is null or convert(date,DATA_DESATIVACAO) > convert(date,?))";
            int num = ExecutarFuncao(sql, unidade, data);
            return num;
        }

        public static string Classificacao(string unidade)
        {
            return ConsultarCampo("select classificacao from ly_unidade_ensino where unidade_ens = ?", unidade);

        }

        public static bool ValidaLotacao(TConnection cn, string matricula, string unidade, string funcao, decimal ano, DateTime? data)
        {
            //verifica se unidade possui controle de extra classe
            if (ControlaExtraClasse(cn, unidade))
            {
                //verifica se existe critério para função e ano
                if (ExisteCriterios(cn, funcao, ano))
                {
                    //verifica quantas funções ativas já existem na unidade
                    int qtd = QuantidadeFuncaoAtiva(cn, funcao, unidade);
                    int permitida = 0;
                    string criterio = string.Empty;

                    //verifica cada critério
                    criterio = "Número de Alunos";
                    if (ExisteCriterio(cn, funcao, ano, criterio))
                    {
                        int numAlunos = NumeroAlunos(unidade, ano);
                        //verifica o intervalo que se encaixa
                        permitida = QuantidadeFuncaoPermitida(cn, funcao, ano, criterio, numAlunos);
                        if (permitida != -1)
                        {
                            if (qtd >= permitida)
                                return false;
                        }
                    }

                    criterio = "Número de Turmas";
                    if (ExisteCriterio(cn, funcao, ano, criterio))
                    {
                        int numTurmas = NumeroTurmas(unidade, ano);
                        permitida = QuantidadeFuncaoPermitida(cn, funcao, ano, criterio, numTurmas);
                        if (permitida != -1)
                        {
                            if (qtd >= permitida)
                                return false;
                        }

                    }

                    criterio = "Segmento/Modalidade";
                    if (ExisteCriterio(cn, funcao, ano, criterio))
                    {
                        int numModalidades = NumeroSegmentoModalidade(unidade, ano);
                        permitida = QuantidadeFuncaoPermitida(cn, funcao, ano, criterio, numModalidades);
                        if (permitida != -1)
                        {
                            if (qtd >= permitida)
                                return false;
                        }

                    }

                    criterio = "Número de Turnos";
                    if (ExisteCriterio(cn, funcao, ano, criterio))
                    {
                        int numTurnos = NumeroTurnos(unidade, ano);
                        permitida = QuantidadeFuncaoPermitida(cn, funcao, ano, criterio, numTurnos);
                        if (permitida != -1)
                        {
                            if (qtd >= permitida)
                                return false;
                        }

                    }

                    criterio = "Número de Servidores";
                    if (ExisteCriterio(cn, funcao, ano, criterio))
                    {
                        int numServidores = NumeroServidores(unidade, data);
                        permitida = QuantidadeFuncaoPermitida(cn, funcao, ano, criterio, numServidores);
                        if (permitida != -1)
                        {
                            if (qtd >= permitida)
                                return false;
                        }

                    }

                    criterio = "Classificação";
                    if (ExisteCriterio(cn, funcao, ano, criterio))
                    {
                        string classificacao = Classificacao(unidade);
                        permitida = QuantidadeFuncaoPermitida(cn, funcao, ano, criterio, classificacao);
                        if (permitida != -1)
                        {
                            if (qtd >= permitida)
                                return false;
                        }

                    }
                }
            }
            return true;
        }

        public static bool ControlaExtraClasse(TConnection cn, string unidade)
        {
            if (!string.IsNullOrEmpty(unidade))
            {
                string sql = "select top 1 1 from LY_UNIDADE_ENSINO where unidade_ens = ? and extraclasse = 'S'";
                int retorno = ExecutarFuncao(sql, cn, unidade);

                if (retorno == 1)
                    return true;
            }
            return false;
        }

        public static bool ExisteCriterios(TConnection cn, string funcao, decimal ano)
        {
            string sql = "select top 1 1 from ly_extra_classe where funcao = ? and ano = ?";
            int retorno = ExecutarFuncao(sql, cn, funcao, ano);

            if (retorno == 1)
                return true;
            else
                return false;
        }

        public static bool ExisteCriterio(TConnection cn, string funcao, decimal ano, string criterio)
        {
            string sql = "select top 1 1 from ly_extra_classe where funcao = ? and ano = ? and criterio = ?";
            int retorno = ExecutarFuncao(sql, cn, funcao, ano, criterio);

            if (retorno == 1)
                return true;
            else
                return false;
        }

        private static int QuantidadeFuncaoAtiva(TConnection cn, string funcao, string unidade) //passar datas!!!!!!
        {
            int count = ExecutarFuncao("select count(1) from ly_lotacao where funcao = ? and unidade_ens = ?", cn, funcao, unidade);
            return count;
        }

        private static int QuantidadeFuncaoPermitida(TConnection cn, string funcao, decimal ano, string criterio, int valor)
        {
            int qtd = ExecutarFuncaoRetMenosUm(@"select top 1 qtde_funcao_permitida 
                                    from ly_extra_classe 
                                    where funcao = ? 
                                    and ano = ?
                                    and criterio = ?
                                    and convert(numeric,valor_criterio_inicial) <= ?
                                    and convert(numeric,valor_criterio_final) >= ?", cn, funcao, ano, criterio, valor, valor);
            return qtd;
        }

        private static int QuantidadeFuncaoPermitida(TConnection cn, string funcao, decimal ano, string criterio, string valor)
        {
            int qtd = ExecutarFuncaoRetMenosUm(@"select top 1 qtde_funcao_permitida 
                                    from ly_extra_classe 
                                    where funcao = ? 
                                    and ano = ?
                                    and criterio = ?
                                    and valor_criterio_inicial <= ?
                                    and valor_criterio_final >= ?", cn, funcao, ano, criterio, valor, valor);
            return qtd;
        }

        public static string FuncaoAnterior(string matricula, decimal? pessoa, decimal? ordem)
        {
            return ConsultarCampo("select funcao from LY_LOTACAO where PESSOA = ? and MATRICULA = ? and ORDEM = ?", pessoa, matricula, ordem);
        }
    }
}
