using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.ProcessoSeletivo
{
    public partial class CandidatoDocenteFicha
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Cadastrar candidatos docentes.");

            help.Oper.TitleAdd("Cadastrando candidato docente");
            help.Oper.Add("Preencha a ficha de inscrição do candidato docente e clique no botão ?I para salvar os dados preenchidos. Caso algum campo obrigatório não esteja preenchido, uma mensagem de erro será retornada.", "~/images/SmallOk.png");
            help.Oper.Add("Caso queira cancelar o cadastro do candidato docente clique no botão ?I.", "~/images/SmallCancel.png");
            help.Oper.Add("Ao término do cadastro, caso queira cadastrar uma nova ficha de inscrição, clique no botão ?I.", "~/Images/SmallNew.png");
            help.Oper.Add("Obs.: As quatro últimas abas só ficam desabilitadas após o término do cadastro.");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.TitleAdd("• Aba: Dados da Inscrição");
            help.Oper.Add("• Candidato: Código de identificação do candidato.");
            help.Oper.TitleAdd("• Área de Pesquisa: Concurso");
            help.Oper.Add("• Concurso: Código de identificação do concurso.");
            help.Oper.Add("• Descrição: Descrição do concurso.");
            help.Oper.TitleAdd("• Área de Pesquisa: Coordenadoria");
            help.Oper.Add("• Coordenadoria: Código de identificação da coordenadoria.");
            help.Oper.Add("• Descrição: Descrição da coordenadoria.");
            help.Oper.TitleAdd("• Área de Pesquisa: Habilitação");
            help.Oper.Add("• Categoria: Código de identificação da habilitação.");
            help.Oper.Add("• Descrição: Descrição da habilitação.");
            help.Oper.TitleAdd("• Área de Pesquisa: Disciplina de Ingresso");
            help.Oper.Add("• Disciplina: Código de identificação da disciplina.");
            help.Oper.Add("• Descrição: Descrição da disciplina.");

            help.Oper.TitleAdd("• Aba: Dados Pessoais");
            help.Oper.Add("• Nome Completo: Nome completo do candidato.");
            help.Oper.Add("• Data Nascimento: Data de nascimento do candidato.");
            help.Oper.Add("• Sexo: Sexo do candidato (Masculino/Feminino).");
            help.Oper.Add("• Necessidade Especial: Preencher caso o candidato tenha alguma necessidade especial. (Tabela Geral: Necessidade Especial)");
            help.Oper.Add("• Nome da Mãe: Nome completo da mãe do candidato.");
            help.Oper.Add("• Nome do Pai: Nome completo do pai do candidato.");
            help.Oper.Add("• Estado Civil: Estado civil do candidato. (Tabela Geral: Estado civil)");
            help.Oper.Add("• País de Nascimento: Páis de nascimento do candidato. (Tabela: Países)");
            help.Oper.Add("• Nacionalidade: Nacionalidade do candidato. (Tabela: Nacionalidades)");
            help.Oper.Add("• Naturalidade: Naturalidade do candidato.");
            help.Oper.Add("• Estado: Estado da naturalidade do candidato.");
            help.Oper.Add("• País: País aonde reside o candidato. (Tabela: Países)");
            help.Oper.Add("• CEP: Código de Endereçamento Postal residencial o candidato.");
            help.Oper.Add("• Município: Município aonde reside o candidato.");
            help.Oper.Add("• Estado: Estado aonde reside o candidato.");
            help.Oper.Add("• Endereço: Endereço residencial do candidato.");
            help.Oper.Add("• N.º: Número do endereço residencial do candidato.");
            help.Oper.Add("• Compl.: Complemento do endereço residencial do candidato.");
            help.Oper.Add("• Bairro: Bairro aonde reside o candidato.");
            help.Oper.Add("• Telefone: Número do telefone residencial do candidato.");
            help.Oper.Add("• Celular: Número do celular do candidato.");
            help.Oper.Add("• E-mail: endereço eletrônico do candidato.");

            help.Oper.TitleAdd("• Aba: Documentos");
            help.Oper.Add("• Tipo: Selecione o tipo de documento desejado do candidato. (Tabela Geral: TIPO DOC)");
            help.Oper.Add("• Número: Número do documento do candidato selecionado anteriormente.");
            help.Oper.Add("• Estado: Estado aonde o documento do candidato foi emitido. (Tabela Fixa: UF)");
            help.Oper.Add("• Órgão Emissor: Órgão emissor do documento do candidato. (Tabela Geral: Orgao RG)");
            help.Oper.Add("• Data de Expedição: Data de expedição do documento do candidato.");
            help.Oper.Add("• CPF: Número do Cadastro de Pessoa Física do candidato.");
            help.Oper.Add("• PIS / PASEP: Número do PIS / PASEP do candidato.");
            help.Oper.TitleAdd("• Carteira Profissional");
            help.Oper.Add("• Número: Número da carteira profissional do candidato.");
            help.Oper.Add("• Série: Série da carteira profissional do candidato.");
            help.Oper.Add("• Data de Expedição: Data de expedição da carteira profissional do candidato.");
            help.Oper.Add("• Estado: Estado de emissão da carteira profissional do candidato. (Tabela Fixa: UF)");

            help.Oper.TitleAdd("Aba: Titulações/Experiências");
            help.Oper.TitleAdd("• Grade: Titulações");
            help.Oper.Add("• Titulação: Titulação associada ao concurso. (Tabela: Titulações para Concursos, cujo filtro é Concurso)");
            help.Oper.Add("• Pontuação: Pontuação referente à titulação.");
            help.Oper.TitleAdd("• Grade: Experiências");
            help.Oper.Add("• Experiência: Experiência requerida pelo concurso. (Tabela: Experiências para Concursos, cujo filtro é Concurso)");
            help.Oper.Add("• Pontuação: Pontuação referente à experiência.");

            help.Oper.TitleAdd("Aba: Processo Seletivo");
            help.Oper.Add("• Status: Status do candidato.");
            help.Oper.Add("• Pontuação: Pontuação do candidato.");
            help.Oper.Add("• Data Apresentação: Data de apresentação do candidato.");
            help.Oper.Add("• Hora Apresentação: Hora de apresentação do candidato.");
            help.Oper.Add("• Avaliado pela COSEP: Avaliação do candidato pela COSEP.");
            help.Oper.Add("• Avaliado pela CDGP: Avaliação do candidato pela CDGP.");
            help.Oper.Add("• Remessa Documento: Remessa documento do candidato.");

            help.Oper.TitleAdd("Aba: Situação de Avaliação");
            help.Oper.Add("• Situação: Situação da avaliação do candidato. (Tabela Fixa: LY_CONCURSO_DOC_SIT_AVALIACAO)");

            help.Oper.TitleAdd("Aba: Contrato Temporário");
            help.Oper.Add("• Status: Status do contrato temporário do docente.");
            help.Oper.Add("• Data de Início: Data de início do contrato temporário.");
            help.Oper.Add("• Data de Término: Data de término do contrato temporário.");

            help.Oper.TitleAdd("Botões");
            help.Oper.Add("?I: Carrega um formulário para novo registro.", "~/Images/SmallNew.png");
            help.Oper.Add("?I: Salva as alterações do registro.", "~/Images/SmallOk.png");
            help.Oper.Add("?I: Cancela a operação corrente e retorna para página inicial.", "~/Images/SmallCancel.png");
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
