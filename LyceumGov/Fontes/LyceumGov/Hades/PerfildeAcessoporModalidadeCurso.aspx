<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="PerfildeAcessoporModalidadeCurso.aspx.cs" Inherits="Techne.Lyceum.Net.Hades.PerfildeAcessoporModalidadeCurso"
     %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnPadraoAcesso" runat="server" GroupingText="Faça uma busca por perfil"
        Width="640px">
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="lblPerfil" runat="server" Text="Perfil:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tsePerfil" runat="server" Caption="" SqlSelect="SELECT id_perfil,descricao from TCE_PERFIL"
                        Key="id_perfil" DataType="Number" ArgumentColumns="60" Columns="20" MaxLength="14"
                        GridWidth="600px" SqlOrder="id_perfil" Connection="Hades">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="id_perfil" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="70%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
        <br/>
        <br/>
        <asp:Panel ID="pnGrid" runat="server">
            <dxwgv:ASPxGridView ID="grdPerfilModalidade" runat="server" AutoGenerateColumns="False"
                ClientInstanceName="grdPerfilModalidade" KeyFieldName="PERFILMODALIDADEID" DataSourceID="odsPerfilModalidade"
                OnStartRowEditing="grdPerfilModalidade_StartRowEditing" OnAfterPerformCallback="grdPerfilModalidade_AfterPerformCallback" 
                OnCellEditorInitialize="grdPerfilModalidade_CellEditorInitialize" OnRowValidating="grdPerfilModalidade_RowValidating" Width="500">
                <SettingsBehavior ConfirmDelete="True" AllowMultiSelection="False" />
                <SettingsEditing Mode="Inline" />
                <SettingsText ConfirmDelete="Confirma remoção?" EmptyDataRow="Não existem dados." />
                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                <Columns>
                    <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="15%">
                        <HeaderCaptionTemplate>
                            <div style="text-align: center">
                                <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                    onclick="grdPerfilModalidade.AddNewRow();" alt="Novo" />
                            </div>
                        </HeaderCaptionTemplate>
               
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
                    <dxwgv:GridViewDataTextColumn FieldName="PERFILMODALIDADEID" ReadOnly="False" VisibleIndex="1"
                        Caption="PERFILMODALIDADEID" HeaderStyle-Font-Bold="true" Width="80px" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataComboBoxColumn Caption="Modalidade" FieldName="MODALIDADE_DESCRICAO" VisibleIndex="2">
                     <Settings FilterMode="DisplayText" />
                        <PropertiesComboBox DataSourceID="odsModalidade" TextField="DESCR_MODALIDADE" ValueField="MODALIDADE"
                            ValueType="System.String" ClientInstanceName="MODALIDADE" DropDownWidth="300px"
                    Width="200px" EnableSynchronization="False" EnableIncrementalFiltering="True">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                <RequiredField ErrorText="Favor informar a Modalidade." IsRequired="True" />
                            </ValidationSettings>
                        </PropertiesComboBox>
                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                    </dxwgv:GridViewDataComboBoxColumn>
                    
                </Columns>
                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
            </dxwgv:ASPxGridView>
            <asp:ObjectDataSource ID="odsPerfilModalidade" TypeName="Techne.Lyceum.Net.Hades.PerfildeAcessoporModalidadeCurso"
                runat="server" SelectMethod="Listar" OnDeleting="odsPadraoPerfil_Deleting" DeleteMethod="Delete"
                OnInserting="odsPadraoPerfil_Inserting" InsertMethod="Insert">
                <SelectParameters>
                    <asp:ControlParameter ControlID="tsePerfil" Name="id_perfil" PropertyName="DBValue" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="odsModalidade" runat="server" SelectMethod="ListarModalidadePerfil"
                TypeName="Techne.Lyceum.RN.Curso">
                <SelectParameters>
                    <asp:ControlParameter ControlID="tsePerfil" Name="perfilid" PropertyName="DBValue" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </asp:Panel>
    </asp:Panel>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
</asp:Content>
