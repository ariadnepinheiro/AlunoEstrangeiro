<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="AssociacaoPrestadorVeiculoCondutor.aspx.cs" Inherits="Techne.Lyceum.Net.Transporte.AssociacaoPrestadorVeiculoCondutor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Informe os dados para consulta:"
        Width="50%">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="lblPrestador" runat="server" Font-Names="Verdana" Text="Prestador:"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tsePrestador" runat="server" Argument="nome" ArgumentColumns="50"
                        DataType="Number" MaxLength="20" Columns="10" AutoPostBack="true"
                        OnChanged="tsePrestador_Changed" SqlWhere=" ativo = 1" Key="prestadorid" SqlSelect="select distinct prestadorid,cnpj, cpf, nome, telefone from Transporte.prestador"
                        SqlOrder="nome">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="prestadorid" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="50%" />
                            <tweb:TSearchBoxColumn Caption="cnpj" FieldName="cnpj" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="cpf" FieldName="cpf" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
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
        <asp:Label runat="server" ID="lblBloco" Text="Associações" SkinID="BcTitulo" />
    </div>
    <br />
    <asp:Panel runat="server" ID="pnlDados" Visible="false">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label Font-Names="Verdana" ID="lblCondutor" runat="server" Text="Condutor:*"
                        SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseCondutor" runat="server" SqlOrder="nome" SqlSelect="select distinct pc.condutorid,cpf,nome,numerocnh,pc.prestadorid from Transporte.prestadorcondutor pc inner join Transporte.condutor c on c.condutorid = pc.condutorid "
                        SqlWhere=" c.ativo = 1 and pc.ativo = 1 and pc.prestadorid = #tsePrestador# " GridWidth="600px"
                        ArgumentColumns="50" Argument="nome" OnChanged="tseCondutor_Changed" Columns="10"
                        MaxLength="11" Key="cpf" DataType="VarChar">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="condutorid" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="CPF" FieldName="cpf" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Nome" FieldName="nome" Width="50%" />
                            <tweb:TSearchBoxColumn Caption="Número CNH" FieldName="numerocnh" Width="20%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
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
    <br />
    <table>
        <tr>
            <td>
                <asp:Panel ID="pnlGrid" runat="server" Visible="false">
                    <asp:ObjectDataSource ID="odsAssociacoes" TypeName="Techne.Lyceum.Net.Transporte.AssociacaoPrestadorVeiculoCondutor"
                        runat="server" SelectMethod="ListarAssociacao" DeleteMethod="Delete">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="tsePrestador" DefaultValue="" Name="prestadorId"
                                PropertyName="DBValue" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <dxwgv:ASPxGridView ID="grdAssociacoes" runat="server" AutoGenerateColumns="False"
                       ClientInstanceName="grdAssociacoes" DataSourceID="odsAssociacoes" 
                        KeyFieldName="CompositeKey" OnRowDeleting="grdAssociacoes_RowDeleting" 
                        OnAfterPerformCallback="grdAssociacoes_AfterPerformCallback" OnCustomButtonCallback="grdAssociacoes_CustomButtonCallback"
                        OnCustomUnboundColumnData="grdAssociacoes_CustomUnboundColumnData" EnableCallBacks="false">                
                        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                        <SettingsBehavior ConfirmDelete="True" />
                        <Columns>
                            <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">                               
                                <CustomButtons>
                                    <dxwgv:GridViewCommandColumnCustomButton Text="Excluir" ID="btnExcluir" Visibility="AllDataRows">
                                        <Image Url="~/img/bt_exclui2.png" Height="16px" AlternateText="Excluir">
                                        </Image>
                                    </dxwgv:GridViewCommandColumnCustomButton>                                  
                                </CustomButtons>
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataTextColumn Caption="PRESTADORID" FieldName="PRESTADORID" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="CONDUTORID" FieldName="CONDUTORID" VisibleIndex="3" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="VEICULOID" FieldName="VEICULOID" VisibleIndex="4" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Condutor" FieldName="CONDUTOR" VisibleIndex="5">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="CPF" FieldName="CPF" VisibleIndex="6">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Placa" FieldName="PLACA" VisibleIndex="7">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Veículo" FieldName="VEICULO" VisibleIndex="8">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="CompositeKey" FieldName="CompositeKey" UnboundType="String"
                                VisibleIndex="9" Visible="false">
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                    </dxwgv:ASPxGridView>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>
