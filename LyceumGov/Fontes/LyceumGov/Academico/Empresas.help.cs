using Techne.Web;

namespace Techne.Lyceum.Net.Academico
{
    public partial class Empresas
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar, cadastrar, alterar e remover empresas.");

            help.Oper.TitleAdd("Consultar Empresa");
            help.Oper.Add("Para consultar uma empresa é necessário fazer uma pesquisa pela empresa de interesse clicando no botão ?I.", 
                "~/Images/bt_drop.png");
            help.Oper.Add("A pesquisa pode ser feita pelo código e razão social da empresa.");
            help.Oper.Add("O resultado da pesquisa aparecerá em uma lista suspensa. Deve-se selecionar a empresa de interesse.");
            help.Oper.Add("Os dados da empresa serão visualizados assim que a empresa desejada for selecionada.");

            help.Oper.TitleAdd("Cadastrar Empresa");
            help.Oper.Add("Para cadastrar uma nova empresa deve-se clicar no botão ?T.", btnNovo);
            help.Oper.Add("Deve-se preencher os campos com os dados da nova empresa.");
            help.Oper.Add("Para salvar os dados da nova empresa deve-se clicar no botão ?T. " +
                "Se algum campo obrigatório não estiver preenchido será exibido um alerta.", btnSalvar);
            help.Oper.Add("Para cancelar a inclusão dos dados da empresa deve-se clicar no botão ?T.", btnCancelar);
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros " +
                " ocorridos durante o cadastro.");

            help.Oper.TitleAdd("Alterar Empresa");
            help.Oper.Add("Para alterar uma empresa é necessário fazer a consulta da empresa desejada. Ver 'Consultar Empresa'.");
            help.Oper.Add("Para alterar os dados de uma empresa deve-se clicar no botão ?T.", btnAlterar);
            help.Oper.Add("Os dados da empresa serão carregados permitindo alteração nos campos.");
            help.Oper.Add("Para salvar os dados da empresa deve-se clicar no botão ?T. Se algum campo obrigatório não estiver " +
                "preenchido será exibido um alerta.", btnSalvar);
            help.Oper.Add("Para cancelar a alteração nos dados da empresa deve-se clicar no botão ?T.", btnCancelar);
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros " +
                "ocorridos durante a alteração.");

            help.Oper.TitleAdd("Remover Empresa");
            help.Oper.Add("Para remover uma empresa é necessário fazer a consulta da empresa desejada. Ver 'Consultar Empresa'.");
            help.Oper.Add("Para remover uma empresa deve-se clicar no botão ?T e confirmar a remoção do registro.", btnExcluir);
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros " +
                "ocorridos durante a remoção.");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.Add("• Empresa: Código da empresa.");
            help.Oper.Add("• Razão Social: Razão Social da empresa.");
            help.Oper.Add("• Nome: Nome da empresa.");
            help.Oper.Add("• CEP: CEP da empresa.");
            help.Oper.Add("• Município: Município da empresa.");
            help.Oper.Add("• UF: UF da empresa.");
            help.Oper.Add("• Endereço: Endereço da empresa.");
            help.Oper.Add("• Número: Número da empresa.");
            help.Oper.Add("• Complemento: Complemento do endereço da empresa.");
            help.Oper.Add("• Bairro: Bairro da empresa.");
            help.Oper.Add("• Inscrição Municipal: Inscrição Municipal da empresa.");
            help.Oper.Add("• Inscrição Estadual: Inscrição Estadual da empresa.");
            help.Oper.Add("• Porte: Porte da empresa.");
            help.Oper.Add("• Ramo: Ramo da empresa.");
            help.Oper.Add("• Atividade: Atividade da empresa.");
            help.Oper.Add("• Número de Empregados: Número de empregados da empresa.");
            help.Oper.Add("• Tipo de Capital: Tipo de capital utilizado pela empresa.");
            
            help.Oper.TitleAdd("Botões");
            help.Oper.Add("?T: Carrega um formulário para novo registro.", btnNovo);
            help.Oper.Add("?T: Salva as alterações do registro.", btnSalvar);
            help.Oper.Add("?T: Cancela a operação corrente.", btnCancelar);
            help.Oper.Add("?T: Permite alteração no registro.", btnAlterar);
            help.Oper.Add("?T: Remove o registro.", btnExcluir);
            help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
        }
    }
}
