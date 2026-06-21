using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Curriculo
{
	public partial class Servidores
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Esta transação permite:");
			help.Summary.Add("1 - Alterar o telefone do servidor/docente.");
			help.Summary.Add("2 - Alterar o celular do servidor/docente.");
			help.Summary.Add("3 - Exibir lotação ativa do servidor/docente em coordenadoria e/ou unidade.");
			help.Summary.Add("4 - Exibir última lotação ativa do servidor em uma coordenadoria.");
			help.Summary.Add("5 - Readaptar servidor/docente na mesma função.");
			help.Summary.Add("6 - Readaptar servidor/docente em nova função.");
			help.Summary.Add("7 - Alterar a situação do servidor/docente.");
			help.Summary.Add("8 - Alterar a função do servidor/docente.");
			help.Summary.Add("9 - Excluir a função do servidor/docente.");
			help.Summary.Add("10 - Consultar aulas alocadas do docente.");
			help.Summary.Add("11 - Consultar se o docente é regente.");

			help.Oper.TitleAdd("Informações de Pesquisa");
            help.Oper.Add("Para consultar ou alterar um docente/servidor é necessário fazer uma pesquisa por coordenadoria.");
            help.Oper.Add("É possível opcionalmente selecionar um município e/ou uma unidade de ensino para filtrar os registros apresentados na grade de servidores.");
            help.Oper.Add("Para selecionar uma coordenadoria, e/ou um município, e/ou uma unidade de ensino, deve-se clicar no botão ?I ao lado de sua respectiva grade de pesquisa.", "~/Images/bt_drop.png");

            help.Oper.TitleAdd("Informações de Pesquisa para Coordenadoria");
            help.Oper.Add("Ao clicar no botão ?I ao lado da grade de pesquisa de coordenadorias, todas as coordenadorias existentes são apresentadas em uma lista suspensa.", "~/Images/bt_drop.png");
            help.Oper.Add("A pesquisa pode ser filtrada pelo código e/ou descrição da coordenadoria. Após definidas estas informações, o resultado é apresentado automaticamente na lista suspensa.");
            help.Oper.Add("Deve-se selecionar a coordenadoria de interesse clicando na linha em que a coordenadoria aparece na lista suspensa. Após selecionada, deve-se clicar no botão ?I para que os registros sejam apresentados na grade de servidores.", "~/Images/bot_buscar.png");

            help.Oper.TitleAdd("Informações de Pesquisa para Município");
            help.Oper.Add("Ao clicar no botão ?I ao lado da grade de pesquisa de município, todos os municípios existentes são apresentados em uma lista suspensa.", "~/Images/bt_drop.png");
            help.Oper.Add("A pesquisa pode ser filtrada pelo código, nome e/ou estado do município. Após definidas estas informações, o resultado é apresentado automaticamente na lista suspensa.");
            help.Oper.Add("Deve-se selecionar o município de interesse clicando na linha em que o município aparece na lista suspensa. Após selecionada, deve-se clicar no botão ?I para que a coordenadoria referente ao município selecionado seja carregada automaticamente, caso não estivesse selecionada previamente.", "~/Images/bot_buscar.png");

            help.Oper.TitleAdd("Informações de Pesquisa para Unidade de Ensino");
            help.Oper.Add("Ao clicar no botão ?I ao lado da grade de pesquisa de unidades de ensino, todas as unidades de ensino existentes são apresentadas em uma lista suspensa.", "~/Images/bt_drop.png");
            help.Oper.Add("A pesquisa pode ser filtrada pelo nome e/ou descrição da unidade de ensino. Após definidas estas informações, o resultado é apresentado automaticamente na lista suspensa.");
            help.Oper.Add("Deve-se selecionar a unidade de ensino de interesse clicando na linha em que a unidade de ensino aparece na lista suspensa. Após selecionada, deve-se clicar no botão ?I para que a unidade de ensino selecionada seja aplicada como filtro à grade de servidores.", "~/Images/bot_buscar.png");

            help.Oper.Add("Obs.: Em quaisquer destas pesquisas, utilize o caracter '%' para aplicar um filtro cujo valor está contido no campo pesquisado e não apenas quando estiver no início, conforme o exemplo abaixo:");
            help.Oper.Add("Exemplo: Para filtrar a coluna 'Coordenadoria' da grade de pesquisa de coordenadorias para que só sejam exibidos registros contendo a palavra 'Centro', digite %Centro na coluna 'Coordenadoria'.");

			help.Oper.TitleAdd("Consultando um docente/servidor");
			help.Oper.Add("A consulta é realizada automaticamente quando a coordenadoria de interesse for selecionada.");
			help.Oper.Add("Os dados do docente/servidor serão exibidos na grade de servidores.");
			help.Oper.Add("Para filtrar a consulta utilize a linha de filtro acima dos registros. Também é possível definir o tipo de filtro clicando no botão ?I.", "~/img/bt_filtro.jpg");
			help.Oper.Add("Para limpar o filtro deve-se clicar no botão ?I.", "~/img/bt_limpa.png");

			help.Oper.TitleAdd("Alterando docente/servidor");
			help.Oper.Add("Para alterar os dados de um docente/servidor deve-se clicar no botão ?I.", "~/img/bt_editar.png");
			help.Oper.Add("Os dados do docente/servidor serão carregados permitindo alteração nos campos.");
			help.Oper.Add("Para salvar os dados do docente/servidor deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/img/bt_salvar.png");
			help.Oper.Add("Para cancelar a alteração nos dados do docente/servidor deve-se clicar no botão ?I.", "~/img/bt_cancelar.png");
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Descrição das Funcionalidades");
            help.Oper.TitleAdd("1 - Alterar o telefone do servidor/docente:");
            help.Oper.Add("• Para realizar esta ação, é necessário fazer a consulta pelo docente/servidor de interesse. Ver 'Consultando um docente/servidor'.");
            help.Oper.Add("• Altera o teltefone cadastrado na transação Servidores, caso o telefone não exista, é inserido um novo.");
            help.Oper.TitleAdd("2 - Alterar o celular do servidor/docente:");
            help.Oper.Add("• Para realizar esta ação, é necessário fazer a consulta pelo docente/servidor de interesse. Ver 'Consultando um docente/servidor'.");
            help.Oper.Add("• Altera o celular cadastrado na transação Servidores, caso o celular não exista, é inserido um novo.");
            help.Oper.TitleAdd("3 - Exibir lotação ativa do docente em coordenadoria e/ou unidade.");
			help.Oper.Add("• Para realizar esta ação, é necessário fazer a consulta pelo docente de interesse. Ver 'Consultando um docente/servidor'.");
			help.Oper.Add("• Todos os docentes exibidos na grade de servidores possuem lotação ativa.");
            help.Oper.TitleAdd("4 - Exibir última lotação ativa do servidor em uma coordenadoria.");
			help.Oper.Add("• Para realizar esta ação, é necessário fazer a consulta pelo servidor de interesse. Ver 'Consultando um docente/servidor'.");
			help.Oper.Add("• Em todos os servidores, será exibida sua última lotação ativa que pode ser nula. Neste caso, é possível adicionar uma nova função para este servidor.");
            help.Oper.TitleAdd("5 - Readaptar servidor/docente na mesma função:");
			help.Oper.Add("• Na consulta, é possível verificar se o docente/servidor é readaptado ou não.");
			help.Oper.Add("• Na edição, é possível readaptar o docente/servidor, selecionando a opção correspondente à 'Readaptado'. Neste caso, é obrigatório selecionar data de início e data de fim da readaptação." );
			help.Oper.Add("• Obs.: Não é possível readaptar docentes com aulas alocadas, nem servidores sem função ativa.");
            help.Oper.TitleAdd("6 - Readaptar servidor/docente em nova função:");
			help.Oper.Add("• Mesmo procedimento do item 3. A única diferença é que o docente/servidor pode ser readaptado para uma nova função que deve ser selecionada.");
            help.Oper.TitleAdd("7 - Alterar a situação do servidor/docente:");
			help.Oper.Add("• Na edição, é possível alterar a situação do servidor/docente. Neste caso, a data de início é obrigatória e a data de fim depende da situação selecionada.");
			help.Oper.Add("• Obs.: Não é possível alterar a situação depois de adicionada. É possível apenas modificar sua data de fim.");
			help.Oper.Add("• Obs.: Não é possível alterar a situação do docente/servidor caso ele tenha aulas alocadas no período.");
            help.Oper.TitleAdd("8 - Alterar a função do servidor/docente:");
			help.Oper.Add("• Na edição, é possível alterar a função do servidor/docente.");
			help.Oper.Add("• Obs.: Exceção para docentes que não possuem aulas alocadas e docentes/servidores cuja função sela diretor ou secretária.");
            help.Oper.TitleAdd("9 - Excluir a função do servidor/ docente:");
			help.Oper.Add("• No caso de docentes, a função ativa é excluída e substituída pela função original cadastrada durante a inclusão do docente.");
			help.Oper.Add("• No caso de servidores, a função ativa é excluída apenas. O registro do servidor permanecerá na grade de servidores para que seja possível cadastrar nova função.");
            help.Oper.TitleAdd("10 - Consultar aulas alocadas do docente:");
			help.Oper.Add("• Para realizar esta ação, é necessário fazer a consulta pelo docente de interesse. Ver 'Consultando um docente/servidor'.");
			help.Oper.Add("• Na consulta, é possível verificar as aulas alocadas para o docente, incluindo as de GLP.");
            help.Oper.TitleAdd("11 - Consultar se o docente é regente:");
			help.Oper.Add("• Para realizar esta ação, é necessário fazer a consulta pelo docente de interesse. Ver 'Consultando um docente/servidor'.");
			help.Oper.Add("• Na consulta, caso o docente tenha aulas alocadas, a coluna 'Função' receberá o valor 'Regente'.");
            
			help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.TitleAdd("• Área de Pesquisa");
			help.Oper.Add("• Coordenadoria: Coordenadoria da unidade de ensino.");
			help.Oper.Add("• Município: Município da unidade de ensino.");
			help.Oper.Add("• Unidade de Ensino.: Unidade de ensino.");

            help.Oper.TitleAdd("• Grade: Servidores");
			help.Oper.Add("• Matrícula: Matrícula do servidor. Este campo não pode ser alterado.");
			help.Oper.Add("• Nome: Nome do servidor. Este campo não pode ser alterado.");
            help.Oper.Add("• Cargo: Cargo do servidor.");
            help.Oper.Add("• Disciplina de Ingresso: Disciplina de ingresso. Este campo não pode ser alterado.");
            help.Oper.Add("• Carga Horária do Cargo: Valor da carga horária do cargo do servidor.");
            help.Oper.Add("• Função: Função do servidor. (Tabela: Funções)");
			help.Oper.Add("• Readaptado: Readaptação do servidor. Este campo não pode ser trocado.");
            help.Oper.Add("• Data Início Readaptação: Data de início da readaptação. Este campo não pode ser trocado.");
            help.Oper.Add("• Data Fim Readaptação: Data de fim da readaptação. Este campo não pode ser trocado.");
            help.Oper.Add("• Situação: Situação do servidor na sua lotação. Este campo não pode ser trocado. (Tabela: Licenças)");
            help.Oper.Add("• Data Início Situação: Data de início desta situação. Este campo não pode ser trocado.");
			help.Oper.Add("• Data Fim Situação: Data de fim desta situação. Este campo não pode ser trocado.");
            help.Oper.Add("• Aulas Alocadas: Número de aulas alocadas.");
            help.Oper.Add("• Aulas Alocadas em GLP: Número de aulas alocadas em GLP");
			help.Oper.Add("• Telefone: Telefone principal de contato do servidor.");
			help.Oper.Add("• Celular: Celular de contato do servidor.");
            help.Oper.Add("• Coordenadoria: Coordenadoria da unidade de ensino.");
            help.Oper.Add("• Unidade de Ensino.: Unidade de ensino.");

			help.Oper.TitleAdd("Botões");
			help.Oper.Add("?I: Salva a inserção/alteração da linha.", "~/img/bt_salvar.png");
			help.Oper.Add("?I: Cancela a inserção/alteração da nova linha.", "~/img/bt_cancelar.png");
			help.Oper.Add("?I: Permite alteração na linha.", "~/img/bt_editar.png");
			help.Oper.Add("?I: Limpa os filtros selecionados.", "~/img/bt_Limpa.png");
			help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
		}
	}
}
