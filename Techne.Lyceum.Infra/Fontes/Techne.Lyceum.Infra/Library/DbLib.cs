using System;
using System.Collections;
using Techne.Data;

namespace Techne
{
    internal abstract class DbLib
    {
        public static Rdbms GetRdbms(string connectionString)
        {
            var connInfo = StrLib.StrToDictionary(connectionString, ';', "=", true, true);
            if (!connInfo.Contains("provider"))
            {
                throw new ArgumentException("A connection string informada não possui a propriedade PROVIDER", "connectionString");
            }

            switch (((string)connInfo["provider"]).ToUpper())
            {
                case "SQLOLEDB.1":
                    return Rdbms.SQLServer;
                case "MSDAORA.1":
                case "ORAOLEDB.ORACLE":
                    return Rdbms.Oracle;
                default:
                    return Rdbms.Unknown;
            }
        }
    }
}