<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="MotivoImpedimentoUnidade.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.MotivoImpedimentoUnidade" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:ObjectDataSource ID="odsMotivoImpedimentoUnidade" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.MotivoImpedimentoUnidade"
        SelectMethod="Lista" InsertMethod="Insert" UpdateMethod="Update" DeleteMethod="Delete">
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdMotivoImpedimentoUnidade" runat="server" DataSourceID="odsMotivoImpedimentoUnidade"
        KeyFieldName="MOTIVOIMPEDIMENTOID" AutoGenerateColumns="false"
        ClientInstanceName="grdMotivoImpedimentoUnidade" OnInitNewRow="grdMotivoImpedimentoUnidade_InitNewRow"
        OnStartRowEditing="grdMotivoImpedimentoUnidade_StartRowEditing" OnRowInserting="grdMotivoImpedimentoUnidade_RowInserting"
        OnRowUpdating="grdMotivoImpedimentoUnidade_RowUpdating" OnRowDeleting="grdMotivoImpedimentoUnidade_RowDeleting"
        OnAfterPerformCallback = "grdMotivoImpedimentoUnidade_AfterPerformCallback"
        Width="60%">
        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <SettingsBehavior ConfirmDelete="true" />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                            onclick="grdMotivoImpedimentoUnidade.AddNewRow();" />
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
            <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="MOTIVOIMPEDIMENTOID"
                Visible="false" Width="700px">
                <PropertiesTextEdit MaxLength="200">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Descrição*" Name="Descricao" VisibleIndex="2"
                FieldName="DESCRICAO" Width="300px">
                <PropertiesTextEdit MaxLength="100">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
           
            <dxwgv:GridViewDataDateColumn Caption="Data Ínicio*" FieldName="DATAINICIO" VisibleIndex="8"
                Width="100px">
                <PropertiesDateEdit Width="100px" EditFormat="Date">
                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                    </CalendarProperties>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Fim" FieldName="DATAFIM" VisibleIndex="8"
                Width="100px">
                <PropertiesDateEdit Width="100px" EditFormat="Date">
                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                    </CalendarProperties>
                </PropertiesDateEdit>
            </dxwgv:GridViewDataDateColumn>
           
        </Columns>
    </dxwgv:ASPxGridView>
</asp:Content>
