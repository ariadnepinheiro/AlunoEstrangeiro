<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Avaliacao.aspx.cs" Inherits="Techne.Lyceum.Net.Interconectividade.Avaliacao" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function grdInterrupcao_EndCallback(s, e) {
            if (s.cpShowDeleteConfirmBox) {
                pucConfirm.Show();
            }
        }       
    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por unidade administrativa, ano e mês"
        Width="650px">
        <table>
            <tr>
                <td align="left">
                    <asp:Label ID="lblUnidadeAdministrativa" runat="server" Text="Unidade Administrativa:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td style="width: 450px">
                    <tweb:TSearchBox ID="tseUnidadeAdministrativa" runat="server" SqlSelect=" SELECT DISTINCT s.setor, nome, ue.UNIDADE_ENS, ua_atual, ua_antiga FROM VW_ZZCRO_UNIDADE_ADMINSTRATIVA S inner join HADES..VW_SETOR se on S.SETOR = se.SETOR inner join LYCEUM.FiscalizacaoLink.CONTRATOSETOR CS ON CS.SETORID=S.SETOR left join LY_UNIDADE_ENSINO ue on S.SETOR = ue.SETOR "
                        SqlOrder="setor" ColumnName="setor" Caption="" MaxLength="6"
                        DataType="Varchar" OnChanged="tseUnidadeAdministrativa_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="setor" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="10%" />                           
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="60%" />
                            <tweb:TSearchBoxColumn Caption="Censo" FieldName="UNIDADE_ENS" Width="10%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="Label1" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAno" runat="server" AutoPostBack="True" DataTextField="ano"
                        DataValueField="ano" Width="90px" Height="20px" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="Label2" runat="server" Text="Mês:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMes" runat="server" AutoPostBack="True" DataTextField="DESCRICAO"
                        DataValueField="mes" Width="90px" Height="20px" OnSelectedIndexChanged="ddlMes_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="Label3" runat="server" Text="Contrato:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseContrato" runat="server" SqlSelect="select DISTINCT numero , descricao, C.contratoid, cs.contratosetorid  from [LYCEUM].[FiscalizacaoLink].[CONTRATO] C inner join [LYCEUM].[FiscalizacaoLink].[CONTRATOSETOR] CS ON C.CONTRATOID=CS.CONTRATOID "
                        SqlOrder="numero" ColumnName="numero" Key="numero" Argument="descricao" Caption=""
                        MaxLength="50" SqlWhere=" SETORID=#tseUnidadeAdministrativa# " DataType="VarChar"
                        OnChanged="tseContrato_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="contratosetorid" Width="20%" Visible="false" />
                            <tweb:TSearchBoxColumn Caption="Número" FieldName="numero" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="Label7" runat="server" Text="Circuito:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseCircuito" runat="server" SqlSelect="select DISTINCT CODIGO,DESCRICAO FROM FiscalizacaoLink.VW_CIRCUITO"
                        SqlOrder="DESCRICAO" ColumnName="circuitosetorid" Key="circuitosetorid" 
                        Caption="" MaxLength="10" SqlWhere=" numero=#tseContrato# AND SETORID=#tseUnidadeAdministrativa# "
                        DataType="Number" OnChanged="tseCircuito_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="circuitosetorid" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
        <br />
        <asp:Label ID="lblMensagemAvaliacao" runat="server" SkinID="lblMensagem"></asp:Label>
    </asp:Panel>
    <br />
    <div class="divEditBlock" style="width: 850px;">
        <asp:ImageButton ID="btnEnviarFaturamento" runat="server" ImageUrl="~/Images/bot_EnviarFaturamento.png"
            ImageAlign="Right" OnClick="btnEnviarFaturamento_Click" Visible="false" ToolTip="Enviar para Faturamento" 
            OnClientClick="return confirm('Após esta ação os dados não poderão ser alterados.\nDeseja realmente enviar a Avaliação para o Faturamento?');" />
        <asp:ImageButton ID="btnSalvar" runat="server" ImageUrl="~/Images/bt_salvar.png"
            ToolTip="Salvar Avaliação" OnClick="btnSalvar_Click" Visible="false" ValidationGroup="SalvarForm"
            ImageAlign="Right" />
        <asp:Label runat="server" ID="lblBlocoAvaliacao" Text="Avaliação" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsAvaliacao" runat="server" EnableClientScript="true"
            ShowMessageBox="true" ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:Panel ID="pnlResposta" runat="server" GroupingText="Resposta" Width="650px">
        <table>
            <tr>
                <td align="left">
                    <asp:RadioButtonList ID="rblResposta" runat="server" RepeatDirection="Vertical" Width="350px"
                        OnSelectedIndexChanged="rblResposta_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="Declaro que NÃO HOUVE interrupção do serviço" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Declaro que HOUVE interrupção do serviço" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnlInterrupcao" runat="server" GroupingText="Dados Interrupção" Width="650px">
        <asp:HiddenField ID="hdnContratoSetorId" runat="server" />
        <asp:HiddenField ID="hdnAvaliacaoId" runat="server" />
        <asp:HiddenField ID="hdnRespostaAvaliacao" runat="server" />
        <asp:HiddenField ID="hdnInterrupcao" runat="server" />
        <table>
            <tr>
                <td align="left">
                    <asp:Label ID="Label4" runat="server" Text="Número do Chamado:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtChamado" runat="server" MaxLength="50" Width="170px" />
                </td>
                <td align="left">
                    <asp:Label ID="Label9" runat="server" Text="Motivo:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMotivoInterrupcao" runat="server" DataTextField="DESCRICAO"
                        DataValueField="MOTIVOINTERRUPCAOID">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="Label10" runat="server" Text="Complemento Motivo:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtComplemento" runat="server" MaxLength="5000" Width="170px" TextMode="MultiLine" />
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="Label5" runat="server" Text="Data/Hora Interrupção:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dtInterrupcao" runat="server" Width="127px" Enabled="true"
                        EnableDefaultAppearance="true" ClientInstanceName="dtInterrupcao" CalendarProperties-ClearButtonText="Limpar"
                        CalendarProperties-TodayButtonText="Hoje" Height="18px">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                </td>
                <td>
                    /
                </td>
                <td>
                    <dxe:ASPxTextBox ID="txtHoraInterrupcao" runat="server" DisplayFormatString="HH:mm"
                        MaxLength="5" Width="40px">
                        <MaskSettings Mask="##:##" />
                    </dxe:ASPxTextBox>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="Label6" runat="server" Text="Data/Hora Reestabelecimento:"></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dtReestabelecimento" runat="server" Width="127px" Enabled="true"
                        EnableDefaultAppearance="true" ClientInstanceName="dtReestabelecimento" CalendarProperties-ClearButtonText="Limpar"
                        CalendarProperties-TodayButtonText="Hoje">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                </td>
                <td>
                    /
                </td>
                <td>
                    <dxe:ASPxTextBox ID="txtHoraReestabelecimento" runat="server" DisplayFormatString="HH:mm"
                        MaxLength="5" Width="40px">
                        <MaskSettings Mask="##:##" />
                    </dxe:ASPxTextBox>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label ID="Label8" runat="server" Text="Tipo de Problema:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:RadioButtonList ID="rblTipoProblema" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Interno" Value="Interno"></asp:ListItem>
                        <asp:ListItem Text="Externo" Value="Externo"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Panel ID="pnlGridInterrupcao" runat="server" Visible="true" Width="850px">
        <asp:ObjectDataSource ID="odsInterrupcao" runat="server" TypeName="Techne.Lyceum.Net.Interconectividade.Avaliacao"
            SelectMethod="Lista" UpdateMethod="Update" DeleteMethod="Delete">
            <SelectParameters>
                <asp:ControlParameter ControlID="hdnAvaliacaoId" Name="avaliacao" PropertyName="Value" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="odsMotivoInterrupcao" runat="server" SelectMethod="ListaMotivoInterrupcaoAtiva" TypeName="Techne.Lyceum.RN.FiscalizacaoLink.MotivoInterrupcao">
        </asp:ObjectDataSource>
        <dxwgv:ASPxGridView ID="grdInterrupcao" runat="server" DataSourceID="odsInterrupcao"
            KeyFieldName="INTERRUPCAOID" OnRowDeleting="grdInterrupcao_RowDeleting" AutoGenerateColumns="false"
            ClientInstanceName="grdInterrupcao" OnInitNewRow="grdInterrupcao_InitNewRow"
            OnInit="grdInterrupcao_Init" OnCellEditorInitialize="grdInterrupcao_CellEditorInitialize"
            OnStartRowEditing="grdInterrupcao_StartRowEditing" Width="100%" OnRowUpdating="grdInterrupcao_RowUpdating"
            OnCommandButtonInitialize="grdInterrupcao_CommandButtonInitialize">
            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsEditing Mode="Inline" />
            <SettingsBehavior ConfirmDelete="False" />
            <ClientSideEvents EndCallback="grdInterrupcao_EndCallback" />
            <Columns>
                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
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
                    <DeleteButton Text="Remover" Visible="True">
                        <Image Url="~/img/bt_exclui2.png" />
                    </DeleteButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="INTERRUPCAOID"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="" Name="CIRCUITOSETORID" VisibleIndex="2"
                    FieldName="CIRCUITOSETORID" Visible="false">
                    <PropertiesTextEdit MaxLength="100">
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="" Name="AVALIACAOID" VisibleIndex="2" FieldName="AVALIACAOID"
                    Visible="false">
                    <PropertiesTextEdit MaxLength="100">
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Chamado*" Name="Chamado" VisibleIndex="2"
                    FieldName="CHAMADO" Width="130px" ReadOnly="true">
                    <PropertiesTextEdit>
                        <ReadOnlyStyle>
                            <Border BorderStyle="None"></Border>
                        </ReadOnlyStyle>
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Contrato*" Name="DESCRICAOCONTRATO" VisibleIndex="3"
                    FieldName="DESCRICAOCONTRATO" Width="130px" ReadOnly="true">
                    <PropertiesTextEdit>
                        <ReadOnlyStyle>
                            <Border BorderStyle="None"></Border>
                        </ReadOnlyStyle>
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Circuito*" Name="DESIGNACAOCIRCUITO" VisibleIndex="4"
                    FieldName="DESIGNACAOCIRCUITO" Width="130px" ReadOnly="true">
                    <PropertiesTextEdit>
                        <ReadOnlyStyle>
                            <Border BorderStyle="None"></Border>
                        </ReadOnlyStyle>
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="MotivoInterrupcao*" FieldName="MOTIVOINTERRUPCAOID"
                    VisibleIndex="5" Width="150px">
                    <PropertiesComboBox DataSourceID="odsMotivoInterrupcao" TextField="descricao" ValueField="motivointerrupcaoid"
                        Width="150px" ValueType="System.Int32" DropDownWidth="150px">
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataTextColumn Caption="Complemento Motivo" Name="MOTIVOCOMPLEMENTO"
                    VisibleIndex="6" FieldName="MOTIVOCOMPLEMENTO" Width="230px">
                    <PropertiesTextEdit MaxLength="5000" >
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Data/Hora Interrupção*" FieldName="DATAINTERRUPCAO"
                    VisibleIndex="7" Width="130px">
                    <EditItemTemplate>
                        <table style="width: 110px">
                            <tr>
                                <td>
                                    <dxe:ASPxDateEdit ID="dtInterrupcao" runat="server" Width="90px" Enabled="true" Value='<%# Bind("DATAINTERRUPCAO") %>'
                                        EnableDefaultAppearance="true" ClientInstanceName="dtInterrupcao" CalendarProperties-ClearButtonText="Limpar"
                                        CalendarProperties-TodayButtonText="Hoje">
                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                        </CalendarProperties>
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td>
                                    <dxe:ASPxTextBox ID="txtHoraInterrupcao" runat="server" DisplayFormatString="HH:mm"
                                        Text='<%#Bind("HORAINTERRUPCAO") %>' MaxLength="5" Width="40px">
                                        <MaskSettings Mask="##:##" />
                                    </dxe:ASPxTextBox>
                                </td>
                            </tr>
                        </table>
                    </EditItemTemplate>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Data/Hora Reestabelecimento" FieldName="DATAREESTABELECIMENTO"
                    VisibleIndex="8" Width="110px">
                    <EditItemTemplate>
                        <table>
                            <tr>
                                <td>
                                    <dxe:ASPxDateEdit ID="dtReestabelecimento" runat="server" Width="90px" Enabled="true"
                                        Value='<%# Bind("DATAREESTABELECIMENTO") %>' EnableDefaultAppearance="true" ClientInstanceName="dtReestabelecimento"
                                        CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                        </CalendarProperties>
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td>
                                    <dxe:ASPxTextBox ID="txtHoraReestabelecimento" runat="server" DisplayFormatString="HH:mm"
                                        Text='<%#Bind("HORAREESTABELECIMENTO") %>' MaxLength="5" Width="40px">
                                        <MaskSettings Mask="##:##" />
                                    </dxe:ASPxTextBox>
                                </td>
                            </tr>
                        </table>
                    </EditItemTemplate>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataColumn Caption="Tipo de problema*" Name="TIPOPROBLEMA" VisibleIndex="9"
                    FieldName="TIPOPROBLEMA" CellStyle-Border-BorderStyle="None">
                    <EditItemTemplate>
                        <dxe:ASPxRadioButtonList ID="rblTipoProblema" runat="server" EnableViewState="false"
                            RepeatDirection="Horizontal" Value='<%# Bind("TIPOPROBLEMA") %>' ValueType="System.String"
                            AutoPostBack="False" ValidationSettings-ValidationGroup="<%# Container.ValidationGroup %>">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                <RequiredField ErrorText="Favor informar o tipo do problema." IsRequired="True" />
                            </ValidationSettings>
                            <Items>
                                <dxe:ListEditItem Text="Interno" Value="Interno" />
                                <dxe:ListEditItem Text="Externo" Value="Externo" />
                            </Items>
                        </dxe:ASPxRadioButtonList>
                    </EditItemTemplate>
                </dxwgv:GridViewDataColumn>
            </Columns>
        </dxwgv:ASPxGridView>
        <dxpc:ASPxPopupControl ID="pucConfirm" runat="server" ClientInstanceName="pucConfirm"
            ShowHeader="false" Width="500px" Modal="True" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter">
            <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
            <ContentCollection>
                <dxpc:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
                    <table width="100%">
                        <tr>
                            <td colspan="2" align="center">
                                Você está excluindo a última interrupção cadastrada.
                                <br />
                                Se confirmar, a resposta da avaliação será atualizada para
                                <br />
                                <b>"Declaro que NÃO HOUVE interrupção do serviço".</b>
                                <br />
                                Deseja realmente remover?
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center;">
                                <asp:Button ID="btnConfirmarExclusao" runat="server" Text="Sim" OnClick="btnConfirmarExclusao_Click"
                                    Width="80px" />
                                <asp:Button ID="btnCancelar" runat="server" Text="Não" OnClick="btnCancelar_Click"
                                    Width="80px" />
                            </td>
                        </tr>
                    </table>
                </dxpc:PopupControlContentControl>
            </ContentCollection>
        </dxpc:ASPxPopupControl>
    </asp:Panel>
</asp:Content>
