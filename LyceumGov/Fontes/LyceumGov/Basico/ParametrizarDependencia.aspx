<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ParametrizarDependencia.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.ParametrizarDependencia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:UpdatePanel ID="UpdateDadosUnidadeEnsino" runat="server">
        <ContentTemplate>
            <br />
            <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe o curso" Width="750px">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblUnidadeTSearch" runat="server" Text="Curso:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <tweb:TSearchBox ID="tseUnidade_Ensino_Destino" runat="server" Key="curso" Argument="nome"
                                MaxLength="8" SqlSelect="SELECT curso, nome from ly_curso" GridWidth="850px"
                                SqlOrder="nome" AutoPostBack="true">
                                <GridColumns>
                                    <tweb:TSearchBoxColumn Caption="Curso" FieldName="curso" Width="20%" />
                                    <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="80%" />
                                </GridColumns>
                            </tweb:TSearchBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <asp:ObjectDataSource ID="odsTipodependencia" runat="server" TypeName="Techne.Lyceum.RN.ParametrizarDependencia"
                SelectMethod="BuscaDependencias"></asp:ObjectDataSource>
            <asp:ObjectDataSource ID="ObTipoDependencia" TypeName="Techne.Lyceum.Net.Basico.ParametrizarDependencia"
                SelectMethod="ListarDependenciaPorCurso" runat="server" OnInserting="ObTipoDependencia_Inserting"
                InsertMethod="InsertObTipoDependencia" DeleteMethod="DeleteObTipoDependencia"
                OnDeleting="ObTipoDependencia_Deleting">
                <SelectParameters>
                    <asp:ControlParameter ControlID="tseUnidade_Ensino_Destino" DefaultValue="" Name="curso"
                        PropertyName="DBValue" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <dxwgv:ASPxGridView ID="grdTipoDependencia" runat="server" DataSourceID="ObTipoDependencia"
                KeyFieldName="CompositeKey" ClientInstanceName="grdTipoDependencia" AutoGenerateColumns="false"
                OnInitNewRow="grdTipoDependencia_InitNewRow" Width="500px" OnRowInserting="grdTipoDependencia_RowInserting"
                OnRowDeleting="grdTipoDependencia_RowDeleting" OnCellEditorInitialize="grdTipoDependencia_CellEditorInitialize"
                OnCustomUnboundColumnData="grdTipoDependencia_CustomUnboundColumnData">
                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                <SettingsBehavior ConfirmDelete="true" AllowMultiSelection="False" />
                <SettingsText ConfirmDelete="Confirma a remoção ?" EmptyDataRow="Não existem dados." />
                <SettingsEditing Mode="Inline" />
                <Columns>
                    <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                        <HeaderCaptionTemplate>
                            <div style="text-align: center">
                                <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                    onclick="grdTipoDependencia.AddNewRow();" alt="Novo" />
                            </div>
                        </HeaderCaptionTemplate>
                        <DeleteButton Text="Remover" Visible="True">
                            <Image Url="~/img/bt_exclui2.png" />
                        </DeleteButton>
                        <CancelButton Text="Cancelar">
                            <Image Url="~/img/bt_cancelar.png" />
                        </CancelButton>
                        <UpdateButton Text="Atualizar">
                            <Image Url="~/img/bt_salvar.png" />
                        </UpdateButton>
                        <ClearFilterButton Text="Limpar" Visible="true">
                            <Image Url="~/img/bt_limpa.png" />
                        </ClearFilterButton>
                    </dxwgv:GridViewCommandColumn>
                    <dxwgv:GridViewDataComboBoxColumn Caption="Tipo dependência*" FieldName="LYTIPODEPENDENCIAID"
                        VisibleIndex="4" Width="150px" HeaderStyle-Font-Bold="true">
                        <PropertiesComboBox DataSourceID="odsTipodependencia" TextField="NOME" ValueField="TIPO_DEPEND"
                            ValueType="System.String" Width="200px" MaxLength="40">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                <RequiredField IsRequired="true" ErrorText="Favor selecionar o Tipo." />
                            </ValidationSettings>
                        </PropertiesComboBox>
                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                    </dxwgv:GridViewDataComboBoxColumn>
                    <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                        Visible="False" VisibleIndex="8">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="IDCURSO" VisibleIndex="9" Caption="LYCURSOID"
                        Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                </Columns>
            </dxwgv:ASPxGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel3" runat="server" CssClass="overlay">
                <asp:Panel ID="Panel2" runat="server" CssClass="loader">
                    <asp:Image ID="Image1" runat="server" AlternateText="Updating..." Height="48" ImageUrl="~/Images/updateProgress.gif"
                        Width="48" />
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
