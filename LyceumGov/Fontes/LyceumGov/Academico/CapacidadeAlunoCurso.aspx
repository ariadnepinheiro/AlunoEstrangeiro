<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="CapacidadeAlunoCurso.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.CapacidadeAlunoCurso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 151px;
            text-align: right;
        }
        .style2
        {
            width: 140px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        $(document).ready(function() {
            $("#<%= this.txtMaxima.ClientID %>").numeric();
            $("#<%= this.txtMinima.ClientID %>").numeric();

        });
        function BloquearCtrl() {
            if (event.keyCode == 17)
            { alert("Proibido utilizar o Ctrl neste campo"); }
        }

        function desabilitaBotaoDireito() {
            if (event.button == 2) {
                alert("Proibido utilizar o botao direito neste campo");
            }
        }

        function onlyNumbers() {
            if (event.keyCode < 48
                || event.keyCode > 57) {
            	
                event.keyCode = 0;
            };
        }

        function blocTexto(campo, qtde) {
            var quant = qtde;
            var valor = $.trim($(campo).val());
            var total = valor.length;

            if (total > quant) {
                $(campo).val(valor.substr(0, quant));
            }
        }
        function ConfirmaReplicacao() {
        	var selectObjReplicar = $("#<%=ddlReplicar.ClientID %>").val();
        	var selectObjfiltro = ( $("#<%=ddlAno.ClientID %>").val() + "/" + $("#<%=ddlPeriodo.ClientID %>").val()) ;
        	if (selectObjReplicar != 'Nenhum')
        	{
        		if (confirm("Confirma replicação de dados para " + selectObjfiltro + " a partir de " + selectObjReplicar + " ?")) {
        			return true;
        		}
        		return false;
        	}
        	return false;
        }
        $(document).ready(function() {
            $('#<%=txtMinima.ClientID %>').bind('keyup', function() { blocTexto(this, 5); });
            $('#<%=txtMaxima.ClientID %>').bind('keyup', function() { blocTexto(this, 5); });
        });

    </script>

    <asp:Panel ID="pnGeral" runat="server" GroupingText="Informe Ano/Período para Pesquisar"
        Width="800px">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblAno" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td class="style2">
                    <asp:DropDownList ID="ddlAno" runat="server" AutoPostBack="True" DataTextField="ano"
                        DataValueField="ano" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged" AppendDataBoundItems="true">
                    </asp:DropDownList>
                </td>
                <td class="style1">
                    <asp:Label ID="lblPeriodo" runat="server" SkinID="lblObrigatorio" Text="Periodo:*"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlPeriodo" runat="server" AppendDataBoundItems="true" AutoPostBack="True"
                        DataTextField="periodo" DataValueField="periodo" OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlGerais" runat="server" GroupingText="Dados Gerais" Width="800px"
        Visible="false">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblReplicar" runat="server" Text="Replicar de:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlReplicar" runat="server" AutoPostBack="True" DataTextField="anoperiodo"
                        DataValueField="anoperiodo" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlReplicar_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblModalidadeTSearch" runat="server" Text="Modalidade:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlModalidade" runat="server" DataTextField="descricao" DataValueField="modalidade"
                        AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="ddlModalidade_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblNivelTSearch" runat="server" Text="Nível:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlNivel" runat="server" DataTextField="descricao" DataValueField="tipo"
                        AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="ddlNivel_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 120px">
                    <asp:Label ID="lblCursoTSearch" runat="server" Text="Escolaridade:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseCurso" runat="server" Caption="" SqlSelect="SELECT distinct  c.curso , c.nome , tc.tipo , tc.descricao, mc.modalidade as cod_modalidade, mc.descricao as modalidade FROM LY_CURSO c left join LY_TIPO_CURSO tc on tc.TIPO = c.TIPO    inner join LY_MODALIDADE_CURSO mc on mc.MODALIDADE = c.MODALIDADE "
                        ArgumentColumns="60" Columns="10" MaxLength="20" ColumnName="curso" DataType="Varchar"
                        SqlOrder="nome" GridWidth="800px">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="50%" />
                            <tweb:TSearchBoxColumn Caption="Nível" FieldName="descricao" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Modalidade" FieldName="modalidade" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                   <%-- <tweb:TSearch ID="tsCurso" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryCursoCapacidade"
                       Width="100%" AutoPostBack="true" ValidateText="true" OnTextChanged="tsCurso_Changed">
                        <QueryParameters>
                            <asp:Parameter Name="ano" DbType="String" />
                            <asp:Parameter Name="periodo" DbType="String" />
                            <asp:Parameter Name="modalidade" DbType="String" />
                            <asp:Parameter Name="tipo" DbType="String" />
                        </QueryParameters>
                    </tweb:TSearch>--%>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblMinima" runat="server" Text="Cap. Mínima:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtMinima" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblMaxima" runat="server" Text="Cap. Máxima:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtMaxima" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="right">
                    <asp:Button ID="btnSalvar" runat="server" ValidationGroup="SalvarForm" Text="Salvar"
                        Visible="True" OnClick="btnSalvar_Click"  />
                        <asp:Button ID="btnReplicar" runat="server" ValidationGroup="SalvarForm" Text="Salvar"
                        Visible="False" OnClick="btnSalvar_Click" OnClientClick="return ConfirmaReplicacao();" />
                </td>
            </tr>
        </table>
        <br />
        <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
        <br />
        <table>
            <tr>
                <td>
                    <asp:Panel ID="pnGrid" runat="server">
                        <dxwgv:ASPxGridView ID="grdCapacidade" runat="server" AutoGenerateColumns="False"
                            Visible="False" ClientInstanceName="grdCapacidade" DataSourceID="odsCapacidade"
                            KeyFieldName="CAPACIDADEALUNOTURMAID" OnStartRowEditing="grdCapacidade_StartRowEditing"
                            OnAfterPerformCallback="grdCapacidade_AfterPerformCallback">
                            <SettingsBehavior ConfirmDelete="True" />
                            <SettingsEditing Mode="Inline" />
                            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                            <Columns>
                                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                    <EditButton Text="Editar" Visible="True">
                                        <Image Url="~/img/bt_editar.png" />
                                    </EditButton>
                                    <DeleteButton Text="Remover" Visible="True">
                                        <Image Url="~/img/bt_exclui2.png" />
                                    </DeleteButton>
                                    <UpdateButton Text="Salvar">
                                        <Image Url="~/img/bt_salvar.png" />
                                    </UpdateButton>
                                    <CancelButton Text="Cancelar">
                                        <Image Url="~/img/bt_cancelar.png" />
                                    </CancelButton>
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="CAPACIDADEALUNOTURMAID"
                                    ReadOnly="true" VisibleIndex="1">
                                    <PropertiesTextEdit>
                                        <ReadOnlyStyle>
                                            <Border BorderStyle="None"></Border>
                                        </ReadOnlyStyle>
                                    </PropertiesTextEdit>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Ano/Período" FieldName="ANOPERIODO" ReadOnly="true"
                                    VisibleIndex="2">
                                    <PropertiesTextEdit>
                                        <ReadOnlyStyle>
                                            <Border BorderStyle="None"></Border>
                                        </ReadOnlyStyle>
                                    </PropertiesTextEdit>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Curso" FieldName="CURSOID" ReadOnly="true"
                                    VisibleIndex="3">
                                    <PropertiesTextEdit>
                                        <ReadOnlyStyle>
                                            <Border BorderStyle="None"></Border>
                                        </ReadOnlyStyle>
                                    </PropertiesTextEdit>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="DESCRICAOCURSO" ReadOnly="true"
                                    Visible="true" VisibleIndex="4">
                                    <PropertiesTextEdit>
                                        <ReadOnlyStyle>
                                            <Border BorderStyle="None"></Border>
                                        </ReadOnlyStyle>
                                    </PropertiesTextEdit>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Tipo" FieldName="DESCRICAOTIPO" ReadOnly="true"
                                    Visible="true" VisibleIndex="5">
                                    <PropertiesTextEdit>
                                        <ReadOnlyStyle>
                                            <Border BorderStyle="None"></Border>
                                        </ReadOnlyStyle>
                                    </PropertiesTextEdit>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataSpinEditColumn Caption="Mínimo*" FieldName="CAPACIDADEMINIMA"
                                    VisibleIndex="6" Width="70px">
                                    <PropertiesSpinEdit DisplayFormatString="g" MaxLength="5" NumberFormat="Custom" NumberType="Integer">
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
				                                                                        e.errorText='Mínima deve ser positivo';
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
                                <dxwgv:GridViewDataSpinEditColumn Caption="Máxima*" FieldName="CAPACIDADEMAXIMA"
                                    VisibleIndex="6" Width="70px">
                                    <PropertiesSpinEdit DisplayFormatString="g" MaxLength="6" NumberFormat="Custom" NumberType="Integer">
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
				                                                                        e.errorText='Máxima deve ser positivo';
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
                                <dxwgv:GridViewDataTextColumn Caption="Data" FieldName="DATAALTERACAO"
                                    ReadOnly="true" VisibleIndex="7">
                                    <PropertiesTextEdit>
                                        <ReadOnlyStyle>
                                            <Border BorderStyle="None"></Border>
                                        </ReadOnlyStyle>
                                    </PropertiesTextEdit>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                        </dxwgv:ASPxGridView>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:ObjectDataSource ID="odsCapacidade" TypeName="Techne.Lyceum.Net.Academico.CapacidadeAlunoCurso"
        runat="server" SelectMethod="Listar" OnDeleting="odsCapacidade_Deleting" DeleteMethod="Delete"
        OnUpdating="odsCapacidade_Updating" UpdateMethod="Update">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlAno" DefaultValue="" Name="ano" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="ddlPeriodo" DefaultValue="" Name="periodo" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
