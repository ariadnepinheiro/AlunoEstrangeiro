<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="CadastroSaladeAula.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.CadastroSaladeAula" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        function abrirPopup() {

            window.setTimeout(function() {
                pucConfirmarDesativacao.Show();
            }, 1000);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Informe os dados para pesquisar as salas de aula"
        Width="500">
        <div>
            <table>
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label ID="lblRegional" runat="server" Font-Names="Verdana" Text="Regional:"></asp:Label>
                    </td>
                    <td colspan="3">
                        <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                            MaxLength="20" Columns="10" AutoPostBack="True" Caption="" OnChanged="tseRegional_Changed"
                            SqlOrder="regional" Key="id_regional" SqlSelect="SELECT DISTINCT S.id_regional AS id_regional, regional FROM VW_UNIDADE_ENSINO_SITUACAO S INNER JOIN TCE_REGIONAL R ON S.ID_REGIONAL=R.ID_REGIONAL"
                            DataType="Number">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:"></asp:Label>
                    </td>
                    <td colspan="3">
                        <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
                            SqlWhere=" id_regional = #tseRegional# " GridWidth="600px" ArgumentColumns="50"
                            OnChanged="tseMunicipio_Changed" Columns="10" MaxLength="10">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                                <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" SkinID="lblObrigatorio"
                            runat="server" Text="Unidade de Ensino:*"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                            MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,id_regional,municipio,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO "
                            SqlWhere=" id_regional = #tseRegional# AND municipio = #tseMunicipio# " GridWidth="850px"
                            OnChanged="tseUnidadeResponsavel_Changed" SqlOrder="nome_comp">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                                <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="10%" />
                                <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvUnidadeResponsavel" runat="server" ControlToValidate="tseUnidadeResponsavel"
                            ErrorMessage="Unidade de Ensino: Preenchimento obrigatório." InitialValue=""
                            ValidationGroup="ConfirmarForm"><img 
                                                alt="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 15%">
                        &nbsp;
                    </td>
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:HiddenField ID="hdnDependencia" runat="server" />
    <asp:Panel ID="pnlGrid" runat="server" Visible="false">
        <dxwgv:ASPxGridView ID="grdSaladeAula" runat="server" AutoGenerateColumns="False"
            EnableCallbackCompression="True" EnableCallBacks="false" DataSourceID="odsSaladeAula"
            ClientInstanceName="grdSaladeAula" KeyFieldName="CompositeKey" OnAfterPerformCallback="grdSaladeAula_AfterPerformCallback"
            OnRowValidating="grdSaladeAula_RowValidating" OnRowInserting="grdSaladeAula_RowInserting"
            OnRowUpdating="grdSaladeAula_RowUpdating" OnRowDeleting="grdSaladeAula_RowDeleting"
            OnCustomUnboundColumnData="grdSaladeAula_CustomUnboundColumnData" OnCustomButtonCallback="grdSaladeAula_CustomButtonCallback">
            <SettingsEditing Mode="EditForm" />
            <Columns>
                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                    <HeaderCaptionTemplate>
                        <div style="text-align: center">
                            <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                onclick="grdSaladeAula.AddNewRow();" alt="Novo" />
                        </div>
                    </HeaderCaptionTemplate>
                    <EditButton Text="Editar" Visible="True">
                        <Image Url="~/img/bt_editar.png" />
                    </EditButton>
                    <UpdateButton Text="Salvar">
                        <Image Url="~/img/bt_salvar.png" />
                    </UpdateButton>
                    <CancelButton Text="Cancelar">
                        <Image Url="~/img/bt_cancelar.png" />
                    </CancelButton>
                    <CustomButtons>
                        <dxwgv:GridViewCommandColumnCustomButton Text="Desativar" ID="btnExcluir" Visibility="AllDataRows"
                            Image-Url="~/img/bt_exclui2.png" Image-Height="15px" Image-AlternateText="Excluir">
                        </dxwgv:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="Sala de Aula" FieldName="DEPENDENCIA" VisibleIndex="2">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Capacidade Máxima de Alunos" FieldName="NUM_ALUNOS"
                    VisibleIndex="3">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataCheckColumn Caption="Sala Anexa?" FieldName="SALA_ANEXA" VisibleIndex="3"
                    Width="50px" HeaderStyle-Font-Bold="true">
                    <PropertiesCheckEdit ValueChecked="S" ValueUnchecked="N" ValueType="System.String">
                    </PropertiesCheckEdit>
                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                </dxwgv:GridViewDataCheckColumn>
                <dxwgv:GridViewDataTextColumn Caption="Área(m²)" FieldName="AREA" VisibleIndex="4">
                    <PropertiesTextEdit DisplayFormatString="{0:G}" Width="150px">
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                    VisibleIndex="5" Visible="False">
                </dxwgv:GridViewDataTextColumn>
            </Columns>
            <Templates>
                <EditForm>
                    <table>
                        <tr>
                            <td>
                                Sala de Aula:
                            </td>
                            <td style="color: #FF0000">
                                <dxe:ASPxTextBox runat="server" ID="txtSala" Text='<%# Bind("DEPENDENCIA") %>' Width="150"
                                    Enabled="false" Visible="false">
                                </dxe:ASPxTextBox>
                                Gerado automaticamente
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Sala Anexa?:
                            </td>
                            <td style="color: #FF0000">
                                <dxe:ASPxCheckBox ID="chkSalaAnexa" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                    runat="server" Checked="false" Value='<%# Bind("SALA_ANEXA") %>'>
                                </dxe:ASPxCheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Capacidade Máxima de Alunos:
                            </td>
                            <td style="color: #FF0000">
                                <dxe:ASPxSpinEdit Enabled="false" runat="server" ID="txtNumAlunos" Value='<%# Bind("NUM_ALUNOS") %>'
                                    Width="100" DisplayFormatString="g" SpinButtons-ShowIncrementButtons="false"
                                    NumberType="Integer" MaxLength="3" Visible="false">
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
				                                                                e.errorText='O Número de Alunos deve ser positivo';
			                                                                }
		                                                                }
	                                                                }
	                                                                catch(ex)
	                                                                {
		                                                                e.isValid = false;
		                                                                e.errorText=ex;
	                                                                }
                                                                }" />
                                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                        <RequiredField ErrorText="Favor informar a Número de Alunos" IsRequired="True" />
                                    </ValidationSettings>
                                </dxe:ASPxSpinEdit>
                                Calculado de acordo com Área
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Área(m²):
                            </td>
                            <td>
                                <dxe:ASPxSpinEdit runat="server" ID="txtArea" MaxLength="3" Width="100" SpinButtons-ShowIncrementButtons="false"
                                    Value='<%# Bind("AREA") %>' DisplayFormatString="{0:G}" NumberType="Integer">
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
				                                                                e.errorText='A Área deve ser positiva';
			                                                                }
		                                                                }
	                                                                }
	                                                                catch(ex)
	                                                                {
		                                                                e.isValid = false;
		                                                                e.errorText=ex;
	                                                                }
                                                                }" />
                                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                        <RequiredField ErrorText="Favor informar a Área" IsRequired="True" />
                                    </ValidationSettings>
                                </dxe:ASPxSpinEdit>
                            </td>
                        </tr>
                    </table>
                    <div style="text-align: right; padding: 5px;">
                        <dxwgv:ASPxGridViewTemplateReplacement ID="UpdateButton" ReplacementType="EditFormUpdateButton"
                            runat="server">
                        </dxwgv:ASPxGridViewTemplateReplacement>
                        <dxwgv:ASPxGridViewTemplateReplacement ID="CancelButton" ReplacementType="EditFormCancelButton"
                            runat="server">
                        </dxwgv:ASPxGridViewTemplateReplacement>
                    </div>
                </EditForm>
            </Templates>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        </dxwgv:ASPxGridView>
        <asp:ObjectDataSource ID="odsSaladeAula" TypeName="Techne.Lyceum.Net.Basico.CadastroSaladeAula"
            runat="server" SelectMethod="Listar" DeleteMethod="Delete">
            <SelectParameters>
                <asp:ControlParameter ControlID="tseUnidadeResponsavel" DefaultValue="" Name="unidade_ens"
                    PropertyName="DBValue" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
    <dxpc:ASPxPopupControl ID="pucConfirmarDesativacao" ClientInstanceName="pucConfirmarDesativacao"
        runat="server" Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false"
        ShowCloseButton="false" ShowFooter="false" ShowHeader="false" ShowSizeGrip="False"
        EnableAnimation="false" Width="300px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <table>
                    <tr align="center">
                        <td>
                            <asp:Label Font-Names="Verdana" ID="lblTexto" SkinID="lblObrigatorio" runat="server"></asp:Label>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr align="center">
                        <td style="text-align: center;">
                            <asp:Button ID="btnSim" runat="server" Text="Sim" OnClick="btnSim_Click" />
                            <asp:Button ID="btnNao" runat="server" Text="Não" OnClick="btnNao_Click" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
