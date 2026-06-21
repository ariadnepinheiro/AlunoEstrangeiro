using System;
using System.Data;
using System.Globalization;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;
using Techne.HadesLyc.CR;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN
{
    public class Verbas : RNBase
    {
        #region Unidades
        public static QueryTable ConsultarVerba(string ano, string periodo, string nucleo)
        {
            string sql = "select " +
                        "ue.unidade_ens as unidade,  " +
                        "ue.nome_comp as nome,  " +
                        "uv.valor as totaldisponib,  " +
                        "count(ht.tipo_aula) horas,  " +
                        "(select valor from LY_VALOR_GLP where ANO = ? and SEMESTRE = ? ) as valor_glp, " +
                        "(count(ht.tipo_aula) * (select valor from LY_VALOR_GLP where ANO = ? and SEMESTRE = ? )) as verbautilizada, " +
                        "(uv.valor - (count(ht.tipo_aula) * (select valor from LY_VALOR_GLP where ANO = ? and SEMESTRE = ?))) as saldo " +
                        "from VW_ZZCRO_UNIDADE_ENSINO  ue  " +
                   "join ly_unidades_associadas u " +
                        "on ue.unidade_ens = u.unidade_ens " +
                   "left join ly_hor_aula h  " +
                        "on  u.unidade_fis = h.faculdade " +
                        "and h.ano = ? " +
                        "and h.semestre = ? " +
                   "left join ly_aula_docente_tipo ht " +
                        "on  h.turno = ht.turno " +
                        "and h.faculdade = ht.faculdade " +
                       "and h.dia_semana = ht.dia_semana " +
                        "and h.aula = ht.aula " +
                        "and h.disciplina = ht.disciplina " +
                        "and h.turma = ht.turma " +
                        "and h.ano = ht.ano " +
                        "and h.semestre = ht.semestre " +
                   "left join LY_UNIDADE_ENSINO_VERBA uv " +
                        "on  ue.unidade_ens = uv.unidade_ens " +
                        "and uv.ano = ? " +
                        "and uv.semestre = ? " +
        "where ht.TIPO_AULA = 'GLP' and ue.nucleo = ? " +
"group by ue.unidade_ens, ue.nome_comp, uv.valor order by nome";

            return Consultar(sql, ano, periodo, ano, periodo, ano, periodo, ano, periodo, ano, periodo, nucleo);
        }

        public static QueryTable ConsultarVerba(string ano, string periodo, string nucleo, string municipio)
        {


            string sql = "select " +
             "ue.unidade_ens as unidade,  " +
             "ue.nome_comp as nome,  " +
             "uv.valor as totaldisponib,  " +
             "count(ht.tipo_aula) horas,  " +
             "(select valor from LY_VALOR_GLP where ANO = ? and SEMESTRE = ? ) as valor_glp, " +
             "(count(ht.tipo_aula) * (select valor from LY_VALOR_GLP where ANO = ? and SEMESTRE = ? )) as verbautilizada, " +
             "(uv.valor - (count(ht.tipo_aula) * (select valor from LY_VALOR_GLP where ANO = ? and SEMESTRE = ?))) as saldo " +
             "from VW_ZZCRO_UNIDADE_ENSINO  ue  " +
        "join ly_unidades_associadas u " +
             "on ue.unidade_ens = u.unidade_ens " +
        "left join ly_hor_aula h  " +
             "on  u.unidade_fis = h.faculdade " +
             "and h.ano = ? " +
             "and h.semestre = ? " +
        "left join ly_aula_docente_tipo ht " +
             "on  h.turno = ht.turno " +
             "and h.faculdade = ht.faculdade " +
            "and h.dia_semana = ht.dia_semana " +
             "and h.aula = ht.aula " +
             "and h.disciplina = ht.disciplina " +
             "and h.turma = ht.turma " +
             "and h.ano = ht.ano " +
             "and h.semestre = ht.semestre " +
        "left join LY_UNIDADE_ENSINO_VERBA uv " +
             "on  ue.unidade_ens = uv.unidade_ens " +
             "and uv.ano = ? " +
             "and uv.semestre = ? " +
"where ht.TIPO_AULA = 'GLP' and ue.nucleo = ? and ue.municipio = ? " +
"group by ue.unidade_ens, ue.nome_comp, uv.valor order by nome";

            return Consultar(sql, ano, periodo, ano, periodo, ano, periodo, ano, periodo, ano, periodo, nucleo, municipio);
        }

        public static void InsereValor(string unidade, int ano, int semestre, decimal valor)
        {
            IAE(" Insert into LY_UNIDADE_ENSINO_VERBA (UNIDADE_ENS, ANO, SEMESTRE, VALOR) values (?, ?, ?, ?)", unidade, ano, semestre, valor.SubstituirVirgulaPorPonto());
        }

        public static void DeletaValor(string unidade, int ano, int semestre, decimal valor)
        {
            if (!string.IsNullOrEmpty(valor.ToString()))
            {
                string sql = " delete from LY_UNIDADE_ENSINO_VERBA where unidade_ens = ? and ano = ? and semestre = ?";
                IAE(sql, unidade, ano, semestre);
            }

        }

        public static void AlteraValor(string unidade, int ano, int semestre, decimal valor)
        {
            string sql = " Update LY_UNIDADE_ENSINO_VERBA Set VALOR = ? where unidade_ens = ? and ano = ? and semestre = ?";
            IAE(sql, valor.SubstituirVirgulaPorPonto(), unidade, ano, semestre);
        }

        public static decimal ConcultaVerbaNucleo(string nucleo, string ano, string semestre)
        {
            string sql = "Select valor From LY_NUCLEO_VERBA where nucleo = ? and ano = ? and semestre = ?";
            return Convert.ToDecimal(ExecutarFuncaoDec(sql, nucleo, ano, semestre));
        }

        public static decimal ConcultarSomatorioVerbas(string nucleo, string ano, string semestre)
        {
            string sql = "select sum(valor) from LY_UNIDADE_ENSINO_VERBA v inner join VW_ZZCRO_UNIDADE_ENSINO u on u.UNIDADE_ENS = v.UNIDADE_ENS " +
                               "where u.nucleo = ? and v.ano = ? and v.semestre = ?";

            return Convert.ToDecimal(ExecutarFuncaoDec(sql, nucleo, ano, semestre));
        }
        #endregion

        #region Coordenadoria
        public static QueryTable ConsultarVerbaCoordenadoria(string ano, string periodo)
        {
            string sql = "select  " +
"n.nucleo as nucleo,    " +
"n.descricao as nome,   " +
"nv.valor as totaldisponib,     " +
"count(ht.tipo_aula) horas,     " +
"(select valor from LY_VALOR_GLP where ANO = ? and SEMESTRE = ? ) as valor_glp, " +
"(count(ht.tipo_aula) * (select valor from LY_VALOR_GLP where ANO = ? and SEMESTRE = ? )) as verbautilizada, " +
"(nv.valor - (count(ht.tipo_aula) * (select valor from LY_VALOR_GLP where ANO = ? and SEMESTRE = ?))) as saldo  " +
"from ly_nucleo n  join ly_UNIDADE_ENSINO ue   " +
                        "on n.nucleo = ue.nucleo " +
                   "join ly_unidades_associadas u  " +
                        "on ue.unidade_ens = u.unidade_ens  " +
                   "left join ly_aula_docente_tipo ht   " +
                        "on u.unidade_fis = ht.faculdade   " +
                        "and ht.ano = ?    " +
                        "and ht.semestre = ?   " +
                        "and ht.TIPO_AULA = 'GLP'    " +
                   "left join ly_nucleo_verba nv    " +
                        "on  n.nucleo = nv.nucleo    " +
                        "and nv.ano = ?    " +
                        "and nv.semestre = ?   " +
"group by n.nucleo, n.descricao, nv.valor order by n.nucleo ";

            return Consultar(sql, ano, periodo, ano, periodo, ano, periodo, ano, periodo, ano, periodo);
        }

        public static decimal ConsultarTotalCoordenadoria(string ano, string semestre)
        {
            string sql = "select sum(valor) from LY_NUCLEO_VERBA v where v.ano = ? and v.semestre = ?";
            return Convert.ToDecimal(ExecutarFuncaoDec(sql, ano, semestre));
        }

        public static decimal ConsultarTotalCoordenadoria(string nucleo, string ano, string semestre)
        {
            string sql = "select sum(valor) from LY_NUCLEO_VERBA v where v.nucleo = ? and v.ano = ? and v.semestre = ?";
            return Convert.ToDecimal(ExecutarFuncaoDec(sql, nucleo, ano, semestre));
        }

        public static decimal ConsultarTotalEscola(string ano, string semestre)
        {
            string sql = "select sum(valor) from LY_UNIDADE_ENSINO_VERBA v where v.ano = ? and v.semestre = ?";
            return Convert.ToDecimal(ExecutarFuncaoDec(sql, ano, semestre));
        }

        public static void InsereValorCoord(string nucleo, int ano, int semestre, decimal valor)
        {
            string sql = " Insert into LY_NUCLEO_VERBA(NUCLEO, ANO, SEMESTRE, VALOR) VALUES(?, ?, ?, ?)";
            IAE(sql, nucleo, ano, semestre, valor.SubstituirVirgulaPorPonto());
        }

        public static void DeletaValorCoord(string nucleo, int ano, int semestre, decimal valor)
        {
            if (!string.IsNullOrEmpty(valor.ToString()))
            {
                string sql = " delete from LY_NUCLEO_VERBA where nucleo = ? and ano = ? and semestre = ?";
                IAE(sql, nucleo, ano, semestre);
            }
        }

        public static void AlteraValorCoord(string nucleo, int ano, int semestre, decimal valor)
        {
            string sql = " Update LY_NUCLEO_VERBA Set VALOR = ?  where nucleo = ? and ano = ? and semestre = ?";
            IAE(sql, valor.SubstituirVirgulaPorPonto(), nucleo, ano, semestre);
        }

         #endregion

        public static bool Operacao(string unidade, int ano, int semestre)
        {
            string sql = "select 1 from LY_UNIDADE_ENSINO_VERBA where UNIDADE_ENS = ? AND ANO = ? AND SEMESTRE = ?";
            Int32 verbaUnidade = ExecutarFuncao(sql, unidade, ano, semestre);
            return (verbaUnidade == 1 ? true : false);
        }

        public static decimal ConsultarTotalGLPUtilizada(string ano, string semestre)
        {
            string sql = "SELECT (count(ht.tipo_aula) * (select valor from LY_VALOR_GLP where ANO = ? and SEMESTRE = ? )) as totalverbautilizada " + 
                            "FROM ly_aula_docente_tipo ht " +
                            "where ht.TIPO_AULA = 'GLP' " + 
                            "and ANO = ? and SEMESTRE = ? ";
            return ExecutarFuncaoDec(sql, ano, semestre, ano, semestre);
        }

        public static decimal ConsultarTotalGLPUtilizada(string coordenadoria, string ano, string semestre)
        {
            string sql = "SELECT (count(ht.tipo_aula) * (select valor from LY_VALOR_GLP where ANO = ? and SEMESTRE = ? )) as totalverbautilizada " + 
                            "FROM ly_aula_docente_tipo ht join LY_UNIDADES_ASSOCIADAS ua " +
                            " on ht.FACULDADE = ua.UNIDADE_FIS " +
                            " join LY_UNIDADE_ENSINO ue " + 
                            " on ua.UNIDADE_ENS = ue.UNIDADE_ENS " +
                            " where ht.TIPO_AULA = 'GLP' " +
                            " and ANO = ? and SEMESTRE = ? " +
                            " and ue.NUCLEO = ? ";
            return ExecutarFuncaoDec(sql, ano, semestre, ano, semestre, coordenadoria);
        }

        public static decimal ConsultarValorVerba(DbObject coordenadoria, string ano, string periodo)
        {
            string sql = "SELECT valor FROM LY_NUCLEO_VERBA WHERE ANO = ? AND SEMESTRE = ? AND NUCLEO = ? ";
            return ExecutarFuncaoDec(sql, ano, periodo, coordenadoria);
        }
    }
}
