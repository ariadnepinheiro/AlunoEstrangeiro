<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="PeriodoReferenciaExtrato.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.PeriodoReferenciaExtrato" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:ObjectDataSource ID="odsPeriodoReferenciaExtrato" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.PeriodoReferenciaExtrato"
        SelectMethod="Lista" InsertMethod="Insert" UpdateMethod="Update" DeleteMethod="Delete">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsDia" runat="server" SelectMethod="ListaDia" TypeName="Techne.Lyceum.RN.Util.Utils">
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdPeriodoReferenciaExtrato" runat="server" DataSourceID="odsPeriodoReferenciaExtrato"
        KeyFieldName="PERIODOREFERENCIAEXTRATOBANCARIOID" AutoGenerateColumns="false"
        ClientInstanceName="grdPeriodoReferenciaExtrato" OnInitNewRow="grdPeriodoReferenciaExtrato_InitNewRow"
        OnStartRowEditing="grdPeriodoReferenciaExtrato_StartRowEditing" OnRowInserting="grdPeriodoReferenciaExtrato_RowInserting"
        OnRowUpdating="grdPeriodoReferenciaExtrato_RowUpdating" OnRowDeleting="grdPeriodoReferenciaExtrato_RowDeleting"
        OnAfterPerformCallback="grdPeriodoReferenciaExtrato_AfterPerformCallback" OnCommandButtonInitialize="grdPeriodoReferenciaExtrato_CommandButtonInitialize" Width="50%">
        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <SettingsBehavior ConfirmDelete="true" />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                            onclick="grdPeriodoReferenciaExtrato.AddNewRow();" />
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
            <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="PERIODOREFERENCIAEXTRATOBANCARIOID"
                Visible="false" Width="700px">
                <PropertiesTextEdit MaxLength="200">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>           
            <dxwgv:GridViewDataComboBoxColumn Caption="Dia referência*" HeaderStyle-Font-Bold="true"
                FieldName="DIAREFERENCIA" VisibleIndex="1" Width="120px">
                <Settings FilterMode="DisplayText" />
                <PropertiesComboBox DataSourceID="odsDia" TextField="DIA" ValueField="DIA"
                    ValueType="System.Int32" DropDownWidth="120px" Width="120px" MaxLength="2">
                </PropertiesComboBox>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Ínicio*" FieldName="DATAINICIO" VisibleIndex="2"
                Width="100px">
                <PropertiesDateEdit Width="100px" EditFormat="Date">
                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                    </CalendarProperties>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Fim" FieldName="DATAFIM" VisibleIndex="3"
                Width="100px">
                <PropertiesDateEdit Width="100px" EditFormat="Date">
                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                    </CalendarProperties>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>           
        </Columns>
    </dxwgv:ASPxGridView>
</asp:Content>
