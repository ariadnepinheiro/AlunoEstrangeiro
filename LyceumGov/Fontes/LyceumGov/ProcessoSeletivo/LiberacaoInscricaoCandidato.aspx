<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="LiberacaoInscricaoCandidato.aspx.cs" Inherits="Techne.Lyceum.Net.ProcessoSeletivo.LiberacaoInscricaoCandidato" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por:" Width="650px">
        <table>
            <tr>
                <td align="right">
                    <asp:Label ID="lblConcursoBusca" runat="server" Text="Concurso:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseConcurso" runat="server" SqlSelect="SELECT CONCURSO,DESCRICAO from LY_CONCURSO_DOCENTE WITH (NOLOCK) "
                        GridWidth="650px" SqlWhere="tipo = 'Contrato' and GETDATE() BETWEEN DT_INI_INSCR and DT_FIM_INSCR">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Concurso" FieldName="CONCURSO" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="DESCRICAO" Width="70%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Label ID="lblCandidato" runat="server" Text="Candidato* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseCandidato" runat="server" Key="candidato" Argument="nome"
                        MaxLength="8" SqlSelect="SELECT CANDIDATO, NOME,CD.CONCURSO as 'PROCESSO_SELETIVO',CPF,FINALIZADO,DHR_CADASTRO from LY_CANDIDATO_DOCENTE CD WITH (NOLOCK) INNER JOIN LY_CONCURSO_DOCENTE C ON C.CONCURSO = CD.CONCURSO"
                        GridWidth="850px" SqlWhere=" CD.CONCURSO = #tseConcurso# AND GETDATE() BETWEEN DT_INI_INSCR and DT_FIM_INSCR " OnChanged="tseCandidato_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Inscrição" FieldName="candidato" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Processo Seletivo" FieldName="processo_seletivo"
                                Width="15%" />
                            <tweb:TSearchBoxColumn Caption="CPF" FieldName="cpf" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="FINALIZADO" FieldName="finalizado" Width="15%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <asp:Panel ID="pnCampos" runat="server" GroupingText="Dados do Candidato:" Width="650px"
        Visible="false">
        <table>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblConcurso" runat="server" Text="Concurso:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtConcurso" runat="server" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblInscricao" runat="server" Text="Inscrição"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtInscrição" runat="server" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblCPF" runat="server" Text="CPF:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCPF" runat="server" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblNome" runat="server" Text="Nome Candidato:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNome" runat="server" ReadOnly="true" Width="500px" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="ldlData" runat="server" Text="Data da Inscrição:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDataInscrição" runat="server" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <dxe:ASPxButton ID="btnLiberar" runat="server" Text="Excluir Inscrição" OnClick="btnLiberar_Click">
                    </dxe:ASPxButton>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
