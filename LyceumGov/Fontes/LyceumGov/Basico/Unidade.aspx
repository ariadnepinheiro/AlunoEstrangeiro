<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Unidade.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.Unidade" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function abrirPopupDocCelebrado() {

            window.setTimeout(function() {
                pucConfirmarDocCelebrado.Show();
            }, 1000);
        }

        function OnEscolaridadeChanged() {
            var descricao = cboEscolaridade.GetText().toString().split('|');

            txtNivel.SetText(descricao[2]);
            txtModalidade.SetText(descricao[1]);
            txtCodigo.SetText(cboEscolaridade.GetValue().toString());
        }

        function OnEdificacao2Changed_banheiro(cmbEdificacao) {
            grdBanheiroeVestiario.GetEditor("pavimento").PerformCallback(cmbEdificacao.GetValue().toString());
        }
        function OnEdificacao2Changed(cmbEdificacao) {
            grdDemaisDependencias.GetEditor("pavimento").PerformCallback(cmbEdificacao.GetValue().toString());
        }

        function OnEdificacaoChanged(cmbEdificacao) {
            grdDependencias.GetEditor("pavimento").PerformCallback(cmbEdificacao.GetValue().toString());
        }

        function OnModalidadeEscolaridadeChanged() {
            grdCompartilhadaOferta.GetEditor("curso").PerformCallback();
        }

        function OnRedeEnsinoChanged(cmbRedeEnsino) {
            var redeEnsino = cmbRedeEnsino.GetValue().toString();

            grdCompartilhada.GetEditor("nome").SetText("");
            grdCompartilhada.GetEditor("censo_compartilhada").SetText("");

            LoadAutoComplete(grdCompartilhada.GetEditor("censo_compartilhada"));

            if (redeEnsino == "Estadual") {
                $(grdCompartilhada.GetEditor("censo_compartilhada").GetInputElement()).autocomplete({ disabled: false });

                grdCompartilhada.GetEditor("nome").SetEnabled(false);
            }
            else {
                $(grdCompartilhada.GetEditor("censo_compartilhada").GetInputElement()).autocomplete({ disabled: true });

                grdCompartilhada.GetEditor("nome").SetEnabled(true);
            }
        }

        function ConfirmaValidacao() {
            if (confirm("Esta operação VALIDA as dependencias da unidade física. Deseja continuar?")) {
                return true;
            }
            return false;
        }

        function ConfirmaNaoValidacao() {
            if (confirm("Esta operação NÃO VALIDA as dependencias da unidade física. Deseja continuar?")) {
                return true;
            }
            return false;
        }

        function ConfirmaReaberturaDependencia() {
            if (confirm("Esta operação REABRIRÁ as dependencias da unidade física para alteração. Deseja continuar?")) {
                return true;
            }
            return false;
        }
        function abrirPopup() {
            window.setTimeout(function() {
                pucConfirmarEquipamentos.Show();
            }, 1000);
        }
        // No primeiro <script>, dentro do $(document).ready ou em função já existente:
        function OnEndCallbacks(s, e) {
            if (s.cpAtualizar != null) {
                $('#' + '<%= ddlSitFuncionamento.ClientID %>').val(s.cpAtualizar);
                s.cpAtualizar = null;
            }
            // Força o Leaflet a recalcular o tamanho após callbacks
            if (map) {
                setTimeout(function() { map.invalidateSize(); }, 200);
            }
        }

        $(document).ready(function() {
            setTimeout(function() { MontaMapa(); }, 300);
        });

        $(document).ready(function() {
            preencherDadosPorCEP2({ tscep: '<%=tseCEPUF.ClientID %>',
                cep: '<%=txtCEPUF.ClientID %>',
                nomeLogradouro: '<%=txtLogradouroUF.ClientID %>',
                nomeMunicipio: '<%=txtMunicipio.ClientID %>',
                codigoMunicipio: '<%=hdnCodMunicipio.ClientID %>',
                uf: '<%=txtEstadoUF.ClientID %>',
                numero: '<%=txtNumeroEndUF.ClientID %>',
                bairro: '<%=ddlBairroUF.ClientID %>'
            });

            $("#<%= this.txtLogradouroUF.ClientID %>").attr("readonly", "readonly");
            $("#<%= this.txtMunicipio.ClientID %>").attr("readonly", "readonly");

            preencherDadosPorCEP2({ tscep: '<%=tsCEP.ClientID %>',
                cep: '<%=txtCEP.ClientID %>',
                nomeLogradouro: '<%=txtEndereco.ClientID %>',
                nomeMunicipio: '<%=txtMunicipioFisica.ClientID %>',
                codigoMunicipio: '<%=hdnCodMunicipioFisica.ClientID %>',
                uf: '<%=txtEstado.ClientID %>',
                numero: '<%=txtEnd_Num.ClientID %>',
                bairro: '<%=ddlBairro.ClientID %>'
            });

            $("#<%= this.txtEndereco.ClientID %>").attr("readonly", "readonly");
            $("#<%= this.txtMunicipioFisica.ClientID %>").attr("readonly", "readonly");

            $("#<%= this.txtNumeroEndUF.ClientID %>").on("change", function() {
                if ($(this).val() != "") {
                    __doPostBack('<%= this.txtNumeroEndUF.UniqueID %>', 'Change');
                }
            });

            $("#<%= this.txtEnd_Num.ClientID %>").on("change", function() {
                if ($(this).val() != "") {
                    __doPostBack('<%= this.txtEnd_Num.UniqueID %>', 'Change');
                }
            });
        });        
    </script>

    <%-- Leaflet.js - OpenStreetMap --%>
    <link rel="stylesheet" href="https://unpkg.com/leaflet@1.9.4/dist/leaflet.css" />
    <script src="https://unpkg.com/leaflet@1.9.4/dist/leaflet.js"></script>

    <script type="text/javascript">
        var map;
        var pin;

        function MontaMapa() {
            // ← USA ClientID corretamente para funcionar com MasterPage
            var txtLatitude  = document.getElementById('<%= txtLatitude.ClientID %>');
            var txtLongitude = document.getElementById('<%= txtLongitude.ClientID %>');

            var lat = txtLatitude  ? parseFloat(txtLatitude.value)  : NaN;
            var lng = txtLongitude ? parseFloat(txtLongitude.value) : NaN;

            var centerLat = !isNaN(lat) ? lat : -22.9068;
            var centerLng = !isNaN(lng) ? lng : -43.1729;

            map = L.map('mapa').setView([centerLat, centerLng], 15);
            L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
                attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a>',
                maxZoom: 19
            }).addTo(map);

            // Se já tem coordenadas salvas, coloca o pin
            if (!isNaN(lat) && !isNaN(lng)) {
                AdicionarPin(lat, lng);
            }
        }

        function AdicionarPin(lat, lng) {
            if (pin) map.removeLayer(pin);
            pin = L.marker([lat, lng], { draggable: true }).addTo(map);
            map.setView([lat, lng], 15);
            pin.on('dragend', function (e) {
                var pos = e.target.getLatLng();
                PreencherLatitudeLongitude(pos.lat, pos.lng);
                alert('Coordenadas atualizadas. Para voltar à posição inicial do endereço, clique novamente no botão Obter Coordenadas.');
            });
        }

        function MarcaMapa() {
            var hdnMarcarMapa = document.getElementById('<%= hdnMarcarMapa.ClientID %>');
            hdnMarcarMapa.value = 'MarcarMapa';
            BuscarEndereco();
            return false;
        }

        function BuscarEndereco() {
            var logradouroUF = document.getElementById('<%= txtLogradouroUF.ClientID %>');
            var endereco, bairro, municipio, uf;

            if (logradouroUF && logradouroUF.value && logradouroUF.value.trim() !== '') {
                endereco  = logradouroUF;
                bairro    = document.getElementById('<%= ddlBairroUF.ClientID %>');
                municipio = document.getElementById('<%= txtMunicipio.ClientID %>');
                uf        = document.getElementById('<%= txtEstadoUF.ClientID %>');
            } else {
                endereco  = document.getElementById('<%= txtEndereco.ClientID %>');
                bairro    = document.getElementById('<%= ddlBairro.ClientID %>');
                municipio = document.getElementById('<%= txtMunicipioFisica.ClientID %>');
                uf        = document.getElementById('<%= txtEstado.ClientID %>');
            }

            if (!endereco || !endereco.value || !municipio || !municipio.value) return;

            var enderecoCompleto = endereco.value + ', ' + bairro.value + ', ' + municipio.value + ' - ' + uf.value + ', Brasil';
            var url = 'https://nominatim.openstreetmap.org/search?format=json&limit=1&q=' + encodeURIComponent(enderecoCompleto);

            fetch(url, { headers: { 'Accept-Language': 'pt-BR' } })
                .then(function (r) { return r.json(); })
                .then(function (data) {
                    if (data && data.length > 0) {
                        var lat = parseFloat(data[0].lat);
                        var lng = parseFloat(data[0].lon);
                        AdicionarPin(lat, lng);
                        PreencherLatitudeLongitude(lat, lng);
                    } else {
                        alert('Coordenadas não encontradas para o endereço informado.');
                    }
                })
                .catch(function () {
                    alert('Erro ao buscar coordenadas. Verifique sua conexão.');
                });
        }

        function PreencherLatitudeLongitude(lat, lng) {
            document.getElementById('<%= txtLatitude.ClientID %>').value  = lat;
            document.getElementById('<%= txtLongitude.ClientID %>').value = lng;
        }

        $(document).ready(function () {
            MontaMapa();
        });
    </script>

    <style type="text/css">
        .dxeListBox_Office2003_Blue .dxeHD
        {
            font-weight: bold;
        }
        .dxeListBox_Blue .dxeHD
        {
            font-weight: bold;
        }
    </style>
    
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por unidade"
        Width="617px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblUnidade" runat="server" Text="Unidade:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidade" runat="server" Key="unidade_ens" Argument="nome_comp"
                        OnChanged="tseUnidade_Changed" MaxLength="8" SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao,municipio,nome, regional,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO_REGIONAL"
                        GridWidth="850px" SqlOrder="nome_comp">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome_comp" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="8%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="8%" />
                            <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="13%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:HiddenField runat="server" ID="hdnCodMunicipio" Value="" />
    <asp:HiddenField runat="server" ID="hdnCodMunicipioFisica" />
    <asp:HiddenField runat="server" ID="hdnMunicipalizacao" />
    <asp:HiddenField ID="hdnCensoCompartilhada" runat="server" Value="" />
    <br />
    <div class="divEditBlock" style="width: 850px;">
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvarNovo" runat="server" SkinID="BcSalva" OnClick="btnSalvarNovo_Click" />
        <asp:Label runat="server" ID="lblBlocoUnidade" Text="Unidade" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsUnidade" runat="server" EnableClientScript="true" ShowMessageBox="true"
            ValidationGroup="SalvarForm" ShowSummary="false" Style="margin-top: 44px" />
    </div>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <dxtc:ASPxPageControl ID="pcUnidade" runat="server" ActiveTabIndex="0" Width="90%">
        <TabPages>
            <dxtc:TabPage Text="Informações Gerais">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl1" runat="server">
                        <asp:Panel ID="Panel5" runat="server" GroupingText="Dados Gerais" Width="900px">
                            <table width="100%">
                                <tr>
                                    <td style="text-align: right" width="150px;">
                                        <asp:Label ID="lblUnidadeEnsino" runat="server" Text="Código do Censo:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtUnidadeEnsino" runat="server" MaxLength="8" Width="150px" SkinID="numerico" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblSituacaoFuncionamento" runat="server" Text="Situação de funcionamento:*"
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlSitFuncionamento" runat="server" DataTextField="descr" DataValueField="item">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblNome_Comp" runat="server" Text="Nome da Unidade Ensino:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtNome_Comp" runat="server" MaxLength="100" Width="560px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblDiretoriaRegional" runat="server" Text="Diretoria Regional:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <tweb:TSearchBox ID="tseRegional" runat="server" SqlOrder="regional" SqlSelect=" select distinct  id_regional ,regional from tce_regional m "
                                            ArgumentColumns="50" Columns="10" MaxLength="10" DataType="Number"
                                            Key="id_regional" AutoPostBack="false">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                                                <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="60%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblNucleo" runat="server" Text="Coordenadoria:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <tweb:TSearchBox ID="tseNucleo" runat="server" Caption="" SqlSelect="SELECT nucleo, descricao FROM ly_nucleo"
                                            SqlOrder="CONVERT(int, nucleo)" MaxLength="5" AutoPostBack="false">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="nucleo" Width="20%" />
                                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblSetor" runat="server" Text="U.A.:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <tweb:TSearchBox ID="tseSetor" runat="server" Argument="nomesetor" Caption="" Key="ua_atual"
                                            SqlOrder="ua_atual" SqlSelect="SELECT setor, nomesetor,ua_atual, ua_antiga FROM hades..vw_setor"
                                            FieldName="ua_atual" ColumnName="ua_atual" MaxLength="15" DataType="Varchar"
                                            AutoPostBack="false" Connection="Hades">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                                                <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
                                                <tweb:TSearchBoxColumn Caption="Nome" FieldName="nomesetor" Width="80%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblClassificacao" runat="server" Text="Classificação:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlClassificacao" runat="server" DataTextField="descr" DataValueField="item">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblImovelCompartilhado" runat="server" Text="Imóvel Compartilhado: "></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxCheckBox ID="chkImovelCompartilhado" ValueChecked="S" ValueUnchecked="N"
                                            ValueType="System.String" runat="server" Checked="false">
                                        </dxe:ASPxCheckBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlEnderecoUF" runat="server" GroupingText="Endereço" Width="900px">
                            <table width="100%">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblCEPUF" runat="server" Text="CEP:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtCEPUF" runat="server" MaxLength="8" SkinID="numerico">
                                        </asp:TextBox>
                                        <tweb:TSearch ID="tseCEPUF" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryBuscarCEP" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblMunicipioUF" runat="server" Text="Município:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMunicipio" runat="server" MaxLength="20" Width="400px"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblEstadoUF" runat="server" Text="UF: "></asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtEstadoUF" runat="server" maxlength="20" class="txtInput" readonly="readonly" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblLogradouroUF" runat="server" Text="Endereço:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLogradouroUF" runat="server" MaxLength="50" Width="400px" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblNumeroEndUF" runat="server" Text="Número:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNumeroEndUF" AutoPostBack="false" runat="server" MaxLength="50"
                                            Width="40px" OnPreRender="txtNumeroEndUF_PreRender" onkeypress="return OnlyNumericEntry(event)" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblComplementoUF" runat="server" Text="Complemento:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtComplementoUF" runat="server" MaxLength="50" Width="400px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblBairroUF" runat="server" Text="Bairro:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddlBairroUF" runat="server" DataTextField="DESCRICAO" DataValueField="CODIGO"
                                            Width="300" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblDistritoUF" runat="server" Text="Distrito: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlDistrito" runat="server" DataTextField="distrito" DataValueField="id_distrito"
                                            Width="300" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="Panel2" runat="server" GroupingText="Localização" Width="900px">
                            <table width="100%">
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblLatitude" runat="server" Text="Latitude:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLatitude" runat="server" Width="100px" MaxLength="50" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblLongitude" runat="server" Text="Longitude:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtLongitude" runat="server" Width="100px" MaxLength="50" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="6" align="center">
                                        <div id="mapa" style="width: 500px; height: 400px">
                                        </div>
                                        <br />
                                        <asp:HiddenField ID="hdnMarcarMapa" runat="server" />
                                        <asp:Button ID="btnEncontraNoMapa" runat="server" Text="Obter coordenadas" OnClientClick="MarcaMapa(); return false;" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Button ID="btnSalvarInfoGerais" runat="server" ValidationGroup="SalvarForm"
                            Text="Salvar Informações Gerais" OnClick="btnSalvarInfoGerais_Click" Visible="false" />
                        <br />
                        <asp:ObjectDataSource ID="odsDiretor" runat="server" TypeName="Techne.Lyceum.RN.UnidadeEnsino"
                            SelectMethod="ConsultarGratificada">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="txtUnidadeEnsino" Name="unidadeEns" PropertyName="Text" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <dxwgv:ASPxGridView ID="grdDiretor" DataSourceID="odsDiretor" runat="server" AutoGenerateColumns="False"
                            ClientInstanceName="grdDiretor" EnableCallBacks="false" Font-Size="Small" Width="942px"
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
                        <asp:ObjectDataSource ID="odsAAGE" TypeName="Techne.Lyceum.RN.AAGE.DocenteAAGEUnidadeEnsino"
                            runat="server" SelectMethod="ListaDocentepor">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="txtUnidadeEnsino" Name="unidadeEnsinoId" PropertyName="Text" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <dxwgv:ASPxGridView ID="grdAAGE" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdAAGE"
                            DataSourceID="odsAAGE" Font-Size="Small" Width="942px" OnCustomColumnDisplayText="grdAAGE_CustomColumnDisplayText">
                            <SettingsBehavior AllowDragDrop="false" />
                            <Columns>
                                <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="MATRICULA" VisibleIndex="1">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NOME_COMPL" VisibleIndex="2">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Função" FieldName="DESCRICAO" VisibleIndex="3">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Telefone" FieldName="FONE" VisibleIndex="4">
                                    <PropertiesTextEdit MaxLength="10">
                                        <MaskSettings IncludeLiterals="None" Mask="(00)0000-0000" ErrorText="* Favor inserir um número de telefone válido." />
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Celular" FieldName="CELULAR" VisibleIndex="5">
                                    <PropertiesTextEdit MaxLength="10">
                                        <MaskSettings IncludeLiterals="All" Mask="(00)0000-0000" ErrorText="* Favor inserir um número de telefone válido." />
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="E-mail" FieldName="E_MAIL_INTERNO" VisibleIndex="6">
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                        </dxwgv:ASPxGridView>
                        <br />
                        <asp:ObjectDataSource ID="odsMediador" TypeName="Techne.Lyceum.RN.AAGE.DocenteMediadorUnidadeEnsino"
                            runat="server" SelectMethod="ListaDocentepor">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="txtUnidadeEnsino" Name="unidadeEnsinoId" PropertyName="Text" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <dxwgv:ASPxGridView ID="grdMediador" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdMediador"
                            DataSourceID="odsMediador" Font-Size="Small" Width="942px" OnCustomColumnDisplayText="grdMediador_CustomColumnDisplayText">
                            <SettingsBehavior AllowDragDrop="false" />
                            <Columns>
                                <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="MATRICULA" VisibleIndex="1">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NOME_COMPL" VisibleIndex="2">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Função" FieldName="DESCRICAO" VisibleIndex="3">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Telefone" FieldName="FONE" VisibleIndex="4">
                                    <PropertiesTextEdit MaxLength="10">
                                        <MaskSettings IncludeLiterals="None" Mask="(00)0000-0000" ErrorText="* Favor inserir um número de telefone válido." />
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Celular" FieldName="CELULAR" VisibleIndex="5">
                                    <PropertiesTextEdit MaxLength="10">
                                        <MaskSettings IncludeLiterals="All" Mask="(00)0000-0000" ErrorText="* Favor inserir um número de telefone válido." />
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="E-mail" FieldName="E_MAIL_INTERNO" VisibleIndex="6">
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                        </dxwgv:ASPxGridView>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Características Físicas/Localização">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl2" runat="server">
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
                                        <asp:TextBox ID="txtNomeComp" runat="server" MaxLength="100" Width="580px" />
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
                                                            OnSelectedIndexChanged="chkRecursoAcessibilidade_SelectedIndexChanged" AutoPostBack="true"
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
                                        <asp:TextBox ID="txtEnd_Num" runat="server" MaxLength="15" SkinID="numerico" OnPreRender="txtNumeroEnd_PreRender">
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
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlAreaTerreno" runat="server" GroupingText="Área do Terreno" Width="900px">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblAreaTotalTerreno" runat="server" Text="Área Total do Terreno (m²):"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAreaTotalTerreno" runat="server" Width="70px" MaxLength="15"
                                            SkinID="numerico" />
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblAreaConstruida" runat="server" Text="Área Construída Total (m²):"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAreaConstruida" runat="server" Width="70px" MaxLength="15" SkinID="numerico">
                                        </asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblAreaTerreno" runat="server" Text="Área do Terreno, livre, sem construção (m²): "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAreaTerreno" runat="server" MaxLength="10" SkinID="numerico"
                                            Width="70px">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Button ID="btnSalvarFisicas" runat="server" ValidationGroup="SalvarForm" Text="Salvar Características Físicas"
                            OnClick="btnSalvarFisicas_Click" Visible="false" />
                        <br />
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Salas de Aula">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl3" runat="server">
                        <asp:Panel ID="pnlEdificacaoPavimento" runat="server" GroupingText="Edificações e Pavimentos">
                            <asp:ObjectDataSource ID="odsEdifPav" TypeName="Techne.Lyceum.Net.Basico.Unidade"
                                SelectMethod="ListarEdifPav" runat="server" DeleteMethod="DeleteEdifPav" InsertMethod="InsertEdifPav"
                                OnDeleting="odsEdificacoesPavimentos_Deleting" OnInserting="odsEdificacoesPavimentos_Inserting"
                                OnUpdating="odsEdificacoesPavimentos_Updating" UpdateMethod="UpdateEdifPav">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="tseUnidade" DefaultValue="" Name="unidade" PropertyName="DBValue" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <dxwgv:ASPxGridView ID="grdEdificacoesPavimentos" runat="server" DataSourceID="odsEdifPav"
                                KeyFieldName="CompositeKey" ClientInstanceName="grdEdificacoesPavimentos" AutoGenerateColumns="false"
                                Width="850px" OnRowInserting="grdEdificacoesPavimentos_RowInserting" OnCellEditorInitialize="grdEdificacoesPavimentos_CellEditorInitialize"
                                OnCustomUnboundColumnData="grdEdificacoesPavimentos_CustomUnboundColumnData"
                                OnRowUpdating="grdEdificacoesPavimentos_RowUpdating" OnRowDeleting="grdEdificacoesPavimentos_RowDeleting">
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                                <SettingsBehavior ConfirmDelete="true" AllowMultiSelection="False" AllowDragDrop="false" />
                                <SettingsText ConfirmDelete="Confirma a remoção ?" EmptyDataRow="Não existem dados." />
                                <SettingsEditing Mode="Inline" />
                                <Columns>
                                    <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                                        <HeaderCaptionTemplate>
                                            <div style="text-align: center">
                                                <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                                    onclick="grdEdificacoesPavimentos.AddNewRow();" alt="Novo" />
                                            </div>
                                        </HeaderCaptionTemplate>
                                        <EditButton Text="Editar" Visible="true">
                                            <Image Url="~/img/bt_editar.png" />
                                        </EditButton>
                                        <DeleteButton Text="Remover" Visible="true">
                                            <Image Url="~/img/bt_exclui2.png" />
                                        </DeleteButton>
                                        <CancelButton Text="Cancelar">
                                            <Image Url="~/img/bt_cancelar.png" />
                                        </CancelButton>
                                        <UpdateButton Text="Atualizar">
                                            <Image Url="~/img/bt_salvar.png" />
                                        </UpdateButton>
                                        <ClearFilterButton Text="Limpar" Visible="true">
                                            <Image Url="~/img/bt_limpa.png" />
                                        </ClearFilterButton>
                                    </dxwgv:GridViewCommandColumn>
                                    <dxwgv:GridViewDataTextColumn VisibleIndex="1" Caption="Edificação*" FieldName="EDIFICACAO"
                                        Width="150px">
                                        <PropertiesTextEdit MaxLength="20">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                <RequiredField ErrorText="Favor informar a edificação" IsRequired="True" />
                                            </ValidationSettings>
                                        </PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn VisibleIndex="2" Caption="Nome da Edificação*" FieldName="NOME_EDIFICACAO"
                                        Width="250">
                                        <PropertiesTextEdit MaxLength="100">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                <RequiredField ErrorText="Favor informar a edificação" IsRequired="True" />
                                            </ValidationSettings>
                                        </PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn VisibleIndex="3" Caption="Pavimento*" FieldName="PAVIMENTO"
                                        Width="150">
                                        <PropertiesTextEdit MaxLength="20">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                <RequiredField ErrorText="Favor informar o pavimento" IsRequired="True" />
                                            </ValidationSettings>
                                        </PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn VisibleIndex="4" Caption="Nome do Pavimento*" FieldName="NOME_PAVIMENTO"
                                        Width="250">
                                        <PropertiesTextEdit MaxLength="100">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                <RequiredField ErrorText="Favor informar o nome do pavimento" IsRequired="True" />
                                            </ValidationSettings>
                                        </PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                                        Visible="False" VisibleIndex="8">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn FieldName="UNIDADE_FIS" VisibleIndex="9" Caption="Unidade Física"
                                        Visible="false">
                                    </dxwgv:GridViewDataTextColumn>
                                </Columns>
                            </dxwgv:ASPxGridView>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlSalasdeAula" runat="server" GroupingText="Salas de Aula">
                            <br />
                            <asp:Panel ID="pnlAtiva" runat="server" GroupingText="Filtro Dependência Ativa">
                                <table>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:CheckBox ID="chkAtivaDepen" runat="server" ValueChecked="S" ValueUnchecked="N"
                                                Checked="true" OnCheckedChanged="chkAtivaDepen_CheckedChanged" AutoPostBack="true"
                                                Text="Ativa?" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:ObjectDataSource ID="odsDependencias" TypeName="Techne.Lyceum.Net.Basico.Unidade"
                                SelectMethod="ListarDependencias" runat="server" DeleteMethod="Delete" UpdateMethod="Update"
                                InsertMethod="Insert" OnDeleting="odsDependencias_Deleting" OnInserting="odsDependencias_Inserting"
                                OnUpdating="odsDependencias_Updating">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="tseUnidade" DefaultValue="" Name="unidade" PropertyName="DBValue" />
                                    <asp:ControlParameter ControlID="chkAtivaDepen" Name="ativa" PropertyName="Checked" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:ObjectDataSource ID="odsEdificacoes" runat="server" TypeName="Techne.Lyceum.RN.UnidadeFisicaEdificacao"
                                SelectMethod="ConsultarEdificacoes">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="tseUnidade" PropertyName="Value" Name="unidade" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <dxwgv:ASPxGridView ID="grdDependencias" runat="server" DataSourceID="odsDependencias"
                                KeyFieldName="CompositeKey" AutoGenerateColumns="False" Width="1100px" ClientInstanceName="grdDependencias"
                                Font-Size="Small" OnAutoFilterCellEditorInitialize="grdDependencias_AutoFilterCellEditorCreate"
                                OnCellEditorInitialize="grdDependencias_CellEditorInitialize" OnCustomUnboundColumnData="grdDependencias_CustomUnboundColumnData"
                                OnRowDeleting="grdDependencias_RowDeleting" OnRowUpdating="grdDependencias_RowUpdating"
                                OnRowInserting="grdDependencias_RowInserting" OnInitNewRow="grdDependencias_InitNewRow"
                                OnStartRowEditing="grdDependencias_StartRowEditing" OnAfterPerformCallback="grdDependencias_AfterPerformCallback"
                                OnCustomColumnDisplayText="grdDependencias_CustomColumnDisplayText">
                                <SettingsPager>
                                    <Summary AllPagesText="Páginas: {0} - {1} ({2} items)" Text="Página {0} of {1} ({2} items)" />
                                </SettingsPager>
                                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                                <SettingsBehavior ConfirmDelete="true" AllowMultiSelection="False" AllowDragDrop="false" />
                                <SettingsText ConfirmDelete="Confirma a remoção ?" EmptyDataRow="Não existem dados." />
                                <SettingsEditing Mode="EditForm" />
                                <Templates>
                                    <EditForm>
                                        <dxw:ContentControl ID="cntTemplateDependencia" runat="server">
                                            <div style="padding: 4px;">
                                                <table>
                                                    <tr>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblTempDependencia" runat="server" Text="Dependência: " Width="150px">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <dxwgv:ASPxGridViewTemplateReplacement ID="TempDependencia" runat="server" ColumnID="dependencia"
                                                                ReplacementType="EditFormCellEditor">
                                                            </dxwgv:ASPxGridViewTemplateReplacement>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblTempDescricao" runat="server" Text="Descrição: " Width="150px">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <dxwgv:ASPxGridViewTemplateReplacement ID="TempDescricao" runat="server" ColumnID="descricao"
                                                                ReplacementType="EditFormCellEditor">
                                                            </dxwgv:ASPxGridViewTemplateReplacement>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblTempAtiva" runat="server" Text="Ativa? " Width="150px">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <dxwgv:ASPxGridViewTemplateReplacement ID="TempAtiva" runat="server" ColumnID="ativa"
                                                                ReplacementType="EditFormCellEditor">
                                                            </dxwgv:ASPxGridViewTemplateReplacement>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="Label12" runat="server" Text="Sala Anexa? " Width="150px">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <dxwgv:ASPxGridViewTemplateReplacement ID="ASPxGridViewTemplateReplacement1" runat="server"
                                                                ColumnID="sala_anexa" ReplacementType="EditFormCellEditor">
                                                            </dxwgv:ASPxGridViewTemplateReplacement>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblTempTipo" runat="server" Text="Tipo:* " SkinID="lblObrigatorio"
                                                                Width="150px">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <dxwgv:ASPxGridViewTemplateReplacement ID="TempTipo" runat="server" ColumnID="tipo_depend"
                                                                ReplacementType="EditFormCellEditor">
                                                            </dxwgv:ASPxGridViewTemplateReplacement>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblTempArea" runat="server" Text="Área (m²): " Width="150px">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <dxwgv:ASPxGridViewTemplateReplacement ID="TempArea" runat="server" ColumnID="area"
                                                                ReplacementType="EditFormCellEditor">
                                                            </dxwgv:ASPxGridViewTemplateReplacement>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right;">
                                                            <asp:Label ID="Label1" runat="server" Text="Edificação: *" SkinID="lblObrigatorio"></asp:Label>
                                                        </td>
                                                        <td colspan="3">
                                                            <dxwgv:ASPxGridViewTemplateReplacement ID="TempEdificacao" runat="server" ColumnID="edificacao"
                                                                ReplacementType="EditFormCellEditor">
                                                            </dxwgv:ASPxGridViewTemplateReplacement>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right;">
                                                            <asp:Label ID="lblTempPavimento" runat="server" Text="Pavimento: *" SkinID="lblObrigatorio"></asp:Label>
                                                        </td>
                                                        <td colspan="3">
                                                            <dxwgv:ASPxGridViewTemplateReplacement ID="TempPavimento" runat="server" ColumnID="pavimento"
                                                                ReplacementType="EditFormCellEditor">
                                                            </dxwgv:ASPxGridViewTemplateReplacement>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblTempObservacao" runat="server" Text="Observação: " Width="150px">
                                                            </asp:Label>
                                                        </td>
                                                        <td>
                                                            <dxwgv:ASPxGridViewTemplateReplacement ID="TempObservacao" runat="server" ColumnID="obs"
                                                                ReplacementType="EditFormCellEditor">
                                                            </dxwgv:ASPxGridViewTemplateReplacement>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <dxwgv:ASPxGridViewTemplateReplacement ID="repUpdate" runat="server" ReplacementType="EditFormUpdateButton">
                                                </dxwgv:ASPxGridViewTemplateReplacement>
                                                <dxwgv:ASPxGridViewTemplateReplacement ID="repCancel" runat="server" ReplacementType="EditFormCancelButton">
                                                </dxwgv:ASPxGridViewTemplateReplacement>
                                            </div>
                                        </dxw:ContentControl>
                                    </EditForm>
                                </Templates>
                                <Columns>
                                    <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="50px">
                                        <HeaderCaptionTemplate>
                                            <div style="text-align: center">
                                                <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                                    onclick="grdDependencias.AddNewRow();" alt="Novo" />
                                            </div>
                                        </HeaderCaptionTemplate>
                                        <EditButton Text="Editar" Visible="True">
                                            <Image Url="~/img/bt_editar.png" />
                                        </EditButton>
                                        <DeleteButton Text="Remover" Visible="True">
                                            <Image Url="~/img/bt_exclui2.png" />
                                        </DeleteButton>
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
                                    <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="faculdade" VisibleIndex="1"
                                        Visible="false">
                                        <PropertiesTextEdit MaxLength="20" Width="150px">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                <RequiredField ErrorText="Favor entre com a Faculdade" IsRequired="True" />
                                            </ValidationSettings>
                                        </PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Dependência*" FieldName="dependencia" VisibleIndex="2"
                                        Width="100px" HeaderStyle-Font-Bold="true">
                                        <PropertiesTextEdit MaxLength="20" Width="150px">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                <RequiredField ErrorText="Favor entre com a Dependência" IsRequired="True" />
                                            </ValidationSettings>
                                        </PropertiesTextEdit>
                                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Descrição" FieldName="descricao" VisibleIndex="3"
                                        Width="150px">
                                        <PropertiesTextEdit MaxLength="255" Width="300px">
                                        </PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataCheckColumn Caption="Ativa?" FieldName="ativa" VisibleIndex="4"
                                        Width="50px" HeaderStyle-Font-Bold="true">
                                        <PropertiesCheckEdit ValueChecked="S" ValueUnchecked="N" ValueType="System.String">
                                        </PropertiesCheckEdit>
                                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                                    </dxwgv:GridViewDataCheckColumn>
                                    <dxwgv:GridViewDataCheckColumn Caption="Sala Anexa?" FieldName="sala_anexa" VisibleIndex="4"
                                        Width="50px" HeaderStyle-Font-Bold="true">
                                        <PropertiesCheckEdit ValueChecked="S" ValueUnchecked="N" ValueType="System.String">
                                        </PropertiesCheckEdit>
                                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                                    </dxwgv:GridViewDataCheckColumn>
                                    <dxwgv:GridViewDataComboBoxColumn Caption="Tipo*" FieldName="tipo_depend" VisibleIndex="5"
                                        Width="100px" HeaderStyle-Font-Bold="true">
                                        <PropertiesComboBox ValueType="System.String" Width="150px" ClientInstanceName="cmbtipo_depend">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                <RequiredField IsRequired="true" ErrorText="Favor inserir o Tipo." />
                                            </ValidationSettings>
                                        </PropertiesComboBox>
                                        <HeaderStyle Font-Bold="True"></HeaderStyle>
                                    </dxwgv:GridViewDataComboBoxColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Área (m²)" FieldName="area" VisibleIndex="6"
                                        Width="70px">
                                        <CellStyle HorizontalAlign="Left">
                                        </CellStyle>
                                        <PropertiesTextEdit MaxLength="3" Style-HorizontalAlign="Left" Width="150px">
                                            <ClientSideEvents KeyPress="function(s,e){ SomentePermitirNumeros(s, e.htmlEvent); } " />
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                <RegularExpression ErrorText="Capacidade deve ser um número de até 3 dígitos." ValidationExpression="\d{0,3}" />
                                            </ValidationSettings>
                                            <Style HorizontalAlign="Left">
                                                </Style>
                                        </PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataComboBoxColumn VisibleIndex="7" Caption="Edificação*" FieldName="edificacao"
                                        Width="115px">
                                        <PropertiesComboBox TextField="nome_edificacao" ValueField="edificacao" EnableSynchronization="False"
                                            EnableIncrementalFiltering="True" DataSourceID="odsEdificacoes">
                                            <ClientSideEvents SelectedIndexChanged="function(s, e) { OnEdificacaoChanged(s); }">
                                            </ClientSideEvents>
                                        </PropertiesComboBox>
                                    </dxwgv:GridViewDataComboBoxColumn>
                                    <dxwgv:GridViewDataComboBoxColumn VisibleIndex="8" Caption="Pavimento*" FieldName="pavimento"
                                        Visible="false">
                                        <PropertiesComboBox EnableSynchronization="False" EnableIncrementalFiltering="True">
                                        </PropertiesComboBox>
                                    </dxwgv:GridViewDataComboBoxColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Pavimento*" FieldName="nome_pavimento" VisibleIndex="9"
                                        Width="115px">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Observação" FieldName="obs" VisibleIndex="10"
                                        Width="300px">
                                        <PropertiesTextEdit MaxLength="2000" Width="300px">
                                        </PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                                        Visible="False" VisibleIndex="11">
                                    </dxwgv:GridViewDataTextColumn>
                                </Columns>
                            </dxwgv:ASPxGridView>
                        </asp:Panel>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Compartilhamento">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl4" runat="server">
                        <asp:ObjectDataSource ID="odsAbaCompartilhadaComboRedeEnsino" runat="server" TypeName="Techne.Lyceum.RN.TabelaGeral"
                            SelectMethod="SelecionarItensAbaCompartilhadaComboRedeEnsino">
                            <SelectParameters>
                                <asp:Parameter Name="tabela" DefaultValue="CompartRedeEnsino"></asp:Parameter>
                                <asp:Parameter Name="usuario" Type="String" DefaultValue="" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:ObjectDataSource ID="odsCompartilhadaXTotalSalas" TypeName="Techne.Lyceum.Net.Basico.Unidade"
                            SelectMethod="ListarCompartilhadasETotalSalas" runat="server" InsertMethod="Insert"
                            OnInserting="odsCompartilhada_Inserting" DeleteMethod="DeleteXTotalSalas" OnDeleting="odsCompartilhada_Deleting"
                            UpdateMethod="Update" OnUpdating="odsCompartilhada_Updating">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseUnidade" DefaultValue="" Name="unidade" PropertyName="DBValue" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:ObjectDataSource ID="odsCompartilhada" TypeName="Techne.Lyceum.Net.Basico.Unidade"
                            SelectMethod="ListarCompartilhadas" runat="server" InsertMethod="Insert" OnInserting="odsCompartilhada_Inserting"
                            DeleteMethod="Delete" OnDeleting="odsCompartilhada_Deleting" UpdateMethod="Update"
                            OnUpdating="odsCompartilhada_Updating">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseUnidade" DefaultValue="" Name="unidade" PropertyName="DBValue" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:ObjectDataSource ID="odsCompartilhadaOferta" TypeName="Techne.Lyceum.Net.Basico.Unidade"
                            SelectMethod="ListarCompartilhadaOferta" runat="server" InsertMethod="InsertCompartilhadaOferta"
                            OnInserting="odsCompartilhadaOferta_Inserting" DeleteMethod="DeleteCompartilhadaOferta"
                            OnDeleting="odsCompartilhadaOferta_Deleting">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hdnCensoCompartilhada" DefaultValue="" Name="idCompartilhada"
                                    PropertyName="Value" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:ObjectDataSource ID="odsModalidade" runat="server" TypeName="Techne.Lyceum.RN.Curso"
                            SelectMethod="ListarModalidadeSerie"></asp:ObjectDataSource>
                        <asp:ObjectDataSource ID="odsNivel" runat="server" TypeName="Techne.Lyceum.RN.Curso"
                            SelectMethod="ListarTipoCurso"></asp:ObjectDataSource>
                        <asp:ObjectDataSource ID="odsTurnoCompartilhada" TypeName="Techne.Lyceum.RN.Turno"
                            SelectMethod="Consultar" runat="server"></asp:ObjectDataSource>
                        <dxwgv:ASPxGridView ID="grdCompartilhada" runat="server" DataSourceID="odsCompartilhadaXTotalSalas"
                            KeyFieldName="id_compartilhada" AutoGenerateColumns="False" Width="1100px" ClientInstanceName="grdCompartilhada"
                            Font-Size="Small" OnCellEditorInitialize="grdCompartilhada_CellEditorInitialize"
                            OnRowInserting="grdCompartilhada_RowInserting" OnInitNewRow="grdCompartilhada_InitNewRow"
                            OnStartRowEditing="grdCompartilhada_StartRowEditing" OnAfterPerformCallback="grdCompartilhada_AfterPerformCallback"
                            OnCommandButtonInitialize="grdCompartilhada_CommandButtonInitialize" OnRowUpdated="grdCompartilhada_RowUpdated">
                            <SettingsPager>
                                <Summary AllPagesText="Páginas: {0} - {1} ({2} items)" Text="Página {0} of {1} ({2} items)" />
                            </SettingsPager>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                            <SettingsBehavior ConfirmDelete="true" AllowMultiSelection="False" AllowFocusedRow="True"
                                AllowDragDrop="false" />
                            <SettingsText ConfirmDelete="Confirma a remoção ?" EmptyDataRow="Não existem dados." />
                            <SettingsEditing Mode="EditForm" />
                            <Templates>
                                <EditForm>
                                    <dxw:ContentControl ID="cntTemplateDependencia" runat="server">
                                        <div style="padding: 4px;">
                                            <table>
                                                <tr>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="lblTempRedeEnsino" runat="server" Text="Rede de Ensino"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <dxwgv:ASPxGridViewTemplateReplacement ID="TempRedeEnsino" runat="server" ColumnID="rede_ensino"
                                                            ReplacementType="EditFormCellEditor">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="lblTempCenso" runat="server" Text="Censo"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <dxwgv:ASPxGridViewTemplateReplacement ID="TempCenso" runat="server" ColumnID="censo_compartilhada"
                                                            ReplacementType="EditFormCellEditor">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="lblTempUnidade" runat="server" Text="Unidade"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <dxwgv:ASPxGridViewTemplateReplacement ID="TempUnidade" runat="server" ColumnID="nome"
                                                            ReplacementType="EditFormCellEditor">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="lblCedidasManha" runat="server" Text="Cedidas Manhã"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <dxwgv:ASPxGridViewTemplateReplacement ID="aspxTxtCedidasManha" runat="server" ColumnID="cedidas_manha"
                                                            ReplacementType="EditFormCellEditor">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="lblCedidasTarde" runat="server" Text="Cedidas Tarde"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <dxwgv:ASPxGridViewTemplateReplacement ID="aspxTxtCedidasTarde" runat="server" ColumnID="cedidas_tarde"
                                                            ReplacementType="EditFormCellEditor">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="lblCedidasNoite" runat="server" Text="Cedidas Noite"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <dxwgv:ASPxGridViewTemplateReplacement ID="aspxTxtCedidasNoite" runat="server" ColumnID="cedidas_noite"
                                                            ReplacementType="EditFormCellEditor">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                </tr>
                                            </table>
                                            <dxwgv:ASPxGridViewTemplateReplacement ID="repUpdate" runat="server" ReplacementType="EditFormUpdateButton">
                                            </dxwgv:ASPxGridViewTemplateReplacement>
                                            <dxwgv:ASPxGridViewTemplateReplacement ID="repCancel" runat="server" ReplacementType="EditFormCancelButton">
                                            </dxwgv:ASPxGridViewTemplateReplacement>
                                        </div>
                                    </dxw:ContentControl>
                                </EditForm>
                            </Templates>
                            <Columns>
                                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="50px">
                                    <HeaderCaptionTemplate>
                                        <div style="text-align: center">
                                            <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                                onclick="grdCompartilhada.AddNewRow();" alt="Novo" />
                                        </div>
                                    </HeaderCaptionTemplate>
                                    <EditButton Text="Editar" Visible="True">
                                        <Image Url="~/img/bt_editar.png" />
                                    </EditButton>
                                    <DeleteButton Text="Remover" Visible="True">
                                        <Image Url="~/img/bt_exclui2.png" />
                                    </DeleteButton>
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
                                <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="id_compartilhada" VisibleIndex="1"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Censo" FieldName="censo" VisibleIndex="2"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataComboBoxColumn Caption="Rede de Ensino" FieldName="rede_ensino"
                                    VisibleIndex="3" Width="150px">
                                    <PropertiesComboBox TextField="Descr" ValueField="Item" EnableSynchronization="False"
                                        EnableIncrementalFiltering="True" DataSourceID="odsAbaCompartilhadaComboRedeEnsino">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnRedeEnsinoChanged(s); }">
                                        </ClientSideEvents>
                                    </PropertiesComboBox>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataSpinEditColumn Caption="Censo" FieldName="censo_compartilhada"
                                    VisibleIndex="4" Width="150px" PropertiesSpinEdit-MaxLength="8">
                                    <PropertiesSpinEdit DisplayFormatString="g" MaxLength="8" NumberFormat="Custom" NumberType="Integer">
                                        <SpinButtons ShowIncrementButtons="False">
                                        </SpinButtons>
                                        <ClientSideEvents Validation="function(s, e) {
	                                                                var strVal=e.value;
	                                                                var iVal=null;
	                                                                e.isValid = true;
	                                                                try
	                                                                {
		                                                                if(strVal!=null)
		                                                                {
			                                                                iVal=parseInt(strVal);
			                                                                if(iVal&lt;1)
			                                                                {
				                                                                e.isValid = false;
				                                                                e.errorText='O Censo deve ser positivo';
			                                                                }
		                                                                }
	                                                                }
	                                                                catch(ex)
	                                                                {
		                                                                e.isValid = false;
		                                                                e.errorText=ex;
	                                                                }
                                                                }" />
                                    </PropertiesSpinEdit>
                                </dxwgv:GridViewDataSpinEditColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Unidade" FieldName="nome" VisibleIndex="5">
                                    <PropertiesTextEdit MaxLength="50" Width="400px">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Cedidas Manhã" FieldName="cedidas_manha" VisibleIndex="6">
                                    <PropertiesTextEdit MaxLength="50" Width="150px">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Cedidas Tarde" FieldName="cedidas_tarde" VisibleIndex="6">
                                    <PropertiesTextEdit MaxLength="50" Width="150px">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Cedidas Noite" FieldName="cedidas_noite" VisibleIndex="6">
                                    <PropertiesTextEdit MaxLength="50" Width="150px">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Salas de Aula da Unidade" FieldName="totSalas"
                                    VisibleIndex="6">
                                    <PropertiesTextEdit MaxLength="50" Width="150px">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                        <br />
                        <asp:Button ID="btnVerOfertas" runat="server" Text="Ver Ofertas" OnClick="btnVerOfertas_Click" />
                        <br />
                        <br />
                        <asp:Label ID="lblCensoCompartilhadoSelecionado" runat="server"></asp:Label>
                        <br />
                        <dxwgv:ASPxGridView ID="grdCompartilhadaOfertaEstadual" runat="server" AutoGenerateColumns="False"
                            Width="900px" Font-Size="Small" ClientVisible="false">
                            <SettingsPager>
                                <Summary AllPagesText="Páginas: {0} - {1} ({2} items)" Text="Página {0} of {1} ({2} items)" />
                            </SettingsPager>
                            <Settings ShowFilterRow="False" ShowFilterRowMenu="False" />
                            <SettingsBehavior AllowMultiSelection="False" AllowDragDrop="false" />
                            <Columns>
                                <dxwgv:GridViewDataTextColumn Caption="Oferta" FieldName="mod_seg_curso" VisibleIndex="4">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Turnos" FieldName="turno" VisibleIndex="5">
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                        <dxwgv:ASPxGridView ID="grdCompartilhadaOferta" runat="server" KeyFieldName="id_compartilhada_oferta"
                            DataSourceID="odsCompartilhadaOferta" AutoGenerateColumns="False" Width="900px"
                            ClientInstanceName="grdCompartilhadaOferta" Font-Size="Small" ClientVisible="false"
                            OnAfterPerformCallback="grdCompartilhadaOferta_AfterPerformCallback" OnCellEditorInitialize="grdCompartilhadaOferta_CellEditorInitialize">
                            <SettingsPager>
                                <Summary AllPagesText="Páginas: {0} - {1} ({2} items)" Text="Página {0} of {1} ({2} items)" />
                            </SettingsPager>
                            <Settings ShowFilterRow="False" ShowFilterRowMenu="False" />
                            <SettingsBehavior ConfirmDelete="true" AllowMultiSelection="False" AllowDragDrop="false" />
                            <SettingsText ConfirmDelete="Confirma a remoção ?" EmptyDataRow="Não existem dados." />
                            <SettingsEditing Mode="EditForm" />
                            <Templates>
                                <EditForm>
                                    <dxw:ContentControl ID="cntTemplateDependencia" runat="server">
                                        <div style="padding: 4px;">
                                            <table>
                                                <tr>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="lblTempModalidade" runat="server" Text="Modalidade"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <dxe:ASPxComboBox ID="cmbTempModalidade" runat="server" DataSourceID="odsModalidade"
                                                            TextField="DESCRICAO" ValueField="MODALIDADE">
                                                            <ClientSideEvents SelectedIndexChanged="function (s, e) { OnModalidadeEscolaridadeChanged(); }">
                                                            </ClientSideEvents>
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="lblTempSegmento" runat="server" Text="Segmento"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <dxe:ASPxComboBox ID="cmbTempSegmento" runat="server" DataSourceID="odsNivel" TextField="DESCRICAO"
                                                            ValueField="TIPO">
                                                            <ClientSideEvents SelectedIndexChanged="function (s, e) { OnModalidadeEscolaridadeChanged(); }">
                                                            </ClientSideEvents>
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="lblTempCurso" runat="server" Text="Curso"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <dxwgv:ASPxGridViewTemplateReplacement ID="TempCurso" runat="server" ColumnID="curso"
                                                            ReplacementType="EditFormCellEditor">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="lblTempTurno" runat="server" Text="Turno"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <dxwgv:ASPxGridViewTemplateReplacement ID="TempTurno" runat="server" ColumnID="turno"
                                                            ReplacementType="EditFormCellEditor">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                </tr>
                                            </table>
                                            <dxwgv:ASPxGridViewTemplateReplacement ID="repUpdate" runat="server" ReplacementType="EditFormUpdateButton">
                                            </dxwgv:ASPxGridViewTemplateReplacement>
                                            <dxwgv:ASPxGridViewTemplateReplacement ID="repCancel" runat="server" ReplacementType="EditFormCancelButton">
                                            </dxwgv:ASPxGridViewTemplateReplacement>
                                        </div>
                                    </dxw:ContentControl>
                                </EditForm>
                            </Templates>
                            <Columns>
                                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="50px">
                                    <HeaderCaptionTemplate>
                                        <div style="text-align: center">
                                            <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                                onclick="grdCompartilhadaOferta.AddNewRow();" alt="Novo" />
                                        </div>
                                    </HeaderCaptionTemplate>
                                    <DeleteButton Text="Remover" Visible="True">
                                        <Image Url="~/img/bt_exclui2.png" />
                                    </DeleteButton>
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
                                <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="id_compartilhada_oferta"
                                    VisibleIndex="1" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="id_compartilhada" VisibleIndex="2"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Oferta" FieldName="mod_seg_curso" VisibleIndex="3">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataComboBoxColumn Caption="Turnos" FieldName="turno" VisibleIndex="4">
                                    <PropertiesComboBox DataSourceID="odsTurnoCompartilhada" ValueField="TURNO" TextField="DESCRICAO">
                                    </PropertiesComboBox>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataComboBoxColumn Caption="Curso" FieldName="curso" VisibleIndex="5"
                                    Visible="false">
                                </dxwgv:GridViewDataComboBoxColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Histórico da Unidade">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl5" runat="server">
                        <dxw:ContentControl ID="ccHistoricoUnidade" runat="server">
                            <asp:ObjectDataSource ID="odsSituacaoUnidade" TypeName="Techne.Lyceum.Net.Basico.Unidade"
                                runat="server" SelectMethod="ListarSituacaoUnidade">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="tseUnidade" PropertyName="DBValue" Name="unidade" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:ObjectDataSource ID="odsTabelaGeralSituacaoUnidade" runat="server" TypeName="Techne.Lyceum.RN.Cache.TabelaGeral"
                                SelectMethod="SelecionarItens">
                                <SelectParameters>
                                    <asp:Parameter Name="tabela" DefaultValue="SituacaoUnidade"></asp:Parameter>
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:ObjectDataSource ID="odsTabelaGeralAtoOficial" runat="server" TypeName="Techne.Lyceum.RN.Cache.TabelaGeral"
                                SelectMethod="SelecionarItens">
                                <SelectParameters>
                                    <asp:Parameter Name="tabela" DefaultValue="AtoOficial"></asp:Parameter>
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <dxwgv:ASPxGridView ID="grdSituacaoUnidade" runat="server" AutoGenerateColumns="False"
                                DataSourceID="odsSituacaoUnidade" KeyFieldName="CompositeKey" ClientInstanceName="grdSituacaoUnidade"
                                OnCustomUnboundColumnData="grdSituacaoUnidade_CustomUnboundColumnData" OnRowDeleting="grdSituacaoUnidade_RowDeleting"
                                OnRowUpdating="grdSituacaoUnidade_RowUpdating" OnAfterPerformCallback="grdSituacaoUnidade_AfterPerformCallback"
                                OnCellEditorInitialize="grdSituacaoUnidade_CellEditorInitialize" OnRowValidating="grdSituacaoUnidade_Validating"
                                OnRowInserting="grdSituacaoUnidade_RowInserting">
                                <SettingsBehavior ConfirmDelete="True" AllowDragDrop="false" />
                                <SettingsEditing Mode="Inline" />
                                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                <Settings ShowFilterRow="false" ShowFilterRowMenu="false" />
                                <ClientSideEvents EndCallback="function(s,e) { OnEndCallback(s); }"></ClientSideEvents>
                                <Columns>
                                    <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                        <HeaderCaptionTemplate>
                                            <img runat="server" id="btnNovoGrid" alt="Novo" src="../img/bt_novo.png" onclick="grdSituacaoUnidade.AddNewRow();" />
                                        </HeaderCaptionTemplate>
                                        <EditButton Text="Editar" Visible="TRUE">
                                            <Image Url="~/img/bt_editar.png" />
                                        </EditButton>
                                        <DeleteButton Text="Remover" Visible="True">
                                            <Image Url="~/img/bt_exclui2.png" />
                                        </DeleteButton>
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
                                    <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="UNIDADE_ENS"
                                        Visible="false" VisibleIndex="1">
                                        <PropertiesTextEdit MaxLength="20">
                                        </PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Ordem" FieldName="ORDEM" Visible="false" VisibleIndex="2">
                                        <PropertiesTextEdit MaxLength="10">
                                        </PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataComboBoxColumn Caption="Rede de Ensino*" FieldName="SITUACAO"
                                        VisibleIndex="3" Width="200px" HeaderStyle-Font-Bold="true">
                                        <PropertiesComboBox DataSourceID="odsTabelaGeralSituacaoUnidade" TextField="Descr"
                                            ValueField="Item" ValueType="System.String" Width="200px" MaxLength="40">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                <RequiredField IsRequired="true" ErrorText="Favor selecionar a Rede de Ensino." />
                                            </ValidationSettings>
                                        </PropertiesComboBox>
                                    </dxwgv:GridViewDataComboBoxColumn>
                                    <dxwgv:GridViewDataComboBoxColumn Caption="Ato*" FieldName="ATO_OFICIAL" VisibleIndex="4"
                                        Width="150px" HeaderStyle-Font-Bold="true">
                                        <PropertiesComboBox DataSourceID="odsTabelaGeralAtoOficial" TextField="Descr" ValueField="Item"
                                            ValueType="System.String" Width="150px" MaxLength="40">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                <RequiredField IsRequired="true" ErrorText="Favor selecionar o Ato." />
                                            </ValidationSettings>
                                        </PropertiesComboBox>
                                    </dxwgv:GridViewDataComboBoxColumn>
                                    <dxwgv:GridViewDataDateColumn FieldName="DT_SITUACAO" VisibleIndex="5" Caption="Data Ato*"
                                        Width="110px">
                                        <PropertiesDateEdit Width="110px">
                                        </PropertiesDateEdit>
                                    </dxwgv:GridViewDataDateColumn>
                                    <dxwgv:GridViewDataDateColumn FieldName="DT_DOU" VisibleIndex="6" Caption="Data DO*"
                                        Width="110px">
                                        <PropertiesDateEdit Width="110px">
                                        </PropertiesDateEdit>
                                    </dxwgv:GridViewDataDateColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Número Ato Oficial*" FieldName="NUMERO_ATO_OFICIAL"
                                        VisibleIndex="7" Width="100px">
                                        <PropertiesTextEdit MaxLength="100" Width="100px">
                                        </PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Observação" FieldName="OBSERVACAO" VisibleIndex="8"
                                        Width="300px">
                                        <PropertiesTextEdit MaxLength="4000" Width="300px">
                                        </PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                                        Visible="False" VisibleIndex="9">
                                    </dxwgv:GridViewDataTextColumn>
                                </Columns>
                            </dxwgv:ASPxGridView>
                            <br />
                        </dxw:ContentControl>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Municipalização">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl60" runat="server">
                        <asp:Panel ID="pnlMunicipalizacao" runat="server" GroupingText="Municipalização">
                            <asp:Panel ID="pnlUltimaMunic" runat="server" GroupingText="Vigente">
                                <table>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label27" runat="server" Text="No. do processo de municipalização:"
                                                SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUltProcMunic" runat="server" Width="180px" ReadOnly="true" Enabled="false" />
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label28" runat="server" Text="Documento vigente:" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUltDocVigente" runat="server" Width="180px" ReadOnly="true" Enabled="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label29" runat="server" Text="Data de autorizo:" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUltDtAutorizo" runat="server" Width="180px" ReadOnly="true" Enabled="false" />
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label30" runat="server" Text="Data de vigência final:" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUltDtVigFinal" runat="server" Width="180px" ReadOnly="true" Enabled="false" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <br />
                            <asp:Panel ID="pnlDadosMunicipalizacao" runat="server" Visible="false" GroupingText="Dados Municipalização">
                                <table>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label5" runat="server" Text="No. do processo de municipalização:*"
                                                SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProcessoMunic" runat="server" MaxLength="50" Width="180px" />
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label6" runat="server" Text="No. do autorizo provisório:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAutorizoProv" runat="server" MaxLength="50" Width="280px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label7" runat="server" Text="Data do autorizo provisório:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="dtAutorizoProv" runat="server" MinDate="1901-01-01" Width="120px"
                                                EnableDefaultAppearance="true" ClientInstanceName="dtAutorizoProv" CalendarProperties-ClearButtonText="Limpar"
                                                CalendarProperties-TodayButtonText="Hoje">
                                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                </CalendarProperties>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label8" runat="server" Text="Validade do Autorizo:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="dtValidadeAutorizo" runat="server" MinDate="1901-01-01" Width="120px"
                                                EnableDefaultAppearance="true" ClientInstanceName="dtValidadeAutorizo" CalendarProperties-ClearButtonText="Limpar"
                                                CalendarProperties-TodayButtonText="Hoje">
                                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                </CalendarProperties>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label9" runat="server" Text="Data da publicação de extrato em DO:*"
                                                SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="dtPublicacaoDO" runat="server" MinDate="1901-01-01" Width="120px"
                                                EnableDefaultAppearance="true" ClientInstanceName="dtPublicacaoDO" CalendarProperties-ClearButtonText="Limpar"
                                                CalendarProperties-TodayButtonText="Hoje">
                                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                </CalendarProperties>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label11" runat="server" Text="Página DO:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPaginaDO" runat="server" MaxLength="3" Width="50px" SkinID="numerico" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnSalvarMunic" runat="server" ValidationGroup="SalvarForm" Text="Salvar Dados Municipalização"
                                                OnClick="btnSalvarMunic_Click" Visible="false" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <br />
                            <asp:Panel ID="pnlDocCelebrado" runat="server" Visible="false" GroupingText="Documento Celebrado">
                                <table>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label25" runat="server" Text="Número Processo:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlNumProcessoDocCelebrado" runat="server" DataTextField="PROCESSO"
                                                DataValueField="ID_MUNICIPALIZACAO">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label21" runat="server" Text="Tipo:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTipoDocCelebrado" runat="server" DataTextField="descr" DataValueField="item">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label22" runat="server" Text="Número:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNumeroDocCelebrado" runat="server" MaxLength="50" Width="608px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label23" runat="server" Text="Data de Validade - Início:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="dtDataValidadeInicio" runat="server" MinDate="1901-01-01" Width="120px"
                                                EnableDefaultAppearance="true" ClientInstanceName="dtAutorizoProv" CalendarProperties-ClearButtonText="Limpar"
                                                CalendarProperties-TodayButtonText="Hoje">
                                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                </CalendarProperties>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label24" runat="server" Text="Data de Validade - Final:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="dtDataValidadeFinal" runat="server" MinDate="1901-01-01" Width="120px"
                                                EnableDefaultAppearance="true" ClientInstanceName="dtAutorizoProv" CalendarProperties-ClearButtonText="Limpar"
                                                CalendarProperties-TodayButtonText="Hoje">
                                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                </CalendarProperties>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="Label26" runat="server" Text="Observação:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtObsDocCelebrado" runat="server" MaxLength="1000" TextMode="MultiLine"
                                                Width="608px" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnSalvarDocCelebrado" runat="server" ValidationGroup="SalvarForm"
                                                Text="Salvar Informações Documentos" OnClick="btnSalvarDocCelebrado_Click" />
                                        </td>
                                        <td colspan="2">
                                            <asp:Button ID="btnCancelarMunicipalizacao" runat="server" Text="Cancelar" OnClick="btnCancelarMunicipalizacao_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:HiddenField ID="hdnIdDocCelebrado" runat="server" />
                            <table>
                                <tr>
                                    <td>
                                        <asp:ObjectDataSource ID="odsDocumentosCelebrados" runat="server" SelectMethod="ListarDocumentos"
                                            TypeName="Techne.Lyceum.Net.Basico.Unidade">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="tseUnidade" Name="unidade" PropertyName="DBValue" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                        <asp:ObjectDataSource ID="odsTabelaGeralTipoDocCelebrado" runat="server" SelectMethod="SelecionarItens"
                                            TypeName="Techne.Lyceum.RN.Cache.TabelaGeral">
                                            <SelectParameters>
                                                <asp:Parameter DefaultValue="TipoDocCelebrado" Name="tabela" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                        <dxwgv:ASPxGridView ID="grdDocumentosCelebrados" runat="server" AutoGenerateColumns="False"
                                            ClientInstanceName="grdDocumentosCelebrados" DataSourceID="odsDocumentosCelebrados"
                                            OnCustomButtonCallback="grdDocumentosCelebrados_CustomButtonCallback" EnableCallBacks="false"
                                            KeyFieldName="ID_DOC_CELEBRADO_MUNICIPALIZACAO" OnAfterPerformCallback="grdDocumentosCelebrados_AfterPerformCallback">
                                            <SettingsBehavior ConfirmDelete="True" AllowDragDrop="false" />
                                            <Columns>
                                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                                                    <HeaderCaptionTemplate>
                                                        <div style="text-align: center" id="dvteste">
                                                            <input type="image" id="btnNovoDocCelebrado" src="../img/bt_novo.png" style="cursor: pointer"
                                                                title="Novo" onserverclick="HabilitaPnlNovo" runat="server" />
                                                        </div>
                                                    </HeaderCaptionTemplate>
                                                    <CustomButtons>
                                                        <dxwgv:GridViewCommandColumnCustomButton Text="Editar" ID="btnEditarDocCelebrado"
                                                            Visibility="AllDataRows" Image-Url="~/img/bt_editar.png" Image-Height="15px"
                                                            Image-AlternateText="Editar">
                                                        </dxwgv:GridViewCommandColumnCustomButton>
                                                        <dxwgv:GridViewCommandColumnCustomButton Text="Excluir" ID="btnExcluirDocCelebrado"
                                                            Visibility="AllDataRows" Image-Url="~/img/bt_exclui2.png" Image-Height="15px"
                                                            Image-AlternateText="Excluir">
                                                        </dxwgv:GridViewCommandColumnCustomButton>
                                                    </CustomButtons>
                                                </dxwgv:GridViewCommandColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Codigo" FieldName="ID_DOC_CELEBRADO_MUNICIPALIZACAO"
                                                    Visible="False" VisibleIndex="1">
                                                    <PropertiesTextEdit MaxLength="20">
                                                    </PropertiesTextEdit>
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Codigo" FieldName="ID_MUNICIPALIZACAO" Visible="False"
                                                    VisibleIndex="1">
                                                    <PropertiesTextEdit MaxLength="20">
                                                    </PropertiesTextEdit>
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Nº do Processo" FieldName="PROCESSO" VisibleIndex="6"
                                                    Width="200px">
                                                    <PropertiesTextEdit MaxLength="100" Width="200px">
                                                    </PropertiesTextEdit>
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataComboBoxColumn Caption="Tipo*" FieldName="TIPO" VisibleIndex="1"
                                                    Width="200px">
                                                    <PropertiesComboBox DataSourceID="odsTabelaGeralTipoDocCelebrado" MaxLength="40"
                                                        TextField="Descr" ValueField="Item" ValueType="System.String" Width="200px">
                                                        <ValidationSettings Display="Dynamic">
                                                            <RequiredField ErrorText="Favor selecionar o Tipo." IsRequired="True" />
                                                        </ValidationSettings>
                                                    </PropertiesComboBox>
                                                    <HeaderStyle Font-Bold="True" />
                                                </dxwgv:GridViewDataComboBoxColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Número*" FieldName="NUMERO" VisibleIndex="2"
                                                    Width="200px">
                                                    <PropertiesTextEdit MaxLength="100" Width="200px">
                                                    </PropertiesTextEdit>
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataDateColumn Caption="Data da Validade Início*" FieldName="DT_CELEBRACAO"
                                                    VisibleIndex="3" Width="110px">
                                                    <HeaderStyle Font-Bold="True" />
                                                </dxwgv:GridViewDataDateColumn>
                                                <dxwgv:GridViewDataDateColumn Caption="Data da Validade Final*" FieldName="DT_VALIDADE"
                                                    VisibleIndex="4" Width="110px">
                                                </dxwgv:GridViewDataDateColumn>
                                                <dxwgv:GridViewDataDateColumn Caption="Data da publicação do extrato em D.O." FieldName="DT_PUBLICACAO_DO"
                                                    VisibleIndex="4" Width="110px">
                                                </dxwgv:GridViewDataDateColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Página D.O.*" FieldName="PAGINA_DO" VisibleIndex="5"
                                                    Width="200px">
                                                    <PropertiesTextEdit MaxLength="100" Width="200px">
                                                    </PropertiesTextEdit>
                                                </dxwgv:GridViewDataTextColumn>
                                                <dxwgv:GridViewDataTextColumn Caption="Observação" FieldName="OBSERVACAO" VisibleIndex="6"
                                                    Width="200px">
                                                    <PropertiesTextEdit MaxLength="100" Width="200px">
                                                    </PropertiesTextEdit>
                                                </dxwgv:GridViewDataTextColumn>
                                            </Columns>
                                            <SettingsEditing Mode="Inline" />
                                            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                        </dxwgv:ASPxGridView>
                                    </td>
                                </tr>
                            </table>
                            <dxpc:ASPxPopupControl ID="pucConfirmarDocCelebrado" ClientInstanceName="pucConfirmarDocCelebrado"
                                runat="server" Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false"
                                ShowCloseButton="false" ShowFooter="false" ShowHeader="false" ShowSizeGrip="False"
                                EnableAnimation="false" Width="300px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                                <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
                                <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
                                <ContentCollection>
                                    <dxpc:PopupControlContentControl>
                                        <table>
                                            <tr align="center">
                                                <td>
                                                    Confirma a exclusão do Documento Celebrado?<br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center;">
                                                    <asp:Button ID="btnSimDocCelebrado" runat="server" Text="Sim" OnClick="btnSimDocCelebrado_Click" />
                                                    <asp:Button ID="btnNaoDocCelebrado" runat="server" Text="Não" OnClick="btnNaoDocCelebrado_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </dxpc:PopupControlContentControl>
                                </ContentCollection>
                            </dxpc:ASPxPopupControl>
                        </asp:Panel>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Implantação de Cursos/Modalidades">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl6" runat="server">
                        <asp:ObjectDataSource ID="odsCursoPorUnidade" TypeName="Techne.Lyceum.Net.Basico.Unidade"
                            runat="server" SelectMethod="ListarCursoUnidade" DeleteMethod="DeleteModalidade"
                            UpdateMethod="UpdateModalidade" InsertMethod="InsertModalidade" OnDeleting="odsCursoPorUnidade_Deleting"
                            OnInserting="odsCursoPorUnidade_Inserting" OnUpdating="odsCursoPorUnidade_Updating">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseUnidade" Name="unidade" PropertyName="DBValue" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:ObjectDataSource ID="odsCursoGrid" runat="server" TypeName="Techne.Lyceum.RN.Curso"
                            SelectMethod="ConsultarDetalhesCurso"></asp:ObjectDataSource>
                        <techne:TTableDataSource ID="dsTurno" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_turno"
                            SqlOrder="descricao">
                        </techne:TTableDataSource>
                        <techne:TTableDataSource ID="dsNivel" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_tipo_curso"
                            SqlOrder="descricao">
                        </techne:TTableDataSource>
                        <techne:TTableDataSource ID="dsModalidade" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_modalidade_curso"
                            SqlOrder="descricao">
                        </techne:TTableDataSource>
                        <dxwgv:ASPxGridView ID="grdCursoPorUnidade" runat="server" AutoGenerateColumns="False"
                            DataSourceID="odsCursoPorUnidade" KeyFieldName="CompositeKey" ClientInstanceName="grdCursoPorUnidade"
                            OnCustomUnboundColumnData="grdCursoPorUnidade_CustomUnboundColumnData" OnRowDeleting="grdCursoPorUnidade_RowDeleting"
                            OnRowUpdating="grdCursoPorUnidade_RowUpdating" OnAfterPerformCallback="grdCursoPorUnidade_AfterPerformCallback"
                            OnCellEditorInitialize="grdCursoPorUnidade_CellEditorInitialize">
                            <SettingsBehavior ConfirmDelete="True" AllowDragDrop="false" />
                            <SettingsEditing Mode="Inline" />
                            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                            <Columns>
                                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                    <HeaderCaptionTemplate>
                                        <div style="text-align: center">
                                            <img runat="server" id="btnNovoGridCursoPorUnidade" alt="Novo" src="../img/bt_novo.png"
                                                onclick="grdCursoPorUnidade.AddNewRow();" />
                                        </div>
                                    </HeaderCaptionTemplate>
                                    <EditButton Text="Editar" Visible="True">
                                        <Image Url="~/img/bt_editar.png" />
                                    </EditButton>
                                    <DeleteButton Text="Remover" Visible="True">
                                        <Image Url="~/img/bt_exclui2.png" />
                                    </DeleteButton>
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
                                <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="unidade_ens"
                                    Visible="false" VisibleIndex="1">
                                    <PropertiesTextEdit MaxLength="20">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Modalidade*" FieldName="modalidade" VisibleIndex="3"
                                    Width="200px" ReadOnly="true">
                                    <PropertiesTextEdit ClientInstanceName="txtModalidade">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Segmento*" FieldName="nivel" VisibleIndex="4"
                                    Width="150px" ReadOnly="true">
                                    <PropertiesTextEdit ClientInstanceName="txtNivel">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="codigo" ReadOnly="True"
                                    VisibleIndex="5">
                                    <PropertiesTextEdit ClientInstanceName="txtCodigo">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataComboBoxColumn Caption="Curso*" HeaderStyle-Font-Bold="true" FieldName="nome"
                                    VisibleIndex="6" Width="300px">
                                    <PropertiesComboBox DataSourceID="odsCursoGrid" EnableDefaultAppearance="false" ListBoxStyle-CssClass="dxeListBox"
                                        CssPostfix="Office2003_Blue" ClientInstanceName="cboEscolaridade" TextFormatString="{1}                                                             |{2}|{3}"
                                        ValueField="curso" ValueType="System.String" DropDownWidth="800px" Width="350px"
                                        MaxLength="300">
                                        <Columns>
                                            <dxe:ListBoxColumn Caption="Código" FieldName="curso" Name="curso" Width="100px" />
                                            <dxe:ListBoxColumn Caption="Escolaridade" FieldName="nome" Name="nome" Width="270px" />
                                            <dxe:ListBoxColumn Caption="Modalidade" FieldName="modalidade" Name="modalidade"
                                                Width="260px" />
                                            <dxe:ListBoxColumn Caption="Nível" FieldName="nivel" Name="nivel" Width="190px" />
                                        </Columns>
                                        <ClientSideEvents SelectedIndexChanged="function(s,e){ OnEscolaridadeChanged(); }" />
                                        <ListBoxStyle CssClass="dxeListBox">
                                        </ListBoxStyle>
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField IsRequired="true" ErrorText="Favor escolher uma escolaridade." />
                                        </ValidationSettings>
                                    </PropertiesComboBox>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataDateColumn FieldName="dt_implantacao" VisibleIndex="7" Caption="Data de Implantação*"
                                    Width="120px">
                                    <PropertiesDateEdit Width="120px">
                                    </PropertiesDateEdit>
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataDateColumn FieldName="dt_do" VisibleIndex="8" Caption="Data D.O.*"
                                    Width="120px">
                                    <PropertiesDateEdit Width="120px">
                                    </PropertiesDateEdit>
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Número Ato*" FieldName="ato" VisibleIndex="9"
                                    Width="80px">
                                    <PropertiesTextEdit MaxLength="100" Width="80px">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Processo de Autorização" FieldName="PROCESSO"
                                    VisibleIndex="10" Width="80px">
                                    <PropertiesTextEdit MaxLength="100" Width="80px">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Observação" FieldName="OBSERVACOES" VisibleIndex="11"
                                    Width="80px">
                                    <PropertiesTextEdit MaxLength="500" Width="80px">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                                    Visible="False" VisibleIndex="12">
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                            <Settings ShowFilterRow="false" ShowFilterRowMenu="false" />
                        </dxwgv:ASPxGridView>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Concessionárias">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl7" runat="server">
                        <asp:Panel ID="pnlEnergiaEletrica" runat="server" GroupingText="Energia Elétrica"
                            Width="931px">
                            <asp:HiddenField ID="hdnCodigoConcessionaria" runat="server" />
                            <asp:Panel ID="pnlTipoAbastecimentoEnergia" runat="server" GroupingText="Tipo de Abastecimento">
                                <table>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:CheckBoxList ID="chkTipoAbastecimentoEnergia" runat="server" RepeatDirection="Horizontal"
                                                OnSelectedIndexChanged="chkTipoAbastecimentoEnergia_SelectedIndexChanged" Style="text-align: left"
                                                AutoPostBack="true">
                                                <asp:ListItem Value="Rede Pública/Concessionária">Rede Pública/Concessionária</asp:ListItem>
                                                <asp:ListItem Value="Gerador">Gerador</asp:ListItem>
                                                <asp:ListItem Value="Outros">Outros(Ex.:Eólica,solar,etc)</asp:ListItem>
                                                <asp:ListItem Value="Inexistente">Inexistente</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlnformacoesEnergia" runat="server" GroupingText="Informações da Concessionária">
                                <table>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblEE_Concessionaria" runat="server" Text="Concessionária:" Width="150px">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <tweb:TSearchBox ID="tseEE_Concessionaria" runat="server" Key="empresa" Argument="razao_social"
                                                SqlSelect="SELECT empresa, razao_social, nome FROM ly_empresa" SqlOrder="razao_social"
                                                Value='<%# Bind("empresa") %>' MaxLength="20" AutoPostBack="false" CssClass="ReadOnlyField">
                                                <GridColumns>
                                                    <tweb:TSearchBoxColumn Caption="Código da Empresa" FieldName="empresa" Width="150px" />
                                                    <tweb:TSearchBoxColumn Caption="Razão Social" FieldName="razao_social" Width="300px" />
                                                    <tweb:TSearchBoxColumn Caption="Nome da Empresa" FieldName="nome" Width="300px" />
                                                </GridColumns>
                                            </tweb:TSearchBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblEE_NumeroCliente" runat="server" Text="Número do Cliente:" Width="150px">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEE_NumeroCliente" runat="server" MaxLength="100" Width="200px"
                                                SkinID="numerico">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="EE_ClasseFornecimento" runat="server" MaxLength="50" Text="Classe de Fornecimento:"
                                                Width="150">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlEE_ClasseFornecimento" runat="server" DataTextField="descr"
                                                DataValueField="item" Width="300px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblEE_Contrato" runat="server" Text="Contrato de Fornecimento" Width="150">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <dxe:ASPxCheckBox ID="chkEE_Contrato" runat="server" ValueChecked="S" ValueUnchecked="N"
                                                ValueType="System.String" Checked="false" MaxLength="50">
                                            </dxe:ASPxCheckBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlSuprimentoAgua" runat="server" GroupingText="Suprimento de Água"
                            Width="931px">
                            <asp:Panel ID="Panel1" runat="server" GroupingText="Tipo de Abastecimento">
                                <table>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:CheckBoxList ID="chkOutrosAbastecimentos" runat="server" RepeatDirection="Horizontal"
                                                OnSelectedIndexChanged="chkOutrosAbastecimentos_SelectedIndexChanged" Style="text-align: left"
                                                AutoPostBack="true">
                                                <asp:ListItem Value="Rede Pública/Concessionária">Rede Pública/Concessionária</asp:ListItem>
                                                <asp:ListItem Value="Outros">Fonte / Rio /Igarapé / Riacho / Córrego</asp:ListItem>
                                                <asp:ListItem Value="Inexistente">Inexistente</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlConcessionariaAgua" runat="server" GroupingText="Informações da Concessionária">
                                <table>
                                    <tr>
                                        <td style="text-align: right; width: 150px">
                                            <asp:Label ID="lblAgua_Concessionaria" runat="server" Text="Concessionária: " Width="150px">
                                            </asp:Label>
                                        </td>
                                        <td colspan="2" style="width: 781px">
                                            <tweb:TSearchBox ID="tseAgua_Concessionaria" runat="server" Key="empresa" Argument="razao_social"
                                                SqlSelect="SELECT empresa, razao_social, nome FROM ly_empresa" SqlOrder="razao_social"
                                                Value='<%# Bind("empresa") %>' MaxLength="20" AutoPostBack="false">
                                                <GridColumns>
                                                    <tweb:TSearchBoxColumn Caption="Código da Empresa" FieldName="empresa" Width="150px" />
                                                    <tweb:TSearchBoxColumn Caption="Razão Social" FieldName="razao_social" Width="300px" />
                                                    <tweb:TSearchBoxColumn Caption="Nome da Empresa" FieldName="nome" Width="300px" />
                                                </GridColumns>
                                            </tweb:TSearchBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 150px">
                                            <asp:Label ID="lblAgua_NumeroCliente" runat="server" Text="Número do Cliente: " Width="150px">
                                            </asp:Label>
                                        </td>
                                        <td colspan="2" style="width: 781px">
                                            <asp:TextBox ID="txtAgua_NumeroCliente" runat="server" Width="200px" MaxLength="100"
                                                SkinID="numerico">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 150px">
                                            <asp:Label ID="lblAgua_Hidrometro" runat="server" Text="Possui Hidrômetro? " Width="150px">
                                            </asp:Label>
                                        </td>
                                        <td colspan="2" style="width: 781px">
                                            <dxe:ASPxCheckBox ID="chkAgua_Hidrometro" runat="server" ValueChecked="S" ValueUnchecked="N"
                                                ValueType="System.String" Checked="false">
                                            </dxe:ASPxCheckBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlOutrosSuprimento" runat="server" GroupingText="Outros Suprimentos"
                                Width="911px">
                                <table>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblPoco" runat="server" Text="Poço:" Width="93px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBoxList ID="chkPoco" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text="Artesiano" Value="Artesiano"></asp:ListItem>
                                                <asp:ListItem Text="Semi-Artesiano" Value="Semi-Artesiano"></asp:ListItem>
                                                <asp:ListItem Text="Cacimba/Cisterna" Value="Cacimba/Cisterna"></asp:ListItem>
                                                <asp:ListItem Text="Carro-Pipa" Value="Carro-Pipa"></asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style1" style="text-align: right;">
                                            Fornece água potável para o consumo humano:
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rblTipoAguaConsumida" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="Filtrada">Sim</asp:ListItem>
                                                <asp:ListItem Value="Não Filtrada">Não</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;" class="style1">
                                            <asp:Label ID="lblAgPoco_Vazao" runat="server" Text="Vazão:" Width="120px">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAgPoco_Vazao" runat="server" MaxLength="4" Width="100px" SkinID="numerico">
                                            </asp:TextBox>
                                            <asp:Label ID="lblIH_1" runat="server" Text="l/h" Width="10px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;" class="style1">
                                            <asp:Label ID="lblAgPoco_Bomba" runat="server" Text="Bomba Submersa:" Width="120px">
                                            </asp:Label>
                                        </td>
                                        <td colspan="2">
                                            <dxe:ASPxCheckBox ID="cbAgPoco_Bomba" runat="server" ValueChecked="S" ValueUnchecked="N"
                                                ValueType="System.String" Checked="false">
                                            </dxe:ASPxCheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;" class="style1">
                                            <asp:Label ID="lblAgPoco_Profundidade" runat="server" Text="Profundidade:" Width="120px">
                                            </asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAgPoco_Profundidade" runat="server" MaxLength="4" Width="100px"
                                                SkinID="numerico">
                                            </asp:TextBox>
                                            <asp:Label ID="lblM3_5" runat="server" Text="m" Width="10px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlSuprimentoGas" runat="server" GroupingText="Suprimento de Gás"
                            Width="931px">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblGas_Concessionaria" runat="server" Text="Concessionária: " Width="120px">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearchBox ID="tseGas_Concessionaria" runat="server" Key="empresa" Argument="razao_social"
                                            SqlSelect="SELECT empresa, razao_social, nome FROM ly_empresa" SqlOrder="razao_social"
                                            Value='<%# Bind("empresa") %>' MaxLength="20" AutoPostBack="false">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código da Empresa" FieldName="empresa" Width="150px" />
                                                <tweb:TSearchBoxColumn Caption="Razão Social" FieldName="razao_social" Width="300px" />
                                                <tweb:TSearchBoxColumn Caption="Nome da Empresa" FieldName="nome" Width="300px" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblGas_NumeroCliente" runat="server" Text="Número do Cliente: " Width="120px">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGas_NumeroCliente" runat="server" Width="200px" MaxLength="100"
                                            SkinID="numerico">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblGas_Tipo" runat="server" Text="Tipo: " Width="120px">
                                        </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlGas_Tipo" runat="server" DataTextField="descr" DataValueField="item"
                                            Width="300px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlEsgoto" runat="server" GroupingText="Esgoto/Destinação do lixo"
                            Width="931px">
                            <asp:Panel ID="Panel3" runat="server" GroupingText="Esgoto Sanitário">
                                <table>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:CheckBoxList ID="chkEsgoto" OnSelectedIndexChanged="chkEsgoto_SelectedIndexChanged"
                                                AutoPostBack="true" runat="server" RepeatDirection="Horizontal" Style="text-align: left">
                                                <asp:ListItem>Rede Pública/Concessionária</asp:ListItem>
                                                <asp:ListItem>Fossa Séptica</asp:ListItem>
                                                <asp:ListItem>Fossa Rudimentar/Comum</asp:ListItem>
                                                <asp:ListItem>Inexistente</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlLixo" runat="server" GroupingText="Destinação do Lixo">
                                <table>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:CheckBoxList ID="chkLixo" runat="server" RepeatDirection="Horizontal" TextAlign="Right"
                                                Style="text-align: left">
                                                <asp:ListItem>Coleta Periódica</asp:ListItem>
                                                <asp:ListItem>Queima</asp:ListItem>
                                                <asp:ListItem>Joga em outra área</asp:ListItem>
                                                <asp:ListItem>Leva a uma destinação final licenciada pelo poder público </asp:ListItem>
                                                <asp:ListItem>Enterra</asp:ListItem>
                                                <asp:ListItem>Outros</asp:ListItem>
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlTratamentoLixo" runat="server" GroupingText="Tratamento do Lixo">
                                <table>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:CheckBoxList ID="chkTratamentoLixo" OnSelectedIndexChanged="chkTratamentoLixo_SelectedIndexChanged"
                                                AutoPostBack="true" runat="server" RepeatDirection="Horizontal" Style="text-align: left">
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:Panel>
                        <br />
                        <table>
                            <tr>
                                <td style="text-align: right;">
                                    <asp:Button ID="btnSalvarConcessionaria" runat="server" Text="Confirmar" OnClick="btnSalvarConcessionaria_Click" />
                                </td>
                            </tr>
                        </table>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Demais Dependências">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl8" runat="server">
                        <asp:Panel ID="pnlDependencia" runat="server" GroupingText="Demais Dependências">
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlSalaAlternativa" runat="server" GroupingText="Sala Alternativa">
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlCondicoesSala" runat="server" GroupingText="Condições das salas de aula utilizadas na escola (dentro e fora do prédio)">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:TextBox ID="txtQtdSalaClimatizada" runat="server" Width="20px" MaxLength="3"
                                            SkinID="numerico">
                                        </asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label14" runat="server" Text="Salas de aula climatizadas">
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:TextBox ID="txtQtdSalaAcessibilidade" runat="server" Width="20px" MaxLength="3"
                                            SkinID="numerico">
                                        </asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label15" runat="server" Text="Salas de aula com acessibilidade para pessoas
