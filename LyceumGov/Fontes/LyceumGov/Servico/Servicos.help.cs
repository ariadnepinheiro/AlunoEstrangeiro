using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Servico
{
    public partial class Servicos
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Cadastro dos serviços a serem utilizados no sistema. A página também permite o cadastro do fluxo de andamento deste serviço. O fluxo indica os grupos de padrões de acesso que deverão dar andamento ao serviço.");
            help.Summary.Add("Consultar, cadastrar, alterar e remover serviços e seus fluxos de andamento.");
           
            help.Oper.TitleAdd("Consultando serviços e seus fluxos de andamento");
            help.Oper.Add("Ao acessar a página de serviços, todos os serviços cadastradas serão exibidos na grade de serviços e o primeiro registro é automaticamente selecionado de forma a exibir todos os fluxos de andamento associados a ele na grade de fluxos de andamento.");
            help.Oper.Add("Para consultar os fluxos de andamento associados a outro serviço deve-se selecionar a serviço desejado clicando na linha em que o serviço aparece na grade de serviços.");
            help.Oper.Add("Os dados dos fluxos de andamento da serviço selecionado serão exibidos automaticamente na grade de fluxos de andamento.");
            help.Oper.Add("Para filtrar a consulta utilize a linha de filtro acima dos registros. Também é possível definir o tipo de filtro clicando no botão ?I.", "~/img/bt_filtro.jpg");
            help.Oper.Add("Para limpar o filtro deve-se clicar no botão ?I.", "~/img/bt_limpa.png");

            help.Oper.TitleAdd("Cadastrando novo serviço");
            help.Oper.Add("Para cadastrar um novo serviço deve-se clicar no botão ?I da grade de serviços.", "~/img/bt_novo.png");
            help.Oper.Add("Deve-se preencher os campos com os dados do novo serviço.");
            help.Oper.Add("Para salvar os dados do novo serviço deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
            help.Oper.Add("Para cancelar a inclusão dos dados do serviço deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

            help.Oper.TitleAdd("Alterando serviço");
            help.Oper.Add("Para alterar os dados de um serviço deve-se clicar no botão ?I da grade de serviços.", "~/img/bt_editar.png");
            help.Oper.Add("Os dados do serviço serão carregados permitindo alterações nos campos.");
            help.Oper.Add("Para salvar os dados do serviço deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
            help.Oper.Add("Para cancelar a alteração nos dados do serviço deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

            help.Oper.TitleAdd("Removendo serviço");
            help.Oper.Add("Para remover um serviço deve-se clicar no botão ?I da grade de serviços e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

            help.Oper.TitleAdd("Cadastrando novo fluxo de andamento");
            help.Oper.Add("Para cadastrar um novo fluxo deve-se selecionar o serviço desejado. Ver 'Consultando serviços e seus fluxos de andamento'.");
            help.Oper.Add("Para cadastrar um fluxo associado à serviço selecionada deve-se clicar no botão ?I da grade de fluxos de andamento.", "~/img/bt_novo.png");
            help.Oper.Add("Deve-se preencher os campos com os dados do novo fluxo.");
            help.Oper.Add("Para salvar os dados do novo fluxo associado à serviço selecionada deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
            help.Oper.Add("Para cancelar a inclusão dos dados do novo fluxo na serviço selecionada deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

            help.Oper.TitleAdd("Alterando fluxo de andamento");
            help.Oper.Add("Para alterar um fluxo deve-se selecionar a serviço desejada. Ver 'Consultando serviços e seus fluxos de andamento'.");
            help.Oper.Add("Para alterar os dados de um fluxo associado ao serviço selecionado deve-se clicar no botão ?I da grade de fluxos de andamento.", "~/img/bt_editar.png");
            help.Oper.Add("Os dados do fluxo serão carregados permitindo alterações nos campos.");
            help.Oper.Add("Para salvar os dados do fluxo associado ao serviço selecionado deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
            help.Oper.Add("Para cancelar a alteração nos dados do fluxo associado ao serviço selecionado deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

            help.Oper.TitleAdd("Removendo fluxo de andamento");
            help.Oper.Add("Para remover um fluxo deve-se selecionar o serviço desejado. Ver 'Consultando serviços e seus fluxos de andamento'.");
            help.Oper.Add("Para remover os dados de um fluxo associado ao serviço selecionado deve-se clicar no botão ?I da grade de fluxos de andamento e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.TitleAdd("Grade de Serviços");
            help.Oper.Add("• Serviço: Código do serviço.");
            help.Oper.Add("• Descrição: Descrição do serviço.");


            help.Oper.TitleAdd("Grade de Fluxos de Andamento");
            help.Oper.Add("• Passo: Número do passo no andamento do serviço.");
            help.Oper.Add("• Padrão de acesso: Padrão de acesso que poderá dar andamento ao serviço");
            help.Oper.Add("• Descrição: Descrição do passo.");


            help.Oper.TitleAdd("Botões");
            help.Oper.Add("?I: Insere uma nova linha.", "~/img/bt_novo.png");
            help.Oper.Add("?I: Salva a inserção/alteração da linha.", "~/img/bt_salvar.png");
            help.Oper.Add("?I: Cancela a inserção/alteração da nova linha.", "~/img/bt_cancelar.png");
            help.Oper.Add("?I: Permite alteração na linha.", "~/img/bt_editar.png");
            help.Oper.Add("?I: Remove a linha.", "~/img/bt_exclui2.png");
            help.Oper.Add("?I: Limpa os filtros selecionados.", "~/img/bt_Limpa.png");
            help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
        }
    }
}
