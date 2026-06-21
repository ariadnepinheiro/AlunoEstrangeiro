<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="GrupoHabilitacao.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.GrupoHabilitacao"
    EnableSessionState="ReadOnly" %>

<asp:Content ID="conHabilitacao" ContentPlaceHolderID="cphFormulario" runat="server">
    <techne:TTableDataSource ID="tdsGrupoHabilitacao" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_grupo_habilitacao"
        SqlOrder="descricao">
    </techne:TTableDataSource>
    <br />
    <div>
        <dxwgv:ASPxGridView ID="grdGrupoHabilitacao" runat="server" AutoGenerateColumns="False"
            ClientInstanceName="grdGrupoHabilitacao" DataSourceID="tdsGrupoHabilitacao" KeyFieldName="agrupamento"
            OnCellEditorInitialize="grdGrupoHabilitacao_CellEditorInitialize" OnInitNewRow="grdGrupoHabilitacao_InitNewRow"
            OnStartRowEditing="grdGrupoHabilitacao_StartRowEditing" OnRowInserting="grdGrupoHabilitacao_RowInserting"
            OnRowUpdating="grdGrupoHabilitacao_RowUpdating" OnAfterPerformCallback="grdGrupoHabilitacao_AfterPerformCallback"
            OnRowValidating="grdGrupoHabilitacao_RowValidating">
            <SettingsBehavior ConfirmDelete="True" />
            <SettingsEditing Mode="Inline" />
            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
            <Columns>
                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                    <HeaderCaptionTemplate>
                        <div style="text-align: center">
                            <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                onclick="grdGrupoHabilitacao.AddNewRow();" alt="Novo" />
                        </div>
                    </HeaderCaptionTemplate>
                    <EditButton Visible="True" Text="Editar">
                        <Image Url="~/img/bt_editar.png" />
                    </EditButton>
                    <DeleteButton Visible="True" Text="Remover">
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
                <dxwgv:GridViewDataTextColumn Caption="Grupo*" HeaderStyle-Font-Bold="true" FieldName="agrupamento"
                    VisibleIndex="1" Width="380px">
                    <PropertiesTextEdit MaxLength="50" Width="380px">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                            <RequiredField ErrorText="Favor preencher Grupo." IsRequired="True" />
                        </ValidationSettings>
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Descrição*" HeaderStyle-Font-Bold="true" FieldName="descricao"
                    VisibleIndex="2" Width="720px">
                    <PropertiesTextEdit MaxLength="200" Width="720px">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                            <RequiredField ErrorText="Favor preencher Descrição." IsRequired="True" />
                        </ValidationSettings>
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="Disponibilidade GLP – DOL" FieldName="disp_glp_dol"
                    VisibleIndex="3" Width="40px">
                    <PropertiesComboBox ValueType="System.String" Width="40px" NullDisplayText="N">
                        <Items>
                            <dxe:ListEditItem Text="S" Value="S" />
                            <dxe:ListEditItem Text="N" Value="N" Selected="true" />
                        </Items>
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                            <RequiredField ErrorText="Favor selectionar a Disponibilidade GLP – DOL." IsRequired="True" />
                        </ValidationSettings>
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataCheckColumn Caption="Atividade Complementar?" FieldName="tipo"
                    VisibleIndex="3" Width="200px">
                    <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="Atividade"
                        ValueType="System.String" ValueUnchecked="Disciplina">
                    </PropertiesCheckEdit>
                </dxwgv:GridViewDataCheckColumn>
                <dxwgv:GridViewDataCheckColumn Caption="Ingresso?" FieldName="ingresso" VisibleIndex="4"
                    Width="200px">
                    <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="S"
                        ValueType="System.String" ValueUnchecked="N" NullDisplayText="N">
                    </PropertiesCheckEdit>
                </dxwgv:GridViewDataCheckColumn>
                <dxwgv:GridViewDataCheckColumn Caption="Ativo?" FieldName="ativo" VisibleIndex="4"
                    Width="200px">
                    <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="S"
                        ValueType="System.String" ValueUnchecked="N" NullDisplayText="N">
                    </PropertiesCheckEdit>
                </dxwgv:GridViewDataCheckColumn>
            </Columns>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        </dxwgv:ASPxGridView>
    </div>
</asp:Content>
