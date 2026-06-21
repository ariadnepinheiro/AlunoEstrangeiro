using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;

namespace Techne
{
    public class StrLib
    {
        /// <summary>
        ///   Indica o que é aceito no parse para float (single e decimal) de uma string numérica no método TypeStr().
        /// </summary>
        public const NumberStyles defaultFloatStyle = NumberStyles.AllowCurrencySymbol |
                                                      NumberStyles.AllowDecimalPoint |
                                                      NumberStyles.AllowLeadingSign |
                                                      NumberStyles.AllowLeadingWhite |
                                                      NumberStyles.AllowThousands |
                                                      NumberStyles.AllowTrailingWhite;

        /// <summary>
        ///   Indica o que é aceito no parse para int de uma string numérica no método TypeStr().
        /// </summary>
        public const NumberStyles defaultIntegerStyle = NumberStyles.AllowLeadingSign |
                                                        NumberStyles.AllowLeadingWhite |
                                                        NumberStyles.AllowThousands |
                                                        NumberStyles.AllowTrailingWhite;

        internal const string defaultFormat = "G";

        internal const string defaultFormatDateTime = "G";

        internal const string defaultFormatDecimal = "G";

        internal const string defaultFormatInt = "0";

        public static string EnumerableToStr(IEnumerable array)
        {
            return EnumerableToStr(array, ", ");
        }

        public static string EnumerableToStr(IEnumerable array, string separator)
        {
            return EnumerableToStr(array, separator, string.Empty, string.Empty);
        }

        public static string EnumerableToStr(IEnumerable array, string separator, 
                                             string itemDelimiter)
        {
            return EnumerableToStr(array, separator, itemDelimiter, itemDelimiter);
        }

        /// <summary>
        ///   Transforma um IEnumerable em string.
        /// </summary>
        /// <param name = "array">Lista a ser convertida para string</param>
        /// <param name = "separator">Separador dos itens na string resultante</param>
        /// <param name = "leftItemDelimiter">Delimitador esquerdo de cada um dos itens</param>
        /// <param name = "rightItemDelimiter">Delimitador direito de cada um dos itens</param>
        /// <param name = "functionItemToString">Função que converte o item para string.
        ///   Se o valor null for passado, o método ToString() de cada item é utilizado.</param>
        public static string EnumerableToStr(IEnumerable array, string separator, 
                                             int lineSize, 
                                             string leftItemDelimiter, string rightItemDelimiter, 
                                             ObjectToStringDelegate functionItemToString)
        {
            var lines = new ArrayList();

            var r = new StringBuilder();
            var first = true;
            foreach (var s in array)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    r.Append(separator);
                }

                string item;
                {
                    string tmp;
                    if (functionItemToString != null)
                    {
                        tmp = functionItemToString(s);
                    }
                    else if (s == null)
                    {
                        tmp = string.Empty;
                    }
                    else if (s is DBNull)
                    {
                        tmp = string.Empty;
                    }
                    else
                    {
                        tmp = s.ToString();
                    }

                    item = leftItemDelimiter + tmp + rightItemDelimiter;
                }

                if (lineSize > 0 && r.Length + item.Length > lineSize)
                {
                    lines.Add(r.ToString());
                    r = new StringBuilder(item);
                }
                else
                {
                    r.Append(item);
                }
            }

            var result = new StringBuilder();
            foreach (string line in lines)
            {
                if (result.Length > 0)
                {
                    result.Append("\r\n");
                }

                result.Append(line);
            }

            if (result.Length > 0)
            {
                result.Append("\r\n");
            }

            result.Append(r);