com deficiência ou mobilidade reduzida">
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:TextBox ID="txtQtdCantinhoLeitura" runat="server" Width="20px" MaxLength="3"
                                            SkinID="numerico">
                                        </asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label38" runat="server" Text="Salas de aula com Cantinho de Leitura para Educação Infantil e o Ensino Fundamental(Anos Iniciais)">
                                        </asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Label ID="lblDataAlteracao" runat="server" SkinID="lblMensagem" Visible="false"></asp:Label>
                        <asp:Label ID="lblInfoValidacao" runat="server" SkinID="lblMensagem" Visible="false"></asp:Label>
                        <br />
                        <br />
                        <asp:Button ID="btnSalvarQtdDependencias" runat="server" ValidationGroup="SalvarForm"
                            Text="Salvar Informações" OnClick="btnSalvarQtdDependencias_Click" Visible="false" />
                        <asp:Button ID="btnValidarDependencias" runat="server" ValidationGroup="SalvarForm"
                            Text="Validar Informações" OnClientClick="return ConfirmaValidacao();" OnClick="btnValidarDependencias_Click"
                            Visible="false" />
                        <asp:Button ID="btnInvalidarDependencias" runat="server" ValidationGroup="SalvarForm"
                            Text="Não Validar Informações" OnClientClick="return ConfirmaNaoValidacao();"
                            OnClick="btnInvalidarDependencias_Click" Visible="false" />
                        <asp:Button ID="btnReabrirValidacao" runat="server" ValidationGroup="SalvarForm"
                            Text="Reabrir Dependência" OnClientClick="return ConfirmaReaberturaDependencia();"
                            OnClick="btnReabrirValidacao_Click" Visible="false" />
                        <br />
                        <asp:ObjectDataSource ID="odsDemaisDependencias" TypeName="Techne.Lyceum.Net.Basico.Unidade"
                            SelectMethod="ListarSalaRecurso" runat="server" DeleteMethod="Delete" UpdateMethod="Update"
                            InsertMethod="Insert" OnDeleting="odsDemaisDependencias_Deleting" OnInserting="odsDemaisDependencias_Inserting"
                            OnUpdating="odsDemaisDependencias_Updating">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseUnidade" DefaultValue="" Name="unidade" PropertyName="DBValue" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:ObjectDataSource ID="odsEdificacoes2" runat="server" TypeName="Techne.Lyceum.RN.UnidadeFisicaEdificacao"
                            SelectMethod="ConsultarEdificacoes">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseUnidade" PropertyName="Value" Name="unidade" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <dxwgv:ASPxGridView ID="grdDemaisDependencias" runat="server" DataSourceID="odsDemaisDependencias"
                            KeyFieldName="CompositeKey" AutoGenerateColumns="False" Width="1100px" ClientInstanceName="grdDemaisDependencias"
                            Font-Size="Small" OnCellEditorInitialize="grdDemaisDependencias_CellEditorInitialize"
                            OnCustomUnboundColumnData="grdDependencias_CustomUnboundColumnData" OnRowDeleting="grdDependencias_RowDeleting"
                            OnRowUpdating="grdDependencias_RowUpdating" OnRowInserting="grdDependencias_RowInserting"
                            OnInitNewRow="grdDemaisDependencias_InitNewRow" OnStartRowEditing="grdDemaisDependencias_StartRowEditing"
                            OnAfterPerformCallback="grdDemaisDependencias_AfterPerformCallback" OnCustomColumnDisplayText="grdDependencias_CustomColumnDisplayText">
                            <SettingsPager>
                                <Summary AllPagesText="Páginas: {0} - {1} ({2} items)" Text="Página {0} of {1} ({2} items)" />
                            </SettingsPager>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                            <SettingsBehavior ConfirmDelete="true" AllowMultiSelection="False" AllowDragDrop="false" />
                            <SettingsText ConfirmDelete="Confirma a remoção ?" EmptyDataRow="Não existem dados." />
                            <SettingsEditing Mode="EditForm" />
                            <Templates>
                                <EditForm>
                                    <dxw:ContentControl ID="cntTemplateDependencia" runat="server">
                                        <div style="padding: 4px;">
                                            <table>
                                                <tr>
                                                    <td style="text-align: right">
                                                        <asp:Label ID="lblTempDependencia" runat="server" Text="Dependência:* " SkinID="lblObrigatorio"
                                                            Width="150px">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <dxwgv:ASPxGridViewTemplateReplacement ID="TempDependencia" runat="server" ColumnID="dependencia"
                                                            ReplacementType="EditFormCellEditor">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">
                                                        <asp:Label ID="lblTempDescricao" runat="server" Text="Descrição: " Width="150px">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <dxwgv:ASPxGridViewTemplateReplacement ID="TempDescricao" runat="server" ColumnID="descricao"
                                                            ReplacementType="EditFormCellEditor">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">
                                                        <asp:Label ID="lblTempAtiva" runat="server" Text="Ativa? " Width="150px">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <dxwgv:ASPxGridViewTemplateReplacement ID="TempAtiva" runat="server" ColumnID="ativa"
                                                            ReplacementType="EditFormCellEditor">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">
                                                        <asp:Label ID="Label13" runat="server" Text="Sala Anexa? " Width="150px">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <dxwgv:ASPxGridViewTemplateReplacement ID="ASPxGridViewTemplateReplacement2" runat="server"
                                                            ColumnID="sala_anexa" ReplacementType="EditFormCellEditor">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">
                                                        <asp:Label ID="lblTempTipo" runat="server" Text="Tipo:* " SkinID="lblObrigatorio"
                                                            Width="150px">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <dxwgv:ASPxGridViewTemplateReplacement ID="TempTipo" runat="server" ColumnID="tipo_depend"
                                                            ReplacementType="EditFormCellEditor">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">
                                                        <asp:Label ID="lblTempArea" runat="server" Text="Área (m²): " Width="150px">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <dxwgv:ASPxGridViewTemplateReplacement ID="TempArea" runat="server" ColumnID="area"
                                                            ReplacementType="EditFormCellEditor">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="Label1" runat="server" Text="Edificação: *" SkinID="lblObrigatorio"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <dxwgv:ASPxGridViewTemplateReplacement ID="TempEdificacao" runat="server" ColumnID="edificacao"
                                                            ReplacementType="EditFormCellEditor">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="lblTempPavimento" runat="server" Text="Pavimento: *" SkinID="lblObrigatorio"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <dxwgv:ASPxGridViewTemplateReplacement ID="TempPavimento" runat="server" ColumnID="pavimento"
                                                            ReplacementType="EditFormCellEditor">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">
                                                        <asp:Label ID="lblTempObservacao" runat="server" Text="Observação: " Width="150px">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <dxwgv:ASPxGridViewTemplateReplacement ID="TempObservacao" runat="server" ColumnID="obs"
                                                            ReplacementType="EditFormCellEditor">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                </tr>
                                            </table>
                                            <dxwgv:ASPxGridViewTemplateReplacement ID="repUpdate" runat="server" ReplacementType="EditFormUpdateButton">
                                            </dxwgv:ASPxGridViewTemplateReplacement>
                                            <dxwgv:ASPxGridViewTemplateReplacement ID="repCancel" runat="server" ReplacementType="EditFormCancelButton">
                                            </dxwgv:ASPxGridViewTemplateReplacement>
                                        </div>
                                    </dxw:ContentControl>
                                </EditForm>
                            </Templates>
                            <Columns>
                                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="50px">
                                    <HeaderCaptionTemplate>
                                        <div style="text-align: center">
                                            <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                                onclick="grdDemaisDependencias.AddNewRow();" alt="Novo" />
                                        </div>
                                    </HeaderCaptionTemplate>
                                    <EditButton Text="Editar" Visible="True">
                                        <Image Url="~/img/bt_editar.png" />
                                    </EditButton>
                                    <DeleteButton Text="Remover" Visible="True">
                                        <Image Url="~/img/bt_exclui2.png" />
                                    </DeleteButton>
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
                                <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="faculdade" VisibleIndex="1"
                                    Visible="false">
                                    <PropertiesTextEdit MaxLength="20" Width="150px">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor entre com a Faculdade" IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Dependência*" FieldName="dependencia" VisibleIndex="2"
                                    Width="100px" HeaderStyle-Font-Bold="true">
                                    <PropertiesTextEdit MaxLength="20" Width="150px">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor entre com a Dependência" IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesTextEdit>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Descrição" FieldName="descricao" VisibleIndex="3"
                                    Width="150px">
                                    <PropertiesTextEdit MaxLength="255" Width="300px">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataCheckColumn Caption="Ativa?" FieldName="ativa" VisibleIndex="4"
                                    Width="50px" HeaderStyle-Font-Bold="true">
                                    <PropertiesCheckEdit ValueChecked="S" ValueUnchecked="N" ValueType="System.String">
                                    </PropertiesCheckEdit>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataCheckColumn>
                                <dxwgv:GridViewDataCheckColumn Caption="Sala Anexa?" FieldName="sala_anexa" VisibleIndex="4"
                                    Width="50px" HeaderStyle-Font-Bold="true">
                                    <PropertiesCheckEdit ValueChecked="S" ValueUnchecked="N" ValueType="System.String">
                                    </PropertiesCheckEdit>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataCheckColumn>
                                <dxwgv:GridViewDataComboBoxColumn Caption="Tipo*" FieldName="tipo_depend" VisibleIndex="5"
                                    Width="100px" HeaderStyle-Font-Bold="true">
                                    <PropertiesComboBox ValueType="System.String" Width="150px" ClientInstanceName="cmbtipo_depend">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField IsRequired="true" ErrorText="Favor inserir o Tipo." />
                                        </ValidationSettings>
                                    </PropertiesComboBox>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Área (m²)" FieldName="area" VisibleIndex="6"
                                    Width="70px">
                                    <CellStyle HorizontalAlign="Left">
                                    </CellStyle>
                                    <PropertiesTextEdit MaxLength="3" Style-HorizontalAlign="Left" Width="150px">
                                        <ClientSideEvents KeyPress="function(s,e){ SomentePermitirNumeros(s, e.htmlEvent); } " />
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RegularExpression ErrorText="Capacidade deve ser um número de até 3 dígitos." ValidationExpression="\d{0,3}" />
                                        </ValidationSettings>
                                        <Style HorizontalAlign="Left">
                                            </Style>
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataComboBoxColumn VisibleIndex="7" Caption="Edificação*" FieldName="edificacao"
                                    Width="115px">
                                    <PropertiesComboBox TextField="nome_edificacao" ValueField="edificacao" EnableSynchronization="False"
                                        EnableIncrementalFiltering="True" DataSourceID="odsEdificacoes2">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnEdificacao2Changed(s); }">
                                        </ClientSideEvents>
                                    </PropertiesComboBox>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataComboBoxColumn VisibleIndex="8" Caption="Pavimento*" FieldName="pavimento"
                                    Visible="false">
                                    <PropertiesComboBox EnableSynchronization="False" EnableIncrementalFiltering="True">
                                    </PropertiesComboBox>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Pavimento*" FieldName="nome_pavimento" VisibleIndex="9"
                                    Width="115px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Observação" FieldName="obs" VisibleIndex="10"
                                    Width="300px">
                                    <PropertiesTextEdit MaxLength="2000" Width="300px">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                                    Visible="False" VisibleIndex="11">
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                        <br />
                        <br />
                        <asp:ObjectDataSource ID="odsSalaAlternativas" TypeName="Techne.Lyceum.Net.Basico.Unidade"
                            SelectMethod="ListarSalasAlternativas" runat="server" OnInserting="odsSalaAlternativas_Inserting"
                            InsertMethod="InsertSalaAlternativa" OnUpdating="odsSalaAlternativas_Updating"
                            UpdateMethod="UpdateSalaAlternativa">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseUnidade" DefaultValue="" Name="unidade" PropertyName="DBValue" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:ObjectDataSource ID="odsTipoSala" runat="server" TypeName="Techne.Lyceum.RN.Dependencia"
                            SelectMethod="BuscaTipoSala"></asp:ObjectDataSource>
                        <dxwgv:ASPxGridView ID="grdSalaAlternativa" runat="server" DataSourceID="odsSalaAlternativas"
                            KeyFieldName="CompositeKey" ClientInstanceName="grdSalaAlternativa" AutoGenerateColumns="false"
                            Width="850px" OnRowInserting="grdSalaAlternativa_RowInserting" OnCellEditorInitialize="grdSalaAlternativa_CellEditorInitialize"
                            OnCustomUnboundColumnData="grdSalaAlternativa_CustomUnboundColumnData" OnRowUpdating="grdSalaAlternativa_RowUpdating"
                            OnInitNewRow="grdSalaAlternativa_InitNewRow">
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                            <SettingsBehavior ConfirmDelete="true" AllowMultiSelection="False" AllowDragDrop="false" />
                            <SettingsText ConfirmDelete="Confirma a remoção ?" EmptyDataRow="Não existem dados." />
                            <SettingsEditing Mode="Inline" />
                            <Columns>
                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                                    <HeaderCaptionTemplate>
                                        <div style="text-align: center">
                                            <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                                onclick="grdSalaAlternativa.AddNewRow();" alt="Novo" />
                                        </div>
                                    </HeaderCaptionTemplate>
                                    <EditButton Text="Editar" Visible="true">
                                        <Image Url="~/img/bt_editar.png" />
                                    </EditButton>
                                    <CancelButton Text="Cancelar">
                                        <Image Url="~/img/bt_cancelar.png" />
                                    </CancelButton>
                                    <UpdateButton Text="Atualizar">
                                        <Image Url="~/img/bt_salvar.png" />
                                    </UpdateButton>
                                    <ClearFilterButton Text="Limpar" Visible="true">
                                        <Image Url="~/img/bt_limpa.png" />
                                    </ClearFilterButton>
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataTextColumn VisibleIndex="1" Caption="Dependencia" FieldName="DEPENDENCIA"
                                    Width="150px">
                                    <PropertiesTextEdit MaxLength="20">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor informar a Dependencia" IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn VisibleIndex="2" Caption="Descrição" FieldName="DESCRICAO"
                                    Width="250">
                                    <PropertiesTextEdit MaxLength="100">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataComboBoxColumn Caption="Tipo*" FieldName="TIPO_DEPEND" VisibleIndex="4"
                                    Width="200px" HeaderStyle-Font-Bold="true">
                                    <PropertiesComboBox DataSourceID="odsTipoSala" TextField="NOME" ValueField="LYTIPODEPENDENCIAID"
                                        ValueType="System.String" Width="200px" MaxLength="40">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField IsRequired="true" ErrorText="Favor selecionar o Tipo." />
                                        </ValidationSettings>
                                    </PropertiesComboBox>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataTextColumn VisibleIndex="4" Caption="Capacidade Máxima de Alunos*"
                                    FieldName="NUM_ALUNOS" Width="280">
                                    <PropertiesTextEdit MaxLength="100">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor informar o nome do Capacidade Máxima de Alunos" IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataCheckColumn VisibleIndex="5" Caption="Ativo*" FieldName="ATIVA"
                                    Width="130" HeaderStyle-Font-Bold="true">
                                    <PropertiesCheckEdit ValueChecked="S" ValueUnchecked="N" ValueType="System.String">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor informe se Ativo" IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesCheckEdit>
                                </dxwgv:GridViewDataCheckColumn>
                                <dxwgv:GridViewDataCheckColumn VisibleIndex="5" Caption="Sala Anexa" FieldName="SALA_ANEXA"
                                    Width="130" HeaderStyle-Font-Bold="true">
                                    <PropertiesCheckEdit ValueChecked="S" ValueUnchecked="N" ValueType="System.String">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor informe se Ativo" IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesCheckEdit>
                                </dxwgv:GridViewDataCheckColumn>
                                <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                                    Visible="False" VisibleIndex="8">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="FACULDADE" VisibleIndex="9" Caption="FACULDADE"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                        <dxwgv:ASPxGridView ID="grdBanheiroeVestiario" runat="server" DataSourceID="odsBanheiroeVestiario"
                            KeyFieldName="CompositeKey" AutoGenerateColumns="False" Width="1100px" ClientInstanceName="grdBanheiroeVestiario"
                            Font-Size="Small" OnCellEditorInitialize="grdBanheiroeVestiario_CellEditorInitialize"
                            OnCustomUnboundColumnData="grdBanheiroeVestiario_CustomUnboundColumnData" OnRowDeleting="grdBanheiroeVestiario_RowDeleting"
                            OnRowUpdating="grdBanheiroeVestiario_RowUpdating" OnRowInserting="grdBanheiroeVestiario_RowInserting"
                            OnInitNewRow="grdBanheiroeVestiario_InitNewRow" OnStartRowEditing="grdBanheiroeVestiario_StartRowEditing"
                            OnAfterPerformCallback="grdBanheiroeVestiario_AfterPerformCallback" OnCustomColumnDisplayText="grdBanheiroeVestiario_CustomColumnDisplayText">
                            <SettingsPager>
                                <Summary AllPagesText="Páginas: {0} - {1} ({2} items)" Text="Página {0} of {1} ({2} items)" />
                            </SettingsPager>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                            <SettingsBehavior ConfirmDelete="true" AllowMultiSelection="False" AllowDragDrop="false" />
                            <SettingsText ConfirmDelete="Confirma a remoção ?" EmptyDataRow="Não existem dados." />
                            <SettingsEditing Mode="EditForm" />
                            <Templates>
                                <EditForm>
                                    <dxw:ContentControl ID="cntTemplateDependencia2" runat="server">
                                        <div style="padding: 4px;">
                                            <table>
                                                <tr>
                                                    <td style="text-align: right">
                                                        <asp:Label ID="lblTempDependencia2" runat="server" Text="Dependência:* " SkinID="lblObrigatorio"
                                                            Width="150px">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <dxwgv:ASPxGridViewTemplateReplacement ID="TempDependencia2" runat="server" ColumnID="dependencia"
                                                            ReplacementType="EditFormCellEditor">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">
                                                        <asp:Label ID="lblTempDescricao" runat="server" Text="Descrição: " Width="150px">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <dxwgv:ASPxGridViewTemplateReplacement ID="TempDescricao2" runat="server" ColumnID="descricao"
                                                            ReplacementType="EditFormCellEditor">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">
                                                        <asp:Label ID="lblTempAtiva2" runat="server" Text="Ativa? " Width="150px">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <dxwgv:ASPxGridViewTemplateReplacement ID="TempAtiva2" runat="server" ColumnID="ativa"
                                                            ReplacementType="EditFormCellEditor">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right">
                                                        <asp:Label ID="lblTempTipo2" runat="server" Text="Tipo:* " SkinID="lblObrigatorio"
                                                            Width="150px">
                                                        </asp:Label>
                                                    </td>
                                                    <td>
                                                        <dxwgv:ASPxGridViewTemplateReplacement ID="TempTipo2" runat="server" ColumnID="tipo_depend"
                                                            ReplacementType="EditFormCellEditor">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="lblTempPavimento" runat="server" Text="Bloco: *" SkinID="lblObrigatorio"
                                                            Width="150px"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <dxwgv:ASPxGridViewTemplateReplacement ID="TempPavimento2" runat="server" ColumnID="edificacao"
                                                            ReplacementType="EditFormCellEditor">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align: right;">
                                                        <asp:Label ID="Label1_" runat="server" Text="Andar: *" SkinID="lblObrigatorio" Width="150px"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <dxwgv:ASPxGridViewTemplateReplacement ID="TempEdificacao2" runat="server" ColumnID="pavimento"
                                                            ReplacementType="EditFormCellEditor">
                                                        </dxwgv:ASPxGridViewTemplateReplacement>
                                                    </td>
                                                </tr>
                                            </table>
                                            <dxwgv:ASPxGridViewTemplateReplacement ID="repUpdate2" runat="server" ReplacementType="EditFormUpdateButton">
                                            </dxwgv:ASPxGridViewTemplateReplacement>
                                            <dxwgv:ASPxGridViewTemplateReplacement ID="repCancel2" runat="server" ReplacementType="EditFormCancelButton">
                                            </dxwgv:ASPxGridViewTemplateReplacement>
                                        </div>
                                    </dxw:ContentControl>
                                </EditForm>
                            </Templates>
                            <Columns>
                                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="50px">
                                    <HeaderCaptionTemplate>
                                        <div style="text-align: center">
                                            <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                                onclick="grdBanheiroeVestiario.AddNewRow();" alt="Novo" />
                                        </div>
                                    </HeaderCaptionTemplate>
                                    <EditButton Text="Editar" Visible="True">
                                        <Image Url="~/img/bt_editar.png" />
                                    </EditButton>
                                    <DeleteButton Text="Remover" Visible="True">
                                        <Image Url="~/img/bt_exclui2.png" />
                                    </DeleteButton>
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
                                <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="faculdade" VisibleIndex="7"
                                    Visible="false">
                                    <PropertiesTextEdit MaxLength="20" Width="150px">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor entre com a Faculdade" IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Dependência*" FieldName="dependencia" VisibleIndex="1"
                                    Width="100px" HeaderStyle-Font-Bold="true">
                                    <PropertiesTextEdit MaxLength="20" Width="150px">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor entre com a Dependência" IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesTextEdit>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Descrição" FieldName="descricao" VisibleIndex="2"
                                    Width="150px">
                                    <PropertiesTextEdit MaxLength="255" Width="300px">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataComboBoxColumn Caption="Tipo*" FieldName="tipo_depend" VisibleIndex="3"
                                    Width="150px" HeaderStyle-Font-Bold="true">
                                    <PropertiesComboBox ValueType="System.String" Width="150px" ClientInstanceName="cmbtipo_depend"
                                        TextField="NOME" ValueField="TIPO_DEPEND" DataSourceID="odsTipoBanheiro">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField IsRequired="true" ErrorText="Favor inserir o Tipo." />
                                        </ValidationSettings>
                                    </PropertiesComboBox>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataComboBoxColumn VisibleIndex="4" Caption="Bloco*" FieldName="edificacao"
                                    Width="115px">
                                    <PropertiesComboBox TextField="nome_edificacao" ValueField="edificacao" EnableSynchronization="False"
                                        EnableIncrementalFiltering="True" DataSourceID="odsEdificacoes2">
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnEdificacao2Changed_banheiro(s); }">
                                        </ClientSideEvents>
                                    </PropertiesComboBox>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Andar*" FieldName="nome_pavimento" VisibleIndex="5"
                                    Width="115px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataCheckColumn Caption="Ativa?" FieldName="ativa" VisibleIndex="6"
                                    Width="50px" HeaderStyle-Font-Bold="true">
                                    <PropertiesCheckEdit ValueChecked="S" ValueUnchecked="N" ValueType="System.String">
                                    </PropertiesCheckEdit>
                                    <HeaderStyle Font-Bold="True"></HeaderStyle>
                                </dxwgv:GridViewDataCheckColumn>
                                <dxwgv:GridViewDataComboBoxColumn VisibleIndex="8" Caption="Pavimento*" FieldName="pavimento"
                                    Visible="false" Width="50px">
                                    <PropertiesComboBox EnableSynchronization="False" EnableIncrementalFiltering="True">
                                    </PropertiesComboBox>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                                    Visible="false" VisibleIndex="11">
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                        <asp:ObjectDataSource ID="odsTipoBanheiro" runat="server" TypeName="Techne.Lyceum.RN.TipoDependencia"
                            SelectMethod="ListarBanheiros"></asp:ObjectDataSource>
                        <asp:ObjectDataSource ID="odsBanheiroeVestiario" TypeName="Techne.Lyceum.Net.Basico.Unidade"
                            SelectMethod="ListarBanheiro" runat="server" DeleteMethod="Delete" UpdateMethod="Update_banheiro"
                            InsertMethod="Insert_banheiro" OnDeleting="odsBanheiroeVestiario_Deleting" OnInserting="odsBanheiroeVestiario_Inserting"
                            OnUpdating="odsBanheiroeVestiario_Updating">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseUnidade" DefaultValue="" Name="unidade" PropertyName="DBValue" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Equipamentos na Unidade">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl9" runat="server">
                        <asp:Panel ID="pnlEquipamentos" runat="server" GroupingText="Equipamentos Unidade">
                        </asp:Panel>
                        <br />
                        <asp:Label ID="lblDataAlteracaoEquip" runat="server" SkinID="lblMensagem" Visible="false"></asp:Label>
                        <br />
                        <asp:Button ID="btnSalvarQtdEquipamentos" runat="server" ValidationGroup="SalvarForm"
                            Text="Salvar Informações" OnClick="btnSalvarQtdEquipamentos_Click" Visible="false" />
                        <br />
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Internet">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl10" runat="server">
                        <asp:Panel ID="pnlInternet" runat="server">
                            <asp:Panel ID="Panel6" runat="server" GroupingText="Possui Internet Banda Larga?">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:RadioButtonList ID="rblInternetBandaLarga" AutoPostBack="true" runat="server"
                                                RepeatDirection="Horizontal" OnSelectedIndexChanged="rblInternetBandaLarga_SelectedIndexChanged">
                                                <asp:ListItem Value="S">Sim</asp:ListItem>
                                                <asp:ListItem Value="N">Não</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlDadosInternet" runat="server" Visible="false">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="Panel7" runat="server" GroupingText="Acesso à internet">
                                                <asp:CheckBoxList ID="chkAcessoInternet" OnSelectedIndexChanged="chkAcessoInternet_SelectedIndexChanged"
                                                    AutoPostBack="true" RepeatColumns="2" CellSpacing="2" runat="server" RepeatDirection="vertical"
                                                    Style="text-align: left">
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel ID="Panel8" runat="server" GroupingText="Equipamentos que os alunos usam para acessar a Internet da escola"
                                    Enabled="false">
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
                                    Enabled="false">
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
                            <asp:Button ID="btnSalvarInternet" runat="server" ValidationGroup="SalvarForm" Text="Salvar Informações"
                                OnClick="btnSalvarInternet_Click" />
                        </asp:Panel>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Pedagógicos">
                <ContentCollection>
                    <dxw:ContentControl ID="conPedagogicos" runat="server">
                        <asp:Panel ID="pnlPedagogico" runat="server">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlMaterialPedagogico" runat="server" GroupingText="Instrumentos, materiais socioculturais e/ou pedagógicos em uso na escola para o desenvolvimento de atividades                           
                                        
