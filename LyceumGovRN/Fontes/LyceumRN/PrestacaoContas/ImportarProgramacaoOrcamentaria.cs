using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Seeduc.Infra.Data;

namespace Techne.Lyceum.RN.PrestacaoContas
{
    public class ImportarProgramacaoOrcamentaria
    {
        private void InsereItem(DataContext contexto, int PLANILHAORCAMENTARIAID, int FONTERECURSOID, string auxREFERENCIA, string auxVALOR, string usuario)
        {
            //insere o item da Programação Orçamentária
            //DataContext contextoitem = DataContextBuilder.FromLyceum.UsingLock();

            ContextQuery contextQueryIPO = new ContextQuery();
            contextQueryIPO.Command = @"insert into LYCEUM.PrestacaoContas.ITEMPLANILHAORCAMENTARIA (PLANILHAORCAMENTARIAID  ,FONTERECURSOID ,REFERENCIA     ,VALOR     ,RETORNOREFERENCIA,USUARIOID,DATACADASTRO ,DATAALTERACAO) values 
                                                                                                            (@PLANILHAORCAMENTARIAID ,@auxFR         ,@auxREFERENCIA ,@auxVALOR ,'E'              ,@USUARIO ,GETDATE()   ,GETDATE())";
            contextQueryIPO.Parameters.Add("@PLANILHAORCAMENTARIAID", SqlDbType.Int, PLANILHAORCAMENTARIAID);
            contextQueryIPO.Parameters.Add("@auxFR", SqlDbType.Int, FONTERECURSOID);
            contextQueryIPO.Parameters.Add("@auxREFERENCIA", SqlDbType.Int, auxREFERENCIA);
            contextQueryIPO.Parameters.Add("@auxVALOR", SqlDbType.Float, auxVALOR);
            contextQueryIPO.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuario);
            contexto.ApplyModifications(contextQueryIPO);
            //contextoitem.Dispose();
        }
        public void ImportaArquivo(DataView dv, int ano, string usuario, out int linha, out List<string> errosprocessamento, out int linhaimportado)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            DataContext contextoitens = DataContextBuilder.FromLyceum.UsingLock();

            linha = 0;
            linhaimportado = 0;
            errosprocessamento = new List<string>();

