<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="CadastroRestricaoIdadeSerie.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.CadastroRestricaoIdadeSerie" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript" language="javascript">
        $(document).ready(function() {
            $("#<%= this.txtIdadeMaxima.ClientID %>").numeric();
            $("#<%= this.txtIdadeMinima.ClientID %>").numeric();

            $('#<%=txtIdadeMaxima.ClientID %>').bind('keyup', function() { blocTexto(this, 3); });
            $('#<%=txtIdadeMinima.ClientID %>').bind('keyup', function() { blocTexto(this, 3); });
        });

        function blocTexto(campo, qtde) {
            var quant = qtde;
            var valor = $.trim($(campo).val());
            var total = valor.length;

            if (total > quant) {
                $(campo).val(valor.substr(0, quant));
            }
        }

        function BloquearCtrl() {
            if (event.keyCode == 17) {
                alert("Proibido utilizar o Ctrl neste campo");
            }
        }

        function desabilitaBotaoDireito() {
            if (event.button == 2) {
                alert("Proibido utilizar o botao direito neste campo");
            }
        }
    </script>

    <asp:Panel ID="pnGeral" runat="server" GroupingText="Informe os dados para inclusão/Consulta:"
        Width="800px">
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="lblModalidadeTSearch" runat="server" Text="Modalidade:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseModalidade" runat="server" Argument="descricao" Caption=""
                        Key="modalidade" SqlOrder="descricao" SqlSelect="SELECT modalidade, descricao FROM ly_modalidade_curso"
                        OnChanged="tseModalidade_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="modalidade" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblNivelTSearch" runat="server" Text="Segmento:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseNivel" runat="server" Argument="descricao" Caption="" Key="tipo"
                        SqlOrder="descricao" SqlSelect="SELECT tipo, descricao FROM ly_tipo_curso" OnChanged="tseNivel_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="tipo" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 120px">
                    <asp:Label ID="lblCursoTSearch" runat="server" Text="Escolaridade:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseCurso" runat="server" Argument="nome" Caption="" Key="curso"
                        SqlOrder="nome" SqlSelect="SELECT curso, nome FROM ly_curso" SqlWhere="tipo = #tseNivel# and tipo is not null AND modalidade = #tseModalidade# and modalidade is not null"
                        OnChanged="tseCurso_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="curso" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="width: 120px; text-align: right;">
                    <asp:Label ID="Label1" runat="server" Text="Série:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbSerie" AutoPostBack="True" runat="server" DataTextField="serie"
                        DataValueField="serie" AppendDataBoundItems="true" OnSelectedIndexChanged="cmbSerie_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblIdadeMinima" runat="server" Text="Idade Mínima:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtIdadeMinima" runat="server" Width="100px" MaxLength="3"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblIdadeMaxima" runat="server" Text="Idade Máxima:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtIdadeMaxima" runat="server" Width="100px" MaxLength="3"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4" style="text-align: right;">
                    <asp:Button ID="btnSalvar" runat="server" ValidationGroup="SalvarForm" Text="Salvar"
                        OnClick="btnSalvar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <table>
        <tr>
            <td>
                <asp:Panel ID="pnGrid" runat="server">
                    <dxwgv:ASPxGridView ID="grdRestricao" runat="server" AutoGenerateColumns="False"
                        OnAfterPerformCallback="grdRestricao_AfterPerformCallback" ClientInstanceName="grdRestricao"
                        DataSourceID="odsRestricao" KeyFieldName="ID_RESTRICAO_IDADE_SERIE">
                        <SettingsBehavior ConfirmDelete="True" />
                        <SettingsEditing Mode="Inline" />
                        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                        <Columns>
                            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                <EditButton Text="Editar" Visible="True">
                                    <Image Url="~/img/bt_editar.png" />
                                </EditButton>
                                <CancelButton Text="Cancelar">
                                    <Image Url="~/img/bt_cancelar.png" />
                                </CancelButton>
                                <UpdateButton>
                                    <Image Url="~/img/bt_salvar.png" />
                                </UpdateButton>
                                <DeleteButton Text="Remover" Visible="True">
                                    <Image Url="~/img/bt_exclui2.png" />
                                </DeleteButton>
                                <ClearFilterButton Text="Limpar" Visible="True">
                                    <Image Url="~/img/bt_limpa.png" />
                                </ClearFilterButton>
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="ID_RESTRICAO_IDADE_SERIE"
                                ReadOnly="true" VisibleIndex="1">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Modalidade" FieldName="MODALIDADE" ReadOnly="true"
                                VisibleIndex="2">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Segmento" FieldName="SEGMENTO" ReadOnly="true"
                                VisibleIndex="3">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Curso" FieldName="CODIGO_CURSO" ReadOnly="true"
                                VisibleIndex="4">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Escolaridade" FieldName="NOME_CURSO" ReadOnly="true"
                                VisibleIndex="5">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Série" FieldName="SERIE" ReadOnly="true" VisibleIndex="6">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataSpinEditColumn Caption="Idade Mínima" FieldName="IDADE_MINIMA"
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
				                                                                        e.errorText='Por favor, informe uma idade válida para o campo IDADE MÍNIMA.';
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
                            <dxwgv:GridViewDataSpinEditColumn Caption="Idade Máxima" FieldName="IDADE_MAXIMA"
                                VisibleIndex="8" Width="70px">
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
				                                                                        e.errorText='Por favor, informe uma idade válida para o campo IDADE MÁXIMA.';
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
                </asp:Panel>
            </td>
        </tr>
    </table>
    <br />
    <asp:ObjectDataSource ID="odsRestricao" TypeName="Techne.Lyceum.Net.Basico.CadastroRestricaoIdadeSerie"
        runat="server" SelectMethod="Listar" OnUpdating="odsRestricao_Updating" UpdateMethod="Update"
        OnDeleting="odsRestricao_Deleting" DeleteMethod="Delete"></asp:ObjectDataSource>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
</asp:Content>
