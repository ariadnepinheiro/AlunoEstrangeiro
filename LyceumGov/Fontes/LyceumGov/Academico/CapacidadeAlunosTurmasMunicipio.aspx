<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="CapacidadeAlunosTurmasMunicipio.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.CapacidadeAlunosTurmasMunicipio" %>

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
            $("#<%= this.txtCapacidade.ClientID %>").numeric();
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
            var selectObjfiltro = ($("#<%=ddlAno.ClientID %>").val() + "/" + $("#<%=ddlPeriodo.ClientID %>").val());
            if (selectObjReplicar != 'Nenhum') {
                if (confirm("Confirma replicação de dados para " + selectObjfiltro + " a partir de " + selectObjReplicar + " ?")) {
                    return true;
                }
                return false;
            }
            return false;
        }
        $(document).ready(function() {
            $('#<%=txtCapacidade.ClientID %>').bind('keyup', function() { blocTexto(this, 5); });
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
                    <asp:Label ID="lblUfTSearch" runat="server" Text="UF:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlUf" runat="server" DataTextField="UF_SIGLA" DataValueField="UF_SIGLA"
                        AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="ddlUf_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right" style="width: 120px">
                    <asp:Label ID="lblMunicipioTSearch" runat="server" Text="Município:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="5">
                    <tweb:TSearchBox ID="tseMunicipio" runat="server" Caption="" SqlOrder="nome" SqlSelect="SELECT codigo, nome, uf_sigla FROM municipio"
                        Columns="10" ArgumentColumns="30" MaxLength="10" OnChanged="tseMunicipio_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                            <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblTipo" runat="server" Text="Tipo:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlTipo" runat="server">
                        <asp:ListItem Text="Selecione" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="0" Text="Mínima"></asp:ListItem>
                        <asp:ListItem Value="1" Text="Máxima"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblCapacidade" runat="server" Text="Capacidade:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCapacidade" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="right">
                    <asp:Button ID="btnSalvar" runat="server" ValidationGroup="SalvarForm" Text="Salvar"
                        Visible="True" OnClick="btnSalvar_Click" />
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
                            KeyFieldName="CAPACIDADEID;CODMUNICIPIO" OnStartRowEditing="grdCapacidade_StartRowEditing" OnAfterPerformCallback="grdCapacidade_AfterPerformCallback">
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
                                <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="CAPACIDADEID" ReadOnly="true" Visible="false"
                                    VisibleIndex="1">
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
                                <dxwgv:GridViewDataTextColumn Caption="Cod. Município" FieldName="CODMUNICIPIO" ReadOnly="true"
                                    Visible="false" VisibleIndex="4">
                                    <PropertiesTextEdit>
                                        <ReadOnlyStyle>
                                            <Border BorderStyle="None"></Border>
                                        </ReadOnlyStyle>
                                    </PropertiesTextEdit>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Município" FieldName="NOME" ReadOnly="true"
                                    Visible="true" VisibleIndex="4">
                                    <PropertiesTextEdit>
                                        <ReadOnlyStyle>
                                            <Border BorderStyle="None"></Border>
                                        </ReadOnlyStyle>
                                    </PropertiesTextEdit>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="UF" FieldName="UFSIGLA" ReadOnly="true" VisibleIndex="3">
                                    <PropertiesTextEdit>
                                        <ReadOnlyStyle>
                                            <Border BorderStyle="None"></Border>
                                        </ReadOnlyStyle>
                                    </PropertiesTextEdit>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Tipo" FieldName="TIPO" ReadOnly="true" Visible="true"
                                    VisibleIndex="5">
                                    <PropertiesTextEdit>
                                        <ReadOnlyStyle>
                                            <Border BorderStyle="None"></Border>
                                        </ReadOnlyStyle>
                                    </PropertiesTextEdit>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataSpinEditColumn Caption="Capacidade*" FieldName="CAPACIDADE" VisibleIndex="6"
                                    Width="70px">
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
				                                                                        e.errorText='Capacidade deve ser positivo';
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
                                <dxwgv:GridViewDataTextColumn Caption="Data" FieldName="DATAALTERACAO" ReadOnly="true"
                                    VisibleIndex="7">
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
    <asp:ObjectDataSource ID="odsCapacidade" TypeName="Techne.Lyceum.Net.Academico.CapacidadeAlunosTurmasMunicipio"
        runat="server" SelectMethod="RetornarCapacidadeAlunoMunicipioPor" OnDeleting="odsCapacidade_Deleting"
        DeleteMethod="Delete" OnUpdating="odsCapacidade_Updating" UpdateMethod="Update">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlAno" DefaultValue="" Name="ano" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="ddlPeriodo" DefaultValue="" Name="periodo" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
