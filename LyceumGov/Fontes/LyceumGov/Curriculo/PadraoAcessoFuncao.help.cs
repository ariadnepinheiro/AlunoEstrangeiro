using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Curriculo
{
    public partial class PadraoAcessoFuncao
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Consultar, cadastrar e remover funções de padrões de acesso.");

            help.Oper.Add("Esta transação permite o cadastro das funções de um padrão de acesso.");

            help.Oper.TitleAdd("Consultando funções de um padrão de acesso");
            help.Oper.Add("A consulta é realizada automaticamente quando um padrão de acesso for selecionado.");
            help.Oper.Add("Para filtrar a consulta clique no botão ?I e depois entre com o Código/Descrição do padrão de acesso e em seguida pressione a tecla Enter.", "~/Images/bt_drop.png");
            help.Oper.Add("Os dados das funções associadas ao padrão de acesso selecionado serão exibidos na grade de 'Função'.");

            help.Oper.TitleAdd("Cadastrando nova função em um padrão de acesso");
            help.Oper.Add("Para cadastrar uma nova função é necessário fazer a consulta do padrão de acesso desejado. Ver 'Consultando funções de um padrão de acesso'.");
            help.Oper.Add("Para cadastrar uma nova função no 'Função para Padrão de Acesso' selecionado deve-se clicar no botão ?I.", "~/img/bt_novo.png");
            help.Oper.Add("Deve-se selecionar a função de interesse.");
            help.Oper.Add("Para consultar uma função entre com o código/descrição da mesma e pressione a tecla Enter.");
            help.Oper.Add("Para salvar os dados da nova função no padrão de acesso selecionado deve-se clicar no botão ?I.", "~/img/bt_salvar.png");
            help.Oper.Add("Para cancelar a inclusão da nova função no padrão de acesso selecionado deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

            help.Oper.TitleAdd("Removendo função de um padrão de acesso");
            help.Oper.Add("Para remover uma função é necessário fazer a consulta do padrão de acesso desejado. Ver 'Consultando funções de um padrão de acesso'.");
            help.Oper.Add("Para remover uma função do padrão de acesso selecionado deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.TitleAdd("• Área de Pesquisa");
            help.Oper.Add("• Padrão de Acesso: Padrão de acesso para funções.");

            help.Oper.TitleAdd("• Grade de Funções");
            help.Oper.Add("• Função: Função de um padrão de acesso.");

            help.Oper.TitleAdd("Botões");
            help.Oper.Add("?I: Insere uma nova linha.", "~/img/bt_novo.png");
            help.Oper.Add("?I: Salva a inserção/alteração da linha.", "~/img/bt_salvar.png");
            help.Oper.Add("?I: Cancela a inserção/alteração da nova linha.", "~/img/bt_cancelar.png");
            help.Oper.Add("?I: Remove a linha.", "~/img/bt_exclui2.png");
            help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");       
        }
    }
}
