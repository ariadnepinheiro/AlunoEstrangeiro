<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true" CodeBehind="Nacionalidade.aspx.cs" Inherits="Techne.Lyceum.Net.Hades.Nacionalidade" %>


<asp:Content ID="conNacionalidades" ContentPlaceHolderID="cphFormulario" runat="server">

    <techne:TTableDataSource ID="tdsNacionalidades" runat="server" DataTableClassName="Techne.HadesLyc.CR.Hd_nacionalidade">
    </techne:TTableDataSource>
        <dxwgv:ASPxGridView ID="grdNacionalidades" runat="server" 
        AutoGenerateColumns="False" ClientInstanceName="grdNacionalidades"
            DataSourceID="tdsNacionalidades" KeyFieldName="nacionalidade" 
        Font-Names="Verdana" Font-Size="Small"
            Width="600px" OnInitNewRow="grdNacionalidades_InitNewRow" OnStartRowEditing="grdNacionalidades_StartRowEditing"
            OnCellEditorInitialize="grdNacionalidades_CellEditorInitialize" 
        onafterperformcallback="grdNacionalidades_AfterPerformCallback">
            <SettingsBehavior ConfirmDelete="True" />
            <SettingsEditing Mode="Inline" />
            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
            <Columns>
                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                    <HeaderCaptionTemplate>
                        <div style="text-align: center">
                            <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer" onclick="grdNacionalidades.AddNewRow();"
                                alt="Novo" />
                        </div>
                    </HeaderCaptionTemplate>
                    <EditButton Text="Editar" Visible="true">
                        <Image Url="~/img/bt_editar.png" />
                    </EditButton>
                    <DeleteButton Text="Remover" Visible="true">
                        <Image Url="~/img/bt_exclui2.png" />
                    </DeleteButton>
                    <CancelButton Text="Cancelar">
                        <Image Url="~/img/bt_cancelar.png" />
                    </CancelButton>
                    <UpdateButton>
                        <Image Url="~/img/bt_salvar.png" />
                    </UpdateButton>
                    <ClearFilterButton Text="Limpar" Visible="true">
                        <Image Url="~/img/bt_limpa.png" />
                    </ClearFilterButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="Nacionalidade*" HeaderStyle-Font-Bold="true" FieldName="nacionalidade" VisibleIndex="1" Width="200">
                    <PropertiesTextEdit MaxLength="10">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                            <RequiredField ErrorText="Favor informar a Nacionalidade." IsRequired="true" />
                            <RegularExpression ErrorText="Nacionalidade não permite caracteres especiais." ValidationExpression="^[A-Za-zÁáÀàÂâÃãÉéÈèÊêÍíÌìÎîÓóÒòÔôÕõÚúÙùÛûÇçÑñ\s]*$" />
                        </ValidationSettings>
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Nome*" HeaderStyle-Font-Bold="true" FieldName="nome" VisibleIndex="2" Width="300">
                    <PropertiesTextEdit MaxLength="50">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                            <RequiredField ErrorText="Favor informar o Nome." IsRequired="true" />
                            <RegularExpression ErrorText="Nome não permite caracteres especiais." ValidationExpression="^[A-Za-zÁáÀàÂâÃãÉéÈèÊêÍíÌìÎîÓóÒòÔôÕõÚúÙùÛûÇçÑñ\s]*$" />
                        </ValidationSettings>
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
            </Columns>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        </dxwgv:ASPxGridView>
</asp:Content>

