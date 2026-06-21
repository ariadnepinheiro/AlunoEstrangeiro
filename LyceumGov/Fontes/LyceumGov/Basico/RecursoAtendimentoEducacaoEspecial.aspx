<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="RecursoAtendimentoEducacaoEspecial.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.RecursoAtendimentoEducacaoEspecial" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
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

    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe o CPF ou o nome do recurso"
        Height="45px" Width="595px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblRecursoTSearch" runat="server" Text="Recurso*: " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseRecurso" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryRecurso"
                        AutoPostBack="true" OnTextChanged="tseRecurso_Changed" MaxLength="11">
                    </tweb:TSearch>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:Label ID="lblmensagemBloqueio" Text="Havendo divergência de informação, fazer acerto pelo módulo de gestão de pessoas ou gestão de ensino."
        runat="server" Style="display: inline-table; color: #FF0000; font-weight: bold;"
        Visible="false"></asp:Label>
    <br />
    <div class="divEditBlock" style="width: 768px;">
        <asp:ImageButton ID="btnDesabilitar" runat="server" SkinID="BcDesabilitar" OnClick="btnDesabilitar_Click"  ImageUrl="~/Images/bot_desabil.png" OnClientClick="return confirm('Confirma a desabilitação do Recurso?');" />
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
            ValidationGroup="SalvarForm" />
        <asp:Label runat="server" ID="lblBloco" Text="Recurso Atendimento Educação Especial"
            SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsRecurso" runat="server" ShowMessageBox="true" ValidationGroup="SalvarForm"
            ShowSummary="false" />
    </div>
    <br />
    <asp:Panel ID="pnlBuscaCPF" runat="server" GroupingText="Informe o CPF" Height="45px"
        AutoPostBack="true" Style="width: 768px;" Visible="false">
        <table>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblCPF" runat="server" Text="CPF:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td style="text-align: left">
                    <asp:TextBox ID="txtCPF" runat="server" onkeypress="formataCPF(this,event)" MaxLength="14"
                        AutoPostBack="true" Width="100px" OnTextChanged="txtCPF_TextChanged"></asp:TextBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <div id="divDados" runat="server" visible="false" style="width: 768px;">
        <asp:Panel ID="pnlDadosPessoais" runat="server" GroupingText="Dados Pessoais">
            <asp:HiddenField ID="hdnPessoa" runat="server" />
            <asp:HiddenField ID="hdnRecursoId" runat="server" />
            <asp:HiddenField ID="hdnBloqueado" runat="server" />
            <table>  
             <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label2" runat="server" Text="Ativo?:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblAtivo" runat="server" RepeatDirection="Horizontal" >
                            <asp:ListItem Text="Sim" Value="1" Selected="true"></asp:ListItem>
                            <asp:ListItem Text="Não" Value="0" ></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>             
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label1" runat="server" Text="Tipo Recurso:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBoxList ID="chkTipoRecurso" runat="server" RepeatDirection="Horizontal"
                            RepeatLayout="Table" Width="100%">
                        </asp:CheckBoxList>
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
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblEstadoCivil" runat="server" Text="Estado Civil:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlEst_Civil" runat="server" DataTextField="descr" DataValueField="item">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </asp:Panel>
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
                            Modal="true" SkinID="CEP" MaxLength="8" />
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
                        <asp:Label ID="lblFone" runat="server" Text="Telefone: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTelefone" onkeyup="formataTelefoneDDD(this,event)" runat="server"
                            MaxLength="30"></asp:TextBox>                       
                    </td>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" ID="lblCelular" runat="server" Text="Celular: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCelular" onkeyup="formataTelefoneDDD(this,event)" runat="server"
                            MaxLength="30"></asp:TextBox>                        
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblEmail" runat="server" Text="E-mail: "></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" Columns="50"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="reEmail" runat="server" ControlToValidate="txtEmail"
                            ErrorMessage="E-mail inválido." ValidationGroup="SalvarForm" SetFocusOnError="true"
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"><img src="../Images/AlertaMens.gif" alt="E-mail inválido" /></asp:RegularExpressionValidator>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
    </div>
</asp:Content>
