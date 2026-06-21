using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.CR;
using Seeduc.Infra.Data;
using System.Configuration;

namespace Techne.Lyceum.RN.DTOs
{
    public class ConsolidaTurnosDoMatriculaFacil
    {
        #region Propriedades

        public String UnidadeEnsino { get; set; }
        public String NomeUnidadeEnsino { get; set; }
        public String Ano { get; set; }
        public String Periodo { get; set; }
        public IList<DetalhesConsolidaTurnosDoMatriculaFacil> DetalhesConsolidaTurnosDoMatriculaFacil { get; set; }

        #endregion

        #region Construtores

        public ConsolidaTurnosDoMatriculaFacil()
        {
            this.DetalhesConsolidaTurnosDoMatriculaFacil = new List<DetalhesConsolidaTurnosDoMatriculaFacil>();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void ExecutaConsolidaTurnosDoMatriculaFacil(ConsolidaTurnosDoMatriculaFacil obj)
        {
            ConsolidaTurnosDoMatriculaFacil objConsolidaTurnosDoMatriculaFacil =
                new ConsolidaTurnosDoMatriculaFacil();
            
            var sufixo = ConfigurationManager.AppSettings["VersaoSufixo"] ?? string.Empty;

            if (obj.DetalhesConsolidaTurnosDoMatriculaFacil.Count > 0)
            {
                ws_ConsolidaTurnosDoMatriculadoFacil.ServicoIntegracaoSisMati servico
                = new ws_ConsolidaTurnosDoMatriculadoFacil.ServicoIntegracaoSisMati();

                //Verifica se o ambiente é de produção (P).
                //Senão, o sistema usa o serviço de desenvolvimento (D) e/ou homologação (H)
                if (sufixo.Equals("P"))
                {
                    //URL de Produção
                    servico.Url = "http://www.matriculaconsultas.rj.gov.br/servicointegracaosismati.asmx";
                }
                else
                {
                    //URL de Desenvolvimento/Homologação
                    servico.Url = "http://www.matriculaconsultas.proderj.rj.gov.br/ServicoIntegracaoSisMati.asmx";
                }

                try
                {
                    foreach (var item in obj.DetalhesConsolidaTurnosDoMatriculaFacil)
                    {
                        //Considera os seguintes cursos e series
                        if ((item.Curso.Equals("0002.31") && item.Serie.Equals("1"))
                           || (item.Curso.Equals("0003.31") && item.Serie.Equals("1"))
                           || (item.Curso.Equals("0002.44") && item.Serie.Equals("1"))
                           || (item.Curso.Equals("0092.39") && item.Serie.Equals("1"))
                           || (item.Curso.Equals("0002.33") && item.Serie.Equals("1"))
                           || (item.Curso.Equals("0401.32") && item.Serie.Equals("1"))
                           || (item.Curso.Equals("0602.32") && item.Serie.Equals("1"))
                           || (item.Curso.Equals("0504.32") && item.Serie.Equals("1"))
                           || (item.Curso.Equals("1204.32") && item.Serie.Equals("1"))
                           || (item.Curso.Equals("0001.21") && item.Serie.Equals("6"))
                           || (item.Curso.Equals("0001.42") && item.Serie.Equals("6"))
                           || (item.Curso.Equals("0091.29") && item.Serie.Equals("1")))
                        {
                            //Não considera as unidades munipalizadas para os seguintes cursos e série
                            if (item.Curso.Equals("0001.21") || item.Curso.Equals("0001.42") && item.Serie.Equals("6"))
                            {
                                bool EhMunicipalizada = VerificaUnidadeMunicipaliza(obj.UnidadeEnsino);

                                if (!EhMunicipalizada)
                                {
                                    servico.ConsolidaTurnosDoMatriculaFacil(obj.UnidadeEnsino
                                    , obj.NomeUnidadeEnsino
                                    , item.Curso
                                    , item.Serie
                                    , item.Turno
                                    , item.ModalidadeCurso
                                    , item.TipoCurso
                                    , obj.Ano
                                    , obj.Periodo
                                    , ref item.TipoRetorno
                                    , ref item.DescricaoRetorno
                                    , item.TipoOperacao);

                                    objConsolidaTurnosDoMatriculaFacil.DetalhesConsolidaTurnosDoMatriculaFacil.Add(item);
                                }
                            }
                            else
                            {
                                servico.ConsolidaTurnosDoMatriculaFacil(obj.UnidadeEnsino
                                    , obj.NomeUnidadeEnsino
                                    , item.Curso
                                    , item.Serie
                                    , item.Turno
                                    , item.ModalidadeCurso
                                    , item.TipoCurso
                                    , obj.Ano
                                    , obj.Periodo
                                    , ref item.TipoRetorno
                                    , ref item.DescricaoRetorno
                                    , item.TipoOperacao);

                                objConsolidaTurnosDoMatriculaFacil.DetalhesConsolidaTurnosDoMatriculaFacil.Add(item);
                            }
                        }
                    }

                    if (objConsolidaTurnosDoMatriculaFacil.DetalhesConsolidaTurnosDoMatriculaFacil.Count > 0)
                    {
                        objConsolidaTurnosDoMatriculaFacil.UnidadeEnsino = obj.UnidadeEnsino;
                        objConsolidaTurnosDoMatriculaFacil.NomeUnidadeEnsino = obj.NomeUnidadeEnsino;
                        objConsolidaTurnosDoMatriculaFacil.Ano = obj.Ano;
                        objConsolidaTurnosDoMatriculaFacil.Periodo = obj.Periodo;

                        var email = Util.Email.MontaEmailMatriculaFacil(objConsolidaTurnosDoMatriculaFacil);

#if !DEBUG
                        Util.Email.EnviarMail(email);
#endif
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UnidadeEnsino"></param>
        /// <param name="Ano"></param>
        /// <param name="Periodo"></param>
        /// <param name="Curso"></param>
        /// <param name="Serie"></param>
        /// <returns></returns>
        [Obsolete()]
        public IList<DetalhesConsolidaTurnosDoMatriculaFacil> RetonaTurnosVagasNovasPor(string UnidadeEnsino
            , string Ano
            , string Periodo
            , string Curso
            , string Serie)
        {
            DetalhesConsolidaTurnosDoMatriculaFacil obj = null;
            IList<DetalhesConsolidaTurnosDoMatriculaFacil> listaConsolidaTurnosDoMatriculaFacil = null;
            var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            var contextQuery = new ContextQuery
                {
                    Command = @"
                                select a.ano, a.periodo, t.censo, t.turno, a.curso, a.serie
                                from TCE_CTV_CONF_TURNO t, TCE_CTV_AGENDA_CONF_TURNO_VAGA a
                                where t.CENSO = @CENSO
                                and t.NOVO = 1
                                and t.CONFIRMADA = 1
                                and a.ANO = @ANO
                                and a.PERIODO = @PERIODO
                                and a.CURSO = @CURSO
                                and a.SERIE = @SERIE
                                and t.ID_AGENDA_CONF_TURNO_VAGA = a.ID_AGENDA_CONF_TURNO_VAGA"
                };

            contextQuery.Parameters.Add("@CENSO",UnidadeEnsino);
            contextQuery.Parameters.Add("@ANO", Ano);
            contextQuery.Parameters.Add("@PERIODO", Periodo);
            contextQuery.Parameters.Add("@CURSO", Curso);
            contextQuery.Parameters.Add("@SERIE", Serie);

            try
            {
                var retorno = ctx.GetDataReader(contextQuery);
                
                listaConsolidaTurnosDoMatriculaFacil = new List<DetalhesConsolidaTurnosDoMatriculaFacil>();

                while (retorno.Read())
                {
                    obj = new DetalhesConsolidaTurnosDoMatriculaFacil();

                    obj.Curso = retorno["curso"].ToString();
                    obj.Serie = retorno["serie"].ToString();
                    obj.Turno = retorno["turno"].ToString();
                    //obj.ModalidadeCurso = retorno["turno"].ToString();

                    listaConsolidaTurnosDoMatriculaFacil.Add(obj);
                }

                return listaConsolidaTurnosDoMatriculaFacil;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UnidadeEnsino"></param>
        /// <param name="Ano"></param>
        /// <param name="Periodo"></param>
        /// <param name="Serie"></param>
        /// <param name="Curso"></param>
        /// <param name="TipoOperacao"></param>
        /// <returns></returns>
        public IList<DetalhesConsolidaTurnosDoMatriculaFacil> RetornaDetalhesRestricaoGeralPor(string UnidadeEnsino, int Ano, int Periodo, string Serie, string Curso, string TipoOperacao)
        {
            DetalhesConsolidaTurnosDoMatriculaFacil obj = null;
            IList<DetalhesConsolidaTurnosDoMatriculaFacil> listaConsolidaTurnosDoMatriculaFacil = null;
            var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            var contextQuery = new ContextQuery
            {
                Command = @"
                                  SELECT A.ano,
                                           A.periodo,
                                           T.censo,
                                           T.turno AS TURNO,
                                           A.curso AS CURSO,
                                           A.serie AS SERIE,
                                           C.nome AS NOMECURSO,
                                           C.tipo AS TIPOCURSO,
                                           TC.descricao DESCRICAOTIPO,
                                           C.modalidade AS MODALIDADE,
                                           ue.id_regional
                                    FROM   tce_ctv_conf_turno T,
                                           tce_ctv_agenda_conf_turno_vaga A,
                                           ly_curso C,
                                           ly_tipo_curso TC,
                                           ly_unidade_ensino ue
                                    WHERE  T.censo = @CENSO
                                           AND T.novo = 1
                                           AND A.ano = @ANO
                                           AND A.periodo = @PERIODO
                                           AND A.curso = @CURSO
                                           AND A.serie = @SERIE
                                           AND T.id_agenda_conf_turno_vaga = A.id_agenda_conf_turno_vaga
                                           AND A.curso = C.curso
                                           AND C.tipo = TC.tipo
                                           AND t.censo = ue.unidade_ens
                                           AND ue.id_regional <> 5
                                             "
            };

            contextQuery.Parameters.Add("@CENSO", UnidadeEnsino);
            contextQuery.Parameters.Add("@ANO", Ano);
            contextQuery.Parameters.Add("@PERIODO", Periodo);
            contextQuery.Parameters.Add("@SERIE", Serie);
            contextQuery.Parameters.Add("@CURSO", Curso);

            try
            {
                var retorno = ctx.GetDataReader(contextQuery);

                listaConsolidaTurnosDoMatriculaFacil = new List<DetalhesConsolidaTurnosDoMatriculaFacil>();

                while (retorno.Read())
                {
                    obj = new DetalhesConsolidaTurnosDoMatriculaFacil();

                    obj.Curso = retorno["CURSO"] == DBNull.Value ? string.Empty : retorno["CURSO"].ToString();
                    obj.Serie = retorno["SERIE"] == DBNull.Value ? string.Empty : retorno["SERIE"].ToString();
                    obj.Turno = retorno["TURNO"] == DBNull.Value ? string.Empty : retorno["TURNO"].ToString();
                    obj.NomeCurso = retorno["NOMECURSO"] == DBNull.Value ? string.Empty : retorno["NOMECURSO"].ToString();
                    obj.TipoCurso = retorno["TIPOCURSO"] == DBNull.Value ? string.Empty : retorno["TIPOCURSO"].ToString();
                    obj.ModalidadeCurso = retorno["MODALIDADE"] == DBNull.Value ? string.Empty : retorno["MODALIDADE"].ToString();
                    obj.TipoOperacao = TipoOperacao;

                    listaConsolidaTurnosDoMatriculaFacil.Add(obj);
                }

                return listaConsolidaTurnosDoMatriculaFacil;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UnidadeEnsino"></param>
        /// <param name="Ano"></param>
        /// <param name="Periodo"></param>
        /// <param name="TipoOperacao"></param>
        /// <returns></returns>
        public IList<DetalhesConsolidaTurnosDoMatriculaFacil> RetornaRestricaoGeral(string UnidadeEnsino, int Ano, int Periodo, string TipoOperacao)
        {
            DetalhesConsolidaTurnosDoMatriculaFacil obj = null;
            IList<DetalhesConsolidaTurnosDoMatriculaFacil> listaConsolidaTurnosDoMatriculaFacil = null;
            var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            var contextQuery = new ContextQuery
            {
                Command = @"
                                  SELECT A.ano,
                                           A.periodo,
                                           T.censo,
                                           T.turno AS TURNO,
                                           A.curso AS CURSO,
                                           A.serie AS SERIE,
                                           C.nome AS NOMECURSO,
                                           C.tipo AS TIPOCURSO,
                                           TC.descricao DESCRICAOTIPO,
                                           C.modalidade AS MODALIDADE,
                                           ue.id_regional
                                    FROM   tce_ctv_conf_turno T,
                                           tce_ctv_agenda_conf_turno_vaga A,
                                           ly_curso C,
                                           ly_tipo_curso TC,
                                           ly_unidade_ensino ue
                                    WHERE  T.censo = @CENSO
                                           AND T.novo = 1
                                           AND T.confirmada = 1
                                           AND A.ano = @ANO
                                           AND A.periodo = @PERIODO
                                           AND T.id_agenda_conf_turno_vaga = A.id_agenda_conf_turno_vaga
                                           AND A.curso = C.curso
                                           AND C.tipo = TC.tipo
                                           AND t.censo = ue.unidade_ens
                                           AND ue.id_regional <> 5
                                             "
            };

            contextQuery.Parameters.Add("@CENSO", UnidadeEnsino);
            contextQuery.Parameters.Add("@ANO", Ano);
            contextQuery.Parameters.Add("@PERIODO", Periodo);

            try
            {
                var retorno = ctx.GetDataReader(contextQuery);

                listaConsolidaTurnosDoMatriculaFacil = new List<DetalhesConsolidaTurnosDoMatriculaFacil>();

                while (retorno.Read())
                {
                    obj = new DetalhesConsolidaTurnosDoMatriculaFacil();

                    obj.Curso = retorno["CURSO"] == DBNull.Value ? string.Empty : retorno["CURSO"].ToString();
                    obj.Serie = retorno["SERIE"] == DBNull.Value ? string.Empty : retorno["SERIE"].ToString();
                    obj.Turno = retorno["TURNO"] == DBNull.Value ? string.Empty : retorno["TURNO"].ToString();
                    obj.NomeCurso = retorno["NOMECURSO"] == DBNull.Value ? string.Empty : retorno["NOMECURSO"].ToString();
                    obj.TipoCurso = retorno["TIPOCURSO"] == DBNull.Value ? string.Empty : retorno["TIPOCURSO"].ToString();
                    obj.ModalidadeCurso = retorno["MODALIDADE"] == DBNull.Value ? string.Empty : retorno["MODALIDADE"].ToString();
                    obj.TipoOperacao = TipoOperacao;

                    listaConsolidaTurnosDoMatriculaFacil.Add(obj);
                }

                return listaConsolidaTurnosDoMatriculaFacil;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UnidadeEnsino"></param>
        /// <param name="Ano"></param>
        /// <param name="Periodo"></param>
        /// <param name="Curso"></param>
        /// <param name="Serie"></param>
        /// <returns></returns>
        [Obsolete()]
        public IList<ConsolidaTurnosDoMatriculaFacil> RetornaRestricaoGeral(string UnidadeEnsino, int Ano, int Periodo, string Curso, string Serie)
        {
            ConsolidaTurnosDoMatriculaFacil obj = null;
            IList<ConsolidaTurnosDoMatriculaFacil> listaConsolidaTurnosDoMatriculaFacil = null;
            var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            var contextQuery = new ContextQuery
            {
                Command = @"
                                  SELECT A.ano AS ANO,
                                           A.periodo AS PERIODO,
                                           T.censo AS CENSO,
                                           ue.nome_abrev AS NOMEUNIDADEENSINO,
                                           T.turno AS TURNO,
                                           A.curso AS CURSO,
                                           A.serie AS SERIE,
                                           C.nome AS NOMECURSO,
                                           C.tipo AS TIPOCURSO,
                                           TC.descricao DESCRICAOTIPO,
                                           C.modalidade AS MODALIDADE,
                                           ue.id_regional
                                    FROM   tce_ctv_conf_turno T,
                                           tce_ctv_agenda_conf_turno_vaga A,
                                           ly_curso C,
                                           ly_tipo_curso TC,
                                           ly_unidade_ensino ue
                                    WHERE  T.censo = @CENSO
                                           AND T.novo = 1
                                           AND T.confirmada = 1
                                           AND A.ano = @ANO
                                           AND A.periodo = @PERIODO
                                           AND T.id_agenda_conf_turno_vaga = A.id_agenda_conf_turno_vaga
                                           AND A.curso = C.curso
                                           AND C.tipo = TC.tipo
                                           AND t.censo = ue.unidade_ens
                                           AND ue.id_regional <> 5
                                             "
            };

            contextQuery.Parameters.Add("@CENSO", UnidadeEnsino);
            contextQuery.Parameters.Add("@ANO", Ano);
            contextQuery.Parameters.Add("@PERIODO", Periodo);
            contextQuery.Parameters.Add("@CURSO", Curso);
            contextQuery.Parameters.Add("@SERIE", Serie);

            try
            {
                var retorno = ctx.GetDataReader(contextQuery);

                listaConsolidaTurnosDoMatriculaFacil = new List<ConsolidaTurnosDoMatriculaFacil>();

                while (retorno.Read())
                {
                    obj = new ConsolidaTurnosDoMatriculaFacil();

                    obj.Ano = retorno["CURSO"] == DBNull.Value ? string.Empty : retorno["CURSO"].ToString();
                    obj.Periodo = retorno["SERIE"] == DBNull.Value ? string.Empty : retorno["SERIE"].ToString();
                    obj.UnidadeEnsino = retorno["TURNO"] == DBNull.Value ? string.Empty : retorno["TURNO"].ToString();
                    obj.NomeUnidadeEnsino = retorno["NOMEUNIDADEENSINO"] == DBNull.Value ? string.Empty : retorno["NOMEUNIDADEENSINO"].ToString();
                    obj.DetalhesConsolidaTurnosDoMatriculaFacil[0].Curso = retorno["curso"] == DBNull.Value ? string.Empty : retorno["curso"].ToString();
                    obj.DetalhesConsolidaTurnosDoMatriculaFacil[0].Serie = retorno["serie"] == DBNull.Value ? string.Empty : retorno["serie"].ToString();
                    obj.DetalhesConsolidaTurnosDoMatriculaFacil[0].Turno = retorno["turno"] == DBNull.Value ? string.Empty : retorno["turno"].ToString();
                    obj.DetalhesConsolidaTurnosDoMatriculaFacil[0].ModalidadeCurso = retorno["MODALIDADE"] == DBNull.Value ? string.Empty : retorno["MODALIDADE"].ToString();
                    obj.DetalhesConsolidaTurnosDoMatriculaFacil[0].TipoCurso = retorno["TIPOCURSO"] == DBNull.Value ? string.Empty : retorno["TIPOCURSO"].ToString();

                    listaConsolidaTurnosDoMatriculaFacil.Add(obj);
                }

                return listaConsolidaTurnosDoMatriculaFacil;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UnidadeEnsino"></param>
        /// <returns></returns>
        public bool VerificaUnidadeMunicipaliza(string UnidadeEnsino)
        {
            var ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            bool blnRet = false;

            var contextQuery = new ContextQuery
            {
                Command = @"
                                    SELECT DISTINCT us.unidade_ens
                                    FROM   ly_unidade_ensino_situacao us
                                    WHERE  us.situacao = 'MUNICIPALIZADA'
                                           AND us.dt_situacao = (SELECT Max(us2.dt_situacao)
                                                                 FROM   ly_unidade_ensino_situacao us2
                                                                 WHERE  us2.unidade_ens = us.unidade_ens
                                                                        AND us.unidade_ens = @CENSO)  
                                             "
            };

            contextQuery.Parameters.Add("@CENSO", UnidadeEnsino);

            try
            {
                var retorno = ctx.GetDataReader(contextQuery);

                if (retorno.Read())
                {
                    blnRet = true;
                }

                return blnRet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

