<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ConsultaUnidade.aspx.cs" Inherits="Techne.Lyceum.Net.GestaoRede.ConsultaUnidade" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">

        function MarcaMapa() {
             var hdnMarcarMapa = document.getElementById("<%=hdnMarcarMapa.ClientID %>");
            hdnMarcarMapa.value = "MarcarMapa";
        };

        $(function() {
            $("#btnImprimir").click(function() {

                var nav = navigator.userAgent.toLowerCase();
                var printContent = document.getElementById("<%=divPrincipal.ClientID %>");
                var title = document.title;

                if (nav.indexOf("chrome") != -1) {
                    var frame1 = $('<iframe />');
                    frame1[0].name = "frame1";
                    frame1.css({ "position": "absolute", "top": "-1000000px" });
                    $("body").append(frame1);
                    var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
                    frameDoc.document.open();
                    frameDoc.document.write('<html><head><title>' + title + '</title>');
                    frameDoc.document.write('</head><body>');
                    frameDoc.document.write('<link href="../LyceumNet.css" rel="stylesheet" type="text/css" />');
                    frameDoc.document.write(printContent.innerHTML);
                    frameDoc.document.write('</body></html>');
                    frameDoc.document.close();
                    setTimeout(function() {
                        window.frames["frame1"].focus();
                        window.frames["frame1"].print();
                        frame1.remove();
                    }, 500);
                }
                else {

                    var windowUrl = 'about:blank';
                    var windowName = 'Impressão';
                    var printWindow = window.open(windowUrl, windowName, 'width=1850,height=800');

                    printWindow.document.write('<link href="../LyceumNet.css" rel="stylesheet" type="text/css" />');
                    printWindow.document.write(printContent.innerHTML);
                    printWindow.document.close();
                    printWindow.focus();
                    printWindow.print();
                    printWindow.close();
                }
            });
        });   
    </script>

    <script type='text/javascript'>
        var map;
        var searchManager;

        function MontaMapa() {
            var today = new Date();
            var h = today.getHours();
            var hdnMarcarMapa = document.getElementById("<%=hdnMarcarMapa.ClientID %>");
            var chave = ''
            if (h <= 15) {
                chave = '<%=ConfigurationManager.AppSettings["BingMapsKey.conexao.educacao.rj.gov.br"].ToString() %>'
            } else {
                chave = '<%=ConfigurationManager.AppSettings["BingMapsKeyApos15h.conexao.educacao.rj.gov.br"].ToString() %>'
            }

            map = new Microsoft.Maps.Map('#mapa', {
                credentials: chave
            });

            if (hdnMarcarMapa.value == "MarcarMapa") {
                ////Busca por endereço
                //geocodeQuery("Rua da Ajuda, Rio de Janeiro, Brazil");
                var txtLatitude = document.getElementById("<%=txtLatitude.ClientID %>");
                var txtLongitude = document.getElementById("<%=txtLongitude.ClientID %>");

                txtLatitude.value = '';
                txtLongitude.value = '';

                //Monta Endereço para busca
                var endereco = document.getElementById("<%=txtEndereco.ClientID %>");
                var Bairro = document.getElementById("<%=ddlBairro.ClientID %>");
                var municipio = document.getElementById("<%=txtMunicipioFisica.ClientID %>");
                var uf = document.getElementById("<%=txtEstado.ClientID %>");

                //Rua Soldado Genaro Pedro Lima, Anil, Rio de Janeiro - RJ, 22765-280, Brasil
                var enderecoCompleto = endereco.value + ', ' + Bairro.value + ', ' + municipio.value + ' - ' + uf.value + ', Brasil';

                if (endereco.value != null && endereco.value != ''
                && municipio.value != null && municipio.value != '') {
                    //Busca por endereço
                    geocodeQuery(enderecoCompleto);
                }
            }

            function geocodeQuery(query) {

                //If search manager is not defined, load the search module.
                if (!searchManager) {
                    //Create an instance of the search manager and call the geocodeQuery function again.
                    Microsoft.Maps.loadModule('Microsoft.Maps.Search', function() {
                        searchManager = new Microsoft.Maps.Search.SearchManager(map);
                        geocodeQuery(query);
                    });
                } else {
                    var searchRequest = {
                        where: query,
                        callback: function(r) {
                            //Add the first result to the map and zoom into it.
                            if (r && r.results && r.results.length > 0) {
                                var Events = Microsoft.Maps.Events;
                                var localizacao = r.results[0].location;

                                var pin = new Microsoft.Maps.Pushpin(localizacao, { icon: '../Images/PinVermelho.png', color: 'Red', draggable: true });
                                map.entities.push(pin);
                                map.setView({ bounds: r.results[0].bestView });

                                PreencherLatitudeLongitude(localizacao);

                                // Adicionado evento para o pin
                                Events.addHandler(pin, 'dragend', function() { displayPinCoordinates('pushpinDragEnd'); });

                                function displayPinCoordinates(id) {
                                    var pin_location = pin.getLocation();
                                    PreencherLatitudeLongitude(pin_location);
                                    alert("Coordenadas atualizadas, para voltar a posição de inicial do endereço clique novamente no botão Obter Coordenadas endereço.");
                                }
                            }
                        },
                        errorCallback: function(e) {
                            //If there is an error, alert the user about it.
                            alert("Coordenadas não encontradas.");
                        }
                    };
                    //Make the geocode request.
                    searchManager.geocode(searchRequest);
                }

                var hdnMarcarMapa = document.getElementById("<%=hdnMarcarMapa.ClientID %>");
                hdnMarcarMapa.value = "";
            }

            function PreencherLatitudeLongitude(item) {
                var txtLatitude = document.getElementById("<%=txtLatitude.ClientID %>");
                var txtLongitude = document.getElementById("<%=txtLongitude.ClientID %>");

                txtLatitude.value = item.latitude;
                txtLongitude.value = item.longitude;
            }
        }
    </script>

    <script type='text/javascript' src='http://www.bing.com/api/maps/mapcontrol?callback=MontaMapa'
        async defer></script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por unidade"
        Width="617px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblUnidade" runat="server" Text="Unidade:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidade" runat="server" Key="unidade_ens" Argument="nome_comp"
                        OnChanged="tseUnidade_Changed" MaxLength="8" SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao,municipio,nome, regional, ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO_REGIONAL"
                        GridWidth="850px" SqlOrder="nome_comp">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome_comp" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="8%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="10%" />							
                            <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="13%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel runat="server" ID="pnlImprimir" Visible="false">
        <div style="width: 65%; float: right;">
            <table>
                <tr>
                    <td>
                        <input type="button" id="btnImprimir" style="background-image: url(../Images/bot_imprimir.png);
                            width: 100px; height: 27px; background-color: transparent!important;" />
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <div class="divEditBlock" style="width: 850px;">
        <asp:Label runat="server" ID="Label8" Text="Unidade" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableClientScript="true"
            ShowMessageBox="true" ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:HiddenField runat="server" ID="hdnCodMunicipioFisica" />
    <br />
    <div id="divPrincipal" visible="false" runat="server">
        <asp:Panel ID="Panel4" runat="server" GroupingText="Dados Gerais" Width="900px">
            <table>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label2" runat="server" Text="Código:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtUnidadeFisica" runat="server" MaxLength="8" Width="180px" SkinID="numerico"
                            Enabled="false" ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblNomeCompleto" runat="server" Text="Nome Completo:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtNomeComp" runat="server" MaxLength="100" Width="580px" onkeypress="return nomeSemNum(event);" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblFormaOcup" runat="server" Text="Localização da U.E.*: " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblLocalizacaoUF" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="RURAL">Rural</asp:ListItem>
                            <asp:ListItem Value="URBANA">Urbana</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td style="text-align: right">
                        <asp:Label ID="lblLocalFuncionamento" runat="server" Text="Local de Funcionamento:* "
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlLocalFuncionamento" runat="server" DataTextField="descr"
                            DataValueField="item" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlLocalFuncionamento_OnSelectedIndexChanged" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblLocalizacaoDiferenciada" runat="server" Text="Localização Diferenciada: * "
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <table>
                            <tr>
                                <td>
                                    <dxe:ASPxCheckBox AutoPostBack="true" ID="chkNaoSeAplica" ValueChecked="S" ValueUnchecked="N"
                                        ValueType="System.String" runat="server" Checked="true" Text="Não se aplica"
                                        OnCheckedChanged="chkNaoSeAplica_CheckedChanged">
                                    </dxe:ASPxCheckBox>
                                    <dxe:ASPxCheckBox ID="chkTerraIndigena" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                        runat="server" Checked="false" Text="Terra indígena">
                                    </dxe:ASPxCheckBox>
                                </td>
                                <td>
                                    <dxe:ASPxCheckBox ID="chkQuilombos" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                        runat="server" Checked="false" Text="Área remanescente de quilombos ">
                                    </dxe:ASPxCheckBox>
                                    <dxe:ASPxCheckBox ID="chkAreaAssentamento" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                        runat="server" Checked="false" Text="Área de assentamento">
                                    </dxe:ASPxCheckBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblTipo" runat="server" Text="Forma de Ocupação:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTipo" runat="server" DataTextField="descr" DataValueField="item"
                            Width="200" />
                    </td>
                    <td style="text-align: right">
                        <asp:Label ID="lblOcupacaoFormal" runat="server" Text="Ocupação com Documento Formal: "></asp:Label>
                    </td>
                    <td>
                        <dxe:ASPxCheckBox ID="chkOcupacaoFormal" runat="server" ValueChecked="S" ValueUnchecked="N"
                            ValueType="System.String" Checked="false">
                        </dxe:ASPxCheckBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label3" runat="server" Text="Imóvel Compartilhado: "></asp:Label>
                    </td>
                    <td>
                        <dxe:ASPxCheckBox ID="chkImovelCompartilhadoFisica" ValueChecked="S" ValueUnchecked="N"
                            ValueType="System.String" runat="server" Checked="false" Enabled="false">
                        </dxe:ASPxCheckBox>
                    </td>
                    <td style="text-align: right">
                        <asp:Label ID="lblExtraclasse" runat="server" Text="Controla extra-classe?:"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkExtraclasse" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblFormaAcesso" runat="server" Text="Possui Acessibilidade ao Portador de Necessidade Especial:* "
                                        SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rblAcessoNecEspecial" AutoPostBack="true" OnSelectedIndexChanged="rblAcessoNecEspecial_SelectedIndexChanged"
                                        runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="S">Sim</asp:ListItem>
                                        <asp:ListItem Value="N">Não</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Panel ID="pnlAcessibilidade" Visible="false" runat="server" GroupingText="Recursos de acessibilidade para pessoas com deficiência ou mobilidade reduzida nas vias de circulação internas da escola">
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:CheckBoxList ID="chkRecursoAcessibilidade" runat="server" RepeatDirection="Vertical"
                                            RepeatColumns="2" CellSpacing="5" Style="text-align: left" Width="100%">
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblAcessoDificil" runat="server" Text="Difícil Acesso: "></asp:Label>
                    </td>
                    <td>
                        <dxe:ASPxCheckBox ID="chkAcessoDificil" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                            runat="server" Checked="false">
                        </dxe:ASPxCheckBox>
                    </td>
                    <td style="text-align: right">
                        <asp:Label ID="lblDependenciaAdministrativa" runat="server" Text="Dependência Administrativa:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDependenciaAdministrativa" runat="server" DataTextField="descr"
                            Width="200px" DataValueField="item">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblEmail" runat="server" Text="Email:"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtEmail" runat="server" Width="580px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblCGC" runat="server" Text="CNPJ:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCGC" Width="200px" runat="server" onkeyup="formataCNPJ(this,event)"></asp:TextBox>
                    </td>
                    <td style="text-align: right;">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label10" runat="server" Text="Fax:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFax" Width="200px" onkeyup="formataTelefoneDDD(this,event)" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblFone" runat="server" Text="Telefone 1:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFone" Width="200px" onkeyup="formataTelefoneDDD(this,event)"
                            runat="server" />
                    </td>
                    <td style="text-align: right;">
                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblFone2" runat="server"
                            Text="Telefone 2:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFone2" Width="200px" onkeyup="formataTelefoneDDD(this,event)"
                            runat="server" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnEndereco" runat="server" GroupingText="Endereço" Width="900px">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblCEP" runat="server" Text="CEP:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="5">
                        <asp:TextBox ID="txtCEP" runat="server" MaxLength="8" SkinID="numerico">
                        </asp:TextBox>
                        <tweb:TSearch ID="tsCEP" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryBuscarCEP"
                            Modal="true" SkinID="CEP" MaxLength="8" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblMunicipio" runat="server" Text="Município:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtMunicipioFisica" runat="server" MaxLength="20" Width="380px"></asp:TextBox>
                    </td>
                    <td style="text-align: right">
                        <asp:Label ID="lblUF" runat="server" Text="UF: "></asp:Label>
                    </td>
                    <td colspan="3">
                        <input id="txtEstado" runat="server" maxlength="20" class="txtInput" readonly="readonly" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblEndereco" runat="server" Text="Endereço:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEndereco" runat="server" MaxLength="50" Width="380px" />
                    </td>
                    <td style="text-align: right">
                        <asp:Label ID="lblEnd_Num" runat="server" Text="Número:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtEnd_Num" runat="server" MaxLength="15" SkinID="numerico">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblEnd_Compl" runat="server" Text="Complemento:"></asp:Label>
                    </td>
                    <td colspan="5">
                        <asp:TextBox ID="txtEnd_Compl" runat="server" MaxLength="50" Width="380px" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblBairro" runat="server" Text="Bairro:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="5">
                        <asp:DropDownList ID="ddlBairro" runat="server" DataTextField="DESCRICAO" DataValueField="CODIGO"
                            Width="300" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label4" runat="server" Text="Distrito: "></asp:Label>
                    </td>
                    <td colspan="5">
                        <asp:DropDownList ID="ddlDistritoFisica" runat="server" DataTextField="distrito"
                            DataValueField="id_distrito" Width="300" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label9" runat="server" Text="Regional:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="5">
                        <asp:TextBox ID="txtRegional" runat="server" MaxLength="50" Width="380px" Enabled="false" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlLocalizacao" runat="server" GroupingText="Localização" Width="900px">
            <table width="100%">
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblLatitude" runat="server" Text="Latitude:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtLatitude" runat="server" Width="130px" MaxLength="50" />
                    </td>
                    <td style="text-align: right">
                        <asp:Label ID="lblLongitude" runat="server" Text="Longitude:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtLongitude" runat="server" Width="130px" MaxLength="50" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6" align="center">
                        <div id="mapa" style="width: 500px; height: 400px">
                        </div>
                        <br />
                        <asp:HiddenField ID="hdnMarcarMapa" runat="server" />
                        <asp:Button ID="btnEncontraNoMapa" runat="server" Text="Marcar no mapa" OnClientClick="MarcaMapa();" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlQuantitativos" runat="server" Width="900px" GroupingText="Quantitativos">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label1" runat="server" Text="Total de Alunos:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAlunos" runat="server" MaxLength="50" Width="180px" Enabled="false" />
                    </td>
                    <td style="text-align: right">
                        <asp:Label ID="Label5" runat="server" Text="Total de Professores:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtProfessores" runat="server" MaxLength="50" Width="180px" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label6" runat="server" Text="Total de Turmas:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTurmas" runat="server" MaxLength="50" Width="180px" Enabled="false" />
                    </td>
                    <td style="text-align: right">
                        <asp:Label ID="Label7" runat="server" Text="Turnos com turmas ativas:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTurnos" runat="server" MaxLength="50" Width="180px" Enabled="false" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:ObjectDataSource ID="odsDiretor" runat="server" TypeName="Techne.Lyceum.RN.UnidadeEnsino"
            SelectMethod="ConsultarGratificada">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtUnidadeFisica" Name="unidadeEns" PropertyName="Text" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <dxwgv:ASPxGridView ID="grdDiretor" DataSourceID="odsDiretor" runat="server" AutoGenerateColumns="False"
            ClientInstanceName="grdDiretor" EnableCallBacks="false" Font-Size="Small" Width="900px"
            OnCustomColumnDisplayText="grdDiretor_CustomColumnDisplayText">
            <SettingsBehavior AllowMultiSelection="False" AllowSort="False" />
            <SettingsPager PageSize="10" />
            <Columns>
                <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="matricula" VisibleIndex="1">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="nome_compl" VisibleIndex="2">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Função" FieldName="descricao" VisibleIndex="3">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Telefone" FieldName="fone" VisibleIndex="4">
                    <PropertiesTextEdit MaxLength="10">
                        <MaskSettings IncludeLiterals="None" Mask="(00)0000-0000" ErrorText="* Favor inserir um número de telefone válido." />
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Celular" FieldName="celular" VisibleIndex="5">
                    <PropertiesTextEdit MaxLength="10">
                        <MaskSettings IncludeLiterals="All" Mask="(00)0000-0000" ErrorText="* Favor inserir um número de telefone válido." />
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="E-mail" FieldName="e_mail" VisibleIndex="6">
                </dxwgv:GridViewDataTextColumn>
            </Columns>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        </dxwgv:ASPxGridView>
        <br />
        <asp:Panel ID="pnlEquipamentos" runat="server" GroupingText="Equipamentos Unidade"
            Enabled="false" Width="900px">
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlInternet" runat="server" Width="900px" Enabled="false">
            <asp:Panel ID="Panel6" runat="server" GroupingText="Possui Internet Banda Larga?">
                <table>
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="rblInternetBandaLarga" AutoPostBack="true" runat="server"
                                Enabled="false" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblInternetBandaLarga_SelectedIndexChanged">
                                <asp:ListItem Value="S">Sim</asp:ListItem>
                                <asp:ListItem Value="N">Não</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlDadosInternet" runat="server" Visible="false" Enabled="false" Width="900px">
                <table>
                    <tr>
                        <td>
                            <asp:Panel ID="Panel7" runat="server" GroupingText="Acesso à internet">
                                <asp:CheckBoxList ID="chkAcessoInternet" OnSelectedIndexChanged="chkAcessoInternet_SelectedIndexChanged"
                                    AutoPostBack="true" RepeatColumns="2" CellSpacing="2" runat="server" RepeatDirection="vertical"
                                    Enabled="false" Style="text-align: left">
                                </asp:CheckBoxList>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="Panel8" runat="server" GroupingText="Equipamentos que os alunos usam para acessar a Internet da escola"
                    Enabled="false" Width="900px">
                    <table>
                        <tr>
                            <td style="text-align: right">
                                <dxe:ASPxCheckBox ID="chkEquipamentoEscola" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                    runat="server" Checked="true" Text="Computadores de mesa, portáteis e tablets da escola (laboratório de informática, biblioteca, sala de aula, etc)">
                                </dxe:ASPxCheckBox>
                                <dxe:ASPxCheckBox ID="chkEquipamentoPessoal" ValueChecked="S" ValueUnchecked="N"
                                    ValueType="System.String" runat="server" Checked="false" Text="Dispositivos pessoais (computadores portáteis, celulares, tablets, etc)">
                                </dxe:ASPxCheckBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="Panel9" runat="server" GroupingText="Rede Local de interligação de computadores"
                    Enabled="false" Width="900px">
                    <table>
                        <tr>
                            <td style="text-align: right">
                                <dxe:ASPxCheckBox ID="chkRedeCabo" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                    runat="server" Checked="true" Text="A Cabo">
                                </dxe:ASPxCheckBox>
                            </td>
                            <td>
                                <dxe:ASPxCheckBox ID="chkRedeWireless" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                    runat="server" Checked="false" Text="Wireless">
                                </dxe:ASPxCheckBox>
                            </td>
                            <td>
                                <dxe:ASPxCheckBox ID="chkSemRedeComputador" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                    AutoPostBack="true" runat="server" Checked="false" Text="Não há rede local interligando computadores"
                                    OnCheckedChanged="chkSemRedeComputador_CheckedChanged">
                                </dxe:ASPxCheckBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
            <br />
        </asp:Panel>
        <asp:Panel ID="pnlDemaisDependenciasG3A1" runat="server" GroupingText="Relatório Inspeção Escolar"
            Width="900px">
            <div id="dDemaisDependnenciasG3A1">
                <asp:Repeater ID="rpdemaisdependenciasGrupoG3A1" runat="server">
                    <ItemTemplate>
                        <br>
                        <%--Grupo--%>
                        <asp:Label ID="lblGrupoDemaisG3A1" runat="server" Text='<%# Eval("DESCRICAO") %>'
                            Style="font-weight: bold; color: #000000;"></asp:Label>
                        <br>
                        <asp:HiddenField ID="hdnGrupoIdG3A1" runat="server" Value='<%# Eval("GrupoId")%>' />
                        <br>
                        <asp:Repeater ID="rpdemaisdependenciasAssuntoG3A1" runat="Server" OnItemDataBound="rpdemaisdependenciasAssuntoG3A1_ItemDataBound"
                            DataSource='<%#Eval("ListaAssunto")%>'>
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td>
                                            <%-- Assunto--%>
                                            <asp:Label ID="lblAssuntoDemaisG3A1" runat="server" Text='<%# Eval("DESCRICAO") %>'
                                                Style="font-weight: bold; color: #000000;"></asp:Label>
                                            <asp:HiddenField ID="hdnAssuntoIdDemaisG3A1" Value='<%# Eval("AssuntoId") + "&" + Eval("TipoAssuntoId")+ "&" + Eval("AcaodeDirecao") %>'
                                                runat="server" />
                                            <asp:TextBox ID="txtRespostaDemaisG3A1" runat="server" Width="150px" Enabled="false"></asp:TextBox>
                                            <asp:RadioButtonList runat="server" ID="rdGrupoDemaisG3A1" DataSource='<%#Eval("ListaOpcao")%>'
                                                Enabled="false" DataValueField="OpcoesAssuntoId" DataTextField="Descricao" RepeatDirection="Horizontal">
                                            </asp:RadioButtonList>
                                            <asp:CheckBoxList ID="chkRespostaDemaisG3A1" runat="server" DataTextField="Descricao"
                                                Enabled="false" DataValueField="valor" DataSource='<%#Eval("ListaOpcao")%>'>
                                            </asp:CheckBoxList>
                                            <tr>
                                                <asp:Repeater ID="rpAcaodeDirecaoG3A1" runat="Server" OnItemDataBound="rpAcaodeDirecaoG3A1_ItemDataBound">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblAcaoDirecaoDemaisG3A1" runat="server" Text='<%#Eval("descricao")%>'></asp:Label>
                                                            </td>
                                                            <td style="width: 135px;">
                                                                <asp:HiddenField ID="hdnOpAssuntoIdDemaisG3A1" runat="server" Value='<%# Eval("OpcoesAssuntoId") %>' />
                                                                <asp:DropDownList ID="ddlPerguntaDemaisG3A1" DataSourceID="OdsAcaodeDirecao" DataTextField="DESCRICAO"
                                                                    DataValueField="CODIGO" runat="server" Enabled="false">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </tr>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <asp:Repeater ID="rpdemaisdependenciasGrupoG3A2" runat="server">
                <ItemTemplate>
                    <br>
                    <%--Grupo--%>
                    <asp:Label ID="lblGrupoDemaisG3A2" runat="server" Text='<%# Eval("DESCRICAO") %>'
                        Style="font-weight: bold; color: #000000;"></asp:Label>
                    <br>
                    <asp:HiddenField ID="hdnGrupoIdG3A2" runat="server" Value='<%# Eval("GrupoId")%>' />
                    <br>
                    <asp:Repeater ID="rpdemaisdependenciasAssuntoG3A2" runat="Server" OnItemDataBound="rpdemaisdependenciasAssuntoG3A2_ItemDataBound"
                        DataSource='<%#Eval("ListaAssunto")%>'>
                        <ItemTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <%-- Assunto--%>
                                        <asp:Label ID="lblAssuntoDemaisG3A2" runat="server" Text='<%# Eval("DESCRICAO") %>'
                                            Style="font-weight: bold; color: #000000;"></asp:Label>
                                        <asp:HiddenField ID="hdnAssuntoIdDemaisG3A2" Value='<%# Eval("AssuntoId") + "&" + Eval("TipoAssuntoId")+ "&" + Eval("AcaodeDirecao") %>'
                                            runat="server" />
                                        <asp:TextBox ID="txtRespostaDemaisG3A2" runat="server" MaxLength="500" Width="150px"
                                            Enabled="false"></asp:TextBox>
                                        <asp:RadioButtonList runat="server" ID="rdGrupoDemaisG3A2" DataSource='<%#Eval("ListaOpcao")%>'
                                            Enabled="false" DataValueField="OpcoesAssuntoId" DataTextField="Descricao" RepeatDirection="Horizontal">
                                        </asp:RadioButtonList>
                                        <asp:CheckBoxList ID="chkRespostaDemaisG3A2" runat="server" DataTextField="Descricao"
                                            Enabled="false" DataValueField="valor" DataSource='<%#Eval("ListaOpcao")%>'>
                                        </asp:CheckBoxList>
                                        <tr>
                                            <asp:Repeater ID="rpAcaodeDirecaoG3A2" runat="Server" OnItemDataBound="rpAcaodeDirecaoG3A2_ItemDataBound">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblAcaoDirecaoDemaisG3A2" runat="server" Text='<%#Eval("descricao")%>'></asp:Label>
                                                        </td>
                                                        <td style="width: 135px;">
                                                            <asp:HiddenField ID="hdnOpAssuntoIdDemaisG3A2" runat="server" Value='<%# Eval("OpcoesAssuntoId") %>' />
                                                            <asp:DropDownList ID="ddlPerguntaDemaisG3A2" DataSourceID="OdsAcaodeDirecao" DataTextField="DESCRICAO"
                                                                DataValueField="CODIGO" runat="server" Enabled="false">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tr>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:Repeater>
                </ItemTemplate>
            </asp:Repeater>
        </asp:Panel>
    </div>
</asp:Content>
