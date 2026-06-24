using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Seeduc.Infra.Data;
using Seeduc.Infra.Extensions;
using Seeduc.Infra.Validation;
using Techne.Data;
using Techne.Library;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN.DTOs;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Servicos;
using Techne.Lyceum.RN.Util;

namespace Techne.Lyceum.RN
{
    [DataObjectAttribute]
    public class Pessoa : RNBase
    {
        public static Ly_pessoa.Row Consultar(string pessoa)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            try
            {
                return Ly_pessoa.QueryFirstRow(connection, "pessoa = ?", pessoa.ToDecimal());
            }
            finally
            {
                connection.Close();
            }
        }

        public static decimal GeraPessoa()
        {
            //Gera código de pessoa a partir do último do banco

            var contextQuery = new ContextQuery(
                @" SELECT ISNULL(MAX(PESSOA),0)+1 FROM LY_PESSOA ");

            return ExecutarFuncao(contextQuery);
        }

        public static decimal GeraPessoa(TConnection connection)
        {
            //Gera código de pessoa a partir do último do banco
            decimal pessoa = (decimal)TCommand.ExecuteScalar(connection, "SELECT ISNULL(MAX(PESSOA),0)+1 FROM LY_PESSOA");

            return pessoa;
        }

        public static decimal GeraOrdem(string param)
        {
            //Gera código de ordem (chave) a partir do último do banco
            TConnection connection = Config.CreateConnection();
            connection.Open();
            decimal ordem;
            DbObject dbordem;
            try
            {
                dbordem = TCommand.ExecuteScalar(connection, "select max(chave) from ly_curriculo_pessoa where pessoa = ?", param);
            }
            finally
            {
                connection.Close();
            }
            if (!dbordem.IsNull)
            {
                ordem = (decimal)dbordem;
                return ordem + 1;
            }
            return 1;
        }

        public static SimpleRow VerificarCPF(string cpf)
        {
            if ((!string.IsNullOrEmpty(cpf)))
            {

                string sql = @"SELECT pessoa, nome_compl, endereco, end_num, end_municipio, cep, rg_tipo, rg_num, rg_dtexp, rg_emissor, rg_uf FROM LY_PESSOA WHERE CPF=? ";

                TConnection connection = Config.CreateConnection();
                connection.Open();

                using (QueryTable qt = new QueryTable(sql))
                {
                    try
                    {
                        qt.Query(connection, cpf.RetirarMascaraCPF());
                        if (qt.Rows.Count > 0)
                            return qt.Rows[0];
                    }
                    finally
                    {
                        connection.Close();
                    }
                }


            }
            else if ((!string.IsNullOrEmpty(cpf.RetirarMascaraCPF())))
            {
                return null;
            }

            return null;
        }

        public static QueryTable ConsultarCPF(string cpf)
        {
            TConnection connection = Config.CreateConnection();
            connection.Open();
            QueryTable qt = null;
            string sql = "select top 1 pessoa,cpf from ly_pessoa WHERE cpf = ?";
            try
            {
                qt = new QueryTable(sql);
                qt.Query(connection, cpf);
            }
            finally
            {
                connection.Close();
            }
            return qt;
        }

        public static bool VerificaMaeCadastrada(string pessoa)
        {
            if (String.IsNullOrEmpty(pessoa))
                return false;
            else
                return !String.IsNullOrEmpty(Convert.ToString(ExecutarFuncaoScalar("select NOME_MAE from ly_pessoa where pessoa = ?", pessoa.Trim())));
        }

        public static bool VerificarInep(string id_censo, decimal pessoa)
        {
            if (!string.IsNullOrEmpty(id_censo))
            {
                TConnection connection = Config.CreateConnection();
                connection.Open();
                try
                {
                    string sql = " select 1 from ly_pessoa where pessoa <> ? AND id_censo = ? ";
                    DbObject valorConsulta = TCommand.ExecuteScalar(connection, sql, pessoa, id_censo);
                    if (!valorConsulta.IsNull)
                        return false;
                }
                finally
                {
                    connection.Close();
                }
            }

            return true;
        }

        public static RetValue AlteraEmail(string email, decimal pessoa)
        {
            using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
            {
                try
                {
                    var contextQuery = new ContextQuery { Command = "UPDATE LY_PESSOA SET E_MAIL_INTERNO = @EMAIL WHERE PESSOA = @PESSOA" };

                    contextQuery.Parameters.Add("@EMAIL", email);
                    contextQuery.Parameters.Add("@PESSOA", pessoa);

                    ctx.ApplyModifications(contextQuery);
                }
                catch (Exception ex)
                {
                    ctx.Abandon();

                    new RetValue(false, string.Empty, new ErrorList(ex.Message));
                }
            }

            return new RetValue(true, "E-mail interno alterado com sucesso.", null);
        }

        public static string PreUpdate(Ly_pessoa.Row row, TConnectionWritable cn)
        {
            RN.RNBase.RetirarEspaco(row);

            //quantidade de caracteres
            if (!string.IsNullOrEmpty(row.Cep))
            {
                row.Cep = row.Cep.RetirarCaracteres();
                if (row.Cep.ToString().Length < 8) return "CEP inválido. <br>O CEP deve ter 8 números.";
            }
            if (!string.IsNullOrEmpty(row.Nome_compl))
            {
                if (row.Nome_compl.Length < 5)
                    return "Nome deve conter pelo menos cinco letras.";
            }

            //valida tipo do dado
            if (!Validacao.Validou(row.Nome_compl, Validacao.Tipo.nome)) return "Nome inválido.<br>O nome deve ter apenas letras.";
            if (!string.IsNullOrEmpty(row.Cprof_num) && !Validacao.Validou(row.Cprof_num, Validacao.Tipo.numerico)) return "Número da carteira profissional inválido.<br>O número da carteira profissional deve ter somente números.";

            //obrigatórios
            if (string.IsNullOrEmpty(row.Dt_nasc.ToString())) return "Data de nascimento inválida.<br>Preenchimento obrigatório.";
            if (string.IsNullOrEmpty(Convert.ToString(row.Municipio_nasc))) return "Município de nascimento inválido.<br>Preenchimento obrigatório.";
            if (string.IsNullOrEmpty(Convert.ToString(row.Nacionalidade))) return "Nacionalidade inválida.<br>Preenchimento obrigatório.";

            //e-mail
            if (!string.IsNullOrEmpty(row.E_mail) && !Validacao.Validou(row.E_mail, Validacao.Tipo.email)) return "Email externo inválido.<br>O e-mail está em um formato incorreto.";
            if (!string.IsNullOrEmpty(row.E_mail_interno) && !Validacao.Validou(row.E_mail_interno, Validacao.Tipo.email)) return "Email interno inválido.<br>O e-mail está em um formato incorreto.";

            //dados carteira profissional
            if (!string.IsNullOrEmpty(row.Cprof_num) && (string.IsNullOrEmpty(row.Cprof_dtexp.ToString()) || string.IsNullOrEmpty(row.Cprof_serie) || string.IsNullOrEmpty(row.Cprof_uf)))
                return "Caso um dado da carteira profissional seja preenchido, todos os outros devem ser preenchidos também.";
            else if (!string.IsNullOrEmpty(row.Cprof_dtexp.ToString()) && (string.IsNullOrEmpty(row.Cprof_num) || string.IsNullOrEmpty(row.Cprof_serie) || string.IsNullOrEmpty(row.Cprof_uf)))
                return "Caso um dado da carteira profissional seja preenchido, todos os outros devem ser preenchidos também.";
            else if (!string.IsNullOrEmpty(row.Cprof_serie) && (string.IsNullOrEmpty(row.Cprof_num) || string.IsNullOrEmpty(row.Cprof_dtexp.ToString()) || string.IsNullOrEmpty(row.Cprof_uf)))
                return "Caso um dado da carteira profissional seja preenchido, todos os outros devem ser preenchidos também.";
            else if (!string.IsNullOrEmpty(row.Cprof_uf) && (string.IsNullOrEmpty(row.Cprof_num) || string.IsNullOrEmpty(row.Cprof_dtexp.ToString()) || string.IsNullOrEmpty(row.Cprof_serie)))
                return "Caso um dado da carteira profissional seja preenchido, todos os outros devem ser preenchidos também.";

            #region Município
            if (!string.IsNullOrEmpty(row.Pais_nasc))
            {
                string paisNasc;
                paisNasc = RN.Endereco.ObterPais(row.Pais_nasc);
                if (!string.IsNullOrEmpty(paisNasc))
                {
                    if (paisNasc.ToUpper() == "BRASIL")
                    {
                        if (!string.IsNullOrEmpty(row.Municipio_nasc))
                        {
                            if (!RN.Endereco.VerificarMunicipio(row.Municipio_nasc))
                                return "Município de nascimento inválido.";
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(row.Municipio_nasc))
                        {
                            if (!RN.Endereco.VerificarMunicipioEstrangeiro(row.Municipio_nasc.ToString(), row.Pais_nasc))
                                return "Município de nascimento inválido.";
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(row.End_pais))
            {
                string paisEnd;
                paisEnd = RN.Endereco.ObterPais(row.End_pais);
                if (!string.IsNullOrEmpty(paisEnd))
                {
                    if (paisEnd.ToUpper() == "BRASIL")
                    {
                        if (!string.IsNullOrEmpty(row.End_municipio))
                        {
                            if (!RN.Endereco.VerificarMunicipio(row.End_municipio))
                                return "Município do endereço inválido.";
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(row.End_municipio))
                        {
                            if (!RN.Endereco.VerificarMunicipioEstrangeiro(row.End_municipio.ToString(), row.End_pais))
                                return "Município do endereço inválido.";
                        }
                    }
                }
            }

            #endregion

            if (!string.IsNullOrEmpty(row.Id_censo))
            {
                if (row.Pessoa.HasValue)
                {
                    if (!RN.Pessoa.VerificarInep(row.Id_censo, row.Pessoa.Value))
                        return "O Número de Identificação no INEP já está sendo usado por outra pessoa.";
                }
            }

            //datas
            if (!string.IsNullOrEmpty(row.Dt_nasc.ToString()) && !Validacao.ValidouData(row.Dt_nasc, Validacao.Tipo.data))
                return "Data de nascimento inválida.<br>A data de nascimento deve ser maior que 1900 e não pode ser maior que a data de hoje.";
            if (!Validacao.ValidouDuasDatas(row.Dt_nasc, row.Rg_dtexp)) return "Data expedição documento/nascimento inválidas.<br>A data de expedição do documento de indentificação deve ser maior que a data de nascimento.";
            if (!Validacao.ValidouDuasDatas(row.Dt_nasc, row.Cprof_dtexp)) return "Data expedição carteira profissional/nascimento inválidas.<br>A data de expedição da carteira profissional deve ser maior que a data de nascimento.";
            if (!Validacao.ValidouDuasDatas(row.Dt_nasc, row.Cert_nasc_emissao)) return "Data de emissão da certidão/nascimento inválidas.<br>A data de emissão da certidão de nascimento deve ser maior que a data de nascimento.";
            if (!Validacao.ValidouDuasDatas(row.Dt_nasc, row.Teleitor_dtexp)) return "Data de emissão do título de eleitor/nascimento inválidas.<br>A data de emissão do título de eleitor deve ser maior que a data de nascimento.";
            if (!Validacao.ValidouDuasDatas(row.Dt_nasc, row.Alist_dtexp)) return "Data de emissão do alistamento militar/nascimento inválidas.<br>A data de emissão do alistamento militar deve ser maior que a data de nascimento.";
            if (!Validacao.ValidouDuasDatas(row.Dt_nasc, row.Cr_dtexp)) return "Data de emissão do certificado de reservista/nascimento inválidas.<br>A data de emissão do certificado de reservista deve ser maior que a data de nascimento.";

            if (!string.IsNullOrEmpty(row.Cpf))
            {
                row.Cpf = row.Cpf.RetirarMascaraCPF();
                if (!Validacao.ValidaCpf(row.Cpf))
                    return "O CPF informado é inválido.";

            }
            if (!string.IsNullOrEmpty(row.Rg_num))
            {
                row.Rg_num = row.Rg_num.RetirarMascaraRG();
                if (row.Rg_num.Length < 5)
                    return "O número do documento deve conter no mínimo cinco dígitos.";
            }

            if (!string.IsNullOrEmpty(row.Celular))
            {
                row.Celular = row.Celular.RetirarMascaraTelefone();
                bool celularOk = RN.Validacao.ValidaCelularComDDD(row.Celular);
                if (celularOk != true)
                    return "Celular inválido.";
            }
            if (!string.IsNullOrEmpty(row.Fone))
            {
                row.Fone = row.Fone.RetirarMascaraTelefone();
                bool telefoneOk = RN.Validacao.ValidaTelefoneComDDD(row.Fone);
                if (telefoneOk != true)
                    return "Telefone inválido.";
            }
            if (!string.IsNullOrEmpty(row.Bairro))
            {
                bool bairroOk = RN.Validacao.ValidaBairro(row.Bairro);
                if (bairroOk != true)
                    return "Bairro inválido.";
            }
            return string.Empty;
        }

        public void Insere(DataContext ctx, LyPessoa pessoa)
        {
            object obj = new Object();
            DateTime dataHoraAtual = DateTime.Now;

            var contextQuery = new ContextQuery
            {
                Command = @" INSERT INTO LY_PESSOA 
                                (PESSOA, 
                                 ALIST_CSM, 
                                 ALIST_DTEXP, 
                                 ALIST_NUM, 
                                 ALIST_RM, 
                                 ALIST_SERIE, 
                                 BAIRRO, 
                                 CELULAR, 
                                 CEP, 
                                 CPF, 
                                 CPROF_DTEXP, 
                                 CPROF_NUM, 
                                 CPROF_SERIE, 
                                 CPROF_UF, 
                                 CR_CAT, 
                                 CR_CSM, 
                                 CR_DTEXP, 
                                 CR_NUM, 
                                 CR_RM, 
                                 CR_SERIE, 
                                 DT_NASC, 
                                 E_MAIL, 
                                 E_MAIL_INTERNO, 
                                 END_COMPL, 
                                 END_MUNICIPIO, 
                                 END_NUM, 
                                 END_PAIS, 
                                 ENDERECO, 
                                 EST_CIVIL, 
                                 FONE, 
                                 ID_CENSO, 
                                 MUNICIPIO_NASC, 
                                 NACIONALIDADE, 
                                 NECESSIDADEESPECIALID, 
                                 NOME_COMPL,                                  
                                 PAIS_NASC, 
                                 RG_DTEXP, 
                                 RG_EMISSOR, 
                                 RG_NUM, 
                                 RG_TIPO, 
                                 RG_UF, 
                                 SEXO, 
                                 STAMP_ATUALIZACAO, 
                                 TELEITOR_DTEXP, 
                                 TELEITOR_NUM, 
                                 TELEITOR_SECAO, 
                                 TELEITOR_ZONA, 
                                 CERT_NASC_NUM, 
                                 CERT_NASC_FOLHA, 
                                 CERT_NASC_LIVRO, 
                                 CERT_NASC_EMISSAO, 
                                 CERT_NASC_CARTORIO_UF, 
                                 CERT_NASC_CARTORIO_EXPED, 
                                 TIPO_SANGUINEO, 
                                 ETNIA, 
                                 CREDO, 
                                 QT_FILHOS,                                  
                                 CERT_NUMERO_MATRICULA, 
                                 ID_CARTORIO, 
                                 NOME_MAE, 
                                 NOME_PAI, 
                                 PAI_FALECIDO, 
                                 MAE_FALECIDA, 
                                 MAE_CPF, 
                                 PAI_CPF, 
                                 MAE_TELEFONE, 
                                 PAI_TELEFONE, 
                                 RESPONSAVEL, 
                                 RESP_NOME_COMPL, 
                                 RESP_CPF, 
                                 RESP_FONE, 
                                 PASSAPORTE, 
                                 TELEITOR_MUN, 
                                 IDFUNCIONAL, 
                                 PISPASEP, 
                                 LATITUDE, 
                                 LONGITUDE, 
                                 AREA_QUILOMBOS,
                                 AREA_TRADICIONAL,
                                 TERRA_INDIGENA, 
                                 AREA_ASSENTAMENTO,
                                 USUARIOID, 
                                 DATACADASTRO, 
                                 DATAALTERACAO, 
                                 PRE_NOME_SOCIAL ) 
                    VALUES      ( (SELECT ISNULL(MAX(PESSOA), 0) + 1 
                                   FROM   LY_PESSOA WITH (UPDLOCK)), 
                                  @ALIST_CSM, 
                                  @ALIST_DTEXP, 
                                  @ALIST_NUM, 
                                  @ALIST_RM, 
                                  @ALIST_SERIE, 
                                  @BAIRRO, 
                                  @CELULAR, 
                                  @CEP, 
                                  @CPF, 
                                  @CPROF_DTEXP, 
                                  @CPROF_NUM, 
                                  @CPROF_SERIE, 
                                  @CPROF_UF, 
                                  @CR_CAT, 
                                  @CR_CSM, 
                                  @CR_DTEXP, 
                                  @CR_NUM, 
                                  @CR_RM, 
                                  @CR_SERIE, 
                                  @DT_NASC, 
                                  @E_MAIL, 
                                  @E_MAIL_INTERNO, 
                                  @END_COMPL, 
                                  @END_MUNICIPIO, 
                                  @END_NUM, 
                                  @END_PAIS, 
                                  @ENDERECO, 
                                  @EST_CIVIL, 
                                  @FONE, 
                                  @ID_CENSO, 
                                  @MUNICIPIO_NASC, 
                                  @NACIONALIDADE, 
                                  @NECESSIDADEESPECIALID, 
                                  @NOME_COMPL,                                   
                                  @PAIS_NASC, 
                                  @RG_DTEXP, 
                                  @RG_EMISSOR, 
                                  @RG_NUM, 
                                  @RG_TIPO, 
                                  @RG_UF, 
                                  @SEXO, 
                                  @STAMP_ATUALIZACAO, 
                                  @TELEITOR_DTEXP, 
                                  @TELEITOR_NUM, 
                                  @TELEITOR_SECAO, 
                                  @TELEITOR_ZONA, 
                                  @CERT_NASC_NUM, 
                                  @CERT_NASC_FOLHA, 
                                  @CERT_NASC_LIVRO, 
                                  @CERT_NASC_EMISSAO, 
                                  @CERT_NASC_CARTORIO_UF, 
                                  @CERT_NASC_CARTORIO_EXPED, 
                                  @TIPO_SANGUINEO, 
                                  @ETNIA, 
                                  @CREDO, 
                                  @QT_FILHOS, 
                                  
                                  @CERT_NUMERO_MATRICULA, 
                                  @ID_CARTORIO, 
                                  @NOME_MAE, 
                                  @NOME_PAI, 
                                  @PAI_FALECIDO, 
                                  @MAE_FALECIDA, 
                                  @MAE_CPF, 
                                  @PAI_CPF, 
                                  @MAE_TELEFONE, 
                                  @PAI_TELEFONE, 
                                  @RESPONSAVEL, 
                                  @RESP_NOME_COMPL, 
                                  @RESP_CPF, 
                                  @RESP_FONE, 
                                  @PASSAPORTE, 
                                  @TELEITOR_MUN, 
                                  @IDFUNCIONAL, 
                                  @PISPASEP, 
                                  @LATITUDE, 
                                  @LONGITUDE, 
                                  @AREA_QUILOMBOS, 
                                  @AREA_TRADICIONAL,
                                  @TERRA_INDIGENA, 
                                  @AREA_ASSENTAMENTO,
                                  @USUARIOID, 
                                  @DATACADASTRO, 
                                  @DATAALTERACAO, @PRE_NOME_SOCIAL ) "
            };

            contextQuery.Parameters.Add("@Alist_csm", TechneDbType.T_ALFASMALL, pessoa.Alist_csm);
            contextQuery.Parameters.Add("@Alist_dtexp", TechneDbType.T_DATA, pessoa.Alist_dtexp);
            contextQuery.Parameters.Add("@Alist_num", TechneDbType.T_ALFASMALL_17, pessoa.Alist_num);
            contextQuery.Parameters.Add("@Alist_rm", TechneDbType.T_ALFASMALL, pessoa.Alist_rm);
            contextQuery.Parameters.Add("@Alist_serie", TechneDbType.T_ALFASMALL, pessoa.Alist_serie);
            contextQuery.Parameters.Add("@Bairro", TechneDbType.T_ALFAMEDIUM, pessoa.Bairro);
            contextQuery.Parameters.Add("@Celular", TechneDbType.T_TELEFONE, pessoa.Celular);
            contextQuery.Parameters.Add("@Cep", pessoa.Cep);
            contextQuery.Parameters.Add("@Cpf", pessoa.Cpf);
            contextQuery.Parameters.Add("@Cprof_dtexp", TechneDbType.T_DATA, pessoa.Cprof_dtexp);
            contextQuery.Parameters.Add("@Cprof_num", pessoa.Cprof_num);
            contextQuery.Parameters.Add("@Cprof_serie", pessoa.Cprof_serie);
            contextQuery.Parameters.Add("@Cprof_uf", pessoa.Cprof_uf);
            contextQuery.Parameters.Add("@Cr_cat", pessoa.Cr_cat);
            contextQuery.Parameters.Add("@Cr_csm", pessoa.Cr_csm);
            contextQuery.Parameters.Add("@Cr_dtexp", TechneDbType.T_DATA, pessoa.Cr_dtexp);
            contextQuery.Parameters.Add("@Cr_num", pessoa.Cr_num);
            contextQuery.Parameters.Add("@Cr_rm", pessoa.Cr_rm);
            contextQuery.Parameters.Add("@Cr_serie", pessoa.Cr_serie);
            contextQuery.Parameters.Add("@Dt_nasc", TechneDbType.T_DATA, pessoa.Dt_nasc);
            contextQuery.Parameters.Add("@E_mail", pessoa.E_mail);
            contextQuery.Parameters.Add("@E_mail_interno", pessoa.E_mail_interno);
            contextQuery.Parameters.Add("@End_compl", pessoa.End_compl);
            contextQuery.Parameters.Add("@End_municipio", pessoa.End_municipio);
            contextQuery.Parameters.Add("@End_num", pessoa.End_num);
            contextQuery.Parameters.Add("@End_pais", pessoa.End_pais);
            contextQuery.Parameters.Add("@Endereco", pessoa.Endereco);
            contextQuery.Parameters.Add("@Est_civil", pessoa.Est_civil);
            contextQuery.Parameters.Add("@Fone", pessoa.Fone);
            contextQuery.Parameters.Add("@Id_censo", pessoa.Id_censo);
            contextQuery.Parameters.Add("@Municipio_nasc", pessoa.Municipio_nasc);
            contextQuery.Parameters.Add("@Nacionalidade", pessoa.Nacionalidade);
            contextQuery.Parameters.Add("@NECESSIDADEESPECIALID", pessoa.NecessidadeEspecialId);
            contextQuery.Parameters.Add("@NOME_COMPL", pessoa.Nome_compl.ToUpper());
            contextQuery.Parameters.Add("@Pais_nasc", pessoa.Pais_nasc);
            contextQuery.Parameters.Add("@Rg_dtexp", TechneDbType.T_DATA, pessoa.Rg_dtexp);
            contextQuery.Parameters.Add("@Rg_emissor", pessoa.Rg_emissor);
            contextQuery.Parameters.Add("@Rg_num", pessoa.Rg_num);
            contextQuery.Parameters.Add("@Rg_tipo", pessoa.Rg_tipo);
            contextQuery.Parameters.Add("@Rg_uf", pessoa.Rg_uf);
            contextQuery.Parameters.Add("@Sexo", pessoa.Sexo);
            contextQuery.Parameters.Add("@Stamp_atualizacao", SqlDbType.DateTime, dataHoraAtual);
            contextQuery.Parameters.Add("@Teleitor_dtexp", TechneDbType.T_DATA, pessoa.Teleitor_dtexp);
            contextQuery.Parameters.Add("@Teleitor_num", pessoa.Teleitor_num);
            contextQuery.Parameters.Add("@Teleitor_secao", pessoa.Teleitor_secao);
            contextQuery.Parameters.Add("@Teleitor_zona", pessoa.Teleitor_zona);
            contextQuery.Parameters.Add("@CERT_NASC_NUM", pessoa.CertNascNum);
            contextQuery.Parameters.Add("@CERT_NASC_FOLHA ", pessoa.CertNascFolha);
            contextQuery.Parameters.Add("@CERT_NASC_LIVRO", pessoa.CertNascLivro);
            contextQuery.Parameters.Add("@CERT_NASC_EMISSAO", TechneDbType.T_DATA, pessoa.CertNascEmissao);
            contextQuery.Parameters.Add("@CERT_NASC_CARTORIO_UF ", pessoa.CertNascCartorioUf);
            contextQuery.Parameters.Add("@CERT_NASC_CARTORIO_EXPED", pessoa.CertNascCartorioExped);
            contextQuery.Parameters.Add("@TIPO_SANGUINEO", pessoa.Tipo_Sanguineo);
            contextQuery.Parameters.Add("@ETNIA", pessoa.Etnia);
            contextQuery.Parameters.Add("@CREDO", pessoa.Credo);
            contextQuery.Parameters.Add("@QT_FILHOS", TechneDbType.T_NUMERO_PEQUENO, pessoa.QtFilhos);
            contextQuery.Parameters.Add("@PRE_NOME_SOCIAL", pessoa.Nome_social);
            contextQuery.Parameters.Add("@CERT_NUMERO_MATRICULA", pessoa.CertNumeroMatricula);
            contextQuery.Parameters.Add("@ID_CARTORIO", pessoa.IdCartorio);
            contextQuery.Parameters.Add("@NOME_MAE", pessoa.NomeMae.IsNullOrEmptyOrWhiteSpace() ? null : pessoa.NomeMae.ToUpper());
            contextQuery.Parameters.Add("@NOME_PAI", pessoa.NomePai.IsNullOrEmptyOrWhiteSpace() ? null : pessoa.NomePai.ToUpper());
            contextQuery.Parameters.Add("@PAI_FALECIDO", pessoa.PaiFalecido);
            contextQuery.Parameters.Add("@MAE_FALECIDA", pessoa.MaeFalecida);
            contextQuery.Parameters.Add("@MAE_CPF", pessoa.MaeCpf);
            contextQuery.Parameters.Add("@PAI_CPF", pessoa.PaiCpf);
            contextQuery.Parameters.Add("@MAE_TELEFONE", pessoa.MaeTelefone);
            contextQuery.Parameters.Add("@PAI_TELEFONE", pessoa.PaiTelefone);
            contextQuery.Parameters.Add("@RESPONSAVEL", pessoa.Responsavel);
            contextQuery.Parameters.Add("@RESP_NOME_COMPL", pessoa.RespNomeCompl.IsNullOrEmptyOrWhiteSpace() ? null : pessoa.RespNomeCompl.ToUpper());
            contextQuery.Parameters.Add("@RESP_CPF", pessoa.RespCpf);
            contextQuery.Parameters.Add("@RESP_FONE", pessoa.RespFone);
            contextQuery.Parameters.Add("@PASSAPORTE", pessoa.Passaporte);
            contextQuery.Parameters.Add("@TELEITOR_MUN", pessoa.Teleitor_mun);
            contextQuery.Parameters.Add("@IDFUNCIONAL", pessoa.IdFuncional);
            contextQuery.Parameters.Add("@PISPASEP", pessoa.Pispasep);
            contextQuery.Parameters.Add("@USUARIOID", pessoa.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, dataHoraAtual);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, dataHoraAtual);
            contextQuery.Parameters.Add("@LATITUDE", pessoa.Latitude);
            contextQuery.Parameters.Add("@LONGITUDE", pessoa.Longitude);
            contextQuery.Parameters.Add("@AREA_ASSENTAMENTO", pessoa.AreaAssentamento);
            contextQuery.Parameters.Add("@TERRA_INDIGENA", pessoa.TerraIndigena);
            contextQuery.Parameters.Add("@AREA_QUILOMBOS", pessoa.AreaQuilombos);
            contextQuery.Parameters.Add("@AREA_TRADICIONAL", pessoa.AreaTradicional);

            ctx.ApplyModifications(contextQuery);


            contextQuery = new ContextQuery(
             @" SELECT  PESSOA
                    FROM    LY_PESSOA
                    WHERE   DT_NASC = @DT_NASC
                            AND NOME_COMPL = @NOME_COMPL
                            AND STAMP_ATUALIZACAO = @STAMP_ATUALIZACAo ");

            contextQuery.Parameters.Add("@Cpf", pessoa.Cpf);
            contextQuery.Parameters.Add("@Dt_nasc", TechneDbType.T_DATA, pessoa.Dt_nasc);
            contextQuery.Parameters.Add("@Nome_compl", pessoa.Nome_compl);
            contextQuery.Parameters.Add("@Stamp_atualizacao", SqlDbType.DateTime, dataHoraAtual);

            obj = ctx.GetReturnValue(contextQuery);

            if (obj != null)
            {
                pessoa.Pessoa = Convert.ToDecimal(obj);
            }
        }

        public void InserePessoaPreCadastro(DataContext ctx, int preCadastroAlunoId, string nome, string mae, DateTime dataNascimento, string usuarioResponsavel, string cpf, out decimal pessoa)
        {
            object obj = new Object();
            DateTime dataHoraAtual = DateTime.Now;
            pessoa = 0;

            var contextQuery = new ContextQuery
            {
                Command = @" INSERT INTO LY_PESSOA 
                                    (PESSOA, 
                                     NOME_COMPL, 
                                     E_MAIL, 
                                     DT_NASC, 
                                     RESPONSAVEL, 
                                     NOME_MAE, 
                                     MAE_CPF, 
                                     NOME_PAI, 
                                     PAI_CPF, 
                                     RESP_NOME_COMPL, 
                                     RESP_FONE, 
                                     RESP_CPF, 
                                     CPF, 
                                     SEXO, 
                                     EST_CIVIL, 
                                     ETNIA,
                                     NACIONALIDADE, 
                                     PAIS_NASC,
                                     MUNICIPIO_NASC, 
                                     CELULAR, 
                                     FONE, 
                                     CEP, 
                                     ENDERECO, 
                                     END_NUM, 
                                     END_COMPL, 
                                     BAIRRO, 
                                     END_MUNICIPIO, 
                                     CERT_NUMERO_MATRICULA, 
                                     CERT_NASC_NUM, 
                                     CERT_NASC_FOLHA, 
                                     CERT_NASC_LIVRO, 
                                     NECESSIDADEESPECIALID, 
                                     RG_NUM,
                                     RG_TIPO,
                                     RG_EMISSOR,
                                     RG_UF,
                                     USUARIOID, 
                                     DATACADASTRO, 
                                     DATAALTERACAO) 
                        SELECT (SELECT ISNULL(MAX(PESSOA), 0) + 1 
                                FROM   LY_PESSOA WITH (UPDLOCK)) AS PESSOA,--PESSOA 
                               @NOME,--NOME_COMPL 
                               EMAIL,--E_MAIL 
                               @DATANASCIMENTO,--DT_NASC 
                               RESPONSAVEL,--RESPONSAVEL        
                               @NOMEMAE,--NOME_MAE 
                               MAECPF,--MAE_CPF 
                               NOMEPAI,--NOME_PAI 
                               PAICPF,--PAI_CPF 
                               RESPONSAVELNOME,--RESP_NOME_COMPL 
                               RESPONSAVELFONE,--RESP_FONE 
                               RESPONSAVELCPF,--RESP_CPF 
                               @CPF,--CPF 
                               SEXO,--SEXO 
                               ESTADOCIVIL,--EST_CIVIL 
                               ETNIA,
                               NACIONALIDADE,--NACIONALIDADE , 
							   PAIS_NASC = (CASE WHEN PC.NACIONALIDADE = 'BRASILEIRA' THEN 1
													ELSE NULL END),
                               MUNICIPIONASCIMENTO,--MUNICIPIO_NASC 
                               CELULAR,--CELULAR 
                               FIXOCELULAR,--FONE 
                               CEP,--CEP 
                               ENDERECO,--ENDERECO 
                               NUMEROENDERECO,--END_NUM 
                               COMPLEMENTOENDERECO,--END_COMPL 
                               BAIRRO,--BAIRRO 
                               MUNICIPIOENDERECO,--END_MUNICIPIO 
                               MATRICULACERTIDAO,--CERT_NUMERO_MATRICULA 
                               TERMOCERTIDAO,--CERT_NASC_NUM 
                               FOLHACERTIDAO,--CERT_NASC_FOLHA 
                               LIVROCERTIDAO,--CERT_NASC_LIVRO 
                               NECESSIDADEESPECIALID,--NECESSIDADEESPECIALID  
                               NUMERORG,
                               CASE
                                    WHEN NUMERORG IS NULL THEN NULL
                                    ELSE 'RG'
                               END  RG_TIPO,
                               ORGAORG, 
                               UFRG,   
                               @USUARIOID , 
                               @DATACADASTRO , 
                               @DATAALTERACAO  
                        FROM   MATRICULA.PRECADASTROALUNO PC 
                        WHERE  PC.PRECADASTROALUNOID = @PRECADASTROALUNOID  "
            };

            contextQuery.Parameters.Add("@NOME", TechneDbType.T_ALFALARGE, nome);
            contextQuery.Parameters.Add("@CPF", TechneDbType.T_ALFALARGE, cpf);
            contextQuery.Parameters.Add("@DATANASCIMENTO", TechneDbType.T_DATA, dataNascimento);
            contextQuery.Parameters.Add("@NOMEMAE", TechneDbType.T_ALFALARGE, mae);
            contextQuery.Parameters.Add("@PRECADASTROALUNOID", SqlDbType.Int, preCadastroAlunoId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, dataHoraAtual);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, dataHoraAtual);

            ctx.ApplyModifications(contextQuery);

            contextQuery = new ContextQuery(
             @" SELECT  PESSOA
                    FROM    LY_PESSOA
                    WHERE   DT_NASC = @DATANASCIMENTO
                            AND NOME_COMPL = @NOME
                            AND NOME_MAE = @NOMEMAE
                            AND DATAALTERACAO = @DATAALTERACAO ");

            contextQuery.Parameters.Add("@NOME", TechneDbType.T_ALFALARGE, nome);
            contextQuery.Parameters.Add("@DATANASCIMENTO", TechneDbType.T_DATA, dataNascimento);
            contextQuery.Parameters.Add("@NOMEMAE", TechneDbType.T_ALFALARGE, mae);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, dataHoraAtual);

            obj = ctx.GetReturnValue(contextQuery);

            if (obj != null)
            {
                pessoa = Convert.ToDecimal(obj);
            }
        }

        public void InsereEncaminhamentoEspecial(DataContext ctx, int preCadastroAlunoId, DadosEncaminhamentoEspecial dados, out decimal pessoa)
        {
            object obj = new Object();
            DateTime dataHoraAtual = DateTime.Now;
            pessoa = 0;

            var contextQuery = new ContextQuery
            {
                Command = @" INSERT INTO LY_PESSOA 
                                    (PESSOA, 
                                     NOME_COMPL, 
                                     E_MAIL, 
                                     DT_NASC, 
                                     RESPONSAVEL, 
                                     NOME_MAE, 
                                     MAE_CPF, 
                                     NOME_PAI, 
                                     PAI_CPF, 
                                     RESP_NOME_COMPL, 
                                     RESP_FONE, 
                                     RESP_CPF, 
                                     CPF, 
                                     SEXO, 
                                     EST_CIVIL, 
                                     NACIONALIDADE, 
                                     PAIS_NASC,
                                     MUNICIPIO_NASC, 
                                     CELULAR, 
                                     FONE, 
                                     CEP, 
                                     ENDERECO, 
                                     END_NUM, 
                                     END_COMPL, 
                                     BAIRRO, 
                                     END_MUNICIPIO, 
                                     CERT_NUMERO_MATRICULA, 
                                     CERT_NASC_NUM, 
                                     CERT_NASC_FOLHA, 
                                     CERT_NASC_LIVRO, 
                                     NECESSIDADEESPECIALID, 
                                     USUARIOID, 
                                     DATACADASTRO, 
                                     DATAALTERACAO) 
                        SELECT (SELECT ISNULL(MAX(PESSOA), 0) + 1 
                                FROM   LY_PESSOA WITH (UPDLOCK)) AS PESSOA,--PESSOA 
                               @NOME,--NOME_COMPL 
                               EMAIL,--E_MAIL 
                               @DATANASCIMENTO,--DT_NASC 
                               RESPONSAVEL,--RESPONSAVEL        
                               @NOMEMAE,--NOME_MAE 
                               MAECPF,--MAE_CPF 
                               ISNULL(@NOMEPAI, NOMEPAI),--NOME_PAI 
                               PAICPF,--PAI_CPF 
                               RESPONSAVELNOME,--RESP_NOME_COMPL 
                               RESPONSAVELFONE,--RESP_FONE 
                               RESPONSAVELCPF,--RESP_CPF 
                               @CPF,--CPF 
                               @SEXO,--SEXO 
                               ESTADOCIVIL,--EST_CIVIL 
                               NACIONALIDADE,--NACIONALIDADE , 
							   PAIS_NASC = (CASE WHEN PC.NACIONALIDADE = 'BRASILEIRA' THEN 1
													ELSE NULL END),
                               MUNICIPIONASCIMENTO,--MUNICIPIO_NASC 
                               CELULAR,--CELULAR 
                               FIXOCELULAR,--FONE 
                               @CEP,--CEP 
                               @ENDERECO,--ENDERECO 
                               @NUMERO,--END_NUM 
                               ISNULL(@COMPLEMENTOENDERECO, COMPLEMENTOENDERECO),--END_COMPL 
                               @BAIRRO,--BAIRRO 
                               @MUNICIPIOENDERECO,--END_MUNICIPIO 
                               MATRICULACERTIDAO,--CERT_NUMERO_MATRICULA 
                               TERMOCERTIDAO,--CERT_NASC_NUM 
                               FOLHACERTIDAO,--CERT_NASC_FOLHA 
                               LIVROCERTIDAO,--CERT_NASC_LIVRO 
                               @NECESSIDADEESPECIALID,--NECESSIDADEESPECIALID       
                               @USUARIOID , 
                               @DATACADASTRO , 
                               @DATAALTERACAO  
                        FROM   MATRICULA.PRECADASTROALUNO PC 
                        WHERE  PC.PRECADASTROALUNOID = @PRECADASTROALUNOID  "
            };

            contextQuery.Parameters.Add("@NOME", TechneDbType.T_ALFALARGE, dados.Nome);
            contextQuery.Parameters.Add("@CPF", TechneDbType.T_ALFALARGE, dados.Cpf);
            contextQuery.Parameters.Add("@DATANASCIMENTO", TechneDbType.T_DATA, dados.DataNascimento);
            contextQuery.Parameters.Add("@NOMEMAE", TechneDbType.T_ALFALARGE, dados.NomeMae);
            contextQuery.Parameters.Add("@PRECADASTROALUNOID", SqlDbType.Int, preCadastroAlunoId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dados.UsuarioResponsavel);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, dataHoraAtual);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, dataHoraAtual);
            contextQuery.Parameters.Add("@SEXO", TechneDbType.T_SEXO, dados.Sexo);
            contextQuery.Parameters.Add("@NOMEPAI", TechneDbType.T_ALFALARGE, dados.NomePai);
            contextQuery.Parameters.Add("@CEP", TechneDbType.T_CEP, dados.Cep);
            contextQuery.Parameters.Add("@ENDERECO", TechneDbType.T_ALFAMEDIUM, dados.Endereco);
            contextQuery.Parameters.Add("@NUMERO", TechneDbType.T_ALFASMALL, dados.NumeroEndereco);
            contextQuery.Parameters.Add("@COMPLEMENTOENDERECO", TechneDbType.T_ALFAMEDIUM, dados.ComplementoEndereco);
            contextQuery.Parameters.Add("@MUNICIPIOENDERECO", TechneDbType.T_CODIGO, dados.MunicipioEndereco);
            contextQuery.Parameters.Add("@BAIRRO", TechneDbType.T_ALFAMEDIUM, dados.Bairro);
            contextQuery.Parameters.Add("@NECESSIDADEESPECIALID", SqlDbType.Int, dados.NecessidadeEspecialId);

            ctx.ApplyModifications(contextQuery);

            contextQuery = new ContextQuery(
             @" SELECT  PESSOA
                    FROM    LY_PESSOA
                    WHERE   DT_NASC = @DATANASCIMENTO
                            AND NOME_COMPL = @NOME
                            AND NOME_MAE = @NOMEMAE
                            AND DATAALTERACAO = @DATAALTERACAO ");


            contextQuery.Parameters.Add("@NOME", TechneDbType.T_ALFALARGE, dados.Nome);
            contextQuery.Parameters.Add("@DATANASCIMENTO", TechneDbType.T_DATA, dados.DataNascimento);
            contextQuery.Parameters.Add("@NOMEMAE", TechneDbType.T_ALFALARGE, dados.NomeMae);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, dataHoraAtual);

            obj = ctx.GetReturnValue(contextQuery);

            if (obj != null)
            {
                pessoa = Convert.ToDecimal(obj);
            }
        }

