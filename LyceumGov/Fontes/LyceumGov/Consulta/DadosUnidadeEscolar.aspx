<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/LyceumMaster.Master" AutoEventWireup="true"
    CodeBehind="DadosUnidadeEscolar.aspx.cs" Inherits="Techne.Lyceum.Net.Consulta.DadosUnidadeEscolar" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v9.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v9.2" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 127px;
        }
        .style2
        {
            width: 128px;
        }
        .style3
        {
            width: 256px;
        }
        .style4
        {
            width: 83px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphFormulario" runat="server">
    <asp:Panel ID="pnBusca" runat="server" GroupingText="Faça uma busca por unidade"
        Width="617px">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblUnidade" runat="server" Text="Unidade: " SkinID="lblObrigatorio"></asp:Label>
                </td>
                <td>
                    <tweb:TSearchBox ID="tseUnidade" runat="server" Key="unidade_ens" Argument="nome_comp"
                        OnChanged="tseUnidade_Changed" MaxLength="8" SqlSelect="SELECT unidade_ens, nome_comp, setor, cgc, situacao,municipio,nome, regional, ua_atual,ua_antiga  from VW_UNIDADE_ENSINO_SITUACAO_REGIONAL"
                        GridWidth="850px" SqlOrder="nome_comp" Connection="Lyceum">
                        <GridColumns>
                            <tweb:TSearchBoxColumn Caption="Regional" FieldName="regional" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Município" FieldName="nome" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Código" FieldName="unidade_ens" Width="10%" />
                            <tweb:TSearchBoxColumn Caption="Descrição" FieldName="nome_comp" Width="20%" />                            
							<tweb:TSearchBoxColumn Caption="U.A." FieldName="ua_atual" Width="10%" />
							<tweb:TSearchBoxColumn Caption="U.A. Antiga" FieldName="ua_antiga" Width="8%" />
                            <tweb:TSearchBoxColumn Caption="CNPJ" FieldName="cgc" Width="13%" />
                            <tweb:TSearchBoxColumn Caption="Situação" FieldName="situacao" Width="18%" />
                        </GridColumns>
                    </tweb:TSearchBox>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <div class="divEditBlock" style="width: 950px;">
        <asp:Label runat="server" ID="lblBlocoUnidade" Text="Unidade" SkinID="BcTitulo" />
        <asp:ValidationSummary ID="vsUnidade" runat="server" EnableClientScript="true" ShowMessageBox="true"
            ValidationGroup="SalvarForm" ShowSummary="false" />
    </div>
    <asp:Label ID="lblMensagem" runat="server" SkinID="lblMensagem"></asp:Label>
    <br />
    <asp:Panel ID="pnlGeral" runat="server" Width="950px">
        <asp:Panel ID="Panel5" runat="server" GroupingText="Dados Gerais" Width="950px">
            <table style="width: 100%">
                <tr>
                    <td style="text-align: right" class="style2">
                        <asp:Label ID="Label2" runat="server" Text="Código do Censo:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblCenso" runat="server"></asp:Label>
                    </td>
                    <td style="text-align: right">
                        <asp:Label ID="Label1" runat="server" Text="Situação de funcionamento:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td class="style3">
                        <asp:Label ID="lblSituacaoFuncionamento" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right" class="style2">
                        <asp:Label ID="Label4" runat="server" Text="Nome da Unidade Ensino:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblNomeUnidade" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right" class="style2">
                        <asp:Label ID="Label3" runat="server" Text="Diretoria Regional:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblDiretoriaRegional" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right" class="style2">
                        <asp:Label ID="Label5" runat="server" Text="Coordenadoria:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblNucleo" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right" class="style2">
                        <asp:Label ID="Label6" runat="server" Text="U.A.:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblSetor" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right" class="style2">
                        <asp:Label ID="Label7" runat="server" Text="Dependência Administrativa:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblDependenciaAdministrativa" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right" class="style2">
                        <asp:Label ID="Label8" runat="server" Text="Email:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblEmail" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right" class="style2">
                        <asp:Label ID="Label9" runat="server" Text="CNPJ:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblCGC" runat="server"></asp:Label>
                    </td>
                    <td style="text-align: right">
                        <asp:Label ID="Label11" runat="server" Text="Classificação:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td class="style3">
                        <asp:Label ID="lblClassificacao" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right" class="style2">
                        <asp:Label ID="Label12" runat="server" Text="Telefone 1:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblFone" runat="server" T></asp:Label>
                    </td>
                    <td style="text-align: right;">
                        <asp:Label ID="Label13" runat="server" Text="Telefone 2:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td class="style3">
                        <asp:Label ID="lblFone2" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="Label10" runat="server" Text="Fax:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblFax" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right" class="style2">
                        <asp:Label ID="lblFormaOcup" runat="server" Text="Localização da U.E.: " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblLocalizacaoUF" runat="server"></asp:Label>
                    </td>
                    <td style="text-align: right">
                        <asp:Label ID="Label100" runat="server" Text="Local de Funcionamento: " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td class="style3">
                        <asp:Label ID="lblLocalFuncionamento" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right" class="style2">
                        <asp:Label ID="lblLocalizacaoDiferenciada" runat="server" Text="Localização Diferenciada:"
                            SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <table>
                            <tr>
                                <td>
                                    <dxe:ASPxCheckBox ID="chkAreaAssentamento" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                        runat="server" Checked="false" Text="Área de assentamento" Enabled="false">
                                    </dxe:ASPxCheckBox>
                                    <dxe:ASPxCheckBox ID="chkTerraIndigena" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                        runat="server" Checked="false" Text="Terra indígena" Enabled="false">
                                    </dxe:ASPxCheckBox>
                                </td>
                                <td>
                                    <dxe:ASPxCheckBox ID="chkQuilombos" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                        runat="server" Checked="false" Text="Área remanescente de quilombos " Enabled="false">
                                    </dxe:ASPxCheckBox>
                                    <dxe:ASPxCheckBox ID="chkSustentavel" ValueChecked="S" ValueUnchecked="N" ValueType="System.String"
                                        runat="server" Checked="false" Text="Unidade de uso sustentável" Enabled="false">
                                    </dxe:ASPxCheckBox>
                                </td>
                                <td>
                                    <dxe:ASPxCheckBox AutoPostBack="true" ID="chkNaoSeAplica" ValueChecked="S" ValueUnchecked="N"
                                        ValueType="System.String" runat="server" Checked="true" Text="Não se aplica"
                                        Enabled="false">
                                    </dxe:ASPxCheckBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:Panel ID="pnlEnderecoUF" runat="server" GroupingText="Endereço" Width="950px">
            <table width="100%">
                <tr>
                    <td style="text-align: right" class="style1">
                        <asp:Label ID="Label14" runat="server" Text="CEP:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblCEPUF" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right" class="style1">
                        <asp:Label ID="Label15" runat="server" Text="Município:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblMunicipioUF" runat="server"></asp:Label>
                    </td>
                    <td style="text-align: right" class="style4">
                        <asp:Label ID="Label16" runat="server" Text="UF:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblEstadoUF" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right" class="style1">
                        <asp:Label ID="Label17" runat="server" Text="Endereço:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblLogradouroUF" runat="server"></asp:Label>
                    </td>
                    <td style="text-align: right" class="style4">
                        <asp:Label ID="Label18" runat="server" Text="Número:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblNumeroEndUF" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right" class="style1">
                        <asp:Label ID="Label19" runat="server" Text="Complemento:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblComplementoUF" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right" class="style1">
                        <asp:Label ID="Label20" runat="server" Text="Bairro:" SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:Label ID="lblBairro" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right" class="style1">
                        <asp:Label ID="Label21" runat="server" Text="Distrito: " SkinID="lblObrigatorio"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblDistritoUF" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:ObjectDataSource ID="odsEquipe" runat="server" TypeName="Techne.Lyceum.RN.UnidadeEnsino"
            SelectMethod="ConsultarGratificada">
            <SelectParameters>
                <asp:ControlParameter ControlID="lblCenso" Name="unidadeEns" PropertyName="Text" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <dxwgv:ASPxGridView ID="grdEquipe" DataSourceID="odsEquipe" runat="server" AutoGenerateColumns="False"
            ClientInstanceName="grdEquipe" EnableCallBacks="false" Font-Size="Small" Width="942px"
            OnCustomColumnDisplayText="grdEquipe_CustomColumnDisplayText">
            <SettingsBehavior AllowMultiSelection="False" AllowSort="False" />
            <SettingsPager PageSize="10" />
            <Columns>
                <dxwgv:GridViewDataTextColumn Caption="Matrícula" FieldName="matricula" VisibleIndex="1">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Nome" FieldName="nome_compl" VisibleIndex="2">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Função" FieldName="descricao" VisibleIndex="3">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Telefone" FieldName="fone" VisibleIndex="4">
                    <PropertiesTextEdit MaxLength="10">
                        <MaskSettings IncludeLiterals="None" Mask="(00)0000-0000" ErrorText="* Favor inserir um número de telefone válido." />
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="Celular" FieldName="celular" VisibleIndex="5">
                    <PropertiesTextEdit MaxLength="10">
                        <MaskSettings IncludeLiterals="All" Mask="(00)0000-0000" ErrorText="* Favor inserir um número de telefone válido." />
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn Caption="E-mail" FieldName="e_mail" VisibleIndex="6">
                </dxwgv:GridViewDataTextColumn>
            </Columns>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" />
        </dxwgv:ASPxGridView>
    </asp:Panel>
</asp:Content>
