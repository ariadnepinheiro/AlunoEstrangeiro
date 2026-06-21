<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Prova.aspx.cs" Inherits="Techne.Lyceum.Net.AvaliacaoExterna.Prova" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">  
    <asp:ObjectDataSource ID="odsAvaliacao" TypeName="Techne.Lyceum.Net.AvaliacaoExterna.Prova"
        runat="server" SelectMethod="ListaAvaliacao">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlAno" DbType="Int32" PropertyName="SelectedValue"
                Name="ano" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsProva" TypeName="Techne.Lyceum.Net.AvaliacaoExterna.Prova"
        runat="server" SelectMethod="Lista" InsertMethod="Insert" UpdateMethod="Update" DeleteMethod="Delete">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlAvaliacao" PropertyName="SelectedValue" DbType="Int32" Name="avaliacaoId" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" GroupingText="Avaliações" Width="775px">
                <table style="width: 750px;">
                    <tr>
                        <td style="width: 70px">
                            <asp:Label ID="lblAvaliacao" runat="server" SkinID="lblObrigatorio" Text="Avaliação:*"></asp:Label>
                        </td>
                        <td style="width: 680px;">
                            <asp:DropDownList ID="ddlAno" runat="server" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged"
                                DataTextField="ANO" DataValueField="ANO" Width="50px" AutoPostBack="true" />
                            <asp:DropDownList ID="ddlAvaliacao" runat="server" DataSourceID="odsAvaliacao"
                                DataTextField="DESCRICAO" DataValueField="AVALIACAOID" Width="300px" AutoPostBack="true" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:PlaceHolder ID="plaGrid" runat="server">
                <br />
                <br />
                <dxwgv:ASPxGridView ID="grdProva" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdProva"
                    KeyFieldName="PROVAID" DataSourceID="odsProva" OnStartRowEditing="grdProva_StartRowEditing"
                    OnInitNewRow="grdProva_InitNewRow" OnCellEditorInitialize="grdProva_CellEditorInitialize"
                    OnRowInserting="grdProva_RowInserting" OnRowUpdating="grdProva_RowUpdating" OnRowDeleting="grdProva_RowDeleting" 
                    Width="775px" OnAfterPerformCallback="grdProva_AfterPerformCallback">
                    <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                    <SettingsEditing Mode="Inline" />
                    <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                    <SettingsBehavior ConfirmDelete="true" />
                    <Columns>
                        <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="100px">
                            <HeaderCaptionTemplate>
                                <div style="text-align: center">
                                    <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                                        onclick="grdProva.AddNewRow();" />
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
                        <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="PROVAID" VisibleIndex="1" Visible="false">
                        </dxwgv:GridViewDataTextColumn>
                         <dxwgv:GridViewDataTextColumn Caption="Avaliação" Name="Avaliação" VisibleIndex="2" FieldName="AVALIACAO" ReadOnly="true">                           
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Descrição*" Name="Descricao" VisibleIndex="3" FieldName="DESCRICAO" >
                            <PropertiesTextEdit MaxLength="100">
                            </PropertiesTextEdit>
                        </dxwgv:GridViewDataTextColumn>                       
                        <dxwgv:GridViewDataSpinEditColumn Caption="Qtd. Questões*" FieldName="QUANTIDADEQUESTOES"  VisibleIndex="4">
                        </dxwgv:GridViewDataSpinEditColumn>
                        <dxwgv:GridViewDataCheckColumn Caption="Ativo*" FieldName="ATIVO" VisibleIndex="5"
                            Width="100px">
                            <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                                ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                            </PropertiesCheckEdit>
                        </dxwgv:GridViewDataCheckColumn>                 
                    </Columns>
                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                </dxwgv:ASPxGridView>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="plaZero" runat="server">
                <br />
                <br />
                <h2 style="width: 775px; text-align: center; color: Black;">
                    <b>Por favor selecione uma avaliação no painel acima</b></h2>
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
