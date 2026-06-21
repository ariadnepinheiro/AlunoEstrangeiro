<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ListarNotificacaoControle.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.ListarNotificacaoControle" %>

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
                    <tweb:TSearch ID="tseAluno" runat="server" OnTextChanged="tseAluno_Changed" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoNotificacao"
                        AutoPostBack="true">
                    </tweb:TSearch>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:HiddenField ID="hdnAluno" runat="server" />
    <div class="divEditBlock" style="width: 850px;">
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:Label runat="server" ID="lblBlocoAluno" Text="Notificações de Órgãos de Controle"
            SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsAlunos" runat="server" EnableClientScript="true" ShowMessageBox="true"
            ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <asp:ObjectDataSource ID="odsNotificacao" TypeName="Techne.Lyceum.Net.Academico.ListarNotificacaoControle"
        runat="server" SelectMethod="Listar">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseAluno" DefaultValue="" Name="alunoFiltro" PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdAlunos" runat="server" AutoGenerateColumns="False" SkinID="NoConfirmDelete"
        DataSourceID="odsNotificacao" ClientInstanceName="grdAlunos" KeyFieldName="NOTIFICACAOID"
        OnAfterPerformCallback="grdAlunos_AfterPerformCallback" OnSelectionChanged="grdAlunos_SelectionChanged">
        <SettingsBehavior ProcessSelectionChangedOnServer="True" AllowSort="false" />
        <SettingsEditing Mode="Inline" />
        <SettingsText EmptyDataRow="Não existem dados." ConfirmDelete="Confirma a remoção?" />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <SelectButton Text="Selecionar" Visible="True">
                    <Image Url="~/img/bt_busca.png" />
                </SelectButton>
                <ClearFilterButton Text="Limpar" Visible="True">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataTextColumn Caption="NOTIFICACAOID" FieldName="NOTIFICACAOID" VisibleIndex="1"
                Visible="False">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Nº FICAI" FieldName="NUMEROFICAI" VisibleIndex="2"
                ReadOnly="true" CellStyle-HorizontalAlign="Center">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Nº FAMI" FieldName="NUMEROFAMI" VisibleIndex="3"
                ReadOnly="true" CellStyle-HorizontalAlign="Center">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Data da Comunicação" FieldName="DATACOMUNICACAO"
                VisibleIndex="4" ReadOnly="true" CellStyle-HorizontalAlign="Center">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Quantidade de Faltas" FieldName="QUANTIDADEFALTAS"
                VisibleIndex="5" ReadOnly="true" CellStyle-HorizontalAlign="Center">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Servidor" FieldName="SERVIDOR" VisibleIndex="6">
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
</asp:Content>
