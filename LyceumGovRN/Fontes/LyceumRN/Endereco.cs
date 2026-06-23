using System;
using System.Data;
using System.Text;
using Seeduc.Infra.Data;
using Techne.Data;
using Techne.HadesLyc.CR;

namespace Techne.Lyceum.RN
{
    public class DadosEndereco
    {
        private string _pais;
        private string _descricaoPais;
        private string _cep;
        private string _municipio;
        private string _descricaoMunicipio;
        private string _uf;
        private string _bairro;
        private string _descricaoBairro;
        private string _logradouro;
        private string _descricaoLogradouro;
        private string _trechoLogradouro;

        private Techne.Library.ErrorList _error;

        public DadosEndereco()
        {

        }

        public string Pais
        {
            get { return _pais; }
            set { _pais = value; }
        }

        public string DescricaoPais
        {
            get { return _descricaoPais; }
            set { _descricaoPais = value; }
        }

        public string Cep
        {
            get { return _cep; }
            set { _cep = value; }
        }

        public string Municipio
        {
            get { return _municipio; }
            set { _municipio = value; }
        }

        public string DescricaoMunicipio
        {
            get { return _descricaoMunicipio; }
            set { _descricaoMunicipio = value; }
        }

        public string UF
        {
            get { return _uf; }
            set { _uf = value; }
        }

        public string Bairro
        {
            get { return _bairro; }
            set { _bairro = value; }
        }

        public string DescricaoBairro
        {
            get { return _descricaoBairro; }
            set { _descricaoBairro = value; }
        }

        public string Logradouro
        {
            get { return _logradouro; }
            set { _logradouro = value; }
        }

        public string DescricaoLogradouro
        {
            get { return _descricaoLogradouro; }
            set { _descricaoLogradouro = value; }
        }

        public string TrechoLogradouro
        {
            get { return _trechoLogradouro; }
            set { _trechoLogradouro = value; }
        }

        public Techne.Library.ErrorList Error
        {
            get { return _error; }
            set { _error = value; }
        }
    }

