<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Experiencias.aspx.cs" Inherits="Techne.Lyceum.Net.ProcessoSeletivo.Experiencias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <techne:TTableDataSource ID="tdsExperiencias" runat="server" DataTableClassName="Techne.Lyceum.CR.LY_CONCURSO_EXPERIENCIA">
    </techne:TTableDataSource>
        <dxwgv:ASPxGridView ID="grdExperiencias" runat="server" AutoGenerateColumns="False"
            ClientInstanceName="grdExperiencias" DataSourceID="tdsExperiencias" KeyFieldName="experiencia"
            Font-Names="Verdana" Font-Size="Small" Width="800px" OnInitNewRow="grdExperiencias_InitNewRow"
            OnStartRowEditing="grdExperiencias_StartRowEditing" 
            oncelleditorinitialize="grdExperiencias_CellEditorInitialize" 
        onafterperformcallback="grdExperiencias_AfterPerformCallback">
            <SettingsBehavior ConfirmDelete="True" />
            <SettingsEditing Mode="Inline" />
            <SettingsText ConfirmDelete="Confirma a remoção ?" EmptyDataRow="Não existem dados." />
            <Columns>
                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                    <HeaderCaptionTemplate>
                        <div style="text-align: center">
                            <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer" onclick="grdExperiencias.AddNewRow();"
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
                <dxwgv:GridViewDataTextColumn Caption="Experiência" FieldName="experiencia" VisibleIndex="1" Width="150">
					<PropertiesTextEdit MaxLength="20" Width="90%">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                            <RequiredField ErrorText="Favor informar a experiência." IsRequired="true" />
                            <RegularExpression ErrorText="Experiência não permite caracteres especiais." ValidationExpression="^[0-9A-Za-zÁáÀàÂâÃãÉéÈèÊêÍíÌìÎîÓóÒòÔôÕõÚúÙùÛûÇçÑñ\s]*$" />
                        </ValidationSettings>
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Descrição" FieldName="descricao" VisibleIndex="2" Width="550">
					<PropertiesTextEdit MaxLength="500" Width="90%">
                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                            <RequiredField ErrorText="Favor informar a descrição." IsRequired="true" />
                        </ValidationSettings>
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
				<dxwgv:GridViewDataComboBoxColumn Caption="Origem" FieldName="origem" VisibleIndex="3" Width="200px">
					<PropertiesComboBox ValueType="System.String">
						<Items>
							<dxe:ListEditItem Text="Dentro da SEEDUC-RJ" Value="S" Selected="true" />
							<dxe:ListEditItem Text="Fora da SEEDUC-RJ" Value="N" />
						</Items>
					</PropertiesComboBox>
				</dxwgv:GridViewDataComboBoxColumn>
            </Columns>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        </dxwgv:ASPxGridView>
</asp:Content>
