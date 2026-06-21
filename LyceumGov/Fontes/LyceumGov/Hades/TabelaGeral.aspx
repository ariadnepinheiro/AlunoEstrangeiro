<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modulos/LyceumMaster.Master"
    CodeBehind="TabelaGeral.aspx.cs" Inherits="Techne.Lyceum.Net.Hades.TabelaGeral" %>

<asp:Content ID="contabelaGeral" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);
        function endRequest(sender, e) {
            if (e.get_error()) {
                document.getElementById("<%=lblMensagem.ClientID %>").innerText = e.get_error().description.replace(e.get_error().name + ": ", "");
                e.set_errorHandled(true);
            }
            else {
                document.getElementById("<%=lblMensagem.ClientID %>").innerText = "";
            }
        }
                
    </script>

    <asp:ScriptManagerProxy ID="manager" runat="server"/>
    <techne:TTableDataSource ID="tdsTabela" runat="server" DataTableClassName="Techne.HadesLyc.CR.Hd_tabela"
        OnSelecting="tdsTabela_Selecting">
    </techne:TTableDataSource>
    <techne:TTableDataSource ID="tdsItem" runat="server" DataTableClassName="Techne.HadesLyc.CR.Hd_tabelaitem">
    </techne:TTableDataSource>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <asp:Label ID="lblSelecione" runat="server" Text="Selecione uma linha da tabela abaixo:"></asp:Label>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <dxwgv:ASPxGridView ID="grdTabela" runat="server" EnableRowsCache="False" DataSourceID="tdsTabela"
                EnableViewState="False" EnableCallBacks="false" ClientInstanceName="grdTabela"
                AutoGenerateColumns="False" KeyFieldName="tabela" OnCellEditorInitialize="grdTabela_CellEditorInitialize"
                OnFocusedRowChanged="grdTabela_FocusedRowChanged" OnInitNewRow="grdTabela_InitNewRow"
                OnStartRowEditing="grdTabela_StartRowEditing" OnRowInserting="grdTabela_RowInserting"
                Width="1128px" OnCancelRowEditing="grdTabela_CancelRowEditing" OnRowDeleted="grdTabela_RowDeleted"
                OnRowInserted="grdTabela_RowInserted" OnRowUpdated="grdTabela_RowUpdated" OnRowDeleting="grdTabela_RowDeleting"
                OnCustomJSProperties="grdTabela_CustomJSProperties" OnAfterPerformCallback="grdTabela_AfterPerformCallback">
                <SettingsBehavior AllowMultiSelection="False" AllowFocusedRow="True" ProcessFocusedRowChangedOnServer="true" />
                <SettingsText EmptyDataRow="Não existem dados." />
                <%--<ClientSideEvents Init="function(s) {if(s.cpUpdateError) s.ShowError(s.cpUpdateError);}" />--%>
                <SettingsBehavior ConfirmDelete="True" AllowMultiSelection="False" />
                <SettingsEditing Mode="EditForm" />
                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                <Templates>
                    <EditForm>
                        <div style="text-align: left; padding: 2px 2px 2px 2px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblTabelaTSearch" runat="server" Text="Tabela:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <dxwgv:ASPxGridViewTemplateReplacement ID="ASPxGridViewTemplateReplacement0" ColumnID="tabela"
                                            ReplacementType="EditFormCellEditor" runat="server">
                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDescricaoTSearch" runat="server" Text="Descrição:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <dxwgv:ASPxGridViewTemplateReplacement ID="ASPxGridViewTemplateReplacement1" ColumnID="descr"
                                            ReplacementType="EditFormCellEditor" runat="server">
                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                    </td>
                                </tr>
                            </table>
                            <dxwgv:ASPxGridViewTemplateReplacement ID="repUpdate" ReplacementType="EditFormUpdateButton"
                                runat="server">
                            </dxwgv:ASPxGridViewTemplateReplacement>
                            <dxwgv:ASPxGridViewTemplateReplacement ID="repCancel" ReplacementType="EditFormCancelButton"
                                runat="server">
                            </dxwgv:ASPxGridViewTemplateReplacement>
                        </div>
                    </EditForm>
                </Templates>
                <Columns>
                    <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                        <HeaderCaptionTemplate>
                            <div style="text-align: center">
                                <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                    onclick="grdTabela.AddNewRow();" alt="Novo" />
                            </div>
                        </HeaderCaptionTemplate>
                        <EditButton Text="Editar" Visible="True">
                            <Image Url="~/img/bt_editar.png" />
                        </EditButton>
                        <DeleteButton Text="Remover" Visible="True">
                            <Image Url="~/img/bt_exclui2.png" />
                        </DeleteButton>
                        <CancelButton Text="Cancelar">
                            <Image Url="~/img/bt_cancelar.png" />
                        </CancelButton>
                        <UpdateButton>
                            <Image Url="~/img/bt_salvar.png" />
                        </UpdateButton>
                        <ClearFilterButton Text="Limpar" Visible="True">
                            <Image Url="~/img/bt_limpa.png" />
                        </ClearFilterButton>
                    </dxwgv:GridViewCommandColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="tabela" VisibleIndex="1" Caption="Tabela"
                        Width="100">
                        <PropertiesTextEdit MaxLength="20" Width="200px">
                            <ClientSideEvents KeyPress="function (s, e){ SomentePermitirCodigoEspaco(s, e.htmlEvent); }" />
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                <RequiredField IsRequired="true" ErrorText="Campo obrigatório!" />
                            </ValidationSettings>
                        </PropertiesTextEdit>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="sis" VisibleIndex="2" Caption="Sistema"
                        Visible="false" Width="100">
                        <PropertiesTextEdit MaxLength="12" Width="200px">
                        </PropertiesTextEdit>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="descr" VisibleIndex="3" Caption="Descrição"
                        Width="400">
                        <PropertiesTextEdit MaxLength="160" Width="1000px">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                <RequiredField ErrorText="Favor inserir a descrição." IsRequired="True" />
                            </ValidationSettings>
                        </PropertiesTextEdit>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="taminfo" VisibleIndex="4" Caption="Tamanho"
                        Width="70" Visible="false">
                        <PropertiesTextEdit MaxLength="2" Width="200px">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                <RegularExpression ValidationExpression="^[+]?\d*$" ErrorText="Tamanho deve ser um número entre 0 e 2 dígitos." />
                            </ValidationSettings>
                        </PropertiesTextEdit>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataComboBoxColumn Caption="Tipo" FieldName="tipo" VisibleIndex="5"
                        Width="110px" Visible="false">
                        <PropertiesComboBox ValueType="System.String" Width="200px">
                            <Items>
                                <dxe:ListEditItem Text="Alfanumérico" Value="A" />
                                <dxe:ListEditItem Text="Numérico" Value="N" />
                            </Items>
                        </PropertiesComboBox>
                    </dxwgv:GridViewDataComboBoxColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="nome_tabela" VisibleIndex="6" Caption="Nome"
                        Width="70" Visible="false">
                        <PropertiesTextEdit MaxLength="50" Width="380px">
                        </PropertiesTextEdit>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="coluna_tabela" VisibleIndex="7" Caption="Coluna"
                        Width="70" Visible="false">
                        <PropertiesTextEdit MaxLength="50" Width="380px">
                        </PropertiesTextEdit>
                    </dxwgv:GridViewDataTextColumn>
                </Columns>
            </dxwgv:ASPxGridView>
            <br />
            <asp:Label ID="lblTabela" runat="server"></asp:Label>
            <br />
            <br />
            <dxwgv:ASPxGridView runat="server" ID="grdItem" ClientInstanceName="detailGridView"
                DataSourceID="tdsItem" AutoGenerateColumns="False" KeyFieldName="CompositeKey"
                OnCellEditorInitialize="grdItem_CellEditorInitialize" OnCustomUnboundColumnData="grdItem_CustomUnboundColumnData"
                OnRowDeleting="grdItem_RowDeleting" OnRowInserting="grdItem_RowInserting" OnRowUpdating="grdItem_RowUpdating"
                OnInitNewRow="grdItem_InitNewRow" OnStartRowEditing="grdItem_StartRowEditing"
                Width="1128px" OnRowValidating="grdItem_RowValidating" OnCancelRowEditing="grdItem_CancelRowEditing"
                OnRowDeleted="grdItem_RowDeleted" OnRowInserted="grdItem_RowInserted" OnRowUpdated="grdItem_RowUpdated"
                EnableCallBacks="true" OnAfterPerformCallback="grdItem_AfterPerformCallback">
                <SettingsBehavior ConfirmDelete="True" AllowMultiSelection="False" />
                <SettingsEditing Mode="Inline" />
                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                <Columns>
                    <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                        <HeaderCaptionTemplate>
                            <div style="text-align: center">
                                <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                    onclick="detailGridView.AddNewRow();" alt="Novo" />
                            </div>
                        </HeaderCaptionTemplate>
                        <EditButton Text="Editar" Visible="True">
                            <Image Url="~/img/bt_editar.png" />
                        </EditButton>
                        <DeleteButton Text="Remover" Visible="True">
                            <Image Url="~/img/bt_exclui2.png" />
                        </DeleteButton>
                        <CancelButton Text="Cancelar">
                            <Image Url="~/img/bt_cancelar.png" />
                        </CancelButton>
                        <UpdateButton>
                            <Image Url="~/img/bt_salvar.png" />
                        </UpdateButton>
                        <ClearFilterButton Text="Limpar" Visible="True">
                            <Image Url="~/img/bt_limpa.png" />
                        </ClearFilterButton>
                    </dxwgv:GridViewCommandColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="tabela" ReadOnly="true" Visible="true" Caption="Tabela*"
                        VisibleIndex="1" HeaderStyle-Font-Bold="true" Width="100px">
                        <PropertiesTextEdit MaxLength="20" Width="100px">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                <RequiredField IsRequired="true" ErrorText="A tabela selecionada foi removida por outro usuário. Favor recarregar a página." />
                            </ValidationSettings>
                        </PropertiesTextEdit>
                        <HeaderStyle Font-Bold="True" />
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="item" ReadOnly="False" Visible="true" Caption="Item*"
                        HeaderStyle-Font-Bold="true" VisibleIndex="2" Width="200px">
                        <PropertiesTextEdit MaxLength="40" Width="200px">
                         <ClientSideEvents KeyPress="function (s, e){ SomentePermitirCodigo(s, e.htmlEvent); }" />
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                <RequiredField IsRequired="true" ErrorText="Campo obrigatório." />
                            </ValidationSettings>
                        </PropertiesTextEdit>
                        <HeaderStyle Font-Bold="True" />
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="descr" ReadOnly="False" Visible="true" Caption="Descrição*"
                        HeaderStyle-Font-Bold="true" VisibleIndex="3" Width="828px">
                        <PropertiesTextEdit MaxLength="180" Width="828px">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                <RequiredField IsRequired="true" ErrorText="Campo obrigatório." />
                            </ValidationSettings>
                        </PropertiesTextEdit>
                        <HeaderStyle Font-Bold="True" />
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                        Visible="False" VisibleIndex="4">
                    </dxwgv:GridViewDataTextColumn>
                </Columns>
                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
            </dxwgv:ASPxGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
