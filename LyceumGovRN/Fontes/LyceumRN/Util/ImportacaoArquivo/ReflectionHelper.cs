using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Globalization;
using Microsoft.VisualBasic;

namespace Techne.Lyceum.RN.Util.ImportacaoArquivo
{
    [Serializable]
    public class PropriedadeAtributoCampo
    {
        public PropertyInfo propriedade { get; set; }
        public ImportaAtributoCampo atributoCampo { get; set; }
    }

    public class ReflectionHelper
    {
        public const int numCasasDecimaisDefault = 2;

        /// <summary>
        /// Carrega os atributos definidos para a classe de importação
        /// </summary>
        /// <param name="entity">Nome da classe de entidade para importação</param>
        /// <returns></returns>
        public static ImportaAtributoArquivo CarregaAtributosImportacaoArquivo(object entity)
        {
            object[] attributes = entity.GetType().GetCustomAttributes(false);
            foreach (object attribute in attributes)
            {
                if (attribute is ImportaAtributoArquivo)
                {
                    return (ImportaAtributoArquivo)attribute;
                }
            }
            return null;
        }

        /// <summary>
        /// Carrega uma lista com os atributos e propriedades da classe para importação
        /// </summary>
        /// <param name="entidade"></param>
        /// <returns></returns>
        public static List<PropriedadeAtributoCampo> GetListProperties(object entidade)
        {
            List<PropriedadeAtributoCampo> listPropriedadeAtributos = new List<PropriedadeAtributoCampo>();

            foreach (PropertyInfo propriedade in entidade.GetType().GetProperties())
            {
                PropriedadeAtributoCampo propriedadeAtributoCampo = new PropriedadeAtributoCampo();

                propriedadeAtributoCampo.propriedade = propriedade;

                ImportaAtributoCampo[] atributos = (ImportaAtributoCampo[])propriedade.GetCustomAttributes(typeof(ImportaAtributoCampo), false);

                if (atributos.Length > 0)
                    propriedadeAtributoCampo.atributoCampo = atributos[0];

                listPropriedadeAtributos.Add(propriedadeAtributoCampo);
            }

            return listPropriedadeAtributos;
        }

        /// <summary>
        /// Adiciona o valor do campo e realiza as validações de acordo com os atributos colocados para a propriedade
        /// </summary>
        /// <param name="listPropriedadeAtributos"></param>
        /// <param name="entidade"></param>
        /// <param name="valor"></param>
        /// <param name="indiceCampo"></param>
        public static void SetPropertyValue(List<PropriedadeAtributoCampo> listPropriedadeAtributos, object entidade, object valor, int indiceCampo, ref string Erro)
        {
            PropriedadeAtributoCampo propriedadeAtributoCampo = listPropriedadeAtributos.Find(x => x.atributoCampo.Posicao == indiceCampo);

            ImportaAtributoCampo campo = propriedadeAtributoCampo.atributoCampo;
            PropertyInfo propriedade = propriedadeAtributoCampo.propriedade;

            if (campo.Posicao == indiceCampo)
            {
                if (campo.Requirido)
                {
                    if (((string)valor).Trim().Length == 0)
                        Erro = string.Format("Valor é obrigatório para o campo '{0}'", campo.NomeCampo);
                }

                if (Erro == string.Empty)
                {
                    if (campo.RetiraEspacos)
                    {
                        valor = ((string)valor).Trim();
                    }

                    if (!campo.PermitePontos)
                    {
                        if (((string)valor).IndexOf('.') != -1)
                            Erro = string.Format("Valor inválido para o campo '{0}'", campo.NomeCampo);
                    }

                    if (EhValidoValorCampo(campo, valor))
                    {
                        try
                        {
                            valor = PreparaValorCampo(campo, propriedade, valor);
                            propriedade.SetValue(entidade, valor, null);
                        }
                        catch (Exception)
                        {
                            Erro = string.Format("Valor inválido para o campo '{0}'", campo.NomeCampo);
                        }
                    }
                    else
                    {
                        Erro = string.Format("Valor inválido para o campo '{0}'", campo.NomeCampo);
                    }
                }
            }
        }

        /// <summary>
        /// Determina se o valor do campo é válido ou não
        /// </summary>
        /// <param name="campo"></param>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static bool EhValidoValorCampo(ImportaAtributoCampo campo, object valor)
        {
            if (campo.ValidacaoRequirida)
            {
                if (campo.ValidacaoRegExp != null && campo.ValidacaoRegExp.Length > 0)
                {
                    if (!Regex.IsMatch((string)valor, campo.ValidacaoRegExp))
                    {
                        return false;
                    }
                }
                if (campo.TamanhoCampo > 0 && ((string)valor).Length != campo.TamanhoCampo)
                {
                    return false;
                }
                if (campo.TamanhoMaximoCampo > 0 && ((string)valor).Length > campo.TamanhoMaximoCampo)
                {
                    return false;
                }
                if (verificaTipoNumerico(campo))
                {
                    if (!Information.IsNumeric(valor))
                    {
                        return false;
                    }
                    if (valor.ToString().Length > 0 && Int64.Parse(valor.ToString(), NumberStyles.Any) == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool verificaTipoNumerico(ImportaAtributoCampo campo)
        {
            DataType tipoDado = campo.TipoDado;

            if (tipoDado == DataType.Integer)
            {
                return true;
            }

            return false;
        }

        /// <summary> 
        /// Modifica o valor do campo para o tipo de dado da propriedade
        /// </summary> 
        /// <param name="campo"></param> 
        /// <param name="propriedade"></param> 
        /// <param name="valor"></param> 
        /// <returns></returns> 
        public static object PreparaValorCampo(ImportaAtributoCampo campo, PropertyInfo propriedade, object valor)
        {
            // Try to convert the input string value to the proper type 
            // of the data, only if data type is not string 
            if (valor is IConvertible && campo.TipoDado != DataType.String)
            {
                Type t = propriedade.PropertyType;

                t = Nullable.GetUnderlyingType(t) ?? t;

                // Adds trailing zeros
                // if the type is decimal
                if (t.Name.Equals("Decimal"))
                {
                    if (!String.IsNullOrEmpty(valor.ToString()))
                        valor = String.Format("{0:F" + (campo.NumeroCasasDecimais != 0 ? campo.NumeroCasasDecimais : numCasasDecimaisDefault) + "}", valor);
                }

                valor = String.IsNullOrEmpty(valor.ToString()) ? null : Convert.ChangeType(valor, t, new CultureInfo("pt-BR"));
            }
            else
            {
                // Custom conversion types
            }

            return valor;
        }

    }
}