        public void InsereEncaminhamentoEspecial(DataContext ctx, DadosEncaminhamentoEspecial dados, out decimal pessoa)
        {
            object obj = new Object();
            DateTime dataHoraAtual = DateTime.Now;
            pessoa = 0;

            var contextQuery = new ContextQuery
            {
                Command = @" INSERT INTO LY_PESSOA 
                                    (PESSOA, 
                                     NOME_COMPL, 
                                     DT_NASC, 
                                     NOME_MAE, 
                                     NOME_PAI, 
                                     CPF, 
                                     SEXO, 
                                     CEP, 
                                     ENDERECO, 
                                     END_NUM, 
                                     END_COMPL, 
                                     BAIRRO, 
                                     END_MUNICIPIO, 
                                     NECESSIDADEESPECIALID, 
                                     USUARIOID, 
                                     DATACADASTRO, 
                                     DATAALTERACAO) 
                        VALUES( (SELECT ISNULL(MAX(PESSOA), 0) + 1 
                                FROM   LY_PESSOA WITH (UPDLOCK)), 
                               @NOME,
                               @DATANASCIMENTO,
                               @NOMEMAE,
                               @NOMEPAI,
                               @CPF,
                               @SEXO,
                               @CEP,
                               @ENDERECO,
                               @NUMEROENDERECO,
                               @COMPLEMENTOENDERECO,
                               @BAIRRO,
                               @MUNICIPIOENDERECO,
                               @NECESSIDADEESPECIALID,      
                               @USUARIOID, 
                               @DATACADASTRO , 
                               @DATAALTERACAO  ) "
            };

            contextQuery.Parameters.Add("@NOME", TechneDbType.T_ALFALARGE, dados.Nome);
            contextQuery.Parameters.Add("@DATANASCIMENTO", TechneDbType.T_DATA, dados.DataNascimento);
            contextQuery.Parameters.Add("@NOMEMAE", TechneDbType.T_ALFALARGE, dados.NomeMae);
            contextQuery.Parameters.Add("@NOMEPAI", TechneDbType.T_ALFALARGE, dados.NomePai);
            contextQuery.Parameters.Add("@CPF", TechneDbType.T_ALFALARGE, dados.Cpf);
            contextQuery.Parameters.Add("@SEXO", TechneDbType.T_SEXO, dados.Sexo);
            contextQuery.Parameters.Add("@CEP", TechneDbType.T_CEP, dados.Cep);
            contextQuery.Parameters.Add("@ENDERECO", TechneDbType.T_ALFAMEDIUM, dados.Endereco);
            contextQuery.Parameters.Add("@NUMEROENDERECO", TechneDbType.T_ALFASMALL, dados.NumeroEndereco);
            contextQuery.Parameters.Add("@COMPLEMENTOENDERECO", TechneDbType.T_ALFAMEDIUM, dados.ComplementoEndereco);
            contextQuery.Parameters.Add("@MUNICIPIOENDERECO", TechneDbType.T_CODIGO, dados.MunicipioEndereco);
            contextQuery.Parameters.Add("@BAIRRO", TechneDbType.T_ALFAMEDIUM, dados.Bairro);
            contextQuery.Parameters.Add("@NECESSIDADEESPECIALID", SqlDbType.Int, dados.NecessidadeEspecialId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dados.UsuarioResponsavel);
            contextQuery.Parameters.Add("@DATACADASTRO", SqlDbType.DateTime, dataHoraAtual);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, dataHoraAtual);

            ctx.ApplyModifications(contextQuery);

            contextQuery = new ContextQuery(
             @" SELECT  PESSOA
                    FROM    LY_PESSOA
                    WHERE   DT_NASC = @DATANASCIMENTO
                            AND NOME_COMPL = @NOME
                            AND NOME_MAE = @NOMEMAE
                            AND DATAALTERACAO = @DATAALTERACAO ");


            contextQuery.Parameters.Add("@NOME", TechneDbType.T_ALFALARGE, dados.Nome);
            contextQuery.Parameters.Add("@DATANASCIMENTO", TechneDbType.T_DATA, dados.DataNascimento);
            contextQuery.Parameters.Add("@NOMEMAE", TechneDbType.T_ALFALARGE, dados.NomeMae);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, dataHoraAtual);

            obj = ctx.GetReturnValue(contextQuery);

