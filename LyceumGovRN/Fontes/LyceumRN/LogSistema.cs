using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Data;

namespace Techne.Lyceum.RN
{
    public class LogSistema : RNBase
    {
        public static RetValue GravaLog(string processo, string log_descricao, DateTime data)
        {
            RetValue retorno = null;

            if (string.IsNullOrEmpty(log_descricao) || data == null)
            { 
                string erro = "Parâmetros obrigatórios de log não preenchidos";
                return new RetValue(false, erro, new Techne.Library.ErrorList(erro));
            }

            return retorno;
        }

        public static QueryTable ConsultarLog(params DateTime[] data)
        {
            if (data == null)
                return Consultar("Select ID, Data, Processo, Log_descricao");
            else
            {
                string date = data[0].Year + string.Format("{0:00}", data[0].Month) + string.Format("{0:00}", data[0].Day);
                return Consultar("Select ID, Data, Processo, Log_descricao where CONVERT(varchar, Data, 112) = " + date);
            }
        }
    }
}
