<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="HistoricoAlteracaoDadosAlunos.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.HistoricoAlteracaoDadosAlunos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe a matrícula ou o nome do aluno"
        Height="45px" Width="580px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblAlunoTSearch" runat="server" Text="Aluno:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseAluno" runat="server" OnTextChanged="tseAluno_Changed" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAluno"
                        AutoPostBack="true">
                    </tweb:TSearch>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:HiddenField ID="hdnCompartilhada" runat="server" />
    <asp:HiddenField ID="hdnAno" runat="server" />
    <asp:HiddenField ID="hdnPeriodo" runat="server" />
    <asp:HiddenField ID="hdnCurso" runat="server" />
    <asp:HiddenField ID="hdnSerie" runat="server" />
    <br />
    <asp:Panel ID="pnDados" GroupingText="" runat="server">
        <table>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblUniEnsino" runat="server" Text="Unidade de Ensino:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtUniEnsino" runat="server" MaxLength="100" Width="600px" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblSituacao" runat="server" Text="Situação:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtSituacao" runat="server" MaxLength="15" Width="600px" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblCurso" runat="server" Text="Escolaridade:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCurso" runat="server" MaxLength="20" Width="50px" ReadOnly="true"
                        Visible="false" />
                    <asp:TextBox ID="txtNomeCurso" runat="server" MaxLength="20" Width="200px" ReadOnly="true" />
                </td>
                <td style="text-align: right">
                    <asp:Label ID="lblTurno" runat="server" Text="Turno:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtTurno" runat="server" MaxLength="20" Width="50px" ReadOnly="true"
                        Visible="false" />
                    <asp:TextBox ID="txtNomeTurno" runat="server" MaxLength="20" Width="200px" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblCurriculo" runat="server" Text="Currículo:" Visible="false"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtCurriculo" runat="server" MaxLength="20" Width="600px" ReadOnly="true"
                        Visible="false" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblSerie" runat="server" Text="Ano de Escolaridade:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtSerie" runat="server" MaxLength="3" Width="50px" ReadOnly="true"
                        Visible="false" />
                    <asp:TextBox ID="txtNomeSerie" runat="server" MaxLength="3" Width="600px" ReadOnly="true" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <dxwgv:ASPxGridView ID="grdEncerramentos" runat="server" AutoGenerateColumns="False"
        OnPageIndexChanged="grdEncerramentos_PageIndexChanged" SkinID="NoConfirmDelete"
        ClientInstanceName="grdEncerramentos" KeyFieldName="HISTORICOALTERACAOALUNO_CAMPOSID"
        OnCustomUnboundColumnData="grdEncerramentos_CustomUnboundColumnData">
        <SettingsBehavior ProcessSelectionChangedOnServer="True" AllowSort="false" />
        <SettingsEditing Mode="Inline" />
        <SettingsText EmptyDataRow="Não existem dados." ConfirmDelete="Confirma a remoção?" />
        <Columns>            
            <dxwgv:GridViewDataTextColumn Caption="Campo Alterado" FieldName="CAMPO" VisibleIndex="1"
                ReadOnly="true" Width ="200px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Informação Anterior" FieldName="VALORANTERIOR"
                VisibleIndex="2" ReadOnly="true">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Informação Atual" FieldName="VALORATUAL" VisibleIndex="3"
                ReadOnly="true">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data da Alteração" FieldName="DATAALTERACAO"
                VisibleIndex="4" ReadOnly="true" CellStyle-HorizontalAlign="Center">
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataTextColumn Caption="Responsável" FieldName="USUARIO" VisibleIndex="5"
                ReadOnly="true" Width ="300px">
            </dxwgv:GridViewDataTextColumn>
        </Columns>
    </dxwgv:ASPxGridView>
</asp:Content>
