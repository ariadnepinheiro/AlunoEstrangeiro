using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using System;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN
{
    public class TipoDependencia : RNBase
    {
        public static DataTable Listar()
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT  TIPO_DEPEND,NOME
                                FROM    dbo.LY_TIPO_DEPENDENCIA 
                                ORDER BY NOME"
                };

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarSalaAula()
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT  TIPO_DEPEND,NOME
                                FROM    dbo.LY_TIPO_DEPENDENCIA 
                                WHERE TIPO_DEPEND = 'SALA'
                                ORDER BY NOME"
                };

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarBanheiros()
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT  TIPO_DEPEND,NOME
                                FROM    dbo.LY_TIPO_DEPENDENCIA 
                                WHERE TIPO_DEPEND LIKE 'BANHEIRO%'
                                ORDER BY NOME"
                };

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static DataTable ListarSalaRecurso()
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @"SELECT  TIPO_DEPEND,NOME
                                FROM    dbo.LY_TIPO_DEPENDENCIA 
                                WHERE TIPO_DEPEND = 'SALAAEE'
                                ORDER BY NOME"
                };

                return ctx.GetDataTable(contextQuery);
            }
        }
    }
}
