<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modulos/LyceumMaster.Master"
    CodeBehind="Cursos.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.Cursos" %>

<%@ Register Assembly="Techne.Lyceum.Infra" Namespace="Techne.Controls" TagPrefix="techne" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="conCursos" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function OnAnoChanged() {
            if (grdDuracao != null) {
                //                if (cmbAno.GetValue() != null) {cmbAno
                //                    grdSubPeriodoLetivo.GetEditor("periodo").PerformCallback(cmbAno.GetValue().toString());
                //                }
            }
        }
    </script>

    <br />
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por escolaridade"
        Width="630px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblCursoTSearch" runat="server" Text="Escolaridade:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseCurso" runat="server" ColumnName="CURSO" MaxLength="20" SqlSelect="SELECT CURSO, NOME FROM ly_curso"
                        OnChanged="tseCurso_Changed" SqlOrder="nome">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="CURSO" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="NOME" Width="70%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <div class="divEditBlock" style="width: 888px;">
        <asp:ImageButton ID="btnExcluir" runat="server" SkinID="BcDeletar" OnClick="btnExcluir_Click"
            OnClientClick="return confirm('Confirma a remoção?');" />
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
            ValidationGroup="SalvarForm" />
        <asp:Label runat="server" ID="lblBloco" Text="Escolaridades" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsDocente" runat="server" ShowMessageBox="true" ValidationGroup="SalvarForm"
            ShowSummary="false" />
    </div>
    <br />
    <asp:Panel ID="pnAbas" runat="server" Width="800px" Visible="true">
        <dxtc:ASPxPageControl ID="pcCursoDuracao" runat="server" ActiveTabIndex="0" Width="800px"
            Visible="true" OnTabClick="pcCursoDuracao_TabClick">
            <TabPages>
                <dxtc:TabPage Text="Dados Gerais">
                    <ContentCollection>
                        <dxw:ContentControl ID="ccDados" runat="server">
                            <div style="text-align: left">
                                <asp:Panel ID="pnDadosGerais" GroupingText="Dados Gerais" runat="server" Width="888px">
                                    <table id="tbCurso" runat="server">
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblCurso" runat="server" Text="Código*: " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtCurso" runat="server" MaxLength="20" Width="114px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblMnemonico" runat="server" Text="Sigla*: " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtMnemonico" Width="50px" runat="server" MaxLength="6">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblNome" runat="server" Text="Nome*: " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtNome" runat="server" MaxLength="100" onKeyUp="Count(this,100)"
                                                    onChange="Count(this,100)" Width="720px">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblModalidade" runat="server" Text="Modalidade*: " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlModalidade" runat="server" DataTextField="DESCRICAO" DataValueField="MODALIDADE">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblTipoCurso" runat="server" Text="Tipo: "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlTipoCurso" runat="server">
                                                    <asp:ListItem Text="Selecione" Value=""></asp:ListItem>
                                                    <asp:ListItem Text="Especial" Value="Especial"></asp:ListItem>
                                                    <asp:ListItem Text="Concomitante/Subsequente" Value="Concomitante/Subsequente"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblNivel" runat="server" Text="Nível/Segmento*: " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="ddlNivel" runat="server" DataTextField="DESCRICAO" DataValueField="TIPO">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblTitulo" runat="server" Text="Certificação*: " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtTitulo" Width="720px" runat="server" MaxLength="200">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="Label4" runat="server" Text="Estrutura Curricular*: " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:RadioButtonList ID="rblEstruturaCurricular" runat="server" RepeatDirection="Horizontal"
                                                    AutoPostBack="true" OnSelectedIndexChanged="rblEstruturaCurricular_SelectedIndexChanged">
                                                    <asp:ListItem Text="Formação geral básica" Value="FormacaoBasica"></asp:ListItem>
                                                    <asp:ListItem Text="Itinerário formativo" Value="Itinerario"></asp:ListItem>
                                                    <asp:ListItem Text="Não se aplica" Value="Nao"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlTrilha" runat="server" Visible="false">
                                                    <table>
                                                        <tr>
                                                            <td style="text-align: right">
                                                                <asp:Label ID="Label7" runat="server" Text="Itinerário Formativo*: " SkinID="lblObrigatorio"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlItinerario" runat="server" DataTextField="DESCRICAO" DataValueField="ITINERARIOFORMATIVOID"
                                                                    OnSelectedIndexChanged="ddlItinerario_SelectedIndexChanged" AutoPostBack="true">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td style="text-align: right">
                                                                <asp:Label ID="Label5" runat="server" Text="Trilha de Aprendizagem*: " SkinID="lblObrigatorio"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlTrilha" runat="server" DataTextField="DESCRICAO" DataValueField="TRILHAAPRENDIZAGEMID">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align: right">
                                                                <asp:Label ID="Label6" runat="server" Text="Máximo Componente por Série:*" SkinID="lblObrigatorio"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtMaxComponente" Width="200px" runat="server" MaxLength="3">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblAtivo" runat="server" Text="Ativo: "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkAtivo" runat="server" />
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label ID="Label3" runat="server" Text="Oferta Eletiva: "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkOfertaEletiva" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblReclasificacao" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                    Text="Tem Reclassificação:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkReclassificacao" runat="server" />
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblFormatura" runat="server" Text="Concluintes Anteriores: "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkFormatura" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblConcomitante" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                    Text="Educação Profissional Concomitante:"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:CheckBox ID="chkConcomitante" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="Label1" runat="server" Font-Names="Verdana" Font-Size="Smaller" Text="Sala Externa:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkSalaExterna" runat="server" />
                                                <asp:HiddenField ID="hdnSalaExterna" runat="server" />
                                            </td>
                                            <td style="text-align: right; width: 500px" colspan="2">
                                                <asp:Panel ID="pnlTurnoseVagas" GroupingText="Confirmação de Turnos e Vagas" runat="server">
                                                    <asp:Label ID="lblParticipaCalculoNovasTurmasTurnosVagas" runat="server" Font-Names="Verdana"
                                                        Font-Size="Smaller" Text="Participa do cálculo de percentual da criação de turmas novas:"></asp:Label>
                                                    <asp:CheckBox ID="chkParticipaCalculoNovasTurmasTurnosVagas" Checked="true" runat="server" />
                                                    <asp:HiddenField ID="hdnParticipaCalculoNovasTurmasTurnosVagas" runat="server" />
                                                    <br />
                                                    <asp:Label ID="Label2" runat="server" Font-Names="Verdana" Font-Size="Smaller" Text="Permite Choque de Turno Integral:"></asp:Label>
                                                    <asp:CheckBox ID="chkChoqueHorarioIntegral" runat="server" />
                                                    <asp:HiddenField ID="hdnChoqueHorarioIntegral" runat="server" />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblParticipaFechamentoAutomatico" runat="server" Font-Names="Verdana"
                                                    Font-Size="Smaller" Text="Participa Fechamento Automático Ano Letivo:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkParticipaFechamentoAutomatico" runat="server" />
                                                <asp:HiddenField ID="hdnParticipaFechamentoAutomatico" runat="server" />
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblPermiteTransferenciaTurmaTotal" runat="server" Font-Names="Verdana"
                                                    Font-Size="Smaller" Text="Permite transferência irrestrita:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkPermiteTransferenciaTurmaTotal" runat="server" />
                                                <asp:HiddenField ID="hdnPermiteTransferenciaTurmaTotal" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblHabilitacao" runat="server" Text="Habilitação: " Enabled="false"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtHabilitacao" runat="server" Width="420px" TextMode="MultiLine"
                                                    Height="40px" MaxLength="255" onKeyUp="Count(this,255)" onChange="Count(this,255)"
                                                    Enabled="false">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblDecreto" runat="server" Text="Decreto: "></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtDecreto" runat="server" Width="420px" TextMode="MultiLine" Height="40px"
                                                    MaxLength="255" onKeyUp="Count(this,255)" onChange="Count(this,255)">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblDataDOU" runat="server" Text="Data DO: "></asp:Label>
                                            </td>
                                            <td>
                                                <dxe:ASPxDateEdit ID="dtDOU" runat="server" MinDate="1901-01-01">
                                                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                    </CalendarProperties>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblVagas" runat="server" Text="Capacidade de Atendimento: "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtVagas" runat="server" MaxLength="9" SkinID="numerico" Width="150px"
                                                    MinimumValue="0">
                                                </asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvVagas" runat="server" ControlToValidate="txtTitulo"
                                                    InitialValue="" ErrorMessage="Capacidade de Atendimento: Preenchimento obrigatório."
                                                    ValidationGroup="SalvarForm" Enabled="false"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Text="Duração Aulas">
                    <ContentCollection>
                        <dxw:ContentControl ID="ccDuracao" runat="server">
                            <dxwgv:ASPxGridView ID="grdDuracao" DataSourceID="odsDuracao" runat="server" AutoGenerateColumns="False"
                                ClientInstanceName="grdDuracao" KeyFieldName="ID_CURSO_DURACAO" OnCellEditorInitialize="grdDuracao_CellEditorInitialize"
                                OnStartRowEditing="grdDuracao_StartRowEditing" OnAfterPerformCallback="grdDuracao_AfterPerformCallback"
                                OnRowValidating="grdDuracao_RowValidating">
                                <SettingsBehavior ConfirmDelete="True" />
                                <SettingsEditing Mode="Inline" />
                                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                <Columns>
                                    <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                        <HeaderCaptionTemplate>
                                            <div style="text-align: center">
                                                <img src="../img/bt_novo.png" style="cursor: pointer" onclick="grdDuracao.AddNewRow();"
                                                    runat="server" id="btnNovoGrid" alt="Novo" />
                                            </div>
                                        </HeaderCaptionTemplate>
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
                                        <ClearFilterButton Text="Limpar" Visible="true">
                                            <Image Url="~/img/bt_limpa.png" />
                                        </ClearFilterButton>
                                    </dxwgv:GridViewCommandColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Id" FieldName="ID_CURSO_DURACAO" VisibleIndex="1"
                                        Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataSpinEditColumn Caption="Ano" FieldName="ANO" VisibleIndex="2"
                                        Width="70px" PropertiesSpinEdit-MaxLength="4">
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
				                                                                e.errorText='O Ano deve ser positivo';
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
                                    <dxwgv:GridViewDataComboBoxColumn Caption="Turno" FieldName="TURNO" VisibleIndex="3"
                                        Width="150px">
                                        <PropertiesComboBox DataSourceID="tdsTurno" TextField="descricao" ValueField="turno"
                                            Width="150px" ValueType="System.String" DropDownWidth="150px">
                                        </PropertiesComboBox>
                                    </dxwgv:GridViewDataComboBoxColumn>
                                    <dxwgv:GridViewDataSpinEditColumn Caption="Duração em minutos" FieldName="DURACAO"
                                        VisibleIndex="4" Width="70px" PropertiesSpinEdit-MaxLength="2">
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
				                                                                e.errorText='A duração deve ser positiva';
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
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Text="Itinerário Formativo(Censo)">
                    <ContentCollection>
                        <dxw:ContentControl ID="ccItinerario" runat="server">
                            <asp:Panel ID="pnlUnidadeCurricular" GroupingText="Unidade curricular" runat="server"
                                Visible="false">
                                <asp:CheckBoxList ID="chkUnidadeCurricular" runat="server" RepeatColumns="3" RepeatDirection="Vertical"
                                    RepeatLayout="Table" Width="100%">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <br />
                            <br />
                            <asp:Panel ID="pnlAreaItinerarioFormativo" GroupingText="Área do itinerário formativo"
                                runat="server" Visible="false">
                                <asp:CheckBoxList ID="chkAreaItinerarioFormativo" runat="server" RepeatColumns="3"
                                    AutoPostBack="true" RepeatDirection="Vertical" RepeatLayout="Table" Width="100%"
                                    OnSelectedIndexChanged="chkAreaItinerarioFormativo_SelectedIndexChanged">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <br />
                            <br />
                            <asp:Panel ID="pnlComposicaoItinerarioFormativo" GroupingText="Composição do itinerário formativo integrado"
                                runat="server" Visible="false">
                                <asp:CheckBoxList ID="chkComposicaoItinerario" runat="server" RepeatColumns="3" RepeatDirection="Vertical"
                                    RepeatLayout="Table" Width="100%">
                                </asp:CheckBoxList>
                            </asp:Panel>
                            <br />
                            <br />
                            <asp:Panel ID="pnlTipoCursoItinerario" GroupingText="Tipo de curso do itinerário de formação técnica e profissional"
                                runat="server" Visible="false">
                                <asp:RadioButtonList ID="rblTipoCursoItinerario" runat="server" RepeatDirection="Horizontal">
                                </asp:RadioButtonList>
                            </asp:Panel>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
            </TabPages>
        </dxtc:ASPxPageControl>
    </asp:Panel>
    <techne:TTableDataSource ID="tdsTurno" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_turno"
        SqlOrder="descricao">
    </techne:TTableDataSource>
    <asp:ObjectDataSource ID="odsDuracao" SelectMethod="Listar" TypeName="Techne.Lyceum.Net.Academico.Cursos"
        OnDeleting="odsDuracao_Deleting" DeleteMethod="Delete" OnUpdating="odsDuracao_Updating"
        UpdateMethod="Update" OnInserting="odsDuracao_Inserting" InsertMethod="Insert"
        runat="server">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseCurso" Name="tseCurso" PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
