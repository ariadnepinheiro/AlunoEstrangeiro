using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using System.Text;
using Techne.Data;
using System.Data.SqlClient;
using Techne.Lyceum.RN.DTOs;

namespace Techne.Lyceum.RN
{
    public class CtvAnalise : RNBase
    {
        public static DataTable Listar()
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command = @" SELECT  A.* ,
                                    C.NOME, convert(varchar,ano) + ' - ' + convert(varchar, periodo) as anoperiodo,
                             convert(varchar,ANO_REFERENCIA) + ' - ' + convert(varchar, PERIODO_REFERENCIA) as anoperiodoreferencia
                            FROM    TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                                    INNER JOIN LY_CURSO C ON A.CURSO = C.CURSO "
                };

                return ctx.GetDataTable(contextQuery);
            }
        }

        public static TceCtvAnalise Carregar(int ano, int periodo, string censo, bool turno, bool vaga)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery =
                    new ContextQuery(
                        @"SELECT *
                                    FROM    TCE_CTV_ANALISE
                                    WHERE   ANO = @ANO
                            AND PERIODO = @PERIODO
                            AND CENSO = @CENSO
                            AND TURNO = @TURNO
                            AND VAGA = @VAGA");

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@VAGA", vaga);

                return ctx.TryToBindEntity<TceCtvAnalise>(contextQuery);
            }
        }

        public List<DadosAnalise> ListaDadosAnalisesPor(int ano, string censo, bool turno, bool vaga)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            List<DadosAnalise> listaAnalises = new List<DadosAnalise>();
            DadosAnalise dadosAnalise = new DadosAnalise();
            ContextQuery contextQuery = new ContextQuery();
            string analiseSuplanSalvas = string.Empty;
            string analiseSupedSalvas = string.Empty;
            string analiseDiespSalvas = string.Empty;
            bool editavel = false;

            try
            {
                contextQuery.Command = @" SELECT  DISTINCT
                                A.ANO ,
                                A.PERIODO ,
                                AN.ANALISE_SUPLAN ,
                                AN.ANALISE_SUPED ,
                                AN.ANALISEDIESP ,
                                AN.MATRICULA_SUPLAN + ' - '
                                + ( SELECT DISTINCT
                                            nome
                                    FROM    hades.dbo.HD_USUARIO u
                                    WHERE   u.USUARIO = an.MATRICULA_SUPLAN
                                  ) AS MATRICULA_SUPLAN ,
                                AN.MATRICULA_SUPED + ' - ' + ( SELECT DISTINCT
                                                                        nome
                                                               FROM     hades.dbo.HD_USUARIO u
                                                               WHERE    u.USUARIO = an.MATRICULA_SUPED
                                                             ) AS MATRICULA_SUPED ,
                                AN.MATRICULADIESP + ' - ' + ( SELECT DISTINCT
                                                                        nome
                                                              FROM      hades.dbo.HD_USUARIO u
                                                              WHERE     u.USUARIO = an.MATRICULADIESP
                                                            ) AS MATRICULADIESP ,
                                AN.DT_ANALISE_SUPLAN ,
                                AN.DT_ANALISE_SUPED ,
                                AN.DATAANALISEDIESP
                        FROM    DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA A
                        INNER JOIN DBO.TCE_CTV_CONF_TURNO_INICIAL I ON A.ID_AGENDA_CONF_TURNO_VAGA = I.ID_AGENDA_CONF_TURNO_VAGA
                        LEFT JOIN DBO.TCE_CTV_ANALISE AN ON A.ANO = AN.ANO
                                                                    AND A.PERIODO = AN.PERIODO
                                                                    AND AN.CENSO = I.CENSO
                                                                    AND AN.TURNO = @TURNO
                                                                    AND AN.VAGA = @VAGA
                        WHERE   A.ANO = @ANO 
                                AND I.CENSO = @CENSO
                                AND NOT EXISTS ( SELECT TOP 1 1
                         FROM   dbo.TCE_CTV_RESTRICAO RE
                         WHERE  RE.ID_AGENDA_CONF_TURNO_VAGA = A.ID_AGENDA_CONF_TURNO_VAGA
                                AND RE.CENSO = I.CENSO ) ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@VAGA", vaga);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosAnalise = new DadosAnalise();

                    dadosAnalise.Periodo = Convert.ToInt32(reader["PERIODO"]);

                    //Verifica se está em periodo de analise
                    if (turno)
                    {
                        editavel = this.PodeAnalisarTurnoPor(ano, dadosAnalise.Periodo, censo);
                        dadosAnalise.AnaliseSuplanEditavel = editavel;
                        dadosAnalise.AnaliseSupedEditavel = editavel;
                        dadosAnalise.AnaliseDiespEditavel = editavel;
                    }
                    if (vaga)
                    {
                        editavel = this.PodeAnalisarVagaPor(ano, dadosAnalise.Periodo, censo);
                        dadosAnalise.AnaliseSuplanEditavel = editavel;
                        dadosAnalise.AnaliseSupedEditavel = editavel;
                        dadosAnalise.AnaliseDiespEditavel = editavel;
                    }

                    if (!string.IsNullOrEmpty(Convert.ToString(reader["ANALISE_SUPLAN"])))
                    {
                        //Carregar Analise SUPLAN
                        dadosAnalise.AnaliseSuplan = Convert.ToString(reader["ANALISE_SUPLAN"]);
                        dadosAnalise.DataAnaliseSuplan = Convert.ToDateTime(reader["DT_ANALISE_SUPLAN"]);
                        dadosAnalise.ResponsavelAnaliseSuplan = Convert.ToString(reader["MATRICULA_SUPLAN"]);
                    }

                    if (!string.IsNullOrEmpty(Convert.ToString(reader["ANALISE_SUPED"])))
                    {
                        //Carregar Analise SUPED
                        dadosAnalise.AnaliseSuped = Convert.ToString(reader["ANALISE_SUPED"]);
                        dadosAnalise.DataAnaliseSuped = Convert.ToDateTime(reader["DT_ANALISE_SUPED"]);
                        dadosAnalise.ResponsavelAnaliseSuped = Convert.ToString(reader["MATRICULA_SUPED"]);
                    }
                    if (!string.IsNullOrEmpty(Convert.ToString(reader["ANALISEDIESP"])))
                    {
                        //Carregar Analise DIESP
                        dadosAnalise.AnaliseDiesp = Convert.ToString(reader["ANALISEDIESP"]);
                        dadosAnalise.DataAnaliseDiesp = Convert.ToDateTime(reader["DATAANALISEDIESP"]);
                        dadosAnalise.ResponsavelAnaliseDiesp = Convert.ToString(reader["MATRICULADIESP"]);
                    }

                    listaAnalises.Add(dadosAnalise);
                }

                return listaAnalises;
            }

            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }

        public DadosAnalise ObtemAnalisesPor(int ano, string censo, bool turno, bool vaga)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            DadosAnalise dadosAnalise = new DadosAnalise();
            ContextQuery contextQuery = new ContextQuery();
            string analiseSuplanSalvas = string.Empty;
            string analiseSupedSalvas = string.Empty;
            string analiseDiespSalvas = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT *
                                    FROM    TCE_CTV_ANALISE
                                    WHERE   ANO = @ANO
                            AND CENSO = @CENSO
                            AND TURNO = @TURNO
                            AND VAGA = @VAGA ";

                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@TURNO", turno);
                contextQuery.Parameters.Add("@VAGA", vaga);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(reader["ANALISE_SUPLAN"])))
                    {
                        //Carregar Analise SUPLAN
                        if (!string.IsNullOrEmpty(analiseSuplanSalvas))
                        {
                            analiseSuplanSalvas = string.Format("{0}{1}", analiseSuplanSalvas, Environment.NewLine);
                        }
                        analiseSuplanSalvas = string.Format("{0}Ano: {1} Periodo: {2} - {3} por {4} - {5}",
                        analiseSuplanSalvas,
                        ano.ToString(),
                        Convert.ToString(reader["PERIODO"]),
                        Convert.ToString(reader["ANALISE_SUPLAN"]),
                        Convert.ToString(reader["MATRICULA_SUPLAN"]),
                        Usuarios.BuscaNome(Convert.ToString(reader["MATRICULA_SUPLAN"])));
                    }

                    if (!string.IsNullOrEmpty(Convert.ToString(reader["ANALISE_SUPED"])))
                    {
                        //Carregar Analise SUPED
                        if (!string.IsNullOrEmpty(analiseSupedSalvas))
                        {
                            analiseSupedSalvas = string.Format("{0}{1}", analiseSupedSalvas, Environment.NewLine);
                        }
                        analiseSupedSalvas = string.Format("{0}Ano: {1} Periodo: {2} - {3} por {4} - {5}",
                        analiseSupedSalvas,
                        ano.ToString(),
                        Convert.ToString(reader["PERIODO"]),
                        Convert.ToString(reader["ANALISE_SUPED"]),
                        Convert.ToString(reader["MATRICULA_SUPED"]),
                        Usuarios.BuscaNome(Convert.ToString(reader["MATRICULA_SUPED"])));
                    }

                    if (!string.IsNullOrEmpty(Convert.ToString(reader["ANALISEDIESP"])))
                    {
                        //Carregar Analise DIESP
                        if (!string.IsNullOrEmpty(analiseDiespSalvas))
                        {
                            analiseDiespSalvas = string.Format("{0}{1}", analiseDiespSalvas, Environment.NewLine);
                        }
                        analiseDiespSalvas = string.Format("{0}Ano: {1} Periodo: {2} - {3} por {4} - {5}",
                        analiseDiespSalvas,
                        ano.ToString(),
                        Convert.ToString(reader["PERIODO"]),
                        Convert.ToString(reader["ANALISEDIESP"]),
                        Convert.ToString(reader["MATRICULADIESP"]),
                        Usuarios.BuscaNome(Convert.ToString(reader["MATRICULADIESP"])));
                    }
                }

                dadosAnalise.AnaliseSuplan = analiseSuplanSalvas;
                dadosAnalise.AnaliseSuped = analiseSupedSalvas;
                dadosAnalise.AnaliseDiesp = analiseDiespSalvas;

                return dadosAnalise;
            }

            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }

        public static ValidacaoDados Validar(TceCtvAnalise ctvAnalise, string perfil)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados
             {
                 Valido = false
             };

            if (ctvAnalise == null)
            {
                return validacaoDados;
            }

            if (ctvAnalise.Ano <= 0)
            {
                mensagens.Add("O campo ANO é obrigatório!");
            }

            if (ctvAnalise.Periodo < 0)
            {
                mensagens.Add("O campo PERIODO é obrigatório!");
            }
            if (string.IsNullOrEmpty(ctvAnalise.Censo))
            {
                mensagens.Add("O campo CENSO é obrigatório!");
            }

            //SUPED
            if (//perfil == "SUPED" || 
                perfil == "SUPED")
            {
                if (string.IsNullOrEmpty(ctvAnalise.AnaliseSuped) || (!string.IsNullOrEmpty(ctvAnalise.AnaliseSuped)
                        && ctvAnalise.AnaliseSuped.Length > 1000))
                {
                    mensagens.Add("O campo ANÁLISE SUPED é obrigatório com o máximo de 1000 caracteres!");
                }
                if (string.IsNullOrEmpty(ctvAnalise.MatriculaSuped)
                    || (!string.IsNullOrEmpty(ctvAnalise.MatriculaSuped)
                        && ctvAnalise.MatriculaSuped.Length > 20))
                {
                    mensagens.Add("O campo MATRICULA SUPED é obrigatório com o máximo de 20 caracteres!");
                }
            }
            if (//perfil == "2" || 
                perfil == "SUPLAN")
            {
                if (string.IsNullOrEmpty(ctvAnalise.AnaliseSuplan) || (!string.IsNullOrEmpty(ctvAnalise.AnaliseSuplan)
                        && ctvAnalise.AnaliseSuplan.Length > 1000))
                {
                    mensagens.Add("O campo ANÁLISE SUPLAN é obrigatório com o máximo de 1000 caracteres!");
                }

                if (string.IsNullOrEmpty(ctvAnalise.MatriculaSuplan)
                    || (!string.IsNullOrEmpty(ctvAnalise.MatriculaSuplan)
                        && ctvAnalise.MatriculaSuplan.Length > 20))
                {
                    mensagens.Add("O campo MATRICULA SUPLAN é obrigatório com o máximo de 20 caracteres!");
                }
            }

            //DIESP
            if (perfil == "DIESP")
            {
                if (string.IsNullOrEmpty(ctvAnalise.AnaliseDiesp) || (!string.IsNullOrEmpty(ctvAnalise.AnaliseDiesp)
                        && ctvAnalise.AnaliseDiesp.Length > 1000))
                {
                    mensagens.Add("O campo ANÁLISE DIESP é obrigatório com o máximo de 1000 caracteres!");
                }

                if (string.IsNullOrEmpty(ctvAnalise.MatriculaDiesp)
                    || (!string.IsNullOrEmpty(ctvAnalise.MatriculaDiesp)
                        && ctvAnalise.MatriculaDiesp.Length > 20))
                {
                    mensagens.Add("O campo MATRICULA DIESP é obrigatório com o máximo de 20 caracteres!");
                }
            }

            if ((!string.IsNullOrEmpty(ctvAnalise.AnaliseSuped) && ctvAnalise.AnaliseSuped.Length > 1000))
            {
                mensagens.Add("O campo ANÁLISE SUPED pode ter no máximo de 1000 caracteres!");
            }

            if ((!string.IsNullOrEmpty(ctvAnalise.AnaliseSuplan) && ctvAnalise.AnaliseSuplan.Length > 1000))
            {
                mensagens.Add("O campo ANÁLISE SUPLAN pode ter no máximo de 1000 caracteres!");
            }

            if ((!string.IsNullOrEmpty(ctvAnalise.AnaliseDiesp) && ctvAnalise.AnaliseDiesp.Length > 1000))
            {
                mensagens.Add("O campo ANÁLISE DIESP pode ter no máximo de 1000 caracteres!");
            }

            if (perfil == "privilegiado")
            {
                if (string.IsNullOrEmpty(ctvAnalise.AnaliseSuplan) && string.IsNullOrEmpty(ctvAnalise.AnaliseSuped) && string.IsNullOrEmpty(ctvAnalise.AnaliseDiesp))
                {
                    mensagens.Add("É necessário o preenchimento de pelo menos uma ANÁLISE!");
                }
            }

            if (ctvAnalise.Turno == ctvAnalise.Vaga)
            {
                mensagens.Add("Informa se a analíse é para TURNO ou VAGAS!");
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

        public ValidacaoDados ValidaListaAnalises(List<TceCtvAnalise> analises, string perfil)
        {
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (analises == null || analises.Count == 0)
            {
                validacaoDados.Mensagem = "Sem análise a ser salva.";
                return validacaoDados;
            }

            foreach (TceCtvAnalise ctvAnalise in analises)
            {
                if (ctvAnalise.Ano <= 0)
                {
                    mensagens.Add("O campo ANO é obrigatório!");
                }

                if (ctvAnalise.Periodo < 0)
                {
                    mensagens.Add("O campo PERIODO é obrigatório!");
                }
                if (string.IsNullOrEmpty(ctvAnalise.Censo))
                {
                    mensagens.Add("O campo CENSO é obrigatório!");
                }

                //SUPED
                if (perfil == "SUPED")
                {
                    if (string.IsNullOrEmpty(ctvAnalise.AnaliseSuped) || (!string.IsNullOrEmpty(ctvAnalise.AnaliseSuped)
                            && ctvAnalise.AnaliseSuped.Length > 1000))
                    {
                        mensagens.Add(string.Format("O campo ANÁLISE SUPED é obrigatório para o ano {0} periodo {1}!", ctvAnalise.Ano, ctvAnalise.Periodo));
                    }
                    if (string.IsNullOrEmpty(ctvAnalise.MatriculaSuped)
                        || (!string.IsNullOrEmpty(ctvAnalise.MatriculaSuped)
                            && ctvAnalise.MatriculaSuped.Length > 20))
                    {
                        mensagens.Add("O campo MATRICULA SUPED é obrigatório com o máximo de 20 caracteres!");
                    }
                }

                //SUPLAN
                if (perfil == "SUPLAN")
                {
                    if (string.IsNullOrEmpty(ctvAnalise.AnaliseSuplan) || (!string.IsNullOrEmpty(ctvAnalise.AnaliseSuplan)
                            && ctvAnalise.AnaliseSuplan.Length > 1000))
                    {
                        mensagens.Add(string.Format("O campo ANÁLISE SUPLAN é obrigatório para o ano {0} periodo {1}!", ctvAnalise.Ano, ctvAnalise.Periodo));
                    }

                    if (string.IsNullOrEmpty(ctvAnalise.MatriculaSuplan)
                        || (!string.IsNullOrEmpty(ctvAnalise.MatriculaSuplan)
                            && ctvAnalise.MatriculaSuplan.Length > 20))
                    {
                        mensagens.Add("O campo MATRICULA SUPLAN é obrigatório com o máximo de 20 caracteres!");
                    }
                }

                //DIESP
                if (perfil == "DIESP")
                {
                    if (string.IsNullOrEmpty(ctvAnalise.AnaliseDiesp) || (!string.IsNullOrEmpty(ctvAnalise.AnaliseDiesp)
                            && ctvAnalise.AnaliseDiesp.Length > 1000))
                    {
                        mensagens.Add(string.Format("O campo ANÁLISE DIESP é obrigatório para o ano {0} periodo {1}!", ctvAnalise.Ano, ctvAnalise.Periodo));
                    }

                    if (string.IsNullOrEmpty(ctvAnalise.MatriculaDiesp)
                        || (!string.IsNullOrEmpty(ctvAnalise.MatriculaDiesp)
                            && ctvAnalise.MatriculaDiesp.Length > 20))
                    {
                        mensagens.Add("O campo MATRICULA DIESP é obrigatório com o máximo de 20 caracteres!");
                    }
                }

                if (ctvAnalise.Turno == ctvAnalise.Vaga)
                {
                    mensagens.Add("Informa se a analíse é para TURNO ou VAGAS!");
                }
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

        public static void Salvar(TceCtvAnalise ctvAnalise, string perfil)
        {
            using (var context = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var idAnalise = RetornaIdAnalise(ctvAnalise);

                    if (idAnalise > 0)
                    {
                        ctvAnalise.IdAnalise = idAnalise;
                        Alterar(context, ctvAnalise, perfil);
                    }
                    else
                    {
                        Inserir(context, ctvAnalise);
                    }
                }
                catch (Exception)
                {
                    context.Abandon();
                    throw;
                }
            }
        }

        public void Salva(List<TceCtvAnalise> analises, string perfil)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            int idAnalise = 0;
            try
            {
                //Salva analises
                foreach (TceCtvAnalise analise in analises)
                {
                    //Verifica se analise já existe
                    idAnalise = CtvAnalise.RetornaIdAnalise(analise);

                    if (idAnalise > 0)
                    {
                        analise.IdAnalise = idAnalise;
                        CtvAnalise.Alterar(ctx, analise, perfil);
                    }
                    else
                    {
                        CtvAnalise.Inserir(ctx, analise);
                    }
                }
            }

            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public static int RetornaIdAnalise(TceCtvAnalise ctvAnalise)
        {
            var idAnalise = 0;

            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @"SELECT  ID_ANALISE
                            FROM    DBO.TCE_CTV_ANALISE
                            WHERE   CENSO = @CENSO
                                    AND ANO = @ANO
                                    AND PERIODO = @PERIODO
                                    AND ID_ANALISE <> @ID_ANALISE
                                    AND TURNO = @TURNO
                                    AND VAGA = @VAGA
                                    "
                };
                contextQuery.Parameters.Add("@ID_ANALISE", ctvAnalise.IdAnalise);
                contextQuery.Parameters.Add("@CENSO", ctvAnalise.Censo);
                contextQuery.Parameters.Add("@ANO", ctvAnalise.Ano);
                contextQuery.Parameters.Add("@PERIODO", ctvAnalise.Periodo);
                contextQuery.Parameters.Add("@TURNO", ctvAnalise.Turno);
                contextQuery.Parameters.Add("@VAGA", ctvAnalise.Vaga);

                using (var reader = ctx.GetDataReader(contextQuery))
                {
                    while (reader.Read())
                    {
                        idAnalise = Convert.ToInt32(reader["ID_ANALISE"]);
                    }
                }
            }
            return idAnalise;
        }

        public static void Inserir(DataContext context, TceCtvAnalise ctvAnalise)
        {
            var contextQuery = new ContextQuery
            {
                Command = @" INSERT INTO TCE_CTV_ANALISE
                                ( ANO ,
                                  PERIODO ,
                                  CENSO ,
                                  TURNO ,
                                  VAGA ,
                                  ANALISE_SUPED ,
                                  MATRICULA_SUPED ,
                                  DT_ANALISE_SUPED ,
                                  ANALISE_SUPLAN ,
                                  MATRICULA_SUPLAN ,
                                  DT_ANALISE_SUPLAN ,
                                  ANALISEDIESP ,
                                  MATRICULADIESP ,
                                  DATAANALISEDIESP
                                )
                        VALUES  ( @ANO ,
                                  @PERIODO ,
                                  @CENSO ,
                                  @TURNO ,
                                  @VAGA ,
                                  @ANALISE_SUPED ,
                                  @MATRICULA_SUPED ,
                                  @DT_ANALISE_SUPED ,
                                  @ANALISE_SUPLAN ,
                                  @MATRICULA_SUPLAN ,
                                  @DT_ANALISE_SUPLAN ,
                                  @ANALISEDIESP ,
                                  @MATRICULADIESP ,
                                  @DATAANALISEDIESP
                                ) "
            };

            contextQuery.Parameters.Add("@ANO", ctvAnalise.Ano);
            contextQuery.Parameters.Add("@PERIODO", ctvAnalise.Periodo);
            contextQuery.Parameters.Add("@CENSO", ctvAnalise.Censo);
            contextQuery.Parameters.Add("@TURNO", ctvAnalise.Turno);
            contextQuery.Parameters.Add("@VAGA", ctvAnalise.Vaga);
            contextQuery.Parameters.Add("@ANALISE_SUPED", ctvAnalise.AnaliseSuped);
            contextQuery.Parameters.Add("@MATRICULA_SUPED", ctvAnalise.MatriculaSuped);
            contextQuery.Parameters.Add("@DT_ANALISE_SUPED", ctvAnalise.DtAnaliseSuped == DateTime.MinValue ? null : ctvAnalise.DtAnaliseSuped);
            contextQuery.Parameters.Add("@ANALISE_SUPLAN", ctvAnalise.AnaliseSuplan);
            contextQuery.Parameters.Add("@MATRICULA_SUPLAN", ctvAnalise.MatriculaSuplan);
            contextQuery.Parameters.Add("@DT_ANALISE_SUPLAN", ctvAnalise.DtAnaliseSuplan == DateTime.MinValue ? null : ctvAnalise.DtAnaliseSuplan);
            contextQuery.Parameters.Add("@ANALISEDIESP", ctvAnalise.AnaliseDiesp);
            contextQuery.Parameters.Add("@MATRICULADIESP", ctvAnalise.MatriculaDiesp);
            contextQuery.Parameters.Add("@DATAANALISEDIESP", ctvAnalise.DataAnaliseDiesp == DateTime.MinValue ? null : ctvAnalise.DataAnaliseDiesp);

            context.ApplyModifications(contextQuery);
        }

        public static void Alterar(DataContext context, TceCtvAnalise ctvAnalise, string perfil)
        {
            //SUPLAN
            if (//perfil == "2" || 
                perfil == "SUPLAN")
            {
                var contextQuery = new ContextQuery
                                       {
                                           Command =
                                               @" UPDATE  TCE_CTV_ANALISE
                            SET     ANALISE_SUPLAN = @ANALISE_SUPLAN ,
                                    MATRICULA_SUPLAN = @MATRICULA_SUPLAN ,
                                    DT_ANALISE_SUPLAN = @DT_ANALISE_SUPLAN
                            WHERE   ID_ANALISE = @ID_ANALISE "
                                       };

                contextQuery.Parameters.Add("@ID_ANALISE", ctvAnalise.IdAnalise);
                contextQuery.Parameters.Add("@ANALISE_SUPLAN", ctvAnalise.AnaliseSuplan);
                contextQuery.Parameters.Add("@MATRICULA_SUPLAN", ctvAnalise.MatriculaSuplan);
                contextQuery.Parameters.Add("@DT_ANALISE_SUPLAN", ctvAnalise.DtAnaliseSuplan);
                context.ApplyModifications(contextQuery);
            }

            //SUPED
            if (//perfil == "1" || 
                perfil == "SUPED")
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @" UPDATE  TCE_CTV_ANALISE
                            SET     ANALISE_SUPED = @ANALISE_SUPED ,
                                    MATRICULA_SUPED = @MATRICULA_SUPED ,
                                    DT_ANALISE_SUPED = @DT_ANALISE_SUPED
                            WHERE   ID_ANALISE = @ID_ANALISE "
                };

                contextQuery.Parameters.Add("@ID_ANALISE", ctvAnalise.IdAnalise);
                contextQuery.Parameters.Add("@ANALISE_SUPED", ctvAnalise.AnaliseSuped);
                contextQuery.Parameters.Add("@MATRICULA_SUPED", ctvAnalise.MatriculaSuped);
                contextQuery.Parameters.Add("@DT_ANALISE_SUPED", ctvAnalise.DtAnaliseSuped);

                context.ApplyModifications(contextQuery);
            }

            //DIESP
            if (//perfil == "7" || 
                perfil == "DIESP")
            {
                var contextQuery = new ContextQuery
                {
                    Command =
                        @" UPDATE  TCE_CTV_ANALISE
                            SET     ANALISEDIESP = @ANALISEDIESP ,
                                    MATRICULADIESP = @MATRICULADIESP ,
                                    DATAANALISEDIESP = @DATAANALISEDIESP
                            WHERE   ID_ANALISE = @ID_ANALISE "
                };

                contextQuery.Parameters.Add("@ID_ANALISE", ctvAnalise.IdAnalise);
                contextQuery.Parameters.Add("@ANALISEDIESP", ctvAnalise.AnaliseDiesp);
                contextQuery.Parameters.Add("@MATRICULADIESP", ctvAnalise.MatriculaDiesp);
                contextQuery.Parameters.Add("@DATAANALISEDIESP", ctvAnalise.DataAnaliseDiesp);
                context.ApplyModifications(contextQuery);
            }

            if (perfil == "privilegiado")
            {
                if (!string.IsNullOrEmpty(ctvAnalise.AnaliseSuped))
                {
                    var contextQuery = new ContextQuery
                    {
                        Command =
                            @" UPDATE  TCE_CTV_ANALISE
                            SET     ANALISE_SUPED = @ANALISE_SUPED ,
                                    MATRICULA_SUPED = @MATRICULA_SUPED ,
                                    DT_ANALISE_SUPED = @DT_ANALISE_SUPED
                            WHERE   ID_ANALISE = @ID_ANALISE "
                    };

                    contextQuery.Parameters.Add("@ID_ANALISE", ctvAnalise.IdAnalise);
                    contextQuery.Parameters.Add("@ANALISE_SUPED", ctvAnalise.AnaliseSuped);
                    contextQuery.Parameters.Add("@MATRICULA_SUPED", ctvAnalise.MatriculaSuped);
                    contextQuery.Parameters.Add("@DT_ANALISE_SUPED", ctvAnalise.DtAnaliseSuped);
                    context.ApplyModifications(contextQuery);
                }

                if (!string.IsNullOrEmpty(ctvAnalise.AnaliseSuplan))
                {
                    var contextQuery = new ContextQuery
                    {
                        Command =
                            @" UPDATE  TCE_CTV_ANALISE
                            SET     ANALISE_SUPLAN = @ANALISE_SUPLAN ,
                                    MATRICULA_SUPLAN = @MATRICULA_SUPLAN ,
                                    DT_ANALISE_SUPLAN = @DT_ANALISE_SUPLAN
                            WHERE   ID_ANALISE = @ID_ANALISE "
                    };

                    contextQuery.Parameters.Add("@ID_ANALISE", ctvAnalise.IdAnalise);
                    contextQuery.Parameters.Add("@ANALISE_SUPLAN", ctvAnalise.AnaliseSuplan);
                    contextQuery.Parameters.Add("@MATRICULA_SUPLAN", ctvAnalise.MatriculaSuplan);
                    contextQuery.Parameters.Add("@DT_ANALISE_SUPLAN", ctvAnalise.DtAnaliseSuplan);
                    context.ApplyModifications(contextQuery);
                }

                if (!string.IsNullOrEmpty(ctvAnalise.AnaliseDiesp))
                {
                    var contextQuery = new ContextQuery
                    {
                        Command =
                            @" UPDATE  TCE_CTV_ANALISE
                            SET     ANALISEDIESP = @ANALISEDIESP ,
                                    MATRICULADIESP = @MATRICULADIESP ,
                                    DATAANALISEDIESP = @DATAANALISEDIESP
                            WHERE   ID_ANALISE = @ID_ANALISE "
                    };

                    contextQuery.Parameters.Add("@ID_ANALISE", ctvAnalise.IdAnalise);
                    contextQuery.Parameters.Add("@ANALISEDIESP", ctvAnalise.AnaliseDiesp);
                    contextQuery.Parameters.Add("@MATRICULADIESP", ctvAnalise.MatriculaDiesp);
                    contextQuery.Parameters.Add("@DATAANALISEDIESP", ctvAnalise.DataAnaliseDiesp);
                    context.ApplyModifications(contextQuery);
                }
            }
        }

        public static void Remover(int id, string usuario)
        {
            TceCtvAnalise objTceCtvAnalise = new TceCtvAnalise();

            bool bln = false;

            if (id < 1)
            {
                return;
            }

            var contextQuery = new ContextQuery();
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    objTceCtvAnalise = VerificaAnalise(id);

                    if (usuario.Equals("SUPED"))
                    {
                        bln = ((objTceCtvAnalise.AnaliseSuplan == null || objTceCtvAnalise.AnaliseSuplan.Equals("")) &&
                        (objTceCtvAnalise.AnaliseDiesp == null || objTceCtvAnalise.AnaliseDiesp.Equals("")));
                    }
                    else if (usuario.Equals("SUPLAN"))
                    {
                        bln = ((objTceCtvAnalise.AnaliseSuped == null || objTceCtvAnalise.AnaliseSuped.Equals("")) &&
                        (objTceCtvAnalise.AnaliseDiesp == null || objTceCtvAnalise.AnaliseDiesp.Equals("")));
                    }
                    else if (usuario.Equals("DIESP"))
                    {
                        bln = ((objTceCtvAnalise.AnaliseSuplan == null || objTceCtvAnalise.AnaliseSuplan.Equals("")) &&
                        (objTceCtvAnalise.AnaliseSuped == null || objTceCtvAnalise.AnaliseSuped.Equals("")));
                    }

                    if (bln)
                    {
                        contextQuery.Command = @" DELETE  TCE_CTV_ANALISE
                            WHERE   ID_ANALISE = @ID_ANALISE ";
                    }
                    else
                    {
                        if (usuario.Equals("SUPED"))
                        {
                            contextQuery.Command =
                            @" UPDATE TCE_CTV_ANALISE SET DT_ANALISE_SUPED = NULL, MATRICULA_SUPED = NULL,
                            ANALISE_SUPED = NULL
                            WHERE   ID_ANALISE = @ID_ANALISE ";
                        }
                        else if (usuario.Equals("SUPLAN"))
                        {
                            contextQuery.Command =
                            @" UPDATE TCE_CTV_ANALISE SET DT_ANALISE_SUPLAN = NULL, MATRICULA_SUPLAN = NULL,
                            ANALISE_SUPLAN = NULL
                            WHERE   ID_ANALISE = @ID_ANALISE ";
                        }
                        else if (usuario.Equals("DIESP"))
                        {
                            contextQuery.Command =
                            @" UPDATE TCE_CTV_ANALISE SET DATAANALISEDIESP = NULL, MATRICULADIESP = NULL,
                            ANALISEDIESP = NULL
                            WHERE   ID_ANALISE = @ID_ANALISE ";
                        }
                    }

                    contextQuery.Parameters.Add("@ID_ANALISE", id);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static bool VerificaSeAgendaEstaEncerrada(int id, string censo, int ano, int periodo)
        {
            bool blnRetorno = true;
            int resultado = 0;
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery =
                    new ContextQuery(
                        @"SELECT COUNT(*) as RESULTADO
                        FROM    DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA AG
                                INNER JOIN DBO.TCE_CTV_CONF_VAGA V ON AG.ID_AGENDA_CONF_TURNO_VAGA = V.ID_AGENDA_CONF_TURNO_VAGA
                        WHERE   AG.ENCERRADO = 0
                                AND AG.ANO = @ANO
                                AND AG.PERIODO = @PERIODO
                                AND V.CENSO = @CENSO");

                contextQuery.Parameters.Add("@ID", id);
                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);

                var reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    resultado = Convert.ToInt32(reader["RESULTADO"]);
                }

                if (resultado > 0)
                {
                    blnRetorno = false;
                }
            }

            return blnRetorno;
        }

        public static TceCtvAnalise VerificaAnalise(int id)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingNoLock())
            {
                var contextQuery =
                    new ContextQuery(
                        @"select * from TCE_CTV_ANALISE
                            where id_analise = @ID");

                contextQuery.Parameters.Add("@ID", id);

                return ctx.TryToBindEntity<TceCtvAnalise>(contextQuery);
            }
        }

        public QueryTable RetornaAnalisesConfirmacaoVagasPor(object ano, object periodo, string strPerfil, string faixaInicial, string faixaFinal, string tipoSerie, string analisado)
        {
            TcePerfil objPerfil = new TcePerfil();

            objPerfil = RN.Perfil.RetornaPerfilPor(strPerfil);

            var ctx = DataContextBuilder.FromLyceum.UsingNoLock();

            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"SELECT DISTINCT  CONVERT(VARCHAR(4), AG.ano) + '/' + CONVERT(VARCHAR(1), AG.periodo)+ '/' + cv.CENSO as ID, ISNULL(id_analise, -1) as id_analise, RG.REGIONAL, MN.NOME MUNICIPIO, CV.CENSO, UE.NOME_COMP, 
                                                CONVERT(VARCHAR(4), AG.ano) + '/' + CONVERT(VARCHAR(1), AG.periodo) AS ANOPERIODO ");

                if (objPerfil.Descricao.Equals("SUPLAN"))
                {
                    //PARA SUPLAN 
                    strSql.Append(@" , AN.ANALISE_SUPLAN ANALISE, AN.DT_ANALISE_SUPLAN DATA, AN.MATRICULA_SUPLAN MATRICULA ");
                }
                else if (objPerfil.Descricao.Equals("SUPED"))
                {
                    //PARA SUPED 
                    strSql.Append(@" , AN.ANALISE_SUPED ANALISE, AN.DT_ANALISE_SUPED DATA, AN.MATRICULA_SUPED MATRICULA ");
                }
                else if (objPerfil.Descricao.Equals("DIESP"))
                {
                    //PARA DIESP 
                    strSql.Append(@" , AN.ANALISEDIESP ANALISE, AN.DATAANALISEDIESP DATA, AN.MATRICULADIESP MATRICULA ");
                }

                strSql.Append(@" FROM   LY_UNIDADE_ENSINO UE (NOLOCK)
                                INNER JOIN MUNICIPIO MN (NOLOCK) ON (UE.MUNICIPIO = MN.CODIGO)
                                INNER JOIN TCE_REGIONAL RG (NOLOCK) ON (UE.ID_REGIONAL = RG.ID_REGIONAL)
                                INNER JOIN TCE_CTV_CONF_VAGA CV (NOLOCK) ON (CV.CENSO = UE.UNIDADE_ENS)
                                INNER JOIN TCE_CTV_AGENDA_CONF_TURNO_VAGA AG (NOLOCK)
                                ON (AG.ID_AGENDA_CONF_TURNO_VAGA = CV.ID_AGENDA_CONF_TURNO_VAGA)
                                INNER JOIN LY_CURSO CR (NOLOCK) ON (CR.CURSO = AG.CURSO) ");

                if (!objPerfil.Descricao.Equals("DIESP"))
                {
                    //BLOCO PARA PERFIS DIFERENTE DE DIESP
                    strSql.Append(@" INNER JOIN PERFILMODALIDADE PM (NOLOCK) ON (PM.MODALIDADEID = CR.MODALIDADE) ");
                }

                strSql.Append(@" LEFT OUTER JOIN TCE_CTV_ANALISE AN (NOLOCK)
                                ON (AG.ANO = AN.ANO AND AG.PERIODO = AN.PERIODO
                                AND AN.CENSO = CV.CENSO AND AN.TURNO = 0 AND AN.VAGA = 1)
                                WHERE  EXISTS (SELECT DISTINCT CF.CENSO FROM TCE_CTV_FINALIZADO CF (NOLOCK)
                                   WHERE CF.ID_AGENDA_CONF_TURNO_VAGA = AG.ID_AGENDA_CONF_TURNO_VAGA
                                     AND CF.CENSO = CV.CENSO AND CF.VAGA = 1
                                   UNION
                                   SELECT DISTINCT CV1.CENSO FROM TCE_CTV_CONF_VAGA CV1 (NOLOCK)
                                   WHERE CV1.ID_AGENDA_CONF_TURNO_VAGA = AG.ID_AGENDA_CONF_TURNO_VAGA
                                     AND AG.DT_FIM_CONF_VAGAS < GETDATE()) AND ");

                if (objPerfil.Descricao.Equals("DIESP"))
                {
                    //PARA DIESP 
                    strSql.Append(@" UE.ID_REGIONAL = 5");
                }
                else
                {
                    //PARA Diferente de DIESP 
                    strSql.Append(@" UE.ID_REGIONAL <> 5 AND PM.PERFILID = " + objPerfil.IdPerfil);
                }

                strSql.Append(@" AND AG.ano = " + ano + @"
                                 AND AG.periodo = " + periodo);

                // verifica se possui análise por perfil
                if (objPerfil.Descricao.Equals("SUPLAN"))
                {
                    if (analisado.Equals("Não"))
                    {
                        strSql.Append(@" AND isnull(AN.ANALISE_SUPLAN, '') = '' ");
                    }
                    else if (analisado.Equals("Sim"))
                    {
                        strSql.Append(@" AND isnull(AN.ANALISE_SUPLAN, '') <> '' ");
                    }
                }
                else if (objPerfil.Descricao.Equals("SUPED"))
                {
                    if (analisado.Equals("Não"))
                    {
                        strSql.Append(@" AND isnull(AN.ANALISE_SUPED, '') = '' ");
                    }
                    else if (analisado.Equals("Sim"))
                    {
                        strSql.Append(@"AND isnull(AN.ANALISE_SUPED, '') <> ''");
                    }
                }
                else if (objPerfil.Descricao.Equals("DIESP"))
                {
                    if (analisado.Equals("Não"))
                    {
                        strSql.Append(@" AND isnull(AN.ANALISEDIESP, '') = '' ");
                    }
                    else if (analisado.Equals("Sim"))
                    {
                        strSql.Append(@" AND isnull(AN.ANALISEDIESP, '') <> '' ");
                    }
                }

                //Verifica tipo série
                if (tipoSerie.Equals("Todos"))
                {
                    //
                }
                else if (tipoSerie.Equals("Demais"))
                {
                    strSql.Append(@" AND not ((AG.CURSO in ('0002.31','0003.31','0002.44','0092.39','0002.33','0401.32','0602.32','0504.32','1204.32') AND AG.SERIE = 1) OR (AG.CURSO in ('0001.21','0001.42') AND AG.SERIE = 6)) ");
                }
                if (tipoSerie.Equals("SISMAT"))
                {
                    strSql.Append(@" AND ((AG.CURSO in ('0002.31','0003.31','0002.44','0092.39','0002.33','0401.32','0602.32','0504.32','1204.32') AND AG.SERIE = 1) OR (AG.CURSO in ('0001.21','0001.42') AND AG.SERIE = 6)) ");
                }

                if (!faixaFinal.Equals("") && !faixaInicial.Equals(""))
                {
                    strSql.Append(@" AND EXISTS
                                (SELECT PS1.CENSO, PS1.ID_AGENDA_CONF_TURNO_VAGA,
                                (ISNULL(PS1.VAGAS_CONTINUIDADE, 0) +
                                ISNULL(PS1.VAGAS_NOVAS, 0)) SEEDUC,
                                (SUM(ISNULL(CV1.VAGAS_CONTINUIDADE, 0)) +
                                SUM(ISNULL(CV1.VAGAS_NOVAS, 0))) DIRETOR,
                                ABS(
                                ((ISNULL(PS1.VAGAS_CONTINUIDADE, 0) +
                                ISNULL(PS1.VAGAS_NOVAS, 0)) -
                                CAST(
                                CASE
                                WHEN (SUM(ISNULL(CV1.VAGAS_CONTINUIDADE, 0)) +
                                SUM(ISNULL(CV1.VAGAS_NOVAS, 0))) <= 0 THEN 1
                                ELSE (SUM(ISNULL(CV1.VAGAS_CONTINUIDADE, 0)) +
                                SUM(ISNULL(CV1.VAGAS_NOVAS, 0)))
                                END AS DECIMAL))
                                /
                                CAST((ISNULL(PS1.VAGAS_CONTINUIDADE, 0) +
                                ISNULL(PS1.VAGAS_NOVAS, 0)) AS DECIMAL)
                                * 100
                                )
                                VARIACAO
                                FROM TCE_CTV_PROPOSTA_SEEDUC PS1 (NOLOCK)
                                INNER JOIN TCE_CTV_CONF_VAGA CV1 (NOLOCK)
                                ON (PS1.ID_AGENDA_CONF_TURNO_VAGA = CV1.ID_AGENDA_CONF_TURNO_VAGA
                                AND PS1.CENSO = CV.CENSO)
                                WHERE CV.CENSO = CV1.CENSO
                                AND CV.ID_AGENDA_CONF_TURNO_VAGA = CV1.ID_AGENDA_CONF_TURNO_VAGA
                                GROUP BY PS1.CENSO, PS1.ID_AGENDA_CONF_TURNO_VAGA, PS1.VAGAS_CONTINUIDADE,
                                PS1.VAGAS_NOVAS
                                HAVING ABS(
                                ((ISNULL(PS1.VAGAS_CONTINUIDADE, 0) +
                                ISNULL(PS1.VAGAS_NOVAS, 0)) -
                                CAST(
                                CASE
                                WHEN (SUM(ISNULL(CV1.VAGAS_CONTINUIDADE, 0)) +
                                SUM(ISNULL(CV1.VAGAS_NOVAS, 0))) <= 0 THEN 1
                                ELSE (SUM(ISNULL(CV1.VAGAS_CONTINUIDADE, 0)) +
                                SUM(ISNULL(CV1.VAGAS_NOVAS, 0)))
                                END AS DECIMAL))
                                /
                                CAST((ISNULL(PS1.VAGAS_CONTINUIDADE, 0) +
                                ISNULL(PS1.VAGAS_NOVAS, 0)) AS DECIMAL)
                                * 100
                                ) BETWEEN " + faixaInicial + @" AND " + faixaFinal + ") ");
                }

                strSql.Append(@" ORDER  BY RG.regional, 
                                  MN.nome, 
                                  UE.nome_comp");

                return Consultar(Convert.ToString(strSql));
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                   Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        private bool PodeAnalisarTurnoPor(int ano, int periodo, string censo)
        {
            //Pode analisar caso a agenda nao esteja encerrada e  esteja no periodo de analise de turno  
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool pode = false;

            try
            {
                contextQuery.Command = @"  SELECT  COUNT(*)
                    FROM    DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA AG
                            INNER JOIN dbo.TCE_CTV_CONF_TURNO_INICIAL t ON AG.ID_AGENDA_CONF_TURNO_VAGA = t.ID_AGENDA_CONF_TURNO_VAGA
                    WHERE   ENCERRADO = 0
                            AND ANO = @ANO
                            AND PERIODO = @PERIODO
                            AND T.CENSO = @CENSO
                            AND ( SELECT    COUNT(*)
                                  FROM      agenda.EVENTO e
                                            INNER JOIN agenda.EVENTO_PERFIL ep ON e.EVENTOID = ep.EVENTOID
                                            INNER JOIN agenda.PARAMETROTURNOVAGA p ON e.agendaID = p.AGENDAID
                                                                                  AND ep.PERFILID = p.PERFILID
                                            INNER JOIN AGENDA.PERIODOLETIVOAGENDA AP ON e.AGENDAID = AP.AGENDAID
                                  WHERE     e.AGENDAID IN (
                                            SELECT DISTINCT
                                                    a.AGENDAID
                                            FROM    agenda.AGENDA a
                                                    INNER JOIN agenda.EVENTO ae ON a.AGENDAID = ae.AGENDAID
                                            WHERE   ae.TIPOEVENTOID = @TIPOEVENTOID
                                                    AND ae.DATAINICIO <= GETDATE()
                                                    AND ae.DATAFIM >= GETDATE()
                                                    AND a.AGENDAID = ag.AGENDAID )
                                            AND PODEANALISAR = 1
                                            AND e.AGENDAID = ag.AGENDAID
                                            AND e.DATAINICIO <= GETDATE()
                                            AND e.DATAFIM >= GETDATE()
                                            AND e.TIPOEVENTOID = @TIPOEVENTOID
                                ) > 0 ";

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@TIPOEVENTOID", (int)RN.Agenda.TipoEvento.TipoEventoAgenda.AnaliseTurnos);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    pode = true;
                }

                return pode;
            }

            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        private bool PodeAnalisarVagaPor(int ano, int periodo, string censo)
        {
            //Pode analisar caso a agenda nao esteja encerrada e  esteja no periodo de analise de vaga
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool pode = false;

            try
            {
                contextQuery.Command = @"  SELECT  COUNT(*)
                    FROM    DBO.TCE_CTV_AGENDA_CONF_TURNO_VAGA AG
                            INNER JOIN dbo.TCE_CTV_CONF_TURNO_INICIAL t ON AG.ID_AGENDA_CONF_TURNO_VAGA = t.ID_AGENDA_CONF_TURNO_VAGA                          
                    WHERE   ENCERRADO = 0
                            AND ANO = @ANO
                            AND PERIODO = @PERIODO
                            AND T.CENSO = @CENSO
                            AND ( SELECT COUNT(*)
                                         FROM   agenda.EVENTO e
                                                INNER JOIN agenda.EVENTO_PERFIL ep ON e.EVENTOID = ep.EVENTOID
                                                INNER JOIN agenda.PARAMETROTURNOVAGA p ON e.agendaID = p.AGENDAID
                                                                                  AND ep.PERFILID = p.PERFILID
                                                INNER JOIN AGENDA.PERIODOLETIVOAGENDA AP ON e.AGENDAID = AP.AGENDAID
                                         WHERE  e.AGENDAID IN (
                                                SELECT DISTINCT
                                                        a.AGENDAID
                                                FROM    agenda.AGENDA a
                                                        INNER JOIN agenda.EVENTO ae ON a.AGENDAID = ae.AGENDAID
                                                WHERE   ae.TIPOEVENTOID = @TIPOEVENTOID
                                                        AND ae.DATAINICIO <= GETDATE()
                                                        AND ae.DATAFIM >= GETDATE()
                                                        AND a.AGENDAID = ag.AGENDAID )
                                                AND PODEANALISAR = 1
                                                AND e.AGENDAID = ag.AGENDAID
                                                AND e.DATAINICIO <= GETDATE()
                                                AND e.DATAFIM >= GETDATE()
                                                AND e.TIPOEVENTOID = @TIPOEVENTOID ) > 0 ";

                contextQuery.Parameters.Add("@CENSO", censo);
                contextQuery.Parameters.Add("@ANO", ano);
                contextQuery.Parameters.Add("@PERIODO", periodo);
                contextQuery.Parameters.Add("@TIPOEVENTOID", (int)RN.Agenda.TipoEvento.TipoEventoAgenda.AnaliseVagas);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    pode = true;
                }

                return pode;
            }

            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public bool EhModoAnaliseTurnoPor(string usuario, bool privilegiado)
        {
            //Pode analisar caso:
            //. Usuario possua perfil que pode analisar ou seja privilegiado
            //. Esteja no periodo de analise de turno
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool pode = false;

            try
            {
                contextQuery.Command = string.Format(@" SELECT  COUNT(*)
                    FROM    HADES.DBO.HD_PADUSUARIO PU ( NOLOCK )
                            INNER JOIN HADES.DBO.TCE_PADACES_PERFIL PP ( NOLOCK ) ON PU.PADACES = PP.PADACES
                            INNER JOIN HADES.DBO.TCE_PERFIL PE ( NOLOCK ) ON PP.ID_PERFIL = PE.ID_PERFIL
                            INNER JOIN AGENDA.PARAMETROTURNOVAGA PT ( NOLOCK ) ON PT.PERFILID = PE.ID_PERFIL
                    WHERE   pt.PODEANALISAR = 1
                            {0}
                            AND EXISTS ( SELECT TOP 1
                                                1
                                         FROM   agenda.EVENTO e ( NOLOCK )
                                                INNER JOIN agenda.EVENTO_PERFIL ep ( NOLOCK ) ON e.EVENTOID = ep.EVENTOID
                                                INNER JOIN agenda.PARAMETROTURNOVAGA p ( NOLOCK ) ON e.agendaID = p.AGENDAID
                                                                                  AND ep.PERFILID = p.PERFILID
                                         WHERE  e.AGENDAID IN (
                                                SELECT DISTINCT
                                                        a.AGENDAID
                                                FROM    agenda.AGENDA a ( NOLOCK )
                                                        INNER JOIN agenda.EVENTO ae ( NOLOCK ) ON a.AGENDAID = ae.AGENDAID
                                                WHERE   ae.TIPOEVENTOID = @TIPOEVENTOID
                                                        AND ae.DATAINICIO <= GETDATE()
                                                        AND ae.DATAFIM >= GETDATE() )
                                                AND PODEANALISAR = 1
                                                AND e.AGENDAID = pt.AGENDAID
                                                AND e.DATAINICIO <= GETDATE()
                                                AND e.DATAFIM >= GETDATE()
                                                AND e.TIPOEVENTOID = @TIPOEVENTOID ) ", privilegiado ? string.Empty : "AND pu.USUARIO = @USUARIO");

                contextQuery.Parameters.Add("@USUARIO", usuario);
                contextQuery.Parameters.Add("@TIPOEVENTOID", (int)RN.Agenda.TipoEvento.TipoEventoAgenda.AnaliseTurnos);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    pode = true;
                }

                return pode;
            }

            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public bool EhModoAnaliseVagaPor(string usuario, bool privilegiado)
        {	
            //Pode analisar caso:
            //. Usuario possua perfil que pode analisar ou seja privilegiado 
            //. Esteja no periodo de analise de turno
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool pode = false;

            try
            {
                contextQuery.Command = string.Format(@" SELECT  COUNT(*)
                    FROM    HADES.DBO.HD_PADUSUARIO PU ( NOLOCK )
                            INNER JOIN HADES.DBO.TCE_PADACES_PERFIL PP ( NOLOCK ) ON PU.PADACES = PP.PADACES
                            INNER JOIN HADES.DBO.TCE_PERFIL PE ( NOLOCK ) ON PP.ID_PERFIL = PE.ID_PERFIL
                            INNER JOIN AGENDA.PARAMETROTURNOVAGA PT ( NOLOCK ) ON PT.PERFILID = PE.ID_PERFIL
                    WHERE   pt.PODEANALISAR = 1
                            {0}
                            AND EXISTS ( SELECT TOP 1
                                                1
                                         FROM   agenda.EVENTO e ( NOLOCK )
                                                INNER JOIN agenda.EVENTO_PERFIL ep ( NOLOCK ) ON e.EVENTOID = ep.EVENTOID
                                                INNER JOIN agenda.PARAMETROTURNOVAGA p ( NOLOCK ) ON e.agendaID = p.AGENDAID
                                                                                  AND ep.PERFILID = p.PERFILID
                                         WHERE  e.AGENDAID IN (
                                                SELECT DISTINCT
                                                        a.AGENDAID
                                                FROM    agenda.AGENDA a ( NOLOCK )
                                                        INNER JOIN agenda.EVENTO ae ( NOLOCK ) ON a.AGENDAID = ae.AGENDAID
                                                WHERE   ae.TIPOEVENTOID = @TIPOEVENTOID
                                                        AND ae.DATAINICIO <= GETDATE()
                                                        AND ae.DATAFIM >= GETDATE() )
                                                AND PODEANALISAR = 1
                                                AND e.AGENDAID = pt.AGENDAID
                                                AND e.DATAINICIO <= GETDATE()
                                                AND e.DATAFIM >= GETDATE()
                                                AND e.TIPOEVENTOID = @TIPOEVENTOID ) ", privilegiado ? string.Empty : "AND pu.USUARIO = @USUARIO");

                contextQuery.Parameters.Add("@USUARIO", usuario);
                contextQuery.Parameters.Add("@TIPOEVENTOID", (int)RN.Agenda.TipoEvento.TipoEventoAgenda.AnaliseVagas);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    pode = true;
                }

                return pode;
            }

            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }
    }
}
