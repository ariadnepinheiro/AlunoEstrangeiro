using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Seeduc.Infra.Data;
using Techne.Data;
using Techne.Lyceum.CR;
using Techne.Lyceum.RN.Entidades;
using Techne.Lyceum.RN.Util;
using System.Data.SqlClient;

namespace Techne.Lyceum.RN
{
    public class UnidadeFisica : RNBase
    {
        /// <summary>
        /// Verifica se o código da unidade fisica passada como parâmetro existe na base
        /// </summary>
        /// <param name="unidade_fis">código da unidade de fisica</param>
        /// <returns>true se existir, false se não existir</returns>
        public static bool VerificarUnidadeFisica(string unidade_fis)
        {
            DataContext ctx = DataContextBuilder.FromLyceum.UsingNoLock();
            ContextQuery contextQuery = new ContextQuery();
            bool retorno = false;

            try
            {
                contextQuery.Command = @" SELECT UNIDADE_FIS, NOME_COMP 
                                        FROM VW_ZZCRO_UNIDADE_FISICA
                                            WHERE UNIDADE_FIS = @UNIDADE_FIS ";

                contextQuery.Parameters.Add("@UNIDADE_FIS", unidade_fis);

                if (ctx.GetReturnValue<int>(contextQuery) > 0)
                {
                    retorno = true;
                }

                return retorno;
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

        /// <summary>
        /// Consulta unidades físicas associadas a unicada de ensino.
        /// </summary>
        /// <param name="unidade_ensino">unidade de ensino</param>
        /// <returns>lista de unidades físicas</returns>
        public static QueryTable ConsultarPorUnidadeEnsino(string unidade_ensino)
        {
            var sql = " select f.unidade_fis, f.nome_comp from ly_unidades_associadas a join ly_unidade_fisica f on a.unidade_fis = f.unidade_fis" +
                         " WHERE a.unidade_ens = ?";
            return Consultar(sql, unidade_ensino);
        }

        /// <summary>
        /// Consulta última chave do contato da unidade física.
        /// </summary>
        /// <param name="unidade">unidade física</param>
        /// <returns>última chave + 1 </returns>
        internal static decimal? ConsultarUltimaChaveContato(string unidade)
        {
            return ExecutarFuncao("select isnull(max(convert(int,chave)),0) + 1 from LY_CONTATO_UNID_FIS where UNIDADE_FIS = ?", unidade);
        }

        public bool ExisteUnidadeFisicaCadastradaPor(DataContext contexto, string censo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(*) 
                                      FROM   LY_UNIDADE_ENSINO (NOLOCK) 
                                      WHERE  UNIDADE_ENS = @CENSO ";

            contextQuery.Parameters.Add("@CENSO", TechneDbType.T_CODIGO, censo);

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }

        public void InsereInformacoesGerais(DataContext contexto, DTOs.UnidadeInformacoesGerais unidadeInformacoesGerais)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" INSERT INTO DBO.LY_UNIDADE_FISICA
                                        (UNIDADE_FIS, 
                                         NOME_COMP, 
                                         NOME_ABREV, 
                                         IMOVEL_COMPARTILHADO, 
                                         CEP, 
                                         MUNICIPIO, 
                                         ENDERECO, 
                                         END_NUM, 
                                         END_COMPL, 
                                         BAIRRO, 
                                         DISTRITO,
                                         LATITUDE,
                                         LONGITUDE,
                                         MATRICULA, 
                                         DT_CADASTRO) 
                            VALUES      (@UNIDADE_FIS, 
                                         @NOME_COMP, 
                                         @NOME_ABREV, 
                                         @IMOVEL_COMPARTILHADO, 
                                         @CEP, 
                                         @MUNICIPIO, 
                                         @ENDERECO, 
                                         @END_NUM, 
                                         @END_COMPL, 
                                         @BAIRRO, 
                                         @DISTRITO,
                                         @LATITUDE,
                                         @LONGITUDE,
                                         @MATRICULA, 
                                         @DT_CADASTRO) ";

            contextQuery.Parameters.Add("@UNIDADE_FIS", TechneDbType.T_CODIGO, unidadeInformacoesGerais.Censo);
            contextQuery.Parameters.Add("@NOME_COMP", TechneDbType.T_ALFALARGE, unidadeInformacoesGerais.NomeUnidade);
            contextQuery.Parameters.Add("@NOME_ABREV", TechneDbType.T_ALFAMEDIUM, unidadeInformacoesGerais.NomeUnidade);
            contextQuery.Parameters.Add("@IMOVEL_COMPARTILHADO", TechneDbType.T_SIMNAO, unidadeInformacoesGerais.ImovelCompartilhado);
            contextQuery.Parameters.Add("@CEP", TechneDbType.T_CEP, unidadeInformacoesGerais.Cep);
            contextQuery.Parameters.Add("@MUNICIPIO", TechneDbType.T_CODIGO, unidadeInformacoesGerais.Municipio);
            contextQuery.Parameters.Add("@ENDERECO", TechneDbType.T_ALFAMEDIUM, unidadeInformacoesGerais.Endereco);
            contextQuery.Parameters.Add("@END_NUM", TechneDbType.T_ALFASMALL, unidadeInformacoesGerais.EnderecoNumero);
            contextQuery.Parameters.Add("@END_COMPL", TechneDbType.T_ALFAMEDIUM, unidadeInformacoesGerais.EnderecoComplemento);
            contextQuery.Parameters.Add("@BAIRRO", TechneDbType.T_ALFAMEDIUM, unidadeInformacoesGerais.EnderecoBairro);
            contextQuery.Parameters.Add("@DISTRITO", SqlDbType.VarChar, unidadeInformacoesGerais.Distrito);
            contextQuery.Parameters.Add("@LATITUDE", TechneDbType.T_ALFASMALL, unidadeInformacoesGerais.Latitude);
            contextQuery.Parameters.Add("@LONGITUDE", TechneDbType.T_ALFASMALL, unidadeInformacoesGerais.Longitude);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, unidadeInformacoesGerais.UsuarioResponsavel);
            contextQuery.Parameters.Add("@DT_CADASTRO", SqlDbType.DateTime, DateTime.Now);

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaInformacoesGerais(DataContext contexto, DTOs.UnidadeInformacoesGerais unidadeInformacoesGerais)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE DBO.LY_UNIDADE_FISICA 
                                SET    IMOVEL_COMPARTILHADO = @IMOVEL_COMPARTILHADO, 
                                       LATITUDE = @LATITUDE, 
                                       LONGITUDE = @LONGITUDE,
                                       MATRICULA = @MATRICULA 
                                WHERE  UNIDADE_FIS = @UNIDADE_FIS ";

            contextQuery.Parameters.Add("@UNIDADE_FIS", TechneDbType.T_CODIGO, unidadeInformacoesGerais.Censo);
            contextQuery.Parameters.Add("@IMOVEL_COMPARTILHADO", TechneDbType.T_SIMNAO, unidadeInformacoesGerais.ImovelCompartilhado);
            contextQuery.Parameters.Add("@LATITUDE", TechneDbType.T_ALFASMALL, unidadeInformacoesGerais.Latitude);
            contextQuery.Parameters.Add("@LONGITUDE", TechneDbType.T_ALFASMALL, unidadeInformacoesGerais.Longitude);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, unidadeInformacoesGerais.UsuarioResponsavel);

