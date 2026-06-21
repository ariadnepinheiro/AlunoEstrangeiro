<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="True"
    CodeBehind="SolicitacaoServicos.aspx.cs" Inherits="Techne.Lyceum.Net.Servico.SolicitacaoServicos" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="conSolicitacaoServicos" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);
        function endRequest(sender, e) {
            if (e.get_error()) {
                document.getElementById("<%=lblMensagem2.ClientID %>").innerText = "";
                document.getElementById("<%=lblMensagem.ClientID %>").innerText = e.get_error().description.replace(e.get_error().name + ": ", "");
                e.set_errorHandled(true);
            }
            else {
                document.getElementById("<%=lblMensagem.ClientID %>").innerText = "";
            }
        }
                
        function OnServicoChanged(servico) {
            grdServicos.GetEditor("obs").PerformCallback(servico.GetValue().toString());
        }
                
    </script>

    <techne:TTableDataSource ID="tdsPadaces" runat="server" DataTableClassName="Techne.HadesLyc.CR.Hd_padaces">
    </techne:TTableDataSource>
    <techne:TTableDataSource ID="tdsMotivos" runat="server" DataTableClassName="Techne.Lyceum.CR.Itemtabela"
        SqlColumns="ITEM, DESCR" SqlWhere="TAB = 'MotivoSolicitacao' And exists (Select 1 from LY_TABELA_SERVICOS Where ((Case When ITEM Not In(1, 2) Then 0 When ITEM = 1 Then 1 End) = @servico) or @servico is null)" SqlOrder="DESCR">
       <SqlWhereParameters>
            <asp:Parameter Name="servico" Type = "String" />
        </SqlWhereParameters>
    </techne:TTableDataSource>
    <techne:TTableDataSource ID="tdsPassos" runat="server" DataTableClassName="Techne.Lyceum.CR.ly_fluxo_de_andamento"
        SqlWhere="ly_fluxo_de_andamento.servico = @servico">
        <SqlWhereParameters>
            <asp:ControlParameter ControlID="txtServico" Name="servico" PropertyName="Text" />
        </SqlWhereParameters>
    </techne:TTableDataSource>
    <asp:ObjectDataSource ID="odsSolicitacoes" TypeName="Techne.Lyceum.Net.Servico.SolicitacaoServicos"
        runat="server" SelectMethod="Listar" DeleteMethod="Delete" InsertMethod="Insert"
        OnDeleting="odsSolicitacoes_Deleting" OnInserting="odsSolicitacoes_Inserting">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseAluno" Name="tseAluno" PropertyName="DBValue" />
        </SelectParameters>
       <InsertParameters>
            <asp:ControlParameter ControlID="tseAluno" Name="tseAluno" PropertyName="DBValue" />
        </InsertParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsAndamentos" TypeName="Techne.Lyceum.Net.Servico.SolicitacaoServicos"
        runat="server" SelectMethod="Listar2" DeleteMethod="Delete" UpdateMethod="Update"
        InsertMethod="Insert">
        <SelectParameters>
            <asp:Parameter Name="solicit" Type="Decimal" />
            <asp:Parameter Name="item" Type="Decimal" />
            <asp:Parameter Name="servico" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsOperadora" TypeName="Techne.Lyceum.Net.Servico.SolicitacaoServicos"
        runat="server" SelectMethod="ListarOperadoras"></asp:ObjectDataSource>
    <techne:TTableDataSource ID="tdsServicos" runat="server" DataTableClassName="Techne.Lyceum.CR.ly_tabela_servicos"
        SqlOrder="descricao" SqlWhere="servico <> 'ALTERACAOCADASTRAL'">
    </techne:TTableDataSource>
    <asp:ScriptManagerProxy ID="manager" runat="server" />
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe a matrícula ou o nome do aluno"
        Height="45px" Width="580px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblAlunoTSearch" runat="server" Text="Aluno:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoSolicitacaoServico"
                        AutoPostBack="true" OnTextChanged="tseAluno_Changed">
                        <QueryParameters>
                            <asp:Parameter Name="ativo" DefaultValue="Ativo" DbType="String" />
                        </QueryParameters>
                    </tweb:TSearch>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:UpdatePanel ID="uppSolicitacao" runat="server">
        <ContentTemplate>
            <br />
            <asp:Label ID="lblMensagem2" runat="server" SkinID="lblMensagem"></asp:Label>
            <br />
            <br />
            <asp:Panel ID="pnGrid" runat="server" Visible="false">
                <asp:Label ID="lblSelecione" runat="server" Text="Selecione uma solicitação:"></asp:Label>
                <dxwgv:ASPxGridView ID="grdServicos" runat="server" DataSourceID="odsSolicitacoes"
                    EnableRowsCache="False" EnableViewState="False" EnableCallBacks="false" ClientInstanceName="grdServicos"
                    AutoGenerateColumns="False" KeyFieldName="solicitacao" OnFocusedRowChanged="grdServicos_FocusedRowChanged"
                    OnInitNewRow="grdServicos_InitNewRow" OnCancelRowEditing="grdServicos_CancelRowEditing"
                    OnRowDeleted="grdServicos_RowDeleted" OnRowInserted="grdServicos_RowInserted"
                    OnRowUpdated="grdServicos_RowUpdated" OnAfterPerformCallback="grdServicos_AfterPerformCallback"
                    OnRowValidating="grdServicos_RowValidating" OnCellEditorInitialize="grdServicos_CellEditorInitialize"
                    OnHtmlRowCreated="grdServicos_HtmlRowCreated">
                    <Templates>
                        <EditForm>
                            <dxw:ContentControl ID="conServico" runat="server">
                                <div style="padding: 4px 4px 3px 4px">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblServico" runat="server" Text="Serviço*:" Font-Bold="true">
                                                </asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <dxwgv:ASPxGridViewTemplateReplacement ID="ASPxGridViewTemplateReplacement1" ReplacementType="EditFormCellEditor"
                                                    runat="server" ColumnID="servico">
                                                </dxwgv:ASPxGridViewTemplateReplacement>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblMotivo" runat="server" Text="Motivo*:" Font-Bold="true">
                                                </asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <dxwgv:ASPxGridViewTemplateReplacement ID="ASPxGridViewTemplateReplacement2" ReplacementType="EditFormCellEditor"
                                                    runat="server" ColumnID="obs">
                                                </dxwgv:ASPxGridViewTemplateReplacement>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblSalineiras" runat="server" Text="Aluno utiliza ônibus Salineiras*?" Font-Bold="true">
                                                </asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <dxwgv:ASPxGridViewTemplateReplacement ID="ASPxGridViewTemplateReplacement3" ReplacementType="EditFormCellEditor"
                                                    runat="server" ColumnID="operadora">
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
                            </dxw:ContentControl>
                            </div>
                        </EditForm>
                    </Templates>
                    <SettingsBehavior AllowMultiSelection="False" AllowFocusedRow="True" ProcessFocusedRowChangedOnServer="true" />
                    <SettingsText EmptyDataRow="Não existem dados." />
                    <ClientSideEvents Init="function(s) {if(s.cpUpdateError) s.ShowError(s.cpUpdateError);}" />
                    <SettingsBehavior ConfirmDelete="True" AllowMultiSelection="False" />
                    <SettingsEditing Mode="EditForm" />
                    <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                    <Columns>
                        <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                            <HeaderCaptionTemplate>
                                <div style="text-align: center">
                                    <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                        onclick="grdServicos.AddNewRow();" alt="Novo" />
                                </div>
                            </HeaderCaptionTemplate>
                            <EditButton Text="Editar" Visible="False">
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
                        <dxwgv:GridViewDataTextColumn FieldName="aluno" VisibleIndex="1" Caption="Aluno"
                            Visible="false">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="item" VisibleIndex="2" Caption="Item" Visible="false">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="qtd" VisibleIndex="2" Caption="Quantidade"
                            Visible="false">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="solicitacao" VisibleIndex="2" Caption="Solicitação"
                            Width="100" ReadOnly="true" CellStyle-HorizontalAlign="Center">
                            <PropertiesTextEdit MaxLength="10" Width="200px">
                            </PropertiesTextEdit>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataComboBoxColumn Caption="Serviço*" FieldName="servico" VisibleIndex="3"
                            Width="200">
                            <PropertiesComboBox DataSourceID="tdsServicos" TextField="descricao" ValueField="servico"
                                ValueType="System.String" Width="200">
                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                    <RequiredField ErrorText="Serviço: Campo obrigatório!" IsRequired="True" />
                                </ValidationSettings>
                                <ClientSideEvents SelectedIndexChanged="function(s, e) { OnServicoChanged(s); }"/>
                            </PropertiesComboBox>
                        </dxwgv:GridViewDataComboBoxColumn>
                        <dxwgv:GridViewDataComboBoxColumn Caption="Motivo*" FieldName="obs" VisibleIndex="3"
                            Width="200">
                            <PropertiesComboBox DataSourceID="tdsMotivos" TextField="descr" ValueField="item"
                                ValueType="System.String" Width="200">
                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                    <RequiredField ErrorText="Motivo: Campo obrigatório!" IsRequired="True" />
                                </ValidationSettings>
                            </PropertiesComboBox>
                        </dxwgv:GridViewDataComboBoxColumn>
                        <dxwgv:GridViewDataComboBoxColumn Caption="Aluno utiliza ônibus Salineiras*?" FieldName="operadora"
                            VisibleIndex="3" Width="200" Visible="false">
                            <PropertiesComboBox DataSourceID="odsOperadora" TextField="OperadoraDs" ValueField="OperadoraId"
                                ValueType="System.String" Width="200">
                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                    <RequiredField ErrorText="Operadora: Campo obrigatório!" IsRequired="true"/>
                                </ValidationSettings>
                            </PropertiesComboBox>
                        </dxwgv:GridViewDataComboBoxColumn>
                        <dxwgv:GridViewDataDateColumn FieldName="data" VisibleIndex="4" Caption="Data" Width="120px"
                            ReadOnly="true">
                            <PropertiesDateEdit Width="120px">
                            </PropertiesDateEdit>
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="status" VisibleIndex="5" Caption="Status"
                            Width="100" ReadOnly="true">
                        </dxwgv:GridViewDataTextColumn>
                    </Columns>
                </dxwgv:ASPxGridView>
                <br />
                <br />
                <asp:Label ID="lblServico" runat="server"></asp:Label>
                <asp:TextBox ID="txtServico" runat="server" Visible="false" />
                <br />
                <br />
                <dxwgv:ASPxGridView runat="server" ID="grdAndamento" ClientInstanceName="grdAndamento"
                    DataSourceID="odsAndamentos" AutoGenerateColumns="False" KeyFieldName="andamento"
                    OnStartRowEditing="grdAndamento_StartRowEditing" EnableCallBacks="false" OnCustomButtonCallback="grdAndamento_CustomButtonCallback">
                    <SettingsBehavior ConfirmDelete="True" AllowMultiSelection="False" />
                    <SettingsEditing Mode="Inline" />
                    <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                    <Columns>
                        <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                            <EditButton Text="Editar" Visible="False">
                                <Image Url="~/img/bt_editar.png" />
                            </EditButton>
                            <DeleteButton Text="Remover" Visible="False">
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
                        <dxwgv:GridViewDataTextColumn FieldName="andamento" VisibleIndex="2" Caption="Andamento"
                            Width="100" ReadOnly="true" CellStyle-HorizontalAlign="Center">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="servico" VisibleIndex="3" Caption="Serviço"
                            Width="100" Visible="false">
                            <PropertiesTextEdit MaxLength="20" Width="200px">
                            </PropertiesTextEdit>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataComboBoxColumn Caption="Passo" FieldName="passo" VisibleIndex="4"
                            Width="250">
                            <PropertiesComboBox DataSourceID="tdsPassos" TextField="descricao" ValueField="passo"
                                ValueType="System.Decimal" Width="250">
                            </PropertiesComboBox>
                        </dxwgv:GridViewDataComboBoxColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="solicitacao" VisibleIndex="5" Caption="Solicitação"
                            Width="100" Visible="false">
                            <PropertiesTextEdit MaxLength="10" Width="100px">
                            </PropertiesTextEdit>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="item" VisibleIndex="6" Caption="Item" Width="100"
                            Visible="false">
                            <PropertiesTextEdit MaxLength="10" Width="100px">
                            </PropertiesTextEdit>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataDateColumn FieldName="data" VisibleIndex="7" Caption="Data" Width="120px"
                            ReadOnly="true">
                            <PropertiesDateEdit Width="120px">
                            </PropertiesDateEdit>
                        </dxwgv:GridViewDataDateColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="status" VisibleIndex="8" Caption="Status"
                            Width="100" ReadOnly="true">
                            <PropertiesTextEdit MaxLength="15" Width="200px">
                            </PropertiesTextEdit>
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="comentario" VisibleIndex="9" Caption="Motivo"
                            Width="100" ReadOnly="true">
                        </dxwgv:GridViewDataTextColumn>
                        <dxwgv:GridViewDataComboBoxColumn Caption="Padrão de Acesso" FieldName="setor" VisibleIndex="10"
                            Width="250">
                            <PropertiesComboBox DataSourceID="tdsPadaces" TextField="nome" ValueField="padaces"
                                ValueType="System.String" Width="250">
                            </PropertiesComboBox>
                        </dxwgv:GridViewDataComboBoxColumn>
                        <dxwgv:GridViewDataTextColumn FieldName="usuario" VisibleIndex="11" Caption="Usuário"
                            Width="100" ReadOnly="true">
                            <PropertiesTextEdit MaxLength="15" Width="200px">
                            </PropertiesTextEdit>
                        </dxwgv:GridViewDataTextColumn>
                    </Columns>
                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                </dxwgv:ASPxGridView>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblComentario" runat="server" Text="Informe o motivo do cancelamento:"></asp:Label>
                            <asp:TextBox ID="txtComentario" runat="server" MaxLength="2000" Width="600px" />
                            <asp:RequiredFieldValidator ErrorMessage="Motivo do cancelamento: Preenchimento obrigatório."
                                ID="rfvCEP" runat="server" ControlToValidate="txtComentario" InitialValue=""
                                ValidationGroup="Cancelar"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnDar" runat="server" Text="Dar Andamento" OnClick="btnDar_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar Solicitação" OnClick="btnCancelar_Click"
                                ValidationGroup="Cancelar" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