de ensino aprendizagem">
                                            <asp:CheckBoxList ID="chkMaterialPedagogico" OnSelectedIndexChanged="chkMaterialPedagogico_SelectedIndexChanged"
                                                AutoPostBack="true" RepeatColumns="2" CellSpacing="10" runat="server" RepeatDirection="vertical"
                                                Style="text-align: left">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlOrgaosColegiados" runat="server" GroupingText="Órgãos colegiados em funcionamento na escola"
                                            Width="100%">
                                            <asp:CheckBoxList ID="chkOrgaoColegiado" OnSelectedIndexChanged="chkOrgaoColegiado_SelectedIndexChanged"
                                                AutoPostBack="true" runat="server" RepeatDirection="vertical" Style="text-align: left"
                                                RepeatColumns="2" CellSpacing="10">
                                            </asp:CheckBoxList>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label16" runat="server" Text="  A escola possui site ou blog ou página em redes sociais para comunicação institucional?"
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButtonList ID="rblPossuiSite" AutoPostBack="true" runat="server" RepeatDirection="Horizontal"
                                            OnSelectedIndexChanged="rblPossuiSite_SelectedIndexChanged">
                                            <asp:ListItem Value="S">Sim</asp:ListItem>
                                            <asp:ListItem Value="N">Não</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSiteBlog" runat="server" Text="Site/Blog:* " SkinID="lblObrigatorio"
                                            Visible="false"></asp:Label>
                                        <asp:TextBox ID="txtSiteBlog" runat="server" MaxLength="500" Width="250px" Visible="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label17" runat="server" Text="A escola usa espaços e equipamentos do entorno escolar para atividades regulares
                                        com os(as) alunos(as)?" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButtonList ID="rblEspacoEquipamentoEntorno" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="S">Sim</asp:ListItem>
                                            <asp:ListItem Value="N">Não</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label18" runat="server" Text="A escola possui projeto político pedagógico ou a proposta pedagógica da escola (conforme
                                        art. 12 da LDB)?" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButtonList ID="rblPossuiProjeto" runat="server" RepeatDirection="Horizontal"
                                            AutoPostBack="true" OnSelectedIndexChanged="rblPossuiProjeto_SelectedIndexChanged">
                                            <asp:ListItem Value="S">Sim</asp:ListItem>
                                            <asp:ListItem Value="N">Não</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlCumpriuProjeto" runat="server" Visible="false">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label19" runat="server" Text="O projeto político pedagógico ou a proposta pedagógica da escola (conforme art.
                                                        12 da LDB) foi atualizada nos últimos 12 meses até a data de referência?" SkinID="lblObrigatorio"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButtonList ID="rblCumpriuProjetoPedagogico" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Value="S">Sim</asp:ListItem>
                                                            <asp:ListItem Value="N">Não</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label20" runat="server" Text="A escola compartilha espaços para atividades de integração escola-comunidade"
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButtonList ID="rblCompartilhaEspacoComunidade" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="S">Sim</asp:ListItem>
                                            <asp:ListItem Value="N">Não</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="Panel10" runat="server" GroupingText="Ações na Área de Educação Ambiental"
                                Width="600px">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label31" runat="server" Text="A escola desenvolve ações na área de educação ambiental?"
                                                SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButtonList ID="rblEducacaoambiental" runat="server" RepeatDirection="Horizontal"
                                                AutoPostBack="true" OnSelectedIndexChanged="rblEducacaoambiental_SelectedIndexChanged">
                                                <asp:ListItem Value="S">Sim</asp:ListItem>
                                                <asp:ListItem Value="N">Não</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label32" runat="server" Text="Como conteúdo dos componentes/campos de experiências presentes no currículo"
                                                SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButtonList ID="rblConteudoComponentes" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="S">Sim</asp:ListItem>
                                                <asp:ListItem Value="N">Não</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label33" runat="server" Text="Como um componente curricular especial, específico, flexível ou eletivo"
                                                SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButtonList ID="rblComponenteCurricular" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="S">Sim</asp:ListItem>
                                                <asp:ListItem Value="N">Não</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label34" runat="server" Text="Como um eixo estruturante do currículo"
                                                SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButtonList ID="rblEixoEstuturante" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="S">Sim</asp:ListItem>
                                                <asp:ListItem Value="N">Não</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label35" runat="server" Text="Em eventos" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButtonList ID="rblEmEventos" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="S">Sim</asp:ListItem>
                                                <asp:ListItem Value="N">Não</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label36" runat="server" Text="Em projetos transversais ou interdisciplinares"
                                                SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButtonList ID="rblProjetosTransversais" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Value="S">Sim</asp:ListItem>
                                                <asp:ListItem Value="N">Não</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label37" runat="server" Text="Nenhuma das opções listadas" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButtonList ID="rblNOL" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                                OnSelectedIndexChanged="rblNOL_SelectedIndexChanged">
                                                <asp:ListItem Value="S">Sim</asp:ListItem>
                                                <asp:ListItem Value="N">Não</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <br />
                            <asp:Button ID="btnSalvarPedagogicos" runat="server" ValidationGroup="SalvarForm"
                                Text="Salvar Informações" OnClick="btnSalvarPedagogicos_Click" />
                        </asp:Panel>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
    <dxpc:ASPxPopupControl ID="pucConfirmarEquipamentos" ClientInstanceName="pucConfirmarEquipamentos"
        runat="server" Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false"
        ShowCloseButton="false" ShowFooter="false" ShowHeader="false" ShowSizeGrip="False"
        EnableAnimation="false" Width="400px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <table>
                    <tr>
                        <td>
                            Alguma(s) quantidade(s) precisa(m) de verificação.
                            <br />
                            Confirma a gravação de:
                        </td>
                    </tr>
                    <tr runat="server" id="trMaximos">
                        <td>
                            <asp:BulletedList ID="blMaximos" runat="server">
                            </asp:BulletedList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblConfirmar" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Button ID="btnConfirmarEquipamentos" runat="server" Text="Confirmar" OnClick="btnConfirmarEquipamentos_Click" />
                            <asp:Button ID="btnCancelarEquipamentos" runat="server" Text="Cancelar" OnClientClick="pucConfirmarEquipamentos.Hide(); return false;" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
