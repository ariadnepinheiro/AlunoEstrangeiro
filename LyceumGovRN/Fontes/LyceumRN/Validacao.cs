using System;
using System.Text.RegularExpressions;
using Seeduc.Infra.Data;
using Techne.Data;
using Seeduc.Infra.Validation;
using System.Collections.Generic;
using Seeduc.Infra.Extensions;
using Techne.Lyceum.RN.Servicos;
using System.Linq;

namespace Techne.Lyceum.RN
{
    public class Validacao : RNBase
    {
        public enum Tipo
        {
            numerico,
            texto,
            nome,
            data,
            email,
            decimal3,
            decimal8,
            dinheiro,
            dinheiroGrande
        }

        public static bool ContemRepeticao(string nome, int quantidade)
        {
            //Retira repetiçoes permitidas
            nome = nome.Replace("III", string.Empty);
            nome = nome.Replace("II", string.Empty);

            bool contemRepeticao = TextValidator.HasCharRepetition(nome, quantidade);

            return contemRepeticao;
        }


        public static bool SomenteLetras(string nome)
        {
            if (Regex.Match(nome, @"^[+]?.*\d+.*$").Success == false)
            {
                if (nome.IndexOf("'") != -1)
                {
                    return true;
                }
                else if (nome.IndexOf("-") != -1)
                {
                    return true;
                }
                else
                {
                    var match = Regex.Match(nome, @"^[+]?([\w]|[\s])*$");
                    return match.Success;
                }
            }
            return false;
        }

        public static bool SomenteLetrasNumeros(string nome)
        {
            Regex rx = new Regex(@"^[A-Za-z0-9]");
            bool resultado = rx.IsMatch(nome);

            if (resultado)
            {
                if (nome.Contains(".") || nome.Contains(","))
                {
                    resultado = false;
                }
            }

            return resultado;
        }

        public static bool contemNumeros(string texto)
        {
            if (texto.Where(c => char.IsNumber(c)).Count() > 0)
                return true;
            else
                return false;
        }

        public static bool VerificarCaracteres(string nome)
        {
            if (Regex.Match(nome, @"(\b(\w+)[;,]?\s?)+([.?!])").Success == false)
            {
                var match = Regex.Match(nome, @"(\b(\w+)[;,]?\s?)+([.?!])");
                return match.Success;
            }
            return false;
        }

