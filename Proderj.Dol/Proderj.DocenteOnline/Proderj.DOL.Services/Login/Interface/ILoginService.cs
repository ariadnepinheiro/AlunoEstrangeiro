using System.Security.Principal;
using Proderj.Foundation.Architecture;
using Proderj.Foundation.Framework.Web.Seguranca;
using System.Collections.Generic;

namespace Proderj.DOL.Service
{
    public interface ILoginService : IService
    {
        DTODocenteLogado VerificaLogin(string matricula, string senha, string captcha, string idfuncional, string vinculo);
        DTODocenteLogado VerificaLoginApi(string matricula, string senha, string idfuncional, string vinculo);
    	void  AutenticaDocenteForms(DTODocenteLogado dtoDocenteLogado, short minutosParaExpirarAutenticacao);
		void DesconectaDocenteForms();
        DTODocenteLogado VerificaAlteraSenha(string matricula, string vinculo, string idFuncional, string senhaAtual, string senhaNova, string senhaNovaConfirmacao);
        string RedefineSenha(string matricula, string captcha);

        string PedidoUsuario(string cpf, string captcha);

      //  string VerificaMatricula(string vinculo, string idfuncional);
        string VerificaNumFunc(string vinculo, long idfuncional);

        List<string> VerificaIdVinculoFunc(string cpf);        
    }
}
