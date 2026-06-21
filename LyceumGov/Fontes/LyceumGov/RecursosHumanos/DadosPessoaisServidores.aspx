<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="DadosPessoaisServidores.aspx.cs" Inherits="Techne.Lyceum.Net.RecursosHumanos.DadosPessoaisServidores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
<script type="text/javascript">

    $().ready(function() {
        trataCep({ tscep: '<%=tsCEP.ClientID %>',
            cep: '<%=txtCEP.ClientID %>',
            nomeLogradouro: '<%=txtEndereco.ClientID %>',
            nomeBairro: '<%=txtBairro.ClientID %>',
            nomeMunicipio: '<%=txtMunicipio.ClientID %>',
            codigoMunicipio: '<%=tseMunicipio.ClientID %>',
            uf: '<%=txtEstado.ClientID %>'
        });

    });
    </script>
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Servidor/Funcionário" Width="800px">
        <table>
            <tr>              
                <td>
                    <tweb:TSearchBox ID="tseUsuario" runat="server" Argument="nomeusuario" Caption=""
                        Key="usuario" SqlOrder="usuario" SqlSelect="SELECT usuario, nomeusuario, p.cpf, matricula, p.e_mail_interno,p.pessoa FROM usuario u left join LY_PESSOA p on u.PESSOA_USUARIO = p.PESSOA"
                        Enabled="false">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Usuário" FieldName="usuario" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nomeusuario" Width="55%" />
                            <tweb:TSearchBoxColumn Caption="CPF" FieldName="cpf" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Pessoa" FieldName="pessoa" Width="30%" Visible="false" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <div class="divEditBlock" style="width: 786px;">
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalva" OnClientClick="return confirmSalvar();"
            OnClick="btnSalvar_Click" />
        <asp:Label runat="server" ID="lblBloco" Text="Servidores/Funcionários" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsPessoas" runat="server" ShowMessageBox="true" ValidationGroup="SalvarForm"
            ShowSummary="false" />
    </div>
    <br />
    <asp:HiddenField ID="hdnPessoa" runat="server" />
    <asp:Panel ID="pnlDadosPessoais" runat="server" GroupingText="Dados Pessoais">
        <table>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblNomeComplPessoa" runat="server" Text="Nome Completo:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtNomeCompl" runat="server" MaxLength="100" Columns="80" onkeypress="return nomeSemNum(event);">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label29" runat="server" Text=" Nome Social:"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtNomeSocial" runat="server" MaxLength="100" Columns="80" onkeypress="return nomeSemNum(event);">
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
                    <asp:DropDownList ID="ddlEstadoCivil" runat="server" DataTextField="descr" DataValueField="item">
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
                    <asp:TextBox ID="txtEndereco" runat="server" MaxLength="50" Columns="50" onkeypress="return endereco(event);"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblEndNumPessoa" runat="server" Text="Nº.:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEndNum" runat="server" MaxLength="15"></asp:TextBox>
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
</asp:Content>
