using System;
using System.ComponentModel;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections;
using Techne.Lyceum.RN.Query;
using System.Linq;
using System.Collections.Generic;

namespace Techne.Lyceum.RN
{

    public class AbsorcaoUnidadeEnsino : RNBase
    {

        #region propriedades
        private string _unidadeEnsinodestinoid;
        public string UnidadeEnsinodestinoid
        {
            get { return _unidadeEnsinodestinoid; }
            set { _unidadeEnsinodestinoid = value; }
        }

        private string _unidadeEnsinoorigemid;
        public string UnidadeEnsinoorigemid
        {
            get { return _unidadeEnsinoorigemid; }
            set { _unidadeEnsinoorigemid = value; }
        }

        private string _cursoorigemid;
        public string Cursoorigemid
        {
            get { return _cursoorigemid; }
            set { _cursoorigemid = value; }
        }

        private string _turnoorigemid;
        public string Turnoorigemid
        {
            get { return _turnoorigemid; }
            set { _turnoorigemid = value; }
        }

        private string _serieorigemid;
        public string Serieorigemid
        {
            get { return _serieorigemid; }
            set { _serieorigemid = value; }
        }

        private DateTime _dataabsorcao;
        public DateTime Dataabsorcao
        {
            get { return _dataabsorcao; }
            set { _dataabsorcao = value; }
        }

        private DateTime _datacadastro;
        public DateTime Datacadastro
        {
            get { return _datacadastro; }
            set { _datacadastro = value; }
        }

        private string _matricula;
        public string Matricula
        {
            get { return _matricula; }
            set { _matricula = value; }
        }

        private string _nivelabsorcaoid;
        public string Nivelabsorcaoid
        {
            get { return _nivelabsorcaoid; }
            set { _nivelabsorcaoid = value; }
        }
        #endregion