    public class Endereco : RNBase
    {
        /// <summary>
        /// Controla o endereço, caso não exista o registro será incluido na base de dados
        /// </summary>
        /// <param name="dadosEndereco">objeto com os dados do endereço, este objeto será atualizado caso os dados não existam</param>
        /// <returns>null no caso de não existir erro</returns>
        public static RetValue ControlarEndereco(DadosEndereco dadosEndereco)
        {
            TConnectionWritable connection = Techne.HadesLyc.Config.CreateWritableConnection();
            connection.Open(true);

            RetValue retorno = null;

            try
            {
                if (!string.IsNullOrEmpty(dadosEndereco.DescricaoPais)
                   && !string.IsNullOrEmpty(dadosEndereco.DescricaoBairro)
                   && !string.IsNullOrEmpty(dadosEndereco.Cep)
                   && !string.IsNullOrEmpty(dadosEndereco.DescricaoMunicipio)
                   && !string.IsNullOrEmpty(dadosEndereco.DescricaoLogradouro)
                   && !string.IsNullOrEmpty(dadosEndereco.UF))
                {

                    if (!string.IsNullOrEmpty(dadosEndereco.Bairro) && !string.IsNullOrEmpty(dadosEndereco.Logradouro) && !string.IsNullOrEmpty(dadosEndereco.Municipio))
                    {

                    }
                    else
                    {
                        //caso não exista o endereço será verificado em qual tabela o registro que não existe na base
                        if (VerificarEndereco(connection, dadosEndereco.Cep, dadosEndereco.DescricaoMunicipio, dadosEndereco.DescricaoBairro, dadosEndereco.DescricaoLogradouro, dadosEndereco.UF) == 0)
                        {
                            string municipio = ObterMunicipio(connection, dadosEndereco.DescricaoMunicipio, dadosEndereco.UF);

                            if (string.IsNullOrEmpty(municipio))
                            {
                                retorno = InserirMunicipio(connection, dadosEndereco.DescricaoMunicipio, dadosEndereco.UF, out municipio);

                                if (retorno != null && !retorno.Ok)
                                {
                                    connection.Rollback();
                                    return retorno;
                                }
                            }

                            dadosEndereco.Municipio = municipio;

                            string bairro = ObterBairro(connection, dadosEndereco.DescricaoBairro, municipio);

                            if (string.IsNullOrEmpty(bairro))
                            {
                                retorno = InserirBairro(connection, dadosEndereco.DescricaoBairro, municipio, out bairro);

                                if (retorno != null && !retorno.Ok)
                                {
                                    connection.Rollback();
                                    return retorno;
                                }
                            }

                            dadosEndereco.Bairro = bairro;

                            string logradouro = ObterLogradouro(connection, dadosEndereco.DescricaoLogradouro, municipio);

                            if (string.IsNullOrEmpty(logradouro))
                            {
                                retorno = InserirLogradouro(connection, dadosEndereco.DescricaoLogradouro, municipio, out logradouro);

                                if (retorno != null && !retorno.Ok)
                                {
                                    connection.Rollback();
                                    return retorno;
                                }
                            }

                            dadosEndereco.Logradouro = logradouro;

                            string trechoLogradouro = ObterTrechoLogradouro(connection, logradouro, municipio, dadosEndereco.Cep, bairro);

                            if (string.IsNullOrEmpty(trechoLogradouro))
                            {
                                retorno = InserirTrechoLogradouro(connection, logradouro, municipio, dadosEndereco.Cep, bairro, out trechoLogradouro);

                                if (retorno != null && !retorno.Ok)
                                {
                                    connection.Rollback();
                                    return retorno;
                                }
                            }

                            dadosEndereco.TrechoLogradouro = trechoLogradouro;
                        }
                    }
                }
            }
            finally
            {
                connection.Close();
            }

            return null;
        }

        public static RetValue ControlarEnderecoEstrangeiro(DadosEndereco dadosEndereco)
        {
            TConnectionWritable connection = Techne.HadesLyc.Config.CreateWritableConnection();
            connection.Open(true);

            RetValue retorno = null;

            try
            {
                if (!string.IsNullOrEmpty(dadosEndereco.DescricaoPais)
                    && !string.IsNullOrEmpty(dadosEndereco.DescricaoMunicipio)
                    )
                {

                    if (dadosEndereco.DescricaoPais.ToUpper() != "BRASIL")
                    {
                        string municipio = ObterMunicipioEstrangeiro(connection, dadosEndereco.DescricaoPais, dadosEndereco.DescricaoMunicipio, dadosEndereco.UF);

                        if (string.IsNullOrEmpty(municipio))
                        {
                            string pais = ObterPais(connection, dadosEndereco.DescricaoPais);

                            retorno = InserirMunicipioEstrangeiro(connection, pais, dadosEndereco.DescricaoMunicipio, dadosEndereco.UF, dadosEndereco.Cep, out municipio);

                            dadosEndereco.Pais = pais;

                            if (retorno != null && !retorno.Ok)
                            {
                                connection.Rollback();
                                return retorno;
                            }
                        }

                        dadosEndereco.Municipio = municipio;
                    }
                }
            }
            finally
            {
                connection.Close();
            }

            return null;
        }


        private static RetValue InserirMunicipioEstrangeiro(TConnectionWritable connection, string pais, string descricao, string estado, string cep, out string municipio)
        {
            municipio = GerarCodigoMunicipioEstrangeiro(connection);

            Hd_municipio_estrangeiro.Row.Insert(connection, municipio, pais, descricao, "nome_estado, cep", estado, cep);

            return VerificarErro(connection.GetErrors());
        }

        private static string ObterPais(TConnection connection, string pais)
        {
            string sql = " select TOP 1 pais from hd_pais where nome = ? ";

            DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, pais);

