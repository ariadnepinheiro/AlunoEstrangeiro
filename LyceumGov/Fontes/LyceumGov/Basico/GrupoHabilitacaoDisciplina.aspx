<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="GrupoHabilitacaoDisciplina.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.GrupoHabilitacaoDisciplina" %>

<asp:Content ID="conHabilitacaoDisciplina" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBuscaAgrupamento" runat="server" GroupingText="Informe o código ou a descrição do grupo"
        Width="590px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblGrupoTSearch" runat="server" Text="Grupo:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseAgrupamento" runat="server" SqlSelect="select agrupamento, descricao from ly_grupo_habilitacao"
                        Key="agrupamento" OnChanged="tseAgrupamento_Changed" SqlOrder="descricao" SqlWhere=" ATIVO='S'">
                        <gridcolumns>
							<tweb:TSearchBoxColumn Caption="Grupo" FieldName="agrupamento" Width="30%" />
							<tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="70%" />
						</gridcolumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <techne:TTableDataSource ID="tdsGrupoHabilitacaoDisc" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_grupo_habilitacao_disc"
        SqlWhere="agrupamento = @agrupamento">
        <sqlwhereparameters>
            <asp:ControlParameter ControlID="tseAgrupamento" Name="agrupamento" PropertyName="DBValue"
                DefaultValue="null" />
        </sqlwhereparameters>
    </techne:TTableDataSource>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <dxwgv:ASPxGridView ID="grdGrupoHabilitacaoDisciplina" runat="server" AutoGenerateColumns="False"
        ClientInstanceName="grdGrupoHabilitacaoDisciplina" DataSourceID="tdsGrupoHabilitacaoDisc"
        KeyFieldName="CompositeKey" OnRowInserting="grdGrupoHabilitacaoDisciplina_RowInserting"
        Width="567px" OnCustomUnboundColumnData="grdGrupoHabilitacaoDisciplina_CustomUnboundColumnData"
        OnRowDeleting="grdGrupoHabilitacaoDisciplina_RowDeleting" OnRowUpdating="grdGrupoHabilitacaoDisciplina_RowUpdating"
        OnInitNewRow="grdGrupoHabilitacaoDisciplina_InitNewRow" OnStartRowEditing="grdGrupoHabilitacaoDisciplina_StartRowEditing"
        OnRowValidating="grdGrupoHabilitacaoDisciplina_RowValidating" OnAfterPerformCallback="grdGrupoHabilitacaoDisciplina_AfterPerformCallback">
        <Templates>
            <EditForm>
                <dxw:ContentControl ID="ContentControl1" runat="server">
                    <div style="padding: 4px 4px 3px 4px">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblDisciplina" runat="server" Text="Disciplina:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <tweb:TSearchBox runat="server" ID="tsbDisciplina" AutoPostBack="true" DataType="VarChar"
                                        Value='<%# Bind("disciplina") %>' Key="disciplina" MaxLength="20" FollowContainerMode="false"
                                        OnChanged="tsbDisciplina_Changed" SqlSelect="select disciplina, nome from ly_disciplina"
                                        SqlOrder="nome">
                                        <gridcolumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="disciplina" Width="20%" />
                                                <tweb:TSearchBoxColumn Caption="Disciplina" FieldName="nome" Width="80%" />
                                            </gridcolumns>
                                    </tweb:TSearchBox>
                                </td>
                            </tr>
                        </table>
                        <dxwgv:ASPxGridViewTemplateReplacement ID="repUpdate" ReplacementType="EditFormUpdateButton"
                            runat="server" Visible="false">
                        </dxwgv:ASPxGridViewTemplateReplacement>
                        <dxwgv:ASPxGridViewTemplateReplacement ID="repCancel" ReplacementType="EditFormCancelButton"
                            runat="server">
                        </dxwgv:ASPxGridViewTemplateReplacement>
                </dxw:ContentControl>
                </div>
            </EditForm>
        </Templates>
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="EditForm" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdGrupoHabilitacaoDisciplina.AddNewRow();" alt="Novo" />
                    </div>
                </HeaderCaptionTemplate>
                <EditButton Text="Editar" Visible="false">
                    <Image Url="~/img/bt_editar.png" />
                </EditButton>
                <DeleteButton Text="Remover" Visible="True">
                    <Image Url="~/img/bt_exclui2.png" />
                </DeleteButton>
                <UpdateButton>
                    <Image Url="~/img/bt_salvar.png" />
                </UpdateButton>
                <CancelButton Text="Cancelar">
                    <Image Url="~/img/bt_cancelar.png" />
                </CancelButton>
                <ClearFilterButton Text="Limpar" Visible="True">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataTextColumn Caption="Grupo" FieldName="agrupamento" ReadOnly="True"
                Visible="False" VisibleIndex="1">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="disciplina" VisibleIndex="2"
                Visible="True" Width="200px">
                <PropertiesTextEdit Width="200px">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Disciplina" FieldName="nome02" VisibleIndex="3"
                Width="300px">
                <PropertiesTextEdit Width="300px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor escolher uma Disciplina." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" Visible="False"
                VisibleIndex="4" UnboundType="String">
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
</asp:Content>
