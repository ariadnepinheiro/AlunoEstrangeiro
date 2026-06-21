<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="DadosCadastraisAluno.aspx.cs" Inherits="Techne.Lyceum.Net.Consulta.DadosCadastraisAluno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por Aluno" Width="80%">
        <table>
            <tr>
                <td align="right">
                    <asp:Label runat="server" ID="Label10" SkinID="lblObrigatorio" Text="Aluno*:"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAluno"
                        AutoPostBack="true" OnTextChanged="tseAluno_Changed">
                    </tweb:TSearch>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem" ClientInstanceName="lblMensagem"></asp:Label>
    <br />
    <asp:Panel ID="pnDadosAluno" runat="server" Visible="false" Width="80%">
        <div>
            <table style="width: 100%;">
                <tr>
                    <td align="center">
                        <table>
                            <tr>
                                <td align="right">
                                    <asp:Image ID="Image1" runat="server" AlternateText="Governo do Rio de Janeiro - Sec de Estado de Educação"
                                        ImageUrl="~/Images/logo.gif" Style="text-align: right" />
                                </td>
                                <td align="center" style="font-size: 13px">
                                    GOVERNO DO ESTADO DO RIO DE JANEIRO - SECRETARIA DE EDUCAÇÃO<br />
                                    <strong>CONSULTA DE DADOS CADASTRAIS DO ALUNO</strong>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <div>
                <strong>DADOS PESSOAIS</strong></div>
            <hr id="tDadosPessoais" style="margin: 1px" />
            <table style="width: 100%; border-spacing: 0;" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td colspan="3">
                        <asp:Label runat="server" ID="Nome" Text="Nome:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblNome"></asp:Label></strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="DataNasc" Text="Data de Nascimento:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblDataNascimento"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Sexo" Text="Sexo:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblSexo"></asp:Label></strong>
                    </td>
                    <td rowspan="8">
                        <dxe:ASPxBinaryImage ID="bimgFotoPessoa" runat="server" AlternateText="sem foto"
                            ClientInstanceName="bimgFotoPessoa" Height="32mm" StoreContentBytesInViewState="True"
                            Width="32mm">
                            <EmptyImage AlternateText="sem foto" Url="~/Images/semfoto.jpg" />
                        </dxe:ASPxBinaryImage>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="QtdeFilhos" Text="Quantidade de Filhos:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblQtdeFilhos"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="TipoSanguineo" Text="Tipo Sanguíneo:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblTipoSanguineo"></asp:Label></strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="Etnia" Text="Etnia:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblEtnia"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="EstadoCivil" Text="Estado Civil:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblEstadoCivil"></asp:Label></strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="PaisNasc" Text="País de Nascimento:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblPaisNasc"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Nacionalidade" Text="Nacionalidade:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblNacionalidade"></asp:Label></strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="UFNasc" Text="UF Nascimento:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblUFNasc"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Naturalidade" Text="Naturalidade:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblNaturalidade"></asp:Label></strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="Credo" Text="Credo:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblCredo"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="NecEspecial" Text="Necessidade Especial:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblNecEspecial"></asp:Label></strong>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label runat="server" ID="Matricula" Text="Matrícula:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblMatricula"></asp:Label></strong>
                    </td>
                </tr>
            </table>
            <br />
            <div>
                <strong>FILIAÇÃO</strong></div>
            <hr id="Hr1" style="margin: 1px" />
            <table style="width: 100%; border-spacing: 0;" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        <asp:Label runat="server" ID="NomeMae" Text="Nome mãe:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblNomeMae"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="MaeFalecida" Text="Falecida:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblMaeFalecida"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="CPFMae" Text="CPF:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblCPFMae"></asp:Label></strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="NomePai" Text="Nome Pai:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblNomePai"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="PaiFalecido" Text="Falecido:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblPaiFalecido"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="CPFPai" Text="CPF:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblCPFPai"></asp:Label></strong>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label runat="server" ID="Label1" Text="Resp. Legal:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblRespLegal"></asp:Label></strong>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label runat="server" ID="Label2" Text="Nome Outros:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblNomeOutros"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label3" Text="CPF:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblCPFOutros"></asp:Label></strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="Label4" Text="Tel Mãe:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblTelMae"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label5" Text="Tel Pai:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblTelPai"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label6" Text="Tel Resp:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblTelResp"></asp:Label></strong>
                    </td>
                </tr>
            </table>
            <br />
            <div>
                <strong>ENDEREÇO</strong></div>
            <hr id="Hr2" style="margin: 1px" />
            <table style="width: 100%; border-spacing: 0;" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        <asp:Label runat="server" ID="Label7" Text="Endereço:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblEndereco"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label8" Text="Número:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblNumero"></asp:Label></strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="Label9" Text="Complemento:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblComplemento"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label11" Text="Bairro:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblBairro"></asp:Label></strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="Label40" Text="Município:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblMunicipio"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label12" Text="Estado:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblEstado"></asp:Label></strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="Label13" Text="CEP:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblCEP"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label14" Text="Localização/Zona de Residência:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblLocalizacao"></asp:Label></strong>
                    </td>
                </tr>
            </table>
            <br />
            <div>
                <strong>CONTATO</strong></div>
            <hr id="Hr3" style="margin: 1px" />
            <table style="width: 100%; border-spacing: 0;" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        <asp:Label runat="server" ID="Label15" Text="Telefone:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblContatoTelefone"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label16" Text="Celular:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblContatoCelular"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label17" Text="E-mail:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblContatoEmail"></asp:Label></strong>
                    </td>
                </tr>
            </table>
            <br />           
            <div>
                <strong>DOCUMENTO</strong></div>
            <hr id="Hr5" style="margin: 1px" />
            <table style="width: 100%; border-spacing: 0;" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td colspan="2">
                        <asp:Label runat="server" ID="Label20" Text="CPF:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblCPFDocumento"></asp:Label></strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="Label21" Text="Tipo:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblTipoDocumento"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label22" Text="Número:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblNumeroDocumento"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label23" Text="Complemento Identidade:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblComplIdent"></asp:Label></strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="Label24" Text="Estado:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblEstadoDocumento"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label25" Text="Órgão Emissor:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblOrgaoEmissor"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label26" Text="Data de Expedição:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblDtExpedicaoDocumento"></asp:Label></strong>
                    </td>
                </tr>
            </table>
            <br />
            <div>
                <strong>OUTRAS INFORMAÇÕES</strong></div>
            <hr id="Hr6" style="margin: 1px" />
            <table style="width: 100%; border-spacing: 0;" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        <asp:Label runat="server" ID="Label27" Text="Identificação no INEP:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblINEP"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label28" Text="Identificação Social(NIS):"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblNIS"></asp:Label></strong>
                    </td>
                </tr>
            </table>
            <br />
            <div>
                <strong>CERTIDÃO CIVIL</strong></div>
            <hr id="Hr7" style="margin: 1px" />
            <table style="width: 100%; border-spacing: 0;" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td colspan="3">
                        <asp:Label runat="server" ID="Label29" Text="Tipo Certidão:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblTipoCertidao"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label30" Text="Certidão Civil:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblCertidaoCivil"></asp:Label></strong>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label runat="server" ID="Label31" Text="UF Cartório:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblUFCartorio"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label32" Text="Município Cartório:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblMunicipioCartorio"></asp:Label></strong>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label runat="server" ID="Label33" Text="Nome do Cartório:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblNomeCartorio"></asp:Label></strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="Label34" Text="Livro:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblLivro"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label35" Text="Folha:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblFolha"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label36" Text="Termo:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblTermo"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label37" Text="Data da Emissão:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblDtEmissaoCertidao"></asp:Label></strong>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label runat="server" ID="Label38" Text="Matrícula da Certidão:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblMatriculaCertidao"></asp:Label></strong>
                    </td>
                </tr>
            </table>
            <br />
        </div>
    </asp:Panel>
</asp:Content>
