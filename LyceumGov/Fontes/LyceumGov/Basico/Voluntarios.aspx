<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Voluntarios.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.Voluntarios" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<asp:Content ID="cnVoluntarios" ContentPlaceHolderID="cphFormulario" runat="server">
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
        <!--
        $().ready(function() {
            trataCep({ tscep: '<%=tsCEP.ClientID %>',
                cep: '<%=txtCEP.ClientID %>',
                nomeLogradouro: '<%=txtEnderecoPessoa.ClientID %>',
                nomeBairro: '<%=txtBairro.ClientID %>',
                nomeMunicipio: '<%=txtMunicipio.ClientID %>',
                codigoMunicipio: '<%=tseMunicipio.ClientID %>',
                uf: '<%=txtEstado.ClientID %>'
            });
        });
        -->
    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe a matrícula ou o nome do voluntário/estagiário"
        Height="45px" Width="695px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblVoluntarioTSearch" runat="server" Text="Voluntário/Estagiário*: "
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseVoluntarios" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryVoluntarioCad"
                        AutoPostBack="true" OnTextChanged="tseVoluntarios_Changed" OnLoad="tseVoluntarios_Load">
                    </tweb:TSearch>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <div class="divEditBlock" style="width: 768px;">
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
            ValidationGroup="SalvarForm" />
        <asp:Label runat="server" ID="lblBloco" Text="Voluntários/Estagiários" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsDocente" runat="server" ShowMessageBox="true" ValidationGroup="SalvarForm"
            ShowSummary="false" />
    </div>
    <br />
    <asp:Label ID="lblmensagemBloqueio" Text="CPF de aluno ativo. Havendo divergência de informação, fazer acerto pelo módulo gestão escolar."
        runat="server" Style="display: inline-table; color: #FF0000; font-weight: bold;"
        Visible="false"></asp:Label>
    <br />
    <asp:Panel ID="pnlBuscaCPF" runat="server" GroupingText="Informe o CPF" Height="45px"
        AutoPostBack="true" Style="width: 768px;" Visible="false">
        <table>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label1" runat="server" Text="CPF:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtCPFBusca" runat="server" onkeypress="formataCPF(this,event)"
                        MaxLength="14" AutoPostBack="true" Width="100px" OnTextChanged="txtCPFBusca_TextChanged"></asp:TextBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <div id="divDados" runat="server" visible="false" style="width: 768px;">
        <dxtc:ASPxPageControl ID="apcVoluntario" runat="server" Height="299px" Width="768px"
            ActiveTabIndex="3" TabIndex="1">
            <TabPages>
                <dxtc:TabPage Name="DadosPessoais" Text="Dados Pessoais">
                    <ContentCollection>
                        <dxw:ContentControl ID="conVoluntario" runat="server">
                            <br />
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
                                        <td style="text-align: right">
                                            <asp:Label ID="Label2" runat="server" SkinID="lblObrigatorio" Text="Função:* "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rblFuncao" runat="server" RepeatDirection="Horizontal" DataValueField="sexo"
                                                Width="250px">
                                                <asp:ListItem Text="Regente Mais Educação" Value="REG MAIS EDUCACAO"></asp:ListItem>
                                                <asp:ListItem Text="Estagiário" Value="ESTAGIARIO (ECO)"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblCodigoVoluntario" runat="server" Text="Voluntário/Estagiários:"
                                                SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtVoluntario" runat="server" ReadOnly="true" Enabled="FALSE"></asp:TextBox>
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
                                                onkeypress="return nomeSemNum(event);"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblNomeSocial" runat="server" Text="Nome Social:* " SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtNomeSocial" runat="server" MaxLength="100" Columns="80" onkeypress="return nomeSemNum(event);"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
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
                                                <asp:DropDownList ID="ddlRaca" runat="server" DataTextField="descr" DataValueField="item">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label ID="lblNecessidadeEspecial" runat="server" Text="Necessidade Especial: "
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
                            <br />
                            <asp:Panel ID="pnlEndereco" runat="server" GroupingText="Endereço" Font-Names="Verdana">
                                <table>
                                    <tr>
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
                                            <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect="SELECT codigo, nome, uf_sigla FROM municipio"
                                                GridWidth="600px" ArgumentColumns="30" Columns="10" OnChanged="tseMunicipio_Changed"
                                                MaxLength="10">
                                                <GridColumns>
                                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                                    <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                                                    <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                                                </GridColumns>
                                            </tweb:TSearchBox>
                                            <asp:TextBox ID="txtMunicipio" runat="server" MaxLength="20" Visible="false"></asp:TextBox>
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
                                            <asp:Label ID="lblFone" runat="server" Text="Telefone: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFone" onkeyup="formataTelefoneDDD(this,event)" runat="server"
                                                MaxLength="30"></asp:TextBox>
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
                                            <asp:Label ID="lblEmail" runat="server" Text="E-mail Institucional: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" Columns="50"></asp:TextBox>
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
                                            <asp:Label ID="lblRGUFPessoa" runat="server" Text="Estado: "></asp:Label>
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
                                            <asp:Label ID="lblRGDataExpPessoa" runat="server" Text="Data de Expedição: "></asp:Label>
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
                                            <asp:Label ID="lblPISPASEP" runat="server" Text="PIS/PASEP: "></asp:Label>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:TextBox ID="txtPISPASEP" runat="server" MaxLength="11" SkinID="numerico"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Name="Vinculos" Text="Vínculos">
                    <ContentCollection>
                        <dxw:ContentControl ID="ccVinculos" runat="server">
                            <asp:Panel ID="pnlVinculos" runat="server" GroupingText="Vinculos" Height="123px">
                                <table>
                                    <tr>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblUnidadeEnsino" runat="server" Text="Unidade de Ensino:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <tweb:TSearchBox ID="tseUnidadeEnsino" runat="server" Caption="" Key="unidade_ens"
                                                MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT DISTINCT nome_comp, e.unidade_ens, setor, cgc FROM  dbo.LY_UNIDADE_ENSINO_CURSOS c INNER JOIN dbo.LY_UNIDADE_ENSINO e ON c.UNIDADE_ENS = e.UNIDADE_ENS "
                                                GridWidth="650px" SqlWhere="c.CURSO = '9999.92'" SqlOrder="nome_comp">
                                                <GridColumns>
                                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                                                    <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                                                </GridColumns>
                                            </tweb:TSearchBox>
                                        </td>
                                    </tr>
                                    <br />
                                    <tr>
                                        <td style="text-align: left">
                                            <asp:Label ID="lblDataInicio" runat="server" Text="Data de Inicio:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td colspan="1">
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxDateEdit ID="dtVincInicio" runat="server">
                                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                            </CalendarProperties>
                                                        </dxe:ASPxDateEdit>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblDataFim" runat="server" Text="Data Fim:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td colspan="1">
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxDateEdit ID="dtVincFim" runat="server">
                                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                                            </CalendarProperties>
                                                        </dxe:ASPxDateEdit>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPrincipal" runat="server" Text="Principal: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chkPrincipal" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="right">
                                            <asp:Button ID="btnSalvarVinculo" runat="server" ValidationGroup="SalvarForm" Text="Salvar"
                                                OnClick="btnSalvarVinculo_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <br />
                            <br />
                            <asp:Panel ID="pnGrid" runat="server">
                                <dxwgv:ASPxGridView ID="grdVinculos" runat="server" AutoGenerateColumns="False" ClientInstanceName="grdVinculos"
                                    KeyFieldName="ID_VINCULO;UNIDADE_ENS" DataSourceID="odsVinculos" OnCellEditorInitialize="grdVinculos_CellEditorInitialize"
                                    OnStartRowEditing="grdVinculos_StartRowEditing" OnRowValidating="grdVinculos_RowValidating"
                                    OnAfterPerformCallback="grdVinculos_AfterPerformCallback">
                                    <Columns>
                                        <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                            <EditButton Text="Editar" Visible="True">
                                                <Image Url="~/img/bt_editar.png" />
                                            </EditButton>
                                            <DeleteButton Text="Remover" Visible="false">
                                                <Image Url="~/img/bt_exclui2.png" />
                                            </DeleteButton>
                                            <CancelButton Text="Cancelar">
                                                <Image Url="~/img/bt_cancelar.png" />
                                            </CancelButton>
                                            <UpdateButton Text="Salvar">
                                                <Image Url="~/img/bt_salvar.png" />
                                            </UpdateButton>
                                        </dxwgv:GridViewCommandColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="ID_VINCULO" Visible="false"
                                            VisibleIndex="1">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Censo" FieldName="UNIDADE_ENS" Visible="false"
                                            VisibleIndex="1">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="NOME_COMP" VisibleIndex="3">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataDateColumn Caption="Data Início" FieldName="DT_INICIO" VisibleIndex="4">
                                        </dxwgv:GridViewDataDateColumn>
                                        <dxwgv:GridViewDataDateColumn Caption="Data Fim" FieldName="DT_FIM" VisibleIndex="5">
                                        </dxwgv:GridViewDataDateColumn>
                                        <dxwgv:GridViewDataCheckColumn Caption="Principal" FieldName="PRINCIPAL" VisibleIndex="6">
                                        </dxwgv:GridViewDataCheckColumn>
                                    </Columns>
                                    <SettingsBehavior ConfirmDelete="True" />
                                    <SettingsEditing Mode="Inline" />
                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                                    <%-- <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />--%>
                                </dxwgv:ASPxGridView>
                            </asp:Panel>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
            </TabPages>
        </dxtc:ASPxPageControl>
    </div>
    <asp:ObjectDataSource ID="odsVinculos" TypeName="Techne.Lyceum.Net.Basico.Voluntarios"
        runat="server" SelectMethod="Listar" UpdateMethod="Update" OnUpdating="odsVinculos_Updating">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseVoluntarios" DefaultValue="" Name="matricula"
                PropertyName="DBValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
