<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Pagamento.aspx.cs" Inherits="Techne.Lyceum.Net.Transporte.Pagamento" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">


        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

        function EndRequestHandler(sender, args) {
            AddEvents();
        }

        $(document).ready(function() {
            AddEvents();
            ControlaBloqCampos();
        });

        function AddEvents() {

            $("#<%=grdPagamento.ClientID %> *[validar=true]").each(function(i) {
                var desconto = this;

                $(desconto).bind('change', function(e) {
                    CalculaDesconto(desconto);
                    e.stopImmediatePropagation();
                });

                $(desconto).bind('blur', function(e) {
                    CalculaDesconto(desconto);
                    e.stopImmediatePropagation();
                });

            });
        }


        function CalculaDesconto(jqObject) {

            var jqTRLancamento = $(jqObject).closest('tr');
            var jqInputDesconto = jqTRLancamento.find('input:text.input-desconto');
            var jqInputCalculado = jqTRLancamento.find('input:text.inputfinal');

            var valorIda = parseFloat($(jqObject).attr("input-ida").replace(",", "."));
            var valorVolta = parseFloat($(jqObject).attr("input-volta").replace(",", "."));
            var calculado = 0;
            var desconto = 0;

            if(jqInputDesconto.val() == null || jqInputDesconto.val() == "")
            {
                desconto = toFloat(jqInputDesconto.val());
            }

            calculado = (valorIda + valorVolta) - desconto;
            $(jqInputCalculado).val(parseFloat(calculado).toLocaleString("pt-BR", { style: "currency", currency: "BRL" }));
        }

        function toFloat(num) {
            dotPos = num.indexOf('.');
            commaPos = num.indexOf(',');

            if (dotPos < 0)
                dotPos = 0;

            if (commaPos < 0)
                commaPos = 0;

            if ((dotPos > commaPos) && dotPos)
                sep = dotPos;
            else {
                if ((commaPos > dotPos) && commaPos)
                    sep = commaPos;
                else
                    sep = false;
            }

            if (sep == false)
                return parseFloat(num.replace(/[^\d]/g, ""));

            return parseFloat(
        num.substr(0, sep).replace(/[^\d]/g, "") + '.' +
        num.substr(sep + 1, num.length).replace(/[^0-9]/, "")
    );

        }

        function ControlaBloqCampos() {

            $(".inputfinal").attr("readonly", "readonly");
            $(".inputfinal").css("background-color", "Gainsboro");
        }


        function ValidaPreenchimentoTotal() {

            //           // alert('ok');
            //            var valores = $("#<%=grdPagamento.ClientID %> input:text.inputfinal").filter(
            //                function() {
            //                    var vazio = this.value == "";

            //                    return !vazio;

            //                });
            //              
            //            var total = valores.length;
            //           // alert(total);
            //            return total;


            var valores = $("#<%=grdPagamento.ClientID %> input:text.inputfinal").filter(function(i, e) {
                // short for whatever you use to get the number

                return +parseFloat($(this).next().text().replace(/[^0-9\.-]+/g, "")) > 0
            }).each(
                function() {
                    items += parseFloat($(this).text());
                    //$('#items').html(items);
                });
            //alert(valores.lenght);


        }

        function onKeyUpOrChange(evt) {
            var newItem = evt.GetValue();
            //alert(newItem);
        }
      
    </script>

    <asp:Panel runat="server" ID="pnlDefinicao" GroupingText="Características do pagamento"
        Width="90%">
        <table width="90%">
            <tr>
                <td style="text-align: right; width: 10%">
                    <asp:Label Font-Names="Verdana" ID="Label2" runat="server" Text="Município:" SkinID="lblObrigatorio"> </asp:Label>
                </td>
                <td>
                    <asp:Label Font-Names="Verdana" ID="lblCodMunicipio" runat="server" SkinID="lblObrigatorio"> </asp:Label>
                    -
                    <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" SkinID="lblObrigatorio"> </asp:Label>
                </td>
                <td style="text-align: right; width: 10%">
                    <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" runat="server" Text="Unidade Ensino:"
                        SkinID="lblObrigatorio"> </asp:Label>
                </td>
                <td>
                    <asp:Label Font-Names="Verdana" ID="lblCenso" runat="server" SkinID="lblObrigatorio"> </asp:Label>
                    -
                    <asp:Label Font-Names="Verdana" ID="lblNomeEscola" runat="server" SkinID="lblObrigatorio"> </asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label Font-Names="Verdana" ID="Label1" runat="server" Text="Data Início:" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:Label Font-Names="Verdana" ID="lblDataInicio" runat="server" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label Font-Names="Verdana" ID="Label3" runat="server" Text="Data Fim:" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:Label Font-Names="Verdana" ID="lblDataFim" runat="server" SkinID="lblObrigatorio"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label Font-Names="Verdana" ID="lblDias" runat="server" Text="Dias Letivo:" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:Label Font-Names="Verdana" ID="lblDiasLetivo" runat="server" SkinID="lblObrigatorio"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:Panel ID="pnlGridMatriculas" runat="server">
        <asp:ObjectDataSource ID="odsPagamento" runat="server" TypeName="Techne.Lyceum.Net.Transporte.Pagamento"
            SelectMethod="Listar">
            <SelectParameters>
                <asp:ControlParameter ControlID="lblCenso" PropertyName="Text" Name="unidade" />
                <asp:ControlParameter ControlID="lblCodMunicipio" PropertyName="Text" Name="codMunicipio" />
                <asp:ControlParameter ControlID="lblDataInicio" PropertyName="Text" Name="dataInicio" />
                <asp:ControlParameter ControlID="lblDataFim" PropertyName="Text" Name="dataFim" />
                <asp:ControlParameter ControlID="lblDiasLetivo" PropertyName="Text" Name="diasLetivo" />
                <asp:ControlParameter ControlID="hdnIdPagamento" PropertyName="Value" Name="idPagamento" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:HiddenField runat="server" ID="hdnIdPagamento" />
        <dxwgv:ASPxGridView ID="grdPagamento" ClientInstanceName="grdPagamento" DataSourceID="odsPagamento"
            runat="server" EnableCallBacks="false" OnHtmlRowCreated="grdPagamento_HtmlRowCreated"
            KeyFieldName="ROTAID" OnCustomButtonCallback="grdPagamento_CustomButtonCallback"
            OnCustomUnboundColumnData="grdPagamento_CustomUnboundColumnData">
            <%--OnCustomSummaryCalculate="grdPagamento_CustomSummaryCalculate"--%>
            <SettingsPager Mode="ShowAllRecords" />
            <SettingsBehavior AllowDragDrop="false" ProcessSelectionChangedOnServer="false" AllowMultiSelection="false"
                AllowGroup="false" AllowSort="false" />
            <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s); }" />
            <Columns>
                <dxwgv:GridViewDataTextColumn Caption="Rota" FieldName="ROTAID" Name="ROTAID" UnboundType="Integer"
                    VisibleIndex="1" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="CODIGO" Name="CODIGO" UnboundType="String"
                    VisibleIndex="2" HeaderStyle-HorizontalAlign="Center">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="TURNO" Name="TURNO" UnboundType="String"
                    VisibleIndex="3" HeaderStyle-HorizontalAlign="Center">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Tipo" FieldName="TIPO" Name="TIPO" UnboundType="String"
                    VisibleIndex="4" HeaderStyle-HorizontalAlign="Center">
                    <%--                    <FooterTemplate>
                        Total</FooterTemplate>--%>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Quantidade Aluno Ida" FieldName="QUANTIDADEALUNOIDA"
                    Name="QUANTIDADEALUNOIDA" UnboundType="String" VisibleIndex="5" Width="80px"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Quantidade Aluno Volta" FieldName="QUANTIDADEALUNOVOLTA"
                    Name="QUANTIDADEALUNOVOLTA" UnboundType="String" VisibleIndex="6" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Quantidade Dias Ida" FieldName="QUANTIDADEDIASIDA"
                    Name="QUANTIDADEDIASIDA" UnboundType="String" VisibleIndex="7" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Quantidade Dias Volta" FieldName="QUANTIDADEDIASVOLTA"
                    Name="QUANTIDADEDIASVOLTA" UnboundType="String" VisibleIndex="8" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Quantidade Km Ida" FieldName="QUANTIDADEKMIDA"
                    Name="QUANTIDADEKMIDA" UnboundType="String" VisibleIndex="9" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Quantidade Km Volta" FieldName="QUANTIDADEKMVOLTA"
                    Name="QUANTIDADEKMVOLTA" UnboundType="String" VisibleIndex="10" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Valor Rota Ida" FieldName="VALORROTAIDA" Name="VALORROTAIDA"
                    UnboundType="String" VisibleIndex="11" Visible="false">
                    <PropertiesTextEdit Width="60px" DisplayFormatString="c" ValidationSettings-Display="Dynamic"
                        MaxLength="9">
                        <MaskSettings Mask="$&lt;0..9999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Valor Rota Volta" FieldName="VALORROTAVOLTA"
                    Name="VALORROTAVOLTA" UnboundType="String" VisibleIndex="12" Visible="false">
                    <PropertiesTextEdit Width="60px" DisplayFormatString="c" ValidationSettings-Display="Dynamic"
                        MaxLength="9">
                        <MaskSettings Mask="$&lt;0..9999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Valor Ida" FieldName="VALORCALCULADOIDA" Name="VALORCALCULADOIDA"
                    UnboundType="String" VisibleIndex="13" HeaderStyle-HorizontalAlign="Center">
                    <CellStyle HorizontalAlign="Left" />
                    <PropertiesTextEdit Width="60px" DisplayFormatString="c" ValidationSettings-Display="Dynamic"
                        MaxLength="9">
                        <MaskSettings Mask="$&lt;0..9999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Valor Volta" FieldName="VALORCALCULADOVOLTA"
                    Name="VALORCALCULADOVOLTA" UnboundType="String" VisibleIndex="14" HeaderStyle-HorizontalAlign="Center">
                    <CellStyle HorizontalAlign="Left" />
                    <PropertiesTextEdit Width="60px" DisplayFormatString="c" ValidationSettings-Display="Dynamic"
                        MaxLength="9">
                        <MaskSettings Mask="$&lt;0..9999999g&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataColumn Caption="Desconto" FieldName="DESCONTO" Name="DESCONTO"
                    UnboundType="String" VisibleIndex="15" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Left" />
                    <DataItemTemplate>
                        <asp:TextBox ID="txtDesconto" AutoPostBack="true" runat="server" CssClass="input-desconto"
                            Text='<%# Bind("DESCONTO") %>' OnTextChanged="txtDesconto_OnTextChanged" Style="text-align: center;
                            width: 45px;" />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Motivo" FieldName="MOTIVO" Name="MOTIVO" UnboundType="String"
                    VisibleIndex="16" HeaderStyle-HorizontalAlign="Center">
                    <DataItemTemplate>
                        <asp:HiddenField runat="server" ID="hdnSituacaoPagamento" Value='<%# Bind("MOTIVO") %>' />
                        <asp:DropDownList ID="ddlSituacaoPagamento" runat="server" DataSourceID="odsSituacaoPagamento"
                            DataTextField="DESCRICAO" DataValueField="SITUACAOPAGAMENTOID" Width="300px">
                        </asp:DropDownList>
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Valor a Pagar" FieldName="ValorFinal" Name="ValorFinal"
                    UnboundType="Decimal" VisibleIndex="15" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />
                    <CellStyle HorizontalAlign="Left" />
                    <DataItemTemplate>
                        <asp:TextBox ID="txtValorFinal" CssClass="inputfinal" runat="server" Style="text-align: center;
                            width: 55px" />
                        <clientsideevents keyup="function(s,e){onKeyUpOrChange(e)}" textchanged="function(s,e){onKeyUpOrChange(e)}" />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="SITUACAO" Name="SITUACAO"
                    UnboundType="Decimal" VisibleIndex="18">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewCommandColumn VisibleIndex="19" ButtonType="Link" Width="50px" Caption="Dados Rota"
                    Name="DadosRota">
                    <CustomButtons>
                        <dxwgv:GridViewCommandColumnCustomButton ID="btnDados" Text="Dados Rota" Image-Width="50px"
                            Visibility="AllDataRows">
                        </dxwgv:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dxwgv:GridViewCommandColumn>
            </Columns>
            <Settings ShowFooter="True" />
            <%--<TotalSummary>
                <dxwgv:ASPxSummaryItem FieldName="VALORCALCULADOIDA" SummaryType="Sum" DisplayFormat="c" />
                <dxwgv:ASPxSummaryItem FieldName="VALORCALCULADOVOLTA" SummaryType="Sum" DisplayFormat="c" />
                <dxwgv:ASPxSummaryItem FieldName="ValorFinal" SummaryType="Custom" DisplayFormat="c" />
            </TotalSummary>--%>
        </dxwgv:ASPxGridView>
        <br />
        <br />
        <table>
            <tr>
                <td style="font-weight: bold; font-size: 15px;">
                    <asp:Label ID="lblDescValorTotal" runat="server" Text="Valor Total a Pagar: R$"></asp:Label>
                </td>
                <td style="font-weight: bold; font-size: 15px;">
                    <asp:Label ID="lblValorTotalUnidade" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:ObjectDataSource ID="odsSituacaoPagamento" runat="server" TypeName="Techne.Lyceum.Net.Transporte.Pagamento"
            SelectMethod="ListarSituacaoPagamento"></asp:ObjectDataSource>
    </asp:Panel>
    <br />
    <br />
    <asp:Panel ID="botoes" runat="server">
        <table>
            <tr>
                <td align="left">
                    <asp:ImageButton ID="btnSalvar" OnClick="btnSalvar_Click" SkinID="BcSalva" runat="server"
                        Visible="true" OnClientClick="return confirm('Confirma o lançamento do pagamento?');" />
                </td>
                <td align="left">
                    <asp:ImageButton ID="btnCancelar" runat="server" SkinID="Cancelar" OnClick="btnCancelarVoltar_Click" />
                </td>
                <td align="left">
                    <asp:ImageButton ID="btnVoltar" runat="server" SkinID="Voltar" OnClick="btnCancelarVoltar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <dxpc:ASPxPopupControl ID="pucRota" ClientInstanceName="pucRota" runat="server" Modal="true"
        ShowShadow="true" AllowDragging="false" AllowResize="false" ShowCloseButton="false"
        ShowFooter="false" ShowHeader="false" ShowSizeGrip="False" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" EnableAnimation="true" Width="600">
        <Border BorderColor="Gainsboro" BorderStyle="Ridge" BorderWidth="4px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,15000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <table>
                    <tr>
                        <td style="font-weight: bold; font-size: 13px;">
                            <asp:Label ID="Label4" runat="server" Text="Código da Rota:"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblCodigo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; font-size: 13px;">
                            <asp:Label ID="Label95" runat="server" Text="Escola:"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblEscola" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; font-size: 13px;">
                            <asp:Label ID="Label96" runat="server" Text="Regional:"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblRegionalRota" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; font-size: 13px;">
                            <asp:Label ID="Label6" runat="server" Text="Município:"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblMunicipioRota" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; font-size: 13px;">
                            <asp:Label ID="Label7" runat="server" Text="Região Financeira:"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblRegiaoFinanceira" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; font-size: 13px;">
                            <asp:Label ID="Label8" runat="server" Text="CNPJ:"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblCnpj" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; font-size: 13px;">
                            <asp:Label ID="Label9" runat="server" Text="Turno:"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblTurno" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="font-weight: bold; font-size: 13px;">
                            <asp:Label ID="Label10" runat="server" Text="Tipo Cálculo Pagamento:"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblTipoCalculoPagamento" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Panel ID="pnlIda" runat="server" GroupingText="Trajeto de Ida" Width="100%">
                    <table>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label120" runat="server" Text="Contratação:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTipoContratacaoIda" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label130" runat="server" Text="Valor:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblValorRotaIda" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label5" runat="server" Text="Quantidade dias:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblQtdeDiasIda" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label14" runat="server" Text="Quantidade Km:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblQuantidadeKmIda" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label15" runat="server" Text="Quantidade Alunos:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblQuantidadeAlunoIda" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label11" runat="server" Text="Prestador:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPrestadorIda" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label122" runat="server" Text="Condutor:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCondutorIda" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label133" runat="server" Text="Veículo:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblVeiculoIda" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
                <asp:Panel ID="pnlVolta" runat="server" GroupingText="Trajeto de Volta" Width="100%">
                    <table>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label16" runat="server" Text="Contratação:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblTipoContratacaoVolta" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label17" runat="server" Text="Valor:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblValorRotaVolta" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label12" runat="server" Text="Quantidade dias:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblQtdeDiasVolta" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label18" runat="server" Text="Quantidade Km:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblQuantidadeKmVolta" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label19" runat="server" Text="Quantidade Alunos:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblQuantidadeAlunoVolta" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label20" runat="server" Text="Prestador:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPrestadorVolta" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label21" runat="server" Text="Condutor:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCondutorVolta" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold; font-size: 13px;">
                                <asp:Label ID="Label22" runat="server" Text="Veículo:"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblVeiculoVolta" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
                <table width="100%">
                    <tr>
                        <td align="right">
                            <asp:Button ID="btCancelar" runat="server" Text="Cancelar" OnClientClick="pucRota.Hide();" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
