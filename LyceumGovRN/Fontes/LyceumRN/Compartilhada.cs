namespace Techne.Lyceum.RN
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Seeduc.Infra.Data;
    using Techne.Lyceum.RN.Entidades;
    using Techne.Lyceum.RN.Util;

    public class Compartilhada : RNBase
    {
        public static void Alterar(TceCompartilhada compartilhada)
        {
            var contextQuery = new ContextQuery(
                @" UPDATE  TCE_COMPARTILHADA
                    SET     NOME = @NOME ,
                            CENSO_COMPARTILHADA = @CENSO_COMPARTILHADA,
                            MATRICULA = @MATRICULA ,
                            DT_ALTERACAO = GETDATE() ,
                            CEDIDAS_MANHA = @CEDIDAS_MANHA ,
                            CEDIDAS_TARDE = @CEDIDAS_TARDE ,
                            CEDIDAS_NOITE = @CEDIDAS_NOITE,
                            REDE_ENSINO = @REDEENSINO
                    WHERE   ID_COMPARTILHADA = @ID_COMPARTILHADA ");

            contextQuery.Parameters.Add("@ID_COMPARTILHADA", compartilhada.IdCompartilhada);
            contextQuery.Parameters.Add("@NOME", compartilhada.Nome);
            contextQuery.Parameters.Add("@CENSO_COMPARTILHADA", compartilhada.CensoCompartilhada);
            contextQuery.Parameters.Add("@MATRICULA", compartilhada.Matricula);
            contextQuery.Parameters.Add("@CEDIDAS_MANHA", compartilhada.CedidasManha);
            contextQuery.Parameters.Add("@CEDIDAS_TARDE", compartilhada.CedidasTarde);
            contextQuery.Parameters.Add("@CEDIDAS_NOITE", compartilhada.CedidasNoite);
            contextQuery.Parameters.Add("@REDEENSINO", compartilhada.RedeEnsino);

            ExecutarAlteracao(contextQuery);
        }

        public static void Inserir(TceCompartilhada compartilhada)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery(
                        @" INSERT INTO dbo.TCE_COMPARTILHADA
                            ( CENSO ,
                              REDE_ENSINO ,
                              CENSO_COMPARTILHADA ,
                              NOME ,
                              CEDIDAS_MANHA ,
                              CEDIDAS_TARDE ,
                              CEDIDAS_NOITE ,                             
                              MATRICULA 
                            )
                    VALUES   ( @CENSO ,
                              @REDE_ENSINO ,
                              @CENSO_COMPARTILHADA ,
                              @NOME ,
                              @CEDIDAS_MANHA ,
                              @CEDIDAS_TARDE ,
                              @CEDIDAS_NOITE ,
                              @MATRICULA 
                            ) ");

                    contextQuery.Parameters.Add("@CENSO", compartilhada.Censo);
                    contextQuery.Parameters.Add("@REDE_ENSINO", compartilhada.RedeEnsino);
                    contextQuery.Parameters.Add("@CENSO_COMPARTILHADA", compartilhada.CensoCompartilhada);
                    contextQuery.Parameters.Add("@NOME", string.IsNullOrEmpty(compartilhada.Nome) ? null : compartilhada.Nome);
                    contextQuery.Parameters.Add("@MATRICULA", compartilhada.Matricula);
                    contextQuery.Parameters.Add("@CEDIDAS_MANHA", compartilhada.CedidasManha);
                    contextQuery.Parameters.Add("@CEDIDAS_TARDE", compartilhada.CedidasTarde);
                    contextQuery.Parameters.Add("@CEDIDAS_NOITE", compartilhada.CedidasNoite);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception)
                {
                    ctx.Abandon();
                    throw;
                }
            }
        }

        public static DataTable Listar(object censo)
        {
            if (censo == null
                || string.IsNullOrEmpty(censo.ToString()))
            {
                return null;
            }

            return Consultar(
                new ContextQuery(
                    @"SELECT  c.id_compartilhada ,
                                c.censo ,
                                c.rede_ensino ,
                                c.censo_compartilhada ,
                                c.cedidas_manha ,
                                c.cedidas_tarde ,
                                c.cedidas_noite ,                               
                                ISNULL(NULLIF(c.nome, ''), ue.nome_comp) AS 'nome'
                        FROM    DBO.TCE_COMPARTILHADA C
                                LEFT JOIN DBO.LY_UNIDADE_ENSINO UE ON C.CENSO_COMPARTILHADA = UE.UNIDADE_ENS
                                                                      AND C.REDE_ENSINO = 'Estadual'
                        WHERE   c.CENSO = @CENSO ",
                    new ContextQueryParameter("@CENSO", censo.ToString())));
        }

        public static DataTable ListarSalasDeAula(object UNIDADE_FIS)
        {

            if (UNIDADE_FIS == null
                || string.IsNullOrEmpty(UNIDADE_FIS.ToString()))
            {
                return null;
            }

            return Consultar(
                new ContextQuery(
                    @"SELECT count(d.faculdade) as totalSalas
                    FROM    dbo.LY_DEPENDENCIA d
                            INNER JOIN dbo.LY_UNIDADE_FISICA uf ON d.FACULDADE = uf.UNIDADE_FIS
                            LEFT JOIN dbo.LY_UNIDADE_FISICA_EDIFICACAO ufe ON uf.UNIDADE_FIS = ufe.UNIDADE_FIS
                                                                              AND d.PAVIMENTO = ufe.PAVIMENTO
                                                                              AND d.EDIFICACAO = ufe.EDIFICACAO
                    WHERE   ATIVA = 'S' 
                            AND uf.UNIDADE_FIS = @UNIDADE_FIS  
                            AND d.TIPO_DEPEND = 'SALA' ",
                    new ContextQueryParameter("@UNIDADE_FIS", UNIDADE_FIS.ToString())));

        }
        public static DataTable ListarCompartilhadasETotalSalas(object censo, object UNIDADE_FIS)
        {
            try
            {
                DataTable dtCompartilhadasEtotalSalas = new DataTable();
                DataTable dtCompartilhada = Listar(censo);
                DataTable dtTotalSalas = ListarSalasDeAula(UNIDADE_FIS);

                DataColumn dcColCompartilhadas = new DataColumn("totSalas", Type.GetType("System.Int32"));
                dcColCompartilhadas.DefaultValue = dtTotalSalas.Rows[0][0].ToString();
                dtCompartilhadasEtotalSalas.Merge(dtCompartilhada);
                dtCompartilhadasEtotalSalas.Columns.Add(dcColCompartilhadas);

                return dtCompartilhadasEtotalSalas;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void Remover(int id)
        {
            if (id < 1)
            {
                return;
            }

            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    ctx.ApplyModifications(
                        new ContextQuery(
                            @"DELETE  FROM dbo.TCE_COMPARTILHADA_OFERTA
                        WHERE   ID_COMPARTILHADA = @ID_COMPARTILHADA",
                            new ContextQueryParameter("@ID_COMPARTILHADA", id)));

                    ctx.ApplyModifications(
                        new ContextQuery(
                            @"DELETE  FROM dbo.TCE_COMPARTILHADA
                        WHERE   ID_COMPARTILHADA = @ID_COMPARTILHADA",
                            new ContextQueryParameter("@ID_COMPARTILHADA", id)));
                }
                catch (Exception)
                {
                    ctx.Abandon();

                    throw;
                }
            }
        }

        public bool PossuiUnidadeCompartilhada(string unidadeDestino, string unidadeOrigem)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.PossuiUnidadeCompartilhada(contexto, unidadeDestino, unidadeOrigem);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
                contexto.Dispose();
            }
        }

        public bool PossuiUnidadeCompartilhada(DataContext contexto, string unidadeDestino, string unidadeOrigem)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   TCE_COMPARTILHADA 
                                        WHERE  CENSO = @UNIDADEDESTINO
                                                AND CENSO_COMPARTILHADA = @UNIDADEORIGEM ";

            contextQuery.Parameters.Add("@UNIDADEDESTINO", unidadeDestino);
            contextQuery.Parameters.Add("@UNIDADEORIGEM", unidadeOrigem);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiUnidadeCompartilhadaDestino(string unidadeDestino)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            try
            {
                contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM   TCE_COMPARTILHADA 
                                        WHERE  CENSO = @UNIDADEDESTINO ";

                contextQuery.Parameters.Add("@UNIDADEDESTINO", unidadeDestino);

                if (contexto.GetReturnValue<int>(contextQuery) > 0)
                {
                    existe = true;
                }

                return existe;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
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
                contexto.Dispose();
            }            
        }

        public string RetornaUnidadeCompartilhada(DataContext ctx, string unidadeDestino, string unidadeOrigem)
        {
            ContextQuery contextQuery = new ContextQuery();
            DataTable dtRedeEnsingo = null;
            string idCompartilhada = string.Empty;

            contextQuery.Command = string.Format(@"
                                    SELECT ID_COMPARTILHADA 
                                        FROM   TCE_COMPARTILHADA 
                                        WHERE  CENSO = '{0}' -- Unidade de Ensino de destino
                                               AND CENSO_COMPARTILHADA = '{1}' -- Unidade de Ensino de origem"
                , unidadeDestino
                , unidadeOrigem);

            dtRedeEnsingo = ctx.GetDataTable(contextQuery);

            if (dtRedeEnsingo.Rows.Count > 0)
            {
                foreach (DataRow row in dtRedeEnsingo.Rows)
                {
                    idCompartilhada = row["ID_COMPARTILHADA"].ToString();
                }
            }

            return idCompartilhada;
        }

        public static ValidacaoDados Validar(TceCompartilhada compartilhada)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados();

            if (compartilhada == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(compartilhada.Censo))
            {
                mensagens.Add("O campo CENSO é obrigatório!");
            }

            if (string.IsNullOrEmpty(compartilhada.RedeEnsino))
            {
                mensagens.Add("O campo REDE DE ENSINO é obrigatório!");
            }
            DataTable dt = Compartilhada.ListarSalasDeAula(compartilhada.UnidadedeEnsino);
            int totalSalas = (int)dt.Rows[0][0];

            if ((compartilhada.CedidasManha) > totalSalas)
            {
                mensagens.Add("Total de salas cedidas no turno da Manhã excede o total de salas para compartilhamento");
            }
            if ((compartilhada.CedidasTarde) > totalSalas)
            {
                mensagens.Add("Total de salas cedidas no turno da Tarde excede o total de salas para compartilhamento");
            }
            if ((compartilhada.CedidasNoite) > totalSalas)
            {
                mensagens.Add("Total de salas cedidas no turno da Noite excede o total de salas para compartilhamento");
            }

            if (!string.IsNullOrEmpty(compartilhada.CensoCompartilhada)
                && !string.IsNullOrEmpty(compartilhada.Censo)
                && compartilhada.Censo == compartilhada.CensoCompartilhada)
            {
                mensagens.Add("Não é permitido selecionar a própria como compartilhada!");
            }

            if (compartilhada.RedeEnsino != "Estadual")
            {
                if (string.IsNullOrEmpty(compartilhada.Nome))
                {
                    mensagens.Add("O campo NOME é obrigatório!");
                }
            }
            if (compartilhada.RedeEnsino != "Estadual")
            {
                var contextQuery = new ContextQuery(
                            @"SELECT Count(*) 
                                FROM   VW_UNIDADE_ENSINO_ESTADUAL 
                                WHERE UNIDADE_ENS = @CENSO_COMPARTILHADA
                                   ");

                contextQuery.Parameters.Add("@CENSO_COMPARTILHADA", compartilhada.CensoCompartilhada);

                var quantidade = ExecutarFuncao<int>(contextQuery);

                if (quantidade > 0)
                {
                    mensagens.Add("Este censo pertence a uma Unidade de Ensino da Rede Estadual.");
                }
            }

            if (mensagens.Count == 0
                        && compartilhada.IdCompartilhada <= 0)
            {
                var contextQuery = new ContextQuery(
                    @"SELECT  COUNT(*)
                            FROM    TCE_COMPARTILHADA
                            WHERE   CENSO = @CENSO
                                    AND CENSO_COMPARTILHADA = @CENSO_COMPARTILHADA ");

                contextQuery.Parameters.Add("@CENSO", compartilhada.Censo);
                contextQuery.Parameters.Add("@CENSO_COMPARTILHADA", compartilhada.CensoCompartilhada);

                var quantidade = ExecutarFuncao<int>(contextQuery);

                if (quantidade > 0)
                {
                    mensagens.Add("Já existe uma UNIDADE COMPARTILHADA cadastrada com estes mesmo censo.");
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

        public static ValidacaoDados ValidarAlterar(TceCompartilhada compartilhada)
        {
            var mensagens = new List<string>();
            var validacaoDados = new ValidacaoDados();

            if (string.IsNullOrEmpty(compartilhada.Nome))
            {
                mensagens.Add("O campo NOME é obrigatório!");
            }
            DataTable dt = Compartilhada.ListarSalasDeAula(compartilhada.UnidadedeEnsino);
            int totalSalas = (int)dt.Rows[0][0];
            if ((compartilhada.CedidasManha) > totalSalas)
            {
                mensagens.Add("Total de salas cedidas no turno da Manhã excede o total de salas para compartilhamento");
            }
            if ((compartilhada.CedidasTarde) > totalSalas)
            {
                mensagens.Add("Total de salas cedidas no turno da Tarde excede o total de salas para compartilhamento");
            }
            if ((compartilhada.CedidasNoite) > totalSalas)
            {
                mensagens.Add("Total de salas cedidas no turno da Noite excede o total de salas para compartilhamento");
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

        public static ValidacaoDados ValidarRemover(int id)
        {
            var validacaoDados = new ValidacaoDados();
            DataContext contexto = null;
            List<string> mensagens = new List<string>();
            RN.CompartilhadaOferta rnCompartilhadaOferta = new CompartilhadaOferta();
            RN.UnidadeEnsinoCompartilhada.Aluno_UnidadeEnsinoCompartilhada rnAluno_UnidadeEnsinoCompartilhada = new Techne.Lyceum.RN.UnidadeEnsinoCompartilhada.Aluno_UnidadeEnsinoCompartilhada();
            
            if (id == 0)
            {
                return validacaoDados;
            }

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                if (rnCompartilhadaOferta.PossuiOfertaPor(contexto, id))
                {
                    mensagens.Add("Não é permitido realizar a exclusão desta ESCOLA COMPARTILHADA, pois ela está sendo utilizada.");
                }

                if (rnAluno_UnidadeEnsinoCompartilhada.PossuiAlunoUnidadeCompartilhadaPor(contexto, id))
                {
                    mensagens.Add("Não é permitido realizar a exclusão desta ESCOLA COMPARTILHADA, pois existe(m) aluno(s) matriculado(s) durante o compartilhamento.");
                }

            }
            catch (Exception ex)
            {
                if (contexto != null)
                {
                    contexto.Abandon();
                }
                throw new Exception(ex.Message);
            }
            finally
            {
                if (contexto != null)
                {
                    contexto.Dispose();
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        


    }
}