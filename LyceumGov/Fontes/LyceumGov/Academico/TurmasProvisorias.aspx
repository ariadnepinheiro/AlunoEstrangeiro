<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="TurmasProvisorias.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.TurmasProvisorias" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style5
        {
            width: 477px;
            font-weight: 700;
        }
        .style6
        {
            text-align: center;
        }
        .style7
        {
            text-align: center;
            width: 232px;
        }
        .style8
        {
            text-align: center;
            width: 231px;
        }
        .style9
        {
            width: 264px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="" Width="617px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblUnidadeFisicaTSearch" runat="server" Text="Unidade Física:* " SkinID="lblObrigatorio"></asp:Label>
                    <br>
                    <br />
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadesFisicas" runat="server" Argument="nome_comp" Key="unidade_fis"
                        SqlSelect="SELECT unidade_fis, nome_comp FROM vw_zzcro_unidade_fisica" MaxLength="8"
                        SqlOrder="nome_comp" Enabled="false">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_fis" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome_comp" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                    <asp:HiddenField ID="hdnAno" runat="server" />
                    <asp:HiddenField ID="hdnTipoEvento" runat="server" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:Panel ID="pnTurmasProvisorias" runat="server" GroupingText="Turmas Provisórias"
        Width="617px">
        <dxwgv:ASPxGridView ID="grdTurmasProvisorias" runat="server" AutoGenerateColumns="False"
            Visible="true" ClientInstanceName="grdTurmasProvisorias" DataSourceID="odsTurmasProvisorias"
            KeyFieldName="ID" EnableCallBacks="False" OnStartRowEditing="grdTurmasProvisorias_StartRowEditing"
            OnAfterPerformCallback="grdTurmasProvisorias_AfterPerformCallback">
            <SettingsBehavior ConfirmDelete="True" />
            <SettingsEditing Mode="Inline" />
            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
            <Columns>
                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                    <DeleteButton Text="Remover" Visible="True">
                        <Image Url="~/img/bt_exclui2.png" />
                    </DeleteButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="ID" VisibleIndex="1" Visible="false"
                    ReadOnly="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="ANO" FieldName="ANO" VisibleIndex="2" ReadOnly="true"
                    Visible="true" Width="50">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="PERIODO" FieldName="PERIODO" VisibleIndex="3"
                    ReadOnly="true" Visible="true" Width="50">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="TURMA" FieldName="TURMA" VisibleIndex="4"
                    Visible="true" ReadOnly="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="CURSO" FieldName="CURSO" VisibleIndex="5"
                    Visible="true" ReadOnly="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="SERIE" FieldName="SERIE" VisibleIndex="6"
                    ReadOnly="true" Width="50">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="CENSO" FieldName="CENSO" VisibleIndex="7"
                    ReadOnly="true" Visible="false" Width="50">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="SALA" FieldName="SALA" VisibleIndex="8" ReadOnly="true"
                    Visible="true" Width="50">
                </dxwgv:GridViewDataTextColumn>
            </Columns>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        </dxwgv:ASPxGridView>
    </asp:Panel>
    <br />
    <br />
    <asp:Button ID="btnVoltar" runat="server" Text="Voltar" OnClick="btnVoltar_Click" />
    <br />
    <asp:ObjectDataSource ID="odsTurmasProvisorias" TypeName="Techne.Lyceum.Net.Academico.TurmasProvisorias"
        runat="server" SelectMethod="ListaTurma" OnDeleting="odsTurmasProvisorias_Deleting"
        DeleteMethod="Delete">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseUnidadesFisicas" DefaultValue="" Name="UnidadeEnsino" PropertyName="DBValue"/>
            <asp:ControlParameter ControlID="hdnAno" DefaultValue="" Name="Ano" PropertyName="Value" />
            <asp:ControlParameter ControlID="hdnTipoEvento" DefaultValue="" Name="TipoEvento" PropertyName="Value" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
