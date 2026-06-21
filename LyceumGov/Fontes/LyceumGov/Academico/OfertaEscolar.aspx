<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="OfertaEscolar.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.OfertaEscolar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        function abrirPopup() {

            window.setTimeout(function() {
                pucConfirmarTrilha.Show();
            }, 1000);
        }
       

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <div id="dvbloqueio" class="Desbloqueado">
    </div>
    <asp:Panel ID="pnGeral" runat="server" GroupingText="Informe os dados para consulta:"
        Width="623px">
        <asp:HiddenField runat="server" ID="hdnIdControle" />
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label ID="Label4" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlAno" runat="server" DataTextField="ano" DataValueField="ano"
                        Width="70px" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:"></asp:Label>
                </td>
                <td colspan="2">
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
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" SkinID="lblObrigatorio"
                        runat="server" Text="Unidade de Ensino:*"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                        MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo,municipio,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO "
                        SqlWhere=" municipio = #tseMunicipio#" GridWidth="850px" OnChanged="tseUnidadeResponsavel_Changed"
                        SqlOrder="nome_comp">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <div class="divEditBlock" style="width: 850px;">
        <asp:Label runat="server" ID="lblBlocoAluno" Text="Oferta de Trilhas de Aprendizagens"
            SkinID="BcTitulo" />
        <asp:ImageButton ID="btnFinalizar" runat="server" SkinID="BcNovoFinalizar" OnClick="btnFinalizar_Click"
            Visible="false" />
        <asp:ValidationSummary ID="vsItinerario" runat="server" EnableClientScript="true"
            ShowMessageBox="true" ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <br />
    <asp:Panel ID="pnAbaNovo" runat="server" Visible="false" Width="850px">
        <asp:HiddenField ID="hdnIdTrilha" runat="server" />
        <asp:HiddenField ID="hdnCurso" runat="server" />
        <asp:HiddenField ID="hdnTurno" runat="server" />
        <table>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label2" runat="server" Text="Modalidade:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseModalidade" runat="server" Argument="descricao" Caption=""
                        Key="modalidade" SqlOrder="descricao" SqlWhere="modalidade in ('RE1','ED2')"
                        SqlSelect="SELECT modalidade, descricao FROM ly_modalidade_curso" OnChanged="tseModalidade_Changed"
                        GridWidth="500px" ArgumentColumns="50">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="modalidade" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="30%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblItinerario" runat="server" Text="Itinerário:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseItinerario" runat="server" Caption="" SqlSelect="SELECT DISTINCT MODALIDADE,CATEGORIAITINERARIOFORMATIVOID  FROM Pedagogico.VW_ITINERARIO_CURSO "
                        ArgumentColumns="60" Columns="10" MaxLength="20" Key="ITINERARIOFORMATIVOID"
                        Argument="ITINERARIO" DataType="Number" SqlWhere=" ATIVO_ITINERARIO = 1 AND MODALIDADE = #tseModalidade# AND OFERTA_ITINERARIO = 1 "
                        SqlOrder="ITINERARIO" GridWidth="800px">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="ITINERARIOFORMATIVOID" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="ITINERARIO" Width="50%" />
                            <tweb:TSearchBoxColumn Caption="CATEGORIAITINERARIOFORMATIVOID" FieldName="CATEGORIAITINERARIOFORMATIVOID"
                                Width="50%" Visible="false" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="Label1" runat="server" Text="Trilha:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseTrilha" runat="server" Caption="" SqlSelect="SELECT DISTINCT ITINERARIOFORMATIVOID  FROM Pedagogico.VW_ITINERARIO_CURSO "
                        ArgumentColumns="60" Columns="10" MaxLength="20" Key="CURSO" Argument="TRILHA"
                        DataType="VarChar" SqlWhere=" ATIVO_TRILHA = 1 and ITINERARIOFORMATIVOID = #tseItinerario# AND OFERTA_TRILHA = 1"
                        SqlOrder="TRILHA" GridWidth="800px">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="CURSO" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="TRILHA" Width="50%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    <asp:Label ID="lblTurno" runat="server" Text="Turno:*" SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlTurno" runat="server" DataTextField="descricao" DataValueField="turno">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="4" align="right">
                    <asp:Button ID="btnSalvarOferta" runat="server" ValidationGroup="SalvarForm" Text="Salvar"
                        OnClick="btnSalvarOferta_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:ObjectDataSource ID="odsTrilha" runat="server" TypeName="Techne.Lyceum.Net.Academico.OfertaEscolar"
        SelectMethod="Lista">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseUnidadeResponsavel" DefaultValue="" Name="censo"
                PropertyName="DBValue" />
            <asp:ControlParameter ControlID="ddlAno" DefaultValue="" Name="ano" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:Panel ID="pnlGrid" runat="server" Visible="false">
        <dxwgv:ASPxGridView ID="grdTrilha" runat="server" DataSourceID="odsTrilha" KeyFieldName="TRILHAAPRENDIZAGEM_ESCOLAID"
            AutoGenerateColumns="false" ClientInstanceName="grdTrilha" OnInitNewRow="grdTrilha_InitNewRow"
            OnAfterPerformCallback="grdTrilha_AfterPerformCallback" EnableCallBacks="false"
            Width="850px" OnStartRowEditing="grdTrilha_StartRowEditing" OnCustomButtonCallback="grdTrilha_CustomButtonCallback">
            <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
            <SettingsEditing Mode="Inline" />
            <Columns>
                <dxwgv:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" Width="50px">
                    <HeaderCaptionTemplate>
                        <div style="text-align: center" id="dvteste">
                            <input type="image" id="btnNovoGridTrilha" src="../img/bt_novo.png" style="cursor: pointer"
                                title="Novo" onserverclick="HabilitaPnlNovo" runat="server" />
                        </div>
                    </HeaderCaptionTemplate>
                    <CustomButtons>
                        <dxwgv:GridViewCommandColumnCustomButton Text="Excluir" ID="btnExcluir" Visibility="AllDataRows"
                            Image-Url="~/img/bt_exclui2.png" Image-Height="15px" Image-AlternateText="Excluir">
                        </dxwgv:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="TRILHAAPRENDIZAGEM_ESCOLAID" Name="TRILHAAPRENDIZAGEM_ESCOLAID"
                    VisibleIndex="1" FieldName="TRILHAAPRENDIZAGEM_ESCOLAID" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="TRILHAAPRENDIZAGEMID" Name="TRILHAAPRENDIZAGEMID"
                    VisibleIndex="2" FieldName="TRILHAAPRENDIZAGEMID" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="ITINERARIOFORMATIVOID" Name="ITINERARIOFORMATIVOID"
                    VisibleIndex="3" FieldName="ITINERARIOFORMATIVOID" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Turno" Name="TURNO" VisibleIndex="4" FieldName="CODTURNO"
                    Width="300px" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="CURSO" Name="CURSO" VisibleIndex="4" FieldName="CURSO"
                    Width="300px" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Turno" Name="TURNO" VisibleIndex="4" FieldName="TURNO"
                    Width="300px">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Itinerário" Name="ITINERARIO" VisibleIndex="4"
                    FieldName="ITINERARIO" Width="300px">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Trilha" Name="TRILHA" VisibleIndex="5" FieldName="TRILHA"
                    Width="300px">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Usuário" Name="USUARIOID" VisibleIndex="6"
                    FieldName="USUARIOID" Width="100px">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Data de Cadastro" Name="DATACADASTRO" VisibleIndex="7"
                    FieldName="DATACADASTRO" Width="200px">
                </dxwgv:GridViewDataTextColumn>
            </Columns>
        </dxwgv:ASPxGridView>
        <dxpc:ASPxPopupControl ID="pucConfirmarTrilha" ClientInstanceName="pucConfirmarTrilha"
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
                                Confirma a exclusão da oferta da trilha para sua unidade escolar?<br />
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
    </asp:Panel>
</asp:Content>