            return result.ToString();
        }

        public static string[] EnumerablesToStrArray(string separator, params IEnumerable[] array)
        {
            var result = new ArrayList();
            var enumerators = new IEnumerator[array.Length];

            for (var i = 0; i < array.Length; i++)
            {
                enumerators[i] = array[i].GetEnumerator();
            }

            while (true)
            {
                var move = false; // Indica se pelo menos um MoveNext() ocorreu com sucesso
                var strItem = new StringBuilder();
                for (var i = 0; i < array.Length; i++)
                {
                    if (i > 0)
                    {
                        strItem.Append(separator);
                    }

                    if (enumerators[i].MoveNext())
                    {
                        strItem.Append(enumerators[i].Current.ToString());
                        move = true;
                    }
                }

                if (!move)
                {
                    break;
                }

                result.Add(strItem.ToString());
            }

            return (string[])result.ToArray(typeof (string));
        }

        /// <summary>
        ///   Devolve true se todos os caracteres de str estiverem em lowercase.
        /// </summary>
        public static bool IsLower(string str)
        {
            foreach (var ch in str)
            {
                if (!char.IsLower(ch))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///   Se a string for vazia ou só contiver espaços, devolve array vazio (string.Split() devolve array com um elemento).
        ///   Caso contrário, faz string.Split() e faz string.Trim() em cada item.
        /// </summary>
        public static string[] Split(string str, char separator)
        {
            if (str == null)
            {
                throw new ArgumentNullException();
            }

            if (str.Trim().Length == 0)
            {
                return new string[0];
            }
            else
            {
                var tokens = str.Split(separator);
                for (var i = 0; i < tokens.Length; i++)
                {
                    tokens[i] = tokens[i].Trim();
                }

                return tokens;
            }
        }

        /// <summary>
        ///   Converte uma lista em IDictionary.
        ///   Cada elemento da lista é composto por um par (nome, valor).
        /// </summary>
        /// <param name = "stream">Lista a ser convertida cujo format é determinado pelos separadores</param>
        /// <param name = "elementSeparator">Separador de elementos. Cada elemento é composto por um par (nome, valor)</param>
        /// <param name = "equalitySign">Para cada elemento, indica o token que separa o nome do valor</param>
        public static IDictionary StrToDictionary(string stream, char elementSeparator, string equalitySign)
        {
            return StrToDictionary(stream, elementSeparator, equalitySign, false);
        }

        /// <summary>
        ///   Converte uma lista em IDictionary.
        ///   Cada elemento da lista é composto por um par (nome, valor).
        /// </summary>
        /// <param name = "stream">Lista a ser convertida cujo format é determinado pelos separadores</param>
        /// <param name = "elementSeparator">Separador de elementos. Cada elemento é composto por um par (nome, valor)</param>
        /// <param name = "equalitySign">Para cada elemento, indica o token que separa o nome do valor</param>
        /// <param name = "lowerKeys">Converte todos os nomes dos elementos para lowercase</param>
        public static IDictionary StrToDictionary(string stream, char elementSeparator, string equalitySign, bool lowerKeys)
        {
            return StrToDictionary(stream, elementSeparator, equalitySign, lowerKeys, false);
        }

        /// <summary>
        ///   Converte uma lista em IDictionary.
        ///   Cada elemento da lista é composto por um par (nome, valor).
        /// </summary>
        /// <param name = "stream">Lista a ser convertida cujo format é determinado pelos separadores</param>
        /// <param name = "elementSeparator">Separador de elementos. Cada elemento é composto por um par (nome, valor)</param>
        /// <param name = "equalitySign">Para cada elemento, indica o token que separa o nome do valor</param>
        /// <param name = "lowerKeys">Converte todos os nomes dos elementos para lowercase</param>
        /// <param name = "ignoreDuplicateKeys">
        ///   Se true, considera somente a primeira ocorrência de cada chave.
        ///   Caso contrário, dá exception caso exista mais de uma ocorrência de uma chave.
        /// </param>
        public static IDictionary StrToDictionary(string stream, char elementSeparator, string equalitySign, bool lowerKeys, 
                                                  bool ignoreDuplicateKeys)
        {
            var result = new ListDictionary();
            var lenEquality = equalitySign.Length;
            string name, value;

            foreach (var pair in stream.Split(elementSeparator))
            {
                var pos = pair.IndexOf(equalitySign);
                if (pos < 0)
                {
                    name = lowerKeys ? pair.ToLower() : pair;
                    value = string.Empty;
                }
                else
                {
                    name = pair.Substring(0, pos);
                    if (lowerKeys)
                    {
                        name = name.ToLower();
                    }

                    value = pair.Substring(pos + lenEquality, pair.Length - pos - lenEquality);
                }

                if (name.Length > 0 && value.Length > 0)
                {
                    try
                    {
                        result.Add(name, value);
                    }
                    catch (ArgumentException)
                    {
                        // An entry with the same key already exists.
                        if (!ignoreDuplicateKeys)
                        {
                            throw new ArgumentException("O elemento de chave '" + name + "' já existe na lista.");
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///   Converte variáveis para string de forma que possam ser recuperadas sem perda através da função TypeStr(string, Type).
        ///   A string retornada NÃO deve ser utilizada para apresentação final, visto que o formato utilizado
        ///   é fixo, independente da cultura.
        /// </summary>
        public static string ToStr(DbObject value)
        {
            return ToStr(value, string.Empty, string.Empty);
        }

        public static DbObject TypeStr(string strValue, DbType type, string culture)
        {
            if (culture == null)
            {
                throw new ArgumentNullException("culture");
            }

            if (strValue == null || strValue == string.Empty)
            {
                return DBNull.Value;
            }

            var mtCultInfo = culture == string.Empty ? CultureInfo.InvariantCulture
                                 : new CultureInfo(culture);

            if (type == DbType.Date)
            {
                try
                {
                    return DateTime.Parse(strValue, mtCultInfo, DateTimeStyles.NoCurrentDateDefault);
                }
                catch (FormatException)
                {
                    throw new FormatException("Formato inválido para data");
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new ArgumentOutOfRangeException("Data inválida");
                }
            }
            else if (type == DbType.Number)
            {
                try
                {
                    return Decimal.Parse(strValue, defaultFloatStyle, mtCultInfo);
                }
                catch (FormatException)
                {
                    throw new FormatException("Formato inválido para número");
                }
                catch (OverflowException)
                {
                    throw new OverflowException("Número inválido");
                }
            }
            else if (type == DbType.VarChar)
            {
                return strValue;
            }
            else
            {
                throw new NotImplementedException("Alterar o método StrLib.TypeStr() para implementar o item DbType." + type + ".");
            }
        }

        internal static string Base64StringToString(string b64str)
        {
            var reader = new BinaryReader(new MemoryStream(Convert.FromBase64String(b64str)));
            try
            {
                return reader.ReadString();
            }
            finally
            {
                reader.Close();
            }
        }

        internal static string DictionaryToStr(IDictionary dictionary, char elementSeparator, string equalitySign)
        {
            var list = new string[dictionary.Count];
            var n = 0;
            foreach (DictionaryEntry item in dictionary)
            {
                list[n++] = item.Key + equalitySign + item.Value;
            }

            return EnumerableToStr(list, elementSeparator.ToString());
        }

        internal static int EndsWith(string[] endSubstrings, string str, bool caseInsensitive)
        {
            var lower = str.ToLower();

            for (var i = 0; i < endSubstrings.Length; i++)
            {
                if (caseInsensitive)
                {
                    if (lower.EndsWith(endSubstrings[i].ToLower()))
                    {
                        return i;
                    }
                }
                else
                {
                    if (str.EndsWith(endSubstrings[i]))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        internal static string EnumerableToStr(IEnumerable array, string separator, 
                                               string leftItemDelimiter, string rightItemDelimiter)
        {
            return EnumerableToStr(array, separator, 0, leftItemDelimiter, rightItemDelimiter, null);
        }

        internal static object[] FromBase64String(string b64str)
        {
            var array = b64str.Split(',');
            var result = new object[array.Length / 2];

            for (var i = 0; i < result.Length; i++)
            {
                result[i] = FromBase64String(array[i * 2], array[i * 2 + 1]);
            }

            return result;
        }

        /// <summary>
        ///   Dado um caractere especial de formatação de datas, devolve o formato "por extenso".
        ///   Exemplo (usando en-US): para 'd' devolve 'MM/dd/yyyy', para 'F' devolve 'dddd, dd MMMM yyyy HH:mm:ss'.
        /// </summary>
        internal static string GetDateFormat(char formatCharacter, DateTimeFormatInfo formatInfo)
        {
            switch (formatCharacter)
            {
                case 'd':
                    return formatInfo.ShortDatePattern;
                case 'D':
                    return formatInfo.LongDatePattern;
                case 'f':
                    return formatInfo.LongDatePattern + ' ' + formatInfo.ShortTimePattern;
                case 'F':
                    return formatInfo.FullDateTimePattern;
                case 'g':
                    return formatInfo.ShortDatePattern + ' ' + formatInfo.ShortTimePattern;
                case 'G':
                    return formatInfo.ShortDatePattern + ' ' + formatInfo.LongTimePattern;
                case 'm':
                case 'M':
                    return formatInfo.MonthDayPattern;
                case 'r':
                case 'R':
                    return formatInfo.RFC1123Pattern;
                case 's':
                    return formatInfo.SortableDateTimePattern;
                case 't':
                    return formatInfo.ShortTimePattern;
                case 'T':
                    return formatInfo.LongTimePattern;
                case 'u':
                    return formatInfo.UniversalSortableDateTimePattern;
                case 'y':
                case 'Y':
                    return formatInfo.YearMonthPattern;
                default:
                    throw new NotSupportedException();
            }
        }

        internal static string[] GetTokens(string stream, char tokenDelimiter)
        {
            var token = new Token(stream, tokenDelimiter);
            var result = token.GetTokens();
            token = null;

            return result;
        }

        internal static int IndexOfInsensitive(string item, IList list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i] is string && string.Compare((string)list[i], item, true) == 0)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        ///   Devolve a string sem os acentos nos caracteres que os contém.
        /// </summary>
        internal static string RemoveAccent(string s)
        {
            var result = new char[s.Length];
            var i = 0;
            foreach (var c in s)
            {
                result[i++] = RemoveAccent(c);
            }

            return new string(result);
        }

        /// <summary>
        ///   Devolve o caractere sem o acento.
        ///   Se não contiver um, devolve o próprio.
        /// </summary>
        internal static char RemoveAccent(char c)
        {
            const string a = "áàâäã éèêë íìîï óòôöõ úùûü ñ ÁÀÂÄÃ ÉÈÊË ÍÌÎÏ ÓÒÔÖÕ ÚÙÛÜ Ñ";
            const string b = "aaaaa eeee iiii ooooo uuuu n AAAAA EEEE IIII OOOOO UUUU N";

            var i = a.IndexOf(c);

            return i < 0 ? c : b[i];
        }

        /// <summary>
        ///   Se a string contém o prefixo, devolve a string sem esse prefixo.
        ///   Caso contrário devolve a própria string.
        /// </summary>
        internal static string RemovePrefix(string str, string prefix)
        {
            return str != null && prefix != null && str.StartsWith(prefix) ? str.Substring(prefix.Length) : str;
        }

        internal static string Replace(string Stream, string From, string To)
        {
            var s = Stream; // ou Stream.Clone???
            var p = Stream.Length;

            while (--p >= 0)
            {
                p = Stream.ToUpper().LastIndexOf(From.ToUpper(), p);
                if (p >= 0)
                {
                    s = s.Substring(0, p) + To + s.Substring(p + From.Length);
                }
            }

            return s;
        }

        /// <summary>
        ///   Substitui os tokens encontrado numa string.
        /// </summary>
        /// <param name = "stream">String que contém os tokens</param>
        /// <param name = "func">Função de substituição</param>
        /// <param name = "tokenDelimiter">Caractere delimitador de tokens</param>
        internal static string ReplaceToken(string stream, StringToStringDelegate func, char tokenDelimiter)
        {
            var token = new Token(stream, tokenDelimiter);
            var result = token.ReplaceToken(func);
            token = null;

            return result;
        }

        internal static string Space(int n)
        {
            return new string(' ', n);
        }

        internal static int StartsWith(string[] startSubstrings, string str, bool caseInsensitive)
        {
            var lower = str.ToLower();

            for (var i = 0; i < startSubstrings.Length; i++)
            {
                if (caseInsensitive)
                {
                    if (lower.StartsWith(startSubstrings[i].ToLower()))
                    {
                        return i;
                    }
                }
                else
                {
                    if (str.StartsWith(startSubstrings[i]))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        internal static bool StartsWithInsensitive(string prefix, string str)
        {
            return str.ToLower().StartsWith(prefix.ToLower());
        }

        internal static string StringToBase64String(string str)
        {
            var stream = new MemoryStream();
            var bwriter = new BinaryWriter(stream);

            bwriter.Write(str);

            try
            {
                return Convert.ToBase64String(stream.ToArray());
            }
            finally
            {
                bwriter.Close();
                stream.Close();
            }
        }

        internal static string ToBase64String(object[] array)
        {
            var list = new string[array.Length];

            for (var i = 0; i < array.Length; i++)
            {
                list[i] = ToBase64String(array[i]);
            }

            return EnumerableToStr(list, ",");
        }

        internal static string ToProper(string s)
        {
            if (s == null || s.Length == 0)
            {
                return s;
            }

            return s.Substring(0, 1).ToUpper() + s.Substring(1).ToLower();
        }

        /// <summary>
        ///   Transforma uma variável em string. Utilizado para display.
        /// </summary>
        /// <param name = "value">A variável a ser convertida para String.</param>
        /// <param name = "format">Alguns valores válidos: numéricos: c, e, f, g, n; data: d (default), g, G, t, T.
        ///   É permitido também montar seus formatos, porém isso NÃO É RECOMENDADO.
        ///   Para isso, verifique o tópico Formatting Strings do Help.</param>
        internal static string ToStr(DbObject value, string format, string culture)
        {
            if (format == null)
            {
                throw new ArgumentNullException("format");
            }

            if (culture == null)
            {
                throw new ArgumentNullException("culture");
            }

            if (value == null || value.IsNull)
            {
                return string.Empty;
            }

            CultureInfo mtCultInfo;

            if (culture.Trim() == string.Empty)
            {
                mtCultInfo = CultureInfo.InvariantCulture;
            }
            else
            {
                mtCultInfo = new CultureInfo(culture);
            }

            var type = value.Type;

            if (type == DbType.VarChar)
            {
                return (string)value;
            }
            else if (type == DbType.Date)
            {
                return ((DateTime)value).ToString(format == string.Empty ? defaultFormatDateTime : format, mtCultInfo);
            }
            else if (type == DbType.Number)
            {
                return ((decimal)value).ToString(format == string.Empty ? defaultFormatDecimal : format, mtCultInfo);
            }
            else
            {
                throw new NotImplementedException("Alterar o método StrLib.ToStr() para implementar o tipo DbType." + value.Type + ".");
            }
        }

        /// <summary>
        ///   Converte uma string para um determinado tipo.
        ///   A conversão é realizada utilizando-se a InvariantCulture. Esta função deve ser utilizada em strings
        ///   retornadas pela função ToStr(DbObject).
        ///   IMPORTANTE: caso strValue seja null ou string vazia, a função devolverá DBNull.
        /// </summary>
        internal static DbObject TypeStr(string strValue, DbType type)
        {
            return TypeStr(strValue, type, string.Empty);
        }

        private static object FromBase64String(string b64type, string b64str)
        {
            var type64 = Base64StringToString(b64type);
            var type = type64 == "null" ? null : Type.GetType(type64);
            var value = Base64StringToString(b64str);

            // Trata os tipos Techne diferenciadamente porque o ChangeType() não chama os conversores implícitos,
            // ou seja, uma string não é convertida para VarChar automaticamente e assim por diante. O mesmo vale
            // para o DBNull, que também não é convertido automaticamente para VarChar, por exemplo.

            if (type == typeof (DBNull))
            {
                // No caso de type == DBNull, value guarda o fullname do tipo
                if (value == typeof (VarChar).FullName)
                {
                    return (VarChar)DBNull.Value;
                }
                else if (value == typeof (Number).FullName)
                {
                    return (Number)DBNull.Value;
                }
                else if (value == typeof (Date).FullName)
                {
                    return (Date)DBNull.Value;
                }
                else
                {
                    return DBNull.Value;
                }
            }
            else if (type == null)
            {
                return null;
            }
            else if (type == typeof (VarChar))
            {
                return (VarChar)value;
            }
            else if (type == typeof (Number))
            {
                return (Number)decimal.Parse(value, CultureInfo.InvariantCulture);
            }
            else if (type == typeof (Date))
            {
                return (Date)DateTime.Parse(value, CultureInfo.InvariantCulture);
            }
            else
            {
                return Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
            }
        }

        private static string ToBase64String(object item)
        {
            // Converte o valor para string em cultura invariante, armazena-o juntamente com o fullname do tipo original
            // em Base64String, exceto o null contido em tipos Techne, onde type guardará o fullname de DBNull e value
            // guardará o fullname do tipo Techne original.
            string type, value;
            if (item == null)
            {
                type = "null";
                value = string.Empty;
            }
            else if (item is IDbObject && ((IDbObject)item).IsNull)
            {
                type = typeof (DBNull).FullName;
                value = item.GetType().FullName;
            }
            else
            {
                type = item.GetType().FullName;
                value = Convert.ToString(item, CultureInfo.InvariantCulture);
            }

            return StringToBase64String(type) + "," + StringToBase64String(value);
        }

        private class Token
        {
            private readonly char pvDelimiter;

            private readonly string pvStream;

            private ArrayList gtResult;

            private StringToStringDelegate rtReplace;

            private string rtResult;

            public Token(string stream, char tokenDelimiter)
            {
                this.pvStream = stream;
                this.pvDelimiter = tokenDelimiter;
            }

            public string[] GetTokens()
            {
                this.gtResult = new ArrayList();
                this.FindTokens(this.gtTokenFound, null);
                var result = (string[])this.gtResult.ToArray(typeof (string));
                this.gtResult = null;
                return result;
            }

            /// <summary>
            ///   Substitui os tokens encontrado numa string.
            /// </summary>
            /// <param name = "func">Função de substituição</param>
            public string ReplaceToken(StringToStringDelegate func)
            {
                this.rtReplace = func;
                this.rtResult = string.Empty;
                this.FindTokens(this.rtTokenFound, this.rtInterFound);
                return this.rtResult;
            }

            private void FindTokens(FoundDelegate gotToken, FoundDelegate gotInter)
            {
                var result = string.Empty;
                var len = this.pvStream.Length;
                var pos = 0;

                while (pos < len)
                {
                    var ini = this.pvStream.IndexOf(this.pvDelimiter, pos);
                    if (ini < 0)
                    {
                        if (gotInter != null)
                        {
                            gotInter(this.pvStream.Substring(pos));
                        }

                        pos = len;
                    }
                    else
                    {
                        if (gotInter != null)
                        {
                            gotInter(this.pvStream.Substring(pos, ini - pos));
                        }

                        var fim = this.pvStream.IndexOf(this.pvDelimiter, ini + 1);
                        if (fim < 0)
                        {
                            throw new FormatException("Não foi encontrado o delimitador direito correspondente ao delimitador esquerdo da posição " + ini);
                        }

                        if (gotToken != null)
                        {
                            gotToken(this.pvStream.Substring(ini + 1, fim - ini - 1));
                        }

                        pos = fim + 1;
                    }
                }
            }

            private void gtTokenFound(string token)
            {
                this.gtResult.Add(token);
            }

            private void rtInterFound(string inter)
            {
                this.rtResult += inter;
            }

            private void rtTokenFound(string token)
            {
                this.rtResult += this.rtReplace(token);
            }

            private delegate void FoundDelegate(string part);
        }
    }
}