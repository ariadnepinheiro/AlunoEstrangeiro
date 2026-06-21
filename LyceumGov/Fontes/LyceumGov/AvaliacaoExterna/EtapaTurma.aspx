<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="EtapaTurma.aspx.cs" Inherits="Techne.Lyceum.Net.AvaliacaoExterna.EtapaTurma" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:ObjectDataSource ID="odsEtapaTurma" TypeName="Techne.Lyceum.Net.AvaliacaoExterna.EtapaTurma"
        runat="server" SelectMethod="Lista" UpdateMethod="Update">
        <SelectParameters>
            <asp:Parameter DbType="Int32" Name="periodo" />
            <asp:Parameter DbType="String" Name="unidadeEnsino" />
            <asp:Parameter DbType="Int32" Name="provaId" />
            <asp:Parameter DbType="Int32" Name="etapaId" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsProva" TypeName="Techne.Lyceum.Net.AvaliacaoExterna.EtapaTurma"
        runat="server" SelectMethod="ListaProva">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseAvaliacao" PropertyName="DBValue" Name="avaliacaoId" />
            <asp:ControlParameter ControlID="tseUnidadeResponsavel" PropertyName="DBValue" Name="censo" />            
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsEtapa" TypeName="Techne.Lyceum.Net.AvaliacaoExterna.EtapaTurma"
        runat="server" SelectMethod="ListaEtapa">
        <SelectParameters>
            <asp:ControlParameter ControlID="ddlProva" PropertyName="SelectedValue" Name="provaId" />
            <asp:ControlParameter ControlID="tseUnidadeResponsavel" PropertyName="DBValue" Name="censo" /> 
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
            <asp:Panel runat="server" ID="pnlDefinicao" GroupingText="Informe os dados para pesquisa."
                Width="775px">
                <table>
                    <tr>
                        <td style="text-align: right; width: 20%">
                            <asp:Label ID="lblAvaliacao" runat="server" SkinID="lblObrigatorio" Font-Names="Verdana"
                                Text="Avaliação:"></asp:Label>
                        </td>
                        <td>
                            <tweb:TSearchBox ID="tseAvaliacao" runat="server" SqlSelect="select distinct AVALIACAOID, DESCRICAO, ANO from AvaliacaoExterna.AVALIACAO"
                                AutoPostBack="True" Columns="10" ArgumentColumns="50" DataType="Number" OnChanged="tseAvaliacao_Changed" SqlWhere="ATIVO = 1"
                                OnPreRender="tseAvaliacao_PreRender">
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
                                Text="Período:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPeriodo" runat="server" Width="375px" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged" OnPreRender="ddlPeriodo_PreRender">
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
                            <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" SkinID="lblObrigatorio"
                                runat="server" Text="Unidade de Ensino:"></asp:Label>
                        </td>
                        <td>
                            <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                                MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" GridWidth="850px"
                                OnChanged="tseUnidadeResponsavel_Changed" OnPreRender="tseUnidadeResponsavel_PreRender" AutoPostBack="True" SqlOrder="nome_comp"
                                SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo,municipio,id_regional, ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO "
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
                    <tr>
                        <td style="width: 70px; text-align: right;">
                            <asp:Label ID="lblProva" runat="server" SkinID="lblObrigatorio" Text="Prova:* "></asp:Label>
                        </td>
                        <td style="width: 305px;">
                            <asp:DropDownList ID="ddlProva" runat="server" DataSourceID="odsProva" DataTextField="DESCRICAO" OnPreRender="ddlProva_PreRender"
                                OnSelectedIndexChanged="ddlProva_SelectedIndexChanged" DataValueField="PROVAID" Width="300px" AutoPostBack="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 70px; text-align: right;">
                            <asp:Label ID="lblEtapa" runat="server" SkinID="lblObrigatorio" Text="Etapa:* "></asp:Label>
                        </td>
                        <td style="width: 305px;">
                            <asp:DropDownList ID="ddlEtapa" runat="server" DataSourceID="odsEtapa" DataTextField="DESCRICAO" OnPreRender="ddlEtapa_PreRender"
                                OnSelectedIndexChanged="ddlEtapa_SelectedIndexChanged" DataValueField="ETAPAID" Width="300px" AutoPostBack="true" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <br />
            <dxwgv:ASPxGridView ID="grdEtapaTurma" runat="server" AutoGenerateColumns="False"
                ClientInstanceName="grdEtapaTurma" DataSourceID="odsEtapaTurma" KeyFieldName="TURMA"
                OnStartRowEditing="grdEtapaTurma_StartRowEditing" OnInitNewRow="grdEtapaTurma_InitNewRow"
                OnCustomButtonCallback="grdEtapaTurma_CustomButtonCallback" OnCustomButtonInitialize="grdEtapaTurma_CustomButtonInitialize"
                OnCancelRowEditing="grdEtapaTurma_CancelRowEditing" OnRowUpdating="grdEtapaTurma_RowUpdating"
                OnHtmlDataCellPrepared="grdEtapaTurma_HtmlDataCellPrepared" Width="775px">
                <Settings ShowFilterRow="true" ShowFilterRowMenu="true" />
                <SettingsEditing Mode="Inline" />
                <Columns>
                    <dxwgv:GridViewCommandColumn Name="Botoes" ButtonType="Image" VisibleIndex="0" Width="75px">
                        <ClearFilterButton Text="Limpar" Visible="True">
                            <Image Url="~/img/bt_limpa.png" />
                        </ClearFilterButton>
                        <CustomButtons>
                            <dxwgv:GridViewCommandColumnCustomButton ID="btnTranscricaoResposta" Text="Cadastrar Transcrição das Respostas"
                                Image-AlternateText="Cadastrar Transcrição das Respostas">
                                <Image Url="../img/bt_editar.png" />
                            </dxwgv:GridViewCommandColumnCustomButton>
                            <dxwgv:GridViewCommandColumnCustomButton ID="btnSolicitacaoReabertura" Text="Solicitar Reabertura"
                                Image-AlternateText="Solicitar Reabertura">
                                <Image Url="../img/ico_altera.gif" />
                            </dxwgv:GridViewCommandColumnCustomButton>
                            <dxwgv:GridViewCommandColumnCustomButton ID="btnVisualizarTranscricao" Text="Visualizar Transcrição"
                                Image-AlternateText="Visualizar Transcrição">
                                <Image Url="../img/bt_busca.png" />
                            </dxwgv:GridViewCommandColumnCustomButton>
                        </CustomButtons>
                        <UpdateButton Visible="true" Text="Alterar">
                            <Image Url="../img/bt_salvar.png" />
                        </UpdateButton>
                        <CancelButton Visible="true" Text="Cancelar">
                            <Image Url="~/img/bt_cancelar.png" />
                        </CancelButton>
                    </dxwgv:GridViewCommandColumn>
                    <dxwgv:GridViewDataColumn Caption="ID" FieldName="ETAPAID" VisibleIndex="1" Visible="false" />
                    <dxwgv:GridViewDataColumn Caption="TRANSCRICAOTURMAID" FieldName="TRANSCRICAOTURMAID" VisibleIndex="2"
                        Visible="false" />
                    <dxwgv:GridViewDataColumn Caption="Turma" FieldName="TURMA" VisibleIndex="3" Width="100px" />
                    <dxwgv:GridViewDataColumn Caption="Ano" FieldName="ANO" VisibleIndex="4" Width="50px" />
                    <dxwgv:GridViewDataColumn Caption="Período" FieldName="SEMESTRE" VisibleIndex="5"
                        Width="50px" />
                    <dxwgv:GridViewDataColumn Caption="Escolaridade" FieldName="NOME" VisibleIndex="6"
                        Width="100px" />
                    <dxwgv:GridViewDataColumn Caption="Turno" FieldName="DESCRICAO" VisibleIndex="7"
                        Width="50px" />
                    <dxwgv:GridViewDataColumn Caption="Ano de Escolaridade" FieldName="SERIE" VisibleIndex="8"
                        Width="50px" />
                    <dxwgv:GridViewDataColumn Caption="Unidade de Ensino" FieldName="NOME_COMP" VisibleIndex="9"
                        Width="200px" />
                    <dxwgv:GridViewDataColumn Caption="Ini Transcr." FieldName="INICIOTRANSCRICAO" VisibleIndex="10"
                        Width="50px" />
                    <dxwgv:GridViewDataColumn Caption="Fim Transcr." FieldName="FIMTRANSCRICAO" VisibleIndex="11"
                        Width="50px" />
                    <dxwgv:GridViewDataColumn Caption="Status Transcr." FieldName="STATUSTRANSCRICAO"
                        VisibleIndex="12" Width="50px" />
                    <dxwgv:GridViewDataColumn Caption="Status Reabertura" FieldName="STATUS" VisibleIndex="13"
                        Width="50px" />
                    <dxwgv:GridViewDataColumn Caption="Nova Dt. Fechamento" FieldName="DATAFECHAMENTO"
                        VisibleIndex="14" Width="50px" />
                    <dxwgv:GridViewDataTextColumn Caption="Justificativa" Name="JUSTIFICATIVA" VisibleIndex="15"
                        Width="700px" Visible="false">
                        <EditItemTemplate>
                            <dxe:ASPxTextBox ID="txtJustificativa" runat="server" MaxLength="100" Width="700px">
                            </dxe:ASPxTextBox>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataTextColumn>
                </Columns>
            </dxwgv:ASPxGridView>
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
</asp:Content>
