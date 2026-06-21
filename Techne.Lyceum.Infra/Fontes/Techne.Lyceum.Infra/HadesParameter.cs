using System;
using System.Globalization;
using Techne.Data;

namespace Techne
{
    internal sealed class HadesParameter
    {
        private HadesParameter()
        {
        }

        public static object Get(string system, string group, string parameter)
        {
            try
            {
                var rowParametrohades = SimpleRow.QueryFirstRow(ConnectionList.CreateConnection("Hades"), 
                                                                "SELECT valor, valor_default, tipo_valor " +
                                                                "FROM hd_parametro_hades (NOLOCK) " +
                                                                "WHERE sis = ? AND grupo = ? AND opcao= ?", 
                                                                system, group, parameter
                    );
                if (rowParametrohades == null)
                {
                    return null;
                }

                var valor = (VarChar)rowParametrohades["valor"];
                var valor_default = (VarChar)rowParametrohades["valor_default"];

                switch ((string)rowParametrohades["tipo_valor"])
                {
                    case "N":
                        if (!valor.IsNull)
                        {
                            return Convert.ToDecimal(valor, CultureInfo.InvariantCulture);
                        }
                        else if (!valor_default.IsNull)
                        {
                            return Convert.ToDecimal(valor_default, CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            return null;
                        }

                    case "D":
                        if (!valor.IsNull)
                        {
                            return Convert.ToDateTime(valor, CultureInfo.InvariantCulture);
                        }
                        else if (valor_default != null)
                        {
                            return Convert.ToDateTime(valor_default, CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            return null;
                        }

                    default:
                        if (!valor.IsNull)
                        {
                            return (string)valor;
                        }
                        else if (!valor_default.IsNull)
                        {
                            return (string)valor_default;
                        }
                        else
                        {
                            return null;
                        }
                }
            }
            catch
            {
                return null;
            }
        }

        public static DateTime GetDateTime(string system, string group, string parameter)
        {
            var result = Get(system, group, parameter);
            if (result is DateTime)
            {
                return (DateTime)result;
            }
            else
            {
                throw new FormatException();
            }
        }

        public static decimal GetDecimal(string system, string group, string parameter)
        {
            var result = Get(system, group, parameter);
            if (result is decimal)
            {
                return (decimal)result;
            }
            else
            {
                throw new FormatException();
            }
        }

        public static string GetString(string system, string group, string parameter)
        {
            var result = Get(system, group, parameter);
            if (result == null || result is string)
            {
                return (string)result;
            }
            else
            {
                throw new FormatException();
            }
        }
    }
}