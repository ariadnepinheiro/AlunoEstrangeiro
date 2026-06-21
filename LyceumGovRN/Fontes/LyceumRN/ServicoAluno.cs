using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Techne.Data;
using Techne.Lyceum.CR;
using System.Xml;

namespace Techne.Lyceum.RN
{
    public class ServicoAluno : RNBase
    {
        public static string GravarAluno(int lote, int registro, string matricula, string alunoDddCel, string alunoCelular, string flagEnderecoConflitante, string flagDadosCelularPaiConflitante, string flagDadosCelularMaeConflitante, string flagDadosCelularResponsavelConflitante, string foto)
        {
            string alunoDddCelular = alunoDddCel;
            string alunoCelularRes;

            TConnectionWritable cn = Config.CreateWritableConnection();
            cn.Open(true);
            try
            {
                string regLog = string.Empty;
                if (matricula.Length > 20 || matricula.Length == 0)
                    regLog += "Matrícula inválida. ";


                if (flagEnderecoConflitante.Length != 1 || flagDadosCelularPaiConflitante.Length != 1 || flagDadosCelularMaeConflitante.Length != 1 || flagDadosCelularResponsavelConflitante.Length != 1)
                    regLog += "Erro flag. ";

                if ((flagEnderecoConflitante != "S" && flagEnderecoConflitante != "N") ||
                   (flagDadosCelularPaiConflitante != "S" && flagDadosCelularPaiConflitante != "N") ||
                   (flagDadosCelularMaeConflitante != "S" && flagDadosCelularMaeConflitante != "N") ||
                   (flagDadosCelularResponsavelConflitante != "S" && flagDadosCelularResponsavelConflitante != "N"))
                {
                    regLog += "Erro flag. ";
                }

                if (alunoDddCelular.Length > 3)
                {
                    regLog += "Erro DDD. ";
                }
                if (alunoDddCelular.Length == 3)
                {
                    alunoDddCelular = alunoDddCelular.Substring(1);
                }
                if (alunoCelular.Length != 8)
                {
                    regLog += "Erro celular. ";
                }
                alunoCelularRes = (alunoDddCelular == "00" ? "" : alunoDddCelular) + (alunoCelular == "00000000" ? "" : alunoCelular);
                RetValue retorno = null;

                QueryTable qt = new QueryTable("Select pessoa from ly_aluno where aluno = ? ");
                qt.Query(cn, matricula);
                if (qt.Rows.Count != 1)
                {
                    regLog += "Aluno não existe";
                }

                if (qt.Rows.Count == 1)
                {
                    string sqlQueryPessoa = @"if exists (select pessoa from LY_PESSOA where PESSOA = ?)
				                                update LY_PESSOA set CELULAR = ? where PESSOA = ?";

                    retorno = IAE(cn, sqlQueryPessoa, qt.Rows[0]["pessoa"], alunoCelularRes, qt.Rows[0]["pessoa"]);
                    if (retorno != null && !retorno.Ok)
                    {
                        regLog += retorno.Errors.ToString();
                    }

                    byte[] fotoAlunoByte = Convert.FromBase64String(foto);
                    if (fotoAlunoByte.Length > 0)
                    {
                        string msgFotoAluno = FotoPessoa.ValidaFoto(fotoAlunoByte);
                        if (msgFotoAluno == string.Empty)
                        {
                            string sqlQueryFoto = @"if not exists (select pessoa from LY_FOTO_PESSOA where PESSOA = ?)
			                                    Insert into LY_FOTO_PESSOA (pessoa, foto) values(?,?)
		                                    else
			                                    update LY_FOTO_PESSOA set FOTO = ? where PESSOA = ?";
                            retorno = IAE(cn, sqlQueryFoto, qt.Rows[0]["pessoa"], qt.Rows[0]["pessoa"], fotoAlunoByte, fotoAlunoByte, qt.Rows[0]["pessoa"]);
                            if (retorno != null && !retorno.Ok)
                            {
                                regLog += retorno.Errors.ToString();
                            }
                        }
                        else
                        {
                            regLog += msgFotoAluno;
                        }
                    }
                }
                string sqlQuery = @"If not Exists (select MATRICULA_NOVA from WEBSERVICE_OI where LOTE = ? and REGISTRO = ? )
                                        BEGIN
                                            insert into WEBSERVICE_OI (LOTE, REGISTRO, MATRICULA_NOVA,ALUNO_DDD_CELULAR, ALUNO_CELULAR, FLAG_ENDERECO_CONFLITANTE,FLAG_DADOS_CELULAR_PAI_CONFLITANTE,FLAG_DADOS_CELULAR_MAE_CONFLITANTE,FLAG_DADOS_CELULAR_RESPONSAVEL_CONFLITANTE,REG_LOG,REG_TIME)
                                            values(?,?,?,?,?,?,?,?,?,?,GETDATE())
                                        END";

                retorno = IAE(cn, sqlQuery, lote, registro, lote, registro, matricula, alunoDddCelular, alunoCelular, flagEnderecoConflitante, flagDadosCelularPaiConflitante, flagDadosCelularMaeConflitante, flagDadosCelularResponsavelConflitante, regLog.Substring(0, regLog.Length > 200 ? 199 : regLog.Length));
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                cn.Close();
            }

            return "Aluno gravado com sucesso.";
        }

        public static string GravarAluno(int lote, int registro, string qtRegistros)
        {
            string sqlQuery = @"If not Exists (select MATRICULA_NOVA from WEBSERVICE_OI where LOTE = ? and REGISTRO = ? )
                                        BEGIN
                                            insert into WEBSERVICE_OI (LOTE, REGISTRO, MATRICULA_NOVA,REG_LOG,REG_TIME)
                                            values(?,?,?,?,GETDATE())
                                        END";
            TConnectionWritable cn = Config.CreateWritableConnection();
            RetValue retorno = null;
            cn.Open(true);
            try
            {
                retorno = IAE(cn, sqlQuery, lote, registro, lote, registro, qtRegistros, "Quantidade de registros: " + qtRegistros);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                cn.Close();
            }

            return string.Empty;
        }
    }
}
