<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ParametrizacaoTermoCompromisso.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.ParametrizacaoTermoCompromisso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnAbas" runat="server" Width="800px">
        <dxtc:ASPxPageControl ID="pcTermo" runat="server" ActiveTabIndex="0" Width="800px"
            OnTabClick="pcTermo_TabClick">
            <TabPages>
                <dxtc:TabPage Text="Conexão Gestão">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl1" runat="server">
                            <asp:Panel ID="pnGeral" runat="server" GroupingText="Informe os dados para inclusão:"
                                Width="800px">
                                <table>
                                    <tr>
                                        <td style="text-align: right; width: 15%">
                                            <asp:Label ID="lblAno" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlAno" runat="server" AutoPostBack="True" DataTextField="ano"
                                                DataValueField="ano" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged" AppendDataBoundItems="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="lblPadaces" runat="server" Text="Padrão de Acesso:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <tweb:TSearchBox ID="tsePadrao" runat="server" Caption="" SqlSelect="SELECT padaces, nomepadaces from padaces"
                                                ArgumentColumns="60" Columns="20" MaxLength="14" GridWidth="600px" SqlOrder="nomepadaces"
                                                CssClass="ReadOnlyField">
                                                <GridColumns>
                                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="padaces" Width="30%" />
                                                    <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nomepadaces" Width="70%" />
                                                </GridColumns>
                                            </tweb:TSearchBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label1" runat="server" Text="Data Início:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDataInicio" runat="server" MaxLength="10" onkeyup="formataData(this,event);"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label2" runat="server" Text="Data Fim:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDataFim" runat="server" MaxLength="10" onkeyup="formataData(this,event);"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label3" runat="server" Text="Arquivo:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtArquivo" runat="server" Width="268px" MaxLength="100"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="right">
                                            <asp:Button ID="btnSalvar" runat="server" ValidationGroup="SalvarForm" Text="Salvar"
                                                OnClick="btnSalvar_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnGrid" runat="server">
                                <dxwgv:ASPxGridView ID="grdCompromissoGestao" runat="server" AutoGenerateColumns="False"
                                    ClientInstanceName="grdCompromissoGestao" KeyFieldName="ID_TERMO_GESTAO" DataSourceID="odsCompromissoGestao"
                                    OnCellEditorInitialize="grdCompromissoGestao_CellEditorInitialize" OnStartRowEditing="grdCompromissoGestao_StartRowEditing"
                                    OnRowValidating="grdCompromissoGestao_RowValidating" OnAfterPerformCallback="grdCompromissoGestao_AfterPerformCallback">
                                    <Columns>
                                        <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                            <EditButton Text="Editar" Visible="True">
                                                <Image Url="~/img/bt_editar.png" />
                                            </EditButton>
                                            <DeleteButton Text="Remover" Visible="True">
                                                <Image Url="~/img/bt_exclui2.png" />
                                            </DeleteButton>
                                            <CancelButton Text="Cancelar">
                                                <Image Url="~/img/bt_cancelar.png" />
                                            </CancelButton>
                                            <UpdateButton Text="Salvar">
                                                <Image Url="~/img/bt_salvar.png" />
                                            </UpdateButton>
                                        </dxwgv:GridViewCommandColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="ID_TERMO_GESTAO" 
                                            Visible="false" VisibleIndex="1">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ANO" VisibleIndex="2">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Padrao de Acesso" 
                                            FieldName="PADRAO_ACESSO" VisibleIndex="3">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataDateColumn Caption="Data Início" FieldName="DT_INICIO" 
                                            VisibleIndex="4">
                                        </dxwgv:GridViewDataDateColumn>
                                        <dxwgv:GridViewDataDateColumn Caption="Data Fim" FieldName="DT_FIM" 
                                            VisibleIndex="5">
                                        </dxwgv:GridViewDataDateColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Arquivo" FieldName="ARQUIVO" 
                                            VisibleIndex="6">
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsBehavior ConfirmDelete="True" />
                                    <SettingsEditing Mode="Inline" />
                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                                    <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                </dxwgv:ASPxGridView>
                            </asp:Panel>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Text="Docente Online">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl2" runat="server">
                            <asp:Panel ID="Panel1" runat="server" GroupingText="Informe os dados para inclusão:"
                                Width="800px">
                                <table>
                                    <tr>
                                        <td style="text-align: right; width: 15%">
                                            <asp:Label ID="Label4" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlAnoDOL" runat="server" AutoPostBack="True" DataTextField="ano"
                                                DataValueField="ano" OnSelectedIndexChanged="ddlAnoDOL_SelectedIndexChanged"
                                                AppendDataBoundItems="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label6" runat="server" Text="Data Início:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDataInicioDOL" runat="server" MaxLength="10" onkeyup="formataData(this,event);"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label7" runat="server" Text="Data Fim:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDataFimDOL" runat="server" MaxLength="10" onkeyup="formataData(this,event);"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <asp:Label ID="Label8" runat="server" Text="Arquivo:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtArquivoDOL" runat="server" Width="268px" MaxLength="100"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" align="right">
                                            <asp:Button ID="btnSalvarDOL" runat="server" ValidationGroup="SalvarForm" Text="Salvar"
                                                OnClick="btnSalvarDOL_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="Panel2" runat="server">
                                <dxwgv:ASPxGridView ID="grdCompromissoDOL" runat="server" AutoGenerateColumns="False"
                                    ClientInstanceName="grdCompromissoDOL" KeyFieldName="ID_TERMO_DOCENTE" DataSourceID="odsCompromissoDOL"
                                    OnCellEditorInitialize="grdCompromissoDOL_CellEditorInitialize" OnStartRowEditing="grdCompromissoDOL_StartRowEditing"
                                    OnRowValidating="grdCompromissoDOL_RowValidating" OnAfterPerformCallback="grdCompromissoDOL_AfterPerformCallback">
                                    <Columns>
                                        <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                                            <EditButton Text="Editar" Visible="True">
                                                <Image Url="~/img/bt_editar.png" />
                                            </EditButton>
                                            <DeleteButton Text="Remover" Visible="True">
                                                <Image Url="~/img/bt_exclui2.png" />
                                            </DeleteButton>
                                            <CancelButton Text="Cancelar">
                                                <Image Url="~/img/bt_cancelar.png" />
                                            </CancelButton>
                                            <UpdateButton Text="Salvar">
                                                <Image Url="~/img/bt_salvar.png" />
                                            </UpdateButton>
                                        </dxwgv:GridViewCommandColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="ID_TERMO_DOCENTE" 
                                            Visible="false" VisibleIndex="1">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ANO" VisibleIndex="2">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataDateColumn Caption="Data Início" FieldName="DT_INICIO" 
                                            VisibleIndex="3">
                                        </dxwgv:GridViewDataDateColumn>
                                        <dxwgv:GridViewDataDateColumn Caption="Data Fim" FieldName="DT_FIM" 
                                            VisibleIndex="4">
                                        </dxwgv:GridViewDataDateColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Arquivo" FieldName="ARQUIVO" 
                                            VisibleIndex="5">
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsBehavior ConfirmDelete="True" />
                                    <SettingsEditing Mode="Inline" />
                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                                    <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                                </dxwgv:ASPxGridView>
                            </asp:Panel>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
            </TabPages>
        </dxtc:ASPxPageControl>
    </asp:Panel>
    <br />
    <asp:ObjectDataSource ID="odsCompromissoGestao" TypeName="Techne.Lyceum.Net.Basico.ParametrizacaoTermoCompromisso"
        runat="server" SelectMethod="Listar" OnDeleting="odsCompromissoGestao_Deleting"
        UpdateMethod="Update" DeleteMethod="Delete" OnUpdating="odsCompromissoGestao_Updating">
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsCompromissoDOL" TypeName="Techne.Lyceum.Net.Basico.ParametrizacaoTermoCompromisso"
        runat="server" SelectMethod="ListarDOL" OnDeleting="odsCompromissoDOL_Deleting"
        UpdateMethod="UpdateDOL" DeleteMethod="DeleteDOL" OnUpdating="odsCompromissoDOL_Updating">
    </asp:ObjectDataSource>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
  
    
</asp:Content>
