using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Data;
using Techne.Lyceum.RN.Entidades;
using Seeduc.Infra.Data;
using System.Data;
using Techne.Lyceum.RN.Util;
using Microsoft.VisualBasic.ApplicationServices;
using System.Data.SqlClient;
using Techne.Lyceum.RN.DTOs;

namespace Techne.Lyceum.RN
{
    public class VinculoLy : RNBase
    {
        public bool PossuiOutroVinculoPor(DataContext ctx, decimal pessoa, int vinculo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM LY_VINCULO (NOLOCK)
                                        WHERE PESSOA = @PESSOA
	                                        AND VINCULO = @VINCULO ";

            contextQuery.Parameters.Add("@PESSOA", pessoa);
            contextQuery.Parameters.Add("@VINCULO", vinculo);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public bool EhServidorPor(decimal pessoa)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                possui = this.EhServidorPor(ctx, pessoa);

                return possui;
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public bool EhServidorPor(DataContext ctx, decimal pessoa)
        {
            bool possui = false;

            ContextQuery contextQuery = new ContextQuery
            {
                Command = @" SELECT COUNT(*) 
                                    FROM   LY_VINCULO 
                                    WHERE  PESSOA = @PESSOA "
            };

            contextQuery.Parameters.Add("@PESSOA", pessoa);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public bool EhServidorAtivoPor(DataContext ctx, decimal pessoa, decimal ordem)
        {
            bool possui = false;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = @" SELECT COUNT(*) 
                                FROM   LY_VINCULO 
                                WHERE  PESSOA = @PESSOA
	                                AND ORDEM = @ORDEM
	                                AND DATA_DESATIVACAO IS NULL "
                };

                contextQuery.Parameters.Add("@PESSOA", pessoa);
                contextQuery.Parameters.Add("@ORDEM", ordem);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    possui = true;
                }

                return possui;
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public DataTable ObtemMatriculaIdVinculoPor(decimal pessoa)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT DISTINCT D.MATRICULA, 
                                                D.MATRICULA + ';' + ISNULL(CONVERT(VARCHAR(20),P.IDFUNCIONAL),'') + ';' + ISNULL(CONVERT(VARCHAR(20),D.VINCULO),'') as MATRICULADESC, 
		                                        P.IDFUNCIONAL,
		                                        D.VINCULO,
		                                        ISNULL((CONVERT(VARCHAR, P.IDFUNCIONAL) + '/' + CONVERT(VARCHAR ,D.VINCULO)), D.MATRICULA) IDVINCULO
                                        FROM LY_VINCULO D
		                                        INNER JOIN LY_PESSOA P ON D.PESSOA = P.PESSOA
                                        WHERE D.PESSOA = @PESSOA ";

                contextQuery.Parameters.Add("@PESSOA", pessoa);

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

        public int ObtemOrdemVinculoAtivoPor(DataContext contexto, string matricula)
        {
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            int retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT TOP 1 ORDEM 
                                        FROM LY_VINCULO (NOLOCK)
                                        WHERE MATRICULA = @MATRICULA 
	                                        AND DATA_DESATIVACAO IS NULL ";

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToInt32(reader["ORDEM"]);
                }

                return retorno;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public bool PossuiVinculoServidorPor(DataContext ctx, decimal pessoa)
        {
            bool possui = false;

            ContextQuery contextQuery = new ContextQuery
            {
                Command = @" SELECT  COUNT(*)
                                    FROM   LY_VINCULO 
                                    WHERE  PESSOA = @PESSOA "
            };

            contextQuery.Parameters.Add("@PESSOA", pessoa);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public DataTable ListaIdVinculo(string pessoa)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            ContextQuery contextQuery = new ContextQuery();
            DataTable dt = null;

            try
            {
                contextQuery.Command = @" SELECT ISNULL((convert(varchar,p.IDFUNCIONAL) + '/' + convert(varchar,V.VINCULO)),v.matricula) as idvinculo, 
		                                    v.matricula, 
		                                    p.idfuncional,
		                                    V.VINCULO,
		                                    v.ordem, 
		                                    v.data_nomeacao, 
		                                    v.data_desativacao, 
		                                    v.categoria, 
		                                     convert(int,v.ch_categoria) ch_categoria
                                    from LY_VINCULO v 
	                                    inner join LY_PESSOA p on v.PESSOA = p.PESSOA
                                    where p.PESSOA = @pessoa  ";

                contextQuery.Parameters.Add("@PESSOA", pessoa);

                dt = ctx.GetDataTable(contextQuery);
            }
            catch (Exception ex)
            {
                ctx.Abandon();
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
                ctx.Dispose();
            }

            return dt;
        }
        public decimal ObtemPessoaPor(string matricula)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();
            decimal retorno = 0;

            try
            {
                contextQuery.Command = @" SELECT PESSOA
                                            FROM LY_VINCULO (NOLOCK)
                                            WHERE MATRICULA = @MATRICULA ";

                contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, matricula);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    retorno = Convert.ToDecimal(reader["PESSOA"]);
                }

                return retorno;
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