            try
            {
                int PLANILHAORCAMENTARIAID = 0;
                int PLANOTRABALHOID = 0;
                int NATUREZADESPESAID = 0;
                int FONTERECURSOID = 0;
                int REGIAOFINANCEIRAID = 0;

                var importaitens = true;
                foreach (DataRowView drv in dv) //Divide o csv
                {
                    importaitens = true;
                    PLANILHAORCAMENTARIAID = 0;
                    PLANOTRABALHOID = 0;
                    NATUREZADESPESAID = 0;
                    FONTERECURSOID = 0;
                    REGIAOFINANCEIRAID = 0;

                    linha++;
                    DataRowView rowView = dv.AddNew();
                    var auxOBJETO = drv["OBJETO"];
                    var auxND = drv["ND"];
                    var auxCODDESP = drv["COD.DESP."];
                    var auxCODAREA = drv["COD ÁREA"];
                    var auxPROCESSO = drv["PROCESSO"];
                    var auxFR = drv["FR"];
                    var auxJANEIRO = drv["JANEIRO"];
                    var auxFEVEREIRO = drv["FEVEREIRO"];
                    var auxMARCO = drv["Março"];
                    var auxABRIL = drv["ABRIL"];
                    var auxMAIO = drv["MAIO"];
                    var auxJUNHO = drv["JUNHO"];
                    var auxJULHO = drv["JULHO"];
                    var auxAGOSTO = drv["AGOSTO"];
                    var auxSETEMBRO = drv["SETEMBRO"];
                    var auxOUTUBRO = drv["OUTUBRO"];
                    var auxNOVEMBRO = drv["NOVEMBRO"];
                    var auxDEZEMBRO = drv["DEZEMBRO"];
                    var auxREFERENCIA = "";
                    var auxVALOR = "";



                    //if (linha == 1)
                    //{
                    //pega o codigo do plano
                    ContextQuery contextQuery = new ContextQuery();
                    contextQuery.Command = @"SELECT PLANOTRABALHOID FROM [LYCEUM].[PrestacaoContas].[PLANOTRABALHO]
                                                 where IDENTIFICADOR =@auxCODDESP ";

                    contextQuery.Parameters.Add("@auxCODDESP", SqlDbType.VarChar, auxCODDESP);

                    DataTable resultado = null;
                    resultado = contexto.GetDataTable(contextQuery);
                    if (resultado.Rows.Count == 0)
                    {
                        errosprocessamento.Add("Valor COD.DESP. não existe");
                        importaitens = false;
                    }
                    else
                    {
                        PLANOTRABALHOID = Convert.ToInt32(resultado.Rows[0][0]);
                    }

                    //pega o codigo do plano
                    ContextQuery contextQueryRF = new ContextQuery();
                    contextQueryRF.Command = @"SELECT REGIAOFINANCEIRAID FROM [LYCEUM].[GestaoRede].[REGIAOFINANCEIRA]
                                                 where CODIGOCG =@auxCODAREA ";

                    contextQueryRF.Parameters.Add("@auxCODAREA", SqlDbType.VarChar, auxCODAREA);

                    DataTable resultadoRF = null;
                    resultadoRF = contexto.GetDataTable(contextQueryRF);
                    if (resultadoRF.Rows.Count == 0)
                    {
                        errosprocessamento.Add("Valor COD. ÁREA não existe");
                        importaitens = false;
                    }
                    else
                    {
                        REGIAOFINANCEIRAID = Convert.ToInt32(resultadoRF.Rows[0][0]);
                    }

                    //pega o codigo da natureza despesa
                    ContextQuery contextQueryND = new ContextQuery();
                    contextQueryND.Command = @"SELECT NATUREZADESPESAID FROM [LYCEUM].[PrestacaoContas].[NATUREZADESPESA]
                                                 where codigosefaz =@codigosefaz ";

                    contextQueryND.Parameters.Add("@codigosefaz", SqlDbType.VarChar, auxND);

                    DataTable resultadond = null;
                    resultadond = contexto.GetDataTable(contextQueryND);
                    if (resultadond.Rows.Count == 0)
                    {
                        errosprocessamento.Add("Valor ND não existe");
                        importaitens = false;
                    }
                    else
                    {
                        NATUREZADESPESAID = Convert.ToInt32(resultadond.Rows[0][0]);
                    }
                    //verifica se já existe
                    ContextQuery contextQuerycND = new ContextQuery();
                    contextQuerycND.Command = @"SELECT PROCESSO FROM LYCEUM.PrestacaoContas.PLANILHAORCAMENTARIA
                                                 where PROCESSO =@auxPROCESSO ";

                    contextQuerycND.Parameters.Add("@auxPROCESSO", SqlDbType.VarChar, auxPROCESSO);


                    DataTable resultadocnd = null;
                    resultadocnd = contexto.GetDataTable(contextQuerycND);

                    if (resultadocnd.Rows.Count == 0 && PLANOTRABALHOID != 0 && NATUREZADESPESAID != 0 && REGIAOFINANCEIRAID != 0)
                    {
                        //insere Programação Orçamentária
                        ContextQuery contextQueryPO = new ContextQuery();
                        contextQueryPO.Command = @"insert into LYCEUM.PrestacaoContas.PLANILHAORCAMENTARIA (NATUREZADESPESAID ,PLANOTRABALHOID  ,REGIAOFINANCEIRAID ,PROCESSO     ,DESCRICAO  ,USUARIOID ,DATACADASTRO,DATAALTERACAO,ANO) values 
                                                                                                       (@NATUREZADESPESAID,@PLANOTRABALHOID ,@REGIAOFINANCEIRAID        ,@auxPROCESSO ,@auxOBJETO ,@USUARIO  ,GETDATE()  ,GETDATE()     ,@ano)
                                                        SELECT IDENT_CURRENT('PrestacaoContas.PLANILHAORCAMENTARIA')";

                        contextQueryPO.Parameters.Add("@REGIAOFINANCEIRAID", SqlDbType.Int, REGIAOFINANCEIRAID);
                        contextQueryPO.Parameters.Add("@PLANOTRABALHOID", SqlDbType.Int, PLANOTRABALHOID);
                        contextQueryPO.Parameters.Add("@NATUREZADESPESAID", SqlDbType.Int, NATUREZADESPESAID);
                        contextQueryPO.Parameters.Add("@auxPROCESSO", SqlDbType.VarChar, auxPROCESSO);
                        contextQueryPO.Parameters.Add("@auxOBJETO", SqlDbType.VarChar, auxOBJETO);
                        contextQueryPO.Parameters.Add("@USUARIO", SqlDbType.VarChar, usuario);
                        contextQueryPO.Parameters.Add("@ano", SqlDbType.VarChar, ano);

                        PLANILHAORCAMENTARIAID = Convert.ToInt32(contexto.GetReturnValue(contextQueryPO));
                    }
                    else
                    {
                        errosprocessamento.Add("Processo já existe.");
                        importaitens = false;
                    }


                    if (importaitens)
                    {
                        linhaimportado++;
                        //pega o codigo da fonte de recurso
                        ContextQuery contextQueryFR = new ContextQuery();
                        contextQueryFR.Command = @"SELECT FONTERECURSOID
                                                            FROM [LYCEUM].[PrestacaoContas].[FONTERECURSO]
                                                            where [CODIGOSEFAZ] = @auxFR";

                        contextQueryFR.Parameters.Add("@auxFR", SqlDbType.VarChar, auxFR);

                        DataTable resultadofr = null;
                        resultadofr = contextoitens.GetDataTable(contextQueryFR);

                        if (resultadofr.Rows.Count > 0)
                        {

                            FONTERECURSOID = Convert.ToInt32(resultadofr.Rows[0][0]);

                            //insere o item Programação Orçamentária
                            if (!Convert.ToString(auxJANEIRO).Contains("-")) { auxREFERENCIA = "1"; auxVALOR = Convert.ToString(auxJANEIRO); try { this.InsereItem(contexto, PLANILHAORCAMENTARIAID, FONTERECURSOID, auxREFERENCIA, auxVALOR, usuario); } catch (Exception ex) { } }
                            if (!Convert.ToString(auxFEVEREIRO).Contains("-")) { auxREFERENCIA = "2"; auxVALOR = Convert.ToString(auxFEVEREIRO); try { this.InsereItem(contexto, PLANILHAORCAMENTARIAID, FONTERECURSOID, auxREFERENCIA, auxVALOR, usuario); } catch (Exception ex) { } }
                            if (!Convert.ToString(auxMARCO).Contains("-")) { auxREFERENCIA = "3"; auxVALOR = Convert.ToString(auxMARCO); try { this.InsereItem(contexto, PLANILHAORCAMENTARIAID, FONTERECURSOID, auxREFERENCIA, auxVALOR, usuario); } catch (Exception ex) { } }
                            if (!Convert.ToString(auxABRIL).Contains("-")) { auxREFERENCIA = "4"; auxVALOR = Convert.ToString(auxABRIL); try { this.InsereItem(contexto, PLANILHAORCAMENTARIAID, FONTERECURSOID, auxREFERENCIA, auxVALOR, usuario); } catch (Exception ex) { } }
                            if (!Convert.ToString(auxMAIO).Contains("-")) { auxREFERENCIA = "5"; auxVALOR = Convert.ToString(auxMAIO); try { this.InsereItem(contexto, PLANILHAORCAMENTARIAID, FONTERECURSOID, auxREFERENCIA, auxVALOR, usuario); } catch (Exception ex) { } }
                            if (!Convert.ToString(auxJUNHO).Contains("-")) { auxREFERENCIA = "6"; auxVALOR = Convert.ToString(auxJUNHO); try { this.InsereItem(contexto, PLANILHAORCAMENTARIAID, FONTERECURSOID, auxREFERENCIA, auxVALOR, usuario); } catch (Exception ex) { } }
                            if (!Convert.ToString(auxJULHO).Contains("-")) { auxREFERENCIA = "7"; auxVALOR = Convert.ToString(auxJULHO); try { this.InsereItem(contexto, PLANILHAORCAMENTARIAID, FONTERECURSOID, auxREFERENCIA, auxVALOR, usuario); } catch (Exception ex) { } }
                            if (!Convert.ToString(auxAGOSTO).Contains("-")) { auxREFERENCIA = "8"; auxVALOR = Convert.ToString(auxAGOSTO); try { this.InsereItem(contexto, PLANILHAORCAMENTARIAID, FONTERECURSOID, auxREFERENCIA, auxVALOR, usuario); } catch (Exception ex) { } }
                            if (!Convert.ToString(auxSETEMBRO).Contains("-")) { auxREFERENCIA = "9"; auxVALOR = Convert.ToString(auxSETEMBRO); try { this.InsereItem(contexto, PLANILHAORCAMENTARIAID, FONTERECURSOID, auxREFERENCIA, auxVALOR, usuario); } catch (Exception ex) { } }
                            if (!Convert.ToString(auxOUTUBRO).Contains("-")) { auxREFERENCIA = "10"; auxVALOR = Convert.ToString(auxOUTUBRO); try { this.InsereItem(contexto, PLANILHAORCAMENTARIAID, FONTERECURSOID, auxREFERENCIA, auxVALOR, usuario); } catch (Exception ex) { } }
                            if (!Convert.ToString(auxNOVEMBRO).Contains("-")) { auxREFERENCIA = "11"; auxVALOR = Convert.ToString(auxNOVEMBRO); try { this.InsereItem(contexto, PLANILHAORCAMENTARIAID, FONTERECURSOID, auxREFERENCIA, auxVALOR, usuario); } catch (Exception ex) { } }
                            if (!Convert.ToString(auxDEZEMBRO).Contains("-")) { auxREFERENCIA = "12"; auxVALOR = Convert.ToString(auxDEZEMBRO); try { this.InsereItem(contexto, PLANILHAORCAMENTARIAID, FONTERECURSOID, auxREFERENCIA, auxVALOR, usuario); } catch (Exception ex) { } }
                        }
                        else
                        {
                            errosprocessamento.Add("Fonte de recurso não identificado.");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                contexto.Abandon();
                contextoitens.Abandon();
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

                contextoitens.Dispose();
            }
        }


    }
}
