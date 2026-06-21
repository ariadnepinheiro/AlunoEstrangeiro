using System;
using System.Collections.Generic;

public class arquivosRemessaResponse
{
    public List<arquivo> arquivos { get; set; }

    public int quantidadeArquivos { get; set; }

    public class arquivo
    {
        public DateTime data { get; set; }

        public string nome { get; set; }

        public int quantidadeRegistros { get; set; }
    }
}