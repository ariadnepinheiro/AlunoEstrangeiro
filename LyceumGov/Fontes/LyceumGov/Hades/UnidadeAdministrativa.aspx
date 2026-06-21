<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modulos/LyceumMaster.Master"
    CodeBehind="UnidadeAdministrativa.aspx.cs" Inherits="Techne.Lyceum.Net.Hades.Setores" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<asp:Content ID="conSetores" ContentPlaceHolderID="cphFormulario" runat="server">
    <style>
        .cursorImagem
        {
            cursor: pointer;
        }
        .txtInput
        {
            background-color: White;
            font-family: Verdana;
            font-size: smaller;
        }
    </style>

    <script type="text/javascript">
    <!--
        $().ready(function() {
            trataCep({ tscep: '<%=tsCEP.ClientID %>',
                cep: '<%=txtCep.ClientID %>',
                nomeLogradouro: '<%=txtEndereco.ClientID %>',
                nomeBairro: '<%=txtBairro.ClientID %>',
                codigoMunicipio: '<%=tseMunicipio.ClientID %>',
                uf: '<%=txtEstado.ClientID %>',
                codigos: '<%=hCEP.ClientID %>'
            });
        });
    -->
    </script>

    <input type="hidden" id="hCEP" runat="server" />
    <techne:TTableDataSource ID="tdsSetores" runat="server" DataTableClassName="Techne.HadesLyc.CR.Hd_setor">
    </techne:TTableDataSource>
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por unidade administrativa"
        Width="650px">
        <table>
            <tr>
                <td align="left">
                    <asp:Label ID="lblUnidadeAdministrativa" runat="server" Text="Unidade Administrativa:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadeAdministrativa" runat="server" SqlSelect="SELECT ua_atual, nomesetor, setor, ua_antiga FROM hades..vw_setor"
                        SqlOrder="ua_atual" ColumnName="ua_atual" Caption="" Connection="Hades" MaxLength="15"
                        DataType="Varchar" OnChanged="tseUnidadeAdministrativa_Changed">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="setor" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nomesetor" Width="70%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="10%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblRetorno" runat="server" Text="" SkinID="lblMensagem"></asp:Label>
    <asp:TextBox ID="txtSetorHid" runat="server" Visible="false"></asp:TextBox>
    <br />
    <br />
    <div class="divEditBlock" style="width: 850px;">
        <asp:ImageButton ID="btnExcluir" runat="server" SkinID="BcDeletar" OnClick="btnExcluir_Click" />
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
            ValidationGroup="SalvarForm" />
        <asp:ImageButton ID="btnConfirmar" runat="server" SkinID="BcConfirmar" OnClick="btnConfirmar_Click" />
        <asp:Label runat="server" ID="lblBloco" Text="Unidades Administrativas" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsSetor" runat="server" ShowMessageBox="true" ValidationGroup="SalvarForm"
            ShowSummary="false" />
    </div>
    <dxtc:ASPxPageControl ID="pcSetor" runat="server" ClientInstanceName="pcSetor" ActiveTabIndex="0"
        Width="850px">
        <TabPages>
            <dxtc:TabPage Text="Geral">
                <ContentCollection>
                    <dxw:ContentControl ID="conGeral" runat="server">
                        <asp:Panel ID="pnSetores" GroupingText="Dados da Unidade Administrativa" runat="server">
                            <table>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label2" runat="server" Text="Código:" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSetor" runat="server" Text="" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label1" runat="server" Text="Unidade Administrativa:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNovaUA" runat="server" MaxLength="15" SkinID="numerico" Width="150px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ErrorMessage="U.A.: Preenchimento obrigatório." ID="RequiredFieldValidator1"
                                            runat="server" ControlToValidate="txtNovaUA" InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblUnidAdministrativa" runat="server" Text="U.A. Antiga: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAntigaUA" runat="server" MaxLength="6" SkinID="numerico" Width="150px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblNome" runat="server" Text="Nome:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtNome" runat="server" MaxLength="100" Width="500px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ErrorMessage="Nome: Preenchimento obrigatório." ID="rfvNome"
                                            runat="server" ControlToValidate="txtNome" InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblTipo" runat="server" Text="Tipo:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTipo" runat="server" DataTextField="descricao" DataValueField="tiposetorid">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ErrorMessage="Tipo: Preenchimento obrigatório." ID="rfvTipo"
                                            runat="server" ControlToValidate="ddlTipo" InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblCNPJ" runat="server" Text="CNPJ:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCNPJ" runat="server" onkeyup="formataCNPJ(this,event)" Width="150px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDtIni" runat="server" Text="Data Início:"></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtIni" runat="server" Width="150px">
                                        </dxe:ASPxDateEdit>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblDtFim" runat="server" Text="Data Fim:"></asp:Label>
                                    </td>
                                    <td>
                                        <dxe:ASPxDateEdit ID="dtFim" runat="server" Width="110px">
                                        </dxe:ASPxDateEdit>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblCep" runat="server" Text="CEP:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCep" SkinID="numerico" runat="server" MaxLength="8" Width="110px"></asp:TextBox>
                                        <tweb:TSearch ID="tsCEP" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryCEP"
                                            Modal="true" SkinID="CEP" />
                                        <asp:RequiredFieldValidator ErrorMessage="CEP: Preenchimento obrigatório." ID="rfvCEP"
                                            runat="server" ControlToValidate="txtCEP" InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revCEP" ControlToValidate="txtCEP" ValidationExpression="^.{8}$"
                                            runat="server" ErrorMessage="CEP: Preenchimento de oito números obrigatório."
                                            ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RegularExpressionValidator>
                                    </td>
                                    <td align="right">
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
                                        <asp:RequiredFieldValidator ErrorMessage="Município: Preenchimento obrigatório."
                                            ID="rfvMunicipio" runat="server" ControlToValidate="tseMunicipio" InitialValue=""
                                            ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblEstado" runat="server" Text="Estado:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtEstado" runat="server" maxlength="20" class="txtInput" readonly="readonly" />
                                        <asp:RequiredFieldValidator ErrorMessage="Estado: Preenchimento obrigatório." ID="rfvEstador"
                                            runat="server" ControlToValidate="txtEstado" InitialValue="" ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblEndereco" runat="server" Text="Endereço:* " SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <input id="txtEndereco" type="text" runat="server" class="txtInput" maxlength="50"
                                            style="width: 260px" />
                                        <asp:RequiredFieldValidator ID="rfvEndereco" runat="server" ControlToValidate="txtEndereco"
                                            InitialValue="" ErrorMessage="Endereço: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblNumero" runat="server" Text="Nº.:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNumero" runat="server" MaxLength="6" SkinID="numerico" Width="110px"></asp:TextBox>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblCompl" runat="server" Text="Compl.:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCompl" runat="server" MaxLength="20" Width="110px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblBairro" runat="server" Text="Bairro:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtBairro" runat="server" MaxLength="50" onkeypress="return alphanumeric_only(event);"
                                            Width="110px" />
                                        <asp:RequiredFieldValidator ID="rfvBairro" runat="server" ControlToValidate="txtBairro"
                                            InitialValue="" ErrorMessage="Bairro: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!"/></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblTelefone" runat="server" Text="Telefone:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTelefone" onkeyup="formataTelefoneDDD(this,event)" runat="server"
                                            MaxLength="15" Width="110px"></asp:TextBox>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblFax" runat="server" Text="Fax:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFax" runat="server" MaxLength="15" onkeyup="formataTelefoneDDD(this,event)"
                                            Width="110px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblAtivo" runat="server" Text="Ativo: "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rblAtivo" runat="server" RepeatDirection="Horizontal" DataValueField="ativo">
                                            <asp:ListItem Text="Sim" Value="S"></asp:ListItem>
                                            <asp:ListItem Text="Não" Value="N"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="lblObs" runat="server" Text="Observação:"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtObs" runat="server" Width="500px" MaxLength="2000" TextMode="MultiLine" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Contatos">
                <ContentCollection>
                    <dxw:ContentControl ID="ccContatos" runat="server">
                        <asp:ObjectDataSource ID="odsContatos" runat="server" TypeName="Techne.Lyceum.RN.Setores"
                            SelectMethod="ConsultarContato">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="lblSetor" PropertyName="Text" Name="setor" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <dxwgv:ASPxGridView ID="grdContato" Width="80%" runat="server" AutoGenerateColumns="False"
                            KeyFieldName="Pessoa" OnCustomColumnDisplayText="grdContato_CustomColumnDisplayText">
                            <SettingsEditing Mode="EditForm" />
                            <SettingsBehavior AllowMultiSelection="False" AllowFocusedRow="False" />
                            <SettingsText EmptyDataRow="Não existem dados." />
                            <Columns>
                                <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NOME_COMPL" VisibleIndex="0">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="MATRICULA" VisibleIndex="1">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Função" FieldName="descricao" VisibleIndex="2">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Telefone" FieldName="FONE" VisibleIndex="3">
                                    <PropertiesTextEdit MaxLength="10">
                                        <MaskSettings IncludeLiterals="None" Mask="(00)0000-0000" ErrorText="* Favor inserir um número de telefone válido." />
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Celular" FieldName="CELULAR" VisibleIndex="4">
                                    <PropertiesTextEdit MaxLength="10">
                                        <MaskSettings IncludeLiterals="None" Mask="(00)0000-0000" ErrorText="* Favor inserir um número de telefone válido." />
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="E-mail" FieldName="E_MAIL" VisibleIndex="5">
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Histórico">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl1" runat="server">
                        <asp:ObjectDataSource ID="odsUA" runat="server" TypeName="Techne.Lyceum.Net.Hades.Setores"
                            SelectMethod="Lista">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="lblSetor" Name="setor" PropertyName="Text" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <dxwgv:ASPxGridView ID="grdHistoricoUA" runat="server" DataSourceID="odsUA" KeyFieldName="HISTORICOUNIDADEADMINISTRATIVAID"
                            AutoGenerateColumns="false" ClientInstanceName="grdHistoricoUA" EnableCallBacks="false"
                            Width="50%">
                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <Columns>
                                <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="HISTORICOUNIDADEADMINISTRATIVAID"
                                    Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="U.A." FieldName="UNIDADEADMINISTRATIVA" Width="30px"
                                    VisibleIndex="3">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataDateColumn FieldName="DATAINICIO" ReadOnly="true" Caption="Data Início"
                                    VisibleIndex="4">
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataDateColumn FieldName="DATAFIM" ReadOnly="true" Caption="Data Fim"
                                    VisibleIndex="5">
                                </dxwgv:GridViewDataDateColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
</asp:Content>
