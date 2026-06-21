<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="LiberacaoTurma.aspx.cs" Inherits="Techne.Lyceum.Net.AvaliacaoExterna.LiberacaoTurma" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        function Confirma() {
            var selectResposta = $("[id$='rblTipoOperacao']").find(":checked").val();
            var operacao ='';

            if (selectResposta == "A") {
                operacao = "aprovação";
            } else {
            operacao = "reprovação";
            }


            if (confirm("Confirma " + operacao + " para todas as solicitações?")) {
                return true;
            }
            return false;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:ObjectDataSource ID="odsLiberacaoTurma" TypeName="Techne.Lyceum.Net.AvaliacaoExterna.LiberacaoTurma"
        runat="server" SelectMethod="Lista" UpdateMethod="Update">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseAvaliacao" PropertyName="DBValue" Name="avaliacaoId" />
            <asp:ControlParameter ControlID="ddlPeriodo" PropertyName="SelectedValue" Name="periodo" />
            <asp:ControlParameter ControlID="tseRegional" PropertyName="DBValue" Name="regional" />
            <asp:ControlParameter ControlID="tseMunicipio" PropertyName="DBValue" Name="municipio" />
            <asp:ControlParameter ControlID="tseUnidadeResponsavel" PropertyName="DBValue" Name="unidadeEnsino" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlDefinicao" GroupingText="Informe os dados para pesquisa."
                Width="775px">
                <table>
                    <tr>
                        <td style="text-align: right; width: 20%">
                            <asp:Label ID="lblAvaliacao" runat="server" SkinID="lblObrigatorio" Font-Names="Verdana"
                                Text="Avaliação:*"></asp:Label>
                        </td>
                        <td>
                            <tweb:TSearchBox ID="tseAvaliacao" runat="server" SqlSelect="select distinct AVALIACAOID, DESCRICAO, ANO from AvaliacaoExterna.AVALIACAO"
                                AutoPostBack="True" Columns="10" ArgumentColumns="50" DataType="Number" OnChanged="tseAvaliacao_Changed">
                                <GridColumns>
                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="AVALIACAOID" Width="20%" />
                                    <tweb:TSearchBoxColumn Caption="Ano" FieldName="ANO" Width="20%" />
                                    <tweb:TSearchBoxColumn Caption="Nome" FieldName="DESCRICAO" Width="60%" />
                                </GridColumns>
                            </tweb:TSearchBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 20%">
                            <asp:Label ID="lblPeriodo" runat="server" SkinID="lblObrigatorio" Font-Names="Verdana"
                                Text="Período:*"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPeriodo" runat="server" Width="375px" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged">
                                <asp:ListItem Text="" Value="" />
                                <asp:ListItem Text="0:ANUAL" Value="0" />
                                <asp:ListItem Text="1:1º SEMESTRE" Value="1" />
                                <asp:ListItem Text="2:2º SEMESTRE" Value="2" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 20%">
                            <asp:Label ID="lblRegional" runat="server" Font-Names="Verdana" Text="Regional:"></asp:Label>
                        </td>
                        <td>
                            <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                                MaxLength="20" Columns="10" AutoPostBack="True" Caption="" OnChanged="tseRegional_Changed"
                                SqlOrder="regional" Key="id_regional" SqlSelect="SELECT DISTINCT S.id_regional AS id_regional, regional FROM VW_UNIDADE_ENSINO_SITUACAO S INNER JOIN TCE_REGIONAL R ON S.ID_REGIONAL=R.ID_REGIONAL"
                                DataType="Number">
                                <GridColumns>
                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                                    <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                                </GridColumns>
                            </tweb:TSearchBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 20%">
                            <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:"></asp:Label>
                        </td>
                        <td>
                            <tweb:TSearchBox ID="tseMunicipio" runat="server" SqlOrder="nome" SqlSelect=" select distinct codigo, nome, uf_sigla from VW_ZZCRO_UNIDADE_ENSINO u join municipio m on u.municipio = m.CODIGO "
                                SqlWhere="id_regional IS NOT NULL and id_regional = #tseRegional# " GridWidth="600px"
                                ArgumentColumns="50" OnChanged="tseMunicipio_Changed" Columns="10" MaxLength="10">
                                <GridColumns>
                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                    <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="60%" />
                                    <tweb:TSearchBoxColumn Caption="Estado" FieldName="uf_sigla" Width="20%" />
                                </GridColumns>
                            </tweb:TSearchBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 20%">
                            <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" runat="server" Text="Unidade de Ensino:"></asp:Label>
                        </td>
                        <td>
                            <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                                MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" GridWidth="850px"
                                OnChanged="tseUnidadeResponsavel_Changed" AutoPostBack="True" SqlOrder="nome_comp"
                                SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo,municipio,id_regional,ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO "
                                SqlWhere="id_regional IS NOT NULL and id_regional = #tseRegional# and municipio = #tseMunicipio# ">
                                <GridColumns>
                                    <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="15%" />
                                    <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="15%" />
                                    <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="15%" />
                                    <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="15%" />
                                    <tweb:TSearchBoxColumn Caption="Regional" FieldName="id_regional" Width="20%" />
                                    <tweb:TSearchBoxColumn Caption="Município" FieldName="municipio" Width="20%" />
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
            <asp:Panel ID="pnlOperacoes" runat="server" GroupingText="Escolha a opção para todos os pedidos"
                Visible="false" Width="775px">
                <table>
                    <tr>
                        <td>
                            <asp:RadioButtonList ID="rblTipoOperacao" runat="server" RepeatDirection="Horizontal"
                                OnSelectedIndexChanged="rblTipoOperacao_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Text="Aprovar Todos" Value="A"></asp:ListItem>
                                <asp:ListItem Text="Reprovar Todos" Value="R"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pnlData" runat="server" Visible="false">
                    <table>
                        <tr>
                            <td style="text-align: right">
                                <asp:Label ID="lblDtfechamento" runat="server" SkinID="lblObrigatorio" Text="Nova Data Fechamento:* "></asp:Label>
                            </td>
                            <td>
                                <dxe:ASPxDateEdit ID="dtfechamento" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                                    ClientInstanceName="dtfechamento" CalendarProperties-ClearButtonText="Limpar"
                                    CalendarProperties-TodayButtonText="Hoje">
                                    <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                    </CalendarProperties>
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table>
                    <tr>
                        <td align="right">
                            <asp:Button ID="btOperacao" runat="server" Text="Executar" OnClick="btExecutar_Click"
                                OnClientClick="return Confirma();" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <dxwgv:ASPxGridView ID="grdLiberacaoTurma" runat="server" AutoGenerateColumns="False"
                ClientInstanceName="grdLiberacaoTurma" DataSourceID="odsLiberacaoTurma" KeyFieldName="REABERTURATURMAID"
                OnCommandButtonInitialize="grdLiberacaoTurma_CommandButtonInitialize" OnStartRowEditing="grdLiberacaoTurma_StartRowEditing"
                OnCancelRowEditing="grdLiberacaoTurma_CancelRowEditing" OnCustomButtonInitialize="grdLiberacaoTurma_CustomButtonInitialize"
                OnHtmlCommandCellPrepared="grdLiberacaoTurma_HtmlCommandCellPrepared" OnRowUpdating="grdLiberacaoTurma_RowUpdating"
                OnCustomButtonCallback="grdLiberacaoTurma_CustomButtonCallback" Width="80%">
                <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                <SettingsEditing Mode="Inline" />
                <SettingsText EmptyDataRow="Não existem dados." />
                <Columns>
                    <dxwgv:GridViewCommandColumn Name="Botoes" ButtonType="Image" VisibleIndex="0" Width="75px">
                        <ClearFilterButton Text="Limpar" Visible="True">
                            <Image Url="~/img/bt_limpa.png" />
                        </ClearFilterButton>
                        <EditButton Visible="True" Text="Aprovar">
                            <Image Url="../Images/sel.png" />
                        </EditButton>
                        <CustomButtons>
                            <dxwgv:GridViewCommandColumnCustomButton ID="btnReprovar" Text="Reprovar">
                                <Image Url="../App_Themes/Blue/Editors/fcgroupremove.png" />
                            </dxwgv:GridViewCommandColumnCustomButton>
                        </CustomButtons>
                        <UpdateButton Visible="true" Text="Alterar">
                            <Image Url="../img/bt_salvar.png" />
                        </UpdateButton>
                        <CancelButton Visible="true" Text="Cancelar">
                            <Image Url="~/img/bt_cancelar.png" />
                        </CancelButton>
                    </dxwgv:GridViewCommandColumn>
                    <dxwgv:GridViewDataColumn Caption="ID" FieldName="REABERTURATURMAID" VisibleIndex="1"
                        Visible="false" />
                    <dxwgv:GridViewDataColumn Caption="Escola" FieldName="NOME_COMP" VisibleIndex="2"
                        Width="100px" />
                    <dxwgv:GridViewDataColumn Caption="Turma" FieldName="TURMA" VisibleIndex="3" Width="100px" />
                    <dxwgv:GridViewDataColumn Caption="Justificativa" FieldName="JUSTIFICATIVA" VisibleIndex="4"
                        Width="575px" />
                    <dxwgv:GridViewDataColumn Caption="Situação" FieldName="STATUS" VisibleIndex="5"
                        Width="100px" />
                    <dxwgv:GridViewDataColumn Caption="Dt. Análise" FieldName="DATAANALISE" VisibleIndex="6"
                        Width="100px" />
                    <dxwgv:GridViewDataDateColumn VisibleIndex="7" Caption="Nova Dt. Fechamento" Name="DATAFECHAMENTO"
                        FieldName="DATAFECHAMENTO" Width="100px">
                        <PropertiesDateEdit EditFormat="DateTime" DisplayFormatString="dd/MM/yyyy" Width="150px">
                        </PropertiesDateEdit>
                        <DataItemTemplate>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataDateColumn>
                </Columns>
            </dxwgv:ASPxGridView>
            <br />
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <asp:Panel ID="Panel1" CssClass="overlay" runat="server">
                <asp:Panel ID="Panel2" CssClass="loader" runat="server">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/updateProgress.gif" AlternateText="Updating..."
                        Height="48" Width="48" />
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <dxpc:ASPxPopupControl ID="popup" ClientInstanceName="popup" runat="server" Modal="true"
        ShowShadow="false" AllowDragging="false" AllowResize="false" ShowCloseButton="false"
        ShowFooter="false" ShowSizeGrip="False" EnableAnimation="false" Width="300px"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/" HeaderText="Confirma a operação?">
        <HeaderStyle HorizontalAlign="Center" />
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <SizeGripImage Height="12px" Width="12px" />
        <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,14000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <div align="center">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btExecutar2" runat="server" Text="Sim" OnClick="btExecutar_Click"
                                    OnClientClick="popup.Hide(); return true;" />
                            </td>
                            <td>
                                <asp:Button ID="btnNao" runat="server" Text="Não" OnClientClick="popup.Hide();" />
                            </td>
                        </tr>
                    </table>
                </div>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
</asp:Content>
