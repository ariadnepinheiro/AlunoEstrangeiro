<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="AtualizaDadosAluno.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.AtualizaDadosAluno" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe a matrícula ou o nome do aluno"
        Width="580px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblAlunoTSearch" runat="server" Text="Aluno:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoConfRenovacao"
                        AutoPostBack="true" OnTextChanged="tseAluno_Changed">
                    </tweb:TSearch>
                </td>
            </tr>
        </table>   
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem" ClientInstancename="lblMensagem"></asp:Label>
    <br />
    <br />
    <div class="divEditBlock" style="width: 850px;">
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click" />
    </div>
    <asp:Panel ID="pnlDados" GroupingText="Dados do Aluno" runat="server" Width="60%">
        <table>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="Label3" runat="server" Text="Nome do Aluno:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:Label ID="lblNomeAluno" runat="server" SkinID="lblObrigatorio"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="Label4" runat="server" Text="Nome da Mãe:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:Label ID="lblNomeMae" runat="server" SkinID="lblObrigatorio"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label5" runat="server" Text="Data Nascimento:"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblDataNascimento" runat="server" SkinID="lblObrigatorio"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label1" runat="server" Text="Situação Aluno:"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblSituacao" runat="server" SkinID="lblObrigatorio"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblCPF" runat="server" Text="CPF:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCPF" runat="server" Width="80px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
