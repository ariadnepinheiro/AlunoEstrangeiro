<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Tesoureiro.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.Tesoureiro" %>

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

        function formataCPFTesoureiro(b, a) {
            a = getEvent(a);
            var c = getKeyCode(a);
            if (!teclaValida(c)) {
                return
            }
            vr = b.value = filtraNumeros(filtraCampo(b));
            if (vr.length > 11) {
                vr = vr.substr(0, 11)
            }
            tam = vr.length;
            if (tam <= 2) {
                b.value = vr
            }
            if (tam > 2 && tam <= 5) {
                b.value = vr.substr(0, tam - 2) + "-" + vr.substr(tam - 2, tam)
            }
            if (tam >= 6 && tam <= 8) {
                b.value = vr.substr(0, tam - 5) + "." + vr.substr(tam - 5, 3) + "-" + vr.substr(tam - 2, tam)
            }
            if (tam >= 9 && tam <= 11) {
                b.value = vr.substr(0, tam - 8) + "." + vr.substr(tam - 8, 3) + "." + vr.substr(tam - 5, 3) + "-" + vr.substr(tam - 2, tam)
            }
        }

        function formataFixoCelularDDD(b, a) {
            //lert(b);
            vr = b.value = filtraNumeros(filtraCampo(b));
            tam = vr.length;
            if (tam < 10)
                return;

            if (tam == 11) {
                formataCelularDDD(b, a);
            }
            if (tam == 10) {
                formataTelefoneDDD(b, a);

            }
        }
    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe os dados para pesquisa"
        Height="45px" Width="600px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblTSearch" runat="server" Text="Tesoureiro*: " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseTesoureiro" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryTesoureiro"
                        AutoPostBack="true" OnTextChanged="tseTesoureiro_Changed" MaxLength="11">
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
        <asp:ImageButton ID="btnExcluir" runat="server" SkinID="BcDeletar" OnClick="btnExcluir_Click"
            OnClientClick="return confirm('Confirma a remoção?');" />
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
            ValidationGroup="SalvarForm" />
        <asp:Label runat="server" ID="lblBloco" Text="Tesoureiro"
            SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsTesoureiro" runat="server" ShowMessageBox="true" ValidationGroup="SalvarForm"
            ShowSummary="false" />
    </div>
    <br />
    <br />
    <div id="divDados" runat="server" visible="false" style="width: 768px;">
        <asp:Panel ID="pnlDadosPessoais" runat="server" GroupingText="Dados Pessoais">
            <asp:HiddenField ID="hdnTesoureiroId" runat="server" />
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblCPF" runat="server" Text="CPF:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtCPF" onkeyup="formataCPFTesoureiro(this,event)" runat="server" MaxLength="14"
                            Width="150px" />
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
                        <asp:Label ID="Label1" runat="server" Text="RG:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td style="text-align: left">
                        <asp:TextBox ID="txtRG" runat="server" MaxLength="14"
                            Width="150px" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
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
                        <asp:Label ID="lblFone" runat="server" SkinID="lblObrigatorio" Text="Telefone: *"></asp:Label>
                    </td>
                    <td>
                         <asp:TextBox ID="txtTelefone" onkeyup="formataFixoCelularDDD(this,event)" runat="server"
                            MaxLength="14" Width="100px" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblEmail" runat="server" SkinID="lblObrigatorio" Text="E-mail: *"></asp:Label>
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
