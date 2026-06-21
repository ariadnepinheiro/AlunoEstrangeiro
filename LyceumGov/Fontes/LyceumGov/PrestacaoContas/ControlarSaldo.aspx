<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ControlarSaldo.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.ControlarSaldo" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript" src="../Scripts/jquery-ui.js"></script>

    <script src="../Scripts/jquery.maskedinput-1.2.2.min.js" type="text/javascript"></script>

    <script type="text/javascript">
    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informações do Projeto / Programa"
        Width="70%">
        <table style="width: 100%">
            <tr>
                <td align="right">
                    <asp:Label Font-Names="Verdana" ID="Label2" SkinID="lblObrigatorio" runat="server"
                        Text="Período Referência:*"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tsePeriodoReferencia" runat="server" Argument="DESCRICAO" ArgumentColumns="50"
                        MaxLength="20" Columns="10" AutoPostBack="True" Caption="" SqlOrder="ANO, MESINICIAL DESC"
                        OnChanged="tsePeriodoReferencia_Changed" Key="PERIODOREFERENCIAID" SqlSelect=" SELECT ANO, MESINICIAL, MESFINAL, REFERENCIA FROM PrestacaoContas.VW_PERIODOREFERENCIA "
                        DataType="Number">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="PERIODOREFERENCIAID" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Período" FieldName="DESCRICAO" Width="80%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblUnidade" runat="server" Text="Unidade de Ensino:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                        OnChanged="tseUnidadeResponsavel_Changed" EnableViewState="true" Argument="nome_comp"
                        SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,municipio, id_regional from VW_UNIDADE_ENSINO_SITUACAO "
                        ArgumentColumns="75" Columns="10" SqlOrder="nome_comp">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="70%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="setor" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="CGC" FieldName="cgc" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Municipio" FieldName="municipio" Width="0%" Visible="false" />
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="id_regional" Width="0%" Visible="false" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblPlanoTrabalho" runat="server" Font-Names="Verdana" Text="Projeto / Programa:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <%--  <tweb:TSearchBox ID="tsePlanoTrabalho2" runat="server" OnChanged="tsePlanoTrabalho_Changed"
                        MaxLength="9" DataType="Number" SqlSelect="  SELECT pt.descricao, F.DESCRICAO AS FINALIDADE, P.DESCRICAO AS PROGRAMA, F.FINALIDADEID from  PrestacaoContas.PLANOTRABALHO pt inner join PrestacaoContas.FINALIDADE f on pt.FINALIDADEID = f.FINALIDADEID inner join PrestacaoContas.PROGRAMATRABALHO po on pt.PROGRAMATRABALHOID = po.PROGRAMATRABALHOID inner join PrestacaoContas.WSPROGRAMASEFAZ p on po.WSPROGRAMASEFAZID = p.WSPROGRAMASEFAZID "
                         Key="PLANOTRABALHOID" >
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="PLANOTRABALHOID" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Finalidade" FieldName="FINALIDADE" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Programa" FieldName="PROGRAMA" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="FINALIDADEID" FieldName="FINALIDADEID" Width="0%" Visible="false" />
                        </GridColumns>
                    </tweb:TSearchBox>--%>
                    <tweb:TSearchBox ID="tsePlanoTrabalho" runat="server" Argument="descricao" Key="PLANOTRABALHOID"
                        MaxLength="20" ArgumentColumns="50" Columns="10" DataType="Number" AutoPostBack="false"
                        SqlSelect=" select pt.descricao,F.DESCRICAO AS FINALIDADE,PT.FINALIDADEID from  PrestacaoContas.PLANOTRABALHO pt inner join PrestacaoContas.PROGRAMATRABALHO po on pt.PROGRAMATRABALHOID = po.PROGRAMATRABALHOID inner join PrestacaoContas.WSPROGRAMASEFAZ p on po.WSPROGRAMASEFAZID = p.WSPROGRAMASEFAZID  INNER JOIN PrestacaoContas.FINALIDADE F ON F.FINALIDADEID = PT.FINALIDADEID"
                        OnChanged="tsePlanoTrabalho_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="PLANOTRABALHOID" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="DESCRICAO" Width="90%" />
                            <tweb:TSearchBoxColumn Caption="Finalidade" FieldName="FINALIDADE" Width="90%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <asp:Button ID="btnBuscar" runat="server" Font-Names="Verdana" OnClick="btnBuscar_Click"
                        Text="Buscar"></asp:Button>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Label ID="lblMensagem" runat="server"></asp:Label>
    <br />
    <div class="divEditBlock" style="width: 70%;">
        <asp:Label runat="server" ID="lblTitulo" Text="Saldo de Projeto / Programa" SkinID="BcTitulo" />
    </div>
    <asp:Panel ID="pnlSaldo" runat="server" Visible="false" Width="70%">
        <table style="width: 100%">
            <tr>
                <td align="right">
                    <asp:Label ID="Label3" runat="server" Font-Names="Verdana" Text="Finalidade:"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblFinalidade" runat="server" SkinID="lblObrigatorio" Font-Names="Verdana"
                        Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblSaldoInicial" runat="server" SkinID="lblObrigatorio" Font-Names="Verdana"
                        Text=""></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label4" runat="server" Font-Names="Verdana" Text="Saldo Inicial (A): saldo do Projeto / Programa da data imediatamente anterior ao dia o ínicio do período de referência selecionado."></asp:Label>
                </td>
            </tr>
              <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblRepasses" runat="server" SkinID="lblObrigatorio" Font-Names="Verdana"
                        Text=""></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label5" runat="server" Font-Names="Verdana" Text="Repasses (B): soma total dos valores referentes às parcelas do Projeto / Programa até a data final do período escolhido."></asp:Label>
                </td>
            </tr>
              <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblDespesas" runat="server" SkinID="lblObrigatorio" Font-Names="Verdana"
                        Text=""></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label6" runat="server" Font-Names="Verdana" Text="Despesas (C): soma total dos valores de despesas lançados pela unidade até a data final do período escolhido."></asp:Label>
                </td>
            </tr>
              <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblCreditos" runat="server" SkinID="lblObrigatorio" Font-Names="Verdana"
                        Text=""></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label7" runat="server" Font-Names="Verdana" Text="Créditos (D): soma total das devoluções proventes do cumprimento de exigências referentes ao Projeto / Programa."></asp:Label>
                </td>
            </tr>
              <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblCreditosAnalisados" runat="server" SkinID="lblObrigatorio" Font-Names="Verdana"
                        Text=""></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label9" runat="server" Font-Names="Verdana" Text="Créditos Indenizatórios (E): Tela de Créditos e Débitos"></asp:Label>
                </td>
            </tr>
              <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblDebitosAnalisados" runat="server" SkinID="lblObrigatorio" Font-Names="Verdana"
                        Text=""></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label10" runat="server" Font-Names="Verdana" Text="Débitos Descontados (F): Tela de Créditos e Débitos"></asp:Label>
                </td>
            </tr>
              <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            
            <tr>
                <td align="right">
                    <asp:Label ID="lblSaldo" runat="server" SkinID="lblObrigatorio" Font-Names="Verdana"
                        Text=""></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label8" runat="server" Font-Names="Verdana" Text="Saldo (A + B + D + E - C - F): valor disponível para utilização nas ações referentes ao Projeto / Programa."></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
