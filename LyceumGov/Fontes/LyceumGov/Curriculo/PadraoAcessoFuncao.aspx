<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="PadraoAcessoFuncao.aspx.cs" Inherits="Techne.Lyceum.Net.Curriculo.PadraoAcessoFuncao" %>

<asp:Content ID="conPadacesFuncao" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnPadraoAcesso" runat="server" GroupingText="Informe o padrão de acesso"
        Width="640px">
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="lblPadaces" runat="server" Text="Padrão de Acesso:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tsePadrao" runat="server" Caption="" SqlSelect="SELECT padaces, nomepadaces from padaces"
                        ArgumentColumns="60" Columns="20" MaxLength="14" GridWidth="600px" SqlOrder="nomepadaces"
                        OnChanged="tsePadrao_Changed">
                        <gridcolumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="padaces" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nomepadaces" Width="70%" />
                        </gridcolumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <br />
    <br />
    <asp:ObjectDataSource ID="odsPadacesFuncao" runat="server" TypeName="Techne.Lyceum.RN.PadraoAcessoFuncao"
        SelectMethod="ListarPadacesFuncao" OnDeleting="odsPadacesFuncao_Deleting" InsertMethod="Insert"
        UpdateMethod="Update" DeleteMethod="Delete" OnInserting="odsPadacesFuncao_Inserting"
        OnUpdating="odsPadacesFuncao_Updating">
        <SelectParameters>
            <asp:ControlParameter ControlID="tsePadrao" Name="padaces" PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdPadacesFuncao" runat="server" DataSourceID="odsPadacesFuncao"
        Visible="false" ClientInstanceName="grdPadacesFuncao" KeyFieldName="padaces;funcao"
        AutoGenerateColumns="False" Width="640px" OnAfterPerformCallback="grdPadacesFuncao_AfterPerformCallback"
        OnRowInserting="grdPadacesFuncao_RowInserting" OnRowUpdating="grdPadacesFuncao_RowUpdating">
        <SettingsBehavior ConfirmDelete="True" AllowMultiSelection="False" />
        <SettingsEditing Mode="EditForm" />
        <SettingsText ConfirmDelete="Confirma a remoção ?" EmptyDataRow="Não existem dados." />
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="15%">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdPadacesFuncao.AddNewRow();" alt="Novo" />
                    </div>
                </HeaderCaptionTemplate>
                <EditButton Text="Editar" Visible="false">
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
            <dxwgv:GridViewDataTextColumn FieldName="padaces" ReadOnly="False" VisibleIndex="1"
                Caption="Padrão de Acesso*" HeaderStyle-Font-Bold="true" Width="150px" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="funcao" VisibleIndex="2" Visible="false"
                Caption="Função*" HeaderStyle-Font-Bold="true" Width="380px">
                <PropertiesTextEdit MaxLength="50" Width="380px">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="descricao" ReadOnly="False" VisibleIndex="3"
                Caption="Função*" HeaderStyle-Font-Bold="true" Width="380px">
                <PropertiesTextEdit MaxLength="50" Width="380px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o Nome." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Templates>
            <EditForm>
                <dxw:ContentControl ID="ContentControl1" runat="server">
                    <div style="padding: 4px 4px 3px 4px">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblFuncao" runat="server" Text="Função:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <tweb:TSearchBox ID="tseFuncao" runat="server" Argument="descricao" ArgumentColumns="70"
                                        FollowContainerMode="false" MaxLength="20" AutoPostBack="false" Columns="10"
                                        DataType="VarChar" Key="funcao" Value='<%# Bind("funcao") %>' SqlOrder="descricao"
                                        SqlSelect="SELECT funcao, descricao FROM Ly_funcao">
                                        <gridcolumns>
                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="funcao" Width="20%" />
                                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
                                        </gridcolumns>
                                    </tweb:TSearchBox>
                                </td>
                            </tr>
                        </table>
                        <dxwgv:ASPxGridViewTemplateReplacement ID="repUpdate" ReplacementType="EditFormUpdateButton"
                            runat="server">
                        </dxwgv:ASPxGridViewTemplateReplacement>
                        <dxwgv:ASPxGridViewTemplateReplacement ID="repCancel" ReplacementType="EditFormCancelButton"
                            runat="server">
                        </dxwgv:ASPxGridViewTemplateReplacement>
                </dxw:ContentControl>
                </div>
            </EditForm>
        </Templates>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
</asp:Content>
