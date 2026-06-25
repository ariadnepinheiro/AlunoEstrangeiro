using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class ImportacaoFgv
    {
        public void ImportaArquivo(DataView dv, int ano, int mes, int regiaofgv, string usuario, out int linha, out List<string> errosprocessamento, out int linhaimportado)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            linha = 0;
            linhaimportado = 0;
            errosprocessamento = new List<string>();

            try
            {
                //Calcula datas:
                DateTime auxdatainicio = new DateTime(ano, mes, 1);  
                DateTime auxdatafim = new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes));

                foreach (DataRowView drv in dv) //Divide o csv
                {
                    linha++;
                    DataRowView rowView = dv.AddNew();
                    var auxCODFGV = drv["CODIGO"];
                    var auxNOME = drv["NOME"];
                    var auxUNIDADEMEDIDA = drv["UNIDADEMEDIDA"];
                    //falta a quantidade de itens no layout
                    var auxVALOR = Convert.ToString(drv["VALOR"]).Replace(",", ".");
                    var auxNCM = drv["NCM"];
                    var row = drv["ROW"];
                    var auxFLAGNCM = 0;
                    var auxUNIDADEMEDIDApesquisado = 0;
                    var gravaPRODUTO = true;
                    if (Convert.ToString(drv["FLAGNCM"]) == "Não") auxFLAGNCM = 0; else auxFLAGNCM = 1;
                    DateTime dataCadastro = DateTime.Now;

                    //Verifica a Unidade de Medida
                    ContextQuery contextQuery = new ContextQuery();
                    contextQuery.Command = @" SELECT UNIDADEMEDIDAID 
                                                         FROM PrestacaoContas.UNIDADEMEDIDA (NOLOCK)
                                                         WHERE UPPER(SIGLA) = UPPER(@UNIDADEMEDIDAID) ";

                    contextQuery.Parameters.Add("@UNIDADEMEDIDAID", SqlDbType.VarChar, auxUNIDADEMEDIDA);
                    
                    DataTable resultado = null;
                    resultado = contexto.GetDataTable(contextQuery);

                    if (resultado.Rows.Count > 0)
                    {
                        auxUNIDADEMEDIDApesquisado = Convert.ToInt32(resultado.Rows[0][0]);

                        ContextQuery contextQueryPRD = new ContextQuery();
                        contextQueryPRD.Command = @" SELECT A.PRODUTOSERVICOID 
                                                                 FROM [LYCEUM].[PRESTACAOCONTAS].[PRODUTOSERVICO] A
                                                                 WHERE A.CODIGOFGV = @CODFGV AND 
                                                                    A.UNIDADEMEDIDAID =@UNIDADEMEDIDAID AND 
                                                                    A.ATIVO = 1 ";

                        contextQueryPRD.Parameters.Add("@UNIDADEMEDIDAID", SqlDbType.VarChar, auxUNIDADEMEDIDApesquisado);
                        contextQueryPRD.Parameters.Add("@CODFGV", SqlDbType.VarChar, auxCODFGV);
                        contextQueryPRD.Parameters.Add("@REGIAOFGVID", SqlDbType.VarChar, regiaofgv);

                        DataTable resultadoPRD = null;
                        resultadoPRD = contexto.GetDataTable(contextQueryPRD);

                        if (resultadoPRD.Rows.Count == 0)
                        {
                            gravaPRODUTO = true;


                            ContextQuery contextQueryPRD2 = new ContextQuery();
                            contextQueryPRD2.Command = @" SELECT A.PRODUTOSERVICOID 
                                                                 FROM [LYCEUM].[PRESTACAOCONTAS].[PRODUTOSERVICO] A
                                                                 WHERE A.NOME = @NOME AND 
                                                                    A.UNIDADEMEDIDAID =@UNIDADEMEDIDAID AND 
                                                                    A.ATIVO = 1 ";

                            contextQueryPRD2.Parameters.Add("@UNIDADEMEDIDAID", SqlDbType.VarChar, auxUNIDADEMEDIDApesquisado);
                            contextQueryPRD2.Parameters.Add("@NOME", SqlDbType.VarChar, auxNOME);
                            

                            DataTable resultadoPRD2 = null;
                            resultadoPRD2 = contexto.GetDataTable(contextQueryPRD2);

                            if (resultadoPRD2.Rows.Count == 0)
                            {
                                gravaPRODUTO = true;
                            }
                            else
                            {
                                gravaPRODUTO = false;
                                errosprocessamento.Add("Linha:" + row + "-O Produto/Serviço de Nome: " + auxNOME + ", com a Unidade de Medida: " + auxUNIDADEMEDIDA + " já consta cadastrado com outro CODIGO!!! ");

                            }
                        }
                        else
                        {
                            gravaPRODUTO = false;
                        }

                        if (gravaPRODUTO == true)
                        {                           
                            ContextQuery contextQueryPS = new ContextQuery();
                            contextQueryPS.Command = @"insert into LYCEUM.PrestacaoContas.PRODUTOSERVICO (PRODUTOSERVICOGRUPOID,NECESSITAORCAMENTO,PEQUENASDESPESAS,INVENTARIAVEL,TIPOPRODUTOSERVICOID,FINALIDADEID,CODIGOFGV,UNIDADEMEDIDAID ,NOME, NCM ,  FLAGNCM , ATIVO, USUARIOID , DATACADASTRO) values 
                                                            (736                  ,0                 ,0               ,0            ,1                   ,2           ,@CODFGV  ,@UNIDADEMEDIDA ,@NOME, @NCM, @FLAGNCM , 1    , @USUARIO  , @DATACADASTRO)";
                            contextQueryPS.Parameters.Add("@CODFGV", SqlDbType.VarChar, auxCODFGV);
                            contextQueryPS.Parameters.Add("@UNIDADEMEDIDA", SqlDbType.Int, auxUNIDADEMEDIDApesquisado);
                            contextQueryPS.Parameters.Add("@NOME", SqlDbType.VarChar, auxNOME);
                            contextQueryPS.Parameters.Add("@NCM", SqlDbType.VarChar, auxNCM);
                            contextQueryPS.Parameters.Add("@FLAGNCM", SqlDbType.Int, auxFLAGNCM);
                            contextQueryPS.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuario);
                            contextQueryPS.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, DateTime.Now);

                            contexto.ApplyModifications(contextQueryPS);
                        }

                        //Verifica se o item/unidade/região/periodo existem
                        ContextQuery consultaRegiao = new ContextQuery();
                        consultaRegiao.Command = @"select a.PRODUTOSERVICOID 
                                                        from [LYCEUM].[PrestacaoContas].[PRODUTOSERVICO] a
                                                        inner join  [LYCEUM].[PrestacaoContas].[PRODUTOSERVICOVALORMAXIMO] b on a.PRODUTOSERVICOID =b.PRODUTOSERVICOID
                                                        where a.CODIGOFGV =@CODFGV and 
                                                              b.REGIAOFGVID =@REGIAOFGVID and 
                                                              a.ATIVO=1 and 
                                                              a.UNIDADEMEDIDAID = @UNIDADEMEDIDAID and 
                                                              year(b.DATAINICIO) = @ANO and 
                                                              month(b.DATAINICIO) = @MES";

                        consultaRegiao.Parameters.Add("@CODFGV", SqlDbType.VarChar, auxCODFGV);
                        consultaRegiao.Parameters.Add("@REGIAOFGVID", SqlDbType.Int, regiaofgv);
                        consultaRegiao.Parameters.Add("@UNIDADEMEDIDAID", SqlDbType.Int, auxUNIDADEMEDIDApesquisado);
                        consultaRegiao.Parameters.Add("@ANO", SqlDbType.Int, ano);
                        consultaRegiao.Parameters.Add("@MES", SqlDbType.Int, mes);
                        
                        DataTable resultadoRegiao = null;
                        resultadoRegiao = contexto.GetDataTable(consultaRegiao);

                        if (resultadoRegiao.Rows.Count == 0)
                        {
                            ContextQuery consultaCodigoProduto = new ContextQuery();
                            consultaCodigoProduto.Command = @"SELECT PRODUTOSERVICOID
                                                                    FROM [LYCEUM].[PrestacaoContas].[PRODUTOSERVICO] A
                                                                    where CODIGOFGV =@CODFGV
                                                                    and A.UNIDADEMEDIDAID =@UNIDADEMEDIDAID AND 
                                                                    A.ATIVO = 1
                            ";

                            consultaCodigoProduto.Parameters.Add("@CODFGV", SqlDbType.VarChar, auxCODFGV);
                            consultaCodigoProduto.Parameters.Add("@UNIDADEMEDIDAID", SqlDbType.Int, auxUNIDADEMEDIDApesquisado);
                            DataTable resultadoConsultaProduto = null;
                            resultadoConsultaProduto = contexto.GetDataTable(consultaCodigoProduto);

                            if (resultadoConsultaProduto.Rows.Count > 0)
                            {

                                var auxPRODUTOSERVICOID = resultadoConsultaProduto.Rows[0][0];

                                ContextQuery contextQueryPSVM = new ContextQuery();
                                contextQueryPSVM.Command = @"INSERT INTO [LYCEUM].[PrestacaoContas].[PRODUTOSERVICOVALORMAXIMO]
                                                              (PRODUTOSERVICOID, REGIAOFGVID,VALORMAXIMO,DATAINICIO,DATAFIM,USUARIOID,DATACADASTRO) 
                                                                VALUES(@PRODUTOSERVICOID,@regiaofgv,@auxVALOR,@auxdatainicio,@auxdatafim,@auxuser,@dataCadastro)";

                                contextQueryPSVM.Parameters.Add("@PRODUTOSERVICOID", auxPRODUTOSERVICOID);
                                contextQueryPSVM.Parameters.Add("@regiaofgv", regiaofgv);
                                contextQueryPSVM.Parameters.Add("@auxVALOR", auxVALOR);
                                contextQueryPSVM.Parameters.Add("@auxdatainicio", auxdatainicio);//auxdatainicio
                                contextQueryPSVM.Parameters.Add("@auxdatafim", auxdatafim);//auxdatafim
                                contextQueryPSVM.Parameters.Add("@dataCadastro", SqlDbType.DateTime, DateTime.Now);
                                contextQueryPSVM.Parameters.Add("@auxuser", usuario);

                                contexto.ApplyModifications(contextQueryPSVM);
                                linhaimportado++;
                            }                           

                        }
                        else
                        {
                            //Verifica se o produto tem valor maior que o atual
                            ContextQuery consultaCodigo = new ContextQuery();
                            consultaCodigo.Command = @"SELECT a.PRODUTOSERVICOID , b.PRODUTOSERVICOVALORMAXIMOID, b.VALORMAXIMO, b.DATAINICIO,b.DATAFIM
                                                                                    FROM [LYCEUM].[PrestacaoContas].[PRODUTOSERVICO] a
                                                                                    inner join  [LYCEUM].[PrestacaoContas].[PRODUTOSERVICOVALORMAXIMO] b on a.PRODUTOSERVICOID =b.PRODUTOSERVICOID
                	                                                                where a.CODIGOFGV =" + auxCODFGV + " and b.REGIAOFGVID =" + regiaofgv + " and a.ATIVO=1 and year(b.DATAINICIO) ='" + ano.ToString() + "' and month(b.DATAINICIO) ='" + mes.ToString() + "'";

                            consultaCodigo.Parameters.Add("@CODFGV", SqlDbType.VarChar, auxCODFGV);
                            DataTable resultadoConsulta = null;
                            resultadoConsulta = contexto.GetDataTable(consultaCodigo);

                            // o produto já existe
                            var auxPRODUTOSERVICOID = Convert.ToInt32(resultadoConsulta.Rows[0][0]);
                            var auxPRODUTOSERVICOVALORMAXIMOID = Convert.ToInt32(resultadoConsulta.Rows[0][0]);
                            var auxVALORMAXIMO = Convert.ToDouble(resultadoConsulta.Rows[0][2]);
                            //O valor inserido é maior que o atual
                            if (auxVALORMAXIMO < Convert.ToDouble(Convert.ToString(drv["VALOR"])))
                            {
                                ContextQuery contextQueryUPSVM = new ContextQuery();
                                contextQueryUPSVM.Command = @"UPDATE LYCEUM.PRESTACAOCONTAS.PRODUTOSERVICOVALORMAXIMO 
                                                                SET VALORMAXIMO = @AUXVALOR 
                                                              FROM [LYCEUM].[PRESTACAOCONTAS].[PRODUTOSERVICO] A
                                                              INNER JOIN  [LYCEUM].[PRESTACAOCONTAS].[PRODUTOSERVICOVALORMAXIMO] B ON A.PRODUTOSERVICOID =B.PRODUTOSERVICOID
                                                              WHERE CODIGOFGV = @AUXCODFGV  
                                                                    AND REGIAOFGVID = @REGIAOFGV 
                                                                    AND YEAR(B.DATAINICIO) = @ANO
                                                                    AND MONTH(B.DATAINICIO) = @MES";
          
                                contextQueryUPSVM.Parameters.Add("@AUXVALOR", auxVALOR);
                                contextQueryUPSVM.Parameters.Add("@AUXCODFGV", SqlDbType.VarChar, auxCODFGV);
                                contextQueryUPSVM.Parameters.Add("@REGIAOFGV", SqlDbType.Int, regiaofgv);
                                contextQueryUPSVM.Parameters.Add("@ANO", SqlDbType.Int, ano);
                                contextQueryUPSVM.Parameters.Add("@MES", SqlDbType.Int, mes);

                                contexto.ApplyModifications(contextQueryUPSVM);
                            }
                            else
                            {
                                errosprocessamento.Add("Linha:" + row + "-O Produto/Serviço Código da Tabela de Preços de Valores Máximos: " + auxCODFGV + ", com a Unidade de Medida: " + auxUNIDADEMEDIDA + " já esta cadastrado!!! ");
                            }
                        }
                    }
                    else
                    {
                        errosprocessamento.Add("Linha:" + row + "-A Unidade de Medida " + auxUNIDADEMEDIDA + " não existe ");
                    }
                }

                //Grava log na tabela de importação
                DateTime dataimportacao = DateTime.Now;
                ContextQuery contextQueryPC = new ContextQuery();
                contextQueryPC.Command = @"INSERT INTO [LYCEUM].[PrestacaoContas].[IMPORTACAOFGV ] 
                                            (REGIAOFGVID
                                            , DATAREFERENCIA
                                            , TOTALITENSTABELA
                                            , TOTALITENSIMPORTADOS
                                            , USUARIOIMPORTACAO
                                            , DATAIMPORTACAO)
                                              VALUES (
                                              @REGIAOFGVID
                                            , @DATAREFERENCIA
                                            , @TOTALITENSTABELA
                                            , @TOTALITENSIMPORTADOS
                                            , @USUARIOIMPORTACAO
                                            , @DATAIMPORTACAO) ";

                contextQueryPC.Parameters.Add("@REGIAOFGVID", SqlDbType.Int, regiaofgv);
                contextQueryPC.Parameters.Add("@DATAREFERENCIA", SqlDbType.DateTime, auxdatainicio);
                contextQueryPC.Parameters.Add("@TOTALITENSTABELA", SqlDbType.Int, linha);
                contextQueryPC.Parameters.Add("@TOTALITENSIMPORTADOS", SqlDbType.Int, linhaimportado);
                contextQueryPC.Parameters.Add("@USUARIOIMPORTACAO", SqlDbType.VarChar, usuario);
                contextQueryPC.Parameters.Add("@DATAIMPORTACAO", SqlDbType.DateTime, DateTime.Now);

                contexto.ApplyModifications(contextQueryPC);

            }
            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Empty;
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

        //        public void ImportaArquivoAntigo()
        //        {
        //            foreach (DataRowView drv in dv) //Divide o csv
        //            {
        //                linha++;
        //                DataRowView rowView = dv.AddNew();
        //                var auxCODFGV = drv["CODFGV"];
        //                var auxNOME = drv["NOME"];
        //                var auxUNIDADEMEDIDA = drv["UNIDADEMEDIDA"];
        //                //falta a quantidade de itens no layout
        //                var auxVALOR = Convert.ToString(drv["VALOR"]).Replace(",", ".");
        //                var auxNCM = drv["NCM"];
        //                var auxFLAGNCM = 0;
        //                var auxUNIDADEMEDIDApesquisado = 0;
        //                var gravaPRODUTO = true;
        //                var gravaPRODUTOVALOR = true;
        //                if (drv["FLAGNCM"] == "Não") auxFLAGNCM = 0; else auxFLAGNCM = 1;
        //                DateTime dataCadastro = DateTime.Now;

        //                //Verifica a Unidade de Medida
        //                DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
        //                ContextQuery contextQuery = new ContextQuery();
        //                contextQuery.Command = @" SELECT UNIDADEMEDIDAID 
        //                                                                               FROM PrestacaoContas.UNIDADEMEDIDA (NOLOCK)
        //                                                                               WHERE upper(SIGLA) = upper(@UNIDADEMEDIDAID) ";

        //                contextQuery.Parameters.Add("@UNIDADEMEDIDAID", SqlDbType.VarChar, auxUNIDADEMEDIDA);
        //                DataTable resultado = null;
        //                resultado = ctx.GetDataTable(contextQuery);
        //                ctx.Dispose();

        //                if (resultado.Rows.Count > 0)
        //                {
        //                    auxUNIDADEMEDIDApesquisado = Convert.ToInt32(resultado.Rows[0][0]);

        //                    DataContext ctx8 = DataContextBuilder.FromLyceum.UsingNoLock();
        //                    ContextQuery contextQueryPRD = new ContextQuery();
        //                    contextQueryPRD.Command = @" select a.PRODUTOSERVICOID 
        //                                                                                             from [LYCEUM].[PrestacaoContas].[PRODUTOSERVICO] a
        //                                                                                             where a.CODIGOFGV = @CODFGV and 
        //                                                                                                a.UNIDADEMEDIDAID =@UNIDADEMEDIDAID and 
        //                                                                                                a.ATIVO=1 ";
        //                    contextQueryPRD.Parameters.Add("@UNIDADEMEDIDAID", SqlDbType.VarChar, auxUNIDADEMEDIDApesquisado);
        //                    contextQueryPRD.Parameters.Add("@CODFGV", SqlDbType.VarChar, auxCODFGV);
        //                    contextQueryPRD.Parameters.Add("@REGIAOFGVID", SqlDbType.VarChar, regiaofgv);
        //                    DataTable resultadoPRD = null;
        //                    resultadoPRD = ctx8.GetDataTable(contextQueryPRD);
        //                    ctx8.Dispose();
        //                    if (resultadoPRD.Rows.Count == 0)
        //                    { gravaPRODUTO = true; }
        //                    else
        //                    { gravaPRODUTO = false; }



        //                    if (gravaPRODUTO == true)
        //                    {
        //                        linhaimportado++;
        //                        try
        //                        {
        //                            DataContext ctx2 = DataContextBuilder.FromLyceum.UsingNoLock();
        //                            ContextQuery contextQueryPS = new ContextQuery();
        //                            contextQueryPS.Command = @"insert into LYCEUM.PrestacaoContas.PRODUTOSERVICO (PRODUTOSERVICOGRUPOID,NECESSITAORCAMENTO,PEQUENASDESPESAS,INVENTARIAVEL,TIPOPRODUTOSERVICOID,FINALIDADEID,CODIGOFGV,UNIDADEMEDIDAID ,NOME, NCM ,  FLAGNCM , ATIVO, USUARIOID , DATACADASTRO) values 
        //                                                                                                                                                 (736                  ,0                 ,0               ,0            ,1                   ,2           ,@CODFGV  ,@UNIDADEMEDIDA ,@NOME, @NCM, @FLAGNCM , 1    , @USUARIO  , @DATACADASTRO)";
        //                            contextQueryPS.Parameters.Add("@CODFGV", SqlDbType.VarChar, auxCODFGV);
        //                            contextQueryPS.Parameters.Add("@UNIDADEMEDIDA", SqlDbType.VarChar, auxUNIDADEMEDIDApesquisado);
        //                            contextQueryPS.Parameters.Add("@NOME", SqlDbType.VarChar, auxNOME);
        //                            contextQueryPS.Parameters.Add("@NCM", SqlDbType.VarChar, auxNCM);
        //                            contextQueryPS.Parameters.Add("@FLAGNCM", SqlDbType.VarChar, auxFLAGNCM);
        //                            contextQueryPS.Parameters.Add("@USUARIO", SqlDbType.VarChar, User.Identity.Name);
        //                            contextQueryPS.Parameters.Add("@DATACADASTRO", SqlDbType.VarChar, dataCadastro);
        //                            ctx2.ApplyModifications(contextQueryPS);
        //                            ctx2.Dispose();
        //                        }
        //                        catch (Exception)
        //                        {
        //                            connection.Rollback();
        //                        }
        //                        finally
        //                        {
        //                            //  connection.Close();
        //                        }
        //                    }

        //                    //Verifica se o item/unidade/região/periodo existem
        //                    DataContext ctx1 = DataContextBuilder.FromLyceum.UsingNoLock();
        //                    ContextQuery consultaRegiao = new ContextQuery();
        //                    consultaRegiao.Command = @"select a.PRODUTOSERVICOID 
        //                                                                                from [LYCEUM].[PrestacaoContas].[PRODUTOSERVICO] a
        //                                                                                inner join  [LYCEUM].[PrestacaoContas].[PRODUTOSERVICOVALORMAXIMO] b on a.PRODUTOSERVICOID =b.PRODUTOSERVICOID
        //                                                                                where a.CODIGOFGV =@CODFGV and 
        //                                                                                      b.REGIAOFGVID =@REGIAOFGVID and 
        //                                                                                      a.ATIVO=1 and 
        //                                                                                      a.UNIDADEMEDIDAID = @UNIDADEMEDIDAID and 
        //                                                                                      year(b.DATAINICIO) = @ANO and 
        //                                                                                      month(b.DATAINICIO) = @MES";

        //                    consultaRegiao.Parameters.Add("@CODFGV", SqlDbType.VarChar, auxCODFGV);
        //                    consultaRegiao.Parameters.Add("@REGIAOFGVID", SqlDbType.VarChar, regiaofgv);
        //                    consultaRegiao.Parameters.Add("@UNIDADEMEDIDAID", SqlDbType.VarChar, auxUNIDADEMEDIDApesquisado);
        //                    consultaRegiao.Parameters.Add("@ANO", SqlDbType.VarChar, txtano.Text);
        //                    consultaRegiao.Parameters.Add("@MES", SqlDbType.VarChar, txtmes.Text);
        //                    DataTable resultadoRegiao = null;
        //                    resultadoRegiao = ctx1.GetDataTable(consultaRegiao);
        //                    ctx1.Dispose();
        //                    if (resultadoRegiao.Rows.Count == 0)
        //                    {
        //                        gravaPRODUTOVALOR = true; // o produto não existe
        //                        try
        //                        {
        //                            DataContext ctx6 = DataContextBuilder.FromLyceum.UsingNoLock();
        //                            ContextQuery consultaCodigoProduto = new ContextQuery();
        //                            consultaCodigoProduto.Command = @"SELECT PRODUTOSERVICOID
        //                                                                                                FROM [LYCEUM].[PrestacaoContas].[PRODUTOSERVICO]
        //            	                                                                                where CODIGOFGV =@CODFGV";

        //                            consultaCodigoProduto.Parameters.Add("@CODFGV", SqlDbType.VarChar, auxCODFGV);
        //                            DataTable resultadoConsultaProduto = null;
        //                            resultadoConsultaProduto = ctx6.GetDataTable(consultaCodigoProduto);
        //                            var auxPRODUTOSERVICOID = resultadoConsultaProduto.Rows[0][0];
        //                            ctx6.Dispose();

        //                            DataContext ctx4 = DataContextBuilder.FromLyceum.UsingNoLock();
        //                            ContextQuery contextQueryPSVM = new ContextQuery();
        //                            contextQueryPSVM.Command = @"INSERT INTO [LYCEUM].[PrestacaoContas].[PRODUTOSERVICOVALORMAXIMO]
        //                                                                                      (PRODUTOSERVICOID, REGIAOFGVID,VALORMAXIMO,DATAINICIO,DATAFIM,USUARIOID,DATACADASTRO) 
        //                                                                                        VALUES(@PRODUTOSERVICOID,@regiaofgv,@auxVALOR,@auxdatainicio,@auxdatafim,@auxuser,@dataCadastro)";
        //                            contextQueryPSVM.Parameters.Add("@PRODUTOSERVICOID", auxPRODUTOSERVICOID);
        //                            contextQueryPSVM.Parameters.Add("@regiaofgv", regiaofgv);
        //                            contextQueryPSVM.Parameters.Add("@auxVALOR", auxVALOR);
        //                            contextQueryPSVM.Parameters.Add("@auxdatainicio", auxdatainicio);//auxdatainicio
        //                            contextQueryPSVM.Parameters.Add("@auxdatafim", auxdatafim);//auxdatafim
        //                            contextQueryPSVM.Parameters.Add("@dataCadastro", dataCadastro);
        //                            contextQueryPSVM.Parameters.Add("@auxuser", User.Identity.Name);
        //                            ctx4.ApplyModifications(contextQueryPSVM);
        //                            ctx4.Dispose();
        //                        }
        //                        catch (Exception)
        //                        { connection.Rollback(); }
        //                        finally
        //                        {
        //                            //  connection.Close();
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //Verifica se o produto tem valor maior que o atual
        //                        DataContext ctx3 = DataContextBuilder.FromLyceum.UsingNoLock();
        //                        ContextQuery consultaCodigo = new ContextQuery();
        //                        consultaCodigo.Command = @"SELECT a.PRODUTOSERVICOID , b.PRODUTOSERVICOVALORMAXIMOID, b.VALORMAXIMO, b.DATAINICIO,b.DATAFIM
        //                                                                                FROM [LYCEUM].[PrestacaoContas].[PRODUTOSERVICO] a
        //                                                                                inner join  [LYCEUM].[PrestacaoContas].[PRODUTOSERVICOVALORMAXIMO] b on a.PRODUTOSERVICOID =b.PRODUTOSERVICOID
        //            	                                                                where a.CODIGOFGV =" + auxCODFGV + " and b.REGIAOFGVID =" + regiaofgv + " and a.ATIVO=1 and year(b.DATAINICIO) ='" + txtano.Text + "' and month(b.DATAINICIO) ='" + txtmes.Text + "'";

        //                        consultaCodigo.Parameters.Add("@CODFGV", SqlDbType.VarChar, auxCODFGV);
        //                        DataTable resultadoConsulta = null;
        //                        resultadoConsulta = ctx3.GetDataTable(consultaCodigo);
        //                        ctx3.Dispose();

        //                        // o produto já existe
        //                        var auxPRODUTOSERVICOID = Convert.ToInt32(resultadoConsulta.Rows[0][0]);
        //                        var auxPRODUTOSERVICOVALORMAXIMOID = Convert.ToInt32(resultadoConsulta.Rows[0][0]);
        //                        var auxVALORMAXIMO = Convert.ToDouble(resultadoConsulta.Rows[0][2]);
        //                        //O valor inserido é maior que o atual
        //                        if (auxVALORMAXIMO < Convert.ToDouble(Convert.ToString(drv["VALOR"])))
        //                        {
        //                            try
        //                            {
        //                                var consql = @"update LYCEUM.PrestacaoContas.PRODUTOSERVICOVALORMAXIMO set VALORMAXIMO =" + auxVALOR + " where CODIGOFGV=" + auxCODFGV + " and REGIAOFGVID =" + regiaofgv + " and year(b.DATAINICIO) ='" + txtano.Text + "' and month(b.DATAINICIO) ='" + txtmes.Text + "'";
        //                                TCommand.ExecuteNonQuery(connection, consql);
        //                            }
        //                            catch (Exception)
        //                            {
        //                                connection.Rollback();
        //                            }
        //                            finally
        //                            {
        //                                //   connection.Close();
        //                            }
        //                        }
        //                        else
        //                        {
        //                            errosprocessamento.Add("Linha:" + linha + "-O Produto/Serviço CodFGV: " + auxCODFGV + " já esta cadastrado!!! ");
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    errosprocessamento.Add("Linha:" + linha + "-A Unidade de Medida " + auxUNIDADEMEDIDA + " não existe ");
        //                }
        //            }
        //            //Grava log na tabela de importação
        //            DateTime dataimportacao = DateTime.Now;
        //            DataContext ctx5 = DataContextBuilder.FromLyceum.UsingLock();
        //            ContextQuery contextQueryPC = new ContextQuery();
        //            contextQueryPC.Command = @"INSERT INTO [LYCEUM].[PrestacaoContas].[IMPORTACAOFGV ] (REGIAOFGVID ,DATAREFERENCIA ,TOTALITENSTABELA ,TOTALITENSIMPORTADOS ,USUARIOIMPORTACAO ,DATAIMPORTACAO) VALUES(" + regiaofgv + ",'" + auxdatainicio + "','" + linha + "','" + linhaimportado + "'," + User.Identity.Name + ",'" + dataimportacao + "')";
        //            ctx5.ApplyModifications(contextQueryPC);
        //            ctx5.Dispose();
        //        }
    }
}
