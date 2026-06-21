<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Relatorios.aspx.cs" Inherits="Techne.Lyceum.Net.Hades.Relatorios" %>

<asp:Content ID="conFormularioHorarioOperacional" ContentPlaceHolderID="cphFormulario"
    runat="server">

    <script type="text/javascript">

        function OnValueChanged(s, e) {
            var lblRelatorio = document.getElementById("<%=lblRelatorio.ClientID %>");

            if (typeof (detailGridView) != 'undefined' && detailGridView != null) {
                detailGridView.PerformCallback(s.GetFocusedRowIndex());
                if (detailGridView.GetSelectedRowCount != 0) {
                    lblRelatorio.innerHTML = "Grupo Selecionado: " + grdGrupoRelatorio.GetSelectedFieldValues("gruporelat");
                }
            }

        }

        function OnGridFocusedRowChanged(source, sourceFieldname, rowIndex) {
            source.GetRowValues(rowIndex, sourceFieldname, OnGetRowValues);
            RefreshGridRelatorio(source);
        }

        function OnGetRowValues(values) {
            var lblRelatorio = document.getElementById("<%=lblRelatorio.ClientID %>");
            if (typeof (values) != 'undefined' && values != null)
                lblRelatorio.innerHTML = "Grupo Selecionado: " + values;
            else
                lblRelatorio.innerHTML = " ";
        }

        function Novo() {
            grdGrupoRelatorio.AddNewRow();
        }

        function RefreshGridRelatorio(sender) {
            //atualiza a grid de Relatorios
            //sender.GetRowValues(rowIndex, 'grupo', sender.GetFocusedRowIndex());

            if (typeof (sender) != 'undefined' && sender != null) {
                if (typeof (sender.cpIsNewRowEditing) != 'undefined' && sender.cpIsNewRowEditing != null) {
                    if (typeof (sender.cpIsEditing) != 'undefined' && sender.cpIsEditing != null) {
                        if (typeof (sender.cpControle) != 'undefined' && sender.cpControle != null) {
                            var isNew = sender.cpIsNewRowEditing;
                            var isEdit = sender.cpIsEditing;
                            var controle = sender.cpControle;

                            var valor = "True";

                            if (isNew == "True")
                                valor = "False";
                            if (isEdit == "True")
                                valor = "False";

                            if (typeof (detailGridView) != 'undefined' && detailGridView != null && controle == "T")
                                detailGridView.PerformCallback(valor);
                        }
                    }
                }
            }
        }

        function RefreshGridGrupoRelatorio(sender) {

            if (typeof (sender) != 'undefined' && sender != null) {
                if (typeof (sender.cpIsNewRowEditing) != 'undefined' && sender.cpIsNewRowEditing != null) {
                    if (typeof (sender.cpIsEditing) != 'undefined' && sender.cpIsEditing != null) {
                        if (typeof (sender.cpControle) != 'undefined' && sender.cpControle != null) {
                            var isNew = sender.cpIsNewRowEditing;
                            var isEdit = sender.cpIsEditing;
                            var controle = sender.cpControle;

                            var valor = "True";

                            if (isNew == "True")
                                valor = "False";
                            if (isEdit == "True")
                                valor = "False";
                            if (valor == "False") {
                                if (typeof (grdGrupoRelatorio) != 'undefined' && grdGrupoRelatorio != null)
                                    grdGrupoRelatorio.enabled = false;
                            }

                            if (typeof (grdGrupoRelatorio) != 'undefined' && grdGrupoRelatorio != null && controle == "I")
                                grdGrupoRelatorio.PerformCallback(valor);
                        }
                    }
                }
            }
        }

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
    <br />
    <techne:TTableDataSource ID="tdsGrupoRelatorio" runat="server" DataTableClassName="Techne.HadesLyc.CR.Hd_grupo_relatorios"
        OnSelecting="tdsGrupoRelatorio_Selecting" SqlWhere="Hd_grupo_relatorios.sis = 'LyceumNet'">
    </techne:TTableDataSource>
    <techne:TTableDataSource ID="tdsRelatorio" runat="server" DataTableClassName="Techne.HadesLyc.CR.Hd_relatorio"
        SqlWhere="Hd_relatorio.sis = 'LyceumNet' and gruporelat = @grupoRel">
        <SqlWhereParameters>
            <asp:SessionParameter Name="grupoRel" SessionField="gruporelat" />
        </SqlWhereParameters>
    </techne:TTableDataSource>
    <%--onfocusedrowchanged="grdGrupoRelatorio_FocusedRowChanged1"--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <br />
            <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
            <br />
            <br />
            <asp:Label ID="lblSelecione" runat="server" Text="Selecione um grupo de relatório:"></asp:Label>
            <br />
            <dxwgv:ASPxGridView ID="grdGrupoRelatorio" runat="server" DataSourceID="tdsGrupoRelatorio"
                ClientInstanceName="grdGrupoRelatorio" AutoGenerateColumns="False" KeyFieldName="gruporelat"
                OnCellEditorInitialize="grdGrupoRelatorio_CellEditorInitialize" Width="90%" Font-Names="Verdana"
                Font-Size="Small" OnInitNewRow="grdGrupoRelatorio_InitNewRow" OnStartRowEditing="grdGrupoRelatorio_StartRowEditing"
                OnRowInserting="grdGrupoRelatorio_RowInserting" OnRowDeleting="grdGrupoRelatorio_RowDeleting"
                OnRowUpdating="grdGrupoRelatorio_RowUpdating" OnFocusedRowChanged="grdGrupoRelatorio_FocusedRowChanged"
                EnableCallBacks="false" OnCancelRowEditing="grdGrupoRelatorio_CancelRowEditing"
                OnRowInserted="grdGrupoRelatorio_RowInserted" OnRowUpdated="grdGrupoRelatorio_RowUpdated"
                OnRowDeleted="grdGrupoRelatorio_RowDeleted">
                <SettingsBehavior ConfirmDelete="True" AllowMultiSelection="false" AllowFocusedRow="True"
                    ProcessFocusedRowChangedOnServer="True" />
                <SettingsEditing Mode="Inline" />
                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                <Templates>
                    <EditForm>
                        <dxw:ContentControl ID="conGrupo" runat="server">
                            <div style="padding: 4px 4px 3px 4px">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblGrupo" runat="server" Text="Grupo:* " SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <dxe:ASPxTextBox ID="txtGrupo" runat="server" EnableViewState="true" MaxLength="50" Width="150px"
                                                Value='<%# Bind("gruporelat") %>' ValidationSettings-ValidationGroup="<%# Container.ValidationGroup %>">
                                                <ValidationSettings ErrorText="Valor inválido.">
                                                    <RequiredField IsRequired="True" ErrorText="Favor informar o grupo."></RequiredField>
                                                </ValidationSettings>
                                            </dxe:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblDescricao" runat="server" Text="Descrição:* " SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <dxe:ASPxTextBox ID="txtDescricao" runat="server" EnableViewState="true" MaxLength="200"  Width="350px"
                                                Value='<%# Bind("descricao") %>' ValidationSettings-ValidationGroup="<%# Container.ValidationGroup %>">
                                                <ValidationSettings ErrorText="Valor inválido.">
                                                    <RequiredField IsRequired="True" ErrorText="Favor informar a descrição do grupo.">
                                                    </RequiredField>
                                                </ValidationSettings>
                                            </dxe:ASPxTextBox>
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
                        </dxw:ContentControl>
                    </EditForm>
                </Templates>
                <Columns>
                    <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                        <HeaderCaptionTemplate>
                            <div style="text-align: center">
                                <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                    onclick="Novo();" alt="Novo" />
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
                    <dxwgv:GridViewDataTextColumn Caption="Grupo*" HeaderStyle-Font-Bold="true" FieldName="gruporelat"
                        VisibleIndex="1">
                        <PropertiesTextEdit MaxLength="50" Width="200px">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                <RequiredField ErrorText="Favor informar o nome do grupo." IsRequired="True" />
                            </ValidationSettings>
                        </PropertiesTextEdit>
                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Descrição*" HeaderStyle-Font-Bold="true" FieldName="descricao"
                        VisibleIndex="2">
                        <PropertiesTextEdit MaxLength="200" Width="720px">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                <RequiredField ErrorText="Favor informar a descrição do grupo." IsRequired="True" />
                            </ValidationSettings>
                        </PropertiesTextEdit>
                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                    </dxwgv:GridViewDataTextColumn>
                </Columns>
                <SettingsPager PageSize="10">
                </SettingsPager>
                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
            </dxwgv:ASPxGridView>
            <br />
            <asp:Label ID="lblRelatorio" runat="server"></asp:Label>
            <br />
            <br />
    <%--OnAfterPerformCallback="grdRelatorio_AfterPerformCallback"
        OnCustomJSProperties="grdRelatorio_CustomJSProperties"
        OnCustomCallback="grdRelatorio_CustomCallback1" --%>
    <dxwgv:ASPxGridView runat="server" ID="grdRelatorio" ClientInstanceName="detailGridView"
        DataSourceID="tdsRelatorio" AutoGenerateColumns="False" KeyFieldName="CompositeKey"
        Width="90%" Font-Names="Verdana" Font-Size="Small" EnableCallBacks="true" OnCellEditorInitialize="grdRelatorio_CellEditorInitialize"
        OnCustomUnboundColumnData="grdRelatorio_CustomUnboundColumnData" OnRowDeleting="grdRelatorio_RowDeleting"
        OnRowInserting="grdRelatorio_RowInserting" OnRowUpdating="grdRelatorio_RowUpdating"
        OnRowValidating="grdRelatorio_RowValidating" OnInitNewRow="grdRelatorio_InitNewRow"
        OnStartRowEditing="grdRelatorio_StartRowEditing" OnCancelRowEditing="grdRelatorio_CancelRowEditing"
        OnRowInserted="grdRelatorio_RowInserted" OnRowUpdated="grdRelatorio_RowUpdated"
        OnAfterPerformCallback="grdRelatorio_AfterPerformCallback1">
        <SettingsBehavior ConfirmDelete="True" AllowMultiSelection="False" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <%--<ClientSideEvents EndCallback="function(s, e) { RefreshGridGrupoRelatorio(s); }" />--%>
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
            <dxwgv:GridViewDataTextColumn FieldName="gruporelat" ReadOnly="False" Visible="false"
                VisibleIndex="1">
                <PropertiesTextEdit MaxLength="20" Width="200px">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="relatorio" ReadOnly="False" VisibleIndex="2"
                Caption="Relatório*" HeaderStyle-Font-Bold="true">
                <PropertiesTextEdit MaxLength="50" Width="150px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RegularExpression ErrorText="Relatório não deve conter acentuação, cedilha e caracteres especiais."
                            ValidationExpression="^[+]?[\w]*$" />
                        <RequiredField ErrorText="Favor informar o nome do relatório." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="descricao" VisibleIndex="3" Caption="Descrição*"
                HeaderStyle-Font-Bold="true">
                <PropertiesTextEdit MaxLength="200" Width="120px" DisplayFormatString="0.00">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a Nota." IsRequired="True" />
                        <RegularExpression ErrorText="" />
                    </ValidationSettings>
                </PropertiesTextEdit>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn FieldName="arquivo" VisibleIndex="4" Caption="Arquivo*"
                HeaderStyle-Font-Bold="true">
                <PropertiesTextEdit MaxLength="200" Width="500px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a Descrição." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Auditar" FieldName="auditar" Visible="False"
                VisibleIndex="4" Width="120px">
                <PropertiesCheckEdit ValueChecked="S" ValueType="System.String" ValueUnchecked="N">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o Valor Mínimo." IsRequired="True" />
                        <RegularExpression ErrorText="Favor informar um valor mínimo válido." ValidationExpression="^\d{0,3}(\,\d{0,2})?$" />
                    </ValidationSettings>
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
            <dxwgv:GridViewDataTextColumn FieldName="funcao" VisibleIndex="4" Caption="Função"
                Width="120px">
                <PropertiesTextEdit MaxLength="50" Width="120px" DisplayFormatString="0.00">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="" />
                        <RegularExpression ErrorText="" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataColumn FieldName="CompositeKey" VisibleIndex="7" UnboundType="String"
                Visible="False">
            </dxwgv:GridViewDataColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
            </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
