<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="FormularioSei.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.FormularioSei" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #divPrincipal
        {
            margin-right: 353px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnGeral" runat="server" GroupingText="Informações da Prestação de Contas:">
        <table>
            <tr>
                <td style="text-align: right; width: 20%">
                    <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" SkinID="lblObrigatorio"
                        runat="server" Text="Unidade de Ensino:*"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens" OnChanged="tseUnidadeResponsavel_Changed" 
                        MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT s.unidade_ens, s.nome_comp, s.setor, s.cgc, s.situacao, s.ua_atual, s.ua_antiga, s.municipio, s.id_regional from VW_UNIDADE_ENSINO_SITUACAO S INNER JOIN TCE_REGIONAL R ON S.ID_REGIONAL=R.ID_REGIONAL "
                        GridWidth="850px" SqlOrder="nome_comp">
                        <gridcolumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                            <tweb:TSearchBoxColumn Caption="Municipio" FieldName="municipio" Width="18%" Visible="false" />
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="id_regional" Width="18%" Visible="false" />
                        </gridcolumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 20%">
                    <asp:Label Font-Names="Verdana" ID="Label1" SkinID="lblObrigatorio" runat="server"
                        Text="Período Referência:*"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tsePeriodoReferencia" runat="server" Argument="DESCRICAO" ArgumentColumns="50" OnChanged="tsePeriodoReferencia_Changed" 
                        MaxLength="20" Columns="10" AutoPostBack="True" Caption="" SqlOrder="ANO, MESINICIAL DESC" 
                        Key="PERIODOREFERENCIAID" SqlSelect=" SELECT ANO, MESINICIAL, MESFINAL, REFERENCIA FROM PrestacaoContas.VW_PERIODOREFERENCIA "
                        DataType="Number">
                        <gridcolumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="PERIODOREFERENCIAID" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Período" FieldName="DESCRICAO" Width="80%" />
                        </gridcolumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnImportar" runat="server" Text="Gerar" OnClick="btnImportar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:Label ID="lblUltimaGeracao" runat="server" SkinID="lblMensagem"></asp:Label>
    <div id="divPrincipal" runat="server" visible="false">
        <table width="100%" style="font-family: Calibri; font-size: 13px; font-weight: bold;
            border: 1px solid #000000">
            <tr>
                <td style="height: 15px" colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" colspan="3">
                    ANEXO XI
                </td>
            </tr>
            <tr>
                <td align="center" style="font-size: 12px;" colspan="3">
                    FORMULÁRIO I da Resolução nº
                </td>
            </tr>
            <tr>
                <td align="center" colspan="3">
                    IDENTIFICAÇÃO DA UNIDADE ESCOLAR
                </td>
            </tr>
            <tr>
                <td style="height: 25px" colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    BLOCO 1- IDENTIFICAÇÃO DA UNIDADE ESCOLAR
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    <table border="1px" width="100%" style="font-weight: normal; font-family: Calibri;
                        font-size: 12px; border-spacing: 0; border-collapse: collapse; border: 1px solid #000000">
                        <tr style="height: 20px">
                            <td colspan="8">
                                &nbsp;1- Nome da AAE:
                                <asp:Label ID="lblNomeAae" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td colspan="8">
                                &nbsp;2- CNPJ:
                                <asp:Label ID="lblCnpj" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td colspan="8">
                                &nbsp;3- Endereço:
                                <asp:Label ID="lblEndereco" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td colspan="4">
                                &nbsp;4- Complemento do Endereço:
                                <asp:Label ID="lblComplemento" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td colspan="4">
                                &nbsp;5-Bairro:
                                <asp:Label ID="lblBairro" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td colspan="4">
                                &nbsp;6-Município:
                                <asp:Label ID="lblMunicipio" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td colspan="4">
                                &nbsp;7-CEP:
                                <asp:Label ID="lblCep" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td>
                                &nbsp;8-DDD:
                                <asp:Label ID="lblDdd" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td colspan="2">
                                &nbsp;9-Telefone:
                                <asp:Label ID="lblTelefone" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td colspan="5">
                                &nbsp;10-E-mail institucional:
                                <asp:Label ID="lblEmailInstitucional" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td colspan="8">
                                &nbsp;11- Diretoria Regional:
                                <asp:Label ID="lblRegional" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td colspan="2" align="center">
                                12-Modalidade de Ensino
                            </td>
                            <td align="center">
                                13-Números de Alunos
                            </td>
                            <td align="center">
                                14-Número de turnos
                            </td>
                            <td colspan="4" align="center">
                                15-Horário Integral
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td>
                                &nbsp;Fundamental
                            </td>
                            <td align="center">
                                <asp:Label ID="lblFundamental" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="lblFundamentalNumeroAlunos" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="lblFundamentalNumeroTurnos" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;sim
                            </td>
                            <td align="center">
                                <asp:Label ID="lblFundamentalIntegralSim" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;não
                            </td>
                            <td align="center">
                                <asp:Label ID="lblFundamentalIntegralNao" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td>
                                &nbsp;Médio
                            </td>
                            <td align="center">
                                <asp:Label ID="lblMedio" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="lblMedioNumeroAlunos" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="lblMedioNumeroTurnos" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;sim
                            </td>
                            <td align="center">
                                <asp:Label ID="lblMedioIntegralSim" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;não
                            </td>
                            <td align="center">
                                <asp:Label ID="lblMedioIntegralNao" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td>
                                &nbsp;Jovens e Adultos
                            </td>
                            <td align="center">
                                <asp:Label ID="lblEja" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="lblEjaNumeroAlunos" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="lblEjaNumeroTurnos" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;sim
                            </td>
                            <td align="center">
                                <asp:Label ID="lblEjaIntegralSim" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;não
                            </td>
                            <td align="center">
                                <asp:Label ID="lblEjaIntegralNao" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td>
                                &nbsp;Educação Especial
                            </td>
                            <td align="center">
                                <asp:Label ID="lblEducacaoEspecial" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="lblEducacaoEspecialNumeroAlunos" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="lblEducacaoEspecialNumeroTurnos" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;sim
                            </td>
                            <td align="center">
                                <asp:Label ID="lblEducacaoEspecialIntegralSim" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;não
                            </td>
                            <td align="center">
                                <asp:Label ID="lblEducacaoEspecialIntegralNao" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="height: 25px" colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    BLOCO 2- IDENTIFICAÇÃO DO(A) DIRETOR(A) DA UNIDADE ESCOLAR
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                Diretor(a) do final do período:
                            </td>
                        </tr>
                    </table>
                    <table border="1px" width="100%" style="font-weight: normal; font-family: Calibri;
                        font-size: 12px; border-spacing: 0; border-collapse: collapse; border: 1px solid #000000">
                        <tr style="height: 20px">
                            <td colspan="3">
                                &nbsp;16- Nome do(a) Diretor(a):
                                <asp:Label ID="lblDiretorNome" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td>
                                &nbsp;17- CPF:
                                <asp:Label ID="lblDiretorCpf" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;18- Matrícula:
                                <asp:Label ID="lblDiretorMatricula" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;19- ID:
                                <asp:Label ID="lblDiretorId" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td colspan="3">
                                &nbsp;20- Data de publicação do ato de nomeação para Diretor:
                                <asp:Label ID="lblDiretorDataDo" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                Diretor(a) do início do período:
                            </td>
                        </tr>
                    </table>
                    <table border="1px" width="100%" style="font-weight: normal; font-family: Calibri;
                        font-size: 12px; border-spacing: 0; border-collapse: collapse; border: 1px solid #000000">
                        <tr style="height: 20px">
                            <td colspan="3">
                                &nbsp;16- Nome do(a) Diretor(a):
                                <asp:Label ID="lblDiretorNomeInicio" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td>
                                &nbsp;17- CPF:
                                <asp:Label ID="lblDiretorCpfInicio" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;18- Matrícula:
                                <asp:Label ID="lblDiretorMatriculaInicio" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;19- ID:
                                <asp:Label ID="lblDiretorIdInicio" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td colspan="3">
                                &nbsp;20- Data de publicação do ato de nomeação para Diretor:
                                <asp:Label ID="lblDiretorDataDoInicio" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="height: 25px" colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    BLOCO 3- IDENTIFICAÇÃO DO(A) TESOUREIRO(A)/CO-GESTOR(A) UNIDADE ESCOLAR
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                Tesoureiro (a)/ Cogestor do final do período:
                            </td>
                        </tr>
                    </table>
                    <table border="1px" width="100%" style="font-weight: normal; font-family: Calibri;
                        font-size: 12px; border-spacing: 0; border-collapse: collapse; border: 1px solid #000000">
                        <tr style="height: 20px">
                            <td colspan="3">
                                &nbsp;21- Nome do(a) Tesoureiro (a)/ Cogestor (a):
                                <asp:Label ID="lblTesoureiroNome" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td>
                                &nbsp;22- CPF:
                                <asp:Label ID="lblTesoureiroCpf" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;23- Matrícula:
                                <asp:Label ID="lblTesoureiroMatricula" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;24- ID:
                                <asp:Label ID="lblTesoureiroId" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
               <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                Tesoureiro (a)/ Cogestor do início do período:
                            </td>
                        </tr>
                    </table>
                    <table border="1px" width="100%" style="font-weight: normal; font-family: Calibri;
                        font-size: 12px; border-spacing: 0; border-collapse: collapse; border: 1px solid #000000">
                        <tr style="height: 20px">
                            <td colspan="3">
                                &nbsp;21- Nome do(a) Tesoureiro (a)/ Cogestor (a):
                                <asp:Label ID="lblTesoureiroNomeInicio" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td>
                                &nbsp;22- CPF:
                                <asp:Label ID="lblTesoureiroCpfInicio" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;23- Matrícula:
                                <asp:Label ID="lblTesoureiroMatriculaInicio" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;24- ID:
                                <asp:Label ID="lblTesoureiroIdInicio" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="height: 25px" colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    BLOCO 4- DADOS BANCÁRIOS
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    <table border="1px" width="100%" style="font-weight: normal; font-family: Calibri;
                        font-size: 12px; border-spacing: 0; border-collapse: collapse; border: 1px solid #000000">
                        <tr style="height: 20px">
                            <td colspan="2">
                                &nbsp;25- Banco:
                                <asp:Label ID="lblBanco" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td>
                                &nbsp;26- Agência::
                                <asp:Label ID="lblAgencia" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;27- Conta Corrente:
                                <asp:Label ID="lblContaCorrente" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="height: 15px" colspan="3">
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <div id="divFormularioSeiII" runat="server" visible="false">
        <table width="100%" style="font-family: Calibri; font-size: 13px; font-weight: bold;
            border: 1px solid #000000">
            <tr>
                <td style="height: 15px" colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" colspan="3">
                    FORMULÁRIO II - A DA Resolução SEEDUC Nº 5.722 DE 18 DE FEVEREIRO DE 2019
                </td>
            </tr>
            <tr>
                <td align="center" colspan="3">
                    RELAÇÃO DOS RECURSOS RECEBIDO PELA SECRETARIA DE ESTADO DE EDUCAÇÃO E DESPESAS REALIZADAS
                    PELA AAE NO PROGRAMA DE APOIO À <u>NUTRIÇÃO ESCOLAR</u>
                </td>
            </tr>
            <tr>
                <td style="height: 25px" colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    BLOCO 1- DADOS DAS RECEITAS RECEBIDAS
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    <table border="1px" width="100%" style="font-weight: normal; font-family: Calibri;
                        font-size: 12px; border-spacing: 0; border-collapse: collapse; border: 1px solid #000000">
                        <tr style="height: 20px">
                            <td>
                                &nbsp;1- Período da Prestação de Contas:
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;&nbsp;<asp:Label ID="lblfrm2PeriodoPrestacao" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td>
                                &nbsp;2 - Saldo Anterior:
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;&nbsp;<asp:Label ID="lblfrm2SaldoAnterior" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td>
                                &nbsp;3 - Repasses Recebidos:
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;&nbsp;<asp:Label ID="lblfrm2RepasseRecebido" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                         <tr style="height: 20px">
                            <td>
                                &nbsp;3.1 - Créditos/Débitos:
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;&nbsp;<asp:Label ID="lblfrm2CreditosDebitos" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td>
                                &nbsp;4 - Saldo Inicial (2 + 3 + 3.1):
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;&nbsp;<asp:Label ID="lblfrm2SaldoInicial" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 25px" colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    BLOCO 2 - DADOS DAS DESPESAS REALIZADAS
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    <div runat="server" id="divGridIIA">
                    </div>
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="height: 25px" colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    BLOCO 3- SALDO FINAL
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    <table border="1px" width="100%" style="font-weight: normal; font-family: Calibri;
                        font-size: 12px; border-spacing: 0; border-collapse: collapse; border: 1px solid #000000">
                        <tr style="height: 20px">
                            <td>
                                &nbsp;10 - Saldo Final (4 &#8211; 9)
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;&nbsp;<asp:Label ID="lblfrm2SaldoFinal" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="height: 15px" colspan="3">
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <div id="divFormularioSeiIII" runat="server" visible="false">
        <table width="100%" style="font-family: Calibri; font-size: 13px; font-weight: bold;
            border: 1px solid #000000">
            <tr>
                <td style="height: 15px" colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" colspan="3">
                    FORMULÁRIO II - B DA Resolução SEEDUC Nº 5.722 DE 18 DE FEVEREIRO DE 2019
                </td>
            </tr>
            <tr>
                <td align="center" colspan="3">
                    RELAÇÃO DOS RECURSOS RECEBIDO PELA SECRETARIA DE ESTADO DE EDUCAÇÃO E DESPESAS REALIZADAS
                    PELA AAE NO PROGRAMA DE APOIO À <u>MANUTENÇÃO</u>
                </td>
            </tr>
            <tr>
                <td style="height: 25px" colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    BLOCO 1- DADOS DAS RECEITAS RECEBIDAS
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    <table border="1px" width="100%" style="font-weight: normal; font-family: Calibri;
                        font-size: 12px; border-spacing: 0; border-collapse: collapse; border: 1px solid #000000">
                        <tr style="height: 20px">
                            <td>
                                &nbsp;1- Período da Prestação de Contas:
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;&nbsp;<asp:Label ID="lblfrm3PeriodoPrestacao" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td>
                                &nbsp;2 - Saldo Anterior:
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;&nbsp;<asp:Label ID="lblfrm3SaldoAnterior" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td>
                                &nbsp;3 - Repasses Recebidos:
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;&nbsp;<asp:Label ID="lblfrm3RepasseRecebido" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                         <tr style="height: 20px">
                            <td>
                                &nbsp;3.1 - Créditos/Débitos:
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;&nbsp;<asp:Label ID="lblfrm3CreditosDebitos" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td>
                                &nbsp;4 - Saldo Inicial (2 + 3 + 3.1):
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;&nbsp;<asp:Label ID="lblfrm3SaldoInicial" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 25px" colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    BLOCO 2 - DADOS DAS DESPESAS REALIZADAS
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    <div runat="server" id="divGridIIB">
                    </div>
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="height: 25px" colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    BLOCO 3- SALDO FINAL
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    <table border="1px" width="100%" style="font-weight: normal; font-family: Calibri;
                        font-size: 12px; border-spacing: 0; border-collapse: collapse; border: 1px solid #000000">
                        <tr style="height: 20px">
                            <td>
                                &nbsp;11 - Saldo Final (4 &#8211; 9 &#8211; 10 + Rendimentos)
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;&nbsp;<asp:Label ID="lblfrm3SaldoFinal" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="height: 15px" colspan="3">
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <div id="divformularioSeiIV" runat="server" visible="false">
        <div runat="server" id="divGridIIC">
        </div>
        <%-- <table width="100%" style="font-family: Calibri; font-size: 13px; font-weight: bold;
            border: 1px solid #000000">
           
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                   
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
        
        </table>--%>
    </div>
    <div id="divFormularioSeiV" runat="server" visible="false">
        <table width="100%" style="font-family: Calibri; font-size: 13px; font-weight: bold;
            border: 1px solid #000000">
            <tr>
                <td style="height: 15px" colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" colspan="3">
                    ANEXO XV
                </td>
            </tr>
            <tr>
                <td align="center" colspan="3">
                    Formulário V da Resolução SEEDUC n.º 5654 DE 21 DE MAIO DE 2018
                </td>
            </tr>
            <tr>
                <td align="center" colspan="3" style="background-color: Yellow;">
                    BALANCETE CONSOLIDADO DAS RECEITAS E DESPESAS
                </td>
            </tr>
            <tr>
                <td align="center" colspan="3">
                    RECURSOS TRANSFERIDOS PELA SECRETARIA DE ESTADO DE EDUCAÇÃO
                </td>
            </tr>
            <tr>
                <td style="height: 25px" colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    <table border="1px" width="100%" style="font-weight: normal; font-family: Calibri;
                        font-size: 12px; border-spacing: 0; border-collapse: collapse; border: 1px solid #000000">
                        <tr style="height: 20px">
                            <td>
                                &nbsp;1- Período
                            </td>
                            <td>
                                &nbsp;<asp:Label ID="lblfrm5PeriodoPrestacao" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="height: 15px" colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    <table border="1px" width="100%" style="font-weight: normal; font-family: Calibri;
                        font-size: 12px; border-spacing: 0; border-collapse: collapse; border: 1px solid #000000">
                        <tr style="height: 20px">
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;Merenda
                            </td>
                            <td>
                                &nbsp;Manutenção
                            </td>
                            <td>
                                &nbsp;Outros Projetos
                            </td>
                            <td>
                                &nbsp;8 -Total
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td>
                                &nbsp;2- Saldo Anterior (R$)
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;<asp:Label ID="lblfrm5MerendaSaldoAnterior" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;<asp:Label ID="lblfrm5ManutencaoSaldoAnterior" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;<asp:Label ID="lblfrm5OutrosProjetosSaldoAnterior" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;<asp:Label ID="lblfrm5TotalSaldoAnterior" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td>
                                &nbsp;3- Repasses (R$)
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;<asp:Label ID="lblfrm5MerendaRepasses" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;<asp:Label ID="lblfrm5ManutencaoRepasses" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;<asp:Label ID="lblfrm5OutrosProjetosRepasses" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;<asp:Label ID="lblfrm5TotalRepasses" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                         <tr style="height: 20px">
                            <td>
                                &nbsp;3.1 - Créditos/Débitos (R$)
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;<asp:Label ID="lblfrm5MerendaCreditosDebitos" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;<asp:Label ID="lblfrm5ManutencaoCreditosDebitos" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;<asp:Label ID="lblfrm5OutrosProjetosCreditosDebitos" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;<asp:Label ID="lblfrm5TotalCreditosDebitos" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td>
                                &nbsp;4- Rendimentos (R$)
                            </td>
                            <td style="background-color: Silver;">
                                &nbsp;<asp:Label ID="lblfrm5MerendaRendimentos" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;<asp:Label ID="lblfrm5ManutencaoRendimentos" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td style="background-color: Silver;">
                                &nbsp;<asp:Label ID="lblfrm5OutrosProjetosRendimentos" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;<asp:Label ID="lblfrm5TotalRendimentos" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td>
                                &nbsp;5- Devoluções (R$)
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;<asp:Label ID="lblfrm5MerendaDevolucoes" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;<asp:Label ID="lblfrm5ManutencaoDevolucoes" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;<asp:Label ID="lblfrm5OutrosProjetosDevolucoes" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;<asp:Label ID="lblfrm5TotalDevolucoes" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td>
                                &nbsp;6- (-) Despesas (R$)
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;<asp:Label ID="lblfrm5MerendaDespesas" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;<asp:Label ID="lblfrm5ManutencaoDespesas" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;<asp:Label ID="lblfrm5OutrosProjetosDespesas" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;<asp:Label ID="lblfrm5TotalDespesas" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td>
                                &nbsp;7- Saldo Final (R$)
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;<asp:Label ID="lblfrm5MerendaSaldoFinal" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;<asp:Label ID="lblfrm5ManutencaoSaldoFinal" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;<asp:Label ID="lblfrm5OutrosProjetosSaldoFinal" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td style="background-color: Yellow;">
                                &nbsp;<asp:Label ID="lblfrm5TotalSaldoFinal" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="height: 15px" colspan="3">
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    <div id="divFormularioSeiVI" runat="server" visible="false">
        <table width="100%" style="font-family: Calibri; font-size: 13px; font-weight: bold;
            border: 1px solid #000000">
            <tr>
                <td style="height: 30px" colspan="3">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" colspan="3">
                    <u>CADASTRO DE FORNECEDORES DAS UNIDADES ESCOLARES E DIRETORIAS REGIONAIS</u>
                </td>
            </tr>
            <tr>
                <td style="height: 30px" colspan="3">
                    &nbsp;<hr style="width: 100%;" />
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td style="height: 25px">
                    CAMPO 1 - DIRETORIA REGIONAL
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    <table width="100%" style="font-weight: normal; font-family: Calibri; font-size: 12px;
                        border-spacing: 0; border-collapse: collapse; border: 1px solid #FFFFFF">
                        <tr style="height: 20px">
                            <td>
                                &nbsp;01 - Identificação da Diretoria Regional
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td style="border-style: solid;">
                                &nbsp;<asp:Label ID="lblfrm6DiretoriaRegional" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    <table width="100%" style="font-weight: normal; font-family: Calibri; font-size: 12px;
                        border-spacing: 0; border-collapse: collapse; border: 1px solid #FFFFFF">
                        <tr style="height: 20px">
                            <td>
                                &nbsp;02 &#8211; Município(s) Atendido(s) pelo fornecedor
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td style="border-style: solid;">
                                &nbsp;<asp:Label ID="lblfrm6MunicipioAtendidos" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="height: 15px" colspan="3">
                    &nbsp;<hr style="width: 100%;" />
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td style="height: 25px">
                    CAMPO 2 &#8211; DADOS DO FORNECEDOR
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    <table width="100%" style="font-weight: normal; font-family: Calibri; font-size: 12px;
                        border-spacing: 0; border-collapse: collapse; border: 1px solid #FFFFFF">
                        <tr style="height: 20px">
                            <td>
                                &nbsp;03 &#8211; CNPJ
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td style="border-style: solid;">
                                &nbsp;<asp:Label ID="lblfrm6Cnpj" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    <table width="100%" style="font-weight: normal; font-family: Calibri; font-size: 12px;
                        border-spacing: 0; border-collapse: collapse; border: 1px solid #FFFFFF">
                        <tr style="height: 20px">
                            <td>
                                &nbsp;04 &#8211; Razão Social *
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td style="border-style: solid;">
                                &nbsp;<asp:Label ID="lblfrm6RazaoSocial" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    <table width="100%" style="font-weight: normal; font-family: Calibri; font-size: 12px;
                        border-spacing: 0; border-collapse: collapse; border: 1px solid #FFFFFF">
                        <tr style="height: 20px">
                            <td>
                                &nbsp;05 &#8211; Inscrição Estadual
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;06 &#8211; Inscrição Municipal
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td style="border-style: solid;">
                                &nbsp;<asp:Label ID="lblfrm6InscricaoEstadual" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td style="border-style: solid;">
                                &nbsp;<asp:Label ID="lblfrm6InscricaoMunicipal" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    <table width="100%" style="font-weight: normal; font-family: Calibri; font-size: 12px;
                        border-spacing: 0; border-collapse: collapse; border: 1px solid #FFFFFF">
                        <tr style="height: 20px">
                            <td>
                                &nbsp;07 &#8211; Endereço
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td style="border-style: solid;">
                                &nbsp;<asp:Label ID="lblfrm6Endereco" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    <table width="100%" style="font-weight: normal; font-family: Calibri; font-size: 12px;
                        border-spacing: 0; border-collapse: collapse; border: 1px solid #FFFFFF">
                        <tr style="height: 20px">
                            <td>
                                &nbsp;08 &#8211; Complemento do Endereço
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;09 &#8211; Bairro / Distrito
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td style="border-style: solid;">
                                &nbsp;<asp:Label ID="lblfrm6ComplementoEndereco" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td style="border-style: solid;">
                                &nbsp;<asp:Label ID="lblfrm6Bairro" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    <table width="100%" style="font-weight: normal; font-family: Calibri; font-size: 12px;
                        border-spacing: 0; border-collapse: collapse; border: 1px solid #FFFFFF">
                        <tr style="height: 20px">
                            <td>
                                &nbsp;10 &#8211; UF
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;11 &#8211; Município
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;12 &#8211; CEP
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;13 &#8211; Caixa Postal
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td style="border-style: solid;">
                                &nbsp;<asp:Label ID="lblfrm6UF" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td style="border-style: solid;">
                                &nbsp;<asp:Label ID="lblfrm6Municipio" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td style="border-style: solid">
                                &nbsp;<asp:Label ID="lblfrm6Cep" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td style="border-style: solid;">
                                &nbsp;<asp:Label ID="lblfrm6CaixaPostal" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    <table width="100%" style="font-weight: normal; font-family: Calibri; font-size: 12px;
                        border-spacing: 0; border-collapse: collapse; border: 1px solid #FFFFFF">
                        <tr style="height: 20px">
                            <td>
                                &nbsp;14 &#8211; DDD
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;15 &#8211; Telefone
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;16 &#8211; E-mail
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td style="border-style: solid;">
                                &nbsp;<asp:Label ID="lblfrm6DDD" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td style="border-style: solid;">
                                &nbsp;<asp:Label ID="lblfrm6Telefone" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td style="border-style: solid;">
                                &nbsp;<asp:Label ID="lblfrm6Email" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="height: 15px" colspan="3">
                    &nbsp;<hr style="width: 100%;" />
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td style="height: 25px">
                    CAMPO 3 &#8211; IDENTIFICAÇÃO DO REPRESENTANTE LEGAL DO FORNECEDOR *
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 10px">
                    &nbsp;
                </td>
                <td>
                    <table width="100%" style="font-weight: normal; font-family: Calibri; font-size: 12px;
                        border-spacing: 0; border-collapse: collapse; border: 1px solid #FFFFFF">
                        <tr style="height: 20px">
                            <td>
                                &nbsp;17 &#8211; Nome
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;18 &#8211; CPF
                            </td>
                        </tr>
                        <tr style="height: 20px">
                            <td style="border-style: solid;">
                                &nbsp;<asp:Label ID="lblfrm6Nome" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td style="border-style: solid;">
                                &nbsp;<asp:Label ID="lblfrm6Cpf" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 10px">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="height: 15px" colspan="3">
                    &nbsp;
                </td>
            </tr>
        </table>
        <br />
        <i>&nbsp;&nbsp;*Informação referente ao último dia do período de prestação de contas</i>
    </div>
</asp:Content>
