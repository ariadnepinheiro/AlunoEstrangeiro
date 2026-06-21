using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Web;

/// <summary>
/// Summary description for arquivosRemessaResponse
/// </summary>
public class arquivosRemessaResponse
{
    //public class Arquivos 
    //{ 

    //}

    public class arquivo
    {

        int _quantidadedeRegistros;
        string _nome;
        DateTime _data;

        public int quantidadeRegistros
        {
            get { return _quantidadedeRegistros; }
            set { _quantidadedeRegistros = value; }
        }

        public string nome
        {
            get { return _nome; }
            set { _nome = value; }
        }

        public DateTime data
        {
            get { return _data; }
            set { _data = value; }
        }
    }


    int _quantidadeDeArquivos;
    List<arquivo> _arquivos;

    public int quantidadeArquivos
    {
        get { return _quantidadeDeArquivos; }
        set { _quantidadeDeArquivos = value; }
    }

    public List<arquivo> arquivos
    {
        get { return _arquivos; }
        set { _arquivos = value; }
    }

    public arquivosRemessaResponse()
    {

    }
}