        public static QueryTable ConsultarCurso(string UnidadaeEns)
        {
            return Consultar(@"SELECT DISTINCT C.*
                                        FROM LY_CURSO C (NOLOCK)
                               INNER JOIN LY_UNIDADE_ENSINO_CURSOS U (NOLOCK)
                                        ON (C.CURSO = U.CURSO
                               AND (U.DT_IMPLANTACAO <= GETDATE() OR U.DT_IMPLANTACAO IS NULL))
                               WHERE U.UNIDADE_ENS = ?
                               ORDER BY NOME", UnidadaeEns);
        }

		public static QueryTable ConsultarCursoAbsorvido(string UnidadeEns)
		{
			string sql = "SELECT DISTINCT C.* ";
			sql += "FROM LY_CURSO C (NOLOCK) ";
			sql += "INNER JOIN LY_UNIDADE_ENSINO_CURSOS U (NOLOCK) ";
            sql += "ON (C.CURSO = U.CURSO AND (U.DT_IMPLANTACAO <= GETDATE() OR u.DT_IMPLANTACAO is NULL)) ";
			sql += "WHERE U.UNIDADE_ENS = ? ";
			sql += "AND NOT EXISTS (SELECT 1 ";
			sql += "FROM SERIEABSORVIDA S (NOLOCK) ";
			sql += "WHERE S.UNIDADEENSINOORIGEMID = U.UNIDADE_ENS AND S.CURSOORIGEMID = U.CURSO AND S.NIVELABSORCAOID = 2) ";
			sql += "ORDER BY NOME";

			return Consultar(sql, UnidadeEns);
		}

        public static QueryTable ConsultarTurno(string UnidadaeEns, string Curso)
        {
            return Consultar(@"SELECT DISTINCT T.DESCRICAO, T.TURNO
                                        FROM LY_UNIDADE_ENSINO_CURSOS C (NOLOCK)
                               JOIN LY_TURNO T ON T.TURNO = C.TURNO
                               WHERE UNIDADE_ENS = ?
                               AND CURSO = ?
                               AND (C.DT_IMPLANTACAO <= GETDATE() OR C.DT_IMPLANTACAO IS NULL)
                               ORDER BY T.DESCRICAO", UnidadaeEns, Curso);
        }

		public static QueryTable ConsultarTurnoAbsorvido(string UnidadeEns, string curso)
		{
			string sql = "SELECT DISTINCT U.TURNO, T.DESCRICAO ";
			sql += "FROM LY_UNIDADE_ENSINO_CURSOS U (NOLOCK) ";
			sql += "JOIN LY_TURNO T ON T.TURNO = U.TURNO ";
			sql += "WHERE U.UNIDADE_ENS = ? ";
            sql += "AND (U.DT_IMPLANTACAO <= GETDATE() OR U.DT_IMPLANTACAO IS NULL) ";
			sql += "AND NOT EXISTS ( SELECT 1 ";
			sql += "FROM SERIEABSORVIDA S (NOLOCK) WHERE S.UNIDADEENSINOORIGEMID = U.UNIDADE_ENS ";
			sql += "AND (S.CURSOORIGEMID = U.CURSO OR S.CURSOORIGEMID IS NULL) ";
			sql += "AND S.TURNOORIGEMID = U.TURNO AND S.NIVELABSORCAOID = 3) ";

			if (curso != "Selecione")
			{
				sql += "AND U.CURSO = ? ";
			}

			sql += "ORDER BY U.TURNO ";

			if (curso != "Selecione")
				return Consultar(sql, UnidadeEns, curso);
			else
				return Consultar(sql, UnidadeEns);
		}

        public static QueryTable ConsultarSerie(string UnidadaeEns, string Curso, string Turno)
        {
            return Consultar(@"SELECT DISTINCT S.SERIE
                                        FROM LY_SERIE S
                                JOIN  LY_UNIDADE_ENSINO_CURSOS C (NOLOCK)
                                        ON C.CURSO = S.CURSO
                                WHERE C.UNIDADE_ENS = ?
                                AND S.CURSO = ?
                                AND S.TURNO = ?
                                ORDER BY
                                S.SERIE", UnidadaeEns, Curso, Turno);
        }

		public static QueryTable ConsultarSerieAbsorvida(string curso, string turno, string UnidadaeEns)
		{
			string sql = "SELECT DISTINCT U.SERIE ";
			sql += "FROM LY_SERIE U (NOLOCK) ";
			sql += "WHERE ";

			if (curso != "Selecione")
			{
				sql += "U.CURSO = ? AND ";
			}

			if (turno != "Selecione" && curso != "Selecione")
			{
				sql += "U.TURNO = ? AND ";
			}

			sql += "NOT EXISTS ( SELECT 1 ";
			sql += "FROM SERIEABSORVIDA S (NOLOCK) ";
			sql += "WHERE S.UNIDADEENSINOORIGEMID = ? ";
			sql += "AND S.CURSOORIGEMID = U.CURSO ";
			sql += "AND (S.TURNOORIGEMID = U.TURNO OR S.TURNOORIGEMID IS NULL) ";
			sql += "AND S.NIVELABSORCAOID = 4 AND S.SERIEORIGEMID = U.SERIE) ORDER BY U.SERIE ";

			if (UnidadaeEns != "Selecione" && curso != "Selecione" && turno == "Selecione")
				return Consultar(sql, curso, UnidadaeEns); 
			else if(UnidadaeEns!= "Selecione" && curso != "Selecione" && turno != "Selecione")
				return Consultar(sql, curso, turno, UnidadaeEns);
			else			
				return Consultar(sql, UnidadaeEns);
		}

        public static QueryTable ConsultarDadosAbsorvidos(string UnidadeDestino)
        {
            return Consultar(@"SELECT S.SERIEABSORVIDAID, UE.NOME_COMP, 
                                CASE S.NIVELABSORCAOID
                                WHEN 1 THEN 'Unidade Ensino'
                                WHEN 2 THEN 'Curso'
                                WHEN 3 THEN 'Turno'
                                WHEN 4 THEN 'Série'
                                END NIVELABSORCAOIDDESCR,
                                S.CURSOORIGEMID,
                                S.TURNOORIGEMID,
                                S.SERIEORIGEMID,
                                S.DATAABSORCAO
                                    FROM SERIEABSORVIDA S (NOLOCK)
                                INNER JOIN LY_UNIDADE_ENSINO UE (NOLOCK)
                                ON (S.UNIDADEENSINOORIGEMID = UE.UNIDADE_ENS)
                                LEFT OUTER JOIN LY_CURSO C (NOLOCK)
                                ON (S.CURSOORIGEMID = C.CURSO)
                                LEFT OUTER JOIN LY_TIPO_CURSO TC (NOLOCK)
                                ON (C.TIPO = TC.TIPO)
                                LEFT OUTER JOIN LY_TURNO T (NOLOCK)
                                ON (S.TURNOORIGEMID = T.TURNO)
                                WHERE S.UNIDADEENSINODESTINOID = ?
                                ORDER BY DATAABSORCAO DESC, UE.NOME_COMP, C.NOME, S.SERIEABSORVIDAID
                                ", UnidadeDestino);
        }

        public static bool ConsultaUnidadeAbsorvida(string UNIDADEENSINOORIGEMID)
        {
            System.Text.StringBuilder sql = new System.Text.StringBuilder();
//            sql.Append(@"SELECT UNIDADE_ENS, NOME_COMP, SETOR, CGC, SITUACAO, MUNICIPIO, NOME, REGIONAL
//						FROM VW_UNIDADE_ENSINO_SITUACAO_REGIONAL U (NOLOCK)
//						WHERE EXISTS (
//											SELECT 1
//											FROM SERIEABSORVIDA S (NOLOCK)
//											WHERE S.UNIDADEENSINOORIGEMID = U.UNIDADE_ENS
//											AND S.NIVELABSORCAOID = 1
//											AND S.UNIDADEENSINOORIGEMID = ?
//										 )");

			sql.Append(@"SELECT 1 FROM SERIEABSORVIDA S (NOLOCK)
						WHERE S.NIVELABSORCAOID = 1 
						AND S.UNIDADEENSINOORIGEMID = ?");

            int retorno;
            retorno = ExecutarFuncao(sql.ToString(), UNIDADEENSINOORIGEMID);

            if (retorno == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static void InserirUnidadeEnsinoAbsorvida(RN.AbsorcaoUnidadeEnsino Propriedades, DataContext context)
        {
            var ctx = DataContextBuilder.FromLyceum.UsingLock();
            try
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"INSERT INTO SERIEABSORVIDA(UNIDADEENSINODESTINOID, UNIDADEENSINOORIGEMID, CURSOORIGEMID, 
				                                                TURNOORIGEMID, SERIEORIGEMID, DATAABSORCAO, DATACADASTRO, 
					                                                MATRICULA, NIVELABSORCAOID)
                                VALUES  ( @UNIDADEENSINODESTINOID, @UNIDADEENSINOORIGEMID, @CURSOORIGEMID,
                                            @TURNOORIGEMID, @SERIEORIGEMID, @DATAABSORCAO, @DATACADASTRO,
                                                @MATRICULA, @NIVELABSORCAOID)"
                };

                contextQuery.Parameters.Add("@UNIDADEENSINODESTINOID ", Propriedades.UnidadeEnsinodestinoid);
                contextQuery.Parameters.Add("@UNIDADEENSINOORIGEMID", Propriedades.UnidadeEnsinoorigemid);
                if (Propriedades.Cursoorigemid == string.Empty || Propriedades.Cursoorigemid == "Selecione")
                {
                    contextQuery.Parameters.Add("@CURSOORIGEMID", DBNull.Value);
                }
                else
                {
                    contextQuery.Parameters.Add("@CURSOORIGEMID", Propriedades.Cursoorigemid);
                }
                if (Propriedades.Turnoorigemid == string.Empty || Propriedades.Turnoorigemid == "Selecione")
                {
                    contextQuery.Parameters.Add("@TURNOORIGEMID", DBNull.Value);
                }
                else
                {
                    contextQuery.Parameters.Add("@TURNOORIGEMID", Propriedades.Turnoorigemid);
                }
                if (Propriedades.Serieorigemid == string.Empty || Propriedades.Serieorigemid == "Selecione")
                {
                    contextQuery.Parameters.Add("@SERIEORIGEMID", DBNull.Value);
                }
                else
                {
                    contextQuery.Parameters.Add("@SERIEORIGEMID", Propriedades.Serieorigemid);
                }
                contextQuery.Parameters.Add("@DATAABSORCAO", Propriedades.Dataabsorcao);
                contextQuery.Parameters.Add("@DATACADASTRO ", Propriedades.Datacadastro);
                contextQuery.Parameters.Add("@MATRICULA ", Propriedades.Matricula);
                contextQuery.Parameters.Add("@NIVELABSORCAOID", Propriedades.Nivelabsorcaoid);

                ExecutarAlteracao(contextQuery);
            }
            catch (Exception)
            {
                ctx.Abandon();
                throw;
            }

        }

        public static void DeletaUnidadeAbsorvida(string SERIEABSORVIDAID)
        {
            var ctx = DataContextBuilder.FromLyceum.UsingLock();
            try
            {
                var contextQuery = new ContextQuery(
                           @"DELETE 
	                            FROM SERIEABSORVIDA
	                    	        WHERE SERIEABSORVIDAID = @SERIEABSORVIDAID");
                contextQuery.Parameters.Add("@SERIEABSORVIDAID", SERIEABSORVIDAID);
                ExecutarAlteracao(contextQuery);
            }
            catch (Exception)
            {
                ctx.Abandon();
                throw;
            }
        }

        public void Delete(object SERIEABSORVIDAID)
        {
            //this.LimparCampos();
        }
    }
}
