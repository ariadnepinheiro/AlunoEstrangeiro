<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Etapa.aspx.cs" Inherits="Techne.Lyceum.Net.AvaliacaoExterna.Etapa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:ObjectDataSource ID="odsAno" TypeName="Techne.Lyceum.Net.AvaliacaoExterna.Etapa"
        runat="server" SelectMethod="ListaAnos" />
    <asp:ObjectDataSource ID="odsAvaliacao" TypeName="Techne.Lyceum.Net.AvaliacaoExterna.Etapa"
        runat="server" SelectMethod="ListaAvaliacao">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlAno" DbType="Int32" PropertyName="SelectedValue"
                Name="ano" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsProva" TypeName="Techne.Lyceum.Net.AvaliacaoExterna.Etapa"
        runat="server" SelectMethod="ListaProva">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlAvaliacao" DbType="Int32" PropertyName="SelectedValue"
                Name="avaliacaoId" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsSerie" TypeName="Techne.Lyceum.Net.AvaliacaoExterna.Etapa"
        runat="server" SelectMethod="ListaSeries" />
    <asp:ObjectDataSource ID="odsCurso" TypeName="Techne.Lyceum.Net.AvaliacaoExterna.Etapa"
        runat="server" SelectMethod="ListaCurso">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlAno" DbType="Int32" PropertyName="SelectedValue"
                Name="ano" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsEtapa" TypeName="Techne.Lyceum.Net.AvaliacaoExterna.Etapa"
        runat="server" SelectMethod="Lista" InsertMethod="Insert" UpdateMethod="Update"
        DeleteMethod="Delete">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlProva" PropertyName="SelectedValue" DbType="Int32"
                Name="provaId" />
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
                            <asp:DropDownList ID="ddlAno" runat="server" DataSourceID="odsAno" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged"
                                DataTextField="ANO" DataValueField="ANO" Width="50px"
                                AutoPostBack="true" />
                            <asp:DropDownList ID="ddlAvaliacao" runat="server" DataSourceID="odsAvaliacao"  OnSelectedIndexChanged="ddlAvaliacao_SelectedIndexChanged"
                                DataTextField="DESCRICAO" DataValueField="AVALIACAOID" Width="300px" AutoPostBack="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 70px; text-align: right;">
                            <asp:Label ID="lblProva" runat="server" SkinID="lblObrigatorio" Text="Prova:* "></asp:Label>
                        </td>
                        <td style="width: 305px;">
                            <asp:DropDownList ID="ddlProva" runat="server" DataSourceID="odsProva" DataTextField="DESCRICAO"
                                DataValueField="PROVAID" Width="300px" AutoPostBack="true" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:PlaceHolder ID="plaGrid" runat="server">
                <br />
                <br />
                <dxwgv:ASPxGridView ID="grdEtapa" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdEtapa"
                    KeyFieldName="ETAPAID" DataSourceID="odsEtapa" OnStartRowEditing="grdEtapa_StartRowEditing"
                    OnInitNewRow="grdEtapa_InitNewRow" OnCustomButtonInitialize="grdEtapa_CustomButtonInitialize"
                    OnRowInserting="grdEtapa_RowInserting" OnRowUpdating="grdEtapa_RowUpdating" 
                    OnRowDeleting="grdEtapa_RowDeleting" 
                    OnCellEditorInitialize="grdEtapa_CellEditorInitialize" Width="80%" OnAfterPerformCallback="grdEtapa_AfterPerformCallback">
                    <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                    <SettingsEditing Mode="Inline" />
                    <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                    <SettingsBehavior ConfirmDelete="true" />
                    <Columns>
                        <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="100px">
                            <HeaderCaptionTemplate>
                                <div style="text-align: center">
                                    <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                                        onclick="grdEtapa.AddNewRow();" />
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
                        <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="ETAPAID" VisibleIndex="1" Visible="false">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn Caption="Prova" Name="PROVA" VisibleIndex="2" FieldName="PROVA" ReadOnly="true">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataComboBoxColumn Caption="Curso*" FieldName="CURSO" VisibleIndex="3" >
                            <PropertiesComboBox DataSourceID="odsCurso" TextField="CURSONOME" ValueField="CURSO"
                                Width="175px" ValueType="System.String" DropDownWidth="175px" ClientInstanceName="cmbCurso"
                                EnableSynchronization="False" EnableIncrementalFiltering="True">
                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                    <RequiredField ErrorText="Favor selecionar um curso." IsRequired="True" />
                                </ValidationSettings>
                                <ClientSideEvents SelectedIndexChanged="function(s, e) { cmbSerie.PerformCallback(); }" />
                            </PropertiesComboBox>
                        </dxwgv:GridViewDataComboBoxColumn>
                        <dxwgv:GridViewDataComboBoxColumn Caption="Série*" FieldName="SERIE" VisibleIndex="4">
                            <PropertiesComboBox Width="100px" DropDownWidth="100px" ClientInstanceName="SERIE"
                                DataSourceID="odsSerie" TextField="SERIE" ValueField="SERIE" EnableSynchronization="False"
                                EnableIncrementalFiltering="True">
                            </PropertiesComboBox>
                            <EditItemTemplate>
                                <dxe:ASPxComboBox ID="cmbSerie" runat="server" TextField="SERIE" ValueField="SERIE"
                                    ClientInstanceName="cmbSerie" OnLoad="cmbSerie_Load" Width="100px">
                                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                        <RequiredField ErrorText="Favor selecionar uma série." IsRequired="True" />
                                    </ValidationSettings>
                                </dxe:ASPxComboBox>
                            </EditItemTemplate>
                        </dxwgv:GridViewDataComboBoxColumn>
                        <dxwgv:GridViewDataDateColumn Caption="Inicio Realização*" FieldName="INICIOREALIZACAO" VisibleIndex="5" >
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataDateColumn Caption="Fim Realização*" FieldName="FIMREALIZACAO" VisibleIndex="6">
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataDateColumn Caption="Inicio Transcrição*" FieldName="INICIOTRANSCRICAO" VisibleIndex="7">
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataDateColumn Caption="Fim Transcrição*" FieldName="FIMTRANSCRICAO" VisibleIndex="8">
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataCheckColumn Caption="Ativo*" FieldName="ATIVO" VisibleIndex="8">
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
