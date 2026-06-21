using System.Collections;
using System.Text;

namespace Techne.Library
{
    internal class ItemList
    {
        public static StrBlock ToStrBlock(ICollection items, 
                                          int[] options, string[] aux, 
                                          BuildListItemDelegate buildListMethod, 
                                          string separator)
        {
            return ToStrBlock(items, 
                              options, aux, 
                              buildListMethod, 
                              string.Empty, string.Empty, string.Empty, string.Empty, 
                              separator);
        }

        public static StrBlock ToStrBlock(ICollection items, 
                                          int[] options, string[] aux, 
                                          BuildListItemDelegate buildListMethod, 
                                          string startFirstLine, string endLastLine, 
                                          string separator)
        {
            return ToStrBlock(items, options, aux, buildListMethod, 
                              startFirstLine, string.Empty, 
                              endLastLine, string.Empty, 
                              separator);
        }

        public static StrBlock ToStrBlock(ICollection items, 
                                          int[] options, string[] aux, 
                                          BuildListItemDelegate buildListMethod, 
                                          string startFirstLine, string startLine, 
                                          string endLastLine, string endLine, 
                                          string separator)
        {
            var b = new StrBlock();
            var offset = startFirstLine.Length;
            var line = new StringBuilder();
            var lineCount = 0;

            var n = 0;
            foreach (var item in items)
            {
                var isFirstItem = n == 0;
                var isLastItem = n == items.Count - 1;

                // startFirstLine
                // (tem que fazer dentro do loop, pq só faz se existirem itens)
                if (isFirstItem)
                {
                    line.Append(startFirstLine);
                }

                // O item
                string strColuna;
                if (buildListMethod == null)
                {
                    strColuna = (aux.Length > 0 ? aux[0] : string.Empty) + item +
                                (aux.Length > 1 ? aux[1] : string.Empty);
                }
                else
                {
                    strColuna = buildListMethod(n, item, options, aux);
                }

                line.Append(strColuna);

                // Separador (se não for o último item)
                if (!isLastItem)
                {
                    line.Append(separator);
                }

                // endLine, se necessário ou endLastLine, se último item
                if (isLastItem)
                {
                    line.Append(endLastLine);
                }
                else if (line.Length > 80)
                {
                    line.Append(endLine);
                    var strline = line.ToString();
                    var p = strline.LastIndexOf("\r\n");
                    b.Add(p < 0 ? strline : strline.Substring(0, p), 0, lineCount == 0 ? 0 : offset);

                    line = new StringBuilder();
                    if (p >= 0)
                    {
                        line.Append(strline.Substring(p + 2));
                    }

                    line.Append(startLine);
                    lineCount++;
                }

                n++;
            }

            if (line.Length > 0)
            {
                b.Add(line.ToString(), 0, lineCount == 0 ? 0 : offset);
            }

            return b;
        }

        internal delegate string BuildListItemDelegate(int order, object Item, int[] options, string[] aux);
    }
}