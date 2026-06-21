<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Circuito.aspx.cs" Inherits="Techne.Lyceum.Net.Interconectividade.Circuito" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por unidade administrativa, ano e mês"
        Width="650px">
        <table>
            <tr>
                <td align="left">
                    <asp:Label ID="lblUnidadeAdministrativa" runat="server" Text="Unidade Administrativa:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td style="width: 450px">
                    <tweb:TSearchBox ID="tseUA" runat="server" SqlSelect="SELECT DISTINCT setor, nome FROM VW_ZZCRO_UNIDADE_ADMINSTRATIVA S inner join LYCEUM.FiscalizacaoLink.CONTRATOSETOR CS ON CS.SETORID=S.SETOR"
                        SqlOrder="setor" ColumnName="setor" Caption="" MaxLength="6" DataType="Varchar"
                        OnChanged="tseUA_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="setor" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <div class="divEditBlock" style="width: 850px;">
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:Label runat="server" ID="lblBlocoContrato" Text="Contrato" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsUnidade" runat="server" EnableClientScript="true" ShowMessageBox="true"
            ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <br />
    <dxtc:ASPxPageControl ID="pcContrato" runat="server" ActiveTabIndex="1" Visible="true"
        Width="60%" OnTabClick="pcContrato_TabClick">
        <TabPages>
            <dxtc:TabPage Text="Dados Gerais">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl1" runat="server">
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Link/Circuito">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl2" runat="server">
                        <table>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="Label8" runat="server" Text="Contrato:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <tweb:TSearchBox ID="tseContratoCircuito" runat="server" SqlSelect="select NUMERO ,DESCRICAO, CONTRATOID,CONTRATOSETORID,OPERADORA,SETORID FROM fiscalizacaoLink.VW_CONTRATO"
                                        SqlOrder="numero" ColumnName="numero" Key="numero" Argument="descricao" Caption=""
                                        MaxLength="15" DataType="VarChar" OnChanged="tseContratoCircuito_Changed">
                                        <GridColumns>
                                            <tweb:TSearchBoxColumn Caption="Número" FieldName="numero" Width="20%" />
                                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="60%" />
                                            <tweb:TSearchBoxColumn Caption="contratosetorid" FieldName="contratosetorid" Visible="false" />
                                            <tweb:TSearchBoxColumn Caption="Operadora" FieldName="operadora" Width="20%" />
                                        </GridColumns>
                                    </tweb:TSearchBox>
                                </td>
                            </tr>
                        </table>
                        <asp:ObjectDataSource ID="odsTecnologia" runat="server" SelectMethod="ListaTecnologiaAtiva"
                            TypeName="Techne.Lyceum.RN.FiscalizacaoLink.Tecnologia"></asp:ObjectDataSource>
                        <asp:ObjectDataSource ID="odsVelocidade" runat="server" SelectMethod="ListaVelocidadeAtiva"
                            TypeName="Techne.Lyceum.RN.FiscalizacaoLink.Velocidade"></asp:ObjectDataSource>
                        <asp:ObjectDataSource ID="odsCircuito" runat="server" TypeName="Techne.Lyceum.Net.Interconectividade.Circuito"
                            SelectMethod="ListaCircuito" InsertMethod="Insert" UpdateMethod="Update" DeleteMethod="Delete">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hdnContratoSetorId" Name="contrato" PropertyName="Value" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:HiddenField ID="hdnContratoSetorId" runat="server" />
                        <dxwgv:ASPxGridView ID="grdCircuito" runat="server" DataSourceID="odsCircuito" KeyFieldName="CIRCUITOSETORID"
                            OnRowInserting="grdCircuito_RowInserting" AutoGenerateColumns="false" ClientInstanceName="grdCircuito"
                            OnInitNewRow="grdCircuito_InitNewRow" OnCellEditorInitialize="grdCircuito_CellEditorInitialize"
                            OnStartRowEditing="grdCircuito_StartRowEditing" Width="100%" OnRowUpdating="grdCircuito_RowUpdating"
                            OnRowDeleting="grdCircuito_RowDeleting" OnCommandButtonInitialize="grdCircuito_CommandButtonInitialize">
                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsEditing Mode="Inline" />
                            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                            <SettingsBehavior ConfirmDelete="true" />
                            <Columns>
                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                                    <HeaderCaptionTemplate>
                                        <div style="text-align: center">
                                            <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                                                onclick="grdCircuito.AddNewRow();" />
                                        </div>
                                    </HeaderCaptionTemplate>
                                    <CancelButton Visible="true" Text="Cancelar">
                                        <Image Url="~/img/bt_cancelar.png" />
                                    </CancelButton>
                                    <EditButton Visible="True" Text="Editar">
                                        <Image Url="../img/bt_editar.png" />
                                    </EditButton>
                                    <ClearFilterButton Text="Limpar" Visible="True">
                                        <Image Url="~/img/bt_limpa.png" />
                                    </ClearFilterButton>
                                    <UpdateButton Visible="true" Text="Alterar">
                                        <Image Url="../img/bt_salvar.png" />
                                    </UpdateButton>
                                    <DeleteButton Visible="True" Text="Remover">
                                        <Image Url="../img/bt_exclui2.png" />
                                    </DeleteButton>
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="CIRCUITOSETORID"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="" Name="CONTRATOSETORID" VisibleIndex="2"
                                    FieldName="CONTRATOSETORID" Visible="false">
                                    <PropertiesTextEdit MaxLength="100">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataComboBoxColumn Caption="Tecnologia*" FieldName="TECNOLOGIAID"
                                    VisibleIndex="3" Width="150px">
                                    <PropertiesComboBox DataSourceID="odsTecnologia" TextField="descricao" ValueField="tecnologiaid"
                                        Width="150px" ValueType="System.Int32" DropDownWidth="150px">
                                    </PropertiesComboBox>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataComboBoxColumn Caption="Velocidade*" FieldName="VELOCIDADEID"
                                    VisibleIndex="4" Width="150px">
                                    <PropertiesComboBox DataSourceID="odsVelocidade" TextField="DESCRICAO" ValueField="velocidadeid"
                                        Width="150px" ValueType="System.Int32" DropDownWidth="150px">
                                    </PropertiesComboBox>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Designação" Name="DESIGNACAO" VisibleIndex="5"
                                    FieldName="DESIGNACAO" Width="300px">
                                    <PropertiesTextEdit MaxLength="500">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataSpinEditColumn Caption="Quantidade Meses*" FieldName="QUANTIDADEMESES"
                                    VisibleIndex="6" Width="150px">
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
				                                                                             e.errorText='Quantidade de Meses deve ser um número';
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
                                <dxwgv:GridViewDataTextColumn Caption="Custo Mensal*" FieldName="CUSTOMENSAL" VisibleIndex="7"
                                    Width="100px" CellStyle-Wrap="False">
                                    <PropertiesTextEdit Width="60px" DisplayFormatString="c" ValidationSettings-Display="Dynamic"
                                        MaxLength="9">
                                        <MaskSettings Mask="$&lt;0..9999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Custo Total" Name="CUSTOTOTAL" FieldName="CUSTOTOTAL"
                                    ReadOnly="true" VisibleIndex="8" Width="100px">
                                    <PropertiesTextEdit Width="60px" DisplayFormatString="c" ValidationSettings-Display="Dynamic"
                                        MaxLength="9">
                                        <MaskSettings Mask="$&lt;0..9999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                        <ReadOnlyStyle>
                                            <Border BorderStyle="None"></Border>
                                        </ReadOnlyStyle>
                                    </PropertiesTextEdit>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Início*" FieldName="INICIO" VisibleIndex="9"
                                    Width="130px">
                                    <EditItemTemplate>
                                        <table style="width: 110px">
                                            <tr>
                                                <td>
                                                    <dxe:ASPxDateEdit ID="dtInicio" runat="server" Width="90px" Enabled="true" Value='<%# Bind("INICIO") %>'
                                                        EnableDefaultAppearance="true" ClientInstanceName="dtInicio" CalendarProperties-ClearButtonText="Limpar"
                                                        CalendarProperties-TodayButtonText="Hoje">
                                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                        </CalendarProperties>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Fim" FieldName="FIM" VisibleIndex="10"
                                    Width="130px">
                                    <EditItemTemplate>
                                        <table style="width: 110px">
                                            <tr>
                                                <td>
                                                    <dxe:ASPxDateEdit ID="dtFim" runat="server" Width="90px" Enabled="true" Value='<%# Bind("FIM") %>'
                                                        EnableDefaultAppearance="true" ClientInstanceName="dtFim" CalendarProperties-ClearButtonText="Limpar"
                                                        CalendarProperties-TodayButtonText="Hoje">
                                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                        </CalendarProperties>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                            </tr>
                                        </table>
                                    </EditItemTemplate>
                                </dxwgv:GridViewDataDateColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
</asp:Content>
