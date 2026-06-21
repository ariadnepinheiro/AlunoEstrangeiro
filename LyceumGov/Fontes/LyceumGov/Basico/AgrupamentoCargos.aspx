<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="AgrupamentoCargos.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.AgrupamentoCargos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:ObjectDataSource ID="odsAgrupamentoCargo" runat="server" TypeName="Techne.Lyceum.Net.Basico.AgrupamentoCargos"
        SelectMethod="Lista" InsertMethod="Insert" UpdateMethod="Update" DeleteMethod="Delete">
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdAgrupamentoCargo" runat="server" DataSourceID="odsAgrupamentoCargo"
        KeyFieldName="AGRUPAMENTOCARGOSID" AutoGenerateColumns="false" ClientInstanceName="grdAgrupamentoCargo"
        OnInitNewRow="grdAgrupamentoCargo_InitNewRow" OnStartRowEditing="grdAgrupamentoCargo_StartRowEditing"
        OnRowInserting="grdAgrupamentoCargo_RowInserting" OnRowUpdating="grdAgrupamentoCargo_RowUpdating"
        OnRowDeleting="grdAgrupamentoCargo_RowDeleting" Width="1000px">
        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
        <SettingsEditing Mode="InLine" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <SettingsBehavior ConfirmDelete="true" />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                            onclick="grdAgrupamentoCargo.AddNewRow();" />
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
            <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="AGRUPAMENTOCARGOSID"
                Visible="false" Width="700px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Grupo*" Name="DESCRICAO" VisibleIndex="2"
                FieldName="DESCRICAO" Width="300px">
                <PropertiesTextEdit MaxLength="500">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataSpinEditColumn Caption="CH*" FieldName="CARGAHORARIA" VisibleIndex="5"
                Width="70px">
                <PropertiesSpinEdit DisplayFormatString="g" MaxLength="2" NumberFormat="Custom" NumberType="Integer">
                    <SpinButtons ShowIncrementButtons="False">
                    </SpinButtons>
                    <ClientSideEvents Validation="function(s, e) {
	                                                                        var strVal=e.value;
	                                                                        var iVal=null;
	                                                                        e.isValid = true;
	                                                                        try
	                                                                        {
		                                                                        if(strVal!=null)
		                                                                        {
			                                                                        iVal=parseInt(strVal);
			                                                                        if(iVal&lt;1)
			                                                                        {
				                                                                        e.isValid = false;
				                                                                        e.errorText='Carga Horária deve ser positivo';
			                                                                        }
		                                                                        }
	                                                                        }
	                                                                        catch(ex)
	                                                                        {
		                                                                        e.isValid = false;
		                                                                        e.errorText=ex;
	                                                                        }
                                                                        }" />
                </PropertiesSpinEdit>
            </dxwgv:GridViewDataSpinEditColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Ativo?*" FieldName="ATIVO" VisibleIndex="10"
                Width="80px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                    ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
        </Columns>
    </dxwgv:ASPxGridView>
    <br />

<asp:Button ID="btnRelatorio" runat="server" Text="Relatório" Width="150px"  />

<br /><br />



</asp:Content>