        public static bool Email(string email)
        {
            if (email.Contains("..") || email.Contains(".@"))
            {
                return false;
            }

            var match = Regex.Match(email, @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            return match.Success;
        }

        public static bool Validou(string campo, Tipo tipo)
        {
            Match match = null;
            if (tipo == Tipo.numerico)
            {
                match = Regex.Match(campo, @"^[+]?\d*$");
            }
            else if (tipo == Tipo.texto)
            {
                match = Regex.Match(campo, @"^[+]?([\w]|[\s])*$");
            }
            else if (tipo == Tipo.nome)
            {
                if (Regex.Match(campo, @"^[+]?.*\d+.*$").Success == false)
                {
                    match = Regex.Match(campo, @"^[+]?([\w]|[\s])*$");
                }
                else
                {
                    return false;
                }
            }
            else if (tipo == Tipo.email)
            {
                match = Regex.Match(campo, @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            }
            else if (tipo == Tipo.decimal3)
            {
                match = Regex.Match(campo, @"^\d{1,3}(.\d{1,4})?$");
            }
            else if (tipo == Tipo.decimal8)
            {
                match = Regex.Match(campo, @"^\d{1,8}(.\d{1,2})?$");
            }
            else if (tipo == Tipo.dinheiro)
            {
                match = Regex.Match(campo, @"^\d{1,8}(,\d{2})?$");
            }

            if (match == null)
            {
                return true;
            }

            return match.Success;
        }

        public static bool ValidouData(DateTime? data, Tipo tipo)
        {
            if (data != null)
            {
                if (data.Value.Year <= 1900)
                {
                    return false;
                }
                DateTime hoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                if (data >= hoje)
                {
                    return false;
                }
                return true;
            }
            return true;
        }

        public static bool ValidouAno(decimal? ano)
        {
            if (ano != null)
            {
                if (ano < 1900)
                {
                    return false;
                }
                decimal atual = DateTime.Now.Year;
                if (ano > atual)
                {
                    return true;
                }
            }
            return true;
        }

        public static bool ValidouDataPodeHoje(DateTime? data, Tipo tipo)
        {
            if (data != null)
            {
                if (data.Value.Year <= 1900)
                {
                    return false;
                }
                DateTime hoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                if (data > hoje)
                {
                    return false;
                }
                return true;
            }
            return true;
        }

        public static bool ValidouDuasDatas(DateTime? data1, DateTime? data2)
        {
            if (data1 != null && data2 != null)
            {
                if (data1 > data2)
                {
                    return false;
                }
            }
            return true;
        }

        //public bool ValidaCnpj(string cnpj)
        //{
        //    int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        //    int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        //    int soma;
        //    int resto;
        //    string digito;
        //    string tempCnpj;

        //    cnpj = cnpj.Trim();
        //    cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

        //    if (cnpj.Length != 14)
        //    {
        //        return false;
        //    }

        //    tempCnpj = cnpj.Substring(0, 12);

        //    soma = 0;
        //    for (int i = 0; i < 12; i++)
        //    {
        //        soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
        //    }

        //    resto = (soma % 11);
        //    if (resto < 2)
        //    {
        //        resto = 0;
        //    }
        //    else
        //    {
        //        resto = 11 - resto;
        //    }

        //    digito = resto.ToString();

        //    tempCnpj = tempCnpj + digito;
        //    soma = 0;
        //    for (int i = 0; i < 13; i++)
        //    {
        //        soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
        //    }

        //    resto = (soma % 11);
        //    if (resto < 2)
        //    {
        //        resto = 0;
        //    }
        //    else
        //    {
        //        resto = 11 - resto;
        //    }

        //    digito = digito + resto.ToString();

        //    return cnpj.EndsWith(digito);
        //}

        public static bool ValidaCnpj(string cnpj)
        {
            if (string.IsNullOrEmpty(cnpj))
            {
                return false;
            }

            byte[] cgcarray = new byte[14];
            {
                string scnpj = cnpj.ToString();
                int i = 13;
                for (int j = scnpj.Length - 1; j >= 0; j--)
                    if (char.IsDigit(scnpj[j]))
                    {
                        if (i < 0)
                        {
                            return false;
                        }
                        cgcarray[i--] = byte.Parse(scnpj[j].ToString());
                    }
                    else if (char.IsLetter(scnpj[j]))
                    {
                        return false;
                    }
                while (i >= 0)
                {
                    cgcarray[i--] = 0;
                }
            }

            // Primeira verificação (loop)
            int soma1 = 0;
            int flag1 = 2;
            for (int i = 11; i >= 0; i--)
            {
                soma1 += cgcarray[i] * flag1;
                flag1 = flag1 == 9 ? 2 : flag1 + 1;
            }

            // Erro encontrado, valor inválido
            if (soma1 == 0)
            {
                return false;
            }

            // Calcula resto
            int resto = soma1 - soma1 / 11 * 11;
            resto = resto == 0 || resto == 1 ? 0 : 11 - resto;

            byte[] cgcarray2 = new byte[13];
            Array.Copy(cgcarray, 0, cgcarray2, 0, 12);
            cgcarray2[12] = (byte)resto;

            // Segunda verificação (loop)
            int soma2 = 0;
            int flag2 = 2;
            for (int i = 12; i >= 0; i--)
            {
                soma2 += cgcarray2[i] * flag2;
                flag2 = flag2 == 9 ? 2 : flag2 + 1;
            }

            // Calcula resto
            int resto2 = soma2 - soma2 / 11 * 11;
            resto2 = resto2 == 0 || resto2 == 1 ? 0 : 11 - resto2;

            return cgcarray[12] == resto && cgcarray[13] == resto2;
        }

        public static bool ValidaCpf(string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
            {
                return false;
            }
            string scpf = cpf.ToString();

            // Verifica se sÃ³ contÃ©m dÃ­gitos.
            foreach (char c in scpf)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }

            // Tamanho entre 3 e 11
            if (scpf.Length < 3 || scpf.Length > 11)
            {
                return false;
            }

            // Preenche com zeros Ã  esquerda atÃ© totalizar 11 dÃ­gitos
            scpf = scpf.PadLeft(11, '0');

            // Não permite CPFs com códigos 00000000000 à 999999999
            if ((scpf == "00000000000") || (scpf == "11111111111") || (scpf == "22222222222") ||
               (scpf == "33333333333") || (scpf == "44444444444") || (scpf == "55555555555") ||
               (scpf == "66666666666") || (scpf == "77777777777") || (scpf == "88888888888") ||
               (scpf == "99999999999")) return false;


            byte digito1 = byte.Parse(scpf[9].ToString());
            byte digito2 = byte.Parse(scpf[10].ToString());

            // Verificando o primeiro dÃ­gito
            int somador1 = 0;
            for (int i = 10; i >= 2; i--)
                somador1 += byte.Parse(scpf[11 - i - 1].ToString()) * i;

            somador1 -= somador1 / 11 * 11;

            if (somador1 == 0 || somador1 == 1)
            {
                if (digito1 != 0)
                {
                    return false;
                }
            }
            else if (11 - somador1 != digito1)
            {
                return false;
            }

            // Verificando o segundo dÃ­gito
            int somador2 = 0;
            for (int i = 11; i >= 2; i--)
            {
                somador2 += byte.Parse(scpf[12 - i - 1].ToString()) * i;
            }

            somador2 -= somador2 / 11 * 11;

            if (somador2 == 0 || somador2 == 1)
            {
                if (digito2 != 0)
                {
                    return false;
                }
            }
            else if (11 - somador2 != digito2)
            {
                return false;
            }

            return true;
        }

        public static bool ValidouPISPASEP(string numero)
        {
            string ftap;
            decimal total;
            int i;
            int resto;

            if (Convert.ToDecimal(numero) == 0 || numero.Length != 11)
                return false;

            ftap = "3298765432";
            total = 0;

            for (i = 0; i < 10; i++)
            {
                total = total + Convert.ToDecimal(numero.Substring(i, 1)) * Convert.ToDecimal(ftap.Substring(i, 1));
            }
            resto = Convert.ToInt16(total % 11);
            if (resto != 0)
            {
                resto = 11 - resto;
            }
            if (resto != Convert.ToInt16(numero.Substring(10, 1)))
            {
                return false;
            }

            return true;
        }

        public static bool ValidaNumerosInteirosPositivos(string numero)
        {
            Regex rx = new Regex(@"^\d+$");
            return rx.IsMatch(numero);
        }

        public static bool ValidaNumerosInteirosPositivosOuVazios(string numero)
        {
            Regex rx = new Regex(@"^\d*$");
            return rx.IsMatch(numero);
        }

        /// <summary>
        /// Verifica se o email é válido ou não.
        /// DEFINIÇÃO: Email deve ter obrigatoriamente um único caracter de arroba, pelo menos um ponto após a arroba
        /// e texto entre 2 e 4 caracteres após este ponto.
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns></returns>
        public static bool ValidaEmail(string email)
        {
            if (email.Contains("..") || email.Contains(".@"))
            {
                return false;
            }
            Regex padrao = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return padrao.IsMatch(email);
        }

        /// <summary>
        /// Verifica se o telefone é válido ou não.
        /// DEFINIÇÃO: Telefone deve possuir exatamente 8 dígitos dos quais o primeiro deve ser entre 1 e 5.
        /// </summary>
        /// <param name="telefone">Telefone sem DDD</param>
        /// <returns></returns>
        public static bool ValidaTelefoneSemDDD(string telefone)
        {
            System.Text.RegularExpressions.Regex expressao =
                new System.Text.RegularExpressions.Regex(@"[1-5]\d{3}\d{4}");
            return expressao.IsMatch(telefone);
        }

        /// <summary>
        /// Verifica se o telefone é válido ou não.
        /// </summary>
        /// <param name="telefone">Telefone com DDD</param>
        /// <returns></returns>
        public static bool ValidaTelefoneComDDD(string telefone)
        {
            System.Text.RegularExpressions.Regex expressao =
                new System.Text.RegularExpressions.Regex(@"[1][1-9][1-5]\d{3}\d{4}|[2-9][0-9][1-5]\d{3}\d{4}");
            return expressao.IsMatch(telefone);
        }

        public static bool Bairro(string bairro)
        {
            System.Text.RegularExpressions.Regex expressao = new System.Text.RegularExpressions.Regex(@"^[A-Za-zÁáÀàÂâÃãÉéÈèÊêÍíÌìÎîÓóÒòÔôÕõÚúÙùÛûÇçÑñ0-9\)\(\- ]{3,50}$");
            return expressao.IsMatch(bairro);
        }

        /// <summary>
        /// Verifica se o celular é válido ou não.
        /// </summary>
        /// <param name="celular">Celular com DDD</param>
        /// <returns></returns>
        public static bool ValidaCelularComDDD(string celular)
        {
            System.Text.RegularExpressions.Regex expressao =
                new System.Text.RegularExpressions.Regex(@"[1][1-9][6-9]\d{3}\d{4}|[2-9][0-9][6-9]\d{3}\d{4}");
            return expressao.IsMatch(celular);
        }

        public static bool ValidaBairro(string bairro)
        {

            System.Text.RegularExpressions.Regex expressao = new System.Text.RegularExpressions.Regex(@"^[A-Za-zÁáÀàÂâÃãÉéÈèÊêÍíÌìÎîÓóÒòÔôÕõÚúÙùÛûÇçÑñ'0-9\)\(\- ]{3,50}$");
            return expressao.IsMatch(bairro);
        }

        /// <summary>
        /// Verifica se o DDD é válido ou não.
        /// DEFINIÇÃO: DDD deve possuir exatamente 2 dígitos e compreende os valores entre 11 e 99.
        /// </summary>
        /// <param name="ddd">DDD</param>
        /// <returns></returns>
        public static bool ValidaDDD(string ddd)
        {
            System.Text.RegularExpressions.Regex expressao =
                new System.Text.RegularExpressions.Regex(@"[1][1-9]|[2-9][0-9]");
            return expressao.IsMatch(ddd);
        }

        public static bool IsNumeric(string anyString)
        {
            bool isNumeric = true;
            long resultDate;
            if (anyString == null)
            {
                anyString = "";
            }
            try
            {
                resultDate = long.Parse(anyString);
                isNumeric = true;
            }
            catch
            {
                isNumeric = false;
            }
            return isNumeric;
        }

        public static bool ValidarCEP(string cep)
        {
            using (var ctx = DataContextBuilder.FromHades.ToFastReadingOnly())
            {
                var contextQuery = new ContextQuery(
                    @"  SELECT 1 FROM HADES.dbo.TCE_LOGRADOURO
                        WHERE CEP = @CEP
                        ");

                contextQuery.Parameters.Add("@CEP", cep);

                var obj = ctx.GetReturnValue(contextQuery);

                if (obj != null)
                {
                    return true;
                }
            }
            return false;
        }

        public static int DigitoModulo11(string numero)
        {
            //variáveis de instancia  
            int soma = 0;
            int resto = 0;
            int dv = 0;
            String[] numeros = new String[numero.Length + 1];
            int multiplicador = 2;

            for (int i = numero.Length; i > 0; i--)
            {
                //Multiplica da direita pra esquerda, incrementando o multiplicador de 2 a 9  
                //Caso o multiplicador seja maior que 9 o mesmo recomeça em 2  
                if (multiplicador > 9)
                {
                    // pega cada numero isoladamente    
                    multiplicador = 2;
                    numeros[i] = (Convert.ToInt32(numero.Substring(i - 1, 1)) * multiplicador).ToString();
                    multiplicador++;
                }
                else
                {
                    numeros[i] = (Convert.ToInt32(numero.Substring(i - 1, 1)) * multiplicador).ToString();
                    multiplicador++;
                }
            }

            //Realiza a soma de todos os elementos do array e calcula o digito verificador  
            //na base 11 de acordo com a regra.       
            for (int i = numeros.Length; i > 0; i--)
            {
                if (numeros[i - 1] != null)
                {
                    soma += Convert.ToInt32(numeros[i - 1]);
                }
            }
            resto = soma % 11;
            dv = 11 - resto;

            //retorna o digito verificador  
            return dv;
        }
        public static bool substitueApostrofe(string texto)
        {
            for (int i = 1; i < texto.Length; i++)
            {
                if (texto.Substring(i, 1) == "'" && texto.Substring(i - 1, 1) == "'")
                {
                    return true;
                }
            }
            return false;

        }
        public static int GeraDigitoRG(string NuRg, bool Gerar)
        {
            String strDig;
            int tamRG;
            Int16[] intNum = new Int16[5];
            int iSoma = 0;
            int dv = 0;
            int i;
            if (NuRg != "")
            {
                tamRG = NuRg.Trim().Length;
                strDig = NuRg.Substring(tamRG - 1, 1);
                if (!Gerar)
                {
                    NuRg = NuRg.Substring(0, tamRG - 1);
                }
                NuRg = NuRg.PadLeft(9, '0');
                intNum[0] = Convert.ToInt16(NuRg.Substring(0, 1));
                intNum[1] = Convert.ToInt16(NuRg.Substring(1, 2));
                intNum[2] = Convert.ToInt16(NuRg.Substring(3, 2));
                intNum[3] = Convert.ToInt16(NuRg.Substring(5, 2));
                intNum[4] = Convert.ToInt16(NuRg.Substring(7, 2));
                for (i = 0; i < 5; i++)
                {
                    if (intNum[i] == 9 || intNum[i] == 14 || intNum[i] == 28 ||
                    intNum[i] == 33 || intNum[i] == 47 || intNum[i] == 52 ||
                    intNum[i] == 66 || intNum[i] == 71 || intNum[i] == 85 ||
                    intNum[i] == 90)
                    {
                        iSoma += 1;
                    }
                    else if (intNum[i] == 4 || intNum[i] == 18 || intNum[i] == 23 ||
                    intNum[i] == 37 || intNum[i] == 42 || intNum[i] == 56 ||
                    intNum[i] == 61 || intNum[i] == 75 || intNum[i] == 80 ||
                    intNum[i] == 99)
                    {
                        iSoma += 2;
                    }
                    else if (intNum[i] == 8 || intNum[i] == 13 || intNum[i] == 27 ||
                    intNum[i] == 32 || intNum[i] == 46 || intNum[i] == 51 ||
                    intNum[i] == 65 || intNum[i] == 70 || intNum[i] == 89 ||
                    intNum[i] == 94)
                    {
                        iSoma += 3;
                    }
                    else if (intNum[i] == 3 || intNum[i] == 17 || intNum[i] == 22 ||
                    intNum[i] == 36 || intNum[i] == 41 || intNum[i] == 55 ||
                    intNum[i] == 60 || intNum[i] == 79 || intNum[i] == 84 ||
                    intNum[i] == 98)
                    {
                        iSoma += 4;
                    }
                    else if (intNum[i] == 7 || intNum[i] == 12 || intNum[i] == 26 ||
                    intNum[i] == 31 || intNum[i] == 45 || intNum[i] == 50 ||
                    intNum[i] == 69 || intNum[i] == 74 || intNum[i] == 88 ||
                    intNum[i] == 93)
                    {
                        iSoma += 5;
                    }
                    else if (intNum[i] == 2 || intNum[i] == 16 || intNum[i] == 21 ||
                    intNum[i] == 35 || intNum[i] == 40 || intNum[i] == 59 ||
                    intNum[i] == 64 || intNum[i] == 78 || intNum[i] == 83 ||
                    intNum[i] == 97)
                    {
                        iSoma += 6;
                    }
                    else if (intNum[i] == 6 || intNum[i] == 11 || intNum[i] == 25 ||
                    intNum[i] == 30 || intNum[i] == 49 || intNum[i] == 54 ||
                    intNum[i] == 68 || intNum[i] == 73 || intNum[i] == 87 ||
                    intNum[i] == 92)
                    {
                        iSoma += 7;
                    }
                    else if (intNum[i] == 1 || intNum[i] == 15 || intNum[i] == 20 ||
                    intNum[i] == 39 || intNum[i] == 44 || intNum[i] == 58 ||
                    intNum[i] == 63 || intNum[i] == 77 || intNum[i] == 82 ||
                    intNum[i] == 96)
                    {
                        iSoma += 8;
                    }
                    else if (intNum[i] == 5 || intNum[i] == 10 || intNum[i] == 29 ||
                    intNum[i] == 34 || intNum[i] == 48 || intNum[i] == 53 ||
                    intNum[i] == 67 || intNum[i] == 72 || intNum[i] == 86 ||
                    intNum[i] == 91)
                    {
                        iSoma += 9;
                    }
                }
                string resultSoma = iSoma.ToString().Trim();
                int tamSoma = resultSoma.Length;
                dv = Convert.ToInt16(resultSoma.Substring(tamSoma - 1, 1));
                if (Gerar)
                {
                    return dv;
                }
                else if (dv == Convert.ToInt16(strDig))
                {
                    return dv;
                }
            }
            return -1;
        }

        public string ValidaNomeProprio(string nomeCampo, string valor)
        {
            IList<string> erros = new List<string>();
            string retorno = string.Empty;

            erros = ValidaBuscaFoneticaAlunoNovo(valor, nomeCampo);

            foreach (string erro in erros)
                retorno += erro + "<br />";

            return retorno;
        }

        public List<string> ValidaBuscaFoneticaAlunoNovo(string parametro, string tipo)
        {
            List<string> mensagens = new List<string>();
            bool contemRepeticao;
            bool nomeInvalido;
            Pessoa rnPessoa = new Pessoa();
            RN.Validacao rnValidacao = new Validacao();
            string[] nomes;
            List<string> nomesValidos = new List<string>();

            try
            {
                contemRepeticao = RN.Validacao.ContemRepeticao(parametro, 3);
                nomeInvalido = TextValidator.HasForbiddenWords(parametro, new PalavrasProibidasEmNomes());

                if (contemNumeros(parametro) == false)
                {
                    if (contemRepeticao)
                    {
                        mensagens.Add("O campo " + tipo + " possui inconsistência por repetição excessiva de letras. Favor corrigir a informação.");
                    }

                    if (nomeInvalido)
                    {
                        mensagens.Add("O campo " + tipo + "  possui inconsistência por não representar um nome válido. Favor corrigir a informação.");
                    }
                }
                else if (!string.IsNullOrEmpty(parametro) && !Validacao.SomenteLetras(parametro))
                {
                    mensagens.Add("O campo " + tipo + "  não pode conter números.");
                }

                nomes = parametro.Trim().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

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
                        mensagens.Add("Não é possível utilizar abreviações no  " + tipo + ".");
                    }
                }

                for (var i = 0; i < nomes.Length; i++)
                {
                    var nome = nomes[i];
                    if (!ehPreposicao(nome))
                    {
                        nomesValidos.Add(nome);
                    }
                }
                if (nomesValidos.Count < 2)
                {
                    mensagens.Add("Por favor informar, nome e sobrenome no campo " + tipo + "(Preposições não são consideradas).");
                }

                return mensagens;
            }
            catch (Exception ex)
            {
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

        }

        public bool ehAbreviacaoValida(string abreviacao)
        {
            switch (abreviacao)
            {
                case "DA":
                case "DE":
                case "DI":
                case "DO":
                case "DU":
                case "SÁ":
                case "JÓ":
                case "Ó":
                    return true;
                default:
                    break;
            }

            return false;
        }

        public bool ehPreposicao(string preposicao)
        {
            switch (preposicao.ToUpper())
            {
                case "DA":
                case "DE":
                case "DI":
                case "DO":
                case "DU":
                case "DAS":
                case "DES":
                case "DIS":
                case "DOS":
                case "DUS":
                    return true;
                default:
                    break;
            }

            return false;
        }
    }
}
