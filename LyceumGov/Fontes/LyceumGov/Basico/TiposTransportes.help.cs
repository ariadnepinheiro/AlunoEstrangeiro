using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
    public partial class TiposTransportes
    {
        public override void HelpInit(HelpData help)
        {
            help.Summary.Add("Consultar, cadastrar, alterar e remover tipos de transportes.");

            help.Oper.TitleAdd("Consultando tipos de transportes");
            help.Oper.Add("Ao carregar a página todos os tipos de transportes são carregadas com a mesma.");
            help.Oper.Add("Para filtrar a consulta utilize a linha de filtro acima dos registros. Também é possível definir o tipo de filtro clicando no botão ?I.", "~/img/bt_filtro.jpg");
            help.Oper.Add("Para limpar o filtro deve-se clicar no botão ?I.", "~/img/bt_limpa.png");

            help.Oper.TitleAdd("Cadastrando novo tipo de transporte");
            help.Oper.Add("Para cadastrar um novo tipo de transporte clique no botão ?I.", "~/img/bt_novo.png");
            help.Oper.Add("Preencha os dados obrigatórios e clique no botão ?I para confirmar o cadastro. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
            help.Oper.Add("Caso queira cancelar o cadastro do novo tipo de transporte, clique no botão ?I.", "~/img/bt_cancelar.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

            help.Oper.TitleAdd("Alterando tipo de transporte");
            help.Oper.Add("Para alterar o tipo de transporte clique no botão ?I.", "~/img/bt_editar.png");
            help.Oper.Add("Faça as alterações necessárias e clique no botão ?I para confirmar a alteração. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
            help.Oper.Add("Caso queira cancelar a alteração do tipo de transporte, clique no botão ?I.", "~/img/bt_cancelar.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

            help.Oper.TitleAdd("Removendo tipo de transporte");
            help.Oper.Add("Para remover o tipo de transporte clique no botão ?I. e confirme a remoção.", "~/img/bt_exclui2.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.Add("• Tipo: Tipo de transportes.");
            help.Oper.Add("• Descrição: Nome descritivo do tipo de transporte.");

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

