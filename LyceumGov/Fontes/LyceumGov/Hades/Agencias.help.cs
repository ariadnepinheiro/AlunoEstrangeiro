using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Hades
{
    public partial class Agencias
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Consultar, cadastrar, alterar e remover agências bancárias.");

            help.Oper.TitleAdd("Informando dados de pesquisa");
            help.Oper.Add("Para consulta as agências bancárias é necessário primeiramente selecionar o banco desejado.");
            help.Oper.Add("Ao clicar no botão ?I ao lado da grade de pesquisa de bancos, todas os bancos existentes são apresentados em uma lista suspensa.", "~/Images/bt_drop.png");
            help.Oper.Add("A pesquisa pode ser filtrada pelo banco ou nome do banco. Após definidas estas informações, deve-se pressionar a tecla ENTER ou clicar no botão ?I para filtrar os resultados.", "~/Images/bot_buscar.png");
            help.Oper.Add("Deve-se selecionar o banco de interesse clicando na linha em que o banco aparece na lista suspensa.");
            help.Oper.Add("Obs.: Na grade de pesquisa de bancos, utilize os caracteres '%' ou '*' para aplicar um filtro cujo valor está contido no campo pesquisado e não apenas quando estiver no início, conforme o exemplo abaixo:");
            help.Oper.Add("Exemplo: Para filtrar a coluna 'Nome' da grade de pesquisa de bancos para que só sejam exibidos registros contendo a palavra 'Bancos', digite %Bancos ou *Bancos na coluna 'Nome' e pressione a tecla ENTER ou clique no botão ?I.", "~/Images/bot_buscar.png");

            help.Oper.TitleAdd("Consultando agências");
            help.Oper.Add("Vide 'Informando dados de pesquisa' para selecionar o banco desejado.");
            help.Oper.Add("A grade de Agências será carregada na tela com as agências referentes ao banco selecionado anteriormente.");
            help.Oper.Add("Para filtrar a consulta utilize a linha de filtro acima dos registros. Também é possível definir o tipo de filtro clicando no botão ?I.", "~/img/bt_filtro.jpg");
            help.Oper.Add("Para limpar o filtro deve-se clicar no botão ?I.", "~/img/bt_limpa.png");

            help.Oper.TitleAdd("Cadastrando nova agência");
            help.Oper.Add("Para cadastrar uma nova agência deve-se clicar no botão ?I.", "~/img/bt_novo.png");
            help.Oper.Add("Deve-se preencher os campos com os dados da nova agência.");
            help.Oper.Add("Para salvar os dados da nova agência deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
            help.Oper.Add("Para cancelar a inclusão dos dados da agência deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

            help.Oper.TitleAdd("Alterando agência");
            help.Oper.Add("Para alterar os dados de uma agência deve-se clicar no botão ?I.", "~/img/bt_editar.png");
            help.Oper.Add("Os dados da agência serão carregados permitindo alterações nos campos.");
            help.Oper.Add("Para salvar os dados da agência deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
            help.Oper.Add("Para cancelar a alteração nos dados da agência deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

            help.Oper.TitleAdd("Removendo agência");
            help.Oper.Add("Para remover uma agência deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.TitleAdd("• Dados de Pesquisa");
            help.Oper.Add("• Banco: Código de identificação do banco.");
            help.Oper.Add("• Nome: Nome do banco.");

            help.Oper.Add("• Número: Código de identificação da agência.");
            help.Oper.Add("• Nome: Nome da agência.");
            help.Oper.Add("• Município: Município aonde se localiza a agência.");
            help.Oper.Add("• UF: Estado aonde se localiza a agência.");
            help.Oper.Add("• Endereço: Endereço comercial da agência.");
            help.Oper.Add("• Contato: Nome do contato da agência.");
            help.Oper.Add("• Cargo: Cargo do contato da agência.");
            help.Oper.Add("• Telefone: Número do telefone da agência.");
            help.Oper.Add("• CEP: Número do CEP aonde se localiza a agência.");

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
