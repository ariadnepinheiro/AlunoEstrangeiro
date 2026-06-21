<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FichaImplantacao.aspx.cs"
    Inherits="Techne.Lyceum.Net.Basico.FichaImplantacao" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Scripts/themes/Ficha.css" rel="stylesheet" type="text/css" />
</head>
<body style="font-family: Arial; font-size: 11px">
    <form id="form1" runat="server">

    <script type="text/javascript">       
        function printpage() {
            window.print();
        }
    </script>

    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem" EnableTheming="true"> </asp:Label>
    <br />
    <div id="divPrincipal" runat="server">
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
                    FICHA DE RESUMO
                </td>
                <td align="rigth">
                    <input type="image" src="../Images/bot_imprimir.png" value="Imprimir" class="noPrint" onclick="printpage()" id="btnPrint" />
                </td>
            </tr>
        </table>
        <br />
        <br />
        <div>
            <strong>DADOS DE INGRESSO</strong></div>
        <hr id="tDadosPessoais" style="margin: 1px" />
        <table style="width: 100%; border-spacing: 0; height: 114px;" cellpadding="0" cellspacing="0"
            border="0">
            <tr>
                <td colspan="4">
                    Matricula: <strong>
                        <asp:Label runat="server" ID="lblMatricula"></asp:Label></strong>
                </td>
            </tr>
             <tr>
                <td colspan="4">
                    ID Funcional/Vínculo: <strong>
                        <asp:Label runat="server" ID="lblIDFuncionalVinculo"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label runat="server" ID="Nome" Text="Nome:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblNome"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="UnidadeAdministrativa" Text="Unidade Administrativa:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblUA"></asp:Label></strong>
                </td>
                <td colspan="2">
                    <asp:Label runat="server" ID="Municipio" Text="Município:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblMunicipioLotacao"></asp:Label></strong>
                </td>
                <td colspan="2">
                    <asp:Label runat="server" ID="DataAdmissao" Text="Data de Admissão:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblDataAdmissao"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Acumulacao" Text="Acumulação:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblAcumulacao"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="MatriculaAcumulacao" Text="Matrícula:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblMatriculaAcumulacao"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Orgao" Text="Órgão:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblOrgaoAcumulacao"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="NumProcesso" Text="Nº de Processo:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblNumProcessoAcumulacao"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td >
                    <asp:Label runat="server" ID="Disciplina" Text="Disciplina de Ingresso:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblDisciplina"></asp:Label></strong>
                </td>
                 <td colspan="3">
                    <asp:Label runat="server" ID="Label2" Text="Ano do Concurso:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblAnoConcurso"></asp:Label></strong>
                </td>
            </tr>
        </table>
        <br />
        <div>
            <strong>DADOS PESSOAIS </strong>
        </div>
        <hr id="Hr1" style="margin: 1px" />
        <table style="width: 100%; border-spacing: 0; height: 249px;" cellpadding="0" cellspacing="0"
            border="0">
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
                <td>
                    <asp:Label runat="server" ID="Etnia" Text="Cor/Raça:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblCorRaca"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="EstadoCivil" Text="Estado Civil:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblEstadoCivil"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Nacionalidade" Text="Nacionalidade:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblNacionalidade"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Naturalidade" Text="Naturalidade:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblNaturalidade"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td colspan="3" class="style3">
                    <asp:Label runat="server" ID="NomeMae" Text="Nome mãe:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblNomeMae"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label runat="server" ID="NomePai" Text="Nome Pai:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblNomePai"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="Label4" Text="Endereço:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblEndereco"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Label7" Text="Número:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblNumero"></asp:Label></strong>
                </td>
                <td colspan="2">
                    <asp:Label runat="server" ID="Label9" Text="Complemento:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblComplemento"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label11" Text="Bairro:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblBairro"></asp:Label></strong>
                </td>
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
                <td>
                    <asp:Label runat="server" ID="Label17" Text="CEP:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblCEP"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label22" Text="Número da Identidade:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblNumeroDocumento"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="OrgaoExp" Text="Órgão Expedidor:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblOrgaoIdentidade"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="DataExpedicao" Text="Data de Expedição:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblDataExpedicao"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="UFDoc" Text="UF:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblUFDoc"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label runat="server" ID="TituloEleitor" Text="Titulo de Eleitor/Zona/Seção:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblTituloEleitor"></asp:Label></strong>
                </td>
                <td colspan="2">
                    <asp:Label runat="server" ID="Certificado" Text="Certificado Reservista/Categoria/Serie:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblCertificado"></asp:Label></strong>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="Label1" Text="CPF:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblCPF"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="PISPASEP" Text="PIS/PASEP:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="LBLPISPASEP"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="CTPS" Text="CTPS:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblCTPS"></asp:Label></strong>
                </td>
                <td>
                    <asp:Label runat="server" ID="Serie" Text="Série:"></asp:Label>
                    <strong>
                        <asp:Label runat="server" ID="lblSerieCtps"></asp:Label></strong>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <asp:Label ID="Label23" runat="server" Text="Responsabilizo-me pela autenticidade de todas as informações acima firmadas, sujeitando-me às sanções da legislação, na hipótese de informações falsas ou ilegíveis."></asp:Label>
        <br />
        <br />
        <br />
        <br />
    </div>
    <div id="divAssinatura" runat="server" class="div-assinatura">
        <table align="center">
            <tr>
                <td align="right" class="style13">
                </td>
                <td class="style12">
                    ,
                </td>
                <td align="right" class="style9">
                </td>
                <td class="style10">
                    de
                </td>
                <td align="right" class="style11">
                </td>
                <td>
                    de
                    <%= DateTime.Now.Year %>
                </td>
            </tr>
            <tr>
                <td style="border-top: 2px  solid" align="center">
                </td>
                <td class="style12">
                    &nbsp;
                </td>
                <td style="border-top: 2px  solid" align="center">
                </td>
                <td style="width: 20px">
                    &nbsp;
                </td>
                <td style="border-top: 2px  solid" align="center">
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
        <br />
        <br />
        <table style="width: 62%" align="center">
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="border-top: 2px  solid" align="center">
                    Assinatura do Concursado
                </td>
            </tr>
        </table>
        <br />
        <br />
        <br />
        <table style="width: 62%" align="center">
            <tr>
                <td style="border-top: 2px  solid" align="center">
                    Assinatura Coordenador de Gestão de Pessoas
                </td>
            </tr>
        </table>
        <br />
        <br />
    </div>
    </form>
</body>
</html>
