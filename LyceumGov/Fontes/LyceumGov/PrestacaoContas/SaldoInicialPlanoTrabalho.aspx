<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="SaldoInicialPlanoTrabalho.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.SaldoInicialPlanoTrabalho" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript" src="../Scripts/jquery-ui.js"></script>

    <script src="../Scripts/jquery.maskedinput-1.2.2.min.js" type="text/javascript"></script>

    <script type="text/javascript">

        function moeda(a, e, r, t) {

            var n = "", h = j = 0, u = tamanho2 = 0, l = ajd2 = "", o = window.Event ? t.which : t.keyCode;
            if (13 == o || 8 == o)
                return !0;
            if (n = String.fromCharCode(o),
    -1 == "0123456789".indexOf(n))
                return !1;
            for (u = a.value.length,
    h = 0; h < u && ("0" == a.value.charAt(h) || a.value.charAt(h) == r); h++)
                ;
            for (l = ""; h < u; h++)
-1 != "0123456789".indexOf(a.value.charAt(h)) && (l += a.value.charAt(h));
            if (l += n,
    0 == (u = l.length) && (a.value = ""),
    1 == u && (a.value = "0" + r + "0" + l),
    2 == u && (a.value = "0" + r + l),
    u > 2) {
                for (ajd2 = "",
        j = 0,
        h = u - 3; h >= 0; h--)
                    3 == j && (ajd2 += e,
            j = 0),
            ajd2 += l.charAt(h),
            j++;
                for (a.value = "",
        tamanho2 = ajd2.length,
        h = tamanho2 - 1; h >= 0; h--)
                    a.value += ajd2.charAt(h);
                a.value += r + l.substr(u - 2, u)
            }
            return !1
        }


        function abrirPopup() {

            window.setTimeout(function() {
                alert('OK');
                pucConfirmar.Show();
            }, 1000);
        }
    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informações do Projeto / Programa"
        Width="817px">
        <table style="width: 600px">
            <tr>
                <td align="left">
                    <asp:Label ID="lblForncedor" runat="server" Text="Unidade de Ensino:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                        EnableViewState="true" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,municipio, id_regional from VW_UNIDADE_ENSINO_SITUACAO "
                        ArgumentColumns="75" Columns="10" OnChanged="tseUnidadeResponsavel_Changed" GridWidth="850px"
                        SqlOrder="nome_comp">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="setor" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="CGC" FieldName="cgc" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Municipio" FieldName="municipio" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="id_regional" Width="15%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: left">
                    <asp:Label ID="lblPlanoTrabalho" runat="server" Font-Names="Verdana" Text="Projeto / Programa*:"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="5">
                    <tweb:TSearchBox ID="tsePlanoTrabalho" runat="server" Argument="descricao" OnChanged="tsePlanoTrabalho_Changed"
                        Key="PLANOTRABALHOID" MaxLength="9" DataType="Number" SqlSelect=" select pt.descricao from  PrestacaoContas.PLANOTRABALHO pt inner join PrestacaoContas.PROGRAMATRABALHO po on pt.PROGRAMATRABALHOID = po.PROGRAMATRABALHOID inner join PrestacaoContas.WSPROGRAMASEFAZ p on po.WSPROGRAMASEFAZID = p.WSPROGRAMASEFAZID ">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="PLANOTRABALHOID" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="DESCRICAO" Width="90%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:HiddenField ID="hdnSaldoInicialId" runat="server" />
    <asp:HiddenField ID="hdnPodeZerar" runat="server" />
    <br />
    <div class="divEditBlock" style="width: 950px;">
        <asp:ImageButton ID="btnZerarSaldo" runat="server" SkinID="BcDeletar"  OnClick="btnZerarSaldo_Click"  />
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click" />
        <asp:ValidationSummary ID="vsFornecedor" runat="server" EnableClientScript="true"
            ShowMessageBox="true" ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <br />
    <div id="divDados" style="width: 950px;" visible="false" runat="server">
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="Label3" runat="server" Font-Names="Verdana" Text="Saldo Inicial*:"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td align="right">
                    <asp:TextBox ID="txtValorInicial" runat="server" MaxLength="18" Font-Names="Verdana"
                        OnKeyPress="return(moeda(this,'.',',',event))"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label2" runat="server" Font-Names="Verdana" Text="Data de Referência para Cálculo do Saldo*:"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td align="right">
                    <asp:TextBox ID="txtDataReferencia" runat="server" Font-Names="Verdana" onkeyup="formataData(this,event)"
                        MaxLength="10"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <table>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Font-Names="Verdana" Text="Saldo Inicial: Saldo Projeto/Programa anterior a utilização do módulo de prestação de contas."></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" Font-Names="Verdana" Text="Data de Referência para Cálculo do Saldo: Data a partir da qual serão considerados os registros ao realizar o cálculo do saldo de um Projeto/Programa."></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <dxpc:ASPxPopupControl ID="pucConfirmar" ClientInstanceName="pucConfirmar" runat="server"
        Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false" ShowCloseButton="false"
        ShowFooter="false" ShowHeader="false" ShowSizeGrip="False" EnableAnimation="false"
        Width="300px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <table>
                    <tr align="center">
                        <td>
                            Confirma zerar o saldo inicial do Projeto / Programa?<br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Button ID="btnSim" runat="server" Text="Sim" OnClick="btnSim_Click" />
                            <asp:Button ID="btnNao" runat="server" Text="Não" OnClick="btnNao_Click" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
