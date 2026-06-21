<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="True"
    CodeBehind="DadosAluno.aspx.cs" Inherits="Techne.Lyceum.Net.Certificacao.DadosAluno" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe a matrícula ou o nome do aluno"
        Width="580px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblAlunoTSearch" runat="server" Text="Aluno:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseAluno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAlunoCertificacao"
                        AutoPostBack="true" OnTextChanged="tseAluno_Changed">
                    </tweb:TSearch>
                </td>
            </tr>
        </table>
        <br />
        <table>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Apenas podem ser editados alunos que não estejam ativos, para alunos ativos utilizar a tela 'Aluno'."
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem" ClientInstancename="lblMensagem"></asp:Label>
    <br />
    <br />
    <div class="divEditBlock" style="width: 850px;">
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
            ValidationGroup="SalvarForm" />
        <asp:Label runat="server" ID="lblBlocoAluno" Text="Alunos" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsAlunos" runat="server" EnableClientScript="true" ShowMessageBox="true"
            ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <asp:Panel ID="pnlDados" GroupingText="Dados do Aluno" runat="server" Width="50%">
        <table>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="lblNomeAluno" runat="server" Text="Nome do Aluno:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtNomeAluno" runat="server" Width="400px" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="lblNomemae" runat="server" Text="Nome da Mãe:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtNomemae" runat="server" MaxLength="200" Width="400px" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="lblNomepai" runat="server" Text="Nome do Pai:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtNomepai" runat="server" MaxLength="200" Width="400px" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lbldtnascimento" runat="server" Text="Data Nascimento:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dtDataNasc" runat="server" Width="100px" Enabled="true" EnableDefaultAppearance="true"
                        ClientInstanceName="dtNasc" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                </td>
                <td style="text-align: right;">
                    <asp:Label ID="lblNascidoEstrangeiro" runat="server" Text="Nascido fora do Brasil?"
                        kinID="lblObrigatorio"></asp:Label>
                </td>
                <td style="text-align: right">
                    <asp:RadioButtonList ID="rblNascidoEstrangeiro" runat="server" RepeatDirection="Horizontal"
                        AutoPostBack="true" OnSelectedIndexChanged="rblNascidoEstrangeiro_SelectedIndexChanged">
                        <asp:ListItem Text="Não" Value="N" Selected="True" />
                        <asp:ListItem Text="Sim" Value="S" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label runat="server" ID="lblNacionalidade" Text="Nacionalidade:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbNacionalidade" runat="server" DataTextField="nome" DataValueField="nacionalidade"
                        AutoPostBack="true" OnSelectedIndexChanged="cmbNacionalidade_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td style="text-align: right">
                    <asp:Label ID="lblMunicipioNascimento" runat="server" Text="Município Nascimento:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <%-- tse Brasil --%>
                    <tweb:TSearchBox ID="tseNaturalidade" runat="server" Visible="false" SqlOrder="nome"
                        SqlSelect="SELECT codigo, nome, uf_sigla FROM municipio" Columns="10" ArgumentColumns="30"
                        AutoPostBack="true" Key="codigo" MaxLength="10" OnChanged="tseNaturalidade_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                            <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                    <%-- tse estrangeira --%>
                    <tweb:TSearchBox ID="tseNaturalidadeEstrangeira" runat="server" Visible="false" Columns="10"
                        SqlSelect="SELECT ESTADO, SIGLA, ID_PAIS, PAIS FROM HADES.dbo.VW_MUNICIPIO_ESTRANGEIRO"
                        Argument="MUNICIPIO" ArgumentColumns="30" AutoPostBack="true" Key="CODIGO" MaxLength="10"
                        OnChanged="tseNaturalidadeEstrangeira_Changed" DataType="Number">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="CODIGO" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Cidade" FieldName="MUNICIPIO" Width="40%" />
                            <tweb:TSearchBoxColumn Caption="Estado/Província" FieldName="ESTADO" Width="40%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
                <%-- Estado de nascimento (preenchido automaticamente pela tseNaturalidadeEstrangeira) --%>
                <td style="text-align: right">
                    <asp:Label ID="lblUFNascimento" runat="server" Text="UF Nascimento:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtUFNascimento" runat="server" Width="40px" ReadOnly="true"></asp:TextBox>
                </td>
                <%-- País de nascimento (preenchido automaticamente pela tseNaturalidadeEstrangeira) --%>
                <td style="text-align: right">
                    <asp:Label ID="lblPaisNascimento" runat="server" Text="País:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtPaisNascimento" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>            
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblRg" runat="server" Text="Nº do RG:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNRg" runat="server" Width="98px"></asp:TextBox>
                </td>
                <td style="text-align: right">
                    <asp:Label ID="lblOrgaoemissor" runat="server" Text="Orgão Emissor:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbRGEmissor" runat="server" DataTextField="item" DataValueField="item"
                        Width="160px">
                    </asp:DropDownList>
                </td>
                <td style="text-align: right">
                    <asp:Label ID="lblufexpedicao" runat="server" Text="UF EXPEDIÇÃO:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbRGUF" runat="server" DataTextField="sigla" DataValueField="sigla"
                        Width="50px">
                    </asp:DropDownList>
                </td>
                <td style="text-align: right">
                    <asp:Label ID="Label2" runat="server" Text="Data Expedição:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dtExpedicaoRg" runat="server" Width="120px" Enabled="true"
                        EnableDefaultAppearance="true" ClientInstanceName="dtExpedicaoRg" CalendarProperties-ClearButtonText="Limpar"
                        CalendarProperties-TodayButtonText="Hoje">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
