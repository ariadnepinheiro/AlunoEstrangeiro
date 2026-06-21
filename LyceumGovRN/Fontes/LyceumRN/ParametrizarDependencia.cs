using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN
{
    public class ParametrizarDependencia : RNBase
    {
        public static DataTable BuscaDependencias()
        {

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT DISTINCT  TIPO_DEPEND, NOME
		                             FROM LY_TIPO_DEPENDENCIA
                                     WHERE TIPO_DEPEND <> 'SALA'
		                             AND TIPO_DEPEND <> 'SALAAEE'"

                };
                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarDependenciaPorCurso(string LYCURSOID)
        {

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT LYCURSOID, LYTIPODEPENDENCIAID
		                               FROM LYCURSO_LYTIPODEPENDENCIA 
                                       WHERE LYCURSOID = @LYCURSOID"
                };
                contextQuery.Parameters.Add("@LYCURSOID", LYCURSOID);

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static void Inserir(LyCursoLyTipoDependencia Tipodepend)
        {
            var ctx = DataContextBuilder.FromLyceum.UsingLock();
            try
            {
                var contextQuery = new ContextQuery(
                    @" INSERT  INTO dbo.LYCURSO_LYTIPODEPENDENCIA
                            ( LYCURSOID, LYTIPODEPENDENCIAID
                            )
                    VALUES  ( @LYCURSOID ,
                              @LYTIPODEPENDENCIAID
                            ) ");

                contextQuery.Parameters.Add("@LYCURSOID", Tipodepend.ID_CURSO);
                contextQuery.Parameters.Add("@LYTIPODEPENDENCIAID", Tipodepend.DEPENDENCIA);

                ExecutarAlteracao(contextQuery);
            }
            catch (Exception)
            {
                ctx.Abandon();
            }
        }

        public static void Update(LyCursoLyTipoDependencia Tipodepend)
        {
            var ctx = DataContextBuilder.FromLyceum.UsingLock();
            try
            {
                var contextQuery = new ContextQuery(
                    @"UPDATE  dbo.LYCURSO_LYTIPODEPENDENCIA
                            SET LYTIPODEPENDENCIAID = @DEPENDENCIATROCA 
                            WHERE LYCURSOID = @LYCURSOID
                            AND LYTIPODEPENDENCIAID = @LYTIPODEPENDENCIAID");

                contextQuery.Parameters.Add("@LYTIPODEPENDENCIAID", Tipodepend.DEPENDENCIA);
                contextQuery.Parameters.Add("@LYCURSOID", Tipodepend.ID_CURSO);
                contextQuery.Parameters.Add("@DEPENDENCIATROCA", Tipodepend.DEPENDENCIATROCA);
              
                ExecutarAlteracao(contextQuery);
            }
            catch (Exception)
            {
                ctx.Abandon();
            }
        }

        public static void Remover(string LYCURSOID, string LYTIPODEPENDENCIAID)
        {
            if (string.IsNullOrEmpty(LYCURSOID)
                || string.IsNullOrEmpty(LYTIPODEPENDENCIAID))
            {
                return;
            }

            var contextQuery = new ContextQuery(
                @"DELETE  LYCURSO_LYTIPODEPENDENCIA
                     WHERE   LYCURSOID = @LYCURSOID
                            AND LYTIPODEPENDENCIAID = @LYTIPODEPENDENCIAID ");

            contextQuery.Parameters.Add("@LYCURSOID", LYCURSOID);
            contextQuery.Parameters.Add("@LYTIPODEPENDENCIAID", LYTIPODEPENDENCIAID);

            ExecutarAlteracao(contextQuery);
        }

        public static DataTable VerificaAssociacaoDependencia(string TIPO_DEPEND)
        {

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT TOP 1 CTD.LYTIPODEPENDENCIAID
		                              FROM LYCURSO_LYTIPODEPENDENCIA CTD
                                 INNER JOIN LY_DEPENDENCIA D 
                                 ON CTD.LYTIPODEPENDENCIAID = D.TIPO_DEPEND
                                WHERE  D.TIPO_DEPEND = @TIPO_DEPEND"

                };
                contextQuery.Parameters.Add("@TIPO_DEPEND", TIPO_DEPEND);
                return ctx.GetDataTable(contextQuery);
            }
        }


        public static ValidacaoDados Validar(LyCursoLyTipoDependencia SalaAlternativa)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (SalaAlternativa == null)
            {
                return validacaoDados;
            }

             DataTable Associada = RN.ParametrizarDependencia.VerificaAssociacaoDependencia(SalaAlternativa.DEPENDENCIA);

             if (Associada.Rows.Count > 0)
             {
                 mensagens.Add("Não é possivel remover o registro pois existem associações.");
             }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

    }
}
