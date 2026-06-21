<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="RelatorioReplicacaoCampanha.aspx.cs" Inherits="Techne.Lyceum.Net.InspecaoEscolar.RelatorioReplicacaoCampanha" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <%--<link href="../Styles/InspecaoEscolar.css" rel="stylesheet" type="text/css" />--%>
    <asp:Label ID="lblLegenda" runat="server" Text="os campos com (*) são de preenchimeto obrigatório"
        ForeColor="Black" Font-Italic="True"></asp:Label>
    <br />
    <br />
    <br />
    <asp:Panel ID="pnlReplicacaoCampanhaOrigem" runat="server" GroupingText="Selecione a campanha de origem"
        Style="width: 858px;">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblCampanhaOrigem" runat="server" Text="Campanha de origem:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseCampanhaOrigem" runat="server" Key="campanhaid" Argument="titulo"
                        OnChanged="tseCampanhaOrigem_Changed" MaxLength="8" SqlSelect="SELECT campanhaid ,titulo, Convert(varchar(4), ano) ano, Convert(varchar(1), semestre) semestre from inspecaoescolar.campanha"
                        GridWidth="850px" SqlOrder="campanhaid" DataType="Number" AutoPostBack="false">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="ID" FieldName="campanhaid" Width="5%" />
                            <tweb:TSearchBoxColumn Caption="Título" FieldName="titulo" Width="60%" />
                            <tweb:TSearchBoxColumn Caption="Ano" FieldName="ano" Width="5%" />
                            <tweb:TSearchBoxColumn Caption="Semestre" FieldName="semestre" Width="10%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <br />
    <asp:Panel ID="pnlReplicacaoCampanhaDestino" runat="server" GroupingText="Informe os dados da campanha a ser replicada"
        Style="width: 850px;">
        <table>
            <tr>
                <td align="right">
                    <asp:Label SkinID="lblObrigatorio" ID="lblAno" runat="server" Text="Ano:*"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAno" runat="server" DataTextField="ANO" DataValueField="ANO"
                        OnSelectedIndexChanged="ddlAno_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td align="left">
                    <asp:Label ID="lblPeriodo" SkinID="lblObrigatorio" runat="server" Text="Semestre:*"></asp:Label>
                    <asp:DropDownList ID="ddlSemestre" runat="server">
                        <asp:ListItem Selected="True" Value="0">Selecione</asp:ListItem>
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblTitulo" runat="server" Text="Título:*" SkinID="lblObrigatorio"></asp:Label>
                    <asp:HiddenField ID="HiddenID" runat="server" />
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtTitulo" runat="server" MaxLength="500" TextMode="MultiLine" Height="79px"
                        Width="750px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblObjetivo" runat="server" Text="Objetivo:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtObjetivo" runat="server" MaxLength="8000" TextMode="MultiLine"
                        Height="79px" Width="750px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblProcedimento" runat="server" Text="Procedimento:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtProcedimento" runat="server" MaxLength="8000" TextMode="MultiLine"
                        Height="79px" Width="750px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblExibeInspecaoEscolar" runat="server" Text="Exibe aba Inspeção Escola:*" SkinID="lblObrigatorio"></asp:Label>
                </td>           
                <td>
                    <asp:RadioButtonList ID="rblExibeInspecaoEscolar" runat="server" RepeatDirection="Horizontal"
                        Width="150px">
                        <asp:ListItem Text="Sim" Value="true"></asp:ListItem>
                        <asp:ListItem Text="Não" Value="false"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
        <br />
    </asp:Panel>
    <br />
    <div id="divMensagem">
        <br />
        <br />
        <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem" Text="Mensagem de Teste ..."
            Font-Size="Smaller" Font-Italic="True" Font-Bold="True"></asp:Label>
    </div>
    <br />
    <table>
        <tr>
            <td>
                <asp:Button ID="btnReplicar" runat="server" Text="Replicar" Font-Size="Small" OnClick="btnReplicar_Click" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" Font-Size="Small" OnClick="btnCancelar_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
