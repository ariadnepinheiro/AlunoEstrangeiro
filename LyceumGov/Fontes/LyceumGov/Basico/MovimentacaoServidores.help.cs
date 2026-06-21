using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
    public partial class MovimentacaoServidores
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Consultar e alterar movimentação de servidores.");
            help.Oper.TitleAdd("Consultando movimentação de servidores");
            help.Oper.Add("Para consultar ou alterar movimentação de servidores é necessário informar a matrícula* ou o nome do servidor e pressionar a tecla Tab para a procura do mesmo.");
            help.Oper.Add("Ao clicar no botão ?I ao lado da grade de pesquisa de servidores, alguns servidores existentes são apresentadas em uma lista suspensa.", "~/Images/bt_drop.png");
            help.Oper.Add("A pesquisa pode ser filtrada por Matrícula/Código/Nome/Documento/CPF. Após definidas estas informações, deve-se clicar no botão ?I para filtrar os resultados.", "~/images/TSearchFind.png");
	        help.Oper.Add("* A matrícula do servidor tem que ser exata.");	

            help.Oper.TitleAdd("Alterando movimentação de servidores");
            help.Oper.Add("Para alterar os dados de uma movimentação de servidores deve-se clicar no botão ?I.", "~/img/bt_editar.png");
            help.Oper.Add("Deve-se selecionar a Unidade Administrativa* desejada para efetuar a alteração.");
            help.Oper.Add("Para salvar os dados da nova movimentação de servidores deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
            help.Oper.Add("Para cancelar a inclusão dos dados da movimentação de servidores deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");
            help.Oper.Add("* A UA tem que ser da mesma coordenadoria em que o servidor pertence.");
	    
	        help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.TitleAdd("• Área de Pesquisa");
            help.Oper.Add("• Matrícula: Número de matrícula do servidor.");
            help.Oper.Add("• Nome do Servidor: Nome do servidor.");

            help.Oper.TitleAdd("• Grade Movimentação de Servidores");
            help.Oper.Add("• Ordem: Campo de controle do sistema (é gerado automaticamente).");
            help.Oper.Add("• Matrícula: Matrícula do servidor.");
            help.Oper.Add("• Função: Função exercida pela lotação.");
            help.Oper.Add("• Tipo de Lotação: Tipo de lotação atribuído ao servidor.");
            help.Oper.Add("• U.A.: Unidade Administrativa da lotação do servidor.");
            help.Oper.Add("• Coordenadoria: Coordenadoria referente à lotação.");
            help.Oper.Add("• Unidade de Ensino: Nome da Unidade de Ensino.");
            help.Oper.Add("• Turno: Turno do servidor.");
            help.Oper.Add("• Data de Admissão: Data de admissão do servidor.");
            help.Oper.Add("• Data da Publicação da Admissão: Data de publicação no diário oficial da admissão do servidor.");
            help.Oper.Add("• Data da Exoneração: Data da exoneração do servidor.");
            help.Oper.Add("• Documentação: Indica se o servidor é responsável por documentação.");
            help.Oper.Add("• Data da Publicação da Exoneração: Data de publicação no diário oficial da exoneração do servidor.");
            help.Oper.Add("• Ato Oficial: Ato oficial da nomeação do servidor.");

            help.Oper.TitleAdd("Botões");
	        help.Oper.Add("?I: Altera a linha.", "~/img/bt_editar.png");
            help.Oper.Add("?I: Salva a inserção/alteração da linha.", "~/img/bt_salvar.png");
            help.Oper.Add("?I: Cancela a inserção/alteração da nova linha.", "~/img/bt_cancelar.png");
            help.Oper.Add("?I: Busca registro usando os filtros.", "~/images/TSearchFind.png");
            help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
        }
    }
}
