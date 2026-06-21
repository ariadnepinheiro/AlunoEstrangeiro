<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AprovarRepasse.aspx.cs"
    MasterPageFile="~/Modulos/LyceumMaster.Master" Inherits="Techne.Lyceum.Net.PrestacaoContas.AprovarRepasse" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel runat="server" ID="Panel1" GroupingText="Dados de Programação Orçamentária"
        Width="800px">
        <table width="800">
             <tr>
                <td align="right">
                     <asp:Label ID="Label1" runat="server" SkinID="lblObrigatorio" Font-Names="Verdana"
                        Text="Período da Prestação de Contas:*"></asp:Label>            
                </td>
                <td>
                    <tweb:TSearchBox ID="tsePeriodoPrestacaoContas" runat="server" 
                        Key="periodoreferenciaid" Argument="descricao"
                        MaxLength="20" ArgumentColumns="50" Columns="10" AutoPostBack="true"
                        SqlSelect="select periodoreferenciaid, mesinicial, mesfinal, referencia, datalimiteprestacaocontas, datalimiteanalise, descricao from prestacaocontas.vw_periodoreferencia"
                        GridWidth="850px" SqlOrder="periodoreferenciaid" DataType="Number" 
                        OnChanged="tsePeriodoPrestacaoContas_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="ID" FieldName="periodoreferenciaid" Width="5%" />
                            <tweb:TSearchBoxColumn Caption="Mês Inicial" FieldName="mesinicial" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Mês Final" FieldName="mesfinal" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Referência" FieldName="referencia" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Dt Lim PContas" FieldName="datalimiteprestacaocontas" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Dt Lim Análise" FieldName="datalimiteanalise" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="15%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label3" runat="server" SkinID="lblObrigatorio" Font-Names="Verdana"
                        Text="Programação Orçamentária:*"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseProgramacaoOrcamentaria" runat="server" Caption="" Key="planilhaorcamentariaid"
                        MaxLength="20" ArgumentColumns="50" Columns="10" Argument="NOME" GridWidth="850px"
                        DataType="Number"  AutoPostBack="True" SqlOrder="planilhaorcamentariaid" SqlSelect="
                        select po.planilhaorcamentariaid, po.regiaofinanceiraid, rf.descricao as descricao_regiaofinanceira, po.ano, po.processo, po.descricao from PrestacaoContas.VW_PLANILHAORCAMENTARIA po
                        inner join GestaoRede.REGIAOFINANCEIRA rf on rf.REGIAOFINANCEIRAID = po.REGIAOFINANCEIRAID"
                        SqlWhere=" po.ano = 1800 " onchanged="tseProgramacaoOrcamentaria_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="planilhaorcamentariaid" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Ano" FieldName="ano" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Região Financeira" FieldName="descricao_regiaofinanceira"
                                Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Processo" FieldName="processo" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="40%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="Label10" runat="server" SkinID="lblObrigatorio" Font-Names="Verdana"
                        Text="Parcela da Programação Orçamentária:*"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseParcelaProgramacaoOrcamentaria" runat="server" Caption=""
                        Key="itemplanilhaorcamentariaid" MaxLength="20" ArgumentColumns="50" Columns="10"
                        Argument="ANO_MES" GridWidth="850px" DataType="Number" AutoPostBack="true"
                        SqlOrder="itemplanilhaorcamentariaid" SqlSelect="
                        SELECT ITEMPLANILHAORCAMENTARIAID, MES, VALOR, RETORNOREFERENCIA,PLANILHA from PrestacaoContas.VW_ITEMPLANILHAORCAMENTARIAPERIODO
                        " SqlWhere=" planilhaorcamentariaid = 0 " OnChanged="tseParcelaProgramacaoOrcamentaria_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="itemplanilhaorcamentariaid" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="PLANILHA" Width="50%" />
                            <tweb:TSearchBoxColumn Caption="Referência" FieldName="MES" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Valor" FieldName="valor" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Retorno Referência" FieldName="retornoreferencia"
                                Width="70%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" EnableViewState="false" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:PlaceHolder ID="plaAprovarRepasse" runat="server" Visible="false" OnPreRender="plaAprovarRepasse_PreRender">
        <div class="divEditBlock" style="width: 800px;">
            <asp:Label runat="server" ID="lblTitulo" Text="Lançamentos de Repasses das Parcelas da Programação Orçamentária"
                SkinID="BcTitulo" />
        </div>
        <br />
        <br />
        <asp:Panel runat="server" ID="Panel2" GroupingText="Dados da Parcela da Programação Orçamentária"
            Width="800px">
            <table width="800">
                <tr>
                    <td width="150" align="right">
                        <span>Mês/Ano Referência:</span>
                    </td>
                    <td width="250">
                        <asp:Label ID="lblMesAnoReferencia" runat="server"></asp:Label>
                    </td>
                    <td width="150" align="right">
                        <span>Região Financeira:</span>
                    </td>
                    <td width="250">
                        <asp:Label ID="lblRegiaoFinanceira" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <span>Fonte de Recursos:</span>
                    </td>
                    <td>
                        <asp:Label ID="lblFonteRecursos" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <br />
        <table width="800">
            <tr>
                <td width="200" align="left">
                    <span>
                        <asp:RadioButton ID="radAprovarTodos" runat="server" Text="Aprovar Todos" GroupName="radAprovacao" /></span>
                </td>
                <td width="600" align="left">
                    <span>
                        <asp:RadioButton ID="radReprovarTodos" runat="server" Text="Reprovar Todos" GroupName="radAprovacao" /></span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <span id="spanMotivoReprovacao" style="display: none;">Motivo da Reprovação: <span
                        style="color: red">*</span>
                        <asp:DropDownList ID="cmbMotivoReprovacao" runat="server" DataSourceID="odsMotivoReprovacaoLancamentoRepasse"
                            DataValueField="MOTIVOREPROVACAOLANCAMENTOREPASSEID" DataTextField="DESCRICAO" />
                    </span>
                </td>
            </tr>
            <tr>
                <td colspan="3" width="800" height="30" align="left" valign="bottom">
                    <asp:Button ID="btnSalvarAprovacao" runat="server" Width="100px" Text="Salvar" OnClick="btnSalvarAprovacao_Click" />
                </td>
            </tr>
        </table>
        <br />
        <br />
        <asp:ObjectDataSource ID="odsMotivoReprovacaoLancamentoRepasse" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.AprovarRepasse"
            SelectMethod="ListaMotivoReprovacaoLancamentoRepasse"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="odsLancamentoRepasse" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.AprovarRepasse"
            SelectMethod="ListaLancamentoRepasse" UpdateMethod="UpdateLancamentoRepasse">
            <SelectParameters>
                <asp:ControlParameter Name="itemPlanilhaOrcamentariaId" ControlID="tseParcelaProgramacaoOrcamentaria"
                    PropertyName="Value" Type="Decimal" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <dxwgv:ASPxGridView runat="server" ID="grdLancamentoRepasse" ClientInstanceName="grdLancamentoRepasse"
            AutoGenerateColumns="False" EnableCallBacks="false" Width="1330" SettingsPager-Mode="ShowAllRecords"
            DataSourceID="odsLancamentoRepasse" KeyFieldName="LANCAMENTOREPASSEID" OnCommandButtonInitialize="grdLancamentoRepasse_CommandButtonInitialize"
            OnRowUpdating="grdLancamentoRepasse_RowUpdating">
            <Settings ShowFilterRow="false" ShowFilterRowMenu="false" />
            <SettingsEditing Mode="Inline" />
            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
            <SettingsBehavior AllowMultiSelection="False" AllowSort="False" ConfirmDelete="true" />
            <Columns>
                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Caption="" Width="50px">
                    <HeaderCaptionTemplate>
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
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataColumn FieldName="LANCAMENTOREPASSEID" VisibleIndex="0" Visible="false">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="ANALISEREPASSEID" VisibleIndex="0" Visible="false">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="WSREPASSESEFAZID" VisibleIndex="0" Visible="false">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Censo" FieldName="CENSO" VisibleIndex="1"
                    Width="70px">
                    <EditItemTemplate>
                        <%# Eval("CENSO") %></EditItemTemplate>
                    <EditCellStyle CssClass="tab-cell">
                    </EditCellStyle>
                    <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Unidade de Ensino" FieldName="ESCOLA" VisibleIndex="2"
                    Width="250px">
                    <EditItemTemplate>
                        <%# Eval("ESCOLA")%></EditItemTemplate>
                    <EditCellStyle CssClass="tab-cell">
                    </EditCellStyle>
                    <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                 <dxwgv:GridViewDataColumn Caption="CNPJ" FieldName="CNPJ" VisibleIndex="3"
                    Width="120px">
                    <EditItemTemplate>
                        <%# Eval("CNPJ")%></EditItemTemplate>
                    <EditCellStyle CssClass="tab-cell">
                    </EditCellStyle>
                    <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Banco" FieldName="BANCO" VisibleIndex="4" Width="50px">
                    <EditItemTemplate>
                        <%# Eval("BANCO") %></EditItemTemplate>
                    <EditCellStyle CssClass="tab-cell">
                    </EditCellStyle>
                    <CellStyle HorizontalAlign="Left" VerticalAlign="NotSet">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Agência" FieldName="AGENCIA" VisibleIndex="5"
                    Width="50px">
                    <EditItemTemplate>
                        <%# Eval("AGENCIA") %></EditItemTemplate>
                    <EditCellStyle CssClass="tab-cell">
                    </EditCellStyle>
                    <CellStyle HorizontalAlign="Left" VerticalAlign="NotSet">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Conta Corrente" FieldName="CONTA" VisibleIndex="6"
                    Width="100px">
                    <EditItemTemplate>
                        <%# Eval("CONTA") %></EditItemTemplate>
                    <EditCellStyle CssClass="tab-cell">
                    </EditCellStyle>
                    <CellStyle HorizontalAlign="Left" VerticalAlign="NotSet">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Valor" FieldName="VALOR" VisibleIndex="7" Width="100px">
                    <EditItemTemplate>
                        <%# Eval("VALOR") %></EditItemTemplate>
                    <EditCellStyle CssClass="tab-cell">
                    </EditCellStyle>
                    <CellStyle HorizontalAlign="Right" VerticalAlign="NotSet">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="NE" FieldName="NUMERONE" VisibleIndex="8" Width="50px">
                    <EditItemTemplate>
                        <%# Eval("NUMERONE")%></EditItemTemplate>
                    <EditCellStyle CssClass="tab-cell">
                    </EditCellStyle>
                    <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="NL" FieldName="NUMERONL" VisibleIndex="9" Width="50px">
                    <EditItemTemplate>
                        <%# Eval("NUMERONL")%></EditItemTemplate>
                    <EditCellStyle CssClass="tab-cell">
                    </EditCellStyle>
                    <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="PD" FieldName="NUMEROPD" VisibleIndex="10" Width="50px">
                    <EditItemTemplate>
                        <%# Eval("NUMEROPD")%></EditItemTemplate>
                    <EditCellStyle CssClass="tab-cell">
                    </EditCellStyle>
                    <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Número OB" FieldName="NUMEROOB" VisibleIndex="11"
                    Width="50px">
                    <EditItemTemplate>
                        <%# Eval("NUMEROOB") %></EditItemTemplate>
                    <EditCellStyle CssClass="tab-cell">
                    </EditCellStyle>
                    <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="OB Lista" FieldName="NUMEROLISTAOB" VisibleIndex="12"
                    Width="50px">
                    <EditItemTemplate>
                        <%# Eval("NUMEROLISTAOB") %></EditItemTemplate>
                    <EditCellStyle CssClass="tab-cell">
                    </EditCellStyle>
                    <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Status" FieldName="STATUSLISTA" VisibleIndex="13"
                    Width="100px">
                    <EditItemTemplate>
                        <%# Eval("STATUSLISTA") %></EditItemTemplate>
                    <EditCellStyle CssClass="tab-cell">
                    </EditCellStyle>
                    <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Num. Processo Repasse" FieldName="NUMEROPROCESSOREPASSE"
                    VisibleIndex="14" Width="50px">
                    <EditItemTemplate>
                        <%# Eval("NUMEROPROCESSOREPASSE") %></EditItemTemplate>
                    <EditCellStyle CssClass="tab-cell">
                    </EditCellStyle>
                    <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Impedida" FieldName="IMPEDIDA" VisibleIndex="15"
                    Width="50px">
                    <DataItemTemplate>
                        <%# Convert.ToBoolean(Eval("IMPEDIDA")) ? "Sim" : "Não" %></DataItemTemplate>
                    <EditItemTemplate>
                        <%# Convert.ToBoolean(Eval("IMPEDIDA")) ? "Sim" : "Não" %></EditItemTemplate>
                    <EditCellStyle CssClass="tab-cell" HorizontalAlign="Center" VerticalAlign="NotSet">
                    </EditCellStyle>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="NotSet">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Ação" FieldName="ACAO" VisibleIndex="16" Width="100px">
                    <DataItemTemplate>
                        <%# Eval("ACAO") == DBNull.Value ? "" : (Convert.ToBoolean(Eval("ACAO")) ? "Aprovado" : "Reprovado") %></DataItemTemplate>
                    <EditItemTemplate>
                        <dxe:ASPxRadioButtonList ID="rblAcao" runat="server" EnableViewState="false" RepeatDirection="Vertical"
                            Value='<%# Convert.ToBoolean(Eval("ACAO") != DBNull.Value ? Eval("ACAO") : "false") %>'
                            ValueType="System.Boolean" AutoPostBack="False" ValidationSettings-ValidationGroup="<%# Container.ValidationGroup %>"
                            Border-BorderWidth="0" ClientInstanceName="rblAcao" ClientSideEvents-SelectedIndexChanged="function (s, e) { MotivoReprovacaoLancamentoRepasseId.SetVisible(!s.GetValue());  }">
                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                <RequiredField ErrorText="Favor informar a ação" IsRequired="True" />
                            </ValidationSettings>
                            <Items>
                                <dxe:ListEditItem Text="Aprovado" Value="true" />
                                <dxe:ListEditItem Text="Reprovado" Value="false" />
                            </Items>
                        </dxe:ASPxRadioButtonList>
                    </EditItemTemplate>
                    <EditCellStyle CssClass="tab-cell" HorizontalAlign="Left">
                    </EditCellStyle>
                    <CellStyle HorizontalAlign="Left" VerticalAlign="NotSet">
                    </CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="Motivo Rep." FieldName="MOTIVOREPROVACAO"
                    VisibleIndex="17" Width="330px">
                    <PropertiesComboBox ClientInstanceName="MotivoReprovacaoLancamentoRepasseId" DataSourceID="odsMotivoReprovacaoLancamentoRepasse"
                        TextField="DESCRICAO" ValueField="MOTIVOREPROVACAOLANCAMENTOREPASSEID">
                        <ClientSideEvents Init="function(s,e){ s.SetVisible(!rblAcao.GetValue()); }" />
                        <Items>
                            <dxe:ListEditItem Text="" Value="" />
                        </Items>
                    </PropertiesComboBox>
                    <EditCellStyle CssClass="tab-cell">
                    </EditCellStyle>
                    <CellStyle HorizontalAlign="Justify" VerticalAlign="NotSet">
                    </CellStyle>
                </dxwgv:GridViewDataComboBoxColumn>
            </Columns>
        </dxwgv:ASPxGridView>
        <br />
        <table width="800">
            <tr>
                <td width="300" align="right">
                    <span>Valor total lançado para repasses:</span>
                </td>
                <td width="500">
                    <span>
                        <asp:Label ID="lblValorTotalRepasse" runat="server"></asp:Label></span>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <span>Valor total da Parcela da Programação Orçamentária:</span>
                </td>
                <td>
                    <span>
                        <asp:Label ID="lblValorTotalParcela" runat="server"></asp:Label></span>
                </td>
            </tr>
        </table>

        <script language="javascript">
            function radAprovarTodos_Click() {

                var radAprovarTodos = $("#<%= radAprovarTodos.ClientID %>");
                var radReprovarTodos = $("#<%= radReprovarTodos.ClientID %>");
                var spanMotivoReprovacao = $("#spanMotivoReprovacao");

                if (radAprovarTodos.prop("checked")) {
                    spanMotivoReprovacao.hide();
                }
                else
                    if (radReprovarTodos.prop("checked"))
                    spanMotivoReprovacao.show();
                else
                    spanMotivoReprovacao.hide();

                spanMotivoReprovacao.find("#<%= cmbMotivoReprovacao.ClientID %>").val("");
            };

            $(document).ready(function() {
                $("#<%= radAprovarTodos.ClientID %>").click(radAprovarTodos_Click);
                $("#<%= radReprovarTodos.ClientID %>").click(radAprovarTodos_Click);
            });
        </script>

    </asp:PlaceHolder>
</asp:Content>
