<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="DisciplinasNaoExcluidasGLP.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.DisciplinasNaoExcluidasGLP" %>

<asp:Content ID="conDisciplinasBloqueadas" ContentPlaceHolderID="cphFormulario" runat="server">
    <dxwgv:ASPxGridView ID="grdDisciplinas" runat="server" DataSourceID="tdsDisciplinasNaoExcluidas"
        AutoGenerateColumns="False" KeyFieldName="id_evento_geral" OnInitNewRow="grdDisciplinas_InitNewRow"
        ClientInstanceName="grdDisciplinas" OnRowInserting="grdDisciplinas_RowInserting"
        OnRowValidating="grdDisciplinas_RowValidating" 
        oncelleditorinitialize="grdDisciplinas_CellEditorInitialize">
        <SettingsBehavior ConfirmDelete="true" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdDisciplinas.AddNewRow();" alt="Novo" />
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
            <dxwgv:GridViewDataComboBoxColumn Caption="Grupo de Disciplinas*" HeaderStyle-Font-Bold="true"
                FieldName="valor_filtro" Width="290px" VisibleIndex="1">
                <PropertiesComboBox DataSourceID="tdsDisciplinas" MaxLength="40" TextField="descricao"
                    ValueField="agrupamento" ValueType="System.String">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor selecionar o grupo de disciplinas." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataDateColumn FieldName="dt_inicio" VisibleIndex="2" Caption="Data Início*"
                HeaderStyle-Font-Bold="true" Width="100px">
                <PropertiesDateEdit>
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a data de início." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn FieldName="dt_fim" VisibleIndex="3" Caption="Data Final*"
                HeaderStyle-Font-Bold="true" Width="100px">
                <PropertiesDateEdit>
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a data de fim." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataTextColumn FieldName="id_evento_geral" VisibleIndex="0" Caption="ID"
                Visible="false">
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
    <techne:TTableDataSource ID="tdsDisciplinas" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_grupo_habilitacao" SqlOrder="descricao"
    SqlWhere=" ATIVO='S'">
    </techne:TTableDataSource>
    <techne:TTableDataSource ID="tdsDisciplinasNaoExcluidas" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_evento_geral"
        SqlWhere="tipo_filtro = 'DisciplinasNaoExcluidas' and (dt_fim >= convert(date,Getdate()) or dt_fim is null)"
        SqlOrder="dt_fim desc">
    </techne:TTableDataSource>
</asp:Content>
