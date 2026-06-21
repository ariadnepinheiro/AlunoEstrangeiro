using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.ProcessoSeletivo
{
    public partial class Experiencias
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Consultar, cadastrar, alterar e remover experiências para processos seletivos.");

            help.Oper.TitleAdd("Consultando experiência para processo seletivo");
            help.Oper.Add("Ao acessar a página de experiências para processos seletivos, todas os registros cadastrados serão exibidos.");
            help.Oper.Add("Para filtrar a consulta utilize a linha de filtro acima dos registros. Também é possível definir o tipo de filtro clicando no botão ?I.", "~/img/bt_filtro.jpg");
            help.Oper.Add("Para limpar o filtro deve-se clicar no botão ?I.", "~/img/bt_limpa.png");

            help.Oper.TitleAdd("Cadastrando nova experiência para processo seletivo");
            help.Oper.Add("Para cadastrar uma nova experiência para processo seletivo deve-se clicar no botão ?I.", "~/img/bt_novo.png");
            help.Oper.Add("Deve-se preencher os campos com os dados da nova experiência para processo seletivo.");
            help.Oper.Add("Para salvar os dados da nova experiência para processo seletivo deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
            help.Oper.Add("Para cancelar a inclusão dos dados da experiência para processo seletivo deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

            help.Oper.TitleAdd("Alterando experiência para processo seletivo");
            help.Oper.Add("Para alterar os dados de uma experiência para processo seletivo deve-se clicar no botão ?I.", "~/img/bt_editar.png");
            help.Oper.Add("Os dados da experiência para processo seletivo serão carregados permitindo alterações nos campos.");
            help.Oper.Add("Para salvar os dados da experiência para processo seletivo deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
            help.Oper.Add("Para cancelar a alteração nos dados da experiência para processo seletivo deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

            help.Oper.TitleAdd("Removendo experiência para processo seletivo");
            help.Oper.Add("Para remover uma experiência para processo seletivo deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/img/bt_exclui2.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.Add("• Experiência: Código da experiência.");
            help.Oper.Add("• Descrição: Nome da experiência.");
            help.Oper.Add("• Origem: Indica se a experiência é dentro ou fora da Seeduc-RJ. (Valores fixos: Dentro da Seeduc-RJ e Fora da Seeduc-RJ).");

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
