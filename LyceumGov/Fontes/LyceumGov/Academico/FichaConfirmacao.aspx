<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="FichaConfirmacao.aspx.cs"
    EnableTheming="false" Inherits="Techne.Lyceum.Net.Academico.FichaConfirmacao" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .style2
        {
            width: 693px;
        }
    </style>
</head>
<body style="font-family: Arial; font-size: 11px">
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem" EnableTheming="true"> </asp:Label>
        <br />
        <input type="image" value="Imprimir" src="../Images/bot_imprimir.png" onclick="this.style.visibility='hidden';window.print()">
        <table>
            <tr>
                <td align="right">
                    <asp:Image ID="Image1" runat="server" AlternateText="Governo do Rio de Janeiro - Sec de Estado de Educação"
                        ImageUrl="~/Images/logo.gif" Style="text-align: right" />
                </td>
                <td align="center" class="style2">
                    GOVERNO DO ESTADO DO RIO DE JANEIRO
                    <br />
                    SECRETARIA DE EDUCAÇÃO
                    <br />
                    FICHA DE MATRÍCULA
                </td>
            </tr>
        </table>
        <br />
        <br />
        <div>
            <strong>DADOS PESSOAIS</strong></div>
        <hr id="tDadosPessoais" style="margin: 1px" />
        <table style="width: 100%; border-spacing: 0;" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td colspan="3">
                    <asp:Label runat="server" ID="Nome" Text="Nome:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblNome"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="DataNasc" Text="Data de Nascimento:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblDataNascimento"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Sexo" Text="Sexo:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblSexo"></asp:Label></strong>
                </td>
                <td rowspan="8">
                    <dxe:ASPxBinaryImage ID="bimgFotoPessoa" runat="server" AlternateText="sem foto"
                        ClientInstanceName="bimgFotoPessoa" Height="32mm" StoreContentBytesInViewState="True"
                        Width="32mm">
                        <EmptyImage AlternateText="sem foto" Url="~/Images/semfoto.jpg" />
                    </dxe:ASPxBinaryImage>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="QtdeFilhos" Text="Quantidade de Filhos:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblQtdeFilhos"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="TipoSanguineo" Text="Tipo Sanguíneo:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblTipoSanguineo"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Etnia" Text="Etnia:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblEtnia"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="EstadoCivil" Text="Estado Civil:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblEstadoCivil"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="PaisNasc" Text="País de Nascimento:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblPaisNasc"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Nacionalidade" Text="Nacionalidade:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblNacionalidade"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="UFNasc" Text="UF Nascimento:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblUFNasc"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Naturalidade" Text="Naturalidade:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblNaturalidade"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Credo" Text="Credo:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblCredo"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="NecEspecial" Text="Necessidade Especial:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblNecEspecial"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="Matricula" Text="Matrícula:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblMatricula"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="DataSit" Text="Data da Situação:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblDataSit"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="HoraSit" Text="Hora:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblHoraSit"></asp:Label></strong>
                </td>
            </tr>
        </table>
        <br />
        <div>
            <strong>FILIAÇÃO</strong></div>
        <hr id="Hr1" style="margin: 1px" />
        <table style="width: 100%; border-spacing: 0;" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td>
                    <asp:Label runat="server" ID="NomeMae" Text="Nome mãe:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblNomeMae"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="MaeFalecida" Text="Falecida:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblMaeFalecida"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="CPFMae" Text="CPF:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblCPFMae"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="NomePai" Text="Nome Pai:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblNomePai"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="PaiFalecido" Text="Falecido:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblPaiFalecido"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="CPFPai" Text="CPF:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblCPFPai"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label runat="server" ID="Label1" Text="Resp. Legal:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblRespLegal"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="Label3" Text="Nome Outros:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblNomeOutros"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label5" Text="CPF:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblCPFOutros"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label2" Text="Tel Mãe:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblTelMae"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label6" Text="Tel Pai:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblTelPai"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label8" Text="Tel Resp:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblTelResp"></asp:Label></strong>
                </td>
            </tr>
        </table>
        <br />
        <div>
            <strong>ENDEREÇO</strong></div>
        <hr id="Hr2" style="margin: 1px" />
        <table style="width: 100%; border-spacing: 0;" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label4" Text="Endereço:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblEndereco"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label7" Text="Número:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblNumero"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label9" Text="Complemento:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblComplemento"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label11" Text="Bairro:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblBairro"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label13" Text="Município:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblMunicipio"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label15" Text="Estado:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblEstado"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label17" Text="CEP:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblCEP"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label19" Text="Localização/Zona de Residência:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblLocalizacao"></asp:Label></strong>
                </td>
            </tr>
        </table>
        <br />
        <div>
            <strong>CONTATO</strong></div>
        <hr id="Hr3" style="margin: 1px" />
        <table style="width: 100%; border-spacing: 0;" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label10" Text="Telefone:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblContatoTelefone"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label14" Text="Celular:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblContatoCelular"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label18" Text="E-mail:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblContatoEmail"></asp:Label></strong>
                </td>
            </tr>
        </table>
        <br />
        <div>
            <strong>RG DETRAN/RJ</strong></div>
        <hr id="Hr4" style="margin: 1px" />
        <table style="width: 100%; border-spacing: 0;" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label12" Text="Número do Documento:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblNumDocDetran"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label20" Text="Data de Exp. RG DETRAN/RJ :"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblDataRGDetran"></asp:Label></strong>
                </td>
            </tr>
        </table>
        <br />
        <div>
            <strong>DOCUMENTO</strong></div>
        <hr id="Hr5" style="margin: 1px" />
        <table style="width: 100%; border-spacing: 0;" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="Label23" Text="CPF:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblCPFDocumento"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label16" Text="Tipo:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblTipoDocumento"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label22" Text="Número:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblNumeroDocumento"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label24" Text="Complemento Identidade:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblComplIdent"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label21" Text="Estado:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblEstadoDocumento"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label25" Text="Órgão Emissor:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblOrgaoEmissor"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label27" Text="Data de Expedição:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblDtExpedicaoDocumento"></asp:Label></strong>
                </td>
            </tr>
        </table>
        <br />
        <div>
            <strong>OUTRAS INFORMAÇÕES</strong></div>
        <hr id="Hr6" style="margin: 1px" />
        <table style="width: 100%; border-spacing: 0;" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label26" Text="Identificação no INEP:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblINEP"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label29" Text="Identificação Social(NIS):"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblNIS"></asp:Label></strong>
                </td>
            </tr>
        </table>
        <br />
        <div>
            <strong>CERTIDÃO CIVIL</strong></div>
        <hr id="Hr7" style="margin: 1px" />
        <table style="width: 100%; border-spacing: 0;" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td colspan="3">
                    <asp:Label runat="server" ID="Label28" Text="Tipo Certidão:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblTipoCertidao"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label31" Text="Certidão Civil:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblCertidaoCivil"></asp:Label></strong>
                </td>
            </tr>
            <tr >
                <td colspan="3">
                    <asp:Label runat="server" ID="Label30" Text="UF Cartório:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblUFCartorio"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label33" Text="Município Cartório:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblMunicipioCartorio"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label runat="server" ID="Label32" Text="Nome do Cartório:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblNomeCartorio"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label38" Text="Livro:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblLivro"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label39" Text="Folha:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblFolha"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label53" Text="Termo:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblTermo"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label41" Text="Data da Emissão:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblDtEmissaoCertidao"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label runat="server" ID="Label42" Text="Matrícula da Certidão:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblMatriculaCertidao"></asp:Label></strong>
                </td>
            </tr>
        </table>
        <br />
        <div>
            <strong>TRANSPORTE</strong></div>
        <hr id="Hr8" style="margin: 1px" />
        <table style="width: 100%; border-spacing: 0;" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label43" Text="Utiliza Transporte?"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblUtilizaTransporte"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label45" Text="Poder Responsável:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblPoderResponsavel"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="Label44" Text="Modal"></asp:Label>
                    <asp:CheckBoxList ID="chkModais" RepeatDirection="Horizontal" runat="server" DataTextField="descr"
                        DataValueField="item" Enabled="false">
                    </asp:CheckBoxList>
                </td>
            </tr>
        </table>
        <br />
        <div style="page-break-after: always">
        </div>
        <div>
            <strong>CONFIRMAÇÃO DE MATRÍCULA - VIA DO ALUNO</strong></div>
        <hr id="Hr9" style="margin: 1px" />
        <table style="width: 100%; border-spacing: 0;" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="Label40" Text="Matrícula:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblMatriculaRenovacaoViaAluno"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="Label47" Text="Nome:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblNomeViaAluno"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label49" Text="Ano Letivo:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblAnoLetivoViaAluno"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label51" Text="Período Letivo:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblPeriodoLetivoViaAluno"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="Label46" Text="Unidade de Ensino:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblUnidadeViaAluno"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="Label50" Text="Código do Censo:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblCensoViaAluno"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="Label54" Text="Modalidade:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblModalidadeViaAluno"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label48" Text="Código do Curso:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblCursoViaAluno"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label55" Text="Nome do Curso:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblNomeCursoViaAluno"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label52" Text="Série:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblSerieViaAluno"></asp:Label></strong>
                    <asp:Label runat="server" ID="Label56" Text="Turno:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblTurnoViaAluno"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label57" Text="Data Sugerida:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblDataSugeridaViaAluno"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label58" Text="Ensino Religioso:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblEnsReligiosoViaAluno"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label60" Text="Língua Estrangeira Optativa:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblOptativaViaAluno"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label62" Text="Situação:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblSituacaoViaAluno"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label64" Text="Data da Situação:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblDataSituacaoViaAluno"></asp:Label></strong>
                    <asp:Label runat="server" ID="Label59" Text="Hora:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblHoraSituacaoViaAluno"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2" align="justify" style="width: 200px">
                    <i>&#8220;Este documento comprova que a matrícula foi confirmada, estando condicionada
                        às regras da legislação de matrícula do período vigente.&#8221;</i>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label61" Text="Matricula do Servidor:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblMatriculaServidorViaAluno"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label65" Text="Nome do Servidor:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblNomeServidorViaAluno"></asp:Label></strong>
                </td>
            </tr>
        </table>
        <table style="width: 50%">
            <tr>
                <td>
                    <br />
                    <br />
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="border-top: 2px  solid" align="center">
                    Local
                </td>
                <td style="width:500px">
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <br />
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="border-top: 2px  solid" align="center">
                    Assinatura do Servidor
                </td>
                <td>
                    &nbsp;
                </td>
                <td style="border-top: 2px  solid" align="center">
                    Assinatura do Responsável do Aluno
                </td>
            </tr>
        </table>
        <br />
        <br />
        <table width="100%">
            <tr>
                <td style="border-top: 2px  dotted">
                    &nbsp;
                </td>
            </tr>
        </table>
        <br />
        <br />
        <div>
            <strong>CONFIRMAÇÃO DE MATRÍCULA - VIA UNIDADE DE ENSINO</strong></div>
        <hr id="Hr10" style="margin: 1px" />
        <table style="width: 100%; border-spacing: 0;" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="Label63" Text="Matrícula:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblMatriculaRenovacaoViaUnidade"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="Label66" Text="Nome:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblNomeViaUnidade"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label67" Text="Ano Letivo:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblAnoLetivoViaUnidade"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label68" Text="Período Letivo:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblPeriodoLetivoViaUnidade"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="Label69" Text="Unidade de Ensino:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblUnidadeViaUnidade"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="Label70" Text="Código do Censo:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblCensoViaUnidade"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="Label71" Text="Modalidade:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblModalidadeViaUnidade"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label72" Text="Código do Curso:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblCursoViaUnidade"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label73" Text="Nome do Curso:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblNomeCursoViaUnidade"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label74" Text="Série:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblSerieViaUnidade"></asp:Label></strong>
                    <asp:Label runat="server" ID="Label75" Text="Turno:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblTurnoViaUnidade"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label76" Text="Data Sugerida:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblDataSugeridaViaUnidade"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label77" Text="Ensino Religioso:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblEnsReligiosoViaUnidade"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label78" Text="Língua Estrangeira Optativa:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblOptativaViaUnidade"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label79" Text="Situação:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblSituacaoViaUnidade"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label80" Text="Data da Situação:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblDataSituacaoViaUnidade"></asp:Label></strong>
                    <asp:Label runat="server" ID="Label81" Text="Hora:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblHoraSituacaoViaUnidade"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2" align="justify">
                    <i>&#8220;Este documento comprova que a matrícula foi confirmada, estando condicionada
                        às regras da legislação de matrícula do período vigente.&#8221;</i>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label82" Text="Matricula do Servidor:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblMatriculaServidorViaUnidade"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label83" Text="Nome do Servidor:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblNomeServidorViaUnidade"></asp:Label></strong>
                </td>
            </tr>
        </table>
        <table style="width: 50%">
            <tr>
                <td>
                    <br />
                    <br />
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="border-top: 2px  solid" align="center">
                    Local
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <br />
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="border-top: 2px  solid" align="center">
                    Assinatura do Servidor
                </td>
                <td>
                    &nbsp;
                </td>
                <td style="border-top: 2px  solid" align="center">
                    Assinatura do Responsável do Aluno
                </td>
            </tr>
        </table>
        <br />
        <br />
    </div>
    </form>
</body>
</html>
