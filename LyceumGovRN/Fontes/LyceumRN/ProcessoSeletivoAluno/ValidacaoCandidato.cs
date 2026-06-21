using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Seeduc.Infra.Validation;
using Techne.Lyceum.RN.Servicos;
using Techne.Lyceum.RN.Util;
using Seeduc.Infra.Extensions;
using System.Text.RegularExpressions;

namespace Techne.Lyceum.RN.ProcessoSeletivoAluno
{
    public class ValidacaoCandidato
    {
        public static void ValidaTelefone(string numeroTelefone, string campoNome, ref List<string> mensagens)
        {
            if (!string.IsNullOrEmpty(numeroTelefone))
            {
                var telefone = Utils.RetirarMascara(numeroTelefone);

                if (!Validacao.ValidaTelefoneComDDD(telefone))
                {
                    mensagens.Add("O campo Telefone " + (campoNome.Equals("Mãe") ? "da " : "do ") + campoNome + " é inválido.!");
                }
            }
        }

        public static void ValidaDadosNome(string nome, string campoNome, string nomeCandidato, ref List<string> mensagens)
        {
            if (!Validacao.SomenteLetras(nome))
            {
                mensagens.Add("O campo " + campoNome + " não pode conter números e/ou caracteres especiais.");
                return;
            }

            if (VerificaExcessoApostrofo(nome))
            {
                mensagens.Add(campoNome + " possui inconsistência por repetição excessiva de apóstrofes. Favor corrigir a informação.");
                return;
            }

            if (ValidaEspaco(nome) != nome)
            {
                mensagens.Add("Não é permitido mais de um espaço no campo " + campoNome + ".");
            }


            if (nome.Length < 5)
            {
                mensagens.Add("O campo " + campoNome + " deve conter pelo menos cinco letras!");
                return;
            }

            var palavras = nome.CountWords();
            var contemRepeticao = RN.Validacao.ContemRepeticao(nome, 3);
            var nomeInvalido = TextValidator.HasForbiddenWords(nome, new PalavrasProibidasEmNomes());

            if (contemNumeros(nome) == false)
            {
                if (palavras < 2)
                {
                    mensagens.Add("Por favor informar nome e sobrenome no campo " + campoNome + ".");
                }

                if (contemRepeticao)
                {
                    mensagens.Add("O campo " + campoNome + " possui inconsistência por repetição excessiva de letras. Favor corrigir a informação.");
                }

                if (nomeInvalido)
                {
                    mensagens.Add("O campo " + campoNome + " possui inconsistência por não representar um nome válido. Favor corrigir a informação.");
                }
            }

            //Validação de Abreviação
            var nomesAbreviacao = nome.Trim().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < nomesAbreviacao.Length; i++)
            {
                var parteNome = nomesAbreviacao[i];

                if (parteNome.Length == 2 && !ehAbreviacaoValida(parteNome) && parteNome.IndexOf(".") != -1)
                {
                    parteNome = parteNome.Remove(1);
                }
                if ((parteNome.Length == 1
                    && string.Compare(parteNome, "e", true) != 0)
                    || (string.Compare(parteNome, "e", true) == 0
                    && (i == 0
                    || i == nomesAbreviacao.Length - 1)))
                {
                    if (string.IsNullOrEmpty(parteNome.Trim()))
                    {
                        mensagens.Add("Não é possível utilizar abreviações no " + campoNome + ".");
                    }
                    else if (!Aluno.ExisteAbreviacao(parteNome.Trim()))
                    {
                        mensagens.Add("Não é possível utilizar abreviações no " + campoNome + ".");
                    }
                }
                else if (parteNome.IndexOf(".") != -1)
                {
                    mensagens.Add("Não é possível utilizar abreviações no " + campoNome + ".");
                }
            }

            if (campoNome != "Nome do Candidato")
            {
                if (!string.IsNullOrEmpty(nomeCandidato) && nomeCandidato == nome)
                {
                    mensagens.Add(campoNome + " não pode ser igual ao nome do candidato.");
                }
            }
        }

        public static string ValidaEspaco(string txt)
        {
            string input = txt;
            string pattern = "\\s+";
            string replacement = " ";
            Regex rgx = new Regex(pattern);
            string result = rgx.Replace(input, replacement);

            return result;
        }

        public static bool ehAbreviacaoValida(string abreviacao)
        {
            switch (abreviacao)
            {
                case "DA":
                case "DE":
                case "DI":
                case "DO":
                case "DU":
                    return true;
                default:
                    break;
            }

            return false;
        }

        private static bool contemNumeros(string texto)
        {
            if (texto.Where(c => char.IsNumber(c)).Count() > 0)
                return true;
            else
                return false;
        }

        public static void ValidaCamposResponsavel(string tipoResponsavel, string valorCPF, System.Web.UI.WebControls.CheckBox chkNaoDeclarado, ref List<string> mensagens)
        {
            if (string.IsNullOrEmpty(valorCPF))
                mensagens.Add("CPF do Responsável Legal (" + tipoResponsavel + ") não informado.");

            if (chkNaoDeclarado != null)
            {
                if (chkNaoDeclarado.Checked)
                {
                    mensagens.Add("O campo Nome " + (tipoResponsavel.Equals("Mãe") ? "da " : "do ") + tipoResponsavel + " deve ser diferente de não declarado(a) quando escolhido(a) como Responsável Legal.");
                }
            }
        }

        public static void ValidaCampoCPF(string tipoResponsavel, string valorCPF, ref List<string> mensagens)
        {
            if (!string.IsNullOrEmpty(valorCPF))
            {
                if (!RN.Validacao.ValidaCpf(valorCPF.RetirarMascaraCPF()))
                {
                    mensagens.Add("CPF " + (tipoResponsavel.Equals("Mãe") ? "da " : "do ") + tipoResponsavel + " é inválido.");
                }
            }
        }

        private static bool VerificaExcessoApostrofo(string valorCampo)
        {
            Regex regex = new Regex(@"\s{2,}");

            string valorValidacao = regex.Replace(valorCampo.Trim().ToUpper(), " ");
            var contemApostrofeRep = Validacao.substitueApostrofe(valorValidacao);

            if (contemApostrofeRep)
                return true;

            return false;
        }
    }
}
