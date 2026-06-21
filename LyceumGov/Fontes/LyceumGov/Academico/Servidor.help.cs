using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Academico
{
    public partial class Servidor
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Consultar, cadastrar, alterar e remover servidores/funcionários.");

            help.Oper.Add("Para consultar, cadastrar, alterar ou remover um servidor/funcionário é necessário fazer uma pesquisa pelo servidor/funcionário de interesse clicando no botão ?I.", "~/Images/bt_drop.png");
            help.Oper.Add("A pesquisa pode ser feita pelo nome, matrícula, documento, CPF e/ou unidade de ensino do servidor/funcionário. Após definidas estas informações, deve-se clicar em 'Buscar'.");
            help.Oper.Add("O resultado da pesquisa aparecerá em uma lista suspensa e deve-se selecionar o servidor/funcionário de interesse clicando na linha em que o aluno aparece nesta lista.");
            help.Oper.Add("Obs.: Esta pesquisa ficará desabilitada em modo de cadastro e de alteração de um registro.");

            help.Oper.TitleAdd("Consultando servidores/funcionários");
            help.Oper.Add("A consulta é realizada automaticamente quando o servidor/funcionário de interesse for selecionado.");
            help.Oper.Add("Os dados do servidor/funcionário selecionado serão exibidos e divididos em sete abas:");
            help.Oper.Add("1 - Dados Pessoais: Dados gerais referentes ao servidor/funcionário.");
            help.Oper.Add("2 - Endereço: Dados residenciais referentes ao servidor/funcionário.");
            help.Oper.Add("3 - Documentos: Dados dos documentos principais referentes ao servidor/funcionário.");
            help.Oper.Add("4 - Outros Documentos: ao servidor/funcionário.");
            help.Oper.Add("5 - Dados Profissionais Externo: ao servidor/funcionário.");
            help.Oper.Add("6 - Observações: Dados adicionais referentes ao servidor/funcionário.");
            help.Oper.Add("7 - Dados de Ingresso: Dados de ingresso associados ao servidor/funcionário.");
            help.Oper.Add("Obs.: A consulta de dados de ingresso ficará desabilitada em modo de cadastro de um registro.");

            help.Oper.TitleAdd("Cadastrando novo servidor/funcionário");
            help.Oper.Add("Para cadastrar um servidor/funcionário deve-se clicar no botão ?I.", "~/Images/SmallNew.png");
            help.Oper.Add("Um formulário em branco é carregado para o preenchimento dos dados do novo registro.");
            help.Oper.Add("Deve-se preencher os campos com os dados do novo servidor/funcionário nas seis primeiras abas.");
            help.Oper.Add("Para salvar os dados do novo servidor/funcionário deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/SmallOk.png");
            help.Oper.Add("Para cancelar a inclusão dos dados do servidor/funcionário deve-se clicar no botão ?I.", "~/Images/SmallCancel.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante o cadastro.");
            help.Oper.Add("Obs.: A aba 'Dados de Ingresso' fica desabilitada em modo de cadastro.");

            help.Oper.TitleAdd("Alterando servidor/funcionário");
            help.Oper.Add("Para alterar um servidor/funcionário é necessário fazer a consulta do servidor/funcionário desejado. Ver 'Consultando servidores/funcionários'.");
            help.Oper.Add("Para alterar os dados do servidor/funcionário deve-se clicar no botão ?I.", "~/Images/SmallEdit.png");
            help.Oper.Add("Um formulário é carregado com os dados do servidor/funcionário selecionado permitindo alteração nos campos.");
            help.Oper.Add("Para salvar os dados do servidor/funcionário deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/SmallOk.png");
            help.Oper.Add("Para cancelar a alteração nos dados do servidor/funcionário deve-se clicar no botão ?I.", "~/Images/SmallCancel.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

            help.Oper.TitleAdd("Removendo servidor/funcionário");
            help.Oper.Add("Para remover um servidor/funcionário é necessário fazer a consulta do servidor/funcionário desejado. Ver 'Consultando servidores/funcionários'.");
            help.Oper.Add("Para remover os dados do servidor/funcionário deve-se clicar no botão ?I e confirmar a remoção do registro.", "~/Images/SmallDelete.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a remoção.");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.TitleAdd("Aba: Dados Pessoais");
            help.Oper.TitleAdd("• Geral:");
            help.Oper.Add("• Código: Código de identificação único do servidor/funcionário, gerado automaticamente ao iniciar um novo cadastro. Este dado não pode ser alterado.");
            help.Oper.Add("• Nome Completo: Nome completo do servidor/funcionário.");
            help.Oper.Add("• Nome Abreviado: Nome abreviado do servidor/funcionário.");
            help.Oper.Add("• Sexo: Sexo do servidor/funcionário.");
            help.Oper.Add("• Estado Civil: Estado civil do servidor/funcionário. (Tabela Geral: Estado civil)");
            help.Oper.Add("• Nome do Cônjuge: Nome do cônjuge do servidor/funcionário.");
            help.Oper.TitleAdd("• Nascimento:");
            help.Oper.Add("• Data de Nascimento: Data de nascimento do servidor/funcionário.");
            help.Oper.Add("• País de Nascimento: País de nascimento do servidor/funcionário. (Tabela: Países)");
            help.Oper.Add("• Nacionalidade: Nacionalidade do servidor/funcionário. (Tabela: Nacionalidades)");
            help.Oper.Add("• Município: Município de nascimento do servidor/funcionário.");
            help.Oper.Add("• Estado: Estado de nascimento do servidor/funcionário.");

            help.Oper.TitleAdd("Aba: Endereço");
            help.Oper.TitleAdd("• Endereço Residencial:");
            help.Oper.Add("Os dados referentes ao endereço do servidor/funcionário podem ser preenchidos de duas maneiras:");
            help.Oper.Add("1 - Ao selecionar no campo país o valor 'Brasil', é possível pesquisar endereços brasileiros clicando no botão ?I localizado ao lado do campo 'CEP'.", "~/Images/bt_drop.png");
            help.Oper.Add("Uma nova janela será aberta para pesquisar endereço, na qual podem ser aplicados filtros de Município, Logradouro e/ou CEP.");
            help.Oper.Add("Ao clicar no botão ?I, os possíveis endereços serão exibidos.", "~/Images/bt_drop.png");
            help.Oper.Add("Deve-se clicar no endereço desejado, retornando para a tela de cadastro de servidores/funcionários com os campos Endereço, Bairro, Município e CEP já preenchidos automaticamente.");
            help.Oper.Add("Obs.: O número e complemento do endereço devem ser informados pelo usuário.");
            help.Oper.Add("2 - Os dados podem ser preenchidos individualmente na própria tela.");
            help.Oper.Add("Obs.: Quando selecionado um valor diferente de 'Brasil' para o campo país, essa será a única maneira de preenchimento dos dados.");
            help.Oper.Add("• País: País onde se localiza a residência do servidor/funcionário. (Tabela: Países)");
            help.Oper.Add("• CEP: CEP da residência do servidor/funcionário.");
            help.Oper.Add("• Município: Município onde se localiza a residência do servidor/funcionário.");
            help.Oper.Add("• Estado: Estado onde se localiza a residência do servidor/funcionário.");
            help.Oper.Add("• Endereço: Endereço da residência do servidor/funcionário.");
            help.Oper.Add("• Número: Número da residência do servidor/funcionário.");
            help.Oper.Add("• Complemento: Complemento da residência do servidor/funcionário.");
            help.Oper.Add("• Bairro: Bairro onde se localiza a residência do servidor/funcionário.");
            help.Oper.Add("• Observação: Informação adicional sobre o endereço do servidor/funcionário.");
            help.Oper.Add("• Endereço Correto: Indica se o endereço foi confirmado e está correto.");

            help.Oper.TitleAdd("• Contatos:");
            help.Oper.Add("• Telefone: Telefone principal de contato do servidor/funcionário.");
            help.Oper.Add("• Celular: Celular de contato do servidor/funcionário.");
            help.Oper.Add("• Fax: Fax de contato do servidor/funcionário.");
            help.Oper.Add("• Telefone para Recados: Telefone para recados do servidor/funcionário.");
            help.Oper.Add("• E-mail Interno: Endereço eletrônico principal do servidor/funcionário.");
            help.Oper.Add("• E-mail Externo: Endereço eletrônico externo do servidor/funcionário.");

            help.Oper.TitleAdd("Aba: Documentos");
            help.Oper.TitleAdd("• Documentos:");
            help.Oper.Add("• Tipo: Tipo do documento informado para a identificação do servidor/funcionário. (Tabela Geral: TIPO DOC)");
            help.Oper.Add("• Número: Número do documento informado.");
            help.Oper.Add("• Órgão Emissor: Órgão emissor do documento informado. (Tabela Geral: Orgao RG)");
            help.Oper.Add("• Estado: Estado onde foi emitido o documento informado. (Tabela Fixa: UF)");
            help.Oper.Add("• Data de Expedição: Data de expedição do documento informado.");
            help.Oper.Add("• CPF: Número do CPF do servidor/funcionário.");
            help.Oper.Add("• Passaporte: Número do passaporte do servidor/funcionário.");
            help.Oper.TitleAdd("• Certidão de Nascimento ou Casamento:");
            help.Oper.Add("• Número do Termo: Número do termo da certidão de nascimento ou casamento do servidor/funcionário.");
            help.Oper.Add("• Folha: Folha da certidão de nascimento ou casamento do servidor/funcionário.");
            help.Oper.Add("• Livro: Livro da certidão de nascimento ou casamento do servidor/funcionário.");
            help.Oper.Add("• Cartório: Cartório onde foi emitida a certidão de nascimento ou casamento do servidor/funcionário.");
            help.Oper.Add("• Data de Emissão: Data de emissão da certidão de nascimento ou casamento do servidor/funcionário.");
            help.Oper.Add("• Estado: Estado onde foi emitida a certidão de nascimento ou casamento do servidor/funcionário. (Tabela Fixa: UF)");

            help.Oper.TitleAdd("Aba: Outros Documentos");
            help.Oper.TitleAdd("• Carteira Profissional:");
            help.Oper.Add("• Número: Número da carteira de trabalho do servidor/funcionário.");
            help.Oper.Add("• Série: Série da carteira de trabalho do servidor/funcionário.");
            help.Oper.Add("• Data de Expedição: Data de expedição da carteira de trabalho do servidor/funcionário.");
            help.Oper.Add("• Estado: Estado onde foi emitida a carteira de trabalho do servidor/funcionário. (Tabela Fixa: UF)");
            help.Oper.TitleAdd("• Título de Eleitor:");
            help.Oper.Add("• Número: Número do título de eleitor do servidor/funcionário.");
            help.Oper.Add("• Zona: Zona do título de eleitor do servidor/funcionário.");
            help.Oper.Add("• Seção: Seção do título de eleitor do servidor/funcionário.");
            help.Oper.Add("• Data de Expedição: Data de expedição do título de eleitor do servidor/funcionário.");
            help.Oper.Add("• Município: Município onde foi emitido o título de eleitor do servidor/funcionário.");
            help.Oper.Add("• Estado: Estado onde foi emitido o título de eleitor do servidor/funcionário.");
            help.Oper.TitleAdd("• Alistamento Militar:");
            help.Oper.Add("• Número: Número do certificado de alistamento militar do servidor/funcionário.");
            help.Oper.Add("• RM: Região militar onde foi emitido o certificado de alistamento militar do servidor/funcionário.");
            help.Oper.Add("• Série: Seção onde foi emitido o certificado de alistamento militar do servidor/funcionário.");
            help.Oper.Add("• CSM: Circunscrição de Serviço Militar onde foi emitido o certificado de alistamento militar do servidor/funcionário.");
            help.Oper.Add("• Data de Expedição: Data de expedição do certificado de alistamento militar do servidor/funcionário.");
            help.Oper.TitleAdd("• Certificado de Reservista:");
            help.Oper.Add("• Número: Número do certificado de reservista do servidor/funcionário.");
            help.Oper.Add("• RM: Região militar onde foi emitido o certificado de reservista do servidor/funcionário.");
            help.Oper.Add("• Série: Seção onde foi emitido o certificado de reservista do servidor/funcionário.");
            help.Oper.Add("• CSM: Circunscrição de Serviço Militar onde foi emitido o certificado de reservista do servidor/funcionário.");
            help.Oper.Add("• CAT: CAT do certificado de reservista do servidor/funcionário.");
            help.Oper.Add("• Data de Expedição: Data de expedição do certificado de reservista do servidor/funcionário.");

            help.Oper.TitleAdd("Aba: Dados Profissionais Externo");
            help.Oper.TitleAdd("• Dados Profissionais Externo:");
            help.Oper.Add("• Profissão: Profissão exercida pelo servidor/funcionário. (Tabela Geral: Profissao)");
            help.Oper.Add("• Cargo: Cargo exercido pelo servidor/funcionário.");
            help.Oper.Add("• Conselho Regional: Conselho regional do curso de formação do servidor/funcionário.");
            help.Oper.Add("• Empresa: Empresa onde a pessoa trabalha atualmente.");
            help.Oper.Add("• País: País onde se localiza a empresa. (Tabela: Países)");
            help.Oper.Add("• CEP: CEP da empresa.");
            help.Oper.Add("• Município: Município onde se localiza a empresa.");
            help.Oper.Add("• Estado: Estado onde se localiza a empresa.");
            help.Oper.Add("• Endereço: Endereço da empresa.");
            help.Oper.Add("• Número: Número da empresa.");
            help.Oper.Add("• Complemento: Complemento da empresa.");
            help.Oper.Add("• Bairro: Bairro onde se localiza a empresa.");
            help.Oper.Add("• E-mail: Endereço eletrônico da empresa.");
            help.Oper.Add("• Telefone: Telefone principal de contato da empresa.");
            help.Oper.Add("• Fax: Fax de contato da empresa.");
            help.Oper.Add("Obs.: O preenchimento do endereço da empresa pode ser realizado utilizando os mesmos recursos de preenchimento do endereço do servidor/funcionário empregados na subseção 'Endereço'.");

            help.Oper.TitleAdd("Aba: Observações");
            help.Oper.Add("• Data de Falecimento: Data de falecimento do servidor/funcionário.");
            help.Oper.Add("• Observações: Observação livre referente ao servidor/funcionário.");
            help.Oper.Add("• Renda Mensal: Renda mensal do servidor/funcionário.");
            help.Oper.Add("• Tipo de Necessidade Especial: Informar se o servidor/funcionário possui alguma necessidade especial. (Tabela Geral: Necessidade Especial)");
            help.Oper.Add("• Cor/Raça: Cor ou raça do servidor/funcionário. (Tabela Geral: Cor_Raca)");

            help.Oper.TitleAdd("Aba: Dados de Ingresso");
            help.Oper.Add("• Matrícula: Matrícula referente ao ingresso associada ao servidor/funcionário.");
            help.Oper.Add("• Ordem: Ordem referente ao ingresso associado ao servidor/funcionário.");
            help.Oper.Add("• Data de Nomeação: Data de nomeação do ingresso associado ao servidor/funcionário.");
            help.Oper.Add("• Data de Demissão: Data de demissão do ingresso associado ao servidor/funcionário.");

            help.Oper.TitleAdd("Botões");
            help.Oper.TitleAdd("Dados Pessoais/Endereço/Documentos/Outros Documentos/Dados Profissionais Externo/Observações");
            help.Oper.Add("?I: Carrega um formulário para novo registro.", "~/Images/SmallNew.png");
            help.Oper.Add("?I: Salva as alterações do registro.", "~/Images/SmallOk.png");
            help.Oper.Add("?I: Cancela a operação corrente e retorna para página inicial.", "~/Images/SmallCancel.png");
            help.Oper.Add("?I: Permite alteração no registro.", "~/Images/SmallEdit.png");
            help.Oper.Add("?I: Remove o registro.", "~/Images/SmallDelete.png");
            help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");

            help.Oper.TitleAdd("Dados de Ingresso");
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
