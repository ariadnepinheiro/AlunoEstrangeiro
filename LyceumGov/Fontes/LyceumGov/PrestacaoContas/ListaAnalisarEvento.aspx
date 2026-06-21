<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListaAnalisarEvento.aspx.cs"
    MasterPageFile="~/Modulos/LyceumMaster.Master" Inherits="Techne.Lyceum.Net.PrestacaoContas.ListaAnalisarEvento" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<%@ Import Namespace="Techne.Lyceum.RN.Util" %>
<%@ Import Namespace="System.Linq" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .DateEditWithoutBorder
        {
            width: 100%;
            padding-left: 1px;
            padding-right: 1px;
            padding-top: 1px;
            padding-bottom: 1px;
        }
        .TSearchButton
        {
            border-width: 0px !important;
            vertical-align: top !important;
            position: relative;
            top: -1px;
        }
        .table-fixed
        {
            table-layout: fixed;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Informações da Despesa" Width="800px">
        <table class="table-fixed" width="800">
            <tr>
                <td width="200" align="right">
                    <span>Período da Prestação de Contas: <span style="color: red">*</span></span>
                </td>
                <td width="600">
                    <tweb:TSearchBox ID="tsePeriodoPrestacaoContas" runat="server" Key="periodoreferenciaid"
                        Argument="descricao" MaxLength="20" ArgumentColumns="50" Columns="10" AutoPostBack="false"
                        SqlSelect="select periodoreferenciaid, mesinicial, mesfinal, referencia, datalimiteprestacaocontas, datalimiteanalise, descricao from prestacaocontas.vw_periodoreferencia"
                        GridWidth="850px" SqlOrder="periodoreferenciaid" DataType="Number">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="ID" FieldName="periodoreferenciaid" Width="5%" />
                            <tweb:TSearchBoxColumn Caption="Mês Inicial" FieldName="mesinicial" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Mês Final" FieldName="mesfinal" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Referência" FieldName="referencia" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Dt Lim PContas" FieldName="datalimiteprestacaocontas"
                                Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Dt Lim Análise" FieldName="datalimiteanalise" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="15%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
        <table class="table-fixed" width="800">
            <tr>
                <td width="200" align="right">
                    <span>Unidade de Ensino:</span>
                </td>
                <td width="600">
                    <tweb:TSearchBox ID="tseUnidadeEnsino" runat="server" Key="unidade_ens" Argument="nome_comp"
                        MaxLength="20" ArgumentColumns="50" Columns="10" AutoPostBack="false" SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao,municipio,nome, regional,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO_REGIONAL"
                        GridWidth="850px" SqlOrder="nome_comp">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome_comp" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="8%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="8%" />
                            <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="13%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="11%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
        <table class="table-fixed" width="800">
            <tr>
                <td width="200" align="right">
                    <span>Projeto / Programa:</span>
                </td>
                <td width="600">
                    <tweb:TSearchBox ID="tsePlanoTrabalho" runat="server" Argument="descricao" Key="PLANOTRABALHOID"
                        MaxLength="20" ArgumentColumns="50" Columns="10" DataType="VarChar" AutoPostBack="false"
                        SqlSelect=" select descricao from [PrestacaoContas].[VW_PLANOTRABALHO] ">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="PLANOTRABALHOID" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="DESCRICAO" Width="90%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
        <table class="table-fixed" width="800">
            <tr>
                <td width="200" align="right">
                    <span>Tipo de Despesa:</span>
                </td>
                <td width="600">
                    <asp:RadioButtonList ID="rblTipoEvento" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="0" Text="Com NF-e"></asp:ListItem>
                        <asp:ListItem Value="1" Text="Com Demais Documentos Fiscais"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Pequena Despesa"></asp:ListItem>
                        <asp:ListItem Value="3" Text="Com Locomoção de Servidores"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
        <table class="table-fixed" width="800">
            <tr>
                <td width="200" align="right">
                    <span>Despesa:</span>
                </td>
                <td width="600">
                    <tweb:TSearchBox ID="tseEvento" runat="server" Caption="" Key="EVENTOID" Argument="numeroevento"
                        MaxLength="20" ArgumentColumns="50" Columns="10" GridWidth="850px" DataType="VarChar"
                        AutoPostBack="false" SqlSelect=" SELECT ESCOLA, NUMEROEVENTO, FINALIDADE, DATAPAGAMENTO, DATAPAGAMENTOFORMATADA, PLANOTRABALHOID, NUMERONOTAFISCAL FROM PRESTACAOCONTAS.VW_EVENTO ">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="EVENTOID" Width="5%" />
                            <tweb:TSearchBoxColumn Caption="Número" FieldName="NUMEROEVENTO" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Nota Fiscal" FieldName="NUMERONOTAFISCAL" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Escola" FieldName="ESCOLA" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Finalidade" FieldName="FINALIDADE" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Dt.Pagamento" FieldName="DATAPAGAMENTOFORMATADA"
                                Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Código Plano" FieldName="PLANOTRABALHOID" Width="5%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
        <table class="table-fixed" width="800">
            <tr>
                <td width="200" align="right">
                    <span>Situação da Despesa: <span style="color: red">*</span></span>
                </td>
                <td width="500">
                    <asp:RadioButtonList ID="rblSituacaoEvento" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="Todos" Text="Todos"></asp:ListItem>
                        <asp:ListItem Value="Enviado para Análise" Text="Enviado para Análise"></asp:ListItem>
                        <asp:ListItem Value="Com Exigências" Text="Com Exigências"></asp:ListItem>
                        <asp:ListItem Value="Validado" Text="Validado"></asp:ListItem>
                        <asp:ListItem Value="Reprovado" Text="Reprovado"></asp:ListItem>
                        <asp:ListItem Value="Cumprida" Text="Exigência Cumprida"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
        <table class="table-fixed" width="800">
            <tr>
                <td width="100" align="right">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" ForeColor="Red" EnableViewState="false"></asp:Label>
    <br />
    <asp:PlaceHolder ID="plaVisibilidadeGrid" runat="server" Visible="false">
        <div class="divEditBlock" style="width: 950px;">
            <asp:Label runat="server" ID="lblBlocoAnalisarEvento" Text="Eventos" SkinID="BcTitulo" />
            <asp:ValidationSummary ID="vsAnalisarEvento" runat="server" EnableClientScript="true"
                ShowMessageBox="true" ValidationGroup="SalvarForm" ShowSummary="false" />
        </div>
        <br />
        <asp:ObjectDataSource ID="odsEvento" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.ListaAnalisarEvento"
            SelectMethod="ListaEvento">
            <SelectParameters>
                <asp:Parameter Name="dataInicio" />
                <asp:Parameter Name="dataFim" />
                <asp:Parameter Name="censo" />
                <asp:Parameter Name="planoTrabalhoId" />
                <asp:Parameter Name="tipoDespesa" />
                <asp:Parameter Name="eventoId" ConvertEmptyStringToNull="true" Type="Int32" />
                <asp:Parameter Name="situacao" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <dxwgv:ASPxGridView runat="server" ID="grdEvento" ClientInstanceName="grdEvento"
            AutoGenerateColumns="False" EnableCallBacks="false" Width="1040" DataSourceID="odsEvento"
            KeyFieldName="EVENTOID">
            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsEditing Mode="Inline" />
            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
            <SettingsBehavior AllowMultiSelection="False" AllowSort="true" ConfirmDelete="true" />
            <Columns>
                <dxwgv:GridViewDataColumn Caption="" VisibleIndex="0" Width="40px">
                    <DataItemTemplate>
                        <asp:ImageButton ID="btnVisualizar" runat="server" EnableViewState="false" CommandArgument='<%# Eval("PLANOTRABALHOID") + "," + Eval("CENSO") + "," + Eval("TIPOEVENTO") + "," + Eval("EVENTOID") %>'
                            OnCommand="btnVisualizar_Command" ImageUrl="~/img/bt_busca.png" Height="15px"
                            AlternateText="Visualizar Evento"></asp:ImageButton>
                    </DataItemTemplate>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Censo" FieldName="CENSO" VisibleIndex="1" Width="80px">
                    <CellStyle HorizontalAlign="Center" VerticalAlign="NotSet">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Escola" FieldName="ESCOLA" VisibleIndex="1" Width="80px">
                    <CellStyle HorizontalAlign="Center" VerticalAlign="NotSet">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Num. Evento" FieldName="NUMEROEVENTO" VisibleIndex="2"
                    Width="120px">
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Finalidade" FieldName="FINALIDADE" VisibleIndex="3"
                    Width="140px">
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Tipo Despesa" FieldName="TIPODESPESA" VisibleIndex="4"
                    Width="140px">
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Data Evento" FieldName="DATAEVENTO" VisibleIndex="5"
                    Width="80px">
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Data da NF" FieldName="DATANOTAFISCAL" VisibleIndex="6"
                    Width="80px">
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Data Pgto." FieldName="DATAPAGAMENTO" VisibleIndex="7"
                    Width="80px">
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Valor Pgto." FieldName="VALORPAGAMENTO" VisibleIndex="8"
                    Width="100px">
                    <CellStyle HorizontalAlign="Right" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Situação" FieldName="SITUACAO" VisibleIndex="9"
                    Width="140px">
                    <CellStyle HorizontalAlign="Left" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Total Exigên cias" FieldName="TOTALEXIGENCIAS"
                    VisibleIndex="10" Width="40px">
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
            </Columns>
        </dxwgv:ASPxGridView>
    </asp:PlaceHolder>

    <script language="javascript">
        var dtePeriodoPagamentoInicio, dtePeriodoPagamentoFim, dxeCalendar, tseUnidadeEnsino, tseUnidadeEnsinoGrid, tsePlanoTrabalho, tsePlanoTrabalhoGrid;
        $(document).ready(function() {
            
            tsePeriodoPrestacaoContas = $("#<%= tsePeriodoPrestacaoContas.ClientID %>");
            tsePeriodoPrestacaoContasGrid = $("#<%= tsePeriodoPrestacaoContas.ClientID %>_grid");
            tseUnidadeEnsino = $("#<%= tseUnidadeEnsino.ClientID %>");
            tseUnidadeEnsinoGrid = $("#<%= tseUnidadeEnsino.ClientID %>_grid");
            tsePlanoTrabalho = $("#<%= tsePlanoTrabalho.ClientID %>");
            tsePlanoTrabalhoGrid = $("#<%= tsePlanoTrabalho.ClientID %>_grid");

            tsePeriodoPrestacaoContas.on("change", filtrarEvento);
            tsePeriodoPrestacaoContasGrid.on("click", filtrarEvento);
            tseUnidadeEnsino.on("change", filtrarEvento);
            tseUnidadeEnsinoGrid.on("click", filtrarEvento);
            tsePlanoTrabalho.on("change", filtrarEvento);
            tsePlanoTrabalhoGrid.on("click", filtrarEvento);
        
        });

        function filtrarEvento(lock) {
            try {
                if (lock != "lock") {
                    window.setTimeout(() => filtrarEvento("lock"), 100);
                    return;
                }
                
                console.log("verificando filtragem...");
                
                if (tsePeriodoPrestacaoContas.val() == undefined || tsePeriodoPrestacaoContas.val() == null || tsePeriodoPrestacaoContas.val() == "")
                    return;

                if (tseUnidadeEnsino.val() == undefined || tseUnidadeEnsino.val() == null || tseUnidadeEnsino.val() == "")
                    return;
                    
                if (tsePlanoTrabalho.val() == undefined || tsePlanoTrabalho.val() == null || tsePlanoTrabalho.val() == "")
                    return;
                
                console.log("filtragem ok, filtrando...");
                    
                __doPostBack("FiltraEvento", null);
            }
            catch (ex) {
            }
        }
    </script>

</asp:Content>
