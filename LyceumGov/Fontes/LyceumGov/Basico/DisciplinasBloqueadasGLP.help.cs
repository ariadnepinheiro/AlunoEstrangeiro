using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
    public partial class DisciplinasBloqueadasGLP
    {
        public override void HelpInit(HelpData help)
        {
            help.Summary.Add("Consultar, cadastrar, e remover disciplinas bloqueadas de GLP.");

            help.Oper.TitleAdd("Consultando disciplinas bloqueadas de GLP");
            help.Oper.Add("Ao carregar a página todas as disciplinas bloqueadas de GLP são carregadas com a mesma.");
            help.Oper.Add("Para filtrar a consulta utilize a linha de filtro acima dos registros. Também é possível definir o tipo de filtro clicando no botão ?I.", "~/img/bt_filtro.jpg");
            help.Oper.Add("Para limpar o filtro deve-se clicar no botão ?I.", "~/img/bt_limpa.png");

            help.Oper.TitleAdd("Cadastrando nova disciplina bloqueada de GLP");
            help.Oper.Add("Para cadastrar uma nova disciplina bloqueada de GLP clique no botão ?I.", "~/img/bt_novo.png");
            help.Oper.Add("Preencha os dados obrigatórios e clique no botão ?I para confirmar o cadastro. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
            help.Oper.Add("Caso queira cancelar o cadastro da nova disciplina bloqueada de GLP, clique no botão ?I.", "~/img/bt_cancelar.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

            help.Oper.TitleAdd("Removendo disciplina bloqueada de GLP");
            help.Oper.Add("Para remover uma disciplina bloqueada de GLP clique no botão ?I. e confirme a remoção.", "~/img/bt_exclui2.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.TitleAdd("• Grade: Disciplinas Bloqueadas de GLP");
            help.Oper.Add("• Grupos de Disciplinas: Nome do grupo de disciplinas bloqueadas.");
            help.Oper.Add("• Data Início: Data de início do bloqueio da disciplina.");
            help.Oper.Add("• Data Final: Data do fim do bloqueio da disciplina.");

            help.Oper.TitleAdd("Botões");
            help.Oper.Add("?I: Insere uma nova linha.", "~/img/bt_novo.png");
            help.Oper.Add("?I: Salva a inserção da linha.", "~/img/bt_salvar.png");
            help.Oper.Add("?I: Cancela a inserção/alteração da nova linha.", "~/img/bt_cancelar.png");
            help.Oper.Add("?I: Remove a linha.", "~/img/bt_exclui2.png");
            help.Oper.Add("?I: Limpa os filtros selecionados.", "~/img/bt_Limpa.png");
            help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
        }
    }
}
