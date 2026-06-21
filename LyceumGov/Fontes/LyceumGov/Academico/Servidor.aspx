<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Servidor.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.Servidor" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<asp:Content ID="conServidor" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">

        $().ready(function() {
            trataCep({ tscep: '<%=tsCEP.ClientID %>',
                cep: '<%=txtCEPResid.ClientID %>',
                nomeLogradouro: '<%=txtEnderecoResid.ClientID %>',
                nomeBairro: '<%=txtBairroResid.ClientID %>',
                nomeMunicipio: '<%=txtMunicipio.ClientID %>',
                codigoMunicipio: '<%=tseMunicipioResid.ClientID %>',
                uf: '<%=txtEstado.ClientID %>'
            });

        });

        function ExecutarPostBack() {
            __doPostBack('<%=btnPostBack.UniqueID%>', '');

        }
        function OnEndCallback(s) {
            if (s.cpAtualizar != null) {
                $("#<%= this.lblMsg.ClientID %>").text(s.cpAtualizar);
                s.cpAtualizar = null;
            }

        }


        function confirmSalvar() {

            if (Page_ClientValidate("SalvarForm")) {
                var campos = "";
                var telefone = document.getElementById("<%=txtTelefone.ClientID %>");
                var rg = document.getElementById("<%=txtDOC_Rg_Num.ClientID %>");
                var reservista = document.getElementById("<%=txtDMIL_Cr_Num.ClientID %>");
                var titulo = document.getElementById("<%=txtDOC_Teleitor_Num.ClientID %>");
                var contadorCamposVazios = 0;

                if (typeof telefone != 'undefined' && telefone != null) {
                    if (telefone.value == "") {
                        if (contadorCamposVazios == 0)
                            campos = "Telefone ";
                        else
                            campos = "- Telefone ";
                        contadorCamposVazios++;
                    }
                }
                if (typeof rg != 'undefined' && rg != null) {
                    if (rg.value == "") {
                        if (contadorCamposVazios == 0)
                            campos = "RG ";
                        else
                            campos += "- RG ";
                        contadorCamposVazios++;
                    }
                }
                if (typeof reservista != 'undefined' && reservista != null) {
                    if (reservista.value == "") {
                        if (contadorCamposVazios == 0)
                            campos = "Certificado de Reservista ";
                        else
                            campos += "- Certificado de Reservista ";
                        contadorCamposVazios++;
                    }
                }
                if (typeof titulo != 'undefined' && titulo != null) {
                    if (titulo.value == "") {
                        if (contadorCamposVazios == 0)
                            campos = "Título de Eleitor ";
                        else
                            campos += "- Título de Eleitor ";
                        contadorCamposVazios++;
                    }
                }
                if (contadorCamposVazios == 1)
                    return confirm("Atenção! O campo " + campos + "foi deixado em branco. Deseja salvar mesmo assim?");
                else if (contadorCamposVazios > 1)
                    return confirm("Atenção! Os campos " + campos + "foram deixados em branco. Deseja salvar mesmo assim?");
                else
                    return true;
            }
            return false;
        }

    </script>

    <br />
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe o ID/Vínculo ou o nome do servidor/funcionário"
        Width="800px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblPessoaTSearch" runat="server" Text="ID/Vínculo do Servidor/Funcionário:* "
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseServidor" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryServidor"
                        AutoPostBack="true" OnTextChanged="tseServidor_Changed" OnLoad="tseServidor_Load">
                    </tweb:TSearch>
                    <br />
                    <asp:Button ID="btnPostBack" runat="server" AutoPostBack="true" Text="Teste" Visible="False" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <div class="divEditBlock" style="width: 786px;">
        <asp:ImageButton ID="btnExcluir" runat="server" SkinID="BcDeletar" OnClick="btnExcluir_Click"
            OnClientClick="return confirm('Confirma a remoção?');" />
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClientClick="return confirmSalvar();"
            OnClick="btnSalvar_Click" />
        <asp:Label runat="server" ID="lblBloco" Text="Servidores/Funcionários" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsPessoas" runat="server" ShowMessageBox="true" ValidationGroup="SalvarForm"
            ShowSummary="false" />
    </div>
    <asp:TextBox ID="txtPessoaHid" runat="server" Visible="false"></asp:TextBox>
    <dxtc:ASPxPageControl ID="pcPessoa" runat="server" Width="800px" ActiveTabIndex="2"
        ClientInstanceName="pcPessoa" OnTabClick="pcPessoa_TabClick" AutoPostBack="true">
        <TabPages>
            <dxtc:TabPage Text="Dados Pessoais">
                <ContentCollection>
                    <dxw:ContentControl ID="ccDadosPessoais" runat="server">
                        <asp:Panel ID="pnlPessoa" runat="server" Visible="false">
                            <asp:Label ID="lblTSPessoa" runat="server" Text="Pessoa: "></asp:Label>
                            <tweb:TSearch ID="tsePessoa" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryPessoa"
                                AutoPostBack="true" OnTextChanged="tsePessoa_Changed">
                            </tweb:TSearch>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnDadosPessoais_Geral" runat="server" GroupingText="Geral">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblPessoa" Text="Código:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbltxtPessoa" runat="server" Text="Valor gerado após inclusão da pessoa."
                                            Visible="false"></asp:Label>
                                        <asp:TextBox ID="txtPessoa" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblNomeCompl" Text="Nome Completo:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtNomeCompl" Width="600px" runat="server" MaxLength="100" onkeypress="return nomeSemNum(event);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblNomeSocial" Text="Nome Social:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtNomeSocial" Width="600px" runat="server" MaxLength="100" onkeypress="return nomeSemNum(event);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblSexo" Text="Sexo:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblSexo" runat="server" RepeatDirection="Horizontal" DataValueField="sexo"
                                            Width="150px">
                                            <asp:ListItem Text="Masculino" Value="M"></asp:ListItem>
                                            <asp:ListItem Text="Feminino" Value="F"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblEtnia" Text="Cor/Raça:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlEtnia" runat="server" DataTextField="NOME" DataValueField="TABELAITEMID"  AutoPostBack="true" OnSelectedIndexChanged="ddlEtnia_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblPovo" Text="Povo Indígena:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                       <asp:DropDownList ID="ddlPovoIndigena" runat="server" DataTextField="DESCRICAO" DataValueField="POVOINDIGENAID">
                                    </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblNecessidadeEspecial" runat="server" Text="Necessidade Especial:*"
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlNecessidadeEspecial" runat="server" DataValueField="NECESSIDADEESPECIALID"
                                            DataTextField="DESCRICAO">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblEstadoCivil" Text="Estado Civil:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlEstadoCivil" runat="server" DataTextField="descr" DataValueField="item">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnDadosPessoais_Nascimento" runat="server" GroupingText="Nascimento">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblDataNasc" Text="Data de Nascimento:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtDataNasc" runat="server" MinDate="01/01/1900" CalendarProperties-ClearButtonText="Limpar"
                                            CalendarProperties-TodayButtonText="Hoje">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblPaisNasc" Text="País de Nascimento:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPaisNasc" runat="server" DataTextField="nome" DataValueField="codigo"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlPaisNasc_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblNacionalidade" Text="Nacionalidade:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlNacionalidade" runat="server" DataTextField="nome" DataValueField="nacionalidade"
                                            AutoPostBack="false">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblMunicipioNaturalidade" Text="Município(Naturalidade):* "
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearchBox ID="tseMunicipioNaturalidade" runat="server" Caption="" SqlOrder="nome"
                                            SqlSelect="SELECT codigo, nome, uf_sigla FROM municipio" Columns="10" ArgumentColumns="30"
                                            MaxLength="10" OnChanged="tseMunicipioNaturalidade_Changed">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                                <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                                                <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                        <asp:TextBox ID="txtMunicipioNaturalidade" runat="server" MaxLength="20"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblEstadoNaturalidade" runat="server" Text="Estado: "></asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtEstadoNaturalidade" runat="server" maxlength="20" class="txtInput" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnlFiliacao" GroupingText="Filiação" runat="server">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label14" runat="server" Text="Nome da Mãe:*"
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNomeMae" runat="server" Width="450px" MaxLength="100" onkeypress="return nomeSemNum(event); removeApostrofosDuplicados(event);" />
                                        <asp:CheckBox ID="chkMaeNaoDeclarada" Text="Não Declarada" AutoPostBack="true" runat="server"
                                            OnCheckedChanged="chkMaeNaoDeclarada_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label15" runat="server" Text="Nome do Pai:*"
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNomePai" runat="server" Width="450px" MaxLength="100" onkeypress="return nomeSemNum(event); removeApostrofosDuplicados(event);" />
                                        <asp:CheckBox ID="chkPaiNaoDeclarado" Text="Não Declarado" AutoPostBack="true" runat="server"
                                            OnCheckedChanged="chkPaiNaoDeclarado_CheckedChanged" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <div style="width: 100%; text-align: right">
                            <dxe:ASPxButton ID="btnProximo" runat="server" AutoPostBack="false" UseSubmitBehavior="false"
                                Text="Próximo >>">
                                <ClientSideEvents Click="function(s, e) {
	                                pcPessoa.ChangeActiveTab(1, false);}" />
                            </dxe:ASPxButton>
                        </div>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Endereço">
                <ContentCollection>
                    <dxw:ContentControl ID="ccEndereco" runat="server">
                        <asp:Panel ID="pnResidencia" runat="server" GroupingText="Endereço Residencial">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblPaisResid" Text="País:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPaisResid" runat="server" DataTextField="nome" DataValueField="codigo"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlPaisResid_Changed" Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblCepResid" Text="CEP:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCEPResid" runat="server" MaxLength="8" SkinID="numerico" Width="200px"></asp:TextBox>
                                        <tweb:TSearch ID="tsCEP" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryCEP"
                                            Modal="true" SkinID="CEP" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblMunicipioResid" Text="Município: "></asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearchBox ID="tseMunicipioResid" runat="server" Caption="" SqlOrder="nome"
                                            SqlSelect="SELECT codigo, nome, uf_sigla FROM municipio" Columns="10" ArgumentColumns="30"
                                            MaxLength="10" OnChanged="tseMunicipioResid_Changed">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                                <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                                                <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                        <asp:TextBox ID="txtMunicipio" runat="server" MaxLength="20"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblEstado" runat="server" Text="Estado: "></asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtEstado" runat="server" maxlength="20" class="txtInput" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblEnderecoResid" Text="Endereço:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEnderecoResid" runat="server" MaxLength="50" Width="250px"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblEndNumResid" Text="Número:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEndNumResid" runat="server" MaxLength="15" Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblComplementoResid" Text="Complemento: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtComplementoResid" runat="server" MaxLength="50" Width="250px"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblBairroResid" Text="Bairro:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBairroResid" runat="server" MaxLength="50" Width="200px" onkeypress="return alphanumeric_only(event);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblFormaOcup" runat="server" Text="Localização/Zona de Residência*: "
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:RadioButtonList ID="rblLocalizacaoUF" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="Rural">Rural</asp:ListItem>
                                            <asp:ListItem Value="Urbana">Urbana</asp:ListItem>
                                        </asp:RadioButtonList>
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
                                                    <dxe:ASPxCheckBox ID="chkQuilombos" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                                        runat="server" Checked="false" Text="Área remanescente de quilombos ">
                                                    </dxe:ASPxCheckBox>
                                                </td>
                                                <td>
                                                    <dxe:ASPxCheckBox ID="chkAreaAssentamento" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                                        runat="server" Checked="false" Text="Área de assentamento">
                                                    </dxe:ASPxCheckBox>
                                                    <dxe:ASPxCheckBox ID="chkTerraIndigena" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                                        runat="server" Checked="false" Text="Terra indígena">
                                                    </dxe:ASPxCheckBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnContatos" runat="server" GroupingText="Contatos">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblTelefone" Text="Telefone: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTelefone" runat="server" onkeyup="formataTelefoneDDD(this,event)"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblCelular" Text="Celular: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCelular" onkeyup="formataCelularDDD(this,event)" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblEmailInterno" Text="E-mail Office 365: "></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtEmailInterno" runat="server" MaxLength="100" Width="410px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="Label2" Text="E-mail Google for Education: "></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtEmailGoogle" runat="server" MaxLength="100" Width="410px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblEmailExterno" Text="E-mail Alternativo: "></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtEmailExterno" runat="server" MaxLength="100" Width="410px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <div style="width: 100%; text-align: center">
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <dxe:ASPxButton ClientInstanceName="btnAnterior" AutoPostBack="false" UseSubmitBehavior="false"
                                            ID="btnAnterior" runat="server" Text="<< Anterior">
                                            <ClientSideEvents Click="function(s, e) {
	                                pcPessoa.ChangeActiveTab(0, false);}" />
                                        </dxe:ASPxButton>
                                    </td>
                                    <td align="right">
                                        <dxe:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="false" UseSubmitBehavior="false"
                                            Text="Próximo >>">
                                            <ClientSideEvents Click="function(s, e) {
	                                pcPessoa.ChangeActiveTab(2, false);}" />
                                        </dxe:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Documentos">
                <ContentCollection>
                    <dxw:ContentControl ID="ccDocumentos" runat="server">
                        <asp:Panel ID="pnDocumentos" runat="server" GroupingText="Documentos">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblTipo" Text="Tipo: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlRG_Tipo" runat="server" DataTextField="descr" DataValueField="item"
                                            Width="250px" OnSelectedIndexChanged="ddlRGTipo_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="NumeroRG" Text="Número: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDOC_Rg_Num" runat="server" MaxLength="12" SkinID="numeroDocumento"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblRGEmissor" Text="Órgão Emissor: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddDOC_Rg_Emissor" runat="server" DataTextField="descr" DataValueField="item"
                                            Width="250px">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblRGUF" Text="Estado: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddDOC_Rg_Uf" runat="server" DataTextField="sigla" DataValueField="sigla">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblDataExp" Text="Data de Expedição: "></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <dxe:ASPxDateEdit ID="dboDOC_Rg_Dtexp" runat="server" MinDate="01/01/1900" CalendarProperties-ClearButtonText="Limpar"
                                                        CalendarProperties-TodayButtonText="Hoje">
                                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                        </CalendarProperties>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblCPF" Text="CPF:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCPF" runat="server" MaxLength="14" onkeyup="formataCPF(this,event)"
                                            Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblPISPASEP" runat="server" Text="PIS/PASEP:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPISPASEP" runat="server" MaxLength="11" SkinID="numerico"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblPassaporte" Text="Passaporte: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPassaporte" runat="server" MaxLength="50" Width="300px"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Panel ID="pnDocumentos_CertNasc" runat="server" GroupingText="Certidão de Nascimento ou Casamento">
                                <table>
                                    <tr>
                                        <td style="text-align: left">
                                            <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblTipoCertidao" runat="server"
                                                Text="Tipo Certidão Civil:*" SkinID="lblObrigatorio" Style="text-align: center"></asp:Label><asp:DropDownList
                                                    ID="ddlTipoCertidao" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoCertidao_SelectedIndexChanged">
                                                    <asp:ListItem Text="Não Informado" Value="Nenhum">
                                                    </asp:ListItem>
                                                    <asp:ListItem Text="Nascimento" Value="Nascimento"></asp:ListItem>
                                                    <asp:ListItem Text="Casamento" Value="Casamento"> </asp:ListItem>
                                                    <asp:ListItem Selected="True" Text="Selecione" Value=""> </asp:ListItem>
                                                </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left">
                                            <asp:Label ID="lblCertCivil" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                Text="Certidão Civil:*" Style="text-align: left"></asp:Label><asp:DropDownList ID="ddlCertidaoCivil"
                                                    runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCertidaoCivil_SelectedIndexChanged">
                                                    <asp:ListItem Text="Selecione" Value=""> </asp:ListItem>
                                                    <asp:ListItem Text="Modelo Antigo" Value="Modelo Antigo"> </asp:ListItem>
                                                    <asp:ListItem Text="Modelo Novo" Value="Modelo Novo"> </asp:ListItem>
                                                </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Panel ID="pnlAntigo" runat="server" Visible="false">
                                                <table>
                                                    <tr>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblUFCartorio" runat="server" Text="UF do Cartório: "></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlUFCartorio" runat="server" AutoPostBack="true" DataTextField="UF"
                                                                DataValueField="codigo_uf" OnSelectedIndexChanged="ddlUFCartorio_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblMunicipioCartorio" runat="server" Text="Município do Cartório: "></asp:Label>
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:DropDownList ID="ddlMunicipioCartorio" runat="server" AutoPostBack="true" DataTextField="municipio"
                                                                DataValueField="codigo_municipio" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlMunicipioCartorio_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblCartorio" runat="server" Text="Cartório: "></asp:Label>
                                                        </td>
                                                        <td colspan="5">
                                                            <asp:DropDownList ID="ddlCartorio" runat="server" DataTextField="nome_cartorio" DataValueField="cod_cartorio">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblCertNasc" runat="server" Text="Número do Termo: "></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDOC_CertNasc_Numero" runat="server" MaxLength="15" SkinID="numerico" />
                                                        </td>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblCertNascEmissao" runat="server" Text="Data de Emissão: "></asp:Label>
                                                        </td>
                                                        <td>
                                                            <dxe:ASPxDateEdit ID="dboDOC_CertNasc_DtEmissao" runat="server" CalendarProperties-ClearButtonText="Limpar"
                                                                CalendarProperties-TodayButtonText="Hoje" MinDate="1901-01-01">
                                                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                                </CalendarProperties>
                                                            </dxe:ASPxDateEdit>
                                                        </td>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="CertNascUF" runat="server" Text="Estado: "></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddDOC_CertNasc_Uf" runat="server" DataTextField="sigla" DataValueField="sigla">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblCertNascFolha" runat="server" Text="Folha: "></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDOC_CertNasc_Folha" runat="server" MaxLength="15" />
                                                        </td>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblNascLivro" runat="server" Text="Livro: "></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDOC_CertNasc_Livro" runat="server" MaxLength="15" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Panel ID="pnlNovo" runat="server" Visible="false">
                                                <table>
                                                    <tr>
                                                        <td style="text-align: right">
                                                            <asp:Label ID="lblNumMatricula" runat="server" Text="Número da matrícula: "></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtNumMatriculaCertidao" Width="200px" runat="server" MaxLength="32"
                                                                SkinID="numerico" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:Panel>
                        <br />
                        <div style="width: 100%; text-align: center">
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <dxe:ASPxButton ClientInstanceName="btnAnterior" AutoPostBack="false" UseSubmitBehavior="false"
                                            ID="ASPxButton2" runat="server" Text="<< Anterior">
                                            <ClientSideEvents Click="function(s, e) {
	                                pcPessoa.ChangeActiveTab(1, false);}" />
                                        </dxe:ASPxButton>
                                    </td>
                                    <td align="right">
                                        <dxe:ASPxButton ID="ASPxButton3" runat="server" AutoPostBack="false" UseSubmitBehavior="false"
                                            Text="Próximo >>">
                                            <ClientSideEvents Click="function(s, e) {
	                                pcPessoa.ChangeActiveTab(3, false);}" />
                                        </dxe:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Outros Documentos">
                <ContentCollection>
                    <dxw:ContentControl ID="ccOutrosDocs" runat="server">
                        <asp:Panel ID="pnDocumentos_CarteiraProf" runat="server" GroupingText="Carteira Profissional">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblCprofNum" Text="Número: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrpof_Num" runat="server" MaxLength="15" Width="120px"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblProfSerie" Text="Série: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCprof_Serie" runat="server" MaxLength="15" Width="120px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblCProf_DtExpedicao" Text="Data de Expedição: "></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dboCprof_DtExp" runat="server" MinDate="01/01/1900" CalendarProperties-ClearButtonText="Limpar"
                                            CalendarProperties-TodayButtonText="Hoje">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblCProf_UF" Text="Estado: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddDlCprof_Uf" runat="server" DataTextField="sigla" DataValueField="sigla"
                                            Width="120px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnDocumentos_TituloEleitor" runat="server" GroupingText="Título de Eleitor">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblEleitorNum" Text="Número: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDOC_Teleitor_Num" SkinID="numerico" runat="server" MaxLength="15"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblTeleitorZona" Text="Zona: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDOC_Teleitor_Zona" runat="server" MaxLength="15"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblEleitorSecao" Text="Seção: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDOC_Teleitor_Secao" runat="server" MaxLength="15"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblEleitorExp" Text="Data de Expedição: "></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dboDOC_Teleitor_DtExp" runat="server" MinDate="01/01/1900"
                                            CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label runat="server" ID="lblEleitorMunicipio" Text="Município: "></asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearchBox ID="tseMunicipioEleitor" runat="server" Caption="" SqlOrder="nome"
                                            SqlSelect="SELECT codigo, nome, uf_sigla FROM municipio" Columns="10" ArgumentColumns="30"
                                            MaxLength="10" OnChanged="tseMunicipioEleitor_Changed">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                                <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                                                <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblEstadoEleitor" runat="server" Text="Estado: "></asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtEstadoEleitor" runat="server" readonly="readonly" class="txtInput" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="pnDocumentosMilitares" runat="server" GroupingText="Documentos Militares"
                            Height="230px">
                            <br />
                            <asp:Panel ID="pnDocumentosMilitares_Alistamento" runat="server" GroupingText="Alistamento Militar">
                                <table>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="lblAlistNum" Text="Número: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDMIL_Alist_Num" SkinID="numerico" runat="server" MaxLength="17"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="lblAlistRM" Text="RM: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDMIL_Alist_RM" runat="server" MaxLength="15"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="lblAlistSerie" Text="Série: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDMIL_Alist_Serie" runat="server" MaxLength="15"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="lblAlistCSM" Text="CSM: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDMIL_Alist_CSM" runat="server" MaxLength="15"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="lblAlisDtExp" Text="Data de Expedição: "></asp:Label>
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="dboDMIL_Alist_DtExp" runat="server" MinDate="01/01/1900" CalendarProperties-ClearButtonText="Limpar"
                                                CalendarProperties-TodayButtonText="Hoje">
                                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                </CalendarProperties>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <br />
                            <asp:Panel ID="pnDocumentosMilitares_CertifReservista" runat="server" GroupingText="Certificado de Reservista">
                                <table>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="lblCrNum" Text="Número: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDMIL_Cr_Num" runat="server" MaxLength="17" SkinID="numerico"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="lblCRRM" Text="RM: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDMIL_Cr_RM" runat="server" MaxLength="15"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="lblCrSerie" Text="Série: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDMIL_Cr_Serie" runat="server" MaxLength="15"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="lblCrCSM" Text="CSM: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDMIL_Cr_CSM" runat="server" MaxLength="15"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="lblCrCat" Text="CAT: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDMIL_Cr_CAT" runat="server" MaxLength="15"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label runat="server" ID="lblCrDtExp" Text="Data de Expedição: "></asp:Label>
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="dboDMIL_Cr_DtExp" runat="server" MinDate="01/01/1900" CalendarProperties-ClearButtonText="Limpar"
                                                CalendarProperties-TodayButtonText="Hoje">
                                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                </CalendarProperties>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <br />
                        </asp:Panel>
                        <br />
                        <br />
                        <br />
                        <br />
                        <div style="width: 100%;">
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <dxe:ASPxButton ClientInstanceName="btnAnterior" AutoPostBack="false" UseSubmitBehavior="false"
                                            ID="ASPxButton4" runat="server" Text="<< Anterior">
                                            <ClientSideEvents Click="function(s, e) {
	                                pcPessoa.ChangeActiveTab(2, false);}" />
                                        </dxe:ASPxButton>
                                    </td>
                                    <td align="right">
                                        <dxe:ASPxButton ID="ASPxButton5" runat="server" AutoPostBack="false" UseSubmitBehavior="false"
                                            Text="Próximo >>">
                                            <ClientSideEvents Click="function(s, e) {
	                                pcPessoa.ChangeActiveTab(4, false);}" />
                                        </dxe:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Dados de Ingresso">
                <ContentCollection>
                    <dxw:ContentControl ID="ccDadosIngresso" runat="server">
                        <asp:Panel ID="pnlDadosIngresso" runat="server" Visible="false">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label1" runat="server" Text="ID Funcional:* " SkinID="lblObrigatorio"></asp:Label>
                                        <asp:TextBox ID="txtMatricula" runat="server" MaxLength="10" SkinID="numerico" Visible="false"></asp:TextBox>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtIdFuncional" runat="server" MaxLength="8" SkinID="numerico"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label25" runat="server" Text="Vinculo:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVinculo" runat="server" MaxLength="2" SkinID="numerico"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblDtAdmissao" runat="server" Text="Data de Nomeação*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dteDtNomeacao" runat="server" MinDate="1901-01-01">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblCargo" runat="server" Text="Cargo:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCargo" runat="server" DataTextField="nome" DataValueField="categoria">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblCHCargo" runat="server" Text="Carga Horária do Cargo:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCHCargo" runat="server" Width="50px" SkinID="numerico" MaxLength="2" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlIdFuncional" runat="server" Visible="false">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblIdFuncional" runat="server" Text="ID Funcional:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIdFuncionalAtualizacao" runat="server" MaxLength="8" SkinID="numerico"
                                            Width="100px" OnTextChanged="txtIdFuncionalAtualizacao_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chkNaoPossuiIdFuncional" AutoPostBack="true" Text="Não Possui"
                                            OnCheckedChanged="chkNaoPossuiIdFuncional_CheckedChanged" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Button ID="btnSalvarIdFuncional" runat="server" ValidationGroup="SalvarForm"
                                            Text="Salvar" OnClick="btnSalvarIdFuncional_Click" />
                                    </td>
                                </tr>
                                <tr>
                                </tr>
                            </table>
                            <br />
                            <asp:Label ID="Label16" runat="server" SkinID="lblMensagem" Visible="false"></asp:Label>
                        </asp:Panel>
                        <br />
                        <asp:Label ID="lblMensagemIdFuncional" runat="server" SkinID="lblMensagem" Visible="false"></asp:Label>
                        <br />
                        <asp:Panel ID="pnlGridDadosIngresso" runat="server" Visible="false">
                            <dxwgv:ASPxGridView ID="grdDadosIngresso" runat="server" AutoGenerateColumns="False"
                                ClientInstanceName="grdDadosIngresso" DataSourceID="odsIdvinculo" KeyFieldName="idvinculo;ordem"
                                OnCellEditorInitialize="grdDadosIngresso_CellEditorInitialize" OnRowInserting="grdDadosIngresso_RowInserting"
                                OnRowDeleting="grdDadosIngresso_RowDeleting" OnRowUpdating="grdDadosIngresso_RowUpdating"
                                OnAfterPerformCallback="grdDadosIngresso_AfterPerformCallback">
                                <SettingsEditing Mode="Inline" />
                                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                <SettingsBehavior ConfirmDelete="true" />
                                <Columns>
                                    <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="70px">
                                        <HeaderCaptionTemplate>
                                            <div style="text-align: center">
                                                <img src="../img/bt_novo.png" style="cursor: pointer" onclick="grdDadosIngresso.AddNewRow();"
                                                    runat="server" id="btnNovoGridIngresso" alt="Novo" />
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
                                    <dxwgv:GridViewDataTextColumn Caption="idvinculo" FieldName="idvinculo" Visible="False"
                                        VisibleIndex="0">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Pessoa" FieldName="pessoa" Visible="False"
                                        VisibleIndex="0">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Id Funcional*" FieldName="idfuncional" VisibleIndex="1"
                                        Width="100px" HeaderStyle-Font-Bold="true">
                                        <PropertiesTextEdit MaxLength="2" Width="100px">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                <RequiredField ErrorText="Favor informar o Id Funcional." IsRequired="True" />
                                                <RegularExpression ValidationExpression="^[+]?\d*$" ErrorText="Esse campo só permite valores númericos." />
                                            </ValidationSettings>
                                        </PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Vínculo*" FieldName="VINCULO" VisibleIndex="2"
                                        Width="100px" HeaderStyle-Font-Bold="true">
                                        <PropertiesTextEdit MaxLength="2" Width="100px">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                <RequiredField ErrorText="Favor informar o vínculo." IsRequired="True" />
                                                <RegularExpression ValidationExpression="^[+]?\d*$" ErrorText="Esse campo só permite valores númericos." />
                                            </ValidationSettings>
                                        </PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Matrícula ou ID/Vínculo" ReadOnly="True" FieldName="matricula"
                                        VisibleIndex="3" Width="100px" HeaderStyle-Font-Bold="true">
                                        <PropertiesTextEdit MaxLength="10" Width="100px">
                                        </PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Ordem" FieldName="ordem" ReadOnly="True" VisibleIndex="4">
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataDateColumn Caption="Data Nomeação*" FieldName="data_nomeacao"
                                        VisibleIndex="3" HeaderStyle-Font-Bold="true" Width="120px">
                                        <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" MinDate="1900-01-01" Width="120px">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                <RequiredField ErrorText="Favor informar a data de nomeação." IsRequired="True" />
                                            </ValidationSettings>
                                        </PropertiesDateEdit>
                                    </dxwgv:GridViewDataDateColumn>
                                    <dxwgv:GridViewDataDateColumn Caption="Data de Demissão" FieldName="data_desativacao"
                                        Width="120px" VisibleIndex="5">
                                        <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" MinDate="1900-01-01" Width="120px">
                                        </PropertiesDateEdit>
                                    </dxwgv:GridViewDataDateColumn>
                                    <dxwgv:GridViewDataComboBoxColumn Caption="Cargo*" FieldName="categoria" VisibleIndex="6"
                                        Width="150px" HeaderStyle-Font-Bold="true">
                                        <PropertiesComboBox DataSourceID="odsCargo" MaxLength="20" TextField="nome" ValueField="categoria"
                                            ValueType="System.String" Width="150px" EnableIncrementalFiltering="true">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                <RequiredField ErrorText="Favor informar a Categoria." IsRequired="True" />
                                            </ValidationSettings>
                                        </PropertiesComboBox>
                                    </dxwgv:GridViewDataComboBoxColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Carga Horária do Cargo*" FieldName="ch_categoria"
                                        HeaderStyle-Font-Bold="true" UnboundType="Integer" VisibleIndex="7" Width="110px">
                                        <PropertiesTextEdit MaxLength="2">
                                            <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                <RequiredField ErrorText="Favor informar a Carga horária da categoria." IsRequired="True" />
                                                <RegularExpression ErrorText="Carga horária da categoria deve ter no máximo 2 dígitos."
                                                    ValidationExpression="\d{0,2}" />
                                            </ValidationSettings>
                                            <ClientSideEvents KeyPress="function(s,e) { return MRAcceptNumber(s,e,3); }" />
                                        </PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>
                                </Columns>
                            </dxwgv:ASPxGridView>
                        </asp:Panel>
                        <techne:TTableDataSource ID="tdsDadosIngresso" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_vinculo"
                            SqlWhere="Ly_vinculo.pessoa = @pessoa">
                            <SqlWhereParameters>
                                <asp:ControlParameter ControlID="txtPessoaHid" Name="pessoa" PropertyName="Text" />
                            </SqlWhereParameters>
                        </techne:TTableDataSource>
                        <asp:ObjectDataSource ID="odsIdvinculo" TypeName="Techne.Lyceum.Net.Academico.Servidor"
                            runat="server" SelectMethod="ListaIdVinculo" InsertMethod="InsertVinculo" UpdateMethod="UpdateVinculo"
                            DeleteMethod="DeleteVinculo">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="txtPessoaHid" DefaultValue="" Name="pessoa" PropertyName="Text" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:ObjectDataSource ID="odsCargo" runat="server" SelectMethod="ListaCategoriaFuncionario"
                            TypeName="Techne.Lyceum.RN.CategoriaDocente"></asp:ObjectDataSource>
                        <br />
                        <div style="width: 100%; text-align: center">
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <dxe:ASPxButton ClientInstanceName="btnAnterior" AutoPostBack="false" UseSubmitBehavior="false"
                                            ID="ASPxButton10" runat="server" Text="<< Anterior">
                                            <ClientSideEvents Click="function(s, e) {
	                                pcPessoa.ChangeActiveTab(5, false);}" />
                                        </dxe:ASPxButton>
                                    </td>
                                    <td align="right">
                                        <dxe:ASPxButton ID="ASPxButton11" runat="server" AutoPostBack="false" UseSubmitBehavior="false"
                                            Text="Próximo >>">
                                            <ClientSideEvents Click="function(s, e) {
	                                pcPessoa.ChangeActiveTab(7, false);}" />
                                        </dxe:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Formação" Name="Formação">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl1" runat="server">
                        <br />
                        <asp:Label ID="lblMensagemFormacao" runat="server" SkinID="lblMensagem"></asp:Label>
                        <br />
                        <dxwgv:ASPxGridView ID="grdFormacaoPessoal" runat="server" AutoGenerateColumns="False"
                            EnableCallBacks="False" ClientInstanceName="grdFormacaoPessoal" DataSourceID="odsFormacaoPessoal"
                            KeyFieldName="ID_FORMACAO_PESSOAL" OnInitNewRow="grdFormacaoPessoal_InitNewRow"
                            OnCellEditorInitialize="grdFormacaoPessoal_CellEditorInitialize" OnStartRowEditing="grdFormacaoPessoal_StartRowEditing"
                            OnAutoFilterCellEditorInitialize="grdFormacaoPessoal_AutoFilterCellEditorInitialize"
                            OnRowValidating="grdFormacaoPessoal_RowValidating" OnAfterPerformCallback="grdFormacaoPessoal_AfterPerformCallback">
                            <SettingsBehavior ConfirmDelete="True" />
                            <SettingsEditing Mode="Inline" />
                            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                            <Columns>
                                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="60px">
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
                                <dxwgv:GridViewDataTextColumn Caption="ID_FORMACAO_PESSOAL" FieldName="ID_FORMACAO_PESSOAL"
                                    VisibleIndex="1" Visible="false">
                                    <PropertiesTextEdit MaxLength="10">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Pessoa" FieldName="PESSOA" VisibleIndex="1"
                                    Visible="true">
                                    <PropertiesTextEdit MaxLength="10">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataComboBoxColumn Caption="Escolaridade" VisibleIndex="2" FieldName="ESCOLARIDADE">
                                    <PropertiesComboBox MaxLength="20" TextField="descr" ClientInstanceName="cmbEscolaridade"
                                        ValueField="descr" ValueType="System.String" Width="150px" EnableIncrementalFiltering="true">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor informar a Escolaridade." IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesComboBox>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataComboBoxColumn Caption="Situação do Curso" VisibleIndex="3" FieldName="SITUACAO_CURSO">
                                    <PropertiesComboBox MaxLength="20" TextField="descr" ClientInstanceName="cmbSituacaoCurso"
                                        ValueField="descr" ValueType="System.String" Width="150px" EnableIncrementalFiltering="true">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor informar a Situação do Curso." IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesComboBox>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Área do Curso" FieldName="AREA" VisibleIndex="4">
                                    <PropertiesTextEdit MaxLength="10">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataComboBoxColumn Caption="Curso" FieldName="AREA_CURSO" VisibleIndex="5"
                                    Width="150px">
                                    <PropertiesComboBox MaxLength="20" ClientInstanceName="cmbCurso" ValueType="System.String"
                                        Width="150px">
                                        <%-- <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o Curso." IsRequired="True" />
                    </ValidationSettings>--%>
                                    </PropertiesComboBox>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataComboBoxColumn Caption="Formação/Complementação Pedagógica" FieldName="FORMACAO_COMPLEMENTACAO_PEDAGOGICA"
                                    VisibleIndex="6" Width="150px">
                                    <PropertiesComboBox MaxLength="20" TextField="item" ClientInstanceName="cmbFormacao"
                                        ValueField="item" ValueType="System.String" Width="150px">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor informar a Formação/Complementação Pedagógica." IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesComboBox>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataSpinEditColumn Caption="Ano de Início" FieldName="ANO_INICIO"
                                    VisibleIndex="7" Width="70px">
                                    <PropertiesSpinEdit DisplayFormatString="g" MaxLength="4" NumberFormat="Custom" NumberType="Integer">
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
				                                                                e.errorText='O Ano de Início deve ser positivo';
			                                                                }
		                                                                }
	                                                                }
	                                                                catch(ex)
	                                                                {
		                                                                e.isValid = false;
		                                                                e.errorText=ex;
	                                                                }
                                                                }" />
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor informar o Ano de Início" IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesSpinEdit>
                                </dxwgv:GridViewDataSpinEditColumn>
                                <dxwgv:GridViewDataSpinEditColumn Caption="Ano de Conclusão" FieldName="ANO_CONCLUSAO"
                                    VisibleIndex="8" Width="70px">
                                    <PropertiesSpinEdit DisplayFormatString="g" MaxLength="4" NumberFormat="Custom" NumberType="Integer">
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
				                                                                        e.errorText='O Ano de Conclusão deve ser positivo';
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
                                <dxwgv:GridViewDataTextColumn Caption="Código Instituição" FieldName="ID_INSTITUICAO"
                                    VisibleIndex="9">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Nome da Instituição" FieldName="NOME_COMP"
                                    VisibleIndex="9">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataCheckColumn Caption="Documentos Comprobatórios?" FieldName="DOC_COMPROBATORIO"
                                    VisibleIndex="10" Width="120px">
                                    <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="Sim"
                                        ValueType="System.String" ValueUnchecked="Não" DisplayTextUndefined="">
                                    </PropertiesCheckEdit>
                                </dxwgv:GridViewDataCheckColumn>
                            </Columns>
                            <Settings ShowFilterRow="false" ShowFilterRowMenu="false" />
                        </dxwgv:ASPxGridView>
                        <br />
                        <asp:Panel ID="PanelGraduacao" runat="server" GroupingText="Formação" Width="650px">
                            <table>
                                <tr>
                                    <td width="200">
                                        <asp:Label ID="lblEscolaridade" runat="server" Text="Escolaridade:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td class="style1">
                                        <asp:DropDownList ID="ddlEscolaridade" runat="server" DataTextField="descr" AutoPostBack="True"
                                            AppendDataBoundItems="true" DataValueField="descr" OnSelectedIndexChanged="ddlEscolaridade_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="200">
                                        <asp:Label ID="lblSituacaoCurso" runat="server" Text="Situação do Curso:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td class="style1">
                                        <asp:DropDownList ID="ddlSituacaoCurso" runat="server" DataTextField="descr" AppendDataBoundItems="true"
                                            DataValueField="descr">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAreaCurso" runat="server" Text="Área do Curso:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td class="style1">
                                        <asp:DropDownList ID="ddlAreaCurso" runat="server" AutoPostBack="True" AppendDataBoundItems="true"
                                            OnSelectedIndexChanged="ddlAreaCurso_SelectedIndexChanged" DataTextField="AREA"
                                            DataValueField="ID_AREA_FORMACAO_PESSOAL">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCurso" runat="server" Text="Curso:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td class="style1">
                                        <asp:DropDownList ID="ddlCurso" runat="server" DataTextField="CURSO" AppendDataBoundItems="true"
                                            AutoPostBack="true" DataValueField="ID_CURSO_FORMACAO_PESSOAL" Enabled="false"
                                            OnSelectedIndexChanged="ddlCurso_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFormComplementPedag" runat="server" Text="Formação/Complementação Pedagógica:*"
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td class="style1">
                                        <asp:DropDownList ID="ddlFormComplementPedag" runat="server" DataTextField="descr"
                                            AppendDataBoundItems="true" DataValueField="descr">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAnoInicio" runat="server" Text="Ano de Inicio:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="txtAnoInicio" runat="server" MaxLength="4"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblAnoConclusao" runat="server" Text="Ano de Conclusão:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="txtAnoConclusao" runat="server" MaxLength="4"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblTipoInstituicao" runat="server" Text="Tipo de Instituição:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td class="style1">
                                        <asp:DropDownList ID="ddlTipoInstituicao" runat="server" AutoPostBack="True" DataTextField="descr"
                                            AppendDataBoundItems="true" DataValueField="descr" OnSelectedIndexChanged="ddlTipoInstituicao_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblInstituicao" runat="server" Text="Instituição:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td class="style1">
                                        <tweb:TSearch ID="tseInstituicao" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryInstituicao"
                                            AutoPostBack="true" OnTextChanged="tseInstituicao_Changed">
                                            <QueryParameters>
                                                <asp:ControlParameter ControlID="ddlTipoInstituicao" Name="TIPO_ORIGEM" PropertyName="SelectedValue" />
                                            </QueryParameters>
                                        </tweb:TSearch>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblDocComprob" runat="server" Text="Documentos Comprobatórios?"></asp:Label>
                                    </td>
                                    <td class="style1">
                                        <asp:CheckBox runat="server" ID="ckDocComprob" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <p align="center">
                                            <asp:Label ID="lblMsg" runat="server" EnableTheming="False" Font-Size="Medium" ForeColor="#FF3300"></asp:Label></p>
                                        <p align="center">
                                            <asp:Button ID="btnSalvarFormacao" runat="server" Enabled="True" ValidationGroup="SalvarForm"
                                                Text="Incluir Formação Pessoal - Graduação" OnClick="btnSalvarFormacao_Click" />
                                        </p>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <br />
                        <asp:Panel ID="Panel3" runat="server" GroupingText="Pós-Graduação" Width="650px">
                            <table>
                                <tr>
                                    <td width="200">
                                        <asp:Label ID="Label3" runat="server" Text="Pós-Graduação:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td class="style1">
                                        <asp:DropDownList ID="ddlEscolaridadePosGraduacao" runat="server" DataTextField="descr"
                                            AutoPostBack="True" AppendDataBoundItems="true" DataValueField="descr" OnSelectedIndexChanged="ddlEscolaridadePosGraduacao_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="200">
                                        <asp:Label ID="Label4" runat="server" Text="Situação do Curso:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td class="style1">
                                        <asp:DropDownList ID="ddlSituacaoCursoPosGraduacao" runat="server" DataTextField="descr"
                                            AppendDataBoundItems="true" DataValueField="descr">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="Área do Curso:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td class="style1">
                                        <asp:DropDownList ID="ddlAreaCursoPosGraduacao" runat="server" AutoPostBack="True"
                                            AppendDataBoundItems="true" OnSelectedIndexChanged="ddlAreaCursoPosGraduacao_SelectedIndexChanged"
                                            DataTextField="AREA" DataValueField="ID_AREA_FORMACAO_PESSOAL">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="Curso:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td class="style1">
                                        <asp:DropDownList ID="ddlCursoPosGraduacao" AutoPostBack="true" runat="server" DataTextField="CURSO"
                                            AppendDataBoundItems="true" DataValueField="ID_CURSO_FORMACAO_PESSOAL" Enabled="false"
                                            OnSelectedIndexChanged="ddlCursoPosGraduacao_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label7" runat="server" Text="Formação/Complementação Pedagógica:*"
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td class="style1">
                                        <asp:DropDownList ID="ddlFormComplementPedagPosGraduacao" runat="server" DataTextField="descr"
                                            AppendDataBoundItems="true" DataValueField="descr">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label8" runat="server" Text="Ano de Inicio:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="txtAnoInicioPosGraduacao" runat="server" MaxLength="4"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" Text="Ano de Conclusão:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td class="style1">
                                        <asp:TextBox ID="txtAnoConclusaoPosGraduacao" runat="server" MaxLength="4"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label10" runat="server" Text="Tipo de Instituição:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td class="style1">
                                        <asp:DropDownList ID="ddlTipoInstituicaoPosGraduacao" runat="server" AutoPostBack="True"
                                            DataTextField="descr" AppendDataBoundItems="true" DataValueField="descr" OnSelectedIndexChanged="ddlTipoInstituicaoPosGraduacao_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label11" runat="server" Text="Instituição:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td class="style1">
                                        <tweb:TSearch ID="tseInstituicaoPosGraduacao" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryInstituicao"
                                            AutoPostBack="true" OnTextChanged="tseInstituicao_Changed">
                                            <QueryParameters>
                                                <asp:ControlParameter ControlID="ddlTipoInstituicaoPosGraduacao" Name="TIPO_ORIGEM"
                                                    PropertyName="SelectedValue" />
                                            </QueryParameters>
                                        </tweb:TSearch>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label12" runat="server" Text="Documentos Comprobatórios?"></asp:Label>
                                    </td>
                                    <td class="style1">
                                        <asp:CheckBox runat="server" ID="ckDocComprobPosGraduacao" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="right">
                                        <p align="center">
                                            <asp:Label ID="Label13" runat="server" EnableTheming="False" Font-Size="Medium" ForeColor="#FF3300"></asp:Label></p>
                                        <p align="center">
                                            <asp:Button ID="btnSalvarFormacaoPosGraduacao" runat="server" Enabled="True" ValidationGroup="SalvarForm"
                                                Text="Incluir Formação Pessoal - Pós-Graduação" OnClick="btnSalvarFormacaoPosGraduacao_Click" />
                                        </p>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <p align="center">
                            <asp:Label ID="lblMensValidacao" runat="server" SkinID="lblMensagem"></asp:Label>
                        </p>
                        <asp:ObjectDataSource ID="odsFormacaoPessoal" TypeName="Techne.Lyceum.Net.Academico.Servidor"
                            runat="server" SelectMethod="ListarPessoa" UpdateMethod="Update" DeleteMethod="Delete"
                            OnDeleting="odsArea_Deleting" OnUpdating="odsArea_Updating">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="txtPessoa" PropertyName="Text" Name="pessoa" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <br />
                        <div style="width: 100%; text-align: center">
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <dxe:ASPxButton ClientInstanceName="btnAnterior" AutoPostBack="false" UseSubmitBehavior="false"
                                            ID="ASPxButton9" runat="server" Text="<< Anterior">
                                            <ClientSideEvents Click="function(s, e) {
	                                pcPessoa.ChangeActiveTab(6, false);}" />
                                        </dxe:ASPxButton>
                                    </td>
                                    <td align="right">
                                        <dxe:ASPxButton ID="ASPxButton12" runat="server" AutoPostBack="false" UseSubmitBehavior="false"
                                            Text="Próximo >>">
                                            <ClientSideEvents Click="function(s, e) {
	                                pcPessoa.ChangeActiveTab(8, false);}" />
                                        </dxe:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Capacitação">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl3" runat="server">
                        <br />
                        <br />
                        <br />
                        <input type="hidden" value="<%= minCargaHorariaCursoCapacitacao %>" id="minCargaHorariaCursoCapacitacao"
                            name="minCargaHorariaCursoCapacitacao" />
                        <asp:ObjectDataSource ID="odsServidorCursoCapacitacao" TypeName="Techne.Lyceum.Net.Academico.Servidor"
                            runat="server" SelectMethod="ListarServidorCursoCapacitacao" OldValuesParameterFormatString="{0}"
                            OnDeleting="odsServidorCursoCapacitacao_Deleting" DeleteMethod="DeleteCursoCapacitacao"
                            OnInserting="odsServidorCursoCapacitacao_Inserting" InsertMethod="InsereCursoCapacitacao"
                            OnUpdating="odsServidorCursoCapacitacao_Updating" UpdateMethod="AlteraCursoCapacitacao">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="txtPessoa" PropertyName="Text" Name="idPessoa" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <dxwgv:ASPxGridView ID="grdCapacitacao" runat="server" AutoGenerateColumns="False"
                            Visible="true" ClientInstanceName="grdCapacitacao" DataSourceID="odsServidorCursoCapacitacao"
                            KeyFieldName="CompositeKey" OnCellEditorInitialize="grdCapacitacao_CellEditorInitialize"
                            OnInitNewRow="grdCapacitacao_InitNewRow" OnCustomUnboundColumnData="grdCapacitacao_CustomUnboundColumnData"
                            OnRowDeleting="grdCapacitacao_RowDeleting" OnRowUpdating="grdCapacitacao_RowUpdating"
                            OnRowInserting="grdCapacitacao_RowInserting" OnStartRowEditing="grdCapacitacao_StartRowEditing"
                            OnRowValidating="grdCapacitacao_RowValidating" OnAfterPerformCallback="grdCapacitacao_AfterPerformCallback"
                            Width="1200px">
                            <SettingsBehavior ConfirmDelete="True" />
                            <SettingsEditing Mode="Inline" />
                            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                            <Columns>
                                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                    <HeaderCaptionTemplate>
                                        <div style="text-align: center">
                                            <img runat="server" id="btnNovoGridCapac" alt="Novo" src="../img/bt_novo.png" onclick="grdCapacitacao.AddNewRow();" />
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
                                <dxwgv:GridViewDataTextColumn Caption="Pessoa" FieldName="PESSOAID" VisibleIndex="1"
                                    Visible="False">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Ordem*" HeaderStyle-Font-Bold="true" FieldName="CURSOCAPACITACAOID"
                                    VisibleIndex="1" Visible="false">
                                    <PropertiesTextEdit MaxLength="9">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Curso/Capacitação*" HeaderStyle-Font-Bold="true"
                                    FieldName="NOMECURSO" VisibleIndex="2" Width="200px">
                                    <PropertiesTextEdit MaxLength="100">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor informar a Capacitação." IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataComboBoxColumn Caption="Tipo do Curso*" FieldName="TIPOCURSOCAPACITACAOID"
                                    VisibleIndex="3" Width="200px">
                                    <PropertiesComboBox DataSourceID="odsTipoCursoCapacitacao" TextField="DESCRICAO"
                                        ValueField="TIPOCURSOCAPACITACAOID" ValueType="System.String">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor informar o Tipo do Curso." IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesComboBox>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataComboBoxColumn Caption="Área de Conhecimento*" FieldName="AREACONHECIMENTOID"
                                    VisibleIndex="4" Width="200px">
                                    <PropertiesComboBox DataSourceID="odsAreaConhecimentoCapacitacao" TextField="DESCRICAO"
                                        ValueField="AREACONHECIMENTOID" ValueType="System.String">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor informar a Área de Conhecimento." IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesComboBox>
                                </dxwgv:GridViewDataComboBoxColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Nome da Instituição*" HeaderStyle-Font-Bold="true"
                                    FieldName="NOMEINSTITUICAO" VisibleIndex="5">
                                    <PropertiesTextEdit MaxLength="100">
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor informar o Nome da Instituição." IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataSpinEditColumn Caption="Carga Horária*" FieldName="CARGAHORARIA"
                                    VisibleIndex="5" Width="70px">
                                    <PropertiesSpinEdit DisplayFormatString="g" MaxLength="5" NumberFormat="Custom" NumberType="Integer">
                                        <SpinButtons ShowIncrementButtons="False">
                                        </SpinButtons>
                                        <ClientSideEvents Validation="function(s, e) {
	                                                    var strVal=e.value;
	                                                    var iVal=null;
	                                                    var minCargaHoraria = window.document.getElementById('minCargaHorariaCursoCapacitacao').value;
	                                                    e.isValid = true;
	                                                    try
	                                                    {
		                                                    if(strVal!=null)
		                                                    {
			                                                    iVal=parseInt(strVal);
			                                                    if(iVal&lt;parseInt(minCargaHoraria))
			                                                    {
				                                                    e.isValid = false;
				                                                    e.errorText='Valor da carga Horária não pode ser inferior a ' + minCargaHoraria + ' horas';
			                                                    }
			                                                }
			                                                else
			                                                {
			                                                    e.isValid = false;
			                                                    e.errorText='Favor informar a Carga Horária.';
		                                                    }
	                                                    }
	                                                    catch(ex)
	                                                    {
		                                                    e.isValid = false;
		                                                    e.errorText=ex;
	                                                    }
                                                    }" />
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor informar a Carga Horária." IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesSpinEdit>
                                </dxwgv:GridViewDataSpinEditColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Conclusão*" HeaderStyle-Font-Bold="true"
                                    FieldName="DATACONCLUSAO" VisibleIndex="6" Width="100px">
                                    <PropertiesDateEdit>
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor informar a Data Conclusão." IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesDateEdit>
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                                    Visible="False" VisibleIndex="5">
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                            <Styles>
                                <CommandColumn Wrap="False">
                                </CommandColumn>
                            </Styles>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                        </dxwgv:ASPxGridView>
                        <asp:ObjectDataSource ID="odsTipoCursoCapacitacao" runat="server" OldValuesParameterFormatString="original_{0}"
                            SelectMethod="ListarTipoCurso" TypeName="Techne.Lyceum.Net.Academico.Servidor">
                        </asp:ObjectDataSource>
                        <asp:ObjectDataSource ID="odsAreaConhecimentoCapacitacao" runat="server" OldValuesParameterFormatString="original_{0}"
                            SelectMethod="ListarAreaConhecimento" TypeName="Techne.Lyceum.Net.Academico.Servidor">
                        </asp:ObjectDataSource>
                        <br />
                        <div style="width: 100%; text-align: center">
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <dxe:ASPxButton ClientInstanceName="btnAnterior" AutoPostBack="false" UseSubmitBehavior="false"
                                            ID="ASPxButton13" runat="server" Text="<< Anterior">
                                            <ClientSideEvents Click="function(s, e) {
	                                                    pcPessoa.ChangeActiveTab(7, false);}" />
                                        </dxe:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
    <%--        </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
