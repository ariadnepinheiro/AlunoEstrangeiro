<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true" CodeBehind="Paises.aspx.cs" Inherits="Techne.Lyceum.Net.Hades.Paises" %>


<asp:Content ID="conPaises" ContentPlaceHolderID="cphFormulario" runat="server">

    <techne:TTableDataSource ID="tdsPaises" runat="server" DataTableClassName="Techne.HadesLyc.CR.Hd_pais">
    </techne:TTableDataSource>
        <dxwgv:ASPxGridView ID="grdPaises" runat="server" 
        AutoGenerateColumns="False" ClientInstanceName="grdPaises"
            DataSourceID="tdsPaises" KeyFieldName="pais" Font-Names="Verdana" Font-Size="Small"
            Width="60%" OnInitNewRow="grdPaises_InitNewRow" OnStartRowEditing="grdPaises_StartRowEditing"
            OnCellEditorInitialize="grdPaises_CellEditorInitialize" 
        onafterperformcallback="grdPaises_AfterPerformCallback">
            <SettingsBehavior ConfirmDelete="True" />
            <SettingsEditing Mode="Inline" />
            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
            <Columns>
                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                    <HeaderCaptionTemplate>
                        <div style="text-align: center">
                            <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer" onclick="grdPaises.AddNewRow();"
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
                    <UpdateButton Text="Salvar">
                        <Image Url="~/img/bt_salvar.png" />
                    </UpdateButton>
                    <ClearFilterButton Text="Limpar" Visible="true">
                        <Image Url="~/img/bt_limpa.png" />
                    </ClearFilterButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="País*" HeaderStyle-Font-Bold="true" FieldName="pais" VisibleIndex="1" Width="40%">
                    <PropertiesTextEdit MaxLength="10" Width="90%">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                            <RequiredField ErrorText="Favor informar o País." IsRequired="true" />
                            <RegularExpression ErrorText="País não permite caracteres especiais." ValidationExpression="^[0-9A-Za-zÁáÀàÂâÃãÉéÈèÊêÍíÌìÎîÓóÒòÔôÕõÚúÙùÛûÇçÑñ\s]*$" />
                        </ValidationSettings>
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Nome*" HeaderStyle-Font-Bold="true" FieldName="nome" VisibleIndex="2" Width="60%">
                    <PropertiesTextEdit MaxLength="50" Width="90%">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                            <RequiredField ErrorText="Favor informar o Nome." IsRequired="true" />
                            <RegularExpression ErrorText="Nome não permite caracteres especiais." ValidationExpression="^[A-Za-zÁáÀàÂâÃãÉéÈèÊêÍíÌìÎîÓóÒòÔôÕõÚúÙùÛûÇçÑñ\s]*$" />
                        </ValidationSettings>
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
            </Columns>
			<Styles>
				<CommandColumn Wrap="False"></CommandColumn>
			</Styles>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        </dxwgv:ASPxGridView>
</asp:Content>

