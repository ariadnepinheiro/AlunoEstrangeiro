using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace SRV.Common.Validation
{
    public class Cpf
    {
        public static bool ValidaCpf(String cpf)
        {
            cpf = RetiraMascaraCpf(cpf);

            int d1 = 0, d2 = 0;
            int digito1 = 0, digito2 = 0, resto = 0;
            int digitoCPF;

            String digResult;

            for (int i = 1; i < cpf.Length - 1; i++)
            {
                digitoCPF = int.Parse(cpf.Substring(i - 1, 1));

                //multiplica a última casa por 2 a seguinte por 3 a seguinte por 4 e assim por diante.  
                d1 = d1 + (11 - i) * digitoCPF;

                //repete o procedimento incluindo o primeiro digito calculado no passo anterior.  
                d2 = d2 + (12 - i) * digitoCPF;
            };

            //Primeiro resto da divisão por 11.  
            resto = (d1 % 11);

            //Se o resultado for 0 ou 1 o digito é 0 caso contrário o digito é 11 menos o resultado anterior.  
            if (resto < 2)
                digito1 = 0;
            else
                digito1 = 11 - resto;

            d2 += 2 * digito1;

            //Segundo resto da divisão por 11.  
            resto = (d2 % 11);

            //Se o resultado for 0 ou 1 o digito é 0 caso contrário o digito é 11 menos o resultado anterior.  
            if (resto < 2)
                digito2 = 0;
            else
                digito2 = 11 - resto;

            //Digito verificador do CPF que está sendo validado.  
            String nDigVerific = cpf.Substring(cpf.Length - 2, 2);

            //Concatenando o primeiro resto com o segundo.  
            digResult = (digito1.ToString()) + (digito2.ToString());

            //comparar o digito verificador do cpf com o primeiro resto + o segundo resto.  
            return nDigVerific.Equals(digResult);
        }

        public static string RetiraMascaraCpf(string cpf)
        {
            return Regex.Replace(cpf, @"[\.\/\-]", "");
        }
    }
}