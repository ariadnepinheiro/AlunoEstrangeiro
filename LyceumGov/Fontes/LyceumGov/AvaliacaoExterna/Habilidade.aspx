<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true" CodeBehind="Habilidade.aspx.cs" Inherits="Techne.Lyceum.Net.AvaliacaoExterna.Habilidade" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    
    <asp:ObjectDataSource ID="objComponente" TypeName="Techne.Lyceum.Net.AvaliacaoExterna.Habilidade" runat="server" SelectMethod="ListaComponente" />
    
    <asp:ObjectDataSource ID="odsHabilidade" TypeName="Techne.Lyceum.Net.AvaliacaoExterna.Habilidade" runat="server" 
        SelectMethod="Lista" InsertMethod="Insert" UpdateMethod="Update" DeleteMethod="Delete">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlComponente" PropertyName="SelectedValue" DbType="Int32" Name="componenteId" />
        </SelectParameters>
    </asp:ObjectDataSource>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    
            <asp:Panel runat="server" GroupingText="Componente" Width="775px">
                <table style="width: 750px;">
                    <tr>
                        <td style="width: 70px;">
                            <asp:Label ID="lblComponente" runat="server" SkinID="lblObrigatorio" Text="Componente:*"></asp:Label>
                        </td>
                        <td style="width: 680px;">
                            <asp:DropDownList ID="ddlComponente" runat="server" DataSourceID="objComponente" 
                            DataTextField="DESCRICAO" DataValueField="COMPONENTEID" Width="300px" AutoPostBack="true"
                            OnDataBound="ddlComponente_DataBound" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: right;">
                            <asp:Button ID="cmdVoltar" runat="server" Text="Voltar para componentes" PostBackUrl="~/AvaliacaoExterna/Componente.aspx" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            
            <asp:PlaceHolder ID="plaGrid" runat="server">
            
            <br /><br />
            
            <dxwgv:ASPxGridView ID="grdHabilidade" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdHabilidade" 
                DataSourceID="odsHabilidade" KeyFieldName="HABILIDADEID"
                OnStartRowEditing="grdHabilidade_StartRowEditing" OnInitNewRow="grdHabilidade_InitNewRow" 
                OnRowInserting="grdHabilidade_RowInserting" OnRowUpdating="grdHabilidade_RowUpdating" 
                OnRowDeleting="grdHabilidade_RowDeleting" Width="775px" OnAfterPerformCallback="grdHabilidade_AfterPerformCallback"	>
                
                <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                <SettingsEditing Mode="Inline" />
                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                <SettingsBehavior ConfirmDelete="true" />
                <Columns>
                    <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="100px">
                        <HeaderCaptionTemplate>
                            <div style="text-align: center">
                                <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                                    onclick="grdHabilidade.AddNewRow();" />
                            </div>
                        </HeaderCaptionTemplate>
                        <CancelButton Visible="true" Text="Cancelar">
                            <Image Url="~/img/bt_cancelar.png" />
                        </CancelButton>
                        <EditButton Visible="True" Text="Editar">
                            <Image Url="../img/bt_editar.png" />
                        </EditButton>
                        <DeleteButton Visible="True" Text="Remover">
                            <Image Url="../img/bt_exclui2.png" />
                        </DeleteButton>
                        <ClearFilterButton Text="Limpar" Visible="True">
                            <Image Url="~/img/bt_limpa.png" />
                        </ClearFilterButton>
                        <UpdateButton Visible="true" Text="Alterar">
                            <Image Url="../img/bt_salvar.png" />
                        </UpdateButton>
                    </dxwgv:GridViewCommandColumn>
                    <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="HABILIDADEID" VisibleIndex="1" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Código*" VisibleIndex="2" FieldName="CODIGO" Width="50px">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Descrição*" VisibleIndex="3" FieldName="DESCRICAO" Width="525px">
                    </dxwgv:GridViewDataTextColumn>
                    <%--<dxwgv:GridViewDataCheckColumn Caption="Ativo" FieldName="ATIVO" VisibleIndex="4" Width="100px">
                        <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1" ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                        </PropertiesCheckEdit>
                    </dxwgv:GridViewDataCheckColumn>--%>
                </Columns>
                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
            </dxwgv:ASPxGridView>
            
            </asp:PlaceHolder>
            
            <asp:PlaceHolder ID="plaZero" runat="server">
            
            <br /><br />
            <h2 style="width: 775px; text-align: center; color: Black;"><b>Por favor selecione um componente no painel acima</b></h2>
            
            </asp:PlaceHolder>
    
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <asp:Panel ID="Panel1" CssClass="overlay" runat="server">
                <asp:Panel ID="Panel2" CssClass="loader" runat="server">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/updateProgress.gif" AlternateText="Updating..."
                        Height="48" Width="48" />
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
    
</asp:Content>