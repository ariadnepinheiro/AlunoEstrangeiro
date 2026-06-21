using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;

namespace Techne.Lyceum.RN
{
    public class UnidadesAssociadas : RNBase
    {
        public static string ConsultarUnidadeAssociada(string usuario)
        {
            string sql = @"select unidade_ens from LY_UNIDADES_ASSOCIADAS ua
                            inner join LY_USUARIO_UNIDADE_FIS  uu
                            on uu.UNIDADE_FIS = ua.UNIDADE_FIS
                            where USUARIO = ?";
            return ConsultarCampo(sql, usuario);
        }

        public void Insere(DataContext contexto, LyUnidadesAssociadas unidadesAssociadas)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO DBO.LY_UNIDADES_ASSOCIADAS 
                                                        (UNIDADE_ENS, 
                                                         UNIDADE_FIS) 
                                            VALUES      ( @UNIDADE_ENS, 
                                                          @UNIDADE_FIS )  ";

            contextQuery.Parameters.Add("@UNIDADE_ENS", TechneDbType.T_CODIGO, unidadesAssociadas.UnidadeEns);
            contextQuery.Parameters.Add("@UNIDADE_FIS", TechneDbType.T_CODIGO, unidadesAssociadas.UnidadeFis);

            contexto.ApplyModifications(contextQuery);
        }
    }
}
