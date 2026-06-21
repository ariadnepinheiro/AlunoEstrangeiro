<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="DiasNaoLetivo.aspx.cs" Inherits="Techne.Lyceum.Net.Matricula.DiasNaoLetivo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

   function Bloqueio() {

            var divBloqueio = document.getElementById("dvbloqueio");
            divBloqueio.className = "Bloqueado";
        }     
     
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Informe o ano para pesquisar os dias não letivos"
        Width="600px">
        <table>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="lblFiltro" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="cmbAno" runat="server" DataTextField="ano" DataValueField="ano"
                        Width="70px" AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="cmbAno_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:Panel ID="pnlDados" runat="server" GroupingText="Dados" Visible="false" Width="600px">
        <table>
            <tr>
                <td>
                    <asp:Panel ID="Panel1" runat="server" GroupingText="Abrangência">
                        <asp:RadioButtonList ID="rblAbrangencia" runat="server" RepeatDirection="Horizontal"
                            AutoPostBack="true" Width="240px" OnSelectedIndexChanged="rblAbrangencia_SelectedIndexChanged">
                            <asp:ListItem Text="Federal/Estadual RJ" Value="E"></asp:ListItem>
                            <asp:ListItem Text="Municipal" Value="M"></asp:ListItem>
                        </asp:RadioButtonList>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <br />
        <asp:Panel ID="pnlMunicipio" runat="server" Visible="false">
            <table>
                <tr>
                    <td >
                        <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td >
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
        </asp:Panel>
        <br />
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label Font-Names="Verdana" ID="Label1" runat="server" Text="Data*:" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dtData" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                        ClientInstanceName="dtData" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                </td>
            </tr>
        </table>
        <table style="width: 100%">
            <tr>
                <td align="right">
                    <asp:Button ID="btnSalvar" runat="server" OnClick="btnSalvar_Click" Text="Salvar"
                        ValidationGroup="SalvarForm" Style="margin-left: 0px" Width="100px" OnClientClick="Bloqueio();this.disabled = true; this.value = 'Aguarde...';"
                        UseSubmitBehavior="false" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlGrid" runat="server" Visible="false" Width="600px">
        <asp:ObjectDataSource ID="odsDiasNaoLetivo" runat="server" TypeName="Techne.Lyceum.Net.Matricula.DiasNaoLetivo"
            SelectMethod="Lista" DeleteMethod="Delete">
            <SelectParameters>
                <asp:ControlParameter ControlID="cmbAno" PropertyName="SelectedValue" Name="ano" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <dxwgv:ASPxGridView ID="grdDiasNaoLetivo" runat="server" DataSourceID="odsDiasNaoLetivo"
            KeyFieldName="DIASNAOLETIVOSID" AutoGenerateColumns="false" ClientInstanceName="grdDiasNaoLetivo"
            OnInitNewRow="grdDiasNaoLetivo_InitNewRow" OnStartRowEditing="grdDiasNaoLetivo_StartRowEditing"
            OnRowDeleting="grdDiasNaoLetivo_RowDeleting" OnAfterPerformCallback="grdDiasNaoLetivo_AfterPerformCallback"
            Width="600px">
            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
            <SettingsBehavior ConfirmDelete="true" />
            <Columns>
                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                    <DeleteButton Visible="True" Text="Remover">
                        <Image Url="../img/bt_exclui2.png" />
                    </DeleteButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="DIASNAOLETIVOSID" Name="DIASNAOLETIVOSID"
                    VisibleIndex="1" FieldName="DIASNAOLETIVOSID" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Município" Name="MUNICIPIO" VisibleIndex="1"
                    FieldName="MUNICIPIO">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataDateColumn VisibleIndex="2" Caption="Dia" Name="DIA" FieldName="DIA"
                    Width="100px" Visible="true" ReadOnly="true">
                    <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" Width="150px">
                        <ValidationSettings>
                            <RequiredField IsRequired="true" ErrorText="Campo Obrigatório." />
                        </ValidationSettings>
                    </PropertiesDateEdit>
                    <CellStyle HorizontalAlign="Center" VerticalAlign="Middle">
                    </CellStyle>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataTextColumn Caption="Usuário" Name="USUARIOID" VisibleIndex="3"
                    FieldName="USUARIOID">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Data do Cadastro" Name="DATACADASTRO" VisibleIndex="4"
                    FieldName="DATACADASTRO">
                </dxwgv:GridViewDataTextColumn>
            </Columns>
        </dxwgv:ASPxGridView>
    </asp:Panel>
</asp:Content>
