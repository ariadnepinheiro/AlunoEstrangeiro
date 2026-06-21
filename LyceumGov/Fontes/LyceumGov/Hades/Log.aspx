<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true" CodeBehind="Log.aspx.cs" Inherits="Techne.Lyceum.Net.Hades.Log" %>
<asp:Content ID="cc1" ContentPlaceHolderID="cphFormulario" runat="server">

<dxwgv:ASPxGridView ID="grdLog" runat="server" AutoGenerateColumns="False"
                ClientInstanceName="grdLog"
                KeyFieldName="OcorrenciaKey">
                <SettingsEditing Mode="Inline" />
                <SettingsText EmptyDataRow="Não existem dados." ConfirmDelete="Confirma a remoção?" />
                <Columns>
                    <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Visible="false">
                        <HeaderCaptionTemplate>
                            <div style="text-align: center">
                                <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer" onclick="grdLog.AddNewRow();" alt="Novo" />
                            </div>
                        </HeaderCaptionTemplate>
                        <EditButton Text="Editar" Visible="True">
                            <Image Url="~/img/bt_editar.png" />
                        </EditButton>
                        <DeleteButton Text="Remover" Visible="True">
                            <Image Url="~/img/bt_exclui2.png" />
                        </DeleteButton>
                        <SelectButton Text="Selecionar" Visible="True">
                            <Image Url="~/img/bt_busca.png" />
                        </SelectButton>
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
                    <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="id" VisibleIndex="1" Visible="false">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Processo" FieldName="processo" VisibleIndex="2">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataDateColumn Caption="Data" FieldName="data" VisibleIndex="3" Width="200px">
                    </dxwgv:GridViewDataDateColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Descrição" FieldName="log_descricao" VisibleIndex="4">
                    </dxwgv:GridViewDataTextColumn>
                </Columns>
                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
            </dxwgv:ASPxGridView>
</asp:Content>
