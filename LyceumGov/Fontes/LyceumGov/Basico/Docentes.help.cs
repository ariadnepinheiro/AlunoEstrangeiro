using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Techne.Web;

namespace Techne.Lyceum.Net.Basico
{
    public partial class Docentes
    {
        public override void HelpInit(HelpData help)
        {
            help.ShowDefaultHelp = false;
            help.Summary.Add("Consultar, cadastrar e alterar docentes.");
            //help.PreReq.Add(typeof(Techne.Lyceum.Net.Hades.TabelaGeral));
            help.Oper.Add("Para consultar ou alterar um docente é necessário fazer uma pesquisa pelo docente de interesse.");
            help.Oper.Add("A pesquisa pode ser feita pelo nome, Id/Vínculo, matrícula, CPF e/ou documento. Após definidas estas informações, deve-se clicar em 'Buscar'..");
            help.Oper.Add("O resultado da pesquisa aparecerá em uma lista suspensa e deve-se selecionar o docente de interesse nesta lista.");
            help.Oper.Add("Esta pesquisa ficará desabilitada nos modos de cadastro e de alteração.");

            help.Oper.TitleAdd("Consultando docentes");
            help.Oper.Add("A consulta é realizada automaticamente quando o docente de interesse for selecionado.");
            help.Oper.Add("Os dados do docente selecionado serão exibidos e divididos em cinco abas:");
            help.Oper.Add("1 - Dados Pessoais: Dados pessoais referentes ao docente.");
            help.Oper.Add("2 - Documentos: Documentos importantes para a identificação do docente, tais como de identidade e de trabalho.");
            help.Oper.Add("3 - Dados de Ingresso: Dados adicionais do docente, tais como matrícula e data de admissão.");
            help.Oper.Add("4 - Formação: Dados sobre escolaridade, formação acadêmica, cursos de aperfeiçoamento do docente.");
            help.Oper.Add("5 - Capacitação: Cursos, área do conhecimento, capacitação, instituições, carga horária dos cursos e conclusão.");

            help.Oper.TitleAdd("Cadastrando novo docente");
            help.Oper.Add("Para cadastrar um novo docente deve-se clicar no botão  ?I.", "~/Images/SmallNew.png");
            help.Oper.Add("Um formulário em branco é carregado para o preenchimento dos dados do novo registro.");
            help.Oper.Add("Deve-se preencher os campos com os dados do novo docente nos blocos 'Dados Pessoais', 'Filiação' e 'Endereço'.");
            help.Oper.TitleAdd("Dados Pessoais");
            help.Oper.Add("• Dados Pessoais");
            help.Oper.Add("• Filiação");
            help.Oper.Add("• Endereço");
            help.Oper.TitleAdd("• Documentos");
            help.Oper.Add("• Documento de Identificação");
            help.Oper.Add("• Carteira Profissional");
            help.Oper.Add("• Título de Eleitor");
            help.Oper.Add("• Alistamento Militar");
            help.Oper.Add("• Certificado de Reservista");
            help.Oper.TitleAdd("• Dados de Ingresso");
            help.Oper.Add("• Acumulação");
            help.Oper.Add("• Disciplina de Ingresso");
            help.Oper.Add("• Lotação");

            help.Oper.Add("O preenchimento dos campos das abas “Dados Pessoais”, “Documentos” e “Dados de Ingresso” podem ser feitos de duas maneiras:");
            help.Oper.Add("1 - Seleciona-se uma pessoa previamente cadastrada realizando uma pesquisa pela pessoa de interesse.");
            help.Oper.Add("Para aplicar filtros à pesquisa deve-se clicar no botão ?I ao lado da grade de pesquisa de pessoas.", "~/img/bt_busca.png");
            help.Oper.Add("A pesquisa pode ser feita pelo nome, Id/Vínculo, matrícula, CPF e/ou documento. Após definidas estas informações, deve-se clicar em 'Buscar'.");
            help.Oper.Add("O resultado da pesquisa aparecerá em uma lista suspensa e deve-se selecionar a pessoa de interesse clicando na linha em que a pessoa aparece nesta lista.");
            help.Oper.Add("Os campos dos blocos 'Dados Pessoais', ‘Filiação’ e 'Endereço' são preenchidos automaticamente com os dados da pessoa selecionada.");
            help.Oper.Add("Obs.: Os campos referentes à pessoa selecionada podem ser modificados, permitindo a alteração dos dados da pessoa no modo de cadastro de docentes.");
            help.Oper.Add("2 - Cadastra-se uma nova pessoa, preenchendo individualmente os campos das abas “Dados Pessoais”, “Documentos” e “Dados de Ingresso”.");
            help.Oper.Add("Para salvar os dados do novo docente deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/SmallOk.png");
            help.Oper.Add("Para cancelar a inclusão dos dados do docente deve-se clicar no botão ?I.", "~/Images/SmallCancel.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, serão exibidas mensagens de erro informando os erros ocorridos durante o cadastro.");

            help.Oper.TitleAdd("Alterando docente");
            help.Oper.Add("Para alterar um docente é necessário fazer a consulta do docente desejado. Ver 'Consultando docentes'.");
            help.Oper.Add("Para alterar os dados do docente selecionado deve-se clicar no botão ?I.", "~/Images/SmallEdit.png");
            help.Oper.Add("Os formulários são carregados com os dados do docente nas 5 abas habilitando os campos editáveis para alteração.");
            help.Oper.Add("Para salvar os dados do docente deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/SmallOk.png");
            help.Oper.Add("Nas abas “Dados Pessoais”, “Documentos” e “Dados de Ingresso”, para salvar os dados do docente, deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/SmallOk.png");
            help.Oper.Add("Na aba “Formação”, para salvar os dados alterados no bloco “Formação”, deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/FormacaoGraduacao.png");
            help.Oper.Add("Na aba “Formação”, para salvar os dados alterados no bloco “Pós-Graduação”, deve-se clicar no botão ?I. Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/FormacaoPosGraduacao.png");
            help.Oper.Add("Na aba “Capacitação”, para salvar os dados alterados, deve-se clicar no botão ?I, Se algum campo obrigatório não estiver preenchido será exibido um alerta.", "~/Images/IncluirCapacitacao.png");
            help.Oper.Add("Para cancelar a alteração dos dados nas abas do Docente deve-se clicar no botão ?I", "~/Images/SmallCancel.png");
            help.Oper.Add("Caso a operação não seja concluída com sucesso, será exibida uma mensagem de erro informando os erros ocorridos durante a alteração.");

            help.Oper.TitleAdd("Descrição dos Campos");
            help.Oper.TitleAdd("Aba: Dados Pessoais");
            help.Oper.TitleAdd("• Dados Pessoais");
            help.Oper.Add("• Pessoa: Código de identificação único da pessoa, gerado automaticamente ao iniciar um novo cadastro. Este campo não pode ser alterado.");
            help.Oper.Add("• ID INEP: Código único atribuído pelo Instituto Nacional de Estudos e Pesquisas Educacionais Anísio Teixeira (INEP) para identificar oficialmente cada instituição de ensino no Brasil.");
            help.Oper.Add("• Nome Completo: Nome completo do docente.");
            help.Oper.Add("• Nome Social: Nome pelo qual uma pessoa deseja ser reconhecida e tratada socialmente.");
            help.Oper.Add("• Data Nascimento: Data de nascimento do docente.");
            help.Oper.Add("• Sexo: Sexo do docente.");
            help.Oper.Add("• Cor/Raça: usado em contextos oficiais, como no censo, para classificar a população com base na aparência física e autoidentificação.");
            help.Oper.Add("• Necessidade Especial: Informar se o docente possui alguma necessidade especial.");
            help.Oper.Add("• Estado Civil: Estado civil do docente.");
            help.Oper.Add("• País de Nascimento: País de nascimento do docente. (Tabela: Países)");
            help.Oper.Add("• Nacionalidade: Nacionalidade do docente. (Tabela: Nacionalidades)");
            help.Oper.Add("• Naturalidade: Naturalidade do docente.");
            help.Oper.Add("• Estado: Estado de nascimento do docente.");

            help.Oper.TitleAdd("• Filiação");
            help.Oper.Add("• Nome da Mãe: Nome da Mãe");
            help.Oper.Add("• Nome do Pai: Nome do Pai");

            help.Oper.TitleAdd("• Endereço");
            help.Oper.Add("• País: País onde se localiza a residência do docente. (Tabela: Países)");
            help.Oper.Add("• CEP: CEP da residência do docente.");
            help.Oper.Add("• Município: Município onde se localiza a residência do docente.");
            help.Oper.Add("• Estado: Estado onde se localiza a residência do docente.");
            help.Oper.Add("• Endereço: Endereço da residência do docente.");
            help.Oper.Add("• N.º: Número da residência do docente.");
            help.Oper.Add("• Complemento: Complemento da residência do docente.");
            help.Oper.Add("• Bairro: Bairro onde se localiza a residência do docente.");
            help.Oper.Add("• Localização/Zona de Residência:");
            help.Oper.Add("• Localização Diferenciada:");
            help.Oper.Add("o Não se aplica");
            help.Oper.Add("o Área remanescente de quilombos");
            help.Oper.Add("o Área de assentamento");
            help.Oper.Add("o Terra indígena");
            help.Oper.Add("• Telefone: Telefone principal de contato do docente.");
            help.Oper.Add("• Celular: Celular de contato com o docente.");
            help.Oper.Add("• E-mail Office 365: Endereço eletrônico de contato com o docente.");
            help.Oper.Add("• E-mail Google for Education: Endereço eletrônico do Google for Education.");
            help.Oper.Add("• E-mail Alternativo: Endereço eletrônico alternativo de contato com o docente.");

            help.Oper.TitleAdd("Aba: Documentos");
            help.Oper.TitleAdd("• Documento de Identificação");
            help.Oper.Add("• Tipo: Tipo do documento informado para a identificação do docente. (Tabela Geral: TIPO DOC)");
            help.Oper.Add("• Número: Número do documento informado.");
            help.Oper.Add("• Estado: Estado onde foi emitido o documento informado.(Tabela Fixa: UF)");
            help.Oper.Add("• Órgão Emissor: Órgão emissor do documento informado. (Tabela Geral: ORGAO RG)");
            help.Oper.Add("• Data de Expedição: Data de expedição do documento informado.");

            help.Oper.TitleAdd("• Outros Documentos");
            help.Oper.Add("• CPF: Número do CPF do docente.");
            help.Oper.Add("• PIS/PASEP: Número do PIS/PASEP do docente.");

            help.Oper.TitleAdd("• Carteira Profissional");
            help.Oper.Add("• Número: Número da carteira de trabalho do docente.");
            help.Oper.Add("• Série: Série da carteira de trabalho do docente.");
            help.Oper.Add("• Data de Expedição: Data de expedição da carteira de trabalho do docente.");
            help.Oper.Add("• Estado: Estado onde foi emitida a carteira de trabalho do docente. (Tabela Fixa: UF)");

            help.Oper.TitleAdd("• Título de Eleitor:");
            help.Oper.Add("• Número: Número do título de eleitor do docente.");
            help.Oper.Add("• Zona: Zona do título de eleitor do docente.");
            help.Oper.Add("• Seção: Seção do título de eleitor do docente.");
            help.Oper.Add("• Data de Expedição: Data de expedição do título de eleitor do docente.");

            help.Oper.TitleAdd("Aba: Documentos Militares");
            help.Oper.TitleAdd("• Alistamento Militar:");
            help.Oper.Add("• Número: Número do certificado de alistamento militar do docente.");
            help.Oper.Add("• RM: Região militar onde foi emitido o certificado de alistamento militar do docente.");
            help.Oper.Add("• Série: Seção onde foi emitido o certificado de alistamento militar do docente.");
            help.Oper.Add("• CSM: Circunscrição de Serviço Militar onde foi emitido o certificado de alistamento militar do docente.");
            help.Oper.Add("• Data de Expedição: Data de expedição do certificado de alistamento militar do docente.");

            help.Oper.TitleAdd("• Certificado de Reservista:");
            help.Oper.Add("• Número: Número do certificado de reservista do docente.");
            help.Oper.Add("• RM: Região militar onde foi emitido o certificado de reservista do docente.");
            help.Oper.Add("• Série: Seção onde foi emitido o certificado de reservista do docente.");
            help.Oper.Add("• CSM: Circunscrição de Serviço Militar onde foi emitido o certificado de reservista do docente.");
            help.Oper.Add("• CAT: CAT do certificado de reservista do docente.");
            help.Oper.Add("• Data de Expedição: Data de expedição do certificado de reservista do docente.");

            help.Oper.TitleAdd("Aba Dados de Ingresso");
            help.Oper.TitleAdd("Dados de Ingresso");
            help.Oper.Add("• Id Funcional: Código de identificação funcional do servidor.");
            help.Oper.Add("• Vínculo: Código de identificação do vínculo do servidor.");
            help.Oper.Add("• Matrícula ou Id/Vínculo: Número de matrícula do docente.");
            help.Oper.Add("• Cargo: Cargo a que pertence o docente.  (Tabela Fixa: LY_CATEGORIA_DOCENTE)");
            help.Oper.Add("• Regime de Trabalho: Regime de trabalho do docente.");
            help.Oper.Add("• Data de Admissão: Data de admissão do docente.");
            help.Oper.Add("• Data de Demissão: Data de demissão do docente.");
            help.Oper.Add("• Ano do Concurso: Ano no qual o docente prestou o concurso.");
            help.Oper.Add("• Candidato: Candidato.");
            help.Oper.Add("• Processo Seletivo: Conjunto de etapas e técnicas utilizadas para encontrar e selecionar novos colaboradores e contratá-los.");

            help.Oper.TitleAdd("Acumulação");
            help.Oper.Add("• Acumulação:	Acúmulo de cargos.");
            help.Oper.Add("o Sim");
            help.Oper.Add("o Não");
            help.Oper.Add("• Matrícula: Número de matrícula do docente.");
            help.Oper.Add("• Órgão:	Órgão onde o docente exerce a acumulação de cargo.");
            help.Oper.Add("• Nº de Processo: Número do processo para a acumulação de cargo.");

            help.Oper.TitleAdd("Disciplina de Ingresso");
            help.Oper.Add("• Disciplina: Disciplina para o qual prestou o processo.");

            help.Oper.TitleAdd("Lotação");
            help.Oper.Add("• Função: Função executada.");
            help.Oper.Add("• CH: Carga horária do docente.");
            help.Oper.Add("• Regional: Subdivisão administrativa da secretaria de educação");
            help.Oper.Add("• Município: Unidade administrativa que compõe o território de um estado");
            help.Oper.Add("• Unidade de Ensino: instituição ou espaço físico onde ocorre a educação, como escolas, universidades, centros de treinamento ou qualquer local onde se transmita conhecimento. ");
            help.Oper.Add("• Data de Nomeação: Data em que o docente é nomeado para o cargo.");

            help.Oper.TitleAdd("Aba Formação");
            help.Oper.TitleAdd("Formação");
            help.Oper.Add("• Escolaridade: Nível de educação formal que um indivíduo alcançou.");
            help.Oper.Add("• Situação do Curso:  Status atual do curso em relação à sua regularidade, reconhecimento, e/ou o progresso do aluno dentro do mesmo.");
            help.Oper.Add("• Área do Curso: Campo geral de conhecimento ou disciplina em que um curso universitário se insere.");
            help.Oper.Add("• Curso:	Programas de estudos específicos e organizados segundo a atividade ou profissão pretendida.");
            help.Oper.Add("• Formação/Complementação Pedagógica: Curso destinado a quem já possui diploma de nível superior (bacharelado ou tecnólogo) e deseja atuar como professor, obtendo a licenciatura na sua área de formação.");
            help.Oper.Add("• Ano do Início: Ano em que o docente iniciou a formação.");
            help.Oper.Add("• Ano de Conclusão: Ano em que o docente concluiu a formação.");
            help.Oper.Add("• Tipo de Instituição: Tipo de instituição.");
            help.Oper.Add("• Instituição: Nome da instituição.");
            help.Oper.Add("• Documentos Probatórios: Documentos que comprovam a formação.");

            help.Oper.TitleAdd("Pós-Graduação:");
            help.Oper.Add("• Situação do Curso:  Status atual do curso em relação à sua regularidade, reconhecimento, e/ou o progresso do aluno dentro do mesmo.");
            help.Oper.Add("• Área do Curso: Campo geral de conhecimento ou disciplina em que um curso universitário se insere.");
            help.Oper.Add("• Curso:	Programas de estudos específicos e organizados segundo a atividade ou profissão pretendida.");
            help.Oper.Add("• Formação/Complementação Pedagógica: Curso destinado a quem já possui diploma de nível superior (bacharelado ou tecnólogo) e deseja atuar como professor, obtendo a licenciatura na sua área de formação.");
            help.Oper.Add("• Ano do Início: Ano em que o docente iniciou a formação.");
            help.Oper.Add("• Ano de Conclusão: Ano em que o docente concluiu a formação.");
            help.Oper.Add("• Tipo de Instituição: Tipo de instituição.");
            help.Oper.Add("• Instituição: Nome da instituição.");
            help.Oper.Add("• Documentos Probatórios: Documentos que comprovam a formação.");

            help.Oper.TitleAdd("Aba Capacitação");
            help.Oper.TitleAdd("GRID Capacitação Profissional");
            help.Oper.Add("• Oferecido SEEDUC");
            help.Oper.Add("• Curso/Capacitação");
            help.Oper.Add("• Tipo de Curso");
            help.Oper.Add("• Área de Conhecimento");
            help.Oper.Add("• Nome da Instituição");
            help.Oper.Add("• Carga Horária");
            help.Oper.Add("• Data de Conclusão");
            help.Oper.TitleAdd("Informe os dados da inclusão / consulta:");
            help.Oper.Add("• Oferecido pela SEEDUC:");
            help.Oper.Add("o Sim");
            help.Oper.Add("o Não");
            help.Oper.Add("• Tipo de Curso: Qual o tipo do curso??");
            help.Oper.Add("• Área de Conhecimento:	 Campos de estudo e pesquisa, como ciências exatas, biológicas, humanas, entre outras.");
            help.Oper.Add("• Curso/Capacitação: Programas de treinamento com foco em aprimorar habilidades e conhecimentos em áreas específicas.");
            help.Oper.Add("• Nome da Instituição: Nome da Instituição em que se capacitou.");
            help.Oper.Add("• Carga Horária: Carga horária da capacitação.");
            help.Oper.Add("• Data de Conclusão: Data de conclusão da capacitação.");

            help.Oper.TitleAdd("Botões");
            help.Oper.Add("?T: Carrega um formulário para novo registro.", btnNovo);
            help.Oper.Add("?T: Salva as alterações do registro.", btnSalvar);
            help.Oper.Add("?T: Cancela a operação corrente e retorna para página inicial.", btnCancel);
            help.Oper.Add("?T: Permite alteração no registro.", btnEditar);

            help.Oper.Add("?I: Salva ou altera o registro da Formação Pessoal - Graduação.", "~/Images/FormacaoGraduacao.png");
            help.Oper.Add("?I: Salva ou altera o registro da Formação Pessoal – Pós-Graduação.", "~/Images/FormacaoPosGraduacao.png");
            help.Oper.Add("?I: Salva ou altera o registro da Capacitação.", "~/Images/IncluirCapacitacao.png");

            help.Oper.Add("Obs.: A visibilidade dos botões depende da operação em andamento.");
        }
    }
}