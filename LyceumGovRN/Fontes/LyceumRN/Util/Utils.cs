namespace Techne.Lyceum.RN.Util
{
    using System;
    using System.Web.UI;
    using System.Reflection;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Web.UI.WebControls;
    using System.Text.RegularExpressions;

    public static class Utils
    {
        public static string AplicarMascaraCNPJ(this string cnpj)
        {
            if (String.IsNullOrEmpty(cnpj))
            {
                return String.Empty;
            }

            long intCnpj;

            var valido = Int64.TryParse(Convert.ToString(cnpj.RetirarMascaraCNPJ()), out intCnpj);

            if (valido)
            {
                return String.Format(@"{0:00\.000\.000\/0000-00}", intCnpj);
            }

            return cnpj;
        }

        public static string RetiraEspacosDuplos(this string texto)
        {
            if (texto == null)
            {
                return String.Empty;
            }

            while (texto.IndexOf("  ") >= 0)
            {
                texto = texto.Replace("  ", " ");
            }

            return texto;
        }

        public static string AplicarMascaraCPF(this string cpf)
        {
            if (String.IsNullOrEmpty(cpf))
            {
                return String.Empty;
            }

            long intCpf;

            var valido = Int64.TryParse(cpf.RetirarMascaraCPF(), out intCpf);

            if (valido)
            {
                return String.Format(@"{0:000\.000\.000-00}", intCpf);
            }

            return cpf;
        }

        public static string AplicarMascaraTelefoneComDDD(this string telefone)
        {
            var fone = telefone.RetirarMascaraTelefone();

            if (fone.Length > 0
                && fone.Length >= 10)
            {
                fone = fone.Substring(fone.Length - 10, 10);
            }

            long intFone;

            var valido = Int64.TryParse(Convert.ToString(fone), out intFone);

            if (valido)
            {
                return String.Format(@"{0:(00)0000-0000}", intFone);
            }

            return telefone;
        }

        public static string AplicarMascaraCelularComDDD(this string celular)
        {
            var cel = celular.RetirarMascaraTelefone();

            if (cel.Length > 0)
            {
                if (cel.Length == 10)
                {
                    cel = cel.Substring(cel.Length - 10, 10);
                }
                else if (cel.Length == 11)
                {
                    cel = cel.Substring(cel.Length - 11, 11);
                }
            }

            long intFone;

            var valido = Int64.TryParse(Convert.ToString(cel), out intFone);

            if (valido)
            {
                if (cel.Length == 10)
                {
                    return String.Format(@"{0:(00)0000-0000}", intFone);
                }
                else if (cel.Length == 11)
                {
                    return String.Format(@"{0:(00)00000-0000}", intFone);
                }

            }

            return celular;
        }

        public static string AplicarMascaraTelefoneSemDDD(this string telefone)
        {
            var fone = telefone.RetirarMascaraTelefone();

            if (fone.Length > 0
                && fone.Length >= 8)
            {
                fone = fone.Substring(fone.Length - 8, 8);
            }

            long intFone;

            var valido = Int64.TryParse(Convert.ToString(fone), out intFone);

            if (valido)
            {
                return String.Format(@"{0:0000-0000}", intFone);
            }

            return telefone;
        }

        public static string GetStringValue(this Enum value)
        {
            var type = value.GetType();
            var fieldInfo = type.GetField(value.ToString());
            var attribs = fieldInfo.GetCustomAttributes(typeof(RNBase.StringValueAttribute), false) as RNBase.StringValueAttribute[];

            return attribs != null && attribs.Length > 0 ? attribs[0].StringValue : null;
        }

        public static string GetEnumDescription(Enum value)
        {
            // Get the Description attribute value for the enum value
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                    typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }

        public static bool IsNullOrEmpty(object value)
        {
            return String.IsNullOrEmpty(Convert.ToString(value));
        }

        public static bool VerificaTriploCaracter(string dado)
        {

            string nome = dado.ToUpper();
            int rep = 0;
            string seq = "";
            bool bSair = false;

            if (dado == null)
            {
                return false;
            }


            for (int x = 0; x < nome.Length; x++)
            {

                for (int y = x + 1; y < nome.Length; y++)
                {
                    if (nome.Substring(x, 1).ToString() == nome.Substring(y, 1).ToString())
                    {
                        rep = rep + 1;
                        seq = seq + nome.Substring(y, 1);

                    }
                    else
                    {
                        rep = 0;
                        seq = "";

                        break;
                    }
                    if (rep == 2)
                    {
                        seq = nome.Substring(x, 1) + seq;
                        bSair = true;
                        break;
                    }

                }
                if (bSair == true)
                {

                    break;
                }

            }

            if (seq.Length == 3)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        public static string TiraBrancosConsec(string dado)
        {

            string res = dado.Trim();
            string cBrancos = "";
            int ini = -1;
            int fim = -1;
            int posInicial = 0;
            int x = 0;
            x = posInicial;
            while (x < res.Length)
            {
                if (res.Substring(x, 1) == " " && cBrancos == "" && ini == -1)
                {
                    ini = x;
                }

                if (ini >= 0 && res.Substring(x, 1) == " ")
                {
                    cBrancos = cBrancos + res.Substring(x, 1);
                }

                if (res.Substring(x, 1) != " " && cBrancos != "")
                {
                    fim = x - 1;
                    cBrancos = res.Substring(ini, fim - ini + 1);
                    posInicial = ini;
                    x = posInicial;
                    res = res.Replace(cBrancos, " ");
                    fim = -1;
                    ini = -1;
                    cBrancos = "";
                }

                x = x + 1;


            }

            return res;

        }

        private static char[] GetAccents()
        {
            char[] accents = new char[256];

            for (int i = 0; i < 256; i++)
                accents[i] = (char)i;

            accents[(byte)'á'] = accents[(byte)'à'] = accents[(byte)'ã'] = accents[(byte)'â'] = accents[(byte)'ä'] = 'a';
            accents[(byte)'Á'] = accents[(byte)'À'] = accents[(byte)'Ã'] = accents[(byte)'Â'] = accents[(byte)'Ä'] = 'A';

            accents[(byte)'é'] = accents[(byte)'è'] = accents[(byte)'ê'] = accents[(byte)'ë'] = 'e';
            accents[(byte)'É'] = accents[(byte)'È'] = accents[(byte)'Ê'] = accents[(byte)'Ë'] = 'E';

            accents[(byte)'í'] = accents[(byte)'ì'] = accents[(byte)'î'] = accents[(byte)'ï'] = 'i';
            accents[(byte)'Í'] = accents[(byte)'Ì'] = accents[(byte)'Î'] = accents[(byte)'Ï'] = 'I';

            accents[(byte)'ó'] = accents[(byte)'ò'] = accents[(byte)'ô'] = accents[(byte)'õ'] = accents[(byte)'ö'] = 'o';
            accents[(byte)'Ó'] = accents[(byte)'Ò'] = accents[(byte)'Ô'] = accents[(byte)'Õ'] = accents[(byte)'Ö'] = 'O';

            accents[(byte)'ú'] = accents[(byte)'ù'] = accents[(byte)'û'] = accents[(byte)'ü'] = 'u';
            accents[(byte)'Ú'] = accents[(byte)'Ù'] = accents[(byte)'Û'] = accents[(byte)'Ü'] = 'U';

            accents[(byte)'ç'] = 'c';
            accents[(byte)'Ç'] = 'C';

            accents[(byte)'ñ'] = 'n';
            accents[(byte)'Ñ'] = 'N';

            accents[(byte)'ÿ'] = accents[(byte)'ý'] = 'y';
            accents[(byte)'Ý'] = 'Y';

            return accents;
        }

        public static string RemoveAcentos(this string text)
        {
            var s_Accents = GetAccents();
            text = text.Replace("’", "").Replace("'", "").Replace("‘", "");
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < text.Length; i++)
                sb.Append(s_Accents[text[i]]);

            return sb.ToString();
        }

        public static string RetirarNumeros(this string str)
        {
            if (str == null)
            {
                return String.Empty;
            }

            foreach (var c in str)
            {
                if (c == '1'
                    && c == '2'
                    && c == '3'
                    && c == '4'
                    && c == '5'
                    && c == '6'
                    && c == '7'
                    && c == '8'
                    && c == '9'
                    && c == '0')
                {
                    var ch = c.ToString();

                    str = str.Replace(ch, String.Empty);
                }
            }

            return str;
        }

        public static string RetiraCaracteresEspeciais(this string texto)
        {
            string ret = texto;

            if (string.IsNullOrEmpty(ret))
                return ret;

            ret = ret.RemoveAcentos();

            ret = System.Text.RegularExpressions.Regex.Replace(ret, @"[^0-9a-zA-ZéúíóáÉÚÍÓÁèùìòàÈÙÌÒÀõãñÕÃÑêûîôâÊÛÎÔÂëÿüïöäËYÜÏÖÄçÇ\s]+?", string.Empty);

            return ret;
        }

        public static string RetirarCaracteres(this string str)
        {
            if (str == null)
            {
                return String.Empty;
            }

            foreach (var c in str)
            {
                if (c != '1'
                    && c != '2'
                    && c != '3'
                    && c != '4'
                    && c != '5'
                    && c != '6'
                    && c != '7'
                    && c != '8'
                    && c != '9'
                    && c != '0')
                {
                    var ch = c.ToString();

                    str = str.Replace(ch, String.Empty);
                }
            }

            return str;
        }

        public static string RetirarMascaraCNPJ(this string cnpj)
        {
            if (cnpj == null)
            {
                return String.Empty;
            }

            return cnpj
                .Replace(".", String.Empty)
                .Replace("-", String.Empty)
                .Replace("/", String.Empty)
                .Replace("\\", String.Empty);
        }

        public static string RetirarMascaraCPF(this string cpf)
        {
            if (cpf == null)
            {
                return String.Empty;
            }

            return cpf.Replace(".", String.Empty).Replace("-", String.Empty);
        }

        public static string RetirarMascara(string campo)
        {
            if (campo == null)
            {
                return string.Empty;
            }

            return campo.Replace(".", String.Empty).Replace("-", String.Empty).Replace("/", String.Empty).Replace("(", String.Empty).Replace(")", String.Empty);
        }

        public static string RetirarMascaraRG(this string rg)
        {
            if (rg == null)
            {
                return String.Empty;
            }

            return rg
                .Replace(".", String.Empty)
                .Replace("-", String.Empty);
        }

        public static string RetirarMascaraTelefone(this string telefone)
        {
            if (telefone == null)
            {
                return String.Empty;
            }

            return telefone
                .Replace("(", String.Empty)
                .Replace(")", String.Empty)
                .Replace("-", String.Empty);
        }

        public static string SubstituirAspasSimplesPorDupla(this string parametro)
        {
            if (!String.IsNullOrEmpty(parametro))
            {
                return parametro.Replace("'", "''");
            }

            return String.Empty;
        }

        public static string SubstituirPontoPorVirgula(this decimal valor)
        {
            return valor
                .ToString()
                .Replace(".", ",");
        }

        public static string SubstituirVirgulaPorPonto(this decimal valor)
        {
            return valor
                .ToString()
                .Replace(",", ".");
        }

        public static decimal? ToDecimal(this ProcessoSeletivo.Status status)
        {
            return (decimal?)status;
        }

        public static DateTime? ToNullableDateTime(object value)
        {
            var strValue = Convert.ToString(value);

            if (String.IsNullOrEmpty(strValue))
            {
                return null;
            }

            DateTime dec;

            return DateTime.TryParse(strValue, out dec) ? dec : (DateTime?)null;
        }

        public static decimal? ToNullableDecimal(object value)
        {
            var strValue = Convert.ToString(value);

            if (String.IsNullOrEmpty(strValue))
            {
                return null;
            }

            decimal dec;

            return Decimal.TryParse(strValue, out dec) ? dec : (decimal?)null;
        }

        public static bool ValidarCpf(this string scpf)
        {
            // Verifica se só contém dígitos.
            foreach (var c in scpf)
            {
                if (!Char.IsDigit(c))
                {
                    return false;
                }
            }

            // Tamanho entre 3 e 11
            if (scpf.Length < 3 || scpf.Length > 11)
            {
                return false;
            }

            // Preenche com zeros à esquerda até totalizar 11 dígitos
            scpf = scpf.PadLeft(11, '0');

            // Não permite CPFs com códigos 00000000000 à 999999999
            if (scpf == "00000000000"
                || scpf == "11111111111"
                || scpf == "22222222222"
                || scpf == "33333333333"
                || scpf == "44444444444"
                || scpf == "55555555555"
                || scpf == "66666666666"
                || scpf == "77777777777"
                || scpf == "88888888888"
                || scpf == "99999999999")
            {
                return false;
            }

            var digito1 = Byte.Parse(scpf[9].ToString());
            var digito2 = Byte.Parse(scpf[10].ToString());

            // Verificando o primeiro dígito
            var somador1 = 0;

            for (var i = 10; i >= 2; i--)
            {
                somador1 += Byte.Parse(scpf[11 - i - 1].ToString()) * i;
            }

            somador1 -= somador1 / 11 * 11;

            if (somador1 == 0
                || somador1 == 1)
            {
                if (digito1 != 0)
                {
                    return false;
                }
            }
            else if (11 - somador1 != digito1)
            {
                return false;
            }

            // Verificando o segundo dígito
            var somador2 = 0;

            for (var i = 11; i >= 2; i--)
            {
                somador2 += Byte.Parse(scpf[12 - i - 1].ToString()) * i;
            }

            somador2 -= somador2 / 11 * 11;

            if (somador2 == 0 || somador2 == 1)
            {
                if (digito2 != 0)
                {
                    return false;
                }
            }
            else if (11 - somador2 != digito2)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Exibe Mensagens na tela
        /// </summary>
        /// <param name="mensagemErro"></param>
        public static void ExibeMensagem(string mensagem, Page pg)
        {
            ScriptManager.RegisterStartupScript(pg, pg.GetType(), Guid.NewGuid().ToString(), "alert('" + mensagem + "')", true);
        }

        public static int CalcularIdade(DateTime dNasc)
        {
            int idade = DateTime.Now.Year - dNasc.Year;

            if (DateTime.Now.Month < dNasc.Month ||
                (DateTime.Now.Month == dNasc.Month &&
                 DateTime.Now.Day < dNasc.Day))
                idade--;
            return idade;
        }

        public static int CalcularIdadePorData(DateTime dNasc, DateTime data)
        {
            int idade = data.Year - dNasc.Year;

            if (data.Month < dNasc.Month ||
                (data.Month == dNasc.Month &&
                 data.Day < dNasc.Day))
                idade--;
            return idade;
        }

        public static bool IsNullOrEmptyOrWhiteSpace(this string input)
        {
            return string.IsNullOrEmpty(input) || input.Trim() == string.Empty;
        }

        public static string Quoted(this string str)
        {
            return "'" + str + "'";
        }

        /// <summary>
        /// Versão 1.0
        /// Cria um datatable com as colunas nomeadas a partir das propriedades do objeto T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> collection) where T : class, new()
        {
            DataTable dt = new DataTable();
            Type t = typeof(T);
            PropertyInfo[] pia = t.GetProperties();
            //Create the columns in the DataTable
            foreach (PropertyInfo pi in pia)
            {
                dt.Columns.Add(pi.Name, pi.PropertyType);
            }
            //Populate the table
            foreach (T item in collection)
            {
                DataRow dr = dt.NewRow();
                dr.BeginEdit();
                foreach (PropertyInfo pi in pia)
                {
                    dr[pi.Name] = pi.GetValue(item, null);
                }
                dr.EndEdit();
                dt.Rows.Add(dr);
            }
            return dt;
        }

        /// <summary>
        /// Convert Data Table To List of Type T
        /// </summary>
        /// <typeparam name="T">Target Class to convert data table to List of T </typeparam>
        /// <param name="datatable">Data Table you want to convert it</param>
        /// <returns>List of Target Class</returns>
        public static List<T> ToList<T>(this DataTable datatable) where T : new()
        {
            List<T> Temp = null;

            try
            {
                if (datatable.Rows.Count > 0)
                {
                    Temp = new List<T>();
                    List<string> columnsNames = new List<string>();
                    foreach (DataColumn DataColumn in datatable.Columns)
                        columnsNames.Add(DataColumn.ColumnName);
                    Temp = datatable.AsEnumerable().ToList().ConvertAll<T>(row => getObject<T>(row, columnsNames));
                }
                return Temp;
            }
            catch { return Temp; }
        }

        /// <summary>
        /// Versão 1.0
        /// As propriedades do objeto T a ser criado devem ter o mesmo nome das colunas do DataTable.
        /// </summary>
        /// <typeparam name="T">Objeto a ser criado</typeparam>
        /// <param name="row">linha do datatable</param>
        /// <param name="columnsName">nome das colunas do datatable</param>
        /// <returns>Objeto T preenchido com os valores da linha do Datatable</returns>
        public static T getObject<T>(DataRow row, List<string> columnsName) where T : new()
        {
            T obj = new T();
            try
            {
                string columnname = "";
                string value = "";
                PropertyInfo[] Properties; Properties = typeof(T).GetProperties();
                foreach (PropertyInfo objProperty in Properties)
                {
                    columnname = columnsName.Find(name => name.ToLower() == objProperty.Name.ToLower());
                    if (!string.IsNullOrEmpty(columnname))
                    {
                        value = row[columnname].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (Nullable.GetUnderlyingType(objProperty.PropertyType) != null)
                            {
                                value = row[columnname].ToString().Replace("$", "").Replace(",", "");
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(Nullable.GetUnderlyingType(objProperty.PropertyType).ToString())), null);
                            }
                            else
                            {
                                value = row[columnname].ToString().Replace("%", "");
                                objProperty.SetValue(obj, Convert.ChangeType(value, Type.GetType(objProperty.PropertyType.ToString())), null);
                            }
                        }
                    }
                } return obj;
            }
            catch { return obj; }
        }

        public static byte[] ToByteArray(this string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string RecuperaPossiveisPeriodos(int periodoOrigem)
        {
            string possiveisPeriodos;

            //Verificar possiveis periodos com relação:                      
            if (periodoOrigem == 2)
            {
                //2 so pode considerar como equivalente 2  
                possiveisPeriodos = "2";
            }
            else
            {
                //0 pode considerar como equivalente 0 ou 1 
                //1 pode considerar como equivalente 0 ou 1
                possiveisPeriodos = "0 , 1";
            }

            return possiveisPeriodos;
        }

        public static string RecuperaPossiveisFuturosPeriodos(int periodoOrigem)
        {
            string possiveisPeriodos;

            //Verificar possiveis futuros periodos com relação:                      
            if (periodoOrigem == 1)
            {
                //1 so pode considerar como futuro 2  
                possiveisPeriodos = "2";
            }
            else
            {
                //0 pode considerar como futuro 0 ou 1 
                //2 considerar como futuro 0 ou 1
                possiveisPeriodos = "0 , 1";
            }

            return possiveisPeriodos;
        }

        public static string RecuperaPossiveisPeriodosCompleto(int periodoOrigem)
        {
            string possiveisPeriodos;

            //Verificar possiveis periodos com relação:         
            if (periodoOrigem == 2)
            {
                //2 pode considerar como equivalente 0 ou 2  
                possiveisPeriodos = "0, 2";
            }
            else if (periodoOrigem == 1)
            {
                //1 pode considerar como equivalente 0 ou 1
                possiveisPeriodos = "0, 1";
            }
            else
            {
                //0 pode considerar como equivalente 0 ou 1 ou 2
                possiveisPeriodos = "0, 1, 2";
            }

            return possiveisPeriodos;
        }

        public static string RecuperaPossiveisPeriodosParaTurmaPrincipal(int periodo)
        {
            string possiveisPeriodos;

            //Verificar possiveis periodos em que a turma principal pode estar:                      
            if (periodo == 2)
            {
                possiveisPeriodos = "0 , 2";
            }
            else if (periodo == 1)
            {
                possiveisPeriodos = "0 , 1";
            }
            else
            {
                possiveisPeriodos = "0";
            }

            return possiveisPeriodos;
        }

        public static string ObtemDescricaoMesPor(int mes)
        {
            string descricao = null; ;

            if (mes > 0 || mes <= 12)
            {
                switch (mes)
                {
                    case 1:
                        {
                            descricao = "Janeiro";
                            break;
                        }
                    case 2:
                        {
                            descricao = "Fevereiro";
                            break;
                        }
                    case 3:
                        {
                            descricao = "Março";
                            break;
                        }
                    case 4:
                        {
                            descricao = "Abril";
                            break;
                        }
                    case 5:
                        {
                            descricao = "Maio";
                            break;
                        }
                    case 6:
                        {
                            descricao = "Junho";
                            break;
                        }
                    case 7:
                        {
                            descricao = "Julho";
                            break;
                        }
                    case 8:
                        {
                            descricao = "Agosto";
                            break;
                        }
                    case 9:
                        {
                            descricao = "Setembro";
                            break;
                        }
                    case 10:
                        {
                            descricao = "Outubro";
                            break;
                        }
                    case 11:
                        {
                            descricao = "Novembro";
                            break;
                        }
                    case 12:
                        {
                            descricao = "Dezembro";
                            break;
                        }
                };
            }

            return descricao;
        }

        public static int? ObtemCodigoMesPor(string descricao)
        {
            int? mes = null;

            if (!descricao.IsNullOrEmptyOrWhiteSpace())
            {
                switch (descricao)
                {
                    case "Janeiro":
                        {
                            mes = 1;
                            break;
                        }
                    case "Fevereiro":
                        {
                            mes = 2;
                            break;
                        }
                    case "Março":
                        {
                            mes = 3;
                            break;
                        }
                    case "Abril":
                        {
                            mes = 4;
                            break;
                        }
                    case "Maio":
                        {
                            mes = 5;
                            break;
                        }
                    case "Junho":
                        {
                            mes = 6;
                            break;
                        }
                    case "Julho":
                        {
                            mes = 7;
                            break;
                        }
                    case "Agosto":
                        {
                            mes = 8;
                            break;
                        }
                    case "Setembro":
                        {
                            mes = 9;
                            break;
                        }
                    case "Outubro":
                        {
                            mes = 10;
                            break;
                        }
                    case "Novembro":
                        {
                            mes = 11;
                            break;
                        }
                    case "Dezembro":
                        {
                            mes = 12;
                            break;
                        }
                };
            }

            return mes;
        }

        public static void Moeda(ref TextBox txt)
        {
            string n = string.Empty;
            double v = 0;
            try
            {
                n = txt.Text.Replace(",", "").Replace(".", "");
                if (n.Equals(""))
                    n = "";
                n = n.PadLeft(3, '0');
                if (n.Length > 3 && n.Substring(0, 1) == "0")
                    n = n.Substring(1, n.Length - 1);
                v = Convert.ToDouble(n) / 100;
                txt.Text = string.Format("{0:N}", v);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static DataTable ListaMes()
        {
            DataTable listaMes = new DataTable();
            listaMes.Columns.Add("CODIGO");
            listaMes.Columns.Add("DESCRICAO");

            listaMes.Rows.Add(1, "Janeiro");
            listaMes.Rows.Add(2, "Fevereiro");
            listaMes.Rows.Add(3, "Março");
            listaMes.Rows.Add(4, "Abril");
            listaMes.Rows.Add(5, "Maio");
            listaMes.Rows.Add(6, "Junho");
            listaMes.Rows.Add(7, "Julho");
            listaMes.Rows.Add(8, "Agosto");
            listaMes.Rows.Add(9, "Setembro");
            listaMes.Rows.Add(10, "Outubro");
            listaMes.Rows.Add(11, "Novembro");
            listaMes.Rows.Add(12, "Dezembro");

            return listaMes;
        }

        public static bool IsValidSqlDatetime(string someval)
        {
            bool valid = false;
            DateTime testDate = DateTime.MinValue;
            DateTime minDateTime = DateTime.MaxValue;
            DateTime maxDateTime = DateTime.MinValue;

            minDateTime = new DateTime(1753, 1, 1);
            maxDateTime = new DateTime(9999, 12, 31, 23, 59, 59, 997);

            if (DateTime.TryParse(someval, out testDate))
            {
                if (testDate >= minDateTime && testDate <= maxDateTime)
                {
                    valid = true;
                }
            }

            return valid;
        }

        public static string FormataCep(this string cep)
        {
            try
            {
                return Convert.ToUInt64(cep).ToString(@"00000\-000");
            }
            catch
            {
                return "";
            }
        }

        public static decimal RetiraMascaraMonetaria(this string str)
        {

            decimal retorno = 0;

            Boolean hasMask = ((str.IndexOf("R$") > -1 || str.IndexOf("$") > -1) && (str.IndexOf(".") > -1 || str.IndexOf(",") > -1));

            // Verificamos se existe máscara

            if (hasMask)
            {
                str = str.Replace("R$", "").Replace("\\,\\w+", "").Replace("\\.\\w+", "");
            }

            retorno = Convert.ToDecimal(str);

            return retorno;

        }

        public static DataTable ListaDia()
        {
            DataTable listaDia = new DataTable();
            listaDia.Columns.Add("DIA");

            for (int i = 1; i <= 31; i++)
            {
                listaDia.Rows.Add(i);
            }


            return listaDia;
        }

        public static bool ValidaChaveNotaFiscal(this string chave)
        {
            if (chave.Length != 44)
                return false;

            var chars = chave.ToCharArray();
            if (!chars.All(q => char.IsNumber(q)))
                return false;

            var digits = chars.Select(s => int.Parse(s.ToString())).ToArray();

            int checkDigit;
            int.TryParse(digits.Last().ToString(), out checkDigit);

            var multipliers = new int[] { 2, 3, 4, 5, 6, 7, 8, 9 };

            var digitPosition = digits.Length - 2;
            var multiplierPosition = 0;
            var weightedSum = 0;
            while (true)
            {
                var digit = digits[digitPosition];
                var multiplier = multipliers[multiplierPosition];
                weightedSum += digit * multiplier;

                digitPosition--;
                if (digitPosition < 0)
                    break;

                multiplierPosition++;
                if (multiplierPosition >= multipliers.Length)
                    multiplierPosition = 0;
            }

            var rest = weightedSum % 11;
            var result = 11 - rest;

            if (rest == 0 || rest == 1)
                return 0 == checkDigit;
            else
                return result == checkDigit;
        }

        public static bool EhNumerico(this string chave)
        {
            return chave.ToCharArray().All(q => char.IsNumber(q));
        }

        public static bool SomenteLetras(string nome)
        {
            if (Regex.Match(nome, @"^[+]?.*\d+.*$").Success == false)
            {
                if (nome.IndexOf("'") != -1)
                {
                    return true;
                }
                else
                {
                    var match = Regex.Match(nome, @"^[+]?([\w]|[\s]|[-])*$");
                    return match.Success;
                }
            }
            return false;
        }

        public static string[] ExtractWords(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new string[0];
            }

            return value.Split(
                new[]
                {
                    " "
                },
                StringSplitOptions.RemoveEmptyEntries);
        }

        public static bool ContemNumeros(string texto)
        {
            if (texto.Where(c => char.IsNumber(c)).Count() > 0)
                return true;
            else
                return false;
        }

        //public static string Capitaliza(this string texto)
        //{
        //    var array = texto.Split(' ');
        //    array = array.Select(s =>
        //    {
        //        var tmp = s.Trim().ToLower();
        //        tmp = tmp.Substring(0, 1).ToUpper() + tmp.Substring(1, tmp.Length - 1).ToLower();
        //        return tmp;
        //    }).ToArray();
        //    var result = array.Aggregate((c, n) => c + " " + n);
        //    return result;
        //}


        public static string Capitaliza(this string texto)
        {
            // Preposições usadas em nomes de pessoas
            var preposicoes = new HashSet<string>
        {
            "da", "de", "di", "do", "dos", "das", "du", "d’"
        };

            // Divide ignorando entradas vazias (resolve espaços duplos, triplos, etc.)
            var array = texto
                .Trim()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select((s, i) =>
                {
                    var tmp = s.Trim().ToLower();

                    // Sempre capitaliza a primeira palavra
                    if (i == 0 || !preposicoes.Contains(tmp))
                    {
                        return char.ToUpper(tmp[0]) + tmp.Substring(1);
                    }
                    else
                    {
                        return tmp; // mantém minúsculo
                    }
                })
                .ToArray();

            return string.Join(" ", array);
        }
    }
}