            if (obj != null)
            {
                pessoa = Convert.ToDecimal(obj);
                dados.Pessoa = pessoa;
            }
        }

        public void AtualizaPessoaPreCadastro(DataContext contexto, int preCadastroAlunoId, string usuarioResponsavel, string cpf)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE P 
                                        SET    E_MAIL = PC.EMAIL, 
                                               NOME_COMPL = PC.NOME,
                                               DT_NASC = PC.DATANASCIMENTO,  
                                               NOME_MAE = PC.NOMEMAE,     
                                               RESPONSAVEL = PC.RESPONSAVEL, 
                                               MAE_CPF = PC.MAECPF, 
                                               NOME_PAI = PC.NOMEPAI, 
                                               PAI_CPF = PC.PAICPF, 
                                               RESP_NOME_COMPL = PC.RESPONSAVELNOME, 
                                               RESP_FONE = PC.RESPONSAVELFONE, 
                                               RESP_CPF = PC.RESPONSAVELCPF, 
                                               CPF = ISNULL(@CPF, P.CPF), 
                                               SEXO = PC.SEXO, 
                                               EST_CIVIL = PC.ESTADOCIVIL,
                                               ETNIA = PC.ETNIA, 
                                               NACIONALIDADE = PC.NACIONALIDADE, 
											   PAIS_NASC = ( CASE WHEN PC.NACIONALIDADE = 'BRASILEIRA' THEN 1
																	ELSE NULL END), 
                                               MUNICIPIO_NASC = PC.MUNICIPIONASCIMENTO, 
                                               CELULAR = PC.CELULAR, 
                                               FONE = PC.FIXOCELULAR, 
                                               CEP = PC.CEP, 
                                               ENDERECO = PC.ENDERECO, 
                                               END_NUM = PC.NUMEROENDERECO, 
                                               END_COMPL = PC.COMPLEMENTOENDERECO, 
                                               BAIRRO = ISNULL(PC.BAIRRO, P.BAIRRO), 
                                               END_MUNICIPIO = PC.MUNICIPIOENDERECO, 
                                               CERT_NUMERO_MATRICULA = PC.MATRICULACERTIDAO, 
                                               CERT_NASC_NUM = PC.TERMOCERTIDAO, 
                                               CERT_NASC_FOLHA = PC.FOLHACERTIDAO, 
                                               CERT_NASC_LIVRO = PC.LIVROCERTIDAO, 
                                               NECESSIDADEESPECIALID = PC.NECESSIDADEESPECIALID, 
											   RG_NUM = PC.NUMERORG,
											   RG_TIPO = CASE WHEN NUMERORG IS NULL THEN NULL ELSE 'RG' END, 
											   RG_EMISSOR = ORGAORG,
											   RG_UF = UFRG,
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        FROM   MATRICULA.PRECADASTROALUNO PC 
                                               INNER JOIN LY_PESSOA P 
                                                       ON PC.PESSOAID = P.PESSOA 
                                        WHERE  PC.PRECADASTROALUNOID = @PRECADASTROALUNOID  ";

            contextQuery.Parameters.Add("@PRECADASTROALUNOID", SqlDbType.Int, preCadastroAlunoId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, usuarioResponsavel);
            contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, cpf);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaEncaminhamentoEspecial(DataContext contexto, DadosEncaminhamentoEspecial dados)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_PESSOA 
                                        SET    NOME_COMPL = @NOME,
                                               DT_NASC = @DATANASCIMENTO,  
                                               NOME_MAE = @NOMEMAE,
                                               NOME_PAI = ISNULL(@NOMEPAI, NOME_PAI),
                                               CPF = @CPF, 
                                               SEXO = @SEXO, 
                                               CEP = @CEP, 
                                               ENDERECO = @ENDERECO, 
                                               END_NUM = @NUMEROENDERECO, 
                                               END_COMPL = ISNULL(@COMPLEMENTOENDERECO, END_COMPL), 
                                               BAIRRO = @BAIRRO, 
                                               END_MUNICIPIO = @MUNICIPIOENDERECO,          
                                               NECESSIDADEESPECIALID = @NECESSIDADEESPECIALID, 
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO 
                                        WHERE  PESSOA = @PESSOA  ";

            contextQuery.Parameters.Add("@NOME", TechneDbType.T_ALFALARGE, dados.Nome);
            contextQuery.Parameters.Add("@DATANASCIMENTO", TechneDbType.T_DATA, dados.DataNascimento);
            contextQuery.Parameters.Add("@NOMEMAE", TechneDbType.T_ALFALARGE, dados.NomeMae);
            contextQuery.Parameters.Add("@NOMEPAI", TechneDbType.T_ALFALARGE, dados.NomePai);
            contextQuery.Parameters.Add("@CPF", TechneDbType.T_ALFALARGE, dados.Cpf);
            contextQuery.Parameters.Add("@SEXO", TechneDbType.T_SEXO, dados.Sexo);
            contextQuery.Parameters.Add("@CEP", TechneDbType.T_CEP, dados.Cep);
            contextQuery.Parameters.Add("@ENDERECO", TechneDbType.T_ALFAMEDIUM, dados.Endereco);
            contextQuery.Parameters.Add("@NUMEROENDERECO", TechneDbType.T_ALFASMALL, dados.NumeroEndereco);
            contextQuery.Parameters.Add("@COMPLEMENTOENDERECO", TechneDbType.T_ALFAMEDIUM, dados.ComplementoEndereco);
            contextQuery.Parameters.Add("@MUNICIPIOENDERECO", TechneDbType.T_CODIGO, dados.MunicipioEndereco);
            contextQuery.Parameters.Add("@BAIRRO", TechneDbType.T_ALFAMEDIUM, dados.Bairro);
            contextQuery.Parameters.Add("@NECESSIDADEESPECIALID", SqlDbType.Int, dados.NecessidadeEspecialId);
            contextQuery.Parameters.Add("@USUARIOID", SqlDbType.VarChar, dados.UsuarioResponsavel);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, dados.Pessoa);

            contexto.ApplyModifications(contextQuery);
        }

        public string ObtemCpfPor(DataContext ctx, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT CPF 
                                    FROM   LY_PESSOA 
                                    WHERE  PESSOA = @PESSOA  ";

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);

            resultado = ctx.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public static LyPessoa ObtemPessoaPorUsuario(string usuario)
        {
            try
            {
                var pessoa = new LyPessoa();

                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"
                        SELECT TOP 1
                            P.*, C.codigo_uf, C.cod_cartorio, C.codigo_municipio, m.NOME AS NOME_MUNICIPIO, ge.EMAIL as E_MAIL_GOOGLE
                        FROM LY_PESSOA P
                            LEFT JOIN CARTORIO C ON c.cod_cartorio = P.id_cartorio
                            LEFT JOIN HADES..TCE_MUNICIPIO m ON p.End_municipio = m.ID_MUNICIPIO
                            LEFT JOIN RecursosHumanos.GOOGLEEDUCATION ge on ge.PESSOA = P.PESSOA
                        WHERE P.PESSOA = (select pessoa from HADES..hd_usuario where usuario = @USUARIO)
                        "
                    };
                    contextQuery.Parameters.Add("@USUARIO", usuario);

                    using (var reader = ctx.GetDataReader(contextQuery))
                    {
                        while (reader.Read())
                        {
                            pessoa.Alist_csm = Convert.ToString(reader["Alist_csm"]);
                            pessoa.Alist_num = Convert.ToString(reader["Alist_num"]);
                            pessoa.Alist_rm = Convert.ToString(reader["Alist_rm"]);
                            pessoa.Alist_serie = Convert.ToString(reader["Alist_serie"]);
                            pessoa.Bairro = Convert.ToString(reader["Bairro"]);
                            pessoa.Celular = Convert.ToString(reader["Celular"]);
                            pessoa.Cep = Convert.ToString(reader["Cep"]);
                            pessoa.Etnia = Convert.ToString(reader["Etnia"]);
                            pessoa.Cpf = Convert.ToString(reader["Cpf"]);
                            pessoa.Cprof_num = Convert.ToString(reader["Cprof_num"]);
                            pessoa.Cprof_serie = Convert.ToString(reader["Cprof_serie"]);
                            pessoa.Cprof_uf = Convert.ToString(reader["Cprof_uf"]);
                            pessoa.Cr_cat = Convert.ToString(reader["Cr_cat"]);
                            pessoa.Cr_csm = Convert.ToString(reader["Cr_csm"]);
                            pessoa.Cr_num = Convert.ToString(reader["Cr_num"]);
                            pessoa.Cr_rm = Convert.ToString(reader["Cr_rm"]);
                            pessoa.Cr_serie = Convert.ToString(reader["Cr_serie"]);
                            pessoa.E_mail = Convert.ToString(reader["E_mail"]);
                            pessoa.E_mail_interno = Convert.ToString(reader["E_mail_interno"]);

                            if (reader["E_mail_google"] != DBNull.Value)
                                pessoa.E_mail_google = Convert.ToString(reader["E_mail_google"]);

                            pessoa.End_compl = Convert.ToString(reader["End_compl"]);
                            pessoa.End_municipio = Convert.ToString(reader["End_municipio"]);
                            pessoa.End_num = Convert.ToString(reader["End_num"]);
                            pessoa.End_pais = Convert.ToString(reader["End_pais"]);
                            pessoa.Endereco = Convert.ToString(reader["Endereco"]);
                            pessoa.Est_civil = Convert.ToString(reader["Est_civil"]);
                            pessoa.Fone = Convert.ToString(reader["Fone"]);
                            pessoa.Id_censo = Convert.ToString(reader["Id_censo"]);
                            pessoa.Municipio_nasc = Convert.ToString(reader["Municipio_nasc"]);
                            pessoa.Nacionalidade = Convert.ToString(reader["Nacionalidade"]);
                            if (reader["NECESSIDADEESPECIALID"] != DBNull.Value)
                            {
                                pessoa.NecessidadeEspecialId = Convert.ToInt32(reader["NECESSIDADEESPECIALID"]);
                            }
                            pessoa.Nome_compl = Convert.ToString(reader["Nome_compl"]);
                            pessoa.Pais_nasc = Convert.ToString(reader["Pais_nasc"]);
                            pessoa.Rg_emissor = Convert.ToString(reader["Rg_emissor"]);
                            pessoa.Rg_num = Convert.ToString(reader["Rg_num"]);
                            pessoa.Rg_tipo = Convert.ToString(reader["Rg_tipo"]);
                            pessoa.Rg_uf = Convert.ToString(reader["Rg_uf"]);
                            pessoa.Sexo = Convert.ToString(reader["Sexo"]);
                            pessoa.Pessoa = Convert.ToDecimal(reader["Pessoa"]);
                            pessoa.Teleitor_num = Convert.ToString(reader["Teleitor_num"]);
                            pessoa.Teleitor_secao = Convert.ToString(reader["Teleitor_secao"]);
                            pessoa.Teleitor_zona = Convert.ToString(reader["Teleitor_zona"]);
                            pessoa.Teleitor_mun = Convert.ToString(reader["Teleitor_mun"]);
                            pessoa.Tipo_Sanguineo = Convert.ToString(reader["TIPO_SANGUINEO"]);
                            pessoa.Credo = Convert.ToString(reader["CREDO"]);
                            pessoa.Etnia = Convert.ToString(reader["ETNIA"]);
                            pessoa.CertNumeroMatricula = Convert.ToString(reader["CERT_NUMERO_MATRICULA"]);
                            pessoa.PreNomeSocial = Convert.ToString(reader["PRE_NOME_SOCIAL"]);
                            pessoa.Nome_social = Convert.ToString(reader["PRE_NOME_SOCIAL"]);
                            pessoa.CertNascNum = Convert.ToString(reader["CERT_NASC_NUM"]);
                            pessoa.CertNascFolha = Convert.ToString(reader["CERT_NASC_FOLHA"]);
                            pessoa.CertNascLivro = Convert.ToString(reader["CERT_NASC_LIVRO"]);
                            pessoa.CertNascCartorioExped = Convert.ToString(reader["CERT_NASC_CARTORIO_EXPED"]);
                            pessoa.CertNascCartorioUf = Convert.ToString(reader["CERT_NASC_CARTORIO_UF"]);
                            pessoa.NomePai = Convert.ToString(reader["NOME_PAI"]);
                            pessoa.NomeMae = Convert.ToString(reader["NOME_MAE"]);
                            pessoa.MaeFalecida = Convert.ToString(reader["MAE_FALECIDA"]);
                            pessoa.PaiFalecido = Convert.ToString(reader["PAI_FALECIDO"]);
                            pessoa.MaeCpf = Convert.ToString(reader["MAE_CPF"]);
                            pessoa.PaiCpf = Convert.ToString(reader["PAI_CPF"]);
                            pessoa.MaeTelefone = Convert.ToString(reader["MAE_TELEFONE"]);
                            pessoa.PaiTelefone = Convert.ToString(reader["PAI_TELEFONE"]);
                            pessoa.Responsavel = Convert.ToString(reader["RESPONSAVEL"]);
                            pessoa.RespNomeCompl = Convert.ToString(reader["RESP_NOME_COMPL"]);
                            pessoa.RespCpf = Convert.ToString(reader["RESP_CPF"]);
                            pessoa.RespFone = Convert.ToString(reader["RESP_FONE"]);
                            pessoa.NomeMunicipio = Convert.ToString(reader["NOME_MUNICIPIO"]);
                            pessoa.Passaporte = Convert.ToString(reader["PASSAPORTE"]);
                            pessoa.Latitude = Convert.ToString(reader["LATITUDE"]);
                            pessoa.Longitude = Convert.ToString(reader["LONGITUDE"]);
                            pessoa.AreaAssentamento = Convert.ToString(reader["AREA_ASSENTAMENTO"]);
                            pessoa.AreaQuilombos = Convert.ToString(reader["AREA_QUILOMBOS"]);
                            pessoa.AreaTradicional = Convert.ToString(reader["AREA_TRADICIONAL"]);
                            pessoa.TerraIndigena = Convert.ToString(reader["TERRA_INDIGENA"]);

                            if (reader["Cprof_dtexp"] != DBNull.Value)
                            {
                                pessoa.Cprof_dtexp = Convert.ToDateTime(reader["Cprof_dtexp"]);
                            }
                            if (reader["Alist_dtexp"] != DBNull.Value)
                            {
                                pessoa.Alist_dtexp = Convert.ToDateTime(reader["Alist_dtexp"]);
                            }
                            if (reader["Cr_dtexp"] != DBNull.Value)
                            {
                                pessoa.Cr_dtexp = Convert.ToDateTime(reader["Cr_dtexp"]);
                            }
                            if (reader["Dt_nasc"] != DBNull.Value)
                            {
                                pessoa.Dt_nasc = Convert.ToDateTime(reader["Dt_nasc"]);
                            }
                            if (reader["Rg_dtexp"] != DBNull.Value)
                            {
                                pessoa.Rg_dtexp = Convert.ToDateTime(reader["Rg_dtexp"]);
                            }
                            if (reader["Teleitor_dtexp"] != DBNull.Value)
                            {
                                pessoa.Teleitor_dtexp = Convert.ToDateTime(reader["Teleitor_dtexp"]);
                            }

                            if (reader["Stamp_atualizacao"] != DBNull.Value)
                            {
                                pessoa.Stamp_atualizacao = Convert.ToDateTime(reader["Stamp_atualizacao"]);
                            }
                            if (reader["ID_CARTORIO"] != DBNull.Value)
                            {
                                pessoa.IdCartorio = Convert.ToInt32(reader["ID_CARTORIO"]);
                            }
                            if (reader["QT_FILHOS"] != DBNull.Value)
                            {
                                pessoa.QtFilhos = Convert.ToDecimal(reader["QT_FILHOS"]);
                            }
                            if (reader["CERT_NASC_EMISSAO"] != DBNull.Value)
                            {
                                pessoa.CertNascEmissao = Convert.ToDateTime(reader["CERT_NASC_EMISSAO"]);
                            }

                            pessoa.CodigoUf = Convert.ToString(reader["codigo_uf"]);
                            pessoa.CodigoMunicipio = Convert.ToString(reader["codigo_municipio"]);
                            if (reader["IdFuncional"] != DBNull.Value)
                            {
                                pessoa.IdFuncional = Convert.ToInt32(reader["IdFuncional"]);
                            }
                            pessoa.Pispasep = Convert.ToString(reader["Pispasep"]);
                            pessoa.UsuarioId = Convert.ToString(reader["UsuarioId"]);
                            if (reader["datacadastro"] != DBNull.Value)
                            {
                                pessoa.DataCadastro = Convert.ToDateTime(reader["datacadastro"]);
                            }
                            if (reader["dataalteracao"] != DBNull.Value)
                            {
                                pessoa.DataAlteracao = Convert.ToDateTime(reader["dataalteracao"]);
                            }


                        }
                        return pessoa;
                    }
                }
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public static LyPessoa Carregar(int idPessoa)
        {
            try
            {
                var pessoa = new LyPessoa();

                using (var ctx = DataContextBuilder.FromLyceum.UsingLock())
                {
                    var contextQuery = new ContextQuery
                    {
                        Command = @"SELECT TOP 1
                                        P.*, C.codigo_uf, C.cod_cartorio, C.codigo_municipio, m.NOME AS NOME_MUNICIPIO, ge.EMAIL as E_MAIL_GOOGLE
                                 FROM LY_PESSOA P
                                        LEFT JOIN CARTORIO C ON c.cod_cartorio = P.id_cartorio
                                        LEFT JOIN HADES..TCE_MUNICIPIO m ON p.End_municipio = m.ID_MUNICIPIO
                                        LEFT JOIN RecursosHumanos.GOOGLEEDUCATION ge on ge.PESSOA = P.PESSOA
                                  WHERE P.PESSOA = @Pessoa "
                    };
                    contextQuery.Parameters.Add("@Pessoa", idPessoa);

                    using (var reader = ctx.GetDataReader(contextQuery))
                    {
                        while (reader.Read())
                        {
                            pessoa.Alist_csm = Convert.ToString(reader["Alist_csm"]);
                            pessoa.Alist_num = Convert.ToString(reader["Alist_num"]);
                            pessoa.Alist_rm = Convert.ToString(reader["Alist_rm"]);
                            pessoa.Alist_serie = Convert.ToString(reader["Alist_serie"]);
                            pessoa.Bairro = Convert.ToString(reader["Bairro"]);
                            pessoa.Celular = Convert.ToString(reader["Celular"]);
                            pessoa.Cep = Convert.ToString(reader["Cep"]);
                            pessoa.Etnia = Convert.ToString(reader["Etnia"]);
                            pessoa.Cpf = Convert.ToString(reader["Cpf"]);
                            pessoa.Cprof_num = Convert.ToString(reader["Cprof_num"]);
                            pessoa.Cprof_serie = Convert.ToString(reader["Cprof_serie"]);
                            pessoa.Cprof_uf = Convert.ToString(reader["Cprof_uf"]);
                            pessoa.Cr_cat = Convert.ToString(reader["Cr_cat"]);
                            pessoa.Cr_csm = Convert.ToString(reader["Cr_csm"]);
                            pessoa.Cr_num = Convert.ToString(reader["Cr_num"]);
                            pessoa.Cr_rm = Convert.ToString(reader["Cr_rm"]);
                            pessoa.Cr_serie = Convert.ToString(reader["Cr_serie"]);
                            pessoa.E_mail = Convert.ToString(reader["E_mail"]);
                            pessoa.E_mail_interno = Convert.ToString(reader["E_mail_interno"]);

                            if (reader["E_mail_google"] != DBNull.Value)
                                pessoa.E_mail_google = Convert.ToString(reader["E_mail_google"]);

                            pessoa.End_compl = Convert.ToString(reader["End_compl"]);
                            pessoa.End_municipio = Convert.ToString(reader["End_municipio"]);
                            pessoa.End_num = Convert.ToString(reader["End_num"]);
                            pessoa.End_pais = Convert.ToString(reader["End_pais"]);
                            pessoa.Endereco = Convert.ToString(reader["Endereco"]);
                            pessoa.Est_civil = Convert.ToString(reader["Est_civil"]);
                            pessoa.Fone = Convert.ToString(reader["Fone"]);
                            pessoa.Id_censo = Convert.ToString(reader["Id_censo"]);
                            pessoa.Municipio_nasc = Convert.ToString(reader["Municipio_nasc"]);
                            pessoa.Nacionalidade = Convert.ToString(reader["Nacionalidade"]);
                            if (reader["NECESSIDADEESPECIALID"] != DBNull.Value)
                            {
                                pessoa.NecessidadeEspecialId = Convert.ToInt32(reader["NECESSIDADEESPECIALID"]);
                            }
                            pessoa.Nome_compl = Convert.ToString(reader["Nome_compl"]);
                            pessoa.Pais_nasc = Convert.ToString(reader["Pais_nasc"]);
                            pessoa.Rg_emissor = Convert.ToString(reader["Rg_emissor"]);
                            pessoa.Rg_num = Convert.ToString(reader["Rg_num"]);
                            pessoa.Rg_tipo = Convert.ToString(reader["Rg_tipo"]);
                            pessoa.Rg_uf = Convert.ToString(reader["Rg_uf"]);
                            pessoa.Sexo = Convert.ToString(reader["Sexo"]);
                            pessoa.Pessoa = Convert.ToDecimal(reader["Pessoa"]);
                            pessoa.Teleitor_num = Convert.ToString(reader["Teleitor_num"]);
                            pessoa.Teleitor_secao = Convert.ToString(reader["Teleitor_secao"]);
                            pessoa.Teleitor_zona = Convert.ToString(reader["Teleitor_zona"]);
                            pessoa.Teleitor_mun = Convert.ToString(reader["Teleitor_mun"]);
                            pessoa.Tipo_Sanguineo = Convert.ToString(reader["TIPO_SANGUINEO"]);
                            pessoa.Credo = Convert.ToString(reader["CREDO"]);
                            pessoa.Etnia = Convert.ToString(reader["ETNIA"]);
                            pessoa.CertNumeroMatricula = Convert.ToString(reader["CERT_NUMERO_MATRICULA"]);
                            pessoa.PreNomeSocial = Convert.ToString(reader["PRE_NOME_SOCIAL"]);
                            pessoa.Nome_social = Convert.ToString(reader["PRE_NOME_SOCIAL"]);
                            pessoa.CertNascNum = Convert.ToString(reader["CERT_NASC_NUM"]);
                            pessoa.CertNascFolha = Convert.ToString(reader["CERT_NASC_FOLHA"]);
                            pessoa.CertNascLivro = Convert.ToString(reader["CERT_NASC_LIVRO"]);
                            pessoa.CertNascCartorioExped = Convert.ToString(reader["CERT_NASC_CARTORIO_EXPED"]);
                            pessoa.CertNascCartorioUf = Convert.ToString(reader["CERT_NASC_CARTORIO_UF"]);
                            pessoa.NomePai = Convert.ToString(reader["NOME_PAI"]);
                            pessoa.NomeMae = Convert.ToString(reader["NOME_MAE"]);
                            pessoa.MaeFalecida = Convert.ToString(reader["MAE_FALECIDA"]);
                            pessoa.PaiFalecido = Convert.ToString(reader["PAI_FALECIDO"]);
                            pessoa.MaeCpf = Convert.ToString(reader["MAE_CPF"]);
                            pessoa.PaiCpf = Convert.ToString(reader["PAI_CPF"]);
                            pessoa.MaeTelefone = Convert.ToString(reader["MAE_TELEFONE"]);
                            pessoa.PaiTelefone = Convert.ToString(reader["PAI_TELEFONE"]);
                            pessoa.Responsavel = Convert.ToString(reader["RESPONSAVEL"]);
                            pessoa.RespNomeCompl = Convert.ToString(reader["RESP_NOME_COMPL"]);
                            pessoa.RespCpf = Convert.ToString(reader["RESP_CPF"]);
                            pessoa.RespFone = Convert.ToString(reader["RESP_FONE"]);
                            pessoa.NomeMunicipio = Convert.ToString(reader["NOME_MUNICIPIO"]);
                            pessoa.Passaporte = Convert.ToString(reader["PASSAPORTE"]);
                            pessoa.Latitude = Convert.ToString(reader["LATITUDE"]);
                            pessoa.Longitude = Convert.ToString(reader["LONGITUDE"]);
                            pessoa.AreaAssentamento = Convert.ToString(reader["AREA_ASSENTAMENTO"]);
                            pessoa.AreaQuilombos = Convert.ToString(reader["AREA_QUILOMBOS"]);
                            pessoa.AreaTradicional = Convert.ToString(reader["AREA_TRADICIONAL"]);
                            pessoa.TerraIndigena = Convert.ToString(reader["TERRA_INDIGENA"]);

                            if (reader["Cprof_dtexp"] != DBNull.Value)
                            {
                                pessoa.Cprof_dtexp = Convert.ToDateTime(reader["Cprof_dtexp"]);
                            }
                            if (reader["Alist_dtexp"] != DBNull.Value)
                            {
                                pessoa.Alist_dtexp = Convert.ToDateTime(reader["Alist_dtexp"]);
                            }
                            if (reader["Cr_dtexp"] != DBNull.Value)
                            {
                                pessoa.Cr_dtexp = Convert.ToDateTime(reader["Cr_dtexp"]);
                            }
                            if (reader["Dt_nasc"] != DBNull.Value)
                            {
                                pessoa.Dt_nasc = Convert.ToDateTime(reader["Dt_nasc"]);
                            }
                            if (reader["Rg_dtexp"] != DBNull.Value)
                            {
                                pessoa.Rg_dtexp = Convert.ToDateTime(reader["Rg_dtexp"]);
                            }
                            if (reader["Teleitor_dtexp"] != DBNull.Value)
                            {
                                pessoa.Teleitor_dtexp = Convert.ToDateTime(reader["Teleitor_dtexp"]);
                            }

                            if (reader["Stamp_atualizacao"] != DBNull.Value)
                            {
                                pessoa.Stamp_atualizacao = Convert.ToDateTime(reader["Stamp_atualizacao"]);
                            }
                            if (reader["ID_CARTORIO"] != DBNull.Value)
                            {
                                pessoa.IdCartorio = Convert.ToInt32(reader["ID_CARTORIO"]);
                            }
                            if (reader["QT_FILHOS"] != DBNull.Value)
                            {
                                pessoa.QtFilhos = Convert.ToDecimal(reader["QT_FILHOS"]);
                            }
                            if (reader["CERT_NASC_EMISSAO"] != DBNull.Value)
                            {
                                pessoa.CertNascEmissao = Convert.ToDateTime(reader["CERT_NASC_EMISSAO"]);
                            }

                            pessoa.CodigoUf = Convert.ToString(reader["codigo_uf"]);
                            pessoa.CodigoMunicipio = Convert.ToString(reader["codigo_municipio"]);
                            if (reader["IdFuncional"] != DBNull.Value)
                            {
                                pessoa.IdFuncional = Convert.ToInt32(reader["IdFuncional"]);
                            }
                            pessoa.Pispasep = Convert.ToString(reader["Pispasep"]);
                            pessoa.UsuarioId = Convert.ToString(reader["UsuarioId"]);
                            if (reader["datacadastro"] != DBNull.Value)
                            {
                                pessoa.DataCadastro = Convert.ToDateTime(reader["datacadastro"]);
                            }
                            if (reader["dataalteracao"] != DBNull.Value)
                            {
                                pessoa.DataAlteracao = Convert.ToDateTime(reader["dataalteracao"]);
                            }


                        }
                        return pessoa;
                    }
                }
            }

            catch (Exception e)
            {
                throw e;
            }
        }

        private void AtualizaDadosIdentificacao(DataContext contexto, string nome, string nomeMae, DateTime dataNascimento, decimal pessoa, string usuarioResponsavel)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_PESSOA 
                        SET  NOME_COMPL = @NOME_COMPL, 
                             DT_NASC = @DT_NASC, 
                             NOME_MAE = @NOME_MAE,
                             STAMP_ATUALIZACAO = @STAMP_ATUALIZACAO, 
                             USUARIOID = @USUARIOID, 
                             DATAALTERACAO = @DATAALTERACAO 
                    WHERE  PESSOA = @PESSOA  ";

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);
            contextQuery.Parameters.Add("@NOME_COMPL", nome);
            contextQuery.Parameters.Add("@NOME_MAE", nomeMae);
            contextQuery.Parameters.Add("@DT_NASC", TechneDbType.T_DATA, dataNascimento);
            contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@USUARIOID", usuarioResponsavel);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaDadosPessoaisServidor(DataContext contexto, LyPessoa pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_PESSOA 
                        SET  NOME_COMPL = @NOME_COMPL, 
                             PRE_NOME_SOCIAL = @NOME_SOCIAL,   
                             DT_NASC = @DT_NASC, 
                             SEXO = @SEXO, 
                             ETNIA = @ETNIA,
                             NECESSIDADEESPECIALID = @NECESSIDADEESPECIALID, 
                             EST_CIVIL = @EST_CIVIL, 
                             PAIS_NASC = @PAIS_NASC,
                             NACIONALIDADE = @NACIONALIDADE, 
                             MUNICIPIO_NASC = @MUNICIPIO_NASC, 
                             NOME_PAI = @NOME_PAI, 
                             NOME_MAE = @NOME_MAE, 
                             END_PAIS = @END_PAIS,
                             CEP = @CEP, 
                             END_MUNICIPIO = @END_MUNICIPIO,                              
                             ENDERECO = @ENDERECO, 
                             END_NUM = @END_NUM, 
                             END_COMPL = @END_COMPL, 
                             BAIRRO = @BAIRRO,  
                             AREA_ASSENTAMENTO = @AREA_ASSENTAMENTO, 
                             TERRA_INDIGENA = @TERRA_INDIGENA, 
                             AREA_QUILOMBOS = @AREA_QUILOMBOS,
                             FONE = @FONE, 
                             CELULAR = @CELULAR, 
                             E_MAIL = @E_MAIL, 
                             STAMP_ATUALIZACAO = @STAMP_ATUALIZACAO,                              
                             USUARIOID = @USUARIOID, 
                             DATAALTERACAO = @DATAALTERACAO 
                    WHERE  PESSOA = @PESSOA  ";

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa.Pessoa);
            contextQuery.Parameters.Add("@NOME_COMPL", pessoa.Nome_compl);
            contextQuery.Parameters.Add("@NOME_SOCIAL", pessoa.Nome_social);
            contextQuery.Parameters.Add("@DT_NASC", TechneDbType.T_DATA, pessoa.Dt_nasc);
            contextQuery.Parameters.Add("@SEXO", pessoa.Sexo);
            contextQuery.Parameters.Add("@ETNIA", pessoa.Etnia);
            contextQuery.Parameters.Add("@NECESSIDADEESPECIALID", pessoa.NecessidadeEspecialId);
            contextQuery.Parameters.Add("@EST_CIVIL", pessoa.Est_civil);
            contextQuery.Parameters.Add("@PAIS_NASC", pessoa.Pais_nasc); 
            contextQuery.Parameters.Add("@NACIONALIDADE", pessoa.Nacionalidade);
            contextQuery.Parameters.Add("@MUNICIPIO_NASC", pessoa.Municipio_nasc);
            contextQuery.Parameters.Add("@NOME_MAE", pessoa.NomeMae.IsNullOrEmptyOrWhiteSpace() ? null : pessoa.NomeMae.ToUpper());
            contextQuery.Parameters.Add("@NOME_PAI", pessoa.NomePai.IsNullOrEmptyOrWhiteSpace() ? null : pessoa.NomePai.ToUpper());
            contextQuery.Parameters.Add("@END_PAIS", pessoa.End_pais);
            contextQuery.Parameters.Add("@CEP", pessoa.Cep);
            contextQuery.Parameters.Add("@END_MUNICIPIO", pessoa.End_municipio);
            contextQuery.Parameters.Add("@ENDERECO", pessoa.Endereco);
            contextQuery.Parameters.Add("@END_NUM", pessoa.End_num);
            contextQuery.Parameters.Add("@END_COMPL", pessoa.End_compl);
            contextQuery.Parameters.Add("@BAIRRO", TechneDbType.T_ALFAMEDIUM, pessoa.Bairro);
            contextQuery.Parameters.Add("@AREA_ASSENTAMENTO", pessoa.AreaAssentamento);
            contextQuery.Parameters.Add("@TERRA_INDIGENA", pessoa.TerraIndigena);
            contextQuery.Parameters.Add("@AREA_QUILOMBOS", pessoa.AreaQuilombos);
            contextQuery.Parameters.Add("@FONE", pessoa.Fone);
            contextQuery.Parameters.Add("@CELULAR", TechneDbType.T_TELEFONE, pessoa.Celular);
            contextQuery.Parameters.Add("@E_MAIL", pessoa.E_mail);
            contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@USUARIOID", pessoa.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaPessoaServidor(DataContext contexto, LyPessoa pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_PESSOA 
                        SET  IDFUNCIONAL = @IDFUNCIONAL,   
                             NOME_COMPL = @NOME_COMPL, 
                             PRE_NOME_SOCIAL = @NOME_SOCIAL,   
                             DT_NASC = @DT_NASC, 
                             SEXO = @SEXO, 
                             EST_CIVIL = @EST_CIVIL, 
                             NECESSIDADEESPECIALID = @NECESSIDADEESPECIALID, 
                             ETNIA = @ETNIA,
                             NACIONALIDADE = @NACIONALIDADE, 
                             PAIS_NASC = @PAIS_NASC, 
                             ENDERECO = @ENDERECO, 
                             END_MUNICIPIO = @END_MUNICIPIO, 
                             END_NUM = @END_NUM, 
                             CEP = @CEP, 
                             BAIRRO = @BAIRRO, 
                             END_PAIS = @END_PAIS, 
                             FONE = @FONE, 
                             CELULAR = @CELULAR, 
                             E_MAIL = @E_MAIL, 
                             E_MAIL_INTERNO = @E_MAIL_INTERNO, 
                             MUNICIPIO_NASC = @MUNICIPIO_NASC, 
                             RG_TIPO = @RG_TIPO, 
                             RG_NUM = @RG_NUM, 
                             RG_UF = @RG_UF, 
                             RG_EMISSOR = @RG_EMISSOR, 
                             RG_DTEXP = @RG_DTEXP, 
                             CPF = @CPF, 
                             PASSAPORTE = @PASSAPORTE, 
                             CERT_NASC_NUM = @CERT_NASC_NUM,
                             CERT_NASC_FOLHA = @CERT_NASC_FOLHA,
                             CERT_NASC_LIVRO = @CERT_NASC_LIVRO,
                             CERT_NASC_CARTORIO_EXPED = @CERT_NASC_CARTORIO_EXPED,
                             CERT_NASC_EMISSAO = @CERT_NASC_EMISSAO,
                             CERT_NASC_CARTORIO_UF = @CERT_NASC_CARTORIO_UF,
                             ID_CARTORIO = @ID_CARTORIO,
                             CERT_NUMERO_MATRICULA = @CERT_NUMERO_MATRICULA,
                             CPROF_DTEXP = @CPROF_DTEXP, 
                             CPROF_NUM = @CPROF_NUM, 
                             CPROF_SERIE = @CPROF_SERIE, 
                             CPROF_UF = @CPROF_UF, 
                             TELEITOR_DTEXP = @TELEITOR_DTEXP, 
                             TELEITOR_NUM = @TELEITOR_NUM, 
                             TELEITOR_SECAO = @TELEITOR_SECAO, 
                             TELEITOR_ZONA = @TELEITOR_ZONA, 
                             TELEITOR_MUN = @TELEITOR_MUN, 
                             ALIST_CSM = @ALIST_CSM, 
                             ALIST_DTEXP = @ALIST_DTEXP, 
                             ALIST_NUM = @ALIST_NUM, 
                             ALIST_RM = @ALIST_RM, 
                             ALIST_SERIE = @ALIST_SERIE, 
                             CR_CAT = @CR_CAT, 
                             CR_CSM = @CR_CSM, 
                             CR_DTEXP = @CR_DTEXP, 
                             CR_NUM = @CR_NUM, 
                             CR_RM = @CR_RM, 
                             CR_SERIE = @CR_SERIE, 
                             END_COMPL = @END_COMPL, 
                             PISPASEP = @PISPASEP,
                             STAMP_ATUALIZACAO = @STAMP_ATUALIZACAO, 
                             AREA_ASSENTAMENTO = @AREA_ASSENTAMENTO, 
                             TERRA_INDIGENA = @TERRA_INDIGENA, 
                             AREA_QUILOMBOS = @AREA_QUILOMBOS,
                             AREA_TRADICIONAL = @AREA_TRADICIONAL,
                             NOME_PAI = @NOME_PAI, 
                             NOME_MAE = @NOME_MAE, 
                             USUARIOID = @USUARIOID, 
                             DATAALTERACAO = @DATAALTERACAO 
                    WHERE  PESSOA = @PESSOA  ";

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa.Pessoa);
            contextQuery.Parameters.Add("@IDFUNCIONAL", pessoa.IdFuncional);
            contextQuery.Parameters.Add("@NOME_COMPL", pessoa.Nome_compl);
            contextQuery.Parameters.Add("@NOME_SOCIAL", pessoa.Nome_social);
            contextQuery.Parameters.Add("@DT_NASC", TechneDbType.T_DATA, pessoa.Dt_nasc);
            contextQuery.Parameters.Add("@SEXO", pessoa.Sexo);
            contextQuery.Parameters.Add("@EST_CIVIL", pessoa.Est_civil);
            contextQuery.Parameters.Add("@NECESSIDADEESPECIALID", pessoa.NecessidadeEspecialId);
            contextQuery.Parameters.Add("@ETNIA", pessoa.Etnia);
            contextQuery.Parameters.Add("@NACIONALIDADE", pessoa.Nacionalidade);
            contextQuery.Parameters.Add("@PAIS_NASC", pessoa.Pais_nasc);
            contextQuery.Parameters.Add("@ENDERECO", pessoa.Endereco);
            contextQuery.Parameters.Add("@END_NUM", pessoa.End_num);
            contextQuery.Parameters.Add("@END_COMPL", pessoa.End_compl);
            contextQuery.Parameters.Add("@CEP", pessoa.Cep);
            contextQuery.Parameters.Add("@BAIRRO", TechneDbType.T_ALFAMEDIUM, pessoa.Bairro);
            contextQuery.Parameters.Add("@END_MUNICIPIO", pessoa.End_municipio);
            contextQuery.Parameters.Add("@END_PAIS", pessoa.End_pais);
            contextQuery.Parameters.Add("@FONE", pessoa.Fone);
            contextQuery.Parameters.Add("@CELULAR", TechneDbType.T_TELEFONE, pessoa.Celular);
            contextQuery.Parameters.Add("@E_MAIL", pessoa.E_mail);
            contextQuery.Parameters.Add("@E_MAIL_INTERNO", pessoa.E_mail_interno);
            contextQuery.Parameters.Add("@MUNICIPIO_NASC", pessoa.Municipio_nasc);
            contextQuery.Parameters.Add("@RG_DTEXP", TechneDbType.T_DATA, pessoa.Rg_dtexp);
            contextQuery.Parameters.Add("@RG_EMISSOR", pessoa.Rg_emissor);
            contextQuery.Parameters.Add("@RG_NUM", pessoa.Rg_num);
            contextQuery.Parameters.Add("@RG_TIPO", pessoa.Rg_tipo);
            contextQuery.Parameters.Add("@RG_UF", pessoa.Rg_uf);
            contextQuery.Parameters.Add("@CPF", pessoa.Cpf);
            contextQuery.Parameters.Add("@PISPASEP", pessoa.Pispasep);
            contextQuery.Parameters.Add("@PASSAPORTE", pessoa.Passaporte);
            contextQuery.Parameters.Add("@CERT_NASC_NUM", pessoa.CertNascNum);
            contextQuery.Parameters.Add("@CERT_NASC_FOLHA", pessoa.CertNascFolha);
            contextQuery.Parameters.Add("@CERT_NASC_LIVRO", pessoa.CertNascLivro);
            contextQuery.Parameters.Add("@CERT_NASC_EMISSAO", TechneDbType.T_DATA, pessoa.CertNascEmissao);
            contextQuery.Parameters.Add("@CERT_NASC_CARTORIO_UF", pessoa.CertNascCartorioUf);
            contextQuery.Parameters.Add("@CERT_NASC_CARTORIO_EXPED", pessoa.CertNascCartorioExped);
            contextQuery.Parameters.Add("@CERT_NUMERO_MATRICULA", pessoa.CertNumeroMatricula);
            contextQuery.Parameters.Add("@ID_CARTORIO", pessoa.IdCartorio);
            contextQuery.Parameters.Add("@CPROF_DTEXP", TechneDbType.T_DATA, pessoa.Cprof_dtexp);
            contextQuery.Parameters.Add("@CPROF_NUM", pessoa.Cprof_num);
            contextQuery.Parameters.Add("@CPROF_SERIE", pessoa.Cprof_serie);
            contextQuery.Parameters.Add("@CPROF_UF", pessoa.Cprof_uf);
            contextQuery.Parameters.Add("@TELEITOR_DTEXP", TechneDbType.T_DATA, pessoa.Teleitor_dtexp);
            contextQuery.Parameters.Add("@TELEITOR_NUM", pessoa.Teleitor_num);
            contextQuery.Parameters.Add("@TELEITOR_SECAO", pessoa.Teleitor_secao);
            contextQuery.Parameters.Add("@TELEITOR_ZONA", pessoa.Teleitor_zona);
            contextQuery.Parameters.Add("@TELEITOR_MUN", pessoa.Teleitor_mun);
            contextQuery.Parameters.Add("@ALIST_CSM", TechneDbType.T_ALFASMALL, pessoa.Alist_csm);
            contextQuery.Parameters.Add("@ALIST_DTEXP", TechneDbType.T_DATA, pessoa.Alist_dtexp);
            contextQuery.Parameters.Add("@ALIST_NUM", TechneDbType.T_ALFASMALL_17, pessoa.Alist_num);
            contextQuery.Parameters.Add("@ALIST_RM", TechneDbType.T_ALFASMALL, pessoa.Alist_rm);
            contextQuery.Parameters.Add("@ALIST_SERIE", TechneDbType.T_ALFASMALL, pessoa.Alist_serie);
            contextQuery.Parameters.Add("@CR_CAT", pessoa.Cr_cat);
            contextQuery.Parameters.Add("@CR_CSM", pessoa.Cr_csm);
            contextQuery.Parameters.Add("@CR_DTEXP", TechneDbType.T_DATA, pessoa.Cr_dtexp);
            contextQuery.Parameters.Add("@CR_NUM", pessoa.Cr_num);
            contextQuery.Parameters.Add("@CR_RM", pessoa.Cr_rm);
            contextQuery.Parameters.Add("@CR_SERIE", pessoa.Cr_serie);
            contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@AREA_ASSENTAMENTO", pessoa.AreaAssentamento);
            contextQuery.Parameters.Add("@TERRA_INDIGENA", pessoa.TerraIndigena);
            contextQuery.Parameters.Add("@AREA_QUILOMBOS", pessoa.AreaQuilombos);
            contextQuery.Parameters.Add("@AREA_TRADICIONAL", pessoa.AreaTradicional);
            contextQuery.Parameters.Add("@NOME_MAE", pessoa.NomeMae.IsNullOrEmptyOrWhiteSpace() ? null : pessoa.NomeMae.ToUpper());
            contextQuery.Parameters.Add("@NOME_PAI", pessoa.NomePai.IsNullOrEmptyOrWhiteSpace() ? null : pessoa.NomePai.ToUpper());
            contextQuery.Parameters.Add("@USUARIOID", pessoa.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaPessoaDocente(DataContext contexto, LyPessoa pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_PESSOA 
                    SET    ALIST_CSM = @ALIST_CSM, 
                           ALIST_DTEXP = @ALIST_DTEXP, 
                           ALIST_NUM = @ALIST_NUM, 
                           ALIST_RM = @ALIST_RM, 
                           ALIST_SERIE = @ALIST_SERIE, 
                           BAIRRO = @BAIRRO, 
                           CELULAR = @CELULAR, 
                           CEP = @CEP, 
                           CPF = @CPF, 
                           CPROF_DTEXP = @CPROF_DTEXP, 
                           CPROF_NUM = @CPROF_NUM, 
                           CPROF_SERIE = @CPROF_SERIE, 
                           CPROF_UF = @CPROF_UF, 
                           CR_CAT = @CR_CAT, 
                           CR_CSM = @CR_CSM, 
                           CR_DTEXP = @CR_DTEXP, 
                           CR_NUM = @CR_NUM, 
                           CR_RM = @CR_RM, 
                           CR_SERIE = @CR_SERIE, 
                           DT_NASC = @DT_NASC, 
                           E_MAIL = @E_MAIL, 
                           E_MAIL_INTERNO = @E_MAIL_INTERNO, 
                           END_COMPL = @END_COMPL, 
                           END_MUNICIPIO = @END_MUNICIPIO, 
                           END_NUM = @END_NUM, 
                           END_PAIS = @END_PAIS, 
                           ENDERECO = @ENDERECO, 
                           EST_CIVIL = @EST_CIVIL, 
                           FONE = @FONE, 
                           MUNICIPIO_NASC = @MUNICIPIO_NASC, 
                           NACIONALIDADE = @NACIONALIDADE, 
                           NECESSIDADEESPECIALID = @NECESSIDADEESPECIALID, 
                           NOME_COMPL = @NOME_COMPL, 
                           PRE_NOME_SOCIAL = @NOME_SOCIAL, 
                           PAIS_NASC = @PAIS_NASC, 
                           RG_DTEXP = @RG_DTEXP, 
                           RG_EMISSOR = @RG_EMISSOR, 
                           RG_NUM = @RG_NUM, 
                           RG_TIPO = @RG_TIPO, 
                           RG_UF = @RG_UF, 
                           SEXO = @SEXO, 
                           STAMP_ATUALIZACAO = @STAMP_ATUALIZACAO, 
                           TELEITOR_DTEXP = @TELEITOR_DTEXP, 
                           TELEITOR_NUM = @TELEITOR_NUM, 
                           TELEITOR_SECAO = @TELEITOR_SECAO, 
                           TELEITOR_ZONA = @TELEITOR_ZONA, 
                           NOME_PAI = @NOME_PAI, 
                           NOME_MAE = @NOME_MAE, 
                           IDFUNCIONAL = @IDFUNCIONAL, 
                           PISPASEP = @PISPASEP, 
                           TELEITOR_MUN = @TELEITOR_MUN,
                           ETNIA = @ETNIA,
                           AREA_ASSENTAMENTO = @AREA_ASSENTAMENTO, 
                           TERRA_INDIGENA = @TERRA_INDIGENA, 
                           AREA_QUILOMBOS = @AREA_QUILOMBOS,
                           AREA_TRADICIONAL = @AREA_TRADICIONAL,
                           USUARIOID = @USUARIOID, 
                           DATAALTERACAO = @DATAALTERACAO 
                    WHERE  PESSOA = @PESSOA  ";

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa.Pessoa);
            contextQuery.Parameters.Add("@ALIST_CSM", TechneDbType.T_ALFASMALL, pessoa.Alist_csm);
            contextQuery.Parameters.Add("@ALIST_DTEXP", TechneDbType.T_DATA, pessoa.Alist_dtexp);
            contextQuery.Parameters.Add("@ALIST_NUM", TechneDbType.T_ALFASMALL_17, pessoa.Alist_num);
            contextQuery.Parameters.Add("@ALIST_RM", TechneDbType.T_ALFASMALL, pessoa.Alist_rm);
            contextQuery.Parameters.Add("@ALIST_SERIE", TechneDbType.T_ALFASMALL, pessoa.Alist_serie);
            contextQuery.Parameters.Add("@BAIRRO", TechneDbType.T_ALFAMEDIUM, pessoa.Bairro);
            contextQuery.Parameters.Add("@CELULAR", TechneDbType.T_TELEFONE, pessoa.Celular);
            contextQuery.Parameters.Add("@CEP", pessoa.Cep);
            contextQuery.Parameters.Add("@CPF", pessoa.Cpf);
            contextQuery.Parameters.Add("@CPROF_DTEXP", TechneDbType.T_DATA, pessoa.Cprof_dtexp);
            contextQuery.Parameters.Add("@CPROF_NUM", pessoa.Cprof_num);
            contextQuery.Parameters.Add("@CPROF_SERIE", pessoa.Cprof_serie);
            contextQuery.Parameters.Add("@CPROF_UF", pessoa.Cprof_uf);
            contextQuery.Parameters.Add("@CR_CAT", pessoa.Cr_cat);
            contextQuery.Parameters.Add("@CR_CSM", pessoa.Cr_csm);
            contextQuery.Parameters.Add("@CR_DTEXP", TechneDbType.T_DATA, pessoa.Cr_dtexp);
            contextQuery.Parameters.Add("@CR_NUM", pessoa.Cr_num);
            contextQuery.Parameters.Add("@CR_RM", pessoa.Cr_rm);
            contextQuery.Parameters.Add("@CR_SERIE", pessoa.Cr_serie);
            contextQuery.Parameters.Add("@DT_NASC", TechneDbType.T_DATA, pessoa.Dt_nasc);
            contextQuery.Parameters.Add("@E_MAIL", pessoa.E_mail);
            contextQuery.Parameters.Add("@E_MAIL_INTERNO", pessoa.E_mail_interno);
            contextQuery.Parameters.Add("@END_COMPL", pessoa.End_compl);
            contextQuery.Parameters.Add("@END_MUNICIPIO", pessoa.End_municipio);
            contextQuery.Parameters.Add("@END_NUM", pessoa.End_num);
            contextQuery.Parameters.Add("@END_PAIS", pessoa.End_pais);
            contextQuery.Parameters.Add("@ENDERECO", pessoa.Endereco);
            contextQuery.Parameters.Add("@EST_CIVIL", pessoa.Est_civil);
            contextQuery.Parameters.Add("@FONE", pessoa.Fone);
            contextQuery.Parameters.Add("@ID_CENSO", pessoa.Id_censo);
            contextQuery.Parameters.Add("@MUNICIPIO_NASC", pessoa.Municipio_nasc);
            contextQuery.Parameters.Add("@NACIONALIDADE", pessoa.Nacionalidade);
            contextQuery.Parameters.Add("@NECESSIDADEESPECIALID", pessoa.NecessidadeEspecialId);
            contextQuery.Parameters.Add("@NOME_COMPL", pessoa.Nome_compl.ToUpper());
            contextQuery.Parameters.Add("@NOME_SOCIAL", pessoa.Nome_social.ToUpper());
            contextQuery.Parameters.Add("@PAIS_NASC", pessoa.Pais_nasc);
            contextQuery.Parameters.Add("@RG_DTEXP", TechneDbType.T_DATA, pessoa.Rg_dtexp);
            contextQuery.Parameters.Add("@RG_EMISSOR", pessoa.Rg_emissor);
            contextQuery.Parameters.Add("@RG_NUM", pessoa.Rg_num);
            contextQuery.Parameters.Add("@RG_TIPO", pessoa.Rg_tipo);
            contextQuery.Parameters.Add("@RG_UF", pessoa.Rg_uf);
            contextQuery.Parameters.Add("@SEXO", pessoa.Sexo);
            contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@TELEITOR_DTEXP", TechneDbType.T_DATA, pessoa.Teleitor_dtexp);
            contextQuery.Parameters.Add("@TELEITOR_NUM", pessoa.Teleitor_num);
            contextQuery.Parameters.Add("@TELEITOR_SECAO", pessoa.Teleitor_secao);
            contextQuery.Parameters.Add("@TELEITOR_ZONA", pessoa.Teleitor_zona);
            contextQuery.Parameters.Add("@NOME_MAE", pessoa.NomeMae.IsNullOrEmptyOrWhiteSpace() ? null : pessoa.NomeMae.ToUpper());
            contextQuery.Parameters.Add("@NOME_PAI", pessoa.NomePai.IsNullOrEmptyOrWhiteSpace() ? null : pessoa.NomePai.ToUpper());
            contextQuery.Parameters.Add("@IDFUNCIONAL", pessoa.IdFuncional);
            contextQuery.Parameters.Add("@PISPASEP", pessoa.Pispasep);
            contextQuery.Parameters.Add("@TELEITOR_MUN", pessoa.Teleitor_mun);
            contextQuery.Parameters.Add("@ETNIA", pessoa.Etnia);
            contextQuery.Parameters.Add("@AREA_ASSENTAMENTO", pessoa.AreaAssentamento);
            contextQuery.Parameters.Add("@TERRA_INDIGENA", pessoa.TerraIndigena);
            contextQuery.Parameters.Add("@AREA_QUILOMBOS", pessoa.AreaQuilombos);
            contextQuery.Parameters.Add("@AREA_TRADICIONAL", pessoa.AreaTradicional);
            contextQuery.Parameters.Add("@USUARIOID", pessoa.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaPessoaCandidato(DataContext contexto, LyPessoa pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_PESSOA 
                    SET    BAIRRO = @BAIRRO, 
                           CELULAR = @CELULAR, 
                           NOME_COMPL = @NOME_COMPL, 
                           NOME_PAI = @NOME_PAI, 
                           NOME_MAE = @NOME_MAE,
                           ENDERECO = @ENDERECO,
                           END_NUM = @END_NUM, 
                           END_MUNICIPIO = @END_MUNICIPIO,
                           CEP = @CEP, 
                           END_PAIS = @END_PAIS,
                           END_COMPL = @END_COMPL, 
                           RG_TIPO = @RG_TIPO, 
                           MUNICIPIO_NASC = @MUNICIPIO_NASC,
                           PAIS_NASC = @PAIS_NASC, 
                           NACIONALIDADE = @NACIONALIDADE, 
                           RG_NUM = @RG_NUM, 
                           RG_UF = @RG_UF, 
                           RG_EMISSOR = @RG_EMISSOR, 
                           CPF = @CPF, 
                           NECESSIDADEESPECIALID = @NECESSIDADEESPECIALID, 
                           EST_CIVIL = @EST_CIVIL, 
                           SEXO = @SEXO, 
                           FONE = @FONE,  
                           E_MAIL = @E_MAIL, 
                           CPROF_DTEXP = @CPROF_DTEXP, 
                           CPROF_NUM = @CPROF_NUM, 
                           CPROF_SERIE = @CPROF_SERIE, 
                           CPROF_UF = @CPROF_UF,
                           DT_NASC = @DT_NASC, 
                           RG_DTEXP = @RG_DTEXP,                        
                           IDFUNCIONAL = @IDFUNCIONAL, 
                           PISPASEP = @PISPASEP, 
                           ETNIA = @ETNIA,
                           STAMP_ATUALIZACAO = @STAMP_ATUALIZACAO, 
                           USUARIOID = @USUARIOID, 
                           DATAALTERACAO = @DATAALTERACAO 
                    WHERE  PESSOA = @PESSOA  ";

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa.Pessoa);
            contextQuery.Parameters.Add("@BAIRRO", TechneDbType.T_ALFAMEDIUM, pessoa.Bairro);
            contextQuery.Parameters.Add("@CELULAR", TechneDbType.T_TELEFONE, pessoa.Celular);
            contextQuery.Parameters.Add("@NOME_COMPL", pessoa.Nome_compl.ToUpper());
            contextQuery.Parameters.Add("@NOME_MAE", pessoa.NomeMae.IsNullOrEmptyOrWhiteSpace() ? null : pessoa.NomeMae.ToUpper());
            contextQuery.Parameters.Add("@NOME_PAI", pessoa.NomePai.IsNullOrEmptyOrWhiteSpace() ? null : pessoa.NomePai.ToUpper());
            contextQuery.Parameters.Add("@ENDERECO", pessoa.Endereco);
            contextQuery.Parameters.Add("@END_MUNICIPIO", pessoa.End_municipio);
            contextQuery.Parameters.Add("@END_NUM", pessoa.End_num);
            contextQuery.Parameters.Add("@CEP", pessoa.Cep);
            contextQuery.Parameters.Add("@END_COMPL", pessoa.End_compl);
            contextQuery.Parameters.Add("@END_PAIS", pessoa.End_pais);
            contextQuery.Parameters.Add("@RG_TIPO", pessoa.Rg_tipo);
            contextQuery.Parameters.Add("@MUNICIPIO_NASC", pessoa.Municipio_nasc);
            contextQuery.Parameters.Add("@PAIS_NASC", pessoa.Pais_nasc);
            contextQuery.Parameters.Add("@NACIONALIDADE", pessoa.Nacionalidade);
            contextQuery.Parameters.Add("@RG_NUM", pessoa.Rg_num);
            contextQuery.Parameters.Add("@RG_EMISSOR", pessoa.Rg_emissor);
            contextQuery.Parameters.Add("@RG_UF", pessoa.Rg_uf);
            contextQuery.Parameters.Add("@CPF", pessoa.Cpf);
            contextQuery.Parameters.Add("@NECESSIDADEESPECIALID", pessoa.NecessidadeEspecialId);
            contextQuery.Parameters.Add("@EST_CIVIL", pessoa.Est_civil);
            contextQuery.Parameters.Add("@SEXO", pessoa.Sexo);
            contextQuery.Parameters.Add("@E_MAIL", pessoa.E_mail);
            contextQuery.Parameters.Add("@CPROF_DTEXP", TechneDbType.T_DATA, pessoa.Cprof_dtexp);
            contextQuery.Parameters.Add("@CPROF_NUM", pessoa.Cprof_num);
            contextQuery.Parameters.Add("@CPROF_SERIE", pessoa.Cprof_serie);
            contextQuery.Parameters.Add("@CPROF_UF", pessoa.Cprof_uf);
            contextQuery.Parameters.Add("@DT_NASC", TechneDbType.T_DATA, pessoa.Dt_nasc);
            contextQuery.Parameters.Add("@RG_DTEXP", TechneDbType.T_DATA, pessoa.Rg_dtexp);
            contextQuery.Parameters.Add("@FONE", pessoa.Fone);
            contextQuery.Parameters.Add("@IDFUNCIONAL", pessoa.IdFuncional);
            contextQuery.Parameters.Add("@PISPASEP", pessoa.Pispasep);
            contextQuery.Parameters.Add("@ETNIA", pessoa.Etnia);
            contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@USUARIOID", pessoa.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaPessoaVoluntario(DataContext contexto, LyPessoa pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_PESSOA 
                        SET NOME_COMPL = @NOME_COMPL, 
                            PRE_NOME_SOCIAL = @NOME_SOCIAL, 
                            ENDERECO = @ENDERECO, 
                            END_MUNICIPIO = @END_MUNICIPIO, 
                            END_NUM = @END_NUM, 
                            CEP = @CEP, 
                            RG_EMISSOR = @RG_EMISSOR, 
                            RG_NUM = @RG_NUM, 
                            RG_TIPO = @RG_TIPO, 
                            MUNICIPIO_NASC = @MUNICIPIO_NASC, 
                            NACIONALIDADE = @NACIONALIDADE, 
                            NECESSIDADEESPECIALID = @NECESSIDADEESPECIALID,                         
                            PAIS_NASC = @PAIS_NASC, 
                            ETNIA = @ETNIA,
                            RG_UF = @RG_UF, 
                            EST_CIVIL = @EST_CIVIL, 
                            SEXO = @SEXO, 
                            END_COMPL = @END_COMPL, 
                            BAIRRO = @BAIRRO,
                            FONE = @FONE, 
                            CELULAR = @CELULAR, 
                            E_MAIL = @E_MAIL, 
                            CPF = @CPF, 
                            DT_NASC = @DT_NASC, 
                            RG_DTEXP = @RG_DTEXP, 
                            PISPASEP = @PISPASEP,
                            STAMP_ATUALIZACAO = @STAMP_ATUALIZACAO,
                            USUARIOID = @USUARIOID, 
                            DATAALTERACAO = @DATAALTERACAO 
                    WHERE  PESSOA = @PESSOA  ";

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa.Pessoa);
            contextQuery.Parameters.Add("@NOME_COMPL", pessoa.Nome_compl);
            contextQuery.Parameters.Add("@NOME_SOCIAL", pessoa.Nome_social);
            contextQuery.Parameters.Add("@ENDERECO", pessoa.Endereco);
            contextQuery.Parameters.Add("@END_MUNICIPIO", pessoa.End_municipio);
            contextQuery.Parameters.Add("@END_NUM", pessoa.End_num);
            contextQuery.Parameters.Add("@CEP", pessoa.Cep);
            contextQuery.Parameters.Add("@RG_EMISSOR", pessoa.Rg_emissor);
            contextQuery.Parameters.Add("@RG_TIPO", pessoa.Rg_tipo);
            contextQuery.Parameters.Add("@MUNICIPIO_NASC", pessoa.Municipio_nasc);
            contextQuery.Parameters.Add("@PAIS_NASC", pessoa.Pais_nasc);
            contextQuery.Parameters.Add("@NACIONALIDADE", pessoa.Nacionalidade);
            contextQuery.Parameters.Add("@NECESSIDADEESPECIALID", pessoa.NecessidadeEspecialId);
            contextQuery.Parameters.Add("@ETNIA", pessoa.Etnia);
            contextQuery.Parameters.Add("@RG_UF", pessoa.Rg_uf);
            contextQuery.Parameters.Add("@EST_CIVIL", pessoa.Est_civil);
            contextQuery.Parameters.Add("@SEXO", pessoa.Sexo);
            contextQuery.Parameters.Add("@END_COMPL", pessoa.End_compl);
            contextQuery.Parameters.Add("@BAIRRO", TechneDbType.T_ALFAMEDIUM, pessoa.Bairro);
            contextQuery.Parameters.Add("@FONE", pessoa.Fone);
            contextQuery.Parameters.Add("@CELULAR", TechneDbType.T_TELEFONE, pessoa.Celular);
            contextQuery.Parameters.Add("@E_MAIL", pessoa.E_mail);
            contextQuery.Parameters.Add("@RG_NUM", pessoa.Rg_num);
            contextQuery.Parameters.Add("@CPF", pessoa.Cpf);
            contextQuery.Parameters.Add("@DT_NASC", TechneDbType.T_DATA, pessoa.Dt_nasc);
            contextQuery.Parameters.Add("@RG_DTEXP", TechneDbType.T_DATA, pessoa.Rg_dtexp);
            contextQuery.Parameters.Add("@PISPASEP", pessoa.Pispasep);
            contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", SqlDbType.DateTime, DateTime.Now);
            contextQuery.Parameters.Add("@USUARIOID", pessoa.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaPessoaAluno(DataContext ctx, LyPessoa pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_PESSOA SET 
                                NOME_COMPL = @NOME_COMPL,
                                DT_NASC = @DT_NASC,
                                MUNICIPIO_NASC = @MUNICIPIO_NASC,
                                PAIS_NASC =	@PAIS_NASC,
                                NACIONALIDADE =	@NACIONALIDADE,
                                SEXO = @SEXO,
                                EST_CIVIL =	@EST_CIVIL,
                                ENDERECO = @ENDERECO,
                                END_NUM	= @END_NUM,
                                END_COMPL =	@END_COMPL,
                                BAIRRO = @BAIRRO,
                                END_MUNICIPIO =	@END_MUNICIPIO,
                                CEP	= @CEP,
                                FONE = @FONE,
                                RG_NUM = @RG_NUM,
                                RG_EMISSOR = @RG_EMISSOR,
                                RG_TIPO = @RG_TIPO, 
                                RG_DTEXP = @RG_DTEXP,
                                RG_UF =	@RG_UF,
                                CPF	= @CPF,
                                E_MAIL = @E_MAIL,
                                CELULAR	= @CELULAR, 
                                NECESSIDADEESPECIALID = @NECESSIDADEESPECIALID,  
                                CERT_NASC_NUM = @CERT_NASC_NUM,
                                CERT_NASC_FOLHA = @CERT_NASC_FOLHA,
                                CERT_NASC_LIVRO = @CERT_NASC_LIVRO,
                                CERT_NASC_EMISSAO = @CERT_NASC_EMISSAO,
                                CERT_NASC_CARTORIO_UF = @CERT_NASC_CARTORIO_UF,
                                CERT_NASC_CARTORIO_EXPED = @CERT_NASC_CARTORIO_EXPED,
                                ID_CENSO = @ID_CENSO,
                                TIPO_SANGUINEO= @TIPO_SANGUINEO,
                                ETNIA = @ETNIA,
                                CREDO = @CREDO,
                                QT_FILHOS = @QT_FILHOS,
                                PRE_NOME_SOCIAL = @PRE_NOME_SOCIAL,
                                CERT_NUMERO_MATRICULA = @CERT_NUMERO_MATRICULA,
                                ID_CARTORIO = @ID_CARTORIO,
                                NOME_PAI = @NOME_PAI ,
                                NOME_MAE = @NOME_MAE,
                                MAE_FALECIDA = @MAE_FALECIDA ,
                                PAI_FALECIDO = @PAI_FALECIDO ,
                                MAE_CPF = @MAE_CPF ,
                                PAI_CPF = @PAI_CPF ,
                                MAE_TELEFONE = @MAE_TELEFONE ,
                                PAI_TELEFONE = @PAI_TELEFONE ,
                                RESPONSAVEL = @RESPONSAVEL,
                                RESP_NOME_COMPL = @RESP_NOME_COMPL,
                                RESP_CPF = @RESP_CPF,
                                RESP_FONE = @RESP_FONE,
                                LATITUDE = @LATITUDE, 
                                LONGITUDE = @LONGITUDE,
                                AREA_ASSENTAMENTO = @AREA_ASSENTAMENTO, 
                                TERRA_INDIGENA = @TERRA_INDIGENA, 
                                AREA_QUILOMBOS = @AREA_QUILOMBOS, 
                                AREA_TRADICIONAL = @AREA_TRADICIONAL,
                                STAMP_ATUALIZACAO=GETDATE(),
                                USUARIOID = @USUARIOID,
                                DATAALTERACAO = GETDATE()
                             WHERE PESSOA = @PESSOA ";

            contextQuery.Parameters.Add("@NOME_COMPL", pessoa.Nome_compl.ToUpper());
            contextQuery.Parameters.Add("@DT_NASC", TechneDbType.T_DATA, pessoa.Dt_nasc);
            contextQuery.Parameters.Add("@MUNICIPIO_NASC", pessoa.Municipio_nasc);
            contextQuery.Parameters.Add("@PAIS_NASC", pessoa.Pais_nasc);
            contextQuery.Parameters.Add("@NACIONALIDADE", pessoa.Nacionalidade);
            contextQuery.Parameters.Add("@SEXO", pessoa.Sexo);
            contextQuery.Parameters.Add("@EST_CIVIL", pessoa.Est_civil);
            contextQuery.Parameters.Add("@ENDERECO", pessoa.Endereco);
            contextQuery.Parameters.Add("@END_NUM", pessoa.End_num);
            contextQuery.Parameters.Add("@END_COMPL", pessoa.End_compl);
            contextQuery.Parameters.Add("@BAIRRO", pessoa.Bairro);
            contextQuery.Parameters.Add("@END_MUNICIPIO", pessoa.End_municipio);
            contextQuery.Parameters.Add("@CEP", pessoa.Cep);
            contextQuery.Parameters.Add("@FONE", pessoa.Fone);
            contextQuery.Parameters.Add("@RG_NUM", pessoa.Rg_num);
            contextQuery.Parameters.Add("@RG_EMISSOR", pessoa.Rg_emissor);
            contextQuery.Parameters.Add("@RG_TIPO", pessoa.Rg_tipo);
            contextQuery.Parameters.Add("@RG_DTEXP", TechneDbType.T_DATA, pessoa.Rg_dtexp);
            contextQuery.Parameters.Add("@RG_UF", pessoa.Rg_uf);
            contextQuery.Parameters.Add("@CPF", pessoa.Cpf);
            contextQuery.Parameters.Add("@E_MAIL", pessoa.E_mail);
            contextQuery.Parameters.Add("@CELULAR", pessoa.Celular);
            contextQuery.Parameters.Add("@NECESSIDADEESPECIALID", pessoa.NecessidadeEspecialId);
            contextQuery.Parameters.Add("@CERT_NASC_NUM", pessoa.CertNascNum);
            contextQuery.Parameters.Add("@CERT_NASC_FOLHA", pessoa.CertNascFolha);
            contextQuery.Parameters.Add("@CERT_NASC_LIVRO", pessoa.CertNascLivro);
            contextQuery.Parameters.Add("@CERT_NASC_EMISSAO", TechneDbType.T_DATA, pessoa.CertNascEmissao);
            contextQuery.Parameters.Add("@CERT_NASC_CARTORIO_UF", pessoa.CertNascCartorioUf);
            contextQuery.Parameters.Add("@CERT_NASC_CARTORIO_EXPED", pessoa.CertNascCartorioExped);
            contextQuery.Parameters.Add("@ID_CENSO", pessoa.Id_censo);
            contextQuery.Parameters.Add("@TIPO_SANGUINEO", pessoa.Tipo_Sanguineo);
            contextQuery.Parameters.Add("@ETNIA", pessoa.Etnia);
            contextQuery.Parameters.Add("@CREDO", pessoa.Credo);
            contextQuery.Parameters.Add("@QT_FILHOS", TechneDbType.T_NUMERO_PEQUENO, pessoa.QtFilhos);
            contextQuery.Parameters.Add("@PRE_NOME_SOCIAL", pessoa.PreNomeSocial);
            contextQuery.Parameters.Add("@CERT_NUMERO_MATRICULA", pessoa.CertNumeroMatricula);
            contextQuery.Parameters.Add("@ID_CARTORIO", pessoa.IdCartorio);
            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa.Pessoa);
            contextQuery.Parameters.Add("@NOME_MAE", pessoa.NomeMae.ToUpper());
            contextQuery.Parameters.Add("@NOME_PAI", pessoa.NomePai.ToUpper());
            contextQuery.Parameters.Add("@PAI_FALECIDO", pessoa.PaiFalecido);
            contextQuery.Parameters.Add("@MAE_FALECIDA", pessoa.MaeFalecida);
            contextQuery.Parameters.Add("@MAE_CPF", pessoa.MaeCpf);
            contextQuery.Parameters.Add("@PAI_CPF", pessoa.PaiCpf);
            contextQuery.Parameters.Add("@MAE_TELEFONE", pessoa.MaeTelefone);
            contextQuery.Parameters.Add("@PAI_TELEFONE", pessoa.PaiTelefone);
            contextQuery.Parameters.Add("@RESPONSAVEL", pessoa.Responsavel);
            contextQuery.Parameters.Add("@RESP_NOME_COMPL", pessoa.RespNomeCompl.IsNullOrEmptyOrWhiteSpace() ? null : pessoa.RespNomeCompl.ToUpper());
            contextQuery.Parameters.Add("@RESP_CPF", pessoa.RespCpf);
            contextQuery.Parameters.Add("@RESP_FONE", pessoa.RespFone);
            contextQuery.Parameters.Add("@USUARIOID", pessoa.UsuarioId);
            contextQuery.Parameters.Add("@LATITUDE", pessoa.Latitude);
            contextQuery.Parameters.Add("@LONGITUDE", pessoa.Longitude);
            contextQuery.Parameters.Add("@AREA_ASSENTAMENTO", pessoa.AreaAssentamento);
            contextQuery.Parameters.Add("@TERRA_INDIGENA", pessoa.TerraIndigena);
            contextQuery.Parameters.Add("@AREA_QUILOMBOS", pessoa.AreaQuilombos);
            contextQuery.Parameters.Add("@AREA_TRADICIONAL", pessoa.AreaTradicional);


            ctx.ApplyModifications(contextQuery);
        }

        public int Remove(DataContext ctx, decimal pessoa)
        {
            try
            {
                var contextQuery = new ContextQuery
                {
                    Command = "DELETE FROM LY_PESSOA WHERE PESSOA = @PESSOA "
                };
                contextQuery.Parameters.Add("@PESSOA", pessoa);

                return ctx.ApplyModifications(contextQuery);

            }

            catch (Exception e)
            {
                throw e;
            }
        }

        public static void AlterarDadosCarteiraProfissional(string strNumCarteira, string strSerie, string strUF, DateTime dtExped, string strPessoa)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"update LY_PESSOA set CPROF_NUM = @CPROF_NUM, CPROF_SERIE = @CPROF_SERIE, CPROF_UF= @CPROF_UF, 
                                        CPROF_DTEXP = @CPROF_DTEXP
                                        WHERE PESSOA = @PESSOA";

                contextQuery.Parameters.Add("@CPROF_NUM", strNumCarteira);
                contextQuery.Parameters.Add("@CPROF_SERIE", strSerie);
                contextQuery.Parameters.Add("@CPROF_UF", strUF);
                contextQuery.Parameters.Add("@CPROF_DTEXP", dtExped);
                contextQuery.Parameters.Add("@PESSOA", strPessoa);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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
                ctx.Dispose();
            }
        }

        public decimal ObtemPessoaPor(DataContext contexto, string nome, string nomeMae, DateTime dataNascimento)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            decimal retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT PESSOA
                                FROM   LY_PESSOA P (NOLOCK)
                                WHERE P.NOME_COMPL = @NOME
	                                AND CONVERT(DATE, P.DT_NASC) = @DATANASCIMENTO 
	                                AND P.NOME_MAE = @NOMEMAE  ";

                contextQuery.Parameters.Add("@NOME", nome);
                contextQuery.Parameters.Add("@DATANASCIMENTO", dataNascimento.Date);
                contextQuery.Parameters.Add("@NOMEMAE", nomeMae);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToDecimal(reader["PESSOA"]);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return retorno;
        }

        public bool PossuiOutroVinculoPor(decimal pessoa, int vinculo, string matricula)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            RN.Docentes rnDocentes = new Docentes();
            RN.VinculoLy rnVinculo = new VinculoLy();
            bool possui = false;
            try
            {
                if (rnDocentes.PossuiOutroVinculoPor(ctx, pessoa, vinculo, matricula) ||
                    rnVinculo.PossuiOutroVinculoPor(ctx, pessoa, vinculo, matricula))
                {
                    possui = true;
                }
                return possui;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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
                ctx.Dispose();
            }
        }

        public bool PossuiCPFPor(DataContext ctx, string cpf, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool temCPF = false;

            contextQuery.Command = @" SELECT COUNT(*) FROM LY_PESSOA WHERE CPF=@CPF AND PESSOA <> @PESSOA ";

            contextQuery.Parameters.Add("@CPF", cpf);
            contextQuery.Parameters.Add("@PESSOA", pessoa);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                temCPF = true;
            }

            return temCPF;
        }

        public bool PossuiOutroNomeMaeDataNascimentoPor(DataContext ctx, string nome, string nomeMae, DateTime dataNascimento, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                      FROM LY_PESSOA P (NOLOCK)
                                      WHERE P.NOME_COMPL = @NOME
	                                        AND CONVERT(DATE, P.DT_NASC) = @DATANASCIMENTO 
	                                        AND P.NOME_MAE = @NOMEMAE
                                            AND PESSOA <> @PESSOA ";

            contextQuery.Parameters.Add("@NOME", nome);
            contextQuery.Parameters.Add("@DATANASCIMENTO", dataNascimento.Date);
            contextQuery.Parameters.Add("@NOMEMAE", nomeMae);
            contextQuery.Parameters.Add("@PESSOA", pessoa);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiOutroCPFPor(DataContext ctx, string cpf, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool temCPF = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                      FROM LY_PESSOA (NOLOCK)
                                      WHERE CPF = @CPF 
                                            AND PESSOA <> @PESSOA ";

            contextQuery.Parameters.Add("@CPF", cpf);
            contextQuery.Parameters.Add("@PESSOA", pessoa);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                temCPF = true;
            }

            return temCPF;
        }

        public bool PossuiOutroEmailInternoPor(DataContext ctx, string emailInterno, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM LY_PESSOA (NOLOCK)
                                WHERE E_MAIL_INTERNO = @E_MAIL_INTERNO
	                                AND PESSOA <> @PESSOA ";

            contextQuery.Parameters.Add("@E_MAIL_INTERNO", SqlDbType.VarChar, emailInterno);
            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public bool PossuiOutroEmailPor(DataContext ctx, string email, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                FROM LY_PESSOA (NOLOCK)
                                WHERE E_MAIL = @E_MAIL
	                                AND PESSOA <> @PESSOA ";

            contextQuery.Parameters.Add("@E_MAIL", SqlDbType.VarChar, email);
            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public string RetornaEmailInternoPor(DataContext contexto, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT TOP 1 E_MAIL_INTERNO 
                            FROM LY_PESSOA (NOLOCK)
                            WHERE PESSOA = @PESSOA  ";

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public string RetornaEmailPor(DataContext contexto, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT TOP 1 E_MAIL 
                            FROM LY_PESSOA (NOLOCK)
                            WHERE PESSOA = @PESSOA  ";

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public void AtualizaEmailInternoAlternativo(DataContext contexto, decimal pessoa, string emailInterno, string email, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_PESSOA
	                                    SET E_MAIL_INTERNO = @E_MAIL_INTERNO,
                                            E_MAIL = @E_MAIL,
		                                    USUARIOID = @USUARIOID,
		                                    DATAALTERACAO = @DATAALTERACAO
                                    WHERE PESSOA = @PESSOA ";

            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, pessoa);
            contextQuery.Parameters.Add("@E_MAIL_INTERNO", emailInterno);
            contextQuery.Parameters.Add("@E_MAIL", email);
            contextQuery.Parameters.Add("@USUARIOID", usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaEmailPessoal(DataContext contexto, int pessoa, string email, string usuarioId)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_PESSOA
	                                    SET E_MAIL = @E_MAIL,
		                                    USUARIOID = @USUARIOID,
		                                    DATAALTERACAO = @DATAALTERACAO
                                    WHERE PESSOA = @PESSOA ";

            contextQuery.Parameters.Add("@PESSOA", SqlDbType.Decimal, pessoa);
            contextQuery.Parameters.Add("@E_MAIL", email);
            contextQuery.Parameters.Add("@USUARIOID", usuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public bool PossuiOutroIdFuncionalPor(DataContext ctx, int idFuncional, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                     FROM LY_PESSOA (NOLOCK)
                                     WHERE IDFUNCIONAL =  @IDFUNCIONAL
	                                    AND PESSOA <> @PESSOA ";

            contextQuery.Parameters.Add("@IDFUNCIONAL", idFuncional);
            contextQuery.Parameters.Add("@PESSOA", pessoa);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public LyPessoa ObtemPessoaPor(string cpf)
        {
            LyPessoa pessoa = new LyPessoa();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"  SELECT TOP 1
                            P.*, C.codigo_uf, C.cod_cartorio, C.codigo_municipio, m.NOME AS NOME_MUNICIPIO, ge.EMAIL as E_MAIL_GOOGLE
                        FROM LY_PESSOA P
                            LEFT JOIN CARTORIO C ON c.cod_cartorio = P.id_cartorio
                            LEFT JOIN HADES..TCE_MUNICIPIO m ON p.End_municipio = m.ID_MUNICIPIO
                            LEFT JOIN RecursosHumanos.GOOGLEEDUCATION ge on ge.PESSOA = P.PESSOA
                        WHERE P.CPF = @CPF ";

                contextQuery.Parameters.Add("@CPF", SqlDbType.VarChar, cpf);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    pessoa.Alist_csm = Convert.ToString(reader["Alist_csm"]);
                    pessoa.Alist_num = Convert.ToString(reader["Alist_num"]);
                    pessoa.Alist_rm = Convert.ToString(reader["Alist_rm"]);
                    pessoa.Alist_serie = Convert.ToString(reader["Alist_serie"]);
                    pessoa.Bairro = Convert.ToString(reader["Bairro"]);
                    pessoa.Celular = Convert.ToString(reader["Celular"]);
                    pessoa.Cep = Convert.ToString(reader["Cep"]);
                    pessoa.Etnia = Convert.ToString(reader["Etnia"]);
                    pessoa.Cpf = Convert.ToString(reader["Cpf"]);
                    pessoa.Cprof_num = Convert.ToString(reader["Cprof_num"]);
                    pessoa.Cprof_serie = Convert.ToString(reader["Cprof_serie"]);
                    pessoa.Cprof_uf = Convert.ToString(reader["Cprof_uf"]);
                    pessoa.Cr_cat = Convert.ToString(reader["Cr_cat"]);
                    pessoa.Cr_csm = Convert.ToString(reader["Cr_csm"]);
                    pessoa.Cr_num = Convert.ToString(reader["Cr_num"]);
                    pessoa.Cr_rm = Convert.ToString(reader["Cr_rm"]);
                    pessoa.Cr_serie = Convert.ToString(reader["Cr_serie"]);
                    pessoa.E_mail = Convert.ToString(reader["E_mail"]);
                    pessoa.E_mail_interno = Convert.ToString(reader["E_mail_interno"]);

                    if (reader["E_mail_google"] != DBNull.Value)
                        pessoa.E_mail_google = Convert.ToString(reader["E_mail_google"]);

                    pessoa.End_compl = Convert.ToString(reader["End_compl"]);
                    pessoa.End_municipio = Convert.ToString(reader["End_municipio"]);
                    pessoa.End_num = Convert.ToString(reader["End_num"]);
                    pessoa.End_pais = Convert.ToString(reader["End_pais"]);
                    pessoa.Endereco = Convert.ToString(reader["Endereco"]);
                    pessoa.Est_civil = Convert.ToString(reader["Est_civil"]);
                    pessoa.Fone = Convert.ToString(reader["Fone"]);
                    pessoa.Id_censo = Convert.ToString(reader["Id_censo"]);
                    pessoa.Municipio_nasc = Convert.ToString(reader["Municipio_nasc"]);
                    pessoa.Nacionalidade = Convert.ToString(reader["Nacionalidade"]);
                    if (reader["NECESSIDADEESPECIALID"] != DBNull.Value)
                    {
                        pessoa.NecessidadeEspecialId = Convert.ToInt32(reader["NECESSIDADEESPECIALID"]);
                    }
                    pessoa.Nome_compl = Convert.ToString(reader["Nome_compl"]);
                    pessoa.Pais_nasc = Convert.ToString(reader["Pais_nasc"]);
                    pessoa.Rg_emissor = Convert.ToString(reader["Rg_emissor"]);
                    pessoa.Rg_num = Convert.ToString(reader["Rg_num"]);
                    pessoa.Rg_tipo = Convert.ToString(reader["Rg_tipo"]);
                    pessoa.Rg_uf = Convert.ToString(reader["Rg_uf"]);
                    pessoa.Sexo = Convert.ToString(reader["Sexo"]);
                    pessoa.Pessoa = Convert.ToDecimal(reader["Pessoa"]);
                    pessoa.Teleitor_num = Convert.ToString(reader["Teleitor_num"]);
                    pessoa.Teleitor_secao = Convert.ToString(reader["Teleitor_secao"]);
                    pessoa.Teleitor_zona = Convert.ToString(reader["Teleitor_zona"]);
                    pessoa.Teleitor_mun = Convert.ToString(reader["Teleitor_mun"]);
                    pessoa.Tipo_Sanguineo = Convert.ToString(reader["TIPO_SANGUINEO"]);
                    pessoa.Credo = Convert.ToString(reader["CREDO"]);
                    pessoa.Etnia = Convert.ToString(reader["ETNIA"]);
                    pessoa.CertNumeroMatricula = Convert.ToString(reader["CERT_NUMERO_MATRICULA"]);
                    pessoa.PreNomeSocial = Convert.ToString(reader["PRE_NOME_SOCIAL"]);
                    pessoa.Nome_social = Convert.ToString(reader["PRE_NOME_SOCIAL"]);
                    pessoa.CertNascNum = Convert.ToString(reader["CERT_NASC_NUM"]);
                    pessoa.CertNascFolha = Convert.ToString(reader["CERT_NASC_FOLHA"]);
                    pessoa.CertNascLivro = Convert.ToString(reader["CERT_NASC_LIVRO"]);
                    pessoa.CertNascCartorioExped = Convert.ToString(reader["CERT_NASC_CARTORIO_EXPED"]);
                    pessoa.CertNascCartorioUf = Convert.ToString(reader["CERT_NASC_CARTORIO_UF"]);
                    pessoa.NomePai = Convert.ToString(reader["NOME_PAI"]);
                    pessoa.NomeMae = Convert.ToString(reader["NOME_MAE"]);
                    pessoa.MaeFalecida = Convert.ToString(reader["MAE_FALECIDA"]);
                    pessoa.PaiFalecido = Convert.ToString(reader["PAI_FALECIDO"]);
                    pessoa.MaeCpf = Convert.ToString(reader["MAE_CPF"]);
                    pessoa.PaiCpf = Convert.ToString(reader["PAI_CPF"]);
                    pessoa.MaeTelefone = Convert.ToString(reader["MAE_TELEFONE"]);
                    pessoa.PaiTelefone = Convert.ToString(reader["PAI_TELEFONE"]);
                    pessoa.Responsavel = Convert.ToString(reader["RESPONSAVEL"]);
                    pessoa.RespNomeCompl = Convert.ToString(reader["RESP_NOME_COMPL"]);
                    pessoa.RespCpf = Convert.ToString(reader["RESP_CPF"]);
                    pessoa.RespFone = Convert.ToString(reader["RESP_FONE"]);
                    pessoa.NomeMunicipio = Convert.ToString(reader["NOME_MUNICIPIO"]);
                    pessoa.Passaporte = Convert.ToString(reader["PASSAPORTE"]);
                    pessoa.Latitude = Convert.ToString(reader["LATITUDE"]);
                    pessoa.Longitude = Convert.ToString(reader["LONGITUDE"]);
                    pessoa.AreaAssentamento = Convert.ToString(reader["AREA_ASSENTAMENTO"]);
                    pessoa.AreaQuilombos = Convert.ToString(reader["AREA_QUILOMBOS"]);
                    pessoa.AreaTradicional = Convert.ToString(reader["AREA_TRADICIONAL"]);
                    pessoa.TerraIndigena = Convert.ToString(reader["TERRA_INDIGENA"]);

                    if (reader["Cprof_dtexp"] != DBNull.Value)
                    {
                        pessoa.Cprof_dtexp = Convert.ToDateTime(reader["Cprof_dtexp"]);
                    }
                    if (reader["Alist_dtexp"] != DBNull.Value)
                    {
                        pessoa.Alist_dtexp = Convert.ToDateTime(reader["Alist_dtexp"]);
                    }
                    if (reader["Cr_dtexp"] != DBNull.Value)
                    {
                        pessoa.Cr_dtexp = Convert.ToDateTime(reader["Cr_dtexp"]);
                    }
                    if (reader["Dt_nasc"] != DBNull.Value)
                    {
                        pessoa.Dt_nasc = Convert.ToDateTime(reader["Dt_nasc"]);
                    }
                    if (reader["Rg_dtexp"] != DBNull.Value)
                    {
                        pessoa.Rg_dtexp = Convert.ToDateTime(reader["Rg_dtexp"]);
                    }
                    if (reader["Teleitor_dtexp"] != DBNull.Value)
                    {
                        pessoa.Teleitor_dtexp = Convert.ToDateTime(reader["Teleitor_dtexp"]);
                    }

                    if (reader["Stamp_atualizacao"] != DBNull.Value)
                    {
                        pessoa.Stamp_atualizacao = Convert.ToDateTime(reader["Stamp_atualizacao"]);
                    }
                    if (reader["ID_CARTORIO"] != DBNull.Value)
                    {
                        pessoa.IdCartorio = Convert.ToInt32(reader["ID_CARTORIO"]);
                    }
                    if (reader["QT_FILHOS"] != DBNull.Value)
                    {
                        pessoa.QtFilhos = Convert.ToDecimal(reader["QT_FILHOS"]);
                    }
                    if (reader["CERT_NASC_EMISSAO"] != DBNull.Value)
                    {
                        pessoa.CertNascEmissao = Convert.ToDateTime(reader["CERT_NASC_EMISSAO"]);
                    }

                    pessoa.CodigoUf = Convert.ToString(reader["codigo_uf"]);
                    pessoa.CodigoMunicipio = Convert.ToString(reader["codigo_municipio"]);
                    if (reader["IdFuncional"] != DBNull.Value)
                    {
                        pessoa.IdFuncional = Convert.ToInt32(reader["IdFuncional"]);
                    }
                    pessoa.Pispasep = Convert.ToString(reader["Pispasep"]);
                    pessoa.UsuarioId = Convert.ToString(reader["UsuarioId"]);
                    if (reader["datacadastro"] != DBNull.Value)
                    {
                        pessoa.DataCadastro = Convert.ToDateTime(reader["datacadastro"]);
                    }
                    if (reader["dataalteracao"] != DBNull.Value)
                    {
                        pessoa.DataAlteracao = Convert.ToDateTime(reader["dataalteracao"]);
                    }
                }

                return pessoa;
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }

        public decimal ObtemPessoaPor(DataContext contexto, string cpf)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            decimal retorno = 0;
            try
            {
                contextQuery.Command = @" SELECT PESSOA
                                FROM   LY_PESSOA P (NOLOCK)
                                WHERE CPF = @CPF  ";

                contextQuery.Parameters.Add("@CPF", cpf);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToDecimal(reader["PESSOA"]);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return retorno;
        }

        public RetValue AlteraPessoaDocenteContrato(TConnectionWritable connection, Ly_pessoa dtPessoa)
        {
            RetValue retorno = null;
            if (dtPessoa != null && dtPessoa.Rows != null)
            {

                System.Collections.ArrayList listaObjeto = new System.Collections.ArrayList();

                System.Text.StringBuilder colunas = new System.Text.StringBuilder();
                colunas.Append("PESSOA,NOME_COMPL,NOME_MAE,NOME_PAI,ENDERECO, END_NUM,END_MUNICIPIO,CEP,END_PAIS,END_COMPL,");
                colunas.Append("BAIRRO,RG_TIPO,MUNICIPIO_NASC,PAIS_NASC,NACIONALIDADE,RG_NUM,RG_UF,RG_EMISSOR,");
                colunas.Append("CPF,NECESSIDADEESPECIALID,EST_CIVIL,CPROF_UF,SEXO,FONE,CELULAR,E_MAIL,");
                colunas.Append("E_MAIL_INTERNO,CPROF_NUM,CPROF_SERIE,STAMP_ATUALIZACAO, idfuncional,pispasep,usuarioid, dataalteracao");

                listaObjeto.Add(dtPessoa.Rows[0].Pessoa);
                listaObjeto.Add(dtPessoa.Rows[0].Nome_compl.ToUpper());
                listaObjeto.Add(dtPessoa.Rows[0].Nome_mae.ToUpper());
                listaObjeto.Add(dtPessoa.Rows[0].Nome_pai.ToUpper());
                listaObjeto.Add(dtPessoa.Rows[0].Endereco);
                listaObjeto.Add(dtPessoa.Rows[0].End_num);
                listaObjeto.Add(dtPessoa.Rows[0].End_municipio);
                listaObjeto.Add(dtPessoa.Rows[0].Cep);
                listaObjeto.Add(dtPessoa.Rows[0].End_pais);
                listaObjeto.Add(dtPessoa.Rows[0].End_compl);
                listaObjeto.Add(dtPessoa.Rows[0].Bairro);
                listaObjeto.Add(dtPessoa.Rows[0].Rg_tipo);
                listaObjeto.Add(dtPessoa.Rows[0].Municipio_nasc);
                listaObjeto.Add(dtPessoa.Rows[0].Pais_nasc);
                listaObjeto.Add(dtPessoa.Rows[0].Nacionalidade);
                listaObjeto.Add(dtPessoa.Rows[0].Rg_num);
                listaObjeto.Add(dtPessoa.Rows[0].Rg_uf);
                listaObjeto.Add(dtPessoa.Rows[0].Rg_emissor);
                listaObjeto.Add(dtPessoa.Rows[0].Cpf);
                listaObjeto.Add(dtPessoa.Rows[0].Necessidadeespecialid);
                listaObjeto.Add(dtPessoa.Rows[0].Est_civil);
                listaObjeto.Add(dtPessoa.Rows[0].Cprof_uf);
                listaObjeto.Add(dtPessoa.Rows[0].Sexo);
                listaObjeto.Add(dtPessoa.Rows[0].Fone);
                listaObjeto.Add(dtPessoa.Rows[0].Celular);
                listaObjeto.Add(dtPessoa.Rows[0].E_mail);
                listaObjeto.Add(dtPessoa.Rows[0].E_mail_interno);
                listaObjeto.Add(dtPessoa.Rows[0].Cprof_num);
                listaObjeto.Add(dtPessoa.Rows[0].Cprof_serie);
                listaObjeto.Add(dtPessoa.Rows[0].Etnia);
                listaObjeto.Add(DateTime.Now);
                listaObjeto.Add(dtPessoa.Rows[0].IdFuncional);
                listaObjeto.Add(dtPessoa.Rows[0].Pispasep);
                listaObjeto.Add(dtPessoa.Rows[0].Usuarioid);
                listaObjeto.Add(DateTime.Now);

                colunas.Append(", DT_NASC,RG_DTEXP,CPROF_DTEXP");

                if (dtPessoa.Rows[0].Dt_nasc.HasValue)
                    listaObjeto.Add(dtPessoa.Rows[0].Dt_nasc);
                else
                    listaObjeto.Add(DBNull.Value);

                if (dtPessoa.Rows[0].Rg_dtexp.HasValue)
                    listaObjeto.Add(dtPessoa.Rows[0].Rg_dtexp);
                else
                    listaObjeto.Add(DBNull.Value);

                if (dtPessoa.Rows[0].Cprof_dtexp.HasValue)
                    listaObjeto.Add(dtPessoa.Rows[0].Cprof_dtexp);
                else
                    listaObjeto.Add(DBNull.Value);


                string ret = PreUpdate(dtPessoa.Rows[0], connection);
                if (!string.IsNullOrEmpty(ret))
                {
                    connection.Rollback();
                    return new RetValue(false, null, new ErrorList(ret));
                }
                Ly_pessoa.Row.Update(connection, dtPessoa.Rows[0].Pessoa, colunas.ToString(), listaObjeto.ToArray());

                retorno = VerificarErro(connection.GetErrors());
                retorno = (retorno ?? new RetValue(true, "Registro alterado com sucesso", null));
            }

            return retorno;

        }

        public RN.DTOs.DadosRecursoNecessidadeEspecial ObtemDadosRecursoNecessidadeEspecialPor(DataContext ctx, string cpf)
        {
            RN.DTOs.DadosRecursoNecessidadeEspecial recurso = new RN.DTOs.DadosRecursoNecessidadeEspecial();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT PESSOA, 
                                   NOME_COMPL, 
                                   CPF, 
                                   CEP, 
                                   END_MUNICIPIO, 
                                   ENDERECO, 
                                   END_NUM, 
                                   END_COMPL, 
                                   BAIRRO, 
                                   E_MAIL, 
                                   FONE, 
                                   CELULAR ,
                                   END_PAIS,
								   SEXO ,
								   DT_NASC ,
								   EST_CIVIL 
                            FROM   LY_PESSOA (NOLOCK) 
                            WHERE  CPF = @CPF ";

                contextQuery.Parameters.Add("@CPF", TechneDbType.T_CPF, cpf);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    recurso.PessoaId = Convert.ToInt32(reader["PESSOA"]);
                    recurso.Nome = Convert.ToString(reader["NOME_COMPL"]);
                    recurso.Cpf = Convert.ToString(reader["CPF"]);
                    recurso.Cep = Convert.ToString(reader["CEP"]);
                    recurso.Municipio = Convert.ToString(reader["END_MUNICIPIO"]);
                    recurso.Endereco = Convert.ToString(reader["ENDERECO"]);
                    recurso.Numero = Convert.ToString(reader["END_NUM"]);
                    recurso.Complemento = Convert.ToString(reader["END_COMPL"]);
                    recurso.Bairro = Convert.ToString(reader["BAIRRO"]);
                    recurso.Email = Convert.ToString(reader["E_MAIL"]);
                    recurso.Telefone = Convert.ToString(reader["FONE"]);
                    recurso.Celular = Convert.ToString(reader["CELULAR"]);
                    recurso.Sexo = Convert.ToString(reader["SEXO"]);
                    recurso.EstadoCivl = Convert.ToString(reader["EST_CIVIL"]);
                    recurso.PaisEndereco = Convert.ToString(reader["END_PAIS"]);
                    if (reader["DT_NASC"] != DBNull.Value)
                    {
                        recurso.DataNascimento = Convert.ToDateTime(reader["DT_NASC"]);
                    }
                    recurso.Bloqueado = false;
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return recurso;
        }

        public RN.DTOs.DadosVoluntario ObtemDadosVoluntarioPor(DataContext ctx, string cpf)
        {
            RN.DTOs.DadosVoluntario voluntario = new RN.DTOs.DadosVoluntario();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT PESSOA, 
                                                   NOME_COMPL, 
                                                   PRE_NOME_SOCIAL, 
                                                   CPF, 
                                                   CEP, 
                                                   END_MUNICIPIO, 
                                                   ENDERECO, 
                                                   END_NUM, 
                                                   END_COMPL, 
                                                   BAIRRO, 
                                                   E_MAIL, 
                                                   FONE, 
                                                   CELULAR, 
                                                   END_PAIS, 
                                                   SEXO, 
                                                   DT_NASC, 
                                                   EST_CIVIL, 
                                                   ID_CENSO, 
                                                   ETNIA, 
                                                   PAIS_NASC, 
                                                   MUNICIPIO_NASC, 
                                                   NACIONALIDADE, 
                                                   NECESSIDADEESPECIALID, 
                                                   RG_TIPO, 
                                                   RG_NUM, 
                                                   RG_DTEXP, 
                                                   RG_UF, 
                                                   RG_EMISSOR 
                                            FROM   LY_PESSOA (NOLOCK) 
                                            WHERE  CPF = @CPF ";

                contextQuery.Parameters.Add("@CPF", TechneDbType.T_CPF, cpf);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    voluntario.PessoaId = Convert.ToInt32(reader["PESSOA"]);
                    voluntario.Nome = Convert.ToString(reader["NOME_COMPL"]);
                    voluntario.NomeSocial = Convert.ToString(reader["PRE_NOME_SOCIAL"]);
                    voluntario.Cpf = Convert.ToString(reader["CPF"]);
                    voluntario.Cep = Convert.ToString(reader["CEP"]);
                    voluntario.Municipio = Convert.ToString(reader["END_MUNICIPIO"]);
                    voluntario.Endereco = Convert.ToString(reader["ENDERECO"]);
                    voluntario.Numero = Convert.ToString(reader["END_NUM"]);
                    voluntario.Complemento = Convert.ToString(reader["END_COMPL"]);
                    voluntario.Bairro = Convert.ToString(reader["BAIRRO"]);
                    voluntario.Email = Convert.ToString(reader["E_MAIL"]);
                    voluntario.Telefone = Convert.ToString(reader["FONE"]);
                    voluntario.Celular = Convert.ToString(reader["CELULAR"]);
                    voluntario.Sexo = Convert.ToString(reader["SEXO"]);
                    voluntario.EstadoCivl = Convert.ToString(reader["EST_CIVIL"]);
                    voluntario.PaisEndereco = Convert.ToString(reader["END_PAIS"]);
                    if (reader["DT_NASC"] != DBNull.Value)
                    {
                        voluntario.DataNascimento = Convert.ToDateTime(reader["DT_NASC"]);
                    }
                    voluntario.IdInep = Convert.ToString(reader["ID_CENSO"]);
                    voluntario.CorRaca = Convert.ToString(reader["ETNIA"]);
                    voluntario.PaisNascimento = Convert.ToString(reader["PAIS_NASC"]);
                    voluntario.MunicipioNascimento = Convert.ToString(reader["MUNICIPIO_NASC"]);
                    voluntario.Nacionalidade = Convert.ToString(reader["NACIONALIDADE"]);
                    if (reader["NECESSIDADEESPECIALID"] != DBNull.Value)
                    {
                        voluntario.NecessidadeEspecial = Convert.ToString(reader["NECESSIDADEESPECIALID"]);
                    }
                    voluntario.RgTipo = Convert.ToString(reader["RG_TIPO"]);
                    voluntario.RgNumero = Convert.ToString(reader["RG_NUM"]);

                    if (reader["RG_DTEXP"] != DBNull.Value)
                    {
                        voluntario.RgDataExpedicao = Convert.ToDateTime(reader["RG_DTEXP"]);
                    }
                    else
                    {
                        voluntario.RgDataExpedicao = null;
                    }

                    voluntario.RgUf = Convert.ToString(reader["RG_UF"]);
                    voluntario.RgEmissor = Convert.ToString(reader["RG_EMISSOR"]);
                    voluntario.Bloqueado = false;
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return voluntario;
        }

        public DataTable ObtemListaPor(string nome, string nomeMae, DateTime dataNascimento, string nomeAnterior, string nomeMaeAnterior, DateTime dataNascimentoAnterior)
        {
            StringBuilder sql = new System.Text.StringBuilder();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                sql.Append(@" SELECT 
                                           P.PESSOA, 
                                           A.ALUNO,
                                           NOME_COMPL, 
                                           NOME_MAE, 
                                           NOME_PAI, 
                                           CONVERT(VARCHAR(20), DT_NASC, 103) AS DT_NASC, 
                                           CPF, 
                                           FONE, 
                                           CELULAR, 
                                           ENDERECO, 
                                           END_NUM, 
                                           BAIRRO,
	                                       E_MAIL,
										   CASE 
											   WHEN FP.FOTO IS NULL THEN 'Não'
											   ELSE 'Sim'
										   END POSSUI_FOTO,
										   CASE 
											   WHEN fl.FL_FIELD_04 IS NULL THEN 'Não'
											   ELSE 'Sim'
										   END GRATUIDADE,
                                           A.SIT_ALUNO
                                    FROM   LY_ALUNO A
                                           INNER JOIN LY_PESSOA P (NOLOCK) ON P.PESSOA = A.PESSOA
										   LEFT JOIN LY_FOTO_PESSOA FP (NOLOCK) ON P.PESSOA = FP.PESSOA
										   LEFT JOIN LY_FL_PESSOA FL (NOLOCK) ON P.PESSOA = FL.PESSOA
                                      WHERE  P.NOME_COMPL = @NOME_COMPL 
                                           AND ISNULL(P.NOME_MAE, '') = @NOME_MAE 
                                           AND P.DT_NASC = @DT_NASC  ");

                if (nome != nomeAnterior
                    || nomeMae != nomeMaeAnterior
                    || dataNascimento != dataNascimentoAnterior)
                {
                    sql.Append(@"  OR ( P.NOME_COMPL = @NOME_COMPL_ANTERIOR 
                                           AND ISNULL(P.NOME_MAE, '') = @NOME_MAE_ANTERIOR 
                                           AND P.DT_NASC = @DT_NASC_ANTERIOR ) ");

                    contextQuery.Parameters.Add("@NOME_COMPL_ANTERIOR", nomeAnterior);
                    contextQuery.Parameters.Add("@NOME_MAE_ANTERIOR", nomeMaeAnterior);
                    contextQuery.Parameters.Add("@DT_NASC_ANTERIOR", dataNascimentoAnterior);
                }

                contextQuery.Command = sql.ToString();

                contextQuery.Parameters.Add("@NOME_COMPL", nome);
                contextQuery.Parameters.Add("@NOME_MAE", nomeMae);
                contextQuery.Parameters.Add("@DT_NASC", dataNascimento);

                dt = contexto.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Empty;
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

            return dt;
        }

        public ValidacaoDados ValidaRemocaoDuplicidade(DadosDuplicidadeAluno dadosDuplicidadeAluno)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            RN.Docentes rnDocentes = new Docentes();
            RN.Aluno rnAluno = new Aluno();
            RN.Curriculo rnCurriculo = new Curriculo();
            RN.VinculoLy rnVinculo = new VinculoLy();
            RN.Matricula rnMatricula = new Matricula();
            RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            RN.RecursosHumanos.NomeSemValidacao rnNomeSemValidacao = new Techne.Lyceum.RN.RecursosHumanos.NomeSemValidacao();
            List<string> listaMatriculasTotal = new List<string>();
            RN.RecursosHumanos.UsuarioExterno rnExterno = new Techne.Lyceum.RN.RecursosHumanos.UsuarioExterno();
            RN.NecessidadeEspecial.RecursoNecessidadeEspecial rnRecurso = new Techne.Lyceum.RN.NecessidadeEspecial.RecursoNecessidadeEspecial();
            Regex regex = new Regex(@"\s{2,}");
            LyCurriculo lyCurriculo = new LyCurriculo();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosDuplicidadeAluno.MatriculaCorreta.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo MATRICULA é obrigatório.");
            }

            if (dadosDuplicidadeAluno.PessoaCorreta <= 0)
            {
                mensagens.Add("Campo PESSOA é obrigatório.");
            }

            if (dadosDuplicidadeAluno.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO RESPONSÁVEL é obrigatório.");
            }

            if (dadosDuplicidadeAluno.Nome.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME é obrigatório.");
            }
            else
            {
                //Busca, caso exista Nome sem validação
                string nomePermitido = rnNomeSemValidacao.ObtemNomePor(dadosDuplicidadeAluno.PessoaCorreta, (int)RN.RecursosHumanos.NomeSemValidacao.Tipo.Nome);

                //Apenas valida para o nome nao seja o permitido
                if (nomePermitido.IsNullOrEmptyOrWhiteSpace() || nomePermitido.ToUpper() != dadosDuplicidadeAluno.Nome.ToUpper())
                {
                    dadosDuplicidadeAluno.Nome = dadosDuplicidadeAluno.Nome.RetiraEspacosDuplos();

                    /// Validaçoes de nome do aluno 
                    if (dadosDuplicidadeAluno.Nome.Length < 5)
                    {
                        mensagens.Add("Campo NOME DO ALUNO deve conter pelo menos cinco letras!");
                    }

                    if (!string.IsNullOrEmpty(dadosDuplicidadeAluno.Nome)
                        && !Validacao.SomenteLetras(dadosDuplicidadeAluno.Nome))
                    {
                        mensagens.Add("Campo NOME DO ALUNO não pode conter números.");
                    }

                    var palavras = dadosDuplicidadeAluno.Nome.CountWords();
                    var contemRepeticao = RN.Validacao.ContemRepeticao(dadosDuplicidadeAluno.Nome, 3);
                    var nomeInvalido = TextValidator.HasForbiddenWords(dadosDuplicidadeAluno.Nome, new PalavrasProibidasEmNomes());
                    if (Validacao.contemNumeros(dadosDuplicidadeAluno.Nome) == false)
                    {
                        if (palavras < 2)
                        {
                            mensagens.Add("Por favor informar, informar nome e sobrenome no campo NOME.");
                        }

                        if (contemRepeticao)
                        {
                            mensagens.Add("Campo NOME possui inconsistência por repetição excessiva de letras. Favor corrigir a informação.");
                        }

                        if (nomeInvalido)
                        {
                            mensagens.Add("Campo NOME possui inconsistência por não representar um nome válido. Favor corrigir a informação.");
                        }
                    }

                    regex = new Regex(@"\s{2,}");
                    string NomeCompl = regex.Replace(dadosDuplicidadeAluno.Nome.Trim().ToUpper(), " ");
                    var contemApostrofeRep = Validacao.substitueApostrofe(NomeCompl);
                    if (contemApostrofeRep)
                    {
                        mensagens.Add("Campo NOME DO ALUNO possui inconsistência por repetição excessiva de apóstrofes. Favor corrigir a informação.");
                    }
                }
            }

            if (dadosDuplicidadeAluno.NomeMae.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME DA MÃE é obrigatório.");
            }
            else
            {
                //Busca, caso exista Nome sem validação
                string nomePermitido = rnNomeSemValidacao.ObtemNomePor(dadosDuplicidadeAluno.PessoaCorreta, (int)RN.RecursosHumanos.NomeSemValidacao.Tipo.Mae);

                //Apenas valida para o nome nao seja o permitido
                if (nomePermitido.IsNullOrEmptyOrWhiteSpace() || nomePermitido.ToUpper() != dadosDuplicidadeAluno.NomeMae.ToUpper())
                {
                    dadosDuplicidadeAluno.NomeMae = dadosDuplicidadeAluno.NomeMae.RetiraEspacosDuplos();

                    //Validaçoes de nome da mae
                    var palavras = dadosDuplicidadeAluno.NomeMae.CountWords();
                    var contemRepeticao = RN.Validacao.ContemRepeticao(dadosDuplicidadeAluno.NomeMae, 3);
                    var nomeInvalido = TextValidator.HasForbiddenWords(dadosDuplicidadeAluno.NomeMae, new PalavrasProibidasEmNomes());
                    if (!Validacao.contemNumeros(dadosDuplicidadeAluno.NomeMae))
                    {
                        if (palavras < 2)
                        {
                            mensagens.Add("Por favor informar, informar nome e sobrenome no campo NOME DA MÃE.");
                        }

                        if (contemRepeticao)
                        {
                            mensagens.Add("Campo NOME DA MÃE possui inconsistência por repetição excessiva de letras. Favor corrigir a informação.");
                        }

                        if (nomeInvalido)
                        {
                            mensagens.Add("Campo NOME DA MÃE possui inconsistência por não representar um nome válido. Favor corrigir a informação.");
                        }
                    }
                    else if (!Validacao.SomenteLetras(dadosDuplicidadeAluno.NomeMae))
                    {
                        mensagens.Add("Campo NOME DA MÃE não pode conter números.");
                    }

                    string NomeMae = regex.Replace(dadosDuplicidadeAluno.NomeMae.Trim().ToUpper(), " ");
                    var contemApostrofeRepMae = Validacao.substitueApostrofe(NomeMae);
                    if (contemApostrofeRepMae)
                    {
                        mensagens.Add("Campo NOME DO MÃE possui inconsistência por repetição excessiva de apóstrofes. Favor corrigir a informação.");
                    }
                }
            }

            if (dadosDuplicidadeAluno.DataNascimento == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DE NASCIMENTO é obrigatório.");
            }
            else
            {
                int idade = Utils.CalcularIdade(dadosDuplicidadeAluno.DataNascimento);

                if (idade > 110)
                {
                    mensagens.Add("A idade não pode ser superior a 110 anos. Favor verificar a Data de Nascimento.");
                }
            }

            if (dadosDuplicidadeAluno.SituacaoMatricula.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo SITUAÇÃO MATICULA ESCOLHIDA é obrigatório.");
            }
            else
            {
                if (dadosDuplicidadeAluno.SituacaoMatricula == "Ativo")
                {
                    if (dadosDuplicidadeAluno.Ano <= 0)
                    {
                        mensagens.Add("Campo ANO é obrigatório.");
                    }

                    if (dadosDuplicidadeAluno.Periodo < 0)
                    {
                        mensagens.Add("Campo PERIODO é obrigatório.");
                    }

                    if (dadosDuplicidadeAluno.Censo.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo UNIDADE é obrigatório.");
                    }

                    if (dadosDuplicidadeAluno.Curso.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo CURSO é obrigatório.");
                    }

                    if (dadosDuplicidadeAluno.Serie < 0)
                    {
                        mensagens.Add("Campo SÉRIE é obrigatório.");
                    }

                    if (dadosDuplicidadeAluno.Turno.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo TURNO é obrigatório.");
                    }

                    if (dadosDuplicidadeAluno.TipoVaga.IsNullOrEmptyOrWhiteSpace())
                    {
                        mensagens.Add("Campo TIPO VAGA é obrigatório.");
                    }
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Monta lista com todas as pessoas ja encontradas
                    listaMatriculasTotal.Add(dadosDuplicidadeAluno.MatriculaCorreta);
                    if (dadosDuplicidadeAluno.MatriculasParaCancelar != null)
                    {
                        if (dadosDuplicidadeAluno.MatriculasParaCancelar.Count > 0)
                        {
                            listaMatriculasTotal.AddRange(dadosDuplicidadeAluno.MatriculasParaCancelar);
                        }
                    }

                    if (dadosDuplicidadeAluno.SituacaoMatricula == "Ativo")
                    {
                        lyCurriculo = rnCurriculo.ObtemPrimeiroAtivoPor(dadosDuplicidadeAluno.Curso, dadosDuplicidadeAluno.Turno, dadosDuplicidadeAluno.Serie, dadosDuplicidadeAluno.Ano, dadosDuplicidadeAluno.Periodo);
                        if (lyCurriculo.Curriculo.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add("Não foi encontrada MATRIZ CURRICULAR para este ano / periodo / curso / turno / serie.");
                        }
                        else
                        {
                            dadosDuplicidadeAluno.Curriculo = lyCurriculo.Curriculo;
                        }

                        if (listaMatriculasTotal != null)
                        {
                            if (listaMatriculasTotal.Count > 0)
                            {
                                //Verifica se existe confirmação de matricula confirmada para os dados
                                if (!rnConfirmacaoMatricula.PossuiConfirmacaoMatriculaConfirmadaPor(contexto, listaMatriculasTotal, dadosDuplicidadeAluno.Curso, dadosDuplicidadeAluno.Serie, dadosDuplicidadeAluno.Turno, dadosDuplicidadeAluno.Ano, dadosDuplicidadeAluno.Periodo, dadosDuplicidadeAluno.Censo))
                                {
                                    mensagens.Add("Não existe confirmação de Matricula Confirmada com estes dados em nenhuma das matriculas deste aluno.");
                                }
                            }
                        }

                        if (listaMatriculasTotal.Count > 1)
                        {
                            //Valida para caso haja mais de uma matriculas e caso exista confirmação para o proximo ano / periodo 
                            //em qualquer uma delas, não será permitido o tratamento da duplicidade.
                            if (rnConfirmacaoMatricula.PossuiConfirmacaoMatriculaConfirmadaEmProximosPeriodosPor(contexto, listaMatriculasTotal, dadosDuplicidadeAluno.Ano, dadosDuplicidadeAluno.Periodo))
                            {
                                mensagens.Add("O tratamento da duplicidade não pode ser realizado pois exista confirmação para o proximo ano / periodo em uma das matriculas.");
                            }
                        }

                        if (mensagens.Count == 0)
                        {
                            //Verifica se a confirmaçao do aluno é para o periodo 2
                            if (dadosDuplicidadeAluno.Periodo == 2)
                            {
                                //Verifica se o aluno possui enturmação ativa no periodo 1
                                if (rnMatricula.PossuiMatriculaAtivaPeriodoPor(contexto, listaMatriculasTotal, dadosDuplicidadeAluno.Ano, 1))
                                {
                                    //NÃO PERMITIR CONFIRMAR O CANDIDATO PARA O PERÍODO 2 ENQUANTO TIVER ENTURMACAO ATIVA NO PERÍODO 1
                                    mensagens.Add("Este aluno não pode ser confirmado no periodo 2 pois possui enturmação ativa no periodo 1 em umas das matriculas.");
                                }
                            }
                            else
                            {
                                //Verifica se o aluno possui enturmação ativa no ano anterior (qq periodo)
                                if (rnMatricula.PossuiMatriculaAtivaAnoPor(contexto, listaMatriculasTotal, dadosDuplicidadeAluno.Ano - 1))
                                {
                                    //NÃO PERMITIR CONFIRMAR O CANDIDATO PARA O UM ANO ENQUANTO TIVER ENTURMACAO ATIVA NO PERÍODO 1NO ANTERIOR
                                    mensagens.Add("Este aluno não pode ser confirmado neste ano pois possui enturmação ativa no ano anterior em umas das matriculas.");
                                }
                            }
                        }
                    }

                    if (listaMatriculasTotal != null)
                    {
                        if (listaMatriculasTotal.Count > 0)
                        {
                            //Verifica se existem mais pessoas
                            if (rnAluno.PossuiOutroAlunoPor(contexto, dadosDuplicidadeAluno.Nome, dadosDuplicidadeAluno.NomeMae, dadosDuplicidadeAluno.DataNascimento, listaMatriculasTotal))
                            {
                                mensagens.Add("Existem outras matriculas com este nome / data de nascimento / nome da mãe, favor realizar novamente a busca.");
                            }
                        }
                    }

                    if (dadosDuplicidadeAluno.PessoasParaRemover != null)
                    {
                        if (dadosDuplicidadeAluno.PessoasParaRemover.Count > 0)
                        {
                            foreach (decimal pessoaId in dadosDuplicidadeAluno.PessoasParaRemover)
                            {
                                //Verifica se é docente
                                if (rnDocentes.EhDocentePor(contexto, pessoaId))
                                {
                                    mensagens.Add(string.Format("Não será possivel tratar a duplicidade, pois existe um docente com esse mesmo nome, nome da mãe e data de nascimento.", pessoaId.ToString()));
                                }

                                //Verifica se é servidor
                                if (rnVinculo.EhServidorPor(contexto, pessoaId))
                                {
                                    mensagens.Add(string.Format("Não será possivel tratar a duplicidade, pois existe um servidor com esse mesmo nome, nome da mãe e data de nascimento.", pessoaId.ToString()));
                                }

                                //Verifica se é recurso
                                if (rnRecurso.EhRecursoPor(contexto, pessoaId))
                                {
                                    mensagens.Add(string.Format("Não será possivel tratar a duplicidade, pois existe um recurso de Necessidade Especial com esse mesmo nome, nome da mãe e data de nascimento.", pessoaId.ToString()));
                                }

                                //Verifica se é externo
                                if (rnExterno.EhExternoPor(contexto, pessoaId))
                                {
                                    mensagens.Add(string.Format("Não será possivel tratar a duplicidade, pois existe um usuário externo vinculado com esse mesmo nome, nome da mãe e data de nascimento.", pessoaId.ToString()));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (contexto != null)
                    {
                        contexto.Dispose();
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void RemoveDuplicidade(DadosDuplicidadeAluno dadosDuplicidadeAluno)
        {
            RN.Aluno rnAluno = new Aluno();
            RN.Aluno.DadosAluno dadosAluno = new Aluno.DadosAluno();
            RN.Carteirinha rnCarteirinha = new Carteirinha();
            RN.Certificacao.DocumentoCertificacao rnDocumentoCertificacao = new Techne.Lyceum.RN.Certificacao.DocumentoCertificacao();
            RN.Turma rnTurma = new Turma();
            RN.RecursosHumanos.PessoaDadosBancarios rnPessoaDadosBancarios = new Techne.Lyceum.RN.RecursosHumanos.PessoaDadosBancarios();
            RN.Matricula rnMatricula = new Matricula();
            RN.Matgrade rnMatgrade = new Matgrade();
            RN.HCursosConcl rnHCursosConcl = new HCursosConcl();
            RN.RenovacaoMatricula.Renovacao rnRenovacao = new Techne.Lyceum.RN.RenovacaoMatricula.Renovacao();
            RN.ConfirmacaoMatricula rnConfirmacaoMatricula = new ConfirmacaoMatricula();
            TceConfirmacaoMatricula confirmacao = new TceConfirmacaoMatricula();
            RN.RelacaoPessoa rnRelacaoPessoa = new RelacaoPessoa();
            RN.Transferencia rnTransferencia = new Transferencia();
            RN.PessoaRecursoAplicacaoProva rnPessoaRecursoAplicacaoProva = new PessoaRecursoAplicacaoProva();
            RN.Matriculas.PessoaAluno rnPessoaAluno = new Techne.Lyceum.RN.Matriculas.PessoaAluno();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.Matriculas.PreCadastroAluno rnPreCadastroAluno = new Techne.Lyceum.RN.Matriculas.PreCadastroAluno();
            RN.Matriculas.InscricaoAluno rnInscricaoAluno = new Techne.Lyceum.RN.Matriculas.InscricaoAluno();
            RN.Matriculas.PreCadastroAlunoPessoaHist rnPreCadastroAlunoPessoaHist = new Techne.Lyceum.RN.Matriculas.PreCadastroAlunoPessoaHist();
            string turma = string.Empty;
            bool enturmacaoCorreta = false;
            RN.RecursosHumanos.GoogleEducation rnGoogleEducation = new Techne.Lyceum.RN.RecursosHumanos.GoogleEducation();
            RN.CartaoEstudante.FotoPessoa rnCartaoEstudanteFotoPessoa = new Techne.Lyceum.RN.CartaoEstudante.FotoPessoa();
            RN.Ocorrencias.Acusado rnAcusado = new Techne.Lyceum.RN.Ocorrencias.Acusado();
            RN.Ocorrencias.Vitima rnVitima = new Techne.Lyceum.RN.Ocorrencias.Vitima();
            RecursosHumanos.HistoricoAlteracaoAluno rnHistoricoAlteracaoAluno = new Techne.Lyceum.RN.RecursosHumanos.HistoricoAlteracaoAluno();
            RecursosHumanos.HistoricoAlteracaoAlunoCampos rnHistoricoAlteracaoAlunoCampos = new Techne.Lyceum.RN.RecursosHumanos.HistoricoAlteracaoAlunoCampos();
            RN.PessoaTranstornoAprendizagem rnPessoaTranstornoAprendizagem = new PessoaTranstornoAprendizagem();

            try
            {
                //Busca pessoa que está com a chave unica na precadastroaluno
                decimal pessoaChaveUnica = rnPreCadastroAluno.ObtemPessoaPor(contexto, dadosDuplicidadeAluno.Nome, dadosDuplicidadeAluno.DataNascimento, dadosDuplicidadeAluno.NomeMae);

                //Verifica se não é a pessoa correta na chave unica
                if (pessoaChaveUnica != dadosDuplicidadeAluno.PessoaCorreta && pessoaChaveUnica != -1)
                {
                    //Verifica se o conjunto de dados possui inscrição
                    if (rnInscricaoAluno.PossuiInscricaoPor(contexto, pessoaChaveUnica))
                    {
                        //Verifica se a pessoa correta nao possui inscrição
                        if (!rnInscricaoAluno.PossuiInscricaoPor(contexto, dadosDuplicidadeAluno.PessoaCorreta))
                        {
                            //Remove do precadastroaluno da pessoa correta 
                            rnPreCadastroAluno.RemovePreCadastroSemInscricao(contexto, dadosDuplicidadeAluno.PessoaCorreta);

                            //ATualiza a pessoa da chave
                            rnPreCadastroAluno.AtualizaPessoa(contexto, pessoaChaveUnica, dadosDuplicidadeAluno.PessoaCorreta);
                        }
                    }
                    else
                    {
                        //Remove do precadastroaluno da chave unica 
                        rnPreCadastroAluno.RemovePreCadastroSemInscricao(contexto, pessoaChaveUnica);
                    }

                    //Leva para historico precadastroaluno da chave unica que possua inscrição 
                    rnPreCadastroAlunoPessoaHist.Insere(contexto, pessoaChaveUnica, dadosDuplicidadeAluno.UsuarioId);

                    //Limpa pessoa do precadastroaluno da chave unica que possua inscrição 
                    rnPreCadastroAluno.LimpaPessoaPreCadastroComInscricao(contexto, pessoaChaveUnica, dadosDuplicidadeAluno.UsuarioId);
                }

                //Atualiza dados da chave unica na pessoa correta
                this.AtualizaDadosIdentificacao(contexto, dadosDuplicidadeAluno.Nome, dadosDuplicidadeAluno.NomeMae, dadosDuplicidadeAluno.DataNascimento, dadosDuplicidadeAluno.PessoaCorreta, dadosDuplicidadeAluno.UsuarioId);

                //Atualiza dados da chave unica no precadastro da pessoa correta
                rnPreCadastroAluno.AtualizaDadosIdentificacao(contexto, dadosDuplicidadeAluno.Nome, dadosDuplicidadeAluno.NomeMae, dadosDuplicidadeAluno.DataNascimento, dadosDuplicidadeAluno.PessoaCorreta, dadosDuplicidadeAluno.UsuarioId);

                //Busca novamente para verficar se ainda existia outra linha com a chave unica
                pessoaChaveUnica = rnPreCadastroAluno.ObtemPessoaPor(contexto, dadosDuplicidadeAluno.Nome, dadosDuplicidadeAluno.DataNascimento, dadosDuplicidadeAluno.NomeMae);

                //Verifica se não é a pessoa correta na chave unica
                if (pessoaChaveUnica != dadosDuplicidadeAluno.PessoaCorreta)
                {
                    //Remove do precadastroaluno pessoa correta que não possua inscrição 
                    rnPreCadastroAluno.RemovePreCadastroSemInscricao(contexto, dadosDuplicidadeAluno.PessoaCorreta);

                    //Leva para historico pessoa correta do precadastroaluno que possua inscrição 
                    rnPreCadastroAlunoPessoaHist.Insere(contexto, dadosDuplicidadeAluno.PessoaCorreta, dadosDuplicidadeAluno.UsuarioId);

                    //Limpa pessoa correta do precadastroaluno que possua inscrição 
                    rnPreCadastroAluno.LimpaPessoaPreCadastroComInscricao(contexto, dadosDuplicidadeAluno.PessoaCorreta, dadosDuplicidadeAluno.UsuarioId);

                    //Leva para historico pessoa que estava com a chave unica 
                    rnPreCadastroAlunoPessoaHist.Insere(contexto, dadosDuplicidadeAluno.Nome, dadosDuplicidadeAluno.DataNascimento, dadosDuplicidadeAluno.NomeMae, dadosDuplicidadeAluno.UsuarioId);

                    //Verifica se existe
                    if (pessoaChaveUnica >= 0)
                    {
                        //Atualiza com pessoa correta a chave unica na precadastroaluno
                        rnPreCadastroAluno.AtualizaPessoa(contexto, dadosDuplicidadeAluno.PessoaCorreta, dadosDuplicidadeAluno.Nome, dadosDuplicidadeAluno.DataNascimento, dadosDuplicidadeAluno.NomeMae, dadosDuplicidadeAluno.UsuarioId);
                    }
                    else
                    {
                        //Insere pessoa correta na chave unica
                        rnPreCadastroAluno.Insere(contexto, dadosDuplicidadeAluno.PessoaCorreta, dadosDuplicidadeAluno.UsuarioId);
                    }
                }

                //Verifica se matricula correta ficará ativa
                if (dadosDuplicidadeAluno.SituacaoMatricula == "Ativo")
                {
                    //Busca dados ma matricula correta
                    dadosAluno = rnAluno.ObtemDadosAluno(contexto, dadosDuplicidadeAluno.MatriculaCorreta);

                    //Verifica se vai existir alguma mudança geral
                    if (dadosAluno.SitAluno.ToUpper() != dadosDuplicidadeAluno.SituacaoMatricula.ToUpper()
                        || dadosAluno.UnidadeResponsavel != dadosDuplicidadeAluno.Censo
                        || dadosAluno.Curso != dadosDuplicidadeAluno.Curso
                        || dadosAluno.Turno != dadosDuplicidadeAluno.Turno
                        || dadosAluno.Serie != dadosDuplicidadeAluno.Serie.ToString()
                        || !rnConfirmacaoMatricula.PossuiConfirmacaoMatriculaConfirmadaPor(contexto, dadosDuplicidadeAluno.MatriculaCorreta, dadosDuplicidadeAluno.Curso, dadosDuplicidadeAluno.Serie, dadosDuplicidadeAluno.Turno, dadosDuplicidadeAluno.Ano, dadosDuplicidadeAluno.Periodo, dadosDuplicidadeAluno.Censo))
                    {
                        enturmacaoCorreta = rnMatricula.PossuiMatriculaPrincipalAtivaPor(contexto, dadosDuplicidadeAluno.MatriculaCorreta, dadosDuplicidadeAluno.Ano, dadosDuplicidadeAluno.Periodo, dadosDuplicidadeAluno.Censo, dadosDuplicidadeAluno.Curso, dadosDuplicidadeAluno.Turno, dadosDuplicidadeAluno.Serie);

                        //Verifica se a matricula correta já esta enturmada
                        if (!enturmacaoCorreta)
                        {
                            if (dadosDuplicidadeAluno.MatriculasParaCancelar != null)
                            {
                                if (dadosDuplicidadeAluno.MatriculasParaCancelar.Count > 0)
                                {
                                    //Caso não esteja verifica se existe enturmação com os dados finais para qualquer outra matricula do aluno
                                    turma = rnMatricula.ObtemTurmaMatriculaPrincipalAtivaPor(contexto, dadosDuplicidadeAluno.MatriculasParaCancelar, dadosDuplicidadeAluno.Ano, dadosDuplicidadeAluno.Periodo, dadosDuplicidadeAluno.Censo, dadosDuplicidadeAluno.Curso, dadosDuplicidadeAluno.Turno, dadosDuplicidadeAluno.Serie);
                                }
                            }
                            //cancela matricula
                            rnMatricula.CancelaMatriculaPor(contexto, dadosDuplicidadeAluno.MatriculaCorreta, dadosDuplicidadeAluno.UsuarioId);

                            //Cancela matgrade
                            rnMatgrade.CancelaMatgradePor(contexto, dadosDuplicidadeAluno.MatriculaCorreta);

                            //Cancela Confirmações
                            rnConfirmacaoMatricula.CancelaPossiveisConfirmacaoMatriculaPor(contexto, dadosDuplicidadeAluno.MatriculaCorreta, dadosDuplicidadeAluno.Ano, dadosDuplicidadeAluno.Periodo, dadosDuplicidadeAluno.UsuarioId);
                        }

                        //Verifica se a matricula correta já era da escola escolhida
                        if (dadosAluno.UnidadeResponsavel != dadosDuplicidadeAluno.Censo)
                        {
                            string turmaTransferencia;

                            if (turma.IsNullOrEmptyOrWhiteSpace())
                            {
                                //Busca primeira turma apenas para alimentar coluna da transferencia, o aluno não será enturmado
                                turmaTransferencia = rnTurma.ObtemPrimeiraTurmaPor(contexto, dadosDuplicidadeAluno.Ano, dadosDuplicidadeAluno.Periodo, dadosDuplicidadeAluno.Censo, dadosDuplicidadeAluno.Turno, dadosDuplicidadeAluno.Curso, dadosDuplicidadeAluno.Serie);
                            }
                            else
                            {
                                turmaTransferencia = turma;
                            }

                            rnTransferencia.GeraTransferenciaAluno(contexto, dadosDuplicidadeAluno.MatriculaCorreta, dadosDuplicidadeAluno.UsuarioId, dadosDuplicidadeAluno.Ano, dadosDuplicidadeAluno.Periodo, dadosDuplicidadeAluno.Censo, dadosDuplicidadeAluno.Curso, dadosDuplicidadeAluno.Turno, dadosDuplicidadeAluno.Serie, turmaTransferencia, dadosDuplicidadeAluno.Curriculo);
                        }

                        //Verifica se matricula esta ativa
                        if (dadosAluno.SitAluno.ToUpper() != "ATIVO")
                        {
                            //Caso não esteja, Reabre aluno
                            rnHCursosConcl.ReabreAlunoDuplicidade(contexto, dadosDuplicidadeAluno.MatriculaCorreta, dadosDuplicidadeAluno.Ano, dadosDuplicidadeAluno.Periodo);
                        }

                        //Criar confirmacao confirmada
                        rnConfirmacaoMatricula.InsereDuplicidade(contexto, dadosDuplicidadeAluno);

                        //Caso o aluno já estivesse enturmado, fazer enturmacao na msm turma
                        if (!enturmacaoCorreta && !turma.IsNullOrEmptyOrWhiteSpace())
                        {
                            //Enturmar aluno
                            rnMatricula.EnturmaAluno(contexto, dadosDuplicidadeAluno.MatriculaCorreta, dadosDuplicidadeAluno.Ano, dadosDuplicidadeAluno.Periodo, turma, dadosDuplicidadeAluno.Curso, dadosDuplicidadeAluno.Curriculo, dadosDuplicidadeAluno.Serie, dadosDuplicidadeAluno.UsuarioId);
                        }

                        //Verifica se vai existir alguma mudança nos dados do aluno
                        if (dadosAluno.SitAluno.ToUpper() != dadosDuplicidadeAluno.SituacaoMatricula.ToUpper()
                            || dadosAluno.UnidadeResponsavel != dadosDuplicidadeAluno.Censo
                            || dadosAluno.Curso != dadosDuplicidadeAluno.Curso
                            || dadosAluno.Turno != dadosDuplicidadeAluno.Turno
                            || dadosAluno.Serie != dadosDuplicidadeAluno.Serie.ToString())
                        {
                            rnAluno.AtualizaAlunoDuplicidade(contexto, dadosDuplicidadeAluno);
                        }
                    }
                }
                else
                {
                    if (dadosDuplicidadeAluno.MatriculasParaCancelar != null)
                    {
                        dadosDuplicidadeAluno.MatriculasParaCancelar.Add(dadosDuplicidadeAluno.MatriculaCorreta);
                    }
                    else
                    {
                        dadosDuplicidadeAluno.MatriculasParaCancelar = new List<string>();
                        dadosDuplicidadeAluno.MatriculasParaCancelar.Add(dadosDuplicidadeAluno.MatriculaCorreta);
                    }
                }

                if (dadosDuplicidadeAluno.MatriculasParaCancelar != null)
                {
                    if (dadosDuplicidadeAluno.MatriculasParaCancelar.Count > 0)
                    {
                        foreach (string matriculaCancelar in dadosDuplicidadeAluno.MatriculasParaCancelar)
                        {
                            confirmacao = new TceConfirmacaoMatricula();

                            //Retira Matriculas para cancelar da pessoaAluno 
                            rnPessoaAluno.Remove(contexto, matriculaCancelar);

                            if (rnAluno.EhAlunoAtivoPor(contexto, matriculaCancelar))
                            {
                                //cancela matricula
                                rnMatricula.CancelaMatriculaPor(contexto, matriculaCancelar, dadosDuplicidadeAluno.UsuarioId);

                                //Cancela matgrade
                                rnMatgrade.CancelaMatgradePor(contexto, matriculaCancelar);

                                confirmacao = rnConfirmacaoMatricula.ObtemUltimaConfirmacaoPor(matriculaCancelar);

                                //Cancela Confirmações
                                rnConfirmacaoMatricula.CancelaPossiveisConfirmacaoMatriculaPor(contexto, matriculaCancelar, confirmacao.Ano, confirmacao.Periodo, dadosDuplicidadeAluno.UsuarioId);

                                //Cancela Renovaçoes
                                rnRenovacao.CancelaRenovacaoPor(contexto, matriculaCancelar, dadosDuplicidadeAluno.UsuarioId);

                                //Inclui encerramento
                                rnHCursosConcl.EncerraAlunoPessoaDuplicadaPor(contexto, matriculaCancelar, confirmacao.Periodo);

                                rnAluno.CancelaAlunoPessoaDuplicadaPor(contexto, matriculaCancelar, dadosDuplicidadeAluno.UsuarioId);
                            }
                        }
                    }
                }

                if (dadosDuplicidadeAluno.PessoasParaRemover != null)
                {
                    if (dadosDuplicidadeAluno.PessoasParaRemover.Count > 0)
                    {
                        foreach (decimal pessoaRemover in dadosDuplicidadeAluno.PessoasParaRemover)
                        {
                            //Retira Pessoas erradas da pessoaAluno 
                            rnPessoaAluno.Remove(contexto, pessoaRemover);

                            //Remove do precadastroaluno pessoa errada que não possua inscrição 
                            rnPreCadastroAluno.RemovePreCadastroSemInscricao(contexto, pessoaRemover);

                            //Leva para historico pessoa errada do precadastroaluno que possua inscrição 
                            rnPreCadastroAlunoPessoaHist.Insere(contexto, pessoaRemover, dadosDuplicidadeAluno.UsuarioId);

                            //Limpa pessoa errada do precadastroaluno que possua inscrição 
                            rnPreCadastroAluno.LimpaPessoaPreCadastroComInscricao(contexto, pessoaRemover, dadosDuplicidadeAluno.UsuarioId);

                            //Trocar as pessoas dos alunos para a correta
                            rnAluno.AtualizaPessoa(contexto, dadosDuplicidadeAluno.PessoaCorreta, pessoaRemover, dadosDuplicidadeAluno.UsuarioId);

                            //Trocar as pessoa da DOCUMENTOCERTIFICACAO para a correta
                            rnDocumentoCertificacao.AtualizaPessoa(contexto, dadosDuplicidadeAluno.PessoaCorreta, pessoaRemover, dadosDuplicidadeAluno.UsuarioId);

                            rnDocumentoCertificacao.RemovePessoa(contexto, pessoaRemover);

                            //deletar field das  outras pessoas
                            FlPessoa.Remover(pessoaRemover, contexto);

                            //deletar foto das outras pessoas 
                            FotoPessoa.Remover(pessoaRemover, contexto);

                            //Atualiza pessoa na carteirinha
                            rnCarteirinha.AtualizaPessoa(contexto, dadosDuplicidadeAluno.PessoaCorreta, pessoaRemover, dadosDuplicidadeAluno.UsuarioId);
                            rnCarteirinha.Remove(contexto, pessoaRemover);

                            rnPessoaDadosBancarios.Remove(contexto, pessoaRemover);

                            //RELACAO PESSOA
                            rnRelacaoPessoa.RemovePessoaDuplicadaPor(contexto, pessoaRemover);

                            //PESSOA_RECURSOAPLICACAOPROVA
                            rnPessoaRecursoAplicacaoProva.RemovePessoaRecursoAplicacaoProvaDuplicadaPor(contexto, pessoaRemover);

                            //Deletar transtorno das outros pessoas
                            rnPessoaTranstornoAprendizagem.Remove(contexto, pessoaRemover);

                            rnGoogleEducation.RemoveEmailAluno(contexto, pessoaRemover);

                            if (!rnCartaoEstudanteFotoPessoa.PossuiFotoPessoaCorretaPor(contexto, dadosDuplicidadeAluno.PessoaCorreta))
                            {
                                rnCartaoEstudanteFotoPessoa.AtualizaPessoaCorreta(contexto, dadosDuplicidadeAluno.PessoaCorreta, pessoaRemover, dadosDuplicidadeAluno.UsuarioId);
                            }
                            else
                            {
                                rnCartaoEstudanteFotoPessoa.RemoveFotoPessoa(contexto, pessoaRemover);
                            }

                            rnAcusado.AtualizaPessoaCorreta(contexto, dadosDuplicidadeAluno.PessoaCorreta, pessoaRemover, dadosDuplicidadeAluno.UsuarioId);
                                                        
                            rnVitima.AtualizaPessoaCorreta(contexto, dadosDuplicidadeAluno.PessoaCorreta, pessoaRemover, dadosDuplicidadeAluno.UsuarioId);
                           
                            rnHistoricoAlteracaoAlunoCampos.Remove(contexto, pessoaRemover);

                            rnHistoricoAlteracaoAluno.Remove(contexto, pessoaRemover);

                            //Deletar outras pessoas
                            this.Remove(contexto, pessoaRemover);
                        }
                    }
                }

                //Retira pessoa correta e matricula correta da pessoa aluno
                rnPessoaAluno.RemovePorPessoaOuAluno(contexto, dadosDuplicidadeAluno.PessoaCorreta, dadosDuplicidadeAluno.MatriculaCorreta);

                //Insere pessoa correta com matricula correta na pessoaaluno
                rnPessoaAluno.Insere(contexto, dadosDuplicidadeAluno.MatriculaCorreta, dadosDuplicidadeAluno.UsuarioId);
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

        public void InserePessoaExterna(DataContext ctx, LyPessoa pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO LY_PESSOA 
                                            (PESSOA, 
                                                NOME_COMPL, 
                                                CPF, 
                                                CEP, 
                                                END_MUNICIPIO, 
                                                ENDERECO, 
                                                END_NUM, 
                                                END_COMPL, 
				                                END_PAIS,
                                                BAIRRO, 
                                                E_MAIL, 
                                                E_MAIL_INTERNO,
                                                FONE, 
                                                CELULAR,                                                
				                                SEXO,
				                                DT_NASC,
				                                EST_CIVIL,
                                                STAMP_ATUALIZACAO,
                                                USUARIOID,
                                                DATACADASTRO,
                                                DATAALTERACAO) 
                                VALUES      ( (SELECT ISNULL(MAX(PESSOA), 0) + 1 
                                                FROM   LY_PESSOA WITH (UPDLOCK)), 
                                                @NOME_COMPL, 
                                                @CPF, 
                                                @CEP, 
                                                @END_MUNICIPIO, 
                                                @ENDERECO, 
                                                @END_NUM, 
                                                @END_COMPL, 
				                                @END_PAIS,
                                                @BAIRRO, 
                                                @E_MAIL, 
                                                @E_MAIL_INTERNO, 
                                                @FONE, 
                                                @CELULAR,                                             
				                                @SEXO,
				                                @DT_NASC,
				                                @EST_CIVIL,
                                                @STAMP_ATUALIZACAO,
                                                @USUARIOID,
                                                @DATACADASTRO,
                                                @DATAALTERACAO) 

                                SELECT  PESSOA
                                    FROM    LY_PESSOA
                                    WHERE   CPF = @CPF
                                            AND STAMP_ATUALIZACAO = @STAMP_ATUALIZACAO ";

            contextQuery.Parameters.Add("@NOME_COMPL", pessoa.Nome_compl);
            contextQuery.Parameters.Add("@CPF", pessoa.Cpf);
            contextQuery.Parameters.Add("@CEP", pessoa.Cep);
            contextQuery.Parameters.Add("@END_MUNICIPIO", pessoa.End_municipio);
            contextQuery.Parameters.Add("@ENDERECO", pessoa.Endereco);
            contextQuery.Parameters.Add("@END_NUM", pessoa.End_num);
            contextQuery.Parameters.Add("@END_COMPL", pessoa.End_compl);
            contextQuery.Parameters.Add("@BAIRRO", pessoa.Bairro);
            contextQuery.Parameters.Add("@E_MAIL", pessoa.E_mail);
            contextQuery.Parameters.Add("@E_MAIL_INTERNO", pessoa.E_mail_interno);
            contextQuery.Parameters.Add("@FONE", pessoa.Fone);
            contextQuery.Parameters.Add("@CELULAR", pessoa.Celular);
            contextQuery.Parameters.Add("@END_PAIS", pessoa.End_pais);
            contextQuery.Parameters.Add("@SEXO", pessoa.Sexo);
            contextQuery.Parameters.Add("@DT_NASC", pessoa.Dt_nasc);
            contextQuery.Parameters.Add("@EST_CIVIL", pessoa.Est_civil);
            contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", DateTime.Now);
            contextQuery.Parameters.Add("@USUARIOID", pessoa.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

            pessoa.Pessoa = Convert.ToInt32(ctx.GetReturnValue(contextQuery));
        }

        public void AtualizaPessoaExterna(DataContext ctx, LyPessoa pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_PESSOA 
                                        SET    NOME_COMPL = @NOME_COMPL, 
                                               CPF = @CPF, 
                                               CEP = @CEP, 
                                               END_MUNICIPIO = @END_MUNICIPIO, 
                                               ENDERECO = @ENDERECO, 
                                               END_NUM = @END_NUM, 
                                               END_COMPL = @END_COMPL, 
                                               BAIRRO = @BAIRRO, 
                                               E_MAIL_INTERNO = @E_MAIL_INTERNO,
                                               E_MAIL = @E_MAIL, 
                                               FONE = @FONE, 
                                               CELULAR = @CELULAR, 
											   END_PAIS = @END_PAIS,
											   SEXO = @SEXO,
											   DT_NASC = @DT_NASC,
											   EST_CIVIL = @EST_CIVIL,
                                               STAMP_ATUALIZACAO = @STAMP_ATUALIZACAO,
                                               DATAALTERACAO = @DATAALTERACAO,
                                               USUARIOID = @USUARIOID 
                                        WHERE  PESSOA = @PESSOA ";

            contextQuery.Parameters.Add("@NOME_COMPL", pessoa.Nome_compl.ToUpper());
            contextQuery.Parameters.Add("@CPF", pessoa.Cpf);
            contextQuery.Parameters.Add("@CEP", pessoa.Cep);
            contextQuery.Parameters.Add("@END_MUNICIPIO", pessoa.End_municipio);
            contextQuery.Parameters.Add("@ENDERECO", pessoa.Endereco);
            contextQuery.Parameters.Add("@END_NUM", pessoa.End_num);
            contextQuery.Parameters.Add("@END_COMPL", pessoa.End_compl);
            contextQuery.Parameters.Add("@BAIRRO", pessoa.Bairro);
            contextQuery.Parameters.Add("@E_MAIL_INTERNO", pessoa.E_mail_interno);
            contextQuery.Parameters.Add("@E_MAIL", pessoa.E_mail);
            contextQuery.Parameters.Add("@FONE", pessoa.Fone);
            contextQuery.Parameters.Add("@CELULAR", pessoa.Celular);
            contextQuery.Parameters.Add("@END_PAIS", pessoa.End_pais);
            contextQuery.Parameters.Add("@SEXO", pessoa.Sexo);
            contextQuery.Parameters.Add("@DT_NASC", pessoa.Dt_nasc);
            contextQuery.Parameters.Add("@EST_CIVIL", pessoa.Est_civil);
            contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", DateTime.Now);
            contextQuery.Parameters.Add("@USUARIOID", pessoa.UsuarioId);
            contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);
            contextQuery.Parameters.Add("@PESSOA", pessoa.Pessoa);

            ctx.ApplyModifications(contextQuery);
        }

        public void AlteraIdFuncional(decimal pessoa, string usuarioId, int? idFuncional)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"UPDATE LY_PESSOA SET 
                                             IDFUNCIONAL = @IDFUNCIONAL,
                                             USUARIOID = @USUARIOID, 
                                             DATAALTERACAO = GETDATE()
                                        WHERE PESSOA = @PESSOA";

                contextQuery.Parameters.Add("@IDFUNCIONAL", idFuncional);
                contextQuery.Parameters.Add("@USUARIOID", usuarioId);
                contextQuery.Parameters.Add("@PESSOA", pessoa);

                ctx.ApplyModifications(contextQuery);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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
                ctx.Dispose();
            }
        }

        public ValidacaoDados ValidaAlteracaoIdFuncional(decimal pessoa, int? idFuncional, bool possuiId, string usuarioResponsavel)
        {
            DataContext contexto = null;
            RN.Validacao rnValidacao = new Validacao();
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (pessoa <= 0)
            {
                mensagens.Add("O campo PESSOA é de preenchimento obrigatório.");
            }

            if (usuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo USUARIO RESPONSAVEL é de preenchimento obrigatório.");
            }

            if (possuiId)
            {
                if (idFuncional == null || idFuncional <= 0)
                {
                    mensagens.Add("O campo ID FUNCIONAL é de preenchimento obrigatório.");
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (possuiId)
                    {
                        //Valida ID FUNCIONAL existente                      
                        if (this.PossuiOutroIdFuncionalPor(contexto, Convert.ToInt32(idFuncional), pessoa))
                        {
                            mensagens.Add("Já existe uma pessoa cadastrada com esse número de ID FUNCIONAL.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (contexto != null)
                    {
                        contexto.Dispose();
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;

        }

        public int? ObtemIdFuncionalPor(decimal pessoa)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingNoLock();
            int? idFuncional = 0;

            try
            {
                idFuncional = this.ObtemIdFuncionalPor(contexto, pessoa);
                return idFuncional;
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

        public int? ObtemIdFuncionalPor(DataContext contexto, decimal pessoa)
        {
            int? idFuncional = 0;
            SqlDataReader reader = null;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT IDFUNCIONAL
                                 FROM LY_PESSOA 
                                    WHERE PESSOA = @PESSOA "
                };
                contextQuery.Parameters.Add("@PESSOA", pessoa);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    if (reader["IDFUNCIONAL"] != DBNull.Value)
                    {
                        idFuncional = Convert.ToInt32(reader["IDFUNCIONAL"]);
                    }
                    else
                    {
                        idFuncional = null;
                    }
                }

                return idFuncional;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public DateTime ObtemDataNascimentoPor(DataContext contexto, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            DateTime dataNascimento = new DateTime();

            try
            {
                contextQuery.Command = @" SELECT DATANASCIMENTO
                                          FROM  LY_PESSOA
                                          WHERE PESSOA = @PESSOA ";

                contextQuery.Parameters.Add("@PESSOA", pessoa);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    if (reader["DATANASCIMENTO"] != DBNull.Value)
                    {
                        dataNascimento = Convert.ToDateTime(reader["DATANASCIMENTO"]);
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return dataNascimento;
        }

        public DadosEncaminhamentoEspecial ObtemDadosEncaminhamentoEspecialPor(DataContext contexto, string nome, string nomeMae, DateTime dataNascimento)
        {
            DadosEncaminhamentoEspecial dados = new DadosEncaminhamentoEspecial();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT   P.PESSOA,
	                                               P.NOME_COMPL, 
                                                   P.DT_NASC, 
                                                   P.NOME_MAE, 
                                                   P.SEXO, 
                                                   P.NOME_PAI, 
                                                   P.CEP, 
                                                   P.ENDERECO, 
                                                   P.END_NUM, 
                                                   P.END_COMPL, 
                                                   P.END_MUNICIPIO, 
                                                   M.NOME AS DESCRICAOMUNICIPIO, 
                                                   M.UF, 
                                                   P.BAIRRO, 
                                                   P.CPF,
                                                   P.NECESSIDADEESPECIALID,
	                                               PC.PRECADASTROALUNOID,
	                                               PA.ALUNO
                                            FROM   LY_PESSOA P (NOLOCK) 
                                                   LEFT JOIN HADES.DBO.TCE_MUNICIPIO M (NOLOCK) 
                                                           ON P.END_MUNICIPIO = M.ID_MUNICIPIO 
	                                               LEFT JOIN Matricula.PRECADASTROALUNO pc (NOLOCK) 
			                                               ON PC.PESSOAID = P.PESSOA
	                                               LEFT JOIN Matricula.PESSOAALUNO pa (NOLOCK)
			                                               ON pa.PESSOAID = P.PESSOA
                                            WHERE  P.NOME_COMPL = @NOME 
                                                   AND P.DT_NASC = @DT_NASC 
                                                   AND P.NOME_MAE = @NOMEMAE  ";

                contextQuery.Parameters.Add("@NOME", TechneDbType.T_ALFALARGE, nome);
                contextQuery.Parameters.Add("@DT_NASC", TechneDbType.T_DATA, dataNascimento);
                contextQuery.Parameters.Add("@NOMEMAE", TechneDbType.T_ALFALARGE, nomeMae);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dados.Pessoa = reader["PESSOA"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["PESSOA"]);
                    dados.Nome = Convert.ToString(reader["NOME_COMPL"]);
                    dados.DataNascimento = reader["DT_NASC"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DT_NASC"]);
                    dados.NomeMae = Convert.ToString(reader["NOME_MAE"]);
                    dados.Sexo = Convert.ToString(reader["SEXO"]);
                    dados.NomePai = Convert.ToString(reader["NOME_PAI"]);
                    dados.Cep = Convert.ToString(reader["CEP"]);
                    dados.Endereco = Convert.ToString(reader["ENDERECO"]);
                    dados.NumeroEndereco = Convert.ToString(reader["END_NUM"]);
                    dados.ComplementoEndereco = Convert.ToString(reader["END_COMPL"]);
                    dados.MunicipioEndereco = Convert.ToString(reader["END_MUNICIPIO"]);
                    dados.DescricaoMunicipioEndereco = Convert.ToString(reader["DESCRICAOMUNICIPIO"]);
                    dados.UfEndereco = Convert.ToString(reader["UF"]);
                    dados.Bairro = Convert.ToString(reader["BAIRRO"]);
                    dados.Cpf = Convert.ToString(reader["CPF"]);
                    dados.NecessidadeEspecialId = reader["NECESSIDADEESPECIALID"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["NECESSIDADEESPECIALID"]);
                    dados.PreCadastroAlunoId = reader["PRECADASTROALUNOID"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["PRECADASTROALUNOID"]);
                }

                dados.MaeNãoDeclarada = dados.NomeMae == "NÃO DECLARADA" ? true : false;
                dados.PaiNãoDeclarado = dados.NomePai == "NÃO DECLARADO" ? true : false;

                return dados;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public RN.DTOs.DadosExterno ObtemDadosExternoPor(DataContext ctx, string cpf)
        {
            RN.DTOs.DadosExterno recurso = new RN.DTOs.DadosExterno();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT PESSOA, 
                                   NOME_COMPL, 
                                   CPF, 
                                   CEP, 
                                   END_MUNICIPIO, 
                                   ENDERECO, 
                                   END_NUM, 
                                   END_COMPL, 
                                   BAIRRO, 
                                   E_MAIL, 
                                   E_MAIL_INTERNO,
                                   FONE, 
                                   CELULAR ,
                                   END_PAIS,
								   SEXO ,
								   DT_NASC ,
								   EST_CIVIL 
                            FROM   LY_PESSOA (NOLOCK) 
                            WHERE  CPF = @CPF ";

                contextQuery.Parameters.Add("@CPF", TechneDbType.T_CPF, cpf);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    recurso.PessoaId = Convert.ToInt32(reader["PESSOA"]);
                    recurso.Nome = Convert.ToString(reader["NOME_COMPL"]);
                    recurso.Cpf = Convert.ToString(reader["CPF"]);
                    recurso.Cep = Convert.ToString(reader["CEP"]);
                    recurso.Municipio = Convert.ToString(reader["END_MUNICIPIO"]);
                    recurso.Endereco = Convert.ToString(reader["ENDERECO"]);
                    recurso.Numero = Convert.ToString(reader["END_NUM"]);
                    recurso.Complemento = Convert.ToString(reader["END_COMPL"]);
                    recurso.Bairro = Convert.ToString(reader["BAIRRO"]);
                    recurso.Email = Convert.ToString(reader["E_MAIL_INTERNO"]);
                    recurso.EmailAlternativo = Convert.ToString(reader["E_MAIL"]);
                    recurso.Telefone = Convert.ToString(reader["FONE"]);
                    recurso.Celular = Convert.ToString(reader["CELULAR"]);
                    recurso.Sexo = Convert.ToString(reader["SEXO"]);
                    recurso.EstadoCivl = Convert.ToString(reader["EST_CIVIL"]);
                    recurso.PaisEndereco = Convert.ToString(reader["END_PAIS"]);
                    if (reader["DT_NASC"] != DBNull.Value)
                    {
                        recurso.DataNascimento = Convert.ToDateTime(reader["DT_NASC"]);
                    }
                    recurso.Bloqueado = false;
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return recurso;
        }

        public DTOs.DadosAlunoCertificacao ObtemDadosCertificacaoPor(decimal pessoa)
        {
            DTOs.DadosAlunoCertificacao dadosAlunoCertificacao = new DadosAlunoCertificacao();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT PESSOA,
		                                    DT_NASC,
		                                    NOME_COMPL,
		                                    NOME_MAE,
		                                    NOME_PAI,
		                                    MUNICIPIO_NASC,
                                            PAIS_NASC,
		                                    RG_TIPO,
		                                    RG_NUM,
		                                    RG_EMISSOR,
		                                    RG_UF,
		                                    NACIONALIDADE,
		                                    RG_DTEXP
                                    FROM LY_PESSOA (NOLOCK)
                                    WHERE PESSOA = @PESSOA ";

                contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, pessoa);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosAlunoCertificacao.Pessoa = Convert.ToInt32(reader["PESSOA"]);
                    dadosAlunoCertificacao.DataNascimento = reader["DT_NASC"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DT_NASC"]);
                    dadosAlunoCertificacao.Nome = Convert.ToString(reader["NOME_COMPL"]);
                    dadosAlunoCertificacao.NomeMae = Convert.ToString(reader["NOME_MAE"]);
                    dadosAlunoCertificacao.NomePai = Convert.ToString(reader["NOME_PAI"]);
                    dadosAlunoCertificacao.MunicipioNascimento = Convert.ToString(reader["MUNICIPIO_NASC"]);
                    dadosAlunoCertificacao.PaisNascimento = Convert.ToString(reader["PAIS_NASC"]);
                    dadosAlunoCertificacao.RgTipo = Convert.ToString(reader["RG_TIPO"]);
                    dadosAlunoCertificacao.Nacionalidade = Convert.ToString(reader["NACIONALIDADE"]);

                    if (dadosAlunoCertificacao.RgTipo == "RG")
                    {
                        dadosAlunoCertificacao.RgNumero = Convert.ToString(reader["RG_NUM"]);
                        dadosAlunoCertificacao.RgEmissor = Convert.ToString(reader["RG_EMISSOR"]);
                        dadosAlunoCertificacao.RgUf = Convert.ToString(reader["RG_UF"]);
                        dadosAlunoCertificacao.RgDataExpedicao = reader["RG_DTEXP"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["RG_DTEXP"]);
                    }
                    else
                    {
                        dadosAlunoCertificacao.RgTipo = string.Empty;
                    }
                }

                return dadosAlunoCertificacao;
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
                if (reader != null)
                {
                    reader.Close();
                }
                contexto.Dispose();
            }
        }

        public ValidacaoDados ValidaDadosCertificacao(DTOs.DadosAlunoCertificacao dadosAlunoCertificacao)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            Regex regex = new Regex(@"\s{2,}");
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosAlunoCertificacao == null)
            {
                return validacaoDados;
            }

            //Tipo para documentos sempre RG
            dadosAlunoCertificacao.RgTipo = "RG";

            if (dadosAlunoCertificacao.Pessoa <= 0)
            {
                mensagens.Add("Campo PESSOA é obrigatório.");
            }

            if (dadosAlunoCertificacao.DataNascimento == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DE NASCIMENTO é obrigatório.");
            }
            else
            {
                int idade = Utils.CalcularIdade(dadosAlunoCertificacao.DataNascimento);

                if (idade > 110)
                {
                    mensagens.Add("A idade não pode ser superior a 110 anos. Favor verificar a Data de Nascimento.");
                }
            }

            if (dadosAlunoCertificacao.Nome.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME é obrigatório.");
            }
            else
            {
                dadosAlunoCertificacao.Nome = dadosAlunoCertificacao.Nome.RetiraEspacosDuplos();

                /// Validaçoes de nome do aluno 
                if (dadosAlunoCertificacao.Nome.Length < 5)
                {
                    mensagens.Add("Campo NOME DO ALUNO deve conter pelo menos cinco letras!");
                }

                if (!string.IsNullOrEmpty(dadosAlunoCertificacao.Nome)
                    && !Validacao.SomenteLetras(dadosAlunoCertificacao.Nome))
                {
                    mensagens.Add("Campo NOME DO ALUNO não pode conter números.");
                }

                var palavras = dadosAlunoCertificacao.Nome.CountWords();
                var contemRepeticao = RN.Validacao.ContemRepeticao(dadosAlunoCertificacao.Nome, 3);
                var nomeInvalido = TextValidator.HasForbiddenWords(dadosAlunoCertificacao.Nome, new PalavrasProibidasEmNomes());
                if (Validacao.contemNumeros(dadosAlunoCertificacao.Nome) == false)
                {
                    if (palavras < 2)
                    {
                        mensagens.Add("Por favor informar, informar nome e sobrenome no campo NOME.");
                    }

                    if (contemRepeticao)
                    {
                        mensagens.Add("Campo NOME possui inconsistência por repetição excessiva de letras. Favor corrigir a informação.");
                    }

                    if (nomeInvalido)
                    {
                        mensagens.Add("Campo NOME possui inconsistência por não representar um nome válido. Favor corrigir a informação.");
                    }
                }

                regex = new Regex(@"\s{2,}");
                string NomeCompl = regex.Replace(dadosAlunoCertificacao.Nome.Trim().ToUpper(), " ");
                var contemApostrofeRep = Validacao.substitueApostrofe(NomeCompl);
                if (contemApostrofeRep)
                {
                    mensagens.Add("Campo NOME DO ALUNO possui inconsistência por repetição excessiva de apóstrofes. Favor corrigir a informação.");
                }
            }

            if (dadosAlunoCertificacao.NomeMae.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME DA MÃE é obrigatório.");
            }
            else
            {
                dadosAlunoCertificacao.NomeMae = dadosAlunoCertificacao.NomeMae.RetiraEspacosDuplos();

                //Validaçoes de nome da mae
                var palavras = dadosAlunoCertificacao.NomeMae.CountWords();
                var contemRepeticao = RN.Validacao.ContemRepeticao(dadosAlunoCertificacao.NomeMae, 3);
                var nomeInvalido = TextValidator.HasForbiddenWords(dadosAlunoCertificacao.NomeMae, new PalavrasProibidasEmNomes());
                if (!Validacao.contemNumeros(dadosAlunoCertificacao.NomeMae))
                {
                    if (palavras < 2)
                    {
                        mensagens.Add("Por favor informar, informar nome e sobrenome no campo NOME DA MÃE.");
                    }

                    if (contemRepeticao)
                    {
                        mensagens.Add("Campo NOME DA MÃE possui inconsistência por repetição excessiva de letras. Favor corrigir a informação.");
                    }

                    if (nomeInvalido)
                    {
                        mensagens.Add("Campo NOME DA MÃE possui inconsistência por não representar um nome válido. Favor corrigir a informação.");
                    }
                }
                else if (!Validacao.SomenteLetras(dadosAlunoCertificacao.NomeMae))
                {
                    mensagens.Add("Campo NOME DA MÃE não pode conter números.");
                }

                string NomeMae = regex.Replace(dadosAlunoCertificacao.NomeMae.Trim().ToUpper(), " ");
                var contemApostrofeRepMae = Validacao.substitueApostrofe(NomeMae);
                if (contemApostrofeRepMae)
                {
                    mensagens.Add("Campo NOME DA MÃE possui inconsistência por repetição excessiva de apóstrofes. Favor corrigir a informação.");
                }
            }

            if (dadosAlunoCertificacao.NomePai.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME DO PAI é obrigatório.");
            }
            else
            {
                dadosAlunoCertificacao.NomePai = dadosAlunoCertificacao.NomePai.RetiraEspacosDuplos();

                //Validaçoes de nome do pai
                var palavras = dadosAlunoCertificacao.NomePai.CountWords();
                var contemRepeticao = RN.Validacao.ContemRepeticao(dadosAlunoCertificacao.NomePai, 3);
                var nomeInvalido = TextValidator.HasForbiddenWords(dadosAlunoCertificacao.NomePai, new PalavrasProibidasEmNomes());
                if (!Validacao.contemNumeros(dadosAlunoCertificacao.NomePai))
                {
                    if (palavras < 2)
                    {
                        mensagens.Add("Por favor informar, informar nome e sobrenome no campo NOME DO PAI.");
                    }

                    if (contemRepeticao)
                    {
                        mensagens.Add("Campo NOME DO PAI possui inconsistência por repetição excessiva de letras. Favor corrigir a informação.");
                    }

                    if (nomeInvalido)
                    {
                        mensagens.Add("Campo NOME DO PAI possui inconsistência por não representar um nome válido. Favor corrigir a informação.");
                    }
                }
                else if (!Validacao.SomenteLetras(dadosAlunoCertificacao.NomePai))
                {
                    mensagens.Add("Campo NOME DO PAI não pode conter números.");
                }

                string NomePai = regex.Replace(dadosAlunoCertificacao.NomePai.Trim().ToUpper(), " ");
                var contemApostrofeRepPai = Validacao.substitueApostrofe(NomePai);
                if (contemApostrofeRepPai)
                {
                    mensagens.Add("Campo NOME DO PAI possui inconsistência por repetição excessiva de apóstrofes. Favor corrigir a informação.");
                }
            }

            if (dadosAlunoCertificacao.RgNumero.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NÚMERO DO RG é obrigatório.");
            }
            else
            {
                var rg = Utils.RetirarMascara(dadosAlunoCertificacao.RgNumero);

                if (rg.Length < 5)
                {
                    mensagens.Add("O NÚMERO DO RG deve conter no mínimo cinco dígitos!");
                }

                if (dadosAlunoCertificacao.RgEmissor == "DETRAN" && dadosAlunoCertificacao.RgUf == "RJ")
                {
                    if (!Validacao.ValidaNumerosInteirosPositivos(dadosAlunoCertificacao.RgNumero))
                    {
                        mensagens.Add("O NÚMERO DO RG DETRAN deve conter só números inteiros.");
                    }
                }
            }

            if (dadosAlunoCertificacao.RgEmissor.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ORGÃO EMISSOR DO RG é obrigatório.");
            }

            if (dadosAlunoCertificacao.RgDataExpedicao == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA EXPEDIÇÃO DO RG é obrigatório.");
            }

            if (dadosAlunoCertificacao.RgUf.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo UF DO RG é obrigatório.");
            }

            if (dadosAlunoCertificacao.Nacionalidade.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NACIONALIDADE é obrigatório.");
            }

            if (dadosAlunoCertificacao.MunicipioNascimento.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo MUNICIPIO DE NASCIMENTO é obrigatório.");
            }

            if (dadosAlunoCertificacao.UsuarioResponsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUÁRIO RESPONSÁVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Valida Nome, mae e data de nascimento existente para outra pessoa
                    if (this.PossuiOutroNomeMaeDataNascimentoPor(contexto, dadosAlunoCertificacao.Nome, dadosAlunoCertificacao.NomeMae, dadosAlunoCertificacao.DataNascimento, dadosAlunoCertificacao.Pessoa))
                    {
                        mensagens.Add("Existe outro cadastro com este Nome / Data de nascimento / Nome da mãe. Favor utilizar a tela de DUPLICIDADE DE ALUNO para tratar o aluno (corrigindo estes campos) antes da atualização de seus demais dados.");
                    }
                }
                catch (Exception ex)
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (contexto != null)
                    {
                        contexto.Dispose();
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Distinct().Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void AtualizaDadosCertificacao(DTOs.DadosAlunoCertificacao dadosAlunoCertificaca)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE LY_PESSOA 
                                        SET    DT_NASC = @DT_NASC, 
                                               NOME_COMPL = @NOME_COMPL, 
                                               NOME_MAE = @NOME_MAE, 
                                               NOME_PAI = @NOME_PAI, 
                                               MUNICIPIO_NASC = @MUNICIPIO_NASC,
                                               PAIS_NASC = @PAIS_NASC, 
                                               RG_TIPO = @RG_TIPO, 
                                               RG_NUM = @RG_NUM, 
                                               RG_EMISSOR = @RG_EMISSOR, 
                                               RG_UF = @RG_UF, 
                                               RG_DTEXP = @RG_DTEXP,
                                               NACIONALIDADE = @NACIONALIDADE,
                                               USUARIOID = @USUARIOID, 
                                               DATAALTERACAO = @DATAALTERACAO ,
                                               STAMP_ATUALIZACAO = @DATAALTERACAO
                                        WHERE  PESSOA = @PESSOA  ";

                contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, dadosAlunoCertificaca.Pessoa);
                contextQuery.Parameters.Add("@DT_NASC", TechneDbType.T_DATA, dadosAlunoCertificaca.DataNascimento);
                contextQuery.Parameters.Add("@NOME_COMPL", TechneDbType.T_ALFALARGE, dadosAlunoCertificaca.Nome);
                contextQuery.Parameters.Add("@NOME_MAE", TechneDbType.T_ALFALARGE, dadosAlunoCertificaca.NomeMae);
                contextQuery.Parameters.Add("@NOME_PAI", TechneDbType.T_ALFALARGE, dadosAlunoCertificaca.NomePai);
                contextQuery.Parameters.Add("@MUNICIPIO_NASC", TechneDbType.T_CODIGO, dadosAlunoCertificaca.MunicipioNascimento);
                contextQuery.Parameters.Add("@PAIS_NASC", TechneDbType.T_CODIGO, dadosAlunoCertificaca.PaisNascimento);
                contextQuery.Parameters.Add("@RG_TIPO", TechneDbType.T_ALFASMALL, dadosAlunoCertificaca.RgTipo);
                contextQuery.Parameters.Add("@RG_NUM", TechneDbType.T_CODIGO, dadosAlunoCertificaca.RgNumero);
                contextQuery.Parameters.Add("@RG_EMISSOR", TechneDbType.T_ALFASMALL, dadosAlunoCertificaca.RgEmissor);
                contextQuery.Parameters.Add("@RG_UF", TechneDbType.T_UF, dadosAlunoCertificaca.RgUf);
                contextQuery.Parameters.Add("@RG_DTEXP", TechneDbType.T_DATA, dadosAlunoCertificaca.RgDataExpedicao);
                contextQuery.Parameters.Add("@NACIONALIDADE", TechneDbType.T_ALFASMALL, dadosAlunoCertificaca.Nacionalidade);
                contextQuery.Parameters.Add("@USUARIOID", dadosAlunoCertificaca.UsuarioResponsavel);
                contextQuery.Parameters.Add("@DATAALTERACAO", SqlDbType.DateTime, DateTime.Now);

                contexto.ApplyModifications(contextQuery);
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

        public string ObtemIdVinculoPor(decimal pessoa, int vinculo)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            string idvinculo = string.Empty;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT IDVINCULO
                                        FROM VW_FUNCIONARIOS D		                                        
                                        WHERE D.PESSOA = @PESSOA 
                                        AND D.VINCULO = @VINCULO ";

                contextQuery.Parameters.Add("@PESSOA", pessoa);
                contextQuery.Parameters.Add("@VINCULO", vinculo);

                idvinculo = contexto.GetReturnValue<string>(contextQuery);
            }
            catch (Exception ex)
            {
                contexto.Abandon();
                string mensagem = string.Empty;
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

            return idvinculo;
        }

        public RecursosHumanos.DTO.DadosCadastraisAluno ObtemDadosCadastraisAluno(int pessoa)
        {
            RecursosHumanos.DTO.DadosCadastraisAluno dadosCadastraisAluno = new RecursosHumanos.DTO.DadosCadastraisAluno();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();

            try
            {
                dadosCadastraisAluno = this.ObtemDadosCadastraisAluno(ctx, pessoa);
                return dadosCadastraisAluno;
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                ctx.Abandon();
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
                ctx.Dispose();
            }
        }

        public RecursosHumanos.DTO.DadosCadastraisAluno ObtemDadosCadastraisAluno(DataContext ctx, int pessoa)
        {
            RecursosHumanos.DTO.DadosCadastraisAluno dadosAluno = new RecursosHumanos.DTO.DadosCadastraisAluno();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @"  SELECT P.*, 
							C.CODIGO_UF, 
							C.COD_CARTORIO, 
							C.CODIGO_MUNICIPIO,  
							c.nome_cartorio,
							M.NOME AS NOME_MUNICIPIO,
							M.UF AS UF_END,
                            mnasc.UF AS UF_NASC,
                            mnasc.NOME AS NOME_MUNICIPIO_NASC,
							FL_FIELD_01,
							FL_FIELD_02,
							FL_FIELD_07,
							FL_FIELD_09,
							pa.NOME as Pais_nasc_nome,
							c.municipio as NOME_MUNICIPIO_CART,
							c.uf as UF_cart
                        FROM LY_PESSOA P
                            LEFT JOIN CARTORIO C ON c.cod_cartorio = P.id_cartorio
                            LEFT JOIN HADES..TCE_MUNICIPIO m ON p.End_municipio = m.ID_MUNICIPIO
							LEFT JOIN HADES..TCE_MUNICIPIO mnasc ON p.MUNICIPIO_NASC = mnasc.ID_MUNICIPIO
							LEFT JOIN LY_FL_PESSOA FL ON P.PESSOA = FL.PESSOA
							left join PAISES pa on p.PAIS_NASC = pa.CODIGO
                        WHERE P.PESSOA = @PESSOA ";

                contextQuery.Parameters.Add("@PESSOA", SqlDbType.VarChar, pessoa);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dadosAluno.Pessoa = Convert.ToDecimal(reader["Pessoa"]);
                    dadosAluno.Nome_compl = Convert.ToString(reader["Nome_compl"]);
                    dadosAluno.PreNomeSocial = Convert.ToString(reader["PRE_NOME_SOCIAL"]);
                    dadosAluno.Sexo = Convert.ToString(reader["Sexo"]);
                    dadosAluno.Etnia = Convert.ToString(reader["Etnia"]);
                    dadosAluno.Est_civil = Convert.ToString(reader["Est_civil"]);
                    dadosAluno.Pais_nasc = Convert.ToString(reader["Pais_nasc"]);
                    dadosAluno.Pais_nasc_nome = Convert.ToString(reader["Pais_nasc_nome"]);
                    dadosAluno.Nacionalidade = Convert.ToString(reader["Nacionalidade"]);
                    dadosAluno.Municipio_nasc = Convert.ToString(reader["Municipio_nasc"]);
                    dadosAluno.Municipio_nasc_nome = Convert.ToString(reader["NOME_MUNICIPIO_NASC"]);
                    dadosAluno.UF_nasc = Convert.ToString(reader["UF_NASC"]);
                    dadosAluno.NomeMae = Convert.ToString(reader["NOME_MAE"]);
                    dadosAluno.MaeFalecida = Convert.ToString(reader["MAE_FALECIDA"]);
                    dadosAluno.MaeCpf = Convert.ToString(reader["MAE_CPF"]);
                    dadosAluno.MaeTelefone = Convert.ToString(reader["MAE_TELEFONE"]);
                    dadosAluno.NomePai = Convert.ToString(reader["NOME_PAI"]);
                    dadosAluno.PaiFalecido = Convert.ToString(reader["PAI_FALECIDO"]);
                    dadosAluno.PaiCpf = Convert.ToString(reader["PAI_CPF"]);
                    dadosAluno.PaiTelefone = Convert.ToString(reader["PAI_TELEFONE"]);
                    dadosAluno.Responsavel = Convert.ToString(reader["RESPONSAVEL"]);
                    dadosAluno.RespNomeCompl = Convert.ToString(reader["RESP_NOME_COMPL"]);
                    dadosAluno.RespCpf = Convert.ToString(reader["RESP_CPF"]);
                    dadosAluno.RespFone = Convert.ToString(reader["RESP_FONE"]);
                    dadosAluno.Cep = Convert.ToString(reader["Cep"]);
                    dadosAluno.End_UF = Convert.ToString(reader["UF_END"]);
                    dadosAluno.End_municipio = Convert.ToString(reader["End_municipio"]);
                    dadosAluno.End_NomeMunicipio = Convert.ToString(reader["NOME_MUNICIPIO"]);
                    dadosAluno.Endereco = Convert.ToString(reader["Endereco"]);
                    dadosAluno.End_num = Convert.ToString(reader["End_num"]);
                    dadosAluno.End_compl = Convert.ToString(reader["End_compl"]);
                    dadosAluno.Bairro = Convert.ToString(reader["Bairro"]);
                    dadosAluno.ZonaResidencial = Convert.ToString(reader["FL_FIELD_01"]);

                    if (reader["Dt_nasc"] != DBNull.Value)
                    {
                        dadosAluno.Dt_nasc = Convert.ToDateTime(reader["Dt_nasc"]);
                    }
                    if (reader["QT_FILHOS"] != DBNull.Value)
                    {
                        dadosAluno.QtFilhos = Convert.ToDecimal(reader["QT_FILHOS"]);
                    }

                    dadosAluno.AreaAssentamento = Convert.ToString(reader["AREA_ASSENTAMENTO"]);
                    dadosAluno.AreaQuilombos = Convert.ToString(reader["AREA_QUILOMBOS"]);
                    dadosAluno.AreaTradicional = Convert.ToString(reader["AREA_TRADICIONAL"]);
                    dadosAluno.TerraIndigena = Convert.ToString(reader["TERRA_INDIGENA"]);
                    dadosAluno.Fone = Convert.ToString(reader["Fone"]);
                    dadosAluno.Celular = Convert.ToString(reader["Celular"]);
                    dadosAluno.E_mail = Convert.ToString(reader["E_mail"]);
                    dadosAluno.Cpf = Convert.ToString(reader["Cpf"]);
                    dadosAluno.Rg_tipo = Convert.ToString(reader["Rg_tipo"]);
                    dadosAluno.Rg_num = Convert.ToString(reader["Rg_num"]);
                    dadosAluno.ComplementoIdentidade = Convert.ToString(reader["FL_FIELD_07"]);
                    dadosAluno.Rg_emissor = Convert.ToString(reader["Rg_emissor"]);
                    dadosAluno.Rg_uf = Convert.ToString(reader["Rg_uf"]);

                    if (reader["Rg_dtexp"] != DBNull.Value)
                    {
                        dadosAluno.Rg_dtexp = Convert.ToDateTime(reader["Rg_dtexp"]);
                    }

                    dadosAluno.TipoCertidao = Convert.ToString(reader["FL_FIELD_02"]);
                    dadosAluno.CertidaoCivil = Convert.ToString(reader["FL_FIELD_09"]);

                    if (reader["ID_CARTORIO"] != DBNull.Value)
                    {
                        dadosAluno.IdCartorio = Convert.ToInt32(reader["ID_CARTORIO"]);
                        dadosAluno.NomeCartorio = Convert.ToString(reader["nome_cartorio"]);
                        dadosAluno.UfCartorio = Convert.ToString(reader["CODIGO_UF"]);
                        dadosAluno.UfCartorioNome = Convert.ToString(reader["UF_cart"]);
                        dadosAluno.MunicipioCartorio = Convert.ToString(reader["CODIGO_MUNICIPIO"]);
                        dadosAluno.MunicipioCartorioNome = Convert.ToString(reader["NOME_MUNICIPIO_CART"]);
                    }

                    dadosAluno.CertNascNum = Convert.ToString(reader["CERT_NASC_NUM"]);
                    dadosAluno.CertNascFolha = Convert.ToString(reader["CERT_NASC_FOLHA"]);
                    dadosAluno.CertNascLivro = Convert.ToString(reader["CERT_NASC_LIVRO"]);
                    dadosAluno.CertNascCartorioUf = Convert.ToString(reader["CERT_NASC_CARTORIO_UF"]);

                    if (reader["CERT_NASC_EMISSAO"] != DBNull.Value)
                    {
                        dadosAluno.CertNascEmissao = Convert.ToDateTime(reader["CERT_NASC_EMISSAO"]);
                    }

                    dadosAluno.CertNumeroMatricula = Convert.ToString(reader["CERT_NUMERO_MATRICULA"]);
                }

                return dadosAluno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public ValidacaoDados ValidaAlteracaoDadosCadastrais(RecursosHumanos.DTO.DadosCadastraisAluno dadosCadastraisAluno, string aluno, string semLocalizacaoDiferenciada, List<RN.Entidades.DeclaracaoAusencia> declaracoesAusencia)
        {
            RN.Usuarios rnUsuarios = new Usuarios();
            RN.Aluno rnAluno = new Aluno();
            RN.Validacao rnValidacao = new Validacao();
            RN.Perfil rnPerfil = new Perfil();
            RN.RecursosHumanos.PeriodoAlteracaoAluno rnPeriodoAlteracaoAluno = new Techne.Lyceum.RN.RecursosHumanos.PeriodoAlteracaoAluno();
            RN.RecursosHumanos.NomeSemValidacao rnNomeSemValidacao = new Techne.Lyceum.RN.RecursosHumanos.NomeSemValidacao();
            string cpf = string.Empty;
            Regex regex = new Regex(@"\s{2,}");
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            int idade = 0;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (dadosCadastraisAluno == null)
            {
                return validacaoDados;
            }

            if (aluno.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ALUNO é obrigatório.");
            }

            if (dadosCadastraisAluno.Pessoa <= 0)
            {
                mensagens.Add("Campo PESSOA é obrigatório.");
            }

            if (dadosCadastraisAluno.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo USUARIO RESPONSAVEL é obrigatório.");
            }

            // valida campos preenchidos da aba dados pessoais

            if (dadosCadastraisAluno.Nome_compl.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME DO ALUNO é obrigatório.");
            }
            else
            {
                //Busca, caso exista Nome sem validação
                string nomePermitido = rnNomeSemValidacao.ObtemNomePor(dadosCadastraisAluno.Pessoa, (int)RN.RecursosHumanos.NomeSemValidacao.Tipo.Nome);

                //Apenas valida para o nome nao seja o permitido
                if (nomePermitido.IsNullOrEmptyOrWhiteSpace() || nomePermitido.ToUpper() != dadosCadastraisAluno.Nome_compl.ToUpper())
                {
                    /// Validaçoes de nome do aluno 
                    if (dadosCadastraisAluno.Nome_compl.Length < 5)
                    {
                        mensagens.Add("Campo NOME DO ALUNO deve conter pelo menos cinco letras!");
                    }

                    if (!string.IsNullOrEmpty(dadosCadastraisAluno.Nome_compl)
                        && !Validacao.SomenteLetras(dadosCadastraisAluno.Nome_compl))
                    {
                        mensagens.Add("Campo NOME DO ALUNO não pode conter números.");
                    }

                    var palavras = dadosCadastraisAluno.Nome_compl.CountWords();
                    var contemRepeticao = RN.Validacao.ContemRepeticao(dadosCadastraisAluno.Nome_compl, 3);
                    var nomeInvalido = TextValidator.HasForbiddenWords(dadosCadastraisAluno.Nome_compl, new PalavrasProibidasEmNomes());
                    if (Validacao.contemNumeros(dadosCadastraisAluno.Nome_compl) == false)
                    {
                        if (palavras < 2)
                        {
                            mensagens.Add("Por favor informar, informar nome e sobrenome no campo NOME.");
                        }

                        if (contemRepeticao)
                        {
                            mensagens.Add("Campo NOME possui inconsistência por repetição excessiva de letras. Favor corrigir a informação.");
                        }

                        if (nomeInvalido)
                        {
                            mensagens.Add("Campo NOME possui inconsistência por não representar um nome válido. Favor corrigir a informação.");
                        }
                    }

                    regex = new Regex(@"\s{2,}");
                    string NomeCompl = regex.Replace(dadosCadastraisAluno.Nome_compl.Trim().ToUpper(), " ");
                    var contemApostrofeRep = Validacao.substitueApostrofe(NomeCompl);
                    if (contemApostrofeRep)
                    {
                        mensagens.Add("Campo NOME DO ALUNO possui inconsistência por repetição excessiva de apóstrofes. Favor corrigir a informação.");
                    }
                }
            }

            if (dadosCadastraisAluno.Dt_nasc == null || dadosCadastraisAluno.Dt_nasc == DateTime.MinValue)
            {
                mensagens.Add("Campo DATA DE NASCIMENTO é obrigatório.");
            }
            else
            {
                idade = Utils.CalcularIdade(Convert.ToDateTime(dadosCadastraisAluno.Dt_nasc));

                if (idade > 110)
                {
                    mensagens.Add("A idade não pode ser superior a 110 anos. Favor verificar a Data de Nascimento.");
                }
            }

            if (dadosCadastraisAluno.Sexo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo SEXO é obrigatório.");
            }

            if (string.IsNullOrEmpty(dadosCadastraisAluno.Etnia) || dadosCadastraisAluno.Etnia == "<Nenhum>")
            {
                mensagens.Add("Campo ETNIA é obrigatório!");
            }


            if (dadosCadastraisAluno.Est_civil.IsNullOrEmptyOrWhiteSpace() || dadosCadastraisAluno.Est_civil == "<Nenhum>")
            {
                mensagens.Add("Campo ESTADO CIVIL é obrigatório.");
            }

            if (dadosCadastraisAluno.Pais_nasc.IsNullOrEmptyOrWhiteSpace() || dadosCadastraisAluno.Pais_nasc == "<Nenhum>")
            {
                mensagens.Add("Campo PAIS DE NASCIMENTO é obrigatório.");
            }

            if (dadosCadastraisAluno.Pais_nasc_nome.IsNullOrEmptyOrWhiteSpace() || dadosCadastraisAluno.Pais_nasc_nome == "<Nenhum>")
            {
                mensagens.Add("Campo PAIS DE NASCIMENTO é obrigatório.");
            }


            if (dadosCadastraisAluno.Nacionalidade.IsNullOrEmptyOrWhiteSpace() || dadosCadastraisAluno.Nacionalidade == "<Nenhum>")
            {
                mensagens.Add("Campo NACIONALIDADE é obrigatório.");
            }
            else
            {
                if (dadosCadastraisAluno.Nacionalidade == "BRASILEIRA")
                {
                    //Apenas validar Naturalidade caso o Pais de nascimento seja o brasil
                    if (dadosCadastraisAluno.Municipio_nasc.IsNullOrEmptyOrWhiteSpace() || dadosCadastraisAluno.Municipio_nasc == "<Nenhum>")
                    {
                        mensagens.Add("Campo NATURALIDADE é obrigatório.");
                    }

                    if (dadosCadastraisAluno.Municipio_nasc_nome.IsNullOrEmptyOrWhiteSpace() || dadosCadastraisAluno.Municipio_nasc_nome == "<Nenhum>")
                    {
                        mensagens.Add("Campo NATURALIDADE é obrigatório.");
                    }
                }

                if (dadosCadastraisAluno.Pais_nasc.IsNullOrEmptyOrWhiteSpace() || dadosCadastraisAluno.Pais_nasc == "<Nenhum>")
                {
                    if (dadosCadastraisAluno.Pais_nasc == "1"
                        && dadosCadastraisAluno.Nacionalidade != "BRASILEIRA")
                    {
                        mensagens.Add("Campo PAÍS DE NASCIMENTO não pode ser Brasil com a NACIONALIDADE diferente de brasileira.");
                    }

                    if (dadosCadastraisAluno.Pais_nasc != "1"
                        && dadosCadastraisAluno.Nacionalidade == "BRASILEIRA")
                    {
                        mensagens.Add("Campo PAÍS DE NASCIMENTO não pode ser diferente de Brasil e a NACIONALIDADE ser brasileira.");
                    }
                }
            }

            if (dadosCadastraisAluno.NomeMae.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME DA MÃE é obrigatório.");
            }
            else
            {
                //Busca, caso exista Nome sem validação
                string nomePermitido = rnNomeSemValidacao.ObtemNomePor(dadosCadastraisAluno.Pessoa, (int)RN.RecursosHumanos.NomeSemValidacao.Tipo.Mae);

                //Apenas valida para o nome nao seja o permitido
                if (nomePermitido.IsNullOrEmptyOrWhiteSpace() || nomePermitido.ToUpper() != dadosCadastraisAluno.NomeMae.ToUpper())
                {
                    /// Validaçoes de nome da mae

                    if (dadosCadastraisAluno.NomeMae == "NÃO DECLARADA" && !dadosCadastraisAluno.DeclaroAusenciaMae)
                    {
                        mensagens.Add("Se o Nome da Mãe for Não Declarada é necessário que a informação de declaração esteja marcada.");
                    }

                    var palavras = dadosCadastraisAluno.NomeMae.CountWords();
                    var contemRepeticao = RN.Validacao.ContemRepeticao(dadosCadastraisAluno.NomeMae, 3);
                    var nomeInvalido = TextValidator.HasForbiddenWords(dadosCadastraisAluno.NomeMae, new PalavrasProibidasEmNomes());
                    if (!Validacao.contemNumeros(dadosCadastraisAluno.NomeMae))
                    {
                        if (palavras < 2)
                        {
                            mensagens.Add("Por favor informar, informar nome e sobrenome no campo NOME DA MÃE.");
                        }

                        if (contemRepeticao)
                        {
                            mensagens.Add("Campo NOME DA MÃE possui inconsistência por repetição excessiva de letras. Favor corrigir a informação.");
                        }

                        if (nomeInvalido)
                        {
                            mensagens.Add("Campo NOME DA MÃE possui inconsistência por não representar um nome válido. Favor corrigir a informação.");
                        }
                    }
                    else if (!Validacao.SomenteLetras(dadosCadastraisAluno.NomeMae))
                    {
                        mensagens.Add("Campo NOME DA MÃE não pode conter números.");
                    }

                    string NomeMae = regex.Replace(dadosCadastraisAluno.NomeMae.Trim().ToUpper(), " ");
                    var contemApostrofeRepMae = Validacao.substitueApostrofe(NomeMae);
                    if (contemApostrofeRepMae)
                    {
                        mensagens.Add("Campo NOME DO MÃE possui inconsistência por repetição excessiva de apóstrofes. Favor corrigir a informação.");
                    }
                }

                if (!string.IsNullOrEmpty(dadosCadastraisAluno.MaeCpf))
                {
                    cpf = Utils.RetirarMascara(dadosCadastraisAluno.MaeCpf);

                    if (!Validacao.ValidaCpf(cpf))
                    {
                        mensagens.Add("O CPF informado da Mãe não é válido!");
                    }
                }
            }

            if (dadosCadastraisAluno.NomePai.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME DO PAI é obrigatório.");
            }
            else
            {
                //Busca, caso exista Nome sem validação
                string nomePermitido = rnNomeSemValidacao.ObtemNomePor(dadosCadastraisAluno.Pessoa, (int)RN.RecursosHumanos.NomeSemValidacao.Tipo.Pai);

                //Apenas valida para o nome nao seja o permitido
                if (nomePermitido.IsNullOrEmptyOrWhiteSpace() || nomePermitido.ToUpper() != dadosCadastraisAluno.NomePai.ToUpper())
                {
                    /// Validações de nome do pai
                    if (dadosCadastraisAluno.NomePai == "NÃO DECLARADO" && !dadosCadastraisAluno.DeclaroAusenciaPai)
                    {
                        mensagens.Add("Se o Nome do Pai for Não Declarado é necessário que a informação de declaração esteja marcada.");
                    }

                    var palavras = dadosCadastraisAluno.NomePai.CountWords();
                    var contemRepeticao = RN.Validacao.ContemRepeticao(dadosCadastraisAluno.NomePai, 3);
                    var nomeInvalido = TextValidator.HasForbiddenWords(dadosCadastraisAluno.NomePai, new PalavrasProibidasEmNomes());
                    if (Validacao.contemNumeros(dadosCadastraisAluno.NomePai) == false)
                    {
                        if (palavras < 2)
                        {
                            mensagens.Add("Por favor informar, informar nome e sobrenome no campo NOME DO PAI.");
                        }

                        if (contemRepeticao)
                        {
                            mensagens.Add("Campo NOME DO PAI possui inconsistência por repetição excessiva de letras. Favor corrigir a informação.");
                        }

                        if (nomeInvalido)
                        {
                            mensagens.Add("Campo NOME DO PAI possui inconsistência por não representar um nome válido. Favor corrigir a informação.");
                        }
                    }
                    else if (!string.IsNullOrEmpty(dadosCadastraisAluno.NomePai) && !Validacao.SomenteLetras(dadosCadastraisAluno.NomePai))
                    {
                        mensagens.Add("Campo NOME DO PAI não pode conter números.");
                    }

                    string NomePai = regex.Replace(dadosCadastraisAluno.NomePai.Trim().ToUpper(), " ");
                    var contemApostrofeRepPai = Validacao.substitueApostrofe(NomePai);
                    if (contemApostrofeRepPai)
                    {
                        mensagens.Add("Campo NOME DO PAI possui inconsistência por repetição excessiva de apóstrofes. Favor corrigir a informação.");
                    }
                }

                if (!string.IsNullOrEmpty(dadosCadastraisAluno.PaiCpf))
                {
                    cpf = Utils.RetirarMascara(dadosCadastraisAluno.PaiCpf);

                    if (!Validacao.ValidaCpf(cpf))
                    {
                        mensagens.Add("O CPF informado do Pai não é válido!");
                    }
                }
            }

            if (!string.IsNullOrEmpty(dadosCadastraisAluno.NomeMae) && !string.IsNullOrEmpty(dadosCadastraisAluno.NomePai))
            {
                if (dadosCadastraisAluno.NomeMae == dadosCadastraisAluno.NomePai)
                {
                    mensagens.Add("Campo NOME DO PAI não pode ser idêntico ao NOME DA MÃE.");
                }
            }

            if (dadosCadastraisAluno.Responsavel.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo RESPONSÁVEL LEGAL é obrigatório.");
            }
            else
            {
                string[] tipo_resp = dadosCadastraisAluno.Responsavel.Split(';');

                if (tipo_resp.Count() > 2)
                {
                    mensagens.Add("Somente poderá ter no máximo duas opções de Responsável Legal.");
                }

                if (dadosCadastraisAluno.Responsavel.Contains("Próprio Aluno"))
                {
                    if (idade < 18)
                    {
                        mensagens.Add("Para o aluno ser Responsável é necessário ter mais que 18 anos.");
                    }
                }

                if (dadosCadastraisAluno.Responsavel.Contains("Pai"))
                {
                    if (dadosCadastraisAluno.NomePai.IsNullOrEmptyOrWhiteSpace() || dadosCadastraisAluno.NomePai == "NÃO DECLARADO")
                    {
                        mensagens.Add("Campo Nome do Pai é de preenchimento obrigatório  e deve ser diferente de não declarado, quando escolhido como responsável.");
                    }
                }

                if (dadosCadastraisAluno.Responsavel.Contains("Mãe"))
                {
                    if (dadosCadastraisAluno.NomeMae.IsNullOrEmptyOrWhiteSpace() || dadosCadastraisAluno.NomeMae == "NÃO DECLARADA")
                    {
                        mensagens.Add("Campo Nome do Mãe é de preenchimento obrigatório e deve ser diferente de não declarada, quando escolhida como responsável.");
                    }
                }

                if (dadosCadastraisAluno.Responsavel.Contains("Outros"))
                {
                    if (string.IsNullOrEmpty(dadosCadastraisAluno.RespNomeCompl))
                    {
                        mensagens.Add("Campo Nome do Responsável Legal é de preenchimento obrigatório.");
                    }
                    else
                    {
                        if (dadosCadastraisAluno.RespNomeCompl == dadosCadastraisAluno.NomeMae || dadosCadastraisAluno.RespNomeCompl == dadosCadastraisAluno.NomePai)
                        {
                            mensagens.Add("Campo Nome do Responsável Legal não pode ser igual ao Nome da(o) Mãe/Pai.");
                        }

                        var palavras = dadosCadastraisAluno.RespNomeCompl.CountWords();
                        var contemRepeticao = RN.Validacao.ContemRepeticao(dadosCadastraisAluno.RespNomeCompl, 3);
                        var nomeInvalido = TextValidator.HasForbiddenWords(dadosCadastraisAluno.RespNomeCompl, new PalavrasProibidasEmNomes())
                                           || TextValidator.HasNumbers(dadosCadastraisAluno.RespNomeCompl);

                        if (palavras < 2)
                        {
                            mensagens.Add("Por favor, informar nome e sobrenome no campo NOME DO RESPONSÁVEL.");
                        }

                        if (contemRepeticao)
                        {
                            mensagens.Add("Campo NOME DO RESPONSÁVEL possui inconsistência por repetição excessiva de letras. Favor corrigir a informação.");
                        }

                        if (nomeInvalido)
                        {
                            mensagens.Add("Campo NOME DO RESPONSÁVEL possui inconsistência por não representar um nome válido. Favor corrigir a informação.");
                        }
                    }
                }
            }

            if (dadosCadastraisAluno.Cep.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CEP é obrigatório!");
            }
            else
            {
                var cep = Utils.RetirarMascara(dadosCadastraisAluno.Cep);

                if (!Validacao.ValidarCEP(cep))
                {
                    mensagens.Add("CEP inválido! Este CEP não foi encontrado em nossa base.");
                }
            }

            if (dadosCadastraisAluno.End_municipio.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo MUNICIPIO é obrigatório.");
            }

            if (dadosCadastraisAluno.End_NomeMunicipio.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo MUNICIPIO é obrigatório.");
            }

            if (dadosCadastraisAluno.Endereco.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ENDEREÇO é obrigatório!");
            }
            else
            {
                if (dadosCadastraisAluno.Endereco.Length > 50)
                {
                    mensagens.Add("Campo ENDEREÇO deve conter no máximo 50 caracteres!");
                }
            }

            if (dadosCadastraisAluno.End_num.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NÚMERO DO ENDEREÇO é obrigatório.");
            }

            if (dadosCadastraisAluno.Bairro.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo BAIRRO é obrigatório.");
            }
            else
            {
                if (!Validacao.Bairro(dadosCadastraisAluno.Bairro))
                {
                    mensagens.Add("Campo BAIRRO é inválido!");
                }
            }

            if (dadosCadastraisAluno.ZonaResidencial.IsNullOrEmptyOrWhiteSpace() || dadosCadastraisAluno.ZonaResidencial == "<Nenhum>")
            {
                mensagens.Add("Campo LOCALIZAÇÃO/ZONA DE RESIDÊNCIA é obrigatório.");
            }

            if (dadosCadastraisAluno.AreaQuilombos.IsNullOrEmptyOrWhiteSpace()
                || (dadosCadastraisAluno.AreaQuilombos != "N" && dadosCadastraisAluno.AreaQuilombos != "S"))
            {
                mensagens.Add("O campo AREA DE QUILOMBOS é obrigatório com os Valores N ou S.");
            }

            if (dadosCadastraisAluno.TerraIndigena.IsNullOrEmptyOrWhiteSpace()
                || (dadosCadastraisAluno.TerraIndigena != "N" && dadosCadastraisAluno.TerraIndigena != "S"))
            {
                mensagens.Add("O campo TERRA INDIGENA é obrigatório com os Valores N ou S.");
            }

            if (dadosCadastraisAluno.AreaAssentamento.IsNullOrEmptyOrWhiteSpace()
                || (dadosCadastraisAluno.AreaAssentamento != "N" && dadosCadastraisAluno.AreaAssentamento != "S"))
            {
                mensagens.Add("O campo AREA DE ASSENTAMENTO é obrigatório com os Valores N ou S.");
            }

            if (semLocalizacaoDiferenciada.IsNullOrEmptyOrWhiteSpace()
                || (semLocalizacaoDiferenciada != "N" && semLocalizacaoDiferenciada != "S"))
            {
                mensagens.Add("O campo SEM LOCALIZAÇÃO DIFERENCIADA é obrigatório com os Valores N ou S.");
            }
            else
            {
                if (semLocalizacaoDiferenciada == "N")
                {
                    if ((dadosCadastraisAluno.AreaAssentamento.IsNullOrEmptyOrWhiteSpace() || dadosCadastraisAluno.AreaAssentamento == "N")
                        && (dadosCadastraisAluno.TerraIndigena.IsNullOrEmptyOrWhiteSpace() || dadosCadastraisAluno.TerraIndigena == "N")
                        && (dadosCadastraisAluno.AreaTradicional.IsNullOrEmptyOrWhiteSpace() || dadosCadastraisAluno.AreaTradicional == "N")
                        && (dadosCadastraisAluno.AreaQuilombos.IsNullOrEmptyOrWhiteSpace() || dadosCadastraisAluno.AreaQuilombos == "N"))
                    {
                        mensagens.Add("O campo LOCALIZAÇÃO DIFERENCIADA é obrigatório.");
                    }
                }
                else
                {
                    if ((!dadosCadastraisAluno.AreaAssentamento.IsNullOrEmptyOrWhiteSpace() && dadosCadastraisAluno.AreaAssentamento == "S")
                        || (!dadosCadastraisAluno.TerraIndigena.IsNullOrEmptyOrWhiteSpace() && dadosCadastraisAluno.TerraIndigena == "S")
                        || (!dadosCadastraisAluno.AreaQuilombos.IsNullOrEmptyOrWhiteSpace() && dadosCadastraisAluno.AreaQuilombos == "S"))
                    {
                        mensagens.Add("O campo LOCALIZAÇÃO DIFERENCIADA não pode possuir outra marcação quando NÃO SE APLICA estiver selecionado.");
                    }
                }
            }

            if (!dadosCadastraisAluno.Fone.IsNullOrEmptyOrWhiteSpace())
            {
                var telefone = Utils.RetirarMascara(dadosCadastraisAluno.Fone);

                if (!Validacao.ValidaTelefoneComDDD(telefone) && !Validacao.ValidaCelularComDDD(telefone))
                {
                    mensagens.Add("Campo TELEFONE OU CELULAR é inválido.");
                }
            }

            if (!dadosCadastraisAluno.Celular.IsNullOrEmptyOrWhiteSpace())
            {
                var celular = Utils.RetirarMascara(dadosCadastraisAluno.Celular);

                if (!Validacao.ValidaCelularComDDD(celular))
                {
                    mensagens.Add("Campo CELULAR é inválido.!");
                }
            }

            if (!dadosCadastraisAluno.E_mail.IsNullOrEmptyOrWhiteSpace())
            {
                if (!Validacao.Email(dadosCadastraisAluno.E_mail))
                {
                    mensagens.Add("Campo E-MAIL está em um formato incorreto!");
                }

                if (dadosCadastraisAluno.E_mail.Contains("educacao.rj.gov.br"))
                {
                    mensagens.Add("Campo E-mail não pode ser do dominio educacao.rj.gov.br ou prof.educacao.rj.gov.br");
                }
            }            

            if (!dadosCadastraisAluno.Cpf.IsNullOrEmptyOrWhiteSpace())
            {
                cpf = Utils.RetirarMascara(dadosCadastraisAluno.Cpf);

                if (!Validacao.ValidaCpf(cpf))
                {
                    mensagens.Add("O CPF informado não é válido!");
                }
                else
                {
                    string cpfMae = Utils.RetirarMascara(dadosCadastraisAluno.MaeCpf);
                    string cpfPai = Utils.RetirarMascara(dadosCadastraisAluno.PaiCpf);
                    string cpfOutros = Utils.RetirarMascara(dadosCadastraisAluno.RespCpf);

                    if (cpf == cpfMae) 
                    {
                        mensagens.Add("O CPF informado não pode ser igual ao cpf da mãe");
                    }

                    if (cpf == cpfPai)
                    {
                        mensagens.Add("O CPF informado não pode ser igual ao cpf do pai");
                    }

                    if (dadosCadastraisAluno.Responsavel.Contains("Outros") && cpf == cpfOutros)
                    {
                        mensagens.Add("O CPF informado não pode ser igual ao cpf do responsável");
                    }
                }
            }


            #region Validações dos Campos de Documento
            bool documentoValido, iniciouMensagem, maisDeUmCampo;
            documentoValido = true;
            iniciouMensagem = maisDeUmCampo = false;
            System.Text.StringBuilder mensagemDocumento = new System.Text.StringBuilder();
            System.Text.StringBuilder camposDocumento = new System.Text.StringBuilder();
            mensagemDocumento.Append("Documento:<br>Não é possível deixar de preencher um dos campos referentes ao tipo de documento.");

            if (!dadosCadastraisAluno.Rg_tipo.IsNullOrEmptyOrWhiteSpace() && dadosCadastraisAluno.Rg_tipo != "<Nenhum>")
            {
                if (Convert.ToString(dadosCadastraisAluno.Rg_num).IsNullOrEmptyOrWhiteSpace())
                {
                    documentoValido = false;
                    iniciouMensagem = true;
                    camposDocumento.Append("Número ");
                }
                else
                {
                    var rg = Utils.RetirarMascara(dadosCadastraisAluno.Rg_num);

                    if (rg.Length < 5)
                    {
                        mensagens.Add("O NÚMERO DO DOCUMENTO deve conter no mínimo cinco dígitos!");
                    }

                    if (dadosCadastraisAluno.Rg_tipo == "RG" && dadosCadastraisAluno.Rg_emissor == "DETRAN" && dadosCadastraisAluno.Rg_uf == "RJ")
                    {
                        if (!Validacao.ValidaNumerosInteirosPositivos(dadosCadastraisAluno.Rg_num))
                        {
                            mensagens.Add("O número de documento do RG Detran deve conter só números inteiros.");
                        }
                    }
                }

                if (dadosCadastraisAluno.Rg_tipo == "RG")
                {
                    if (dadosCadastraisAluno.Rg_emissor != "CGPI/DIREX/DPF" && Convert.ToString(dadosCadastraisAluno.Rg_uf).IsNullOrEmptyOrWhiteSpace())
                    {
                        documentoValido = false;
                        if (iniciouMensagem)
                        {
                            maisDeUmCampo = true;
                            camposDocumento.Append("- Estado ");
                        }
                        else
                        {
                            iniciouMensagem = true;
                            camposDocumento.Append("Estado ");
                        }
                    }

                    if (Convert.ToString(dadosCadastraisAluno.Rg_emissor).IsNullOrEmptyOrWhiteSpace())
                    {
                        documentoValido = false;
                        if (iniciouMensagem)
                        {
                            maisDeUmCampo = true;
                            camposDocumento.Append("- Órgão Emissor ");
                        }
                        else
                        {
                            iniciouMensagem = true;
                            camposDocumento.Append("Órgão Emissor ");
                        }
                    }

                    if (dadosCadastraisAluno.Rg_dtexp == null)
                    {
                        documentoValido = false;
                        if (iniciouMensagem)
                        {
                            maisDeUmCampo = true;
                            camposDocumento.Append("- Data de Expedição ");
                        }
                        else
                        {
                            iniciouMensagem = true;
                            camposDocumento.Append("Data de Expedição ");
                        }
                    }

                    if (dadosCadastraisAluno.Rg_dtexp != null && dadosCadastraisAluno.Rg_dtexp != DateTime.MinValue)
                    {
                        if (Convert.ToDateTime(dadosCadastraisAluno.Dt_nasc).Date >= Convert.ToDateTime(dadosCadastraisAluno.Rg_dtexp).Date)
                        {
                            mensagens.Add("A Data de expedição da identidade tem que ser maior que a data de nascimento.");
                        }
                    }
                }
                else
                {
                    if (dadosCadastraisAluno.Rg_tipo == "PASSAPORTE" && dadosCadastraisAluno.Nacionalidade == "BRASILEIRA")
                    {
                        mensagens.Add("Para alunos de Nacionalidade 'BRASILEIRA' o documento obrigatório é o RG.");
                    }
                }
            }

            if (dadosCadastraisAluno.Rg_emissor == "CGPI/DIREX/DPF" && dadosCadastraisAluno.Nacionalidade == "BRASILEIRA")
            {
                mensagens.Add("Para alunos de Nacionalidade 'BRASILEIRA' o Órgão Emissor não pode ser 'CGPI/DIREX/DPF'.");
            }

            if (!documentoValido)
            {
                if (maisDeUmCampo)
                {
                    mensagemDocumento.Append("<br>Campos Necessários: ");
                }
                else
                {
                    mensagemDocumento.Append("<br>Campo Necessário: ");
                }

                mensagemDocumento.Append(camposDocumento);
                mensagens.Add(mensagemDocumento.ToString());
            }

            #endregion



            if (dadosCadastraisAluno.TipoCertidao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Necessário selecionar o tipo de certidão civil.");
            }
            else
            {

                if (dadosCadastraisAluno.TipoCertidao == "Nenhum")
                {
                    RN.Entidades.DeclaracaoAusencia declaracao = declaracoesAusencia.Where(x => x.TipoDeclaracaoAusenciaId == Convert.ToInt32(RN.DeclaracaoAusencia.TipoDeclaracaoAusenciaId.DeclaracaoCertidaoCivil)).First();
                    if (declaracao == null
                        || declaracao.TipoDeclaracaoAusenciaId <= 0
                        || declaracao.Motivo.IsNullOrEmptyOrWhiteSpace()
                        || !dadosCadastraisAluno.DeclaroCertidaoCivil)
                    {
                        mensagens.Add("É necessário preencher a Declaração/Motivo da Certidão Civil.");
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(dadosCadastraisAluno.CertidaoCivil))
                    {
                        mensagens.Add("Necessário selecionar a certidão civil.");
                    }

                    if (dadosCadastraisAluno.TipoCertidao == "Nascimento")
                    {
                        if (string.IsNullOrEmpty(dadosCadastraisAluno.CertidaoCivil)
                            && ((dadosCadastraisAluno.CertNascEmissao != null && dadosCadastraisAluno.CertNascEmissao != DateTime.MinValue)
                                || !string.IsNullOrEmpty(dadosCadastraisAluno.TipoCertidao)
                                || !string.IsNullOrEmpty(dadosCadastraisAluno.CertNascFolha)
                                || !string.IsNullOrEmpty(dadosCadastraisAluno.CertNascLivro)
                                || !string.IsNullOrEmpty(dadosCadastraisAluno.CertNascNum)
                                || !string.IsNullOrEmpty(dadosCadastraisAluno.CertNumeroMatricula)))
                        {
                            mensagens.Add("É necessário preencher os dados referentes a Certidão Civil");
                        }
                    }

                    if (!string.IsNullOrEmpty(dadosCadastraisAluno.CertidaoCivil) ||
                        !string.IsNullOrEmpty(dadosCadastraisAluno.TipoCertidao))
                    {
                        if (dadosCadastraisAluno.CertidaoCivil == "Modelo Novo" &&
                            string.IsNullOrEmpty(dadosCadastraisAluno.CertNumeroMatricula))
                        {
                            mensagens.Add("É necessário preencher os dados referentes a Certidão Civil Modelo Novo.");
                        }
                        else if ((dadosCadastraisAluno.CertidaoCivil == "Modelo Antigo") &&
                                 (dadosCadastraisAluno.IdCartorio == null || dadosCadastraisAluno.IdCartorio <= 0
                                  || string.IsNullOrEmpty(dadosCadastraisAluno.NomeCartorio)
                                  || string.IsNullOrEmpty(dadosCadastraisAluno.CertNascCartorioUf)
                                  || string.IsNullOrEmpty(dadosCadastraisAluno.UfCartorio)
                                  || string.IsNullOrEmpty(dadosCadastraisAluno.UfCartorioNome)
                                  || string.IsNullOrEmpty(dadosCadastraisAluno.MunicipioCartorio)
                                  || string.IsNullOrEmpty(dadosCadastraisAluno.MunicipioCartorioNome)
                                  || string.IsNullOrEmpty(dadosCadastraisAluno.CertNascFolha)
                                  || string.IsNullOrEmpty(dadosCadastraisAluno.CertNascLivro)
                                  || string.IsNullOrEmpty(dadosCadastraisAluno.CertNascNum)
                            //|| string.IsNullOrEmpty(dadosCadastraisAluno.CertNascCartorioExped)
                                 ))
                        {
                            mensagens.Add("É necessário preencher os dados referentes a Certidão Civil Modelo Antigo");
                        }

                        if (dadosCadastraisAluno.CertidaoCivil == "Modelo Novo" &&
                           !string.IsNullOrEmpty(dadosCadastraisAluno.CertNumeroMatricula))
                        {
                            if (!Validacao.ValidaNumerosInteirosPositivos(dadosCadastraisAluno.CertNumeroMatricula))
                            {
                                mensagens.Add("O número de matrícula da Certidão Modelo Novo deve conter só números inteiros.");
                            }

                            if (dadosCadastraisAluno.CertNumeroMatricula.Length != 32)
                            {
                                mensagens.Add("O número de matrícula da Certidão Modelo Novo deve ter 32 dígitos.");
                            }
                        }
                    }

                    if (dadosCadastraisAluno.Dt_nasc != null && dadosCadastraisAluno.CertNascEmissao != null)
                    {
                        if (Convert.ToDateTime(dadosCadastraisAluno.Dt_nasc) > Convert.ToDateTime(dadosCadastraisAluno.CertNascEmissao))
                        {
                            mensagens.Add("A data de Emissão da Certidão não pode ser inferior a data de nascimento.");
                        }
                    }

                    if (dadosCadastraisAluno.Dt_nasc != null && dadosCadastraisAluno.Rg_dtexp != null && dadosCadastraisAluno.Rg_dtexp < dadosCadastraisAluno.Dt_nasc)
                    {
                        mensagens.Add("A DATA DE EXPEDIÇÃO do documento de indentificação deve ser maior que a data de nascimento!");
                    }

                    if (dadosCadastraisAluno.Dt_nasc != null && dadosCadastraisAluno.CertNascEmissao != null && dadosCadastraisAluno.CertNascEmissao < dadosCadastraisAluno.Dt_nasc)
                    {
                        mensagens.Add("A DATA DE EMISSÃO da certidão de nascimento deve ser maior que a data de nascimento!");
                    }
                }
            }

            foreach (var declaracaoAusencia in declaracoesAusencia)
            {
                if (declaracaoAusencia.TipoDeclaracaoAusenciaId != 0)
                {
                    if (declaracaoAusencia.TipoDeclaracaoAusenciaId == 0)
                    {
                        mensagens.Add("Campo Tipo de Declaração é obrigatório.");
                    }

                    if (declaracaoAusencia.TipoDeclaracaoAusenciaId == 3 && string.IsNullOrEmpty(declaracaoAusencia.Motivo))
                    {
                        mensagens.Add("Campo Motivo é obrigatório para Certidão Civil.");
                    }

                    if (!string.IsNullOrEmpty(declaracaoAusencia.Motivo))
                    {
                        if (declaracaoAusencia.Motivo.Length < 10)
                        {
                            mensagens.Add("Campo MOTIVO deve ter mais 10 caracteres.");
                        }

                        if (declaracaoAusencia.Motivo.Length > 200)
                        {
                            mensagens.Add("Campo MOTIVO é obrigatório com o máximo de 200 caracteres!");
                        }

                        regex = new Regex(@"(\w)\1\1+");

                        if (regex.IsMatch(declaracaoAusencia.Motivo))
                        {
                            mensagens.Add("Campo MOTIVO não deve ter mais de 2 letras consecutivas repetidas.");
                        }
                    }

                    if (string.IsNullOrEmpty(declaracaoAusencia.Matricula)
                            || (!string.IsNullOrEmpty(declaracaoAusencia.Matricula)
                                && declaracaoAusencia.Matricula.Length > 12))
                    {
                        mensagens.Add("Campo MATRICULA é obrigatório com o máximo de 12 caracteres!");
                    }
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Veriica se aluno está ativo
                    if (!rnAluno.EhAlunoAtivoPor(contexto, aluno))
                    {
                        mensagens.Add("Este aluno não pode ser editado, pois não está ATIVO.");
                    }

                    //Verifica se pessoa nao tem perfil para alterar em qualquer data ou se esta em periodo que permite a alteração
                    if (!rnPerfil.PossuiPerfilAlteracaoDadosCadastraisAlunoForaPeriodoPor(contexto, dadosCadastraisAluno.UsuarioId)
                         && !rnPeriodoAlteracaoAluno.PossuiPeriodoAlteracaoAlunoAbertoPor(contexto, DateTime.Now.Year, DateTime.Now)
                         && !rnUsuarios.EhPrivilegiado(contexto, dadosCadastraisAluno.UsuarioId))
                    {
                        mensagens.Add("O período para alteração dos dados cadastrais do aluno não está aberto.");
                    }

                    //Busca pessoa caso existe com mesmo nome/data de nascimento/nome da mãe
                    decimal pessoaBase = this.ObtemPessoaPor(contexto, dadosCadastraisAluno.Nome_compl, dadosCadastraisAluno.NomeMae, Convert.ToDateTime(dadosCadastraisAluno.Dt_nasc));

                    if (pessoaBase != 0 && dadosCadastraisAluno.Pessoa != pessoaBase)
                    {
                        mensagens.Add("Já existe outra pessoa com mesmo nome/data de nascimento/nome da mãe cadastrada.");
                    }

                    if (!dadosCadastraisAluno.Cpf.IsNullOrEmptyOrWhiteSpace())
                    {
                        //Verifica se existe outro aluno com o mesmo cpf
                        string retorno = rnAluno.PossuiOutroCPFPor(contexto, dadosCadastraisAluno.Cpf, aluno);
                        if (!retorno.IsNullOrEmptyOrWhiteSpace())
                        {
                            mensagens.Add(string.Format("O aluno {0} já foi cadastrado com este CPF.", retorno));
                        }
                        else
                        {
                            if (this.PossuiCPFPor(contexto, dadosCadastraisAluno.Cpf, dadosCadastraisAluno.Pessoa))
                            {
                                mensagens.Add("CPF já existente cadastrado para um docente / servidor.");
                            }
                        }
                    }

                    //Verifica se aluno permite abreviação
                    if (rnAluno.ExisteAbreviacao(contexto, aluno))
                    {
                        var nomes = dadosCadastraisAluno.Nome_compl.Trim().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        for (var i = 0; i < nomes.Length; i++)
                        {
                            var nome = nomes[i];

                            if (nome.Length == 2 && !rnValidacao.ehAbreviacaoValida(nome) && nome.IndexOf(".") != -1)
                            {
                                nome = nome.Remove(1);
                            }

                            if ((nome.Length == 1 && !rnValidacao.ehAbreviacaoValida(nome) && (string.Compare(nome, "e", true) != 0)) ||
                                ((string.Compare(nome, "e", true) == 0) && (i == 0 || i == nomes.Length - 1)))
                            {
                                mensagens.Add("Não é possível utilizar abreviações no nome do aluno.");
                            }
                        }

                        var nomesMae = dadosCadastraisAluno.NomeMae.Trim().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        for (var i = 0; i < nomesMae.Length; i++)
                        {
                            var nome = nomesMae[i];

                            if (nome.Length == 2 && !rnValidacao.ehAbreviacaoValida(nome) && nome.IndexOf(".") != -1)
                            {
                                nome = nome.Remove(1);
                            }
                            if ((nome.Length == 1 && !rnValidacao.ehAbreviacaoValida(nome) && (string.Compare(nome, "e", true) != 0)) ||
                                ((string.Compare(nome, "e", true) == 0) && (i == 0 || i == nomesMae.Length - 1)))
                            {
                                mensagens.Add("Não é possível utilizar abreviações no nome da mãe.");
                            }
                        }

                        var nomesPai = dadosCadastraisAluno.NomePai.Trim().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                        for (var i = 0; i < nomesPai.Length; i++)
                        {
                            var nome = nomesPai[i];

                            if (nome.Length == 2 && !rnValidacao.ehAbreviacaoValida(nome) && nome.IndexOf(".") != -1)
                            {
                                nome = nome.Remove(1);
                            }
                            if ((nome.Length == 1 && (string.Compare(nome, "e", true) != 0)) ||
                                ((string.Compare(nome, "e", true) == 0) && (i == 0 || i == nomesPai.Length - 1)))
                            {
                                mensagens.Add("Não é possível utilizar abreviações no nome do pai.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw new Exception(ex.Message);
                }
                finally
                {
                    if (contexto != null)
                    {
                        contexto.Dispose();
                    }
                }
            }

            if (mensagens.Count > 0)
            {
                validacaoDados.Mensagem = mensagens.Aggregate((x, y) => x + Environment.NewLine + y);
            }
            else
            {
                validacaoDados.Valido = true;
            }

            return validacaoDados;
        }

        public void AlteraDadosCadastrais(RecursosHumanos.DTO.DadosCadastraisAluno dadosCadastraisAluno, List<RN.Entidades.DeclaracaoAusencia> declaracoesAusencia, string aluno)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();
            RN.FlPessoa rnFlPessoa = new FlPessoa();
            RecursosHumanos.Entidades.HistoricoAlteracaoAluno historicoAlteracaoAluno = new Techne.Lyceum.RN.RecursosHumanos.Entidades.HistoricoAlteracaoAluno();
            RecursosHumanos.HistoricoAlteracaoAluno rnHistoricoAlteracaoAluno = new Techne.Lyceum.RN.RecursosHumanos.HistoricoAlteracaoAluno();
            RecursosHumanos.HistoricoAlteracaoAlunoCampos rnHistoricoAlteracaoAlunoCampos = new Techne.Lyceum.RN.RecursosHumanos.HistoricoAlteracaoAlunoCampos();
            RecursosHumanos.DTO.DadosCadastraisAluno dadosBase = new Techne.Lyceum.RN.RecursosHumanos.DTO.DadosCadastraisAluno();
            RN.Aluno rnAluno = new Aluno();

            try
            {
                //Busca dados atuais na base
                dadosBase = this.ObtemDadosCadastraisAluno(contexto, Convert.ToInt32(dadosCadastraisAluno.Pessoa));

                //Monta Entidade de historico
                historicoAlteracaoAluno.Pessoa = Convert.ToInt32(dadosCadastraisAluno.Pessoa);
                historicoAlteracaoAluno.UsuarioId = dadosCadastraisAluno.UsuarioId;

                //Insere Historico
                rnHistoricoAlteracaoAluno.Insere(contexto, historicoAlteracaoAluno);

                //Verifica se cada campo foi modificado para inserir no historico
                if ((dadosBase.Nome_compl.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.Nome_compl.ToUpper().Trim())
                    != (dadosCadastraisAluno.Nome_compl.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.Nome_compl.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "NOME", dadosBase.Nome_compl, dadosCadastraisAluno.Nome_compl);
                }

                if ((dadosBase.PreNomeSocial.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.PreNomeSocial.ToUpper().Trim())
                    != (dadosCadastraisAluno.PreNomeSocial.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.PreNomeSocial.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "NOME SOCIAL", dadosBase.PreNomeSocial, dadosCadastraisAluno.PreNomeSocial);
                }

                if (dadosBase.Dt_nasc != dadosCadastraisAluno.Dt_nasc)
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "DATA NASCIMENTO", Convert.ToString(dadosBase.Dt_nasc), Convert.ToString(dadosCadastraisAluno.Dt_nasc));
                }

                if ((dadosBase.Sexo.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.Sexo.ToUpper().Trim())
                    != (dadosCadastraisAluno.Sexo.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.Sexo.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "SEXO", dadosBase.Sexo == "F" ? "Feminino" : "Masculino", dadosCadastraisAluno.Sexo == "F" ? "Feminino" : "Masculino");
                }

                if (dadosBase.QtFilhos != dadosCadastraisAluno.QtFilhos)
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "QUANTIDADE FILHOS", Convert.ToString(dadosBase.QtFilhos), Convert.ToString(dadosCadastraisAluno.QtFilhos));
                }

                if ((dadosBase.Etnia.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.Etnia.ToUpper().Trim())
                    != (dadosCadastraisAluno.Etnia.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.Etnia.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "ETNIA", dadosBase.Etnia, dadosCadastraisAluno.Etnia);
                }

                if ((dadosBase.Est_civil.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.Est_civil.ToUpper().Trim())
                    != (dadosCadastraisAluno.Est_civil.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.Est_civil.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "ESTADO CIVIL", dadosBase.Est_civil, dadosCadastraisAluno.Est_civil);
                }

                if ((dadosBase.Pais_nasc.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.Pais_nasc.ToUpper().Trim())
                    != (dadosCadastraisAluno.Pais_nasc.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.Pais_nasc.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "PAIS NASCIMENTO", dadosBase.Pais_nasc_nome, dadosCadastraisAluno.Pais_nasc_nome);
                }

                if ((dadosBase.Nacionalidade.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.Nacionalidade.ToUpper().Trim())
                    != (dadosCadastraisAluno.Nacionalidade.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.Nacionalidade.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "NACIONALIDADE", dadosBase.Nacionalidade, dadosCadastraisAluno.Nacionalidade);
                }

                if ((dadosBase.UF_nasc.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.UF_nasc.ToUpper().Trim())
                   != (dadosCadastraisAluno.UF_nasc.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.UF_nasc.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "UF NASCIMENTO", dadosBase.UF_nasc, dadosCadastraisAluno.UF_nasc);
                }

                if ((dadosBase.Municipio_nasc.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.Municipio_nasc.ToUpper().Trim())
                  != (dadosCadastraisAluno.Municipio_nasc.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.Municipio_nasc.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "NATURALIDADE", dadosBase.Municipio_nasc_nome, dadosCadastraisAluno.Municipio_nasc_nome);
                }

                if ((dadosBase.NomeMae.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.NomeMae.ToUpper().Trim())
                   != (dadosCadastraisAluno.NomeMae.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.NomeMae.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "MÃE", dadosBase.NomeMae, dadosCadastraisAluno.NomeMae);
                }

                if ((dadosBase.MaeFalecida.IsNullOrEmptyOrWhiteSpace() ? "N" : dadosBase.MaeFalecida.ToUpper().Trim())
                   != (dadosCadastraisAluno.MaeFalecida.IsNullOrEmptyOrWhiteSpace() ? "N" : dadosCadastraisAluno.MaeFalecida.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "MÃE FALECIDA", dadosBase.MaeFalecida == "S" ? "Sim" : "Não", dadosCadastraisAluno.MaeFalecida == "S" ? "Sim" : "Não");
                }

                if ((dadosBase.MaeCpf.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.MaeCpf.ToUpper().Trim().RetirarMascaraCPF())
                   != (dadosCadastraisAluno.MaeCpf.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.MaeCpf.ToUpper().Trim().RetirarMascaraCPF()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "MÃE CPF", dadosBase.MaeCpf, dadosCadastraisAluno.MaeCpf);
                }

                if ((dadosBase.MaeTelefone.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.MaeTelefone.ToUpper().Trim().RetirarMascaraTelefone())
                   != (dadosCadastraisAluno.MaeTelefone.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.MaeTelefone.ToUpper().Trim().RetirarMascaraTelefone()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "MÃE TELEFONE", dadosBase.MaeTelefone, dadosCadastraisAluno.MaeTelefone);
                }

                if ((dadosBase.NomePai.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.NomePai.ToUpper().Trim())
                    != (dadosCadastraisAluno.NomePai.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.NomePai.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "PAI", dadosBase.NomePai, dadosCadastraisAluno.NomePai);
                }

                if ((dadosBase.PaiFalecido.IsNullOrEmptyOrWhiteSpace() ? "N" : dadosBase.PaiFalecido.ToUpper().Trim())
                    != (dadosCadastraisAluno.PaiFalecido.IsNullOrEmptyOrWhiteSpace() ? "N" : dadosCadastraisAluno.PaiFalecido.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "PAI FALECIDO", dadosBase.PaiFalecido == "S" ? "Sim" : "Não", dadosCadastraisAluno.PaiFalecido == "S" ? "Sim" : "Não");
                }

                if ((dadosBase.PaiCpf.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.PaiCpf.ToUpper().Trim().RetirarMascaraCPF())
                  != (dadosCadastraisAluno.PaiCpf.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.PaiCpf.ToUpper().Trim().RetirarMascaraCPF()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "PAI CPF", dadosBase.PaiCpf, dadosCadastraisAluno.PaiCpf);
                }

                if ((dadosBase.PaiTelefone.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.PaiTelefone.ToUpper().Trim().RetirarMascaraTelefone())
                   != (dadosCadastraisAluno.PaiTelefone.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.PaiTelefone.ToUpper().Trim().RetirarMascaraTelefone()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "PAI TELEFONE", dadosBase.PaiTelefone, dadosCadastraisAluno.PaiTelefone);
                }

                if ((dadosBase.Responsavel.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.Responsavel.ToUpper().Trim())
                    != (dadosCadastraisAluno.Responsavel.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.Responsavel.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "RESPONSAVEL", dadosBase.Responsavel, dadosCadastraisAluno.Responsavel);
                }

                if ((dadosBase.RespNomeCompl.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.RespNomeCompl.ToUpper().Trim())
                    != (dadosCadastraisAluno.RespNomeCompl.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.RespNomeCompl.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "NOME RESPONSAVEL", dadosBase.RespNomeCompl, dadosCadastraisAluno.RespNomeCompl);
                }

                if ((dadosBase.RespCpf.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.RespCpf.ToUpper().Trim().RetirarMascaraCPF())
                 != (dadosCadastraisAluno.RespCpf.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.RespCpf.ToUpper().Trim().RetirarMascaraCPF()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "RESPONSAVEL CPF", dadosBase.RespCpf, dadosCadastraisAluno.RespCpf);
                }

                if ((dadosBase.RespFone.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.RespFone.ToUpper().Trim().RetirarMascaraTelefone())
                    != (dadosCadastraisAluno.RespFone.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.RespFone.ToUpper().Trim().RetirarMascaraTelefone()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "RESPONSAVEL TELEFONE", dadosBase.RespFone, dadosCadastraisAluno.RespFone);
                }

                if ((dadosBase.Cep.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.Cep.ToUpper().Trim())
                     != (dadosCadastraisAluno.Cep.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.Cep.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "CEP", dadosBase.Cep, dadosCadastraisAluno.Cep);
                }

                if ((dadosBase.End_municipio.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.End_municipio.ToUpper().Trim())
                     != (dadosCadastraisAluno.End_municipio.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.End_municipio.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "MUNICIPIO ENDEREÇO", dadosBase.End_NomeMunicipio, dadosCadastraisAluno.End_NomeMunicipio);
                }

                if ((dadosBase.Endereco.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.Endereco.ToUpper().Trim())
                      != (dadosCadastraisAluno.Endereco.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.Endereco.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "ENDEREÇO", dadosBase.Endereco, dadosCadastraisAluno.Endereco);
                }

                if ((dadosBase.End_num.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.End_num.ToUpper().Trim())
                      != (dadosCadastraisAluno.End_num.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.End_num.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "NÚMERO ENDEREÇO", dadosBase.End_num, dadosCadastraisAluno.End_num);
                }

                if ((dadosBase.End_compl.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.End_compl.ToUpper().Trim())
                      != (dadosCadastraisAluno.End_compl.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.End_compl.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "COMPLEMENTO ENDEREÇO", dadosBase.End_compl, dadosCadastraisAluno.End_compl);
                }

                if ((dadosBase.Bairro.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.Bairro.ToUpper().Trim())
                      != (dadosCadastraisAluno.Bairro.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.Bairro.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "COMPLEMENTO ENDEREÇO", dadosBase.Bairro, dadosCadastraisAluno.Bairro);
                }

                if ((dadosBase.ZonaResidencial.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.ZonaResidencial.ToUpper().Trim())
                      != (dadosCadastraisAluno.ZonaResidencial.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.ZonaResidencial.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "ZONA RESIDENCIAL", dadosBase.ZonaResidencial, dadosCadastraisAluno.ZonaResidencial);
                }

                if ((dadosBase.AreaQuilombos.IsNullOrEmptyOrWhiteSpace() ? "N" : dadosBase.AreaQuilombos.ToUpper().Trim())
                      != (dadosCadastraisAluno.AreaQuilombos.IsNullOrEmptyOrWhiteSpace() ? "N" : dadosCadastraisAluno.AreaQuilombos.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "AREA QUILOMBOS", dadosBase.AreaQuilombos == "S" ? "Sim" : "Não", dadosCadastraisAluno.AreaQuilombos == "S" ? "Sim" : "Não");
                }

                if ((dadosBase.AreaTradicional.IsNullOrEmptyOrWhiteSpace() ? "N" : dadosBase.AreaTradicional.ToUpper().Trim())
                   != (dadosCadastraisAluno.AreaTradicional.IsNullOrEmptyOrWhiteSpace() ? "N" : dadosCadastraisAluno.AreaTradicional.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "AREA TRADICIONAL", dadosBase.AreaTradicional == "S" ? "Sim" : "Não", dadosCadastraisAluno.AreaTradicional == "S" ? "Sim" : "Não");
                }

                if ((dadosBase.TerraIndigena.IsNullOrEmptyOrWhiteSpace() ? "N" : dadosBase.TerraIndigena.ToUpper().Trim())
                     != (dadosCadastraisAluno.TerraIndigena.IsNullOrEmptyOrWhiteSpace() ? "N" : dadosCadastraisAluno.TerraIndigena.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "TERRA INDIGENA", dadosBase.TerraIndigena == "S" ? "Sim" : "Não", dadosCadastraisAluno.TerraIndigena == "S" ? "Sim" : "Não");
                }

                if ((dadosBase.AreaAssentamento.IsNullOrEmptyOrWhiteSpace() ? "N" : dadosBase.AreaAssentamento.ToUpper().Trim())
                   != (dadosCadastraisAluno.AreaAssentamento.IsNullOrEmptyOrWhiteSpace() ? "N" : dadosCadastraisAluno.AreaAssentamento.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "AREA ASSENTAMENTO", dadosBase.AreaAssentamento == "S" ? "Sim" : "Não", dadosCadastraisAluno.AreaAssentamento == "S" ? "Sim" : "Não");
                }

                if ((dadosBase.Fone.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.Fone.ToUpper().Trim().RetirarMascaraTelefone())
                    != (dadosCadastraisAluno.Fone.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.Fone.ToUpper().Trim().RetirarMascaraTelefone()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "TELEFONE", dadosBase.Fone, dadosCadastraisAluno.Fone);
                }

                if ((dadosBase.Celular.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.Celular.ToUpper().Trim().RetirarMascaraTelefone())
                    != (dadosCadastraisAluno.Celular.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.Celular.ToUpper().Trim().RetirarMascaraTelefone()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "CELULAR", dadosBase.Celular, dadosCadastraisAluno.Celular);
                }

                if ((dadosBase.E_mail.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.E_mail.ToUpper().Trim())
                   != (dadosCadastraisAluno.E_mail.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.E_mail.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "E-MAIL", dadosBase.E_mail, dadosCadastraisAluno.E_mail);

                    //Atualiza data de alteração email
                    rnAluno.AtualizaDataEmail(contexto, aluno, DateTime.Now, historicoAlteracaoAluno.UsuarioId);
                }

                if ((dadosBase.Cpf.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.Cpf.ToUpper().Trim().RetirarMascaraCPF())
                 != (dadosCadastraisAluno.Cpf.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.Cpf.ToUpper().Trim().RetirarMascaraCPF()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "CPF", dadosBase.Cpf, dadosCadastraisAluno.Cpf);
                }

                if ((dadosBase.Rg_tipo.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.Rg_tipo.ToUpper().Trim())
                 != (dadosCadastraisAluno.Rg_tipo.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.Rg_tipo.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "TIPO DOCUMENTO", dadosBase.Rg_tipo, dadosCadastraisAluno.Rg_tipo);
                }

                if ((dadosBase.Rg_num.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.Rg_num.ToUpper().Trim())
                 != (dadosCadastraisAluno.Rg_num.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.Rg_num.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "NUMERO DOCUMENTO", dadosBase.Rg_num, dadosCadastraisAluno.Rg_num);
                }

                if ((dadosBase.ComplementoIdentidade.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.ComplementoIdentidade.ToUpper().Trim())
                 != (dadosCadastraisAluno.ComplementoIdentidade.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.ComplementoIdentidade.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "COMPLEMENTO IDENTIDADE", dadosBase.ComplementoIdentidade, dadosCadastraisAluno.ComplementoIdentidade);
                }

                if ((dadosBase.Rg_uf.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.Rg_uf.ToUpper().Trim())
                 != (dadosCadastraisAluno.Rg_uf.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.Rg_uf.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "ESTADO DOCUMENTO", dadosBase.Rg_uf, dadosCadastraisAluno.Rg_uf);
                }

                if ((dadosBase.Rg_emissor.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.Rg_emissor.ToUpper().Trim())
                    != (dadosCadastraisAluno.Rg_emissor.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.Rg_emissor.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "EMISSOR DOCUMENTO", dadosBase.Rg_emissor, dadosCadastraisAluno.Rg_emissor);
                }

                if (dadosBase.Rg_dtexp != dadosCadastraisAluno.Rg_dtexp)
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "DATA EXPEDIÇÃO DOCUMENTO", Convert.ToString(dadosBase.Rg_dtexp), Convert.ToString(dadosCadastraisAluno.Rg_dtexp));
                }

                if ((dadosBase.Rg_emissor.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.Rg_emissor.ToUpper().Trim())
                    != (dadosCadastraisAluno.Rg_emissor.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.Rg_emissor.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "ÓRGÃO EMISSOR", dadosBase.Rg_emissor, dadosCadastraisAluno.Rg_emissor);
                }

                if ((dadosBase.CertidaoCivil.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.CertidaoCivil.ToUpper().Trim())
                    != (dadosCadastraisAluno.CertidaoCivil.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.CertidaoCivil.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "CERTIDÃO CIVIL", dadosBase.CertidaoCivil, dadosCadastraisAluno.CertidaoCivil);
                }

                if ((dadosBase.TipoCertidao.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.TipoCertidao.ToUpper().Trim())
                    != (dadosCadastraisAluno.TipoCertidao.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.TipoCertidao.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "TIPO CERTIDÃO", dadosBase.TipoCertidao, dadosCadastraisAluno.TipoCertidao);
                }

                if ((dadosBase.UfCartorio.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.UfCartorio.ToUpper().Trim())
                    != (dadosCadastraisAluno.UfCartorio.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.UfCartorio.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "UF CARTÓRIO", dadosBase.UfCartorioNome, dadosCadastraisAluno.UfCartorioNome);
                }

                if ((dadosBase.MunicipioCartorio.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.MunicipioCartorio.ToUpper().Trim())
                    != (dadosCadastraisAluno.MunicipioCartorio.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.MunicipioCartorio.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "MUNICIPIO CARTÓRIO", dadosBase.MunicipioCartorioNome, dadosCadastraisAluno.MunicipioCartorioNome);
                }

                if (dadosBase.IdCartorio != dadosCadastraisAluno.IdCartorio)
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "CARTÓRIO", dadosBase.NomeCartorio, dadosCadastraisAluno.NomeCartorio);
                }

                if ((dadosBase.CertNascNum.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.CertNascNum.ToUpper().Trim())
                    != (dadosCadastraisAluno.CertNascNum.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.CertNascNum.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "NÚMERO DO TERMO", dadosBase.CertNascNum, dadosCadastraisAluno.CertNascNum);
                }

                if (dadosBase.CertNascEmissao != dadosCadastraisAluno.CertNascEmissao)
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "DATA EMISSÃO CERTIDÃO", Convert.ToString(dadosBase.CertNascEmissao), Convert.ToString(dadosCadastraisAluno.CertNascEmissao));
                }

                if ((dadosBase.CertNascCartorioUf.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.CertNascCartorioUf.ToUpper().Trim())
                    != (dadosCadastraisAluno.CertNascCartorioUf.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.CertNascCartorioUf.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "ESTADO CERTIDÃO", dadosBase.CertNascCartorioUf, dadosCadastraisAluno.CertNascCartorioUf);
                }

                if ((dadosBase.CertNascFolha.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.CertNascFolha.ToUpper().Trim())
                    != (dadosCadastraisAluno.CertNascFolha.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.CertNascFolha.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "FOLHA", dadosBase.CertNascFolha, dadosCadastraisAluno.CertNascFolha);
                }

                if ((dadosBase.CertNascLivro.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.CertNascLivro.ToUpper().Trim())
                    != (dadosCadastraisAluno.CertNascLivro.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.CertNascLivro.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "LIVRO", dadosBase.CertNascLivro, dadosCadastraisAluno.CertNascLivro);
                }

                if ((dadosBase.CertNumeroMatricula.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosBase.CertNumeroMatricula.ToUpper().Trim())
                    != (dadosCadastraisAluno.CertNumeroMatricula.IsNullOrEmptyOrWhiteSpace() ? string.Empty : dadosCadastraisAluno.CertNumeroMatricula.ToUpper().Trim()))
                {
                    rnHistoricoAlteracaoAlunoCampos.Insere(contexto, historicoAlteracaoAluno.HistoricoAlteracaoAlunoId, "NÚMERO MATRICULA", dadosBase.CertNumeroMatricula, dadosCadastraisAluno.CertNumeroMatricula);
                }

                //Alterar dados tabela pessoa
                this.AlteraDadosCadastrais(contexto, dadosCadastraisAluno);

                //verifica se a pessoa esta dentro da tabela LY_FL_PESSOA
                if (rnFlPessoa.ExistePor(contexto, dadosCadastraisAluno.Pessoa))
                {
                    //Alterar flPessoa
                    rnFlPessoa.AlteraDadosCadastrais(contexto, dadosCadastraisAluno.Pessoa, dadosCadastraisAluno.ZonaResidencial, dadosCadastraisAluno.TipoCertidao, dadosCadastraisAluno.ComplementoIdentidade, dadosCadastraisAluno.CertidaoCivil);
                }
                else
                {
                    //inseri flPessoa
                    rnFlPessoa.InsereDadosCadastrais(contexto, dadosCadastraisAluno.Pessoa, dadosCadastraisAluno.ZonaResidencial, dadosCadastraisAluno.TipoCertidao, dadosCadastraisAluno.ComplementoIdentidade, dadosCadastraisAluno.CertidaoCivil);
                }

                //Inserir dados na tabela de Declaracao de ausencia
                //1º Remove as declaracao anteriores (menos a de necessidade especial pq nao esta neste tela)
                RN.DeclaracaoAusencia.RemoverDadosCadastraisPorAluno(aluno, contexto);

                foreach (var declaracaoAusencia in declaracoesAusencia)
                {
                    //2º Insere as declarações necessárias
                    declaracaoAusencia.AlunoId = aluno;
                    declaracaoAusencia.Matricula = dadosCadastraisAluno.UsuarioId;
                    RN.DeclaracaoAusencia.Inserir(declaracaoAusencia, contexto);
                }
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

        private void AlteraDadosCadastrais(DataContext ctx, RecursosHumanos.DTO.DadosCadastraisAluno dadosCadastraisAluno)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE LY_PESSOA SET 
                                NOME_COMPL = @NOME_COMPL,
                                DT_NASC = @DT_NASC,
                                MUNICIPIO_NASC = @MUNICIPIO_NASC,
                                PAIS_NASC =	@PAIS_NASC,
                                NACIONALIDADE =	@NACIONALIDADE,
                                SEXO = @SEXO,
                                EST_CIVIL =	@EST_CIVIL,
                                ENDERECO = @ENDERECO,
                                END_NUM	= @END_NUM,
                                END_COMPL =	@END_COMPL,
                                BAIRRO = @BAIRRO,
                                END_MUNICIPIO =	@END_MUNICIPIO,
                                CEP	= @CEP,
                                FONE = @FONE,
                                RG_NUM = @RG_NUM,
                                RG_EMISSOR = @RG_EMISSOR,
                                RG_TIPO = @RG_TIPO, 
                                RG_DTEXP = @RG_DTEXP,
                                RG_UF =	@RG_UF,
                                CPF	= @CPF,
                                E_MAIL = @E_MAIL,
                                CELULAR	= @CELULAR, 
                                CERT_NASC_NUM = @CERT_NASC_NUM,
                                CERT_NASC_FOLHA = @CERT_NASC_FOLHA,
                                CERT_NASC_LIVRO = @CERT_NASC_LIVRO,
                                CERT_NASC_EMISSAO = @CERT_NASC_EMISSAO,
                                CERT_NASC_CARTORIO_UF = @CERT_NASC_CARTORIO_UF,
                                ETNIA = @ETNIA,
                                QT_FILHOS = @QT_FILHOS,
                                PRE_NOME_SOCIAL = @PRE_NOME_SOCIAL,
                                CERT_NUMERO_MATRICULA = @CERT_NUMERO_MATRICULA,
                                ID_CARTORIO = @ID_CARTORIO,
                                NOME_PAI = @NOME_PAI ,
                                NOME_MAE = @NOME_MAE,
                                MAE_FALECIDA = @MAE_FALECIDA ,
                                PAI_FALECIDO = @PAI_FALECIDO ,
                                MAE_CPF = @MAE_CPF ,
                                PAI_CPF = @PAI_CPF ,
                                MAE_TELEFONE = @MAE_TELEFONE ,
                                PAI_TELEFONE = @PAI_TELEFONE ,
                                RESPONSAVEL = @RESPONSAVEL,
                                RESP_NOME_COMPL = @RESP_NOME_COMPL,
                                RESP_CPF = @RESP_CPF,
                                RESP_FONE = @RESP_FONE,
                                AREA_ASSENTAMENTO = @AREA_ASSENTAMENTO, 
                                TERRA_INDIGENA = @TERRA_INDIGENA, 
                                AREA_QUILOMBOS = @AREA_QUILOMBOS, 
                                AREA_TRADICIONAL = @AREA_TRADICIONAL,
                                STAMP_ATUALIZACAO=GETDATE(),
                                USUARIOID = @USUARIOID,
                                DATAALTERACAO = GETDATE()
                             WHERE PESSOA = @PESSOA ";

            contextQuery.Parameters.Add("@PESSOA", TechneDbType.T_NUMERO, dadosCadastraisAluno.Pessoa);
            contextQuery.Parameters.Add("@NOME_COMPL", dadosCadastraisAluno.Nome_compl.ToUpper());
            contextQuery.Parameters.Add("@PRE_NOME_SOCIAL", dadosCadastraisAluno.PreNomeSocial);
            contextQuery.Parameters.Add("@DT_NASC", TechneDbType.T_DATA, dadosCadastraisAluno.Dt_nasc);
            contextQuery.Parameters.Add("@SEXO", dadosCadastraisAluno.Sexo);
            contextQuery.Parameters.Add("@QT_FILHOS", TechneDbType.T_NUMERO_PEQUENO, dadosCadastraisAluno.QtFilhos);
            contextQuery.Parameters.Add("@ETNIA", dadosCadastraisAluno.Etnia);
            contextQuery.Parameters.Add("@EST_CIVIL", dadosCadastraisAluno.Est_civil);
            contextQuery.Parameters.Add("@PAIS_NASC", dadosCadastraisAluno.Pais_nasc);
            contextQuery.Parameters.Add("@NACIONALIDADE", dadosCadastraisAluno.Nacionalidade);
            contextQuery.Parameters.Add("@MUNICIPIO_NASC", dadosCadastraisAluno.Municipio_nasc);
            contextQuery.Parameters.Add("@NOME_MAE", dadosCadastraisAluno.NomeMae.ToUpper());
            contextQuery.Parameters.Add("@MAE_FALECIDA", dadosCadastraisAluno.MaeFalecida);
            contextQuery.Parameters.Add("@MAE_CPF", dadosCadastraisAluno.MaeCpf);
            contextQuery.Parameters.Add("@MAE_TELEFONE", dadosCadastraisAluno.MaeTelefone);
            contextQuery.Parameters.Add("@NOME_PAI", dadosCadastraisAluno.NomePai.ToUpper());
            contextQuery.Parameters.Add("@PAI_FALECIDO", dadosCadastraisAluno.PaiFalecido);
            contextQuery.Parameters.Add("@PAI_CPF", dadosCadastraisAluno.PaiCpf);
            contextQuery.Parameters.Add("@PAI_TELEFONE", dadosCadastraisAluno.PaiTelefone);
            contextQuery.Parameters.Add("@RESPONSAVEL", dadosCadastraisAluno.Responsavel);
            contextQuery.Parameters.Add("@RESP_NOME_COMPL", dadosCadastraisAluno.RespNomeCompl.IsNullOrEmptyOrWhiteSpace() ? null : dadosCadastraisAluno.RespNomeCompl.ToUpper());
            contextQuery.Parameters.Add("@RESP_CPF", dadosCadastraisAluno.RespCpf);
            contextQuery.Parameters.Add("@RESP_FONE", dadosCadastraisAluno.RespFone);
            contextQuery.Parameters.Add("@CEP", dadosCadastraisAluno.Cep);
            contextQuery.Parameters.Add("@END_MUNICIPIO", dadosCadastraisAluno.End_municipio);
            contextQuery.Parameters.Add("@ENDERECO", dadosCadastraisAluno.Endereco);
            contextQuery.Parameters.Add("@END_NUM", dadosCadastraisAluno.End_num);
            contextQuery.Parameters.Add("@END_COMPL", dadosCadastraisAluno.End_compl);
            contextQuery.Parameters.Add("@BAIRRO", dadosCadastraisAluno.Bairro);
            contextQuery.Parameters.Add("@AREA_ASSENTAMENTO", dadosCadastraisAluno.AreaAssentamento);
            contextQuery.Parameters.Add("@TERRA_INDIGENA", dadosCadastraisAluno.TerraIndigena);
            contextQuery.Parameters.Add("@AREA_QUILOMBOS", dadosCadastraisAluno.AreaQuilombos);
            contextQuery.Parameters.Add("@AREA_TRADICIONAL", dadosCadastraisAluno.AreaTradicional);
            contextQuery.Parameters.Add("@FONE", dadosCadastraisAluno.Fone);
            contextQuery.Parameters.Add("@E_MAIL", dadosCadastraisAluno.E_mail);
            contextQuery.Parameters.Add("@CELULAR", dadosCadastraisAluno.Celular);
            contextQuery.Parameters.Add("@CPF", dadosCadastraisAluno.Cpf);
            contextQuery.Parameters.Add("@RG_TIPO", dadosCadastraisAluno.Rg_tipo);
            contextQuery.Parameters.Add("@RG_NUM", dadosCadastraisAluno.Rg_num);
            contextQuery.Parameters.Add("@RG_EMISSOR", dadosCadastraisAluno.Rg_emissor);
            contextQuery.Parameters.Add("@RG_DTEXP", TechneDbType.T_DATA, dadosCadastraisAluno.Rg_dtexp);
            contextQuery.Parameters.Add("@RG_UF", dadosCadastraisAluno.Rg_uf);
            contextQuery.Parameters.Add("@ID_CARTORIO", dadosCadastraisAluno.IdCartorio);
            contextQuery.Parameters.Add("@CERT_NASC_NUM", dadosCadastraisAluno.CertNascNum);
            contextQuery.Parameters.Add("@CERT_NASC_FOLHA", dadosCadastraisAluno.CertNascFolha);
            contextQuery.Parameters.Add("@CERT_NASC_LIVRO", dadosCadastraisAluno.CertNascLivro);
            contextQuery.Parameters.Add("@CERT_NASC_EMISSAO", TechneDbType.T_DATA, dadosCadastraisAluno.CertNascEmissao);
            contextQuery.Parameters.Add("@CERT_NASC_CARTORIO_UF", dadosCadastraisAluno.CertNascCartorioUf);
            contextQuery.Parameters.Add("@CERT_NUMERO_MATRICULA", dadosCadastraisAluno.CertNumeroMatricula);
            contextQuery.Parameters.Add("@USUARIOID", dadosCadastraisAluno.UsuarioId);

            ctx.ApplyModifications(contextQuery);
        }
    }
}