<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="HistoricoConfirmacaoTurnosVagas.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.HistoricoConfirmacaoTurnosVagas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel runat="server" ID="pnFiltro" GroupingText="Informe os dados para pesquisar a confirmação de turnos e vagas">
        <div>
            <table>
                <tr>
                    <td style="text-align: right">
                        <asp:Label Font-Names="Verdana" ID="lblAno" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlAno" runat="server" DataTextField="ano" DataValueField="ano"
                            Width="70px" AutoPostBack="True" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged"
                            AppendDataBoundItems="true">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvAno" runat="server" ControlToValidate="ddlAno"
                            ErrorMessage="Ano: Preenchimento obrigatório." InitialValue="" ValidationGroup="SalvarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfvAnoPesquisa" runat="server" ControlToValidate="ddlAno"
                            ErrorMessage="Ano: Preenchimento obrigatório." InitialValue="" ValidationGroup="ConfirmarForm"><img title="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label6" runat="server" Text="Período para exibir quadro de salas:*"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPeriodo" runat="server" DataTextField="Descricao" DataValueField="Periodos"
                            AutoPostBack="True" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label9" runat="server" Text="Lançamento feito por:*" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddlTipoHistorico" runat="server" DataTextField="DESCRICAO"
                            DataValueField="TIPOHISTORICOID" AutoPostBack="True" AppendDataBoundItems="true"
                            OnSelectedIndexChanged="ddlTipoHistorico_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 15%">
                        <asp:Label ID="lblRegional" runat="server" Font-Names="Verdana" Text="Regional:"></asp:Label>
                    </td>
                    <td colspan="2">
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
                    <td style="text-align: right; width: 15%">
                        <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:"></asp:Label>
                    </td>
                    <td colspan="2">
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
                    <td style="text-align: right; width: 15%">
                        <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" SkinID="lblObrigatorio"
                            runat="server" Text="Unidade de Ensino:*"></asp:Label>
                    </td>
                    <td>
                        <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                            MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" GridWidth="850px"
                            OnChanged="tseUnidadeResponsavel_Changed" AutoPostBack="True" SqlOrder="nome_comp">
                            <GridColumns>
                                <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="15%" />
                                <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="45%" />
                                <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                                <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />                               
                            </GridColumns>
                        </tweb:TSearchBox>
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="rfvUnidadeResponsavel" runat="server" ControlToValidate="tseUnidadeResponsavel"
                            ErrorMessage="Unidade de Ensino: Preenchimento obrigatório." InitialValue=""
                            ValidationGroup="ConfirmarForm">
                            <img alt="Preenchimento obrigatório" src="../Images/AlertaMens.gif" />
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:Panel runat="server" ID="pnTurnos" Visible="false" GroupingText="Confirmação de Turnos">
        <br />
        <asp:Label ID="lblMensagemFinalizarTurno" runat="server" SkinID="lblMensagem"></asp:Label>
        <br />
        <table>
            <tr>
                <td>
                    <asp:HiddenField runat="server" ID="hdnPerfil" />
                    <asp:Panel ID="pnGridTurnos" runat="server">
                        <dxwgv:ASPxGridView ID="grdConfTurnos" runat="server" AutoGenerateColumns="False"
                            Visible="False" ClientInstanceName="grdConfTurnos" DataSourceID="odsConfirmacaoTurnos"
                            KeyFieldName="IdAgendaConfTurnoVaga" OnHtmlRowCreated="grdConfTurnos_HtmlRowCreated"
                            SettingsBehavior-AllowDragDrop="false" SettingsBehavior-AllowSort="False">
                            <SettingsPager Mode="ShowAllRecords" />
                            <Columns>
                                <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="IdAgendaConfTurnoVaga"
                                    VisibleIndex="0" ReadOnly="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="AgendaId" FieldName="AgendaId" VisibleIndex="1"
                                    Visible="false" ReadOnly="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Período" FieldName="Periodo" VisibleIndex="2"
                                    ReadOnly="true" Width="50">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Manhã Código" FieldName="ManhaCodigo" VisibleIndex="3"
                                    Visible="false" ReadOnly="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Tarde Código" FieldName="TardeCodigo" VisibleIndex="4"
                                    Visible="false" ReadOnly="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Noite Código" FieldName="NoiteCodigo" VisibleIndex="5"
                                    Visible="false" ReadOnly="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Integral Código" FieldName="IntegralCodigo"
                                    VisibleIndex="6" Visible="false" ReadOnly="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Ampliado Código" FieldName="AmpliadoCodigo"
                                    VisibleIndex="7" Visible="false" ReadOnly="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Manhã Código" FieldName="ManhaNovoCodigo"
                                    VisibleIndex="8" Visible="false" ReadOnly="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Tarde Código" FieldName="TardeNovoCodigo"
                                    VisibleIndex="9" Visible="false" ReadOnly="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Noite Código" FieldName="NoiteNovoCodigo"
                                    VisibleIndex="10" Visible="false" ReadOnly="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Integral Código" FieldName="IntegralNovoCodigo"
                                    VisibleIndex="11" Visible="false" ReadOnly="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Ampliado Código" FieldName="AmpliadoNovoCodigo"
                                    VisibleIndex="12" Visible="false" ReadOnly="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="CodModalidade" FieldName="CodigoModalidade"
                                    VisibleIndex="13" Visible="false" ReadOnly="true" Width="200">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="CodTipo" FieldName="CodigoTipo" VisibleIndex="14"
                                    Visible="false" ReadOnly="true" Width="200">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Modalidade" FieldName="Modalidade" VisibleIndex="15"
                                    Visible="true" ReadOnly="true" Width="200">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Curso" FieldName="Curso" VisibleIndex="16"
                                    Visible="false" ReadOnly="true">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Escolaridade" FieldName="NomeCurso" VisibleIndex="17"
                                    Visible="true" ReadOnly="true" Width="200">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Série" FieldName="Serie" VisibleIndex="18"
                                    ReadOnly="true" Width="50">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Série" FieldName="DescricaoSerie" VisibleIndex="19"
                                    ReadOnly="true" Width="50" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataColumn Caption="Manhã" FieldName="Manha" VisibleIndex="20" Name="Manha"
                                    Width="50">
                                    <DataItemTemplate>
                                        <asp:CheckBox ID="chkManha" runat="server" Checked='<%# Bind("Manha") %>'></asp:CheckBox>
                                    </DataItemTemplate>
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataColumn Caption="Tarde" FieldName="Tarde" VisibleIndex="21" Name="Tarde"
                                    Width="50">
                                    <DataItemTemplate>
                                        <asp:CheckBox ID="chkTarde" runat="server" Checked='<%# Bind("Tarde") %>'></asp:CheckBox>
                                    </DataItemTemplate>
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataColumn Caption="Noite" FieldName="Noite" VisibleIndex="22" Name="Noite"
                                    Width="50">
                                    <DataItemTemplate>
                                        <asp:CheckBox ID="chkNoite" runat="server" Checked='<%# Bind("Noite") %>'></asp:CheckBox>
                                    </DataItemTemplate>
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataColumn Caption="Integral" FieldName="Integral" VisibleIndex="23"
                                    Name="Integral" Width="50">
                                    <DataItemTemplate>
                                        <asp:CheckBox ID="chkIntegral" runat="server" Checked='<%# Bind("Integral") %>'>
                                        </asp:CheckBox>
                                    </DataItemTemplate>
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataColumn Caption="Ampliado" FieldName="Ampliado" VisibleIndex="24"
                                    Name="Ampliado" Width="50">
                                    <DataItemTemplate>
                                        <asp:CheckBox ID="chkAmpliado" runat="server" Checked='<%# Bind("Ampliado")  %>'>
                                        </asp:CheckBox>
                                    </DataItemTemplate>
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataColumn Caption="Justificativa" FieldName="Justificativa" VisibleIndex="25"
                                    Name="Justificativa" Width="200">
                                    <DataItemTemplate>
                                        <asp:TextBox ID="txtJustificativa" runat="server" Width="200" Text='<%# Bind("Justificativa") %>'></asp:TextBox>
                                        <asp:HiddenField ID="hdnValorAntigo" runat="server" Value='<%# Bind("TurnosIniciais") %>' />
                                        <asp:HiddenField ID="hdnValorNovo" runat="server" Value='<%# Bind("Turnos") %>' />
                                        <asp:HiddenField ID="hdnPerfilResponsavel" runat="server" Value='<%# Bind("PerfilResponsavel") %>' />
                                        <asp:HiddenField ID="hdnFinalizado" runat="server" Value='<%# Bind("Finalizado") %>' />
                                        <asp:HiddenField ID="hdnEncerrado" runat="server" Value='<%# Bind("Encerrado") %>' />
                                    </DataItemTemplate>
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataColumn Caption="Manhã" FieldName="ManhaNovo" VisibleIndex="26"
                                    Name="ManhaNovo">
                                    <DataItemTemplate>
                                        <asp:CheckBox ID="chkManhaNovo" runat="server" Checked='<%# Bind("ManhaNovo") %>'>
                                        </asp:CheckBox>
                                    </DataItemTemplate>
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataColumn Caption="Tarde" FieldName="TardeNovo" VisibleIndex="27"
                                    Name="TardeNovo">
                                    <DataItemTemplate>
                                        <asp:CheckBox ID="chkTardeNovo" runat="server" Checked='<%# Bind("TardeNovo") %>'>
                                        </asp:CheckBox>
                                    </DataItemTemplate>
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataColumn Caption="Noite" FieldName="NoiteNovo" VisibleIndex="28"
                                    Name="NoiteNovo">
                                    <DataItemTemplate>
                                        <asp:CheckBox ID="chkNoiteNovo" runat="server" Checked='<%# Bind("NoiteNovo") %>'>
                                        </asp:CheckBox>
                                    </DataItemTemplate>
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataColumn Caption="Integral" FieldName="IntegralNovo" VisibleIndex="29"
                                    Name="IntegralNovo">
                                    <DataItemTemplate>
                                        <asp:CheckBox ID="chkIntegralNovo" runat="server" Checked='<%# Bind("IntegralNovo") %>'>
                                        </asp:CheckBox>
                                    </DataItemTemplate>
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataColumn Caption="Ampliado" FieldName="AmpliadoNovo" VisibleIndex="30"
                                    Name="AmpliadoNovo">
                                    <DataItemTemplate>
                                        <asp:CheckBox ID="chkAmpliadoNovo" runat="server" Checked='<%# Bind("AmpliadoNovo") %>'>
                                        </asp:CheckBox>
                                    </DataItemTemplate>
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataColumn Caption="Justificativa" FieldName="JustificativaNovo" VisibleIndex="31"
                                    Name="JustificativaNovo" Width="200">
                                    <DataItemTemplate>
                                        <asp:TextBox ID="txtJustificativaNovo" runat="server" Width="200" Text='<%# Bind("JustificativaNovo") %>'></asp:TextBox>
                                        <asp:HiddenField ID="hdnValorAntigoNovo" runat="server" Value='<%# Bind("TurnosIniciais") %>' />
                                        <asp:HiddenField ID="hdnValorNovoNovo" runat="server" Value='<%# Bind("TurnosNovo") %>' />
                                    </DataItemTemplate>
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Perfil" FieldName="PerfilResponsavel" Name="PerfilResponsavel"
                                    VisibleIndex="32" UnboundType="String" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Finalizado" FieldName="Finalizado" Name="Finalizado"
                                    VisibleIndex="33" UnboundType="String" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="Encerrado" FieldName="Encerrado" Name="Encerrado"
                                    VisibleIndex="34" UnboundType="String" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                                <dxwgv:GridViewDataTextColumn Caption="TurnosListaInicial" FieldName="TurnosListaInicial"
                                    VisibleIndex="35" Name="TurnosListaInicial" UnboundType="String" Visible="false">
                                </dxwgv:GridViewDataTextColumn>
                            </Columns>
                            <Templates>
                                <TitlePanel>
                                    <table width="100%" border="0">
                                        <tr>
                                            <td colspan="3">
                                                &nbsp; &nbsp; &nbsp;
                                            </td>
                                            <td colspan="6" align="center">
                                                Turnos Matriculas Continuidade
                                            </td>
                                            <td colspan="6" align="center">
                                                Turnos Matriculas Novas
                                            </td>
                                        </tr>
                                    </table>
                                </TitlePanel>
                            </Templates>
                            <Settings ShowFilterRow="False" ShowFilterRowMenu="False" />
                        </dxwgv:ASPxGridView>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <br />
        <asp:Panel ID="pnlAnaliseTurnos" runat="server" Visible="false">
            <table id="tbAnalise" runat="server">
                <tr>
                    <td align="left">
                        <asp:Label runat="server" ID="label50" Text="Análise Turnos SUPED - Validação:" SkinID="lblobrigatorio"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label runat="server" ID="lblAnaliseTurnosGeralSUPED" Visible="false"> </asp:Label>
                        <asp:Label runat="server" ID="lblAnaliseTurnosSUPED0" Visible="false" Text="Periodo: 0"> </asp:Label>
                        <asp:Label runat="server" ID="lblDescAnaliseTurnosSUPEDPeriodo0" Visible="false"> </asp:Label>
                       
                        <br />
                        <asp:Label runat="server" ID="lblAnaliseTurnosSUPED1" Visible="false" Text="Periodo: 1"> </asp:Label>
                        <asp:Label runat="server" ID="lblDescAnaliseTurnosSUPEDPeriodo1" Visible="false"> </asp:Label>
                     
                        <br />
                        <asp:Label runat="server" ID="lblAnaliseTurnosSUPED2" Text="Periodo: 2" Visible="false"> </asp:Label>
                        <asp:Label runat="server" ID="lblDescAnaliseTurnosSUPEDPeriodo2" Visible="false"> </asp:Label>
                                          </td>
                </tr>
                <tr>
                    <td align="left">
                        <br />
                        <asp:Label runat="server" ID="label51" Text="Análise Turnos SUPLAN - Validação:"
                            SkinID="lblobrigatorio"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label runat="server" ID="lblAnaliseTurnosGeralSUPLAN" Visible="false"> </asp:Label>
                        <asp:Label runat="server" ID="lblAnaliseTurnosSUPLAN0" Visible="false" Text="Periodo: 0"> </asp:Label>
                        <asp:Label runat="server" ID="lblDescAnaliseTurnosSUPLANPeriodo0" Visible="false"> </asp:Label>
                      
                        <br />
                        <asp:Label runat="server" ID="lblAnaliseTurnosSUPLAN1" Visible="false" Text="Periodo: 1"> </asp:Label>
                        <asp:Label runat="server" ID="lblDescAnaliseTurnosSUPLANPeriodo1" Visible="false"> </asp:Label>
                      
                        <br />
                        <asp:Label runat="server" ID="lblAnaliseTurnosSUPLAN2" Text="Periodo: 2" Visible="false"> </asp:Label>
                        <asp:Label runat="server" ID="lblDescAnaliseTurnosSUPLANPeriodo2" Visible="false"> </asp:Label>
                      
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <br />
                        <asp:Label runat="server" ID="label52" Text="Análise Turnos DIESP - Validação:" SkinID="lblobrigatorio"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label runat="server" ID="lblAnaliseTurnosGeralDIESP" Visible="false"> </asp:Label>
                        <asp:Label runat="server" ID="lblAnaliseTurnosDIESP0" Visible="false" Text="Periodo: 0"> </asp:Label>
                        <asp:Label runat="server" ID="lblDescAnaliseTurnosDIESPPeriodo0" Visible="false"> </asp:Label>
                      
                        <br />
                        <asp:Label runat="server" ID="lblAnaliseTurnosDIESP1" Visible="false" Text="Periodo: 1"> </asp:Label>
                        <asp:Label runat="server" ID="lblDescAnaliseTurnosDIESPPeriodo1" Visible="false"> </asp:Label>
                       
                        <br />
                        <asp:Label runat="server" ID="lblAnaliseTurnosDIESP2" Text="Periodo: 2" Visible="false"> </asp:Label>
                        <asp:Label runat="server" ID="lblDescAnaliseTurnosDIESPPeriodo2" Visible="false"> </asp:Label>
                       
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagemVagas" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:Panel runat="server" ID="pnGridVagas" GroupingText="Confirmação de Vagas">
        <br />
        <asp:Label ID="lblMensagemFinalizarVagas" runat="server" SkinID="lblMensagem"></asp:Label>
        <br />
        <dxwgv:ASPxGridView ID="gridVagas" Width="90%" runat="server" AutoGenerateColumns="False"
            ClientInstanceName="gridVagas" KeyFieldName="ID" DataSourceID="odsConfVagas"
            SettingsPager-PageSize="200" OnHtmlRowCreated="gridVagas_onHtmlRowCreated" SettingsBehavior-AllowDragDrop="false"
            SettingsBehavior-AllowSort="False">
            <Columns>
                <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="ID" VisibleIndex="1" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Width="3%" Caption="Código" FieldName="IDAgenda" VisibleIndex="2"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Width="3%" Caption="AgendaID Evento" FieldName="AgendaID"
                    VisibleIndex="3" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Width="3%" Caption="Editavel" FieldName="Editavel"
                    VisibleIndex="4" Visible="false">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Width="3%" Caption="Período" FieldName="Periodo" VisibleIndex="7"
                    Visible="true" ReadOnly="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Width="8%" Caption="Modalidade" FieldName="Modalidade"
                    VisibleIndex="8" Visible="true" ReadOnly="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Curso" Width="5%" FieldName="Curso" VisibleIndex="9"
                    Visible="false" ReadOnly="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Curso" Width="5%" FieldName="NomeCurso" VisibleIndex="10"
                    Visible="true" ReadOnly="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Série" Width="3%" FieldName="Serie" VisibleIndex="11"
                    Visible="true" ReadOnly="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Série" Width="4%" FieldName="DescricaoSerie"
                    VisibleIndex="12" Visible="false" ReadOnly="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Cont." Width="4%" FieldName="QuantContSeeduc"
                    VisibleIndex="13" Visible="true" ReadOnly="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Nova" Width="4%" FieldName="QuantNovaSeeduc"
                    VisibleIndex="14" Visible="true" ReadOnly="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Cont." Width="4%" FieldName="QuantCont"
                    VisibleIndex="15" Visible="true" ReadOnly="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Nova" Width="3%" FieldName="QuantNovas"
                    VisibleIndex="16" Visible="true" ReadOnly="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Taxa de Aprovação" Width="5%" FieldName="TaxaAprovacao"
                    VisibleIndex="17" Visible="true" ReadOnly="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Taxa de Reprovação" Width="5%" FieldName="TaxaReprovacao"
                    VisibleIndex="18" Visible="true" ReadOnly="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Justificativa Nova" Width="30%" FieldName="JustificativaNova"
                    VisibleIndex="19" Visible="true" ReadOnly="false">
                    <DataItemTemplate>
                        <asp:TextBox ID="txtJFNova" runat="server" Width="95%" Text='<%# Bind("JustificativaNova") %>'
                            MaxLength="500" Enabled="false"></asp:TextBox>
                    </DataItemTemplate>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Cont." Width="4%" FieldName="QuantContVagasUtilizadas"
                    VisibleIndex="20" Visible="true" ReadOnly="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Nova" Width="4%" FieldName="QuantNovasVagasUtilizadas"
                    VisibleIndex="21" Visible="true" ReadOnly="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Cont." Width="4%" FieldName="QuantContSaldo"
                    VisibleIndex="22" Visible="true" ReadOnly="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Nova" Width="4%" FieldName="QuantNovaSaldo"
                    VisibleIndex="23" Visible="true" ReadOnly="true">
                </dxwgv:GridViewDataTextColumn>
            </Columns>
            <Templates>
                <TitlePanel>
                    <asp:Label ID="lb1" runat="server" Width="22%"></asp:Label>
                    <%-- <asp:Label ID="lb2" runat="server" CssClass="borderBottomNone" BorderStyle="Solid"
                                                    BorderColor="#6ca6ea" Width="17%">&nbsp;&nbsp;&nbsp;&nbsp;Proposta SEEDUC&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Proposta U.E.</asp:Label>--%>
                    <asp:Label ID="lb2" runat="server" CssClass="borderBottomNone" BorderStyle="Solid"
                        BorderColor="#6ca6ea" Width="9%">Proposta SEEDUC</asp:Label>
                    <asp:Label ID="lb3" runat="server" CssClass="borderBottomNone" BorderStyle="Solid"
                        BorderColor="#6ca6ea" Width="7%">Proposta U.E.</asp:Label>
                    <asp:Label ID="lb4" runat="server" Width="41%"></asp:Label>
                    <%-- <asp:Label ID="lb5" runat="server" BorderStyle="Solid" BorderColor="#6ca6ea" Width="18%">&nbsp;&nbsp;&nbsp;&nbsp;Vagas Utilizadas&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Saldo de Vagas</asp:Label>--%>
                    <asp:Label ID="lb5" runat="server" CssClass="borderBottomNone" BorderStyle="Solid"
                        BorderColor="#6ca6ea" Width="8%">Vagas Utilizadas</asp:Label>
                    <asp:Label ID="lb6" runat="server" CssClass="borderBottomNone" BorderStyle="Solid"
                        BorderColor="#6ca6ea" Width="8%">Saldo de Vagas</asp:Label>
                </TitlePanel>
            </Templates>
            <Settings ShowFilterRow="False" ShowFilterRowMenu="False" />
        </dxwgv:ASPxGridView>
        <asp:Panel runat="server" ID="pnTurmas" BorderWidth="0">
            <dxwgv:ASPxGridView ID="gridSalas" runat="server" AutoGenerateColumns="False" ClientInstanceName="gridSalas"
                OnHtmlRowCreated="gridSalas_onHtmlRowCreated" Width="100%" SettingsPager-PageSize="200">
                <Columns>
                    <dxwgv:GridViewDataTextColumn Caption="Salas de Aula / Capacidade" Width="12%" FieldName="SalaCapacidade"
                        VisibleIndex="0" Visible="true" ReadOnly="true">
                    </dxwgv:GridViewDataTextColumn>
                    <dxwgv:GridViewDataColumn Caption="Manhã" Name="Manha" VisibleIndex="1">
                        <DataItemTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="hdnIDManha" runat="server" />
                                        <asp:HiddenField ID="hdnEditavelManha" runat="server" />
                                        <asp:Label ID="lblTurma_M" runat="server" Text="Turma" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblVCM_M" ToolTip="Vagas Continuidade" runat="server" Text="VC" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblVN_M" ToolTip="Vagas Novas" runat="server" Text="VN" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtHistTurmas_M" runat="server" ToolTip="Turma" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVC_M" runat="server" ToolTip="Vagas Continuidade" AutoPostBack="true"
                                            MaxLength="3" Enabled="false" Width="20"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVN_M" runat="server" ToolTip="Vagas Novas" AutoPostBack="true"
                                            MaxLength="3" Enabled="false" Width="20"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn Caption="Tarde" Name="Tarde" VisibleIndex="2">
                        <DataItemTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="hdnIDTarde" runat="server" />
                                        <asp:HiddenField ID="hdnEditavelTarde" runat="server" />
                                        <asp:Label ID="lblTurma_T" runat="server" Text="Turma" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblVCM_T" ToolTip="Vagas Continuidade" runat="server" Text="VC" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblVN_T" ToolTip="Vagas Novas" runat="server" Text="VN" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtHistTurmas_T" runat="server" ToolTip="Turma" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVC_T" runat="server" ToolTip="Vagas Continuidade" AutoPostBack="true"
                                            MaxLength="3" Enabled="false" Width="20"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVN_T" runat="server" ToolTip="Vagas Novas" AutoPostBack="true"
                                            MaxLength="3" Enabled="false" Width="20"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn Caption="Noite" Name="Noite" VisibleIndex="3">
                        <DataItemTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="hdnIDNoite" runat="server" />
                                        <asp:HiddenField ID="hdnEditavelNoite" runat="server" />
                                        <asp:Label ID="lblTurma_N" runat="server" Text="Turma" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblVCM_N" ToolTip="Vagas Continuidade" runat="server" Text="VC" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblVN_N" ToolTip="Vagas Novas" runat="server" Text="VN" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtHistTurmas_N" runat="server" ToolTip="Turma" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVC_N" runat="server" ToolTip="Vagas Continuidade" AutoPostBack="true"
                                            MaxLength="3" Enabled="false" Width="20"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVN_N" runat="server" ToolTip="Vagas Novas" AutoPostBack="true"
                                            MaxLength="3" Enabled="false" Width="20"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn Caption="Ampliado" Name="Ampliado" VisibleIndex="4">
                        <DataItemTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="hdnIDAmpliado" runat="server" />
                                        <asp:HiddenField ID="hdnEditavelAmpliado" runat="server" />
                                        <asp:Label ID="lblTurma_A" runat="server" Text="Turma" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblVCM_A" ToolTip="Vagas Continuidade" runat="server" Text="VC" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblVN_A" ToolTip="Vagas Novas" runat="server" Text="VN" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtHistTurmas_A" runat="server" ToolTip="Turma" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVC_A" runat="server" ToolTip="Vagas Continuidade" AutoPostBack="true"
                                            MaxLength="3" Enabled="false" Width="20"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVN_A" runat="server" ToolTip="Vagas Novas" AutoPostBack="true"
                                            MaxLength="3" Enabled="false" Width="20"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn Caption="Integral" Name="Integral" VisibleIndex="5">
                        <DataItemTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:HiddenField ID="hdnIDIntegral" runat="server" />
                                        <asp:HiddenField ID="hdnEditavelIntegral" runat="server" />
                                        <asp:Label ID="lblTurma_I" runat="server" Text="Turma" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblVCM_I" ToolTip="Vagas Continuidade" runat="server" Text="VC" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblVN_I" ToolTip="Vagas Novas" runat="server" Text="VN" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtHistTurmas_I" runat="server" ToolTip="Turma" Enabled="false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVC_I" runat="server" ToolTip="Vagas Continuidade" AutoPostBack="true"
                                            MaxLength="3" Enabled="false" Width="20"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVN_I" runat="server" ToolTip="Vagas Novas" AutoPostBack="true"
                                            MaxLength="3" Enabled="false" Width="20"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataColumn>
                </Columns>
                <Settings ShowFilterRow="False" ShowFilterRowMenu="False" />
            </dxwgv:ASPxGridView>
            <asp:Label ID="lblMensagemVagasBottom" runat="server" SkinID="lblMensagem"></asp:Label>
            <asp:Panel ID="pnlAnaliseVagas" runat="server" Visible="false">
                <table id="Table6" runat="server">
                    <tr>
                        <td align="left">
                            <asp:Label runat="server" ID="label11" Text="Análise Vagas SUPED - Validação:" SkinID="lblobrigatorio"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label runat="server" ID="lblAnaliseVagasGeralSUPED" Visible="false"> </asp:Label>
                            <asp:Label runat="server" ID="lblAnaliseVagasSUPED0" Visible="false" Text="Periodo: 0"> </asp:Label>
                            <asp:Label runat="server" ID="lblDescAnaliseVagasSUPEDPeriodo0" Visible="false"> </asp:Label>
                            <asp:DropDownList ID="ddlResultadoVagasSUPEDPeriodo0" Visible="false" runat="server"
                                DataTextField="DESCRICAO" DataValueField="DESCRICAO" AppendDataBoundItems="true">
                            </asp:DropDownList>
                            <br />
                            <asp:Label runat="server" ID="lblAnaliseVagasSUPED1" Visible="false" Text="Periodo: 1"> </asp:Label>
                            <asp:Label runat="server" ID="lblDescAnaliseVagasSUPEDPeriodo1" Visible="false"> </asp:Label>
                            <asp:DropDownList ID="ddlResultadoVagasSUPEDPeriodo1" Visible="false" runat="server"
                                DataTextField="DESCRICAO" DataValueField="DESCRICAO" AppendDataBoundItems="true">
                            </asp:DropDownList>
                            <br />
                            <asp:Label runat="server" ID="lblAnaliseVagasSUPED2" Text="Periodo: 2" Visible="false"> </asp:Label>
                            <asp:Label runat="server" ID="lblDescAnaliseVagasSUPEDPeriodo2" Visible="false"> </asp:Label>
                            <asp:DropDownList ID="ddlResultadoVagasSUPEDPeriodo2" Visible="false" runat="server"
                                DataTextField="DESCRICAO" DataValueField="DESCRICAO" AppendDataBoundItems="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <br />
                            <asp:Label runat="server" ID="label12" Text="Análise Vagas SUPLAN - Validação:" SkinID="lblobrigatorio"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label runat="server" ID="lblAnaliseVagasGeralSUPLAN" Visible="false"> </asp:Label>
                            <asp:Label runat="server" ID="lblAnaliseVagasSUPLAN0" Visible="false" Text="Periodo: 0"> </asp:Label>
                            <asp:Label runat="server" ID="lblDescAnaliseVagasSUPLANPeriodo0" Visible="false"> </asp:Label>
                            <asp:DropDownList ID="ddlResultadoVagasSUPLANPeriodo0" Visible="false" runat="server"
                                DataTextField="DESCRICAO" DataValueField="DESCRICAO" AppendDataBoundItems="true">
                            </asp:DropDownList>
                            <br />
                            <asp:Label runat="server" ID="lblAnaliseVagasSUPLAN1" Visible="false" Text="Periodo: 1"> </asp:Label>
                            <asp:Label runat="server" ID="lblDescAnaliseVagasSUPLANPeriodo1" Visible="false"> </asp:Label>
                            <asp:DropDownList ID="ddlResultadoVagasSUPLANPeriodo1" Visible="false" runat="server"
                                DataTextField="DESCRICAO" DataValueField="DESCRICAO" AppendDataBoundItems="true">
                            </asp:DropDownList>
                            <br />
                            <asp:Label runat="server" ID="lblAnaliseVagasSUPLAN2" Text="Periodo: 2" Visible="false"> </asp:Label>
                            <asp:Label runat="server" ID="lblDescAnaliseVagasSUPLANPeriodo2" Visible="false"> </asp:Label>
                            <asp:DropDownList ID="ddlResultadoVagasSUPLANPeriodo2" Visible="false" runat="server"
                                DataTextField="DESCRICAO" DataValueField="DESCRICAO" AppendDataBoundItems="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <br />
                            <asp:Label runat="server" ID="label13" Text="Análise Vagas DIESP - Validação:" SkinID="lblobrigatorio"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label runat="server" ID="lblAnaliseVagasGeralDIESP" Visible="false"> </asp:Label>
                            <asp:Label runat="server" ID="lblAnaliseVagasDIESP0" Visible="false" Text="Periodo: 0"> </asp:Label>
                            <asp:Label runat="server" ID="lblDescAnaliseVagasDIESPPeriodo0" Visible="false"> </asp:Label>
                            <asp:DropDownList ID="ddlResultadoVagasDIESPPeriodo0" Visible="false" runat="server"
                                DataTextField="DESCRICAO" DataValueField="DESCRICAO" AppendDataBoundItems="true">
                            </asp:DropDownList>
                            <br />
                            <asp:Label runat="server" ID="lblAnaliseVagasDIESP1" Visible="false" Text="Periodo: 1"> </asp:Label>
                            <asp:Label runat="server" ID="lblDescAnaliseVagasDIESPPeriodo1" Visible="false"> </asp:Label>
                            <asp:DropDownList ID="ddlResultadoVagasDIESPPeriodo1" Visible="false" runat="server"
                                DataTextField="DESCRICAO" DataValueField="DESCRICAO" AppendDataBoundItems="true">
                            </asp:DropDownList>
                            <br />
                            <asp:Label runat="server" ID="lblAnaliseVagasDIESP2" Text="Periodo: 2" Visible="false"> </asp:Label>
                            <asp:Label runat="server" ID="lblDescAnaliseVagasDIESPPeriodo2" Visible="false"> </asp:Label>
                            <asp:DropDownList ID="ddlResultadoVagasDIESPPeriodo2" Visible="false" runat="server"
                                DataTextField="DESCRICAO" DataValueField="DESCRICAO" AppendDataBoundItems="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
    <%-- Ods --%>
    <asp:ObjectDataSource ID="odsConfirmacaoTurnos" TypeName="Techne.Lyceum.Net.Academico.HistoricoConfirmacaoTurnosVagas"
        runat="server" SelectMethod="ListarTurnos">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseUnidadeResponsavel" DefaultValue="" Name="unidadeEns"
                PropertyName="DBValue" />
            <asp:ControlParameter ControlID="ddlAno" DefaultValue="" Name="ano" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="ddlTipoHistorico" DefaultValue="" Name="tipoHistorico"
                PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="odsConfVagas" TypeName="Techne.Lyceum.Net.Academico.HistoricoConfirmacaoTurnosVagas"
        runat="server" SelectMethod="ListarVagas"></asp:ObjectDataSource>
</asp:Content>
