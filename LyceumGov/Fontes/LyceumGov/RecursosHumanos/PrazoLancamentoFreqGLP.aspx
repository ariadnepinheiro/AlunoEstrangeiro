<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="PrazoLancamentoFreqGLP.aspx.cs" Inherits="Techne.Lyceum.Net.RecursosHumanos.PrazoLancamentoFreqGLP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:ObjectDataSource ID="odsPeriodoLancamento" runat="server" TypeName="Techne.Lyceum.Net.RecursosHumanos.PrazoLancamentoFreqGLP"
        SelectMethod="Lista" UpdateMethod="Update" InsertMethod="Insert" DeleteMethod="Delete">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsAno" runat="server" SelectMethod="ListaAnoPatrimonio"
        TypeName="Techne.Lyceum.RN.PeriodoLetivo"></asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdPrazo" runat="server" AutoGenerateColumns="False"
        ClientInstanceName="grdPrazo" DataSourceID="odsPeriodoLancamento"
        KeyFieldName="PERIODOLANCAMENTOFREQGLPID" OnRowDeleting="grdPrazo_RowDeleting"
        OnRowUpdating="grdPrazo_RowUpdating" Font-Names="Verdana" Font-Size="Small"
        OnCellEditorInitialize="grdPrazo_CellEditorInitialize" OnInitNewRow="grdPrazo_InitNewRow"
        OnStartRowEditing="grdPrazo_StartRowEditing" OnRowInserting="grdPrazo_RowInserting"
        OnAfterPerformCallback="grdPrazo_AfterPerformCallback">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdPrazo.AddNewRow();" alt="Novo" />
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
            <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="PERIODOLANCAMENTOFREQGLPID"
                Visible="false" Width="700px">
                <PropertiesTextEdit MaxLength="200">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Ano*" FieldName="ANO" VisibleIndex="1"
                Width="80px">
                <PropertiesComboBox ValueType="System.String" DataSourceID="odsAno" TextField="ANO"
                    ClientInstanceName="cmbANO" ValueField="ANO" Width="80px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor selecionar uma ano." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Mês*" VisibleIndex="2" FieldName="MES">
                <PropertiesComboBox>
                    <Items>
                        <dxe:ListEditItem Text="Janeiro" Value="1" />
                        <dxe:ListEditItem Text="Fevereiro" Value="2" />
                        <dxe:ListEditItem Text="Março" Value="3" />
                        <dxe:ListEditItem Text="Abril" Value="4" />
                        <dxe:ListEditItem Text="Maio" Value="5" />
                        <dxe:ListEditItem Text="Junho" Value="6" />
                        <dxe:ListEditItem Text="Julho" Value="7" />
                        <dxe:ListEditItem Text="Agosto" Value="8" />
                        <dxe:ListEditItem Text="Setembro" Value="9" />
                        <dxe:ListEditItem Text="Outubro" Value="10" />
                        <dxe:ListEditItem Text="Novembro" Value="11" />
                        <dxe:ListEditItem Text="Dezembro" Value="12" />
                    </Items>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataDateColumn Caption="Início Período*" HeaderStyle-Font-Bold="true"
                FieldName="DATAINICIO" VisibleIndex="5" Width="200px">
                <PropertiesDateEdit Width="200px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o início do período." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Término Período*" HeaderStyle-Font-Bold="true"
                FieldName="DATAFIM" VisibleIndex="6" Width="200px">
                <PropertiesDateEdit Width="200px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o término do período." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesDateEdit>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataDateColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
</asp:Content>
