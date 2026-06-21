using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Entity;
using Techne.Lyceum.RN.CartaoEstudante.DTO;
using Techne.Lyceum.RN.CartaoEstudante.Query;
using System.Globalization;

namespace Techne.Lyceum.RN.CartaoEstudante.Service
{
    public class RetornoLoginService : SingletonBase<RetornoLoginService>
    {
        private static readonly RetornoLoginQuery retornoLoginQuery = RetornoLoginQuery.Instancia;
        private static readonly OperadoraQuery operadoraQuery = OperadoraQuery.Instancia;
        private static readonly AlunoQuery alunoQuery = AlunoQuery.Instancia;
        private static readonly RemessaQuery remessaQuery = RemessaQuery.Instancia;

        RetornoLoginService() { }

        public RetornoLoginDTO AtualizaLogin(string matricula, string idBeneficiario, string dataConfirmacaoAluno, string login, string codigoOperadora)
        {
            RetornoLoginDTO dto = new RetornoLoginDTO();
            try
            {
                if (!PossuiCritica(matricula, idBeneficiario, dataConfirmacaoAluno, login, codigoOperadora, out dto))
                {
                    DateTime dtDataConfirmacao = DateTime.ParseExact(dataConfirmacaoAluno, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                    RetornoLogin entidade = new RetornoLogin(int.Parse(idBeneficiario), matricula, login, dtDataConfirmacao, int.Parse(codigoOperadora));

                    retornoLoginQuery.Insere(entidade);

                    dto.Criticas.Add(new CriticaDTO { Codigo = "00" });
                }
            }
            catch(Exception)
            {
                dto.Criticas.Add(new CriticaDTO { Codigo = "01", Descricao = "Erro decorrente de exceção do sistema." });
            }

            dto.Matricula = matricula;
            dto.IdBeneficiario = idBeneficiario;
            return dto;
        }

        private bool PossuiCritica(string matricula, string idBeneficiario, string dataConfirmacaoAluno, string login, string codigoOperadora, out RetornoLoginDTO dto)
        {
            dto = new RetornoLoginDTO();
            int intTemp;
            long longTemp;
            DateTime dateTemp;

            if (!long.TryParse(matricula, out longTemp))
            {
                dto.Criticas.Add(new CriticaDTO { Codigo = "04", Descricao = "Matrícula nula ou inválida." });
            }
            else
            {
                if (!alunoQuery.ExisteAluno(matricula) || !remessaQuery.ExisteRemessaPara(matricula))
                    dto.Criticas.Add(new CriticaDTO { Codigo = "02", Descricao = "Erro na chave de acesso do sistema (matrícula não encontrada)." });
            }

            if (!int.TryParse(idBeneficiario, out intTemp))
            {
                dto.Criticas.Add(new CriticaDTO { Codigo = "05", Descricao = "Identificador de beneficiário nulo ou inválido." });
            }

            try
            {
                DateTime.TryParseExact(dataConfirmacaoAluno, "yyyyMMddHHmmss", CultureInfo.CurrentCulture, DateTimeStyles.None, out dateTemp);
                if(dateTemp == DateTime.MinValue)
                    dto.Criticas.Add(new CriticaDTO { Codigo = "06", Descricao = "Data de confirmação nula ou inválida." });

            }
            catch (ArgumentException)
            {
                dto.Criticas.Add(new CriticaDTO { Codigo = "06", Descricao = "Data de confirmação nula ou inválida." });
            }

            if (String.IsNullOrEmpty(login) || !Validacao.Email(login))
                dto.Criticas.Add(new CriticaDTO { Codigo = "07", Descricao = "Login nulo ou inválido." });

            if (String.IsNullOrEmpty(codigoOperadora) || !int.TryParse(codigoOperadora, out intTemp) || !operadoraQuery.ExisteOperadora(Convert.ToInt32(codigoOperadora)))
                dto.Criticas.Add(new CriticaDTO { Codigo = "08", Descricao = "Operadora inexistente." });

            return dto.Criticas.Count > 0;
        }

        public string ObtemLoginOperadoraPor(string aluno)
        {
            RetornoLogin retornoLogin = retornoLoginQuery.ObtemPor(aluno);

            return (retornoLogin != null) 
                ? retornoLogin.LoginOperadora 
                : string.Empty;
        }
    }
}
