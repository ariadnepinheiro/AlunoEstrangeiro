<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="AgenteResponsavel.aspx.cs" Inherits="Techne.Lyceum.Net.Patrimonio.AgenteResponsavel" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por unidade administrativa"
        Width="650px">
        <table>
            <tr>
                <td align="left">
                    <asp:Label ID="lblUnidadeAdministrativa" runat="server" Text="Unidade Administrativa:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadeAdministrativa" runat="server" SqlSelect="SELECT ua_atual, r.nome, ua_antiga, r.setor  from VW_UNIDADE_ADMINISTRATIVA_REGIONAL r inner join hades..vw_setor s on r.SETOR = s.SETOR"
                        SqlOrder="setor" ColumnName="setor" Caption="" Connection="Lyceum" MaxLength="15"
                        DataType="Varchar" OnChanged="tseUnidadeAdministrativa_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="setor" Width="10%" />                            
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:ObjectDataSource ID="odsAgente" TypeName="Techne.Lyceum.Net.Patrimonio.AgenteResponsavel"
        runat="server" SelectMethod="Lista" DeleteMethod="Delete" UpdateMethod="Update"
        InsertMethod="Insert">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseUnidadeAdministrativa" Name="setor" PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdAgente" runat="server" EnableCallBacks="false" ClientInstanceName="grdAgente"
        AutoGenerateColumns="False" DataSourceID="odsAgente" KeyFieldName="AGENTERESPONSAVELID"
        OnRowDeleting="grdAgente_RowDeleting" OnRowUpdating="grdAgente_RowUpdating" OnRowInserting="grdAgente_RowInserting"
        OnStartRowEditing="grdAgente_StartRowEditing" OnAfterPerformCallback="grdAgente_AfterPerformCallback" OnHtmlRowCreated="grdAgente_HtmlRowCreated"
        Width="1200px" Visible="false" OnCancelRowEditing="grdAgente_CancelRowEditing"
        OnCellEditorInitialize="grdAgente_CellEditorInitialize">
        <SettingsEditing Mode="EditForm" />
        <Templates>
            <EditForm>
                <div style="padding: 4px 4px 3px 4px">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblMatricula" runat="server" Text="Matrícula:* " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td colspan="3">
                                <tweb:TSearch ID="tseServidor" ColumnID="MATRICULA" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAgentePatrimonio"
                                    AutoPostBack="true" OnTextChanged="tseServidor_Changed" Value='<%# Bind("MATRICULA") %>'>
                                </tweb:TSearch>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblFunc" runat="server" Text="Função: "></asp:Label>
                            </td>
                            <td colspan="3" >
                                <dxe:ASPxLabel ID="lblFuncao" ClientInstanceName="lblFuncao" Value='<%# Bind("FUNCAODESCRICAO") %>' runat="server" >
                                </dxe:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDataNomeacao" runat="server" Text="Data da Designação:* " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                 <dxwgv:ASPxGridViewTemplateReplacement ColumnID="DATANOMEACAO" ID="ASPxGridViewTemplateReplacement1"
                                    ReplacementType="EditFormCellEditor" runat="server">
                                </dxwgv:ASPxGridViewTemplateReplacement>
                            </td>
                            <td>
                                <asp:Label ID="lblDataNomeacaoDO" runat="server" Text="Data da Publicação da Designação: "></asp:Label>
                            </td>
                            <td>
                                <dxwgv:ASPxGridViewTemplateReplacement ColumnID="DATAPUBLICACAONOMEACAO" ID="ASPxGridViewTemplateReplacement6"
                                    ReplacementType="EditFormCellEditor" runat="server">
                                </dxwgv:ASPxGridViewTemplateReplacement>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <dxe:ASPxLabel ID="lblDataDesativacao" Text="Data da Dispensa: " ClientInstanceName="lblDataDesativacao"
                                    runat="server" Width="150px">
                                </dxe:ASPxLabel>
                            </td>
                            <td style="width: 80px">
                                <dxwgv:ASPxGridViewTemplateReplacement ColumnID="DATADISPENSA" ID="ASPxGridViewTemplateReplacement8"
                                    ReplacementType="EditFormCellEditor" runat="server">
                                </dxwgv:ASPxGridViewTemplateReplacement>
                            </td>
                            <td>
                                <asp:Label ID="lblDataDesativacaoDO" runat="server" Text="Data da Publicação da Dispensa: "></asp:Label>
                            </td>
                            <td>
                                <dxwgv:ASPxGridViewTemplateReplacement ColumnID="DATAPUBLICACAODISPENSA" ID="ASPxGridViewTemplateReplacement7"
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
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" alt="Novo" src="../img/bt_novo.png" onclick="grdAgente.AddNewRow();" />
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
            <dxwgv:GridViewDataTextColumn Caption="AGENTERESPONSAVELID" FieldName="AGENTERESPONSAVELID"
                VisibleIndex="0" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Matrícula" FieldName="MATRICULA" VisibleIndex="1"  Width="70px">
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NOME_COMPL" VisibleIndex="2" >
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Função" FieldName="FUNCAODESCRICAO" VisibleIndex="3" >
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Unidade Administrativa" FieldName="SETORDESCRICAO" VisibleIndex="4">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data da Designação" FieldName="DATANOMEACAO" VisibleIndex="5">
                <PropertiesDateEdit Width="120px" EditFormat="Date">
                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                    </CalendarProperties>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn FieldName="DATAPUBLICACAONOMEACAO" Caption="Data da Publicação da Designação" VisibleIndex="6">
                <PropertiesDateEdit Width="120px" EditFormat="Date">
                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                    </CalendarProperties>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data da Dispensa" FieldName="DATADISPENSA" VisibleIndex="7">
                <PropertiesDateEdit Width="120px" EditFormat="Date">
                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                    </CalendarProperties>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data da Publicação da Dispensa" FieldName="DATAPUBLICACAODISPENSA" VisibleIndex="8" >
                <PropertiesDateEdit Width="120px" EditFormat="Date">
                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                    </CalendarProperties>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
          <dxwgv:GridViewDataDateColumn Caption="Data Cadastro" FieldName="DATACADASTRO" VisibleIndex="10" Width="70px" >
            </dxwgv:GridViewDataDateColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
</asp:Content>
