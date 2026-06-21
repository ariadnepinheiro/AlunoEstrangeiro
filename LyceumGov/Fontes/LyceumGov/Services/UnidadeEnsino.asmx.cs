namespace Techne.Lyceum.Net.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Web.Script.Services;
    using System.Web.Services;

    [WebService(Namespace = "http://tempuri.org/"), WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1), ToolboxItem(false), ScriptService]
    public class UnidadeEnsino : WebService
    {
        [WebMethod, ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> Buscar(string texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                return new List<string>();
            }

            var dataTable = RN.UnidadeEnsino.Buscar(texto);
            var censos = new List<string>();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                censos.Add(Convert.ToString(dataRow["nome"]));
            }

            return censos;
        }
    }
}