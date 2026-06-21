using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Servico
{
    public partial class SolicitacaoServicos
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Cadastro das solicitações de serviços. A página também permite dar andamento nos serviços já cadastrados. Somente os usuários autorizados poderão dar o andamento.");

            help.Oper.TitleAdd("Consultando alunos");
            help.Oper.Add("Deve-se consultar o aluno para o qual se deseja cadastrar uma nova solicitação de serviço ou consultar os serviços já solicitados.");
            help.Oper.Add("Para consultar um aluno é necessário fazer uma pesquisa pelo aluno de interesse clicando no botão ?I.", "~/Images/bt_drop.png");
            help.Oper.Add("A pesquisa pode ser feita pelo nome, matrícula, nome da mãe e/ou nome do pai do aluno. Após definidas estas informações, deve-se clicar em ?I.", "~/Images/bot_buscar.png");
            help.Oper.Add("O resultado da pesquisa aparecerá em uma lista suspensa e deve-se selecionar o aluno de interesse clicando na linha em que o aluno aparece nesta lista.");

            help.Oper.TitleAdd("Consultando solicitações de serviços e seus andamentos");
            help.Oper.Add("Ao acessar a página de solicitações de serviços, todas as solicitações cadastradas oara o aluno selecionado serão exibidas na grade de solicitações e o primeiro registro é automaticamente selecionado de forma a exibir os andamentos associados a ele na grade de andamentos.");
            help.Oper.Add("Para consultar os andamentos associados a outra solicitação deve-se selecionar a solicitação desejada clicando na linha correspondente na grade de solicitações.");
            help.Oper.Add("Os dados dos andamentos da solicitação selecionada serão exibidos automaticamente na grade de andamentos.");
            help.Oper.Add("Para filtrar a consulta utilize a linha de filtro acima dos registros. Também é possível definir o tipo de filtro clicando no botão ?I.", "~/img/bt_filtro.jpg");
            help.Oper.Add("Para limpar o filtro deve-se clicar no botão ?I.", "~/img/bt_limpa.png");

            help.Oper.TitleAdd("Cadastrando nova solicitação");
            help.Oper.Add("Para cadastrar uma nova solicitação deve-se clicar no botão ?I da grade de solicitações.", "~/img/bt_novo.png");
            help.Oper.Add("Deve-se preencher apenas o campo 'Serviço'.");
            help.Oper.Add("Para salvar os dados da nova solicitação deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
            help.Oper.Add("Para cancelar a inclusão dos dados da solicitação deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

            help.Oper.TitleAdd("Removendo solicitação");
            help.Oper.Add("Para remover uma solicitação deve-se clicar no botão ?I da grade de solicitações e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

            help.Oper.TitleAdd("Dando andamento à solicitação");
            help.Oper.Add("Para dar andamento a uma solicitação deve-se clicar no botão 'Dar Andamento', o novo andamento aparecerá na grade de andamentos com o status 'Executado'.");
            help.Oper.Add("Caso ocorrá algum erro, este será informado.");
            help.Oper.Add("Erros comuns:");
            help.Oper.Add("• Não existir próximo passo para a solicitação selecionada, significa que já foram realizados todos os passos disponíveis para este serviço.");
            help.Oper.Add("• Já existir um cancelamento em algum passo anterior da solicitação.");

            help.Oper.TitleAdd("Cancelando andamento da solicitação");
            help.Oper.Add("Para cancelar uma solicitação deve-se clicar no botão 'Cancelar Solicitação', o novo andamento aparecerá na grade de andamentos com o status 'Cancelado'.");
            help.Oper.Add("Caso ocorrá algum erro, este será informado.");
            help.Oper.Add("Erros comuns:");
            help.Oper.Add("• Não existir próximo passo para a solicitação selecionada, significa que já foram realizados todos os passos disponíveis para este serviço.");
            help.Oper.Add("• Já existir um cancelamento em algum passo anterior da solicitação.");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.TitleAdd("Grade de Solicitações");
            help.Oper.Add("• Solicitação: Código da solicitação de serviço.");
            help.Oper.Add("• Serviço: Serviço solicitado pelo aluno. (Tabela Fixa: LY_TABELA_SERVICOS)");
            help.Oper.Add("• Motivo: Causa da solicitação. (Tabela Geral: MotivoSolicitacao)");
            help.Oper.Add("• Data: Data da solicitação do serviço.");
            help.Oper.Add("• Motivo: Descrição do motivo da solicitação do serviço.");
            help.Oper.Add("• Status: Status da solicitação do serviço.");
            help.Oper.Add("(Pode exibir os seguintes valores:");
            help.Oper.Add("\t• Concluído – significa que todos os passos do fluxo do serviço foram executados com sucesso.");
            help.Oper.Add("\t• Cancelado – significa que um dos passos do fluxo do serviço foi cancelado.");
            help.Oper.Add("\t• Pendente - significa que ainda existem passos pendentes no fluxo do serviço.)");
            
            help.Oper.TitleAdd("Grade de Andamentos");
            help.Oper.Add("• Andamento	Código do andamento.");
            help.Oper.Add("• Passo: Número do passo já executado no andamento do serviço.");
            help.Oper.Add("• Data: Data do andamento.");
            help.Oper.Add("• Status: Status do andamento.");
            help.Oper.Add("(Pode exibir os seguintes valores:");
            help.Oper.Add("\t• Executado – significa que este andamento foi executado com sucesso.");
            help.Oper.Add("\t• Cancelado - significa que este andamento foi cancelado.)");
            help.Oper.Add("• Motivo: Descrição do motivo do cancelamento da solicitação do serviço.");
            help.Oper.Add("• Padrão de acesso: Padrão de acesso que deu o andamento no serviço.");
            help.Oper.Add("• Usuário: Usuário que deu o andamento no serviço.");

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
