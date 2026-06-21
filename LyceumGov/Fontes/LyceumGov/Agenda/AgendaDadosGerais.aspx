<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="AgendaDadosGerais.aspx.cs" Inherits="Techne.Lyceum.Net.Agenda.AgendaDadosGerais"
    Title="Agenda de eventos" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">

        function UpdateUploadButton() {
            btnUpload.SetEnabled(document.getElementById("<%= FileUpload.ClientID %>").value != "");
        }

        function IniciaUpload() {
            window.setTimeout("popupProcessamento.Show();", 500);
            document.getElementById("<%= importaArquivo.ClientID %>").value = "1";
        }

    </script>

    <div class="divEditBlock" style="width: 90%;">
        <asp:ImageButton ID="btnVoltar" runat="server" ImageAlign="Right" SkinID="Voltar"
            OnClick="btnCancel_Click" />
        <asp:Label runat="server" ID="lblBloco" Text="Agenda de Eventos" SkinID="BcTitulo" />
    </div>
    <asp:UpdatePanel ID="upVisualizaEditalProcessoSeletivo" runat="server">
        <ContentTemplate>
            <dxtc:ASPxPageControl ID="pcAgenda" runat="server" AutoPostBack="true" ActiveTabIndex="0"
                Height="348px" Width="895px" ClientInstanceName="pcAgenda" OnTabClick="pcAgenda_TabClick">
                <TabPages>
                    <dxtc:TabPage Text="Agenda">
                        <ContentCollection>
                            <dxw:ContentControl ID="ccAgenda" runat="server">
                                <br />
                                <br />
                                <asp:Panel ID="pnlDadosGerais" GroupingText="Dados gerais" runat="server" Enabled="false">
                                    <table>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblDescricaoDadosGerais" runat="server" SkinID="lblObrigatorio" Text="Descrição:*"></asp:Label>
                                            </td>
                                            <td colspan="4">
                                                <asp:TextBox ID="txtDescricaoDadosGerais" runat="server" MaxLength="8" Width="400px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblDatainicio" runat="server" SkinID="lblObrigatorio" Text="Data Inicio:*"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDataInicioDadosGerais" runat="server" MaxLength="100" ReadOnly="True"
                                                    Width="90px"></asp:TextBox>
                                            </td>
                                            <td style="text-align: right;" colspan="2">
                                                <asp:Label ID="lblDataFim" runat="server" SkinID="lblObrigatorio" Text="Data Fim:*"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDataFimDadosGerais" runat="server" MaxLength="100" ReadOnly="True"
                                                    Width="90px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblTipoEvento" runat="server" SkinID="lblObrigatorio" Text="Tipo do Evento:*"></asp:Label>
                                            </td>
                                            <td colspan="4">
                                                <asp:DropDownList ID="ddlTipoEvento" DataTextField="NOME" DataValueField="TIPOEVENTOID"
                                                    runat="server" Width="410px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblAnoPeriodo" runat="server" SkinID="lblObrigatorio" Text="Ano/Periodo:*"></asp:Label>
                                            </td>
                                            <td colspan="4">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList ID="ddlAno" runat="server" DataTextField="ANO" DataValueField="ANO"
                                                                Width="100px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBoxList ID="cblPeriodo" runat="server" DataTextField="PERIODO" DataValueField="ID_REDUZIDA"
                                                                RepeatDirection="Horizontal">
                                                            </asp:CheckBoxList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblUnidadeParticipante" runat="server" SkinID="lblObrigatorio" Text="Unidade Participantes do evento:*"></asp:Label>
                                            </td>
                                            <td colspan="4">
                                                <asp:RadioButtonList ID="rblUnidadeParticipante" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Todas" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Informar somente as que participam" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Informar somente as que NÃO participam" Value="2"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblCursoParticipante" runat="server" SkinID="lblObrigatorio" Text="Cursos participantes do evento:*"></asp:Label>
                                            </td>
                                            <td colspan="4">
                                                <asp:RadioButtonList ID="rblCursoParticipante" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Text="Todas" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Informar somente as que participam" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Informar somente as que NÃO participam" Value="2"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblCursoPorUnidade" runat="server" SkinID="lblObrigatorio" Text="Informar cursos por unidade:*"></asp:Label>
                                            </td>
                                            <td colspan="4">
                                                <asp:CheckBox ID="chkCursoPorUnidade" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblObservacao" runat="server" SkinID="lblObrigatorio" Text="Observações:*"></asp:Label>
                                            </td>
                                            <td colspan="4">
                                                <asp:TextBox ID="txtObservacao" runat="server" TextMode="MultiLine" Width="400px"
                                                    Height="100px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                    <dxtc:TabPage Text="Cursos por Unidade">
                        <ContentCollection>
                            <dxw:ContentControl ID="ccCursosPorUnidade" runat="server">
                                <br />
                                <br />
                                <dxwgv:ASPxGridView ID="grdCursoPorUnidade" runat="server" KeyFieldName="AGENDAID"
                                    ClientInstanceName="grdCursoPorUnidade" AutoGenerateColumns="False" Width="100%">
                                    <SettingsBehavior AllowMultiSelection="False" AllowSort="true" ProcessSelectionChangedOnServer="true" />
                                    <Styles CommandColumn-Wrap="False">
                                        <CommandColumn Wrap="False">
                                        </CommandColumn>
                                    </Styles>
                                    <Columns>
                                        <dxwgv:GridViewDataTextColumn VisibleIndex="2" Caption="Código" FieldName="UNIDADEENSINOID"
                                            Width="90px" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Left">
                                            <PropertiesTextEdit MaxLength="255">
                                            </PropertiesTextEdit>
                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                            <CellStyle HorizontalAlign="Left">
                                            </CellStyle>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn VisibleIndex="2" Caption="Unidade de Ensino" FieldName="NOMEUNIDADEENSINO"
                                            Width="300px" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Left">
                                            <PropertiesTextEdit MaxLength="255">
                                            </PropertiesTextEdit>
                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                            <CellStyle HorizontalAlign="Left">
                                            </CellStyle>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="CURSOID" VisibleIndex="3"
                                            Width="90px" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Left">
                                            <PropertiesTextEdit MaxLength="100">
                                            </PropertiesTextEdit>
                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                            <CellStyle HorizontalAlign="Left">
                                            </CellStyle>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn VisibleIndex="4" Caption="Curso" FieldName="NOMECURSO"
                                            Width="300px" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Left">
                                            <PropertiesTextEdit MaxLength="100">
                                            </PropertiesTextEdit>
                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                            <CellStyle HorizontalAlign="Left">
                                            </CellStyle>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn VisibleIndex="5" Caption="Série" FieldName="SERIE"
                                            Width="50px" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Left">
                                            <PropertiesTextEdit MaxLength="100">
                                            </PropertiesTextEdit>
                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                            <CellStyle HorizontalAlign="Left">
                                            </CellStyle>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn VisibleIndex="6" Caption="Turno" FieldName="TURNOS"
                                            Width="150px" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Left">
                                            <PropertiesTextEdit MaxLength="100">
                                            </PropertiesTextEdit>
                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                            <CellStyle HorizontalAlign="Left">
                                            </CellStyle>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                                            Visible="False" VisibleIndex="7">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn FieldName="AGENDAID" VisibleIndex="8" Caption="AGENDAID"
                                            Visible="false" Width="0px">
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                                </dxwgv:ASPxGridView>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                    <dxtc:TabPage Text="Processo Seletivo">
                        <ContentCollection>
                            <dxw:ContentControl ID="ccProcessoSeletivo" runat="server">
                                <br />
                                <br />
                                <asp:Panel ID="pnlProcessoSeletivo" GroupingText="Dados gerais" runat="server">
                                    <table>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblNumeroEdital" runat="server" SkinID="lblObrigatorio" Text="Número do Edital:*"></asp:Label>
                                            </td>
                                            <td colspan="6">
                                                <asp:TextBox ID="txtNumeroEdital" runat="server" Enabled="false" Width="350px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblTextoEdital" runat="server" SkinID="lblObrigatorio" Text="Texto do edital:*"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTextoEdital" runat="server" Enabled="false" ReadOnly="True" Width="350px"></asp:TextBox>
                                                <asp:ImageButton ID="imgbtnVisualizaEditalProcessoSeletivo" OnClick="btnVisualizaEditalProcessoSeletivo_Click"
                                                    runat="server" Width="16px" Height="16px" ImageUrl="~/img/bt_busca.png" />
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label ID="Label1" runat="server" SkinID="lblObrigatorio" Text="Data de Nascimento:*"></asp:Label>
                                            </td>
                                            <td colspan="5">
                                                <table cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td>
                                                            <dxe:ASPxDateEdit ID="dtNascimentoInicial" ReadOnly="true" Enabled="false" runat="server"
                                                                MinDate="1901-01-01" Width="120px" ClientInstanceName="dtNasc" CalendarProperties-ClearButtonText="Limpar">
                                                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                                </CalendarProperties>
                                                            </dxe:ASPxDateEdit>
                                                        </td>
                                                        <td style="padding: 0 4px 0 4px;">
                                                            <asp:Label ID="lblA" runat="server" SkinID="lblObrigatorio" Text=" a "></asp:Label>
                                                        </td>
                                                        <td>
                                                            <dxe:ASPxDateEdit ReadOnly="true" Enabled="false" ID="dtNascimentoFinal" runat="server"
                                                                MinDate="1901-01-01" Width="120px" ClientInstanceName="dtNasc" CalendarProperties-ClearButtonText="Limpar">
                                                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                                </CalendarProperties>
                                                            </dxe:ASPxDateEdit>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <br />
                                <asp:Panel ID="pnlProcSelGrid" GroupingText="Parametrização Unidade de Ensino Participante"
                                    runat="server">
                                    <dxwgv:ASPxGridView ID="grdUnidadeEnsinoProcessoSeletivo" runat="server" KeyFieldName="AGENDAID"
                                        ClientInstanceName="grdUnidadeEnsinoProcessoSeletivo" AutoGenerateColumns="False"
                                        Width="100%">
                                        <SettingsBehavior AllowMultiSelection="False" AllowSort="true" ProcessSelectionChangedOnServer="true" />
                                        <Styles CommandColumn-Wrap="False">
                                            <CommandColumn Wrap="False">
                                            </CommandColumn>
                                        </Styles>
                                        <Columns>
                                            <dxwgv:GridViewDataTextColumn VisibleIndex="2" Caption="Unidade de Ensino" FieldName="NOMEUNIDADEENSINO"
                                                Width="200px" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Left">
                                                <PropertiesTextEdit MaxLength="255">
                                                </PropertiesTextEdit>
                                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                <CellStyle HorizontalAlign="Left">
                                                </CellStyle>
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn VisibleIndex="6" Caption="Mensagem" FieldName="MENSAGEM"
                                                Width="400px" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Left">
                                                <PropertiesTextEdit MaxLength="100">
                                                </PropertiesTextEdit>
                                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                                                <CellStyle HorizontalAlign="Left">
                                                </CellStyle>
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                                                Visible="False" VisibleIndex="7">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn FieldName="AGENDAID" VisibleIndex="8" Caption="AGENDAID"
                                                Visible="false" Width="0px">
                                            </dxwgv:GridViewDataTextColumn>
                                        </Columns>
                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                                    </dxwgv:ASPxGridView>
                                </asp:Panel>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                    <dxtc:TabPage Text="Eventos">
                        <ContentCollection>
                            <dxw:ContentControl ID="ccEventos" runat="server">
                                <br />
                                <br />
                                <dxwgv:ASPxGridView ID="grdEventos" runat="server" KeyFieldName="AGENDAID" Visible="False"
                                    ClientInstanceName="grdEventos" AutoGenerateColumns="False" Width="100%">
                                    <SettingsBehavior AllowMultiSelection="False" AllowSort="true" ProcessSelectionChangedOnServer="true" />
                                    <Styles CommandColumn-Wrap="False">
                                        <CommandColumn Wrap="False">
                                        </CommandColumn>
                                    </Styles>
                                    <Columns>
                                        <dxwgv:GridViewDataTextColumn VisibleIndex="4" Caption="Tipo de Evento" FieldName="NOME"
                                            Width="400" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Left">
                                            <PropertiesTextEdit MaxLength="100">
                                            </PropertiesTextEdit>
                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                            <CellStyle HorizontalAlign="Left">
                                            </CellStyle>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn VisibleIndex="5" Caption="Data Início" FieldName="DATAINICIO"
                                            Width="100" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Left">
                                            <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy" MaxLength="100">
                                            </PropertiesTextEdit>
                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                            <CellStyle HorizontalAlign="Left">
                                            </CellStyle>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn VisibleIndex="6" Caption="Data Fim" FieldName="DATAFIM"
                                            Width="100" HeaderStyle-Font-Bold="true" CellStyle-HorizontalAlign="Left">
                                            <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy" MaxLength="100">
                                            </PropertiesTextEdit>
                                            <HeaderStyle Font-Bold="True"></HeaderStyle>
                                            <CellStyle HorizontalAlign="Left">
                                            </CellStyle>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                                            Visible="False" VisibleIndex="7">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn FieldName="AGENDAID" VisibleIndex="8" Caption="AGENDAID"
                                            Visible="false" Width="0px">
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                                </dxwgv:ASPxGridView>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                    <dxtc:TabPage Text="Gestão Processo Seletivo">
                        <ContentCollection>
                            <dxw:ContentControl ID="ccGestaoProcessoSeletivo" runat="server">
                                <br />
                                <asp:Panel ID="pnlExportacaoCandidatosInscritos" GroupingText="Exportação dos Candidatos Inscritos no Processo Seletivo"
                                    runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <dxe:ASPxButton ID="btnExportacaoCandidatosInscritos" runat="server" OnClick="btnExportacaoCandidatosInscritos_Click"
                                                    Text="Exportação dos Candidatos Inscritos" ClientInstanceName="btnExportacaoCandidatosInscritos">
                                                </dxe:ASPxButton>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <br />
                                <br />
                                <asp:Panel ID="pnlImportacaoCandidatosClassificados" GroupingText="Importação de Arquivo de Classificados do Processo Seletivo"
                                    runat="server">
                                    <asp:HiddenField ID="importaArquivo" EnableViewState="true" runat="server" Value="" />
                                    <table>
                                        <tr>
                                            <td style="width: 700px">
                                                <br />
                                                <asp:Panel ID="pnLocalizarArquivoImportacao" runat="server" GroupingText="Localizar arquivo para importação"
                                                    Width="700px">
                                                    <table>
                                                        <tr>
                                                            <td style="text-align: right; width: 60px">
                                                                <asp:Label ID="lblTituloArquivoImportacao" runat="server" Text="Arquivo:*" SkinID="lblObrigatorio"></asp:Label>
                                                            </td>
                                                            <td style="height: 70px">
                                                                <input type="file" id="FileUpload" name="FileUpload" onchange="UpdateUploadButton()"
                                                                    enableviewstate="true" size="70" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </td>
                                            <td align="left">
                                                <dxe:ASPxButton ID="btnUpload" runat="server" AutoPostBack="False" Text="Iniciar Importação"
                                                    ClientInstanceName="btnUpload" ClientEnabled="False">
                                                    <ClientSideEvents Click="function(s, e) { IniciaUpload(); __doPostBack('',''); }" />
                                                </dxe:ASPxButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 15px;" colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="height: 100%;">
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <dxwgv:ASPxGridView ID="grdHistoricoImportacaoProcessoSeletivo" runat="server" AutoGenerateColumns="False"
                                                                ClientInstanceName="grdHistoricoImportacaoProcessoSeletivo" KeyFieldName="HISTORICOIMPORTACAOID"
                                                                Styles-Cell-Paddings-PaddingTop="8px" Styles-Cell-Paddings-PaddingBottom="8px"
                                                                OnHtmlDataCellPrepared="grdHistoricoImportacaoProcessoSeletivo_HtmlDataCellPrepared"
                                                                OnHtmlRowCreated="grdHistoricoImportacaoProcessoSeletivo_HtmlRowCreated" EnableCallBacks="false">
                                                                <SettingsBehavior AllowMultiSelection="False" AllowSort="true" />
                                                                <SettingsText EmptyDataRow="Não existem dados." />
                                                                <Columns>
                                                                    <dxwgv:GridViewDataTextColumn Caption="Id Importação" FieldName="HISTORICOIMPORTACAOID"
                                                                        VisibleIndex="0" Visible="false" CellStyle-HorizontalAlign="Left">
                                                                    </dxwgv:GridViewDataTextColumn>
                                                                    <dxwgv:GridViewDataTextColumn Caption="Data Importação" FieldName="DATAIMPORTACAO"
                                                                        VisibleIndex="1" Width="400px" CellStyle-HorizontalAlign="Left">
                                                                    </dxwgv:GridViewDataTextColumn>
                                                                    <dxwgv:GridViewDataTextColumn Caption="Número Chamada" FieldName="NUMEROCHAMADA"
                                                                        VisibleIndex="2" Width="150px" CellStyle-HorizontalAlign="Left">
                                                                    </dxwgv:GridViewDataTextColumn>
                                                                    <dxwgv:GridViewDataTextColumn Caption="Nomer Arquivo" FieldName="NOMEARQUIVO" VisibleIndex="3"
                                                                        Width="500px" CellStyle-HorizontalAlign="Left">
                                                                    </dxwgv:GridViewDataTextColumn>
                                                                    <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="STATUSPROCESSAMENTO"
                                                                        VisibleIndex="4" CellStyle-HorizontalAlign="Left" Width="180px" CellStyle-BorderBottom-BorderColor="#bfd7f3"
                                                                        CellStyle-BorderBottom-BorderStyle="Solid" CellStyle-BorderBottom-BorderWidth="1px">
                                                                        <DataItemTemplate>
                                                                            <%# Techne.Lyceum.Net.Util.Utils.GetEnumDescription((Techne.Lyceum.RN.HistoricoImportacao.StatusProcessamento)Convert.ToInt32(Eval("STATUSPROCESSAMENTO")))%>
                                                                        </DataItemTemplate>
                                                                    </dxwgv:GridViewDataTextColumn>
                                                                    <dxwgv:GridViewDataTextColumn Caption="Total Candidatos" FieldName="TOTALREGISTROIMPORTADO"
                                                                        VisibleIndex="5" Width="150px" CellStyle-HorizontalAlign="Left">
                                                                    </dxwgv:GridViewDataTextColumn>
                                                                    <dxwgv:GridViewDataTextColumn Caption="Arquivo/Log" Name="ArquivoLog" VisibleIndex="6"
                                                                        CellStyle-HorizontalAlign="Center" Width="90px">
                                                                        <DataItemTemplate>
                                                                            <asp:ImageButton ID="ArquivoLog" runat="server" EnableViewState="false" CommandArgument='<%# Eval("HISTORICOIMPORTACAOID") %>'
                                                                                OnCommand="ArquivoLog_Command" ImageUrl="~/img/bt_busca.png" Height="15px" AlternateText="Visualizar Arquivo/Log">
                                                                            </asp:ImageButton>
                                                                        </DataItemTemplate>
                                                                    </dxwgv:GridViewDataTextColumn>
                                                                </Columns>
                                                            </dxwgv:ASPxGridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <dxpc:ASPxPopupControl ID="popupProcessamento" runat="server" ClientInstanceName="popupProcessamento"
                                        Modal="True" CloseAction="None" Width="80px" Height="40px" PopupHorizontalAlign="WindowCenter"
                                        PopupVerticalAlign="WindowCenter" AllowDragging="False" PopupAnimationType="None"
                                        ShowHeader="false" HeaderText="Importando arquivo..." ShowCloseButton="False"
                                        ShowPageScrollbarWhenModal="true">
                                        <ContentCollection>
                                            <dxpc:PopupControlContentControl ID="popupProcessamentoConteudo" runat="server">
                                                <table style="width: 100%; height: 100%;">
                                                    <tr>
                                                        <td style="width: 100%; height: 100%;" align="center">
                                                            <img id="imgProcessando" alt="" src="../Images/ajax-loader.gif" />
                                                            <br />
                                                            <asp:Label ID="lblProcessando" runat="server" Text="Processando..."></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </dxpc:PopupControlContentControl>
                                        </ContentCollection>
                                    </dxpc:ASPxPopupControl>
                                </asp:Panel>
                                <br />
                                <br />
                                <asp:Panel ID="pnlGeracaoPreMatricula" GroupingText="Geração de Pré-Matrícula dos Candidatos Classificados do Processo Seletivo"
                                    runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <dxe:ASPxButton ID="btnGeracaoPreMatricula" runat="server" OnClick="btnGeracaoPreMatricula_Click"
                                                    Text="Gerar Pré-Matrícula">
                                                </dxe:ASPxButton>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <dxwgv:ASPxGridView ID="grdHistoricoGeracaoPreMatricula" runat="server" AutoGenerateColumns="False"
                                                    ClientInstanceName="grdHistoricoGeracaoPreMatricula" KeyFieldName="HISTORICOGERACAOPREMATRICULAID"
                                                    Styles-Cell-Paddings-PaddingTop="8px" Styles-Cell-Paddings-PaddingBottom="8px"
                                                    EnableCallBacks="false">
                                                    <SettingsBehavior AllowMultiSelection="False" AllowSort="true" />
                                                    <SettingsText EmptyDataRow="Não existem dados." />
                                                    <Columns>
                                                        <dxwgv:GridViewDataTextColumn Caption="Id_Historico_geracao" FieldName="HISTORICOGERACAOPREMATRICULAID"
                                                            VisibleIndex="0" Visible="false">
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="Data da Geração" FieldName="DATAGERACAO" VisibleIndex="1"
                                                            Width="200px">
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="Número da Chamada" FieldName="NUMEROCHAMADA"
                                                            VisibleIndex="1" Width="200px">
                                                        </dxwgv:GridViewDataTextColumn>
                                                        <dxwgv:GridViewDataTextColumn Caption="Total de Candidatos" FieldName="TOTALPREMATRICULAGERADA"
                                                            VisibleIndex="1" Width="200px">
                                                        </dxwgv:GridViewDataTextColumn>
                                                    </Columns>
                                                </dxwgv:ASPxGridView>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <br />
                                <br />
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                </TabPages>
            </dxtc:ASPxPageControl>
            <dxpc:ASPxPopupControl ID="popupAgenda" ClientInstanceName="popupAgenda" runat="server"
                Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false" ShowCloseButton="true"
                ShowFooter="false" ShowHeader="True" HeaderText="Edital" ShowSizeGrip="False"
                EnableAnimation="false" Width="900px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
                <ContentCollection>
                    <dxpc:PopupControlContentControl>
                        <div id="divPopup" runat="server" visible="false">
                            <table style="width: 850px">
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td style="height: 5px;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div id="divConteudoPopup" style="overflow: auto; width: 850px; height: 400px; border: solid 1px;"
                                                        runat="server">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 5px;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 10px;">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </dxpc:PopupControlContentControl>
                </ContentCollection>
            </dxpc:ASPxPopupControl>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="updPrg" runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel1" CssClass="overlay" runat="server">
                <asp:Panel ID="Panel2" CssClass="loader" runat="server">
                    <asp:Image ID="Image1" runat="server" ImageUrl="../Images/updateProgress.gif" AlternateText="Updating..."
                        Height="48" Width="48" />
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
