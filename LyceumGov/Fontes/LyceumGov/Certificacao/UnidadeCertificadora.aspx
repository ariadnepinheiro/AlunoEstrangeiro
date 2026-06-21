<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="UnidadeCertificadora.aspx.cs" Inherits="Techne.Lyceum.Net.Certificacao.UnidadeCertificadora" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

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

    <asp:Panel ID="pnlBuscarUnidade" runat="server" GroupingText="Selecione a unidade certificadora desejada"
        Width="650px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblBuscaUnidade" runat="server" Text="Unidade Certificadora:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidade" runat="server" Key="UNIDADECERTIFICADORAID" Argument="DESCRICAO" 
                        SqlSelect="SELECT TIPO FROM [CertificacaoEscolar].[UNIDADECERTIFICADORA] UC "
                        SqlOrder="DESCRICAO" MaxLength="20" OnChanged="tseUnidade_Changed" DataType="Number">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="UNIDADECERTIFICADORAID" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Unidade" FieldName="DESCRICAO" Width="50%" />   
                            <tweb:TSearchBoxColumn Caption="Tipo" FieldName="TIPO" Width="30%" />                         
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
    <div class="divEditBlock" style="width: 700px;">
        <asp:ImageButton ID="btnExcluir" runat="server" SkinID="BcDeletar" OnClick="btnExcluir_Click"
            OnClientClick="return confirm('Confirma a remoção?');" />
        <asp:ImageButton ID="btnAlterar" runat="server" SkinID="BcEditar" OnClick="btnAlterar_Click" />
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancelar" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
            ValidationGroup="SalvarForm" />
        <asp:Label runat="server" ID="lblBloco" Text="Unidade Certificadora" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsUnidade" runat="server" ShowMessageBox="true" ValidationGroup="SalvarForm"
            ShowSummary="false" />
    </div>
    <asp:Panel ID="pnlUnidade" runat="server" GroupingText="Dados da Unidade Certificadora"
        Width="700" Visible="false">
        <table>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblTipo" runat="server" SkinID="lblObrigatorio" Text="Tipo:* "></asp:Label>
                </td>
                <td>
                    <asp:RadioButtonList ID="rblTipo" runat="server" RepeatDirection="Horizontal" Width="150px">                        
                        <asp:ListItem Text="Polo" Value="Polo"></asp:ListItem>
                        <asp:ListItem Text="CEJA" Value="Ceja"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>            
                <td style="text-align: right">
                    <asp:Label runat="server" ID="lblGrupo" Text="Grupo:*" SkinID="lblObrigatorio" Width="80px"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlGrupo" runat="server" DataTextField="DESCRICAO" DataValueField="GRUPOUNIDADECERTIFICADORAID">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblUnidade" runat="server" Text="Unidade:* " SkinID="lblObrigatorio"
                        Width="80px"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtUnidade" runat="server" MaxLength="100" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblDescricaoSite" runat="server" Text="Descrição para site:* " SkinID="lblObrigatorio"
                        Width="80px"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtDescricaoSite" runat="server" MaxLength="100" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblCEP" runat="server" Text="CEP:* " Width="80px" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtCEP" runat="server" MaxLength="8" SkinID="numerico" Width="100px"></asp:TextBox>
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
                    <asp:Label ID="lblMunicipio" runat="server" Text="Município:* " Width="80px" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseMunicipio" runat="server" Caption="" MaxLength="20" SqlSelect="SELECT codigo, nome, uf_sigla FROM municipio"
                        SqlOrder="nome" ArgumentColumns="30" Columns="10" GridWidth="600px" OnChanged="tseMunicipio_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                            <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
                <td style="text-align: right">
                    <asp:Label ID="lblEstado" runat="server" Text="UF:* " Width="80px" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEstado" runat="server" MaxLength="20" ReadOnly="true" Width="50px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblEndereco" runat="server" Text="Endereço:* " Width="80px" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEndereco" runat="server" MaxLength="50" Width="300px"></asp:TextBox>
                </td>
                <td style="text-align: right">
                    <asp:Label ID="lblNumero" runat="server" Text="Número:* " Width="150px" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNumero" runat="server" MaxLength="15" Width="50px" SkinID="numerico">
                    </asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblComplemento" runat="server" Text="Complemento: " Width="80px"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtComplemento" runat="server" MaxLength="100" Width="300px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblBairro" runat="server" Text="Bairro:* " SkinID="lblObrigatorio"
                        Width="80px"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtBairro" runat="server" MaxLength="50" Width="300px"></asp:TextBox>
                </td>           
                <td style="text-align: right">
                    <asp:Label ID="lblFone" runat="server" Text="Telefone:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtFone" onkeyup="formataFixoCelularDDD(this,event)" runat="server"
                        MaxLength="14" Width="100px" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:CheckBox ID="chkAtivo" Text="Ativo?" runat="server" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
