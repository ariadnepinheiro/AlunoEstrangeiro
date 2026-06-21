<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="PadacesRelatorios.aspx.cs" Inherits="Techne.Lyceum.Net.Hades.PadacesRelatorios" %>

<asp:Content ID="conPadacesRelatorios" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">

        function OnGrupoRelatChanged(cmbRelat) {
            if (grdRelatorios.GetEditor("relatorio") != null)
                grdRelatorios.GetEditor("relatorio").PerformCallback(cmbRelat.GetValue().toString());
        }
 
            
    </script>

    <asp:Label ID="lblPadaceselecionado" runat="server" Text="Padrão de Acesso:"></asp:Label>
    <asp:Label ID="lblPadaces" runat="server"></asp:Label>
    <asp:Label ID="lblInvisible" runat="server" Visible="false"></asp:Label>
    <techne:TTableDataSource ID="tdsSistema" runat="server" DataTableClassName="Techne.HadesLyc.CR.Hd_sistema"
        SqlWhere="sis = 'LyceumNet'">
    </techne:TTableDataSource>
    <techne:TTableDataSource ID="tdsTransacao" runat="server" DataTableClassName="Techne.HadesLyc.CR.Hd_transacao"
        SqlWhere="Hd_transacao.sis = 'LyceumNet'" SqlOrder="itemmenu">
    </techne:TTableDataSource>
    <techne:TTableDataSource ID="tdsRelatorios" runat="server" DataTableClassName="Techne.HadesLyc.CR.Hd_padrel"
        SqlWhere="hd_padrel.padaces = @padaces and hd_padrel.sis = 'LyceumNet'">
        <SqlWhereParameters>
            <asp:ControlParameter ControlID="lblPadaces" Name="padaces" PropertyName="Text" />
        </SqlWhereParameters>
    </techne:TTableDataSource>
    <techne:TTableDataSource ID="tdsGrupoRelat" runat="server" DataTableClassName="Techne.HadesLyc.CR.Hd_grupo_relatorios"
        SqlWhere="Hd_grupo_relatorios.sis = 'LyceumNet'">
    </techne:TTableDataSource>
    <techne:TTableDataSource ID="tdsComboRelat" runat="server" DataTableClassName="Techne.HadesLyc.CR.Hd_relatorio"
        SqlWhere="Hd_relatorio.sis = 'LyceumNet' and Hd_relatorio.gruporelat = ''" SqlOrder="descricao">
    </techne:TTableDataSource>
    <dxwgv:ASPxGridView runat="server" ID="grdRelatorios" DataSourceID="tdsRelatorios"
        ClientInstanceName="grdRelatorios" AutoGenerateColumns="False" KeyFieldName="CompositeKey"
        OnCellEditorInitialize="grdRelatorios_CellEditorInitialize" OnCustomUnboundColumnData="grdRelatorios_CustomUnboundColumnData"
        OnRowDeleting="grdRelatorios_RowDeleting" OnRowInserting="grdRelatorios_RowInserting"
        OnRowValidating="grdRelatorios_RowValidating" OnInitNewRow="grdRelatorios_InitNewRow"
        OnRowUpdating="grdRelatorios_RowUpdating" Width="411px" 
        onafterperformcallback="grdRelatorios_AfterPerformCallback">
        <SettingsBehavior ConfirmDelete="True" AllowMultiSelection="False" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção ?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="15%">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer" onclick="grdRelatorios.AddNewRow();"
                            alt="Novo" />
                    </div>
                </HeaderCaptionTemplate>
                <EditButton Text="Editar">
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
            <dxwgv:GridViewDataComboBoxColumn Caption="Sistema*" HeaderStyle-Font-Bold="true"
                FieldName="sis" VisibleIndex="1" Width="160px" Visible="false">
                <PropertiesComboBox DataSourceID="tdsSistema" MaxLength="15" TextField="sis" ValueField="sis"
                    ValueType="System.String" Width="160px" DropDownWidth="160px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o Sistema." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataTextColumn FieldName="padaces" ReadOnly="True" VisibleIndex="2"
                Caption="Padrão de Acesso*" HeaderStyle-Font-Bold="true" Visible="True" Width="100px">
                <PropertiesTextEdit MaxLength="14" Width="100px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o Padrão de Acesso." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Grupo do Relatório*" HeaderStyle-Font-Bold="true"
                FieldName="gruporelat" VisibleIndex="3" Visible="true" Width="350px">
                <PropertiesComboBox DataSourceID="tdsGrupoRelat" TextField="gruporelat" ValueField="gruporelat"
                    ValueType="System.String" Width="350px" DropDownWidth="350px">
                    <ClientSideEvents SelectedIndexChanged="function(s, e) {OnGrupoRelatChanged(s);}" />
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o Grupo do Relatório." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Relatório*" HeaderStyle-Font-Bold="true"
                FieldName="relatorio" VisibleIndex="4" Visible="true" Width="350px">
                <PropertiesComboBox TextField="descricao" ValueField="relatorio" ValueType="System.String"
                    Width="350px" DropDownWidth="350px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o Relatório." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataColumn FieldName="CompositeKey" VisibleIndex="5" UnboundType="String"
                Visible="False">
            </dxwgv:GridViewDataColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
    <br />
    <br />
    <asp:Button ID="btnVoltar" runat="server" Text="Voltar" OnClick="btnVoltar_Click" />
</asp:Content>
