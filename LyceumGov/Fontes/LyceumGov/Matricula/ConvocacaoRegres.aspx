<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ConvocacaoRegres.aspx.cs" Inherits="Techne.Lyceum.Net.Matricula.ConvocacaoRegres" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <div id="dvbloqueio" class="Desbloqueado">
    </div>
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por disciplina"
        Width="50%">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblAno" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAno" runat="server" DataTextField="ano" DataValueField="ano"
                        Width="70px" AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblCursoTSearch" runat="server" Text="Disciplina:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseCurso" runat="server" ColumnName="CURSO" MaxLength="20" SqlSelect="SELECT CURSO, NOME FROM ly_curso "
                        OnChanged="tseCurso_Changed" SqlOrder="nome" SqlWhere="CURSO IN ('9999.81','9999.82','9999.83','9999.84','9999.85','9999.86','9999.87')">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="CURSO" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="NOME" Width="70%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 120px">
                    <asp:Label ID="lblTurno" runat="server" Text="Turno:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlTurno" AutoPostBack="False" runat="server" DataTextField="descricao"
                        DataValueField="turno" AppendDataBoundItems="true">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <br />
        <asp:ImageButton ID="btnBuscar" runat="server" SkinID="BcBuscar" OnClick="btnBuscar_Click" />
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:Panel ID="pnlDados" runat="server" GroupingText="Informações para convocação"
        Width="50%" Visible="false">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblVagasDisponiveisTexto" Text="Total de Vagas Disponíveis:" runat="server"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblVagasDisponiveis" runat="server" SkinID="lblObrigatorio"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblTotalFilaTexto" Text="Total de Alunos na Fila:" runat="server"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblTotalFila" runat="server" SkinID="lblObrigatorio"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <asp:Button ID="btnConvocar" runat="server" Text="Convocar" OnClientClick="Bloqueio();"  OnClick="btnConvocar_Click" />
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlAviso" runat="server" GroupingText="Avisos" Width="50%" Visible="false">
        <br />
        <asp:Label ID="lblAviso" runat="server" SkinID="lblMensagem"></asp:Label>
        <br />
    </asp:Panel>
</asp:Content>
