<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modulos/LyceumMaster.Master"
    CodeBehind="AplicacoesFinanceiras.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.AplicacoesFinanceiras" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function MascaraMoeda(objTextBox, SeparadorMilesimo, SeparadorDecimal, e, Tamanho) {

            var sep = 0;
            var key = '';
            var i = j = 0;
            var len = len2 = 0;
            var strCheck = '0123456789';
            var aux = aux2 = '';
            var whichCode = (window.Event) ? e.which : e.keyCode;
            if (whichCode == 13) return true;
            if (whichCode == 8) return true;

            key = String.fromCharCode(whichCode); // Valor para o código da Chave
            if (strCheck.indexOf(key) == -1) return false; // Chave inválida
            if (Tamanho < objTextBox.value.length) return false; // Tamanho
            len = objTextBox.value.length;

            for (i = 0; i < len; i++)
                if ((objTextBox.value.charAt(i) != '0') && (objTextBox.value.charAt(i) != SeparadorDecimal)) break;
            aux = '';
            for (; i < len; i++)
                if (strCheck.indexOf(objTextBox.value.charAt(i)) != -1) aux += objTextBox.value.charAt(i);
            aux += key;
            len = aux.length;
            if (len == 0) objTextBox.value = '';
            if (len == 1) objTextBox.value = '0' + SeparadorDecimal + '0' + aux;
            if (len == 2) objTextBox.value = '0' + SeparadorDecimal + aux;
            if (len > 2) {
                aux2 = '';
                for (j = 0, i = len - 3; i >= 0; i--) {
                    if (j == 3) {
                        aux2 += SeparadorMilesimo;
                        j = 0;
                    }
                    aux2 += aux.charAt(i);
                    j++;
                }
                objTextBox.value = '';
                len2 = aux2.length;
                for (i = len2 - 1; i >= 0; i--)
                    objTextBox.value += aux2.charAt(i);
                objTextBox.value += SeparadorDecimal + aux.substr(len - 2, len);
            }
            return false;
        }
        function moeda(a, e, r, t) {

            var n = "", h = j = 0, u = tamanho2 = 0, l = ajd2 = "", o = window.Event ? t.which : t.keyCode;
            if (13 == o || 8 == o)
                return !0;
            if (n = String.fromCharCode(o),
    -1 == "0123456789".indexOf(n))
                return !1;
            for (u = a.value.length,
    h = 0; h < u && ("0" == a.value.charAt(h) || a.value.charAt(h) == r); h++)
                ;
            for (l = ""; h < u; h++)
-1 != "0123456789".indexOf(a.value.charAt(h)) && (l += a.value.charAt(h));
            if (l += n,
    0 == (u = l.length) && (a.value = ""),
    1 == u && (a.value = "0" + r + "0" + l),
    2 == u && (a.value = "0" + r + l),
    u > 2) {
                for (ajd2 = "",
        j = 0,
        h = u - 3; h >= 0; h--)
                    3 == j && (ajd2 += e,
            j = 0),
            ajd2 += l.charAt(h),
            j++;
                for (a.value = "",
        tamanho2 = ajd2.length,
        h = tamanho2 - 1; h >= 0; h--)
                    a.value += ajd2.charAt(h);
                a.value += r + l.substr(u - 2, u)
            }
            return !1
        }
    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Dados das aplicações financeiras"
        Width="617px">
        <table style="width: 600px">
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label3" runat="server" Font-Names="Verdana" Text="Unidade de Ensino:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                        MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao, nucleo, municipio,id_regional, ua_atual,ua_antiga  from VW_UNIDADE_ENSINO_SITUACAO "
                        GridWidth="850px" OnChanged="tseUnidadeResponsavel_Changed" SqlOrder="nome_comp">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="CGC" FieldName="cgc" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label10" runat="server" Font-Names="Verdana" Text="Extrato Bancário:"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseExtratoBancario" runat="server" Caption="" Key="EXTRATOBANCARIOID"
                        MaxLength="20" ArgumentColumns="50" Columns="10" Argument="DESCRICAO" SqlSelect=" select OBSERVACAO, CENSO, ANO, MES from PrestacaoContas.VW_EXTRATOBANCARIO eb "
                        GridWidth="850px" DataType="Number" SqlWhere=" eb.censo = #tseUnidadeResponsavel# ">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Observação" FieldName="OBSERVACAO" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Ano" FieldName="ANO" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Mês" FieldName="MES" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="EXTRATOBANCARIOID"
                                Width="20%" Visible="false" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
                <td>
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click"
                        OnClientClick="return Valida();" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <div class="divEditBlock" style="width: 950px;">
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:Label runat="server" ID="lblBloco" Text="Poupança" SkinID="BcTitulo" />
    </div>
    <br />
    <asp:Panel ID="pnlAplicacao" runat="server">
        <asp:Panel ID="pnlDados" runat="server" Visible="false">
            <table style="width: 600px">
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label1" runat="server" Font-Names="Verdana" Text="Unidade Ensino:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseUnidadeResponsavelDados" runat="server" Caption="" Key="unidade_ens"
                            MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao, nucleo, municipio,id_regional, ua_atual,ua_antiga  from VW_UNIDADE_ENSINO_SITUACAO "
                            GridWidth="850px" SqlOrder="nome_comp" OnChanged="tseUnidadeResponsavelDados_Changed">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="12%" />
                                <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="CGC" FieldName="cgc" Width="10%" />
                                <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label2" runat="server" Font-Names="Verdana" Text="Extrato Bancario:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                     <tweb:TSearchBox ID="tseExtratoBancarioDados" runat="server" Caption="" Key="EXTRATOBANCARIOID"
                        MaxLength="20" ArgumentColumns="50" Columns="10" Argument="DESCRICAO" SqlSelect=" select OBSERVACAO, CENSO, ANO, MES from PrestacaoContas.VW_EXTRATOBANCARIO eb "
                        GridWidth="850px" DataType="Number" SqlWhere=" eb.censo = #tseUnidadeResponsavelDados# ">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Observação" FieldName="OBSERVACAO" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Ano" FieldName="ANO" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Mês" FieldName="MES" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="EXTRATOBANCARIOID"
                                Width="20%" Visible="false" />
                        </GridColumns>
                    </tweb:TSearchBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label4" runat="server" Font-Names="Verdana" Text="Valor Aplicação:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtValorAplicacao" runat="server" Font-Names="Verdana" OnKeyPress="return(moeda(this,'.',',',event))"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label5" runat="server" Font-Names="Verdana" Text="Justificativa:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtJustificativa" runat="server" Font-Names="Verdana" Width="350px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label6" runat="server" Font-Names="Verdana" Text="Anexo do Comprovante:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:FileUpload ID="FileUpload2" runat="server" />
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <table>
                <tr>
                    <td align="left">
                        <asp:Button ID="btnSalvar" runat="server" Font-Names="Verdana" Text="Salvar" OnClick="btnSalvar_Click">
                        </asp:Button>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:ObjectDataSource ID="odsAplicacoesFinanceiras" TypeName="Techne.Lyceum.Net.PrestacaoContas.AplicacoesFinanceiras"
            runat="server" SelectMethod="ListaDados" UpdateMethod="Update" DeleteMethod="Delete">
            <SelectParameters>
                <asp:ControlParameter ControlID="tseUnidadeResponsavel" DefaultValue="" Name="censo"
                    PropertyName="DBValue" />
                <asp:ControlParameter ControlID="tseExtratoBancario" DefaultValue="" Name="extratoBancario"
                    PropertyName="DBValue" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:HiddenField ID="hdnAplicacaoFinanceiraId" runat="server" />
        <asp:HiddenField ID="hdnAplicacaoFinanceiraComprovanteArquivoId" runat="server" />
        <dxwgv:ASPxGridView ID="grdAplicacoesFinanceiras" runat="server" DataSourceID="odsAplicacoesFinanceiras"
            KeyFieldName="APLICACAOFINANCEIRAID" AutoGenerateColumns="false" ClientInstanceName="grdAplicacoesFinanceiras"
            OnCustomButtonCallback="grdAplicacoesFinanceiras_CustomButtonCallback" OnCustomUnboundColumnData="grdAplicacoesFinanceiras_CustomUnboundColumnData"
            OnRowDeleting="grdAplicacoesFinanceiras_RowDeleting" Width="80%" EnableCallBacks="false" OnCustomButtonInitialize="grdAplicacoesFinanceiras_CustomButtonInitialize">
            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsEditing Mode="Inline" />
            <SettingsText ConfirmDelete="Deseja executar a operação de exclusão do período do rendimento de aplicação financeira?"
                EmptyDataRow="Não existem dados." />
            <SettingsBehavior ConfirmDelete="true" />
            <Columns>
                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                    <CancelButton Visible="true" Text="Cancelar">
                        <Image Url="~/img/bt_cancelar.png" />
                    </CancelButton>
                    <ClearFilterButton Text="Limpar" Visible="True">
                        <Image Url="~/img/bt_limpa.png" />
                    </ClearFilterButton>
                    <UpdateButton Visible="true" Text="Alterar">
                        <Image Url="../img/bt_salvar.png" />
                    </UpdateButton>
                    <DeleteButton Visible="True" Text="Remover">
                        <Image Url="../img/bt_exclui2.png" />
                    </DeleteButton>
                    <CustomButtons>
                        <dxwgv:GridViewCommandColumnCustomButton Text="Editar" ID="btnEditarCustom" Visibility="AllDataRows"
                            Image-Url="~/img/bt_editar.png" Image-Height="15px" Image-AlternateText="Editar">
                        </dxwgv:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="APLICACAOFINANCEIRACOMPROVANTEARQUIVOID" FieldName="APLICACAOFINANCEIRACOMPROVANTEARQUIVOID"
                    VisibleIndex="0" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="APLICACAOFINANCEIRAID" FieldName="APLICACAOFINANCEIRAID"
                    VisibleIndex="0" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="EXTRATOBANCARIOID" FieldName="EXTRATOBANCARIOID"
                    VisibleIndex="1" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Censo" FieldName="CENSO" VisibleIndex="2"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="UNIDADEENSINO"
                    VisibleIndex="3" Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Ano Extrato" FieldName="ANO" VisibleIndex="4"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Mês Extrato" FieldName="MES" VisibleIndex="5"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Valor Aplicação" FieldName="VALOR" VisibleIndex="6"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Justificativa" FieldName="JUSTIFICATIVA" VisibleIndex="7"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="TIPOARQUIVO" FieldName="TIPOARQUIVO" VisibleIndex="13"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Comprovante" Name="btnVisualizar" VisibleIndex="10"
                    Width="35px">
                    <DataItemTemplate>
                        <asp:ImageButton ID="btnVisualizar" runat="server" EnableViewState="false" CommandArgument='<%# Eval("APLICACAOFINANCEIRAID") + "," + Eval("TIPOARQUIVO") %>'
                            OnCommand="btnVisualizar_Command" ImageUrl="~/img/bt_busca.png" Height="15px"
                            AlternateText="Visualizar Documento"></asp:ImageButton>
                    </DataItemTemplate>
                </dxwgv:GridViewDataTextColumn>
            </Columns>
        </dxwgv:ASPxGridView>
    </asp:Panel>
    <br />
    <dxpc:ASPxPopupControl ID="pucVisualizarArquivo" ClientInstanceName="pucVisualizarArquivo"
        runat="server" Modal="true" ShowShadow="false" AllowDragging="true" AllowResize="false"
        ShowCloseButton="true" ShowFooter="false" ShowSizeGrip="False" EnableAnimation="false"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/" HeaderText="Visualizar Documento">
        <HeaderStyle HorizontalAlign="Center" />
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <dxe:ASPxBinaryImage ID="bimgArquivo" Width="350px" Height="350px" runat="server"
                    Visible="false" StoreContentBytesInViewState="True" AlternateText="sem foto"
                    ClientInstanceName="bimgArquivo">
                    <EmptyImage AlternateText="sem foto" Url="~/Images/semfoto.jpg" />
                </dxe:ASPxBinaryImage>
                <asp:Literal ID="ltEmbed" runat="server" Visible="false" />
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
