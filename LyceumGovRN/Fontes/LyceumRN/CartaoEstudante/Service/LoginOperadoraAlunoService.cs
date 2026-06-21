using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Techne.Lyceum.RN.CartaoEstudante.Entity;
using Techne.Lyceum.RN.CartaoEstudante.DTO;
using Techne.Lyceum.RN.CartaoEstudante.Query;
using System.Globalization;
using Techne.Lyceum.RN.CartaoEstudante.Domain;

namespace Techne.Lyceum.RN.CartaoEstudante.Service
{
    public class LoginOperadoraAlunoService : SingletonBase<LoginOperadoraAlunoService>
    {
        private static readonly LoginOperadoraAlunoQuery loginOperadoraAlunoQuery = LoginOperadoraAlunoQuery.Instancia;
        private static readonly AlunoQuery alunoQuery = AlunoQuery.Instancia;

        LoginOperadoraAlunoService() { }

        public DadosAlunoOperadoraDTO ObtemLoginOperadoraPor(string aluno)
        {                       
            LoginOperadoraAluno loginOperadoraAluno = loginOperadoraAlunoQuery.ObtemPor(aluno);
            DadosAlunoOperadoraDTO dto = ToDto(loginOperadoraAluno);
            return dto;
        }

        public DadosAlunoOperadoraDTO ToDto(LoginOperadoraAluno entity)
        {
            DadosAlunoOperadoraDTO dto = null;

            if (entity != null && entity.LoginOperadoraAlunoId > 0)
            {
                dto = new DadosAlunoOperadoraDTO();
                dto.LoginOperadora = entity.LoginOperadora;
                dto.AlunoOperadoraId = entity.IdBeneficiario;
                dto.DataAtualizacaoLogin = entity.DataAtualizacaoOperadora;
                dto.NomeOperadora = Techne.Lyceum.RN.Util.Utils.GetEnumDescription((OperadoraEnum)entity.OperadoraId);
            }

            return dto;
        }
    }
}
