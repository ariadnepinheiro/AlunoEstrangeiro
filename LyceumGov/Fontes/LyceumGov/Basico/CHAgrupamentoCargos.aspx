<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="CHAgrupamentoCargos.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.CHAgrupamentoCargos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
       
        function OnCHCalculaChanged() {
            calculaCH();
        }

        function calculaCH() {
            try {
                var planejamento = 0;
                var regencia = 0;
                var complementacao = 0;
                var valor = 0;

                if (grdCHAgrupamento.GetEditor("CARGAHORARIACOMPLEMENTACAO").GetText() != null) {
                    complementacao = parseInt(grdCHAgrupamento.GetEditor("CARGAHORARIACOMPLEMENTACAO").GetText());
                }
                if (grdCHAgrupamento.GetEditor("CARGAHORARIAREGENCIA").GetText() != null) {
                    regencia = parseInt(grdCHAgrupamento.GetEditor("CARGAHORARIAREGENCIA").GetText());
                }
                if (grdCHAgrupamento.GetEditor("CARGAHORARIAPLANEJAMENTO").GetText() != null) {
                    planejamento = parseInt(grdCHAgrupamento.GetEditor("CARGAHORARIAPLANEJAMENTO").GetText());
                }
            
                valor = complementacao + regencia + planejamento;
                grdCHAgrupamento.GetEditor("CARGAHORARIAGRUPO").SetText(valor);

            } catch (e) {
                grdCHAgrupamento.GetEditor("CARGAHORARIAGRUPO").SetText("");

            }
        }
    </script>

    <asp:ObjectDataSource ID="odsCategoria" runat="server" TypeName="Techne.Lyceum.Net.Basico.CHAgrupamentoCargos"
        SelectMethod="Lista" InsertMethod="Insert" UpdateMethod="Update" DeleteMethod="Delete">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsGrupo" TypeName="Techne.Lyceum.Net.Basico.CategoriaDocente"
        SelectMethod="ListaGrupo" runat="server"></asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdCHAgrupamento" runat="server" DataSourceID="odsCategoria"
        AutoGenerateColumns="False" KeyFieldName="CH_AGRUPAMENTOCARGOID" ClientInstanceName="grdCHAgrupamento"
        OnStartRowEditing="grdCHAgrupamento_StartRowEditing" OnInitNewRow="grdCHAgrupamento_InitNewRow"
        OnRowInserting="grdCHAgrupamento_RowInserting" OnRowUpdating="grdCHAgrupamento_RowUpdating"
        OnRowDeleting="grdCHAgrupamento_RowDeleting" OnCellEditorInitialize="grdCHAgrupamento_CellEditorInitialize"
        OnAfterPerformCallback="grdCHAgrupamento_AfterPerformCallback">
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
                                    <asp:Label ID="lblCHComplementacao" runat="server" Text="C.H. Complementação :* "
                                        SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <dxwgv:ASPxGridViewTemplateReplacement ColumnID="CARGAHORARIACOMPLEMENTACAO" ID="ASPxGridViewTemplateReplacement3"
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
                                    <dxwgv:ASPxGridViewTemplateReplacement ColumnID="TOTAL" ID="ASPxGridViewTemplateReplacement7"
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
                            onclick="grdCHAgrupamento.AddNewRow();" alt="Novo" />
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
            <dxwgv:GridViewDataTextColumn Caption="ID" Visible="false" FieldName="CH_AGRUPAMENTOCARGOID">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataComboBoxColumn VisibleIndex="1" Caption="Grupo*" FieldName="CHAVE"
                Width="150px">
                <PropertiesComboBox TextField="DESCRICAO" ValueField="CHAVE" EnableSynchronization="False"
                    EnableIncrementalFiltering="True" DataSourceID="odsGrupo" ClientInstanceName="CHAVE">
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataTextColumn Caption="Função Relacionada*" HeaderStyle-Font-Bold="true"
                FieldName="FUNCAO" VisibleIndex="3" Width="150px">
                <PropertiesTextEdit MaxLength="100">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Função*" HeaderStyle-Font-Bold="true" FieldName="DESCRICAOFUNCAO"
                VisibleIndex="4" Width="250px">
                <PropertiesTextEdit MaxLength="100">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataSpinEditColumn Caption="C.H. Complementacao*" FieldName="CARGAHORARIACOMPLEMENTACAO"
                VisibleIndex="5" Width="70px">
                <PropertiesSpinEdit DisplayFormatString="g" MaxLength="4" NumberFormat="Custom" NumberType="Integer">
                    <SpinButtons ShowIncrementButtons="False">
                    </SpinButtons>
                    <ClientSideEvents ValueChanged="function(s, e) { OnCHCalculaChanged(s); }"
                        Validation="function(s, e) {
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
				                                                                        e.errorText='C.H. Complementacao deve ser positivo';
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
                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                </CellStyle>
                <HeaderStyle HorizontalAlign="Center" />
            </dxwgv:GridViewDataSpinEditColumn>
            <dxwgv:GridViewDataSpinEditColumn Caption="C.H. Regência*" FieldName="CARGAHORARIAREGENCIA"
                VisibleIndex="6" Width="70px">
                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                </CellStyle>
                <HeaderStyle HorizontalAlign="Center" />
                <PropertiesSpinEdit DisplayFormatString="g" MaxLength="4" NumberFormat="Custom" NumberType="Integer">
                    <SpinButtons ShowIncrementButtons="False">
                    </SpinButtons>
                    <ClientSideEvents ValueChanged="function(s, e) { OnCHCalculaChanged(s); }" Validation="function(s, e) {
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
                VisibleIndex="7" Width="70px">
                <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                </CellStyle>
                <HeaderStyle HorizontalAlign="Center" />
                <PropertiesSpinEdit DisplayFormatString="g" MaxLength="4" NumberFormat="Custom" NumberType="Integer">
                    <SpinButtons ShowIncrementButtons="False">
                    </SpinButtons>
                    <ClientSideEvents ValueChanged="function(s, e) { OnCHCalculaChanged(s); }" Validation="function(s, e) {
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
            <dxwgv:GridViewDataSpinEditColumn Caption="C.H. Total" FieldName="TOTAL" Name="CARGAHORARIAGRUPO"
                VisibleIndex="8" Width="70px">
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
</asp:Content>
