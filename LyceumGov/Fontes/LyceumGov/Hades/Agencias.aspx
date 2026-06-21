<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Agencias.aspx.cs" Inherits="Techne.Lyceum.Net.Hades.Agencias" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <techne:TTableDataSource ID="tdsAgencia" runat="server" DataTableClassName="Techne.HadesLyc.CR.Hd_agencia"
        SqlOrder="Hd_agencia.nome" SqlWhere="Hd_agencia.banco = @banco">
        <sqlwhereparameters>
            <asp:ControlParameter ControlID="tseBanco" Name="banco" PropertyName="DBValue" />
        </sqlwhereparameters>
    </techne:TTableDataSource>
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça um busca pelo banco">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblTseach" runat="server" Text="Banco: "></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseBanco" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryBanco"
                        AutoPostBack="true" OnChanged="tseBanco_Changed" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <dxwgv:ASPxGridView ID="grdAgencia" runat="server" AutoGenerateColumns="False" DataSourceID="tdsAgencia"
        Visible="false" ClientInstanceName="grdAgencia" KeyFieldName="agencia;banco"
        Font-Names="Verdana" Font-Size="Small" OnAfterPerformCallback="grdAgencia_AfterPerformCallback"
        OnRowInserting="grdAgencia_RowInserting" OnRowUpdating="grdAgencia_RowUpdating"
        OnCustomColumnDisplayText="grdAgencia_CustomColumnDisplayText">
        <SettingsBehavior ConfirmDelete="True" />
        <SettingsEditing Mode="EditForm" />
        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
        <Columns>
            <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                <HeaderCaptionTemplate>
                    <div style="text-align: center">
                        <img runat="server" id="btnNovoGrid" src="../img/bt_novo.png" style="cursor: pointer"
                            onclick="grdAgencia.AddNewRow();" alt="Novo" />
                    </div>
                </HeaderCaptionTemplate>
                <EditButton Text="Editar" Visible="True">
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
            <dxwgv:GridViewDataTextColumn Caption="Banco*" HeaderStyle-Font-Bold="true" FieldName="banco"
                VisibleIndex="1" Visible="false">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Número*" HeaderStyle-Font-Bold="true" FieldName="agencia"
                VisibleIndex="1">
                <PropertiesTextEdit MaxLength="7" Width="60px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o número da agência." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Nome" HeaderStyle-Font-Bold="true" FieldName="nome"
                VisibleIndex="2">
                <PropertiesTextEdit MaxLength="50" Width="350px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor informar o nome." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Município" FieldName="municipio" Visible="false"
                VisibleIndex="3">
                <PropertiesTextEdit MaxLength="60" Width="250px">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Município" FieldName="nome_municipio" VisibleIndex="4">
                <PropertiesTextEdit MaxLength="60" Width="250px">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Bairro" FieldName="bairro" Visible="false"
                VisibleIndex="5">
                <PropertiesTextEdit MaxLength="60" Width="250px">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Bairro" FieldName="nome_bairro" Visible="false"
                VisibleIndex="6">
                <PropertiesTextEdit MaxLength="60" Width="250px">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="UF" FieldName="uf" VisibleIndex="7">
                <PropertiesTextEdit MaxLength="60" Width="90%">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Endereço" FieldName="endereco" VisibleIndex="8">
                <PropertiesTextEdit MaxLength="60" Width="250px">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Contato" FieldName="contato" VisibleIndex="9">
                <PropertiesTextEdit MaxLength="60" Width="250px">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Cargo" FieldName="cargo" VisibleIndex="10">
                <PropertiesTextEdit MaxLength="12" Width="100px">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Telefone" FieldName="telefone" VisibleIndex="11">
                <PropertiesTextEdit MaxLength="13" Width="100px">
                    <MaskSettings IncludeLiterals="None" Mask="(##)####-####" />
                    <ValidationSettings>
                        <RegularExpression ErrorText="Telefone inválido." ValidationExpression="[1][1-9][1-5]\d{3}\d{4}\d*|[2-9][0-9][1-5]\d{3}\d{4}\d*" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="CEP" FieldName="cep" VisibleIndex="12">
                <PropertiesTextEdit MaxLength="8" Width="100px">
                    <MaskSettings IncludeLiterals="None" Mask="#####-###" />
                    <ClientSideEvents KeyPress="function (s, e){ SomentePermitirNumeros(s, e.htmlEvent); }" />
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RegularExpression ErrorText="O campo CEP só aceita números inteiros e positivos."
                            ValidationExpression="^[+]?\d*$" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Templates>
            <EditForm>
                <dxw:ContentControl ID="conGrid" runat="server">
                    <div style="padding: 4px 4px 3px 4px">
                        <table>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblAgencia" runat="server" Text="Número:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                    <dxwgv:ASPxGridViewTemplateReplacement runat="server" ColumnID="agencia" ID="idAgencia"
                                        ReplacementType="EditFormCellEditor">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblNome" runat="server" Text="Nome:* " SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <dxwgv:ASPxGridViewTemplateReplacement runat="server" ColumnID="nome" ID="idNome"
                                        ReplacementType="EditFormCellEditor">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblMunicipio" runat="server" Text="Município: "></asp:Label>
                                </td>
                                <td colspan="3">
                                    <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect="SELECT codigo, nome, uf_sigla FROM municipio"
                                        GridWidth="600px" ArgumentColumns="30" Columns="10" Value='<%# Bind("municipio") %>'
                                        MaxLength="10" OnChanged="tseMunicipio_Changed">
                                        <gridcolumns>
                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                                            <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                                        </gridcolumns>
                                    </tweb:TSearchBox>
                                    <input id="txtUF" runat="server" maxlength="2" class="txtInput" readonly="readonly"
                                        style="width: 20px; vertical-align: bottom" value='<%# Bind("uf") %>' />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblBairro" runat="server" Text="Bairro: "></asp:Label>
                                </td>
                                <td colspan="3">
                                    <tweb:TSearchBox ID="tseBairro" runat="server" SqlOrder="nome" SqlSelect="select bairro, nome from hd_bairro"
                                        GridWidth="600px" ArgumentColumns="30" Columns="10" Value='<%# Bind("bairro") %>'
                                        SqlWhere="municipio = #tseMunicipio#" MaxLength="10" Connection="Hades">
                                        <gridcolumns>
                                            <tweb:TSearchBoxColumn Caption="Código" FieldName="bairro" Width="30%" />
                                            <tweb:TSearchBoxColumn Caption="Bairro" FieldName="nome" Width="70%" />
                                        </gridcolumns>
                                    </tweb:TSearchBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblEndereco" runat="server" Text="Endereço: "></asp:Label>
                                </td>
                                <td colspan="3">
                                    <dxwgv:ASPxGridViewTemplateReplacement runat="server" ColumnID="endereco" ID="idEndereco"
                                        ReplacementType="EditFormCellEditor">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblContato" runat="server" Text="Contato: "></asp:Label>
                                </td>
                                <td colspan="3">
                                    <dxwgv:ASPxGridViewTemplateReplacement runat="server" ColumnID="contato" ID="idContato"
                                        ReplacementType="EditFormCellEditor">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblCargo" runat="server" Text="Cargo: "></asp:Label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cmbCargo" runat="server" TextField="cargo" ValueField="cargo"
                                        Value='<%# Bind("cargo") %>'>
                                        <Items>
                                            <dxe:ListEditItem Text="Outros" Value="Outros" />
                                            <dxe:ListEditItem Text="Caixa" Value="Caixa" />
                                            <dxe:ListEditItem Text="Subgerente" Value="Subgerente" />
                                            <dxe:ListEditItem Text="Gerente" Value="Gerente" />
                                        </Items>
                                    </dxe:ASPxComboBox>
                                </td>
                                <td align="right">
                                    <asp:Label ID="lblTelefone" runat="server" Text="Telefone: "></asp:Label>
                                </td>
                                <td>
                                    <dxwgv:ASPxGridViewTemplateReplacement runat="server" ColumnID="telefone" ID="idTelefone"
                                        ReplacementType="EditFormCellEditor">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="lblCep" runat="server" Text="CEP: "></asp:Label>
                                </td>
                                <td colspan="3">
                                    <dxwgv:ASPxGridViewTemplateReplacement runat="server" ColumnID="cep" ID="idCep" ReplacementType="EditFormCellEditor">
                                    </dxwgv:ASPxGridViewTemplateReplacement>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <dxwgv:ASPxGridViewTemplateReplacement ID="repUpdate" ReplacementType="EditFormUpdateButton"
                        runat="server">
                    </dxwgv:ASPxGridViewTemplateReplacement>
                    <dxwgv:ASPxGridViewTemplateReplacement ID="repCancel" ReplacementType="EditFormCancelButton"
                        runat="server">
                    </dxwgv:ASPxGridViewTemplateReplacement>
                </dxw:ContentControl>
            </EditForm>
        </Templates>
        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
    </dxwgv:ASPxGridView>
</asp:Content>