        public ValidacaoDados ValidaServidor(LyPessoa pessoa, LyVinculo vinculo, bool cadastro, string SemLocalizacaoDiferenciada, string zonaResidencial, RecursosHumanos.Entidades.GoogleEducation googleEducation, string povoIndigenaId)
        {
            DataContext contexto = null;
            RN.Validacao rnValidacao = new Validacao();
            RN.Docentes rnDocentes = new Docentes();
            RN.Pessoa rnPessoa = new Pessoa();
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (pessoa == null)
            {
                return validacaoDados;
            }

            if (!cadastro)
            {

                if (pessoa.Pessoa <= 0)
                {
                    mensagens.Add("O campo PESSOA é de preenchimento obrigatório.");
                }
            }

            if (pessoa.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo USUÁRIO RESPONSÁVEL é de preenchimento obrigatório.");
            }

            if (pessoa.Nome_compl.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo NOME é de preenchimento obrigatório.");
            }
            else
            {
                //Nome abreviado
                var nomes = pessoa.Nome_compl.Trim().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                for (var i = 0; i < nomes.Length; i++)
                {
                    var nome = nomes[i];

                    if (nome.Length == 2 && !rnValidacao.ehAbreviacaoValida(nome) && nome.IndexOf(".") != -1)
                    {
                        nome = nome.Remove(1);
                    }

                    if ((nome.Length == 1 && (string.Compare(nome, "e", true) != 0)) ||
                        ((string.Compare(nome, "e", true) == 0) && (i == 0 || i == nomes.Length - 1)))
                    {
                        mensagens.Add("Não é possível utilizar abreviações no nome.");
                    }
                }

                //Verificar nome valido
                int n = 0;
                for (n = 0; n <= 9; n++)
                {
                    if (pessoa.Nome_compl.IndexOf(n.ToString()) > 0)
                    {
                        mensagens.Add("Nome Completo: Não se pode ter números no nome.(" + n.ToString() + ").");
                    }
                }

                string[] vetorNome = pessoa.Nome_compl.Split(' ');

                if (vetorNome.Length == 1)
                {
                    mensagens.Add("Nome Completo: O Nome não pode ser formado por apenas uma palavra.");
                }

                if (Utils.VerificaTriploCaracter(pessoa.Nome_compl))
                {
                    mensagens.Add("Nome Completo: Não se pode ter três letras iguais consecutivas no nome.");
                }
            }

            if (pessoa.Sexo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo SEXO é de preenchimento obrigatório.");
            }

            if (pessoa.Etnia.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo COR/RAÇA é de preenchimento obrigatório.");
            }
            else
            {
                //Verifica se foi informado o codigo do povo indigena e se a cor/raça é indigena
                if (!povoIndigenaId.IsNullOrEmptyOrWhiteSpace() && pessoa.Etnia != "Índigena")
                {
                    mensagens.Add("Apenas pode ser informado o campo POVO ÍNDIGENA quando o campo COR/RAÇA for Índigena.");
                }

                if (pessoa.Etnia == "Índigena" && povoIndigenaId.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo POVO INDÍGENA é obrigatório!");
                }
            }

            if (pessoa.NecessidadeEspecialId == null || pessoa.NecessidadeEspecialId <= 0)
            {
                mensagens.Add("O campo NECESSIDADE ESPECIAL é obrigatório.");
            }

            if (pessoa.Est_civil.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo ESTADO CIVIL é de preenchimento obrigatório.");
            }

            if (pessoa.Dt_nasc == DateTime.MinValue || pessoa.Dt_nasc == null)
            {
                mensagens.Add("O campo DATA DE NASCIMENTO é de preenchimento obrigatório.");
            }
            else
            {
                if (!Validacao.ValidouData(pessoa.Dt_nasc, Validacao.Tipo.data))
                {
                    mensagens.Add("DATA DE NASCIMENTO inválida.<br>A data de nascimento deve ser maior que 1900 e não pode ser maior que a data de hoje.");
                }
            }

            if (pessoa.End_pais.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo PAÍS DE ENDEREÇO é de preenchimento obrigatório.");
            }

            if (pessoa.Nacionalidade.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo NACIONALIDADE é de preenchimento obrigatório.");
            }
            else
            {
                if (pessoa.Pais_nasc == "1" && pessoa.Nacionalidade != "BRASILEIRA")
                {
                    mensagens.Add("O campo PAÍS DE NASCIMENTO não pode ser Brasil com a NACIONALIDADE diferente de brasileira.");
                }

                if (pessoa.Pais_nasc != "1" && pessoa.Nacionalidade == "BRASILEIRA")
                {
                    mensagens.Add("O campo PAÍS DE NASCIMENTO não pode ser diferente de Brasil e a NACIONALIDADE ser brasileira.");
                }

                if (pessoa.Nacionalidade == "BRASILEIRA")
                {
                    if (string.IsNullOrEmpty(pessoa.Municipio_nasc) || pessoa.Municipio_nasc == "<Nenhum>")
                    {
                        mensagens.Add("O campo MUNICÍPIO NASCIMENTO (NATURALIDADE) é de preenchimento obrigatório.");
                    }
                }
            }

            if (pessoa.Municipio_nasc.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo MUNICÍPIO DE NASCIMENTO (NATURALIDADE) é de preenchimento obrigatório.");
            }

            if (pessoa.NomeMae.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo NOME DA MÃE é de preenchimento obrigatório.");
            }
            if (pessoa.NomePai.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo NOME DO PAI é de preenchimento obrigatório.");
            }

            if (pessoa.End_pais.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo PAÍS DO ENDEREÇO é de preenchimento obrigatório.");
            }

            if (pessoa.Cep.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo CEP é de preenchimento obrigatório.");
            }
            else
            {
                var cep = RN.Util.Utils.RetirarMascara(pessoa.Cep);

                if (!Validacao.ValidarCEP(cep))
                {
                    mensagens.Add("CEP inválido! Este CEP não foi encontrado em nossa base.");
                }
            }

            if (pessoa.End_municipio.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo MUNICÍPIO (ENDEREÇO RESIDENCIAL) é de preenchimento obrigatório.");
            }
            if (pessoa.Endereco.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo ENDEREÇO DA PESSOA é de preenchimento obrigatório.");
            }
            if (pessoa.End_num.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo NÚMERO DO ENDEREÇO é de preenchimento obrigatório.");
            }
            if (pessoa.Bairro.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo BAIRRO é de preenchimento obrigatório.");
            }

            if (zonaResidencial.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O  campo LOCALIZAÇÃO é de preenchimento obrigatório.");
            }

            if (!string.IsNullOrEmpty(pessoa.Id_censo))
            {
                if (!RN.Pessoa.VerificarInep(pessoa.Id_censo, pessoa.Pessoa))
                {
                    mensagens.Add("O Número de Identificação no INEP já está sendo usado por outra pessoa.");
                }
            }

            if (pessoa.AreaQuilombos.IsNullOrEmptyOrWhiteSpace()
               || (pessoa.AreaQuilombos != "N" && pessoa.AreaQuilombos != "S"))
            {
                mensagens.Add("O campo AREA DE QUILOMBOS é obrigatório com os Valores N ou S.");
            }

            if (pessoa.TerraIndigena.IsNullOrEmptyOrWhiteSpace()
                || (pessoa.TerraIndigena != "N" && pessoa.TerraIndigena != "S"))
            {
                mensagens.Add("O campo TERRA INDIGENA é obrigatório com os Valores N ou S.");
            }

            if (pessoa.AreaAssentamento.IsNullOrEmptyOrWhiteSpace()
                || (pessoa.AreaAssentamento != "N" && pessoa.AreaAssentamento != "S"))
            {
                mensagens.Add("O campo AREA DE ASSENTAMENTO é obrigatório com os Valores N ou S.");
            }

            if (SemLocalizacaoDiferenciada.IsNullOrEmptyOrWhiteSpace()
                || (SemLocalizacaoDiferenciada != "N" && SemLocalizacaoDiferenciada != "S"))
            {
                mensagens.Add("O campo SEM LOCALIZAÇÃO DIFERENCIADA é obrigatório com os Valores N ou S.");
            }
            else
            {
                if (SemLocalizacaoDiferenciada == "N")
                {
                    if ((pessoa.AreaAssentamento.IsNullOrEmptyOrWhiteSpace() || pessoa.AreaAssentamento == "N")
                        && (pessoa.TerraIndigena.IsNullOrEmptyOrWhiteSpace() || pessoa.TerraIndigena == "N")
                        && (pessoa.AreaQuilombos.IsNullOrEmptyOrWhiteSpace() || pessoa.AreaQuilombos == "N"))
                    {
                        mensagens.Add("O campo LOCALIZAÇÃO DIFERENCIADA é obrigatório.");
                    }
                }
                else
                {
                    if ((!pessoa.AreaAssentamento.IsNullOrEmptyOrWhiteSpace() && pessoa.AreaAssentamento == "S")
                        || (!pessoa.TerraIndigena.IsNullOrEmptyOrWhiteSpace() && pessoa.TerraIndigena == "S")
                        || (!pessoa.AreaQuilombos.IsNullOrEmptyOrWhiteSpace() && pessoa.AreaQuilombos == "S"))
                    {
                        mensagens.Add("O campo LOCALIZAÇÃO DIFERENCIADA não pode possuir outra marcação quando NÃO SE APLICA estiver selecionado.");
                    }
                }
            }

            if (pessoa.Cpf.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo CPF é de preenchimento obrigatório.");
            }
            else
            {
                if (!Utils.ValidarCpf(Utils.RetirarMascara(pessoa.Cpf)))
                {
                    mensagens.Add("CPF inválido.");
                }
                else
                {
                    pessoa.Cpf = pessoa.Cpf.RetirarMascaraCPF();
                }
            }

            if (!pessoa.Fone.IsNullOrEmptyOrWhiteSpace())
            {
                if (!Validacao.ValidaTelefoneComDDD(pessoa.Fone.RetirarMascaraTelefone()))
                {
                    mensagens.Add("TELEFONE inválido.");
                }
            }

            if (!pessoa.Celular.IsNullOrEmptyOrWhiteSpace())
            {
                if (!Validacao.ValidaCelularComDDD(pessoa.Celular.RetirarMascaraTelefone()))
                {
                    mensagens.Add("CELULAR inválido.");
                }
            }

            if (!pessoa.E_mail_interno.IsNullOrEmptyOrWhiteSpace()
              && !(pessoa.E_mail_interno.Split('@')[1].Trim() == "prof.educacao.rj.gov.br"
              || pessoa.E_mail_interno.Split('@')[1].Trim() == "educacao.rj.gov.br"))
            {
                mensagens.Add("No campo E-MAIL OFFICE 365 serão aceitos apenas e-mails institucionais @educacao.rj.gov.br ou @prof.educacao.rj.gov.br");
            }

            if (!pessoa.E_mail.IsNullOrEmptyOrWhiteSpace())
            {
                if (!Validacao.Email(pessoa.E_mail))
                {
                    mensagens.Add("O campo E-MAIL ALTERNATIVO está em um formato incorreto!");
                }
            }

            #region Validações dos Campos de Documento
            bool documentoValido, iniciouMensagem, maisDeUmCampo;
            documentoValido = true;
            iniciouMensagem = maisDeUmCampo = false;
            System.Text.StringBuilder mensagemDocumento = new System.Text.StringBuilder();
            System.Text.StringBuilder camposDocumento = new System.Text.StringBuilder();
            mensagemDocumento.Append("Documento:<br>Não é possível deixar de preencher um dos campos referentes ao tipo de documento.");

            if (!pessoa.Rg_tipo.IsNullOrEmptyOrWhiteSpace() && pessoa.Rg_tipo != "<Nenhum>")
            {
                if (Convert.ToString(pessoa.Rg_num).IsNullOrEmptyOrWhiteSpace())
                {
                    documentoValido = false;
                    iniciouMensagem = true;
                    camposDocumento.Append("Número ");
                }
                else
                {
                    var rg = Utils.RetirarMascara(pessoa.Rg_num);

                    if (rg.Length < 5)
                    {
                        mensagens.Add("O NÚMERO DO DOCUMENTO deve conter no mínimo cinco dígitos!");
                    }

                    if (pessoa.Rg_tipo == "RG" && pessoa.Rg_emissor == "DETRAN" && pessoa.Rg_uf == "RJ")
                    {
                        if (!Validacao.ValidaNumerosInteirosPositivos(pessoa.Rg_num))
                        {
                            mensagens.Add("O número de documento do RG Detran deve conter só números inteiros.");
                        }
                    }
                }

                if (pessoa.Rg_tipo == "RG")
                {
                    if (Convert.ToString(pessoa.Rg_uf).IsNullOrEmptyOrWhiteSpace())
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

                    if (Convert.ToString(pessoa.Rg_emissor).IsNullOrEmptyOrWhiteSpace())
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

                    if (pessoa.Rg_dtexp == null)
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

                    if (pessoa.Rg_dtexp != null && pessoa.Rg_dtexp != DateTime.MinValue)
                    {
                        if (Convert.ToDateTime(pessoa.Dt_nasc).Date >= Convert.ToDateTime(pessoa.Rg_dtexp).Date)
                        {
                            mensagens.Add("A Data de expedição da identidade tem que ser maior que a data de nascimento.");
                        }
                    }
                }
                else
                {
                    if (pessoa.Rg_tipo == "PASSAPORTE" && pessoa.Nacionalidade == "BRASILEIRA")
                    {
                        mensagens.Add("Para Nacionalidade 'BRASILEIRA' o documento obrigatório é o RG.");
                    }
                }
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

            if (!string.IsNullOrEmpty(pessoa.Cprof_num) &&
                !Validacao.Validou(pessoa.Cprof_num, Validacao.Tipo.numerico))
            {
                mensagens.Add("Número da carteira profissional inválido.<br>O número da carteira profissional deve ter somente números.");
            }

            //Dados carteira profissional
            if (!string.IsNullOrEmpty(pessoa.Cprof_num) && (string.IsNullOrEmpty(pessoa.Cprof_dtexp.ToString())
                || string.IsNullOrEmpty(pessoa.Cprof_serie) || string.IsNullOrEmpty(pessoa.Cprof_uf)))
            {
                mensagens.Add("Caso um dado da carteira profissional seja preenchido, todos os outros devem ser preenchidos também.");
            }
            else if (!string.IsNullOrEmpty(pessoa.Cprof_dtexp.ToString()) && (string.IsNullOrEmpty(pessoa.Cprof_num)
                || string.IsNullOrEmpty(pessoa.Cprof_serie) || string.IsNullOrEmpty(pessoa.Cprof_uf)))
            {
                mensagens.Add("Caso um dado da carteira profissional seja preenchido, todos os outros devem ser preenchidos também.");
            }
            else if (!string.IsNullOrEmpty(pessoa.Cprof_serie) && (string.IsNullOrEmpty(pessoa.Cprof_num)
                || string.IsNullOrEmpty(pessoa.Cprof_dtexp.ToString()) || string.IsNullOrEmpty(pessoa.Cprof_uf)))
            {
                mensagens.Add("Caso um dado da carteira profissional seja preenchido, todos os outros devem ser preenchidos também.");
            }
            else if (!string.IsNullOrEmpty(pessoa.Cprof_uf) && (string.IsNullOrEmpty(pessoa.Cprof_num)
                || string.IsNullOrEmpty(pessoa.Cprof_dtexp.ToString()) || string.IsNullOrEmpty(pessoa.Cprof_serie)))
            {
                mensagens.Add("Caso um dado da carteira profissional seja preenchido, todos os outros devem ser preenchidos também.");
            }

            #endregion

            if (!Validacao.ValidouDuasDatas(pessoa.Dt_nasc, pessoa.Rg_dtexp))
            {
                mensagens.Add("Data expedição documento/nascimento inválidas.<br>A data de expedição do documento de indentificação deve ser maior que a data de nascimento.");
            }
            if (!Validacao.ValidouDuasDatas(pessoa.Dt_nasc, pessoa.Cprof_dtexp))
            {
                mensagens.Add("Data expedição carteira profissional/nascimento inválidas.<br>A data de expedição da carteira profissional deve ser maior que a data de nascimento.");
            }
            if (!Validacao.ValidouDuasDatas(pessoa.Dt_nasc, pessoa.CertNascEmissao))
            {
                mensagens.Add("Data de emissão da certidão/nascimento inválidas.<br>A data de emissão da certidão de nascimento deve ser maior que a data de nascimento.");
            }
            if (!Validacao.ValidouDuasDatas(pessoa.Dt_nasc, pessoa.Teleitor_dtexp))
            {
                mensagens.Add("Data de emissão do título de eleitor/nascimento inválidas.<br>A data de emissão do título de eleitor deve ser maior que a data de nascimento.");
            }
            if (!Validacao.ValidouDuasDatas(pessoa.Dt_nasc, pessoa.Alist_dtexp))
            {
                mensagens.Add("Data de emissão do alistamento militar/nascimento inválidas.<br>A data de emissão do alistamento militar deve ser maior que a data de nascimento.");
            }
            if (!Validacao.ValidouDuasDatas(pessoa.Dt_nasc, pessoa.Cr_dtexp))
            {
                mensagens.Add("Data de emissão do certificado de reservista/nascimento inválidas.<br>A data de emissão do certificado de reservista deve ser maior que a data de nascimento.");
            }

            //Se for cadastro valida vinculo e IdFuncional
            if (cadastro)
            {
                if (pessoa.IdFuncional == null || pessoa.IdFuncional <= 0)
                {
                    mensagens.Add("O campo ID FUNCIONAL é de preenchimento obrigatório.");
                }

                if (vinculo.Vinculo == null || vinculo.Vinculo <= 0)
                {
                    mensagens.Add("O campo VÍNCULO é de preenchimento obrigatório.");
                }

                if (vinculo.DataNomeacao == null || vinculo.DataNomeacao == DateTime.MinValue)
                {
                    mensagens.Add("O campo DATA DE ADMISSÃO é de preenchimento obrigatório.");
                }

                if (vinculo.Categoria.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("O campo CARGO é de preenchimento obrigatório.");
                }

                if (vinculo.ChCategoria == null || vinculo.ChCategoria <= 0)
                {
                    mensagens.Add("O campo CARGA HORÁRIA DO CARGO é de preenchimento obrigatório.");
                }
            }

            if (!cadastro)
            {
                if (googleEducation != null)
                {
                    if (!googleEducation.Email.IsNullOrEmptyOrWhiteSpace()
                     && !(googleEducation.Email.Split('@')[1].Trim() == "prof.educa.rj.gov.br"
                     || googleEducation.Email.Split('@')[1].Trim() == "educa.rj.gov.br"))
                    {
                        mensagens.Add("No campo E-MAIL GOOGLE FOR EDUCATION serão aceitos apenas e-mails @educa.rj.gov.br ou @prof.educa.rj.gov.br");
                    }
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Valida CPF existente
                    if (rnPessoa.PossuiOutroCPFPor(contexto, pessoa.Cpf, pessoa.Pessoa))
                    {
                        if (pessoa.Pessoa <= 0)
                        {
                            mensagens.Add("CPF já existente. Favor utilizar a busca através da Pessoa .");
                        }
                        else
                        {
                            mensagens.Add("CPF já cadastrado para outra Pessoa .");
                        }
                    }
                    else
                    {
                        //Valida Nome, mae e data de nascimento existente 
                        if (rnPessoa.PossuiOutroNomeMaeDataNascimentoPor(contexto, pessoa.Nome_compl, pessoa.NomeMae, Convert.ToDateTime(pessoa.Dt_nasc), pessoa.Pessoa))
                        {
                            mensagens.Add("Nome/data de nascimento/nome da mãe já existente. Favor utilizar a busca através da Pessoa.");
                        }
                    }

                    if (cadastro)
                    {
                        vinculo.Matricula = string.Format("{0}/{1}", pessoa.IdFuncional, vinculo.Vinculo);

                        //Valida matricula
                        if (rnDocentes.PossuiOutraMatriculaPor(contexto, vinculo.Matricula, pessoa.Pessoa))
                        {
                            mensagens.Add("Número de matrícula ou id/Vinculo já cadastrado para outro docente.");
                        }

                        if (this.PossuiOutraMatriculaPor(contexto, vinculo.Matricula, pessoa.Pessoa))
                        {
                            mensagens.Add("Número de matrícula ou id/Vinculo já cadastrado para outro servidor.");
                        }

                        //Valida ID FUNCIONAL existente                      
                        if (rnPessoa.PossuiOutroIdFuncionalPor(contexto, Convert.ToInt32(pessoa.IdFuncional), pessoa.Pessoa))
                        {
                            mensagens.Add("Já existe uma pessoa cadastrada com esse número de ID FUNCIONAL.");
                        }

                        //if (pessoa.Pessoa > 0 && this.EhServidorVigentePor(contexto, pessoa.Pessoa))
                        //{
                        //    mensagens.Add("Já existe um servidor vigente cadastrado para esta pessoa.");
                        //}

                        if (pessoa.Pessoa > 0 && this.EhVinculoExistentePor(contexto, pessoa.Pessoa, vinculo.Vinculo))
                        {
                            mensagens.Add("Vínculo já associado a essa pessoa.");
                        }
                        if (pessoa.Pessoa > 0)
                        {
                            //Valida se o vinculo já foi utilizado para esta pessoa
                            if (rnDocentes.PossuiOutroVinculoPor(contexto, pessoa.Pessoa, Convert.ToInt32(vinculo.Vinculo)) ||
                                this.PossuiOutroVinculoPor(contexto, pessoa.Pessoa, Convert.ToInt32(vinculo.Vinculo)))
                            {
                                mensagens.Add("Este VINCULO já se encontra cadastrado para esta pessoa.");
                            }
                        }
                    }
                    else
                    {
                        if (rnPessoa.PossuiOutroCPFPor(contexto, pessoa.Cpf, pessoa.Pessoa))
                        {
                            mensagens.Add("CPF já existente.");
                        }
                        else
                        {
                            //Valida Nome, mae e data de nascimento existente 
                            if (rnPessoa.PossuiOutroNomeMaeDataNascimentoPor(contexto, pessoa.Nome_compl, pessoa.NomeMae, Convert.ToDateTime(pessoa.Dt_nasc), pessoa.Pessoa))
                            {
                                mensagens.Add("Nome/data de nascimento/nome da mãe já existente.");
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

        public ValidacaoDados ValidaDadosPessoaisServidor(LyPessoa pessoa, string SemLocalizacaoDiferenciada, string zonaResidencial, string povoIndigenaId)
        {
            DataContext contexto = null;
            RN.Validacao rnValidacao = new Validacao();
            RN.Docentes rnDocentes = new Docentes();
            RN.Pessoa rnPessoa = new Pessoa();
            List<string> mensagens = new List<string>();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (pessoa == null)
            {
                return validacaoDados;
            }

            if (pessoa.Pessoa <= 0)
            {
                mensagens.Add("O campo PESSOA é de preenchimento obrigatório.");
            }

            if (pessoa.UsuarioId.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo USUÁRIO RESPONSÁVEL é de preenchimento obrigatório.");
            }

            if (pessoa.Nome_compl.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo NOME é de preenchimento obrigatório.");
            }
            else
            {
                //Nome abreviado
                var nomes = pessoa.Nome_compl.Trim().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                for (var i = 0; i < nomes.Length; i++)
                {
                    var nome = nomes[i];

                    if (nome.Length == 2 && !rnValidacao.ehAbreviacaoValida(nome) && nome.IndexOf(".") != -1)
                    {
                        nome = nome.Remove(1);
                    }

                    if ((nome.Length == 1 && (string.Compare(nome, "e", true) != 0)) ||
                        ((string.Compare(nome, "e", true) == 0) && (i == 0 || i == nomes.Length - 1)))
                    {
                        mensagens.Add("Não é possível utilizar abreviações no nome.");
                    }
                }

                //Verificar nome valido
                int n = 0;
                for (n = 0; n <= 9; n++)
                {
                    if (pessoa.Nome_compl.IndexOf(n.ToString()) > 0)
                    {
                        mensagens.Add("Nome Completo: Não se pode ter números no nome.(" + n.ToString() + ").");
                    }
                }

                string[] vetorNome = pessoa.Nome_compl.Split(' ');

                if (vetorNome.Length == 1)
                {
                    mensagens.Add("Nome Completo: O Nome não pode ser formado por apenas uma palavra.");
                }

                if (Utils.VerificaTriploCaracter(pessoa.Nome_compl))
                {
                    mensagens.Add("Nome Completo: Não se pode ter três letras iguais consecutivas no nome.");
                }
            }

            if (pessoa.Dt_nasc == DateTime.MinValue || pessoa.Dt_nasc == null)
            {
                mensagens.Add("O campo DATA DE NASCIMENTO é de preenchimento obrigatório.");
            }
            else
            {
                if (!Validacao.ValidouData(pessoa.Dt_nasc, Validacao.Tipo.data))
                {
                    mensagens.Add("DATA DE NASCIMENTO inválida.<br>A data de nascimento deve ser maior que 1900 e não pode ser maior que a data de hoje.");
                }
            }

            if (pessoa.Sexo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo SEXO é de preenchimento obrigatório.");
            }

            if (pessoa.Etnia.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo COR/RAÇA é de preenchimento obrigatório.");
            }
            else
            {
                //Verifica se foi informado o codigo do povo indigena e se a cor/raça é indigena
                if (!povoIndigenaId.IsNullOrEmptyOrWhiteSpace() && pessoa.Etnia != "Índigena")
                {
                    mensagens.Add("Apenas pode ser informado o campo POVO ÍNDIGENA quando o campo COR/RAÇA for Índigena.");
                }

                if (pessoa.Etnia == "Índigena" && povoIndigenaId.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo POVO INDÍGENA é obrigatório!");
                }
            }

            if (pessoa.NecessidadeEspecialId == null || pessoa.NecessidadeEspecialId <= 0)
            {
                mensagens.Add("O campo NECESSIDADE ESPECIAL é obrigatório.");
            }

            if (pessoa.Est_civil.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo ESTADO CIVIL é de preenchimento obrigatório.");
            }

            if (pessoa.Nacionalidade.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo NACIONALIDADE é de preenchimento obrigatório.");
            }
            else
            {
                if (pessoa.Pais_nasc == "1" && pessoa.Nacionalidade != "BRASILEIRA")
                {
                    mensagens.Add("O campo PAÍS DE NASCIMENTO não pode ser Brasil com a NACIONALIDADE diferente de brasileira.");
                }

                if (pessoa.Pais_nasc != "1" && pessoa.Nacionalidade == "BRASILEIRA")
                {
                    mensagens.Add("O campo PAÍS DE NASCIMENTO não pode ser diferente de Brasil e a NACIONALIDADE ser brasileira.");
                }

                if (pessoa.Nacionalidade == "BRASILEIRA")
                {
                    if (string.IsNullOrEmpty(pessoa.Municipio_nasc) || pessoa.Municipio_nasc == "<Nenhum>")
                    {
                        mensagens.Add("O campo MUNICÍPIO NASCIMENTO (NATURALIDADE) é de preenchimento obrigatório.");
                    }
                }
            }

            if (pessoa.Municipio_nasc.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo MUNICÍPIO DE NASCIMENTO (NATURALIDADE) é de preenchimento obrigatório.");
            }

            if (pessoa.NomeMae.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo NOME DA MÃE é de preenchimento obrigatório.");
            }
            if (pessoa.NomePai.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo NOME DO PAI é de preenchimento obrigatório.");
            }

            if (pessoa.End_pais.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo PAÍS DE ENDEREÇO é de preenchimento obrigatório.");
            }

            if (pessoa.Cep.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo CEP é de preenchimento obrigatório.");
            }
            else
            {
                var cep = RN.Util.Utils.RetirarMascara(pessoa.Cep);

                if (!Validacao.ValidarCEP(cep))
                {
                    mensagens.Add("CEP inválido! Este CEP não foi encontrado em nossa base.");
                }
            }

            if (pessoa.End_municipio.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo MUNICÍPIO (ENDEREÇO RESIDENCIAL) é de preenchimento obrigatório.");
            }

            if (pessoa.Endereco.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo ENDEREÇO DA PESSOA é de preenchimento obrigatório.");
            }

            if (pessoa.End_num.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo NÚMERO DO ENDEREÇO é de preenchimento obrigatório.");
            }

            if (pessoa.Bairro.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo BAIRRO é de preenchimento obrigatório.");
            }

            if (zonaResidencial.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo LOCALIZAÇÃO/ZONA DE RESIDÊNCIA é de preenchimento obrigatório.");
            }

            if (pessoa.AreaQuilombos.IsNullOrEmptyOrWhiteSpace()
               || (pessoa.AreaQuilombos != "N" && pessoa.AreaQuilombos != "S"))
            {
                mensagens.Add("O campo AREA DE QUILOMBOS é obrigatório com os Valores N ou S.");
            }

            if (pessoa.TerraIndigena.IsNullOrEmptyOrWhiteSpace()
                || (pessoa.TerraIndigena != "N" && pessoa.TerraIndigena != "S"))
            {
                mensagens.Add("O campo TERRA INDIGENA é obrigatório com os Valores N ou S.");
            }

            if (pessoa.AreaAssentamento.IsNullOrEmptyOrWhiteSpace()
                || (pessoa.AreaAssentamento != "N" && pessoa.AreaAssentamento != "S"))
            {
                mensagens.Add("O campo AREA DE ASSENTAMENTO é obrigatório com os Valores N ou S.");
            }

            if (SemLocalizacaoDiferenciada.IsNullOrEmptyOrWhiteSpace()
                || (SemLocalizacaoDiferenciada != "N" && SemLocalizacaoDiferenciada != "S"))
            {
                mensagens.Add("O campo SEM LOCALIZAÇÃO DIFERENCIADA é obrigatório com os Valores N ou S.");
            }
            else
            {
                if (SemLocalizacaoDiferenciada == "N")
                {
                    if ((pessoa.AreaAssentamento.IsNullOrEmptyOrWhiteSpace() || pessoa.AreaAssentamento == "N")
                        && (pessoa.TerraIndigena.IsNullOrEmptyOrWhiteSpace() || pessoa.TerraIndigena == "N")
                        && (pessoa.AreaQuilombos.IsNullOrEmptyOrWhiteSpace() || pessoa.AreaQuilombos == "N"))
                    {
                        mensagens.Add("O campo LOCALIZAÇÃO DIFERENCIADA é obrigatório.");
                    }
                }
                else
                {
                    if ((!pessoa.AreaAssentamento.IsNullOrEmptyOrWhiteSpace() && pessoa.AreaAssentamento == "S")
                        || (!pessoa.TerraIndigena.IsNullOrEmptyOrWhiteSpace() && pessoa.TerraIndigena == "S")
                        || (!pessoa.AreaQuilombos.IsNullOrEmptyOrWhiteSpace() && pessoa.AreaQuilombos == "S"))
                    {
                        mensagens.Add("O campo LOCALIZAÇÃO DIFERENCIADA não pode possuir outra marcação quando NÃO SE APLICA estiver selecionado.");
                    }
                }
            }

            if (!pessoa.Fone.IsNullOrEmptyOrWhiteSpace())
            {
                if (!Validacao.ValidaTelefoneComDDD(pessoa.Fone.RetirarMascaraTelefone()))
                {
                    mensagens.Add("TELEFONE inválido.");
                }
            }

            if (!pessoa.Celular.IsNullOrEmptyOrWhiteSpace())
            {
                if (!Validacao.ValidaCelularComDDD(pessoa.Celular.RetirarMascaraTelefone()))
                {
                    mensagens.Add("CELULAR inválido.");
                }
            }

            if (!pessoa.E_mail.IsNullOrEmptyOrWhiteSpace())
            {
                if (!Validacao.Email(pessoa.E_mail))
                {
                    mensagens.Add("O campo E-MAIL ALTERNATIVO está em um formato incorreto!");
                }
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Valida Nome, mae e data de nascimento existente 
                    if (rnPessoa.PossuiOutroNomeMaeDataNascimentoPor(contexto, pessoa.Nome_compl, pessoa.NomeMae, Convert.ToDateTime(pessoa.Dt_nasc), pessoa.Pessoa))
                    {
                        mensagens.Add("Nome/data de nascimento/nome da mãe já existente.");
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


        public ValidacaoDados Valida(LyVinculo vinculo, int? idFuncional, bool cadastro)
        {
            RN.Pessoa rnPessoa = new Pessoa();
            RN.Docentes rnDocentes = new Docentes();
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (vinculo == null)
            {
                return validacaoDados;
            }

            //Verifica se é alteração
            if (!cadastro)
            {
                if (vinculo.Ordem <= 0)
                {
                    mensagens.Add("Campo ORDEM é obrigatório.");
                }

                if (vinculo.Matricula.IsNullOrEmptyOrWhiteSpace())
                {
                    mensagens.Add("Campo MATRÍCULA OU ID/VÍNCULO é obrigatório.");
                }
            }

            if (vinculo.Pessoa <= 0)
            {
                mensagens.Add("Campo PESSOA é obrigatório.");
            }

            if (idFuncional == null || idFuncional <= 0)
            {
                mensagens.Add("O campo ID FUNCIONAL é de preenchimento obrigatório.");
            }

            if (vinculo.Vinculo == null || vinculo.Vinculo <= 0)
            {
                mensagens.Add("O campo VÍNCULO é de preenchimento obrigatório.");
            }

            if (vinculo.DataNomeacao == null || vinculo.DataNomeacao == DateTime.MinValue)
            {
                mensagens.Add("O campo DATA DE ADMISSÃO é de preenchimento obrigatório.");
            }
            else if (vinculo.DataDesativacao != null && vinculo.DataDesativacao > DateTime.MinValue)
            {
                if (vinculo.DataDesativacao < vinculo.DataNomeacao)
                {
                    mensagens.Add("A data de DEMISSÃO deve ser maior ou igual a data de admissão.");
                }
            }

            if (vinculo.Categoria.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo CARGO é de preenchimento obrigatório.");
            }

            if (vinculo.ChCategoria == null || vinculo.ChCategoria <= 0)
            {
                mensagens.Add("O campo CARGA HORÁRIA DO CARGO é de preenchimento obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (cadastro)
                    {
                        vinculo.Ordem = this.GeraOrdemServidorPor(contexto, vinculo.Pessoa);
                        vinculo.Matricula = string.Format("{0}/{1}", idFuncional, vinculo.Vinculo);

                        //Valida matricula
                        if (rnDocentes.PossuiOutraMatriculaPor(contexto, vinculo.Matricula, vinculo.Pessoa))
                        {
                            mensagens.Add("Número de matrícula ou id/Vinculo já cadastrado para outro docente.");
                        }

                        if (this.PossuiOutraMatriculaPor(contexto, vinculo.Matricula, vinculo.Pessoa))
                        {
                            mensagens.Add("Número de matrícula ou id/Vinculo já cadastrado para outro servidor.");
                        }

                        //Valida ID FUNCIONAL existente                      
                        if (rnPessoa.PossuiOutroIdFuncionalPor(contexto, Convert.ToInt32(idFuncional), vinculo.Pessoa))
                        {
                            mensagens.Add("Já existe uma pessoa cadastrada com esse número de ID FUNCIONAL.");
                        }

                        if (vinculo.Pessoa > 0 && this.EhVinculoExistentePor(contexto, vinculo.Pessoa, vinculo.Vinculo))
                        {
                            mensagens.Add("Vínculo já associado a essa pessoa.");
                        }
                        if (vinculo.Pessoa > 0)
                        {
                            //Valida se o vinculo já foi utilizado para esta pessoa
                            if (rnDocentes.PossuiOutroVinculoPor(contexto, vinculo.Pessoa, Convert.ToInt32(vinculo.Vinculo)) ||
                                this.PossuiOutroVinculoPor(contexto, vinculo.Pessoa, Convert.ToInt32(vinculo.Vinculo)))
                            {
                                mensagens.Add("Este VINCULO já se encontra cadastrado para esta pessoa.");
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

        public void InsereServidor(LyPessoa pessoa, LyVinculo vinculo, string zonaResidencial, string povoIndigenaId)
        {
            RN.Pessoa rnPessoa = new Pessoa();
            RN.FlPessoa rnFlPessoa = new FlPessoa();
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingLock();
                if (pessoa.Pessoa == 0)
                {
                    rnPessoa.Insere(contexto, pessoa);

                    //Caso a pessoa não exista ou o flpessoa não exista Insere
                    rnFlPessoa.InsereZonaResidencialPovoIndigena(contexto, pessoa.Pessoa, zonaResidencial, povoIndigenaId);
                }
                else
                {
                    rnPessoa.AtualizaPessoaServidor(contexto, pessoa);

                    if (!rnFlPessoa.ExistePor(contexto, pessoa.Pessoa))
                    {
                        //Caso a pessoa não exista ou o flpessoa não exista Insere
                        rnFlPessoa.InsereZonaResidencialPovoIndigena(contexto, pessoa.Pessoa, zonaResidencial, povoIndigenaId);
                    }
                    else
                    {
                        //Caso contrario, Atualiza Zona Residencial (FL_PESSOA - FL_FIELD_01)
                        rnFlPessoa.AtualizaZonaResidencialPovoIndigena(contexto, pessoa.Pessoa, zonaResidencial, povoIndigenaId);
                    }
                }
                //Gera ordem para vinculo
                vinculo.Ordem = this.GeraOrdemServidorPor(contexto, pessoa.Pessoa);
                vinculo.UsuarioId = pessoa.UsuarioId;
                vinculo.Pessoa = pessoa.Pessoa;
                vinculo.DataDesativacao = null;

                //Insere vinculo
                this.Insere(contexto, vinculo);
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

        public void AtualizaDadosPessoaisServidor(LyPessoa pessoa, string zonaResidencial, string povoIndigenaId)
        {
            RN.Pessoa rnPessoa = new Pessoa();
            RN.FlPessoa rnFlPessoa = new FlPessoa();
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingLock();

                rnPessoa.AtualizaDadosPessoaisServidor(contexto, pessoa);

                if (!rnFlPessoa.ExistePor(contexto, pessoa.Pessoa))
                {
                    //Caso a pessoa não exista ou o flpessoa não exista Insere
                    rnFlPessoa.InsereZonaResidencialPovoIndigena(contexto, pessoa.Pessoa, zonaResidencial, povoIndigenaId);
                }
                else
                {
                    //Caso contrario, Atualiza Zona Residencial (FL_PESSOA - FL_FIELD_01)
                    rnFlPessoa.AtualizaZonaResidencialPovoIndigena(contexto, pessoa.Pessoa, zonaResidencial, povoIndigenaId);
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

        public void AtualizaServidor(LyPessoa pessoa, string zonaResidencial, RecursosHumanos.Entidades.GoogleEducation googleEducation, string povoIndigenaId)
        {
            RN.Pessoa rnPessoa = new Pessoa();
            RN.FlPessoa rnFlPessoa = new FlPessoa();
            RecursosHumanos.GoogleEducation rnGoogleEducation = new Techne.Lyceum.RN.RecursosHumanos.GoogleEducation();
            DataContext contexto = null;

            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingLock();

                //Busca idFuncional, terá alteração diferenciada
                pessoa.IdFuncional = rnPessoa.ObtemIdFuncionalPor(contexto, pessoa.Pessoa);

                rnPessoa.AtualizaPessoaServidor(contexto, pessoa);

                if (!rnFlPessoa.ExistePor(contexto, pessoa.Pessoa))
                {
                    //Caso a pessoa não exista ou o flpessoa não exista Insere
                    rnFlPessoa.InsereZonaResidencialPovoIndigena(contexto, pessoa.Pessoa, zonaResidencial, povoIndigenaId);
                }
                else
                {
                    //Caso contrario, Atualiza Zona Residencial (FL_PESSOA - FL_FIELD_01)
                    rnFlPessoa.AtualizaZonaResidencialPovoIndigena(contexto, pessoa.Pessoa, zonaResidencial, povoIndigenaId);
                }

                //Verifica de tem email google
                if (googleEducation != null && !googleEducation.Email.IsNullOrEmptyOrWhiteSpace())
                {
                    googleEducation.Pessoa = pessoa.Pessoa;
                    googleEducation.UsuarioId = pessoa.UsuarioId;
                    rnGoogleEducation.Salva(contexto, googleEducation);
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

        public void Atualiza(LyVinculo vinculo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE DBO.LY_VINCULO
                                        SET    DATA_NOMEACAO = @DATA_NOMEACAO, 
                                               DATA_DESATIVACAO = @DATA_DESATIVACAO, 
                                               VINCULO = @VINCULO,
                                               CATEGORIA = @CATEGORIA, 
                                               CH_CATEGORIA = @CH_CATEGORIA, 
                                               USUARIOID = @USUARIOID,
                                               DATAALTERACAO = @DATAALTERACAO,
                                               STAMP_ATUALIZACAO = @STAMP_ATUALIZACAO 
                                        WHERE  PESSOA = @PESSOA
                                               AND ORDEM = @ORDEM ";

                contextQuery.Parameters.Add("@PESSOA", vinculo.Pessoa);
                contextQuery.Parameters.Add("@ORDEM", vinculo.Ordem);
                contextQuery.Parameters.Add("@VINCULO", vinculo.Vinculo);
                contextQuery.Parameters.Add("@DATA_NOMEACAO", vinculo.DataNomeacao);
                contextQuery.Parameters.Add("@DATA_DESATIVACAO", vinculo.DataDesativacao);
                contextQuery.Parameters.Add("@CATEGORIA", vinculo.Categoria);
                contextQuery.Parameters.Add("@CH_CATEGORIA", vinculo.ChCategoria);
                contextQuery.Parameters.Add("@USUARIOID", vinculo.UsuarioId);
                contextQuery.Parameters.Add("@DATACADASTRO", DateTime.Now);
                contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);
                contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", DateTime.Now);

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

        public ValidacaoDados ValidaRemocaoServidor(decimal pessoa)
        {
            DataContext contexto = null;
            List<string> mensagens = new List<string>();
            RN.Aluno rnAluno = new Aluno();
            RN.Docentes rnDocentes = new Docentes();
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (pessoa <= 0)
            {
                mensagens.Add("O campo PESSOA não foi encontrado.");
            }
            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (rnAluno.EhAlunoPor(contexto, pessoa))
                    {
                        mensagens.Add("Existe aluno vinculado a esta pessoa.");
                    }

                    if (rnDocentes.EhDocentePor(contexto, pessoa))
                    {
                        mensagens.Add("Existe docente vinculado a esta pessoa.");
                    }

                    if (this.EhServidorPor(contexto, pessoa))
                    {
                        mensagens.Add("Existe dados de ingresso vinculado a esta pessoa.");
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

        public ValidacaoDados ValidaRemocao(decimal pessoa, decimal ordem, string matricula)
        {
            RN.Lotacao rnLotacao = new Lotacao();
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (pessoa <= 0)
            {
                mensagens.Add("O campo PESSOA não foi encontrado.");
            }

            if (ordem <= 0)
            {
                mensagens.Add("O campo ORDEM não foi encontrado.");
            }

            if (matricula.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("O campo MATRÍCULA OU ID/VÍNCULO não foi encontrado.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    //Verifica se tem lotação                    
                    if (rnLotacao.PossuiLotacaoPor(matricula, pessoa, ordem))
                    {
                        mensagens.Add("Existe Lotação vinculada a esta MATRÍCULA OU ID/VÍNCULO.");
                    }
                }
                catch
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw;
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

        public void Remove(decimal pessoa, decimal ordem)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" DELETE DBO.LY_VINCULO
                                          WHERE  PESSOA = @PESSOA
                                               AND ORDEM = @ORDEM  ";

                contextQuery.Parameters.Add("@PESSOA", pessoa);
                contextQuery.Parameters.Add("@ORDEM", ordem);

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

        public void RemoveServidor(decimal pessoa)
        {
            RN.Pessoa rnPessoa = new Pessoa();
            DataContext contexto = null;
            try
            {
                contexto = DataContextBuilder.FromLyceum.UsingLock();
                rnPessoa.Remove(contexto, pessoa);
            }
            catch (Exception ex)
            {
                if (contexto != null)
                    contexto.Abandon();

                throw ex;
            }
            finally
            {
                if (contexto != null)
                    contexto.Dispose();
            }
        }

        public void Insere(LyVinculo vinculo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                this.Insere(ctx, vinculo);
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

        private void Insere(DataContext ctx, LyVinculo vinculo)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO DBO.LY_VINCULO 
                                            (PESSOA, 
                                             ORDEM, 
                                             MATRICULA, 
                                             DATA_NOMEACAO, 
                                             DATA_DESATIVACAO, 
                                             STAMP_ATUALIZACAO, 
                                             CATEGORIA, 
                                             CH_CATEGORIA, 
                                             VINCULO, 
                                             USUARIOID, 
                                             DATACADASTRO, 
                                             DATAALTERACAO) 
                                VALUES      (@PESSOA, 
                                             @ORDEM, 
                                             @MATRICULA, 
                                             @DATA_NOMEACAO, 
                                             @DATA_DESATIVACAO, 
                                             @STAMP_ATUALIZACAO, 
                                             @CATEGORIA, 
                                             @CH_CATEGORIA, 
                                             @VINCULO, 
                                             @USUARIOID, 
                                             @DATACADASTRO, 
                                             @DATAALTERACAO)  ";

            contextQuery.Parameters.Add("@PESSOA", vinculo.Pessoa);
            contextQuery.Parameters.Add("@ORDEM", vinculo.Ordem);
            contextQuery.Parameters.Add("@MATRICULA", vinculo.Matricula);
            contextQuery.Parameters.Add("@DATA_NOMEACAO", vinculo.DataNomeacao);
            contextQuery.Parameters.Add("@DATA_DESATIVACAO", vinculo.DataDesativacao);
            contextQuery.Parameters.Add("@STAMP_ATUALIZACAO", DateTime.Now);
            contextQuery.Parameters.Add("@CATEGORIA", vinculo.Categoria);
            contextQuery.Parameters.Add("@CH_CATEGORIA", vinculo.ChCategoria);
            contextQuery.Parameters.Add("@VINCULO", vinculo.Vinculo);
            contextQuery.Parameters.Add("@USUARIOID", vinculo.UsuarioId);
            contextQuery.Parameters.Add("@DATACADASTRO", DateTime.Now);
            contextQuery.Parameters.Add("@DATAALTERACAO", DateTime.Now);

            ctx.ApplyModifications(contextQuery);
        }

        public decimal GeraOrdemServidorPor(decimal pessoa)
        {
            DataContext contexto = null;
            decimal retorno = 0;

            try
            {
                contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();
                retorno = this.GeraOrdemServidorPor(contexto, pessoa);
            }
            catch (Exception ex)
            {
                string mensagem = string.Empty;
                contexto.Abandon();
                mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                        Environment.NewLine,
                        Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                if (contexto != null)
                {
                    contexto.Dispose();
                }
            }
            return retorno;
        }

        private decimal GeraOrdemServidorPor(DataContext ctx, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            SqlDataReader reader = null;
            decimal retorno = 0;

            contextQuery.Command = @" SELECT ISNULL(MAX(ORDEM),0) + 1  AS ORDEM
                                      FROM LY_VINCULO (NOLOCK)
                                        WHERE PESSOA = @PESSOA  ";

            contextQuery.Parameters.Add("@PESSOA", pessoa);

            reader = ctx.GetDataReader(contextQuery);

            while (reader.Read())
            {
                retorno = Convert.ToInt32(reader["ORDEM"]);
            }

            if (reader != null)
            {
                reader.Close();
            }

            return retorno;
        }

        public static bool VerificaDataDesativacao(decimal pessoa, decimal? ordem)
        {
            int cont = ExecutarFuncao("select COUNT(*) from LY_VINCULO where DATA_DESATIVACAO is null and PESSOA = ? and ORDEM <> ?", pessoa, ordem);
            return cont > 1;
        }

        public static bool ValidaMatricula(decimal? pessoa, string matricula)
        {
            return ExecutarFuncao("select COUNT(*) from LY_VINCULO where PESSOA = ? and Matricula = ?", pessoa, matricula) == 0;
        }

        public static bool ExisteMatricula(string matricula)
        {
            return ExecutarFuncao(@"select 1 from LY_DOCENTE where MATRICULA = ?
                                    union 
                                    select 1 from LY_VINCULO where MATRICULA = ?", matricula, matricula) == 1;
        }

        public bool PossuiOutraMatriculaPor(DataContext ctx, string matricula, decimal pessoa)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM LY_VINCULO 
                                        WHERE MATRICULA = @MATRICULA
	                                        AND PESSOA <> @PESSOA ";

            contextQuery.Parameters.Add("@MATRICULA", matricula);
            contextQuery.Parameters.Add("@PESSOA", pessoa);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                retorno = true;
            }

            return retorno;
        }
        public bool EhServidorVigentePor(decimal pessoa)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                possui = this.EhServidorVigentePor(ctx, pessoa);

                return possui;
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }
        public bool EhServidorVigentePor(DataContext ctx, decimal pessoa)
        {
            bool possui = false;

            ContextQuery contextQuery = new ContextQuery
            {
                Command = @" SELECT COUNT(*) 
                                    FROM   LY_VINCULO 
                                    WHERE  PESSOA = @PESSOA 
                                           AND (DATA_DESATIVACAO IS NULL OR DATA_DESATIVACAO > GETDATE())"
            };

            contextQuery.Parameters.Add("@PESSOA", pessoa);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public bool EhVinculoExistentePor(decimal pessoa, int? vinculo)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            bool possui = false;

            try
            {
                possui = this.EhVinculoExistentePor(ctx, pessoa, vinculo);

                return possui;
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                string mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }
        public bool EhVinculoExistentePor(DataContext ctx, decimal pessoa, int? vinculo)
        {
            bool possui = false;

            ContextQuery contextQuery = new ContextQuery
            {
                Command = @" SELECT COUNT(*) 
                                    FROM   LY_VINCULO 
                                    WHERE  PESSOA = @PESSOA 
                                           AND VINCULO = @VINCULO"
            };

            contextQuery.Parameters.Add("@PESSOA", pessoa);
            contextQuery.Parameters.Add("@VINCULO", vinculo);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public string ObtemCategoriaPor(string matricula)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.ObtemCategoriaPor(contexto, matricula);
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

        public string ObtemCategoriaPor(DataContext contexto, string matricula)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT CATEGORIA 
                            FROM LY_VINCULO (NOLOCK)
                            WHERE MATRICULA = @MATRICULA ";

            contextQuery.Parameters.Add("@MATRICULA", TechneDbType.T_CODIGO, matricula);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public string ObtemCategoriaLotacaoPor(string matricula)
        {
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                return this.ObtemCategoriaLotacaoPor(contexto, matricula);
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

        public string ObtemCategoriaLotacaoPor(DataContext contexto, string matricula)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT v.CATEGORIA 
                            FROM LY_VINCULO v (NOLOCK)
                            INNER JOIN LY_CATEGORIA_DOCENTE CD ON V.CATEGORIA = CD.CATEGORIA
                            WHERE MATRICULA = @MATRICULA ";

            contextQuery.Parameters.Add("@MATRICULA", TechneDbType.T_CODIGO, matricula);

            resultado = contexto.GetReturnValue<string>(contextQuery);


            if (resultado.IsNullOrEmptyOrWhiteSpace())
            {
                resultado = "EM DEFINCAO";
            }

            return resultado;
        }



        public LyVinculo ObtemPrimeiroVinculoPor(string matricula)
        {
            LyVinculo vinculo = new LyVinculo();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            try
            {
                vinculo = this.ObtemPrimeiroVinculoPor(ctx, matricula);

                return vinculo;
            }
            catch (Exception ex)
            {
                ctx.Abandon();
                var mensagem = string.Format("Falha de Acesso ao Banco de Dados. Entre em contato com o Administrador do Sistema repassando esta mensagem ou tente novamente mais tarde.{0}Erro gerado: {1}",
                Environment.NewLine, Convert.ToString(ex.Message));
                throw new Exception(mensagem);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public LyVinculo ObtemPrimeiroVinculoPor(DataContext ctx, string matricula)
        {
            LyVinculo vinculo = new LyVinculo();
            SqlDataReader reader = null;

            try
            {
                ContextQuery contextQuery = new ContextQuery
                {
                    Command = (@" SELECT TOP 1 *
                        FROM    dbo.LY_VINCULO
                        WHERE MATRICULA = @MATRICULA 
                        ORDER BY DATA_NOMEACAO  ")
                };

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {

                    vinculo.Pessoa = Convert.ToDecimal(reader["PESSOA"]);
                    vinculo.Matricula = Convert.ToString(reader["MATRICULA"]);
                    vinculo.Ordem = Convert.ToDecimal(reader["ORDEM"]);
                    vinculo.Categoria = Convert.ToString(reader["CATEGORIA"]);
                    vinculo.ChCategoria = reader["CH_CATEGORIA"] != DBNull.Value ? Convert.ToDecimal(reader["CH_CATEGORIA"]) : (decimal?)null;
                    vinculo.Vinculo = reader["VINCULO"] != DBNull.Value ? Convert.ToInt32(reader["VINCULO"]) : (int?)null;
                    vinculo.UsuarioId = Convert.ToString(reader["USUARIOID"]);

                    if (reader["DATA_NOMEACAO"] != DBNull.Value)
                    {
                        vinculo.DataNomeacao = Convert.ToDateTime(reader["DATA_NOMEACAO"]);
                    }

                    if (reader["DATA_DESATIVACAO"] != DBNull.Value)
                    {
                        vinculo.DataDesativacao = Convert.ToDateTime(reader["DATA_DESATIVACAO"]);
                    }

                }

                return vinculo;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public bool PossuiOutroVinculoPor(DataContext ctx, decimal pessoa, int vinculo, string matricula)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool possui = false;

            contextQuery.Command = @" SELECT COUNT(*) 
                                        FROM LY_VINCULO (NOLOCK)
                                        WHERE PESSOA = @PESSOA
	                                        AND VINCULO = @VINCULO 
                                            AND MATRICULA <> @MATRICULA ";

            contextQuery.Parameters.Add("@PESSOA", pessoa);
            contextQuery.Parameters.Add("@VINCULO", vinculo);
            contextQuery.Parameters.Add("@MATRICULA", matricula);

            if (ctx.GetReturnValue<int>(contextQuery) > 0)
            {
                possui = true;
            }

            return possui;
        }

        public DadosTrocaMatriculaVinculo ObtemDadosTrocaMatriculaVinculoPor(string matricula)
        {
            DadosTrocaMatriculaVinculo dados = new DadosTrocaMatriculaVinculo();
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT DISTINCT d.PESSOA ,
									p.IDFUNCIONAL,
									d.VINCULO,
                                    d.MATRICULA ,
                                    p.NOME_COMPL ,
                                    p.DT_NASC ,
                                    p.CPF ,
                                    p.SEXO
                            FROM    LY_VINCULO d ( NOLOCK )
                                    INNER JOIN dbo.LY_PESSOA p ( NOLOCK ) ON d.PESSOA = p.PESSOA
                            WHERE   d.MATRICULA = @MATRICULA ";

                contextQuery.Parameters.Add("@MATRICULA", matricula);

                reader = ctx.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    dados.Pessoa = Convert.ToInt32(reader["PESSOA"]);

                    if (reader["IDFUNCIONAL"] != DBNull.Value)
                    {
                        dados.IdFuncional = Convert.ToInt32(reader["IDFUNCIONAL"]);
                    }

                    if (reader["VINCULO"] != DBNull.Value)
                    {
                        dados.Vinculo = Convert.ToInt32(reader["VINCULO"]);
                    }

                    dados.Matricula = Convert.ToString(reader["MATRICULA"]);
                    dados.NomeCompl = Convert.ToString(reader["NOME_COMPL"]);

                    if (reader["DT_NASC"] != DBNull.Value)
                    {
                        dados.DtNasc = Convert.ToDateTime(reader["DT_NASC"]);
                    }

                    dados.Cpf = Convert.ToString(reader["CPF"]);
                    dados.Sexo = Convert.ToString(reader["SEXO"]);

                }

                return dados;
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
                if (reader != null)
                {
                    reader.Close();
                }
                ctx.Dispose();
            }
        }

        public ValidacaoDados ValidaTrocaMatricula(Entidades.LogAtualizacaoMatriculaServidor logAtualizacaoMatriculaServidor, int pessoa)
        {
            RN.Pessoa rnPessoa = new Pessoa();
            RN.VinculoLy rnVinculo = new VinculoLy();
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            int aulasAlocadas = 0;
            int glpsAlocadas = 0;
            int matricula = 0;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (logAtualizacaoMatriculaServidor == null)
            {
                return validacaoDados;
            }

            if (string.IsNullOrEmpty(logAtualizacaoMatriculaServidor.MatriculaAnterior) && logAtualizacaoMatriculaServidor.IdFuncionalAnterior == null)
            {
                mensagens.Add("Campo ID FUNCIONAL OU MATRICULA ATUAL é obrigatório.");
            }

            else
            {
                if (string.IsNullOrEmpty(logAtualizacaoMatriculaServidor.MatriculaAnterior))
                {
                    logAtualizacaoMatriculaServidor.MatriculaAnterior = string.Format("{0}/{1}", logAtualizacaoMatriculaServidor.IdFuncionalAnterior, logAtualizacaoMatriculaServidor.VinculoAnterior);
                }

                if (logAtualizacaoMatriculaServidor.IdFuncionalNovo == logAtualizacaoMatriculaServidor.IdFuncionalAnterior
                    && logAtualizacaoMatriculaServidor.VinculoNovo == logAtualizacaoMatriculaServidor.VinculoAnterior
                    && logAtualizacaoMatriculaServidor.MatriculaNova == logAtualizacaoMatriculaServidor.MatriculaAnterior)
                {
                    mensagens.Add("Campo ID FUNCIONAL OU VINCULO OU MATRÍCULA NOVA deve ser diferente do ATUAL.");
                }

                if (logAtualizacaoMatriculaServidor.MatriculaNova == "0")
                {
                    mensagens.Add("Campo MATRICULA NOVA deve ser diferente de 0.");
                }
            }

            if (string.IsNullOrEmpty(logAtualizacaoMatriculaServidor.MatriculaNova) && logAtualizacaoMatriculaServidor.IdFuncionalNovo == null)
            {
                mensagens.Add("Campo ID FUNCIONAL OU MATRICULA NOVA é obrigatório.");
            }
            else if (string.IsNullOrEmpty(logAtualizacaoMatriculaServidor.MatriculaNova))
            {
                logAtualizacaoMatriculaServidor.MatriculaNova = string.Format("{0}/{1}", logAtualizacaoMatriculaServidor.IdFuncionalNovo, logAtualizacaoMatriculaServidor.VinculoNovo);
            }

            if (logAtualizacaoMatriculaServidor.IdFuncionalNovo != null && logAtualizacaoMatriculaServidor.VinculoNovo == null)
            {
                mensagens.Add("Campo VINCULO NOVO é obrigatório quando o ID FUNCIONAL NOVO for informado.");
            }

            if (logAtualizacaoMatriculaServidor.Pessoa <= 0)
            {
                mensagens.Add("O NUM_FUNC é obrigatório.");
            }

            if (pessoa <= 0)
            {
                mensagens.Add("O PESSOA é obrigatório.");
            }

            if (string.IsNullOrEmpty(logAtualizacaoMatriculaServidor.UsuarioId))
            {
                mensagens.Add("O USUÁRIO RESPONSAVEL é obrigatório.");
            }

            if (mensagens.Count == 0)
            {
                try
                {
                    contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

                    if (logAtualizacaoMatriculaServidor.VinculoNovo <= 0)
                    {
                        mensagens.Add("O VINCULO NOVO não pode ser 0");
                    }

                    if (string.IsNullOrEmpty(logAtualizacaoMatriculaServidor.MatriculaNova) || logAtualizacaoMatriculaServidor.IdFuncionalNovo == null)
                    {
                        mensagens.Add("Campo ID/VINCULO é obrigatório.");
                    }

                    if (logAtualizacaoMatriculaServidor.IdFuncionalNovo != null && logAtualizacaoMatriculaServidor.IdFuncionalAnterior != logAtualizacaoMatriculaServidor.IdFuncionalNovo)
                    {
                        //Verifica se existe o Id para otura pessoa
                        if (rnPessoa.PossuiOutroIdFuncionalPor(contexto, Convert.ToInt32(logAtualizacaoMatriculaServidor.IdFuncionalNovo), pessoa))
                        {
                            mensagens.Add("Já existe uma pessoa cadastrada com esse número de ID FUNCIONAL.");
                        }
                    }

                    if (logAtualizacaoMatriculaServidor.VinculoNovo != null && logAtualizacaoMatriculaServidor.VinculoAnterior != logAtualizacaoMatriculaServidor.VinculoNovo)
                    {
                        //Verifica se já existe o vinculo para este docente | na primeira linha era logAtualizacaoMatriculaServidor.DocenteID 
                        if (this.PossuiOutroVinculoPor(contexto, pessoa, Convert.ToInt32(logAtualizacaoMatriculaServidor.VinculoNovo), logAtualizacaoMatriculaServidor.MatriculaAnterior) ||
                            rnVinculo.PossuiOutroVinculoPor(contexto, pessoa, Convert.ToInt32(logAtualizacaoMatriculaServidor.VinculoNovo), logAtualizacaoMatriculaServidor.MatriculaAnterior))
                        {
                            mensagens.Add("Este VINCULO já se encontra cadastrado para esta pessoa.");
                        }
                    }

                    if (logAtualizacaoMatriculaServidor.MatriculaAnterior != logAtualizacaoMatriculaServidor.MatriculaNova)
                    {
                        //Verifica se matricula é utilizada por outro docente
                        matricula = this.ObtemOrdemVinculoAtivoPor(contexto, logAtualizacaoMatriculaServidor.MatriculaNova);
                        if (matricula > 0)
                        {
                            mensagens.Add(string.Format("Id/Vinculo ou Matrícula {0} está associada a outro servidor. Atualização não permitida.", logAtualizacaoMatriculaServidor.MatriculaNova));
                        }
                    }
                }
                catch
                {
                    if (contexto != null)
                    {
                        contexto.Abandon();
                    }
                    throw;
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

        private void AlteraMatriculaServidor(DataContext ctx, Entidades.LogAtualizacaoMatriculaServidor logAtualizacaoMatriculaServidor)
        {
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" UPDATE  LY_VINCULO
                                    SET     MATRICULA = @MATRICULANOVA,
                                            VINCULO = @VINCULONOVO
                                    WHERE   PESSOA = @PESSOA
                                            AND MATRICULA = @MATRICULAANTERIOR ";

                contextQuery.Parameters.Add("@PESSOA", logAtualizacaoMatriculaServidor.Pessoa);
                contextQuery.Parameters.Add("@MATRICULAANTERIOR", logAtualizacaoMatriculaServidor.MatriculaAnterior);
                contextQuery.Parameters.Add("@MATRICULANOVA", logAtualizacaoMatriculaServidor.MatriculaNova);
                contextQuery.Parameters.Add("@VINCULONOVO", logAtualizacaoMatriculaServidor.VinculoNovo);

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
        }

        public void TrocaMatricula(Entidades.LogAtualizacaoMatriculaServidor logAtualizacaoMatriculaServidor, int pessoa)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingLock();
            RN.LogAtualizacaoMatriculaServidor rnLogAtualizacaoMatriculaServidor = new LogAtualizacaoMatriculaServidor();
            RN.Lotacao rnLotacao = new Lotacao();
            RN.Pessoa rnPessoa = new Pessoa();

            try
            {
                if (logAtualizacaoMatriculaServidor.MatriculaAnterior != logAtualizacaoMatriculaServidor.MatriculaNova
                    || logAtualizacaoMatriculaServidor.VinculoAnterior != logAtualizacaoMatriculaServidor.VinculoNovo)
                {
                    //Altera tabela docente
                    this.AlteraMatriculaServidor(ctx, logAtualizacaoMatriculaServidor);
                }

                if (logAtualizacaoMatriculaServidor.MatriculaAnterior != logAtualizacaoMatriculaServidor.MatriculaNova)
                {
                    //Altera tabela lotaçao
                    rnLotacao.AlteraMatriculaServidorLotacao(ctx, logAtualizacaoMatriculaServidor);
                }

                if (logAtualizacaoMatriculaServidor.IdFuncionalAnterior != logAtualizacaoMatriculaServidor.IdFuncionalNovo)
                {
                    //Altera Id
                    rnPessoa.AlteraIdFuncional(pessoa, logAtualizacaoMatriculaServidor.UsuarioId, logAtualizacaoMatriculaServidor.IdFuncionalNovo);
                }

                //insere log
                rnLogAtualizacaoMatriculaServidor.Insere(ctx, logAtualizacaoMatriculaServidor);
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

    }
}
