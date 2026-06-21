<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true" CodeBehind="ProgramasSociais.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.ProgramasSociais" %>
<asp:Content ID="ctProgramasSociais" ContentPlaceHolderID="cphFormulario" runat="server">

<dxwgv:ASPxGridView ID="grdProgramasSociais" runat="server" AutoGenerateColumns="False"
        ClientInstanceName="grdProgramasSociais" 
        DataSourceID="tdsProgramasSociais" KeyFieldName="agencia;programa"
        Width="970px" 
        OnAfterPerformCallback="grdProgramasSociais_AfterPerformCallback" OnInitNewRow="grdProgramasSociais_InitNewRow"
		OnRowValidating="grdProgramasSociais_RowValidating" 
        OnStartRowEditing="grdProgramasSociais_StartRowEditing" 
        onrowdeleting="grdProgramasSociais_RowDeleting">
		<Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer" onclick="grdProgramasSociais.AddNewRow();"
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
                <UpdateButton Text="Salvar">
                    <Image Url="~/img/bt_salvar.png" />
                </UpdateButton>
                <ClearFilterButton Text="Limpar" Visible="True">
                    <Image Url="~/img/bt_limpa.png" />
                </ClearFilterButton>
            </dxwgv:GridViewCommandColumn>
            <dxwgv:GridViewDataTextColumn Caption="Agência de Fomento*" FieldName="agencia" 
                VisibleIndex="1">
                <PropertiesTextEdit MaxLength="20">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar a agência de fomento." 
                            IsRequired="True" />
                            <RegularExpression ErrorText="Agência de Fomento não permite caracteres especiais." ValidationExpression="^[A-Za-z0-9ÁáÀàÂâÃãÉéÈèÊêÍíÌìÎîÓóÒòÔôÕõÚúÙùÛûÇçÑñ\s]*$" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Nome da Agência*" VisibleIndex="2" FieldName="nome_agencia">
                <PropertiesTextEdit MaxLength="100">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o nome da agência." 
                            IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Programa*" FieldName="programa" 
                VisibleIndex="3">
                <PropertiesTextEdit MaxLength="20">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o programa." IsRequired="True" />
                        <RegularExpression ErrorText="Programa não permite caracteres especiais." ValidationExpression="^[A-Za-z0-9ÁáÀàÂâÃãÉéÈèÊêÍíÌìÎîÓóÒòÔôÕõÚúÙùÛûÇçÑñ\s]*$" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Nome do Programa*" 
                FieldName="nome_programa" VisibleIndex="4">
                <PropertiesTextEdit MaxLength="100">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o nome do programa." 
                            IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
        </Columns>
    </dxwgv:ASPxGridView>
    <techne:TTableDataSource ID="tdsProgramasSociais" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_agencia_programa">
    </techne:TTableDataSource>
</asp:Content>
