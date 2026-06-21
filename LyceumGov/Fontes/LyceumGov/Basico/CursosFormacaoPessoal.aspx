<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="CursosFormacaoPessoal.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.CursosFormacaoPessoal"
    Title="Cursos de Formação Pessoal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 369px;
        }
        .style2
        {
            width: 98px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnAbas" runat="server" Width="800px">
        <dxtc:ASPxPageControl ID="pcAreaCurso" runat="server" ActiveTabIndex="0" Width="500px"
            OnTabClick="pcAreaCurso_TabClick">
            <TabPages>
                <dxtc:TabPage Text="Áreas">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl1" runat="server">
                            <table>
                                <tr>
                                    <td class="style2">
                                        <asp:Label ID="lblArea" runat="server" Text="Nome da Área:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td class="style1">
                                        &nbsp;
                                        <asp:TextBox ID="txtArea" runat="server" Width="373px" MaxLength = "100"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:Button ID="btnSalvarArea" runat="server" ValidationGroup="SalvarForm" Text="Incluir Área"
                                            OnClick="btnSalvarArea_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <dxwgv:ASPxGridView ID="grdArea" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdArea"
                                KeyFieldName="ID_AREA_FORMACAO_PESSOAL" OnCellEditorInitialize="grdArea_CellEditorInitialize"
                                Font-Names="Verdana" OnStartRowEditing="grdArea_StartRowEditing" OnAfterPerformCallback="grdArea_AfterPerformCallback"
                                DataSourceID="odsArea" OnRowValidating="grdArea_RowValidating">
                                <SettingsBehavior ConfirmDelete="True" />
                                <SettingsEditing Mode="Inline" />
                                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                <Columns>
                                    <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
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
                                    <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="ID_AREA_FORMACAO_PESSOAL" VisibleIndex="1"
                                        Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Nome da Área" FieldName="AREA" VisibleIndex="2">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataDateColumn Caption="Data do Cadastro" FieldName="DT_CADASTRO"
                                        VisibleIndex="3" Visible="false">
                                    </dxwgv:GridViewDataDateColumn>
                                </Columns>
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                            </dxwgv:ASPxGridView>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Text="Cursos">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl2" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAreaCurso" runat="server" Text="Área:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:ObjectDataSource ID="odsCursoArea" TypeName="Techne.Lyceum.Net.Basico.CursosFormacaoPessoal"
                                            runat="server" SelectMethod="ListarCursoArea" UpdateMethod="UpdateCursoArea"
                                            DeleteMethod="DeleteCursoArea"  OnUpdating="odsCursoArea_Updating" OnDeleting="odsCursoArea_Deleting">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="ddlArea" PropertyName="SelectedValue" Name="ID_AREA_FORMACAO_PESSOAL" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                        <asp:DropDownList ID="ddlArea" runat="server" AutoPostBack="True" DataTextField="AREA"
                                            AppendDataBoundItems="true" DataValueField="ID_AREA_FORMACAO_PESSOAL" OnSelectedIndexChanged="ddlArea_SelectedIndexChanged1">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCurso" runat="server" Text="Curso:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCurso" runat="server" Width="403px" MaxLength = "100"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblGrau" runat="server" Text="Grau:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlGrauCurso" runat="server" AutoPostBack="True" DataTextField="descr"
                                            DataValueField="descr" AppendDataBoundItems="true">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <asp:Button ID="btnSalvarCurso" runat="server" ValidationGroup="SalvarForm" Text="Incluir Curso"
                                            OnClick="btnSalvarCurso_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <dxwgv:ASPxGridView ID="grdCurso" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdCurso"
                                KeyFieldName="ID_CURSO_FORMACAO_PESSOAL" OnCellEditorInitialize="grdCurso_CellEditorInitialize"
                                Font-Names="Verdana" OnStartRowEditing="grdCurso_StartRowEditing" OnAfterPerformCallback="grdCurso_AfterPerformCallback"
                                DataSourceID="odsCursoArea" OnRowValidating="grdCurso_RowValidating">
                                <SettingsBehavior ConfirmDelete="True" />
                                <SettingsEditing Mode="Inline" />
                                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                <Columns>
                                    <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                        <EditButton Text="Editar" Visible="true">
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
                                        <ClearFilterButton Text="Limpar" Visible="True">
                                            <Image Url="~/img/bt_limpa.png" />
                                        </ClearFilterButton>
                                    </dxwgv:GridViewCommandColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="ID_CURSO_FORMACAO_PESSOAL"
                                        VisibleIndex="1" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="ID_AREA" FieldName="ID_AREA_FORMACAO_PESSOAL"
                                        VisibleIndex="2" Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Área" FieldName="AREA" VisibleIndex="3">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Curso" VisibleIndex="4" FieldName="CURSO">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataComboBoxColumn Caption="Grau" VisibleIndex="5" FieldName="GRAU">
                                        <PropertiesComboBox DataSourceID="tdsItemTabela" MaxLength="20" TextField="descr"
                                            ValueField="descr" ValueType="System.String" Width="150px" EnableIncrementalFiltering="true">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                <RequiredField ErrorText="Favor informar o Grau." IsRequired="True" />
                                            </ValidationSettings>
                                        </PropertiesComboBox>
                                    </dxwgv:GridViewDataComboBoxColumn>
                                    <dxwgv:GridViewDataDateColumn Caption="Data de Cadastro" FieldName="DT_CADASTRO"
                                        VisibleIndex="6" Visible="false">
                                    </dxwgv:GridViewDataDateColumn>
                                </Columns>
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                            </dxwgv:ASPxGridView>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
            </TabPages>
        </dxtc:ASPxPageControl>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:ObjectDataSource ID="odsArea" TypeName="Techne.Lyceum.Net.Basico.CursosFormacaoPessoal"
        runat="server" SelectMethod="Listar" UpdateMethod="Update" DeleteMethod="Delete"
        OnDeleting="odsArea_Deleting" OnUpdated="ods_Update" OnUpdating="odsArea_Updating">
    </asp:ObjectDataSource>
    <techne:TTableDataSource ID="tdsItemTabela" runat="server" DataTableClassName="Techne.Lyceum.CR.Itemtabela"
        SqlWhere="tab = 'GrauCursoFormacao'" SqlOrder="descr">
    </techne:TTableDataSource>
</asp:Content>
