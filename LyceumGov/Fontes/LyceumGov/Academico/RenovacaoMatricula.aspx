<%@ Page Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="RenovacaoMatricula.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.RenovacaoMatricula"
    Title="Renovação de Matrícula" %>

<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style5
        {
            width: 477px;
            font-weight: 700;
        }
        .style6
        {
            text-align: center;
        }
        .style7
        {
            text-align: center;
            width: 232px;
        }
        .style8
        {
            text-align: center;
            width: 231px;
        }
        .style9
        {
            width: 264px;
        }
    </style>

    <script src="../Scripts/DevExpressProderj.js" type="text/javascript"></script>

    <script type="text/javascript">    
    
    function abrirPopupBloqueado()
    {
     if (!abriu) {
         var elem = document.getElementById('divPopupBloqueado');
         elem.style.display = 'block';
     }
    }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">

    <script type="text/javascript">
        function abrirPopupFinalizar() {
            window.setTimeout(function() {
                pucConfirmar.Show();
            }, 1000);
        }

        function Confirma() {
            var anoPeriodoRenovacaoMatricula = $("#<%=ddlAnoPeriodoRenovacaoMatricula.ClientID %>").val();
            var unidadeEnsinoRenovacaoMatricula = $("#<%=ddlUnidadeEnsinoRenovacaoMatricula.ClientID %>").val();
            var modalidadeRenovacaoMatricula = $("#<%=ddlModalidadeRenovacaoMatricula.ClientID %>").val();
            var serieAluno = $("#<%=ddlSerieAluno.ClientID %>").val();
            var turnoRenovacaoMatricula = $("#<%=ddlTurnoRenovacaoMatricula.ClientID %>").val();

            if (
            (anoPeriodoRenovacaoMatricula != 'Selecione')
            && (unidadeEnsinoRenovacaoMatricula != 'Selecione')
            && (modalidadeRenovacaoMatricula != 'Selecione')
            && (serieAluno != 'Selecione')
            && (turnoRenovacaoMatricula != 'Selecione')
            ) {
                if (confirm("ATENÇÃO!\n\n Confirma a gravação da Renovação de Matricula?")) {
                    return true;
                }
                return false;
            }
            return true;
        }
    </script>

    <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="rbPesquisaAluno" />
            <asp:AsyncPostBackTrigger ControlID="tseAluno" EventName="Changed" />
            <asp:AsyncPostBackTrigger ControlID="rbPesquisaRenovacoes" />
            <asp:AsyncPostBackTrigger ControlID="tseRegional" EventName="Changed" />
            <asp:AsyncPostBackTrigger ControlID="tseUnidadeResponsavel" EventName="Changed" />
            <asp:AsyncPostBackTrigger ControlID="tseEscolaridade" EventName="Changed" />
            <asp:AsyncPostBackTrigger ControlID="ddlAno" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlPeriodo" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlTurno" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlSerie" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnImprimirRenovacao" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="hplFicha" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="grdAluno" EventName="HtmlRowCreated" />
            <asp:AsyncPostBackTrigger ControlID="grdRenovacoes" EventName="HtmlRowCreated" />
            <asp:AsyncPostBackTrigger ControlID="ddlAnoPeriodoRenovacaoMatricula" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlUnidadeEnsinoRenovacaoMatricula" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlModalidadeRenovacaoMatricula" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlTurnoRenovacaoMatricula" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnIncluir" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnCancelarRenovacao" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="pnlPesquisa" runat="server" Visible="true" Width="80%">
                <fieldset>
                    <legend>Escolha o tipo de busca que deseja</legend>
                    <table>
                        <tr>
                            <td style="width: 120px;">
                                <asp:RadioButton ID="rbPesquisaAluno" runat="server" AutoPostBack="true" GroupName="Busca"
                                    Checked="true" OnCheckedChanged="rbPesquisaAluno_Changed" />
                            </td>
                            <td style="text-align: right; width: 15%">
                                <asp:Label ID="lblMatriculaTSearch" runat="server" Text="Aluno: " SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <tweb:TSearch ID="tseAluno" runat="server" ValidateText="true" SettingsTypeName="Techne.Lyceum.RN.Query.QueryAluno"
                                    AutoPostBack="true" MaxLength="20" ArgumentColumns="50" Columns="10" GridWidth="850px"
                                    OnTextChanged="tseAluno_Changed">
                                    <QueryParameters>
                                        <asp:Parameter Name="ativo" DefaultValue="Ativo" DbType="String" />
                                    </QueryParameters>
                                </tweb:TSearch>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <asp:HiddenField ID="hdnAluno" runat="server" />
                    <hr style="width: 70%; float: left;" />
                    <br />
                    <table>
                        <tr>
                            <td style="width: 120px;">
                                <asp:RadioButton ID="rbPesquisaRenovacoes" runat="server" AutoPostBack="true" Checked="false"
                                    OnCheckedChanged="rbPesquisaRenovacoes_Changed" GroupName="Busca" />
                            </td>
                            <td style="text-align: right; width: 15%">
                                <asp:Label ID="lblRegional" runat="server" Font-Names="Verdana" Text="Regional:"></asp:Label>
                            </td>
                            <td>
                                <tweb:TSearchBox ID="tseRegional" runat="server" Argument="regional" ArgumentColumns="50"
                                    MaxLength="20" Columns="10" AutoPostBack="True" Caption="" Key="id_regional"
                                    SqlSelect="SELECT DISTINCT re.id_regional, re.regional FROM tce_regional re (nolock)"
                                    OnChanged="tseRegional_Changed" DataType="Number">
                                    <GridColumns>
                                        <tweb:TSearchBoxColumn Caption="Código" FieldName="id_regional" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="80%" />
                                    </GridColumns>
                                </tweb:TSearchBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td style="text-align: right; width: 15%">
                                <asp:Label Font-Names="Verdana" ID="lblUnidadeResponsavel" SkinID="lblObrigatorio"
                                    runat="server" Text="Unidade de Ensino:*"></asp:Label>
                            </td>
                            <td>
                                <tweb:TSearchBox ID="tseUnidadeResponsavel" runat="server" Caption="" Key="unidade_ens"
                                    MaxLength="20" ArgumentColumns="50" Columns="10" Argument="nome_comp" GridWidth="850px"
                                    OnChanged="tseUnidadeResponsavel_Changed" SqlSelect=" SELECT DISTINCT unidade_ens, nome_comp, setor, cgc, situacao, nucleo, municipio, id_regional from VW_UNIDADESESTADUAISCOMRENOVACAO ">
                                    <GridColumns>
                                        <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="12%" />
                                        <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                                        <tweb:TSearchBoxColumn Caption="U.A." FieldName="setor" Width="30%" />
                                        <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="15%" />
                                    </GridColumns>
                                </tweb:TSearchBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td style="text-align: right; width: 15%">
                                <asp:Label ID="lblEscolaridade" runat="server" Font-Names="Verdana" Text="Escolaridade:*"
                                    SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <tweb:TSearchBox ID="tseEscolaridade" runat="server" Argument="descricao" ArgumentColumns="50"
                                    SqlOrder="descricao" MaxLength="20" Columns="10" AutoPostBack="True" SqlWhere=" UNIDADEENSINOID = isnull(#tseUnidadeResponsavel#,'')"
                                    Caption="" Key="codigo" OnChanged="tseEscolaridade_Changed" SqlSelect=" SELECT DISTINCT cs.Nome AS descricao, cs.Curso AS codigo FROM dbo.RENOVACAO r INNER JOIN LY_CURSO cs ON r.CURSOID = cs.Curso ">
                                    <GridColumns>
                                        <tweb:TSearchBoxColumn Caption="Código" FieldName="codigo" Width="20%" />
                                        <tweb:TSearchBoxColumn Caption="Descrição" FieldName="descricao" Width="80%" />
                                    </GridColumns>
                                </tweb:TSearchBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td style="text-align: right; width: 15%">
                                <asp:Label ID="lblAno" runat="server" Font-Names="Verdana" Font-Size="Smaller" Text="Ano:*"
                                    SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlAno" runat="server" AutoPostBack="True" DataTextField="ano"
                                    Enabled="false" DataValueField="ano" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged"
                                    AppendDataBoundItems="true">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td style="text-align: right; width: 15%">
                                <asp:Label ID="lblPeriodo" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                    Text="Período:*" SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlPeriodo" runat="server" AppendDataBoundItems="true" AutoPostBack="True"
                                    Enabled="false" DataTextField="periodo" DataValueField="periodo" OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td style="text-align: right; width: 15%">
                                <asp:Label ID="lblTurno" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                    SkinID="lblObrigatorio" Text="Turno:*"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTurno" runat="server" AutoPostBack="true" DataTextField="descricao"
                                    Enabled="false" DataValueField="turno" OnSelectedIndexChanged="ddlTurno_SelectedIndexChanged"
                                    Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td style="text-align: right; width: 15%">
                                <asp:Label ID="lblSerie" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                    SkinID="lblObrigatorio" Text="Série/Ano Escolar:*"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSerie" runat="server" DataTextField="SERIE" DataValueField="SERIE"
                                    Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="ddlSerie_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <p>
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" /></p>
                </fieldset>
                <br />
            </asp:Panel>
            <p style="color: Red;">
                <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem" /></p>
            <br />
            <asp:Panel ID="pnlAluno" runat="server" Visible="false" Width="80%">
                <fieldset>
                    <legend>Dados Gerais</legend>
                    <table>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblAnoRenovacaoMatricula" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                Text="Ano/Período:*" SkinID="lblObrigatorio"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblAnoPeriodoRenovacaoMatricula" runat="server" Visible="false"></asp:Label>
                                            <asp:DropDownList ID="ddlAnoPeriodoRenovacaoMatricula" runat="server" AutoPostBack="true"
                                                DataTextField="ano" Enabled="false" DataValueField="ano" AppendDataBoundItems="true"
                                                OnSelectedIndexChanged="ddlAnoPeriodoRenovacaoMatricula_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblUniEnsinoRenovacaoMatricula" runat="server" Font-Names="Verdana"
                                                Font-Size="Smaller" SkinID="lblObrigatorio" Text="Unidade de Ensino:*"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlUnidadeEnsinoRenovacaoMatricula" runat="server" AutoPostBack="true"
                                                Enabled="false" DataTextField="UnidadeEnsinoNome" DataValueField="UnidadeEnsino"
                                                Width="500px" OnSelectedIndexChanged="ddlUnidadeEnsinoRenovacaoMatricula_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblModalidadeRenovacaoMatricula" runat="server" SkinID="lblObrigatorio"
                                                Text="Modalidade/Segmento/Curso*: "></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlModalidadeRenovacaoMatricula" runat="server" DataTextField="ModalidadeSegmentoCurso"
                                                DataValueField="Curso" Width="500px" Enabled="false" OnSelectedIndexChanged="ddlModalidadeRenovacaoMatricula_SelectedIndexChanged"
                                                AutoPostBack="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblSerieRenovacaoMatricula" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                SkinID="lblObrigatorio" Text="Série/Ano Escolar:*"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlSerieAluno" runat="server" DataTextField="SerieSeguinte"
                                                DataValueField="SerieSeguinte" Enabled="false" OnSelectedIndexChanged="ddlSerieAluno_SelectedIndexChanged"
                                                AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblTurnoRenovacaoMatricula" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                                SkinID="lblObrigatorio" Text="Turno:*"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlTurnoRenovacaoMatricula" runat="server" AutoPostBack="true"
                                                DataTextField="TurnoNome" Enabled="false" DataValueField="Turno" OnSelectedIndexChanged="ddlTurnoRenovacaoMatricula_SelectedIndexChanged"
                                                Width="200px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkEnsReligioso" runat="server" Text="Ensino Religioso" Width="140px"
                                                Enabled="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkLinguaEstrangeira" runat="server" Text="Língua Estrangeira Facultativa"
                                                Enabled="false" Width="177px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <fieldset style="border: 2px solid">
                                    <strong>As opções apresentadas para renovação do aluno são apenas as possíveis de acordo
                                        com as regras abaixo:<br />
                                        - Agenda de Eventos de Renovação<br />
                                        - Implantação do Curso/Modalidade/Série na Unidade de Ensino<br />
                                        - Absorção de Unidades<br />
                                        - Matriz Curricular (progressão de série e opções de optativa)<br />
                                        - Progressão de Curso e Série<br />
                                        - Restrição Idade/Série e Necessidade Especial do Aluno<br />
                                        - Confirmação de Turnos<br />
                                        - Restrição / Terminalidade</strong>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                    <p>
                        <asp:Button ID="btnIncluir" runat="server" Text="Incluir" OnClick="btnIncluir_Click" /></p>
                    <dxwgv:ASPxGridView ID="grdAluno" runat="server" AutoGenerateColumns="False" Visible="true"
                        ClientInstanceName="grdAluno" KeyFieldName="ID" EnableCallBacks="true" OnAfterPerformCallback="grdAluno_AfterPerformCallback">
                        <SettingsPager PageSize="10" />
                        <SettingsBehavior ConfirmDelete="True" />
                        <SettingsEditing Mode="Inline" />
                        <SettingsText ConfirmDelete="Confirma a remoção?" EmptyDataRow="Não existem dados." />
                        <Columns>
                            <dxwgv:GridViewDataTextColumn Caption="ID" FieldName="ID" ReadOnly="true" VisibleIndex="0"
                                Visible="false">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="ALUNO" ReadOnly="true"
                                VisibleIndex="1" Visible="true">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NOMEALUNO" ReadOnly="true"
                                Visible="true" VisibleIndex="2">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ANO" ReadOnly="true" VisibleIndex="3">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Período" FieldName="PERIODO" ReadOnly="true"
                                VisibleIndex="3">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="CENSO" FieldName="CENSO" ReadOnly="true" VisibleIndex="4">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Nome da Escola" FieldName="NOME_COMP" ReadOnly="true"
                                Visible="true" VisibleIndex="5">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Modalidade/Segmento/Curso" FieldName="MOD_SEG_CURSO"
                                ReadOnly="true" Visible="true" VisibleIndex="6">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="TURNO" ReadOnly="true" Visible="true"
                                VisibleIndex="7">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Série" FieldName="SERIE" ReadOnly="true" Visible="true"
                                VisibleIndex="8">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Ensino Religioso" FieldName="ENS_RELIGIOSO"
                                ReadOnly="true" Visible="true" VisibleIndex="9">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Língua Estrangeira" FieldName="LINGUA_ESTRANGEIRA"
                                ReadOnly="true" Visible="true" VisibleIndex="10">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="SITUACAO" ReadOnly="true"
                                Visible="true" VisibleIndex="11">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Usuário" FieldName="USUARIO" ReadOnly="true"
                                Visible="true" VisibleIndex="12">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Data/Hora" FieldName="DATA_HORA" ReadOnly="true"
                                Visible="true" VisibleIndex="13">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                    </dxwgv:ASPxGridView>
                </fieldset>
                <br />
                <asp:Panel ID="pnImprimir" GroupingText="Imprimir Ficha de Renovação" runat="server">
                    <table width="100%">
                        <tr>
                            <td style="text-align: right">
                                <asp:Label Font-Names="Verdana" Font-Size="Smaller" ID="Label16" runat="server" Text="Renovação de Matrícula:*"
                                    SkinID="lblObrigatorio"></asp:Label>
                            </td>
                            <td style="width: auto">
                                <dxe:ASPxComboBox ID="ddlRenovacaoMatricula" runat="server" DataSourceID="odsImprimirRenovacao"
                                    ValueField="RENOVACAOID" AutoPostBack="true" TextFormatString="{0} - {1}|{2} - {3}"
                                    DropDownWidth="700px" Width="480px" Height="5px" ClientInstanceName="ddlRenovacaoMatricula">
                                    <Columns>
                                        <dxe:ListBoxColumn Caption="Código" FieldName="RENOVACAOID" Width="15%" />
                                        <dxe:ListBoxColumn Caption="Ano" FieldName="ANO" Width="10%" />
                                        <dxe:ListBoxColumn Caption="Periodo" FieldName="PERIODO" Width="10%" />
                                        <dxe:ListBoxColumn Caption="Modalidade/Segmento/Curso" FieldName="MOD_SEG_CURSO"
                                            Width="60%" />
                                        <dxe:ListBoxColumn Caption="Unidade de Ensino" FieldName="UNIDADE_ENSINO" Width="60%" />
                                        <dxe:ListBoxColumn Caption="Série" FieldName="SERIE" Width="10%" />
                                        <dxe:ListBoxColumn Caption="Turno" FieldName="NOME_TURNO" Width="20%" />
                                    </Columns>
                                </dxe:ASPxComboBox>
                            </td>
                            <td>
                                <asp:Button ID="btnImprimirRenovacao" runat="server" OnClick="btnImprimirRenovacao_Click"
                                    Text="Imprimir Renovação" />
                            </td>
                            </tr>
                            <tr>
                            <td colspan="3">
                                <div style="display:none;font-family:arial;color:red;font-size:14px;font-weight:bold;" id="divPopupBloqueado">A janela popup de impressão foi bloqueada pelo navegador, para abri-la 
                                    <asp:LinkButton ID="hplFicha" Font-Size="14px" Font-Bold="true" OnClientClick="javascript: return abrir();" runat="server">clique aqui</asp:LinkButton>
                                </div>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:ObjectDataSource ID="odsImprimirRenovacao" runat="server" TypeName="Techne.Lyceum.RN.RenovacaoMatricula.Renovacao"
                    SelectMethod="ListaRenovacoesMatriculaAtivaPor">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="tseAluno" DefaultValue="" Name="aluno" PropertyName="Value" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </asp:Panel>
            <asp:Panel ID="pnlRenovacoes" runat="server" Visible="false" Width="80%">
                <fieldset>
                    <legend>Dados Gerais</legend>
                    <dxwgv:ASPxGridView ID="grdRenovacoes" runat="server" AutoGenerateColumns="False"
                        Visible="true" ClientInstanceName="grdRenovacoes" KeyFieldName="RENOVACAOID"
                        EnableCallBacks="true" OnAfterPerformCallback="grdRenovacoes_AfterPerformCallback">
                        <SettingsPager PageSize="10" />
                        <ClientSideEvents EndCallback="function(s, e) { OnEndCallBack(s); }" />
                        <Columns>
                            <dxwgv:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" ButtonType="Image">
                                <HeaderTemplate>
                                    <input id="chkRenovacao" runat="server" type="checkbox" onclick="grdRenovacoes.SelectAllRowsOnPage(this.checked);"
                                        title="Select/Unselect all rows on the page" />
                                </HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="ALUNO" ReadOnly="true"
                                VisibleIndex="2" Visible="true">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="NOMEALUNO" ReadOnly="true"
                                Visible="true" VisibleIndex="3">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Ano" FieldName="ANO" ReadOnly="true" VisibleIndex="4">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Período" FieldName="PERIODO" ReadOnly="true"
                                VisibleIndex="4">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="CENSO" FieldName="CENSO" ReadOnly="true" VisibleIndex="5">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Nome da Escola" FieldName="NOME_COMP" ReadOnly="true"
                                Visible="true" VisibleIndex="6">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Modalidade / Segmento / Curso" FieldName="MOD_SEG_CURSO"
                                VisibleIndex="7">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="TURNO" ReadOnly="true" Visible="true"
                                VisibleIndex="8">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Série" FieldName="SERIE" ReadOnly="true" Visible="true"
                                VisibleIndex="9">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataCheckColumn Caption="Ensino Religioso" FieldName="ENS_RELIGIOSO"
                                Name="ENS_RELIGIOSO" VisibleIndex="10">
                                <DataItemTemplate>
                                    <asp:CheckBox ID="chkEnsinoReligioso" runat="server" Checked='<%# this.VerificaCheck(Eval("ENS_RELIGIOSO")) %>'
                                        Enabled="false" />
                                </DataItemTemplate>
                            </dxwgv:GridViewDataCheckColumn>
                            <dxwgv:GridViewDataCheckColumn Caption="Língua Estrangeira Facultativa" FieldName="LINGUA_ESTRANGEIRA"
                                Name="LINGUA_ESTRANGEIRA" VisibleIndex="11">
                                <DataItemTemplate>
                                    <asp:CheckBox ID="chkLinguaEstrangeira" runat="server" Checked='<%# this.VerificaCheck(Eval("LINGUA_ESTRANGEIRA")) %>'
                                        Enabled="false" />
                                </DataItemTemplate>
                            </dxwgv:GridViewDataCheckColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Situação" FieldName="SITUACAO" ReadOnly="true"
                                Visible="true" VisibleIndex="12">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Usuário" FieldName="USUARIO" ReadOnly="true"
                                Visible="true" VisibleIndex="13">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn Caption="Data/Hora" FieldName="DATA_HORA" ReadOnly="true"
                                Visible="true" VisibleIndex="14">
                                <PropertiesTextEdit>
                                    <ReadOnlyStyle>
                                        <Border BorderStyle="None"></Border>
                                    </ReadOnlyStyle>
                                </PropertiesTextEdit>
                                <HeaderStyle Font-Bold="True"></HeaderStyle>
                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                    </dxwgv:ASPxGridView>
                    <p>
                        <asp:Button ID="btnCancelarRenovacao" runat="server" Text="Cancelar" OnClick="btnCancelarRenovacao_Click"
                            OnClientClick="return window.confirm('ATENÇÃO!\n\n Confirma o cancelamento das Renovações de Matrículas?')" /></p>
                </fieldset>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- ODS 
    -->
    <dxpc:ASPxPopupControl ID="pucConfirmar" ClientInstanceName="pucConfirmar" runat="server"
        Modal="true" ShowShadow="false" AllowDragging="false" AllowResize="false" ShowCloseButton="true"
        ShowFooter="false" ShowSizeGrip="False" EnableAnimation="true" Width="100%" CloseAction="None"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
        CssPostfix="Aqua" ImageFolder="~/App_Themes/Aqua/{0}/" HeaderText="Incluir Renovação de Matrícula">
        <HeaderStyle HorizontalAlign="Center" />
        <Border BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="2px" />
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <ModalBackgroundStyle BackColor="White" Opacity="0" />
        <SizeGripImage Height="12px" Width="12px" />
                <ClientSideEvents Init="function(s,e){ OnInitASPxPopupControlSize(s,e,12000); }" />
        <ContentCollection>
            <dxpc:PopupControlContentControl>
                <asp:UpdatePanel ID="updatePanel9" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnConfirmar" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancelar" EventName="Click" />
                    </Triggers>
                    <ContentTemplate>
                        <asp:Label ID="lblPergunta" runat="server" Text="Confirma a gravação da renovação de matricula?" />
                        <br />
                        <table id="Table1" runat="server">
                            <tr>
                                <td style="text-align: right;" class="style5">
                                    <asp:Button ID="btnConfirmar" runat="server" Text="Confirmar" OnClick="btnConfirmar_Click"
                                        OnClientClick="pucConfirmar.Hide(); return true;" />
                                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClientClick="pucConfirmar.Hide(); return false;" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </dxpc:PopupControlContentControl>
        </ContentCollection>
    </dxpc:ASPxPopupControl>
    <%--<asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <asp:Panel ID="Panel1" CssClass="overlay" runat="server">
                <asp:Panel ID="Panel2" CssClass="loader" runat="server">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/updateProgress.gif" AlternateText="Updating..."
                        Height="48" Width="48" />
                </asp:Panel>
            </asp:Panel>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>
</asp:Content>
