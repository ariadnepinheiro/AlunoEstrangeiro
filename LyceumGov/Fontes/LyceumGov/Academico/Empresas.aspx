<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Empresas.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.Empresas"
    MasterPageFile="~/Modulos/LyceumMaster.Master" %>

<asp:Content ID="conEmpresas" runat="server" ContentPlaceHolderID="cphFormulario">

    <script type="text/javascript">
    <!--
        $().ready(function() {
            trataCep({
                tscep: '<%=tsCEP.ClientID %>',
                cep: '<%=txtCEP.ClientID %>',
                nomeLogradouro: '<%=txtEndereco.ClientID %>',
                nomeBairro: '<%=txtBairro.ClientID %>',
                codigoMunicipio: '<%=tseMunicipio.ClientID %>',
                uf: '<%=txtEstado.ClientID %>'
            });
        });
    -->    
    </script>

    <asp:Panel ID="pnlBuscarEmpresas" runat="server" GroupingText="Selecione a empresa desejada"
        Width="650px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblBuscaEmpresa" runat="server" Text="Empresa"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseEmpresa" runat="server" Key="empresa" Argument="razao_social"
                        SqlSelect="SELECT empresa, razao_social FROM ly_empresa" SqlOrder="empresa" MaxLength="20"
                        OnChanged="tseEmpresa_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código da Empresa" FieldName="empresa" Width="150px" />
                            <tweb:TSearchBoxColumn Caption="Razão Social" FieldName="razao_social" Width="400px" />
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
    <br />
    <div class="divEditBlock" style="width: 931px;">
        <asp:ImageButton ID="btnExcluir" runat="server" SkinID="BcDeletar" OnClick="btnExcluir_Click"
            OnClientClick="return confirm('Confirma a remoção?');" />
        <asp:ImageButton ID="btnAlterar" runat="server" SkinID="BcEditar" OnClick="btnAlterar_Click" />
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancelar" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
            ValidationGroup="SalvarForm" />
        <asp:Label runat="server" ID="lblBloco" Text="Empresas" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsEmpresa" runat="server" ShowMessageBox="true" ValidationGroup="SalvarForm"
            ShowSummary="false" />
    </div>
    <asp:Panel ID="pnlEmpresa" runat="server" GroupingText="Empresa" Width="960px">
        <table>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblEmpresa" runat="server" Text="Empresa:* " SkinID="lblObrigatorio"
                        Width="150px"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEmpresa" runat="server" MaxLength="20" Width="150px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEmpresa" runat="server" ControlToValidate="txtEmpresa"
                        InitialValue="" ErrorMessage="Empresa: Preenchimento obrigatório." ValidationGroup="SalvarForm">
                        <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblRazaoSocial" runat="server" Text="Razão Social:* " SkinID="lblObrigatorio"
                        Width="150px"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtRazaoSocial" runat="server" MaxLength="100" Width="300px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvRazaoSocial" runat="server" ControlToValidate="txtRazaoSocial"
                        InitialValue="" ErrorMessage="Razão Social: Preenchimento obrigatório." ValidationGroup="SalvarForm">
                        <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblNome" runat="server" Text="Nome: " Width="150px"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNome" runat="server" MaxLength="50" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblCEP" runat="server" Text="CEP: " Width="150px"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCEP" runat="server" MaxLength="8" SkinID="numerico"
                        Width="100px"></asp:TextBox>
                    <tweb:TSearch ID="tsCEP" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryCEP"
                        Modal="true" SkinID="CEP">
                        <GridColumns>
                            <tweb:TSearchColumn Caption="CEP" FieldName="cep" Width="15%" />
                            <tweb:TSearchColumn Caption="Logradouro" FieldName="nomeLogradouro" Width="30%" />
                            <tweb:TSearchColumn Caption="Município" FieldName="nomeMunicipio" Width="30%" />
                            <tweb:TSearchColumn Caption="Bairro" FieldName="nomeBairro" Width="15%" />
                            <tweb:TSearchColumn Caption="Estado" FieldName="uf" Width="10%" />
                            <tweb:TSearchColumn Caption="codigoLogradouro" FieldName="codigoLogradouro" Visible="false"
                                Width="0%" />
                            <tweb:TSearchColumn Caption="codigoMunicipio" FieldName="codigoMunicipio" Visible="false"
                                Width="0%" />
                            <tweb:TSearchColumn Caption="codigoBairro" FieldName="codigoBairro" Visible="false"
                                Width="0%" />
                        </GridColumns>
                        <GridFilterParameters>
                            <tweb:TSearchParameter Caption="Município" ParameterName="municipio" ShowInFilterPanel="true"
                                MaxLength="100"></tweb:TSearchParameter>
                            <tweb:TSearchParameter Caption="Logradouro" ParameterName="logradouro" ShowInFilterPanel="true"
                                MaxLength="20"></tweb:TSearchParameter>
                            <tweb:TSearchParameter Caption="CEP" ParameterName="cep" ShowInFilterPanel="true"
                                MaxLength="20"></tweb:TSearchParameter>
                        </GridFilterParameters>
                        <Messages KeyNotFound="Código não encontrado" TooManyRows="Foram encontrados mais de {0} registros. Mostrando os {0} primeiros."
                            QueryFailure="Ocorreu uma falha durante a busca dos registros. Execute a busca novamente.">
                        </Messages>
                    </tweb:TSearch>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblMunicipio" runat="server" Text="Município" Width="150px"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseMunicipio" runat="server" Caption="" MaxLength="20" SqlSelect="SELECT codigo, nome, uf_sigla FROM municipio"
                        SqlOrder="nome" ArgumentColumns="30" Columns="10"
                        GridWidth="600px" OnChanged="tseMunicipio_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                            <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
                <td style="text-align: right">
                    <asp:Label ID="lblEstado" runat="server" Text="UF: " Width="150px"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEstado" runat="server" MaxLength="20" ReadOnly="true" Width="50px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblEndereco" runat="server" Text="Endereço: " Width="150px"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEndereco" runat="server" MaxLength="50" Width="300px"></asp:TextBox>
                </td>
                <td style="text-align: right">
                    <asp:Label ID="lblNumero" runat="server" Text="Número: " Width="150px"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNumero" runat="server" MaxLength="15" Width="50px" SkinID="numerico">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblComplemento" runat="server" Text="Complemento: " Width="150px"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtComplemento" runat="server" MaxLength="50" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblBairro" runat="server" Text="Bairro: " Width="150px"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtBairro" runat="server" MaxLength="50" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblCNPJ" runat="server" Text="CNPJ:* " SkinID="lblObrigatorio" Width="150px"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCNPJ" runat="server" MaxLength="19" onkeyup="formataCNPJ(this,event)"
                        Width="150px">                                                                        
                    </asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvCNPJ" runat="server" ControlToValidate="txtCNPJ"
                        InitialValue="" ErrorMessage="CNPJ: Preenchimento obrigatório." ValidationGroup="SalvarForm">
                        <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" />                        
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblInscricaoMunicipal" runat="server" Text="Inscrição Municipal: "
                        Width="150px"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtInscricaoMunicipal" runat="server" MaxLength="50" Width="300px"
                        SkinID="numerico">                        
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblInscricaoEstadual" runat="server" Text="Inscrição Estadual: " Width="150px"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtInscricaoEstadual" runat="server" MaxLength="50" Width="300px"
                        SkinID="numerico">                        
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblPorte" runat="server" Text="Porte: " Width="150px"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPorte" runat="server" MaxLength="40" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblRamo" runat="server" Text="Ramo: " Width="150px"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtRamo" runat="server" MaxLength="40" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblAtividade" runat="server" Text="Atividade: " Width="150px"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtAtividade" runat="server" MaxLength="40" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblNumeroEmpregados" runat="server" Text="Número de Empregados: "
                        Width="150px"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNumeroEmpregados" runat="server" MaxLength="9" Width="50px" SkinID="numerico">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblTipoCapital" runat="server" Text="Tipo de Capital: " Width="150px"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtTipoCapital" runat="server" MaxLength="40" Width="300px"></asp:TextBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
