using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using Techne.Lyceum.RN.CartaoEstudante.Entities;
using Techne.Lyceum.RN.CartaoEstudante.Util;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN.CartaoEstudante.Repository
{
    public class RepositoryRemessa : Conexao
    {
        /// <summary>
        /// Método que a lista as remessas de um determinado lote
        /// </summary>
        /// <returns></returns>
        public List<Remessa> Listaremessas(Int32 idLoteRemessa, string nomeArquivo)
        {
            List<Remessa> lstRemessa = new List<Remessa>();

            ///conexão para persiti o log
             var conn2 = new SqlConnection(System.Text.RegularExpressions.Regex.Replace(Techne.Data.ConnectionList.GetConnectionString("Lyceum"), "Provider=SQLOLEDB.1;", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase));
            
            try
            {
                conn2.Open();

                var command = new SqlCommand(ListaRemessaQuery(idLoteRemessa), openConn());
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    //if (registro.registros == null)
                    //{
                    //    registro.registros = new List<registroRemessaResponse.registro>();
                    //}

                    var registroAux = new Remessa();



                    if (reader["REMESSAID"] != null && !(reader["REMESSAID"] is DBNull))
                    {
                        ///Metodo que persite o log
                        Logremessa(Convert.ToString(reader["REMESSAID"]), conn2);
                    }

                    registroAux.tipoMovimentacao = 0;
                    if (reader["SOLICITACAOID"] != null && !(reader["SOLICITACAOID"] is DBNull))
                    {
                        registroAux.tipoMovimentacao = Convert.ToInt32(reader["SOLICITACAOID"]);
                    }

                    if (reader["REMESSAID"] != null && !(reader["REMESSAID"] is DBNull))
                    {
                        registroAux.IdSolicitacao = Convert.ToInt32(reader["REMESSAID"]);
                    }

                    registroAux.matricula = string.Empty;
                    if (reader["ALUNO"] != null && !(reader["ALUNO"] is DBNull))
                    {
                        registroAux.matricula = Convert.ToString(reader["ALUNO"]);
                    }

                    registroAux.alunoNome = string.Empty;
                    if (reader["NOME_COMPL"] != null && !(reader["NOME_COMPL"] is DBNull))
                    {
                        registroAux.alunoNome = Convert.ToString(reader["NOME_COMPL"]);
                    }

                    registroAux.alunoDataNascimento = DateTime.MinValue;
                    if (reader["DT_NASC"] != null && !(reader["DT_NASC"] is DBNull))
                    {
                        var alunoDataNascimento = DateTime.MinValue;

                        if (DateTime.TryParse(Convert.ToString(reader["DT_NASC"]), out alunoDataNascimento))
                        {
                            registroAux.alunoDataNascimento = alunoDataNascimento;
                        }
                    }

                    if (reader["STAMP_ATUALIZACAO"] != null && !(reader["STAMP_ATUALIZACAO"] is DBNull))
                    {
                        var dataFoto = DateTime.MinValue;

                        if (DateTime.TryParse(Convert.ToString(reader["STAMP_ATUALIZACAO"]), out dataFoto))
                        {
                            registroAux.dataHoraFoto = dataFoto;
                        }
                    }

                    registroAux.paiNome = string.Empty;
                    if (reader["NOME_PAI"] != null && !(reader["NOME_PAI"] is DBNull))
                    {
                        registroAux.paiNome = Convert.ToString(reader["NOME_PAI"]);
                    }

                    registroAux.maeNome = string.Empty;
                    if (reader["NOME_MAE"] != null && !(reader["NOME_MAE"] is DBNull))
                    {
                        registroAux.maeNome = Convert.ToString(reader["NOME_MAE"]);
                    }

                    registroAux.alunoCpf = string.Empty;
                    if (reader["CPF"] != null && !(reader["CPF"] is DBNull))
                    {
                        registroAux.alunoCpf = Convert.ToString(reader["CPF"]);
                    }

                    registroAux.alunoRg = string.Empty;
                    if (reader["RG_NUM"] != null && !(reader["RG_NUM"] is DBNull))
                    {
                        registroAux.alunoRg = Convert.ToString(reader["RG_NUM"]);
                    }

                    registroAux.alunoRgUfExpedicao = string.Empty;
                    if (reader["RG_UF"] != null && !(reader["RG_UF"] is DBNull))
                    {
                        registroAux.alunoRgUfExpedicao = Convert.ToString(reader["RG_UF"]);
                    }

                    registroAux.alunoRGOrgaoExpedicao = string.Empty;
                    if (reader["RG_EMISSOR"] != null && !(reader["RG_EMISSOR"] is DBNull))
                    {
                        registroAux.alunoRGOrgaoExpedicao = Convert.ToString(reader["RG_EMISSOR"]);
                    }

                    registroAux.alunoRGDataExpedicao = DateTime.MinValue;
                    if (reader["RG_DTEXP"] != null && !(reader["RG_DTEXP"] is DBNull))
                    {
                        registroAux.alunoRGDataExpedicao = Convert.ToDateTime(reader["RG_DTEXP"]);
                    }

                    registroAux.enderecoCep = string.Empty;
                    if (reader["CEP"] != null && !(reader["CEP"] is DBNull))
                    {
                        registroAux.enderecoCep = Convert.ToString(reader["CEP"]);
                    }

                    registroAux.enderecoTipoLogradouro = string.Empty;
                    if (reader["END_TP_LOGRADOURO"] != null && !(reader["END_TP_LOGRADOURO"] is DBNull))
                    {
                        registroAux.enderecoTipoLogradouro = Convert.ToString(reader["END_TP_LOGRADOURO"]);
                    }

                    registroAux.enderecoNomeLogradouro = string.Empty;
                    if (reader["ENDERECO"] != null && !(reader["ENDERECO"] is DBNull))
                    {
                        registroAux.enderecoNomeLogradouro = Convert.ToString(reader["ENDERECO"]);
                    }

                    registroAux.enderecoNumero = string.Empty;
                    if (reader["END_NUM"] != null && !(reader["END_NUM"] is DBNull))
                    {
                        registroAux.enderecoNumero = Convert.ToString(reader["END_NUM"]);
                    }

                    registroAux.enderecoComplemento = string.Empty;
                    if (reader["END_COMPL"] != null && !(reader["END_COMPL"] is DBNull))
                    {
                        registroAux.enderecoComplemento = Convert.ToString(reader["END_COMPL"]);
                    }

                    registroAux.enderecoBairro = string.Empty;
                    if (reader["BAIRRO"] != null && !(reader["BAIRRO"] is DBNull))
                    {
                        registroAux.enderecoBairro = Convert.ToString(reader["BAIRRO"]);
                    }

                    registroAux.enderecoMunicipioestado = string.Empty;
                    if (reader["END_MUNICIPIO"] != null && !(reader["END_MUNICIPIO"] is DBNull))
                    {
                        registroAux.enderecoMunicipioestado = Convert.ToString(reader["END_MUNICIPIO"]);
                    }

                    registroAux.codigoCenso = string.Empty;
                    if (reader["UNIDADE_ENS"] != null && !(reader["UNIDADE_ENS"] is DBNull))
                    {
                        registroAux.codigoCenso = Convert.ToString(reader["UNIDADE_ENS"]);
                    }

                    registroAux.Turno = string.Empty;
                    if (reader["QHITurno"] != null && !(reader["QHITurno"] is DBNull))
                    {
                        registroAux.Turno = Convert.ToString(reader["QHITurno"]);
                    }

                    registroAux.Turma = string.Empty;
                    if (reader["QHITurma"] != null && !(reader["QHITurma"] is DBNull))
                    {
                        registroAux.Turma = Convert.ToString(reader["QHITurma"]);
                    }

                    if (reader["FOTO"] != null && !(reader["FOTO"] is DBNull))
                    {
                        registroAux.alunoFoto = (byte[])reader["FOTO"];
                    }

                    registroAux.gratuidade = string.Empty;
                    if (reader["GRATUIDADE"] != null && !(reader["GRATUIDADE"] is DBNull))
                    {
                        registroAux.gratuidade = Convert.ToString(reader["GRATUIDADE"]);
                    }

                    registroAux.modalTREM = string.Empty;
                    if (reader["MODALTREM"] != null && !(reader["MODALTREM"] is DBNull))
                    {
                        registroAux.modalTREM = Convert.ToString(reader["MODALTREM"]);
                    }

                    registroAux.modalONIBUS = string.Empty;
                    if (reader["MODALONIBUS"] != null && !(reader["MODALONIBUS"] is DBNull))
                    {
                        registroAux.modalONIBUS = Convert.ToString(reader["MODALONIBUS"]);
                    }

                    registroAux.modalMETRO = string.Empty;
                    if (reader["MODALMETRO"] != null && !(reader["MODALMETRO"] is DBNull))
                    {
                        registroAux.modalMETRO = Convert.ToString(reader["MODALMETRO"]);
                    }

                    registroAux.modalBARCAS = string.Empty;
                    if (reader["MODALBARCAS"] != null && !(reader["MODALBARCAS"] is DBNull))
                    {
                        registroAux.modalBARCAS = Convert.ToString(reader["MODALBARCAS"]);
                    }

                    registroAux.email = string.Empty;
                    if (reader["E_MAIL_INTERNO"] != null && !(reader["E_MAIL_INTERNO"] is DBNull))
                    {
                        registroAux.email = Convert.ToString(reader["E_MAIL_INTERNO"]);
                    }

                    registroAux.login = string.Empty;
                    if (reader["LOGINRIOCARD"] != null && !(reader["LOGINRIOCARD"] is DBNull))
                    {
                        registroAux.login = Convert.ToString(reader["LOGINRIOCARD"]);
                    }

                    registroAux.AssinaturaDigital = string.Empty;
                    if (reader["ASSINATURADIGITAL"] != null && !(reader["ASSINATURADIGITAL"] is DBNull))
                    {
                        registroAux.AssinaturaDigital = Convert.ToString(reader["ASSINATURADIGITAL"]);
                    }

                    registroAux.Serie = string.Empty;
                    if (reader["SERIE"] != null && !(reader["SERIE"] is DBNull))
                    {
                        registroAux.Serie = Convert.ToString(reader["SERIE"]);
                    }

                    lstRemessa.Add(registroAux);


                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Log.Adicionar(ex.ToString().Replace("\n", ". "));  
                throw ex;
            }
            finally
            {
                closeConn();
                conn2.Close();
            }

            return lstRemessa;
        }

        /// <summary>
        /// Grava os logs das remessas
        /// </summary>
        /// <returns></returns>
        public void Logremessa(string remessaid, SqlConnection conn2)
        {
            try
            {
                var cmd = new SqlCommand(LogremessaQuery(remessaid), conn2);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Log.Adicionar(ex.ToString().Replace("\n", ". ")); 
                throw ex;
            }
        }

        /// <summary>
        /// Método que retorna o total dos lotes de remessa 
        /// </summary>
        /// <returns></returns>
        public RetornoRemessa TotalRemessa(Int32 idLoteRemessa, string nomearquivo)
        {

            RetornoRemessa _retornoRemessa = new RetornoRemessa();

            try
            {

                var cmd = new SqlCommand(TotalRemessaQuery(idLoteRemessa), openConn());
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //_retornoRemessa.nomeArquivo = string.Empty;

                    //if (reader["nomeArquivo"] != null && !(reader["nomeArquivo"] is DBNull))
                    //{
                    _retornoRemessa.nomeLote = nomearquivo;
                    //}

                    _retornoRemessa.quantidadeRegistros = 0;

                    if (reader["quantidadeArquivos"] != null && !(reader["quantidadeArquivos"] is DBNull))
                    {
                        _retornoRemessa.quantidadeRegistros = Convert.ToInt32(reader["quantidadeArquivos"]);
                    }
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Log.Adicionar(ex.ToString().Replace("\n", ". ")); 
                throw ex;
            }
            finally
            {
                closeConn();
            }

            return _retornoRemessa;
        }

        /// <summary>
        /// Query com o total de remessas
        /// </summary>
        /// <returns></returns>
        public string TotalRemessaQuery(Int32 idLoteRemessa)
        {
            return "SELECT count(*) as quantidadeArquivos FROM CartaoEstudante.REMESSA where LOTEREMESSAID = " + idLoteRemessa;
        }

        /// <summary>
        /// Query Com a lista de remessas
        /// </summary>
        /// <returns></returns>
        public string ListaRemessaQuery(Int32 idLoteRemessa)
        {
            return @"SELECT T.CODSOLICITACAO AS SOLICITACAOID
                      ,[REMESSAID]
                      ,[DATAINCLUSAO]
                      ,R.ALUNO AS ALUNO
                      ,[NOME_COMPL]
                      ,[DT_NASC]
                      ,[NOME_PAI]
                      ,[NOME_MAE]
                      ,[CPF]
                      ,[RG_NUM]
                      ,[RG_UF]
                      ,[RG_EMISSOR]
                      ,[RG_DTEXP]
                      ,[CEP]
                      ,[END_TP_LOGRADOURO]
                      ,[ENDERECO]
                      ,[END_NUM]
                      ,[END_COMPL]
                      ,[BAIRRO]
                      ,[END_MUNICIPIO]
                      ,[UNIDADE_ENS]
                      ,[FOTO]
                      ,[GRATUIDADE]
                      ,[MODALTREM]
                      ,[MODALONIBUS]
                      ,[MODALMETRO]
                      ,[MODALBARCAS]
                      ,[STAMP_ATUALIZACAO]
                      ,[QHITURNO]
                      ,[QHITURMA]
                      ,[SERIE]
                      ,[E_MAIL_INTERNO]
                      ,[LOGINRIOCARD]
                      ,[ASSINATURADIGITAL]
                        from CartaoEstudante.REMESSA AS R 
                         INNER JOIN CartaoEstudante.SOLICITACAO AS S
	                        ON R.SOLICITACAOID = S.SOLICITACAOID
	                        INNER JOIN CartaoEstudante.TIPOSOLICITACAO AS T
	                        ON T.TIPOSOLICITACAOID = S.TIPOSOLICITACAOID
                            where LOTEREMESSAID = " + idLoteRemessa +
                            " order by SOLICITACAOID";
        }


        /// <summary>
        /// Query que grava os logs das remessas
        /// </summary>
        /// <returns></returns>
        public string LogremessaQuery(string remessaid)
        {
            return "INSERT INTO CartaoEstudante.LOGREMESSA (REMESSAID, DATAENVIO) VALUES(" + remessaid + ", GETDATE())";
        }
    }
}
