using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Academico
{
	public partial class Alunos
	{
		public override void HelpInit(HelpData help)
		{
			help.ShowDefaultHelp = false;
			help.Summary.Add("Consultar, cadastrar, alterar e remover alunos.");

			help.Oper.TitleAdd("Informando dados de pesquisa");
			help.Oper.Add("Para consultar um aluno é necessário fazer uma pesquisa pelo aluno de interesse clicando no botão ?I.", "~/Images/bt_drop.png");
			help.Oper.Add("A pesquisa pode ser feita pelo nome, matrícula, nome da mãe e/ou nome do pai do aluno. Após definidas estas informações, deve-se clicar em ?I.", "~/Images/bot_buscar.png");
			help.Oper.Add("O resultado da pesquisa aparecerá em uma lista suspensa e deve-se selecionar o aluno de interesse clicando na linha em que o aluno aparece nesta lista.");

			help.Oper.TitleAdd("Consultando alunos");
			help.Oper.Add("A consulta será realizada automaticamente quando o aluno de interesse for selecionado.");
			help.Oper.Add("Os dados do aluno selecionado serão exibidos e divididos em cinco abas:");
			help.Oper.Add("1 - Dados Pessoais: Dados referentes à pessoa do aluno que podem ser consultados na tela 'Pessoas'.");
			help.Oper.Add("2 - Dados Escolares: Dados referentes ao aluno.");
			help.Oper.Add("3 - Responsáveis: Permite cadastro, alteração e remoção dos responsáveis relacionados ao aluno.");
			help.Oper.Add("4 - Irmãos: Permite cadastro, alteração e remoção dos irmãos relacionados ao aluno.");
			help.Oper.Add("5 - Opções de Unidade de Ensino: Permite a consulta das unidades de ensino disponíveis ao aluno.");
			//help.Oper.Add("6 - Documentos Entregues: Permite cadastro, alteração e remoção dos documentos entregues relacionados ao aluno.");
			help.Oper.Add("Obs.: A consulta destas quatro últimas abas ficará desabilitada em modo de cadastro e de alteração de um registro.");

			help.Oper.TitleAdd("Cadastrando novo aluno");
			help.Oper.Add("Para cadastrar um novo aluno é necessário cadastrar uma pessoa.");
			help.Oper.Add("Esta pessoa pode ser cadastrada previamente na tela 'Pessoas' e selecionada nesta tela, ou cadastrada na própria tela 'Alunos'.");
			help.Oper.Add("Para cadastrar um novo aluno deve-se clicar no botão ?T.", btnNovo);
			help.Oper.Add("Será carregado um formulário em branco para preenchimento dos dados.");
			help.Oper.Add("Deve-se preencher os campos com os dados do novo aluno nas abas 'Dados Pessoais' e 'Dados Escolares'.");
			help.Oper.Add("Obs.: A aba 'Responsáveis' fica desabilitada no modo de inclusão.");
			help.Oper.Add("Os dados pessoais do aluno podem ser preenchidos de duas maneiras:");
			help.Oper.Add("1 - Pessoa já cadastrada");
			help.Oper.Add("• Para utilizar uma pessoa já cadastrada é necessário realizar uma pesquisa clicando no botão ?I.", "~/Images/bt_drop.png");
			help.Oper.Add("• A pesquisa pode ser feita pelo código, nome, RG e/ou CPF da pessoa. Após definidas estas informações, deve-se clicar em 'Buscar'.");
			help.Oper.Add("• O resultado da pesquisa aparecerá em uma lista suspensa e deve-se selecionar a pessoa de interesse clicando na linha em que a pessoa aparece nesta lista.");
			help.Oper.Add("• Os dados da pessoa selecionada serão carregados na aba 'Dados Pessoais'.");
			help.Oper.Add("2 - Nova pessoa");
			help.Oper.Add("• Para incluir uma nova pessoa é necessário preencher individualmente os dados referentes à pessoa na aba 'Dados Pessoais'.");
			help.Oper.Add("Para salvar os dados do novo aluno deve-se clicar no botão ?T. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", btnSalvar);
			help.Oper.Add("Para cancelar a inclusão dos dados do aluno deve-se clicar no botão ?T.", btnCancel);
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");

			help.Oper.TitleAdd("Alterando aluno");
			help.Oper.Add("Para alterar um aluno é necessário fazer a consulta do aluno desejado. Ver 'Consultando alunos'.");
			help.Oper.Add("Para alterar os dados do aluno selecionado deve-se clicar no botão ?T.", btnEditar);
			help.Oper.Add("Os dados do aluno serão carregados permitindo alterações nos campos.");
			help.Oper.Add("Para salvar os dados do aluno deve-se clicar no botão ?T. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", btnSalvar);
			help.Oper.Add("Para cancelar a alteração nos dados do aluno deve-se clicar no botão ?T.", btnCancel);
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

			help.Oper.TitleAdd("Removendo aluno");
			help.Oper.Add("Para remover um aluno é necessário fazer a consulta do aluno desejado. Ver 'Consultando alunos'.");
			help.Oper.Add("Para remover os dados do aluno deve-se clicar no botão ?T e confirmar a remoção do registro.", btnExcluir);
			help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");
			help.Oper.Add("Obs.: Ao remover um aluno, os dados da pessoa relacionada a este aluno também serão removidos, exceto quando esta pessoa estiver relacionada a outro aluno ou funcionário.");

			help.Oper.TitleAdd("Incluindo, modificando e excluindo foto de um aluno");
			help.Oper.Add("Para realizar as operações com foto é necessário criar um novo registro de aluno ou editar um registro existente.");
			help.Oper.Add("• Incluir: Para incluir uma foto deve-se clicar no botão 'Procurar', selecionar a foto desejada no diretório correspondente e clicar no botão ?I.", "~/images/bot_confirmar.png");
			help.Oper.Add("• Excluir: Para excluir a foto deve-se clicar no botão ?I.", "~/Images/SmallDelete.png");
			help.Oper.Add("• Modificar: Para modificar a foto deve-se incluir uma nova foto. Não há necessidade de apagar a foto anterior.");

            help.Oper.TitleAdd("Enturmando aluno");
            help.Oper.Add("Para enturmar um aluno clique no botão 'Enturmar Aluno' dentro da aba 'Dados Escolares'.");
            help.Oper.Add("A tela será redirecionada para a tela 'Matrícula', já trazendo preenchida as informações do aluno selecionado.");

			help.Oper.TitleAdd("Descrição dos Campos");
			help.Oper.TitleAdd("Aba: Dados Pessoais");
			help.Oper.TitleAdd("• Dados Pessoais:");
			help.Oper.Add("• Pessoa: Código de identificação único da pessoa, gerado automaticamente ao iniciar um novo cadastro. Este dado não pode ser alterado.");
			help.Oper.Add("• Nome: Nome completo do aluno.");
            help.Oper.Add("• Nome Social: Nome pelo qual uma pessoa se identifica e é reconhecida no meio social, diferente do nome de registro civil.");
			help.Oper.Add("• Data Nascimento: Data de nascimento do aluno.");
			help.Oper.Add("• Sexo: Sexo do aluno.");
            help.Oper.Add("• Necessidade Especial: Informar se o aluno possui alguma necessidade especial. (Tabela Geral: Necessidade Especial)");
            help.Oper.Add("• Estado Civil: Estado civil do aluno. (Tabela Geral: Estado civil)");
            help.Oper.Add("• País de Nascimento: País de nascimento do aluno. (Tabela: Países)");
            help.Oper.Add("• Nacionalidade: País de nascimento do aluno. (Tabela: Nacionalidades)");
			help.Oper.Add("• Naturalidade: Município de nascimento do aluno.");
			help.Oper.Add("• UF de Nascimento: Estado referente à naturalidade do aluno. Este dado é exibido automaticamente após seleção da naturalidade e não permite edição.");
            help.Oper.Add("• Tipo Sanguíneo: Tipo sanguíneo do aluno. (Tabela Geral: TipoSanguineo)");
            help.Oper.Add("• Cor/Raça: Cor ou raça do aluno. (Tabela Geral: Cor_Raca)");
            help.Oper.Add("• Credo: Credo do aluno. (Tabela Geral: Credo)");
			help.Oper.Add("• Quantidade de filhos: Quantidade de filhos do aluno.");

			help.Oper.TitleAdd("• Endereço:");
			help.Oper.Add("Os dados referentes ao endereço do aluno podem ser preenchidos de duas maneiras:");
			help.Oper.Add("1 - Ao selecionar no campo país o valor 'Brasil' é possível selecionar o CEP clicando no botão ?I.", "~/Images/bt_drop.png");
			help.Oper.Add("Uma nova janela será aberta, nesta pode ser informado Município, Logradouro ou CEP e ao clicar no botão ?I os possíveis endereços serão exibidos.", "~/Images/bt_drop.png");
			help.Oper.Add("Deve se clicar no endereço desejado, retornando para a tela de cadastro de alunos com os campos Endereço, Bairro, Município e CEP já preenchidos automaticamente.");
			help.Oper.Add("Obs.: O número deverá ser informado pelo usuário.");
			help.Oper.Add("2 - Os dados podem ser preenchidos um a um na própria tela.");
			help.Oper.Add("Obs.: Quando selecionado um valor diferente de 'Brasil' para o campo país, essa será a única maneira de preenchimento dos dados.");
            help.Oper.Add("• País: País de residência do aluno. (Tabela: Países)");
            help.Oper.Add("• CEP: CEP de residência do aluno.");
            help.Oper.Add("• Município: Município de residência do aluno.");
            help.Oper.Add("• Estado: Estado da residência do aluno.");
            help.Oper.Add("• Endereço: Endereço de residência completo do aluno.");
			help.Oper.Add("• Nº: Número da residência do aluno.");
			help.Oper.Add("• Compl.: Complemento residencial do aluno.");
			help.Oper.Add("• Bairro: Bairro de residência do aluno.");
            help.Oper.Add("• Localização/Zona de Residência: Localização/zona de residência do aluno.");

			help.Oper.TitleAdd("• Contato:");
			help.Oper.Add("• Telefone: Telefone principal de contato do aluno.");
			help.Oper.Add("• Celular: Celular de contato do aluno.");
			help.Oper.Add("• E-mail: Endereço eletrônico do aluno.");

			help.Oper.TitleAdd("• Documento:");
            help.Oper.Add("• CPF: Número do CPF do aluno.");
            help.Oper.Add("• Tipo: Tipo do documento informado. (Tabela Geral: TIPO DOC)");
			help.Oper.Add("• Número: Número do documento informado.");
            help.Oper.Add("• Estado: Estado de emissão do documento informado. (AC, AL, AM, AP, BA, CE, DF, ES, GO, MA, MG, MS, MT, PA, PB, PE, PI, PR, RJ, RN, RO, RR, RS, SC, SE, SP, TO)");
            help.Oper.Add("• Órgão Emissor: Órgão emissor do documento informado. (Tabela Geral: Orgao RG)");
			help.Oper.Add("• Data de Expedição: Data de expedição do documento informado.");

			help.Oper.TitleAdd("• Outras Informações:");
			help.Oper.Add("• Observação: Observação livre referente ao aluno.");
			help.Oper.Add("• Dívida Biblioteca: Indica se o aluno possui ou não dívida pendente com a biblioteca.");
			help.Oper.Add("• Autoriza Envio de E-mail: Indica se o aluno autoriza ou não contato por endereço eletrônico.");
			help.Oper.Add("• Número Censo: Identifica o código do censo do aluno.");
			help.Oper.Add("• Número Regua: Identifica o número regua do aluno. Corresponde ao registro único do aluno na SeeducRJ.");

			help.Oper.TitleAdd("• Certidão Civil:");
            help.Oper.Add("• Tipo Certidão Civil: Tipo da certidão civil do aluno.");
			help.Oper.Add("• Número do Termo: Número do termo da certidão de nascimento do aluno.");
			help.Oper.Add("• Folha: Folha de registro da certidão de nascimento do aluno.");
			help.Oper.Add("• Livro: Livro de registro da certidão de nascimento do aluno.");
			help.Oper.Add("• Cartório: Cartório de registro da certidão de nascimento do aluno.");
			help.Oper.Add("• Data de Emissão: Data de emissão da certidão de nascimento do aluno.");
            help.Oper.Add("• Estado: Estado de registro da certidão de nascimento do aluno. (AC, AL, AM, AP, BA, CE, DF, ES, GO, MA, MG, MS, MT, PA, PB, PE, PI, PR, RJ, RN, RO, RR, RS, SC, SE, SP, TO)");

			help.Oper.TitleAdd("Aba: Dados Escolares");
			help.Oper.TitleAdd("• Aluno:");
			help.Oper.Add("• Matrícula: Código identificador único do aluno. Este dado não pode ser alterado após inclusão.");
			help.Oper.Add("• Situação: Situação do aluno na unidade de ensino.");
			help.Oper.Add("• Causa do Encerramento: Causa do encerramento do aluno na unidade de ensino.");
			help.Oper.Add("• Motivo: Motivo do encerramento.");
			help.Oper.Add("Obs.: Os dados anteriores não podem ser cadastrados ou editados. São utilizados apenas para consulta.");

			help.Oper.TitleAdd("• Escolaridade:");
			help.Oper.Add("• Unidade de Ensino: Unidade de ensino relacionada à escolaridade do aluno.");
			help.Oper.Add("• Unidade Física: Unidade física na qual o aluno está matriculado.");
			help.Oper.Add("• Nível/Segmento: Nível da escolaridade. Este campo é utilizado para filtrar a escolaridade. (Tabela: Nível de Ensino)");
			help.Oper.Add("• Modalidade: Modalidade da escolaridade. Este campo é utilizado para filtrar a escolaridade. (Tabela: Modalidade de Ensino)");
			help.Oper.Add("• Escolaridade: Escolaridade na qual está matriculado o aluno.");
			help.Oper.Add("• Turno: Turno referente à escolaridade do aluno. Os valores possíveis para este campo devem ser previamente informados na tela 'Turnos'. (Tabela: Turno, turnos da Matriz Curricular cujo o filtro é Escolaridade)");
			help.Oper.Add("Obs.: Este dado só poderá ser preenchido após informar a escolaridade, e exibirá os turnos válidos para a escolaridade selecionada.");
            help.Oper.Add("• Matriz Curricular: Matriz curricular do aluno. (Tabela Matriz Curricular, cujos dados são filtrados por Escolaridade e Turno)");
            help.Oper.Add("• Ano Ingresso: Ano de ingresso do aluno. Os valores possíveis para este campo devem ser previamente informados na tela 'Ano Letivo'. (Tabela: Ano Letivo)");
			help.Oper.Add("• Período Ingresso: Período de ingresso do aluno. Os valores possíveis para este campo devem ser previamente informados na tela 'Ano Letivo' (Tabela: Ano Letivo).");
			help.Oper.Add("• Ano Escolar: Ano escolar atual do aluno. (Tabela: Matriz Curricular)");
			help.Oper.Add("Obs.: Este dado só poderá ser preenchido após informar a escolaridade, turno, ano e período de ingresso, e só serão exibidos os anos escolares relacionados aos valores selecionados.");
			help.Oper.Add("• Tipo de Ingresso: Tipo de ingresso do aluno. Os valores possíveis para este campo devem ser previamente informados na aba 'Anos Escolares' da tela 'Tipos de Ingresso'. (Tabela: Tipos de Ingresso)");
            help.Oper.Add("• Escola de Origem: Escola de origem do aluno.");
            help.Oper.Add("• Recebe Escolarização em Outro Espaço (diferente da escola)??: Se o aluno recebe escolarização em outro espaço que não seja escola. (Tabela Geral: EscolarizacaoExterna)");

            help.Oper.TitleAdd("• Transporte:");
            help.Oper.Add("• Gratuidade: Se o transporte é gratuito ou não.");
            help.Oper.Add("• Modal: Modo de transporte. (Tabela Geral: TransporteModal)");

			help.Oper.TitleAdd("Aba: Responsáveis");
			help.Oper.Add("• Responsável: Responsabilidade atribuída à pessoa. Os valores possíveis para este campo devem ser previamente informados na tela 'Responsáveis'. (Tabela: Papel dos Responsáveis");
			help.Oper.Add("• Nome: Nome completo da pessoa.");
			help.Oper.Add("• CPF: Número de CPF da pessoa.");
			help.Oper.Add("• RG: Número de identidade da pessoa.");
			help.Oper.Add("• Órgão Emissor do RG: Órgão emissor do Registro Geral da pessoa.");
			help.Oper.Add("• Estado Emissor: Estado de emissão do Registro Geral da pessoa.");
			help.Oper.Add("• Telefone: Número de telefone para contato.");
			help.Oper.Add("• Celular: Número de celular para contato.");
			help.Oper.Add("• E-mail: Endereço de correio eletrônico para contato.");
			help.Oper.Add("• Responsável Legal: Indicar se a pessoa é responsável legal pelo aluno.");

			help.Oper.TitleAdd("Aba: Irmãos");
			help.Oper.Add("• Nome: Nome completo do irmão.");
			help.Oper.Add("• Telefone: Número de telefone para contato.");
			help.Oper.Add("• Celular: Número de celular para contato.");
			help.Oper.Add("• E-mail: Endereço eletrônico para contato.");
			help.Oper.Add("• Responsável Legal: Indicar se a pessoa é responsável legal pelo aluno.");

			help.Oper.TitleAdd("Aba: Opções de Unidade de Ensino");
			help.Oper.Add("• Ano: Ano da opção de unidade de ensino.");
			help.Oper.Add("• Período: Período da opção de unidade de ensino.");
			help.Oper.Add("• Ordem: Ordem da opção de unidade de ensino.");
			help.Oper.Add("• Unidade de Ensino: Unidade de ensino.");

			//help.Oper.TitleAdd("Aba: Documentos Entregues");
			//help.Oper.Add("• Documento: Descrição do tipo de documento entregue.");
			//help.Oper.Add("• Observação: Observação livre referente ao documento entregue.");
			//help.Oper.Add("• Status: Status do documento entregue.");
			//help.Oper.Add("• Quantidade: Quantidade de documentos entregues do tipo especificado.");

			help.Oper.TitleAdd("Botões");
			help.Oper.TitleAdd("Dados Pessoais/Dados Escolares");
			help.Oper.Add("?T: Carrega um formulário para novo registro.", btnNovo);
			help.Oper.Add("?T: Salva as alterações do registro.", btnSalvar);
			help.Oper.Add("?T: Cancela a operação corrente e retorna para página inicial.", btnCancel);
			help.Oper.Add("?T: Permite alteração no registro.", btnEditar);
			help.Oper.Add("?T: Remove o registro.", btnExcluir);
			help.Oper.Add("?I: Confirma a inclusão/modificação de foto.", "~/images/bot_confirmar.png");
			help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");

			help.Oper.TitleAdd("Responsáveis/Irmãos");
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