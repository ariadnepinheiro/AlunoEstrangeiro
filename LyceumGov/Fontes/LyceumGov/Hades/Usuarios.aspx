<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Modulos/LyceumMaster.Master"
    CodeBehind="Usuarios.aspx.cs" Inherits="Techne.Lyceum.Net.Hades.Usuarios" %>

<asp:Content ID="conUsuarios" ContentPlaceHolderID="cphFormulario" runat="server">
    <techne:TTableDataSource ID="tdsPadUsuario" runat="server" DataTableClassName="Techne.HadesLyc.CR.Hd_padusuario"
        SqlWhere="hd_padusuario.usuario = @usuario">
        <SqlWhereParameters>
            <asp:ControlParameter ControlID="tseUsuario" Name="usuario" PropertyName="DBValue" />
        </SqlWhereParameters>
    </techne:TTableDataSource>
    <techne:TTableDataSource ID="tdsPadaces" runat="server" DataTableClassName="Techne.HadesLyc.CR.Hd_padaces">
    </techne:TTableDataSource>
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por usuário"
        Height="51px" Width="600px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblBusca" runat="server" Text="Usuário:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUsuario" runat="server" Argument="nomeusuario" Caption=""
                        Key="usuario" SqlOrder="usuario" SqlSelect="SELECT usuario, nomeusuario, matricula, idvinculo, grupousu, p.e_mail_interno FROM usuario u left join ly_pessoa p on u.pessoa_usuario = p.pessoa"
                        OnChanged="tseUsuario_Changed" MaxLength="15">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Usuário" FieldName="usuario" Width="15%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nomeusuario" Width="55%" />
                            <tweb:TSearchBoxColumn Caption="E-mail" FieldName="e_mail_interno" Width="30%" />
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
        <asp:ImageButton ID="btnExcluir" runat="server" SkinID="BcDesabilitar" OnClick="btnExcluir_Click"
            OnClientClick="return confirm('Confirma a desabilitação?');" />
        <asp:ImageButton ID="btnEditar" runat="server" SkinID="BcEditar" OnClick="btnEditar_Click" />
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnSalvar" runat="server" SkinID="BcSalvar" OnClick="btnSalvar_Click"
            ValidationGroup="SalvarForm" />
        <asp:Label runat="server" ID="lblBloco" Text="Usuários" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsDocente" runat="server" ShowMessageBox="true" ValidationGroup="SalvarForm"
            ShowSummary="false" />
    </div>
    <dxtc:ASPxPageControl ID="pcUsuarios" runat="server" ActiveTabIndex="0" Width="600px">
        <TabPages>
            <dxtc:TabPage Text="Dados do Usuário">
                <ContentCollection>
                    <dxw:ContentControl runat="server">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblTipoUsuarios" runat="server" Text="Tipo:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:RadioButtonList ID="rblTipoUsuario" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" OnSelectedIndexChanged="rblTipoUsuario_SelectedIndexChanged">
                                        <asp:ListItem Text="Seeduc" Value="Seeduc" Selected="true"></asp:ListItem>
                                        <asp:ListItem Text="Externo" Value="Externo"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblTipoUsuarioExterno" runat="server" Text="Tipo Externo:* " SkinID="lblObrigatorio"
                                        Visible="false"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlTipoUsuarioExterno" runat="server" DataTextField="DESCRICAO"
                                        DataValueField="TIPOUSUARIOEXTERNOID" Visible="false">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblFuncionario" runat="server" Text="Funcionário:* " SkinID="lblObrigatorio"></asp:Label>
                                    <asp:Label ID="lblUsuarioExterno" runat="server" Text="Externo:* " SkinID="lblObrigatorio"
                                        Visible="false"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <tweb:TSearch ID="tseFuncionario" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryFuncionario"
                                        AutoPostBack="true" OnChanged="tseFuncionario_Changed" MaxLength="8">
                                    </tweb:TSearch>
                                    <tweb:TSearch ID="tseUsuarioExterno" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryUsuarioExterno"
                                        AutoPostBack="true" OnTextChanged="tseUsuarioExterno_Changed" MaxLength="11"
                                        Visible="false">
                                        <QueryParameters>
                                            <asp:ControlParameter ControlID="ddlTipoUsuarioExterno" Name="tipoUsuarioExterno"
                                                PropertyName="SelectedValue" />
                                        </QueryParameters>
                                    </tweb:TSearch>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblIdVinculo" runat="server" Text="ID/Vinculo:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtIdVinculo" runat="server" MaxLength="50" Width="200px" ReadOnly="true" />
                                </td>
                                <td>
                                    <asp:Label ID="lblMatricula" runat="server" Text="Matricula:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMatricula" runat="server" MaxLength="50" Width="200px" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblNome" runat="server" Text="Nome:"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtNome" runat="server" MaxLength="50" Width="400px" ReadOnly="true" />
                                    <asp:RequiredFieldValidator ID="rfvNome" runat="server" ControlToValidate="txtNome"
                                        InitialValue="" ErrorMessage="Nome: Preenchimento obrigatório." ValidationGroup="SalvarForm">
                                        <img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblTelefone" runat="server" Text="Telefone:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTelefone" runat="server" MaxLength="50" Width="200px" ReadOnly="true" />
                                </td>
                                <td>
                                    <asp:Label ID="lblCelular" runat="server" Text="Celular:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCelular" runat="server" MaxLength="50" Width="200px" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblEmail" runat="server" Text="E-mail:"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtEmail" autocomplete="off" AutoCompleteType="Disabled" runat="server"
                                        MaxLength="50" Width="400px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSetor" runat="server" Text="Unidade Administrativa:"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <tweb:TSearchBox ID="tseSetor" runat="server" SqlSelect="SELECT ua_atual, nomesetor, ua_antiga, setor FROM hades..vw_setor"
                                        AutoPostBack="false" SqlOrder="ua_atual" Caption="" Key="ua_atual" Connection="Hades"
                                        MaxLength="15" DataType="Varchar" ReadOnly="true" CssClass="ReadOnlyField" Argument="nomesetor">
                                        <GridColumns>
                                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
                                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nomesetor" Width="80%" />
                                        </GridColumns>
                                    </tweb:TSearchBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblUsuario" runat="server" Text="Usuário:*" SkinID="lblObrigatorio"
                                        Enabled="false"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtUsuario" runat="server" MaxLength="8" Enabled="false" />
                                    <asp:RequiredFieldValidator ID="rfvUsuario" runat="server" ControlToValidate="txtUsuario"
                                        InitialValue="" ErrorMessage="Usuário: Preenchimento obrigatório." ValidationGroup="SalvarForm"><img src="../Images/AlertaMens.gif" alt="Campo Obrigatório!" /></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSenha" runat="server" Text="Senha:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtSenha" runat="server" MaxLength="30" TextMode="Password" />
                                    <asp:Label Font-Names="Verdana" ID="lblSenha2" runat="server" Text="****"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblPrivilegiado" runat="server" Text="Privilegiado:"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <dxe:ASPxCheckBox ID="chkPrivilegiado" runat="server" ValueType="System.String" ValueChecked="S"
                                        ValueUnchecked="N" Checked="true">
                                    </dxe:ASPxCheckBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblHabilitado" runat="server" Text="Habilitado:"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <dxe:ASPxCheckBox ID="chkHabilitado" runat="server" ValueType="System.String" ValueChecked="S"
                                        ValueUnchecked="N" Checked="true">
                                    </dxe:ASPxCheckBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblPrivilegiadoUE" runat="server" Text="Privilegiado - Unidades de Ensino:"></asp:Label>
                                </td>
                                <td>
                                    <dxe:ASPxCheckBox ID="chkPrivilegiadoUE" runat="server" ValueType="System.String"
                                        ValueChecked="S" ValueUnchecked="N" Checked="true">
                                    </dxe:ASPxCheckBox>
                                </td>
                                <td colspan="2" align="right">
                                    <dxe:ASPxButton ID="btnResetarCpf" runat="server" Text="Resetar Senha para CPF" OnClick="btnResetarCpf_Click">
                                    </dxe:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Padrões de Acesso" Name="Padrões de Acesso">
                <ContentCollection>
                    <dxw:ContentControl runat="server">
                        <dxwgv:ASPxGridView runat="server" ID="grdPadUsuario" DataSourceID="tdsPadUsuario"
                            ClientInstanceName="grdPadUsuario" AutoGenerateColumns="False" KeyFieldName="CompositeKey"
                            OnCellEditorInitialize="grdPadUsuario_CellEditorInitialize" OnCustomUnboundColumnData="grdPadUsuario_CustomUnboundColumnData"
                            OnRowDeleting="grdPadUsuario_RowDeleting" OnRowInserting="grdPadUsuario_RowInserting"
                            OnRowUpdating="grdPadUsuario_RowUpdating" Width="500px" OnInitNewRow="grdPadUsuario_InitNewRow"
                            OnAfterPerformCallback="grdPadUsuario_AfterPerformCallback">
                            <SettingsBehavior ConfirmDelete="True" AllowMultiSelection="False" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsText ConfirmDelete="Confirma a remoção ?" EmptyDataRow="Não existem dados." />
                            <Templates>
                                <EditForm>
                                    <dxw:ContentControl ID="conProgramaAluno" runat="server">
                                        <div style="padding: 4px 4px 3px 4px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblProgramaUnidade" runat="server" Text="Padrão de Acesso:* " SkinID="lblObrigatorio"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <tweb:TSearchBox ID="tsePadrao" AutoPostBack="false" runat="server" Value='<%# Bind("padaces") %>'
                                                            Caption="" SqlSelect="SELECT padaces, nomepadaces from padaces" ArgumentColumns="60"
                                                            Columns="20" MaxLength="14" GridWidth="600px" SqlOrder="nomepadaces">
                                                            <GridColumns>
                                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="padaces" Width="30%" />
                                                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nomepadaces" Width="70%" />
                                                            </GridColumns>
                                                        </tweb:TSearchBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <dxwgv:ASPxGridViewTemplateReplacement ID="repUpdate" ReplacementType="EditFormUpdateButton"
                                                runat="server">
                                            </dxwgv:ASPxGridViewTemplateReplacement>
                                            <dxwgv:ASPxGridViewTemplateReplacement ID="repCancel" ReplacementType="EditFormCancelButton"
                                                runat="server">
                                            </dxwgv:ASPxGridViewTemplateReplacement>
                                    </dxw:ContentControl>
                                    </div>
                                </EditForm>
                            </Templates>
                            <Columns>
                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="15%">
                                    <HeaderCaptionTemplate>
                                        <div style="text-align: center">
                                            <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                                onclick="grdPadUsuario.AddNewRow();" alt="Novo" />
                                        </div>
                                    </HeaderCaptionTemplate>
                                    <EditButton Text="Editar">
                                        <Image Url="~/img/bt_editar.png" />
                                    </EditButton>
                                    <DeleteButton Text="Remover" Visible="True">
                                        <Image Url="~/img/bt_exclui2.png" />
                                    </DeleteButton>
                                    <CancelButton Text="Cancelar">
                                        <Image Url="~/img/bt_cancelar.png" />
                                    </CancelButton>
                                    <UpdateButton>
                                        <Image Url="~/img/bt_salvar.png" />
                                    </UpdateButton>
                                    <ClearFilterButton Text="Limpar" Visible="True">
                                        <Image Url="~/img/bt_limpa.png" />
                                    </ClearFilterButton>
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Padrão de Acesso*" FieldName="padaces" VisibleIndex="1"
                                    ReadOnly="true" Visible="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="usuario" ReadOnly="False" VisibleIndex="2"
                                    Caption="Usuário" Visible="false">
                                    <PropertiesTextEdit MaxLength="20">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn FieldName="nome02" VisibleIndex="3" Caption="Nome"
                                    Visible="false">
                                    <PropertiesTextEdit MaxLength="50">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataColumn FieldName="CompositeKey" VisibleIndex="4" UnboundType="String"
                                    Visible="False">
                                </dxwgv:GridViewDataColumn>
                            </Columns>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                        </dxwgv:ASPxGridView>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Restrição de acesso por usuário" Name="Restrição de acesso por usuário">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl1" runat="server">
                        <techne:TTableDataSource ID="tdsUsuarioUnidade" runat="server" DataTableClassName="Techne.Lyceum.CR.Ly_usuario_unidade_fis"
                            SqlWhere="usuario = @usuario">
                            <SqlWhereParameters>
                                <asp:ControlParameter ControlID="tseUsuario" DefaultValue="" Name="usuario" PropertyName="DBValue" />
                            </SqlWhereParameters>
                        </techne:TTableDataSource>
                        <techne:TTableDataSource ID="tdsUnidadeFisica" runat="server" DataTableClassName="Techne.Lyceum.CR.Vw_zzcro_unidade_fisica">
                        </techne:TTableDataSource>
                        <br />
                        <br />
                        <asp:Label ID="Label1" runat="server" SkinID="lblMensagem"></asp:Label>
                        <asp:Button ID="btnAdicionarTodasUnidades" Text="Incluir todas Unidades" Width="160px"
                            runat="server" OnClick="btnAdicionarTodasUnidades_Click" Visible="false" OnClientClick="javascript: return confirm('Todas as Unidades Físicas cadastradas serão associadas ao usuário.\nClique em OK para continuar.');" />
                        <asp:Button ID="btnRemoverTodasUnidades" Text="Remover todas Unidades" Width="160px"
                            runat="server" OnClick="btnRemoverTodasUnidades_Click" Visible="false" OnClientClick="javascript: return confirm('Todas as associações de Unidades Físicas relacionadas ao usuário serão removidas.\nClique em OK para continuar.');" />
                        <br />
                        <br />
                        <dxwgv:ASPxGridView ID="grdUsuarioUnidade" runat="server" AutoGenerateColumns="False"
                            Width="500px" ClientInstanceName="grdUsuarioUnidade" DataSourceID="tdsUsuarioUnidade"
                            KeyFieldName="CompositeKey" OnCustomUnboundColumnData="grdUsuarioUnidade_CustomUnboundColumnData"
                            OnRowDeleting="grdUsuarioUnidade_RowDeleting" OnCellEditorInitialize="grdUsuarioUnidade_CellEditorInitialize"
                            OnRowInserting="grdUsuarioUnidade_RowInserting" OnRowUpdating="grdUsuarioUnidade_RowUpdating"
                            OnInitNewRow="grdUsuarioUnidade_InitNewRow" OnRowValidating="grdUsuarioUnidade_RowValidating"
                            OnAfterPerformCallback="grdUsuarioUnidade_AfterPerformCallback" Visible="false">
                            <SettingsBehavior ConfirmDelete="True" />
                            <SettingsEditing Mode="EditForm" />
                            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                            <Templates>
                                <EditForm>
                                    <dxw:ContentControl ID="ContentControl1" runat="server">
                                        <div style="padding: 4px 4px 3px 4px">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblUnidadeFisica" runat="server" Text="Unidade Física:* " SkinID="lblObrigatorio"></asp:Label>
                                                    </td>
                                                    <td colspan="3">
                                                        <tweb:TSearchBox ID="tseUnidadeFisica" runat="server" Argument="nome_comp" ArgumentColumns="30"
                                                            FollowContainerMode="false" MaxLength="20" AutoPostBack="false" Columns="10"
                                                            DataType="VarChar" Key="unidade_fis" Value='<%# Bind("unidade_fis") %>' SqlOrder="nome_comp"
                                                            SqlSelect="SELECT unidade_fis, nome_comp FROM Ly_Unidade_Fisica">
                                                            <GridColumns>
                                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_fis" Width="20%" />
                                                                <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome_comp" Width="80%" />
                                                            </GridColumns>
                                                        </tweb:TSearchBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <dxwgv:ASPxGridViewTemplateReplacement ID="repUpdate" ReplacementType="EditFormUpdateButton"
                                                runat="server">
                                            </dxwgv:ASPxGridViewTemplateReplacement>
                                            <dxwgv:ASPxGridViewTemplateReplacement ID="repCancel" ReplacementType="EditFormCancelButton"
                                                runat="server">
                                            </dxwgv:ASPxGridViewTemplateReplacement>
                                    </dxw:ContentControl>
                                    </div>
                                </EditForm>
                            </Templates>
                            <Columns>
                                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                    <HeaderCaptionTemplate>
                                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                                            onclick="grdUsuarioUnidade.AddNewRow();" alt="Novo" />
                                    </HeaderCaptionTemplate>
                                    <EditButton Text="Editar">
                                        <Image Url="~/img/bt_editar.png" />
                                    </EditButton>
                                    <DeleteButton Text="Remover" Visible="True">
                                        <Image Url="~/img/bt_exclui2.png" />
                                    </DeleteButton>
                                    <CancelButton Text="Cancelar">
                                        <Image Url="~/img/bt_cancelar.png" />
                                    </CancelButton>
                                    <UpdateButton>
                                        <Image Url="~/img/bt_salvar.png" />
                                    </UpdateButton>
                                    <ClearFilterButton Text="Limpar" Visible="True">
                                        <Image Url="~/img/bt_limpa.png" />
                                    </ClearFilterButton>
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Usuário" FieldName="usuario" ReadOnly="True"
                                    VisibleIndex="1" Visible="False">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="unidade_fis" FieldName="unidade_fis" VisibleIndex="1"
                                    Visible="False">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Unidade Física" FieldName="nome_comp03" VisibleIndex="2">
                                    <PropertiesTextEdit>
                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField ErrorText="Favor informar a unidade física." IsRequired="True" />
                                        </ValidationSettings>
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                                    Visible="False" VisibleIndex="3">
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                        </dxwgv:ASPxGridView>
                        <asp:Panel ID="pnRegional" runat="server" GroupingText="Selecione a Regional para inserir unidades físicas por Regional"
                            Width="600px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblRegional" runat="server" Text="Regional:"></asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                                            MaxLength="20" Columns="10" AutoPostBack="True" Caption="" SqlOrder="regional"
                                            Key="id_regional" SqlSelect="SELECT DISTINCT S.id_regional AS id_regional, regional FROM VW_UNIDADE_ENSINO_SITUACAO S INNER JOIN TCE_REGIONAL R ON S.ID_REGIONAL=R.ID_REGIONAL"
                                            DataType="Number">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                                                <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Button ID="btnInserirRegional" Text="Incluir" Width="160px" runat="server" OnClick="btnInserirRegional_Click"
                                Visible="true" OnClientClick="javascript: return confirm('Todas as Unidades Físicas relacionadas a Regional selecionada serão associadas ao usuário.\nClique em OK para continuar.');" />
                            <asp:Button ID="btnRemoverRegional" Text="Remover" Width="160px" runat="server" OnClick="btnRemoverRegional_Click"
                                Visible="true" OnClientClick="javascript: return confirm('Todas as associações de Unidades Físicas do usuário relacionadas a Regional selecionada serão removidas.\nClique em OK para continuar.');" />
                        </asp:Panel>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
</asp:Content>
