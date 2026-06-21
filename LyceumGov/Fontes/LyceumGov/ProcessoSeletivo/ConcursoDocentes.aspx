<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ConcursoDocentes.aspx.cs" Inherits="Techne.Lyceum.Net.ProcessoSeletivo.ConcursoDocentes" %>

<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxTitleIndex"
    TagPrefix="dxti" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<asp:Content ID="conConcursoDocentes" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">        k
        function AddSelectedItems() {
            MoveSelectedItems(lbAvailable, lbChoosen);
            UpdateButtonState();
        }
        function AddAllItems() {
            MoveAllItems(lbAvailable, lbChoosen);
            UpdateButtonState();
        }
        function RemoveSelectedItems() {
            MoveSelectedItems(lbChoosen, lbAvailable);
            UpdateButtonState();
        }
        function RemoveAllItems() {
            MoveAllItems(lbChoosen, lbAvailable);
            UpdateButtonState();
        }
        function MoveSelectedItems(srcListBox, dstListBox) {
            srcListBox.BeginUpdate();
            dstListBox.BeginUpdate();
            var items = srcListBox.GetSelectedItems();
            for (var i = items.length - 1; i >= 0; i = i - 1) {
                dstListBox.AddItem(items[i].text, items[i].value);
                srcListBox.RemoveItem(items[i].index);
            }
            srcListBox.EndUpdate();
            dstListBox.EndUpdate();
        }
        function MoveAllItems(srcListBox, dstListBox) {
            srcListBox.BeginUpdate();
            var count = srcListBox.GetItemCount();
            for (var i = 0; i < count; i++) {
                var item = srcListBox.GetItem(i);
                dstListBox.AddItem(item.text, item.value);
            }
            srcListBox.EndUpdate();
            srcListBox.ClearItems();
        }
        function UpdateButtonState() {
            btnMoveAllItemsToRight.SetEnabled(lbAvailable.GetItemCount() > 0);
            btnMoveAllItemsToLeft.SetEnabled(lbChoosen.GetItemCount() > 0);
            btnMoveSelectedItemsToRight.SetEnabled(lbAvailable.GetSelectedItems().length > 0);
            btnMoveSelectedItemsToLeft.SetEnabled(lbChoosen.GetSelectedItems().length > 0);
        }
    </script>

    <dxpc:ASPxPopupControl ID="ppcMensagem" runat="server" CloseAction="CloseButton"
        HeaderText="Habilitações" Modal="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        AllowDragging="true" ClientInstanceName="pucItensDespesa" EnableAnimation="true"
        EnableViewState="false" ShowCloseButton="true" Width="520px" Height="380px" HeaderStyle-BackColor="#BFD7F3"
        HeaderStyle-Font-Bold="true" ContentStyle-Font-Bold="true">
        <Border BorderColor="Gainsboro" BorderStyle="Ridge" BorderWidth="4px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,15000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl ID="ppcccMensagem" runat="server">
                <table>
                    <tr>
                        <td style="text-align: right; width: 15%">
                            <asp:Label ID="Label3" runat="server" Font-Names="Verdana" Text="Regional:" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                                AutoPostBack="true" MaxLength="20" Columns="10" Caption="" Key="id_regional"
                                SqlSelect="SELECT distinct RE.id_regional,RE.regional FROM TCE_REGIONAL RE "
                                SqlOrder="regional" DataType="Number">
                                <GridColumns>
                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                                    <tweb:TSearchBoxColumn Caption="REGIONAL" FieldName="regional" Width="80%" />
                                </GridColumns>
                            </tweb:TSearchBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 15%">
                            <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:"
                                Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <tweb:TSearchBox ID="tseMunicipio2" runat="server" Key="codigo" Argument="nome" SqlSelect="select distinct codigo, nome, uf_sigla from LY_UNIDADE_ENSINO u join municipio m on u.MUNICIPIO= m.CODIGO "
                                SqlWhere=" SIT_FUNCIONAMENTO='EmAtividade' and ID_REGIONAL = #tseRegional# "
                                SqlOrder="nome" GridWidth="600px" Columns="10" AutoPostBack="true" MaxLength="10"
                                OnChanged="tseMunicipio2_Changed">
                                <GridColumns>
                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                    <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                                    <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                                </GridColumns>
                            </tweb:TSearchBox>
                        </td>
                    </tr>
                </table>
                <br />
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td valign="top" style="width: 35%">
                            <div class="BottomPadding">
                                <asp:Label Font-Names="Verdana" ID="Label1" runat="server" Text="Disciplinas Disponíveis:"
                                    Font-Bold="true" ForeColor="#004A80"></asp:Label>
                            </div>
                            <br />
                            <dxe:ASPxListBox ID="listFrom" TextField="descricao" ValueField="agrupamento" runat="server"
                                ClientInstanceName="lbAvailable" Width="100%" Height="240px" SelectionMode="CheckColumn">
                                <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonState(); }" />
                            </dxe:ASPxListBox>
                        </td>
                        <td valign="middle" align="center" style="padding: 10px; width: 30%">
                            <div>
                                <dxe:ASPxButton ID="btnMoveSelectedItemsToRight" runat="server" ClientInstanceName="btnMoveSelectedItemsToRight"
                                    AutoPostBack="False" Text=">" Width="130px" Height="23px" ClientEnabled="False"
                                    ToolTip="Adicionar items selecionados">
                                    <ClientSideEvents Click="function(s, e) { AddSelectedItems(); }" />
                                </dxe:ASPxButton>
                            </div>
                            <div class="TopPadding">
                                <dxe:ASPxButton ID="btnMoveAllItemsToRight" runat="server" ClientInstanceName="btnMoveAllItemsToRight"
                                    AutoPostBack="False" Text=">>" Width="130px" Height="23px" ToolTip="Adiciona todos os items">
                                    <ClientSideEvents Click="function(s, e) { AddAllItems(); }" />
                                </dxe:ASPxButton>
                            </div>
                            <div style="height: 32px">
                            </div>
                            <div>
                                <dxe:ASPxButton ID="btnMoveSelectedItemsToLeft" runat="server" ClientInstanceName="btnMoveSelectedItemsToLeft"
                                    AutoPostBack="False" Text="<" Width="130px" Height="23px" ClientEnabled="False"
                                    ToolTip="Remover items selecionados">
                                    <ClientSideEvents Click="function(s, e) { RemoveSelectedItems(); }" />
                                </dxe:ASPxButton>
                            </div>
                            <div class="TopPadding">
                                <dxe:ASPxButton ID="btnMoveAllItemsToLeft" runat="server" ClientInstanceName="btnMoveAllItemsToLeft"
                                    AutoPostBack="False" Text="<<" Width="130px" Height="23px" ClientEnabled="False"
                                    ToolTip="Remover todos os Items">
                                    <ClientSideEvents Click="function(s, e) { RemoveAllItems(); }" />
                                </dxe:ASPxButton>
                            </div>
                        </td>
                        <td valign="top" style="width: 35%">
                            <div class="BottomPadding">
                                <asp:Label Font-Names="Verdana" ID="Label4" runat="server" Text="Disciplinas Habilitadas:"
                                    Font-Bold="true" ForeColor="#004A80"></asp:Label>
                            </div>
                            <br />
                            <dxe:ASPxListBox ID="listTo" runat="server" ClientInstanceName="lbChoosen" Width="100%"
                                Height="240px" SelectionMode="CheckColumn">
                                <ClientSideEvents SelectedIndexChanged="function(s, e) { UpdateButtonState(); }">
                                </ClientSideEvents>
                            </dxe:ASPxListBox>
                        </td>
                    </tr>
                </table>
                <div>
                    <br />
                    <dxe:ASPxButton ClientInstanceName="btnSalvar" AutoPostBack="true" UseSubmitBehavior="false"
                        ID="btnGravar" OnClick="btnGravar_Click" runat="server" Text="Salvar">
                    </dxe:ASPxButton>
                </div>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
    <input id="hdnField" runat="server" type="hidden" />
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por processo seletivo"
        Width="650px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblConcursoSearch" runat="server" Text="Processo Seletivo:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseConcurso" runat="server" Key="concurso" Argument="descricao"
                        OnChanged="tseConcurso_Changed" MaxLength="20" SqlSelect="SELECT concurso, descricao,ano from LY_CONCURSO_DOCENTE"
                        GridWidth="850px" SqlOrder=" ano desc">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="concurso" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
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
    <div class="divEditBlock" style="width: 640px;">
        <asp:ImageButton ID="btnExcluir" runat="server" SkinID="BcDeletar" OnClick="btnExcluir_Click"
            OnClientClick="return confirm('Confirma a remoção?');" />
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
            ValidationGroup="SalvarForm" />
        <asp:Label runat="server" ID="lblBloco" Text="Processos Seletivos" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsSetor" runat="server" ShowMessageBox="true" ValidationGroup="SalvarForm"
            ShowSummary="false" />
    </div>
    <techne:TTableDataSource ID="tdsHabilitacoes" runat="server" DataTableClassName="Techne.Lyceum.CR.LY_CONCURSO_DOC_HABILITACAO"
        SqlWhere="concurso = @concurso">
        <SqlWhereParameters>
            <asp:SessionParameter Name="concurso" SessionField="vsConcurso" Type="String" />
        </SqlWhereParameters>
    </techne:TTableDataSource>
    <asp:ObjectDataSource ID="odsHabilitacoes" runat="server" SelectMethod="ConsultaHabilitacoesConcurso"
        TypeName="Techne.Lyceum.RN.ConcursoDocHabilitacao" OldValuesParameterFormatString="original_{0}">
        <SelectParameters>
            <asp:SessionParameter Name="concurso" SessionField="vsConcurso" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <techne:TTableDataSource ID="tdsExperiencias" runat="server" DataTableClassName="Techne.Lyceum.CR.LY_CONCURSO_DOC_EXPERIENCIA"
        SqlWhere="concurso = @concurso" SqlOrder="pontuacao">
        <SqlWhereParameters>
            <asp:SessionParameter Name="concurso" SessionField="vsConcurso" Type="String" />
        </SqlWhereParameters>
    </techne:TTableDataSource>
    <techne:TTableDataSource ID="tdsTitulacoes" runat="server" DataTableClassName="Techne.Lyceum.CR.LY_CONCURSO_DOC_TITULACOES"
        SqlWhere="concurso = @concurso" SqlOrder="pontuacao">
        <SqlWhereParameters>
            <asp:SessionParameter Name="concurso" SessionField="vsConcurso" Type="String" />
        </SqlWhereParameters>
    </techne:TTableDataSource>
    <asp:ObjectDataSource ID="odsCargos" runat="server" SelectMethod="ConsultaCargosConcurso"
        TypeName="Techne.Lyceum.RN.ContratoTemporario.ConcursoDocente_CategoriaDocente"
        OldValuesParameterFormatString="original_{0}" InsertMethod="InserirCargosConcurso">
        <SelectParameters>
            <asp:SessionParameter Name="concurso" SessionField="vsConcurso" Type="String" />
        </SelectParameters>
        <InsertParameters>
            <asp:SessionParameter Name="concurso" SessionField="vsConcurso" Type="String" />
            <asp:SessionParameter Name="categoria" SessionField="vsCategoria" Type="String" />
        </InsertParameters>
    </asp:ObjectDataSource>
    <dxtc:ASPxPageControl ID="pcConcursoDocentes" runat="server" ActiveTabIndex="1" Height="348px"
        Width="895px" ClientInstanceName="pcConcursoDocentes">
        <TabPages>
            <dxtc:TabPage Text="Dados do Processo Seletivo">
                <ContentCollection>
                    <dxw:ContentControl ID="ccDados" runat="server">
                        <table>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblConcurso" runat="server" Text="Processo Seletivo:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan ="2">
                                    <asp:TextBox ID="txtConcurso" runat="server" MaxLength="20" Width="180px" />
                                    <asp:RequiredFieldValidator ID="rfvConcurso" runat="server" ControlToValidate="txtConcurso"
                                        InitialValue="" ErrorMessage="Processo Seletivo: Preenchimento obrigatório."
                                        ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                    <asp:CheckBox runat="server" ID="chkIndigena" Text="Indígena" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="Label2" runat="server" Text="Função:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan ="2">
                                    <tweb:TSearchBox ID="tseCargo" runat="server" Key="codigo" Argument="descricao" MaxLength="20"
                                        SqlSelect="SELECT F.FUNCAOID AS codigo, (LF.DESCRICAO + ' - ' + F.DESCRICAO) AS descricao FROM FUNCAO F INNER JOIN LY_FUNCAO LF ON F.FUNCAOID = LF.FUNCAO AND ISNULL(LF.CAMPO_10, 'N') = 'S' "
                                        GridWidth="850px">
                                        <GridColumns>
                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
                                        </GridColumns>
                                    </tweb:TSearchBox>
                                    <asp:RequiredFieldValidator ID="rfvcmbCargo" runat="server" ControlToValidate="tseCargo"
                                        ErrorMessage="Função: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblAno" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlAno" runat="server" DataValueField="ano" DataTextField="ano"
                                        OnSelectedIndexChanged="ddlAno_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvAno" runat="server" ControlToValidate="ddlAno"
                                        ErrorMessage="Ano: Preenchimento obrigatório." InitialValue="" ValidationGroup="SalvarForm"><img 
                            src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblPeriodo" runat="server" Text="Período:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlPeriodo" runat="server" DataValueField="periodo" DataTextField="periodo">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvPeriodo" runat="server" ControlToValidate="ddlPeriodo"
                                        ErrorMessage="Período: Preenchimento obrigatório." InitialValue="" ValidationGroup="SalvarForm"><img 
                            src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblStatus" runat="server" Text="Status:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlStatus" runat="server" DataValueField="item" DataTextField="descr">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="ddlStatus"
                                        ErrorMessage="Status: Preenchimento obrigatório." InitialValue="" ValidationGroup="SalvarForm"><img 
                            src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="Label5" runat="server" Text="Tipo:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTipo" runat="server">
                                        <asp:ListItem Text="Selecione" Value=""></asp:ListItem>
                                        <asp:ListItem Text="Contrato" Value="Contrato"></asp:ListItem>
                                        <asp:ListItem Text="Migração" Value="Migracao"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnData" runat="server" GroupingText="Data" Width="250px">
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblDtInicio" runat="server" Text="Início:*" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td style="width: 150px">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxDateEdit ID="dtDtInicio" runat="server" CalendarProperties-ClearButtonText="Limpar"
                                                                    CalendarProperties-TodayButtonText="Hoje" Width="120px">
                                                                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                                    </CalendarProperties>
                                                                </dxe:ASPxDateEdit>
                                                            </td>
                                                            <td>
                                                                <asp:RequiredFieldValidator ID="rfvDtInicio" runat="server" ControlToValidate="dtDtInicio"
                                                                    InitialValue="" ErrorMessage="Data Início: Preenchimento obrigatório." ValidationGroup="SalvarForm">
                            <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblDtFim" runat="server" Text="Fim:*" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td style="width: 150px">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <dxe:ASPxDateEdit ID="dtDtFim" runat="server" MinDate="01/01/1900" CalendarProperties-ClearButtonText="Limpar"
                                                                    CalendarProperties-TodayButtonText="Hoje" Width="120px">
                                                                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                                    </CalendarProperties>
                                                                </dxe:ASPxDateEdit>
                                                            </td>
                                                            <td>
                                                                <asp:RequiredFieldValidator ID="rfvDtFim" runat="server" ControlToValidate="dtDtFim"
                                                                    InitialValue="" ErrorMessage="Data Fim: Preenchimento obrigatório." ValidationGroup="SalvarForm">
                                                <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td>
                                    <asp:Panel ID="pnInscricao" runat="server" GroupingText="Inscrição" Width="250px">
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblInscrIni" runat="server" Text="Início:*" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td style="width: 150px">
                                                    <dxe:ASPxDateEdit ID="dtInscrIni" runat="server" MinDate="01/01/1900" CalendarProperties-ClearButtonText="Limpar"
                                                        CalendarProperties-TodayButtonText="Hoje" Width="120px">
                                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                        </CalendarProperties>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblInscrFim" runat="server" Text="Fim:*" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td style="width: 150px">
                                                    <dxe:ASPxDateEdit ID="dtInscrFim" runat="server" MinDate="01/01/1900" CalendarProperties-ClearButtonText="Limpar"
                                                        CalendarProperties-TodayButtonText="Hoje" Width="120px">
                                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                        </CalendarProperties>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnLiberConsulta" runat="server" GroupingText="Liberação Consulta"
                                        Width="250px">
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblLiConsIni" runat="server" Text="Início:*" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td style="width: 150px">
                                                    <dxe:ASPxDateEdit ID="dtLiConsIni" runat="server" MinDate="01/01/1900" CalendarProperties-ClearButtonText="Limpar"
                                                        CalendarProperties-TodayButtonText="Hoje" Width="120px">
                                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                        </CalendarProperties>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblLiConsFim" runat="server" Text="Fim:*" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td style="width: 150px">
                                                    <dxe:ASPxDateEdit ID="dtLiConsFim" runat="server" MinDate="01/01/1900" CalendarProperties-ClearButtonText="Limpar"
                                                        CalendarProperties-TodayButtonText="Hoje" Width="120px">
                                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                        </CalendarProperties>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td>
                                    <asp:Panel ID="pnConvocacao" runat="server" GroupingText="Convocação" Width="250px">
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblConvocIni" runat="server" Text="Início:*" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td style="width: 150px">
                                                    <dxe:ASPxDateEdit ID="dtConvocIni" runat="server" MinDate="01/01/1900" CalendarProperties-ClearButtonText="Limpar"
                                                        CalendarProperties-TodayButtonText="Hoje" Width="120px">
                                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                        </CalendarProperties>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblConvocFim" runat="server" Text="Fim:*" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td style="width: 150px">
                                                    <dxe:ASPxDateEdit ID="dtConvocFim" runat="server" MinDate="01/01/1900" CalendarProperties-ClearButtonText="Limpar"
                                                        CalendarProperties-TodayButtonText="Hoje" Width="120px">
                                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                        </CalendarProperties>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnIngresso" runat="server" GroupingText="Ingresso" Width="250px">
                                        <table>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblIngrIni" runat="server" Text="Início:*" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td style="width: 150px">
                                                    <dxe:ASPxDateEdit ID="dtIngrIni" runat="server" MinDate="01/01/1900" CalendarProperties-ClearButtonText="Limpar"
                                                        CalendarProperties-TodayButtonText="Hoje" Width="120px">
                                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                        </CalendarProperties>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblIngrFim" runat="server" Text="Fim:*" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td style="width: 150px">
                                                    <dxe:ASPxDateEdit ID="dtIngrFim" runat="server" MinDate="01/01/1900" CalendarProperties-ClearButtonText="Limpar"
                                                        CalendarProperties-TodayButtonText="Hoje" Width="120px">
                                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                        </CalendarProperties>
                                                    </dxe:ASPxDateEdit>
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
                                    <asp:Label ID="lblDigitos" runat="server" Text="Dígitos para Número de Inscrição:*"
                                        SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDigitos" runat="server" MaxLength="1" Width="50px" SkinID="umAteCinco">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvDigitos" runat="server" ControlToValidate="txtDigitos"
                                        ErrorMessage="Digitos para número de inscrição: Preenchimento obrigatório." InitialValue=""
                                        ValidationGroup="SalvarForm">
										<img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revDigitos" ControlToValidate="txtDigitos" ValidationExpression="^[1-5]$"
                                        runat="server" ErrorMessage="Dígitos para número de inscrição permite apenas valores entre 1 e 5."
                                        ValidationGroup="SalvarForm">
										<img src="../Images/AlertaMens.gif" alt="Dígitos para número de inscrição permite apenas valores entre 1 e 5."/>
                                    </asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblDiasConvoc" runat="server" Text="Dias para a Apresentação:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDiasConvoc" runat="server" MaxLength="3" Width="50px" SkinID="numerico">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvDiasConvoc" runat="server" ControlToValidate="txtDiasConvoc"
                                        ErrorMessage="Dias para a convocação: Preenchimento obrigatório." InitialValue=""
                                        ValidationGroup="SalvarForm">
										<img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblNumResolu" runat="server" Text="Resolução SEEDUC:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNumResolu" runat="server" MaxLength="20" Width="150px" SkinID="numericoComPonto"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvNumResolu" runat="server" ControlToValidate="txtNumResolu"
                                        ErrorMessage="Resolução SEEDUC: Preenchimento obrigatório." InitialValue="" ValidationGroup="SalvarForm">
										<img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                                    </asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblDtPubliDO" runat="server" Text="Data de Publicação do DO:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <dxe:ASPxDateEdit ID="dtPubliDO" runat="server" MinDate="01/01/1900" CalendarProperties-ClearButtonText="Limpar"
                                                    CalendarProperties-TodayButtonText="Hoje">
                                                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                    </CalendarProperties>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                            <td>
                                                <asp:RequiredFieldValidator ID="rfvPubliDO" runat="server" ControlToValidate="dtPubliDO"
                                                    ErrorMessage="Data de publicação do DO: Preenchimento obrigatório." InitialValue=""
                                                    ValidationGroup="SalvarForm">
													<img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                                                </asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblObservacao" runat="server" Text="Observação:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtObservacao" runat="server" Height="160px" TextMode="MultiLine"
                                        Width="667px" MaxLength="2000"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Cargos">
                <ContentCollection>
                    <dxw:ContentControl ID="ccCargos" runat="server">
                        <dxwgv:ASPxGridView ID="grdCargos" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdCargos"
                            DataSourceID="odsCargos" KeyFieldName="CATEGORIA" Font-Names="Verdana" Font-Size="Small"
                            OnRowDeleting="grdCargos_RowDeleting" OnRowUpdating="grdCargos_RowUpdating" OnInitNewRow="grdCargos_InitNewRow"
                            OnRowInserting="grdCargos_RowInserting" OnAfterPerformCallback="grdCargos_AfterPerformCallback"
                            Width="700px">
                            <SettingsBehavior ConfirmDelete="True" />
                            <SettingsEditing Mode="EditForm" />
                            <Templates>
                                <EditForm>
                                    <dxw:ContentControl ID="ccCargos" runat="server">
                                        <div style="padding: 4px 4px 3px 4px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCargo" runat="server" Text="Cargo:*" SkinID="lblObrigatorio"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <tweb:TSearchBox runat="server" ID="tseCargos" Key="categoria" AutoPostBack="false"
                                                            SqlSelect="select CATEGORIAID as categoria, NOMECATEGORIA as nome, CH_SEMANAL_EFETIVA as cargaefetiva, CH_SEMANAL_TOTAL as cargatotal from ContratoTemporario.VW_CATEGORIAS_POR_CONCURSO_DOCENTE_CONTRATO_TEMPORARIO"
                                                            MaxLength="20" Value='<%# Bind("categoria") %>'>
                                                            <GridColumns>
                                                                <tweb:TSearchBoxColumn Caption="Cargo" FieldName="categoria" Width="20%" />
                                                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome" Width="40%" />
                                                                <tweb:TSearchBoxColumn Caption="Carga Efetiva" FieldName="cargaefetiva" Width="20%" />
                                                                <tweb:TSearchBoxColumn Caption="Carga Total" FieldName="cargatotal" Width="20%" />
                                                            </GridColumns>
                                                        </tweb:TSearchBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <dxwgv:ASPxGridViewTemplateReplacement ID="repUpdate" ReplacementType="EditFormUpdateButton"
                                                runat="server">
                                            </dxwgv:ASPxGridViewTemplateReplacement>
                                            <dxwgv:ASPxGridViewTemplateReplacement ID="repCancel" ReplacementType="EditFormCancelButton"
                                                runat="server">
                                            </dxwgv:ASPxGridViewTemplateReplacement>
                                        </div>
                                    </dxw:ContentControl>
                                </EditForm>
                            </Templates>
                            <SettingsText ConfirmDelete="Confirma a remoção ?" EmptyDataRow="Não existem dados." />
                            <Columns>
                                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                    <HeaderCaptionTemplate>
                                        <div style="text-align: center">
                                            <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                                onclick="grdCargos.AddNewRow();" alt="Novo" />
                                        </div>
                                    </HeaderCaptionTemplate>
                                    <DeleteButton Text="Remover" Visible="True">
                                        <Image Url="~/img/bt_exclui2.png" />
                                    </DeleteButton>
                                    <EditButton Text="Editar" Visible="false">
                                        <Image Url="~/img/bt_editar.png" />
                                    </EditButton>
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
                                <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                                    Visible="False" VisibleIndex="0">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Cargo" FieldName="CATEGORIA" UnboundType="String"
                                    VisibleIndex="1">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Descrição" FieldName="NOME" VisibleIndex="2">
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                        </dxwgv:ASPxGridView>
                        <div style="width: 100%; text-align: center">
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <dxe:ASPxButton ClientInstanceName="btnAnterior" AutoPostBack="false" UseSubmitBehavior="false"
                                            ID="ASPxButton5" runat="server" Text="<< Anterior">
                                            <ClientSideEvents Click="function(s, e) {
	                                                             pcConcursoDocentes.ChangeActiveTab(0, false);}" />
                                        </dxe:ASPxButton>
                                    </td>
                                    <td align="right">
                                        <dxe:ASPxButton ID="ASPxButton6" runat="server" AutoPostBack="false" UseSubmitBehavior="false"
                                            Text="Próximo >>">
                                            <ClientSideEvents Click="function(s, e) {
	                                                            pcConcursoDocentes.ChangeActiveTab(2, false);}" />
                                        </dxe:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Habilitações">
                <ContentCollection>
                    <dxw:ContentControl ID="ccHabilitacoes" runat="server">
                        <dxwgv:ASPxGridView ID="grdHabilitacoes" runat="server" AutoGenerateColumns="True"
                            DataSourceID="odsHabilitacoes" ClientInstanceName="grdHabilitacoes" KeyFieldName="CompositeKey"
                            Font-Names="Verdana" Font-Size="Small" Width="700px" OnRowDeleting="grdHabilitacoes_RowDeleting"
                            OnRowUpdating="grdHabilitacoes_RowUpdating" OnInitNewRow="grdHabilitacoes_InitNewRow"
                            OnRowInserting="grdHabilitacoes_RowInserting" OnStartRowEditing="grdHabilitacoes_StartRowEditing"
                            OnAfterPerformCallback="grdHabilitacoes_AfterPerformCallback" OnCustomButtonCallback="grdHabilitacoes_CustomButtonCallback"
                            EnableCallBacks="true">
                            <SettingsBehavior ConfirmDelete="True" />
                            <SettingsText ConfirmDelete="Confirma a remoção ?" EmptyDataRow="Não existem dados." />
                            <ClientSideEvents CustomButtonClick="function(s,e) { if(e.buttonID == 'btExcluir') { e.processOnServer = confirm('Tem certeza que deseja excluir a disciplina?'); return; }
                                else if(e.buttonID == 'btnReativar') { e.processOnServer = confirm('Tem certeza que deseja reativar a turma?'); return; }}" />
                            <Columns>
                                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                    <HeaderCaptionTemplate>
                                        <div style="text-align: center">
                                            <input type="image" id="btNovo" src="../img/bt_novo.png" style="cursor: pointer"
                                                onserverclick="HabilitaPopUpInsercao" runat="server" />
                                        </div>
                                    </HeaderCaptionTemplate>
                                    <CustomButtons>
                                        <dxwgv:GridViewCommandColumnCustomButton Text="Excluir" ID="btExcluir" Visibility="AllDataRows"
                                            Image-Url="~/img/bt_exclui2.png" Image-Height="15px" Image-AlternateText="Desativar">
                                        </dxwgv:GridViewCommandColumnCustomButton>
                                    </CustomButtons>
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                                    Visible="False" VisibleIndex="0">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Coordenadoria/Regional" FieldName="NUCLEO">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="NUCLEOID" FieldName="NUCLEO_ID" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Municipio" FieldName="MUNICIPIO">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="MUNICIPIOID" FieldName="MUNICIPIO_ID" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Disciplina" FieldName="DISCIPLINA">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Disciplina" FieldName="DISCIPLINA_ID" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                        </dxwgv:ASPxGridView>
                        <div style="width: 100%; text-align: center">
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <dxe:ASPxButton ClientInstanceName="btnAnterior" AutoPostBack="false" UseSubmitBehavior="false"
                                            ID="btnAnterior" runat="server" Text="<< Anterior">
                                            <ClientSideEvents Click="function(s, e) {
	                                pcConcursoDocentes.ChangeActiveTab(1, false);}" />
                                        </dxe:ASPxButton>
                                    </td>
                                    <td align="right">
                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" UseSubmitBehavior="false"
                                            Text="Próximo >>">
                                            <ClientSideEvents Click="function(s, e) {
	                                pcConcursoDocentes.ChangeActiveTab(3, false);}" />
                                        </dxe:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Experiências">
                <ContentCollection>
                    <dxw:ContentControl ID="ccExperiencias" runat="server">
                        <dxwgv:ASPxGridView ID="grdExperiencias" runat="server" AutoGenerateColumns="False"
                            ClientInstanceName="grdExperiencias" DataSourceID="tdsExperiencias" KeyFieldName="CompositeKey"
                            Font-Names="Verdana" Font-Size="Small" OnCustomUnboundColumnData="grdExperiencias_CustomUnboundColumnData"
                            OnRowDeleting="grdExperiencias_RowDeleting" OnRowUpdating="grdExperiencias_RowUpdating"
                            OnInitNewRow="grdExperiencias_InitNewRow" OnRowInserting="grdExperiencias_RowInserting"
                            OnStartRowEditing="grdExperiencias_StartRowEditing" OnAfterPerformCallback="grdExperiencias_AfterPerformCallback"
                            OnRowValidating="grdExperiencias_RowValidating" Width="700px">
                            <SettingsBehavior ConfirmDelete="True" />
                            <SettingsEditing Mode="EditForm" />
                            <Templates>
                                <EditForm>
                                    <dxw:ContentControl ID="ContentControl1" runat="server">
                                        <div style="padding: 4px 4px 3px 4px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblExperiencia" runat="server" Text="Experiência:*" SkinID="lblObrigatorio"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <tweb:TSearchBox runat="server" ID="tseExperiencia" Key="experiencia" AutoPostBack="false"
                                                            DataType="VarChar" Value='<%# Bind("experiencia") %>' SqlSelect="SELECT experiencia, descricao FROM ly_concurso_experiencia"
                                                            SqlOrder="descricao" MaxLength="20" FollowContainerMode="False">
                                                            <GridColumns>
                                                                <tweb:TSearchBoxColumn Caption="Experiência" FieldName="experiencia" Width="20%" />
                                                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
                                                            </GridColumns>
                                                        </tweb:TSearchBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblPontuacao" runat="server" Text="Pontuação:* " SkinID="lblObrigatorio"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <dxwgv:ASPxGridViewTemplateReplacement runat="server" ColumnID="pontuacao" ID="txtPontuacao1"
                                                            ReplacementType="EditFormCellEditor">
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
                            <SettingsText ConfirmDelete="Confirma a remoção ?" EmptyDataRow="Não existem dados." />
                            <Columns>
                                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                    <HeaderCaptionTemplate>
                                        <div style="text-align: center">
                                            <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                                onclick="grdExperiencias.AddNewRow();" alt="Novo" />
                                        </div>
                                    </HeaderCaptionTemplate>
                                    <DeleteButton Text="Remover" Visible="True">
                                        <Image Url="~/img/bt_exclui2.png" />
                                    </DeleteButton>
                                    <CancelButton Text="Cancelar">
                                        <Image Url="~/img/bt_cancelar.png" />
                                    </CancelButton>
                                    <EditButton Text="Editar" Visible="true">
                                        <Image Url="~/img/bt_editar.png" />
                                    </EditButton>
                                    <UpdateButton>
                                        <Image Url="~/img/bt_salvar.png" />
                                    </UpdateButton>
                                    <ClearFilterButton Text="Limpar" Visible="True">
                                        <Image Url="~/img/bt_limpa.png" />
                                    </ClearFilterButton>
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                                    Visible="False" VisibleIndex="0">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Processo Seletivo" FieldName="concurso" VisibleIndex="1"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Experiência" FieldName="experiencia" VisibleIndex="2"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Experiência" FieldName="descricao" VisibleIndex="2">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Pontuação" FieldName="pontuacao" VisibleIndex="3"
                                    CellStyle-HorizontalAlign="Center">
                                    <PropertiesTextEdit MaxLength="5" Width="100px">
                                       
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor informar a Pontuação." IsRequired="True" />
                                           
                                        </ValidationSettings>
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                        </dxwgv:ASPxGridView>
                        <div style="width: 100%; text-align: center">
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <dxe:ASPxButton ClientInstanceName="btnAnterior" AutoPostBack="false" UseSubmitBehavior="false"
                                            ID="ASPxButton2" runat="server" Text="<< Anterior">
                                            <ClientSideEvents Click="function(s, e) {
	                                pcConcursoDocentes.ChangeActiveTab(2, false);}" />
                                        </dxe:ASPxButton>
                                    </td>
                                    <td align="right">
                                        <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" UseSubmitBehavior="false"
                                            Text="Próximo >>">
                                            <ClientSideEvents Click="function(s, e) {
	                                pcConcursoDocentes.ChangeActiveTab(4, false);}" />
                                        </dxe:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Titulações">
                <ContentCollection>
                    <dxw:ContentControl ID="ccTitulacoes" runat="server">
                        <dxwgv:ASPxGridView ID="grdTitulacoes" runat="server" AutoGenerateColumns="False"
                            ClientInstanceName="grdTitulacoes" DataSourceID="tdsTitulacoes" KeyFieldName="CompositeKey"
                            Font-Names="Verdana" Font-Size="Small" OnCustomUnboundColumnData="grdTitulacoes_CustomUnboundColumnData"
                            OnRowDeleting="grdTitulacoes_RowDeleting" OnRowUpdating="grdTitulacoes_RowUpdating"
                            OnInitNewRow="grdTitulacoes_InitNewRow" OnRowInserting="grdTitulacoes_RowInserting"
                            OnStartRowEditing="grdTitulacoes_StartRowEditing" OnAfterPerformCallback="grdTitulacoes_AfterPerformCallback"
                            OnRowValidating="grdTitulacoes_RowValidating" Width="700px">
                            <SettingsBehavior ConfirmDelete="True" />
                            <SettingsEditing Mode="EditForm" />
                            <Templates>
                                <EditForm>
                                    <dxw:ContentControl ID="ContentControl1" runat="server">
                                        <div style="padding: 4px 4px 3px 4px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblTitulacao" runat="server" Text="Titulação:*" SkinID="lblObrigatorio"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <tweb:TSearchBox runat="server" ID="tseTitulacao" Key="titulacao" AutoPostBack="false"
                                                            Value='<%# Bind("titulacao") %>' SqlSelect="SELECT titulacao, descricao FROM ly_concurso_titulacao"
                                                            SqlOrder="descricao" MaxLength="20" FollowContainerMode="False">
                                                            <GridColumns>
                                                                <tweb:TSearchBoxColumn Caption="Titulação" FieldName="titulacao" Width="20%" />
                                                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
                                                            </GridColumns>
                                                        </tweb:TSearchBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblPontuacao" runat="server" Text="Pontuação:* " SkinID="lblObrigatorio"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <dxwgv:ASPxGridViewTemplateReplacement runat="server" ColumnID="pontuacao" ID="txtPontuacao2"
                                                            ReplacementType="EditFormCellEditor">
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
                            <SettingsText ConfirmDelete="Confirma a remoção ?" EmptyDataRow="Não existem dados." />
                            <Columns>
                                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                    <HeaderCaptionTemplate>
                                        <div style="text-align: center">
                                            <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                                onclick="grdTitulacoes.AddNewRow();" alt="Novo" />
                                        </div>
                                    </HeaderCaptionTemplate>
                                    <DeleteButton Text="Remover" Visible="True">
                                        <Image Url="~/img/bt_exclui2.png" />
                                    </DeleteButton>
                                    <EditButton Text="Editar" Visible="true">
                                        <Image Url="~/img/bt_editar.png" />
                                    </EditButton>
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
                                <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                                    Visible="False" VisibleIndex="0">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Processo Seletivo" FieldName="concurso" VisibleIndex="1"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Titulação" FieldName="titulacao" VisibleIndex="2"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Titulação" FieldName="descricao" VisibleIndex="2">
                                    <PropertiesTextEdit MaxLength="9" Width="150px">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor informar a Titulação." IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Pontuação" FieldName="pontuacao" VisibleIndex="3"
                                    CellStyle-HorizontalAlign="Center">
                                    <PropertiesTextEdit MaxLength="5" Width="100px">                                        
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor informar a Pontuação." IsRequired="True" />
                                           
                                        </ValidationSettings>
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                        </dxwgv:ASPxGridView>
                        <div style="width: 100%;">
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <dxe:ASPxButton ClientInstanceName="btnAnterior" AutoPostBack="false" UseSubmitBehavior="false"
                                            ID="ASPxButton4" runat="server" Text="<< Anterior">
                                            <ClientSideEvents Click="function(s, e) {
	                                pcConcursoDocentes.ChangeActiveTab(3, false);}" />
                                        </dxe:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Documentos Necessários">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl2" runat="server">
                        <asp:ObjectDataSource ID="odsTipoDocumentoConcurso" runat="server" TypeName="Techne.Lyceum.Net.ProcessoSeletivo.ConcursoDocentes"
                            SelectMethod="ListaTipoDocumentoConcurso" InsertMethod="InsertTipoDocumentoConcurso" UpdateMethod="UpdateTipoDocumentoConcurso" DeleteMethod="DeleteTipoDocumentoConcurso">
                            <SelectParameters>
                                <asp:SessionParameter Name="concurso" SessionField="vsConcurso" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:ObjectDataSource ID="odsTipoDocumento" runat="server" TypeName="Techne.Lyceum.Net.ProcessoSeletivo.ConcursoDocentes"
                            SelectMethod="ListaTipoDocumento"></asp:ObjectDataSource>
                        <dxwgv:ASPxGridView ID="grdTipoDocumento" runat="server" DataSourceID="odsTipoDocumentoConcurso"
                            KeyFieldName="TIPODOCUMENTOCONCURSOID" AutoGenerateColumns="false" ClientInstanceName="grdTipoDocumento"
                            OnInitNewRow="grdTipoDocumento_InitNewRow" OnStartRowEditing="grdTipoDocumento_StartRowEditing"
                            OnRowInserting="grdTipoDocumento_RowInserting" OnRowUpdating="grdTipoDocumento_RowUpdating"
                            OnCellEditorInitialize="grdTipoDocumento_CellEditorInitialize" OnRowDeleting="grdTipoDocumento_RowDeleting"
                            OnAfterPerformCallback="grdTipoDocumento_AfterPerformCallback" Width="50%">
                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsEditing Mode="Inline" />
                            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                            <SettingsBehavior ConfirmDelete="true" />
                            <Columns>
                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                                    <HeaderCaptionTemplate>
                                        <div style="text-align: center">
                                            <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                                                onclick="grdTipoDocumento.AddNewRow();" />
                                        </div>
                                    </HeaderCaptionTemplate>
                                    <CancelButton Visible="true" Text="Cancelar">
                                        <Image Url="~/img/bt_cancelar.png" />
                                    </CancelButton>
                                    <EditButton Visible="True" Text="Editar">
                                        <Image Url="../img/bt_editar.png" />
                                    </EditButton>
                                    <DeleteButton Visible="True" Text="Remover">
                                        <Image Url="../img/bt_exclui2.png" />
                                    </DeleteButton>
                                    <ClearFilterButton Text="Limpar" Visible="True">
                                        <Image Url="~/img/bt_limpa.png" />
                                    </ClearFilterButton>
                                    <UpdateButton Visible="true" Text="Alterar">
                                        <Image Url="../img/bt_salvar.png" />
                                    </UpdateButton>
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="TIPODOCUMENTOCONCURSOID"
                                    Visible="false" Width="700px">
                                    <PropertiesTextEdit MaxLength="200">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataComboBoxColumn Caption="Tipo Documento*" FieldName="TIPODOCUMENTOID"
                                    Width="200px" VisibleIndex="3">
                                    <PropertiesComboBox ValueType="System.String" DataSourceID="odsTipoDocumento" ValueField="TIPODOCUMENTOID"
                                        Width="200px" TextField="DESCRICAO">
                                    </PropertiesComboBox>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataCheckColumn Caption="Anexo?*" FieldName="ANEXO" VisibleIndex="6"
                                    Width="120px">
                                    <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                                        ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                                    </PropertiesCheckEdit>
                                </dxwgv:GridViewDataCheckColumn>
                              
                            </Columns>
                        </dxwgv:ASPxGridView>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
</asp:Content>
