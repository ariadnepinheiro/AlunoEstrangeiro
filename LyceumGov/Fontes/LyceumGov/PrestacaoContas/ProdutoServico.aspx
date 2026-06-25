<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ProdutoServico.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.ProdutoServico" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function OnlyNumericEntry(e) {

            var charCode = (e.which) ? e.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }

        function mostrarResultado(box, num_max, spContador) {
            var contagem_carac = box.length;
            if (contagem_carac != 0) {
                document.getElementById(spContador).innerHTML = contagem_carac + " caracteres digitados";
                if (contagem_carac == 1) {
                    document.getElementById(spContador).innerHTML = contagem_carac + " caracter digitado";
                }
                if (contagem_carac >= num_max) {
                    document.getElementById(spContador).innerHTML = "Limite de caracteres excedido!";
                }
            } else {
                document.getElementById(spContador).innerHTML = "Limite de " + num_max + " caracteres";
            }
        }
        function contarCaracteres(box, valor, spContador, campoMult) {

            var conta = valor - box.length;
            document.getElementById(spContador).innerHTML = "Você ainda pode digitar " + conta + " caracteres";
            if (box.length >= valor) {
                document.getElementById(spContador).innerHTML = "Limite excedido.";
                campoMult.value = campoMult.value.substr(0, valor);
            }
        }
        function Bloqueio() {

            var divBloqueio = document.getElementById("dvbloqueio");
            divBloqueio.className = "Bloqueado";
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

    <div id="dvbloqueio" class="Desbloqueado">
    </div>
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Filtros" Width="60%">
        <table>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label1" runat="server" Text="Grupo de Produto / Serviço:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseGrupoFiltro" runat="server" Caption="" SqlOrder="descricao"
                        Argument="descricao" SqlSelect="select  convert(varchar, produtoservicogrupoid) as produtoservicogrupoid, descricao, codigocnae FROM   PrestacaoContas.PRODUTOSERVICOGRUPO"
                        Columns="10" ArgumentColumns="50" AutoPostBack="true" Key="codigocnae" MaxLength="500"
                        OnChanged="tseGrupoFiltro_Changed" DataType="VarChar">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="produtoservicogrupoid" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Código CNAE" FieldName="codigocnae" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="60%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblProdutoServico" runat="server" Text="Produto / Serviço:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseProdutoServico" runat="server" Caption="" SqlOrder="nome"
                        Argument="nome" SqlSelect="select produtoservicoid,nome,codigocnae from prestacaocontas.produtoservico p inner join prestacaocontas.produtoservicogrupo s on p.produtoservicogrupoid=s.produtoservicogrupoid"
                        Columns="10" ArgumentColumns="50" AutoPostBack="true" Key="produtoservicoid"
                        MaxLength="5" SqlWhere=" codigocnae = #tseGrupoFiltro# " OnChanged="tseProdutoServico_Changed"
                        DataType="Number">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Codigo" FieldName="produtoservicoid" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="nome" FieldName="nome" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Código CNAE" FieldName="codigocnae" Visible="false"
                                Width="0%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
        <p style="text-align: right">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" /></p>
    </asp:Panel>
    <br />
    <div class="divEditBlock" style="width: 950px;">
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click" />
        <asp:Label runat="server" ID="lblBlocoItem" Text="Produtos e Serviços" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsItem" runat="server" EnableClientScript="true" ShowMessageBox="true"
            ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <dxtc:ASPxPageControl ID="pcProdutoServico" runat="server" ActiveTabIndex="0" Width="60%"
        OnTabClick="pcProdutoServico_TabClick">
        <TabPages>
            <dxtc:TabPage Text="Produto / Serviço">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl1" runat="server">
                        <table>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="Label2" runat="server" Text="Grupo Produto / Serviço:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <tweb:TSearchBox ID="tseGrupo" runat="server" Caption="" SqlOrder="descricao" Argument="descricao"
                                        SqlSelect="select  convert(varchar, produtoservicogrupoid) as produtoservicogrupoid, descricao, codigocnae FROM   PrestacaoContas.PRODUTOSERVICOGRUPO"
                                        Columns="10" ArgumentColumns="50" AutoPostBack="true" Key="codigocnae" MaxLength="500"
                                        OnChanged="tseGrupo_Changed" DataType="VarChar">
                                        <GridColumns>
                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="produtoservicogrupoid" Width="10%" />
                                            <tweb:TSearchBoxColumn Caption="Código CNAE" FieldName="codigocnae" Width="30%" />
                                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="60%" />
                                        </GridColumns>
                                    </tweb:TSearchBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="Label9" runat="server" Text="Nome Produto / Serviço:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtProduto" runat="server" MaxLength="500" Width="600px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="Label6" runat="server" Text="NCM:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNcm" runat="server" MaxLength="20" />
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkNcmNaoEspecifico" runat="server" Text="NCM Não Específico" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="Label10" runat="server" Text="Detalhe:"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtDetalhes" runat="server" MaxLength="1000" TextMode="MultiLine"
                                        name="txtDetalhes" Height="75px" Width="600px" onkeyup="mostrarResultado(this.value,100,'spContadorDetalhe');contarCaracteres(this.value,1000,'spContadorDetalhe',this)" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td style="text-align: right;">
                                    <span id="spContadorDetalhe" style="font-family: Georgia; text-align: right">Limite
                                        de 1000 caracteres</span>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="Label3" runat="server" Text="Tipo*: " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlTipo" Width="100%" runat="server" DataTextField="DESCRICAO"
                                        DataValueField="TIPOPRODUTOSERVICOID">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="Label11" runat="server" Text="Código da Tabela de Preços de Valores Máximos: "></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCodigoFGV" runat="server" Width="70%" onkeypress="return OnlyNumericEntry(event)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="Label4" runat="server" Text="Unidade de Medida*: " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlUnidadeMedida" Width="100%" runat="server" DataTextField="DESCRICAO"
                                        DataValueField="UNIDADEMEDIDAID">
                                    </asp:DropDownList>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="Label5" runat="server" Text="Finalidade*: " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList Width="70%" ID="ddlFinalidade" runat="server" DataTextField="DESCRICAO"
                                        DataValueField="FINALIDADEID">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlCaracteristica" runat="server" GroupingText="Características do Produto / Serviço"
                                        Font-Names="Verdana" Width="400px">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox runat="server" ID="chkPequenaDespesa" Text="Pequena Despesa" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox runat="server" ID="chkInventariavel" Text="Inventariável" />
                                                </td>
                                                <td>
                                                    <asp:CheckBox runat="server" ID="chkOrcamento" Text="Necessita Orçamento" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Restrição de Valores">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl3" runat="server">
                    <br />
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label7" runat="server" Font-Names="Verdana" Text="Áreas Geográficas:*"
                                        SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                     <asp:DropDownList ID="ddlRegiaoFgv" Width="100%" runat="server" DataTextField="DESCRICAO"
                                        DataValueField="REGIAOFGVID">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblValor" runat="server" Font-Names="Verdana" Text="Valor Máximo:*"
                                        SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtValorMaximo" runat="server" Width="80px" OnKeyPress="return(moeda(this,'.',',',event))" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label Font-Names="Verdana" ID="lblDataInicio" SkinID="lblObrigatorio" runat="server"
                                        Text="Data Início:*"></asp:Label>
                                </td>
                                <td>
                                    <dxe:ASPxDateEdit ID="dtDataInicio" runat="server" Width="100px" Enabled="true" EnableDefaultAppearance="true"
                                        ClientInstanceName="dtDataInicio" CalendarProperties-ClearButtonText="Limpar"
                                        CalendarProperties-TodayButtonText="Hoje">
                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                        </CalendarProperties>
                                    </dxe:ASPxDateEdit>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label Font-Names="Verdana" ID="Label8" runat="server"
                                        Text="Data Fim:"></asp:Label>
                                </td>
                                <td>
                                    <dxe:ASPxDateEdit ID="dtDataFim" runat="server" Width="100px" Enabled="true" EnableDefaultAppearance="true"
                                        ClientInstanceName="dtDataFim" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                        </CalendarProperties>
                                    </dxe:ASPxDateEdit>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnSalvarRestricao" runat="server" ValidationGroup="SalvarForm" Text="Salvar"
                                        OnClick="btnSalvarRestricao_Click" OnClientClick="this.disabled = true; this.value = 'Aguarde...';"
                                        UseSubmitBehavior="false" />
                                </td>
                            </tr>
                        </table>
                        <asp:ObjectDataSource ID="odsRestricao" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.ProdutoServico"
                            SelectMethod="ListaRestricao" UpdateMethod="Update" DeleteMethod="Delete">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseProdutoServico" DefaultValue="DBValue" Name="produtoServicoId" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <dxwgv:ASPxGridView ID="grdRestricao" runat="server" DataSourceID="odsRestricao"
                            KeyFieldName="PRODUTOSERVICOVALORMAXIMOID" AutoGenerateColumns="false" ClientInstanceName="grdRestricao"
                            OnInitNewRow="grdRestricao_InitNewRow" OnStartRowEditing="grdRestricao_StartRowEditing"
                            OnRowUpdating="grdRestricao_RowUpdating" OnRowDeleting="grdRestricao_RowDeleting"
                            OnAfterPerformCallback="grdRestricao_AfterPerformCallback" Width="70%">
                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsEditing Mode="Inline" />
                            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                            <SettingsBehavior ConfirmDelete="true" />
                            <Columns>
                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
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
                                <dxwgv:GridViewDataTextColumn Caption="PRODUTOSERVICOVALORMAXIMOID" Name="PRODUTOSERVICOVALORMAXIMOID"
                                    VisibleIndex="1" FieldName="PRODUTOSERVICOVALORMAXIMOID" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                 <dxwgv:GridViewDataTextColumn Caption="REGIAOFGVID" Name="REGIAOFGVID"
                                    VisibleIndex="2" FieldName="REGIAOFGVID" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                 <dxwgv:GridViewDataTextColumn Caption="Áreas Geográficas" Name="REGIAOFGV"
                                    VisibleIndex="3" FieldName="REGIAOFGV" Visible="true" ReadOnly="true">
                                </dxwgv:GridViewDataTextColumn>                                
                                <dxwgv:GridViewDataTextColumn Caption="Valor Máximo" Name="VALORMAXIMO" VisibleIndex="4"
                                    FieldName="VALORMAXIMO">
                                    <PropertiesTextEdit MaxLength="13" Width="150px" DisplayFormatString="c">
                                        <MaskSettings Mask="$&lt;0..9999999999&gt;.&lt;00..99&gt;" IncludeLiterals="DecimalSymbol" />
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataDateColumn VisibleIndex="5" Caption="Data Início" Name="DATAINICIO"
                                    FieldName="DATAINICIO" Width="100px" Visible="true">
                                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                                        <ValidationSettings>
                                            <RequiredField IsRequired="true" ErrorText="Campo Obrigatório." />
                                        </ValidationSettings>
                                    </PropertiesDateEdit>
                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                    </CellStyle>
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataDateColumn VisibleIndex="6" Caption="Data Fim" Name="DATAFIM"
                                    FieldName="DATAFIM" Width="100px" Visible="true">
                                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">                                       
                                    </PropertiesDateEdit>
                                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                                    </CellStyle>
                                </dxwgv:GridViewDataDateColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
</asp:Content>
