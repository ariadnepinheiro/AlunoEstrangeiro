using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proderj.DOL.Repository;
using Proderj.DOL.Domain;
using AutoMapper;
using System.Dynamic;

namespace Proderj.DOL.Service
{
	public class LY_UNIDADE_ENSINOService : ILY_UNIDADE_ENSINOService
	{
        private ILY_UNIDADE_ENSINORepository repositorioUnidadeEnsino;

        public LY_UNIDADE_ENSINOService(ILY_UNIDADE_ENSINORepository repositorioUnidadeEnsino)
		{
			this.repositorioUnidadeEnsino = repositorioUnidadeEnsino;
		}

        public IDictionary<string, string> DicionarioPor(int? id_regional, string municipio)
        {
            string sql = @"
            SELECT  UNIDADE_ENS, NOME_COMP
            FROM    ( SELECT    UE.UNIDADE_ENS ,
                                UE.NOME_COMP ,
                                UE.SETOR ,
                                UE.CGC ,
                                UE.NUCLEO ,
                                UE.MUNICIPIO ,
					            ue.SIT_FUNCIONAMENTO,
                                ( SELECT TOP 1
                                            SITUACAO
                                    FROM      LY_UNIDADE_ENSINO_SITUACAO UES
                                    WHERE     UES.UNIDADE_ENS = UE.UNIDADE_ENS
                                    ORDER BY  DT_SITUACAO DESC
                                ) AS SITUACAO ,
                                UE.ID_REGIONAL
                        FROM      DBO.LY_UNIDADE_ENSINO UE
                    ) AS T
            WHERE   SITUACAO = 'ESTADUAL'
            and SIT_FUNCIONAMENTO = 'EmAtividade'
            and ID_REGIONAL = :ID_REGIONAL
            and MUNICIPIO = :MUNICIPIO
            ";

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add(new KeyValuePair<string, object>("ID_REGIONAL", id_regional));
            parameters.Add(new KeyValuePair<string, object>("MUNICIPIO", municipio));

            return repositorioUnidadeEnsino
                .CreateSQLQuery<DTOLY_UNIDADE_ENSINO>(sql, parameters)
                .ToDictionary(kvp => kvp.UNIDADE_ENS as string, kvp => kvp.NOME_COMP as string);
        }

        public IList<dynamic> ListaPor(int? id_regional, string municipio)
        {
            string sql = @"
            SELECT  UNIDADE_ENS, NOME_COMP
            FROM    ( SELECT    UE.UNIDADE_ENS ,
                                UE.NOME_COMP ,
                                UE.SETOR ,
                                UE.CGC ,
                                UE.NUCLEO ,
                                UE.MUNICIPIO ,
					            ue.SIT_FUNCIONAMENTO,
                                ( SELECT TOP 1
                                            SITUACAO
                                    FROM      LY_UNIDADE_ENSINO_SITUACAO UES
                                    WHERE     UES.UNIDADE_ENS = UE.UNIDADE_ENS
                                    ORDER BY  DT_SITUACAO DESC
                                ) AS SITUACAO ,
                                UE.ID_REGIONAL
                        FROM      DBO.LY_UNIDADE_ENSINO UE
                    ) AS T
            WHERE   SITUACAO = 'ESTADUAL'
            and SIT_FUNCIONAMENTO = 'EmAtividade'
            and ID_REGIONAL = :ID_REGIONAL
            and MUNICIPIO = :MUNICIPIO
            ORDER BY NOME_COMP
            ";

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add(new KeyValuePair<string, object>("ID_REGIONAL", id_regional));
            parameters.Add(new KeyValuePair<string, object>("MUNICIPIO", municipio));

            return repositorioUnidadeEnsino
                .CreateSQLQuery<DTOLY_UNIDADE_ENSINO>(sql, parameters)
                .Select(s => (dynamic)new { s.UNIDADE_ENS, s.NOME_COMP })
                .ToList();
        }

        public IList<string> ListaCodigoPor(int? id_regional, string municipio)
        {
            string sql = @"
            SELECT  UNIDADE_ENS
            FROM    ( SELECT    UE.UNIDADE_ENS ,
                                UE.NOME_COMP ,
                                UE.SETOR ,
                                UE.CGC ,
                                UE.NUCLEO ,
                                UE.MUNICIPIO ,
					            ue.SIT_FUNCIONAMENTO,
                                ( SELECT TOP 1
                                            SITUACAO
                                    FROM      LY_UNIDADE_ENSINO_SITUACAO UES
                                    WHERE     UES.UNIDADE_ENS = UE.UNIDADE_ENS
                                    ORDER BY  DT_SITUACAO DESC
                                ) AS SITUACAO ,
                                UE.ID_REGIONAL
                        FROM      DBO.LY_UNIDADE_ENSINO UE
                    ) AS T
            WHERE   SITUACAO = 'ESTADUAL'
            and SIT_FUNCIONAMENTO = 'EmAtividade'
            and ID_REGIONAL = :ID_REGIONAL
            and MUNICIPIO = :MUNICIPIO
            ";

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add(new KeyValuePair<string, object>("ID_REGIONAL", id_regional));
            parameters.Add(new KeyValuePair<string, object>("MUNICIPIO", municipio));

            return repositorioUnidadeEnsino
                .CreateSQLQuery<DTOLY_UNIDADE_ENSINO>(sql, parameters)
                .Select(s => s.UNIDADE_ENS)
                .ToList();
        }

        public dynamic ObtemRegionalEMunicipioPor(string unidadeEnsino)
        {
            string sql = @"
            SELECT  ID_REGIONAL, MUNICIPIO
            FROM    ( SELECT    UE.UNIDADE_ENS ,
                                UE.NOME_COMP ,
                                UE.SETOR ,
                                UE.CGC ,
                                UE.NUCLEO ,
                                UE.MUNICIPIO ,
					            ue.SIT_FUNCIONAMENTO,
                                ( SELECT TOP 1
                                            SITUACAO
                                    FROM      LY_UNIDADE_ENSINO_SITUACAO UES
                                    WHERE     UES.UNIDADE_ENS = UE.UNIDADE_ENS
                                    ORDER BY  DT_SITUACAO DESC
                                ) AS SITUACAO ,
                                UE.ID_REGIONAL
                        FROM      DBO.LY_UNIDADE_ENSINO UE
                    ) AS T
            WHERE   SITUACAO = 'ESTADUAL'
            and SIT_FUNCIONAMENTO = 'EmAtividade'
            and UNIDADE_ENS = :UNIDADE_ENS
            ";

            IDictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add(new KeyValuePair<string, object>("UNIDADE_ENS", unidadeEnsino));

            return repositorioUnidadeEnsino
                .CreateSQLQuery<DTOLY_UNIDADE_ENSINO>(sql, parameters)
                .Select(s => (dynamic)new { s.ID_REGIONAL, s.MUNICIPIO })
                .FirstOrDefault();

            //return repositorioUnidadeEnsino.ListaQueryable()
            //    .Where(q => q.UNIDADE_ENS == unidadeEnsino)
            //    .Select(s => new { s.ID_REGIONAL, s.MUNICIPIO })
            //    .FirstOrDefault();
        }

        public IList<DTOListaMUNICIPIOPorID_REGIONAL> ListaMunicipioPor(int id_regional)
        {
            return repositorioUnidadeEnsino.ListaMunicipioPor<DTOListaMUNICIPIOPorID_REGIONAL>(id_regional);
        }
	}
}
