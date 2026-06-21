using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Web;

/// <summary>
/// Summary description for arquivosRemessaResponse
/// </summary>
public class registroRemessaResponse 
{
    //public class Arquivos 
    //{ 

    //}

    public class registro
    {

        int _tipoMovimentacao;
        string _matriculaNova;
        string _alunoNome;
        DateTime _alunoDataNascimento;
        string _paiNome;
        string _maeNome;
        string _alunoCpf;
        string _alunoRg;
        string _alunoRgUfExpedicao;
        string _alunoRGOrgaoExpedicao;
        DateTime _alunoRGDataExpedicao;
        string _enderecoCep;
        string _enderecoTipoLogradouro;
        string _enderecoNomeLogradouro;
        string _enderecoNumero;
        string _enderecoComplemento;
        string _enderecoBairro;
        string _enderecoMunicipioestado;
        string _codigoCenso;
        string _QHITurno;
        string _QHITurma;
        byte[] _alunoFoto;
        string _gratuidade;
        string _modalTREM;
        string _modalONIBUS;
        string _modalMETRO;
        string _modalBARCAS;

        public int tipoMovimentacao
        {
            get { return _tipoMovimentacao; }
            set { _tipoMovimentacao = value; }
        }

        public string matriculaNova
        {
            get { return _matriculaNova; }
            set { _matriculaNova = value; }
        }

        public string alunoNome
        {
            get { return _alunoNome; }
            set { _alunoNome = value; }
        }

        public DateTime alunoDataNascimento
        {
            get { return _alunoDataNascimento; }
            set { _alunoDataNascimento = value; }
        }

        public string paiNome
        {
            get { return _paiNome; }
            set { _paiNome = value; }
        }

        public string maeNome
        {
            get { return _maeNome; }
            set { _maeNome = value; }
        }

        public string alunoCpf
        {
            get { return _alunoCpf; }
            set { _alunoCpf = value; }
        }

        public string alunoRg
        {
            get { return _alunoRg; }
            set { _alunoRg = value; }
        }

        public string alunoRgUfExpedicao
        {
            get { return _alunoRgUfExpedicao; }
            set { _alunoRgUfExpedicao = value; }
        }

        public string alunoRGOrgaoExpedicao
        {
            get { return _alunoRGOrgaoExpedicao; }
            set { _alunoRGOrgaoExpedicao = value; }
        }

        public DateTime alunoRGDataExpedicao
        {
            get { return _alunoRGDataExpedicao; }
            set { _alunoRGDataExpedicao = value; }
        }

        public string enderecoCep
        {
            get { return _enderecoCep; }
            set { _enderecoCep = value; }
        }

        public string enderecoTipoLogradouro
        {
            get { return _enderecoTipoLogradouro; }
            set { _enderecoTipoLogradouro = value; }
        }

        public string enderecoNomeLogradouro
        {
            get { return _enderecoNomeLogradouro; }
            set { _enderecoNomeLogradouro = value; }
        }

        public string enderecoNumero
        {
            get { return _enderecoNumero; }
            set { _enderecoNumero = value; }
        }

        public string enderecoComplemento
        {
            get { return _enderecoComplemento; }
            set { _enderecoComplemento = value; }
        }

        public string enderecoBairro
        {
            get { return _enderecoBairro; }
            set { _enderecoBairro = value; }
        }

        public string enderecoMunicipioestado
        {
            get { return _enderecoMunicipioestado; }
            set { _enderecoMunicipioestado = value; }
        }

        public string codigoCenso
        {
            get { return _codigoCenso; }
            set { _codigoCenso = value; }
        }

        public string QHITurno
        {
            get { return _QHITurno; }
            set { _QHITurno = value; }
        }

        public string QHITurma
        {
            get { return _QHITurma; }
            set { _QHITurma = value; }
        }

        public byte[] alunoFoto
        {
            get { return _alunoFoto; }
            set { _alunoFoto = value; }
        }

        public string gratuidade
        {
            get { return _gratuidade; }
            set { _gratuidade = value; }
        }

        public string modalTREM
        {
            get { return _modalTREM; }
            set { _modalTREM = value; }
        }
        public string modalONIBUS
        {
            get { return _modalONIBUS; }
            set { _modalONIBUS = value; }
        }

        public string modalMETRO
        {
            get { return _modalMETRO; }
            set { _modalMETRO = value; }
        }
        public string modalBARCAS
        {
            get { return _modalBARCAS; }
            set { _modalBARCAS = value; }
        }

    }


    string _nomeArquivo;
    int _quantidadeRegistros;

    List<registro> _registros;


    public string nomeArquivo
    {
        get { return _nomeArquivo; }
        set { _nomeArquivo = value; }
    }

    public int quantidadeRegistros
    {
        get { return _quantidadeRegistros; }
        set { _quantidadeRegistros = value; }
    }

    public List<registro> registros
    {
        get { return _registros; }
        set { _registros = value; }
    }

    public registroRemessaResponse()
    {

    }
}
