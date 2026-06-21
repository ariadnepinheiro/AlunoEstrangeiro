<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="MovimentacaoServidores.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.MovimentacaoServidores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Informe ID/Vínculo do Servidor ou o nome do servidor"
        Width="580px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblServidor" runat="server" Text="Servidor:* " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearch ID="tseServidor" runat="server" SettingsTypeName="Techne.Lyceum.RN.Query.QueryFuncionario"
                        AutoPostBack="true" OnTextChanged="tseServidor_Changed">
                    </tweb:TSearch>
                    <asp:TextBox ID="txtPessoaHidden" Visible="false" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <asp:ObjectDataSource ID="odsLotacao" TypeName="Techne.Lyceum.Net.Basico.MovimentacaoServidores"
        runat="server" SelectMethod="Listar" OnUpdating="odsLotacao_Movimentacao" UpdateMethod="Update">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtPessoaHidden" Name="pessoa" PropertyName="Text" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <dxwgv:ASPxGridView ID="grdLotacao" runat="server" ClientInstanceName="grdLotacao"
        AutoGenerateColumns="False" DataSourceID="odsLotacao" KeyFieldName="pessoa;matricula;ordem"
        OnAfterPerformCallback="grdLotacao_AfterPerformCallback" Width="1500px" Visible="false"
        OnHtmlEditFormCreated="grdLotacao_HtmlEditFormCreated">
        <SettingsEditing Mode="EditForm" />
        <Templates>
            <EditForm>
                <dxw:ContentControl ID="ContentControl1" runat="server">
                    <div style="padding: 4px 4px 3px 4px">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="lblPessoa" runat="server" Text="Pessoa:" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <dxe:ASPxTextBox ID="txtPessoa" runat="server" Value='<%# Bind("pessoa") %>' Visible="false">
                                    </dxe:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblOrdem" runat="server" Text="Ordem: " Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <dxe:ASPxTextBox ID="txtOrdem" runat="server" Value='<%# Bind("ordem") %>' Visible="false">
                                    </dxe:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblMatricula" runat="server" Text="Matrícula:" Visible="false"></asp:Label>
                                </td>
                                <td>
                                    <dxe:ASPxTextBox ID="txtMatricula" runat="server" Value='<%# Bind("matricula") %>'
                                        Visible="false">
                                    </dxe:ASPxTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblSetor" runat="server" Text="Unidade Administrativa:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td>
                                 <tweb:TSearchBox ID="tseSetor" runat="server" SqlSelect="select distinct SETOR, NOME_COMP,UNIDADE_ENS,UA_ATUAL, ua_antiga from VW_UNIDADE_ENSINO_SITUACAO"
                                        AutoPostBack="false" SqlOrder="ua_atual" ColumnName="ua_atual" Caption="" key="ua_atual"  FollowContainerMode="false"
                                         MaxLength="15" DataType="Varchar" Value='<%# Bind("ua_atual") %>' >
                                        <GridColumns>                                           
                                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="NOME_COMP" Width="80%" />
                                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
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
                <EditButton Text="Editar" Visible="True">
                    <Image Url="~/img/bt_editar.png" />
                </EditButton>
                <DeleteButton Text="Remover" Visible="False">
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
            <dxwgv:GridViewDataTextColumn Caption="Pessoa" FieldName="pessoa" VisibleIndex="0"
                Visible="false">
                <PropertiesTextEdit>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Ordem" FieldName="ordem" VisibleIndex="1"
                CellStyle-HorizontalAlign="Center">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Id/Vínculo" FieldName="idvinculo" VisibleIndex="2">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Matrícula ou Id/Vínculo" FieldName="matricula" VisibleIndex="3">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Função" FieldName="descricao02" VisibleIndex="4"
                Width="200px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="U.A. Antiga" FieldName="setor" Width="150px" VisibleIndex="6">
                <PropertiesTextEdit MaxLength="15" Width="150px">
                    <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithTooltip">
                        <RequiredField ErrorText="Favor selecionar um setor." IsRequired="True" />
                    </ValidationSettings>
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="U.A." FieldName="ua_atual" Width="30px" VisibleIndex="8">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Coordenadoria" FieldName="descricao03" VisibleIndex="7"
                Width="300px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="nomecomp02"
                VisibleIndex="8" Width="300px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="turno" VisibleIndex="9"
                Width="150px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data Admissão" FieldName="data_nomeacao" VisibleIndex="10"
                Width="100px">
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn FieldName="data_nomeacao_do" Caption="Data da Publicação da Admissão"
                VisibleIndex="11" Width="100px">
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data da Exoneração" FieldName="data_desativacao"
                VisibleIndex="12" Width="100px">
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataDateColumn Caption="Data da Publicação da Exoneração" FieldName="data_desativacao_do"
                VisibleIndex="13" Width="100px">
            </dxwgv:GridViewDataDateColumn>
            <dxwgv:GridViewDataComboBoxColumn Caption="Documentação" VisibleIndex="12" FieldName="resp_documentacao"
                Width="100px">
                <PropertiesComboBox ValueType="System.String" Width="100px">
                    <Items>
                        <dxe:ListEditItem Text="Sim" Value="S" />
                        <dxe:ListEditItem Text="Não" Value="N" />
                    </Items>
                </PropertiesComboBox>
            </dxwgv:GridViewDataComboBoxColumn>
            <dxwgv:GridViewDataTextColumn Caption="Ato Oficial" FieldName="ato_oficial" VisibleIndex="14"
                Width="200px">
            </dxwgv:GridViewDataTextColumn>
            <dxwgv:GridViewDataTextColumn Caption="Unidade Física" FieldName="unidade_fis" VisibleIndex="15"
                Width="150px" Visible="false">
                <PropertiesTextEdit Width="150px">
                </PropertiesTextEdit>
            </dxwgv:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFilterRow="false" ShowFilterRowMenu="false" />
    </dxwgv:ASPxGridView>
</asp:Content>
