<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ItinerarioTrilha.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.ItinerarioTrilha" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        function abrirPopup() {

            window.setTimeout(function() {
                pucConfirmarItinerario.Show();
            }, 1000);
        }

        function abrirPopupTrilha() {
            window.setTimeout(function() {
                pucConfirmarTrilha.Show();
            }, 1000);
        }     

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <br />
    <div class="divEditBlock" style="width: 850px;">
        <asp:Label runat="server" ID="lblBlocoAluno" Text="Itinerários Formativos e Trilhas de Aprendizagens"
            SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsItinerario" runat="server" EnableClientScript="true"
            ShowMessageBox="true" ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <dxtc:ASPxPageControl ID="pcItinerarioTrilha" runat="server" ActiveTabIndex="0" Width="800px"
        OnTabClick="pcItinerarioTrilha_TabClick">
        <TabPages>
            <dxtc:TabPage Text="Itinerário Formativo">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl1" runat="server">
                        <asp:Panel ID="pnAbaItinerario" runat="server" Visible="false">
                            <asp:HiddenField ID="hdnIdItinerario" runat="server" />
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Categoria:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlCategoria" runat="server" DataTextField="descricao" DataValueField="CATEGORIAITINERARIOFORMATIVOID">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblItiner" runat="server" Text="Descrição:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtItinerario" runat="server" Width="450px" MaxLength="500"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chkAtivo" Text="Ativo" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="Objetivo:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtObjetivoItinerario" runat="server" Width="450px" MaxLength="500" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                 <td>
                                        <asp:Label ID="Label6" runat="server" Text="Oferta na Pesquisa:"></asp:Label>
                                    </td>
                                 <td>
                                        <asp:CheckBox runat="server" ID="chkOfertaPesquisaItinerario" Text="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="right">
                                        <asp:Button ID="btnSalvarItinerario" runat="server" ValidationGroup="SalvarForm"
                                            Text="Salvar" OnClick="btnSalvarItinerario_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:ObjectDataSource ID="odsItinerario" runat="server" TypeName="Techne.Lyceum.Net.Academico.ItinerarioTrilha"
                            SelectMethod="Lista" DeleteMethod="Delete"></asp:ObjectDataSource>
                        <dxwgv:ASPxGridView ID="grdItinerario" runat="server" DataSourceID="odsItinerario"
                            OnAfterPerformCallback="grdItinerario_AfterPerformCallback" EnableCallBacks="false"
                            KeyFieldName="ITINERARIOFORMATIVOID" AutoGenerateColumns="false" ClientInstanceName="grdItinerario"
                            OnInitNewRow="grdItinerario_InitNewRow" OnStartRowEditing="grdItinerario_StartRowEditing"
                            OnCustomButtonCallback="grdItinerario_CustomButtonCallback" OnRowDeleting="grdItinerario_RowDeleting">
                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsEditing Mode="Inline" />
                            <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                            <SettingsBehavior ConfirmDelete="true" />
                            <Columns>
                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                                    <HeaderCaptionTemplate>
                                        <div style="text-align: center" id="dvteste">
                                            <input type="image" id="btnNovoGridIti" src="../img/bt_novo.png" style="cursor: pointer"
                                                title="Novo" onserverclick="HabilitaPnlNovo" runat="server" />
                                        </div>
                                    </HeaderCaptionTemplate>
                                    <CustomButtons>
                                        <dxwgv:GridViewCommandColumnCustomButton Text="Editar" ID="btnEditar" Visibility="AllDataRows"
                                            Image-Url="~/img/bt_editar.png" Image-Height="15px" Image-AlternateText="Editar">
                                        </dxwgv:GridViewCommandColumnCustomButton>
                                        <dxwgv:GridViewCommandColumnCustomButton Text="Excluir" ID="btnExcluir" Visibility="AllDataRows"
                                            Image-Url="~/img/bt_exclui2.png" Image-Height="15px" Image-AlternateText="Excluir">
                                        </dxwgv:GridViewCommandColumnCustomButton>
                                    </CustomButtons>
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="ITINERARIOFORMATIVOID"
                                    Visible="false" Width="700px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="CATEGORIAITINERARIOFORMATIVOID"
                                    Visible="false" Width="700px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Categoria*" Name="CATEGORIA" VisibleIndex="2"
                                    FieldName="CATEGORIA" Width="400px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Descrição*" Name="Descricao" VisibleIndex="3"
                                    FieldName="DESCRICAO" Width="600px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Objetivo" Name="Objetivo" VisibleIndex="4" FieldName="OBJETIVO"
                                    Width="400px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataCheckColumn Caption="Oferta na Pesquisa?" FieldName="OFERTA" VisibleIndex="5"
                                    Width="120px">
                                    <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                                        ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                                    </PropertiesCheckEdit>
                                </dxwgv:GridViewDataCheckColumn>
                                <dxwgv:GridViewDataCheckColumn Caption="Ativo?*" FieldName="ATIVO" VisibleIndex="5"
                                    Width="120px">
                                    <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                                        ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                                    </PropertiesCheckEdit>
                                </dxwgv:GridViewDataCheckColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
            <dxtc:TabPage Text="Trilha de Aprendizagem">
                <ContentCollection>
                    <dxw:ContentControl ID="ContentControl2" runat="server">
                        <asp:Panel ID="pnlAbaTrilha" runat="server" Visible="false">
                            <asp:HiddenField ID="hdnIdTrilha" runat="server" />
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblItinerario" runat="server" Text="Itinerário:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <tweb:TSearchBox ID="tseItinerario" runat="server" Caption="" SqlSelect="SELECT  FROM [Pedagogico].[ITINERARIOFORMATIVO] "
                                            ArgumentColumns="60" Columns="10" MaxLength="20" Key="ITINERARIOFORMATIVOID"
                                            Argument="DESCRICAO" DataType="Number" SqlWhere=" ATIVO = 1" SqlOrder="DESCRICAO"
                                            GridWidth="800px">
                                            <GridColumns>
                                                <tweb:TSearchBoxColumn Caption="Código" FieldName="ITINERARIOFORMATIVOID" Width="10%" />
                                                <tweb:TSearchBoxColumn Caption="Descrição" FieldName="DESCRICAO" Width="50%" />
                                            </GridColumns>
                                        </tweb:TSearchBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Descrição:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTrilha" runat="server" Width="450px"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chkTrilhaAtiva" Text="Ativa" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" runat="server" Text="Tipo:*" SkinID="lblObrigatorio"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlTipo" runat="server">
                                            <asp:ListItem Text="Aprofundamento" Value="APROFUNDAMENTO"> </asp:ListItem>
                                            <asp:ListItem Text="Profissionalizante" Value="PROFISSIONALIZANTE"> </asp:ListItem>
                                            <asp:ListItem Text="Selecione" Value="" Selected="True"> </asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Text="Objetivo:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtObjetivoTrilha" runat="server" Width="450px" MaxLength="500" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                 <tr>
                                 <td>
                                        <asp:Label ID="Label7" runat="server" Text="Oferta na Pesquisa:"></asp:Label>
                                    </td>
                                 <td>
                                        <asp:CheckBox runat="server" ID="chkOfertaPesquisaTrilha" Text="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align="right">
                                        <asp:Button ID="btnSalvarTrilha" runat="server" ValidationGroup="SalvarForm" Text="Salvar"
                                            OnClick="btnSalvarTrilha_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:ObjectDataSource ID="odsTrilha" runat="server" TypeName="Techne.Lyceum.Net.Academico.ItinerarioTrilha"
                            SelectMethod="ListaTrilha" DeleteMethod="DeleteTrilha">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="tseItinerario" DefaultValue="" Name="itinerario"
                                    PropertyName="DBValue" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <dxwgv:ASPxGridView ID="grdTrilha" runat="server" DataSourceID="odsTrilha" KeyFieldName="TRILHAAPRENDIZAGEMID"
                            AutoGenerateColumns="false" ClientInstanceName="grdTrilha" OnInitNewRow="grdTrilha_InitNewRow"
                            OnAfterPerformCallback="grdTrilha_AfterPerformCallback" EnableCallBacks="false"
                            OnStartRowEditing="grdTrilha_StartRowEditing" OnCustomButtonCallback="grdTrilha_CustomButtonCallback"
                            OnRowDeleting="grdTrilha_RowDeleting">
                            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                            <SettingsEditing Mode="Inline" />
                            <Columns>
                                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                                    <HeaderCaptionTemplate>
                                        <div style="text-align: center" id="dvteste">
                                            <input type="image" id="btnNovoGridTrilha" src="../img/bt_novo.png" style="cursor: pointer"
                                                title="Novo" onserverclick="HabilitaPnlNovaTrilha" runat="server" />
                                        </div>
                                    </HeaderCaptionTemplate>
                                    <CustomButtons>
                                        <dxwgv:GridViewCommandColumnCustomButton Text="Editar" ID="btnEditarTrilha" Visibility="AllDataRows"
                                            Image-Url="~/img/bt_editar.png" Image-Height="15px" Image-AlternateText="Editar">
                                        </dxwgv:GridViewCommandColumnCustomButton>
                                        <dxwgv:GridViewCommandColumnCustomButton Text="Excluir" ID="btnExcluirTrilha" Visibility="AllDataRows"
                                            Image-Url="~/img/bt_exclui2.png" Image-Height="15px" Image-AlternateText="Excluir">
                                        </dxwgv:GridViewCommandColumnCustomButton>
                                    </CustomButtons>
                                </dxwgv:GridViewCommandColumn>
                                <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="TRILHAAPRENDIZAGEMID"
                                    Visible="false" Width="700px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="ID" Name="ID" VisibleIndex="1" FieldName="ITINERARIOFORMATIVOID"
                                    Visible="false" Width="700px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Itinerário*" Name="Descricao" VisibleIndex="2"
                                    FieldName="ITINERARIO" Width="400px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Descrição*" Name="Descricao" VisibleIndex="3"
                                    FieldName="DESCRICAO" Width="400px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Tipo*" Name="Tipo" VisibleIndex="4" FieldName="TIPO"
                                    Width="400px">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Objetivo" Name="Objetivo" VisibleIndex="5" FieldName="OBJETIVO"
                                    Width="600px" >
                                </dxwgv:GridViewDataTextColumn>
                                 <dxwgv:GridViewDataCheckColumn Caption="Oferta na Pesquisa?" FieldName="OFERTA" VisibleIndex="6"
                                    Width="120px">
                                    <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                                        ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                                    </PropertiesCheckEdit>
                                </dxwgv:GridViewDataCheckColumn>
                                <dxwgv:GridViewDataCheckColumn Caption="Ativo?*" FieldName="ATIVO" VisibleIndex="6"
                                    Width="120px">
                                    <PropertiesCheckEdit DisplayTextChecked="Sim" DisplayTextUnchecked="Não" ValueChecked="1"
                                        ValueType="System.String" ValueUnchecked="0" DisplayTextUndefined="">
                                    </PropertiesCheckEdit>
                                </dxwgv:GridViewDataCheckColumn>
                            </Columns>
                        </dxwgv:ASPxGridView>
                    </dxw:ContentControl>
                </ContentCollection>
            </dxtc:TabPage>
        </TabPages>
    </dxtc:ASPxPageControl>
    <dxpc:ASPxPopupControl ID="pucConfirmarItinerario" ClientInstanceName="pucConfirmarItinerario"
        runat="server" Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false"
        ShowCloseButton="false" ShowFooter="false" ShowHeader="false" ShowSizeGrip="False"
        EnableAnimation="false" Width="300px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <table>
                    <tr align="center">
                        <td>
                            Confirma a exclusão do Itinerário Formativo?<br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:Button ID="btnSim" runat="server" Text="Sim" OnClick="btnSim_Click" />
                            <asp:Button ID="btnNao" runat="server" Text="Não" OnClick="btnNao_Click" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
    <dxpc:ASPxPopupControl ID="pucConfirmarTrilha" ClientInstanceName="pucConfirmarTrilha"
        runat="server" Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false"
        ShowCloseButton="false" ShowFooter="false" ShowHeader="false" ShowSizeGrip="False"
        EnableAnimation="false" Width="300px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,14000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <table>
                    <tr align="center">
                        <td>
                            Confirma a exclusão da Thilha de Aprendizagem?<br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <asp:Button ID="btnSimTrilha" runat="server" Text="Sim" OnClick="btnSimTrilha_Click" />
                            <asp:Button ID="btnNaoTrilha" runat="server" Text="Não" OnClick="btnNaoTrilha_Click" />
                        </td>
                    </tr>
                </table>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
