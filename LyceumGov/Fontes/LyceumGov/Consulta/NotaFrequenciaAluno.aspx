<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="NotaFrequenciaAluno.aspx.cs" Inherits="Techne.Lyceum.Net.Consulta.NotaFrequenciaAluno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <link rel="stylesheet" href="../Styles/Boletim.css" type="text/css" />
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por Aluno" Width="80%">
        <table>
            <tr>
                <td align="right">
                    <asp:Label runat="server" ID="Label1" SkinID="lblObrigatorio" Text="Aluno*:"></asp:Label>
                </td>
                <td colspan="3">
                    <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAluno"
                        AutoPostBack="true" OnTextChanged="tseAluno_Changed">
                    </tweb:TSearch>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label runat="server" SkinID="lblObrigatorio" ID="AnoBusca" Text="Ano*:"></asp:Label>
                </td>
                <td>
                    <asp:Label runat="server" ID="lblAnoBusca"></asp:Label>
                </td>
                <td align="right">
                    <asp:Label runat="server" SkinID="lblObrigatorio" ID="PeriodoBusca" Text="Período*:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbPeriodoBusca" runat="server" AutoPostBack="false" Enabled="true"
                        Width="127px">
                        <asp:ListItem Selected="True" Text="<Nenhum>" Value=""></asp:ListItem>
                        <asp:ListItem Text="Anual" Value="0"></asp:ListItem>
                        <asp:ListItem Text="1o. semestre" Value="1"></asp:ListItem>
                        <asp:ListItem Text="2o. semestre" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <dxe:ASPxButton ID="btnBuscar" runat="server" Image-Url="~/Images/bot_buscar.png"
                        Cursor="pointer" ValidationGroup="ConfirmarForm" OnClick="btnBuscar_Click" BackColor="White"
                        EnableDefaultAppearance="false">
                        <Image Url="~/Images/bot_buscar.png"></Image>
                        <BackgroundImage ImageUrl="~/Images/bot_buscar.png"></BackgroundImage>
                    </dxe:ASPxButton>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem" ClientInstanceName="lblMensagem"></asp:Label>
    <br />
    <asp:Panel ID="pnDadosAluno" runat="server" Visible="false" Width="80%">
        <div style="width: 100%;">
            <table style="width: 80%;">
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
                                    <strong>CONSULTA DE DADOS ESCOLARES DO ALUNO</strong>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
            <div>
                <strong>DADOS DO ALUNO</strong></div>
            <hr id="tDadosPessoais" style="margin: 1px" />
            <asp:Panel ID="pnAluno" runat="server" Width="100%">
                <table>
                    <tr>
                        <td align="right" style="width:100px;">
                            <asp:Label runat="server" ID="Reginal" Text="Regional:"></asp:Label>
                        </td>
                        <td colspan="3">
                            <strong>
                                <asp:Label runat="server" ID="lblRegional"></asp:Label></strong>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label runat="server" ID="Municipio" Text="Município:"></asp:Label>
                        </td>
                        <td colspan="3">
                            <strong>
                                <asp:Label runat="server" ID="lblMunicipio"></asp:Label></strong>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label runat="server" ID="Censo" Text="Censo:"></asp:Label>
                        </td>
                        <td style="width:150px;">
                            <strong>
                                <asp:Label runat="server" ID="lblCenso"></asp:Label></strong>
                        </td>
                        <td align="right">
                            <asp:Label runat="server" ID="Escola" Text="Unidade:"></asp:Label>
                        </td>
                        <td>
                            <strong>
                                <asp:Label runat="server" ID="lblEscola"></asp:Label></strong>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label runat="server" ID="Matricula" Text="Mastrícula:"></asp:Label>
                        </td>
                        <td>
                            <strong>
                                <asp:Label runat="server" ID="lblMatricula"></asp:Label></strong>
                        </td>
                        <td align="right">
                            <asp:Label runat="server" ID="Nome" Text="Nome do aluno:"></asp:Label>
                        </td>
                        <td colspan="3">
                            <strong>
                                <asp:Label runat="server" ID="lblNome"></asp:Label></strong>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label runat="server" ID="Mae" Text="Nome da Mãe:"></asp:Label>
                        </td>
                        <td colspan="3">
                            <strong>
                                <asp:Label runat="server" ID="lblMae"></asp:Label></strong>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label runat="server" ID="DataNascimento" Text="Data Nascimento:"></asp:Label>
                        </td>
                        <td>
                            <strong>
                                <asp:Label runat="server" ID="lblDataNascimento"></asp:Label></strong>
                        </td>
                        <td align="right">
                            <asp:Label runat="server" ID="Status" Text="Status Matrícula:"></asp:Label>
                        </td>
                        <td>
                            <strong>
                                <asp:Label runat="server" ID="lblStatus"></asp:Label></strong>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <div>
                <strong>DADOS DA ENTURMAÇÃO</strong></div>
            <hr id="Hr1" style="margin: 1px" />
            <asp:Panel ID="pnEntumacao" runat="server" Visible="false"
                Width="100%">
                <table>
                    <tr>
                        <td align="right" style="width:100px;">
                            <asp:Label runat="server" ID="Ano" Text="Ano:"></asp:Label>
                        </td>
                        <td style="width:150px;">
                            <strong>
                                <asp:Label runat="server" ID="lblAno"></asp:Label></strong>
                        </td>
                        <td align="right">
                            <asp:Label runat="server" ID="Periodo" Text="Período:"></asp:Label>
                        </td>
                        <td>
                            <strong>
                                <asp:Label runat="server" ID="lblPeriodo"></asp:Label></strong>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label runat="server" ID="Escolaridade" Text="Escolaridade:"></asp:Label>
                        </td>
                        <td>
                            <strong>
                                <asp:Label runat="server" ID="lblEscolaridade"></asp:Label></strong>
                        </td>
                        <td align="right">
                            <asp:Label runat="server" ID="AnoEscolaridade" Text="Série:"></asp:Label>
                        </td>
                        <td>
                            <strong>
                                <asp:Label runat="server" ID="lblAnoEscolaridade"></asp:Label></strong>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label runat="server" ID="Turno" Text="Turno:"></asp:Label>
                        </td>
                        <td>
                            <strong>
                                <asp:Label runat="server" ID="lblTurno"></asp:Label></strong>
                        </td>
                        <td align="right">
                            <asp:Label runat="server" ID="TurmaPrincipal" Text="Turma Principal:"></asp:Label>
                        </td>
                        <td>
                            <strong>
                                <asp:Label runat="server" ID="lblTurmaPrincipal"></asp:Label></strong>
                        </td>
                    </tr>
                </table>
                <div runat="server" id="divGrdBoletim">
                </div>
            </asp:Panel>
            <asp:Label ID="lblAviso" runat="server" SkinID="lblMensagem" ClientInstanceName="lblAviso"></asp:Label>
        </div>
    </asp:Panel>
</asp:Content>
