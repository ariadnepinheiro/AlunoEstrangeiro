<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Servicos.aspx.cs" Inherits="Techne.Lyceum.Net.Servico.Servicos" %>

<asp:Content ID="conServicos" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
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

    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <asp:ScriptManagerProxy ID="manager" runat="server">
    </asp:ScriptManagerProxy>
    <techne:TTableDataSource ID="tdsServicos" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_tabela_servicos"
        OnSelecting="tdsServicos_Selecting">
    </techne:TTableDataSource>
    <techne:TTableDataSource ID="tdsFluxos" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_fluxo_de_andamento"
        SqlOrder="passo asc">
    </techne:TTableDataSource>
    <techne:TTableDataSource ID="tdsPadaces" runat="server" DataTableClassName="Techne.HadesLyc.CR.Hd_padaces"
        SqlOrder="nome">
    </techne:TTableDataSource>
    <asp:UpdatePanel ID="uppSolicitacao" runat="server" Visible="true">
        <ContentTemplate>
            <dxwgv:ASPxGridView ID="grdServicos" runat="server" DataSourceID="tdsServicos" EnableRowsCache="False"
                EnableViewState="False" EnableCallBacks="false" ClientInstanceName="grdServicos"
                AutoGenerateColumns="False" KeyFieldName="servico" OnCellEditorInitialize="grdServicos_CellEditorInitialize"
                OnFocusedRowChanged="grdServicos_FocusedRowChanged" OnInitNewRow="grdServicos_InitNewRow"
                OnStartRowEditing="grdServicos_StartRowEditing" OnCancelRowEditing="grdServicos_CancelRowEditing"
                OnRowDeleted="grdServicos_RowDeleted" OnRowInserted="grdServicos_RowInserted"
                OnRowUpdated="grdServicos_RowUpdated" OnCustomJSProperties="grdServicos_CustomJSProperties"
                OnRowInserting="grdServicos_RowInserting" OnDataBound="grdServicos_DataBound"
                OnAfterPerformCallback="grdServicos_AfterPerformCallback">
                <SettingsBehavior AllowMultiSelection="False" AllowFocusedRow="True" ProcessFocusedRowChangedOnServer="true" />
                <SettingsText EmptyDataRow="Não existem dados." />
                <ClientSideEvents Init="function(s) {if(s.cpUpdateError) s.ShowError(s.cpUpdateError);}" />
                <SettingsBehavior ConfirmDelete="True" AllowMultiSelection="False" />
                <SettingsEditing Mode="Inline" />
                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                <Columns>
                    <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                        <HeaderCaptionTemplate>
                            <div style="text-align: center">
                                <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer" onclick="grdServicos.AddNewRow();"
                                    alt="Novo" />
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
                    <dxwgv:GridViewDataTextColumn FieldName="servico" VisibleIndex="1" Caption="Serviço"
                        Width="200">
                        <PropertiesTextEdit MaxLength="20" Width="200px">
                            <ClientSideEvents KeyPress="function (s, e){ SomentePermitirCodigo(s, e.htmlEvent); }" />
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                <RegularExpression ErrorText="Serviço não permite caracteres especiais, acentos e espaços." ValidationExpression="^[A-Za-z0-9]*$" />
                                <RequiredField IsRequired="true" ErrorText="Serviço: Campo obrigatório!" />
                            </ValidationSettings>
                        </PropertiesTextEdit>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="descricao" VisibleIndex="2" Caption="Descrição"
                        Width="300">
                        <PropertiesTextEdit MaxLength="100" Width="300px">
                           <ClientSideEvents KeyPress="function (s, e){ SomentePermitirDescricao(s, e.htmlEvent); }" />
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                <RegularExpression ErrorText="Descrição não permite caracteres especiais." ValidationExpression="^[A-Za-z0-9ÁáÀàÂâÃãÉéÈèÊêÍíÌìÎîÓóÒòÔôÕõÚúÙùÛûÇçÑñ\s]*$" />
                                <RequiredField IsRequired="true" ErrorText="Descrição: Campo obrigatório!" />
                            </ValidationSettings>
                        </PropertiesTextEdit>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="solicitavel" VisibleIndex="3" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="revisao_prova" VisibleIndex="4" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="prazo_entrega" VisibleIndex="5" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="solicitavel_web" VisibleIndex="6" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                </Columns>
            </dxwgv:ASPxGridView>
            <br />
            <br />
            <asp:Label ID="lblServico" runat="server"></asp:Label>
            <br />
            <br />
            <dxwgv:ASPxGridView runat="server" ID="grdFluxos" ClientInstanceName="grdFluxos"
                DataSourceID="tdsFluxos" AutoGenerateColumns="False" KeyFieldName="servico;passo"
                OnStartRowEditing="grdFluxos_StartRowEditing" EnableCallBacks="true" OnCellEditorInitialize="grdFluxos_CellEditorInitialize"
                OnInitNewRow="grdFluxos_InitNewRow" OnRowInserting="grdFluxos_RowInserting" OnAfterPerformCallback="grdFluxos_AfterPerformCallback">
                <SettingsBehavior ConfirmDelete="True" AllowMultiSelection="False" />
                <SettingsEditing Mode="Inline" />
                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                <Columns>
                    <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                        <HeaderCaptionTemplate>
                            <div style="text-align: center">
                                <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer" onclick="grdFluxos.AddNewRow();"
                                    alt="Novo" />
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
                    <dxwgv:GridViewDataTextColumn FieldName="servico" VisibleIndex="1" Caption="Serviço"
                        Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="passo" VisibleIndex="2" Caption="Passo"
                        Width="50">
                        <PropertiesTextEdit MaxLength="3" Width="50px">
                            <ClientSideEvents KeyPress="function (s, e){ SomentePermitirNumeros(s, e.htmlEvent); }" />
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                <RegularExpression ErrorText="Passo deve ser um número com até 3 dígitos." ValidationExpression="\d{0,3}" />
                                <RequiredField IsRequired="true" ErrorText="Passo: Campo obrigatório!" />
                            </ValidationSettings>
                        </PropertiesTextEdit>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataComboBoxColumn Caption="Padrão de Acesso" FieldName="setor" VisibleIndex="3"
                        Width="250">
                        <PropertiesComboBox DataSourceID="tdsPadaces" TextField="nome" ValueField="padaces"
                            ValueType="System.String" Width="250">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                <RequiredField ErrorText="Padrão de Acesso: Campo obrigatório!" IsRequired="True" />
                            </ValidationSettings>
                        </PropertiesComboBox>
                    </dxwgv:GridViewDataComboBoxColumn>
                    <dxwgv:GridViewDataTextColumn FieldName="descricao" VisibleIndex="4" Caption="Descrição"
                        Width="300">
                        <PropertiesTextEdit MaxLength="255" Width="300px">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                <RequiredField ErrorText="Descrição: Campo obrigatório!" IsRequired="True" />
                            </ValidationSettings>
                        </PropertiesTextEdit>
                    </dxwgv:GridViewDataTextColumn>
                </Columns>
                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
            </dxwgv:ASPxGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
