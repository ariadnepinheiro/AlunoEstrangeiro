using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
    public partial class RestricaoAcessoUsuarios
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Consultar, cadastrar e remover unidades físicas da instituição a que o usuário tem acesso.");
            help.Oper.Add("Para consultar, cadastrar, alterar ou remover uma unidade física é necessário fazer uma pesquisa pelo usuário de interesse clicando no botão ?I.", "~/Images/bt_drop.png");
            help.Oper.Add("Todos os usuários existentes são apresentados em uma lista suspensa.");
            help.Oper.Add("A pesquisa pode ser filtrada pelo login e/ou nome do usuário. Após definidas estas informações, o resultado é apresentado automaticamente na lista suspensa.");
            help.Oper.Add("Deve-se selecionar o usuário de interesse clicando na linha em que o usuário aparece na lista suspensa.");
            help.Oper.Add("Obs.: Nesta pesquisa, utilize o caracter '%' para aplicar um filtro cujo valor está contido no campo pesquisado e não apenas quando estiver no início, conforme o exemplo abaixo:");
            help.Oper.Add("Exemplo: Para filtrar a coluna 'Nome' para que só sejam exibidos registros contendo a palavra 'Paula', digite %Paula na coluna 'Nome'.");

            help.Oper.TitleAdd("Consultando unidades físicas");
            help.Oper.Add("A consulta é realizada automaticamente quando o usuário de interesse for selecionado.");
            help.Oper.Add("Para filtrar a consulta utilize a linha de filtro acima dos registros. Também é possível definir o tipo de filtro clicando no botão ?I.", "~/img/bt_filtro.jpg");
            help.Oper.Add("Para limpar o filtro deve-se clicar no botão ?I.", "~/img/bt_limpa.png");

            help.Oper.TitleAdd("Cadastrando nova unidade física");
            help.Oper.Add("Para cadastrar uma nova unidade física é necessário fazer a consulta do usuário desejado. Ver 'Consultando unidades físicas'.");
            help.Oper.Add("Para cadastrar uma nova unidade física associada ao aluno selecionado deve-se clicar no botão ?I.", "~/img/bt_novo.png");
            help.Oper.Add("Deve-se pesquisar a unidade física desejada clicando no botão ?I localizado na grade de pesquisa de unidades físicas. Todas as unidades físicas existentes são apresentadas em uma lista suspensa.");
            help.Oper.Add("Para facilitar a pesquisa, utilize os filtros de código e/ou nome da unidade física localizados na lista suspensa. Após definidos valores aos filtros, o resultado da pesquisa é apresentado nesta lista automaticamente.");
            help.Oper.Add("Obs.: Nesta pesquisa, utilize o caracter '%' para aplicar um filtro cujo valor está contido no campo pesquisado e não apenas quando estiver no início, conforme o exemplo abaixo:");
            help.Oper.Add("Exemplo: Para filtrar a coluna 'Nome' para que só sejam exibidos registros contendo a palavra 'Carlos', digite %Carlos na coluna 'Nome'.");
            help.Oper.Add("Deve-se selecionar a unidade física desejada clicando na linha em que a unidade física aparece na lista suspensa.");
            help.Oper.Add("Para salvar os dados da nova unidade física associada ao usuário selecionado deve-se clicar no botão ?I.", "~/img/bt_salvar.png");
            help.Oper.Add("Para cancelar a inclusão da nova unidade física deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

            help.Oper.TitleAdd("Cadastrando todas unidades físicas");
            help.Oper.Add("Para associar ao aluno todas as unidades físicas cadastradas no sistema deve-se clicar no botão \"" + "Incluir todas Unidades" + "\".");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante os cadastros.");

            help.Oper.TitleAdd("Removendo unidade física");
            help.Oper.Add("Para remover uma unidade física é necessário fazer a consulta do usuário desejado. Ver 'Consultando unidades físicas'.");
            help.Oper.Add("Para remover uma unidade física associada ao aluno selecionado deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

            help.Oper.TitleAdd("Removendo todas as unidades físicas");
            help.Oper.Add("Para remover todas unidades físicas associadas ao aluno deve-se clicar no botão \"" + "Remover todas Unidades" + "\" e confirmar a remoção dos registros.");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

            help.Oper.TitleAdd("Cadastrando todas unidades físicas de uma Coordenadoria");
            help.Oper.Add("Para inserir todas as unidades físicas de uma Coordenadoria para um usuário deve-se selecionar a Coordenadoria desejada e clicar em “Incluir”.");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

            help.Oper.TitleAdd("Removendo todas as unidades físicas de uma Coordenadoria");
            help.Oper.Add("Para remover todas as unidades físicas de uma Coordenadoria associadas a um usuário deve-se selecionar a Coordenadoria desejada e clicar em “Incluir”.");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.Add("• Unidade Física: Unidade física a que o usuário tem acesso.");

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
