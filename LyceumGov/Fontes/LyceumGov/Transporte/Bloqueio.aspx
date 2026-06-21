<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="Bloqueio.aspx.cs" Inherits="Techne.Lyceum.Net.Transporte.Bloqueio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Informe os dados para consulta:"
        Width="50%">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblTipoBloqueio" runat="server" Font-Names="Verdana" Text="Tipo de Bloqueio:"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlTipoBloqueio" runat="server" AppendDataBoundItems="true"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlTipoBloqueio_SelectedIndexChanged">
                        <asp:ListItem Text="Selecione" Value="" Selected="True"> </asp:ListItem>
                        <asp:ListItem Text="Prestador" Value="1"> </asp:ListItem>
                        <asp:ListItem Text="Veiculo" Value="2"> </asp:ListItem>
                        <asp:ListItem Text="Condutor" Value="3"> </asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <div class="divEditBlock" style="width: 740px;">
        <asp:ImageButton ID="btnNovo" runat="server" SkinID="BcNovo" OnClick="btnNovo_Click" />
        <asp:ImageButton ID="btnCancel" runat="server" SkinID="BcCancelar" OnClick="btnCancel_Click" />
        <asp:ImageButton ID="btnIncluir" runat="server" SkinID="BcSalvar" OnClick="btnIncluir_Click"
            OnClientClick="Bloqueio()" ValidationGroup="SalvarForm" />
        <asp:Label runat="server" ID="lblBloco" Text="Bloqueio" SkinID="BcTitulo" />
    </div>
    <br />
    <asp:Panel runat="server" ID="pnlTipoBloqueio" Visible="false">
        <asp:Panel ID="pnlCondutor" runat="server" Visible="false">
            <table>
                <tr>
                    <td style="text-align: right; ">
                        <asp:Label Font-Names="Verdana" ID="lblCondutor" runat="server" Text="Condutor:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseCondutor" runat="server" SqlOrder="nome" SqlSelect="select distinct c.condutorid, cpf, nome, numerocnh from Transporte.condutor c "
                            SqlWhere=" ativo = 1 " GridWidth="600px" ArgumentColumns="50" Argument="nome"
                            OnChanged="tseCondutor_Changed" Columns="10" MaxLength="11" Key="cpf" DataType="VarChar">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="condutorid" Width="10%" />
                                <tweb:TSearchBoxColumn Caption="CPF" FieldName="cpf" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="50%" />
                                <tweb:TSearchBoxColumn Caption="Número CNH" FieldName="numerocnh" Width="20%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlVeiculo" runat="server" Visible="false">
            <table>
                <tr>
                    <td style="text-align: right; ">
                        <asp:Label Font-Names="Verdana" ID="lblVeiculo" runat="server" Text="Veículo:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseVeiculo" runat="server" Argument="nome" ArgumentColumns="50"
                            MaxLength="20" Columns="10" AutoPostBack="True" Caption="" OnChanged="tseVeiculo_Changed"
                            SqlWhere=" ativo = 1" Key="placa" SqlSelect="select distinct veiculoid,placa,nome,anolicenciamento from Transporte.veiculo"
                            SqlOrder="nome" DataType="VarChar">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="veiculoid" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Placa" FieldName="placa" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="40%" />
                                <tweb:TSearchBoxColumn Caption="Ano do Licenciamento" FieldName="anolicenciamento"
                                    Width="20%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlPrestador" runat="server" Visible="false">
            <table>
                <tr>
                    <td style="text-align: right; ">
                        <asp:Label Font-Names="Verdana" ID="Label1" runat="server" Text="Prestador:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tsePrestador" runat="server" SqlOrder="nome" SqlSelect="select distinct prestadorid,cpf,cnpj,nome from transporte.prestador"
                            SqlWhere=" ativo = 1 " GridWidth="600px" ArgumentColumns="50" Argument="nome"
                            OnChanged="tsePrestador_Changed" Columns="10" MaxLength="11" Key="prestadorid"
                            DataType="Number">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="prestadorid" Width="10%" />
                                <tweb:TSearchBoxColumn Caption="CPF" FieldName="cpf" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cnpj" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="50%" />
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
    <br />
    <asp:Panel runat="server" ID="pnlDadosBloqueio" Visible="false">
        <table>
            <tr>
                <td style="text-align: right; ">
                    <asp:Label Font-Names="Verdana" ID="lblMotivo" runat="server" Text="Motivo:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMotivoBloqueio" runat="server" DataTextField="DESCRICAO"
                        DataValueField="MOTIVOBLOQUEIOID" AppendDataBoundItems="true">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="lblObservacao" runat="server" Text="Observação:"></asp:Label>
                </td>
                <td colspan="4">
                    <asp:TextBox ID="txtObservacao" runat="server" TextMode="MultiLine" Width="400px"
                        Height="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="Label5" runat="server" Text="Data Bloqueio:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <dxe:ASPxDateEdit ID="dtInicio" runat="server" Width="127px" Enabled="true" EnableDefaultAppearance="true"
                        ClientInstanceName="dtInicio" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje"
                        Height="18px">
                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                        </CalendarProperties>
                    </dxe:ASPxDateEdit>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <table>
        <tr>
            <td>
                <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                    <asp:Panel ID="pnlGrdPrestador" runat="server" Visible="false">
                        <asp:ObjectDataSource ID="odsPrestador" runat="server" TypeName="Techne.Lyceum.Net.Transporte.Bloqueio"
                            SelectMethod="ListarPrestador" UpdateMethod="UpdatePrestador" DeleteMethod="DeletePrestador">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddlTipoBloqueio" PropertyName="SelectedValue" Name="tipo" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <dxwgv:ASPxGridView ID="grdPrestador" runat="server" DataSourceID="odsPrestador"
                            KeyFieldName="PRESTADORBLOQUEIOID" AutoGenerateColumns="false" ClientInstanceName="grdPrestador"
                            OnInitNewRow="grdPrestador_InitNewRow" OnStartRowEditing="grdPrestador_StartRowEditing"
                            OnRowUpdating="grdPrestador_RowUpdating" OnRowDeleting="grdPrestador_RowDeleting"
                            Width="1000px" OnCustomColumnDisplayText="grdPrestador_CustomColumnDisplayText"
                            OnCellEditorInitialize="grdPrestador_CellEditorInitialize" OnCommandButtonInitialize="grdPrestador_CommandButtonInitialize">
                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsEditing Mode="InLine" />
                            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                            <SettingsBehavior ConfirmDelete="true" />
                            <Columns>
                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
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
                                <dxwgv:GridViewDataTextColumn Caption="PRESTADORBLOQUEIOID" Name="PRESTADORBLOQUEIOID"
                                    VisibleIndex="1" FieldName="PRESTADORBLOQUEIOID" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="CNPJ/CPF" Name="CNPJCPF" VisibleIndex="2"
                                    FieldName="CNPJCPF" Width="100px" ReadOnly="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Nome" Name="NOME" ReadOnly="true" VisibleIndex="3"
                                    FieldName="NOME" Width="200px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Motivo" Name="MOTIVOBLOQUEIO" VisibleIndex="5"
                                    FieldName="MOTIVOBLOQUEIO" Width="100px" ReadOnly="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Observação" Name="OBSERVACAO" VisibleIndex="6"
                                    FieldName="OBSERVACAO" Width="100px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Bloqueio*" FieldName="DATABLOQUEIO" VisibleIndex="7"
                                    Width="100px" ReadOnly="true">
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Desbloqueio*" FieldName="DATADESBLOQUEIO"
                                    VisibleIndex="8" Width="100px">
                                    <PropertiesDateEdit Width="100px" EditFormat="Date">
                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                        </CalendarProperties>
                                    </PropertiesDateEdit>
                                </dxwgv:GridViewDataDateColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                    </asp:Panel>
                    <asp:Panel ID="pnlGrdCondutor" runat="server" Visible="false">
                        <asp:ObjectDataSource ID="odsCondutor" runat="server" TypeName="Techne.Lyceum.Net.Transporte.Bloqueio"
                            SelectMethod="ListarCondutor" UpdateMethod="Update" DeleteMethod="Delete">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddlTipoBloqueio" PropertyName="SelectedValue" Name="tipo" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <dxwgv:ASPxGridView ID="grdCondutor" runat="server" DataSourceID="odsCondutor" KeyFieldName="CONDUTORBLOQUEIOID"
                            AutoGenerateColumns="false" ClientInstanceName="grdCondutor" OnInitNewRow="grdCondutor_InitNewRow"
                            OnStartRowEditing="grdCondutor_StartRowEditing" OnRowUpdating="grdCondutor_RowUpdating"
                            OnRowDeleting="grdCondutor_RowDeleting" Width="1000px" OnCustomColumnDisplayText="grdCondutor_CustomColumnDisplayText"
                            OnCellEditorInitialize="grdCondutor_CellEditorInitialize" OnCommandButtonInitialize="grdCondutor_CommandButtonInitialize">
                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsEditing Mode="InLine" />
                            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                            <SettingsBehavior ConfirmDelete="true" />
                            <Columns>
                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
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
                                <dxwgv:GridViewDataTextColumn Caption="CONDUTORBLOQUEIOID" Name="CONDUTORBLOQUEIOID"
                                    VisibleIndex="1" FieldName="CONDUTORID" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="CPF" Name="CPF" VisibleIndex="2" FieldName="CPF"
                                    Width="100px" ReadOnly="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Nome" Name="NOME" ReadOnly="true" VisibleIndex="3"
                                    FieldName="NOME" Width="200px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Motivo" Name="MOTIVOBLOQUEIO" VisibleIndex="5"
                                    FieldName="MOTIVOBLOQUEIO" Width="100px" ReadOnly="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Observação" Name="OBSERVACAO" VisibleIndex="6"
                                    FieldName="OBSERVACAO" Width="100px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Bloqueio*" FieldName="DATABLOQUEIO" VisibleIndex="7"
                                    Width="100px" ReadOnly="true">
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Desbloqueio*" FieldName="DATADESBLOQUEIO"
                                    VisibleIndex="8" Width="100px">
                                    <PropertiesDateEdit Width="100px" EditFormat="Date">
                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                        </CalendarProperties>
                                    </PropertiesDateEdit>
                                </dxwgv:GridViewDataDateColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                    </asp:Panel>
                    <asp:Panel ID="pnlGrdVeiculo" runat="server" Visible="false">
                        <asp:ObjectDataSource ID="odsVeiculo" runat="server" TypeName="Techne.Lyceum.Net.Transporte.Bloqueio"
                            SelectMethod="ListarVeiculo" UpdateMethod="UpdateVeiculo" DeleteMethod="DeleteVeiculo">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="ddlTipoBloqueio" PropertyName="SelectedValue" Name="tipo" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <dxwgv:ASPxGridView ID="grdVeiculo" runat="server" DataSourceID="odsVeiculo" KeyFieldName="VEICULOBLOQUEIOID"
                            AutoGenerateColumns="false" ClientInstanceName="grdVeiculo" OnInitNewRow="grdVeiculo_InitNewRow"
                            OnStartRowEditing="grdVeiculo_StartRowEditing" OnRowUpdating="grdVeiculo_RowUpdating"
                            OnRowDeleting="grdVeiculo_RowDeleting" OnCellEditorInitialize="grdVeiculo_CellEditorInitialize"
                            Width="1000px" OnCommandButtonInitialize="grdVeiculo_CommandButtonInitialize">
                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsEditing Mode="InLine" />
                            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                            <SettingsBehavior ConfirmDelete="true" />
                            <Columns>
                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">                                   
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
                                <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="VEICULOBLOQUEIOID"
                                    Visible="false" Width="700px">
                                    <PropertiesTextEdit MaxLength="200">
                                    </PropertiesTextEdit>
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Nome" Name="NOME" VisibleIndex="2" FieldName="NOME"
                                    Width="100px" ReadOnly="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Placa" Name="PLACA" VisibleIndex="4" FieldName="PLACA"
                                    Width="100px" ReadOnly="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Tipo" Name="TIPO" VisibleIndex="4" FieldName="TIPO"
                                    Width="100px" ReadOnly="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Motivo" Name="MOTIVOBLOQUEIO" VisibleIndex="5"
                                    FieldName="MOTIVOBLOQUEIO" Width="100px" ReadOnly="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Observação" Name="OBSERVACAO" VisibleIndex="6"
                                    FieldName="OBSERVACAO" Width="100px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Bloqueio*" FieldName="DATABLOQUEIO" VisibleIndex="7"
                                    Width="100px" ReadOnly="true">
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataDateColumn Caption="Data Desbloqueio*" FieldName="DATADESBLOQUEIO"
                                    VisibleIndex="8" Width="100px">
                                    <PropertiesDateEdit Width="100px" EditFormat="Date">
                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                        </CalendarProperties>
                                    </PropertiesDateEdit>
                                </dxwgv:GridViewDataDateColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                    </asp:Panel>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
