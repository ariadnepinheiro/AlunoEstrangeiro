<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="FrequenciaGLP.aspx.cs" Inherits="Techne.Lyceum.Net.Basico.FrequenciaGLP" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript">
        //https://www.devexpress.com/Support/Center/Question/Details/T341395/aspxpopupcontrol-version-9-x-modal-background-problem-on-firefox-44
        function SetZIndexForLightBox(control) {
            if (typeof control.name == 'undefined') return;
            $("#" + control.name + "_TCFix-1").css("z-index", 12000);
        }

        function grdTurma_EndCallback(s, e) {
            $("#<%= UpdateProgress1.ClientID %>").css("display", "none");
            pucDetalhe.Show();
            SetZIndexForLightBox(pucDetalhe);
        }
        
        function grdFrequencia_OnCustomButtonClick(s, e) {
            switch (e.buttonID) {
                
                <% if (PossuiPerfilAlteracao || EhDiaPermitidoParaAlteracao || EhPrivilegiado) { %>
                case "btnEditar":
                    grdFrequencia.GetRowValues(e.visibleIndex, 'ANO_FILTRADO;MES_FILTRADO;NUM_FUNC;UNIDADE_ENS;CH_MENSAL;ID_CARGAHNAOTRABMES;CHNAOTRABALHADAMES;MATRICULA;NOME', function(values) {
                        $("#<%= hidAno.ClientID %>").val(values[0]);
                        $("#<%= hidMes.ClientID %>").val(values[1]);
                        $("#<%= hidNumFunc.ClientID %>").val(values[2]);
                        $("#<%= hidUnidadeEns.ClientID %>").val(values[3]);
                        $("#<%= hidCHMensal.ClientID %>").val(values[4]);
                        $("#<%= hidIDCargaHNaoTrabMes.ClientID %>").val(values[5]);
                        $("#<%= hidMatricula.ClientID %>").val(values[7]);
                        $("#<%= hidNome.ClientID %>").val(values[8]);
                        

                        $("#<%= txtCHNaoTrab.ClientID %>").val(values[6]);
                        $("#<%= lblCHMensal.ClientID %>").html(values[4]);
                        $("#<%= lblCHMensalTotal.ClientID %>").html(values[4] - values[6]);
                        $("#<%= lblNomeMatricula.ClientID %>").html(values[7] + " - " + values[8]);
                    });
                    pucCHNaoTrab.Show();
                    SetZIndexForLightBox(pucCHNaoTrab);
                    break;
                <% } %>

                case "btnVisualizar":
                    grdFrequencia.GetRowValues(e.visibleIndex, 'ANO_FILTRADO;MES_FILTRADO;NUM_FUNC;UNIDADE_ENS', function(values) {
                        $("#<%= UpdateProgress1.ClientID %>").css("display", "block");
                        $("#<%= hidAno_Detalhe.ClientID %>").val(values[0]);
                        $("#<%= hidMes_Detalhe.ClientID %>").val(values[1]);
                        $("#<%= hidNumFunc_Detalhe.ClientID %>").val(values[2]);
                        $("#<%= hidFaculdade_Detalhe.ClientID %>").val(values[3]);
                        grdTurma.Refresh();
                    });
                    break;
            }
            
            return false;
        }

        <% if (PossuiPerfilAlteracao || EhDiaPermitidoParaAlteracao || EhPrivilegiado) { %>
        function salvarPucCHNaoTrab() {

            if ($("#<%= hidAno.ClientID %>").val() == "" ||
                $("#<%= hidAno.ClientID %>").val() == null ||
                isNaN($("#<%= hidAno.ClientID %>").val())) {
                $("#<%= lblMensagemCHNaoTrab.ClientID %>").text("Erro ao carregar o popup de edição de CH não trabalhada. ano inválido.");
                return false;
            }

            if ($("#<%= hidMes.ClientID %>").val() == "" ||
                $("#<%= hidMes.ClientID %>").val() == null ||
                isNaN($("#<%= hidMes.ClientID %>").val())) {
                $("#<%= lblMensagemCHNaoTrab.ClientID %>").text("Erro ao carregar o popup de edição de CH não trabalhada. mês inválido.");
                return false;
            }

            if ($("#<%= hidNumFunc.ClientID %>").val() == "" ||
                $("#<%= hidNumFunc.ClientID %>").val() == null ||
                isNaN($("#<%= hidNumFunc.ClientID %>").val())) {
                $("#<%= lblMensagemCHNaoTrab.ClientID %>").text("Erro ao carregar o popup de edição de CH não trabalhada. hidNumFunc inválido.");
                return false;
            }

            if ($("#<%= hidUnidadeEns.ClientID %>").val() == "" ||
                $("#<%= hidUnidadeEns.ClientID %>").val() == null ||
                isNaN($("#<%= hidUnidadeEns.ClientID %>").val())) {
                $("#<%= lblMensagemCHNaoTrab.ClientID %>").text("Erro ao carregar o popup de edição de CH não trabalhada. hidUnidadeEns inválido.");
                return false;
            }            
            
            if ($("#<%= hidCHMensal.ClientID %>").val() == "" ||
                $("#<%= hidCHMensal.ClientID %>").val() == null ||
                isNaN($("#<%= hidCHMensal.ClientID %>").val())) {
                $("#<%= lblMensagemCHNaoTrab.ClientID %>").text("Erro ao carregar o popup de edição de CH não trabalhada. hidCHMensal inválido.");
                return false;
            }
            
            if ($("#<%= txtCHNaoTrab.ClientID %>").val() == "" ||
                $("#<%= txtCHNaoTrab.ClientID %>").val() == null ||
                isNaN($("#<%= txtCHNaoTrab.ClientID %>").val())) {
                $("#<%= lblMensagemCHNaoTrab.ClientID %>").text("É necessário preencher o campo \"CH Não Trabalhada Mês\" com um número.");
                return false;
            }

            if (parseInt($("#<%= txtCHNaoTrab.ClientID %>").val()) > parseInt($("#<%= hidCHMensal.ClientID %>").val())) {
                $("#<%= lblMensagemCHNaoTrab.ClientID %>").text("A \"CH Não Trabalhada Mês\" não pode ser superior a \"CH Mensal\".");
                return false;
            }

            if (parseInt($("#<%= txtCHNaoTrab.ClientID %>").val()) < 0) {
                $("#<%= lblMensagemCHNaoTrab.ClientID %>").text("A \"CH Não Trabalhada Mês\" não pode ser negativa.");
                return false;
            }

//            if (!$("#<%= chkTermoResponsabilidade.ClientID %>").prop("checked")) {
//                $("#<%= lblMensagemCHNaoTrab.ClientID %>").text("É necessário concordar com o termo de responsabilidade marcando a opção acima.");
//                return false;
//            }

            $("#<%= lblMensagemCHNaoTrab.ClientID %>").text("");
            pucCHNaoTrab.Hide();
        }

        function fecharPucCHNaoTrab() {
            $("#<%= hidAno.ClientID %>").val("");
            $("#<%= hidMes.ClientID %>").val("");
            $("#<%= hidNumFunc.ClientID %>").val("");
            $("#<%= hidUnidadeEns.ClientID %>").val("");
            $("#<%= hidCHMensal.ClientID %>").val("");
            $("#<%= hidIDCargaHNaoTrabMes.ClientID %>").val("");
            $("#<%= hidMatricula.ClientID %>").val("");
            $("#<%= hidNome.ClientID %>").val("");
            $("#<%= txtCHNaoTrab.ClientID %>").val("");
            $("#<%= chkTermoResponsabilidade.ClientID %>").prop("checked", false);
            $("#<%= lblMensagemCHNaoTrab.ClientID %>").text("");
            pucCHNaoTrab.Hide();
            return false;
        }

        function alteraCHNaoTrab() {
            $("#<%= lblCHMensalTotal.ClientID %>").html($("#<%= hidCHMensal.ClientID %>").val() - $("#<%= txtCHNaoTrab.ClientID %>").val());
        }
        <% } %>
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script language="javascript">
        /*
        DevExpress Workaround:
        https://www.devexpress.com/Support/Center/Question/Details/T555320/a-popup-is-not-shown-or-is-shown-at-an-incorrect-position-in-chrome-61-and-newer-versions
        A popup is not shown or is shown at an incorrect position in Chrome 61 and newer versions
                    
        Nas versões recentes do Chrome, o popup não estava aparecendo centralizado na tela visível do usuário. Ao invés disso, estava aparecendo centralizado como
        se o scrollTop estivesse zerado.
                    
        As funções abaixo corrigem o scrollTop para o que o componente do DevExpress realmente espera receber.
        */
        window.onload = function() {
            function _aspxGetDocumentScrollTop() {
                return document.documentElement.scrollTop || document.body.scrollTop
            }
            if (window._aspxGetDocumentScrollTop) {
                window._aspxGetDocumentScrollTop = _aspxGetDocumentScrollTop;
            }
            /* Begin -> Correct ScrollLeft */
            function _aspxGetDocumentScrollLeft() {
                return document.documentElement.scrollLeft || document.body.scrollLeft
            }
            if (window._aspxGetDocumentScrollLeft) {
                window._aspxGetDocumentScrollLeft = _aspxGetDocumentScrollLeft;
            }
            /* End -> Correct ScrollLeft */
        }
    </script>

    <table>
        <tr>
            <td>
                <asp:Panel runat="server" ID="pnlFiltro" GroupingText="Escolha o tipo de busca que deseja"
                    Width="800px">
                    <table>
                        <tr>
                            <td style="text-align: right; width: 90px">
                                <asp:Label ID="lblAno" runat="server" Font-Names="Verdana" SkinID="lblObrigatorio"
                                    Text="Ano:*"></asp:Label>
                            </td>
                            <td style="width: 100px">
                                <asp:DropDownList ID="ddlAno" runat="server" DataSourceID="odsAno" DataTextField="ANO"
                                    OnSelectedIndexChanged="ddlAno_SelectedIndexChanged" DataValueField="ANO" AppendDataBoundItems="true"
                                    AutoPostBack="true">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: right; width: 90px">
                                <asp:Label ID="lblMes" runat="server" Font-Names="Verdana" SkinID="lblObrigatorio"
                                    Text="Mês:*"></asp:Label>
                            </td>
                            <td style="width: 100px">
                                <asp:DropDownList ID="ddlMes" runat="server" DataSourceID="odsMes" AppendDataBoundItems="true"
                                    OnSelectedIndexChanged="ddlMes_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="text-align: right; width: 90px">
                                <asp:Label ID="Label1" runat="server" Font-Names="Verdana" SkinID="lblObrigatorio"
                                    Text="Período Lançamento:*"></asp:Label>
                            </td>
                            <td style="width: 100px">
                                <asp:DropDownList ID="ddlPeriodoLancamento" runat="server" DataValueField="PERIODO"
                                    DataTextField="PERIODO" AppendDataBoundItems="true" OnSelectedIndexChanged="ddlPeriodoLancamento_SelectedIndexChanged"
                                    AutoPostBack="true">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td style="text-align: right; width: 90px">
                                <asp:Label ID="lblRegional" runat="server" Font-Names="Verdana" Text="Regional:*"
                                    SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                                    MaxLength="20" Columns="10" Caption="" AutoPostBack="true" SqlOrder="regional"
                                    Key="id_regional" SqlSelect="SELECT DISTINCT S.id_regional AS id_regional, regional FROM VW_UNIDADE_ENSINO_SITUACAO S INNER JOIN TCE_REGIONAL R ON S.ID_REGIONAL=R.ID_REGIONAL"
                                    DataType="Number" OnChanged="tseRegional_Changed">
                                    <GridColumns>
                                        <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                                    </GridColumns>
                                </tweb:TSearchBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; width: 90px">
                                <asp:Label Font-Names="Verdana" ID="lblMunicipio" runat="server" Text="Município:*"
                                    SkinID="lblObrigatorio"></asp:Label>
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
                        
                            <td style="text-align: right; width: 90px">
                                <asp:Label ID="Label2" runat="server" Font-Names="Verdana" Text="Situação de Funcionamento:*"
                                    SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <tweb:TSearchBox ID="tseSituaçaoFuncionamento" runat="server" Argument="descr"
                                    ArgumentColumns="50" MaxLength="50" Columns="10" Caption="" AutoPostBack="true"
                                    SqlOrder="descr" Key="item" SqlSelect="SELECT DISTINCT item, descr  from itemtabela" SqlWhere=" tab = 'SitFuncionamentoUE'"
                                    DataType="VarChar" OnChanged="tseSituacaoFuncionamento_Changed">
                                    <GridColumns>
                                        <tweb:TSearchBoxColumn Caption="Código" FieldName="item" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="Situação" FieldName="descr" Width="80%" />
                                    </GridColumns>
                                </tweb:TSearchBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; width: 90px">
                                <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" runat="server" Text="Unidade de Ensino:*"
                                    SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                                    MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" GridWidth="850px"
                                    OnChanged="tseUnidadeResponsavel_Changed" AutoPostBack="TRUE" SqlOrder="nome_comp"
                                    SqlSelect=" SELECT unidade_ens, nome_comp, setor, cgc, situacao,municipio,nome, regional,id_regional, ua_atual,ua_antiga from VW_UNIDADE_ENSINO_SITUACAO_REGIONAL "
                                    SqlWhere="SITUACAO_FUNCIONAMENTO = #tseSituaçaoFuncionamento# and (id_regional IS NOT NULL and id_regional = #tseRegional#) and (municipio is not null and municipio = #tseMunicipio#) ">
                                    <GridColumns>
                                        <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="10%" />
                                        <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="10%" />
                                        <tweb:TSearchBoxColumn Caption="Municipio" FieldName="nome" Width="20%" />
                                    </GridColumns>
                                </tweb:TSearchBox>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:ImageButton ID="btnBuscar" runat="server" SkinID="BcBuscar" OnClick="btnBuscar_Click" />
                </asp:Panel>
            </td>
            <td>
                <asp:Panel runat="server" ID="pnlRatificar" GroupingText="Finalização da Frequência Mensal"
                    Width="400px" Visible="false">
                    <table>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkTermoResponsabilidade" runat="server" Text="Assumo a responsabilidade pela veracidade das informações aqui prestadas, estando ciente de que serão utilizadas como base de cálculo para o pagamento do professor que aderiu à Ampliação da Jornada de Trabalho nesta Unidade de Ensino."
                                    AutoPostBack="true" OnCheckedChanged="chkTermoResponsabilidade_CheckedChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:Button ID="btnFinalizar" runat="server" Text="Ratifico e salvo as informações aqui prestadas"
                        OnClick="btnFinalizar_Click" Enabled="false" />
                </asp:Panel>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hdnFinalizacao" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:PlaceHolder ID="plaTela" runat="server">
                <% if (!string.IsNullOrEmpty(lblMensagemFinalizacao.Text))
                   { %>
                <br />
                <asp:Label ID="lblMensagemFinalizacao" runat="server" SkinID="lblMensagem" EnableViewState="false"></asp:Label>
                <% } %>
                <% if (!string.IsNullOrEmpty(lblMensagem.Text))
                   { %>
                <br />
                <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem" EnableViewState="false"></asp:Label>
                <% } %>
                <br />
                <asp:PlaceHolder ID="plaFrequencia" runat="server" Visible="false">
                    <dxwgv:ASPxGridView ID="grdFrequencia" runat="server" AutoGenerateColumns="False"
                        SkinID="SkinFrequencia" Width="1400px" Visible="true" ClientInstanceName="grdFrequencia"
                        DataSourceID="odsFrequencia" KeyFieldName="NUM_FUNC;UNIDADE_ENS" OnCustomButtonInitialize="grdFrequencia_CustomButtonInitialize">
                        <SettingsBehavior ConfirmDelete="True" />
                        <SettingsPager PageSize="50">
                        </SettingsPager>
                        <SettingsEditing Mode="Inline" />
                        <SettingsText EmptyDataRow="Não existem dados." />
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                        <ClientSideEvents CustomButtonClick="grdFrequencia_OnCustomButtonClick" />
                        <Columns>
                            <dxwgv:GridViewCommandColumn Caption="" ButtonType="Image" Width="50px" VisibleIndex="0">
                                <CustomButtons>
                                    <dxwgv:GridViewCommandColumnCustomButton ID="btnEditar" Text="Editar CH Não Trabalhada"
                                        Image-AlternateText="Editar CH Não Trabalhada" Visibility="Invisible">
                                        <Image Url="~/img/bt_editar.png" />
                                    </dxwgv:GridViewCommandColumnCustomButton>
                                    <dxwgv:GridViewCommandColumnCustomButton ID="btnVisualizar" Text="Detalhes GLP por Turma"
                                        Image-AlternateText="Detalhes GLP por Turma">
                                        <Image Url="~/img/bt_busca.png" />
                                    </dxwgv:GridViewCommandColumnCustomButton>
                                </CustomButtons>
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataColumn Caption="NUM_FUNC" Name="NUM_FUNC" FieldName="NUM_FUNC"
                                Visible="false" />
                            <dxwgv:GridViewDataColumn Caption="ANO_FILTRADO" Name="ANO_FILTRADO" FieldName="ANO_FILTRADO"
                                Visible="false" />
                            <dxwgv:GridViewDataColumn Caption="MES_FILTRADO" Name="MES_FILTRADO" FieldName="MES_FILTRADO"
                                Visible="false" />
                            <dxwgv:GridViewDataColumn Caption="ID_CARGAHNAOTRABMES" Name="ID_CARGAHNAOTRABMES"
                                FieldName="ID_CARGAHNAOTRABMES" Visible="false" ReadOnly="true" />
                            <dxwgv:GridViewDataColumn Caption="Mês" FieldName="MES_EXTENSO" Width="80px" VisibleIndex="1" />
                            <dxwgv:GridViewDataColumn Caption="Regional GLP" FieldName="NOME_REGIONAL" Width="200px"
                                VisibleIndex="3" />
                            <dxwgv:GridViewDataColumn Caption="Muni. GLP" FieldName="NOME_MUNICIPIO" Width="200px"
                                VisibleIndex="5" />
                            <dxwgv:GridViewDataColumn Caption="U. E. GLP" FieldName="NOME_UNIDADE_ENS" Width="200px"
                                VisibleIndex="7" />
                            <dxwgv:GridViewDataColumn Caption="U. A. GLP" FieldName="SETOR_UNIDADE_ENS" Width="70px"
                                VisibleIndex="8" />
                            <dxwgv:GridViewDataColumn Caption="Matrícula" FieldName="MATRICULA" Width="100px"
                                Name="MATRICULA" VisibleIndex="9" />
                            <dxwgv:GridViewDataColumn Caption="ID/Vínculo" FieldName="IDVINCULO" Width="50px"
                                VisibleIndex="10" />
                            <dxwgv:GridViewDataColumn Caption="Nome" FieldName="NOME" Width="150px" VisibleIndex="12"
                                Name="NOME" />
                            <dxwgv:GridViewDataColumn Caption="CH Mensal" FieldName="CH_MENSAL" Width="50px"
                                VisibleIndex="15" />
                            <dxwgv:GridViewDataColumn Caption="CH Não Trabalhada Mês" FieldName="CHNAOTRABALHADAMES"
                                Width="50px" VisibleIndex="16" />
                            <dxwgv:GridViewDataColumn Caption="CH Mensal Final" FieldName="CH_MENSAL_TOTAL" Width="50px"
                                VisibleIndex="17" />
                        </Columns>
                    </dxwgv:ASPxGridView>
                    <br />
                    <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" OnClick="btnImprimir_Click" />
                </asp:PlaceHolder>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="plaRelatorio" runat="server" Visible="false">
                <style>
                    #ReportFramectl00_cphFormulario_rvwFrequenciaGLP
                    {
                        height: 490px !important;
                    }
                </style>
                <rsweb:ReportViewer ID="rvwFrequenciaGLP" runat="server" Width="100%" Height="530px">
                </rsweb:ReportViewer>
                <br />
                <br />
                <asp:Button ID="btnVoltar" runat="server" Text="Voltar" OnClick="btnVoltar_Click" />
            </asp:PlaceHolder>
            <% if (PossuiPerfilAlteracao || EhDiaPermitidoParaAlteracao || EhPrivilegiado)
               { %>
            <dxpc:ASPxPopupControl ID="pucCHNaoTrab" runat="server" Width="300px" Height="200px"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                AllowDragging="false" ClientInstanceName="pucCHNaoTrab" ShowHeader="false" CloseAction="CloseButton">
                <Border BorderColor="Gainsboro" BorderStyle="Ridge" BorderWidth="1px" />
                <ContentCollection>
                    <dxpc:PopupControlContentControl ID="puccCHNaoTrab" runat="server" Visible="true">
                        <asp:HiddenField ID="hidAno" runat="server" />
                        <asp:HiddenField ID="hidMes" runat="server" />
                        <asp:HiddenField ID="hidNumFunc" runat="server" />
                        <asp:HiddenField ID="hidUnidadeEns" runat="server" />
                        <asp:HiddenField ID="hidCHMensal" runat="server" />
                        <asp:HiddenField ID="hidIDCargaHNaoTrabMes" runat="server" />
                        <asp:HiddenField ID="hidMatricula" runat="server" />
                        <asp:HiddenField ID="hidNome" runat="server" />
                        <table>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblNomeMatricula" runat="server" SkinID="lblObrigatorio"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    CH Não Trabalhada Mês:
                                    <asp:TextBox ID="txtCHNaoTrab" runat="server" Width="50px" onchange="alteraCHNaoTrab()"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    CH Mensal:
                                    <asp:Label ID="lblCHMensal" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    CH Mensal Final:
                                    <asp:Label ID="lblCHMensalTotal" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnSalvarCHNaoTrab" runat="server" Text="Salvar" OnClientClick="return salvarPucCHNaoTrab()"
                                        OnClick="btnSalvarCHNaoTrab_Click" />
                                    <asp:Button ID="btnFecharCHNaoTrab" runat="server" Text="Fechar" OnClientClick="return fecharPucCHNaoTrab()" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Label ID="lblMensagemCHNaoTrab" runat="server" SkinID="lblMensagem"></asp:Label>
                    </dxpc:PopupControlContentControl>
                </ContentCollection>
            </dxpc:ASPxPopupControl>
            <% } %>
            <dxpc:ASPxPopupControl ID="pucDetalhe" runat="server" Width="600px" Height="400px"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                AllowDragging="false" ClientInstanceName="pucDetalhe" ShowHeader="false" CloseAction="CloseButton">
                <Border BorderColor="Gainsboro" BorderStyle="Ridge" BorderWidth="1px" />
                <ContentCollection>
                    <dxpc:PopupControlContentControl ID="PopupControlContentControl1" runat="server"
                        Visible="true">
                        <asp:HiddenField ID="hidAno_Detalhe" runat="server" />
                        <asp:HiddenField ID="hidMes_Detalhe" runat="server" />
                        <asp:HiddenField ID="hidFaculdade_Detalhe" runat="server" />
                        <asp:HiddenField ID="hidNumFunc_Detalhe" runat="server" />
                        <dxwgv:ASPxGridView ID="grdTurma" runat="server" AutoGenerateColumns="False" Width="630px"
                            Visible="true" ClientInstanceName="grdTurma" DataSourceID="odsTurma">
                            <SettingsPager PageSize="10">
                            </SettingsPager>
                            <SettingsEditing Mode="Inline" />
                            <SettingsText EmptyDataRow="Não existem dados." />
                            <Settings ShowFilterRow="False" ShowFilterRowMenu="false" />
                            <ClientSideEvents EndCallback="grdTurma_EndCallback" />
                            <Columns>
                                <dxwgv:GridViewDataColumn Caption="Mês" FieldName="MES_EXTENSO" Width="80px" VisibleIndex="1" />
                                <dxwgv:GridViewDataColumn Caption="Turma" FieldName="TURMA" Width="140px" VisibleIndex="2" />
                                <dxwgv:GridViewDataColumn Caption="Disciplina" FieldName="NOME_DISCIPLINA" Width="200px"
                                    VisibleIndex="3" />
                                <dxwgv:GridViewDataColumn Caption="Início" FieldName="DATA_INICIO" Width="70px" VisibleIndex="4" />
                                <dxwgv:GridViewDataColumn Caption="Fim" FieldName="DATA_FIM" Width="70px" VisibleIndex="5" />
                                <dxwgv:GridViewDataColumn Caption="CH Mensal" FieldName="CH_MENSAL" Width="35px"
                                    VisibleIndex="6" />
                            </Columns>
                        </dxwgv:ASPxGridView>
                        <table style="width: 100%;">
                            <tr>
                                <td align="center">
                                    <asp:Button ID="Button2" runat="server" Text="Fechar" OnClientClick="pucDetalhe.Hide(); return false;" />
                                </td>
                            </tr>
                        </table>
                        <asp:ObjectDataSource ID="odsTurma" runat="server" TypeName="Techne.Lyceum.Net.Basico.FrequenciaGLP"
                            SelectMethod="ListaTurma">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hidAno_Detalhe" PropertyName="Value" Name="ano" />
                                <asp:ControlParameter ControlID="hidMes_Detalhe" PropertyName="Value" Name="mes" />
                                <asp:ControlParameter ControlID="hidFaculdade_Detalhe" PropertyName="Value" Name="faculdade" />
                                <asp:ControlParameter ControlID="hidNumFunc_Detalhe" PropertyName="Value" Name="num_func" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </dxpc:PopupControlContentControl>
                </ContentCollection>
            </dxpc:ASPxPopupControl>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnImprimir" />
        </Triggers>
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
    <asp:ObjectDataSource ID="odsAno" runat="server" TypeName="Techne.Lyceum.Net.Basico.FrequenciaGLP"
        SelectMethod="ListaAno" />
    <asp:ObjectDataSource ID="odsMes" runat="server" TypeName="Techne.Lyceum.Net.Basico.FrequenciaGLP"
        SelectMethod="ListaMes" />
    <asp:ObjectDataSource ID="odsFrequencia" runat="server" TypeName="Techne.Lyceum.Net.Basico.FrequenciaGLP"
        SelectMethod="ListaFrequencia">
        <SelectParameters>
            <asp:Parameter Name="ano" />
            <asp:Parameter Name="mes" />
            <asp:Parameter Name="periodo" />
            <asp:Parameter Name="id_regional" />
            <asp:Parameter Name="municipio" />
            <asp:Parameter Name="faculdade" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
