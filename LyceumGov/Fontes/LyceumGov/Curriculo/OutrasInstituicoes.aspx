<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="OutrasInstituicoes.aspx.cs" Inherits="Techne.Lyceum.Net.Curriculo.OutrasInstituicoes" %>

<asp:Content ID="conOutrasinstituicoes" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        <!--
        $(document).ready(function() {
            trataCep({ tscep: '<%=tsCEP.ClientID %>',
                cep: '<%=txtCEP.ClientID %>',
                nomeLogradouro: '<%=txtEndereco.ClientID %>',
                nomeBairro: '<%=txtBairro.ClientID %>',
                codigoMunicipio: '<%=tseMunicipio.ClientID %>',
                uf: '<%=txtEstado.ClientID %>'
            });
        });
        -->
        
    </script>

    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por instituição"
        Width="640px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblInstituicao" runat="server" Text="Instituição:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseInstituicao" runat="server" Key="outra_faculdade" Argument="nome_comp"
                        OnChanged="tseInstituicao_Changed" MaxLength="20" SqlSelect="SELECT outra_faculdade, nome_comp from ly_instituicao"
                        GridWidth="850px" SqlOrder="nome_comp">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="outra_faculdade" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome_comp" Width="70%" />
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
    <div class="divEditBlock" style="width: 895px;">
        <asp:ImageButton ID="btnExcluir" runat="server" SkinID="BcDeletar" OnClick="btnExcluir_Click"
            OnClientClick="return confirm('Confirma a remoção?');" />
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
            ValidationGroup="SalvarForm" />
        <asp:Label runat="server" ID="lblBloco" Text="Outras Instituições" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsDocente" runat="server" ShowMessageBox="true" ValidationGroup="SalvarForm"
            ShowSummary="false" />
    </div>
    <dxtc:ASPxPageControl ID="pcInstituicao" runat="server" ActiveTabIndex="0" Height="348px"
        Width="895px">
        <TabPages>
            <dxtc:TabPage Text="Informações Gerais">
                <ContentCollection>
                    <dxw:ContentControl ID="ccOutrasInstituicoes" runat="server">
                        <asp:TextBox ID="txtOutraFaculdade" runat="server" Visible="false"></asp:TextBox>
                        <table>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblNome_Comp" runat="server" Text="Nome Completo:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtNome_Comp" runat="server" MaxLength="100" Width="600px" ReadOnly="true"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvNomeComp" runat="server" ControlToValidate="txtNome_Comp"
                                        InitialValue="" ErrorMessage="Nome Completo: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblCEP" runat="server" Text="CEP:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCEP" runat="server" MaxLength="8" SkinID="numerico" />
                                    <tweb:TSearch ID="tsCEP" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryCEP"
                                        Modal="true" SkinID="CEP" AutoPostBack="false">
                                        <Messages KeyNotFound="C&#243;digo n&#227;o encontrado" TooManyRows="Foram encontrados mais de {0} registros. Mostrando os {0} primeiros."
                                            QueryFailure="Ocorreu uma falha durante a busca dos registros. Execute a busca novamente.">
                                        </Messages>
                                    </tweb:TSearch>
                                    <asp:RequiredFieldValidator ID="rfvCEP" runat="server" ControlToValidate="txtCEP"
                                        InitialValue="" ErrorMessage="CEP: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblMunicipio" runat="server" Text="Município:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <tweb:TSearchBox ID="tseMunicipio" runat="server" Caption="" SqlOrder="nome" SqlSelect="SELECT codigo, nome, uf_sigla FROM municipio"
                                        OnChanged="tseMunicipio_Changed" GridWidth="600px" ArgumentColumns="30" Columns="10"
                                        MaxLength="20">
                                        <GridColumns>
                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                                            <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                                        </GridColumns>
                                    </tweb:TSearchBox>
                                    <asp:RequiredFieldValidator ID="rfvMunicipio" runat="server" ControlToValidate="tseMunicipio"
                                        InitialValue="" ErrorMessage="Município: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblUF" runat="server" Text="UF: "></asp:Label>
                                </td>
                                <td>
                                    <input id="txtEstado" runat="server" maxlength="20" class="txtInput" readonly="readonly" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblEndereco" runat="server" Text="Endereço:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEndereco" runat="server" MaxLength="50" Width="380px" />
                                    <asp:RequiredFieldValidator ID="rfvEndereco" runat="server" ControlToValidate="txtEndereco"
                                        InitialValue="" ErrorMessage="Endereço: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                </td>
                                <td style="text-align: right">
                                    <asp:Label ID="lblEnd_Num" runat="server" Text="Número:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEnd_Num" runat="server" MaxLength="15" SkinID="numerico" />
                                    <asp:RequiredFieldValidator ID="rfvNum" runat="server" ControlToValidate="txtEnd_Num"
                                        InitialValue="" ErrorMessage="Número: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblEnd_Compl" runat="server" Text="Complemento:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEnd_Compl" runat="server" MaxLength="50" Width="380px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right">
                                    <asp:Label ID="lblBairro" runat="server" Text="Bairro:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBairro" runat="server" Width="380px" MaxLength="50" />
                                    <asp:RequiredFieldValidator ID="rfvBairro" runat="server" ControlToValidate="txtBairro"
                                        InitialValue="" ErrorMessage="Bairro: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; vertical-align: middle; width: 15%">
                                    <asp:Label Font-Names="Verdana" ID="Label3" runat="server" Text="Tipo de Instituição:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td >
                                    <asp:DropDownList ID="ddlTipoInstituicao" runat="server" DataValueField="DESCR" DataTextField="DESCR"  AppendDataBoundItems="true"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvTipoInstituicao" runat="server" ControlToValidate="ddlTipoInstituicao" 
                                        ErrorMessage="Tipo Instituição: Preenchimento obrigatório." InitialValue="Selecione" ValidationGroup="SalvarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>

                                </td>
                            </tr>
                        </table>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
</asp:Content>
