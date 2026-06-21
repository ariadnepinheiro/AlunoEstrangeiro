<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="LoginRedeEmail.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.LoginRedeEmail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe o funcionário"
        Height="45px" Width="784px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblDocenteTSearch" runat="server" Text="Funcionário*: " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseUsuario" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryUsuarioRedeEmail"
                        AutoPostBack="true" OnTextChanged="tseUsuario_Changed" OnLoad="tseUsuario_Load">
                    </tweb:TSearch>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <asp:HiddenField ID="hdnPessoa" runat="server" />
    <br />
    <br />
    <div class="divEditBlock" style="width: 968px;">
        <asp:Label runat="server" ID="lblBloco" Text="Usuários" SkinID="BcTitulo" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
            ValidationGroup="SalvarForm" />
        <asp:ValidationSummary ID="vsUsuarios" runat="server" ShowMessageBox="true" ValidationGroup="SalvarForm"
            ShowSummary="false" />
    </div>
    <br />
    <br />
    <asp:Panel ID="pnlGeral" runat="server" Visible="false">
        <asp:Panel ID="pnlDadosPessoais" GroupingText="Dados Pessoais" runat="server" Enabled="false">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblNomeComplPessoa" runat="server" Text="Nome:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtNomeComplPessoa" runat="server" MaxLength="100" Columns="80"
                            Enabled="false" ReadOnly="true">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblCPF" runat="server" Text="CPF:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCPF" runat="server" onkeyup="formataCPF(this,event)" MaxLength="14"
                            Enabled="false" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label1" runat="server" Text="ID Funcional:* " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtIDFncional" runat="server" onkeyup="formataCPF(this,event)" MaxLength="14"
                            Enabled="false" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlDadosAcesso" GroupingText="Dados de Acesso" runat="server">
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label2" runat="server" Text="E-mail Office 365:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmailOffice" runat="server" MaxLength="100" Columns="50">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label3" runat="server" Text="E-mail Google For Education:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmailGoogle" runat="server" MaxLength="100" Columns="50"></asp:TextBox>
                    </td>
                </tr>
                 <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label4" runat="server" Text="E-mail Alternativo:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmailAlternativo" runat="server" MaxLength="100" Columns="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Label ID="Label5" runat="server" Text="Login de Rede:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtLoginRede" runat="server" MaxLength="50" Columns="50" onkeypress="return nomeSemNum(event);"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlGrid" runat="server">
            <asp:ObjectDataSource ID="odsLotacao" runat="server" TypeName="Techne.Lyceum.Net.Basico.LoginRedeEmail"
                SelectMethod="ListaLotacao">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hdnPessoa" Name="pessoa" PropertyName="Value" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <dxwgv:ASPxGridView ID="grdLotacao" DataSourceID="odsLotacao" runat="server" AutoGenerateColumns="False"
                ClientInstanceName="grdLotacao" EnableCallBacks="false" Font-Size="Small">
                <SettingsBehavior AllowMultiSelection="False" AllowSort="False" />
                <SettingsPager PageSize="10" />
                <Columns>
                    <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="MATRICULA" VisibleIndex="1">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="ID/Vínculo" FieldName="IDVINCULO" VisibleIndex="2">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Função" FieldName="FUNCAO" VisibleIndex="3">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Regional" FieldName="REGIONAL" VisibleIndex="4">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Município" FieldName="MUNICIPIO" VisibleIndex="5">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="UA" FieldName="UA" VisibleIndex="6">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Censo" FieldName="CENSO" VisibleIndex="7">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Nome Da Unidade / UA" FieldName="ESCOLA" VisibleIndex="8">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Endereço da Unidade" FieldName="ENDERECOESCOLA"
                        VisibleIndex="9">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="Telefone Unidade" FieldName="FONE" VisibleIndex="10">
                        <PropertiesTextEdit MaxLength="10">
                            <MaskSettings IncludeLiterals="None" Mask="(00)0000-0000" ErrorText="* Favor inserir um número de telefone válido." />
                        </PropertiesTextEdit>
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataTextColumn Caption="E-mail" FieldName="E_MAIL" VisibleIndex="11">
                    </dxwgv:GridViewDataTextColumn>
                </Columns>
                <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
            </dxwgv:ASPxGridView>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
