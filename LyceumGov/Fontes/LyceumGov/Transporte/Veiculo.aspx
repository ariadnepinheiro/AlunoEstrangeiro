<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Veiculo.aspx.cs" Inherits="Techne.Lyceum.Net.Transporte.Veiculo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:ObjectDataSource ID="odsVeiculo" runat="server" TypeName="Techne.Lyceum.Net.Transporte.Veiculo"
        SelectMethod="ListarVeiculo" InsertMethod="Insert" UpdateMethod="Update" DeleteMethod="Delete">
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdVeiculo" runat="server" DataSourceID="odsVeiculo" KeyFieldName="VEICULOID"
        AutoGenerateColumns="false" ClientInstanceName="grdVeiculo" OnInitNewRow="grdVeiculo_InitNewRow"
        OnStartRowEditing="grdVeiculo_StartRowEditing" OnRowInserting="grdVeiculo_RowInserting"
        OnRowUpdating="grdVeiculo_RowUpdating" OnRowDeleting="grdVeiculo_RowDeleting"
        OnCellEditorInitialize="grdVeiculo_CellEditorInitialize" Width="1000px">
        <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
        <SettingsEditing Mode="EditForm" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <SettingsBehavior ConfirmDelete="true" />
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                            onclick="grdVeiculo.AddNewRow();" />
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
            <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="VEICULOID"
                Visible="false" Width="700px">
                <PropertiesTextEdit MaxLength="200">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Tipo de Veículo*" HeaderStyle-Font-Bold="true"
                FieldName="TIPOVEICULOID" VisibleIndex="2" Width="120px">
                <Settings FilterMode="DisplayText" />
                <PropertiesComboBox DataSourceID="odsTipoVeiculo" TextField="DESCRICAO" ValueField="TIPOVEICULOID"
                    ValueType="System.String" ClientInstanceName="TIPOVEICULOID" DropDownWidth="120px"
                    Width="120px" EnableSynchronization="False" EnableIncrementalFiltering="True">
                </PropertiesComboBox>
                <HeaderStyle Font-Bold="True"></HeaderStyle>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataTextColumn Caption="Nome*" Name="NOME" VisibleIndex="3" FieldName="NOME"
                Width="300px">
                <PropertiesTextEdit MaxLength="100">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Placa*" Name="PLACA" VisibleIndex="4" FieldName="PLACA"
                Width="100px">
                <PropertiesTextEdit MaxLength="10">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataSpinEditColumn Caption="Ano de Licenciamento*" FieldName="ANOLICENCIAMENTO"
                VisibleIndex="5" Width="70px">
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
			                                                                        if(iVal&lt;1)
			                                                                        {
				                                                                        e.isValid = false;
				                                                                        e.errorText='Ano de Licenciamento deve ser positivo';
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
            <dxwgv:GridViewDataSpinEditColumn Caption="Ano do Modelo*" FieldName="ANOMODELO"
                VisibleIndex="6" Width="70px">
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
			                                                                        if(iVal&lt;1)
			                                                                        {
				                                                                        e.isValid = false;
				                                                                        e.errorText='Ano do Modelo deve ser positivo';
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
            <dxwgv:GridViewDataSpinEditColumn Caption="Quantidade de Assentos*" FieldName="QUANTIDADEASSENTOS"
                VisibleIndex="7" Width="70px">
                <PropertiesSpinEdit DisplayFormatString="g" MaxLength="3" NumberFormat="Custom" NumberType="Integer">
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
				                                                                        e.errorText='Quantidade de Assentos deve ser positivo';
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
            <dxwgv:GridViewDataMemoColumn FieldName="OBSERVACAO" Caption="Observação" VisibleIndex="9"
                Width="300px">
                <PropertiesMemoEdit Height="30px" Width="400px" />
            </dxwgv:GridViewDataMemoColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Ativo?*" FieldName="ATIVO" VisibleIndex="10"
                Width="80px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                    ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
        </Columns>
        <Templates>
            <EditForm>
                <dxw:ContentControl ID="ContentControl1" runat="server">
                    <div style="padding: 4px 4px 3px 4px">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblTipo" runat="server" Text="Tipo de Veículo: "></asp:Label>
                                </td>
                                <td>
                                    <dxwgv:ASPxGridViewTemplateReplacement ColumnID="TIPOVEICULOID" ID="ASPxGridViewTemplateReplacement11"
                                        ReplacementType="EditFormCellEditor" runat="server">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Nome:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td style="color: #FF0000">
                                    <dxwgv:ASPxGridViewTemplateReplacement ColumnID="NOME" ID="ASPxGridViewTemplateReplacement7"
                                        ReplacementType="EditFormCellEditor" runat="server">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="Placa:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td style="color: #FF0000">
                                    <dxwgv:ASPxGridViewTemplateReplacement ColumnID="PLACA" ID="ASPxGridViewTemplateReplacement2"
                                        ReplacementType="EditFormCellEditor" runat="server">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="Ano de Licenciamento:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td style="color: #FF0000">
                                    <dxwgv:ASPxGridViewTemplateReplacement ColumnID="ANOLICENCIAMENTO" ID="ASPxGridViewTemplateReplacement1"
                                        ReplacementType="EditFormCellEditor" runat="server">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="Ano do Modelo:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td style="color: #FF0000">
                                    <dxwgv:ASPxGridViewTemplateReplacement ColumnID="ANOMODELO" ID="ASPxGridViewTemplateReplacement3"
                                        ReplacementType="EditFormCellEditor" runat="server">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblDisciplina" runat="server" Text="Quantidade de Assentos:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <dxwgv:ASPxGridViewTemplateReplacement ColumnID="QUANTIDADEASSENTOS" ID="ASPxGridViewTemplateReplacement4"
                                        ReplacementType="EditFormCellEditor" runat="server">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                            </tr>                            
                            <tr>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text="Observação:"></asp:Label>
                                </td>
                                <td>
                                    <dxwgv:ASPxGridViewTemplateReplacement ColumnID="OBSERVACAO" ID="ASPxGridViewTemplateReplacement6"
                                        ReplacementType="EditFormCellEditor" runat="server">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Text="Ativo:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <dxwgv:ASPxGridViewTemplateReplacement ColumnID="ATIVO" ID="ASPxGridViewTemplateReplacement8"
                                        ReplacementType="EditFormCellEditor" runat="server">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                            </tr>
                        </table>
                        <dxwgv:ASPxGridViewTemplateReplacement ID="repUpdate" ReplacementType="EditFormUpdateButton"
                            runat="server">
                        </dxwgv:ASPxGridViewTemplateReplacement>
                        <dxwgv:ASPxGridViewTemplateReplacement ID="repCancel" ReplacementType="EditFormCancelButton"
                            runat="server">
                        </dxwgv:ASPxGridViewTemplateReplacement>
                </dxw:ContentControl>
                </div>
            </EditForm>
        </Templates>
    </dxwgv:ASPxGridView>
    <asp:ObjectDataSource ID="odsTipoVeiculo" TypeName="Techne.Lyceum.Net.Transporte.Veiculo"
        SelectMethod="ListarTipoVeiculo" runat="server"></asp:ObjectDataSource>
</asp:Content>
