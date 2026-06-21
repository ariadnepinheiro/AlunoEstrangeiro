<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Docentes.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.Docentes" EnableEventValidation="false" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<asp:Content ID="cnDocentes" ContentPlaceHolderID="cphFormulario" runat="server">
    <style>
        .cursorImagem
        {
            cursor: pointer;
        }
        .txtInput
        {
            background-color: White;
            font-family: Verdana;
            font-size: smaller;
        }
    </style>

    <script type="text/javascript">
        var doProcessClick;
        var visibleIndex;

        function BloquearCtrl() {
            if (event.keyCode == 17)
            { alert("Proibido utilizar o Ctrl neste campo"); }
        }

        function desabilitaBotaoDireito() {
            if (event.button == 2) { alert("Proibido utilizar o botao direito neste campo"); }
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        function OnGetRowValues(result) {
            for (var i = 0; i < result.length; i++)
                for (var j = 0; j < result[i].length; j++) {
                alert(result[i][j]);
            }
        }

        function ProcessClick(stringkey) {
            $("#<%=txtFormacaoPessoalID.ClientID %>").val(stringkey);
            ExecutarPostBack();

        }

        function ExecutarPostBack() {
            __doPostBack('<%=this.txtFormacaoPessoalID.UniqueID%>', '');
        }

        function OnEndCallback(s) {
            if (s.cpAtualizar != null) {
                $("#<%= this.lblMsg.ClientID %>").text(s.cpAtualizar);
                s.cpAtualizar = null;
            }
        }


        $().ready(function() {
            trataCep({
                tscep: '<%=tsCEP.ClientID %>',
                cep: '<%=txtCEP.ClientID %>',
                nomeLogradouro: '<%=txtEnderecoPessoa.ClientID %>',
                nomeBairro: '<%=txtBairro.ClientID %>',
                nomeMunicipio: '<%=txtMunicipio.ClientID %>',
                codigoMunicipio: '<%=tseMunicipio.ClientID %>',
                uf: '<%=txtEstado.ClientID %>'
            });

        });

    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe o ID/Vínculo ou o nome do docente"
        Height="50px" Width="750px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblDocenteTSearch" runat="server" Text="ID/Vínculo do docente*: "
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseDocentes" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryDocenteCad"
                        AutoPostBack="true" OnTextChanged="tseDocentes_Changed" OnLoad="tseDocentes_Load">
                    </tweb:TSearch>
                </td>
                <tr>
                    <td style="text-align: left" colspan="2">
                        <asp:Label ID="lblMensagem1" runat="server" Text="*Caso não tenha Id/Vínculo, informar a Matrícula."
                            Style="color: red"></asp:Label>
                    </td>
                </tr>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:Label ID="lblAvisoDol" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <div class="divEditBlock" style="width: 968px;">
        <asp:Label runat="server" ID="lblBloco" Text="Docentes" SkinID="BcTitulo" />
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
            ValidationGroup="SalvarForm" />
        <asp:ImageButton ID="btnImprimir" SkinID="Imprimir" runat="server" ImageAlign="Right"
            OnClick="btnImprimir_Click" />
        <asp:ValidationSummary ID="vsDocente" runat="server" ShowMessageBox="true" ValidationGroup="SalvarForm"
            ShowSummary="false" />
    </div>
    <dxtc:ASPxPageControl ID="apcDocente" runat="server" Height="299px" Width="968px"
        ActiveTabIndex="4" TabIndex="1">
        <TabPages>
            <dxtc:TabPage Name="DadosPessoais" Text="Dados Pessoais">
                <ContentCollection>
                    <dxw:ContentControl ID="conDocentes" runat="server">
                        <asp:Label ID="lblPessoaTSearch" runat="server" Text="Pessoa: "></asp:Label>
                        <tweb:TSearch ID="tsePessoa" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryPessoa"
                            AutoPostBack="true" OnTextChanged="tsePessoa_Changed">
                        </tweb:TSearch>
                        <br />
                        <br />
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" OnUnload="UpdatePanel1_Unload">
                            <ContentTemplate>
                                <asp:Panel ID="pnlDadosPessoais" runat="server" GroupingText="Dados Pessoais">
                                    <table>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblPessoa" runat="server" Text="Pessoa:* " SkinID="lblObrigatorio"
                                                    Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbltxtPessoa" runat="server" Text="Valor gerado após inclusão do docente."
                                                    Visible="false"></asp:Label>
                                                <asp:TextBox ID="txtPessoa" runat="server" MaxLength="10" ReadOnly="true" Visible="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <asp:Label ID="lblIDINEP" runat="server" Text="ID INEP:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtIDINEP" runat="server" MaxLength="10" ReadOnly="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblNomeComplPessoa" runat="server" Text="Nome Completo:* " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtNomeComplPessoa" runat="server" MaxLength="100" Columns="80"
                                                    onkeypress="return nomeSemNum(event);">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="Label29" runat="server" Text=" Nome Social:"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="TxtNomeSocial" runat="server" MaxLength="100" Columns="80" onkeypress="return nomeSemNum(event);">
                                                </asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblDtNasc" runat="server" Text="Data Nascimento:* " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <dxe:ASPxDateEdit ID="dteDtNasc" runat="server" MinDate="1901-01-01">
                                                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                    </CalendarProperties>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblSexo" runat="server" Text="Sexo:* " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rblSexo" runat="server" RepeatDirection="Horizontal" DataValueField="sexo">
                                                    <asp:ListItem Text="Masculino" Value="M"></asp:ListItem>
                                                    <asp:ListItem Text="Feminino" Value="F"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblEtnia" Text="Cor/Raça:* " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlRaca" runat="server" DataTextField="NOME" DataValueField="TABELAITEMID"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlRaca_SelectedIndexChanged">
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
                                                <asp:Label ID="lblEstadoCivil" runat="server" Text="Estado Civil:* " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlEst_Civil" runat="server" DataTextField="descr" DataValueField="item">
                                                </asp:DropDownList>
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
                                                <asp:Label ID="lblNaturalidade" runat="server" Text="Naturalidade:* " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <tweb:TSearchBox ID="tseNaturalidade" runat="server" Caption="" SqlOrder="nome" SqlSelect="SELECT codigo, nome, uf_sigla FROM municipio"
                                                    Columns="10" ArgumentColumns="30" MaxLength="10" OnChanged="tseNaturalidade_Changed">
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
                                            <td>
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
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br />
                        <asp:Panel ID="pnlEndereco" runat="server" GroupingText="Endereço" Font-Names="Verdana">
                            <table>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblPais" runat="server" Text="País:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPais" runat="server" DataValueField="codigo" DataTextField="nome"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlPais_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblCEP" runat="server" Text="CEP:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCEP" SkinID="numerico" runat="server" MaxLength="8"></asp:TextBox>
                                        <tweb:TSearch ID="tsCEP" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryCEP"
                                            Modal="true" SkinID="CEP" />
                                        <asp:RegularExpressionValidator ID="revCEP" ControlToValidate="txtCEP" ValidationExpression="^.{8}$"
                                            runat="server" ErrorMessage="CEP: Preenchimento de oito números obrigatório."
                                            ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblMunicipio" runat="server" Text="Município:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMunicipio" runat="server" MaxLength="20"></asp:TextBox>
                                        <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect="SELECT codigo, nome, uf_sigla FROM municipio"
                                            GridWidth="600px" ArgumentColumns="30" Columns="10" OnChanged="tseMunicipio_Changed"
                                            MaxLength="10">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                                <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                                                <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblEstado" runat="server" Text="Estado:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtEstado" runat="server" maxlength="20" class="txtInput" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblEnderecoPessoa" runat="server" Text="Endereço:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtEnderecoPessoa" runat="server" MaxLength="50" Columns="50" onkeypress="return endereco(event);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblEndNumPessoa" runat="server" Text="Nº.:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEndNumPessoa" runat="server" MaxLength="15"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblEndCompl" runat="server" Text="Complemento: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEndCompl" runat="server" MaxLength="50" onkeypress="return endereco(event);"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblBairro" runat="server" Text="Bairro: *" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtBairro" runat="server" MaxLength="50" onkeypress="return alphanumeric_only(event);"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="reBairro" runat="server" ControlToValidate="txtBairro"
                                            ErrorMessage="Bairro inválido." ValidationGroup="SalvarForm" SetFocusOnError="true"
                                            ValidationExpression="^[A-Za-zÁáÀàÂâÃãÉéÈèÊêÍíÌìÎîÓóÒòÔôÕõÚúÙùÛûÇçÑñ0-9\)\(\- ]{3,50}$">
                                            <img src="../Images/AlertaMens.gif" alt="Bairro inválido!"/>
                                        </asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblFormaOcup" runat="server" Text="Localização/Zona de Residência*: "
                                            SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
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
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblFone" runat="server" Text="Telefone: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFone" onkeyup="formataTelefoneDDD(this,event)" runat="server"
                                            MaxLength="13"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" ID="lblCelular" runat="server" Text="Celular: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCelular" onkeyup="formataCelularDDD(this,event)" runat="server"
                                            MaxLength="14"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblEmail" runat="server" Text="E-mail Office 365: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmailInstitucional" runat="server" MaxLength="100" Columns="50"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="reEmail" runat="server" ControlToValidate="txtEmailInstitucional"
                                            ErrorMessage="E-mail inválido." ValidationGroup="SalvarForm" SetFocusOnError="true"
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"><img src="../Images/AlertaMens.gif" alt="E-mail inválido" /></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label28" runat="server" Text="E-mail Google for Education: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmailGoogle" runat="server" MaxLength="100" Columns="50" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="Label26" runat="server" Text="E-mail Alternativo: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" Columns="50"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                                            ErrorMessage="E-mail inválido." ValidationGroup="SalvarForm" SetFocusOnError="true"
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"><img src="../Images/AlertaMens.gif" alt="E-mail inválido" /></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Name="Documentos" Text="Documentos">
                <ContentCollection>
                    <dxw:ContentControl ID="ccDocumentos" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" OnUnload="UpdatePanel1_Unload">
                            <ContentTemplate>
                                <asp:Panel ID="pnlCarteiraIdentidade" runat="server" GroupingText="Documento de identificação"
                                    Height="123px">
                                    <table>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblRGTipoPessoa" runat="server" Text="Tipo:* " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlRGTipoPessoa" runat="server" DataValueField="item" DatatTextField="descr"
                                                    OnSelectedIndexChanged="ddlRGTipoPessoa_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblRGNumPessoa" runat="server" Text="Número:* " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtRGNumPessoa" runat="server" MaxLength="15" SkinID="numeroDocumento"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblRGUFPessoa" runat="server" Text="Estado:* " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlRGUFPessoa" runat="server" DataValueField="sigla" DatatTextField="sigla">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblRGEmissorPessoa" runat="server" Text="Órgão Emissor:*" SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlRGEmissorPessoa" runat="server" DataValueField="item" DatatTextField="item">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblRGDataExpPessoa" runat="server" Text="Data de Expedição:* " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <dxe:ASPxDateEdit ID="dteRGDataExpPessoa" runat="server">
                                                                <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                                </CalendarProperties>
                                                            </dxe:ASPxDateEdit>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlOutrosDocumentos" runat="server" GroupingText="Outros Documentos">
                                    <table>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblCPF" runat="server" Text="CPF:* " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCPF" runat="server" onkeyup="formataCPF(this,event)" MaxLength="14"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPISPASEP" runat="server" Text="PIS/PASEP:* " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtPISPASEP" runat="server" MaxLength="11" SkinID="numerico"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlCarteiraProfissional" runat="server" GroupingText="Carteira Profissional">
                                    <table>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblCProfNum" runat="server" Text="Número: "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCProfNum" runat="server" MaxLength="15"></asp:TextBox>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblCProfSerie" runat="server" Text="Série: "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCProfSerie" runat="server" MaxLength="15"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblCProfDtExp" runat="server" Text="Data de Expedição: "></asp:Label>
                                            </td>
                                            <td>
                                                <dxe:ASPxDateEdit ID="dteCProfDtExp" runat="server" MinDate="1901-01-01">
                                                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                    </CalendarProperties>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblCProfUF" runat="server" Text="Estado: "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="txtCProfUF" runat="server" DataValueField="sigla" DatatTextField="sigla">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
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
                                                <dxe:ASPxDateEdit ID="dteDOC_Teleitor_DtExp" runat="server" MinDate="01/01/1900"
                                                    CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                                                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                    </CalendarProperties>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
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
                                                    <dxe:ASPxDateEdit ID="dteDMIL_Alist_DtExp" runat="server" MinDate="01/01/1900" CalendarProperties-ClearButtonText="Limpar"
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
                                                    <dxe:ASPxDateEdit ID="dteDMIL_Cr_DtExp" runat="server" MinDate="01/01/1900" CalendarProperties-ClearButtonText="Limpar"
                                                        CalendarProperties-TodayButtonText="Hoje">
                                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                        </CalendarProperties>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </asp:Panel>
                                <br />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Name="DadosLotacao" Text="Dados de Ingresso">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl2" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" OnUnload="UpdatePanel1_Unload">
                            <ContentTemplate>
                                <asp:Panel ID="pnlDocente" runat="server" Height="550px" Font-Names="Verdana">
                                    <table>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblNumFunc" runat="server" Text="Docente:* " SkinID="lblObrigatorio"
                                                    Visible="false"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:Label ID="lbltxtNumFunc" runat="server" Text="Valor gerado após inclusão do docente."
                                                    Visible="false"></asp:Label>
                                                <asp:TextBox ID="txtNumFunc" runat="server" MaxLength="15" ReadOnly="true" Visible="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="Label25" runat="server" Text="ID Funcional:* " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtIdFuncional" runat="server" MaxLength="8" SkinID="numerico"></asp:TextBox>
                                                <asp:CheckBox runat="server" ID="chkNaoPossuiIdFuncional" AutoPostBack="true" Text="Não Possui"
                                                    Enabled="false" OnCheckedChanged="chkNaoPossuiIdFuncional_CheckedChanged" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="Label27" runat="server" Text="Vinculo:* " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtVinculo" runat="server" MaxLength="2" SkinID="numerico"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblMatricula" runat="server" Text="Matrícula ou ID/Vínculo:"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtMatricula" runat="server" MaxLength="10" SkinID="numerico" ReadOnly="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblCategoria" runat="server" Text="Cargo:* " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <tweb:TSearchBox ID="tseCategoria" AutoPostBack="true" runat="server" Key="categoria"
                                                    Argument="nome" Caption="" MaxLength="20" GridWidth="850px" OnChanged="tseCategoria_Changed"
                                                    SqlSelect="Select distinct categoria,nome from ly_categoria_docente" SqlOrder="nome">
                                                    <GridColumns>
                                                        <tweb:TSearchBoxColumn Caption="Código" FieldName="categoria" Width="12%" />
                                                        <tweb:TSearchBoxColumn Caption="Cargo" FieldName="nome" Width="30%" />
                                                    </GridColumns>
                                                </tweb:TSearchBox>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblRegTrabalho" runat="server" Text="Regime de Contratação:* " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:DropDownList ID="cmbRegContratacao" runat="server" DataTextField="descricao"
                                                    AppendDataBoundItems="true" OnSelectedIndexChanged="cmbRegContratacao_SelectedIndexChanged"
                                                    DataValueField="regimecontratacaoid" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblCargaHoraria" Text="Carga Horária:* " SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlCargaHoraria" Width="100px">
                                                </asp:DropDownList>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblAulasAlocadas" runat="server" Text="Aulas Alocadas:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAulasAlocadas" runat="server" Width="50px" ReadOnly="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblDtAdmissao" runat="server" Text="Data de Admissão*" SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <dxe:ASPxDateEdit ID="dteDtAdmissao" runat="server" MinDate="1901-01-01">
                                                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                    </CalendarProperties>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblDtDemissao" runat="server" Text="Data de Demissão"></asp:Label>
                                            </td>
                                            <td>
                                                <dxe:ASPxDateEdit ID="dteDtDemissao" runat="server" MinDate="1901-01-01">
                                                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                    </CalendarProperties>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                        </tr>
                                       
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblAnoConcurso" runat="server" Text="Ano do Concurso: "></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtAnoConcurso" runat="server" MaxLength="4" Width="40px" SkinID="numerico"></asp:TextBox>
                                            </td>
                                        </tr>
                                        
                                       
                                        
                                        <table>
                                            <tr>
                                                <td style="text-align: right; width: 15%">
                                                    <asp:Label ID="Label30" runat="server" Text="Disciplina de Ingresso:*" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td colspan="3">
                                                <asp:TextBox ID= "txtDisciplinadeIngresso" Enabled ="false" runat="server" MaxLength="8" Width="131px"></asp:TextBox>
                                            </td>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="Label1" runat="server" Text="Candidato: "></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtCandidato" Enabled="false" runat="server" MaxLength="8" onkeypress="return alphanumeric_only(event);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="Label2" runat="server" Text="Processo Seletivo: "></asp:Label>
                                            </td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtProcesso" Enabled="false" runat="server" MaxLength="8" onkeypress="return alphanumeric_only(event);"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <div>
                                                <asp:Label runat="server" ID="lblMensDocente" SkinID="lblMensagem"></asp:Label>
                                            </div>
                                        </tr>
                                    </table>
                                    <asp:Panel ID="pnlAcumulacao" runat="server" Font-Names="Verdana" GroupingText="Acumulação">
                                        <table>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label19" runat="server" Text="Acumulação:*" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rblAcumulacao" OnSelectedIndexChanged="rblAcumulacao_SelectedIndexChanged"
                                                        AutoPostBack="true" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem Value="1">Sim</asp:ListItem>
                                                        <asp:ListItem Value="0">Não</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label16" runat="server" Text="Matrícula:" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMatriculaAcumulacao" runat="server" MaxLength="20" SkinID="numerico"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label17" runat="server" Text="Órgão:" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtOrgaoAcumulacao" MaxLength="120" runat="server"></asp:TextBox>
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label18" runat="server" Text="Nº de Processo:" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNumProcessoAcumulacao" MaxLength="25" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <br />
                                    <asp:Panel ID="pnlDisciplinaIngresso" runat="server" Visible="false" Font-Names="Verdana"
                                        GroupingText="Disciplina de Ingresso">
                                        <table>
                                            <tr>
                                                <td style="text-align: right; width: 15%">
                                                    <asp:Label ID="Label23" runat="server" Font-Names="Verdana" Text="Disciplina:*" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <tweb:TSearchBox ID="tseDisciplinaIngresso" runat="server" Argument="descricao" ArgumentColumns="70"
                                                        FollowContainerMode="false" MaxLength="20" AutoPostBack="false" Columns="10"
                                                        SqlWhere=" INGRESSO='S' AND ATIVO = 'S'" DataType="VarChar" Key="agrupamento"
                                                        SqlOrder="descricao" SqlSelect="select agrupamento,descricao from ly_grupo_habilitacao ">
                                                        <GridColumns>
                                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="agrupamento" Width="20%" />
                                                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
                                                        </GridColumns>
                                                    </tweb:TSearchBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <br />
                                    <asp:Panel ID="pnlLotacao" runat="server" Visible="false" Font-Names="Verdana" GroupingText="Lotação">
                                        <table>
                                            <tr>
                                                <td style="text-align: right; width: 15%">
                                                    <asp:Label ID="Label22" runat="server" Font-Names="Verdana" Text="Função:*" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <tweb:TSearchBox ID="tseFuncaoLotacao" runat="server" Argument="descricao" ArgumentColumns="70"
                                                        FollowContainerMode="false" MaxLength="20" AutoPostBack="true" Columns="10" DataType="VarChar"
                                                        Key="funcao" OnChanged="tseFuncaoLotacao_Changed" SqlOrder="descricao" SqlSelect="SELECT DISTINCT F.funcao, F.descricao FROM Ly_funcao F INNER JOIN ly_categoria_docente CD ON F.FUNCAO=CD.FUNCAO">
                                                        <GridColumns>
                                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="funcao" Width="20%" />
                                                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
                                                        </GridColumns>
                                                    </tweb:TSearchBox>
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label24" runat="server" Text="CH:* " SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCH" runat="server" MaxLength="2" ReadOnly="true"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right; width: 15%">
                                                    <asp:Label ID="lblRegional" runat="server" Font-Names="Verdana" Text="Regional:*"
                                                        SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <tweb:TSearchBox ID="tseRegionalLotacao" runat="server" Argument="regional" ArgumentColumns="50"
                                                        MaxLength="20" Columns="10" AutoPostBack="True" Caption="" OnChanged="tseRegionalLotacao_Changed"
                                                        SqlOrder="regional" Key="id_regional" SqlSelect="SELECT DISTINCT S.id_regional, regional FROM VW_UNIDADE_ENSINO_SITUACAO S INNER JOIN TCE_REGIONAL R ON S.ID_REGIONAL=R.ID_REGIONAL"
                                                        DataType="Number">
                                                        <GridColumns>
                                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                                                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                                                        </GridColumns>
                                                    </tweb:TSearchBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right; width: 15%">
                                                    <asp:Label Font-Names="Verdana" ID="Label20" runat="server" Text="Município:*" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <tweb:TSearchBox ID="tseMunicipioLotacao" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
                                                        SqlWhere="id_regional IS NOT NULL and id_regional = #tseRegionalLotacao# " GridWidth="600px"
                                                        ArgumentColumns="50" OnChanged="tseMunicipioLotacao_Changed" Columns="10" MaxLength="10">
                                                        <GridColumns>
                                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                                                            <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                                                        </GridColumns>
                                                    </tweb:TSearchBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right; width: 15%">
                                                    <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" SkinID="lblObrigatorio"
                                                        runat="server" Text="Unidade de Ensino:*"></asp:Label>
                                                </td>
                                                <td>
                                                    <tweb:TSearchBox ID="tseUnidadeLotacao" runat="server" Caption="" Key="unidade_ens"
                                                        MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo,municipio,id_regional, ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO "
                                                        SqlWhere=" id_regional = #tseRegionalLotacao# AND municipio = #tseMunicipioLotacao# and situacao = 'ESTADUAL'"
                                                        GridWidth="850px" OnChanged="tseUnidadeLotacao_Changed" SqlOrder="nome_comp">
                                                        <GridColumns>
                                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                                                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="20%" />
                                                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                                                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
                                                            <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="10%" />
                                                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                                                        </GridColumns>
                                                    </tweb:TSearchBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label21" runat="server" Text="Data de Nomeação*" SkinID="lblObrigatorio"></asp:Label>
                                                </td>
                                                <td>
                                                    <dxe:ASPxDateEdit ID="dtNomeacao" runat="server" MinDate="1901-01-01">
                                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                        </CalendarProperties>
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Formação">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl1" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" OnUnload="UpdatePanel1_Unload">
                            <ContentTemplate>
                                <br />
                                <dxwgv:ASPxGridView ID="grdFormacaoPessoal" runat="server" AutoGenerateColumns="False"
                                    EnableCallBacks="False" ClientInstanceName="grdFormacaoPessoal" DataSourceID="odsFormacaoPessoal"
                                    KeyFieldName="ID_FORMACAO_PESSOAL" OnInitNewRow="grdFormacaoPessoal_InitNewRow"
                                    OnCellEditorInitialize="grdFormacaoPessoal_CellEditorInitialize" OnStartRowEditing="grdFormacaoPessoal_StartRowEditing"
                                    OnInit="grdFormacaoPessoal_Init" OnAutoFilterCellEditorInitialize="grdFormacaoPessoal_AutoFilterCellEditorInitialize"
                                    OnRowValidating="grdFormacaoPessoal_RowValidating" OnAfterPerformCallback="grdFormacaoPessoal_AfterPerformCallback"
                                    OnSelectionChanged="grdFormacaoPessoal_SelectionChanged" OnCustomButtonCallback="grdFormacaoPessoal_CustomButtonCallback">
                                    <SettingsBehavior ConfirmDelete="True" />
                                    <SettingsEditing Mode="Inline" />
                                    <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                    <ClientSideEvents RowClick="function(s, e) {
                 doProcessClick = true;
                 visibleIndex = e.visibleIndex+1;
                 var key = s.GetRowKey(e.visibleIndex);
                 
                 
                             
                 
                 window.setTimeout(ProcessClick(key),500);
                 
                 
            }" RowDblClick="function(s, e) {
	doProcessClick = false;
	var key = s.GetRowKey(e.visibleIndex);
	    
	alert('Here is the RowDoubleClick action in a row with the Key = '+descricao);
            }" />
                                    <Columns>
                                        <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="60px">
                                            <%--<EditButton Text="Editar" Visible="True">
                                                <Image Url="~/img/bt_editar.png" />
                                            </EditButton>--%>
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
                                            <CustomButtons>
                                                <dxwgv:GridViewCommandColumnCustomButton Text="Editar" ID="Editar" Visibility="AllDataRows"
                                                    Image-Url="~/img/bt_editar.png" Image-Height="15px" Image-AlternateText="Editar">
                                                </dxwgv:GridViewCommandColumnCustomButton>
                                            </CustomButtons>
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
                                        <dxwgv:GridViewDataTextColumn Visible="false" Caption="Código da Área" FieldName="CODIGOAREA"
                                            VisibleIndex="4">
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
                                        <dxwgv:GridViewDataTextColumn Visible="false" Caption="Código do Curso" FieldName="CODIGOCURSO"
                                            VisibleIndex="5">
                                        </dxwgv:GridViewDataTextColumn>
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
                                        <dxwgv:GridViewDataTextColumn Visible="false" Caption="Tipo Instituição" FieldName="TIPOINSTITUICAO"
                                            VisibleIndex="9">
                                        </dxwgv:GridViewDataTextColumn>
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
                                <p>
                                    <dxwgv:ASPxGridView ID="grdDisciplinaAdicional" runat="server" AutoGenerateColumns="False"
                                        Visible="false" ClientInstanceName="grdDisciplinaAdicional" DataSourceID="odsDisciplina"
                                        KeyFieldName="ID_COMPOSTO" OnCellEditorInitialize="grdDisciplinaAdicional_CellEditorInitialize"
                                        OnInitNewRow="grdDisciplinaAdicional_InitNewRow" OnCustomUnboundColumnData="grdDisciplinaAdicional_CustomUnboundColumnData"
                                        OnRowDeleting="grdDisciplinaAdicional_RowDeleting" OnRowUpdating="grdDisciplinaAdicional_RowUpdating"
                                        OnRowInserting="grdDisciplinaAdicional_RowInserting" OnStartRowEditing="grdDisciplinaAdicional_StartRowEditing"
                                        OnRowValidating="grdDisciplinaAdicional_RowValidating" OnAfterPerformCallback="grdDisciplinaAdicional_AfterPerformCallback"
                                        Width="100%">
                                        <SettingsBehavior ConfirmDelete="True" />
                                        <SettingsEditing Mode="Inline" />
                                        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                        <Columns>
                                            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                                <HeaderCaptionTemplate>
                                                    <div style="text-align: center">
                                                        <img runat="server" id="btnNovoGrid" alt="Novo" src="../img/bt_novo.png" onclick="grdDisciplinaAdicional.AddNewRow();" />
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
                                            <dxwgv:GridViewDataTextColumn Caption="Composto" FieldName="ID_COMPOSTO" VisibleIndex="1"
                                                Visible="False">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="FormaçãoID" HeaderStyle-Font-Bold="true" FieldName="FORMACAOPESSOALID"
                                                VisibleIndex="2">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataTextColumn Caption="DisciplinaID" HeaderStyle-Font-Bold="true"
                                                FieldName="ESTUDOADICIONALID" VisibleIndex="3">
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataComboBoxColumn Caption="Disciplina" FieldName="NOME_DISCIPLINA_ADICIONAL"
                                                VisibleIndex="4">
                                                <PropertiesComboBox DataSourceID="odsDisciplinaAdicional" TextField="NOME_DISCIPLINA_ADICIONAL"
                                                    ValueField="estudoadicionalid" ValueType="System.String">
                                                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                        <RequiredField ErrorText="Favor informar a disciplina." IsRequired="True" />
                                                    </ValidationSettings>
                                                </PropertiesComboBox>
                                            </dxwgv:GridViewDataComboBoxColumn>
                                        </Columns>
                                        <Styles>
                                            <CommandColumn Wrap="False">
                                            </CommandColumn>
                                        </Styles>
                                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                                    </dxwgv:ASPxGridView>
                                </p>
                                <asp:Panel ID="PanelGraduacao" runat="server" GroupingText="Formação" Width="850px">
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
                                            <td rowspan="10">
                                                <p>
                                                    <asp:Panel ID="pnDisciplinaAdicional" runat="server" GroupingText="Estudos Adicionais"
                                                        Enabled="true" Visible="true" Height="200px" Width="184px" top="100">
                                                        <asp:CheckBoxList ID="chkListDisciplinaAdicional" runat="server" DataTextField="NOME_DISCIPLINA_ADICIONAL"
                                                            DataValueField="estudoadicionalid" DataSourceID="odsDisciplinaAdicional" AutoPostBack="true"
                                                            RepeatDirection="Vertical">
                                                        </asp:CheckBoxList>
                                                    </asp:Panel>
                                                </p>
                                            </td>
                                        </tr>
                                        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}"
                                            SelectMethod="ListarTipoCurso" TypeName="Techne.Lyceum.Net.Basico.Capacitacao">
                                        </asp:ObjectDataSource>
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
                                                <asp:Label ID="lblDocComprob" runat="server" Text="Documentos Comprobatórios?" SkinID="lblObrigatorio"></asp:Label>
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
                                                    <asp:Button ID="btnSalvarFormacao" AutoPostBack="true" runat="server" Enabled="True"
                                                        ValidationGroup="SalvarForm" Text="Incluir Formação Pessoal - Graduação" OnClick="btnSalvarFormacao_Click" />
                                                </p>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <br />
                                <p align="center">
                                    <asp:Label ID="lblMensValidacao" runat="server" SkinID="lblMensagem"></asp:Label>
                                </p>
                                <br />
                                <asp:Panel ID="Panel3" runat="server" GroupingText="Pós-Graduação" Width="850px">
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
                                                        <asp:ControlParameter ControlID="ddlTipoInstituicao" Name="TIPO_ORIGEM" PropertyName="SelectedValue" />
                                                    </QueryParameters>
                                                </tweb:TSearch>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label12" runat="server" Text="Documentos Comprobatórios?" SkinID="lblObrigatorio"></asp:Label>
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
                                                    <asp:Button ID="btnSalvarFormacaoPosGraduacao" AutoPostBack="true" runat="server"
                                                        Enabled="True" ValidationGroup="SalvarForm" Text="Incluir Formação Pessoal - Pós-Graduação"
                                                        OnClick="btnSalvarFormacaoPosGraduacao_Click" />
                                                </p>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <p align="center">
                                    <asp:TextBox ID="txtFormacaoPessoalID" runat="server" MaxLength="10" ReadOnly="false"
                                        Visible="false"></asp:TextBox>
                                </p>
                                <p>
                                    <asp:ObjectDataSource ID="odsDisciplina" TypeName="Techne.Lyceum.Net.Academico.Servidor"
                                        runat="server" SelectMethod="ListarPessoaDisciplinaAdic" UpdateMethod="Update"
                                        DeleteMethod="Delete">
                                        <SelectParameters>
                                            <asp:ControlParameter ControlID="txtFormacaoPessoalID" PropertyName="Text" Name="formacaopessoalid" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </p>
                                <asp:ObjectDataSource ID="odsDisciplinaAdicional" TypeName="Techne.Lyceum.RN.Conceito"
                                    SelectMethod="ConsultarDisciplinaAdicional" runat="server"></asp:ObjectDataSource>
                                <asp:ObjectDataSource ID="odsFormacaoPessoal" TypeName="Techne.Lyceum.Net.Academico.Servidor"
                                    runat="server" SelectMethod="ListarPessoa" DeleteMethod="Delete" OnDeleting="odsArea_Deleting"
                                    OnUpdating="odsArea_Updating">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="txtPessoa" PropertyName="Text" Name="pessoa" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <br />
                                <div style="width: 100%; text-align: left">
                                    <dxe:ASPxButton ClientInstanceName="btnAnterior" AutoPostBack="false" UseSubmitBehavior="false"
                                        ID="ASPxButton9" runat="server" Text="<< Anterior">
                                        <ClientSideEvents Click="function(s, e) {
	                                pcPessoa.ChangeActiveTab(4, false);}" />
                                    </dxe:ASPxButton>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Capacitação">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl3" runat="server">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" OnUnload="UpdatePanel1_Unload">
                            <ContentTemplate>
                                <dxwgv:ASPxGridView ID="grdCapacitacao" runat="server" AutoGenerateColumns="False"
                                    Visible="true" ClientInstanceName="grdCapacitacao" KeyFieldName="CompositeKey"
                                    DataSourceID="odsDocenteCursoCapacitacao" OnAfterPerformCallback="grdCapacitacao_AfterPerformCallback"
                                    OnStartRowEditing="grdCapacitacao_StartRowEditing" OnCellEditorInitialize="grdCapacitacao_CellEditorInitialize"
                                    OnCustomUnboundColumnData="grdCapacitacao_CustomUnboundColumnData" Width="1200px"
                                    OnCancelRowEditing="grdCapacitacao_CancelRowEditing" OnRowDeleting="grdCapacitacao_RowDeleting"
                                    OnRowUpdating="grdCapacitacao_RowUpdating">
                                    <SettingsBehavior ConfirmDelete="True" />
                                    <SettingsEditing Mode="Inline" />
                                    <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                    <Columns>
                                        <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                            <EditButton Text="Editar" Visible="True">
                                                <Image Url="~/img/bt_editar.png" />
                                            </EditButton>
                                            <DeleteButton Text="Remover" Visible="True">
                                                <Image Url="~/img/bt_exclui2.png" />
                                            </DeleteButton>
                                            <UpdateButton Text="Salvar">
                                                <Image Url="~/img/bt_salvar.png" />
                                            </UpdateButton>
                                            <CancelButton Text="Cancelar">
                                                <Image Url="~/img/bt_cancelar.png" />
                                            </CancelButton>
                                            <ClearFilterButton Text="Limpar" Visible="True">
                                                <Image Url="~/img/bt_limpa.png" />
                                            </ClearFilterButton>
                                        </dxwgv:GridViewCommandColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Pessoa" FieldName="PESSOAID" VisibleIndex="1"
                                            Visible="False">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Capacitacao" FieldName="CURSOCAPACITACAOID"
                                            VisibleIndex="2" Visible="False">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataComboBoxColumn Caption="Oferecido SEEDUC" HeaderStyle-Font-Bold="true"
                                            FieldName="OFERECIDOSEEDUC" VisibleIndex="3" Width="100px" ReadOnly="true">
                                            <PropertiesComboBox ValueType="System.String">
                                                <Items>
                                                    <dxe:ListEditItem Text="Sim" Value="1" />
                                                    <dxe:ListEditItem Text="Não" Value="0" />
                                                </Items>
                                            </PropertiesComboBox>
                                        </dxwgv:GridViewDataComboBoxColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Curso/Capacitação*" HeaderStyle-Font-Bold="true"
                                            FieldName="NOMECURSO" VisibleIndex="4" Width="200px">
                                            <PropertiesTextEdit MaxLength="100">
                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                    <RequiredField ErrorText="Favor informar o Nome do Curso." IsRequired="True" />
                                                </ValidationSettings>
                                            </PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataComboBoxColumn Caption="Tipo do Curso*" FieldName="TIPOCURSOCAPACITACAOID"
                                            VisibleIndex="5" Width="200px">
                                            <PropertiesComboBox DataSourceID="odsTipoCursoCapacitacao" TextField="DESCRICAO"
                                                ValueField="TIPOCURSOCAPACITACAOID" ValueType="System.String">
                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                    <RequiredField ErrorText="Favor informar o Tipo do Curso." IsRequired="True" />
                                                </ValidationSettings>
                                            </PropertiesComboBox>
                                        </dxwgv:GridViewDataComboBoxColumn>
                                        <dxwgv:GridViewDataComboBoxColumn Caption="Área de Conhecimento*" FieldName="AREACONHECIMENTOID"
                                            VisibleIndex="6" Width="200px">
                                            <PropertiesComboBox DataSourceID="odsAreaConhecimentoCapacitacao" TextField="DESCRICAO"
                                                ValueField="AREACONHECIMENTOID" ValueType="System.String">
                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                    <RequiredField ErrorText="Favor informar a Área de Conhecimento." IsRequired="True" />
                                                </ValidationSettings>
                                            </PropertiesComboBox>
                                        </dxwgv:GridViewDataComboBoxColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Nome da Instituição*" HeaderStyle-Font-Bold="true"
                                            FieldName="NOMEINSTITUICAO" VisibleIndex="7">
                                            <PropertiesTextEdit MaxLength="100">
                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                    <RequiredField ErrorText="Favor informar o Nome da Instituição." IsRequired="True" />
                                                </ValidationSettings>
                                            </PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataSpinEditColumn Caption="Carga Horária*" FieldName="CARGAHORARIA"
                                            VisibleIndex="8" Width="70px">
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
                                            FieldName="DATACONCLUSAO" VisibleIndex="9" Width="100px">
                                            <PropertiesDateEdit>
                                                <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                                    <RequiredField ErrorText="Favor informar a Data Conclusão." IsRequired="True" />
                                                </ValidationSettings>
                                            </PropertiesDateEdit>
                                        </dxwgv:GridViewDataDateColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                                            Visible="False" VisibleIndex="10">
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                    <Styles>
                                        <CommandColumn Wrap="False">
                                        </CommandColumn>
                                    </Styles>
                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                                </dxwgv:ASPxGridView>
                                <br />
                                <asp:Panel ID="pnGeralCapacitacao" runat="server" GroupingText="Informe os dados para inclusão / Consulta:"
                                    Width="800px">
                                    <table>
                                        <tr>
                                            <td style="text-align: right; width: 25%">
                                                <asp:Label ID="lblOferecidoSEEDUC" runat="server" Text="Oferecido pela SEEDUC:*"
                                                    SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rbtListOferecidoSEEDUC" runat="server" RepeatDirection="Horizontal"
                                                    AutoPostBack="true" OnSelectedIndexChanged="rbtListOferecidoSEEDUC_SelectedIndexChanged">
                                                    <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Não" Value="0"></asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 25%">
                                                <asp:Label ID="lblTipoCursoCapacitacao" runat="server" Text="Tipo de Curso:*" SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlTipoCursoCapacitacao" runat="server" DataTextField="DESCRICAO"
                                                    DataValueField="TIPOCURSOCAPACITACAOID" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoCursoCapacitacao_SelectedIndexChanged"
                                                    AppendDataBoundItems="true" Width="300px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; width: 25%">
                                                <asp:Label ID="lblAreaConhecimentoCapacitacao" runat="server" Text="Área de Conhecimento:*"
                                                    SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlAreaConhecimentoCapacitacao" runat="server" DataTextField="DESCRICAO"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlAreaConhecimentoCapacitacao_SelectedIndexChanged"
                                                    DataValueField="AREACONHECIMENTOID" AppendDataBoundItems="true" Width="300px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblCursoCapacitacao" runat="server" Text="Curso/Capacitação:*" SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCursoCapacitacao" runat="server" Width="500px" MaxLength="100"></asp:TextBox>
                                                <asp:DropDownList ID="ddlCursoCapacitacao" runat="server" DataTextField="NOMECURSO"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlCursoCapacitacao_SelectedIndexChanged"
                                                    DataValueField="CURSOCAPACITACAOID" AppendDataBoundItems="true" Width="300px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblNomeInstituicaoCapacitacao" runat="server" Text="Nome da Instituição:*"
                                                    SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtNomeInstituicaoCapacitacao" runat="server" Width="500px" MaxLength="200"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblCargaHorariaCapacitacao" runat="server" Text="Carga Horária:*"
                                                    SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCargaHorariaCapacitacao" runat="server" Width="80px" MaxLength="5"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblDataConclusaoCapacitacao" runat="server" Text="Data de Conclusão:*"
                                                    SkinID="lblObrigatorio"></asp:Label>
                                            </td>
                                            <td>
                                                <dxe:ASPxDateEdit ID="dteDataConclusaoCapacitacao" runat="server" MinDate="1901-01-01">
                                                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                    </CalendarProperties>
                                                </dxe:ASPxDateEdit>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="right">
                                                <asp:Button ID="btnSalvarCapacitacao" runat="server" ValidationGroup="SalvarForm"
                                                    Text="Incluir Capacitação" OnClick="btnSalvarCapacitacao_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <br />
                                <asp:Label ID="lblMensagemCapacitacao" runat="server" SkinID="lblMensagem"></asp:Label>
                                <input type="hidden" value="<%= minCargaHorariaCursoCapacitacao %>" id="minCargaHorariaCursoCapacitacao"
                                    name="minCargaHorariaCursoCapacitacao" />
                                <br />
                                <br />
                                <asp:ObjectDataSource ID="odsDocenteCursoCapacitacao" TypeName="Techne.Lyceum.Net.Basico.Docentes"
                                    runat="server" SelectMethod="ListarDocenteCursoCapacitacao" OldValuesParameterFormatString="{0}"
                                    OnDeleting="odsDocenteCursoCapacitacao_Deleting" DeleteMethod="DeleteCursoCapacitacao"
                                    OnUpdating="odsDocenteCursoCapacitacao_Updating" UpdateMethod="AlteraCursoCapacitacao">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="txtPessoa" PropertyName="Text" Name="idPessoa" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <asp:ObjectDataSource ID="odsAreaConhecimentoCapacitacao" runat="server" OldValuesParameterFormatString="original_{0}"
                                    SelectMethod="ListarAreaConhecimento" TypeName="Techne.Lyceum.Net.Basico.Docentes">
                                </asp:ObjectDataSource>
                                <asp:ObjectDataSource ID="odsTipoCursoCapacitacao" runat="server" OldValuesParameterFormatString="original_{0}"
                                    SelectMethod="ListarTipoCurso" TypeName="Techne.Lyceum.Net.Basico.Docentes">
                                </asp:ObjectDataSource>
                                <br />
                                <br />
                                <br />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
</asp:Content>
