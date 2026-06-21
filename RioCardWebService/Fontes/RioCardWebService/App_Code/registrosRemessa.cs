using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Services;

[WebService(Namespace = "http://200.222.27.185/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class registrosremessa : WebService
{
    [WebMethod(Description = "registros_Remessa")]
    public registroRemessaResponse registros_Remessa(string nomeArquivo)
    {
        var registro = new registroRemessaResponse();

        var conexao = new SqlConnection(ConfigurationManager.ConnectionStrings["conexaoSQL"].ConnectionString);

        var sql1 = "SELECT distinct nomeArquivo, quantidaderegistros FROM RIOCARD_REMESSA where nomearquivo = '" + nomeArquivo + "' AND nomearquivo like 'SEEDUC%'";

        var sql = "SELECT tipomovimentacao, matriculanova, nomealuno, alunodatanascimento, " +
                  "painome, maenome, alunocpf, alunorg, alunorgufexpedicao, " +
                  "alunorgorgaoexpedicao, alunorgdataexpedicao, enderecocep, enderecotipologradouro, endereconomelogradouro, " +
                  "endereconumero, enderecocomplemento, enderecobairro, enderecomunicipio, " +
                  "codigocenso, qhiturno, qhiturma, alunofoto, gratuidade, modalTREM, modalONIBUS, modalMETRO, modalBARCAS from riocard_remessa where nomearquivo = '" + nomeArquivo + "' AND nomearquivo like 'SEEDUC%'";

        try
        {
            conexao.Open();

            var command1 = new SqlCommand(sql1, conexao);
            var reader1 = command1.ExecuteReader();

            while (reader1.Read())
            {
                registro.nomeArquivo = string.Empty;

                if (reader1["nomeArquivo"] != null && !(reader1["nomeArquivo"] is DBNull))
                {
                    registro.nomeArquivo = Convert.ToString(reader1["nomeArquivo"]);
                }

                registro.quantidadeRegistros = 0;

                if (reader1["quantidadeRegistros"] != null && !(reader1["quantidadeRegistros"] is DBNull))
                {
                    registro.quantidadeRegistros = Convert.ToInt32(reader1["quantidadeRegistros"]);
                }
            }

            reader1.Close();

            var command = new SqlCommand(sql, conexao);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                if (registro.registros == null)
                {
                    registro.registros = new List<registroRemessaResponse.registro>();
                }

                var registroAux = new registroRemessaResponse.registro();

                registroAux.tipoMovimentacao = 0;
                if (reader["tipoMovimentacao"] != null && !(reader["tipoMovimentacao"] is DBNull))
                {
                    registroAux.tipoMovimentacao = Convert.ToInt32(reader["tipoMovimentacao"]);
                }

                registroAux.matriculaNova = string.Empty;
                if (reader["matriculaNova"] != null && !(reader["matriculaNova"] is DBNull))
                {
                    registroAux.matriculaNova = Convert.ToString(reader["matriculaNova"]);
                }

                registroAux.alunoNome = string.Empty;
                if (reader["nomealuno"] != null && !(reader["nomealuno"] is DBNull))
                {
                    registroAux.alunoNome = Convert.ToString(reader["nomealuno"]);
                }

                registroAux.alunoDataNascimento = DateTime.MinValue;
                if (reader["alunoDataNascimento"] != null && !(reader["alunoDataNascimento"] is DBNull))
                {
                    var alunoDataNascimento = DateTime.MinValue;

                    if (DateTime.TryParse(Convert.ToString(reader["alunoDataNascimento"]), out alunoDataNascimento))
                    {
                        registroAux.alunoDataNascimento = alunoDataNascimento;
                    }
                }

                registroAux.paiNome = string.Empty;
                if (reader["paiNome"] != null && !(reader["paiNome"] is DBNull))
                {
                    registroAux.paiNome = Convert.ToString(reader["paiNome"]);
                }

                registroAux.maeNome = string.Empty;
                if (reader["maeNome"] != null && !(reader["maeNome"] is DBNull))
                {
                    registroAux.maeNome = Convert.ToString(reader["maeNome"]);
                }

                registroAux.alunoCpf = string.Empty;
                if (reader["alunoCpf"] != null && !(reader["alunoCpf"] is DBNull))
                {
                    registroAux.alunoCpf = Convert.ToString(reader["alunoCpf"]);
                }

                registroAux.alunoRg = string.Empty;
                if (reader["alunoRg"] != null && !(reader["alunoRg"] is DBNull))
                {
                    registroAux.alunoRg = Convert.ToString(reader["alunoRg"]);
                }

                registroAux.alunoRgUfExpedicao = string.Empty;
                if (reader["alunoRgUfExpedicao"] != null && !(reader["alunoRgUfExpedicao"] is DBNull))
                {
                    registroAux.alunoRgUfExpedicao = Convert.ToString(reader["alunoRgUfExpedicao"]);
                }

                registroAux.alunoRGOrgaoExpedicao = string.Empty;
                if (reader["alunoRGOrgaoExpedicao"] != null && !(reader["alunoRGOrgaoExpedicao"] is DBNull))
                {
                    registroAux.alunoRGOrgaoExpedicao = Convert.ToString(reader["alunoRGOrgaoExpedicao"]);
                }

                registroAux.alunoRGDataExpedicao = DateTime.MinValue;
                if (reader["alunoRGDataExpedicao"] != null && !(reader["alunoRGDataExpedicao"] is DBNull))
                {
                    registroAux.alunoRGDataExpedicao = Convert.ToDateTime(reader["alunoRGDataExpedicao"]);
                }

                registroAux.enderecoCep = string.Empty;
                if (reader["enderecoCep"] != null && !(reader["enderecoCep"] is DBNull))
                {
                    registroAux.enderecoCep = Convert.ToString(reader["enderecoCep"]);
                }

                registroAux.enderecoTipoLogradouro = string.Empty;
                if (reader["enderecoTipoLogradouro"] != null && !(reader["enderecoTipoLogradouro"] is DBNull))
                {
                    registroAux.enderecoTipoLogradouro = Convert.ToString(reader["enderecoTipoLogradouro"]);
                }

                registroAux.enderecoNomeLogradouro = string.Empty;
                if (reader["enderecoNomeLogradouro"] != null && !(reader["enderecoNomeLogradouro"] is DBNull))
                {
                    registroAux.enderecoNomeLogradouro = Convert.ToString(reader["enderecoNomeLogradouro"]);
                }

                registroAux.enderecoNumero = string.Empty;
                if (reader["enderecoNumero"] != null && !(reader["enderecoNumero"] is DBNull))
                {
                    registroAux.enderecoNumero = Convert.ToString(reader["enderecoNumero"]);
                }

                registroAux.enderecoComplemento = string.Empty;
                if (reader["enderecoComplemento"] != null && !(reader["enderecoComplemento"] is DBNull))
                {
                    registroAux.enderecoComplemento = Convert.ToString(reader["enderecoComplemento"]);
                }

                registroAux.enderecoBairro = string.Empty;
                if (reader["enderecoBairro"] != null && !(reader["enderecoBairro"] is DBNull))
                {
                    registroAux.enderecoBairro = Convert.ToString(reader["enderecoBairro"]);
                }

                registroAux.enderecoMunicipioestado = string.Empty;
                if (reader["enderecoMunicipio"] != null && !(reader["enderecoMunicipio"] is DBNull))
                {
                    registroAux.enderecoMunicipioestado = Convert.ToString(reader["enderecoMunicipio"]);
                }

                registroAux.codigoCenso = string.Empty;
                if (reader["codigoCenso"] != null && !(reader["codigoCenso"] is DBNull))
                {
                    registroAux.codigoCenso = Convert.ToString(reader["codigoCenso"]);
                }

                registroAux.QHITurno = string.Empty;
                if (reader["QHITurno"] != null && !(reader["QHITurno"] is DBNull))
                {
                    registroAux.QHITurno = Convert.ToString(reader["QHITurno"]);
                }

                registroAux.QHITurma = string.Empty;
                if (reader["QHITurma"] != null && !(reader["QHITurma"] is DBNull))
                {
                    registroAux.QHITurma = Convert.ToString(reader["QHITurma"]);
                }

                if (reader["alunofoto"] != null && !(reader["alunofoto"] is DBNull))
                {
                    registroAux.alunoFoto = (byte[])reader["alunofoto"];
                }

                registroAux.gratuidade = string.Empty;
                if (reader["gratuidade"] != null && !(reader["gratuidade"] is DBNull))
                {
                    registroAux.gratuidade = Convert.ToString(reader["gratuidade"]);
                }

                registroAux.modalTREM = string.Empty;
                if (reader["modalTREM"] != null && !(reader["modalTREM"] is DBNull))
                {
                    registroAux.modalTREM = Convert.ToString(reader["modalTREM"]);
                }

                registroAux.modalONIBUS = string.Empty;
                if (reader["modalONIBUS"] != null && !(reader["modalONIBUS"] is DBNull))
                {
                    registroAux.modalONIBUS = Convert.ToString(reader["modalONIBUS"]);
                }

                registroAux.modalMETRO = string.Empty;
                if (reader["modalMETRO"] != null && !(reader["modalMETRO"] is DBNull))
                {
                    registroAux.modalMETRO = Convert.ToString(reader["modalMETRO"]);
                }

                registroAux.modalBARCAS = string.Empty;
                if (reader["modalBARCAS"] != null && !(reader["modalBARCAS"] is DBNull))
                {
                    registroAux.modalBARCAS = Convert.ToString(reader["modalBARCAS"]);
                }

                registro.registros.Add(registroAux);
            }

            reader.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (conexao.State == System.Data.ConnectionState.Open)
            {
                conexao.Close();
            }
        }

        return registro;
    }
}