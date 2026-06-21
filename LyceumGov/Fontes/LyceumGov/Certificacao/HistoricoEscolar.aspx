<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="HistoricoEscolar.aspx.cs" Inherits="Techne.Lyceum.Net.Certificacao.HistoricoEscolar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <div>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblLegenda" runat="server" Text="os campos com (*) são de preenchimeto obrigatório"
                        ForeColor="Black" Font-Italic="True"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <!--Busca de Alunos-->
        <asp:Panel ID="pnBuscaAluno" runat="server" GroupingText="Faça uma busca por matrícula ou nome do aluno"
            Width="80%">
            <table>
                <tr>
                    <td align="right">
                        <asp:Label runat="server" ID="lblAluno" SkinID="lblAluno" Text="Aluno*:"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAluno"
                            OnChanged="tseAluno_Changed" AutoPostBack="true">
                        </tweb:TSearch>
                    </td>
                    <td>
                        <br />
                    </td>
                    <td align="right">
                        <asp:Label runat="server" ID="lblTipoConclusao" SkinID="lblTipoConclusao" Text="Nível*:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTipoConclusao" runat="server" DataTextField="DESCRICAO"
                            DataValueField="TIPOCONCLUSAOID" OnSelectedIndexChanged="ddlTipoConclusao_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btBuscar" runat="server" Text="Buscar" OnClick="btBuscar_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <!-- Mensagem -->
        <br />
        <br />
        <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem" ClientInstancename="lblMensagem"></asp:Label>
        <br />
        <br />
        <asp:Panel ID="pnDadosAluno" runat="server" Visible="false" Width="80%">
            <!-- Cabeçalho com logo -->
            <div>
                <table>
                    <tr>
                        <td align="right">
                            <asp:Image ID="Image1" runat="server" AlternateText="Governo do Rio de Janeiro - Sec de Estado de Educação"
                                ImageUrl="~/Images/logo.gif" Style="text-align: right" />
                        </td>
                        <td align="center" style="width: 693px;">
                            GOVERNO DO ESTADO DO RIO DE JANEIRO
                            <br />
                            SECRETARIA DE EDUCAÇÃO
                            <br />
                            FICHA DOS DADOS DO ALUNO
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <!--Dados Pessoais-->
            <div>
                <strong>DADOS PESSOAIS</strong></div>
            <hr id="tDadosPessoais" style="margin: 1px" />
            <table style="width: 100%; border-spacing: 0;" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td colspan="3">
                        <br />
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
                <tr>
                    <td>
                        <asp:Label runat="server" ID="DataSit" Text="Data da Situação:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblDataSit"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="HoraSit" Text="Hora:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblHoraSit"></asp:Label></strong>
                    </td>
                </tr>
            </table>
            <br />
            <!-- Filiação -->
            <div>
                <strong>FILIAÇÃO</strong></div>
            <hr id="Hr1" style="margin: 1px" />
            <table style="width: 100%; border-spacing: 0;" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        <br />
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
                        <asp:Label runat="server" ID="Label3" Text="Nome Outros:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblNomeOutros"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label5" Text="CPF:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblCPFOutros"></asp:Label></strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="Label2" Text="Tel Mãe:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblTelMae"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label6" Text="Tel Pai:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblTelPai"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label8" Text="Tel Resp:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblTelResp"></asp:Label></strong>
                    </td>
                </tr>
            </table>
            <br />
            <!-- Endereço -->
            <div>
                <strong>ENDEREÇO</strong></div>
            <hr id="Hr2" style="margin: 1px" />
            <table style="width: 100%; border-spacing: 0;" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        <br />
                        <asp:Label runat="server" ID="Label4" Text="Endereço:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblEndereco"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label7" Text="Número:"></asp:Label>
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
                        <asp:Label runat="server" ID="Label13" Text="Município:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblMunicipio"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label15" Text="Estado:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblEstado"></asp:Label></strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="Label17" Text="CEP:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblCEP"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label19" Text="Localização/Zona de Residência:"></asp:Label>
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
                        <br />
                        <asp:Label runat="server" ID="Label10" Text="Telefone:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblContatoTelefone"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label14" Text="Celular:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblContatoCelular"></asp:Label></strong>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="Label18" Text="E-mail:"></asp:Label>
                        <strong>
                            <asp:Label runat="server" ID="lblContatoEmail"></asp:Label></strong>
                    </td>
                </tr>
            </table>
            <br />
            <!-- Observações -->
            <br />
            <div>
                <strong>OBSERVAÇÕES DO HISTÓRICO ESCOLAR</strong></div>
            <hr id="Hr10" style="margin: 1px" />
            <table style="width: 100%; border-spacing: 0;" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        <br />
                        <asp:TextBox ID="txtObservacao" runat="server" Columns="30" Rows="10" MaxLength="2"
                            TextMode="MultiLine" Height="79px" Width="750px" Enabled="true"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <br />
        </asp:Panel>
        <br />
        <asp:Button ID="btnGerarHistorico" runat="server" Text="Gerar Histórico Escolar"
            Font-Size="Small" Visible="false" OnClick="btnGerarHistorico_Click" />
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" Font-Size="Small" Visible="false"
            OnClick="btCancelar_Click" />
    </div>
</asp:Content>
