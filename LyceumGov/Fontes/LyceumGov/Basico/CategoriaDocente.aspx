<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="CategoriaDocente.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.CategoriaDocente" %>

<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>

<asp:Content ID="conCategoriaDocente" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">

        function OnGrupoChanged(cmbGrupo) {

            grdCategoria.GetEditor("CARGAHORARIAGRUPO").SetEnabled(false);
            grdCategoria.GetEditor("CARGAHORARIAGRUPO").SetText(cmbGrupo.GetValue().toString().split('_')[1]);
        }        

    </script>

    <asp:ObjectDataSource ID="odsCategoria" runat="server" TypeName="Techne.Lyceum.Net.Basico.CategoriaDocente"
        SelectMethod="Lista" InsertMethod="Insert" UpdateMethod="Update" DeleteMethod="Delete">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsGrupo" TypeName="Techne.Lyceum.Net.Basico.CategoriaDocente"
        SelectMethod="ListaGrupo" runat="server" onselecting="odsGrupo_Selecting"></asp:ObjectDataSource>
    
    <dxwgv:ASPxGridView ID="grdCategoria" runat="server" DataSourceID="odsCategoria"
        AutoGenerateColumns="False" KeyFieldName="CATEGORIA" ClientInstanceName="grdCategoria"
        OnStartRowEditing="grdCategoria_StartRowEditing" OnInitNewRow="grdCategoria_InitNewRow"
        OnRowInserting="grdCategoria_RowInserting" OnRowUpdating="grdCategoria_RowUpdating"
        OnRowDeleting="grdCategoria_RowDeleting" OnCellEditorInitialize="grdCategoria_CellEditorInitialize"
        Width="100%" OnAfterPerformCallback="grdCategoria_AfterPerformCallback">
        <SettingsBehavior ConfirmDelete="true" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <SettingsEditing Mode="EditForm" />
        <Templates>
            <EditForm>
                <dxw:ContentControl ID="conCategoria" runat="server">
                    <div style="padding: 4px 4px 3px 4px">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCategoria" runat="server" Text="Cargo*: " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <dxwgv:ASPxGridViewTemplateReplacement ColumnID="CATEGORIA" ID="ASPxGridViewTemplateReplacement4"
                                        ReplacementType="EditFormCellEditor" runat="server">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblDescricao" runat="server" Text="Descrição*: " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <dxwgv:ASPxGridViewTemplateReplacement ColumnID="NOME" ID="ASPxGridViewTemplateReplacement1"
                                        ReplacementType="EditFormCellEditor" runat="server">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblFuncao" runat="server" Text="Função Relacionada:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <tweb:TSearchBox ID="tseFuncao" runat="server" Argument="descricao" ArgumentColumns="70"
                                        FollowContainerMode="false" MaxLength="20" AutoPostBack="false" Columns="10"
                                        DataType="VarChar" Key="funcao" Value='<%# Bind("FUNCAO") %>' SqlOrder="descricao"
                                        SqlSelect="SELECT funcao, descricao FROM Ly_funcao" SqlWhere=" ATIVO = 'S' "> 
                                        <GridColumns>
                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="funcao" Width="20%" />
                                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
                                        </GridColumns>
                                    </tweb:TSearchBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblIngresso" runat="server" Text="Ingresso?:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <dxwgv:ASPxGridViewTemplateReplacement ColumnID="INGRESSO" ID="ASPxGridViewTemplateReplacement9"
                                        ReplacementType="EditFormCellEditor" runat="server">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" Text="Necessita Superior?:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <dxwgv:ASPxGridViewTemplateReplacement ColumnID="NECESSITA_SUPERIOR" ID="ASPxGridViewTemplateReplacement2"
                                        ReplacementType="EditFormCellEditor" runat="server">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                            </tr>
                              <tr>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text="Funcionário?:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <dxwgv:ASPxGridViewTemplateReplacement ColumnID="FUNCIONARIO" ID="ASPxGridViewTemplateReplacement3"
                                        ReplacementType="EditFormCellEditor" runat="server">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblTipo" runat="server" Text="Tipo:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="ddlTipo" runat="server" Value='<%# Bind("TIPO") %>' ValueType="System.String"
                                        Width="200px">
                                        <Items>
                                            <dxe:ListEditItem Selected="True" Text="Selecione" Value="Selecione" />
                                            <dxe:ListEditItem Text="Contrato" Value="Contrato" />
                                            <dxe:ListEditItem Text="Funcionário" Value="Funcionario" />
                                            <dxe:ListEditItem Text="Professor Docente I" Value="DocI" />
                                            <dxe:ListEditItem Text="Professor Docente II" Value="DocII" />
                                            <dxe:ListEditItem Text="Monitor" Value="Monitor" />
                                            <dxe:ListEditItem Text="Especial" Value="Especial" />
                                        </Items>
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" Text="Grupo:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <dxwgv:ASPxGridViewTemplateReplacement ColumnID="CHAVE" ID="ASPxGridViewTemplateReplacement11"
                                        ReplacementType="EditFormCellEditor" runat="server">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="C.H. Regência:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <dxwgv:ASPxGridViewTemplateReplacement ColumnID="CARGAHORARIAREGENCIA" ID="ASPxGridViewTemplateReplacement5"
                                        ReplacementType="EditFormCellEditor" runat="server">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" Text="C.H. Planejamento:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <dxwgv:ASPxGridViewTemplateReplacement ColumnID="CARGAHORARIAPLANEJAMENTO" ID="ASPxGridViewTemplateReplacement6"
                                        ReplacementType="EditFormCellEditor" runat="server">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="C.H. Total:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <dxwgv:ASPxGridViewTemplateReplacement ColumnID="CARGAHORARIAGRUPO" ID="ASPxGridViewTemplateReplacement7"
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
        <Columns>
            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdCategoria.AddNewRow();" alt="Novo" />
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
            <dxwgv:GridViewDataTextColumn Caption="Cargo*" HeaderStyle-Font-Bold="true" FieldName="CATEGORIA"
                VisibleIndex="1" Width="250px">
                <PropertiesTextEdit MaxLength="20" Width="250px">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Descrição*" HeaderStyle-Font-Bold="true" FieldName="NOME"
                VisibleIndex="2" Width="250px">
                <PropertiesTextEdit MaxLength="100" Width="250px">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Função Relacionada*" HeaderStyle-Font-Bold="true"
                FieldName="FUNCAO" VisibleIndex="3" Width="250px">
                <PropertiesTextEdit MaxLength="100">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Função*" HeaderStyle-Font-Bold="true" FieldName="FUNCAODESCRICAO"
                VisibleIndex="3" Width="250px">
                <PropertiesTextEdit MaxLength="100">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Ingresso?*" FieldName="INGRESSO" VisibleIndex="4"
                Width="120px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="S"
                    ValueType="System.String" ValueUnchecked="N" DisplayTextUndefined="">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
            <dxwgv:GridViewDataCheckColumn Caption="Necessita Superior?*" FieldName="NECESSITA_SUPERIOR"
                VisibleIndex="5" Width="120px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="S"
                    ValueType="System.String" ValueUnchecked="N" DisplayTextUndefined="">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
             <dxwgv:GridViewDataCheckColumn Caption="Funcionário?*" FieldName="FUNCIONARIO"
                VisibleIndex="5" Width="120px">
                <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="S"
                    ValueType="System.String" ValueUnchecked="N" DisplayTextUndefined="">
                </PropertiesCheckEdit>
            </dxwgv:GridViewDataCheckColumn>
            <dxwgv:GridViewDataTextColumn Caption="Tipo*" HeaderStyle-Font-Bold="true" FieldName="TIPO"
                VisibleIndex="6" Width="250px">
                <PropertiesTextEdit MaxLength="100" Width="250px">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataComboBoxColumn VisibleIndex="7" Caption="Grupo*" FieldName="CHAVE"
                Width="250px">
                <PropertiesComboBox TextField="DESCRICAO" ValueField="CHAVE" EnableSynchronization="False"
                    EnableIncrementalFiltering="True" DataSourceID="odsGrupo" ClientInstanceName="CHAVE">
                    <ClientSideEvents SelectedIndexChanged="function(s, e) { OnGrupoChanged(s); }"></ClientSideEvents>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataSpinEditColumn Caption="C.H. Regência*" FieldName="CARGAHORARIAREGENCIA"
                VisibleIndex="9" Width="70px">
                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                </CellStyle>
                <HeaderStyle HorizontalAlign="Center" />
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
			                                                                        if(iVal&lt;0)
			                                                                        {
				                                                                        e.isValid = false;
				                                                                        e.errorText='C.H. Regência deve ser positivo';
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
            <dxwgv:GridViewDataSpinEditColumn Caption="C.H. Planejamento*" FieldName="CARGAHORARIAPLANEJAMENTO"
                VisibleIndex="10" Width="70px">
                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                </CellStyle>
                <HeaderStyle HorizontalAlign="Center" />
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
			                                                                        if(iVal&lt;0)
			                                                                        {
				                                                                        e.isValid = false;
				                                                                        e.errorText='C.H. Planejamento deve ser positivo';
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
            <dxwgv:GridViewDataSpinEditColumn Caption="C.H. Total" FieldName="CARGAHORARIAGRUPO"
                Name="CARGAHORARIAGRUPO" VisibleIndex="11" Width="70px">
                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                </CellStyle>
                <HeaderStyle HorizontalAlign="Center" />
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
				                                                                        e.errorText='C.H. Total deve ser positivo';
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
        </Columns>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
       
    <asp:Button ID="Button1" runat="server" Text="Exportar" OnClick="Button1_Click_ExportarButton1_Click" />
       
    </asp:Content>