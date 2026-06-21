using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Data;

namespace Techne.Lyceum.RN
{
    public class Empresas : RNBase
    {
        #region :: Consultar ::
        public static QueryTable Consultar(String empresa)
        {
            TConnection connection = Config.CreateConnection();
            QueryTable qtEmpresa = null;

            try
            {
                connection.Open();

                qtEmpresa = new QueryTable(@"
                    SELECT emp.EMPRESA,
                        emp.RAZAO_SOCIAL,
                        emp.NOME,
                        emp.ENDERECO,
                        emp.END_NUM,
                        emp.END_COMPL ,
                        emp.BAIRRO,
                        emp.MUNICIPIO,
                        mun.UF_SIGLA,
                        emp.CEP,
                        emp.CNPJ,
                        emp.INSCR_MUNICIPAL,
                        emp.INSCR_ESTADUAL,
                        emp.PORTE,
                        emp.RAMO,
                        emp.ATIVIDADE,
                        emp.NUM_EMPREGADOS,
                        emp.TIPO_CAPITAL,
                        emp.PESSOA
                    FROM LY_EMPRESA emp
                    LEFT OUTER JOIN MUNICIPIO mun ON emp.MUNICIPIO = mun.CODIGO
                    WHERE emp.EMPRESA = ? ");
                qtEmpresa.Query(connection, empresa);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally 
            {
                connection.Close();
            }
            return qtEmpresa;
        }
        #endregion

        #region :: Inserir ::
        public static RetValue Inserir(Dictionary<String, String> dadosEmpresa)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;

            try
            {
                connection.Open(true);

                String listaCampos = @"nome, endereco, end_num, end_compl, bairro, municipio, cep, cnpj, inscr_municipal, 
                    inscr_estadual, porte, ramo, atividade, num_empregados, tipo_capital";

                if (dadosEmpresa != null)
                {
                    decimal? numeroEmpregados;
                    if (!String.IsNullOrEmpty(dadosEmpresa["NumeroEmpregados"]))
                    {
                        numeroEmpregados = Convert.ToDecimal(dadosEmpresa["NumeroEmpregados"]);
                    }
                    else
                    {
                        numeroEmpregados = null;
                    }

                    Techne.Lyceum.CR.Ly_empresa.Row.Insert(connection, dadosEmpresa["Empresa"], dadosEmpresa["RazaoSocial"],
                        listaCampos,
                        dadosEmpresa["Nome"], dadosEmpresa["Endereco"], dadosEmpresa["Numero"], dadosEmpresa["Complemento"],
                        dadosEmpresa["Bairro"], dadosEmpresa["Municipio"], dadosEmpresa["CEP"], dadosEmpresa["CNPJ"], 
                        dadosEmpresa["InscricaoMunicipal"], dadosEmpresa["InscricaoEstadual"], dadosEmpresa["Porte"],
                        dadosEmpresa["Ramo"], dadosEmpresa["Atividade"], numeroEmpregados, dadosEmpresa["TipoCapital"]);

                    retorno = VerificarErro(connection.GetErrors());

                    if (retorno != null && !retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }

                    retorno = new RetValue(true, "Empresa incluída com sucesso.", null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }

            return retorno;
        }
        #endregion

        #region :: Alterar ::
        public static RetValue Alterar(Dictionary<String, String> dadosEmpresa)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;

            try
            {
                connection.Open(true);

                String listaCampos = @"razao_social, nome, endereco, end_num, end_compl, bairro, municipio, cep, cnpj, inscr_municipal, 
                    inscr_estadual, porte, ramo, atividade, num_empregados, tipo_capital";

                if (dadosEmpresa != null)
                {
                    decimal? numeroEmpregados;
                    if (!String.IsNullOrEmpty(dadosEmpresa["NumeroEmpregados"]))
                    {
                        numeroEmpregados = Convert.ToDecimal(dadosEmpresa["NumeroEmpregados"]);
                    }
                    else
                    {
                        numeroEmpregados = null;
                    }

                    Techne.Lyceum.CR.Ly_empresa.Row.Update(connection, dadosEmpresa["Empresa"], 
                        listaCampos,
                        dadosEmpresa["RazaoSocial"], dadosEmpresa["Nome"], dadosEmpresa["Endereco"], dadosEmpresa["Numero"], 
                        dadosEmpresa["Complemento"], dadosEmpresa["Bairro"], dadosEmpresa["Municipio"], dadosEmpresa["CEP"], 
                        dadosEmpresa["CNPJ"], dadosEmpresa["InscricaoMunicipal"], dadosEmpresa["InscricaoEstadual"],
                        dadosEmpresa["Porte"], dadosEmpresa["Ramo"], dadosEmpresa["Atividade"], numeroEmpregados, 
                        dadosEmpresa["TipoCapital"]);

                    retorno = VerificarErro(connection.GetErrors());

                    if (retorno != null && !retorno.Ok)
                    {
                        connection.Rollback();
                        return retorno;
                    }

                    retorno = new RetValue(true, "Empresa alterada com sucesso.", null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }

            return retorno;
        }
        #endregion

        #region :: Excluir ::
        public static RetValue Excluir(String empresa)
        {
            TConnectionWritable connection = Config.CreateWritableConnection();
            RetValue retorno = null;

            try
            {
                connection.Open(true);

                Techne.Lyceum.CR.Ly_empresa.Row.Delete(connection, empresa);

                retorno = VerificarErro(connection.GetErrors());
                if (retorno != null && !retorno.Ok)
                {
                    connection.Rollback();
                    return retorno;
                }
                retorno = new RetValue(true, "Empresa removida com sucesso.", null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return retorno;
        }
        #endregion
    }
}
