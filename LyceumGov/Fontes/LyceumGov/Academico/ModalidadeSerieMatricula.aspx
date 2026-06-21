<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="ModalidadeSerieMatricula.aspx.cs" Inherits="Techne.Lyceum.Net.Academico.ModalidadeSerieMatricula" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            font-weight: bold;
            text-align: center;
            background-color: #669999;
            width: 107px;
        }
        .style3
        {
            text-align: center;
            width: 132px;
        }
        .style4
        {
            font-weight: bold;
            text-align: center;
            background-color: #669999;
            width: 159px;
        }
        .style5
        {
            text-align: center;
            width: 159px;
        }
        .style6
        {
            font-weight: bold;
            text-align: center;
            background-color: #669999;
            width: 132px;
        }
        .style7
        {
            width: 107px;
        }
    </style>

    <script type="text/javascript" language="javascript">
        $(document).ready(function() {
        $("#<%= this.txtPropostaVagasNovas.ClientID %>").numeric();
        $("#<%= this.txtPropostaVagasContinuidade.ClientID %>").numeric();
        });
    </script>

    <script src="../Scripts/DevExpressProderj.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnGeral" runat="server" GroupingText="Informe os dados para inclusão/Consulta:"
        Width="800px">
        <asp:UpdatePanel ID="updatePanel3" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlAno" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlPeriodo" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="tseUnidadeResponsavel" EventName="Changed" />
                <asp:AsyncPostBackTrigger ControlID="cmbModalidade" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="cmbNivel" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="cmbEscolaridade" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="cmbSerie" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="chkManha" EventName="CheckedChanged" />
                <asp:AsyncPostBackTrigger ControlID="chkTarde" EventName="CheckedChanged" />
                <asp:AsyncPostBackTrigger ControlID="chkNoite" EventName="CheckedChanged" />
                <asp:AsyncPostBackTrigger ControlID="chkAmpliado" EventName="CheckedChanged" />
                <asp:AsyncPostBackTrigger ControlID="chkIntegral" EventName="CheckedChanged" />
                <asp:AsyncPostBackTrigger ControlID="btnSalvar" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                <table>
                    <tr>
                        <td style="text-align: right; width: 15%">
                            <asp:Label ID="lblAno" runat="server" Text="Ano:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlAno" runat="server" AutoPostBack="True" DataTextField="ano"
                                DataValueField="ano" OnSelectedIndexChanged="ddlAno_SelectedIndexChanged" AppendDataBoundItems="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 15%">
                            <asp:Label ID="lblPeriodo" runat="server" Text="Periodo:*" SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPeriodo" runat="server" AutoPostBack="True" DataTextField="periodo"
                                DataValueField="periodo" OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged"
                                AppendDataBoundItems="true">
                            </asp:DropDownList>
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
                                SqlWhere=" situacao = 'ESTADUAL'" GridWidth="850px" OnChanged="tseUnidadeResponsavel_Changed"
                                SqlOrder="nome_comp">
                                <GridColumns>
                                    <tweb:TSearchBoxColumn Caption="Censo" FieldName="unidade_ens" Width="12%" />
                                    <tweb:TSearchBoxColumn Caption="Unidade de Ensino" FieldName="nome_comp" Width="30%" />
                                    <tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="30%" />
                                    <tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="30%" />                                   
                                </GridColumns>
                            </tweb:TSearchBox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvUnidadeResponsavel" runat="server" ControlToValidate="tseUnidadeResponsavel"
                                ErrorMessage="Unidade de Ensino: Preenchimento obrigatório." InitialValue=""
                                ValidationGroup="ConfirmarForm"><img 
                                                alt="Preenchimento obrigatório" src="../Images/AlertaMens.gif" /></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblModalidadeTSearch" runat="server" Text="Modalidade:* " SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="cmbModalidade" runat="server" DataTextField="descricao" DataValueField="modalidade"
                                AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="cmbModalidade_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblNivelTSearch" runat="server" Text="Nível:* " SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="cmbNivel" runat="server" DataTextField="descricao" DataValueField="tipo"
                                AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="cmbNivel_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 120px">
                            <asp:Label ID="lblCursoTSearch" runat="server" Text="Escolaridade:* " SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="cmbEscolaridade" runat="server" AutoPostBack="True" DataTextField="nome"
                                DataValueField="curso" OnSelectedIndexChanged="cmbEscolaridade_SelectedIndexChanged"
                                AppendDataBoundItems="true">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 120px">
                            <asp:Label ID="Label1" runat="server" Text="Serie:* " SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="cmbSerie" runat="server" DataTextField="serie" DataValueField="serie"
                                AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="cmbSerie_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 120px">
                            <asp:Label ID="Label2" runat="server" Text="Proposta de Vagas Novas:* " SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPropostaVagasNovas" runat="server" Width="100px" MaxLength="3"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 120px">
                            <asp:Label ID="Label3" runat="server" Text="Proposta de Vagss de Continuidade:* "
                                SkinID="lblObrigatorio"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPropostaVagasContinuidade" runat="server" Width="100px" MaxLength="3"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 120px">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table border="0" align="center">
                                <tr>
                                    <td class="style1">
                                        Turnos
                                    </td>
                                    <td class="style4">
                                        Continuidade
                                    </td>
                                    <td class="style6">
                                        Nova
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style7">
                                        &nbsp;
                                        <asp:CheckBox ID="chkManha" runat="server" Text="Manhã" OnCheckedChanged="chkManha_CheckedChanged"
                                            AutoPostBack="true" />
                                    </td>
                                    <td class="style5">
                                        &nbsp;
                                        <asp:CheckBox ID="chkManhaVC" runat="server" Enabled="False" />
                                    </td>
                                    <td class="style3">
                                        &nbsp;
                                        <asp:CheckBox ID="chkManhaVN" runat="server" Enabled="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style7">
                                        &nbsp;
                                        <asp:CheckBox ID="chkTarde" runat="server" Text="Tarde" OnCheckedChanged="chkTarde_CheckedChanged"
                                            AutoPostBack="true" />
                                    </td>
                                    <td class="style5">
                                        &nbsp;
                                        <asp:CheckBox ID="chkTardeVC" runat="server" Enabled="False" />
                                    </td>
                                    <td class="style3">
                                        &nbsp;
                                        <asp:CheckBox ID="chkTardeVN" runat="server" Enabled="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style7">
                                        &nbsp;
                                        <asp:CheckBox ID="chkNoite" runat="server" Text="Noite" OnCheckedChanged="chkNoite_CheckedChanged"
                                            AutoPostBack="true" />
                                    </td>
                                    <td class="style5">
                                        &nbsp;
                                        <asp:CheckBox ID="chkNoiteVC" runat="server" Enabled="False" />
                                    </td>
                                    <td class="style3">
                                        &nbsp;
                                        <asp:CheckBox ID="chkNoiteVN" runat="server" Enabled="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style7">
                                        &nbsp;
                                        <asp:CheckBox ID="chkAmpliado" runat="server" Text="Ampliado" OnCheckedChanged="chkAmpliado_CheckedChanged"
                                            AutoPostBack="true" />
                                    </td>
                                    <td class="style5">
                                        &nbsp;
                                        <asp:CheckBox ID="chkAmpliadoVC" runat="server" Enabled="False" />
                                    </td>
                                    <td class="style3">
                                        &nbsp;
                                        <asp:CheckBox ID="chkAmpliadoVN" runat="server" Enabled="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style7">
                                        &nbsp;
                                        <asp:CheckBox ID="chkIntegral" runat="server" Text="Integral" OnCheckedChanged="chkIntegral_CheckedChanged"
                                            AutoPostBack="true" />
                                    </td>
                                    <td class="style5">
                                        &nbsp;
                                        <asp:CheckBox ID="chkIntegralVC" runat="server" Enabled="False" />
                                    </td>
                                    <td class="style3">
                                        &nbsp;
                                        <asp:CheckBox ID="chkIntegralVN" runat="server" Enabled="False" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="3">
                            <asp:Button ID="btnSalvar" runat="server" OnClick="btnSalvar_Click" Text="Salvar"
                                ValidationGroup="SalvarForm" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:Panel ID="pnGrid" runat="server">
                                <dxwgv:ASPxGridView ID="grdModalidade" runat="server" AutoGenerateColumns="False"
                                    Visible="False" ClientInstanceName="grdModalidade" DataSourceID="odsModalidade"
                                    KeyFieldName="ID_CONF_TURNO">
                                    <Columns>
                                        <dxwgv:GridViewDataTextColumn Caption="Código" FieldName="ID_CONF_TURNO" ReadOnly="true"
                                            VisibleIndex="1" Visible="false">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Censo" FieldName="CENSO" ReadOnly="true" VisibleIndex="2"
                                            Visible="false">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Unidade de Ensino" FieldName="ESCOLA" ReadOnly="true"
                                            VisibleIndex="3">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Modalidade" FieldName="MODALIDADE" ReadOnly="true"
                                            Visible="true" VisibleIndex="4">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Escolaridade" FieldName="NOME_CURSO" ReadOnly="true"
                                            Visible="true" VisibleIndex="5">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Série" FieldName="SERIE" ReadOnly="true" VisibleIndex="6"
                                            Width="100">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Turno" FieldName="NOME_TURNO" ReadOnly="true"
                                            VisibleIndex="7" Width="120">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Continuidade" FieldName="CONTINUIDADE" ReadOnly="true"
                                            Width="150" VisibleIndex="8">
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn Caption="Nova" FieldName="NOVO" ReadOnly="true" VisibleIndex="9"
                                            Width="50">
                                        </dxwgv:GridViewDataTextColumn>
                                    </Columns>
                                    <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
                                </dxwgv:ASPxGridView>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <br />
    <asp:ObjectDataSource ID="odsModalidade" TypeName="Techne.Lyceum.Net.Academico.ModalidadeSerieMatricula"
        runat="server" SelectMethod="Listar">
        <SelectParameters>
            <asp:ControlParameter ControlID="tseUnidadeResponsavel" DefaultValue="" Name="unidade_ens"
                PropertyName="DBValue" />
            <asp:ControlParameter ControlID="ddlAno" DefaultValue="" Name="ano" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="ddlPeriodo" DefaultValue="" Name="periodo" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <br />
</asp:Content>