            if (!valorConsulta.IsNull)
                return (string)valorConsulta;

            return string.Empty;
        }

        public static string ObterCodigoPais(string descricaoPais)
        {
            TConnection connection = Techne.HadesLyc.Config.CreateConnection();

            string sql = " select TOP 1 pais from hd_pais where nome = ? ";

            connection.Open();

            try
            {
                DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, descricaoPais);

                if (!valorConsulta.IsNull)
                    return (string)valorConsulta;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return string.Empty;
        }



        public static string ObterDescricaoMunicipio(string municipio)
        {
            string sql = " select nome from hd_MUNICIPIO " +
                         " where " +
                         " municipio = ? ";

            TConnection connection = Techne.HadesLyc.Config.CreateConnection();
            connection.Open();

            try
            {
                DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, municipio);

                if (!valorConsulta.IsNull)
                    return (string)valorConsulta;
            }
            finally
            {
                connection.Close();
            }

            return string.Empty;
        }


        public static string ObterPais(string pais)
        {
            if (!string.IsNullOrEmpty(pais))
            {
                TConnection connection = Techne.HadesLyc.Config.CreateConnection();

                connection.Open();

                try
                {
                    string sql = " select TOP 1 nome from hd_pais where pais = ? ";

                    DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, pais);

                    if (!valorConsulta.IsNull)
                        return (string)valorConsulta;
                }
                finally
                {
                    connection.Close();
                }
            }

            return string.Empty;
        }

        public static string ObterPaisEstrangeiro(string PAIS)
        {
            if (!string.IsNullOrEmpty(PAIS))
            {
                TConnection connection = Techne.HadesLyc.Config.CreateConnection();

                connection.Open();

                try
                {
                    string sql = " SELECT TOP 1 NOME_PAIS FROM HD_MUNICIPIO_CERTIFICACAO WHERE NOME_PAIS = ? ";

                    DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, PAIS);

                    if (!valorConsulta.IsNull)
                        return (string)valorConsulta;
                }
                finally
                {
                    connection.Close();
                }
            }

            return string.Empty;
        }

        private static string ObterMunicipioEstrangeiro(TConnection connection, string descricaoPais, string descricao, string estado)
        {
            string sql = " select TOP 1 municipio_estrangeiro from hd_municipio_estrangeiro " +
                         " inner join hd_pais P ON P.pais = hd_municipio_estrangeiro.pais " +
                         " where P.Nome = ? " +
                         " and hd_municipio_estrangeiro.nome = ? " +
                         " and hd_municipio_estrangeiro.nome_estado = ? ";

            DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, descricaoPais, descricao, estado);

            if (!valorConsulta.IsNull)
                return (string)valorConsulta;

            return string.Empty;
        }


        public static string ObterUFMunicipio(string municipio)
        {
            string sql = " select UF from hd_MUNICIPIO " +
                         " where " +
                         " municipio = ? ";

            TConnection connection = Techne.HadesLyc.Config.CreateConnection();
            connection.Open();

            try
            {
                DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, municipio);

                if (!valorConsulta.IsNull)
                    return (string)valorConsulta;
            }
            finally
            {
                connection.Close();
            }

            return string.Empty;
        }


        /// <summary>
        /// Verifica se o código do municipio passado como parâmetro existe na base
        /// </summary>
        /// <param name="municipio">código do municipio</param>
        /// <returns>true se existir, false se não existir</returns>
        public static bool VerificarMunicipio(string municipio)
        {
            string sql = " select 1 from MUNICIPIO " +
                         " where " +
                         " codigo = ? ";

            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, municipio);

                if (!valorConsulta.IsNull)
                    return true;
            }
            finally
            {
                connection.Close();
            }

            return false;
        }


        public static DadosEndereco ConsultarDescricaoEndereco(string bairro, string municipio, string logradouro)
        {
            DadosEndereco dados = null;
            if (!string.IsNullOrEmpty(municipio))
            {
                string sql = " select B.nome nomeBairro, L.NOME nomeLogradouro " +
                             " FROM hd_bairro B " +
                             " INNER JOIN hd_municipio M  ON M.MUNICIPIO = B.MUNICIPIO " +
                             " INNER JOIN HD_LOGRADOURO L ON L.MUNICIPIO = M.MUNICIPIO " +
                             " INNER JOIN HD_TRECHO_LOGRADOURO T ON T.BAIRRO = B.BAIRRO " +
                             " AND T.MUNICIPIO = M.MUNICIPIO AND T.LOGRADOURO = L.LOGRADOURO " +
                             " WHERE " +
                             " T.BAIRRO = ? AND T.MUNICIPIO = ? AND T.LOGRADOURO = ? ";

                TConnection connection = Techne.HadesLyc.Config.CreateConnection();
                connection.Open();

                try
                {
                    QueryTable qt = new QueryTable(sql);

                    qt.Query(connection, bairro, municipio, logradouro);

                    if (qt.Rows.Count > 0)
                    {
                        dados = new DadosEndereco();
                        dados.DescricaoBairro = Convert.ToString(qt.Rows[0]["nomeBairro"]);
                        dados.DescricaoLogradouro = Convert.ToString(qt.Rows[0]["nomeLogradouro"]);
                    }
                }
                finally
                {
                    connection.Close();
                }
            }

            return dados;
        }


        public static bool VerificarMunicipioEstrangeiro(string municipio, string pais)
        {
            string sql = " select 1 from MUNICIPIO_ESTR " +
                         " where " +
                         " codigo = ? " +
                         " and pai_codigo = ? ";

            TConnection connection = Config.CreateConnection();
            connection.Open();

            try
            {
                DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, municipio, pais);

                if (!valorConsulta.IsNull)
                    return true;
            }
            finally
            {
                connection.Close();
            }

            return false;
        }

        public static SimpleRow ObterMunicipioEstrangeiro(string municipio)
        {
            if (!string.IsNullOrEmpty(municipio))
            {
                string sql = " select nome, nome_estado from hd_municipio_estrangeiro " +
                             " where  " +
                             " municipio_estrangeiro = " + municipio;

                TConnection connection = Techne.HadesLyc.Config.CreateConnection();
                connection.Open();

                try
                {
                    QueryTable qt = new QueryTable(sql);

                    qt.Query(connection);

                    if (qt.Rows.Count > 0)
                        return qt.Rows[0];
                }
                finally
                {
                    connection.Close();
                }
            }

            return null;
        }


        /// <summary>
        /// Verifica se existe dados para o endereço de acordo com os parametros
        /// </summary>
        /// <param name="cep"></param>
        /// <param name="descricaoMunicipio"></param>
        /// <param name="descricaoBairro"></param>
        /// <param name="descricaoLogradouro"></param>
        /// <param name="UF"></param>
        /// <returns>Zero caso não exista registro</returns>
        public static int VerificarEndereco(TConnection connection, string cep, string descricaoMunicipio, string descricaoBairro, string descricaoLogradouro, string UF)
        {
            string sql = " SELECT COUNT(TL.CEP) " +
                         " from municipio M (NOLOCK) " +
                         " inner join logradouro L (NOLOCK) ON L.municipio_codigo = M.codigo " +
                         " inner join trecho_logradouro TL (NOLOCK) ON TL.logradouro_codigo = L.codigo " +
                         " AND M.codigo = TL.logr_municipio_codigo " +
                         " inner join mBairro MB (NOLOCK) ON MB.codigo = TL.MBairro_codigo AND l.municipio_codigo = mb.municipio_codigo " +
                         " WHERE " +
                         " TL.CEP =  ? " +
                         " AND L.Nome = ? " +
                         " AND M.Nome = ? " +
                         " AND M.UF_SIGLA = ? " +
                         " AND MB.Nome = ? ";

            DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, cep, descricaoLogradouro, descricaoMunicipio, UF, descricaoBairro);

            if (!valorConsulta.IsNull)
                return (int)valorConsulta;

            return 0;
        }

        public static DadosEndereco ObterDadosEndereco(string cep, string descricaoMunicipio, string descricaoBairro, string descricaoLogradouro, string UF)
        {
            QueryTable qt = null;

            TConnection cn = Config.CreateConnection();

            cn.Open();

            try
            {

                string sql = " SELECT M.CODIGO codigoMunicipio, L.CODIGO codigoLogradouro, MB.CODIGO codigoBairro " +
                             " from municipio M " +
                             " inner join logradouro L ON L.municipio_codigo = M.codigo " +
                             " inner join trecho_logradouro TL ON TL.logradouro_codigo = L.codigo " +
                             " AND M.codigo = TL.logr_municipio_codigo " +
                             " inner join mBairro MB ON MB.codigo = TL.MBairro_codigo " +
                             " WHERE " +
                             " TL.CEP =  ? " +
                             " AND L.Nome = ? " +
                             " AND M.Nome = ? " +
                             " AND M.UF_SIGLA = ? " +
                             " AND MB.Nome = ? ";

                qt = new QueryTable(sql);

                qt.Query(cn, cep, descricaoLogradouro, descricaoMunicipio, UF, descricaoBairro);

                if (qt != null)
                {
                    if (qt.Rows.Count > 0)
                    {
                        DadosEndereco dados = new DadosEndereco();
                        dados.Municipio = Convert.ToString(qt.Rows[0]["codigoMunicipio"]);
                        dados.Logradouro = Convert.ToString(qt.Rows[0]["codigoLogradouro"]);
                        dados.Bairro = Convert.ToString(qt.Rows[0]["codigoBairro"]);

                        return dados;
                    }
                }

            }
            finally
            {
                cn.Close();
            }

            return null;
        }

        private static string ObterTrechoLogradouro(TConnection connection, string logradouro, string municipio, string cep, string bairro)
        {
            string sql = " select top 1 trecho_logr from hd_trecho_logradouro " +
                         " where " +
                         " logradouro = ? " +
                         " AND municipio = ? " +
                         " AND cep = ? " +
                         " AND bairro = ? ";

            DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, logradouro, municipio, cep, bairro);

            if (!valorConsulta.IsNull)
                return (string)valorConsulta;

            return string.Empty;
        }

        private static string ObterLogradouro(TConnection connection, string descricaoLogradouro, string municipio)
        {
            string sql = " select top 1 logradouro from hd_logradouro " +
                         " where " +
                         " municipio = ? " +
                         " AND nome = ? ";

            DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, municipio, descricaoLogradouro);

            if (!valorConsulta.IsNull)
                return (string)valorConsulta;

            return string.Empty;
        }

        private static string ObterBairro(TConnection connection, string descricaoBairro, string municipio)
        {
            string sql = " select top 1 bairro from hd_bairro " +
                         " where " +
                         " municipio = ? " +
                         " AND nome = ? ";

            DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, municipio, descricaoBairro);

            if (!valorConsulta.IsNull)
                return (string)valorConsulta;

            return string.Empty;
        }

        private static string ObterMunicipio(TConnection connection, string descricaoMunicipio, string UF)
        {
            string sql = " select top 1 municipio from hd_municipio " +
                         " where UF = ? " +
                         " AND nome = ? ";

            DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, UF, descricaoMunicipio);

            if (!valorConsulta.IsNull)
                return (string)valorConsulta;

            return string.Empty;
        }

        private static string GerarCodigoMunicipio(TConnection connection)
        {
            string sql = " SELECT MAX(CAST(municipio AS INT)) + 1 FROM hd_municipio ";

            DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql);

            if (!valorConsulta.IsNull)
                return valorConsulta.ToString("00000000");
            else
                return "00000001";
        }

        private static string GerarCodigoMunicipioEstrangeiro(TConnection connection)
        {
            string sql = " SELECT MAX(CAST(MUNICIPIO_ESTRANGEIRO AS INT)) + 1 FROM hd_municipio_estrangeiro ";

            DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql);

            if (!valorConsulta.IsNull)
                return Convert.ToString(valorConsulta);
            else
                return "1";
        }

        private static string GerarCodigoBairro(TConnection connection, string municipio)
        {
            string sql = " SELECT MAX(CAST(bairro AS INT)) + 1 FROM hd_bairro where municipio = ?";

            DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, municipio);

            if (!valorConsulta.IsNull)
                return valorConsulta.ToString("00000000");
            else
                return "00000001";

        }

        private static string GerarCodigoLogradouro(TConnection connection, string municipio)
        {
            string sql = " SELECT MAX(CAST(logradouro AS INT)) + 1 FROM hd_logradouro where municipio = ? ";

            DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, municipio);

            if (!valorConsulta.IsNull)
                return valorConsulta.ToString("00000000");
            else
                return "00000001";
        }

        private static string GerarCodigoTrechoLogradouro(TConnection connection, string logradouro, string municipio, string bairro, string cep)
        {
            string sql = " SELECT MAX(CAST(trecho_logr AS INT)) FROM hd_trecho_logradouro where municipio = ? AND logradouro = ? AND bairro = ? AND cep = ?";

            DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, municipio, logradouro, bairro, cep);

            if (!valorConsulta.IsNull)
                return valorConsulta.ToString("LL000000");
            else
                return "LL000001";
        }

        private static RetValue InserirMunicipio(TConnectionWritable connection, string descricao, string UF, out string municipio)
        {
            municipio = GerarCodigoMunicipio(connection);

            Hd_municipio.Row.Insert(connection, municipio, UF, descricao);

            return VerificarErro(connection.GetErrors());
        }

        private static RetValue InserirBairro(TConnectionWritable connection, string descricao, string municipio, out string bairro)
        {
            bairro = GerarCodigoBairro(connection, municipio);

            Hd_bairro.Row.Insert(connection, municipio, bairro, descricao);

            return VerificarErro(connection.GetErrors());
        }

        private static RetValue InserirLogradouro(TConnectionWritable connection, string descricao, string municipio, out string logradouro)
        {
            logradouro = GerarCodigoLogradouro(connection, municipio);

            Hd_logradouro.Row.Insert(connection, municipio, logradouro, "081", descricao, "S");

            return VerificarErro(connection.GetErrors());
        }


        private static RetValue InserirTrechoLogradouro(TConnectionWritable connection, string logradouro, string municipio, string cep, string bairro, out string trechoLogradouro)
        {
            trechoLogradouro = GerarCodigoTrechoLogradouro(connection, logradouro, municipio, bairro, cep);

            if (!string.IsNullOrEmpty(trechoLogradouro))
            {
                Hd_trecho_logradouro.Row.Insert(connection, logradouro, municipio, trechoLogradouro, "N", "cep, bairro", cep, bairro);

                return VerificarErro(connection.GetErrors());
            }

            return null;
        }

        public static QueryTable ConsultarPorCEP(string cep)
        {
            QueryTable qt = null;

            if (!string.IsNullOrEmpty(cep))
            {

                TConnection cn = Config.CreateConnection();

                cn.Open();

                try
                {
                    qt = new QueryTable(" SELECT TL.CEP cep, L.Codigo codigoLogradouro, L.Nome nomeLogradouro, M.codigo codigoMunicipio, " +
                                        " M.Nome nomeMunicipio, MB.codigo codigoBairro, MB.nome nomeBairro " +
                                        " from municipio M " +
                                        " inner join logradouro L ON L.municipio_codigo = M.codigo " +
                                        " inner join trecho_logradouro TL ON TL.logradouro_codigo = L.codigo " +
                                        " AND M.codigo = TL.logr_municipio_codigo " +
                                        " inner join mBairro MB ON MB.codigo = TL.MBairro_codigo " +
                                        " WHERE " +
                                        " TL.CEP = " + cep);

                    qt.Query(cn);
                }
                finally
                {
                    cn.Close();
                }
            }

            return qt;
        }

        public static QueryTable Consultar(string logradouro, string municipio, string cep)
        {
            QueryTable qt = null;

            if (!string.IsNullOrEmpty(logradouro) || !string.IsNullOrEmpty(municipio) || !string.IsNullOrEmpty(cep))
            {

                TConnection cn = Config.CreateConnection();

                cn.Open();

                try
                {
                    System.Text.StringBuilder sql = new StringBuilder();
                    sql.Append(" SELECT TL.CEP cep, L.Codigo codigoLogradouro, L.Nome nomeLogradouro, M.codigo codigoMunicipio, ");
                    sql.Append(" M.Nome nomeMunicipio, MB.codigo codigoBairro, MB.nome nomeBairro, M.uf_sigla ");
                    sql.Append(" from municipio M ");
                    sql.Append(" inner join logradouro L ON L.municipio_codigo = M.codigo ");
                    sql.Append(" inner join trecho_logradouro TL ON TL.logradouro_codigo = L.codigo ");
                    sql.Append(" AND M.codigo = TL.logr_municipio_codigo ");
                    sql.Append(" inner join mBairro MB ON MB.codigo = TL.MBairro_codigo ");
                    sql.Append(" AND MB.MUNICIPIO_CODIGO = TL.LOGR_MUNICIPIO_CODIGO  ");
                    sql.Append(" WHERE ");
                    sql.Append(" TL.CEP IS NOT NULL ");

                    if (!string.IsNullOrEmpty(logradouro))
                        sql.Append(String.Format(" AND L.NOME LIKE '%{0}%'", logradouro));
                    if (!string.IsNullOrEmpty(municipio))
                        sql.Append(String.Format(" AND M.NOME LIKE '%{0}%'", municipio));
                    if (!string.IsNullOrEmpty(cep))
                        sql.Append(String.Format(" AND TL.CEP = {0}", cep));

                    sql.Append(" ORDER BY TL.CEP, L.Codigo ");

                    qt = new QueryTable(sql.ToString());

                    qt.Query(cn);
                }
                finally
                {
                    cn.Close();
                }
            }

            return qt;
        }

        public static SimpleRow ObterCodigoMunicipioEstrangeiro(string municipio)
        {
            if (!string.IsNullOrEmpty(municipio))
            {
                string sql = @" select municipio_estrangeiro,nome, nome_estado from hd_municipio_estrangeiro
                             where  
                             nome = ?";

                TConnection connection = Techne.HadesLyc.Config.CreateConnection();
                connection.Open();

                try
                {
                    QueryTable qt = new QueryTable(sql);

                    qt.Query(connection, municipio);

                    if (qt.Rows.Count > 0)
                        return qt.Rows[0];
                }
                finally
                {
                    connection.Close();
                }
            }

            return null;
        }

        public DataTable ObterListaPaisesEstrangeirosCertificacao()
        {
            TConnection connection = Techne.HadesLyc.Config.CreateConnection();
            connection.Open();

            try
            {
                string sql = @"
                            SELECT DISTINCT
                                NOME_PAIS AS PAIS
                            FROM HADES.dbo.HD_MUNICIPIO_CERTIFICACAO
                            ORDER BY NOME_PAIS";

                QueryTable qt = new QueryTable(sql);
                qt.Query(connection);

                // Converte para DataTable, compatível com DataSource + DropDownList
                DataTable dt = new DataTable();
                dt.Columns.Add("PAIS", typeof(string));

                foreach (SimpleRow row in qt.Rows)
                {
                    dt.Rows.Add(
                        row["PAIS"].ToString()
                    );
                }

                return dt;
            }
            finally
            {
                connection.Close();
            }
        }

        public string ObterCodigoMunicipioEstrangeiroCertificacaoPor(string municipio)
        {
            DataContext contexto = DataContextBuilder.FromHades.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            try
            {
                contextQuery.Command = @" 
                    SELECT ID_MUNICIPIO
                    FROM HD_MUNICIPIO_CERTIFICACAO
                    WHERE MUNICIPIO = @MUNICIPIO
                ";

                contextQuery.Parameters.Add("@MUNICIPIO", SqlDbType.VarChar, municipio);

                resultado = contexto.GetReturnValue<string>(contextQuery);

                return resultado;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }
        }

        /// Use este método em telas que referenciam alunos/pessoas nascidas fora do Brasil.
        public static SimpleRow ObterMunicipioEstrangeiroHades(string municipio)
        {
            if (!string.IsNullOrEmpty(municipio))
            {
                string sql = @"SELECT CAST(CODIGO AS VARCHAR(20)) AS municipio_estrangeiro,
                              MUNICIPIO  AS nome,
                              ESTADO     AS nomeestado
                         FROM HADES.dbo.VW_MUNICIPIO_ESTRANGEIRO
                        WHERE CAST(CODIGO AS VARCHAR(20)) = ?";

                TConnection connection = Techne.HadesLyc.Config.CreateConnection();
                connection.Open();
                try
                {
                    QueryTable qt = new QueryTable(sql);
                    qt.Query(connection, municipio);
                    if (qt.Rows.Count > 0)
                        return qt.Rows[0];
                }
                finally
                {
                    connection.Close();
                }
            }
            return null;
        }

        /// Use este método em telas que referenciam alunos/pessoas nascidas fora do Brasil.
        public static SimpleRow ObterCodigoMunicipioEstrangeiroHades(string municipio)
        {
            if (!string.IsNullOrEmpty(municipio))
            {
                string sql = @"SELECT CAST(CODIGO AS VARCHAR(20)) AS municipio_estrangeiro,
                              MUNICIPIO  AS nome,
                              ESTADO     AS nomeestado
                         FROM HADES.dbo.VW_MUNICIPIO_ESTRANGEIRO
                        WHERE MUNICIPIO = ?";

                TConnection connection = Techne.HadesLyc.Config.CreateConnection();
                connection.Open();
                try
                {
                    QueryTable qt = new QueryTable(sql);
                    qt.Query(connection, municipio);
                    if (qt.Rows.Count > 0)
                        return qt.Rows[0];
                }
                finally
                {
                    connection.Close();
                }
            }
            return null;
        }

        public string ObtemCodigoMunicipioPor(string municipio)
        {
            DataContext contexto = DataContextBuilder.FromHades.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            try
            {
                contextQuery.Command = @" 
                    select municipio_estrangeiro 
                    from hd_municipio_estrangeiro
                    WHERE NOME = @MUNICIPIO
                ";

                contextQuery.Parameters.Add("@MUNICIPIO", SqlDbType.VarChar, municipio);

                resultado = contexto.GetReturnValue<string>(contextQuery);

                return resultado;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }

        }

        public string ObtemMunicipioPor(string codMunicipio)
        {
            DataContext contexto = DataContextBuilder.FromHades.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            try
            {
                contextQuery.Command = @" select nome from hd_municipio_estrangeiro
                WHERE municipio_estrangeiro = @CODMUNICIPIO
                ";

                contextQuery.Parameters.Add("@CODMUNICIPIO", SqlDbType.VarChar, codMunicipio);

                resultado = contexto.GetReturnValue<string>(contextQuery);

                return resultado;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                if (!Convert.ToString(ex.Message).Contains("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde."))
                {
                    mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                }
                else
                {
                    mensagem = Convert.ToString(ex.Message);
                }
                throw new Exception(mensagem);
            }
            finally
            {
                contexto.Dispose();
            }

        }
    }
}
