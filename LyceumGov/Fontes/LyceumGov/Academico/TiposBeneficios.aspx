<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true" CodeBehind="TiposBeneficios.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.TiposBeneficios" %>
<asp:Content ID="ctTiposBeneficios" ContentPlaceHolderID="cphFormulario" runat="server">
<dxwgv:ASPxGridView ID="grdTiposBeneficios" runat="server" AutoGenerateColumns="False"
        ClientInstanceName="grdTiposBeneficios" DataSourceID="tdsTiposBeneficios" KeyFieldName="tipo_beneficio"
        Width="970px" 
        OnAfterPerformCallback="grdTiposBeneficios_AfterPerformCallback" OnInitNewRow="grdTiposBeneficios_InitNewRow"
		OnRowValidating="grdTiposBeneficios_RowValidating" 
        OnStartRowEditing="grdTiposBeneficios_StartRowEditing" 
        onrowdeleting="grdTiposBeneficios_RowDeleting">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="70px">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer" onclick="grdTiposBeneficios.AddNewRow();"
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
            <dxwgv:GridViewDataTextColumn Caption="Código*" FieldName="tipo_beneficio" Width="200px"
                VisibleIndex="1">
                <PropertiesTextEdit MaxLength="20" Width="200px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o código." 
                            IsRequired="True" />
                            <RegularExpression ErrorText="O código não permite caracteres especiais." ValidationExpression="^[A-Za-z0-9ÁáÀàÂâÃãÉéÈèÊêÍíÌìÎîÓóÒòÔôÕõÚúÙùÛûÇçÑñ\s]*$" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Descrição*" VisibleIndex="2" 
                FieldName="descricao" Width="600px">
                <PropertiesTextEdit MaxLength="100" Width="600px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a descrição." 
                            IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
    <techne:TTableDataSource ID="tdsTiposBeneficios" runat="server" 
        DataTableClassName="Techne.Lyceum.CR.Ly_tipo_beneficio">
    </techne:TTableDataSource>
</asp:Content>
