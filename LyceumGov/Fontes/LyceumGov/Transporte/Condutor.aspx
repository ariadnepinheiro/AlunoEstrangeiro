<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Condutor.aspx.cs" Inherits="Techne.Lyceum.Net.Transporte.Condutor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">

        function onlyNumbers() {
            //alert('ok');
            if (event.keyCode < 48 || event.keyCode > 57) {

                event.keyCode = 0;
            };
        }

        function OnlyNumericEntry(e) {

            var charCode = (e.which) ? e.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

    </script>

    <asp:ObjectDataSource ID="odsCondutor" runat="server" TypeName="Techne.Lyceum.Net.Transporte.Condutor"
        SelectMethod="ListarCondutor" InsertMethod="Insert" UpdateMethod="Update" DeleteMethod="Delete">
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdCondutor" runat="server" DataSourceID="odsCondutor" KeyFieldName="CONDUTORID"
        AutoGenerateColumns="false" ClientInstanceName="grdCondutor" OnInitNewRow="grdCondutor_InitNewRow"
        OnStartRowEditing="grdCondutor_StartRowEditing" OnRowInserting="grdCondutor_RowInserting"
        OnRowUpdating="grdCondutor_RowUpdating" OnRowDeleting="grdCondutor_RowDeleting"
        Width="60%" OnCustomColumnDisplayText="grdCondutor_CustomColumnDisplayText" OnCellEditorInitialize="grdCondutor_CellEditorInitialize">
        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
        <SettingsEditing Mode="EditForm" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <SettingsBehavior ConfirmDelete="true" />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                            onclick="grdCondutor.AddNewRow();" />
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
            <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="CONDUTORID"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="CPF*" Name="CPF" VisibleIndex="2" FieldName="CPF"
                Width="100px">
                <PropertiesTextEdit MaxLength="14" Width="100px">
                    <MaskSettings IncludeLiterals="None" Mask="000,000,000-00" />
                    <ClientSideEvents KeyPress="function (s, e){ SomentePermitirNumeros(s, e.htmlEvent); }" />
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RegularExpression ErrorText="O campo CPF só aceita números inteiros e positivos."
                            ValidationExpression="^[+]?\d*$" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Nome*" Name="NOME" VisibleIndex="3" FieldName="NOME">
                <PropertiesTextEdit MaxLength="255" Width="450px">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Número CNH*" HeaderStyle-Font-Bold="true"
                FieldName="NUMEROCNH" VisibleIndex="3" Width="120px">
                <CellStyle HorizontalAlign="Left">
                </CellStyle>
                <PropertiesTextEdit MaxLength="14" Width="120px">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Validade CNH*" FieldName="DATAVALIDADECNH"
                VisibleIndex="4" Width="100px">
                <PropertiesDateEdit Width="100px" EditFormat="Date">
                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                    </CalendarProperties>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Categoria*" FieldName="CATEGORIA" VisibleIndex="5">
                <PropertiesComboBox ValueType="System.String" Width="20%">
                    <Items>
                        <dxe:ListEditItem Text="AD" Value="AD" />
                        <dxe:ListEditItem Text="D" Value="D" />
                        <dxe:ListEditItem Text="E" Value="E" />
                        <dxe:ListEditItem Text="MAC" Value="MAC" />
                    </Items>
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField IsRequired="true" ErrorText="Favor selecionar a Categoria." />
                    </ValidationSettings>
                </PropertiesComboBox>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Ativo?*" FieldName="ATIVO" VisibleIndex="6"
                Width="80px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                    ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
        </Columns>
        <Templates>
            <EditForm>
                <dxw:ContentControl ID="ContentControl1" runat="server">
                    <div style="padding: 4px 4px 3px 4px">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label8" runat="server" Text="CPF:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td style="color: #FF0000; width: 200px">
                                    <dxwgv:ASPxGridViewTemplateReplacement ColumnID="CPF" ID="ASPxGridViewTemplateReplacement9"
                                        ReplacementType="EditFormCellEditor" runat="server">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Nome:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td style="color: #FF0000;">
                                    <dxwgv:ASPxGridViewTemplateReplacement ColumnID="NOME" ID="ASPxGridViewTemplateReplacement7"
                                        ReplacementType="EditFormCellEditor" runat="server">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="Número CNH:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td style="color: #FF0000; width: 200px">
                                    <dxwgv:ASPxGridViewTemplateReplacement ColumnID="NUMEROCNH" ID="ASPxGridViewTemplateReplacement2"
                                        ReplacementType="EditFormCellEditor" runat="server">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblDataValidadeCNH" runat="server" Text="Data Validade CNH:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td style="color: #FF0000; width: 200px">
                                    <dxe:ASPxDateEdit runat="server" ID="DATAVALIDADECNH" Value='<%# Bind("DATAVALIDADECNH") %>'>
                                    </dxe:ASPxDateEdit>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCategoria" runat="server" Text="Categoria:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="ddlCategoria" runat="server" Value='<%# Bind("CATEGORIA") %>'
                                        ValueType="System.String" Width="200px">
                                        <Items>
                                            <dxe:ListEditItem Selected="True" Text="Selecione" Value="" />
                                            <dxe:ListEditItem Text="AD" Value="AD" />
                                            <dxe:ListEditItem Text="D" Value="D" />
                                            <dxe:ListEditItem Text="E" Value="E" />
                                            <dxe:ListEditItem Text="MAC" Value="MAC" />
                                        </Items>
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Text="Ativo:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <dxwgv:ASPxGridViewTemplateReplacement ColumnID="ATIVO" ID="ASPxGridViewTemplateReplacement8"
                                        ReplacementType="EditFormCellEditor" runat="server">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                            </tr>
                            <tr>
                                <td>
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
    </dxwgv:ASPxGridView>
</asp:Content>
