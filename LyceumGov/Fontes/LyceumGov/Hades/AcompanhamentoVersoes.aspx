<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="AcompanhamentoVersoes.aspx.cs" Inherits="Techne.Lyceum.Net.Hades.AcompanhamentoVersoes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <dxwgv:ASPxGridView ID="grdVersoes" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdVersoes"
        KeyFieldName="ID_VERSAO" DataSourceID="odsVersoes" OnAfterPerformCallback="grdVersoes_AfterPerformCallback"
        OnRowValidating="grdVersoes_RowValidating" OnRowInserting="grdVersoes_RowInserting"
        OnRowUpdating="grdVersoes_RowUpdating" OnRowDeleting="grdVersoes_RowDeleting">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="EditForm" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdVersoes.AddNewRow();" alt="Novo" />
                    </div>
                </HeaderCaptionTemplate>
                <EditButton Text="Editar" Visible="True">
                    <Image Url="~/img/bt_editar.png" />
                </EditButton>
                <DeleteButton Text="Remover" Visible="True">
                    <Image Url="~/img/bt_exclui2.png" />
                </DeleteButton>
                <UpdateButton Text="Salvar">
                    <Image Url="~/img/bt_salvar.png" />
                </UpdateButton>
                <CancelButton Text="Cancelar">
                    <Image Url="~/img/bt_cancelar.png" />
                </CancelButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="ID_VERSAO" VisibleIndex="1"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Versão" FieldName="VERSAO" VisibleIndex="2">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Gestão Online?" FieldName="GESTAO_ONLINE"
                VisibleIndex="3" Width="120px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="Sim"
                    ValueType="System.String" ValueUnchecked="Não" DisplayTextUndefined="">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Docente Online?" FieldName="DOCENTE_ONLINE"
                VisibleIndex="4" Width="120px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="Sim"
                    ValueType="System.String" ValueUnchecked="Não" DisplayTextUndefined="">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Aluno Online?" FieldName="ALUNO_ONLINE" VisibleIndex="5"
                Width="120px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="Sim"
                    ValueType="System.String" ValueUnchecked="Não" DisplayTextUndefined="">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Motivo" FieldName="MOTIVO" VisibleIndex="6">
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data" FieldName="DATA_VERSAO" VisibleIndex="7">
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataMemoColumn Caption="Descrição" FieldName="DESCRICAO" VisibleIndex="8">
            </dxwgv:GridViewDataMemoColumn>
        </Columns>
        <Templates>
            <EditForm>
                <table width="100%" style="padding: 5px;">
                    <tr>
                        <td style="width: 50px;">
                            Versão:
                        </td>
                        <td>
                            <dxe:ASPxTextBox runat="server" ID="ed1" Text='<%# Bind("VERSAO") %>' Width="150">
                            </dxe:ASPxTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 50px;">
                            Gestao Online?
                        </td>
                        <td>
                            <dxe:ASPxCheckBox ID="ed2" runat="server" Value='<%# Bind("GESTAO_ONLINE") %>'>
                            </dxe:ASPxCheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 50px;">
                            Docente Online?
                        </td>
                        <td>
                            <dxe:ASPxCheckBox ID="ed3" runat="server" Value='<%# Bind("DOCENTE_ONLINE") %>'>
                            </dxe:ASPxCheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 50px;">
                            Aluno Online?
                        </td>
                        <td>
                            <dxe:ASPxCheckBox ID="ed4" runat="server" Value='<%# Bind("ALUNO_ONLINE") %>'>
                            </dxe:ASPxCheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Motivo:
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="ed5" Value='<%# Bind("MOTIVO") %>' Width="150">
                                <Items>
                                    <dxe:ListEditItem Text="(Selecione o Motivo)" Value="" />
                                    <dxe:ListEditItem Text="Implementação" Value="Implementação" />
                                    <dxe:ListEditItem Text="Alteração" Value="Alteração" />                                    
                                </Items>
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Data:
                        </td>
                        <td>
                            <dxe:ASPxDateEdit runat="server" ID="ed6" Value='<%# Bind("DATA_VERSAO") %>' Width="150">
                            </dxe:ASPxDateEdit>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="height: 20px; vertical-align: bottom;">
                            Descrição:
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <dxe:ASPxMemo runat="server" ID="ed7" Text='<%# Bind("DESCRICAO") %>' Width="100%"
                                Rows="3">
                            </dxe:ASPxMemo>
                        </td>
                    </tr>
                </table>
                <div style="text-align: right; padding: 5px;">
                    <dxwgv:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                        runat="server">
                    </dxwgv:ASPxGridViewTemplateReplacement>
                    <dxwgv:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                        runat="server">
                    </dxwgv:ASPxGridViewTemplateReplacement>
                </div>
            </EditForm>
        </Templates>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
    <asp:ObjectDataSource ID="odsVersoes" TypeName="Techne.Lyceum.Net.Hades.AcompanhamentoVersoes"
        runat="server" SelectMethod="Listar"></asp:ObjectDataSource>
</asp:Content>
