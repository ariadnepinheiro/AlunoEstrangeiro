<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    SmartNavigation="true" MaintainScrollPositionOnPostback="true" CodeBehind="AvaliacaoNecessidadeEspecial.aspx.cs"
    Inherits="Techne.Lyceum.Net.Academico.AvaliacaoNecessidadeEspecial" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function OnEndCallBack(source) {
            var lblMensagem = document.getElementById("<%=lblMensagem.ClientID %>");
            if (grdSolHabilitacao.cpMessage != null)
                lblMensagem.innerHTML = grdSolHabilitacao.cpMessage;
        }
        function Bloqueio() {
            var divBloqueio = document.getElementById("dvbloqueioMatricula");
            divBloqueio.className = "Bloqueado";
        }
        function blocTexto(campo, qtde) {
            var quant = qtde;
            var valor = $.trim($(campo).val());
            var total = valor.length;

            if (total > quant) {
                $(campo).val(valor.substr(0, quant));
            }
        }
        $(document).ready(function() {
            $('#<%=txtJustificativaCuidador.ClientID %>').bind('keyup', function() { blocTexto(this, 200); });
            $('#<%=txtJustificativaLedor.ClientID %>').bind('keyup', function() { blocTexto(this, 200); });
            $('#<%=txtJustificativaInterprete.ClientID %>').bind('keyup', function() { blocTexto(this, 200); });
            $('#<%=txtJustificativaPAPEE.ClientID %>').bind('keyup', function() { blocTexto(this, 200); });
            $('#<%=txtJustificativaSalaRecurso.ClientID %>').bind('keyup', function() { blocTexto(this, 200); });
        });
       
    </script>

    <div id="dvbloqueioMatricula" class="Desbloqueado">
    </div>
    <asp:Panel runat="server" ID="pnlDefinicao" GroupingText="Informe os dados para pesquisa."
        Width="90%">
        <table>
            <tr>
                <td style="text-align: right; width: 15%">
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
                <td style="text-align: right; width: 15%">
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
                <td style="text-align: right; width: 15%">
                    <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" SkinID="lblObrigatorio"
                        runat="server" Text="Unidade de Ensino:*"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                        MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" GridWidth="850px"
                        OnChanged="tseUnidadeResponsavel_Changed" AutoPostBack="True" SqlOrder="nome_comp"
                        SqlWhere=" u.id_regional IS NOT NULL and u.id_regional = #tseRegional# and u.municipio = #tseMunicipio# "
                        SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,nucleo, u.municipio, u.id_regional,ua_atual,ua_antiga, r.regional from VW_UNIDADE_ENSINO_SITUACAO u left join TCE_REGIONAL r on u.ID_REGIONAL = r.ID_REGIONAL ">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                            <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="30%" />
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="30%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 15%">
                    <asp:Label Font-Names="Verdana" ID="lblSituacao" runat="server" Text="Situação da Avaliação:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSituacao" runat="server" AutoPostBack="true">
                        <asp:ListItem Text="Avaliado" Value="Avaliado"> </asp:ListItem>
                        <asp:ListItem Text="Pendente" Value="Pendente"> </asp:ListItem>
                        <asp:ListItem Text="Todos" Value="" Selected="True"> </asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:Panel ID="pnlAvaliacao" runat="server" Visible="false">
        <asp:ObjectDataSource ID="odsAvaliacao" TypeName="Techne.Lyceum.Net.Academico.AvaliacaoNecessidadeEspecial"
            runat="server" SelectMethod="ListaAvaliacao">
            <SelectParameters>
                <asp:ControlParameter ControlID="tseUnidadeResponsavel" Name="unidade_ens" PropertyName="DBValue" />
                <asp:ControlParameter ControlID="ddlSituacao" PropertyName="SelectedValue" Name="situacao" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <br />
        <dxwgv:ASPxGridView ID="grdAvaliacao" runat="server" AutoGenerateColumns="False"
            Width="700" Visible="true" ClientInstanceName="grdAvaliacao" DataSourceID="odsAvaliacao"
            KeyFieldName="ALUNO" OnAfterPerformCallback="grdAvaliacao_AfterPerformCallback"
            OnCustomButtonCallback="grdAvaliacao_CustomButtonCallback" OnCustomButtonInitialize="grdAvaliacao_CustomButtonInitialize"
            EnableCallBacks="false">
            <SettingsBehavior ConfirmDelete="True" />
            <SettingsEditing Mode="Inline" />
            <SettingsText EmptyDataRow="Não existem dados." />
            <Columns>
                <dxwgv:GridViewCommandColumn VisibleIndex="4" ButtonType="Link" Width="50px" Caption="Avaliar"
                    Name="Avaliar">
                    <CustomButtons>
                        <dxwgv:GridViewCommandColumnCustomButton ID="btnAvaliar" Text="Avaliar" Image-Width="50px"
                            Visibility="AllDataRows">
                        </dxwgv:GridViewCommandColumnCustomButton>
                    </CustomButtons>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0">
                    <ClearFilterButton Text="Limpar" Visible="True">
                        <Image Url="~/img/bt_limpa.png" />
                    </ClearFilterButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataTextColumn Caption="Aluno" FieldName="ALUNO" VisibleIndex="1"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Nome Aluno" FieldName="NOME_COMPL" VisibleIndex="2"
                    Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Necessidade Especial" FieldName="NECESSIDADE_ESPECIAL"
                    VisibleIndex="2" Visible="true">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Avaliação" FieldName="AVALIACAONAPES" VisibleIndex="3"
                    Visible="false">
                </dxwgv:GridViewDataTextColumn>
            </Columns>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        </dxwgv:ASPxGridView>
    </asp:Panel>
    <asp:Panel ID="PnlTiposAvaliacao" GroupingText="Avaliação Necessidade Especial" Visible="false"
        runat="server" Style="font-size: 14px;" Width="50%">
        <asp:HiddenField ID="hdnAvaliacao" runat="server" />
        <table>
            <tr>
                <td style="text-align: right; font-weight: bold; font-size: 11px;">
                    <asp:Label ID="Label1" runat="server" Text="Matrícula do Aluno:"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblAluno" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; font-weight: bold; font-size: 11px;">
                    <asp:Label ID="Label2" runat="server" Text="Nome:"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblNome" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <asp:Label ID="lblMensagemPopup" runat="server" SkinID="lblMensagem"></asp:Label>
        <br />
        <table>
            <tr>
                <td>
                    <!-- Cuidador -->
                    <asp:Panel ID="Panel7" GroupingText="Cuidador" runat="server" Style="font-size: 14px;"
                        Width="431px" Height="150px">
                        <asp:HiddenField ID="hdnCodigoCuidador" runat="server" />
                        <table>
                            <tr>
                                <td style="text-align: right; font-weight: bold; font-size: 11px; width: 100px">
                                    <asp:Label ID="lblNecessitaCuidador" runat="server" Text="Necessita?*"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:RadioButtonList ID="rblNecessitaCuidador" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" OnSelectedIndexChanged="rblNecessitaCuidador_SelectedIndexChanged">
                                        <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Não" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr id="trTipoCuidador" runat="server">
                                <td style="text-align: right; font-weight: bold; font-size: 11px; width: 100px">
                                    <asp:Label ID="lblTipoCuidador" runat="server" Text="Tipo:*"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:RadioButtonList ID="rblTipoCuidador" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" OnSelectedIndexChanged="rblTipoCuidador_SelectedIndexChanged">
                                        <asp:ListItem Text="Permanente" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Transitório" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr id="trVigenciaCuidador" runat="server">
                                <td style="text-align: right; font-weight: bold; font-size: 11px; width: 100px">
                                    <asp:Label ID="lblVigenciaCuidador" runat="server" Text="Vigência:*"></asp:Label>
                                </td>
                                <td>
                                    <dxe:ASPxDateEdit ID="dtInicioCuidador" runat="server" Width="120px" Enabled="true"
                                        EnableDefaultAppearance="true" ClientInstanceName="dtInicioCuidador" CalendarProperties-ClearButtonText="Limpar"
                                        CalendarProperties-TodayButtonText="Hoje">
                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                        </CalendarProperties>
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td>
                                    a
                                </td>
                                <td>
                                    <dxe:ASPxDateEdit ID="dtFimCuidador" runat="server" Width="120px" Enabled="true"
                                        EnableDefaultAppearance="true" ClientInstanceName="dtFimCuidador" CalendarProperties-ClearButtonText="Limpar"
                                        CalendarProperties-TodayButtonText="Hoje">
                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                        </CalendarProperties>
                                    </dxe:ASPxDateEdit>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; font-size: 11px; width: 100px">
                                    <asp:Label ID="lblJustificativaCuidador" runat="server" Text="Justificativa:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtJustificativaCuidador" TextMode="MultiLine" runat="server" Width="294px"
                                        MaxLength="200" Style="overflow: auto"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td>
                    <!-- Ledor -->
                    <asp:Panel ID="Panel1" GroupingText="Ledor" runat="server" Style="font-size: 14px;"
                        Width="431px" Height="150px">
                        <asp:HiddenField ID="hdnCodigoLedor" runat="server" />
                        <table>
                            <tr>
                                <td style="text-align: right; font-weight: bold; font-size: 11px; width: 100px">
                                    <asp:Label ID="lblNecessitaLedor" runat="server" Text="Necessita?*"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:RadioButtonList ID="rblNecessitaLedor" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" OnSelectedIndexChanged="rblNecessitaLedor_SelectedIndexChanged">
                                        <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Não" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr id="trTipoLedor" runat="server">
                                <td style="text-align: right; font-weight: bold; font-size: 11px; width: 100px">
                                    <asp:Label ID="lblTipoLedor" runat="server" Text="Tipo:*"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:RadioButtonList ID="rblTipoLedor" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" OnSelectedIndexChanged="rblTipoLedor_SelectedIndexChanged">
                                        <asp:ListItem Text="Permanente" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Transitório" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr id="trVigenciaLedor" runat="server">
                                <td style="text-align: right; font-weight: bold; font-size: 11px; width: 100px">
                                    <asp:Label ID="lblVigenciaLedor" runat="server" Text="Vigência:*"></asp:Label>
                                </td>
                                <td>
                                    <dxe:ASPxDateEdit ID="dtInicioLedor" runat="server" Width="120px" Enabled="true"
                                        EnableDefaultAppearance="true" ClientInstanceName="dtInicioLedor" CalendarProperties-ClearButtonText="Limpar"
                                        CalendarProperties-TodayButtonText="Hoje">
                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                        </CalendarProperties>
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td>
                                    a
                                </td>
                                <td>
                                    <dxe:ASPxDateEdit ID="dtFimLedor" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                                        ClientInstanceName="dtFimLedor" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                        </CalendarProperties>
                                    </dxe:ASPxDateEdit>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; font-size: 11px; width: 100px">
                                    <asp:Label ID="lblJustificativaLedor" runat="server" Text="Justificativa:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtJustificativaLedor" TextMode="MultiLine" runat="server" Width="294px"
                                        MaxLength="200" Style="overflow: auto"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <!-- Interprete -->
                    <asp:Panel ID="Panel2" GroupingText="Intérprete" runat="server" Style="font-size: 14px;"
                        Width="431px" Height="150px">
                        <asp:HiddenField ID="hdnCodigoInterprete" runat="server" />
                        <table>
                            <tr>
                                <td style="text-align: right; font-weight: bold; font-size: 11px; width: 100px">
                                    <asp:Label ID="lblNecessitaInterprete" runat="server" Text="Necessita?*"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:RadioButtonList ID="rblNecessitaInterprete" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" OnSelectedIndexChanged="rblNecessitaInterprete_SelectedIndexChanged">
                                        <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Não" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr id="trTipoInterprete" runat="server">
                                <td style="text-align: right; font-weight: bold; font-size: 11px; width: 100px">
                                    <asp:Label ID="lblTipoInterprete" runat="server" Text="Tipo:*"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:RadioButtonList ID="rblTipoInterprete" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" OnSelectedIndexChanged="rblTipoInterprete_SelectedIndexChanged">
                                        <asp:ListItem Text="Permanente" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Transitório" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr id="trVigenciaInterprete" runat="server">
                                <td style="text-align: right; font-weight: bold; font-size: 11px; width: 100px">
                                    <asp:Label ID="lblVigenciaInterprete" runat="server" Text="Vigência:*"></asp:Label>
                                </td>
                                <td>
                                    <dxe:ASPxDateEdit ID="dtInicioInterprete" runat="server" Width="120px" Enabled="true"
                                        EnableDefaultAppearance="true" ClientInstanceName="dtInicioInterprete" CalendarProperties-ClearButtonText="Limpar"
                                        CalendarProperties-TodayButtonText="Hoje">
                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                        </CalendarProperties>
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td>
                                    a
                                </td>
                                <td>
                                    <dxe:ASPxDateEdit ID="dtFimInterprete" runat="server" Width="120px" Enabled="true"
                                        EnableDefaultAppearance="true" ClientInstanceName="dtFimInterprete" CalendarProperties-ClearButtonText="Limpar"
                                        CalendarProperties-TodayButtonText="Hoje">
                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                        </CalendarProperties>
                                    </dxe:ASPxDateEdit>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; font-size: 11px; width: 100px">
                                    <asp:Label ID="lblJustificativaInterprete" runat="server" Text="Justificativa:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtJustificativaInterprete" TextMode="MultiLine" runat="server"
                                        Width="294px" MaxLength="200" Style="overflow: auto"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
                <td>
                    <!-- Professor Articulador Pedagógico Educação Especial -->
                    <asp:Panel ID="Panel5" GroupingText="Professor Articulador Pedagógico Educação Especial"
                        runat="server" Style="font-size: 14px;" Width="431px" Height="150px">
                        <asp:HiddenField ID="hdnCodigoPAPEE" runat="server" />
                        <table>
                            <tr>
                                <td style="text-align: right; font-weight: bold; font-size: 11px; width: 100px">
                                    <asp:Label ID="Label9" runat="server" Text="Necessita?*"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:RadioButtonList ID="rblNecessitaPAPEE" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" OnSelectedIndexChanged="rblNecessitaPAPEE_SelectedIndexChanged">
                                        <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Não" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr id="trTipoPAPEE" runat="server">
                                <td style="text-align: right; font-weight: bold; font-size: 11px; width: 100px">
                                    <asp:Label ID="Label10" runat="server" Text="Tipo:*"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:RadioButtonList ID="rblTipoPAPEE" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" OnSelectedIndexChanged="rblTipoPAPEE_SelectedIndexChanged">
                                        <asp:ListItem Text="Permanente" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Transitório" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr id="trVigenciaPAPEE" runat="server">
                                <td style="text-align: right; font-weight: bold; font-size: 11px; width: 100px">
                                    <asp:Label ID="Label11" runat="server" Text="Vigência:*"></asp:Label>
                                </td>
                                <td>
                                    <dxe:ASPxDateEdit ID="dtInicioPAPEE" runat="server" Width="120px" Enabled="true"
                                        EnableDefaultAppearance="true" ClientInstanceName="dtInicioPAPEE" CalendarProperties-ClearButtonText="Limpar"
                                        CalendarProperties-TodayButtonText="Hoje">
                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                        </CalendarProperties>
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td>
                                    a
                                </td>
                                <td>
                                    <dxe:ASPxDateEdit ID="dtFimPAPEE" runat="server" Width="120px" Enabled="true" EnableDefaultAppearance="true"
                                        ClientInstanceName="dtFimPAPEE" CalendarProperties-ClearButtonText="Limpar" CalendarProperties-TodayButtonText="Hoje">
                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                        </CalendarProperties>
                                    </dxe:ASPxDateEdit>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; font-size: 11px; width: 100px">
                                    <asp:Label ID="Label12" runat="server" Text="Justificativa:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtJustificativaPAPEE" TextMode="MultiLine" runat="server" Width="294px"
                                        MaxLength="200" Style="overflow: auto"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <!-- Sala de Recursos -->
                    <asp:Panel ID="Panel3" GroupingText="Sala de Recursos" runat="server" Style="font-size: 14px;">
                        <asp:HiddenField ID="hdnCodigoSalaRecurso" runat="server" />
                        <table>
                            <tr>
                                <td style="text-align: right; font-weight: bold; font-size: 11px; width: 100px">
                                    <asp:Label ID="Label3" runat="server" Text="Necessita?*"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:RadioButtonList ID="rblNecessitaSalaRecurso" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" OnSelectedIndexChanged="rblNecessitaSalaRecurso_SelectedIndexChanged">
                                        <asp:ListItem Text="Sim" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Não" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr id="trTipoSalaRecurso" runat="server">
                                <td style="text-align: right; font-weight: bold; font-size: 11px; width: 100px">
                                    <asp:Label ID="Label4" runat="server" Text="Tipo:*"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:RadioButtonList ID="rblTipoSalaRecurso" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" OnSelectedIndexChanged="rblTipoSalaRecurso_SelectedIndexChanged">
                                        <asp:ListItem Text="Permanente" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Transitório" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr id="trVigenciaSalaRecurso" runat="server">
                                <td style="text-align: right; font-weight: bold; font-size: 11px; width: 100px">
                                    <asp:Label ID="Label5" runat="server" Text="Vigência:*"></asp:Label>
                                </td>
                                <td>
                                    <dxe:ASPxDateEdit ID="dtInicioSalaRecurso" runat="server" Width="120px" Enabled="true"
                                        EnableDefaultAppearance="true" ClientInstanceName="dtInicioSalaRecurso" CalendarProperties-ClearButtonText="Limpar"
                                        CalendarProperties-TodayButtonText="Hoje">
                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                        </CalendarProperties>
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td>
                                    a
                                </td>
                                <td>
                                    <dxe:ASPxDateEdit ID="dtFimSalaRecurso" runat="server" Width="120px" Enabled="true"
                                        EnableDefaultAppearance="true" ClientInstanceName="dtFimSalaRecurso" CalendarProperties-ClearButtonText="Limpar"
                                        CalendarProperties-TodayButtonText="Hoje">
                                        <CalendarProperties ClearButtonText="Limpar" TodayButtonText="Hoje">
                                        </CalendarProperties>
                                    </dxe:ASPxDateEdit>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; font-size: 11px; width: 100px">
                                    <asp:Label ID="Label6" runat="server" Text="Justificativa:*" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtJustificativaSalaRecurso" TextMode="MultiLine" runat="server"
                                        Width="294px" MaxLength="200" Style="overflow: auto"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                       
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td align="center">
                    <asp:Button ID="btnConfirma" runat="server" Text="Confirma" OnClick="btnConfirma_Click"
                        OnClientClick="Bloqueio()" />
                </td>
                <td align="left">
                    <asp:Button ID="btCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
