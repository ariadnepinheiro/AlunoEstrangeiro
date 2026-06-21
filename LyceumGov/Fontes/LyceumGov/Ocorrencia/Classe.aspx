<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Classe.aspx.cs" Inherits="Techne.Lyceum.Net.Ocorrencia.Classe" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:ObjectDataSource ID="odsClasse" runat="server" TypeName="Techne.Lyceum.Net.Ocorrencia.Classe"
        SelectMethod="Lista" InsertMethod="Insert" UpdateMethod="Update" DeleteMethod="Delete">
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdClasse" runat="server" DataSourceID="odsClasse" KeyFieldName="CLASSEID"
        AutoGenerateColumns="false" ClientInstanceName="grdClasse" OnInitNewRow="grdClasse_InitNewRow"
        OnStartRowEditing="grdClasse_StartRowEditing" OnRowInserting="grdClasse_RowInserting"
        OnRowUpdating="grdClasse_RowUpdating" OnRowDeleting="grdClasse_RowDeleting" OnAfterPerformCallback="grdClasse_AfterPerformCallback"
        Width="50%">
        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
        <SettingsEditing Mode="Inline" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <SettingsBehavior ConfirmDelete="true" />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                            onclick="grdClasse.AddNewRow();" />
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
            <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="CLASSEID"
                Visible="false" Width="700px">
                <PropertiesTextEdit MaxLength="200">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Descrição*" Name="Descricao" VisibleIndex="2"
                FieldName="DESCRICAO" Width="400px">
                <PropertiesTextEdit MaxLength="100">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataSpinEditColumn Caption="Ordem*" FieldName="ORDEM" VisibleIndex="3"
                Width="70px">
                <PropertiesSpinEdit DisplayFormatString="g" MaxLength="4" NumberFormat="Custom" NumberType="Integer">
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
			                                                                        if (strVal != 0)
			                                                                        {
			                                                                             if(iVal&lt;1)
			                                                                             {
				                                                                             e.isValid = false;
				                                                                             e.errorText='Vagas Nova deve ser um número';
			                                                                             }
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
            <dxwgv:GridViewDataCheckColumn Caption="Ativo?*" FieldName="ATIVO" VisibleIndex="4"
                Width="120px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                    ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
        </Columns>
    </dxwgv:ASPxGridView>
</asp:Content>