            contexto.ApplyModifications(contextQuery);
        }

        public DTOs.UnidadeCaracteristicasFisicas ObtemCaracteristicasFisicasPor(string censo)
        {
            DTOs.UnidadeCaracteristicasFisicas entidade = new DTOs.UnidadeCaracteristicasFisicas();
            RN.GestaoRede.UnidadeFisicaFormasAcessibilidade rnUnidadeFisicaFormasAcessibilidade = new Techne.Lyceum.RN.GestaoRede.UnidadeFisicaFormasAcessibilidade();
            DataContext contexto = DataContextBuilder.FromLyceum.ToFastReadingOnly();

            try
            {
                //Busca caracteristicas fisicas
                entidade = this.ObtemCaracteristicasFisicasPor(contexto, censo);

                //Busca formas de acesso
                entidade.FormasAcessibilidade = rnUnidadeFisicaFormasAcessibilidade.ListaFormasAcessibilidadePor(contexto, censo);

                return entidade;
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

        public DTOs.UnidadeCaracteristicasFisicas ObtemCaracteristicasFisicasPor(DataContext contexto, string censo)
        {
            DTOs.UnidadeCaracteristicasFisicas entidade = new DTOs.UnidadeCaracteristicasFisicas();
            SqlDataReader reader = null;
            ContextQuery contextQuery = new ContextQuery();

            try
            {
                contextQuery.Command = @" SELECT UF.UNIDADE_FIS, 
                                       UF.NOME_COMP, 
                                       UF.NOME_ABREV, 
                                       FORMA_OCUPACAO, 
                                       LOCAL_FUNCIONAMENTO, 
                                       AREA_ASSENTAMENTO, 
                                       TERRA_INDIGENA, 
                                       AREA_QUILOMBOS,
                                       UNIDADE_SUSTENTAVEL, 
                                       UF.TIPO, 
                                       TITULAR_PREDIO, 
                                       IMOVEL_COMPARTILHADO_REDE, 
                                       UF.CEP, 
                                       UF.MUNICIPIO, 
                                       M.NOME AS NOMEMUNICIPIO,
		                               M.UF_SIGLA AS UF,
                                       UF.ENDERECO, 
                                       UF.END_NUM, 
                                       UF.END_COMPL, 
                                       UF.BAIRRO,
                                       BA.DESCRICAO AS DESCRICAOBAIRRO,
                                       UF.DISTRITO, 
                                       ACESSO_NECESSIDADE_ESPECIAL,
                                       ACESSO_DIFICIL, 
                                       AREA_TOTAL_TERRENO, 
                                       AREA_TOTAL_CONSTRUIDA, 
                                       AREA_TERRENO, 
                                       DEPENDENCIA_ADM, 
                                       UE.E_MAIL, 
                                       UE.CGC, 
                                       EXTRACLASSE, 
                                       UE.FONE, 
                                       TEL2, 
                                       UE.FAX,
                                       SALACLIMATIZADA,
                                       SALAACESSIBILIDADE,
                                       SALACANTINHOLEITURA,
                                       RE.REGIONAL, 
									   LATITUDE,
									   LONGITUDE
                                FROM   LY_UNIDADE_ENSINO UE (NOLOCK) 
                                        INNER JOIN MUNICIPIO M 
				                               ON UE.MUNICIPIO = M.CODIGO
                                       INNER JOIN LY_UNIDADES_ASSOCIADAS UA (NOLOCK) 
                                               ON UE.UNIDADE_ENS = UA.UNIDADE_ENS 
                                       INNER JOIN LY_UNIDADE_FISICA UF (NOLOCK) 
                                               ON UF.UNIDADE_FIS = UA.UNIDADE_FIS 
                                       LEFT JOIN HADES.dbo.BAIRRO BA (NOLOCK)
													ON BA.CODIGO = UF.BAIRRO
                                       LEFT JOIN TCE_REGIONAL RE 
                                                    ON RE.ID_REGIONAL = UE.ID_REGIONAL
                                WHERE  UE.UNIDADE_ENS = @UNIDADE_ENS ";

                contextQuery.Parameters.Add("@UNIDADE_ENS", TechneDbType.T_CODIGO, censo);

                reader = contexto.GetDataReader(contextQuery);

                while (reader.Read())
                {
                    entidade.UnidadeFisica = Convert.ToString(reader["UNIDADE_FIS"]);
                    entidade.NomeUnidadeFisica = Convert.ToString(reader["NOME_COMP"]);
                    entidade.FormaOcupacaoLocalizacao = Convert.ToString(reader["FORMA_OCUPACAO"]);
                    entidade.LocalFuncionamento = Convert.ToString(reader["LOCAL_FUNCIONAMENTO"]);
                    entidade.AreaAssentamento = Convert.ToString(reader["AREA_ASSENTAMENTO"]);
                    entidade.TerraIndigena = Convert.ToString(reader["TERRA_INDIGENA"]);
                    entidade.AreaQuilombo = Convert.ToString(reader["AREA_QUILOMBOS"]);
                    entidade.UnidadeSustentavel = Convert.ToString(reader["UNIDADE_SUSTENTAVEL"]);
                    entidade.FormaOcupacaoTipo = Convert.ToString(reader["TIPO"]);
                    entidade.OcupacaoFormal = Convert.ToString(reader["TITULAR_PREDIO"]);
                    entidade.ImovelCompartilhadoRede = Convert.ToString(reader["IMOVEL_COMPARTILHADO_REDE"]);
                    entidade.Cep = Convert.ToString(reader["CEP"]);
                    entidade.Municipio = Convert.ToString(reader["MUNICIPIO"]);
                    entidade.MunicipioDescricao = Convert.ToString(reader["NOMEMUNICIPIO"]);
                    entidade.UF = Convert.ToString(reader["UF"]);
                    entidade.Latitude = Convert.ToString(reader["LATITUDE"]);
                    entidade.Longitude = Convert.ToString(reader["LONGITUDE"]);
                    entidade.Endereco = Convert.ToString(reader["ENDERECO"]);
                    entidade.EnderecoNumero = Convert.ToString(reader["END_NUM"]);
                    entidade.EnderecoComplemento = Convert.ToString(reader["END_COMPL"]);
                    entidade.EnderecoBairro = Convert.ToString(reader["BAIRRO"]);
                    entidade.EnderecoDescricaoBairro = Convert.ToString(reader["DESCRICAOBAIRRO"]);
                    entidade.AcessoNecessidadeEspecial = Convert.ToString(reader["ACESSO_NECESSIDADE_ESPECIAL"]);
                    entidade.Distrito = Convert.ToString(reader["DISTRITO"]);
                    entidade.AcessoDificil = Convert.ToString(reader["ACESSO_DIFICIL"]);
                    entidade.DependenciaAdministrativa = Convert.ToString(reader["DEPENDENCIA_ADM"]);
                    entidade.Email = Convert.ToString(reader["E_MAIL"]);
                    entidade.Cnpj = Convert.ToString(reader["CGC"]);
                    entidade.Extraclasse = Convert.ToString(reader["EXTRACLASSE"]);
                    entidade.Telefone1 = Convert.ToString(reader["FONE"]);
                    entidade.Telefone2 = Convert.ToString(reader["TEL2"]);
                    entidade.Fax = Convert.ToString(reader["FAX"]);

                    if (reader["AREA_TOTAL_TERRENO"] != DBNull.Value)
                    {
                        entidade.AreaTotalTerreno = Convert.ToDecimal(reader["AREA_TOTAL_TERRENO"]);
                    }

                    if (reader["AREA_TOTAL_CONSTRUIDA"] != DBNull.Value)
                    {
                        entidade.AreaTotalConstruida = Convert.ToDecimal(reader["AREA_TOTAL_CONSTRUIDA"]);
                    }

                    if (reader["AREA_TERRENO"] != DBNull.Value)
                    {
                        entidade.AreaTerreno = Convert.ToDecimal(reader["AREA_TERRENO"]);
                    }

                    if (reader["SALACLIMATIZADA"] != DBNull.Value)
                    {
                        entidade.SalaClimatizada = Convert.ToInt32(reader["SALACLIMATIZADA"]);
                    }

                    if (reader["SALAACESSIBILIDADE"] != DBNull.Value)
                    {
                        entidade.SalaAcessibilidade = Convert.ToInt32(reader["SALAACESSIBILIDADE"]);
                    }

                    if (reader["SALACANTINHOLEITURA"] != DBNull.Value)
                    {
                        entidade.SalaCantinhoLeitura = Convert.ToInt32(reader["SALACANTINHOLEITURA"]);
                    }

                    if (reader["REGIONAL"] != DBNull.Value)
                    {
                        entidade.Regional = Convert.ToString(reader["REGIONAL"]);
                    }
                }

                return entidade;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }

        public ValidacaoDados ValidaCaracteristicasFisicas(DTOs.UnidadeCaracteristicasFisicas unidadeCaracteristicasFisicas)
        {
            List<string> mensagens = new List<string>();
            DataContext contexto = null;
            string cep = string.Empty;
            ValidacaoDados validacaoDados = new ValidacaoDados
            {
                Valido = false
            };

            if (unidadeCaracteristicasFisicas == null)
            {
                return validacaoDados;
            }

            if (unidadeCaracteristicasFisicas.UnidadeFisica.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CÓDIGO é obrigatório.");
            }
            else if (unidadeCaracteristicasFisicas.UnidadeFisica.Length != 8)
            {
                mensagens.Add("Campo CÓDIGO deve ser composto de 8 digitos.");
            }

            if (unidadeCaracteristicasFisicas.NomeUnidadeFisica.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NOME COMPLETO é obrigatório.");
            }

            if (!unidadeCaracteristicasFisicas.Cnpj.IsNullOrEmptyOrWhiteSpace() && !Validacao.ValidaCnpj(unidadeCaracteristicasFisicas.Cnpj))
            {
                mensagens.Add("CNPJ inválido.");
            }

            if (unidadeCaracteristicasFisicas.FormaOcupacaoLocalizacao.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo LOCALIZAÇÃO DA U.E. é obrigatório.");
            }

            if (unidadeCaracteristicasFisicas.LocalFuncionamento.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo LOCAL DE FUNCIONAMENTO é obrigatório.");
            }
            else if (unidadeCaracteristicasFisicas.LocalFuncionamento == "PredioEscolar" && unidadeCaracteristicasFisicas.FormaOcupacaoTipo.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo FORMA DE OCUPAÇÃO é obrigatório no PRÉDIO ESCOLAR.");
            }

            if (unidadeCaracteristicasFisicas.ImovelCompartilhadoRede.IsNullOrEmptyOrWhiteSpace()
                || (unidadeCaracteristicasFisicas.ImovelCompartilhadoRede != "N" && unidadeCaracteristicasFisicas.ImovelCompartilhadoRede != "S"))
            {
                mensagens.Add("Campo IMÓVEL COMPARTILHADO é obrigatório com os Valores N ou S.");
            }

            if (unidadeCaracteristicasFisicas.Cep.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo CEP é obrigatório.");
            }
            else
            {
                cep = Utils.RetirarMascara(unidadeCaracteristicasFisicas.Cep);

                if (!Validacao.ValidarCEP(cep))
                {
                    mensagens.Add("CEP inválido! Este CEP não foi encontrado em nossa base.");
                }
            }

            if (unidadeCaracteristicasFisicas.Municipio.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo MUNICÍPIO é obrigatório.");
            }

            if (unidadeCaracteristicasFisicas.Endereco.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo ENDEREÇO é obrigatório.");
            }

            if (unidadeCaracteristicasFisicas.EnderecoNumero.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo NÚMERO é obrigatório.");
            }

            if (unidadeCaracteristicasFisicas.EnderecoBairro.IsNullOrEmptyOrWhiteSpace())
            {
                mensagens.Add("Campo BAIRRO é obrigatório.");
            }

            if (unidadeCaracteristicasFisicas.AcessoNecessidadeEspecial.IsNullOrEmptyOrWhiteSpace()
               || (unidadeCaracteristicasFisicas.AcessoNecessidadeEspecial != "N" && unidadeCaracteristicasFisicas.AcessoNecessidadeEspecial != "S"))
            {
                mensagens.Add("Campo ACESSIBILIDADE AO PORTADOR DE NECESSIDADE ESPECIAL é obrigatório com os Valores N ou S.");
            }


            if (unidadeCaracteristicasFisicas.AcessoNecessidadeEspecial == "S")
            {
                if (unidadeCaracteristicasFisicas.FormasAcessibilidade == null || unidadeCaracteristicasFisicas.FormasAcessibilidade.Count == 0)
                {
                    mensagens.Add("Selecione ao menos umas das forma de acesso.");
                }
                else
                { 
                    //9	Nenhum dos recursos de acessibilidade listados
                    if (unidadeCaracteristicasFisicas.FormasAcessibilidade.Contains(9) && unidadeCaracteristicasFisicas.FormasAcessibilidade.Count != 1)
                    {
                        mensagens.Add("Com a opção NENHUM DOS RECURSOS DE ACESSIBILIDADE LISTADOS marcada, nenhuma outra opção pode ser escolhida.");
                    }
                }
            }
            else
            {
                if (unidadeCaracteristicasFisicas.FormasAcessibilidade != null && unidadeCaracteristicasFisicas.FormasAcessibilidade.Count > 0)
                {
                    mensagens.Add("Para selecionar uma das forma de acesso, é necessário marcar o campo ACESSIBILIDADE AO PORTADOR DE NECESSIDADE ESPECIAL.");
                }
                else
                {
                    unidadeCaracteristicasFisicas.FormasAcessibilidade = new List<int>();
                }
            }

            if (unidadeCaracteristicasFisicas.AcessoDificil.IsNullOrEmptyOrWhiteSpace()
                || (unidadeCaracteristicasFisicas.AcessoDificil != "N" && unidadeCaracteristicasFisicas.AcessoDificil != "S"))
            {
                mensagens.Add("Campo DIFÍCIL ACESSO é obrigatório com os Valores N ou S.");
            }

            if (unidadeCaracteristicasFisicas.Extraclasse.IsNullOrEmptyOrWhiteSpace()
                || (unidadeCaracteristicasFisicas.Extraclasse != "N" && unidadeCaracteristicasFisicas.Extraclasse != "S"))
            {
                mensagens.Add("O campo CONTROLA EXTRA-CLASSE é obrigatório com os Valores N ou S.");
            }

            if (!unidadeCaracteristicasFisicas.Email.IsNullOrEmptyOrWhiteSpace() && !Validacao.ValidaEmail(unidadeCaracteristicasFisicas.Email))
            {
                mensagens.Add("EMAIL inválido.");
            }

            if (!unidadeCaracteristicasFisicas.Telefone1.IsNullOrEmptyOrWhiteSpace())
            {
                string telefone = Utils.RetirarMascara(unidadeCaracteristicasFisicas.Telefone1);
                if (!Validacao.ValidaTelefoneComDDD(telefone))
                {
                    mensagens.Add("TELEFONE 1 inválido.");
                }
            }

            if (!unidadeCaracteristicasFisicas.Telefone2.IsNullOrEmptyOrWhiteSpace())
            {
                string telefone = Utils.RetirarMascara(unidadeCaracteristicasFisicas.Telefone2);
                if (!Validacao.ValidaTelefoneComDDD(telefone))
                {
                    mensagens.Add("TELEFONE 2 inválido.");
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

        private string ObtemOutraUnidadeFisicaCadastradaPor(DataContext contexto, string cep, string endereco, string numero, string unidadeFisica)
        {
            ContextQuery contextQuery = new ContextQuery();
            string resultado = string.Empty;

            contextQuery.Command = @" SELECT TOP 1 NOME_COMP
                                            FROM    DBO.LY_UNIDADE_FISICA
                                            WHERE   CEP = @CEP
                                                    AND ENDERECO = @ENDERECO
                                                    AND END_NUM = @END_NUM 
                                                    AND UNIDADE_FIS <> @UNIDADE_FIS  ";

            contextQuery.Parameters.Add("@CEP", TechneDbType.T_CEP, cep);
            contextQuery.Parameters.Add("@ENDERECO", TechneDbType.T_ALFAMEDIUM, endereco);
            contextQuery.Parameters.Add("@END_NUM", TechneDbType.T_ALFASMALL, numero);
            contextQuery.Parameters.Add("@UNIDADE_FIS", TechneDbType.T_CODIGO, unidadeFisica);

            resultado = contexto.GetReturnValue<string>(contextQuery);

            return resultado;
        }

        public void AtualizaCaracteristicasFisicas(DTOs.UnidadeCaracteristicasFisicas unidadeCaracteristicasFisicas)
        {
            RN.UnidadeEnsino rnUnidadeEnsino = new UnidadeEnsino();
            RN.GestaoRede.UnidadeFisicaFormasAcessibilidade rnUnidadeFisicaFormasAcessibilidade = new Techne.Lyceum.RN.GestaoRede.UnidadeFisicaFormasAcessibilidade();
            DataContext contexto = DataContextBuilder.FromLyceum.UsingLock();

            try
            {
                //Atualiza unidade Fisica
                this.AtualizaCaracteristicasFisicas(contexto, unidadeCaracteristicasFisicas);

                //Atualiza dados da unidade Ensino
                rnUnidadeEnsino.AtualizaCaracteristicasFisicas(contexto, unidadeCaracteristicasFisicas);

                //Remove todas as formas de acessibilidade
                rnUnidadeFisicaFormasAcessibilidade.RemovePorUnidade(contexto, unidadeCaracteristicasFisicas.UnidadeFisica);

                if (unidadeCaracteristicasFisicas.FormasAcessibilidade != null && unidadeCaracteristicasFisicas.FormasAcessibilidade.Count > 0)
                {
                    foreach (int idFormaAcessibilidade in unidadeCaracteristicasFisicas.FormasAcessibilidade)
                    {
                        //Inserir formas de acessibilidade
                        rnUnidadeFisicaFormasAcessibilidade.Insere(contexto, unidadeCaracteristicasFisicas.UnidadeFisica, idFormaAcessibilidade, unidadeCaracteristicasFisicas.UsuarioResponsavel);
                    }
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

        private void AtualizaCaracteristicasFisicas(DataContext contexto, DTOs.UnidadeCaracteristicasFisicas unidadeCaracteristicasFisicas)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE DBO.LY_UNIDADE_FISICA 
                                    SET    NOME_COMP = @NOME_COMP, 
                                           NOME_ABREV = @NOME_ABREV, 
                                           FORMA_OCUPACAO = @FORMA_OCUPACAO, 
                                           LOCAL_FUNCIONAMENTO = @LOCAL_FUNCIONAMENTO, 
                                           AREA_ASSENTAMENTO = @AREA_ASSENTAMENTO, 
                                           TERRA_INDIGENA = @TERRA_INDIGENA, 
                                           AREA_QUILOMBOS = @AREA_QUILOMBOS, 
                                           UNIDADE_SUSTENTAVEL = @UNIDADE_SUSTENTAVEL, 
                                           TIPO = @TIPO, 
                                           TITULAR_PREDIO = @TITULAR_PREDIO, 
                                           IMOVEL_COMPARTILHADO_REDE = @IMOVEL_COMPARTILHADO_REDE, 
                                           CEP = @CEP, 
                                           MUNICIPIO = @MUNICIPIO, 
                                           ENDERECO = @ENDERECO, 
                                           END_NUM = @END_NUM, 
                                           END_COMPL = @END_COMPL, 
                                           BAIRRO = @BAIRRO, 
                                           DISTRITO = @DISTRITO,
                                           ACESSO_NECESSIDADE_ESPECIAL = @ACESSO_NECESSIDADE_ESPECIAL, 
                                           ACESSO_DIFICIL = @ACESSO_DIFICIL, 
                                           AREA_TOTAL_TERRENO = @AREA_TOTAL_TERRENO, 
                                           AREA_TOTAL_CONSTRUIDA = @AREA_TOTAL_CONSTRUIDA, 
                                           AREA_TERRENO = @AREA_TERRENO, 
                                           E_MAIL = @E_MAIL, 
                                           CGC = @CGC, 
                                           FONE = @FONE, 
                                           FAX = @FAX, 
                                           MATRICULA = @MATRICULA 
                                    WHERE  UNIDADE_FIS = @UNIDADE_FIS ";

            contextQuery.Parameters.Add("@UNIDADE_FIS", TechneDbType.T_CODIGO, unidadeCaracteristicasFisicas.UnidadeFisica);
            contextQuery.Parameters.Add("@NOME_COMP", TechneDbType.T_ALFALARGE, unidadeCaracteristicasFisicas.NomeUnidadeFisica);
            contextQuery.Parameters.Add("@NOME_ABREV", TechneDbType.T_ALFAMEDIUM, unidadeCaracteristicasFisicas.NomeUnidadeFisica);
            contextQuery.Parameters.Add("@FORMA_OCUPACAO", TechneDbType.T_ALFALARGE, unidadeCaracteristicasFisicas.FormaOcupacaoLocalizacao);
            contextQuery.Parameters.Add("@LOCAL_FUNCIONAMENTO", SqlDbType.VarChar, unidadeCaracteristicasFisicas.LocalFuncionamento);
            contextQuery.Parameters.Add("@AREA_ASSENTAMENTO", SqlDbType.Char, unidadeCaracteristicasFisicas.AreaAssentamento);
            contextQuery.Parameters.Add("@TERRA_INDIGENA", SqlDbType.Char, unidadeCaracteristicasFisicas.TerraIndigena);
            contextQuery.Parameters.Add("@AREA_QUILOMBOS", SqlDbType.Char, unidadeCaracteristicasFisicas.AreaQuilombo);
            contextQuery.Parameters.Add("@UNIDADE_SUSTENTAVEL", SqlDbType.Char, unidadeCaracteristicasFisicas.UnidadeSustentavel);
            contextQuery.Parameters.Add("@TIPO", TechneDbType.T_ALFALARGE, unidadeCaracteristicasFisicas.FormaOcupacaoTipo);
            contextQuery.Parameters.Add("@TITULAR_PREDIO", TechneDbType.T_SIMNAO, unidadeCaracteristicasFisicas.OcupacaoFormal);
            contextQuery.Parameters.Add("@IMOVEL_COMPARTILHADO_REDE", TechneDbType.T_SIMNAO, unidadeCaracteristicasFisicas.ImovelCompartilhadoRede);
            contextQuery.Parameters.Add("@CEP", TechneDbType.T_CEP, unidadeCaracteristicasFisicas.Cep);
            contextQuery.Parameters.Add("@MUNICIPIO", TechneDbType.T_CODIGO, unidadeCaracteristicasFisicas.Municipio);
            contextQuery.Parameters.Add("@ENDERECO", TechneDbType.T_ALFAMEDIUM, unidadeCaracteristicasFisicas.Endereco);
            contextQuery.Parameters.Add("@END_NUM", TechneDbType.T_ALFASMALL, unidadeCaracteristicasFisicas.EnderecoNumero);
            contextQuery.Parameters.Add("@END_COMPL", TechneDbType.T_ALFAMEDIUM, unidadeCaracteristicasFisicas.EnderecoComplemento);
            contextQuery.Parameters.Add("@BAIRRO", TechneDbType.T_ALFAMEDIUM, unidadeCaracteristicasFisicas.EnderecoBairro);
            contextQuery.Parameters.Add("@DISTRITO", SqlDbType.VarChar, unidadeCaracteristicasFisicas.Distrito);
            contextQuery.Parameters.Add("@ACESSO_NECESSIDADE_ESPECIAL", TechneDbType.T_ALFAMEDIUM, unidadeCaracteristicasFisicas.AcessoNecessidadeEspecial);
            contextQuery.Parameters.Add("@ACESSO_DIFICIL", TechneDbType.T_SIMNAO, unidadeCaracteristicasFisicas.AcessoDificil);
            contextQuery.Parameters.Add("@E_MAIL", SqlDbType.VarChar, unidadeCaracteristicasFisicas.Email);
            contextQuery.Parameters.Add("@CGC", TechneDbType.T_CGC, unidadeCaracteristicasFisicas.Cnpj);
            contextQuery.Parameters.Add("@FONE", TechneDbType.T_TELEFONE, unidadeCaracteristicasFisicas.Telefone1);
            contextQuery.Parameters.Add("@FAX", TechneDbType.T_TELEFONE, unidadeCaracteristicasFisicas.Fax);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, unidadeCaracteristicasFisicas.UsuarioResponsavel);

            if (unidadeCaracteristicasFisicas.AreaTotalTerreno == null || unidadeCaracteristicasFisicas.AreaTotalTerreno <= 0)
            {
                contextQuery.Parameters.Add("@AREA_TOTAL_TERRENO", TechneDbType.T_DECIMAL_MEDIO, null);
            }
            else
            {
                contextQuery.Parameters.Add("@AREA_TOTAL_TERRENO", TechneDbType.T_DECIMAL_MEDIO, unidadeCaracteristicasFisicas.AreaTotalTerreno);
            }

            if (unidadeCaracteristicasFisicas.AreaTotalConstruida == null || unidadeCaracteristicasFisicas.AreaTotalConstruida <= 0)
            {
                contextQuery.Parameters.Add("@AREA_TOTAL_CONSTRUIDA", TechneDbType.T_DECIMAL_MEDIO, null);
            }
            else
            {
                contextQuery.Parameters.Add("@AREA_TOTAL_CONSTRUIDA", TechneDbType.T_DECIMAL_MEDIO, unidadeCaracteristicasFisicas.AreaTotalConstruida);
            }

            if (unidadeCaracteristicasFisicas.AreaTerreno == null || unidadeCaracteristicasFisicas.AreaTerreno <= 0)
            {
                contextQuery.Parameters.Add("@AREA_TERRENO", TechneDbType.T_DECIMAL_MEDIO, null);
            }
            else
            {
                contextQuery.Parameters.Add("@AREA_TERRENO", TechneDbType.T_DECIMAL_MEDIO, unidadeCaracteristicasFisicas.AreaTerreno);
            }

            contexto.ApplyModifications(contextQuery);
        }

        public void AtualizaDadosPedagogicos(DataContext contexto, DTOs.UnidadeDadosPedagogicos unidadeDadosPedagogicos)
        {
            ContextQuery contextQuery = new ContextQuery();

            contextQuery.Command = @" UPDATE DBO.LY_UNIDADE_FISICA 
                                    SET    ESPACOEQUIPAMENTOENTORNO = @ESPACOEQUIPAMENTOENTORNO,    
                                           ESPACOESCOLACOMUNIDADE = @ESPACOESCOLACOMUNIDADE,
                                            Educacaoambiental = @Educacaoambiental,
                                            ConteudoComponentes = @ConteudoComponentes,
                                            Componentecurricular = @Componentecurricular,
                                            EixoEstuturante = @EixoEstuturante,
                                            EmEventos = @EmEventos,
                                            ProjetosTransversais = @ProjetosTransversais,
                                            NOL = @NOL,
                                           MATRICULA = @MATRICULA 
                                    WHERE  UNIDADE_FIS = @UNIDADE_FIS ";

            contextQuery.Parameters.Add("@UNIDADE_FIS", SqlDbType.VarChar, unidadeDadosPedagogicos.Censo);
            contextQuery.Parameters.Add("@ESPACOEQUIPAMENTOENTORNO", SqlDbType.VarChar, unidadeDadosPedagogicos.EspacoEquipamentoEntorno);
            contextQuery.Parameters.Add("@ESPACOESCOLACOMUNIDADE", SqlDbType.VarChar, unidadeDadosPedagogicos.EspacoEscolaComunidade);
            contextQuery.Parameters.Add("@Educacaoambiental", SqlDbType.VarChar, unidadeDadosPedagogicos.Educacaoambiental);
            contextQuery.Parameters.Add("@ConteudoComponentes", SqlDbType.VarChar, unidadeDadosPedagogicos.ConteudoComponentes);
            contextQuery.Parameters.Add("@Componentecurricular", SqlDbType.VarChar, unidadeDadosPedagogicos.Componentecurricular);
            contextQuery.Parameters.Add("@EixoEstuturante", SqlDbType.VarChar, unidadeDadosPedagogicos.EixoEstuturante);
            contextQuery.Parameters.Add("@EmEventos", SqlDbType.VarChar, unidadeDadosPedagogicos.EmEventos);
            contextQuery.Parameters.Add("@ProjetosTransversais", SqlDbType.VarChar, unidadeDadosPedagogicos.ProjetosTransversais);
            contextQuery.Parameters.Add("@NOL", SqlDbType.VarChar, unidadeDadosPedagogicos.NOL);
            contextQuery.Parameters.Add("@MATRICULA", SqlDbType.VarChar, unidadeDadosPedagogicos.UsuarioResponsavel);

            contexto.ApplyModifications(contextQuery);
        }

        public bool EhEscolaRuralPor(DataContext contexto, string censo)
        {
            ContextQuery contextQuery = new ContextQuery();
            bool existe = false;


            contextQuery.Command = @" SELECT COUNT(1) 
                                    FROM LY_UNIDADE_FISICA
                                    WHERE FORMA_OCUPACAO = 'RURAL'
                                        AND UNIDADE_FIS = @CENSO ";

            contextQuery.Parameters.Add("@CENSO", TechneDbType.T_CODIGO, censo); 

            if (contexto.GetReturnValue<int>(contextQuery) > 0)
            {
                existe = true;
            }

            return existe;
        }
    }
}
