<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="RegiaoFgv.aspx.cs" Inherits="Techne.Lyceum.Net.PrestacaoContas.RegiaoFgv" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:Panel ID="pnAbas" runat="server" Width="800px" Visible="true">
        <dxtc:ASPxPageControl ID="pcRegiao" runat="server" ActiveTabIndex="0" Width="800px"
            Visible="true" OnTabClick="pcRegiao_TabClick">
            <TabPages>
                <dxtc:TabPage Text="Dados Gerais" Name="Dados Gerais">
                    <ContentCollection>
                        <dxw:ContentControl ID="ccDados" runat="server">
                            <asp:ObjectDataSource ID="odsRegiaoFgv" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.RegiaoFgv"
                                SelectMethod="Lista" InsertMethod="Insert" UpdateMethod="Update" DeleteMethod="Delete">
                            </asp:ObjectDataSource>
                            <dxwgv:ASPxGridView ID="grdRegiaoFgv" runat="server" DataSourceID="odsRegiaoFgv"
                                KeyFieldName="REGIAOFGVID" AutoGenerateColumns="false" ClientInstanceName="grdRegiaoFgv"
                                OnInitNewRow="grdRegiaoFgv_InitNewRow" OnStartRowEditing="grdRegiaoFgv_StartRowEditing"
                                OnRowInserting="grdRegiaoFgv_RowInserting" OnRowUpdating="grdRegiaoFgv_RowUpdating"
                                OnRowDeleting="grdRegiaoFgv_RowDeleting" OnAfterPerformCallback="grdRegiaoFgv_AfterPerformCallback"
                                Width="60%">
                                <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                <SettingsEditing Mode="Inline" />
                                <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                <SettingsBehavior ConfirmDelete="true" />
                                <Columns>
                                    <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                                        <HeaderCaptionTemplate>
                                            <div style="text-align: center">
                                                <img id="btnNovoGrid" runat="server" alt="Novo" style="cursor: pointer" src="../img/bt_novo.png"
                                                    onclick="grdRegiaoFgv.AddNewRow();" />
                                            </div>
                                        </HeaderCaptionTemplate>
                                        <CancelButton Visible="true" Text="Cancelar">
                                            <Image Url="~/img/bt_cancelar.png" />
                                        </CancelButton>
                                        <EditButton Visible="True" Text="Editar">
                                            <Image Url="../img/bt_editar.png" />
                                        </EditButton>
                                        <DeleteButton Visible="True" Text="Remover">
                                            <Image Url="../img/bt_exclui2.png" />
                                        </DeleteButton>
                                        <ClearFilterButton Text="Limpar" Visible="True">
                                            <Image Url="~/img/bt_limpa.png" />
                                        </ClearFilterButton>
                                        <UpdateButton Visible="true" Text="Alterar">
                                            <Image Url="../img/bt_salvar.png" />
                                        </UpdateButton>
                                    </dxwgv:GridViewCommandColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="REGIAOFGVID"
                                        Visible="false" Width="700px">
                                        <PropertiesTextEdit MaxLength="200">
                                        </PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataTextColumn Caption="Descrição*" Name="Descricao" VisibleIndex="2"
                                        FieldName="DESCRICAO" Width="300px">
                                        <PropertiesTextEdit MaxLength="100">
                                        </PropertiesTextEdit>
                                    </dxwgv:GridViewDataTextColumn>
                                    <dxwgv:GridViewDataDateColumn Caption="Data Ínicio*" FieldName="DATAINICIO" VisibleIndex="8"
                                        Width="100px">
                                        <PropertiesDateEdit Width="100px" EditFormat="Date">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </PropertiesDateEdit>
                                    </dxwgv:GridViewDataDateColumn>
                                    <dxwgv:GridViewDataDateColumn Caption="Data Fim" FieldName="DATAFIM" VisibleIndex="8"
                                        Width="100px">
                                        <PropertiesDateEdit Width="100px" EditFormat="Date">
                                            <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                            </CalendarProperties>
                                        </PropertiesDateEdit>
                                    </dxwgv:GridViewDataDateColumn>
                                </Columns>
                            </dxwgv:ASPxGridView>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Text="Municípios" Name="Municípios">
                    <ContentCollection>
                        <dxw:ContentControl ID="ccVigencia" runat="server">
                            <asp:Panel ID="Panel1" runat="server">
                                <table>
                                    <tr>
                                        <td style="text-align: right; width: 15%">
                                            <asp:Label Font-Names="Verdana" ID="Label1" runat="server" Text="Região FGV:* " SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <tweb:TSearchBox ID="tseRegiaoFGV" runat="server" SqlSelect=" select * from PrestacaoContas.REGIAOFGV "
                                                GridWidth="600px" ArgumentColumns="50" OnChanged="tseRegiaoFGV_Changed" Columns="10"
                                                Argument="DESCRICAO" Key="REGIAOFGVID" MaxLength="10" DataType="Number">
                                                <GridColumns>
                                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="REGIAOFGVID" Width="20%" />
                                                    <tweb:TSearchBoxColumn Caption="Região" FieldName="DESCRICAO" Width="60%" />
                                                </GridColumns>
                                            </tweb:TSearchBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right; width: 15%">
                                            <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:* "
                                                SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td colspan="3">
                                            <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
                                                GridWidth="600px" ArgumentColumns="50" OnChanged="tseMunicipio_Changed" Columns="10"
                                                MaxLength="10">
                                                <GridColumns>
                                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                                    <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                                                    <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                                                </GridColumns>
                                            </tweb:TSearchBox>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td align="left">
                                            <asp:Button ID="btnSalvar" runat="server" ValidationGroup="SalvarForm" Text="Salvar"
                                                OnClick="btnSalvar_Click" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnCancelarAtualizacao" runat="server" ValidationGroup="SalvarForm"
                                                Text="Cancelar" OnClick="btnCancelarAtualizacao_Click" Visible="false" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                                <br />
                                <asp:ObjectDataSource ID="odsRegiaoMunicipio" runat="server" TypeName="Techne.Lyceum.Net.PrestacaoContas.RegiaoFgv"
                                    SelectMethod="ListaMunicipio" DeleteMethod="DeleteMunicipio">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="tseRegiaoFGV" DefaultValue="" Name="regiao" PropertyName="DBValue" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                                <dxwgv:ASPxGridView ID="grdRegiaoMunicipio" runat="server" DataSourceID="odsRegiaoMunicipio"
                                    KeyFieldName="REGIAOFGV__MUNICIPIOID" AutoGenerateColumns="false" ClientInstanceName="grdRegiaoMunicipio"
                                    OnInitNewRow="grdRegiaoMunicipio_InitNewRow" OnStartRowEditing="grdRegiaoMunicipio_StartRowEditing"
                                    OnRowDeleting="grdRegiaoMunicipio_RowDeleting" OnAfterPerformCallback="grdRegiaoMunicipio_AfterPerformCallback"
                                    Width="60%">
                                    <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                                    <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                    <SettingsBehavior ConfirmDelete="true" />
                                    <Columns>
                                        <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                                            <CancelButton Visible="true" Text="Cancelar">
                                                <Image Url="~/img/bt_cancelar.png" />
                                            </CancelButton>
                                            <DeleteButton Visible="True" Text="Remover">
                                                <Image Url="../img/bt_exclui2.png" />
                                            </DeleteButton>
                                            <ClearFilterButton Text="Limpar" Visible="True">
                                                <Image Url="~/img/bt_limpa.png" />
                                            </ClearFilterButton>
                                        </dxwgv:GridViewCommandColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="REGIAOFGV__MUNICIPIOID" FieldName="REGIAOFGV__MUNICIPIOID"
                                            Visible="False" VisibleIndex="5">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="MUNICIPIOID" Name="MUNICIPIOID" VisibleIndex="1"
                                            FieldName="MUNICIPIOID" Visible="false" Width="700px">
                                            <PropertiesTextEdit MaxLength="200">
                                            </PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="REGIAOFGVID" Name="REGIAOFGVID" VisibleIndex="1"
                                            FieldName="REGIAOFGVID" Visible="false" Width="700px">
                                            <PropertiesTextEdit MaxLength="200">
                                            </PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Município*" Name="Nome" VisibleIndex="2" FieldName="NOME"
                                            Width="300px">
                                            <PropertiesTextEdit MaxLength="100">
                                            </PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                </dxwgv:ASPxGridView>
                            </asp:Panel>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
            </TabPages>
        </dxtc:ASPxPageControl>
    </asp:Panel>
</asp:Content>
